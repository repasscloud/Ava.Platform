#!/usr/bin/env bash

# =============================================================================
# Professional Deployment Script
# =============================================================================
# This script performs the following operations:
# 1. Sets up environment variables and paths.
# 2. Manages the Docker network "common_net".
# 3. Resets the PostgreSQL database "avaprod".
# 4. Applies EF Core migrations.
# 5. Manages Docker Compose for both the API and web app.
# 6. Injects region data via API.
# 7. Executes additional PowerShell scripts for data import and processing.
# 8. Sets up the company profile and client configuration.
#
# Enjoy the colorful output and clear progress feedback!
# =============================================================================

# ANSI color definitions
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[0;33m'
BLUE='\033[0;34m'
NC='\033[0m'  # No Color

echo -e "${BLUE}========== Starting Deployment Script ==========${NC}"

# -----------------------------------------------------------------------------
# Parameters
# -----------------------------------------------------------------------------
PROJECT_BASE_PATH="/Users/danijeljw/Developer/dotnet-dev/Ava.dev/Ava.API"
APP_BASE_PATH="/Users/danijeljw/Developer/dotnet-dev/Ava.dev/Ava.WebApp"
export PGPASSWORD="devpassword"
export COMPOSE_BAKE=true
API_URL="http://localhost:5165"
regions=("APAC" "NA" "EMEA" "LATAM")

echo -e "${GREEN}Project Base Path:${NC} $PROJECT_BASE_PATH"
echo -e "${GREEN}App Base Path:${NC} $APP_BASE_PATH"
echo -e "${GREEN}API URL:${NC} $API_URL"

# -----------------------------------------------------------------------------
# Docker Network: common_net
# -----------------------------------------------------------------------------
echo -e "${BLUE}Checking for Docker network 'common_net'...${NC}"
if docker network inspect common_net >/dev/null 2>&1; then
  echo -e "${YELLOW}Network 'common_net' exists. Removing it...${NC}"
  docker network rm common_net
else
  echo -e "${YELLOW}Network 'common_net' does not exist. Will create it now...${NC}"
fi

echo -e "${BLUE}Creating Docker network 'common_net'...${NC}"
docker network create common_net
echo -e "${GREEN}Network 'common_net' re-created successfully.${NC}"

# -----------------------------------------------------------------------------
# Reset Migrations and Database
# -----------------------------------------------------------------------------
echo -e "${BLUE}Removing Migrations folder...${NC}"
rm -rf "$APP_BASE_PATH/Migrations"
echo -e "${GREEN}Migrations folder removed.${NC}"

echo -e "${BLUE}Dropping and re-creating PostgreSQL database 'avaprod'...${NC}"
psql -h 170.64.135.239 -p 5432 -d postgres -U postgres -f "$PROJECT_BASE_PATH/_help/sql_files/drop_avaprod_db.sql"
psql -h 170.64.135.239 -p 5432 -d postgres -U postgres -f "$PROJECT_BASE_PATH/_help/sql_files/create_avaprod_db.sql"
echo -e "${GREEN}Database 'avaprod' reset successfully.${NC}"

# -----------------------------------------------------------------------------
# EF Core Migrations
# -----------------------------------------------------------------------------
echo -e "${BLUE}Creating EF Core migration 'initDb'...${NC}"
dotnet ef migrations add initDb --project "$APP_BASE_PATH" --startup-project "$APP_BASE_PATH"
echo -e "${GREEN}Migration 'initDb' created successfully.${NC}"

echo -e "${BLUE}Updating database with EF Core migrations...${NC}"
dotnet ef database update --verbose --project "$APP_BASE_PATH" --startup-project "$APP_BASE_PATH" > output.txt 2>&1
echo -e "${GREEN}Database updated successfully. Check 'output.txt' for details.${NC}"

# -----------------------------------------------------------------------------
# Docker Compose: Stop & Start Services
# -----------------------------------------------------------------------------
echo -e "${BLUE}Stopping Web API using Docker Compose...${NC}"
docker compose -f "$PROJECT_BASE_PATH/compose.yaml" down
echo -e "${GREEN}Web API stopped.${NC}"

echo -e "${BLUE}Stopping Web App using Docker Compose...${NC}"
docker compose -f "$APP_BASE_PATH/compose.yaml" down
echo -e "${GREEN}Web App stopped.${NC}"

echo -e "${BLUE}Starting Web API using Docker Compose...${NC}"
docker compose -f "$PROJECT_BASE_PATH/compose.yaml" up --build --force-recreate -d
echo -e "${GREEN}Web API started successfully.${NC}"

sleep 20

# -----------------------------------------------------------------------------
# Inject Region Data via API
# -----------------------------------------------------------------------------
echo -e "${BLUE}Injecting region data via API...${NC}"
for region in "${regions[@]}"; do
  curl -X POST "$API_URL/api/Region" \
    -H "accept: */*" \
    -H "Content-Type: application/json" \
    -d "{\"name\": \"$region\"}"
  echo -e "${GREEN}Posted region: $region${NC}"
done

# -----------------------------------------------------------------------------
# Additional Data Import via PowerShell Scripts
# -----------------------------------------------------------------------------
echo -e "${BLUE}Importing continent data via PowerShell...${NC}"
pwsh -File "$PROJECT_BASE_PATH/_help/pwsh_files/02_import_continent_data.ps1"
echo -e "${GREEN}Continent data imported.${NC}"

echo -e "${BLUE}Importing country data via PowerShell...${NC}"
pwsh -File "$PROJECT_BASE_PATH/_help/pwsh_files/03a_import_country_data.ps1"
pwsh -File "$PROJECT_BASE_PATH/_help/pwsh_files/03b_import_country_data_patched.ps1"
echo -e "${GREEN}Country data imported.${NC}"

echo -e "${BLUE}Generating IATA codes from CSV via PowerShell...${NC}"
pwsh -File "$PROJECT_BASE_PATH/_help/pwsh_files/01a_generate_iata_codes_sql.ps1"
pwsh -File "$PROJECT_BASE_PATH/_help/pwsh_files/01b_generate_iata_codes_sql.ps1"
echo -e "${GREEN}IATA codes generated.${NC}"

echo -e "${BLUE}Injecting IATA codes from SQL...${NC}"
psql -h 170.64.135.239 -p 5432 -d avaprod -U postgres -f "$PROJECT_BASE_PATH/_help/sql_files/import_airports_merged.sql"
echo -e "${GREEN}IATA codes injected.${NC}"

# -----------------------------------------------------------------------------
# Setup Company Profile and Default Travel Policy
# -----------------------------------------------------------------------------
echo -e "${BLUE}Setting up company profile and default travel policy...${NC}"
curl -X 'POST' \
  "$API_URL/api/AvaClient" \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "companyName": "DJJM Investments Pty Ltd",
  "contactPersonFirstName": "Johnny",
  "contactPersonLastName": "McClay",
  "contactPersonPhone": "+61410650710",
  "contactPersonEmail": "jonny@djjm.io",
  "contactPersonJobTitle": "Chief Customer Experience Officer",
  "billingPersonFirstName": "Danijel-James",
  "billingPersonLastName": "Wynyard",
  "billingPersonPhone": "+61492257868",
  "billingPersonEmail": "danijel@djjm.io",
  "billingPersonJobTitle": "Director of IT",
  "adminPersonFirstName": "Danijel-James",
  "adminPersonLastName": "Wynyard",
  "adminPersonPhone": "+61492257868",
  "adminPersonEmail": "danijel@djjm.io",
  "adminPersonJobTitle": "Director of IT",
  "createDefaultTravelPolicy": true
}'
echo -e "${GREEN}Company profile set up.${NC}"

# -----------------------------------------------------------------------------
# Fetch Client ID and Setup Client Email Address
# -----------------------------------------------------------------------------
echo -e "${BLUE}Fetching current ClientID...${NC}"
clientId=$(curl -s -X GET "$API_URL/api/AvaClient/1" -H 'accept: */*' | jq -r '.cliendID')
echo -e "${GREEN}Fetched ClientID: $clientId${NC}"

echo -e "${BLUE}Setting up client email address...${NC}"
jsonPayload=$(cat <<EOF
{
  "id": 0,
  "supportedEmailDomain": "djjm.io",
  "avaClientId": 1,
  "clientCode": "$clientId"
}
EOF
)
curl -X 'POST' "$API_URL/api/ClientSupportedDomains" \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d "$jsonPayload"
echo -e "${GREEN}Client email address configured.${NC}"

# -----------------------------------------------------------------------------
# Final Steps: Restart Web App
# -----------------------------------------------------------------------------
echo -e "${BLUE}Stopping Web App using Docker Compose...${NC}"
docker compose -f "$APP_BASE_PATH/compose.yaml" down
echo -e "${GREEN}Web App stopped.${NC}"

echo -e "${BLUE}Starting Web App using Docker Compose...${NC}"
docker compose -f "$APP_BASE_PATH/compose.yaml" up --build --force-recreate -d
echo -e "${GREEN}Web App started successfully.${NC}"

echo -e "${BLUE}========== Deployment Script Completed Successfully! ==========${NC}"

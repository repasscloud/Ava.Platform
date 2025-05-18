#!/usr/bin/env bash

# ANSI color definitions
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[0;33m'
BLUE='\033[0;34m'
NC='\033[0m'  # No Color

echo -e "${BLUE}========== Starting Deployment Script ==========${NC}"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PARENT_DIR="$(dirname "$SCRIPT_DIR")"

export SHARED_BASE_PATH="$PARENT_DIR/Ava.Shared"
export API_BASE_PATH="$PARENT_DIR/Ava.API"
export WEBAPP_BASE_PATH="$PARENT_DIR/Ava.WebApp"
export DEPLOY_BASE_PATH="$PARENT_DIR/Ava.Deploy"
export DEV_BASE_PATH="$PARENT_DIR"
export PGPASSWORD="AVNS_FBtjZZxMnRawVcHCtRH"
export COMPOSE_BAKE=true
export COMPOSE_FILE="$PARENT_DIR/compose.yaml"
export API_URL="http://localhost:5165"
export regions=("APAC" "NA" "EMEA" "LATAM")

echo -e "${GREEN}Shared Base Path:${NC} $SHARED_BASE_PATH"
echo -e "${GREEN}WebApp Base Path:${NC} $WEBAPP_BASE_PATH"
echo -e "${GREEN}API Base Path:${NC} $API_BASE_PATH"
echo -e "${GREEN}API URL:${NC} $API_URL"

# -----------------------------------------------------------------------------
# Docker Network: common_net
# -----------------------------------------------------------------------------
# echo -e "${BLUE}Stopping using Docker Compose...${NC}"
# docker compose -f "$COMPOSE_FILE" down >/dev/null 2>&1
# echo -e "${GREEN}Docker Compose stopped.${NC}"

# echo -e "${BLUE}Checking for Docker network 'common_net'...${NC}"
# if docker network inspect common_net >/dev/null 2>&1; then
#   echo -e "${YELLOW}Network 'common_net' exists. Removing it...${NC}"
#   docker network rm common_net
# else
#   echo -e "${YELLOW}Network 'common_net' does not exist. Will create it now...${NC}"
# fi

# echo -e "${BLUE}Creating Docker network 'common_net'...${NC}"
# docker network create common_net
# echo -e "${GREEN}Network 'common_net' re-created successfully.${NC}"

# -----------------------------------------------------------------------------
# Reset Migrations and Database
# -----------------------------------------------------------------------------
# echo -e "${BLUE}Removing Migrations folder...${NC}"
# rm -rf "$SHARED_BASE_PATH/Migrations"
# echo -e "${GREEN}Migrations folder removed.${NC}"

# echo -e "${BLUE}Dropping and re-creating PostgreSQL database 'avaprod'...${NC}"
# psql \
#   "postgresql://doadmin@db-postgresql-syd1-10702-do-user-9791434-0.k.db.ondigitalocean.com:25060/defaultdb?sslmode=require" \
#   -f "$DEPLOY_BASE_PATH/Resources/SQL/drop_avaprod_db.sql"

# psql \
#   "postgresql://doadmin@db-postgresql-syd1-10702-do-user-9791434-0.k.db.ondigitalocean.com:25060/defaultdb?sslmode=require" \
#   -f "$DEPLOY_BASE_PATH/Resources/SQL/create_avaprod_db.sql"
# echo -e "${GREEN}Database 'avaprod' reset successfully.${NC}"

# -----------------------------------------------------------------------------
# EF Core Migrations
# -----------------------------------------------------------------------------
# echo -e "${BLUE}Creating EF Core migration 'initDb'...${NC}"
# dotnet ef migrations add initDb --project "$SHARED_BASE_PATH" --startup-project "$SHARED_BASE_PATH"
# echo -e "${GREEN}Migration 'initDb' created successfully.${NC}"

# echo -e "${BLUE}Updating database with EF Core migrations...${NC}"
# dotnet ef database update --verbose --project "$SHARED_BASE_PATH" --startup-project "$SHARED_BASE_PATH" > ./.logs/output.txt 2>&1
# echo -e "${GREEN}Database updated successfully. Check 'output.txt' for details.${NC}"

# -----------------------------------------------------------------------------
# Add Database Triggers
# -----------------------------------------------------------------------------
# echo -e "${YELLOW}Inserting triggers in PostgreSQL database 'avadev'...${NC}"
# export PGPASSWORD="AVNS_FBtjZZxMnRawVcHCtRH"
# psql \
#   "postgresql://avaai@db-postgresql-syd1-10702-do-user-9791434-0.k.db.ondigitalocean.com:25060/avadev?sslmode=require" \
#   -f "$DEPLOY_BASE_PATH/Resources/SQL/Triggers/trg_after_insert_aspnetusers.sql"
# psql \
#   "postgresql://avaai@db-postgresql-syd1-10702-do-user-9791434-0.k.db.ondigitalocean.com:25060/avadev?sslmode=require" \
#   -f "$DEPLOY_BASE_PATH/Resources/SQL/Triggers/chk_after_insert_aspnetusers.sql"
# psql \
# "postgresql://avaai@db-postgresql-syd1-10702-do-user-9791434-0.k.db.ondigitalocean.com:25060/avadev?sslmode=require" \
# -f "$DEPLOY_BASE_PATH/Resources/SQL/Triggers/trg_after_insert_licenseagreements_sync_avaclients.sql"
# psql \
#   "postgresql://avaai@db-postgresql-syd1-10702-do-user-9791434-0.k.db.ondigitalocean.com:25060/avadev?sslmode=require" \
#   -f "$DEPLOY_BASE_PATH/Resources/SQL/Triggers/trg_after_insert_storageentries.sql"
# psql \
#   "postgresql://avaai@db-postgresql-syd1-10702-do-user-9791434-0.k.db.ondigitalocean.com:25060/avadev?sslmode=require" \
#   -f "$DEPLOY_BASE_PATH/Resources/SQL/Triggers/trg_before_insert_update_expirejwttokens.sql"
# echo -e "${GREEN}Database 'avadev' reset successfully.${NC}"

# # -----------------------------------------------------------------------------
# # Docker Compose: Start Services
# # -----------------------------------------------------------------------------
# echo -e "${BLUE}Starting Web API using Docker Compose...${NC}"
# #docker compose -f "$COMPOSE_FILE" up --build --force-recreate -d
# docker compose -f "$COMPOSE_FILE" up --build -d
# sleep 20
# echo -e "${GREEN}Web API started successfully.${NC}"

# # -----------------------------------------------------------------------------
# # Inject Region Data via API
# # -----------------------------------------------------------------------------
# echo -e "${BLUE}Injecting region data via API...${NC}"
# for region in "${regions[@]}"; do
#   curl -X POST "$API_URL/api/Region" \
#     -H "accept: */*" \
#     -H "Content-Type: application/json" \
#     -d "{\"name\": \"$region\"}"
#   echo -e "${GREEN}Posted region: $region${NC}"
# done

# # -----------------------------------------------------------------------------
# # Additional Data Import via PowerShell Scripts
# # -----------------------------------------------------------------------------
# echo -e "${BLUE}Importing continent data via PowerShell...${NC}"
# pwsh -File "$DEPLOY_BASE_PATH/Resources/PWSH/02_import_continent_data.ps1"
# echo -e "${GREEN}Continent data imported.${NC}"

# echo -e "${BLUE}Importing country data via PowerShell...${NC}"
# pwsh -File "$DEPLOY_BASE_PATH/Resources/PWSH/03a_import_country_data.ps1"
# pwsh -File "$DEPLOY_BASE_PATH/Resources/PWSH/03b_import_country_data_patched.ps1"
# echo -e "${GREEN}Country data imported.${NC}"

# echo -e "${BLUE}Generating IATA codes from CSV via PowerShell...${NC}"
# pwsh -File "$DEPLOY_BASE_PATH/Resources/PWSH/01a_generate_iata_codes_sql.ps1"
# pwsh -File "$DEPLOY_BASE_PATH/Resources/PWSH/01b_generate_iata_codes_sql.ps1"
# echo -e "${GREEN}IATA codes generated.${NC}"

# echo -e "${BLUE}Injecting IATA codes from SQL...${NC}"
# psql \
#   "postgresql://avaai@db-postgresql-syd1-10702-do-user-9791434-0.k.db.ondigitalocean.com:25060/avadev?sslmode=require" \
#   -f "$DEPLOY_BASE_PATH/Resources/SQL/import_airports_merged.sql"
# echo -e "${GREEN}IATA codes injected.${NC}"

# echo -e "${BLUE}Import latest Aircraft Type Designators CSV...${NC}"
# pwsh -File "$DEPLOY_BASE_PATH/Resources/PWSH/05_upload_aircraftdesignators.ps1"
# echo -e "${GREEN}Import complete.${NC}"

# # -----------------------------------------------------------------------------
# # Setup Company Profile and Default Travel Policy
# # -----------------------------------------------------------------------------
# echo -e "${BLUE}Setting up company profile and default travel policy...${NC}"
# curl -X 'POST' \
#   "$API_URL/api/AvaClient" \
#   -H 'accept: */*' \
#   -H 'Content-Type: application/json' \
#   -d '{
#   "companyName": "DJJM Investments Pty Ltd",
#   "contactPersonFirstName": "Johnny",
#   "contactPersonLastName": "McClay",
#   "contactPersonPhone": "+61410650710",
#   "contactPersonEmail": "jonny@djjm.io",
#   "contactPersonJobTitle": "Chief Customer Experience Officer",
#   "billingPersonFirstName": "Danijel-James",
#   "billingPersonLastName": "Wynyard",
#   "billingPersonPhone": "+61492257868",
#   "billingPersonEmail": "danijel@djjm.io",
#   "billingPersonJobTitle": "Director of IT",
#   "adminPersonFirstName": "Danijel-James",
#   "adminPersonLastName": "Wynyard",
#   "adminPersonPhone": "+61492257868",
#   "adminPersonEmail": "danijel@djjm.io",
#   "adminPersonJobTitle": "Director of IT",
#   "defaultBillingCurrency": "AUD",
#   "createDefaultTravelPolicy": true
# }'
# echo -e "${GREEN}Company profile set up.${NC}"

# # -----------------------------------------------------------------------------
# # Fetch Client ID and Setup Client Email Address
# # -----------------------------------------------------------------------------
# echo -e "${YELLOW}Fetching current ClientID...${NC}"
# clientId=$(curl -s -X GET "$API_URL/api/AvaClient/1" -H 'accept: */*' | jq -r '.cliendID')
# echo -e "${GREEN}Fetched ClientID:${NC} $clientId"

# echo -e "${YELLOW}Setting up client email address...${NC}"
# jsonPayload=$(cat <<EOF
# {
#   "id": 0,
#   "supportedEmailDomain": "djjm.io",
#   "avaClientId": 1,
#   "clientCode": "$clientId"
# }
# EOF
# )
# curl -X 'POST' "$API_URL/api/ClientSupportedDomains" \
#   -H 'accept: */*' \
#   -H 'Content-Type: application/json' \
#   -d "$jsonPayload"
# echo -e "${GREEN}Client email address configured.${NC}"

# # -----------------------------------------------------------------------------
# # Final Steps: Restart Web App
# # -----------------------------------------------------------------------------
# echo -e "${BLUE}Stopping using Docker Compose...${NC}"
# docker compose -f "$COMPOSE_FILE" down >/dev/null 2>&1
# echo -e "${GREEN}Docker Compose stopped.${NC}"

# echo -e "${BLUE}Starting Web API using Docker Compose...${NC}"
# docker compose -f "$COMPOSE_FILE" up --build -d
# sleep 20
# echo -e "${GREEN}Web API started successfully.${NC}"

# echo -e "${BLUE}========== Deployment Script Completed Successfully! ==========${NC}"

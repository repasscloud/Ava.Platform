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
export PGPASSWORD="devpassword"
export COMPOSE_BAKE=true
export COMPOSE_FILE="$PARENT_DIR/compose.yaml"
export API_URL="http://localhost:5165"
export regions=("APAC" "NA" "EMEA" "LATAM")

echo -e "${GREEN}Shared Base Path:${NC} $SHARED_BASE_PATH"
echo -e "${GREEN}WebApp Base Path:${NC} $WEBAPP_BASE_PATH"
echo -e "${GREEN}API Base Path:${NC} $API_BASE_PATH"
echo -e "${GREEN}API URL:${NC} $API_URL"

# -----------------------------------------------------------------------------
# Add Database Triggers
# -----------------------------------------------------------------------------
echo -e "${YELLOW}Inserting triggers in PostgreSQL database 'avaprod'...${NC}"
export PGPASSWORD="avaaiPassword1"
psql -h 170.64.135.239 -p 5432 -d avaprod -U avaai -f "$DEPLOY_BASE_PATH/Resources/SQL/Triggers/trg_after_insert_aspnetusers.sql"
psql -h 170.64.135.239 -p 5432 -d avaprod -U avaai -f "$DEPLOY_BASE_PATH/Resources/SQL/Triggers/chk_after_insert_aspnetusers.sql"
echo -e "${GREEN}Database 'avaprod' reset successfully.${NC}"

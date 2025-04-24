#!/bin/zsh

# Configuration
GITHUB_TOKEN="ghp_yourTokenHere"
REPO_OWNER="repasscloud"
REPO_NAME="AvaPlatform-Issues"

# Sample label names & descriptions
LABEL_NAMES=(
  "cat-auth"
  "cat-uiux"
  "cat-data"
  "cat-onboarding"
  "cat-performance"
  "cat-access"
  "cat-integration"
  "cat-infra"
)

LABEL_DESCRIPTIONS=(
  "Authentication-related issues"
  "UI/UX and frontend concerns"
  "Backend data and database"
  "New user/client onboarding"
  "Performance optimization tasks"
  "Access control and permissions"
  "API/3rd-party integration problems"
  "Infrastructure and deployment"
)

# Function to generate random HEX color
generate_hex_color() {
  printf "%06x" $(( RANDOM * RANDOM % 0xFFFFFF ))
}

# Create labels
for i in {1..${#LABEL_NAMES[@]}}; do
  NAME="${LABEL_NAMES[$i]}"
  DESCRIPTION="${LABEL_DESCRIPTIONS[$i]}"
  COLOR=$(generate_hex_color)

  echo "Creating label: $NAME ($COLOR)"

  curl -s -X POST "https://api.github.com/repos/$REPO_OWNER/$REPO_NAME/labels" \
    -H "Authorization: token $GITHUB_TOKEN" \
    -H "Accept: application/vnd.github+json" \
    -d @- <<EOF
{
  "name": "$NAME",
  "description": "$DESCRIPTION",
  "color": "$COLOR"
}
EOF

  sleep 1
done

echo "✅ Label creation completed."

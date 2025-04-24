#!/bin/zsh

# Configuration
GITHUB_TOKEN="ghp_yourTokenHere"
REPO_OWNER="repasscloud"
REPO_NAME="AvaPlatform-Issues"

# Get all label names
echo "🔍 Fetching all labels from $REPO_OWNER/$REPO_NAME..."

LABELS=$(curl -s -X GET "https://api.github.com/repos/$REPO_OWNER/$REPO_NAME/labels?per_page=100" \
  -H "Authorization: token $GITHUB_TOKEN" \
  -H "Accept: application/vnd.github+json" | jq -r '.[].name')

if [[ -z "$LABELS" ]]; then
  echo "❌ No labels found or failed to fetch labels."
  exit 1
fi

# Delete each label
for LABEL in ${(f)LABELS}; do
  ENCODED_LABEL=$(python3 -c "import urllib.parse; print(urllib.parse.quote('''$LABEL'''))")
  echo "🗑️  Deleting label: $LABEL"

  curl -s -X DELETE "https://api.github.com/repos/$REPO_OWNER/$REPO_NAME/labels/$ENCODED_LABEL" \
    -H "Authorization: token $GITHUB_TOKEN" \
    -H "Accept: application/vnd.github+json"

  sleep 1
done

echo "✅ All labels deleted."

#!/usr/bin/env zsh

# Define owner and repo
OWNER="repasscloud"
REPO="Ava.Platform"

echo "📥 Fetching subscribed issues..."

# Fetch subscribed issues and extract their node IDs
issue_ids=($(gh api graphql -f query='
{
  repository(owner: "repasscloud", name: "Ava.Platform") {
    issues(first: 100, states: OPEN, filterBy: {viewerSubscribed: true}) {
      nodes {
        id
        number
        title
      }
    }
  }
}' --jq '.data.repository.issues.nodes[].id'))

# Loop through each issue ID and unsubscribe
for issue_id in "${issue_ids[@]}"; do
  echo "🚫 Unsubscribing from issue ID: $issue_id"
  gh api graphql -f query="
    mutation {
      updateSubscription(input: {
        subscribableId: \"$issue_id\",
        state: UNSUBSCRIBED
      }) {
        subscribable {
          viewerSubscription
        }
      }
    }
  " > /dev/null
done

echo "✅ Unsubscribed from all subscribed issues in $REPO."

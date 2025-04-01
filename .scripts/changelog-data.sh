#!/usr/bin/env zsh

SCRIPT_DIR="$(cd "$(dirname "${(%):-%x}")" && pwd)"
PARENT_DIR="$(dirname "$SCRIPT_DIR")"

# Export current issues
gh issue list --limit 100 --json number,title --jq '.[] | "#\(.number) \(.title)"' > "$PARENT_DIR/.logs/issues.txt"

# Format as markdown list with links
sed -E 's/#([0-9]+) (.+)/- [#\1](https:\/\/github.com\/repasscloud\/Ava.WebApp\/issues\/\1)\n  \2/' "$PARENT_DIR/.logs/issues.txt" > "$PARENT_DIR/.logs/issues_updated.txt"

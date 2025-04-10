#!/usr/bin/env bash

gh issue list --label "to-be-merged" --state open --limit 1000 --json number -q '.[].number' | while read issue; do
  gh issue close "$issue"
  gh issue edit "$issue" --remove-label "to-be-merged" --add-label "complete"
done

gh issue list --state closed --limit 1000 --json number -q '.[].number' | while read issue; do
  gh issue edit "$issue" --add-label "complete"
done

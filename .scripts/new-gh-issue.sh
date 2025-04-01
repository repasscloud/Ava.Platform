#!/usr/bin/env zsh

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PARENT_DIR="$(dirname "$SCRIPT_DIR")"

gh issue create \
  --title "🧾 Create new Transaction table and model" \
  --body-file "$PARENT_DIR/.logs/issue.md" \
  --label "class-modify,backlog,enhancement" \
  --assignee "@me"


# class-modify                  #5C1263
# docker                        #166B82
# backlog                       #D376E6
# build-issue                   #14B121
# ISO-3166                      #07BC8C
# nuget-packages                #e99695
# enhancement                   #a2eeef
# identity                      #5319e7
# travel-domain                 #fbca04
# api-contract                  #0052cc
# blazor-webapp                 #512da8
# webapp                        #0366d6
# shared-library                #e99695
# performance                   #c2e0c6
# security                      #d876e3
# breaking-change               #ffcc00
# documentation                 #0075ca
# refactor                      #e1e4e8
# sql                           #FD65C0
# services                      #2F7400
# user-preference               #73D343
# will-not-fix                  #639487
# api-controllers               #9BBA64
# complete                      #34BD0A
# re-opened                     #510624
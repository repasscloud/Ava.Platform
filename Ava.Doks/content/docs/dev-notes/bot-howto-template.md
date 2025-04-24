---
title: "Bot: [short title here]"
description: "[one-sentence purpose]"
summary: ""
date: 2025-04-23T10:45:00+10:00
lastmod: 2025-04-23T10:45:00+10:00
draft: false
weight: 900
toc: true
seo:
  title: ""
  description: ""
  canonical: ""
  robots: "index,follow"
---

## Goal

Explain what this bot is doing, who runs it, and what repo(s) or services it's tied to.

## Bot Requirements

| Scope/API         | Why it's needed                          |
|:------------------|:------------------------------------------|
| `repo`            | Full control of private repos (issues etc.) |
| `public_repo`     | Public repos only                        |
| `workflow`        | Dispatch workflows                       |
| `write:discussion`| For interacting with GitHub Discussions   |

> ✅ Include exact GitHub token scope or GitHub App permission.

## Repo Setup

1. Add bot to repo: **Settings → Collaborators**
2. Set **Write** access
3. If GitHub App: grant required **permissions**
4. Ensure **issues** or **actions** are enabled

## Behavior

- What this bot does automatically
- What triggers it (comment, webhook, cron, etc.)
- What it should NOT do

## Example Usage

```sh
curl -X POST \
  -H "Authorization: token <TOKEN>" \
  -d '{"body": "ping"}' \
  https://api.github.com/repos/org/repo/issues/123/comments
```

## Common Errors

| Error Message                          | What it means                             |
|:--------------------------------------|:------------------------------------------|
| Must have admin rights to Repository. | Token lacks write or bot not added        |
| 403 Forbidden                         | Token doesn't have required scope         |
| 404 Not Found                         | Endpoint is wrong or bot isn't authorized |

## Notes

- Use `Octokit` or `curl` for testing
- Document token owner or GitHub App identity
- Rotate or expire tokens regularly

---
title: "GitHub Bot Permissions for Issues"
description: "Minimum permissions and repo access required for a bot to manage issues."
summary: ""
date: 2025-04-23T10:30:00+10:00
lastmod: 2025-04-23T10:30:00+10:00
draft: false
weight: 920
toc: true
seo:
  title: ""
  description: ""
  canonical: ""
  robots: "index,follow"
---

## Required Scopes (GitHub PAT)

| Scope        | Needed for                    |
|:-------------|:------------------------------|
| `public_repo` | Public repo (read/write issues) |
| `repo`        | Private repo (full access to issues) |

> ✅ Use `repo` for internal bots on private repos.

## Bot Cannot Close Issues?

Check:

- Token has `repo` scope (or `public_repo` for public)
- Repo has **Issues enabled**
- Issue is not created by someone the bot can't close on (must be a collaborator or have write)

## Add Bot to Repo (Write Access)

1. Go to **repo → Settings → Collaborators**
2. **Invite bot account (e.g., `ava2-system-ticket-bot`)**
3. Set **access level** to: `Write`
4. Accept the invite from the bot account if needed

> ❗ Without this, the bot will fail to close issues with:
> `Error: Must have admin rights to Repository.`

## Diagnostic

```sh
# Can read comments
curl -H "Authorization: token <token>" \
     https://api.github.com/repos/org/repo/issues/1/comments

# Can post comment
curl -X POST \
     -H "Authorization: token <token>" \
     -H "Accept: application/vnd.github.v3+json" \
     -d '{"body":"Test comment"}' \
     https://api.github.com/repos/org/repo/issues/1/comments

# Cannot close if not collaborator
# Octokit throws: "Must have admin rights to Repository"
```

## Reminder

- **Posting comments** = OK with token only
- **Closing issues** = Must have `write` access
- **Changing labels** = Also needs write

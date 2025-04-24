---
title: "Run GitHub CLI in Isolated Alpine Docker Container"
description: "Quick guide to run GitHub CLI (`gh`) in a clean Alpine Linux Docker container without affecting your local machine."
summary: ""
date: 2023-09-07T16:13:18+02:00
lastmod: 2023-09-07T16:13:18+02:00
# tags: [docker, alpine, github-cli, dev-tools, internal]
draft: false
weight: 910
toc: true
seo:
  title: "" # custom title (optional)
  description: "" # custom description (recommended)
  canonical: "" # custom canonical URL (optional)
  robots: "index,follow" # custom robot tags (optional)
---

## 🚀 Run GitHub CLI in an Isolated Container

Useful for testing GitHub CLI (`gh`) commands in a clean shell.

### 🧪 One-liner (Ad-hoc)

```bash
docker run -it --rm alpine /bin/sh
```

Inside the container:

```bash
apk add --no-cache curl bash git

GH_VERSION=$(curl -s https://api.github.com/repos/cli/cli/releases/latest | grep '"tag_name":' | cut -d'"' -f4)

curl -L "https://github.com/cli/cli/releases/download/${GH_VERSION}/gh_${GH_VERSION#v}_linux_amd64.tar.gz" -o gh.tar.gz
tar -xzf gh.tar.gz
mv gh_*/bin/gh /usr/local/bin/

gh version
```

## 📦 Dockerfile (Reusable Image)

```dockerfile
FROM alpine:latest

RUN apk add --no-cache curl bash git && \
    GH_VERSION=$(curl -s https://api.github.com/repos/cli/cli/releases/latest | grep '"tag_name":' | cut -d'"' -f4) && \
    curl -L "https://github.com/cli/cli/releases/download/${GH_VERSION}/gh_${GH_VERSION#v}_linux_amd64.tar.gz" -o gh.tar.gz && \
    tar -xzf gh.tar.gz && \
    mv gh_*/bin/gh /usr/local/bin/ && \
    rm -rf gh.tar.gz gh_*

ENTRYPOINT ["/bin/sh"]
```

Build & run:

```bash
docker build -t ghcli-alpine .
docker run -it --rm ghcli-alpine
```

## 🧰 Basic Commands

```bash
gh repo list             # List your repositories
gh repo clone owner/repo # Clone a repo
gh issue list            # List issues in current or specified repo
gh pr status             # Show PRs relevant to you
gh api /user             # Make raw GitHub API call
```

Use `--help` on any command, e.g., `gh pr --help`.

### 🔐 Authenticate to GitHub with a Token (non-interactive)

If you're in a headless container or want to skip the browser-based login, use a **GitHub Personal Access Token** (PAT):

```bash
gh auth login --with-token < token.txt
```

Or inline:

```bash
echo "ghp_abc123yourtoken" | gh auth login --with-token
```

## 🔐 Example: Create and Use Token

1. Go to `GitHub` → `Settings` → `Developer settings` → `Personal access tokens`

1. Generate a classic or fine-grained token with appropriate access

1. Store it in a file (optional):

    ```bash
    echo "ghp_abc123yourtoken" > token.txt
    ```

1. Login:

    ```bash
    gh auth login --with-token < token.txt
    ```

1. Verify with:

    ```bash
    gh auth status
    ```

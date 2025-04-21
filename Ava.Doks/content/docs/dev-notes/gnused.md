---
title: "Recursive Search & Replace with GNU sed"
description: "Terse DevOps pattern for in-place text substitution across files using grep and GNU sed."
summary: "How to recursively search and replace strings in-place using GNU sed and grep—ideal for renaming across large codebases."
date: 2023-09-07T16:13:18+02:00
lastmod: 2023-09-07T16:13:18+02:00
draft: false
weight: 910
toc: true
seo:
  title: "How to Recursively Replace Text with grep + GNU sed"
  description: "Quick DevOps recipe: recursively replace strings across a repo using grep and gnu-sed. Ideal for renaming classes, namespaces, tokens."
  canonical: "" # add canonical URL if this is cross-posted
  robots: "index,follow" # custom robot tags (optional)
---

When updating project-wide names, namespaces, or tokens (e.g. company name changes, rebranding, refactors), use this pattern with `gnu-sed`.

## Command

```zsh
grep -rl '<old_value>' <path> --exclude-dir=.git | xargs sed -i 's/<old_value>/<new_value>/g'
```

## Real Example

To rename all instances of AvaTerminal to AvaAITerminal:

```zsh
grep -rl 'AvaTerminal' . --exclude-dir=.git | xargs sed -i 's/AvaTerminal/AvaAITerminal/g'
```

## Pre-check

```zsh
grep -r '<old_value>' <path>
```

## Optional: Rename Files or Folders

```zsh
find . -type f -name '*<old_value>*' -exec bash -c 'mv "$0" "${0/<old_value>/<new_value>}"' {} \;
```

## BSD/macOS Note

If you're using default macOS `sed`, add `-i ''` instead:

```zsh
sed -i '' 's/foo/bar/g'
```

But if you're running GNU sed (`sed --version` returns `gnu`), the `-i` flag works without quotes.

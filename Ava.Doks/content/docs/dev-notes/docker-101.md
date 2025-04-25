---
title: "Docker & Compose 101"
description: "Cheat sheet of core Docker and Docker Compose commands, plus build cache essentials"
summary: ""
date: 2025-04-25T12:00:00+10:00
lastmod: 2025-04-25T12:00:00+10:00
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

Provide a concise reference for essential Docker and Docker Compose commands and explain how Docker’s build cache works.

## Requirements

| Tool               | Why it's needed                                    |
|:-------------------|:----------------------------------------------------|
| `docker`           | CLI for building, running, and managing containers  |
| `docker-compose`   | Compose tool for multi-container application stacks |
| BuildKit           | (Optional) advanced build cache and mount caching   |

> ✅ Docker Engine ≥ 19.03 for BuildKit support.

## Setup

1. **Install Docker Engine**  
   Follow the official guide: https://docs.docker.com/engine/install  
2. **Install Docker Compose**  
   - v2 plugin (recommended): comes bundled with recent Docker Desktop/Engine  
   - Legacy: `pip install docker-compose` or package manager  
3. **Enable BuildKit**  
   ```bash
   export DOCKER_BUILDKIT=1
   ```
4. **(Optional) Remote cache**  
   Ensure network access to your registry if you plan to push/pull build cache layers.

## Commands

```bash
# ─── Docker “101” ────────────────────────────────────────────────────────
docker build -t myapp:latest .         # 1. Build image from Dockerfile
docker images                          # 2. List all images
docker images -f "dangling=true"       # 3. List only <none> images
docker rmi myapp:latest                # 4. Remove image by name or ID
docker image prune                     # 5. Remove dangling images (interactive)
docker image prune -f                  #    skip prompt
docker image prune -a -f               # 6. Remove all unused images
docker run --rm -it myapp:latest /bin/bash  # 7. Run container interactively
docker run -d -p 8080:80 --name web nginx:alpine  # 8. Detached run with port mapping
docker ps                              # 9. List running containers
docker ps -a                           # 10. List all containers
docker stop web                        # 11. Stop a running container
docker rm web another                  # 12. Remove containers
docker container prune -f              # 13. Remove all stopped containers
docker logs -f web                     # 14. Stream container logs
docker exec -it web sh                 # 15. Exec into running container
docker cp ./file.txt web:/app/         # 16. Copy files host↔container
docker stats                           # 17. Show real-time container stats
docker inspect web                     # 18. Inspect container or image
docker tag myapp:latest repo/myapp:v1  # 19. Tag an image
docker push repo/myapp:v1              # 20. Push to registry

# ─── Cleanup Essentials ─────────────────────────────────────────────────
docker system prune                    # Remove unused data (interactive)
docker system prune -a -f              # Include all unused images
docker builder prune -f                # Clear build cache only

# ─── Docker Compose “101” ───────────────────────────────────────────────
docker-compose build                   # 21. Build all services
docker-compose build web               # 22. Build specific service
docker-compose up                      # 23. Start in foreground
docker-compose up -d                   # 24. Start detached
docker-compose logs                    # 25. View all logs
docker-compose logs -f db              # 26. Follow logs for one service
docker-compose stop                    # 27. Stop services
docker-compose down                    # 28. Stop & remove containers/networks
docker-compose down -v                 # 29. Also remove named volumes
docker-compose ps                      # 30. List Compose-managed containers
docker-compose exec web sh             # 31. Run command in service container
docker-compose rm                      # 32. Remove stopped service containers
docker-compose up -d --scale worker=5  # 33. Scale a service
docker-compose run --rm web migrate    # 34. One-off run, then remove
```

## Build Cache

Docker’s build cache reuses previously built layers to speed up rebuilds:

- **Layer key** = hash of the instruction + its context (files, args).
- On rebuild, if the hash matches, Docker pulls the layer from cache instead of rerunning it.
- **Benefits:**
    - Faster incremental builds (only changed layers rebuild).
    -  CI/CD jobs can share cache via `--cache-from`/`--cache-to`.
    - Reduced feedback loop when iterating on code.

### Examples

```bash
# Inline cache mounts (BuildKit)
DOCKER_BUILDKIT=1 docker build \
  --mount=type=cache,target=/root/.npm \
  -t myapp:dev .
```

```bash
# Remote cache export/import
DOCKER_BUILDKIT=1 docker build \
  --cache-from=type=registry,ref=registry.io/myapp:cache \
  --cache-to=type=registry,ref=registry.io/myapp:cache,mode=max \
  -t myapp:latest .
```

## Example Usage

```bash
# 1. Clean up dangling images
docker images -f "dangling=true"
docker image prune -f

# 2. Build & cache
export DOCKER_BUILDKIT=1
docker build --cache-to=type=local,dest=./.cache \
             --cache-from=type=local,src=./.cache \
             -t myapp:latest .

# 3. Bring up full stack
docker-compose up -d
```

## Common Errors

| Error Message | What it means |
|:--------------|:--------------|
| `no space left on device` | Disk full (too many images/build caches) |
| `invalid reference format` | Bad image name/tag syntax |
| `permission denied while trying…` | Insufficient permissions (use sudo or add user) |

## Notes

- Always tag your builds (`-t name:version`) to avoid `<none>:<none>`.
- Prune system and builder caches periodically:
   ```bash
   docker system prune -a -f
   docker builder prune -f
   ```
- Use BuildKit mounts (`--mount=type=cache`) for dependency caches without bloating images.

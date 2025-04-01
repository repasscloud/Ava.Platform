#!/usr/bin/env zsh

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PARENT_DIR="$(dirname "$SCRIPT_DIR")"

[ -f "$PARENT_DIR/Ava.API/Ava.API.sln" ] && rm -rf "$PARENT_DIR/Ava.API/Ava.API.sln"
[ -f "$PARENT_DIR/Ava.WebApp/Ava.WebApp.sln" ] && rm -rf "$PARENT_DIR/Ava.WebApp/Ava.WebApp.sln"
[ -f "$PARENT_DIR/Ava.Shared/Ava.Shared.sln" ] && rm -rf "$PARENT_DIR/Ava.Shared/Ava.Shared.sln"


docker buildx build \
  --platform linux/arm64 \
  --file "$PARENT_DIR/Dockerfile.builder" \
  --tag djjm.io/dotnet-sdk-preloaded:9.0.200-alpine \
  --load \
  "$PARENT_DIR"

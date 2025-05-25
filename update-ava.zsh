#!/usr/bin/env zsh
# 🚀 update-ava.zsh — bump X.Y.Z in ava_version and sync all submodules (no functions!)

set -euo pipefail

# ──────────────────────────────────────────────────────────────────────────────
# 🛠️ Config
VERSION_FILE="ava_version"

SHARED_DIR="Ava.Shared"
SHARED_VERSION_FILE="$SHARED_DIR/Models/Static/VersionInfo.cs"

API_DIR="Ava.API"
API_DOCKERFILE_SUB="${API_DIR}/Dockerfile.API"
API_DOCKERFILE_LOCAL="Dockerfile.API"

API_DOCKER_DIR="ava.api.docker"
API_COMPOSE_FILE="$API_DOCKER_DIR/compose.yaml"

DEPLOY_DOCKER_DIR="ava.deploy.docker"

TERM3_DIR="ava.terminal3"
TERM3_VERSION_FILE="$TERM3_DIR/Models/Static/AppVersion.cs"

# ──────────────────────────────────────────────────────────────────────────────
# 📄 Ensure version file
if [[ ! -f $VERSION_FILE ]]; then
  echo "0.0.0" >| $VERSION_FILE
  echo "📄 Created $VERSION_FILE → 0.0.0"
fi

# ──────────────────────────────────────────────────────────────────────────────
# 📥 Read & bump version
current=$(< $VERSION_FILE)
echo "🔎 Current version: $current"
IFS='.' read -r major minor patch <<< "$current"

mode=${1:---build}
case "$mode" in
  --patch)   (( minor++ )); patch=0 ;;
  --version) (( major++ )); minor=0; patch=0 ;;
  --build|"" ) (( patch++ )) ;;
  *) echo "Usage: $0 [--build|--patch|--version]"; exit 1 ;;
esac

new_version="${major}.${minor}.${patch}"
echo "📈 Bumped → $new_version"
echo $new_version >| $VERSION_FILE
echo "✅ Wrote $VERSION_FILE"

# ──────────────────────────────────────────────────────────────────────────────
# 1) Ava.Shared
if [[ -f $SHARED_VERSION_FILE ]]; then
  echo "\n---\n⚙️  Ava.Shared"
  sed -i -E 's#(ClientVersion *= *")[^"]*(";)#\1'"$new_version"'\2#' "$SHARED_VERSION_FILE"
  echo "  ✏️ Updated $SHARED_VERSION_FILE"
  cp $VERSION_FILE "$SHARED_DIR/$VERSION_FILE"

  echo "  🔄 Commit & push Ava.Shared"
  pushd "$SHARED_DIR" >/dev/null
    git add .
    git commit -m "v${new_version}" || echo "  ⚠️ no changes"
    git push origin HEAD:main --force
    if git ls-remote --exit-code --heads origin dev &>/dev/null; then
      git push origin HEAD:dev --force
    fi
    if ! git rev-parse "refs/tags/v${new_version}" &>/dev/null; then
      git tag -s "v${new_version}" -m "v${new_version}"
      git push origin "v${new_version}"
    fi
  popd >/dev/null
  echo "  🎉 Ava.Shared done"
fi

# ──────────────────────────────────────────────────────────────────────────────
# 2) Ava.API submodule
if [[ -f $API_DOCKERFILE_SUB ]]; then
  echo "\n---\n⚙️  Ava.API"
  sed -i -E 's#^LABEL version="[^"]*"#LABEL version="'"$new_version"'"#' "$API_DOCKERFILE_SUB"
  echo "  ✏️ Updated $API_DOCKERFILE_SUB"
  cp $VERSION_FILE "$API_DIR/$VERSION_FILE"

  echo "  🔄 Commit & push Ava.API"
  pushd "$API_DIR" >/dev/null
    git add .
    git commit -m "v${new_version}" || echo "  ⚠️ no changes"
    git push origin HEAD:main --force
    if git ls-remote --exit-code --heads origin dev &>/dev/null; then
      git push origin HEAD:dev --force
    fi
    if ! git rev-parse "refs/tags/v${new_version}" &>/dev/null; then
      git tag -s "v${new_version}" -m "v${new_version}"
      git push origin "v${new_version}"
    fi
  popd >/dev/null
  echo "  🎉 Ava.API done"
fi

# ──────────────────────────────────────────────────────────────────────────────
# 3) Local Dockerfile.API only
if [[ -f $API_DOCKERFILE_LOCAL ]]; then
  echo "\n---\n⚙️  local Dockerfile.API"
  sed -i -E 's#^LABEL version="[^"]*"#LABEL version="'"$new_version"'"#' "$API_DOCKERFILE_LOCAL"
  echo "  🎉 Updated local Dockerfile.API"
fi

# ──────────────────────────────────────────────────────────────────────────────
# 4) ava.api.docker
if [[ -f $API_COMPOSE_FILE ]]; then
  echo "\n---\n⚙️  ava.api.docker"
  sed -i -E 's#(repasscloud/ava-api:).*#\1'"$new_version"'#' "$API_COMPOSE_FILE"
  echo "  ✏️ Updated $API_COMPOSE_FILE"
  cp $VERSION_FILE "$API_DOCKER_DIR/$VERSION_FILE"

  echo "  🔄 Commit & push ava.api.docker"
  pushd "$API_DOCKER_DIR" >/dev/null
    git add .
    git commit -m "v${new_version}" || echo "  ⚠️ no changes"
    git push origin HEAD:main --force
    if git ls-remote --exit-code --heads origin dev &>/dev/null; then
      git push origin HEAD:dev --force
    fi
    if ! git rev-parse "refs/tags/v${new_version}" &>/dev/null; then
      git tag -s "v${new_version}" -m "v${new_version}"
      git push origin "v${new_version}"
    fi
  popd >/dev/null
  echo "  🎉 ava.api.docker done"
fi

# ──────────────────────────────────────────────────────────────────────────────
# 5) ava.deploy.docker
echo "\n---\n⚙️  ava.deploy.docker"
cp $VERSION_FILE "$DEPLOY_DOCKER_DIR/$VERSION_FILE"

echo "  🔄 Commit & push ava.deploy.docker"
pushd "$DEPLOY_DOCKER_DIR" >/dev/null
  git add .
  git commit -m "v${new_version}" || echo "  ⚠️ no changes"
  git push origin HEAD:main --force
  if git ls-remote --exit-code --heads origin dev &>/dev/null; then
    git push origin HEAD:dev --force
  fi
  if ! git rev-parse "refs/tags/v${new_version}" &>/dev/null; then
    git tag -s "v${new_version}" -m "v${new_version}"
    git push origin "v${new_version}"
  fi
popd >/dev/null
echo "  🎉 ava.deploy.docker done"

# ──────────────────────────────────────────────────────────────────────────────
# 6) ava.terminal3
if [[ -f $TERM3_VERSION_FILE ]]; then
  echo "\n---\n⚙️  ava.terminal3"
  sed -i -E 's#(VersionInfo *= *")[^"]*(";)#\1'"$new_version"'\2#' "$TERM3_VERSION_FILE"
  echo "  ✏️ Updated $TERM3_VERSION_FILE"
  cp $VERSION_FILE "$TERM3_DIR/$VERSION_FILE"

  echo "  🔄 Commit & push ava.terminal3"
  pushd "$TERM3_DIR" >/dev/null
    git add .
    git commit -m "v${new_version}" || echo "  ⚠️ no changes"
    git push origin HEAD:main --force
    if git ls-remote --exit-code --heads origin dev &>/dev/null; then
      git push origin HEAD:dev --force
    fi
    if ! git rev-parse "refs/tags/v${new_version}" &>/dev/null; then
      git tag -s "v${new_version}" -m "v${new_version}"
      git push origin "v${new_version}"
    fi
  popd >/dev/null
  echo "  🎉 ava.terminal3 done"
fi

# ──────────────────────────────────────────────────────────────────────────────
# ⭐ Final root commit & tag
echo "\n---\n🔄 Finalizing root repo"
git add .
git commit -m "v${new_version}" || echo "⚠️ no root changes"
git push origin HEAD:main --force
git push origin HEAD:dev --force

root_tag="v${new_version}"
if ! git rev-parse "refs/tags/$root_tag" &>/dev/null; then
  git tag -s "$root_tag" -m "$root_tag"
  git push origin "$root_tag"
fi

echo "\n🎉 All done! Ava is now at v${new_version}"

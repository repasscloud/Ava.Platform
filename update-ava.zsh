#!/usr/bin/env zsh
# 🚀 update-ava.zsh — bump X.Y.Z in ava_version and sync all submodules

set -euo pipefail

# 🛠️ Configuration
VERSION_FILE="ava_version"

# 📁 Submodule directories & files
SHARED_DIR="Ava.Shared"
SHARED_VERSION_FILE="$SHARED_DIR/Models/Static/VersionInfo.cs"

API_DIR="Ava.API"
API_DOCKERFILE_SUB="${API_DIR}/Dockerfile.API"
API_DOCKERFILE_LOCAL="Dockerfile.API"

API_DOCKER_DIR="ava.api.docker"
API_COMPOSE_FILE="${API_DOCKER_DIR}/compose.yaml"

DEPLOY_DOCKER_DIR="ava.deploy.docker"

TERM3_DIR="ava.terminal3"
TERM3_VERSION_FILE="$TERM3_DIR/Models/Static/AppVersion.cs"

# 📄 Ensure the version file exists
if [[ ! -f "$VERSION_FILE" ]]; then
  echo "0.0.0" >| "$VERSION_FILE"
  echo "📄  Created $VERSION_FILE with initial version 0.0.0"
fi

# 📥 Read the current version
current_version="$(< "$VERSION_FILE")"
echo "🔎  Current Ava version: $current_version"
IFS='.' read -r major minor patch <<< "$current_version"

# 🔧 Bump logic: --build (patch), --patch (minor+1, reset patch), --version (major+1, reset others)
mode="${1:---build}"
case "$mode" in
  --patch)
    (( minor++ )); patch=0
    echo "⚙️  Minor bump → new version will be: $major.$minor.$patch"
    ;;
  --version)
    (( major++ )); minor=0; patch=0
    echo "🚀  Major bump → new version will be: $major.$minor.$patch"
    ;;
  --build|"")
    (( patch++ ))
    echo "🔨  Patch bump → new version will be: $major.$minor.$patch"
    ;;
  *)
    echo "❓ Usage: $0 [--build|--patch|--version]"
    exit 1
    ;;
esac

new_version="${major}.${minor}.${patch}"

# 💾 Write new version to file
echo "$new_version" >| "$VERSION_FILE"
echo "✅  Updated $VERSION_FILE → $new_version"

# 🛠️ commit_submodule: add, commit, push & tag in each submodule repo
commit_submodule() {
  local dir="$1"
  local tag="v${new_version}"

  echo "\n🔄 Processing submodule: $dir"

  # commit changes
  git -C "$dir" add .
  git -C "$dir" commit -m "$tag" || echo "⚠️  No changes in $dir"

  # push main branch
  git -C "$dir" push origin HEAD:main --force \
    && echo "✅  Pushed main→origin/main in $dir"

  # push dev branch if it exists remotely
  if git -C "$dir" ls-remote --exit-code --heads origin dev &>/dev/null; then
    git -C "$dir" push origin HEAD:dev --force \
      && echo "✅  Pushed main→origin/dev in $dir"
  else
    echo "⚠️  No remote 'dev' branch in $dir, skipped dev push"
  fi

  # tag and push tag
  if ! git -C "$dir" rev-parse "refs/tags/$tag" >/dev/null 2>&1; then
    git -C "$dir" tag -s "$tag" -m "$tag" \
      && echo "🏷  Created signed tag $tag in $dir"
    git -C "$dir" push origin "$tag" \
      && echo "✅  Pushed tag $tag in $dir"
  else
    echo "⚠️  Tag $tag already exists in $dir, skipping"
  fi

  echo "🎉  Done submodule $dir"
}

# 🔄 Update & tag each submodule in sequence

# 1) Ava.Shared
if [[ -f "$SHARED_VERSION_FILE" ]]; then
  echo "\n---\n⚙️  Updating Ava.Shared version"
  sed -i -E "s#(ClientVersion *= *\")[^\"]*(\";)#\1${new_version}\2#" "$SHARED_VERSION_FILE"
  cp "$VERSION_FILE" "$SHARED_DIR/$VERSION_FILE"
  commit_submodule "$SHARED_DIR"
fi

# 2) Ava.API submodule
if [[ -f "$API_DOCKERFILE_SUB" ]]; then
  echo "\n---\n⚙️  Updating Ava.API Dockerfile"
  sed -i -E "s#^LABEL version=\"[^\"]*\"#LABEL version=\"${new_version}\"#" "$API_DOCKERFILE_SUB"
  cp "$VERSION_FILE" "$API_DIR/$VERSION_FILE"
  commit_submodule "$API_DIR"
fi

# 3) Local Dockerfile.API (no commit)
if [[ -f "$API_DOCKERFILE_LOCAL" ]]; then
  echo "\n---\n⚙️  Updating local Dockerfile.API"
  sed -i -E "s#^LABEL version=\"[^\"]*\"#LABEL version=\"${new_version}\"#" "$API_DOCKERFILE_LOCAL"
fi

# 4) ava.api.docker
if [[ -f "$API_COMPOSE_FILE" ]]; then
  echo "\n---\n⚙️  Updating compose.yaml"
  sed -i -E "s#(repasscloud/ava-api:).*#\1${new_version}#" "$API_COMPOSE_FILE"
  cp "$VERSION_FILE" "$API_DOCKER_DIR/$VERSION_FILE"
  commit_submodule "$API_DOCKER_DIR"
fi

# 5) ava.deploy.docker
cp "$VERSION_FILE" "$DEPLOY_DOCKER_DIR/$VERSION_FILE"
commit_submodule "$DEPLOY_DOCKER_DIR"

# 6) ava.terminal3
if [[ -f "$TERM3_VERSION_FILE" ]]; then
  echo "\n---\n⚙️  Updating Ava.Terminal3 version"
  sed -i -E "s#(VersionInfo *= *\")[^\"]*(\";)#\1${new_version}\2#" "$TERM3_VERSION_FILE"
  cp "$VERSION_FILE" "$TERM3_DIR/$VERSION_FILE"
  commit_submodule "$TERM3_DIR"
fi

# ⭐ Final root commit & tag — only after all submodules are done
echo "\n---\n🔄 Finalizing root repository"
git add .
git commit -m "v${new_version}" || echo "⚠️  No root changes"
git push origin HEAD:main --force && echo "✅  Root pushed to main"
git push origin HEAD:dev --force  && echo "✅  Root pushed to dev"

# tag and push root
root_tag="v${new_version}"
if ! git rev-parse "refs/tags/$root_tag" >/dev/null 2>&1; then
  git tag -s "$root_tag" -m "$root_tag" && echo "🏷  Tagged root $root_tag"
  git push origin "$root_tag"               && echo "✅  Pushed root tag $root_tag"
else
  echo "⚠️  Root tag $root_tag already exists—skipping"
fi

echo "\n🎉 All done! Ava is now at $new_version"

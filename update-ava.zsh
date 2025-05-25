#!/usr/bin/env zsh
# 🚀 update-ava.zsh — bump X.Y.Z in ava_version and sync all submodules

set -euo pipefail

# 🛠️ Configuration
VERSION_FILE="ava_version"

# 📁 Submodule directories & files
SHARED_DIR="ava.shared"
SHARED_VERSION_FILE="$SHARED_DIR/Models/Static/VersionInfo.cs"

API_DIR="ava.api"
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
  echo "📄 Created $VERSION_FILE with initial version 0.0.0"
fi

# 📥 Read the current version
current_version="$(< "$VERSION_FILE")"
echo "🔎 Current Ava version: $current_version"
IFS='.' read -r major minor patch <<< "$current_version"

# 🔧 Bump logic: --build (patch), --patch (minor+1, reset patch), --version (major+1, reset others)
mode="${1:---build}"
case "$mode" in
  --patch)
    (( minor++ ))
    patch=0
    echo "⚙️  Minor bump → new version will be: $major.$minor.$patch"
    ;;
  --version)
    (( major++ ))
    minor=0
    patch=0
    echo "🚀 Major bump → new version will be: $major.$minor.$patch"
    ;;
  --build|"")
    (( patch++ ))
    echo "🔨 Patch bump → new version will be: $major.$minor.$patch"
    ;;
  *)
    echo "❓ Usage: $0 [--build|--patch|--version]"
    exit 1
    ;;
esac

new_version="${major}.${minor}.${patch}"

# 💾 Write new version back to file
echo "$new_version" >| "$VERSION_FILE"
echo "✅ Updated $VERSION_FILE → $new_version"

# Function to commit & push in a submodule
commit_submodule() {
  local dir="$1"
  echo "🔄 Entering submodule: $dir"
  pushd "$dir" > /dev/null

  git add .
  git commit -m "v${new_version}" \
    || echo "⚠️  No changes to commit in $dir"

  # push main
  git push origin HEAD:main --force \
    && echo "✅ Pushed main → origin/main in $dir"

  # only force‐push to dev if it already exists
  if git ls-remote --exit-code --heads origin dev &>/dev/null; then
    git push origin HEAD:dev --force \
      && echo "✅ Pushed main → origin/dev in $dir"
  else
    echo "⚠️  Remote 'dev' branch not found in $dir—skipping dev push"
  fi

  # signed tag
  git tag -s "v${new_version}" -m "v${new_version}" \
    && echo "🏷 Created signed tag v${new_version}" \
    || echo "⚠️  Tag v${new_version} already exists"

  git push --tags \
    && echo "✅ Pushed tags in $dir"

  popd > /dev/null
  echo "🎉 Done with submodule $dir"
}

# 🧩 Update ava.shared
if [[ -f "$SHARED_VERSION_FILE" ]]; then
  echo "🔄 Updating version in $SHARED_VERSION_FILE"
  sed -i -E \
    's#(public static string ClientVersion *= *")[^"]*(";)#\1'"${new_version}"'\2#' \
    "$SHARED_VERSION_FILE"
  echo "✅ Updated shared version → $new_version"
  cp "$VERSION_FILE" "$SHARED_DIR/$VERSION_FILE"
  commit_submodule "$SHARED_DIR"
else
  echo "⚠️  $SHARED_VERSION_FILE not found—skipping shared submodule"
fi

# 🐙 Update ava.api Dockerfile in submodule
if [[ -f "$API_DOCKERFILE_SUB" ]]; then
  echo "🔄 Updating LABEL version in $API_DOCKERFILE_SUB"
  sed -i -E "s#^LABEL[[:space:]]*version=\"[^"]*\"#LABEL version=\"${new_version}\"#" "$API_DOCKERFILE_SUB"
  echo "✅ Updated API submodule Dockerfile version → $new_version"
  cp "$VERSION_FILE" "$API_DIR/$VERSION_FILE"
  commit_submodule "$API_DIR"
else
  echo "⚠️  $API_DOCKERFILE_SUB not found—skipping API submodule"
fi

# 🐳 Update local Dockerfile.API
if [[ -f "$API_DOCKERFILE_LOCAL" ]]; then
  echo "🔄 Updating LABEL version in local $API_DOCKERFILE_LOCAL"
  sed -i -E "s#^LABEL[[:space:]]*version=\"[^"]*\"#LABEL version=\"${new_version}\"#" "$API_DOCKERFILE_LOCAL"
  echo "✅ Updated local Dockerfile.API version → $new_version"
else
  echo "⚠️  $API_DOCKERFILE_LOCAL not found—skipping local Dockerfile"
fi

# 🐳 Update ava.api.docker compose.yaml
if [[ -f "$API_COMPOSE_FILE" ]]; then
  echo "🔄 Updating image tag in $API_COMPOSE_FILE"
  sed -i -E "s#(image:[[:space:]]*repasscloud/ava-api:).*#\1${new_version}#" "$API_COMPOSE_FILE"
  echo "✅ Updated compose image → repasscloud/ava-api:${new_version}"
  cp "$VERSION_FILE" "$API_DOCKER_DIR/$VERSION_FILE"
  commit_submodule "$API_DOCKER_DIR"
else
  echo "⚠️  $API_COMPOSE_FILE not found—skipping compose update"
fi

# 🚚 Update ava.deploy.docker
echo "🔄 Sync version file to $DEPLOY_DOCKER_DIR"
cp "$VERSION_FILE" "$DEPLOY_DOCKER_DIR/$VERSION_FILE"
echo "✅ Copied version file to deploy-docker submodule"
commit_submodule "$DEPLOY_DOCKER_DIR"

# 🖥️ Update ava.terminal3 version
if [[ -f "$TERM3_VERSION_FILE" ]]; then
  echo "🔄 Updating version in $TERM3_VERSION_FILE"
  sed -i -E \
    's#(public static readonly string VersionInfo *= *")[^"]*(";)#\1'"${new_version}"'\2#' \
    "$TERM3_VERSION_FILE"
  echo "✅ Updated terminal3 version → $new_version"
  cp "$VERSION_FILE" "$TERM3_DIR/$VERSION_FILE"
  commit_submodule "$TERM3_DIR"
else
  echo "⚠️  $TERM3_VERSION_FILE not found—skipping terminal3 submodule"
fi

# ⭐ Finalize in root repo
echo "🔄 Committing root version bump"
# commit root changes
git add .
git commit -m "v${new_version}" || echo "⚠️  No root changes to commit"
git push origin HEAD:main --force
git push origin main:dev --force

# 🏷 Tag and push root
echo "🏷 Creating signed tag v${new_version}"
git tag -s "v${new_version}" -m "v${new_version}"
git push origin "v${new_version}"

echo "🎉 All done! Ava version is now v${new_version}"

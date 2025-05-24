#!/usr/bin/env zsh
#
# update-ava.zsh — bump X.Y.Z in ava_version
#

# Variables
VERSION_FILE="ava_version"
AVAAPI_SUBMODULE_DIR="ava.api.docker"
AVAAPI_COMPOSE_FILE="${AVAAPI_SUBMODULE_DIR}/compose.yaml"

# Ensure file exists
if [[ ! -f $VERSION_FILE ]]; then
  echo "0.0.0" >| $VERSION_FILE
fi

# Read current version
version=$(< $VERSION_FILE)
IFS='.' read -r major minor patch <<< "$version"

# Bump logic
case "$1" in
  --patch)
    (( minor++ ))
    patch=0
    ;;
  --version)
    (( major++ ))
    minor=0
    patch=0
    ;;
  ""|--build)
    (( patch++ ))
    ;;
  *)
    echo "Usage: $0 [--build|--patch|--version]"
    exit 1
    ;;
esac

# Write back new version
new_version="${major}.${minor}.${patch}"
echo $new_version >| $VERSION_FILE

# Feedback
echo "Updated ava_version → $new_version"

# ava.api.docker
if [[ -f $AVAAPI_COMPOSE_FILE ]]; then
  echo "🔄 Updating image tag in $AVAAPI_COMPOSE_FILE"
  sed -i -E \
    "s#^([[:space:]]*image:[[:space:]]*repasscloud/ava-api:).*#\1${new_version}#" \
    "$AVAAPI_COMPOSE_FILE"
  echo "✅ Updated $AVAAPI_COMPOSE_FILE → image: repasscloud/ava-api:${new_version}"

  # ommit & push in submodule
  echo "🔄 Committing changes in $AVAAPI_SUBMODULE_DIR"
  pushd "$AVAAPI_SUBMODULE_DIR" > /dev/null
    git add .
    git commit -m "v${new_version}"
    git push
  popd > /dev/null
  echo "✅ Pushed submodule update"
else
  echo "⚠️  $AVAAPI_COMPOSE_FILE not found; skipping image update and commit"
fi

# Dockerfile.API
sed -i -E \
  "s#^LABEL[[:space:]]*version=\"[^\"]*\"#LABEL version=\"${new_version}\"#" \
  Dockerfile.API

# ava.platform
git add .
git commit -m "v${new_version}"
git push origin --force
git push origin main:dev --force

# docker containers build
git tag --annotate --sign "v${new_version}" -m "v${new_version}"
git push origin "v${new_version}"

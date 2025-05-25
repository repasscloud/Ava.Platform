#!/usr/bin/env zsh
#
# update-ava.zsh — bump X.Y.Z in ava_version
#

# Variables
VERSION_FILE="ava_version"

AVAAPI_SUBMODULE_DIR="ava.api"
AVAAPI_DCKR_FILE="${AVAAPI_SUBMODULE_DIR}/Dockerfile.API"
AVAAPI_DCKR_LOCAL_FILE="Dockerfile.API"

AVAAPI_DCKR_SUBMODULE_DIR="ava.api.docker"
AVAAPI_DCKR_COMPOSE_FILE="${AVAAPI_DCKR_SUBMODULE_DIR}/compose.yaml"

AVATERM3_SUBMODULE_DIR="ava.terminal3"
AVATERM3_VERSION_FILE="${AVATERM3_SUBMODULE_DIR}/Models/Static/AppVersion.cs"

AVASHARED_SUBMODULE_DIR="ava.shared"
AVASHARED_VERSION_FILE="${AVASHARED_SUBMODULE_DIR}/Models/Static/VersionInfo.cs"

AVADEPLOY_DCKR_SUBMODULE_DIR="ava.deploy.docker"


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


# ava.shared
if [[ -f $AVASHARED_VERSION_FILE ]]; then
  echo "🔄 Updating version in $AVASHARED_VERSION_FILE"
  sed -i -E \
    's#^([[:space:]]*public static string ClientVersion[[:space:]]*\{[^\}]*\}[[:space:]]*=[[:space:]]*)"[^"]*";$#\1"'"${new_version}"'";#' \
    "$AVASHARED_VERSION_FILE"
  echo "✅ Updated $AVASHARED_VERSION_FILE → ${new_version}"

  # commit & push in submodule
  echo "🔄 Committing changes in $AVASHARED_SUBMODULE_DIR"
  pushd "$AVASHARED_SUBMODULE_DIR" > /dev/null
    git add .
    git commit -m "v${new_version}"
    git push
    # git push origin main:dev --force
    git tag -s v${new_version}" -m v${new_version}"
    git push --tags
  popd > /dev/null
  echo "✅ Pushed submodule update"
else
  echo "⚠️  $AVASHARED_VERSION_FILE not found; skipping image update and commit"
fi


# ava.api
if [[ -f $AVAAPI_DCKR_FILE ]]; then
  echo "🔄 Updating version in $AVAAPI_DCKR_FILE"
  sed -i -E \
  "s#^LABEL[[:space:]]*version=\"[^\"]*\"#LABEL version=\"${new_version}\"#" \
    $AVAAPI_DCKR_FILE
  echo "✅ Updated $AVAAPI_DCKR_FILE → ${new_version}"

  # commit & push in submodule
  echo "🔄 Committing changes in $AVAAPI_SUBMODULE_DIR"
  pushd "$AVASHARED_SUBMODULE_DIR" > /dev/null
    git add .
    git commit -m "v${new_version}"
    git push
    git push origin main:dev --force
    git tag -s v${new_version}" -m v${new_version}"
    git push --tags
  popd > /dev/null
  echo "✅ Pushed submodule update"
else
  echo "⚠️  $AVAAPI_DCKR_FILE not found; skipping image update and commit"
fi

if [[ -f $AVAAPI_DCKR_LOCAL_FILE ]]; then
  echo "🔄 Updating version in $AVAAPI_DCKR_LOCAL_FILE"
  sed -i -E \
  "s#^LABEL[[:space:]]*version=\"[^\"]*\"#LABEL version=\"${new_version}\"#" \
    $AVAAPI_DCKR_LOCAL_FILE
  echo "✅ Updated $AVAAPI_DCKR_LOCAL_FILE → ${new_version}"
else
  echo "⚠️  $AVAAPI_DCKR_LOCAL_FILE not found; skipping image update and commit"
fi


# ava.api.docker
if [[ -f $AVAAPI_DCKR_COMPOSE_FILE ]]; then
  echo "🔄 Updating image tag in $AVAAPI_DCKR_COMPOSE_FILE"
  sed -i -E \
    "s#^([[:space:]]*image:[[:space:]]*repasscloud/ava-api:).*#\1${new_version}#" \
    "$AVAAPI_DCKR_COMPOSE_FILE"
  echo "✅ Updated $AVAAPI_DCKR_COMPOSE_FILE → image: repasscloud/ava-api:${new_version}"

  # commit & push in submodule
  echo "🔄 Committing changes in $AVAAPI_DCKR_SUBMODULE_DIR"
  pushd "$AVAAPI_DCKR_SUBMODULE_DIR" > /dev/null
    git add .
    git commit -m "v${new_version}"
    git push
    git tag -s v${new_version}" -m v${new_version}"
    git push --tags
  popd > /dev/null
  echo "✅ Pushed submodule update"
else
  echo "⚠️  $AVAAPI_DCKR_COMPOSE_FILE not found; skipping image update and commit"
fi


# ava.deploy.docker
cp "${VERSION_FILE}" "${AVADEPLOY_DCKR_SUBMODULE_DIR}/${VERSION_FILE}"
echo "🔄 Committing changes in $AAVADEPLOY_DCKR_SUBMODULE_DIR"
  pushd "$AVADEPLOY_DCKR_SUBMODULE_DIR" > /dev/null
    git add .
    git commit -m "v${new_version}"
    git push
    git tag -s v${new_version}" -m v${new_version}"
    git push --tags
  popd > /dev/null
echo "✅ Pushed submodule update"


# ava.terminal3
if [[ -f $AVATERM3_VERSION_FILE ]]; then
  echo "🔄 Updating version in $AVATERM3_VERSION_FILE"
  sed -i -E \
    's#^([[:space:]]*public static readonly string VersionInfo *= *)"[^"]*";$#\1"'"${new_version}"'";#' \
    "$AVATERM3_VERSION_FILE"
  echo "✅ Updated $AVATERM3_VERSION_FILE → ${new_version}"

  # commit & push in submodule
  echo "🔄 Committing changes in $AVATERM3_SUBMODULE_DIR"
  pushd "$AVATERM3_SUBMODULE_DIR" > /dev/null
    git add .
    git commit -m "v${new_version}"
    git push
    git push origin main:dev --force
    git tag -s v${new_version}" -m v${new_version}"
    git push --tags
  popd > /dev/null
  echo "✅ Pushed submodule update"
else
  echo "⚠️  $AVAAPI_DCKR_COMPOSE_FILE not found; skipping image update and commit"
fi


# ava.platform
git add .
git commit -m "v${new_version}"
git push origin --force
git push origin main:dev --force

# docker containers build
git tag --annotate --sign "v${new_version}" -m "v${new_version}"
git push origin "v${new_version}"


#!/usr/bin/env sh

# Define container and volume names
CONTAINER_NAME="pg_ava_api_dev"
VOLUME_NAME="pg_ava_api_dev_data"
PROJECT_PATH="./"  # Set to your .NET project path if not root
MIGRATION_NAME="InitialCreate"

echo "🛑 Stopping and removing existing PostgreSQL container ($CONTAINER_NAME)..."
docker stop $CONTAINER_NAME 2>/dev/null
docker rm $CONTAINER_NAME 2>/dev/null

echo "🗑️  Removing existing volume ($VOLUME_NAME)..."
docker volume rm $VOLUME_NAME 2>/dev/null

echo "📦 Creating a new volume ($VOLUME_NAME)..."
docker volume create $VOLUME_NAME

echo "🚀 Starting a new PostgreSQL container ($CONTAINER_NAME) with host networking..."
docker run --name $CONTAINER_NAME \
    -e POSTGRES_USER=postgres \
    -e POSTGRES_PASSWORD=devpassword \
    -e POSTGRES_DB=ava_db \
    --network pg_network \
    -v $VOLUME_NAME:/var/lib/postgresql/data \
    -d postgres:17.2

echo "⏳ Waiting for PostgreSQL to be ready..."
sleep 5  # Give it time to start

echo "⏳ Checking PostgreSQL readiness..."
until docker exec -it $CONTAINER_NAME pg_isready -U postgres > /dev/null 2>&1; do
  echo "⏳ PostgreSQL is not ready yet. Retrying..."
  sleep 2
done
echo "✅ PostgreSQL is ready!"


# # Move to the .NET project directory
# cd "$PROJECT_PATH"

# echo "📦 Restoring .NET dependencies..."
# dotnet restore

# echo "🔄 Removing old migrations..."
# rm -rf "$PROJECT_PATH/Migrations"

# echo "🚀 Creating new migrations..."
# dotnet ef migrations add $MIGRATION_NAME --project "$PROJECT_PATH"

# echo "📂 Applying migrations to database..."
# dotnet ef database update --project "$PROJECT_PATH"

# echo "🚀 Starting the API with 'dotnet run'..."
# dotnet run

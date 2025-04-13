#!/usr/bin/env zsh

# Exit on error
set -e

# Resolve to script's directory
SCRIPT_DIR="${0:A:h}"
cd "$SCRIPT_DIR"

echo "📁 Working from: $SCRIPT_DIR"

# Clean, restore, and build
dotnet clean
rm -rf ./obj ./bin
dotnet build
dotnet run -f net9.0-maccatalyst

#dotnet publish -f net9.0-maccatalyst -c Release -o ./publish/macos
#dotnet publish -f net9.0-windows10.0.19041.0 -c Release -o ./publish/windows

# macOS self-contained (Intel or ARM)
#dotnet publish -f net9.0-maccatalyst -c Release -r osx-arm64 --self-contained true -o ./publish/self/macos

# Windows self-contained
#dotnet publish -f net9.0-windows10.0.19041.0 -c Release -r win-x64 --self-contained true -o ./publish/self/windows

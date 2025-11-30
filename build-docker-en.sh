#!/bin/bash

# AirdPro Docker Build Script
# For macOS environment (using Wine to run .NET Framework applications)

echo "Starting AirdPro Docker image build..."
echo "Note: This image uses Wine to run .NET Framework applications on macOS"

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "Error: Docker is not installed. Please install Docker Desktop for Mac first"
    echo "Download URL: https://www.docker.com/products/docker-desktop"
    exit 1
fi

# Check Docker service status
if ! docker info &> /dev/null; then
    echo "Error: Docker service is not running. Please start Docker Desktop"
    exit 1
fi

# Build macOS version image (using Wine)
echo "Building macOS version image (using Wine to run .NET Framework)..."
docker build --target runtime-macos -t airdpro:macos .

if [ $? -eq 0 ]; then
    echo "✅ macOS version image built successfully"
else
    echo "❌ macOS version image build failed"
    echo "Tip: Wine installation may take longer time, please be patient"
    exit 1
fi

# Build CLI version image
echo "Building CLI version image..."
docker build --target runtime-macos -t airdpro:cli .

if [ $? -eq 0 ]; then
    echo "✅ CLI version image built successfully"
else
    echo "❌ CLI version image build failed"
    exit 1
fi

# Build development version image
echo "Building development version image..."
docker build --target runtime-macos -t airdpro:dev .

if [ $? -eq 0 ]; then
    echo "✅ Development version image built successfully"
else
    echo "❌ Development version image build failed"
    exit 1
fi

echo ""
echo "🎉 All Docker images built successfully!"
echo ""
echo "Available images:"
echo "  - airdpro:macos    (GUI version, requires XQuartz)"
echo "  - airdpro:cli      (Command-line version)"
echo "  - airdpro:dev      (Development version)"
echo ""
echo "Run commands:"
echo "  ./run-gui-en.sh       # Run GUI version (requires XQuartz installation)"
echo "  ./run-cli-en.sh       # Run command-line version"
echo ""
echo "Important notes:"
echo "1. GUI version requires XQuartz: brew install --cask xquartz"
echo "2. First Wine run requires longer initialization time for .NET Framework"
echo "3. Performance may not match native Windows environment"
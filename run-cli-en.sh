#!/bin/bash

# AirdPro CLI Run Script
# For macOS environment (using Wine to run .NET Framework applications)

echo "Starting AirdPro command-line version..."
echo "Note: This version uses Wine to run .NET Framework applications on macOS"

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

# Check if image exists
if ! docker image inspect airdpro:cli &> /dev/null; then
    echo "Error: airdpro:cli image does not exist. Please run ./build-docker-en.sh first"
    exit 1
fi

# Display usage help
if [[ "$1" == "--help" || "$1" == "-h" ]]; then
    echo "Usage: $0 [command-line arguments]"
    echo ""
    echo "Examples:"
    echo "  $0 --help                    # Display help"
    echo "  $0 --version                 # Display version information"
    echo "  $0 -i input.raw -o output.mzML  # File conversion"
    echo ""
    echo "Note: This version uses Wine to run, performance may not match native Windows environment"
    echo "First run requires Wine and .NET Framework initialization, please be patient"
    echo ""
    echo "All arguments will be passed to AirdPro application"
    exit 0
fi

# Create necessary directories
mkdir -p data logs

echo ""
echo "Starting AirdPro CLI container (using Wine to run .NET Framework)..."
echo "First run may take longer to initialize Wine and .NET Framework..."
echo ""

# Run CLI container
docker run -it --rm \
    --name airdpro-cli \
    -v "$(pwd)/data":/data \
    -v "$(pwd)/logs":/app/logs \
    -e WINEPREFIX=/wine \
    -e WINEARCH=win64 \
    -e WINEDEBUG=-all \
    airdpro:cli "$@"

echo ""
echo "AirdPro CLI has exited"
echo "Log files location: $(pwd)/logs/"
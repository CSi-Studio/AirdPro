#!/bin/bash

# AirdPro Docker Configuration Test Script (macOS environment)
# Verify Docker and Wine configurations are correct

echo "Starting AirdPro Docker configuration test (macOS + Wine)..."
echo "================================================"

# 1. Check if Docker is installed
echo "1. Checking Docker installation..."
if command -v docker &> /dev/null; then
    echo "✅ Docker is installed"
    docker --version
else
    echo "❌ Docker is not installed"
    echo "Please install Docker Desktop for Mac: https://www.docker.com/products/docker-desktop"
    exit 1
fi

# 2. Check Docker service status
echo ""
echo "2. Checking Docker service status..."
if docker info &> /dev/null; then
    echo "✅ Docker service is running normally"
else
    echo "❌ Docker service is not running"
    echo "Please start Docker Desktop application"
    exit 1
fi

# 3. Check if images exist
echo ""
echo "3. Checking Docker images..."
images=("airdpro:macos" "airdpro:cli" "airdpro:dev")

for image in "${images[@]}"; do
    if docker image inspect "$image" &> /dev/null; then
        echo "✅ $image image exists"
    else
        echo "❌ $image image does not exist"
        echo "   Please run: ./build-docker-en.sh"
    fi
done

# 4. Test Wine configuration
echo ""
echo "4. Testing Wine configuration..."
echo "Starting test container to check Wine and .NET Framework..."
if docker run --rm airdpro:macos wine --version &> /dev/null; then
    echo "✅ Wine installation is normal"
    docker run --rm airdpro:macos wine --version
else
    echo "❌ Wine configuration test failed"
    echo "Image may need to be rebuilt"
fi

# 5. Test .NET Framework application startup
echo ""
echo "5. Testing .NET Framework application startup..."
if docker run --rm airdpro:cli --version &> /dev/null; then
    echo "✅ .NET Framework application startup test passed"
else
    echo "⚠️  .NET Framework application startup test failed (may require first-run initialization)"
    echo "First Wine run requires longer initialization time for .NET Framework"
fi

# 6. Check macOS specific X11 configuration
echo ""
echo "6. Checking macOS X11 configuration..."
if [[ "$OSTYPE" == "darwin"* ]]; then
    if command -v xquartz &> /dev/null || [ -d "/Applications/Utilities/XQuartz.app" ]; then
        echo "✅ XQuartz is installed"
        
        if ps aux | grep -v grep | grep -q "XQuartz"; then
            echo "✅ XQuartz is running"
            echo "X11 display: host.docker.internal:0"
        else
            echo "⚠️  XQuartz is not running, GUI version requires XQuartz to be started"
            echo "Run: open -a XQuartz"
        fi
    else
        echo "❌ XQuartz is not installed, GUI version cannot run"
        echo "Install command: brew install --cask xquartz"
    fi
else
    echo "ℹ️  Non-macOS environment, skipping X11 check"
fi

# 7. Check directory structure
echo ""
echo "7. Checking directory structure..."
directories=("data" "logs")

for dir in "${directories[@]}"; do
    if [ -d "$dir" ]; then
        echo "✅ $dir directory exists"
    else
        echo "⚠️  $dir directory does not exist, will be created automatically"
        mkdir -p "$dir"
    fi
done

# 8. Set script permissions
echo ""
echo "8. Setting script permissions..."
scripts=("build-docker-en.sh" "run-gui-en.sh" "run-cli-en.sh" "test-docker-en.sh")

for script in "${scripts[@]}"; do
    if [ -f "$script" ]; then
        chmod +x "$script"
        echo "✅ $script permissions set successfully"
    else
        echo "❌ $script file does not exist"
    fi
done

echo ""
echo "================================================"
echo "Test completed!"
echo ""
echo "Next steps:"
echo "1. Build images (first time): ./build-docker-en.sh"
echo "2. Run GUI version: ./run-gui-en.sh"
echo "3. Run CLI version: ./run-cli-en.sh --help"
echo ""
echo "Important notes:"
echo "- First run requires longer initialization time for Wine and .NET Framework"
echo "- GUI version requires XQuartz installation and running"
echo "- Performance may not match native Windows environment"
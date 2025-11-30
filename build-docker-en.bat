@echo off
REM AirdPro Docker Build Script (Windows environment)

echo Starting AirdPro Docker image build...

REM Check if Docker is installed
docker --version >nul 2>&1
if errorlevel 1 (
    echo Error: Docker is not installed. Please install Docker Desktop first
    pause
    exit /b 1
)

REM Build Windows version image
echo Building Windows version image...
docker build --target runtime-windows -t airdpro:windows .

if errorlevel 1 (
    echo ❌ Windows version image build failed
    pause
    exit /b 1
) else (
    echo ✅ Windows version image built successfully
)

echo.
echo 🎉 Docker image build completed!
echo.
echo Available images:
echo   - airdpro:windows  (Windows container version)
echo.
echo Run command:
echo   run-windows-en.bat     # Run Windows version

pause
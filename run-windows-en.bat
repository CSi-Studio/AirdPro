@echo off
REM AirdPro Windows Container Run Script

echo Starting AirdPro Windows version...

REM Create necessary directories
if not exist data mkdir data
if not exist logs mkdir logs

REM Run Windows container
echo Starting Docker container...
docker run -it --rm ^
    --name airdpro-windows ^
    -v "%cd%\data:/data" ^
    -v "%cd%\logs:/app/logs" ^
    -e DOTNET_RUNNING_IN_CONTAINER=true ^
    --platform windows/amd64 ^
    airdpro:windows

echo AirdPro Windows version has exited
pause
@echo off
REM AirdPro Windows容器运行脚本

echo 启动AirdPro Windows版本...

REM 创建必要的目录
if not exist data mkdir data
if not exist logs mkdir logs

REM 运行Windows容器
echo 启动Docker容器...
docker run -it --rm ^
    --name airdpro-windows ^
    -v "%cd%\data:/data" ^
    -v "%cd%\logs:/app/logs" ^
    -e DOTNET_RUNNING_IN_CONTAINER=true ^
    --platform windows/amd64 ^
    airdpro:windows

echo AirdPro Windows版本已退出
pause
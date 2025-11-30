@echo off
REM AirdPro跨平台Docker构建脚本

echo =============================================
echo   AirdPro跨平台Docker构建脚本
echo =============================================
echo.

REM 检查Docker是否安装
echo [1/5] 检查Docker环境...
docker --version >nul 2>&1
if errorlevel 1 (
    echo ❌ 错误：Docker未安装。请先安装Docker Desktop
    echo 下载地址：https://www.docker.com/products/docker-desktop
    pause
    exit /b 1
) else (
    echo ✅ Docker已安装
)

REM 检查Docker服务状态
docker info >nul 2>&1
if errorlevel 1 (
    echo ❌ 错误：Docker服务未运行。请启动Docker Desktop
    pause
    exit /b 1
) else (
    echo ✅ Docker服务正在运行
)

echo.
echo [2/5] 开始构建多平台镜像...
echo 这可能需要较长时间，请耐心等待...
echo.

REM 构建Windows版本
echo 📦 构建Windows版本镜像...
docker build --target runtime-windows -t airdpro:windows .
if errorlevel 1 (
    echo ❌ Windows版本镜像构建失败
    pause
    exit /b 1
) else (
    echo ✅ Windows版本镜像构建成功
)

echo.

REM 构建Linux/macOS版本（使用Wine）
echo 📦 构建Linux/macOS版本镜像...
docker build --target runtime-linux -t airdpro:linux .
if errorlevel 1 (
    echo ❌ Linux/macOS版本镜像构建失败
    pause
    exit /b 1
) else (
    echo ✅ Linux/macOS版本镜像构建成功
)

echo.

REM 构建CLI版本
echo 📦 构建CLI版本镜像...
docker build --target runtime-linux -t airdpro:cli --build-arg CLI_MODE=true .
if errorlevel 1 (
    echo ❌ CLI版本镜像构建失败
    pause
    exit /b 1
) else (
    echo ✅ CLI版本镜像构建成功
)

echo.

REM 构建开发版本
echo 📦 构建开发版本镜像...
docker build --target runtime-linux -t airdpro:dev --build-arg DEV_MODE=true .
if errorlevel 1 (
    echo ❌ 开发版本镜像构建失败
    pause
    exit /b 1
) else (
    echo ✅ 开发版本镜像构建成功
)

echo.
echo [3/5] 创建必要目录...
if not exist "data" mkdir data
if not exist "logs" mkdir logs
echo ✅ 目录创建完成

echo.
echo [4/5] 显示构建结果...
echo.
docker images | findstr "airdpro"

echo.
echo [5/5] 构建完成！
echo =============================================
echo 🎉 AirdPro跨平台Docker镜像构建成功！
echo =============================================
echo.
echo 可用镜像：
echo   • airdpro:windows  - Windows原生容器版本
echo   • airdpro:linux    - Linux/macOS版本（使用Wine）
echo   • airdpro:cli      - 命令行版本（无GUI）
echo   • airdpro:dev      - 开发版本（带调试）
echo.
echo 运行命令：
echo   run-windows.bat    # 运行Windows版本
echo   run-gui-en.sh      # 运行GUI版本（macOS/Linux）
echo   run-cli-en.sh      # 运行CLI版本
echo   test-docker-en.sh  # 运行测试脚本
echo.
echo 更多详细信息请查看 CROSS-PLATFORM-GUIDE.md
echo.

pause
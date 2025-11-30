@echo off
REM AirdPro macOS Docker镜像构建脚本（Windows环境）
REM 专门为macOS平台构建兼容的Docker镜像

echo ========================================
echo    AirdPro macOS Docker镜像构建工具
echo ========================================
echo.

REM 检查Docker是否安装
docker --version >nul 2>&1
if errorlevel 1 (
    echo ❌ 错误: Docker未安装
    echo.
    echo 请按以下步骤操作:
    echo 1. 下载Docker Desktop: https://www.docker.com/products/docker-desktop
    echo 2. 安装并启动Docker Desktop
    echo 3. 确保启用WSL 2后端
    echo 4. 切换到Linux容器模式
    echo.
    echo 安装完成后重新运行此脚本
    pause
    exit /b 1
)

echo ✅ Docker已安装:
docker --version
echo.

REM 检查Docker服务状态
echo 检查Docker服务状态...
docker info >nul 2>&1
if errorlevel 1 (
    echo ❌ Docker服务未运行
    echo 请启动Docker Desktop应用程序
    pause
    exit /b 1
)

echo ✅ Docker服务运行正常
echo.

REM 检查构建环境
echo 检查构建环境配置...

REM 检查是否支持多平台构建
docker buildx version >nul 2>&1
if errorlevel 1 (
    echo ⚠️  Docker Buildx未启用，将使用标准构建
    echo 建议启用Buildx以获得更好的多平台支持
) else (
    echo ✅ Docker Buildx已启用
)

echo.
echo ========================================
echo           开始构建macOS镜像
echo ========================================
echo.

echo 构建说明:
echo - 目标平台: macOS (使用Wine运行.NET Framework)
echo - 基础镜像: Ubuntu 22.04
echo - 运行时: Wine + .NET Framework 4.8
echo - 预计时间: 首次构建可能需要30-60分钟
echo.

echo 注意: 构建过程需要下载大量依赖，请确保网络连接稳定
echo.

REM 确认开始构建
set /p confirm="是否开始构建? (y/n): "
if /i not "%confirm%"=="y" if /i not "%confirm%"=="yes" (
    echo 构建已取消
    pause
    exit /b 0
)

echo.
echo 🚀 开始构建macOS版本镜像...
echo.

REM 构建macOS版本镜像（使用Wine）
echo 步骤1: 构建macOS基础镜像...
docker build --target runtime-macos -t airdpro:macos .

if errorlevel 1 (
    echo.
    echo ❌ macOS镜像构建失败
    echo.
    echo 可能的原因:
    echo - 网络连接问题
    echo - Docker资源不足
    echo - 构建缓存问题
    echo.
    echo 解决方案:
    echo 1. 检查网络连接
    echo 2. 增加Docker内存分配(建议8GB+)
    echo 3. 清理Docker缓存: docker system prune -f
    echo 4. 重新运行构建
    pause
    exit /b 1
)

echo.
echo ✅ macOS基础镜像构建成功
echo.

REM 构建CLI版本
echo 步骤2: 构建CLI版本镜像...
docker build --target runtime-macos -t airdpro:cli .

if errorlevel 1 (
    echo ❌ CLI镜像构建失败
    pause
    exit /b 1
)

echo ✅ CLI版本镜像构建成功
echo.

REM 构建开发版本
echo 步骤3: 构建开发版本镜像...
docker build --target runtime-macos -t airdpro:dev .

if errorlevel 1 (
    echo ❌ 开发版本镜像构建失败
    pause
    exit /b 1
)

echo ✅ 开发版本镜像构建成功
echo.

echo ========================================
echo           构建完成总结
echo ========================================
echo.

echo 🎉 所有macOS兼容镜像构建完成!
echo.

echo 可用镜像列表:
echo.
echo 📦 airdpro:macos    - GUI版本 (macOS + Wine)
echo 📦 airdpro:cli      - 命令行版本
echo 📦 airdpro:dev      - 开发版本
echo.

echo 镜像导出说明:
echo.
echo 1. 导出镜像到文件:
echo    docker save -o airdpro-macos.tar airdpro:macos
echo.
echo 2. 在macOS上导入镜像:
echo    docker load -i airdpro-macos.tar
echo.
echo 3. 运行说明:
echo    - macOS需要安装XQuartz: brew install --cask xquartz
echo    - 首次运行需要较长时间初始化Wine
echo    - 使用提供的英文版脚本运行
echo.

echo 下一步操作:
echo 1. 将镜像文件传输到macOS设备
echo 2. 在macOS上导入镜像: docker load -i <镜像文件>
echo 3. 运行测试: ./test-docker-en.sh
echo 4. 运行应用: ./run-gui-en.sh 或 ./run-cli-en.sh
echo.

pause
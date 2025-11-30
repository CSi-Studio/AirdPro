@echo off
REM AirdPro Docker构建脚本（Windows环境）

echo 开始构建AirdPro Docker镜像...

REM 检查Docker是否安装
docker --version >nul 2>&1
if errorlevel 1 (
    echo 错误: Docker未安装，请先安装Docker Desktop
    pause
    exit /b 1
)

REM 构建Windows版本镜像
echo 构建Windows版本镜像...
docker build --target runtime-windows -t airdpro:windows .

if errorlevel 1 (
    echo ❌ Windows版本镜像构建失败
    pause
    exit /b 1
) else (
    echo ✅ Windows版本镜像构建成功
)

echo.
echo 🎉 Docker镜像构建完成！
echo.
echo 可用镜像:
echo   - airdpro:windows  (Windows容器版本)
echo.
echo 运行命令:
echo   run-windows.bat     # 运行Windows版本

pause
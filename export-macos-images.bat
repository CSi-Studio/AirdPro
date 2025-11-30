@echo off
REM AirdPro macOS镜像导出脚本
REM 将构建好的Docker镜像导出为文件，便于传输到macOS

echo ========================================
echo      AirdPro macOS镜像导出工具
echo ========================================
echo.

REM 检查Docker是否安装
docker --version >nul 2>&1
if errorlevel 1 (
    echo ❌ 错误: Docker未安装
    echo 请先安装Docker Desktop
    pause
    exit /b 1
)

echo 检查镜像是否存在...
echo.

set images_found=0

REM 检查macOS镜像
if exist "airdpro:macos" (
    docker image inspect airdpro:macos >nul 2>&1
    if errorlevel 1 (
        echo ❌ airdpro:macos镜像不存在
    ) else (
        echo ✅ airdpro:macos镜像存在
        set /a images_found+=1
    )
) else (
    docker image inspect airdpro:macos >nul 2>&1
    if errorlevel 1 (
        echo ❌ airdpro:macos镜像不存在
    ) else (
        echo ✅ airdpro:macos镜像存在
        set /a images_found+=1
    )
)

REM 检查CLI镜像
docker image inspect airdpro:cli >nul 2>&1
if errorlevel 1 (
    echo ❌ airdpro:cli镜像不存在
) else (
    echo ✅ airdpro:cli镜像存在
    set /a images_found+=1
)

REM 检查开发镜像
docker image inspect airdpro:dev >nul 2>&1
if errorlevel 1 (
    echo ❌ airdpro:dev镜像不存在
) else (
    echo ✅ airdpro:dev镜像存在
    set /a images_found+=1
)

echo.

if %images_found% == 0 (
    echo ❌ 没有找到任何AirdPro镜像
    echo.
    echo 请先运行构建脚本:
    echo   build-macos-image.bat
    pause
    exit /b 1
)

echo 发现 %images_found% 个镜像可用于导出
echo.

REM 创建导出目录
if not exist "export" mkdir export
echo 📁 导出目录: %cd%\export
echo.

echo 镜像导出选项:
echo 1. 导出所有镜像 (推荐)
echo 2. 仅导出GUI版本 (airdpro:macos)
echo 3. 仅导出CLI版本 (airdpro:cli)
echo 4. 自定义选择
echo.

set /p choice="请选择导出选项 (1-4): "

echo.
echo ========================================
echo           开始导出镜像
echo ========================================
echo.

set export_all=0
set export_gui=0
set export_cli=0
set export_dev=0

if "%choice%"=="1" (
    set export_all=1
    echo 🚀 导出所有镜像...
) else if "%choice%"=="2" (
    set export_gui=1
    echo 🚀 导出GUI版本镜像...
) else if "%choice%"=="3" (
    set export_cli=1
    echo 🚀 导出CLI版本镜像...
) else if "%choice%"=="4" (
    echo 自定义选择导出镜像:
    set /p export_gui="导出GUI版本 (y/n): "
    set /p export_cli="导出CLI版本 (y/n): "
    set /p export_dev="导出开发版本 (y/n): "
) else (
    echo ❌ 无效选择，默认导出所有镜像
    set export_all=1
)

if "%export_all%"=="1" (
    set export_gui=1
    set export_cli=1
    set export_dev=1
)

echo.

REM 导出镜像
if "%export_gui%"=="1" (
    echo 📦 导出GUI版本镜像 (airdpro:macos)...
    docker save -o export\airdpro-macos.tar airdpro:macos
    if errorlevel 1 (
        echo ❌ GUI镜像导出失败
    ) else (
        echo ✅ GUI镜像导出成功
        for /f %%i in ('dir export\airdpro-macos.tar ^| find "airdpro-macos.tar"') do echo 文件大小: %%i
    )
    echo.
)

if "%export_cli%"=="1" (
    echo 📦 导出CLI版本镜像 (airdpro:cli)...
    docker save -o export\airdpro-cli.tar airdpro:cli
    if errorlevel 1 (
        echo ❌ CLI镜像导出失败
    ) else (
        echo ✅ CLI镜像导出成功
        for /f %%i in ('dir export\airdpro-cli.tar ^| find "airdpro-cli.tar"') do echo 文件大小: %%i
    )
    echo.
)

if "%export_dev%"=="1" (
    echo 📦 导出开发版本镜像 (airdpro:dev)...
    docker save -o export\airdpro-dev.tar airdpro:dev
    if errorlevel 1 (
        echo ❌ 开发镜像导出失败
    ) else (
        echo ✅ 开发镜像导出成功
        for /f %%i in ('dir export\airdpro-dev.tar ^| find "airdpro-dev.tar"') do echo 文件大小: %%i
    )
    echo.
)

echo ========================================
echo           导出完成总结
echo ========================================
echo.

echo 📊 导出统计:
for /f %%i in ('dir export\*.tar ^| find ".tar" /c') do set file_count=%%i
echo 导出文件数量: %file_count%
echo.

echo 📁 导出文件位置: %cd%\export\
echo.

dir export\*.tar
echo.

echo 🔄 macOS导入说明:
echo.
echo 1. 将export目录下的.tar文件传输到macOS
echo 2. 在macOS终端中运行以下命令导入:
echo.
echo   # 导入GUI版本
echo   docker load -i airdpro-macos.tar
echo.
echo   # 导入CLI版本  
echo   docker load -i airdpro-cli.tar
echo.
echo   # 导入开发版本
echo   docker load -i airdpro-dev.tar
echo.

echo 3. 验证镜像导入:
echo    docker images | grep airdpro
echo.

echo 4. 运行应用:
echo    ./run-gui-en.sh    # GUI版本
echo    ./run-cli-en.sh    # CLI版本
echo.

echo 💡 提示: 镜像文件较大，建议使用高速传输工具
echo.

pause
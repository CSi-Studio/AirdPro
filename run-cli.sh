#!/bin/bash

# AirdPro CLI运行脚本
# 适用于macOS环境（使用Wine运行.NET Framework应用）

echo "启动AirdPro命令行版本..."
echo "注意：此版本使用Wine在macOS上运行.NET Framework应用程序"

# 检查Docker是否安装
if ! command -v docker &> /dev/null; then
    echo "错误: Docker未安装，请先安装Docker Desktop for Mac"
    echo "下载地址: https://www.docker.com/products/docker-desktop"
    exit 1
fi

# 检查Docker服务状态
if ! docker info &> /dev/null; then
    echo "错误: Docker服务未运行，请启动Docker Desktop"
    exit 1
fi

# 检查镜像是否存在
if ! docker image inspect airdpro:cli &> /dev/null; then
    echo "错误: airdpro:cli镜像不存在，请先运行 ./build-docker.sh"
    exit 1
fi

# 显示使用帮助
if [[ "$1" == "--help" || "$1" == "-h" ]]; then
    echo "用法: $0 [命令行参数]"
    echo ""
    echo "示例:"
    echo "  $0 --help                    # 显示帮助"
    echo "  $0 --version                 # 显示版本信息"
    echo "  $0 -i input.raw -o output.mzML  # 文件转换"
    echo ""
    echo "注意：此版本使用Wine运行，性能可能不如原生Windows环境"
    echo "首次运行需要初始化Wine和.NET Framework，请耐心等待"
    echo ""
    echo "所有参数将传递给AirdPro应用程序"
    exit 0
fi

# 创建必要的目录
mkdir -p data logs

echo ""
echo "启动AirdPro CLI容器（使用Wine运行.NET Framework）..."
echo "首次运行可能需要较长时间初始化Wine和.NET Framework..."
echo ""

# 运行CLI容器
docker run -it --rm \
    --name airdpro-cli \
    -v "$(pwd)/data":/data \
    -v "$(pwd)/logs":/app/logs \
    -e WINEPREFIX=/wine \
    -e WINEARCH=win64 \
    -e WINEDEBUG=-all \
    airdpro:cli "$@"

echo ""
echo "AirdPro CLI已退出"
echo "日志文件位置: $(pwd)/logs/"
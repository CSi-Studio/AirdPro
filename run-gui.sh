#!/bin/bash

# AirdPro GUI运行脚本
# 适用于macOS环境（使用Wine运行.NET Framework应用）

echo "启动AirdPro GUI版本..."
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
if ! docker image inspect airdpro:macos &> /dev/null; then
    echo "错误: airdpro:macos镜像不存在，请先运行 ./build-docker.sh"
    exit 1
fi

# macOS特定的X11配置
if [[ "$OSTYPE" == "darwin"* ]]; then
    echo "检测到macOS环境，配置X11显示..."
    
    # 检查XQuartz是否安装
    if ! command -v xquartz &> /dev/null && [ ! -d "/Applications/Utilities/XQuartz.app" ]; then
        echo "错误: XQuartz未安装，GUI无法显示"
        echo "请安装XQuartz: brew install --cask xquartz"
        echo "安装后需要重启终端并运行: open -a XQuartz"
        exit 1
    fi
    
    # 检查XQuartz是否运行
    if ! ps aux | grep -v grep | grep -q "XQuartz"; then
        echo "启动XQuartz..."
        open -a XQuartz
        echo "等待XQuartz启动..."
        sleep 5
    fi
    
    # 设置macOS特定的X11显示
    export DISPLAY=host.docker.internal:0
    echo "X11显示设置为: $DISPLAY"
    
    # 允许来自Docker容器的X11连接
    xhost +localhost
fi

# 创建必要的目录
mkdir -p data logs

echo ""
echo "启动AirdPro GUI容器（使用Wine运行.NET Framework）..."
echo "首次运行可能需要较长时间初始化Wine和.NET Framework..."
echo ""

# 运行GUI容器
docker run -it --rm \
    --name airdpro-gui \
    -v "$(pwd)/data":/data \
    -v "$(pwd)/logs":/app/logs \
    -v /tmp/.X11-unix:/tmp/.X11-unix \
    -e DISPLAY=$DISPLAY \
    -e WINEPREFIX=/wine \
    -e WINEARCH=win64 \
    -e WINEDEBUG=-all \
    --privileged \
    airdpro:macos

echo ""
echo "AirdPro GUI已退出"
echo "日志文件位置: $(pwd)/logs/"
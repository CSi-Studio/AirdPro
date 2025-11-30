#!/bin/bash

# AirdPro Docker构建脚本
# 适用于macOS环境（使用Wine运行.NET Framework应用）

echo "开始构建AirdPro Docker镜像..."
echo "注意：此镜像使用Wine在macOS上运行.NET Framework应用程序"

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

# 构建macOS版本镜像（使用Wine）
echo "构建macOS版本镜像（使用Wine运行.NET Framework）..."
docker build --target runtime-macos -t airdpro:macos .

if [ $? -eq 0 ]; then
    echo "✅ macOS版本镜像构建成功"
else
    echo "❌ macOS版本镜像构建失败"
    echo "提示：Wine安装可能需要较长时间，请耐心等待"
    exit 1
fi

# 构建CLI版本镜像
echo "构建CLI版本镜像..."
docker build --target runtime-macos -t airdpro:cli .

if [ $? -eq 0 ]; then
    echo "✅ CLI版本镜像构建成功"
else
    echo "❌ CLI版本镜像构建失败"
    exit 1
fi

# 构建开发版本镜像
echo "构建开发版本镜像..."
docker build --target runtime-macos -t airdpro:dev .

if [ $? -eq 0 ]; then
    echo "✅ 开发版本镜像构建成功"
else
    echo "❌ 开发版本镜像构建失败"
    exit 1
fi

echo ""
echo "🎉 所有Docker镜像构建完成！"
echo ""
echo "可用镜像:"
echo "  - airdpro:macos    (GUI版本，需要XQuartz)"
echo "  - airdpro:cli      (命令行版本)"
echo "  - airdpro:dev      (开发版本)"
echo ""
echo "运行命令:"
echo "  ./run-gui.sh       # 运行GUI版本（需要先安装XQuartz）"
echo "  ./run-cli.sh       # 运行命令行版本"
echo ""
echo "重要提示："
echo "1. GUI版本需要安装XQuartz: brew install --cask xquartz"
echo "2. 首次运行Wine需要较长时间初始化.NET Framework"
echo "3. 性能可能不如原生Windows环境"
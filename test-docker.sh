#!/bin/bash

# AirdPro Docker配置测试脚本（macOS环境）
# 验证Docker和Wine配置是否正确

echo "开始测试AirdPro Docker配置（macOS + Wine）..."
echo "================================================"

# 1. 检查Docker是否安装
echo "1. 检查Docker安装..."
if command -v docker &> /dev/null; then
    echo "✅ Docker已安装"
    docker --version
else
    echo "❌ Docker未安装"
    echo "请安装Docker Desktop for Mac: https://www.docker.com/products/docker-desktop"
    exit 1
fi

# 2. 检查Docker服务状态
echo ""
echo "2. 检查Docker服务状态..."
if docker info &> /dev/null; then
    echo "✅ Docker服务运行正常"
else
    echo "❌ Docker服务未运行"
    echo "请启动Docker Desktop应用程序"
    exit 1
fi

# 3. 检查镜像是否存在
echo ""
echo "3. 检查Docker镜像..."
images=("airdpro:macos" "airdpro:cli" "airdpro:dev")

for image in "${images[@]}"; do
    if docker image inspect "$image" &> /dev/null; then
        echo "✅ $image 镜像存在"
    else
        echo "❌ $image 镜像不存在"
        echo "   请运行: ./build-docker.sh"
    fi
done

# 4. 测试Wine配置
echo ""
echo "4. 测试Wine配置..."
echo "启动测试容器检查Wine和.NET Framework..."
if docker run --rm airdpro:macos wine --version &> /dev/null; then
    echo "✅ Wine安装正常"
    docker run --rm airdpro:macos wine --version
else
    echo "❌ Wine配置测试失败"
    echo "可能需要重新构建镜像"
fi

# 5. 测试.NET Framework应用启动
echo ""
echo "5. 测试.NET Framework应用启动..."
if docker run --rm airdpro:cli --version &> /dev/null; then
    echo "✅ .NET Framework应用启动测试通过"
else
    echo "⚠️  .NET Framework应用启动测试失败（可能是首次运行需要初始化）"
    echo "首次运行Wine需要较长时间初始化.NET Framework"
fi

# 6. 检查macOS特定的X11配置
echo ""
echo "6. 检查macOS X11配置..."
if [[ "$OSTYPE" == "darwin"* ]]; then
    if command -v xquartz &> /dev/null || [ -d "/Applications/Utilities/XQuartz.app" ]; then
        echo "✅ XQuartz已安装"
        
        if ps aux | grep -v grep | grep -q "XQuartz"; then
            echo "✅ XQuartz正在运行"
            echo "X11显示: host.docker.internal:0"
        else
            echo "⚠️  XQuartz未运行，GUI版本需要启动XQuartz"
            echo "运行: open -a XQuartz"
        fi
    else
        echo "❌ XQuartz未安装，GUI版本无法运行"
        echo "安装命令: brew install --cask xquartz"
    fi
else
    echo "ℹ️  非macOS环境，跳过X11检查"
fi

# 7. 检查目录结构
echo ""
echo "7. 检查目录结构..."
directories=("data" "logs")

for dir in "${directories[@]}"; do
    if [ -d "$dir" ]; then
        echo "✅ $dir 目录存在"
    else
        echo "⚠️  $dir 目录不存在，将自动创建"
        mkdir -p "$dir"
    fi
done

# 8. 设置脚本权限
echo ""
echo "8. 设置脚本权限..."
scripts=("build-docker.sh" "run-gui.sh" "run-cli.sh" "test-docker.sh")

for script in "${scripts[@]}"; do
    if [ -f "$script" ]; then
        chmod +x "$script"
        echo "✅ $script 权限设置完成"
    else
        echo "❌ $script 文件不存在"
    fi
done

echo ""
echo "================================================"
echo "测试完成！"
echo ""
echo "下一步操作:"
echo "1. 构建镜像（首次）: ./build-docker.sh"
echo "2. 运行GUI版本: ./run-gui.sh"
echo "3. 运行CLI版本: ./run-cli.sh --help"
echo ""
echo "重要提示:"
echo "- 首次运行需要较长时间初始化Wine和.NET Framework"
echo "- GUI版本需要安装并运行XQuartz"
echo "- 性能可能不如原生Windows环境"
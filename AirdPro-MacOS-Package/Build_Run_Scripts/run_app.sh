#!/bin/bash

# AirdPro Docker容器内应用启动脚本
# AirdPro application start script for Docker container

set -e

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
PURPLE='\033[0;35m'
NC='\033[0m'

print_info() {
    echo -e "${BLUE}[信息]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[成功]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[警告]${NC} $1"
}

print_error() {
    echo -e "${RED}[错误]${NC} $1"
}

# 获取应用目录
APP_DIR="/app"
USER_HOME="/home/user"

# 查找AirdPro可执行文件
find_airdpro_exe() {
    local possible_paths=(
        "$APP_DIR/AirdPro/bin/Release/AirdPro.exe"
        "$APP_DIR/AirdPro/bin/Release/net48/AirdPro.exe"
        "$APP_DIR/AirdPro/bin/x64/Release/AirdPro.exe"
        "$USER_HOME/.wine/drive_c/AirdPro/AirdPro.exe"
        "/opt/airdpro/bin/AirdPro.exe"
    )
    
    for path in "${possible_paths[@]}"; do
        if [ -f "$path" ]; then
            echo "$path"
            return 0
        fi
    done
    
    print_error "找不到AirdPro.exe文件"
    echo "请确保应用程序文件已正确放置"
    return 1
}

# 初始化Wine环境
init_wine() {
    if [ ! -d "$USER_HOME/.wine" ]; then
        print_info "初始化Wine环境..."
        export WINEPREFIX="$USER_HOME/.wine"
        export WINEARCH=win64
        wineboot --init
        wineserver -w
        print_success "Wine环境初始化完成"
    else
        print_info "Wine环境已存在"
    fi
}

# 创建Wine配置文件
setup_wine_config() {
    print_info "配置Wine设置..."
    
    export WINEPREFIX="$USER_HOME/.wine"
    export WINEARCH=win64
    
    # 设置Wine为Windows 10模式
    winecfg /v win10
    
    # 安装常用组件
    if command -v winetricks &> /dev/null; then
        winetricks -q corefonts vcrun2019
    fi
}

# 启动AirdPro应用
start_airdpro() {
    local exe_path=$(find_airdpro_exe)
    
    if [ -z "$exe_path" ]; then
        print_error "未找到应用程序文件"
        exit 1
    fi
    
    print_info "启动AirdPro应用程序..."
    print_info "可执行文件路径: $exe_path"
    
    cd "$(dirname "$exe_path")"
    
    # 启动应用程序
    wine "$exe_path" &
    
    local app_pid=$!
    print_success "AirdPro已启动，PID: $app_pid"
    
    # 等待应用程序启动
    sleep 2
    
    # 检查进程是否还在运行
    if kill -0 $app_pid 2>/dev/null; then
        print_success "应用程序运行正常"
    else
        print_error "应用程序启动失败"
        exit 1
    fi
    
    # 保持脚本运行
    print_info "应用程序在后台运行..."
    print_info "要查看Wine调试信息，请使用: docker logs <container_name>"
    
    # 等待进程完成
    wait $app_pid
}

# 主函数
main() {
    print_info "AirdPro Docker容器启动器"
    
    # 初始化Wine
    init_wine
    
    # 配置Wine
    setup_wine_config
    
    # 启动应用
    start_airdpro
}

# 运行主函数
main "$@"
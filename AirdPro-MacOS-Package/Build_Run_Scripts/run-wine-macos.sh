#!/bin/bash

# AirdPro macOS Wine运行脚本
# AirdPro Wine run script for macOS

set -e

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
PURPLE='\033[0;35m'
CYAN='\033[0;36m'
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

print_header() {
    echo -e "${PURPLE}=============================================${NC}"
    echo -e "${PURPLE}  $1${NC}"
    echo -e "${PURPLE}=============================================${NC}"
}

# 显示使用帮助
show_help() {
    echo "AirdPro macOS Wine运行脚本"
    echo ""
    echo "用法: $0 [选项] [应用参数...]"
    echo ""
    echo "选项:"
    echo "  -h, --help         显示此帮助信息"
    echo "  --no-x11-check     跳过X11检查"
    echo "  --init-wine        强制重新初始化Wine环境"
    echo "  --debug            启用调试模式"
    echo ""
    echo "示例:"
    echo "  $0                          # 运行GUI应用"
    echo "  $0 --debug                  # 调试模式运行"
    echo "  $0 --init-wine              # 重新初始化Wine"
    echo ""
    echo "注意:"
    echo "  - 需要先安装Wine和XQuartz"
    echo "  - 首次运行可能需要较长时间初始化Wine环境"
    echo "  - 请确保XQuartz正在运行"
}

# 解析命令行参数
SKIP_X11_CHECK=false
INIT_WINE=false
DEBUG_MODE=false

while [[ $# -gt 0 ]]; do
    case $1 in
        -h|--help)
            show_help
            exit 0
            ;;
        --no-x11-check)
            SKIP_X11_CHECK=true
            shift
            ;;
        --init-wine)
            INIT_WINE=true
            shift
            ;;
        --debug)
            DEBUG_MODE=true
            shift
            ;;
        --)
            shift
            break
            ;;
        *)
            break
            ;;
    esac
done

# 获取脚本所在目录
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
AIRDPRO_DIR="$(dirname "$SCRIPT_DIR")"

# 检查Wine是否安装
check_wine() {
    print_info "检查Wine安装..."
    if ! command -v wine &> /dev/null; then
        print_error "Wine未安装"
        echo "请运行安装脚本："
        echo "  ./install-macos.sh"
        echo ""
        echo "或者手动安装："
        echo "  brew install --cask xquartz"
        echo "  brew install wine-stable"
        exit 1
    fi
    
    local wine_version=$(wine --version | head -1)
    print_success "Wine已安装：$wine_version"
}

# 检查X11环境
check_x11() {
    if [ "$SKIP_X11_CHECK" = true ]; then
        return
    fi
    
    print_info "检查X11环境..."
    
    # 检查XQuartz
    if ! command -v xquartz &> /dev/null && [ ! -d "/Applications/Utilities/XQuartz.app" ]; then
        print_error "XQuartz未安装"
        echo "请安装XQuartz："
        echo "  brew install --cask xquartz"
        exit 1
    fi
    
    # 检查XQuartz进程
    if ! pgrep Xquartz > /dev/null; then
        print_warning "XQuartz未运行"
        echo "正在启动XQuartz..."
        open -a XQuartz
        sleep 3
    fi
    
    # 设置DISPLAY变量
    export DISPLAY=:0
    
    print_success "X11环境检查通过"
}

# 初始化Wine环境
init_wine_environment() {
    if [ "$INIT_WINE" = true ]; then
        print_info "强制重新初始化Wine环境..."
        rm -rf ~/.wine
    fi
    
    if [ ! -d "$HOME/.wine" ]; then
        print_info "首次运行，初始化Wine环境..."
        print_info "这可能需要几分钟时间，请耐心等待..."
        
        # 设置Wine环境变量
        export WINEPREFIX="$HOME/.wine"
        export WINEARCH=win64
        export WINEDEBUG=-all
        
        # 初始化Wine
        wineboot --init
        
        # 安装.NET Framework
        print_info "安装.NET Framework 4.8..."
        local dotnet48_url="https://download.microsoft.com/download/6/5/6/65632d60-c5c1-4a47-82e4-6f4b4e6e5c5e/ndp48-web.exe"
        local temp_file=$(mktemp)
        
        if curl -L -o "$temp_file" "$dotnet48_url"; then
            wine "$temp_file" /quiet
            rm "$temp_file"
        else
            print_warning "无法下载.NET Framework，请手动安装"
        fi
        
        # 安装常用字体和运行时
        if command -v winetricks &> /dev/null; then
            print_info "安装常用字体和运行时..."
            winetricks -q corefonts vcrun2019
        fi
        
        # 等待Wine处理完成
        wineserver -w
        
        print_success "Wine环境初始化完成"
    else
        print_success "Wine环境已存在"
    fi
}

# 设置Wine优化配置
setup_wine_optimization() {
    if [ "$DEBUG_MODE" = true ]; then
        export WINEDEBUG=+all
    else
        export WINEDEBUG=-all
    fi
    
    export WINEPREFIX="$HOME/.wine"
    export WINEARCH=win64
    export DISPLAY=:0
    
    # 性能优化设置
    export LIBGL_ALWAYS_INDIRECT=0
    export MESA_GL_VERSION_OVERRIDE=3.3
    
    # 禁用Wine错误对话框（除调试模式外）
    if [ "$DEBUG_MODE" = false ]; then
        export WINEDLLOVERRIDES="mscoree=d"
    fi
}

# 查找AirdPro可执行文件
find_airdpro_exe() {
    local possible_paths=(
        "$AIRDPRO_DIR/AirdPro/bin/Release/AirdPro.exe"
        "$AIRDPRO_DIR/AirdPro/bin/Release/net48/AirdPro.exe"
        "$AIRDPRO_DIR/AirdPro/bin/x64/Release/AirdPro.exe"
        "$HOME/.wine/drive_c/AirdPro/AirdPro.exe"
    )
    
    for path in "${possible_paths[@]}"; do
        if [ -f "$path" ]; then
            echo "$path"
            return 0
        fi
    done
    
    print_error "找不到AirdPro.exe文件"
    echo "请确保应用程序文件已正确放置在以下位置之一："
    for path in "${possible_paths[@]}"; do
        echo "  - $path"
    done
    exit 1
}

# 运行AirdPro应用
run_airdpro() {
    local exe_path=$(find_airdpro_exe)
    
    print_header "启动AirdPro应用程序"
    print_info "可执行文件路径：$exe_path"
    print_info "Wine前缀：$WINEPREFIX"
    print_info "架构：$WINEARCH"
    
    cd "$(dirname "$exe_path")"
    
    if [ "$DEBUG_MODE" = true ]; then
        print_info "以调试模式运行..."
        wine "$exe_path" "$@" &
    else
        print_info "运行AirdPro..."
        wine "$exe_path" "$@" &
    fi
    
    local wine_pid=$!
    print_success "AirdPro已启动，PID: $wine_pid"
    
    # 等待应用程序启动
    sleep 2
    
    # 检查进程是否还在运行
    if kill -0 $wine_pid 2>/dev/null; then
        print_success "应用程序正在运行"
    else
        print_error "应用程序启动失败"
        return 1
    fi
    
    print_info "要在终端中查看Wine调试输出，请运行："
    echo "  tail -f ~/.wine/drive_c/users/$USER/AppData/Local/Temp/AirdPro.log"
}

# 清理函数
cleanup() {
    # 这里可以添加清理逻辑
    :
}

# 信号处理
trap cleanup EXIT

# 主函数
main() {
    print_header "AirdPro macOS Wine启动器"
    
    # 检查系统环境
    check_wine
    check_x11
    
    # 设置Wine环境
    init_wine_environment
    setup_wine_optimization
    
    # 运行应用程序
    run_airdpro "$@"
    
    print_success "启动完成！"
    print_info "如需技术支持，请查看日志文件或文档"
}

# 运行主函数
main "$@"
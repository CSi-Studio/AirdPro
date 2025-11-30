#!/bin/bash

# AirdPro Linux Wine运行脚本
# AirdPro Linux Wine running script

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

# 默认配置
WINE_VERSION="latest"
WINE_ARCH="win64"
WINE_PREFIX="$HOME/.wine-airdpro"
WINE_DIR="$WINE_PREFIX/drive_c/AirdPro"
APP_DIR="./AirdPro"
LOG_DIR="./logs"
GUI_MODE="auto"  # auto, x11, wayland, headless
DISPLAY="${DISPLAY:-:0}"
DEBUG_MODE=false
INTERACTIVE_MODE=false
FAST_MODE=false
NO_SETUP=false
REMOVE_DATA=false

# Wine配置选项
WINE_OPTIONS=""
ENV_OPTIONS=""

# 显示帮助信息
show_help() {
    echo "AirdPro Linux Wine运行脚本"
    echo
    echo "用法: $0 [选项]"
    echo
    echo "选项:"
    echo "  --wine-version VER    指定Wine版本（默认：latest）"
    echo "  --wine-arch ARCH      指定Wine架构（win32/win64，默认：win64）"
    echo "  --wine-prefix PATH    指定Wine前缀（默认：$WINE_PREFIX）"
    echo "  --app-dir PATH        指定AirdPro应用目录（默认：$APP_DIR）"
    echo "  --gui-mode MODE       GUI模式（auto/x11/wayland/headless，默认：auto）"
    echo "  --display DISPLAY     X11显示（默认：$DISPLAY）"
    echo "  --debug               启用调试模式"
    echo "  --interactive, -i     交互模式"
    echo "  --fast, -f            快速模式（跳过非必要检查）"
    echo "  --no-setup            跳过Wine环境设置"
    echo "  --remove-data         运行后删除Wine前缀"
    echo "  --wine-options OPTS   附加Wine选项"
    echo "  --env-options OPTS    附加环境变量"
    echo "  --memory SIZE         设置Wine内存限制（如：2g）"
    echo "  --version             显示Wine版本"
    echo "  -h, --help            显示帮助信息"
    echo
    echo "示例:"
    echo "  $0                      # 标准启动"
    echo "  $0 --gui-mode x11       # 强制X11模式"
    echo "  $0 --interactive        # 交互模式"
    echo "  $0 --debug --wine-options '--verbose'  # 调试模式"
    echo "  $0 --wine-arch win32    # 32位模式启动"
}

# 检查Wine安装
check_wine_installation() {
    print_header "检查Wine安装"
    
    if ! command -v wine &> /dev/null; then
        print_error "Wine未安装，请运行 install-linux.sh 安装"
        print_info "或手动安装Wine："
        print_info "  Ubuntu/Debian: sudo apt install wine wine64 wine32"
        print_info "  Fedora/CentOS: sudo dnf install wine"
        print_info "  Arch Linux: sudo pacman -S wine"
        exit 1
    fi
    
    local wine_version=$(wine --version | head -n1)
    print_success "Wine版本：$wine_version"
    
    # 检查Wine架构
    if [ "$WINE_ARCH" = "win64" ]; then
        if ! wine64 --version &> /dev/null; then
            print_error "Wine64未安装，无法使用64位模式"
            print_info "安装Wine64或使用--wine-arch win32"
            exit 1
        fi
    elif [ "$WINE_ARCH" = "win32" ]; then
        if ! wine --version &> /dev/null; then
            print_error "Wine32未安装，无法使用32位模式"
            print_info "安装Wine32或使用--wine-arch win64"
            exit 1
        fi
    fi
    
    print_success "Wine架构检查通过：$WINE_ARCH"
}

# 检查系统环境
check_system_environment() {
    if [ "$FAST_MODE" = "true" ]; then
        print_info "快速模式，跳过详细环境检查"
        return 0
    fi
    
    print_header "检查系统环境"
    
    # 检查X11环境
    case "$GUI_MODE" in
        "x11"|"auto")
            if [ -z "$DISPLAY" ]; then
                print_warning "未设置DISPLAY环境变量，GUI可能无法显示"
                print_info "如果您在桌面环境中，通常会自动设置"
                if [ "$GUI_MODE" = "x11" ]; then
                    print_error "X11模式需要DISPLAY环境变量"
                    exit 1
                fi
            else
                print_success "X11显示设置：$DISPLAY"
                # 检查X11访问权限
                if xset q > /dev/null 2>&1; then
                    print_success "X11访问正常"
                else
                    print_warning "无法访问X11显示，尝试设置权限..."
                    xhost +local:docker > /dev/null 2>&1 || print_warning "无法设置X11权限"
                fi
            fi
            ;;
        "wayland")
            if [ -z "$WAYLAND_DISPLAY" ]; then
                print_warning "WAYLAND_DISPLAY未设置"
                if [ "$GUI_MODE" = "wayland" ]; then
                    print_error "Wayland模式需要WAYLAND_DISPLAY环境变量"
                    exit 1
                fi
            else
                print_success "Wayland显示设置：$WAYLAND_DISPLAY"
            fi
            ;;
        "headless")
            print_info "无头模式运行，GUI不可用"
            ;;
    esac
    
    # 检查依赖库
    print_info "检查依赖库..."
    
    # 检查libc6（32位，如果需要32位支持）
    if [ "$WINE_ARCH" = "win32" ]; then
        if ! dpkg -l | grep -q libc6:i386; then
            print_warning "32位libc6未安装，可能需要sudo apt install libc6:i386"
        fi
    fi
    
    # 检查必要的图形库
    if [ "$GUI_MODE" != "headless" ]; then
        local missing_libs=0
        
        # 检查X11相关库
        for lib in libx11-6 libxext6 libxi6 libxrandr2 libxrender1 libfontconfig1; do
            if ! ldconfig -p | grep -q "$lib"; then
                print_warning "缺少库：$lib"
                missing_libs=$((missing_libs + 1))
            fi
        done
        
        if [ $missing_libs -gt 0 ]; then
            print_warning "缺少 $missing_libs 个依赖库，可能影响GUI显示"
            print_info "尝试运行：sudo apt install libx11-6 libxext6 libxi6 libxrandr2 libxrender1 libfontconfig1"
        else
            print_success "图形库检查通过"
        fi
    fi
    
    # 检查磁盘空间
    local available_space=$(df -h . | awk 'NR==2 {print $4}' | sed 's/G.*//')
    if [ "$available_space" -lt 3 ]; then
        print_warning "建议至少3GB可用空间，当前：${available_space}GB"
    else
        print_success "磁盘空间检查通过：${available_space}GB可用"
    fi
    
    # 检查内存
    local total_memory=$(free -m | awk 'NR==2{printf "%.0f", $2}')
    if [ "$total_memory" -lt 2048 ]; then
        print_warning "建议至少2GB内存，当前：${total_memory}MB"
    else
        print_success "内存检查通过：${total_memory}MB"
    fi
}

# 初始化Wine环境
setup_wine_environment() {
    if [ "$NO_SETUP" = "true" ]; then
        print_info "跳过Wine环境设置（--no-setup）"
        return 0
    fi
    
    print_header "设置Wine环境"
    
    # 创建Wine前缀目录
    if [ -d "$WINE_PREFIX" ] && [ "$REMOVE_DATA" = "true" ]; then
        print_info "删除现有Wine前缀：$WINE_PREFIX"
        rm -rf "$WINE_PREFIX"
    fi
    
    if [ ! -d "$WINE_PREFIX" ]; then
        print_info "创建Wine前缀：$WINE_PREFIX"
        mkdir -p "$WINE_PREFIX"
        
        # 设置Wine架构
        export WINEPREFIX="$WINE_PREFIX"
        export WINEARCH="$WINE_ARCH"
        
        # 创建Wine前缀
        print_info "初始化Wine前缀..."
        if wineboot --init; then
            print_success "Wine前缀创建成功"
        else
            print_error "Wine前缀创建失败"
            exit 1
        fi
    else
        print_success "Wine前缀已存在：$WINE_PREFIX"
    fi
    
    # 安装必要的运行时
    print_info "安装必要的运行时组件..."
    
    # 设置Wine前缀和架构
    export WINEPREFIX="$WINE_PREFIX"
    export WINEARCH="$WINE_ARCH"
    
    # 安装常用组件
    local components=("dotnet48" "vcrun2019" "msxml6" "corefonts")
    
    for component in "${components[@]}"; do
        print_info "安装组件：$component"
        if winetricks --unattended "$component" > /dev/null 2>&1; then
            print_success "组件安装成功：$component"
        else
            print_warning "组件安装失败：$component"
        fi
    done
    
    # 复制或创建AirdPro目录
    if [ -d "$APP_DIR" ]; then
        print_info "应用目录已存在：$APP_DIR"
    else
        print_info "创建应用目录：$APP_DIR"
        mkdir -p "$WINE_DIR"
        print_info "请将AirdPro应用程序文件复制到：$WINE_DIR"
    fi
}

# 设置显示环境
setup_display_environment() {
    print_info "配置显示环境..."
    
    # 设置显示变量
    case "$GUI_MODE" in
        "x11")
            export DISPLAY="$DISPLAY"
            export WINEDLLOVERRIDES="winegstreamer=d"
            ENV_OPTIONS="$ENV_OPTIONS DISPLAY=$DISPLAY"
            ;;
        "wayland")
            export WAYLAND_DISPLAY="${WAYLAND_DISPLAY:-wayland-0}"
            ENV_OPTIONS="$ENV_OPTIONS WAYLAND_DISPLAY=$WAYLAND_DISPLAY"
            ;;
        "headless")
            # 无头模式，不需要GUI
            print_info "无头模式运行"
            ;;
        "auto")
            # 自动检测
            if [ -n "$DISPLAY" ]; then
                print_info "使用X11模式"
                export DISPLAY="$DISPLAY"
                ENV_OPTIONS="$ENV_OPTIONS DISPLAY=$DISPLAY"
            elif [ -n "$WAYLAND_DISPLAY" ]; then
                print_info "使用Wayland模式"
                export WAYLAND_DISPLAY="$WAYLAND_DISPLAY"
                ENV_OPTIONS="$ENV_OPTIONS WAYLAND_DISPLAY=$WAYLAND_DISPLAY"
            else
                print_info "使用无头模式"
                GUI_MODE="headless"
            fi
            ;;
    esac
    
    print_success "显示环境配置完成：$GUI_MODE"
}

# 运行AirdPro
run_airdpro() {
    print_header "运行AirdPro"
    
    # 设置Wine环境
    export WINEPREFIX="$WINE_PREFIX"
    export WINEARCH="$WINE_ARCH"
    
    # 准备运行命令
    local wine_cmd="wine"
    if [ "$WINE_ARCH" = "win64" ]; then
        wine_cmd="wine64"
    fi
    
    # 添加Wine选项
    if [ -n "$WINE_OPTIONS" ]; then
        wine_cmd="$wine_cmd $WINE_OPTIONS"
    fi
    
    # 检查应用目录
    if [ ! -d "$WINE_DIR" ]; then
        print_error "AirdPro目录不存在：$WINE_DIR"
        print_info "请确保已将AirdPro应用程序复制到Wine前缀中"
        exit 1
    fi
    
    # 创建日志目录
    mkdir -p "$LOG_DIR"
    
    # 运行应用
    print_info "启动AirdPro..."
    print_info "Wine前缀：$WINE_PREFIX"
    print_info "应用目录：$WINE_DIR"
    print_info "日志目录：$LOG_DIR"
    
    if [ "$DEBUG_MODE" = "true" ]; then
        print_info "调试模式启用"
        ENV_OPTIONS="$ENV_OPTIONS WINEDEBUG=+all"
    fi
    
    if [ "$INTERACTIVE_MODE" = "true" ]; then
        print_info "交互模式启动"
        cd "$WINE_DIR"
        if [ "$GUI_MODE" = "headless" ]; then
            bash  # 启动shell
        else
            $wine_cmd explorer /desktop=AirdPro,AirdPro.exe &
            bash
        fi
    else
        # 启动应用程序
        cd "$WINE_DIR"
        
        if [ "$GUI_MODE" = "headless" ]; then
            # 无头模式，可能无法运行GUI应用
            print_warning "无头模式下可能无法运行GUI应用"
            print_info "如需GUI显示，请使用 --gui-mode x11 或 --gui-mode wayland"
        fi
        
        # 启动命令
        local start_cmd="$wine_cmd AirdPro.exe"
        
        print_info "执行命令：$start_cmd"
        
        # 设置环境
        eval "export $ENV_OPTIONS"
        
        # 运行应用（后台）
        if [ -z "$DEBUG_MODE" ] && [ "$GUI_MODE" != "headless" ]; then
            nohup $start_cmd > "$LOG_DIR/airdpro.log" 2>&1 &
            local pid=$!
            print_success "AirdPro已启动，PID: $pid"
            print_info "日志文件：$LOG_DIR/airdpro.log"
            
            # 等待一下看是否成功启动
            sleep 3
            if kill -0 $pid 2>/dev/null; then
                print_success "AirdPro运行正常"
            else
                print_error "AirdPro启动失败，检查日志：$LOG_DIR/airdpro.log"
                cat "$LOG_DIR/airdpro.log"
                exit 1
            fi
        else
            # 交互式运行
            $start_cmd
        fi
    fi
}

# 显示Wine信息
show_wine_info() {
    print_header "Wine环境信息"
    
    export WINEPREFIX="$WINE_PREFIX"
    
    echo "Wine前缀：$WINE_PREFIX"
    echo "Wine架构：$WINE_ARCH"
    echo "GUI模式：$GUI_MODE"
    
    if [ -n "$DISPLAY" ]; then
        echo "X11显示：$DISPLAY"
    fi
    
    if [ -n "$WAYLAND_DISPLAY" ]; then
        echo "Wayland显示：$WAYLAND_DISPLAY"
    fi
    
    echo
    echo "Wine版本："
    wine --version
    
    echo
    echo "Wine配置："
    winecfg --version 2>/dev/null || echo "winecfg不可用"
    
    echo
    echo "已安装的程序："
    if [ -d "$WINE_PREFIX/drive_c/Program Files" ]; then
        ls -la "$WINE_PREFIX/drive_c/Program Files" 2>/dev/null || echo "无已安装程序"
    else
        echo "未找到Program Files目录"
    fi
}

# 清理Wine环境
cleanup_wine_environment() {
    print_header "清理Wine环境"
    
    if [ -d "$WINE_PREFIX" ]; then
        read -p "是否删除Wine前缀 $WINE_PREFIX？(y/N): " -n 1 -r
        echo
        if [[ $REPLY =~ ^[Yy]$ ]]; then
            print_info "删除Wine前缀..."
            rm -rf "$WINE_PREFIX"
            print_success "Wine前缀已删除"
        else
            print_info "保留Wine前缀"
        fi
    else
        print_info "Wine前缀不存在，无需清理"
    fi
    
    # 清理日志文件
    if [ -d "$LOG_DIR" ]; then
        read -p "是否清理日志文件？(y/N): " -n 1 -r
        echo
        if [[ $REPLY =~ ^[Yy]$ ]]; then
            rm -rf "$LOG_DIR"/*
            print_success "日志文件已清理"
        fi
    fi
}

# 重置Wine环境
reset_wine_environment() {
    print_header "重置Wine环境"
    
    if [ -d "$WINE_PREFIX" ]; then
        print_info "备份现有Wine前缀..."
        local backup_dir="$WINE_PREFIX.backup.$(date +%Y%m%d_%H%M%S)"
        mv "$WINE_PREFIX" "$backup_dir"
        print_success "现有前缀已备份到：$backup_dir"
    fi
    
    print_info "重置Wine环境..."
    setup_wine_environment
    
    print_success "Wine环境重置完成"
}

# 主函数
main() {
    ACTION="start"
    
    # 解析命令行参数
    while [[ $# -gt 0 ]]; do
        case $1 in
            --wine-version)
                WINE_VERSION="$2"
                shift 2
                ;;
            --wine-arch)
                WINE_ARCH="$2"
                shift 2
                ;;
            --wine-prefix)
                WINE_PREFIX="$2"
                shift 2
                ;;
            --app-dir)
                APP_DIR="$2"
                shift 2
                ;;
            --gui-mode)
                GUI_MODE="$2"
                shift 2
                ;;
            --display)
                DISPLAY="$2"
                export DISPLAY
                shift 2
                ;;
            --debug)
                DEBUG_MODE=true
                shift
                ;;
            --interactive|-i)
                INTERACTIVE_MODE=true
                shift
                ;;
            --fast|-f)
                FAST_MODE=true
                shift
                ;;
            --no-setup)
                NO_SETUP=true
                shift
                ;;
            --remove-data)
                REMOVE_DATA=true
                shift
                ;;
            --wine-options)
                WINE_OPTIONS="$2"
                shift 2
                ;;
            --env-options)
                ENV_OPTIONS="$2"
                shift 2
                ;;
            --memory)
                WINE_OPTIONS="$WINE_OPTIONS --memory $2"
                shift 2
                ;;
            info)
                ACTION="info"
                shift
                ;;
            cleanup)
                ACTION="cleanup"
                shift
                ;;
            reset)
                ACTION="reset"
                shift
                ;;
            --version)
                echo "AirdPro Linux Wine运行脚本 v1.0"
                wine --version
                exit 0
                ;;
            -h|--help)
                show_help
                exit 0
                ;;
            *)
                print_error "未知选项: $1"
                show_help
                exit 1
                ;;
        esac
    done
    
    # 检查是否在正确的目录中
    if [ ! -f "install-linux.sh" ] && [ "$ACTION" != "info" ]; then
        print_error "请在AirdPro-Linux-Package目录中运行此脚本"
        exit 1
    fi
    
    print_header "AirdPro Linux Wine运行脚本 v1.0"
    echo "Wine前缀: $WINE_PREFIX"
    echo "Wine架构: $WINE_ARCH"
    echo "GUI模式: $GUI_MODE"
    echo "显示: $DISPLAY"
    echo "应用目录: $APP_DIR"
    echo
    
    # 执行相应操作
    case "$ACTION" in
        start)
            check_wine_installation
            check_system_environment
            setup_display_environment
            setup_wine_environment
            run_airdpro
            ;;
        info)
            check_wine_installation
            show_wine_info
            ;;
        cleanup)
            cleanup_wine_environment
            ;;
        reset)
            check_wine_installation
            reset_wine_environment
            ;;
        *)
            print_error "未知操作: $ACTION"
            show_help
            exit 1
            ;;
    esac
}

# 运行主函数
main "$@"
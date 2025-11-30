#!/bin/bash

# AirdPro跨平台GUI运行脚本
# Cross-platform GUI run script for AirdPro

set -e

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
PURPLE='\033[0;35m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

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
    echo "AirdPro 跨平台GUI运行脚本"
    echo ""
    echo "用法: $0 [选项] [应用参数...]"
    echo ""
    echo "选项:"
    echo "  -h, --help         显示此帮助信息"
    echo "  --linux-only       仅在Linux环境下运行"
    echo "  --macos-only       仅在macOS环境下运行"
    echo "  --no-x11-check     跳过X11检查（仅Linux）"
    echo ""
    echo "示例:"
    echo "  $0                          # 运行GUI应用"
    echo "  $0 --macos-only             # 仅macOS环境运行"
    echo "  $0 -- linux-only            # 仅Linux环境运行"
    echo "  $0 --no-x11-check           # 跳过X11检查"
    echo ""
    echo "注意:"
    echo "  - 首次运行可能需要较长时间初始化Wine环境"
    echo "  - macOS需要安装XQuartz: brew install --cask xquartz"
    echo "  - Linux需要X11显示服务器"
}

# 解析命令行参数
LINUX_ONLY=false
MACOS_ONLY=false
SKIP_X11_CHECK=false

while [[ $# -gt 0 ]]; do
    case $1 in
        -h|--help)
            show_help
            exit 0
            ;;
        --linux-only)
            LINUX_ONLY=true
            shift
            ;;
        --macos-only)
            MACOS_ONLY=true
            shift
            ;;
        --no-x11-check)
            SKIP_X11_CHECK=true
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

# 检查运行环境
check_environment() {
    local os_type=$(uname -s)
    
    if [ "$LINUX_ONLY" = true ] && [[ "$os_type" != "Linux" ]]; then
        print_error "仅允许在Linux环境下运行"
        exit 1
    fi
    
    if [ "$MACOS_ONLY" = true ] && [[ "$os_type" != "Darwin" ]]; then
        print_error "仅允许在macOS环境下运行"
        exit 1
    fi
    
    # 检测操作系统
    if [[ "$os_type" == "Darwin"* ]]; then
        print_info "检测到macOS环境"
        check_macos_environment
    elif [[ "$os_type" == "Linux" ]]; then
        print_info "检测到Linux环境"
        check_linux_environment
    else
        print_error "不支持的操作系统: $os_type"
        exit 1
    fi
}

# 检查macOS环境
check_macos_environment() {
    # 检查Docker是否安装
    if ! command -v docker &> /dev/null; then
        print_error "Docker未安装"
        echo "请先安装Docker Desktop for Mac:"
        echo "https://docs.docker.com/desktop/mac/install/"
        exit 1
    fi
    
    # 检查Docker服务状态
    if ! docker info &> /dev/null; then
        print_error "Docker服务未运行"
        echo "请启动Docker Desktop应用"
        exit 1
    fi
    
    # 检查XQuartz
    if [ "$SKIP_X11_CHECK" = false ]; then
        if ! command -v xquartz &> /dev/null && [ ! -d "/Applications/Utilities/XQuartz.app" ]; then
            print_warning "XQuartz未安装"
            echo "GUI需要XQuartz支持，请运行:"
            echo "  brew install --cask xquartz"
            echo "安装后重启终端并手动启动XQuartz:"
            echo "  open -a XQuartz"
            echo ""
            read -p "是否继续运行？(可能无法显示GUI) [y/N]: " -n 1 -r
            echo
            if [[ ! $REPLY =~ ^[Yy]$ ]]; then
                exit 1
            fi
        fi
    fi
}

# 检查Linux环境
check_linux_environment() {
    # 检查Docker是否安装
    if ! command -v docker &> /dev/null; then
        print_error "Docker未安装"
        echo "请先安装Docker:"
        echo "  Ubuntu/Debian: sudo apt-get install docker.io"
        echo "  CentOS/RHEL: sudo yum install docker"
        exit 1
    fi
    
    # 检查Docker服务状态
    if ! docker info &> /dev/null; then
        print_error "Docker服务未运行"
        echo "请启动Docker服务:"
        echo "  sudo systemctl start docker"
        exit 1
    fi
    
    # 检查X11环境
    if [ "$SKIP_X11_CHECK" = false ]; then
        if [ -z "$DISPLAY" ]; then
            print_warning "DISPLAY环境变量未设置"
            echo "如果使用SSH，请确保X11转发已启用:"
            echo "  ssh -X user@host"
            echo ""
            read -p "是否继续运行？(可能无法显示GUI) [y/N]: " -n 1 -r
            echo
            if [[ ! $REPLY =~ ^[Yy]$ ]]; then
                exit 1
            fi
        fi
    fi
}

# 检查镜像是否存在
check_image() {
    print_info "检查AirdPro镜像..."
    if ! docker image inspect airdpro:linux &> /dev/null; then
        print_error "airdpro:linux镜像不存在"
        echo "请先运行构建脚本:"
        echo "  ./build-docker-en.sh"
        exit 1
    fi
    print_success "镜像检查通过"
}

# 配置X11显示
setup_x11_display() {
    local os_type=$(uname -s)
    
    if [[ "$os_type" == "Darwin"* ]]; then
        # macOS配置
        print_info "配置macOS X11显示..."
        
        # 检查XQuartz是否运行
        if ! ps aux | grep -v grep | grep -q "XQuartz"; then
            print_info "启动XQuartz..."
            open -a XQuartz || true
            echo "等待XQuartz启动..."
            sleep 5
        fi
        
        # 设置显示环境变量
        export DISPLAY=host.docker.internal:0
        echo "X11显示设置为: $DISPLAY"
        
        # 允许X11连接
        print_info "配置X11安全设置..."
        xhost +localhost 2>/dev/null || true
        
    elif [[ "$os_type" == "Linux" ]]; then
        # Linux配置
        print_info "配置Linux X11显示..."
        
        if [ -z "$DISPLAY" ]; then
            print_warning "DISPLAY未设置，使用默认:0"
            export DISPLAY=:0
        fi
        echo "X11显示设置为: $DISPLAY"
        
        # 允许X11连接
        if command -v xhost &> /dev/null; then
            xhost +local:docker 2>/dev/null || true
        fi
    fi
}

# 创建必要目录
create_directories() {
    print_info "创建必要目录..."
    mkdir -p data logs
    print_success "目录创建完成"
}

# 运行GUI容器
run_gui_container() {
    print_info "启动AirdPro GUI容器..."
    print_warning "首次运行需要较长时间初始化Wine环境，请耐心等待..."
    echo
    
    # 构建docker run命令
    local docker_cmd="docker run -it --rm"
    docker_cmd="$docker_cmd --name airdpro-gui"
    docker_cmd="$docker_cmd -v \"$(pwd)/data\":/data"
    docker_cmd="$docker_cmd -v \"$(pwd)/logs\":/logs"
    docker_cmd="$docker_cmd -e DISPLAY=$DISPLAY"
    docker_cmd="$docker_cmd -e WINEPREFIX=/wine"
    docker_cmd="$docker_cmd -e WINEARCH=win64"
    docker_cmd="$docker_cmd -e WINEDEBUG=-all"
    docker_cmd="$docker_cmd --privileged"
    docker_cmd="$docker_cmd airdpro:linux"
    
    # 添加用户参数
    if [ $# -gt 0 ]; then
        docker_cmd="$docker_cmd \"$@\""
    fi
    
    print_info "执行命令: $docker_cmd"
    echo
    
    # 执行docker命令
    eval $docker_cmd
}

# 清理函数
cleanup() {
    print_info "正在清理..."
    # 杀死可能的残留进程
    docker kill airdpro-gui 2>/dev/null || true
}

# 设置信号处理
trap cleanup EXIT INT TERM

# 主函数
main() {
    print_header "AirdPro 跨平台GUI运行"
    
    # 检查环境
    check_environment
    
    # 检查镜像
    check_image
    
    # 创建目录
    create_directories
    
    # 配置X11
    setup_x11_display
    
    # 运行GUI
    run_gui_container "$@"
    
    print_success "AirdPro GUI已退出"
    print_info "日志文件位置: $(pwd)/logs/"
}

# 执行主函数
main "$@"
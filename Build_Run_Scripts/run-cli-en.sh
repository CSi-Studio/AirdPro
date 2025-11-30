#!/bin/bash

# AirdPro跨平台命令行运行脚本
# Cross-platform CLI run script for AirdPro

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
    echo "AirdPro 跨平台命令行运行脚本"
    echo ""
    echo "用法: $0 [选项] [应用参数...]"
    echo ""
    echo "选项:"
    echo "  -h, --help         显示此帮助信息"
    echo "  --linux-only       仅在Linux环境下运行"
    echo "  --macos-only       仅在macOS环境下运行"
    echo "  --windows-only     仅在Windows环境下运行"
    echo "  --console          显示详细信息（默认静默运行）"
    echo "  --verbose          显示详细调试信息"
    echo ""
    echo "示例:"
    echo "  $0                          # 静默运行CLI版本"
    echo "  $0 --console                # 显示详细信息运行"
    echo "  $0 -- verbose               # 显示调试信息运行"
    echo "  $0 --windows-only --console # Windows环境控制台模式"
    echo ""
    echo "注意:"
    echo "  - Windows环境使用原生.NET Framework"
    echo "  - Linux/macOS环境使用Wine容器"
    echo "  - 首次运行可能需要初始化环境"
}

# 解析命令行参数
LINUX_ONLY=false
MACOS_ONLY=false
WINDOWS_ONLY=false
CONSOLE=false
VERBOSE=false

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
        --windows-only)
            WINDOWS_ONLY=true
            shift
            ;;
        --console)
            CONSOLE=true
            shift
            ;;
        --verbose)
            VERBOSE=true
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
    local platform=$(uname -m)
    
    # 检查平台限制
    if [ "$LINUX_ONLY" = true ] && [[ "$os_type" != "Linux" ]]; then
        print_error "仅允许在Linux环境下运行"
        exit 1
    fi
    
    if [ "$MACOS_ONLY" = true ] && [[ "$os_type" != "Darwin" ]]; then
        print_error "仅允许在macOS环境下运行"
        exit 1
    fi
    
    if [ "$WINDOWS_ONLY" = true ]; then
        if [[ "$os_type" == "MINGW"* ]] || [[ "$os_type" == "MSYS"* ]] || [[ "$os_type" == "CYGWIN"* ]]; then
            print_info "检测到Windows环境"
        else
            print_error "仅允许在Windows环境下运行"
            exit 1
        fi
    fi
    
    # 检测操作系统
    if [[ "$os_type" == "Darwin"* ]]; then
        print_info "检测到macOS环境 (架构: $platform)"
        check_macos_environment
    elif [[ "$os_type" == "Linux" ]]; then
        print_info "检测到Linux环境 (架构: $platform)"
        check_linux_environment
    elif [[ "$os_type" == "MINGW"* ]] || [[ "$os_type" == "MSYS"* ]] || [[ "$os_type" == "CYGWIN"* ]]; then
        print_info "检测到Windows环境"
        check_windows_environment
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
}

# 检查Windows环境
check_windows_environment() {
    # 检查.NET Framework是否安装
    if [ ! -f "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\csc.exe" ] && \
       [ ! -f "C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\csc.exe" ]; then
        print_warning ".NET Framework 4.8未找到，请确保已安装"
    fi
}

# 检查镜像是否存在
check_image() {
    print_info "检查AirdPro镜像..."
    
    local image_name=""
    local os_type=$(uname -s)
    
    if [[ "$os_type" == "Darwin"* ]] || [[ "$os_type" == "Linux" ]]; then
        # Linux/macOS使用Wine镜像
        if ! docker image inspect airdpro:cli &> /dev/null; then
            print_error "airdpro:cli镜像不存在"
            echo "请先运行构建脚本:"
            echo "  Linux/macOS: ./build-docker-en.sh"
            echo "  Windows: build-docker-en.bat"
            exit 1
        fi
        image_name="airdpro:cli"
    else
        # Windows使用原生镜像
        if ! docker image inspect airdpro:windows &> /dev/null; then
            print_error "airdpro:windows镜像不存在"
            echo "请先运行构建脚本:"
            echo "  Windows: build-docker-en.bat"
            exit 1
        fi
        image_name="airdpro:windows"
    fi
    
    print_success "镜像检查通过 ($image_name)"
    echo $image_name > /tmp/airdpro_image_name.tmp
}

# 创建必要目录
create_directories() {
    print_info "创建必要目录..."
    mkdir -p data logs
    print_success "目录创建完成"
}

# 运行Windows原生容器
run_windows_container() {
    print_info "启动AirdPro Windows容器..."
    
    # 构建docker run命令
    local docker_cmd="docker run -it --rm"
    docker_cmd="$docker_cmd --name airdpro-cli"
    docker_cmd="$docker_cmd -v \"$(pwd)/data\":C:\\data"
    docker_cmd="$docker_cmd -v \"$(pwd)/logs\":C:\\logs"
    docker_cmd="$docker_cmd -e CONSOLE_MODE=$CONSOLE"
    docker_cmd="$docker_cmd airdpro:windows"
    
    # 添加用户参数
    if [ $# -gt 0 ]; then
        docker_cmd="$docker_cmd \"$@\""
    fi
    
    print_info "执行命令: $docker_cmd"
    echo
    
    # 执行docker命令
    eval $docker_cmd
}

# 运行Linux/macOS Wine容器
run_wine_container() {
    print_info "启动AirdPro Wine容器..."
    
    # 设置环境变量
    local wine_env=""
    if [ "$VERBOSE" = true ]; then
        wine_env="$wine_env -e WINEDEBUG=+all"
    else
        wine_env="$wine_env -e WINEDEBUG=-all"
    fi
    
    # 构建docker run命令
    local docker_cmd="docker run -it --rm"
    docker_cmd="$docker_cmd --name airdpro-cli"
    docker_cmd="$docker_cmd -v \"$(pwd)/data\":/data"
    docker_cmd="$docker_cmd -v \"$(pwd)/logs\":/logs"
    docker_cmd="$docker_cmd -e WINEPREFIX=/wine"
    docker_cmd="$docker_cmd -e WINEARCH=win64"
    docker_cmd="$docker_cmd -e CONSOLE_MODE=$CONSOLE"
    docker_cmd="$docker_cmd $wine_env"
    docker_cmd="$docker_cmd airdpro:cli"
    
    # 添加用户参数
    if [ $# -gt 0 ]; then
        docker_cmd="$docker_cmd \"$@\""
    fi
    
    print_info "执行命令: $docker_cmd"
    echo
    
    # 执行docker命令
    eval $docker_cmd
}

# 运行Windows原生应用
run_windows_native() {
    print_info "启动AirdPro Windows原生应用..."
    
    # 查找可执行文件
    local exe_path=""
    if [ -f "AirdPro.exe" ]; then
        exe_path="AirdPro.exe"
    elif [ -f "bin/Release/AirdPro.exe" ]; then
        exe_path="bin/Release/AirdPro.exe"
    else
        print_error "未找到AirdPro.exe文件"
        echo "请确保在正确的目录中运行脚本"
        exit 1
    fi
    
    print_info "执行应用: $exe_path"
    
    # 设置控制台模式
    if [ "$CONSOLE" = true ]; then
        print_info "控制台模式: 详细信息"
    fi
    
    if [ "$VERBOSE" = true ]; then
        print_info "调试模式: 启用详细日志"
    fi
    
    echo
    
    # 运行应用
    if [ $# -gt 0 ]; then
        mono $exe_path "$@" 2>/dev/null || wine $exe_path "$@"
    else
        mono $exe_path 2>/dev/null || wine $exe_path
    fi
}

# 清理函数
cleanup() {
    print_info "正在清理..."
    # 杀死可能的残留进程
    docker kill airdpro-cli 2>/dev/null || true
    rm -f /tmp/airdpro_image_name.tmp
}

# 设置信号处理
trap cleanup EXIT INT TERM

# 主函数
main() {
    print_header "AirdPro 跨平台命令行运行"
    
    # 检查环境
    check_environment
    
    # 检查镜像
    check_image
    
    # 创建目录
    create_directories
    
    # 根据平台运行不同容器
    local os_type=$(uname -s)
    local image_name=""
    
    if [ -f "/tmp/airdpro_image_name.tmp" ]; then
        image_name=$(cat /tmp/airdpro_image_name.tmp)
    fi
    
    if [[ "$os_type" == "MINGW"* ]] || [[ "$os_type" == "MSYS"* ]] || [[ "$os_type" == "CYGWIN"* ]]; then
        # Windows原生环境
        if [[ "$image_name" == "windows" ]]; then
            run_windows_container "$@"
        else
            run_windows_native "$@"
        fi
    else
        # Linux/macOS Wine环境
        if [[ "$image_name" == "cli" ]]; then
            run_wine_container "$@"
        else
            print_error "未知的容器类型: $image_name"
            exit 1
        fi
    fi
    
    print_success "AirdPro CLI已退出"
    print_info "日志文件位置: $(pwd)/logs/"
}

# 执行主函数
main "$@"
#!/bin/bash

# AirdPro跨平台Docker构建脚本 (Linux/macOS)
# Cross-platform Docker build script for AirdPro

set -e  # 遇到错误立即退出

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
PURPLE='\033[0;35m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# 打印带颜色的信息
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
    echo "AirdPro 跨平台Docker构建脚本"
    echo ""
    echo "用法: $0 [选项]"
    echo ""
    echo "选项:"
    echo "  -h, --help         显示此帮助信息"
    echo "  --windows-only     仅构建Windows版本"
    echo "  --linux-only       仅构建Linux/macOS版本"
    echo "  --all             构建所有版本 (默认)"
    echo "  --clean           构建前清理现有镜像"
    echo "  --no-cache        不使用缓存构建"
    echo ""
    echo "示例:"
    echo "  $0                 # 构建所有版本"
    echo "  $0 --windows-only  # 仅构建Windows版本"
    echo "  $0 --clean --all   # 清理后构建所有版本"
}

# 解析命令行参数
WINDOWS_ONLY=false
LINUX_ONLY=false
BUILD_ALL=true
CLEAN_BUILD=false
NO_CACHE=false

while [[ $# -gt 0 ]]; do
    case $1 in
        -h|--help)
            show_help
            exit 0
            ;;
        --windows-only)
            WINDOWS_ONLY=true
            BUILD_ALL=false
            shift
            ;;
        --linux-only)
            LINUX_ONLY=true
            BUILD_ALL=false
            shift
            ;;
        --all)
            BUILD_ALL=true
            shift
            ;;
        --clean)
            CLEAN_BUILD=true
            shift
            ;;
        --no-cache)
            NO_CACHE=true
            shift
            ;;
        *)
            print_error "未知选项: $1"
            show_help
            exit 1
            ;;
    esac
done

# 检查Docker是否安装
check_docker() {
    print_info "检查Docker环境..."
    if ! command -v docker &> /dev/null; then
        print_error "Docker未安装"
        echo "请先安装Docker:"
        echo "  macOS: https://docs.docker.com/desktop/mac/install/"
        echo "  Ubuntu: sudo apt-get install docker.io"
        exit 1
    fi
    
    if ! docker info &> /dev/null; then
        print_error "Docker服务未运行"
        echo "请启动Docker服务:"
        echo "  macOS: 启动Docker Desktop应用"
        echo "  Linux: sudo systemctl start docker"
        exit 1
    fi
    
    print_success "Docker环境检查通过"
}

# 清理现有镜像
clean_existing_images() {
    if [ "$CLEAN_BUILD" = true ]; then
        print_info "清理现有的AirdPro镜像..."
        docker rmi -f airdpro:windows airdpro:linux airdpro:cli airdpro:dev 2>/dev/null || true
        print_success "镜像清理完成"
    fi
}

# 构建Windows版本
build_windows_version() {
    print_info "构建Windows版本镜像..."
    local build_cmd="docker build --target runtime-windows -t airdpro:windows ."
    
    if [ "$NO_CACHE" = true ]; then
        build_cmd="$build_cmd --no-cache"
    fi
    
    if $build_cmd; then
        print_success "Windows版本镜像构建成功"
    else
        print_error "Windows版本镜像构建失败"
        return 1
    fi
}

# 构建Linux/macOS版本
build_linux_version() {
    print_info "构建Linux/macOS版本镜像..."
    local build_cmd="docker build --target runtime-linux -t airdpro:linux ."
    
    if [ "$NO_CACHE" = true ]; then
        build_cmd="$build_cmd --no-cache"
    fi
    
    if $build_cmd; then
        print_success "Linux/macOS版本镜像构建成功"
    else
        print_error "Linux/macOS版本镜像构建失败"
        return 1
    fi
}

# 构建CLI版本
build_cli_version() {
    print_info "构建CLI版本镜像..."
    local build_cmd="docker build --target runtime-linux -t airdpro:cli --build-arg CLI_MODE=true ."
    
    if [ "$NO_CACHE" = true ]; then
        build_cmd="$build_cmd --no-cache"
    fi
    
    if $build_cmd; then
        print_success "CLI版本镜像构建成功"
    else
        print_error "CLI版本镜像构建失败"
        return 1
    fi
}

# 构建开发版本
build_dev_version() {
    print_info "构建开发版本镜像..."
    local build_cmd="docker build --target runtime-linux -t airdpro:dev --build-arg DEV_MODE=true ."
    
    if [ "$NO_CACHE" = true ]; then
        build_cmd="$build_cmd --no-cache"
    fi
    
    if $build_cmd; then
        print_success "开发版本镜像构建成功"
    else
        print_error "开发版本镜像构建失败"
        return 1
    fi
}

# 创建必要目录
create_directories() {
    print_info "创建必要目录..."
    mkdir -p data logs
    print_success "目录创建完成"
}

# 显示构建结果
show_results() {
    echo
    print_info "构建结果:"
    echo
    docker images | grep "airdpro" | head -10
}

# 显示使用说明
show_usage() {
    echo
    print_success "🎉 AirdPro跨平台Docker镜像构建成功！"
    echo
    echo -e "${CYAN}可用镜像:${NC}"
    echo "  • airdpro:windows  - Windows原生容器版本"
    echo "  • airdpro:linux    - Linux/macOS版本（使用Wine）"
    echo "  • airdpro:cli      - 命令行版本（无GUI）"
    echo "  • airdpro:dev      - 开发版本（带调试）"
    echo
    echo -e "${CYAN}运行命令:${NC}"
    echo "  ./run-gui-en.sh      # 运行GUI版本（macOS/Linux）"
    echo "  ./run-cli-en.sh      # 运行CLI版本"
    echo "  ./test-docker-en.sh  # 运行测试脚本"
    echo
    echo -e "${CYAN}更多详细信息请查看CROSS-PLATFORM-GUIDE.md${NC}"
    echo
}

# 主函数
main() {
    print_header "AirdPro 跨平台Docker构建"
    
    # 检查Docker环境
    check_docker
    
    # 清理现有镜像（如果需要）
    clean_existing_images
    
    # 创建目录
    create_directories
    
    echo
    print_info "开始构建Docker镜像..."
    echo "这可能需要较长时间，请耐心等待..."
    echo
    
    local build_failed=false
    
    if [ "$WINDOWS_ONLY" = true ]; then
        build_windows_version || build_failed=true
    elif [ "$LINUX_ONLY" = true ]; then
        build_linux_version || build_failed=true
        build_cli_version || build_failed=true
        build_dev_version || build_failed=true
    elif [ "$BUILD_ALL" = true ]; then
        build_windows_version || build_failed=true
        echo
        build_linux_version || build_failed=true
        echo
        build_cli_version || build_failed=true
        echo
        build_dev_version || build_failed=true
    fi
    
    if [ "$build_failed" = true ]; then
        print_error "构建过程中出现错误"
        exit 1
    fi
    
    # 显示结果
    show_results
    show_usage
}

# 执行主函数
main "$@"
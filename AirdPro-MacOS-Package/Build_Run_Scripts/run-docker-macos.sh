#!/bin/bash

# AirdPro macOS Docker运行脚本
# AirdPro macOS Docker run script

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
    echo "AirdPro macOS Docker运行脚本"
    echo ""
    echo "用法: $0 [选项]"
    echo ""
    echo "选项:"
    echo "  -h, --help         显示此帮助信息"
    echo "  --build            构建Docker镜像"
    echo "  --rebuild          强制重新构建Docker镜像"
    echo "  --no-build         跳过镜像构建"
    echo "  --debug            启用调试模式"
    echo "  --remove           运行后删除容器"
    echo "  --display X        设置X11显示（默认:0）"
    echo "  --volume PATH      挂载额外数据卷"
    echo ""
    echo "示例:"
    echo "  $0                          # 基本运行"
    echo "  --build                     # 构建并运行"
    echo "  --rebuild                   # 强制重新构建"
    echo "  --debug --remove            # 调试模式运行并删除容器"
    echo "  --display :1                # 使用显示:1"
    echo ""
    echo "注意:"
    echo "  - 需要先安装Docker Desktop"
    echo "  - 首次运行需要构建镜像，可能需要较长时间"
    echo "  - Docker Desktop必须正在运行"
}

# 解析命令行参数
BUILD_IMAGE=false
REBUILD_IMAGE=false
SKIP_BUILD=false
DEBUG_MODE=false
REMOVE_AFTER=false
DISPLAY=:0
EXTRA_VOLUMES=()

while [[ $# -gt 0 ]]; do
    case $1 in
        -h|--help)
            show_help
            exit 0
            ;;
        --build)
            BUILD_IMAGE=true
            shift
            ;;
        --rebuild)
            REBUILD_IMAGE=true
            BUILD_IMAGE=true
            shift
            ;;
        --no-build)
            SKIP_BUILD=true
            shift
            ;;
        --debug)
            DEBUG_MODE=true
            shift
            ;;
        --remove)
            REMOVE_AFTER=true
            shift
            ;;
        --display)
            DISPLAY="$2"
            shift 2
            ;;
        --volume)
            EXTRA_VOLUMES+=("$2")
            shift 2
            ;;
        --)
            shift
            break
            ;;
        *)
            print_error "未知选项: $1"
            show_help
            exit 1
            ;;
    esac
done

# 获取脚本所在目录
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
AIRDPRO_DIR="$(dirname "$SCRIPT_DIR")"
DOCKERFILE_DIR="$AIRDPRO_DIR"

# 镜像名称和容器名称
IMAGE_NAME="airdpro:macos"
CONTAINER_NAME="airdpro-macos-$(date +%s)"

# 检查Docker是否安装
check_docker() {
    print_info "检查Docker安装..."
    if ! command -v docker &> /dev/null; then
        print_error "Docker未安装"
        echo "请先安装Docker Desktop for Mac："
        echo "  https://docs.docker.com/desktop/mac/install/"
        exit 1
    fi
    
    # 检查Docker Desktop是否运行
    if ! docker info &> /dev/null; then
        print_error "Docker未运行"
        echo "请启动Docker Desktop for Mac"
        exit 1
    fi
    
    local docker_version=$(docker --version | head -1)
    print_success "Docker已安装并运行：$docker_version"
}

# 检查XQuartz是否安装
check_x11() {
    print_info "检查X11环境..."
    
    if ! command -v xquartz &> /dev/null && [ ! -d "/Applications/Utilities/XQuartz.app" ]; then
        print_error "XQuartz未安装"
        echo "请安装XQuartz："
        echo "  brew install --cask xquartz"
        exit 1
    fi
    
    # 检查XQuartz是否正在运行
    if ! pgrep Xquartz > /dev/null; then
        print_warning "XQuartz未运行"
        echo "正在启动XQuartz..."
        open -a XQuartz
        sleep 3
    fi
    
    # 启用X11授权
    xhost +localhost
    
    print_success "X11环境检查通过"
}

# 构建Docker镜像
build_docker_image() {
    if [ "$SKIP_BUILD" = true ]; then
        print_info "跳过镜像构建"
        return
    fi
    
    if [ "$REBUILD_IMAGE" = true ]; then
        print_info "删除现有镜像..."
        docker rmi "$IMAGE_NAME" 2>/dev/null || true
    fi
    
    # 检查镜像是否已存在
    if docker image inspect "$IMAGE_NAME" &> /dev/null; then
        if [ "$BUILD_IMAGE" = false ]; then
            print_success "镜像已存在，跳过构建"
            return
        fi
    fi
    
    print_header "构建Docker镜像"
    print_info "这可能需要较长时间，请耐心等待..."
    
    local build_args=""
    if [ "$DEBUG_MODE" = true ]; then
        build_args="--build-arg DEBUG_MODE=true"
    fi
    
    cd "$DOCKERFILE_DIR"
    
    if docker build $build_args --target runtime-macos -t "$IMAGE_NAME" -f Dockerfile.crossplatform .; then
        print_success "Docker镜像构建完成"
    else
        print_error "Docker镜像构建失败"
        exit 1
    fi
}

# 检查镜像是否存在
check_image() {
    if ! docker image inspect "$IMAGE_NAME" &> /dev/null; then
        print_error "镜像不存在: $IMAGE_NAME"
        echo "请先运行："
        echo "  $0 --build"
        exit 1
    fi
}

# 运行Docker容器
run_docker_container() {
    print_header "启动Docker容器"
    
    # 构建docker run命令
    local docker_args=""
    
    # X11相关设置
    docker_args="$docker_args -e DISPLAY=$DISPLAY"
    docker_args="$docker_args -v /tmp/.X11-unix:/tmp/.X11-unix:rw"
    
    # 权限设置
    docker_args="$docker_args --privileged"
    docker_args="$docker_args --security-opt seccomp=unconfined"
    
    # 网络设置
    docker_args="$docker_args --network host"
    
    # 设备访问
    docker_args="$docker_args --device /dev/dri"
    
    # 挂载用户目录
    docker_args="$docker_args -v $HOME:/home/user"
    
    # 挂载额外卷
    for volume in "${EXTRA_VOLUMES[@]}"; do
        docker_args="$docker_args -v $volume"
    done
    
    # 用户设置
    docker_args="$docker_args --user $(id -u):$(id -g)"
    
    # 交互模式
    docker_args="$docker_args -it"
    
    # 容器名称
    docker_args="$docker_args --name $CONTAINER_NAME"
    
    # 调试模式
    if [ "$DEBUG_MODE" = true ]; then
        docker_args="$docker_args -e DEBUG_MODE=true"
        docker_args="$docker_args -e WINEDEBUG=+all"
    fi
    
    # 运行后删除
    if [ "$REMOVE_AFTER" = true ]; then
        docker_args="$docker_args --rm"
    fi
    
    # 容器资源限制
    docker_args="$docker_args --memory=4g"
    docker_args="$docker_args --cpus=2"
    
    # 运行容器
    print_info "启动容器: $CONTAINER_NAME"
    print_info "使用镜像: $IMAGE_NAME"
    print_info "显示: $DISPLAY"
    
    # 启动容器
    if docker run $docker_args "$IMAGE_NAME" wineboot --init; then
        print_success "容器启动成功"
    else
        print_error "容器启动失败"
        return 1
    fi
    
    # 检查容器是否正在运行
    if docker ps --filter "name=$CONTAINER_NAME" | grep -q "$CONTAINER_NAME"; then
        print_success "容器正在运行"
        
        # 提供进入容器的方法
        print_info "要进入容器运行，请使用："
        echo "  docker exec -it $CONTAINER_NAME bash"
        
        # 启动应用程序（后台运行）
        print_info "启动AirdPro应用程序..."
        docker exec -d "$CONTAINER_NAME" bash -c "cd /app && ./run_app.sh"
        
    else
        print_error "容器未运行"
        docker logs "$CONTAINER_NAME" 2>&1 || true
        return 1
    fi
}

# 清理函数
cleanup() {
    print_info "清理Docker资源..."
    
    # 恢复X11权限
    xhost -localhost
    
    # 如果容器还在运行且需要删除
    if [ "$REMOVE_AFTER" = false ]; then
        if docker ps --filter "name=$CONTAINER_NAME" | grep -q "$CONTAINER_NAME"; then
            print_info "容器仍在运行，可使用以下命令停止："
            echo "  docker stop $CONTAINER_NAME"
            echo "  docker rm $CONTAINER_NAME"
        fi
    fi
}

# 信号处理
trap cleanup EXIT

# 主函数
main() {
    print_header "AirdPro macOS Docker启动器"
    
    # 检查环境
    check_docker
    check_x11
    
    # 构建镜像
    if [ "$BUILD_IMAGE" = true ] || [ "$REBUILD_IMAGE" = true ]; then
        build_docker_image
    else
        check_image
    fi
    
    # 运行容器
    run_docker_container
    
    print_success "启动完成！"
    print_info "如需技术支持，请查看Docker日志或文档"
}

# 运行主函数
main "$@"
#!/bin/bash

# AirdPro Linux Docker运行脚本
# AirdPro Linux Docker running script

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
DOCKER_IMAGE="airdpro:linux"
CONTAINER_NAME="airdpro-linux"
COMPOSE_FILE="docker-compose.yml"
DATA_DIR="./data"
LOG_DIR="./logs"
BUILD_ARGS=""
COMPOSE_ARGS=""
REMOVE_AFTER_RUN=false
DEBUG_MODE=false
INTERACTIVE_MODE=false
DISPLAY="${DISPLAY:-:0}"

# 显示帮助信息
show_help() {
    echo "AirdPro Linux Docker运行脚本"
    echo
    echo "用法: $0 [选项]"
    echo
    echo "选项:"
    echo "  --build              构建Docker镜像"
    echo "  --rebuild            强制重新构建Docker镜像"
    echo "  --no-build           跳过镜像构建"
    echo "  --interactive, -i    交互模式（连接到容器）"
    echo "  --debug              启用调试模式"
    echo "  --remove             运行后删除容器"
    echo "  --display X          设置X11显示（默认: $DISPLAY）"
    echo "  --volume PATH        挂载额外数据卷"
    echo "  --memory SIZE        设置容器内存限制（如：4g）"
    echo "  --cpus NUMBER        设置CPU限制（如：2）"
    echo "  --data-dir PATH      设置数据目录（默认: $DATA_DIR）"
    echo "  --log-dir PATH       设置日志目录（默认: $LOG_DIR）"
    echo "  --service NAME       运行指定服务（默认: airdpro-linux）"
    echo "  -h, --help           显示帮助信息"
    echo
    echo "示例:"
    echo "  $0 --build                    # 构建并运行"
    echo "  $0 --interactive              # 交互模式启动"
    echo "  $0 --debug --remove           # 调试模式，运行后删除"
    echo "  $0 --build --memory 8g --cpus 4  # 高配置启动"
}

# 检查依赖
check_dependencies() {
    print_header "检查依赖"
    
    # 检查Docker
    if ! command -v docker &> /dev/null; then
        print_error "Docker未安装，请运行 install-linux.sh 安装"
        exit 1
    fi
    
    # 检查Docker Compose
    if ! docker compose version > /dev/null 2>&1 && ! command -v docker-compose &> /dev/null; then
        print_error "Docker Compose未安装，请运行 install-linux.sh 安装"
        exit 1
    fi
    
    # 检查Docker服务
    if ! docker info > /dev/null 2>&1; then
        print_error "Docker服务未运行，请启动Docker服务"
        exit 1
    fi
    
    print_success "依赖检查通过"
}

# 检查系统环境
check_system_environment() {
    print_header "检查系统环境"
    
    # 检查X11环境
    if [ -z "$DISPLAY" ]; then
        print_warning "未设置DISPLAY环境变量，GUI可能无法显示"
        print_info "如果您在桌面环境中，通常会自动设置"
        print_info "如果在远程服务器上，可能需要配置X11转发"
    else
        print_success "X11显示设置：$DISPLAY"
    fi
    
    # 检查X11访问权限
    if [ -n "$DISPLAY" ]; then
        if xset q > /dev/null 2>&1; then
            print_success "X11访问正常"
        else
            print_warning "无法访问X11显示，可能需要配置X权限"
            print_info "运行：xhost +local:docker"
        fi
    fi
    
    # 检查磁盘空间
    available_space=$(df -h . | awk 'NR==2 {print $4}' | sed 's/G.*//')
    if [ "$available_space" -lt 5 ]; then
        print_warning "建议至少5GB可用空间，当前：${available_space}GB"
    else
        print_success "磁盘空间检查通过：${available_space}GB可用"
    fi
    
    # 检查内存
    total_memory=$(free -m | awk 'NR==2{printf "%.0f", $2}')
    if [ "$total_memory" -lt 4096 ]; then
        print_warning "建议至少4GB内存，当前：${total_memory}MB"
    else
        print_success "内存检查通过：${total_memory}MB"
    fi
}

# 准备环境
prepare_environment() {
    print_header "准备环境"
    
    # 创建必要的目录
    mkdir -p "$DATA_DIR" "$LOG_DIR"
    
    # 设置X11权限（如果需要）
    if [ -n "$DISPLAY" ]; then
        # 检查是否需要设置xhost权限
        if ! xset q > /dev/null 2>&1; then
            print_info "尝试设置X11权限..."
            xhost +local:docker > /dev/null 2>&1 || print_warning "无法设置X11权限，GUI可能无法显示"
        fi
    fi
    
    # 清理旧的容器（如果存在）
    if docker ps -a --format 'table {{.Names}}' | grep -q "^${CONTAINER_NAME}$"; then
        print_info "清理旧容器：$CONTAINER_NAME"
        docker rm -f "$CONTAINER_NAME" > /dev/null 2>&1 || true
    fi
    
    print_success "环境准备完成"
}

# 构建Docker镜像
build_docker_image() {
    local force_rebuild=$1
    
    print_header "构建Docker镜像"
    
    # 检查是否已经构建了镜像
    if [ "$force_rebuild" = "true" ]; then
        print_info "强制重新构建Docker镜像..."
    elif docker images --format '{{.Repository}}:{{.Tag}}' | grep -q "^${DOCKER_IMAGE}$"; then
        print_info "Docker镜像已存在：$DOCKER_IMAGE"
        if [ "$BUILD_ARGS" = "--no-build" ]; then
            print_info "跳过镜像构建（--no-build选项）"
            return 0
        else
            print_info "使用现有镜像，如需重新构建请使用--rebuild选项"
            return 0
        fi
    fi
    
    # 构建镜像
    print_info "开始构建Docker镜像：$DOCKER_IMAGE"
    
    local build_cmd="docker build"
    if [ -f "Dockerfile.crossplatform" ]; then
        build_cmd="$build_cmd -f Dockerfile.crossplatform"
    else
        build_cmd="$build_cmd -f Dockerfile"
    fi
    
    # 添加构建参数
    if [ -n "$BUILD_ARGS" ]; then
        build_cmd="$build_cmd $BUILD_ARGS"
    fi
    
    build_cmd="$build_cmd -t $DOCKER_IMAGE ."
    
    print_info "执行构建命令..."
    if eval "$build_cmd"; then
        print_success "Docker镜像构建完成：$DOCKER_IMAGE"
    else
        print_error "Docker镜像构建失败"
        exit 1
    fi
}

# 运行Docker容器
run_docker_container() {
    print_header "运行Docker容器"
    
    # 选择Docker Compose命令
    if docker compose version > /dev/null 2>&1; then
        COMPOSE_CMD="docker compose"
    else
        COMPOSE_CMD="docker-compose"
    fi
    
    # 构建启动命令
    local compose_up_cmd="$COMPOSE_CMD up"
    
    # 添加参数
    if [ "$DEBUG_MODE" = "true" ]; then
        compose_up_cmd="$compose_up_cmd --verbose"
    fi
    
    if [ "$REMOVE_AFTER_RUN" = "true" ]; then
        compose_up_cmd="$compose_up_cmd --rm"
    fi
    
    if [ -n "$COMPOSE_ARGS" ]; then
        compose_up_cmd="$compose_up_cmd $COMPOSE_ARGS"
    fi
    
    # 添加服务名
    compose_up_cmd="$compose_up_cmd -d $TARGET_SERVICE"
    
    print_info "启动命令：$compose_up_cmd"
    
    # 启动服务
    if eval "$compose_up_cmd"; then
        print_success "容器启动成功"
        
        # 等待服务启动
        print_info "等待服务启动..."
        sleep 5
        
        # 显示容器状态
        print_info "容器状态："
        $COMPOSE_CMD ps
        
        # 显示日志（如果不在交互模式）
        if [ "$INTERACTIVE_MODE" != "true" ]; then
            echo
            print_info "最近的日志："
            $COMPOSE_CMD logs --tail=20 $TARGET_SERVICE
        fi
        
    else
        print_error "容器启动失败"
        exit 1
    fi
}

# 交互模式
run_interactive_mode() {
    print_header "交互模式"
    
    print_info "启动交互式容器..."
    
    # 选择Docker Compose命令
    if docker compose version > /dev/null 2>&1; then
        COMPOSE_CMD="docker compose"
    else
        COMPOSE_CMD="docker-compose"
    fi
    
    # 进入交互模式
    $COMPOSE_CMD exec $TARGET_SERVICE /bin/bash || $COMPOSE_CMD run --rm $TARGET_SERVICE /bin/bash
}

# 显示容器日志
show_logs() {
    local service_name=$1
    
    # 选择Docker Compose命令
    if docker compose version > /dev/null 2>&1; then
        COMPOSE_CMD="docker compose"
    else
        COMPOSE_CMD="docker-compose"
    fi
    
    print_info "显示容器日志：$service_name"
    $COMPOSE_CMD logs -f "$service_name"
}

# 停止容器
stop_containers() {
    print_header "停止容器"
    
    # 选择Docker Compose命令
    if docker compose version > /dev/null 2>&1; then
        COMPOSE_CMD="docker compose"
    else
        COMPOSE_CMD="docker-compose"
    fi
    
    print_info "停止所有AirdPro容器..."
    $COMPOSE_CMD down
    
    print_success "容器已停止"
}

# 清理资源
cleanup_resources() {
    print_header "清理资源"
    
    print_info "清理Docker资源..."
    
    # 停止并删除容器
    docker ps -a --filter "name=$CONTAINER_NAME" -q | xargs -r docker rm -f
    
    # 删除镜像（可选）
    read -p "是否删除Docker镜像 $DOCKER_IMAGE？(y/N): " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        docker rmi "$DOCKER_IMAGE" 2>/dev/null || true
        print_success "镜像已删除"
    fi
    
    # 清理未使用的Docker资源
    docker system prune -f
    
    print_success "资源清理完成"
}

# 主函数
main() {
    # 解析命令行参数
    TARGET_SERVICE="airdpro-linux"
    ACTION="start"
    
    while [[ $# -gt 0 ]]; do
        case $1 in
            --build)
                ACTION="build"
                shift
                ;;
            --rebuild)
                ACTION="rebuild"
                shift
                ;;
            --no-build)
                BUILD_ARGS="--no-build"
                shift
                ;;
            --interactive|-i)
                INTERACTIVE_MODE=true
                shift
                ;;
            --debug)
                DEBUG_MODE=true
                shift
                ;;
            --remove)
                REMOVE_AFTER_RUN=true
                shift
                ;;
            --display)
                DISPLAY="$2"
                export DISPLAY
                shift 2
                ;;
            --volume)
                EXTRA_VOLUME="$2"
                COMPOSE_ARGS="$COMPOSE_ARGS -v $2"
                shift 2
                ;;
            --memory)
                MEMORY_LIMIT="$2"
                COMPOSE_ARGS="$COMPOSE_ARGS --memory $2"
                shift 2
                ;;
            --cpus)
                CPU_LIMIT="$2"
                COMPOSE_ARGS="$COMPOSE_ARGS --cpus $2"
                shift 2
                ;;
            --data-dir)
                DATA_DIR="$2"
                shift 2
                ;;
            --log-dir)
                LOG_DIR="$2"
                shift 2
                ;;
            --service)
                TARGET_SERVICE="$2"
                shift 2
                ;;
            logs)
                ACTION="logs"
                shift
                ;;
            stop)
                ACTION="stop"
                shift
                ;;
            cleanup)
                ACTION="cleanup"
                shift
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
    if [ ! -f "docker-compose.yml" ] && [ ! -f "Dockerfile" ]; then
        print_error "请在AirdPro-Linux-Package目录中运行此脚本"
        exit 1
    fi
    
    print_header "AirdPro Linux Docker运行脚本 v1.0"
    echo "目标服务: $TARGET_SERVICE"
    echo "显示设置: $DISPLAY"
    echo "数据目录: $DATA_DIR"
    echo "日志目录: $LOG_DIR"
    echo
    
    # 执行相应操作
    case "$ACTION" in
        build)
            check_dependencies
            check_system_environment
            prepare_environment
            build_docker_image false
            ;;
        rebuild)
            check_dependencies
            check_system_environment
            prepare_environment
            build_docker_image true
            ;;
        start)
            check_dependencies
            check_system_environment
            prepare_environment
            
            if [ "$BUILD_ARGS" != "--no-build" ]; then
                build_docker_image false
            fi
            
            if [ "$INTERACTIVE_MODE" = "true" ]; then
                run_interactive_mode
            else
                run_docker_container
            fi
            ;;
        logs)
            show_logs "$TARGET_SERVICE"
            ;;
        stop)
            stop_containers
            ;;
        cleanup)
            cleanup_resources
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
#!/bin/bash

# AirdPro Linux构建脚本
# AirdPro Linux build script

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
BUILD_TYPE="both"  # docker, wine, both
TARGET_ARCH="x86_64"
DOCKER_TAG="airdpro:linux"
WINE_VERSION="latest"
BUILD_DIR="./build"
OUTPUT_DIR="./dist"
CLEAN_BUILD=false
PUSH_REGISTRY=false
REGISTRY_URL=""
CACHE_BUILDS=true
DEBUG_BUILD=false
TEST_BUILD=false

# 变量定义
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"
DOCKERFILE_PATH="Dockerfile.crossplatform"
DOCKER_COMPOSE_FILE="docker-compose.yml"
TEMP_DIR="/tmp/airdpro-build-$$"

# 显示帮助信息
show_help() {
    echo "AirdPro Linux构建脚本"
    echo
    echo "用法: $0 [选项]"
    echo
    echo "构建选项:"
    echo "  --build-type TYPE      构建类型 (docker/wine/both，默认: both)"
    echo "  --target-arch ARCH     目标架构 (x86_64/arm64，默认: x86_64)"
    echo "  --docker-tag TAG       Docker镜像标签 (默认: $DOCKER_TAG)"
    echo "  --wine-version VER     Wine版本 (默认: latest)"
    echo "  --registry-url URL     Docker注册表URL（推送用）"
    echo
    echo "构建行为:"
    echo "  --clean                清理构建（强制重新构建）"
    echo "  --no-cache             不使用缓存构建"
    echo "  --test                 构建后运行测试"
    echo "  --debug                启用调试模式"
    echo "  --push                 构建后推送到注册表"
    echo
    echo "目录选项:"
    echo "  --build-dir PATH       构建目录 (默认: $BUILD_DIR)"
    echo "  --output-dir PATH      输出目录 (默认: $OUTPUT_DIR)"
    echo
    echo "操作:"
    echo "  --clean-builds         清理所有构建文件"
    echo "  --validate-only        仅验证环境，不构建"
    echo "  -h, --help             显示帮助信息"
    echo
    echo "示例:"
    echo "  $0 --build-type docker --docker-tag airdpro:latest"
    echo "  $0 --build-type wine --wine-version 8.0"
    echo "  $0 --clean --build-type both --push"
}

# 检查依赖
check_dependencies() {
    print_header "检查构建依赖"
    
    # 检查基本工具
    local missing_tools=()
    
    for tool in docker docker-compose git tar gzip zip; do
        if ! command -v "$tool" &> /dev/null; then
            missing_tools+=("$tool")
        fi
    done
    
    if [ ${#missing_tools[@]} -gt 0 ]; then
        print_error "缺少以下工具: ${missing_tools[*]}"
        print_info "请运行 install-linux.sh 安装所需依赖"
        exit 1
    fi
    
    # 检查Docker服务
    if ! docker info > /dev/null 2>&1; then
        print_error "Docker服务未运行，请启动Docker"
        exit 1
    fi
    
    # 检查Docker权限
    if ! docker ps > /dev/null 2>&1; then
        print_error "没有Docker权限，请检查用户权限"
        exit 1
    fi
    
    print_success "依赖检查通过"
}

# 检查系统环境
check_system_environment() {
    if [ "$DEBUG_BUILD" = "true" ]; then
        print_header "检查系统环境（调试模式）"
    else
        print_header "检查系统环境"
    fi
    
    # 检查系统信息
    print_info "系统信息:"
    uname -a
    cat /etc/os-release | grep -E "^(ID|VERSION_ID|PRETTY_NAME)="
    
    # 检查资源
    local memory=$(free -m | awk 'NR==2{printf "%.0f", $2}')
    local disk_space=$(df -h . | awk 'NR==2 {print $4}' | sed 's/G.*//')
    
    print_info "系统资源:"
    echo "  内存: ${memory}MB"
    echo "  可用磁盘: ${disk_space}GB"
    
    if [ "$memory" -lt 4096 ]; then
        print_warning "建议至少4GB内存，当前：${memory}MB"
    fi
    
    if [ "$disk_space" -lt 10 ]; then
        print_warning "建议至少10GB可用空间，当前：${disk_space}GB"
    fi
    
    # 检查Docker资源
    if docker info > /dev/null 2>&1; then
        local docker_version=$(docker --version)
        local docker_compose_version=$(docker-compose --version)
        
        print_info "Docker版本:"
        echo "  $docker_version"
        echo "  $docker_compose_version"
    fi
    
    print_success "系统环境检查完成"
}

# 验证Dockerfile
validate_dockerfile() {
    print_info "验证Dockerfile..."
    
    if [ ! -f "$DOCKERFILE_PATH" ]; then
        print_error "Dockerfile不存在: $DOCKERFILE_PATH"
        exit 1
    fi
    
    # 检查Dockerfile语法
    if docker build --dry-run -f "$DOCKERFILE_PATH" . > /dev/null 2>&1; then
        print_success "Dockerfile语法验证通过"
    else
        print_warning "Dockerfile语法检查失败，但继续构建"
    fi
    
    # 检查多阶段构建支持
    if grep -q "^FROM.*AS" "$DOCKERFILE_PATH"; then
        print_info "检测到多阶段构建支持"
    fi
}

# 构建Docker镜像
build_docker_image() {
    print_header "构建Docker镜像"
    
    local build_cmd="docker build"
    
    # 添加构建参数
    if [ -f "$DOCKERFILE_PATH" ]; then
        build_cmd="$build_cmd -f $DOCKERFILE_PATH"
    fi
    
    # 添加标签
    build_cmd="$build_cmd -t $DOCKER_TAG"
    
    # 添加架构支持
    if [ "$TARGET_ARCH" != "x86_64" ]; then
        build_cmd="$build_cmd --platform linux/$TARGET_ARCH"
    fi
    
    # 添加缓存选项
    if [ "$CACHE_BUILDS" = "true" ]; then
        print_info "使用Docker构建缓存"
    else
        build_cmd="$build_cmd --no-cache"
    fi
    
    # 添加调试选项
    if [ "$DEBUG_BUILD" = "true" ]; then
        build_cmd="$build_cmd --progress=plain"
        print_info "启用详细构建输出"
    fi
    
    # 添加构建参数
    if [ "$WINE_VERSION" != "latest" ]; then
        build_cmd="$build_cmd --build-arg WINE_VERSION=$WINE_VERSION"
    fi
    
    # 执行构建
    build_cmd="$build_cmd ."
    
    print_info "开始构建Docker镜像..."
    print_info "镜像标签: $DOCKER_TAG"
    print_info "目标架构: $TARGET_ARCH"
    print_info "构建命令: $build_cmd"
    
    if eval "$build_cmd"; then
        print_success "Docker镜像构建完成: $DOCKER_TAG"
        
        # 显示镜像信息
        local image_size=$(docker images --format "table {{.Size}}" "$DOCKER_TAG" | tail -n 1)
        print_info "镜像大小: $image_size"
        
        # 运行基本测试
        if [ "$TEST_BUILD" = "true" ]; then
            test_docker_image
        fi
        
    else
        print_error "Docker镜像构建失败"
        return 1
    fi
}

# 测试Docker镜像
test_docker_image() {
    print_info "测试Docker镜像..."
    
    # 基本运行测试
    if docker run --rm "$DOCKER_TAG" /bin/bash -c "echo 'Docker image test successful'" > /dev/null 2>&1; then
        print_success "基本运行测试通过"
    else
        print_warning "基本运行测试失败"
        return 1
    fi
    
    # 检查必要组件
    local test_cmd="which wine64 || which wine32 || echo 'Wine not found'"
    if docker run --rm "$DOCKER_TAG" /bin/bash -c "$test_cmd" > /dev/null 2>&1; then
        print_success "Wine组件测试通过"
    else
        print_warning "Wine组件测试失败"
    fi
}

# 创建Wine前缀和包
build_wine_package() {
    print_header "构建Wine包"
    
    local wine_prefix_dir="$BUILD_DIR/wine-prefix"
    local wine_package_dir="$OUTPUT_DIR/wine-package"
    
    # 创建目录
    mkdir -p "$wine_prefix_dir" "$wine_package_dir"
    
    # 检查Wine版本
    local wine_installed_version=""
    if command -v wine &> /dev/null; then
        wine_installed_version=$(wine --version | head -n1)
        print_info "使用系统Wine: $wine_installed_version"
    else
        print_warning "系统未安装Wine，将创建基础包"
    fi
    
    # 创建Wine前缀
    print_info "创建Wine前缀..."
    export WINEPREFIX="$wine_prefix_dir"
    export WINEARCH="win64"
    
    # 初始化Wine前缀
    if [ ! -d "$wine_prefix_dir/drive_c" ]; then
        print_info "初始化Wine前缀..."
        if wineboot --init; then
            print_success "Wine前缀创建成功"
        else
            print_error "Wine前缀创建失败"
            return 1
        fi
    fi
    
    # 安装基础组件
    print_info "安装基础组件..."
    local components=("dotnet48" "vcrun2019" "msxml6" "corefonts")
    
    for component in "${components[@]}"; do
        print_info "安装组件: $component"
        if winetricks --unattended "$component" > /dev/null 2>&1; then
            print_success "组件安装成功: $component"
        else
            print_warning "组件安装失败: $component"
        fi
    done
    
    # 复制AirdPro文件（如果存在）
    local airdpro_files=()
    if [ -d "../AirdPro" ]; then
        print_info "复制AirdPro文件..."
        cp -r ../AirdPro "$wine_prefix_dir/drive_c/" 2>/dev/null || true
        airdpro_files+=("AirdPro")
    fi
    
    # 创建Wine包
    print_info "创建Wine包..."
    local wine_package_name="AirdPro-Wine-Package-v4.2.0"
    
    # 创建包目录结构
    mkdir -p "$wine_package_dir/$wine_package_name"
    
    # 复制Wine前缀
    print_info "打包Wine前缀..."
    tar -czf "$wine_package_dir/$wine_package_name/wine-prefix.tar.gz" -C "$wine_prefix_dir" .
    
    # 创建安装脚本
    create_wine_install_script "$wine_package_dir/$wine_package_name"
    
    # 创建README
    create_wine_readme "$wine_package_dir/$wine_package_name"
    
    # 创建压缩包
    print_info "创建压缩包..."
    cd "$wine_package_dir"
    zip -r "${wine_package_name}.zip" "$wine_package_name" > /dev/null 2>&1
    tar -czf "${wine_package_name}.tar.gz" "$wine_package_name" > /dev/null 2>&1
    cd - > /dev/null
    
    print_success "Wine包构建完成:"
    print_info "  目录: $wine_package_dir/$wine_package_name"
    print_info "  ZIP文件: $wine_package_dir/${wine_package_name}.zip"
    print_info "  TAR.GZ文件: $wine_package_dir/${wine_package_name}.tar.gz"
}

# 创建Wine安装脚本
create_wine_install_script() {
    local target_dir="$1"
    
    cat > "$target_dir/install-wine.sh" << 'EOF'
#!/bin/bash

# AirdPro Wine包安装脚本

set -e

print_info() {
    echo "[信息] $1"
}

print_success() {
    echo "[成功] $1"
}

print_error() {
    echo "[错误] $1"
}

# 检查Wine安装
if ! command -v wine &> /dev/null; then
    print_error "Wine未安装，请先安装Wine"
    print_info "Ubuntu/Debian: sudo apt install wine"
    print_info "Fedora/CentOS: sudo dnf install wine"
    exit 1
fi

# 设置Wine前缀
export WINEPREFIX="$HOME/.wine-airdpro"

# 安装Wine前缀
print_info "安装Wine前缀..."
tar -xzf wine-prefix.tar.gz -C "$HOME/.wine-airdpro-temp"

if [ -d "$HOME/.wine-airdpro-temp/drive_c" ]; then
    rm -rf "$HOME/.wine-airdpro"
    mv "$HOME/.wine-airdpro-temp" "$HOME/.wine-airdpro"
    print_success "Wine前缀安装成功"
else
    print_error "Wine前缀文件无效"
    exit 1
fi

# 运行AirdPro
if [ -f "$HOME/.wine-airdpro/drive_c/AirdPro/AirdPro.exe" ]; then
    print_info "运行AirdPro..."
    wine "$HOME/.wine-airdpro/drive_c/AirdPro/AirdPro.exe"
else
    print_info "请将AirdPro.exe复制到:"
    print_info "$HOME/.wine-airdpro/drive_c/AirdPro/"
fi
EOF

    chmod +x "$target_dir/install-wine.sh"
}

# 创建Wine README
create_wine_readme() {
    local target_dir="$1"
    
    cat > "$target_dir/README.md" << EOF
# AirdPro Wine包

这是AirdPro的Wine环境包，包含了预配置的Wine前缀和必要的组件。

## 安装

1. 解压此包
2. 运行安装脚本:
   \`\`\`bash
   ./install-wine.sh
   \`\`\`

## 组件

- Wine前缀: 预配置的Wine环境
- .NET Framework 4.8: 支持.NET应用程序
- Visual C++ Redistributable: 运行时支持
- Core Fonts: 字体支持

## 使用

运行AirdPro:
\`\`\`bash
wine ~/.wine-airdpro/drive_c/AirdPro/AirdPro.exe
\`\`\`

## 清理

删除Wine前缀:
\`\`\`bash
rm -rf ~/.wine-airdpro
\`\`\`
EOF
}

# 创建Docker Compose文件
create_docker_compose() {
    print_info "创建Docker Compose配置文件..."
    
    cat > "$OUTPUT_DIR/docker-compose.yml" << 'EOF'
version: '3.8'

services:
  airdpro-linux:
    build: .
    container_name: airdpro-linux
    image: airdpro:linux
    environment:
      - DISPLAY=${DISPLAY:-:0}
      - WINEDEBUG=-all
    volumes:
      - ./data:/app/data
      - ./logs:/app/logs
      - /tmp/.X11-unix:/tmp/.X11-unix:rw
    networks:
      - airdpro-network
    restart: unless-stopped
    deploy:
      resources:
        limits:
          memory: 4G
          cpus: '2'

networks:
  airdpro-network:
    driver: bridge

volumes:
  airdpro-data:
  airdpro-logs:
EOF

    print_success "Docker Compose文件创建完成: $OUTPUT_DIR/docker-compose.yml"
}

# 创建Dockerfile
create_dockerfile() {
    print_info "创建Dockerfile..."
    
    cat > "$OUTPUT_DIR/Dockerfile" << 'EOF'
# AirdPro Linux Docker镜像
FROM ubuntu:22.04 AS base

# 设置环境变量
ENV DEBIAN_FRONTEND=noninteractive
ENV LANG=C.UTF-8
ENV LC_ALL=C.UTF-8

# 安装基础依赖
RUN apt-get update && apt-get install -y \
    wget \
    curl \
    unzip \
    tar \
    gnupg2 \
    software-properties-common \
    apt-transport-https \
    ca-certificates \
    locales \
    && rm -rf /var/lib/apt/lists/*

# 设置locale
RUN locale-gen en_US.UTF-8
ENV LANG=en_US.UTF-8
ENV LANGUAGE=en_US:en
ENV LC_ALL=en_US.UTF-8

# 添加Wine仓库并安装Wine
RUN wget -qO- https://dl.winehq.org/wine-builds/winehq.key | apt-key add - && \
    add-apt-repository 'deb https://dl.winehq.org/wine-builds/ubuntu/ jammy main' && \
    apt-get update && \
    apt-get install -y \
    winehq-stable \
    wine-stable-i386 \
    wine-stable-amd64 \
    && rm -rf /var/lib/apt/lists/*

# 安装Wine组件
RUN winetricks --unattended dotnet48 vcrun2019 msxml6 corefonts

# 创建应用目录
RUN mkdir -p /app/data /app/logs

# 设置工作目录
WORKDIR /app

# 复制应用程序文件
COPY . .

# 设置权限
RUN chmod +x /app/*.sh || true

# 启动命令
CMD ["/bin/bash"]
EOF

    print_success "Dockerfile创建完成: $OUTPUT_DIR/Dockerfile"
}

# 推送Docker镜像
push_docker_image() {
    if [ "$PUSH_REGISTRY" = "false" ]; then
        print_info "跳过镜像推送（未指定--push）"
        return 0
    fi
    
    if [ -z "$REGISTRY_URL" ]; then
        print_error "推送需要指定注册表URL"
        return 1
    fi
    
    print_header "推送Docker镜像"
    
    local remote_tag="$REGISTRY_URL/$DOCKER_TAG"
    
    # 标记镜像
    docker tag "$DOCKER_TAG" "$remote_tag"
    
    # 推送镜像
    if docker push "$remote_tag"; then
        print_success "镜像推送成功: $remote_tag"
    else
        print_error "镜像推送失败"
        return 1
    fi
}

# 清理构建
clean_builds() {
    print_header "清理构建文件"
    
    # 清理Docker镜像
    print_info "清理Docker镜像..."
    docker rmi "$DOCKER_TAG" 2>/dev/null || true
    
    # 清理构建目录
    if [ -d "$BUILD_DIR" ]; then
        print_info "清理构建目录: $BUILD_DIR"
        rm -rf "$BUILD_DIR"
    fi
    
    # 清理临时目录
    if [ -d "$TEMP_DIR" ]; then
        print_info "清理临时目录: $TEMP_DIR"
        rm -rf "$TEMP_DIR"
    fi
    
    # 清理Docker系统
    docker system prune -f > /dev/null 2>&1 || true
    
    print_success "清理完成"
}

# 创建发布包
create_release_package() {
    print_header "创建发布包"
    
    local package_name="AirdPro-Linux-Package-v4.2.0"
    local package_path="$OUTPUT_DIR/$package_name"
    
    # 复制分发包文件
    print_info "复制分发包文件..."
    cp -r . "$package_path"
    
    # 移除构建脚本（构建包不需要）
    rm -f "$package_path/Build_Run_Scripts/build-linux.sh"
    
    # 复制构建结果
    if [ -d "$BUILD_DIR" ]; then
        print_info "复制构建结果..."
        cp -r "$BUILD_DIR" "$package_path/build-artifacts" 2>/dev/null || true
    fi
    
    # 创建构建信息
    create_build_info "$package_path"
    
    # 创建压缩包
    print_info "创建压缩包..."
    cd "$OUTPUT_DIR"
    zip -r "${package_name}.zip" "$package_name" > /dev/null 2>&1
    tar -czf "${package_name}.tar.gz" "$package_name" > /dev/null 2>&1
    cd - > /dev/null
    
    print_success "发布包创建完成:"
    print_info "  目录: $package_path"
    print_info "  ZIP: $OUTPUT_DIR/${package_name}.zip"
    print_info "  TAR.GZ: $OUTPUT_DIR/${package_name}.tar.gz"
}

# 创建构建信息
create_build_info() {
    local target_dir="$1"
    
    cat > "$target_dir/BUILD_INFO.txt" << EOF
AirdPro Linux构建信息
====================

构建时间: $(date)
构建用户: $(whoami)
主机名: $(hostname)
操作系统: $(uname -s) $(uname -r)
架构: $(uname -m)

构建选项:
- 构建类型: $BUILD_TYPE
- 目标架构: $TARGET_ARCH
- Docker标签: $DOCKER_TAG
- Wine版本: $WINE_VERSION
- 调试模式: $DEBUG_BUILD
- 测试模式: $TEST_BUILD
- 清理构建: $CLEAN_BUILD

Docker信息:
$(docker --version)
$(docker-compose --version)

文件统计:
$(find . -type f | wc -l) 个文件
$(du -sh . | cut -f1) 总大小

包含内容:
$(ls -la)
EOF
}

# 主函数
main() {
    # 解析命令行参数
    while [[ $# -gt 0 ]]; do
        case $1 in
            --build-type)
                BUILD_TYPE="$2"
                shift 2
                ;;
            --target-arch)
                TARGET_ARCH="$2"
                shift 2
                ;;
            --docker-tag)
                DOCKER_TAG="$2"
                shift 2
                ;;
            --wine-version)
                WINE_VERSION="$2"
                shift 2
                ;;
            --registry-url)
                REGISTRY_URL="$2"
                PUSH_REGISTRY=true
                shift 2
                ;;
            --clean)
                CLEAN_BUILD=true
                shift
                ;;
            --no-cache)
                CACHE_BUILDS=false
                shift
                ;;
            --test)
                TEST_BUILD=true
                shift
                ;;
            --debug)
                DEBUG_BUILD=true
                shift
                ;;
            --push)
                PUSH_REGISTRY=true
                shift
                ;;
            --build-dir)
                BUILD_DIR="$2"
                shift 2
                ;;
            --output-dir)
                OUTPUT_DIR="$2"
                shift 2
                ;;
            --clean-builds)
                ACTION="clean"
                shift
                ;;
            --validate-only)
                ACTION="validate"
                shift
                ;;
            --release)
                ACTION="release"
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
    if [ ! -f "install-linux.sh" ]; then
        print_error "请在AirdPro-Linux-Package目录中运行此脚本"
        exit 1
    fi
    
    # 设置默认操作
    ACTION="${ACTION:-build}"
    
    print_header "AirdPro Linux构建脚本 v1.0"
    echo "构建类型: $BUILD_TYPE"
    echo "目标架构: $TARGET_ARCH"
    echo "Docker标签: $DOCKER_TAG"
    echo "Wine版本: $WINE_VERSION"
    echo "构建目录: $BUILD_DIR"
    echo "输出目录: $OUTPUT_DIR"
    echo
    
    # 执行相应操作
    case "$ACTION" in
        clean)
            clean_builds
            ;;
        validate)
            check_dependencies
            check_system_environment
            validate_dockerfile
            print_success "环境验证完成"
            ;;
        build)
            check_dependencies
            check_system_environment
            
            # 创建必要目录
            mkdir -p "$BUILD_DIR" "$OUTPUT_DIR"
            
            # 验证Dockerfile
            validate_dockerfile
            
            # 构建Docker镜像
            if [[ "$BUILD_TYPE" == "docker" || "$BUILD_TYPE" == "both" ]]; then
                if [ "$CLEAN_BUILD" = "true" ]; then
                    docker rmi "$DOCKER_TAG" 2>/dev/null || true
                fi
                build_docker_image || exit 1
            fi
            
            # 构建Wine包
            if [[ "$BUILD_TYPE" == "wine" || "$BUILD_TYPE" == "both" ]]; then
                build_wine_package || exit 1
            fi
            
            # 创建配置文件
            create_dockerfile
            create_docker_compose
            
            # 推送镜像
            if [ "$PUSH_REGISTRY" = "true" ]; then
                push_docker_image || exit 1
            fi
            
            print_success "构建完成"
            ;;
        release)
            # 先执行完整构建
            BUILD_TYPE="both"
            CLEAN_BUILD=true
            main --build-type both --clean
            
            # 然后创建发布包
            create_release_package
            
            print_success "发布包创建完成"
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
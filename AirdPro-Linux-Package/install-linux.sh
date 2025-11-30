#!/bin/bash

# AirdPro Linux 自动安装脚本
# Automatic AirdPro installation script for Linux

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

# 检测Linux发行版
detect_linux_distribution() {
    print_header "检测Linux发行版"
    
    if [ -f /etc/os-release ]; then
        . /etc/os-release
        DISTRO=$ID
        VERSION=$VERSION_ID
    elif type lsb_release >/dev/null 2>&1; then
        DISTRO=$(lsb_release -si | tr '[:upper:]' '[:lower:]')
        VERSION=$(lsb_release -sr)
    else
        print_error "无法检测Linux发行版"
        exit 1
    fi
    
    print_success "检测到系统：$PRETTY_NAME"
    echo "发行版: $DISTRO"
    echo "版本: $VERSION"
    
    # 检查支持的发行版
    case "$DISTRO" in
        ubuntu|debian)
            PACKAGE_MANAGER="apt"
            ;;
        centos|rhel|fedora)
            PACKAGE_MANAGER="yum"
            ;;
        opensuse*)
            PACKAGE_MANAGER="zypper"
            ;;
        arch|manjaro)
            PACKAGE_MANAGER="pacman"
            ;;
        *)
            print_warning "未明确支持的发行版: $DISTRO，将尝试通用安装方法"
            PACKAGE_MANAGER="apt"
            ;;
    esac
    
    echo "包管理器: $PACKAGE_MANAGER"
}

# 检查系统要求
check_system_requirements() {
    print_header "检查系统要求"
    
    # 检查是否为root用户
    if [ "$EUID" -eq 0 ]; then
        print_error "请不要使用root用户运行此脚本"
        print_info "请使用普通用户运行，脚本会在需要时请求sudo权限"
        exit 1
    fi
    
    # 检查内存
    total_memory=$(free -m | awk 'NR==2{printf "%.0f", $2}')
    if [ "$total_memory" -lt 4096 ]; then
        print_warning "建议至少4GB内存，当前：${total_memory}MB"
    else
        print_success "内存检查通过：${total_memory}MB"
    fi
    
    # 检查磁盘空间
    available_space=$(df -h . | awk 'NR==2 {print $4}' | sed 's/G.*//')
    if [ "$available_space" -lt 10 ]; then
        print_warning "建议至少10GB可用空间，当前：${available_space}GB"
    else
        print_success "磁盘空间检查通过：${available_space}GB可用"
    fi
    
    # 检查是否为64位系统
    arch=$(uname -m)
    if [ "$arch" != "x86_64" ]; then
        print_error "仅支持x86_64架构，当前架构：$arch"
        exit 1
    fi
    
    print_success "系统架构检查通过：$arch"
}

# 更新系统包
update_system_packages() {
    print_header "更新系统包"
    
    case "$PACKAGE_MANAGER" in
        apt)
            print_info "更新APT包索引..."
            sudo apt update
            sudo apt upgrade -y
            ;;
        yum)
            print_info "更新YUM包..."
            sudo yum update -y
            ;;
        zypper)
            print_info "更新Zypper包..."
            sudo zypper refresh
            sudo zypper update -y
            ;;
        pacman)
            print_info "更新Pacman包..."
            sudo pacman -Sy --noconfirm
            sudo pacman -Su --noconfirm
            ;;
    esac
    
    print_success "系统包更新完成"
}

# 安装基础依赖
install_basic_dependencies() {
    print_header "安装基础依赖"
    
    case "$PACKAGE_MANAGER" in
        apt)
            print_info "安装基础依赖包..."
            sudo apt install -y \
                curl \
                wget \
                gnupg \
                lsb-release \
                software-properties-common \
                apt-transport-https \
                ca-certificates \
                jq \
                vim \
                htop \
                unzip
            ;;
        yum)
            print_info "安装基础依赖包..."
            sudo yum install -y \
                curl \
                wget \
                gnupg2 \
                lsb_release \
                yum-utils \
                ca-certificates \
                jq \
                vim \
                htop \
                unzip
            ;;
        zypper)
            print_info "安装基础依赖包..."
            sudo zypper install -y \
                curl \
                wget \
                gnupg2 \
                lsb-release \
                software-management \
                ca-certificates \
                jq \
                vim \
                htop \
                unzip
            ;;
        pacman)
            print_info "安装基础依赖包..."
            sudo pacman -Sy --noconfirm \
                curl \
                wget \
                gnupg \
                lsb-release \
                software-management \
                ca-certificates \
                jq \
                vim \
                htop \
                unzip
            ;;
    esac
    
    print_success "基础依赖安装完成"
}

# 安装Docker
install_docker() {
    print_header "安装Docker"
    
    if command -v docker &> /dev/null; then
        print_success "Docker已安装"
        docker --version
        return 0
    fi
    
    case "$DISTRO" in
        ubuntu)
            print_info "在Ubuntu上安装Docker..."
            # 卸载旧版本
            sudo apt remove -y docker docker-engine docker.io containerd runc 2>/dev/null || true
            
            # 添加Docker官方GPG密钥
            curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
            
            # 添加稳定版仓库
            echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
            
            # 安装Docker
            sudo apt update
            sudo apt install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin
            
            # 启动Docker服务
            sudo systemctl start docker
            sudo systemctl enable docker
            
            # 添加用户到docker组
            sudo usermod -aG docker $USER
            ;;
            
        debian)
            print_info "在Debian上安装Docker..."
            # 卸载旧版本
            sudo apt remove -y docker docker-engine docker.io containerd runc 2>/dev/null || true
            
            # 添加Docker官方GPG密钥
            curl -fsSL https://download.docker.com/linux/debian/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
            
            # 添加稳定版仓库
            echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/debian $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
            
            # 安装Docker
            sudo apt update
            sudo apt install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin
            
            # 启动Docker服务
            sudo systemctl start docker
            sudo systemctl enable docker
            
            # 添加用户到docker组
            sudo usermod -aG docker $USER
            ;;
            
        centos|rhel)
            print_info "在CentOS/RHEL上安装Docker..."
            # 安装必要工具
            sudo yum install -y yum-utils
            
            # 添加Docker仓库
            sudo yum-config-manager --add-repo https://download.docker.com/linux/centos/docker-ce.repo
            
            # 安装Docker
            sudo yum install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin
            
            # 启动Docker服务
            sudo systemctl start docker
            sudo systemctl enable docker
            
            # 添加用户到docker组
            sudo usermod -aG docker $USER
            ;;
            
        fedora)
            print_info "在Fedora上安装Docker..."
            # 安装必要工具
            sudo dnf install -y yum-utils
            
            # 添加Docker仓库
            sudo dnf config-manager --add-repo https://download.docker.com/linux/fedora/docker-ce.repo
            
            # 安装Docker
            sudo dnf install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin
            
            # 启动Docker服务
            sudo systemctl start docker
            sudo systemctl enable docker
            
            # 添加用户到docker组
            sudo usermod -aG docker $USER
            ;;
            
        *)
            print_info "使用脚本安装Docker（通用方法）..."
            # 使用官方安装脚本
            curl -fsSL https://get.docker.com -o get-docker.sh
            sudo sh get-docker.sh
            
            # 启动Docker服务
            sudo systemctl start docker
            sudo systemctl enable docker
            
            # 添加用户到docker组
            sudo usermod -aG docker $USER
            
            # 清理安装脚本
            rm get-docker.sh
            ;;
    esac
    
    print_success "Docker安装完成"
    
    # 验证Docker安装
    if sudo docker run hello-world > /dev/null 2>&1; then
        print_success "Docker安装验证成功"
    else
        print_warning "Docker安装验证失败，可能需要重启系统"
        print_info "请尝试注销并重新登录，或运行: newgrp docker"
    fi
}

# 安装Docker Compose
install_docker_compose() {
    print_header "安装Docker Compose"
    
    # 检查是否已安装最新版本的Docker Compose
    if docker compose version > /dev/null 2>&1; then
        print_success "Docker Compose已安装（Docker插件形式）"
        docker compose version
        return 0
    fi
    
    # 下载最新版本的Docker Compose
    print_info "下载Docker Compose..."
    
    COMPOSE_VERSION=$(curl -s https://api.github.com/repos/docker/compose/releases/latest | jq -r '.tag_name')
    COMPOSE_URL="https://github.com/docker/compose/releases/download/${COMPOSE_VERSION}/docker-compose-$(uname -s)-$(uname -m)"
    
    sudo curl -L "$COMPOSE_URL" -o /usr/local/bin/docker-compose
    sudo chmod +x /usr/local/bin/docker-compose
    
    print_success "Docker Compose安装完成"
    
    # 验证安装
    docker-compose --version
}

# 安装GUI支持
install_gui_support() {
    print_header "安装GUI支持"
    
    case "$PACKAGE_MANAGER" in
        apt)
            print_info "安装X11相关包..."
            sudo apt install -y \
                xorg \
                x11-apps \
                xauth \
                x11-xserver-utils \
                libx11-6 \
                libxext6 \
                libxrender1 \
                libxtst6 \
                libxi6
            ;;
        yum)
            print_info "安装X11相关包..."
            sudo yum groupinstall -y "X Window System"
            sudo yum install -y \
                xorg-x11-apps \
                xorg-x11-xauth \
                xorg-x11-utils \
                libX11 \
                libXext \
                libXrender \
                libXtst \
                libXi
            ;;
        zypper)
            print_info "安装X11相关包..."
            sudo zypper install -y \
                xorg-x11-server \
                xorg-x11-apps \
                xorg-x11-auth \
                x11-tools \
                libX11-6 \
                libXext6 \
                libXrender1 \
                libXtst6 \
                libXi6
            ;;
        pacman)
            print_info "安装X11相关包..."
            sudo pacman -Sy --noconfirm \
                xorg-server \
                xorg-apps \
                xorg-xauth \
                x11-utils \
                libx11 \
                libxext \
                libxrender \
                libxtst \
                libxi
            ;;
    esac
    
    print_success "GUI支持安装完成"
}

# 配置Docker权限
configure_docker_permissions() {
    print_header "配置Docker权限"
    
    # 确保用户在docker组中
    if groups $USER | grep &>/dev/null '\bdocker\b'; then
        print_success "用户已在docker组中"
    else
        print_info "添加用户到docker组..."
        sudo usermod -aG docker $USER
        print_warning "请注销并重新登录使组权限生效，或运行: newgrp docker"
    fi
    
    # 配置Docker守护进程
    print_info "配置Docker守护进程..."
    
    # 创建Docker配置目录
    sudo mkdir -p /etc/docker
    
    # 创建daemon.json配置文件
    sudo tee /etc/docker/daemon.json > /dev/null <<EOF
{
  "log-driver": "json-file",
  "log-opts": {
    "max-size": "10m",
    "max-file": "3"
  },
  "storage-driver": "overlay2",
  "live-restore": true,
  "userland-proxy": false
}
EOF
    
    # 重启Docker服务
    sudo systemctl restart docker
    
    print_success "Docker权限配置完成"
}

# 创建用户配置脚本
create_user_scripts() {
    print_header "创建用户配置脚本"
    
    # 创建Docker启动脚本
    mkdir -p ~/bin
    
    cat > ~/bin/start-airdpro-docker.sh << 'EOF'
#!/bin/bash

# AirdPro Docker启动脚本

# 检查Docker是否运行
if ! docker info > /dev/null 2>&1; then
    echo "错误：Docker未运行或未正确配置"
    echo "请启动Docker服务："
    echo "  systemctl start docker"
    exit 1
fi

# 切换到AirdPro目录
if [ -d "AirdPro-Linux-Package" ]; then
    cd AirdPro-Linux-Package
else
    echo "错误：未找到AirdPro-Linux-Package目录"
    exit 1
fi

# 设置X11环境变量
export DISPLAY=${DISPLAY:-:0}

# 启动容器
echo "启动AirdPro容器..."
docker-compose up -d airdpro-linux

# 等待容器启动
echo "等待容器启动..."
sleep 5

# 显示容器状态
docker-compose ps

echo "AirdPro已启动！"
echo "使用以下命令查看日志："
echo "  docker-compose logs -f airdpro-linux"
echo "停止容器："
echo "  docker-compose down"
EOF
    
    chmod +x ~/bin/start-airdpro-docker.sh
    
    # 创建系统服务文件
    print_info "创建系统服务..."
    
    sudo tee /etc/systemd/system/airdpro-docker.service > /dev/null <<EOF
[Unit]
Description=AirdPro Docker Service
Requires=docker.service
After=docker.service

[Service]
Type=oneshot
RemainAfterExit=yes
User=$USER
Group=docker
WorkingDirectory=$PWD/AirdPro-Linux-Package
ExecStart=/usr/bin/docker-compose up -d airdpro-linux
ExecStop=/usr/bin/docker-compose down
TimeoutStartSec=0

[Install]
WantedBy=multi-user.target
EOF
    
    # 重新加载systemd
    sudo systemctl daemon-reload
    
    print_success "用户配置脚本创建完成"
}

# 清理临时文件
cleanup() {
    print_header "清理临时文件"
    
    # 清理包缓存
    case "$PACKAGE_MANAGER" in
        apt)
            sudo apt autoremove -y
            sudo apt autoclean
            ;;
        yum)
            sudo yum autoremove -y
            sudo yum clean all
            ;;
        zypper)
            sudo zypper clean
            ;;
        pacman)
            sudo pacman -Sc --noconfirm
            ;;
    esac
    
    print_success "临时文件清理完成"
}

# 显示安装完成信息
show_completion_info() {
    print_header "安装完成"
    
    print_success "AirdPro Linux环境安装完成！"
    echo
    echo "下一步操作："
    echo "1. 运行验证脚本："
    echo "   ./validate-package.sh"
    echo
    echo "2. 启动AirdPro："
    echo "   ./Build_Run_Scripts/run-docker-linux.sh --build"
    echo "   或使用快捷脚本："
    echo "   ~/bin/start-airdpro-docker.sh"
    echo
    echo "3. 查看运行日志："
    echo "   docker-compose logs -f airdpro-linux"
    echo
    echo "4. 停止服务："
    echo "   docker-compose down"
    echo
    echo "5. 如果需要系统服务支持（可选）："
    echo "   sudo systemctl enable airdpro-docker"
    echo "   sudo systemctl start airdpro-docker"
    echo
    print_warning "重要提醒："
    echo "- 如果Docker权限有问题，请注销并重新登录"
    echo "- 或者运行: newgrp docker"
    echo "- 确保X11显示设置正确：echo \$DISPLAY"
    echo
    print_info "查看详细文档请阅读 README.md 文件"
}

# 显示使用说明
show_usage() {
    echo "AirdPro Linux 自动安装脚本"
    echo
    echo "用法: $0 [选项]"
    echo
    echo "选项:"
    echo "  --minimal     最小化安装（仅Docker，不安装GUI支持）"
    echo "  --gui-only    仅安装GUI支持"
    echo "  --help        显示此帮助信息"
    echo
    echo "示例:"
    echo "  $0           # 完整安装"
    echo "  $0 --minimal # 最小化安装"
}

# 主函数
main() {
    # 解析命令行参数
    INSTALL_MODE="full"
    while [[ $# -gt 0 ]]; do
        case $1 in
            --minimal)
                INSTALL_MODE="minimal"
                shift
                ;;
            --gui-only)
                INSTALL_MODE="gui-only"
                shift
                ;;
            --help)
                show_usage
                exit 0
                ;;
            *)
                print_error "未知选项: $1"
                show_usage
                exit 1
                ;;
        esac
    done
    
    # 检查是否在包目录中
    if [ ! -f "install-linux.sh" ]; then
        print_error "请在AirdPro-Linux-Package目录中运行此脚本"
        exit 1
    fi
    
    print_header "AirdPro Linux 自动安装脚本 v1.0"
    echo "检测到的安装模式: $INSTALL_MODE"
    echo
    
    # 执行安装步骤
    case "$INSTALL_MODE" in
        full)
            detect_linux_distribution
            check_system_requirements
            update_system_packages
            install_basic_dependencies
            install_docker
            install_docker_compose
            install_gui_support
            configure_docker_permissions
            create_user_scripts
            cleanup
            show_completion_info
            ;;
        minimal)
            detect_linux_distribution
            check_system_requirements
            update_system_packages
            install_basic_dependencies
            install_docker
            install_docker_compose
            configure_docker_permissions
            create_user_scripts
            cleanup
            show_completion_info
            ;;
        gui-only)
            install_gui_support
            show_completion_info
            ;;
    esac
}

# 运行主函数
main "$@"
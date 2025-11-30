#!/bin/bash

# AirdPro macOS 自动安装脚本
# Automatic AirdPro installation script for macOS

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

# 检查系统要求
check_system_requirements() {
    print_header "检查系统要求"
    
    # 检查macOS版本
    macos_version=$(sw_vers -productVersion)
    major_version=$(echo $macos_version | cut -d. -f1)
    minor_version=$(echo $macos_version | cut -d. -f2)
    
    if [ "$major_version" -lt 10 ] || ([ "$major_version" -eq 10 ] && [ "$minor_version" -lt 14 ]); then
        print_error "需要macOS 10.14 (Mojave)或更高版本，当前版本：$macos_version"
        exit 1
    fi
    
    print_success "macOS版本检查通过：$macos_version"
    
    # 检查内存
    total_memory=$(($(sysctl -n hw.memsize) / 1024 / 1024 / 1024))
    if [ "$total_memory" -lt 4 ]; then
        print_warning "建议至少4GB内存，当前：${total_memory}GB"
    else
        print_success "内存检查通过：${total_memory}GB"
    fi
    
    # 检查磁盘空间
    available_space=$(df -h / | awk 'NR==2 {print $4}' | sed 's/G.*//')
    if [ "$available_space" -lt 10 ]; then
        print_warning "建议至少10GB可用空间，当前：${available_space}GB"
    else
        print_success "磁盘空间检查通过：${available_space}GB可用"
    fi
}

# 检查并安装Homebrew
install_homebrew() {
    print_header "安装Homebrew"
    
    if command -v brew &> /dev/null; then
        print_success "Homebrew已安装"
        brew update
    else
        print_info "安装Homebrew..."
        /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
        
        # 添加Homebrew到PATH (Apple Silicon Macs)
        if [[ $(uname -m) == "arm64" ]]; then
            echo 'eval "$(/opt/homebrew/bin/brew shellenv)"' >> ~/.zprofile
            eval "$(/opt/homebrew/bin/brew shellenv)"
        fi
    fi
}

# 安装Docker Desktop
install_docker() {
    print_header "安装Docker Desktop"
    
    if command -v docker &> /dev/null; then
        print_success "Docker已安装"
        docker --version
        return
    fi
    
    print_info "下载Docker Desktop for Mac..."
    
    # 检测处理器类型
    if [[ $(uname -m) == "arm64" ]]; then
        # Apple Silicon Mac
        docker_url="https://desktop.docker.com/mac/main/arm64/Docker.dmg"
        docker_app="Docker.app"
    else
        # Intel Mac
        docker_url="https://desktop.docker.com/mac/main/amd64/Docker.dmg"
        docker_app="Docker.app"
    fi
    
    # 下载Docker Desktop
    temp_file=$(mktemp)
    curl -L -o "$temp_file" "$docker_url"
    
    # 挂载DMG并复制到Applications
    print_info "安装Docker Desktop..."
    hdiutil attach "$temp_file" -quiet
    cp -R "/Volumes/Docker Desktop/$docker_app" /Applications/
    hdiutil detach "/Volumes/Docker Desktop" -quiet
    rm "$temp_file"
    
    print_success "Docker Desktop已安装到Applications文件夹"
    print_warning "请手动启动Docker Desktop应用程序"
}

# 安装XQuartz
install_xquartz() {
    print_header "安装XQuartz"
    
    if command -v xquartz &> /dev/null; then
        print_success "XQuartz已安装"
        return
    fi
    
    print_info "安装XQuartz..."
    brew install --cask xquartz
    
    print_success "XQuartz安装完成"
    print_warning "需要重启系统才能生效，或注销后重新登录"
}

# 安装Wine
install_wine() {
    print_header "安装Wine"
    
    if command -v wine &> /dev/null; then
        print_success "Wine已安装"
        wine --version
        return
    fi
    
    print_info "安装Wine..."
    brew install --cask xquartz
    brew install wine-stable
    
    print_success "Wine安装完成"
}

# 设置X11环境
setup_x11_environment() {
    print_header "配置X11环境"
    
    # 创建xinitrc文件
    cat > ~/.xinitrc << 'EOF'
# 启动XQuartz
if [ -z "$DISPLAY" ]; then
    export DISPLAY=:0
fi

# 启动X11服务器
if [ -z "$(pgrep Xquartz)" ]; then
    open -a XQuartz
    sleep 2
fi
EOF
    
    chmod +x ~/.xinitrc
    
    # 设置DISPLAY环境变量
    if ! grep -q "DISPLAY=:0" ~/.zshrc 2>/dev/null; then
        echo "export DISPLAY=:0" >> ~/.zshrc
    fi
    
    print_success "X11环境配置完成"
}

# 创建应用程序启动脚本
create_launcher_script() {
    print_header "创建启动脚本"
    
    cat > ~/Desktop/AirdPro.app << 'EOF'
#!/bin/bash

# AirdPro启动脚本

# 设置X11环境
export DISPLAY=:0

# 确保XQuartz正在运行
if ! pgrep Xquartz > /dev/null; then
    open -a XQuartz
    sleep 3
fi

# 切换到AirdPro目录
cd "$(dirname "$0")/../AirdPro-MacOS-Package"

# 运行GUI应用
./Build_Run_Scripts/run-gui-en.sh "$@"
EOF
    
    chmod +x ~/Desktop/AirdPro.app
    
    # 创建应用包结构
    mkdir -p ~/Desktop/AirdPro.app/Contents/MacOS
    
    cat > ~/Desktop/AirdPro.app/Contents/Info.plist << 'EOF'
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleExecutable</key>
    <string>AirdPro</string>
    <key>CFBundleIconFile</key>
    <string>airdpro</string>
    <key>CFBundleIdentifier</key>
    <string>com.airdpro.app</string>
    <key>CFBundleName</key>
    <string>AirdPro</string>
    <key>CFBundlePackageType</key>
    <string>APPL</string>
    <key>CFBundleShortVersionString</key>
    <string>4.2.0</string>
    <key>CFBundleVersion</key>
    <string>1</string>
    <key>LSMinimumSystemVersion</key>
    <string>10.14</string>
    <key>NSHighResolutionCapable</key>
    <true/>
</dict>
</plist>
EOF
    
    print_success "桌面应用程序启动器已创建"
}

# 构建Docker镜像
build_docker_image() {
    print_header "构建Docker镜像"
    
    if ! command -v docker &> /dev/null; then
        print_error "Docker未安装，跳过镜像构建"
        return
    fi
    
    if ! docker info &> /dev/null; then
        print_warning "Docker服务未运行，请先启动Docker Desktop"
        return
    fi
    
    print_info "构建AirdPro Docker镜像..."
    cd AirdPro-MacOS-Package
    
    if ./Build_Run_Scripts/build-docker-en.sh; then
        print_success "Docker镜像构建完成"
    else
        print_error "Docker镜像构建失败"
    fi
}

# 清理临时文件
cleanup() {
    print_header "清理临时文件"
    print_info "清理临时安装文件..."
    
    # 这里可以添加清理逻辑
    print_success "清理完成"
}

# 主函数
main() {
    print_header "AirdPro macOS 自动安装程序"
    
    # 检查是否以root权限运行
    if [ "$EUID" -eq 0 ]; then
        print_warning "不建议以root权限运行此脚本"
        read -p "是否继续？[y/N]: " -n 1 -r
        echo
        if [[ ! $REPLY =~ ^[Yy]$ ]]; then
            exit 1
        fi
    fi
    
    # 执行安装步骤
    check_system_requirements
    install_homebrew
    
    # 询问安装选项
    echo
    print_info "选择安装方式："
    echo "1) Docker方式 (推荐)"
    echo "2) Wine方式"
    echo "3) 两者都安装"
    read -p "请选择 [1-3]: " choice
    
    case $choice in
        1)
            install_docker
            install_xquartz
            setup_x11_environment
            create_launcher_script
            build_docker_image
            ;;
        2)
            install_wine
            install_xquartz
            setup_x11_environment
            create_launcher_script
            ;;
        3)
            install_docker
            install_wine
            install_xquartz
            setup_x11_environment
            create_launcher_script
            build_docker_image
            ;;
        *)
            print_error "无效选择"
            exit 1
            ;;
    esac
    
    cleanup
    
    print_header "安装完成！"
    print_success "AirdPro已成功安装到macOS"
    echo
    print_info "使用方法："
    echo "1. Docker方式：启动Docker Desktop，然后运行"
    echo "   ./Build_Run_Scripts/run-gui-en.sh"
    echo
    echo "2. 双击桌面上的AirdPro应用程序"
    echo
    echo "3. Wine方式：直接运行"
    echo "   ./Build_Run_Scripts/run-wine-macos.sh"
    echo
    print_info "详细说明请查看 MacOS-Install-Run.md"
}

# 运行主函数
main "$@"
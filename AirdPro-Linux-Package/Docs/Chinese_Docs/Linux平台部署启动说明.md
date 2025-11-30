# AirdPro Linux分发包部署启动说明

## 概述

AirdPro是一款强大的跨平台数据传输工具，本文档将指导您在Linux系统上安装、配置和运行AirdPro。

## 系统要求

### 最低要求
- **操作系统**: Ubuntu 18.04+、Debian 10+、CentOS 7+、Fedora 30+ 或其他主流Linux发行版
- **处理器**: x86_64 (64位)
- **内存**: 2GB RAM（推荐4GB或更多）
- **磁盘空间**: 5GB可用空间
- **网络**: 互联网连接（用于依赖下载）

### 推荐配置
- **内存**: 8GB RAM或更多
- **磁盘空间**: 10GB可用空间
- **显卡**: 支持硬件加速（可选）
- **显示**: X11或Wayland显示服务器

## 快速开始

### 方式一：自动安装脚本（推荐）

1. **下载并解压分发包**
   ```bash
   # 下载AirdPro-Linux-Package-v4.2.0.zip（从发布页面）
   unzip AirdPro-Linux-Package-v4.2.0.zip
   cd AirdPro-Linux-Package
   ```

2. **运行自动安装脚本**
   ```bash
   # 给脚本执行权限
   chmod +x install-linux.sh
   
   # 运行安装脚本（选择full模式）
   ./install-linux.sh --mode full
   ```

3. **选择运行方式**
   
   **Docker模式（推荐）:**
   ```bash
   chmod +x Build_Run_Scripts/run-docker-linux.sh
   ./Build_Run_Scripts/run-docker-linux.sh --build
   ```
   
   **Wine模式（传统方式）:**
   ```bash
   chmod +x Build_Run_Scripts/run-wine-linux.sh
   ./Build_Run_Scripts/run-wine-linux.sh
   ```

### 方式二：手动安装

1. **安装系统依赖**
   ```bash
   # Ubuntu/Debian
   sudo apt update
   sudo apt install -y curl wget unzip tar
   sudo apt install -y wine64 wine32   # Wine模式需要
   sudo apt install -y docker.io docker-compose   # Docker模式需要
   
   # Fedora/CentOS
   sudo dnf install -y curl wget unzip tar
   sudo dnf install -y wine   # Wine模式需要
   sudo dnf install -y docker docker-compose   # Docker模式需要
   
   # Arch Linux
   sudo pacman -S curl wget unzip tar
   sudo pacman -S wine   # Wine模式需要
   sudo pacman -S docker docker-compose   # Docker模式需要
   ```

2. **配置Docker服务（如果使用Docker模式）**
   ```bash
   # 启动Docker服务
   sudo systemctl start docker
   sudo systemctl enable docker
   
   # 添加用户到docker组
   sudo usermod -aG docker $USER
   
   # 重新登录或执行
   newgrp docker
   ```

3. **配置X11权限（如果使用图形界面）**
   ```bash
   # 设置X11权限
   xhost +local:docker
   ```

## Docker模式运行

### 基本运行
```bash
# 构建并运行Docker容器
./Build_Run_Scripts/run-docker-linux.sh --build

# 仅运行现有容器
./Build_Run_Scripts/run-docker-linux.sh

# 交互模式（连接到容器内部）
./Build_Run_Scripts/run-docker-linux.sh --interactive
```

### 高级选项
```bash
# 调试模式
./Build_Run_Scripts/run-docker-linux.sh --debug

# 高配置运行（8GB内存，4核CPU）
./Build_Run_Scripts/run-docker-linux.sh --memory 8g --cpus 4

# 自定义数据目录
./Build_Run_Scripts/run-docker-linux.sh --data-dir /path/to/data

# 查看日志
./Build_Run_Scripts/run-docker-linux.sh logs

# 停止容器
./Build_Run_Scripts/run-docker-linux.sh stop

# 清理资源
./Build_Run_Scripts/run-docker-linux.sh cleanup
```

### Docker Compose配置

项目包含`docker-compose.yml`文件，支持以下服务：

- **airdpro-linux**: 主要服务
- **数据持久化**: 自动挂载`./data`和`./logs`目录
- **X11支持**: 自动配置显示转发

自定义配置示例：
```yaml
version: '3.8'
services:
  airdpro-linux:
    build: .
    environment:
      - DISPLAY=${DISPLAY}
    volumes:
      - ./data:/app/data
      - ./logs:/app/logs
      - /tmp/.X11-unix:/tmp/.X11-unix:rw
    deploy:
      resources:
        limits:
          memory: 8G
          cpus: '4'
```

## Wine模式运行

### 基本运行
```bash
# 标准启动
./Build_Run_Scripts/run-wine-linux.sh

# 强制X11模式
./Build_Run_Scripts/run-wine-linux.sh --gui-mode x11

# 交互模式
./Build_Run_Scripts/run-wine-linux.sh --interactive

# 32位模式
./Build_Run_Scripts/run-wine-linux.sh --wine-arch win32
```

### 高级选项
```bash
# 自定义Wine前缀
./Build_Run_Scripts/run-wine-linux.sh --wine-prefix /path/to/prefix

# 调试模式
./Build_Run_Scripts/run-wine-linux.sh --debug

# 自定义应用目录
./Build_Run_Scripts/run-wine-linux.sh --app-dir /path/to/airdpro

# 无头模式（服务器环境）
./Build_Run_Scripts/run-wine-linux.sh --gui-mode headless

# 查看Wine信息
./Build_Run_Scripts/run-wine-linux.sh info

# 重置Wine环境
./Build_Run_Scripts/run-wine-linux.sh reset

# 清理Wine环境
./Build_Run_Scripts/run-wine-linux.sh cleanup
```

### Wine环境配置

脚本会自动配置以下组件：

- **.NET Framework 4.8**: 支持.NET应用程序
- **Visual C++ Redistributable**: 运行时支持
- **MSXML 6**: XML解析支持
- **Core Fonts**: 字体支持

## 故障排除

### 常见问题

#### 1. Docker模式问题

**问题**: Docker服务未运行
```bash
# 检查Docker状态
sudo systemctl status docker

# 启动Docker服务
sudo systemctl start docker
```

**问题**: 权限不足
```bash
# 添加用户到docker组
sudo usermod -aG docker $USER
newgrp docker
```

**问题**: GUI无法显示
```bash
# 检查X11权限
xhost +local:docker

# 检查DISPLAY环境变量
echo $DISPLAY

# 如果在远程服务器，可能需要X11转发
ssh -X user@server
```

#### 2. Wine模式问题

**问题**: Wine未安装或版本不兼容
```bash
# 检查Wine版本
wine --version

# Ubuntu/Debian安装最新Wine
sudo apt install wine
```

**问题**: 32位应用程序无法运行
```bash
# 安装32位支持
sudo apt install wine32

# 使用32位模式
./Build_Run_Scripts/run-wine-linux.sh --wine-arch win32
```

**问题**: GUI显示问题
```bash
# 检查X11显示
echo $DISPLAY
xset q

# 强制X11模式
./Build_Run_Scripts/run-wine-linux.sh --gui-mode x11
```

#### 3. 通用问题

**问题**: 内存不足
```bash
# 检查可用内存
free -h

# Docker模式减少内存限制
./Build_Run_Scripts/run-docker-linux.sh --memory 2g
```

**问题**: 磁盘空间不足
```bash
# 检查磁盘使用
df -h

# 清理Docker资源
docker system prune -f
```

### 日志文件

- **Docker模式**: `./logs/airdpro.log`
- **Wine模式**: `./logs/wine-*.log`

查看最新日志：
```bash
tail -f logs/airdpro.log
```

## 性能优化

### Docker模式优化
```bash
# 启用GPU支持（如果可用）
./Build_Run_Scripts/run-docker-linux.sh --gpu

# 使用自定义镜像仓库
export DOCKER_REGISTRY=your-registry.com
./Build_Run_Scripts/run-docker-linux.sh --build
```

### Wine模式优化
```bash
# 优化Wine设置
./Build_Run_Scripts/run-wine-linux.sh --wine-options '--sync command'

# 使用DXVK（更好的DirectX支持）
./Build_Run_Scripts/run-wine-linux.sh --wine-options 'PROTON_USE_WINED3D=1'
```

## 安全注意事项

1. **网络安全**: 在防火墙中限制不必要的端口
2. **数据安全**: 定期备份数据目录
3. **权限管理**: 不要以root用户运行应用程序
4. **更新维护**: 定期更新系统和依赖包

## 技术支持

如果遇到问题，请：

1. 查看错误日志文件
2. 检查系统要求是否满足
3. 尝试重新安装或重置环境
4. 提交Issue到项目仓库

## 版本信息

- **文档版本**: v1.0
- **支持版本**: AirdPro v4.2.0+
- **最后更新**: 2025年11月30日

## 许可证

请遵循AirdPro项目的许可条款。
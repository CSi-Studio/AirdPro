# AirdPro 跨平台 Docker 部署指南

## 概述

AirdPro是一个基于.NET Framework 4.8的C#科学数据处理应用。本指南提供了使用Docker技术将AirdPro打包为跨平台可执行软件的完整解决方案，支持Windows、Linux和macOS平台。

## 系统要求

### Windows 平台
- Windows 10/11 (64位)
- Docker Desktop for Windows
- 至少8GB内存
- 至少10GB可用磁盘空间

### Linux 平台
- Ubuntu 18.04+, CentOS 7+, 或其他现代Linux发行版
- Docker Engine 20.10+
- X11显示服务器 (用于GUI模式)
- 至少8GB内存
- 至少10GB可用磁盘空间

### macOS 平台
- macOS 10.14+ (64位)
- Docker Desktop for Mac
- XQuartz (用于GUI模式)
- 至少8GB内存
- 至少10GB可用磁盘空间

## 安装步骤

### 1. 准备Docker环境

#### Windows
1. 下载并安装 [Docker Desktop for Windows](https://docs.docker.com/desktop/windows/install/)
2. 启动Docker Desktop
3. 确保WSL 2功能已启用

#### Linux
```bash
# Ubuntu/Debian
sudo apt-get update
sudo apt-get install docker.io

# CentOS/RHEL
sudo yum install docker
sudo systemctl start docker
sudo systemctl enable docker

# 添加用户到docker组
sudo usermod -aG docker $USER
```

#### macOS
1. 下载并安装 [Docker Desktop for Mac](https://docs.docker.com/desktop/mac/install/)
2. 启动Docker Desktop
3. 安装XQuartz: `brew install --cask xquartz`

### 2. 获取AirdPro项目

确保项目文件位于 `d:\workspace\AirdPro` 目录中。

### 3. 构建Docker镜像

#### Windows环境
```powershell
# 在PowerShell中执行
cd d:\workspace\AirdPro
.\build-docker-en.bat
```

#### Linux/macOS环境
```bash
# 在终端中执行
cd /path/to/AirdPro
chmod +x build-docker-en.sh
./build-docker-en.sh
```

### 4. 可选：构建特定平台镜像

#### 仅构建Windows版本
```bash
./build-docker-en.sh --windows-only
```

#### 仅构建Linux/macOS版本
```bash
./build-docker-en.sh --linux-only
```

#### 清理旧镜像
```bash
./build-docker-en.sh --clean
```

## 使用方法

### 1. GUI模式运行

#### Windows环境
```powershell
# GUI模式
docker run -it --rm `
  -v "${PWD}\data":C:\data `
  -v "${PWD}\logs":C:\logs `
  airdpro:windows
```

#### Linux环境
```bash
# GUI模式
./run-gui-en.sh

# 指定Linux环境运行
./run-gui-en.sh --linux-only

# 跳过X11检查
./run-gui-en.sh --no-x11-check
```

#### macOS环境
```bash
# GUI模式
./run-gui-en.sh

# 指定macOS环境运行
./run-gui-en.sh --macos-only
```

### 2. 命令行模式运行

#### Windows环境
```powershell
# CLI模式
.\run-cli-en.bat

# 控制台模式
.\run-cli-en.bat --console
```

#### Linux/macOS环境
```bash
# CLI模式
./run-cli-en.sh

# 显示详细信息
./run-cli-en.sh --console

# 调试模式
./run-cli-en.sh --verbose

# 指定平台运行
./run-cli-en.sh --linux-only --console
```

### 3. 使用Docker Compose

#### 运行GUI版本
```bash
# Linux/macOS
docker-compose up airdpro-linux

# Windows
docker-compose up airdpro-windows
```

#### 运行CLI版本
```bash
# 所有平台
docker-compose up airdpro-cli
```

#### 运行开发版本
```bash
# 开发环境
docker-compose up airdpro-dev
```

## 目录结构

项目构建后会产生以下目录结构：

```
AirdPro/
├── Dockerfile              # Docker镜像构建文件
├── docker-compose.yml      # Docker Compose配置
├── build-docker-en.bat     # Windows构建脚本
├── build-docker-en.sh      # Linux/macOS构建脚本
├── run-gui-en.sh          # GUI运行脚本
├── run-cli-en.sh          # CLI运行脚本
├── data/                  # 数据目录
├── logs/                  # 日志目录
└── AirdPro.sln           # Visual Studio解决方案文件
```

## 常见问题解决

### 1. 镜像构建失败

**问题**: Docker镜像构建过程中出现错误

**解决方案**:
1. 检查Docker服务是否正常运行
2. 确保网络连接正常
3. 清理Docker缓存: `docker system prune -a`
4. 重新构建镜像: `./build-docker-en.sh --clean`

### 2. GUI无法显示

**问题**: 在Linux/macOS上GUI应用无法显示窗口

**解决方案**:

#### macOS
1. 确认XQuartz已安装: `brew install --cask xquartz`
2. 启动XQuartz: `open -a XQuartz`
3. 在XQuartz中启用"Allow connections from network clients"
4. 重启终端

#### Linux
1. 检查DISPLAY环境变量: `echo $DISPLAY`
2. 如果为空，设置DISPLAY: `export DISPLAY=:0`
3. 安装X11相关包: `sudo apt-get install xorg`
4. 允许X11连接: `xhost +local:docker`

### 3. 权限问题

**问题**: Linux/macOS上脚本权限不足

**解决方案**:
```bash
# 添加执行权限
chmod +x build-docker-en.sh
chmod +x run-gui-en.sh
chmod +x run-cli-en.sh
```

### 4. Docker内存不足

**问题**: 构建过程中Docker报告内存不足

**解决方案**:
1. 增加Docker Desktop内存限制到至少6GB
2. 关闭其他占用内存的程序
3. 使用磁盘交换: `sudo swapon --show`

### 5. 网络问题

**问题**: 构建过程中网络下载失败

**解决方案**:
1. 检查网络连接
2. 使用国内镜像源 (中国用户):
   ```dockerfile
   # 在Dockerfile开头添加
   RUN sed -i 's/deb.debian.org/mirrors.aliyun.com/g' /etc/apt/sources.list && \
       sed -i 's/security.debian.org/mirrors.aliyun.com/g' /etc/apt/sources.list
   ```

## 性能优化

### 1. 减少构建时间
- 使用`.dockerignore`文件排除不必要的文件
- 启用Docker BuildKit: `export DOCKER_BUILDKIT=1`
- 使用多阶段构建优化镜像大小

### 2. 提高运行时性能
- 分配足够的CPU和内存资源给Docker Desktop
- 使用volume挂载而非COPY复制文件
- 启用Docker Desktop的GPU支持 (如果有)

### 3. 监控资源使用
```bash
# 监控容器资源使用
docker stats

# 检查镜像大小
docker images | grep airdpro
```

## 安全注意事项

1. **容器隔离**: 使用`--rm`参数确保容器退出后自动清理
2. **最小权限**: 仅挂载必要的目录
3. **网络安全**: 避免在生产环境中暴露Docker守护进程
4. **镜像更新**: 定期更新基础镜像以获取安全补丁

## 故障排除日志

### 查看构建日志
```bash
# 详细构建过程
docker build --progress=plain --no-cache -t airdpro:linux .

# 查看特定阶段构建日志
docker build --target runtime-linux --progress=plain --no-cache -t airdpro:linux .
```

### 运行时调试
```bash
# 进入容器调试
docker run -it --rm airdpro:linux /bin/bash

# 查看容器输出
docker logs <container_id>

# 实时查看日志
docker logs -f <container_id>
```

## 技术支持

如果遇到问题，请提供以下信息：

1. 操作系统版本
2. Docker版本: `docker --version`
3. 完整错误日志
4. 重现问题的步骤

## 更新日志

### v1.0.0 (当前版本)
- 支持Windows原生容器运行
- 支持Linux/macOS Wine容器运行
- 完整的跨平台构建脚本
- GUI和CLI双模式支持
- Docker Compose集成

---

**注意**: 首次运行可能需要较长时间来初始化Wine环境、.NET Framework以及相关依赖。这是正常现象，请耐心等待。
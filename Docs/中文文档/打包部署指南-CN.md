# AirdPro Docker打包部署指南

## 概述

本指南提供了使用Docker在多个平台上打包和部署AirdPro的完整说明。AirdPro是一个质谱数据分析工具，通过容器化部署支持Windows、Linux和macOS平台。

## 目录

1. [系统要求](#系统要求)
2. [项目结构](#项目结构)
3. [Docker构建流程](#docker构建流程)
4. [平台特定说明](#平台特定说明)
5. [镜像变体](#镜像变体)
6. [运行说明](#运行说明)
7. [配置和数据管理](#配置和数据管理)
8. [故障排除](#故障排除)
9. [性能优化](#性能优化)
10. [安全注意事项](#安全注意事项)

## 系统要求

### 硬件要求

- **内存**: 最少8GB内存（推荐16GB）
- **存储**: 至少20GB可用磁盘空间
- **CPU**: 多核处理器（推荐4核以上）
- **平台**: Windows 10/11、macOS 10.15+ 或 Linux发行版

### 软件依赖

- **Docker**: 版本20.10或更高
- **Docker Compose**: 版本2.0或更高（可选）
- **平台特定要求**:
  - **Windows**: Docker Desktop for Windows
  - **macOS**: Docker Desktop for Mac
  - **Linux**: Docker Engine或Docker Desktop

### 网络要求

- 首次镜像下载需要互联网连接
- 处理大型数据集时需要稳定带宽

## 项目结构

```
AirdPro/
├── Dockerfile                 # 多阶段Docker构建文件
├── Dockerfile.crossplatform   # 跨平台优化版本
├── docker-compose.yml         # 多服务编排文件
├── build-docker-en.bat        # Windows构建脚本
├── build-docker-en.sh         # Linux/macOS构建脚本
├── run-gui-en.sh             # GUI运行时脚本
├── run-cli-en.sh             # CLI运行时脚本
├── README_CROSSPLATFORM.md   # 跨平台部署指南
├── AirdPro.sln               # 解决方案文件
├── AirdPro/                  # 主应用程序项目
├── CSharpSDK/                # .NET SDK组件
├── ImportLibs/               # 第三方库
└── data/                     # 数据目录
└── logs/                     # 日志目录
```

## Docker构建流程

### 快速开始构建

#### Windows环境
```batch
# 导航到项目目录
cd D:\workspace\AirdPro

# 构建所有变体
build-docker-en.bat
```

#### Linux/macOS环境
```bash
# 导航到项目目录
cd /path/to/AirdPro

# 构建所有变体
./build-docker-en.sh
```

### 手动构建流程

#### 1. 构建Windows原生版本
```bash
docker build --target runtime-windows -t airdpro:windows .
```

#### 2. 构建Linux/macOS Wine版本
```bash
docker build --target runtime-linux -t airdpro:linux .
```

#### 3. 构建CLI版本
```bash
docker build --target runtime-cli -t airdpro:cli .
```

#### 4. 构建开发版本
```bash
docker build --target runtime-dev -t airdpro:dev .
```

### 使用Docker Compose构建

```bash
# 构建所有服务
docker-compose build

# 构建特定服务
docker-compose build airdpro-windows
docker-compose build airdpro-linux
```

## 平台特定说明

### Windows部署

#### 前置条件
1. 安装Docker Desktop for Windows
2. 启用WSL 2后端
3. 切换到Linux容器模式

#### 构建命令
```batch
# 使用构建脚本
build-docker-en.bat

# 手动构建
docker build --target runtime-windows -t airdpro:windows .
```

#### 运行命令
```batch
# Windows原生容器
docker run -it --rm --name airdpro-windows ^
    -v "%cd%\data:/data" ^
    -v "%cd%\logs:/logs" ^
    --platform windows/amd64 ^
    airdpro:windows
```

### macOS部署

#### 前置条件
1. 安装Docker Desktop for Mac
2. 安装XQuartz以支持GUI: `brew install --cask xquartz`
3. 启动XQuartz应用程序

#### 构建命令
```bash
# 使用构建脚本
./build-docker-en.sh

# 手动构建
docker build --target runtime-linux -t airdpro:macos .
```

#### 运行命令
```bash
# GUI版本
./run-gui-en.sh --macos-only

# CLI版本
./run-cli-en.sh --macos-only
```

#### GUI设置
1. 安装XQuartz: `brew install --cask xquartz`
2. 启动XQuartz: `open -a XQuartz`
3. 在XQuartz偏好设置中启用"允许来自网络客户端的连接"
4. 重启XQuartz

### Linux部署

#### 前置条件
1. 安装Docker Engine: `curl -fsSL https://get.docker.com | sh`
2. 为了支持GUI，安装X11工具: `sudo apt install xvfb`

#### 构建命令
```bash
# 使用构建脚本
./build-docker-en.sh --linux-only

# 手动构建
docker build --target runtime-linux -t airdpro:linux .
```

#### 运行命令
```bash
# GUI版本
./run-gui-en.sh --linux-only

# CLI版本
./run-cli-en.sh --linux-only
```

## 镜像变体

### 1. Windows原生版本 (`airdpro:windows`)
- **基础镜像**: Windows Server Core LTSC 2022
- **运行时**: .NET Framework 4.8
- **目标平台**: Windows（原生性能）
- **使用场景**: Windows环境，最佳性能
- **大小**: 约4-5GB

### 2. Linux/macOS Wine版本 (`airdpro:linux`, `airdpro:macos`)
- **基础镜像**: Ubuntu 22.04
- **运行时**: Wine + .NET Framework 4.8
- **目标平台**: Linux/macOS（跨平台兼容性）
- **使用场景**: 非Windows环境
- **大小**: 约8-10GB（包括Wine依赖）

### 3. CLI版本 (`airdpro:cli`)
- **基础**: Ubuntu 22.04 + Wine（最小依赖）
- **特性**: 仅命令行，无GUI
- **使用场景**: 自动化处理、批处理作业
- **大小**: 约6-7GB

### 4. 开发版本 (`airdpro:dev`)
- **基础**: Linux运行时 + 开发工具
- **特性**: 完整开发环境
- **使用场景**: 开发和调试
- **大小**: 约9-11GB

## 运行说明

### GUI模式

#### 自动化脚本
```bash
# 跨平台GUI脚本
./run-gui-en.sh

# 平台特定选项
./run-gui-en.sh --linux-only     # 仅Linux
./run-gui-en.sh --macos-only     # 仅macOS
./run-gui-en.sh --windows-only   # 仅Windows
```

#### 手动命令
```bash
# Linux/macOS GUI
docker run -it --rm \
    --name airdpro-gui \
    -e DISPLAY=${DISPLAY} \
    -v /tmp/.X11-unix:/tmp/.X11-unix \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    airdpro:linux
```

### CLI模式

#### 自动化脚本
```bash
# 跨平台CLI脚本
./run-cli-en.sh

# 平台特定选项
./run-cli-en.sh --linux-only     # 仅Linux
./run-cli-en.sh --macos-only     # 仅macOS
./run-cli-en.sh --windows-only   # 仅Windows
```

#### 手动命令
```bash
# CLI执行
docker run -it --rm \
    --name airdpro-cli \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    airdpro:cli

# 带参数
docker run -it --rm \
    --name airdpro-cli \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    airdpro:cli --help
```

### Docker Compose模式

```bash
# 启动所有服务
docker-compose up -d

# 启动特定服务
docker-compose up airdpro-linux -d

# 查看日志
docker-compose logs -f airdpro-linux

# 停止服务
docker-compose down
```

## 配置和数据管理

### 数据目录

- **数据卷**: `/data` - 输入/输出数据文件
- **日志卷**: `/logs` - 应用程序日志
- **配置**: 默认配置文件包含在容器中

### 卷挂载

```bash
# 挂载本地目录
docker run -v $(pwd)/my-data:/data -v $(pwd)/my-logs:/logs airdpro:linux

# 命名卷（推荐）
docker run --name airdpro-data -v airdpro-data:/data airdpro:linux
```

### 环境变量

```bash
# 性能调优
-e DOTNET_GCHeapCount=8
-e DOTNET_GCServer=1

# Wine优化
-e WINEPREFIX=/wine
-e WINEARCH=win64

# 日志级别
-e AIRDPRO_LOG_LEVEL=DEBUG
```

## 故障排除

### 常见问题

#### 1. Docker构建失败
```bash
# 清理Docker缓存
docker system prune -f

# 无缓存重建
docker build --no-cache --target runtime-linux -t airdpro:linux .
```

#### 2. Wine初始化问题
```bash
# 移除Wine前缀并重启
docker run -it --rm -e WINE_INITIALIZE=force airdpro:linux

# 检查Wine健康状态
docker exec airdpro-container /app/healthcheck.sh
```

#### 3. GUI显示问题 (macOS/Linux)
```bash
# 检查X11转发
xhost +local:docker

# 测试X11连接
docker run --rm -e DISPLAY=${DISPLAY} alpine xclock
```

#### 4. 权限问题
```bash
# 设置适当权限
chmod +x build-docker-en.sh run-gui-en.sh run-cli-en.sh

# 修复卷权限
sudo chown -R $USER:$USER data/ logs/
```

#### 5. 内存问题
```bash
# 检查Docker资源
docker system df

# 增加Docker内存分配
# Docker Desktop: 设置 > 资源 > 内存 > 8GB+
```

### 性能问题

#### 内存优化
- 为大型数据集将Docker内存增加到8GB+
- 必要时启用交换空间
- 对批处理使用CLI版本

#### 存储优化
```bash
# 清理未使用的镜像和容器
docker system prune -a

# 使用多阶段构建减小镜像大小
docker build --target runtime-cli -t airdpro:cli .
```

## 性能优化

### 构建优化
1. **使用BuildKit**: 启用BuildKit以获得更快的构建
   ```bash
   export DOCKER_BUILDKIT=1
   docker build --target runtime-linux -t airdpro:linux .
   ```

2. **多阶段构建**: 只在运行时镜像中包含必要文件

3. **缓存优化**: 使用.dockerignore排除不必要文件

### 运行时优化

#### 资源分配
```yaml
# docker-compose.yml 资源限制
services:
  airdpro:
    deploy:
      resources:
        limits:
          memory: 8G
          cpus: '4.0'
        reservations:
          memory: 4G
          cpus: '2.0'
```

#### 环境调优
```bash
# .NET优化
-e DOTNET_GCHeapCount=8
-e DOTNET_GCServer=1
-e DOTNET_TieredCompilation=false

# Wine优化
-e WINEDEBUG=-all
-e WINEINIT=disabled
```

### 网络优化
```bash
# 使用本地注册表加速镜像分发
docker tag airdpro:linux localhost:5000/airdpro:linux
docker push localhost:5000/airdpro:linux
```

## 安全注意事项

### 容器安全

1. **非root用户**: 可能时以非root用户运行容器
2. **只读文件系统**: 生产环境使用只读容器
3. **资源限制**: 设置内存和CPU限制

```bash
# 安全容器执行
docker run --rm \
    --read-only \
    --user 1000:1000 \
    --memory=4g \
    --cpu-shares=512 \
    airdpro:linux
```

### 数据安全

1. **加密卷**: 对敏感数据使用加密卷
2. **网络安全**: 尽可能限制网络访问
3. **镜像扫描**: 定期扫描镜像漏洞

```bash
# 扫描镜像漏洞
docker scan airdpro:linux

# 使用安全选项运行
docker run --rm \
    --security-opt=no-new-privileges \
    --cap-drop=ALL \
    --cap-add=NET_BIND_SERVICE \
    airdpro:linux
```

## 支持与维护

### 维护任务

1. **定期更新**: 每月更新基础镜像
2. **安全补丁**: 及时应用安全补丁
3. **性能监控**: 监控容器资源使用情况
4. **日志管理**: 定期日志轮换和清理

### 健康检查

```bash
# 检查容器健康状态
docker ps
docker inspect --format='{{.State.Health.Status}}' airdpro-container

# 手动健康检查
docker exec airdpro-container /app/healthcheck.sh
```

### 备份与恢复

```bash
# 备份数据卷
docker run --rm -v airdpro-data:/data -v $(pwd):/backup alpine \
    tar czf /backup/airdpro-data-backup.tar.gz /data

# 恢复数据
docker run --rm -v airdpro-data:/data -v $(pwd):/backup alpine \
    tar xzf /backup/airdpro-data-backup.tar.gz -C /
```

## 版本信息

- **当前版本**: 1.0.0
- **Docker版本**: 20.10+
- **平台支持**: Windows 10+、macOS 10.15+、Ubuntu 18.04+
- **最后更新**: 2024年12月

## 联系方式与支持

如有问题、疑问或贡献：

1. 检查上面的故障排除部分
2. 查看`logs/`目录中的日志
3. 在项目仓库中创建问题
4. 联系开发团队

---

**注意**: 本指南涵盖了最常见的情况。对于特定平台优化或高级配置，请参考特定平台文档或联系支持。
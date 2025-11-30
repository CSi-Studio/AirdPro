# AirdPro macOS平台部署启动说明

## 概述

本指南专门针对macOS平台用户提供AirdPro的完整部署和启动说明。通过容器化部署，macOS用户可以轻松使用这个强大的质谱数据分析工具。

## 目录

1. [系统要求](#系统要求)
2. [安装Docker Desktop](#安装docker-desktop)
3. [配置XQuartz支持GUI](#配置xquartz支持gui)
4. [项目下载和设置](#项目下载和设置)
5. [Docker镜像构建](#docker镜像构建)
6. [启动和运行](#启动和运行)
7. [常见问题和解决方案](#常见问题和解决方案)
8. [性能优化](#性能优化)
9. [维护和更新](#维护和更新)

## 系统要求

### 硬件要求

- **处理器**: Intel x64 或 Apple Silicon (M1/M2/M3) Mac
- **内存**: 最少8GB RAM（推荐16GB）
- **存储**: 至少20GB可用磁盘空间
- **macOS版本**: 10.15 (Catalina) 或更高版本

### 软件要求

- **Docker Desktop for Mac**: 版本4.0或更高
- **XQuartz**: 用于GUI支持
- **终端**: 任意终端应用（Terminal.app、iTerm2等）

### 网络要求

- 稳定的互联网连接用于下载Docker镜像
- 访问GitHub等代码托管平台

## 安装Docker Desktop

### 步骤1：下载Docker Desktop

1. 访问 [Docker官网](https://www.docker.com/products/docker-desktop/)
2. 下载适用于Mac的Docker Desktop
3. 选择正确的版本：
   - **Intel Mac**: 下载Intel处理器版本
   - **Apple Silicon**: 下载Apple Silicon (M1/M2/M3) 版本

### 步骤2：安装Docker Desktop

1. 双击下载的`.dmg`文件
2. 将Docker图标拖拽到Applications文件夹
3. 启动Docker Desktop应用程序
4. 等待Docker服务启动完成（状态栏显示绿色图标）

### 步骤3：验证安装

打开终端，运行以下命令验证安装：

```bash
docker --version
docker-compose --version
docker info
```

如果看到版本信息和服务详情，说明安装成功。

## 配置XQuartz支持GUI

### 步骤1：安装XQuartz

使用Homebrew安装（推荐）：

```bash
# 安装Homebrew（如果未安装）
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# 安装XQuartz
brew install --cask xquartz
```

或手动下载：
1. 访问 [XQuartz官网](https://www.xquartz.org/)
2. 下载最新版本
3. 双击安装包进行安装

### 步骤2：配置XQuartz

1. **启动XQuartz**:
   ```bash
   open -a XQuartz
   ```

2. **配置安全设置**:
   - 打开XQuartz > 首选项
   - 在"安全性"标签页中勾选"允许来自网络客户端的连接"
   - 勾选"允许来自网络客户端的连接"和"启用认证"

3. **设置X11转发**:
   ```bash
   # 设置DISPLAY环境变量
   export DISPLAY=host.docker.internal:0
   echo 'export DISPLAY=host.docker.internal:0' >> ~/.zshrc
   source ~/.zshrc
   ```

### 步骤3：验证X11设置

```bash
# 测试X11连接
xclock
# 或
xeyes
```

如果出现时钟或眼睛图案的GUI窗口，说明X11配置正确。

### 步骤4：允许Docker连接

```bash
# 添加Docker到X11访问列表
xhost +local:docker
```

## 项目下载和设置

### 步骤1：获取项目代码

```bash
# 克隆项目（如果使用Git）
git clone <项目仓库地址>
cd AirdPro

# 或下载ZIP文件并解压
```

### 步骤2：设置文件权限

```bash
# 设置脚本执行权限
chmod +x build-docker-en.sh
chmod +x run-gui-en.sh
chmod +x run-cli-en.sh

# 创建必要的目录
mkdir -p data logs
chmod 755 data logs
```

### 步骤3：检查文件结构

```bash
# 验证项目结构
ls -la
```

应该看到以下关键文件：
- `Dockerfile`
- `Dockerfile.crossplatform`
- `build-docker-en.sh`
- `run-gui-en.sh`
- `run-cli-en.sh`
- `docker-compose.yml`

## Docker镜像构建

### 方法1：使用自动化脚本（推荐）

```bash
# 构建所有镜像
./build-docker-en.sh

# 或仅构建Linux/macOS版本
./build-docker-en.sh --macos-only
```

### 方法2：手动构建

```bash
# 构建主要镜像
docker build --target runtime-linux -t airdpro:linux .

# 构建CLI版本（可选）
docker build --target runtime-cli -t airdpro:cli .

# 构建开发版本（可选）
docker build --target runtime-dev -t airdpro:dev .
```

### 方法3：使用Docker Compose

```bash
# 构建所有服务
docker-compose build

# 或仅构建特定服务
docker-compose build airdpro-linux
```

### 构建优化选项

```bash
# 使用BuildKit加速构建
export DOCKER_BUILDKIT=1
docker build --target runtime-linux -t airdpro:linux .

# 多线程构建（如果CPU核心允许）
docker build --target runtime-linux -t airdpro:linux . --memory=8g --cpu-shares=2048
```

## 启动和运行

### GUI模式运行

#### 方法1：使用自动化脚本（推荐）

```bash
# 运行GUI版本
./run-gui-en.sh

# 运行带参数的GUI版本
./run-gui-en.sh --macos-only --data-path ./data --log-level debug
```

#### 方法2：手动命令

```bash
# 设置X11环境变量
export DISPLAY=host.docker.internal:0

# 运行GUI容器
docker run -it --rm \
    --name airdpro-gui \
    -e DISPLAY=host.docker.internal:0 \
    -e XAUTHORITY=/tmp/.docker.xauth \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    -v /tmp/.X11-unix:/tmp/.X11-unix \
    airdpro:linux
```

#### X11认证配置

```bash
# 创建X11认证令牌
xauth list | grep $(hostname)
if [ $? -eq 0 ]; then
    xauth list | grep $(hostname) > /tmp/.docker.xauth
    chmod 644 /tmp/.docker.xauth
fi

# 使用认证运行
docker run -it --rm \
    -v /tmp/.docker.xauth:/tmp/.docker.xauth \
    -e XAUTHORITY=/tmp/.docker.xauth \
    -e DISPLAY=host.docker.internal:0 \
    airdpro:linux
```

### CLI模式运行

#### 方法1：使用自动化脚本

```bash
# 运行CLI版本
./run-cli-en.sh

# 交互式CLI模式
./run-cli-en.sh --interactive

# 批处理模式
./run-cli-en.sh --batch --input-file /data/analysis.csv
```

#### 方法2：手动命令

```bash
# 运行CLI容器
docker run -it --rm \
    --name airdpro-cli \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    airdpro:linux --help

# 批处理分析
docker run --rm \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    airdpro:linux batch-process --input /data/my-data --output /data/results
```

### Docker Compose运行

```bash
# 启动所有服务
docker-compose up -d

# 查看运行状态
docker-compose ps

# 查看日志
docker-compose logs -f airdpro-linux

# 停止服务
docker-compose down
```

### 常用运行示例

#### 1. 交互式分析会话

```bash
# 启动交互式GUI会话
./run-gui-en.sh --interactive --session-name "analysis-session-1"
```

#### 2. 批量处理数据

```bash
# 批量处理分析
docker run --rm \
    -v $(pwd)/batch-data:/data/input \
    -v $(pwd)/batch-results:/data/output \
    -v $(pwd)/logs:/logs \
    airdpro:linux batch-process \
        --input /data/input \
        --output /data/output \
        --config /data/config.json \
        --parallel-jobs 4
```

#### 3. 开发调试模式

```bash
# 开发模式运行
docker run -it --rm \
    --name airdpro-dev \
    -v $(pwd):/workspace \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    airdpro:dev --debug
```

## 常见问题和解决方案

### 1. Docker Desktop无法启动

**症状**: Docker Desktop启动失败或一直显示"Starting Docker"

**解决方案**:
```bash
# 重置Docker Desktop
docker system prune -af
docker system prune --volumes

# 重启Docker Desktop
# 系统偏好设置 > Docker > Restart
```

### 2. X11显示问题

**症状**: GUI应用程序无法显示窗口

**解决方案**:
```bash
# 检查XQuartz是否运行
ps aux | grep XQuartz

# 重启XQuartz
killall XQuartz
open -a XQuartz

# 重新设置DISPLAY
export DISPLAY=host.docker.internal:0

# 检查X11转发
xhost +local:docker
```

### 3. 权限问题

**症状**: 脚本无法执行或文件权限错误

**解决方案**:
```bash
# 修复脚本权限
chmod +x *.sh

# 修复目录权限
sudo chown -R $(whoami):staff data/ logs/

# 验证权限
ls -la data/ logs/
```

### 4. 内存不足

**症状**: 构建或运行时出现内存错误

**解决方案**:
```bash
# 增加Docker Desktop内存分配
# Docker Desktop > 设置 > 资源 > 内存 > 8GB

# 检查内存使用情况
docker stats

# 优化容器内存使用
docker run --rm -m 4g airdpro:linux
```

### 5. 网络连接问题

**症状**: 镜像拉取失败或网络超时

**解决方案**:
```bash
# 配置Docker镜像源
# Docker Desktop > 设置 > Docker Engine
{
  "registry-mirrors": ["https://docker.mirrors.ustc.edu.cn"]
}

# 清理网络缓存
docker network prune -f

# 重新启动Docker
```

### 6. Apple Silicon兼容性问题

**症状**: 在M1/M2 Mac上构建失败

**解决方案**:
```bash
# 为Apple Silicon使用正确的镜像
docker build --platform linux/amd64 --target runtime-linux -t airdpro:linux .

# 或在Docker Desktop中设置
# Settings > General > Use Rosetta for x86/amd64 emulation
```

## 性能优化

### Docker Desktop设置

1. **内存分配**: 
   - 设置 > 资源 > 内存: 8GB-16GB
   - 确保足够内存用于大文件处理

2. **CPU核心**: 
   - 设置 > 资源 > CPU: 6-8核心
   - 利用所有可用的CPU核心

3. **磁盘映像大小**:
   - 设置 > 资源 > 磁盘映像大小: 100GB+
   - 确保足够空间存储镜像和容器

### 容器运行时优化

```bash
# 资源限制设置
docker run --rm \
    --memory=8g \
    --cpus=6 \
    --memory-swap=12g \
    airdpro:linux

# 使用多阶段构建
docker build --target runtime-cli -t airdpro:cli .
```

### 文件系统优化

```bash
# 使用命名卷
docker volume create airdpro-data
docker volume create airdpro-logs

# 运行容器使用命名卷
docker run --rm \
    -v airdpro-data:/data \
    -v airdpro-logs:/logs \
    airdpro:linux
```

## 维护和更新

### 定期维护任务

```bash
# 清理未使用的镜像和容器
docker system prune -af

# 清理未使用的卷
docker volume prune

# 更新镜像
docker pull airdpro:latest

# 重建镜像以获取最新更新
docker build --no-cache --target runtime-linux -t airdpro:linux .
```

### 数据备份

```bash
# 备份数据卷
docker run --rm \
    -v airdpro-data:/data \
    -v $(pwd):/backup \
    alpine tar czf /backup/airdpro-data-$(date +%Y%m%d).tar.gz /data

# 备份配置文件
cp -r data/config backup/config-$(date +%Y%m%d)
```

### 日志管理

```bash
# 查看容器日志
docker logs airdpro-container

# 实时监控日志
docker logs -f airdpro-container

# 清理旧日志
find logs/ -name "*.log" -mtime +30 -delete
```

### 性能监控

```bash
# 监控容器资源使用
docker stats

# 检查磁盘使用
docker system df

# 监控容器健康状态
docker inspect --format='{{.State.Health.Status}}' airdpro-container
```

## 故障排除检查清单

### 启动前检查

- [ ] Docker Desktop正常运行
- [ ] XQuartz已安装并配置
- [ ] 项目文件权限正确
- [ ] 磁盘空间充足（>20GB）
- [ ] 网络连接正常

### 构建时检查

- [ ] 使用最新Docker版本
- [ ] Docker Desktop配置正确
- [ ] 内存分配充足
- [ ] 网络连接稳定
- [ ] 文件路径正确

### 运行时检查

- [ ] DISPLAY环境变量设置
- [ ] X11转发权限
- [ ] 数据目录挂载
- [ ] 日志目录可写
- [ ] 容器资源充足

## 支持和帮助

### 获取帮助

1. **查看日志**: 检查`logs/`目录中的日志文件
2. **运行诊断**: 使用内置诊断工具
3. **检查资源**: 使用`docker stats`检查资源使用
4. **重置环境**: 使用`docker system prune -af`重置

### 常用诊断命令

```bash
# 检查系统状态
docker info
docker version
docker-compose version

# 检查容器状态
docker ps -a
docker inspect airdpro-container

# 网络诊断
docker network ls
netstat -an | grep :3000

# 系统资源
top -p $(pgrep docker)
df -h
```

---

**重要提示**: 

1. 确保始终使用最新版本的Docker Desktop和XQuartz
2. 对于大型数据集，分配足够的内存和CPU资源
3. 定期备份重要数据文件
4. 遵循权限最佳实践，避免使用root权限
5. 在生产环境中监控容器资源使用情况

## 版本兼容性

- **macOS支持**: 10.15 (Catalina) 及更高版本
- **Docker Desktop**: 4.0+ (支持Apple Silicon和Intel)
- **XQuartz**: 最新稳定版本
- **Homebrew**: 最新版本（用于软件安装）

**最后更新**: 2024年12月
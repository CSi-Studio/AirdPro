# AirdPro Linux平台部署启动说明

## 概述

本指南为Linux用户提供AirdPro的完整部署和启动说明。通过容器化部署，Linux用户可以在各种发行版上使用这个强大的质谱数据分析工具，支持GUI和CLI两种运行模式。

## 目录

1. [系统要求](#系统要求)
2. [安装Docker](#安装docker)
3. [配置GUI支持](#配置gui支持)
4. [项目下载和设置](#项目下载和设置)
5. [Docker镜像构建](#docker镜像构建)
6. [启动和运行](#启动和运行)
7. [服务化部署](#服务化部署)
8. [常见问题和解决方案](#常见问题和解决方案)
9. [性能优化](#性能优化)
10. [维护和更新](#维护和更新)
11. [高级配置](#高级配置)

## 系统要求

### 支持的Linux发行版

- **Ubuntu**: 18.04 LTS及以上版本
- **Debian**: 10 (Buster)及以上版本
- **CentOS**: 7及以上版本
- **RHEL**: 7及以上版本
- **Fedora**: 30及以上版本
- **openSUSE**: Leap 15.0及以上版本
- **Arch Linux**: 最新稳定版

### 硬件要求

- **处理器**: x86_64 (AMD64)架构
- **内存**: 最少8GB RAM（推荐16GB）
- **存储**: 至少20GB可用磁盘空间
- **网络**: 稳定的互联网连接

### 软件要求

- **Docker**: 版本20.10或更高
- **Docker Compose**: 版本2.0或更高（可选）
- **X11**: 用于GUI支持（大多数桌面环境默认安装）
- **终端**: bash、zsh或sh

## 安装Docker

### Ubuntu/Debian系统

#### 步骤1：卸载旧版本

```bash
sudo apt remove docker docker-engine docker.io containerd runc
sudo apt autoremove
```

#### 步骤2：设置仓库

```bash
# 更新包索引
sudo apt update

# 安装必要工具
sudo apt install apt-transport-https ca-certificates curl gnupg lsb-release

# 添加Docker官方GPG密钥
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg

# 设置稳定版仓库
echo "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
```

#### 步骤3：安装Docker Engine

```bash
# 更新包索引
sudo apt update

# 安装最新版本
sudo apt install docker-ce docker-ce-cli containerd.io docker-compose-plugin

# 启动Docker服务
sudo systemctl start docker
sudo systemctl enable docker

# 添加用户到docker组（避免每次使用sudo）
sudo usermod -aG docker $USER

# 重新登录或运行
newgrp docker
```

### CentOS/RHEL/Fedora系统

#### CentOS/RHEL

```bash
# 安装必要工具
sudo yum install -y yum-utils

# 添加Docker仓库
sudo yum-config-manager --add-repo https://download.docker.com/linux/centos/docker-ce.repo

# 安装Docker
sudo yum install docker-ce docker-ce-cli containerd.io docker-compose-plugin

# 启动并启用Docker
sudo systemctl start docker
sudo systemctl enable docker

# 添加用户到docker组
sudo usermod -aG docker $USER
```

#### Fedora

```bash
# 安装必要工具
sudo dnf install -y yum-utils

# 添加Docker仓库
sudo dnf config-manager --add-repo https://download.docker.com/linux/fedora/docker-ce.repo

# 安装Docker
sudo dnf install docker-ce docker-ce-cli containerd.io docker-compose-plugin

# 启动并启用Docker
sudo systemctl start docker
sudo systemctl enable docker

# 添加用户到docker组
sudo usermod -aG docker $USER
```

### 验证安装

```bash
# 检查Docker版本
docker --version
docker compose version

# 测试Docker是否正常工作
sudo docker run hello-world

# 检查Docker服务状态
sudo systemctl status docker
```

### 使用脚本安装（所有发行版）

```bash
# 使用官方安装脚本
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# 启动Docker服务
sudo systemctl start docker
sudo systemctl enable docker

# 添加用户到docker组
sudo usermod -aG docker $USER
```

## 配置GUI支持

### 检查X11安装

```bash
# 检查X11是否安装
which xauth
echo $DISPLAY

# 如果未安装X11，Ubuntu/Debian系统运行
sudo apt install xauth x11-apps

# CentOS/RHEL系统运行
sudo yum install xorg-x11-xauth xorg-x11-utils

# Fedora系统运行
sudo dnf install xorg-x11-xauth xorg-x11-utils
```

### 配置X11转发

#### 启用X11转发

```bash
# 检查SSH配置（如果通过SSH连接）
sudo vim /etc/ssh/sshd_config

# 确保以下选项启用
X11Forwarding yes
X11DisplayOffset 10
X11UseLocalhost no

# 重启SSH服务
sudo systemctl restart sshd
```

#### 允许Docker访问X11

```bash
# 获取当前用户信息
export USER=$(whoami)

# 允许Docker访问X11
xhost +local:docker

# 创建X11认证令牌
touch ~/.Xauthority
xauth list | grep $(hostname) > ~/.docker.xauth
chmod 644 ~/.docker.xauth

# 添加到shell配置（可选）
echo 'export XAUTHORITY=~/.docker.xauth' >> ~/.bashrc
source ~/.bashrc
```

### 测试GUI支持

```bash
# 测试X11连接
docker run --rm -e DISPLAY=$DISPLAY -v /tmp/.X11-unix:/tmp/.X11-unix alpine xclock

# 如果出现时钟窗口，说明GUI配置正确
```

## 项目下载和设置

### 获取项目代码

#### 使用Git克隆

```bash
# 克隆项目（替换为实际仓库地址）
git clone <项目仓库地址>
cd AirdPro
```

#### 下载ZIP文件

```bash
# 下载并解压
wget <项目下载链接> -O airdpro.zip
unzip airdpro.zip
cd AirdPro
```

### 设置文件权限

```bash
# 设置脚本执行权限
chmod +x *.sh

# 创建必要目录
mkdir -p data logs
chmod 755 data logs

# 设置适当的所有权
sudo chown -R $USER:$USER data/ logs/

# 验证权限设置
ls -la data/ logs/
```

### 检查项目结构

```bash
# 验证项目文件
ls -la

# 检查Docker相关文件
ls -la Dockerfile*
ls -la *.sh
ls -la docker-compose.yml

# 检查权限
./build-docker-en.sh --help
```

## Docker镜像构建

### 方法1：使用自动化脚本（推荐）

```bash
# 构建所有镜像
./build-docker-en.sh

# 仅构建Linux版本
./build-docker-en.sh --linux-only

# 查看帮助信息
./build-docker-en.sh --help
```

### 方法2：手动构建

```bash
# 构建主要镜像
docker build --target runtime-linux -t airdpro:linux .

# 构建CLI版本（轻量级）
docker build --target runtime-cli -t airdpro:cli .

# 构建开发版本
docker build --target runtime-dev -t airdpro:dev .

# 查看构建结果
docker images | grep airdpro
```

### 方法3：使用Docker Compose

```bash
# 构建所有服务
docker-compose build

# 构建特定服务
docker-compose build airdpro-linux
docker-compose build airdpro-cli

# 查看构建进度
docker-compose build --no-cache
```

### 构建优化选项

```bash
# 启用BuildKit加速构建
export DOCKER_BUILDKIT=1

# 多阶段构建减小镜像大小
docker build --target runtime-cli -t airdpro:cli .

# 使用并行构建
docker build --target runtime-linux -t airdpro:linux . --memory=8g --cpu-shares=2048

# 无缓存重建（获取最新依赖）
docker build --no-cache --target runtime-linux -t airdpro:linux .
```

## 启动和运行

### GUI模式运行

#### 方法1：使用自动化脚本（推荐）

```bash
# 运行GUI版本
./run-gui-en.sh

# 带参数运行
./run-gui-en.sh --data-path ./data --log-level debug

# 查看脚本帮助
./run-gui-en.sh --help
```

#### 方法2：手动命令

```bash
# 设置环境变量
export DISPLAY=$DISPLAY
export XAUTHORITY=~/.docker.xauth

# 运行GUI容器
docker run -it --rm \
    --name airdpro-gui \
    -e DISPLAY=$DISPLAY \
    -e XAUTHORITY=/root/.docker.xauth \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    -v /tmp/.X11-unix:/tmp/.X11-unix \
    -v ~/.docker.xauth:/root/.docker.xauth:ro \
    airdpro:linux
```

### CLI模式运行

#### 方法1：使用自动化脚本

```bash
# 运行CLI版本
./run-cli-en.sh

# 交互式模式
./run-cli-en.sh --interactive

# 批处理模式
./run-cli-en.sh --batch --input-file /data/analysis.csv
```

#### 方法2：手动命令

```bash
# 基本CLI运行
docker run -it --rm \
    --name airdpro-cli \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    airdpro:linux

# 带参数的CLI运行
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

# 重启服务
docker-compose restart airdpro-linux
```

### 常用运行示例

#### 1. 交互式分析会话

```bash
# 启动交互式GUI会话
./run-gui-en.sh --interactive --session-name "analysis-session-1"

# 或手动启动
docker run -it --rm \
    --name airdpro-session-1 \
    -e DISPLAY=$DISPLAY \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    airdpro:linux --session analysis-session-1
```

#### 2. 批量处理数据

```bash
# 批量处理
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
# 开发模式
docker run -it --rm \
    --name airdpro-dev \
    -e DEBUG=1 \
    -v $(pwd):/workspace \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    airdpro:dev
```

## 服务化部署

### 使用systemd服务

#### 创建systemd单元文件

```bash
# 创建服务文件
sudo tee /etc/systemd/system/airdpro.service > /dev/null <<EOF
[Unit]
Description=AirdPro Docker Container
Requires=docker.service
After=docker.service

[Service]
Type=oneshot
RemainAfterExit=yes
User=$USER
ExecStart=/usr/bin/docker run -it --rm \
    --name airdpro-service \
    -e DISPLAY=$DISPLAY \
    -v /home/$USER/AirdPro/data:/data \
    -v /home/$USER/AirdPro/logs:/logs \
    -v /tmp/.X11-unix:/tmp/.X11-unix \
    -v /home/$USER/.docker.xauth:/root/.docker.xauth:ro \
    airdpro:linux
ExecStop=/usr/bin/docker stop airdpro-service
ExecStopPost=/usr/bin/docker rm airdpro-service

[Install]
WantedBy=multi-user.target
EOF
```

#### 启用和管理服务

```bash
# 重新加载systemd配置
sudo systemctl daemon-reload

# 启用服务（开机自启）
sudo systemctl enable airdpro

# 启动服务
sudo systemctl start airdpro

# 查看服务状态
sudo systemctl status airdpro

# 停止服务
sudo systemctl stop airdpro
```

### 使用Docker Compose服务

#### 创建生产环境compose文件

```bash
# 创建生产环境compose文件
cat > docker-compose.prod.yml <<EOF
version: '3.8'

services:
  airdpro:
    image: airdpro:linux
    container_name: airdpro-service
    restart: unless-stopped
    environment:
      - DISPLAY=${DISPLAY}
      - XAUTHORITY=/root/.docker.xauth
    volumes:
      - ./data:/data
      - ./logs:/logs
      - /tmp/.X11-unix:/tmp/.X11-unix
      - ${HOME}/.docker.xauth:/root/.docker.xauth:ro
    networks:
      - airdpro-network
    deploy:
      resources:
        limits:
          memory: 8G
          cpus: '4.0'
        reservations:
          memory: 4G
          cpus: '2.0'

networks:
  airdpro-network:
    driver: bridge
EOF
```

#### 启动生产服务

```bash
# 启动生产服务
docker-compose -f docker-compose.prod.yml up -d

# 查看服务状态
docker-compose -f docker-compose.prod.yml ps

# 查看日志
docker-compose -f docker-compose.prod.yml logs -f

# 停止服务
docker-compose -f docker-compose.prod.yml down
```

## 常见问题和解决方案

### 1. Docker权限问题

**症状**: "Permission denied"错误

**解决方案**:
```bash
# 添加用户到docker组
sudo usermod -aG docker $USER

# 重新登录或运行
newgrp docker

# 验证组成员身份
groups $USER

# 如果仍然有问题，重启Docker服务
sudo systemctl restart docker
```

### 2. X11显示问题

**症状**: GUI应用程序无法显示

**解决方案**:
```bash
# 检查DISPLAY环境变量
echo $DISPLAY

# 检查X11是否运行
ps aux | grep X11
ps aux | grep xauth

# 重新设置X11权限
xhost +local:docker
export XAUTHORITY=~/.docker.xauth

# 检查X11转发设置
xclock  # 测试X11功能
```

### 3. 端口冲突

**症状**: 容器启动失败，提示端口被占用

**解决方案**:
```bash
# 检查端口占用
sudo netstat -tulpn | grep :3000
sudo lsof -i :3000

# 使用不同端口
docker run -p 3001:3000 airdpro:linux

# 停止占用端口的进程
sudo kill -9 <PID>
```

### 4. 内存不足

**症状**: 构建或运行时出现内存错误

**解决方案**:
```bash
# 检查系统内存
free -h
df -h

# 清理Docker缓存
docker system prune -af

# 增加swap空间
sudo fallocate -l 4G /swapfile
sudo chmod 600 /swapfile
sudo mkswap /swapfile
sudo swapon /swapfile

# 限制容器内存使用
docker run -m 4g airdpro:linux
```

### 5. 网络连接问题

**症状**: 镜像拉取失败或网络超时

**解决方案**:
```bash
# 检查网络连接
ping 8.8.8.8
ping docker.io

# 配置Docker镜像源（中国用户）
sudo tee /etc/docker/daemon.json > /dev/null <<EOF
{
  "registry-mirrors": [
    "https://docker.mirrors.ustc.edu.cn",
    "https://hub-mirror.c.163.com",
    "https://mirror.baidubce.com"
  ]
}
EOF

# 重启Docker服务
sudo systemctl restart docker
```

### 6. 构建失败

**症状**: Docker构建过程失败

**解决方案**:
```bash
# 清理Docker构建缓存
docker builder prune -af

# 重新构建（无缓存）
docker build --no-cache --target runtime-linux -t airdpro:linux .

# 检查Docker版本兼容性
docker version

# 重新拉取基础镜像
docker pull ubuntu:22.04
docker system prune -af
```

### 7. GUI启动慢

**症状**: GUI启动时间长或卡顿

**解决方案**:
```bash
# 使用虚拟显示加速
sudo apt install xvfb  # Ubuntu/Debian
sudo yum install Xvfb  # CentOS/RHEL

# 使用虚拟显示运行
xvfb-run docker run -it --rm -e DISPLAY=:99 airdpro:linux

# 检查系统负载
top
htop
```

## 性能优化

### 系统级优化

#### Docker配置优化

```bash
# 编辑Docker daemon配置
sudo tee /etc/docker/daemon.json > /dev/null <<EOF
{
  "storage-driver": "overlay2",
  "log-driver": "json-file",
  "log-opts": {
    "max-size": "10m",
    "max-file": "3"
  },
  "default-runtime": "runc",
  "runtimes": {
    "runc": {
      "path": "runc"
    }
  },
  "default-ulimits": {
    "nofile": {
      "Name": "nofile",
      "Hard": 64000,
      "Soft": 64000
    }
  }
}
EOF

# 重启Docker服务
sudo systemctl restart docker
```

#### 系统资源优化

```bash
# 增加文件描述符限制
echo "* soft nofile 65536" | sudo tee -a /etc/security/limits.conf
echo "* hard nofile 65536" | sudo tee -a /etc/security/limits.conf

# 优化内核参数
echo "vm.swappiness=10" | sudo tee -a /etc/sysctl.conf
echo "vm.dirty_ratio=15" | sudo tee -a /etc/sysctl.conf
echo "vm.dirty_background_ratio=5" | sudo tee -a /etc/sysctl.conf
sudo sysctl -p
```

### 容器级优化

#### 资源限制配置

```bash
# 设置CPU和内存限制
docker run --rm \
    --memory=8g \
    --memory-swap=12g \
    --cpus=6 \
    --cpu-shares=2048 \
    --pids-limit=1000 \
    airdpro:linux

# 使用Docker Compose资源限制
cat > docker-compose.resource.yml <<EOF
version: '3.8'
services:
  airdpro:
    image: airdpro:linux
    deploy:
      resources:
        limits:
          memory: 8G
          cpus: '6.0'
        reservations:
          memory: 4G
          cpus: '2.0'
EOF
```

#### 网络优化

```bash
# 使用host网络模式（如果不需要网络隔离）
docker run --network host airdpro:linux

# 使用自定义网络
docker network create airdpro-network
docker run --network airdpro-network airdpro:linux
```

### 构建优化

#### 多阶段构建

```bash
# 使用多阶段构建减小镜像大小
docker build --target runtime-cli -t airdpro:cli .

# 使用.dockerignore文件
cat > .dockerignore <<EOF
.git
.gitignore
README.md
*.md
.vscode
node_modules
npm-debug.log
Dockerfile*
docker-compose*
EOF

# 启用BuildKit
export DOCKER_BUILDKIT=1
docker build --target runtime-linux -t airdpro:linux .
```

## 维护和更新

### 定期维护任务

```bash
# 清理未使用的镜像和容器
docker system prune -af

# 清理未使用的卷
docker volume prune

# 清理未使用的网络
docker network prune

# 查看Docker磁盘使用情况
docker system df

# 完整清理
docker system prune -af --volumes
```

### 镜像更新

```bash
# 重新构建镜像获取最新更新
docker build --no-cache --target runtime-linux -t airdpro:linux .

# 拉取最新基础镜像
docker pull ubuntu:22.04
docker pull mcr.microsoft.com/dotnet/framework/sdk:4.8

# 更新Docker Compose服务
docker-compose pull
docker-compose up -d --force-recreate
```

### 数据备份

```bash
# 备份数据卷
docker run --rm \
    -v airdpro-data:/data \
    -v $(pwd):/backup \
    alpine tar czf /backup/airdpro-data-$(date +%Y%m%d).tar.gz /data

# 使用rsync进行增量备份
rsync -av --progress /path/to/data/ /path/to/backup/data/

# 自动化备份脚本
cat > backup-airdpro.sh <<'EOF'
#!/bin/bash
BACKUP_DIR="/backup/airdpro"
DATE=$(date +%Y%m%d_%H%M%S)

mkdir -p $BACKUP_DIR
docker run --rm \
    -v airdpro-data:/data \
    -v $BACKUP_DIR:/backup \
    alpine tar czf /backup/airdpro-data-$DATE.tar.gz /data

# 保留最近7天的备份
find $BACKUP_DIR -name "airdpro-data-*.tar.gz" -mtime +7 -delete
EOF

chmod +x backup-airdpro.sh
```

### 日志管理

```bash
# 查看容器日志
docker logs airdpro-container
docker logs -f airdpro-container --tail 100

# 日志轮转配置
sudo tee /etc/logrotate.d/airdpro <<EOF
/var/lib/docker/containers/*/*.log {
    daily
    missingok
    rotate 7
    compress
    delaycompress
    notifempty
    copytruncate
}
EOF

# 实时监控日志
tail -f logs/airdpro.log
```

### 性能监控

```bash
# 监控容器资源使用
docker stats

# 系统资源监控
htop
iotop
nethogs

# Docker特定监控
docker info
docker system df

# 健康检查
docker inspect --format='{{.State.Health.Status}}' airdpro-container
```

## 高级配置

### 自定义Docker网络

```bash
# 创建专用网络
docker network create airdpro-network --driver bridge

# 运行容器使用专用网络
docker run --network airdpro-network \
    --name airdpro-container \
    airdpro:linux

# 网络配置检查
docker network inspect airdpro-network
```

### 数据持久化

```bash
# 创建命名数据卷
docker volume create airdpro-data
docker volume create airdpro-logs

# 使用命名卷运行
docker run -v airdpro-data:/data \
    -v airdpro-logs:/logs \
    airdpro:linux

# 查看卷信息
docker volume ls
docker volume inspect airdpro-data
```

### 安全配置

```bash
# 以非root用户运行
docker run --user 1000:1000 airdpro:linux

# 只读容器
docker run --read-only airdpro:linux

# 限制容器功能
docker run --cap-drop=ALL \
    --cap-add=NET_BIND_SERVICE \
    airdpro:linux

# 安全选项
docker run --security-opt=no-new-privileges \
    --security-opt=apparmor:unconfined \
    airdpro:linux
```

### 监控和报警

```bash
# 使用cAdvisor监控
docker run \
    --volume=/:/rootfs:ro \
    --volume=/var/run:/var/run:ro \
    --volume=/sys:/sys:ro \
    --volume=/var/lib/docker/:/var/lib/docker:ro \
    --publish=8080:8080 \
    --detach=true \
    --name=cadvisor \
    gcr.io/cadvisor/cadvisor

# 健康检查脚本
cat > healthcheck.sh <<'EOF'
#!/bin/bash
if docker ps | grep -q airdpro-container; then
    if docker inspect --format='{{.State.Health.Status}}' airdpro-container | grep -q "healthy"; then
        echo "AirdPro is healthy"
        exit 0
    else
        echo "AirdPro is unhealthy"
        exit 1
    fi
else
    echo "AirdPro container is not running"
    exit 1
fi
EOF

chmod +x healthcheck.sh
```

## 故障排除检查清单

### 部署前检查

- [ ] 系统满足最低要求
- [ ] Docker已正确安装并运行
- [ ] 用户在docker组中
- [ ] 网络连接正常
- [ ] 磁盘空间充足（>20GB）

### 构建时检查

- [ ] Docker daemon运行正常
- [ ] 足够的内存和CPU资源
- [ ] 网络连接稳定
- [ ] 文件权限正确
- [ ] 防火墙配置允许Docker

### 运行时检查

- [ ] X11配置正确（GUI模式）
- [ ] DISPLAY环境变量设置
- [ ] 数据目录挂载正常
- [ ] 容器资源限制合理
- [ ] 日志目录可写

### 维护时检查

- [ ] 定期备份数据
- [ ] 监控磁盘使用
- [ ] 更新安全补丁
- [ ] 检查容器健康状态
- [ ] 清理过期镜像

## 支持和帮助

### 获取帮助

1. **查看日志**: 检查`logs/`目录中的日志文件
2. **运行诊断**: 使用`docker logs`、`docker stats`等命令
3. **检查资源**: 监控系统资源使用情况
4. **重置环境**: 使用`docker system prune -af`重置

### 常用诊断命令

```bash
# 系统信息
uname -a
cat /etc/os-release
docker --version
docker info

# 容器状态
docker ps -a
docker inspect airdpro-container
docker logs airdpro-container
docker logs -f airdpro-container

# 网络诊断
docker network ls
docker network inspect bridge
netstat -tulpn | grep docker

# 资源监控
docker stats
top -p $(pgrep dockerd)
free -h
df -h
```

### 获取技术支持

1. **查看项目文档**: 阅读README和相关文档
2. **检查GitHub Issues**: 搜索已知问题和解决方案
3. **运行诊断脚本**: 使用项目提供的诊断工具
4. **收集日志信息**: 准备详细的日志和错误信息

---

**重要提示**:

1. 始终使用最新版本的Docker和操作系统
2. 为生产环境定期备份数据
3. 监控系统资源使用，避免资源耗尽
4. 遵循安全最佳实践，避免不必要的安全风险
5. 在生产环境中考虑使用负载均衡和高可用性配置

**版本兼容性**:
- **Docker**: 20.10+
- **Linux内核**: 4.0+
- **支持发行版**: Ubuntu 18.04+, Debian 10+, CentOS 7+, Fedora 30+

**最后更新**: 2024年12月
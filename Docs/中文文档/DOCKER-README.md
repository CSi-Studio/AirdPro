# AirdPro Docker化部署指南（macOS + Wine）

## 项目概述

AirdPro是一个基于.NET Framework 4.8的Windows桌面应用程序，包含GUI界面和命令行功能。由于macOS原生不支持.NET Framework，本项目通过Docker + Wine技术实现在macOS上的运行。

**重要提示**: 此方案使用Wine在Linux容器中运行Windows应用程序，性能可能不如原生Windows环境，且首次运行需要较长时间初始化。

## 前提条件

### macOS环境要求
- macOS 10.15+ (Catalina或更高版本)
- Docker Desktop for Mac（版本20.10+）
- XQuartz（用于GUI显示支持）
- 至少8GB可用内存（推荐16GB）
- 稳定的网络连接（首次运行需要下载Wine和.NET Framework）

### 硬件要求
- Intel或Apple Silicon Mac
- 至少20GB可用磁盘空间
- 支持虚拟化的CPU

## 快速开始

### 1. 安装依赖

#### 安装Docker Desktop
```bash
# 下载并安装Docker Desktop for Mac
# 下载地址: https://www.docker.com/products/docker-desktop

# 安装后启动Docker Desktop
open -a "Docker Desktop"
```

#### 安装XQuartz（GUI版本必需）
```bash
# 使用Homebrew安装XQuartz
brew install --cask xquartz

# 启动XQuartz
open -a XQuartz

# 配置XQuartz（首次运行需要）
# 1. 打开XQuartz偏好设置
# 2. 在"安全"选项卡中勾选"允许来自网络客户端的连接"
# 3. 重启XQuartz
```

### 2. 构建Docker镜像

```bash
# 构建所有镜像（首次构建需要较长时间）
./build-docker.sh

# 构建过程说明：
# - 下载Ubuntu基础镜像
# - 安装Wine和依赖包
# - 初始化Wine环境
# - 安装.NET Framework 4.8
# - 编译AirdPro应用程序
```

### 3. 测试配置

```bash
# 运行配置测试
./test-docker.sh
```

### 4. 运行应用程序

#### GUI版本（推荐用于日常使用）
```bash
./run-gui.sh
```

#### CLI版本（用于批处理任务）
```bash
# 显示帮助
./run-cli.sh --help

# 文件转换示例
./run-cli.sh -i data/input.raw -o data/output.mzML
```

## 📁 目录结构

```
AirdPro/
├── Dockerfile              # 多阶段Docker构建配置
├── docker-compose.yml      # Docker Compose配置
├── build-docker.sh         # macOS/Linux构建脚本
├── run-gui.sh             # GUI版本运行脚本
├── run-cli.sh             # 命令行版本运行脚本
├── build-docker.bat        # Windows构建脚本
├── run-windows.bat         # Windows运行脚本
├── data/                   # 数据目录（自动创建）
├── logs/                   # 日志目录（自动创建）
└── DOCKER-README.md       # 本文档
```

## 🔧 Docker配置说明

### 多阶段构建

Dockerfile包含三个阶段：

1. **构建阶段**：使用.NET Framework SDK编译项目
2. **Windows运行时**：使用Windows容器运行（适用于Windows Docker环境）
3. **Linux运行时**：使用Wine在Linux容器中运行Windows应用（适用于macOS/Linux）

### 环境变量

- `WINEPREFIX`: Wine配置目录
- `WINEDEBUG`: Wine调试级别
- `DISPLAY`: X11显示设置（GUI版本需要）
- `DOTNET_RUNNING_IN_CONTAINER`: .NET容器运行标识

### 数据持久化

- `./data` 目录映射到容器的 `/data` 路径
- `./logs` 目录映射到容器的 `/app/logs` 路径

## 🛠️ 高级用法

### 使用Docker Compose

```bash
# 构建并启动Linux版本
docker-compose up airdpro-linux

# 仅构建
docker-compose build

# 后台运行
docker-compose up -d airdpro-linux
```

### 自定义构建

```bash
# 仅构建Linux版本
docker build --target runtime-linux -t airdpro:custom .

# 构建Windows版本（需要Windows Docker环境）
docker build --target runtime-windows -t airdpro:windows .
```

## 🔍 故障排除

### 常见问题

1. **X11连接失败**
   - 确保XQuartz正在运行
   - 检查XQuartz偏好设置中的网络客户端连接选项
   - 尝试重启XQuartz

2. **Wine初始化失败**
   - 删除现有的Wine配置：`rm -rf ~/.wine`
   - 重新构建Docker镜像

3. **权限问题**
   - 确保脚本有执行权限：`chmod +x *.sh`
   - 检查Docker是否有足够的权限

4. **容器启动失败**
   - 检查Docker日志：`docker logs airdpro-linux`
   - 验证镜像是否构建成功：`docker images | grep airdpro`

### 调试模式

```bash
# 进入容器进行调试
docker run -it --rm --entrypoint /bin/bash airdpro:linux

# 查看容器日志
docker logs airdpro-linux
```

## 📝 使用示例

### 命令行模式示例

```bash
# 将本地文件复制到数据目录
cp your-file.raw ./data/

# 运行转换任务
./run-cli.sh -i /data/your-file.raw -o /data/output --config Default

# 查看结果
ls ./data/output/
```

### GUI模式注意事项

- GUI模式在macOS上通过X11转发实现
- 性能可能不如原生Windows环境
- 建议优先使用命令行模式进行批量处理

## 🔄 更新和维护

### 更新Docker镜像

当源代码更新时，需要重新构建镜像：

```bash
# 清理旧镜像
docker rmi airdpro:linux airdpro:cli

# 重新构建
./build-docker.sh
```

### 清理Docker资源

```bash
# 停止所有容器
docker stop $(docker ps -aq)

# 删除所有容器
docker rm $(docker ps -aq)

# 删除所有镜像
docker rmi $(docker images -q)

# 清理未使用的资源
docker system prune -a
```

## 📞 技术支持

如果遇到问题，请检查：

1. Docker Desktop是否正常运行
2. 系统资源是否充足
3. 网络连接是否正常
4. 查看日志文件获取详细错误信息

## 📄 许可证

本项目基于Mulan PSL v2许可证。详细信息请参阅LICENSE文件。

---

**注意**: 这是一个技术预览版本，GUI功能在macOS上可能有限制。建议在生产环境中使用命令行模式。
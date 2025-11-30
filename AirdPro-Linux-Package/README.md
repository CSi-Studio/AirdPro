# AirdPro Linux分发包

## 概述

这是专门为Linux系统设计的AirdPro应用程序分发包，提供了两种运行方式：直接使用Wine和通过Docker容器运行。本分发包支持多种Linux发行版，包括Ubuntu、Debian、CentOS、Fedora和Arch Linux等主流发行版。

## 文件结构

```
AirdPro-Linux-Package/
├── README.md                    # 本文件
├── install-linux.sh             # 自动安装脚本
├── Build_Run_Scripts/           # 构建和运行脚本目录
│   ├── run-docker-linux.sh     # Docker运行脚本
│   └── run-wine-linux.sh       # Wine运行脚本
├── Docs/                       # 文档目录
│   ├── Chinese_Docs/           # 中文文档
│   │   └── Linux平台部署启动说明.md
│   └── English_Docs/           # 英文文档
│       └── Linux_Platform_Deployment_Guide.md
├── Dockerfile.crossplatform     # 跨平台Docker文件（参考）
└── docker-compose.yml          # Docker Compose配置（参考）
```

## 快速开始

### 方法一：使用Docker（推荐）

1. **自动安装依赖**：
   ```bash
   chmod +x install-linux.sh
   ./install-linux.sh --mode full
   ```

2. **构建并运行**：
   ```bash
   chmod +x Build_Run_Scripts/run-docker-linux.sh
   ./Build_Run_Scripts/run-docker-linux.sh --build
   ```

3. **启动应用程序**：
   ```bash
   # 标准启动
   ./Build_Run_Scripts/run-docker-linux.sh
   
   # 交互模式
   ./Build_Run_Scripts/run-docker-linux.sh --interactive
   
   # 调试模式
   ./Build_Run_Scripts/run-docker-linux.sh --debug
   ```

### 方法二：直接使用Wine

1. **安装最小依赖**：
   ```bash
   chmod +x install-linux.sh
   ./install-linux.sh --mode minimal
   ```

2. **运行应用程序**：
   ```bash
   chmod +x Build_Run_Scripts/run-wine-linux.sh
   ./Build_Run_Scripts/run-wine-linux.sh
   ```

3. **高级Wine选项**：
   ```bash
   # 强制X11模式
   ./Build_Run_Scripts/run-wine-linux.sh --gui-mode x11
   
   # 32位模式
   ./Build_Run_Scripts/run-wine-linux.sh --wine-arch win32
   
   # 调试模式
   ./Build_Run_Scripts/run-wine-linux.sh --debug
   ```

## 系统要求

### 硬件要求
- **处理器**: x86_64 (64位) 兼容处理器
- **内存**: 至少 2GB RAM，推荐 4GB 以上
- **存储空间**: 至少 5GB 可用空间
- **显卡**: 支持X11或Wayland显示服务器（GUI模式）

### 软件要求

#### 最低要求
- **Linux内核**: 4.0 或更高版本
- **发行版支持**: 
  - Ubuntu 18.04+ / Debian 10+ / CentOS 7+ / Fedora 30+ / Arch Linux
- **X11显示服务器**（用于GUI模式）

#### Docker模式额外要求
- **Docker**: 20.10+ 
- **Docker Compose**: 2.0+
- **用户权限**: 能访问Docker守护进程

#### Wine模式额外要求
- **Wine**: 最新稳定版（6.0+ 推荐）
- **Wine架构支持**: wine64 和 wine32（根据需要）
- **依赖库**: libx11-6, libfontconfig1 等

### 网络要求
- 互联网连接（用于下载依赖包和首次运行）
- 如果在公司网络环境中，可能需要配置代理

## 特性与优势

### 双运行模式支持
- **Docker模式**: 容器化隔离，安全性更高，维护更容易
- **Wine模式**: 直接运行，性能更佳，资源占用更少

### 智能安装脚本
- **自动检测**: 自动检测Linux发行版和版本
- **依赖管理**: 自动安装和配置所需依赖
- **多种安装模式**: full/minimal/gui-only三种模式
- **权限检查**: 自动检查和配置Docker权限

### 完善的运行脚本
- **Docker运行脚本**: 完整的容器生命周期管理
- **Wine运行脚本**: 智能Wine环境配置和运行
- **调试支持**: 详细的日志和调试信息
- **多种选项**: 支持各种配置和运行选项

### 完整文档支持
- **中文文档**: 详细的中文部署启动说明
- **英文文档**: 完整的英文技术文档
- **故障排除**: 常见问题解决方案
- **性能优化**: 性能调优建议

## 高级功能

### Docker模式功能
```bash
# 高性能配置
./Build_Run_Scripts/run-docker-linux.sh --memory 8g --cpus 4

# 自定义数据目录
./Build_Run_Scripts/run-docker-linux.sh --data-dir /custom/path

# 查看实时日志
./Build_Run_Scripts/run-docker-linux.sh logs

# 资源清理
./Build_Run_Scripts/run-docker-linux.sh cleanup
```

### Wine模式功能
```bash
# 自定义Wine前缀
./Build_Run_Scripts/run-wine-linux.sh --wine-prefix /custom/prefix

# 无头模式（服务器环境）
./Build_Run_Scripts/run-wine-linux.sh --gui-mode headless

# 快速模式（跳过检查）
./Build_Run_Scripts/run-wine-linux.sh --fast

# 重置Wine环境
./Build_Run_Scripts/run-wine-linux.sh reset
```

## 故障排除

### 常见问题快速解决

#### 1. Docker权限问题
```bash
# 添加用户到docker组
sudo usermod -aG docker $USER
newgrp docker
```

#### 2. X11显示问题
```bash
# 设置X11权限
xhost +local:docker

# 检查DISPLAY环境变量
echo $DISPLAY
```

#### 3. Wine初始化问题
```bash
# 重置Wine环境
./Build_Run_Scripts/run-wine-linux.sh reset
```

#### 4. 系统依赖问题
```bash
# 重新运行安装脚本
./install-linux.sh --mode full --force
```

### 日志文件位置
- **Docker模式**: `./logs/airdpro.log`
- **Wine模式**: `./logs/wine-*.log`
- **系统日志**: `/var/log/syslog`

## 性能优化建议

### Docker模式优化
- 使用本地SSD存储数据
- 合理设置内存和CPU限制
- 启用Docker Desktop的增强功能

### Wine模式优化
- 使用最新的稳定版Wine
- 根据需要选择合适的Wine前缀
- 配置合适的Wine参数

### 系统级优化
- 关闭不必要的后台程序
- 确保足够的磁盘空间
- 使用高速网络连接

## 安全注意事项

1. **权限管理**: 不要以root用户运行应用程序
2. **网络安全**: 在防火墙中限制不必要的端口
3. **数据保护**: 定期备份重要数据
4. **更新维护**: 定期更新系统和依赖包

## 卸载清理

### 卸载Docker版本
```bash
./Build_Run_Scripts/run-docker-linux.sh stop
./Build_Run_Scripts/run-docker-linux.sh cleanup

# 可选：完全清理Docker资源
docker system prune -af
```

### 卸载Wine版本
```bash
./Build_Run_Scripts/run-wine-linux.sh cleanup

# 可选：完全清理Wine环境
rm -rf ~/.wine ~/.wine-airdpro
```

## 技术支持

### 获取帮助
1. **查看文档**: 首先阅读`Docs/`目录下的详细文档
2. **检查日志**: 查看日志文件定位问题
3. **运行诊断**: 使用 `--debug` 选项获取详细信息
4. **社区支持**: 在项目仓库提交Issue

### 系统信息收集
运行以下命令收集系统信息用于问题报告：
```bash
# 系统信息
uname -a
cat /etc/os-release

# Docker信息（如果使用Docker模式）
docker --version
docker-compose --version

# Wine信息（如果使用Wine模式）
wine --version

# 内存和存储
free -h
df -h
```

## 版本信息

- **包版本**: v4.2.0
- **目标平台**: Linux x86_64
- **支持的发行版**: Ubuntu 18.04+, Debian 10+, CentOS 7+, Fedora 30+, Arch Linux
- **构建日期**: 2025年11月30日
- **文档更新**: 2025年11月30日

## 许可证

本分发包遵循AirdPro原始软件许可证条款。请确保您有合法的软件使用许可。

---

**注意**: 这是一个社区维护的Linux分发包，非官方发布版本。使用前请确保您有合法的软件许可。

## 更新日志

### v4.2.0 (2025-11-30)
- 初始Linux分发包发布
- 支持Docker和Wine两种运行模式
- 提供完整的安装脚本和运行脚本
- 包含中英文详细文档
- 支持多种主流Linux发行版
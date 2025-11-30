# AirdPro macOS 分发包

## 概述

这是专门为macOS系统设计的AirdPro应用程序分发包，提供了两种运行方式：直接使用Wine和通过Docker容器运行。

## 文件结构

```
AirdPro-MacOS-Package/
├── README.md                    # 本文件
├── MacOS-Install-Run.md         # 详细安装运行指南
├── install-macos.sh             # 自动安装脚本
├── Dockerfile.crossplatform     # 跨平台Docker文件
├── Dockerfile                   # Windows Docker文件
├── Build_Run_Scripts/           # 构建和运行脚本目录
│   ├── run-wine-macos.sh       # Wine运行脚本
│   ├── run-docker-macos.sh     # Docker运行脚本
│   ├── run_app.sh              # 容器内应用启动脚本
│   └── build-macos.sh          # Docker镜像构建脚本
└── Docs/                       # 文档目录（从原项目复制）
```

## 快速开始

### 方法一：使用Docker（推荐）

1. **安装依赖**：
   ```bash
   chmod +x install-macos.sh
   ./install-macos.sh
   ```

2. **构建并运行**：
   ```bash
   chmod +x Build_Run_Scripts/run-docker-macos.sh
   ./Build_Run_Scripts/run-docker-macos.sh --build
   ```

### 方法二：直接使用Wine

1. **安装依赖**：
   ```bash
   chmod +x install-macos.sh
   ./install-macos.sh --minimal
   ```

2. **运行应用程序**：
   ```bash
   chmod +x Build_Run_Scripts/run-wine-macos.sh
   ./Build_Run_Scripts/run-wine-macos.sh
   ```

## 系统要求

### 硬件要求
- **处理器**: Intel x86_64 或 Apple Silicon (M1/M2/M3) Mac
- **内存**: 至少 4GB RAM，推荐 8GB 以上
- **存储空间**: 至少 2GB 可用空间
- **显卡**: 支持OpenGL 3.3或更高版本

### 软件要求
- **macOS版本**: 10.15 (Catalina) 或更高版本
- **XQuartz**: 2.8.0 或更高版本
- **Wine**: 最新稳定版
- **Docker Desktop** (可选，用于Docker运行方式)

## 运行选项

### Docker运行选项
```bash
./Build_Run_Scripts/run-docker-macos.sh [选项]

选项:
  --build            构建Docker镜像
  --rebuild          强制重新构建Docker镜像
  --no-build         跳过镜像构建
  --debug            启用调试模式
  --remove           运行后删除容器
  --display X        设置X11显示（默认:0）
  --volume PATH      挂载额外数据卷
  -h, --help         显示帮助信息
```

### Wine运行选项
```bash
./Build_Run_Scripts/run-wine-macos.sh [选项]

选项:
  --no-x11-check     跳过X11检查
  --init-wine        强制重新初始化Wine环境
  --debug            启用调试模式
  -h, --help         显示帮助信息
```

## 故障排除

### 常见问题

1. **X11显示问题**
   - 确保XQuartz已安装并正在运行
   - 检查DISPLAY环境变量设置
   - 验证X11授权配置

2. **Wine初始化失败**
   - 使用 `--init-wine` 选项重新初始化
   - 检查Wine版本兼容性
   - 验证Wine前缀配置

3. **Docker容器启动问题**
   - 确保Docker Desktop正在运行
   - 检查端口和权限配置
   - 验证X11挂载设置

4. **性能问题**
   - 增加Docker容器内存限制
   - 关闭不必要的macOS应用程序
   - 使用 `--debug` 选项查看详细日志

### 调试模式

启用调试模式获取详细信息：

```bash
# Docker调试模式
./Build_Run_Scripts/run-docker-macos.sh --debug

# Wine调试模式
./Build_Run_Scripts/run-wine-macos.sh --debug
```

## 性能优化

### Docker优化建议
- 增加容器内存限制到8GB
- 使用本地SSD存储
- 启用Docker Desktop的VirtioFS

### Wine优化建议
- 使用Wine 8.0+稳定版
- 启用macOS系统的硬件加速
- 关闭不必要的macOS视觉效果

## 卸载

### 卸载Docker版本
```bash
docker stop $(docker ps -q --filter "name=airdpro-macos")
docker rm $(docker ps -aq --filter "name=airdpro-macos")
docker rmi airdpro:macos
```

### 卸载Wine版本
```bash
rm -rf ~/.wine
brew uninstall --cask xquartz
brew uninstall wine-stable
```

## 技术支持

### 日志文件位置
- **Wine日志**: `~/.wine/drive_c/users/$USER/AppData/Local/Temp/AirdPro.log`
- **Docker日志**: 使用 `docker logs <container_name>` 查看
- **系统日志**: Console.app > System Report

### 获取帮助
1. 查看 `MacOS-Install-Run.md` 详细文档
2. 运行诊断脚本检查系统环境
3. 收集错误日志和系统信息报告问题

## 版本信息

- **包版本**: v4.2.0
- **目标平台**: macOS 10.15+
- **支持的架构**: x86_64, arm64
- **构建日期**: $(date +'%Y-%m-%d')
- **文档更新**: $(date +'%Y-%m-%d')

## 许可证

本分发包遵循AirdPro原始软件许可证条款。请确保您有合法的软件使用许可。

---

**注意**: 这是一个社区维护的macOS分发包，非官方发布版本。使用前请确保您有合法的软件许可。
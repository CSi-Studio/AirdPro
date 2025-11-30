# Windows到macOS Docker镜像构建指南

## 概述

本指南详细说明如何在Windows环境下为macOS平台构建AirdPro的Docker镜像。通过本指南，您可以在Windows上构建macOS兼容的镜像，然后传输到macOS设备上运行。

## 前提条件

### Windows环境要求
- Windows 10/11 (64位)
- 至少8GB可用内存（推荐16GB）
- 至少20GB可用磁盘空间
- 稳定的网络连接

### 软件依赖
- Docker Desktop for Windows
- WSL 2 (Windows Subsystem for Linux 2)

## 安装步骤

### 1. 安装Docker Desktop

1. **下载Docker Desktop**
   - 访问: https://www.docker.com/products/docker-desktop
   - 下载Windows版本并安装

2. **配置Docker Desktop**
   ```powershell
   # 安装后启动Docker Desktop
   # 确保启用以下设置:
   # - 使用WSL 2后端
   # - Linux容器模式
   # - 分配足够内存(建议8GB+)
   ```

3. **验证安装**
   ```powershell
   # 打开PowerShell，运行以下命令验证安装
   docker --version
   docker info
   ```

### 2. 启用WSL 2（如未启用）

```powershell
# 以管理员身份运行PowerShell，执行以下命令
wsl --install

# 设置WSL 2为默认版本
wsl --set-default-version 2

# 验证WSL状态
wsl --status
```

## 构建macOS兼容镜像

### 方法一：使用自动化脚本（推荐）

1. **运行构建脚本**
   ```cmd
   # 在AirdPro项目根目录下
   build-macos-image.bat
   ```

2. **构建过程说明**
   - 首次构建需要30-60分钟
   - 需要下载Ubuntu基础镜像和Wine依赖
   - 自动安装.NET Framework 4.8
   - 构建三个版本：GUI、CLI、开发版

### 方法二：手动构建

```cmd
# 构建macOS GUI版本
docker build --target runtime-macos -t airdpro:macos .

# 构建CLI版本
docker build --target runtime-macos -t airdpro:cli .

# 构建开发版本
docker build --target runtime-macos -t airdpro:dev .
```

## 镜像导出和传输

### 1. 导出镜像到文件

```cmd
# 使用导出脚本（推荐）
export-macos-images.bat

# 或手动导出
docker save -o airdpro-macos.tar airdpro:macos
docker save -o airdpro-cli.tar airdpro:cli
docker save -o airdpro-dev.tar airdpro:dev
```

### 2. 文件传输到macOS

推荐传输方式：
- **局域网共享**：通过SMB共享文件夹
- **云存储**：Google Drive、Dropbox等
- **USB设备**：使用外接硬盘或U盘
- **网络传输**：scp或rsync命令

### 3. 在macOS上导入镜像

```bash
# 在macOS终端中运行
docker load -i airdpro-macos.tar
docker load -i airdpro-cli.tar
docker load -i airdpro-dev.tar

# 验证导入
docker images | grep airdpro
```

## macOS环境配置

### 1. 安装必要软件

```bash
# 安装Docker Desktop for Mac
# 下载地址: https://www.docker.com/products/docker-desktop

# 安装XQuartz (GUI版本需要)
brew install --cask xquartz
```

### 2. 配置运行环境

```bash
# 确保脚本有执行权限
chmod +x *.sh

# 测试配置
./test-docker-en.sh

# 运行GUI版本
./run-gui-en.sh

# 运行CLI版本
./run-cli-en.sh --help
```

## 故障排除

### 常见问题及解决方案

#### 1. Docker构建失败

**问题**: 构建过程中网络超时或依赖下载失败
**解决方案**:
```cmd
# 清理Docker缓存
docker system prune -f

# 重新构建，使用国内镜像源（如需要）
# 在Dockerfile中替换apt源为国内镜像
```

#### 2. 镜像文件过大

**问题**: 导出的.tar文件体积过大
**解决方案**:
```cmd
# 压缩镜像文件
tar -czf airdpro-macos.tar.gz airdpro-macos.tar

# 或使用分卷压缩
tar -czf - airdpro-macos.tar | split -b 1000M - airdpro-macos.tar.gz.
```

#### 3. macOS导入失败

**问题**: 镜像格式不兼容或损坏
**解决方案**:
```bash
# 验证文件完整性
md5 airdpro-macos.tar

# 重新导出镜像
docker save -o airdpro-macos-v2.tar airdpro:macos
```

#### 4. Wine初始化缓慢

**问题**: 首次运行需要很长时间初始化
**解决方案**:
- 这是正常现象，Wine需要初始化.NET Framework
- 首次运行可能需要10-30分钟
- 后续运行会快很多

## 性能优化建议

### Windows构建优化

1. **增加Docker资源分配**
   - 内存：至少8GB
   - CPU：4核心以上
   - 磁盘：SSD优先

2. **使用构建缓存**
   ```dockerfile
   # 在Dockerfile中合理使用缓存层
   COPY package.json .
   RUN npm install
   COPY . .
   ```

3. **多阶段构建优化**
   ```dockerfile
   # 使用多阶段构建减少最终镜像大小
   FROM build-stage AS runtime
   COPY --from=build-stage /app /app
   ```

### macOS运行优化

1. **Docker资源配置**
   ```bash
   # 在Docker Desktop中分配足够资源
   # 内存：8GB+
   # CPU：4核心+
   ```

2. **网络优化**
   ```bash
   # 使用国内镜像源加速下载
   # 配置Docker镜像加速器
   ```

## 技术架构说明

### 镜像构建原理

1. **多阶段构建**
   - 第一阶段：Windows环境编译.NET应用
   - 第二阶段：Ubuntu + Wine环境运行应用

2. **Wine兼容层**
   - 使用Wine在Linux上运行Windows应用
   - 预装.NET Framework 4.8
   - 配置X11显示支持

3. **跨平台兼容性**
   - 基于Linux容器，可在任何支持Docker的系统运行
   - 使用标准化的运行时环境

### 文件结构

```
AirdPro/
├── build-macos-image.bat      # macOS镜像构建脚本
├── export-macos-images.bat    # 镜像导出脚本
├── build-docker-en.sh          # 英文版构建脚本(macOS)
├── run-gui-en.sh               # 英文版GUI运行脚本
├── run-cli-en.sh               # 英文版CLI运行脚本
├── test-docker-en.sh           # 英文版测试脚本
├── Dockerfile                  # 多阶段Docker构建文件
└── export/                     # 镜像导出目录
    ├── airdpro-macos.tar       # GUI版本镜像
    ├── airdpro-cli.tar         # CLI版本镜像
    └── airdpro-dev.tar         # 开发版本镜像
```

## 支持与反馈

### 获取帮助

如遇到问题，请按以下步骤排查：

1. **检查前提条件**：确保所有依赖已正确安装
2. **查看日志**：运行脚本时查看详细错误信息
3. **验证环境**：使用测试脚本验证环境配置
4. **查阅文档**：参考相关技术文档

### 联系方式

- 项目文档：查看DOCKER-README-EN.md
- 技术问题：查看故障排除章节
- 功能建议：通过项目issue反馈

---

**注意**: 本方案使用Wine运行Windows应用，性能可能不如原生Windows环境。建议在非关键任务环境中测试验证后再投入生产使用。
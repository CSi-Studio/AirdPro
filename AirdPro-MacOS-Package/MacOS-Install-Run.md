# AirdPro macOS 安装和运行指南

## 概述

本包提供了在macOS上运行AirdPro应用程序的完整解决方案。AirdPro是一个.NET Framework 4.8应用程序，通过Wine在macOS上运行。

## 系统要求

- macOS 10.14 (Mojave) 或更高版本
- 至少 4GB 可用内存
- 至少 2GB 可用磁盘空间
- 管理员权限（用于安装依赖）

## 快速开始

### 方法1：使用Docker运行（推荐）

1. 确保已安装Docker Desktop for Mac
   ```bash
   # 如果未安装，请从官网下载安装
   open https://docs.docker.com/desktop/mac/install/
   ```

2. 启动Docker Desktop应用

3. 运行应用程序：
   ```bash
   cd Build_Run_Scripts
   ./run-gui-en.sh
   ```

### 方法2：使用Wine直接运行

1. 安装Wine：
   ```bash
   # 通过Homebrew安装
   brew install wine-stable
   
   # 或者手动下载安装
   open https://winehq.org/
   ```

2. 运行应用程序：
   ```bash
   cd Build_Run_Scripts
   ./run-wine-macos.sh
   ```

## 详细安装步骤

### 安装XQuartz（GUI支持必需）

1. 安装XQuartz：
   ```bash
   brew install --cask xquartz
   ```

2. 重启系统或注销登录

3. 启动XQuartz：
   ```bash
   open -a XQuartz
   ```

4. 在XQuartz中启用"允许来自网络客户端的连接"：
   - 打开XQuartz偏好设置
   - 转到"安全"标签
   - 勾选"允许来自网络客户端的连接"

### 构建Docker镜像

如果需要自定义镜像：

1. 构建镜像：
   ```bash
   cd Build_Run_Scripts
   ./build-docker-en.sh
   ```

2. 或者只构建macOS版本：
   ```bash
   docker build --target runtime-linux -t airdpro:macos -f ../Dockerfile.crossplatform ..
   ```

## 运行选项

### GUI模式（默认）
```bash
./run-gui-en.sh
```

### CLI模式
```bash
./run-gui-en.sh --cli
```

### 开发模式
```bash
./run-gui-en.sh --dev
```

## 故障排除

### 问题1：Docker镜像拉取失败
**解决方案：** 检查网络连接，或使用离线构建

### 问题2：X11显示问题
**解决方案：**
1. 确保XQuartz正在运行
2. 设置DISPLAY环境变量：
   ```bash
   export DISPLAY=:0
   ```

### 问题3：Wine权限问题
**解决方案：**
```bash
sudo chown -R $(whoami) ~/.wine
```

### 问题4：应用程序启动缓慢
**解决方案：**
- 这是正常现象，首次启动需要初始化Wine环境
- 后续启动会较快

## 性能优化

1. **调整Wine配置：**
   ```bash
   winecfg
   ```

2. **设置共享内存：**
   ```bash
   export WINEDLLOVERRIDES="mscoree=d"
   ```

3. **启用硬件加速（如果支持）：**
   ```bash
   export WINEDEBUG=-all
   export LIBGL_ALWAYS_INDIRECT=0
   ```

## 数据目录

应用程序数据存储在以下位置：
- Linux/macOS: `~/AirdPro_Data/`
- Docker容器: `/data/`

## 日志位置

- 应用程序日志: `~/AirdPro_Data/Logs/`
- Wine日志: `~/.wine/drive_c/users/$(whoami)/AppData/Local/AirdPro/`

## 卸载

### 卸载Docker版本：
```bash
docker rmi airdpro:macos airdpro:linux
```

### 卸载Wine版本：
```bash
rm -rf ~/.wine
rm -rf ~/AirdPro_Data
```

## 技术支持

如果遇到问题：

1. 查看日志文件获取详细错误信息
2. 尝试使用CLI模式运行以获得更多调试信息
3. 检查系统兼容性要求

## 版本信息

- AirdPro 版本：4.2.0
- 支持平台：macOS 10.14+
- Docker支持：是
- Wine支持：是

## 许可证

请参阅项目根目录的LICENSE文件了解详细信息。
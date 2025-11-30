# 快速开始指南：Windows到macOS Docker镜像构建

## 第一步：安装Docker Desktop

1. **下载Docker Desktop**
   - 访问: https://www.docker.com/products/docker-desktop
   - 下载Windows版本并安装

2. **安装后配置**
   - 启动Docker Desktop
   - 确保选择"使用WSL 2后端"
   - 切换到Linux容器模式
   - 分配至少8GB内存给Docker

3. **验证安装**
   ```cmd
   # 打开命令提示符，运行以下命令
   docker --version
   docker info
   ```

## 第二步：构建macOS镜像

1. **运行构建脚本**
   ```cmd
   # 在AirdPro项目根目录下
   build-macos-image.bat
   ```

2. **构建过程**
   - 首次构建需要30-60分钟
   - 自动下载Ubuntu基础镜像和Wine依赖
   - 构建三个版本：GUI、CLI、开发版

## 第三步：导出镜像

1. **使用导出脚本**
   ```cmd
   # 自动导出所有镜像
   export-macos-images.bat
   ```

2. **手动导出（可选）**
   ```cmd
   docker save -o airdpro-macos.tar airdpro:macos
   docker save -o airdpro-cli.tar airdpro:cli
   docker save -o airdpro-dev.tar airdpro:dev
   ```

## 第四步：传输到macOS

1. **传输方式选择**
   - USB设备：复制.tar文件到U盘
   - 网络共享：通过局域网传输
   - 云存储：上传到Google Drive等

2. **在macOS上导入**
   ```bash
   # 在macOS终端中
   docker load -i airdpro-macos.tar
   docker load -i airdpro-cli.tar
   docker load -i airdpro-dev.tar
   ```

## 第五步：在macOS上运行

1. **安装必要软件**
   ```bash
   # 安装Docker Desktop for Mac
   # 安装XQuartz (GUI版本需要)
   brew install --cask xquartz
   ```

2. **运行应用**
   ```bash
   # 测试环境
   ./test-docker-en.sh
   
   # 运行GUI版本
   ./run-gui-en.sh
   
   # 运行CLI版本
   ./run-cli-en.sh --help
   ```

## 故障排除

### Docker安装问题
- 确保Windows版本支持WSL 2 (Windows 10 2004+)
- 启用虚拟化功能（BIOS设置）
- 以管理员身份运行安装程序

### 构建失败
- 检查网络连接
- 增加Docker内存分配（8GB+）
- 清理Docker缓存：`docker system prune -f`

### macOS导入问题
- 验证文件完整性：`md5 airdpro-macos.tar`
- 确保macOS上Docker Desktop正常运行

## 支持文件

- **详细指南**: WINDOWS-TO-MACOS-GUIDE.md
- **构建脚本**: build-macos-image.bat
- **导出脚本**: export-macos-images.bat
- **英文版运行脚本**: *.sh 和 *.bat 文件

## 预计时间

- Docker安装：15-30分钟
- 镜像构建：30-60分钟（首次）
- 镜像导出：5-10分钟
- 传输时间：取决于文件大小和网络速度

---

**注意**: 首次在macOS上运行需要较长时间初始化Wine环境（10-30分钟），这是正常现象。
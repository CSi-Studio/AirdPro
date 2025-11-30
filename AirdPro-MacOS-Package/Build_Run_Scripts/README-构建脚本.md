# 构建运行脚本目录

本目录包含AirdPro项目的所有构建和运行脚本，支持跨平台Docker镜像构建和容器运行。

## 脚本列表

### 🔧 构建脚本

#### build-docker-en.bat (Windows)
```batch
# Windows环境下构建Docker镜像
build-docker-en.bat

# 构建特定目标
build-docker-en.bat --target=runtime-linux
build-docker-en.bat --target=runtime-cli
```

#### build-docker-en.sh (Linux/macOS)
```bash
# Linux/macOS环境下构建Docker镜像
./build-docker-en.sh

# 平台特定构建
./build-docker-en.sh --linux-only
./build-docker-en.sh --macos-only

# 查看所有选项
./build-docker-en.sh --help
```

### 🚀 运行脚本

#### run-gui-en.sh
```bash
# GUI模式运行（跨平台）
./run-gui-en.sh

# 平台特定运行
./run-gui-en.sh --linux-only
./run-gui-en.sh --macos-only

# 带参数运行
./run-gui-en.sh --data-path ./data --log-level debug
```

#### run-cli-en.sh
```bash
# CLI模式运行（跨平台）
./run-cli-en.sh

# 交互式模式
./run-cli-en.sh --interactive

# 批处理模式
./run-cli-en.sh --batch --input-file /data/analysis.csv
```

## 支持的构建目标

### Docker镜像变体
1. **runtime-linux** - Linux/macOS兼容版本（Wine）
2. **runtime-cli** - 命令行版本（轻量级）
3. **runtime-dev** - 开发版本（包含开发工具）

### 平台兼容性
- ✅ **Windows**: 使用 .bat 脚本
- ✅ **Linux**: 使用 .sh 脚本
- ✅ **macOS**: 使用 .sh 脚本（需要Docker Desktop + XQuartz）

## 使用流程

### 1. 构建镜像
```bash
# Windows用户
build-docker-en.bat

# Linux/macOS用户
./build-docker-en.sh
```

### 2. 运行应用
```bash
# GUI模式
./run-gui-en.sh

# CLI模式
./run-cli-en.sh
```

## 脚本功能特性

### 构建脚本特性
- ✅ 自动检测Docker环境
- ✅ 多阶段构建优化
- ✅ 平台特定优化
- ✅ 构建进度显示
- ✅ 错误处理和重试

### 运行脚本特性
- ✅ 自动卷挂载管理
- ✅ 环境变量配置
- ✅ X11转发支持（GUI模式）
- ✅ 数据持久化
- ✅ 日志管理

## 文件结构

```
Build_Run_Scripts/
├── README-构建脚本.md        # 本索引文件
├── build-docker-en.bat       # Windows构建脚本
├── build-docker-en.sh        # Linux/macOS构建脚本
├── run-gui-en.sh            # GUI运行脚本
└── run-cli-en.sh            # CLI运行脚本
```

## 权限设置

使用前请确保脚本具有执行权限：

```bash
# Linux/macOS
chmod +x *.sh

# Windows
# .bat文件默认具有执行权限
```

## 依赖要求

### 构建依赖
- **Docker**: 20.10+ 
- **Docker Compose**: 2.0+ (可选)

### 运行依赖
- **Docker**: 运行时环境
- **XQuartz**: macOS GUI支持
- **X11**: Linux GUI支持

## 故障排除

### 常见问题
1. **权限错误**: 确保脚本有执行权限
2. **Docker未运行**: 检查Docker服务状态
3. **网络问题**: 检查镜像拉取和网络配置
4. **内存不足**: 增加Docker内存分配

### 获取帮助
```bash
# 查看构建脚本帮助
./build-docker-en.sh --help

# 查看运行脚本帮助
./run-gui-en.sh --help
./run-cli-en.sh --help
```

## 版本兼容性

- **Docker版本**: 20.10+
- **脚本版本**: 1.0.0
- **支持平台**: Windows 10+, macOS 10.15+, Ubuntu 18.04+

---

**使用建议**: 首次使用时，建议阅读相应的中文或英文文档获取详细说明。
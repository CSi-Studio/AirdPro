# AirdPro Documentation Hub

欢迎使用AirdPro项目文档中心。本文件夹包含项目的所有文档，按语言分类组织。

## 📁 文档结构

```
Docs/
├── README.md                    # 本索引文件
├── Chinese_Docs/                # 中文文档
│   ├── README-中文文档.md         # 中文文档索引
│   ├── 打包部署指南-CN.md         # 完整打包部署指南
│   ├── Mac平台部署启动说明.md       # macOS平台指南
│   ├── Linux平台部署启动说明.md     # Linux平台指南
│   └── DOCKER-README.md         # Docker使用说明
├── English_Docs/               # 英文文档
│   ├── README-English-Docs.md   # English docs index
│   ├── PACKAGING-GUIDE-EN.md    # Complete packaging guide
│   └── DOCKER-README-EN.md      # Docker usage documentation
└── [Additional Documentation]  # 其他文档将在这里添加
```

## 🌍 快速开始指南

### 中文用户
- **首次使用**: 请先阅读 `Chinese_Docs/打包部署指南-CN.md`
- **macOS用户**: 查看 `Chinese_Docs/Mac平台部署启动说明.md`
- **Linux用户**: 查看 `Chinese_Docs/Linux平台部署启动说明.md`
- **Docker相关**: 查看 `Chinese_Docs/DOCKER-README.md`

### English Users
- **First-time users**: Start with `English_Docs/PACKAGING-GUIDE-EN.md`
- **Docker operations**: See `English_Docs/DOCKER-README-EN.md`
- **Platform-specific guides**: Refer to sections in the main packaging guide

## 📋 文档分类

### 🔧 打包部署文档 (Packaging & Deployment)
- **Chinese**: `打包部署指南-CN.md` - 完整的打包和部署指南
- **English**: `PACKAGING-GUIDE-EN.md` - Complete packaging guide

### 🍎 macOS平台文档 (macOS Platform)
- **Chinese**: `Mac平台部署启动说明.md` - macOS专用部署指南
- **English**: 包含在主打包指南的macOS章节中

### 🐧 Linux平台文档 (Linux Platform)
- **Chinese**: `Linux平台部署启动说明.md` - Linux专用部署指南
- **English**: 包含在主打包指南的Linux章节中

### 🐳 Docker文档 (Docker Documentation)
- **Chinese**: `DOCKER-README.md` - Docker使用说明
- **English**: `DOCKER-README-EN.md` - Docker usage guide

## 🚀 常用操作

### 构建和运行脚本
- 所有构建和运行脚本位于项目根目录的 `Build_Run_Scripts` 文件夹
- 详细使用说明请参考 `Build_Run_Scripts/README-构建脚本.md`

### 平台支持
- ✅ **Windows 10+** - 完全支持
- ✅ **macOS 10.15+** - 支持，需要Docker Desktop + XQuartz
- ✅ **Linux (Ubuntu 18.04+)** - 完全支持

## 📞 技术支持

### 获取帮助
1. **文档查找**: 使用搜索功能查找相关问题
2. **常见问题**: 查看各文档中的故障排除章节
3. **运行时问题**: 检查Docker日志和环境配置

### 性能优化
- 根据您的平台选择对应的优化建议
- 调整Docker资源分配
- 使用适当的镜像变体（runtime-linux, runtime-cli等）

## 🔄 版本信息

- **当前版本**: AirdPro v1.0
- **文档版本**: 2024年12月
- **支持平台**: Windows, macOS, Linux
- **Docker版本**: 20.10+

---

**使用建议**: 建议首次用户从对应的语言文档开始，根据您的操作系统选择相应的指南进行操作。
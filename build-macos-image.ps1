# PowerShell脚本构建macOS兼容的Docker镜像
Write-Host "开始构建macOS兼容的Docker镜像..." -ForegroundColor Green

# 检查Docker是否运行
try {
    $dockerInfo = docker info 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "错误: Docker服务未运行，请先启动Docker Desktop" -ForegroundColor Red
        exit 1
    }
    Write-Host "✓ Docker服务正常运行" -ForegroundColor Green
} catch {
    Write-Host "错误: 无法连接到Docker服务" -ForegroundColor Red
    exit 1
}

# 检查当前容器模式
$dockerVersion = docker version --format "{{.Server.Os}}" 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "当前容器模式: $dockerVersion" -ForegroundColor Yellow
    
    if ($dockerVersion -eq "linux") {
        Write-Host "警告: 当前为Linux容器模式，需要切换到Windows容器模式" -ForegroundColor Yellow
        Write-Host "请按以下步骤操作:" -ForegroundColor Yellow
        Write-Host "1. 打开Docker Desktop应用程序" -ForegroundColor Yellow
        Write-Host "2. 在界面中查找Switch to Windows containers按钮" -ForegroundColor Yellow
        Write-Host "3. 切换后重新运行此脚本" -ForegroundColor Yellow
        exit 1
    }
}

# 构建macOS基础镜像
Write-Host "开始构建macOS基础镜像 (runtime-macos)..." -ForegroundColor Cyan
docker build --target runtime-macos -t airdpro:macos .

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ macOS基础镜像构建成功" -ForegroundColor Green
    
    # 构建CLI版本
    Write-Host "开始构建CLI版本镜像..." -ForegroundColor Cyan
    docker build --target cli -t airdpro:cli .
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ CLI版本镜像构建成功" -ForegroundColor Green
    } else {
        Write-Host "⚠ CLI版本镜像构建失败，但基础镜像已成功" -ForegroundColor Yellow
    }
    
    # 构建开发版本
    Write-Host "开始构建开发版本镜像..." -ForegroundColor Cyan
    docker build --target dev -t airdpro:dev .
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ 开发版本镜像构建成功" -ForegroundColor Green
    } else {
        Write-Host "⚠ 开发版本镜像构建失败，但基础镜像已成功" -ForegroundColor Yellow
    }
    
    Write-Host "`n构建完成!" -ForegroundColor Green
    Write-Host "可用镜像:" -ForegroundColor Green
    docker images airdpro:*
    
    Write-Host "`n下一步操作:" -ForegroundColor Cyan
    Write-Host "1. 运行 .\export-macos-images.ps1 导出镜像" -ForegroundColor Cyan
    Write-Host "2. 将导出的镜像文件传输到macOS" -ForegroundColor Cyan
    Write-Host "3. 在macOS上导入并运行镜像" -ForegroundColor Cyan
    
} else {
    Write-Host "错误: 镜像构建失败" -ForegroundColor Red
    Write-Host "可能的原因:" -ForegroundColor Red
    Write-Host "- 需要Windows容器模式" -ForegroundColor Red
    Write-Host "- 网络连接问题" -ForegroundColor Red
    Write-Host "- Docker Desktop配置问题" -ForegroundColor Red
    exit 1
}
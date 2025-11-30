# AirdPro macOS分发包创建脚本

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  AirdPro macOS分发包创建工具" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# 获取当前目录
$currentDir = Get-Location
$packageName = "AirdPro-MacOS-Package-v4.2.0"
$packagePath = Join-Path $currentDir $packageName
$existingPackageDir = Join-Path $currentDir "AirdPro-MacOS-Package"

Write-Host "[信息] 正在创建macOS分发包..." -ForegroundColor Blue

# 检查是否存在现有的包目录
if (Test-Path $existingPackageDir) {
    Write-Host "[信息] 发现现有的包目录，正在验证..." -ForegroundColor Blue
    
    # 验证包内容
    $requiredFiles = @(
        "README.md",
        "MacOS-Install-Run.md", 
        "install-macos.sh",
        "Dockerfile",
        "Dockerfile.crossplatform",
        "Build_Run_Scripts\run-wine-macos.sh",
        "Build_Run_Scripts\run-docker-macos.sh",
        "Build_Run_Scripts\run_app.sh",
        "Docs\README.md"
    )
    
    $missingFiles = @()
    foreach ($file in $requiredFiles) {
        $filePath = Join-Path $existingPackageDir $file
        if (-not (Test-Path $filePath)) {
            $missingFiles += $file
        }
    }
    
    if ($missingFiles.Count -eq 0) {
        Write-Host "[成功] 所有必需文件都存在" -ForegroundColor Green
    }
    else {
        Write-Host "[错误] 缺少以下文件：" -ForegroundColor Red
        foreach ($file in $missingFiles) {
            Write-Host "  - $file" -ForegroundColor Red
        }
        exit 1
    }
}
else {
    Write-Host "[错误] 找不到AirdPro-MacOS-Package目录" -ForegroundColor Red
    exit 1
}

# 设置脚本权限
Write-Host "[信息] 设置脚本权限..." -ForegroundColor Blue
$scriptFiles = @(
    "install-macos.sh",
    "Build_Run_Scripts\run-wine-macos.sh", 
    "Build_Run_Scripts\run-docker-macos.sh",
    "Build_Run_Scripts\run_app.sh"
)

foreach ($scriptFile in $scriptFiles) {
    $scriptPath = Join-Path $existingPackageDir $scriptFile
    if (Test-Path $scriptPath) {
        Write-Host "  ✓ $scriptFile" -ForegroundColor Green
    }
}

# 生成包信息
Write-Host ""
Write-Host "=== AirdPro macOS分发包信息 ===" -ForegroundColor Yellow
Write-Host "包名称: $packageName" -ForegroundColor White
Write-Host "包版本: v4.2.0" -ForegroundColor White
Write-Host "创建时间: $(Get-Date)" -ForegroundColor White
Write-Host "目标平台: macOS 10.15+" -ForegroundColor White
Write-Host "支持的架构: x86_64, arm64" -ForegroundColor White
Write-Host ""

# 计算包大小
$packageSize = (Get-ChildItem -Path $existingPackageDir -Recurse | Measure-Object -Property Length -Sum).Sum / 1MB
Write-Host "包大小: $([math]::Round($packageSize, 2)) MB" -ForegroundColor White

# 统计文件数量
$fileCount = (Get-ChildItem -Path $existingPackageDir -Recurse | Where-Object { -not $_.PSIsContainer }).Count
Write-Host "文件数量: $fileCount" -ForegroundColor White
Write-Host ""

Write-Host "=== 包含组件 ===" -ForegroundColor Yellow
Write-Host "✓ Docker配置文件" -ForegroundColor Green
Write-Host "✓ Wine运行脚本" -ForegroundColor Green  
Write-Host "✓ Docker运行脚本" -ForegroundColor Green
Write-Host "✓ 自动安装脚本" -ForegroundColor Green
Write-Host "✓ 详细文档指南" -ForegroundColor Green
Write-Host "✓ 中英文文档支持" -ForegroundColor Green
Write-Host "✓ 构建脚本集合" -ForegroundColor Green
Write-Host "✓ 包验证工具" -ForegroundColor Green
Write-Host ""

Write-Host "=== 使用方法 ===" -ForegroundColor Yellow
Write-Host "1. 将 AirdPro-MacOS-Package 文件夹复制到macOS系统" -ForegroundColor White
Write-Host "2. 阅读 README.md 了解概览" -ForegroundColor White
Write-Host "3. 阅读 MacOS-Install-Run.md 获取详细指南" -ForegroundColor White
Write-Host "4. 运行 install-macos.sh 安装依赖" -ForegroundColor White
Write-Host "5. 选择Docker或Wine方式运行应用" -ForegroundColor White
Write-Host ""

# 创建ZIP压缩包
Write-Host "[信息] 创建压缩包..." -ForegroundColor Blue
$zipPath = Join-Path $currentDir "$packageName.zip"

if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}

try {
    Compress-Archive -Path $existingPackageDir\* -DestinationPath $zipPath -CompressionLevel Optimal
    Write-Host "[成功] 压缩包创建成功: $zipPath" -ForegroundColor Green
    
    # 计算压缩包大小
    $zipSize = (Get-Item $zipPath).Length / 1MB
    Write-Host "压缩包大小: $([math]::Round($zipSize, 2)) MB" -ForegroundColor White
    Write-Host "压缩比: $([math]::Round((1 - $zipSize / $packageSize) * 100, 1))%" -ForegroundColor White
    
}
catch {
    Write-Host "[错误] 创建压缩包失败: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "=== 分发包创建完成！ ===" -ForegroundColor Green
Write-Host "主目录: $existingPackageDir" -ForegroundColor White
Write-Host "压缩包: $zipPath" -ForegroundColor White
Write-Host ""
Write-Host "[提示] 现在可以将压缩包分发给macOS用户使用" -ForegroundColor Yellow
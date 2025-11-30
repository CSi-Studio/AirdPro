# PowerShell脚本导出macOS镜像
Write-Host "开始导出macOS兼容的Docker镜像..." -ForegroundColor Green

# 检查镜像是否存在
$images = docker images airdpro:* --format "table {{.Repository}}:{{.Tag}}"
if ($images.Count -le 1) {
    Write-Host "错误: 未找到airdpro镜像，请先运行build-macos-image.ps1构建镜像" -ForegroundColor Red
    exit 1
}

Write-Host "找到以下镜像:" -ForegroundColor Cyan
docker images airdpro:*

# 创建导出目录
$exportDir = "docker-images-macos"
if (-not (Test-Path $exportDir)) {
    New-Item -ItemType Directory -Path $exportDir | Out-Null
    Write-Host "✓ 创建导出目录: $exportDir" -ForegroundColor Green
}

# 导出镜像
$imagesToExport = @("airdpro:macos", "airdpro:cli", "airdpro:dev")

foreach ($image in $imagesToExport) {
    $imageExists = docker images $image --format "{{.Repository}}:{{.Tag}}" 2>&1
    if ($imageExists -and $imageExists -ne "") {
        $fileName = $image.Replace(":", "-") + ".tar"
        $filePath = Join-Path $exportDir $fileName
        
        Write-Host "正在导出 $image 到 $filePath..." -ForegroundColor Cyan
        docker save $image -o $filePath
        
        if ($LASTEXITCODE -eq 0) {
            $fileSize = [math]::Round((Get-Item $filePath).Length / 1MB, 2)
            Write-Host "✓ 导出成功 ($fileSize MB)" -ForegroundColor Green
        } else {
            Write-Host "⚠ 导出失败: $image" -ForegroundColor Yellow
        }
    } else {
        Write-Host "⚠ 镜像不存在: $image" -ForegroundColor Yellow
    }
}

# 显示导出结果
Write-Host "`n导出完成!" -ForegroundColor Green
Write-Host "导出的镜像文件:" -ForegroundColor Cyan
Get-ChildItem $exportDir -Filter "*.tar" | Format-Table Name, Length -AutoSize

Write-Host "`n下一步操作:" -ForegroundColor Cyan
Write-Host "1. 将 $exportDir 文件夹复制到macOS" -ForegroundColor Cyan
Write-Host "2. 在macOS上运行以下命令导入镜像:" -ForegroundColor Cyan
Write-Host "   docker load -i airdpro-macos.tar" -ForegroundColor Cyan
Write-Host "3. 运行镜像: docker run -it airdpro:macos" -ForegroundColor Cyan

Write-Host "`n导出目录位置: $(Resolve-Path $exportDir)" -ForegroundColor Yellow
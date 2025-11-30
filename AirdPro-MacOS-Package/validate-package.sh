#!/bin/bash

# AirdPro macOS分发包验证脚本
# AirdPro macOS package validation script

set -e

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

print_info() {
    echo -e "${BLUE}[信息]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[成功]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[警告]${NC} $1"
}

print_error() {
    echo -e "${RED}[错误]${NC} $1"
}

# 验证包结构
validate_package_structure() {
    print_info "验证包结构..."
    
    local required_files=(
        "README.md"
        "MacOS-Install-Run.md"
        "install-macos.sh"
        "Dockerfile"
        "Dockerfile.crossplatform"
        "Build_Run_Scripts/run-wine-macos.sh"
        "Build_Run_Scripts/run-docker-macos.sh"
        "Build_Run_Scripts/run_app.sh"
        "Docs/README.md"
    )
    
    local missing_files=()
    
    for file in "${required_files[@]}"; do
        if [ ! -e "$file" ]; then
            missing_files+=("$file")
        fi
    done
    
    if [ ${#missing_files[@]} -eq 0 ]; then
        print_success "所有必需文件都存在"
        return 0
    else
        print_error "缺少以下文件："
        for file in "${missing_files[@]}"; do
            echo "  - $file"
        done
        return 1
    fi
}

# 验证文件权限
validate_file_permissions() {
    print_info "验证文件权限..."
    
    local script_files=(
        "install-macos.sh"
        "Build_Run_Scripts/run-wine-macos.sh"
        "Build_Run_Scripts/run-docker-macos.sh"
        "Build_Run_Scripts/run_app.sh"
    )
    
    local permission_errors=0
    
    for file in "${script_files[@]}"; do
        if [ -f "$file" ]; then
            if [ ! -x "$file" ]; then
                print_warning "文件 $file 没有执行权限"
                permission_errors=$((permission_errors + 1))
            fi
        fi
    done
    
    if [ $permission_errors -eq 0 ]; then
        print_success "所有脚本文件都有正确的执行权限"
        return 0
    else
        print_error "$permission_errors 个文件权限错误"
        return 1
    fi
}

# 验证脚本语法
validate_script_syntax() {
    print_info "验证脚本语法..."
    
    local script_files=(
        "install-macos.sh"
        "Build_Run_Scripts/run-wine-macos.sh"
        "Build_Run_Scripts/run-docker-macos.sh"
        "Build_Run_Scripts/run_app.sh"
    )
    
    local syntax_errors=0
    
    for file in "${script_files[@]}"; do
        if [ -f "$file" ]; then
            if ! bash -n "$file"; then
                print_error "脚本 $file 语法错误"
                syntax_errors=$((syntax_errors + 1))
            fi
        fi
    done
    
    if [ $syntax_errors -eq 0 ]; then
        print_success "所有脚本语法检查通过"
        return 0
    else
        print_error "$syntax_errors 个脚本存在语法错误"
        return 1
    fi
}

# 验证文档完整性
validate_documentation() {
    print_info "验证文档完整性..."
    
    local doc_files=(
        "README.md"
        "MacOS-Install-Run.md"
        "Docs/README.md"
    )
    
    local empty_docs=()
    
    for file in "${doc_files[@]}"; do
        if [ -f "$file" ]; then
            if [ ! -s "$file" ]; then
                empty_docs+=("$file")
            fi
        fi
    done
    
    if [ ${#empty_docs[@]} -eq 0 ]; then
        print_success "所有文档文件都有内容"
        return 0
    else
        print_error "以下文档文件为空："
        for file in "${empty_docs[@]}"; do
            echo "  - $file"
        done
        return 1
    fi
}

# 生成包信息
generate_package_info() {
    print_info "生成包信息..."
    
    local package_size=$(du -sh . | cut -f1)
    local file_count=$(find . -type f | wc -l)
    local total_size=$(du -sh . | cut -f1)
    
    echo "=== AirdPro macOS分发包信息 ==="
    echo "包大小: $package_size"
    echo "文件数量: $file_count"
    echo "创建时间: $(date)"
    echo "包版本: v4.2.0"
    echo "目标平台: macOS 10.15+"
    echo "支持的架构: x86_64, arm64"
    echo ""
    
    echo "=== 包含组件 ==="
    echo "✓ Docker配置文件"
    echo "✓ Wine运行脚本"
    echo "✓ Docker运行脚本"
    echo "✓ 自动安装脚本"
    echo "✓ 详细文档指南"
    echo "✓ 中英文文档支持"
    echo "✓ 构建脚本集合"
    echo ""
    
    echo "=== 使用方法 ==="
    echo "1. 阅读 README.md 了解概览"
    echo "2. 阅读 MacOS-Install-Run.md 获取详细指南"
    echo "3. 运行 install-macos.sh 安装依赖"
    echo "4. 选择Docker或Wine方式运行应用"
    echo ""
}

# 主验证函数
main() {
    echo "============================================"
    echo "  AirdPro macOS分发包验证工具"
    echo "============================================"
    echo ""
    
    local validation_passed=true
    
    # 执行所有验证
    validate_package_structure || validation_passed=false
    echo ""
    
    validate_file_permissions || validation_passed=false
    echo ""
    
    validate_script_syntax || validation_passed=false
    echo ""
    
    validate_documentation || validation_passed=false
    echo ""
    
    generate_package_info
    
    if [ "$validation_passed" = true ]; then
        print_success "包验证通过！分发包可以正常使用。"
        echo ""
        print_info "下一步："
        echo "1. 将整个 AirdPro-MacOS-Package 文件夹复制到macOS系统"
        echo "2. 按照README.md中的说明进行安装和配置"
        echo "3. 选择Docker或Wine方式运行AirdPro应用"
    else
        print_error "包验证失败！请检查上述错误并修复。"
        exit 1
    fi
}

# 运行验证
main "$@"
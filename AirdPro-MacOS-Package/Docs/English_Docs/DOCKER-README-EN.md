# AirdPro Docker Deployment Guide (macOS + Wine)

## Project Overview

AirdPro is a Windows desktop application based on .NET Framework 4.8, featuring both GUI interface and command-line functionality. Since macOS does not natively support .NET Framework, this project enables AirdPro to run on macOS through Docker + Wine technology.

**Important Note**: This solution uses Wine to run Windows applications in Linux containers. Performance may not match native Windows environments, and the first run requires longer initialization time.

## Prerequisites

### macOS Environment Requirements
- macOS 10.15+ (Catalina or later)
- Docker Desktop for Mac (version 20.10+)
- XQuartz (for GUI display support)
- Minimum 8GB available RAM (16GB recommended)
- Stable internet connection (required for downloading Wine and .NET Framework on first run)

### Hardware Requirements
- Intel or Apple Silicon Mac
- Minimum 20GB available disk space
- CPU with virtualization support

## Quick Start

### 1. Install Dependencies

#### Install Docker Desktop
```bash
# Download and install Docker Desktop for Mac
# Download URL: https://www.docker.com/products/docker-desktop

# Start Docker Desktop after installation
open -a "Docker Desktop"
```

#### Install XQuartz (Required for GUI version)
```bash
# Install XQuartz using Homebrew
brew install --cask xquartz

# Start XQuartz
open -a XQuartz

# Configure XQuartz (required for first run)
# 1. Open XQuartz Preferences
# 2. Check "Allow connections from network clients" in Security tab
# 3. Restart XQuartz
```

### 2. Build Docker Images

```bash
# Build all images (first build may take longer)
./build-docker.sh

# Build process explanation:
# - Download Ubuntu base image
# - Install Wine and dependencies
# - Initialize Wine environment
# - Install .NET Framework 4.8
# - Compile AirdPro application
```

### 3. Test Configuration

```bash
# Run configuration test
./test-docker.sh
```

### 4. Run Application

#### GUI Version (Recommended for daily use)
```bash
./run-gui.sh
```

#### CLI Version (For batch processing tasks)
```bash
# Display help
./run-cli.sh --help

# File conversion example
./run-cli.sh -i data/input.raw -o data/output.mzML
```

## Project Structure

```
AirdPro/
├── Dockerfile              # Multi-stage Docker build configuration
├── docker-compose.yml      # Docker Compose configuration
├── build-docker.sh         # macOS/Linux build script
├── run-gui.sh              # GUI version run script
├── run-cli.sh              # Command-line version run script
├── build-docker.bat        # Windows build script
├── run-windows.bat         # Windows run script
├── data/                   # Data directory (auto-created)
├── logs/                   # Logs directory (auto-created)
└── DOCKER-README.md       # This document (Chinese version)
└── DOCKER-README-EN.md    # English version documentation
```

## Technical Architecture

### Wine + Docker Solution

This project uses the following technology stack to run .NET Framework applications on macOS:

1. **Docker Containers**: Provide isolated Linux runtime environment
2. **Wine**: Windows application compatibility layer
3. **XQuartz**: X11 server for macOS, used for GUI display
4. **.NET Framework 4.8**: Installed and run through Wine

### Multi-stage Build Strategy

Dockerfile uses an optimized build strategy:

1. **Build Stage**: Compile application using .NET Framework SDK
2. **macOS Runtime**: Based on Ubuntu 22.04, pre-configured with Wine and .NET Framework

## Advanced Usage

### Using Docker Compose

```bash
# Build all services
docker-compose build

# Run GUI service
docker-compose up airdpro-macos

# Run CLI service
docker-compose run airdpro-cli --help

# Development mode (mount source code)
docker-compose up airdpro-dev
```

### Custom Wine Configuration

Customize Wine behavior using environment variables:

```bash
# Enable Wine debugging (view detailed logs)
export WINEDEBUG=fixme-all
./run-gui.sh

# Use different Wine prefix
export WINEPREFIX=/custom-wine
./run-gui.sh
```

### Performance Optimization Configuration

```yaml
# Adjust resource limits in docker-compose.yml
services:
  airdpro-macos:
    deploy:
      resources:
        limits:
          memory: 8G
          cpus: '4.0'
```

## Troubleshooting

### Common Issues and Solutions

#### 1. **Long First Run Time**
**Problem**: First run takes more than 30 minutes
**Cause**: Wine needs to initialize and download .NET Framework components
**Solution**: Be patient and ensure stable network connection

#### 2. **GUI Not Displaying**
**Problem**: Application starts but no interface appears
**Check Steps**:
```bash
# Check if XQuartz is running
ps aux | grep -i xquartz

# Check X11 connection permissions
xhost

# Reconfigure XQuartz
open -a XQuartz
# Enable network client connections in Preferences
```

#### 3. **Insufficient Memory**
**Problem**: Container terminated due to insufficient memory
**Solution**:
- Allocate more memory to Docker (8GB+ recommended)
- Close other memory-intensive applications
- Use CLI version to reduce memory usage

#### 4. **Wine Initialization Failure**
**Problem**: Wine configuration error or .NET Framework installation failure
**Solution**:
```bash
# Rebuild images (clear cache)
docker system prune -a
./build-docker.sh

# Check Wine status
docker run --rm airdpro:macos wine --version
```

### Log Viewing and Debugging

#### Application Logs
```bash
# View application logs
tail -f logs/airdpro.log

# View Wine logs
docker run --rm airdpro:macos wine debuglog
```

#### Docker Container Logs
```bash
# View container runtime logs
docker logs airdpro-gui

# View logs in real-time
docker logs -f airdpro-gui
```

#### System-level Debugging
```bash
# Check container resource usage
docker stats airdpro-gui

# Enter container for debugging
docker exec -it airdpro-gui bash
```

## Usage Examples

### Basic File Operations

```bash
# Check version information
./run-cli.sh --version

# File format conversion
./run-cli.sh -i data/sample.raw -o data/sample.mzML

# Batch processing
./run-cli.sh -i data/input_folder -o data/output_folder --recursive
```

### Advanced Features

```bash
# Run with specific parameters
./run-cli.sh --config custom_config.xml --threads 4

# Generate detailed logs
./run-cli.sh --verbose --log-level debug
```

## Performance Optimization Recommendations

### System-level Optimization

1. **Memory Allocation**: Allocate at least 8GB memory to Docker
2. **CPU Cores**: Allocate 4 or more CPU cores
3. **Storage Performance**: Use SSD storage, avoid network storage
4. **Network Optimization**: Use wired network connection

### Application Optimization

1. **Use CLI Mode**: Use CLI version for batch processing to reduce resource usage
2. **Batch Processing**: Process large files in batches to avoid memory overflow
3. **Regular Cleanup**: Regularly clean temporary files and logs

## Maintenance and Updates

### Routine Maintenance

```bash
# Clean Docker system resources
docker system prune -f

# Backup important data
tar -czf airdpro-backup-$(date +%Y%m%d).tar.gz data/ logs/

# Check for image updates
docker images | grep airdpro
```

### Version Updates

```bash
# Rebuild after updating application code
./build-docker.sh

# Update base image (use with caution)
# Edit base image version in Dockerfile and rebuild
```

## Known Limitations

### Technical Limitations

1. **Performance Impact**: 20-30% performance degradation when running through Wine
2. **Compatibility Issues**: Some Windows-specific features may not be fully compatible
3. **Memory Usage**: Uses more memory than native Windows environment
4. **Startup Time**: Longer initialization time required for first run

### Functional Limitations

1. **Hardware Acceleration**: Some hardware acceleration features may not be available
2. **File System**: Case-sensitive file paths may cause issues
3. **Network Access**: Complex network configurations may require additional setup

## Technical Support

### Getting Help

When encountering issues, follow these troubleshooting steps:

1. Run configuration test: `./test-docker.sh`
2. Check application logs: `logs/airdpro.log`
3. Check container status: `docker ps -a`
4. View detailed error information: `docker logs <container-name>`

### Common Error Codes

- **Wine Errors**: Check Wine configuration and .NET Framework installation
- **X11 Connection Errors**: Check XQuartz configuration and network settings
- **Insufficient Memory**: Increase Docker memory allocation
- **File Permission Errors**: Check data directory permissions

## License and Copyright

- AirdPro Application: Follows original license
- Docker Configuration and Scripts: MIT License
- Wine: LGPL License
- .NET Framework: Microsoft Software License

---

**Note**: This solution is designed to provide an alternative way for macOS users to run .NET Framework applications, but cannot guarantee identical performance and functionality as native Windows environments. Thorough testing is recommended before using in critical production environments.
# AirdPro Docker Packaging Guide (English)

## Overview

This guide provides comprehensive instructions for packaging and deploying AirdPro using Docker across multiple platforms. AirdPro is a mass spectrometry data analysis tool that supports Windows, Linux, and macOS through containerized deployment.

## Table of Contents

1. [System Requirements](#system-requirements)
2. [Project Structure](#project-structure)
3. [Docker Build Process](#docker-build-process)
4. [Platform-Specific Instructions](#platform-specific-instructions)
5. [Image Variants](#image-variants)
6. [Running Instructions](#running-instructions)
7. [Configuration and Data Management](#configuration-and-data-management)
8. [Troubleshooting](#troubleshooting)
9. [Performance Optimization](#performance-optimization)
10. [Security Considerations](#security-considerations)

## System Requirements

### Hardware Requirements

- **Memory**: Minimum 8GB RAM (16GB recommended)
- **Storage**: At least 20GB free disk space
- **CPU**: Multi-core processor (4+ cores recommended)
- **Platform**: Windows 10/11, macOS 10.15+, or Linux distributions

### Software Dependencies

- **Docker**: Version 20.10 or later
- **Docker Compose**: Version 2.0 or later (optional)
- **Platform-specific requirements**:
  - **Windows**: Docker Desktop for Windows
  - **macOS**: Docker Desktop for Mac
  - **Linux**: Docker Engine or Docker Desktop

### Network Requirements

- Internet connection for initial image downloads
- Stable bandwidth for large dataset processing

## Project Structure

```
AirdPro/
├── Dockerfile                 # Multi-stage Docker build file
├── Dockerfile.crossplatform   # Cross-platform optimized version
├── docker-compose.yml         # Multi-service orchestration
├── build-docker-en.bat        # Windows build script
├── build-docker-en.sh         # Linux/macOS build script
├── run-gui-en.sh             # GUI runtime script
├── run-cli-en.sh             # CLI runtime script
├── README_CROSSPLATFORM.md   # Cross-platform deployment guide
├── AirdPro.sln               # Solution file
├── AirdPro/                  # Main application project
├── CSharpSDK/                # .NET SDK components
├── ImportLibs/               # Third-party libraries
└── data/                     # Data directory
└── logs/                     # Logs directory
```

## Docker Build Process

### Quick Start Build

#### Windows Environment
```batch
# Navigate to project directory
cd D:\workspace\AirdPro

# Build all variants
build-docker-en.bat
```

#### Linux/macOS Environment
```bash
# Navigate to project directory
cd /path/to/AirdPro

# Build all variants
./build-docker-en.sh
```

### Manual Build Process

#### 1. Build Windows Native Version
```bash
docker build --target runtime-windows -t airdpro:windows .
```

#### 2. Build Linux/macOS Wine Version
```bash
docker build --target runtime-linux -t airdpro:linux .
```

#### 3. Build CLI Version
```bash
docker build --target runtime-cli -t airdpro:cli .
```

#### 4. Build Development Version
```bash
docker build --target runtime-dev -t airdpro:dev .
```

### Build with Docker Compose

```bash
# Build all services
docker-compose build

# Build specific service
docker-compose build airdpro-windows
docker-compose build airdpro-linux
```

## Platform-Specific Instructions

### Windows Deployment

#### Prerequisites
1. Install Docker Desktop for Windows
2. Enable WSL 2 backend
3. Switch to Linux containers mode

#### Build Commands
```batch
# Using build script
build-docker-en.bat

# Manual build
docker build --target runtime-windows -t airdpro:windows .
```

#### Run Commands
```batch
# Windows native container
docker run -it --rm --name airdpro-windows ^
    -v "%cd%\data:/data" ^
    -v "%cd%\logs:/logs" ^
    --platform windows/amd64 ^
    airdpro:windows
```

### macOS Deployment

#### Prerequisites
1. Install Docker Desktop for Mac
2. Install XQuartz for GUI support: `brew install --cask xquartz`
3. Start XQuartz application

#### Build Commands
```bash
# Using build script
./build-docker-en.sh

# Manual build
docker build --target runtime-linux -t airdpro:macos .
```

#### Run Commands
```bash
# GUI version
./run-gui-en.sh --macos-only

# CLI version
./run-cli-en.sh --macos-only
```

#### GUI Setup
1. Install XQuartz: `brew install --cask xquartz`
2. Start XQuartz: `open -a XQuartz`
3. Enable "Allow connections from network clients" in XQuartz preferences
4. Restart XQuartz

### Linux Deployment

#### Prerequisites
1. Install Docker Engine: `curl -fsSL https://get.docker.com | sh`
2. For GUI support, install X11 utilities: `sudo apt install xvfb`

#### Build Commands
```bash
# Using build script
./build-docker-en.sh --linux-only

# Manual build
docker build --target runtime-linux -t airdpro:linux .
```

#### Run Commands
```bash
# GUI version
./run-gui-en.sh --linux-only

# CLI version
./run-cli-en.sh --linux-only
```

## Image Variants

### 1. Windows Native (`airdpro:windows`)
- **Base Image**: Windows Server Core LTSC 2022
- **Runtime**: .NET Framework 4.8
- **Target Platform**: Windows (native performance)
- **Use Case**: Windows environments, best performance
- **Size**: ~4-5GB

### 2. Linux/macOS Wine (`airdpro:linux`, `airdpro:macos`)
- **Base Image**: Ubuntu 22.04
- **Runtime**: Wine + .NET Framework 4.8
- **Target Platform**: Linux/macOS (cross-platform compatibility)
- **Use Case**: Non-Windows environments
- **Size**: ~8-10GB (includes Wine dependencies)

### 3. CLI Version (`airdpro:cli`)
- **Base**: Ubuntu 22.04 + Wine (minimal dependencies)
- **Features**: Command-line only, no GUI
- **Use Case**: Automated processing, batch jobs
- **Size**: ~6-7GB

### 4. Development Version (`airdpro:dev`)
- **Base**: Linux runtime + development tools
- **Features**: Full development environment
- **Use Case**: Development and debugging
- **Size**: ~9-11GB

## Running Instructions

### GUI Mode

#### Automatic Scripts
```bash
# Cross-platform GUI script
./run-gui-en.sh

# Platform-specific options
./run-gui-en.sh --linux-only     # Linux only
./run-gui-en.sh --macos-only     # macOS only
./run-gui-en.sh --windows-only   # Windows only
```

#### Manual Commands
```bash
# Linux/macOS GUI
docker run -it --rm \
    --name airdpro-gui \
    -e DISPLAY=${DISPLAY} \
    -v /tmp/.X11-unix:/tmp/.X11-unix \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    airdpro:linux
```

### CLI Mode

#### Automatic Scripts
```bash
# Cross-platform CLI script
./run-cli-en.sh

# Platform-specific options
./run-cli-en.sh --linux-only     # Linux only
./run-cli-en.sh --macos-only     # macOS only
./run-cli-en.sh --windows-only   # Windows only
```

#### Manual Commands
```bash
# CLI execution
docker run -it --rm \
    --name airdpro-cli \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    airdpro:cli

# With arguments
docker run -it --rm \
    --name airdpro-cli \
    -v $(pwd)/data:/data \
    -v $(pwd)/logs:/logs \
    airdpro:cli --help
```

### Docker Compose Mode

```bash
# Start all services
docker-compose up -d

# Start specific service
docker-compose up airdpro-linux -d

# View logs
docker-compose logs -f airdpro-linux

# Stop services
docker-compose down
```

## Configuration and Data Management

### Data Directories

- **Data Volume**: `/data` - Input/output data files
- **Logs Volume**: `/logs` - Application logs
- **Configuration**: Default configuration files included in container

### Volume Mounting

```bash
# Mount local directories
docker run -v $(pwd)/my-data:/data -v $(pwd)/my-logs:/logs airdpro:linux

# Named volumes (recommended)
docker run --name airdpro-data -v airdpro-data:/data airdpro:linux
```

### Environment Variables

```bash
# Performance tuning
-e DOTNET_GCHeapCount=8
-e DOTNET_GCServer=1

# Wine optimization
-e WINEPREFIX=/wine
-e WINEARCH=win64

# Logging level
-e AIRDPRO_LOG_LEVEL=DEBUG
```

## Troubleshooting

### Common Issues

#### 1. Docker Build Failures
```bash
# Clean Docker cache
docker system prune -f

# Rebuild with no cache
docker build --no-cache --target runtime-linux -t airdpro:linux .
```

#### 2. Wine Initialization Issues
```bash
# Remove Wine prefix and restart
docker run -it --rm -e WINE_INITIALIZE=force airdpro:linux

# Check Wine health
docker exec airdpro-container /app/healthcheck.sh
```

#### 3. GUI Display Issues (macOS/Linux)
```bash
# Check X11 forwarding
xhost +local:docker

# Test X11 connection
docker run --rm -e DISPLAY=${DISPLAY} alpine xclock
```

#### 4. Permission Issues
```bash
# Set proper permissions
chmod +x build-docker-en.sh run-gui-en.sh run-cli-en.sh

# Fix volume permissions
sudo chown -R $USER:$USER data/ logs/
```

#### 5. Memory Issues
```bash
# Check Docker resources
docker system df

# Increase Docker memory allocation
# Docker Desktop: Settings > Resources > Memory > 8GB+
```

### Performance Issues

#### Memory Optimization
- Increase Docker memory to 8GB+ for large datasets
- Enable swap space if needed
- Use CLI version for batch processing

#### Storage Optimization
```bash
# Clean unused images and containers
docker system prune -a

# Use multi-stage builds to reduce image size
docker build --target runtime-cli -t airdpro:cli .
```

## Performance Optimization

### Build Optimization
1. **Use BuildKit**: Enable BuildKit for faster builds
   ```bash
   export DOCKER_BUILDKIT=1
   docker build --target runtime-linux -t airdpro:linux .
   ```

2. **Multi-stage Builds**: Only include necessary files in runtime images

3. **Cache Optimization**: Use .dockerignore to exclude unnecessary files

### Runtime Optimization

#### Resource Allocation
```yaml
# docker-compose.yml resource limits
services:
  airdpro:
    deploy:
      resources:
        limits:
          memory: 8G
          cpus: '4.0'
        reservations:
          memory: 4G
          cpus: '2.0'
```

#### Environment Tuning
```bash
# .NET optimization
-e DOTNET_GCHeapCount=8
-e DOTNET_GCServer=1
-e DOTNET_TieredCompilation=false

# Wine optimization
-e WINEDEBUG=-all
-e WINEINIT=disabled
```

### Network Optimization
```bash
# Use local registry for faster image distribution
docker tag airdpro:linux localhost:5000/airdpro:linux
docker push localhost:5000/airdpro:linux
```

## Security Considerations

### Container Security

1. **Non-root User**: Run containers as non-root user when possible
2. **Read-only File System**: Use read-only containers for production
3. **Resource Limits**: Set memory and CPU limits

```bash
# Secure container execution
docker run --rm \
    --read-only \
    --user 1000:1000 \
    --memory=4g \
    --cpu-shares=512 \
    airdpro:linux
```

### Data Security

1. **Encrypted Volumes**: Use encrypted volumes for sensitive data
2. **Network Security**: Restrict network access when possible
3. **Image Scanning**: Regular security scanning of images

```bash
# Scan image for vulnerabilities
docker scan airdpro:linux

# Run with security options
docker run --rm \
    --security-opt=no-new-privileges \
    --cap-drop=ALL \
    --cap-add=NET_BIND_SERVICE \
    airdpro:linux
```

## Support and Maintenance

### Maintenance Tasks

1. **Regular Updates**: Update base images monthly
2. **Security Patches**: Apply security patches promptly
3. **Performance Monitoring**: Monitor container resource usage
4. **Log Management**: Regular log rotation and cleanup

### Health Checks

```bash
# Check container health
docker ps
docker inspect --format='{{.State.Health.Status}}' airdpro-container

# Manual health check
docker exec airdpro-container /app/healthcheck.sh
```

### Backup and Recovery

```bash
# Backup data volumes
docker run --rm -v airdpro-data:/data -v $(pwd):/backup alpine \
    tar czf /backup/airdpro-data-backup.tar.gz /data

# Restore data
docker run --rm -v airdpro-data:/data -v $(pwd):/backup alpine \
    tar xzf /backup/airdpro-data-backup.tar.gz -C /
```

## Version Information

- **Current Version**: 1.0.0
- **Docker Version**: 20.10+
- **Platform Support**: Windows 10+, macOS 10.15+, Ubuntu 18.04+
- **Last Updated**: December 2024

## Contact and Support

For issues, questions, or contributions:

1. Check the troubleshooting section above
2. Review logs in the `logs/` directory
3. Create an issue in the project repository
4. Contact the development team

---

**Note**: This guide covers the most common scenarios. For platform-specific optimizations or advanced configurations, please refer to platform-specific documentation or contact support.
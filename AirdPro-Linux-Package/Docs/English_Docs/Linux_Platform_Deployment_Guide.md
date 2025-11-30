# AirdPro Linux Distribution Package Deployment Guide

## Overview

AirdPro is a powerful cross-platform data transfer tool. This guide will walk you through installing, configuring, and running AirdPro on Linux systems.

## System Requirements

### Minimum Requirements
- **Operating System**: Ubuntu 18.04+, Debian 10+, CentOS 7+, Fedora 30+, or other mainstream Linux distributions
- **Processor**: x86_64 (64-bit)
- **Memory**: 2GB RAM (4GB+ recommended)
- **Disk Space**: 5GB available space
- **Network**: Internet connection (for dependency downloads)

### Recommended Configuration
- **Memory**: 8GB RAM or more
- **Disk Space**: 10GB available space
- **Graphics**: Hardware acceleration support (optional)
- **Display**: X11 or Wayland display server

## Quick Start

### Method 1: Automated Installation Script (Recommended)

1. **Download and Extract Distribution Package**
   ```bash
   # Download AirdPro-Linux-Package-v4.2.0.zip (from release page)
   unzip AirdPro-Linux-Package-v4.2.0.zip
   cd AirdPro-Linux-Package
   ```

2. **Run Automated Installation Script**
   ```bash
   # Give script execution permission
   chmod +x install-linux.sh
   
   # Run installation script (choose full mode)
   ./install-linux.sh --mode full
   ```

3. **Choose Running Method**
   
   **Docker Mode (Recommended):**
   ```bash
   chmod +x Build_Run_Scripts/run-docker-linux.sh
   ./Build_Run_Scripts/run-docker-linux.sh --build
   ```
   
   **Wine Mode (Traditional Method):**
   ```bash
   chmod +x Build_Run_Scripts/run-wine-linux.sh
   ./Build_Run_Scripts/run-wine-linux.sh
   ```

### Method 2: Manual Installation

1. **Install System Dependencies**
   ```bash
   # Ubuntu/Debian
   sudo apt update
   sudo apt install -y curl wget unzip tar
   sudo apt install -y wine64 wine32   # Required for Wine mode
   sudo apt install -y docker.io docker-compose   # Required for Docker mode
   
   # Fedora/CentOS
   sudo dnf install -y curl wget unzip tar
   sudo dnf install -y wine   # Required for Wine mode
   sudo dnf install -y docker docker-compose   # Required for Docker mode
   
   # Arch Linux
   sudo pacman -S curl wget unzip tar
   sudo pacman -S wine   # Required for Wine mode
   sudo pacman -S docker docker-compose   # Required for Docker mode
   ```

2. **Configure Docker Service (if using Docker mode)**
   ```bash
   # Start Docker service
   sudo systemctl start docker
   sudo systemctl enable docker
   
   # Add user to docker group
   sudo usermod -aG docker $USER
   
   # Re-login or execute
   newgrp docker
   ```

3. **Configure X11 Permissions (if using graphical interface)**
   ```bash
   # Set X11 permissions
   xhost +local:docker
   ```

## Docker Mode Operation

### Basic Operation
```bash
# Build and run Docker container
./Build_Run_Scripts/run-docker-linux.sh --build

# Run existing container only
./Build_Run_Scripts/run-docker-linux.sh

# Interactive mode (connect to container)
./Build_Run_Scripts/run-docker-linux.sh --interactive
```

### Advanced Options
```bash
# Debug mode
./Build_Run_Scripts/run-docker-linux.sh --debug

# High-performance setup (8GB memory, 4 CPU cores)
./Build_Run_Scripts/run-docker-linux.sh --memory 8g --cpus 4

# Custom data directory
./Build_Run_Scripts/run-docker-linux.sh --data-dir /path/to/data

# View logs
./Build_Run_Scripts/run-docker-linux.sh logs

# Stop container
./Build_Run_Scripts/run-docker-linux.sh stop

# Clean up resources
./Build_Run_Scripts/run-docker-linux.sh cleanup
```

### Docker Compose Configuration

The project includes a `docker-compose.yml` file supporting the following services:

- **airdpro-linux**: Main service
- **Data Persistence**: Automatic mounting of `./data` and `./logs` directories
- **X11 Support**: Automatic display forwarding configuration

Custom Configuration Example:
```yaml
version: '3.8'
services:
  airdpro-linux:
    build: .
    environment:
      - DISPLAY=${DISPLAY}
    volumes:
      - ./data:/app/data
      - ./logs:/app/logs
      - /tmp/.X11-unix:/tmp/.X11-unix:rw
    deploy:
      resources:
        limits:
          memory: 8G
          cpus: '4'
```

## Wine Mode Operation

### Basic Operation
```bash
# Standard startup
./Build_Run_Scripts/run-wine-linux.sh

# Force X11 mode
./Build_Run_Scripts/run-wine-linux.sh --gui-mode x11

# Interactive mode
./Build_Run_Scripts/run-wine-linux.sh --interactive

# 32-bit mode
./Build_Run_Scripts/run-wine-linux.sh --wine-arch win32
```

### Advanced Options
```bash
# Custom Wine prefix
./Build_Run_Scripts/run-wine-linux.sh --wine-prefix /path/to/prefix

# Debug mode
./Build_Run_Scripts/run-wine-linux.sh --debug

# Custom application directory
./Build_Run_Scripts/run-wine-linux.sh --app-dir /path/to/airdpro

# Headless mode (server environment)
./Build_Run_Scripts/run-wine-linux.sh --gui-mode headless

# View Wine information
./Build_Run_Scripts/run-wine-linux.sh info

# Reset Wine environment
./Build_Run_Scripts/run-wine-linux.sh reset

# Clean up Wine environment
./Build_Run_Scripts/run-wine-linux.sh cleanup
```

### Wine Environment Configuration

The script automatically configures the following components:

- **.NET Framework 4.8**: Support for .NET applications
- **Visual C++ Redistributable**: Runtime support
- **MSXML 6**: XML parsing support
- **Core Fonts**: Font support

## Troubleshooting

### Common Issues

#### 1. Docker Mode Issues

**Problem**: Docker service not running
```bash
# Check Docker status
sudo systemctl status docker

# Start Docker service
sudo systemctl start docker
```

**Problem**: Insufficient permissions
```bash
# Add user to docker group
sudo usermod -aG docker $USER
newgrp docker
```

**Problem**: GUI not displaying
```bash
# Check X11 permissions
xhost +local:docker

# Check DISPLAY environment variable
echo $DISPLAY

# If on remote server, may need X11 forwarding
ssh -X user@server
```

#### 2. Wine Mode Issues

**Problem**: Wine not installed or version incompatible
```bash
# Check Wine version
wine --version

# Ubuntu/Debian latest Wine installation
sudo apt install wine
```

**Problem**: 32-bit applications cannot run
```bash
# Install 32-bit support
sudo apt install wine32

# Use 32-bit mode
./Build_Run_Scripts/run-wine-linux.sh --wine-arch win32
```

**Problem**: GUI display issues
```bash
# Check X11 display
echo $DISPLAY
xset q

# Force X11 mode
./Build_Run_Scripts/run-wine-linux.sh --gui-mode x11
```

#### 3. General Issues

**Problem**: Insufficient memory
```bash
# Check available memory
free -h

# Docker mode reduce memory limit
./Build_Run_Scripts/run-docker-linux.sh --memory 2g
```

**Problem**: Insufficient disk space
```bash
# Check disk usage
df -h

# Clean Docker resources
docker system prune -f
```

### Log Files

- **Docker Mode**: `./logs/airdpro.log`
- **Wine Mode**: `./logs/wine-*.log`

View latest logs:
```bash
tail -f logs/airdpro.log
```

## Performance Optimization

### Docker Mode Optimization
```bash
# Enable GPU support (if available)
./Build_Run_Scripts/run-docker-linux.sh --gpu

# Use custom image registry
export DOCKER_REGISTRY=your-registry.com
./Build_Run_Scripts/run-docker-linux.sh --build
```

### Wine Mode Optimization
```bash
# Optimize Wine settings
./Build_Run_Scripts/run-wine-linux.sh --wine-options '--sync command'

# Use DXVK (better DirectX support)
./Build_Run_Scripts/run-wine-linux.sh --wine-options 'PROTON_USE_WINED3D=1'
```

## Security Considerations

1. **Network Security**: Restrict unnecessary ports in firewall
2. **Data Security**: Regularly backup data directories
3. **Permission Management**: Do not run applications as root user
4. **Update Maintenance**: Regularly update system and dependency packages

## Technical Support

If you encounter issues:

1. Check error log files
2. Verify system requirements are met
3. Try reinstalling or resetting environment
4. Submit Issues to project repository

## Version Information

- **Documentation Version**: v1.0
- **Supported Version**: AirdPro v4.2.0+
- **Last Updated**: November 30, 2025

## License

Please follow the AirdPro project license terms.
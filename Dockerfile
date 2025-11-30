# 多阶段Docker构建文件，支持AirdPro跨平台运行
# 第一阶段：构建阶段（Windows环境）
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build

# 设置工作目录
WORKDIR /src

# 复制项目文件
COPY AirdPro.sln .
COPY AirdPro/AirdPro.csproj ./AirdPro/
COPY AirdProTests/AirdProTests.csproj ./AirdProTests/
COPY CSharpSDK/AirdSDK.csproj ./CSharpSDK/

# 恢复NuGet包
RUN nuget restore AirdPro.sln

# 复制源代码
COPY AirdPro/ ./AirdPro/
COPY AirdProTests/ ./AirdProTests/
COPY CSharpSDK/ ./CSharpSDK/
COPY ImportLibs/ ./ImportLibs/

# 构建项目
RUN msbuild AirdPro.sln /p:Configuration=Release /p:Platform="Any CPU" /p:RestorePackages=false

# 第二阶段：Windows原生容器运行时
FROM mcr.microsoft.com/windows/servercore:ltsc2022 AS runtime-windows

# 安装.NET Framework 4.8
ADD https://download.microsoft.com/download/6/5/6/65632d60-c5c1-4a47-82e4-6f4b4e6e5c5e/ndp48-web.exe /temp/ndp48-web.exe
RUN C:\temp\ndp48-web.exe /quiet

# 创建工作目录
WORKDIR /app

# 复制Windows运行时文件
COPY --from=build /src/AirdPro/bin/Release/ ./
COPY --from=build /src/ImportLibs/ ./ImportLibs/

# 设置入口点
ENTRYPOINT ["AirdPro.exe"]

# 第三阶段：macOS/Linux运行时（使用Wine运行Windows应用）
FROM ubuntu:22.04 AS runtime-linux

# 设置环境变量避免交互式安装
ENV DEBIAN_FRONTEND=noninteractive
ENV WINEPREFIX=/wine
ENV WINEARCH=win64
ENV WINEDEBUG=-all
ENV DISPLAY=:99

# 安装必要的依赖
RUN dpkg --add-architecture i386 && \
    apt-get update && \
    apt-get install -y \
    wget \
    gnupg2 \
    software-properties-common \
    xvfb \
    curl \
    cabextract \
    fonts-wine \
    libasound2 \
    libc6 \
    libglib2.0-0 \
    libgphoto2-6 \
    libgphoto2-port12 \
    liblcms2-2 \
    libldap-2.5-0 \
    libltdl7 \
    libmpg123-0 \
    libopenal1 \
    libpcap0.8 \
    libpulse0 \
    libudev1 \
    libv4l-0 \
    libx11-6 \
    libx11-xcb1 \
    libxcb1 \
    libxcomposite1 \
    libxcursor1 \
    libxext6 \
    libxfixes3 \
    libxi6 \
    libxrandr2 \
    libxrender1 \
    libxss1 \
    libxtst6 \
    && wget -qO- https://dl.winehq.org/wine-builds/winehq.key | apt-key add - && \
    apt-add-repository 'deb https://dl.winehq.org/wine-builds/ubuntu/ jammy main' && \
    apt-get update && \
    apt-get install -y --install-recommends winehq-staging && \
    rm -rf /var/lib/apt/lists/*

# 创建工作目录
WORKDIR /app

# 复制Windows运行时文件
COPY --from=build /src/AirdPro/bin/Release/ ./windows/
COPY --from=build /src/ImportLibs/ ./windows/ImportLibs/

# 创建优化脚本
RUN echo '#!/bin/bash' > /app/optimize-wine.sh && \
    echo 'set -e' >> /app/optimize-wine.sh && \
    echo 'echo "优化Wine配置..."' >> /app/optimize-wine.sh && \
    echo 'export WINEPREFIX=/wine' >> /app/optimize-wine.sh && \
    echo 'export WINEARCH=win64' >> /app/optimize-wine.sh && \
    echo 'wineboot --init' >> /app/optimize-wine.sh && \
    echo 'wget -O /tmp/dotnet48.exe https://download.microsoft.com/download/6/5/6/65632d60-c5c1-4a47-82e4-6f4b4e6e5c5e/ndp48-web.exe' >> /app/optimize-wine.sh && \
    echo 'wine /tmp/dotnet48.exe /quiet' >> /app/optimize-wine.sh && \
    echo 'rm /tmp/dotnet48.exe' >> /app/optimize-wine.sh && \
    echo 'winetricks -q corefonts vcrun2019' >> /app/optimize-wine.sh && \
    echo 'wineserver -w' >> /app/optimize-wine.sh && \
    echo 'echo "Wine优化完成"' >> /app/optimize-wine.sh && \
    chmod +x /app/optimize-wine.sh

# 创建启动脚本
RUN echo '#!/bin/bash' > /app/start.sh && \
    echo 'set -e' >> /app/start.sh && \
    echo 'if [ ! -d "/wine/dosdevices/c:/windows" ]; then' >> /app/start.sh && \
    echo '    echo "首次运行，正在初始化Wine环境..."' >> /app/start.sh && \
    echo '    /app/optimize-wine.sh' >> /app/start.sh && \
    echo 'fi' >> /app/start.sh && \
    echo 'echo "启动AirdPro应用程序..."' >> /app/start.sh && \
    echo 'cd /app/windows' >> /app/start.sh && \
    echo 'export WINEPREFIX=/wine' >> /app/start.sh && \
    echo 'export WINEARCH=win64' >> /app/start.sh && \
    echo 'wine AirdPro.exe "$@"' >> /app/start.sh && \
    chmod +x /app/start.sh

# 创建健康检查脚本
RUN echo '#!/bin/bash' > /app/healthcheck.sh && \
    echo 'export WINEPREFIX=/wine' >> /app/healthcheck.sh && \
    echo 'export WINEARCH=win64' >> /app/healthcheck.sh && \
    echo 'wineboot --status' >> /app/healthcheck.sh && \
    echo 'if [ $? -eq 0 ]; then' >> /app/healthcheck.sh && \
    echo '    exit 0' >> /app/healthcheck.sh && \
    echo 'else' >> /app/healthcheck.sh && \
    echo '    exit 1' >> /app/healthcheck.sh && \
    echo 'fi' >> /app/healthcheck.sh && \
    chmod +x /app/healthcheck.sh

# 设置数据卷
VOLUME ["/data"]
VOLUME ["/logs"]

# 健康检查
HEALTHCHECK --interval=30s --timeout=10s --start-period=120s --retries=3 \
    CMD ["/app/healthcheck.sh"]

# 设置入口点
ENTRYPOINT ["/app/start.sh"]
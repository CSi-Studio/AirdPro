# 多阶段Docker构建文件，支持AirdPro在macOS上运行
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

# 第二阶段：macOS/Linux运行时（使用Wine运行Windows应用）
FROM ubuntu:22.04 AS runtime-macos

# 安装必要的依赖 - 包括Wine和.NET Framework支持
RUN dpkg --add-architecture i386 && \
    apt-get update && \
    apt-get install -y \
    wget \
    gnupg2 \
    software-properties-common \
    xvfb \
    && wget -qO- https://dl.winehq.org/wine-builds/winehq.key | apt-key add - && \
    apt-add-repository 'deb https://dl.winehq.org/wine-builds/ubuntu/ jammy main' && \
    apt-get update && \
    apt-get install -y --install-recommends winehq-stable winetricks && \
    rm -rf /var/lib/apt/lists/*

# 设置Wine环境变量
ENV WINEPREFIX=/wine
ENV WINEARCH=win64
ENV WINEDEBUG=-all
ENV DISPLAY=:99

# 创建工作目录
WORKDIR /app

# 复制Windows运行时文件
COPY --from=build /src/AirdPro/bin/Release/ ./windows/
COPY --from=build /src/ImportLibs/ ./windows/ImportLibs/

# 初始化Wine并安装.NET Framework 4.8
RUN mkdir -p /wine && \
    wineboot --init && \
    # 安装必要的Windows组件
    winetricks -q corefonts && \
    winetricks -q vcrun2019 && \
    winetricks -q dotnet48 && \
    # 等待Wine初始化完成
    sleep 10 && \
    wineserver -w

# 创建启动脚本
RUN echo '#!/bin/bash' > /app/start.sh && \
    echo 'set -e' >> /app/start.sh && \
    echo 'echo "启动Xvfb虚拟显示..."' >> /app/start.sh && \
    echo 'Xvfb :99 -screen 0 1024x768x16 &' >> /app/start.sh && \
    echo 'sleep 2' >> /app/start.sh && \
    echo 'echo "启动AirdPro应用程序..."' >> /app/start.sh && \
    echo 'cd /app/windows' >> /app/start.sh && \
    echo 'wine AirdPro.exe "$@"' >> /app/start.sh && \
    chmod +x /app/start.sh

# 创建健康检查脚本
RUN echo '#!/bin/bash' > /app/healthcheck.sh && \
    echo 'wineboot --status' >> /app/healthcheck.sh && \
    echo 'if [ $? -eq 0 ]; then' >> /app/healthcheck.sh && \
    echo '    exit 0' >> /app/healthcheck.sh && \
    echo 'else' >> /app/healthcheck.sh && \
    echo '    exit 1' >> /app/healthcheck.sh && \
    echo 'fi' >> /app/healthcheck.sh && \
    chmod +x /app/healthcheck.sh

# 设置数据卷
VOLUME ["/data"]

# 健康检查
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD ["/app/healthcheck.sh"]

# 设置入口点
ENTRYPOINT ["/app/start.sh"]
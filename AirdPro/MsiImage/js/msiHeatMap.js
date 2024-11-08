function drawHeatmap(heatmapData, maxIntensity, maxPixelX, maxPixelY) {
    const msiContainer = document.getElementById('msiContainer');
    const msiHeatMap = echarts.init(msiContainer);

    // 计算宽高比
    const aspectRatio = maxPixelX / maxPixelY;

    // 动态设置容器尺寸
    function setContainerSize() {
        const panelWidth = msiContainer.offsetWidth; // 获取 panel 宽度
        const panelHeight = msiContainer.offsetHeight; // 获取 panel 高度

        // 根据宽高比和窗口尺寸计算容器的宽度和高度
        let containerWidth, containerHeight;
        if (panelWidth / panelHeight > aspectRatio) {
            // 如果 panel 的宽高比大于热力图的宽高比，那么高度应该与 panel 高度相匹配
            containerHeight = panelHeight;
            containerWidth = aspectRatio * containerHeight;
        } else {
            // 否则，宽度应该与 panel 宽度相匹配
            containerWidth = panelWidth;
            containerHeight = containerWidth / aspectRatio;
        }

        // 设置容器的尺寸
        msiContainer.style.width = `${containerWidth}px`;
        msiContainer.style.height = `${containerHeight}px`;

        // 调整ECharts图表大小以适应容器尺寸
        msiHeatMap.resize();
    }

    // 初始化时设置容器尺寸
    setContainerSize();

    // 监听窗口大小变化事件，以便在窗口大小变化时调整容器尺寸
    window.addEventListener('resize', setContainerSize);

    // 蓝黄红渐变(echarts案例)
    const colorList = [
        '#313695',
        '#4575b4',
        '#74add1',
        '#abd9e9',
        '#e0f3f8',
        '#ffffbf',
        '#fee090',
        '#fdae61',
        '#f46d43',
        '#d73027',
        '#a50026'
    ];

    const option = {
        tooltip: {
            position: 'top'
        },
        grid: {
            left: '10%',
            right: '10%',
            top: '10%',
            bottom: '10%',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            data: Array.from({ length: maxPixelX }, (_, i) => i),
            splitArea: { show: true },
            axisLine: {
                show: false
            },
            axisTick: {
                show: false
            },
            position: 'top'
        },
        yAxis: {
            type: 'category',
            data: Array.from({ length: maxPixelY }, (_, i) => i).reverse(),
            splitArea: { show: true },
            axisLine: {
                show: false
            },
            axisTick: {
                show: false
            }
        },
        visualMap: {
            min: 0,
            max: maxIntensity,
            calculable: true,
            type: 'continuous',
            orient: 'horizontal',
            left: 'center',
            top: 'top',
            inRange: {
                color: colorList
            },
            textStyle: {
                color: '#fff'
            }
        },
        series: [{
            name: 'Heatmap',
            type: 'heatmap',
            coordinateSystem: 'cartesian2d',
            data: heatmapData.map(p => [p[0] - 1, maxPixelY - p[1], p[2]]),
            emphasis: {
                itemStyle: {
                    borderColor: 'transparent',
                    borderWidth: 0
                }
            },
            animation: false
        }]
    };

    msiHeatMap.setOption(option);
}
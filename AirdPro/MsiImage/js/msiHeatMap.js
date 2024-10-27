function drawHeatmap(heatmapData, maxPixelX, maxPixelY, maxIntensity) {
    if (typeof echarts === 'undefined') {
        console.error('ECharts is not loaded');
        return;
    }

    const msiContainer = document.getElementById('msiContainer');
    if (!msiContainer) {
        console.error('msiContainer not found');
        return;
    }

    const msiHeatMap = echarts.init(msiContainer);

    // 定义从蓝色到绿色再到黄色的渐变色
    const colorList = [        
        '#00BFFF',  // 蓝色
        '#C0E0C0',  // 绿色
        '#FFD700'   // 黄色
    ];

    const option = {
        tooltip: {
            position: 'top'
        },
        xAxis: {
            type: 'category',
            data: Array.from({ length: maxPixelX }, (_, i) => i),
            splitArea: { show: true }
        },
        yAxis: {
            type: 'category',
            data: Array.from({ length: maxPixelY }, (_, i) => i).reverse(),
            splitArea: { show: true },
            axisLine: {
                show: false
            }
        },
        visualMap: {
            min: 0,
            max: maxIntensity,
            calculable: true,
            orient: 'vertical',
            left: 'left',
            bottom: 'center',
            inRange: {
                color: colorList // 应用颜色渐变
            },
            textStyle: {
                color: '#fff'
            }
        },
        series: [{
            name: 'Heatmap',
            type: 'heatmap',
            coordinateSystem: 'cartesian2d',
            data: heatmapData.map(p => [p[0], maxPixelY - p[1], p[2]]),
            emphasis: {
                itemStyle: {
                    borderColor: 'transparent', // 设置边框为透明
                    borderWidth: 0 // 设置边框宽度为0，隐藏边框
                }
            }
        }]
    };

    msiHeatMap.setOption(option);
    window.addEventListener('resize', msiHeatMap.resize);
}
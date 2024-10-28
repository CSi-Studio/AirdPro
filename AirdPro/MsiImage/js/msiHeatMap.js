function drawHeatmap(heatmapData, maxIntensity, maxPixelX, maxPixelY) {
    const msiHeatMap = echarts.init(document.getElementById('msiContainer'));

    //蓝黄红渐变(echarts案例)
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
        xAxis: {
            position: 'top',
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
            type: 'continuous',
            orient: 'vertical',
            left: 'left',
            bottom: 'center',
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
    window.addEventListener('resize', msiHeatMap.resize);
}
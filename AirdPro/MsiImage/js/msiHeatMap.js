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

    // 定义颜色渐变，从浅到深
    const colorList = ['#313695', '#4575b4', '#74add1', '#abd9e9', '#e0f3f8', '#ffffbf', '#fee090', '#fdae61', '#f46d43', '#d73027', '#a50026'];

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
            data: Array.from({ length: maxPixelY }, (_, i) => i),
            splitArea: { show: true }
        },
        visualMap: {
            min: 0,
            max: maxIntensity,
            calculable: true,
            orient: 'horizontal',
            left: 'center',
            bottom: '15%',
            inRange: {
                color: colorList.reverse() // 反转颜色数组，以确保强度值越大，颜色越深
            }
        },
        series: [{
            name: 'Heatmap',
            type: 'heatmap',
            coordinateSystem: 'cartesian2d',
            data: heatmapData,
            emphasis: {
                itemStyle: {
                    borderColor: '#333',
                    borderWidth: 1
                }
            }
        }]
    };

    msiHeatMap.setOption(option);
    window.addEventListener('resize', msiHeatMap.resize);
}
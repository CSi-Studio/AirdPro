function drawBarChart(mzArray, intensityArray) {
    const msBarChart = echarts.init(document.getElementById('msContainer'));

    const option = {
        title: {
            text: 'Spectrum'
        },
        xAxis: {
            name: 'm/z',
            type: 'category',
            data: mzArray
        },
        yAxis: {
            name: 'intensity',
            type: 'value',
            axisLine: {
                show: true,
            }
        },
        series: [{
            type: 'bar',
            barWidth: 2,
            data: intensityArray
        }]
    };

    msBarChart.setOption(option);
    // 监听窗口大小变化，调整图表大小
    window.addEventListener('resize', msBarChart.resize);
}
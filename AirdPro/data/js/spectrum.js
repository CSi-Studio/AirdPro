function drawBarChart(mzArray, intensityArray) {
    const spectrumChart = echarts.init(document.getElementById('spectrumContainer'));

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

    spectrumChart.setOption(option);
}
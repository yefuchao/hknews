function pageLoad() {
    var data = getData();
    var myChart = echarts.init(document.getElementById("main"));
    option = {
        title: {
            text: '同名数量统计',
            subtext: '纯属虚构',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
            type: 'scroll',
            orient: 'vertical',
            right: 10,
            top: 20,
            bottom: 20,
            data: data.legendData,

            selected: data.selected
        },
        series: [
            {
                name: '姓名',
                type: 'pie',
                radius: '55%',
                center: ['40%', '50%'],
                data: data.seriesData,
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            }
        ]
    };

    myChart.setOption(option);
}

function getData() {
    $.get("http://localhost:56109/api/stock?datestr=2018-01-02", function (data) {
        return {
            legendData: data.name,
            seriesData: data.rate,
            selected: data.selected
        }
    });
}
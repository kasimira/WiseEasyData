$(document).ready(function () {

    $.ajax({
        type: "POST",
        url: 'HomeApp/Intex',
        data: JSON.stringify({}),
        contentType: "application/json:charset=utf-8",
        dataType: "json",
        success: function (json) {
            var values = json.DashBoardcount;
            var malecount = parseInt(values[0]);
            var femalecount = parseInt(values[1]);

            Highcharts.chart('container', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Emploees gender count'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    name: 'Count',
                    colorByPoint: true,
                    data: [{
                        name: 'Male',
                        y: malecount,
                        sliced: true,
                        selected: true
                    }, {
                        name: 'Female',
                        y: femalecount                     
                    }]
                }]
            });
        }
    })
})

Highcharts.chart('container', {
    chart: {
        plotBackgroundColor: null,
        plotBorderWidth: null,
        plotShadow: false,
        type: 'pie'
    },
    title: {
        text: 'Emploees gender count'
    },
    tooltip: {
        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
    },
    accessibility: {
        point: {
            valueSuffix: '%'
        }
    },
    plotOptions: {
        pie: {
            allowPointSelect: true,
            cursor: 'pointer',
            dataLabels: {
                enabled: false
            },
            showInLegend: true
        }
    },
    series: [{
        name: 'Count',
        colorByPoint: true,
        data: [{
            name: 'Male',
            y: malecount,
            sliced: true,
            selected: true
        }, {
            name: 'Female',
            y: femalecount
        }]
    }]
});
   

// Build the chart


//
//Highcharts.chart('container', {
//    title: {
//        text: 'Chart.update'
//    },
//    subtitle: {
//        text: 'Plain'
//    },
//    xAxis: {
//        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
//    },
//    series: [{
//        type: 'column',
//        colorByPoint: true,
//        //data: [29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4],
//        data: values,
//        showInLegend: false
//    }]
//});
//
//document.getElementById('plain').addEventListener('click', () => {
//    chart.update({
//        chart: {
//            inverted: false,
//            polar: false
//        },
//        subtitle: {
//            text: 'Plain'
//        }
//    });
//});
//
//document.getElementById('inverted').addEventListener('click', () => {
//    chart.update({
//        chart: {
//            inverted: true,
//            polar: false
//        },
//        subtitle: {
//            text: 'Inverted'
//        }
//    });
//});
//
//document.getElementById('polar').addEventListener('click', () => {
//    chart.update({
//        chart: {
//            inverted: false,
//            polar: true
//        },
//        subtitle: {
//            text: 'Polar'
//        }
//    });
//});

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PAK_OUT_Boss.aspx.cs" Inherits="webroot_aspx_PAK_OUT_Boss" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta http-equiv="viewport" content="width=device-width,initial-scale=1.0">
<head id="Head1" runat="server">
    <title>PAK产出汇总</title>

    <style>
    HTML,BODY,FORM
    {
        height:100%;
    }
   </style>
</head>
<body leftMargin="0" topMargin="0" rightMargin="1">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="../../Service/WebServiceEchart.asmx" />
            </Services>
        </asp:ScriptManager>
  <div style="width: 100%; height: 100%;">
      <div id="barpic"  style="width: 50%; height: 100%; margin: 0px auto; position: absolute; top: 0px; left: 0%; border: 1px solid #eee "></div>
      <div id="mainsmt" style="width: 50%; height: 100%; margin: 0px auto; position: absolute; top: 0px; left: 50%; border: 0px solid #eee "></div>

      
      
      

  </div>
  
     
      
       
        
     
    
     <!-- ECharts单文件引入 -->
     <script type="text/javascript" src="../../echarts-2.2.7/build/source/echarts.js"></script>
       <%--  <script type="text/javascript" src="../../echarts-2.2.7/build/dist/chart/line.js"></script>
            <script type="text/javascript" src="../../echarts-2.2.7/build/dist/chart/bar.js"></script>--%>
    <script type="text/javascript">
        require.config({
            paths: {
                echarts: '../../echarts-2.2.7/build/dist'
            }
        });

        // 使用
        require(
            [
                'echarts',
                'echarts/chart/line',
                'echarts/chart/bar', // 使用柱状图就加载bar模块，按需加载
              //'echarts/chart/gauge' // 使用柱状图就加载bar模块，按需加载
                  'echarts/chart/pie',                  'echarts/chart/funnel',                  
                
            ], DrawEChartAll
            )
        function DrawEChartAll(ec) {
            DrawEChartFABarOut(ec); //画SMT
            DrawEChartFAPieOut(ec); //画SMT

            
        }
        var FABarChart;
        var FApieChart;
       
        //======================================================
        function DrawEChartFAPieOut(ec) {
            FApieChart = ec.init(document.getElementById('barpic'));
            var ecConfig = require('echarts/config');
            pieoption = {
                title: {
                    text: 'PAK产出(当天)',
                    subtext: '数据来自IMES',
                    x: 'center'
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                },
                legend: {
                    orient: 'vertical',
                    x: 'left',
                    data: []
                },
                toolbox: {
                    show: true,
                    feature: {
                        mark: { show: true },
                        dataView: { show: true, readOnly: false },
                        magicType: {
                            show: true,
                            type: ['pie', 'funnel'],
                            option: {
                                funnel: {
                                    x: '25%',
                                    width: '50%',
                                    funnelAlign: 'left',
                                    max: 1548
                                }
                            }
                        },
                        restore: { show: true },
                        saveAsImage: { show: true }
                    }
                },
                
               calculable: true,
                        series: [
                            {
                                name: '工單数量占比分析:',
                                type: 'pie',
                                radius: '55%',
                                center: ['50%', '60%'],
                                itemStyle: {
                                    normal: {
                                        color: function (params) {
                                            var colorList = [
                                              '#C1232B', '#B5C334', '#FCCE10', '#E87C25', '#27727B'      
                                            ];
                                            return colorList[params.dataIndex]
                                        },
                                        label: {
                                            show: true,
                                            position: 'outer',
                                            formatter: "{b}\n{d}%"//在饼状图上显示百分比

                                        },
                                        labelLine: {
                                            show: true
                                        }
                                    },
                                    emphasis: {
                                        label: {
                                            show: false,
                                            formatter: "{d}%"
                                        }
                                    }
                                },
                                data: [
                                ]
                            }
                        ]
                        };
                    FApieChart.showLoading({
                text: "图表数据正在努力加载..."
            });
                FApieChart.setOption(pieoption, true);
                WebServiceEchart.GetData_OutPut("PA", inputSucc, inputFail);
                function inputSucc(GetYieldAnalysis) {
                    if (GetYieldAnalysis) {
                    var seriesArray = new Array();
                    var selecteditems = new Array();
                    seriesArray = GetYieldAnalysis[1];
                    for (var i = 0; i < seriesArray.length; i++) {
                       var item = {
                           name: seriesArray[i].Name,
                           value: seriesArray[i].Value
                        }
                        selecteditems.push(item);
                   }
                
                    pieoption.legend.data = GetYieldAnalysis[0];
                    pieoption.series[0].data = selecteditems;
                      pieoption.title.subtext=GetYieldAnalysis[2];
                        FApieChart.hideLoading();
                        FApieChart.setOption(pieoption, true);
                    }


                }
                function inputFail(result) {
                    var options = FApieChart.getOption();
                    FApieChart.showLoading({
                        text: "效率系统服务连接失败..."
                    });

                }

        }



        //======================================================


        function DrawEChartFABarOut(ec) {
            FABarChart = ec.init(document.getElementById('mainsmt'));
            var ecConfig = require('echarts/config');
            option = {
                title: {
                    text: 'PAK产出汇总',
                    subtext: '',
                    x: 0,   //這個指示位置,單位px
                    y: 20

                },
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'shadow'
                    }
                   
                },
                toolbox: {
                    y: 20,
                    show: true,
                    feature: {
                        mark: { show: true },
                        dataView: { show: true, readOnly: false },
                        magicType: { show: true, type: ['line', 'bar'] },
                        restore: { show: true },
                        saveAsImage: { show: true }
                    }
                },
                calculable: true,
                legend: {
                    selected: {},
                    data: []
                },
                 xAxis: [{
                            type: 'category',
                            boundaryGap: true,//两端留白,false时则顶头
                             name: '线别',
                            margin: 3,
                            data: [0],
                           axisLabel: {
                                           interval: 0,
                                            textStyle: {
                                                fontWeight: 'bold',
                                                fontSize: 12

                                            }
                                        },
                        }],
                 yAxis: [{
                            type: 'value',
                            name: '数量',
                            axisLabel: {
                                formatter: '{value} pcs'
                            }
                        }
                        ],
                series: [
                            {
                                name: '总产出',
                                type: 'bar',
                                data: [],
                                itemStyle: {
                                    normal: {

                                        label: {
                                            show: true, formatter: '{c} pcs',
                                            textStyle: {
                                                fontWeight: 'bolder'
                                            }
                                        }
                                    }
                                }
                            }
                        ]



            };


            // 为echarts对象加载数据
            FABarChart.showLoading({
                text: "图表数据正在努力加载..."
            });
            FABarChart.setOption(option);
             WebServiceEchart.GetData_OutPut("PA", inputSucc, inputFail);
           // setInterval(AutoRefresh, 1000 * 60);
            function AutoRefresh() {
                var options = FABarChart.getOption();
                FABarChart.showLoading({
                    text: "图表数据正在努力加载..."
                });
                // myChart.refresh(true);
               WebServiceEchart.GetData_OutPut("PA", inputSucc, inputFail);
            }
            function inputSucc(GetYieldAnalysis) {
                if (GetYieldAnalysis) {
                    drawseries(option, GetYieldAnalysis)//根据返回的数据自动添加线条
                    FABarChart.hideLoading();
                    FABarChart.setOption(option, true);
                }


            }

            function drawseries(option, ChartItem) {
                var seriesArray = new Array();
                var legendlist = new Array(); //图例
                var XValue= new Array(); //X轴
                var seriesitems = new Array();
                // option.title.subtext = ChartItem.Title;
                seriesArray = ChartItem[1];
                for (var i = 0; i < seriesArray.length; i++) {
                      
                   seriesitems.push(seriesArray[i].Value);
                   XValue.push(seriesArray[i].Name);
                   }
                option.series[0].data = seriesitems;
                option.xAxis[0].data = XValue;
                option.title.subtext=ChartItem[2];
               // option.legend.data = XValue;


            }
            function inputFail(result) {
                var options = FABarChart.getOption();
                FABarChart.showLoading({
                    text: "效率系统服务连接失败..."
                });

            }



        }
        
        
    </script>
    </form>
</body>
</html>
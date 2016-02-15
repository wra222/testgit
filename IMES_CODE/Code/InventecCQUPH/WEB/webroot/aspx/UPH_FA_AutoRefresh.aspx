<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UPH_FA_AutoRefresh.aspx.cs" Inherits="webroot_aspx_UPH_FA_AutoRefresh" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FA产出报表</title>
  <style type="text/css">         

    HTML,BODY,FORM
    {
        height:100%;
    }
  
    </style>
</head>
<body leftMargin="0" topMargin="0" rightMargin="1">
 <form id="form1" runat="server"  >
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="../../Service/WebServiceEchart.asmx" />
            </Services>
        </asp:ScriptManager>
    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
    <div id="main"   style="height:100%;width:100%; " ></div>
    <!-- ECharts单文件引入 -->
     <script type="text/javascript" src="../../echarts-2.2.7/build/source/echarts.js"></script>
      <%--  <script type="text/javascript" src="../../echarts-2.2.7/build/dist/chart/line.js"></script>
            <script type="text/javascript" src="../../echarts-2.2.7/build/dist/chart/bar.js"></script>--%>
    <script type="text/javascript">

        // 路径配置
        // 路径配置

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
                'echarts/chart/bar' // 使用柱状图就加载bar模块，按需加载
            ], DrawEChart
            )
        var myChart;
         function DrawEChart(ec) {
                    myChart = ec.init(document.getElementById('main'));
                    var ecConfig = require('echarts/config');
                    option = {
                        title: {
                            text: 'FA产出（pcs/h）',
                            subtext: '',
                            x: 0,   //這個指示位置,單位px
                            y:20,
                            textStyle: {
                                color: '#C1232B',
                                fontWeight: 'bolder'
                            }

                        },
                        tooltip: {
                            trigger: 'axis',
                            axisPointer: {
                                type: 'shadow'
                            },
                           formatter: '{c} pcs',
                        },
                        toolbox: {
                            y:20,
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
                           selected: { },
                            data: []
                        },
                        xAxis: [
                                {
                                    type: 'category',
                                    data: []
                                }
                        ],
                        yAxis: [
                            {
                                type: 'value',
                                name: '数量',
                                axisLabel: {
                                    formatter: '{value} pcs'
                                }
                            },
                            {
                                type: 'value',
                                name: '良率',
                                axisLabel: {
                                    formatter: '{value} %'
                                }
                            }
                        ],
                         series: []
                       

                        
                    };


            // 为echarts对象加载数据
            myChart.showLoading({
                text: "图表数据正在努力加载..."
            });
            myChart.setOption(option);
            WebServiceEchart.GetData_Daily("FA", inputSucc, inputFail);
             setInterval(AutoRefresh, 1000*60);
            function AutoRefresh() {
                var options = myChart.getOption();
                myChart.showLoading({
                    text: "图表数据正在努力加载..."
                });
               // myChart.refresh(true);
                WebServiceEchart.GetData_Daily("FA", inputSucc, inputFail);
            }
            function inputSucc(GetYieldAnalysis) {
                if (GetYieldAnalysis) {
                    drawseries(option, GetYieldAnalysis)//根据返回的数据自动添加线条
                    myChart.hideLoading();
                    myChart.setOption(option, true);
                }


            }

            function drawseries(option, ChartItem) {
                var seriesArray = new Array();
                var legendlist = new Array(); //图例
                var XValue; //X轴
                var selecteditems = new Array(); 
                // option.title.subtext = ChartItem.Title;
                seriesArray = ChartItem.Series;
                for (var i = 0; i < seriesArray.length; i++) {
                    var item = {
                                    name: seriesArray[i].Name,
                                    type: "line",
                                    data: seriesArray[i].SeriesValues,
                                    itemStyle: {
                                            normal: {
                                                      //  color: '#60c0dd',
                                                         label: {
                                                               show: true, 
                                                               formatter: '{c} pcs',
                                                               textStyle: {
                                                                       fontWeight: 'bolder'
                                                                     }
                                                                  }
                                                     }
                                                  }
                               }

                               XValue = seriesArray[i].XValue;
                               selecteditems.push(item);
                               option.series.push(item);
                    var val = seriesArray[i].Name;
                    legendlist.push(seriesArray[i].Name);
                   
                 //只显示5笔
                    if (i > 3) {
                        option.legend.selected[val] = false;
                    }
              }


                option.series = selecteditems;
                option.xAxis[0].data = ChartItem.XValue;
                option.legend.data = legendlist;
                

            }
            function inputFail(result) {
                var options = myChart.getOption();
                myChart.showLoading({
                    text: "效率系统服务连接失败..."
                });

            }



        }


           
            
    </script>
    
     <script type="text/javascript">
         function showPic(obj, Layer1) {
             if (obj.alt == "(No ESOP)") return;
             var x, y;
             x = event.clientX;
             y = event.clientY;
             document.getElementById(Layer1).style.right = 200;
             document.getElementById(Layer1).style.top = 25;
             document.getElementById(Layer1).innerHTML = "<img src=" + obj.src + ">";
             document.getElementById(Layer1).style.display = "block";
         }
         function zoom(id, rate) {
             var t = document.getElementById(id).style;
             var w = Math.floor(parseInt(t.width) * rate);
             var h = Math.floor(parseInt(t.height) * rate);
             if (w > 20 && h > 20) {
                 t.width = w + "px";
                 t.height = h + "px";
             }
         }
     </script>
    </form>
</body>
</html>

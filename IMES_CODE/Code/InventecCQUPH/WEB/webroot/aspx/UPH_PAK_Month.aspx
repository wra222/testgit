<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UPH_PAK_Month.aspx.cs" Inherits="webroot_aspx_UPH_PAK_Month" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PAK每月生产效率排行榜</title>
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
    <div id="main"   style="height:100%;width:100%;"  ></div>
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
                // 基于准备好的dom，初始化echarts图表
                 myChart = ec.init(document.getElementById('main'));

                 option = {
                    


                     //renderAsImage: true,
                     //  calculable:true,
                     title: {

                     text: 'PAK生产效率(每月)',
                         subtext: '数据来自IMES系统（每5分钟整理一次）',
                         x: 'left',   //這個指示位置,單位px

                         textStyle: {
                           
                             fontSize: 12,
                             fontWeight: 'bolder'
                         }, //主标题
                         subtextStyle: { fontSize: 10, fontWeight: 'bolder'}//副标题

                     },
                     tooltip: {         // Option config. Can be overwrited by series or data
                         trigger: 'axis',
                         //show: true,   //default true
                         showDelay: 0,
                         hideDelay: 50,
                         transitionDuration: 0,
                         backgroundColor: 'rgba(255,0,255,0.7)',
                         borderColor: '#f50',
                         borderRadius: 8,
                         borderWidth: 2,
                         padding: 10,    // [5, 10, 15, 20]
                         position: function(p) {
                             // 位置回调
                             // console.log && console.log(p);
                             return [p[0] + 10, p[1] - 10];
                         },
                         textStyle: {
                             color: 'yellow',
                             decoration: 'none',
                             fontFamily: 'Verdana, sans-serif',
                             fontSize: 15,
                             fontStyle: 'italic',
                             fontWeight: 'bold'
                         },
                         formatter: function(params, ticket, callback) {
                             //  console.log(params)
                             var res = 'Function formatter : <br/>' + params[0].name;
                             for (var i = 0, l = params.length; i < l; i++) {
                                 res += '<br/>' + params[i].seriesName + ' : ' + params[i].value;
                             }
                             setTimeout(function() {
                                 // 仅为了模拟异步回调
                                 callback(ticket, res);
                             }, 1000)
                             return 'loading';
                         }

                     },

                     legend: {
                        // x: 'center',
                        // y: 'bottom',
                         // orient:'vertical',
                     textStyle: { fontSize: 15 },
                     selected: {},
                         data: []
                     },
                     toolbox: {
                     show: true,
                         y:'25',
                         feature: {
                             mark: { show: true },
                             dataView: { show: true, readOnly: false },
                             magicType: { show: true, type: ['line', 'bar', 'stack', 'tiled'] },
                             restore: { show: true },
                             saveAsImage: { show: true },
                             dataZoom: {
                                 show: true,
                                 title: {
                                     dataZoom: '区域缩放',
                                     dataZoomReset: '区域缩放后退'
                                 }
                             }
                         }
                     },
                     calculable: true,
                     grid: [
                    {
                        borderWidth: 10,
                        borderColor: '#ccc'
                    }
                    ],
                     xAxis: [
                                    {
                                        type: 'category',
                                        boundaryGap: false,
                                        name: '时间段',

                                        data: [],
                                        nameTextStyle:
                                        {
                                          
                                            fontWeight: 'bold',
                                            fontSize: 18
                                        },
                                        axisLine: {
                                            lineStyle: { width: 3,
                                                type: 'solid'
                                            }
                                        },
                                        axisLabel: {
                                          //  interval: 0,
                                            textStyle: {
                                               
                                                fontWeight: 'bold',
                                                fontSize: 12

                                            }
                                        },
                                        splitLine: { show: true }

                                    }
                                ],
                     yAxis: [
                                {
                                    type: 'value',
                                    name: '达成率(%)',
                                    textStyle: {  },
                                    axisLabel: {
                                        formatter: '{value} %'
                                    },
                                    nameTextStyle:
                                        {
                                            
                                            fontWeight: 'bold',
                                            fontSize: 10
                                        },
                                    axisLine: {
                                        lineStyle: { width: 3,
                                            type: 'solid'
                                        }
                                    },
                                    axisLabel: {
                                        interval: 0,
                                        textStyle: {
                                          
                                            fontWeight: 'bold',
                                            fontSize: 10

                                        }
                                    },
                                    splitLine: { show: false }


                                }
                            ],
                     series: [
                                         
                                         ]
                 };


                 // 为echarts对象加载数据
                 myChart.showLoading({
                     text: "图表数据正在努力加载..."
                 });
                myChart.setOption(option);
                WebServiceEchart.GetData_Month("PA",inputSucc, inputFail);
               // setInterval(AutoRefresh, 1000*60);
                function AutoRefresh() {
                    var options = myChart.getOption();
                    myChart.showLoading({
                        text: "图表数据正在努力加载..."
                    });
                    WebServiceEchart.GetData_Month("PA", inputSucc, inputFail);
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
                   // option.title.subtext = ChartItem.Title;
                    // option.xAxis[0].data = ChartItem.XValue;
                    seriesArray = ChartItem.Series;
                    for (var i = 0; i < seriesArray.length; i++) {
                        var item = {
                            name: seriesArray[i].Name,
                            type: "line",
                            data: seriesArray[i].SeriesValues,
                            itemStyle: {
                                normal: {
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

                        option.series.push(item);
                        legendlist.push(seriesArray[i].Name); //把线条的名字放在图例
                        if (i > 3) {
                            option.legend.selected[seriesArray[i].Name] = false;
                        }
                    }
                    option.legend.data = legendlist; //把线条的名字放在图例

                    option.xAxis[0].data = XValue;
                    
                    
                }
                function inputFail(result) {
                    var options = myChart.getOption();
                    myChart.showLoading({
                        text: "图表数据加载失败..."
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

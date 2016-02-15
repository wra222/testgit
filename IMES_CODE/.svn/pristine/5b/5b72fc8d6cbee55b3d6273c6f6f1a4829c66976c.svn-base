<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SMT_OUT_Boss.aspx.cs" Inherits="webroot_aspx_SMT_OUT_Boss" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SMT产出</title>

    <style>
    HTML,BODY,FORM
    {
        height:100%;
    }
   </style>
</head>
<body leftMargin="0" topMargin="0" rightMargin="1">
    <form id="form1" runat="server">
    <div style="width: 100%; height: 100%;">

      <div id="mainsmt" style="width: 100%; height: 100%; margin: 0px auto; position: absolute; top: 0px; left: 0px; border: 1px solid #eee "></div>
      <div id="st"  style="width: 0%; height: 100%; margin: 0px auto; position: absolute; top: 0px; left: 50%; border: 1px solid #eee "></div>
      


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
                 'echarts/chart/gauge' // 使用柱状图就加载bar模块，按需加载
                
            ], DrawEChartAll
            )
        function DrawEChartAll(ec) {
            DrawEChartSMT(ec); //画SMT
//            DrawEChartSA(ec); //画SA
//            DrawEChartFA(ec); //画FA
//            DrawEChartPAK(ec); //画PAK
            
        }
        var SMTChart;
        var SAChart;
        var FAChart;
        var PAKChart;
        //======================================================
        function DrawEChartSMT(ec) {
            SMTChart = ec.init(document.getElementById('mainsmt'));
            //================
           
           option = {
                        tooltip : {
                            formatter: "{a} <br/>{c} {b}"
                        },
                        toolbox: {
                            show : true,
                            feature : {
                                mark : {show: true},
                                restore : {show: true},
                                saveAsImage : {show: true}
                            }
                        },
                        series : [
                            {
                                name:'速度',
                                type:'gauge',
                                z: 3,
                                min:0,
                                max:220,
                                splitNumber:11,
                                axisLine: {            // 坐标轴线
                                    lineStyle: {       // 属性lineStyle控制线条样式
                                        width: 10
                                    }
                                },
                                axisTick: {            // 坐标轴小标记
                                    length :15,        // 属性length控制线长
                                    lineStyle: {       // 属性lineStyle控制线条样式
                                        color: 'auto'
                                    }
                                },
                                splitLine: {           // 分隔线
                                    length :20,         // 属性length控制线长
                                    lineStyle: {       // 属性lineStyle（详见lineStyle）控制线条样式
                                        color: 'auto'
                                    }
                                },
                                title : {
                                    textStyle: {       // 其余属性默认使用全局文本样式，详见TEXTSTYLE
                                        fontWeight: 'bolder',
                                        fontSize: 20,
                                        fontStyle: 'italic'
                                    }
                                },
                                detail : {
                                    textStyle: {       // 其余属性默认使用全局文本样式，详见TEXTSTYLE
                                        fontWeight: 'bolder'
                                    }
                                },
                                data:[{value: 40, name: 'km/h'}]
                            },
                            {
                                name:'转速',
                                type:'gauge',
                                center : ['25%', '55%'],    // 默认全局居中
                                radius : '50%',
                                min:0,
                                max:7,
                                endAngle:45,
                                splitNumber:7,
                                axisLine: {            // 坐标轴线
                                    lineStyle: {       // 属性lineStyle控制线条样式
                                        width: 8
                                    }
                                },
                                axisTick: {            // 坐标轴小标记
                                    length :12,        // 属性length控制线长
                                    lineStyle: {       // 属性lineStyle控制线条样式
                                        color: 'auto'
                                    }
                                },
                                splitLine: {           // 分隔线
                                    length :20,         // 属性length控制线长
                                    lineStyle: {       // 属性lineStyle（详见lineStyle）控制线条样式
                                        color: 'auto'
                                    }
                                },
                                pointer: {
                                    width:5
                                },
                                title : {
                                    offsetCenter: [0, '-30%']      // x, y，单位px
                                },
                                detail : {
                                    textStyle: {       // 其余属性默认使用全局文本样式，详见TEXTSTYLE
                                        fontWeight: 'bolder'
                                    }
                                },
                                data:[{value: 1.5, name: 'x1000 r/min'}]
                            },
                            {
                                name:'油表',
                                type:'gauge',
                                center : ['75%', '50%'],    // 默认全局居中
                                radius : '50%',
                                min:0,
                                max:2,
                                startAngle:135,
                                endAngle:45,
                                splitNumber:2,
                                axisLine: {            // 坐标轴线
                                    lineStyle: {       // 属性lineStyle控制线条样式
                                        color: [[0.2, '#ff4500'],[0.8, '#48b'],[1, '#228b22']], 
                                        width: 8
                                    }
                                },
                                axisTick: {            // 坐标轴小标记
                                    splitNumber:5,
                                    length :10,        // 属性length控制线长
                                    lineStyle: {       // 属性lineStyle控制线条样式
                                        color: 'auto'
                                    }
                                },
                                axisLabel: {
                                    formatter:function(v){
                                        switch (v + '') {
                                            case '0' : return 'E';
                                            case '1' : return 'Gas';
                                            case '2' : return 'F';
                                        }
                                    }
                                },
                                splitLine: {           // 分隔线
                                    length :15,         // 属性length控制线长
                                    lineStyle: {       // 属性lineStyle（详见lineStyle）控制线条样式
                                        color: 'auto'
                                    }
                                },
                                pointer: {
                                    width:2
                                },
                                title : {
                                    show: false
                                },
                                detail : {
                                    show: false
                                },
                                data:[{value: 0.5, name: 'gas'}]
                            },
                            {
                                name:'水表',
                                type:'gauge',
                                center : ['75%', '50%'],    // 默认全局居中
                                radius : '50%',
                                min:0,
                                max:2,
                                startAngle:315,
                                endAngle:225,
                                splitNumber:2,
                                axisLine: {            // 坐标轴线
                                    lineStyle: {       // 属性lineStyle控制线条样式
                                        color: [[0.2, '#ff4500'],[0.8, '#48b'],[1, '#228b22']], 
                                        width: 8
                                    }
                                },
                                axisTick: {            // 坐标轴小标记
                                    show: false
                                },
                                axisLabel: {
                                    formatter:function(v){
                                        switch (v + '') {
                                            case '0' : return 'H';
                                            case '1' : return 'Water';
                                            case '2' : return 'C';
                                        }
                                    }
                                },
                                splitLine: {           // 分隔线
                                    length :15,         // 属性length控制线长
                                    lineStyle: {       // 属性lineStyle（详见lineStyle）控制线条样式
                                        color: 'auto'
                                    }
                                },
                                pointer: {
                                    width:2
                                },
                                title : {
                                    show: false
                                },
                                detail : {
                                    show: false
                                },
                                data:[{value: 0.5, name: 'gas'}]
                            }
                        ]
                    };


                        option.series[0].data[0].value = (Math.random()*100).toFixed(2) - 0;
                        option.series[1].data[0].value = (Math.random()*7).toFixed(2) - 0;
                        option.series[2].data[0].value = (Math.random()*2).toFixed(2) - 0;
                        option.series[3].data[0].value = (Math.random()*2).toFixed(2) - 0;
                      //  SMTChart.setOption(option, true);
                   
                    
           //=============
                    SMTChart.setOption(option);

        }
       
        //======================================================
        
        
    </script>
    </form>
</body>
</html>

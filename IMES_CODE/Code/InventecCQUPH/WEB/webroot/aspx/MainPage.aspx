<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPage.aspx.cs" Inherits="webroot_aspx_MainPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产生效率监控系统</title>
     <style type="text/css">          
    form{margin: 0px;
         
         }
    span
    {
    	font:bold 8pt Verdana;
    	color:White;
    	background-color:Transparent;
    }  
  .alertFont{font-family:"Arial Narrow";font-size:17pt;font-weight:bold;color:white}
 .alert{position:relative;float:left;min-width:1024px;width:100%;_width:expression( document.body.clientWidth < 1024? '1020px':'100%' );margin:5px 0 5px 0;padding:0; repeat-x;height:29px;}	
.iMes_Master_msgbox
{
	font-family:Verdana;
	font-size: 13pt;
	color: Red;
	font-weight: bold;
	border: solid 1px black;
	background: white; 
	overflow:auto;
	height:45px;
	width:95%;
}
     </style>
</head>
<body leftMargin="0" topMargin="0" rightMargin="1" class="form" scroll="yes" 
    style="" >      <script type ="text/javascript" >
                                                          setInterval
("curTime.innerHTML=new Date().toLocaleString()+' 星期'+'日一二三四五六'.charAt(new Date().getDay());", 1000); </script>
<div>
<asp:Image ID="Image1" runat="server" 
        ImageUrl="~/webroot/images/inventec.png" />
</div>
    <form id="form1" runat="server" Class="form" >
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <table id="titleTable"  width="100%" bgcolor="rgb(60,64,67)">
        <tr align="center">
        <td align="left" width="85%">
             <marquee  bgcolor="rgb(60,64,67)"   direction="left" behavior="scroll" 
                     scrollamount="3" scrolldelay="100" style="height: 30px"><font class="alertFont" id="Font2"> 英业达(重庆)生产效率监控系统</font></marquee>          
        </td>
        <td align="right" width="15%" >
            <asp:Label ID="curTime" runat="server" ForeColor="Red" 
                Font-Size="Small"></asp:Label>
        </td>
        </tr>
        </table>
      <table>
   
   <tr>
   <td>
    
   


         <div id="master"  style="height:100%;width:100%">
         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Timer ID="Timer1" runat="server" Interval="1000" ontick="Timer1_Tick">
            </asp:Timer>
      <asp:Chart ID="Chart1" runat="server" BackColor="LightSteelBlue" 
                BackGradientStyle="TopBottom" BackSecondaryColor="White" EnableTheming="False" 
                EnableViewState="True" Width="1350px" Height="500px" align="center">
                 <Titles>
                   <asp:Title Font="微软雅黑, 16pt" Name="Title1" Text="英业达重庆生产效率监控系统">
                  </asp:Title>
                </Titles>
             
          <Series>
                <asp:Series  >
                </asp:Series> 
           </Series>
        <ChartAreas>
          <asp:ChartArea Name="ChartArea1">
        </asp:ChartArea>
       
       </ChartAreas>
</asp:Chart>
</ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </td>
    </tr>
    
    </table>  
    </form>
  

</body>
 <table border="0" width="100%" cellspacing="0" cellpadding="0"  >
 <tr valign="top">
 <td align="center" >	<address>版权所有@2015 英业达重庆制造系统部</address>
 <address><a href="Detail.aspx" target="_blank">明细查询</a></address>	     
</td>
</tr>
</table>

</html>

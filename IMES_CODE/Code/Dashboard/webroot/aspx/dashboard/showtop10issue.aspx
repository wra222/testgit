<%@ Page Language="C#"  MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="showtop10issue.aspx.cs" Inherits="webroot_aspx_dashboard_top10" %>


<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%-- 
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>Top 10 Issue</title>
</head>
<base target="_self"></base>
<body>
   
    

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
<center >
<a id="reload" href="" tyle="display:none">reload</a> 

<div style="background-color: #C0C0C0">
<fieldset id="grpCarton" style="border: thin solid #000000; width: 100%;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="HPIMES TOP Issus 图表" CssClass="iMes_label_13pt"></asp:Label></legend> 
      
 <asp:Chart ID="Chart1" runat="server" Height="500px" Width="1000px"  
       onclick="Chart1_Click">
            <Series>
                <asp:Series Name="Series1">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart> 
     
  <table border="0" width="100%" style="font-family: Tahoma" >
       <tr>
          <td>
              <asp:Label ID="Label1" runat="server" Text="Stage:" CssClass="iMes_label_13pt"></asp:Label>
          </td>
          <td>
              <asp:DropDownList ID="stagelist" runat="server" Height="30px" Width="242px" AutoPostBack="true"
                  onselectedindexchanged="stagelist_SelectedIndexChanged" 
                >
              </asp:DropDownList>
          </td>
       </tr>
       <tr>
          <td>
              <asp:Label ID="Label2" runat="server" Text="Pdline:" CssClass="iMes_label_13pt"></asp:Label>
          </td>
          <td>
              <asp:DropDownList ID="pdlinelist" runat="server" Height="30px" Width="242px" 
                  AutoPostBack="true" onselectedindexchanged="pdlinelist_SelectedIndexChanged">
              </asp:DropDownList>
          </td>
       </tr>
        <tr>
          <td>
              <asp:Label ID="Label3" runat="server" Text="Station:" CssClass="iMes_label_13pt"></asp:Label>
          </td>
          <td>
              <asp:DropDownList ID="stationlist" runat="server" Height="30px" Width="242px" 
                  AutoPostBack="true" onselectedindexchanged="stationlist_SelectedIndexChanged">
              </asp:DropDownList>
          </td>
          <td>
              <asp:Button ID="query" runat="server" Text="GetReport" Enabled="False" 
                  onclick="query_Click" />
          </td>
          
       </tr>
       </table>
        <asp:GridView ID="gvResult" runat="server" GvExtHeight="250px" 
            Width="31%" GvExtWidth="98%" Height="1px" 
            style="top: 0px; left: 1px" BackColor="#DEBA84" BorderColor="#DEBA84" 
        BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" >
      <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
      <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
      <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
      <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
      <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
   </fieldset> 
       </div>
 </center>

</body>
</html>

<script language="javascript" type="text/javascript">
    var limit = "5:00";
    var refresh = 120;
    window.onload = function() {
        refreshtime = '<%=Request["RefreshTime"] %>';
        //alert(refreshtime);
       if (refreshtime!=null) {
           refresh = parseInt(refreshtime) ;
        }
        parselimit = refresh;
        beginrefresh();
    };
    //if (document.images) {
       // var parselimit = limit.split(":")
      //  parselimit = parselimit[0] * 60 + parselimit[1] * 1
       // parselimit = refresh
        
   // }
function beginrefresh(){
    if (!document.images)
      return
  if (parselimit == 1)
  {
      //reload.href = window.location.href;
     // reload.click();
       window.location.reload()
  }
  else {
      parselimit -= 1
      curmin = Math.floor(parselimit / 60)
      cursec = parselimit % 60
      if (curmin != 0)
          curtime = curmin + "分" + cursec + "秒后重刷本页！"
      else
          curtime = cursec + "秒后重刷本页！"
      //window.status = curtime
      document.title = curtime;
      setTimeout("beginrefresh()", 1000)
  }
}
 </script>
 </asp:Content>



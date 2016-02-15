<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="SARepairDashboard.aspx.cs" Inherits="Query_SA_SARepairDashboard" EnableViewState="false" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>
    <script type="text/javascript" src="../../js/jscal2.js"></script>

    
    <script src="../../js/lang/cn.js"></script>
    <script src="../../js/jquery.dateFormat-1.0.js"></script>
    <script type="text/javascript" src="../../js/assets/prettify.js"></script>


    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
 <style type="text/css">
       
        
        .title
        {
            font-size: 18px;
            font-weight: bold;
            color:White;
        }
        th
        {
        	background-color: #000000;
        }
        .titleData
        {
            font-size: 22px;
            font-weight: bold;
            color: #57FEFF;
        }
        .titleSmallData
        {
            font-size: 16px;
             color: #FFF8C6;
        }
        .blackMode
        {
         
         }
       .ctl td{  overflow:hidden;white-space: nowrap;padding:2px;font-size:9px}
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
      
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>        
        <button id="btnRefresh" runat="server" type="button" onclick="" style="display: none" />
             <script language="javascript" type="text/javascript">
                </script>
    </ContentTemplate>    
  </asp:UpdatePanel> 
    <div style=" visibility:hidden">
      <imesquery:cmbdbtype ID="CmbDBType" runat="server" />
       <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
      </div>
        <table id="titleTable"  width="98%"style='background-color:#000000'>
        <tr align="center">
        <td align="left">
            <asp:Label ID="title" font-family="Agency FB" runat="server" ForeColor="Red" Text="修復個人產能報表" 
                Font-Size="XX-Large"></asp:Label>
        </td>

        </tr>
        </table>
        
        <table id="mainTable2" class="iMes_grid_TableGvExt"  style="width: 98%;border-width:0px;height:1px;table-layout:fixed;background-color: #000000;">                                
   
            <th class="style3" style=" color: #FFFF00; font-family: Verdana ">
                RefreshTimes:<asp:Label ID="lbRefreshTime" runat="server" style=" color: #FFFF00; font-family: Verdana "></asp:Label>分钟
            </th>
   <td align="right">
            <asp:Label ID="curTime" runat="server" ForeColor="Red" 
                Font-Size="X-Large"></asp:Label>
        </td>
                       
       </table>
         <table border="0" width="99%" id='TableMain'>
            <tr>
            <td width="50%">
                <fieldset id="Fieldset1" 
                    style="border: thin solid #000000; width: 98%; height: auto;">
                    <legend align="left" style="height :20px">
                        <asp:Label ID="Label111" runat="server" CssClass="iMes_label_13pt" 
                            Text="白班(08:00—20:30)" ForeColor="#000000" Font-Size="14"></asp:Label>
                    </legend>
                       <table class='ctl' style='border-width:0px;height:50%;width:100%;table-layout:fixed;background-color: #FFFF00;'>
                            <tr>  
                                <td align="left" style="font-size: 15px;width: 20%">工号</td>  
                                <td align="left" style="font-size: 15px;width: 20%">Input Qty</td>  
                                <td align="left" style="font-size: 15px;width: 20%">Output Qty</td>  
                                <td align="left" style="font-size: 15px;width: 20%">Repair Qty</td>  
                            </tr> 
                        </table> 
                    <div id="DivDay" style="width:100%; height:200px; overflow:scroll; overflow-x:hidden;"></div>
       <%--             <iMES:GridViewExt ID="gvResult" runat="server" 
                                AutoGenerateColumns="true" GvExtHeight="100px" GvExtWidth="100%" Height="1px" 
                                style="top: 189px; left: 15px" Width="98%">
                            </iMES:GridViewExt>--%>
                </fieldset>
            </td>
            <td width="50%">
                <fieldset id="Fieldset4" 
                    style="border: thin solid #000000; width: 98%; height: auto;">
                    <legend align="left" style="height :20px" >
                        <asp:Label ID="Label1111" runat="server" CssClass="iMes_label_13pt" 
                            Text="夜班（20:30—08：00）" ForeColor="#000000" Font-Size="14"></asp:Label>
                    </legend>
                            <table class='ctl' style='border-width:0px;height:50%;width:100%;table-layout:fixed;background-color: #FFFF00;'>
                            <tr>  
                                <td align="left" style="font-size: 15px;width: 20%">工号</td>  
                                <td align="left" style="font-size: 15px;width: 20%">Input Qty</td>  
                                <td align="left" style="font-size: 15px;width: 20%">Output Qty</td>  
                                <td align="left" style="font-size: 15px;width: 20%">Repair Qty</td>  
                            </tr> 
                        </table> 
                    <div id="DivNight" style="width:100%; height:200px; overflow:scroll; overflow-x:hidden;"></div>
       <%--                 <iMES:GridViewExt ID="gvResult2" runat="server" AutoGenerateColumns="true" 
                                GvExtHeight="100px" GvExtWidth="100%" Height="1px" Width="98%" 
                                style="top: 100px; left: 2px">
                            </iMES:GridViewExt>--%>
                </fieldset>
            </td>
        </tr>
         </table>
        <table   width="98%"style='background-color:#000000'>
        <tr align="center">
            <td align="center">
                <asp:Label ID="Label33" runat="server" ForeColor="yellow" Text="不良原因TOP10" 
                Font-Size="X-Large"></asp:Label>
            </td>
        </table>
        <div id="DivDefect" style="width:98%; height:300px; overflow:scroll; overflow-x:hidden;"></div>
<%--                  <iMES:GridViewExt ID="gvResult3" runat="server" AutoGenerateColumns="true" 
                                GvExtHeight="100px" GvExtWidth="100%" Height="1px" Width="98%" 
                                style="top: 100px; left: 2px">
                            </iMES:GridViewExt>--%>
      <asp:HiddenField ID="hidConnection" runat="server" Value="1" />
    <asp:HiddenField ID="hidDbName" runat="server" Value="" />
    
    <asp:HiddenField ID="hidImgPath" runat="server" Value="1" /> 
     

    <script type="text/javascript">
      var userName="<%=userName %>";
      var userId="<%=userId %>"
      var accountId="<%=accountId %>";
      var customer="<%=customer %>";
      window.onload = function() {

          getTime();
          GetMain();
          // setTimeout('GetMain( )', 10000)
      };

      function getTime() {
          var now = new Date();
          var t = now.format("yyyy-MM-dd HH:mm:ss");
          var curTime = '#' + "<%=curTime.ClientID %>";
          $(curTime).text(t);
          setTimeout('getTime( )', 1000);
      }
      function GetMain() {
          var connection = document.getElementById("<%=hidConnection.ClientID %>").value;

          PageMethods.RefreshTimeAndBindData(connection,onSuccessForMain, onErrorForMain);
      }
      function onSuccessForMain(receiveData) {
          document.getElementById('DivDay').innerHTML = '';
          document.getElementById('DivDay').innerHTML = receiveData[0];
          document.getElementById('DivNight').innerHTML = '';
          document.getElementById('DivNight').innerHTML = receiveData[1];
          var RefreshTime = '#' + "<%=lbRefreshTime.ClientID %>";
          $(RefreshTime).text(receiveData[2]);

          document.getElementById('DivDefect').innerHTML = '';
          document.getElementById('DivDefect').innerHTML = receiveData[3];
      
          setTimeout('GetMain()', parseInt(receiveData[2]) * 60 * 1000);
      }
      function onErrorForMain(error) {
          if (error != null)
              alert(error.get_message());
      }

      var speed = 50;
      function Marquee() {

          var divHight = $("#DivNight").height();
          distanceScrollCount = DivNight.scrollHeight;
          distanceScroll = DivNight.scrollTop;
          if ((distanceScroll + divHight) >= distanceScrollCount) {
              DivNight.scrollTop = 0;
          }
          else {
              DivNight.scrollTop++;
          }

          var divHight1 = $("#DivDay").height();
          distanceScrollCount = DivDay.scrollHeight;
          distanceScroll = DivDay.scrollTop;
          if ((distanceScroll + divHight1) >= distanceScrollCount) {
              DivDay.scrollTop = 0;
          }
          else {
              DivDay.scrollTop++;
          }
          var divHight2 = $("#DivDefect").height();
          distanceScrollCount = DivDefect.scrollHeight;
          distanceScroll = DivDefect.scrollTop;
          if ((distanceScroll + divHight2) >= distanceScrollCount) {
              DivDefect.scrollTop = 0;
          }
          else {
              DivDefect.scrollTop++;
          }



      }
      var MyMar = setInterval(Marquee, speed);

      DivNight.onmouseover = function() {
          clearInterval(MyMar);
      }

      DivNight.onmouseout = function() {
          MyMar = setInterval(Marquee, speed);
      }
      DivDay.onmouseover = function() {
          clearInterval(MyMar);
      }

      DivDay.onmouseout = function() {
          MyMar = setInterval(Marquee, speed);
      }
      DivDefect.onmouseover = function() {
          clearInterval(MyMar);
      }

      DivDefect.onmouseout = function() {
          MyMar = setInterval(Marquee, speed);
      }  
       
    </script>

    </asp:Content>

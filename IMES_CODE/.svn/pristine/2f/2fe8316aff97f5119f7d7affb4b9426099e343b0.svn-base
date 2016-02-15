<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="SMT_Dashboard.aspx.cs" Inherits="Query_SA_SMT_Dashboard" EnableViewState="false" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

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
            <asp:Label ID="title" font-family="Agency FB" runat="server" ForeColor="Red" Text="SMT Dashboard" 
                Font-Size="XX-Large"></asp:Label>
        </td>
        <td align="right">
            <asp:Label ID="curTime" runat="server" ForeColor="Red" 
                Font-Size="X-Large"></asp:Label>
        </td>
        </tr>
        </table>
        
        <table id="mainTable2" class="iMes_grid_TableGvExt"  style="width: 98%;border-width:0px;height:1px;table-layout:fixed;background-color: #000000;">                                
            <tr class="iMes_grid_HeaderRowGvExt ">
            <th class="style16" style=" color: #FFFF00; font-family: Verdana">
                <asp:Label ID="Label2" runat="server" Text="Line：" ></asp:Label>
                <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Always" RenderMode="Inline" Visible="true" >
                        <ContentTemplate>                                                 
                            <asp:DropDownList ID="dropLine" runat="server" Width="100"
                               onchange="GetMain();" />
                        </ContentTemplate>
                </asp:UpdatePanel>
            </th>
            <th class="style7" style=" color: #FFFF00; font-family: Verdana">
                    Station：<asp:label ID="labStation" runat="server" 
                                                style="margin-left: 0px"></asp:label>
            </th>
                
            <th class="style3" style=" color: #FFFF00; font-family: Verdana ">
                RefreshTimes:<asp:Label ID="lbRefreshTime" runat="server" style=" color: #FFFF00; font-family: Verdana "></asp:Label>分钟
            </th>
            <th class="style4" style=" color: #FFFF00; font-family: Verdana">
                    <asp:label ID="labTime" runat="server" Width="111px"></asp:label>
            </th>
            <th class="style5" style=" color: #FFFF00; font-family: Verdana">
                    <input ID="btnSetting" type="button"  value="Setting" onclick="UploadSMT_DashboardSet()"/>
            </th>
            </tr>                         
       </table>
         <table border="0" width="99%" id='TableMain'>
            <tr>
            <td width="50%">
                <fieldset id="Fieldset1" 
                    style="border: thin solid #000000; width: 98%; height: auto;">
                    <legend align="left" style="height :20px">
                        <asp:Label ID="Label111" runat="server" CssClass="iMes_label_13pt" 
                            Text="白班(08:00—20:30)" ForeColor="#000000" Font-Size="13"></asp:Label>
                    </legend>
                    <div id="DivDay"></div>
                            <%--<iMES:GridViewExt ID="gvResult" runat="server" 
                                AutoGenerateColumns="true" GvExtHeight="150px" GvExtWidth="100%" Height="1px" 
                                style="top: 100px; left: 2px" Width="98%">
                            </iMES:GridViewExt>--%>
                </fieldset>
            </td>
            <td width="50%">
                <fieldset id="Fieldset4" 
                    style="border: thin solid #000000; width: 98%; height: auto;">
                    <legend align="left" style="height :20px" >
                        <asp:Label ID="Label1111" runat="server" CssClass="iMes_label_13pt" 
                            Text="夜班（20:30—08：00）" ForeColor="#000000" Font-Size="13"></asp:Label>
                    </legend>
                    <div id="DivNight"></div>
                            <%--<iMES:GridViewExt ID="gvResult2" runat="server" AutoGenerateColumns="true" 
                                GvExtHeight="150px" GvExtWidth="100%" Height="1px" Width="98%" 
                                style="top: 100px; left: 2px">
                            </iMES:GridViewExt>--%>
                </fieldset>
            </td>
        </tr>
         </table>
        <table   width="98%"style='background-color:#000000'>
        <tr align="center">
            <td align="center">
                <asp:Label ID="Label33" runat="server" ForeColor="yellow" Text="不良原因分析" 
                Font-Size="X-Large"></asp:Label>
            </td>
        </table>
        <div id="DivDefect"></div>
       
      <asp:HiddenField ID="hidConnection" runat="server" Value="1" />
    <asp:HiddenField ID="hidDbName" runat="server" Value="" />
    
    <asp:HiddenField ID="hidImgPath" runat="server" Value="1" /> 
     

    <script type="text/javascript">
      var userName="<%=userName %>";
      var userId="<%=userId %>"
      var accountId="<%=accountId %>";;
      var customer="<%=customer %>";
        window.onload = function() {
            
            getTime();
            GetMain();
            // setTimeout('GetMain( )', 10000)
        };
        
        function getTime()
       {
            var now = new Date();
            var t = now.format("yyyy-MM-dd HH:mm:ss");
            var curTime = '#' + "<%=curTime.ClientID %>";
            $(curTime).text(t);
            setTimeout('getTime( )', 1000);
       }
       function GetMain()
       {
        var connection = document.getElementById("<%=hidConnection.ClientID %>").value;
        var line=document.getElementById("<%=dropLine.ClientID %>").value;
        PageMethods.RefreshTimeAndBindData(connection, line, onSuccessForMain, onErrorForMain);
       }
       function onSuccessForMain(receiveData) 
       {
            document.getElementById('DivDay').innerHTML = '';
            document.getElementById('DivDay').innerHTML =receiveData[0];
            document.getElementById('DivNight').innerHTML = '';
            document.getElementById('DivNight').innerHTML = receiveData[1];
            var RefreshTime = '#' + "<%=lbRefreshTime.ClientID %>";
            var station = '#' + "<%=labStation.ClientID %>";
            $(RefreshTime).text(receiveData[2]);
            
            document.getElementById('DivDefect').innerHTML = '';
            document.getElementById('DivDefect').innerHTML =receiveData[3];
            $(station).text(receiveData[4]);
            setTimeout('GetMain()',parseInt(receiveData[2])*60*1000);     
       }    
        function onErrorForMain(error)
        {
             if (error != null)
                alert(error.get_message());
        }
        
        function UploadSMT_DashboardSet() {
        var dbName = document.getElementById("<%=hidDbName.ClientID %>").value;
        var dlgFeature = "dialogHeight:600px;dialogWidth:800px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "../../Query/SA/SMT_DashboardSet.aspx?dbName=" + dbName+"&UserId="+userId+"&Customer="+customer+"&AccountId="+accountId;
        var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
    }
        
        
    </script>






</asp:Content>


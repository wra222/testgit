<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="SAReceiveReturnMBQuery.aspx.cs" Inherits="Query_SAReceiveReturnMBQuery" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
    <script src="../../js/jscal2.js"></script>
    <script src="../../js/lang/cn.js "></script>    

    <script type="text/javascript" src="../../js/jquery.multiselect.js"></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js"></script>     
    <script type="text/javascript" src="../../js/wz_tooltip.js"></script>
    <script type="text/javascript" src="../../js/ui.dropdownchecklist.js"></script>

    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
    <link rel="stylesheet" type="text/css" href="../../css/ui.dropdownchecklist.themeroller.css" />                                                   
    
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
                
        </asp:ScriptManager>
 <center>
    <div>
        <fieldset style="border: solid #000000">
            <legend>SA Receive Return MB Query</legend>
            <table border="0" width="100%" style="border-width:thin;">
            <tr>
                <td width ="10%">
                      <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td>                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>
                <td>
                    
                </td>
                <td colspan="3">
                   
                </td>
            </tr>
            <tr>                    
                <td width="10%">
                    <asp:Label ID="lblDate" runat="server" Text="Date:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="35%">
                   <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
                    <asp:TextBox id="txtStartTime" runat="server" Width="150px" Height="20px"></asp:TextBox>
                    <button id="btnFromDate" type="button" style="width: 20px">...</button>                           
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtEndTime" runat="server" Width="150px" Height="20px"></asp:TextBox>
                    <button id="btnToDate" type="button" style="width: 20px">...</button>
                </td>
                <td width="10%">
                    <asp:Label ID="Label9" runat="server" Text="MB Code:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="10%">
                    <asp:ListBox ID="ddlMBCode" runat="server" SelectionMode="Multiple" Width="150px"  CssClass="CheckBoxList">                     
                    </asp:ListBox>
                </td>
                <td width="10%">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="6" align="center">            
                    <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click"  onclick="beginWaitingCoverDiv();" 
                        style="width: 100px">Query</button>
                    &nbsp;&nbsp;&nbsp;                    
                    <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px">Export</button>
                </td>
            </tr>
        </table>
        </fieldset>
    </div>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>                     
                   <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="390px"
                    Width="98%" GvExtWidth="98%" Height="1px">
                    </iMES:GridViewExt>                     
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:LinkButton ID="lbtFreshPage" runat="server" 
                    OnClientClick="return ReloadImesPage();" onclick="lbtFreshPage_Click"></asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
           
</center>
       <asp:HiddenField ID="hidUser" runat="server" />       
       <asp:HiddenField ID="hidprocess" runat="server" />              
       <asp:HiddenField ID="hidsource" runat="server" />              
    </div>

    <script type="text/javascript">
        var inputObj;
        
        function setCommonFocus() {
            endWaitingCoverDiv();
            inputObj.focus();
            inputObj.select();
            window.onload();
        }
        function EndRequestHandler(sender, args) {
           $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
            
           Calendar.setup({
               inputField: "<%=txtStartTime.ClientID%>",
               trigger: "btnFromDate",
               onSelect: updateCalendarFieldswithSeconds,
               onTimeChange: updateCalendarFieldswithSeconds,
               showTime: 24,
               dateFormat: "%Y-%m-%d 00:00:00",
               minuteStep: 1
           });
           Calendar.setup({
               inputField: "<%=txtEndTime.ClientID%>",
               trigger: "btnToDate",
               onSelect: updateCalendarFieldswithSeconds,
               onTimeChange: updateCalendarFieldswithSeconds,
               showTime: 24,
               dateFormat: "%Y-%m-%d %H:%M:%S",
               minuteStep: 1
           });
       };

       window.onload = function() {
           EndRequestHandler();
           Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
       };                  
       </script>      
</asp:Content>
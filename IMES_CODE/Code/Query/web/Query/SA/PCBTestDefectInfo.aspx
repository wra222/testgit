<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="PCBTestDefectInfo.aspx.cs" Inherits="Query_PCBTestDefect"  %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Register Src="~/CommonControl/CmbPdLine.ascx" TagPrefix="CmbPdLine" TagName="CmbPdLine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <script type="text/javascript" src="../../js/jquery-1.7.1.js "></script>    
    <script type="text/javascript" src="../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js "></script>     
    <script src="../../js/jscal2.js "></script>
    <script src="../../js/lang/cn.js "></script>    

    <script type="text/javascript" src="../../js/jquery.multiselect.js "></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js "></script>     
    <script type="text/javascript" src="../../js/wz_tooltip.js "></script>
        
    
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />      
    <script type="text/javascript">

    </script>                 
                         
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
                
    </asp:ScriptManager>    

 <center>
        <fieldset style="border: solid #000000">
            <legend>PCB Test Defect Info</legend>
            <table border="0" width="100%" style="border-width:thin;">
                <tr>
                    <td width="10%">
                        <asp:Label ID="Label3" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="40%">
                        <iMESQuery:CmbDBType id="CmbDBType" runat="server"  />                                                
                    </td>
                    <td  width="10%">
                        <asp:Label ID="Label4" runat="server" Text="PdLine:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td  width="40%">
                        <asp:ListBox ID="lboxPdLine" runat="server" SelectionMode="Multiple" Height="95%" 
                                    Width="300px" CssClass="CheckBoxList">
                        </asp:ListBox>
                    </td>
                </tr>
            <tr>                                    
                <td width="10%">
                         <asp:Label ID="Label8" runat="server" Text="Date:" CssClass="iMes_label_13pt"></asp:Label>                   
                </td>
                <td width="40%">
                    <asp:Label ID="Label7" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
                    <asp:TextBox id="txtStartTime" runat="server" Width="150px" Height="20px"></asp:TextBox>                           
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtEndTime" runat="server" Width="150px" Height="20px"></asp:TextBox>
                </td>
                <td width="10%">
                    <asp:Label ID="Label10" runat="server" Text="Station:" CssClass="iMes_label_13pt"></asp:Label>                                      
                </td>
                <td width="40%">
                    <asp:ListBox ID="lboxStation" runat="server" SelectionMode="Multiple" Width="150px"  CssClass="CheckBoxList">                     
                    </asp:ListBox>
                </td>   
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Family:" CssClass="iMes_label_13pt"></asp:Label>                   
                </td>
                <td>
                    <asp:DropDownList ID="ddlFamily" runat="server"  Width="220px"
                            onselectedindexchanged="ddlFamily_SelectedIndexChanged" AutoPostBack="true">
                     </asp:DropDownList>        
                </td>
                <td> 
                    <asp:Label ID="Label9" runat="server" Text="MB Code:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td> 
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" >
                        <ContentTemplate>
                            <asp:ListBox ID="lboxModel" runat="server" SelectionMode="Multiple" Width="150px" CssClass="CheckBoxList">
                            </asp:ListBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlFamily" EventName="selectedindexchanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click"  onclick="beginWaitingCoverDiv();" 
                        style="width: 100px">Query</button>
                    &nbsp;&nbsp;&nbsp                   
                    <button id="btnExport" runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px;">Export</button>
                </td>                    
            </tr>            
            </table>
        </fieldset>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="380px"
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


    <script language="javascript" type="text/javascript">
        var inputObj;
        
        
    
        function bind() 
        {
            //beginWaitingCoverDiv();
            //
        }

        function processFun(backData) {
            ShowInfo("");
            beginWaitingCoverDiv();            
            document.getElementById("<%=btnQuery.ClientID%>").click();
        }

        function initPage() {
            clearData();
            inputObj.value = "";
            getAvailableData("processFun");
            inputObj.focus();
        }

        function setCommonFocus() {
            endWaitingCoverDiv();
            inputObj.focus();
            inputObj.select();
            window.onload();
        }

       function EndRequestHandler(sender, args) {

            $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please PdLine ' }).multiselectfilter();

            Calendar.setup({
                inputField: "<%=txtStartTime.ClientID%>",
                trigger: "<%=txtStartTime.ClientID%>",
                onSelect: updateCalendarFields,
                onTimeChange: updateCalendarFields,
                showTime: 24,
                dateFormat: "%Y-%m-%d %H:%M",
                minuteStep: 1
            });
            Calendar.setup({
                inputField: "<%=txtEndTime.ClientID%>",
                trigger: "<%=txtEndTime.ClientID%>",
                onSelect: updateCalendarFields,
                onTimeChange: updateCalendarFields,
                showTime: 24,
                dateFormat: "%Y-%m-%d %H:%M",
                minuteStep: 1
            });
        };
 
        window.onload = function() {
            EndRequestHandler();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        };
    </script>
      

</asp:Content>
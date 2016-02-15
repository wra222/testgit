<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ProductPlanInputQuery_CR.aspx.cs" Inherits="Query_ProductPlanInputQuery_CR" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>


<%@ Import Namespace="com.inventec.iMESWEB" %>

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
    <script type="text/javascript"></script>
                     
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <center>
 
            <fieldset style="border: solid #000000">
                <legend>Product Plan Input Query for CleanRoom</legend>
            
                <table border="0" width="100%" style="border-width:thin;">
                    <tr>
                        <td width ="6%">
                            <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="60%">
                            <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                        </td>
                        <td width="30%">
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDate" runat="server" Text="Date:" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td >
                            <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
                            <asp:TextBox id="txtFromDate" runat="server" Width="140px" Height="20px"></asp:TextBox>                            
                            <button id="btnFromDate" type="button" style="width: 20px">...</button>
                            <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                            <asp:TextBox ID="txtToDate" runat="server" Width="140px" Height="20px"></asp:TextBox>
                            <button id="btnToDate" type="button" style="width: 20px">...</button>
                        </td>
                        <td>
                            <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="Query();" style="width: 100px">Query</button>
                            <input type="hidden" id="hidFromData" runat="server" />
                            <input type="hidden" id="hidToData" runat="server" />
                        </td>            
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="PdLine:" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="lboxPdLine" runat="server" Width="250px"></asp:DropDownList>
                        </td>
                        <td>
                            <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                                            style="width: 100px; display: none;">Export</button>
                        </td>
                    </tr>
                </table>
        
            </fieldset>
  
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>        
                    <asp:HiddenField ID="hidPeriod" runat="server" />
                    <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="420px"
                    Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px">
                    </iMES:GridViewExt>
                    <asp:LinkButton ID="lbtFreshPage" runat="server" 
                        OnClientClick="return ReloadImesPage();" onclick="lbtFreshPage_Click"></asp:LinkButton>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
                </Triggers>
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

            $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select...' }).multiselectfilter();

            //yyyy = year
            //MM = month
            //dd = day
            //hh = hour in am/pm (1-12)
            //HH = hour in day (0-23)
            //mm = minute
            //ss = second
            //a = Am/pm marker


            //<![CDATA[
            //]]>
            $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
            $("#<%=gvQuery.ClientID%> tr:nth-child(n+2) ").click(function() { rowclick(); });

            Calendar.setup({
                trigger: "btnFromDate",
                inputField: "<%=txtFromDate.ClientID%>",
                onSelect: updateCalendarFields,
                onTimeChange: updateCalendarFields,
                showTime: 24,
                dateFormat: "%Y-%m-%d %H:%M",
                minuteStep: 1
            });
            Calendar.setup({
                inputField: "<%=txtToDate.ClientID%>",
                trigger: "btnToDate",
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

            //inputObj = getCommonInputObject();
            //getAvailableData("processFun");
        };

        function Query() {           
            if (document.getElementById("<%=txtFromDate.ClientID %>").value == "") {
                alert('Please input Date'); return;
            }
            
            beginWaitingCoverDiv();
            
            document.getElementById("<%=btnQuery.ClientID%>").click()
        }        
    </script>
    <script type="text/javascript">
          //EndRequestHandler();
    </script>
</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="MaterialUsed.aspx.cs" Inherits="Query_MaterialUsed" %>

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
    
    <script type="text/javascript">

    </script>
                     
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
                
        </asp:ScriptManager>
 <center>
 
        <fieldset style="border: solid #000000">
            <legend>Query Used Materials</legend>
            <table border="0" width="100%" style="border-width:thin;">
            <tr>                                    
                <td width="20%">
                         <asp:Label ID="Label8" runat="server" Text="Query type:" 
                             CssClass="iMes_label_13pt"></asp:Label>                   
                </td>
                <td width="15%">
                    <asp:RadioButtonList ID="rbPeriod" runat="server" 
                       RepeatDirection=Vertical>
                       <asp:ListItem Selected="True" Value="M">by Month</asp:ListItem>
                       <asp:ListItem Value="Q">by Quarter</asp:ListItem>
                    </asp:RadioButtonList>                </td>
                <td width="35%">
                <!--
                    <asp:DropDownList ID="ddlQryMonth_Year" runat="server" Width="150px"></asp:DropDownList>&nbsp;&nbsp;<asp:DropDownList ID="ddlQryMonth_Month" runat="server" Width="150px"></asp:DropDownList>
                    <br />
                    <asp:DropDownList ID="ddlQryQuarter_Year" runat="server" Width="150px"></asp:DropDownList>&nbsp;&nbsp;<asp:DropDownList ID="ddlQryQuarter_Quarter" runat="server" Width="150px"></asp:DropDownList>
                -->
                    <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
                    <asp:TextBox id="txtFromDate" runat="server" Width="140px" Height="20px"></asp:TextBox>                            
                    <button id="btnFromDate" type="button" style="width: 20px">...</button>
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="140px" Height="20px"></asp:TextBox>
                    <button id="btnToDate" type="button" style="width: 20px">...</button> 
                    
                    <!--                
                    <asp:Label ID="lblDateFrom" runat="server" Text=" From" 
                        CssClass="iMes_label_13pt"></asp:Label>           
                        &nbsp;<input type="text" id="txtFromDate" style="width:120px;" readonly="readonly" /><input id="btnSetD1" type="button" value=".." 
                        onclick="showCalendar('txtFromDate')" style="width: 17px" 
                        class="iMes_button"  />
                    
                   <asp:Label ID="lblDateTo" runat="server" Text=" ~ to" 
                        CssClass="iMes_label_13pt"></asp:Label>           
                        &nbsp;<input type="text" id="txtToDate" style="width:120px;" readonly="readonly" /><input id="btnSetD2" type="button" value=".." 
                        onclick="showCalendar('txtToDate')" style="width: 17px" 
                        class="iMes_button"  />&nbsp;&nbsp;
                   -->
                </td>
                <td >
                    <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="Query();"  
                        style="width: 100px">Query</button><input type="hidden" id="hidFromData" runat="server" /><input type="hidden" id="hidToData" runat="server" />
                    <br />
                    <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px;">Export</button>
                </td>   
            </tr>
        </table>
        
        </fieldset>
  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>        
                <asp:HiddenField ID="hidPeriod" runat="server" />
                <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="420px"
                Width="98%" GvExtWidth="98%" Height="1px">
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
                onSelect: function() { this.hide(); },
                dateFormat: "%Y-%m-%d",
                minuteStep: 1
            });
            Calendar.setup({
                inputField: "<%=txtToDate.ClientID%>",
                trigger: "btnToDate",
                onSelect: function() { this.hide(); },
                dateFormat: "%Y-%m-%d",
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
            if (document.getElementById("<%=txtFromDate.ClientID %>").value == "" || document.getElementById("<%=txtToDate.ClientID %>").value == "") {
                alert('Please input Date'); return;
            }
            if ((Date.parse(document.getElementById("<%=txtFromDate.ClientID %>").value.replace('-', '/'))).valueOf() > (Date.parse(document.getElementById("<%=txtToDate.ClientID %>").value.replace('-', '/'))).valueOf()) {
                alert('Please input FromDate with earlier Date'); return;
            }
            beginWaitingCoverDiv();
            //document.getElementById("<%=hidFromData.ClientID %>").value = document.getElementById('txtFromDate').Text;
            //document.getElementById("<%=hidToData.ClientID %>").value = document.getElementById('txtToDate').Text;
            document.getElementById("<%=btnQuery.ClientID%>").click()
        }        
    </script>
    <script type="text/javascript">
          //EndRequestHandler();
    </script>
</asp:Content>
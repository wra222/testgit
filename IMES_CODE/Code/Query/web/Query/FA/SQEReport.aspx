<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="SQEReport.aspx.cs" Inherits="Query_SQEReport" %>

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
            <legend> SQE Report </legend>
            <table border="0" width="100%" style="border-width:thin;">
            <tr valign="top">
                <td width="100px" align="right">
					<asp:Label ID="Label8" runat="server" Text="Query type:" 
                            CssClass="iMes_label_13pt"></asp:Label>
				</td>
				<td width="120px">
					<asp:RadioButtonList ID="rbPeriod" runat="server" 
                        RepeatDirection=Vertical>
                        <asp:ListItem Selected="True" Value="D">by Date</asp:ListItem>
						<asp:ListItem Value="M">by Month</asp:ListItem>
                        <asp:ListItem Value="Q">by Quarter</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
				<td width="90px" align="right">
					Date From:<br />
					To:
				</td>
                <td width="220px" align="left">
                    <asp:TextBox id="txtFromDate" runat="server" Width="140px" Height="20px"></asp:TextBox>                            
                    <button id="btnFromDate" type="button" style="width: 20px">...</button><br />
                    <asp:TextBox ID="txtToDate" runat="server" Width="140px" Height="20px"></asp:TextBox>
                    <button id="btnToDate" type="button" style="width: 20px">...</button> 
                </td>
                <td >
					<button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="Query();"  
                        style="width: 100px">Query</button>
					<input type="hidden" id="hidFromData" runat="server" /><input type="hidden" id="hidToData" runat="server" />
                    <br />
                    <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px;">Export</button>
                </td>   
            </tr>
			<tr valign="top"><td colspan="5"></td></tr>
			<tr valign="top">
				<td width="100px" align="right">
					<asp:Label ID="Label9" runat="server" Text="Material Type:" CssClass="iMes_label_13pt"></asp:Label><br />
				</td>
				<td width="120px">
					<asp:DropDownList ID="ddlMaterialType" runat="server" Width="150px" AutoPostBack="true" 
						onselectedindexchanged="ddlMaterialType_SelectedIndexChanged"></asp:DropDownList>
					</td>
				<td width="90px" align="right">
					KP:
				</td>
				<td colspan="2">
					<asp:DropDownList ID="ddlKP" runat="server" Width="150px" ></asp:DropDownList>
				</td>
			</tr>
        </table>
        
        </fieldset>
  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
</Triggers>
        <ContentTemplate>
                <asp:HiddenField ID="hidPeriod" runat="server" />
                <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="420px"
                Width="98%" GvExtWidth="98%" Height="1px">
                </iMES:GridViewExt>
             </ContentTemplate>
        </asp:UpdatePanel>
           
</center>
		<asp:HiddenField ID="hidUser" runat="server" />       
        <asp:HiddenField ID="hidKp" runat="server" />
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
        };

        function Query() {           
            if (document.getElementById("<%=txtFromDate.ClientID %>").value == "" || document.getElementById("<%=txtToDate.ClientID %>").value == "") {
                alert('Please input Date'); return;
            }
            if ((Date.parse(document.getElementById("<%=txtFromDate.ClientID %>").value.replace('-', '/'))).valueOf() > (Date.parse(document.getElementById("<%=txtToDate.ClientID %>").value.replace('-', '/'))).valueOf()) {
                alert('Please input FromDate with earlier Date'); return;
            }
			
            beginWaitingCoverDiv();
			var kp=document.getElementById("<%=ddlKP.ClientID %>").options[document.getElementById("<%=ddlKP.ClientID %>").selectedIndex].value;
			document.getElementById("<%=hidKp.ClientID %>").value = kp;
            document.getElementById("<%=btnQuery.ClientID%>").click()
        }        
    </script>
</asp:Content>
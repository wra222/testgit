<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ECOModelQuery.aspx.cs" Inherits="Query_ECOModelQuery" Title="Untitled Page" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
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
<style type="text/css">
    tr.clicked
    {
        background-color: white; 
    }
    .querycell
    {
        background-color: yellow;
     
    }
    .querycell:hover , .querycell.clicked
    {
        background-color: Blue;                       
    }
    


</style>   
           
<script type="text/javascript">
    //脚本类型
 </script>
 
 <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
<%--添加 ScriptManager 控件--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<%--    引用UpdatePanel控件，能够实现页面的部分刷新--%>
    <ContentTemplate>    
             <script language="javascript" type="text/javascript">
                </script>
    </ContentTemplate>    
  </asp:UpdatePanel>
  
    
<body>   
    <center> <%--水平居中--%>
    <fieldset id="grpCarton" style="border: thin solid #000000;"><%--定义域--%>
        <legend align ="left" style ="height :20px" ><%--legend 元素为 fieldset 元素定义标题 --%>
            <asp:Label ID="lblTitle" runat="server" Text="ECO Model Query" CssClass="iMes_label_13pt" meta:resourcekey="lblTitleResource1"></asp:Label>
        </legend> 
        <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="5%">
                    <asp:Label ID="lblModelTop" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td width="35%">
                    <input type="text" id="txtModelTop" runat="server" />
                </td>                
                <td width ="5%">&nbsp;</td>
                <td width="35%">&nbsp;</td>
                <td rowspan="3"  >
                    <button id="btnQuery" runat="server" onserverclick="btnQuery_Click" onclick="if(clkQuery())" style="width: 100px">Query</button>
                    <br />
                    <%--<button id="btnExport"  runat="server" onserverclick="btnExport_Click" style="width: 100px; ">Export</button>--%>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                   <asp:Label ID="lblDate" runat="server" Text="StartDate:" CssClass="iMes_label_13pt" meta:resourcekey="lblDateResource1"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt" meta:resourcekey="lblFromResource1"></asp:Label>
                    <asp:TextBox id="txtFromDate" runat="server" Width="140px" Height="20px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                    <button id="btnFromDate" type="button" style="width: 20px">...</button>
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt" meta:resourcekey="lblToResource1"></asp:Label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="140px" Height="20px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                    <button id="btnToDate" type="button" style="width: 20px">...</button> 
                </td>            
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </fieldset> 
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="350px"   align ="left"
            Width="100%" GvExtWidth="100%" Height="1px" ShowFooter="True" 
            style="top: -4px; left: -61px; margin-left: 0px;" AutoHighlightScrollByValue="False" 
            GetTemplateValueEnable="False" HiddenColCount="0" HighLightRowPosition="1" 
            meta:resourcekey="gvResultResource1" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False">            
            </iMES:GridViewExt>        
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="serverclick" />
        </Triggers>
    </asp:UpdatePanel>
    <fieldset id="Fieldset1" style="border: thin solid #000000;"><%--定义域--%>
        <legend align ="left" style ="height :20px" ><%--legend 元素为 fieldset 元素定义标题 --%>
            <asp:Label ID="Label1" runat="server" Text="" CssClass="iMes_label_13pt"></asp:Label>
        </legend> 
        <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="6%">
                    <asp:Label ID="lblECRNO" runat="server" Text="ECR No:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td width="25%">
                    <asp:Label ID="lblECRNOContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td width ="5%">
                    <asp:Label ID="lblModel" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="25%">
                    <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width ="5%">
                    <asp:Label ID="lblPlant" runat="server" Text="Plant:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="25%">
                    <asp:Label ID="lblPlantContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblECONO" runat="server" Text="ECO No:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblECONOContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblStatus" runat="server" Text="Status:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblStatusContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblChangeStatus" runat="server" Text="ChangeStatusTo :" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbChangeStatus" runat="server">
                        <asp:ListItem Text="" Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRemark" runat="server" Text="Remark:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td colspan="5">
                    <input type="text" id="txtRemark" runat="server" style="width:80%" />
                </td>
                <td>
                    <button id="btnSave" runat="server" onserverclick="btnSave_Click" onclick="if(clkSave())" style="width: 100px">Save</button>
                </td>
            </tr>
        </table>
    </fieldset> 
    <input type="hidden" id="hidID" runat="server" />
    <input type="hidden" id="hidInfoType" runat="server" />
    <input type="hidden" id="hidContent" runat="server" />
    <input type="hidden" id="dTableHeight" runat="server" />
</center>   
    <script type="text/javascript">

        function EndRequestHandler(sender, args) {
            $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
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

        $(window).load(function() {
            EndRequestHandler();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        });

        function clkQuery() {
            var model = document.getElementById("<%=txtModelTop.ClientID %>").value;
            var from = document.getElementById("<%=txtFromDate.ClientID %>").value;
            var to = document.getElementById("<%=txtToDate.ClientID %>").value;
            if (model == "" && from == "" && to == "") {
                alert("Please Input Model");
                return false;
            }
            if (model == "") {
                if (from == "" || to == "") {
                    alert("Please Select DateTime");
                    return false;
                }
            }
            return true;
        }

        function clkSave() {
            var changeStatus = document.getElementById("<%=cmbChangeStatus.ClientID %>").value;

            if (changeStatus == "") {
                alert("Please Select changeStatus");
                return false;
            }
            return true;
        }

        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);
        }

        var iSelectedRowIndex = null;
        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gvResult.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gvResult.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }

        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }
            var status = con.cells[7].innerText.trim();
            document.getElementById("<%=lblECRNOContent.ClientID %>").innerText = con.cells[1].innerText.trim();
            document.getElementById("<%=lblECONOContent.ClientID %>").innerText = con.cells[2].innerText.trim();
            document.getElementById("<%=lblModelContent.ClientID %>").innerText = con.cells[3].innerText.trim();
            document.getElementById("<%=lblPlantContent.ClientID %>").innerText = con.cells[5].innerText.trim();
            document.getElementById("<%=lblStatusContent.ClientID %>").innerText = status;
            document.getElementById("<%=cmbChangeStatus.ClientID %>").value = status;
            document.getElementById("<%=txtRemark.ClientID %>").value = con.cells[8].innerText.trim();
            document.getElementById("<%=hidID.ClientID %>").value = con.cells[0].innerText.trim();
            var currentId = con.cells[0].innerText.trim();
            if (currentId == "") {
                setNewItemValue();
                return;
            }
            else {
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
            }
            if (status == "Close") {
                document.getElementById("<%=cmbChangeStatus.ClientID %>").disabled = true;
            }
            else {
                document.getElementById("<%=cmbChangeStatus.ClientID %>").disabled = false;
            }
        }

        function setNewItemValue() {
            
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;

        }
        
    </script>   
</asp:Content>



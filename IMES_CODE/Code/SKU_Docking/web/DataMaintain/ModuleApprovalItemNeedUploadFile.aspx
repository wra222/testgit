<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" 
    CodeFile="ModuleApprovalItemNeedUploadFile.aspx.cs" Inherits="ModuleApprovalItemNeedUploadFile" ValidateRequest="false" %>
    

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div style=" height:99%; width: 95%; margin: 0 auto;">
        <asp:Label ID="lblfamily" runat="server"></asp:Label>
        <fieldset id="FsInput" style="height: 95%; width: 95%;">
            <legend id="LegendInput" style="font-weight: bold; color: Blue">Family List</legend>
                <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                    Visible="true" ChildrenAsTriggers="false">
                    <ContentTemplate>
                        <div id="div2" style="height: 400px">
                            <input id="hidRecordCount" type="hidden" runat="server" />
                            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="98%"
                                RowStyle-Height="20" GvExtWidth="100%" GvExtHeight="390px" Height="382px" AutoHighlightScrollByValue="true"
                                HighLightRowPosition="3"  OnRowDataBound="gd_RowDataBound" EnableViewState="false">
                            </iMES:GridViewExt>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
        </fieldset>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
        </Triggers>                      
        </asp:UpdatePanel>
        <input type="hidden" id="dTableHeight" runat="server" />
    </div>
    <script type="text/javascript">

var iSelectedRowIndex = null;
function setGdHighLight(con) {
    if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
        setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
    }
    setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
    iSelectedRowIndex = parseInt(con.index, 10);
}

function resetTableHeight() {
    //动态调整表格的高度
    var adjustValue = 55;
    var marginValue = 12;
    var tableHeigth = 300;
    //var tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
    try {
        tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
    }
    catch (e) {

        //ignore
    }
    //为了使表格下面有写空隙
    var extDivHeight = tableHeigth + marginValue;
    div2.style.height = extDivHeight + "px";
    document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
    document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
}
</script>
</asp:Content>

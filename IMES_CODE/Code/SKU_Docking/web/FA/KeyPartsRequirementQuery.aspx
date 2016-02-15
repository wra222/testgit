
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" AsyncTimeout="3000" CodeFile="KeyPartsRequirementQuery.aspx.cs" Inherits="PAK_KeyPartsRequirementQuery" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
         <Services>
            
        </Services>
    </asp:ScriptManager>
   <center>
   
    <table border="0" width="95%">
    <tr><td style="width:100%" colspan="5"> &nbsp; </td></tr>
   
    <tr>
        <td><asp:Label ID="lblFile" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></td>
        <td style="width:95%" colspan="4">
            <iframe name="action" id="action" src="KeyPartsRequirementQuery_Upload.aspx"  scrolling="no"  frameborder="0" width="100%" height="30px"></iframe>
        </td>
    </tr>
    <tr><td colspan="5"><hr></td></tr>
    </table>
    
    <table border="0" width="95%">
    <tr>
        <td style="width:40%">
            <asp:Label ID="lblKeyPartList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
        </td>
        <td style="width:15%">
            <input id="toExcel" type="button" style="width:100%" runat="server" class="iMes_button" 
               onclick="" onserverclick="excelClick" onmouseover="this.className='iMes_button_onmouseover'" 
               onmouseout="this.className='iMes_button_onmouseout'"/>
        </td>
        <td style="width:15%">
            <input id="refresh" type="button" style="width:100%" runat="server" class="iMes_button" 
               onclick="clkRefresh()" onmouseover="this.className='iMes_button_onmouseover'" 
               onmouseout="this.className='iMes_button_onmouseout'"/>
        </td>
        <td style="width:1%">&nbsp;</td>
        <td style="width:29%">
            <asp:Label ID="lblModelList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
        </td>
    </tr>
    
    <tr>
        <td colspan="3">
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
            <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnUploadOver" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
	            <iMES:GridViewExt ID="gdleft" runat="server" AutoGenerateColumns="true" AutoHighlightScrollByValue="true" 
                        GetTemplateValueEnable="False" GvExtHeight="250px" Height="250px" 
                        GvExtWidth="100%" OnGvExtRowClick=""
                        OnGvExtRowDblClick="" SetTemplateValueEnable="False" 
                        HighLightRowPosition="1" HorizontalAlign="Left"
                        onrowdatabound="gdleft_RowDataBound">                                     
                </iMES:GridViewExt>                                                                      
            </ContentTemplate>   
        </asp:UpdatePanel>
        </td>
        <td>&nbsp;</td>
        <td>
        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" >
            <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnUploadOver" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
	            <iMES:GridViewExt ID="gdright" runat="server" AutoGenerateColumns="true" AutoHighlightScrollByValue="true" 
                        GetTemplateValueEnable="False" GvExtHeight="250px" Height="250px" 
                        GvExtWidth="100%" OnGvExtRowClick=""
                        OnGvExtRowDblClick="" SetTemplateValueEnable="False" 
                        HighLightRowPosition="1" HorizontalAlign="Left"
                        onrowdatabound="gdright_RowDataBound">
                </iMES:GridViewExt>                                                                      
            </ContentTemplate>   
        </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Always">
                <ContentTemplate>
                    <input id="btnUploadOver" type="button" onclick="" onserverclick="uploadOver" style="display:
                        none" runat="server" />
                    <input type="hidden" runat="server" id="hidFileName"/>
                    <button id="btnGridClear" runat="server" type="button" onclick="" style="display:none" onserverclick="clearGrid"></button>
                </ContentTemplate>   
            </asp:UpdatePanel> 
        </td>
    </tr>
    </table>
  
        
    </center>
</div>
<script type="text/javascript">

    var errorMessage;
    var index = 0;
    var strRowsCount = "<%=fullRowCount%>";
    var initRowsCount = parseInt(strRowsCount, 10) + 1;
    var leftindex = 1;
    var rightindex = 1;
    var station;
    var editor;
    var customer;
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var emptyPattern = /^\s*$/;




    document.body.onload = function() {
        station = "<%=Station%>";
        editor = "<%=UserId%>";
        customer = "<%=Customer%>";
    }

    function clkRefresh() {
        beginWaitingCoverDiv();
        document.getElementById("<%=btnGridClear.ClientID%>").click();
    }

    function clearTable() {
        ClearGvExtTable("<%=gdleft.ClientID%>", initRowsCount);
        //表头是第0行，数据是第1行，因此index=1，表示从第一行还是添加
        leftindex = 1;

        ClearGvExtTable("<%=gdright.ClientID%>", initRowsCount);
        rightindex = 1;
    }

    function finish(param) {
        document.getElementById("<%=hidFileName.ClientID%>").value = param;
        ShowInfo("");
        clearTable();
        document.getElementById("<%=btnUploadOver.ClientID%>").click();
    }

    function AddRowInfo(RowArray) {
        if (index < initRowsCount) {
            eval("ChangeCvExtRowByIndex_" + GridViewExt1ClientID + "(RowArray,false, index)");
        } else {
            eval("AddCvExtRowToBottom_" + GridViewExt1ClientID + "(RowArray,false)");
        }
        index++;
        setSrollByIndex(index, false);
    }
    
    
</script>
</asp:Content>
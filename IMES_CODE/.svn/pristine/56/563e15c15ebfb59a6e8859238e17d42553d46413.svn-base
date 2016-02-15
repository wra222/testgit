<%@ Page Language="C#" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" 
    CodeFile="DefectComponentRejudge_Query.aspx.cs" Inherits="DefectComponentRejudge_Query" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>DefectComponentRejudge_Query</title>   
</head>

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>




<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        
    </asp:ScriptManager>
    
<script language="JavaScript">

    var parentParams;
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    var line;
    var editor;
    var customer;
    var initRowsCount = 10;
    var index = 1;
    var GridViewExt1ClientID = "<%=gridview.ClientID%>";

    document.body.onload = function() {
        parentParams = window.dialogArguments;
        customer = parentParams[0];
        clearTable();
        
    }

    function clearTable() {
        ClearGvExtTable("<%=gridview.ClientID%>", initRowsCount);
        //表头是第0行，数据是第1行，因此index=1，表示从第一行还是添加
        index = 1;
    }

    function updateTable(infos, len) {
        for (var i = 0; i < len; i++) {
            var rowInfo = new Array();
            rowInfo.push(infos[i]);
            AddRowInfo(rowInfo);
        }
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

    function Query() {
        var partSN = document.getElementById("<%=txtPartSN.ClientID %>").value;
        if (partSN == "" || partSN == null) {
            alert("請輸入PartSN");
            return false;
        }
        document.getElementById("<%=hidcustsn.ClientID %>").value = customer;
        return true;
        //PageMethods.DefectComponentRejudgeQuery(partSN, onSucceed, onFail);
    }

    function onSucceed(result) {
        try {
            if (result == null) {
                alert("Failed");
            }
            else if (result[0] == SUCCESSRET) {                              
                updateTable(result[1], result[2]);
            }
            else {
                var content = result[0];
                alert(content);
            }
        } catch (e) {
            alert(e.description);
        }
    }

    function onSysError(error) {
        alert(error.get_message());        
    }

    function onFail(error) {
        try {
            onSysError(error);
        } catch (e) {
            alert(e.description);
        }
    }

</script>     

    
    <div>
        <table cellpadding="0px" cellspacing="5px" border="0" width = "100%" height="100%">
        <tr>
            <td>
                <asp:Label ID="lblPaetSN" runat="server" Text="PartSN:"></asp:Label>
                <input type="text" id="txtPartSN" runat="server" class="iMes_input_White" style="width: 80%;"/>
                <input id="btquery" type="button" runat="server" class="iMes_button" value="Query"
                           onclick="if(Query())" onmouseover="this.className='iMes_button_onmouseover'" 
                           onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnQuery_ServerClick"/>
            </td>
        </tr>
        <tr>
	        <td>
	         <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btquery" EventName="ServerClick" />
                        
                </Triggers>
                <ContentTemplate>
                    <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="true" AutoHighlightScrollByValue="true" 
                            GetTemplateValueEnable="False" GvExtHeight="240px" Height="230px" 
                            GvExtWidth="100%" Width="99.9%" 
                            SetTemplateValueEnable="true"    HighLightRowPosition="1" 
                            onrowdatabound="GridViewExt1_RowDataBound" HorizontalAlign="Left" >
                    </iMES:GridViewExt>                                                                 
                </ContentTemplate>   
            </asp:UpdatePanel>
	        </td>
         </tr>
        </table>
    </div>                    
       
    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
        <ContentTemplate>
             <input type="hidden" runat="server" id="stationHidden" />
             <input type="hidden" runat="server" id="pCodeHidden" />
             <input type="hidden" runat="server" id="firstLineMode" />
             <input type="hidden" runat="server" id="hidcustsn" />
        </ContentTemplate>   
    </asp:UpdatePanel> 
    </form>
    
     
</body>
</html>



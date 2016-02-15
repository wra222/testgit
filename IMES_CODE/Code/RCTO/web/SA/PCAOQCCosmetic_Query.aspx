
<%@ Page Language="C#" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" 
    CodeFile="PCAOQCCosmetic_Query.aspx.cs" Inherits="PCAOQCCosmeticQuery" Title="iMES-OQC Cosmetic Query"%>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
</head>

<body style=" position: relative; width: 100%">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
          <asp:ServiceReference Path="Service/WebServicePCAOQCCosmetic.asmx" />
        </Services>
    </asp:ScriptManager>   

    <div>
        <table style="table-layout:fixed;">
        <tr>
	        <td>
	            <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
	            <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="hidbtn" EventName="ServerClick" />
                </Triggers>
                <ContentTemplate>
                <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true" GvExtWidth="100%"  GvExtHeight="240px" 
                  Width="99%" Height="230px"
                SetTemplateValueEnable="true"  GetTemplateValueEnable="true"  HighLightRowPosition="3" 
               onrowdatabound="GridViewExt1_RowDataBound" HorizontalAlign="Left" >
                    <Columns>
                        <asp:BoundField DataField="LotNo"  />
                        <asp:BoundField DataField="PCBNo"  />
                        <asp:BoundField DataField="Editor"  />
                        <asp:BoundField DataField="Cdt"  />
                    </Columns>
                 </iMES:GridViewExt>
                 </ContentTemplate>
                 </asp:UpdatePanel>
	        </td>
         </tr>
         <tr><td><button id="hidbtn" style="width: 0; display: none;" runat="server" onserverclick="hidbtn_ServerClick"></button>
         </td></tr>
         <tr>
           
            <td align="right">
            <input id="btnCancel" type="button" style="width:15%" runat="server" class="iMes_button" 
               onclick="cancel()" onmouseover="this.className='iMes_button_onmouseover'" 
               onmouseout="this.className='iMes_button_onmouseout'"/></td>
        </tr>
        </table>
    </div>                    
       
    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
        <ContentTemplate>
             
        </ContentTemplate>   
    </asp:UpdatePanel> 
    </form>
    
     
</body>
</html>

<script language="JavaScript">

    var parentParams;
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";

    var line;
    var editor;
    var customer;
    var station;

    var initRowsCount = 10;
    var index = 1;
    var GridViewExt1ClientID = "<%=gridview.ClientID%>";

    document.body.onload = function() {
        parentParams = window.dialogArguments;
        line = parentParams[0];
        editor = parentParams[1];
        customer = parentParams[2];
        station = parentParams[3];

        clearTable();
        //beginWaitingCoverDiv();

        document.getElementById("<%=hidbtn.ClientID %>").click();
        
    }

    function clearTable() {
        ClearGvExtTable("<%=gridview.ClientID%>", initRowsCount);
        //表头是第0行，数据是第1行，因此index=1，表示从第一行还是添加
        index = 1;
    }

    function updateTable(infos) {
        var length = infos[0].length;
        var curDay = new Date();
        alert(curDay.getMinutes().toString() + ":" + curDay.getSeconds().toString());
        for (var i = 0; i < length; i++) {
            var rowInfo = new Array();
            rowInfo.push(infos[0][i]);
            rowInfo.push(infos[1][i]);
            rowInfo.push(infos[2][i]);
            rowInfo.push(infos[3][i]);
            AddRowInfo(rowInfo);
        }
        var curDay1 = new Date();
        alert(curDay1.getMinutes().toString() + ":" + curDay1.getSeconds().toString());
    }

    function AddRowInfo(RowArray) {
        if (index < initRowsCount) {
            eval("ChangeCvExtRowByIndex_" + GridViewExt1ClientID + "(RowArray,false, index)");
        } else {
            eval("AddCvExtRowToBottom_" + GridViewExt1ClientID + "(RowArray,false)");
        }
        index++;
        //setSrollByIndex(index, false);
    }

    function onSucceed(result) {
        try {
            if (result == null) {
                alert("Failed");
            }
            else if (result[0] == SUCCESSRET) {
                updateTable(result[1]);
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

    function cancel() {
        window.close();
        return;
    }

</script>  


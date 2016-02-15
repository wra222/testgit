<%--
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/FA/ITCNDCheck Page
 * UI:CI-MES12-SPEC-FA-UI ITCNDCheck.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC ITCNDCheck.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-11   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* Check Item
* 
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
--%>
<%@ Page Language="C#" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" 
    CodeFile="ITCNDCheck_Query.aspx.cs" Inherits="_ITCNDCheckQuery" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Pass Qty On This Work Day</title>   
</head>

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>




<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
          <asp:ServiceReference Path="Service/WebServiceITCNDCheckQuery.asmx" />
        </Services>
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
        line = parentParams[0];
        editor = parentParams[1];
        customer = parentParams[2];
        station = parentParams[3];

        clearTable();
        //beginWaitingCoverDiv();
        WebServiceITCNDCheckQuery.ITCNDQuery(line, station, editor, customer, onSucceed, onFail);
    }

    function clearTable() {
        ClearGvExtTable("<%=gridview.ClientID%>", initRowsCount);
        //表头是第0行，数据是第1行，因此index=1，表示从第一行还是添加
        index = 1;
    }

    function updateTable(infos, len) {
        for (var i = 0; i < len; i=i+2) {
            var rowInfo = new Array();
            rowInfo.push(infos[i]);
            rowInfo.push(infos[i+1]);
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
                <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true" GvExtWidth="100%"  GvExtHeight="240px" 
                  Width="99.9%" Height="230px"
                SetTemplateValueEnable="true"  GetTemplateValueEnable="true"  HighLightRowPosition="3" 
               onrowdatabound="GridViewExt1_RowDataBound" HorizontalAlign="Left" >
                    <Columns>
                        <asp:BoundField DataField="Model"  />
                        <asp:BoundField DataField="Pass Qty"  />
                    </Columns>
                 </iMES:GridViewExt>
	        </td>
         </tr>
        </table>
    </div>                    
       
    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
        <ContentTemplate>
             <input type="hidden" runat="server" id="stationHidden" />
             <input type="hidden" runat="server" id="pCodeHidden" />
             <input type="hidden" runat="server" id="firstLineMode" />
        </ContentTemplate>   
    </asp:UpdatePanel> 
    </form>
    
     
</body>
</html>



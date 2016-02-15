<%--
 INVENTEC corporation (c)2012 all rights reserved. 
 Description: Common FileUpLoad control
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2011-11-02  Wang,ShaoHua         Create 

 Known issues:
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbUpLoadFile.ascx.cs"
    Inherits="CommonControl_CmbUpLoadFile" %>
<table style="width: 100%; margin-left: 0px">
    <tr>
        <asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <td style="width: 87.5%;">
                    <asp:FileUpload ID="txtBrowse" name="txtBrowse" Style="width: 100%;" runat="server"
                        onkeypress='return OnKeyPress(this)' />
                </td>
            </ContentTemplate>
        </asp:UpdatePanel>
        <td align="center" width="12.5%">
            <input type="button" id="btnFileUpload" name="btnFileUpload" runat="server" class="iMes_button"
                onclick="if(checkFileSize())" />
        </td>
    </tr>
</table>
<input type="hidden" id="hidFileMaxSize" runat="server" />

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

<script language="javascript" type="text/javascript">
    var msgExceedMaxSize = "<%=msgExceedMaxSize %>";
    var msgNoFile = '<%=msgNoFile%>';
    var msgInvalidFileType = '<%=msgInvalidFileType %>';

    function getObjectFileUpload() {
        return document.getElementById("<%=txtBrowse.ClientID %>");
    }
    function getObjectFielButton() {
        return document.getElementById("<%=btnFileUpload.ClientID %>");
    }
    function UploadComplete(strFileName) {
        window.returnValue = strFileName;
        window.close();

    }

    function OnKeyPress(obj) {
        var key = event.keyCode;

        if (key == 13)//enter
        {
            event.cancelBubble = true;
            return false;

        }
    }

    function btnCancel_Click() {
        window.returnValue = null;
        window.close();
    }
    var maxFileSize;
    function checkFileSize() {
        var maxFileSize = document.getElementById("<%=hidFileMaxSize.ClientID %>").value;
        var filePath = document.getElementById("<%=txtBrowse.ClientID %>").value;
        var extend = filePath.substring(filePath.lastIndexOf(".") + 1);
        if (extend == "") {
        } else {
            if (!((extend == "xls" || extend == "xlsx"))) {
                alert(msgInvalidFileType);
                return false;
            }
        }

        if (ShowSize(filePath) == "false") return false;
        if (ShowSize(filePath) >= parseFloat(maxFileSize)) {
            alert(msgExceedMaxSize + maxFileSize + "M.");
            return false;
        }
        return true;
    }

    function ShowSize(fileName) {
        var fso;
        var f;
        fso = new ActiveXObject("Scripting.FileSystemObject");
        if (fso.FileExists(fileName)) {
            f = fso.GetFile(fileName);
            return parseFloat(f.size / 1024 / 1024);
        } else {
            alert(msgNoFile);
            return "false";
        }

    }
</script>


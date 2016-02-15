<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BTOceanOrderUploadFile.aspx.cs"
    Inherits="DataMaintain_BTOceanOrderUploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Please Choose excel File!</title>
    <base target="_self"></base>
</head>

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

<script language="javascript" type="text/javascript">
    var msgExceedMaxSize = "<%=msgExceedMaxSize %>";
    var msgNoFile = '<%=msgNoFile%>';
    var msgInvalidFileType = '<%=msgInvalidFileType %>';

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
    function uploadFile() {
        var strFileName = document.getElementById("<%=hidFileName.ClientID %>").value;
        if (strFileName != "") {
            window.returnValue = strFileName;
            window.close();
        }
    }
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

<body scroll="no">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    &nbsp&nbsp PdLine:<asp:Label ID="lblPDline" runat="server" Text="Label"></asp:Label>
    <div align="center" style="height: 350px; width: 424px">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:FileUpload ID="txtBrowse" name="txtBrowse" Style="width: 80%; margin-top: 50px;"
                    runat="server" onkeypress='return OnKeyPress(this)' />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnFileUpload" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <br />
        <table width="100%">
            <tr>
                <td align="center">
                    <input type="button" id="btnFileUpload" name="btnFileUpload" runat="server" class="iMes_button"
                        onclick="if(checkFileSize())" onserverclick="Upload_ServerClick" />
                </td>
                <td align="center">
                    <input type="button" id="cancel" name="cancel" runat="server" class="iMes_button"
                        onclick="btnCancel_Click()" />
                </td>
            </tr>
        </table>
        <input type="hidden" id="hidFileMaxSize" runat="server" />
        <input type="hidden" id="hidFileName" runat="server" />
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="COAReceivingUploadFile.aspx.cs" Inherits="DataMaintain_COAReceivingUploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <base target="_self"></base>
</head>

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
<script language="javascript" type="text/javascript">
    
    var msgInvalidFileType = '<%=msgInvalidFileType %>';
    var msgEmptyFile='<%=msgEmptyFile %>';
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
    
    function uploadFile() {
        if (checkFileSize()) {
           var strFileName = document.getElementById("<%=hidFileName.ClientID %>").value;
            if (strFileName != "") {
//                window.returnValue = strFileName;
//                window.close();
                document.getElementById("<%=btnFileUpload.ClientID %>").disable=true;
                return true;
            }
        }
        else
        {
            return false;
        }
    }
    function closeDialog()
    {
        window.returnValue = document.getElementById("<%=hidFileName.ClientID %>").value;
        window.close();
    }
    
    function checkFileSize() {
        
        var filePath = document.getElementById("<%=txtBrowse.ClientID %>").value;
        if(filePath=="")
        {
            alert(msgEmptyFile);
            return false;
        }
        var extend = filePath.substring(filePath.lastIndexOf(".") + 1);
        if (extend == "")
        {
            alert(msgInvalidFileType);
            return false;
        } 
        else {
            if (!((extend.toLowerCase() == "txt"))) {
                alert(msgInvalidFileType);
                return false;
            }
        }
        return true;
    }
    
    function SelectFile() {
        document.getElementById("hidFileName").value = document.getElementById("txtBrowse").value;
    }

    
</script>

<body scroll="no">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" style="height: 200px; width: 424px">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:FileUpload ID="txtBrowse" name="txtBrowse" Style="width: 80%; margin-top: 50px;"
                    runat="server" onkeypress='return OnKeyPress(this)' onchange="SelectFile()" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnFileUpload" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <br />
        <br />
        <table width="100%">
            <tr>
                <td align="center">
                    <input type="button" id="btnFileUpload" name="btnFileUpload" runat="server" class="iMes_button"
                        onclick="if(uploadFile())" onserverclick="Upload_ServerClick"/>
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


<%@ Page Language="C#" AutoEventWireup="true" CodeFile="errorpage.aspx.cs" Inherits="webroot_aspx_dashboard_errorpage" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    </div>
    </form>

    <table class="style1">
        <tr>
            <td width="10%">
                &nbsp;</td>
            <td height="60" width="80%">
                &nbsp;</td>
            <td width="10%">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
               
                <div style="text-align: left; width:100%" NOWRAP>
                <label id="prompttitle" 
                    style="font-family: 'Times New Roman', Times, serif; font-size: medium" >Find error:</label>
                <label id="idPrompt" 
                    style="font-family: 'Times New Roman', Times, serif; font-size: medium" > Invalid logon user </label>
                    
                </div>               
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
</body>
<script language ="javascript" type="text/javascript">
var errorMsg = changeNull('<%=Request.Params["errorMsg"]%>');
function changeNull(val)
{
    if ("undefined" == val || null == val)
    {
        val = "";
    }
    return val;
}
document.getElementById("idPrompt").innerHTML=errorMsg;
</script>

</html>

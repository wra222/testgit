<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_datasource_deleteDataServerAndDatabase, App_Web_deletedataserveranddatabase.aspx.af99fe74" theme="MainTheme" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Delete database server connection</title>
</head>
<fis:header id="header2" runat="server"/>
<body style="margin:5px" class="dialogBody">
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
    <table align=center>
        <tr>
            <td colspan=3 >&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;&nbsp;<img src="../../images/question.GIF"/></td>
            <td>&nbsp;</td>
            <td align=center style="word-wrap:break-word" id="td1"></td>
        </tr>
        <tr>
            <td colspan=3 >&nbsp;</td>
        </tr>
        <tr>
            <td colspan=3 align=center>
                <table align=center>
                    <tr>
                        <td align=right>
                            <button id="yes" onclick="fYes()">Yes</button>
                        </td>
                        <td align=center>
                            <button id="no" onclick="fNo()">No</button>
                        </td>
                        <td align=left>
                            <button id="cancel" onclick="fCancel()">Cancel</button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        
    </table>
</body>
<fis:footer id="footer1" runat="server"/> 
</html>
<script language=javascript>
    var rtnValue = "";
    window.returnValue = "";
    
    document.body.onload=function()
    {
        document.getElementById("td1").innerText = " Are you sure you want to delete the Database server connection ["+ changeNull("<%= Request.Params[0]%>") +"]?";
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: fYes btn
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%> 
    function fYes()
    {
        window.returnValue = "0";
        window.close();
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: fNo btn
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>       
    function fNo()
    {
        window.close();
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: fCancel btn
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>          
    function fCancel()
    {
        window.close();
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ת�����ַ���
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>          
    function changeNull(val)
    {
        if (undefined == val || null == val)
        {
            val = "";
        }
        
        return trimString(val);
    }	    
</script>

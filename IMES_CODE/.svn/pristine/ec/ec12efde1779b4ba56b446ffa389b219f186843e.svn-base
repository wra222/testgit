<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DetialShow.aspx.cs" Inherits="Query_SA_DetialShow" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript" language="javascript">
var scanVal = "";
 



function OK_onClick()
{
    this.window.close();
     
}

 

</script>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" padding:20 20px 0  ">
    <asp:Label ID="Title" runat="server" style=" direction:ltr; padding-left:45%;"></asp:Label>
        <asp:GridView ID="gvQuery" runat="server" 
            onpageindexchanging="gvQuery_PageIndexChanging" BackColor="#DEBA84" 
            BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
            CellSpacing="2" >
            <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
            <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
            <PagerSettings FirstPageText="第一頁" LastPageText="最後頁" Mode="NextPreviousFirstLast"
                            NextPageText="下一頁" Position="TopAndBottom" PreviousPageText="上一頁" />
        </asp:GridView>
        <div style=" direction:ltr; padding-left:45%;">
    <button id="OK" onclick="OK_onClick()" onkeydown="event.returnValue = false;"  class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
     <%=Resources.iMESGlobalDisplay.ResourceManager.GetString(pre + "_btnOK")%>
     </button>
<%--     <button id="btnExport" runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px;">Export</button>
                        </div>--%>
    </div>
    </form>
</body>
</html>

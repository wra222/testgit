<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MACRangeInfo.aspx.cs" Inherits="DataMaintain_MacRangeInfo" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

<head id="Head1" runat="server">
    <title>iMES-MAC Range Info</title>

    <script type="text/javascript">    
      
        var isWindowLoad=false;
        var isMessageOk=false;
        var msgString="";
        function btnOK_Click() 
        {
            window.returnValue = null;
            window.close();         
        }
        
        function page_load()
        {
            isWindowLoad=true;
            DealShowMsg();
        }
        
        function ToShowMessage(msg)
        {
            isMessageOk=true;
            msgString=msg;
            DealShowMsg();
//            alert(msg);
        }
        
        function DealShowMsg()
        {
             if(isWindowLoad==true && isMessageOk==true)
             {
                ShowMessage(msgString);
             }
        }

    </script>
</head>
<body style="background-color: #ECE9D8" onload="page_load()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
        <div style="height:16px; width: 273px;">
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    <div style="width:100%; text-align :center">
        <table style="width:90%;" >
              <tr style="height :35px">
                <td style="width:5%"></td>
                <td style="width:55%">
                    <asp:Label ID="lblMACRangeCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td style="width:45%">
                    <asp:Label ID="MACRangeCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
            </tr>
             <tr style="height :35px">
                <td style="width:5%"></td>
                <td style="width:55%">
                    <asp:Label ID="lblMACTotal" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td style="width:45%">
                    <asp:Label ID="MACTotal" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
            </tr>
            <tr style="height :35px">
                <td style="width:5%"></td>
                <td style="width:55%">
                    <asp:Label ID="lblMACUsed" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td style="width:45%">
                    <asp:Label ID="MACUsed" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
            </tr>
            
            <tr style="height :35px">
                <td style="width:5%"></td>
                <td style="width:55%">
                    <asp:Label ID="lblMACleft" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td style="width:45%">
                    <asp:Label ID="MACleft" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
            </tr>
            <tr style="height :35px">
                <td style="width:5%"></td>
                <td style="width:55%">
                    <asp:Label ID="lblHDCPQuery" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td style="width:45%">
                    <asp:Label ID="HDCPQuery" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
            </tr>
                                                                       
        </table>
    </div>
    
    <div style="height:24px; width: 273px;">
    </div>
        
    <div style="width: 100%">
        <table style="width: 80%">
        <tr>
        <td align="right">            
            <input type="button" id="btnOK" value="OK" style="width:90px" onclick="return btnOK_Click()" runat="server" />
        </td>
         </tr>   
        </table>
    </div>

    </form>
</body>
</html>


<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbPdLine
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbPdLineForMaintain.ascx.cs" Inherits="CommonControl_CmbPdLineForMaintain" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpPDLine" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine") %>';;
        
    function getPdLineCmbObj()
    {    
       return document.getElementById("<%=drpPDLine.ClientID %>");   
    }
    
    function getPdLineCmbText()
    {
        if (document.getElementById("<%=drpPDLine.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
           return document.getElementById("<%=drpPDLine.ClientID %>")[document.getElementById("<%=drpPDLine.ClientID %>").selectedIndex].text;
        }
    }
    
    function getPdLineCmbValue()
    {
       return document.getElementById("<%=drpPDLine.ClientID %>").value;  
    }
    
    function checkPdLineCmb()
    {
        if (getPdLineCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }
    
    function setPdLineCmbFocus()
    {
        document.getElementById("<%=drpPDLine.ClientID %>").focus();   
    }    
                
</script>


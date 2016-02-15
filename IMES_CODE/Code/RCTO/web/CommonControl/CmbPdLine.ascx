<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbPdLine
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-05  Tong.Zhi-Yong         Create    
 * 2010-01-14  Yuan XiaoWei          Modify All javascript function
 * 2010-01-15  Yuan XiaoWei          Modify ITC-1103-0100
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbPdLine.ascx.cs" Inherits="CommonControl_CmbPdLine" %>
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


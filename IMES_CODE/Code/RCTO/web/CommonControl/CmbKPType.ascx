<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbKPType
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-09  Tong.Zhi-Yong         Create   
 * 2010-01-14  Yuan XiaoWei          Modify All javascript function         
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbKPType.ascx.cs" Inherits="CommonControl_CmbKPType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpKPType" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getKPTypeCmbObj()
    {    
         return document.getElementById("<%=drpKPType.ClientID %>");   
    }
    
    function getKPTypeCmbText()
    { 
        if (document.getElementById("<%=drpKPType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpKPType.ClientID %>")[document.getElementById("<%=drpKPType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getKPTypeCmbValue()
    {
         return document.getElementById("<%=drpKPType.ClientID %>").value;
    }
    
    function checkKPTypeCmb()
    {
        if ( getKPTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setKPTypeCmbFocus()
    {
        document.getElementById("<%=drpKPType.ClientID %>").focus();   
    }                
</script>

<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbDismantleType
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-09  Tong.Zhi-Yong         Create   
 * 2010-01-14  Yuan XiaoWei          Modify All javascript function         
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbDismantleType.ascx.cs" Inherits="CommonControl_CmbDismantleType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpDismantelType" runat="server" Width="326px"  AutoPostBack="true"
        >
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">

    function getDismantleTypeCmbObj()
    {    
         return document.getElementById("<%=drpDismantelType.ClientID %>");   
    }
    
    function getDismantleTypeCmbText()
    { 
        if (document.getElementById("<%=drpDismantelType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
            return document.getElementById("<%=drpDismantelType.ClientID %>")[document.getElementById("<%=drpDismantelType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getDismantleTypeCmbValue()
    {
         return document.getElementById("<%=drpDismantelType.ClientID %>").value;
    }
    
    function checkDismantleTypeCmb()
    {
        if ( getDismantleTypeCmbText() == "")
        {
            return false;
        }

        return true;
    }
    
    function setDismantleTypeCmbFocus()
    {
        document.getElementById("<%=drpDismantelType.ClientID %>").focus();   
    }                
</script>

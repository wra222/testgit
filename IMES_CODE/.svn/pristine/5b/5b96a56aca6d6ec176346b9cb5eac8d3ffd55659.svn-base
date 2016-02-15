<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbIECPN
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-09-04  kaisheng              Create    
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbIECPN.ascx.cs" Inherits="CommonControl_DataMaintain__CmbIECPN" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpIECPN" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputIECPN") %>';;
        
    function getIECPNCmbObj()
    {    
       return document.getElementById("<%=drpIECPN.ClientID %>");   
    }
    
    function getIECPNCmbText()
    {
        if (document.getElementById("<%=drpIECPN.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
           return document.getElementById("<%=drpIECPN.ClientID %>")[document.getElementById("<%=drpIECPN.ClientID %>").selectedIndex].text;
        }
    }
    
    function getIECPNCmbValue()
    {
       return document.getElementById("<%=drpIECPN.ClientID %>").value;  
    }
    
    function checkIECPNCmb()
    {
        if (getIECPNCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }
    
    function setIECPNCmbFocus()
    {
        document.getElementById("<%=drpIECPN.ClientID %>").focus();   
    }    
                
</script>


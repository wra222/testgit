<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbSamplea
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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbSamplea.ascx.cs" Inherits="CommonControl_CmbSamplea" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpSamplea" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = "no selected !.";

    function getSampleaCmbObj()
    {
        return document.getElementById("<%=drpSamplea.ClientID %>");   
    }

    function getSampleaCmbText()
    {
        if (document.getElementById("<%=drpSamplea.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpSamplea.ClientID %>")[document.getElementById("<%=drpSamplea.ClientID %>").selectedIndex].text;
        }
    }

    function getSampleaCmbValue()
    {
        return document.getElementById("<%=drpSamplea.ClientID %>").value;  
    }

    function checkSampleaCmb()
    {
        if (getSample_aCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }

    function setSampleaCmbFocus()
    {
        document.getElementById("<%=drpSamplea.ClientID %>").focus();   
    }    
                
</script>


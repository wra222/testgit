<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbPPIDType
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-14  Tong.Zhi-Yong         Create          
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbPPIDType.ascx.cs" Inherits="CommonControl_CmbPPIDType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpPPIDType" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    function getPPIDTypeCmbObj()
    {    
        try 
        {
           return document.getElementById("<%=drpPPIDType.ClientID %>");   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getPPIDTypeCmbText()
    {
        try 
        {
           return document.all("<%=drpPPIDType.ClientID %>").options[document.all("<%=drpPPIDType.ClientID %>").selectedIndex].text.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getPPIDTypeCmbValue()
    {
        try 
        {
           return document.all("<%=drpPPIDType.ClientID %>").options[document.all("<%=drpPPIDType.ClientID %>").selectedIndex].value.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function checkPPIDTypeCmb()
    {
        try 
        {
            var msgSelectNullCmb = '';
            
            if (getPPIDTypeCmbText() == "")
            {
                ShowMessage(msgSelectNullCmb);
                return false;
            }

            return true;
           
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function setPPIDTypeCmbFocus()
    {
        try 
        {
            getPPIDTypeCmbObj().focus();   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }                 
</script>

<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbPPIDDescription
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-14  Tong.Zhi-Yong         Create          
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbPPIDDescription.ascx.cs" Inherits="CommonControl_CmbPPIDDescription" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpPPIDDescription" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    function getPPIDDescriptionCmbObj()
    {    
        try 
        {
           return document.getElementById("<%=drpPPIDDescription.ClientID %>");   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getPPIDDescriptionCmbText()
    {
        try 
        {
           return document.all("<%=drpPPIDDescription.ClientID %>").options[document.all("<%=drpPPIDDescription.ClientID %>").selectedIndex].text.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getPPIDDescriptionCmbValue()
    {
        try 
        {
           return document.all("<%=drpPPIDDescription.ClientID %>").options[document.all("<%=drpPPIDDescription.ClientID %>").selectedIndex].value.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function checkPPIDDescriptionCmb()
    {
        try 
        {
            var msgSelectNullCmb = '';
            
            if (getPPIDDescriptionCmbText() == "")
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
    
    function setPPIDDescriptionCmbFocus()
    {
        try 
        {
            getPPIDDescriptionCmbObj().focus();   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }               
</script>

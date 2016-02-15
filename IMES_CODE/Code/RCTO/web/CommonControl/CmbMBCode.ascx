<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbMBCode
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-12  Tong.Zhi-Yong         Create          
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMBCode.ascx.cs" Inherits="CommonControl_CmbMBCode" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpMBCode" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    function getMBCodeCmbObj()
    {    
        try 
        {
           return document.getElementById("<%=drpMBCode.ClientID %>");   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getMBCodeCmbText()
    {
        try 
        {
           return document.all("<%=drpMBCode.ClientID %>").options[document.all("<%=drpMBCode.ClientID %>").selectedIndex].text.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getMBCodeCmbValue()
    {
        try 
        {
           return document.all("<%=drpMBCode.ClientID %>").options[document.all("<%=drpMBCode.ClientID %>").selectedIndex].value.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function checkMBCodeCmb()
    {
        try 
        {
            var msgSelectNullCmb = '';
            
            if (getMBCodeCmbText() == "")
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
    
    function setMBCodeCmbFocus()
    {
        try 
        {
            getMBCodeCmbObj().focus();   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }               
</script>

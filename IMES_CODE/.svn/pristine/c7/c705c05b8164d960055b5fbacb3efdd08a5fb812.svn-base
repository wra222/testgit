<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:Cmb111Level
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-12  Tong.Zhi-Yong         Create          
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Cmb111Level.ascx.cs" Inherits="CommonControl_Cmb111Level" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drp111Level" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    function get111LevelCmbObj()
    {    
        try 
        {
           return document.getElementById("<%=drp111Level.ClientID %>");   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function get111LevelCmbText()
    {
        try 
        {
           return document.all("<%=drp111Level.ClientID %>").options[document.all("<%=drp111Level.ClientID %>").selectedIndex].text.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function get111LevelCmbValue()
    {
        try 
        {
           return document.all("<%=drp111Level.ClientID %>").options[document.all("<%=drp111Level.ClientID %>").selectedIndex].value.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function check111LevelCmb()
    {
        try 
        {
            var msgSelectNullCmb = '';
            
            if (get111LevelCmbText() == "")
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
    
    function set111LevelCmbFocus()
    {
        try 
        {
            get111LevelCmbObj().focus();   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }                
</script>
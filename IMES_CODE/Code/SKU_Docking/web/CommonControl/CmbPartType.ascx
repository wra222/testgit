<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbPartType
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-09  Tong.Zhi-Yong         Create          
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbPartType.ascx.cs" Inherits="CommonControl_CmbPartType" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpPartType" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    function getPartTypeCmbObj()
    {    
        try 
        {
           return document.getElementById("<%=drpPartType.ClientID %>");   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getPartTypeCmbText()
    {
        try 
        {
           return document.all("<%=drpPartType.ClientID %>").options[document.all("<%=drpPartType.ClientID %>").selectedIndex].text.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getPartTypeCmbValue()
    {
        try 
        {
           return document.all("<%=drpPartType.ClientID %>").options[document.all("<%=drpPartType.ClientID %>").selectedIndex].value.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function checkPartTypeCmb()
    {
        try 
        {
            var msgSelectNullCmb = '';
            
            if (getPartTypeCmbText() == "")
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
    
    function setPartTypeCmbFocus()
    {
        try 
        {
            getPartTypeCmbObj().focus();   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }                
</script>

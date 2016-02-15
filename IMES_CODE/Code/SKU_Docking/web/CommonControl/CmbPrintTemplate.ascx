<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbPrintTemplate
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-06  Tong.Zhi-Yong         Create          
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbPrintTemplate.ascx.cs" Inherits="CommonControl_CmbPrintTemplate" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpPrintTemplate" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    function getPrintTemplateCmbObj()
    {    
        try 
        {
           return document.getElementById("<%=drpPrintTemplate.ClientID %>");   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getPrintTemplateCmbText()
    {
        try 
        {
           return document.all("<%=drpPrintTemplate.ClientID %>").options[document.all("<%=drpPrintTemplate.ClientID %>").selectedIndex].text.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getPrintTemplateCmbValue()
    {
        try 
        {
           return document.all("<%=drpPrintTemplate.ClientID %>").options[document.all("<%=drpPrintTemplate.ClientID %>").selectedIndex].value.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function checkPrintTemplateCmb()
    {
        try 
        {
            var msgSelectNullCmb = '';
            
            if (getPrintTemplateCmbText() == "")
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
    
    function setPrintTemplateCmbFocus()
    {
        try 
        {
            getPrintTemplateCmbObj().focus();   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }                
</script>
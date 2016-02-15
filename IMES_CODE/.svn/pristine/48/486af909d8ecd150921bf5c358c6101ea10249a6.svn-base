<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbReturnStation
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-12-02  Tong.Zhi-Yong         Create          
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbReturnStation.ascx.cs" Inherits="CommonControl_CmbReturnStation" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpReturnStation" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    function getReturnStationCmbObj()
    {    
        try 
        {
           return document.getElementById("<%=drpReturnStation.ClientID %>");   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getReturnStationCmbText()
    {
        try 
        {
           return document.all("<%=drpReturnStation.ClientID %>").options[document.all("<%=drpReturnStation.ClientID %>").selectedIndex].text.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getReturnStationCmbValue()
    {
        try 
        {
           return document.all("<%=drpReturnStation.ClientID %>").options[document.all("<%=drpReturnStation.ClientID %>").selectedIndex].value.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function checkReturnStationCmb()
    {
        try 
        {
            var msgSelectNullCmb = '';
            
            if (getReturnStationCmbText() == "")
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
    
    function setReturnStationCmbFocus()
    {
        try 
        {
            getReturnStationCmbObj().focus();   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }    
                
</script>
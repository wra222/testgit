<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbFloor
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-10-23  Dorothy         Create          
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbLocFloor.ascx.cs" Inherits="CommonControl_CmbLocFloor" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpFloor" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    function getLocFloorCmbObj()
    {    
        try 
        {
           return document.getElementById("<%=drpFloor.ClientID %>");   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getLocFloorCmbText()
    {
        try 
        {
           return document.all("<%=drpFloor.ClientID %>").options[document.all("<%=drpFloor.ClientID %>").selectedIndex].text.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function getLocFloorCmbValue()
    {
        try 
        {
           return document.all("<%=drpFloor.ClientID %>").options[document.all("<%=drpFloor.ClientID %>").selectedIndex].value.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function checkLocFloorCmb()
    {
        try 
        {
            var msgSelectNullCmb = '';
            
            if (getLocFloorCmbText() == "")
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
    
    function setLocFloorCmbFocus()
    {
        try 
        {
            getLocFloorCmbObj().focus();   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }              
</script>

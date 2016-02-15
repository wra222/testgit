<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbFloor
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-12  Tong.Zhi-Yong         Create          
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbFloor.ascx.cs" Inherits="CommonControl_CmbFloor" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpFloor" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    function getFloorCmbObj()
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
    
    function getFloorCmbText()
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
    
    function getFloorCmbValue()
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
    
    function checkFloorCmb()
    {
        try 
        {
            var msgSelectNullCmb = '';
            
            if (getFloorCmbText() == "")
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
    
    function setFloorCmbFocus()
    {
        try 
        {
            getFloorCmbObj().focus();   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }              
</script>

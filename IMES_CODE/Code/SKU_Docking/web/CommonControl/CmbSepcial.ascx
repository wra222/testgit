<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Cmb111Level
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-03-07  Qian.Jia-li        Create          
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbSepcial.ascx.cs" Inherits="CommonControl_CmbSepcial" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" 
    UpdateMode="Conditional">
    <ContentTemplate>
        <asp:DropDownList ID="DropSpecial" runat="server" Width="120px"  CssClass="iMes_label_10pt_Red">
            <asp:ListItem Selected="True">Trial Run</asp:ListItem>
            <asp:ListItem>Pilot Run</asp:ListItem>
            <asp:ListItem>Traceability</asp:ListItem>
        </asp:DropDownList>
    </ContentTemplate>
</asp:UpdatePanel>
<script language="javascript" type="text/javascript">
  function GetDropOBj()
    {    
        try 
        {
           return document.getElementById("<%=DropSpecial.ClientID %>");   
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
    
    function GetDropValue()
    {
        try 
        {
           return document.all("<%=DropSpecial.ClientID %>").options[document.all("<%=DropSpecial.ClientID %>").selectedIndex].Value.trim();
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }
  
</script>

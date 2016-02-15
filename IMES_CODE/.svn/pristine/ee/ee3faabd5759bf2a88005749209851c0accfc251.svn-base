<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: model info maintain
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-11-10  98079                Create 
 qa bug no:ITC-1281-0043
 Known issues:

 --%>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="FamilyCreateCustomer.aspx.cs" Inherits="FamilyCreateCustomer" Title="" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>

    <div>
    <table border=0>
        <tr>
            <td height=5px>
            </td>
        </tr>
        <tr>

            <td><asp:Label ID="lblCustomer" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
            <td><asp:TextBox id="txtCustomer" MaxLength="50" runat="server" SkinId="textBoxSkin" Width="300px"></asp:TextBox></td>                        
        </tr>
        <tr>
            <td><asp:Label ID="lblCode" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
            <td colspan=4><asp:TextBox id="txtCode" MaxLength="50" runat="server" SkinId="textBoxSkin" Width="300px"></asp:TextBox></td>
            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="ServerClick" />
            </Triggers>
            </asp:UpdatePanel>                         
            
        </tr>
        <tr>

            <td><asp:Label ID="lblPlant" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
            <td><asp:TextBox id="txtPlant" MaxLength="10" runat="server" SkinId="textBoxSkin" Width="150px"></asp:TextBox></td>                        
        </tr>
        <tr>

            <td><asp:Label ID="lblDescr" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
            <td><asp:TextBox id="txtDescr" MaxLength="255" runat="server" SkinId="textBoxSkin" TextMode= "MultiLine" Width="300px" Height="50px"></asp:TextBox></td>                        
        </tr>
        <tr>
            <td height=8px>
            </td>
        </tr>
        <tr>
            <td align=right valign=middle colspan=3>
                <input id="btnOK" type="button"  runat="server" class="iMes_button"  onclick="if(clkSave())" onserverclick="btnOK_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                <input id="btnCancel" type="button"  runat="server" class="iMes_button" onclick="onCancel()" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
            </td>
        </tr>
    </table>
    </div> 

<script type="text/javascript">
    var ObjRegion;
    var selectedRowIndex = null;
    
    var msgAddSave = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAddSave").ToString() %>';
   

    function onOk()
    {
        window.returnValue = document.getElementById("<%=txtCustomer.ClientID %>").value;
        window.close();
    }

    function onCancel()
    {
        window.returnValue = false;
        window.close();
    }

    function clkSave()
    {
        ShowInfo("");
        var customer = document.getElementById("<%=txtCustomer.ClientID %>").value;   
        if(customer.trim()=="")
        {
           alert(msgAddSave);
           return false;
        }   

        return true;
        
    }
    
</script>
</asp:Content>

 


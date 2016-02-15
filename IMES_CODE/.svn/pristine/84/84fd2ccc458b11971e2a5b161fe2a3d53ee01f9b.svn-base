<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageMaintain.master" CodeFile="InputSnListForPAKSorting.aspx.cs" Inherits="PAK_InputSnListForPAKSorting" Title="Input SN" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div style="width: 95%; margin: 0 auto;">
    <table>
    <tr>
     <td><asp:Label runat="server" ID="lblStation"  CssClass="iMes_label_13pt" Text="Input SN:"></asp:Label> </td>
     <td><asp:Label runat="server" ID="Label1"  CssClass="iMes_label_13pt"  Text="Result:"></asp:Label> </td>
    </tr>
    <tr>
     <td>
      <asp:TextBox  ID="txtSnList" runat="server" Height="490px" TextMode="MultiLine" 
            Width="205px"></asp:TextBox>
     </td>
     <td>
         <asp:UpdatePanel ID="updatePanel1" runat="server" RenderMode="Inline" >
		<ContentTemplate>
         <asp:TextBox 
            ID="txtLog" runat="server" Height="490px" TextMode="MultiLine" 
            Width="400px" BackColor="#FFFFCC" ReadOnly="True"></asp:TextBox>
            	</ContentTemplate>   
	</asp:UpdatePanel> 
     </td>
    </tr>
    
    </table>
   
       
        

             <asp:HiddenField ID="hidSnList" runat="server"  /><br /><br />
             
             <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
		<ContentTemplate>
            <asp:Button ID="btnOK" runat="server" Text="OK" onclick="btnOK_Click" OnClientClick="return Check();" />
		</ContentTemplate>   
	             <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
                 </Triggers>
	</asp:UpdatePanel> 
             

 <input id="Button2" type="button" value="Cancel" onclick="btnCancel_Click()" 
            class="iMes_button" /><asp:HiddenField ID="hidEditor" runat="server"  />
      <asp:HiddenField ID="hidStation" runat="server"  />
   </div>
    
<script type="text/javascript">
    
    function btnCancel_Click() {

        window.close();
    }
    function Check() {
        var b;
        var sn = document.getElementById("<%=txtSnList.ClientID %>").value.replace(/\r\n/g, "");;
        if (sn == "") {
            alert("Please input sn!");
            return false;
        }
        else {
            b= confirm("Are you sure to upload sn list?");
        }
        if (b) {
            beginWaitingCoverDiv();
        }
        else {
            return b;
        }
    }
    function Clear() {
        document.getElementById("<%=txtSnList.ClientID %>").value = "";
    }

</script>    
</asp:Content>

<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChxLstProductType.ascx.cs" Inherits="CommonControl_ChxLstProductType" %>
<asp:CheckBoxList ID="chxPrd" runat="server" RepeatDirection="Horizontal" Class="xxxx">
    <asp:ListItem Selected="True"  Class="xxxx">PC</asp:ListItem>
    <asp:ListItem>RCTO</asp:ListItem>
    <asp:ListItem>FRU</asp:ListItem>
    <asp:ListItem>BIRCH</asp:ListItem>
</asp:CheckBoxList>
<script type="text/javascript">


    function GetProductTypeList() {

        return $('#<%=chxPrd.ClientID %> input[type=checkbox]:checked').map(function() {
            return $('label[for=' + this.id + ']').html();
         }).get().join(',');


}


</script>
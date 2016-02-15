<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeFile="WithdrawTestResult.aspx.cs" Inherits="Query_PAK_WithdrawTestResult"  EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
      <style type="text/css">
    .querycell
    {
        background-color: yellow;
     
    }
    .querycell:hover
    {
        background-color: #8AF2E7;
     
    }
     tr.clicked
    {
        background-color: white; 
    }
    .clicked
    {
        background-color: #8AF2E7;
    }
  .row1
    {
  	  background-color: #FFF8DC
  	  }
    .row2
    {
      background-color: #98FF98;
     }
     .row2:hover, .row1:hover
{
	background-color: white;
}
</style>    
</head>

<body >
    <form id="form1" runat="server">
    <div>
    <br />
      <br />
        <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="520px"     AutoGenerateColumns="false"
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" OnRowDataBound="gvResult_RowDataBound"
            meta:resourcekey="gvResultResource1" OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False">
              <Columns>
                                        
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox id="chk" runat="server"  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Consolidated" />
                                        <asp:BoundField DataField="PalletNo" />
                                        <asp:BoundField DataField="DeliveryNo" />
                                        <asp:BoundField DataField="CartonQty" />
                                        <asp:BoundField DataField="DeviceQty" />
                                        <asp:BoundField DataField="Result" />
                                       <asp:BoundField DataField="Select"  Visible="false" />
                                    </Columns>
        </iMES:GridViewExt>
    
    </div>
   
    </form>
    
    <p>
    
<script type="text/javascript">
   

</script>    
</body>

</html>
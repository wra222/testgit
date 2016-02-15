<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="HPIMESStationQuery.aspx.cs" Inherits="Query_FA_HPIMESStationQuery" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../../js/jscal2.js"></script>
    <script type="text/javascript" src="../../js/lang/en.js"></script>

    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
  
  <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Station Query" 
            CssClass="iMes_label_13pt" ></asp:Label></legend> 
            
            <table>
            <tr>
                <td>
                    <iMESQuery:CmbDBType id="CmbDBType" runat="server"  />    
                </td>
            </tr>
            <tr>
            <td style="height:50px">
                       <asp:Label ID="Label1" runat="server" Text="Station"></asp:Label>
                       <asp:TextBox ID="txtStation" runat="server"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnQuery" runat="server" Text="Query" 
        onclick="btnQuery_Click" />
            
            </td>
            </tr>
            </table>
     
            </fieldset>
            

    
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>          
       <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="450px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 3px">
        </iMES:GridViewExt>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
    
</asp:Content>


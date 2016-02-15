<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="QueryByPartSN.aspx.cs" Inherits="Query_FA_QueryByPartSN" Title="Untitled Page" %>

<%@ Register assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
<script src="../../js/jscal2.js"></script>
<script src="../../js/lang/en.js"></script>
    
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Query by Part SN" CssClass="iMes_label_13pt"></asp:Label></legend> 
       <br />
       <table>
       <tr style="height:10px">
      <td width ="5%" >
                      <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td width="35%">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>    
       </tr>
       <tr>
        <td style=" width:10% ">
        <asp:Label ID="Label1" runat="server" Text="Part SN: "></asp:Label>
        </td>
        <td style=" width:40% ">
     <iMES:Input ID="txtInput" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" ReplaceRegularExpression="" Width="80%" IsClear="true" IsPaste="true"/>
     
        </td>
        <td style=" width:40% ">
          <asp:Button ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" style=" display:none" />
            <asp:HiddenField ID="hidInput" runat="server" />
        </td>
       </tr>
       </table>
    
 
  
  
  </fieldset> 
    
    <br />
    <br />
    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>          
       <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" 
            GvExtHeight="150px" GvExtWidth="98%" 
            style="top: 0px; left: 0px; " Height="1px" >
        </iMES:GridViewExt>
        <br />
        <iMES:GridViewExt ID="Gr2" runat="server" AutoGenerateColumns="true" 
            GvExtHeight="150px" GvExtWidth="98%" 
            style="top: 0px; left: 0px;  " Height="1px" >
        </iMES:GridViewExt>
        
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>
 <script type="text/javascript">
  window.onload = function() {
        inputObj = getCommonInputObject();
        getAvailableData("processFun");
    };

    function processFun(backData) {
       document.getElementById("<%=hidInput.ClientID%>").value=backData;
      beginWaitingCoverDiv();
       document.getElementById("<%=btnQuery.ClientID%>").click();
       var obj=getCommonInputObject();
       obj.focus();
        getAvailableData("processFun");
    }

 
 
 </script>
 
 
</asp:Content>


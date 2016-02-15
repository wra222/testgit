<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="BTLocQuery.aspx.cs" Inherits="Query_PAK_BTLocQuery"  %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 
    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>
    <script type="text/javascript" src="../../js/jscal2.js"></script>
    <script type="text/javascript" src="../../js/lang/en.js"></script>
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="BT Loc Query" 
            CssClass="iMes_label_13pt" ></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="10%" align="right">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label></td>                
                <td align="left" style="width: 40%">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>        
                <td width ="10%" align="right">
                 <asp:Label ID="lblPdLine" runat="server" Text="PdLine" CssClass="iMes_label_13pt"></asp:Label>
                    
                </td>       
                <td width ="40%" align="left">
                    <iMES:CmbPdLine ID="CmbPdLine" runat="server" />
                                               
                </td> 
           
            </tr>
            <tr>
              <td width ="10%" align="right">
                   <asp:RadioButtonList ID="radInputType" runat="server" 
                       RepeatDirection="Horizontal">
                       <asp:ListItem Selected="True">Model</asp:ListItem>
                       <asp:ListItem>CPQSNO</asp:ListItem>
                   </asp:RadioButtonList>
                </td>
                 <td align="left" style="width: 40%"> 
                
                    <asp:TextBox ID="txtInput" runat="server" Height="19px" Width="310px"></asp:TextBox>
                
                </td>
            
             <td width ="10%" align="right">
                 &nbsp;</td>       
                <td width ="40%" align="left">
                    &nbsp;</td> 
                <td >
                    &nbsp;</td>
                     <td width ="5%">
                         <asp:Button CssClass="iMesQuery_button" ID="btnExcel" runat="server" 
                             Text="Excel" onclick="btnExcel_Click" style=" display:none" />
       
                    
            
                </td>
                <td width ="5%">

                  <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" OnClientClick="beginWaitingCoverDiv()" />
                <asp:Button ID="btnShowDetail" runat="server" onclick="btnShowDetail_Click" 
                        Text="Button" style="display: none"  />
                </td>
            </tr>
    
         </table>
     <asp:HiddenField ID="hidLocID" runat="server" />
</fieldset> 

   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" RenderMode="Inline">
    <ContentTemplate>
      
       <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="200px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" 
            meta:resourcekey="gvResultResource1" OnGvExtRowClick="" OnGvExtRowDblClick="ShowDetail(this)" 
            SetTemplateValueEnable="False" onrowdatabound="gvResult_RowDataBound" 
            >
        </iMES:GridViewExt>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
  <br />
  <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" RenderMode="Inline">
    <ContentTemplate>
  <iMES:GridViewExt ID="grvDetail" runat="server" GvExtHeight="200px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" 
             OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False">
        </iMES:GridViewExt>
         </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnShowDetail" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>

</center>

<script type="text/javascript">
    function ShowDetail(row) {
        if (row != null) {
            var LocID = row.cells[0].innerText.trim();
            document.getElementById(ConvertID("hidLocID")).value = LocID;
            beginWaitingCoverDiv();
            document.getElementById(ConvertID("btnShowDetail")).click();
        }
           // alert(LocID); 
    }
 
   
    
    </script>


</asp:Content>


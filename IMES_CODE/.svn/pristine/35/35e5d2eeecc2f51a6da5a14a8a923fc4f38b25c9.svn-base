<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ASTQuery.aspx.cs" Inherits="Query_PAK_ASTQuery"  EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>



<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 
    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
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
       <asp:Label ID="lblTitle" runat="server" Text="AST Query" 
            CssClass="iMes_label_13pt"></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="10%" align="right">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt" 
                       ></asp:Label></td>                
                <td width ="35%" align="left">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>        
                <td width ="10%" align="right">
                
                  </td>       
                <td width ="35%" align="left">
           </td> 
                <td width ="5%">
                 <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="Excel" style=" display:none"/>
             
                </td>
                <td width ="5%">
                
                    
       <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" OnClientClick="beginWaitingCoverDiv()"/>
     
                    
            
                </td>
            </tr>
    
         </table>
</fieldset> 

   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
      
       <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="400px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" 
            OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False">
        </iMES:GridViewExt>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
</center>





</asp:Content>


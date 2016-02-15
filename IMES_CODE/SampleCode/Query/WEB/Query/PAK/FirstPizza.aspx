<%@ Page Title="FirstPizza" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="FirstPizza.aspx.cs" Inherits="Query_PAK_FirstPizza"  EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script src="../../js/jscal2.js"></script>
    <script src="../../js/lang/en.js"></script>
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
      
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
  
  <center>
<fieldset id="grpCarton" style="border: thin solid #000000;">
     <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="First Pizza Query" CssClass="iMes_label_13pt"></asp:Label></legend>        
        <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="10%">
                      <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td colspan="3">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>                
            </tr>
            <tr>
               <td width ="10%">
                   <asp:Label ID="lblModel" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
               <td width ="40%">
                   <asp:TextBox ID="txtModel" runat="server" Width="300px" Height="20px"></asp:TextBox>                                                
               </td>        
               <td width ="10%">                  
               </td>
               <td width ="40%">                    
               </td>                       
            </tr>
             <tr>               
                <td colspan="4" align="center">                    
                    <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="beginWaitingCoverDiv();" style="width: 100px">Query</button>
                    &nbsp;&nbsp;&nbsp;                    
                    <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px; display: none;">Export</button>
                </td>                                
            </tr>                     
         </table>
         
         <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>      
       <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="300px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 6px; left: 0px">            
           <HeaderStyle Font-Size="Smaller" Width="50px" />
        </iMES:GridViewExt>                        
    </ContentTemplate>
    <Triggers>                 
         <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
    </Triggers>  
  </asp:UpdatePanel>
         
         
         
</fieldset>
</center>
</asp:Content>


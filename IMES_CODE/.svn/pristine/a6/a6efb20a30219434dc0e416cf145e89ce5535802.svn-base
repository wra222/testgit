<%@ Page Title="ModelBOM" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ModelBOM.aspx.cs" Inherits="Query_FA_ModelBOM" EnableEventValidation="false"%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
<script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
<%--<script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
<script src="../../js/jscal2.js"></script>
<script src="../../js/lang/en.js"></script>

<script src="../../js/jquery.dateFormat-1.0.js"></script>    
<script type="text/javascript" src="../../js/assets/prettify.js"></script>
<script type="text/javascript" src="../../js/jquery.multiselect.js"></script>     
<script type="text/javascript"  src="../../js/jquery.multiselect.filter.js"></script>     
    
<link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
<link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
<link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
<link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">

<link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
<link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
<link rel="stylesheet" type="text/css" href="../../css/assets/style.css" />
<link rel="stylesheet" type="text/css" href="../../css/assets/prettify.css" />      --%>
    
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="ModelBOM" CssClass="iMes_label_13pt"></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="10%">
                      <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td width ="40%">                      
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>                
                                
               <td width ="10%">
                   <asp:Label ID="lblPno" runat="server" Text="Pno:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
               <td width ="40%">
                    <asp:TextBox ID="txtModel" runat="server" Width="300px"></asp:TextBox>
               </td> 
            </tr>
          
            <tr>               
                <td colspan="4" align="center">                    
                    <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="beginWaitingCoverDiv();" style="width: 100px">Query</button>                     
                </td>
                
            </tr>
         </table>
</fieldset> 
</center>


<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" RenderMode="Inline">
    <ContentTemplate>           
        <table style="width: 100%">
            <tr>
                <td style="width: 35%; vertical-align: top;">
                 <asp:TreeView ID="tvModelBOM" runat="server" Font-Names="Tahoma" 
                     Font-Size="10pt" ShowLines="True" 
                        onselectednodechanged="tvModelBOM_SelectedNodeChanged">
                     <SelectedNodeStyle ForeColor="#0000CC" />
                     <NodeStyle ForeColor="#000000" />
                 </asp:TreeView>
               </td>            
                <td style="vertical-align: top; width: 65%">                       
                <fieldset id="Fieldset1" style="border: thin solid #000000;">
                    <legend align ="left" style ="height :20px" >
                        <asp:Label ID="Label1" runat="server" Text="ModelInfo" CssClass="iMes_label_13pt"></asp:Label>
                    </legend> 
                    <iMES:GridViewExt ID="gvModelInfo" runat="server" AutoGenerateColumns="true" Width="98%"
                        GvExtWidth="100%" GvExtHeight="200px" Height="1px" 
                        style="top: 21px; left: 2px">                        
                    </iMES:GridViewExt> 
                </fieldset>
                
                <fieldset id="Fieldset2" style="border: thin solid #000000;">
                    <legend align ="left" style ="height :20px" >
                        <asp:Label ID="Label2" runat="server" Text="PartInfo" CssClass="iMes_label_13pt"></asp:Label>
                    </legend>                  
                    <iMES:GridViewExt ID="gvPartInfo" runat="server" AutoGenerateColumns="true" Width="98%"
                        GvExtWidth="100%" GvExtHeight="200px" Height="1px" 
                        style="top: 21px; left: 2px">                        
                    </iMES:GridViewExt>                               
                </td>
                </fieldset>
                
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
    </Triggers>
  </asp:UpdatePanel>
  

</asp:Content>


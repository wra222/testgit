<%@ Page Title="COAStockQty" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="COAStockQty.aspx.cs" Inherits="Query_PAK_COAStockQty" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 
    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
        <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>    
    <script src="../../js/jscal2.js"></script>
    <script src="../../js/lang/en.js"></script>
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
      <style type="text/css">
    .data1
    {
        background-color: #E3E4FA; text-align:right; font-size:14px;
     
    }
     
    </style>
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
<fieldset id="grpCarton" style="border: thin solid #000000; background-color: #82CAFF;" >
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="COA Stock Qty" CssClass="iMes_label_13pt"></asp:Label></legend> 
        <table border="0" width="100%" style="font-family: Tahoma; ">                    
    <tr>
        <td width ="5%">
              <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
        </td>                
        <td >                        
            <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
        </td>        
        <td>
            <asp:Label ID="Label1" runat="server" Text="Status"></asp:Label>
         <asp:DropDownList ID="droStatus" runat="server" Height="20px" Width="100px">   </asp:DropDownList>
        </td>        
         <td>
            <asp:Label ID="Label2" runat="server" Text="Line"></asp:Label>
         <asp:DropDownList ID="droLine" runat="server" Height="20px" Width="100px">   </asp:DropDownList>
             <asp:CheckBox ID="chkSummary" runat="server" Text="Summary" />
        </td>  
            <td width ="5%"">
         <button id="btnExport"  runat="server" onserverclick="btnExport_Click"  style="width: 100px; display: none;">Export</button>
        </td>
        <td>
         <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="beginWaitingCoverDiv();" style="width: 100px">Query</button>
        </td>
     
    </tr>
   
 </table>
</fieldset> 


   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>          
       <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="450px" 
            Width="98%" GvExtWidth="98%" Height="1px"  onrowdatabound="gvResult_RowDataBound" >
        </iMES:GridViewExt>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
    </Triggers>
  </asp:UpdatePanel>
<iframe id="img" src="../../Images/Corner-2.jpg" width="0px" height="0px" scrolling="no" frameborder="0px" ></iframe>

</center>
<script type="text/javascript">    //<![CDATA[    

 </script>


                  
</asp:Content>

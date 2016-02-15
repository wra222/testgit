<%@ Page Title="COAStatusReport" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="COAStatusReport.aspx.cs" Inherits="Query_PAK_COAStatusReport" EnableEventValidation="false" %>
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
      
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
<fieldset id="grpCarton" style="border: thin solid #000000;">
     <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="COA Status Report" CssClass="iMes_label_13pt"></asp:Label></legend>        
       
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate> 
                   
    <table border="0" width="100%" style="font-family: Tahoma">                    
    <tr>
        <td style="width: 5%">
              &nbsp;</td>                
        <td style="width: 10%" align="left">
              <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
        </td>                
        <td align="left">                       
            <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
        </td>                
    </tr>
    <tr>
       <td style="width: 5%" rowspan="6" valign="top" align="right">
           <asp:RadioButtonList ID="rblQCondition" runat="server" AutoPostBack="True" 
               onselectedindexchanged="rblQCondition_SelectedIndexChanged">
               <asp:ListItem Value="1" Selected="True"></asp:ListItem>
               <asp:ListItem Value="2"></asp:ListItem>
               <asp:ListItem Value="3"></asp:ListItem>
               <asp:ListItem Value="4"></asp:ListItem>
               <asp:ListItem Value="5"></asp:ListItem>
               <asp:ListItem Value="6"></asp:ListItem>
           </asp:RadioButtonList>
       </td>
       <td style="width: 10%" align="left">
           <asp:Label ID="lblCOASN" runat="server" Text="COASN:" CssClass="iMes_label_13pt"></asp:Label>
       </td>
       <td align="left">
           <asp:Label ID="lblStartSN" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>
           <asp:TextBox ID="txtStartSN" runat="server" Width="180px" Height="20px" ></asp:TextBox>                       
           <asp:Label ID="lblEndSN" runat="server" Text="End" CssClass="iMes_label_13pt"></asp:Label>
           <asp:TextBox ID="txtEndSN" runat="server" Width="180px" Height="20px"></asp:TextBox>                       
        </td>
     </tr>
     <tr>
        <td style="width: 10%" align="left">
           <asp:Label ID="lblDate" runat="server" Text="Date:" CssClass="iMes_label_13pt"></asp:Label>
        </td>   
        <td align="left">
           <asp:Label ID="lblStartDate" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>
           <asp:DropDownList ID="ddlStartDate" runat="server"  Width="180px"></asp:DropDownList>
           <asp:Label ID="lblEndDate" runat="server" Text="End" CssClass="iMes_label_13pt"></asp:Label>
           <asp:DropDownList ID="ddlEndDate" runat="server"  Width="180px"></asp:DropDownList>                       
        </td>
    </tr>    
    <tr>
        <td style="width: 10%" align="left">
           <asp:Label ID="lblStatus" runat="server" Text="Status:" CssClass="iMes_label_13pt"></asp:Label>
       </td>   
       <td align="left">
           <asp:DropDownList ID="ddlCOAStatus" runat="server" Width="300px"></asp:DropDownList>
       </td>       
    </tr>    
    <tr>
        <td style="width: 10%" align="left">
           <asp:Label ID="lblResult" runat="server" Text="Result:" CssClass="iMes_label_13pt"></asp:Label>
       </td>   
       <td align="left">         
       </td>       
    </tr>
     <tr>
        <td style="width: 10%" align="left">
            <asp:Label ID="lblCOARemind" runat="server" Text="COARemind:" CssClass="iMes_label_13pt"></asp:Label>
       </td>   
       <td align="left">         
           <asp:DropDownList ID="ddlCOARemind" runat="server" Width="300px"></asp:DropDownList>
       </td>       
    </tr>
    
     <tr>
        <td style="width: 10%" align="left">
            <asp:Label ID="lblCOADailyCheck" runat="server" Text="COADailyCheck:" CssClass="iMes_label_13pt"></asp:Label>
       </td>   
       <td align="left">         
           <asp:DropDownList ID="ddlCOADailyCheck" runat="server" Width="300px">
               <asp:ListItem Value="1" Text="COA Trans"></asp:ListItem>
               <asp:ListItem Value="2" Text="COA Remove"></asp:ListItem>               
           </asp:DropDownList>
           <asp:Label ID="lblStartDateTR" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>
           <asp:DropDownList ID="ddlStartDateTR" runat="server"  Width="180px"></asp:DropDownList>
           <asp:Label ID="lblEndDateTR" runat="server" Text="End" CssClass="iMes_label_13pt"></asp:Label>
           <asp:DropDownList ID="ddlEndDateTR" runat="server"  Width="180px"></asp:DropDownList>                       
       </td>       
    </tr>
    </ContentTemplate>
    <Triggers>        
        <asp:AsyncPostBackTrigger ControlID="rblQCondition" EventName="SelectedIndexChanged" />         
    </Triggers>
    </asp:UpdatePanel>
    <tr>               
        <td align="center">                    
            &nbsp;</td>
        
        <td colspan="2" align="center">                    
            <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="beginWaitingCoverDiv();" style="width: 100px">Query</button>
            &nbsp;&nbsp;&nbsp;                    
            <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
              style="width: 100px; display: none;">Export</button>
        </td>
        
    </tr>
 </table>
</fieldset> 
<button id="btnQueryDetail" runat="server"  onserverclick="QueryDetailClick" style="display: none">QueryDetail</button> 
<asp:HiddenField ID="hfIECPN" runat="server" />
<asp:HiddenField ID="hfLine" runat="server" />
<asp:HiddenField ID="hfStatus" runat="server" />
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" RenderMode="Inline">
    <ContentTemplate>          
       <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="200px" 
            Width="98%" GvExtWidth="98%" Height="1px" 
            onrowdatabound="gvResult_RowDataBound">
        </iMES:GridViewExt>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />        
    </Triggers>
    </asp:UpdatePanel>
    
    
    
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always" RenderMode="Inline">
    <ContentTemplate>      
       <iMES:GridViewExt ID="gvResultDetail" runat="server" AutoGenerateColumns="true" GvExtHeight="300px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 6px; left: 0px">            
           <HeaderStyle Font-Size="Smaller" Width="50px" />
        </iMES:GridViewExt>                        
    </ContentTemplate>
    <Triggers>                 
         <asp:AsyncPostBackTrigger ControlID="btnQueryDetail" EventName="ServerClick" />
    </Triggers>  
  </asp:UpdatePanel>



</center>
<script type="text/javascript">

    function SelectDetail(IECPN,Line,Status) {
        beginWaitingCoverDiv();
        document.getElementById("<%=hfIECPN.ClientID%>").value = IECPN;
        document.getElementById("<%=hfLine.ClientID%>").value = Line;
        document.getElementById("<%=hfStatus.ClientID%>").value = Status;
        document.getElementById("<%=btnQueryDetail.ClientID%>").click();
    }
    function ClearEndTxt() {
        document.getElementById("<%=txtEndSN.ClientID%>").value = "";
    
    }

   </script>



                  
</asp:Content>


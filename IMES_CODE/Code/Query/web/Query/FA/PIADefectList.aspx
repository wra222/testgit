<%@ Page Title="PIADefectList" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="PIADefectList.aspx.cs" Inherits="Query_FA_PIADefectList" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 
    <script type="text/javascript" src="../../js/jquery-1.7.1.js "></script>   
    <script type="text/javascript" src="../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script> 
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js "></script>     
    <script src="../../js/jscal2.js "></script>
    <script src="../../js/lang/cn.js "></script>    

    <script type="text/javascript" src="../../js/jquery.multiselect.js "></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js "></script>     
    <script type="text/javascript" src="../../js/wz_tooltip.js "></script>
        
    
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
<script type="text/javascript">
</script>      
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
<fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="PIA Defect List" CssClass="iMes_label_13pt"></asp:Label></legend> 
        <table border="0" width="100%" style="font-family: Tahoma">                    
    <tr>
        <td width ="10%">
              <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
        </td>                
        <td width ="40%">                        
            <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
        </td>         
        <td width ="10%">
            <asp:Label ID="lblPdLine" runat="server" Text="PdLine:" CssClass="iMes_label_13pt"></asp:Label>
        </td>
        <td width ="40%">
            <asp:ListBox ID="lboxPdLine" runat="server" SelectionMode="Multiple" Height="95%" 
                                    Width="300px" CssClass="CheckBoxList">
            </asp:ListBox>
        </td>
    </tr>
    <tr>
       <td width ="10%">
           <asp:Label ID="lblRD" runat="server" Text="Repair Date:" CssClass="iMes_label_13pt"></asp:Label>
       </td>
       <td width ="40%">
            <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
            <asp:TextBox id="txtFromDate" runat="server" Width="140px" Height="20px"></asp:TextBox>                            
            <button id="btnFromDate" type="button" style="width: 20px">...</button>
            <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
            <asp:TextBox ID="txtToDate" runat="server" Width="140px" Height="20px"></asp:TextBox>
            <button id="btnToDate" type="button" style="width: 20px">...</button>          
       </td>            
       <td width ="10%">
            <asp:Label ID="lblStation" runat="server" Text="Station:" CssClass="iMes_label_13pt"></asp:Label>
       </td>
       <td width ="40%">         
         <asp:ListBox ID="lboxStaion" runat="server" Width="300px" CssClass="CheckBoxList" SelectionMode="Multiple"></asp:ListBox>
       </td>
    </tr>
    <tr>
       <td width ="10%">
            <asp:Label ID="lblDefect" runat="server" Text="Defect:" CssClass="iMes_label_13pt"></asp:Label>
       </td>
       <td width ="40%">
            <asp:ListBox ID="lboxDefect" runat="server" Width="300px" CssClass="CheckBoxList" SelectionMode="Multiple"></asp:ListBox>
       </td>            
       <td width ="10%">
            <asp:Label ID="lblCause" runat="server" Text="Cause:" CssClass="iMes_label_13pt"></asp:Label>                    
       </td>
       <td width ="40%">
            <asp:ListBox ID="lboxCause" runat="server" Width="300px" CssClass="CheckBoxList" SelectionMode="Multiple"></asp:ListBox>
       </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblModelCategory" runat="server" Text="Model Category:" CssClass="iMes_label_13pt"></asp:Label>
        </td>
        <td><iMESQuery:ChxLstProductType ID="ChxLstProductType1" runat="server"   />
        </td>
        <td colspan="2" align="center">                    
            <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="beginWaitingCoverDiv();" 
            style="width: 100px">Query</button>
            &nbsp;&nbsp;&nbsp;                    
            <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
              style="width: 100px; display: none;">Export</button>            
        </td>
        
    </tr>
 </table>
</fieldset> 


       
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>   
           <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="380px" 
            Width="98%" GvExtWidth="98%" Height="1px">
        </iMES:GridViewExt>        
       </ContentTemplate>
       <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="serverclick" />
       </Triggers>   
  </asp:UpdatePanel>


</center>
<script type="text/javascript">

    function EndRequestHandler(sender, args) {

        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
        $("#<%=gvResult.ClientID%> tr:nth-child(n+2) ").click(function() { rowclick(); });

        Calendar.setup({
            trigger: "btnFromDate",
            inputField: "<%=txtFromDate.ClientID%>",
            onSelect: updateCalendarFields,
            onTimeChange: updateCalendarFields,
            showTime: 24,
            dateFormat: "%Y-%m-%d %H:%M",
            minuteStep: 1
        });
        Calendar.setup({
            inputField: "<%=txtToDate.ClientID%>",
            trigger: "btnToDate",
            onSelect: updateCalendarFields,
            onTimeChange: updateCalendarFields,
            showTime: 24,
            dateFormat: "%Y-%m-%d %H:%M",
            minuteStep: 1
        });

    };

    window.onload = function() {
        EndRequestHandler();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    };   

</script>


                  
</asp:Content>


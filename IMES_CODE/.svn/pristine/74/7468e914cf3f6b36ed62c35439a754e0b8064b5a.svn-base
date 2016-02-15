<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="WaittingDefect Component Rejudge.aspx.cs" Inherits="Query_FA_WaittingDefect_Component_Rejudge" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
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
    <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>        
             <script language="javascript" type="text/javascript">
                </script>
    </ContentTemplate>    
  </asp:UpdatePanel>  
   <body> 
    <center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="ProductRepairQuery" 
            CssClass="iMes_label_13pt"></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="5%">
                      <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td width="35%">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>                
                <td width ="5%">
                   <asp:Label ID="Label1" runat="server" Text="PartType:" CssClass="iMes_label_13pt"></asp:Label>
                 </td>
                <td width="35%">
                     <asp:DropDownList ID="drpparttype" runat="server" Width="114px">
                        <asp:ListItem> </asp:ListItem>
                         <asp:ListItem>MB </asp:ListItem>
                         <asp:ListItem>KP/ME</asp:ListItem>
                     </asp:DropDownList>
                </td>
                <td rowspan="3" width="20%" >
                    <button id="btnQuery" runat="server" onclick="beginWaitingCoverDiv();"
                        onserverclick="btnQuery_Click" style="width: 100px">
                        Query</button>
                    <br />
                    <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                                    style="width: 100px;  display: none;">Export</button>
                    <br />

                </td>
            </tr>
            <tr>
               <td>
                   <asp:Label ID="lblDate" runat="server" Text="Date:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
               <td >
                            <asp:Label ID="Label3" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
                            <asp:TextBox id="txtPeriodFromDate" class="txtPeriod" runat="server" Width="120px" Height="20px"></asp:TextBox>                            
                            <button id="btnPeriodFromDate" type="button" style="width: 20px">...</button>
                            <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                            <asp:TextBox ID="txtPeriodToDate" class="txtPeriod" runat="server" Width="120px" Height="20px"></asp:TextBox>
                            <button id="btnPeriodToDate" type="button" style="width: 20px">...</button>                               

              </td>        
               <td>
                    <asp:Label ID="Label2" runat="server" Text="Status:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
               <td>
                   <asp:DropDownList ID="dropstatus" runat="server" Width="114px">
                    <asp:ListItem Value=""></asp:ListItem>
                       <asp:ListItem Value="00">待复判</asp:ListItem>
                       <asp:ListItem Value="11">Q复判不良品</asp:ListItem>
                       <asp:ListItem Value="10">Q复判良品</asp:ListItem>
                       <asp:ListItem Value="20">厂商复判良品</asp:ListItem>
                       <asp:ListItem Value="21">厂商复判不良</asp:ListItem>
                   </asp:DropDownList>
                </td>
            </tr>
            <tr>
               <td >
                    &nbsp;</td>
               <td >                   
                   &nbsp;</td>            
               <td >
                    &nbsp;</td>
               <td >
                   <br />
               </td>
            </tr>
         </table>
</fieldset> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
       <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="450px" 
            Width="98%" GvExtWidth="98%" Height="1px" ShowFooter="True">            
        </iMES:GridViewExt>        
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="serverclick" />
        </Triggers>
     </asp:UpdatePanel>
     
       <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
           <asp:HiddenField ID="hidModelList" runat="server" />
        </ContentTemplate>
     </asp:UpdatePanel>
</center>
    
<script language="javascript" type="text/javascript">
    function EndRequestHandler(sender, args) {
        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select...' }).multiselectfilter();
        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
      


        Calendar.setup({
            trigger: "btnPeriodFromDate",
            inputField: "<%=txtPeriodFromDate.ClientID%>",
            onSelect: function() { updateCalendarFields, $("#rbPeriodByPeriod").attr("checked", "true"); },
            onTimeChange: function() { updateCalendarFields, $("#rbPeriodByPeriod").attr("checked", "true"); },
            showTime: 24,
            dateFormat: "%Y-%m-%d %H:%M",
            minuteStep: 30
        });

        Calendar.setup({
            trigger: "btnPeriodToDate",
            inputField: "<%=txtPeriodToDate.ClientID%>",
            onSelect: function() { updateCalendarFields, $("#rbPeriodByPeriod").attr("checked", "true"); },
            onTimeChange: function() { updateCalendarFields, $("#rbPeriodByPeriod").attr("checked", "true"); },
            showTime: 24,
            dateFormat: "%Y-%m-%d %H:%M",
            minuteStep: 30
        });

        $("#<%=txtPeriodFromDate.ClientID %>").focus(function() {
            $("#rbPeriodByPeriod").attr("checked", "true");
        });
        $("#<%=txtPeriodToDate.ClientID %>").focus(function() {
            $("#rbPeriodByPeriod").attr("checked", "true");
        });

    };

    window.onload = function() {
        EndRequestHandler();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    };
</script>
</asp:Content>


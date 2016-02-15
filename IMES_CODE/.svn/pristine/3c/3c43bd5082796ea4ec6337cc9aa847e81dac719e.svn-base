<%@ Page Title="ReworkQuery" Language="C#" MasterPageFile="~/MasterPageMaintain.master"  AutoEventWireup="true"  CodeFile="ReworkQuery.aspx.cs" Inherits="Query_ReworkQuery" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>


<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

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
                     
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
                
        </asp:ScriptManager>
 <center>

 <fieldset style="border: solid #000000">
            <legend>Rework Query</legend>
            <table border="0" width="100%" style="border-width:thin;">
            <tr>
               <td style="width: 11%">
                      <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td style="width: 44%">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td> 
            </tr>
            <tr>                                    
                <td style="width: 11%">
                         <asp:Label ID="lblDate" runat="server" Text="Date:" 
                             CssClass="iMes_label_13pt"></asp:Label>                   
                </td>
                <td style="width: 44%">
                    <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
                    <asp:TextBox id="txtFromDate" runat="server" Width="140px" Height="20px"></asp:TextBox>                            
                    <button id="btnFromDate" type="button" style="width: 20px">...</button>
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="140px" Height="20px"></asp:TextBox>
                    <button id="btnToDate" type="button" style="width: 20px">...</button> 
                </td>
                <td >
                    <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick=""  
                        style="width: 100px">Query</button><input type="hidden" id="hidFromData" runat="server" /><input type="hidden" id="hidToData" runat="server" />
                    <br />
                    <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px;">Export</button>
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
<script type="text/javascript">
    function EndRequestHandler(sender, args) {

        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();


        Calendar.setup({
            trigger: "btnFromDate",
            inputField: "<%=txtFromDate.ClientID%>",
            onSelect: function() { this.hide(); },
            dateFormat: "%Y-%m-%d",
            minuteStep: 1
        });
        Calendar.setup({
            inputField: "<%=txtToDate.ClientID%>",
            trigger: "btnToDate",
            onSelect: function() { this.hide(); },
            dateFormat: "%Y-%m-%d",
            minuteStep: 1
        });
    };

    $(window).load(function() {
        EndRequestHandler();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    });

   

</script>
</asp:Content>


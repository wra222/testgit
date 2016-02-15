<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ASTReport.aspx.cs" Inherits="Query_ASTReport" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    
    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
<script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
<script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
<script src="../../js/jscal2.js"></script>
<script src="../../js/lang/cn.js"></script>
    
<script src="../../js/jquery.dateFormat-1.0.js"></script>    
    <script type="text/javascript" src="../../js/assets/prettify.js"></script>
    <script type="text/javascript" src="../../js/jquery.multiselect.js"></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js"></script>     
    <script type="text/javascript" src="../../js/wz_tooltip.js"></script>
        
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">

    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
    <link rel="stylesheet" type="text/css" href="../../css/assets/style.css" />
    <link rel="stylesheet" type="text/css" href="../../css/assets/prettify.css" />                   
<style type="text/css" >
        .ast_qty:hover
        {
            background-color: blue;
            cursor:pointer;
        }
        .ast_qty
        {
            background-color: yellow;    
            cursor:pointer;
        }
        .ast_qty_click
        {
            background-color: blue;
            cursor:pointer;
        }
            
</style>
<script type="text/javascript">
    function load() {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }

    function EndRequestHandler(sender, args) {

        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();

        Calendar.setup({
            inputField: "<%=txtFromDate.ClientID%>",
            trigger: "<%=txtFromDate.ClientID%>",
            onSelect: function() { this.hide()},
            showTime: 24,
            dateFormat: "%Y-%m-%d",
            minuteStep: 1
        });
        Calendar.setup({
            inputField: "<%=txtToDate.ClientID%>",
            trigger: "<%=txtToDate.ClientID%>",
            onSelect: function() { this.hide()},
            showTime: 24,
            dateFormat: "%Y-%m-%d",
            minuteStep: 1
        });

        //$("#<%=txtFromDate.ClientID%>").val($.format.date($("#<%=txtFromDate.ClientID%>").val(), "yyyy-MM-dd HH:mm"));
        //$("#<%=txtToDate.ClientID%>").val($.format.date($("#<%=txtToDate.ClientID%>").val(), "yyyy-MM-dd HH:mm"));
    };
 </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
                
        </asp:ScriptManager>
 <center>
        <fieldset style="border: solid #000000">
            <legend>AST Report</legend>
    <table border="0" width="100%" style="font-family: Tahoma">                    
    <tr>
        <td width ="10%">
              <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
        </td>                
        <td width="40%">                        
            <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
        </td>
        <td width="10%">
            <asp:Label ID="lblFamily" runat="server" Text="Family:" CssClass="iMes_label_13pt"></asp:Label>
        </td>                
        <td width="40%">        
             
                         <asp:DropDownList ID="ddlFamily" runat="server"  Width="200px" 
                             onselectedindexchanged="ddlFamily_SelectedIndexChanged" 
                             AutoPostBack="true">
                         </asp:DropDownList>
                
        </td>
    </tr>
    <tr>
       <td width ="10%">
           <asp:Label ID="lblRD" runat="server" Text="Period:" CssClass="iMes_label_13pt"></asp:Label>
       </td>
       <td width ="40%">
                   <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
                     <asp:TextBox id="txtFromDate" runat="server" Width="150px" Height="20px"></asp:TextBox>                           
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" Height="20px"></asp:TextBox>
                    </td>            
       <td width ="10%">
            <asp:Label ID="Label5" runat="server" Text="Model(AST):" CssClass="iMes_label_13pt"></asp:Label>

       </td>
       <td width ="40%">
            <asp:ListBox ID="lstModel" runat="server" Width="300px" CssClass="CheckBoxList" SelectionMode="Multiple" ></asp:ListBox>        
       </td>
    </tr>
    <tr>               
        <td colspan="4" align="center">                    
            <asp:Button id="btnQuery" runat="server" onserverclick="queryClick" Text="Query" 
                        onclick="btnQuery_Click" />                   
            &nbsp;&nbsp;&nbsp;                    
            <button id="Button2"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px; display: none;">Export</button>
                    <asp:Button id="btnExport" runat="server" onserverclick="ExportClick" Text="Export"
                        onclick="btnExport_Click" />
            <button id="btnQueryDetail" runat="server"  onserverclick="btnQueryDetail_Click" style="display: none"></button> 
        </td>
        
    </tr>
 </table>

        </fieldset>
        <asp:HiddenField ID="hfModel" runat="server" />

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="370px"
                Width="98%" GvExtWidth="98%" Height="1px">
                </iMES:GridViewExt>
            </ContentTemplate>
        </asp:UpdatePanel>
                <div style="padding: 5px 0 0 0 ">
                
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>        
                <iMES:GridViewExt ID="gvDetail" runat="server" AutoGenerateColumns="true" GvExtHeight="200px"
                Width="98%" GvExtWidth="98%" Height="1px">
                </iMES:GridViewExt>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQueryDetail" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>            
           
</center>
        <script type="text/javascript">

         EndRequestHandler();
         function SelectDetail(model) {
             //beginWaitingCoverDiv();
             document.getElementById("<%=hfModel.ClientID%>").value = model;
             document.getElementById("<%=btnQueryDetail.ClientID%>").click();
             $("#<%=gvQuery.ClientID %> > tbody > tr:nth-child(n) > td:nth-child(3)").each(function() {
                 if ($(this).html() > 0) {
                     //$(this).css("background", "yellow");
                     $(this).addClass("ast_qty");
                     $(this).removeClass("ast_qty_click");
                 }
                 if ($(this).parent().find("td").eq(0).html() == model) {
                     //$(this).css("background", "blue");
                     $(this).removeClass("ast_qty");
                     $(this).addClass("ast_qty_click");
                 }
                 //if ($(this).parent()) { 
                 //}
             })
            //$(this).css("background","blue");            
         }


    </script>


  
</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="KitPartQuery.aspx.cs" Inherits="Query_KitPartQuery" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>


<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>        
    <script src="../../js/jscal2.js"></script>
    <script src="../../js/lang/en.js"></script>
        
    <script type="text/javascript" src="../../js/ui.dropdownchecklist.js"></script>
    <script type="text/javascript" src="../../js/jquery.dateFormat-1.0.js"></script>        
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../../css/ui.dropdownchecklist.themeroller.css">                         
                         
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
                
        </asp:ScriptManager>
 <center>
        <fieldset style="border: solid #000000">
            <legend>Kitting Part Query</legend>
            <table border="0" width="100%" style="border-width:thin;">
            <tr>
                <td>
                    <div style="width: 600px; padding: 10px 0 0 10px; margin: 0 auto ; float: left ; display: none" visible="false">
                        <asp:Label ID="Label3" runat="server" Text="DBName:"></asp:Label>
                        <iMESQuery:CmbDBType id="CmbDBType" runat="server"  />                                                
                    </div>
                </td>
            </tr>
            <tr>                                    
                <td style=" width: 100%; height: 75px">                                                
                <div style="float:left; padding: 0 0 0 10px; margin: 0 auto;">                                                                                                
                  <asp:UpdatePanel ID="UpdatePanel1" runat="server">                     
                     <ContentTemplate>                   
                                    <iMES:CmbPdLine ID="cbPdLine" runat="server" />
                                    <asp:Label ID="lblFrom" runat="server" Text="Family:" Width="70px" CssClass="iMes_label_13pt"></asp:Label>                  
                                    <asp:DropDownList ID="ddlFamily" runat="server"  Width="200px" 
                                         onselectedindexchanged="ddlFamily_SelectedIndexChanged" AutoPostBack="true">
                                     </asp:DropDownList>                                     
                                     
                                    <asp:Label ID="lblModel" runat="server" Text="Model:" Width="70px" CssClass="iMes_label_13pt"></asp:Label>                  
                                    <asp:DropDownList ID="ddlModel" runat="server"  Width="200px" >
                                    </asp:DropDownList>
                                    <asp:Label ID="lblQty" runat="server" Text="Qty:" Width="70px" CssClass="iMes_label_13pt"></asp:Label>                                                                                                                                                                                                                               
                                    <input type="text" id="tbQty"  class="iMes_textbox_input_Yellow" />                                    
                                    <input type="button" class="btnAddRow" value="Add" onclick="addRow('<%=gvModel.ClientID %>');" />                                
                       </ContentTemplate>                                     
                   </asp:UpdatePanel> 
                </div>                              
                <div style="float:left; padding: 0 0 0 10px; height:100%; position: relative">                                   
                   <div style="bottom: 10px; position: absolute ; width: 220px"> 
                    <asp:Button id="btnQuery" runat="server" onserverclick="queryClick" Text="Query" 
                        onclick="btnQuery_Click" />        
                    <asp:Button id="btnExport" runat="server" onserverclick="ExportClick" Text="Export"
                        onclick="btnExport_Click" />
                   </div>                    
                </div>                    
                </td>                    
            </tr>
        </table>
        </fieldset>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>                
                <iMES:GridViewExt ID="gvModel" runat="server" AutoGenerateColumns="true" GvExtHeight="100px"
                Width="98%" GvExtWidth="98%" Height="1px" >
                </iMES:GridViewExt>                
                
                <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="380px"
                Width="98%" GvExtWidth="98%" Height="1px">
                </iMES:GridViewExt>
            </ContentTemplate>
        </asp:UpdatePanel>
        
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:LinkButton ID="lbtFreshPage" runat="server" 
                    OnClientClick="return ReloadImesPage();" onclick="lbtFreshPage_Click"></asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
           
</center>
       <asp:HiddenField ID="hidUser" runat="server" />       
       <asp:HiddenField ID="hidprocess" runat="server" />              
       <asp:HiddenField ID="hidsource" runat="server" />              
       <asp:HiddenField ID="hidmodelcnt" runat="server" />
    </div>


    <script language="javascript" type="text/javascript">
        var inputObj;
        
        window.onload = function() {
            //inputObj = getCommonInputObject();
            //getAvailableData("processFun");
        };
    
        function bind() 
        {
            //beginWaitingCoverDiv();
            //
        }

        function processFun(backData) {
            ShowInfo("");
            beginWaitingCoverDiv();            
            document.getElementById("<%=btnQuery.ClientID%>").click();
        }

        function initPage() {
            clearData();
            inputObj.value = "";
            getAvailableData("processFun");
            inputObj.focus();
        }

        function setCommonFocus() {
            endWaitingCoverDiv();
            inputObj.focus();
            inputObj.select();
            window.onload();
        }


        
    </script>
       <script type="text/javascript">           //<![CDATA[
           function addRow(tableID) {
               var table = document.getElementById(tableID);

               var family = $("#<%=ddlFamily.ClientID %> :selected").html();
               if (family == null || family == "" || family == "-") {
                   alert("Please select family");
                   return;
               }
               var model = $("#<%=ddlModel.ClientID %> :selected").html();
               if (model == null || model == "") {
                   alert("Please select model");
                   return;
               }
               var qty = $("#tbQty").val();
               if (!(parseInt(qty) > 0)) 
               {
                   alert("Please keyin Qty");
                   return;
               }
               
               if ($("#<%=gvModel.ClientID %> tr").length == 2 &&
                   $("#<%=gvModel.ClientID %> tr:eq(1) td:eq(0)").html() == "&nbsp;") 
               {
                   $("#<%=gvModel.ClientID %> tr:eq(1) td:eq(0)").html(family);
                   $("#<%=gvModel.ClientID %> tr:eq(1) td:eq(1)").html(model);
                   $("#<%=gvModel.ClientID %> tr:eq(1) td:eq(2)").html(qty);
               }
               else{                   
                   var cut = $("#<%=gvModel.ClientID %> tr").length;
                   if (cut % 2 == 1) {
                       $('#<%=gvModel.ClientID %> tr:eq(0)').after('<tr class="iMes_grid_RowGvExt"><td>' + family + '</td><td>' + model + '</td><td>' + qty + '</td></tr>');
                   }
                   else {
                       $('#<%=gvModel.ClientID %> tr:eq(0)').after('<tr class="iMes_grid_AlternatingRowGvExt"><td>' + family + '</td><td>' + model + '</td><td>' + qty + '</td></tr>');
                   }
               }
               $("#<%=hidmodelcnt.ClientID %>").val($("#<%=hidmodelcnt.ClientID %>").val() + model + ":" + qty + ";");
           }
           
       </script>
</asp:Content>
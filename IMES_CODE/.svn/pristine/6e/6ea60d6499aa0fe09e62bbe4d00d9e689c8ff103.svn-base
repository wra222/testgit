<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="FALocQuery.aspx.cs" Inherits="Query_FA_FALocQuery" Title="FA LOC QUERY" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../../js/jquery-1.7.1.js "></script>    
    <script type="text/javascript" src="../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js "></script>     
    <script src="../../js/jscal2.js "></script>
    <script src="../../js/lang/cn.js "></script>    
    <script type="text/javascript" src="../../js/superTables.js"></script>
    <script type="text/javascript" src="../../js/jquery.superTable.js"></script>

    <script type="text/javascript" src="../../js/jquery.multiselect.js "></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js "></script>     
    <script type="text/javascript" src="../../js/wz_tooltip.js "></script>
        
    
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
    <link rel="stylesheet" type="text/css" href="../../css/superTables.css" />
    
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
     
    </asp:ScriptManager>
  <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="FA LOC Query" CssClass="iMes_label_13pt"></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="5%">
                      <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td width="35%">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>                
                <td width ="5%">
                    <asp:Label ID="lblStation" runat="server" Text="Station:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="35%">
                     <asp:DropDownList ID="drpstation" runat="server" Height="20px" Width="35%">
                         <asp:ListItem>DownLoad</asp:ListItem>
                         <asp:ListItem Value="RunIn"></asp:ListItem>
                     </asp:DropDownList>
                      <asp:Label ID="Label2" runat="server" Text="QueryType:" CssClass="iMes_label_13pt"></asp:Label>
                       <asp:DropDownList ID="querytype" runat="server" Height="20px" 
                         Width="35%">
                       <asp:ListItem>Time</asp:ListItem>
                       <asp:ListItem>Line</asp:ListItem>
                       <asp:ListItem>SN/Model</asp:ListItem>
                   </asp:DropDownList>
                </td>
               
            </tr>
            <tr>
               <td>
                   <asp:Label ID="lblDate" runat="server" Text="Date:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
               <td >
                   <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
                     <asp:TextBox id="txtFromDate" runat="server" Width="140px" Height="20px"></asp:TextBox>                            
                     <button id="btnFromDate" type="button" style="width: 20px">...</button>
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="140px" Height="20px"></asp:TextBox>
                    <button id="btnToDate" type="button" style="width: 20px">...</button>
               </td>    
               
               <td >
                <asp:Label ID="lblPdLine" runat="server" Text="PdLine:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
                      
               <td>
               
                   <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>  
   <asp:ListBox ID="lboxPdLine" runat="server" SelectionMode="Multiple" Height="95%" 
                                    Width="200px" CssClass="CheckBoxList">
                            </asp:ListBox> 
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btqry" EventName="ServerClick" />
    </Triggers>
    </asp:UpdatePanel>
    
                     
               </td>
               <td>
              <button id="btqry"  runat="server" onserverclick="btqry_Click"    style="width: 100px; display: none"  >Query</button>

                   
               
               </td>
            </tr>
            
            <tr>
               <td>
                    <br />
               </td>
               <td>
                   
                    <br />
               </td>            
                <td>
                <asp:Label ID="Label3" runat="server" Text="Model/SN:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
               <td>
                   
                   <input type="text" id="txtModel" style="width: 150px" maxlength="14" class="iMes_textbox_input_Yellow" />
                   <input  type="button" id="BtnBrowse" value="Browse"  onclick="UploadModelList()" /></td>
                <td >
                <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" 
                        onclick="beginWaitingCoverDiv();"  style="width: 100px; display: none">Query</button>
                <input type="button" onclick="query();" value="Query" style="width: 100px">
                
                <br />
                <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px; display: none;">Export</button><br />
                <button id="btnDetailExport"  runat="server" onserverclick="btnDetailExport_Click" 
                        style="width: 100px; display: none;">DetailExport</button>
                <input type="button" onclick="queryexcel();" value="Export" style="width: 100px">
                </td>
            </tr>
         </table>
          </fieldset> 
          <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>      
       <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="250px" 
            Width="98%" GvExtWidth="98%"  Height="1px">           
        </iMES:GridViewExt>     
         
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btqry" EventName="ServerClick" />
    </Triggers>
    </asp:UpdatePanel>

<asp:HiddenField ID="hidModelList" runat="server" />
<asp:HiddenField ID="hfModel" runat="server" />
<script language="javascript" type="text/javascript">
    window.onload = function() {
        EndRequestHandler();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    };
    $("#txtModel").change(function() {
        var v = $(this).val();
        if (v.length > 0 ) {
            $("#<%=hfModel.ClientID %>").val(v);
        }
    });
    function EndRequestHandler(sender, args) {

        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
        

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

    function query() {
       beginWaitingCoverDiv();
       var pdline = '';
       var lineID = '#' + ConvertID("lboxPdLine");

       $(lineID + " option:selected").each(function() {
            pdline = pdline + $(this).val() + ',';
       });
        var txtFromdate = $("#<%=txtFromDate.ClientID %>").val();
        var txtTodate = $("#<%=txtToDate.ClientID %>").val();
        var station = $("#<%=drpstation.ClientID %>").val();
        var type = $("#<%=querytype.ClientID %>").val();
        var list = $("#<%=hfModel.ClientID %>").val();
        if (txtFromdate == "" || txtTodate == "") {
            alert("时间不能为空！");
            endWaitingCoverDiv();
            return;
        }
//        if (station == "") {
//            alert("请选择查询内容！");
//            endWaitingCoverDiv();
//            return;
//        }
//        if (type == "") {
//            alert("请选择查询方式！");
//            endWaitingCoverDiv();
//            return;
//        }
//        if (type == "Line") {
//            if (pdline == "") {
//                alert("Please Select Line");
//                endWaitingCoverDiv();
//                return;
//            }
//        }
//        if (type == "SN/Model") {
//            if (list == "") {
//                alert("Please Input Model or SN");
//                endWaitingCoverDiv();
//                return;
//            }
//        }
        
        PageMethods.GetMain_WebMethod(txtFromdate, txtTodate, station, type, pdline, list, onSuccessForMain, onErrorForMain);
        document.getElementById("<%=btqry.ClientID%>").click(); 
       
        
    }
      function onErrorForMain(error) {
           if (error != null)
               alert(error.get_message());
           endWaitingCoverDiv();
          
         
       }
       function onSuccessForMain(receiveData) {      
           endWaitingCoverDiv();

       }
    function UploadModelList() {
        var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelList.ClientID %>").value;
        var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
        if (dlgReturn) {

            dlgReturn = dlgReturn.replace(/\r\n/g, ",");
            document.getElementById("<%=hfModel.ClientID %>").value = RemoveBlank(dlgReturn);

            //   document.getElementById("<%=hidModelList.ClientID %>").value = dlgReturn;


        }
        else {
            if (dlgReturn == "")
            { document.getElementById("<%=hfModel.ClientID %>").value = ""; }
            return;
        }

    }
    function RemoveBlank(modelList) {
        var arr = modelList.split(",");
        var model = "";
        if (modelList != "") {
            for (var m in arr) {
                if (arr[m] != "") {
                    model = model + arr[m] + ",";
                }


                // content += key + ' : ' + myarr[key] + '<br />';

            }
            model = model.substring(0, model.length - 1)
        }

        return model;
    }
    function ResetPage() {
        EndRequestHandler();
        document.getElementById("<%=hfModel.ClientID %>").value = "";
        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
    }
    function queryexcel() {
        document.getElementById("<%=btnExport.ClientID%>").click(); 
    }
   </script>
</asp:Content>


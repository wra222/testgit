<%@ Page Title="MPInputEx" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="MPInputEx.aspx.cs" Inherits="Query_FA_MPInputEx" EnableEventValidation="false"%>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" >     

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


<style type="text/css">
        
  
    .querycell
    {
        background-color: yellow;
     
    }
    .querycell:hover
    {
        cursor:pointer;                 
    }
    .querycell.nopointer:hover
    {
        background-color: #30CEE4;
        cursor:inherit;
    }
    
    .querycell.clicked
    {
        background-color: #30CEE4;
    }
    .footer
    {
        font-size: 16px;
        border-color: #FFFFFF;
        font-weight:bold;        
      
    }
  .clicked
        {
            background-color: #80FFFF;
        }
          .altRow { background-color: #ddddff; }
</style>    
                      

<script type="text/javascript">

 </script>
 
 <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"  >        
  </asp:ScriptManager>

<table width="100%" id="menu">
<tr  width="100%" align="left" style=" background-color:#29088A">
 <td align="left" >
 <asp:Label ID="lblTitle" runat="server" Text="FA Report" CssClass="iMes_label_13pt" 
         Font-Size="14pt" ForeColor="White" Font-Bold="True" Font-Names="Verdana"></asp:Label>
 </td>
</tr>
<center>
</table>
      
         <table border="1" width="100%" style="font-family: Tahoma" id="menuTable">                    
            <tr>
                <td width ="5%">
                      <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td width="35%">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>                
                <td width ="5%">
                    <asp:Label ID="lblPdLine" runat="server" Text="PdLine:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="30%">
                   
                             <asp:ListBox ID="lboxPdLine" runat="server" CssClass="CheckBoxList" 
                                 SelectionMode="Multiple" Width="200px" Height="31px"></asp:ListBox>
                         
                </td>
                <td width="25%" >
                
                 <iMESQuery:ChxLstProductType ID="ChxLstProductType1" runat="server"   />
                </td>
            </tr>
            <tr>
               <td>
                   <asp:Label ID="lblShipDate" runat="server" Text="Ship Date:" CssClass="iMes_label_13pt"></asp:Label>                   
               </td>
               <td >
                     <asp:TextBox id="txtShipDate" runat="server" Width="140px" Height="20px"></asp:TextBox>                            
                     <button id="btnShipDate" type="button" style="width: 20px">...</button>
                   
               </td>            
               <td>
                    <asp:Label ID="lblModel" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
               <td>
                  
                   <input type="text" id="txtModel" style="width: 150px" maxlength="14" class="iMes_textbox_input_Yellow" /><input  type="button" id="BtnBrowse" value="Browse"  onclick="UploadModelList()" /></td>
               <td>
                    <asp:Label ID="lblFamily" runat="server" Text="Family:" CssClass="iMes_label_13pt"></asp:Label>
                   <asp:DropDownList ID="ddlFamily" runat="server" Width="220px" 
                       onselectedindexchanged="ddlFamily_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                   
                </td>
            </tr>
            
            <tr>
               <td>
                    <asp:Label ID="lblTotalQty" runat="server" Text="TotalQty:"
                        Font-Bold="True" ForeColor="Red" Font-Size="Large" Font-Names="Arial"></asp:Label>
               </td>
               <td>
                   <asp:Label ID="lblTotalQtyCount" runat="server" Font-Size="X-Large"></asp:Label></td>            
               <td>
                   <asp:Label ID="lblActualQty" runat="server" Text="ActualQty :" 
                        Font-Size="Large"></asp:Label></td>
               <td>
                    <asp:Label ID="lblActualQtyCount" runat="server"  Font-Size="X-Large"></asp:Label>
                  
               </td>
               <td >
                <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" 
                        onclick="beginWaitingCoverDiv();"  style="width: 100px; display: none">Query</button>
                <input type="button" onclick="query();" value="Query" style="width: 100px">
                <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px; display: none;">Export</button>
                <button id="btnDetailExport"  runat="server" onserverclick="btnDetailExport_Click" 
                        style="width: 100px; display: none;">DetailExport</button>
                <input type="button" onclick="queryexcel();" value="Export" style="width: 100px">
                </td>
            </tr>
         </table>

<br />
    <button id="btnQueryDetail" runat="server"  onserverclick="btnQueryDetail_Click" style="display: none"></button> 
    <asp:HiddenField ID="hfStation" runat="server" />
    <asp:HiddenField ID="hfLine" runat="server" /> 
    <asp:HiddenField ID="hfModel" runat="server" />
    <asp:HiddenField ID="hfFamily" runat="server" />
    <asp:HiddenField ID="hfShipDate" runat="server" />
    <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfLineShife" runat="server" value="true" />
                     
    <asp:HiddenField ID="hfMVS_SUM" runat="server" Value="" />
    <asp:HiddenField ID="hfPAKCosmetic_SUM" runat="server" Value="" />
    <asp:HiddenField ID="hfCOA_SUM" runat="server" Value="" />
    
    <asp:HiddenField ID="hfProcess_All" runat="server" Value="" />
    <asp:HiddenField ID="hfProcess_FA" runat="server" Value="" />    
    <asp:HiddenField ID="hfProcess_PAK" runat="server" Value="" />
    
    <div id="pnl_query"></div>
    
    <div id="pnl_detail"></div>
    <div id="pnl_excel"></div>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>      
    <br />
       <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="250px" 
            Width="98%" GvExtWidth="98%" Height="1px">           
        </iMES:GridViewExt>     
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
    </Triggers>
    </asp:UpdatePanel>
     
    <div style="padding: 5px 0 0 0 ">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>      
       <iMES:GridViewExt ID="gvStationDetail" runat="server" AutoGenerateColumns="true"  GvExtHeight="200px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" Visible="false" >            
           <HeaderStyle Font-Size="Smaller" Width="50px" Height="14px" />
        </iMES:GridViewExt>                        
    </ContentTemplate>
    <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnQueryDetail" EventName="ServerClick" />
    </Triggers>
  </asp:UpdatePanel>
    </div>
    
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
        <ContentTemplate>
            <asp:HiddenField ID="hidModelList" runat="server" />
        </ContentTemplate>
        <Triggers>
        
            <asp:AsyncPostBackTrigger ControlID="ddlFamily" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="HiddenField1" runat="server" />
</center>
<asp:HiddenField ID="hidConnection" runat="server" Value='' />
    <asp:HiddenField ID="hidDBName" runat="server" Value='' />

<script language="javascript" type="text/javascript">
    var MVS_SUM = $("#<%=hfMVS_SUM.ClientID %>").val();
    var PAKCosmetic_SUM = $("#<%=hfPAKCosmetic_SUM.ClientID %>").val();
    var COA_SUM = $("#<%=hfCOA_SUM.ClientID %>").val();
    function GetDNQty() {
        // WipTrackingByDN_PAK.TESTAA('x','x','dbName', onSuccessGetQ_callback);
        var lblTotalQty = '#' + ConvertID("lblTotalQtyCount");
        var lblActualQty = '#' + ConvertID("lblActualQtyCount");
        $(lblTotalQty).text('Updating..');
        $(lblActualQty).text('Updating..');
        var inputID = '#' + ConvertID("txtShipDate");
        var shipDate = $("#<%=txtShipDate.ClientID %>").val();
        var connection = document.getElementById("<%=hidConnection.ClientID %>").value;
        var dbName = document.getElementById("<%=hidDBName.ClientID %>").value;
        var txtModelID = '#' + ConvertID("txtModel");
        var model = $("#<%=hfModel.ClientID %>").val();
        var modelList = $("#<%=hfModel.ClientID %>").val();
        //var prdType = "PC";
       var prdType= GetProductTypeList();
        PageMethods.GetDNQty_WebMethod(connection, shipDate, model, modelList, dbName, prdType, onSuccessGetQ_callback, onErrorGetQ_callback);

    }
    function onSuccessGetQ_callback(receiveData) {
        var dnQ = receiveData;
        var lblTotalQty = '#' + ConvertID("lblTotalQtyCount");
        var lblActualQty = '#' + ConvertID("lblActualQtyCount");
        $(lblTotalQty).text(dnQ[0]);
        $(lblActualQty).text(dnQ[1]);
   
    }
    function onErrorGetQ_callback(error) {
        if (error != null)
            alert(error.get_message());
    }
    var isQueryDetail = false;
    var selectLine = "";
    var inputModel = "";
    function query() {
        beginWaitingCoverDiv();
        //Get DN Qty
        isQueryDetail = false;

        GetDNQty();
        //Get DN Qty
     //   gvDetail
        $("#gvDetail").hide();
        var pdline = '';
        $("#ctl00_iMESContent_lboxPdLine option:selected").each(function() {
            pdline = pdline + $(this).val() + ',';
        });
        selectLine = pdline;
        var txtShipDate = $("#<%=txtShipDate.ClientID %>").val();
        if(txtShipDate=="")
        {alert("Please select ShipDate");return;}
        var family =  $("#<%=ddlFamily.ClientID %>").val();
        var model = $("#<%=hfModel.ClientID %>").val();
        inputModel = model;
        var modelCategory = GetProductTypeList();
        var lineshift = $("#<%=hfLineShife.ClientID %>").val();
        var rbProcess = $(".iMes_Radio > input:checked").val();
        var stationlist = "CombineDN,MVS_SUM,PAKCosmetic_SUM" + "," + $("#<%=hfProcess_All.ClientID %>").val();
      

        var inputstation = "";
        
        var grpmodel=false;
//        if ($("#grpmodel").attr('checked') == "checked") {
//            grpmodel = "true";
//        }
//        else {
//            grpmodel = "false";
//        }
        
        var dbtype = $("#ctl00_iMESContent_CmbDBType_ddlDBType option:selected").val();
        var dbname = $("#ctl00_iMESContent_CmbDBType_ddlDB option:selected").val();
		document.getElementById('pnl_query').innerHTML = '';
		$.ajax({
		    url: 'server/MPInputEx.aspx',
		    type: "POST",
		    data: {
		        action: "GET_SUMMARY",
		        txtShipDate: txtShipDate,
		        //  txtToDate: txtTodate,
		        family: family,
		        model: model,
		        pdline: pdline,
		        lineshift: lineshift,
		        rbProcess: rbProcess,
		        stationlist: stationlist,
		        mvs_sum: MVS_SUM,
		        pakcosmetic_sum: PAKCosmetic_SUM,
		        coa_sum: COA_SUM,
		        inputstation: inputstation,
		        grpmodel: grpmodel,
		        dbtype: dbtype,
		        dbname: dbname,
		        modelCategory: modelCategory,
		        isGetDetail: isQueryDetail
		    },
		    dataType: "html",
		    success: function(response) {
		        //$("#pnl_query").html(response);
		        document.getElementById('pnl_query').innerHTML = response;
		        var aa = $("#gvResult").height();
		        var h = aa + 80 + "px";
		        if (aa < 400)
		        { $("#divData").height(aa + 50); $("#gvResult").toSuperTable({ height: h }); }
		        else
		        { $("#gvResult").toSuperTable({ height: "400px" }); }
		        //		        else
		        //		        { $("#gvResult").toSuperTable({ height: "400px" }); }
		        //
		   //     $("#<%=hfModel.ClientID %>").val('');
		        endWaitingCoverDiv();
		    },
		    error: function(response) {
		        alert(response);
		        endWaitingCoverDiv();
		    }
		});

    }


    function SelectDetail_Ajax() {
        beginWaitingCoverDiv();
        isQueryDetail = true;
        var txtShipDate = $("#<%=txtShipDate.ClientID %>").val();
    
        var family = $(event.srcElement).siblings()[1].innerHTML;
        var model = $(event.srcElement).siblings()[2].innerHTML;
        if (model == "")
        {model = inputModel;}
        var station = $("#gvResult > thead >tr > th")[event.srcElement.cellIndex].innerHTML;
        if (station == "MVS_SUM")
        { station = MVS_SUM; }
        if (station == "PAKCosmetic")
        { station = PAKCosmetic_SUM; }
        if (station == "COA_SUM")
        { station = COA_SUM; }

        //MVS_SUM PAKCosmetic_SUM COA_SUM
        var inputLine = "";
        var line = $(event.srcElement).siblings()[0].innerHTML;
        if (line == "ALL")
        { inputLine = ""; }
        else if (!line)
        { inputLine = selectLine; }
        else
        {inputLine = line; }
     
      
        var lineshift = $("#<%=hfLineShife.ClientID %>").val();
 
        var inputstation = "";
        var dbtype = $("#ctl00_iMESContent_CmbDBType_ddlDBType option:selected").val();
        var dbname = $("#ctl00_iMESContent_CmbDBType_ddlDB option:selected").val();


        $.ajax({
            url: 'server/MPInputEx.aspx',
            type: "POST",
            data: {
                action: "GET_DETAIL",
                txtShipDate: txtShipDate,
            //    txtToDate: txtTodate,
                family: family,
                model: model,
                station: station,
                lineshift: lineshift,
                line: line,
                dbtype: dbtype,
                dbname: dbname,
                inputstation: inputstation,
                isGetDetail: isQueryDetail,
                pdline: inputLine 
            },
            dataType: "html",
            success: function(response) {
                $("#pnl_detail").html(response);
                //$("#gvDetail").toSuperTable({ width: "100%" });
                endWaitingCoverDiv();
            },
            error: function(response) {
                alert(response);
                endWaitingCoverDiv();
            }
        });

    }
    function queryexcel() {
        beginWaitingCoverDiv();
        var pdline = '';
        $("#ctl00_iMESContent_lboxPdLine option:selected").each(function() {
            pdline = pdline + $(this).val() + ',';
        });
        
        var txtShipDate = $("#<%=txtShipDate.ClientID %>").val();

        var family = $("#<%=ddlFamily.ClientID %>").val();
        var model = $("#<%=hfModel.ClientID %>").val();
        var modelCategory = GetProductTypeList();
        var station = "";

        var lineshift = $("#<%=hfLineShife.ClientID %>").val();
        //var line = $(event.srcElement).siblings()[0].innerHTML;
        var rbProcess = $(".iMes_Radio > input:checked").val();
        var stationlist = "";
        if (rbProcess == "All") {
            stationlist = "MVS_SUM,PAKCosmetic_SUM,COA_SUM" + "," + $("#<%=hfProcess_All.ClientID %>").val();
        } else if (rbProcess == "FA") {
            stationlist = $("#<%=hfProcess_FA.ClientID %>").val();
        } else if (rbProcess == "PAK") {
            stationlist = $("#<%=hfProcess_PAK.ClientID %>").val();
        }
        
     
        var inputstation = "";
        var grpmodel;
        if ($("#grpmodel").attr('checked') == "checked") {
            grpmodel = "true";
        }
        else {
            grpmodel = "false";
        }
        var dbtype = $("#ctl00_iMESContent_CmbDBType_ddlDBType option:selected").val();
        var dbname = $("#ctl00_iMESContent_CmbDBType_ddlDB option:selected").val();
        var key = ["action", "txtShipDate", "txtToDate", "family", "model", "pdline", "station", "lineshift", "rbProcess", "stationlist", "grpmodel", 
                            "inputstation", "dbtype", "dbname","mvs_sum","pakcosmetic_sum","coa_sum","isGetDetail"];
  
        var data = { "action": "GET_EXCEL",
            "txtShipDate": txtShipDate,
           
            "family": family,
            "model": model,    
            "pdline" :pdline,
            "station": station,
            "lineshift": lineshift,
            "rbProcess": rbProcess,
            "stationlist": stationlist,
            "grpmodel": grpmodel,
            "inputstation": inputstation,
            "dbtype": dbtype,
            "dbname": dbname,
            "modelCategory": modelCategory,
                "mvs_sum": MVS_SUM,
                "pakcosmetic_sum": PAKCosmetic_SUM,
                "coa_sum": COA_SUM,
                "isGetDetail": isQueryDetail
        };
        var url = "server/MPInputEx.aspx";

        $("#pnl_excel").html("<iframe width='0' height='0' frameborder='0' src='" + getposturl(key, data, url) + "'></iframe>");
        endWaitingCoverDiv();
    }


    function getposturl(parakey, paravalue, url) {
        url += "?";
        for (i = 0; i < parakey.length; i++) {
            url += parakey[i] + "=" + paravalue[parakey[i]] + "&";
        }
        return url;
    }
    function EndRequestHandler(sender, args) {

        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
        //$("#<%=gvResult.ClientID%> tr:nth-child(n+2) ").click(function() { rowclick(); });

        Calendar.setup({
            trigger: "btnShipDate",
            inputField: "<%=txtShipDate.ClientID%>",
            onSelect: function() { this.hide() },
           
            showTime: 24,
            dateFormat: "%Y-%m-%d",
            minuteStep: 1
        });
      
    };
    
//    function updateFields(cal) {
//        var date = cal.selection.get();
//        if (date) {
//            date = Calendar.intToDate(date);
//            cal.inputField.value = Calendar.printDate(date, "%Y-%m-%d");
//        }
//    };
    function updateFields(cal)
    { }

    function SelectDetail(station,line,model,family,shift) {
        beginWaitingCoverDiv();
        document.getElementById("<%=hfStation.ClientID%>").value = station;
        document.getElementById("<%=hfLine.ClientID%>").value = line;
        document.getElementById("<%=hfModel.ClientID%>").value = model;
        document.getElementById("<%=hfFamily.ClientID%>").value = family;
        document.getElementById("<%=hfLineShife.ClientID%>").value = shift;
        $(".clicked").removeClass("clicked");
        $(event.srcElement.parentNode).addClass("clicked");
        $(event.srcElement).addClass("clicked");    
        document.getElementById("<%=btnQueryDetail.ClientID%>").click();
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

    var SelRowIdx;
    function CR(row) {
        var RowIndex = row.rowIndex;
        var newSelectRow = $("#gvResult > tbody > tr ")[RowIndex];
        
        if (SelRowIdx != 0) {
            $($("#gvResult > tbody > tr ")[SelRowIdx-1]).removeClass("clicked");
        }
        $($("#gvResult > tbody > tr ")[RowIndex-1]).addClass("clicked");
        SelRowIdx = RowIndex;
        //  $(".clicked").removeClass("clicked");
        // $($(event.srcElement).parent()).addClass("clicked");
    }

    $("#txtModel").change(function() {
        var v = $(this).val();
        if (v.length > 10 || v.length == 0) {
            $("#<%=hfModel.ClientID %>").val(v);
        }
    });

    window.onload = function() {
        EndRequestHandler();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        $("#menu").click(function() {
            var d = $("#divData").height();
            var h = $("#gvResult").height();
            if (d == null) { return; }
            $("#menuTable").toggle(); return;
            if ($("#menuTable").css('display') == 'table') {
                $("#divData").height(300);
            }
            else {
                if (h != null && d != null) {
                    if (h < 299) {
                        $("#divData").height(300);
                    }
                    else {
                        $("#divData").height(450);
                        $('#gvResult').height(440);
                    }
                }

            }


        });

    };

   </script>

</asp:Content>


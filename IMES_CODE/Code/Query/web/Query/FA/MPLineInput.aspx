<%@ Page Title="MPLineInput" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="MPLineInput.aspx.cs" Inherits="Query_MPLineInput" EnableEventValidation="false"%>

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
        
    tr.clicked
    {
        background-color: white; 
    }
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

</style>    
                      

<script type="text/javascript">

 </script>
 
 <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="MP Input" CssClass="iMes_label_13pt"></asp:Label></legend> 
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
                     <asp:DropDownList ID="ddlStation" runat="server" Width="300px" 
                         onselectedindexchanged="ddlStation_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>                                     
                </td>
                <td width="20%" rowspan="2">
                 <asp:Label ID="lblModelCategory" runat="server" Text="Model Category:" CssClass="iMes_label_13pt"></asp:Label><br />
                 <iMESQuery:ChxLstProductType ID="ChxLstProductType1" runat="server"   />
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
               <td>
                    <asp:Label ID="lblPdLine" runat="server" Text="PdLine:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
               <td>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:ListBox ID="lboxPdLine" runat="server" SelectionMode="Multiple" Height="95%" 
                                    Width="200px" CssClass="CheckBoxList">
                            </asp:ListBox>                        
                            <asp:Button ID="btnChangeLine" runat="server" Text="ChangeLine" 
                                onclick="btnChangeLine_Click"/>
                            <asp:HiddenField ID="hfLineShife" value="true" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnChangeLine" EventName="click" />
                        </Triggers>
                    </asp:UpdatePanel>
               </td>
            </tr>
            
            <tr>
               <td>
                    <asp:Label ID="lblFamily" runat="server" Text="Family:" CssClass="iMes_label_13pt"></asp:Label>
                    <br />
                    <asp:Label ID="lblProcess" runat="server" Text="Process:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
               <td>
                   <asp:DropDownList ID="ddlFamily" runat="server" Width="300px" 
                       onselectedindexchanged="ddlFamily_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <br />
                   <asp:RadioButton ID="rbProcess_All" Text="FA&PAK" GroupName="rbProcess" runat="server" oncheckedchanged="Process_CheckedChanged" value="All" Checked="true" />
                   <asp:RadioButton ID="rbProcess_FA"  Text="FA" GroupName="rbProcess" runat="server" oncheckedchanged="Process_CheckedChanged" value="FA" Visible="false" />
                   <asp:RadioButton ID="rbProcess_PAK" Text="PAK" GroupName="rbProcess"  runat="server" oncheckedchanged="Process_CheckedChanged" value="PAK" Visible="false" />
               </td>            
               <td>
                    <asp:Label ID="lblModel" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
               <td>
                   <!--<asp:TextBox ID="txtModel" runat="server" Width="300px" isvalid="false" ></asp:TextBox>-->
                   <input type="text" id="txtModel" style="width: 150px" maxlength="14" class="iMes_textbox_input_Yellow" />
                   <input  type="button" id="BtnBrowse" value="Browse"  onclick="UploadModelList()" />
                   <label><input type="checkbox" id="grpmodel" value="grpModel" />GrpModel</label>
               </td>
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
    <button id="btnQueryDetail" runat="server"  onserverclick="btnQueryDetail_Click" style="display: none"></button> 
    <asp:HiddenField ID="hfStation" runat="server" />
    <asp:HiddenField ID="hfLine" runat="server" /> 
    <asp:HiddenField ID="hfModel" runat="server" />
    <asp:HiddenField ID="hfFamily" runat="server" />
    <asp:HiddenField ID="hfFromDate" runat="server" />
    <asp:HiddenField ID="hfToDate" runat="server" />
    
    <asp:HiddenField ID="hfMVS_SUM" runat="server" Value="" />
    <asp:HiddenField ID="hfITCND_SUM" runat="server" Value="" />
    <asp:HiddenField ID="hfCOA_SUM" runat="server" Value="" />
    
    <asp:HiddenField ID="hfProcess_All" runat="server" Value="" />
    <asp:HiddenField ID="hfProcess_FA" runat="server" Value="" />    
    <asp:HiddenField ID="hfProcess_PAK" runat="server" Value="" />
    
    <div id="pnl_query"></div>
    
    <div id="pnl_detail"></div>
    <div id="pnl_excel"></div>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>      
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
            <asp:AsyncPostBackTrigger ControlID="ddlStation" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlFamily" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="HiddenField1" runat="server" />
</center>


<script language="javascript" type="text/javascript">
    var MVS_SUM = $("#<%=hfMVS_SUM.ClientID %>").val();
    var ITCND_SUM = $("#<%=hfITCND_SUM.ClientID %>").val();
    var COA_SUM = $("#<%=hfCOA_SUM.ClientID %>").val();


    function query() {
        beginWaitingCoverDiv();
        var pdline = '';
        $("#ctl00_iMESContent_lboxPdLine option:selected").each(function() {
            pdline = pdline + $(this).val() + ',';
        });

        var txtFromdate = $("#<%=txtFromDate.ClientID %>").val();
        var txtTodate = $("#<%=txtToDate.ClientID %>").val();
        var family =  $("#<%=ddlFamily.ClientID %>").val();
        var model = $("#<%=hfModel.ClientID %>").val();
        var modelCategory = GetProductTypeList();
        var lineshift = $("#<%=hfLineShife.ClientID %>").val();
        var rbProcess = $(".iMes_Radio > input:checked").val();
        var stationlist = "";
        if (rbProcess == "All") {
            stationlist = "MVS_SUM,ITCND_SUM,COA_SUM" + "," + $("#<%=hfProcess_All.ClientID %>").val();
        } else if (rbProcess == "FA") {
            stationlist = $("#<%=hfProcess_FA.ClientID %>").val();
        } else if (rbProcess == "PAK") {
            stationlist = $("#<%=hfProcess_PAK.ClientID %>").val();
        }
        
        
        var inputstation = $("#<%=ddlStation.ClientID %> option:selected").val();
        var grpmodel;
        if ($("#grpmodel").attr('checked') == "checked") {
            grpmodel = "true";
        }
        else {
            grpmodel = "false";
        }
        
        var dbtype = $("#ctl00_iMESContent_CmbDBType_ddlDBType option:selected").val();
        var dbname = $("#ctl00_iMESContent_CmbDBType_ddlDB option:selected").val();
		document.getElementById('pnl_query').innerHTML = '';
        $.ajax({
            url: 'server/mplineinput.aspx',
            type: "POST",
            data: {
                action: "GET_SUMMARY",
                txtFromDate: txtFromdate,
                txtToDate: txtTodate,
                family: family,
                model: model ,
                pdline: pdline ,
                lineshift:  lineshift,
                rbProcess: rbProcess,
                stationlist: stationlist,
                mvssum: MVS_SUM,
                itcnd_sum: ITCND_SUM,
                coa_sum: COA_SUM,
                inputstation: inputstation,
                grpmodel: grpmodel,
                dbtype: dbtype,
                dbname: dbname,
                modelCategory: modelCategory
            },
            dataType: "html",
            success: function(response) {				
                //$("#pnl_query").html(response);
				document.getElementById('pnl_query').innerHTML=response;
                //$("#gvResult").toSuperTable({ width: "100%" });
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

        var txtFromdate = $("#<%=txtFromDate.ClientID %>").val();
        var txtTodate = $("#<%=txtToDate.ClientID %>").val();
        var family = $(event.srcElement).siblings()[1].innerHTML;
        var model = $(event.srcElement).siblings()[2].innerHTML;
        var station = $("#gvResult > thead >tr > th")[event.srcElement.cellIndex].innerHTML;
        var line = $(event.srcElement).siblings()[0].innerHTML;
        var lineshift = $("#<%=hfLineShife.ClientID %>").val();
        var inputstation = $("#<%=ddlStation.ClientID %> option:selected").val();
        var dbtype = $("#ctl00_iMESContent_CmbDBType_ddlDBType option:selected").val();
        var dbname = $("#ctl00_iMESContent_CmbDBType_ddlDB option:selected").val();


        $.ajax({
            url: 'server/mplineinput.aspx',
            type: "POST",
            data: {
                action: "GET_DETAIL",
                txtFromDate: txtFromdate,
                txtToDate: txtTodate,
                family: family,
                model: model,
                station: station,
                lineshift: lineshift,
                line: line,
                dbtype: dbtype,
                dbname: dbname,
                inputstation: inputstation
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
        
        var txtFromdate = $("#<%=txtFromDate.ClientID %>").val();
        var txtTodate = $("#<%=txtToDate.ClientID %>").val();
        var family = $("#<%=ddlFamily.ClientID %>").val();
        var model = $("#<%=hfModel.ClientID %>").val();
        var modelCategory = GetProductTypeList();
        var station = "";

        var lineshift = $("#<%=hfLineShife.ClientID %>").val();
        //var line = $(event.srcElement).siblings()[0].innerHTML;
        var rbProcess = $(".iMes_Radio > input:checked").val();
        var stationlist = "";
        if (rbProcess == "All") {
            stationlist = "MVS_SUM,ITCND_SUM,COA_SUM" + "," + $("#<%=hfProcess_All.ClientID %>").val();
        } else if (rbProcess == "FA") {
            stationlist = $("#<%=hfProcess_FA.ClientID %>").val();
        } else if (rbProcess == "PAK") {
            stationlist = $("#<%=hfProcess_PAK.ClientID %>").val();
        }
        
        var inputstation = $("#<%=ddlStation.ClientID %> option:selected").val();
        var grpmodel;
        if ($("#grpmodel").attr('checked') == "checked") {
            grpmodel = "true";
        }
        else {
            grpmodel = "false";
        }
        var dbtype = $("#ctl00_iMESContent_CmbDBType_ddlDBType option:selected").val();
        var dbname = $("#ctl00_iMESContent_CmbDBType_ddlDB option:selected").val();
        var key = ["action", "txtFromDate", "txtToDate", "family", "model", "pdline", "station", "lineshift", "rbProcess","stationlist",  "grpmodel", "inputstation", "dbtype", "dbname"];
  
        var data = { "action": "GET_EXCEL",
            "txtFromDate": txtFromdate,
            "txtToDate": txtTodate,
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
            "modelCategory": modelCategory
        };
        var url = "server/mplineinput.aspx";

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
    
    function updateFields(cal) {
        var date = cal.selection.get();
        if (date) {
            date = Calendar.intToDate(date);
            cal.inputField.value = Calendar.printDate(date, "%Y-%m-%d") +
             " " + ("0" + cal.getHours()).right(2) + ":" + ("0" + cal.getMinutes()).right(2);
        }
    };


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

    $("#txtModel").change(function() {
        var v = $(this).val();
        if (v.length > 10 || v.length == 0) {
            $("#<%=hfModel.ClientID %>").val(v);
        }
    });

    window.onload = function() {
        EndRequestHandler();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    };

   </script>

</asp:Content>


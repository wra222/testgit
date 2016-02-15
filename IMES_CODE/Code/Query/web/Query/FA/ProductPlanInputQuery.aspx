<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ProductPlanInputQuery.aspx.cs" Inherits="Query_ProductPlanInputQuery" %>

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
    
    <script type="text/javascript"></script>
                     
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <center>
            <fieldset style="border: solid #000000">
                <legend>Product Plan Input Query</legend>
                <table border="0" width="100%" style="border-width:thin;">
                    <tr>
                        <td width ="10%">
                            <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="40%">
                            <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                        </td>
                        <td width="50%">
                            <asp:Label ID="lblModel" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>
                            <input type="text" id="txtModel" style="width: 300px" maxlength="14" class="iMes_textbox_input_Yellow" />
                            <input id="BtnBrowse" type="button" value="Browse"  onclick="UploadModelList()" />               
                            <asp:HiddenField ID="hfModel" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <%--<td width="10%"></td>--%>
                        <td width="50%" colspan="2">
                            <input type="radio" name="rbPeriod" class="rbPeriod" id="rbPeriodByPeriod" value="GET_PERIOD" checked="checked" />By InputDate
                            <asp:Label ID="Label2" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
                            <asp:TextBox id="txtPeriodFromDate" class="txtPeriod" runat="server" Width="120px" Height="20px"></asp:TextBox>                            
                            <button id="btnPeriodFromDate" type="button" style="width: 20px">...</button>
                            <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                            <asp:TextBox ID="txtPeriodToDate" class="txtPeriod" runat="server" Width="120px" Height="20px"></asp:TextBox>
                            <button id="btnPeriodToDate" type="button" style="width: 20px">...</button>                               
                            
                            <br />
                            <input type="radio" name="rbPeriod" class="rbPeriod" id="rbPeriodByDay" value="GET_DAY" />By ShipDate
                            <asp:TextBox id="txtFromDate" runat="server" Width="150px" Height="20px"></asp:TextBox>
                            <button id="btnFromDate" type="button" style="width: 20px">...</button>
                        </td>
                        <td>
                            <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="Query();" style="width: 100px">Query</button>
                            <input type="hidden" id="hidFromData" runat="server" />
                            <input type="hidden" id="hidToData" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="InputLine/PlanLine:" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:ListBox ID="lboxPdLine" runat="server" SelectionMode="Multiple" Height="95%" 
                                            Width="200px" CssClass="CheckBoxList">
                                    </asp:ListBox>                        
                                    <asp:HiddenField ID="hfLineShife" value="true" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                                            style="width: 100px; display: none;">Export</button>
                            <button id="btnExportDetail"  runat="server" onserverclick="btnExportDetail_Click" 
                                            style="width: 100px; display: none;">ExportDetail</button>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>        
                    <asp:HiddenField ID="hidPeriod" runat="server" />
                    <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="220px"
                                        Width="98%" GvExtWidth="98%" Height="1px" ShowFooter="True" 
                        style="top: 0px; left: 0px" onrowdatabound="gvQuery_RowDataBound" >
                    </iMES:GridViewExt>
                    <asp:LinkButton ID="lbtFreshPage" runat="server" OnClientClick="return ReloadImesPage();" onclick="lbtFreshPage_Click"></asp:LinkButton>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
            <hr />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>        
                    <iMES:GridViewExt ID="gvQuery_Detail" runat="server" AutoGenerateColumns="true" GvExtHeight="220px"
                                        Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px">
                    </iMES:GridViewExt>
                    <asp:LinkButton ID="lbtFreshPage2" runat="server" OnClientClick="return ReloadImesPage();" onclick="lbtFreshPage_Click"></asp:LinkButton>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQueryDetailTotal" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnQueryDetail" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
           
        </center>
        <asp:HiddenField ID="hidUser" runat="server" />       
        <asp:HiddenField ID="hidprocess" runat="server" />              
        <asp:HiddenField ID="hidsource" runat="server" />   
        <asp:HiddenField ID="hidModelList" runat="server" />   
        <asp:HiddenField ID="hidPdLineList" runat="server" />   
        <asp:HiddenField ID="hidredValue" runat="server" />   
        <asp:HiddenField ID="hidshipDate" runat="server" />
        <asp:HiddenField ID="hidpdLine" runat="server" />
        <asp:HiddenField ID="hidmodel" runat="server" />
        <asp:HiddenField ID="hidinputPdLine" runat="server" />
        <asp:HiddenField ID="hidstation" runat="server" />
        <asp:HiddenField ID="hidtype" runat="server" />
           
        
        
        <button id="btnQueryDetail" runat="server"  onserverclick="QueryDetailClick" style="display: none"></button> 
        <button id="btnQueryDetailTotal" runat="server"  onserverclick="QueryDetailTotalClick" style="display: none"></button>    
    </div>


    <script language="javascript" type="text/javascript">

        var inputObj;        
          
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

        function EndRequestHandler(sender, args) {
            $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select...' }).multiselectfilter();
            $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
            $("#<%=gvQuery.ClientID%> tr:nth-child(n+2) ").click(function() { rowclick(); });

            Calendar.setup({
                trigger: "btnFromDate",
                inputField: "<%=txtFromDate.ClientID%>",
                onSelect: function() { updateCalendarFields, $("#rbPeriodByDay").attr("checked", "true"); },
                onTimeChange: function() { updateCalendarFields, $("#rbPeriodByDay").attr("checked", "true"); },
                dateFormat: "%Y-%m-%d",
                minuteStep: 1
            });

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

            $("#<%=txtFromDate.ClientID %>").focus(function() {
            $("#rbPeriodByDay").attr("checked", "true");
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

        function UploadModelList() {
            var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
            var saveasUrl = "UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelList.ClientID %>").value;
            var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
            if (dlgReturn) {
                dlgReturn = dlgReturn.replace(/\r\n/g, ",");
                document.getElementById("<%=hfModel.ClientID %>").value = RemoveBlank(dlgReturn);
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
                }
                model = model.substring(0, model.length - 1)
            }
            return model;
        }

        function Query() {
            var action = $(".rbPeriod:checked").val();
            document.getElementById("<%=hidredValue.ClientID %>").value = action;
            if (document.getElementById("<%=txtFromDate.ClientID %>").value == "") {
                alert('Please input Date'); return;
            }
            var pdline = '';
            $("#ctl00_iMESContent_lboxPdLine option:selected").each(function() {
                pdline = pdline + $(this).val() + ',';
            });

            document.getElementById("<%=hidPdLineList.ClientID %>").value = pdline;
            if (document.getElementById('txtModel').value != "") {
                document.getElementById("<%=hfModel.ClientID %>").value = document.getElementById('txtModel').value;
            }
            beginWaitingCoverDiv();
            document.getElementById("<%=btnQuery.ClientID%>").click();
        }

        function cleanhfModel() {
            document.getElementById("<%=hfModel.ClientID %>").value = "";
            document.getElementById("<%=hidPdLineList.ClientID %>").value = "";
        }

        function SelectDetail(shipDate,pdLine,model,inputPdLine,station,type) {
            document.getElementById("<%=hidshipDate.ClientID%>").value = shipDate;
            document.getElementById("<%=hidpdLine.ClientID%>").value = pdLine;
            document.getElementById("<%=hidmodel.ClientID%>").value = model;
            document.getElementById("<%=hidinputPdLine.ClientID%>").value =inputPdLine ;
            document.getElementById("<%=hidstation.ClientID%>").value = station;
            document.getElementById("<%=hidtype.ClientID%>").value = type;

            beginWaitingCoverDiv();
            document.getElementById("<%=btnQueryDetail.ClientID%>").click();

        }

        function SelectDetailTotal(shipDate, station, type) {
            document.getElementById("<%=hidshipDate.ClientID%>").value = shipDate;
            document.getElementById("<%=hidstation.ClientID%>").value = station;
            document.getElementById("<%=hidtype.ClientID%>").value = type;


            var pdline = '';
            $("#ctl00_iMESContent_lboxPdLine option:selected").each(function() {
                pdline = pdline + $(this).val() + ',';
            });

            document.getElementById("<%=hidpdLine.ClientID %>").value = pdline;
            document.getElementById("<%=hidinputPdLine.ClientID%>").value = pdline;
            if (document.getElementById('txtModel').value != "") {
                document.getElementById("<%=hfModel.ClientID %>").value = document.getElementById('txtModel').value;
            }
            document.getElementById("<%=hidmodel.ClientID%>").value = document.getElementById("<%=hfModel.ClientID %>").value;
            beginWaitingCoverDiv();
            document.getElementById("<%=btnQueryDetailTotal.ClientID%>").click();

        }

        
        
        
    </script>
    <script type="text/javascript">
          //EndRequestHandler();
    </script>
</asp:Content>
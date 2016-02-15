<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for UploadShipmentData Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI Upload Shipment Data to SAP.docx –2011/10/26 
 * UC:CI-MES12-SPEC-PAK-UC Upload Shipment Data to SAP.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-11  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="UploadShipmentData.aspx.cs" Inherits="PAK_UploadShipmentData" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

       <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
	<script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../CommonControl/calendar/calendar.js"></script>
    <script type="text/VBscript" src="../CommonControl/calendar/calendar.vbs"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0" EnablePageMethods="true">
        </asp:ScriptManager>
        <center>
          
                          <fieldset style="width: 98%">
                            <table width="100%">
                                <tr>
                                    <td align="right" style="width:15%">
                                        <asp:Label ID="lblFrom" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td align="left" style="width:35%">
                                        <input type="text" id="txtDateFrom" readonly="readonly" onfocus="CalDisappear();" />
	                                    <button type="button" id="btnFrom" onclick="showCalFrame(txtDateFrom)">...</button>
                                    </td>
                                       <td style="width:5%">
                                     <asp:Label ID="lblDnList" Text="DN:" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                     
                                    <asp:TextBox ID="txtDN" runat="server" Height="19px" Width="200px"></asp:TextBox>
                                      <input id="BtnBrowse" type="button" value="Browse"  onclick="UploadDNList()" />
                                    </td>
                                        <td style="width:10%" />
                                </tr>                
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <input type="text" id="txtDateTo" readonly="readonly" onfocus="CalDisappear();" />
	                                    <button type="button" id="btnTo" onclick="showCalFrame(txtDateTo)">...</button>
                                    </td>
                                    <td style="width:5%">
                                       <asp:Label ID="Label1" Text="Status:" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td style="width:35%"  align="left">
                                    
                                           <imes:cmbconstvaluetype ID="cmbStatus" runat="server" Width="30" 
                                               IsPercentage="true" />
                                    </td>
                                    <td align="right">
                                        <input type="button" value="Query" onclick="Query()" style="width: 100px" />
                                      
                                    </td>
                                </tr>                             
                            </table>
                        </fieldset>
             
              <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblLabelList" runat="server" CssClass="iMes_label_13pt" Text="DN  List"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                <tr style="height:auto">
                <td align="right" > <input type="button" id="btnUpload" value="Upload" onclick="Upload()" style="width: 100px; display:none" /></td>
                </tr>
                    <tr>
                        <td>
                        <imes:gridviewext ID="gvDN" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="320px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                    Width="99.9%" Height="300px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                      onrowdatabound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreate"
                                    HighLightRowPosition="3" HorizontalAlign="Left" >
                                    <Columns>
                                       <asp:TemplateField HeaderText="">

                                            <ItemTemplate>
                                                <asp:CheckBox id="chk" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Date" />
                                        <asp:BoundField DataField="DN" />
                                        <asp:BoundField DataField="PN" />
                                        <asp:BoundField DataField="Model" />
                                        <asp:BoundField DataField="Qty" />
                                        <asp:BoundField DataField="Status" />
                                        <asp:BoundField DataField="Pack" />
                                        <asp:BoundField DataField="PAQC" />
                                        <asp:BoundField DataField="Flag" />
                                          <asp:BoundField DataField="Udt" />
                                    </Columns>
                                </imes:gridviewext>
                        </td>
                    </tr>
                </table>
            </fieldset>
             
             
           <asp:HiddenField ID="hidFromDate" runat="server" />
              <asp:HiddenField ID="hidToDate" runat="server" />
      <asp:HiddenField ID="hidDNList" runat="server" />
        </center>
    </div>

    <script type="text/javascript">
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var defaultRow = '<%=defaultRow%>';
        var tblDN;
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgNoSelData = '<%=this.GetLocalResourceObject(Pre + "_msgNoSelData").ToString()%>';
        var tblDNObj;

        window.onload = function() {
            d = new Date();
            now_year = d.getYear();
            now_month = d.getMonth() + 1;
            now_month = now_month >= 10 ? now_month : "0" + now_month;
            now_date = d.getDate();
            now_date = now_date >= 10 ? now_date : "0" + now_date;
            formattedDate = now_year + "-" + now_month + "-" + now_date;
            document.getElementById("txtDateFrom").value = formattedDate;
            document.getElementById("txtDateTo").value = formattedDate;
            document.getElementById("<%=hidFromDate.ClientID%>").value = formattedDate;
            document.getElementById("<%=hidToDate.ClientID%>").value = formattedDate;
            tblDNObj = $("#" + "<%=gvDN.ClientID %>");
            tblDN = "<%=gvDN.ClientID %>";
            //    ClearGvExtTable(tblDN, defaultRow + 1);
            RestTable();
            //  ClearAll();
        }
        initCalFrame("../CommonControl/calendar/");
        function RestTable() {

            ClearGvExtTable(tblDN, defaultRow + 1);
       //     return;
            //
            tblDNObj.find(" tr").each(function(i, obj) {
            var tr = $(this);
                if (i > 10) {
                    tr.remove();
                }
            
            });
     
        }
        function Query() {
            RestTable();
            CalDisappear();
            ShowInfo("");
            var _f = document.getElementById("txtDateFrom").value;
            var _t = document.getElementById("txtDateTo").value; //txtDN
            var _s;
            if (document.getElementById("<%=txtDN.ClientID%>").value.trim() != "") {
               _s = document.getElementById("<%=txtDN.ClientID%>").value;
            }
            else {
                _s = document.getElementById("<%=hidDNList.ClientID%>").value;
            }
            var _st = getConstValueTypeCmbValue();
            beginWaitingCoverDiv();
            PageMethods.Query(_f, _t, _s, _st, onQuerySucceed, onQueryFail);
     
        }
 
        function onQuerySucceed(result) {
            endWaitingCoverDiv();
            BindGrDN(result);
        }
        function onQueryFail(result) {
            endWaitingCoverDiv();
            ShowAllMsg(result.get_message());
        }
        function Upload() {
            var _c = 0;
            var dnArr = [];
            var dnUdtArr = [];
            tblDNObj.find(" tr:gt(0)").each(function(i, obj) {
                var tr = $(this);
                var _q = tr.find("td:eq(7)").text();
                var _d = tr.find("td:eq(2)").text();
                var _udt = tr.find("td:eq(10)").text();
                var _ox = tr.find("td:eq(0)").find('input:checkbox');
                if (_ox.length > 0 && _ox.attr("checked") == "checked") {
                   _c = parseInt(_c) + parseInt(_q);
                    dnArr.push(_d);
                    dnUdtArr.push(_udt);
                }
             });
            if (dnArr.length == 0) {
                alert(msgNoSelData);
                return;
            }
			/*
            if (_c > 100) {
                alert("Qty 數量超過上傳限制：1000台，請拿掉部分DN後在上傳!");
                return;
            }
			*/
            beginWaitingCoverDiv();
            PageMethods.Upload(mpUserInfo, dnArr, dnUdtArr, onUploadSucceed, onUploadFail);
        }
        function onUploadSucceed() {
            ClearGvExtTable(tblDN, defaultRow + 1);
            Query();
            ShowInfo(msgSuccess, "green");
        }
        function onUploadFail(result) {
            endWaitingCoverDiv();
            ShowAllMsg(result.get_message());
        }
        function BindGrDN(lst) { // "CustSN", "PrdId", "Model", "Station", "Scanned SN"
            
            var dateStr;
            var udt;
             for (var i = 0; i < lst.length; i++) {
                var rowArray = new Array();
                rowArray.push("");
                rowArray.push(lst[i].m_date);
                rowArray.push(lst[i].m_dn);
                rowArray.push(lst[i].m_pn);
                rowArray.push(lst[i].m_model);
                rowArray.push(lst[i].m_qty);
                rowArray.push(lst[i].m_status);
                rowArray.push(lst[i].m_pack);
                rowArray.push(lst[i].m_paqc);
                rowArray.push(lst[i].m_bAllowUpload);
                rowArray.push(lst[i].m_udt);
                if (i < defaultRow) {
                    eval("ChangeCvExtRowByIndex_" + tblDN + "(rowArray, false, i+1);");
                    setSrollByIndex(i, true, tblDN);
                }
                else {
                //    eval("AddCvExtRowToBottom_" + tblDN + "(rowArray,true)");
                  eval("rw = AddCvExtRowToBottom_" + tblDN + "(rowArray, false);");
                    setSrollByIndex(i, true, tblDN);
               
                }
            }
          setRowSelectedOrNotSelectedByIndex(i - 1, false, tblDN);
            var _h = false;
            var chx;
            tblDNObj.find(" tr:gt(0)").each(function(i, obj) {
           var tr = $(this);
           tr.find("td:eq(9)").css('display', 'none');
           tr.find("td:eq(10)").css('display', 'none');
       
                var _b = tr.find("td:eq(9)").text();
                chx = "";
                if (_b == "True") {
                    chx = "<input type='checkbox' id='chxDN'  name='checkboxName'>";
                    chx = chx.replace('chxDN', 'chxDN' + i).replace('checkboxName', 'checkboxName' + i);
                    tr.find('td:eq(0)').append(chx);
                    _h = true;
                }
                else {
                    tr.find('td:eq(0)').html("");
                }

            });
            tblDNObj.find(" tr").find("th:eq(0)").html("");
            if (_h) {
                chx = "<input type='checkbox' id='chxDNALL' onclick='checkAll(this)'  name='checkboxALL'>";
                $("#btnUpload").show();
                tblDNObj.find(" tr").find("th:eq(0)").append(chx);
            }
            else {
                $("#btnUpload").hide();
            }
            //tblDNObj.find(" tr:eq(0)").find("th:eq(9)").width(0);
          //    tblDNObj.find(" tr:eq(0)").find("th:eq(10)").width(0);
        }
    
        function checkAll(header) {
          $('#<%= gvDN.ClientID %> input[type=checkbox]').prop("checked", header.checked);
        }
     
        function UploadDNList() {
            var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
            var saveasUrl = "InputDNlist.aspx?DNList=" + document.getElementById("<%=hidDNList.ClientID %>").value;
            var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
            if (dlgReturn) {
                dlgReturn = dlgReturn.replace(/\r\n/g, ",");
                document.getElementById("<%=hidDNList.ClientID %>").value = RemoveBlank(dlgReturn);
            }
            else {
                if (dlgReturn == "")
                { document.getElementById("<%=hidDNList.ClientID %>").value = ""; }
                return;
            }
        }
        function RemoveBlank(dnList) {
            var arr = dnList.split(",");
            var dn = "";
            if (dnList != "") {
                for (var m in arr) {
                    if (arr[m] != "") {
                        dn = dn + arr[m] + ",";
                    }
                }
                dn = dn.substring(0, dn.length - 1)
            }
            return dn;
        }
        function ShowAllMsg(msg) {
            ShowMessage(msg);
            ShowInfo(msg);
        }
        
    </script>

</asp:Content>

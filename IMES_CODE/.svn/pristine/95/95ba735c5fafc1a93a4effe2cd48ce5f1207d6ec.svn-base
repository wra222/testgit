<%--
/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:UI for PAQC Output For RCTO Page
* UI:CI-MES12-SPEC-PAK-UI PAQC Output_RCTO.docx –2012/6/11 
* UC:CI-MES12-SPEC-PAK-UC PAQC Output_RCTO.docx –2012/7/10            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-07-25   Jessica Liu           Create
* Known issues:
* TODO：
* ITC-1428-0064, Jessica Liu, 2012-9-17
* ITC-1428-0068, Jessica Liu, 2012-9-17
* ITC-1428-0069, Jessica Liu, 2012-9-17
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PAQCOutputForRCTO.aspx.cs" Inherits="RCTO_PAQCOutput" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <%-- Jessica Liu, 2012-9-11--%>
            <asp:ServiceReference Path="~/PAK/Service/WebServicePAQCOutputForRCTO.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 80px;" />
                    <col />
                    <col style="width: 80px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblPDLine" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtPdLine" runat="server" CssClass="iMes_textbox_input_Disabled"
                            Width="95%" IsClear="true" ReadOnly="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblOKQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblOKQtyContent" runat="server" CssClass="iMes_label_11pt" Text="0"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNGQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNGQtyContent" runat="server" CssClass="iMes_label_11pt" Text="0"></asp:Label>
                    </td>
                </tr>
            </table>
            <hr />
            <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblProductInfo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
                <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col style="width: 110px;" />
                        <col />
                        <col style="width: 110px;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:Label ID="lblCustSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblCustSNContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductIDContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblMBSNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblMBSNoContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
             <fieldset style="width: 98%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblDefectList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td>
                        <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                            GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
                            HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                        </iMES:GridViewExt>
                    </td>
                </tr>
            </table>
            </fieldset>
            <div id="div3">
                <table width="100%">
                    <tr>
                        <td style="width: 110px;">
                            <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                        </td>
                        <td>
                            <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="99%" IsClear="true" IsPaste="true" />
                        </td>
                        <td style="width: 150px;">
                            <asp:CheckBox ID="ScanChk" runat="server" CssClass="iMes_CheckBox" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var scanFlag = false;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var defectCount = 0;
        var defectInTable = [];
        var passQty = 0;
        var failQty = 0;
        var gprodId = "";
        var editor;
        var customer;
        var stationId;
        var inputObj;
        var emptyPattern = /^\s*$/;

        var productID = "";
        var customerSN = "";

        //2012-7-20
        var MBSno = "";

        //error message
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputSN = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputSN").ToString() %>';
        var msgInputPdLine = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputPdLine") %>'
        //2012-7-17
        var msgWrongCode = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgWrongCode").ToString() %>';
        //2012-7-20
        var msgBadProductID = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgBadProductID").ToString() %>';
        var msgInputProdIdFirst = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputProdIdFirst").ToString() %>';
        //2012-9-11
        var msgInputMBSN = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMBSN").ToString() %>';
        //2012-9-17
        var msgInput9999 = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInput9999").ToString() %>';
        
		var needCheckMB = true;
		
        window.onload = function() {

            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';

            document.getElementById("<%=ScanChk.ClientID%>").checked = false;
            //getPdLineCmbObj().selectedIndex = 0;
            initPage();
            callNextInput();

        };

        window.onbeforeunload = function() {

            OnCancel();

        };
        function initPage() {
            tbl = "<%=gd.ClientID %>";           
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblOKQtyContent.ClientID %>"), passQty);
            setInputOrSpanValue(document.getElementById("<%=lblNGQtyContent.ClientID %>"), failQty);
            //2012-7-20
            setInputOrSpanValue(document.getElementById("<%=lblMBSNoContent.ClientID %>"), "");
            MBSno = "";
            
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            inputFlag = false;
            defectCount = 0;
            defectInTable = [];
            productID = "";
            customerSN = "";
			needCheckMB = true;
        }

        //2012-7-20
        function isProductID(input) {
            if (input.length != 9 && input.length != 10) {
                return false;
            }

            //Jessica Liu, 2012-9-11
            if (input.length == 9) {
                return true;
            }
            
            if (input.length == 10) {
                if (input.substr(4, 1) != 'M' && input.substr(4, 1) != 'B') {
                    return true;
                }
            }

            return false;
        }
        
        //2012-7-20
        function isMBSno(input) {
            var ret = false;
            //SubStringSN(input, Type)
            if ((input.length == 11) && (input.substr(4, 1) == "M" || input.substr(4, 1) == "B")) {
                ret = true;
            }
            else if ((input.length == 10) && (input.substr(4, 1) == "M" || input.substr(4, 1) == "B")) {
                ret = true;
            }
            else if ((input.length == 11) && (input.substr(5, 1) == "M" || input.substr(5, 1) == "B")) {
                ret = true;
            }
            return ret;
//            if ((input.length == 10 || input.length == 11) && (input.substr(4, 1) == "M")) {
//                ret = true;
//            }
//            else if ((input.length == 11) && (input.substr(5, 1) == "M")) {
//                ret = true;
//            }

//            return ret;
        }
		function isMAC(input) {
            var ret = false;
            if (input.length == 12) {
                ret = true;
            }
            return ret;
        }

        //2012-7-20
        function isProductIDOrMBSno(input) {
            return isProductID(input) || isMBSno(input) || isMAC(input);
        }
        

        function input(inputData) {

            scanFlag = !document.getElementById("<%=ScanChk.ClientID%>").checked;

            if (inputData == "7777") {
                ResetPage();
            }
            else if (inputData == "9999" && scanFlag) {
                //ITC-1428-0064, Jessica Liu, 2012-9-17
                if (productID == "") {
                    alert(msgInputProdIdFirst);
                    callNextInput();
                    return;
                }
                if (needCheckMB){
					var mbsnoInfo = getInputOrSpanValue(document.getElementById("<%=lblMBSNoContent.ClientID %>"));
					if (mbsnoInfo == "")
					{
						alert(msgInputMBSN);
						callNextInput();
						return;
					}
				}
            
                ShowInfo("");
                save();
            }
            else if (isProductID(inputData)) {
				ShowInfo("");
				beginWaitingCoverDiv();
				productID = SubStringSN(inputData, "ProdId");
				//2012-9-7
				var line = document.getElementById("<%=txtPdLine.ClientID%>").innerText; //getPdLineCmbValue();
				
				needCheckMB = true;
				WebServicePAQCOutputForRCTO.input(line, productID, editor, stationId, customer, inputSucc, inputFail);
			}
			else if (needCheckMB && (isMBSno(inputData) || isMAC(inputData))) {
				if (productID != "") {

					//2012-9-17         
					var mbsnoInfo = getInputOrSpanValue(document.getElementById("<%=lblMBSNoContent.ClientID %>"));
					if (mbsnoInfo != "") {
						alert(msgInput9999);
						callNextInput();
						return;
					}
					
					ShowInfo("");
					beginWaitingCoverDiv();
					//MBSno = SubStringSN(inputData, "MB");
					MBSno = inputData;

					//2012-9-7
					var line = document.getElementById("<%=txtPdLine.ClientID%>").innerText; //getPdLineCmbValue();
					
					WebServicePAQCOutputForRCTO.checkmb(line, MBSno, productID, editor, stationId, customer, checkSucc, checkFail);
				}
				else {
					alert(msgInputProdIdFirst);
					callNextInput();
				}
			}
            else if (inputData.length == 4) {
                inputDefect(inputData);
            }
            else {
                alert(msgWrongCode);
                callNextInput();
            }
        }

        function inputSucc(result) {

            endWaitingCoverDiv();
            setInfo(result);
          
            needCheckMB = ("N" != result[3]);
			if (! needCheckMB){
				if (!scanFlag) {
					ShowInfo("");
					save();
				}
			}
            callNextInput();
        }

        function inputFail(result) {
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        //2012-7-20
        function checkSucc(result) {
            endWaitingCoverDiv();
            setInputOrSpanValue(document.getElementById("<%=lblMBSNoContent.ClientID %>"), MBSno);
            //2012-9-17
            //callNextInput();
            if (!scanFlag) {
                ShowInfo("");
                save();
            }
            else {
                callNextInput();
            }            
        }

        //2012-7-20
        function checkFail(result) {
            endWaitingCoverDiv();
            
            currentMBSno = "";
            tbl = "<%=gd.ClientID %>";
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            defectCount = 0;
            defectInTable = [];
            
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function isPass() {
            if (defectCount == 0) {
                return true;
            }

            return false;
        }

        function setInfo(info) {
            //set value to the label
            productID = info[0]["ProductID"];
            //2012-7-20
            customerSN = info[0]["CustSN"];
            
            setInputOrSpanValue(document.getElementById("<%=lblCustSNContent.ClientID %>"), info[0]["CustSN"]);
            setInputOrSpanValue(document.getElementById("<%=lblProductIDContent.ClientID %>"), productID);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), info[0]["Model"]);
            setInputOrSpanValue(document.getElementById("<%=txtPdLine.ClientID %>"), info[2]);
            //set defectCache value
            defectCache = info[1];
        }

        function save() {
            beginWaitingCoverDiv();
            var prodId = document.getElementById("<%=lblProductIDContent.ClientID %>").innerText;

            WebServicePAQCOutputForRCTO.save(prodId, defectInTable, saveSucc, saveFail);
        }

        function saveSucc(result) {

            endWaitingCoverDiv();
            //ShowSuccessfulInfo(true);

            if (isPass()) {
                passQty++;
                setInputOrSpanValue(document.getElementById("<%=lblOKQtyContent.ClientID %>"), passQty);
            }
            else {
                failQty++;
                setInputOrSpanValue(document.getElementById("<%=lblNGQtyContent.ClientID %>"), failQty);
            }

            //2012-7-20
            //var tmpinfo = customerSN;
            var tmpinfo = productID;

            ResetPage();
            
            //2012-7-20
            //ShowSuccessfulInfoFormat(true, "Customer SN", tmpinfo);
            ShowSuccessfulInfoFormat(true, "Product ID", tmpinfo);
            
            callNextInput();
        }


        function saveFail(result) {
            endWaitingCoverDiv();            
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function inputDefect(data) {
            //ITC-1428-0069, Jessica Liu, 2012-9-17
            if (productID == "") {
                alert(msgInputProdIdFirst);
                callNextInput();
                return;
            }
			if (needCheckMB){
				var mbsnoInfo = getInputOrSpanValue(document.getElementById("<%=lblMBSNoContent.ClientID %>"));
				if (mbsnoInfo == "") {
					alert(msgInputMBSN);
					callNextInput();
					return;
				}
			}

            if (isExistInTable(data)) {
                //error message
                alert(msgDuplicateData);
                callNextInput();
                return;
            }

            if (isExistInCache(data)) {
                var desc = getDesc(data);
                var rowArray = new Array();
                var rw;

                rowArray.push(data);
                rowArray.push(desc);

                //add data to table
                if (defectInTable.length < 12) {
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, defectInTable.length + 1);");
                    setSrollByIndex(defectInTable.length, true, tbl);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    setSrollByIndex(defectInTable.length, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }

                defectInTable[defectCount++] = data;
                callNextInput();
            }
            else {
                alert(msgInputValidDefect);
                callNextInput();
            }
        }

        function isExistInTable(data) {
            if (defectInTable != undefined && defectInTable != null) {
                for (var i = 0; i < defectInTable.length; i++) {
                    if (defectInTable[i] == data) {
                        return true;
                    }
                }
            }

            return false;
        }

        function isExistInCache(data) {
            if (defectCache != undefined && defectCache != null) {
                for (var i = 0; i < defectCache.length; i++) {
                    if (defectCache[i]["id"] == data) {
                        return true;
                    }
                }
            }

            return false;
        }

        function getDesc(data) {
            if (defectCache != undefined && defectCache != null) {
                for (var i = 0; i < defectCache.length; i++) {
                    if (defectCache[i]["id"] == data) {
                        return defectCache[i]["description"];
                    }
                }
            }

            return "";
        }

        function OnCancel() {
            if (productID != "") {
                WebServicePAQCOutputForRCTO.cancel(productID);
            }
        }

        function callNextInput() {

            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }

        function ExitPage() {
            OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
            callNextInput();
        }                                                                                                                                   
    </script>

</asp:Content>

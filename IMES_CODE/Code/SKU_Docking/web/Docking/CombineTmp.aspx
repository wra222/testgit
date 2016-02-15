<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Tmp For Docking
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* Known issues:
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CombineTmp.aspx.cs" Inherits="CombineTmp" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
			<asp:ServiceReference Path="~/Docking/Service/WebServiceCombineTmp.asmx" />
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
                <!--<tr>
                    <td>
                        <asp:Label runat="server" ID="lblPDLine" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="CmbPdLine1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>-->
            </table>
            <fieldset style="width: 100%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblProductInfo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>   
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td style="width:10%">
                            <asp:Label ID="lbCustSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                         </td>
                        <td style="width: 25%;">
                           <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="txtCustSN" runat="server" CssClass="iMes_label_13pt" />
                                </ContentTemplate>
                            </asp:UpdatePanel>                            
                         </td>
                        <td style="width: 10%;">
                            <asp:Label ID="lbProdId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td style="width: 25%;">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="txtProdId" runat="server" CssClass="iMes_label_13pt" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                         </td>
                    </tr>
                    <tr>
                        <td style="width:10%">
                            <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td  style="width: 25%;">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_13pt" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                         </td>
                    </tr>
                </table>
            </fieldset> 
            
            <fieldset style="width: 100%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblCollectionData" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </legend>   
                <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td colspan="5">
                        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGridFresh" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick"></asp:AsyncPostBackTrigger>
                            </Triggers>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick" />
                            </Triggers>
                            <ContentTemplate>
                                    <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true" GvExtWidth="100%"  GvExtHeight="240px" 
                                    OnGvExtRowClick="" OnGvExtRowDblClick="showCollection(this)"  Width="99.9%" Height="230px"
                                    SetTemplateValueEnable="true"  GetTemplateValueEnable="true"  HighLightRowPosition="3" 
                                    onrowdatabound="GridViewExt1_RowDataBound" HorizontalAlign="Left" >
                                     <Columns>
                                        <asp:BoundField DataField="Image" />
                                        <asp:BoundField DataField="SubstitutePartNo" />
                                        <asp:BoundField DataField="SubstituteDescr" />
                                        <asp:BoundField DataField="PartNo" />
                                        <asp:BoundField DataField="PartType"  />
                                        <asp:BoundField DataField="PartDescr"  />
                                        <asp:BoundField DataField="Qty" />
                                        <asp:BoundField DataField="PQty" />
                                        <asp:BoundField DataField="CollectionData" />
                                        <asp:BoundField DataField="HfCollectionData" />
                                        <asp:BoundField DataField="HfPartNo" />
                                    </Columns>
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                </table>
            </fieldset>   
            <div id="div3">
                <table width="100%">
                    <tr>
                    <td align="left" Width="15%">
                        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td   align="left"  Width="75%">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnGridFresh" runat="server" type="button" onclick="" style="display: none"
                                    onserverclick="FreshGrid">
                                </button>
                                <button id="btnGridClear" runat="server" type="button" onclick="" style="display: none"
                                    onserverclick="clearGrid">
                                </button>
                                <input id="prodHidden" type="hidden" runat="server" />
                             
                                <input id="sumCountHidden" type="hidden" runat="server" />
                                <input type="hidden" runat="server" id="station" />
                                 <input type="hidden" runat="server" id="useridHidden" />
                                 <input type="hidden" runat="server" id="hidRowCnt" /> 
                                <input id="scanQtyHidden" type="hidden" runat="server" value="0" />
                                 <input type="hidden" runat="server" id="pCode" />

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
    
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgCollectNoItem = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectNoItem") %>';
        var msgCollectExceedCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedCount") %>';
        var msgCollectExceedSumCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedSumCount") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgProdIdError = '<%=this.GetLocalResourceObject(Pre + "_mesProdIDError").ToString() %>';
        var msgMBSNError = '<%=this.GetLocalResourceObject(Pre + "_mesSNError").ToString() %>';
        var msgWrongCode = '<%=this.GetLocalResourceObject(Pre + "_mesWrongCode").ToString() %>';
        var msgProdIdisNull = '<%=this.GetLocalResourceObject(Pre + "_mesProdIdisNull").ToString() %>';

        var prodid = "";
        var model = "";
        var passQty = 0;
        var mbSNo = "";
        var dcode = "";
        var modelId = "";

        var hidMBSn = "";

        var tmpDCode = "";
        var customer;
        //¼ÇÂ¼Ë¢µÄpart snÊýÄ¿
        var scanQty = 0;
        var sumQty = 0;
        var gvClientID = "<%=GridViewExt1.ClientID %>";
        var gvTable = document.getElementById(gvClientID);
        var strRowsCount = "<%=initRowsCount%>";
        var initRowsCount = parseInt(strRowsCount, 10) + 1;

        var mbormac = "";
        var codetype = "";

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //| Name		:	onload
        //| Author		:	Lucy Liu
        //| Create Date	:	10/27/2009
        //| Description	:	¼ÓÔØ½ÓÊÜÊäÈëÊý¾ÝÊÂ¼þ²¢ÖÃ½¹µã
        //| Input para.	:	
        //| Ret value	:	
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        document.body.onload = function() {
            try {
                //        setPdLineCmbFocus()
                customer = "<%=customer%>";

                getAvailableData("processDataEntry");
            } catch (e) {
                alert(e.description);
            }

        }

        function ProdIDClear() {
            document.getElementById("<%=txtProdId.ClientID %>").innerText = "";
            strProdID = "";
        }
        function ModelClear() {
            document.getElementById("<%=txtModel.ClientID %>").innerText = "";
            strModel = "";
        }

        //modify ITC-1414-0101 bug
        function TransferData(data, type) {
            data = data.trim().toUpperCase();
            var datalength = parseInt(data.length);

            switch (type) {
                case ("ProdIdorCustSN"):
                    {
                        //if ((datalength == 9 || datalength == 10) && data.substring(0, 2) != "CN" && data.indexOf("-") == -1) {
						if ((datalength == 9 || datalength == 10) && !CheckCustomerSN(data) && data.indexOf("-") == -1) {
                            //modify ITC-1414-0150 BUG
                            var datatmp = data.substring(2, 3);
                            var pattren1 = "[1-9A-C]{1}";
                            var regExp = new RegExp(pattren1);
                            var ret1 = datatmp.search(regExp);
                            if (ret1 != -1) {
                                codetype = "ProdId";
                                return data.substring(0, 9);
                            }
                        }

                        //if (datalength == 10 && data.substring(0, 2) == "CN") {
						if (datalength == 10 && CheckCustomerSN(data)) {
                            codetype = "CustSN";
                            return data;
                        }
                        if ((document.getElementById("<%=txtProdId.ClientID%>").innerText == "") && (datalength == 9 || datalength == 10 || datalength == 12 || datalength == 14) && (codetype != "ProdId" || codetype != "CustSN")) {
                            data = "prodIdnull";
                            return data;

                        }
                        else {
                            //alert(msgWrongCode);
                            data = "WrongCode";
                            return data;

                        }
                        break;
                    }
                case ("VerdorCT"):
                    {
                        if (datalength == 9 || datalength == 10 || datalength == 12 || datalength == 14) {

                        }
                        else {
                            data = "WrongCode";

                        }
                        return data;
                        
                        break;
                    }

                default: { return data; }
            }
        }  

        

        function processDataEntry(inputData) {
            try {
                //document.getElementById("<%=prodHidden.ClientID%>").value = SubStringSN(inputData, "ProdId");

                //    inputData = SubStringSN(inputData, "ProdId");
                var againflag = false;
                
                var errorFlag = false;
                if (!errorFlag) {
                    if (document.getElementById("<%=txtProdId.ClientID%>").innerText == "") {
                        ShowInfo("");

                        //For BN  Custs SN lenth =16 so not need  SubStringSN(...) Modify by Benson
                        var tmpdata = TransferData(inputData, "ProdIdorCustSN");
                        codetype = "";
                        //modify ITC-1414-0073 bug
                        if (tmpdata == "WrongCode") {
                            ShowInfo("");
                            alert(msgWrongCode);
                            getAvailableData("processDataEntry");

                        }
                        else if (tmpdata == "prodIdnull")
                        {
                            ShowInfo("");
                            alert(msgProdIdisNull);
                            getAvailableData("processDataEntry");
                        
                        }
                        else {
                            document.getElementById("<%=prodHidden.ClientID%>").value = tmpdata;
                            beginWaitingCoverDiv();
                            document.getElementById("<%=btnGridFresh.ClientID%>").click(); //Refesh DataGrid..
                            getCommonInputObject().focus();
                            //getAvailableData("processDataEntry");
                        }

                    } else {
                            //modify itc-1414-0093 bug
                            if (inputData == "getbomnull") {

                                WebServiceCombineTmp.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, onSaveSucceed, onSaveFail);

                    }
                    else {
                        var tempdata = TransferData(inputData, "ProdIdorCustSN");
                        var tmpno = TransferData(inputData, "VerdorCT");
                       if (codetype == "ProdId" || codetype == "CustSN") {
                            //modify ITC-1414-0190 BUG
                            ExitPage();
                            codetype = "";
                            againflag = true;
                            document.getElementById("<%=prodHidden.ClientID%>").value = tempdata;
                            beginWaitingCoverDiv();
                            document.getElementById("<%=btnGridFresh.ClientID%>").click(); //Refesh DataGrid..
                            getCommonInputObject().focus();

                        }
                        //modify ITC-1414-0195 BUG
                        if (tempdata == "WrongCode" && tmpno == "WrongCode")
                        {
                            ShowInfo("");
                            ResetPage();
                            alert(msgWrongCode);
                            getAvailableData("processDataEntry");
                        }
                        else {
                            hidMBSn = TransferData(inputData, "VerdorCT");
                            if (hidMBSn == "WrongCode") {
                                ShowInfo("");
                                alert(msgWrongCode);
                                getAvailableData("processDataEntry");

                            }
                            else if (againflag == false){
                                WebServiceCombineTmp.inputMBSn(document.getElementById("<%=txtProdId.ClientID%>").innerText, hidMBSn, onSucceed, onFail);
                            }
                        }
                    }  
                            
                    }
                }

            } catch (e) {
                alert(e.description);
            }

        }
        //ITC-1360-0533
        function onSucceed(result) {
            // endWaitingCoverDiv();
            //ShowInfo("onSucceed0!");
            //ShowMessage("onSucceed1!");
            //WebServiceCombineTmp.SetDataCodeValue(document.getElementById("<%=txtModel.ClientID %>").innerText, customer, setSucc, setFail);
            ////////////////////////////////////////////////////////modify by itc200052, 2012.2.21
            try {
                // endWaitingCoverDiv();
                eval("setRowNonSelected_" + gvClientID + "()");

                var iLength = result.length;
                if (result == null) {
                    endWaitingCoverDiv();
                    var content = msgSystemError;
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }
                else if ((result.length == 3) && (result[0] == SUCCESSRET)) {
                    //处理界面输出信息
                    gvTable = document.getElementById("<%=GridViewExt1.ClientID%>");
                    qtySum = 0;
                    pqtySum = 0;
                    needCheck = true;
                    alreadyMatch = false;
                    var rowCnt = document.getElementById("<%=hidRowCnt.ClientID%>").value;
                    repVC = result[1];
                    for (k = 1; k <= rowCnt; k++) {
                        qty = parseInt(gvTable.rows[k].cells[6].innerText);
                        pqty = parseInt(gvTable.rows[k].cells[7].innerText);
                        qtySum += qty;
                        pqtySum += pqty;
                        if (!alreadyMatch) {
                            //oldVC = "," + gvTable.rows[k].cells[10].innerText + ",";
                            //if (oldVC.indexOf("," + repVC.substring(0, 5) + ",") >= 0) {
                            //    if (qty <= pqty) {
                            //        needCheck = false;
                            //        ShowInfo("Part \"" + repVC + "\" already changed!\nPlease scan/input another part!");
                            //        break;
                            //    }
                            if (gvTable.rows[k].cells[1].innerText.indexOf(repVC) != -1) {
                                //if (gvTable.rows[k].cells[1].innerText.indexOf(repVC.substring(0, 2)) != -1) {
                                eval("setScrollTopForGvExt_" + gvClientID + "('" + gvTable.rows[k].cells[3].innerText + "',3,'','MUTISELECT')");

                                alreadyMatch = true;
                                pqty=1;
                                //pqtySum++;
                                
                                gvTable.rows[k].cells[7].innerText = pqty;
                                collectionData = gvTable.rows[k].cells[8].innerText;
                                // }
                                /*if (collectionData != " ") {
                                    collectionData += "," + result[2];
                                }
                                else {*/
                                    collectionData = result[2];
                                //}
                                gvTable.rows[k].cells[8].innerText = collectionData;
                                gvTable.rows[k].cells[8].title = collectionData;

                            }
                        }
                    }
                    //ShowInfo("qtySum" + qtySum + "pqtySum" + pqtySum);
                    qtySum = 0;
                    pqtySum = 0;
                    for (kk = 1; kk <= rowCnt; kk++) {
                        qty = parseInt(gvTable.rows[kk].cells[6].innerText);
                        pqty = parseInt(gvTable.rows[kk].cells[7].innerText);
                        qtySum += qty;
                        pqtySum += pqty;
                    }
                   
                    
                    if (needCheck == true && qtySum <= pqtySum) {
                        //if (needCheck == true && 1 == pqtySum) {
                        //ResetPage();
                        WebServiceCombineTmp.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, onSaveSucceed, onSaveFail);

                    }
                    endWaitingCoverDiv();
                    callNextInput();
                }
                else {
                    endWaitingCoverDiv();
                    var content = result[0];
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }
            } catch (e) {
                alert(e.description);
            }
            //////////////////////////////////////////////////////////////////////

            //WebServiceCombineTmp.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, lstPrintItem, onSaveSucceed, onSaveFail);

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //| Name		:	clearTable
        //| Author		:	Lucy Liu
        //| Create Date	:	10/27/2009
        //| Description	:	½«Defect List tableµÄÄÚÈÝÇå¿Õ
        //| Input para.	:	
        //| Ret value	:	
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        function clearTable() {
            try {
                ClearGvExtTable("<%=GridViewExt1.ClientID%>", initRowsCount);

            } catch (e) {
                alert(e.description);

            }

        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //| Name		:	setStatus
        //| Author		:	Lucy Liu
        //| Create Date	:	10/27/2009
        //| Description	:	ÉèÖÃÒ³ÃæËùÓÐÓëProdIdÉ¨ÈëÏà¹Ø¿Ø¼þµÄ×´Ì¬
        //| Input para.	:	
        //| Ret value	:	
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        function setStatus(status) {

            try {

               // getPdLineCmbObj().disabled = status;


            } catch (e) {
                alert(e.description);

            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //| Name		:	showCollection
        //| Author		:	Lucy Liu
        //| Create Date	:	10/27/2009
        //| Description	:	ÏÔÊ¾ÏêÏ¸ÐÅÏ¢
        //| Input para.	:	
        //| Ret value	:	
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        function showCollection(row) {
            if (row != null) {
                var qty = row.cells[6].innerText.trim();
                var pqty = row.cells[7].innerText.trim();
                var PN = row.cells[3].innerText.trim();
                var Cldata = row.cells[9].innerText.trim();
                var ClPartNodata = row.cells[10].innerText.trim();

                var dataCllist = Cldata.trim().split(",");
                var dataPartNoCllist = ClPartNodata.trim().split(",");
                if (Cldata == "") {
                    dataCllist.pop();
                }
                if (ClPartNodata == "") {
                    dataPartNoCllist.pop();
                }
                var popParam = new dataInfo(PN, qty, pqty, dataCllist, dataPartNoCllist);
                ShowCollection(popParam);
            }
            else {
                alert("Please select a row");
            }
        }


        function onFail(error) {
            // endWaitingCoverDiv();
            //ShowMessage("Match onFail!");                    

            try {
                //       endWaitingCoverDiv();
                setStatus(true);
                //ShowMessage("Match Error!");

                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
                clearData();
                callNextInput();
            } catch (e) {
                alert(e.description);
                //        endWaitingCoverDiv();
            }
        }



        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //| Name		:	onSaveSucceed
        //| Author		:	Lucy Liu
        //| Create Date	:	10/27/2009
        //| Description	:	±£´æ³É¹¦
        //| Input para.	:	
        //| Ret value	:	
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        function onSaveSucceed(result) {
            //ShowMessage("onSaveSucceed!");                    

            try {
                endWaitingCoverDiv();
                eval("setRowNonSelected_" + gvClientID + "()");
                if (result == null) {
                    setStatus(true);
                    var content = msgSystemError;
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }
                else if (result[0] == SUCCESSRET && result.length == 2) {

                    //modify itc-1360-1688 bug
                    var tmpid = document.getElementById("<%=txtProdId.ClientID%>").innerText;
                    reset();

                    scanQty = 0;
                    sumQty = 0;
                    setStatus(false);
                    callNextInput();
                    ShowSuccessfulInfo(true, "[" + tmpid + "] " + msgSuccess);
                }
                else {
                    setStatus(true);
                    var content = result[0];
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }


            } catch (e) {
                alert(e.description);
                endWaitingCoverDiv();
            }

        }


        function onSaveFail(error) {
            //ShowMessage("onSaveFail!");                    

            try {
                endWaitingCoverDiv();
                eval("setRowNonSelected_" + gvClientID + "()");
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
                callNextInput();
            } catch (e) {
                alert(e.description);
            }
        }


        function reset() {
            try {
                document.getElementById("<%=txtProdId.ClientID%>").innerText = "";
                document.getElementById("<%=txtModel.ClientID%>").innerText = "";
                document.getElementById("<%=txtCustSN.ClientID%>").innerText = "";
                mbSNo = "";
                clearTable();

            } catch (e) {
                alert(e.description);

            }
        }


        function callNextInput() {

            getCommonInputObject().focus();
            getAvailableData("processDataEntry");
        }


        window.onbeforeunload = function() {
            ExitPage();

        }

        function onClearSucceeded(result) {


            try {

                if (result == null) {
                    ShowInfo("");
                    //service·½·¨Ã»ÓÐ·µ»Ø
                    var content = msgSystemError;
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }
                else if ((result.length == 2) && (result[0] == SUCCESSRET)) {

                    //            reset();
                    //           callNextInput();
                    ShowInfo("");

                }
                else {
                    ShowInfo("");
                    var content = result[0];
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }



            } catch (e) {
                alert(e.description);

            }


        }

        function onClearFailed(error) {


            try {


                ShowInfo("");
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
                //ÐèÒªÇå¿Õ½çÃæ
                reset();

                callNextInput();

            } catch (e) {
                alert(e.description);

            }


        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //| Name		:	ExitPage
        //| Author		:	Lucy Liu
        //| Create Date	:	01/24/2010
        //| Description	:	ÍË³öÒ³ÃæÊ±µ÷ÓÃ
        //| Input para.	:	
        //| Ret value	:	
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        function ExitPage() {
            if (document.getElementById("<%=txtProdId.ClientID%>").innerText != "") {

                WebServiceCombineTmp.Cancel(document.getElementById("<%=txtProdId.ClientID%>").innerText, document.getElementById("<%=station.ClientID%>").value, onClearSucceeded, onClearFailed);
                sleep(waitTimeForClear);
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //| Name		:	ResetPage
        //| Author		:	Lucy Liu
        //| Create Date	:	01/24/2010
        //| Description	:	Ë¢ÐÂÒ³ÃæÊ±µ÷ÓÃ
        //| Input para.	:	
        //| Ret value	:	
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        function ResetPage() {
            ExitPage();
            reset();

            getCommonInputObject().value = "";
        }

        function setScanHiddenQty() {

            scanQty = parseInt(document.getElementById("<%=scanQtyHidden.ClientID%>").value, 10);
        }





    </script>

</asp:Content>

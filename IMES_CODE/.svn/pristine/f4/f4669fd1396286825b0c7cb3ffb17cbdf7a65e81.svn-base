<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Board Input(FA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-27  Lucy Liu(EB2)        Create 
 2010-04-07  Lucy Liu(EB2)       Modify:   ITC-1122-0066
 2010-04-07  Lucy Liu(EB2)       Modify:   ITC-1122-0073
 Known issues:
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="BoardInput.aspx.cs" Inherits="FA_BoardInput" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="Service/WebServiceBoardInput.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="5">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true"   />
                         
                    </td>
                </tr>
            </table>
         <table border="0" width="95%">   
     
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="labQty" runat="server" CssClass="iMes_label_13pt">Pass Qty:</asp:Label>
                    </td>
                    <td align="left">
                           <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional" 
                               RenderMode="Inline">
                    <ContentTemplate>
                    <asp:Label ID="txtTotalQty" runat="server" CssClass="iMes_label_13pt">0</asp:Label>
                    </ContentTemplate>
                       
                         </asp:UpdatePanel>
                    </td>
                </tr>                              
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbProdId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtProdId" runat="server" CssClass="label_normal" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtModel" runat="server" CssClass="label_normal" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>                    
                </tr>

                
                <tr>
                    <td colspan="2" align="left">
                        <asp:Label ID="lbCollection" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
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
                                        <asp:BoundField DataField="PartType"  />
                                        <asp:BoundField DataField="PartDescr"  />
                                        <asp:BoundField DataField="PartNo" />
                                        <%--
                                        <asp:BoundField DataField="ValueType" />
                                         --%>
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
         <table border="0" width="95%">   
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td  align="right">
                        <input id="btpPrintSet" type="button" runat="server" class="iMes_button" onclick="showPrintSettingDialog()"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                        <input type="hidden" runat="server" id="pCode" />
                     </td>
                   </tr>
                    
 
                 <tr>
                    <td colspan="2">
                        <asp:CheckBox id="lbneedprint" runat="server" Checked="true" BackColor="Transparent" BorderStyle="None"></asp:CheckBox>
                    </td>               
                    <td align="right">
                        <input id="btnPrint" type="button"  runat="server"  class="iMes_button" onclick="reprint()"
                        onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                    </td>
                </tr>
                
            </table>
        </center>
    </div>

    <script type="text/javascript">
        function pdlineChange() {
            var line = getPdLineCmbValue();
            if (line != "") {
                WebServiceBoardInput.GetQty(line, onGetQSucc, onGetQFail)
            }
            else
            {document.getElementById("<%=txtTotalQty.ClientID%>").innerText = ""; }
        }
        function onGetQSucc(result) {
          
            document.getElementById("<%=txtTotalQty.ClientID%>").innerText = result[1];
        }
        function onGetQFail(error) {
            // endWaitingCoverDiv();
            try {
              
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
            
            } catch (e) {
                alert(e.description);
      
            }
        }
        
        
        
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgCollectNoItem = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectNoItem") %>';
        var msgCollectExceedCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedCount") %>';
        var msgCollectExceedSumCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedSumCount") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';

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

        var printflag = false;
        var reprintflag = false;
        var accountid = '<%=AccountId%>';
        var username = '<%=UserName%>';
        var login = '<%=Login%>';
        var pcode;
        var station;
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
                editor = "<%=userId%>";
                pcode = document.getElementById("<%=pCode.ClientID%>").value;
                getPdLineCmbObj().setAttribute("AutoPostBack", "True");
                station = document.getElementById("<%=station.ClientID%>").value;
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

        function reprint() {
            
            //document.getElementById("<%=station.ClientID%>").value, document.getElementById("<%=pCode.ClientID%>").value
            var url = "BoardInputReprint.aspx?Station=" + station + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
            var paramArray = new Array();
            paramArray[0] = getPdLineCmbValue();
            paramArray[1] = editor;
            paramArray[2] = customer;
            paramArray[3] = station;
            window.showModalDialog(url, paramArray, 'dialogWidth:800px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');


        }
        
        function repsuss(result) {
            dcode = result[1];
            //save();
        }

        function repfail(result) {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
        }
        
        var lstPrintItem;
        function processDataEntry(inputData) {
            try {
                //document.getElementById("<%=prodHidden.ClientID%>").value = SubStringSN(inputData, "ProdId");
                
            //    inputData = SubStringSN(inputData, "ProdId");
                var errorFlag = false;
                if (getPdLineCmbValue() == "") {
                    alert(mesNoSelPdLine);
                    errorFlag = true;
                    setPdLineCmbFocus();
                    getAvailableData("processDataEntry");
                }
                if (!errorFlag) {
                    if (document.getElementById("<%=txtProdId.ClientID%>").innerText == "") {
                        ShowInfo("");
                    
                        //For BN  Custs SN lenth =16 so not need  SubStringSN(...) Modify by Benson
                         document.getElementById("<%=prodHidden.ClientID%>").value = inputData;
                        beginWaitingCoverDiv();
                        document.getElementById("<%=btnGridFresh.ClientID%>").click(); //Refesh DataGrid..
                        getCommonInputObject().focus();
                        //getAvailableData("processDataEntry");

                    } else {
                    
                        lstPrintItem = getPrintItemCollection();
                        //if (lstPrintItem == null) {
                       //     alert(msgPrintSettingPara);
                       //     getAvailableData("processDataEntry");
                       // }
                       // else
                         {
                            var proid = document.getElementById("<%=txtProdId.ClientID%>").innerText;
                            proid = SubStringSN(proid, "ProdId");
                            hidMBSn = SubStringSN(inputData, "MB");
                            //    beginWaitingCoverDiv();
                            //modify itc-1360-1048 bug
                            WebServiceBoardInput.inputMBSn(document.getElementById("<%=txtProdId.ClientID%>").innerText, hidMBSn, onSucceed, onFail);

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
            //WebServiceBoardInput.SetDataCodeValue(document.getElementById("<%=txtModel.ClientID %>").innerText, customer, setSucc, setFail);
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
                                eval("setScrollTopForGvExt_" + gvClientID + "('" + gvTable.rows[k].cells[5].innerText + "',5,'','MUTISELECT')");
                                
                                if (gvTable.rows[k].cells[3].innerText.indexOf('MB') != -1)
                                {
                                    mbSNo = hidMBSn;
                                }
                                alreadyMatch = true;
                                pqty++;
                                pqtySum++;
                                gvTable.rows[k].cells[7].innerText = pqty;
                                collectionData = gvTable.rows[k].cells[8].innerText;
                                // }
                                if (collectionData != " ") {
                                    collectionData += "," + result[2];
                                }
                                else {
                                    collectionData = result[2];
                                }
                                gvTable.rows[k].cells[8].innerText = collectionData;
                                gvTable.rows[k].cells[8].title = collectionData;

                            }
                        }
                    }
                    //ShowInfo("qtySum" + qtySum + "pqtySum" + pqtySum);
                    if (needCheck == true && qtySum <= pqtySum) {
                       //if (needCheck == true && 1 == pqtySum) {
                        //ResetPage();
                        printflag = false;
                        if (mbSNo != "") {
                            if (document.getElementById("<%=lbneedprint.ClientID%>").checked) {
                                printflag = true;

                            }
                            else {
                                printflag = false;

                            }

                            WebServiceBoardInput.SetDataCodeValue(document.getElementById("<%=txtModel.ClientID %>").innerText, customer, setSucc, setFail);
                        }
                        else {
                            //modify ITC-1360-1529 BUG
                            WebServiceBoardInput.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, lstPrintItem, printflag, onSaveSucceed, onSaveFail);
                        }
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
           
            //WebServiceBoardInput.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, lstPrintItem, onSaveSucceed, onSaveFail);
        /*           
            try {
                if (result == null) {
                    setStatus(true);
                    var content = msgSystemError;
                    ShowMessage("Match Succeed0!");
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();

                }
                else if ((result.length == 2) && (result[0] == SUCCESSRET)) {

                    setStatus(true);
                    setScrollCycle(result[1]);
                    callNextInput();
                    ShowMessage("Match Succeed1!"); 
                    ShowInfo("");

                }
                else {
                    setStatus(true);
                    var content = result[0];
                    ShowMessage(content);
                    ShowMessage("Match Succeed2!");
                    ShowInfo(content);
                    clearData();
                    callNextInput();
                }
            } catch (e) {
                alert(e.description);
            }*/
        }
        function setSucc(result) {
        /*
            setInputOrSpanValue(document.getElementById("<%=txtModel.ClientID %>"), result[0]);
            //getDecodeTypeCmbObj().selectedValue = result[1];
            var drop = getDecodeTypeCmbObj();
            for (var i = 0; i < drop.options.length; i++) {
                if (drop.options[i].text == result[1])
                { drop.options[i].selected = true; }
            }
            inputFlag = false;
            getAvailableData("input");
            getPdLineCmbObj().disabled = true;
            inputObj.focus();
            dcode = getDecodeTypeValue();
            */
            dcode = result[1];
            save();
        }

        function setFail(result) {
            //show error message
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            //inputObj.focus();
        }

        function save() {

            beginWaitingCoverDiv();

            var lstPrintItem = getPrintItemCollection();
            //打印方法
            if (lstPrintItem != null) {
                
                //WebServiceBoardInput.saveForPCAShippingLabel(document.getElementById("<%=txtProdId.ClientID%>").innerText, document.getElementById("<%=prodHidden.ClientID%>").value, document.getElementById("<%=txtModel.ClientID%>").innerText, dcode, lstPrintItem, saveSucc, saveFail);
                WebServiceBoardInput.saveForPCAShippingLabel(document.getElementById("<%=txtProdId.ClientID%>").innerText, mbSNo, document.getElementById("<%=txtModel.ClientID%>").innerText, dcode, lstPrintItem, saveSucc, saveFail);

             }
             else {
                 alert(msgPrintSettingPara);
                 callNextInput();
                 endWaitingCoverDiv();

            }
           
            
        }

        function saveSucc(result) {
            endWaitingCoverDiv();
            //show success message
           // var keyCollection = new Array();
           // var valueCollection = new Array();

           // var retDCode = result[0];
           // var printLst = result[1];
            //tmpDCode = result[0];
          /*  
            keyCollection[0] = "@MBSNO";
            valueCollection[0] = generateArray(mbSNo);

            keyCollection[1] = "@DCode";
            valueCollection[1] = generateArray(retDCode);

            setPrintParam(printLst, "MB CT Label", keyCollection, valueCollection);

           // printLabels(printLst, false);
            lstPrintItem = getPrintItemCollection();*/
            WebServiceBoardInput.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, lstPrintItem, printflag, onSaveSucceed, onSaveFail);
            //modify ITC-1360-1718 BUG
            //DEBUG Mantis0001389
            //ShowSuccessfulInfo(true, "[" + document.getElementById("<%=txtProdId.ClientID%>").innerText + "] " + msgSuccess);

            //initPage
            /*
            initPage();
            getAvailableData("input");
            inputObj.focus();
            */
        }

        function saveFail(result) {
            //show error message
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            initPage();
            //inputObj.focus();
        }
        
        
             
        function setScrollCycle(matchDataArray) 
        {
            eval("setRowNonSelected_" + gvClientID + "()");
              var qty = document.getElementById("<%=sumCountHidden.ClientID%>").value;
             sumQty = scanQty;
            sumQty += matchDataArray.length;
            if (sumQty > qty) {

                alert(msgCollectExceedSumCount);
                return;
            }

            for (var i = 0; i < matchDataArray.length; i++) {

                setScroll(matchDataArray[i].ValueType, matchDataArray[i].PNOrItemName, matchDataArray[i].CollectionData)
            }
        }
        function setScroll(valueType, partno, inputData)
        {
            try {
                var oldQty;
                var oldScanedQty;
                var oldCldata;
                var oldhfCldata;
                var oldhfPartno;
                var newCldata;
                var newhfCldata;
                var newhfPartno;
                var newScanedQty;
                var rowQty;
                var row;
                var subArray;
                var subFindFlag = false;
                var qty = document.getElementById("<%=sumCountHidden.ClientID%>").value;
                var table = document.getElementById(gvClientID);
                for (var i = 1; i < table.rows.length; i++)
                 {
                     if (table.rows[i].cells[6].innerText.trim() == valueType) {
                        if (table.rows[i].cells[3].innerText == partno) {
                            row = eval("setScrollTopForGvExt_" + gvClientID + "('" + partno + "',3,'','MUTISELECT')");
                            break;
                        }
                        subArray = table.rows[i].cells[1].innerText.split(";");
                        for (var j = 0; j < subArray.length; j++) {
                            if (partno == subArray[j]) {
                                subFindFlag = true;
                            }
                        }

                        if (subFindFlag)
                         {
                            row = eval("setScrollTopForGvExt_" + gvClientID + "('" + table.rows[i].cells[3].innerText + "',3,'','MUTISELECT')");
                            break;
                        }
                    }
                }
                if (row != null) 
                {
                    rowQty = row.cells[7].innerText.trim();
                    oldScanedQty = row.cells[8].innerText.trim();
                    oldCldata = row.cells[9].innerText.trim();
                    oldhfCldata = row.cells[10].innerText.trim();
                    oldhfPartno = row.cells[11].innerText.trim();
                    newScanedQty = parseInt(oldScanedQty, 10) + 1;
                    if (newScanedQty > parseInt(rowQty, 10)) {
                        alert(msgCollectExceedCount);
                        eval("setRowNonSelected_" + gvClientID + "()");
                        return;
                    }
                    if (oldhfPartno.length > 0) {
                        newhfPartno = oldhfPartno + "," + partno;
                    }
                    else {
                        newhfPartno = oldhfPartno + partno;
                    }

                    if (oldhfCldata.length > 0) {
                        newhfCldata = oldhfCldata + "," + inputData;
                    }
                    else {
                        newhfCldata = oldhfCldata + inputData;
                    }
                    var newCldata = newhfCldata;
                    if (newhfCldata.length > 20) {
                        newCldata = newhfCldata.substring(0, 20) + "...";
                    }
                    row.cells[8].innerText = newScanedQty;
                    row.cells[9].innerText = newCldata;
                    row.cells[10].innerText = newhfCldata;
                    row.cells[11].innerText = newhfPartno;
                    scanQty++;
                    if (scanQty == parseInt(qty, 10)) {
                        beginWaitingCoverDiv();


                        WebServiceBoardInput.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, lstPrintItem, onSaveSucceed, onSaveFail);
                    }

                }
                else {
                    alert(msgCollectNoItem);
                    eval("setRowNonSelected_" + gvClientID + "()");
                }
            } catch (e) {
                alert(e.description);
            }
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

                getPdLineCmbObj().disabled = status;


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
                var PN = row.cells[5].innerText.trim();
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


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //| Name		:	showSubstitute
        //| Author		:	Lucy Liu
        //| Create Date	:	10/27/2009
        //| Description	:	ÏÔÊ¾Ìæ´úÁÏ
        //| Input para.	:	
        //| Ret value	:	
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        function showSubstitute(partno, substitutePartNo, substituteDescr) {

            try {
                var dataPartNolist = substitutePartNo.trim().split(";");
                var dataDescrlist = substituteDescr.trim().split("|");
                var popParam = new substituteInfo(partno, dataPartNolist, dataDescrlist);

                ShowSubstitute(popParam);

            } catch (e) {
                alert(e.description);

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
                else if (result[0] == SUCCESSRET && result.length == 3) {
                            //var lstPrintItem = getPrintItemCollection();
                //打印方法
                if (lstPrintItem != null && result[1][0].TemplateName != "" && result[2] != null) {
                    reprintflag = true;
                    var keyCollection = new Array();
                    var valueCollection = new Array();

                    var retDCode = result[2];
                    var printLst = result[1];

                    keyCollection[0] = "@MBSNO";
                    valueCollection[0] = generateArray(mbSNo);

                    keyCollection[1] = "@DCode";
                    valueCollection[1] = generateArray(retDCode);

                    for (var ii = 0; ii < printLst.length; ii++) {
                        printLst[ii].ParameterKeys = keyCollection;
                        printLst[ii].ParameterValues = valueCollection;
                    }
                    //setPrintParam(printLst, "MB CT Label", keyCollection, valueCollection);

                    printLabels(printLst, false);
                }


                    //modify itc-1360-1688 bug
                    var tmpid = document.getElementById("<%=txtProdId.ClientID%>").innerText;
                    reset();

                    scanQty = 0;
                    sumQty = 0;
                    setStatus(false);
                    callNextInput();
                    //setPrintItemListParam(result[1], result[2]); // 1 : PrintItem   2 : Custsn
                    //printLabels(result[1], false);
                    //passQty++;
                    //  passQty = result[3];
                    //var passQty = document.getElementById("<%=txtTotalQty.ClientID%>").innerText;
                    //passQty++;
                    //document.getElementById("<%=txtTotalQty.ClientID%>").innerText = passQty ;
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

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Print using~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

         function setPrintItemListParam(backPrintItemList, custsn)
         {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            keyCollection[0] = "@sn";
            valueCollection[0] = generateArray(custsn);
            setPrintParam(lstPrtItem, "Customer Label", keyCollection, valueCollection);
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
                mbSNo = "";
                reprintflag = false;
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

                WebServiceBoardInput.Cancel(document.getElementById("<%=txtProdId.ClientID%>").innerText, document.getElementById("<%=station.ClientID%>").value, onClearSucceeded, onClearFailed);
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
            getPdLineCmbObj().selectedIndex = 0;
            setPdLineCmbFocus();

        }

        function setScanHiddenQty() {

            scanQty = parseInt(document.getElementById("<%=scanQtyHidden.ClientID%>").value, 10);
        }
        function showPrintSettingDialog() {
            showPrintSetting(document.getElementById("<%=station.ClientID%>").value, document.getElementById("<%=pCode.ClientID%>").value);
        }
    </script>

</asp:Content>

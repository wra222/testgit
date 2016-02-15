<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Board Input For Docking
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2012-5-18      itc200052       Create 
 Known issues:
 --%>
 
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="BoardInputForDocking.aspx.cs" Inherits="Docking_BoardInputForDocking" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="~/Docking/Service/WebServiceBoardInputForDocking.asmx" />
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
                            CanUseKeyboard="true" IsPaste="true" MaxLength="100" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
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
                        <input id="btnQuery" type="button" runat="server" class="iMes_button" onclick="ShowQueryData()"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                        <input id="btpPrintSet" type="button" runat="server" class="iMes_button" onclick="showPrintSettingDialog()"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                        
                        <input type="hidden" runat="server" id="pCode" />
                     </td>
                   </tr>
                    
                 <tr>
                    
                    <td colspan="2" >
                        <asp:CheckBox id="lbneedprint" runat="server" Checked="false" BackColor="Transparent" BorderStyle="None"></asp:CheckBox>
                    </td>                                 
                </tr>
                
            </table>
        </center>
    </div>

    <script language="javascript" for="objMSComm" event="OnComm">    
            //ProcessMSComm()
    </script>
	
	<script type="text/javascript">
        function pdlineChange() {
            var line = getPdLineCmbValue();
            if (line != "") {
                WebServiceBoardInputForDocking.GetQty(line, onGetQSucc, onGetQFail)
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
        
		var enableComm = '<%=Request["SendPass"] %>'=='1' ? true : false;
        var objMSComm = document.getElementById("objMSComm");
		function CommSendPass() {
            if(enableComm)
				objMSComm.Output = "Pass";
        }
		function CommSendFail() {
            if(enableComm)
				objMSComm.Output = "Fail";
        }
        
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
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
        var msgQuerynull = '<%=this.GetLocalResourceObject(Pre + "_mesQuerynull").ToString() %>';

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
        var passQty = 0;    
        var printflag = false;           
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
                if(enableComm){
					hostname = getClientHostName();
					PageMethods.getCommSetting(hostname, "<%=userId%>", onGetCommSettingSuccess, onGetCommSettingFail);
				}
				
				//        setPdLineCmbFocus()
                customer = "<%=customer%>";
                getPdLineCmbObj().setAttribute("AutoPostBack", "True");

                getAvailableData("processDataEntry");
            } catch (e) {
                alert(e.description);
            }

        }
		
		function onGetCommSettingSuccess(result) {
            if (result[0] == SUCCESSRET) {
                m_port = result[1];
                m_baud = result[2];
                m_rth = result[3];
                m_sth = result[4];
                m_hs = result[5];
                //alert(m_port);
                //alert(m_baud);
                //alert(m_rth);
                //alert(m_sth);
                //alert(m_hs);
                if (objMSComm.CommPort != m_port) {
                    if (!!objMSComm.PortOpen) {
                        objMSComm.PortOpen = false;
                    }
                    objMSComm.CommPort = m_port;
                }

                objMSComm.Settings = m_baud;
                objMSComm.RThreshold = m_rth;
                objMSComm.SThreshold = m_sth;
                objMSComm.Handshaking = m_hs;

                try {
                    if (!objMSComm.PortOpen)
                        objMSComm.PortOpen = true;
                } catch (e) {
                    alert(e.description);
                }
                //ShowInfo("CommPort,Settings,RThreshold,SThreshold,Handshaking,PortOpen  is: "+objMSComm.CommPort+" , "+objMSComm.Settings +" , "+objMSComm.RThreshold+" , "+objMSComm.SThreshold+" , "+objMSComm.Handshaking+" , "+objMSComm.PortOpen);     

            }
            else {
                ShowMessage(result.get_message());
                ShowInfo(result.get_message());
            }
        }

        function onGetCommSettingFail(result) {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }

        function ProdIDClear() {
            document.getElementById("<%=txtProdId.ClientID %>").innerText = "";
            strProdID = "";
        }
        function ModelClear() {
            document.getElementById("<%=txtModel.ClientID %>").innerText = "";
            strModel = "";
        }

        function TransferData(data, Type) {
            data = data.trim().toUpperCase();
            var datalength = parseInt(data.length);
            switch (Type) {
                case ("ProductID"):
                    {
                        if (datalength == 9) {
                            return data;
                        }
                        else if (datalength == 10) {
                            if (data.substring(4, 5) != "M") {
                                return data.substring(0, 9);
                            }

                        }
                        else {
                            data = "Wrong Code";
                            return data;
                        }
                        break;
                    }
                case ("MBorMAC"):
                    {
                        if (datalength == 10) {
                            mbormac = "MB";
                            return data;
                        }
                        else if (datalength == 11) {
                            if (data.substring(5, 6) == "M") {
                                mbormac = "MB";
                                return data;
                            }
                            else if (data.substring(4, 5) == "M") {
                                mbormac = "MB";
                                return data.substring(0, 10);
                            }
                            else {
                                data = "Wrong Code";
                                return data;
                            }
                        }
                        else if (datalength == 12) {
                            mbormac = "MAC";
                            return data;
                        }
                        else {
                            data = "Wrong Code";
                            return data;
                        }

                        break;
                    }

                default: { return data; }
            }
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

                        //modify ITC-1414-0041 bug
                        var tmpdata = TransferData(inputData, "ProdIdorCustSN");
                        if (tmpdata == "Wrong Code") {
                            CommSendFail();
							alert(msgWrongCode);
                            getAvailableData("processDataEntry");

                        }
                        else {

                            document.getElementById("<%=prodHidden.ClientID%>").value = tmpdata;
                            beginWaitingCoverDiv();
                            document.getElementById("<%=btnGridFresh.ClientID%>").click(); //Refesh DataGrid..
                            getCommonInputObject().focus();
                            //getAvailableData("processDataEntry");
                        }

                    } 
                    else
                    {
                        if (inputData == "7777")
                        {
                            ExitPage();
                            reset();
                            getAvailableData("processDataEntry");

                        }
                        else 
                        {
                            lstPrintItem = getPrintItemCollection();
                            WebServiceBoardInputForDocking.inputMBSn(document.getElementById("<%=txtProdId.ClientID%>").innerText, inputData, onSucceed, onFail);
                            /*
                            hidMBSn = TransferData(inputData, "MBorMAC");
                            if (hidMBSn == "Wrong Code") {
    CommSendFail();
							    alert(msgWrongCode);
                                getAvailableData("processDataEntry");

                            }
                            else {
                                WebServiceBoardInputForDocking.inputMBSn(document.getElementById("<%=txtProdId.ClientID%>").innerText, hidMBSn, onSucceed, onFail);
                            }
                            */
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
            //WebServiceBoardInputForDocking.SetDataCodeValue(document.getElementById("<%=txtModel.ClientID %>").innerText, customer, setSucc, setFail);
            ////////////////////////////////////////////////////////modify by itc200052, 2012.2.21
            try {
                // endWaitingCoverDiv();
                eval("setRowNonSelected_" + gvClientID + "()");

                var iLength = result.length;
                if (result == null) {
                    endWaitingCoverDiv();
                    var content = msgSystemError;
                    CommSendFail();
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

                            if (repVC != "NIC Address") {
                                if (gvTable.rows[k].cells[1].innerText.indexOf(repVC) != -1) {
                                    //if (gvTable.rows[k].cells[1].innerText.indexOf(repVC.substring(0, 2)) != -1) {
                                    eval("setScrollTopForGvExt_" + gvClientID + "('" + gvTable.rows[k].cells[5].innerText + "',5,'','MUTISELECT')");

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
                            else if (repVC == "NIC Address") {
                            if (gvTable.rows[k].cells[5].innerText.indexOf(repVC) != -1) {
                                //if (gvTable.rows[k].cells[1].innerText.indexOf(repVC.substring(0, 2)) != -1) {
                                eval("setScrollTopForGvExt_" + gvClientID + "('" + gvTable.rows[k].cells[5].innerText + "',5,'','MUTISELECT')");

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
                    }
                    //ShowInfo("qtySum" + qtySum + "pqtySum" + pqtySum);
                    if (needCheck == true && qtySum <= pqtySum) {
                       //if (needCheck == true && 1 == pqtySum) {
                        //ResetPage();
                        
                        printflag = false;
                        
                            if (document.getElementById("<%=lbneedprint.ClientID%>").checked) {
                                printflag = true;

                            }
                            else {
                                printflag = false;

                            }
                        
                        WebServiceBoardInputForDocking.save(document.getElementById("<%=txtProdId.ClientID%>").innerText,lstPrintItem, printflag, onSaveSucceed, onSaveFail);

                    }
					else{
//CommSendPass();
					}
                    endWaitingCoverDiv();
                    callNextInput();
                }
                else {
                    endWaitingCoverDiv();
                    var content = result[0];
                    CommSendFail();
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }
            } catch (e) {
                alert(e.description);
            }           
            //////////////////////////////////////////////////////////////////////
           
            //WebServiceBoardInputForDocking.save(document.getElementById("<%=txtProdId.ClientID%>").innerText, lstPrintItem, onSaveSucceed, onSaveFail);
 
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


        function onFail(error) {
            // endWaitingCoverDiv();
            //ShowMessage("Match onFail!");                    

            try {
                //       endWaitingCoverDiv();
                setStatus(true);
                //ShowMessage("Match Error!");
CommSendFail();                
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
                    CommSendFail();
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }
                else if (result[0] == SUCCESSRET && result.length == 3) {
                     //打印方法
                if (printflag && lstPrintItem != null && result[1][0].TemplateName != ""&& result[2]!= null ) {

                            for (var i = 0; i < result[1].length; i++) {	
                                var keyCollection = new Array();
                                var valueCollection = new Array();

                                    var productID = result[2];
                                    var printLst = result[1];
                                    setPrintItemListParam(printLst, result[1][i].LabelType,productID);
                                    printLabels(printLst, false);
                                }
                            }
                    
                    
                    passQty++;
                    document.getElementById("<%=txtTotalQty.ClientID%>").innerText = passQty + "";

                       
                    //modify itc-1360-1688 bug
                    var tmpid = document.getElementById("<%=txtProdId.ClientID%>").innerText;
                    CommSendPass();
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
                    CommSendFail();
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }


            } catch (e) {
				alert(e.description);
                endWaitingCoverDiv();
            }

        }
        
        function setPrintItemListParam(backPrintItemList,LabelType, productId)
         {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            keyCollection[0] = "@ProductId";
            valueCollection[0] = generateArray(productId);
            setPrintParam(lstPrtItem,LabelType, keyCollection, valueCollection);
        }

        function onSaveFail(error) {
            //ShowMessage("onSaveFail!");                    

            try {
                endWaitingCoverDiv();
CommSendFail();
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

                WebServiceBoardInputForDocking.Cancel(document.getElementById("<%=txtProdId.ClientID%>").innerText, document.getElementById("<%=station.ClientID%>").value, onClearSucceeded, onClearFailed);
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


        function ShowQueryData() {
            
            var line = getPdLineCmbValue();

            if (line != "") {
                WebServiceBoardInputForDocking.ShowQueryData(line, onQuerySucc, onQueryFail);
            }
            else 
            {
                     alert(mesNoSelPdLine);
           
            }
        }

        function onQuerySucc(result) {
            if (result[1] == "") {
                ShowMessage(msgQuerynull);
            }
            else
                ShowMessage(result[1]);
        }
        function onQueryFail(result) {
            // endWaitingCoverDiv();
            try {

                ShowMessage(result.get_message());
                ShowInfo(result.get_message());

            } catch (e) {
                alert(e.description);

            }
        }

        function showPrintSettingDialog() {
            showPrintSetting(document.getElementById("<%=station.ClientID%>").value, document.getElementById("<%=pCode.ClientID%>").value);
        }

    </script>

</asp:Content>

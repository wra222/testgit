<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Final Scan page
 * UI:CI-MES12-SPEC-PAK-UI Final Scan.docx --2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC Final Scan.docx --2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-10  Zhang.Kai-sheng       (Reference Ebook SourceCode) Create
 * TODO:

 * Known issues:
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FinalScan.aspx.cs" Inherits="PAK_FinalScan" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" ></bgsound>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceFinalScan.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 80px;"/>
                    <col />
                    <col style="width: 80px;"/>
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblPickID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPickIDContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblForwarder" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblForwarderContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDriver" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDriverContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTruckID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTruckIDContent" runat="server" CssClass="iMes_label_11pt" ></asp:Label>
                    </td>                                                            
                </tr> 
                <tr>
                    <td></td>
                    <td colspan ="2" align="right">
                        <asp:Label ID="lblChepPalletQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblChepPalletQtyContent" runat="server" CssClass="iMes_label_11pt" ></asp:Label>
                    </td>                                                            
                </tr>
            </table>
            <hr />
            <div>       
                <fieldset>
                    <legend><%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_lblRemainPalletList").ToString()%></legend>
                    <table width="98%" border="0" style="table-layout: fixed;margin: 0 auto;">
                        <tr>
                            <td style="width: 110px;">
                                <asp:Label ID="lblRemainQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblRemainQtyContent" runat="server" CssClass="iMes_label_11pt" Text="0"></asp:Label>
                               <%-- <button id="hidBtn" runat="server" onserverclick="hidBtn_ServerClick" style="width: 0;"></button> --%>
                            </td>
                        
                            <td style="width: 110px;">
                                <asp:Label ID="lblChepPallet" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblChepPalletContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                            </td>
                        
                            
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="upPanl" runat="server">
                                <%-- 
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="hidBtn" EventName="ServerClick" />
                                    </Triggers>
                                --%>   
                                    <ContentTemplate>
                                        <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                                            GvExtWidth="100%" GvExtHeight="228px" style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False" HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                                        </iMES:GridViewExt> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>                
                </fieldset> 
            </div>              
        </div>
        <div>
            <table width="100%">
                <tr>
                    <td nowrap="noWrap" style="width: 15%;">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td style="width: 55%;">
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="90%" IsClear="true" IsPaste="true" />
                    </td>
                    <td style="width: 30%;">
                        <label id="lblWarnInputPickID" runat="server" class="iMes_label_13pt" style="color: Red;"><%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_lblWarnInputPickID").ToString()%></label>
                    </td>
                </tr>
            </table>            
        </div>      
            
        <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>  
    </div>    
    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var editor;
        var tbl;
        var DEFAULT_ROW_NUM = 9; //13; //9?
        var customer;
        var stationId;
        var inputObj;     
        var emptyPattern = /^\s*$/;
        var emptyString = "";
        var gKeyPickID = "";
        var gKeyPalletNo = "";
        var gKeychepPallet = "";
        var bNeedCheckChep = false;
        var msgExistPalletNo = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgExistPalletNo").ToString() %>';
        var msgWrongCode = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgWrongCode").ToString() %>';
        var msgNotExistChep = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgNotExistChep").ToString() %>';
        var msgInputChepplt = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputChepplt").ToString() %>';
        var msgOutpalletNo = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgOutpalletNo").ToString() %>';
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSuccess") %>';

        var inPalletlst;
        var outpPallerlst;
        var iChepPalletQty;
        ///---------------------------------------------------
        ///| Name		    :	window.onload
        ///| Description	:	
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
        window.onload = function()
        {
            //Get env value Setup Input Object Event;
            tbl = "<%=gd.ClientID %>";
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';

            inputObj = getCommonInputObject();
            getAvailableData("Input_txtEntry");
                        
        };
        ///---------------------------------------------------------------------------------------------------
        ///| Name		    :	Input_txtEntry
        ///| Description	:	handle Input PickID or palletNo
        ///| Input para.	:	Pick ID / Pallet No / Chep Pallet No 都是10位,3.	UCC ID 是20 位长
        ///|                    10位长以字符'3' 开头的数据，可以视为Chep Pallet No
        ///| Ret value      :
        ///---------------------------------------------------------------------------------------------------
        function Input_txtEntry(data)
        {
            //Default inputFlag = false ->Input Pick ID
            //After PickID succ, inputFlag = true->Input Pallet No
            //set inputFlag = false in func resetUI
            //7777--reset UI

            PlaySoundClose();

            
            if(data == "7777")
            {
                //DEBUG ITC-1360-0555, first ResetPage
                ResetPage();
                ShowInfo("");
                resetUI();
                //ResetPage();
                getAvailableData("Input_txtEntry");
                inputObj.focus();
                return true;                
            }
            //Pick ID / Pallet No / Chep Pallet No--10； UCC ID -20 ；
            //other--Wrong Code!
            if (!((data.length == 10)||(data.length == 20)))
            {
                //ShowMessage(msgWrongCode);
                PlaySoundFail();
                ShowInfo(msgWrongCode);
                getAvailableData("Input_txtEntry");
                inputObj.focus();
                return true;
            }  
            
            if (inputFlag)    //Input [Pallet No]
            {
                //UCC ID --20
                if (data.length == 20) 
                {
                    //call back Function
                    ShowInfo("");
                    beginWaitingCoverDiv();
                    FinalScanService.InputUCCID(emptyString, gKeyPickID, data, editor, stationId, customer, inputUCCIDSucc, inputUCCIDFail);
                }
                else 
                {
                    ShowInfo("");
                    if (bNeedCheckChep) 
                    {
                        gKeychepPallet = data.trim();
                        if (gKeychepPallet.substring(0, 1) != "3") {
                            //ShowMessage(msgInputChepplt);
                            PlaySoundFail();
                            ShowInfo(msgInputChepplt);
                            gKeychepPallet = "";
                            getAvailableData("Input_txtEntry");
                            inputObj.focus();
                            return true;           
                            
                        }
                        beginWaitingCoverDiv();
                        FinalScanService.InputChepPalletNo(emptyString, gKeyPickID, gKeyPalletNo, data, editor, stationId, customer, inputChepPltSucc, inputChepPltFail)
                    }
                    else 
                    {
                          
                        inputPalletNo(data);
                    }
                }
            }
            else              //Input [Pick ID]
            {
                if (data.length != 10) 
                {
                    //ShowMessage(msgWrongCode);
                    PlaySoundFail();
                    ShowInfo(msgWrongCode);
                    getAvailableData("Input_txtEntry");
                    inputObj.focus();
                    return true;           
                }
                
                ShowInfo("");
                beginWaitingCoverDiv();
                gKeyPickID = data;
                //call back Function
                FinalScanService.inputPickID(emptyString, data, editor, stationId, customer, inputPickIDSucc, inputPickIDFail);
            }
        }

        ///---------------------------------------------------
        ///| Name		    :	inputUCCIDSucc
        ///| Description	:	Callback for service:inpuUCCID--Success Case  
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
        function inputUCCIDSucc(result) 
        {
            endWaitingCoverDiv();
            if (result[0] == null || result[0].length == 0)   //return NULL
            {
                //ShowMessage(msgExistPalletNo);
                PlaySoundFail();
                ShowInfo(msgExistPalletNo);
                getAvailableData("Input_txtEntry");
                inputObj.focus();
            }
            else 
            {
                //alert("pallet");
                ShowInfo("");
                inputPalletNo(result[0].toUpperCase());
            }
        }
        ///---------------------------------------------------
        ///| Name		    :	inputUCCIDFail
        ///| Description	:	Callback for service:inpuUCCID--Fail Case  
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
        function inputUCCIDFail(result) 
        {
            endWaitingCoverDiv();
            //ShowMessage(msgExistPalletNo);
            PlaySoundFail();
            ShowInfo(msgExistPalletNo);
            getAvailableData("Input_txtEntry");
            inputObj.focus();
        }

        //inputChepPltSucc, inputChepPltFail
        //--------------------------------------
        function inputChepPltSucc(result) 
        {
                 endWaitingCoverDiv();
                var remainQty = 0;
                //remove a record from table
                if (result[0] == null || result[0].length == 0) 
                {
                    remainQty = 0;
                    //show success info and reset UI
                    ShowSuccessfulInfo(true, "[" + document.getElementById("<%=lblPickIDContent.ClientID %>").innerText + "] " + msgSuccess);
                    resetUI();
                    //getAvailableData("Input_txtEntry");
                }
                else 
                {
                    remainQty = result[0].length;
                    bNeedCheckChep = false;
                    ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
                    addOrChange(result[0]);
                    setInputOrSpanValue(document.getElementById("<%=lblRemainQtyContent.ClientID %>"), remainQty);
                    setChepInfo(gKeychepPallet);
                    //Add  2012/06/14
                    iChepPalletQty = iChepPalletQty +1;
                    var strChepPalletQty = iChepPalletQty.toString();
                    setInputOrSpanValue(document.getElementById("<%=lblChepPalletQtyContent.ClientID %>"), strChepPalletQty);                     
                
                    
                    PlaySoundSucc();
                    inputObj.focus();
                }
            
                getAvailableData("Input_txtEntry");
        }
        ///---------------------------------------------------
        ///| Name		    :	inputChepPltFail
        ///| Description	:	Callback for service:inputPalletNo--Fail Case  
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
        function inputChepPltFail(result) 
        {
            endWaitingCoverDiv();
            //ShowMessage(result.get_message());
            PlaySoundFail();
            ShowInfo(result.get_message());
            //resetUI();
            inputObj.focus();
            getAvailableData("Input_txtEntry");
        }
    
        ///---------------------------------------------------
        ///| Name		    :	inputPickIDSucc
        ///| Description	:	Callback for service:inputPickID--Success Case  
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
        function inputPickIDSucc(result)
        {
            var length = 0;
            inputFlag = true;
            length = result[1] == null ? 0 : result[1].length;
            
            if (length == 0)   //return NULL
            {
                endWaitingCoverDiv();
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, "[" + document.getElementById("<%=lblPickIDContent.ClientID %>").innerText + "] " + msgSuccess);
                resetUI();
                getAvailableData("Input_txtEntry");
                inputObj.focus();
            }
            else               
            {
                //Update Labels Information: Get Forwarder, Driver, Truck ID
                endWaitingCoverDiv();
                //setInfo(result[0].PickID, result[0].Fwd, result[0].Driver, result[0].TruckID, length);
                setInfo(result[0].PickID, result[2], result[3], result[0].TruckID, length);
                setChepInfo(emptyString);
                //Add :UC update--Duplicate
                inPalletlst = result[1];
                outpPallerlst = result[4];

                //Add 2012/06/14 Chep pallet Qty
                iChepPalletQty = result[5];
                var strChepPalletQty = iChepPalletQty.toString();
                setInputOrSpanValue(document.getElementById("<%=lblChepPalletQtyContent.ClientID %>"), strChepPalletQty);                     
                
                //update Table Information
                addOrChange(result[1]);
                PlaySoundSucc();
                getAvailableData("Input_txtEntry");
                inputObj.focus();                
            }            
        }
        var GridViewExt1ClientID = "<%=gd.ClientID %>";

        ///---------------------------------------------------
        ///| Name		    :	addOrChange
        ///| Description	:	update table Info 
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
        function addOrChange(lstRow)
        {
            for (var i = 0; i < lstRow.length; i++)
            {
                var rowArray = new Array();

                rowArray.push(lstRow[i].PickID);
                rowArray.push(lstRow[i].Plt);
                rowArray.push(lstRow[i].Qty);

                if (i < 8) //12//9-8
                {   
                    eval("ChangeCvExtRowByIndex_" +GridViewExt1ClientID+"(rowArray, false, i + 1)");
                }
                else
                {
                    eval("AddCvExtRowToBottom_"+GridViewExt1ClientID+"(rowArray,false)");
                }                
            }
        }
        ///---------------------------------------------------
        ///| Name		    :	inputPickIDFail
        ///| Description	:	Callback for service:inputPickID--Fail Case  
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
         function inputPickIDFail(result)
        {
            //ShowMessage(result.get_message());
            PlaySoundFail();
            ShowInfo(result.get_message());
            getAvailableData("Input_txtEntry");
            endWaitingCoverDiv();
            inputObj.focus();
        }
        ///---------------------------------------------------
        ///| Name		    :	inputPalletNo
        ///| Description	:	invoke FinalScanService.InputPalletNo  
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
        function inputPalletNo(data)
        {
            //palletNo invalid，again
            if (!isInTable(data))
            {
                //add check pallet "in"/"out"?
                if (isInInorOutList(data)) 
                {
                    //PlaySound;
                    PlaySound();
                    //ShowMessage(msgOutpalletNo, false);
                    //PlaySoundClose();
                    ShowInfo(msgOutpalletNo);
                    getAvailableData("Input_txtEntry");
                    inputObj.focus();
                    return;
                }
                else 
                {
                    //alert(msgExistPalletNo);
                    //ShowMessage(msgExistPalletNo);
                    PlaySoundFail();
                    ShowInfo(msgExistPalletNo);
                    getAvailableData("Input_txtEntry");
                    inputObj.focus();
                    return;
                }
            }
            //call back Function
            beginWaitingCoverDiv();
            gKeyPalletNo = data;
            FinalScanService.InputPalletNo(emptyString, gKeyPickID, data, editor, stationId, customer, inputPalletNoSucc, inputPalletNoFail);
        }

        function isInInorOutList(pallet) 
        {
            //inPalletlst = result[1];
            //outpPallerlst = result[4];
            var palletlength = 0;
            var i;
            var bFoundPlt = false;
            palletlength = inPalletlst == null ? 0 : inPalletlst.length;
            for (i = 0; i < palletlength; i++) {
                if (pallet.toUpperCase() == inPalletlst[i].Plt.toUpperCase()) 
                {
                    bFoundPlt = true;
                    break;
                }
            }
            if (bFoundPlt == false) 
            {
                palletlength = outpPallerlst == null ? 0 : outpPallerlst.length;
                if (palletlength > 0) 
                {
                    for (i = 0; i < palletlength; i++) 
                    {
                        if (pallet.toUpperCase() == outpPallerlst[i].Plt.toUpperCase()) 
                        {
                            bFoundPlt = true;
                            break;
                        }
                    }
                }
            }
            return bFoundPlt;
        }
        
        function isInTable(pallet)
        {
            //judge Pallet No in Table?
            var tblObj = document.getElementById("<%=gd.ClientID %>");
            var length = tblObj.rows.length;
            
            for (var i = 1; i < length; i++)
            {
                if (tblObj.rows[i].cells[1].innerText.toUpperCase() == pallet.toUpperCase())
                {
                    return true;
                }
            }
            return false;            
        }
        ///---------------------------------------------------
        ///| Name		    :	inputPalletNoSucc
        ///| Description	:	Callback for service:inputPalletNo--Success Case,update Table, set remainQty  
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
        function inputPalletNoSucc(result)
        {
            endWaitingCoverDiv();
            if (result[1] == true) 
            {
                //PlaySoundSucc();
                PlaySoundChepPallet();
                bNeedCheckChep = true;
                inputObj.focus();
            }
            else 
            {
                var remainQty = 0;
                 //remove a record from table
                if (result[0] == null || result[0].length == 0) 
                {
                    remainQty = 0;
                    //show success info and reset UI
                    //ShowSuccessfulInfo(true);
                    //dyh
					var CompletesUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["CompleteAudioFile"] %>'
                    ShowCompleteInfo(true, "[" + document.getElementById("<%=lblPickIDContent.ClientID %>").innerText + "] " + msgSuccess,CompletesUrl);
                    resetUI();
                    //getAvailableData("Input_txtEntry");
                }
                else 
                {
                    remainQty = result[0].length;
                    ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
                    addOrChange(result[0]);
                    setInputOrSpanValue(document.getElementById("<%=lblRemainQtyContent.ClientID %>"), remainQty);
                    PlaySoundSucc();
                    bNeedCheckChep = false;
                    inputObj.focus();
                }
            }
            getAvailableData("Input_txtEntry");
        }
        ///---------------------------------------------------
        ///| Name		    :	inputPalletNoFail
        ///| Description	:	Callback for service:inputPalletNo--Fail Case  
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
         function inputPalletNoFail(result)
        {
            endWaitingCoverDiv();
            //ShowMessage(result.get_message());
            PlaySoundFail();
            ShowInfo(result.get_message());
            //resetUI();
            inputObj.focus();
            getAvailableData("Input_txtEntry");
        }
        ///---------------------------------------------------
        ///| Name		    :	setInfo
        ///| Description	:	Set PickID,Forwarder,Driver,TruckID and RemainQty Info 
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
        function setInfo(pickID, forwarder, driver, truckID, remainQty)
        {
            setInputOrSpanValue(document.getElementById("<%=lblPickIDContent.ClientID %>"), pickID);     
            setInputOrSpanValue(document.getElementById("<%=lblForwarderContent.ClientID %>"), forwarder); 
            setInputOrSpanValue(document.getElementById("<%=lblDriverContent.ClientID %>"), driver); 
            setInputOrSpanValue(document.getElementById("<%=lblTruckIDContent.ClientID %>"), truckID);
            setInputOrSpanValue(document.getElementById("<%=lblRemainQtyContent.ClientID %>"), remainQty);      
        }
        function setChepInfo(cheppallet) 
        {
            setInputOrSpanValue(document.getElementById("<%=lblChepPalletContent.ClientID %>"), cheppallet);       
        }
        function resetUI()
        {
            //Set Input symbol
            inputFlag = false;
            gKeyPickID = "";
            gKeyPalletNo = "";
            bNeedCheckChep = false;
            //Set labels is Null
            setInfo(emptyString, emptyString, emptyString, emptyString, "0");
            setChepInfo(emptyString);

            //Add  2012/06/14
            iChepPalletQty = 0;
            var strChepPalletQty = iChepPalletQty.toString();
            setInputOrSpanValue(document.getElementById("<%=lblChepPalletQtyContent.ClientID %>"), "");                     
                
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            inputObj.focus();
        }
        
        window.onbeforeunload = function()
        {
            if (inputFlag)
            {
                OnCancel();
            }
        };
        
        function OnCancel()
        {
            //invoke when Exit Page
            //if Input Obj is not NULL
            if (!emptyPattern.test(document.getElementById("<%=lblPickIDContent.ClientID %>").innerText))
            {
                FinalScanService.cancel(document.getElementById("<%=lblPickIDContent.ClientID %>").innerText);
            }
        }

        function ExitPage()
        {
            OnCancel();
        }

        function ResetPage()
        {
            ExitPage();
            ShowInfo("");
        }
        function PlaySound() {
            var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["DuplicateAudioFile"] %>';
            var obj = document.getElementById("bsoundInModal");
            //obj.loop =-1;
            obj.loop = 5;
            obj.src = sUrl;
        }

        function PlaySoundSucc() {
            var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["PassAudioFile"] %>';
            var obj = document.getElementById("bsoundInModal");
            obj.loop = 1;
            obj.src = sUrl;
        }

        function PlaySoundChepPallet() {
            var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["SpecialPassAudioFile"] %>';
            var obj = document.getElementById("bsoundInModal");
            obj.loop = 1;
            obj.src = sUrl;
        }
        function PlaySoundFail() {
            var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["FailAudioFile"] %>';
            var obj = document.getElementById("bsoundInModal");
            //obj.loop = -1;
            obj.loop = 5;
            obj.src = sUrl;
        }
        function PlaySoundClose() {

            var obj = document.getElementById("bsoundInModal");
            
            obj.src = "";
        }


    </script>
</asp:Content>


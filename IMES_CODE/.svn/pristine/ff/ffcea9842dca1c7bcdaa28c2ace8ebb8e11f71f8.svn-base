<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PCAShippingLabelPrint page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-20  zhu lei               Create
 * 2012-02-27  Li.Ming-Jun           ITC-1360-0573
 * 2012-02-28  Li.Ming-Jun           ITC-1360-0536
 * 2012-02-28  Li.Ming-Jun           ITC-1360-0715
 * 2012-02-28  Li.Ming-Jun           ITC-1360-0864
 * 2012-03-01  Li.Ming-Jun           ITC-1360-0948
 * 2012-03-04  Li.Ming-Jun           ITC-1360-0882
 * 2012-03-04  Li.Ming-Jun           ITC-1360-1031
 * 2012-03-15  Li.Ming-Jun           ITC-1360-1467
 * 2012-04-11  Li.Ming-Jun           ITC-1360-1662
 * Known issues:
 * TODO:
 * checkitems 中的WWANcheck未完成，由于与BOM有关还没有最终确定所以暂时缺少
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PCAShippingLabelPrint.aspx.cs" Inherits="FA_PCAShippingLabelPrint" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServicePCAShippingLabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
             <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 15%;"/>
                    <col />
                    <col style="width: 15%;"/>
                    <col />
                </colgroup>
                <tr>
                    <td></td>
                    <td align="center" colspan="2">
                        <asp:RadioButtonList ID="rblPCA" runat="server" RepeatDirection="Horizontal" Enabled="true">
                            <asp:ListItem Selected Value="ForPC" onclick="rblPCA_Changed(this)">For PC</asp:ListItem>
                            <asp:ListItem Value="ForRCTO" onclick="rblPCA_Changed(this)" Enabled="false">For RCTO</asp:ListItem>
                            <asp:ListItem Value="ForFRU" onclick="rblPCA_Changed(this)" Enabled="false">For FRU</asp:ListItem>
                        </asp:RadioButtonList> 
                    </td>
                    <td align="center">
		                <input id="btnPrintSet" type="button"  runat="server"  class="iMes_button" onclick="clkSetting()" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPdline" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="CmbPdLine" runat="server" Width="99%" Stage="FA" />
                    </td>
                </tr> 
                <tr>
                    <td>
                        <asp:Label ID="lblDCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbDCodeType ID="CmbDCode" runat="server" Width="99%" />
                    </td>
                </tr> 
                <tr>
                    <td>
                        <asp:Label ID="lblRegion" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="drpRegion" runat="server" Width="99%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>  
                <tr>
                    <td>
                        <asp:Label ID="lblMBSNO" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblMBSNOContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblProductModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblProductModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>                
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblFamilyContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblVersion" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblVersionContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblECR" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblECRContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>                     
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMAC" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:Label ID="lblMACContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>                     

                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                </tr>
             </table>                                       
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>       
    </div>    
    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var editor;
        var DEFAULT_ROW_NUM = 7;
        var mbSNo = "";
        var dcode = "";
        var modelId = "";
        var checkPCMBRCTOMB = "";
        var customer;
        var stationId;
        var pCode;
        var inputObj;
        var val
        
        //error message
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var mesNoSelDCodeType = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelDCodeType").ToString() %>';
        var msgDCodeTypeNull = '<%=this.GetLocalResourceObject(Pre + "_msgDCodeTypeNull").ToString() %>';
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString() %>';
        var msgMBSnoNull = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoNull").ToString()%>';
        var msgMBSnoLength = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoLength").ToString()%>';

        window.onload = function() {
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            val = "ForPC";
            document.getElementById("<%=drpRegion.ClientID%>").disabled = true;
            getDecodeTypeCmbObj().disabled = true;
            inputObj.focus();
        };

        function rblPCA_Changed(obj) {
            val = obj.value;
            if (val == "ForFRU") {
                document.getElementById("<%=drpRegion.ClientID%>").disabled = false;
                getDecodeTypeCmbObj().disabled = false;
                getDecodeTypeCmbObj().options[0].selected = true;
//                getDecodeTypeCmbObj().disabled = true;
//                var drop = getDecodeTypeCmbObj();
//                for (var i = 0; i < drop.options.length; i++) {
//                    if (drop.options[i].text == 'FRU')
//                    {drop.options[i].selected = true;}
//                }
            } else if (val == "ForPC") {
                document.getElementById("<%=drpRegion.ClientID%>").disabled = true;
                getDecodeTypeCmbObj().disabled = true;
                getDecodeTypeCmbObj().options[0].selected = true;
            } else if (val == "ForRCTO"){
                document.getElementById("<%=drpRegion.ClientID%>").disabled = true;
                getDecodeTypeCmbObj().disabled = false;
                getDecodeTypeCmbObj().options[0].selected = true;
            }
        }

        function checkMBSno() {
            if (mbSNo == "" || mbSNo == null) {
                alert(msgMBSnoNull);
                return false;
            }
            if (!(mbSNo.length == 10 || mbSNo.length == 11)) {
                alert(msgMBSnoLength);
                return false;
            }
            mbSNo = SubStringSN(mbSNo, "MB");
            return true;
        }

        function input(data)
        {
            if (getPdLineCmbValue() == "") {
            
                alert(mesNoSelPdLine);
                setPdLineCmbFocus();
                getAvailableData("input");
                return;
            }

            if (getDecodeTypeValue() == "" && (val == "ForRCTO" || val == "ForFRU")) {
                alert(mesNoSelDCodeType);
                setDecodeTypeCmbFocus();
                getAvailableData("input");
                return;
            }

            if (inputFlag) {
                ShowInfo("");
                modelId = data.trim();
                if (val == "ForPC") {
                    WebServicePCAShippingLabelPrint.SetDataCodeValue(data, customer, setSucc, setFail);
                }
                else {
                    if (val == "ForRCTO") {
                        setInputOrSpanValue(document.getElementById("ctl00_iMESContent_lblProductModelContent"), modelId);
                    }
                    dcode = getDecodeTypeValue();
                    save();
                }
            }
            else
            {
                ShowInfo("");
                mbSNo = data.trim();
                if (checkMBSno()) {
                    if (val == "ForPC") {
                        checkPCMBRCTOMB = "PC";
                    }
                    else if (val == "ForRCTO") {
                        checkPCMBRCTOMB = "RCTO";
                    }
                    else {
                        checkPCMBRCTOMB = "";
                    }
                    WebServicePCAShippingLabelPrint.InputMB(getPdLineCmbValue(), mbSNo, checkPCMBRCTOMB, editor, stationId, customer, inputSucc, inputFail);
                }
                else {
                    getAvailableData("input");
                    inputObj.focus();
                }
            }
        }
        
        function inputSucc(result) {
            setInfo(mbSNo, result);
            if (val == "ForFRU") {
                modelId = result[1];
                dcode = getDecodeTypeValue();
                save();
            }
            else {
                inputFlag = true;
                getAvailableData("input");
                getPdLineCmbObj().disabled = true;
                inputObj.focus();
            }
        }

        function inputFail(result) {
            //show error message
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            inputObj.focus();
        }


        function setSucc(result)
        {
            setInputOrSpanValue(document.getElementById("<%=lblProductModelContent.ClientID %>"), result[0]);
            //getDecodeTypeCmbObj().selectedValue = result[1];
            var drop = getDecodeTypeCmbObj();
            for (var i = 0; i < drop.options.length; i++) {
            if (drop.options[i].text == result[1])
                {drop.options[i].selected = true;}
            }
            inputFlag = false;
            getAvailableData("input");
            getPdLineCmbObj().disabled = true;
            inputObj.focus();

            dcode = getDecodeTypeValue();
            if (dcode == "" && val == "ForPC") {
                alert(msgDCodeTypeNull);
                ResetPage();
                getAvailableData("input");
                inputObj.focus();
                return;
            }

            save();
        }

        function setFail(result)
        {
            //show error message
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            inputObj.focus();
        }

        function setInfo(mbSNo, info)
        {
            //set value to the label
            setInputOrSpanValue(document.getElementById("<%=lblMBSNOContent.ClientID %>"), mbSNo);
            setInputOrSpanValue(document.getElementById("<%=lblFamilyContent.ClientID %>"), info[0]);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), info[1]);
            setInputOrSpanValue(document.getElementById("<%=lblMACContent.ClientID %>"), info[2]);
            setInputOrSpanValue(document.getElementById("<%=lblVersionContent.ClientID %>"), info[3]);
            setInputOrSpanValue(document.getElementById("<%=lblECRContent.ClientID %>"), info[4]);
        }
        
        function save()
        {
            var lstPrintItem = getPrintItemCollection();
            //打印方法
            if (lstPrintItem == null) {
                alert(msgPrintSettingPara);
                ResetPage();
                return;
            }

            var region = document.getElementById("<%=drpRegion.ClientID%>").value;

            beginWaitingCoverDiv();
            WebServicePCAShippingLabelPrint.save(mbSNo, modelId, dcode, region, lstPrintItem, saveSucc, saveFail);
        }

        function saveSucc(result) 
        {
            endWaitingCoverDiv();
            //show success message
            var keyCollection = new Array();
            var valueCollection = new Array();

            var retDCode = result[0];
            var printLst = result[1];

            keyCollection[0] = "@MBSNO";
            valueCollection[0] = generateArray(mbSNo);

            keyCollection[1] = "@DCode";
            valueCollection[1] = generateArray(retDCode);
			
			var ModelTypeForPrint = "PC";
            if (val == "ForFRU") {
                ModelTypeForPrint = "FRU";
            } else if (val == "ForPC") {
                ModelTypeForPrint = "PC";
            } else if (val == "ForRCTO") {
                ModelTypeForPrint = "RCTO";
            }
            keyCollection[2] = "@ModelType";
            valueCollection[2] = generateArray(ModelTypeForPrint);
            keyCollection[3] = "@Model";
            valueCollection[3] = generateArray(modelId);
            for (var jj = 0; jj < backPrintItemList.length; jj++) {
                backPrintItemList[jj].ParameterKeys = keyCollection;
                backPrintItemList[jj].ParameterValues = valueCollection;
            }
            //setPrintParam(printLst, "MB CT Label", keyCollection, valueCollection);

            printLabels(printLst, false);

            ShowSuccessfulInfo(true, "[" + mbSNo + "] " + msgSuccess);
            
            //initPage
            initPage();
            getAvailableData("input");
            inputObj.focus();
        }
        
        function initPage()
        {
            setInputOrSpanValue(document.getElementById("<%=lblMBSNOContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblECRContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblFamilyContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblMACContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblVersionContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblProductModelContent.ClientID %>"), "");
            
            //setSrollByIndex(0 ,false);
            inputFlag = false;
            getPdLineCmbObj().disabled = false;
            if (val == "ForPC") {
                document.getElementById("<%=drpRegion.ClientID%>").disabled = true;
                getDecodeTypeCmbObj().selectedIndex = 0;
                getDecodeTypeCmbObj().disabled = true;
            }
        }
        
        function saveFail(result)
        {
            //show error message
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            initPage();
            inputObj.focus();
        }
        
        function clkSetting() {
            showPrintSetting(stationId, pCode);
        }

        window.onbeforeunload = function()
        {
            if (inputFlag)
            {
                OnCancel();
            }
        };   
        
        function OnCancel() {
            if (mbSNo != "") {
                WebServicePCAShippingLabelPrint.cancel(mbSNo);
            }
        }
        
        function ExitPage(){
            OnCancel();
        }
        
        function ResetPage(){
            ExitPage();
            initPage();
            ShowInfo("");
        }                  
       
    </script>
</asp:Content>


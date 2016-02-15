<%--
 INVENTEC corporation (c)2011 all rights reserved. 
 Description: Change To PIA/EPIA (FA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-27  CHENPENG        Create 

 Known issues:
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="RegenerateCustSNForDocking.aspx.cs" Inherits="Docking_RegenerateCustSN" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/Docking/Service/RegenerateCustSNForDocking.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                  <tr><td></td><td>&nbsp;</td></tr> 
                  <tr><td></td><td>&nbsp;</td></tr> 
                 <tr><td></td><td>&nbsp;</td></tr>                                
                <tr>           
                    <td style="width: 5%" align="left">
                        <asp:Label ID="lbProdId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 10%" align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtProdId" runat="server" CssClass="label_normal" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>                   
                </tr>
                <tr><td></td><td>&nbsp;</td></tr> 
                <tr>
                <td style="width: 5%" align="left">
                        <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtModel" runat="server" CssClass="label_normal" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td></tr>
                    <tr><td></td><td>&nbsp;</td></tr> 
                <tr>
                    <td style="width: 5%" align="left">
                        <asp:Label ID="lblPdline" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="txtPdline" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                     
                    </td>
                    
                    
                </tr>
                
                     <tr><td></td><td>&nbsp;</td></tr> 
                        <tr><td></td><td>&nbsp;</td></tr> 
                               <tr><td></td><td>&nbsp;</td></tr> 
                    <tr><td></td><td>&nbsp;</td></tr> 
                      <tr><td></td><td>&nbsp;</td></tr> 

                <tr>
                   
                </tr>
                <tr>   
                <td width="15%" align="left">
                    <asp:Label id="lblDataEntry" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                </td>
                <td width="55%" align="left" >
                   <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                    CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                    InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
                </td>
                <td align="right" width="10%">
                    <input id="btnPrintSetting" style="height:auto" type="button"  runat="server" 
                            onclick="showPrintSettingDialog()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"/> 
                            &nbsp; 
                </td>
               
            </tr>
            
             <tr>
            <td>
               <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
                    <ContentTemplate>
                      <input id="HD_deliveryDN" type="hidden" runat="server" />
                      <input type="hidden" runat="server" id="station" />
                    </ContentTemplate>   
                </asp:UpdatePanel>
            </td>
        </tr>
            </table>
        </center>
    </div>

    <script type="text/javascript">

        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgCollectNoItem = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectNoItem") %>';
        var msgCollectExceedCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedCount") %>';
        var msgCollectExceedSumCount = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectExceedSumCount") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgEPIASuc = '<%=this.GetLocalResourceObject(Pre + "_msgEPIASuc").ToString()%>';
        var msgPIASuc = '<%=this.GetLocalResourceObject(Pre + "_msgPIASuc").ToString()%>';

        var prodid = "";
        var radChangeToEPIA;
        var passQty = 0;
        var failQty = 0;
        var station = '<%=Request["Station"] %>';
        var pcode = '<%=Request["PCode"] %>';

        window.onload = function() {
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            inputObj.focus();
        };
        
        function showPrintSettingDialog() {
            showPrintSetting(station, pcode);
        }

        function checkInput(data) {
            if (data.length == 9)
                return data;
            if (data.length == 10) {
                //if (data.substring(0, 3) == "CNU")
				if (CheckCustomerSN(data))
                    return data;
                else
                    return data.substring(0, 9);
            }
            return '0';
        }
        
        function input(data) {
            ShowInfo("");
            var lstPrintItem = getPrintItemCollection();
            custsn = checkInput(data)
            if (custsn == '0') {
                alert("wrong code");
                getAvailableData("input");
            } else if (lstPrintItem == null){
                    ShowInfo(msgPrintSettingPara);
                    getAvailableData("input");
//                    return;
                }
                else 
                {
                    beginWaitingCoverDiv();
                    RegenerateCustSN.input("", custsn, editor, stationId, customer, lstPrintItem,inputSucc, inputFail);
                } 
            
        }

        function inputSucc(result) {
            setInfo(custsn, result);
            endWaitingCoverDiv();
            //
            if (result == null) {
                //service方法没有返回
                ShowErrorMessage(msgSystemError);
            }
            else if (result[0] == SUCCESSRET) {

                var LabelType = "DK_SN_Label";
                var keyCollection = new Array();
                var valueCollection = new Array();

                keyCollection[0] = "@sn";
                valueCollection[0] = generateArray(result[1][4]); //sn

                setPrintParam(result[1][0], LabelType, keyCollection, valueCollection); //result
                printLabels(result[1][0], false);
                ShowSuccessfulInfo(true);

            }
            getAvailableData("input");
            inputObj.focus();
        }

        function inputFail(result) {
            //show error message
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            
            endWaitingCoverDiv();
            getAvailableData("input");
            inputObj.focus();
        }
        function setnull() {
            //set value to the label
            setInputOrSpanValue(document.getElementById("<%=txtProdId.ClientID %>"), ""); 
            setInputOrSpanValue(document.getElementById("<%=txtModel.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=txtPdline.ClientID %>"), "");
        }

        function setInfo(custsn, info) {
            //set value to the label
            setInputOrSpanValue(document.getElementById("<%=txtProdId.ClientID %>"), info[1][1]); 
            setInputOrSpanValue(document.getElementById("<%=txtModel.ClientID %>"), info[1][2]);
            setInputOrSpanValue(document.getElementById("<%=txtPdline.ClientID %>"), info[1][3]);
            radChangeToEPIA = info[3];
        }

        function initPage() {
            setInputOrSpanValue(document.getElementById("<%=txtProdId.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=txtModel.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=txtPdline.ClientID %>"), "");

            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            //inputFlag = false;
            defectCount = 0;
            defectInTable = [];
            preStation = "";
            qcStatus = "";
        }

 

        window.onbeforeunload = function() {
            
                OnCancel();

        };

        function OnCancel() {
          //  WebServicePIAOutput.cancel(custsn); // 由gprodId改成custsn
        }

        function ExitPage() {
            OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
        }

        
    </script>

</asp:Content>

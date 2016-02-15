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
    AutoEventWireup="true" CodeFile="EPIAInputForDocking.aspx.cs" Inherits="Docking_EPIAInput" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/Docking/Service/WebServiceEPIAInputForDocking.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">

                  <tr><td></td><td>&nbsp;</td></tr> 
                  <tr><td></td><td>&nbsp;</td></tr> 
                 <tr><td></td><td>&nbsp;</td></tr> 

                <tr>
                <td style="width: 10%" align="left"></td>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel" ></asp:Label>
                    </td>
                    <td colspan="4" align="left">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="300px"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <input id="prodHidden" type="hidden" runat="server" />
                                <input id="sumCountHidden" type="hidden" runat="server" />
                                <input type="hidden" runat="server" id="station" />
                                <input id="scanQtyHidden" type="hidden" runat="server" value="0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                
                    
                <tr>
                <td style="width: 10%" align="left"></td>
                    <td style="width: 10%" align="left">
                        <asp:Label ID="labQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 10%" align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtPassQty" runat="server" CssClass="label_normal" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 10%" align="left">
                        <asp:Label ID="labEpiaQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtEpiaQty" runat="server" CssClass="label_normal" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                 
                <tr>
                <td style="width: 10%" align="left"></td>
                    <td style="width: 10%" align="left">
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
                
                <tr>
                <td style="width: 10%" align="left"></td>
                <td style="width: 10%" align="left">
                        <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtModel" runat="server" CssClass="label_normal" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td></tr>
                <tr>
                <td style="width: 10%" align="left"></td>
                    <td style="width: 10%" align="left">
                        <asp:Label ID="lblCustomSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="txtCustomSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                     
                    </td>
                    
                    
                </tr>
                
                     <tr><td></td><td>&nbsp;</td></tr> 
                        <tr><td></td><td>&nbsp;</td></tr> 
                               <tr><td></td><td>&nbsp;</td></tr> 
                    <tr><td></td><td>&nbsp;</td></tr> 
                      <tr><td></td><td>&nbsp;</td></tr> 
                         <tr><td></td><td>&nbsp;</td></tr> 
                      <tr><td></td><td>&nbsp;</td></tr> 
                        <tr><td></td><td>&nbsp;</td></tr> 
                <tr>
                   
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
        //var msgEPIASuc = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgEPIASuc") %>';
        var msgEPIASuc = '<%=this.GetLocalResourceObject(Pre + "_msgEPIASuc").ToString()%>';
        var msgPIASuc = '<%=this.GetLocalResourceObject(Pre + "_msgPIASuc").ToString()%>';

        var prodid = "";
        var radChangeToEPIA;
        var passQty = 0;
        var failQty = 0;

        window.onload = function() {
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            setInputOrSpanValue(document.getElementById("<%=txtPassQty.ClientID %>"), passQty);
            setInputOrSpanValue(document.getElementById("<%=txtEpiaQty.ClientID %>"), failQty);
            inputObj.focus();
        };
        
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
           
            custsn = checkInput(data)
            if (custsn == '0') {
                alert("wrong code");
                getAvailableData("input");
            } else {
                beginWaitingCoverDiv();
                WebServiceChangeToEPIAPIA.input("", custsn, editor, stationId, customer, inputSucc, inputFail);
            }
        }

        function inputSucc(result) {
            setInfo(custsn, result);
            endWaitingCoverDiv();
            //
            ShowSuccessfulInfo(true);
            if (radChangeToEPIA != "1") {
                //ShowMessage(msgEPIASuc);
                // var msgSuccess1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
                passQty++;
                setInputOrSpanValue(document.getElementById("<%=txtPassQty.ClientID %>"), passQty);
                ShowSuccessfulInfo(true, "[" + custsn + "]" + msgPIASuc );
            }
            else {
                // ShowMessage(msgPIASuc);
                //var msgSuccess1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
                failQty++;
                setInputOrSpanValue(document.getElementById("<%=txtEpiaQty.ClientID %>"), failQty);
                ShowSuccessfulInfo(true, "[" + custsn + "]" + msgEPIASuc);
            }
            
           // setnull();
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
            setInputOrSpanValue(document.getElementById("<%=txtCustomSN.ClientID %>"), "");
        }

        function setInfo(custsn, info) {
            //set value to the label
            setInputOrSpanValue(document.getElementById("<%=txtProdId.ClientID %>"), info[1]["productId"]); 
            setInputOrSpanValue(document.getElementById("<%=txtModel.ClientID %>"), info[0]["modelId"]);
            setInputOrSpanValue(document.getElementById("<%=txtCustomSN.ClientID %>"), info[0]["customSN"]);
            radChangeToEPIA = info[3];
        }

        function initPage() {
            setInputOrSpanValue(document.getElementById("<%=txtProdId.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=txtModel.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=txtCustomSN.ClientID %>"), "");

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

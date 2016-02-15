<%--
/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:ChangeSamplePO page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 
 * Known issues:
 */
 --%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ChangeSamplePO.aspx.cs" Inherits="FA_ChangeSamplePO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServiceChangeSamplePO.asmx" />
        </Services>
    </asp:ScriptManager>
    <div style="width: 96%; margin: 0 auto;">
        <table width="100%">
            <tr>
				<td align="left" width="15%">
					<asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
				</td>
				<td align="left">
					<iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />                         
				</td>
			</tr>
		</table>
		<table width="100%">
            <tr>
				<td align="left" width="50%">
					<asp:Panel ID="Panel1" runat="server" Width="100%">
					<table width="100%"><tr><td>
					<asp:Label ID="lblProdId" runat="server" CssClass="iMes_label_13pt" Text="ProdId：" />
					<asp:Label ID="lblProdIdValue" runat="server" CssClass="iMes_label_13pt" Text="" /><br/>
					<asp:Label ID="lblCustSN" runat="server" CssClass="iMes_label_13pt" Text="CustSN：" />
					<asp:Label ID="lblCustSNValue" runat="server" CssClass="iMes_label_13pt" Text="" /><br/>
					<asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt" Text="Model：" />
					<asp:Label ID="lblModelValue" runat="server" CssClass="iMes_label_13pt" Text="" /><br/>
					<asp:Label ID="lblPoNo" runat="server" CssClass="iMes_label_13pt" Text="PoNo：" />
					<asp:Label ID="lblPoNoValue" runat="server" CssClass="iMes_label_13pt" Text="" /><br/>
					<asp:Label ID="lblMO" runat="server" CssClass="iMes_label_13pt" Text="MO：" />
					<asp:Label ID="lblMOValue" runat="server" CssClass="iMes_label_13pt" Text="" /><br/>
					<asp:Label ID="lblStation" runat="server" CssClass="iMes_label_13pt" Text="Station：" />
					<asp:Label ID="lblStationValue" runat="server" CssClass="iMes_label_13pt" Text="" /><br/>
					</td></tr></table>
					</asp:Panel>
				</td>
				<td align="left" width="50%">
					<asp:Panel ID="Panel2" runat="server" Width="100%">
					<table width="100%"><tr><td>
					<asp:Label ID="lblProdId2" runat="server" CssClass="iMes_label_13pt" Text="ProdId：" />
					<asp:Label ID="lblProdIdValue2" runat="server" CssClass="iMes_label_13pt" Text="" /><br/>
					<asp:Label ID="lblCustSN2" runat="server" CssClass="iMes_label_13pt" Text="CustSN：" />
					<asp:Label ID="lblCustSNValue2" runat="server" CssClass="iMes_label_13pt" Text="" /><br/>
					<asp:Label ID="lblModel2" runat="server" CssClass="iMes_label_13pt" Text="Model：" />
					<asp:Label ID="lblModelValue2" runat="server" CssClass="iMes_label_13pt" Text="" /><br/>
					<asp:Label ID="lblPoNo2" runat="server" CssClass="iMes_label_13pt" Text="PoNo：" />
					<asp:Label ID="lblPoNoValue2" runat="server" CssClass="iMes_label_13pt" Text="" /><br/>
					<asp:Label ID="lblMO2" runat="server" CssClass="iMes_label_13pt" Text="MO：" />
					<asp:Label ID="lblMOValue2" runat="server" CssClass="iMes_label_13pt" Text="" /><br/>
					<asp:Label ID="lblStation2" runat="server" CssClass="iMes_label_13pt" Text="Station：" />
					<asp:Label ID="lblStationValue2" runat="server" CssClass="iMes_label_13pt" Text="" /><br/>
					</td></tr></table>
					</asp:Panel>
				</td>
			</tr>
		</table>
		
        <table width="100%">
            <tr>
                <td style="width: 20%">
                    <asp:Label ID="LabelDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td style="width: 80%">
                    <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" Width="80%" CanUseKeyboard="true"
                        IsClear="true" IsPaste="true" MaxLength="12" />
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        var sessionKey = "";
        var editor;
        var customer;
        var hostname = "";
        var langPre = "eng_";
        var station = "<%=Station %>";
        var line = "";
        var dataEntryObj;
		var msgSuccess = 'SUCCESS';
		var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
		var msgPdLineNull = '<%=this.GetLocalResourceObject(Pre + "msgPdLineNull").ToString() %>';
		var msgInputSN1 = '<%=this.GetLocalResourceObject(Pre + "msgInputSN1").ToString() %>';
		var msgInputSN2 = '<%=this.GetLocalResourceObject(Pre + "msgInputSN2").ToString() %>';
		var msgInput9999 = '<%=this.GetLocalResourceObject(Pre + "msgInput9999").ToString() %>';

        var sn1 = "";
        var sn2 = "";
        
        window.onload = function() {
            editor = '<%=Editor %>';
            customer = '<%=Customer %>';
            langPre = '<%=Pre %>';

            dataEntryObj = getCommonInputObject();
            getAvailableData("InputDataEntry");
            try {
                dataEntryObj.focus();
				ShowInfo(msgInputSN1);
            } catch (e) { }
        };

        function InputDataEntry(InputData) {
			if ("" == sn1){
				ResetUI();
				if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
					alert(msgPdLineNull);
					callNextInput();
					return;
				}
				sn1 = InputData;
				beginWaitingCoverDiv();
				WebServiceChangeSamplePO.InputSN1(InputData, editor, getPdLineCmbValue(), station, customer, InputSN1Succeed, onSN1Fail);
			}
			else if ("" == sn2){
				sn2 = InputData;
				beginWaitingCoverDiv();
				WebServiceChangeSamplePO.InputSN2(sn1, sn2, InputSN2Succeed, onSN2Fail);
			}
			else if ("9999" == InputData){
				beginWaitingCoverDiv();
				WebServiceChangeSamplePO.Change(sn1, sn2, ChangeSucceed, onChangeFail);
			}
			else{
				ShowInfo(msgInput9999);
			}
			callNextInput();
        }

        function InputSN1Succeed(result) {
            endWaitingCoverDiv();
			try {
				if (result == null) {
					ShowInfo("");
					var content = msgSystemError;
					ShowMessage(content);
					ShowInfo(content);
					callNextInput();
				} else if (result[0] == SUCCESSRET) {
					ShowProd1(result[1]);
					ShowInfo(msgInputSN2);
				} else {
					ShowInfo("");
					var content1 = result[0];
					ShowMessage(content1);
					ShowInfo(content1);
					dataEntryObj.value = "";
					dataEntryObj.focus();
				}
			} catch (e) {
				alert(e);
				callNextInput();
			}
        }
		
		function onSN1Fail(error) {
			endWaitingCoverDiv();
			try {
				sn1 = "";
				sn2 = "";
				ShowMessage(error.get_message());
				ShowInfo(error.get_message());
				callNextInput();
			} catch (e) {
				alert(e.description);
				endWaitingCoverDiv();
				dataEntryObj.focus();
			}
		}
		
		function InputSN2Succeed(result) {
            endWaitingCoverDiv();
			try {
				if (result == null) {
					sn2 = "";
					ShowInfo("");
					var content = msgSystemError;
					ShowMessage(content);
					ShowInfo(content);
					callNextInput();
				} else if (result[0] == SUCCESSRET) {
					ShowProd2(result[1]);
					ShowInfo(msgInput9999);
				} else {
					sn2 = "";
					ShowInfo("");
					var content1 = result[0];
					ShowMessage(content1);
					ShowInfo(content1);
					dataEntryObj.value = "";
					dataEntryObj.focus();
				}
			} catch (e) {
				alert(e);
				callNextInput();
			}
        }
		
		function onSN2Fail(error) {
			endWaitingCoverDiv();
			try {
				sn2 = "";
				ShowMessage(error.get_message());
				ShowInfo(error.get_message());
				callNextInput();
			} catch (e) {
				alert(e.description);
				endWaitingCoverDiv();
				dataEntryObj.focus();
			}
		}
		
		function ChangeSucceed(result) {
            endWaitingCoverDiv();
			try {
				sn1 = "";
				sn2 = "";
				if (result == null) {
					ShowInfo("");
					var content = msgSystemError;
					ShowMessage(content);
					ShowInfo(content);
					callNextInput();
				} else if (result[0] == SUCCESSRET) {
					ShowProd1(result[1]);
					ShowProd2(result[2]);
					ShowSuccessfulInfo(true, msgSuccess)
				} else {
					ShowInfo("");
					var content1 = result[0];
					ShowMessage(content1);
					ShowInfo(content1);
					dataEntryObj.value = "";
					dataEntryObj.focus();
				}
			} catch (e) {
				alert(e);
				callNextInput();
			}
        }
		
		function onChangeFail(error) {
			endWaitingCoverDiv();
			try {
				sn1 = "";
				sn2 = "";
				ShowMessage(error.get_message());
				ShowInfo(error.get_message());
				callNextInput();
			} catch (e) {
				alert(e.description);
				endWaitingCoverDiv();
				dataEntryObj.focus();
			}
		}

        function Cleanup() {
            window.clearInterval(idTmr);
            CollectGarbage();
        }

		function ShowProd1(objPrd){
			document.getElementById("<%=lblProdIdValue.ClientID %>").innerText = objPrd.ProdId;
			document.getElementById("<%=lblCustSNValue.ClientID %>").innerText = objPrd.CustSN;
			document.getElementById("<%=lblModelValue.ClientID %>").innerText = objPrd.Model;
			document.getElementById("<%=lblPoNoValue.ClientID %>").innerText = objPrd.PoNo;
			document.getElementById("<%=lblMOValue.ClientID %>").innerText = objPrd.MO;
			document.getElementById("<%=lblStationValue.ClientID %>").innerText = objPrd.Station;
		}
		
		function ShowProd2(objPrd){
			document.getElementById("<%=lblProdIdValue2.ClientID %>").innerText = objPrd.ProdId;
			document.getElementById("<%=lblCustSNValue2.ClientID %>").innerText = objPrd.CustSN;
			document.getElementById("<%=lblModelValue2.ClientID %>").innerText = objPrd.Model;
			document.getElementById("<%=lblPoNoValue2.ClientID %>").innerText = objPrd.PoNo;
			document.getElementById("<%=lblMOValue2.ClientID %>").innerText = objPrd.MO;
			document.getElementById("<%=lblStationValue2.ClientID %>").innerText = objPrd.Station;
		}
		
        function ResetUI() {
            sn1 = "";
            sn2 = "";
            document.getElementById("<%=lblProdIdValue.ClientID %>").innerText = "";
			document.getElementById("<%=lblCustSNValue.ClientID %>").innerText = "";
			document.getElementById("<%=lblModelValue.ClientID %>").innerText = "";
			document.getElementById("<%=lblPoNoValue.ClientID %>").innerText = "";
			document.getElementById("<%=lblMOValue.ClientID %>").innerText = "";
			document.getElementById("<%=lblStationValue.ClientID %>").innerText = "";
			document.getElementById("<%=lblProdIdValue2.ClientID %>").innerText = "";
			document.getElementById("<%=lblCustSNValue2.ClientID %>").innerText = "";
			document.getElementById("<%=lblModelValue2.ClientID %>").innerText = "";
			document.getElementById("<%=lblPoNoValue2.ClientID %>").innerText = "";
			document.getElementById("<%=lblMOValue2.ClientID %>").innerText = "";
			document.getElementById("<%=lblStationValue2.ClientID %>").innerText = "";
        }
        function ShowError(result) {
            ShowMessage(result);
            ShowInfo(result);
            getAvailableData("InputDataEntry");
            dataEntryObj.focus();
        }

        function Cancel() {
            if (sn1 != "") {
                WebServiceChangeSamplePO.Cancel(sn1);
            }
			sn1 = "";
            sn2 = "";
        }
		
		function callNextInput() {
			getCommonInputObject().value = "";
			getCommonInputObject().focus();
			getAvailableData("InputDataEntry");
		}

        window.onbeforeunload = function() {
            Cancel();
        };
    </script>

</asp:Content>

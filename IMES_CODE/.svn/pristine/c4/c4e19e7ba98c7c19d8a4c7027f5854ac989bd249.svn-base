<%--
/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for RemoveKPCT
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="RemoveKPCT.aspx.cs" Inherits="FA_RemoveKPCT" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td align="left" width="15%">
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />                         
                    </td>
                </tr>
                
                <tr>
                    <td align="left">
                    </td>
                    <td align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblProdId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
								<asp:Label ID="txtProdId" runat="server" CssClass="iMes_label_13pt" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>             
                
                <tr>
                    <td colspan="2" align="left">
                        <asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="240px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                    Width="99.9%" Height="230px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left">
                                    <Columns>
                                        <asp:BoundField DataField="ScanedPartSn" />
                                        <asp:BoundField DataField="KP" />
										<asp:BoundField DataField="PartSn" />
                                    </Columns>
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                
                <tr>
                    <td align="left">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td align="left">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnInputProdId" runat="server" type="button" style="display: none" />
								<button id="btnInputMBCT2" runat="server" type="button" style="display: none" />
                                <button id="btnSave" runat="server" type="button" style="display: none" />
                                <button id="btnExit" runat="server" type="button" style="display: none" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="upHidden" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <input type="hidden" runat="server" id="hidStation" />
                                <input type="hidden" runat="server" id="hidInput" />
								<input type="hidden" runat="server" id="hidCntPartSn" value="0" />
								<input type="hidden" runat="server" id="hidScanedPartSn" value="0" />
								<input type="hidden" runat="server" id="hidMbct2" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </center>
    </div>

    <script type="text/javascript">        
		
        document.body.onload = function() {
            try {
                getPdLineCmbObj().setAttribute("AutoPostBack", "True");
                getAvailableData("processDataEntry");
            } catch (e) {
                ShowInfo(e.description);
            }
        }

        function processDataEntry(inputData) {
            ShowInfo("");
            line = getPdLineCmbValue();
            if (line == "") {
                ShowInfo('<%=this.GetLocalResourceObject(Pre + "_msgNoSelectPdLine").ToString()%>');
                setPdLineCmbFocus();
                getAvailableData("processDataEntry");
                return;
            }

            inputData = inputData.trim();
            var prodId = document.getElementById("<%=txtProdId.ClientID%>").innerText.trim();
            
            //scan 9999 to save
            /*if (inputData == "9999") {
                if (prodId == "") {
                    ShowInfo('<%=this.GetLocalResourceObject(Pre + "_msgNoProdIdInput").ToString()%>');
                    callNextInput();
                    return;
                }
                beginWaitingCoverDiv();
                document.getElementById("<%=btnSave.ClientID%>").click();
                return;
            }*/

            if (prodId == "") {  //Need input prodId
                if (inputData.length != 9) {
                    ShowInfo('Wrong Code!');
                    callNextInput();
                    return;
                }
                
                beginWaitingCoverDiv();
                document.getElementById("<%=hidCntPartSn.ClientID%>").value = "0";
                document.getElementById("<%=hidScanedPartSn.ClientID%>").value = "0";
                document.getElementById("<%=hidInput.ClientID%>").value = inputData;
                document.getElementById("<%=btnInputProdId.ClientID%>").click();
                return;
            }
			
			var mbct2 = document.getElementById("<%=hidMbct2.ClientID%>").value;
			if (mbct2 == "") {
				beginWaitingCoverDiv();
				document.getElementById("<%=hidInput.ClientID%>").value = inputData;
				document.getElementById("<%=btnInputMBCT2.ClientID%>").click();
                return;
			}
			else { // scan PartSn
				if (ChkPartSn(inputData)) {
				    var scaned = parseInt(document.getElementById("<%=hidScanedPartSn.ClientID%>").value) + 1;
				    document.getElementById("<%=hidScanedPartSn.ClientID%>").value = scaned;
					if (scaned >= parseInt(document.getElementById("<%=hidCntPartSn.ClientID%>").value)) {
						beginWaitingCoverDiv();
						document.getElementById("<%=btnSave.ClientID%>").click();
						return;
					}
					else {
						ShowInfo('<%=this.GetLocalResourceObject(Pre + "_msgScanPartSn").ToString()%>');
						callNextInput();
						return;
					}
				}
				else {
					ShowInfo('<%=this.GetLocalResourceObject(Pre + "_msgBadPartSn").ToString()%>');
					callNextInput();
					return;
				}
			}

            //document.getElementById("<%=hidInput.ClientID%>").value = inputData;
            return;
        }

        function callNextInput() {
            getCommonInputObject().focus();
            getCommonInputObject().select();
            getAvailableData("processDataEntry");
        }

        window.onbeforeunload = function() {
            document.getElementById("<%=btnExit.ClientID%>").click();
        }
        
        window.onunload = function() {
            document.getElementById("<%=btnExit.ClientID%>").click();
        }
		
		function ChkPartSn(partsn) {
			var gdObj = document.getElementById("<%=GridViewExt1.ClientID%>");
			for (var i = 1; i < gdObj.rows.length; i++) {
			    if ((partsn == gdObj.rows[i].cells[2].innerText) && (partsn != gdObj.rows[i].cells[0].innerText)) {
					gdObj.rows[i].cells[0].innerText = partsn;
					return true;
				}
			}
			return false;
		}
    </script>

</asp:Content>

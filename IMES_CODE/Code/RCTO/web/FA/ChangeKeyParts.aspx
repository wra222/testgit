<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for ChangeKeyParts Page
 *             
 * UI:CI-MES12-SPEC-FA-UI Change Key Parts.docx –2011/10/26 
 * UC:CI-MES12-SPEC-FA-UC Change Key Parts.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-11  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="ChangeKeyParts.aspx.cs" Inherits="FA_ChangeKeyParts" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="Service/WebServiceChangeKeyParts.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="5">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true"  />                         
                    </td>
                </tr>
                
                <tr>
                    <td align="left">
                        <asp:Label ID="lblKeyPart" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3" align="left"> 
                        <iMES:CmbChangeKPType ID="cmbKPT" runat="server" Width="100" IsPercentage="true"   />                            
                    </td>
                    <td align="left">
                        <asp:Label ID="lblRetutnWC" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">  
                        <asp:UpdatePanel runat="server" ID="upRetWC" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtRetWC" runat="server" CssClass="iMes_label_13pt" />
                            </ContentTemplate>
                        </asp:UpdatePanel>                     
                    </td>
                </tr>             
                
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lblProId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 20%" align="left">
                        <asp:UpdatePanel runat="server" ID="upProId" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtProId" runat="server" CssClass="iMes_label_13pt" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lblCTSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 15%" align="left">
                        <asp:UpdatePanel runat="server" ID="upCTSN" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtCTSN" runat="server" CssClass="iMes_label_13pt" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:UpdatePanel runat="server" ID="upModel" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_13pt" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="6" align="left">
                        <asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="6">
                        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="240px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                    Width="99.9%" Height="230px" OnRowDataBound="gd_DataBound" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left">
                                    <Columns>
                                        <asp:BoundField DataField="Type"  />
                                        <asp:BoundField DataField="Desc"  />
                                        <asp:BoundField DataField="Name" />
                                        <asp:BoundField DataField="Qty" />
                                        <asp:BoundField DataField="PQty" />
                                        <asp:BoundField DataField="Collection" />
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
                    <td align="left" colspan="5">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnGridFresh" runat="server" type="button" onclick="" style="display: none" /> 
                                <button id="btnReset" runat="server" type="button" onclick="" style="display: none" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="upHidden" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <input type="hidden" runat="server" id="hidStation" />
                                <input type="hidden" runat="server" id="hidLine" />
                                <input type="hidden" runat="server" id="hidKpt" />
                                <input type="hidden" runat="server" id="hidRowCnt" />
                                <input type="hidden" runat="server" id="hidWantData" />
                                <input type="hidden" runat="server" id="hidInput" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </center>
    </div>

    <script type="text/javascript">

        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
        var mesNoSelKPT = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectKPT").ToString()%>';
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";

        document.body.onload = function() {
            try {
                getPdLineCmbObj().setAttribute("AutoPostBack", "True");
                document.getElementById("<%=hidWantData.ClientID%>").value = "0";   //ProdId wanted
                getAvailableData("processDataEntry");
            } catch (e) {
                alert(e.description);
            }

        }

        function processDataEntry(inputData) {
            ShowInfo("");
            selLine = getPdLineCmbValue();
            selKpt = getChangeKPTypeCmbValue();
            if (selLine == "") {
                alert(mesNoSelPdLine);
                setPdLineCmbFocus();
                getAvailableData("processDataEntry");
                return;
            }
            if (selKpt == "") {
                alert(mesNoSelKPT);
                setChangeKPTypeCmbFocus();
                getAvailableData("processDataEntry");
                return;
            }
            document.getElementById("<%=hidLine.ClientID%>").value = selLine;
            document.getElementById("<%=hidKpt.ClientID%>").value = selKpt;

            try {
                prodid = document.getElementById("<%=txtProId.ClientID%>").innerText;

                if (document.getElementById("<%=hidWantData.ClientID%>").value == "0") {
                    if (!isProdIDorCustSN(inputData, selLine)) {
                        alert('<%=this.GetLocalResourceObject(Pre + "_msgBadProductID").ToString()%>');
                        callNextInput();
                        return;
                    }
                    if (isProdID(inputData, selLine)) {
                        inputData = SubStringSN(inputData, "ProdId");
                    }
                    document.getElementById("<%=hidInput.ClientID%>").value = inputData;
                    beginWaitingCoverDiv();
                    document.getElementById("<%=btnGridFresh.ClientID%>").click();
                }
                else {
                    beginWaitingCoverDiv();
                    if (inputData == "7777") {
                        WebServiceChangeKeyParts.ClearCT(prodid, onClearSuccess, onSaveFail);
                    }
                    else {
                        WebServiceChangeKeyParts.InputPartNo(prodid, inputData, onSaveSuccess, onSaveFail);
                    }
                }
                return;
            } catch (e) {
                alert(e.description);
            }
        }

        function onClearSuccess(result) {
            try {
                if (result == null) {
                    endWaitingCoverDiv();
                    var content = msgSystemError;
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }
                else if ((result.length == 1) && (result[0] == SUCCESSRET)) {
                    //处理界面输出信息
                    gvTable = document.getElementById("<%=GridViewExt1.ClientID%>");
                    rowCnt = parseInt(document.getElementById("<%=hidRowCnt.ClientID%>").value);
                    for (k = 1; k <= rowCnt; k++) {
                        gvTable.rows[k].cells[4].innerText = "0";
                        gvTable.rows[k].cells[5].innerText = "";
                        gvTable.rows[k].cells[5].title = "";
                    }
                    endWaitingCoverDiv();
                    disableCombox();
                    callNextInput();
                }
                else {
                    endWaitingCoverDiv();
                    disableCombox();
                    var content = result[0];
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }
            }
            catch (e) {
                alert(e.description);
                endWaitingCoverDiv();
                disableCombox();
            }         
        }

        function onSaveSuccess(result) {
            try {
                if (result == null) {
                    endWaitingCoverDiv();
                    var content = msgSystemError;
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }
                else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
                    //处理界面输出信息
                    kpType = document.getElementById("<%=hidKpt.ClientID%>").value;
                    gvTable = document.getElementById("<%=GridViewExt1.ClientID%>");
                    qtySum = 0;
                    pqtySum = 0;
                    alreadyMatch = false;
                    rowCnt = parseInt(document.getElementById("<%=hidRowCnt.ClientID%>").value);
                    repVC = result[1];
                    for (k = 1; k <= rowCnt; k++) {
                        qty = parseInt(gvTable.rows[k].cells[3].innerText);
                        pqty = parseInt(gvTable.rows[k].cells[4].innerText);
                        qtySum += qty;
                        pqtySum += pqty;
                        if (!alreadyMatch) {
                            bFound = false;
                            if (kpType == "CPU") {
                                bFound = true;
                            }
                            else {
                                oldVC = "," + gvTable.rows[k].cells[2].innerText + ",";
                                if (oldVC.indexOf("," + repVC.substring(0, 5) + ",") >= 0) {
                                    bFound = true;
                                }
                            }

                            if (bFound) {
                                if (qty <= pqty) {
                                    /*
                                    * Answer to: ITC-1360-1659
                                    * Description: Deal with multi-line of same part No..
                                    */
                                    continue;
                                }
                                alreadyMatch = true;
                                pqty++;
                                pqtySum++;
                                gvTable.rows[k].cells[4].innerText = pqty;
                                collectionData = gvTable.rows[k].cells[5].title.trim();
                                if (collectionData.length != 0) {
                                    collectionData += "," + result[1];
                                }
                                else {
                                    collectionData = result[1];
                                }
                                toShowReg = new RegExp(/^[^,]*,[^,]*,[^,]*,/);
                                toShow = collectionData.match(toShowReg);
                                if (toShow != null) {
                                    gvTable.rows[k].cells[5].innerText = toShow + " ...";
                                }
                                else {
                                    gvTable.rows[k].cells[5].innerText = collectionData;
                                }
                                gvTable.rows[k].cells[5].title = collectionData;
                            }
                        }
                    }
                    endWaitingCoverDiv();
                    disableCombox();
                    if (qtySum <= pqtySum) {
                        document.getElementById("<%=btnReset.ClientID%>").click();
                    }
                    callNextInput();
                }
                else {
                    endWaitingCoverDiv();
                    disableCombox();
                    var content = result[0];
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }
            } 
            catch (e) {
                alert(e.description);
                endWaitingCoverDiv();
                disableCombox();
            }
        }
        
        function onSaveFail(error) {
            endWaitingCoverDiv();
            disableCombox();
            try {
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
                callNextInput();
            } catch (e) {
                alert(e.description);
            }
        }
        
        function disableCombox() {
            getPdLineCmbObj().disabled = true;
            getChangeKPTypeCmbObj().disabled = true;
        }

        /*
        * Answer to: ITC-1360-1086
        * Description: Focus data entry.
        */
        function callNextInput() {
            getCommonInputObject().focus();
            getCommonInputObject().select();
            getAvailableData("processDataEntry");
        }

        window.onbeforeunload = function() {
            prodid = document.getElementById("<%=txtProId.ClientID%>").innerText;
            WebServiceChangeKeyParts.Cancel(prodid);
        }

        /*
        * Answer to: ITC-1360-0918
        * Description: Clear session before reload.
        */
        window.onunload = function() {
            prodid = document.getElementById("<%=txtProId.ClientID%>").innerText;
            WebServiceChangeKeyParts.Cancel(prodid);
        }
    </script>

</asp:Content>

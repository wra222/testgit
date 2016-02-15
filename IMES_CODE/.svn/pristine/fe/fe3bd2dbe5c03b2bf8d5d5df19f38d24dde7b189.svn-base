<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for AFTMVS Page
 *             
 * UI:CI-MES12-SPEC-FA-UI AFT MVS.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC AFT MVS.docx –2011/10/25            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-28  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="AFTMVS.aspx.cs" Inherits="FA_AFTMVS" %>

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
                    <td style="width:83%">
                        <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                                     
                                </td>
                            </tr>
                            
                            <tr>
                                <td style="width: 15%" align="left">
                                    <asp:Label ID="lbPassCnt" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td style="width: 25%" align="left">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="txtPassCnt" runat="server" CssClass="iMes_label_13pt" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                
                                <td style="width: 15%" align="left">
                                    <asp:Label ID="lblEpiaCnt" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td style="width: 25%" align="left">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="txtEpiaCnt" runat="server" CssClass="iMes_label_13pt" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="width: 10%" align="left">
                                    <!--<asp:Label ID="lblPiaCnt" runat="server" CssClass="iMes_label_13pt"></asp:Label>-->
                                </td>
                                <td align="left">
                                    <!--<asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="txtPiaCnt" runat="server" CssClass="iMes_label_13pt" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>-->
                                </td>
                            </tr>             
                            
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblProId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="txtProId" runat="server" CssClass="iMes_label_13pt" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
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
                                                GvExtWidth="100%" GvExtHeight="240px" OnRowDataBound="gd_DataBound" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                                Width="99.9%" Height="230px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                                HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartType"  />
                                                    <asp:BoundField DataField="Descr"  />
                                                    <asp:BoundField DataField="PartNo" />
                                                    <asp:BoundField DataField="Qty" />
                                                    <asp:BoundField DataField="PQty" />
                                                    <asp:BoundField DataField="ColData" />
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
                                            <button id="btnProcess" runat="server" type="button" onclick="" style="display: none" />
                                            <button id="btnExit" runat="server" type="button" onclick="" style="display: none" />
                                            <button id="btnComplete" runat="server" type="button" onclick="" style="display: none" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="upHidden" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                        <ContentTemplate>
                                            <input type="hidden" runat="server" id="hidStation" />
                                            <input type="hidden" runat="server" id="hidInput" />
                                            <input type="hidden" runat="server" id="hidProdId" />
                                            <input type="hidden" runat="server" id="hidWantData" />
                                            <input type="hidden" runat="server" id="hidRowCnt" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                    
                    <td style="width:2%">
                    </td>
                    
                    <td>
                        <asp:UpdatePanel ID="upESOP" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <table border="0" width="100%">
                                    <tr>
                                        <td style="height:105; vertical-align:middle" align="center">
                                            <img id ="Img1" runat="server" style=" height:100px; width:154px" src="" onmouseout="hiddenPic('Layer1')" alt="(No ESOP)" onmousemove="showPic(this,'Layer1')" /> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height:105; vertical-align:middle" align="center">
                                            <img id ="Img2" runat="server" style=" height:100px; width:154px" src="" onmouseout="hiddenPic('Layer1')" alt="(No ESOP)" onmousemove="showPic(this,'Layer1')" /> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height:105; vertical-align:middle" align="center">
                                            <img id ="Img3" runat="server" style=" height:100px; width:154px" src="" onmouseout="hiddenPic('Layer1')" alt="(No ESOP)" onmousemove="showPic(this,'Layer1')" /> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height:105; vertical-align:middle" align="center">
                                            <img id ="Img4" runat="server" style=" height:100px; width:154px" src="" onmouseout="hiddenPic('Layer1')" alt="(No ESOP)" onmousemove="showPic(this,'Layer1')" /> 
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>        
        </center>
    </div>

    <div id="Layer1" style="display:none;position:absolute;z-index:1"></div> 

    <script type="text/javascript">        
        
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var globalPrdID = "";
        var bInputCSN = false;

        document.body.onload = function() {
            try {
                getPdLineCmbObj().setAttribute("AutoPostBack", "True");
                document.getElementById("<%=hidWantData.ClientID%>").value = "0";   //ProdId wanted
                getAvailableData("processDataEntry");
            } catch (e) {
                alert(e.description);
            }

        }

        function showPic(obj, Layer1) {
            if (obj.alt == "(No ESOP)") return;
            var x, y;
            x = event.clientX;
            y = event.clientY;
            document.getElementById(Layer1).style.right = 200;
            document.getElementById(Layer1).style.top = 25;
            document.getElementById(Layer1).innerHTML = "<img src=" + obj.src + ">";
            document.getElementById(Layer1).style.display = "block";
        }
        
        function hiddenPic(Layer1) {
            document.getElementById(Layer1).innerHTML = "";
            document.getElementById(Layer1).style.display = "none";
        } 


        function processDataEntry(inputData) {
            ShowInfo("");
            line = getPdLineCmbValue();
            if (line == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoSelectPdLine").ToString()%>');
                setPdLineCmbFocus();
                getAvailableData("processDataEntry");
                return;
            }
            try {
                if (document.getElementById("<%=hidWantData.ClientID%>").value == "0") {
                    if (!isProdID(inputData, line)) {
                        alert('<%=this.GetLocalResourceObject(Pre + "_msgBadProductID").ToString()%>');
                        callNextInput();
                        return;
                    }
                    //if (isProdID(inputData, line)) {
                    inputData = SubStringSN(inputData, "ProdId");
                    //}
                    document.getElementById("<%=hidInput.ClientID%>").value = inputData;
                    globalPrdID = inputData;
                    beginWaitingCoverDiv();
                    document.getElementById("<%=btnProcess.ClientID%>").click();
                }
                else {
                    document.getElementById("<%=hidInput.ClientID%>").value = inputData;
                    beginWaitingCoverDiv();
                    document.getElementById("<%=btnProcess.ClientID%>").click();
                }
            } catch (e) {
                alert(e.description);
            }
        }

        function processTableUpdate(pn, ast) {
            gvTable = document.getElementById("<%=GridViewExt1.ClientID%>");
            qtySum = 0;
            pqtySum = 0;
            rowCnt = parseInt(document.getElementById("<%=hidRowCnt.ClientID%>").value);
            pattPP = /^[^*].{5}[*]+$/;
            alreadyMatch = false;
            for (k = 1; k <= rowCnt; k++) {
                qty = parseInt(gvTable.rows[k].cells[3].innerText);
                pqty = parseInt(gvTable.rows[k].cells[4].innerText);
                qtySum += qty;
                pqtySum += pqty;
                if (qty <= pqty) continue;
                if (alreadyMatch) continue;

                parttype = gvTable.rows[k].cells[0].innerText.trim();
                partno = gvTable.rows[k].cells[2].innerText.trim();
                if (pattPP.exec(partno)) continue;

                /*
                * Answer to: ITC-1360-1708
                * Description: Process table update for PP parts.
                */
                if (partno == pn || (parttype == "Customer SN" && pn == "Customer SN") || (parttype == "Product ID" && pn == "Product ID")) {
                    alreadyMatch = true;
                    pqty++;
                    pqtySum++;
                    gvTable.rows[k].cells[4].innerText = pqty;
                    collectionData = gvTable.rows[k].cells[5].title.trim();
                    if (collectionData.length != 0) {
                        collectionData += "," + ast;
                    }
                    else {
                        collectionData = ast;
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
                    if (pn == "Customer SN") bInputCSN = true;
                }
            }
            if (!alreadyMatch)  //Try PP parts
            {
                for (k = 1; k <= rowCnt; k++) {
                    partno = gvTable.rows[k].cells[2].innerText.trim();
                    if (!pattPP.exec(partno)) continue;
                    
                    qty = parseInt(gvTable.rows[k].cells[3].innerText);
                    pqty = parseInt(gvTable.rows[k].cells[4].innerText);
                    if (qty <= pqty) continue;

                    if (pn.indexOf(partno.substring(0, 5)) == 0) {
                        pqty++;
                        pqtySum++;
                        gvTable.rows[k].cells[4].innerText = pqty;
                        collectionData = gvTable.rows[k].cells[5].title.trim();
                        if (collectionData.length != 0) {
                            collectionData += "," + ast;
                        }
                        else {
                            collectionData = ast;
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
                        break;
                    }
                }
            }
            if (qtySum <= pqtySum) {
                bInputCSN = false;
                document.getElementById("<%=hidWantData.ClientID%>").value = "0";
                beginWaitingCoverDiv();
                document.getElementById("<%=btnComplete.ClientID%>").click();
            }
            else {
                if (bInputCSN) {
                    ShowInfo('<%=this.GetLocalResourceObject(Pre + "_msgInputAst").ToString()%>', 'green');
                }
                else if (qtySum == (pqtySum + 1)) {
                    ShowInfo('<%=this.GetLocalResourceObject(Pre + "_msgInputCSN").ToString()%>', 'green');
                }
                else {
                    ShowInfo('<%=this.GetLocalResourceObject(Pre + "_msgInputCSNorAst").ToString()%>', 'green');
                }
            }
        }

        /*
        * Answer to: ITC-1360-1080
        * Description: Focus data entry.
        */
        function callNextInput() {
            getCommonInputObject().focus();
            getCommonInputObject().select();
            getAvailableData("processDataEntry");
        }

        function showQCMethod(method) {
            ShowSuccessfulInfo(true, '[' + globalPrdID + ']\n' + method);
        }

        window.onbeforeunload = function() {
            document.getElementById("<%=btnExit.ClientID%>").click();
        }

        window.onunload = function() {
            document.getElementById("<%=btnExit.ClientID%>").click();
        }
    </script>

</asp:Content>

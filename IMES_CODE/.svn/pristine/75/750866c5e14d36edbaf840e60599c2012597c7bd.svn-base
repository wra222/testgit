
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ChangeAST.aspx.cs" Inherits="FA_ChangeAST" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceChangeAST.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    
    <table border="0" width="95%">
    <tr>
        <td style="width:10%"><asp:Label ID="lblPdline" runat="server" CssClass="iMes_label_11pt"></asp:Label></td>
        <td style="width:90%"><iMES:CmbPdLine ID="CmbPdLine" runat="server" Width="99%"/></td>                    
    </tr>
    </table>
    
    <hr class="footer_line" style="width:95%"/>
    
    <table border="0" width="95%">
    <tr>
        <td style="width:10%"><asp:Label ID="lblProdId1" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>                    
        <td style="width:20%">
            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
            <ContentTemplate>                    
                <asp:Label ID="lblProdId1Content" runat="server" CssClass="iMes_label_11pt"></asp:Label>
            </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <td style="width:5%">&nbsp;</td>
        <td style="width:10%"><asp:Label ID="lblModel1" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>                    
        <td style="width:20%">
            <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
            <ContentTemplate>                    
                <asp:Label ID="lblModel1Content" runat="server" CssClass="iMes_label_11pt"></asp:Label>
            </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <td style="width:35%">&nbsp;</td>
    </tr>
    <tr>
    <td colspan="6">
	    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
	    <ContentTemplate>
        <iMES:GridViewExt ID="GridView1" runat="server" AutoGenerateColumns="false" 
                      AutoHighlightScrollByValue="false" GvExtWidth="100%" Width="99.9%" GvExtHeight="120px" 
                     OnGvExtRowClick="" OnGvExtRowDblClick=""  
                     SetTemplateValueEnable="false"  GetTemplateValueEnable="false"  
                     HighLightRowPosition="3" onrowdatabound="gd1_RowDataBound" 
                     Height="110px">
                            <Columns >  
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="HeaderChk"  runat="server" onclick="ChkAllClick(this)" Text="ALL" ForeColor="Black" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="RowChk" runat="server" onclick="CheckClick(this)"/>
                                </ItemTemplate>
                             </asp:TemplateField> 
                               <asp:BoundField DataField="ASTType"  />    
                               <asp:BoundField DataField="PartNo"  />
                               <asp:BoundField DataField="PartSn"  />   
                                
                            </Columns>

                        </iMES:GridViewExt>
        </ContentTemplate>                                    
        </asp:UpdatePanel>
        </td>
    </tr>
    </table>
    
    <hr class="footer_line" style="width:95%"/>
    
    <table border="0" width="95%">
    <tr>
        <td style="width:10%"><asp:Label ID="lblProdId2" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>                    
        <td style="width:20%">
            <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
            <ContentTemplate>                    
                <asp:Label ID="lblProdId2Content" runat="server" CssClass="iMes_label_11pt"></asp:Label>
            </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <td style="width:5%">&nbsp;</td>
        <td style="width:10%"><asp:Label ID="lblModel2" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>                    
        <td style="width:20%">
            <asp:UpdatePanel ID="updatePanel5" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
            <ContentTemplate>                    
                <asp:Label ID="lblModel2Content" runat="server" CssClass="iMes_label_11pt"></asp:Label>
            </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <td style="width:35%">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="6">
        <asp:UpdatePanel ID="updatePanel6" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
            <iMES:GridViewExt ID="GridView2" runat="server" AutoGenerateColumns="true" 
                        GvExtWidth="100%" GvExtHeight="110px" OnRowDataBound="gd2_RowDataBound" 
                        Height="100px" OnGvExtRowClick="" 
                        OnGvExtRowDblClick="" style="top: 0px; left: 29px">
            </iMES:GridViewExt>
        </ContentTemplate>
        </asp:UpdatePanel>
        </td>
    </tr>
    </table>
    
    <table border="0" width="95%">
    <tr>
        <td style="width:12%"><asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></td>
	    <td style="width:68%">
	        <iMES:Input ID="DataEntry" runat="server" CssClass="iMes_textbox_input_Yellow" MaxLength="30"
                                    Width="99%" CanUseKeyboard="true" ProcessQuickInput="true" IsPaste="true" />
        </td>
        <td style="width:10%" align="left"><input id="btnChange" type="button"  runat="server" style="width:100%" onclick="change()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/></td>
        <td style="width:10%" align="left"><input id="btnSetting" type="button"  runat="server" style="width:100%" onclick="showPrintSettingDialog()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/></td>
    </tr>
    <tr>
        <td>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
            <input type="hidden" runat="server" id="hidStation" />
            <input type="hidden" runat="server" id="hidPCode" />
            <input type="hidden" runat="server" id="hidRecordCount" />
            <input type="hidden" runat="server" id="hidProdId" />
            <input type="hidden" runat="server" id="hidLine" />
            <button id="btnCheck" style="width: 0; display: none;" runat="server" onserverclick="btnCheck_ServerClick"></button>
            <button id="btnClear" style="width: 0; display: none;" runat="server" onserverclick="btnClear_ServerClick"></button>
        </ContentTemplate>
        </asp:UpdatePanel>
        </td>
    </tr>
    </table>
    </center>
       
</div>
 
<script type="text/javascript">

    
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelPdLine").ToString()%>';
    var mesNoProdId = '<%=this.GetLocalResourceObject(Pre + "_mesNoProdId").ToString()%>';
    var mesNoCheck = '<%=this.GetLocalResourceObject(Pre + "_mesNoCheck").ToString()%>';



    var SUCCESSRET = "SUCCESSRET";
    var editor = '<%=UserId%>';
    var customer = '<%=Customer%>';
    var lstPrintItem;
    var inputObj;
    var inpuProdid;
    var gvClientID = "<%=GridView1.ClientID %>";
    var gvHeaderID = "<%=GridView1.HeaderRow.ClientID%>";
    
    document.body.onload = function() {
        inputObj = getCommonInputObject();
        ShowInfo("");
        getAvailableData("ProcessInput");
        getCommonInputObject().focus();
    }

    function ProcessInput(inputData) {
        try {
            if (getPdLineCmbValue() == "") {
                alert(mesNoSelPdLine);
                setPdLineCmbFocus();
                getAvailableData("ProcessInput");
                return;
            }
            ShowInfo("");
            document.getElementById("<%=hidProdId.ClientID %>").value = inputData;
            document.getElementById("<%=hidLine.ClientID %>").value = getPdLineCmbValue();
            beginWaitingCoverDiv();
            document.getElementById("<%=btnCheck.ClientID %>").click();
            
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function inputFinish() {
        endWaitingCoverDiv();
        getAvailableData("ProcessInput");
        inputObj.focus();
        inputObj.select();
        inputObj.value = "";
    }

    function setCommonFocus() {
        endWaitingCoverDiv();
        getAvailableData("ProcessInput");
        inputObj.focus();
        inputObj.select();
        inputObj.value = "";
    } 

    function ChkAllClick(headChk) {
        try {
            gvTable = document.getElementById(gvClientID);
            
            for (var i = 0; i < gvTable.rows.length - 1; i++) {
                document.getElementById(gvClientID + "_" + i + "_RowChk").checked = headChk.checked;
            }
        } catch (e) {
            alert(e.description);
        }
    }

    function CheckClick(singleChk, id) {
        try {
            gvTable = document.getElementById(gvClientID);
            if (singleChk.checked) {
                for (var i = 0; i < gvTable.rows.length - 1; i++) {
                    if (gvTable.rows[i + 1].cells[1].innerText != " ") {
                        if (!document.getElementById(gvClientID + "_" + i + "_RowChk").checked) {
                            return;
                        }
                    }
                }
                document.getElementById(gvHeaderID + "_HeaderChk").checked = true;
            }
            else {
                document.getElementById(gvHeaderID + "_HeaderChk").checked = false;
            }
        } catch (e) {
            alert(e.description);
        }
    }
    
    function alertAndCallNext(message) {
        endWaitingCoverDiv();
        alert(message);
        getAvailableData("ProcessInput");
    }

    function change() {
        try {
            if (getPdLineCmbValue() == "") {
                alert(mesNoSelPdLine);
                setPdLineCmbFocus();
                return;
            }
            if (document.getElementById("<%=lblProdId1Content.ClientID %>").innerHTML == "") {
                alert(mesNoProdId);
                inputObj.focus();
                return;
            }
            if (document.getElementById("<%=lblProdId2Content.ClientID %>").innerHTML == "") {
                alert(mesNoProdId);
                inputObj.focus();
                return;
            }

            if (document.getElementById("<%=lblModel1Content.ClientID %>").innerHTML == "" ||
                document.getElementById("<%=lblModel2Content.ClientID %>").innerHTML == "") {
                
            }

            var station = document.getElementById("<%=hidStation.ClientID %>").value;
            var prod1 = document.getElementById("<%=lblProdId1Content.ClientID %>").innerHTML;
            var prod2 = document.getElementById("<%=lblProdId2Content.ClientID %>").innerHTML;
            var model1 = document.getElementById("<%=lblModel1Content.ClientID %>").innerHTML;
            var model2 = document.getElementById("<%=lblModel2Content.ClientID %>").innerHTML;

            var checked = false;
            try {
                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == null) {
                    msg = msgPrintSettingPara;
                    alert(msg);
                    return;
                }
            } catch (e) {
                alertAndCallNext(e);
                return;
            }
            beginWaitingCoverDiv();
            gvTable = document.getElementById(gvClientID);
            for (var i = 0; i < gvTable.rows.length - 1; i++) {
                if (document.getElementById(gvClientID + "_" + i + "_RowChk").checked) {
                    if (gvTable.rows[i + 1].cells[1].innerHTML != "&nbsp;") {
                        checked = true;
                        WebServiceChangeAST.Change(getPdLineCmbValue(), station, editor, customer, 
                                                    prod1, prod2, model1, model2,
                                                    gvTable.rows[i + 1].cells[1].innerHTML, 
                                                    gvTable.rows[i + 1].cells[2].innerHTML, 
                                                    gvTable.rows[i + 1].cells[3].innerHTML,lstPrintItem,
                                                    onSucceed, onFail);
                    }
                }
            }

            if (checked == false) {
                endWaitingCoverDiv();
                alert(mesNoCheck);
                return;
            }
            else {
                clearUI();
            }
            
        } catch (e) {
            alertAndCallNext(e.description);
        }       
    }

    function clearUI() {
        document.getElementById("<%=btnClear.ClientID %>").click();
    }

    var glo_errors = "";

    function setPrintItemListParam(backPrintItemList, prodid) {
        //============================================generate PrintItem List==========================================
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@productID";
        valueCollection[0] = generateArray(prodid);

        setPrintParam(lstPrtItem, "Asset Label", keyCollection, valueCollection);
    }
    
    function onSucceed(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
            else if (result[0] == SUCCESSRET) {
                if (result[1][0] == "1") {
                    setPrintItemListParam(result[1][1], result[1][2]);
                    printLabels(result[1][1], false);
                    ShowSuccessfulInfo(true);
                }
                else {
                    ShowInfo("Change success, but no print");
                }
            }
            else {
                endWaitingCoverDiv();
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function onFail(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
        } catch (e) {
            endWaitingCoverDiv();
            alertAndCallNext(e.description);
        }
    }
    
    function setPrintItemListParam1(backPrintItemList, sn)
    {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@CustomerSN";
        valueCollection[0] = generateArray(sn);

        setPrintParam(lstPrtItem, "DK_Shipto_Label", keyCollection, valueCollection);
    }



    function ExitPage()
    { }


    function ResetPage() {
        ExitPage();
        ShowInfo("");
        endWaitingCoverDiv();

    }

    function showPrintSettingDialog() {
        showPrintSetting(document.getElementById("<%=hidStation.ClientID%>").value, document.getElementById("<%=hidPCode.ClientID%>").value);
    }
</script>
</asp:Content>


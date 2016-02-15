<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Board Input(FA)
 Update: 9999Benson
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-27  Lucy Liu(EB2)        Create 
 2010-04-07  Lucy Liu(EB2)       Modify:   ITC-1122-0066
 2010-04-07  Lucy Liu(EB2)       Modify:   ITC-1122-0073
 Known issues:
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="TravelCardWithCollectKP.aspx.cs" Inherits="FA_TravelCardWithCollectKP" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="Service/WebServiceTravelCardWithCollectKP.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server"  Width="100" IsPercentage="true"   />
                         
                    </td>
                </tr>
                <tr>
                   <td style="width: 15%" align="left">
                            <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                           <iMES:CmbFamily ID="cmbFamily" runat="server" Width="100" IsPercentage="true"/>
                        </td>
                
                </tr>
                 <tr>
                   <td style="width:15%" align="left"><asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	               <td ><iMES:CmbModel ID="cmbModel" runat="server" Width="100" IsPercentage="true" /></td>
                 
                 </tr>
         
         
            
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td align="right">
                        <input id="btpPrintSet" type="button" runat="server" class="iMes_button" onclick="showPrintSettingDialog()"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
					<input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
                     </td>
                </tr>
            </table>
        </center>
    </div>

    <script type="text/javascript">
        function pdlineChange() {
           
        }
        var mesNoSelModel = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectModel").ToString()%>';
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgCollectNoItem = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgCollectNoItem") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var user;
        var customer;
        var station;
        var line;
        var model;
		var pCode;
		var accountid;
		var login;
		//            var url = "../FA/RePrintCollectTabletFaPart.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountid + "&Login=" + login; ;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //| Name		:	onload
        //| Author		:	Lucy Liu
        //| Create Date	:	10/27/2009
        //| Description	:	加载接受输入数据事件并置焦点
        //| Input para.	:	
        //| Ret value	:	
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        document.body.onload = function() {
            try {
                customer = "<%=customer%>";
                station = "<%=station%>";
                user = "<%=userId%>";
				    pCode = '<%=Request["PCode"] %>';

            accountid = '<%=AccountId%>';

            login = '<%=Login%>';
                getPdLineCmbObj().setAttribute("AutoPostBack", "True");
                getAvailableData("processDataEntry");
            } catch (e) {
                alert(e.description);
            }

        }


        
        function processDataEntry(inputData) {
            try {
     
                line = getPdLineCmbValue();
                model = getModelCmbValue();
                var  lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == null) {
                    alert(msgPrintSettingPara);
                    getAvailableData("processDataEntry");
                    return;
                }
          
                if (line == "") {
                    alert(mesNoSelPdLine);
                    setPdLineCmbFocus();
                    getAvailableData("processDataEntry"); //mesNoSelModel
                    return;
                }
                if (model == "") {
                    alert(mesNoSelModel);
                    setPdLineCmbFocus();
                    getAvailableData("processDataEntry"); //mesNoSelModel
                    return;
                }
                ShowInfo("");
                beginWaitingCoverDiv();
                WebServiceTravelCardWithCollectKP.InputCT(inputData, line, user, station, customer, model, lstPrintItem, onSucceed, onFail);
            } catch (e) {
                alert(e.description);
            }

        }

        function onSucceed(result) {
           endWaitingCoverDiv();
            try {
                if (result == null) {
                    setStatus(true);
                    var content = msgSystemError;
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();

                }
                else if ((result.length == 4) && (result[0] == SUCCESSRET)) {
                callNextInput();
                setPrintItemListParam( result[3],result[1], result[2], model); // 1 : PrintItem   2 : Custsn backPrintItemList, sn, id, model
                printLabels(result[3], false);
                ShowInfo("Success!");

                }
                else {
                  
                    var content = result[0];
                    ShowMessage(content);
                    ShowInfo(content);
                    callNextInput();
                }
            } catch (e) {
                alert(e.description);
            }
        }
        function onFail(error) {
            endWaitingCoverDiv();
            try {

                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
                clearData();
                callNextInput();
            } catch (e) {
                alert(e.description);

            }
        }

        function setPrintItemListParam(backPrintItemList, sn, id, model) {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            keyCollection[0] = "@sn";
            valueCollection[0] = generateArray(sn);
        //    keyCollection[1] = "@id";
           // valueCollection[1] = generateArray(id);
            //keyCollection[2] = "@model";
            //valueCollection[2] = generateArray(model);
            setPrintParam(lstPrtItem, "BIRCH FA Label", keyCollection, valueCollection);
        }
       
        function callNextInput() {

            getCommonInputObject().focus();
            getAvailableData("processDataEntry");
        }


        window.onbeforeunload = function() {
            ExitPage();

        }
        function ExitPage() {
           
        }
        function ResetPage() {
            ExitPage();
            reset();
            getCommonInputObject().value = "";
            getPdLineCmbObj().selectedIndex = 0;
            setPdLineCmbFocus();

        }
       function reprint() {
            //Station=" + fistSelStation + "&PCode=" + pcode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
            var url = "../FA/RePrintCollectTabletFaPart.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + user + "&Customer=" + customer + "&AccountId=" + accountid + "&Login=" + login; ;
            var paramArray = new Array();
            paramArray[0] = getPdLineCmbValue();
            paramArray[1] = user;
            paramArray[2] = customer;
            paramArray[3] = station;
            window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');


        }
       
        function showPrintSettingDialog() {
            showPrintSetting(station, "<%=pCode%>");
        }
    </script>

</asp:Content>

<%--
 INVENTEC corporation (c)2011 all rights reserved. 
 Description: Unpack DN by SN
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2011-03-15 chenpeng           Create 
 Known issues:
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UnpackbySNForDocking.aspx.cs" Inherits="UnpackForDocking" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="Service/UnpackService.asmx" />
        </Services>
    </asp:ScriptManager>
    
<div>
   <center >
        <table width="95%" style="vertical-align:middle; height:20%" cellpadding="0" cellspacing="0" >
        <tr style="height:10%">
            <td align="left" colspan="2">
                <asp:Label ID="lblTitle" runat="server"  CssClass="iMes_label_13pt" ></asp:Label>
            </td>
        </tr>     
        <colgroup>
                <col style="width:240px;"/>
                <col />
                <col style="width:150px;"/>
            </colgroup>
        <tr style="height:10%">
            <td style="width:15%" align="left"  >
                <asp:Label ID="lblDeliveryNo" runat="server"  CssClass="iMes_DataEntryLabel"   ></asp:Label>
            </td>
            <td   align="left"  >
                                <iMES:Input ID="Input1" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="500px"  IsPaste="true"  />

            </td>

            <td align ="right" colspan="1">
                <button  id ="btnUnpack"  style ="width:110px; height:24px;" 
                            onclick="btnUnpackClick()"  >
                            <%=Resources.iMESGlobalDisplay.ResourceManager.GetObject(Pre + "_btnOK").ToString()%>
                </button> 
            </td>  
        </tr>
<tr><td></td><td>&nbsp;</td></tr>

        <tr>
            <asp:UpdatePanel ID="UpdatePanelAll" runat="server"  RenderMode="Inline">
            <ContentTemplate>
                 <input type="hidden" runat="server" id="station" /> 
                 <input type="hidden" runat="server" id="pCode" /> 
            </ContentTemplate>   
             </asp:UpdatePanel> 
        </tr>
       </table>
  </center>
</div>

    <script type="text/javascript">

        var msgUnpackProdSNNull = '<%=this.GetLocalResourceObject(Pre + "_msgUnpackProdSNNull").ToString() %>';
        var msgInvalidPattern = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidPattern").ToString() %>';

        var msgCartonProdSNNull = '<%=this.GetLocalResourceObject(Pre + "_msgCartonProdSNNull").ToString() %>';
        //var msgInvalidPattern = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidPattern").ToString() %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var ProductIDOrSNOrCartonNo = "";
        var station = "";
        var inputControl = getCommonInputObject();
        var msgConfirmUnpack = '<%=this.GetLocalResourceObject(Pre + "_msgConfirmUnpack").ToString()%>';
        var sn = "";
        var inputSNControl;

        window.onload = function() {
            station = document.getElementById("<%=station.ClientID%>").value;
            ShowInfo("");
            //置快速控件的焦点
            //inputSNControl = getCommonInputObject();
            inputControl.focus();
            //支持回车tab键
            getAvailableData("processDataEntry");

        };

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //| Name		:	processDataEntry
        //| Author		:	Lucy Liu
        //| Create Date	:	10/27/2009
        //| Description	:	在DataEntry输入数据后，进行处理
        //| Input para.	:	
        //| Ret value	:	
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        function processDataEntry(inputData) {
            btnUnpackClick();
        }
        function checkInput(data) {
            //if (data.length == 10&& data.substring(0,2)=="CN")
			if (data.length == 10&& CheckCustomerSN(data))
                return data;
            if (data.length == 11) {
                //if (data.substring(0, 3) == "SCN")
				if (CheckCustomerSN(data))
                    return data.substring(1, 11);
                else
                    return data
            }
            return '0';
        }

        function btnUnpackClick() {
            //var input = getCommonInputObject().value;
            sn = checkInput(inputControl.value.trim());
            if (sn != '0') {
                ShowInfo("");
                beginWaitingCoverDiv();
                UnpackService.UnpackbySNCheck(sn, "<%=UserId%>", station, "<%=Customer%>", onSuccess, onFail);

            } else {
                alert(msgInvalidPattern);
                inputControl.focus();
                getAvailableData("processDataEntry");
            }
            onClearAll();
        }


        function onSuccess(result) {
            ShowInfo("");
            endWaitingCoverDiv();
            try {
                if (result == null) {
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                }

                else if (result == SUCCESSRET) {
                    //ShowSuccessfulInfo(true);
                    beginWaitingCoverDiv();
                    if (confirm(msgConfirmUnpack)) {
                        //alert("save active");
                       
                        UnpackService.UnpackbySNSave(sn, onSaveSuccess, onFail);
                    }
                    else {
                        //alert("cancle active");
                        UnpackService.cancel(sn);
                        endWaitingCoverDiv();
                        inputControl.focus();
                        getAvailableData("processDataEntry");
                    }
                }
                else {
                    var content = result;
                    ShowMessage(content);
                    ShowInfo(content);
                }

            }
            catch (e) {
                alert(e.description);
            }
            onClearAll();
            getAvailableData("processDataEntry");

        }

        function onSaveSuccess(result) {
            ShowInfo("");
            endWaitingCoverDiv();
            try {
                if (result == null) {
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                }
                else if (result == SUCCESSRET) {
                    ShowInfo(sn+"unpack ok!");
                }
                inputControl.focus();
                getAvailableData("processDataEntry");
            }
            catch (e) {
                alert(e.description);
            }
        }

        function onFail(error) {
            ShowInfo("");
            endWaitingCoverDiv();
            try {
                // ShowMessage(msgSnNoExistErr);
                // ShowInfo(msgSnNoExistErr);
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
            }
            catch (e) {
                alert(e.description);
            }
            onClearAll();
            getAvailableData("processDataEntry");

        }
        
        function onClearAll() {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            //        ProductIDOrSNOrCartonNo="";

        }


        //*********************************************************************************
        //| Function	:	Exit Page 
        //| Author		:	Chen Xu
        //| Create Date	:	03/12/2010
        //| Description	:	Clear the UI Session
        //| Input para.	:	Product ID, station
        //| Ret value	:	
        //*********************************************************************************


        window.onbeforeunload = function() {
            ExitPage();
        }

        function ExitPage() {
            //    if (ProductIDOrSNOrCartonNo!="")
            //    {
            //        CombinePOInCartonUnpackService.Cancel(ProductIDOrSNOrCartonNo);
            //        sleep(waitTimeForClear);
            //    }
            //    uutInput=true;
        }


        function ResetPage() {
            ExitPage();
            ShowInfo("");
            onClearAll();

        }


    </script>

</asp:Content>

<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for RepairInfoQuery Page
 *             
 * UI:CI-MES12-SPEC-FA-UI RepairInfo Query.docx
 * UC:CI-MES12-SPEC-FA-UC RepairInfo Query.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-30  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="RepairInfoEdit.aspx.cs" Inherits="FA_RepairInfoEdit" Title="iMES - Repair Log" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
</head>
<body style=" position: relative; width: 100%; text-align:center"> 
    <form id="form1" runat="server"> 
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Services>
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto; margin-top:25">
        <div id="div1" class="iMes_div_center">
            <table width="100%" border="0" style="table-layout:fixed;">                
                <colgroup>
	                <col style="width:145px;" />
	                <col />
	                <col style="width:110px;" />
	                <col />
                </colgroup>             
                <tr>
                    <td>
                        <asp:Label ID="lblDefect" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbDefect runat="server" ID="cmbDefect" Width="100%" />
                    </td>
                </tr>                                                           
                <tr>
                    <td>
                        <asp:Label ID="lblTestStn" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbTestStation runat="server" ID="cmbTestStn" Width="100%" />
                    </td>
                    <td>
                        <asp:Label ID="lblSubDefect" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbSubDefect ID="cmbSubDefect" runat="server" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCause" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbCause ID="cmbCause" runat="server" Width="100%" />
                    </td>
                </tr>   
                <tr>
                    <td>
                        <asp:Label ID="lblMajorPart" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbPMajorPart ID="cmbMajorPart" runat="server" Width="100%" />
                    </td>
                    <td>
                        <asp:Label ID="lblComponent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbComponent ID="cmbComponent" runat="server" Width="100%" />
                    </td>
                </tr>                                                        
                <tr>
                    <td>
                        <asp:Label ID="lblSite" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtSite" runat="server" maxlength="4" class="iMes_input_White" style="width: 80%;" />
                    </td>
                    <td>
                        <asp:Label ID="lblTrackStat" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbTrackingStatus ID="cmbTrackingStatus" runat="server" Width="100%" />
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblObligation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbObligation ID="cmbObligation" runat="server" Width="100%" />
                    </td>
                    <td>
                        <asp:Label ID="lblMark" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbPCARepairMark ID="cmbMark" runat="server" Width="100%" />
                    </td>
                </tr>                                                    
                <tr>
                    <td>
                        <asp:Label ID="lblAction" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtAction" runat="server" style="width: 80%;" />
                    </td>
                    <td>
                        <asp:Label ID="lblDistribution" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbDistribution ID="cmbDistribution" runat="server" Width="100%" />
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblResponsibility" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbResponsibility ID="cmbResponsibility" runat="server" Width="100%" />
                    </td>
                    <td>
                        <asp:Label ID="lbl4M" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td>
                        <iMES:Cmb4M ID="cmb4M" runat="server" Width="100%" />
                    </td>
                </tr>   
                <tr>
                    <td>
                        <asp:Label ID="lblCover" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbCover ID="cmbCover" runat="server" Width="100%" />
                    </td>
                    <td>
                        <asp:Label ID="lblUncover" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbUncover ID="cmbUncover" runat="server" Width="100%" />
                    </td>
                </tr>     
                <tr>
                    <td>
                        <asp:Label ID="lblOldPart" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblOldPartValue" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNewPart" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNewPartValue" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>     
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <textarea id="tareaRemark" runat="server" rows="3" cols="4" style="width: 99%;" onkeyup="OnkeyupRemark()" ></textarea>
                    </td>
                </tr>                                                                                        
            </table>
        </div>
        <div id="btnArea" style="text-align: center; padding-bottom: 1px; margin-top:20">
            <button id="btnOK" runat="server" onclick="clkOK()" class="iMes_button" style="margin-left: 30px; margin-right: 30px;" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
            <button id="btnCancel" runat="server" onclick="clkCancel()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
            <button id="hidbtn" style="width: 0; display: none;" runat="server" ></button>
            <input type="hidden" id="hidCTNO" runat="server" />
        </div>        
    </div>
    </form>
    <script language="javascript" type="text/javascript">
        var parentWindow;
        var defectObj;
        var testStationObj;
        var subDefectObj;
        var causeObj;
        var majorPartObj;
        var componentObj;
        var trackingStatusObj;
        var obligationObj;
        var markObj;
        var distributionObj;
        var responsibilityObj;
        var M4MObj;
        var coverObj;
        var uncoverObj;

        var defectInfoID = 0;
        var productID = "";
        var editObj = new DefectInfo();
        
        var emptyPattern = /^\s*$/;
        var sitePattern = /^[0-9a-zA-Z]{0,4}$/;
        var editor;
        var customer;
        var stationId;
        var addedDefects = ";";     
        
        //error message
        var msgInputMajorPart = '<%=this.GetLocalResourceObject(Pre + "_msgInputMajorPart").ToString() %>';
        var msgObligationRequired = '<%=this.GetLocalResourceObject(Pre + "_msgObligationRequired").ToString() %>';

        window.onload = function() {
            parentWindow = window.dialogArguments;
            defectObj = getDefectCmbObj();
            testStationObj = getTestStationCmbObj();
            subDefectObj = getSubDefectCmbObj();
            causeObj = getCauseCmbObj();
            majorPartObj = getMajorPartCmbObj();
            componentObj = getComponentCmbObj();
            trackingStatusObj = getTrackingStatusCmbObj();
            obligationObj = getObligationCmbObj();
            markObj = getPCARepairMarkCmbObj();
            distributionObj = getDistributeCmbObj();
            responsibilityObj = getResponsibilityCmbObj();
            M4MObj = getM4MCmbObj();
            coverObj = getCoverCmbObj()
            uncoverObj = getUncoverCmbObj();

            editor = parentWindow.editor;
            customer = parentWindow.customer;
            stationId = parentWindow.stationId;

            initpage();

            setDefectInfo();
        };

        function initpage() {
            defectObj.selectedIndex = 0;
            testStationObj.selectedIndex = 0;
            subDefectObj.selectedIndex = 0;
            causeObj.selectedIndex = 0;
            majorPartObj.selectedIndex = 0;
            componentObj.selectedIndex = 0;
            document.getElementById("<%=txtSite.ClientID %>").value = "";
            trackingStatusObj.selectedIndex = 0;
            obligationObj.selectedIndex = 0;
            markObj.selectedIndex = 0;
            document.getElementById("<%=txtAction.ClientID %>").value = "";
            distributionObj.selectedIndex = 0;
            responsibilityObj.selectedIndex = 0;
            M4MObj.selectedIndex = 0;
            coverObj.selectedIndex = 0;
            uncoverObj.selectedIndex = 0;
            document.getElementById("<%=lblOldPartValue.ClientID %>").innerText = "";
            document.getElementById("<%=lblNewPartValue.ClientID %>").innerText = "";
            document.getElementById("<%=tareaRemark.ClientID %>").value = "";
        }
        
        function setDefectInfo()
        {
            arr = parentWindow.ValueTransToChild.split("+");
            defectInfoID = parseInt(arr[0]);
            productID = arr[1];
            PageMethods.getRepairInfo(defectInfoID, onGetRepairInfoSuccess, onGetRepairInfoFail);
        }
        
        function onGetRepairInfoSuccess(result) {
            editObj.id = result.id;
            editObj.repairID = result.repairID;
            editObj.type = result.type;
            editObj.defectCodeID = result.defectCodeID;
            editObj.cause = result.cause;            
            editObj.obligation = result.obligation;
            editObj.component = result.component;
            editObj.site = result.site;
            editObj.majorPart = result.majorPart;
            editObj.remark = result.remark;
            editObj.vendorCT = result.vendorCT;
            editObj.partType = result.partType;
            editObj.oldPart = result.oldPart;
            editObj.oldPartSno = result.oldPartSno;
            editObj.newPart = result.newPart;
            editObj.newPartSno = result.newPartSno;
            editObj.manufacture = result.manufacture;
            editObj.versionA = result.versionA;
            editObj.versionB = result.versionB;
            editObj.returnSign = result.returnSign;
            editObj.mark = result.mark;
            editObj.subDefect = result.subDefect;
            editObj.piaStation = result.piaStation;
            editObj.distribution = result.distribution;
            editObj._4M = result._4M;
            editObj.responsibility = result.responsibility;
            editObj.action = result.action;
            editObj.cover = result.cover;
            editObj.uncover = result.uncover;
            editObj.trackingStatus = result.trackingStatus;
            editObj.isManual = result.isManual;
            editObj.mtaID = result.mtaid;
            editObj.editor = result.editor;
            editObj.returnStation = result.returnStation;
            
            defectObj.value = editObj.defectCodeID;
            testStationObj.value = editObj.piaStation;
            subDefectObj.value = editObj.subDefect;
            causeObj.value = editObj.cause;
            majorPartObj.value = editObj.majorPart;
            componentObj.value = editObj.component;
            document.getElementById("<%=txtSite.ClientID %>").value = editObj.site;
            trackingStatusObj.value = editObj.trackingStatus;
            obligationObj.value = editObj.obligation;
            markObj.value = editObj.mark;
            document.getElementById("<%=txtAction.ClientID %>").value = editObj.action;
            distributionObj.value = editObj.distribution;
            responsibilityObj.value = editObj.responsibility;
            M4MObj.value = editObj._4M;
            coverObj.value = editObj.cover;
            uncoverObj.value = editObj.uncover;
            document.getElementById("<%=lblOldPartValue.ClientID %>").innerText = editObj.oldPartSno;
            document.getElementById("<%=lblNewPartValue.ClientID %>").innerText = editObj.newPartSno;
            document.getElementById("<%=tareaRemark.ClientID %>").value = editObj.remark;

            defectObj.disabled = true;
            causeObj.disabled = true;
        }

        function onGetRepairInfoFail(result) {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }
        
        function getDefect() {            
            editObj.Identity = defectInfoID;
            editObj.editor = editor;
            //editObj.defectCodeID = defectObj.value;
            editObj.piaStation = getTestStationCmbValue();
            editObj.subDefect = getSubDefectCmbValue();
            //editObj.cause = causeObj.value;
            editObj.majorPart = majorPartObj.value;
            editObj.component = componentObj.value;
            editObj.site = document.getElementById("<%=txtSite.ClientID %>").value.toUpperCase();
            editObj.trackingStatus = getTrackingStatusCmbValue();
            editObj.obligation = obligationObj.value;
            editObj.mark = markObj.value;
            editObj.action = document.getElementById("<%=txtAction.ClientID %>").value;
            editObj.distribution = distributionObj.value;
            editObj.responsibility = getResponsibilityValue();
            editObj._4M = getM4MValue();
            editObj.cover = getCoverCmbValue()
            editObj.uncover = getUncoverCmbValue();
            //editObj.oldPartSno = document.getElementById("<%=lblOldPartValue.ClientID %>").innerText;
            //editObj.newPartSno = document.getElementById("<%=lblNewPartValue.ClientID %>").innerText;
            editObj.remark = document.getElementById("<%=tareaRemark.ClientID %>").value; 
        }

        function clkOK()
        {
            if (editInputCheck()) {
                getDefect();
                PageMethods.setRepairInfo(productID, editObj, onSetRepairInfoSuccess, onSetRepairInfoFail);
            }
        }

        function onSetRepairInfoSuccess(result) {
            parentWindow.flagModify = true;
            window.close();
        }

        function onSetRepairInfoFail(result)
        {
            ShowMessage(result._message);
        }

        function editInputCheck() {

            var cause = causeObj.value;
            var obligation = obligationObj.value;

            if (cause.toUpperCase() != "WW" && emptyPattern.test(obligationObj.value)) {
                alert(msgObligationRequired);
                obligationObj.focus();
                return false;
            }

            if (cause.toUpperCase() != "WW" && cause.toUpperCase() != "CN") {
                if (emptyPattern.test(majorPartObj.value)) {
                    alert(msgInputMajorPart);
                    majorPartObj.focus();
                    return false;
                }
            }         
            
            return true;
        }

        function clkCancel() {
            parentWindow.flagModify = false;
            window.close();
        }
        
        function OnkeyupRemark()
        {
            var str = document.getElementById("<%=tareaRemark.ClientID%>").value;
            
            if ((str.length) > 100)
            {
                document.getElementById("<%=tareaRemark.ClientID%>").value = str.substring(0,100);
            }
        }        
    </script>
</body>  
</html>

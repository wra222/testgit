﻿

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FARepairAddEdit.aspx.cs" Inherits="FA_FARepairAddEdit" Title="iMES - Repair Log" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
</head>
<body style=" position: relative; width: 100%"> 
    <form id="form1" runat="server">     
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/FARepairService.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1" class="iMes_div_center">
            <table width="100%" style="table-layout:fixed;">
                <colgroup>
	                <col style="width:140px;" />
	                <col />
	                <col style="width:110px;" />
	                <col />
                </colgroup>
                <tr>
                    <td><asp:Label ID="lblDefect" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td colspan="3"><iMES:CmbDefect runat="server" ID="cmbDefect" Width="100%" /></td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblCause" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td colspan="3"><iMES:CmbCause ID="cmbCause" runat="server" Width="100%"/></td>
                </tr> 
                <tr>
                    <td><asp:Label ID="lblMajorPart" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td><iMES:CmbPMajorPart ID="cmbMajorPart" runat="server" Width="100%"/></td>
                    <td><asp:Label ID="lblComponent" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td><iMES:CmbComponent ID="cmbComponent" runat="server" Width="100%"/></td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblSite" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td><input type="text" id="txtSite" runat="server" maxlength="10" class="iMes_input_losercase" style="width: 80%;" /></td>
                    <td><asp:Label ID="lblPartType" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td><iMES:CmbPartType ID="cmbPartType" runat="server" Width="80%" Enabled="True" /></td>  
                    
                </tr>
                <tr>                    
                    <td><asp:Label ID="lblFaultyPartSno" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td><input type="text" id="txtFaultyPartSno" runat="server" maxlength="50" style="width: 80%;" class="iMes_input_White"/></td>
                    <td><asp:Label ID="lblNewPartSno" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td><input type="text" id="txtNewPartSno" runat="server" class="iMes_input_White" maxlength="50" style="width: 80%;"/></td>
                </tr>                
                <tr>
                    <td><asp:Label ID="lblMac" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td><input type="text" id="txtMac" runat="server" class="iMes_input_White" maxlength="30" style="width: 80%;" readonly="readonly" /></td>
                    <td><asp:Label ID="lblStation" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td>
                    <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>                    
                                <input type="text" id="txtStation" runat="server" class="iMes_input_White" maxlength="50" style="width: 80%;" readonly="readonly" />
                            </ContentTemplate>
                    </asp:UpdatePanel>
                    
                    </td>
                </tr>                                
                <tr>
                    <td><asp:Label ID="lblObligation" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td><iMES:CmbObligation ID="cmbObligation" runat="server" Width="100%" /></td>                    
                    <td><asp:Label ID="lblMark" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td><iMES:CmbMark ID="cmbMark" runat="server" Width="80%" /></td>
                </tr>
                <tr>
                    <td valign="top"><asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td colspan="3">
                        <textarea id="tareaRemark" runat="server" rows="3" cols="4" style="width: 99%;" onkeyup="OnkeyupRemark()"></textarea>
                    </td>
                </tr>                                                                                                                                                                                 
            </table>
        </div>
        <div id="btnArea" style="text-align: center; padding-bottom: 1px;">
            <button id="btnAdd" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
            <button id="btnOK" runat="server" class="iMes_button" style="margin-left: 30px; margin-right: 30px;" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
            <button id="btnCancel" runat="server" onclick="clkCancel()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
            
        
            <button id="hidbtn" style="width: 0; display: none;" runat="server" ></button>
            <input type="hidden" id="hidMBSN" runat="server" />
            <input type="hidden" id="hidPdLine" runat="server" />
            <input type="hidden" id="hidProdId" runat="server" />
        </div>           
        <div style="display:none">        
            <input type="text" id="lblListDefect" runat="server"/>            
        </div>
    </div>
    </form>
    <script language="javascript" type="text/javascript">
        var parentWindow;
        var causeObj;
        var componentObj;
        var defectObj;
        var obligationObj;
        var mainPartObj;
        var emptyPattern = /^\s*$/;
        
        var markObj;
        var partTypeObj;
        var editor;
        var customer;
        var stationId;
        var macValue;           
        
        //error message
        var msgInputDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputDefect").ToString() %>';
        var msgInputCause = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputCause").ToString() %>';
        var msgInputMajorPart = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMajorPart").ToString() %>';
        var msgInputObligation = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputObligation").ToString() %>';
        var msgInputComponent = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputComponent").ToString() %>';
        var msgInputSite = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputSite").ToString() %>';
        var msgInputFaultyPartSno = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputFaultyPartSno").ToString() %>';
        var msgInputNewPartSno = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputNewPartSno").ToString() %>';
        var msgInputFaultyPartSn = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputFaultyPartSn").ToString() %>';
        var msgInputNewPartSn = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputNewPartSn").ToString() %>';
        var msgInputStation = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputStation").ToString() %>';
        var msgInputMark = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMark").ToString() %>';
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgDuplicateMBType = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateMBType").ToString() %>';
        var msgInputMac = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMac").ToString() %>';
        var msgSite = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSite").ToString() %>';
        var msgMajorPart = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMajorPart").ToString() %>';
        var msgMBSN = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMBSN").ToString() %>';
        var msgInputOldAndNew = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputOldAndNew").ToString() %>';
        
        //confirm info
        var msgAddAnotherDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAddAnotherDefect").ToString() %>';

        window.onload = function() {

            parentWindow = window.dialogArguments;
            causeObj = getCauseCmbObj();
            //6.13
            causeObj.onchange = causeChangeEvent;
            componentObj = getComponentCmbObj();
            defectObj = getDefectCmbObj();
            obligationObj = getObligationCmbObj();
            mainPartObj = getMajorPartCmbObj();
            
            markObj = getMarkCmbObj();
            //HideInfoText();
            partTypeObj = getPartTypeCmbObj();
            partTypeObj.onchange = partTypeChangeEvent;

            editor = parentWindow.editor;
            customer = parentWindow.customer;
            stationId = parentWindow.stationId;
            document.getElementById("<%=hidProdId.ClientID %>").value = parentWindow.globalProID;

            //if the status is 'Edit', disable the Add button.
            //register edit click event.
            if (parentWindow.status == "E") {
                document.getElementById("<%=btnOK.ClientID %>").onclick = editOkClick;
                document.getElementById("<%=btnAdd.ClientID %>").disabled = true;

                if (!isThisStationAdd()) {
                    //defectObj.disabled = true;
                }

                setDefectInfo();
            }
            else {
                document.getElementById("<%=txtFaultyPartSno.ClientID %>").value = "";
                document.getElementById("<%=txtNewPartSno.ClientID %>").value = "";
                document.getElementById("<%=txtFaultyPartSno.ClientID %>").disabled = true;
                document.getElementById("<%=txtNewPartSno.ClientID %>").disabled = true;
                document.getElementById("<%=btnOK.ClientID %>").onclick = addOkClick;
                document.getElementById("<%=btnAdd.ClientID %>").onclick = addAddClick;
            }
        };
        function setDefectInfo() {
            var editObj = parentWindow.editObj;

            defectObj.value = editObj.defectCodeID;
            causeObj.value = editObj.cause;
            mainPartObj.value = editObj.majorPart;
            componentObj.value = editObj.component;
            document.getElementById("<%=txtSite.ClientID %>").value = editObj.site;
            markObj.value = editObj.mark;
            document.getElementById("<%=txtFaultyPartSno.ClientID %>").value = editObj.oldPartSno;
            document.getElementById("<%=txtNewPartSno.ClientID %>").value = editObj.newPartSno;
            obligationObj.value = editObj.obligation;
            document.getElementById("<%=tareaRemark.ClientID %>").value = editObj.remark;
            partTypeObj.value = editObj.partType;

            
            
            if (!emptyPattern.test(editObj.newPartSno)) {
                document.getElementById("<%=txtFaultyPartSno.ClientID %>").disabled = false;
                document.getElementById("<%=txtNewPartSno.ClientID %>").disabled = true;            
            }
            else {
                document.getElementById("<%=txtFaultyPartSno.ClientID %>").disabled = false;
                document.getElementById("<%=txtNewPartSno.ClientID %>").disabled = false;
            }

            if (partTypeObj.value.toUpperCase() == "") {
                document.getElementById("<%=txtFaultyPartSno.ClientID %>").disabled = true;
                document.getElementById("<%=txtNewPartSno.ClientID %>").disabled = true;
            }
            else {
                document.getElementById("<%=txtFaultyPartSno.ClientID %>").disabled = false;
                document.getElementById("<%=txtNewPartSno.ClientID %>").disabled = false;
            }
            //document.getElementById("<%=txtMac.ClientID %>").value = editObj.mac;
            document.getElementById("<%=txtMac.ClientID %>").value = parentWindow.mac_value;
            document.getElementById("<%=txtStation.ClientID %>").value = editObj.returnStation;
        }            
        
        function partTypeChangeEvent()
        {
            var type = partTypeObj.value;
            
            if (type.toUpperCase() == "") {
                document.getElementById("<%=txtFaultyPartSno.ClientID %>").value = "";
                document.getElementById("<%=txtNewPartSno.ClientID %>").value = "";
                document.getElementById("<%=txtFaultyPartSno.ClientID %>").disabled = true;
                document.getElementById("<%=txtNewPartSno.ClientID %>").disabled = true;                
            }
            else {
                document.getElementById("<%=txtFaultyPartSno.ClientID %>").disabled = false;
                document.getElementById("<%=txtNewPartSno.ClientID %>").disabled = false;                
            }
        }

        //6.13
        function causeChangeEvent() {
            var cause = causeObj.value;

            if (cause.toUpperCase().substring(0, 2) == "WW" ||
                cause.toUpperCase().substring(0, 2) == "CN") {
                document.getElementById("<%=txtSite.ClientID %>").value = "";
                componentObj.value = "";
                document.getElementById("<%=tareaRemark.ClientID %>").value = "";
                getMajorPartCmbObj().value = "";

                document.getElementById("<%=txtSite.ClientID %>").disabled = true;
                componentObj.disabled = true;
                document.getElementById("<%=tareaRemark.ClientID %>").disabled = true;
                getMajorPartCmbObj().disabled = true;
            }
            else {
                document.getElementById("<%=txtSite.ClientID %>").disabled = false;
                componentObj.disabled = false;
                document.getElementById("<%=tareaRemark.ClientID %>").disabled = false;
                getMajorPartCmbObj().disabled = false;
            }
        }
        
        function addOkClick()
        {
            if (editInputCheck()) {
                macValue = document.getElementById("<%=txtMac.ClientID %>").value;
                FARepairService.add(parentWindow.globalProID, macValue, getDefect(parentWindow.status), editor, stationId, customer , addOkSuccess, addOkFail); 
            }        
        }        
        
        function addOkSuccess(result)
        {   
            window.close();
        }        
        
        function addOkFail(result)
        {            
            ShowMessage(result._message);           
        }       
        
        function addAddClick()
        {
            if (editInputCheck()) {
                macValue = document.getElementById("<%=txtMac.ClientID %>").value;
                FARepairService.add(parentWindow.globalProID, macValue, getDefect(parentWindow.status), editor, stationId, customer, addAddSuccess, addOkFail);
            }         
        }
        
        function addAddSuccess(result)
        {
            if (confirm(msgAddAnotherDefect))
            {
                //Dean 20110526 AddDefect 時要先檢查所hide的值，因為母頁不會刷新，要避免重複Defect
                if (parentWindow.status == "A") {
                    document.getElementById("<%=lblListDefect.ClientID %>").value += getDefectCmbValue() + ";";
                }
                //Dean 20110526 AddDefect 時要先檢查所hide的值，因為母頁不會刷新，要避免重複Defect
                initPage();                                                
            }
            else
            {
                window.close();
            }
        }                 
        
        function editOkClick()
        {
            if (editInputCheck()) {
                macValue = document.getElementById("<%=txtMac.ClientID %>").value;
                FARepairService.update(parentWindow.globalProID, macValue, getDefect(parentWindow.status), editor, stationId, customer, editSuccess, editFail);
            }
        }
        
       
        
        function getDefect(type)
        {
            var editObj = null;
        
            if (type == "A")
            {
                editObj = new DefectInfo();
                editObj.editor = editor;
                editObj.isManual = "1";                
            }
            else
            {
                editObj = parentWindow.editObj;   
                editObj.editor = editor;        
            }

            editObj.defectCodeID = getDefectCmbValue();
            editObj.cause = causeObj.value;
            editObj.majorPart = mainPartObj.value;
            editObj.component = componentObj.value;
            editObj.site = document.getElementById("<%=txtSite.ClientID %>").value;
            editObj.mark = markObj.value;
            //editObj.oldPartSno = (partTypeObj.value == "MB" ? SubStringSN(document.getElementById("<%=txtFaultyPartSno.ClientID %>").value.trim().toUpperCase(), "MB") : document.getElementById("<%=txtFaultyPartSno.ClientID %>").value.trim().toUpperCase());
            //editObj.newPartSno = (partTypeObj.value == "MB" ? SubStringSN(document.getElementById("<%=txtNewPartSno.ClientID %>").value.trim().toUpperCase(), "MB") : document.getElementById("<%=txtNewPartSno.ClientID %>").value.trim().toUpperCase());
            editObj.oldPartSno = (partTypeObj.value == "MB" ? checkMBSN(document.getElementById("<%=txtFaultyPartSno.ClientID %>").value.trim().toUpperCase()) : document.getElementById("<%=txtFaultyPartSno.ClientID %>").value.trim().toUpperCase());
            editObj.newPartSno = (partTypeObj.value == "MB" ? checkMBSN(document.getElementById("<%=txtNewPartSno.ClientID %>").value.trim().toUpperCase()) : document.getElementById("<%=txtNewPartSno.ClientID %>").value.trim().toUpperCase());           
            
            editObj.obligation = obligationObj.value;
            editObj.remark = document.getElementById("<%=tareaRemark.ClientID %>").value;
            editObj.partType = partTypeObj.value;
            editObj.returnStation = document.getElementById("<%=txtStation.ClientID %>").value;           
            
            return editObj;
        }

        function NumberAndEnglishChar(data) {
            var inputContent = data;
            var pattern = /^[0-9A-Z]*$/;

            if (pattern.test(inputContent)) {
                return true;
            } else {
                return false;
            }
        }

        function checkMBSN(sn) {
            var ret = "";
            if (sn.length == 10) {
                if (sn.charAt(4) == "B") {
                    return sn;
                }
                else if (sn.charAt(4) == "M") {
                    return sn;
                }
            }
            else if (sn.length == 11) {
                if (sn.charAt(4) == "B") {
                    ret = sn.substring(0, 10);
                    return ret;
                }
                else if (sn.charAt(5) == "M") {
                    return sn;
                }
            }
            return ret;
        }


        function editInputCheck() {
        
            var defectValue = getDefectCmbValue();
            if (emptyPattern.test(defectValue)) {
                alert(msgInputDefect);
                setDefectCmbFocus();
                return false;
            }
            
            if (emptyPattern.test(getCauseCmbValue()))
            {
                alert(msgInputCause);
                setCauseCmbFocus();
                return false;
            }
            if (getCauseCmbValue().toUpperCase() == "WW" && emptyPattern.test(getObligationCmbValue()))
            {
                alert(msgInputObligation);
                obligationObj.focus();
                return false;
            }
            //6.13
            var cause = causeObj.value;

            if (cause.toUpperCase().substring(0, 2) != "WW" &&
                cause.toUpperCase().substring(0, 2) != "CN") {
                if (getMajorPartCmbValue() == "") {
                    alert(msgMajorPart);
                    return false;
                }
            }

            var site = document.getElementById("<%=txtSite.ClientID %>").value;
            if (site != "") {
                if (!NumberAndEnglishChar(site)) {
                    alert(msgSite);
                    return false;
                }
            }

            var objId = (parentWindow.status == "E" ? parentWindow.editObj.id : "");
            //AddDefect 時要先檢查所hide的值，因為母頁不會刷新，要避免重複Defect
            if (parentWindow.status == "A") {
                if (parentWindow.isExistDefectInTable(defectValue, objId)) {
                    alert(msgDuplicateData);
                    setDefectCmbFocus();
                    return false;                    
                }
                if (document.getElementById("<%=lblListDefect.ClientID %>").value != "") {
                    var strArry = new Array();
                    strArry = document.getElementById("<%=lblListDefect.ClientID %>").value.split(";");
                    for (var i = 0; i < strArry.length; i++) {
                        if (strArry[i] == defectValue) {
                            alert(msgDuplicateData);
                            setDefectCmbFocus();
                            return false;
                        }
                    }
                }
            }                        
            if (parentWindow.status == "E") {
                if (parentWindow.editObj.isManual == "1") {
                    if (parentWindow.isExistDefectInTable(defectValue, objId)) {
                        alert(msgDuplicateData);
                        setDefectCmbFocus();                        
                        return false;
                    }
                }
            }

            var returnStation = document.getElementById("<%=txtStation.ClientID %>").value;
            if (emptyPattern.test(returnStation)) {

            }

            var fpsno = document.getElementById("<%=txtFaultyPartSno.ClientID %>").value.trim().toUpperCase();
            var newFpsno = document.getElementById("<%=txtNewPartSno.ClientID %>").value.trim().toUpperCase();
            //若New Part Sno不为空，则Faulty Part Sno必须输入
            if (!emptyPattern.test(newFpsno) && emptyPattern.test(fpsno)) {
                alert(msgInputFaultyPartSno);
                document.getElementById("<%=txtFaultyPartSno.ClientID %>").focus();
                return false;
            }

            //mantis 1476
            if (partTypeObj.value == "MB") {
                if (emptyPattern.test(fpsno)) {
                    alert(msgInputOldAndNew);
                    document.getElementById("<%=txtFaultyPartSno.ClientID %>").focus();
                    return false;
                }
                else if (emptyPattern.test(newFpsno)) {
                    alert(msgInputOldAndNew);
                    document.getElementById("<%=txtNewPartSno.ClientID %>").focus();
                    return false;
                }
                else {
                }
            }


            //MBSN
            if (partTypeObj.value == "MB" && fpsno != "") {
                if (checkMBSN(fpsno) == "") {
                    alert(msgMBSN);
                    return false;
                }
            }
            if (partTypeObj.value == "MB" && newFpsno != "") {
                if (checkMBSN(newFpsno) == "") {
                    alert(msgMBSN);
                    return false;
                }
            }

            //if (getPartTypeCmbValue() == "MB" && document.getElementById("<%=txtMac.ClientID %>").value == "") {
            //    alert(msgInputMac);
            //    document.getElementById("<%=txtMac.ClientID %>").focus();
            //    return false;
            //}
                      
            return true;
        }        
        
        function editSuccess(result)
        {
            //maybe it should show a message?
            window.close();
        }
        
        function editFail(result)
        {
            ShowMessage(result._message);
        }        
        
        function isThisStationAdd()
        {
            //-----------------------------------------------------add some logic to get flag
            if (parentWindow.editObj.isManual == "1")
            {
                return true;
            }
            
            return false;
        }
        
        function initPage()
        {
            defectObj.selectedIndex = -1;
            causeObj.selectedIndex = -1;
            mainPartObj.selectedIndex = -1;
            componentObj.selectedIndex = -1;
            document.getElementById("<%=txtSite.ClientID %>").value = "";
            markObj.selectedIndex = -1;
            document.getElementById("<%=txtFaultyPartSno.ClientID %>").value = "";
            document.getElementById("<%=txtNewPartSno.ClientID %>").value = "";
            document.getElementById("<%=tareaRemark.ClientID %>").value = "";
            obligationObj.selectedIndex = -1;
            partTypeObj.selectedIndex = -1;
            document.getElementById("<%=txtFaultyPartSno.ClientID %>").disabled = true;
            document.getElementById("<%=txtNewPartSno.ClientID %>").disabled = true;
        }

        function clkCancel() {       
            window.close();
            return;
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


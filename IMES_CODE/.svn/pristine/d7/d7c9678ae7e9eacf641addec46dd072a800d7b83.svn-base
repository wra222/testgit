<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for KP Repair Page
 *             
 * UI:CI-MES12-SPEC-FA-UI KeyParts Repair.docx
 * UC:CI-MES12-SPEC-FA-UC KeyParts Repair.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-26  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="KPRepairLogAddEdit.aspx.cs" Inherits="FA_KPRepairLogAddEdit" Title="iMES - KP Repair Log" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
</head>
<body style=" position: relative; width: 100%; text-align:center"> 
    <form id="form1" runat="server"> 
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/KPRepairService.asmx" />
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
                    <td colspan="3">
                        <iMES:CmbPMajorPart ID="cmbMajorPart" runat="server" Width="100%" />
                    </td>
                </tr> 
                <tr>
                    <td>
                        <asp:Label ID="lblComponent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
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
                        <asp:Label ID="lblMark" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbPCARepairMark ID="cmbMark" runat="server" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblObligation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbObligation ID="cmbObligation" runat="server" Width="100%" />
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
            <button id="btnAdd" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
            <button id="btnOK" runat="server" class="iMes_button" style="margin-left: 30px; margin-right: 30px;" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
            <button id="btnCancel" runat="server" onclick="clkCancel()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
            <button id="hidbtn" style="width: 0; display: none;" runat="server" ></button>
            <input type="hidden" id="hidCTNO" runat="server" />
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
        /*
        * Answer to: ITC-1360-1782
        * Description: UC updated(see below contents).
        */
        //(5/1)去除维修时Site必须输入的限制，允许输入0/1/2/3/4位
        var sitePattern = /^[0-9a-zA-Z]{0,4}$/;
        var testStationObj;
        var markObj;
        var editor;
        var customer;
        var stationId;
        var addedDefects = ";";     
        
        //error message
        var msgInputDefect = '<%=this.GetLocalResourceObject(Pre + "_msgInputDefect").ToString() %>';
        var msgInputCause = '<%=this.GetLocalResourceObject(Pre + "_msgInputCause").ToString() %>';
        var msgInputMajorPart = '<%=this.GetLocalResourceObject(Pre + "_msgInputMajorPart").ToString() %>';
        var msgInputObligation = '<%=this.GetLocalResourceObject(Pre + "_msgInputObligation").ToString() %>';
        var msgInputComponent = '<%=this.GetLocalResourceObject(Pre + "_msgInputComponent").ToString() %>';
        var msgInputSite = '<%=this.GetLocalResourceObject(Pre + "_msgInputSite").ToString() %>';
        var msgInputMark = '<%=this.GetLocalResourceObject(Pre + "_msgInputMark").ToString() %>';
        var msgDuplicateData = '<%=this.GetLocalResourceObject(Pre + "_msgDuplicateData").ToString() %>';
        
        //confirm info
        var msgAddAnotherDefect = '<%=this.GetLocalResourceObject(Pre + "_msgAddAnotherDefect").ToString() %>';
        var msgFixTimes = '<%=this.GetLocalResourceObject(Pre + "_msgFixTimes").ToString() %>';

        window.onload = function() {
            parentWindow = window.dialogArguments;
            causeObj = getCauseCmbObj();
            causeObj.onchange = causeChangeEvent;
            componentObj = getComponentCmbObj();
            defectObj = getDefectCmbObj();
            obligationObj = getObligationCmbObj();
            mainPartObj = getMajorPartCmbObj();
            markObj = getPCARepairMarkCmbObj();
            editor = parentWindow.editor;
            customer = parentWindow.customer;
            stationId = parentWindow.stationId;
            //if the status is 'Edit', disable the Add button.
            //register edit click event.
            if (parentWindow.status == "E") {
                document.getElementById("<%=btnOK.ClientID %>").onclick = editOkClick;
                document.getElementById("<%=btnAdd.ClientID %>").disabled = true;

                if (!isThisStationAdd()) {
                    defectObj.disabled = true;
                    causeObj.focus();
                }

                setDefectInfo();
                causeChangeEvent();
            }
            else {
                document.getElementById("<%=btnOK.ClientID %>").onclick = addOkClick;
                document.getElementById("<%=btnAdd.ClientID %>").onclick = addAddClick;
            }
        };
        
        function causeChangeEvent()
        {
            var selValue = getCauseCmbValue();

            if (selValue.toUpperCase() == "WW" || selValue.toUpperCase() == "CN") {
            componentObj.selectedIndex = -1;
            componentObj.disabled = true;
            document.getElementById("<%=txtSite.ClientID %>").value = "";
            document.getElementById("<%=txtSite.ClientID %>").disabled = true;
            //document.getElementById("<%=tareaRemark.ClientID %>").value = "";
            //document.getElementById("<%=tareaRemark.ClientID %>").disabled = true;
            }
            else {
            componentObj.disabled = false;
            document.getElementById("<%=txtSite.ClientID %>").disabled = false;
            //document.getElementById("<%=tareaRemark.ClientID %>").disabled = false;
            }
        }
        
        function addOkClick()
        {
            if (editInputCheck())
            {
                KPRepairService.add(parentWindow.globalCTNO, getDefect(parentWindow.status), addOkSuccess, addOkFail);
            }        
        }
        
        function addOkSuccess(result)
        {
            fixCnt = parseInt(result);	
            if (fixCnt > 0) {
                alert(msgFixTimes.replace("XXX", result));
            }

            if (confirm(msgAddAnotherDefect)) {
                addedDefects += defectObj.value + ";"
                initPage();
            }
            else {
                window.close();
            }
        }        
        
        function addOkFail(result)
        {
            ShowMessage(result._message);           
        }
        
        function addAddClick()
        {
            if (editInputCheck())
            {
                KPRepairService.add(parentWindow.globalCTNO, getDefect(parentWindow.status), addAddSuccess, addOkFail);
            }         
        }

        function addAddSuccess(result) 
        {
            fixCnt = parseInt(result);
            if (fixCnt > 0) {
                alert(msgFixTimes.replace("XXX", result));
            }
            addedDefects += defectObj.value + ";"
            initPage();
        }        
        
        function setDefectInfo()
        {
            var editObj = parentWindow.editObj;
            
            defectObj.value = editObj.defectCodeID;
            causeObj.value = editObj.cause;
            mainPartObj.value = editObj.majorPart;
            componentObj.value = editObj.component;
            document.getElementById("<%=txtSite.ClientID %>").value = editObj.site;
            markObj.value = editObj.mark;
            
            obligationObj.value = editObj.obligation;
            document.getElementById("<%=tareaRemark.ClientID %>").value = editObj.remark;
        }
        
        function initPage()
        {
            //testStationObj.selectedIndex = -1;
            defectObj.selectedIndex = -1;
            causeObj.selectedIndex = -1;
            mainPartObj.selectedIndex = -1;
            componentObj.selectedIndex = -1;
            document.getElementById("<%=txtSite.ClientID %>").value = "";            
            markObj.value = 0;
            document.getElementById("<%=tareaRemark.ClientID %>").value = "";
            obligationObj.selectedIndex = -1;
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
            }

            editObj.defectCodeID = defectObj.value;
            editObj.cause = causeObj.value;
            editObj.majorPart = mainPartObj.value;
            editObj.component = componentObj.value;
            editObj.site = document.getElementById("<%=txtSite.ClientID %>").value.toUpperCase();
            editObj.mark = markObj.value;
            editObj.obligation = obligationObj.value;
            editObj.remark = document.getElementById("<%=tareaRemark.ClientID %>").value;
            
            return editObj;      
        }
        
        function editOkClick()
        {
            if (editInputCheck())
            {
                KPRepairService.update(parentWindow.globalCTNO, getDefect(parentWindow.status), editSuccess, editFail);
            }
        }

        function editSuccess(result) 
        {
            fixCnt = parseInt(result);
            if (fixCnt > 0) {
                alert(msgFixTimes.replace("XXX", result));
            }
            
            window.close();
        }
        
        function editFail(result)
        {
            ShowMessage(result._message);
        }

        function editInputCheck() {
            if (!defectObj.disabled && emptyPattern.test(defectObj.value))
            {
                alert(msgInputDefect);
                defectObj.focus();
                return false;
            }
            
            if (emptyPattern.test(causeObj.value))
            {
                alert(msgInputCause);
                causeObj.focus();
                return false;
            }
            
            /*
            if (emptyPattern.test(markObj.value))
            {
                alert(msgInputMark);
                markObj.focus();
                return false;                 
            }
            */
            
            /*
            if (emptyPattern.test(obligationObj.value))
            {
                alert(msgInputObligation);
                obligationObj.focus();
                return false;                  
            }
            */
            
            var selValue = getCauseCmbValue();
            var selectText = getCauseCmbText();
            
            if (selValue.toUpperCase() != "WW" && selValue.toUpperCase() != "CN")
            {
                if (emptyPattern.test(mainPartObj.value)) 
                {
                    alert(msgInputMajorPart);
                    mainPartObj.focus();
                    return false;
                }
                
                /*
                if (emptyPattern.test(componentObj.value))
                {
                    alert(msgInputComponent);
                    componentObj.focus();
                    return false;     
                }
                */

                if (!sitePattern.test(document.getElementById("<%=txtSite.ClientID %>").value))
                {
                    alert(msgInputSite);
                    document.getElementById("<%=txtSite.ClientID %>").select();
                    return false;                     
                }
            }
            
            var objId = (parentWindow.status == "E" ? parentWindow.editObj.id : "");

            if (parentWindow.isExistDefectInTable(defectObj.value, objId) || addedDefects.indexOf(";" + defectObj.value + ";") >= 0)
            {
                alert(msgDuplicateData);
                defectObj.focus();
                return false;
            }             
            
            return true;
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
        
        function clkCancel()
        {
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

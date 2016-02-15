<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for QC Repair Add/Edit Page
 * UI:CI-MES12-SPEC-FA-UI QC Repair.docx –2011/10/19 
 * UC:CI-MES12-SPEC-FA-UC QC Repair.docx –2011/10/19            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-14   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0463, Jessica Liu, 2012-2-16
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PAQCRepairAddEdit.aspx.cs" Inherits="FA_PAQCRepairAddEdit" Title="iMES - OQC Repair Log" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
</head>
<body style=" position: relative; width: 100%"> 
    <form id="form1" runat="server">   
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/FA/Service/PAQCRepairService.asmx" />
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
                    <td>
                        <asp:Label ID="lblDefect" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbDefect ID="CmbDefect" runat="server" Width="92%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTestStation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbReturnStation ID="CmbTestStation" runat="server" Width="80%" Type="PiaWc" />
                    </td>
                    <td>
                        <asp:Label ID="lblSubDefect" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbSubDefect ID="CmbSubDefect" runat="server" Width="80%"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCause" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbCause ID="CmbCause" runat="server" Width="92%" Stage="FA"/>
                    </td>
                </tr>                
                <tr>
                    <td>
                        <asp:Label ID="lblMajorPart" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbPMajorPart ID="CmbPMajorPart" runat="server" Width="80%"/>
                    </td>
                    <td>
                        <asp:Label ID="lblComponent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbComponent ID="CmbComponent" runat="server" Width="80%"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSite" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtSite" runat="server" maxlength="10" class="iMes_input_losercase" style="width: 80%;"/>
                    </td>
                    <%-- 2012-1-11，根据UC删除
                    <td>
                        <asp:Label ID="lblPartType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbPartType ID="CmbPartType" runat="server" Width="80%"  Enabled="false"/>
                    </td>
                    --%>
                    <%-- ITC-1360-0463, Jessica Liu, 2012-2-16--%>
                    <td>
                        <asp:Label ID="lblTrackingStatus" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbTrackingStatus ID="CmbTrackingStatus" runat="server" Width="80%"/>
                    </td>
                </tr>
                <%-- 2012-1-11，根据UC删除
                <tr>
                    <td>
                        <asp:Label ID="lblFaultyPartSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtFaultyPartSN" runat="server" class="iMes_input_White" 
                            maxlength="30" style="width: 80%;" disabled="disabled"  readonly="readonly"/>
                    </td>
                    <td>
                        <asp:Label ID="lblFaultyPartSno" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtFaultyPartSno" runat="server" maxlength="50" 
                            style="width: 80%;" class="iMes_input_White" disabled="disabled" readonly="readonly"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNewPartSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtNewPartSN" runat="server" class="iMes_input_White" 
                            maxlength="30" style="width: 80%;" disabled="disabled" readonly="readonly"/>
                    </td>
                    <td>
                        <asp:Label ID="lblNewPartSno" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtNewPartSno" runat="server" class="iMes_input_White" 
                            maxlength="50" style="width: 80%;" disabled="disabled" readonly="readonly"/>
                    </td>                    
                </tr>
                --%>
                <%-- ITC-1360-0463, Jessica Liu, 2012-2-16
                <tr>
                    <td>
                        <asp:Label ID="lblTrackingStatus" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbTrackingStatus ID="CmbTrackingStatus" runat="server" Width="80%"/>
                    </td>
                    <td>
                        <asp:Label ID="lblMAC" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtMAC" runat="server" class="iMes_input_White" 
                            maxlength="50" style="width: 80%;"/>
                    </td>                              
                </tr>
                --%> 
                <tr>
                    <td>
                        <asp:Label ID="lblObligation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbObligation ID="CmbObligation" runat="server" Width="80%"/>
                    </td> 
                    <td>
                        <asp:Label ID="lblMark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbMark ID="CmbMark" runat="server" Width="80%"/>
                    </td>                   
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblAction" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtAction" runat="server" style="width: 80%;" maxlength="50" class="iMes_input_losercase"/>
                    </td>
                    <td>
                        <asp:Label ID="lblDistribution" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbDistribution ID="CmbDistribution" runat="server" Width="80%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblResponsibility" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbResponsibility ID="CmbResponsibility" runat="server" Width="80%"/>
                    </td>
                    <td>
                        <asp:Label ID="lbl4M" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:Cmb4M ID="Cmb4M" runat="server" Width="80%"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCover" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbCover ID="CmbCover" runat="server" Width="80%"/>
                    </td>
                    <td>
                        <asp:Label ID="lblUncover" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMES:CmbUncover ID="CmbUncover" runat="server" Width="80%"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <textarea id="txtAreaRemark" cols="3" rows="4" style="width: 92%;" onkeyup="OnkeyupRemark()" runat="server"></textarea>
                    </td>
                </tr>
            </table>
        </div>
        <div id="btnArea" style="text-align: center; padding-bottom: 1px;">
            <button id="btnAdd" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
            <button id="btnOK" runat="server" style="margin-left: 30px; margin-right: 30px;" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
            <button id="btnCancel" runat="server" onclick="clkCancel()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
            <button id="hidbtn" style="width: 0; display: none;" runat="server" ></button>
            <input type="hidden" id="hidMBSN" runat="server" />
			<input type="hidden" id="hidPdLine" runat="server" />
        </div>
        <div style="display:none">
            <input type="text" id="lblListDefect" runat="server"/>            
        </div>      
    </div> 
    </form>    
    <script language="javascript" type="text/javascript">
        var parentWindow;
        var emptyPattern = /^\s*$/;
        
        var defectObj;
        var piaTestStationObj;
        var causeObj;
        var mainPartObj;
        var trackingStatusObj;
        var obligationObj;
        var responsibilityObj;
        var coverObj;
        var subDefectObj;
        var componentObj;
        //var partTypeObj;
        var markObj;
        var distributionObj;
        var _4mObj;
        var uncoverObj;
        
        var editor;
        var customer;
        var stationId;
        
        var msgAddAnotherDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAddAnotherDefect").ToString() %>';
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
        var msgInputMAC = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMAC").ToString() %>';;
        
        window.onload = function() 
        {
            //HideInfoText();
            parentWindow = window.dialogArguments;
            
            defectObj = getDefectCmbObj();
            piaTestStationObj = getReturnStationCmbObj();
            causeObj = getCauseCmbObj();
            causeObj.onchange = function() {};
            mainPartObj = getMajorPartCmbObj();
            trackingStatusObj = getTrackingStatusCmbObj();
            trackingStatusObj.onchange = function() {};
            obligationObj = getObligationCmbObj();
            responsibilityObj = getResponsibilityCmbObj();
            responsibilityObj.onchange = function() {};
            coverObj = getCoverCmbObj();
            coverObj.onchange = function() {};
            subDefectObj = getSubDefectCmbObj();
            subDefectObj.onchange = function() {};
            componentObj = getComponentCmbObj();
            //partTypeObj = getPartTypeCmbObj();
            //partTypeObj.onchange = partTypeChangeEvent;
            markObj = getMarkCmbObj();
            distributionObj = getDistributeCmbObj();
            _4mObj = getM4MCmbObj();
            _4mObj.onchange = function() {};
            uncoverObj = getUncoverCmbObj();
            uncoverObj.onchange = function() {};
            editor = parentWindow.editor;
            customer = parentWindow.customer;
            stationId = parentWindow.stationId;
            
            if (parentWindow.status == "E")
            {
                document.getElementById("<%=btnOK.ClientID %>").onclick = editOkClick;
                document.getElementById("<%=btnAdd.ClientID %>").disabled = true;
                
                if (!isThisStationAdd())
                {
                    defectObj.disabled = true;
                }
                
                setDefectInfo();
            }
            else
            {
                selectEmpty();                              
                document.getElementById("<%=btnOK.ClientID %>").onclick = addOkClick;
                document.getElementById("<%=btnAdd.ClientID %>").onclick = addAddClick;
            }            
        };
                
        function selectOtherType()
        {
                  
        }
        
        function selectEmpty()
        {
            
        }
        
        function selectOther()
        {
            
        }            
                
        function partTypeChangeEvent()
        {
            //Dean 20110316
            var type = partTypeObj.value;
            
            if (type.toUpperCase() == "OTHER TYPE")
            {
                selectOtherType();
            }
            else if (type.toUpperCase() == "")
            {
                selectEmpty();
            }
            else
            {
                selectOther();                 
            }
        }
        
        function addOkClick()
        {
            if (editInputCheck())
            {
                PAQCRepairService.add(parentWindow.globalProID, getDefect(parentWindow.status), editor, stationId, customer , addOkSuccess, addOkFail); 
            }        
        }
        
        function addOkSuccess(result)
        {   
            window.close();
        }
        
        function addOkFail(result)
        {
            //show error message
            ShowMessage(result._message);
            //ShowInfo(result._message);            
        }
        
        function addAddClick()
        {
            if (editInputCheck())
            {
                PAQCRepairService.add(parentWindow.globalProID, getDefect(parentWindow.status), editor, stationId, customer, addAddSuccess, addOkFail);
            }         
        }
        
        function addAddSuccess(result)
        {
            if (confirm(msgAddAnotherDefect))
            {
                //Dean 20110526 AddDefect 時要先檢查所hide的值，因為母頁不會刷新，要避免重複Defect
                if (parentWindow.status == "A") {
                    document.getElementById("<%=lblListDefect.ClientID %>").value += defectObj.value + ";";
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
            if (editInputCheck())
            {
                //edit action
                PAQCRepairService.update(parentWindow.globalProID, getDefect(parentWindow.status), editor, stationId, customer, editSuccess, editFail);
            }
        }        
        
        function setDefectInfo()
        {
            var editObj = parentWindow.editObj;
            
            defectObj.value = editObj.defectCodeID;
            piaTestStationObj.value = editObj.piaStation;
            causeObj.value = editObj.cause;
            mainPartObj.value = editObj.majorPart;
            trackingStatusObj.value = editObj.trackingStatus;
            obligationObj.value = editObj.obligation;
            responsibilityObj.value = editObj.responsibility;
            coverObj.value = editObj.cover;
            subDefectObj.value = editObj.subDefect;
            componentObj.value = editObj.component;
            //partTypeObj.value = editObj.partType;
            markObj.value = editObj.mark;
            distributionObj.value = editObj.distribution;
            _4mObj.value = editObj._4M;
            uncoverObj.value = editObj.uncover;           
            document.getElementById("<%=txtSite.ClientID %>").value = editObj.site;
            document.getElementById("<%=txtAreaRemark.ClientID %>").value = editObj.remark;
            document.getElementById("<%=txtAction.ClientID %>").value = editObj.action;          
            
            /*
            if (!emptyPattern.test(editObj.newPartSno))
            {
                partTypeObj.disabled = true;
            }            
            
            if (editObj.partType.toUpperCase() == "OTHER TYPE")
            {
                selectOtherType();
            }
            else if (editObj.partType.toUpperCase() == "")
            {
                selectEmpty();
            }
            else
            {
                selectOther();                 
            }
            */
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

            editObj.defectCodeID = defectObj.value;
            editObj.piaStation = piaTestStationObj.value;
            editObj.cause = causeObj.value;
            editObj.majorPart = mainPartObj.value;
            editObj.trackingStatus = trackingStatusObj.value;
            editObj.obligation = obligationObj.value;
            editObj.responsibility = responsibilityObj.value;
            editObj.cover = coverObj.value;
            editObj.subDefect = subDefectObj.value;
            editObj.component = componentObj.value;
            //editObj.partType = partTypeObj.value;
            //ITC-1360-0463, Jessica Liu, 2012-2-16
            //editObj.mac = "";           
            editObj.mark = markObj.value;
            editObj.distribution = distributionObj.value;
            editObj._4M = _4mObj.value;
            editObj.uncover = uncoverObj.value;           
            editObj.site = document.getElementById("<%=txtSite.ClientID %>").value;
            editObj.remark = document.getElementById("<%=txtAreaRemark.ClientID %>").value;
            editObj.action = document.getElementById("<%=txtAction.ClientID %>").value;

            /* 2012-8-2
            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            editObj.returnStation = parentWindow.returnStationValue;
            */
            
            return editObj;      
            }         
        
        function editInputCheck()
            {     
            if (emptyPattern.test(defectObj.value))
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
            
            if (causeObj.value.toUpperCase() == "WW" && emptyPattern.test(obligationObj.value))
            {
            alert(msgInputObligation);
            obligationObj.focus();
            return false;
            } 
            
            if (emptyPattern.test(markObj.value))
            {
            alert(msgInputMark);
            markObj.focus();
            return false;                
            }      
            
            
            /*
            if (!emptyPattern.test(newFpsno) && emptyPattern.test(fpsno))
            {
            //show error message
            alert(msgInputFaultyPartSno);
                
            return false;
            }
            */
            /*
            if (!emptyPattern.test(npn) && emptyPattern.test(fpn))
            {
                alert(msgInputFaultyPartSn);
                
                return false;
            }
            */
            
            /*
            var type = partTypeObj.value;
            
            if (type.toUpperCase() != "OTHER TYPE" && !emptyPattern.test(type) && emptyPattern.test(fpsno))
            {
                //show error message
                alert(msgInputFaultyPartSno);
                
                return false;                
            }
            */
            /*
            if (parentWindow.status == "E" && !emptyPattern.test(parentWindow.globalNewPartSno) && emptyPattern.test(newFpsno))
            {
                alert(msgInputNewPartSno);
                
                return false;     
            }
            */
            
            //Dean 20110526 AddDefect 時要先檢查所hide的值，因為母頁不會刷新，要避免新增重複Defect
            if (parentWindow.status == "A") {
                if (document.getElementById("<%=lblListDefect.ClientID %>").value != "") {

                    var strArry = new Array();
                    strArry = document.getElementById("<%=lblListDefect.ClientID %>").value.split(";");
                    for (var i = 0; i < strArry.length; i++) {
                        if (strArry[i] == defectObj.value) {
                            alert(msgDuplicateData);
                            defectObj.focus();
                            return false;
                        }
                    }
                }
            }
            //Dean 20110526 AddDefect 時要先檢查所hide的值，因為母頁不會刷新，要避免新增重複Defect
            
            var objId = (parentWindow.status == "E" ? parentWindow.editObj.id : "");
            
            if (parentWindow.isExistDefectInTable(defectObj.value, objId))
            {
                alert(msgDuplicateData);
                defectObj.focus();
                return false;
            }
            /*
            if (type.toUpperCase() == "MB" && parentWindow.hasMBRecord(objId))
            {
                alert(msgDuplicateMBType);
                //partTypeObj.focus();
                return false;                
            }  
            */
                        
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
            piaTestStationObj.selectedIndex = -1;
            causeObj.selectedIndex = -1;
            mainPartObj.selectedIndex = -1;
            trackingStatusObj.selectedIndex = -1;
            obligationObj.selectedIndex = -1;
            responsibilityObj.selectedIndex = -1;
            coverObj.selectedIndex = -1;
            subDefectObj.selectedIndex = -1;
            componentObj.selectedIndex = -1;
            //partTypeObj.selectedIndex = -1;
            markObj.selectedIndex = -1;
            //distributionObj.selectedIndex = -1;
            _4mObj.selectedIndex = -1;
            uncoverObj.selectedIndex = -1;         
            document.getElementById("<%=txtSite.ClientID %>").value = "";
            document.getElementById("<%=txtAreaRemark.ClientID %>").value = "";
                        
            document.getElementById("<%=txtAction.ClientID %>").value = "";             
        }                 
        
        function clkCancel()
        {
            window.close();
        }
        
        function OnkeyupRemark()
        {
            var str = document.getElementById("<%=txtAreaRemark.ClientID%>").value;
            
            if ((str.length) > 100)
            {
                document.getElementById("<%=txtAreaRemark.ClientID%>").value = str.substring(0,100);
            }
        }                      
    </script>       
</body>  
</html>


<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="ModuleApprovalItem.aspx.cs" Inherits="DataMaintain_ModuleApprovalItem"
    ValidateRequest="false" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<%--
 ITC-1361-0016
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

     
    
    
    
    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>    
    <script type="text/javascript" src="../CommonControl/JS/Browser.js"></script>

    <script type="text/javascript" src="../js/jquery.dataTables.js"></script>
    
    <script type="text/javascript" src="../js/jquery-ui-1.8.13.custom.min.js "></script>     
    <script type="text/javascript" src="../js/jscal2.js "></script>
    <script type="text/javascript" src="../js/lang/cn.js "></script>    

    <script type="text/javascript" src="../js/jquery.multiselect.js "></script>     
    <script type="text/javascript"  src="../js/jquery.multiselect.filter.js "></script>     
    <script type="text/javascript" src="../js/wz_tooltip.js "></script>
        
    
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.multiselect.filter.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.dataTables.css" />
    
    
    
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width: 200px;">
                        <asp:Label ID="lblModuleTop" runat="server" CssClass="iMes_label_13pt" Text="Module:"></asp:Label>
                    </td>
                    <td width="42%">
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                            Visible="true">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbModuleTop" runat="server" Width="70%">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 80px;">
                    </td>
                    <td width="42%">
                    </td>
                </tr>
            </table>
            <table width="100%" border="0">
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="40%">
                    </td>
                    <td align="right">
                        <input type="button" id="btnStdPalletWeight" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" 
                        onmouseout="this.className='iMes_button_onmouseout'" onclick="ShowDialog();" value="Need Upload List" style="width:20%"/> 
                        <input type="button" id="btnDelete" runat="server" onclick="if(clkButton())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnDelete_ServerClick"></input>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional" RenderMode="Inline"
            Visible="true" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div id="div2" style="height: 400px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="130%"
                        RowStyle-Height="20" GvExtWidth="100%" GvExtHeight="390px" Height="382px" AutoHighlightScrollByValue="true"
                        HighLightRowPosition="3" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                        OnRowDataBound="gd_RowDataBound" EnableViewState="false">
                    </iMES:GridViewExt>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td width="15%">
                        <asp:Label ID="lblModule" runat="server" CssClass="iMes_label_13pt" Text="Module:"></asp:Label>
                    </td>
                    <td width="25%">
                        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                            Visible="true">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbModule" runat="server" Width="70%">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td width="15%">
                        <asp:Label ID="lblActionName" runat="server" CssClass="iMes_label_13pt" Text="ActionName:"></asp:Label>
                    </td>
                    <td width="25%">
                        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                            Visible="true">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbActionName" runat="server" Width="70%">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDepartment" runat="server" CssClass="iMes_label_13pt" Width="98%"
                            Text="Department:"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                            Visible="true">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbDepartment" runat="server" Width="70%">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblIsNeedApproval" runat="server" CssClass="iMes_label_13pt" Width="98%"
                            Text="IsNeedApproval:"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updatePanel5" runat="server" UpdateMode="Conditional" RenderMode="Inline" Visible="true">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbIsNeedApproval" runat="server" Width="70%">
                                    <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblOwnerEmail" runat="server" CssClass="iMes_label_13pt" Width="98%"
                            Text="OwnerEmail:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <input type="text" id="txtOwnerEmail" runat="server" maxlength="255" style="width: 98%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCCEmail" runat="server" CssClass="iMes_label_13pt" Width="98%"
                            Text="CCEmail:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <input type="text" id="txtCCEmail" runat="server" maxlength="255" style="width: 98%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblIsNeedUploadFile" runat="server" CssClass="iMes_label_13pt" Text="IsNeedUploadFile:"></asp:Label>
                    </td>
                    <td>
                        <%--<asp:UpdatePanel ID="updatePanel5" runat="server" UpdateMode="Conditional" RenderMode="Inline" Visible="true">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbIsNeedUploadFile" runat="server" Width="70%">
                                    <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="N" Value="N" ></asp:ListItem>
                                </asp:DropDownList>   
                                
                                
                            </ContentTemplate>
                        </asp:UpdatePanel>--%>
                        N<input type="radio" id="N" name="IsNeedUpload" value="N" onclick="changeRadio()"/>
                        Y<input type="radio" id="Y" name="IsNeedUpload" value="Y" checked="checked" onclick="changeRadio()"/>
                        <asp:ListBox ID="lboxFamily" runat="server" SelectionMode="Multiple" Height="95%"
                        Width="300px" CssClass="CheckBoxList"></asp:ListBox>

                    </td>
                    <td colspan="2">
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNoticeMsg" runat="server" CssClass="iMes_label_13pt" Width="98%"
                            Text="NoticeMsg:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <input type="text" id="txtNoticeMsg" runat="server" maxlength="255" style="width: 98%" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    </td>
                    <td>
                        <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnSave_ServerClick"></input>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        <input id="hidID" type="hidden" runat="server" />
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnModuleChange" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnModuleselectChange" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="hidActionName" runat="server" />
        <input type="hidden" id="hidDepartment" runat="server" />
        <input type="hidden" id="hidIsNeedUpload" runat="server" />
        <input type="hidden" id="hidFamilyListCount" runat="server" />
        <input type="hidden" id="hidFamilyTop" runat="server" />
        <input type="hidden" id="hidCheckFamilyList" runat="server" />
        
        <input type="button" id="btnModuleChange" runat="server" style="display: none" onserverclick="btnModuleChange_ServerClick" />
        <input type="button" id="btnModuleselectChange" runat="server" style="display: none"
            onserverclick="btnModuleselectChange_ServerClick" />
    </div>
    <div id="divWait" oselectid="" align="center" style="cursor: wait; filter: Chroma(Color=skyblue);
        background-color: skyblue; display: none; top: 0; width: 100%; height: 100%;
        z-index: 10000; position: absolute">
        <table style="cursor: wait; background-color: #FFFFFF; border: 1px solid #0054B9;
            font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr>
                <td align="center">
                    <img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif" />
                </td>
                <td align="center" style="color: #0054B9; font-size: 10pt; font-weight: bold;">
                    Please wait.....
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">

        var msg1 = "";
        var msg2 = "";
        var msg3 = "";
        var msg4 = "";
        var msg5 = "";
        var msg6 = "";
        var msg7 = "";
        var msg8 = "";
        var msg9 = "";
        var msg10 = "";
        var FamilyTop = "Sepcific Family";
        var CheckFamilyList = '';
        var sessionId;

        function clkButton() {
            switch (event.srcElement.id) {
                case "<%=btnSave.ClientID %>":

                    if (!clkSave()) {
                        return false;
                    }
                    break;

                case "<%=btnDelete.ClientID %>":

                    if (!clkDelete()) {
                        return false;
                    }
                    break;
            }
            ShowWait();
            return true;
        }

        var iSelectedRowIndex = null;
        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }

        function getModuleTopCmbObj() {
            return document.getElementById("<%=cmbModuleTop.ClientID%>");
        }

        function getModuleCmbObj() {
            return document.getElementById("<%=cmbModule.ClientID%>");
        }

        function initControls() {
            getModuleTopCmbObj().onchange = ModuleTopSelectOnChange;
            getModuleCmbObj().onchange = ModuleSelectOnChange;
        }

        function ModuleTopSelectOnChange() {
            document.getElementById("<%=btnModuleChange.ClientID%>").click();
            ShowWait();
        }

        function ModuleSelectOnChange() {
            document.getElementById("<%=btnModuleselectChange.ClientID%>").click();
            //ShowWait();
        }

        function changeRadio() {
            var isneed = "";
            $("#ctl00_iMESContent_lboxFamily").multiselect("uncheckAll");
            if (document.getElementById('N').checked) {
                $("#ctl00_iMESContent_lboxFamily").multiselect("disable");
                isneed = "N";
            }
            else {
                $("#ctl00_iMESContent_lboxFamily").multiselect("enable");
                isneed = "Y";
            }
            document.getElementById("<%=hidIsNeedUpload.ClientID %>").value = isneed;
        }


        window.onload = function() {
            msg1 = "<%=pmtMessage1%>";
            msg2 = "<%=pmtMessage2%>";
            msg3 = "<%=pmtMessage3%>";
            msg4 = "<%=pmtMessage4%>";
            msg5 = "<%=pmtMessage5%>";
            msg10 = "<%=pmtMessage10%>";
            EndRequestHandler();
            document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";
            sessionId = '<%=Request["SessionId"] %>';
            initControls();
            //changeRadio();
            ShowRowEditInfo(null);
            //设置表格的高度  
            resetTableHeight();

        };

        //设置表格的高度  
        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 55;
            var marginValue = 12;
            var tableHeigth = 300;
            //var tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
            try {
                tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
            }
            catch (e) {

                //ignore
            }
            //为了使表格下面有写空隙
            var extDivHeight = tableHeigth + marginValue;
            div2.style.height = extDivHeight + "px";
            document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
        }

        function clkDelete() {
            var gdObj = document.getElementById("<%=gd.ClientID %>")
            var curIndex = gdObj.index;
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            if (curIndex >= recordCount) {
                alert(msg1);
                return false;
            }

            var ret = confirm(msg2);
            if (!ret) {
                return false;
            }

            return true;

        }

        function DeleteComplete() {
            ShowRowEditInfo(null);
            SetCmbIsNeedUploadFile(null);
        }

        function clkSave() {
            CheckFamilyList = "";
            $("#ctl00_iMESContent_lboxFamily option:selected").each(function() {
                CheckFamilyList = CheckFamilyList + $(this).val() + ',';
            });
            
            return check();

        }

        

        function check() {
            if (document.getElementById("<%=cmbModule.ClientID %>").value == "") {
                alert("Please select Module");
                return false;
            }
            if (document.getElementById("<%=cmbActionName.ClientID %>").value == "") {
                alert("Please select ActionName");
                return false;
            }
            if (document.getElementById("<%=cmbDepartment.ClientID %>").value == "") {
                alert("Please select Department");
                return false;
            }

            if (document.getElementById("<%=cmbIsNeedApproval.ClientID %>").value == "") {
                alert("Please select IsNeedApproval");
                return false;
            }

            if (document.getElementById("<%=cmbModule.ClientID %>").value == "FAIBTO" || document.getElementById("<%=cmbModule.ClientID %>").value == "FAIBTF") {
                if (document.getElementById("<%=cmbDepartment.ClientID %>").value == "OQC") {
                    if (document.getElementById("<%=cmbIsNeedApproval.ClientID %>").value == "N") {
                        alert("OQC部門必須Approval");
                        return false;
                    }
                }
            }
            
            if (document.getElementById('Y').checked == true && CheckFamilyList == "") {
                
                alert("Please select IsNeedUploadFile");
                return false;
            }
            
            
            document.getElementById("<%=hidCheckFamilyList.ClientID %>").value = CheckFamilyList;
            if (document.getElementById("<%=txtOwnerEmail.ClientID %>").value == "") {
                alert("Please input OwnerEmail");
                return false;
            }
            return true;
        }

        function clickTable(con) {
            ShowWait();
            setGdHighLight(con);
            ShowRowEditInfo(con);
            SetCmbIsNeedUploadFile(con);
            HideWait();
        }

        function setNewItemValue() {
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            ChengeDrp(document.getElementById("<%=cmbModule.ClientID %>"), "");
            ChengeDrp(document.getElementById("<%=cmbActionName.ClientID %>"), "");
            ChengeDrp(document.getElementById("<%=cmbDepartment.ClientID %>"), "");
            document.getElementById("<%=hidActionName.ClientID %>").value = "";
            document.getElementById("<%=hidDepartment.ClientID %>").value = "";
            ChengeDrp(document.getElementById("<%=cmbIsNeedApproval.ClientID %>"), "");
            document.getElementById("<%=txtOwnerEmail.ClientID %>").value = "";
            document.getElementById("<%=txtCCEmail.ClientID %>").value = "";
            
            document.getElementById("<%=txtNoticeMsg.ClientID %>").value = "";
            document.getElementById("<%=hidID.ClientID %>").value = "";
        }

        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }
            var currentID = con.cells[0].innerText.trim();
            if (currentID == "") {
                setNewItemValue();
            }
            else {
                var module = con.cells[1].innerText.trim();
                var department = con.cells[3].innerText.trim();
                document.getElementById("<%=hidID.ClientID %>").value = currentID;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;

                ChengeDrp(document.getElementById("<%=cmbModule.ClientID %>"), module);
                ChengeDrp(document.getElementById("<%=cmbActionName.ClientID %>"), con.cells[2].innerText.trim());
                ChengeDrp(document.getElementById("<%=cmbDepartment.ClientID %>"), department);
                document.getElementById("<%=hidActionName.ClientID %>").value = con.cells[2].innerText.trim();
                document.getElementById("<%=hidDepartment.ClientID %>").value = con.cells[3].innerText.trim();
                if (module.startsWith("FAI") && department == "OQC") {
                    ChengeDrp(document.getElementById("<%=cmbIsNeedApproval.ClientID %>"), "Y");
                    $("#cmbIsNeedApproval").attr("disabled", true);
                }
                else {
                    ChengeDrp(document.getElementById("<%=cmbIsNeedApproval.ClientID %>"), con.cells[4].innerText.trim());
                    document.getElementById("<%=cmbIsNeedApproval.ClientID %>").disabled = false;
                }
                document.getElementById("<%=txtOwnerEmail.ClientID %>").value = con.cells[5].innerText.trim();
                document.getElementById("<%=txtCCEmail.ClientID %>").value = con.cells[6].innerText.trim();
                
                if (con.cells[7].innerText.trim() == "Y") {
                    document.getElementById('Y').checked = true;
                }
                else {
                    document.getElementById('N').checked = true;
                }
                changeRadio();
                document.getElementById("<%=txtNoticeMsg.ClientID %>").value = con.cells[8].innerText.trim();
                ModuleSelectOnChange();
                document.getElementById("ctl00_iMESContent_cmbIsNeedApproval").disabled = true;
            }
        }

        function setNewCmbIsNeedUploadFile() {
            $("#ctl00_iMESContent_lboxFamily").multiselect("uncheckAll");
            document.getElementById('Y').checked = true;
            CheckFamilyList = "";
            $("#ctl00_iMESContent_lboxFamily").multiselect("refresh");
        }

        function SetCmbIsNeedUploadFile(con) {
            if (con == null) {
                setNewCmbIsNeedUploadFile();
                return;
            }
            var currentID = con.cells[0].innerText.trim();
            if (currentID == "") {
                setNewCmbIsNeedUploadFile();
            }
            else { 
                var temp = con.cells[12].innerText.trim();
                var family = temp.split(',');
                $("#ctl00_iMESContent_lboxFamily").multiselect("uncheckAll");
                if (family[0] === "ALL") {
                    $("#ctl00_iMESContent_lboxFamily").multiselect("checkAll");
                    FamilyTop = "All Family";
                }
                else {
                    $("#ctl00_iMESContent_lboxFamily option").each(function() {
                        for (var i = 0; i < family.length; i++) {

                            if ($(this).val() === family[i]) {
                                $(this).attr('selected', 'selected');
                            }
                            continue;
                        }
                    });
                    //$("#ctl00_iMESContent_lboxFamily").multiselect("refresh");
                }
                $("#ctl00_iMESContent_lboxFamily").multiselect("refresh");
                document.getElementById("<%=hidFamilyTop.ClientID %>").value = FamilyTop;
            }
            
        }

        function changeModule() {
            ChengeDrp(document.getElementById("<%=cmbActionName.ClientID %>"), document.getElementById("<%=hidActionName.ClientID %>").value);
            ChengeDrp(document.getElementById("<%=cmbDepartment.ClientID %>"), document.getElementById("<%=hidDepartment.ClientID %>").value);
        }

        function ChengeDrp(obj, value) {
            var ddl = obj;
            var opts = ddl.options.length;
            for (var i = 0; i < opts; i++) {
                if (ddl.options[i].value == value) {
                    ddl.options[i].selected = true;
                    break;
                }
            }
        }

        function AddUpdateComplete(id) {

            var gdObj = document.getElementById("<%=gd.ClientID %>");
            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[0].innerText == id) {
                    selectedRowIndex = i;
                    break;
                }
            }
            if (selectedRowIndex < 0) {
                ShowRowEditInfo(null);
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                ShowRowEditInfo(con);
                SetCmbIsNeedUploadFile(con);
            }
        }

        function DealHideWait() {
            HideWait();
            getModuleTopCmbObj().disabled = false;

        }

        function EndRequestHandler(sender, args) {
            $('.CheckBoxList').multiselect({ selectedList: 1, maxselect: 6, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
        };

        function ShowDialog() {
            var ID = document.getElementById("ctl00_iMESContent_hidID").value;
            var FamilyTop = document.getElementById("<%=hidFamilyTop.ClientID %>").value;
            //var ret = window.showModalDialog("ModuleApprovalItemNeedUploadFile.aspx?AccountId=111&approvalItemID=" + ID + "&familyTop=" + FamilyTop, 0, "dialogwidth:1000px; dialogheight:400px;status:no;help:no; ");
            var url = "ModuleApprovalItemNeedUploadFile.aspx?approvalItemID=" + ID + "&familyTop=" + FamilyTop + "&PCode=" + mpUserInfo.PCode + "&Customer=" + mpUserInfo.Customer + "&UserId=" + mpUserInfo.UserId + "&AccountId=" + mpUserInfo.AccountId + "&Login=" + mpUserInfo.Login + "SessionId=" + sessionId;
            window.showModalDialog(url, null, 'dialogWidth:1000px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');                    
        }
        
    </script>

</asp:Content>

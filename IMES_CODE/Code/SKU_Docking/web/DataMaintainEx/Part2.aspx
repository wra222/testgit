<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Part2.aspx.cs" Inherits="DataMaintain_Part2"
    MasterPageFile="~/MasterPageMaintain.master" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
            
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblPartList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="50%">
                          <asp:TextBox ID="dSearch" runat="server" MaxLength="50" Width="56%" CssClass="iMes_textbox_input_Yellow"
                            onkeypress="OnKeyPress(this)"></asp:TextBox>
                      
                    </td>
                    <td  width="20%" align="right">
                        <button id="btnSaveAs" runat="server" onclick="clkButton()" class="iMes_button">SaveAs</button>
                    </td>
                    <td width="10%" align="right">
                        <input type="button" id="btnSetAttribute" runat="server" class="iMes_button" onclick="if(clickButton())"
                            onserverclick="btnSetAttribute_ServerClick" />
                    </td>
                    <td width="10%" align="right">
                        <input type="button" id="btnDelete" runat="server" onclick="if(clickButton())" onserverclick="btnDelete_ServerClick"
                            class="iMes_button" />
                    </td>
                </tr>
            </table>
        </div>
      
        <div id="div2" style="height: 366px">
            <input id="hidRecordCount" type="hidden" runat="server" />
            <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" 
                RenderMode="Inline">
                <ContentTemplate>
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="125%"
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue="true" HighLightRowPosition="3"
                        RowStyle-Height="20" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                        OnRowDataBound="gd_RowDataBound" EnableViewState="false" 
                        Style="top: 0px; left: 26px">
                    </iMES:GridViewExt>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGetPart" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width: 5%;">
                        <asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="dPartNo" runat="server" MaxLength="20" Width="96%"  CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                    </td>
                    <td style="width: 8%;">
                        <asp:Label ID="lblNodeType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="20%">
                        <iMESMaintain:CmbBomNodeType ID="drpMaintainPartNodeType" runat="server" Width="95%" />
                    </td>
                    <td style="width: 6%;">
                        <asp:Label ID="lblPartType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="20%">
                        <asp:UpdatePanel ID="updatePanel_PartType" runat="server" UpdateMode="Conditional" RenderMode="Inline" Visible="true">
                            <ContentTemplate>
                                <asp:DropDownList ID="CmbMaintainPartTypeAll" runat="server" Width="95%" AutoPostBack="true"
                                 onselectedindexchanged="CmbMaintainPartTypeAll_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 5%;">
                        <asp:Label ID="lblDesc" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="25%" >
                        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline" Visible="true">
                            <ContentTemplate>
                                <asp:DropDownList ID="CmbMiantainDesc" runat="server" Width="98%"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    
                    <td style="width: 8%;">
                        <asp:Label ID="lblCustPN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="dCustPn" runat="server" MaxLength="20" Width="93%" SkinID="textBoxSkin" Style="ime-mode: disabled;" onpaste="return false"></asp:TextBox>
                    </td>
                    <td style="width: 6%;">
                        <asp:Label ID="lblflag" runat="server" CssClass="iMes_label_13pt" Text="Flag:"></asp:Label>
                    </td>
                    <td width="20%">
                        <asp:DropDownList ID="cmbFlag" runat="server" Width="95%">
                            <asp:ListItem Text="Normal" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Hold" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 5%;">
                        <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="20%" colspan="5">
                        <asp:TextBox ID="dRemark" runat="server" MaxLength="80" Width="98%" SkinID="textBoxSkin"
                        Style='ime-mode: disabled;' onpaste="return false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <input id="checkAutoDL" runat="server" ispercentage="true" type="checkbox" width="100" />
                        <asp:Label ID="lblAutoDL" runat="server" CssClass="iMes_label_13pt" />
                    </td>

                    <td width="10%" colspan="2" align="center">
                        <input id="btnAdd" runat="server" class="iMes_button" onclick="if(clickButton())" type="button" onserverclick="btnAdd_ServerClick" />
                        <input id="btnSave" runat="server" class="iMes_button" onclick="if(clickButton())" type="button" onserverclick="btnSave_ServerClick" />
                    </td>
                </tr>
            </table>
                        
                     
        </div>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"
            UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnBomNodeTypeChange" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnPartTypeChange" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnPartTypeChange" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSetAttribute" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnGetPartType" EventName="ServerClick" />
              <asp:AsyncPostBackTrigger ControlID="btnGetPart" EventName="ServerClick" />
                <%--    <asp:AsyncPostBackTrigger ControlID="btnComBoxFamilyChange" EventName="ServerClick" />--%>
            </Triggers>
        </asp:UpdatePanel>
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="hidPartNo" runat="server" />
        <input type="hidden" id="hidPartType" runat="server" />
        <input type="hidden" id="hidDesc" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="hidInputPartNo" runat="server" />
      <input type="hidden" id="hidOldPN" runat="server" />
        <button id="btnPartTypeChange" runat="server" type="button" style="display: none"
            onserverclick="btnPartTypeChange_ServerClick">
        </button>
        <button id="btnBomNodeTypeChange" runat="server" type="button" style="display: none"
            onserverclick="btnBomNodeTypeChange_ServerClick">
        </button>
        <button id="btnGetPartType" runat="server" type="button" style="display: none"
            onserverclick="btnGetPartType_ServerClick">
        </button>
        <%--  <button id="btnComBoxFamilyChange" runat="server" type="button" style="display: none"
        onserverclick="btnComBoxFamilyChange_ServerClick">
    </button>--%>
    
    <button id="btnGetPart" runat="server" type="button" style="display: none"
            onserverclick="btnGetPart_ServerClick">
        </button>
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

    <script type="text/javascript">
        window.onload = function() {
            msg1 = "<%=pmtMessage1%>";
            msg2 = "<%=pmtMessage2%>";
            msg3 = "<%=pmtMessage3%>";
            msg4 = "<%=pmtMessage4%>";
            msg5 = "<%=pmtMessage5%>";
            msg6 = "<%=pmtMessage6%>";
            msg7 = "<%=pmtMessage7%>";
            msg8 = "<%=pmtMessage8%>";
            
            editor = "<%=username %>";
            //设置表格的高度  
            initControls();
        };
        var iSelectedRowIndex = null;
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
       
        function OnKeyPress(obj) {
            var key = event.keyCode;
            if (key == 13)//enter
            {
                var value = document.getElementById("<%=dSearch.ClientID %>").value.trim().toUpperCase();
                document.getElementById("<%=hidInputPartNo.ClientID%>").value = value;
                document.getElementById("<%=hidOldPN.ClientID%>").value = value;

                
                document.getElementById("<%=hidDesc.ClientID%>").value = "";
                if (value != "")
                { ShowWait(); document.getElementById("<%=btnGetPart.ClientID%>").click(); }
                return;
                
                if (event.srcElement.id == "<%=dSearch.ClientID %>") {
               
                    if (value != "") {
                        findPartNo(value, true);
                    }
                }
            }

        }
        function findPartNo(searchValue, isNeedPromptAlert) {
            var gdObj = document.getElementById("<%=gd.ClientID %>");
            searchValue = searchValue.toUpperCase();
            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (searchValue.trim() != "" && gdObj.rows[i].cells[0].innerText.toUpperCase().indexOf(searchValue) == 0) {
                    selectedRowIndex = i;
                    break;
                }
            }

            if (selectedRowIndex < 0) {
                if (isNeedPromptAlert == true) {
                    alert(msg6);
                }
                else if (isNeedPromptAlert == null) {
                    ShowRowEditInfo(null);
                }
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                ShowRowEditInfo(con);
            }
        }
        function NoMatchFamily() {
            alert(msg6);
            return;
        }
        function initControls() {
            setNewItemValue();
            getBomNodeTypeObj().onchange = btnPartNodeTypeChangeOnChange;
            getPartTypeObj().onchange = btnPartTypeChangeOnChange;
           // getMaintainPartTypeCmbObj().onchange = btnPartTypeChangeOnChange;
        }
        function DeleteComplete() {
            ShowRowEditInfo(null);
            DealHideWait();
            setNewItemValue();
        }

        function clkButton() {
            var dlgFeature = "dialogHeight:200px;dialogWidth:400px;center:yes;status:no;help:no";
            var oldPartNoValue = document.getElementById("<%=hidPartNo.ClientID%>").value.trim().toUpperCase();
            if (oldPartNoValue != "") {
                oldPartNoValue = encodeURI(Encode_URL2(oldPartNoValue));
                var user = "<%=Master.userInfo.UserId%>";
                user = encodeURI(Encode_URL2(user));
                var dlgReturn = window.showModalDialog("MaintainSaveAs.aspx?OldPartNo=" + oldPartNoValue + "&userName=" + user, window, dlgFeature);

            }
            else {
                alert(msg2);
            }
        }

        function btnPartNodeTypeChangeOnChange() {
          
            document.getElementById("<%=btnBomNodeTypeChange.ClientID%>").click();
        }
        function btnPartTypeChangeOnChange() {
           
            document.getElementById("<%=btnPartTypeChange.ClientID%>").click();
        }

        function AddUpdateComplete(id) {

            DealHideWait();
            return;
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

            }

        }
        function clickButton() {

            switch (event.srcElement.id) {
                case "<%=btnSave.ClientID %>":
                    if (clkSave() == false) {
                        return false;
                    }
                    break;

                case "<%=btnDelete.ClientID %>":

                    if (clkDelete() == false) {
                        return false;
                    }
                    break;
                case "<%=btnAdd.ClientID %>":
                    if (clkAdd() == false) {
                        return false;
                    }
                    break;

                case "<%=btnSetAttribute.ClientID %>":
                    if (btnSetAttribute() == false) {
                        return false;
                    }
                    break;
            }
            ShowWait();
            return true;

        }

        function clkDelete() {
            if (iSelectedRowIndex == null) {
                alert(msg1);
                return false;
            }
            //var ret = confirm("确定要删除这条记录么？");
            var ret = confirm(msg2);
            if (!ret) {
                return false;
            }

            return true;

        }

        function clkSave() {

            return check();

        }
        function clkAdd() {

            return check();
        }
        function check() {
            var partNo = document.getElementById("<%=dPartNo.ClientID %>");
            if (partNo.value == "") {
                alert(msg3);
                return false;
            }
            var bomNodeType = getBomNodeTypeObj();
            if (bomNodeType.selectedIndex <= 0) {
                alert(msg4);
                return false;
            }

            var partType = getPartTypeObj();
            if (partType.selectedIndex <= 0) {
                alert(msg8);
                return false;
            } 
            var descType = getTypeDescCmbObj();
            if (descType.selectedIndex <= 0) {
                alert(msg5);
                return false;
            }
            return true;
        }

        function btnSetAttribute() {
            partNo = document.getElementById("<%=hidPartNo.ClientID %>").value;
            nodeType = document.getElementById("<%=hidPartType.ClientID %>").value;
            var dlgFeature = "dialogHeight:660px;dialogWidth:920px;center:yes;status:yes;help:no;";
            var dlgReturn = window.showModalDialog("SetPartAttribute2.aspx?username=" + editor + "&partNo=" + partNo + "&nodeType=" + nodeType, window, dlgFeature);
        }
        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);

        }

        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }

        function ShowRowEditInfo(con) {
            if (con == null) {
               setNewItemValue();
                return;
            }
            
            //getMaintainFamilyCmbObj().value = con.cells[0].innerText.trim();
            document.getElementById("<%=dPartNo.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=dCustPn.ClientID %>").value = con.cells[4].innerText.trim();
            document.getElementById("<%=cmbFlag.ClientID %>").value = con.cells[6].innerText.trim();
            document.getElementById("<%=dRemark.ClientID %>").value = con.cells[7].innerText.trim();
           // getMaintainPartTypeCmbObj().value = con.cells[1].innerText.trim();
            getBomNodeTypeObj().value = con.cells[1].innerText.trim();
            var checkDL = con.cells[5].innerText.trim();
            if (checkDL == "1") {
                document.getElementById("<%=checkAutoDL.ClientID %>").checked = true;
            }
            document.getElementById("<%=hidPartType.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=hidDesc.ClientID %>").value = con.cells[3].innerText.trim();
            var currentId = con.cells[0].innerText.trim();
            document.getElementById("<%=hidPartNo.ClientID %>").value = currentId;

            if (currentId == "") {
                //            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
                //            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;
                setNewItemValue();
                return;
            }
            else {
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
                document.getElementById("<%=btnSaveAs.ClientID %>").disabled = false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
                document.getElementById("<%=btnSetAttribute.ClientID %>").disabled = false;

            }
           // btnPartTypeChangeOnChange();
            //   setBomNodeTypeValue();
            getPartTypeList();
        }
        function getPartTypeList() {
            document.getElementById("<%=btnGetPartType.ClientID%>").click();
        }
        function setBomNodeTypeValue() {
            document.getElementById("<%=btnBomNodeTypeChange.ClientID%>").click();
        }
        function setNewItemValue() {
            getBomNodeTypeObj().selectedIndex = 0;
           getTypeDescCmbObj().selectedIndex = 0;
           getPartTypeObj().selectedIndex = 0;
           document.getElementById("<%=dSearch.ClientID %>").value = ""
           document.getElementById("<%=dCustPn.ClientID %>").value = "";
           document.getElementById("<%=cmbFlag.ClientID %>").selectedIndex = 0;
           document.getElementById("<%=dPartNo.ClientID %>").value = "";
           document.getElementById("<%=dRemark.ClientID %>").value = "";
           document.getElementById("<%=checkAutoDL.ClientID %>").checked = false;
           document.getElementById("<%=btnSetAttribute.ClientID %>").disabled = true;
           document.getElementById("<%=btnSave.ClientID %>").disabled = true;
           document.getElementById("<%=btnSaveAs.ClientID %>").disabled = true;
           document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        }
        function DealHideWait() {
            HideWait();
        }
        function getPartTypeObj() {
            return document.getElementById("<%=CmbMaintainPartTypeAll.ClientID %>");
        }
        function getTypeDescCmbObj() {
            return document.getElementById("<%=CmbMiantainDesc.ClientID %>");
            //drpMaintainPartNodeType
        }
        function RemoveGvRow() {
            var gdObj = document.getElementById("<%=gd.ClientID %>");
            gdObj.deleteRow(1);
        }
//        function getBomNodeTypeObj() {
//            return document.getElementById("<%=drpMaintainPartNodeType.ClientID %>");
//            //drpMaintainPartNodeType getBomNodeTypeObj
//        }
    </script>

</asp:Content>

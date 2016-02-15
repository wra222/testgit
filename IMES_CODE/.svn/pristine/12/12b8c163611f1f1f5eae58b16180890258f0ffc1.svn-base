<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="PalletStandard.aspx.cs" Inherits="DataMaintain_PLTStandard" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <style type="text/css">
        .iMes_div_MainTainEdit
        {
            border: thin solid Black;
            background-color: #99CDFF;
            margin: 0 0 20 0;
        }
        .iMes_textbox_input_Yellow
        {
        }
        #btnDel
        {
            width: 14px;
        }
        .style1
        {
            width: 5px;
        }
    </style>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" class="">
                <tr>
                    <td nowrap>
                        <asp:Label ID="lblPalletNoQuery"  runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="40%">
                        <input type="text" ID="dSearch" MaxLength="12" Width="50%" CssClass="iMes_textbox_input_Yellow"
                            onkeypress='OnKeyPress(this)' Visible="true" />
                    </td>
                    <td width="32%" align="right">
                        <input type="button" id="btnDel" runat="server" class="iMes_button" onclick="if(clkDelete())"
                            onserverclick="btnDelete_ServerClick" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional" RenderMode="Inline"
            Visible="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDel" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div2">
            <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                Visible="true">
                <ContentTemplate>
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt SkinID="clearStyle" ID="gd" runat="server" AutoGenerateColumns="true"
                        Width="124%" GvExtHeight="676px" RowStyle-Height="20" GvExtWidth="100%" AutoHighlightScrollByValue="true"
                        HighLightRowPosition="3" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                        OnRowDataBound="gd_RowDataBound" EnableViewState="false" Style="top: -163px;
                        left: -286px">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
           <asp:UpdatePanel ID="updatePanelEdit" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                Visible="true">
            <ContentTemplate>

            <table width="100%" class="iMes_div_MainTainEdit">
                 <tr>
                    <td width="10%">
                        <asp:Label ID="lblPalletSpec" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="50%" colspan="3">
                        <iMESMaintain:CmbMaintainPalletSpec ID="ddlPalletSpec" runat="server" Width="100%"></iMESMaintain:CmbMaintainPalletSpec>
                    </td>
                    <td width="10%">
                        <asp:Label ID="lblPalletNO" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="TextPalletNO" runat="server"   MaxLength="12"   Width="90%" SkinId="textBoxSkin"  ></asp:TextBox>                      
                    </td>
                    
                    <td align="right" width="10%">
                        <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnAdd_ServerClick" />
                    </td>
                     
                </tr>
                <tr>
                    <td width="10%">
                        <asp:Label ID="lblLength" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    
                    <td width="20%">
                     
                        <asp:TextBox ID="TextLength" runat="server"  OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"  MaxLength="18"   Width="99%" SkinId="textBoxSkin"  ></asp:TextBox>                         
                     
                    </td>
                     
                    <td width="10%">
                        <asp:Label ID="lblWidth" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="TextWidth" runat="server" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"   MaxLength="18"   Width="99%" SkinId="textBoxSkin"  ></asp:TextBox>                      
                    </td>
                    <td width="10%">
                        <asp:Label ID="lblHeight" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="TextHeight" runat="server"  OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"  MaxLength="18"   Width="90%" SkinId="textBoxSkin"  ></asp:TextBox>                      
                    </td>
                    <td align="right">
                        <input type="button" id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnSave_ServerClick" />
                        <input type="hidden" id="dTableHeight" runat="server" />
                    </td>
                </tr>
            </table>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="HiddenCustomerName" runat="server" />
        <input type="hidden" id="HiddenSelectedId" runat="server" />
        
        <input type="hidden" id="HiddenLenght" runat="server" />
        <input type="hidden" id="HiddenWidth" runat="server" />
        <input type="hidden" id="HiddenHeight" runat="server" />
        
        <button id="btnRefreshCustomerList" runat="server" type="button" onclick="" style="display: none">
        </button>
    </div>
    <div id="divWait" oselectid="" align="center" style="cursor: wait; 
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

        var customerObj;
        var descriptionObj;

        var msgSelectOne = "";
        var msgDelConfirm = "";
        var msgPalletNoSel = "";
        var msgNotFound = "";
        var msgLength = "";
        var msgWidth = "";
        var msgHeight = "";
        window.onload = function() {
            msgSelectOne = "<%=MsgSelectOne%>";
            msgDelConfirm = "<%=MsgDelConfirm%>";
            msgPalletNoSel = "<%=msgPalletNoSel%>";
            msgLength = "<%=msgLength%>";
            msgWidth = "<%=msgWidth%>";
            msgHeight = "<%=msgHeight%>";
            msgNotFound ="<%=MsgNotFound%>";
            document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";

            ShowRowEditInfo(null);

            resetTableHeight();
        };

        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 80;
            var marginValue = 10;
            var tableHeigth = 300;

            try {
                //tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div4.offsetHeight - div3.offsetHeight - adjustValue;
                tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight  - div3.offsetHeight - adjustValue;
            }
            catch (e) {
                //ignore
            }
            //为了使表格下面有写空隙
            var extDivHeight = tableHeigth + marginValue;

            document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
            //alert(document.getElementById("div_<%=gd.ClientID %>").style.height)
            div2.style.height = extDivHeight + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
        }

        var iSelectedRowIndex = null;
        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                //去掉过去高亮行
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            //设置当前高亮行   
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            //记住当前高亮行
            iSelectedRowIndex = parseInt(con.index, 10);
        }


        function clkDelete() {
            ShowInfo("");
            var gdObj = document.getElementById("<%=gd.ClientID %>")
            var curIndex = gdObj.index;
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            if (curIndex >= recordCount) {
                //            alert("Please select one row!");   //2
                alert(msgSelectOne);
                return false;
            }

            var ret = confirm(msgDelConfirm);
            if (!ret) {
                return false;
            }
            ShowWait();

            return true;

        }

        function DeleteComplete() {
            ShowRowEditInfo(null);
        }

        function clkSave() {
            return checkAdaptorInfo();
        } 
        

        function clkAdd() {
            return checkAdaptorInfo();
        }

        function checkAdaptorInfo() {

            ShowInfo("");

            var PalletSpecObj = getMaintainPalletSpecCmbObj();
            var PalletNoObj = document.getElementById("<%=this.TextPalletNO.ClientID%>");
            var LengthObj = document.getElementById("<%=this.TextLength.ClientID%>");
            var WidthObj = document.getElementById("<%=this.TextWidth.ClientID%>");
            var HeightObj = document.getElementById("<%=this.TextHeight.ClientID%>");

            var getLen = LengthObj.value.trim();
            var getWidth = WidthObj.value.trim();
            var getHeight = HeightObj.value.trim();
            if ((getLen.length <= 16) && (getLen.indexOf(".") == -1) && (getLen.length != 0))
                LengthObj.value = getLen + ".0";
            if ((getWidth.length <= 16) && (getWidth.indexOf(".") == -1) && (getWidth.length != 0))
                WidthObj.value = getWidth + ".0";
            if ((getHeight.length <= 16) && (getHeight.indexOf(".") == -1) && (getHeight.length !=0))
                HeightObj.value = getHeight + ".0";
                
            //if (PalletSpecObj.value.trim() == "") {
            //    alert(msgPalletSpecSel);
            //    return false;
            //}

            if (PalletNoObj.value.trim() == "") {
                alert(msgPalletNoSel);
                return false;
            }
        
            if ((LengthObj.value.trim() == "")||(checkDecimal(LengthObj.value.trim()) == false)) {
                alert(msgLength);
                return false;
            }
            var ilendot;
            ilendot = LengthObj.value.trim().indexOf(".");
            if ((LengthObj.value.trim().length > 16) && ((ilendot == -1) || (ilendot > 17))) {
                alert(msgLength);
                return false;
            }
                        
            if ((WidthObj.value.trim() == "")||(checkDecimal(WidthObj.value.trim()) == false)) {
                alert(msgWidth);
                return false;
            }
            ilendot = WidthObj.value.trim().indexOf(".");
            if ((WidthObj.value.trim().length > 16) && ((ilendot == -1) || (ilendot > 17))) {
                alert(msgWidth);
                return false;
            }
            if ((HeightObj.value.trim() == "")||(checkDecimal(HeightObj.value.trim()) == false)) {
                alert(msgHeight);
                return false;
            }
            ilendot = HeightObj.value.trim().indexOf(".");
            if ((HeightObj.value.trim().length > 16) && ((ilendot == -1) || (ilendot > 17))) {
                alert(msgHeight);
                return false;
            }
            document.getElementById("<%=HiddenLenght.ClientID %>").value = LengthObj.value;
            document.getElementById("<%=HiddenWidth.ClientID %>").value = WidthObj.value;
            document.getElementById("<%=HiddenHeight.ClientID %>").value = HeightObj.value;
            
            
            ShowWait();
            return true;
        }

        function checkDecimal(strDecimal) {
            if (isNaN(strDecimal)) {
                return false;
            } 
            var Regex = /^-?[\d]+.[\d]{0,1}$/;

            if (Regex.test(strDecimal) == false) {
                return false;
            }
            return true;
        }
        
        function clickTable(con) {
            ShowInfo("");
            var selectedRowIndex = con.index;
            setGdHighLight(con);
            ShowRowEditInfo(con);
        }

        function ShowRowEditInfo(con) {

            var PalletSpecObj = getMaintainPalletSpecCmbObj();
            var PalletNoObj = document.getElementById("<%=this.TextPalletNO.ClientID%>");
            var LengthObj = document.getElementById("<%=this.TextLength.ClientID%>");
            var WidthObj = document.getElementById("<%=this.TextWidth.ClientID%>");
            var HeightObj = document.getElementById("<%=this.TextHeight.ClientID%>");
            if (con == null) {
                PalletSpecObj.selectedIndex = 0;
                PalletNoObj.value = "";
                LengthObj.value = "";
                WidthObj.value = "";
                HeightObj.value = "";
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                document.getElementById("<%=btnDel.ClientID %>").disabled = true;
                LengthObj.disabled = false;
                WidthObj.disabled = false;
                HeightObj.disabled = false; 
                return;
            }
            
            var curPalletNo = con.cells[0].innerText.trim();
            var curLength = con.cells[1].innerText.trim();
            var curWidth = con.cells[2].innerText.trim();
            var curHeight = con.cells[3].innerText.trim();
            //var curPalletSpec = con.cells[4].innerText.trim();

            //PalletSpecObj.value = curPalletSpec;
            PalletSpecObj.selectedIndex = 0;
            PalletNoObj.value = curPalletNo;
            LengthObj.value = curLength;
            WidthObj.value = curWidth;
            HeightObj.value = curHeight;
            LengthObj.disabled = false;
            WidthObj.disabled = false;
            HeightObj.disabled = false; 
            document.getElementById("<%=HiddenSelectedId.ClientID %>").value = con.cells[7].innerText.trim();
            if (curPalletNo == "") {
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            }
            else {
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
                document.getElementById("<%=btnDel.ClientID %>").disabled = false;
            }

        }
        function SetTextBox(txtLength, txtWidth, txtHeight) {
            if ((txtLength != "") && (float.Parse(txtLength) != 0.0)) {
                document.getElementById("<%=this.TextLength.ClientID%>").value = txtLength;
                document.getElementById("<%=this.TextLength.ClientID%>").disabled = true;
            }
            else {
                document.getElementById("<%=this.TextLength.ClientID%>").disabled = false;
            }
            if ((txtWidth != "") && (float.Parse(txtWidth) != 0.0)) {
                document.getElementById("<%=this.TextWidth.ClientID%>").value = txtWidth;
                document.getElementById("<%=this.TextWidth.ClientID%>").disabled = true;
            }
            else {
                document.getElementById("<%=this.TextWidth.ClientID%>").disabled = false;
            }
            if ((txtHeight != "") && (float.Parse(txtHeight) != 0.0)) {
                document.getElementById("<%=this.TextHeight.ClientID%>").value = txtHeight;
                document.getElementById("<%=this.TextHeight.ClientID%>").disabled = true;
            }
            else {
                document.getElementById("<%=this.TextHeight.ClientID%>").disabled = false;
            } 
            if ((txtLength =="") &&(txtWidth=="")&&(txtHeight==""))
                document.getElementById("<%=this.TextPalletNO.ClientID%>").focus();
            
        }
        function AddUpdateComplete(id) {
            //var PalletSpecObj = getMaintainPalletSpecCmbObj();
            var PalletNoObj = document.getElementById("<%=this.TextPalletNO.ClientID%>");
            var LengthObj = document.getElementById("<%=this.TextLength.ClientID%>");
            var WidthObj = document.getElementById("<%=this.TextWidth.ClientID%>");
            var HeightObj = document.getElementById("<%=this.TextHeight.ClientID%>");

            var gdObj = document.getElementById("<%=gd.ClientID %>");

            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[7].innerText == id) {
                    selectedRowIndex = i;
                }
            }

            if (selectedRowIndex < 0) {
                PalletSpecObj.selectedIndex = 0;
                PalletNoObj.value = "";
                LengthObj.value = "";
                WidthObj.value = "";
                HeightObj.value = "";
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                document.getElementById("<%=btnDel.ClientID %>").disabled = true;
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                //去掉标题行
                setGdHighLight(con);
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                ShowRowEditInfo(con);
            }

        }
        
        function OnKeyPress(obj) {
            var key = event.keyCode;
            if (key == 13)//enter
            {
                if (event.srcElement.id == "dSearch") {
                    var value = document.getElementById("dSearch").value.trim().toUpperCase();
                    if (value != "") {
                        findPalletNo(value, true);
                    }
                }
                event.keyCode = 0;
            }
        }

        function findPalletNo(searchValue, isNeedPromptAlert) {
            
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
                    alert(msgNotFound);
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
        
        function disposeTree(sender, args) {
            var elements = args.get_panelsUpdating();
            for (var i = elements.length - 1; i >= 0; i--) {
                var element = elements[i];
                var allnodes = element.getElementsByTagName('*'),
                length = allnodes.length;
                var nodes = new Array(length)
                for (var k = 0; k < length; k++) {
                    nodes[k] = allnodes[k];
                }
                for (var j = 0, l = nodes.length; j < l; j++) {
                    var node = nodes[j];
                    if (node.nodeType === 1) {
                        if (node.dispose && typeof (node.dispose) === "function") {
                            node.dispose();
                        }
                        else if (node.control && typeof (node.control.dispose) === "function") {
                            node.control.dispose();
                        }

                        var behaviors = node._behaviors;
                        if (behaviors) {
                            behaviors = Array.apply(null, behaviors);
                            for (var k = behaviors.length - 1; k >= 0; k--) {
                                behaviors[k].dispose();
                            }
                        }
                    }
                }
                element.innerHTML = "";
            }

        }
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(disposeTree);
    </script>

</asp:Content>

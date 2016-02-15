<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="ChepPallet.aspx.cs" Inherits="DataMaintain_ChepPallet"
    ValidateRequest="false" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline"
            Visible="true">
            <ContentTemplate>
                <div id="div1" class="iMes_div_MainTainDiv1">
                    <table width="100%" border="0">
                        <tr>
                            <td class="iMes_div_MainTainListLable">
                                <asp:Label ID="lbChepPalletList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                            </td>
                            <td width="42%">
                                <asp:TextBox ID="chepPalletList" runat="server" MaxLength="20" Width="56%" SkinId="textBoxSkin" 
                                    onkeypress='OnKeyPress(this)'></asp:TextBox>
                                <input type="hidden" id="Hidden1" runat="server" />
                            </td>
                            <td align="right">
                                <input type="button" id="btnDelete" onclick="if(clkButton())" class="iMes_button"
                                    runat="server" onserverclick="btnDelete_ServerClick" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" UpdateMode="Conditional" RenderMode="Inline"
            Visible="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline"
            Visible="true">
            <ContentTemplate>
                <div id="div2" style="height: 366px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="98%" RowStyle-Height="20"
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue="true" HighLightRowPosition="3"
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' OnRowDataBound="gd_RowDataBound"
                        EnableViewState="false">
                    </iMES:GridViewExt>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td width="16%">
                        <asp:Label ID="lbChepPalletNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="64%" align="left">
                        <asp:TextBox ID="chepPalletNo" runat="server" Width="60%" MaxLength="20" onkeypress="onkeypressForAdd()" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td align="right" width="20%">
                        <input type="button" id="btnAdd" onclick="if(clkButton())" class="iMes_button" runat="server"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnAdd_ServerClick" />
                    </td>
                </tr>
            </table>
        </div>
        <input type="hidden" id="oldChepPalletNo" runat="server" />
        <input type="hidden" id="itemId" runat="server" />
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
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

        //var familyObj;
        // var descriptionObj;

        var msg1 = "";
        var msg2 = "";
        var msg3 = "";
        var msg4 = "";
        var msg5 = "";


        window.onload = function() {
            //customerObj = getCustomerCmbObj();
            //customerObj.onchange = addNew;

            msg1 = "<%=pmtMessage1%>";
            msg2 = "<%=pmtMessage2%>";
            msg3 = "<%=pmtMessage3%>";
            msg4 = "<%=pmtMessage4%>";
            msg5 = "<%=pmtMessage5%>";

            document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";
            resetTableHeight();
            ShowRowEditInfo(null);

        };

        //设置表格的高度  
        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 55;
            var marginValue = 12;
            var tableHeigth = 300;
            var a = document.body.parentElement.offsetHeight;
            try {
                var tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
            }
            catch (e) {
                //ignore
            }
            //为了使表格下面有写空隙
            var extDivHeight = tableHeigth + marginValue;

            document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
            div2.style.height = extDivHeight + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";

        }
        //设置高亮行
        var iSelectedRowIndex = null;
        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }

        //按键事件
        function OnKeyPress(obj) {

            var key = event.keyCode;
            if (key == 13)//enter
            {
                if (event.srcElement.id == "<%=chepPalletList.ClientID %>") {
                    var value = document.getElementById("<%=chepPalletList.ClientID %>").value.trim().toUpperCase();
                    if (value != "") {
                        findChepPalletInfo(value, true);
                    }

                }
            }

        }


        //在列表里查找List输入项
        function findChepPalletInfo(searchValue, isNeedPromptAlert) {
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
                    alert(msg5);
                    //                alert("找不到列表中与Family栏位匹配的项");     
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
        //所有button的前台检查
        function clkButton() {
            switch (event.srcElement.id) {
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
            }
            ShowWait();
            return true;
        }
        //delete的前台检查
        function clkDelete() {
            ShowInfo("");
            var gdObj = document.getElementById("<%=gd.ClientID %>")
            var curIndex = gdObj.index;
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            if (curIndex >= recordCount) {
                //            alert("Please select one row!");   //2
                alert(msg1);
                return false;
            }

            //         var ret = confirm("Do you really want to delete this item?");  //3
            var ret = confirm(msg2);  //3
            if (!ret) {
                return false;
            }
            return true;

        }
        //后台执行完删除操作后，前台显示
        function DeleteComplete() {
            ShowRowEditInfo(null);
        }
        //Add Button的前台检查
        function clkAdd() {

            ShowInfo("");
            var chepPalletNo = document.getElementById("<%=chepPalletNo.ClientID %>").value.toString();
            if (chepPalletNo.trim() == "") {
                //           alert("Please input [ChepPalletNo] first!!");  //4
                alert(msg3);
                document.getElementById("<%=chepPalletNo.ClientID %>").focus();
                return false;
            }


            if (chepPalletNo.length != 10 || chepPalletNo.indexOf("3") == -1 || chepPalletNo.indexOf("3") != 0) {
                //alert("This Chep Error!!");  //4
                alert(msg4);
                document.getElementById("<%=chepPalletNo.ClientID %>").focus();
                return false;
            }
            return true;
        }
        //单击行事件
        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);

        }
        //点击高亮后，显示行信息
        function ShowRowEditInfo(con) {

            if (con == null) {
                document.getElementById("<%=chepPalletNo.ClientID %>").value = "";
                document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
                return;
            }

            var chepPalletNo = con.cells[0].innerText.trim();
            document.getElementById("<%=chepPalletNo.ClientID %>").value = chepPalletNo;
            document.getElementById("<%=itemId.ClientID %>").value = con.cells[4].innerText.trim(); ;
            if (chepPalletNo == "") {
                document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            }
            else {
                document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
            }
        }

        //添加和更新操作完成后的前台设置
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

            }

        }

        function onkeypressForAdd() {
            var keyCode = event.keyCode;
            if (keyCode == 13) {
                AddChepPallet();
            }
        }

        //ChepPalletNo的blur事件
        function AddChepPallet() {
            document.getElementById("<%=btnAdd.ClientID %>").focus();
            document.getElementById("<%=btnAdd.ClientID%>").click();
            //ShowWait();
        }
        function DealHideWait() {
            HideWait();
        }

        function NoMatchFamily() {
            //         alert("Cant find that match family.");    //5 
            alert(msg5);
            return;
        }
   
    </script>

</asp:Content>

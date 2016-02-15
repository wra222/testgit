<%--
/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for FamilyInfo Page
 *             
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/05/18
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/05/18            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-18  Kaisheng             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="FamilyInfo.aspx.cs" Inherits="DataMaintain_FamilyInfo"
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
                            <td style="width: 80%">
                                <asp:Label ID="FamilyInfoList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                            </td>
                            <td style="width: 20%" align="right">
                                <button id="btnDelete" runat="server" onclick="if(clkButton())" class="iMes_button"
                                    onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                                    onserverclick="btnDelete_ServerClick">
                                </button>
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
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline"
            Visible="true">
            <ContentTemplate>
                <div id="div2" style="height: 366px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="130%" RowStyle-Height="20"
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
                    <td style="width: 1%" />
                    <td style="width: 10%">
                        <asp:Label ID="lbFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="30%">
                         <iMESMaintain:CmbFamilyForMaintain ID="ddlFamily" runat="server" Height="30px" Width="98%" FirstNullItem="false" />
                    </td>
                    <td style="width: 3%" />
                    <td style="width: 10%">
                        <asp:Label ID="lbName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="30%">
                        
                        <asp:DropDownList ID="cmbFamilyInfoName" runat="server"></asp:DropDownList>
                    </td>
                    <td align="right">
                        <button id="btnAdd" runat="server" onclick="if(clkButton())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnAdd_ServerClick">
                        </button>
                    </td>
                </tr>
                <tr>
                    <td style="width: 1%;" />
                    <td style="width: 10%;">
                        <asp:Label ID="lbValue" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="txtValue" runat="server" MaxLength="200" Width="96%" SkinId="textBoxSkin" 
                            onkeypress='checkCodePress(this)' onpaste="return false"></asp:TextBox>
                    </td>
                    <td style="width: 3%;" />
                    <td style="width: 10%;">
                        <asp:Label ID="lbDescr" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="txtDescr" runat="server" MaxLength="80" Width="96%" SkinId="textBoxSkin" 
                            onkeypress='checkCodePress(this)' onpaste="return false"></asp:TextBox>
                    </td>
                    <td align="right">
                        <button id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnSave_ServerClick">
                        </button>
                        <input type="hidden" id="oldFullQtyId" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <input type="hidden" id="hiddenUserName" runat="server" />
        <input type="hidden" id="itemId" runat="server" />
        <input type="hidden" id="selectedRowsIndex" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="hidFamilyInfoName" runat="server" />
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
        var msg6 = "";
        var msg8 = "";
        var msg9 = "";
        var msg10 = "";
        var msg11 = "";
        var msg16 = "";

        var currentFamily = "";
        var currentName = "";
        window.onload = function() {
            //customerObj = getCustomerCmbObj();
            //customerObj.onchange = addNew;

            msg1 = "<%=pmtMessage1%>";
            msg2 = "<%=pmtMessage2%>";
            msg3 = "<%=pmtMessage3%>";
            msg4 = "<%=pmtMessage4%>";
            msg5 = "<%=pmtMessage5%>";
            msg6 = "<%=pmtMessage6%>";
            msg8 = "<%=pmtMessage8%>";
            msg9 = "<%=pmtMessage9%>";
            msg10 = "<%=pmtMessage10%>";
            msg11 = "<%=pmtMessage11%>";
            msg12 = "<%=pmtMessage12%>";
            msg13 = "<%=pmtMessage13%>";
            msg14 = "<%=pmtMessage14%>";
            msg15 = "<%=pmtMessage15%>";
            msg16 = "<%=pmtMessage16 %>";

            document.getElementById("<%=hiddenUserName.ClientID %>").value = "<%=userName%>";
            //设置表格的高度  
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
                case "<%=btnSave.ClientID %>":
                    if (clkSave() == false) {
                        return false;
                    }
                    break;
            }
            ShowWait();
            return true;
        }

        //   删除操作的前台检查
        function clkDelete() {
            ShowInfo("");
            var gdObj = document.getElementById("<%=gd.ClientID %>")
            var curIndex = gdObj.index;
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            if (curIndex >= recordCount) {
                //            alert("Please select one row!");   //2
                alert(msg5);
                return false;
            }

            //         var ret = confirm("Do you really want to delete this item?");  //3
            var ret = confirm(msg6);  //3
            if (!ret) {
                return false;
            }
            return true;

        }

        function DeleteComplete() {
            ShowRowEditInfo(null);
        }

        function clkAdd() {

            return checkPalletQty("Family+Name");

        }

        function clkSave() {

            return checkPalletQty("ALL");
        }

        //GridView的单击行事件
        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);

        }

        //判断输入项是否符合标准
        function checkPalletQty(strType) {
            //var ddlFamily = document.getElementById("<%=ddlFamily.ClientID %>").value;
            var txtName = document.getElementById("<%=cmbFamilyInfoName.ClientID %>").value;
            var txtValue = document.getElementById("<%=txtValue.ClientID %>").value;
            var txtDescr = document.getElementById("<%=txtDescr.ClientID %>").value;
            
            if (getFamilyCmbObj().value.trim() == "") {
                //         alert("Please input [ddlFamily] first!!");
                alert(msg1);
                //document.getElementById("<%=ddlFamily.ClientID %>").focus();
                getFamilyCmbObj().focus();
                return false;
            }
            if (txtName.trim() == "") {
                //         alert("Please input [txtName] first!!");
                alert(msg2);
                document.getElementById("<%=cmbFamilyInfoName.ClientID %>").focus();
                return false;
            }
            if (txtValue.trim() == "") {
                //         alert("Please input [txtValue] first!!");
                alert(msg3);
                document.getElementById("<%=txtValue.ClientID %>").focus();
                return false;
            }
            document.getElementById("<%=hidFamilyInfoName.ClientID %>").value = document.getElementById("<%=cmbFamilyInfoName.ClientID %>").value;
            //DEBUG ITC1361-0141  
            /*if (txtDescr.trim() == "") {
                //         alert("Please input [txtDescr] first!!");
                alert(msg4);
                document.getElementById("<%=txtDescr.ClientID %>").focus();
                return false;
            }*/
            if (CheckInTable(strType, getFamilyCmbObj().value.trim(),txtName.trim(), txtValue.trim(), txtDescr.trim())) {
                alert(msg16);
                getFamilyCmbObj().focus();
                return false;
            }

                return true;
        }

        //显示行信息
        function ShowRowEditInfo(con) {

            if (con == null) {
                //document.getElementById("<%=ddlFamily.ClientID %>").value = "";
                
                document.getElementById("<%=cmbFamilyInfoName.ClientID %>").value = "";
                document.getElementById("<%=txtValue.ClientID %>").value = "";
                document.getElementById("<%=txtDescr.ClientID %>").value = "";
                //document.getElementById("<%=oldFullQtyId.ClientID %>").value = "";
                document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
                currentFamily = "";
                currentName = "";
                
                return;
            }

            var strfamilyinfo = con.cells[0].innerText.trim();
            getFamilyCmbObj().value = strfamilyinfo;
            //document.getElementById("<%=ddlFamily.ClientID %>").value = curfullQtyId;
            
            document.getElementById("<%=cmbFamilyInfoName.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=txtValue.ClientID %>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=txtDescr.ClientID %>").value = con.cells[3].innerText.trim();
            document.getElementById("<%=itemId.ClientID %>").value = con.cells[7].innerText.trim();
            currentFamily = strfamilyinfo;
            currentName = con.cells[1].innerText.trim();
            
            //保存当前信息的主键
            //document.getElementById("<%=oldFullQtyId.ClientID %>").value = curfullQtyId;
            if (strfamilyinfo == "") {
                document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            }
            else {
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
                document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
            }
        }

        //添加和更新操作完成之后的前天设置
        function AddUpdateComplete(fullQty) {

            var gdObj = document.getElementById("<%=gd.ClientID %>");

            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[7].innerText == fullQty) {
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

        //输入框的输入限制，只能输入数字
        function checkCodePress(obj) {
            //var key = event.keyCode;
            //if (!(key >= 48 && key <= 57)) {
            //    event.keyCode = 0;
            //}
        }
        function DealHideWait() {
            HideWait();
        }

        function NoMatchFamily() {
            //         alert("Cant find that match family.");    //5 
            alert(msg5);
            return;
        }

        function CheckInTable(strType, sFamily, sName, sValue, sDescr) {
            var tbl = "<%=gd.ClientID %>";
            var table = document.getElementById(tbl);
            var subFindFlag = false;
            for (var i = 1; i < table.rows.length; i++) {
                if ((table.rows[i].cells[0].innerText.trim() == "") &&
                    (table.rows[i].cells[1].innerText.trim() == ""))
                    break;
                
                
                if ((table.rows[i].cells[0].innerText.trim() == sFamily) &&
                    (table.rows[i].cells[1].innerText.trim() == sName)){// &&
                    //(table.rows[i].cells[2].innerText.trim() == sValue) &&
                    //(table.rows[i].cells[3].innerText.trim() == sDescr)) {
                    if (strType != "ALL") {
                        subFindFlag = true;
                    }
                    else {
                        //currentFamily = strfamilyinfo;
                        //currentName = con.cells[1].innerText.trim();
                        if ((sFamily ==currentFamily)&&(sName==currentName)) {
                            if ((table.rows[i].cells[2].innerText.trim() == sValue) &&(table.rows[i].cells[3].innerText.trim() == sDescr)) {
                                subFindFlag = true;
                            }
                        }
                        else {
                            subFindFlag = true;
                        }
            
                    }
                    
                    break;
                }
            }
            return subFindFlag;
        }
      
   
    </script>

</asp:Content>


<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="ModelAssemblyCode.aspx.cs" Inherits="DataMaintain_ModelAssemblyCode" Title="ModelAssemblyCode"
    EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="iMESContent" runat="Server">
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
    </style>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td width="10%">
                        &nbsp;
                    </td>
                    <td width="15%">
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                            Visible="true">
                        </asp:UpdatePanel>
                    </td>
                    <td width="10%">
                        &nbsp;
                    </td>
                    <td width="15%">
                    </td>
                    <td width="18%">
                    </td>
                    <td width="32%" align="right">
                        &nbsp;<input type="button" id="btnDel" runat="server" class="iMes_button" onclick="if(clkDelete())"
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
        </asp:UpdatePanel>
        <div id="div2">
            <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                Visible="true">
                <ContentTemplate>
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%"
                        RowStyle-Height="20" GvExtWidth="99%" AutoHighlightScrollByValue="true" HighLightRowPosition="3"
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' OnRowDataBound="gd_RowDataBound"
                        EnableViewState="false" Style="top: 0px; left: 30px">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <p>
        </p>
        <div id="div4">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width: 110px;">
                        <asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt">Model</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:TextBox ID="TXTModel" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 110px;">
                        <asp:Label ID="Label2" runat="server" CssClass="iMes_label_13pt">AssemblyCode</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:TextBox ID="TXTAssemblycode" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 110px;">
                        <asp:Label ID="Label3" runat="server" CssClass="iMes_label_13pt">Revision</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:TextBox ID="TXTRev" runat="server"></asp:TextBox>
                    </td>
                    <td align="right" style="width: 3%;">
                        <input id="btnAdd" runat="server" class="iMes_button" onclick="if(clkAdd())" onmouseout="this.className='iMes_button_onmouseout'"
                            onmouseover="this.className='iMes_button_onmouseover'" onserverclick="btnAdd_ServerClick"
                            type="button" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 110px;">
                        <asp:Label ID="Label4" runat="server" CssClass="iMes_label_13pt">SupplierCode</asp:Label>
                    </td>
                    <td width="180px">
                       <asp:TextBox ID="TXTSuppliercode" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 110px;">
                        <asp:Label ID="Label5" runat="server" CssClass="iMes_label_13pt">Remark</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:TextBox ID="TXTRemark" runat="server"></asp:TextBox>
                    </td>
                   <td></td>
                   <td></td>
                    
                    <td align="right">
                        <input type="button" id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnSave_ServerClick" />
                        <input type="hidden" id="Hidden1" runat="server" />
                        <input type="hidden" id="Hidden2" runat="server" />
                        <input type="hidden" id="Hidden3" runat="server" />
                        <input type="hidden" id="Hidden4" runat="server" />
                    </td>
                </tr>
               
                
            </table>
        </div>
        <input type="hidden" id="hidmodel" runat="server" />
        <input type="hidden" id="hidAssemblyCode" runat="server" />
		<input type="hidden" id="hidSelectedAstCode" runat="server" />
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="HiddenCustomerName" runat="server" />
        <input type="hidden" id="hidAssignAstSNStation" runat="server" />
    
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
        var _Model = "";
        var _AssemblyCode = "";
       
        function SetGrDivHeight() {
            document.getElementById("div_<%=gd.ClientID %>").style.height = "300px";
        }
       
        function GetDroText(objId) {
            var obj = document.getElementById(objId);
            return obj.options[obj.selectedIndex].text;
        }

        window.onload = function() {
            ShowRowEditInfo(null);
            SetGrDivHeight();
			
        }
        function DisableDeleteBtn() {
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
        }
        function clkDelete() {
            ShowInfo("");
            if (_Model == "" || _AssemblyCode == "") {
                alert("Please select row to delete");
                return false;
            }
            document.getElementById("<%=hidmodel.ClientID %>").value = _Model;
            document.getElementById("<%=hidAssemblyCode.ClientID %>").value = _AssemblyCode;
            var ret = confirm("Do you really want to delete this item?");  //3
            if (!ret) {
                return false;
            }
            ShowWait();
            return true;
        }

        function clkSave() {
            if (document.getElementById("<%=TXTModel.ClientID %>") == "") {
                alert("Please select Model!");
                return false;
            }
            if (!checkinput()) {
                return false;
            }
            if (CheckAstTypeAndCode()) {
                        alert("Duplicate AstCode!");
                        return false;
                 }
            document.getElementById("<%=hidmodel.ClientID %>").value = _Model;
            document.getElementById("<%=hidAssemblyCode.ClientID %>").value = _AssemblyCode;
            ShowWait();
            return true;
        }
        function checkinput() {
            var model = document.getElementById("<%=TXTModel.ClientID %>").value;
            var Assemblycode = document.getElementById("<%=TXTAssemblycode.ClientID %>").value;
            var Rev = document.getElementById("<%=TXTRev.ClientID %>").value;
            var Suppliercodel = document.getElementById("<%=TXTSuppliercode.ClientID %>").value;
            var Remarkl = document.getElementById("<%=TXTRemark.ClientID %>").value;
            if (model == "" || Assemblycode == "" || Rev == "" || Suppliercodel == "") {
                alert("Please Input ALL!!");
                return false;
            }
            if (model.length != 12) {
                alert("請輸入正確的機型!");
                return false;
            }
            if (Assemblycode.length != 5) {
                alert("請輸入正確的Assemblycode!");
                return false;
            }
            if (Rev.length != 2) {
                alert("請輸入正確的Rev!");
                return false;
            }
            return true;
        }
        function clkAdd() {
            if (!checkinput()) {
                return false;
            }
              var b = CheckAdd();
            if (b) {
                ShowWait();
            }
            return b;
        }
        
        function CheckAdd() {
            if (CheckAstTypeAndCode()) {
                alert("Duplicate Model!");
                return false;
            }
            
            return true;
        }
        function CheckAstTypeAndCode() {
           
            var _t = document.getElementById("<%=TXTModel.ClientID %>").value;
            var s = "<%=gd.ClientID %>";
            var tbl = $("#" + s);
            var r = false;
            tbl.find(" tr:gt(0)").each(function() {
            if ($(this).find('td').eq(1).text() == _t) {
                    r = true;
                    return false;
                }
            });
            return r;

        }
        function checkAssetRangeInfo() {

            return true;
        }
        var iSelectedRowIndex = null;
        function setGdHighLight(con) {
            var idx = con.sectionRowIndex - 1;
         //   if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(idx, 10))) {
                //去掉过去高亮行
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            //设置当前高亮行   
           // setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            setRowSelectedOrNotSelectedByIndex(idx, true, "<%=gd.ClientID %>");
           //记住当前高亮行
//            iSelectedRowIndex = parseInt(con.index, 10);
            iSelectedRowIndex = parseInt(idx, 10);
        }

        function setDropDownList(elementRef, valueToSetTo) {
            var isFound = false;
            for (var i = 0; i < elementRef.options.length; i++) {
                if (elementRef.options[i].value == valueToSetTo) {
                    elementRef.options[i].selected = true;
                    isFound = true;
                }
            }
            if (isFound == false)
                elementRef.options[0].selected = true;
        }

		function SetFirstDefault(obj) {
            obj.options[0].selected = true;
        }
        function Reset() {


            document.getElementById("<%=TXTModel.ClientID %>").value = "";
            document.getElementById("<%=TXTAssemblycode.ClientID %>").value = "";
            document.getElementById("<%=TXTRev.ClientID %>").value = "";
            document.getElementById("<%=TXTSuppliercode.ClientID %>").value = "";
            document.getElementById("<%=TXTRemark.ClientID %>").value =  "";
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
           document.getElementById("<%=btnDel.ClientID %>").disabled = true;
        }
        function clickTable(con) {
            _Model = con.cells[0].innerText.trim();
            _AssemblyCode = con.cells[1].innerText.trim();
            if (_Model == "" || _AssemblyCode == "") {
                Reset();
                var selectedRowIndex = con.index;
                setGdHighLight(con);
                return;
            }
            var model = con.cells[0].innerText.trim();
            var Assemblycode = con.cells[1].innerText.trim();
            var Rev = con.cells[2].innerText.trim();
            var Suppliercodel = con.cells[3].innerText.trim();
            var Remarkl = con.cells[4].innerText.trim();


            document.getElementById("<%=TXTModel.ClientID %>").value = model;
            document.getElementById("<%=TXTAssemblycode.ClientID %>").value = Assemblycode;
            document.getElementById("<%=TXTRev.ClientID %>").value = Rev;
            document.getElementById("<%=TXTSuppliercode.ClientID %>").value = Suppliercodel;
            document.getElementById("<%=TXTRemark.ClientID %>").value = Remarkl;
         
            
           
         
          //  var selectedRowIndex = con.index;
            var selectedRowIndex = con.sectionRowIndex - 1;
            setGdHighLight(con);
            ShowRowEditInfo(con);

        }

        function ShowRowEditInfo(con) {
            if (con == null) {
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                document.getElementById("<%=btnDel.ClientID %>").disabled = true;
                return;
            }
            if (_Model == "" || _AssemblyCode == "") {
                document.getElementById("<%=btnDel.ClientID %>").disabled = true;
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            }
            else {
                document.getElementById("<%=btnDel.ClientID %>").disabled = false;
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
            }

        }
        function resetTableHeight() {
            return;
            //动态调整表格的高度
            //        var adjustValue=70;     
            //        var marginValue=10;  
            //        var tableHeigth=170;

            //        
            //        try{
            //            tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
            //        }
            //        catch(e){
            //            //ignore
            //        }                
            //        //为了使表格下面有写空隙
            //        var extDivHeight=tableHeigth+marginValue;
            //       
            //        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
            //        //alert(document.getElementById("div_<%=gd.ClientID %>").style.height)
            //        
            //        div2.style.height=extDivHeight+"px";

        }


        //  Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(disposeTree);


    </script>

</asp:Content>



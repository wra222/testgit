<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="AstDefine.aspx.cs" Inherits="DataMaintain_AstDefine" Title="AstDefine"
    EnableEventValidation="false" %>

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
                        <asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt">AstType</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="droAstType" runat="server" Height="25px" Width="170px">
                            <asp:ListItem Selected="True">AT</asp:ListItem>
                            <asp:ListItem>PP</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 110px;">
                        <asp:Label ID="Label2" runat="server" CssClass="iMes_label_13pt">AstCode</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="droAstCode" runat="server" Height="25px" Width="170px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 110px;">
                        <asp:Label ID="Label3" runat="server" CssClass="iMes_label_13pt">AstLocation</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="droAstLocation" runat="server" Height="25px" Width="170px">
                            <asp:ListItem Selected="True">chassis</asp:ListItem>
                            <asp:ListItem>shipping</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right" style="width: 3%;">
                        <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnAdd_ServerClick" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 110px;">
                        <asp:Label ID="Label4" runat="server" CssClass="iMes_label_13pt">NeedAssignAstSN</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="droNeedAssignAstSN" runat="server" Height="25px" Width="170px">
                            <asp:ListItem Selected="True">Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 110px;">
                        <asp:Label ID="Label5" runat="server" CssClass="iMes_label_13pt">AssignAstSNStation</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="droAssignAstSNStation" runat="server" Height="25px" Width="170px">
                            
                        </asp:DropDownList>
                    </td>
                    <td style="width: 110px;">
                        <asp:Label ID="lblCombineStation" runat="server" CssClass="iMes_label_13pt">CombineStation</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="droCombineStation" runat="server" Height="25px" Width="170px">
                           
                        </asp:DropDownList>
                    </td>
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
                <tr>
                    <td style="width: 110px;">
                        <asp:Label ID="Label6" runat="server" CssClass="iMes_label_13pt">HasCDSIAst</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="droHasCDSIAst" runat="server" Height="25px" Width="170px">
                            <asp:ListItem Selected="True">Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 110px;">
                        <asp:Label ID="Label7" runat="server" CssClass="iMes_label_13pt">NeedPrint</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="droNeedPrint" runat="server" Height="25px" Width="170px">
                            <asp:ListItem Selected="True">Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 110px;">
                        <asp:Label ID="Label8" runat="server" CssClass="iMes_label_13pt">NeedScanSN</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="droNeedScanSN" runat="server" Height="25px" Width="170px">
                            <asp:ListItem Selected="True">Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                    </td>
                </tr>
                <tr>
                    <td style="width: 110px;">
                        <asp:Label ID="LabelCheckUnique" runat="server" CssClass="iMes_label_13pt">CheckUnique</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="droCheckUnique" runat="server" Height="25px" Width="170px">
                            <asp:ListItem Selected="True">Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
					<td style="width: 110px;">
                        <asp:Label ID="Label9" runat="server" CssClass="iMes_label_13pt">HasUPSAst</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="dropHasUPSAst" runat="server" Height="25px" Width="170px">
                            <asp:ListItem Selected="True">N</asp:ListItem>
                            <asp:ListItem>Y</asp:ListItem>
                        </asp:DropDownList>
                    </td>
					<td style="width: 110px;">
                        <asp:Label ID="Label11" runat="server" CssClass="iMes_label_13pt">NeedBindUPSPO</asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="dropNeedBindUPSPO" runat="server" Height="25px" Width="170px">
                            <asp:ListItem Selected="True">N</asp:ListItem>
                            <asp:ListItem>Y</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td style="width: 110px;">
                        <asp:Label ID="Label10" runat="server" CssClass="iMes_label_13pt">Comment</asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtComment" runat="server" MaxLength="20" Width="70%" SkinID="textBoxSkin"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <input type="hidden" id="hidAstType" runat="server" />
        <input type="hidden" id="hidAstCode" runat="server" />
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
        var _astType = "";
        var _astCode = "";
        var arrAssignAstSNStation = new Array('AT', '65','50');
        function ResetSelectValue() {
            _astCode = "";
            _astType = "";
        }
        function SetGrDivHeight() {
            document.getElementById("div_<%=gd.ClientID %>").style.height = "600px";
        }
        function ChanegNeedAssignAstSN(obj) {
            // [AssignAstSNStation] [varchar](16) Null default(''), --假如NeedAssignSN=Y則是AT/65,否則空白
            SetAssignAstSNStation(GetDroText(obj.id));
        }
        function GetDroText(objId) {
            var obj = document.getElementById(objId);
            return obj.options[obj.selectedIndex].text;
        }
        function SetAssignAstSNStation(c) {
            var obj = document.getElementById("<%=droAssignAstSNStation.ClientID%>");
            var len = obj.options.length;
            for (i = 0; i < len; i++) {
                obj.remove(0); //It is 0 (zero) intentionally
            }
            if (c == "Y") {
                for (var key in arrAssignAstSNStation) {
                    var opt = document.createElement("option");
                    opt.text = arrAssignAstSNStation[key];
                    opt.value = arrAssignAstSNStation[key];
                    document.getElementById("<%=droAssignAstSNStation.ClientID%>").options.add(opt);

                }
            }
            else {
                var opt = document.createElement("option");
                opt.text = '';
                opt.value = '';
                document.getElementById("<%=droAssignAstSNStation.ClientID%>").options.add(opt);
            }
        }


        window.onload = function() {
            ShowRowEditInfo(null);
            SetGrDivHeight();
			document.getElementById('<%=droAstType.ClientID %>').onchange = AstTypeOnChange;
			AstTypeOnChange();
        }
        function DisableDeleteBtn() {
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
        }
        function clkDelete() {
            ShowInfo("");
            if (_astType == "" || _astCode == "") {
                alert("Please select row to delete");
                return false;
            }
            document.getElementById("<%=hidAstType.ClientID %>").value = _astType;
            document.getElementById("<%=hidAstCode.ClientID %>").value = _astCode;
            var ret = confirm("Do you really want to delete this item?");  //3
            if (!ret) {
                return false;
            }
            ShowWait();
            return true;
        }
        function CheckCombineStation() {
            var _c = GetDroText("<%=droCombineStation.ClientID%>");
            var _np = GetDroText("<%=droNeedPrint.ClientID%>");
            var _ns = GetDroText("<%=droNeedScanSN.ClientID%>");
            if(_c=="" && (_np!="N" || _ns!="N"))
            {
                alert("NeedPrint and NeedScanSN must be N when not set CombineStation");
                return false;
            }
            return true;
        //    if( GetDroText("<%=droCombineStation.ClientID%>")=="" && (
        
        }
        function clkSave() {
            if (GetDroText("<%=droAstCode.ClientID%>") == "") {
                alert("Please select AstCode!");
                return false;
            }
            var _b = CheckCombineStation();
            if (!_b)
            {return false;}
            var _selAstType = GetDroText("<%=droAstType.ClientID%>");
            var _selAstCode = GetDroText("<%=droAstCode.ClientID%>");
            if (_astCode != "" || _astType != "") {
                if (_selAstType != _astType || _selAstCode != _astCode) {
                    if (CheckAstTypeAndCode()) {
                        alert("Duplicate AstCode!");
                        return false;
                    }
                }

            }
            document.getElementById("<%=hidAssignAstSNStation.ClientID %>").value = GetDroText("<%=droAssignAstSNStation.ClientID%>");
            //GetDroText("<%=droAstCode.ClientID%>")
            if (_astCode == "" || _astType == "") {
                document.getElementById("<%=hidAstType.ClientID %>").value = _selAstType;
                document.getElementById("<%=hidAstCode.ClientID %>").value = _selAstCode;

            }
            else {
                document.getElementById("<%=hidAstType.ClientID %>").value = _astType;
                document.getElementById("<%=hidAstCode.ClientID %>").value = _astCode;
            }
			document.getElementById("<%=hidSelectedAstCode.ClientID %>").value = _selAstCode;
            ShowWait();
            return true;
        }
        function clkAdd() {
            var _b = CheckCombineStation();
            if (!_b)
            { return false; }
            document.getElementById("<%=hidAssignAstSNStation.ClientID %>").value = GetDroText("<%=droAssignAstSNStation.ClientID%>");
            var b = CheckAdd();
            if (b) {
                ShowWait();
            }
			var _selAstCode = GetDroText("<%=droAstCode.ClientID%>");
			document.getElementById("<%=hidSelectedAstCode.ClientID %>").value = _selAstCode;
            return b;
        }
        function CheckAdd() {
            if (CheckAstTypeAndCode()) {
                alert("Duplicate AstCode!");
                return false;
            }
            if (GetDroText("<%=droAstCode.ClientID%>") == "") {
                alert("Please select AstCode!");
                return false;
            }
            return true;
        }
        function CheckAstTypeAndCode() {
            var _t = GetDroText("<%=droAstType.ClientID%>");
            var _c = GetDroText("<%=droAstCode.ClientID%>");

            var s = "<%=gd.ClientID %>";
            var tbl = $("#" + s);
            var r = false;
            tbl.find(" tr:gt(0)").each(function() {
                if ($(this).find('td').eq(1).text() == _c) {
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
		function setAstCode(astType){
			var drpAstCode=document.getElementById('<%=droAstCode.ClientID %>');
			while(drpAstCode.options.length > 0){
				drpAstCode.remove(0);
			}
			drpAstCode.options.add(new Option("", ""));
			var codes=("AT"==astType)? "<%=AstCodeAT%>" : "<%=AstCodePP%>";
			var lstCodes=codes.split(",");
			for(var i=0; i<lstCodes.length; i++) {
				if ("" != lstCodes[i]){
					drpAstCode.options.add(new Option(lstCodes[i], lstCodes[i]));
				}
			}
		}
        function AstTypeOnChange(){
			setAstCode(GetDroText("<%=droAstType.ClientID%>"));
		}
		function SetFirstDefault(obj) {
            obj.options[0].selected = true;
        }
        function Reset() {

            SetFirstDefault(document.getElementById('<%=droAstType.ClientID %>'));
			AstTypeOnChange();
            setDropDownList(document.getElementById('<%=droAstCode.ClientID %>'));
            setDropDownList(document.getElementById('<%=droAstLocation.ClientID %>'));
            SetAssignAstSNStation("Y");
            // setDropDownList(document.getElementById('<%=droAstCode.ClientID %>'), astCode);
            setDropDownList(document.getElementById('<%=droHasCDSIAst.ClientID %>'));
            setDropDownList(document.getElementById('<%=droNeedPrint.ClientID %>'));
            setDropDownList(document.getElementById('<%=droNeedScanSN.ClientID %>'));
            document.getElementById('<%=txtComment.ClientID %>').value = "";
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
        }
        function clickTable(con) {

            _astType = con.cells[0].innerText.trim();
            _astCode = con.cells[1].innerText.trim();
            if (_astType == "" || _astCode == "") {
                Reset();
                var selectedRowIndex = con.index;
                setGdHighLight(con);
                return;
            }
            var astType = con.cells[0].innerText.trim();
            var astCode = con.cells[1].innerText.trim();
            var astLocation = con.cells[2].innerText.trim();
            var needAssignAstSN = con.cells[3].innerText.trim();
            var assignAstSNStation = con.cells[4].innerText.trim();
            var combineStation = con.cells[5].innerText.trim();
            var hasCDSIAst = con.cells[6].innerText.trim();
            var needPrint = con.cells[7].innerText.trim();
            var needScanSN = con.cells[8].innerText.trim();
            var comment = con.cells[9].innerText.trim();
            var checkUnique = con.cells[10].innerText.trim();
            var Hasupsast = con.cells[11].innerText.trim();
            var needbindupspo = con.cells[12].innerText.trim();
            SetAssignAstSNStation(needAssignAstSN);
            setDropDownList(document.getElementById('<%=droAstType.ClientID %>'), astType);
			AstTypeOnChange();
            setDropDownList(document.getElementById('<%=droAstCode.ClientID %>'), astCode);
            setDropDownList(document.getElementById('<%=droAstLocation.ClientID %>'), astLocation);
            setDropDownList(document.getElementById('<%=droNeedAssignAstSN.ClientID %>'), needAssignAstSN);
            setDropDownList(document.getElementById('<%=droAssignAstSNStation.ClientID %>'), assignAstSNStation);
            setDropDownList(document.getElementById('<%=droCombineStation.ClientID %>'), combineStation);
      
            //droCombineStation setDropDownList(document.getElementById('<%=droAstCode.ClientID %>'), astCode);
            setDropDownList(document.getElementById('<%=droHasCDSIAst.ClientID %>'), hasCDSIAst);
            setDropDownList(document.getElementById('<%=droNeedPrint.ClientID %>'), needPrint);
            setDropDownList(document.getElementById('<%=droNeedScanSN.ClientID %>'), needScanSN);
            setDropDownList(document.getElementById('<%=droCheckUnique.ClientID %>'), checkUnique);
            setDropDownList(document.getElementById('<%=dropHasUPSAst.ClientID %>'), Hasupsast);
            setDropDownList(document.getElementById('<%=dropNeedBindUPSPO.ClientID %>'), needbindupspo);
            document.getElementById('<%=txtComment.ClientID %>').value = comment;
         
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
            if (_astCode == "" || _astType == "") {
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

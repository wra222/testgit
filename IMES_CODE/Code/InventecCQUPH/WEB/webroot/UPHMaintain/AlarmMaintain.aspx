﻿<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="AlarmMaintain.aspx.cs" Inherits="webroot_UPHMaintain_AlarmMaintain" EnableEventValidation = "false"  Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
 <style></style>
<div >
<div id="TT" style="position:absolute;left:40%;top:1.5%; font-size:xx-large;height:20px">Alarm Maintain</div>
    <table id="TB" style="height: 400px; width: 100%; position:relative; top:30px; "  
        cellpadding="0">
<tr>
        <td style="width:12%"></td>
         <td nowrap="nowrap">
                <asp:Label ID="Label1" runat="server" Text="制程:" Font-Size="17"></asp:Label>
         </td>
         <td style="height: 40px; width: 305px;" >
      <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:DropDownList ID="Process" name="Process" runat="server" Width="353px" 
                 Height="25px" AutoPostBack="True" onselectedindexchanged="Process_SelectedIndexChanged">
            </asp:DropDownList>
         </ContentTemplate>
       </asp:UpdatePanel>
        </td>
        <td style="width:5%"></td>
        <td>
            <asp:Label ID="Label4" runat="server" Text="时间:" Font-Size="17"></asp:Label>
         </td>
        <td style="height: 40px">
            <input runat="server"  name="BeginTime" type="text" 
                style="width:158px;height:20px"   onclick="_SetTime(this)"  id="BeginTime" 
                readonly="readonly"/>&nbsp 
                <label>to</label>
            <input runat="server" name="EndTime" type="text"  
                style="width:158px;height:20px" onclick="_SetTime(this)"   id="EndTime" 
                readonly="readonly"/>
        </td>
</tr>
<tr>
        <td></td>
        <td style="height: 40px" >
            <asp:Label ID="Label2" runat="server" Text="班次:" Font-Size="17"></asp:Label>
        </td>
        <td style="height: 40px; width: 305px;">
       <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <asp:DropDownList ID="Shift" name="Shift" runat="server" Width="353px" 
                Height="25px">
                <asp:ListItem Value="ALL">全部班次</asp:ListItem>
            <asp:ListItem Value="D">白班</asp:ListItem>
            <asp:ListItem Value="N">夜班</asp:ListItem>
            </asp:DropDownList>
        </ContentTemplate>
      </asp:UpdatePanel>
        </td>
        <td></td>
        <td style="height: 40px">
            <asp:Label ID="Label6" runat="server" Text="类型:" Font-Size="17"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="Status" name="Status" runat="server" Width="345px" 
                Height="25px">
                <asp:ListItem Value="01">上班</asp:ListItem>
                <asp:ListItem Value="02">重工</asp:ListItem>
                <asp:ListItem Value="03">清板</asp:ListItem>
                <asp:ListItem Value="04">结尾单</asp:ListItem>
                <asp:ListItem Value="05">试产</asp:ListItem>
                <asp:ListItem Value="06">拆机</asp:ListItem>
                <asp:ListItem Value="07">SMT大板线打小板</asp:ListItem>
                <asp:ListItem Value="08">5S整顿</asp:ListItem>
                <asp:ListItem Value="09">物品看护交接</asp:ListItem>
                <asp:ListItem Value="10">盘点</asp:ListItem>
            </asp:DropDownList>
               
        </td>
</tr>
<tr>
        <td></td>
        <td >
            <asp:Label ID="Label3" runat="server" Text="线别:" Font-Size="17"></asp:Label>
        </td>
        <td style="width: 305px">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:DropDownList ID="PdLine" name="PdLine" runat="server" Width="353px" 
                Height="25px">
                    </asp:DropDownList>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <td>&nbsp;</td>
        <td>
        <asp:Label ID="Label7" runat="server" Text="Remark:" Font-Size="15pt" 
            style="font-size: medium"></asp:Label>
            </td>
        <td>
            <textarea  id="Remark" name="Remark" 
                style="height: 35px; width: 340px; resize:none;" maxlength="40" cols="20" 
                rows="1"></textarea>
        </td>
</tr>
<tr>
        <td colspan="5">
        </td>
        <td align="right">
        <asp:Button ID="Button_Query" runat="server" Text="Query" 
                     onclick="Button_Query_Click" />
            <asp:Button ID="Button_Maintain" runat="server" Text="Maintain" 
                    onclick="Button_Maintain_Click" />
                <asp:Button ID="Button_Update" runat="server" onclick="Button_Update_Click" 
                Text="Update" ToolTip="更新时只可更改时间"/>
                <asp:Button ID="Button_Delete" runat="server" OnClientClick="return confirm('Are  you  sure?');" onclick="Button_Delete_Click" 
                    Text="Delete" />
        </td>
</tr>
<tr>
        <td colspan="6">
<%--        <div style="border: 1px inset #CC0000;  width:100%;height:250px ; overflow:auto;">
--%>    <%--<asp:GridView ID="GridView1" runat="server" 
        BackColor="White" BorderColor="#CC9966" 
        BorderWidth="1px" CellPadding="4" BorderStyle="None" Width="100%" 
        EmptyDataText="数据为空！数据已删除或尚未维护相应数据！">
        <RowStyle BackColor="White" ForeColor="#330099"/>
        <EmptyDataRowStyle Font-Size="XX-Large" HorizontalAlign="Center" />
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" BorderColor="#D50000" 
                        BorderStyle="Outset" BorderWidth="2px" BackColor="#FFCAE4" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="CheckALL" runat="server" AutoPostBack="True" 
                        onclick="javascript: SelectAllCheckboxes(this);" BorderColor="#AB00E3" 
                        BorderStyle="Outset" BorderWidth="2px" BackColor="#8A0000"/>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
        <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
    </asp:GridView>--%>
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="GridView1" runat="server" Width="98%" RowStyle-Height="20"
                        GvExtWidth="100%" GvExtHeight="320px" 
                AutoHighlightScrollByValue ="True" HighLightRowPosition="3" 
                OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' 
                GetTemplateValueEnable="False" HiddenColCount="0" 
                SetTemplateValueEnable="False"
                style="top: 155px; left: 2px">
                        <RowStyle Height="20px"></RowStyle>
                    </iMES:GridViewExt>
<%--</div> --%>
        </td>
</tr>
<tr>
        <td style="height: 29px"></td>
        <td style="height: 29px"></td>
        <td style="height: 29px"></td>
        <td style="height: 29px"></td>
        <td style="height: 29px"></td>
        <td align="right" style="height: 29px">
                 <asp:FileUpload ID="FileUpload1" runat="server" Width="152px" 
                     BackColor="#B871FF" BorderColor="White" BorderStyle="None" />
                 <asp:Button ID="Button_Import" runat="server" OnClientClick="return confirm('上传前请确认excel后缀名为.xls，含有空值的数据将不会被上传！');" onclick="Button_Import_Click" 
                     Text="InputExcel" Width="77px" />
            <asp:Button ID="Button_Export" runat="server" onclick="Button_Export_Click" 
                Text="DownExcel" Width="87px" />
                 </td>
</tr>
</table>
</div>
<input runat="server" type="hidden" id="get_id" />
<input runat="server" type="hidden" id="get_line" />
<script language="javascript">
    var str = "";
    document.writeln("<div id=\"_contents\" style=\"padding:3px; background-color:#9966ff; font-size: 12px; border: 1px solid #777777; position:absolute; left:?px; top:63%; width:?px; height:?px; z-index:1; visibility:hidden\">");
    str += "\u65f6<select id=\"_hour\">";
    for (h = 0; h <= 9; h++) {
        str += "<option value=\"0" + h + "\">0" + h + "</option>";
    }
    for (h = 10; h <= 23; h++) {
        str += "<option value=\"" + h + "\">" + h + "</option>";
    }
    str += "</select> \u5206<select id=\"_minute\">";
    for (m = 0; m <= 9; m++) {
        str += "<option value=\"0" + m + "\">0" + m + "</option>";
    }
    for (m = 10; m <= 59; m++) {
        str += "<option value=\"" + m + "\">" + m + "</option>";
    }
    //    str += "</select> \u79d2<select id=\"_second\">";
    //    for (s = 0; s <= 9; s++) {
    //        str += "<option value=\"0" + s + "\">0" + s + "</option>";
    //    }
    //    for (s = 10; s <= 59; s++) {
    //        str += "<option value=\"" + s + "\">" + s + "</option>";
    //    }
    str += "</select><input name=\"queding\" type=\"button\" onclick=\"_select()\" value=\"\u786e\u5b9a\" style=\"font-size:12px\" /></select> <input name=\"cancel\" type=\"button\" onclick=\"_cancel()\" value=\"\u53d6\u6d88\" style=\"font-size:12px\" /></div>";

    document.writeln(str);
    var _fieldname;
    function _SetTime(tt) {
        _fieldname = tt;
        var tttt ="0.5%";  //TT控件的定位点高
        var thei = tt.clientHeight;    //TT控件本身的高
        var tleft ="56%";    //TT控件的定位点宽
        var ttop;
        //        while (tttt = tt.offsetParent) {
        //            ttop += tt.offsetTop;
        //            tleft += tt.offsetLeft;
        //        }
        document.getElementById("_contents").style.top = tttt;
        document.getElementById("_contents").style.left = tleft;
        document.getElementById("_contents").style.visibility = "visible";
    }
    function _select() {
        _fieldname.value = document.getElementById("_hour").value + ":" + document.getElementById("_minute").value /*+ ":" + document.getElementById("_second").value*/;
        document.getElementById("_contents").style.visibility = "hidden";
    }
    function _cancel() {
        document.getElementById("_contents").style.visibility = "hidden";
    }

// 全选提示框
    function SelectAllCheckboxes(spanChk) {
        if (confirm("确认全选或取消全选？")) {
            elm = document.forms[0];
            for (i = 0; i <= elm.length - 1; i++) {
                if (elm[i].type == "checkbox" && elm[i].id != spanChk.id) {
                    if (elm.elements[i].checked != spanChk.checked) {
                        elm.elements[i].checked = spanChk.checked;
                    }
                }
            }
        }
        else {
            if (spanChk.checked == true) { spanChk.checked = false; }
            else {spanChk.checked = true; }
        }
    }
    function clickTable(con) {

        setGdHighLight(con);
        ShowRowEditInfo(con);
    }
    var iSelectedRowIndex = null;
    function setGdHighLight(con) {
        var idx = con.sectionRowIndex - 1;
        //   if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
        if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(idx, 10))) {
            //去掉过去高亮行
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=GridView1.ClientID %>");
        }
        //设置当前高亮行
        // setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=GridView1.ClientID %>");
        setRowSelectedOrNotSelectedByIndex(idx, true, "<%=GridView1.ClientID %>");
        //记住当前高亮行
        //            iSelectedRowIndex = parseInt(con.index, 10);
        iSelectedRowIndex = parseInt(idx, 10);
    }
    var get_id;
    function ShowRowEditInfo(con) {
        if (con == null) {
            setNewItemValue();
            return;
        }
        get_id=con.cells[0].innerText.trim();
        var pr = con.cells[1].innerText.trim();
        var sf = con.cells[2].innerText.trim();
        var pl = con.cells[3].innerText.trim();
        var st = con.cells[6].innerText.trim();
//        var st = con.cells[6].innerText.trim();
        setDropDownList(document.getElementById("<%=Process.ClientID %>"),pr);
        setDropDownList(document.getElementById("<%=Shift.ClientID %>"),sf);
        setDropDownList(document.getElementById("<%=PdLine.ClientID %>"), pl);
        setDropDownList(document.getElementById("<%=Status.ClientID %>"), st);
        document.getElementById("<%=BeginTime.ClientID %>").value = con.cells[4].innerText.trim();
        document.getElementById("<%=EndTime.ClientID %>").value = con.cells[5].innerText.trim();
//        setDropDownList(document.getElementById("<%=Status.ClientID %>"), st);
        document.getElementById("<%=get_id.ClientID %>").value = con.cells[0].innerText.trim();
        document.getElementById("<%=get_line.ClientID %>").value = con.cells[3].innerText.trim();

        //getConstValueTypeCmbObj().value = con.cells[1].innerText.trim(); //Type
        var currentId = con.cells[0].innerText.trim();

        if (currentId == "") {
            setNewItemValue();
            return;
        }
        else {
            document.getElementById("<%=Button_Delete.ClientID %>").disabled = false;
        }
    }
//    function setNewItemValue() {
//        //getConstValueTypeCmbObj().selectedIndex = 0;

//        setDropDownList(document.getElementById("<%=Process.ClientID %>"), "");
//        setDropDownList(document.getElementById("<%=Shift.ClientID %>"), "");
//        setDropDownList(document.getElementById("<%=PdLine.ClientID %>"), "");
//        document.getElementById("<%=Button_Delete.ClientID %>").disabled = true;
//    }
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
</script> 
</asp:Content>


﻿<%@ Page Title="Dinner  Time  Maintian" Language="C#" MasterPageFile="~/MasterPageMaintain.master" EnableEventValidation = "false"  AutoEventWireup="true" CodeFile="DinnerTimeMaintian.aspx.cs" Inherits="webroot_DinnerTimeMaintian_DinnerTimeMaintian" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <body>
    <table style="width: 119%; position: absolute; top: 5px; left: 4px; height: 241px;">
        <tr>
            <td style="width: 49px; height: 45px">
&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="製程" Font-Bold="True"></asp:Label>
            </td>
            <td style="width: 185px; height: 45px;">
                <asp:DropDownList ID="txtprocess" runat="server" style="width: 300px" 
                    AutoPostBack="True" 
                    onselectedindexchanged="txtprocess_SelectedIndexChanged" Height="20px" Width="197px" 
                     >
                </asp:DropDownList>
            </td>
            <td style="width: 50px; height: 45px">
                &nbsp;
                <asp:Label ID="Label2" runat="server" Text="班別" Font-Bold="True"></asp:Label>
            </td>
            <td style="width: 300px; height: 45px;">
                <asp:DropDownList ID="txtclass" runat="server" style="width: 300px" 
                    AutoPostBack="True" onselectedindexchanged="txtclass_SelectedIndexChanged" 
                    Height="21px" Width="266px">
                    <asp:ListItem Value="D">白班</asp:ListItem>
                    <asp:ListItem Value="N">夜班</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 49px; height: 45px">
            &nbsp;&nbsp;
                <asp:Label ID="Label3" runat="server" Text="線別" Font-Bold="True"></asp:Label>
            </td>
            <td style="width: 308px; height: 45px">
                <asp:DropDownList ID="txtpdline" runat="server" style="margin-bottom: 0px;" 
                    onselectedindexchanged="txtpdline_SelectedIndexChanged" 
                    AutoPostBack="True" Height="20px" Width="300px">
                </asp:DropDownList>
            </td>
            <td style="height: 45px">
            </td>
        </tr>
        <tr>
            <td style="width: 49px; height: 39px;">
                &nbsp;&nbsp;
                <asp:Label ID="Label4" runat="server" Text="Type" Font-Bold="True"></asp:Label>
            </td>
            <td style="height: 39px; width: 185px;">
                <asp:DropDownList ID="txttype" runat="server" style="width: 300px" 
                    Height="20px" Width="255px">
                    <asp:ListItem Value="E">就餐</asp:ListItem>
                    <asp:ListItem Value="R">休息</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="height: 39px; width: 50px;">
                &nbsp;&nbsp;
                <asp:Label ID="Label5" runat="server" Text="時間" Font-Bold="True"></asp:Label>
            </td>
            <td style="height: 39px;" colspan="3">
                <input id="txtfromdate" type="text" style="width: 292px" runat="server"
                    onclick="_SetTime(this)" readonly="readonly" />&nbsp;&nbsp;&nbsp;&nbsp; TO&nbsp;&nbsp;&nbsp;                 
                <input id="txttodate" type="text" style="width: 296px; margin-bottom: 0px;" runat="server"
                    onclick="_SetTime(this)" readonly="readonly" /></td>
        </tr>
        <tr>
            <td style="width: 49px; height: 39px;">
                &nbsp;
                <asp:Label ID="Label6" runat="server" Text="描述" Font-Bold="True"></asp:Label>
            </td>
            <td style="height: 39px; " colspan="5">
                <asp:TextBox ID="txtremark" runat="server" style="width: 299px" Width="289px" Height="22px" 
                  ></asp:TextBox>
                &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:FileUpload 
                    ID="FileUpload1" runat="server" BackColor="#F3F3F3" Width="219px" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="txtImport" runat="server" Font-Bold="True" Text="導入" 
                    onclick="txtImport_Click" Width="70px" BackColor="#F3F3F3" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="txtExport" runat="server" Font-Bold="True" Text="導出" 
                    Width="70px" onclick="txtExport_Click" BackColor="#F3F3F3" />
            </td>
        </tr>
        <tr>
            <td style="width: 49px; height: 30px;">
                &nbsp;
                </td>
            <td style="height: 30px; " colspan="5">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="txtquery" runat="server" Text="查詢所有" 
                    style=" top: 279px; left: 13px; margin-bottom: 0px; width: 70px;" 
                    Font-Bold="True" onclick="txtquery_Click" BackColor="#F3F3F3"/>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                <asp:Button ID="txtsave" runat="server" Text="保存" 
                    style="top: 280px; left: 155px; height: 26px;width: 70px;" 
                    Font-Bold="True" onclick="txtsave_Click" Width="69px" 
                    BackColor="#F3F3F3" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="txtupdate" runat="server" Text="更改" 
                    style="top: 280px; left: 155px; height: 26px;width: 70px;" 
                    Font-Bold="True" onclick="txtupdate_Click" Width="69px" 
                    BackColor="#F3F3F3" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="txtdelete" runat="server" Text="刪除" 
                    style="top: 280px; left: 301px; " Font-Bold="True" 
                    onclick="txtdelete_Click" Width="70px" 
                    onclientclick="return confirm('Are  you  sure?')" BackColor="#F3F3F3" />
            </td>
        </tr>
        <tr>
            <td style="width: 49px; height: 30px;">
                &nbsp;
                </td>
            <td style="height: 30px; width: 185px;">
           <div style="position:absolute; top: 79%; left: 1%; width:1312px; height:298px; overflow:auto; margin-top: 0px;">
    <asp:GridView ID="dinnertime" runat="server" 
                    BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" CellSpacing="2" Width="1309px">
        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="CheckAllBox" runat="server" AutoPostBack="True" 
                        onclick="javascript: SelectAllCheckboxes(this);"  />
                </HeaderTemplate>
                <ControlStyle Width="10px" />
                <HeaderStyle Width="10px" />
                <ItemStyle Width="10px" />
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" 
            Width="50px" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
    </div>
            </td>
            <td style="height: 30px; width: 50px;">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="height: 37px; width: 49px;">
                </td>
            <td style="width: 185px">
                &nbsp;</td>
            <td style="width: 50px">
                &nbsp;</td>
        </tr>
    </table>

<script language="javascript">
    function SelectAllCheckboxes(spanChk) {
        //瞳蚚勤趕遺殿隙腔硉 ㄗtrue 麼氪 falseㄘ
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
            else { spanChk.checked = true; }
        }
    }

    var str = "";
    document.writeln("<div id=\"_contents\" style=\"padding:6px; background-color:#E3E3E3; font-size: 12px; border: 1px solid #777777; position:absolute; left:?px; top:?px; width:?px; height:?px; z-index:1; visibility:hidden\">");
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
    str += "</select> <input name=\"queding\" type=\"button\" onclick=\"_select()\" value=\"\u786e\u5b9a\" style=\"font-size:12px\" /></div>";
    document.writeln(str);
    var _fieldname;
    function _SetTime(tt) {
        _fieldname = tt;
        var tttt = tt.offsetTop;    //TT諷璃腔隅弇萸詢  
        var thei = tt.clientHeight;    //TT諷璃掛旯腔詢
        var tleft = tt.offsetLeft;    //TT諷璃腔隅弇萸遵
        var ttop;
        //      
        //               while (tttt = tt.offsetParent) {
        //                    ttop += tt.offsetTop;
        //                   tleft += tt.offsetLeft;
        //              }
        document.getElementById("_contents").style.top = 100;  //tttt + thei + 4;
        document.getElementById("_contents").style.left = 660; //tleft;
        document.getElementById("_contents").style.visibility = "visible";
    }

    function _select() {
        _fieldname.value = document.getElementById("_hour").value + ":" + document.getElementById("_minute").value; //+ ":" + document.getElementById("_second").value;
        document.getElementById("_contents").style.visibility = "hidden";
    }

</script>  
</body>
</asp:Content>

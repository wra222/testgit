<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="AlarmMailAddress.aspx.cs" Inherits="webroot_UPHMaintain_AlarmMailAddress" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Register assembly="waitCover" namespace="waitCover" tagprefix="iMES" %>


    <asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
        <style type="text/css">
   .tdTxt {
    color: blue;
  }
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
        #btnAdd
        {
            width: 53px;
        }
       
        .style1
        {
            width: 87px;
        }
       
  </style>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <div id="container" style="width: 95%;  border: solid 0px red; margin: 0 auto;">
        <div>
        <table style="width:100%; border:1" class="iMes_div_MainTainEdit">
        <tr>
        <td colspan="7">
        <td style="width: 30px">
          <asp:Button ID="Button_Del" runat="server" Text="Delete" Width="100px" 
                Height="23px" onclick="Button_Del_Click" />
         </td>
        </td>
        </tr>
        <td colspan="8">
         
                    <input id="hidRecordCount" runat="server" type="hidden" />
                    <iMES:GridViewExt ID="GridView1" runat="server" AutoGenerateColumns="true" 
                        AutoHighlightScrollByValue="true" EnableViewState="true" GvExtWidth="99%" 
                        HighLightRowPosition="3" 
                         OnGvExtRowClick='clickTable(this)'
                         RowStyle-Height="20" 
                        Style="top: 0px; left: -1px; height: 420px;" Width="100%" BorderColor="Red" 
                        BorderStyle="None" BorderWidth="1px" GvExtHeight="400px">
                        <RowStyle Height="20px" />
                    </iMES:GridViewExt>
              
       
        </td>
        
        </tr>
            <tr>
               <td style="width: 100px">
                    <asp:Label ID="Label2" runat="server" Text="Dept："></asp:Label>
                </td>
                <td style="width: 180px">
                    <asp:DropDownList ID="DL_Dept" runat="server" AutoPostBack="True" Width="140px"> 
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="成制一部"></asp:ListItem>
                        <asp:ListItem Value="成制二部"></asp:ListItem>
                        <asp:ListItem Value="半制部"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 100px">
                    <asp:Label ID="Label8" runat="server" Text="Process："></asp:Label>
                </td>
                <td style="width: 180px">
                    <asp:DropDownList ID="DL_Process" runat="server" AutoPostBack="True" 
                        Width="140px" >
                         <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="SA"></asp:ListItem>
                        <asp:ListItem Value="FA"></asp:ListItem>
                        <asp:ListItem Value="PA"></asp:ListItem> 
                        <asp:ListItem Value="SMT"></asp:ListItem> 
                        <asp:ListItem Value="DK"></asp:ListItem> 

                    </asp:DropDownList>
                </td>
                 <td style="width: 100px">
                    <asp:Label ID="Label1" runat="server" Text="PdLine："></asp:Label>
                </td>
                <td style="width: 180px">
                    <asp:DropDownList ID="DL_PdLine" runat="server" AutoPostBack="True" 
                        Width="140px" onselectedindexchanged="DL_PdLine_SelectedIndexChanged" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
            <td style="width: 100px">
                    <asp:Label ID="Label3" runat="server" Text="Repark："></asp:Label>
                </td>
                <td colspan="4">
                    <asp:TextBox ID="TB_Remark" runat="server" Width="50%"></asp:TextBox>
                </td>
                </tr>
                <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label4" runat="server" Text="MailAddress："></asp:Label>
                </td>
                <td colspan="6">
                    <asp:TextBox ID="TB_MailAddress" runat="server" Width="99%"></asp:TextBox>
                </td>
                <td class="style1">
                  <asp:Button ID="Button_Save" runat="server" Text="Save" Width="108px" 
                        Height="23px" onclick="Button_Save_Click" />
                </td>
                <td style="width: 30px">
                  <asp:Button ID="Button_Insert" runat="server" Text="Insert" Width="108px" 
                        Height="23px" onclick="Button_Insert_Click" />
                </td>
          
            </tr>
      </table>
      </div>
         <input id="Hidden1" type="hidden" runat=server/> 
      </div>
         <script language="javascript" type="text/javascript">
             var iSelectedRowIndex = null;
             function setGdHighLight(con) {
                 var idx = con.sectionRowIndex - 1;
                 //   if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                 if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(idx, 10))) {
                     //去掉过去高亮行
                     setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=GridView1.ClientID %>");
                 }
                 //设置当前高亮行   
                 setRowSelectedOrNotSelectedByIndex(idx, true, "<%=GridView1.ClientID %>");
                 //记住当前高亮行
                 //            iSelectedRowIndex = parseInt(con.index, 10);
                 iSelectedRowIndex = parseInt(idx, 10);
             }
             function setNewItemValue() {
                 //getConstValueTypeCmbObj().selectedIndex = 0;

                 document.getElementById("<%=TB_Remark.ClientID %>").value = ""
                 document.getElementById("<%=TB_MailAddress.ClientID %>").value = ""
                

             }
             function ShowRowEditInfo(con) {
                 if (con == null) {
                     setNewItemValue();
                     return;
                 }
                 var Select1 = con.cells[0].innerText.trim();
                 var Select2 = con.cells[1].innerText.trim();
                 var Select3 = con.cells[2].innerText.trim();


                 document.getElementById("<%=TB_MailAddress.ClientID %>").value = con.cells[3].innerText.trim();
                 document.getElementById("<%=TB_Remark.ClientID %>").value = con.cells[4].innerText.trim();


                 setDropDownList(document.getElementById('<%=DL_Dept.ClientID %>'), Select1);
                 setDropDownList(document.getElementById('<%=DL_Process.ClientID %>'), Select2);
                 setDropDownList(document.getElementById('<%=DL_PdLine.ClientID %>'), Select3);

                 var currentId = con.cells[0].innerText.trim();


             }

             function clickTable(con) {

                 setGdHighLight(con);
                 ShowRowEditInfo(con);
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
           </script>
</asp:Content>

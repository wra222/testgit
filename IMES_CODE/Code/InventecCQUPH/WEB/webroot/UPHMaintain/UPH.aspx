<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="UPH.aspx.cs" Inherits="webroot_aspx_UPH" Title="Untitled Page" %>
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
       
  </style>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <div id="container" style="width: 95%;  border: solid 0px red; margin: 0 auto;">
        <div>
        <table style="width:100%; border:1" class="iMes_div_MainTainEdit">
            <tr>
               <td style="width: 100px">
                    <asp:Label ID="Label2" runat="server" Text="制成："></asp:Label>
                </td>
                <td style="width: 180px">
                    <asp:DropDownList ID="Select1" runat="server" AutoPostBack="True" Width="140px" 
                        onselectedindexchanged="Select1_SelectedIndexChanged" >
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="FA"></asp:ListItem>
                        <asp:ListItem Value="PA"></asp:ListItem>
                        <asp:ListItem Value="SA"></asp:ListItem>
                        <asp:ListItem Value="SMT"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                 <td style="width: 100px">
                    <asp:Label ID="Label6" runat="server" Text="Attend_normal："></asp:Label>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="TextBox1" runat="server" Width="154px"></asp:TextBox>
                </td>
                <td style="width: 100px">
                    <asp:Label ID="Label8" runat="server" Text="Cycle："></asp:Label>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="TextBox3" runat="server" Width="90%"></asp:TextBox>
                </td> 
                   
               <td style="width: 30px">
                    <asp:Button ID="Button1" runat="server" Text="查询" onclick="Button1_Click" Height="25px" 
                        />
                  
                </td>
                <td style="width: 30px">
                  <asp:Button ID="Button6" runat="server" Text="全部查询" onclick="Button6_Click" 
                        Height="25px"/>
                </td>
            </tr>
            <tr>
               <td style="width: 100px">
                    <asp:Label ID="Label4" runat="server" Text="Family："></asp:Label>
                </td>
               <td style="width: 180px">
                    <asp:DropDownList ID="Select3" runat="server" Width="140px"  
                        AutoPostBack="True" onselectedindexchanged="Select3_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td >
                    <asp:Label ID="Label11" runat="server" Text="ST："></asp:Label>
                </td>
                <td >
                    <asp:TextBox ID="TextBox6" runat="server" Width="154px"></asp:TextBox>
                </td>
                <td >
                    <asp:Label ID="Label9" runat="server" Text="Remark："></asp:Label>
                </td>
               <td >
                    <asp:TextBox ID="TextBox4" runat="server" Width="90%"></asp:TextBox>
                </td>
                <td style="width: 30px">
                 
                   <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="插入" 
                            ValidationGroup="1" Height="25px"  />
                </td>
                <td style="width: 30px">
                    <asp:Button ID="Button4" runat="server" Text="删除" onclick="Button4_Click" Height="25px" 
                         ValidationGroup="2" />
                         
                </td>
               
            </tr>
            <tr>
                  <td style="width: 100px">
                    <asp:Label ID="Label5" runat="server" Text="Special："></asp:Label>
                </td>
                <td style="width: 180px">
                    <asp:DropDownList ID="Select2" runat="server" AutoPostBack="True" Width="140px"  >
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="DIS"></asp:ListItem>
                        <asp:ListItem Value="RCTO"></asp:ListItem>
                        <asp:ListItem Value="UMA"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 74px">
                    <asp:Label ID="Label7" runat="server" Text="NormalUPH："></asp:Label>
                </td>
                <td style="width: 95px">
                    <asp:TextBox ID="TextBox2" runat="server" Width="154px"></asp:TextBox>
                </td>
                <td style="width: 99px">
                     
                   </td>
               <td style="width: 99px">
                     <asp:FileUpload ID="FileUpload1" runat="server" Height="21px"   />
                    </td>
                 <td colspan="1">
                     <asp:Button ID="Button5" runat="server" Text="变更" onclick="Button5_Click" 
                            ValidationGroup="1" Height="25px"  />
                 </td>
                 <td>
                     &nbsp;</td>
            </tr>
            <tr>
                 <td style="width: 100px">
                   
                </td>
                <td style="width: 151px">
                    
                </td>
                <td style="width: 74px">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="TextBox1" ErrorMessage="Attend_normal为空！" 
                        ValidationGroup="1"></asp:RequiredFieldValidator>
                </td>
                <td style="width: 95px">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="TextBox2" ErrorMessage="NormalUPH为空！" ValidationGroup="1"></asp:RequiredFieldValidator>
                </td>
                <td style="width: 99px">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ControlToValidate="TextBox3" ErrorMessage="Cycle为空！" ValidationGroup="1"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                        ControlToValidate="TextBox6" ErrorMessage="ST为空！" ValidationGroup="1"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                        ControlToValidate="TextBox1" ErrorMessage="先查询后再操作！" ValidationGroup="2"></asp:RequiredFieldValidator>
                </td>
               <td style="width: 30px">
             
              
                  <asp:Button ID="Button8" runat="server" onclick="Button8_Click" 
                            Text="UPH-Excel导入" Width="108px" Height="23px" />
             
              
                </td>
              <td style="width: 30px">
             
              
             <asp:Button ID="Button7" runat="server" onclick="Button7_Click" 
                            Text="UPH-Exce导出" Width="108px" Height="25px" />
             
              
                </td>
            </tr>
        <tr>
          <td colspan="8">
          <!--
        <div style="position:absolute; top: 200px; left: -0.1%; width:1200px; height:300px; overflow:auto;">
           -->
                    <input id="hidRecordCount" runat="server" type="hidden" />
                    <iMES:GridViewExt ID="GridView1" runat="server" AutoGenerateColumns="true" 
                        AutoHighlightScrollByValue="true" EnableViewState="true" GvExtWidth="99%" 
                        HighLightRowPosition="3" 
                        OnGvExtRowClick='clickTable(this)'
                         RowStyle-Height="20" 
                        Style="top: 0px; left: 0px; height: 420px;" Width="100%" BorderColor="Red" 
                        BorderStyle="None" BorderWidth="1px" GvExtHeight="400px">
                        <RowStyle Height="20px" />
                    </iMES:GridViewExt>
              
       
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

                 document.getElementById("<%=TextBox1.ClientID %>").value = ""
                 document.getElementById("<%=TextBox6.ClientID %>").value = ""
                 document.getElementById("<%=TextBox3.ClientID %>").value = ""
                 document.getElementById("<%=TextBox2.ClientID %>").value = ""
                 document.getElementById("<%=TextBox4.ClientID %>").value = ""

             }
             function ShowRowEditInfo(con) {
                 if (con == null) {
                     setNewItemValue();
                     return;
                 }
                 var Select1 = con.cells[1].innerText.trim();
                 var Select2 = con.cells[7].innerText.trim();
                 var Select3 = con.cells[2].innerText.trim();


                 document.getElementById("<%=TextBox1.ClientID %>").value = con.cells[3].innerText.trim();
                 document.getElementById("<%=TextBox6.ClientID %>").value = con.cells[4].innerText.trim();
                 document.getElementById("<%=TextBox3.ClientID %>").value = con.cells[5].innerText.trim();
                 document.getElementById("<%=TextBox2.ClientID %>").value = con.cells[6].innerText.trim();
                 document.getElementById("<%=TextBox4.ClientID %>").value = con.cells[8].innerText.trim();

                 setDropDownList(document.getElementById('<%=Select1.ClientID %>'), Select1);
                 setDropDownList(document.getElementById('<%=Select2.ClientID %>'), Select2);
                 setDropDownList(document.getElementById('<%=Select3.ClientID %>'), Select3);
                
            







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


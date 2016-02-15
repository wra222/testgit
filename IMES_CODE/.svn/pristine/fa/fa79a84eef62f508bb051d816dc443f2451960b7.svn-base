<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WHPalletControl
 * CI-MES12-SPEC-PAK-W/H Pallet Control.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
*/
 --%>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WHPalletControl.aspx.cs" Inherits="PAK_WHPalletControl" Title="Untitled Page" %>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>

    <asp:ScriptManager runat="server" ID="SM"  EnablePartialRendering="true">

    </asp:ScriptManager>
    <script type="text/javascript" language="javascript"  >
    var editor = '<%=userId%>';
    var customer = '<%=customer%>';
    var station = '<%=station%>';
    var table = "";
    var msgNoFromDate = '<%=this.GetLocalResourceObject(Pre + "_msgNoFromDate").ToString()%>';
    var msgNoToDate = '<%=this.GetLocalResourceObject(Pre + "_msgNoToDate").ToString()%>';
    var msgBadDate = '<%=this.GetLocalResourceObject(Pre + "_msgBadDate").ToString()%>';
    var msgWrongPlt = '<%=this.GetLocalResourceObject(Pre + "_msgWrongPlt").ToString()%>';
    var msgWrongHave = '<%=this.GetLocalResourceObject(Pre + "_msgWrongHave").ToString()%>';
    var msgWrongIn = '<%=this.GetLocalResourceObject(Pre + "_msgWrongIn").ToString()%>';
    var msgWrongOut = '<%=this.GetLocalResourceObject(Pre + "_msgWrongOut").ToString()%>';
    var msg12 = '<%=this.GetLocalResourceObject(Pre + "_msg12").ToString()%>';
    var msg11 = '<%=this.GetLocalResourceObject(Pre + "_msg11").ToString()%>';
    var msg13 = '<%=this.GetLocalResourceObject(Pre + "_msg13").ToString()%>';
    var msg31 = '<%=this.GetLocalResourceObject(Pre + "_msg31").ToString()%>';
    var msgWrongInRW = '<%=this.GetLocalResourceObject(Pre + "_msgWrongInRW").ToString()%>';
    var msgWrongOutRW = '<%=this.GetLocalResourceObject(Pre + "_msgWrongOutRW").ToString()%>';
    var msgWrongEX = '<%=this.GetLocalResourceObject(Pre + "_msgWrongEX").ToString()%>';
    var msgWrongDT = '<%=this.GetLocalResourceObject(Pre + "_msgWrongDT").ToString()%>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';  
    window.onload = function() {
        d = new Date();
        now_year = d.getYear();
        now_month = d.getMonth() + 1;
        now_month = now_month >= 10 ? now_month : "0" + now_month;
        now_date = d.getDate();
        now_date = now_date >= 10 ? now_date : "0" + now_date;
        formattedDate = now_year + "-" + now_month + "-" + now_date;
        document.getElementById("txtDateFrom").value = formattedDate;
        document.getElementById("txtDateTo").value = formattedDate;
        table = document.getElementById("<%=gridViewExt1.ClientID%>");
        document.getElementById("<%=TextBox1.ClientID%>").value = "";
        document.getElementById("<%=TextBox1.ClientID%>").focus();
    }

    /*document.onkeydown = EnterTab();
    function EnterTab() {
        if (window.event.keyCode == 9)
            window.event.keyCode = 13;
    }*/
    
    function onTextBox1KeyDown() {
        ShowInfo("");
        if (event.keyCode == 9 || event.keyCode == 13) 
        {
            event.cancel = true;
            event.returnValue = false;
            document.getElementById("<%=TextBox1.ClientID%>").focus();
            document.getElementById("<%=HPLT.ClientID%>").value = "";
            var inputTextBox1 = document.getElementById("<%=TextBox1.ClientID%>").value.trim();
            inputTextBox1 = inputTextBox1.toUpperCase();
            if (inputTextBox1.length == 10) {
                if (!(inputTextBox1.substr(0, 2) == "00"
                || inputTextBox1.substr(0, 2) == "01"
                 || inputTextBox1.substr(0, 2) == "02"
                  || inputTextBox1.substr(0, 2) == "03"
                || inputTextBox1.substr(0, 2) == "90")
                ) {
                    getDisplay(1);
                    document.getElementById("<%=TextBox1.ClientID%>").focus();
                    return;
                }
            }
            else if (inputTextBox1.length == 20) {
            }
            else {
                getDisplay(1);
                document.getElementById("<%=TextBox1.ClientID%>").focus();
                return;
            }
            inputTextBox1 = inputTextBox1.toUpperCase();
            document.getElementById("<%=HPLT.ClientID%>").value = inputTextBox1;
            document.getElementById("<%=btnOnInput.ClientID%>").click();
            document.getElementById("<%=TextBox1.ClientID%>").focus();
        }
    }
    function onTextBox2KeyDown() {
         ShowInfo("");
         if (event.keyCode == 9 || event.keyCode == 13) {
             event.cancel = true;
             event.returnValue = false;
             btnRemove_onclick();
          }
    }
    function btnTotal_onclick() {
        document.getElementById("<%=txtTotal.ClientID%>").value = "";
        document.getElementById("<%=btnTotalCount.ClientID%>").click(); 
    }

    function btnQueryIN_onclick() {
        ShowInfo("");

        
        dateFrom = document.getElementById("txtDateFrom").value;
        dateTo = document.getElementById("txtDateTo").value;
        if (dateFrom == null || dateFrom == "") {
            alert(msgNoFromDate);
            document.getElementById("btnFrom").click();
            return;
        }
        if (dateTo == null || dateTo == "") {
            alert(msgNoToDate);
            document.getElementById("btnTo").click();
            return;
        }
        d1 = dateFrom.substring(0, 4);
        d2 = dateTo.substring(0, 4);
        if (d1 > d2) {
            alert(msgBadDate);
            document.getElementById("btnFrom").click();
            return;
        }
        else if (d1 == d2) {
            d1 = dateFrom.substring(5, 7);
            d2 = dateTo.substring(5, 7);
            if (d1 > d2) {
                alert(msgBadDate);
                document.getElementById("btnFrom").click();
                return;
            }
            else if (d1 == d2) {
                d1 = dateFrom.substring(8, 10);
                d2 = dateTo.substring(8, 10);
                if (d1 > d2) {
                    alert(msgBadDate);
                    document.getElementById("btnFrom").click();
                    return;
                }
            }
        }
        document.getElementById("<%=HDateFrom.ClientID%>").value = dateFrom;
        document.getElementById("<%=HDateTo.ClientID%>").value = dateTo;
        document.getElementById("<%=btnOnQueryIN.ClientID%>").click();
        beginWaitingCoverDiv();
    }

    function btnQuery7Days_onclick() {
        ShowInfo("");
        document.getElementById("<%=btn7Days.ClientID%>").click();
        beginWaitingCoverDiv();
    }
    function getDate() {
        ShowInfo("");
        dateFrom = document.getElementById("txtDateFrom").value;
        dateTo = document.getElementById("txtDateTo").value;
        if (dateFrom == null || dateFrom == "") {
            alert(msgNoFromDate);
            document.getElementById("btnFrom").click();
            return;
        }
        if (dateTo == null || dateTo == "") {
            alert(msgNoToDate);
            document.getElementById("btnTo").click();
            return;
        }
        d1 = dateFrom.substring(0, 4);
        d2 = dateTo.substring(0, 4);
        if (d1 > d2) {
            alert(msgBadDate);
            document.getElementById("btnFrom").click();
            return;
        }
        else if (d1 == d2) {
            d1 = dateFrom.substring(5, 7);
            d2 = dateTo.substring(5, 7);
            if (d1 > d2) {
                alert(msgBadDate);
                document.getElementById("btnFrom").click();
                return;
            }
            else if (d1 == d2) {
                d1 = dateFrom.substring(8, 10);
                d2 = dateTo.substring(8, 10);
                if (d1 > d2) {
                    alert(msgBadDate);
                    document.getElementById("btnFrom").click();
                    return;
                }
            }
        }
        document.getElementById("<%=HDateFrom.ClientID%>").value = dateFrom;
        document.getElementById("<%=HDateTo.ClientID%>").value = dateTo;
    }
    function btnQueryNotIN_onclick() {
        ShowInfo("");
        dateFrom = document.getElementById("txtDateFrom").value;
        dateTo = document.getElementById("txtDateTo").value;
        if (dateFrom == null || dateFrom == "") {
            alert(msgNoFromDate);
            document.getElementById("btnFrom").click();
            return;
        }
        if (dateTo == null || dateTo == "") {
            alert(msgNoToDate);
            document.getElementById("btnTo").click();
            return;
        }
        d1 = dateFrom.substring(0, 4);
        d2 = dateTo.substring(0, 4);
        if (d1 > d2) {
            alert(msgBadDate);
            document.getElementById("btnFrom").click();
            return;
        }
        else if (d1 == d2) {
            d1 = dateFrom.substring(5, 7);
            d2 = dateTo.substring(5, 7);
            if (d1 > d2) {
                alert(msgBadDate);
                document.getElementById("btnFrom").click();
                return;
            }
            else if (d1 == d2) {
                d1 = dateFrom.substring(8, 10);
                d2 = dateTo.substring(8, 10);
                if (d1 > d2) {
                    alert(msgBadDate);
                    document.getElementById("btnFrom").click();
                    return;
                }
            }
        }
        document.getElementById("<%=HDateFrom.ClientID%>").value = dateFrom;
        document.getElementById("<%=HDateTo.ClientID%>").value = dateTo;
        document.getElementById("<%=btnOnQueryNotIN.ClientID%>").click();
        beginWaitingCoverDiv();
    }
    function getTotal(count) {
        ShowInfo("");
        document.getElementById("<%=txtTotal.ClientID%>").value = count;
    }
    function getDisplay(count) {
        var temp = "";
        if (document.getElementById("<%=HPLT.ClientID%>").value != "") {
            temp = "Pallet NO:" + document.getElementById("<%=HPLT.ClientID%>").value + ", ";
        }
        else if (document.getElementById("<%=TextBox1.ClientID%>").value != "") {
        temp = "Pallet NO:" + document.getElementById("<%=TextBox1.ClientID%>").value.trim().toUpperCase() + ", ";
        }
        if (count == 1) {
            var displayTemp = temp + msgWrongPlt;
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
        }
        else if (count == 2) {
            var displayTemp = temp  + msgWrongHave;
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
        }
        else if (count == 3) {
            var displayTemp = temp  + msgWrongIn;
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
        }
        else if (count == 4) {
            var displayTemp = temp  + msgWrongOut;
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
        }
        else if (count == 5) {
            var displayTemp = temp  + msgWrongDT;
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
        }
        else if (count == 12) {
            var displayTemp = temp  + msg12;
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
        }
        else if (count == 11) {
            var displayTemp = temp  + msg11;
            //ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            m_playSound(false);
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
        }
        else if (count == 13) {
            var displayTemp = temp  + msg13;
            //ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            m_playSound(false);
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
        }
        else if (count == 31) {
            var displayTemp = temp  + msg31;
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
        }
        else if (count == 14) {
            var displayTemp = temp  + "NO delivery data";
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
        }
        else if (count == 15) {
            var displayTemp = temp  + "NO WhPltType data";
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();
        }
    }
    function getRemoveDisplay(count) {
        var temp = "";
        if (document.getElementById("<%=HRMPLT.ClientID%>").value != "") {
            temp = "Pallet NO:" + document.getElementById("<%=HRMPLT.ClientID%>").value + ", ";
        }
        else if (document.getElementById("<%=TextBox2.ClientID%>").value != "") {
            temp = "Pallet NO:" + document.getElementById("<%=TextBox2.ClientID%>").value.trim().toUpperCase() + ", ";
        }
        if (count == 1) {
            var displayTemp = temp  + msgWrongPlt;
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox2.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").focus();
        }
        else if (count == 2) {
            var displayTemp = temp  + msgWrongHave;
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox2.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").focus();
        }
        else if (count == 3) {
            var displayTemp = temp  + msgWrongInRW;
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox2.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").focus();
        }
        else if (count == 4) {
            var displayTemp = temp  + msgWrongOutRW;
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox2.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").focus();
        }
        else if (count == 5) {
            var displayTemp = temp  + msgWrongEX;
            ShowMessage(displayTemp);
            ShowInfo(displayTemp);
            document.getElementById("<%=TextBox2.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").focus();
        }
        else if (count == 6) {
            //ShowMessage("Remove success");
            //ShowInfo("Remove success");
            document.getElementById("<%=TextBox2.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").focus();
        }
    }
     function btnExcel_onclick() {
         var strTitle = "";
         var dgData = document.getElementById("<%=gridViewExt1.ClientID%>");
         var iStartCol = 0;
         var iEndCol = 10;
         // 定义Excel Applicaiton Object
　　     var appExcel = null;
　　     // 当前激活的工作簿
　　     var currentWork = null;
　　     var currentSheet = null;
    　　　
　　     try
　　     {
            // 初始化application
            appExcel = new ActiveXObject("Excel.Application");
            appExcel.Visible = false;
            appExcel.DisplayFullScreen = false;
            appExcel.DisplayFullScreen = true;
         
　　     }
　　     catch(e)
　　     {
　　　　     window.alert("Please Install Excel First");
    　　　　　
　　　　     return;
　　     }
    　　　
         appExcel.Visible = false;
　　     // 获取当前激活的工作部
　　     currentWork = appExcel.Workbooks.Add();
　　     currentSheet = currentWork.ActiveSheet;
         currentSheet.Columns('A').ColumnWidth = 1;
         currentSheet.Columns('B:D').ColumnWidth = 21;
         currentSheet.Columns('E').ColumnWidth = 5;
         currentSheet.Columns('F:I').ColumnWidth = 16;
　　     // 填充excel内容
　　     // 设置标题
　　     //currentSheet.Cells(1,1).Value = strTitle;
　　     // 填充内容
　　     for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++)
　　     {
　　　　     // 显示指定列的内容
　　　　     for (var iCol = iStartCol; iCol <= iEndCol; iCol++)
　　　　     {
　　　　　　
　　　　　　     currentSheet.Cells(iRow + 4, iCol + 2).Value =　
　　　　　　　　     dgData.rows[iRow].cells[iCol].innerText;
　　　　     }
         }
        appExcel.Visible = true;
      }
      function btnTo_onclick() {
          showCalendar(document.getElementById("txtDateTo"));
          //document.getElementById("<%=txtTotal.ClientID%>").value = "";
      }
     function btnFrom_onclick() {
         showCalendar(document.getElementById("txtDateFrom"));
         //document.getElementById("<%=txtTotal.ClientID%>").value = "";
     }
     function btnRemove_onclick() {
         ShowInfo("");
         document.getElementById("<%=HRMPLT.ClientID%>").value = "";
         var inputTextBox2 = document.getElementById("<%=TextBox2.ClientID%>").value.trim();
         inputTextBox2 = inputTextBox2.toUpperCase();
         if (inputTextBox2.length == 10) {
             if (!(inputTextBox2.substr(0, 2) == "00"
                || inputTextBox2.substr(0, 2) == "01"
                || inputTextBox2.substr(0, 2) == "90"
                || inputTextBox2.substr(0, 2) == "02")
                ) {
                 getRemoveDisplay(1);
                 return;
             }
         }
         else if (inputTextBox2.length == 20) {
         }
         else {
             getRemoveDisplay(1);
             return;
         }
         inputTextBox2 = inputTextBox2.toUpperCase();
         document.getElementById("<%=HRMPLT.ClientID%>").value = inputTextBox2;
         document.getElementById("<%=TextBox2.ClientID%>").value = "";
         document.getElementById("<%=btnOnRemove.ClientID%>").click(); 
     }
     function getResetSN() {
         document.getElementById("<%=TextBox1.ClientID%>").value = "";
         document.getElementById("<%=HPLT.ClientID%>").value = "";
     }
     function ShowInfoSuccessWH(msg) {
         var successTemp = "";
         var temp = "";
         if (msg != "") {
             if (msg.indexOf("removed") != -1) {
                
                 if (document.getElementById("<%=HRMPLT.ClientID%>").value != "") {
                     temp = "Pallet NO:" + document.getElementById("<%=HRMPLT.ClientID%>").value ;
                 }
                 else if (document.getElementById("<%=TextBox2.ClientID%>").value != "") {
                     temp = "Pallet NO:" + document.getElementById("<%=TextBox2.ClientID%>").value.trim().toUpperCase() ;
                 }
             }
             else {
                 if (document.getElementById("<%=HPLT.ClientID%>").value != "") {
                     temp = "Pallet NO:" + document.getElementById("<%=HPLT.ClientID%>").value  ;
                 }
                 else if (document.getElementById("<%=TextBox1.ClientID%>").value != "") {
                     temp = "Pallet NO:" + document.getElementById("<%=TextBox1.ClientID%>").value.trim().toUpperCase() ;
                 }
             }
             successTemp = "[" + temp + "]" + msgSuccess + " \n" + msg;
         }
         if (successTemp != "") {
             //ShowSuccessfulInfo(true);
             ShowSuccessfulInfo(true, successTemp);
         }
         else {
             ShowSuccessfulInfo(true);
         }
     }
    </script>

 <div>
   <center >
   <table width="95%" style="vertical-align:middle; height:20%" cellpadding="0" cellspacing="0" >
    <tr>
        <td align="left" colspan="1">
            <fieldset>
            <legend align="center"><asp:Label ID="lbDataEntryTitle" runat="server"></asp:Label></legend>
            <asp:Label ID="lbDataEntry" Width = "13%" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server"  Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"  style="width:60%" />
             </fieldset>
        </td>
    </tr>
    <tr>
        <td align="left" colspan="1">
            <fieldset>
            <legend align="center"><asp:Label ID="lbPalletNOTitle" runat="server"></asp:Label></legend>
            <asp:Label ID="lbPalletNO" Width = "13%" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
            <asp:TextBox ID="TextBox2" runat="server"  Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red" style="width:60%" />
            <input id="btnRemove" type="button"  runat="server" 
                    class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'" visible="True"  onclick="btnRemove_onclick()" />
             </fieldset>
        </td>
    </tr>    
    <tr>
        
        
        <td colspan="4" align="left">
        <fieldset>
            <legend align="center"><asp:Label ID="lbTableTitle" runat="server"></asp:Label></legend>
             <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lbFrom" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    <input type="text" id="txtDateFrom" readonly="readonly" />
                  <%--  <input type="text" id="txtDateFrom"  />--%>
                    <button type="button" id="btnFrom" onclick="btnFrom_onclick()">...</button>
                    <asp:Label ID="lbTo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    <input type="text" id="txtDateTo" readonly="readonly" />
                   <%-- <input type="text" id="txtDateTo" />--%>
                    <button type="button" id="btnTo" onclick="btnTo_onclick()">...</button>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                    <input id="btnTotal" type="button"  runat="server" 
                    class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'" visible="True"  onclick="btnTotal_onclick()" />
                    <asp:TextBox ID="txtTotal" runat="server"  style="width:7%" 
                        BackColor="#FFFF99" Font-Bold="True" Font-Size="Large"   />
                </td>
             </tr>
            <tr>
                <td>
                <input id="btnQueryIN" type="button"  runat="server" 
                    class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'" visible="True" 
                          onclick="btnQueryIN_onclick()" />
                <input id="btnQueryNotIN" type="button"  runat="server" 
                    class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'" visible="True"  onclick="btnQueryNotIN_onclick()" />
                <input id="btnQuery7Days" type="button"  runat="server" 
                    class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'" visible="True"  onclick="btnQuery7Days_onclick()" />
                <input id="btnExcel" type="button"  runat="server" 
                    class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'"  visible="True"  onclick="btnExcel_onclick()" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanelTable" UpdateMode="Conditional">
                        <ContentTemplate>
                            <iMES:GridViewExt ID="gridViewExt1" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="True"
                                GvExtWidth="100%" GvExtHeight="220px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                Width="99.9%" Height="210px" SetTemplateValueEnable="True" GetTemplateValueEnable="True"
                                HighLightRowPosition="3" HorizontalAlign="Left" HiddenColCount="0" > 
                                <Columns>
                                    <asp:BoundField DataField="DeliveryNO" />
                                     <asp:BoundField DataField="ShipmentNO" />
                                    <asp:BoundField DataField="Model" />
                                    <asp:BoundField DataField="PalletNO" />
                                    <asp:BoundField DataField="CartonQty" />
                                    <asp:BoundField DataField="Qty" />
                                    <asp:BoundField DataField="Forwarder" />
                                    <asp:BoundField DataField="HAWB" />
                                    <asp:BoundField DataField="Satus" />
                                    <asp:BoundField DataField="LOC" />
                                   <asp:BoundField DataField="InWHCdt" />
                                </Columns>
                            </iMES:GridViewExt>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
               </tr>
             </table>
            </fieldset>
        </td>
    </tr>
    
    
    <tr>
        <td>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
	            <ContentTemplate>
                    <button id="btnGridFresh" runat="server" type="button" style="display: none" />
                    <button id="btnOnQueryIN" runat="server" type="button" style="display: none" />
                    <button id="btnOnQueryNotIN" runat="server" type="button" style="display: none" />
                    <button id="btn7Days" runat="server" type="button" style="display: none" />
                    <button id="btnTotalCount" runat="server" type="button" style="display: none" />
                    <button id="btnOnInput" runat="server" type="button" style="display: none" />
                    <button id="btnOnRemove" runat="server" type="button" style="display: none" />
                    <input type="hidden" runat="server" id="HDateFrom" /> 
                    <input type="hidden" runat="server" id="HDateTo" /> 
                    <input type="hidden" runat="server" id="HPLT" /> 
                    <input type="hidden" runat="server" id="HRMPLT" /> 
	            </ContentTemplate>   
            </asp:UpdatePanel> 
        </td>
    </tr>
    </table>
    </center>
</div>

</asp:Content>
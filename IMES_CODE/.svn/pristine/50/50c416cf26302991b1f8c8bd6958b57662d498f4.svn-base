<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MaterialSend.aspx.cs" Inherits="SA_MaterialSend" Title="IMES MaterialControl--发料系统" ValidateRequest="false"%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

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

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
	<script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>
	  <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
	  <script type="text/javascript" src="../CommonControl/IMESUti/IMESUti.Message.js"></script>
	<script type='text/javascript' src="../CommonControl/JS/json2.js"></script>
	  <link rel="stylesheet" type="text/css" href="../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.multiselect.filter.css" />
    <script src="../js/jscal2.js "></script>
    <script src="../js/lang/cn.js "></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        
    </asp:ScriptManager>
   
    <div id="container" style="width: 95%;  border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table  width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width:110px">
                        <asp:Label ID="lblVendor" runat="server" CssClass="iMes_label_13pt" Text="Stage:"></asp:Label>
                    </td>
                    <td style="width:180px">
                        <asp:DropDownList ID="cmdstage" runat="server" Width="100%"  AutoPostBack="true" OnSelectedIndexChanged="cmdstage_Selected"></asp:DropDownList>
                    </td>
                  <td style="width:110px">
                     <asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt" Text="Pdline:"></asp:Label>
                    </td>
                      <td style="width:180px">
                       <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                          <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" AutoPostBack="true" />
                           
                        </ContentTemplate>                                        
                     </asp:UpdatePanel>
            
            
                             
                    </td>
                    
                     <td style="width:110px">
                         <input id="btquery" type="button"  runat="server"  class="iMes_button"  value="查询"  onclick="if(clkQuery())"  onserverclick="btquery_ServerClick"/></td>
                  
                </tr>
                <tr>
                 <td style="width:110px">
                       
                     <asp:Label ID="Label5" runat="server" CssClass="iMes_label_13pt" Text="Family:"></asp:Label>
                       
                    </td>
                    <td style="width:180px">
                         
                        
                            <asp:TextBox ID="txtfamily" runat="server"></asp:TextBox>
                      
                        
                    </td>
                     <td style="width:110px">
                      <asp:Label ID="Label4" runat="server" CssClass="iMes_label_13pt" Text="PartNo:"></asp:Label>
                         </td>
                     <td style="width:180px">
                     <asp:TextBox ID="txtpartno" runat="server" Width="100%"></asp:TextBox>
                          </td>
                    
                    
                        <td style="width:110px">
                         <input id="btdel" type="button"  runat="server"  class="iMes_button"  value="发料"
                         onclick="if(clkUpdate())" 
                            onmouseout="this.className='iMes_button_onmouseout'"
                              onmouseover="this.className='iMes_button_onmouseover'" 
                                onserverclick="btupdate_ServerClick" /></td>
                    
                </tr>
                <tr>
                     
                   
                         
               <td style="width:110px">
                        <asp:Label ID="lblFrom" runat="server" CssClass="iMes_label_13pt" Text="Date From:"></asp:Label>&nbsp;&nbsp;
                    </td>
                    <td style="width:280px">
                       
                     
	                    
	                  
	                     <input type="text" id="txtDateFrom" />
                        <button id="btnCal" type="button" style="width: 20px">...</button>
                        
                    </td>
                    
                     <td  >
                      <asp:Label ID="lblTo" runat="server" CssClass="iMes_label_13pt" Text="To:"></asp:Label>&nbsp;&nbsp;
                       
                     </td>
                       <td  >
                     <input type="text" id="txtDateTo" />
	                    <button type="button" id="btnTo" style="width: 20px">...</button>
                     </td>
                     
                     
                     <td align="right" style="width: 3%;">
                         <input id="btnAdd" runat="server" class="iMes_button" onclick="listenserver()" 
                            onmouseout="this.className='iMes_button_onmouseout'"
                              onmouseover="this.className='iMes_button_onmouseover'" 
                              type="button" value="监听" /></td>
                   
                </tr>
                <tr>
                <td style="width:110px">
                        <asp:Label ID="Label3" runat="server" CssClass="iMes_label_13pt" Text="Staus:"></asp:Label>&nbsp;&nbsp;
                    </td>
                    <td style="width:180px">
                       <asp:DropDownList ID="cmdstatus" runat="server" Width="100%" IsPercentage="true" >
                               <asp:ListItem></asp:ListItem>
                               <asp:ListItem Value="Waiting">Waiting</asp:ListItem>
                               <asp:ListItem Value="Approve" >Approve</asp:ListItem>
                           </asp:DropDownList>
                        
                    </td>
                    
                     <td style="width:110px">
                        <asp:Label ID="Label2" runat="server" CssClass="iMes_label_13pt" Text="Reamrk:"></asp:Label>&nbsp;&nbsp;
                    </td>
                    <td style="width:280px">
                        <asp:TextBox ID="txtremark" runat="server"  Width="100%" ></asp:TextBox>
                        
                    </td>
                    <td>
                    <input type="button" id="btnExport" runat="server" value="Export" 
                            onserverclick="btnToExcel_ServerClick" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"  disabled=disabled/>
                    </td>
                    
                </tr>
            </table>
        </div>
        <div id="div2">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                <ContentTemplate>
                    <fieldset>
                        <legend >待发料:
                        <asp:Label ID="waitqty" runat="server" Text="0" Font-Bold="True" Font-Size="Large" 
                                ForeColor="#FF3300" ></asp:Label>
                        </legend>
                      
                        <div id="div4" style="height:155px">
                            <iMES:GridViewExt ID="gd" runat="server" EnableViewState="true"
                               
                                AutoGenerateColumns="false" 
                                SetTemplateValueEnable="True" 
                                GetTemplateValueEnable="True"
                                GvExtHeight="150px"
                                GvExtWidth="100%" 
                                style="top: 0px; left: 0px" 
                                Height="140" 
                                HighLightRowPosition="3" 
                                AutoHighlightScrollByValue="True"
                                onrowdatabound="gd_RowDataBound"
                                  OnDataBound="gd_DataBound"
                                OnGvExtRowClick='clickTable(this)'>
                                 <Columns>
                                       
                                        <asp:BoundField DataField="ID"  />
                                        <asp:BoundField DataField="PartNo" />
                                        <asp:BoundField DataField="Stage" />
                                        <asp:BoundField DataField="Family" />
                                        <asp:BoundField DataField="PCBModelID" />
                                        <asp:BoundField DataField="Line" />
                                        <asp:BoundField DataField="Location" />
                                        <asp:BoundField DataField="Qty" />
                                        <asp:BoundField DataField="Status" />
                                          <asp:BoundField DataField="Loc" />   
                                          <asp:BoundField DataField="AssignUser" />
                                          <asp:BoundField DataField="Remark" />   
                                             <asp:BoundField DataField="Editor" />    
                                                <asp:BoundField DataField="Cdt" />     
                                                  <asp:BoundField DataField="Udt" />     
                                         
                                         <asp:TemplateField>
                                            <ItemTemplate>
                                                 <asp:CheckBox id="chk" runat="server"  />
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="CheckAllBox" runat="server"
                                                    onclick="javascript: SelectAllCheckboxes(this);"  />
                                            </HeaderTemplate>
                                            <ControlStyle Width="10px" />
                                            <HeaderStyle Width="10px" />
                                            <ItemStyle Width="10px" />
                                        </asp:TemplateField>
                                    </Columns>
                            </iMES:GridViewExt> 
                            
                               
                        </div>
                    </fieldset>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btquery" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
             <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                <ContentTemplate>
                    <fieldset>
                        <legend  >已经发料:
                        <asp:Label ID="approveqty" runat="server" Text="0" Font-Bold="True" 
                                Font-Size="Large" ForeColor="Red"></asp:Label>
                        </legend>
                        <div id="div5" style="height:155px">
                            <iMES:GridViewExt ID="gd2" runat="server" 
                                AutoGenerateColumns="true" 
                                GvExtHeight="150px"
                                GvExtWidth="100%" 
                                style="top: 0px; left: 0px" 
                                Height="140" 
                                SetTemplateValueEnable="False" 
                                HighLightRowPosition="3" 
                                AutoHighlightScrollByValue="True"
                                >
                                
                            </iMES:GridViewExt> 
                        </div>
                    </fieldset>
                    
                </ContentTemplate>
                <Triggers>
                   <asp:AsyncPostBackTrigger ControlID="btquery" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
          
        </div>
  <input type="hidden" id="hidDeleteID" runat="server" />
    <input id="selectfromdate" type="hidden" runat="server" />
        <input id="selecttodate" type="hidden" runat="server" />
    </div>   
   
    <script language="javascript" type="text/javascript">

        var AccountId = '<%=Request["AccountId"] %>';
        var Login = '<%=Request["Login"] %>';
        var editor;
        var customer;
        var station;
        var pCode;
		var sn = "";
		var custsn = "";
		var mbsn = "";
		var mono = "";
		var proId = "";
		var selectedRowIndex = -1;
		var webservicesend = '<%=System.Configuration.ConfigurationManager.AppSettings["MessageSendUrl"]%>';
		var webserviceinject = '<%=System.Configuration.ConfigurationManager.AppSettings["MessageInjectUrl"]%>';
		var WebSocketObj = IMESUti.Message;
		window.onload = function() {
		    $('td[id^="td"]').css("color", "blue");

		    station = '<%=Request["Station"] %>';
		    pCode = '<%=Request["PCode"] %>';
		    d = new Date();
		    now_year = d.getYear();
		    now_month = d.getMonth() + 1;
		    now_month = now_month >= 10 ? now_month : "0" + now_month;
		    now_date = d.getDate();
		    now_date = now_date >= 10 ? now_date : "0" + now_date;

		    toattedDate = now_year + "-" + now_month + "-" + now_date+" 20:30";
		    document.getElementById("txtDateTo").value = toattedDate;
		    
		    d.setDate(d.getDate() - 1);
		    now_date = d.getDate();
		    now_date = now_date >= 10 ? now_date : "0" + now_date;
		    now_month = d.getMonth() + 1;
		    now_month = now_month >= 10 ? now_month : "0" + now_month;
		    now_year = d.getYear();

		    formattedDate = now_year + "-" + now_month + "-" + now_date + " 08:00";
		    document.getElementById("txtDateFrom").value = formattedDate;
		   
		    
		    EndRequestHandler();
		    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);



		};
		function EndRequestHandler(sender, args) {
		    Calendar.setup({
		        trigger: "btnCal",
		        inputField: "txtDateFrom",
		        onSelect: updateCalendarFields,
		        onTimeChange: updateCalendarFields,
		        showTime: 24,
		        dateFormat: "%Y-%m-%d %H:%M",
		        minuteStep: 1
		    });

		    Calendar.setup({
		    trigger: "btnTo",
		    inputField: "txtDateTo",
		        onSelect: updateCalendarFields,
		        onTimeChange: updateCalendarFields,
		        showTime: 24,
		        dateFormat: "%Y-%m-%d %H:%M",
		        minuteStep: 1
		    });
		};
		function checkselectdatetime() {
		    document.getElementById("<%=selectfromdate.ClientID %>").value = document.getElementById('txtDateFrom').value;
		    document.getElementById("<%=selecttodate.ClientID %>").value = document.getElementById('txtDateTo').value;
		}
		
		function clickTable(con) {
		    selectedRowIndex = parseInt(con.index, 10);
            setGdHighLight(con);
            ShowRowEditInfo(con);
        }

        var iSelectedRowIndex = null;
        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }
        function SetGrDivHeight() {
            document.getElementById("div_<%=gd.ClientID %>").style.height = "600px";
        }
        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }
            if (con.cells[0].innerText.trim() == "") {
                setNewItemValue();
                return;
            }
           setDropDownList(document.getElementById("<%=cmdstage.ClientID %>"), con.cells[2].innerText.trim());
            //            setDropDownList(document.getElementById("<%=cmbPdLine.InnerDropDownList.ClientID %>"), con.cells[5].innerText.trim());
          //  document.getElementById("<%=txtfamily.ClientID %>").value = con.cells[3].innerText.trim();
          //  document.getElementById("<%=txtpartno.ClientID %>").value = con.cells[1].innerText.trim();
           document.getElementById("<%=txtremark.ClientID %>").value = con.cells[11].innerText.trim();
          
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
        function setNewItemValue() {
         
        }

        function query() {
            ShowInfo("");
        }

      

        function onSaveSucess(result) {
            ShowInfo("");
            endWaitingCoverDiv();
            setPrintItemListParam1(result[0], result[1]);
            printLabels(result[0], false);
            clearTable();
            setNewItemValue();
          
            //CallNextInput();
            ShowInfo("SUCCESS!","green");
        }

        function onSaveError(result) {
            endWaitingCoverDiv();
            clearTable();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }

       

        function clearTable() {
            try {
                ClearGvExtTable("<%=gd.ClientID%>", 6);
            
            } catch (e) {
                alert(e.description);
            }
        }
        
        function initPage() {
            sn = "";
            custsn = "";
            mbsn = "";
            mbno = "";
            proId = "";
            $('td[id^="td"]').text('');
        }
        window.onbeforeunload = function() {
            if (sn != "") {
                OnCancel();
                initPage();
            }
            WebSocketObj.DisconnectWS();
        };
        
        function OnCancel() {
             PageMethods.Cancel(sn);
         }
         function clkAdd() {

         }
         function clkUpdate() {
//            
//                 var tblObj = document.getElementById("<%=gd.ClientID %>");
//                 var row = tblObj.rows[selectedRowIndex + 1];
//                 var status = row.cells[8].innerText;
//                 if (status != "Waiting") {
//                     alert("您选择的数据状态为：" + status + "不允许发料！");
//                     return false;
//                 }
//                 if (confirm("确定要发料么? ID=" + row.cells[0].innerText)) {
//                     document.getElementById("<%=hidDeleteID.ClientID %>").value = row.cells[0].innerText;
//                      selectedRowIndex = -1;
//                      return true;
//                }
//                return false;


                var _c = 0;
                var dnArr = new Array();
                var dnUdtArr = [];
                $("#" + "<%=gd.ClientID %>" + " tr:gt(0)").each(
                       function(i, obj) {
                           //tblDNObj.each.find("tr:gt(0)")(function(i, obj) {
                           var tr = $(this);
                           var _id = tr.find("td:eq(0)").text();
                           var _ox = tr.find("td:eq(15)").find('input:checkbox');
                           if (_ox.length > 0 && _ox.attr("checked") == "checked") {
                               // ctlSelectedId.value = ctlSelectedId.value + _id + ',';
                               dnArr.push(_id);
                           }
                       });
                if (dnArr.length == 0) {
                    alert("Please Select");
                    return false;
                }
                if (confirm("确定要发料 " + dnArr.length + " 条记录么?")) {
                    var idlist = "";
                    for (var m in dnArr) {
                        if (dnArr[m] != "") {
                            idlist = idlist + dnArr[m] + ',';
                        }
                    }
                    document.getElementById("<%=hidDeleteID.ClientID %>").value = idlist;
                    selectedRowIndex = -1;
                    return true;
                }
                return false;
             
           
         }
         function unpdateComplete() {
             document.getElementById("<%=hidDeleteID.ClientID %>").value = "";
             sendmessgetoserver();
             clearDetailInfo();

         }
         //发送消息
         function sendmessgetoserver() {
             var stage = document.getElementById("<%=cmdstage.ClientID %>").value;
             var pdline = document.getElementById("<%=cmbPdLine.InnerDropDownList.ClientID %>").value;
             var partno = document.getElementById("<%=txtpartno.ClientID %>").value;
             var Subject = stage + "SendPart-" + pdline;
             var messageInput = {
                 Stage: stage,
                 Pdline: pdline,
                 PartNo: partno
             };
             var data = JSON.stringify(messageInput);
             WebSocketObj.Publish(webservicesend, Subject, data, sendsuccesscallback, senderrorcallback);
         }
         function sendsuccesscallback(url, subject, data, responseText) {
             ShowInfo("SUCCESS!--->>>《发料信息》已经通知到材料需求者！！ " + "主旨:" + subject, "green");
         }
         function senderrorcallback(url, subject, data, status) {
             ShowMessage("Error!--->>>呼叫服务器失败" + status);
             ShowInfo("Error!--->>>呼叫服务器失败" + status, "red");

         }
         
         function clearDetailInfo() {
             setDropDownList(document.getElementById("<%=cmdstage.ClientID %>"), "");


         }

         function clkQuery() {
             checkselectdatetime();
             var stage = document.getElementById("<%=cmdstage.ClientID %>").value;
             var status = document.getElementById("<%=cmdstatus.ClientID %>").value;
             var pdline = document.getElementById("<%=cmbPdLine.InnerDropDownList.ClientID %>").value;
             var partno = document.getElementById("<%=txtpartno.ClientID %>").value;
             var begin = document.getElementById('txtDateFrom').value;
             var end = document.getElementById('txtDateTo').value;
             
             
             if (stage == "") {
                 alert("请选择Stage！");
                 return false;
             }
             if (begin == "" || end == "") {
                 alert("请选择时间！");
                 return false;
             }
             if (begin > end) {
                 alert("Begin Time 不能打印结束时间！");
                 return false;
             }
             document.getElementById("<%=btnExport.ClientID %>").disabled = false;
             return true;


         }
         //接受信息
         function listenserver() {

             var stage = document.getElementById("<%=cmdstage.ClientID %>").value;
             var pdline = document.getElementById("<%=cmbPdLine.InnerDropDownList.ClientID %>").value;
             var Subject = stage + "RequestPart-" + pdline;//监听叫料主旨
             //订阅数据
             WebSocketObj.Subscribe(webserviceinject, Subject, OnSentCallBack, OnMessageFunctioncallback, OnOpenFunctioncallback, onCloseFunction);
         }
         function OnSentCallBack(wsUrl, sendData,subject) {
             ShowInfo("已经成功监听《材料需求单位》,任何材料需求将会通知到本界面（本界面不能关闭！）！ 主旨：" + subject);
         }
         function OnMessageFunctioncallback(rawData, Context) {
             ShowSuccessfulInfo(true, "收到新的叫料需求！ " + Context, true);
             //ShowInfo("收到新消息！ " + Context);
         }
         function OnOpenFunctioncallback() { 
              ShowInfo("连接监听服务成功！ "); }
         function onCloseFunction() {
             ShowInfo("关闭监听服务！ " );
         }

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

    </script>
   

</asp:Content>
 



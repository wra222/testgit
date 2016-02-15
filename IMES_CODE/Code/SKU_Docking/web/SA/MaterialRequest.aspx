<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MaterialRequest.aspx.cs" Inherits="SA_MaterialRequest" Title="IMES MaterialControl--叫料系统" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

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
        .iframestyle{ height:80%;width:100%;z-index:-1;background: #FFFFF0 no-repeat;}
  </style>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
	<script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>
	<script type="text/javascript" src="../CommonControl/IMESUti/IMESUti.Message.js"></script>
	<script type='text/javascript' src="../CommonControl/JS/json2.js"></script>
	
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table  width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width:110px">
                        <asp:Label ID="lblVendor" runat="server" CssClass="iMes_label_13pt" Text="Stage:"></asp:Label>
                    </td>
                    <td style="width:180px">
                        <asp:DropDownList ID="cmdstage" runat="server" Width="100%" IsPercentage="true" ></asp:DropDownList>
                    </td>
                  <td style="width:110px">
                     <asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt" Text="Pdline:"></asp:Label>
                    </td>
                      <td style="width:180px">
                               <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                    </td>
                    
                     <td style="width:110px">
                     <asp:Label ID="Label5" runat="server" CssClass="iMes_label_13pt" Text="Family:"></asp:Label>
                    </td>
                      <td style="width:180px">
                            <asp:TextBox ID="txtfamily" runat="server"></asp:TextBox>
                      
                    </td>
                   <td align="right" style="width: 3%;">
                          <input id="btnAdd" runat="server" class="iMes_button" onclick="if(clkAdd())" 
                            onmouseout="this.className='iMes_button_onmouseout'"
                              onmouseover="this.className='iMes_button_onmouseover'" onserverclick="btnAdd_ServerClick"
                              type="button" value="叫料" />
                    </td>
                  
                </tr>
                <tr>
                    
                   <td style="width:110px">
                        <asp:Label ID="lblTotalQty" runat="server" CssClass="iMes_label_13pt" Text="Model:"></asp:Label>
                    </td>
                     <td style="width:180px">
                        <asp:TextBox ID="txtmodel" runat="server" Width="100%"></asp:TextBox>
                    </td>
                    
                     <td style="width:110px">
                        <asp:Label ID="Label2" runat="server" CssClass="iMes_label_13pt" Text="Location:"></asp:Label>
                    </td>
                     <td style="width:180px">
                          <asp:TextBox ID="txtlocation" runat="server" Width="100%"></asp:TextBox>
                    </td>
                    
                    
                        <td style="width:110px">
                        <asp:Label ID="Label3" runat="server" CssClass="iMes_label_13pt" Text="Qty:"></asp:Label>
                    </td>
                     <td style="width:180px">
                         <asp:TextBox ID="txtqty" runat="server" Width="100%"></asp:TextBox>
                    </td>
                     <td align="right" style="width: 3%;">
                         <input id="btdel" type="button"  runat="server"  class="iMes_button"  value="删除"
                         onclick="if(clkDelete())" 
                            onmouseout="this.className='iMes_button_onmouseout'"
                              onmouseover="this.className='iMes_button_onmouseover'" onserverclick="btdel_ServerClick" />
                    </td>
                    
                </tr>
                <tr>
                     <td style="width:110px">
                        <asp:Label ID="Label4" runat="server" CssClass="iMes_label_13pt" Text="PartNo:"></asp:Label>
                    </td>
                    <td style="width:180px">
                         <asp:TextBox ID="txtpartno" runat="server" Width="100%"></asp:TextBox>
                        
                    </td>
                    <td style="width:110px">
                        <asp:Label ID="Label6" runat="server" CssClass="iMes_label_13pt" Text="Remark:"></asp:Label>
                        </td>
                     <td style="width:280px" >
                        <asp:TextBox ID="txtremark" runat="server" Width="100%"></asp:TextBox>
                    </td>
                     <td style="width:110px">
                        
                        </td>
                     <td style="width:280px" >
                           
                    </td>
                     
                     
                     <td align="right" style="width: 3%;">
                         <input id="btquery" type="button"  runat="server"  class="iMes_button"  value="查询"  onclick="OpenQuery();" onserverclick="btquery_ServerClick"/>
                    </td>
                   
                </tr>
                <tr>
                     <td style="width:280px" colspan="6">
                    </td>
                      <td style="width:280px">
                       <input id="Button1" type="button"  runat="server"  class="iMes_button"  value="监听" onclick="listenserver()" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                <ContentTemplate>
                    <fieldset>
                        <legend>PartNo Summary</legend>
                        <div id="div4" style="height:355px">
                            <iMES:GridViewExt ID="gd" runat="server" 
                                AutoGenerateColumns="true" 
                                GvExtHeight="350px"
                                GvExtWidth="100%" 
                                style="top: 0px; left: 0px" 
                                Height="140" 
                                SetTemplateValueEnable="False" 
                                HighLightRowPosition="3" 
                                AutoHighlightScrollByValue="True"
                                onrowdatabound="gd_RowDataBound"
                                OnGvExtRowClick='clickTable(this)'>
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
    </div> 
    <div id="reportTableDiv" style="width: 85%; height: 768px; z-index: 999; position: absolute; top: 0px; left: 30px;display:none">
    
             
        	<iframe  class="iframestyle" id="Iframe4" src=""  frameborder="0" marginwidth="0" marginheight="0" scrolling="yes" allowtransparency="true" ></iframe>
        	

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
		};

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
            setDropDownList(document.getElementById("<%=cmdstage.ClientID %>") , con.cells[2].innerText.trim());
//            setDropDownList(document.getElementById("<%=cmbPdLine.InnerDropDownList.ClientID %>"), con.cells[5].innerText.trim());
            document.getElementById("<%=txtfamily.ClientID %>").value = con.cells[3].innerText.trim();
           document.getElementById("<%=txtmodel.ClientID %>").value = con.cells[4].innerText.trim();
           document.getElementById("<%=txtlocation.ClientID %>").value = con.cells[6].innerText.trim();
           document.getElementById("<%=txtqty.ClientID %>").value = con.cells[7].innerText.trim();
           document.getElementById("<%=txtpartno.ClientID %>").value = con.cells[1].innerText.trim();
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
            sendmessgetoserver();
            clearDetailInfo();
        }
        //发送消息
        function sendmessgetoserver() {
            var stage = document.getElementById("<%=cmdstage.ClientID %>").value;
            var pdline = document.getElementById("<%=cmbPdLine.InnerDropDownList.ClientID %>").value;
            var model = document.getElementById("<%=txtmodel.ClientID %>").value;
            var location = document.getElementById("<%=txtlocation.ClientID %>").value;
            var qty = document.getElementById("<%=txtqty.ClientID %>").value;
            var partno = document.getElementById("<%=txtpartno.ClientID %>").value;
            var Subject = stage +"RequestPart-"+ pdline;
            var messageInput = {
                Stage: stage,
                Pdline: pdline,
                Model: model,
                Location: location,
                Qty: qty,
                PartNo: partno
            };
            var data=JSON.stringify(messageInput);
            WebSocketObj.Publish(webservicesend, Subject, data, sendsuccesscallback, senderrorcallback);
        }
        function sendsuccesscallback(url, subject, data, responseText) {
            ShowInfo("SUCCESS!--->>>已经成功呼叫服务器 " + "主旨:" + subject, "green");
        }
        function senderrorcallback(url, subject, data, status) {
            ShowMessage("Error!--->>>呼叫服务器失败" + status);
            ShowInfo("Error!--->>>呼叫服务器失败" + status, "red");
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
        window.onbeforeunload = function() {
             WebSocketObj.DisconnectWS();
        };   
       
       
         function clkAdd() {
             var stage = document.getElementById("<%=cmdstage.ClientID %>").value;
             var pdline = document.getElementById("<%=cmbPdLine.InnerDropDownList.ClientID %>").value;
             var model = document.getElementById("<%=txtmodel.ClientID %>").value;
             var location = document.getElementById("<%=txtlocation.ClientID %>").value;
             var qty = document.getElementById("<%=txtqty.ClientID %>").value;
             var partno = document.getElementById("<%=txtpartno.ClientID %>").value;
             if (stage == "" || pdline == "" || model == "" || location == "" || qty == "" || partno == "") {
                 alert("请填写完整信息！！");
                 return false;
             }
             if (isNaN(qty)) {
                 alert("数量格式不对！！");
                 return false;
             }
            
             return true;
         }
         function clkDelete() {
            

             var tblObj = document.getElementById("<%=gd.ClientID %>");
             var row = tblObj.rows[selectedRowIndex + 1];
             var status = row.cells[8].innerText;
             if (status != "Waiting") {
                 alert("您选择的数据状态为：" + status + "不允许删除！");
                 return false;
             }
             if (confirm("确定要删除这条记录么? ID=" + row.cells[0].innerText)) {
                 document.getElementById("<%=hidDeleteID.ClientID %>").value = row.cells[0].innerText;
                 selectedRowIndex = -1;
                 clearDetailInfo();
                 return true;
             }
             return false;
             
         }
         function DeleteComplete() {
             document.getElementById("<%=hidDeleteID.ClientID %>").value = "";
         }
         function clearDetailInfo() {
             setDropDownList(document.getElementById("<%=cmdstage.ClientID %>"), "");
       
             document.getElementById("<%=txtfamily.ClientID %>").value = "";
             document.getElementById("<%=txtmodel.ClientID %>").value = "";
             document.getElementById("<%=txtlocation.ClientID %>").value ="";
             document.getElementById("<%=txtqty.ClientID %>").value = "";
             document.getElementById("<%=txtpartno.ClientID %>").value = "";
             document.getElementById("<%=txtremark.ClientID %>").value = "";
         }
         //接受信息
         function listenserver() {

             var stage = document.getElementById("<%=cmdstage.ClientID %>").value;
             var pdline = document.getElementById("<%=cmbPdLine.InnerDropDownList.ClientID %>").value;
             var Subject = "SendPart-" 
             //订阅数据
             WebSocketObj.Subscribe(webserviceinject, Subject, OnSentCallBack, OnMessageFunctioncallback, OnOpenFunctioncallback, onCloseFunction);
         }
         function OnSentCallBack(wsUrl, sendData, subject) {
             ShowInfo("订阅成功！ 主旨：" + subject);
         }
         function OnMessageFunctioncallback(rawData, Context) {
             ShowSuccessfulInfo(true, "收到新消息！ " + Context, true);
             //ShowInfo("收到新消息！ " + Context);
         }
         function OnOpenFunctioncallback() {
             ShowInfo("连接监听服务成功！ ");
         }
         function onCloseFunction() {
             ShowInfo("关闭监听服务！ ");
         }
         function OpenQuery() {
             var editor = '<%=Master.userInfo.UserId%>';
            // var dlgFeature = "dialogHeight:768px;dialogWidth:1024px;center:yes;status:no;help:no";
            // var dlgReturn = window.showModalDialog("MaterialSend.aspx?UserId=" + editor + "&Query=Y", window, dlgFeature);
             document.getElementById("Iframe4").src = "MaterialSend.aspx?UserId=" + editor + "&Query=Y";
             $("#reportTableDiv").fadeToggle(1000);
         }
    

    </script>
</asp:Content>

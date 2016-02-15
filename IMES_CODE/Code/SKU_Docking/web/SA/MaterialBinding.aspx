<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MaterialBinding.aspx.cs" Inherits="SA_MaterialBinding" Title="IMES MaterialControl--绑定系统" %>
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
                        <asp:Label ID="Label4" runat="server" CssClass="iMes_label_13pt" Text="工号:"></asp:Label>
                    </td>
                    <td style="width:180px">
                         <asp:TextBox ID="txtAssignUser" runat="server" Width="100%" MaxLength="9"></asp:TextBox>
                        
                    </td>
                    <td style="width:110px">
                        <asp:Label ID="Label6" runat="server" CssClass="iMes_label_13pt" Text="MB号:"></asp:Label>
                        </td>
                     <td style="width:280px" >
                        <asp:TextBox ID="txtPCBNo" runat="server" Width="100%" MaxLength="14"></asp:TextBox>
                    </td>
                     
                     
                   <td align="right" style="width: 3%;">
                          <input id="btnAdd" runat="server" class="iMes_button" onclick="if(clkAdd())" 
                            onmouseout="this.className='iMes_button_onmouseout'"
                              onmouseover="this.className='iMes_button_onmouseover'" onserverclick="btnAdd_ServerClick"
                              type="button" value="保存" />
                    </td>
                   
                </tr>

            </table>
        </div>
        <div id="div2">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
               <ContentTemplate>
<%--                    <fieldset>
                        <legend>PartNo Summary</legend>
                        <div id="div4" style="height:355px">
                            <iMES:GridViewExt ID="gd" runat="server" 
                                GvExtHeight="350px"
                                GvExtWidth="100%" 
                                style="top: 0px; left: 0px" 
                                Height="140px" 
                                SetTemplateValueEnable="False" 
                                HighLightRowPosition="3" 
                                AutoHighlightScrollByValue="True"
                                onrowdatabound="gd_RowDataBound"
                                OnGvExtRowClick='clickTable(this)' GetTemplateValueEnable="False" 
                                HiddenColCount="0">
                            </iMES:GridViewExt> 
                        </div>
                    </fieldset>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
       <input type="hidden" id="hidDeleteID" runat="server" />
    </div> 
    <div id="reportTableDiv" style="width: 85%; height: 768px; z-index:-1; position: absolute; top: 0px; left: 30px;display:none">
    
             
        	<iframe  class="iframestyle" id="Iframe4" src=""  frameborder="0" marginwidth="0" marginheight="0" scrolling="yes" allowtransparency="true" ></iframe>
        	

    </div>  
   
    <script language="javascript" type="text/javascript">
        var webservicesend = '<%=System.Configuration.ConfigurationManager.AppSettings["MessageSendUrl"]%>';
		var WebSocketObj = IMESUti.Message;

		window.onload = function() {
		    $('td[id^="td"]').css("color", "blue");
		};

        function onSaveSucess(result) {
            sendmessgetoserver();
            clearDetailInfo();
        }

      //发送消息
        function sendmessgetoserver() {
            var stage = "";
            var pdline = "";
            var model = "";
            var location = "";
            var PCBNo = document.getElementById("<%=txtPCBNo.ClientID %>").value.toUpperCase();
            var AssignUser = document.getElementById("<%=txtAssignUser.ClientID %>").value.toUpperCase();
            var Subject = PCBNo
            var messageInput = {
                Stage: stage,
                Pdline: pdline,
                Model: model,
                Location: location,
                AssignUser: AssignUser
            };
            var data=JSON.stringify(messageInput);
            WebSocketObj.Publish(webservicesend, Subject, data, sendsuccesscallback,senderrorcallback);
        }
        function sendsuccesscallback(url, subject, data, responseText) {
            ShowInfo("SUCCESS!--->>>已经成功保存 " + "MB号:" + subject+"！！", "green");
        }
        function senderrorcallback(url, subject, data, status) {
            var subject = document.getElementById("<%=txtPCBNo.ClientID %>").value.toUpperCase();
            ShowInfo("Error!--->>>保存失败" + "MB号:" + subject+"已存在！！", "red");
            clearDetailInfo();
        }
        function onSaveError(result) {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }
         function clkAdd() {
             var AssignUser = document.getElementById("<%=txtAssignUser.ClientID %>").value.toUpperCase();
             var PCBNo = document.getElementById("<%=txtPCBNo.ClientID %>").value.toUpperCase();
             
             if ( PCBNo==""|| AssignUser == ""||AssignUser.length!=9) 
             {
                 alert("请填写完整信息！！");
                 return false;
             }
//             if (AssignUser.substring(0,3)!="ICC") 
//             {
//                  alert("工号输入错误！！");
//                  return false;
//             }
             
         if (!(PCBNo.length == 10 || PCBNo.length == 11)) {
             alert("MB号位数错误！！");

             return false;
         }

         if ((PCBNo.substr(4, 1) != "M") && (PCBNo.substr(5, 1) != "M")) {
             if ((PCBNo.substr(4, 1) != "B") && (PCBNo.substr(5, 1) != "B")) {
                 alert("MB号错误！！");

                 return false;
             }
         }

         return true;
         }
         function clearDetailInfo() {
             document.getElementById("<%=txtAssignUser.ClientID %>").value;
             document.getElementById("<%=txtPCBNo.ClientID %>").value = "";
         }

    </script>
</asp:Content>





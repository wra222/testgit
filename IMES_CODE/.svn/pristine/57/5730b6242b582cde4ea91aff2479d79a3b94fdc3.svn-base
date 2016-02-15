<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Unit Weight
 * UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx –2011/11/25
 * UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx –2011/11/25         
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-25   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：   新增页面
 *
 * UC Revision：  4078
 */
--%>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ModelWeight.aspx.cs" Inherits="PAK_ModelWeight" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
        <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
     <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceModelWeight.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="z-index: 0;">
        <div style="width:100%;height:20px;"></div>
        <div id="div1" style="margin-top:5px;margin-bottom:5px;">
             <table  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
                <tr>
                    <td style="width:22%;height:32px;">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="78%">
                        <asp:TextBox ID="dModel" runat="server"   MaxLength="20"  Width="92%"  class="iMes_textbox_input_Yellow"  onkeypress="inputNumberAndEnglishChar(this)"  onkeydown="TabModel()" ></asp:TextBox>
                    </td>   
                    
                </tr>
                <tr >                           
                    <td style="width:22%;height:32px;">
                        <asp:Label ID="lblStandardWeight" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="78%">                  
                        <asp:TextBox ID="dStandardWeight" runat="server"   MaxLength="8"  Width="92%"  class="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)' ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td style="width:22%;height:32px;">
                     <asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt" Text="Defect Code:"></asp:Label>
                    </td>
                 <td width="78%">
                     <asp:DropDownList ID="droDefect" runat="server" Height="25px" Width="350px">
                     </asp:DropDownList>
                 </td>
                </tr>
                <tr>
                 <td style="width:22%;height:32px;">
                </td>
                <td width="78%">
                    <input id="btnH" type="button" value="Hold And Save" onclick="HoldAndSave()"  disabled="disabled"
                        style="width: 150px" />
                       <input id="btnS" type="button" value="Save" onclick="Save()" style="width: 150px"  />
                          <input id="btnC" type="button" value="Cancel" onclick="Cancel()" style="width: 150px"  />
                </td>
              
                </tr>
                <tr>
                <td style="height:32px;"colspan="2">
                    <asp:Label ID="labHoldMsg" runat="server" Text="" ForeColor="Green" Font-Size="X-Large"></asp:Label>
                </td>
                </tr>
             </table>  
                                                    
              
        </div>
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGetUnitWeight" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                   <asp:AsyncPostBackTrigger ControlID="btnHoldAndSave" EventName="ServerClick" />
            </Triggers> 
            <ContentTemplate>
            <input id="Hidd_Model" type="hidden" runat="server" />       
                <input id="hidMessage" type="hidden" runat="server" />
            <input id="hid_PrdLsit" type="hidden" runat="server" />
             
            </ContentTemplate>                          
        </asp:UpdatePanel>         
           
           <button id="btnGetUnitWeight" runat="server" type="button" style="display:none" onserverclick ="btnGetUnitWeight_ServerClick"> </button>
           <button id="btnSave" runat="server" type="button" style="display:none" onserverclick ="btnSave_ServerClick"> </button>
           <button id="btnHoldAndSave" runat="server" type="button" style="display:none" onserverclick ="btnHoldAndSave_ServerClick"> </button>
            <input id="hidDefect" type="hidden" runat="server" />
        
    </div>
   
    <script language="javascript" type="text/javascript">
       
        var msgConfirm = '<%=this.GetLocalResourceObject(Pre + "_msgConfirm").ToString() %>';
        
    var msgWrongModel = '<%=this.GetLocalResourceObject(Pre + "_msgWrongModel").ToString() %>';
    var msgWrongFormat = '<%=this.GetLocalResourceObject(Pre + "_msgWrongFormat").ToString() %>';
    var msgSaveSucc = '<%=this.GetLocalResourceObject(Pre + "_msgSaveSucc").ToString() %>';
    var msgMaxWeight = '<%=this.GetLocalResourceObject(Pre + "_msgMaxWeight").ToString() %>';
    var editor = "<%=UserId%>";
    var customer = '<%=Customer %>';
    var checkModelFlag = false;
    var weightTemp = "";
    var inputModel = "";
    var inputcustsn = "";
    window.onload = function() {
          inputcustsn = '<%=Request["InputCustSN"] %>';
        trySetFocusModel();
      
    };
    function show(str) {
        execScript("n = msgbox('" + str + "',3,'')", "vbscript");
        return (n);
    }
    function SetHoldMsg(msg) {
        var id = "#" + "<%=labHoldMsg.ClientID %>";
       $(id).text(msg);
        
    }
    function GetSelectDefect() {
        var id = "#"+"<%=droDefect.ClientID %>";
        var s = $(id).val();
        document.getElementById("<%=hidDefect.ClientID %>").value = s;
        return s;
    }
    function trySetFocusModel() {
        var itemObj = document.getElementById("<%=dModel.ClientID %>");

        if (itemObj != null && itemObj != undefined && itemObj.disabled != true) {
            itemObj.focus();
        }
    }

    function trySetFocusStandardWeight() {
        ShowInfo("");
        var itemObj = document.getElementById("<%=dStandardWeight.ClientID %>");

        if (itemObj != null && itemObj != undefined && itemObj.disabled != true) {
            itemObj.focus();
        }
    }
   
    function TabModel() {
        if (event.keyCode == 9 || event.keyCode == 13) {
            if (document.getElementById("<%=dModel.ClientID%>").value != "") {
                var inputData = document.getElementById("<%=dModel.ClientID%>").value;
                if (inputcustsn == "Y") {
                    if (!CheckCustomerSN(inputData)) {
                        alert("you must Input CustSN!!!");
                        document.getElementById("<%=dModel.ClientID%>").value = "";
                        event.returnValue = false;
                        return false;
                    
                    }
                }
                else {
                    var reExp = /^[-0-9a-zA-Z\+\s\*]*$/;
                    if (reExp.exec(inputData) == null || reExp.exec(inputData) == false) {
                        alert(msgWrongModel);
                        document.getElementById("<%=dModel.ClientID%>").value = "";
                        event.returnValue = false;
                        return false;
                    }
                }
                
                beginWaitingCoverDiv();
                document.getElementById("<%=btnGetUnitWeight.ClientID %>").click();
                event.returnValue = false;
            }
        }
    }
    
    function OnKeyPress(obj)
    {
        var key = event.keyCode;
        if (key==13 || key==9)  //Enter or Tab
        {
         
            if (checkModelFlag && event.srcElement.id == "<%=dStandardWeight.ClientID %>") {
                if (check() == false) {
                    document.getElementById("<%=dStandardWeight.ClientID %>").value = weightTemp;
                   return;
                }
               return;
            }
            else trySetFocusModel();
        }

    }
    function EnabelAllBtn(b) {
        $("#btnH").attr("disabled", b);
//        $("#btnS").attr("disabled", b);
//        $("#btnC").attr("disabled", b);

    }
    function Cancel() {
        EnabelAllBtn(true);
        SetHoldMsg("");
        var dm = "#" + "<%=dModel.ClientID%>";
        $(dm).attr("disabled", false);
        ResetPage();
    }
    function Save() {
        if (check() == false) {
            document.getElementById("<%=dStandardWeight.ClientID %>").value = weightTemp;
            return;
        }
        beginWaitingCoverDiv();
        document.getElementById("<%=btnSave.ClientID %>").click();
    
    }
    function HoldAndSave() {
        if (check() == false) {
            document.getElementById("<%=dStandardWeight.ClientID %>").value = weightTemp;
            //  alert(msgWrongFormat);
            return;
        }
        var d = GetSelectDefect();
        if (d == "")
        { alert("Please select defect code!"); return; }
        beginWaitingCoverDiv();
        document.getElementById("<%=btnHoldAndSave.ClientID %>").click();
    }

    function check()
    {
    
       var valueWeight=document.getElementById("<%=dStandardWeight.ClientID %>").value;
        //则检查本框输入内容是否满足格式要求
        var reExp = /^[0-9]+(\.[0-9]?[0-9]?)?$/;
	    if (reExp.exec(valueWeight)==null||reExp.exec(valueWeight)==false){
	        alert(msgWrongFormat);
		    return false;
	    }
	    	    
	    if(parseFloat(valueWeight)>100)
	    {
	        alert(msgMaxWeight);
	        return false;
	    }
	    
        return true;
    }

    function HaveHold(m) {
        EnabelAllBtn(false);
       // var m = document.getElementById("<%=hidMessage.ClientID %>").value;
        SetHoldMsg(m);
    }

    function NoHold() {
        $("#btnS").attr("disabled", false);
        $("#btnC").attr("disabled", false);
        SetHoldMsg("");
    }
   
    
   function GetUnitWeightComplete(isGetOK, weightNum) {

       endWaitingCoverDiv();
       if(isGetOK=="True")
       {
           document.getElementById("<%=dStandardWeight.ClientID %>").value = weightNum;
           weightTemp = weightNum;
           checkModelFlag = true;
           inputModel = document.getElementById("<%=dModel.ClientID %>").value;
           var dm = "#" + "<%=dModel.ClientID%>";
           $(dm).attr("disabled", true);
           trySetFocusStandardWeight();
       }
       else 
       {           
           document.getElementById("<%=dModel.ClientID %>").value ="";
           document.getElementById("<%=dStandardWeight.ClientID %>").value = "";
           SetHoldMsg("");
       }       
   }
   
    
    function SaveComplete() {

        inputModel = document.getElementById("<%=dModel.ClientID %>").value;
        
      var SuccessItem = "[" +inputModel+ "]";
      ShowSuccessfulInfo(true, SuccessItem + " " + msgSaveSucc);
      ResetPage();
    }

    function ResetPage() {

        document.getElementById("<%=dModel.ClientID %>").value = "";
        document.getElementById("<%=dStandardWeight.ClientID %>").value = "";
        SetHoldMsg("");
        weightTemp = "";
        checkModelFlag = false;
        EnabelAllBtn(true);
        var dm = "#" + "<%=dModel.ClientID%>";
        $(dm).attr("disabled", false);
        trySetFocusModel();
       
    }
   
    function checkCodePress(obj)
    { 
       var key = event.keyCode;

       if(!(key >= 48 && key <= 57))
       {  
           event.keyCode = 0;
       }
 
     }
     
    </script>
</asp:Content>


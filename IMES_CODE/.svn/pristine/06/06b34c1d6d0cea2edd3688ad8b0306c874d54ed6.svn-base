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
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ModelWeightDocking.aspx.cs" Inherits="Docking_ModelWeight" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
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
             </table>  
                                                    
              
        </div>
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGetUnitWeight" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            </Triggers> 
            <ContentTemplate>
            <input id="Hidd_Model" type="hidden" runat="server" />       
            </ContentTemplate>                          
        </asp:UpdatePanel>         
           
           <button id="btnGetUnitWeight" runat="server" type="button" style="display:none" onserverclick ="btnGetUnitWeight_ServerClick"> </button>
           <button id="btnSave" runat="server" type="button" style="display:none" onserverclick ="btnSave_ServerClick"> </button>
           
    </div>
   
    <script language="javascript" type="text/javascript">

    var msgWrongModel = '<%=this.GetLocalResourceObject(Pre + "_msgWrongModel").ToString() %>';
    var msgWrongFormat = '<%=this.GetLocalResourceObject(Pre + "_msgWrongFormat").ToString() %>';
    var msgSaveSucc = '<%=this.GetLocalResourceObject(Pre + "_msgSaveSucc").ToString() %>';
    var msgMaxWeight = '<%=this.GetLocalResourceObject(Pre + "_msgMaxWeight").ToString() %>';
    var editor = "<%=UserId%>";
    var customer = '<%=Customer %>';
    var checkModelFlag = false;
    var weightTemp = "";

    window.onload = function() {

        trySetFocusModel();
    };
    
      
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
                var reExp = /^[-0-9a-zA-Z\+\s\*]*$/;
                if (reExp.exec(inputData) == null || reExp.exec(inputData) == false) {
                    alert(msgWrongModel);
                    document.getElementById("<%=dModel.ClientID%>").value = "";
                    event.returnValue = false;
                    return false;
                }
                
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
                  //  alert(msgWrongFormat);
                    return;
                }
                document.getElementById("<%=btnSave.ClientID %>").click();

            }
            else trySetFocusModel();
        }       

    }
    
    //!!!
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

   
    
   function GetUnitWeightComplete(isGetOK, weightNum)
   {   
       if(isGetOK=="True")
       {
           document.getElementById("<%=dStandardWeight.ClientID %>").value = weightNum;
           weightTemp = weightNum;
           checkModelFlag = true;
           trySetFocusStandardWeight();
       }
       else 
       {           
           document.getElementById("<%=dModel.ClientID %>").value ="";
           document.getElementById("<%=dStandardWeight.ClientID %>").value ="";
       }       
   }
   
    
    function SaveComplete()
    {
       var SuccessItem = "[" + document.getElementById("<%=dModel.ClientID %>").value + "]";
       ShowSuccessfulInfo(true, SuccessItem + " " + msgSaveSucc);

       ResetPage();
    }

    function ResetPage() {

        document.getElementById("<%=dModel.ClientID %>").value = "";
        document.getElementById("<%=dStandardWeight.ClientID %>").value = "";

        weightTemp = "";
        checkModelFlag = false;
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


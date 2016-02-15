<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Dismantle page
 * UI:CI-MES12-SPEC-FA-UI ProdId Dismantle For Docking.docx
 * UC:CI-MES12-SPEC-FA-UC ProdId Dismantle For Docking.docx        
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-06-08  Zhang.Kai-sheng        Create
 * TODO:
 * Known issues:
 */
 --%>

 
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DismantleDocking.aspx.cs" Inherits="Docking_Dismantle" Title="Untitled Page" %>

  
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

<asp:ScriptManager runat="server" ID="SM"  EnablePartialRendering="true">
    <Services>
        <asp:ServiceReference Path="Service/WebServiceDismantleDocking.asmx" />
    </Services>
</asp:ScriptManager>

<div> 
  
 <table width="98%">
    <tr>
        <td nowrap="noWrap" style="width: 110px;">
            <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
        </td>
        <td>
            <iMES:Input ID="txt" runat="server" IsClear="false"  ProcessQuickInput="true"  Width="99%"
                CanUseKeyboard="true" IsPaste="true"  MaxLength="30"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
        </td>
    </tr>
    <tr>
        <td align ="right" colspan="2">
            <button  id ="btnDismantle"  style ="width:120px; height:28px;" onclick="btnDismantleClick()" >
                <%=Resources.iMESGlobalDisplay.ResourceManager.GetObject(languagePre + "btnDismantle").ToString()%>
            </button> 
       </td>  
    </tr>
</table>

</div>
<script language ="javascript" type ="text/javascript"  >
 
var inputControl=getCommonInputObject();
var editor;
var customer;
var station;
var Pcode;
var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "msgSystemError") %>';
var msgConfirmDismantle = '<%=this.GetLocalResourceObject(languagePre + "msgConfirmDismantle").ToString()%>';
var msgNoInput = '<%=this.GetLocalResourceObject(languagePre + "msgNoInput").ToString()%>';
var msgWrongCode = '<%=this.GetLocalResourceObject(languagePre + "WrongCode").ToString()%>';

var msgNoAvailable = '<%=this.GetLocalResourceObject(languagePre + "msgNoAvailable").ToString()%>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "msgSuccess") %>';
var msgdismantle = '<%=this.GetLocalResourceObject(languagePre + "Dismantle") %>';
var strProdIDorSN="";

document.body.onload = function() {
    try {
        editor = '<%=UserId%>';
        customer = '<%=Customer%>';
        station = '<%=Station%>';
        Pcode = '<%=pCode%>';
        inputControl.focus();
        getAvailableData("processDataEntry");
    }
    catch (e) {
        //alert(e.description);
        ShowMessage(e.description);
        ShowInfo(e.description);

    }
}
function V_endWaitingCoverDiv() 
{
    endWaitingCoverDiv();
}
///---------------------------------------------------
///| Name		    :	processDataEntry
///| Description	:	handle Input Data (Data Entry)
///| Input para.	:	
///| Ret value	:
///---------------------------------------------------
function processDataEntry(inputData) 
{
    //beginWaitingCoverDiv();
    //WebServiceFADismantle.inputProdId(inputData, "", Pcode, editor, station, customer, onProdIdSucceed, onProdIdFailed);
    if ((inputData.length == 9) || (inputData.length == 10))
    {
        ShowInfo(""); 
        beginWaitingCoverDiv();
        //if ((inputData.trim().length == 10) && (inputData.trim().substr(0, 3) == "CNU")) {
		if (CheckCustomerSN(inputData.trim())) {
            strProdIDorSN = inputData.trim().toUpperCase();
        }
        else {
            strProdIDorSN = SubStringSN(inputData.trim().toUpperCase(), "ProdId");
        }
        btnDismantleClick();
        //WebServiceProdIDDismantleDocking.inputProdId(strProdIDorSN, "", Pcode, editor, station, customer, onProdIdSucceed, onProdIdFailed);
    }
    else {
        ShowInfo("");
        ShowMessage(msgWrongCode + "<br>" + msgNoInput);
        ShowInfo(msgWrongCode + "\r\n" + msgNoInput);
        inputControl.value = "";
        getCommonInputObject().focus();
        getAvailableData("processDataEntry");
    }
}
//---------------------------------------
//ITC-1360-0125 --Clear Data Entry
//---------------------------------------


        
 //reset page
function ResetPage() {
    strProdIDorSN = "";
    inputControl.value = "";
    ShowInfo("");         
    getCommonInputObject().focus();
    getAvailableData("processDataEntry");
}
window.onbeforeunload = function() {
    //ExitPage();
    ResetPage();
} 
// exit page
 function ExitPage()
 {
  
 }
 
 function btnDismantleClick() 
 {
    
    ShowInfo("");
    var ischecktype = false;
    //alert("CLICK");
    if ((strProdIDorSN.length ==0)||(strProdIDorSN ==""))  //((sn == "") ||(!((sn.length == 9) || (sn.length == 10))))
    {

        processDataEntry( inputControl.value);
        return; 
        ShowMessage(msgNoInput);
        ShowInfo(msgNoInput);
        getCommonInputObject().focus();
        getAvailableData("processDataEntry");
    }
    else 
    {
    
        //Comfirm Dismantle  Dismantle Type Select: Product	KP	IMEI AST

        if(confirm(msgConfirmDismantle))
        {
            beginWaitingCoverDiv();
            WebServiceDismantleForDocking.Dismantle(strProdIDorSN, "", "", "", "", Pcode, editor, station, customer, onDismantleReturn);
        }
        else 
        {
            //DEBUG ITC-1414-0146  Kaisheng 2012/06/14
            //=======================================
            endWaitingCoverDiv();
            inputControl.value = "";
            strProdIDorSN = "";
            //========================================
            getCommonInputObject().focus();
            getAvailableData("processDataEntry");
        }
   }
 }
 
 function onDismantleReturn(result)
 {
    V_endWaitingCoverDiv();
    
     if(result!=null)
    {
        if(result=="")
        {
//           var msg='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "msgSuccess") %>'
            //           ShowInfo(inputControl.value  + " " +msg);

            ShowSuccessfulInfo(true, "Product：" + strProdIDorSN + " " + msgdismantle + msgSuccess);
        }
        else
        {
             ShowMessage(result);       
             ShowInfo(result);
        }
         
         inputControl.value = "";
         strProdIDorSN = "";
         getCommonInputObject().focus();
         getAvailableData("processDataEntry"); 
    }
    else
    {
        //alert(msgSystemError);
        //DEBUG ITC-1360-0401
        ShowMessage(msgSystemError);
        ShowInfo(msgSystemError);
        getCommonInputObject().focus();
        getAvailableData("processDataEntry");
    }
}

</script>
</asp:Content>


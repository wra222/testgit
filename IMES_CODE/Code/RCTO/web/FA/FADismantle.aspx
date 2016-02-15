<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Dismantle page
 * UI:CI-MES12-SPEC-FA-UI Dismantle.docx --2011/10/20 
 * UC:CI-MES12-SPEC-FA-UC Dismantle.docx --2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-4  Zhang.Kai-sheng        Create
 * TODO:
 * Known issues:
 */
 --%>

 
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FADismantle.aspx.cs" Inherits="FA_Dismantle" Title="Untitled Page" %>

  
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

<asp:ScriptManager runat="server" ID="SM"  EnablePartialRendering="true">
    <Services>
        <asp:ServiceReference Path="Service/WebServiceFADismantle.asmx" />
    </Services>
</asp:ScriptManager>

<div> 
<table  width="98%" >
    <colgroup>
        <col style="width: 20%;"/>
        <col style="width: 30%;"/>
        <col style="width: 35%;"/>
        <col style="width: 15%;"/>
    </colgroup>
    <tr>
        <td nowrap="noWrap">
            <asp:Label runat="server" ID="lblDismantletype" Text =""  CssClass="iMes_label_13pt"></asp:Label>
        </td>
        <td align="right">
            <iMES:CmbDismantleType ID="DropDismantletype" runat="server" Width="100" IsPercentage="true" />
        </td >
        <td nowrap="noWrap" align="center">       
             <asp:CheckBox runat="server" ID="batchdismantle" AutoPostBack="False" 
                            CssClass="iMes_label_11pt" BackColor="Transparent" BorderStyle="None" OnClick="checkonclick()">
              </asp:CheckBox>
        </td >
        <td align="right">
            <button  id ="btnDismantle"  style ="width:110px; height:24px;" onclick="btnDismantleClick()" >
                <%=Resources.iMESGlobalDisplay.ResourceManager.GetObject(languagePre + "btnDismantle").ToString() %>
            </button> 
        </td>
    </tr>
     </table>
  
    <table id="spanKP" width="98%"> 
    <colgroup>
        <col style="width: 20%;"/>
        <col style="width: 30%;"/>
        <col style="width: 20%;"/>
        <col style="width: 30%;"/>
    </colgroup>
    <tr>
        <td nowrap="noWrap">
            <asp:Label runat="server" ID="lblKeyparts" Text =""  CssClass="iMes_label_13pt"  AutoPostBack="true"></asp:Label>
        </td>
        <td >
            <iMES:CmbDismantleType ID="CmbKeyparts" runat="server" Width="100" IsPercentage="true"  />
       </td>
         <td nowrap="noWrap">
            <asp:Label runat="server" ID="lblReturnStation" Text =""  CssClass="iMes_label_13pt"  AutoPostBack="true"></asp:Label>
        </td>
        <td>
              <iMES:CmbDismantleType ID="CmbReturnstation" runat="server" Width="100" IsPercentage="true"  />
       </td>  
           
     </tr>
 </table>

  <table  id="uploadspan" border="0" width="98%">
    <tr>
        
        <td style="width:100%" align="left">
            
          <iframe name="action1" id="action1" src="UploadFile.aspx"  scrolling="no"  frameborder="0" width="100%" height="50px" ></iframe> 
    
        </td>
    </tr>
 </table>

 <hr />
 <table width="100%">
    <colgroup>
        <col style="width: 100px;"/>
        <col />
        <col style="width: 100px;"/>
        <col />
    </colgroup>
    <tr>
        <td>
            <asp:Label runat="server" ID="lblPdline" Text =""  CssClass="iMes_label_13pt"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPdlineContext" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
        </td>
         <td >
            <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
        </td>
        <td >
            <asp:Label ID="lblModelContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
        </td>                    
        
    </tr>
    <tr>
       
        <td >
            <asp:Label ID="lblProdId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
        </td>
        <td >
            <asp:Label ID="lblProdIdContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
        </td>
        <td >
            <asp:Label ID="lblCustSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
        </td>
        <td >
            <asp:Label ID="lblCustSNContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
        </td>                    
    </tr>  

</table>
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
var msgConfirmBatchDismantle = '<%=this.GetLocalResourceObject(languagePre + "msgConfirmBatchDismantle").ToString()%>';
var msgNoInput = '<%=this.GetLocalResourceObject(languagePre + "msgNoInput").ToString()%>';
var msgNoAvailable = '<%=this.GetLocalResourceObject(languagePre + "msgNoAvailable").ToString()%>';
var errDismantletype = '<%=this.GetLocalResourceObject(languagePre + "errDismantletype").ToString()%>';
var mesNoInputFile = '<%=this.GetLocalResourceObject(languagePre + "mesNoInputFile").ToString()%>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "msgSuccess") %>';
var strProdIDorSN="";

document.body.onload = function() {
    try {
        editor = '<%=UserId%>';
        customer = '<%=Customer%>';
        station = '<%=Station%>';
        Pcode = '<%=pCode%>';
        //alert(editor);
        document.getElementById("<%=this.CmbKeyparts.ClientID%>_drpDismantelType").disabled = true;
        document.getElementById("<%=this.CmbReturnstation.ClientID%>_drpDismantelType").disabled = true;
        //uploadspan.style.display = "none";

        inputControl.focus();
        getAvailableData("processDataEntry");
        CheckUploadEnabledStatus();
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
    document.getElementById("<%=lblPdlineContext.ClientID%>").innerText = "";
    document.getElementById("<%=lblModelContent.ClientID%>").innerText = "";
    document.getElementById("<%=lblProdIdContent.ClientID%>").innerText = "";
    document.getElementById("<%=lblCustSNContent.ClientID%>").innerText = "";
    if (document.getElementById("<%=this.DropDismantletype.ClientID%>_drpDismantelType").value != "KP") {
        document.getElementById("<%=this.CmbKeyparts.ClientID%>_drpDismantelType").disabled = true;
        document.getElementById("<%=this.CmbReturnstation.ClientID%>_drpDismantelType").disabled = true;
    }
    CheckUploadEnabledStatus();

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
        if ((inputData.trim().length == 10) && (inputData.trim().substr(0, 3) == "CNU")) {
            strProdIDorSN = inputData.trim().toUpperCase();
        }
        else {
            strProdIDorSN = SubStringSN(inputData.trim().toUpperCase(), "ProdId");
        }

        WebServiceFADismantle.inputProdId(strProdIDorSN, "", Pcode, editor, station, customer, onProdIdSucceed, onProdIdFailed);
    }
    else {
        ShowMessage(msgNoInput);
        ShowInfo(msgNoInput);
        inputControl.value = "";
        getCommonInputObject().focus();
        getAvailableData("processDataEntry");
    }
}
//---------------------------------------
//ITC-1360-0125 --Clear Data Entry
//---------------------------------------

function onProdIdSucceed(result)
{
    try 
    {
        V_endWaitingCoverDiv();
        ShowInfo("");
        if (result[0] != SUCCESSRET)
        {
            strProdIDorSN = "";
            //document.getElementById("<%=lblPdlineContext.ClientID%>").innerText = "";
            //document.getElementById("<%=lblModelContent.ClientID%>").innerText = "";
            //document.getElementById("<%=lblProdIdContent.ClientID%>").innerText = "";
            //document.getElementById("<%=lblCustSNContent.ClientID%>").innerText = "";
            
            ShowMessage(result[0]);
            ShowInfo(result[0]);
        }
        else
        {
            document.getElementById("<%=lblPdlineContext.ClientID%>").innerText = result[1];
            document.getElementById("<%=lblModelContent.ClientID%>").innerText = result[2];
            document.getElementById("<%=lblProdIdContent.ClientID%>").innerText = result[3];
            document.getElementById("<%=lblCustSNContent.ClientID%>").innerText = result[4];
            strProdIDorSN = result[3].trim();
        }
        inputControl.value = "";
        getCommonInputObject().focus();
        getAvailableData("processDataEntry");
        
     }
     catch (e) {
         alert(e.description);
     }  
}
function onProdIdFailed(result)
{
    try 
    {
        strProdIDorSN = "";
        //document.getElementById("<%=lblPdlineContext.ClientID%>").innerText = "";
        //document.getElementById("<%=lblModelContent.ClientID%>").innerText = "";
        //document.getElementById("<%=lblProdIdContent.ClientID%>").innerText = "";
        //document.getElementById("<%=lblCustSNContent.ClientID%>").innerText = "";
            
        inputControl.value="";
        V_endWaitingCoverDiv();
        ShowInfo("");
        //ShowMessage(result[0].get_message());
        //ShowInfo(result[0].get_message());
        getCommonInputObject().focus();
        getAvailableData("processDataEntry");
        
     }
     catch (e) 
    {
        alert(e.description);
    }
}

        
 //reset page
function ResetPage() {
    strProdIDorSN = "";
    inputControl.value = "";
    document.getElementById("<%=lblPdlineContext.ClientID%>").innerText = "";
    document.getElementById("<%=lblModelContent.ClientID%>").innerText = "";
    document.getElementById("<%=lblProdIdContent.ClientID%>").innerText = "";
    document.getElementById("<%=lblCustSNContent.ClientID%>").innerText = "";
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
 function checkdismantletype(dismantletype) {
     var checkitem = dismantletype.trim().toUpperCase();
     if ((checkitem == "PRODUCT") || (checkitem == "KP") || (checkitem == "IMEI") || (checkitem == "AST")) {
         return true;
     }
     else {
         return false;
     }
 }
 function checkonclick() {
     //alert(document.getElementById("<%=this.batchdismantle.ClientID%>").checked);
     //DEBUG ITC-1360-0389  
     try
     {
        var ifr = document.getElementById("action1");
        var objupload = ifr.contentWindow.document.getElementById("FileUpload");
        if (document.getElementById("<%=this.batchdismantle.ClientID%>").checked) {
            objupload.disabled = false;
        }
        else {
            objupload.disabled = true;
        }
     }
     catch(e)
     {
        
     }

 }
 function CheckUploadEnabledStatus()
 {
     try
     {
        var ifr = document.getElementById("action1");
        var objupload = ifr.contentWindow.document.getElementById("FileUpload");
        if (document.getElementById("<%=this.batchdismantle.ClientID%>").checked) {
            objupload.disabled = false;
        }
        else {
            objupload.disabled = true;
        }
     }
     catch(e)
     {
        
     }
 }
 
 function btnDismantleClick() 
 {
     ShowInfo("");
     var ischecktype = false;
     //alert("CLICK");
     var dismantletype = document.getElementById("<%=this.DropDismantletype.ClientID%>_drpDismantelType").value;
     var keypartsvalue = document.getElementById("<%=this.CmbKeyparts.ClientID%>_drpDismantelType").value;
     var returnstationvalue = document.getElementById("<%=this.CmbReturnstation.ClientID%>_drpDismantelType").value;

     var bselBatch = document.getElementById("<%=this.batchdismantle.ClientID%>").checked;
     ischecktype = checkdismantletype(dismantletype);

     if (ischecktype == false) {
         //errDismantletype
         ShowMessage(errDismantletype);
         ShowInfo(errDismantletype);
         getCommonInputObject().focus();
         getAvailableData("processDataEntry");
         return;
     }
     
     if (bselBatch)
     {
         var ifr = document.getElementById("action1");
         var objupload = ifr.contentWindow.document.getElementById("Customer");
         objupload.value = '<%=Customer%>';
         objupload = ifr.contentWindow.document.getElementById("Editor");
         objupload.value = '<%=UserId%>';
         objupload = ifr.contentWindow.document.getElementById("Station");
         objupload.value = '<%=Station%>';
         objupload = ifr.contentWindow.document.getElementById("pCode");
         objupload.value = '<%=pCode%>';
         objupload = ifr.contentWindow.document.getElementById("lDismantletype");
         objupload.value = dismantletype;
         objupload = ifr.contentWindow.document.getElementById("lKeyparts");
         objupload.value = keypartsvalue;
         objupload = ifr.contentWindow.document.getElementById("lReturnStation");
         objupload.value = returnstationvalue;
         //alert(txt.value);
         objupload = ifr.contentWindow.document.getElementById("FileUpload");
         //alert(objupload.value);
         if (objupload.value == "") {
             inputControl.value = "";
             ShowMessage(mesNoInputFile);
             ShowInfo(mesNoInputFile);
             getCommonInputObject().focus();
             getAvailableData("processDataEntry");
             return;
         }
         objupload = ifr.contentWindow.document.getElementById("btnUpload");
         if (confirm(msgConfirmBatchDismantle)) {
             objupload.click();
         }
         //window.frames[0].document.getElementById("btnUpload").click();
         //inputControl.value = "";
         //ShowInfo(msgNoAvailable);
         //getCommonInputObject().focus();
         //getAvailableData("processDataEntry");
         return;
     }
     //ShowInfo("");
    
    //inputControl.value=inputControl.value.toUpperCase();
    //var sn=inputControl.value.trim();
    if ((strProdIDorSN.length ==0)||(strProdIDorSN ==""))  //((sn == "") ||(!((sn.length == 9) || (sn.length == 10))))
    {
       //ShowMessage(msgNoInput);
        inputControl.value = "";
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
            WebServiceFADismantle.Dismantle(strProdIDorSN, dismantletype, keypartsvalue, returnstationvalue, "", Pcode, editor, station, customer, onDismantleReturn);

        }
        else 
        {
            getCommonInputObject().focus();
            getAvailableData("processDataEntry");
        }
   }
 }
 
 function onDismantleReturn(result)
 {
     var strprodid = document.getElementById("<%=lblProdIdContent.ClientID%>").innerText;
     
    V_endWaitingCoverDiv();
    //document.getElementById("<%=lblPdlineContext.ClientID%>").innerText = "";
    //document.getElementById("<%=lblModelContent.ClientID%>").innerText = "";
    //document.getElementById("<%=lblProdIdContent.ClientID%>").innerText = "";
    //document.getElementById("<%=lblCustSNContent.ClientID%>").innerText = "";
    strProdIDorSN = "";
    //document.getElementById("<%=this.CmbKeyparts.ClientID%>_drpDismantelType").refresh();
    //document.getElementById("<%=this.CmbReturnstation.ClientID%>_drpDismantelType").refresh();
     if(result!=null)
    {
        if(result=="")
        {
//           var msg='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "msgSuccess") %>'
            //           ShowInfo(inputControl.value  + " " +msg);

            ShowSuccessfulInfo(true, "[" + strprodid + "] " + msgSuccess);
        }
        else
        {
             ShowMessage(result);       
             ShowInfo(result);
         }
         
         inputControl.value="";
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
function setCmbKPdisabled() 
{
    uploadspan.style.display = "none";
   
}

function setCmbKPenabled() 
{
    uploadspan.style.display = "";
   

}

function setbachfileenbabled() 
{
    document.getElementById("<%=this.batchdismantle.ClientID%>").disabled = false;
    
}
function setbachfiledisabled() 
{
    document.getElementById("<%=this.batchdismantle.ClientID%>").disabled = true;
}

</script>
</asp:Content>


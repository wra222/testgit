<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PackingPizzaUnpack page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-21  zhu lei               Create          
 * Known issues:
 * TODO:
 * 编码完成，但数据库尚无相关可供测试数据。尚未调试，需整合阶段再行调试
 */
 --%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PackingPizzaUnpack.aspx.cs" Inherits="PAK_PackingPizzaUnpack" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
         <Services>
             <asp:ServiceReference Path= "~/PAK/Service/PackingPizzaWebService.asmx" />
        </Services>
    </asp:ScriptManager>
<div>
   <center >
        <table width="95%" style="vertical-align:middle; height:20%" cellpadding="0" cellspacing="0" >
                  
        <tr>
            <td style="width:18%; height: 11%;" align="left"  >
                <asp:Label ID="lblKitID" runat="server"  CssClass="iMes_label_13pt"   ></asp:Label>
            </td>
            <td style="width:82%; height: 11%;" align="left"  >
               <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                         <iMES:Input ID="Input1" runat="server"  CanUseKeyboard="true" IsPaste="true" ProcessQuickInput="true" IsClear="false"
                         MaxLength="30" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]" 
                         Width="98.2%"  TabIndex="0" />
                    </ContentTemplate>   
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="height: 95px">
                <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>    
            </td>
            <td colspan="2" style="height: 95px">
                <textarea id="txtReason" rows="5" style="width:98%;" 
                runat="server" maxlength="80" onkeypress="return imposeMaxLength(this)" 
                onblur="ismaxlength(this)" onkeydown="Tab(this)" tabindex="1"></textarea>
            </td>
        </tr>
        <tr>
        <td align ="center" colspan="2">
        <button  id ="btnUnpack"  style ="width:110px; height:24px;" onclick="btnUnpackClick()" >
         <%=Resources.iMESGlobalDisplay.ResourceManager.GetObject(Pre + "_btnUnpack").ToString()%>
        </button> 
        </td>  
        </tr>
        <tr>
            <asp:UpdatePanel ID="UpdatePanelAll" runat="server"  RenderMode="Inline">
            <ContentTemplate>
                 <input type="hidden" runat="server" id="station" /> 
                 <input type="hidden" runat="server" id="pCode" /> 
            </ContentTemplate>   
            </asp:UpdatePanel> 
        </tr>
       </table>
  </center>
</div>
   
<script type="text/javascript">

var editor;
var customer;
var station;
var kitID;
var emptyPattern = /^\s*$/;
var msgReasonNull = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReasonNull").ToString() %>';
var msgKitIDNull = '<%=this.GetLocalResourceObject(Pre + "_msgKitIDNull").ToString() %>';
var SUCCESSRET ="<%=WebConstant.SUCCESSRET%>";
var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var inputControl= getCommonInputObject();
var msgConfirmUnpack = '<%=this.GetLocalResourceObject(Pre + "_msgConfirmUnpack").ToString()%>';

window.onload = function() 
{
    ShowInfo("");
    editor = "<%=UserId%>";
    customer = "<%=Customer%>";
    station = '<%=Request["Station"] %>';
    //置快速控件的焦点
    getCommonInputObject().focus();
    //bug:ITC-1268-0061
    //支持回车tab键
    getAvailableData("processDataEntry");
 
}

function processDataEntry(inputData) 
{
    btnUnpackClick();
}  

function btnUnpackClick()
{
    ShowInfo("");
    kitID = inputControl.value.trim();
    if (kitID != "") 
    {
        if(confirm(msgConfirmUnpack))
        {
            var strReason = document.getElementById("<%=txtReason.ClientID %>").value.trim();

            if (emptyPattern.test(strReason)) {
                alert(msgReasonNull);
                getAvailableData("processDataEntry");
                document.getElementById("<%=txtReason.ClientID %>").focus();
                return;
            } 
            beginWaitingCoverDiv();
            PackingPizzaWebService.UnpackPizza(kitID, editor, station, customer, strReason, onSuccess, onFail);   
        } else {
            inputControl.focus();
            getAvailableData("processDataEntry");  
            
        }
        
    } else {

        alert(msgKitIDNull);
        inputControl.focus();
        getAvailableData("processDataEntry");
    }
        
   
}
 
function onSuccess(result)
{
    ShowInfo("");
    endWaitingCoverDiv();
    try 
    {
        if(result==null)
        {
            ShowMessage(msgSystemError);
            ShowInfo(msgSystemError);
        }

        else if (result==SUCCESSRET)
        {
              ShowSuccessfulInfo(true);
        }
        
        else 
        {
            var content =result;
            ShowMessage(content);
            ShowInfo(content);
        } 
      
    } 
    catch(e)
    {
        alert(e.description);
    }
    onClearAll();
    getAvailableData("processDataEntry");
     
}
    
function onFail(error)
{
    ShowInfo("");
    endWaitingCoverDiv();
    try
    {
       ShowMessage(error.get_message());
       ShowInfo(error.get_message());
    }
    catch(e) 
    {
    alert(e.description);
    }
    onClearAll();
    getAvailableData("processDataEntry");

}

function imposeMaxLength(obj) {
    var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
    return (obj.value.length < mlength);
}

function ismaxlength(obj) {
    var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
    if (obj.getAttribute && obj.value.length > mlength) {
        alert(msgInputMaxLength1 + mlength + msgInputMaxLength2);
        obj.value = obj.value.substring(0, mlength);
        reasonFocus();
    }
}


function Tab(reasonPara) {
    if (event.keyCode == 9) {
        getCommonInputObject().focus();
        event.returnValue = false;
    }
}

function reasonFocus() {
    document.getElementById("<%=txtReason.ClientID %>").focus();
} 
function onClearAll()
{
    getCommonInputObject().value = "";
    document.getElementById("<%=txtReason.ClientID %>").value = "";
    getCommonInputObject().focus();

}

window.onbeforeunload= function() 
{
    ExitPage();
} 

function ExitPage()
{
//    if (ProductIDOrSNOrCartonNo!="")
//    {
//        CombinePOInCartonUnpackService.Cancel(ProductIDOrSNOrCartonNo);
//        sleep(waitTimeForClear);
//    }
//    uutInput=true;
} 


function ResetPage()
{
    ExitPage();
    ShowInfo("");
    onClearAll(); 
    
}


 </script>
    



</asp:Content>



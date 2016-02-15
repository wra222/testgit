<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="KittingInput.aspx.cs" Inherits="FA_KittingInput" Title="Untitled Page"%>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceKittingInput.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
    <tr>
	    <td style="width:10%" align="right">&nbsp;</td>
	    <td align="center" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
            <asp:Label ID="lbInputAmt" runat="server" CssClass="iMes_label_13pt" 
                ForeColor="Red"></asp:Label><asp:Label ID="txtInputAmt" runat="server" CssClass="iMes_label_13pt"/></td>
    </tr>
    <tr>
	    <td style="width:10%" align="left" ><asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td style="width: 66%"><iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true"/></td>
	   
    </tr>
    <tr>
	    <td style="width:10%" align="left"><asp:Label ID="lbProdId" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td align="left" style="width: 66%"><asp:Label ID="txtProdId" runat="server" CssClass="iMes_label_13pt"/></td>
    </tr>
	<tr>
	    <td style="width:10%" align="left"><asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td align="left" style="width: 66%"><asp:Label ID="txtModel" runat="server" CssClass="iMes_label_13pt"/></td>
    </tr>
    <tr>
	    <td style="width:10%" align="left"><asp:Label ID="lbBoxId" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td align="left" style="width: 66%"><asp:Label ID="txtBoxId" runat="server" CssClass="iMes_label_13pt"/></td>	   
    </tr>
	<tr>
	    <td style="width:10%" align="left"><asp:Label ID="lbDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label></td>
	    <td align="left" style="width: 66%" ><iMES:Input ID="txt" runat="server"   ProcessQuickInput="true" IsClear="true" Width="99%"
             CanUseKeyboard="true" IsPaste="true"  MaxLength="50"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
             <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <input type="hidden" runat="server" id="station" />
                </ContentTemplate>   
            </asp:UpdatePanel> 
	   </td>

    </tr>
   
    </table>
    </center>
</div>


<script type="text/javascript">
var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
var mesBoxIdLengthError = '<%=this.GetLocalResourceObject(Pre + "_mesBoxIdLengthError").ToString()%>';
var mesBoxIdMustBeDigit = '<%=this.GetLocalResourceObject(Pre + "_mesBoxIdMustBeDigit").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';

var PdLine = "";
var station = "";
var EverInputSucced = "FALSE";
var InputAmt = 0;
var __current_proId = "";

document.body.onload = function() {
    try {
        getAvailableData("processDataEntry");
    }
    catch (e) {
        alert(e.description);
    }
};

function is_all_digit(inputData) 
{
    var ret = true;

    for (var i = 0; i < inputData.length; i++) 
    {
        if ((inputData.substring(i, i+1) >= '0') && (inputData.substring(i, i+1) <= '9')) 
        {
            continue;
        }
        else 
        {
            ret = false;
            break;
        }
    }
    
    return ret;
}

function processDataEntry(inputData)
{
    try {
        station = document.getElementById("<%=station.ClientID%>").value;

        __current_proId = document.getElementById("<%=txtProdId.ClientID%>").innerText;
        if (__current_proId == "") 
        {
            var this_prodId = SubStringSN(inputData, "ProdId");
            var this_cus = "<%=customer%>";
            var this_uid = "<%=userId%>";
            PdLine = getPdLineCmbValue();
            beginWaitingCoverDiv();
            WebServiceKittingInput.inputProdId(PdLine, this_prodId, this_uid, station, this_cus, onProdIdSucceed, onProdIdFail); //getPdLineCmbValue()
        }
        else 
        {
            var _all_digit = is_all_digit(inputData);
            if ((inputData.length == 4) && (_all_digit == true)) 
            {
                beginWaitingCoverDiv();
                WebServiceKittingInput.inputBoxId(__current_proId, inputData, onBoxIdSucceed, onBoxIdFail);
            }
            else if (_all_digit == true) 
            {
                alert(mesBoxIdLengthError);
                getAvailableData("processDataEntry");
            }
            else 
            {
                alert(mesBoxIdMustBeDigit);
                getAvailableData("processDataEntry");
            }
        }
    }
    catch (e) 
    {
        alert(e.description);
    }
}

function onCloseLocationSucceed(result) 
{
    try 
    {
        endWaitingCoverDiv();
        if (result == null) 
        {
            ShowInfo(msgSystemError);
        }
        else 
        {
            ShowInfo(result[0]);
        }
        callNextInput();
    } 
    catch (e) 
    {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

function onCloseLocationFail(result) 
{
    try 
    {
        endWaitingCoverDiv();
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        callNextInput();
    } 
    catch (e) 
    {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

function onProdIdSucceed(result)
{
    try 
    {
        EverInputSucced = "TRUE";
        endWaitingCoverDiv();
               
        if(result==null)
        {
            ShowInfo("");
            ShowMessage(msgSystemError);
            ShowInfo(msgSystemError);
            callNextInput();
        }
        else if (result.length == 2)
        {
            document.getElementById("<%=txtProdId.ClientID%>").innerText = result[0];
            __current_proId = result[0];
            document.getElementById("<%=txtModel.ClientID%>").innerText = result[1];
            ShowInfo("");
            callNextInput();
        }
        else 
        {
            ShowMessage(result[0]);
            ShowInfo(result[0]);
            callNextInput();
        }
    }
    catch (e) 
    {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

function onProdIdFail(error)
{
    try 
    {
        endWaitingCoverDiv();
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        callNextInput();
    }
    catch (e) 
    {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

function onBoxIdSucceed(result) 
{
    try {
        endWaitingCoverDiv();
        if (result == null) 
        {
            reset();
            ShowMessage(msgSystemError);
            ShowInfo(msgSystemError);
            callNextInput();
        }
        else if ((result.length == 2) && (result[0] == SUCCESSRET)) 
        {
            InputAmt = InputAmt + 1;
            document.getElementById("<%=txtInputAmt.ClientID%>").innerText = InputAmt.toString();
            document.getElementById("<%=txtBoxId.ClientID%>").innerText = result[1];
            var _proID = document.getElementById("<%=txtProdId.ClientID%>").innerText;
            reset();
            ShowSuccessfulInfo(true, "[" + "productID:" + _proID + "] " + msgSuccess);
            callNextInput();
            //-- ShowSuccessfulInfo(true);
        }
        else 
        {
            reset();
            ShowMessage(result[0]);
            ShowInfo(result[0]);
            callNextInput();
        }
    }
    catch (e)
    {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

function onBoxIdFail(error) 
{
    try 
    {
        endWaitingCoverDiv();
        WebServiceKittingInput.Cancel(__current_proId);
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        reset();
        callNextInput();
    }
    catch (e) 
    {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

function reset()
{
    try 
    {
        document.getElementById("<%=txtProdId.ClientID%>").innerText = "";__current_proId = "";
        document.getElementById("<%=txtModel.ClientID%>").innerText = "";
        document.getElementById("<%=txtBoxId.ClientID%>").innerText = "";
    }
    catch (e) 
    {
        alert(e.description);
    }
}

function callNextInput()
{
    getCommonInputObject().focus();
    getAvailableData("processDataEntry");
}

window.onbeforeunload = function() {
    ExitPage();
};

function ExitPage()
{
    if (__current_proId != "")
    {
        WebServiceKittingInput.Cancel(__current_proId);
        sleep(waitTimeForClear);   
    } 
}

function ResetPage()
{
    ExitPage();
    reset();
    getCommonInputObject().value = "";
}

</script>
</asp:Content>



<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Assign WH Location for BT
* UI:CI-MES12-SPEC-PAK-UI Assign WHLication for BT.docx –2011/11/21 
* UC:CI-MES12-SPEC-PAK-UC Assign WHLication for BT.docx –2011/11/21            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011/11/21    liuqingbiao           Create   
* Known issues:
* TODO：
* 
*/
 --%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AssignWHLocation.aspx.cs" Inherits="PAK_AssignWHLocation" Title="Untitled Page"%>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceAssignWHLocation.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
        <tr>
            <td style="width:12%" align="left">
	            <asp:Label ID="lbFloor" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	        </td>
	        <td colspan="3">
                <select name="CmbFloor" id = "CmbFloor" style="Width:99%">
                    <option value="1">&nbsp</option>
                </select> 
            </td>
        </tr>
    </table>
    <fieldset style="width: 95%">
        <legend align="left" style="height: 20px">
            <asp:Label ID="lblProductInfo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
        </legend>    
        <table border="0" width="95%">
        <tr>
	        <td style="width:15%" align="left"><asp:Label ID="lbCustomerSn" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	        <td align="left" style="width: 335px"><asp:Label ID="txtCustomerSn" runat="server" CssClass="iMes_label_13pt"/></td>
	    
	        <td style="width:15%" align="left"><asp:Label ID="lbProdId" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	        <td align="left" style="width: 349px"><asp:Label ID="txtProdId" runat="server" CssClass="iMes_label_13pt"/></td>
        </tr>
	    <tr>
	        <td style="width:15%" align="left"><asp:Label ID="lbModel" runat="server" 
                    CssClass="iMes_label_13pt"></asp:Label>
            </td>
	        <td align="left" style="width: 335px">
	            <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_13pt"/>
	        </td>
        </tr>
    </table>
    </fieldset>   
    
    <fieldset style="width: 95%">
        <legend align="left" style="height: 20px">
            <asp:Label ID="lblWHLocation_Info" runat="server" CssClass="iMes_label_13pt"></asp:Label>
        </legend>    
        <table border="0" width="95%">     
            <tr>
	            <td style="width:15%" align="left">
	                <asp:Label ID="lbLocation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	            </td>
	            <td align="left" style="width: 335px">
	                <asp:Label ID="txtLocation" runat="server" CssClass="iMes_label_13pt"/>
	            </td>
	            <td style="width:15%" align="left">
	                <asp:Label ID="lbQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	            </td>
	            <td align="left" style="width: 349px">
	                <asp:Label ID="txtQty" runat="server" CssClass="iMes_label_13pt"/>
	            </td>	   
            </tr>
        </table>
    </fieldset>	
    <br/>
    <br/>
    <br/>
    <br/>
    <br/>
    <table border="0" width="95%">
	    <tr>
	        <td style="height:40%" align="left" >
	        </td>
	    </tr>
	    <tr>
	        <td style="width:12%" align="left">
	            <asp:Label ID="lbDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
	        </td>
	        <td align="left" style="width: 80%" >
	            <iMES:Input ID="txt" runat="server"   ProcessQuickInput="true" IsClear="true" Width="80%"
                    CanUseKeyboard="true" IsPaste="true"  MaxLength="50"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
                    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <input type="hidden" runat="server" id="pdLine"  />
                            <input type="hidden" runat="server" id="station" />
                        </ContentTemplate>   
                    </asp:UpdatePanel> 
	        </td>
        </tr>
    </table>
    </center>
</div>


<script type="text/javascript">
var mesNoSelFloor = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectFloor").ToString()%>';
var msgCloseLocationSuccess = '<%=this.GetLocalResourceObject(Pre + "_msgCloseLocationSuccess").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgLocation_empty = '<%=this.GetLocalResourceObject(Pre + "_msgLocation_empty").ToString()%>';
var msgCustSn_empty = '<%=this.GetLocalResourceObject(Pre + "_msgCustSn_empty").ToString()%>';
var msgPutLocation_1 = '<%=this.GetLocalResourceObject(Pre + "_msgPutLocation_1").ToString()%>';
var msgPutLocation_2 = '<%=this.GetLocalResourceObject(Pre + "_msgPutLocation_2").ToString()%>';

var pdLine = "";
var station = "";
var outLocation ="";
var outQty = "";
var _succ_model = "init_label"
var __rem_prodID = "";
var __custSn_remembered = "";


var LOCATION_CLEAR = 1;
var LOCATION_KEEP = -1;

var FLOOR_CLEAR = 1;
var FLOOR_KEEP = -1;

var FOCUS_IN_FLOOR = 1;
var FOCUS_IN_DATA_ENTRY = -1;

var OTHER_DATA_CLEAR = 1;
var OTHER_DATA_KEEP = -1;

var IF_NECCESSARY_DO_CANCEL = 1;
var DO_NOT_DO_CANCEL = -1;

document.body.onload = function() 
{
    try 
    {
        document.getElementById("CmbFloor").selectedIndex = 0;
        document.getElementById("CmbFloor").focus();
        getAvailableData("processDataEntry");
		
		var f = document.getElementById('CmbFloor');
		var allowFloors = '<%=Request["Floor"] %>'.split(",");
		for (var i=0; i<allowFloors.length; i++){
			f.options.add(new Option(allowFloors[i], allowFloors[i]));
		}
    }
    catch (e) 
    {
        alert(e.description);
    }
};

function processDataEntry(inputData)
{
    try 
    {
        var errorFlag = false;
        station = document.getElementById("<%=station.ClientID%>").value;
        pdLine = document.getElementById("<%=pdLine.ClientID%>").value;

        var floor = document.getElementById("CmbFloor")[document.getElementById("CmbFloor").selectedIndex].text;
        // if input data, and the data is not "7777", "9999", do the following.
        if ((inputData != "7777") && (inputData != "9999")) 
        {
            if (floor == "") 
            {
                alert(mesNoSelFloor);
                errorFlag = true;
                document.getElementById("CmbFloor").focus(); 
            } 
        }

        if (!errorFlag) 
        {
            if ((inputData == "7777") || (inputData == "9999")) 
            {
                if (inputData == "7777") 
                {
                    ShowInfo("");
                    // do not do_cancel, interface clear_all, focus in floor.
                    ResetPage(DO_NOT_DO_CANCEL, LOCATION_CLEAR, FLOOR_CLEAR, FOCUS_IN_FLOOR, OTHER_DATA_CLEAR);
                    getAvailableData("processDataEntry");
                }
                else if (inputData == "9999") 
                {
                    var customerSn = document.getElementById("<%=txtCustomerSn.ClientID %>").innerText;
                    var editor = "<%=userId%>";
                    var customerId = "<%=customer%>";
                    var location = document.getElementById("<%=txtLocation.ClientID %>").innerText;
                    if ((location == "") || (customerSn == "")) 
                    {
                        if (location == "") 
                        {
                            alert(msgLocation_empty);
                            ShowInfo(msgLocation_empty);
                        }
                        else 
                        {
                            alert(msgCustSn_empty);
                            ShowInfo(msgCustSn_empty);
                        }
                        callNextInput();
                    }
                    else 
                    {
                        beginWaitingCoverDiv();
                        WebServiceAssignWHLocation.closeLocation(floor, pdLine, customerSn, editor, station, customerId, location, onCloseLocationSucceed, onCloseLocationFail);
                    }
                }
            }
            else if ((inputData.toString().length != 9) && (inputData.toString().length != 10)) 
            {
                ResetPage(DO_NOT_DO_CANCEL, LOCATION_KEEP, FLOOR_KEEP, FOCUS_IN_DATA_ENTRY, OTHER_DATA_KEEP);
                ShowInfo("input data length error! data=" + inputData);
                callNextInput();
            }
            else 
            {
                var customerSn = inputData.toString().substring(0, 10);
                var customerId = "<%=customer%>";
                var editor = "<%=userId%>";
                __custSn_remembered = customerSn;
                var __pre_model = document.getElementById("<%=txtModel.ClientID %>").innerText;
                beginWaitingCoverDiv();
                WebServiceAssignWHLocation.inputProdId(floor, pdLine, customerSn, editor, station, customerId, __pre_model, onProdIdSucceed, onProdIdFail);
            }
        } 
        else 
        {
            getAvailableData("processDataEntry");
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
            callNextInput();
        }
        else 
        {
            _succ_model = "init_label";
            ResetPage(DO_NOT_DO_CANCEL, LOCATION_CLEAR, FLOOR_KEEP, FOCUS_IN_DATA_ENTRY, OTHER_DATA_CLEAR);
            ShowInfo(msgCloseLocationSuccess);
            callNextInput();
        }
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
        ResetPage(IF_NECCESSARY_DO_CANCEL, LOCATION_KEEP, FLOOR_KEEP, FOCUS_IN_DATA_ENTRY, OTHER_DATA_KEEP);
        ShowMessage(result.get_message());
        ShowInfo(result.get_message());
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
        endWaitingCoverDiv();
        if(result==null)
        {
            ShowMessage(msgSystemError);
            ShowInfo(msgSystemError);
            callNextInput();
        }
        else if (result.length == 5)
        {
            document.getElementById("<%=txtCustomerSn.ClientID%>").innerText = result[0];
            document.getElementById("<%=txtProdId.ClientID%>").innerText = result[1];
            __rem_prodID = result[1];
            document.getElementById("<%=txtModel.ClientID%>").innerText = result[2];
            document.getElementById("<%=txtLocation.ClientID%>").innerText = result[3];
            document.getElementById("<%=txtQty.ClientID%>").innerText = result[4];

            if (result[2] != "") 
            {
                if (_succ_model == "init_label") 
                {
                    _succ_model = result[2];
                }

                ShowInfo(msgPutLocation_1 + result[3] + msgPutLocation_2)
                callNextInput();
            }
            else 
            {
                ShowInfo("");
                callNextInput();
            }
        }
        else 
        {
            ShowInfo("");
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
        // here we do not clear interface. and focus in Data_Entry, floor keep.
        ResetPage(DO_NOT_DO_CANCEL, LOCATION_KEEP, FLOOR_KEEP, FOCUS_IN_DATA_ENTRY, OTHER_DATA_KEEP);
        // only give error messages.
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

function reset(Location_clear, floor_clear, other_data_clear)
{
    try 
    {
        if (other_data_clear == OTHER_DATA_CLEAR) 
        {
            document.getElementById("<%=txtCustomerSn.ClientID%>").innerText = "";
            document.getElementById("<%=txtProdId.ClientID%>").innerText = ""; __rem_prodID = "";
            document.getElementById("<%=txtModel.ClientID%>").innerText = "";
            document.getElementById("<%=txtQty.ClientID%>").innerText = "";
        }
        if (Location_clear == LOCATION_CLEAR) 
        {
            document.getElementById("<%=txtLocation.ClientID%>").innerText = "";
        }
        if (floor_clear == FLOOR_CLEAR) 
        {
            document.getElementById("CmbFloor").selectedIndex = 0;
        }
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
    ExitPage(IF_NECCESSARY_DO_CANCEL);
};

function ExitPage(if_neccessary_do_cancel) 
{
    if (if_neccessary_do_cancel == IF_NECCESSARY_DO_CANCEL) 
    {
        if (__rem_prodID != "") 
        {
            WebServiceAssignWHLocation.Cancel(__rem_prodID);
            __rem_prodID = "";
            sleep(waitTimeForClear);
        }
    } 
}

function ResetPage(if_neccessary_do_cancel, Location_clear, floor_clear, focus_in_floor, other_data_clear)
{
    ExitPage(if_neccessary_do_cancel);
    reset(Location_clear, floor_clear, other_data_clear);
    if (focus_in_floor == FOCUS_IN_FLOOR) 
    {
        document.getElementById("CmbFloor").focus();
    }
    else 
    {
        getCommonInputObject().value = "";
    }
}
</script>
</asp:Content>



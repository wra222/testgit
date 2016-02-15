
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PilotRunMO_Add.aspx.cs" Inherits="FA_PilotRunMO_Add" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServicePilotRunMO.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
        <table border="0" width="95%">
            <tr>
                <td style="width:10%">
                    <asp:Label ID="lblStage" runat="server" Text="Stage:"></asp:Label>
                </td>
                <td style="width:20%">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="cmbStage" runat="server" Width="98%" AutoPostBack="true"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="width:12%"></td>
                <td style="width:15%"></td>
                <td style="width:15%"></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMOType" runat="server" Text="Start Process:"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="cmbMOType" runat="server" Width="98%" ></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lblEndMoType" runat="server" Text="End Process:"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="cmbEndMoType" runat="server" Width="98%" ></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFamily" runat="server" Text="Family:"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="cmbFamily" runat="server" Width="98%"  AutoPostBack="true"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                </td>
                <td>
                    <asp:Label ID="lblModel" runat="server" Text="Model:"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="cmbModel" runat="server" Width="98%" ></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel6" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtModel" runat="server" Width="90%"></asp:TextBox>    
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblQty" runat="server" Text="Qty:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtQty" runat="server" Width="94%" MaxLength="10" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblPlanStartTime" runat="server" Text="Plan Start Time:"></asp:Label>
                </td>
                <td>
                    <input type="text" id="dCalShipdate" style="width:90px;" readonly="readonly" />&nbsp;
                    <input id="btnCalfrom" type="button" value=".." onclick="showCalendar('dCalShipdate')" style="width: 17px" class="iMes_button"  />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPartNo" runat="server" Text="PartNo:"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtPartNo" runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVendor" runat="server" Text="Vendor:"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtVendor" runat="server" Width="98%" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCauesDescr" runat="server" Text="CauesDescr:"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtCauesDescr" runat="server" Width="98%" MaxLength="64"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRemark" runat="server" Text="Remark:"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtRemark" runat="server" Width="98%" MaxLength="64"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel7" UpdateMode="Conditional">
                        <ContentTemplate>
                            <input id="btnAdd" type="button"  runat="server" style="width:100px" onclick="if(Add())" onserverclick="btnAdd_ServerClick" value="Add"
	                        onmouseover="this.className='iMes_button_onmouseover'" 
	                        onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <input id="btnCloes" type="button"  runat="server" style="width:100px" onclick="Close()" value="Close"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/>
                </td>
            </tr>
        </table>
    </center>
       
</div>
<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
    <ContentTemplate>          
    </ContentTemplate>   
</asp:UpdatePanel> 

<asp:HiddenField ID="hidPlanTime" runat="server" />
<asp:HiddenField ID="stationHF" runat="server" />
<input type="hidden" runat="server" id="pCode" /> 
<script type="text/javascript">

    var mesNoSelReason = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelReason").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var mesReasonOutRange = '<%=this.GetLocalResourceObject(Pre + "_mesReasonOutRange").ToString()%>';
    var mesNoProdId = '<%=this.GetLocalResourceObject(Pre + "_mesNoProdId").ToString()%>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';


    var SUCCESSRET = "SUCCESSRET";
    var lstPrintItem;
    var inpuKBCT;
    var parentParams;
    var model;
    var editor;
    var customer;
    var station;
    var toDate = document.getElementById('dCalShipdate');
    document.body.onload = function() {
        parentParams = window.dialogArguments;
        model = parentParams[0];
        editor = parentParams[1];
        customer = parentParams[2];
        station = parentParams[3];
        ShowInfo("");
        toDate.value = "<%=today%>";
    }
    
    function Add() {
        try {

            var stage = document.getElementById("<%=cmbStage.ClientID %>").value;
            if (stage == "") {
                alert("Please Select Stage");
                document.getElementById("<%=cmbStage.ClientID %>").focus();
                return false;
            }
            var motype = document.getElementById("<%=cmbMOType.ClientID %>").value;
            
            if (motype == "") {
                alert("Please Select MO Type");
                document.getElementById("<%=cmbMOType.ClientID %>").focus();
                return false;
            }
            var endmotype = document.getElementById("<%=cmbEndMoType.ClientID %>").value;
            if (endmotype == "") {
                alert("Please Select End MO Type");
                document.getElementById("<%=cmbEndMoType.ClientID %>").focus();
                return false;
            }
            var family = document.getElementById("<%=cmbFamily.ClientID %>").value;
            if (family == "") {
                alert("Please Select Family");
                document.getElementById("<%=cmbFamily.ClientID %>").focus();
                return false;
            }
            var model1 = document.getElementById("<%=cmbModel.ClientID %>").value;
            var model2 = document.getElementById("<%=txtModel.ClientID %>").value;
            if (stage == "SA") {
                if (model1 == "") {
                    alert("Please Select Model");
                    document.getElementById("<%=cmbModel.ClientID %>").focus();
                    return false;
                }
            }
            else if (stage == "FA") {
                if (model2 == "") {
                    alert("Please Input Model");
                    document.getElementById("<%=txtModel.ClientID %>").focus();
                    return false;
                }
            }
            var qty = document.getElementById("<%=txtQty.ClientID %>").value;
            if (qty == "") {
                alert("Please Input Qty");
                document.getElementById("<%=txtQty.ClientID %>").focus();
                return false;
            }
            var partNo = document.getElementById("<%=txtPartNo.ClientID %>").value;
            if (partNo == "") {
                alert("Please Input Part No");
                document.getElementById("<%=txtPartNo.ClientID %>").focus();
                return false;
            }
            var vendor = document.getElementById("<%=txtVendor.ClientID %>").value;
            if (partNo == "") {
                alert("Please Input Vendor");
                document.getElementById("<%=txtVendor.ClientID %>").focus();
                return false;
            }
            var cauesdescr = document.getElementById("<%=txtCauesDescr.ClientID %>").value;
            if (partNo == "") {
                alert("Please Input Caues Descr");
                document.getElementById("<%=txtCauesDescr.ClientID %>").focus();
                return false;
            }
            document.getElementById("<%=hidPlanTime.ClientID %>").value = document.getElementById('dCalShipdate').value;
            return true;
        } catch (e) {
            alert(e.description);
        }
    }

    function ExitPage() { 
        
    }

    function Close() {
        window.close();
    }

    function ResetPage() {
        //ExitPage();
        document.getElementById("<%=cmbStage.ClientID %>").value = "";
        document.getElementById("<%=cmbMOType.ClientID %>").value = "";
        document.getElementById("<%=cmbEndMoType.ClientID %>").value = "";
        document.getElementById("<%=cmbFamily.ClientID %>").value = "";
        document.getElementById("<%=cmbModel.ClientID %>").value = "";
        document.getElementById("<%=txtModel.ClientID %>").value = "";
        document.getElementById("<%=txtQty.ClientID %>").value = "";
        document.getElementById("<%=txtPartNo.ClientID %>").value = "";
        document.getElementById("<%=txtVendor.ClientID %>").value = "";
        document.getElementById("<%=txtCauesDescr.ClientID %>").value = "";
        document.getElementById("<%=txtRemark.ClientID %>").value = "";
        
        ShowInfo("");
        endWaitingCoverDiv();
    }

</script>
</asp:Content>


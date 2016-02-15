<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SQEDefectReportMaintain.aspx.cs"
    Inherits="DataMaintain_SQEDefectReportMaitain" MasterPageFile="~/MasterPageMaintain.master" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ Register Assembly="myControls" Namespace="myControls" TagPrefix="iMES" %>
<%@ Register Src="../CommonControl/DataMaintain/CmbMaitainSQEReport.ascx" TagName="CmbMaitainSQEReport"
    TagPrefix="iMESMaintain" %>
<asp:Content ID="contentHolder" runat="server" ContentPlaceHolderID="iMESContent">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js">
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <table width="100%" class="iMes_div_MainTainEdit">
            <tr>
                <td style="width: 124px">
                    <asp:Label CssClass="iMes_label_13pt" ID="lblCTNO" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="dCTNO" runat="server" SkinID="textBoxSkin" onkeyPress="OnKeyPress(this);"></asp:TextBox>
                </td>
               
                <td style="width: 124px">
                    <asp:Label CssClass="iMes_label_13pt" ID="lblIECPN" runat="server"></asp:Label>
                </td>
                <td style="width: 150px">
                    <iMESMaintain:CmbIECPN ID="cmbiecpn" runat="server" Width="98" IsPercentage="true"></iMESMaintain:CmbIECPN>
                </td>
                
            </tr>
        </table>
        <div id="div1" class="iMes_div_MainTainDiv1">
            <asp:UpdatePanel ID="updatePanel1" RenderMode="Inline" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <table width="100%" class="iMes_div_MainTainEdit">
                        <tr>
                            <td>
                                <asp:Label CssClass="iMes_label_13pt" ID="lblModelPartNo" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="dModelPartNo" runat="server" SkinID="textBoxSkin" ReadOnly="true" style="background-color:RGB(192,192,192)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RadioButton ID="ridoMpToSQE" runat="server" GroupName="TP" onclick="radioButtonSetValue();"
                                    value="MP->SQE" />
                                <asp:Label CssClass="iMes_label_13pt" ID="lblMpToSQE" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButton ID="ridoPE" runat="server" GroupName="TP" Checked="true" onclick="radioButtonSetValue();"
                                    value="PE" />
                                <asp:Label CssClass="iMes_label_13pt" ID="lblPE" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButton ID="ridoSQE" runat="server" GroupName="TP" onclick="radioButtonSetValue();"
                                    value="SQE" />
                                <asp:Label CssClass="iMes_label_13pt" ID="lblSQE" runat="server"></asp:Label>
                            </td>
                            <td align="right">
                                <input type="button" id="btnSave" runat="server" onserverclick="btnSave_ServerClick"
                                    class="iMes_button" onclick="if(clickSave())" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="iMes_label_13pt" ID="lblDefect" runat="server"></asp:Label>
                            </td>
                            <td colspan="5">
                                <iMESMaintain:SQEReportDefectCode runat="server" ID="drpDefect" Width="100%"></iMESMaintain:SQEReportDefectCode>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="iMes_label_13pt" ID="lblCause" runat="server"></asp:Label>
                            </td>
                            <td colspan="5">
                                <iMESMaintain:CmbMaitainSQEReport runat="server" ID="drpCause" Width="100%"></iMESMaintain:CmbMaitainSQEReport>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="iMes_label_13pt" ID="lblMajorPart" runat="server"></asp:Label>
                            </td>
                            <td colspan="5">
                                <iMESMaintain:CmbMaitainSQEReport runat="server" ID="drpMajorPart" Width="100%">
                                </iMESMaintain:CmbMaitainSQEReport>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="iMes_label_13pt" ID="lblCommponent" runat="server"></asp:Label>
                            </td>
                            <td colspan="5">
                                <iMESMaintain:CmbMaitainSQEReport runat="server" ID="drpCommponent" Width="100%">
                                </iMESMaintain:CmbMaitainSQEReport>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="iMes_label_13pt" ID="lblObligation" runat="server"></asp:Label>
                            </td>
                            <td colspan="5">
                                <iMESMaintain:CmbMaitainSQEReport runat="server" ID="drpObligation" Width="100%">
                                </iMESMaintain:CmbMaitainSQEReport>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="iMes_label_13pt" ID="lblRemark" runat="server"></asp:Label>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="dRemark" runat="server" SkinID="textBoxSkin" MaxLength="500" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="iMes_label_13pt" ID="lblResult" runat="server" SkinID="textBoxSkin"></asp:Label>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="dResult" runat="server" SkinID="textBoxSkin" MaxLength="500" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <table id="" class="iMes_div_MainTainListLable">
            <tr>
                <td>
                    <asp:Label ID="lblDefectList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
            </tr>
        </table>
        <div id="div2">
            <asp:UpdatePanel ID="updatePanel2" RenderMode="Inline" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%"
                        RowStyle-Height="20" GvExtWidth="100%" GvExtHeight="100%" AutoHighlightScrollByValue="true"
                        HighLightRowPosition="3" Height="350px" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                        EnableViewState="false" OnRowDataBound="gd_RowDataBound">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanelAll" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnShowDetailCtno" EventName="ServerClick" />
        </Triggers>
    </asp:UpdatePanel>
    <input type="hidden" id="dTableHeight" runat="server" />
    <input type="hidden" id="hiddenUserName" runat="server" />
    <input type="hidden" id="radioButtonGroup" runat="server" />
    <button style="display: none" id="btnShowDetailCtno" runat="server" onserverclick="btnShowDetailCtno_ServerClick">
    </button>
    <div id="divWait" oselectid="" align="center" style="cursor: wait; filter: Chroma(Color=skyblue);
        background-color: skyblue; display: none; top: 0; width: 100%; height: 100%;
        z-index: 10000; position: absolute">
        <table style="cursor: wait; background-color: #FFFFFF; border: 1px solid #0054B9;
            font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr>
                <td align="center">
                    <img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif" />
                </td>
                <td align="center" style="color: #0054B9; font-size: 10pt; font-weight: bold;">
                    Please wait.....
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        var msg1;
        var msg2;
        var msg3;
        var msg4;
        var msg5;
        var msg7;
        var msg8;
        var msg9;
        var msg10;
        var msg11;
        var msg12;
        window.onload = function() {
            msg1 = "<%=pmtMessage1 %>";
            msg2 = "<%=pmtMessage2 %>";
            msg3 = "<%=pmtMessage3 %>";
            msg4 = "<%=pmtMessage4 %>";
            msg5 = "<%=pmtMessage5 %>";
            msg7 = "<%=pmtMessage7 %>";
            msg8 = "<%=pmtMessage8 %>";
            msg9 = "<%=pmtMessage9 %>";
            msg10 = "<%=pmtMessage10 %>";
            msg11 = "<%=pmtMessage11 %>";
            msg12 = "<%=pmtMessage12 %>";
            document.getElementById("<%=radioButtonGroup.ClientID %>").value = document.getElementById("<%=ridoPE.ClientID %>").value;
        }

        function showListCTNO() {
            document.getElementById("<%=btnShowDetailCtno.ClientID %>").click();
        }

        function OnKeyPress(obj) {
            var key = event.keyCode;
            if (key == 13)//enter
            {
                var ctno = document.getElementById("<%=dCTNO.ClientID %>").value;
                if (ctno != "" && ctno.length == 14) {
                    showListCTNO();
                }
                else {
                    alert(msg1);
                    return false;

                }
            }

        }

        function radioButtonSetValue() {
            var input = document.getElementsByTagName("input");
            for (var i = 0; i < input.length; i++) {
                if (input[i].type == "radio") {
                    if (input[i].checked == true) {
                        document.getElementById("<%=radioButtonGroup.ClientID %>").value = input[i].value;
                    }
                }
            }
        }

        //设置表格的高度  
        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 110;
            var marginValue = 5;
            var tableHeigth = 240;
            //var tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
            try {
                tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
            }
            catch (e) {

                //ignore
            }
            //为了使表格下面有写空隙
            var extDivHeight = tableHeigth + marginValue;
            div2.style.height = extDivHeight + "px";
            document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
        }

        function clickSave() {
            ShowWait();
            if (checkNull() == false) {
                DealHideWait();
                return false;
            }
            return true;
        }

        var iSelectedRowIndex = null;
        function checkNull() {
            var ctno = document.getElementById("<%=dCTNO.ClientID %>").value;
            if (ctno == "") {
                alert(msg7);
                return false;
            }

            var defect = getSQAReportDefectCodeCmbObj().value;
            if (defect == "") {
                alert(msg9);
                return false;
            }

            var majorPart = document.getElementById("<%=drpMajorPart.ClientID %>" + "_SQAReportUserControl").value;
            if (majorPart == "") {
                alert(msg10);
                return false;
            }

            var component = document.getElementById("<%=drpCommponent.ClientID %>" + "_SQAReportUserControl").value;
            if (component == "") {
                alert(msg12);
                return false;
            }

            var obligation = document.getElementById("<%=drpObligation.ClientID %>" + "_SQAReportUserControl").value;
            if (obligation == "") {
                alert(msg11);
                return false;
            }

            return true;

        }
        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }
            //document.getElementById("<%=dModelPartNo.ClientID %>").value = con.cells[7].innerText.trim();

            var currentId = con.cells[7].innerText.trim();
            if (currentId == "") {
                setNewItemValue();
                return;
            }
            else {

            }
            familyChange();
        }


        function DealHideWait() {
            HideWait();
        }
        function isDigit() {
            return ((event.keyCode >= 48) && (event.keyCode <= 57));
        }
    
    
    </script>

</asp:Content>

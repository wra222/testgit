<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModelBOM_PartInfo.aspx.cs"
    Inherits="DataMaintain_ModelBOM_PartInfo" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<html>
<head runat="server">
<title>Part Attribute</title>

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <style type="text/css">
        body
        {
            resizable: no;
            overflow-x: hidden;
            overflow-y: hidden;
        }
    </style>
    <link href="CSS/dataMaintain.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

</head>
<body>
    <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="iMes_div_MainTainContainer_" style="width: 900px; height: 570px;">
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%">
                <tr>
                    <td width="33%">
                        <asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        <asp:Label ID="lblPartNoValue" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="lblNodeType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        <asp:Label ID="lblNodeTypeValue" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="33%" align="right">
                        <input type="button" runat="server" id="btnClose" class="iMes_button" onclick="clickButton()" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblAttributeList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="div2">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%"
                        GvExtWidth="100%" GvExtHeight="656px" AutoHighlightScrollByValue="true" HighLightRowPosition="3"
                        OnRowDataBound="gd_RowDataBound"
                        EnableViewState="false" Style="top: 0px; left: 0px">
                    </iMES:GridViewExt>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false">
        </asp:UpdatePanel>
        <input id="HiddenUsername" type="hidden" name="HiddenUsername" runat="server" />
        <input type="hidden" id="hidPartNo" runat="server" />
        <input type="hidden" id="hidInfoValue" runat="server" />
            <input type="hidden" id="hidInfoType" runat="server" />
        <input type="hidden" id="hidContent" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="hidMultiItem" runat="server" value="VendorCode" />
         <input type="hidden" id="hidCdt" runat="server" />
         <input type="hidden" id="hidUdt" runat="server" />
           <input type="hidden" id="hidID" runat="server" />
    </div>
    
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
        window.onload = function() {
            msg1 = "<%=pmtMessage1%>";
            msg2 = "<%=pmtMessage2%>";
            msg3 = "<%=pmtMessage3%>";
            msg4 = "<%=pmtMessage4%>";
            msg5 = "<%=pmtMessage5%>";
            msg6 = "<%=pmtMessage6%>";
            msg7 = "<%=pmtMessage7%>";
        };
        function clickButton() {
            switch (event.srcElement.id) {
                case "<%=btnClose.ClientID %>":

                    if (clickClose() == false) {
                        return false;
                    }
                    break;
            }
            ShowWait();
            return true;
        }

        function AddUpdateComplete(id) {
            DealHideWait();
            var gdObj = document.getElementById("<%=gd.ClientID %>");

            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[0].innerText == id) {
                    selectedRowIndex = i;
                    break;
                }
            }

            if (selectedRowIndex < 0) {
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            }

        }

        var iSelectedRowIndex = null;
        function clickClose() {
            window.returnValue = null;
            window.close();
            return true;
        }

        function clickTable(con) {
            setGdHighLight(con);
        }

        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }

        function DealHideWait() {
            HideWait();
        }
        
    </script>

    </form>
</body>
</html>

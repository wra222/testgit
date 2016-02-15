<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetMailList.aspx.cs"
    Inherits="Query_PAK_SetMailList"  %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<html>
<head runat="server">
<title>Set Mail List</title>

 <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
<script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
 <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>    

    <style type="text/css">
        body
        {
            resizable: no;
            overflow-x: hidden;
            overflow-y: hidden;
        }
    </style>
   

</head>
<body>
    <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="iMes_div_MainTainContainer_" style="width: 500px; height: 570px;">
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
                        <input type="button" runat="server" id="btnClose" class="iMes_button" onclick="clickButton()" value="Close" />
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
                        GvExtWidth="100%" GvExtHeight="400px" AutoHighlightScrollByValue="true" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' OnRowDataBound="gd_RowDataBound"
                        EnableViewState="false" Style="top: 0px; left: 0px">
                    </iMES:GridViewExt>
                </div>
                <input type="hidden" id="hidMailList" runat="server" />
                       <input type="hidden" id="hidSelectMail" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div3">
            <table width="100%"  class="iMes_div_MainTainEdit">
                <tr style="height:30px">
                    <td width="10%">
                        <asp:Label ID="lblItemName" runat="server" Text="Mail:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="90%">
                      
                  <asp:TextBox ID="txtInfoValue" runat="server"  SkinID="textBoxSkin"
                            Style='ime-mode: disabled;' Width="100%"></asp:TextBox>
                    </td>
                   
                </tr>
                <tr>
                <td  align="right" colspan="2">
                  <input type="button" id="btnDelete" runat="server" onclick="if(clkDelete())"  onserverclick="btnDelete_ServerClick"
                            class="iMes_button" value="Delete" />
&nbsp;<input type="button" runat="server" id="btnAdd" onclick="if(clkAdd())" class="iMes_button"
                            onserverclick="btnAdd_ServerClick" value="Add" />
                        <input type="button" runat="server" id="btnSave" onclick="clkSave();" class="iMes_button"
                            onserverclick="btnSave_ServerClick" value="Save"/>
                </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                     <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                     <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            </Triggers>
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
        
            setNewItemValue();
        
        };
        function clickButton() {
            switch (event.srcElement.id) {

                case "<%=btnSave.ClientID %>":

                    if (clkSave() == false) {
                        return false;
                    }
                    break;

                case "<%=btnClose.ClientID %>":

                    if (clickClose() == false) {
                        return false;
                    }
                    break;
            }
          //  ShowWait();
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
                ShowRowEditInfo(null);
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                ShowRowEditInfo(con);
            }

        }

        var iSelectedRowIndex = null;
        function clickClose() {
            window.returnValue = null;
            window.close();
            return true;
        }
        function clkAdd() {

            var check;
            var input=document.getElementById("<%=txtInfoValue.ClientID %>").value.trim();
            if (input=="") {

                alert("Please input mail");
                return false;
            }

            if (CheckHaveInfoType(input)) {

                alert(input + " 已存在Mail List");
                return false;
            }
            
            
            document.getElementById("<%=hidInfoValue.ClientID %>").value = document.getElementById("<%=txtInfoValue.ClientID %>").value;
            return true;

        }
        function clkDelete() {
            if (iSelectedRowIndex == null) {
                alert(msg1);
                return false;
            }
            //var ret = confirm("确定要删除这条记录么？");
            var ret = confirm("Are you suer to delete it?");
            if (!ret) {
                return false;
            }

            return true;

        }
        function clkSave() {
            var itemValue = document.getElementById("<%=txtInfoValue.ClientID %>");
            document.getElementById("<%=hidInfoValue.ClientID %>").value = document.getElementById("<%=txtInfoValue.ClientID %>").value;
            
            if (itemValue.value == "") {
                var ret = confirm("确定要清除这条记录么？");
                if (!ret) {
                    return false;
                }
            }

            return true;

        }

        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);

        }

        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }

        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }

            document.getElementById("<%=txtInfoValue.ClientID %>").value = con.cells[0].innerText.trim();

            document.getElementById("<%=hidSelectMail.ClientID %>").value = con.cells[0].innerText.trim();
          
            var currentId = con.cells[0].innerText.trim();

            if (currentId == "") {
                setNewItemValue();
                return;
            }
            else {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
                if (currentId == document.getElementById("<%=hidMultiItem.ClientID %>").value)
                { document.getElementById("<%=btnAdd.ClientID %>").disabled = false; }
         
            }
        }
        function setNewItemValue() {
            document.getElementById("<%=txtInfoValue.ClientID %>").value = "";
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            
       
            

        }
        function DealHideWait() {
            HideWait();
        }
        function CheckHaveInfoType(value) {
            var gdObj = document.getElementById("<%=gd.ClientID %>");
            var searchValue = value.toUpperCase();
            var check = false;
            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (searchValue.trim() == gdObj.rows[i].cells[0].innerText.toUpperCase()) {
                    check=true;break;
                }
            }
            return check;
        }
        
    </script>

    </form>
</body>
</html>

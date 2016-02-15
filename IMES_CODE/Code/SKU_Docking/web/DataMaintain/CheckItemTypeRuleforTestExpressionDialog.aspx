<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" enableEventValidation="false"
    CodeFile="CheckItemTypeRuleforTestExpressionDialog.aspx.cs" Inherits="CheckItemTypeRuleforTestREDialog" ValidateRequest="false" %>
    

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div style=" height:99%; width: 95%; margin: 0 auto;">
        <fieldset id="FsInput" style="height: 95%; width: 95%;">
            <legend id="LegendInput" style="font-weight: bold; color: Blue">Test Expression</legend>
            <table width="100%" class="iMes_div_MainTainEdit"  >
                <tr>
                    <td style="width:15%">
                        <label id="lblTestObject" >Test Object:</label>
                    </td>
                    <td style="width:15%">
                        <asp:DropDownList ID="cmdTestObject" runat="server" Width="100%" onchange="DD(this)"></asp:DropDownList>
                    </td>
                    <td style="width:20%">
                        <input id="txtTestObject" type="text" maxlength="255" runat="server" style="width:100%" />
                    </td>
                    <td style="width:50%"></td>
                </tr>
                <tr>
                    <td>
                        <label id="lblTestCondition" >Test Condition:</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbTestCondition" runat="server" Width="100%"></asp:DropDownList>
                    </td>
                    <td>
                        <input id="txtTestCondition" type="text" maxlength="255" runat="server" style="width:100%" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <label id="lblExpression" >Expression:</label>
                    </td>
                    <td colspan="3">
                        <input id="txtExpression" type="text" maxlength="255" runat="server" style="width:100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label id="lblResult">Result:</label>
                    </td>
                    <td>
                        <label id="lblResultAnws" runat="server" ></label>  
                    </td>
                    <td></td>
                    <td>
                        <input type="button" id="betTestReg" runat="server" onclick="if(check())" onserverclick="betTest_ServerClick" value="Test" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" UpdateMode="Conditional" RenderMode="Inline" Visible="true">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="betTestReg" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
            <input type="hidden" id="hidObjectType" runat="server" />
            <input type="hidden" id="hidConditionType" runat="server" />
        </fieldset>
        <hr />
    </div>
    <script type="text/javascript">
        function DD(val) {
            var text = val.innerText;
            var value = val.value;
            if (text == "") {
                alert("Pleaase Select Test Object");
                return;
            }
            addItem(value);
        }

        function addItem(val) {
            var ddl = document.getElementById("<%=cmbTestCondition.ClientID%>");

            while (ddl.options.length > 0)
                ddl.remove(0);
            var arr = val.split(",");
            for (var i = 0; i < arr.length; i++) {
                var opt = document.createElement('option');
                opt.text = arr[i];
                opt.value = arr[i];
                ddl.options.add(opt);
            }
        }

        function check() {
            var objectvalue = document.getElementById("<%=cmdTestObject.ClientID %>").options[document.getElementById("<%=cmdTestObject.ClientID %>").selectedIndex].text;
            var objecttxt = document.getElementById("<%=txtTestObject.ClientID%>").value;
            var conditionvalue = document.getElementById("<%=cmbTestCondition.ClientID %>").options[document.getElementById("<%=cmbTestCondition.ClientID %>").selectedIndex].text;
            var conditiontxt = document.getElementById("<%=txtTestCondition.ClientID%>").value;
            var expressiontxt = document.getElementById("<%=txtExpression.ClientID%>").value;
            if (objectvalue.trim() != "") {
                if (objecttxt.trim() == "") {
                    alert("Please Input Test Object");
                    document.getElementById("<%=txtTestObject.ClientID%>").focus();
                    return false;
                }
                document.getElementById("<%=hidObjectType.ClientID%>").value = objectvalue;
            }
            else {
                alert("Please Select Test Object");
                document.getElementById("<%=cmdTestObject.ClientID%>").focus();
                return false;
            }

            if (conditionvalue.trim() != "") {
                if (conditiontxt.trim() == "" && conditionvalue != "TestLog") {
                    alert("Please Input Test Condition");
                    document.getElementById("<%=txtTestCondition.ClientID%>").focus();
                    return false;
                }
                document.getElementById("<%=hidConditionType.ClientID%>").value = conditionvalue;
            }

            if (expressiontxt.trim() == "") {
                alert("Please Input Expression");
                document.getElementById("<%=txtExpression.ClientID%>").focus();
                return false;
            }
            return true;
        }

        function ResultAnws(val) {
            var p = document.getElementById("<%=lblResultAnws.ClientID%>");
            if (val=="Y") {
                p.innerText = "True";
            }
            else {
                p.innerText = "False";
            }
        }
        
    </script>
</asp:Content>

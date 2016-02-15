<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" 
    CodeFile="CheckItemTypeRuleforTestREDialog.aspx.cs" Inherits="CheckItemTypeRuleforTestREDialog" ValidateRequest="false" %>
    

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div style=" height:99%; width: 95%; margin: 0 auto;">
        <fieldset id="FsInput" style="height: 95%; width: 95%;">
            <legend id="LegendInput" style="font-weight: bold; color: Blue">Test RegExp</legend>
            <table width="100%" class="iMes_div_MainTainEdit"  >
                <tr>
                    <td style="width:10%;">
                        <label id="lblInput" >Input:</label>
                    </td>
                    <td style="width:70%;">
                        <input id="txtInput" type="text" maxlength="255" value="abc" runat="server" style="width:98%" />
                        <%--<asp:TextBox ID="txtInput" MaxLength="255" Text="abc" runat="server" Width="98%"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label id="lblRegExp">RegExp:</label>
                    </td>
                    <td>
                        <input id="txtRegExp" type="text" maxlength="255" value="[a-z]" runat="server" style="width:98%" />
                        <%--<asp:TextBox ID="txtRegExp" MaxLength="255" Text="[a-z]" runat="server" Width="98%"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label id="lblResult">Result:</label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate>
                                <label id="lblResultAnws" runat="server" ></label>    
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="betTestReg" EventName="ServerClick" /> 
                            </Triggers>
                        </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right">
                        <button type="button" id="betTestReg" runat="server" onserverclick="betTestReg_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" >Test</button>
                    </td>
                </tr>
            </table>
        </fieldset>
        <hr />
    </div>
    <script type="text/javascript">
function r_test(regex, s) {
    var p = document.getElementById('lblResultAnws');
    var result = document.createTextNode(
        regex.test(s)
            ? 'True!'
            : 'False!'
    );
    p.innerText = result.data;
}
 
function regexp_match() {
    //var p = document.getElementById('regex');

    var inputString = document.getElementById("<%=txtInput.ClientID%>").value;
    var patternString = document.getElementById("<%=txtRegExp.ClientID%>").value;
    
    var flags = '';
    
 
    var regex = new RegExp(patternString, flags);
    //p.replaceChild(document.createTextNode(regex.toString()), p.lastChild);
 
    r_test(regex, inputString);
}
</script>
</asp:Content>

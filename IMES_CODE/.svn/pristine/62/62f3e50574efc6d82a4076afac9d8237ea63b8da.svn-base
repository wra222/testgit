<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:CmbPdLine
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-10 wang shaohua           Create    
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMiantainDesc.ascx.cs"
    Inherits="CommonControl_DataMaintain_CmbMiantainDesc" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:DropDownList ID="drplstTypeDesc" runat="server" AutoPostBack="true" Width="326px" >
        </asp:DropDownList>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">

    function getTypeDescCmbObj() {

        try {
            return document.getElementById("<%=drplstTypeDesc.ClientID %>");

        } catch (e) {
            alert(e.description);
        }

    }

    function getTypeDescCmbText() {

        try {
            return document.all("<%=drplstTypeDesc.ClientID %>").options[document.all("<%=drplstTypeDesc.ClientID %>").selectedIndex].text.trim();

        } catch (e) {
            alert(e.description);
        }

    }
    function getTypeDescCmbValue() {

        try {
            return document.all("<%=drplstTypeDesc.ClientID %>").options[document.all("<%=drplstTypeDesc.ClientID %>").selectedIndex].value.trim();

        } catch (e) {
            alert(e.description);
        }

    }
    function checkTypeDescCmb() {

        try {
            var msgSelectNullCmb = "";
            if (getTypeDescCmbText() == "") {
                ShowMessage(msgSelectNullCmb);
                return false;
            }

            return true;

        } catch (e) {
            alert(e.description);
        }

    }

    function setTypeDescCmbFocus() {

        try {
            getTypeDescCmbObj().focus();

        } catch (e) {
            alert(e.description);
        }

    }              
</script>


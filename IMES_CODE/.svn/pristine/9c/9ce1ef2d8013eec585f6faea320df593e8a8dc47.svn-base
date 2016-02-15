<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CmbPalletType
* UI:CI-MES12-SPEC-PAK-UI-Combine Carton in DN
* UC:CI-MES12-SPEC-PAK-UC-Combine Carton in DN          
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012/09/07    ITC000052             Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbDeliveryListByCarton.ascx.cs" Inherits="CommonControl_CmbDeliveryListByCarton" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpDeliverybyCarton" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine") %>'; ;

    function getDeliveryCmbObj() {
        return document.getElementById("<%=drpDeliverybyCarton.ClientID %>");
    }

    function getDeliveryCmbText() {
        if (document.getElementById("<%=drpDeliverybyCarton.ClientID %>").selectedIndex == -1) {
            return "";
        } else {
        return document.getElementById("<%=drpDeliverybyCarton.ClientID %>")[document.getElementById("<%=drpDeliverybyCarton.ClientID %>").selectedIndex].text;
        }
    }

    function getDeliveryCmbValue() {
        return document.getElementById("<%=drpDeliverybyCarton.ClientID %>").value;
    }

    function checkDeliveryCmb() {
        if (getDeliveryCmbText() == "") {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }

    function setDeliveryFocus() {
        document.getElementById("<%=drpDeliverybyCarton.ClientID %>").focus();
    }    
                
</script>

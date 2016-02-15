<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CmbPalletType
* UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx –2011/12/16 
* UC:CI-MES12-SPEC-PAK-UC Pallet Weight –2011/12/16            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-12-16   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbLabelKittingCode.ascx.cs" Inherits="CommonControl_CmbLabelKittingCode" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpLabelKittingCode" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine") %>';;

    function getLabelKittingCodeCmbObj()
    {
        return document.getElementById("<%=drpLabelKittingCode.ClientID %>");   
    }

    function getLabelKittingCodeCmbText()
    {
        if (document.getElementById("<%=drpLabelKittingCode.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpLabelKittingCode.ClientID %>")[document.getElementById("<%=drpLabelKittingCode.ClientID %>").selectedIndex].text;
        }
    }

    function getLabelKittingCodeCmbValue()
    {
        return document.getElementById("<%=drpLabelKittingCode.ClientID %>").value;  
    }

    function checkLabelKittingCodeCmb()
    {
        if (getLabelKittingCodeCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }

    function setLabelKittingCodeCmbFocus()
    {
        document.getElementById("<%=drpLabelKittingCode.ClientID %>").focus();   
    }    
                
</script>


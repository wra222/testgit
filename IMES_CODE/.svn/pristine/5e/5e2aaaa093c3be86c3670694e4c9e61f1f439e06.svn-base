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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbPalletType.ascx.cs" Inherits="CommonControl_CmbPalletType" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpPalletType" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine") %>';;
        
    function getPalletTypeCmbObj()
    {
        return document.getElementById("<%=drpPalletType.ClientID %>");   
    }

    function getPalletTypeCmbText()
    {
        if (document.getElementById("<%=drpPalletType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpPalletType.ClientID %>")[document.getElementById("<%=drpPalletType.ClientID %>").selectedIndex].text;
        }
    }
    
    function getPalletTypeCmbValue()
    {
        return document.getElementById("<%=drpPalletType.ClientID %>").value;  
    }
    
    function checkPalletTypeCmb()
    {
        if (getPalletTypeCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }
    
    function setPalletTypeCmbFocus()
    {
        document.getElementById("<%=drpPalletType.ClientID %>").focus();   
    }    
                
</script>


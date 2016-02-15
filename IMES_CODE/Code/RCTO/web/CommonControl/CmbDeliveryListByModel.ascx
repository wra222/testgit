<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CmbPalletType
* UI:CI-MES12-SPEC-PAK-UI Pizza Kitting –2011/1/6 
* UC:CI-MES12-SPEC-PAK-UC Pizza Kitting –2011/1/6            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-1-6   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbDeliveryListByModel.ascx.cs" Inherits="CommonControl_CmbDeliveryListByModel" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpDelivery" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine") %>';;

    function getDeliveryCmbObj()
    {
        return document.getElementById("<%=drpDelivery.ClientID %>");   
    }

    function getDeliveryCmbText()
    {
        if (document.getElementById("<%=drpDelivery.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpDelivery.ClientID %>")[document.getElementById("<%=drpDelivery.ClientID %>").selectedIndex].text;
        }
    }

    function getDeliveryCmbValue()
    {
        return document.getElementById("<%=drpDelivery.ClientID %>").value;  
    }

    function checkDeliveryCmb()
    {
        if (getDeliveryCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }

    function setDeliveryFocus()
    {
        document.getElementById("<%=drpDelivery.ClientID %>").focus();   
    }    
                
</script>


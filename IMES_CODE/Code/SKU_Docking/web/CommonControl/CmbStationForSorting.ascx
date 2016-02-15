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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbStationForSorting.ascx.cs" Inherits="CommonControl_CmbStationForSorting" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpStationSorting" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine") %>';;

    function getStationForSortingCmbObj()
    {
        return document.getElementById("<%=drpStationSorting.ClientID %>");   
    }

    function getStationForSortingCmbText()
    {
        if (document.getElementById("<%=drpStationSorting.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpStationSorting.ClientID %>")[document.getElementById("<%=drpStationSorting.ClientID %>").selectedIndex].text;
        }
    }

    function getStationForSortingCmbValue()
    {
        return document.getElementById("<%=drpStationSorting.ClientID %>").value;  
    }

    function checkStationForSortingCmb()
    {
        if (getStationForSortingCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }
        return true;
    }

    function setStationForSortingFocus()
    {
        document.getElementById("<%=drpStationSorting.ClientID %>").focus();   
    }    
                
</script>


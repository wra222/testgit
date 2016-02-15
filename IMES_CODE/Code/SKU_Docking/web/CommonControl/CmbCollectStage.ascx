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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbCollectStage.ascx.cs" Inherits="CommonControl_CmbCollectStage" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpStage" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine") %>';;

    function getStationByTypeCmbObj()
    {
        return document.getElementById("<%=drpStage.ClientID %>");   
    }

    function getStationByTypeCmbText()
    {
        if (document.getElementById("<%=drpStage.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpStage.ClientID %>")[document.getElementById("<%=drpStage.ClientID %>").selectedIndex].text;
        }
    }

    function getStationByTypeCmbValue()
    {
        return document.getElementById("<%=drpStage.ClientID %>").value;  
    }
    
    function checkStationByTypeCmb()
    {
        if (getStationByTypeCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }

    function setStationByTypeFocus()
    {
        document.getElementById("<%=drpStage.ClientID %>").focus();   
    }    
                
</script>


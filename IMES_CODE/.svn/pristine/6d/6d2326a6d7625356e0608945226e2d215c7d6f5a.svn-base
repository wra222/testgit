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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbStationForFAReinput.ascx.cs" Inherits="CommonControl_CmbStationForFAReiput" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpStationFAReinput" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine") %>';;

    function getStationForFAReiputCmbObj()
    {
        return document.getElementById("<%=drpStationFAReinput.ClientID %>");   
    }

    function getStationForFAReiputCmbText()
    {
        if (document.getElementById("<%=drpStationFAReinput.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
        return document.getElementById("<%=drpStationFAReinput.ClientID %>")[document.getElementById("<%=drpStationFAReinput.ClientID %>").selectedIndex].text;
        }
    }

    function getStationForFAReiputCmbValue()
    {
        return document.getElementById("<%=drpStationFAReinput.ClientID %>").value;  
    }

    function checkStationForFAReiputCmb()
    {
        if (getStationForFAReiputCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }
        return true;
    }

    function setStationForFAReiputFocus()
    {
        document.getElementById("<%=drpStationFAReinput.ClientID %>").focus();   
    }    
                
</script>


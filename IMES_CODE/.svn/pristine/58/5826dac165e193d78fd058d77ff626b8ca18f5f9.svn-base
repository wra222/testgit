<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbPdLine
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-05  Tong.Zhi-Yong         Create    
 * 2010-01-14  Yuan XiaoWei          Modify All javascript function
 * 2010-01-15  Yuan XiaoWei          Modify ITC-1103-0100
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbConstValueType.ascx.cs" Inherits="CommonControl_CmbConstValueType" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Always" >
<ContentTemplate>
    <asp:DropDownList ID="drpConstValueType" runat="server" Width="326px" AutoPostBack="true"  >
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine") %>';;

    function getConstValueTypeCmbObj()
    {    
       return document.getElementById("<%=drpConstValueType.ClientID %>");   
    }

    function getConstValueTypeCmbText()
    {
        if (document.getElementById("<%=drpConstValueType.ClientID %>").selectedIndex == -1)
        {
            return "";
        }else{
           return document.getElementById("<%=drpConstValueType.ClientID %>")[document.getElementById("<%=drpConstValueType.ClientID %>").selectedIndex].text;
        }
    }

    function getConstValueTypeCmbValue()
    {
       return document.getElementById("<%=drpConstValueType.ClientID %>").value;  
    }

    function checkConstValueTypeCmb()
    {
        if (getPdLineCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }

    function setConstValueTypeFocus()
    {
        document.getElementById("<%=drpConstValueType.ClientID %>").focus();   
    }    
                
</script>


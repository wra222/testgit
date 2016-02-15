<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CmbConstValueType
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1              
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-8-6     Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbConstValueType.ascx.cs" Inherits="CommonControl_CmbConstValueType" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpConstValueType" runat="server" Width="326px">
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
        } 
        else
        {
            return document.getElementById("<%=drpConstValueType.ClientID %>")[document.getElementById("<%=drpConstValueType.ClientID %>").selectedIndex].text;
        }
    }

    function getConstValueTypeCmbValue()
    {
        return document.getElementById("<%=drpConstValueType.ClientID %>").value;  
    }

    function checkConstValueTypeCmb()
    {
        if (getConstValueTypeCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }

    function setConstValueTypeCmbFocus()
    {
        document.getElementById("<%=drpConstValueType.ClientID %>").focus();   
    }    
                
</script>


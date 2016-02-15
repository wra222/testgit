<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CmbCheckItemTypeList
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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbCheckItemTypeList.ascx.cs" Inherits="CommonControl_CmbCheckItemTypeList" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="drpCheckItemTypeList" runat="server" Width="326px">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" language="javascript">
    var msgSelectNullCmb = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputPdLine") %>';;

    function getCheckItemTypeListCmbObj()
    {
        return document.getElementById("<%=drpCheckItemTypeList.ClientID %>");   
    }

    function getCheckItemTypeListCmbText()
    {
        if (document.getElementById("<%=drpCheckItemTypeList.ClientID %>").selectedIndex == -1)
        {
            return "";
        } 
        else
        {
            return document.getElementById("<%=drpCheckItemTypeList.ClientID %>")[document.getElementById("<%=drpCheckItemTypeList.ClientID %>").selectedIndex].text;
        }
    }

    function getCheckItemTypeListCmbValue()
    {
        return document.getElementById("<%=drpCheckItemTypeList.ClientID %>").value;  
    }

    function checkCheckItemTypeListCmb()
    {
        if (getCheckItemTypeListCmbText() == "")
        {
            alert(msgSelectNullCmb);
            ShowInfo(msgSelectNullCmb);
            return false;
        }

        return true;
    }

    function setCheckItemTypeListCmbFocus()
    {
        document.getElementById("<%=drpCheckItemTypeList.ClientID %>").focus();   
    }    
                
</script>


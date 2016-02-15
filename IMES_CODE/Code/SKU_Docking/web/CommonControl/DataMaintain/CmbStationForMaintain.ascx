<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbStation
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-05  liu xiaoling         Create    
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbStationForMaintain.ascx.cs" Inherits="CommonControl_CmbStationForMaintain" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList_Station" runat="server"  AutoPostBack="true" >
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>


<SCRIPT LANGUAGE="JavaScript">
<!--

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getStationCmbObj
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	获取combobox控件对象句柄
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getStationCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList_Station.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}
 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getStationCmbText
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	获取combobox当前选项的text
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getStationCmbText()
{
    
    try {
       return document.all("<%=DropDownList_Station.ClientID %>").options[document.all("<%=DropDownList_Station.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getStationCmbValue
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	获取combobox当前选项的value
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getStationCmbValue()
{
    
    try {
       return document.all("<%=DropDownList_Station.ClientID %>").options[document.all("<%=DropDownList_Station.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkStationCmb
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	检查combobox当前选项是否不为空
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function checkStationCmb()
{
    
    try {
        var msgSelectNullCmb = "";
        if (getStationCmbText() == "")
        {
            ShowMessage(msgSelectNullCmb);
            return false;
        }
    
        return true;
       
    } catch(e) {
        alert(e.description);
    }
    
}

 function setStationCmbFocus()
    {
        
        try {
            getStationCmbObj().focus();
           
        } catch(e) {
            alert(e.description);
        }
        
    }              

//-->
</SCRIPT>


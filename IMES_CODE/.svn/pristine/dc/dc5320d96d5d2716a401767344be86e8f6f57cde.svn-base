<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbPdLine
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2010-11-05  liu xiao ling         Create    
  * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbAttributeForMaintain.ascx.cs" Inherits="CommonControl_CmbAttributeForMaintain" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList_MyAttribute" runat="server"  AutoPostBack="true" >
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>


<SCRIPT LANGUAGE="JavaScript">
<!--

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getMyAttributeCmbObj
//| Author		:	Liu
//| Create Date	:	10/15/2009
//| Description	:	获取combobox控件对象句柄
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getMyAttributeCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList_MyAttribute.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}
 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getMyAttributeCmbText
//| Author		:	Liu
//| Create Date	:	10/15/2009
//| Description	:	获取combobox当前选项的text
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getMyAttributeCmbText()
{
    
    try {
       return document.all("<%=DropDownList_MyAttribute.ClientID %>").options[document.all("<%=DropDownList_MyAttribute.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getMyAttributeCmbValue
//| Author		:	Liu
//| Create Date	:	10/15/2009
//| Description	:	获取combobox当前选项的value
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getMyAttributeCmbValue()
{
    
    try {
       return document.all("<%=DropDownList_MyAttribute.ClientID %>").options[document.all("<%=DropDownList_MyAttribute.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkMyAttributeCmb
//| Author		:	Liu
//| Create Date	:	10/15/2009
//| Description	:	检查combobox当前选项是否不为空
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function checkMyAttributeCmb()
{
    
    try {
        var msgSelectNullCmb = "";
        if (getMyAttributeCmbText() == "")
        {
            ShowMessage(msgSelectNullCmb);
            return false;
        }
    
        return true;
       
    } catch(e) {
        alert(e.description);
    }
    
}

 function setMyAttributeCmbFocus()
    {
        
        try {
            getMyAttributeCmbObj().focus();
           
        } catch(e) {
            alert(e.description);
        }
        
    }              

//-->
</SCRIPT>


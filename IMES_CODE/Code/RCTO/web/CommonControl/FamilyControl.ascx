<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Family common control
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-15  Lucy Liu(EB2)        Create 
 2010-04-21  Lucy Liu(EB2)       Modify:   ITC-1122-0282
 Known issues:
 --%>
 
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FamilyControl.ascx.cs" Inherits="CommonControl_FamilyControl" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList1" runat="server"  AutoPostBack="true" >
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>


<SCRIPT LANGUAGE="JavaScript">
<!--

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getFamilyCmbObj
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	获取combobox控件对象句柄
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getFamilyCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList1.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}
 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getFamilyCmbText
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	获取combobox当前选项的text
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getFamilyCmbText()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getFamilyCmbValue
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	获取combobox当前选项的value
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getFamilyCmbValue()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkFamilyCmb
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	检查combobox当前选项是否不为空
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function checkFamilyCmb()
{
    
    try {
        var msgSelectNullCmb = "";
        if (getFamilyCmbText() == "")
        {
            ShowMessage(msgSelectNullCmb);
            return false;
        }
    
        return true;
       
    } catch(e) {
        alert(e.description);
    }
    
}

 function setFamilyCmbFocus()
    {
        
        try {
            getFamilyCmbObj().focus();
           
        } catch(e) {
            alert(e.description);
        }
        
    }              

//-->
</SCRIPT>
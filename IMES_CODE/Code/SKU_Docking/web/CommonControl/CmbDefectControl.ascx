<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Defect common control
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-15  207006               Create 
 Known issues:
 --%>
 
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbDefectControl.ascx.cs" Inherits="CommonControl_defectControl" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList1" runat="server" >
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>


<SCRIPT LANGUAGE="JavaScript">
<!--


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getDefectCmbObj
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��ȡcombobox�ؼ�������
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getDefectCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList1.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}
 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getDefectCmbText
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��ȡcombobox��ǰѡ���text
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getDefectCmbText()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getDefectCmbValue
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��ȡcombobox��ǰѡ���value
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getDefectCmbValue()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkDefectCmb
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	���combobox��ǰѡ���Ƿ�Ϊ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function checkDefectCmb()
{
    
    try {
        var msgSelectNullCmb = ""
        if (getDefectCmbText() == "")
        {
            ShowMessage(msgSelectNullCmb);
            return false;
        }
    
        return true;
       
    } catch(e) {
        alert(e.description);
    }
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	setMOCmbFocus
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��combobox�ý���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function setDefectCmbFocus()
{
    
    try {
        getDefectCmbObj().focus();
       
    } catch(e) {
        alert(e.description);
    }   
}

//-->
</SCRIPT>
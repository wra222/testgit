<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: VGA common control
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-15  207006(EB2)          Create 
 Known issues:
 --%>
 
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbVGAControl.ascx.cs" Inherits="CommonControl_VGAControl" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList1" runat="server"  AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>


<SCRIPT LANGUAGE="JavaScript">
<!--

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getVGACmbObj
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��ȡcombobox�ؼ�������
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getVGACmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList1.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}
 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getVGACmbText
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��ȡcombobox��ǰѡ���text
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getVGACmbText()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getVGACmbValue
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��ȡcombobox��ǰѡ���value
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getVGACmbValue()
{
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkVGACmb
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	���combobox��ǰѡ���Ƿ�Ϊ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function checkVGACmb()
{
    
    try {
        var msgSelectNullCmb = "";
        if (getVGACmbText() == "")
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
function setVGACmbFocus()
{
    
    try {
        getVGACmbObj().focus();
       
    } catch(e) {
        alert(e.description);
    }   
}


//-->
</SCRIPT>
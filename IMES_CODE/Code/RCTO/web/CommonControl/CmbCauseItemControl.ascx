<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Cause common control
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2012-07-24  itc000052               Create 
 Known issues:
 --%>
 
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbCauseItemControl.ascx.cs" Inherits="CommonControl_causeItemControl" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList1" runat="server"  AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>


<SCRIPT LANGUAGE="JavaScript">

<!--

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getCauseCmbObj
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��ȡcombobox�ؼ�������
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getCauseItemCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList1.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}
 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getCauseCmbText
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��ȡcombobox��ǰѡ���text
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getCauseItemCmbText()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getCauseCmbValue
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��ȡcombobox��ǰѡ���value
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getCauseItemCmbValue()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkCauseCmb
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	���combobox��ǰѡ���Ƿ�Ϊ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function checkCauseItemCmb()
{
    
    try {
        var msgSelectNullCmb = "";
        if (getCauseItemCmbText() == "")
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
function setCauseItemCmbFocus()
{
    
    try {
        getCauseItemCmbObj().focus();
       
    } catch(e) {
        alert(e.description);
    }   
}

//-->
</SCRIPT>
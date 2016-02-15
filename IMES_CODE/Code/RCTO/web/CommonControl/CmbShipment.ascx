<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description:Shipment common control
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-030-18  Lucy Liu(EB2)        Create 
 Known issues:
 --%>
 
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbShipment.ascx.cs" Inherits="CommonControl_CmbShipment" %>

<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:DropDownList ID="DropDownList1" runat="server"  AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>


<SCRIPT LANGUAGE="JavaScript">
<!--


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getShipmentCmbObj
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��ȡcombobox�ؼ�������
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getShipmentCmbObj()
{
    
    try {
       return document.getElementById("<%=DropDownList1.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}
 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getShipmentCmbText
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��ȡcombobox��ǰѡ���text
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getShipmentCmbText()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].text.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getShipmentCmbValue
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��ȡcombobox��ǰѡ���value
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getShipmentCmbValue()
{
    
    try {
       return document.all("<%=DropDownList1.ClientID %>").options[document.all("<%=DropDownList1.ClientID %>").selectedIndex].value.trim();
       
    } catch(e) {
        alert(e.description);
    }
    
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkShipmentCmb
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	���combobox��ǰѡ���Ƿ�Ϊ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function checkShipmentCmb()
{
    
    try {
        var msgSelectNullCmb = ""
        if (getPartnoCmbText() == "")
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
//| Name		:	setPartnoFocus
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	��combobox�ý���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function setShipmentFocus()
{
    
    try {
        getShipmentCmbObj().focus();
       
    } catch(e) {
        alert(e.description);
    }
    
}
//-->
</SCRIPT>

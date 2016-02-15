<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:CmbPdLine
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-05 liu xiaoling        Create    
 * Known issues:
 */
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CblPCBProcessForMaintain.ascx.cs" Inherits="CommonControl_CblPCBProcessForMaintain" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
                                <asp:CheckBoxList id="cblPCBProcess" 
                                       AutoPostBack="True"
                                       CellPadding="0"
                                       CellSpacing="0"
                                       RepeatColumns="1"
                                       RepeatDirection="Vertical"
                                       RepeatLayout="Flow"
                                       TextAlign="Right"
                                       Width="100px"
                                       Height="260px"
                                       runat="server"
                                       BorderWidth=0
                                       Visible=true>
                                </asp:CheckBoxList>

</ContentTemplate>
</asp:UpdatePanel>


<SCRIPT LANGUAGE="JavaScript">
<!--

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getPCBProcessCmbObj
//| Author		:	Lucy Liu
//| Create Date	:	10/15/2009
//| Description	:	获取combobox控件对象句柄
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getPCBProcessCblObj()
{
    
    try {
       return document.getElementById("<%=cblPCBProcess.ClientID %>");
       
    } catch(e) {
        alert(e.description);
    }
    
}
 


 function setPCBProcessCblFocus()
    {
        
        try {
            getPCBProcessCblObj().focus();
           
        } catch(e) {
            alert(e.description);
        }
        
    }              

//-->
</SCRIPT>


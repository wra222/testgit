 <%--
/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:Model Input
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-03  Chen Xu (EB1-4)      Create      
 * Known issues:
 */
 --%> 
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModelInputControl.ascx.cs" Inherits="CommonControl_ModelInputControl" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <input type="text" id="txtModel" style="ime-mode:disabled;width:300px"  class="iMes_textbox_input_Yellow"
      MaxLength="50" onkeypress="inputNumberAndEnglishChar(this)"  onkeydown="OnKeyDownCheck()"       
      runat="server" width="99%"  />
</ContentTemplate>
</asp:UpdatePanel>



<script type="text/javascript" language="javascript">
var EnterTabJSCode = ""; //Enter or Tag后需要执行的JS代码

function getModelCmbObj()
{
    try {
        return document.getElementById("<%=txtModel.ClientID %>");
    } catch(e) {
        alert(e.description);
    }
    
}
 
 function getModelCmbValue()
{
    
    try {
        return document.getElementById("<%=txtModel.ClientID%>").value.trim().toUpperCase();
       
    } catch(e) {
        alert(e.description);
    }
    
}


function setModelCmbFocus()
{
    
    try {
        getModelCmbObj().focus();
       
    } catch(e) {
        alert(e.description);
    }

}

function OnKeyDownCheck() 
{
    var inputContent = document.getElementById("<%=txtModel.ClientID%>").value;

    if (event.keyCode == 9 || event.keyCode == 13) 
    {
        inputContent = inputContent.toUpperCase();
        document.getElementById("<%=txtModel.ClientID%>").value = inputContent.trim();

        if (EnterTabJSCode != null && EnterTabJSCode.trim() != "") {
            eval(EnterTabJSCode);
        }
        
        event.returnValue = false;
    }
}

</script>

<%@ Page Language="C#" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" 
    CodeFile="ChooseFromList.aspx.cs" Inherits="ChooseFromList" Title="" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
</head>

<body style=" position: relative; width: 100%">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
        </Services>
    </asp:ScriptManager>   

    <div>
        <asp:Label ID="lblTitle" runat="server" CssClass="iMes_label_13pt" Font-Size="12pt"></asp:Label><br />
        <asp:ListBox ID="lstChoose" Style="width: 500px; height: 400px;" size="10" runat="server"
            CssClass="iMes_label_13pt" Font-Size="12pt"></asp:ListBox>
		<br/>
		<input id="btnOk" type="button" class="iMes_button" onclick="javascript:setRet();" 
onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" value="Ok" />
		&nbsp;<input id="btnCancel" type="button" class="iMes_button" onclick="javascript:self.close();" 
onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" value="Cancel" />
    </div>                    
       
    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
        <ContentTemplate>
             
        </ContentTemplate>   
    </asp:UpdatePanel> 
    </form>
    
     
</body>
</html>

<script language="JavaScript">

    document.body.onload = function() {
        
    }
	
	function setRet(){
		var ret='';
		var lst=document.getElementById("<%=lstChoose.ClientID%>");
		for(var i=0;i<lst.options.length;i++){
			if(lst.options[i].selected){
				ret=lst.options[i].value;
				break;
			}
		}
		window.returnValue=ret;
		self.close();
	}
	
    function cancel() {
        window.close();
        return;
    }

</script>  


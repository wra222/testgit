<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Small Parts Upload(Iframe Page)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2012-07-20  itc000052              Create 
 
 Known issues:
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="COARemove_Upload.aspx.cs" Inherits="PAK_COARemove_Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body style="background-color:RGB(210,210,210);">
    
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager2" runat="server" >
        <Services>
            
        </Services>
    </asp:ScriptManager>
    <div>
      <TABLE width="100%" height="100%" border="0" >
        
        <TR>
	        <TD style="width:80%">	       
	           <asp:FileUpload id="FileUpload" name="FileUpload" style="width:99%" runat="server"  ContentEditable="false" />	          
	        </TD>
       
	        <TD align="left" ><input id="btnUpload" type="button" onclick="clientUpload();" onserverclick="uploadClick" class="iMes_button" runat="server" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/></TD>
	           <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Always">
                    <ContentTemplate>
                                      

                    </ContentTemplate>   
                </asp:UpdatePanel> 
        </TR>
        </TABLE>
         
    </div>
    </form>
</body>
</html>

<script type="text/javascript">
function clientUpload() {
    window.parent.beginWaitingCoverDiv();
}
</script>
<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Small Parts Upload(Iframe Page)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-05-05  Lucy Liu(EB1)        Create 
 2010-05-11  Lucy Liu(EB2)        Modify:   ITC-1122-0312
 2010-05-11  Lucy Liu(EB2)        Modify:   ITC-1122-0314
 
 Known issues:
 --%>
 
<%@ Page Language="C#"  AutoEventWireup="true" CodeFile="UploadFile.aspx.cs" Inherits="FA_UploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body style="background-color:RGB(210,210,210);">
    
    <form id="form1" runat="server" autopostback="false">
    <asp:ScriptManager id="ScriptManager2" runat="server" EnablePartialRendering="true" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceFADismantle.asmx" />
        </Services>
    </asp:ScriptManager>
    <div>
      <table width="100%"  border="0" >
        
        <tr>
	        <td nowrap="noWrap" style="width:20%"><asp:Label ID="lblFile" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	        <td style="width:80%">	   
	          <asp:FileUpload id="FileUpload" name="FileUpload" style="width:100%" runat="server"  ContentEditable="false" />	          
	        </td>
       
	        <td align="right" ><div style="display:none"><input id="btnUpload" type="button"  onclick="clientUpload();" onserverclick="uploadClick" class="iMes_button" runat="server" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" style="width: 0px; height: 0px; top: 0px; right: 0px;" /></div></td>
	           <asp:UpdatePanel id="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Always">
                    <ContentTemplate>
                        <input type="hidden" runat="server" id="Editor" />
                        <input type="hidden" runat="server" id="Customer" />
                        <input type="hidden" runat="server" id="pCode" />
                        <input type="hidden" runat="server" id="Station" />
                        <input type="hidden" runat="server" id="lDismantletype" />
                        <input type="hidden" runat="server" id="lKeyparts" />
                        <input type="hidden" runat="server" id="lReturnStation" />
                        

                    </ContentTemplate>   
                </asp:UpdatePanel> 
	        
        </tr>
        
        </table>
         
    </div>
    </form>
</body>
</html>

<script type="text/javascript">
function clientUpload()
{
    window.parent.beginWaitingCoverDiv();
}
</script>
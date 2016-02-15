<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for Const Value Maintain Dlg Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1              
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-8-6     Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/
--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConstValueMaintainDlg.aspx.cs" Inherits="DataMaintain_ConstValueMaintainDlg" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

<head id="Head1" runat="server">
    <title>Add Type</title>
    <base target="_self"></base>
<style type="text/css">

</style>   
</head>
 
<script type="text/javascript">    
    var isLoad = false;
    
    var msg1;
    function page_load()
    {   
        isLoad = true;        
    }
    
    
    function page_Unload()
    {
        //window.returnValue = "OK" ;
    }
    
    function btnCancel_Click() 
    {        
        window.close();         
    }
    
    function OKComplete()
    {
        window.returnValue = "OK" ;
        window.close();   
    }
    
    /*
    function OnKeyPress(obj)
    {
        var key = event.keyCode;
                
        if (key == 13)//enter
        {
            event.cancelBubble = true;
            return false;

        }
        else
        {
           return false;
        }       
    }
    */
    function EnterTextBox(button) {

        if (event.keyCode == 13) {
            if (document.getElementById("<%=dType.ClientID %>").value != "") {
                event.keyCode = 9;
                event.returnValue = false;
                document.getElementById("<%=btnSave.ClientID %>").click();
            }
            else {
                event.keyCode = 0;
                event.returnValue = false;
            }
        }
        
    }  

    function DealHideWait()
    {
        HideWait();
    }

    function clkButton() {
        ShowWait();
        return true;
    }
    
</script>

<body style="background-color: #ECE9D8" onload="page_load()" onunload="page_Unload()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <div style="height:2px; width: 100%;">
    </div>

    <div style="width:100%; text-align :center">
         <table width="94%" border="0" >
            <tr style="height:10px;"> <td></td></tr>
            <tr style="height :20px;width:100%">
                <td style="width: 90px;padding-left: 10px;">
                    <asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td  width="80%">
                    <asp:TextBox ID="dType" runat="server"   MaxLength="20" Width="99%" SkinId="textBoxSkin" onkeypress="EnterTextBox('searchbutton');" style='ime-mode:disabled;' ></asp:TextBox>
                </td>                    
            </tr>           
         </table>   
         
         <table width="85%" border="0" >
            <tr style="height :65px">
              
              <td align="right">            
                  <input type="button" id="btnSave" runat="server"  class="iMes_button" onclick="if(clkButton())" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick" />
              </td>
              <td align="right">            
                  <input type="button" id="btnCancel" runat="server"  class="iMes_button" onclick="return btnCancel_Click()" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
              </td>
            </tr>            
         </table>       
    </div>
   
     <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
     </asp:UpdatePanel>
     
    <input type="hidden" id="HiddenUserName" runat="server" />
    <input type="hidden" id="hidIsSubmitOK" runat="server" />
     
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>

    </form>
</body>
</html>




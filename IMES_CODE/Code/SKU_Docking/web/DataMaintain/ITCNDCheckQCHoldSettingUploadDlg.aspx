﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ITCNDCheckQCHoldSettingUploadDlg.aspx.cs" Inherits="DataMaintain_ITCNDCheckQCHoldSettingUploadDlg" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

<head id="Head1" runat="server">
    <title>ITCND Check QC Hold Setting Upload</title>
    <base target="_self"></base>
<style type="text/css">

</style>   
</head>
 
<script type="text/javascript">    
    var isLoad=false;
       
    var msg6;
    function page_load()
    {   
        isLoad=true;        
    }
    
    function page_Unload()
    {
        if(document.getElementById("<%=hidIsSubmitOK.ClientID %>").value!="")
        {
            window.returnValue = "OK" ;
        }
    }
    
    function btnCancel_Click() 
    {
        if(document.getElementById("<%=hidIsSubmitOK.ClientID %>").value!="")
        {
            window.returnValue = "OK" ;
        }
        window.close();         
    }
    
    /*
    *检查上传文件大小
    */
    function ShowSize(fileName) {  
        var fso;
        var f;
        fso = new ActiveXObject("Scripting.FileSystemObject");
	        //f = fso.GetFile(fileName);  
        if(fso.FileExists(fileName)){
	        f = fso.GetFile(fileName);
	        return parseFloat(f.size / 1024 / 1024);
	        //return f.size;  
        }else{
	        return 0;	
        }
    }
    
    function OKComplete()
    {
        msg6 ="<%=pmtMessage6%>";
        alert(msg6)
        var parentWindow = window.dialogArguments;
        parentWindow.uploadResultDataString=document.getElementById("<%=dUploadTableResultData.ClientID %>").value;
        window.returnValue = "OK" ;
        window.close();   
    }
    
    function OnKeyPress(obj)
    {
        var key = event.keyCode;
                
        if (key==13)//enter
        {
            event.cancelBubble =true;
            return false;

        }
        else
        {
           return false;
        }       
    }
    
    function DealHideWait()
    {
        HideWait();   
        //checkSetEnableCombox();
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
                    
                </td>
                <td  width="80%">
                    
                </td>     
                
            </tr>
            <tr style="height:10px;"> <td></td></tr>

            <tr style="height :35px;width:100%">
                <td style="width: 300px; padding-left: 10px;" colspan ="2">
                <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnOK"  />
                </Triggers>
                <ContentTemplate>                 
                    <asp:FileUpload ID="dFileUpload" runat="server" onkeypress='return OnKeyPress(this)' Width ="360px" />  
                    <input type="hidden" id="dUploadTableResultData" runat="server" /> 
                </ContentTemplate>
                </asp:UpdatePanel>                        
                </td>      
            </tr>
           
         </table>   
         <table width="85%" border="0" >
            <tr style="height :65px">
              
              <td align="right">            
                  <input type="button" id="btnOK" runat="server"  class="iMes_button" onclick="if(clkButton())" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnOK_ServerClick" />
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
     <input type="hidden" id="hidMsg1" runat="server" />
     <input type="hidden" id="hidFileMaxSize" runat="server" />
     <input type="hidden" id="hidIsSubmitOK" runat="server" />
     
        <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
            <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
                <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
            </table>
        </div>

    </form>
</body>
<script type="text/javascript">    
    
    function clkButton()
    {
        var msg9 ="<%=pmtMessage9%>";
        var ret = confirm(msg9);  
        if (!ret) {
            return false;
        }    
        var limitSize=parseFloat(document.getElementById("<%=hidFileMaxSize.ClientID %>").value); 
        var filePath=document.getElementById("<%=dFileUpload.ClientID %>").value;
        if (ShowSize(filePath) >= limitSize) {
            var msg1=document.getElementById("<%=hidMsg1.ClientID %>").value; 
            alert(msg1);
            return false;
        }
        ShowWait();
        return true;
    }
       
    function trySetFocusPdLine() //ok
    {
        var descObj=getMaintainLightNoPdLineCmbObj();

        if(descObj!=null && descObj!=undefined && descObj.disabled!=true)
        {
            descObj.focus();
        }
    }
       
    </script>
</html>



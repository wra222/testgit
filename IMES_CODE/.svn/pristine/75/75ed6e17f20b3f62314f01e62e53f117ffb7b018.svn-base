<%--
/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: Message Box page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2008-08-22  Zhao Meili(DD)        Create    
 * 2008-08-23  Zhao Meili(DD)        Modify                    
 * Known issues:
 */
 --%>

<%@ Page  ContentType="text/html;Charset=UTF-8" Language="C#" AutoEventWireup="true"  CodeFile="ErrMessageDisplay.aspx.cs" Inherits="ErrMessageDisplay" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title> 
    <%=Resources.iMESGlobalDisplay.ResourceManager.GetString(pre + "_titbErrMessage")%>
    </title>
<bgsound  src="" autostart="true" id="bsoundInModal" loop="infinite"></bgsound>
<script type="text/javascript" src="CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" language="javascript">
var scanVal = "";
isErrMsg = true;

function loadParams()
{
    if(window.dialogArguments != null)
    {
        playSoundInModalWindow();
      
        var tempString =window.dialogArguments.toString();       
        for(var i=0;i<tempString.length;i++)
        {
           if(tempString.charAt(i).toString() == String.fromCharCode(10))
           { 
               if ((i > 0) && (tempString.charAt(i-1).toString() == String.fromCharCode(13)))  
               { 
                    tempString = tempString.replace(tempString.charAt(i-1).toString(), "");  
                    tempString = tempString.replace(tempString.charAt(i-1).toString(), "<br>");
                }
                else
                {
                    tempString = tempString.replace(tempString.charAt(i).toString(), "<br>");
                }
           }
        }    
        document.getElementById("mess").innerHTML= tempString; 
        window.document.getElementById("form1").OK.blur() ;   
    }  
}

function OK_onClick()
{ 
     this.window.close();     
}

function getSoundObj()
{
    return document.getElementById("bsoundInModal");
}

</script>
</head>
<body  onload="loadParams()"  bgcolor="white"  >
    <form id="form1" runat="server">  
        <div style="text-align:center"> 
            <br />
            <table  id="table1" border="0"   runat="server">
                <tr>
                    <td  align="center" >   
                        <div id="mess"  class= "errMessage_style" > 
		  	            </div>
                        <button id="OK" onclick="OK_onClick()" onkeydown="event.returnValue = false;"  class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        <span>
                        <%=Resources.iMESGlobalDisplay.ResourceManager.GetString(pre + "_btnOK")%>
                        </span>
                    </button>  
                    
                    
                    </td>
                </tr>
            </table>
        </div>
    </form>
<script language="javascript" type="text/javascript">
    var sUrl = 'Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["FailAudioFile"] %>';
    
    function playSoundInModalWindow()
    {
        var paraPlay = '<%=Request["play"] %>';
        
        if (paraPlay != '' && paraPlay == 'true')
        {
            var obj=getSoundObj();
            obj.src = sUrl;    
        }
       
    }
</script>
</body>
</html>



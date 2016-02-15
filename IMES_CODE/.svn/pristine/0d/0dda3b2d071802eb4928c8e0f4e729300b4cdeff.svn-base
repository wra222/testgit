
<%@ Page  ContentType="text/html;Charset=UTF-8" Language="C#" AutoEventWireup="true"  CodeFile="ShipToCartonLabel_ChangeLabel.aspx.cs" Inherits="ShipToCartonLabel_ChangeLabel" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title> 
    <%=Resources.iMESGlobalDisplay.ResourceManager.GetString(pre + "_titbErrMessage")%>
    </title>
<bgsound  src="" autostart="true" id="bsoundInModal" loop="infinite"></bgsound>
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" language="javascript">

function loadParams()
{
    if(window.dialogArguments != null)
    {      
        var tempString =window.dialogArguments[0].toString();       
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
</script>
</body>
</html>



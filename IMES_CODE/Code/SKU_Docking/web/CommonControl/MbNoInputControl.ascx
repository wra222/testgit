<%--
 INVENTEC corporation (c)2009 all rights reserved. 
 Description: Common input control
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-27  Tong.Zhi-Yong(EB2)   Create 

 Known issues:
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MbNoInputControl.ascx.cs" Inherits="CommonControl_CommonInputControl" %>
<asp:TextBox id="txtInput1" runat="server"/>
<input type="hidden" id="hidKeyAllowTime" runat="server" />
<script language="javascript" type="text/javascript">
    var flag2 = "";

    var allowKeyInTime2 = -1;
    //记录input框输入的数组
    var sInput2 = new Array(100);
    //起始的读位置
    var readIndex2=0;
    //起始的写位置
    var writeIndex2=-1;
    var finishInputflag2 = false;
    

    
    function onDateKeyDown2() {
        var inputContent = document.getElementById("<%=txtInput1.ClientID%>").value;
   
        if(inputContent.length == 0)
        {
            timeStart = new Date();  
        }
        
        if ( event.keyCode == 13) 
        {        
            var canUserKeyboard = document.getElementById("<%=txtInput1.ClientID%>").getAttribute("keyboard");
            var isClear = document.getElementById("<%=txtInput1.ClientID%>").getAttribute("isClear");
            //added by itc207013
            var ETFun= document.getElementById("<%=txtInput1.ClientID%>").getAttribute("enterOrTabFun");
            
            if (isClear == null) {
                finishInputflag2 = true; 
            }
            
            
            
            if (inputContent.length != 0 && inputContent.trim() != "") {
                if (canUserKeyboard != null) {
                    if (canUserKeyboard.toLowerCase() == "false") {
                        var isScanner = null;
                    
                        timeStop = new Date();
                        isScanner = CountTime(timeStart,timeStop); 
                        if (!isScanner) {
                            //need modify the parameter content
                            alert("Please use scanner to input data!");
                            event.returnValue = false;
                            
                            try
                            {
                                //ShowInfo("Please use scanner to input data!");
                            }
                            catch(e)
                            {
                                document.getElementById("<%=txtInput1.ClientID%>").value = "";
                                document.getElementById("<%=txtInput1.ClientID%>").focus();
                                
                                return;
                            }
                            
                            document.getElementById("<%=txtInput1.ClientID%>").value = "";
                            document.getElementById("<%=txtInput1.ClientID%>").focus();
                            
                            return;       
                        }
                    }
                }
                
                inputContent = inputContent.toUpperCase();
                document.getElementById("<%=txtInput1.ClientID%>").value = inputContent.trim();
                
                if (document.getElementById("<%=txtInput1.ClientID%>").getAttribute("ProcessQuickInput") == "true") {
                    writeIndex2++;
                    
                    sInput2[writeIndex2] = event.returnValue ? content : inputContent.trim();                   
                }
                
                if (isClear != null) {
                    document.getElementById("<%=txtInput1.ClientID%>").value = "";
                }  
                
                event.returnValue = false;
            } else {
                //need modify the parameter content
                if (document.getElementById("<%=txtInput1.ClientID%>").getAttribute("ProcessQuickInput") == "true") {
                    var txtObj = document.getElementById("<%=txtInput1.ClientID%>");
                    
                    if (txtObj.AllowPrompt == undefined || txtObj.AllowPrompt == null || txtObj.AllowPrompt == true)
                    {
                        alert("Please input or scan!");    
                        event.returnValue = false;
                        
                        //ITC-932-0200 2009-01-21 Tong.Zhi-Yong
                        try
                        {
                            //ShowInfo("Please input or scan!");
                        }
                        catch(e)
                        {
                            document.getElementById("<%=txtInput1.ClientID%>").focus();
                            return;
                        }
                        
                        document.getElementById("<%=txtInput1.ClientID%>").focus();                          
                    }
                    else
                    {
                        txtObj.AllowPrompt = true;                
                    }
                }
                
                event.returnValue = false;             
            }
        //modified by itc207013
        if ((ETFun != null)&&(ETFun != ""))
        {
            eval(ETFun);
        }
            
        } else {
            if (isClear == null) {
                if ((finishInputflag2)&&(event.keyCode != 17)) 
                {
                    document.getElementById("<%=txtInput1.ClientID%>").value = "";
                    finishInputflag2 = false;
                }                
            }
        
        }
    }
    
    function keyPress2() {
        var inputContent = document.getElementById("<%=txtInput1.ClientID%>").value;
        var pattern = /^[-0-9a-zA-Z\s\*]*$/;
        var content = inputContent + String.fromCharCode(event.keyCode);
        var inputPattern = document.getElementById("<%=txtInput1.ClientID%>").getAttribute("expression");
        
        if (inputPattern != null && inputPattern != "") {
            pattern = new RegExp(inputPattern);
        }
        
        if (pattern.test(content)) {
	        event.returnValue = true;
	    } else {
	        event.returnValue = false;
	    }
    }

    function getCommonInputObject2() {
        return document.getElementById("<%=txtInput1.ClientID%>");
    }
  
 
    
  
</script>
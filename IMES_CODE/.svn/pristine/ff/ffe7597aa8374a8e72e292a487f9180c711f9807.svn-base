<%--
 INVENTEC corporation (c)2009 all rights reserved. 
 Description: Common input control
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-27  Tong.Zhi-Yong(EB2)   Create 

 Known issues:
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MBNoInputContrl2.ascx.cs" Inherits="CommonControl_MBNoInputContrl2" %>
<asp:TextBox id="txtInput2" runat="server"/>
<input type="hidden" id="hidKeyAllowTime" runat="server" />
<script language="javascript" type="text/javascript">
    var flag3 = "";

    var allowKeyInTime3 = -1;
    //记录input框输入的数组
    var sInput3 = new Array(100);
    //起始的读位置
    var readIndex3=0;
    //起始的写位置
    var writeIndex3=-1;
    var finishInputflag3 = false;
    

    
    function onDateKeyDown3() {
        var inputContent = document.getElementById("<%=txtInput2.ClientID%>").value;
   
        if(inputContent.length == 0)
        {
            timeStart = new Date();  
        }
        
        if (event.keyCode == 13) 
        {        
            var canUserKeyboard = document.getElementById("<%=txtInput2.ClientID%>").getAttribute("keyboard");
            var isClear = document.getElementById("<%=txtInput2.ClientID%>").getAttribute("isClear");
            //added by itc207013
            var ETFun= document.getElementById("<%=txtInput2.ClientID%>").getAttribute("enterOrTabFun");
            
            if (isClear == null) {
                finishInputflag3 = true; 
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
                                document.getElementById("<%=txtInput2.ClientID%>").value = "";
                                document.getElementById("<%=txtInput2.ClientID%>").focus();
                                
                                return;
                            }
                            
                            document.getElementById("<%=txtInput2.ClientID%>").value = "";
                            document.getElementById("<%=txtInput2.ClientID%>").focus();
                            
                            return;       
                        }
                    }
                }
                
                inputContent = inputContent.toUpperCase();
                document.getElementById("<%=txtInput2.ClientID%>").value = inputContent.trim();
                
                if (document.getElementById("<%=txtInput2.ClientID%>").getAttribute("ProcessQuickInput") == "true") {
                    writeIndex3++;
                    
                    sInput3[writeIndex3] = event.returnValue ? content : inputContent.trim();                   
                }
                
                if (isClear != null) {
                    document.getElementById("<%=txtInput2.ClientID%>").value = "";
                }  
                
                event.returnValue = false;
            } else {
                //need modify the parameter content
                if (document.getElementById("<%=txtInput2.ClientID%>").getAttribute("ProcessQuickInput") == "true") {
                    var txtObj = document.getElementById("<%=txtInput2.ClientID%>");
                    
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
                            document.getElementById("<%=txtInput2.ClientID%>").focus();
                            return;
                        }
                        
                        document.getElementById("<%=txtInput2.ClientID%>").focus();                          
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
                if ((finishInputflag3)&&(event.keyCode != 17)) 
                {
                    document.getElementById("<%=txtInput2.ClientID%>").value = "";
                    finishInputflag3 = false;
                }                
            }
        
        }
    }
    
    function keyPress3() {
        var inputContent = document.getElementById("<%=txtInput2.ClientID%>").value;
        var pattern = /^[-0-9a-zA-Z\s\*]*$/;
        var content = inputContent + String.fromCharCode(event.keyCode);
        var inputPattern = document.getElementById("<%=txtInput2.ClientID%>").getAttribute("expression");
        
        if (inputPattern != null && inputPattern != "") {
            pattern = new RegExp(inputPattern);
        }
        
        if (pattern.test(content)) {
	        event.returnValue = true;
	    } else {
	        event.returnValue = false;
	    }
    }

    function getCommonInputObject3() {
        return document.getElementById("<%=txtInput2.ClientID%>");
    }
  
 
    
  
</script>
<%--
 INVENTEC corporation (c)2009 all rights reserved. 
 Description: Common input control
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-27  207006(EB2)          Create 

 Known issues:
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CommonInputWithoutQuick.ascx.cs" Inherits="CommonControl_CommonInputWithoutQuick" %>
<asp:TextBox id="txtInput_1" runat="server"/>
<input type="hidden" id="hidKeyAllowTime" runat="server" />
<script language="javascript" type="text/javascript">
    var flag = "";
    var timeStart;
    var timeStop;
    var allowKeyInTime = -1;
//    //记录input框输入的数组
//    var sInput = new Array(100);
//    //起始的读位置
//    var readIndex=0;
//    //起始的写位置
//    var writeIndex=-1;
    var finishInputFlag = false;
     
    function onDateKeyDown_1() 
    {
        var inputContent = document.getElementById("<%=txtInput_1.ClientID%>").value;
   
        if(inputContent.length == 0)
        {
            timeStart = new Date();  
        }
     
        if (event.keyCode == 9 || event.keyCode == 13) 
        {        
            var canUserKeyboard = document.getElementById("<%=txtInput_1.ClientID%>").getAttribute("keyboard");
            var isClear = document.getElementById("<%=txtInput_1.ClientID%>").getAttribute("isClear");
            //added by itc207013
            var ETFun= document.getElementById("<%=txtInput_1.ClientID%>").getAttribute("enterOrTabFun");
            
            if (isClear == null) 
            {
                finishInputFlag = true; 
            }
            
            //document.getElementById("<%=txtInput_1.ClientID%>").value = "";
            
            if (inputContent.length != 0 && inputContent.trim() != "") 
            {
                if (canUserKeyboard != null) 
                {
                    if (canUserKeyboard.toLowerCase() == "false") 
                    {
                        var isScanner = null;
                    
                        timeStop = new Date();
                        isScanner = CountTime_1(timeStart,timeStop); 
                        if (!isScanner) 
                        {
                            //need modify the parameter content
                            ShowMessageByUrl("../ErrMessageDisplay.aspx", "Please use scanner to input data!");
                            event.returnValue = false;
                            
                            try
                            {
                                ShowInfo("Please use scanner to input data!");
                            }
                            catch(e)
                            {
                                document.getElementById("<%=txtInput_1.ClientID%>").value = "";
                                document.getElementById("<%=txtInput_1.ClientID%>").focus();
                                
                                return;
                            }
                            
                            document.getElementById("<%=txtInput_1.ClientID%>").value = "";
                            document.getElementById("<%=txtInput_1.ClientID%>").focus();
                            
                            return;       
                        }
                    }
                }
                
                inputContent = inputContent.toUpperCase();
                document.getElementById("<%=txtInput_1.ClientID%>").value = inputContent.trim();
                
//                if (document.getElementById("<%=txtInput_1.ClientID%>").getAttribute("ProcessQuickInput") == "true") {
//                    writeIndex++;
//                    
//                    sInput[writeIndex] = event.returnValue ? content : inputContent.trim();                   
//                }
                
                if (isClear != null) 
                {
                    document.getElementById("<%=txtInput_1.ClientID%>").value = "";
                }  
                
                event.returnValue = false;
            } 
            else 
            {
                //need modify the parameter content
//                if (document.getElementById("<%=txtInput_1.ClientID%>").getAttribute("ProcessQuickInput") == "true") {
                    ShowMessageByUrl("../ErrMessageDisplay.aspx", "Please input or scan!");    
                    event.returnValue = false;
                    
                    //ITC-932-0200 2009-01-21 Tong.Zhi-Yong
                    try
                    {
                        ShowInfo("Please input or scan!");
                    }
                    catch(e)
                    {
                        document.getElementById("<%=txtInput_1.ClientID%>").focus();
                        return;
                    }
                    
                    document.getElementById("<%=txtInput_1.ClientID%>").focus();      
//                }        
            }
            //modified by itc207013
            if ((ETFun != null)&&(ETFun != ""))
            {
                eval(ETFun);
            }   
        } 
        else 
        {
            if (isClear == null) 
            {
                if ((finishInputFlag)&&(event.keyCode != 17)) 
                {
                    document.getElementById("<%=txtInput_1.ClientID%>").value = "";
                    finishInputFlag = false;
                }                
            }
            if (getCommonInputObject_1().onkeypress == null) 
            {
                getCommonInputObject_1().onkeypress = keyPress_1;
            }
        }
    }
    
    function keyPress_1() 
    {
        var inputContent = document.getElementById("<%=txtInput_1.ClientID%>").value;
        var pattern = /^[-0-9a-zA-Z\s\*]*$/;
        var content = inputContent + String.fromCharCode(event.keyCode);
        var inputPattern = document.getElementById("<%=txtInput_1.ClientID%>").getAttribute("expression");
        
        if (inputPattern != null && inputPattern != "") {
            pattern = new RegExp(inputPattern);
        }
        
        if (pattern.test(content)) {
	        event.returnValue = true;
	    } else {
	        event.returnValue = false;
	    }
    }
    
    //计算时间
    function CountTime_1(timeStart1,timeStop1)
    {
        var flag1 = false;
        var tmp = timeStop1.getTime() - timeStart1.getTime();
        var hidTime = document.getElementById("<%=hidKeyAllowTime.ClientID %>").value;
        
        allowKeyInTime = parseInt(hidTime, 10);
        
        if(tmp <= allowKeyInTime)
        {
            flag1 = true;
        }
        
        return flag1;
    }    
        
    String.prototype.trim = function() {
        if (this == null) {
            this == "";
        }
        
        return this.replace(/^\s*(.*?)[\s\n]*$/g, '$1');
    }   
    
    function getCommonInputObject_1() {
        return document.getElementById ("<%=txtInput_1.ClientID%>");
    }
</script>
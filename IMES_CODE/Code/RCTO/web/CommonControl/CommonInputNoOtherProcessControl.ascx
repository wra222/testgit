﻿<%--
 INVENTEC corporation (c)2009 all rights reserved. 
 Description: Common input control
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-27  Tong.Zhi-Yong(EB2)   Create 

 Known issues:
 --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CommonInputNoOtherProcessControl.ascx.cs" Inherits="CommonControl_CommonInputControl" %>
<asp:TextBox id="txtInput" runat="server"  Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"  />
<input type="hidden" id="hidKeyAllowTime" runat="server" />
<script language="javascript" type="text/javascript">
    var flag = "";
    var timeStart;
    var timeStop;
    var allowKeyInTime = -1;
    //记录input框输入的数组
    var sInput = new Array(100);
    //起始的读位置
    var readIndex=0;
    //起始的写位置
    var writeIndex=-1;
    var finishInputFlag = false;
    
    function inputClick() {
        alert("inputClick");
    }
    
    function onDateKeyDown() {
        var inputContent = document.getElementById("<%=txtInput.ClientID%>").value;
   
        if (event.keyCode == 9 || event.keyCode == 13) 
        {        
            var isClear = document.getElementById("<%=txtInput.ClientID%>").getAttribute("isClear");
            
            if (isClear == null) {
                finishInputFlag = true; 
            }
            
            if (inputContent.length != 0 && inputContent.trim() != "") {
                inputContent = inputContent.toUpperCase();
                document.getElementById("<%=txtInput.ClientID%>").value = inputContent.trim();
                
                if (document.getElementById("<%=txtInput.ClientID%>").getAttribute("ProcessQuickInput") == "true") {
                    writeIndex++;
                    
                    sInput[writeIndex] = event.returnValue ? content : inputContent.trim();                   
                }
                
                if (isClear != null) {
                    document.getElementById("<%=txtInput.ClientID%>").value = "";
                }  
                
                event.returnValue = false;
            } else {
                //need modify the parameter content
                if (document.getElementById("<%=txtInput.ClientID%>").getAttribute("ProcessQuickInput") == "true") {
                    var txtObj = document.getElementById("<%=txtInput.ClientID%>");
                    
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
                            document.getElementById("<%=txtInput.ClientID%>").focus();
                            return;
                        }
                        
                        document.getElementById("<%=txtInput.ClientID%>").focus();                          
                    }
                    else
                    {
                        txtObj.AllowPrompt = true;                
                    }
                }
                
                event.returnValue = false;             
            }
        } else {
            if (isClear == null) {
                if ((finishInputFlag)&&(event.keyCode != 17)) 
                {
                    document.getElementById("<%=txtInput.ClientID%>").value = "";
                    finishInputFlag = false;
                }                
            }
        }
    }
    
    //show Error
    function showErrorMessage(errorContent)
    {
        window.showModalDialog('ErrMessageDisplayFram.aspx',errorContent,'dialogWidth:650px;dialogHeight:450px;status:no;help:no;menubar:no;toolbar:no;resize:no;');
        document.getElementById ("<%=txtInput.ClientID%>").focus();
    }    
    
    String.prototype.trim = function() {
        if (this == null) {
            this == "";
        }
        
        return this.replace(/^\s*(.*?)[\s\n]*$/g, '$1');
    }
    
    //public method
    function hasAvailableData() {
        return readIndex > writeIndex ? false : true;
    }
    
    function getAvailableData(param) {
        if (!hasAvailableData()) {
            window.setTimeout("getAvailableData('" + param + "')", 300);
            return;
        } else {
            var returnData = sInput[readIndex];
            
            readIndex++;
            eval(param + "('" + returnData + "');");     
        }
    }
    
    function clearData() {
        readIndex=0;
        writeIndex=-1;
    }
    
    function disableControl() {
        document.getElementById("<%=txtInput.ClientID%>").disabled = true;
    }
    
    function enableControl() {
        document.getElementById("<%=txtInput.ClientID%>").disabled = false;
    }
    
    function getCommonInputObject() {
        return document.getElementById ("<%=txtInput.ClientID%>");
    }
</script>
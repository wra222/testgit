<%@ Page Language="C#" AutoEventWireup="true"  ContentType="text/html;Charset=UTF-8" CodeFile="ProcessSaveAs.aspx.cs" Inherits="DataMaintain_ProcessSaveAs"  ValidateRequest="false" %>
<%--ITC-1361-0068--%>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>iMES-Save As a new Process</title>
    <link href="CSS/dataMaintain.css" type="text/css" rel="stylesheet" />    
    <script type="text/javascript" src=" ../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
</head>

<body style="background-color: #ECE9D8" >
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div style="height:30px; width: 273px;">
    </div>
    <div>
        <table style="width:100%;" >
             <tr style="height :35px">
                <td style="width:2%"></td>
                <td style="width:10%">
                    <asp:Label ID="lblProcess" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td style="width:35%">
                    <asp:TextBox ID="dProcess" runat="server"   MaxLength="10"  Width="86%" SkinId="textBoxSkin" onkeypress='return OnKeyPress(this)'></asp:TextBox>
                </td>
                <td style="width:2%"></td>
            </tr>  
             <tr style="height :35px">
                <td style="width:2%"></td>
                <td style="width:10%">
                    <asp:Label ID="lblDescr" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td style="width:35%">
                    <asp:TextBox ID="dDescr" runat="server"   MaxLength="80"  Width="86%" SkinId="textBoxSkin" onkeypress='return OnKeyPress(this)'></asp:TextBox>
                </td>
                <td style="width:2%"></td>
            </tr> 
             <tr style="height :35px">
                <td style="width:2%"></td>
                <td style="width:10%">
                    <asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td >
                    <asp:Label ID="dType" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                </td>                                        
                <td style="width:2%"></td>
            </tr>                           
        </table>
    </div>
    
    <div style="height:20px; width: 100%;">
    </div>
        
    <div style="width: 100%">
        <table style="width: 87%">
        <tr>
        <td align="right">            
          
            <input type="button" id="btnOK" runat="server" onclick="if(clkButton())" class="iMes_button" onserverclick="btnOK_ServerClick"></input>
            &nbsp;
            <input type="button" id="btnCancel" runat="server" onclick="btnCancel_Click()" class="iMes_button" ></input>
        </td>
         </tr>   
        </table>
    </div>
    
    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="ServerClick" />

    </Triggers>                      
    </asp:UpdatePanel>   
    
    <input type="hidden" id="HiddenUserName" runat="server" />
    <input type="hidden" id="dOldProcess" name="dCustomerId" runat="server" />
    <input type="hidden" id="dOldType" name="dCustomerId" runat="server" />
    </form>
    
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div> 
    
</body>
    <script type="text/javascript">    
   
   var msg1="";
   var msg2="";
   var msg3="";
    
    function clkButton() 
    {
       switch(event.srcElement.id)
       {
           case "<%=btnOK.ClientID %>":      
                var processValue=document.getElementById("<%=dProcess.ClientID %>").value.trim();
                if(processValue=="")
                {
                    alert(msg1);
                    return false;
                }                
 	       break;
 	    }
 	    ShowWait();
        return true; 	    

    }
       
    function AddUpdateComplete(process)
    {
        window.returnValue = process;
        window.close();    
    }


//    window.onunload=function()
//    {
//    
//    }
    
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";

    }

    function btnCancel_Click() {
        window.returnValue = null;
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
    }

    function DealHideWait()
    {
        HideWait();   
    }


    </script>
</html>





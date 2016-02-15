<%@ Page Language="C#" AutoEventWireup="true"  ContentType="text/html;Charset=UTF-8" CodeFile="MaintainSaveAs.aspx.cs" Inherits="DataMaintain_MaintainSaveAs"  ValidateRequest="false" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>BOM Code Save As</title>
</head>
<body style="background-color: #ECE9D8" >
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/DataMaintainEx/Service/BOMNodeDataService.asmx" />
        </Services>
    </asp:ScriptManager>
    <input type="hidden" id="dOldPartNo" name="dCustomerId" runat="server" />
    <div style="height:30px; width: 273px;">
    </div>
    <div>
        <table style="width:100%;" >
            <tr style="height :35px">
                <td style="width:2%"></td>
                <td style="width:45%" colspan="2">
                    <asp:Label ID="lblmessage" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td style="width:2%"></td>
            </tr>    
             <tr style="height :35px">
                <td style="width:2%"></td>
                <td style="width:10%">
                    <asp:Label ID="lblNewCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td style="width:35%">
                    <asp:TextBox ID="dNewCode" runat="server"   MaxLength="50"  Width="86%" CssClass="iMes_textbox_input_Yellow"  onkeypress='return OnKeyPress(this)'></asp:TextBox>
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
          
            <input type="button" id="btnOK" value="OK" style="width:66px" onclick="return btnOK_Click()" runat="server" />
            &nbsp;
            <input type="button" id="btnCancel" value="Cancel" style="width:66px" onclick="return btnCancel_Click()" runat="server" />
            
        </td>
         </tr>   
        </table>
    </div>
    <input type="hidden" id="HiddenUserName" runat="server" />
    </form>
</body>
    <script type="text/javascript">
        var IsSaveDummy = '<%=Request["IsSaveDummy"] %>';
        var Customer = '<%=Request["Customer"] %>';
       var oldCode;
       var newCode;
       
       var isDoing;
       
       var msg1="";
       var msg2="";
       var msg3="";
        
       function changeNull(val)
       {
           if ("undefined" == val || null == val)
           {
               val = "";
           }
           return val;
       }
    
        function btnOK_Click() 
        {
            if(isDoing==true)
            {
                return;
            }
            var editor = document.getElementById("HiddenUserName").value;
            oldCode = document.getElementById("dOldPartNo").value.trim();
            if (IsSaveDummy) {
                BOMNodeDataService.SaveAsDummy(oldCode, editor, Customer, true, AddComplete, AddFailComplete);
            }
            else {
                newCode = document.getElementById("dNewCode").value.trim().toUpperCase();
                if (newCode == "") {
                    alert(msg1);
                    return;
                }
                if (newCode == oldCode) {
                    alert(msg2);
                    return;
                }
                isDoing = true;
                BOMNodeDataService.SaveAs(oldCode, newCode, editor, true, AddComplete, AddFailComplete);
            }
        }
        
        function AddFailComplete(error) {
            //alert(error.get_message());
            //!!! need change
            //ShowMessage(error.get_message());
            alert(error.get_message());
            isDoing=false;
            return;
        }          
           
        function AddComplete(returnCode) {

            if(returnCode=="")
            {
               //正常情况下一定有返回值的
               //3
               var ret=confirm(msg3);
//             var ret=confirm("您所输入的Code已存在于Model BOM中，若继续执行本操作将导致该Code的所有下阶Code都被替换。请问是否继续？");
               if (!ret) 
               {
                   isDoing=false;
                   return;  
               }  
               else
               {
                   var editor=document.getElementById("HiddenUserName").value;
                   BOMNodeDataService.SaveAs(oldCode, newCode, editor, false, AddComplete, AddFailComplete);
               }             
            }
            else
            {
                window.returnValue = returnCode;
                window.close();
            }
            isDoing=false;
        }

        window.onload = function() {
            msg1 = "<%=pmtMessage1%>";
            msg2 = "<%=pmtMessage2%>";
            msg3 = "<%=pmtMessage3%>";
            if (IsSaveDummy == "Y") {
                document.getElementById("lblNewCode").style.visibility = "hidden"
                document.getElementById("dNewCode").style.visibility = "hidden"
                document.getElementById("lblmessage").innerText = "是否要產生DummyModel?";
            }
            document.getElementById("dNewCode").focus();
            isDoing = false;
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

    </script>
</html>





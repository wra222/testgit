<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InputProductlist.aspx.cs" Inherits="PAK_InputProductlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Input Product/CUSTSN</title>
    <style type="text/css">
      
        #Button1
        {
            height: 36px;
            width: 75px;
        }
        #Button2
        {
           height: 36px;
            width: 75px;
        }
    </style>
</head>
<body onload="GetDN()" >
    <form id="form1" runat="server">
    <div>
    
        <br />
    
    </div>
    <p>
        <textarea id="dnList" 
            
            
            
            style="font-family: 'Courier New';overflow-x:hidden; overflow-y:scroll; font-size: medium; height: 500px; width: 230px;" ></textarea>
  

    <asp:HiddenField ID="hidDNList" runat="server"  />
    </form>
    
    
        &nbsp;&nbsp;
        <input id="Button1" type="button" value="OK" onclick="btnOK_Click()" />
 <input id="Button2" type="button" value="Cancel" onclick="btnCancel_Click()" />
<script type="text/javascript">
    function btnOK_Click() {

        document.getElementById("<%=hidDNList.ClientID %>").value = document.getElementById('dnList').value;
        window.returnValue = document.getElementById('dnList').value;
        
         window.close();
    }
    function btnCancel_Click() {
        if (document.getElementById("<%=hidDNList.ClientID %>").value != "")
        { window.returnValue = document.getElementById("<%=hidDNList.ClientID %>").value ; }
        else
        { window.returnValue = false; }
        
        window.close();
    }
    function GetDN() {

        var dnList = document.getElementById("<%=hidDNList.ClientID %>").value;
       var dn="";

       if (dnList != "") {
           var arr = dnList.split(",");

           for (var m in arr) {
              dn = dn + arr[m] + "\r";
             
              // content += key + ' : ' + myarr[key] + '<br />';

          }
          //model = model.substring(0, model.length-1)
       }


       document.getElementById("dnList").value = dn;
        
      //  alert(c);
    }

</script>    
</body>

</html>

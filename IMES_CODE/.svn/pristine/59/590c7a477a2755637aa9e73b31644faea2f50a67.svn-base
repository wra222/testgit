<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadCartonSNList.aspx.cs" Inherits="Query_PAK_UploadCartonSNList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body onload="a()">
    <form id="form1" runat="server">
    <div>
    
        <br />
    
    </div>
    
        <textarea id="CartonSNList" 
            
            
            style="font-family:'Courier New';overflow-x:hidden; overflow-y:scroll; font-size: medium; height: 500px;" ></textarea>
  
<div style="margin-top:12px">
         <input id="Button1" type="button" value="OK" onclick="btnOK_Click()" />
 <input id="Button2" type="button" value="Cancel" onclick="btnCancel_Click()" />
</div>
    <asp:HiddenField ID="hidModelList" runat="server" />
    </form>
    
        &nbsp;&nbsp;

<script type="text/javascript">
    function btnOK_Click() {

        document.getElementById("<%=hidModelList.ClientID %>").value = document.getElementById('CartonSNList').value;
        window.returnValue = document.getElementById('CartonSNList').value;
        
         window.close();
    }
    function btnCancel_Click() {
        window.returnValue = false;
        window.close();
    }
    function a() {
     
       var modelList=document.getElementById("<%=hidModelList.ClientID %>").value;
       var model="";

       if (modelList != "") {
           var arr = modelList.split(",");

           for (var m in arr) {
              model = model + arr[m] + "\r";
             
              // content += key + ' : ' + myarr[key] + '<br />';

          }
          //model = model.substring(0, model.length-1)
       }
    
      
        document.getElementById("CartonSNList").value = model;
        
      //  alert(c);
    }

</script>    
</body>
</html>

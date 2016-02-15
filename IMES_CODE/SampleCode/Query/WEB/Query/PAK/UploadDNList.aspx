﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadDNList.aspx.cs" Inherits="PAK_UploadDNList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #TextArea1
        {
            height: 416px;
            width: 292px;
            border-style:ridge;
	border-width:1px;
	border-left-color:Black;
	border-right-color:Gray;
	border-bottom-color:Black;
	border-top-color:Gray;
	font-family:Verdana;
	font-size: 9pt;
	padding-left:2px;
	padding-right:2px;
	text-align: left;
	vertical-align:middle;
    background-color:White;
    margin-left:1px;
    margin-right:1px;
    white-space: pre-line;		/* ITC-1103-0065 */
    word-wrap: break-word;
    overflow:hidden;
        }
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
            
            
            
            style="font-family: 'Courier New';overflow-x:hidden; overflow-y:scroll; font-size: medium; height: 450px; width: 300px;" ></textarea>
    </p>
    <p>
        &nbsp;</p>

    <asp:HiddenField ID="hidDNList" runat="server" />
    </form>
    
    <p>
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
        window.returnValue = false;
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
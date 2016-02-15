<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Inventec_MainPage.aspx.cs" Inherits="webroot_aspx_Inventec_1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>英业达（重庆）生产效率（部门汇总）</title>
    <style>
   
        
   
   
        
   
   </style>
</head>
<body leftMargin="0" topMargin="0" rightMargin="1" scrolling="no">


  

<div style="width: 1300px; height: 350px;">



<div id="Div1" style="width: 50%; height: 50%; margin: 0px auto; position: absolute; top: 10px; left: 0px; border: 1px solid #eee;" >

       <iframe id="SMT" src="InventecUPD.aspx" style="height:100%;width:100%;z-index:-1;" frameborder="0" marginwidth="0" marginheight="0" scrolling="no" allowtransparency="true" ></iframe>

 </div>
<div id="Div2" style="width: 50%; height: 50%; margin: 0px auto; position: absolute; top: 10px; left: 700px; border: 1px solid #eee;">

      <iframe id="SA" src="SAUPH.aspx" style="height:100%;width:100%;z-index:-1;" frameborder="0" marginwidth="0" marginheight="0" scrolling="no" allowtransparency="true" ></iframe>

  </div>
<div id="Div3" style="width: 50%; height: 50%; margin: 10px auto; position: absolute; top: 50%; left: 0px; border: 1px solid #eee;">

       <iframe id="Iframe1" src="FAUPH.aspx" style="height:100%;width:100%;z-index:-1;" frameborder="0" marginwidth="0" marginheight="0" scrolling="no" allowtransparency="true" ></iframe>
</div>

<div id="Div4" style="width: 50%; height: 50%; margin: 10px auto; position: absolute; top: 50%; left: 700px; border: 1px solid #eee;">

        <iframe id="PAK" src="PAKUPH.aspx" style="height:100%;width:100%;z-index:-1;" frameborder="0" marginwidth="0" marginheight="0" scrolling="no" allowtransparency="true" ></iframe>
</div>
 
</div>
 

  <form id="form1" runat="server" >  
</form>
</body>
</html>

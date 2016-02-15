<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Inventec_all_Month.aspx.cs" Inherits="webroot_aspx_Inventec_all_Month" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>英业达（重庆）生产效率（按月汇总）</title>
    <style>
   
        
   
   
        
   
   </style>
</head>
<body leftMargin="0" topMargin="0" rightMargin="1" scrolling="no">


  

<div style="width: 100%; height: 100%;">



<div id="Div1" style="width: 50%; height: 50%; margin: 0px auto; position: absolute; top: 0px; left: 0px; border: 1px solid #eee;" >

       <iframe id="SMT" src="UPH_SMT_Month.aspx" style="height:100%;width:100%;z-index:-1;" frameborder="0" marginwidth="0" marginheight="0" scrolling="no" allowtransparency="true" ></iframe>

 </div>
<div id="Div2" style="width: 50%; height: 50%; margin: 0px auto; position: absolute; top: 0px; left: 50%; border: 1px solid #eee;">

      <iframe id="SA" src="UPH_SA_Month.aspx" style="height:100%;width:100%;z-index:-1;" frameborder="0" marginwidth="0" marginheight="0" scrolling="no" allowtransparency="true" ></iframe>

  </div>
<div id="Div3" style="width: 50%; height: 50%; margin: 0px auto; position: absolute; top: 50%; left: 0px; border: 1px solid #eee;">

       <iframe id="Iframe1" src="UPH_FA_Month.aspx" style="height:100%;width:100%;z-index:-1;" frameborder="0" marginwidth="0" marginheight="0" scrolling="no" allowtransparency="true" ></iframe>
</div>

<div id="Div4" style="width: 50%; height: 50%; margin: 0px auto; position: absolute; top: 50%; left: 50%; border: 1px solid #eee;">

        <iframe id="PAK" src="UPH_PAK_Month.aspx" style="height:100%;width:100%;z-index:-1;" frameborder="0" marginwidth="0" marginheight="0" scrolling="no" allowtransparency="true" ></iframe>
</div>
 
</div>
 

  <form id="form1" runat="server" >  
</form>
</body>
</html>

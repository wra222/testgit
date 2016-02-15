
<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: ģ��Ԥ��ҳ��
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   liu xiaoling(EB2)  create
' Known issues:Any restrictions about this file
--%> 
<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_Preview_child, App_Web_preview_child.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" xmlns:v="urn:schemas-microsoft-com:vml">
<head id=Head1 runat="server">
<title>
Preview
</title>
<style>
.btn 
{
	width:40px;
}
span
{
	width:30px;
	height:10px;
	color:Black;
    text-align:center;
	background-color:rgb(147,190,225);
    font:normal normal bold 9pt Verdana;	
}
font
{
    font:normal normal bold 9pt Verdana;	
}

f\:*{behavior:url(#default#vml)} 

</style>
</head>

<fis:header id="header1" runat="server"/>
<body style="margin-bottom:0px;height:100%">
    <form id="form2" runat="server">
    <div>
    </div>
    </form>
    
<table cellpadding=0 cellspacing=0 height=40 width=100% style="border:solid 0 red;">
<tr height=40>
<td style="background-color:rgb(147,190,225)">
&nbsp;
<font>Total:</font><span id=theTotal></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font>Current Page:</font><span id=theNo></span>
</td>
<td align=right style="background-color:rgb(147,190,225)">
<button class="btn" id="btnPrevious" onclick="ToPrevious()" disabled><img id="imgPrevious" src="../main/left-d.JPG"></button>
&nbsp;
<button disabled id="btnNext" onclick="ToNext()" class="btn"><img id="imgNext" src="../main/right-d.JPG"></button>
&nbsp;
</td>
</tr>
</table>  
<div style="overflow:scroll;height:478px;width:644px;background-color:rgb(204,204,204); text-align:center; border:solid 1 gray;">
<!--img id="imgPage" src="" style="height:97%;border:3 solid blue;left:0;top:10; position:relative; background-color :White"-->
<!--bug no:ITC-992-0065-->
<!--reason:ͼƬʧ��-->
<div id="frame"></div>
<xml:namespace prefix=f />
<f:Image id="imgPage" name="imgPage" style="border:solid 0px blue" src=""/>

</div>
</body>
<fis:footer id="footer1" runat="server"/> 

</html>

<script language="javascript">
//    var uuid = '<%= Request.QueryString["uuid"]%>';    
//alert(uuid);
var mm_per_inch = 25.4;//25.4mm/inch

var strArg = window.dialogArguments;
var arrArg = strArg.split("&");
var uuid = arrArg[0];
var total = arrArg[1];
var pixel_per_inch_y = arrArg[2];//96pixels/inch
var pixel_per_inch_x = arrArg[3];//96pixels/inch
var width;// = arrArg[2];
var height;// = arrArg[3];
var verticalResolution;
var horizontalResolution;
var arrImageSize = new Array();
document.getElementById("theTotal").innerText = total;
if(total > 1 ){
    document.getElementById("btnNext").disabled = false;
    document.getElementById("imgNext").src = "../main/right.JPG";
}

var curPage = 0;
document.getElementById("theNo").innerText = curPage + 1;

var rtn = webroot_aspx_template_Preview_child.getImageSize(uuid);
if (rtn.error!=null) {
    alert(rtn.error.Message);
} else {
    var strRtn = rtn.value;
    if(strRtn == "wrong picture"){
        alert("<%=Resources.VisualTemplate.No_Picture%>"); 
    }else{
        arrImageSize = strRtn.split("&");
        width = arrImageSize[0];
        height = arrImageSize[1];
        verticalResolution = arrImageSize[2];
        horizontalResolution = arrImageSize[3];
        
        
        document.getElementById("imgPage").style.width=width*(pixel_per_inch_x/horizontalResolution);//width+"mm";
        document.getElementById("imgPage").style.height=height*(pixel_per_inch_y/verticalResolution);//height+"mm";
        document.getElementById("imgPage").src = "Preview_CreateImg.aspx?name="+uuid+"&page=" + curPage;
    }
}


function ToPrevious(){
    curPage = curPage - 1;
    if(curPage == 0){
        document.getElementById("btnPrevious").disabled = true;
        document.getElementById("imgPrevious").src = "../main/left-d.JPG";
    }
    document.getElementById("btnNext").disabled = false;
    document.getElementById("imgNext").src = "../main/right.JPG";
    document.getElementById("theNo").innerText = curPage + 1;
    imgPage.src = "Preview_CreateImg.aspx?name="+uuid+"&page="+curPage;
}
function ToNext(){
    curPage = curPage + 1;
    if(curPage == (total-1)){
        document.getElementById("btnNext").disabled = true;
        document.getElementById("imgNext").src = "../main/right-d.JPG";
    }
    document.getElementById("btnPrevious").disabled = false;
    document.getElementById("imgPrevious").src = "../main/left.JPG";
    document.getElementById("theNo").innerText = curPage + 1;
    imgPage.src = "Preview_CreateImg.aspx?name="+uuid+"&page="+curPage;
}



function pixelToMm_x(numPixel){
    return Math.ceil(Math.round(parseFloat(numPixel)*mm_per_inch*100/pixel_per_inch_x)/100);
}

function pixelToMm_y(numPixel){
    return Math.ceil(Math.round(parseFloat(numPixel)*mm_per_inch*100/pixel_per_inch_y)/100);
}

</script>
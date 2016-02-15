<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_TemplateSetting3, App_Web_templatesetting3.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id=Head1 runat="server">
    <title>Template Setting-Step 3</title>
    
</head>
<%--load all js files--%>
<fis:header id="header1" runat="server"/>

<body onload="initPage()"  class="bgBody">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
    <DIV class="wizardTip">
        <span style="font-weight: bold"><%=Resources.Template.templateSetting%></span><br>
        <span style="margin-left:50px"><%=Resources.Template.templateSettingTitle3%></span><br><br>
    </DIV>
    <DIV class="wizardContent" >
    <fieldset >
	    <LEGEND><%=Resources.Template.setting%></LEGEND>
	  
	    <TABLE   border="0" cellpadding="0" cellspacing="5" >
	    <TR>
		    <TD style="width:35%" align="left" class="inputTitle"><%=Resources.Template.width%>:</TD>
		    <TD align="left"><INPUT TYPE="text" NAME="width" id="width" class="inputStyle">mm</TD>
	    </TR>
	    <TR>
		    <TD class="inputTitle"><%=Resources.Template.height%>:</TD>
		    <TD><INPUT TYPE="text" NAME="height" id="height" class="inputStyle">mm</TD>
	    </TR>
	    <TR>
		    <TD class="inputTitle"><%=Resources.Template.headerHeight%>:</TD>
		    <TD><INPUT TYPE="text" NAME="pageheaderheight" id="pageheight" class="inputStyle">mm</TD>
	    </TR>
	    <TR>
		    <TD class="inputTitle"><%=Resources.Template.footerHeight%>:</TD>
		    <TD><INPUT TYPE="text" NAME="pagefootheight" id="pagefootheight" class="inputStyle">mm</TD>
	    </TR>
	    <TR>
		    <TD class="inputTitle"><%=Resources.Template.sec1Height%>:</TD>
		    <TD><INPUT TYPE="text" NAME="sec1height" id="sec1height" class="inputStyle" onkeydown="setButtonStatus()" >mm</TD>
	    </TR>
	    <TR>
		    <TD class="inputTitle"><%=Resources.Template.sec2Height%>:</TD>
		    <TD><INPUT TYPE="text" NAME="sec2height" id="sec2height" class="inputStyle" onkeydown="setButtonStatus()" >mm</TD>
	    </TR>
		    <TR>
		    <TD colspan="2"><font class="tipFont1"><%=Resources.Template.notice%></font></TD>
	    </TR>
	    </TABLE>

    </FIELDSET>
    <br>
    <fieldset>
	    <LEGEND><INPUT TYPE="checkbox" NAME="pageFootCheck" id="pageFootCheck" onclick="setInfoHeight()"><%=Resources.Template.printPageFoot%></LEGEND>
        <DIV style="font:normal normal 9pt Verdana;">
		    &nbsp;&nbsp;<%=Resources.Template.infoHeight%>:&nbsp;&nbsp;<INPUT TYPE="text" NAME="infoHeight" id="infoHeight" class="inputStyle" disabled value="" style="width:177px">mm
        </DIV>
    </FIELDSET>
    </DIV>
    
</body>

<%--set style for readonly input controller--%>
<fis:footer id="footer1" runat="server"/> 

<SCRIPT LANGUAGE="JavaScript">
<!--
function setInfoHeight(){
    if(document.getElementById("pageFootCheck").checked==true){
        document.getElementById("infoHeight").disabled = false;
    }else{
        document.getElementById("infoHeight").disabled = true;
    }
}
var pixel_per_inch_x;
var pixel_per_inch_y;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	initPage
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	��ʼ����ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function initPage()
{   

    pixel_per_inch_x = window.parent.pixel_per_inch_x;
    pixel_per_inch_y = window.parent.pixel_per_inch_y;
    if (!window.parent.framePreFlag[2]) {
        if (window.parent.templateXmlAndHtml.PrintTemplateInfo.TemplateWidth == "") {
            document.getElementById("width").value = "80"
        } else {
            document.getElementById("width").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.TemplateWidth;
        }
        if (window.parent.templateXmlAndHtml.PrintTemplateInfo.TemplateHeight == "") {
            document.getElementById("height").value = "120"
        } else {
            document.getElementById("height").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.TemplateHeight;
        }
        if (window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.AreaHeight == "") {
            document.getElementById("pageheaderheight").value = "0"
        } else {
            document.getElementById("pageheaderheight").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.AreaHeight;
        }
        if (window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.AreaHeight == "") {
            document.getElementById("pagefootheight").value = "0"
        } else {
            document.getElementById("pagefootheight").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.AreaHeight;
        }
         
        
        if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.length > 0 ) {
            document.getElementById("sec1height").value = "0";
            document.getElementById("sec2height").value = "0";
            for (var i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.length; i++) {
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == 0) {
                    document.getElementById("sec1height").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight;
                } 
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == 1) {
                    document.getElementById("sec2height").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight;
                }
            
            }
        } else {
            document.getElementById("sec1height").value = "0";
            document.getElementById("sec2height").value = "0";
        }
       
        if (window.parent.templateXmlAndHtml.PrintTemplateInfo.PrintPageFooter != "") {
            document.getElementById("pageFootCheck").checked = "checked"
            document.getElementById("infoHeight").disabled = false;
            window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooterHeightOffset = window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooterHeightOffset == undefined? "":window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooterHeightOffset
            document.getElementById("infoHeight").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooterHeightOffset;
        }
       
    } else {
        //�����Previous��ť���ִ�ǰһҳ���Next��ť���� 
        document.getElementById("width").value = window.parent.width;
        document.getElementById("height").value = window.parent.height;
        document.getElementById("pageheaderheight").value = window.parent.headerheight;
        document.getElementById("pagefootheight").value = window.parent.footerheight;
        document.getElementById("sec1height").value = window.parent.detail1height;
        document.getElementById("sec2height").value = window.parent.detail2height;
         if (window.parent.printpagefooter != "") {
            document.getElementById("pageFootCheck").checked = "checked"
            document.getElementById("infoHeight").disabled = false;
            document.getElementById("infoHeight").value = window.parent.infoheight;
        }
    }
     window.parent.setButtonStatus(2);
     var sec1height = document.getElementById("sec1height").value;
     var sec2height = document.getElementById("sec2height").value;
     if (((sec1height == "") || (parseFloat(sec1height) == 0)) && ((sec2height == "") || (parseFloat(sec2height) == 0))) {
       
            window.parent.document.getElementById("nextBtn").disabled = true;
            window.parent.document.getElementById("finishBtn").disabled = false;
     }
     window.parent.framePreFlag[2] = false;
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkPageHeader
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	���PageHeader����Ŀؼ��Ƿ���PageHeader����֮��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkPageHeader(pageheaderheight, width1)
{
    var errorFlag = false;
    if (pageheaderheight != "") {
        var height = parseFloat(pageheaderheight);
        var width = parseFloat(width1);
        var convertTemplateWidth = pixelToMm_x(mmToPixel_x(width));
        var convertAreaHeight = pixelToMm_y(mmToPixel_y(height));
        
        
        for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.TextObjects.length; i++) {
            if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.TextObjects[i].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.TextObjects[i].Angle == "180")) {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.TextObjects[i].Y)  + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.TextObjects[i].Height) > parseFloat(convertAreaHeight)) {
                    alert("<%=Resources.Template.textYIllegalHead%>" + "(Page Header Height:" + height + ")");
                    document.getElementById("pageheaderheight").focus();
                    errorFlag = true;
                    break;
                }
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.TextObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.TextObjects[i].Width) > parseFloat(convertTemplateWidth)) {
                    alert("<%=Resources.Template.textXIllegalHead%>"  + "(Tempalte Width:" + width + ")");
                    document.getElementById("width").focus();
                    errorFlag = true;
                    break;
                }
            } else {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.TextObjects[i].Y)  + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.TextObjects[i].Width) > parseFloat(convertAreaHeight)) {
                    alert("<%=Resources.Template.textYIllegalHead%>" + "(Page Header Height:" + height + ")");
                    document.getElementById("pageheaderheight").focus();
                    errorFlag = true;
                    break;
                }
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.TextObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.TextObjects[i].Height) > parseFloat(convertTemplateWidth)) {
                    alert("<%=Resources.Template.textXIllegalHead%>"  + "(Tempalte Width:" + width + ")");
                    document.getElementById("width").focus();
                    errorFlag = true;
                    break;
                }
            }
        }
        if (!errorFlag) {
            for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.BarcodeObjects.length; i++) {
                if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.BarcodeObjects[i].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.BarcodeObjects[i].Angle == "180")) {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.BarcodeObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.BarcodeObjects[i].Height) > parseFloat(convertAreaHeight)) {
                        alert("<%=Resources.Template.barcodeYIllegalHead%>" + "(Page Header Height:" + height + ")");
                        document.getElementById("pageheaderheight").focus();
                        errorFlag = true;
                        break;
                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.BarcodeObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.BarcodeObjects[i].Width)  > parseFloat(convertTemplateWidth)) {
                        alert("<%=Resources.Template.barcodeXIllegalHead%>"  + "(Tempalte Width:" + width + ")");
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                } else {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.BarcodeObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.BarcodeObjects[i].Width) > parseFloat(convertAreaHeight)) {
                        alert("<%=Resources.Template.barcodeYIllegalHead%>" + "(Page Header Height:" + height + ")");
                        document.getElementById("pageheaderheight").focus();
                        errorFlag = true;
                        break;
                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.BarcodeObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.BarcodeObjects[i].Height)  > parseFloat(convertTemplateWidth)) {
                        alert("<%=Resources.Template.barcodeXIllegalHead%>"  + "(Tempalte Width:" + width + ")");
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                }
            }
        }

        if (!errorFlag) {
            for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.LineObjects.length; i++) {
                if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.LineObjects[i].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.LineObjects[i].Angle == "180")) {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.LineObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.LineObjects[i].Thickness)  > parseFloat(convertAreaHeight)) {
                        alert("<%=Resources.Template.lineYIllegalHead%>" + "(Page Header Height:" + height + ")");
                        document.getElementById("pageheaderheight").focus();
                        errorFlag = true;
                        break;
                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.LineObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.LineObjects[i].Length) > parseFloat(convertTemplateWidth)) {
                        alert("<%=Resources.Template.lineXIllegalHead%>"  + "(Tempalte Width:" + width + ")");
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                } else {
                     if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.LineObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.LineObjects[i].Length)  > parseFloat(convertAreaHeight)) {
                        alert("<%=Resources.Template.lineYIllegalHead%>" + "(Page Header Height:" + height + ")");
                        document.getElementById("pageheaderheight").focus();
                        errorFlag = true;
                        break;
                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.LineObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.LineObjects[i].Thickness) > parseFloat(convertTemplateWidth)) {
                        alert("<%=Resources.Template.lineXIllegalHead%>"  + "(Tempalte Width:" + width + ")");
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                
                }
            }
        }
        if (!errorFlag) {
            for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.RectangleObjects.length; i++) {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.RectangleObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.RectangleObjects[i].Height) > parseFloat(convertAreaHeight)) {
                    alert("<%=Resources.Template.recYIllegalHead%>" + "(Page Header Height:" + height + ")");
                    document.getElementById("pageheaderheight").focus();
                    errorFlag = true;
                    break;
                }
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.RectangleObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.RectangleObjects[i].Width) > parseFloat(convertTemplateWidth)) {
                    alert("<%=Resources.Template.recXIllegalHead%>"  + "(Tempalte Width:" + width + ")");
                    document.getElementById("width").focus();
                    errorFlag = true;
                    break;
                }
            }
        }
        if (!errorFlag) {
            for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.PictureObjects.length; i++) {
                if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.PictureObjects[i].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.PictureObjects[i].Angle == "180")) {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.PictureObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.PictureObjects[i].Height) > parseFloat(convertAreaHeight)) {
                        alert("<%=Resources.Template.picYIllegalHead%>"  + "(Page Header Height:" + height + ")");
                        document.getElementById("pageheaderheight").focus();
                        errorFlag = true;
                        break;
                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.PictureObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.PictureObjects[i].Width) > parseFloat(convertTemplateWidth)) {
                        alert("<%=Resources.Template.picXIllegalHead%>"  + "(Tempalte Width:" + width + ")");
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                } else {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.PictureObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.PictureObjects[i].Width) > parseFloat(convertAreaHeight)) {
                        alert("<%=Resources.Template.picYIllegalHead%>"  + "(Page Header Height:" + height + ")");
                        document.getElementById("pageheaderheight").focus();
                        errorFlag = true;
                        break;
                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.PictureObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.PictureObjects[i].Height) > parseFloat(convertTemplateWidth)) {
                        alert("<%=Resources.Template.picXIllegalHead%>"  + "(Tempalte Width:" + width + ")");
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                }
            }
        }
         if (!errorFlag) {
            for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.ArearObjects.length; i++) {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.ArearObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.ArearObjects[i].Height) > parseFloat(convertAreaHeight)) {
                    alert("<%=Resources.Template.areaYIllegalHead%>"  + "(Page Header Height:" + height + ")");
                    document.getElementById("pageheaderheight").focus();
                    errorFlag = true;
                    break;
                }
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.ArearObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.ArearObjects[i].Width) > parseFloat(convertTemplateWidth)) {
                    alert("<%=Resources.Template.areaXIllegalHead%>"  + "(Tempalte Width:" + width + ")");
                    document.getElementById("width").focus();
                    errorFlag = true;
                    break;
                }
            }
        }
        
    }
    return errorFlag;
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkPageFooter
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	���PageFooter����Ŀؼ��Ƿ���PageFooter����֮��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkPageFooter(pagefootheight, width1)
{
    var errorFlag = false;
    if (pagefootheight != "") {
         var height = parseFloat(pagefootheight);
         var width = parseFloat(width1);
         var convertTemplateWidth = pixelToMm_x(mmToPixel_x(width));
         var convertAreaHeight = pixelToMm_y(mmToPixel_y(height));
        
        for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.TextObjects.length; i++) {
            if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.TextObjects[i].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.TextObjects[i].Angle == "180")) {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.TextObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.TextObjects[i].Height) > parseFloat(convertAreaHeight)) {
                    alert("<%=Resources.Template.textYIllegalFoot%>" + "(Page Footer Height:" + height + ")");
                    document.getElementById("pagefootheight").focus();
                    errorFlag = true;
                    break;
                }
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.TextObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.TextObjects[i].Width)  > parseFloat(convertTemplateWidth)) {
                    alert("<%=Resources.Template.textXIllegalFoot%>"  + "(Tempalte Width:" + width + ")");
                    document.getElementById("width").focus();
                    errorFlag = true;
                    break;
                }
            } else {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.TextObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.TextObjects[i].Width) > parseFloat(convertAreaHeight)) {
                    alert("<%=Resources.Template.textYIllegalFoot%>" + "(Page Footer Height:" + height + ")");
                    document.getElementById("pagefootheight").focus();
                    errorFlag = true;
                    break;
                }
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.TextObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.TextObjects[i].Height)  > parseFloat(convertTemplateWidth)) {
                    alert("<%=Resources.Template.textXIllegalFoot%>"  + "(Tempalte Width:" + width + ")");
                    document.getElementById("width").focus();
                    errorFlag = true;
                    break;
                }
            }
            
        }
        if (!errorFlag) {
            for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.BarcodeObjects.length; i++) {
                if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.BarcodeObjects[i].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.BarcodeObjects[i].Angle == "180")) {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.BarcodeObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.BarcodeObjects[i].Height) > parseFloat(convertAreaHeight)) {
                        alert("<%=Resources.Template.barcodeYIllegalFoot%>" + "(Page Footer Height:" + height + ")");
                        document.getElementById("pagefootheight").focus();
                        errorFlag = true;
                        break;
                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.BarcodeObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.BarcodeObjects[i].Width) > parseFloat(convertTemplateWidth)) {
                        alert("<%=Resources.Template.barcodeXIllegalFoot%>"  + "(Tempalte Width:" + width + ")");
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                } else {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.BarcodeObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.BarcodeObjects[i].Width) > parseFloat(convertAreaHeight)) {
                        alert("<%=Resources.Template.barcodeYIllegalFoot%>" + "(Page Footer Height:" + height + ")");
                        document.getElementById("pagefootheight").focus();
                        errorFlag = true;
                        break;
                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.BarcodeObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.BarcodeObjects[i].Height) > parseFloat(convertTemplateWidth)) {
                        alert("<%=Resources.Template.barcodeXIllegalFoot%>"  + "(Tempalte Width:" + width + ")");
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                }
            }
        }
        if (!errorFlag) {
            
            for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.LineObjects.length; i++) {
                if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.LineObjects[i].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.LineObjects[i].Angle == "180")) {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.LineObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.LineObjects[i].Thickness) > parseFloat(convertAreaHeight)) {
                        alert("<%=Resources.Template.lineYIllegalFoot%>" + "(Page Footer Height:" + height + ")");
                        document.getElementById("pagefootheight").focus();
                        errorFlag = true;
                        break;
                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.LineObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.LineObjects[i].Length) > parseFloat(convertTemplateWidth)) {
                        alert("<%=Resources.Template.lineXIllegalFoot%>"  + "(Tempalte Width:" + width + ")");
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                } else {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.LineObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.LineObjects[i].Length) > parseFloat(convertAreaHeight)) {
                        alert("<%=Resources.Template.lineYIllegalFoot%>" + "(Page Footer Height:" + height + ")");
                        document.getElementById("pagefootheight").focus();
                        errorFlag = true;
                        break;
                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.LineObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.LineObjects[i].Thickness) > parseFloat(convertTemplateWidth)) {
                        alert("<%=Resources.Template.lineXIllegalFoot%>"  + "(Tempalte Width:" + width + ")");
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                }
            }
        }
        if (!errorFlag) {
            for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.RectangleObjects.length; i++) {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.RectangleObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.RectangleObjects[i].Height) > parseFloat(convertAreaHeight)) {
                    alert("<%=Resources.Template.recYIllegalFoot%>" + "(Page Footer Height:" + height + ")");
                    document.getElementById("pagefootheight").focus();
                    errorFlag = true;
                    break;
                }
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.RectangleObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.RectangleObjects[i].Width) > parseFloat(convertTemplateWidth)) {
                    alert("<%=Resources.Template.recXIllegalFoot%>"  + "(Tempalte Width:" + width + ")");
                    document.getElementById("width").focus();
                    errorFlag = true;
                    break;
                }
            }
        }
        if (!errorFlag) {
            for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.PictureObjects.length; i++) {
                if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.PictureObjects[i].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.PictureObjects[i].Angle == "180")) {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.PictureObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.PictureObjects[i].Height) > parseFloat(convertAreaHeight)) {
                        alert("<%=Resources.Template.picYIllegalFoot%>" + "(Page Footer Height:" + height + ")");
                        document.getElementById("pagefootheight").focus();
                        errorFlag = true;
                        break;
                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.PictureObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.PictureObjects[i].Width) > parseFloat(convertTemplateWidth)) {
                        alert("<%=Resources.Template.picXIllegalFoot%>"  + "(Tempalte Width:" + width + ")");
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                } else {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.PictureObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.PictureObjects[i].Width) > parseFloat(convertAreaHeight)) {
                        alert("<%=Resources.Template.picYIllegalFoot%>" + "(Page Footer Height:" + height + ")");
                        document.getElementById("pagefootheight").focus();
                        errorFlag = true;
                        break;
                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.PictureObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.PictureObjects[i].Height) > parseFloat(convertTemplateWidth)) {
                        alert("<%=Resources.Template.picXIllegalFoot%>"  + "(Tempalte Width:" + width + ")");
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                }
            }
        }
         if (!errorFlag) {
            for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.ArearObjects.length; i++) {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.ArearObjects[i].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.ArearObjects[i].Height) > parseFloat(convertAreaHeight)) {
                    alert("<%=Resources.Template.areaYIllegalFoot%>" + "(Page Footer Height:" + height + ")");
                    document.getElementById("height").focus();
                    errorFlag = true;
                    break;
                }
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.ArearObjects[i].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.ArearObjects[i].Width) > parseFloat(convertTemplateWidth)) {
                    alert("<%=Resources.Template.areaXIllegalFoot%>"  + "(Tempalte Width:" + width + ")");
                    document.getElementById("width").focus();
                    errorFlag = true;
                    break;
                }
            }
        }
        
    }
    return errorFlag;
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkSection
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	���Section����Ŀؼ��Ƿ���Section����֮��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkSection(detail1height, detail2height,width1)
{


    var errorFlag = false;
    var width = parseFloat(width1);
    var dec1Flag = false;
    var headerHeight = 0;
    var rowHeight = 0;
    var decCellWidth = 0;
    
    var convertTemplateWidth = pixelToMm_x(mmToPixel_x(width));
    
         
    for (i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.length; i++) {
        dec1Flag = false;
    
        if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == 0) {
//            height = parseFloat(detail1height);
            
            dec1Flag = true;
        } else {
//            height = parseFloat(detail2height);
           
         }   
        headerHeight = parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderHeight);
        rowHeight = parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].RowHeight);
        
        decCellWidth = Math.round(parseFloat(width1/window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum)*100)/100;
        
        var convertDecCellWidth = pixelToMm_x(getWidthPerColumn(mmToPixel_x(width), window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum))
        var convertHeaderHeight = pixelToMm_y(mmToPixel_y(headerHeight));
        var convertRowHeight = pixelToMm_y(mmToPixel_y(rowHeight));
        //�ȼ��section������header����
////        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.AreaHeight) >= height) {
////            if (dec1Flag) {
////                alert("<%=Resources.Template.headerHeightIllegalSec1%>");
////                document.getElementById("sec1height").focus();
////            } else {
////                alert("<%=Resources.Template.headerHeightIllegalSec2%>");
////                document.getElementById("sec2height").focus();
////            }
////            errorFlag = true;
////              
////          }
        for (j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects.length; j++) {
            
            if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Angle == "180")) {
//                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Height) > parseFloat(convertHeaderHeight)) {
//                    if (dec1Flag) {
//                        alert("<%=Resources.Template.textYIllegalSection1%>" + "(Section 1 Height:" + headerHeight + ")");
//                        document.getElementById("sec1height").focus();
//                    } else {
//                        alert("<%=Resources.Template.textYIllegalSection2%>" + "(Section 2 Height:" + headerHeight + ")");
//                        document.getElementById("sec2height").focus();
//                    }
//                    errorFlag = true;
//                    break;
//                }
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Width) > parseFloat(convertTemplateWidth)) {
                    if (dec1Flag) {
                        alert("<%=Resources.Template.textXIllegaSection1%>" + "(Template Width:" + width + ")");
                    } else {
                        alert("<%=Resources.Template.textXIllegaSection2%>" + "(Template Width:" + width + ")");
                    }
                    document.getElementById("width").focus();
                    errorFlag = true;
                    break;
                }
            } else {
//                 if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Width) > parseFloat(convertHeaderHeight)) {
//                    if (dec1Flag) {
//                        alert("<%=Resources.Template.textYIllegalSection1%>" + "(Section 1 Height:" + headerHeight + ")");
//                        document.getElementById("sec1height").focus();
//                    } else {
//                        alert("<%=Resources.Template.textYIllegalSection2%>" + "(Section 2 Height:" + headerHeight + ")");
//                        document.getElementById("sec2height").focus();
//                    }
//                    errorFlag = true;
//                    break;
//                }
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Height) >  parseFloat(convertTemplateWidth)) {
                    if (dec1Flag) {
                        alert("<%=Resources.Template.textXIllegaSection1%>" + "(Template Width:" + width + ")");
                    } else {
                        alert("<%=Resources.Template.textXIllegaSection2%>" + "(Template Width:" + width + ")");
                    }
                    document.getElementById("width").focus();
                    errorFlag = true;
                    break;
                }
            }
        }
        
        if (!errorFlag) {
            for (j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects.length; j++) {
                if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Angle == "180")) {
//                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Height) > parseFloat(convertHeaderHeight)) {
//                        if (dec1Flag) {
//                            alert("<%=Resources.Template.barcodeYIllegalSection1%>" + "(Section 1 Height:" + headerHeight + ")");
//                            document.getElementById("sec1height").focus();
//                        } else {
//                            alert("<%=Resources.Template.barcodeYIllegalSection2%>" + "(Section 2 Height:" + headerHeight + ")");
//                            document.getElementById("sec2height").focus();
//                        }
//                        errorFlag = true;
//                        break;
//                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Width) >  parseFloat(convertTemplateWidth)) {
                        if (dec1Flag) {
                            alert("<%=Resources.Template.barcodeXIllegalSection1%>" + "Template Width:" + width + ")");
                         
                        } else {
                            alert("<%=Resources.Template.barcodeXIllegalSection2%>" + "Template Width:" + width + ")");
                            
                        }
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                } else {
//                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Width) > parseFloat(convertHeaderHeight)) {
//                        if (dec1Flag) {
//                            alert("<%=Resources.Template.barcodeYIllegalSection1%>" + "(Section 1 Height:" + headerHeight + ")");
//                            document.getElementById("sec1height").focus();
//                        } else {
//                            alert("<%=Resources.Template.barcodeYIllegalSection2%>" + "(Section 2 Height:" + headerHeight + ")");
//                            document.getElementById("sec2height").focus();
//                        }
//                        errorFlag = true;
//                        break;
//                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Height) >  parseFloat(convertTemplateWidth)) {
                        if (dec1Flag) {
                            alert("<%=Resources.Template.barcodeXIllegalSection1%>" + "Template Width:" + width + ")");
                         
                        } else {
                            alert("<%=Resources.Template.barcodeXIllegalSection2%>" + "Template Width:" + width + ")");
                            
                        }
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                }
            }
        }
        
         if (!errorFlag) {
            for (j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects.length; j++) {
                if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Angle == "180")) {
//                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Thickness) > parseFloat(convertHeaderHeight)) {
//                        if (dec1Flag) {
//                            alert("<%=Resources.Template.lineYIllegalSection1%>" + "(Section 1 Height:" + headerHeight + ")");
//                            document.getElementById("sec1height").focus();
//                        } else {
//                            alert("<%=Resources.Template.lineYIllegalSection2%>" + "(Section 2 Height:" + headerHeight + ")");
//                            document.getElementById("sec2height").focus();
//                        }
//                        errorFlag = true;
//                        break;
//                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Length) >  parseFloat(convertTemplateWidth)) {
                        if (dec1Flag) {
                            alert("<%=Resources.Template.lineXIllegalSection1%>" + "Template Width:" + width + ")");
                         
                        } else {
                            alert("<%=Resources.Template.lineXIllegalSection2%>" + "Template Width:" + width + ")");
                            
                        }
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                } else {
//                     if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Length) > parseFloat(convertHeaderHeight)) {
//                        if (dec1Flag) {
//                            alert("<%=Resources.Template.lineYIllegalSection1%>" + "(Section 1 Height:" + headerHeight + ")");
//                            document.getElementById("sec1height").focus();
//                        } else {
//                            alert("<%=Resources.Template.lineYIllegalSection2%>" + "(Section 2 Height:" + headerHeight + ")");
//                            document.getElementById("sec2height").focus();
//                        }
//                        errorFlag = true;
//                        break;
//                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Thickness) >  parseFloat(convertTemplateWidth)) {
                        if (dec1Flag) {
                            alert("<%=Resources.Template.lineXIllegalSection1%>" + "Template Width:" + width + ")");
                         
                        } else {
                            alert("<%=Resources.Template.lineXIllegalSection2%>" + "Template Width:" + width + ")");
                            
                        }
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                    
                }
            }
        }
        
        if (!errorFlag) {
            for (j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.RectangleObjects.length; j++) {
//                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.RectangleObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.RectangleObjects[j].Height) > parseFloat(convertHeaderHeight)) {
//                    if (dec1Flag) {
//                        alert("<%=Resources.Template.recYIllegalSection1%>" + "(Section 1 Height:" + headerHeight + ")");
//                        document.getElementById("sec1height").focus();
//                    } else {
//                        alert("<%=Resources.Template.recYIllegalSection2%>" + "(Section 2 Height:" + headerHeight + ")");
//                        document.getElementById("sec2height").focus();
//                    }
//                    errorFlag = true;
//                    break;
//                }
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.RectangleObjects[j].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.RectangleObjects[j].Width) >  parseFloat(convertTemplateWidth)) {
                    if (dec1Flag) {
                        alert("<%=Resources.Template.recXIllegalSection1%>" + "(Template Width:" + width + ")");
                    } else {
                        alert("<%=Resources.Template.recXIllegalSection2%>" + "(Template Width:" + width + ")");
                    }
                    document.getElementById("width").focus();
                    errorFlag = true;
                    break;
                }
                
            }
        }
       
        if (!errorFlag) {
            for (j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects.length; j++) {
                if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Angle == "180")) {
//                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Y) +  parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Height) > parseFloat(convertHeaderHeight)) {
//                        if (dec1Flag) {
//                            alert("<%=Resources.Template.picYIllegalSection1%>" + "(Section 1 Height:" + headerHeight + ")");
//                            document.getElementById("sec1height").focus();
//                        } else {
//                            alert("<%=Resources.Template.picYIllegalSection2%>" + "(Section 2 Height:" + headerHeight + ")");
//                            document.getElementById("sec2height").focus();
//                        }
//                        errorFlag = true;
//                        break;
//                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Width) >  parseFloat(convertTemplateWidth)) {
                        if (dec1Flag) {
                            alert("<%=Resources.Template.picXIllegalSection1%>" + "(Template Width:" + width + ")");
                        } else {
                            alert("<%=Resources.Template.picXIllegalSection2%>" + "(Template Width:" + width + ")");
                        }
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                } else {
//                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Y) +  parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Width) > parseFloat(convertHeaderHeight)) {
//                        if (dec1Flag) {
//                            alert("<%=Resources.Template.picYIllegalSection1%>" + "(Section 1 Height:" + headerHeight + ")");
//                            document.getElementById("sec1height").focus();
//                        } else {
//                            alert("<%=Resources.Template.picYIllegalSection2%>" + "(Section 2 Height:" + headerHeight + ")");
//                            document.getElementById("sec2height").focus();
//                        }
//                        errorFlag = true;
//                        break;
//                    }
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Height) >  parseFloat(convertTemplateWidth)) {
                        if (dec1Flag) {
                            alert("<%=Resources.Template.picXIllegalSection1%>" + "(Template Width:" + width + ")");
                        } else {
                            alert("<%=Resources.Template.picXIllegalSection2%>" + "(Template Width:" + width + ")");
                        }
                        document.getElementById("width").focus();
                        errorFlag = true;
                        break;
                    }
                }
                
            }
        }
    
        if (!errorFlag) {
            for (j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.ArearObjects.length; j++) {
//                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.ArearObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.ArearObjects[j].Height) > parseFloat(convertHeaderHeight)) {
//                    if (dec1Flag) {
//                        alert("<%=Resources.Template.areaYIllegalSection1%>" + "(Section 1 Height:" + headerHeight + ")");
//                        document.getElementById("sec1height").focus();
//                    } else {
//                        alert("<%=Resources.Template.areaYIllegalSection2%>" + "(Section 2 Height:" + headerHeight + ")");
//                        document.getElementById("sec2height").focus();
//                    }
//                    errorFlag = true;
//                    break;
//                }
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.ArearObjects[j].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.ArearObjects[j].Width) >  parseFloat(convertTemplateWidth)) {
                    if (dec1Flag) {
                        alert("<%=Resources.Template.areaXIllegalSection1%>" + "(Template Width:" + width + ")");
                    } else {
                        alert("<%=Resources.Template.areaXIllegalSection2%>" + "(Template Width:" + width + ")");
                    }
                    document.getElementById("width").focus();
                    errorFlag = true;
                    break;
                }
                
            }
        }
        
    
        //�ټ��section������detail����
        
       
//        if (!errorFlag) {
//            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].RowHeight) >= height) {
//                if (dec1Flag) {
//                    alert("<%=Resources.Template.rowHeightIllegalSec1%>");
//                    document.getElementById("sec1height").focus();
//                } else {
//                    alert("<%=Resources.Template.rowHeightIllegalSec2%>");
//                    document.getElementById("sec2height").focus();
//                }
//                errorFlag = true;
//              
//            }
//        }
        if (!errorFlag) {
            for (j=0; j<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells.length; j++) {
                for (l=0; l<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects.length; l++) 
                {
                    if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Angle == "180")) {
//                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Height) > parseFloat(convertRowHeight)) {
//                            if (dec1Flag) {
//                                alert("<%=Resources.Template.textYIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Height:" + rowHeight + ")");
//                                document.getElementById("sec1height").focus();
//                            } else {
//                                alert("<%=Resources.Template.textYIllegalCellSec2%>" + "(Cell:" + j + ")"  + " (Section 2 Height:" + rowHeight + ")");
//                                document.getElementById("sec2height").focus();
//                            }
//                            errorFlag = true;
//                            break;
//                        }
                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Width) > parseFloat(convertDecCellWidth)) {
                            if (dec1Flag) {
                                alert("<%=Resources.Template.textXIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Cell Width:" + convertDecCellWidth + ")");
                            } else {
                                alert("<%=Resources.Template.textXIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Cell Width:" + convertDecCellWidth + ")");
                            }
                            document.getElementById("width").focus();
                            errorFlag = true;
                            break;
                        }  
                     } else {
//                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Width) > parseFloat(convertRowHeight)) {
//                            if (dec1Flag) {
//                                alert("<%=Resources.Template.textYIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Height:" + rowHeight + ")");
//                                document.getElementById("sec1height").focus();
//                            } else {
//                                alert("<%=Resources.Template.textYIllegalCellSec2%>" + "(Cell:" + j + ")"  + " (Section 2 Height:" + rowHeight + ")");
//                                document.getElementById("sec2height").focus();
//                            }
//                            errorFlag = true;
//                            break;
//                        }
                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Height) > parseFloat(convertDecCellWidth)) {
                            if (dec1Flag) {
                                alert("<%=Resources.Template.textXIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Cell Width:" + convertDecCellWidth + ")");
                            } else {
                                alert("<%=Resources.Template.textXIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Cell Width:" + convertDecCellWidth + ")");
                            }
                            document.getElementById("width").focus();
                            errorFlag = true;
                            break;
                        }  
                     }  
                }
                
                if (!errorFlag) {
                    for (l=0; l<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects.length; l++) {
                        if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Angle == "180")) {
//                            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Height) > parseFloat(convertRowHeight)) {
//                                if (dec1Flag) {
//                                    alert("<%=Resources.Template.barcodeYIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Height:" + rowHeight + ")");
//                                    document.getElementById("sec1height").focus();
//                                } else {
//                                    alert("<%=Resources.Template.barcodeYIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Height:" + rowHeight + ")");
//                                    document.getElementById("sec2height").focus();
//                                }
//                                errorFlag = true;
//                                break;
//                            }
                            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Width) > parseFloat(convertDecCellWidth)) {
                                if (dec1Flag) {
                                    alert("<%=Resources.Template.barcodeXIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Cell Width:" + convertDecCellWidth + ")");
                                } else {
                                    alert("<%=Resources.Template.barcodeXIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Cell Width:" + convertDecCellWidth + ")");
                                }
                                document.getElementById("width").focus();
                                errorFlag = true;
                                break;
                            } 
                        } else {
//                             if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Width) > parseFloat(convertRowHeight)) {
//                                if (dec1Flag) {
//                                    alert("<%=Resources.Template.barcodeYIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Height:" + rowHeight + ")");
//                                    document.getElementById("sec1height").focus();
//                                } else {
//                                    alert("<%=Resources.Template.barcodeYIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Height:" + rowHeight + ")");
//                                    document.getElementById("sec2height").focus();
//                                }
//                                errorFlag = true;
//                                break;
//                            }
                            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Height) > parseFloat(convertDecCellWidth)) {
                                if (dec1Flag) {
                                    alert("<%=Resources.Template.barcodeXIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Cell Width:" + convertDecCellWidth + ")");
                                } else {
                                    alert("<%=Resources.Template.barcodeXIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Cell Width:" + convertDecCellWidth + ")");
                                }
                                document.getElementById("width").focus();
                                errorFlag = true;
                                break;
                            } 
                        }
                    }   
                }
                
                if (!errorFlag) {
                    for (l=0; l<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects.length; l++) {
                        if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Angle == "180")) {
//                            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Y)  + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Thickness) > parseFloat(convertRowHeight)) {
//                                if (dec1Flag) {
//                                    alert("<%=Resources.Template.lineYIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Height:" + rowHeight + ")");
//                                  document.getElementById("sec1height").focus();
//                                } else {  
//                                    alert("<%=Resources.Template.lineYIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Height:" + rowHeight + ")");
//                                    document.getElementById("sec2height").focus();
//                                }
//                                errorFlag = true;
//                                break;
//                            }
                            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Length) > parseFloat(convertDecCellWidth)) {
                                if (dec1Flag) {
                                    alert("<%=Resources.Template.lineXIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Cell Width:" + convertDecCellWidth + ")");
                                } else {
                                    alert("<%=Resources.Template.lineXIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Cell Width:" + convertDecCellWidth + ")");
                                }
                                document.getElementById("width").focus();
                                errorFlag = true;
                                break;
                            }
                        } else {
//                            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Y)  + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Length) > parseFloat(convertRowHeight)) {
//                                if (dec1Flag) {
//                                    alert("<%=Resources.Template.lineYIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Height:" + rowHeight + ")");
//                                  document.getElementById("sec1height").focus();
//                                } else {  
//                                    alert("<%=Resources.Template.lineYIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Height:" + rowHeight + ")");
//                                    document.getElementById("sec2height").focus();
//                                }
//                                errorFlag = true;
//                                break;
//                            }
                            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Thickness) > parseFloat(convertDecCellWidth)) {
                                if (dec1Flag) {
                                    alert("<%=Resources.Template.lineXIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Cell Width:" + convertDecCellWidth + ")");
                                } else {
                                    alert("<%=Resources.Template.lineXIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Cell Width:" + convertDecCellWidth + ")");
                                }
                                document.getElementById("width").focus();
                                errorFlag = true;
                                break;
                            }
                        
                        }
                    }    
                }
                
                if (!errorFlag) {
                    for (l=0; l<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].RectangleObjects.length; l++) {
//                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].RectangleObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].RectangleObjects[l].Height) > parseFloat(convertRowHeight)) {
//                            if (dec1Flag) {
//                                alert("<%=Resources.Template.recYIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Height:" + rowHeight + ")");
//                                document.getElementById("sec1height").focus();
//                            } else {
//                                alert("<%=Resources.Template.recYIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Height:" + rowHeight + ")");
//                                document.getElementById("sec2height").focus();
//                            }
//                            errorFlag = true;
//                            break;
//                        }
                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].RectangleObjects[l].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].RectangleObjects[l].Width) > parseFloat(convertDecCellWidth)) {
                            if (dec1Flag) {
                                alert("<%=Resources.Template.recXIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Cell Width:" + convertDecCellWidth + ")");
                            } else {
                                alert("<%=Resources.Template.recXIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Cell Width:" + convertDecCellWidth + ")");
                            }
                            document.getElementById("width").focus();
                            errorFlag = true;
                            break;
                        }    
                    }
                }
                
                if (!errorFlag) {
                    for (l=0; l<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects.length; l++) {
                        if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Angle == "180")) {
//                            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Height) > parseFloat(convertRowHeight)) {
//                                if (dec1Flag) {
//                                    alert("<%=Resources.Template.picYIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Height:" + rowHeight + ")");
//                                    document.getElementById("sec1height").focus();
//                                } else {
//                                    alert("<%=Resources.Template.picYIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Height:" + rowHeight + ")");
//                                    document.getElementById("sec2height").focus();
//                                }
//                                errorFlag = true;
//                                break;
//                            }
                            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].X)  + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Width) > parseFloat(convertDecCellWidth)) {
                                if (dec1Flag) {
                                    alert("<%=Resources.Template.picXIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Cell Width:" + convertDecCellWidth + ")");
                                } else {
                                    alert("<%=Resources.Template.picXIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Cell Width:" + convertDecCellWidth + ")");
                                }
                                document.getElementById("width").focus();
                                errorFlag = true;
                                break;
                            }
                        } else {
//                             if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Width) > parseFloat(convertRowHeight)) {
//                                if (dec1Flag) {
//                                    alert("<%=Resources.Template.picYIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Height:" + rowHeight + ")");
//                                    document.getElementById("sec1height").focus();
//                                } else {
//                                    alert("<%=Resources.Template.picYIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Height:" + rowHeight + ")");
//                                    document.getElementById("sec2height").focus();
//                                }
//                                errorFlag = true;
//                                break;
//                            }
                            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].X)  + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Height) > parseFloat(convertDecCellWidth)) {
                                if (dec1Flag) {
                                    alert("<%=Resources.Template.picXIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Cell Width:" + convertDecCellWidth + ")");
                                } else {
                                    alert("<%=Resources.Template.picXIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Cell Width:" + convertDecCellWidth + ")");
                                }
                                document.getElementById("width").focus();
                                errorFlag = true;
                                break;
                            }
                        }    
                    }
                }
                
                 if (!errorFlag) {
                    for (l=0; l<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].ArearObjects.length; l++) {
//                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].ArearObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].ArearObjects[l].Height) > parseFloat(convertRowHeight)) {
//                            if (dec1Flag) {
//                                alert("<%=Resources.Template.areaYIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Height:" + rowHeight + ")");
//                                document.getElementById("sec1height").focus();
//                            } else {
//                                alert("<%=Resources.Template.areaYIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Height:" + rowHeight + ")");
//                                document.getElementById("sec2height").focus();
//                            }
//                            errorFlag = true;
//                            break;
//                        }
                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].ArearObjects[l].X) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].ArearObjects[l].Width)  > parseFloat(convertDecCellWidth)) {
                            if (dec1Flag) {
                                alert("<%=Resources.Template.areaXIllegalCellSec1%>" + "(Cell:" + j + ")" + " (Section 1 Cell Width:" + convertDecCellWidth + ")");
                            } else {
                                alert("<%=Resources.Template.areaXIllegalCellSec2%>" + "(Cell:" + j + ")" + " (Section 2 Cell Width:" + convertDecCellWidth + ")");
                            }
                            document.getElementById("width").focus();
                            errorFlag = true;
                            break;
                        }    
                    }
                }
                if (errorFlag) {
                    break;
                }
            }//end for cell
           
        
        }//end if
        if (errorFlag) {
            break;
        }
 
    }//end for section  
   
    return errorFlag;
       
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	exitPage
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�˳���ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function setButtonStatus()
{
    setTimeout(setButtonStatus1, 100);
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	setButtonStatus1
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�˳���ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function setButtonStatus1()
{
   
  
    var sec1height = trimString(document.getElementById("sec1height").value);
    var sec2height = trimString(document.getElementById("sec2height").value);
    var totalHeight = 0;
    
    if ((sec1height != "") && (checkPosition(sec1height))) {
        totalHeight = totalHeight + parseFloat(sec1height);
       
        
    } 
    if ((sec2height != "") && (checkPosition(sec2height))) {
        totalHeight = totalHeight + parseFloat(sec2height);
      
        
    } 
    if (totalHeight == 0) {
        window.parent.document.getElementById("nextBtn").disabled = true;
        window.parent.document.getElementById("finishBtn").disabled = false;
    } else {
         window.parent.document.getElementById("nextBtn").disabled = false;
         window.parent.document.getElementById("finishBtn").disabled = true;
    }
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	exitPage
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�˳���ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function exitPage()
{


    var errorFlag = false;
    
    var width = trimString(document.getElementById("width").value);
    var height = trimString(document.getElementById("height").value);
    var pageheaderheight = trimString(document.getElementById("pageheaderheight").value);
    var pagefootheight = trimString(document.getElementById("pagefootheight").value);
    var sec1height = trimString(document.getElementById("sec1height").value);
    var sec2height = trimString(document.getElementById("sec2height").value);
    var totalHeight = 0;
    var totalDetailHeight = 0;
    var detail1Height = 0;
    var detail2Height = 0;
    var headerheight = 0;
    var footheight = 0;
    var bottomMargin = trimString(document.getElementById("infoHeight").value);
    
    
        //�ж�����ĺϷ���
        if (!check1DecimalNotZero(width)) {
            alert("<%=Resources.Template.oneDecimalFormat%>");
            errorFlag = true;
             document.getElementById("width").focus();
        } else if (parseFloat(width) < parseFloat("<%=Constants.AREA_LIMITED_WIDTH%>")) {
            alert("<%=Resources.Template.areaMinWidth%>" + "(Minimal Width:" + "<%=Constants.AREA_LIMITED_WIDTH%>" + ")");
            errorFlag = true;
            document.getElementById("width").focus();
        } else if (!check1DecimalNotZero(height)) {
            alert("<%=Resources.Template.oneDecimalFormat%>");
            errorFlag = true;
             document.getElementById("height").focus();
        } else if (parseFloat(height) < parseFloat("<%=Constants.AREA_LIMITED_HEIGHT%>") ){
            alert("<%=Resources.Template.areaMinHeight%>" + "(Minimal Height:" + "<%=Constants.AREA_LIMITED_HEIGHT%>" + ")");
            errorFlag = true;
            document.getElementById("height").focus();
        } else if (!checkPosition(pageheaderheight)) {
            alert("<%=Resources.Template.positionFormat%>");
            errorFlag = true;
            document.getElementById("pageheaderheight").focus();
        } else if ((parseFloat(pageheaderheight) > 0) && (parseFloat(pageheaderheight) < parseFloat("<%=Constants.AREA_LIMITED_HEIGHT%>"))) {
            alert("<%=Resources.Template.areaMinHeight%>" + "(Minimal Height:" + "<%=Constants.AREA_LIMITED_HEIGHT%>" + ")");
            errorFlag = true;
            document.getElementById("pageheaderheight").focus();
        } else if (!checkPosition(pagefootheight)) {
            alert("<%=Resources.Template.positionFormat%>");
            errorFlag = true;
            document.getElementById("pagefootheight").focus();
        } else if ((parseFloat(pagefootheight) > 0) && (parseFloat(pagefootheight) < parseFloat("<%=Constants.AREA_LIMITED_HEIGHT%>"))) {
            alert("<%=Resources.Template.areaMinHeight%>" + "(Minimal Height:" + "<%=Constants.AREA_LIMITED_HEIGHT%>" + ")");
            errorFlag = true;
            document.getElementById("pagefootheight").focus();
        } else if (!checkPosition(sec1height)) {
            alert("<%=Resources.Template.positionFormat%>");
            errorFlag = true;
            document.getElementById("sec1height").focus();
        } else if ((parseFloat(sec1height) > 0) && (parseFloat(sec1height) < parseFloat("<%=Constants.AREA_LIMITED_HEIGHT%>"))) {
            alert("<%=Resources.Template.areaMinHeight%>" + "(Minimal Height:" + "<%=Constants.AREA_LIMITED_HEIGHT%>" + ")");
            errorFlag = true;
            document.getElementById("sec1height").focus();
        } else if (!checkPosition(sec2height)) {
            alert("<%=Resources.Template.positionFormat%>");
            errorFlag = true;
            document.getElementById("sec2height").focus();
        } else if ((parseFloat(sec2height) > 0) && (parseFloat(sec2height) < parseFloat("<%=Constants.AREA_LIMITED_HEIGHT%>"))) {
            alert("<%=Resources.Template.areaMinHeight%>" + "(Minimal Height:" + "<%=Constants.AREA_LIMITED_HEIGHT%>" + ")");
            errorFlag = true;
            document.getElementById("sec2height").focus();
        } else if (document.getElementById("pageFootCheck").checked && !check1DecimalNotZero(bottomMargin)) {
            alert("<%=Resources.Template.oneDecimalFormat%>");
            errorFlag = true;
             document.getElementById("infoHeight").focus();
        } else if (document.getElementById("pageFootCheck").checked && (parseFloat(bottomMargin) > parseFloat(height))) {
            alert("<%=Resources.Template.areaMinWidth%>" + "(Minimal Height:" + "<%=Constants.AREA_LIMITED_WIDTH%>" + ")");
            errorFlag = true;
            document.getElementById("infoHeight").focus();
        } else {
           
             if (pageheaderheight != "") {
                headerheight = parseFloat(pageheaderheight);
                totalHeight = totalHeight + parseFloat(pageheaderheight);
               
             }
              if (pagefootheight != "") {
                footheight =  parseFloat(pagefootheight);
                totalHeight = totalHeight + parseFloat(pagefootheight);
             }
             if (sec1height != "") {
                totalHeight = totalHeight + parseFloat(sec1height);
                totalDetailHeight = totalDetailHeight + parseFloat(sec1height);
                detail1Height = parseFloat(sec1height);
             }
              if (sec2height != "") {
                totalHeight = totalHeight + parseFloat(sec2height);
                totalDetailHeight = totalDetailHeight + parseFloat(sec2height);
                detail2Height = parseFloat(sec2height);
             }
            
             if (totalHeight == 0) {
             
                alert("<%=Resources.Template.chooseArea%>");
                errorFlag = true;
             }  else if (totalHeight < parseFloat("<%=Constants.AREA_LIMITED_HEIGHT%>")) {
                 alert("<%=Resources.Template.areaTotalMin%>");
                 errorFlag = true;
                document.getElementById("height").focus();
             }else if (totalHeight != parseFloat(height)) {
                 alert("<%=Resources.Template.heightConflic%>");
                 errorFlag = true;
                document.getElementById("height").focus();
             }
        }
    
        if (!errorFlag) {
            errorFlag = checkPageHeader(pageheaderheight, width);
        }
        if (!errorFlag) {
            errorFlag = checkPageFooter(pagefootheight, width);
        }

         if (!errorFlag) {
            errorFlag = checkSection(detail1Height, detail2Height,width);
        }
   
    if (!errorFlag) {
  
    
       //��ҳ����Ϣ�����ڿͻ��˵Ľṹ��
        var section;
        var i;
        var bFindFlag = false;
        

        window.parent.templateXmlAndHtml.PrintTemplateInfo.TemplateWidth = width;
        window.parent.templateXmlAndHtml.PrintTemplateInfo.TemplateHeight = height; 
        
        window.parent.templateXmlAndHtml.PrintTemplateInfo.PageHeader.AreaHeight = headerheight + "";
        window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooter.AreaHeight = footheight + "";
 
        //detail1�и߶�
        if (detail1Height != 0) {

             for (i = 0 ; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.length; i++) {
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == 0) {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight = detail1Height + "";
                    break;
                }
             }
             
       } else {
            
             //���ԭ��Cell�ж����Ӧ��dataobject
             clearDataObject(0);
             for (i = 0 ; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.length; i++) {
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == 0) {
//                     window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.splice(i,1);
                     var rtn = com.inventec.template.manager.TemplateManager.getSectionFromStructure();
                     if (rtn.error != null) {
                        alert(rtn.error.Message);
                     } else {
                        var section = rtn.value;
                        section.Index = 0;
                        section.AreaHeight = 0 + "";
                        window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i]= section;
                                   
                     }
                     break;
                        
                }
             }
            
       }
       
       
      
        //detail2�и߶�
        if (detail2Height != 0) {
            
             for (i = 0 ; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.length; i++) {
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == 1) {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight = detail2Height + "";
                    break;
                }
             }   
        } else {
            //���ԭ��Cell�ж����Ӧ��dataobject
            clearDataObject(1);
            for (i = 0 ; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.length; i++) {
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == 1) {
//                     window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.splice(i,1);
                     var rtn = com.inventec.template.manager.TemplateManager.getSectionFromStructure();
                     if (rtn.error != null) {
                        alert(rtn.error.Message);
                     } else {
                        var section = rtn.value;
                        section.Index = 1;
                        section.AreaHeight = 0 + "";
                        window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i]= section;
                                   
                     }
                     break;
                        
                }
             }
       }             
     
//    <bug>
//        BUG NO:ITC-992-0083
//        REASON:δѡ��ʱ��Ӧ�ÿ�ֵ
//    </bug>
       if ( document.getElementById("pageFootCheck").checked) {
           window.parent.templateXmlAndHtml.PrintTemplateInfo.PrintPageFooter = "1";
           window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooterHeightOffset = bottomMargin;
       } else {
           window.parent.templateXmlAndHtml.PrintTemplateInfo.PrintPageFooter = "";
           window.parent.templateXmlAndHtml.PrintTemplateInfo.PageFooterHeightOffset = "";
       }
   }

   return errorFlag;
    
}


function clearDataObject(index)
{
    
           
    for (i=0; i<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[index].Cells.length; i++) {
        for (j=0; j<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[index].Cells[i].TextObjects.length; j++) {
            for (k=0 ;k<window.parent.templateXmlAndHtml.PrintTemplateInfo.DataObjects.length; k++) {
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[index].Cells[i].TextObjects[j].ObjectName == window.parent.templateXmlAndHtml.PrintTemplateInfo.DataObjects[k].ObjectName){
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DataObjects.splice(k,1);
                    break;
                }
            }
           
        }
        
        for (j=0; j<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[index].Cells[i].BarcodeObjects.length; j++) {
            for (k=0 ;k<window.parent.templateXmlAndHtml.PrintTemplateInfo.DataObjects.length; k++) {
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[index].Cells[i].BarcodeObjects[j].ObjectName == window.parent.templateXmlAndHtml.PrintTemplateInfo.DataObjects[k].ObjectName){
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DataObjects.splice(k,1);
                    break;
                }
            }
           
        }
        
         for (j=0; j<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[index].Cells[i].PictureObjects.length; j++) {
            for (k=0 ;k<window.parent.templateXmlAndHtml.PrintTemplateInfo.DataObjects.length; k++) {
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[index].Cells[i].PictureObjects[j].ObjectName == window.parent.templateXmlAndHtml.PrintTemplateInfo.DataObjects[k].ObjectName){
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DataObjects.splice(k,1);
                    break;
                }
            }
           
        }
    }
    
    
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	exitPrePage
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�˳���ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function exitPrePage()
{
    

    var errorFlag = false;
    
    var width = trimString(document.getElementById("width").value);
    var height = trimString(document.getElementById("height").value);
    var pageheaderheight = trimString(document.getElementById("pageheaderheight").value);
    var pagefootheight = trimString(document.getElementById("pagefootheight").value);
    var sec1height = trimString(document.getElementById("sec1height").value);
    var sec2height = trimString(document.getElementById("sec2height").value);
    
    
    
  
    
       //��ҳ����Ϣ�����ڿͻ��˵Ľṹ��
        window.parent.width = width;
        window.parent.height = height;
        window.parent.headerheight = pageheaderheight;
        window.parent.footerheight = pagefootheight;
        window.parent.detail1height = sec1height;
        window.parent.detail2height = sec2height;
         
        if ( document.getElementById("pageFootCheck").checked) {
          window.parent.printpagefooter = "1";
          window.parent.infoheight = trimString(document.getElementById("infoHeight").value);
       } else {
           window.parent.printpagefooter = "";
           window.parent.infoheight = "";
       }

 
    
}
//-->
</SCRIPT>
</html>



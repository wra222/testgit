

<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: ģ���趨��ҳ��4
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)  create
' Known issues:Any restrictions about this file
--%> 
<%@ page language="C#" autoeventwireup="true" inherits="webroot_axpx_template_TemplateSetting4, App_Web_templatesetting4.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Template Setting-Step 4</title>
 <script type="text/javascript" src="../../commoncontrol/btnTabs.js"></script>
 <style>
    .tabcontent 
    {
        border:rgb(147,191,218) 0.04cm solid;
        height:200px;
       
    }
  
 </style>
</head>
<%--load all js files--%>
<fis:header id="header1" runat="server"/>

<body class="bgBody" onload="initPage()" >
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
   <DIV class="wizardTip"   >
        <span style="font-weight: bold"><%=Resources.Template.templateSetting%></span><br>
        <span style="margin-left:50px"><%=Resources.Template.templateSettingTitle4%></span><br><br>
    </DIV>
    
     <DIV class="wizardContent" >
     
     <div id="con" ></div>
    
	 <div id="div1"  class="propertyDialogContent" >
	    
		
	        <TABLE width="100%"  border="0" cellpadding="0" cellspacing="5" >
	        <TR>
		        <TD width="50%" class="inputTitle"><%=Resources.Template.secHeaderHeight%>:</TD>
		        <TD><INPUT TYPE="text" NAME="sec1headerheight" id="sec1headerheight" class="inputStyle">mm</TD>
	        </TR>
	        <TR>
		        <TD class="inputTitle"><%=Resources.Template.secRowHeight%>:</TD>
		        <TD><INPUT TYPE="text" NAME="sec1rowheight" id="sec1rowheight" class="inputStyle">mm</TD>
	        </TR>
	        <TR colspan="2">
		       
		        <TD><INPUT TYPE="checkbox" NAME="sec1reset" id="sec1reset" onclick="sec1Reset()"><%=Resources.Template.reset%></TD>
	        </TR>
	        <TR>
		        <TD class="inputTitle"><%=Resources.Template.secCols%>:</TD>
		        <TD><INPUT TYPE="text" NAME="sec1horizon" id="sec1horizon" class="inputStyle"></TD>
	        </TR>
	        
	        <TR>
		        <TD colspan="2"><font class="tipFont1"><%=Resources.Template.sectionTip1%>1</font></TD>
	        </TR>
	        <TR>
		        <TD class="inputTitle"><%=Resources.Template.secDataSet%>:</TD>
		        <TD><INPUT TYPE="text" NAME="sec1dataset" id="sec1dataset" class="inputStyle"></TD>
	        </TR>
	        <TR>
		        <TD colspan="2"><font class="tipFont1"><%=Resources.Template.sectionTip2%></font></TD>
	        </TR>
	        
	        <TR>
		        <TD colspan="2"><INPUT TYPE="checkbox" NAME="sec1fixedheight" ID="sec1fixedheight"><%=Resources.Template.secFixed%></TD>
	        </TR>
            </TABLE>
	
	</div>
	 
	<div id="div2" style="display:none;" class="propertyDialogContent">
		
	        <TABLE width="100%"  border="0" cellpadding="0" cellspacing="5">
	        <TR>
		        <TD width="50%" class="inputTitle"><%=Resources.Template.secHeaderHeight%>:</TD>
		        <TD><INPUT TYPE="text" NAME="sec2headerheight" id="sec2headerheight" class="inputStyle">mm</TD>
	        </TR>
	        <TR>
		        <TD class="inputTitle"><%=Resources.Template.secRowHeight%>:</TD>
		        <TD><INPUT TYPE="text" NAME="sec2rowheight" id="sec2rowheight" class="inputStyle">mm</TD>
	        </TR>
	        <TR colspan="2">
		       
		        <TD><INPUT TYPE="checkbox" NAME="sec2reset" id="sec2reset" onclick="sec2Reset()"><%=Resources.Template.reset%></TD>
	        </TR>
	        <TR>
		        <TD class="inputTitle"><%=Resources.Template.secCols%>:</TD>
		        <TD><INPUT TYPE="text" NAME="sec2horizon" id="sec2horizon" class="inputStyle"></TD>
	        </TR>
	        <TR>
		        <TD colspan="2"><font class="tipFont1"><%=Resources.Template.sectionTip1%></font></TD>
	        </TR>
	        <TR>
		        <TD class="inputTitle"><%=Resources.Template.secDataSet%>:</TD>
		        <TD><INPUT TYPE="text" NAME="sec2dataset" id="sec2dataset" class="inputStyle"></TD>
	        </TR>
	        <TR>
		        <TD colspan="2"><font class="tipFont1"><%=Resources.Template.sectionTip2%></font></TD>
	        </TR>
	        
	        <TR>
		        <TD colspan="2"><INPUT TYPE="checkbox" NAME="sec2fixedheight" ID="sec2fixedheight"><%=Resources.Template.secFixed%></TD>
	        </TR>
            </TABLE>
	   
	    
	</div>
	 </DIV >
</body>

<%--set style for readonly input controller--%>
<fis:footer id="footer1" runat="server"/> 

<SCRIPT LANGUAGE="JavaScript">
<!--

//��������,���ڴ�ſ�cell��index��
var blankCellIndex=new Array();
var pageIndex = 0;
var sec1BeginFlag = true;
var sec2BeginFlag = true;
var sec1Horizon;
var sec1Dataset;
var sec2Horizon;
var sec2Dataset;
var pixel_per_inch_x;
var pixel_per_inch_y;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	createBtnTabs
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����Tab��ǩ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function createBtnTabs()
{
    tabs = new clsButtonTabs("tabs");

    tabs.preSelectChanged = preCallback;
	tabs.selectChanged = callback;
		 
	for (var i=0; i<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.length; i++)
	{
	    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight) > 0) 
	    {
	        var temp = new clsButton("tabs");
		    temp.normalPic = "../../images/section"+ window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index +"-1.jpg";
		    temp.selPic = "../../images/section"+ window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index +"-2.jpg";
		    temp.disablePic = "../../images/section"+ window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index +"-3.jpg";
    		tabs.addButton(temp);
    		
		}	
	}
	
	for (var i=0; i<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.length; i++)
	{
	    if ((parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight) > 0) && (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == 0))
	    {
	          pageIndex = 0;
		      document.getElementById("div1").style.display = "";
		 	  document.getElementById("div2").style.display = "none"; 
		 	  break;  
	    } else {
	        pageIndex = 1;
		    document.getElementById("div2").style.display = "";
		 	document.getElementById("div1").style.display = "none";
		   break;
	    }
		      
     }    
        
	
	
	// tabs.diableTab(0,true);
	con.innerHTML = tabs.toString();
	tabs.initSelect(0);
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	preCallback
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�뿪Tabҳ����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~   
function preCallback()
{
    var ret = true;
    switch(pageIndex) {
        case 0: 
            ret = !judgeLegal(0); 
		    break;
	    case 1: 
            ret = !judgeLegal(1); 
		    break;
         
        default:
            break;	
	}
     
	return ret;
}
	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	callback
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�л�Tabҳ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  
function callback(index)
{  
    pageIndex = index;

    switch(index){
	    case 0: 
     	 	document.getElementById("div1").style.display = "";
		 	document.getElementById("div2").style.display = "none";
		    break;
	    case 1: 
     	    document.getElementById("div2").style.display = "";
		 	document.getElementById("div1").style.display = "none";
		    break;
     
        default:
            break;	
    }
    return true;
}
	
	
	

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
    if (!window.parent.framePreFlag[3]) {
        for (var i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.length; i++) {
           
            if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == 0) && 
                (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight) > 0))
                {
            
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderHeight != "") {
                    document.getElementById("sec1headerheight").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderHeight;
                } else {
                    document.getElementById("sec1headerheight").value = "<%=Constants.AREA_LIMITED_HEIGHT%>";
                }
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].RowHeight != "") {
                    document.getElementById("sec1rowheight").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].RowHeight;
                } else {
                    document.getElementById("sec1rowheight").value = "<%=Constants.AREA_LIMITED_HEIGHT%>";
                }
             
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum != "") {
                    sec1BeginFlag = false;
                    document.getElementById("sec1horizon").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum;
                } else {
                    document.getElementById("sec1horizon").value = "1";
                }
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum != "") {
                    document.getElementById("sec1dataset").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum;
                } else {
                    document.getElementById("sec1dataset").value = "1";
                }
               document.getElementById("sec1horizon").disabled = true;
               document.getElementById("sec1dataset").disabled = true;
      
               if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeightFixed != "") {
                    document.getElementById("sec1fixedheight").checked = true;
               }
               //�����ʼ�����horizon��dataset
               sec1Horizon = parseFloat(document.getElementById("sec1horizon").value);
               sec1Dataset = parseFloat(document.getElementById("sec1dataset").value);
               window.parent.detail1initdataset = sec1Dataset;
               window.parent.detail1initcolumn = sec1Horizon;
               
            } else if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == 1) && 
                (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight) > 0))
                { 
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderHeight != "") {
                    document.getElementById("sec2headerheight").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderHeight;
                } else {
                    document.getElementById("sec2headerheight").value = "<%=Constants.AREA_LIMITED_HEIGHT%>";
                }
                
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].RowHeight != "") {
                    document.getElementById("sec2rowheight").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].RowHeight;
                } else {
                    document.getElementById("sec2rowheight").value = "<%=Constants.AREA_LIMITED_HEIGHT%>";
                }
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum != "") {
                    sec2BeginFlag = false;
                    document.getElementById("sec2horizon").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum;
                } else {
                    document.getElementById("sec2horizon").value = "1";
                }
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum != "") {
                    document.getElementById("sec2dataset").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum;
                } else {
                    document.getElementById("sec2dataset").value = "1";
                }
                document.getElementById("sec2horizon").disabled = true;
                document.getElementById("sec2dataset").disabled = true;
                if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeightFixed != "") {
                    document.getElementById("sec2fixedheight").checked = true;
                }
               sec2Horizon = parseFloat(document.getElementById("sec2horizon").value);
               sec2Dataset = parseFloat(document.getElementById("sec2dataset").value);
               window.parent.detail2initdataset = sec2Dataset;
               window.parent.detail2initcolumn = sec2Horizon;
            }
        
        }
    } else {
        document.getElementById("sec1headerheight").value = window.parent.detail1headerheight;
        document.getElementById("sec1rowheight").value = window.parent.detail1rowheight;
        document.getElementById("sec1horizon").value = window.parent.detail1columnnum;
        document.getElementById("sec1dataset").value = window.parent.detail1datasetnum;
        document.getElementById("sec1reset").checked = window.parent.detail1reset;
        if (!window.parent.detail1reset) {
             document.getElementById("sec1horizon").disabled = true;
             document.getElementById("sec1dataset").disabled = true;
        } 
        document.getElementById("sec1fixedheight").checked = window.parent.detail1fixedheight;
        
        document.getElementById("sec2headerheight").value = window.parent.detail2headerheight;
        document.getElementById("sec2rowheight").value = window.parent.detail2rowheight;
        document.getElementById("sec2horizon").value = window.parent.detail2columnnum;
        document.getElementById("sec2dataset").value = window.parent.detail2datasetnum;
        document.getElementById("sec2reset").checked = window.parent.detail2reset;
        if (!window.parent.detail1reset) {
             document.getElementById("sec2horizon").disabled = true;
             document.getElementById("sec2dataset").disabled = true;
        } 
        document.getElementById("sec2fixedheight").checked = window.parent.detail2fixedheight;

    }
   
    createBtnTabs();
    window.parent.setButtonStatus(3);
    window.parent.framePreFlag[3] = false;
    
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	judgeLegal
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�ж�tabҳ�������ݺϷ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function judgeLegal(index) 
{

    var errorFlag = false;
  
    var sec1headerheight = trimString(document.getElementById("sec1headerheight").value);
    var sec1rowheight = trimString(document.getElementById("sec1rowheight").value);
    var sec1horizon = trimString(document.getElementById("sec1horizon").value);
    var sec1dataset = trimString(document.getElementById("sec1dataset").value);
    var sec2headerheight = trimString(document.getElementById("sec2headerheight").value);
    var sec2rowheight = trimString(document.getElementById("sec2rowheight").value);
    var sec2horizon = trimString(document.getElementById("sec2horizon").value);
    var sec2dataset = trimString(document.getElementById("sec2dataset").value);
    var se1reset = document.getElementById("sec1reset").checked;
    var se2reset = document.getElementById("sec2reset").checked;
    var totalHeight;

 
   
    for (var i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.length; i++) {
       
      
        if (index == 0 && window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == index) {
        
            if (!checkPosition(sec1headerheight)) {
                alert("<%=Resources.Template.positionFormat%>");
                errorFlag = true;
                document.getElementById("sec1headerheight").focus();
            }  else if ((parseFloat(sec1headerheight) > 0) && (parseFloat(sec1headerheight) < parseFloat("<%=Constants.AREA_LIMITED_HEIGHT%>"))){
                alert("<%=Resources.Template.areaMinHeight%>" + "(Minimal Height:" + "<%=Constants.AREA_LIMITED_HEIGHT%>" + ")");
                errorFlag = true;
                document.getElementById("sec1headerheight").focus();
            } else if (parseFloat(sec1headerheight) > parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight)) {
                alert("<%=Resources.Template.headerHeightIllegalSec1%>" + "(Section 1 Height:" + window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight + ")");
                errorFlag = true;
                document.getElementById("sec1headerheight").focus();
            }  else if (!checkPosition(sec1rowheight)) {
                alert("<%=Resources.Template.positionFormat%>");
                errorFlag = true;
                document.getElementById("sec1rowheight").focus();
            }  else if ((parseFloat(sec1rowheight) > 0) && (parseFloat(sec1rowheight) < parseFloat("<%=Constants.AREA_LIMITED_HEIGHT%>"))){
                alert("<%=Resources.Template.areaMinHeight%>" + "(Minimal Height:" + "<%=Constants.AREA_LIMITED_HEIGHT%>" + ")");
                errorFlag = true;
                document.getElementById("sec1rowheight").focus();
            } else if (parseFloat(sec1rowheight) > parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight)) {
                alert("<%=Resources.Template.rowHeightIllegalSec1%>" + "(Section 1 Height:" + window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight + ")");
                errorFlag = true;
                document.getElementById("sec1rowheight").focus();
            } else {
                 
//                totalHeight = parseFloat(sec1rowheight) + parseFloat(sec1headerheight);
               if (!errorFlag) {
                    if (se1reset) {
                        if (!checkDigits(sec1horizon)) {
                            alert("<%=Resources.Template.digitFormat%>");
                            errorFlag = true;
                           document.getElementById("sec1horizon").focus();
                        } else if (!checkDigits(sec1dataset)) {
                            alert("<%=Resources.Template.digitFormat%>");
                            errorFlag = true;
                            document.getElementById("sec1dataset").focus();
                        }
                    } else {
                         if (!checkDigits(sec1horizon)) {
                            alert("<%=Resources.Template.digitHorrizon%>");
                            errorFlag = true;
//                           document.getElementById("sec1horizon").focus();
                        } else if (!checkDigits(sec1dataset)) {
                            alert("<%=Resources.Template.digitDataSet%>");
                            errorFlag = true;
//                            document.getElementById("sec1dataset").focus();
                        }
                    }
                }
                if (!errorFlag) {
                    if (parseFloat(sec1dataset%sec1horizon) == 0) {
                       totalHeight =  parseFloat(sec1dataset/sec1horizon*sec1rowheight) + parseFloat(sec1headerheight);
                    } else {
                       totalHeight =  parseFloat((parseInt(sec1dataset/sec1horizon)+1)*sec1rowheight) + parseFloat(sec1headerheight);
                    }
                    if (totalHeight < parseFloat("<%=Constants.AREA_LIMITED_HEIGHT%>")){
                        alert("<%=Resources.Template.areaTotalMin%>" + "(Minimal Height:" + "<%=Constants.AREA_LIMITED_HEIGHT%>" + ")");
                        errorFlag = true;
                        document.getElementById("sec1headerheight").focus();
                    } else if (totalHeight > parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight)) 
                    {
                        alert("<%=Resources.Template.sectionHeightConfic%>" + "(Section 1 Height:" + window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight + ")");
                        errorFlag = true;
                        document.getElementById("sec1headerheight").focus();
                    }
               }
               
            } 
            if (!errorFlag) {
                errorFlag = checkSection(i, sec1headerheight, sec1rowheight, se1reset)
//                 if (!errorFlag) {
//                    checkSell(i, sec1dataset);
//                 }
            }
           
            break;
        } 
        if (index == 1 && window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == index) {
            if (!checkPosition(sec2headerheight)) {
                alert("<%=Resources.Template.positionFormat%>");
                errorFlag = true;
                document.getElementById("sec2headerheight").focus();
            }  else if ((parseFloat(sec2headerheight) > 0) && (parseFloat(sec2headerheight) < parseFloat("<%=Constants.AREA_LIMITED_HEIGHT%>"))){
                alert("<%=Resources.Template.areaMinHeight%>" + "(Minimal Height:" + "<%=Constants.AREA_LIMITED_HEIGHT%>" + ")");
                errorFlag = true;
                document.getElementById("sec2headerheight").focus();
            } else if (parseFloat(sec2headerheight) > parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight)) {
                alert("<%=Resources.Template.headerHeightIllegalSec2%>" + "(Section 2 Height:" + window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight + ")");
                errorFlag = true;
                document.getElementById("sec2headerheight").focus();
             }  else if (!checkPosition(sec2rowheight)) {
                alert("<%=Resources.Template.positionFormat%>");
                errorFlag = true;
                document.getElementById("sec2rowheight").focus();
            }  else if ((parseFloat(sec2rowheight) > 0) && (parseFloat(sec2rowheight) < parseFloat("<%=Constants.AREA_LIMITED_HEIGHT%>"))){
                alert("<%=Resources.Template.areaMinHeight%>" + "(Minimal Height:" + "<%=Constants.AREA_LIMITED_HEIGHT%>" + ")");
                errorFlag = true;
                document.getElementById("sec2rowheight").focus();
            }  else if (parseFloat(sec2rowheight) > parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight)) {
                alert("<%=Resources.Template.rowHeightIllegalSec2%>" + "(Section 2 Height:" + window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight + ")");
                errorFlag = true;
                document.getElementById("sec2rowheight").focus();
           
            } else {
               
                
                if (!errorFlag) {
                    if (se2reset) {
                        if (!checkDigits(sec2horizon)) {
                            alert("<%=Resources.Template.digitFormat%>");
                            errorFlag = true;
                            document.getElementById("sec2horizon").focus();
                        } else if (!checkDigits(sec2dataset)) {
                            alert("<%=Resources.Template.digitFormat%>");
                            errorFlag = true;
                            document.getElementById("sec2dataset").focus();
                        }
                    } else {
                        if (!checkDigits(sec2horizon)) {
                            alert("<%=Resources.Template.digitHorrizon%>");
                            errorFlag = true;
//                            document.getElementById("sec2horizon").focus();
                        } else if (!checkDigits(sec2dataset)) {
                            alert("<%=Resources.Template.digitDataSet%>");
                            errorFlag = true;
//                            document.getElementById("sec2dataset").focus();
                        }
                    }
                }
                if (!errorFlag) {
//                    totalHeight = parseFloat(sec2rowheight) + parseFloat(sec2headerheight);
                    if (parseFloat(sec2dataset%sec2horizon) == 0) {
                       totalHeight =  parseFloat(sec2dataset/sec2horizon*sec2rowheight) + parseFloat(sec2headerheight);
                    } else {
                       totalHeight =  parseFloat((parseInt(sec2dataset/sec2horizon)+1)*sec2rowheight) + parseFloat(sec2headerheight);
                    }
                    if (totalHeight < parseFloat("<%=Constants.AREA_LIMITED_HEIGHT%>")){
                        alert("<%=Resources.Template.areaTotalMin%>" + "(Minimal Height:" + "<%=Constants.AREA_LIMITED_HEIGHT%>" + ")");
                        errorFlag = true;
                        document.getElementById("sec2headerheight").focus();
                    } else if (totalHeight > parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight)) 
                    {
                        alert("<%=Resources.Template.sectionHeightConfic%>" + "(Section 2 Height:" + window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight + ")");
                        errorFlag = true;
                        document.getElementById("sec2headerheight").focus();
                    }
                }
            }
            if (!errorFlag) {
                errorFlag = checkSection(i, sec2headerheight, sec2rowheight, se2reset);
//                if (!errorFlag) {
//                    checkSell(i, sec2dataset);
//                }
                
            }
            
            break;
        }
    
    }
    
   
      return errorFlag;
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkSell
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	���ݴ�DataSet�趨ֵ�����ṹ��Cell����Ŀ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkSell(i,dataSetPara)
{

     blankCellIndex.length = 0;
     var errorFlag = false;
     var datasetNo;
     var saveIdArray = new Array();
     
     if (dataSetPara != "") {
        datasetNo = parseInt(dataSetPara)
     } else {
        datasetNo = 0;
     }
     var cellcount = window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells.length;
   
     //����趨��dataSet��ĿС��ԭ�е�Cell���������¼�¿�cell����ţ��Ժ�ɾ��
     if (datasetNo < cellcount) {
        for (j=0; j<cellcount; j++) {
            if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects.length == 0) &&
                (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects.length == 0) &&
                (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects.length == 0) &&
                (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].RectangleObjects.length == 0) &&
                (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects.length == 0) &&
                (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].ArearObjects.length == 0)) {
                blankCellIndex.push(j);
            }
        }
        //����յ�cell�����������򱨴�
        if (blankCellIndex.length < cellcount-datasetNo) {
            errorFlag = true;
            alert("<%=Resources.Template.fullCell%>" + "(Nonempty Cells Count:" + parseInt(cellcount-blankCellIndex.length) + ")" );
         }
                    
    }
    return errorFlag;
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkPageHeader
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	���PageHeader����Ŀؼ��Ƿ���PageHeader����֮��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkSection(i,headerheightPara, rowheightPara,reset)
{

    var errorFlag = false;
    var dec1Flag = false;
    var height;
    var rowheight;
 
    if (headerheightPara != "" ) {
        height = parseFloat(headerheightPara);
    } else{
        height = 0;
    }
    
    if (rowheightPara != "" ) {
        rowheight = parseFloat(rowheightPara);
    } else{
        rowheight = 0;
    }
  
    if (i == 0) {
        dec1Flag = true;
    }
    var convertHeaderHeight = pixelToMm_y(mmToPixel_y(height));
   
    for (j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects.length; j++) {
        if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Angle == "180")) {
            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Height) > parseFloat(convertHeaderHeight)) {
                if (dec1Flag) {
                    alert("<%=Resources.Template.textYIllegalSection1%>" + " (Section 1 Header Height:" + height + ")");
                    document.getElementById("sec1headerheight").focus();
                } else {
                    alert("<%=Resources.Template.textYIllegalSection2%>" + " (Section 2 Header Height:" + height + ")");
                    document.getElementById("sec2headerheight").focus();
                }
                errorFlag = true;
                break;
            }
        } else {
             if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.TextObjects[j].Width) > parseFloat(convertHeaderHeight)) {
                if (dec1Flag) {
                    alert("<%=Resources.Template.textYIllegalSection1%>" + " (Section 1 Header Height:" + height + ")");
                    document.getElementById("sec1headerheight").focus();
                } else {
                    alert("<%=Resources.Template.textYIllegalSection2%>" + " (Section 2 Header Height:" + height + ")");
                    document.getElementById("sec2headerheight").focus();
                }
                errorFlag = true;
                break;
            }
        }   
            
    }
        
    if (!errorFlag) {
        for (j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects.length; j++) {
            if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Angle == "180")) {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Height) > parseFloat(convertHeaderHeight)) {
                    if (dec1Flag) {
                        alert("<%=Resources.Template.barcodeYIllegalSection1%>"+ " (Section 1 Header Height:" + height + ")");
                        document.getElementById("sec1headerheight").focus();
                    } else {
                         alert("<%=Resources.Template.barcodeYIllegalSection2%>"+ " (Section 2 Header Height:" + height + ")");
                         document.getElementById("sec2headerheight").focus();
                    }
                    errorFlag = true;
                    break;
                }
            } else {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.BarcodeObjects[j].Width) > parseFloat(convertHeaderHeight)) {
                    if (dec1Flag) {
                        alert("<%=Resources.Template.barcodeYIllegalSection1%>"+ " (Section 1 Header Height:" + height + ")");
                        document.getElementById("sec1headerheight").focus();
                    } else {
                         alert("<%=Resources.Template.barcodeYIllegalSection2%>"+ " (Section 2 Header Height:" + height + ")");
                         document.getElementById("sec2headerheight").focus();
                    }
                    errorFlag = true;
                    break;
                }
            }
                
        }
    }
    
     if (!errorFlag) {
        for (j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects.length; j++) {
            if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Angle == "180")) {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Thickness) > parseFloat(convertHeaderHeight)) {
                    if (dec1Flag) {
                        alert("<%=Resources.Template.lineYIllegalSection1%>"+ " (Section 1 Header Height:" + height + ")");
                        document.getElementById("sec1headerheight").focus();
                    } else {
                         alert("<%=Resources.Template.lineYIllegalSection2%>"+ " (Section 2 Header Height:" + height + ")");
                         document.getElementById("sec2headerheight").focus();
                    }
                    errorFlag = true;
                    break;
                }
            } else {
                 if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.LineObjects[j].Length) > parseFloat(convertHeaderHeight)) {
                    if (dec1Flag) {
                        alert("<%=Resources.Template.lineYIllegalSection1%>"+ " (Section 1 Header Height:" + height + ")");
                        document.getElementById("sec1headerheight").focus();
                    } else {
                         alert("<%=Resources.Template.lineYIllegalSection2%>"+ " (Section 2 Header Height:" + height + ")");
                         document.getElementById("sec2headerheight").focus();
                    }
                    errorFlag = true;
                    break;
                }
            }   
                
        }
    }
    if (!errorFlag) {
        for (j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.RectangleObjects.length; j++) {
            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.RectangleObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.RectangleObjects[j].Height) > parseFloat(convertHeaderHeight)) {
                if (dec1Flag) {
                    alert("<%=Resources.Template.recYIllegalSection1%>"+ " (Section 1 Header Height:" + height + ")");
                    document.getElementById("sec1headerheight").focus();
                } else {
                    alert("<%=Resources.Template.recYIllegalSection2%>"+ " (Section 2 Header Height:" + height + ")");
                    document.getElementById("sec2headerheight").focus();
                }
                errorFlag = true;
                break;
            }
              
                
        }
    }
       
    if (!errorFlag) {
        for (j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects.length; j++) {
            if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Angle == "180")) {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Height) > parseFloat(convertHeaderHeight)) {
                    if (dec1Flag) {
                        alert("<%=Resources.Template.picYIllegalSection1%>"+ " (Section 1 Header Height:" + height + ")");
                        document.getElementById("sec1headerheight").focus();
                    } else {
                        alert("<%=Resources.Template.picYIllegalSection2%>"+ " (Section 2 Header Height:" + height + ")");
                        document.getElementById("sec2headerheight").focus();
                    }
                    errorFlag = true;
                    break;
                }
            } else {
                if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.PictureObjects[j].Width) > parseFloat(convertHeaderHeight)) {
                    if (dec1Flag) {
                        alert("<%=Resources.Template.picYIllegalSection1%>"+ " (Section 1 Header Height:" + height + ")");
                        document.getElementById("sec1headerheight").focus();
                    } else {
                        alert("<%=Resources.Template.picYIllegalSection2%>"+ " (Section 2 Header Height:" + height + ")");
                        document.getElementById("sec2headerheight").focus();
                    }
                    errorFlag = true;
                    break;
                }
            }   
                
        }
    }
    
    if (!errorFlag) {
        for (j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.ArearObjects.length; j++) {
            if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.ArearObjects[j].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderArea.ArearObjects[j].Height) > parseFloat(convertHeaderHeight)) {
                if (dec1Flag) {
                    alert("<%=Resources.Template.picXIllegalSection1%>"+ " (Section 1 Header Height:" + height + ")");
                    document.getElementById("sec1headerheight").focus();
                } else {
                    alert("<%=Resources.Template.picXIllegalSection2%>"+ " (Section 2 Header Height:" + height + ")");
                    document.getElementById("sec2headerheight").focus();
                }
                errorFlag = true;
                break;
            }
            
                
        }
    }
       
    
    
    if (!errorFlag && !reset) {
        height = parseFloat(rowheight);
        var convertRowHeight = pixelToMm_y(mmToPixel_y(height));
        for (j=0; j<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells.length; j++) {
            for (l=0; l<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects.length; l++) {
                if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Angle == "180")) {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Height) > parseFloat(convertRowHeight)) {
                        if (dec1Flag) {
                            alert("<%=Resources.Template.textYIllegalCellSec1%>" + "(Cell:" + j + ")"+ " (Section 1 Cell Height:" + height + ")");
                            document.getElementById("sec1rowheight").focus();
                        } else {
                            alert("<%=Resources.Template.textYIllegalCellSec2%>" + "(Cell:" + j + ")"+ " (Section 2 Cell Height:" + height + ")");
                            document.getElementById("sec2rowheight").focus();
                        }
                        errorFlag = true;
                        break;
                    }
                } else {
                     if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].TextObjects[l].Width) > parseFloat(convertRowHeight)) {
                        if (dec1Flag) {
                            alert("<%=Resources.Template.textYIllegalCellSec1%>" + "(Cell:" + j + ")"+ " (Section 1 Cell Height:" + height + ")");
                            document.getElementById("sec1rowheight").focus();
                        } else {
                            alert("<%=Resources.Template.textYIllegalCellSec2%>" + "(Cell:" + j + ")"+ " (Section 2 Cell Height:" + height + ")");
                            document.getElementById("sec2rowheight").focus();
                        }
                        errorFlag = true;
                        break;
                    }
                }
                 
            }
                
            if (!errorFlag) {
                for (l=0; l<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects.length; l++) {
                    if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Angle == "180")) {
                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Height) > parseFloat(convertRowHeight)) {
                            if (dec1Flag) {
                                alert("<%=Resources.Template.barcodeYIllegalCellSec1%>" + "(Cell:" + j + ")"+ " (Section 1 Cell Height:" + height + ")");
                                document.getElementById("sec1rowheight").focus();
                            } else {
                                alert("<%=Resources.Template.barcodeYIllegalCellSec2%>" + "(Cell:" + j + ")"+ " (Section 2 Cell Height:" + height + ")");
                                document.getElementById("sec2rowheight").focus();
                            }
                            errorFlag = true;
                            break;
                        }
                   } else {
                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].BarcodeObjects[l].Width) > parseFloat(convertRowHeight)) {
                            if (dec1Flag) {
                                alert("<%=Resources.Template.barcodeYIllegalCellSec1%>" + "(Cell:" + j + ")"+ " (Section 1 Cell Height:" + height + ")");
                                document.getElementById("sec1rowheight").focus();
                            } else {
                                alert("<%=Resources.Template.barcodeYIllegalCellSec2%>" + "(Cell:" + j + ")"+ " (Section 2 Cell Height:" + height + ")");
                                document.getElementById("sec2rowheight").focus();
                            }
                            errorFlag = true;
                            break;
                        }
                   }
                   
                }   
            }
                
            if (!errorFlag) {
                for (l=0; l<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects.length; l++) {
                    if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Angle == "180")) {
                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Thickness) > parseFloat(convertRowHeight)) {
                            if (dec1Flag) {
                                alert("<%=Resources.Template.lineYIllegalCellSec1%>" + "(Cell:" + j + ")"+ " (Section 1 Cell Height:" + height + ")");
                                document.getElementById("sec1rowheight").focus();
                            } else {
                                alert("<%=Resources.Template.lineYIllegalCellSec2%>" + "(Cell:" + j + ")"+ " (Section 2 Cell Height:" + height + ")");
                                document.getElementById("sec2rowheight").focus();
                            }
                            errorFlag = true;
                            break;
                        }
                   } else {
                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].LineObjects[l].Length) > parseFloat(convertRowHeight)) {
                            if (dec1Flag) {
                                alert("<%=Resources.Template.lineYIllegalCellSec1%>" + "(Cell:" + j + ")"+ " (Section 1 Cell Height:" + height + ")");
                                document.getElementById("sec1rowheight").focus();
                            } else {
                                alert("<%=Resources.Template.lineYIllegalCellSec2%>" + "(Cell:" + j + ")"+ " (Section 2 Cell Height:" + height + ")");
                                document.getElementById("sec2rowheight").focus();
                            }
                            errorFlag = true;
                            break;
                        }
                   }
                        
                }
            }    
            if (!errorFlag) {
                for (l=0; l<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].RectangleObjects.length; l++) {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].RectangleObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].RectangleObjects[l].Height) > parseFloat(convertRowHeight)) {
                        if (dec1Flag) {
                            alert("<%=Resources.Template.recYIllegalCellSec1%>" + "(Cell:" + j + ")"+ " (Section 1 Cell Height:" + height + ")");
                            document.getElementById("sec1rowheight").focus();
                        } else {
                            alert("<%=Resources.Template.recYIllegalCellSec1%>" + "(Cell:" + j + ")"+ " (Section 2 Cell Height:" + height + ")");
                            document.getElementById("sec2rowheight").focus();
                        }
                        errorFlag = true;
                        break;
                    }
                  
                }
            }      
            
            if (!errorFlag) {
                for (l=0; l<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects.length; l++) {
                    if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Angle == "0") || (window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Angle == "180")) {
                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Height) > parseFloat(convertRowHeight)) {
                            if (dec1Flag) {
                               alert("<%=Resources.Template.picYIllegalCellSec1%>" + "(Cell:" + j + ")"+ " (Section 1 Cell Height:" + height + ")");
                               document.getElementById("sec1rowheight").focus();
                            } else {
                               alert("<%=Resources.Template.picYIllegalCellSec2%>" + "(Cell:" + j + ")"+ " (Section 2 Cell Height:" + height + ")");
                               document.getElementById("sec2rowheight").focus();
                            }
                            errorFlag = true;
                            break;
                        }
                   } else {
                        if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].PictureObjects[l].Width) > parseFloat(convertRowHeight)) {
                            if (dec1Flag) {
                               alert("<%=Resources.Template.picYIllegalCellSec1%>" + "(Cell:" + j + ")"+ " (Section 1 Cell Height:" + height + ")");
                               document.getElementById("sec1rowheight").focus();
                            } else {
                               alert("<%=Resources.Template.picYIllegalCellSec2%>" + "(Cell:" + j + ")"+ " (Section 2 Cell Height:" + height + ")");
                               document.getElementById("sec2rowheight").focus();
                            }
                            errorFlag = true;
                            break;
                        }
                   }
                    
                }
            }    
            if (!errorFlag) {
                for (l=0; l<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].ArearObjects.length; l++) {
                    if (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].ArearObjects[l].Y) + parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells[j].ArearObjects[l].Height) > parseFloat(convertRowHeight)) {
                        if (dec1Flag) {
                            alert("<%=Resources.Template.areaYIllegalCellSec1%>" + "(Cell:" + j + ")"+ " (Section 1 Cell Height:" + height + ")");
                            document.getElementById("sec1rowheight").focus();
                        } else {
                            alert("<%=Resources.Template.areaYIllegalCellSec2%>" + "(Cell:" + j + ")"+ " (Section 2 Cell Height:" + height + ")");
                            document.getElementById("sec2rowheight").focus();
                        }
                        errorFlag = true;
                        break;
                    }
                 
                    
                }
            }
            if (errorFlag) {
                    break;
            }
           
        }
    }
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
function exitPage()
{

    var errorFlag = false;
   
    
    var sec1headerheight = trimString(document.getElementById("sec1headerheight").value);
    var sec1rowheight = trimString(document.getElementById("sec1rowheight").value);
    var sec1horizon = trimString(document.getElementById("sec1horizon").value);
    var sec1dataset = trimString(document.getElementById("sec1dataset").value);
    var sec2headerheight = trimString(document.getElementById("sec2headerheight").value);
    var sec2rowheight = trimString(document.getElementById("sec2rowheight").value);
    var sec2horizon = trimString(document.getElementById("sec2horizon").value);
    var sec2dataset = trimString(document.getElementById("sec2dataset").value);
    var cellcount;
    var datasetNo;
    var se1reset = document.getElementById("sec1reset").checked;
    var se2reset = document.getElementById("sec2reset").checked;
   
   
    errorFlag = judgeLegal(pageIndex);
   
    if (!errorFlag)
    {
        for (var i=0; i<window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections.length; i++)
        {
            
            if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == 0) && 
            (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight) > 0))
            
            {  
                if (sec1headerheight == "") {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderHeight = "0";
                } else {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderHeight = sec1headerheight;
                }
                
                if (sec1rowheight == "") {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].RowHeight = "0";
                } else {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].RowHeight = sec1rowheight ;
                }
                if (se1reset) {
                    if (sec1horizon == "") {
                        window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum = "0";
                    } else {
                        window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum = sec1horizon ;
                    }
                    if (sec1dataset == "") {
                        window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum = "0" ;
                    } else {
                        window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum = sec1dataset ;
                    }
                    //��dataobject
                    clearDataObject(0);
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells.length = 0;
                    for (j=0; j<parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum); j++) {
                            var cell = com.inventec.template.manager.TemplateManager.getSectionCellFromStructure();
                            var secCell = cell.value;
                            window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells.push(secCell); 
                        
                    } 

                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Reset = "1";
                } else {
                     //����ǵ�һ�δ���section
                     //if(sec1BeginFlag) {
                     if(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum == "") {
                        if (sec1horizon == "") {
                        window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum = "0" ;
                        } else {
                            window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum = sec1horizon ;
                        }
                        if (sec1dataset == "") {
                            window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum = "0" ;
                        } else {
                            window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum = sec1dataset ;
                        }
                         //��dataobject
                        for (j=0; j<parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum); j++) {
                                var cell = com.inventec.template.manager.TemplateManager.getSectionCellFromStructure();
                                var secCell = cell.value;
                                window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells.push(secCell); 
                            
                        } 
                   }
                     window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Reset = "";
                }
                if (document.getElementById("sec1fixedheight").checked == true) {
                    
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeightFixed = "1";
                    
                } else {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeightFixed = "";
                }
                
            
            } 
            if ((window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Index == 1) && 
            (parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].AreaHeight) > 0))
            {
                if (sec2headerheight == "") {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderHeight = "0";
                } else {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeaderHeight = sec2headerheight;
                }
                if (sec2rowheight == "") {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].RowHeight = "0" ;
                } else {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].RowHeight = sec2rowheight 
                }
                if (se2reset) {
                    if (sec2horizon == "") {
                        window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum = "0" ;
                    } else {
                        window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum = sec2horizon ;
                    }
                    if (sec2dataset == "") {
                        window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum = "0" ;
                    } else {
                        window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum = sec2dataset ;
                    }
                     //��dataobject
                     clearDataObject(1);
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells.length = 0;
                    for (j=0; j<parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum); j++) {
                            var cell = com.inventec.template.manager.TemplateManager.getSectionCellFromStructure();
                            var secCell = cell.value;
                            window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells.push(secCell); 
                        
                    } 

                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Reset = "1";
                } else {
//                   if(sec2BeginFlag) {
                     if(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum == "") {
                        if (sec2horizon == "") {
                            window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum = "0" ;
                        } else {
                            window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].ColumnNum = sec2horizon ;
                        }
                        if (sec2dataset == "") {
                            window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum = "0" ;
                        } else {
                            window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum = sec2dataset ;
                        }
                         //��dataobject
                        for (j=0; j<parseFloat(window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].DateSetNum); j++) {
                                var cell = com.inventec.template.manager.TemplateManager.getSectionCellFromStructure();
                                var secCell = cell.value;
                                window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Cells.push(secCell); 
                            
                        } 
                   }
                   window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].Reset = "";
                }
                
                if (document.getElementById("sec2fixedheight").checked == true) {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeightFixed = "1";
                    
                } else {
                    window.parent.templateXmlAndHtml.PrintTemplateInfo.DetailSections[i].HeightFixed = "";
                }
                
            }
        } 
        
   }

   return errorFlag;
}

function sec1Reset()
{
   if (document.getElementById("sec1reset").checked) {
        document.getElementById("sec1horizon").disabled = false;
        document.getElementById("sec1dataset").disabled = false;
   } else {
        document.getElementById("sec1horizon").value = window.parent.detail1initcolumn;
        document.getElementById("sec1dataset").value = window.parent.detail1initdataset;
        document.getElementById("sec1horizon").disabled = true;
        document.getElementById("sec1dataset").disabled = true;
   }
}	
function sec2Reset()
{
    if (document.getElementById("sec2reset").checked) {
        document.getElementById("sec2horizon").disabled = false;
        document.getElementById("sec2dataset").disabled = false;
   } else {
        document.getElementById("sec2horizon").value = window.parent.detail2initcolumn;
        document.getElementById("sec2dataset").value =  window.parent.detail2initdataset;
        document.getElementById("sec2horizon").disabled = true;
        document.getElementById("sec2dataset").disabled = true;
   }
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
     
    
    var sec1headerheight = trimString(document.getElementById("sec1headerheight").value);
    var sec1rowheight = trimString(document.getElementById("sec1rowheight").value);
    var sec1horizon = trimString(document.getElementById("sec1horizon").value);
    var sec1dataset = trimString(document.getElementById("sec1dataset").value);
    var sec2headerheight = trimString(document.getElementById("sec2headerheight").value);
    var sec2rowheight = trimString(document.getElementById("sec2rowheight").value);
    var sec2horizon = trimString(document.getElementById("sec2horizon").value);
    var sec2dataset = trimString(document.getElementById("sec2dataset").value);
    var se1reset = document.getElementById("sec1reset").checked;
    var se2reset = document.getElementById("sec2reset").checked;
    var se1fixedheight = document.getElementById("sec1fixedheight").checked;
    var se2fixedheight = document.getElementById("sec2fixedheight").checked
  
  
 
   window.parent.detail1headerheight = sec1headerheight;
   window.parent.detail1rowheight = sec1rowheight;
   window.parent.detail1datasetnum = sec1dataset;
   window.parent.detail1columnnum = sec1horizon;
   window.parent.detail1reset = se1reset
   window.parent.detail1fixedheight = se1fixedheight;
   
   window.parent.detail2headerheight = sec2headerheight;
   window.parent.detail2rowheight = sec2rowheight;
   window.parent.detail2datasetnum = sec2dataset;
   window.parent.detail2columnnum = sec2horizon;
   window.parent.detail2reset = se2reset
   window.parent.detail2fixedheight = se2fixedheight;

  
}
//-->
</SCRIPT>

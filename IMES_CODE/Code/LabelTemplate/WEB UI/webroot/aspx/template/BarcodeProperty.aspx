﻿<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: Barcode ��������Tabҳ
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)       create
' 2012-02-29   Lucy Liu(EB2)       Modify:Barcode����inverse�趨
' Known issues:Any restrictions about this file
--%> 
<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_BarcodeProperty, App_Web_barcodeproperty.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id=Head1 runat="server">
    <title>Barcode Properties</title>
 <script type="text/javascript" src="../../commoncontrol/btnTabs.js"></script>
 <script language="JavaScript" src="../../commoncontrol/treeView/treeViewControl.js"></script>
 <script type="text/javascript" src="../../commoncontrol/createcounter.js"></script>
 <script language="JavaScript" src="../../commoncontrol/function/commonFunction.js"></script>
 
 
 </head>
<OBJECT classid=clsid:F9043C85-F6F2-101A-A3C9-08002B2F49FB VIEWASTEXT id="commonDlg" style="display:none" >
<PARAM NAME="_ExtentX" VALUE="847">
<PARAM NAME="_ExtentY" VALUE="847">
<PARAM NAME="_Version" VALUE="393216">
<PARAM NAME="CancelError" VALUE="0"><PARAM NAME="Color" VALUE="0">
<PARAM NAME="Copies" VALUE="1"><PARAM NAME="DefaultExt" VALUE="">
<PARAM NAME="DialogTitle" VALUE=""><PARAM NAME="FileName" VALUE="">
<PARAM NAME="Filter" VALUE=""><PARAM NAME="FilterIndex" VALUE="0">
<PARAM NAME="Flags" VALUE="1"><PARAM NAME="FontBold" VALUE="0">
<PARAM NAME="FontItalic" VALUE="0"><PARAM NAME="FontName" VALUE="">
<PARAM NAME="FontSize" VALUE="8"><PARAM NAME="FontStrikeThru" VALUE="0">
<PARAM NAME="FontUnderLine" VALUE="0"><PARAM NAME="FromPage" VALUE="0">
<PARAM NAME="HelpCommand" VALUE="0"><PARAM NAME="HelpContext" VALUE="0">
<PARAM NAME="HelpFile" VALUE=""><PARAM NAME="HelpKey" VALUE="">
<PARAM NAME="InitDir" VALUE=""><PARAM NAME="Max" VALUE="0">
<PARAM NAME="Min" VALUE="0"><PARAM NAME="MaxFileSize" VALUE="260">
<PARAM NAME="PrinterDefault" VALUE="1">
<PARAM NAME="ToPage" VALUE="0">
<PARAM NAME="Orientation" VALUE="1">
</object>
<%--load all js files--%>
<%--<fis:header id="header1" runat="server"/>--%>

<body class="bgBody" >
    <form id="form2" runat="server">
    <div>
    </div>
    </form>
    
    <div class="propertyDialog"> 
    <div id="con" ></div>
	<div id="div1" style="display:none;padding-right:15px" class="propertyDialogContent" >
		<TABLE width="100%"  border="0" cellpadding="2px" cellspacing="5px" >
		<TR>
	        <TD style="width:15%"><%=Resources.Template.objectName%>:</TD>
	        <TD>
	      
	            <input type="text" id="objectName" class="inputStyle" maxlength="20">
	        </TD>
        </TR>
        <TR>
	        <TD style="width:50%"><%=Resources.Template.symbology%>:</TD>
	        <TD>
	            <SELECT NAME="symbology" id="symbology" class="inputStyle" onchange = "changeSymbology(this.value)" >
	                <option value="<%=Constants.SYMBOLOGY_TYPE_39%>">Code 39</option>
	                <option value="<%=Constants.SYMBOLOGY_TYPE_128_A%>">Code 128A</option>	                
	                <option value="<%=Constants.SYMBOLOGY_TYPE_128_B%>">Code 128B</option>
	                <option value="<%=Constants.SYMBOLOGY_TYPE_128_C%>">Code 128C</option>	  
	                <option value="<%=Constants.SYMBOLOGY_TYPE_128_Auto%>">Code 128Auto</option>	   	                 	                
	                <option value="<%=Constants.SYMBOLOGY_TYPE_93%>">Code 93</option>
	                <option value="<%=Constants.SYMBOLOGY_TYPE_EANJAN_13%>">EAN/JAN-13</option>
	                <option value="<%=Constants.SYMBOLOGY_TYPE_UPC_A%>">UPC-A</option>
	            
	            </SELECT>
	        </TD>
        </TR>
        <%--<TR>
	        <TD><%=Resources.Template.barWidth%>:</TD>
	        <TD><input type="text" NAME="narrowwidth" id="narrowwidth" class="inputStyle"></input>mm</TD>
        </TR>--%>
        
          <TR>
	        <TD><%=Resources.Template.barMultipleRatio%>:</TD>
	        <TD>
	            <SELECT NAME="multipleratio" id="multipleratio" class="inputStyle"  >
	                <option value="<%=Constants.NARROWBAR_RATIO_1%>"><%=Constants.NARROWBAR_RATIO_1%></option>
	                <option value="<%=Constants.NARROWBAR_RATIO_2%>"><%=Constants.NARROWBAR_RATIO_2%></option>
	                <option value="<%=Constants.NARROWBAR_RATIO_3%>"><%=Constants.NARROWBAR_RATIO_3%></option>
	                <option value="<%=Constants.NARROWBAR_RATIO_4%>"><%=Constants.NARROWBAR_RATIO_4%></option>
	                <option value="<%=Constants.NARROWBAR_RATIO_5%>"><%=Constants.NARROWBAR_RATIO_5%></option>
	                <option value="<%=Constants.NARROWBAR_RATIO_6%>"><%=Constants.NARROWBAR_RATIO_6%></option>
	                <option value="<%=Constants.NARROWBAR_RATIO_7%>"><%=Constants.NARROWBAR_RATIO_7%></option>
	                <option value="<%=Constants.NARROWBAR_RATIO_8%>"><%=Constants.NARROWBAR_RATIO_8%></option>
	                <option value="<%=Constants.NARROWBAR_RATIO_9%>"><%=Constants.NARROWBAR_RATIO_9%></option>
	                <option value="<%=Constants.NARROWBAR_RATIO_10%>"><%=Constants.NARROWBAR_RATIO_10%></option>
	                	            
	            </SELECT>
	        </TD>
        </TR>
      
        <TR>
           <%-- <bug>
                BUG NO:ITC-992-0054
                REASON:��Դд��
            </bug> --%>
	        <TD><%=Resources.Template.areaHeight%>:</TD>
	        <TD><input type="text" NAME="height" id="height" class="inputStyle"></input>mm</TD>
        </TR>
        <TR>
	        <TD ><%=Resources.Template.ratio%>:</TD>
	        <TD>
	            <SELECT NAME="ratio" id="ratio" class="inputStyle">
	                <option value="2.0">2.0</option>
	                <%--<option value="2.1">2.1</option>
	                <option value="2.2">2.2</option>
	                <option value="2.3">2.3</option>
	                <option value="2.4">2.4</option>--%>
	                <option value="2.5">2.5</option>
	                <%--<option value="2.6">2.6</option>
	                <option value="2.7">2.7</option>
	                <option value="2.8">2.8</option>
	                <option value="2.9">2.9</option>--%>
	                <option value="3.0">3.0</option>
	                
	            </SELECT>
	        </TD>
        </TR>
        <TR>
	        <TD colspan="2" ><INPUT TYPE="checkbox" NAME="inverse" id="inverse"><%=Resources.Template.inverse%></TD>
	    </TR>
        </TABLE>
       
	        <FIELDSET  >
	        <LEGEND><INPUT TYPE="checkbox" NAME="readable" id="readable" onclick="chooseHuman(this.checked)" ><%=Resources.Template.humanReadable%></LEGEND>
	            <TABLE width="100%"  border="0" cellpadding="5px" cellspacing="5px">
                    <TR>
	                <TD style="width:50%"><%=Resources.Template.font%>:</TD>
	                <TD>
	                <input type="text" id="showFont" class="inputStyle" readonly ><INPUT type="button" value="..." id="setFont"  onclick="showFontDlg()" >
	                </TD>
                 </TABLE>    	     
	        </FIELDSET> 
	    <input type="hidden" id="fontName">
        <input type="hidden" id="fontStyle">
        <input type="hidden" id="fontSize">
       
	       <%-- <TD colspan="2"><INPUT TYPE="checkbox" NAME="readable" id="readable" ><%=Resources.Template.humanReadable%></TD>--%>
	   
	</div>
	 
	<div id="div2" style="display:none" class="propertyDialogContent">
		<TABLE width="100%"  border="0" cellpadding="5px" cellspacing="5px" >
        <TR>
	        <TD align="left" ><%=Resources.Template.source%>:&nbsp;&nbsp;&nbsp;</TD>
	        <TD>
	            <SELECT NAME="source" id="source" class="inputStyle" onchange="changeSource(this.value)">
	                <option value="<%=Constants.DATA_SOURCE_SCREEN_DATA%>">Screen Data</option>
	                <option value="<%=Constants.DATA_SOURCE_DATA_SET%>">DataSet</option>
	              <%--  <option value="<%=Constants.DATA_SOURCE_DATE%>">Date</option>
	                <option value="<%=Constants.DATA_SOURCE_PAGE%>">Page</option>--%>
	            
	            </SELECT>
	        </TD>
        </TR>
        </TABLE>
        
	    <FIELDSET>
	    <LEGEND><%=Resources.Template.option%></LEGEND>
	        <div id="screenDataDiv" style="display:;height:150px">
	            <TABLE width="100%"  border="0" cellpadding="5px" cellspacing="5px">
                <TR>
	            <TD style="width:30%"><%=Resources.Template.screenData%>:</TD>
	            <TD>
	                <input type="text" NAME="screenData" id="screenData" class="inputStyle" maxlength="50"></input>
	            </TD>
                </TR>
                </TABLE>
	        </div>
	        
	        <div id="dataSetDiv" style="display:none;height:150px">
	            <TABLE width="100%"  border="0" cellpadding="5px" cellspacing="5px">
                <TR>
	            <TD style="width:35%"><%=Resources.Template.dataset%>:</TD>
	            <TD>
	                <SELECT NAME="dataset" id="dataset" class="inputStyle" onchange = "changeDataSet(this.value)">
	                </SELECT>
	            </TD>
                </TR>
                <TR>
	            <TD ><%=Resources.Template.field%>:</TD>
	            <TD>
	                <SELECT NAME="field" id="field" class="inputStyle" >
	                 </SELECT>
	            </TD>
                </TR>
                <%-- <bug>
                    BUG NO:ITC-992-0044
                    REASON:ȥ��˴���diaplaytext
                </bug> --%>
                 <%-- <bug>
                    BUG NO:ITC-992-0047
                    REASON:��Դд��
                </bug> --%>
                <TR>
	            <TD ><%=Resources.Template.displayText%>:</TD>
	            <TD>
	                <input type="text" NAME="displayText" id="displayText"class="inputStyle" maxlength="50"></input>
	            </TD>
                </TR>
                <TR>
	            <TD style="width:35%"><INPUT TYPE="checkbox" NAME="recordIndexCheck" id="recordIndexCheck" onclick="setRecordIndex()"><%=Resources.Template.reIndex%>:</TD>
	            <TD>
	                <input type="text" NAME="txtRecordIndex" id="txtRecordIndex" class="inputStyle" disabled></input>
	            </TD>
                </TR>
                </TABLE>
	        </div>
	        
	        <div id="dateDiv" style="display:none;height:150px">
	            <TABLE width="100%"  border="0" cellpadding="5px" cellspacing="5px">
                <TR>
	            <TD style="width:30%"><%=Resources.Template.dateFormat%>:</TD>
	            <TD>
                    <%-- <bug>
                        BUG NO:ITC-992-0082
                        REASON:option��ʾ�ִ�д��
                    </bug> --%>
	                <SELECT NAME="dateFormat" id="dateFormat" class="inputStyle" >
	                    <option value="<%=Constants.MM_DD_YYYY%>">MM/DD/YYYY</option>
	                    <option value="<%=Constants.DD_MM_YYYY%>">DD/MM/YYYY</option>
	                    <option value="<%=Constants.YYYY_MM_DD%>">YYYY/MM/DD</option>
	                    <option value="<%=Constants.MMADDAYYYY%>">MM-DD-YYYY</option>
	                    <option value="<%=Constants.DDAMMAYYYY%>">DD-MM-YYYY</option>
	                    <option value="<%=Constants.YYYYAMMADD%>">YYYY-MM-DD</option>
	                    <option value="<%=Constants.DDBMMBYYYY%>">DD.MM.YYYY</option>
	                    <option value="<%=Constants.YYYYBMMBDD%>">YYYY.MM.DD</option>
	                </SELECT>
	            </TD>
                </TR>
                <TR>
	            <TD><%=Resources.Template.dateOffset%>:</TD>
	            <TD>
	                <input type="text" style="width:90px" name="dateOffset_ctr_0_100" id="dateOffset_ctr_0_100" value="0"  />&nbsp;
	                <SELECT NAME="dateOffsetUnitType" id="dateOffsetUnitType" style="width:90px" >
	                    <option value="<%=Constants.DATE_OFFSET_YEAR%>">Year</option>
	                    <option value="<%=Constants.DATE_OFFSET_MONTH%>">Month</option>
	                    <option value="<%=Constants.DATE_OFFSET_DAY%>">Day</option>
	                </SELECT>
	            </TD>
                </TR>
                </TABLE>
	        </div>
	        
	        <div id="pageDiv" style="display:none;height:150px">
	            <TABLE width="100%"  border="0" cellpadding="5px" cellspacing="5px">
                <TR>
	            <TD style="width:30%"><%=Resources.Template.pageFormat%>:</TD>
	            <TD>
	                <SELECT NAME="pageFormat" id="pageFormat" class="inputStyle" >
	                    <option value="<%=Constants.PAGE_FORMAT_1%>">1 of 1</option>
	                    <option value="<%=Constants.PAGE_FORMAT_2%>">1/1</option>
	                   
	                </SELECT>
	            </TD>
                </TR>
                </TABLE>
	        </div>
	    </FIELDSET>
	  
	</div>
	
	<div id="div3" style="display:;" class="propertyDialogContent" >
		
	   <FIELDSET >
	        <LEGEND ><%=Resources.Template.position%></LEGEND>
	            <TABLE width="100%"  border="0" cellpadding="5" cellspacing="5">
                    <TR>
	                <TD style="width:20%"><%=Resources.Template.x%>:</TD>
	                <TD>
	                    <input type="text" NAME="x" id="Text1" class="inputStyle"></input>mm
	                </TD>
                    </TR>
                    <TR>
	                <TD style="width:20%"><%=Resources.Template.y%>:</TD>
	                <TD>
	                    <input type="text" NAME="y" id="Text2" class="inputStyle"></input>mm
	                </TD>
                    </TR>
                 </TABLE>
    	       
	        </FIELDSET>
	        <div style="height:10px"></div>
	        <FIELDSET  >
	        <LEGEND><%=Resources.Template.eddy%></LEGEND>
	            <TABLE width="100%"  border="0" cellpadding="5" cellspacing="5">
                    <TR>
	                <TD style="width:20%"><%=Resources.Template.angle%>:</TD>
    	           
	                <TD>
	                    <SELECT NAME="angle" id="angle" class="inputStyle" >
	                        <option value="<%=Constants.ANGLE_0%>"><%=Constants.ANGLE_0%></option>
	                        <option value="<%=Constants.ANGLE_90%>"><%=Constants.ANGLE_90%></option>
	                        <option value="<%=Constants.ANGLE_180%>"><%=Constants.ANGLE_180%></option>
	                        <option value="<%=Constants.ANGLE_270%>"><%=Constants.ANGLE_270%></option>
	                       
	                    </SELECT>
	                </TD>
                    </TR>
                 </TABLE>    	     
	        </FIELDSET> 
	         <div style="height:10px"></div>
	         <FIELDSET  >
	        <LEGEND><%=Resources.Template.layoutArea%></LEGEND>
	            <TABLE width="100%"  border="0" cellpadding="5" cellspacing="5">
                    <TR>
	                <TD style="width:20%"><%=Resources.Template.layout%>:</TD>    	           
	                <TD>
	                  <SELECT NAME="areaLayout" id="areaLayout" class="inputStyle" >
	                        <option value="<%=Constants.CENTOR_LAYOUT%>"><%=Resources.Template.centralLayout%></option>
	                        <option value="<%=Constants.RIGHT_LAYOUT%>"><%=Resources.Template.rightLayout%></option>
	                        <option value=""></option>
	                    </SELECT>
	                 </TD>
	                </TR>
                 </TABLE>    	     
	        </FIELDSET>
	</div>
	<div id="div4" style="display:none;" class="propertyDialogContent">
	    <div style="margin-bottom:10px">
	        <INPUT TYPE="checkbox" NAME="matchcheck" id="matchcheck" onclick="checkMatch()" unchecked>Set Dependency
	    </div>
	    <div  style="width:40%;float:left;" >
            <FIELDSET >
	        <LEGEND><%=Resources.Template.dataset%></LEGEND>  
	            <div id=treeviewarea style="width:100%;height:150px;overflow:auto;" >
                </div>
	         </FIELDSET> 
         </div>
         <div  style="width:40%;float:left;" >
            <FIELDSET >
	        <LEGEND><%=Resources.Template.matchArea%></LEGEND>
	            <div style="height:150px;">
	            <TABLE width="100%"  border="0" cellpadding="5" cellspacing="5"  >
			    <TR>
	                <TD colspan="2"><INPUT TYPE="checkbox" NAME="reverse" id="reverse" disabled><%=Resources.Template.reverse%></TD>
	            </TR>
			    <TR>
	                <TD colspan="2">&nbsp;<%=Resources.Template.matchValue%></TD>
	            </TR>
                <TR>
	                <TD style="width:25%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<INPUT TYPE="radio" NAME="match" id="nullradio"  unchecked onclick="checkValue(this.id)" disabled>NULL</TD>
			    <TD></TD>
	            </TR>
                <TR>
	                <TD style="width:25%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<INPUT TYPE="radio" NAME="match" id="matchradio"  checked onclick="checkValue(this.id)" disabled>Value</TD>
	                <TD>
	                    <input type="text" NAME="matchvalue" id="matchvalue" style="width:100px" maxlength="50" disabled></input>
	                 </TD>
                </TR>
                </TABLE>  
            	 </div>
            	 
	          </FIELDSET>
         </div>
     
  	</div>
  	
  	
	<div  class="propertyDialogButton">
		<button id="save" type="button"  onclick="Save()"  ><%=Resources.Template.okButton%></button>&nbsp;&nbsp;
        <button id="cancel" type="button"  onclick="Cancel()"  ><%=Resources.Template.cancelButton%></button> 
	</div>
	</div>
	<input type="hidden" id="selfield" />
	<input type="hidden" id="seldataset" />
	<input type="hidden" id="seltype" />
</body>

<%--set style for readonly input controller--%>
<fis:footer id="footer1" runat="server"/> 

<SCRIPT LANGUAGE="JavaScript">

<!--

var templateXmlandHtml;
var printTemplateInfo = "";
var barcodeObject = "";
var dataObject = "";
var areaHeight = 0;
var templateWidth = 0;
var pageIndex = 2;
var isLoadingPage = true;
var tree =null;
var interval;
var belongSecCell = false;
var cellDataSetName = "";
var parentId;
var parentObj;
var convertTemplateWidth = 0;
var convertAreaHeight = 0;
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
		 
	for (var i=0; i<4; i++)
	{
	    var temp = new clsButton("tabs");
	    if (i == 0) {
	         temp.normalPic = "../../images/barcode" +"-1.jpg";
		    temp.selPic = "../../images/barcode" +"-2.jpg";
		    temp.disablePic = "../../images/barcode" +"-3.jpg";
	    } else {
		    temp.normalPic = "../../images/fields"+ i +"-1.jpg";
		    temp.selPic = "../../images/fields"+ i +"-2.jpg";
		    temp.disablePic = "../../images/fields"+ i +"-3.jpg";
	   }	
		tabs.addButton(temp);
			
	}
		
	//tabs.diableTab(0,true);
	con.innerHTML = tabs.toString();
	tabs.initSelect(2);
//		createTree();
}

//��������
createBtnTabs();

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	preCallback
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�л�Tabҳʱ��֤������
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
function preCallback(index)
{  
    var bLeaveFlag = true;
	switch(pageIndex){
	    case 0: 
       	    bLeaveFlag = !leaveDiv1();
		    break;
	    case 1: 
            bLeaveFlag = !leaveDiv2();
		    break;
        case 2: 
            bLeaveFlag = !leaveDiv3();
		    break;
        case 3: 
            bLeaveFlag = !leaveDiv4();
		    break;
         
        default:
            break;	
    }
	return bLeaveFlag;
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
		 	document.getElementById("div3").style.display = "none";
		 	document.getElementById("div4").style.display = "none";
		    break;
	    case 1: 
            
		    document.getElementById("div1").style.display = "none";
		 	document.getElementById("div2").style.display = "";
		 	document.getElementById("div3").style.display = "none";
		 	document.getElementById("div4").style.display = "none";
		    break;
        case 2: 
            
		    document.getElementById("div1").style.display = "none";
		 	document.getElementById("div2").style.display = "none";
		 	document.getElementById("div3").style.display = "";
		 	document.getElementById("div4").style.display = "none";
		    break;
        case 3: 
            
		    document.getElementById("div1").style.display = "none";
		 	document.getElementById("div2").style.display = "none";
		 	document.getElementById("div3").style.display = "none";
		 	document.getElementById("div4").style.display = "";
		    break;
         
        default:
            break;	
     }
         
     return true;
}
	
	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	createTree
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	���ô���������
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 	
function createTree()
{	
    tree = new MzTreeView("tree");
    tree.useArrow = false;
	tree.LoadData = "Load()";
	tree.icons["property"] = "property.gif";
	tree.icons["css"] = "collection.gif";
	tree.icons["book"]  = "book.gif";
	tree.iconsExpand["book"] = "bookopen.gif"; 
	tree.setIconPath("./"); 
	tree.nodes["-1_" + tree.total++] = "uuid" + tree.attribute_suffix + "-1"+tree.attribute_d+"text" + tree.attribute_suffix + "DataSet"+tree.attribute_d+" hasChild" + tree.attribute_suffix + "true"+tree.attribute_d+" type" + tree.attribute_suffix + "NODE_ROOT" +tree.attribute_d+"nodeuuid" + tree.attribute_suffix + "-1" ;
	tree.customIconFun = useSOPMngIcon;
      
    tree.nodeClick = function clicknode()
	{
	    var currentNode = tree.currentNode; 
		var uuid = currentNode.uuid;
		var pid = currentNode.parentId;
		type =  currentNode.type;
	    var text = currentNode.text;
	    var nodeuuid = currentNode.nodeuuid;
	    document.getElementById("selfield").value = text;
	    document.getElementById("seldataset").value = nodeuuid;
	    document.getElementById("seltype").value = type;
	    if (type == "0" && document.getElementById("matchcheck").checked) {
	        document.getElementById("nullradio").disabled = false;
            document.getElementById("matchradio").disabled = false;
            document.getElementById("matchvalue").disabled = false;
            document.getElementById("reverse").disabled = false;
	    } else {
	        document.getElementById("nullradio").disabled = true;
            document.getElementById("matchradio").disabled = true;
            document.getElementById("matchvalue").disabled = true;
            document.getElementById("reverse").disabled = true;
	    }
	            
	 }	
		   
	 document.getElementById('treeviewarea').innerHTML = tree.toString();
	
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	useSOPMngIcon
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	ʹ��Icon
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	    
function useSOPMngIcon(node, treeObj)
{
    var strIconName = "";
	var strPrefix = "../../images/";
			
	switch (node.type){
        case "NODE_ROOT":
            strIconName = "root.gif";
            break;
        case "1":
            strIconName = "dataserver.gif";
            break;
        case "0":
             strIconName = "database.gif";
             break;
               
        default:
             break;
	}
    return strPrefix +strIconName;
}		

	    
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Load
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	������ʱ����ȡ����Դ����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~        
function Load()
{
    var source= tree.nodes[tree.expandingNode.sourceIndex]
    var uuid = tree.getAttribute(source, "uuid")
    ShowWait();
    InitChild(uuid);
}
       
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	InitChild
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	��������Դ����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~   	       
function InitChild(uuid)
{
    if (uuid == "-1") {
        for (i=0; i < printTemplateInfo.DatasettingList.length; i++) {
            var arrNode = new Array();
			arrNode[arrNode.length] = 'uuid' + tree.attribute_suffix + i;
			arrNode[arrNode.length] = 'text' + tree.attribute_suffix + printTemplateInfo.DatasettingList[i].DataSetName;
			arrNode[arrNode.length] = 'type' + tree.attribute_suffix + "1";
			arrNode[arrNode.length] = 'nodeuuid' + tree.attribute_suffix + "root";
    		arrNode[arrNode.length] = 'hasChild' + tree.attribute_suffix + "true";
			tree.nodes[tree.expandingNode.sid + '_' + ++tree.total] = arrNode.join(tree.attribute_d);
        }
                
                
     } else {
               
        for (i=0; i < printTemplateInfo.DatasettingList[uuid].Fields.length; i++) {
            var arrNode = new Array();
			arrNode[arrNode.length] = 'uuid' + tree.attribute_suffix + i;
			arrNode[arrNode.length] = 'text' + tree.attribute_suffix + printTemplateInfo.DatasettingList[uuid].Fields[i].FieldName1;
			arrNode[arrNode.length] = 'type' + tree.attribute_suffix + "0";
			arrNode[arrNode.length] = 'nodeuuid' + tree.attribute_suffix + printTemplateInfo.DatasettingList[uuid].DataSetName;
    		arrNode[arrNode.length] = 'hasChild' + tree.attribute_suffix + "false";
			tree.nodes[tree.expandingNode.sid + '_' + ++tree.total] = arrNode.join(tree.attribute_d);
         }
     }
     tree.dataFormat();
	 tree.load(tree.expandingNode.id);
	 tree.buildNode(tree.expandingNode.id);
	        
	 //new add by lzy to expand the last visiting node.
	        
	 tree.locateNode("UUID")
		    
     HideWait();
	 isLoadingPage = false;
		    
	       
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	changeSource
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�ı�source select��ֵ��option������Ӧ�仯
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function changeSource(value)
{
    if (value == "<%=Constants.DATA_SOURCE_SCREEN_DATA%>") {
        document.getElementById("screenDataDiv").style.display = "";
        document.getElementById("dataSetDiv").style.display = "none";
		document.getElementById("dateDiv").style.display = "none";
		document.getElementById("pageDiv").style.display = "none";
//		document.getElementById("screenData").value = "";
    } else if (value == "<%=Constants.DATA_SOURCE_DATA_SET%>") {
        document.getElementById("screenDataDiv").style.display = "none";
		document.getElementById("dataSetDiv").style.display = "";
		document.getElementById("dateDiv").style.display = "none";
		document.getElementById("pageDiv").style.display = "none";
//		document.getElementById("displayText").value = "";
		if (document.getElementById("dataset").length > 0) {
		    document.getElementById("dataset").options[0].selected = true;
		    changeDataSet(document.getElementById("dataset").options[0].value)
		}
    } else if (value == "<%=Constants.DATA_SOURCE_DATE%>") {
        document.getElementById("screenDataDiv").style.display = "none";
		document.getElementById("dataSetDiv").style.display = "none";
		document.getElementById("dateDiv").style.display = "";
		document.getElementById("pageDiv").style.display = "none";
		document.getElementById("dateFormat").options[0].selected = true;
		document.getElementById("dateOffset_ctr_0_100").value = 0;
		document.getElementById("dateOffsetUnitType").options[0].selected = true;
    } else if (value == "<%=Constants.DATA_SOURCE_PAGE%>") {
        document.getElementById("screenDataDiv").style.display = "none";
		document.getElementById("dataSetDiv").style.display = "none";
		document.getElementById("dateDiv").style.display = "none";
		document.getElementById("pageDiv").style.display = "";
		document.getElementById("pageFormat").options[0].selected = true;
    } 
}	

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	judgeOutBound
//| Author		:	Lucy Liu
//| Create Date	:	9/23/2009
//| Description	:	�жϽ���Ԫ���Ƿ񳬳�ģ�巶Χ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//function judgeOutBound()
//{
//    var errorFlag = false;
//  
//    var x = trimString(document.getElementById("x").value);
//    var y = trimString(document.getElementById("y").value);
//    var angle = document.getElementById("angle").value;
//    var height = trimString(document.getElementById("height").value);
//       
//   if ((angle == "0") || (angle == "180")) { 
//        if (parseFloat(x) > parseFloat(convertTemplateWidth)) {
//             if (belongSecCell) {
//               alert("<%=Resources.Template.barcodeXIllegal%>" + "(Section Cell Width:" + convertTemplateWidth + ")");
//            } else {
//                alert("<%=Resources.Template.barcodeXIllegal%>" + "(Template Width:" + templateWidth + ")")
//            }
//           
//            errorFlag = true;
//        } else if (parseFloat(y) + parseFloat(height) > parseFloat(convertAreaHeight)) {
//            
//            alert("<%=Resources.Template.barcodeYIllegal%>" + "(Area Height:" + areaHeight + ")");
//          
//            errorFlag = true;
//        } 
//    } else {
//         if (parseFloat(x)  + parseFloat(height) > parseFloat(convertTemplateWidth)) {
//             if (belongSecCell) {
//               alert("<%=Resources.Template.barcodeXIllegal%>" + "(Section Cell Width:" + convertTemplateWidth + ")");
//            } else {
//                alert("<%=Resources.Template.barcodeXIllegal%>" + "(Template Width:" + templateWidth + ")")
//            }
//         
//            errorFlag = true;
//        } else if (parseFloat(y) > parseFloat(convertAreaHeight)) {
//            
//            alert("<%=Resources.Template.barcodeYIllegal%>" + "(Area Height:" + areaHeight + ")");
//         
//            errorFlag = true;
//        } 
//        
//    }   
//   return errorFlag;
//}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	leaveDiv1
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�뿪Div1����ʱ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function leaveDiv1()
{
 
    var errorFlag = false;
   
    var height = trimString(document.getElementById("height").value);
//        <bug>
//            BUG NO:ITC-992-0079
//            REASON:����3λС��
//        </bug>
    if (!check1DecimalNotZero(height)) {
        alert("<%=Resources.Template.oneDecimalFormat%>");
        document.getElementById("height").focus();
        errorFlag = true;
    } 
    
    return errorFlag;
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	saveDiv1
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����Div1����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function saveDiv1()
{

    var errorFlag = false;
   
    var symbology = document.getElementById("symbology").value;

    var multipleratio = document.getElementById("multipleratio").value;
    var height = trimString(document.getElementById("height").value);
    var ratio = document.getElementById("ratio").value;
    var objectName = document.getElementById("objectName").value;

    barcodeObject.Symbology = symbology;
    barcodeObject.NarrowBarWidthPixel = multipleratio;
    barcodeObject.Height = height;
    if (symbology == "<%=Constants.SYMBOLOGY_TYPE_39%>") {
        barcodeObject.Ratio = ratio;
    } else {
        barcodeObject.Ratio = "";
    }
    if (document.getElementById("readable").checked) {
        barcodeObject.HumanReadable = "1"
        var fontName = document.getElementById("fontName").value;
        var fontStyle = document.getElementById("fontStyle").value;
        var fontSize = document.getElementById("fontSize").value;
        
        barcodeObject.TextFont = fontName;
        barcodeObject.TextStyle = fontStyle;
        barcodeObject.TextSize = fontSize;
    } else {
         barcodeObject.HumanReadable = "";
    }
    barcodeObject.RealObjectName = objectName;
    //BarcodeObjectҲ��TextObjectһ��������inverse����
    
    if (document.getElementById("inverse").checked) {
        barcodeObject.Inverse = "1";
    } else {
        barcodeObject.Inverse = "";
    }
    
   
   
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	leaveDiv2
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�뿪Div2����ʱ����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function leaveDiv2()
{
 
    var errorFlag = false;
   
    var source = document.getElementById("source").value;
    var screenData;
    var displayText;
     
 
    //�ж�����Ϸ���
    if (source == "<%=Constants.DATA_SOURCE_SCREEN_DATA%>") {
            screenData = document.getElementById("screenData").value;
            if (trimString(screenData) == "") {
                 alert("<%=Resources.Template.noInputDisplay%>");
                 document.getElementById("screenData").focus();
                 errorFlag = true;
            } 
            
     } else if (source == "<%=Constants.DATA_SOURCE_DATA_SET%>") {
            if (document.getElementById("dataset").value == "") {
                alert("<%=Resources.Template.emptyDataset%>");
                document.getElementById("dataset").focus();
                errorFlag = true;
            } 
            else if (document.getElementById("field").value == "") {
                alert("<%=Resources.Template.emptyField%>");
                document.getElementById("field").focus();
                errorFlag = true;
            }  else {
                if (belongSecCell) {
                    if ((cellDataSetName != "") && (document.getElementById("dataset").value != cellDataSetName)) {
                        alert("<%=Resources.Template.repeatCellDataset%>" + "(" +cellDataSetName + ")" );
                        document.getElementById("dataset").focus();
                        errorFlag = true;
                    }
                   
                } 
                if (!errorFlag) {
                    displayText = document.getElementById("displayText").value;
                   
                    if (trimString(displayText) == "") {
                         alert("<%=Resources.Template.noInputDisplay%>");
                         document.getElementById("displayText").focus();
                         errorFlag = true;
                    
                    } else if (!document.getElementById("recordIndexCheck").disabled && document.getElementById("recordIndexCheck").checked) {
                        var reIndex = document.getElementById("txtRecordIndex").value;
                        if (reIndex == "") {
                             alert("<%=Resources.Template.noInputRecordIndex%>");
                             document.getElementById("txtRecordIndex").focus();
                             errorFlag = true;
                         } else if (!check100Digits(reIndex)) {
                             alert("<%=Resources.Template.digit100Format%>");
                             document.getElementById("txtRecordIndex").focus();
                             errorFlag = true;
                         }
                    }
                }
           }
            
            
     } 
    
    
    return errorFlag;
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	saveDiv2
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����Div2����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function saveDiv2()
{
 
    
   
    var source = document.getElementById("source").value;
    var screenData;
    var displayText;
     
 
    //�ж�����Ϸ���
    if (source == "<%=Constants.DATA_SOURCE_SCREEN_DATA%>") {
        screenData = document.getElementById("screenData").value;
          
            
     } else if (source == "<%=Constants.DATA_SOURCE_DATA_SET%>") {
           
        displayText = document.getElementById("displayText").value;
                 
            
     } 
    
    //����

    dataObject.Source = source;
   if (source == "<%=Constants.DATA_SOURCE_SCREEN_DATA%>") {
        dataObject.DisplayTxt = screenData;
       
        
    } else if (source == "<%=Constants.DATA_SOURCE_DATA_SET%>") {
       dataObject.DataSet = document.getElementById("dataset").value;
       dataObject.OutputField = document.getElementById("field").value;
       dataObject.DisplayTxt = displayText;
	   if (!document.getElementById("recordIndexCheck").disabled) {
           if (document.getElementById("recordIndexCheck").checked) {
                dataObject.RecordIndexSet = "1";
                dataObject.RecordIndex = document.getElementById("txtRecordIndex").value;
           } else {
                dataObject.RecordIndexSet = "";
                dataObject.RecordIndex = "";
           }  
        }
      
        
    } else if (source == "<%=Constants.DATA_SOURCE_DATE%>") {
      dataObject.DataFormat = document.getElementById("dateFormat").value;
       dataObject.DateOffset = document.getElementById("dateOffset_ctr_0_100").value;
	   dataObject.DateOffsetUnitType = document.getElementById("dateOffsetUnitType").value;
        
    } else if (dataObject.Source == "<%=Constants.DATA_SOURCE_PAGE%>") {
       dataObject.PageFormat = document.getElementById("pageFormat").value;
        
    } 
   
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	leaveDiv3
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�뿪Div3����ʱ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function leaveDiv3()
{
 
    var errorFlag = false;
  
    var x = trimString(document.getElementById("x").value);
    var y = trimString(document.getElementById("y").value);
    var angle = document.getElementById("angle").value;

    //�ж�����Ϸ���
   if (!checkPosition(x)) {
        alert("<%=Resources.Template.positionFormat%>");
        document.getElementById("x").focus();
        errorFlag = true;
    } else if (!checkPosition(y)) {
        alert("<%=Resources.Template.positionFormat%>");
        document.getElementById("y").focus();
        errorFlag = true;
    } 
    
    return errorFlag;
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	saveDiv3
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����Div3����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function saveDiv3()
{
   
    var x = trimString(document.getElementById("x").value);
    var y = trimString(document.getElementById("y").value);
    var angle = document.getElementById("angle").value;
    var areaLayout = document.getElementById("areaLayout").value;
    
    barcodeObject.X = x;
    barcodeObject.Y = y;
    barcodeObject.Angle = angle;
    barcodeObject.AlignInArea = areaLayout;
    
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	leaveDiv4
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�뿪Div4����ʱ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function leaveDiv4()
{
    var errorFlag = false;
    var type = document.getElementById("seltype").value;
    var seldataset = document.getElementById("seldataset").value;
    var selfield = document.getElementById("selfield").value;
    var matchValue;
 
    if (document.getElementById("matchcheck").checked) {
        if (type == "0") {
            if (document.getElementById("matchradio").checked) {
                matchValue = trimString(document.getElementById("matchvalue").value);
                if (matchValue == "") {
                    alert("<%=Resources.Template.noInputMatchedValue%>");
                    document.getElementById("matchvalue").focus();
                    errorFlag = true;
                 } 
            }
       
         
        } 
    } 
 
    return errorFlag;
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	saveDiv4
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����Div4����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function saveDiv4()
{
    
    var type = document.getElementById("seltype").value;
    var seldataset = document.getElementById("seldataset").value;
    var selfield = document.getElementById("selfield").value;
    var matchValue;
 
    if (document.getElementById("matchcheck").checked) {
        if (type == "0") {
            if (document.getElementById("matchradio").checked) {
                matchValue = trimString(document.getElementById("matchvalue").value);
                dataObject.DependencedValue = matchValue;
                dataObject.NullValue = "";
               
            } else if (document.getElementById("nullradio").checked) {
                dataObject.NullValue = "1"; 
                dataObject.DependencedValue = "";
           } 
           dataObject.OutputFiledDep = selfield;
           dataObject.DataSetDep = seldataset;
           if (document.getElementById("reverse").checked) {
                dataObject.Reverse = "1";
            } else {
                dataObject.Reverse = "";
            }
       
           
        } else {
            dataObject.Reverse = "";
            dataObject.OutputFiledDep = "";
            dataObject.DataSetDep = "";
            dataObject.DependencedValue = "";
            dataObject.NullValue = "";

        }
    } else {
        dataObject.Reverse = "";
        dataObject.OutputFiledDep = "";
        dataObject.DataSetDep = "";
        dataObject.DependencedValue = "";
        dataObject.NullValue = "";
    }
 
    
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	initPage
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	��ʼ��ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function initPage()
{


    //��ȡ��������printTemplateInfo����
//    printTemplateInfo = window.dialogArguments.printTemplate;
   var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
    if (ret.error != null) {
        alert(ret.error.Message);
    } else{
        templateXmlandHtml = ret.value;
         printTemplateInfo = templateXmlandHtml.PrintTemplateInfo;
           
    }
//    templateWidth = printTemplateInfo.TemplateWidth;
    pixel_per_inch_x = window.dialogArguments.pixel_per_inch_x;
    pixel_per_inch_y = window.dialogArguments.pixel_per_inch_y;
   
    //��ȡҪ�༭��barcode����
    var ret = searchObject(window.dialogArguments.id);
    barcodeObject = ret.object;
    areaHeight =  ret.areaHeight;
    templateWidth = ret.templateWidth;
    convertAreaHeight =  ret.convertAreaHeight;
    convertTemplateWidth =  ret.convertTemplateWidth;
    belongSecCell =  ret.belongSecCell;
    
    searchDataObject(barcodeObject.ObjectName, "");
    
    //�����Section����Ķ�����Ҫ��ȡ����Cell�ж����DataSet����֤һ��Cell��ֻ��ѡȡDataSet
    if (belongSecCell) {
        searchCellDataset(window.dialogArguments.id);
        document.getElementById("recordIndexCheck").disabled = true;
        document.getElementById("txtRecordIndex").disabled = true;
    }
    
    //��ҳ���ʼֵ

     //Tab4ҳ��
    createTree();
//    if (printTemplateInfo.DatasettingList.length > 0 ) {
        
       
        if (dataObject.OutputFiledDep != "") {
            document.getElementById("matchcheck").checked = true;
            document.getElementById("reverse").disabled = false;
            
            if (dataObject.NullValue == "1") {
                document.getElementById("nullradio").checked = true;
                document.getElementById("matchvalue").disabled = true;
            } else {
                document.getElementById("matchradio").checked = true;
                document.getElementById("matchvalue").value = dataObject.DependencedValue;
                document.getElementById("matchvalue").disabled = false;
            }
            if (dataObject.Reverse != "") {
                document.getElementById("reverse").checked = true;
            } else {
                document.getElementById("reverse").checked = false;
            }
        }
    
//    }

    //Tab1ҳ��
    document.getElementById("objectName").value = barcodeObject.RealObjectName;
    if (barcodeObject.Symbology != "") {
        for (i = 0; i < document.getElementById("symbology").options.length; i++) {
            if (barcodeObject.Symbology == document.getElementById("symbology").options[i].value)
            {
                document.getElementById("symbology").options[i].selected = true;
                break;
             }
        }
        changeSymbology(barcodeObject.Symbology);
    }
    if (barcodeObject.NarrowBarWidthPixel != "") {
        for (i = 0; i < document.getElementById("multipleratio").options.length; i++) {
            if (barcodeObject.NarrowBarWidthPixel == document.getElementById("multipleratio").options[i].value)
            {
                document.getElementById("multipleratio").options[i].selected = true;
                break;
             }
        }
      
    }
  
////    if (barcodeObject.NarrowBarWidth == "") {
////        
////        document.getElementById("narrowwidth").value = "0";
////    } else {
////        document.getElementById("narrowwidth").value = barcodeObject.NarrowBarWidth;
////    }
    if (barcodeObject.Height == "") {
        document.getElementById("height").value = "0";
    } else {
       document.getElementById("height").value = barcodeObject.Height;
    }
    if (barcodeObject.Ratio != "") {
        for (i = 0; i < document.getElementById("ratio").options.length; i++) {
            if (barcodeObject.Ratio == document.getElementById("ratio").options[i].value)
            {
                document.getElementById("ratio").options[i].selected = true;
                break;
             }
        }
    }
    
    if (barcodeObject.HumanReadable != "") {
        document.getElementById("readable").checked = true;
    } else {
        document.getElementById("readable").checked = false;
    }
    chooseHuman(document.getElementById("readable").checked);
    if (document.getElementById("readable").checked) {
        if (barcodeObject.TextFont != "") {
            document.getElementById("fontName").value = barcodeObject.TextFont;
        } else {
            document.getElementById("fontName").value = "";
        }
        commonDlg.FontName = document.getElementById("fontName").value;
        if (barcodeObject.TextStyle != "") {
            document.getElementById("fontStyle").value = barcodeObject.TextStyle;
        } else {
            document.getElementById("fontStyle").value = "";
        }
        if (document.getElementById("fontStyle").value == "<%=Constants.FONT_STYLE_ITALIC%>") {
            commonDlg.FontItalic = true;
        } else  if (document.getElementById("fontStyle").value == "<%=Constants.FONT_STYLE_BOLD%>") {
            commonDlg.FontBold = true;
        } else  if (document.getElementById("fontStyle").value == "<%=Constants.FONT_STYLE_BOLDITALIC%>") {
            commonDlg.FontBold = true;
            commonDlg.FontItalic = true;
        }
        if (barcodeObject.TextSize != "") {
            document.getElementById("fontSize").value = barcodeObject.TextSize;
            commonDlg.FontSize = document.getElementById("fontSize").value;
        } else {
            document.getElementById("fontSize").value = "";
        }
        
        document.getElementById("showFont").value = document.getElementById("fontName").value + " " + document.getElementById("fontSize").value;
    }
    
    if (typeof(barcodeObject.Inverse) != "undefined") {
    //�ϰ��ģ��û���������
        if (barcodeObject.Inverse != "") {
            document.getElementById("inverse").checked = true;
        }
    }
    
    
    //Tab2ҳ��
    //����dataSet List
    var bFlag = false;
   
    for (i=0; i < printTemplateInfo.DatasettingList.length; i++) {
        var oOption = document.createElement("OPTION");
        
		oOption.value = printTemplateInfo.DatasettingList[i].DataSetName;
		oOption.text  = printTemplateInfo.DatasettingList[i].DataSetName;
	    dataset.options.add(oOption);
	    //����dataSet Field List
	    
	    for (j=0; j < printTemplateInfo.DatasettingList[i].Fields.length; j++) {
	         var oOption = document.createElement("OPTION");
		    oOption.value = printTemplateInfo.DatasettingList[i].Fields[j].FieldName1;
		    oOption.text  = printTemplateInfo.DatasettingList[i].Fields[j].FieldName1;
	        field.options.add(oOption);
	    }
    }
   
    if (dataObject != "") {
        
        
        for (i = 0; i < document.getElementById("source").options.length; i++) {
            if (dataObject.Source == document.getElementById("source").options[i].value)
            {
                document.getElementById("source").options[i].selected = true;
                break;
             }
        }

        changeSource(dataObject.Source);
        
        if (dataObject.Source == "<%=Constants.DATA_SOURCE_SCREEN_DATA%>") {

            document.getElementById("screenData").value = dataObject.DisplayTxt;
            
        } else if (dataObject.Source == "<%=Constants.DATA_SOURCE_DATA_SET%>") {

            for (i = 0; i < document.getElementById("dataset").options.length; i++) {
                if (dataObject.DataSet == document.getElementById("dataset").options[i].value)
                {
                    bFlag = true;
                    document.getElementById("dataset").options[i].selected = true;
                    break;
                 }
            }
            if (!bFlag) {
                 var oOption = document.createElement("OPTION");       
	            oOption.value = "";
	            oOption.text  = "";
	            dataset.options.add(oOption);
	            document.getElementById("dataset").options[dataset.options.length-1].selected = true;
            }
            bFlag = false;
            changeDataSet(dataObject.DataSet);
             for (i = 0; i < document.getElementById("field").options.length; i++) {
                if (dataObject.OutputField == document.getElementById("field").options[i].value)
                {
                    bFlag = true;
                    document.getElementById("field").options[i].selected = true;
                    break;
                 }
            }
            if (!bFlag) {
               var oOption = document.createElement("OPTION");       
	           oOption.value = "";
	           oOption.text  = "";
	           field.options.add(oOption);
	           document.getElementById("field").options[field.options.length-1].selected = true;
            }
            document.getElementById("displayText").value = dataObject.DisplayTxt;
            if (!document.getElementById("recordIndexCheck").disabled) {
                if (dataObject.RecordIndexSet == "") {
                   
                    document.getElementById("recordIndexCheck").checked = false
                
                } else {
                 
                    document.getElementById("recordIndexCheck").checked = true;
                    document.getElementById("txtRecordIndex").disabled = false;
                    document.getElementById("txtRecordIndex").value = dataObject.RecordIndex;
    		     
               } 
           }
            
        } else if (dataObject.Source == "<%=Constants.DATA_SOURCE_DATE%>") {
            for (i = 0; i < document.getElementById("dateFormat").options.length; i++) {
                if (dataObject.DataFormat == document.getElementById("dateFormat").options[i].value)
                {
                    document.getElementById("dateFormat").options[i].selected = true;
                    break;
                 }
            }
            
            document.getElementById("dateOffset_ctr_0_100").value = dataObject.DateOffset;
            for (i = 0; i < document.getElementById("dateOffsetUnitType").options.length; i++) {
                if (dataObject.DateOffsetUnitType == document.getElementById("dateOffsetUnitType").options[i].value)
                {
                    document.getElementById("dateOffsetUnitType").options[i].selected = true;
                    break;
                 }
            }
		   
        
        } else if (dataObject.Source == "<%=Constants.DATA_SOURCE_PAGE%>") {
            for (i = 0; i < document.getElementById("pageFormat").options.length; i++) {
                if (dataObject.PageFormat == document.getElementById("pageFormat").options[i].value)
                {
                    document.getElementById("pageFormat").options[i].selected = true;
                    break;
                 }
            }
        } else {
            dataObject.Source = "<%=Constants.DATA_SOURCE_SCREEN_DATA%>";
            document.getElementById("screenData").value = "Text";
        }

    }

    //Tab3ҳ��
    if (barcodeObject.X == "") {
        document.getElementById("x").value = "0";
    } else {
        document.getElementById("x").value = barcodeObject.X;
    }
     if (barcodeObject.Y == "") {
        document.getElementById("y").value = "0";
    } else {
        document.getElementById("y").value = barcodeObject.Y;
    }
   
    if (barcodeObject.Angle != "") {
        for (i = 0; i < document.getElementById("angle").options.length; i++) {
            if (barcodeObject.Angle == document.getElementById("angle").options[i].value)
            {
                document.getElementById("angle").options[i].selected = true;
             }
        }
    }
     for (i = 0; i < document.getElementById("areaLayout").options.length; i++) {
        if (barcodeObject.AlignInArea == document.getElementById("areaLayout").options[i].value)
        {
            document.getElementById("areaLayout").options[i].selected = true;
         }
     }
    
     try {
        document.getElementById("x").focus();
    } catch(e) {
    }
    
    
    
}
//����

initPage();



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	searchDataObject
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����objectName(id)��printTemplateInfo�ṹ��DataObject�������ҵ���Ӧ��dataObject����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function searchDataObject(id, setDataObject) {
 
   
   
   
    for (i=0 ;i<printTemplateInfo.DataObjects.length; i++) {
        if (id == printTemplateInfo.DataObjects[i].ObjectName) {
            if (setDataObject == "") {
                dataObject = printTemplateInfo.DataObjects[i];
            } else {
                printTemplateInfo.DataObjects[i] = setDataObject;
            }
            break;
        }
    }
  
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getDimention
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����id��ȡ��������
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function getDimention(id)
{
    var myReg = /_a\d+_b$/ig;
    var dimension = id.match(myReg);//"_a0_b"
    var tmp = dimension[0].replace("_a","");
    tmp = tmp.replace("_b","");     //"0"
    tmp = parseInt(tmp);
	return tmp;
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	searchCellDataset
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	��ȡһ��Cell�����ſؼ���DataSetName(һ��Cell��ֻ��һ��Dataset)
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

function searchCellDataset(id) 
{
       
     var bFind = false;
     var temp = id.replace(new RegExp("_a","g"),"[");
     temp = temp.replace(new RegExp("_b","g"),"]");
     temp = temp.replace(new RegExp("_dot_","g"),".");
     var index = temp.indexOf(".BarcodeObjects");
     parentId = temp.substring(0,index);
     
     parentObj = eval(parentId);
     var dimention = getDimention(id);
     
    for (i=0; i<parentObj.TextObjects.length; i++) {
////        if (id != parentObj.TextObjects[i].ObjectName) {
            //����objectName��DataObject��������DataSet
            for (j=0 ;j<printTemplateInfo.DataObjects.length; j++) {
                if ((parentObj.TextObjects[i].ObjectName == printTemplateInfo.DataObjects[j].ObjectName) && (printTemplateInfo.DataObjects[j].Source == "<%=Constants.DATA_SOURCE_DATA_SET%>")){
                    cellDataSetName = printTemplateInfo.DataObjects[j].DataSet;
                    bFind = true;
                    break;
                }
            }
            
//        }
        if (bFind) {
            break;
        }
    }
    
    if (!bFind) {
        for (i=0; i<parentObj.BarcodeObjects.length; i++) {
            if (dimention != i) {
                //����objectName��DataObject��������DataSet
                for (j=0 ;j<printTemplateInfo.DataObjects.length; j++) {
                    if ((parentObj.BarcodeObjects[i].ObjectName == printTemplateInfo.DataObjects[j].ObjectName) && (printTemplateInfo.DataObjects[j].Source == "<%=Constants.DATA_SOURCE_DATA_SET%>")) {
                        cellDataSetName = printTemplateInfo.DataObjects[j].DataSet;
                        bFind = true;
                        break;
                    }
                }
                
            }
            if (bFind) {
                break;
            }
        }
    }
    
//    if (!bFind)
//    {
//        if (parentId.indexOf("ArearObjects") == -1)
//        {
//        
//            for (i=0; i<parentObj.ArearObjects.length; i++) {
//               for (j=0; j<parentObj.ArearObjects[i].TextObjects.length; j++) {
//                    if (id != parentObj.ArearObjects[i].TextObjects[j].ObjectName) {
//                        //����objectName��DataObject��������DataSet
//                        for (k=0 ;k<printTemplateInfo.DataObjects.length; k++) {
//                            if ((parentObj.ArearObjects[i].TextObjects[j].ObjectName == printTemplateInfo.DataObjects[k].ObjectName) && (printTemplateInfo.DataObjects[k].Source == "<%=Constants.DATA_SOURCE_DATA_SET%>")){
//                                cellDataSetName = printTemplateInfo.DataObjects[k].DataSet;
//                                bFind = true;
//                                break;
//                            }
//                        }
//                       
//                    }
//                    if (bFind) {
//                         break;
//                    }
//                }
//                if (bFind) {
//                   break;
//                }
//            }
//        
//        
//            if (!bFind) {
//                for (i=0; i<parentObj.ArearObjects.length; i++) {
//                    for (j=0; j<parentObj.ArearObjects[i].BarcodeObjects.length; j++) {
//                        if (id != parentObj.ArearObjects[i].BarcodeObjects[j].ObjectName) {
//                            //����objectName��DataObject��������DataSet
//                            for (k=0 ;k<printTemplateInfo.DataObjects.length; k++) {
//                                if ((parentObj.ArearObjects[i].BarcodeObjects[j].ObjectName == printTemplateInfo.DataObjects[k].ObjectName) &&  (printTemplateInfo.DataObjects[j].Source == "<%=Constants.DATA_SOURCE_DATA_SET%>")){
//                                    cellDataSetName = printTemplateInfo.DataObjects[k].DataSet;
//                                    bFind = true;
//                                    break;
//                                }
//                            }
//                            if (bFind) {
//                                 break;
//                            }
//                        }
//                        if (bFind) {
//                            break;
//                        }
//                    }
//                }
//            }
//        }
//   }
             
}             
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	changeSymbology
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�ı�Symbologyֵ��ratio�ؼ���״̬����Ӧ�仯��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function changeSymbology(value) 
{
    if (value == "<%=Constants.SYMBOLOGY_TYPE_39%>") {
        document.getElementById("ratio").disabled = false;
    } else {
        document.getElementById("ratio").selectedIndex = 0;
        document.getElementById("ratio").disabled = true;
    }
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	changeDataSet
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�ı�DataSetֵ��field�ؼ��������¹��졣
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function changeDataSet(value) 
{
    //���field select������
    obj = document.getElementById("field");  
    obj.options.length=0;
    //����dataSet Field List
    for (i=0; i < printTemplateInfo.DatasettingList.length; i++) {
        if (value == printTemplateInfo.DatasettingList[i].DataSetName) {
		    
	        for (j=0; j < printTemplateInfo.DatasettingList[i].Fields.length; j++) {
	            var oOption = document.createElement("OPTION");
		        oOption.value = printTemplateInfo.DatasettingList[i].Fields[j].FieldName1;
		        oOption.text  = printTemplateInfo.DatasettingList[i].Fields[j].FieldName1;
	            field.options.add(oOption);
	        }
	        break;
	    }
    }
}

        
        
        
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Cancel
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	ȡ���˴β������رմ��ڡ�
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function Cancel()
{
//    window.returnValue = printTemplateInfo;
    window.close();
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Finish
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�������ã��رմ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function Save()
{

    var ret = preCallback(pageIndex);
    if (ret) {

         var x = trimString(document.getElementById("x").value);
         var y = trimString(document.getElementById("y").value);
         var angle = document.getElementById("angle").value;
         var height = trimString(document.getElementById("height").value);
         var judgeObject = new Object();
         judgeObject.Width = 0;
         judgeObject.Height = height;
         judgeObject.X = x;
         judgeObject.Y = y;
         judgeObject.Angle = angle;
         var retObject = judgeOutBound(2,areaHeight,convertAreaHeight,templateWidth,convertTemplateWidth,belongSecCell,judgeObject,false)
         if (!retObject.errorFlag) {
            saveDiv1();
            saveDiv2();
            saveDiv3();
            saveDiv4();
        } else {
            alert(getPrefixMessage(retObject.prefixErr) + retObject.suffixErr)
            return;
        }
               
        templateXmlandHtml.toJSON = function(){return toJSON(this);};
	    var ret = com.inventec.template.manager.TemplateManager.saveSession(templateXmlandHtml);
	    if (ret.error != null) {
            alert(ret.error.Message);
        } else{
               
            window.returnValue = "";
            window.close();  
        }
     
   } 
}
	

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkValue
//| Author		:	Lucy Liu
//| Create Date	:	11/26/2008
//| Description	:	ѡ��nullvalue radio����ѡ��matchvalue radio,matchvalue�ı�������Ի���Ӧ�仯
//| Input para.	:	
////| Ret value	:	
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkValue(id)
{
    if (id == "nullradio") {
      
        document.getElementById("matchvalue").disabled = true;
    } else {
        document.getElementById("matchvalue").disabled = false;
    }
     
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkMatch
//| Author		:	Lucy Liu
//| Create Date	:	11/26/2008
//| Description	:	ѡ��match checkBox
//| Input para.	:	
////| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkMatch()
{
    if (document.getElementById("matchcheck").checked &&   document.getElementById("seltype").value == "0") {
        document.getElementById("nullradio").disabled = false;
        document.getElementById("matchradio").disabled = false;
        document.getElementById("matchvalue").disabled = false;
        document.getElementById("reverse").disabled = false;
     } else {
        document.getElementById("nullradio").disabled = true;
        document.getElementById("matchradio").disabled = true;
        document.getElementById("matchvalue").disabled = true;
        document.getElementById("reverse").disabled = true;
     }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	locateTree
//| Author		:	Lucy Liu
//| Create Date	:	11/26/2008
//| Description	:	���ڵ㶨λ
//| Input para.	:	
////| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

function locateTree()
{
    var index = new Array();
    if (!isLoadingPage) {
        for (i=0; i < printTemplateInfo.DatasettingList.length; i++) {
            if (dataObject.DataSetDep == printTemplateInfo.DatasettingList[i].DataSetName) {
			    index.push(i);
			    document.getElementById("seldataset").value = dataObject.DataSetDep;
			    for (j=0; j < printTemplateInfo.DatasettingList[i].Fields.length; j++) {
                    if (dataObject.OutputFiledDep == printTemplateInfo.DatasettingList[i].Fields[j].FieldName1) {
			            index.push(j);
			            document.getElementById("selfield").value = dataObject.OutputFiledDep;
			            document.getElementById("seltype").value = 0;
			            break;
                    }
			    }
            }
        }       
       
        tree.freshPath=index.reverse();
	    tree.locateNode("UUID");
	    clearInterval(interval);
	   
        
        
	} 	 
	     
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getPrefixMessage
//| Author		:	Lucy Liu
//| Create Date	:	9/23/2009
//| Description	:	����flag��ȡ������Ϣ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function getPrefixMessage(prefixFlag)
{
    var message = "";
    switch(prefixFlag) {
        case 11:
            //text
            message = "<%=Resources.Template.textXIllegal%>";          
            break;
        case 12:
            //text
            message = "<%=Resources.Template.textYIllegal%>";          
            break;    
        case 21:
            //barcode
            message = "<%=Resources.Template.barcodeXIllegal%>"
            break;
        case 22:
            //barcode
            message = "<%=Resources.Template.barcodeYIllegal%>";         
            break;
        case 31:
            //picture
            message = "<%=Resources.Template.picXIllegal%>";          
            break;
        case 32:
            //picture
            message = "<%=Resources.Template.picYIllegal%>";         
            break;
        case 41:
            //line
            message = "<%=Resources.Template.lineXIllegal%>";
            break;
        case 42:
            //line
            message = "<%=Resources.Template.lineYIllegal%>";
            break;
        case 51:
            //area
            message = "<%=Resources.Template.areaXIllegal%>";          
            break;
        case 52:
            //area
            message = "<%=Resources.Template.areaYIllegal%>";     
            break;
        case 61:
            //rectangle
            message = "<%=Resources.Template.recXIllegal%>";
            break; 
        case 62:
            //rectangle
            message = "<%=Resources.Template.recYIllegal%>";
            break; 
          default:
            break;
     }
     return message;
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	setRecordIndex
//| Author		:	Lucy Liu
//| Create Date	:	11/4/2009
//| Description	:	ʹ��checkbox������ı������checkbox�Ĺ�ѡ��������
//| Input para.	:	
////| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function setRecordIndex()
{
    if (document.getElementById("recordIndexCheck").checked) {
        document.getElementById("txtRecordIndex").disabled = false;
    } else {
        document.getElementById("txtRecordIndex").disabled = true;
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	showFontDlg
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�����������öԻ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
 function showFontDlg()
  { 
	 var fontStyle = "<%=Constants.FONT_STYLE_NORMAL%>";
     commonDlg.ShowFont();
	 document.getElementById("showFont").value = commonDlg.fontName + " " + commonDlg.FontSize;
	 document.getElementById("fontName").value = commonDlg.fontName;
	 if (commonDlg.FontBold && commonDlg.FontItalic) {
	    fontStyle = "<%=Constants.FONT_STYLE_BOLDITALIC%>";
	 } else if (commonDlg.FontBold) {
	    fontStyle = "<%=Constants.FONT_STYLE_BOLD%>";
	 } else if (commonDlg.FontItalic) {
	    fontStyle = "<%=Constants.FONT_STYLE_ITALIC%>";
	 } else {
	     fontStyle = "<%=Constants.FONT_STYLE_NORMAL%>";
	 }
	 
	 document.getElementById("fontStyle").value = fontStyle;
	 
	 document.getElementById("fontSize").value = commonDlg.FontSize;
     
      
     
  
  }

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	chooseHuman
//| Author		:	Lucy Liu
//| Create Date	:	11/15/2010
//| Description	:	ѡ���˶�����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
function chooseHuman(checked) 
{
    if(checked) {
        document.getElementById("showFont").disabled = false;
        document.getElementById("setFont").disabled = false;
    } else {
        document.getElementById("showFont").disabled = true;
        document.getElementById("setFont").disabled = true;
    }
}
//��ѯ���Ƿ�������
interval = setInterval("locateTree()", 2000);

//-->
</SCRIPT>
  
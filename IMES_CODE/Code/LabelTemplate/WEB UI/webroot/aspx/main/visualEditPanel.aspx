<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: 可视化模板编辑工具页面
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-22   xiaoling liu(EB2)   create
' Known issues:Any restrictions about this file
' qa-bug-no:ITC-992-0017
' reason:Alias只允许输入英文
' qa-bug-no:ITC-992-0040
' reason:OCX控件不能正常下载
' qa bug no:ITC-1114-0003，ITC-1114-0006,ITC-1200-0002, ITC-1200-0003，ITC-1200-0006，ITC-1200-0005,ITC-1200-0008
' 2010-11-01 Lucy Liu(eb1)        Modeify:IssueCode:	ITC-1200-0011,ITC-1200-0019,ITC-1200-0015

--%> 
<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_main_visualEditPanel, App_Web_visualeditpanel.aspx.39cd9290" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"  runat="server">
	<title>Edit Panel</title>
	<script type="text/javascript" src="jquery/jquery-1.3.2.js"></script>
	<script type="text/javascript" src="jquery/ui/ui.core.js"></script>
	<script type="text/javascript" src="jquery/ui/ui.draggable.js"></script>
	<script type="text/javascript" src="jquery/ui/ui.droppable.js"></script>
	<script type="text/javascript" src="jquery/ui/ui.selectable.js"></script>



    <script language="JavaScript" src="../../commoncontrol/treeView/treeViewControl.js"></script>
    <script language="JavaScript" src="../../commoncontrol/function/commonFunction.js"></script>
    <script language="JavaScript" src="../../commoncontrol/btnTabs.js"></script>

	<style type="text/css">
	.nodisplay { visibility:hidden; }
	span.general {
		position:absolute;
		border: 1px solid black;
		width:5px;
		height:5px;
		background-color:rgb(0,255,0);
		font-size:0;
	}

 	span.north-resize	  {border-color:black; left:51%;top:0;	margin-left:-3px; margin-top:-1px;}
	span.west-resize	  {border-color:black; left:0;top:50%;	margin-left:-1px; margin-top:-3px;}
	span.east-resize	  {border-color:black; right:0;top:50%; margin-right:-1px;margin-top:-3px;}
	span.south-resize	  {border-color:black; left:51%;bottom:0;margin-left:-3px; margin-bottom:-1px;}

 	.draggablePicture span.north-resize	  {border-color:black; left:50%;top:0;	margin-left:-3px; margin-top:0px;}
	.draggablePicture span.west-resize	  {border-color:black; left:0;top:50%;	margin-left:0px; margin-top:-3px;}
	.draggablePicture span.east-resize	  {border-color:black; right:0;top:50%; margin-right:0px;margin-top:-3px;}
	.draggablePicture span.south-resize	  {border-color:black; left:50%;bottom:0;margin-left:-3px; margin-bottom:0px;}

	.draggable_tool { width: 130px; height: 30px; padding: 5px; margin: 0 0 0 0; border: 0px solid #CCC;z-index:0;vertical-align:middle;font-family:monospace;font-size:12pt}

	.draggableText { position:absolute; padding: 0em; margin: 0 0 0 0; overflow:visible;white-space:nowrap; border: 1px dotted rgb(215,215,215);}

	.draggableLine { background-color:Black;position:absolute; padding: 0em; margin: 0 0 0 0; border: 0px solid black; }

	.draggableRectangle { position:absolute; padding: 0em; margin: 0 0 0 0; overflow:visible;white-space:nowrap;  border: 1px solid black;}

	.draggableArea { background:url("area.jpg");position:absolute; padding: 0em; margin: 0 0 0 0; overflow:visible;white-space:nowrap;  border: 1px dotted rgb(215,215,215);}
	
	.draggableBarcode {font-size:0; position:absolute; padding: 0em; margin: 0 0 0 0;overflow:visible;white-space:nowrap;  border: 1px dotted rgb(215,215,215);}

	.draggablePicture { background:url("picture.jpg"); position:absolute; padding: 0em; margin: 0 0 0 0; overflow:visible;white-space:nowrap;  border: 0px dotted rgb(215,215,215);}

	.selectedRectangleArea { font-size:0pt; position:absolute; padding: 0em; margin: 0 0 0 0; overflow:visible;white-space:nowrap; border: 1px dashed rgb(0,255,0);}

	.ui-state-hover {background-color:Blue}

	.ui-state-active {background-color:LightBlue}
	
		
	.area_title{
		position: absolute;
		background-color:rgb(204,204,204);
		border-bottom:solid rgb(204,204,204) 0px;
		border-left:solid rgb(204,204,204) 0px;
		border-top:solid rgb(127,127,127) 1px;
		border-right:solid rgb(204,204,204) 0px;
	}

	.area_body {
		position: absolute;
		background-color:white;
		border: solid rgb(204,204,204) 0px;
		border-bottom:solid rgb(204,204,204) 0px;
		border-left:solid rgb(204,204,204) 0px;
		border-top:solid rgb(204,204,204) 0px;
		border-right:solid rgb(204,204,204) 0px;
		z-index: 10000;
		overflow:hidden;
	}
	
	.section_cell {
		position: absolute;
		background-color:transparent;
		border-bottom:solid rgb(204,204,204) 1px;
		border-left:solid rgb(204,204,204) 1px;
		border-top:solid rgb(204,204,204) 1px;
		border-right:solid rgb(204,204,204) 1px;
		z-index: 10000;
		overflow:hidden;
	}


	.toolbar {
		width:32px;
		height:30px;
        display:block;   
        float:left;   
        margin:0 0 0 0;   
        background-color:rgb(255,255,255);   
        border:0px solid #dedede;   
        border-top:0px solid #eee;   
        border-left:0px solid #eee;   
		
	}		
	
    .toolbar button img{   
        margin:0 3px -3px 0;   
        padding:0;   
        border:none;   
        width:20px;   
        height:20px;   
    }   	
	
    .dataset_button 
    {
    	width:40px;
    }

	

	</style>
	

	


</head>
<object classid="clsid:24BB7CDB-562E-4D60-8A56-4DD1DD77BE48" codebase="../../activex/print.cab#version=<%= Constants.version%>"
        id="printWB" style="display:none"></object>

<%--load all js files--%>
<fis:header id="header1" runat="server"/>

<body onmouseup="fOnMouseUp();">
	<!--#page_header_body .ui-selecting, .area_body .ui-selecting, .section_cell .ui-selecting,#page_footer_body .ui-selecting { background: #FECA40; }-->

	<!--#page_header_body .ui-selected, .area_body .ui-selected, .section_cell .ui-selected,#page_footer_body .ui-selected { background: #F39814; color: white; }-->





    <form id="form2" runat="server">
    <div>
    
    </div>
    <asp:HiddenField ID="user" runat="server"></asp:HiddenField>
    </form>
<table  style="width:100%; border:0px" id="EditTitle">
    <tr>
    <td id="title" class="title" style=" border:0px; word-wrap:normal;">Quality Report-Edit</td>
    </tr>
</table>
<div id="toolbar" style="overflow:hidden;position:absolute;height:31px;width:100%;left:0;top:23;border:1px solid #a9a9a9;">
    <table cellpadding=0 cellspacing=0 style="border:0px solid red;width:100px;">
        <tr>
            <td><button id="btnSave" class="toolbar" accesskey="S" onclick="save()"  onmouseover='btn_mouse_over("imgSave")' onmouseout='btn_mouse_out("imgSave")'><img id="imgSave" src="../../images/Tool Save.gif" alt="Save(Alt+S)"></button></td>
            <td><button id="btnPrint" class="toolbar" accesskey="P" onclick="print()"  onmouseover='btn_mouse_over("imgPrint")' onmouseout='btn_mouse_out("imgPrint")'><img id="imgPrint" src="../../images/Tool Print.gif" alt="Print(Alt+P)"></button></td>
            <td><button id="btnPreview" class="toolbar" accesskey="V" onclick="preview()"   onmouseover='btn_mouse_over("imgPreview")' onmouseout='btn_mouse_out("imgPreview")'><img id="imgPreview" src="../../images/Tool Preview.gif" alt="Preview(Alt+V)"></button></td>
            <td><button id="btnSetTemplate" class="toolbar" accesskey="T" onclick="setTemplate()"   onmouseover='btn_mouse_over("imgSetTemplate")' onmouseout='btn_mouse_out("imgSetTemplate")'><img id="imgSetTemplate" src="../../images/Tool Template Setting.gif" alt="Set Template(Alt+T)"></button></td>

            <td><button class="toolbar" disabled style="cursor:auto"><img src="../../images/Tool Thick Line.jpg"></button></td>

            <td><button id="btnProperty" accesskey="A" class="toolbar" onclick="setProperty()" onmouseover='btn_mouse_over("imgProperty")' onmouseout='btn_mouse_out("imgProperty")'><img id="imgProperty" src="../../images/Tool Property.gif" alt="Set Property(Alt+A)"></button></td>
            <td><button id="btnCopy" class="toolbar" onclick="beginCopyObjects()" title="Copy(Ctrl+C)" onmouseover='btn_mouse_over("imgCopy")' onmouseout='btn_mouse_out("imgCopy")'><img id="imgCopy" src="../../images/Tool Paste.gif"></button></td>
            <td><button id="btnDelete" class="toolbar" onclick="delSelected()" title="Delete(Delete)" onmouseover='btn_mouse_over("imgDelete")' onmouseout='btn_mouse_out("imgDelete")'><img id="imgDelete" src="../../images/Tool Delete.gif"></button></td>

            <td><button class="toolbar" disabled style="cursor:auto"><img src="../../images/Tool Short Line.jpg"></button></td>

            <td><button id="btnBottom" class="toolbar" onclick="alignBottom()" onmouseover='btn_mouse_over("imgBottom")' onmouseout='btn_mouse_out("imgBottom")'><img id="imgBottom" src="../../images/Tool AlignBottom.gif" alt="Align Bottoms"></button></td>
            <td><button id="btnTop" class="toolbar" onclick="alignTop()" onmouseover='btn_mouse_over("imgTop")' onmouseout='btn_mouse_out("imgTop")'><img id="imgTop" src="../../images/Tool AlignTop.gif" alt="Align Tops"></button></td>
            <td><button id="btnLeft" class="toolbar" onclick="alignLeft()" onmouseover='btn_mouse_over("imgLeft")' onmouseout='btn_mouse_out("imgLeft")'><img id="imgLeft" src="../../images/Tool AlignLeft.gif" alt="Align Lefts"></button></td>
            <td><button id="btnRight" class="toolbar" onclick="alignRight()" onmouseover='btn_mouse_over("imgRight")' onmouseout='btn_mouse_out("imgRight")'><img id="imgRight" src="../../images/Tool AlignRight.gif" alt="Align Rights"></button></td>
            <td><button id="btnCenter" class="toolbar" onclick="alignCenter()" onmouseover='btn_mouse_over("imgCenter")' onmouseout='btn_mouse_out("imgCenter")'><img id="imgCenter" src="../../images/Tool AlignCenter.gif" alt="Align Centers"></button></td>
            <td><button id="btnMiddle" class="toolbar" onclick="alignMiddle()" onmouseover='btn_mouse_over("imgMiddle")' onmouseout='btn_mouse_out("imgMiddle")'><img id="imgMiddle" src="../../images/Tool AlignMiddle.gif" alt="Align Middles"></button></td>
            <td><button id="btnEddy" class="toolbar" onclick="eddy()" onmouseover='btn_mouse_over("imgEddy")' onmouseout='btn_mouse_out("imgEddy")'><img id="imgEddy" src="../../images/Tool Eddy.gif" alt="Eddy"></button></td>

            <td><button class="toolbar" disabled style="cursor:auto"> <img src="../../images/Tool Short Line.jpg" ></button></td>
        </tr>
    </table>





<!--
<input type="button" value="del" onclick="delSelected();" class="toolbar">
<input type="button" value="Bottom" onclick="createArea();" class="toolbar">
<input type="button" value="Top" onclick="alignTop();" class="toolbar">
<input type="button" value="Left" onclick="alignLeft();" class="toolbar">
<input type="button" value="Right" onclick="alignRight();" class="toolbar">
<input type="button" value="Center" onclick="alignCenter();" class="toolbar">
<input type="button" value="Middle" onclick="alignMiddle();" class="toolbar">
<input type="button" value="eddy" onclick="eddy();" class="toolbar">-->

<!--
<input value="X(mm):" type="text" id="rainX1" readonly style="position:absolute;border:0px solid black;left:62%;top:5px;font-family:Monospace;font-size:12pt"/>
<input value="" class="coordiator" type="text" id="rainX" style="left:70%;width:8%;top:5px;"/>
<input value="Y(mm):" type="text" id="rainY1" readonly style="border:0px solid black;position:absolute;left:79%;top:5px;font-family:Monospace;font-size:12pt"/>
<input value="" class="coordiator" type="text" id="rainY" style="left:87%;width:8%;top:5px;"/>
<input type=button value=Set style="position:absolute;left:96%;width:4%;top:5px;"/>
-->
<h2 id="rainX" style="position:absolute;border:0px solid black;left:70%;top:5px;font-family:Monospace;font-size:12pt">X(mm):</h2>
<h2 id="rainY" style="border:0px solid black;position:absolute;left:85%;top:5px;font-family:Monospace;font-size:12pt">Y(mm):</h2>
</div><!-- End toolbar -->


<div id="editarea" style="background-color:#e2f1f8;overflow:scroll;position:absolute;width: 77%; top:54px;height: 92%; left: 0px; right: auto;border:1px solid #a9a9a9;border-top:0px solid blue;">

<div id="area_title" style="font-family:monospace;font-size:12pt" class="area_title nodisplay"></div>
<div id="area_body" class="area_body nodisplay"></div>

<div id="section_cell" class="section_cell nodisplay"></div>

</div><!-- End editarea -->




<div id="toolbox" style="position:absolute;left: auto;top:55px; right: 0px;width:23%;height:38%;border:0px solid black;" onmouseout="highlight_tool('nothing');">
	<table  style="width:100%; border:0px" id="tableTitle">
	    <tr>
	    <td class="title" style=" border:0px; word-wrap:normal;">Object</td>
	    </tr>
	</table>
    <div style= "position:absolute;right:0px;top:0px;">
        <INPUT style="" type="image" src="../../images/nav.gif" value="button" id="shleft" name="shleft" onclick="OpenClose()">
    </div>    
	<div id="draggableText_tool" class="draggable_tool" onmousedown="loseFocus();setZero();" onmouseover="highlight_tool(this);">
	<img src="../../images/Toolbox Text.gif">&nbsp;<font style="vertical-align:top">Text</font>
	</div>
	<div id="draggableLine_tool" class="draggable_tool" onmousedown="loseFocus();setZero();" onmouseover="highlight_tool(this);">
	<img src="../../images/Toolbox Line.gif">&nbsp;<font style="vertical-align:top">Line</font>
	</div>
	<div id="draggableRectangle_tool" class="draggable_tool" onmousedown="loseFocus();setZero();" onmouseover="highlight_tool(this);">
	<img src="../../images/Toolbox Rectangle.gif">&nbsp;<font style="vertical-align:top">Rectangle</font>
	</div>
	<div id="draggableArea_tool" class="draggable_tool" onmousedown="loseFocus();setZero();" onmouseover="highlight_tool(this);">
	<img src="../../images/Toolbox Area.gif">&nbsp;<font style="vertical-align:top">Area</font>
	</div>
	<div id="draggableBarcode_tool" class="draggable_tool" onmousedown="loseFocus();setZero();" onmouseover="highlight_tool(this);">
	<img src="../../images/Toolbox Barcode.gif">&nbsp;<font style="vertical-align:top">Barcode</font>
	</div>
	<div id="draggablePicture_tool" class="draggable_tool" onmousedown="loseFocus();setZero();" onmouseover="highlight_tool(this);">
	<img src="../../images/Toolbox Picture.gif">&nbsp;<font style="vertical-align:top">Picture</font>
	</div>


    <div id="draggableText" class="draggableText nodisplay yes">
    Text
	    <span class="north-resize general"></span>
	    <span class="west-resize general"> </span>
	    <span class="east-resize general"> </span>
	    <span class="south-resize general"></span>
    </div>

    <div id="draggableLine" class="draggableLine nodisplay yes">
	    <span class="west-resize general"> </span>
	    <span class="east-resize general"> </span>
    </div>

    <div id="draggableRectangle" class="draggableRectangle nodisplay yes">
	    <span class="north-resize general"></span>
	    <span class="west-resize general"> </span>
	    <span class="east-resize general"> </span>
	    <span class="south-resize general"></span>
    </div>

    <div id="draggableArea" class="draggableArea nodisplay yes">
	    <span class="north-resize general"></span>
	    <span class="west-resize general"> </span>
	    <span class="east-resize general"> </span>
	    <span class="south-resize general"></span>
    </div>

    <div id="draggableBarcode" class="draggableBarcode nodisplay yes">
        <img src="barcode.gif"/>
	    <span class="north-resize general"></span>
	    <span class="west-resize general"> </span>
	    <span class="east-resize general"> </span>
	    <span class="south-resize general"></span>
    </div>

    <div id="draggablePicture" class="draggablePicture nodisplay yes">
        <!--img id="imgPicture" src="" style="display:none"-->
	    <span class="north-resize general"></span>
	    <span class="west-resize general"> </span>
	    <span class="east-resize general"> </span>
	    <span class="south-resize general"></span>
    </div>

    <div id="selectedRectangleArea" class="selectedRectangleArea nodisplay yes">
    </div>
    <div id="lineV" class="draggableLine nodisplay" style="font-size:0pt; border-left:1px dotted gray; border-bottom:0px solid red; border-top:0px solid red; border-right:0px solid red;">
    </div>
    <div id="lineH" class="draggableLine nodisplay" style="overflow:hidden; border-left:0px dotted gray; border-bottom:1px dotted gray; border-top:0px dotted red; border-right:0px dotted red;">
    </div>

</div><!-- End toolbox -->
<!--bug no:ITC-992-0073-->
<!--reason:增加table-layout:fixed-->
<div id="divAll" style="position:absolute;left: auto;top:43%; right: 0px;width:23%;height:50%;">
<div id="divTab" style="position:relative;left: auto;top:0; left: 0px;width:100%;height:20px;border:0px solid #a9a9a9;border-bottom:1px solid #a9a9a9;border-top:1px solid #a9a9a9;">
</div>

 <!--<div id="Div1" style="position:absolute;left: auto;top:43%; right: 0px;width:20%;height:53%;border:0px solid #a9a9a9;border-bottom:1px solid #a9a9a9;border-top:1px solid #a9a9a9;">
	    <table  style="table-layout:fixed;width:100%;height:100%; border:0px solid black" id="table2">
	        <tr>
	        <td class="title" style=" border:0px solid blue; word-wrap:normal;height:8%">DataSet</td>
	        </tr>
	        <tr>
	        <td><div id="Div2" style="background-color:White; overflow:scroll;width:100%;height:100%;border:0px solid red;"></div></td>
	        </tr>
	    </table>
    </div>-->

<div id="divDataset" style="display:none">
    <div id="dataset" style="position:relative;left: auto;top:0; left: 0px;width:100%;height:92%;border:0px solid #a9a9a9;border-bottom:1px solid #a9a9a9;border-top:1px solid #a9a9a9;">
	    <table  style="table-layout:fixed;width:100%;height:100%; border:0px solid black" id="table_dataset_title">
	        <tr>
	        <td><div id="treeviewarea" style="background-color:White; overflow:scroll;width:100%;height:100%;border:0px solid red;"></div></td>
	        </tr>
	    </table>
    </div>

    <div id="dataset_button" align=center style="position:relative;top:3px;border:0px solid red;border-bottom:0px solid black;">
    <input type="button" id="add" value="Add" onclick="addOrEditDataSet('add');" class="dataset_button">
    <input type="button" id="edit" value="Edit" onclick="addOrEditDataSet('edit');" class="dataset_button">
    <input type="button" id="delete" value="Del" onclick="deleteDataSet();" class="dataset_button">
    <input type="hidden" id="selNodeId" value=""/>
    <input type="hidden" id="selUUID" value=""/>
    </div><!-- End dataset_button -->
</div>
<div id="divObjectTree">
    <div id="objecttree" style="position:relative;left: auto;top:0; left: 0px;width:100%;height:100%;border:0px solid #a9a9a9;border-bottom:1px solid #a9a9a9;border-top:1px solid #a9a9a9;">
	    <table  style="table-layout:fixed;width:100%;height:100%; border:0px solid black" id="table1">
	        <tr>
	        <td><div id="treeviewobject" style="background-color:White; overflow:scroll;width:100%;height:100%;border:0px solid red;"></div></td>
	        </tr>
	    </table>
    </div>
</div>
</div>
<!--ie6背景图片不缓存bug处理-->
<!--[if IE 6]><script type="text/javascript">try { document.execCommand('BackgroundImageCache', false, true); } catch(e) {}</script><![endif]-->

</body>
<%--set style for readonly input controller--%>
<fis:footer id="footer1" runat="server"/> 

</html>

<script type="text/javascript">
    //	#page_header_body .ui-selected, #section1_body .ui-selected, #section2_body .ui-selected,#page_footer_body .ui-selected { background: #F39814; color: white; }
    //	#page_header_body .ui-selected, #section1_body .ui-selected, #section2_body .ui-selected,#page_footer_body .ui-selected	{filter:progid:DXImageTransform.Microsoft.Light();}

    //tip:jQuery innerHTML
    //In jQuery, if you use Selectors to select at HTML element, then you can not use the default JavaScript function innerHTML to get the HTML inside the element, instead, you need to use a jQuery function called html( ).
    //For instance, if you want to copy the HTML from the first span to the second one.
    //script> 
    //$(document).ready(function(){ 
    //var str = $("span:first").html(); 
    //$("span:last").html(str); 
    //}); 
    //script> 

    var pixel_per_inch_x = printWB.getLogPixElsx();//96pixels/inch
    var pixel_per_inch_y = printWB.getLogPixElsy();//96pixels/inch
    var mm_per_inch = 25.4;//25.4mm/inch
    var focus_container = "";
    var selectable_obj, droppable_obj, draggable_obj;
    var g_elementID = 0;
    var printTemplateInfo;
    var printTemplateXMLAndHtmlInfo;

    var constText = 0;
    var constRectangle = 1;
    var constLine = 2;
    var constPicture = 3;
    var constBarcode = 4;
    var constArea = 5;
    var gX,gY;

    var parentid;    
    var method; //add,edit   
    var nodeuuid=""; //edit/targetId //templateid 
    var uuid;//treeid
    var userName;//username
    var type;//type
    var title_height = 20;//所有title的高度都是20px
    var mytop = new Array();//存放每个cell的top值,2对应两个section
    mytop[0] = new Array();
    mytop[1] = new Array();
    
    var coor_top = 0;//暂存pageheader,section,pagefooter的top值
    var inner_cell_height_offset = 0, inner_cell_width_offset = 0;
        	            
    var f_PageHeaderMouseMove, f_PageFooterMouseMove, f_SectionHeaderMouseMove, f_SectionCellMouseMove;
    var AREA_LIMITED_MAX_HEIGHT = parseFloat("<%=Constants.AREA_LIMITED_MAX_HEIGHT%>");//判断section cell/header的高度的边界值

    var g_arrBarcodeWidthHeight = new Array();


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	fOnMouseUp
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2010
    //| Description	:	鼠标在容器外抬起（左边tree，上边title，右边空白），触发该事件，做失焦点处理。
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function fOnMouseUp(){
    //alert(g_bMouseMove);
                    if(!g_bMouseMove) return;
                    g_bMouseMove = false;
                    loseFocus();
                    g_CountAll = 0;
                    $("#"+focus_container).selectable('enable');

                    $("#" + focus_container + "_LineLeft").replaceWith("");
                    $("#" + focus_container + "_LineRight").replaceWith("");
                    $("#" + focus_container + "_LineTop").replaceWith("");
                    $("#" + focus_container + "_LineBottom").replaceWith("");

    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	$(document).keydown
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2010
    //| Description	:	键盘按键处理。包括1、处理上下左右箭头的；2、object group复制功能；3、delete
    //| Input para.	:	imgId
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    $(document).keydown(function(e){
        /*
        if(e.which == 46 || e.which == 110){
        //不让objects tree响应按键
        e.cancelBubble = true;
        return false;
        }*/
        
        //delete
        switch(e.which){
            case 46:
            case 110:
                delSelected();
                e.cancelBubble = true;
                return false;
        }
        
        //object group复制功能
        if(e.ctrlKey){
            switch(e.which){
                case 86://ctrl+v//paste
                
                    break;
                case 67://ctrl+c//copy
                    beginCopyObjects();
                    break;
                default:
                    return false;
            }
 
            e.cancelBubble = true;
            return false;
        }
        
        //处理上下左右箭头的
        var offX, offY;
		if(e.shiftKey){//0.1mm

            switch(e.which){
                case 37://left arrow
                    offX = -0.1;
                    offY = 0;
                    break;
                case 38://up arrow
                    offX = 0;
                    offY = -0.1;
                    break;
                case 39://right arrow
                    offX = 0.1;
                    offY = 0;
                    break;
                case 40://down arrow
                    offX = 0;
                    offY = 0.1;
                    break;
                default:
                    return false;
            }

		}else{//1mm

            switch(e.which){
                case 37://left arrow
                    offX = -1;
                    offY = 0;
                    break;
                case 38://up arrow
                    offX = 0;
                    offY = -1;
                    break;
                case 39://right arrow
                    offX = 1;
                    offY = 0;
                    break;
                case 40://down arrow
                    offX = 0;
                    offY = 1;
                    break;
                default:
                    return false;
            }

        }
        var selectedCount = $("#"+focus_container+" div").filter(".ui-selected").size();
        var oldX = new Array();
        var oldY = new Array();
        var arrObjId = new Array();
        var arrId = new Array();
        var currentNum = -1;
        
        var bErr = false;//标志是否有越界
        $("#"+focus_container+" div").filter(".ui-selected").each(function(){
            currentNum = currentNum + 1;
	        var id = $(this).attr("id");                                //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
            var objId = idToObj(id);

            //偏移后的坐标
            var tmpX = (Math.ceil(parseFloat(objId.X)*10)+offX*10)/10;
            var tmpY = (Math.ceil(parseFloat(objId.Y)*10)+offY*10)/10;//parseFloat(objId.Y)+offY;

            //保存原来的坐标
            oldX[currentNum] = objId.X;
            oldY[currentNum] = objId.Y;
            arrObjId[currentNum] = objId;
            arrId[currentNum] = id;
            
            //偏移后的坐标写入结构
            objId.X = tmpX + "";
            objId.Y = tmpY + "";
            
            var propertyType;
            
	        if(id.indexOf("LineObjects") > 0){
    	    
	            propertyType = 4;
    	        
	        }else if(id.indexOf("TextObjects") > 0){
    	    
	            propertyType = 1;
    	        
	        }else if(id.indexOf("BarcodeObjects") > 0){

	            propertyType = 2;
    	        
	        }else if(id.indexOf("PictureObjects") > 0){
    	    
	            propertyType = 3;
    	        
	        }else if(id.indexOf("RectangleObjects") > 0){
    	    
	            propertyType = 6;
    	        
	        }else if(id.indexOf("ArearObjects") > 0){
    	    
	            propertyType = 5;
    	        
	        }            
	        var tmp = getWidthHeightById(id).split("&");
            var retObject = judgePropertyBound(id, propertyType,true, tmp[0], tmp[1]);//tmp[0]:width, tmp[1]:height


            //越界，报告错误信息
            if (retObject.errorFlag) {
                bErr = true;
                //新的坐标越界，恢复原来的坐标
                for(var key = 0;key < oldX.length; key++){
                    (arrObjId[key]).X = oldX[key];
                    (arrObjId[key]).Y = oldY[key];
                }
               //越界，报告错误信息
                alert(getPrefixMessage(retObject.prefixErr) + retObject.suffixErr);
                //不让objects tree响应按键
                e.cancelBubble = true;
                return false;
            }
        });
        
        

        if(!bErr){
            //定位objects
            for(var key = 0;key < arrObjId.length; key++){
                $("#"+arrId[key]).css("left", mmToPixel_x((arrObjId[key]).X));
                $("#"+arrId[key]).css("top", mmToPixel_y((arrObjId[key]).Y));
                
                //1、记下绿色高亮的坐标，用于右上坐标显示
                //2、调整选择区域的位置
                if($("#"+arrId[key]).hasClass("thefirst")){
                    //1、记下绿色高亮的坐标，用于右上坐标显示
                    document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):"+(arrObjId[key]).X)				            
                    document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):"+(arrObjId[key]).Y)				            

                    //2、调整选择区域的位置
                    g_MinX = g_MinX + offX;
                    g_MinY = g_MinY + offY;
                    g_MaxX = g_MaxX + offX;
                    g_MaxY = g_MaxY + offY;
                    
                    
                    var objSelectBox = $("#" + focus_container + "_selectedRectangleArea");
                    objSelectBox.css({"left":mmToPixel_x(g_MinX),"top":mmToPixel_x(g_MinY)});

                    window.parent.frames("menu").bTemplateChanged = true;
                    
                }
                
            }
        }

        //不让objects tree响应按键
        e.cancelBubble = true;
        return false;
	});    
	
	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	beginCopyObjects
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2010
    //| Description	:	显示等待，如果不设定定时，无法显示等待画面
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
	function beginCopyObjects(){
	    if(g_CountAll == 0) return;
        ShowWait();
        setTimeout("copyObjects()",10);
	}
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	copyObjects
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2010
    //| Description	:	object group复制功能
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    var bFreshForPaste = false;
    function copyObjects(){
        window.parent.frames("menu").bTemplateChanged = true;
        /*
        //以下是取得所有复制对象相对于0，0点的基准坐标
        var minX = 0, minY = 0, count = 0;
        $("#"+focus_container+" div").filter(".ui-selected").each(function(){
            var srcId = $(this).attr("id");         //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
            var srcObj = idToObj(srcId);

            count++;
            if(count == 1){
                //使用第一个对象的x,y初始化最小值
                minX = parseFloat(srcObj.X);
                minY = parseFloat(srcObj.Y);
                
            }else{
                //取最小的X，Y值
                if(parseFloat(srcObj.X) < minX){
                    minX = parseFloat(srcObj.X);
                }
                if(parseFloat(srcObj.Y) < minY){
                    minY = parseFloat(srcObj.Y);
                }
                
            }
        })
        */
        //以下开始复制对象
        $("#"+focus_container+" div").filter(".ui-selected").each(function(){
            var srcId = $(this).attr("id");         //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
            var srcObj = idToObj(srcId);

            var strObjType;
            var obj_type;
            if(srcId.indexOf("LineObjects") > 0){

                strObjType = "#draggableLine";
                obj_type = constLine;
                
            }else if(srcId.indexOf("TextObjects") > 0){

                strObjType = "#draggableText";
                obj_type = constText;
                
            }else if(srcId.indexOf("BarcodeObjects") > 0){

                strObjType = "#draggableBarcode";
                obj_type = constBarcode;
                
            }else if(srcId.indexOf("PictureObjects") > 0){

                strObjType = "#draggablePicture";
                obj_type = constPicture;
                
            }else if(srcId.indexOf("RectangleObjects") > 0){

                strObjType = "#draggableRectangle";
                obj_type = constRectangle;
                
            }else if(srcId.indexOf("ArearObjects") > 0){

                strObjType = "#draggableArea";
                obj_type = constArea;
                
            }    
            
            var cloneObj = $(strObjType).clone(true);//ui.draggable.clone();
            cloneObj.appendTo($("#"+focus_container));


            cloneObj.draggable(draggable_obj)
                    .dblclick(f_ObjectDblClick)
                    .mousedown(f_ObjectMouseDown)
                    .mouseup(f_ObjectMouseUp)
                   .bind("setBottom", function(event,a){
                   
                        //置底(ArearObject/RectangleObject除外)，在loseFocus中trigger
                        var id = $(this).attr("id");
                        if(!(id.indexOf("ArearObject") > 0 || id.indexOf("RectangleObject") > 0)){
                            $(this).css({"z-index":"0"});
                            $(this).children("span").css({"z-index":"0"});//设置四个得焦点标志
                        }
                    });

             if(g_CountAll == 1){
                 cloneObj.draggable("enable")
             }else{
                 cloneObj.draggable("disable")
             }
             cloneObj.addClass("ui-selected")
                     .addClass("yes")
                     .addClass("son")
	                 .removeClass("nodisplay")
	                 //.draggable('enable')
                     .css({cursor:"move"});
                     
	         cloneObj.children("span").addClass("yes");

             
             if(obj_type == constArea){
                 cloneObj.css({"z-index":"-9"});
             }
             if(obj_type == constRectangle){
                 cloneObj.css({"z-index":"-10"});
             }
	                    
             //把对象加入到结构中，返回对象的唯一id，也是text/line/barcode/....的objectname，同时也是dataobject.objectname
             var desId = fillup_structure(focus_container,obj_type,cloneObj);
             var desObj = idToObj(desId);
             
             cloneObj.attr("id",desId);
             
             
             ////////////1、复制对象的属性，从源到目的，其中属性“ObjectName”不能被复制，且每个新生成的对象的RealObjectName是源对象名(如果有名称)+“-C”
             for   (var   i   in   srcObj) {
                //“ObjectName”不能被复制
                if(i == "ObjectName"){
                    continue;
                }
                
                desObj[i] = srcObj[i];
                
                //每个新生成的对象的RealObjectName是源对象名(如果有名称)+“-C”
                if((i == "RealObjectName") && (srcObj[i] != "")){
                    desObj[i] = srcObj[i]+"-C";
                }
                
                //为图片生成新的uuid，否则会在刘欣的服务端报错，因为图片对应唯一的uuid。
                //同时，调用addPicSession，为新的uuid生成对应的图片，在SetProperty_Objects函数中createImg.aspx文件用来取得该图片。
                //srcObj[i] != ""表示picture object不是空，而是装载了具体的图片，有uuid的值（如果没有uuid的值，则新的复制对象也就不需取得uuid）
                if(i == "Uuid" && srcObj[i] != ""){
                    var rtn = webroot_aspx_main_visualEditPanel.getUUID();
                    if (rtn.error!=null) {
                        alert("<%=Resources.VisualTemplate.UUID_Object_Name%>");
                        return;
                    } else {
                        desObj[i] = rtn.value;
                        var ret = com.inventec.template.manager.TemplateManager.addPicSession(desObj[i], srcObj["Value"]);
                        if (ret.error != null) {

                            alert(ret.error.Message);

                        } 
                        
                    }        
                }
                
             }
             //做四舍五入
             desObj.X = Math.round((parseFloat(srcObj.X) - g_MinX)*10)/10 + "";
             desObj.Y = Math.round((parseFloat(srcObj.Y) - g_MinY)*10)/10 + "";
            //clone后的对象，其对应的四个span初始是绿色高亮的
            //判断当前源对象是否是thefirst,如果不是，需要去掉绿色高亮
            if($(this).hasClass("thefirst")){
                cloneObj.addClass("thefirst");
                document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):"+desObj.X)				            
                document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):"+desObj.Y)				            
                window.parent.frames("menu").bTemplateChanged = true;

            }


             ////////////2、复制对象对应的DataObjects的属性，从源到目的
             var desNo, srcNo;
             //在DataObjects中，找到源和目的对应的位置
             for(var kk=0;kk<printTemplateInfo.DataObjects.length;kk++){
                 if(printTemplateInfo.DataObjects[kk].ObjectName == srcObj.ObjectName){
                     srcNo = kk;
                     continue;
                 }
                 if(printTemplateInfo.DataObjects[kk].ObjectName == desObj.ObjectName){
                     desNo = kk;
                     break;
                 }
             }
             //复制，其中属性“ObjectName”不能被复制
             for   (var   i   in   printTemplateInfo.DataObjects[srcNo]) {
                if(i != "ObjectName"){
                    printTemplateInfo.DataObjects[desNo][i] = printTemplateInfo.DataObjects[srcNo][i];
                }
             }

             //根据上述复制完的对象的属性和DataObjects对应属性，设定对象的页面显示
             SetProperty_Objects(cloneObj, desId);

             
             //去掉源对象的选中状态
             $(this).draggable('disable')
                .css({cursor:"auto"})
                .removeClass("yes")
                .removeClass("ui-selected")
                .removeClass("thefirst");
             $(this).children("span").removeClass("yes");
             $(this).children("span").css({"visibility":"hidden", "background-color":"transparent"})
            

        })         
        //将选择框移动至0，0
        g_MaxX = g_MaxX - g_MinX;
        g_MaxY = g_MaxY - g_MinY;
        g_DiffX = 0;
        g_DiffY = 0;
        g_MinX = 0;
        g_MinY = 0;
        
        $("#" + focus_container + "_selectedRectangleArea").css("left",g_MinX)
                                                        .css("top",g_MinY);                   
        HideWait();
        bFreshForPaste = true;
        treeObjects.freshRootNode();
        treeObjects.freshPath = "";
    }


	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

//| Name      :    getPrefixMessage

//| Author         :    Lucy Liu

//| Create Date    :    9/23/2009

//| Description    :    根据flag获取错误信息

//| Input para.    :    

//| Ret value :    

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

	
	
	

    //以下为toolbar工具条按钮状态设定
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	btn_mouse_over
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	鼠标mouseover事件，换图
    //| Input para.	:	imgId
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function btn_mouse_over(imgId){
        var img = document.getElementById(imgId).src;
        var myReg = /-h\.gif$/ig;
        var dimension = img.match(myReg);//"_a0_b"
        if(dimension == null){
            img = img.replace(new RegExp("\\.gif$","g"),"-h.gif");
            document.getElementById(imgId).src = img;
        }
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	btn_mouse_out
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	鼠标mouseout事件，换图
    //| Input para.	:	imgId
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function btn_mouse_out(imgId){
        var img = document.getElementById(imgId).src;
        img = img.replace(new RegExp("-h","g"),"");
        document.getElementById(imgId).src = img;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	set_btn_state
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	根据选择的object的总数，设定toolbar的按钮状态
    //| Input para.	:	g_CountAll（选择的object的总数）
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function set_btn_state(count){
    //debugger;
        if(count == 0){
            set_single_btn_disabled("Property",true);
            set_single_btn_disabled("Copy",true);
            set_single_btn_disabled("Delete",true);
            set_single_btn_disabled("Bottom",true);
            set_single_btn_disabled("Top",true);
            set_single_btn_disabled("Left",true);
            set_single_btn_disabled("Right",true);
            set_single_btn_disabled("Center",true);
            set_single_btn_disabled("Middle",true);
            set_single_btn_disabled("Eddy",true);
        }else if(count == 1){
            set_single_btn_disabled("Property",false);
            set_single_btn_disabled("Copy",false);
            set_single_btn_disabled("Delete",false);
            set_single_btn_disabled("Bottom",true);
            set_single_btn_disabled("Top",true);
            set_single_btn_disabled("Left",true);
            set_single_btn_disabled("Right",true);
            set_single_btn_disabled("Center",true);
            set_single_btn_disabled("Middle",true);
            set_single_btn_disabled("Eddy",true);
            //只有文本框可以旋转
            $("#" + focus_container + " .ui-selected").each(function(){
                var id = $(this).attr("id");
                if(id.indexOf("TextObjects") > 0 || id.indexOf("BarcodeObjects") > 0 || id.indexOf("PictureObjects") > 0 || id.indexOf("LineObjects") > 0){
                    set_single_btn_disabled("Eddy",false);
                }
            })
        }else{//>=2
            //没有thefirst高亮绿色，则对齐按钮不可用
//            if($("#"+focus_container+" div").filter(".thefirst").size() == 0){
//                set_single_btn_disabled("Bottom",true);
//                set_single_btn_disabled("Top",true);
//                set_single_btn_disabled("Left",true);
//                set_single_btn_disabled("Right",true);
//                set_single_btn_disabled("Center",true);
//                set_single_btn_disabled("Middle",true);
//            }else{
                set_single_btn_disabled("Bottom",false);
                set_single_btn_disabled("Top",false);
                set_single_btn_disabled("Left",false);
                set_single_btn_disabled("Right",false);
                set_single_btn_disabled("Center",false);
                set_single_btn_disabled("Middle",false);
//            }
            set_single_btn_disabled("Property",true);
            set_single_btn_disabled("Copy",false);
            set_single_btn_disabled("Delete",false);
            set_single_btn_disabled("Eddy",true);
        }
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	set_single_btn_disabled
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	根据id，设定一个按钮的状态
    //| Input para.	:	id，待设定的状态
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function set_single_btn_disabled(id,bDisabled){
        var img = document.getElementById("img"+id).src;
        if(bDisabled == true){
            img = img.replace(new RegExp("-d","g"),"");
            //有可能在高亮的时候，改变状态
            img = img.replace(new RegExp("-h","g"),"");
            img = img.replace(new RegExp("\\.gif$","g"),"-d.gif");
        }else{
            img = img.replace(new RegExp("-d","g"),"");
        }
        document.getElementById("img"+id).src = img;
        document.getElementById("btn"+id).disabled = bDisabled;
    }
    //toolbar工具条按钮状态设定 end



    //以下为toolbar按钮响应处理函数

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	idToObj
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	根据id，转对象
    //| Input para.	:	id
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function idToObj(id){
        var tmp;
        tmp = id.replace(new RegExp("_a","g"),"[");
        tmp = tmp.replace(new RegExp("_b","g"),"]");
        tmp = tmp.replace(new RegExp("_dot_","g"),".");
        return eval(tmp);
    }
    

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	setProperty
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	根据选中的对象，打开对应的属性设定对话框
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function setProperty(){
        window.parent.frames("menu").bTemplateChanged = true;
        
	    $("#"+focus_container+" .ui-selected").each(function(){
	        var id = $(this).attr("id");                                //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
	        var dialogWindow;
	        if(id.indexOf("LineObjects") > 0){
    	    
	            dialogWindow = "../template/LineProperty.aspx";
    	        
	        }else if(id.indexOf("TextObjects") > 0){
    	    
	            dialogWindow = "../template/TextProperty.aspx";
    	        
	        }else if(id.indexOf("BarcodeObjects") > 0){
    	        //$(this).css({"background":"none","font-size":"0"});
                //$(this).prepend("<img src='barcode.gif'>");
	            dialogWindow = "../template/BarcodeProperty.aspx";
    	        
	        }else if(id.indexOf("PictureObjects") > 0){
    	    
	            dialogWindow = "../template/PictureProperty.aspx";
    	        
	        }else if(id.indexOf("RectangleObjects") > 0){
    	    
	            dialogWindow = "../template/RectangleProperty.aspx";
    	        
	        }else if(id.indexOf("ArearObjects") > 0){
    	    
	            dialogWindow = "../template/AreaProperty.aspx";
    	        
	        }

            var diagArgs = new Object();
            diagArgs.id = id;
            diagArgs.pixel_per_inch_x =  pixel_per_inch_x;            
            diagArgs.pixel_per_inch_y =  pixel_per_inch_y;
            
            printTemplateXMLAndHtmlInfo.toJSON = function(){return toJSON(this);};
            var ret = com.inventec.template.manager.TemplateManager.saveSession(printTemplateXMLAndHtmlInfo);
            if (ret.error != null) {
                alert(ret.error.Message);
                return;
            } 

            //debugger;
            var ret = window.showModalDialog(dialogWindow, diagArgs, "dialogWidth:480px;dialogHeight:370px;center:yes;scroll:off;status:no;help:no")
            //debugger;
            
            
            if (typeof(ret) != "undefined"){
                var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
                if (ret.error != null) {
                    alert(ret.error.Message);
                    return;
                } else{
                    printTemplateXMLAndHtmlInfo = ret.value; 
                    printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;
                }

                SetProperty_Objects($(this), id);
	            
                //刷新object树，因为可能object会更名。先暂存并清空focus_container，是因为treeObjects.freshRootNode()会触发tree的nodeclick事件，default时会loseFocus，当前高亮消失
                var tmpContainer = focus_container;
                focus_container = "";
                treeObjects.freshRootNode();
                treeObjects.freshPath = "";
                focus_container = tmpContainer;
            }
        })      
    }

    function SetProperty_Objects(objThis, id){
        var objId = idToObj(id);

        objThis.css("left",mmToPixel_x(objId.X));
        objThis.css("top",mmToPixel_y(objId.Y));
    
        if(id.indexOf("LineObjects") > 0){
	    
            //objThis.css("width",objId.Length+"mm");
            //objThis.css("height",objId.Thickness+"mm");
            

            if(objId.Angle == "<%=Constants.ANGLE_90%>"){
            
                var strSpan = '<SPAN class="north-resize general yes"></SPAN><SPAN class="south-resize general yes"></SPAN>';
                objThis.css({"height":objId.Length+"mm","width":objId.Thickness+"mm"})
                       .html(strSpan);
                       
            }else if(objId.Angle == "<%=Constants.ANGLE_180%>"){

                var strSpan = '<SPAN class="west-resize general yes"></SPAN><SPAN class="east-resize general yes"></SPAN>';
                objThis.css({"width":objId.Length+"mm","height":objId.Thickness+"mm"})
                       .html(strSpan);

            }else if(objId.Angle == "<%=Constants.ANGLE_270%>"){

                var strSpan = '<SPAN class="north-resize general yes"></SPAN><SPAN class="south-resize general yes"></SPAN>';
                objThis.css({"height":objId.Length+"mm","width":objId.Thickness+"mm"})
                       .html(strSpan);

            }else if(objId.Angle == "<%=Constants.ANGLE_0%>"){

                var strSpan = '<SPAN class="west-resize general yes"></SPAN><SPAN class="east-resize general yes"></SPAN>';
                objThis.css({"width":objId.Length+"mm","height":objId.Thickness+"mm"})
                       .html(strSpan);

            }
        }else if(id.indexOf("TextObjects") > 0){
	    
            
	        objThis.css({"font-family":objId.Font});//Arial
	        objThis.css({"font-size":objId.Size+"pt"});//对应属性字体大小
	        switch(objId.TextStyle){
	            case "<%=Constants.FONT_STYLE_NORMAL%>":
    	            objThis.css({"font-style":"normal","font-weight":"normal"});
	                break;
	            case "<%=Constants.FONT_STYLE_BOLD%>":
    	            objThis.css({"font-style":"normal","font-weight":"bold"});
	                break;
	            case "<%=Constants.FONT_STYLE_ITALIC%>":
    	            objThis.css({"font-style":"italic","font-weight":"normal"});//italic
	                break;
	            case "<%=Constants.FONT_STYLE_BOLDITALIC%>":
    	            objThis.css({"font-style":"italic","font-weight":"bold"});//oblique
	                break;
	            
	        }

	        if(objId.Inverse == ""){//正常
	            objThis.css({"background-color":"white"});
	            objThis.css({"color":"black"});
	        }else{
	            objThis.css({"background-color":"black"});//对应属性反白显示Inverse
	            objThis.css({"color":"white"});//对应属性反白显示Inverse
	        }
	        
            var strFilter_90 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=1)";
            var strFilter_180 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=2)";
            var strFilter_270 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=3)";
            var strFilter_0 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=0)";
    	
            if(objId.Angle == "<%=Constants.ANGLE_90%>"){
	            objThis.css("filter", strFilter_90);
            }else if(objId.Angle == "<%=Constants.ANGLE_180%>"){
	            objThis.css("filter", strFilter_180);
            }else if(objId.Angle == "<%=Constants.ANGLE_270%>"){
	            objThis.css("filter", strFilter_270);
            }else{
	            objThis.css("filter", strFilter_0);
	            //this.style.filter = strFilter_0;
            }
	        
            for(var kk=0;kk<printTemplateInfo.DataObjects.length;kk++){
                if(printTemplateInfo.DataObjects[kk].ObjectName == objId.ObjectName){
                    if(printTemplateInfo.DataObjects[kk].DisplayTxt == ""){
                        printTemplateInfo.DataObjects[kk].DisplayTxt = "Text";//可能从属性页传来空串""
                    }
                    //this.innerHTML = this.innerHTML.replace(this.innerText,printTemplateInfo.DataObjects[kk].DisplayTxt);//对应属性，动态修改内容
                    var strSpan = '<SPAN class="north-resize general yes"></SPAN><SPAN class="west-resize general yes"></SPAN><SPAN class="east-resize general yes"></SPAN><SPAN class="south-resize general yes"></SPAN>';
                    objThis.html(printTemplateInfo.DataObjects[kk].DisplayTxt + strSpan);
                    break;
                }
            }
            //bug no:ITC-992-0005
            //reason:设定模板宽高时，考虑object的宽高 
            //2009/9/23从属性页设定，所以我不再设定
            //问题原因：TEXT旋转后，不越界，但进入Template Property Setting编辑窗口时，却报越界，无法再编辑Template Property
            //objId.Width = pixelToMm_x(this.offsetWidth)+"";
            //objId.Height = pixelToMm_y(this.offsetHeight)+"";
            
        }else if(id.indexOf("BarcodeObjects") > 0){
	    
	        var showtext;
            //objThis.css("height",objId.Height+"mm");
            if((objId.HumanReadable).length == 0){//false
                showtext = "OFF";
            }else{
                showtext = "ON";
            }

            //bug no:ITC-992-0090
            //reason:没有做缩放
            for(var kk=0;kk<printTemplateInfo.DataObjects.length;kk++){
                if(printTemplateInfo.DataObjects[kk].ObjectName == objId.ObjectName){
                    //var createimg = "CreateImgVB.aspx?Data=" + printTemplateInfo.DataObjects[kk].DisplayTxt + "&BackColor=White&BarColor=Black&CheckDigit=&CheckDigitToText=&NarrowBarWidth="+objId.NarrowBarWidth+"&Orientation=0&Symbology="+objId.Symbology+"&ShowText=" + showtext + "&Wide2NarrowRatio="+objId.Ratio+"&BarHeight="+objId.Height;
                    objId.BarcodeValue = printTemplateInfo.DataObjects[kk].DisplayTxt;
                    var ret = webroot_aspx_main_visualEditPanel.getImage(objId,printTemplateInfo.PrinterDpi);
                    if (ret.error != null) {
                        alert(ret.error.Message);
                        return;
                    }else{
                        var strRtn = ret.value;
                        var arrImageSize = strRtn.split("&");
                        var width = arrImageSize[0];
                        var height = arrImageSize[1];
                        var verticalResolution = arrImageSize[2];
                        var horizontalResolution = arrImageSize[3];
                        
                        
                        var myWidth=width*(pixel_per_inch_x/horizontalResolution);//width+"mm";
                        var myHeight=height*(pixel_per_inch_y/verticalResolution);//height+"mm";

                        /*
                        //g_arrBarcodeWidthHeight是临时存储变量，用于多选时(函数getWidthHeightById使用)，得到barcode的宽、高
                        $.each(g_arrBarcodeWidthHeight, function(index,callback){
                            if(g_arrBarcodeWidthHeight[index].id == id){
                                g_arrBarcodeWidthHeight[index].width = pixelToMm_x(myWidth);
                                g_arrBarcodeWidthHeight[index].height = pixelToMm_y(myHeight);
                                return false;
                            }
                        });
                        */
                        //var myWidth=width;
                        //var myHeight=height;
                        
                        var createimg = "../../../webroot/aspx/main/MyCreateBarcode.aspx?BarcodeValue=" + encodeURIComponent(printTemplateInfo.DataObjects[kk].DisplayTxt) +"&IsPageNum="+objId.IsPageNum +"&HumanReadable="+objId.HumanReadable+"&NarrowBarWidthPixel="+objId.NarrowBarWidthPixel+"&Angle="+objId.Angle+"&Symbology="+objId.Symbology+ "&Ratio="+objId.Ratio+"&Height="+objId.Height+"&PrinterDpi="+printTemplateInfo.PrinterDpi+"&font="+objId.TextFont+"&fontsize="+objId.TextSize+"&fontstyle="+objId.TextStyle+"&inverse="+objId.Inverse;


                        objThis.css({"width":"1","height":"1"});

                        objThis.children("img").each(function(){
                        //$("img", this).each(function(){
                            $(this).attr("src","");
                        
                            $(this).css({"width":myWidth,"height":myHeight});
                            $(this).attr("src",createimg);
                        })
                        

                        
                        var strFilter_90 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=1)";
                        var strFilter_180 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=2)";
                        var strFilter_270 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=3)";
                        var strFilter_0 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=0)";
                	
                        if(objId.Angle == "<%=Constants.ANGLE_90%>"){
	                        objThis.css("filter", strFilter_90);
                        }else if(objId.Angle == "<%=Constants.ANGLE_180%>"){
	                        objThis.css("filter", strFilter_180);
                        }else if(objId.Angle == "<%=Constants.ANGLE_270%>"){
	                        objThis.css("filter", strFilter_270);
                        }else{
	                        objThis.css("filter", strFilter_0);
	                        //this.style.filter = strFilter_0;
                        }                         
                    }
                    


                    break;

                }
            }

	        
        }else if(id.indexOf("PictureObjects") > 0){
	        if(objId.Uuid == ""){
	            return;
            }
	        objId.Width = pixelToMm_x(objId.WidthPixel)+"";
	        objId.Height = pixelToMm_y(objId.HeightPixel)+"";
	        objThis.css({"background":"none"});
            objThis.css({"background":"url('../template/CreateImg.aspx?name=" + objId.Uuid+"')","background-repeat":"no-repeat"})
                    .css("width",objId.Width+"mm")
                    .css("height",objId.Height+"mm");////////////////应该是px
            //debugger;
            //$("#imgPicture", this)[0].src="../template/CreateImg.aspx?name=" + objId.Uuid;
            //$("#imgPicture", this)[0].style.display = "";

            //2009/9/23在Picture Property Seeting中设置Picture的角度不起效果，必须点击旋转按钮，页面显示旋转了，但Seting窗口显示角度仍为0，打印出来后也不是旋转效果
            var strFilter_90 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=1)";
            var strFilter_180 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=2)";
            var strFilter_270 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=3)";
            var strFilter_0 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=0)";
    	
            if(objId.Angle == "<%=Constants.ANGLE_90%>"){
	            objThis.css("filter", strFilter_90);
            }else if(objId.Angle == "<%=Constants.ANGLE_180%>"){
	            objThis.css("filter", strFilter_180);
            }else if(objId.Angle == "<%=Constants.ANGLE_270%>"){
	            objThis.css("filter", strFilter_270);
            }else{
	            objThis.css("filter", strFilter_0);
            }
	        
        }else if(id.indexOf("RectangleObjects") > 0){
	    
            objThis.css("border-width",objId.Border+"mm");
            objThis.css("width",objId.Width+"mm");
            objThis.css("height",objId.Height+"mm");
            objThis.css("background-color",objId.BackColor);
	        
        }else if(id.indexOf("ArearObjects") > 0){
	    
            objThis.css("width",objId.Width+"mm");
            objThis.css("height",objId.Height+"mm");
	        
        }

        //与粘贴方法共用，所有需要把不是thefirst的对象变为选中但是非绿色高亮显示
        //粘贴clone后的对象，其对应的四个span初始是绿色高亮的
        //判断当前目的对象是否是thefirst,如果不是，需要去掉绿色高亮
        if(!objThis.hasClass("thefirst")){
            objThis.children("span").css({"visibility":"visible","background-color":"transparent"});
            //objThis.children("span").removeClass("yes");
        }
        
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	delSelected
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	删除选中的对象
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function delSelected(){
        window.parent.frames("menu").bTemplateChanged = true;
	    //////////////////////////////alert($("#header_container").html());
	    //alert(focus_container);
	    $("#"+focus_container+" .ui-selected").each(function(){
	        var id = $(this).attr("id");                                //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
            //重新整理结构
            rearrangeStructureForDelete(id);

            /*
            //删除g_arrBarcodeWidthHeight
		    if(id.indexOf("BarcodeObjects") > 0){
                $.each(g_arrBarcodeWidthHeight, function(index,callback){
                        if(g_arrBarcodeWidthHeight[index].id == id){
                            g_arrBarcodeWidthHeight.splice(index,1);
                            return false;
                        }
                });
		    }
		    */
	    })
        set_btn_state(0);

        g_CountAll = 0;
        $("#" + focus_container + "_selectedRectangleArea").replaceWith("");

        //刷新object树，因为可能object会更名。先暂存并清空focus_container，是因为treeObjects.freshRootNode()会触发tree的nodeclick事件，default时会loseFocus，当前高亮消失
        var tmpContainer = focus_container;
        focus_container = "";
        treeObjects.freshRootNode();
        treeObjects.freshPath = "";
        focus_container = tmpContainer;
        
    }
    
    
    
    //rearrangeStructureForDelete与rearrangeStructureForFirst的区别：
    //在于Delete要把dataobjects的对应项删除
    //First不需要，因为生成的是临时结构，不是删除
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	rearrangeStructureForDelete
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	重新整理结构，把id对应的元素从结构中删除，并把它下面的元素上移
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function rearrangeStructureForDelete(id){
        var str_saveId = id.replace(new RegExp("_a\\d+_b$","g"),"");//"printTemplateInfo_dot_DetailSections_a0_b_dot_Cells_a2_b_dot_LineObjects"
        var obj_saveId = idToObj(str_saveId);//printTemplateInfo.DetailSections[0].Cells[2].LineObjects
        
        var myReg = /_a\d+_b$/ig;
        var dimension = id.match(myReg);//"_a0_b"
        var tmp = dimension[0].replace("_a","");
        tmp = tmp.replace("_b","");     //"0"
        tmp = parseInt(tmp);            //0

        //删除这个object
        $("#"+id).replaceWith("");
        
        //修改删除元素下面的元素的id，每个id最后一个[num]变为[num-1]/_a(num-1)_b
        for(var i = tmp+1; i<obj_saveId.length;i++){
            var str_new_id = str_saveId + "_a" + (i-1)+ "_b";
            $("#"+str_saveId + "_a" + i + "_b").attr("id",str_new_id);
        }
        
        //从dataobjects里面删除对应的dataobject
        var objDataObjects = idToObj("printTemplateInfo_dot_DataObjects");
        for(var i = 0;i < objDataObjects.length;i++){
            if(objDataObjects[i].ObjectName == idToObj(id).ObjectName){
                objDataObjects.splice(parseInt(i),1);
                break;
            }
        }
        
        //结构里面删除对象
        obj_saveId.splice(parseInt(tmp),1);   
    }
    

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	rearrangeStructureForFirst
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	重新整理结构，把id对应的元素从结构中删除，并把它下面的元素上移
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function rearrangeStructureForFirst(id){
        var str_saveId = id.replace(new RegExp("_a\\d+_b$","g"),"");//"printTemplateInfo_dot_DetailSections_a0_b_dot_Cells_a2_b_dot_LineObjects"
        var obj_saveId = idToObj(str_saveId);//printTemplateInfo.DetailSections[0].Cells[2].LineObjects
        
        var myReg = /_a\d+_b$/ig;
        var dimension = id.match(myReg);//"_a0_b"
        var tmp = dimension[0].replace("_a","");
        tmp = tmp.replace("_b","");     //"0"
        tmp = parseInt(tmp);            //0
        
        
        //修改删除元素下面的元素的id，每个id最后一个[num]变为[num-1]/_a(num-1)_b
        $("#"+id).replaceWith("");
        
        for(var i = tmp+1; i<obj_saveId.length;i++){
            var str_new_id = str_saveId + "_a" + (i-1)+ "_b";
            $("#"+str_saveId + "_a" + i + "_b").attr("id",str_new_id);
        }
        
        //结构里面删除对象
        obj_saveId.splice(parseInt(tmp),1);   
    }
        

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	alignBottom
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	选中的object底对齐
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function alignBottom(){
        window.parent.frames("menu").bTemplateChanged = true;

//	    var maxIndex = $("#"+focus_container+" div").filter(".ui-selected").size() - 1;
	    var reference = $("#"+focus_container+" div").filter(".thefirst");
	    var ref_top = reference.offset().top;
	    var ref_bottom = ref_top + reference.height();
	    
        No = 0;
        getSelectedBoxPosition(idToObj(reference.attr("id")), reference.attr("id"));
        
	    $("#"+focus_container+" div").filter(".ui-selected").not(".thefirst").each(function(){
            var curId = $(this).attr("id");         //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
            var curObj = idToObj(curId);
		    var each_top = $(this).offset().top;
		    var each_bottom = each_top + $(this).height();
		    var diff_bottom = ref_bottom - each_bottom;
		    each_top = each_top + diff_bottom;
		    //< 不是 <=，因为没有误差
		    if((each_top-$(this).offsetParent().offset().top) < 0){
                cssobj = {"background-color":"rgb(255,0,0)"};
                $("span",this).css(cssobj);
		    }else{
		        $(this).css("top",each_top-$(this).offsetParent().offset().top);
		        curObj.Y = pixelToMm_XY_Y(($(this).css("top"))) + "";
		    }

            getSelectedBoxPosition(curObj, curId);

	    })
	    
        //以下设定虚拟矩形选择区域的大小和位置
        theWidth = g_MaxX - g_MinX;
        theHeight = g_MaxY - g_MinY;
        $("#" + focus_container + "_selectedRectangleArea").css("left",mmToPixel_x(g_MinX))
                                                           .css("top",mmToPixel_y(g_MinY))
                                                           .css("width",mmToPixel_x(theWidth))
                                                           .css("height",mmToPixel_y(theHeight));
	    
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	alignTop
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	选中的object顶对齐
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function alignTop(){
        window.parent.frames("menu").bTemplateChanged = true;

	    var reference = $("#"+focus_container+" div").filter(".thefirst");
	    var mm_ref_top = idToObj(reference.attr("id")).Y;
	    var pixel_ref_top = mmToPixel_XY_Y(mm_ref_top);

        No = 0;
        getSelectedBoxPosition(idToObj(reference.attr("id")), reference.attr("id"));

	    $("#"+focus_container+" div").filter(".ui-selected").not(".thefirst").each(function(){
            var curId = $(this).attr("id");         //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
            var curObj = idToObj(curId);

		    if((pixel_ref_top+$(this).height()) > $(this).offsetParent().height()){
                cssobj = {"background-color":"rgb(255,0,0)"};
                $("span",this).css(cssobj);
		    }else{
		        $(this).css("top",pixel_ref_top);
		        curObj.Y = mm_ref_top + "";
		    }

            getSelectedBoxPosition(curObj, curId);

	    })
	    
        //以下设定虚拟矩形选择区域的大小和位置
        theWidth = g_MaxX - g_MinX;
        theHeight = g_MaxY - g_MinY;
        $("#" + focus_container + "_selectedRectangleArea").css("left",mmToPixel_x(g_MinX))
                                                           .css("top",mmToPixel_y(g_MinY))
                                                           .css("width",mmToPixel_x(theWidth))
                                                           .css("height",mmToPixel_y(theHeight));
                                                           
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	alignLeft
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	选中的object左对齐
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function alignLeft(){
        window.parent.frames("menu").bTemplateChanged = true;

	    var reference = $("#"+focus_container+" div").filter(".thefirst");
	    var mm_ref_left = idToObj(reference.attr("id")).X;
	    var pixel_ref_left = mmToPixel_XY_X(mm_ref_left);

        No = 0;
        getSelectedBoxPosition(idToObj(reference.attr("id")), reference.attr("id"));

	    $("#"+focus_container+" div").filter(".ui-selected").not(".thefirst").each(function(){
            var curId = $(this).attr("id");         //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
            var curObj = idToObj(curId);

		    if((pixel_ref_left+$(this).width()) > $(this).offsetParent().width()){
                cssobj = {"background-color":"rgb(255,0,0)"};
                $("span",this).css(cssobj);
		    }else{
		        $(this).css("left",pixel_ref_left);
		        curObj.X = mm_ref_left + "";
		    }
		    
            getSelectedBoxPosition(curObj, curId);
	    })

        //以下设定虚拟矩形选择区域的大小和位置
        theWidth = g_MaxX - g_MinX;
        theHeight = g_MaxY - g_MinY;
        $("#" + focus_container + "_selectedRectangleArea").css("left",mmToPixel_x(g_MinX))
                                                           .css("top",mmToPixel_y(g_MinY))
                                                           .css("width",mmToPixel_x(theWidth))
                                                           .css("height",mmToPixel_y(theHeight));
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	alignRight
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	选中的object右对齐
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function alignRight(){
        window.parent.frames("menu").bTemplateChanged = true;

	    var reference = $("#"+focus_container+" div").filter(".thefirst");
	    var ref_left = reference.offset().left;
	    var ref_right = ref_left + reference.width();

        No = 0;
        getSelectedBoxPosition(idToObj(reference.attr("id")), reference.attr("id"));

	    $("#"+focus_container+" div").filter(".ui-selected").not(".thefirst").each(function(){
            var curId = $(this).attr("id");         //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
            var curObj = idToObj(curId);

		    var each_left = $(this).offset().left;
		    var each_right = each_left + $(this).width();
		    var diff_right = ref_right - each_right;
		    each_left = each_left + diff_right;
		    
		    if((each_left-$(this).offsetParent().offset().left) < 0){
                cssobj = {"background-color":"rgb(255,0,0)"};
                $("span",this).css(cssobj);
            }else{
		        $(this).css("left",each_left-$(this).offsetParent().offset().left);
		        curObj.X = pixelToMm_XY_X(($(this).css("left"))) + "";
		    }
		    
            getSelectedBoxPosition(curObj, curId);
	    })

        //以下设定虚拟矩形选择区域的大小和位置
        theWidth = g_MaxX - g_MinX;
        theHeight = g_MaxY - g_MinY;
        $("#" + focus_container + "_selectedRectangleArea").css("left",mmToPixel_x(g_MinX))
                                                           .css("top",mmToPixel_y(g_MinY))
                                                           .css("width",mmToPixel_x(theWidth))
                                                           .css("height",mmToPixel_y(theHeight));
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	alignCenter
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	选中的object Y方向中心对齐
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function alignCenter(){
        window.parent.frames("menu").bTemplateChanged = true;

	    var reference = $("#"+focus_container+" div").filter(".thefirst");
	    var ref_left = reference.offset().left;
	    var ref_center = ref_left + reference.width()/2;

        No = 0;
        getSelectedBoxPosition(idToObj(reference.attr("id")), reference.attr("id"));

	    $("#"+focus_container+" div").filter(".ui-selected").not(".thefirst").each(function(){
            var curId = $(this).attr("id");         //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
            var curObj = idToObj(curId);
		    var each_left = $(this).offset().left;
		    var each_center = each_left + $(this).width()/2;
		    var diff_center = ref_center - each_center;
		    each_left = each_left + diff_center;

		    if((each_left-$(this).offsetParent().offset().left) < 0 || (each_left-$(this).offsetParent().offset().left+$(this).width()) > $(this).offsetParent().width()){
                cssobj = {"background-color":"rgb(255,0,0)"};
                $("span",this).css(cssobj);
            }else{
		        $(this).css("left",each_left-$(this).offsetParent().offset().left);
		        curObj.X = pixelToMm_XY_X(($(this).css("left"))) + "";
		    }

            getSelectedBoxPosition(curObj, curId);
	    })

        //以下设定虚拟矩形选择区域的大小和位置
        theWidth = g_MaxX - g_MinX;
        theHeight = g_MaxY - g_MinY;
        $("#" + focus_container + "_selectedRectangleArea").css("left",mmToPixel_x(g_MinX))
                                                           .css("top",mmToPixel_y(g_MinY))
                                                           .css("width",mmToPixel_x(theWidth))
                                                           .css("height",mmToPixel_y(theHeight));
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	alignMiddle
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	选中的object X方向中心对齐
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function alignMiddle(){
        window.parent.frames("menu").bTemplateChanged = true;

	    var reference = $("#"+focus_container+" div").filter(".thefirst");
	    var ref_top = reference.offset().top;
	    var ref_middle = ref_top + reference.height()/2;

        No = 0;
        getSelectedBoxPosition(idToObj(reference.attr("id")), reference.attr("id"));

	    $("#"+focus_container+" div").filter(".ui-selected").not(".thefirst").each(function(){
            var curId = $(this).attr("id");         //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
            var curObj = idToObj(curId);
		    var each_top = $(this).offset().top;
		    var each_middle = each_top + $(this).height()/2;
		    var diff_middle = ref_middle - each_middle;
		    each_top = each_top + diff_middle;

		    if((each_top-$(this).offsetParent().offset().top) < 0 || (each_top-$(this).offsetParent().offset().top+$(this).height()) > $(this).offsetParent().height()){
                cssobj = {"background-color":"rgb(255,0,0)"};
                $("span",this).css(cssobj);
            }else{
		        $(this).css("top",each_top-$(this).offsetParent().offset().top);
		        curObj.Y = pixelToMm_XY_Y(($(this).css("top"))) + "";
		    }

            getSelectedBoxPosition(curObj, curId);
	    })

        //以下设定虚拟矩形选择区域的大小和位置
        theWidth = g_MaxX - g_MinX;
        theHeight = g_MaxY - g_MinY;
        $("#" + focus_container + "_selectedRectangleArea").css("left",mmToPixel_x(g_MinX))
                                                           .css("top",mmToPixel_y(g_MinY))
                                                           .css("width",mmToPixel_x(theWidth))
                                                           .css("height",mmToPixel_y(theHeight));
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	eddy
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	选中的object顺时针旋转
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function eddy(){
        window.parent.frames("menu").bTemplateChanged = true;

	    var strFilter_90 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=1)";
	    var strFilter_180 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=2)";
	    var strFilter_270 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=3)";
	    var strFilter_0 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=0)";
    	
	    $("#"+focus_container+" div").filter(".ui-selected").each(function(){
		    if(idToObj(this.id).Angle == "" || idToObj(this.id).Angle == "<%=Constants.ANGLE_0%>"){
    		    var each_top = $(this).offset().top;
                var parent_top = $(this).offsetParent().offset().top;

                if((each_top - parent_top) + $(this).width() > $(this).offsetParent().height()){
                    cssobj = {"background-color":"rgb(255,0,0)"};
                    $("span",this).css(cssobj);
                    return false;
                }
                
                if((this.id).indexOf("LineObjects") > 0){
                    var strSpan = '<SPAN class="north-resize general yes"></SPAN><SPAN class="south-resize general yes"></SPAN>';
			        $(this).css({"height":idToObj(this.id).Length+"mm","width":idToObj(this.id).Thickness+"mm"})
			               .html(strSpan);
			    }else{
			        this.style.filter = strFilter_90;
                }

			    idToObj(this.id).Angle = "<%=Constants.ANGLE_90%>";
			    
		    }else if(idToObj(this.id).Angle == "<%=Constants.ANGLE_90%>"){
    		    var each_left = $(this).offset().left;
                var parent_left = $(this).offsetParent().offset().left;

                if((each_left - parent_left) + $(this).height() > $(this).offsetParent().width()){
                    cssobj = {"background-color":"rgb(255,0,0)"};
                    $("span",this).css(cssobj);
                    return false;
                }

                if((this.id).indexOf("LineObjects") > 0){
                    var strSpan = '<SPAN class="west-resize general yes"></SPAN><SPAN class="east-resize general yes"></SPAN>';
			        $(this).css({"width":idToObj(this.id).Length+"mm","height":idToObj(this.id).Thickness+"mm"})
			               .html(strSpan);
                }else{
			        this.style.filter = strFilter_180;
			    }
			    
			    idToObj(this.id).Angle = "<%=Constants.ANGLE_180%>";
			    
		    }else if(idToObj(this.id).Angle == "<%=Constants.ANGLE_180%>"){
    		    var each_top = $(this).offset().top;
                var parent_top = $(this).offsetParent().offset().top;

                if((each_top - parent_top) + $(this).width() > $(this).offsetParent().height()){
                    cssobj = {"background-color":"rgb(255,0,0)"};
                    $("span",this).css(cssobj);
                    return false;
                }

                if((this.id).indexOf("LineObjects") > 0){
                    var strSpan = '<SPAN class="north-resize general yes"></SPAN><SPAN class="south-resize general yes"></SPAN>';
			        $(this).css({"height":idToObj(this.id).Length+"mm","width":idToObj(this.id).Thickness+"mm"})
			               .html(strSpan);
                }else{
			        this.style.filter = strFilter_270;
			    }
			    
			    idToObj(this.id).Angle = "<%=Constants.ANGLE_270%>";
			    
		    }else if(idToObj(this.id).Angle == "<%=Constants.ANGLE_270%>"){
    		    var each_left = $(this).offset().left;
                var parent_left = $(this).offsetParent().offset().left;

                if((each_left - parent_left) + $(this).height() > $(this).offsetParent().width()){
                    cssobj = {"background-color":"rgb(255,0,0)"};
                    $("span",this).css(cssobj);
                    return false;
                }

                if((this.id).indexOf("LineObjects") > 0){
                    var strSpan = '<SPAN class="west-resize general yes"></SPAN><SPAN class="east-resize general yes"></SPAN>';
			        $(this).css({"width":idToObj(this.id).Length+"mm","height":idToObj(this.id).Thickness+"mm"})
			               .html(strSpan);
                }else{
    			    this.style.filter = strFilter_0;
			    }
			    
			    idToObj(this.id).Angle = "<%=Constants.ANGLE_0%>";
			    
		    }
	    })
    }

    function front(){
	    $("#"+focus_container+" div").filter(".ui-selected").each(function(){
		    $(this).css("z-index","1000");
	    })
    }

    function back(){
	    $("#"+focus_container+" div").filter(".ui-selected").each(function(){
	    //alert(this.id);
		    $(this).css("z-index","-1000");
	    })
    }	


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	save
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	保存模板
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function save(){
  	    var dialogWindow = "addChangeList.aspx";
        var diagArgs = new Object();
        var retDescription = window.showModalDialog(dialogWindow, diagArgs, "dialogWidth:300px;dialogHeight:240px;center:yes;scroll:off;status:no;help:no")
        if(retDescription == "cancel"){
            return;
        }

        window.parent.frames("menu").ShowWait();    
        document.getElementById("btnSave").disabled = true;
        ShowWait();
        //alert("tt");
        //存盘之前，把焦点失去
        loseFocus();
        var oriPrintTemplateXMLAndHtmlInfo;
        var oriPrintTemplateInfo;// = objCloneDeep(printTemplateInfo);
        var oriHtml = document.getElementById("editarea").innerHTML;

        //把结构保存到session上
        printTemplateXMLAndHtmlInfo.toJSON = function(){return toJSON(this);};
        var ret = com.inventec.template.manager.TemplateManager.saveSession(printTemplateXMLAndHtmlInfo);
        if (ret.error != null) {
            alert(ret.error.Message);
            HideWait();
            window.parent.frames("menu").HideWait();    
            document.getElementById("btnSave").disabled = false;
            return;
        } 

        //把原来的结构做暂存
        var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
        //alert("IMPORT_SESSION"+ret.value);
        if (ret.error != null) {
            alert(ret.error.Message);
            HideWait();
            window.parent.frames("menu").HideWait();    
            document.getElementById("btnSave").disabled = false;
            return;
        }else{
            oriPrintTemplateXMLAndHtmlInfo = ret.value; 
            oriPrintTemplateInfo = oriPrintTemplateXMLAndHtmlInfo.PrintTemplateInfo;
        }


/*        return;
        //把原来的结构做暂存
        artn = com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
        if (artn.error != null) {
            alert(artn.error.Message);
        }else{
            oriPrintTemplateInfo = artn.value;
            objCloneDeep(oriPrintTemplateInfo,printTemplateInfo);
        }
        printTemplateInfo.TemplateWidth = "100";
//        var oriPrintTemplateInfo = objCloneDeep(printTemplateInfo);
*/
        //begin 把area内的对象，放到area的结构中，并从templateinfo中删除
        var strTemplateRef, objTemplateRef;
        first("printTemplateInfo_dot_PageHeader");

        first("printTemplateInfo_dot_PageFooter");
        //debugger;
        strTemplateRef = "printTemplateInfo_dot_DetailSections";
        for(var i = 0;i < idToObj(strTemplateRef).length;i++){
		    var strTemp = strTemplateRef + "_a" + i + "_b_dot_HeaderArea";//"printTemplateInfo.DetailSections[num].HeaderArea"
            first(strTemp);
		    var num_DataSet = parseInt(printTemplateInfo.DetailSections[i].DateSetNum);//section.DataSetNum
            for(var j = 0;j < num_DataSet;j++){
                var strTemp = strTemplateRef + "_a" + i + "_b_dot_Cells_a" + j + "_b";
                first(strTemp);
            }
        }
        //end

        

        printTemplateXMLAndHtmlInfo.OrignalXml = printTemplateInfo;//转换后，有area层
        printTemplateXMLAndHtmlInfo.PrintTemplateInfo = oriPrintTemplateInfo;//原始的
        
        //把img src的 http://itc-98079/lxltest 替换成 ../../..，解决模板换了server不能生成barcode的问题 start
        //debugger;
        var start = oriHtml.indexOf("src=\"http://");
        if(start != -1){
            var end = oriHtml.indexOf("/",start+12);
            end = oriHtml.indexOf("/",end+1);
            var tmpStr = oriHtml.substr(start+5,end-(start+5));
            oriHtml = oriHtml.replace(new RegExp(tmpStr,"g"),"../../..");
            //end
        }
        
        printTemplateXMLAndHtmlInfo.HtmlInfo = oriHtml;//document.getElementById("editarea").innerHTML;
        printTemplateXMLAndHtmlInfo.toJSON = function(){return toJSON(this);};	    
        
        var artn;
        if(method == "add" || method == "import"){
            artn = com.inventec.template.manager.TemplateManager.createPrintTemplateInfo(printTemplateXMLAndHtmlInfo,parentid,userName,retDescription);
            if (artn.error != null) {
                alert(artn.error.Message);
                //恢复原来的结构
                var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
                if (ret.error != null) {
                    alert(ret.error.Message);
                    //return;
                } else{
                    //恢复原来的结构
                    printTemplateXMLAndHtmlInfo = ret.value; 
                    printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;
                    $("#editarea").html(oriHtml);
                    attachEventsToAllPartsAndObjects();
                }

                //document.getElementById("editarea").innerHTML = oriHtml;
            }else{
                uuid = artn.value;
                //刷新树节点
                //    <bug>
                //        BUG NO:ITC-992-0006 
                //        REASON:建议save后不回到Root根目录下。而是继续停留在Template Edit界面。
                //    </bug>
                
                window.parent.frames("menu").bTemplateChanged = false;
                window.parent.frames("menu").tree.freshCurrentNode(0); 
                window.parent.frames("menu").tree.freshPath[0] = uuid;
                
                //window.parent.frames("menu").tree.searchInChildNodes("uuid", uuid, false);		
            }
        }else{//edit
        //updatePrintTemplateInfo(TemplateXmlAndHtml template, String treeId, String templateId, String parentId, String userName)
            //bug no:ITC-992-0023
            //reason:没有传username
            artn = com.inventec.template.manager.TemplateManager.updatePrintTemplateInfo(printTemplateXMLAndHtmlInfo,uuid,nodeuuid,parentid,userName,retDescription);
            if (artn.error != null) {
                alert(artn.error.Message);
                //恢复原来的结构
                var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
                if (ret.error != null) {
                    alert(ret.error.Message);
                    //return;
                } else{
                    //恢复原来的结构
                    printTemplateXMLAndHtmlInfo = ret.value; 
                    printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;
                    $("#editarea").html(oriHtml);
                    attachEventsToAllPartsAndObjects();
                }

                //document.getElementById("editarea").innerHTML = oriHtml;
            }else{
                //刷新树节点
                /*
                //if filename changed refresh root , else locate
                var latestFileName = objSturcture.FileName;
                var title = document.getElementById("head").innerText; 
                if(title!="")
                {
                    title = title.substring(0, title.length-7);
                }
                if(latestFileName == title)
                {
                    var treeObj = window.parent.frames("menu").tree;
                    treeObj.focusClientNode(treeObj.currentNode.id);
                } 
                else
                {
                     window.parent.frames("menu").tree.freshRootNode();
                }
                */
                window.parent.frames("menu").bTemplateChanged = false;
                window.parent.frames("menu").tree.freshRootNode();
                //window.parent.frames("menu").tree.freshCurrentNode(1); 
            }
        }
        HideWait();
        window.parent.frames("menu").HideWait();    
        document.getElementById("btnSave").disabled = false;
        
        //恢复原来的结构
        //printTemplateInfo = oriPrintTemplateInfo;
        //document.getElementById("title").innerText = printTemplateInfo.FileName;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	saveForTree
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	12/2009
    //| Description	:	保存模板，给树节点调用，与save的区别在于，这个函数——存盘后，不做树结点的定位
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function saveForTree(){
  	    var dialogWindow = "addChangeList.aspx";
        var diagArgs = new Object();
        var retDescription = window.showModalDialog(dialogWindow, diagArgs, "dialogWidth:300px;dialogHeight:240px;center:yes;scroll:off;status:no;help:no")
        if(retDescription == "cancel"){
            return;
        }

        window.parent.frames("menu").ShowWait();    
        document.getElementById("btnSave").disabled = true;
        ShowWait();
        //alert("tt");
        //存盘之前，把焦点失去
        loseFocus();
        var oriPrintTemplateXMLAndHtmlInfo;
        var oriPrintTemplateInfo;// = objCloneDeep(printTemplateInfo);
        var oriHtml = document.getElementById("editarea").innerHTML;

        //把结构保存到session上
        printTemplateXMLAndHtmlInfo.toJSON = function(){return toJSON(this);};
        var ret = com.inventec.template.manager.TemplateManager.saveSession(printTemplateXMLAndHtmlInfo);
        if (ret.error != null) {
            alert(ret.error.Message);
            HideWait();
            window.parent.frames("menu").HideWait();    
            document.getElementById("btnSave").disabled = false;
            return;
        } 

        //把原来的结构做暂存
        var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
        //alert("IMPORT_SESSION"+ret.value);
        if (ret.error != null) {
            alert(ret.error.Message);
            HideWait();
            window.parent.frames("menu").HideWait();    
            document.getElementById("btnSave").disabled = false;
            return;
        }else{
            oriPrintTemplateXMLAndHtmlInfo = ret.value; 
            oriPrintTemplateInfo = oriPrintTemplateXMLAndHtmlInfo.PrintTemplateInfo;
        }


        //begin 把area内的对象，放到area的结构中，并从templateinfo中删除
        var strTemplateRef, objTemplateRef;
        first("printTemplateInfo_dot_PageHeader");

        first("printTemplateInfo_dot_PageFooter");
        //debugger;
        strTemplateRef = "printTemplateInfo_dot_DetailSections";
        for(var i = 0;i < idToObj(strTemplateRef).length;i++){
		    var strTemp = strTemplateRef + "_a" + i + "_b_dot_HeaderArea";//"printTemplateInfo.DetailSections[num].HeaderArea"
            first(strTemp);
		    var num_DataSet = parseInt(printTemplateInfo.DetailSections[i].DateSetNum);//section.DataSetNum
            for(var j = 0;j < num_DataSet;j++){
                var strTemp = strTemplateRef + "_a" + i + "_b_dot_Cells_a" + j + "_b";
                first(strTemp);
            }
        }
        //end

        

        printTemplateXMLAndHtmlInfo.OrignalXml = printTemplateInfo;//转换后，有area层
        printTemplateXMLAndHtmlInfo.PrintTemplateInfo = oriPrintTemplateInfo;//原始的
        
        //把img src的 http://itc-98079/lxltest 替换成 ../../..，解决模板换了server不能生成barcode的问题 start
        //debugger;
        var start = oriHtml.indexOf("src=\"http://");
        if(start != -1){
            var end = oriHtml.indexOf("/",start+12);
            end = oriHtml.indexOf("/",end+1);
            var tmpStr = oriHtml.substr(start+5,end-(start+5));
            oriHtml = oriHtml.replace(new RegExp(tmpStr,"g"),"../../..");
            //end
        }
        
        printTemplateXMLAndHtmlInfo.HtmlInfo = oriHtml;//document.getElementById("editarea").innerHTML;
        printTemplateXMLAndHtmlInfo.toJSON = function(){return toJSON(this);};	    
        
        var artn;
        if(method == "add" || method == "import"){
            artn = com.inventec.template.manager.TemplateManager.createPrintTemplateInfo(printTemplateXMLAndHtmlInfo,parentid,userName,retDescription);
            if (artn.error != null) {
                alert(artn.error.Message);
                //恢复原来的结构
                var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
                if (ret.error != null) {
                    alert(ret.error.Message);
                    //return;
                } else{
                    //恢复原来的结构
                    printTemplateXMLAndHtmlInfo = ret.value; 
                    printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;
                    $("#editarea").html(oriHtml);
                    attachEventsToAllPartsAndObjects();
                }

                //document.getElementById("editarea").innerHTML = oriHtml;
            }
        }else{//edit
        //updatePrintTemplateInfo(TemplateXmlAndHtml template, String treeId, String templateId, String parentId, String userName)
            //bug no:ITC-992-0023
            //reason:没有传username
            artn = com.inventec.template.manager.TemplateManager.updatePrintTemplateInfo(printTemplateXMLAndHtmlInfo,uuid,nodeuuid,parentid,userName,retDescription);
            if (artn.error != null) {
                alert(artn.error.Message);
                //恢复原来的结构
                var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
                if (ret.error != null) {
                    alert(ret.error.Message);
                    //return;
                } else{
                    //恢复原来的结构
                    printTemplateXMLAndHtmlInfo = ret.value; 
                    printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;
                    $("#editarea").html(oriHtml);
                    attachEventsToAllPartsAndObjects();
                }

                //document.getElementById("editarea").innerHTML = oriHtml;
            }
        }
        HideWait();
        window.parent.frames("menu").HideWait();    
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	first
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	被save调用，整理结构，把坐落在area区域内的对象，移放到下层area结构中，同时从上层结构中删除
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function first(strContainerID){
        var objArea = idToObj(strContainerID).ArearObjects;//printTemplateInfo.PageHeader.ArearObjects
        //如果PageHeader有ArearObjects
        if(objArea.length == 0){
            return;
        }       

        for(var i=0;i<objArea.length;i++){
            var left = parseFloat(objArea[i].X);
            var top = parseFloat(objArea[i].Y);
            var right = left + parseFloat(objArea[i].Width);
            var bottom = top + parseFloat(objArea[i].Height);

        
            $("#"+strContainerID+" div").filter(".ui-draggable").each(function(){
                var id = $(this).attr("id");//"printTemplateInfo_dot_PageHeader_dot_TextObjects_a[num]_b"
	            if((id.indexOf("ArearObjects") > 0) || $(this).hasClass("area")){
		            return true;
	            }else{
	                var objObjInTemplate = idToObj(id);//printTemplateInfo.PageHeader.TextObjects[0]
                    var X = objObjInTemplate.X;
                    var Y = objObjInTemplate.Y;
                    //判断是否在Arear区域内
                    if((left < X) && (right > X) && (top < Y) && (bottom > Y)){
                        //取object类型
                        var arrTmp = id.split("_dot_");
                        var strObjType = arrTmp[arrTmp.length-1].replace(new RegExp("_a\\d+_b$","g"),"");//"LineObjects"

                        //在printTemplateInfo结构area中增加对象
                        var strObjInArea = strContainerID + ".ArearObjects[" + i +"]." +strObjType;//"printTemplateInfo.PageHeader.ArearObjects[num].TextObjects"
                        var objObjInArea = idToObj(strObjInArea);
                        
                        var temp = objClone(objObjInTemplate);
                        objObjInArea.push(temp);

                        //定义标志在area区域
                        $(this).addClass("area");//对象所在区域：area


                        //在printTemplateInfo结构原来位置做删除
                        /*
                        var str_saveId = id.replace(new RegExp("_a\\d+_b$","g"),"");//"printTemplateInfo_dot_DetailSections_a0_b_dot_Cells_a2_b_dot_LineObjects"
                        var obj_saveId = idToObj(str_saveId);//printTemplateInfo.DetailSections[0].Cells[2].LineObjects
                        
                        var myReg = /_a\d+_b$/ig;
                        var dimension = id.match(myReg);//"_a0_b"
                        var tmp = dimension[0].replace("_a","");
                        tmp = tmp.replace("_b","");     //"0"
                        tmp = parseInt(tmp);            //0
                        
                        //结构里面删除
                        obj_saveId.splice(parseInt(tmp),1);    */
                        //rearrangeStructureForFirst(id);
                    }    	        
	            }
            })
        }
        
        
        $("#"+strContainerID+" div").filter(".area").each(function(){
            rearrangeStructureForFirst($(this).attr("id"));
            //$(this).removeClass("area");
        })            
    }


    function   objClone(obj)   
    {   
        var result   =   new   Object();   
        for   (var   i   in   obj) {  
            result[i]   =   obj[i]   
        }
        return   result   
    } 


    function   objCloneDeep1(obj)   
    {   
        var result   =   new   Object();   
        for   (var   i   in   obj) { 
            if(typeof(obj[i]) == "object"){
                result[i] = objCloneDeep(obj[i]);
                if(obj[i] != null){
                    result[i].length = obj[i].length;
                }
            }else{
                result[i]   =   obj[i];
            }
        }
        return   result   
    } 


    function   objCloneDeep(obj)   
    {   
        var result;
        if(obj.constructor == Array){
            result = new Array();
        }else{
            result = new Object();
        }
        for   (var   i   in   obj) { 
            if(obj[i] != null && typeof(obj[i]) == "object"){
                if(obj.constructor == Array){
                
                    result.push(objCloneDeep(obj[i]));
                }else{
                    result[i] = objCloneDeep(obj[i]);
                }
            }else{
                result[i]   =   obj[i];
            }
        }
        return   result   
    } 


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	print
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	打印模板
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function print(){
        if(nodeuuid == ""){
            alert("<%=Resources.VisualTemplate.New_Template%>");
            return;
        }
        //ITC-1200-0011
        var ret = window.showModalDialog("../template/Print.aspx", printTemplateInfo, "dialogWidth:400px;dialogHeight:390px;center:yes;scroll:off;status:no;help:no")

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	preview
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	预览模板
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function preview(){
        //bug no:ITC-992-0028
        //reason:结构复制不正确
        //bug no:ITC-992-0012
        //reason:preview能够预览已经编辑但没有存盘的内容
        
        document.getElementById("btnSave").disabled = true;
        
        //把结构保存到session上
        printTemplateXMLAndHtmlInfo.toJSON = function(){return toJSON(this);};
        var ret = com.inventec.template.manager.TemplateManager.saveSession(printTemplateXMLAndHtmlInfo);
        if (ret.error != null) {
            alert(ret.error.Message);
            document.getElementById("btnSave").disabled = false;
            return;
        } 

        
        var oriHtml = document.getElementById("editarea").innerHTML;

        //begin 把area内的对象，放到area的结构中，并从templateinfo中删除
        var strTemplateRef, objTemplateRef;
        first("printTemplateInfo_dot_PageHeader");

        first("printTemplateInfo_dot_PageFooter");
        //debugger;
        strTemplateRef = "printTemplateInfo_dot_DetailSections";
        for(var i = 0;i < idToObj(strTemplateRef).length;i++){
		    var strTemp = strTemplateRef + "_a" + i + "_b_dot_HeaderArea";//"printTemplateInfo.DetailSections[num].HeaderArea"
            first(strTemp);
		    var num_DataSet = parseInt(printTemplateInfo.DetailSections[i].DateSetNum);//section.DataSetNum
            for(var j = 0;j < num_DataSet;j++){
                var strTemp = strTemplateRef + "_a" + i + "_b_dot_Cells_a" + j + "_b";
                first(strTemp);
            }
        }
        //end
        
        ///临时传入x,y的DPI值
        printTemplateInfo.pixel_per_inch_x = pixel_per_inch_x;
        printTemplateInfo.pixel_per_inch_y = pixel_per_inch_y;
        
        var ret = window.showModalDialog("../template/preview.aspx", printTemplateInfo, "dialogWidth:400px;dialogHeight:200px;center:yes;scroll:off;status:no;help:no")
        if(ret != undefined){
            var wHandle = window.showModalDialog("../template/Preview_child.aspx",ret,"dialogWidth:650px;dialogHeight:550px;scroll:off;center:yes;status:no;help:no");
        }
        //恢复原来的结构
        var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
        if (ret.error != null) {
            alert(ret.error.Message);
            document.getElementById("btnSave").disabled = false;
            return;
        } else{
            printTemplateXMLAndHtmlInfo = ret.value; 
            printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;
            $("#editarea").html(oriHtml);
            attachEventsToAllPartsAndObjects();
            document.getElementById("btnSave").disabled = false;
        }
    }



    //以下为拖拽对象的属性定义

    //begin获得模板结构
    /*
    var rtn_XmlAndHtml = com.inventec.template.manager.TemplateManager.getTemplateXmlAndHtmlFromStructure();
    if (rtn_XmlAndHtml.error != null) {
        alert(rtn_XmlAndHtml.error.Message);
    } else {

        //获得模板结构
        printTemplateXMLAndHtmlInfo = rtn_XmlAndHtml.value;
    }	        

    printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;*/
    /*
    var rtn = com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    if (rtn.error != null) {
        alert(rtn.error.Message);
    } else {

        //获得模板结构
        printTemplateInfo = rtn.value;
    }	  */      
    //end获得模板结构



    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	hightlightTreeNode
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	selectable_obj选中节点,droppable_obj新增节点，调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    

    function hightlightTreeNode(id){
    
        var arrTmpPath = new Array();
        var arrLayer = id.split("_dot_");
        switch(arrLayer.length){
            case 3:
                arrTmpPath[2] = arrLayer[0] + "_dot_" + arrLayer[1];//printTemplateInfo_dot_PageHeader
                arrTmpPath[1] = arrLayer[0] + "_dot_" + arrLayer[1] + "_dot_";//printTemplateInfo_dot_PageHeader_dot_
                break;
            case 4:

                arrTmpPath[3] = arrLayer[0] + "_dot_" + arrLayer[1];//printTemplateInfo_dot_DetailSections_a1_b
                arrTmpPath[2] = arrLayer[0] + "_dot_" + arrLayer[1] + "_dot_" + arrLayer[2];//printTemplateInfo_dot_DetailSections_a1_b_dot_HeaderArea，printTemplateInfo_dot_DetailSections_a1_b_dot_Cells_a0_b
                arrTmpPath[1] = arrLayer[0] + "_dot_" + arrLayer[1] + "_dot_" + arrLayer[2] + "_dot_";//printTemplateInfo_dot_DetailSections_a1_b_dot_HeaderArea_dot_
                break;
        }

        if(id.indexOf("LineObjects") > 0){
	    
            arrTmpPath[1] = arrTmpPath[1] + "LineObjects";//printTemplateInfo_dot_DetailSections_a1_b_dot_HeaderArea_dot_LineObjects
	        
        }else if(id.indexOf("TextObjects") > 0){
	    
            arrTmpPath[1] = arrTmpPath[1] + "TextObjects";
	        
        }else if(id.indexOf("BarcodeObjects") > 0){

            arrTmpPath[1] = arrTmpPath[1] + "BarcodeObjects";
	        
        }else if(id.indexOf("PictureObjects") > 0){
	    
            arrTmpPath[1] = arrTmpPath[1] + "PictureObjects";
	        
        }else if(id.indexOf("RectangleObjects") > 0){
	    
            arrTmpPath[1] = arrTmpPath[1] + "RectangleObjects";
	        
        }else if(id.indexOf("ArearObjects") > 0){
	    
            arrTmpPath[1] = arrTmpPath[1] + "ArearObjects";
	        
        }
        
        arrTmpPath[0] = id;

        treeObjects.freshCurrentNode(6);
        treeObjects.freshPath = arrTmpPath;  
          
    }
    
    function checkIfInSelectedBox(x, y){
        if(x > g_MinX && x < g_MaxX && y > g_MinY && y < g_MaxY){
            return true;
        }
        return false;
    }


    //		$("#header_container").selectable(selectable_obj);
    //		$("#detail_container").selectable(selectable_obj);




    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	draggable_obj
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	draggable
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    //是否使用鼠标的坐标刷新右上的坐标显示，true使用
    var bUpdateXY = true;
    var dragX, dragY;
    draggable_obj = {
        containment: 'parent',
        cursor:'auto',
        start:function(e){
        /*
            if(!$("#" + focus_container + "_selectedRectangleArea").size()){//size > 0        
                return false;
            }*/
        
            window.parent.frames("menu").bTemplateChanged = true;

	        $(this).parent().selectable('disable');//获得焦点后，鼠标开始拖动，屏蔽select功能
	        bUpdateXY = false;//否使用鼠标的坐标刷新右上的坐标显示
        },
        
        drag:function(e){
	        //使用被拖动object的左上角坐标刷新右上的坐标显示
            var x = pixelToMm_XY_X(($(this).css("left")).replace("px",""));
            var y = pixelToMm_XY_Y(($(this).css("top")).replace("px",""));

            document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):"+x)				            
            document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):"+y)				            

        },

        stop:function(){
            //恢复使用鼠标的坐标刷新右上的坐标显示
            bUpdateXY = true;

	        $(this).parent().selectable('enable');//鼠标停止拖动，允许select功能
	        //alert($(this).attr("class"));
	        $(this).addClass("ui-selected");////////////////////？？？？？？
    		
            
            //////////刷新坐标显示，并向结构里面写入坐标,begin
	        //使用被拖动object的左上角坐标刷新右上的坐标显示
            var x = pixelToMm_XY_X(($(this).css("left")).replace("px",""));
            var y = pixelToMm_XY_Y(($(this).css("top")).replace("px",""));
            if(x < 0){
                x = 0;
            }        
            if(y < 0){
                y = 0;
            }        
            x = x + "";
            y = y + "";


            document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):"+x)				            
            document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):"+y)				            

	        //向结构里面写x,y坐标
            var id = $(this).attr("id");
            (idToObj(id)).X = x;//pixelToMm_XY_X(($(this).css("left")).replace("px","")) + "";
            (idToObj(id)).Y = y;//pixelToMm_XY_Y(($(this).css("top")).replace("px","")) + "";
            //////////刷新坐标显示，并向结构里面写入坐标,end


             if(id.indexOf("ArearObjects") > 0){
    	         $(this).css({"z-index":"-9"});
             }
             if(id.indexOf("RectangleObjects") > 0){
    	         $(this).css({"z-index":"-10"});
             }
    		
        }
    };

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	droppable_obj
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	droppable
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
     droppable_obj = {
        accept: '#toolbox > div',
        tolerance: 'fit',
        //activeClass: 'ui-state-hover',
        //hoverClass: 'ui-state-active',
        drop: function(ev, ui) {
            //alert("drop");
            window.parent.frames("menu").bTemplateChanged = true;

	        focus_container = this.id;
	        //alert(focus_container);
	        var obj_clone;
	        var obj_structure;
	        var obj_type;
	        if(ui.draggable.attr("id") == "draggableText_tool"){
    		
		        obj_clone = "#draggableText";
                obj_type = constText;
                
	        }else if(ui.draggable.attr("id") == "draggableLine_tool"){
    		
		        obj_clone = "#draggableLine";
                obj_type = constLine;
                
	        }else if(ui.draggable.attr("id") == "draggableRectangle_tool"){
    		
		        obj_clone = "#draggableRectangle";
                obj_type = constRectangle;
                
	        }else if(ui.draggable.attr("id") == "draggableArea_tool"){
    		
		        obj_clone = "#draggableArea";
                obj_type = constArea;
                
	        }else if(ui.draggable.attr("id") == "draggableBarcode_tool"){
    		
		        obj_clone = "#draggableBarcode";
                obj_type = constBarcode;
                
	        }else if(ui.draggable.attr("id") == "draggablePicture_tool"){
    		
		        obj_clone = "#draggablePicture";
                obj_type = constPicture;
                
	        }
	        var cloneObj = $(obj_clone).clone(true);//ui.draggable.clone();
	        cloneObj.appendTo($(this));


	        cloneObj.draggable(draggable_obj)
	                .dblclick(f_ObjectDblClick)
                    .mousedown(f_ObjectMouseDown)
                    .mouseup(f_ObjectMouseUp)
                    .bind("setBottom", function(event,a){
                   
                        //置底(ArearObject/RectangleObject除外)，在loseFocus中trigger
                        var id = $(this).attr("id");
                        if(!(id.indexOf("ArearObject") > 0 || id.indexOf("RectangleObject") > 0)){
                            $(this).css({"z-index":"0"});
                            $(this).children("span").css({"z-index":"0"});//设置四个得焦点标志
                        }
                    });

	         cloneObj.addClass("ui-selected")
	                 .addClass("yes")
	                 .addClass("thefirst")
	                 .addClass("son")
	                 .removeClass("nodisplay");
            ////////////////////////////document.getElementById(cloneObj.attr("id")).filters[0].addAmbient(255,187,119,100);//选中后的透明效果
    		         
	         cloneObj.css("position", "absolute");
	         var tmp_top = ui.offset.top - cloneObj.offsetParent().offset().top;
	         var tmp_left = ui.offset.left - cloneObj.offsetParent().offset().left;
             //tmp_top = pixelToMm_XY(ui.offset.top - cloneObj.offsetParent().offset().top);//Math.round(parseFloat(tmp_top)/pixel_per_inch*mm_per_inch*10)/10;
             //tmp_left = Math.round(parseFloat(tmp_left)/pixel_per_inch*mm_per_inch*10)/10;
	         cloneObj.css("top",  tmp_top);//drop后，对象位置的top//////////////////px
	         cloneObj.css("left", tmp_left);//drop后，对象位置的left////////////////px
	         cloneObj.css({"cursor":"move"});
  	         cloneObj.css({"z-index":"0"});
             if(obj_type == constArea){
    	         cloneObj.css({"z-index":"-9"});
             }
             if(obj_type == constRectangle){
    	         cloneObj.css({"z-index":"-10"});
             }

	         cloneObj.children("span").addClass("yes");
	         cloneObj.children("span").css({"visibility":"visible","background-color":"rgb(0,255,0)"});//设置四个得焦点标志
    		 
    		 
            //把对象加入到结构中，返回对象的唯一id，也是text/line/barcode/....的objectname，同时也是dataobject.objectname
            var obj_id = fillup_structure(focus_container,obj_type,cloneObj);
            var objTmp = idToObj(obj_id);
            document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):"+objTmp.X)				            
            document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):"+objTmp.Y)				            

            //不能被放入到容器中的情况
            if(obj_id == false){
                $("#draggableArea").replaceWith("");
                return;
            }
            
            cloneObj.attr("id",obj_id);
            
            
            if(obj_type == constBarcode){
                var objId = idToObj(obj_id);
                objId.BarcodeValue = "<%=Constants.BARCODE_DEFAULT_TEXT%>";
                var ret = webroot_aspx_main_visualEditPanel.getImage(objId,printTemplateInfo.PrinterDpi);
                if (ret.error != null) {
                    alert(ret.error.Message);
                    return;
                }else{
                    var strRtn = ret.value;
                    var arrImageSize = strRtn.split("&");
                    var width = arrImageSize[0];
                    var height = arrImageSize[1];
                    var verticalResolution = arrImageSize[2];
                    var horizontalResolution = arrImageSize[3];
                    
                    
                    var myWidth=width*(pixel_per_inch_x/horizontalResolution);//width+"mm";
                    var myHeight=height*(pixel_per_inch_y/verticalResolution);//height+"mm";

                    /*
                    //g_arrBarcodeWidthHeight是临时存储变量，用于多选时(函数getWidthHeightById使用)，得到barcode的宽、高
                    var objTmp = new Object();
                    //alert(obj_id);
                    objTmp.id = obj_id;
                    objTmp.width = pixelToMm_x(myWidth);
                    objTmp.height = pixelToMm_y(myHeight);
                    g_arrBarcodeWidthHeight.push(objTmp);
                    */
                    
                    var createimg = "../../../webroot/aspx/main/MyCreateBarcode.aspx?BarcodeValue=" + "<%=Constants.BARCODE_DEFAULT_TEXT%>" +"&IsPageNum="+objId.IsPageNum +"&HumanReadable="+objId.HumanReadable+"&NarrowBarWidthPixel="+objId.NarrowBarWidthPixel+"&Angle="+objId.Angle+"&Symbology="+objId.Symbology+ "&Ratio="+objId.Ratio+"&Height="+objId.Height+"&PrinterDpi="+printTemplateInfo.PrinterDpi+"&font="+objId.TextFont+"&fontsize="+objId.TextSize+"&fontstyle="+objId.TextStyle+"&inverse="+objId.Inverse;


                    $("#" + obj_id).css({"width":"1","height":"1"});

                    $("#" + obj_id + " img").each(function(){
                        $(this).attr("src","");
                    
                        $(this).css({"width":myWidth,"height":myHeight});
                        $(this).attr("src",createimg);
                        //$(this).css("background","url(" + createimg +")");
                    })  
                }          
            }         
               
            //设定操作object的按钮
            set_btn_state(1);
            
            //设定选中一个时的各个全局量，用于对象复制
            g_CountAll = 1;
            var tmpObj = idToObj(obj_id);
            g_MinX = parseFloat(tmpObj.X);
            g_MinY = parseFloat(tmpObj.Y);
            var tmp = getWidthHeightById(obj_id).split("&");
            g_MaxX = g_MinX + parseFloat(tmp[0]);//width
            g_MaxY = g_MinY + parseFloat(tmp[1]);//height

            
            
            //把这个object插入树中
            hightlightTreeNode(obj_id);

        }
    };


    //以下为工具函数

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	OpenClose
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	打开关闭右边的区域
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
     function OpenClose()
    {
        var expendPic = "images/nav.gif";
        var srcValue = document.getElementById("shleft").src;
        var from = srcValue.lastIndexOf(expendPic);
        var to = srcValue.length;
        var tmp = srcValue.substring(from, to);
        if (tmp == expendPic) 
        {
		    document.getElementById("shleft").src="../../images/nav2.gif";
		    document.getElementById("toolbox").style.width = "3%";
		    
		    document.getElementById("dataset").style.width = "3%";
		    document.getElementById("objecttree").style.width = "3%";
		    document.getElementById("dataset_button").style.width = "3%";
		    
		    document.getElementById("editarea").style.width = "97%";
		    
		    document.getElementById("tableTitle").style.display="none";
		    document.getElementById("table_dataset_title").style.display="none";
		    document.getElementById("table1").style.display="none";
		    document.getElementById("divTab").style.display="none";
		    
//		    $("#toolbox").css({"overflow":"hidden"});
//		    $("#dataset").css({"overflow":"hidden"});

		    $(".dataset_button").addClass("nodisplay");
		    $(".draggable_tool").addClass("nodisplay");
    		
        }
        else
	    {
		    document.getElementById("shleft").src="../../images/nav.gif";
		    document.getElementById("toolbox").style.width = "23%";
		    
		    document.getElementById("dataset").style.width = "100%";
		    document.getElementById("objecttree").style.width = "100%";
		    document.getElementById("dataset_button").style.width = "100%";

		    document.getElementById("editarea").style.width = "77%";

		    document.getElementById("tableTitle").style.display="";
    	    document.getElementById("table_dataset_title").style.display="";
		    document.getElementById("table1").style.display="";
		    document.getElementById("divTab").style.display="";

		    
//		    $("#toolbox").css({"overflow":"visible"});
//		    $("#dataset").css({"overflow":"visible"});
		    
		    $(".dataset_button").removeClass("nodisplay");
		    $(".draggable_tool").removeClass("nodisplay");
	    }	
    }    


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	highlight_tool
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	鼠标mouseover在objects工具栏中的处理
    //| Input para.	:	tool:
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function highlight_tool(tool){
	    $("#toolbox .draggable_tool").each(function(){	
			    $(this).css({"border":"0px solid #3b85b6","background-color":"white","color":"black"});
    	
	    });
        if(tool != "nothing"){
    	    $(tool).css({"border":"1px solid rgb(50,108,172)","background-color":"rgb(75,167,252)","color":"black"});
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	loseFocus
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	失焦点处理
    //| Input para.	:	tool:
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function loseFocus(){
	    //alert(focus_container);
	    //高亮toolbox的某个工具时，需要把容器里面的焦点失去，下面就是失焦点的处理,注意：失焦点的同时也就不可拖拽了
	    if(focus_container == ""){
	        return;
	    }
	    
        //虚拟选择区域
        $("#" + focus_container + "_selectedRectangleArea").replaceWith("");
	    
	    $("#" + focus_container + " .ui-selected > span").removeClass("yes")
	                                                   .css({"visibility":"hidden","background-color":"transparent"});
	    $("#" + focus_container + " .ui-selected").removeClass("ui-selected")
	                                              .removeClass("yes")
	                                              .removeClass("thefirst")
	                                              .draggable('disable')
	                                              .css({cursor:"auto"})
                                                  .trigger("setBottom",["test"]);
        //g_CountAll = 0;
        set_btn_state(0);
        //$("#" + focus_container + "_selectedRectangleArea").replaceWith("");

        /*
        if(!(type == "ArearObject" || type == "RectangleObject")){
            $("#"+uuid).css({"z-index":"1"})
             $("#"+uuid).children("span").css({"z-index":"1"});//设置四个得焦点标志
        }*/
    	
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	setZero
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2010
    //| Description	:	selected object的总数清0
    //| Input para.	:	tool:
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function setZero(){
    
        g_CountAll = 0;
    }
/*    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	getWidthPerColumn
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	模板宽度，按照列数均分，得到列的宽度（向下取整）
    //| Input para.	:	template_width：模板宽度（px）；num_Column：列数
    //| Ret value	:	列的宽度
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function getWidthPerColumn(template_width, num_Column){
        return Math.floor(parseFloat(template_width)/num_Column);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	mmToPixel_x
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	宽度方向mm转pixel
    //| Input para.	:	numMm
    //| Ret value	:	对应的pixel
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function mmToPixel_x(numMm){
        return Math.floor(parseFloat(numMm)*pixel_per_inch_x/mm_per_inch);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	mmToPixel_y
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	高度方向mm转pixel
    //| Input para.	:	numMm
    //| Ret value	:	对应的pixel
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function mmToPixel_y(numMm){
        return Math.floor(parseFloat(numMm)*pixel_per_inch_y/mm_per_inch);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	pixelToMm_x
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	宽度方向pixel转mm
    //| Input para.	:	numPixel
    //| Ret value	:	对应的mm
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function pixelToMm_x(numPixel){
        return Math.ceil(parseFloat(numPixel)*mm_per_inch/pixel_per_inch_x);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	pixelToMm_y
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	高度方向pixel转mm
    //| Input para.	:	numPixel
    //| Ret value	:	对应的mm
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function pixelToMm_y(numPixel){
        return Math.ceil(parseFloat(numPixel)*mm_per_inch/pixel_per_inch_y);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	pixelToMm_XY_X
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	X坐标pixel转mm
    //| Input para.	:	numPixel
    //| Ret value	:	对应的mm
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function pixelToMm_XY_X(numPixel){
        return Math.ceil(parseFloat(numPixel)*mm_per_inch*10/pixel_per_inch_x)/10;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	pixelToMm_XY_Y
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	Y坐标pixel转mm
    //| Input para.	:	numPixel
    //| Ret value	:	对应的mm
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function pixelToMm_XY_Y(numPixel){
        return Math.ceil(parseFloat(numPixel)*mm_per_inch*10/pixel_per_inch_y)/10;
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	mmToPixel_XY_X
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	X坐标mm转pixel
    //| Input para.	:	numMM
    //| Ret value	:	对应的pixel
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function mmToPixel_XY_X(numMM){
        return Math.floor(parseFloat(numMM)*pixel_per_inch_x*10/mm_per_inch)/10
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	mmToPixel_XY_Y
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	Y坐标mm转pixel
    //| Input para.	:	numMM
    //| Ret value	:	对应的pixel
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function mmToPixel_XY_Y(numMM){
        return Math.floor(parseFloat(numMM)*pixel_per_inch_y*10/mm_per_inch)/10;
        
    }
*/


    				    
			            				        
    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	setTemplate
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	响应模版设定按钮——set template
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function setTemplate(){
        window.parent.frames("menu").bTemplateChanged = true;
        
        var diagArgs = new Object();
        diagArgs.treeId = uuid; //get the treeId
        diagArgs.pixel_per_inch_x =  pixel_per_inch_x;            
        diagArgs.pixel_per_inch_y =  pixel_per_inch_y;
        
        //保存结构到session
        printTemplateXMLAndHtmlInfo.toJSON = function(){return toJSON(this);};
        var ret = com.inventec.template.manager.TemplateManager.saveSession(printTemplateXMLAndHtmlInfo);
        if (ret.error != null) {
            alert(ret.error.Message);
        } 
        
        var ret = window.showModalDialog("../template/TemplateSetting.aspx", diagArgs, "dialogWidth:580px;dialogHeight:450px;center:yes;scroll:off;status:no;menubar:no;toolbar:no;resize:no;help:no");
        //debugger;
        if (typeof(ret) != "undefined"){
            var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
            if (ret.error != null) {
                alert(ret.error.Message);
                return;
            } else{
                printTemplateXMLAndHtmlInfo = ret.value; 
                printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;
            }
        

            //printTemplateInfo
            var strTemplateRef, objTemplateRef;

            var pixel_TemplateWidth = mmToPixel_x(printTemplateInfo.TemplateWidth);
            var pixel_TemplateHeight = mmToPixel_y(printTemplateInfo.TemplateHeight);
            var pixel_PageHeaderAreaHeight = mmToPixel_y(printTemplateInfo.PageHeader.AreaHeight);
            var pixel_PageFooterAreaHeight = mmToPixel_y(printTemplateInfo.PageFooter.AreaHeight);
		    var template_width = pixel_TemplateWidth;//printTemplateInfo.TemplateWidth
		    coor_top = 0;
		    
            //page header
		    if(pixel_PageHeaderAreaHeight == 0){
                $("#page_header_title").replaceWith("");
                $("#printTemplateInfo_dot_PageHeader1").replaceWith("");
            }else{
                if($("#page_header_title").size() == 0){//表示之前没有这个区域
		            var header_title = $("#area_title").clone(true);
		            header_title.attr({"id":"page_header_title"})
		                        .css({"width":template_width,"height":title_height,"top":coor_top})
		                        .removeClass("nodisplay")
		                        .html("Page Header");
                    $("#editarea").append(header_title);
                    
		            coor_top = coor_top + title_height;
		            var header_body = $("#area_body").clone(true);
		            header_body.attr({"id":"printTemplateInfo_dot_PageHeader"})
		                        .css({"width":template_width,"top":0,"height":pixel_PageHeaderAreaHeight})//PageHeader.AreaHeight
		                        .removeClass("nodisplay")
					            .selectable(selectable_obj)
					            .droppable(droppable_obj)
					            .mousemove(f_PageHeaderMouseMove)			
                                .mouseup(f_PageMouseUp);             				    		
            				    		
	                var section_body = $("#area_body").clone(true);
	                section_body.attr({"id":"printTemplateInfo_dot_PageHeader1"})
	                            .css({"border-width":"0","width":template_width,"top":coor_top,"height":pixel_PageHeaderAreaHeight})//AreaHeight
	                            .removeClass("nodisplay");
                    $("#editarea").append(section_body);

                    $("#printTemplateInfo_dot_PageHeader1").append(header_body);
                    printTemplateInfo.PageHeader.ObjectName = "printTemplateInfo_dot_PageHeader";                
                }else{
                    $("#page_header_title").css("width",template_width);
		            coor_top = coor_top + title_height;
                    $("#printTemplateInfo_dot_PageHeader1").css({"width":template_width,"top":coor_top,"height":pixel_PageHeaderAreaHeight});
                    $("#printTemplateInfo_dot_PageHeader").css({"width":template_width,"height":pixel_PageHeaderAreaHeight});
                }
            }

            //sections
            var last_area_height = pixel_PageHeaderAreaHeight;//parseFloat(printTemplateInfo.PageHeader.AreaHeight);
            for(var i = 0;i < printTemplateInfo.DetailSections.length;i++){

                coor_top = coor_top + last_area_height;

                var pixel_DetailSectionsAreaHeight = mmToPixel_y(printTemplateInfo.DetailSections[i].AreaHeight);			    
                if(pixel_DetailSectionsAreaHeight == 0){
	                $("#section" + i + "_title").replaceWith("");
	                $("#section" + i + "_body").replaceWith("");
	                last_area_height = 0;
	                continue;
                }
                


                if(printTemplateInfo.DetailSections[i].Reset != ""){//reset不为空，表示需要重置
	                $("#section" + i + "_title").replaceWith("");
	                $("#section" + i + "_body").replaceWith("");
	                
	                
			        var num_DataSet = parseInt(printTemplateInfo.DetailSections[i].DateSetNum);//section.DataSetNum
			        var num_Column = parseInt(printTemplateInfo.DetailSections[i].ColumnNum);//section.ColumnNum
                    
		            var index = parseInt(printTemplateInfo.DetailSections[i].Index);
		            var section_title = $("#area_title").clone(true);
		            section_title.attr({"id":"section" + index + "_title"})
		                        .css({"width":template_width,"top":coor_top})
		                        .removeClass("nodisplay")
		                        .html("Detail(Section " + (index+1) + ")");
                    $("#editarea").append(section_title);
        			
                
		            coor_top = coor_top + title_height;
		            var section_body = $("#area_body").clone(true);
		            section_body.attr({"id":"section" + index + "_body"})
		                        .css({"width":template_width,"top":coor_top,"height":pixel_DetailSectionsAreaHeight})//AreaHeight
		                        .removeClass("nodisplay")
                    $("#editarea").append(section_body);
        		    
        			
		            //section inner
		            //header
		            var coor_body_inner_top = 0;
		            var inner_header_height = mmToPixel_y(printTemplateInfo.DetailSections[i].HeaderHeight);
		            var inner_cell_height = mmToPixel_y(printTemplateInfo.DetailSections[i].RowHeight);//section.RowHeight

                    var inner_header_height_offset = 0;
                    if(inner_header_height != 0){//新建section header
                        //inner_header_height_offset = 1;
		                var section_body_inner_header = $("#section_cell").clone(true);
		                section_body_inner_header.attr({"id":"printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea"})
		                            .css({"z-index":"10002","width":template_width,"top":coor_body_inner_top,"height":inner_header_height,"border":"solid rgb(204,204,204) 0px"})
		                            .removeClass("nodisplay")
					                .selectable(selectable_obj)
					                .droppable(droppable_obj)
					                .mousemove(f_SectionHeaderMouseMove)			
                                    .mouseup(f_PageMouseUp);             				    		
                        if(parseFloat(printTemplateInfo.DetailSections[i].HeaderHeight) < AREA_LIMITED_MAX_HEIGHT){
                            section_body_inner_header.droppable("option", "tolerance", "pointer");
                        }else{
                            section_body_inner_header.droppable("option", "tolerance", "fit");
                        }

		                var section_body_inner_header1 = $("#section_cell").clone(true);
		                section_body_inner_header1.attr({"id":"printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea1"})
		                            .css({"z-index":"10001","width":template_width,"top":coor_body_inner_top,"height":inner_header_height+inner_header_height_offset,"border":"solid rgb(204,204,204) 0px","border-bottom":"solid rgb(204,204,204) 1px"})
		                            .removeClass("nodisplay");
                        $("#section" + index + "_body").append(section_body_inner_header1);			
					                
                        $("#section" + index + "_body").append(section_body_inner_header);			
					                
                        //$("#printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea1").append(section_body_inner_header);			
                        printTemplateInfo.DetailSections[i].HeaderArea.ObjectName = "printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea";
                    }
                    //cells
                    //cell top
		            coor_body_inner_top = coor_body_inner_top + parseFloat(inner_header_height) + inner_header_height_offset;
		            //cell left
		            var body_inner_cell_left = 0;
		            //得到每个cell的宽度
		            var body_inner_cell_width = getWidthPerColumn(template_width, num_Column);
		            //得到每个cell的高度
		            //var body_inner_cell_height = parseInt(printTemplateInfo.DetailSections[i].RowHeight);//section.RowHeight
		            //循环创建每个cell
		            mytop[i].length = 0;
                    coor_body_inner_top = coor_body_inner_top - inner_cell_height - inner_cell_height_offset;
                    var count_x;
	                if(inner_cell_height != 0){//新建cells
		                for(var j=0;j<num_DataSet;j++){
		                    if(j%num_Column == 0){
		                        count_x = 0;
                                coor_body_inner_top = coor_body_inner_top + inner_cell_height + inner_cell_height_offset;
	                            mytop[i][parseInt(mytop[i].length)] = coor_body_inner_top;
		                    }else{
		                        count_x = count_x + 1;
	                            mytop[i][parseInt(mytop[i].length)] = coor_body_inner_top;
		                    }

		                    //cell的left
		                    body_inner_cell_left = count_x * (body_inner_cell_width + inner_cell_width_offset); 
		                    //创建cell对象
		                    var section_body_inner_cell = $("#section_cell").clone(true);
		                    //创建cell的唯一id
		                    section_body_inner_cell.attr("id","printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+j+"_b")
		                                .css({"z-index":"10002","width":body_inner_cell_width,"top":coor_body_inner_top,"left":body_inner_cell_left,"height":inner_cell_height,"border":"solid rgb(204,204,204) 0px"})
		                                .removeClass("nodisplay")
					                    .selectable(selectable_obj)
					                    .droppable(droppable_obj)
					                    .mousemove(f_SectionCellMouseMove)			
                                        .mouseup(f_PageMouseUp);             				    		
                            if(parseFloat(printTemplateInfo.DetailSections[i].RowHeight) < AREA_LIMITED_MAX_HEIGHT){
                                section_body_inner_cell.droppable("option", "tolerance", "pointer");
                            }else{
                                section_body_inner_cell.droppable("option", "tolerance", "fit");
                            }

				                        
		                    //创建cell对象
		                    var section_body_inner_cell1 = $("#section_cell").clone(true);
		                    //创建cell的唯一id
		                    section_body_inner_cell1.attr("id","printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+j+"_b1")
		                                .css({"z-index":"10001","width":body_inner_cell_width+inner_cell_width_offset,"top":coor_body_inner_top,"left":body_inner_cell_left,"height":inner_cell_height+inner_cell_height_offset,"border":"solid rgb(204,204,204) 0px","border-bottom":"solid rgb(204,204,204) 1px","border-right":"solid rgb(204,204,204) 1px"})
		                                .removeClass("nodisplay");			
				                        
                            $("#section" + index + "_body").append(section_body_inner_cell1);	
                            $("#section" + index + "_body").append(section_body_inner_cell);	
                            
            		        
                            //$("#printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+j+"_b1").append(section_body_inner_cell);	
                            printTemplateInfo.DetailSections[i].Cells[j].ObjectName = "printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+j+"_b";
            		        
                        }
                    }
                }else{//不需要重置，但是有两种情况，1、只改变区域中header和cell的宽高；2、原来没有的区域，新建
			        
			        var num_DataSet = parseInt(printTemplateInfo.DetailSections[i].DateSetNum);//section.DataSetNum
			        var num_Column = parseInt(printTemplateInfo.DetailSections[i].ColumnNum);//section.ColumnNum
                    if($("#section" + i + "_title").size() == 0){//原来没有这个section，需要新建

		                var index = parseInt(printTemplateInfo.DetailSections[i].Index);
		                var section_title = $("#area_title").clone(true);
		                section_title.attr({"id":"section" + index + "_title"})
		                            .css({"width":template_width,"top":coor_top})
		                            .removeClass("nodisplay")
		                            .html("Detail(Section " + (index+1) + ")");
                        $("#editarea").append(section_title);
            			
                    
		                coor_top = coor_top + title_height;
		                var section_body = $("#area_body").clone(true);
		                section_body.attr({"id":"section" + index + "_body"})
		                            .css({"width":template_width,"top":coor_top,"height":pixel_DetailSectionsAreaHeight})//AreaHeight
		                            .removeClass("nodisplay")
                        $("#editarea").append(section_body);
            		    
            			
		                //section inner
		                //header
		                var coor_body_inner_top = 0;
		                var inner_header_height = mmToPixel_y(printTemplateInfo.DetailSections[i].HeaderHeight);
		                var inner_cell_height = mmToPixel_y(printTemplateInfo.DetailSections[i].RowHeight);//section.RowHeight
		                
		                var inner_header_height_offset = 0;
                        if(inner_header_height != 0){//新建section header
                            //inner_header_height_offset = 1;
                        
		                    var section_body_inner_header = $("#section_cell").clone(true);
		                    section_body_inner_header.attr({"id":"printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea"})
		                                .css({"z-index":"10002","width":template_width,"top":coor_body_inner_top,"height":inner_header_height,"border":"solid rgb(204,204,204) 0px"})
		                                .removeClass("nodisplay")
					                    .selectable(selectable_obj)
					                    .droppable(droppable_obj)
					                    .mousemove(f_SectionHeaderMouseMove)	
                                        .mouseup(f_PageMouseUp);             				    		
                            if(parseFloat(printTemplateInfo.DetailSections[i].HeaderHeight) < AREA_LIMITED_MAX_HEIGHT){
                                section_body_inner_header.droppable("option", "tolerance", "pointer");
                            }else{
                                section_body_inner_header.droppable("option", "tolerance", "fit");
                            }
				                        
		                    var section_body_inner_header1 = $("#section_cell").clone(true);
		                    section_body_inner_header1.attr({"id":"printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea1"})
		                                .css({"z-index":"10001","width":template_width,"top":coor_body_inner_top,"height":inner_header_height+inner_header_height_offset,"border":"solid rgb(204,204,204) 0px","border-bottom":"solid rgb(204,204,204) 1px"})
		                                .removeClass("nodisplay");			
                            $("#section" + index + "_body").append(section_body_inner_header1);			
                            $("#section" + index + "_body").append(section_body_inner_header);			
				                        
				                        		
                            $("#printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea1").append(section_body_inner_header);			
                            printTemplateInfo.DetailSections[i].HeaderArea.ObjectName = "printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea";
                        }
                        //cells
                        //cell top
		                coor_body_inner_top = coor_body_inner_top + parseFloat(inner_header_height) + inner_header_height_offset;
		                //cell left
		                var body_inner_cell_left = 0;
		                //得到每个cell的宽度
		                var body_inner_cell_width = getWidthPerColumn(template_width, num_Column);
		                //得到每个cell的高度
		                //var body_inner_cell_height = parseInt(printTemplateInfo.DetailSections[i].RowHeight);//section.RowHeight
		                //循环创建每个cell
		                mytop[i].length = 0;
                        coor_body_inner_top = coor_body_inner_top - inner_cell_height - inner_cell_height_offset;
                        var count_x;
   		                if(inner_cell_height != 0){//新建cells
		                    for(var j=0;j<num_DataSet;j++){
		                        if(j%num_Column == 0){
		                            count_x = 0;
                                    coor_body_inner_top = coor_body_inner_top + inner_cell_height + inner_cell_height_offset;
	                                mytop[i][parseInt(mytop[i].length)] = coor_body_inner_top;
		                        }else{
		                            count_x = count_x + 1;
	                                mytop[i][parseInt(mytop[i].length)] = coor_body_inner_top;
		                        }

		                        //cell的left
		                        body_inner_cell_left = count_x * (body_inner_cell_width + inner_cell_height_offset); 
		                        //创建cell对象
		                        var section_body_inner_cell = $("#section_cell").clone(true);
		                        //创建cell的唯一id
		                        section_body_inner_cell.attr("id","printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+j+"_b")
		                                    .css({"z-index":"10002","width":body_inner_cell_width,"top":coor_body_inner_top,"left":body_inner_cell_left,"height":inner_cell_height,"border":"solid rgb(204,204,204) 0px"})
		                                    .removeClass("nodisplay")
					                        .selectable(selectable_obj)
					                        .droppable(droppable_obj)
					                        .mousemove(f_SectionCellMouseMove)			
                                            .mouseup(f_PageMouseUp);             				    		
                                if(parseFloat(printTemplateInfo.DetailSections[i].RowHeight) < AREA_LIMITED_MAX_HEIGHT){
                                    section_body_inner_cell.droppable("option", "tolerance", "pointer");
                                }else{
                                    section_body_inner_cell.droppable("option", "tolerance", "fit");
                                }
				                            

	                            var section_body_inner_cell1 = $("#section_cell").clone(true);
                                section_body_inner_cell1.attr("id","printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+ j +"_b1")
                                            .css({"z-index":"10001","width":body_inner_cell_width+inner_cell_width_offset,"top":coor_body_inner_top,"left":body_inner_cell_left,"height":inner_cell_height+inner_cell_height_offset,"border":"solid rgb(204,204,204) 0px","border-bottom":"solid rgb(204,204,204) 1px","border-right":"solid rgb(204,204,204) 1px"})
                                            .removeClass("nodisplay");
        			                        			                        
                                $("#section" + index + "_body").append(section_body_inner_cell1);	
                                $("#section" + index + "_body").append(section_body_inner_cell);	
				                            
				                            
                                //$("#printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+ j +"_b1").append(section_body_inner_cell);	
                                printTemplateInfo.DetailSections[i].Cells[j].ObjectName = "printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+j+"_b";
                		        
                            }
                        }
                    //原来没有这个section，需要新建 end
                    }else{
                    //修改一个已有的section
		                //coor_top = coor_top + last_area_height;
		                var index = parseInt(printTemplateInfo.DetailSections[i].Index);
		                var section_title = $("#section" + index + "_title");
		                section_title.css({"width":template_width,"top":coor_top});
            			
                    
		                coor_top = coor_top + title_height;
		                var section_body = $("#section" + index + "_body");
		                section_body.css({"width":template_width,"top":coor_top,"height":pixel_DetailSectionsAreaHeight})//AreaHeight
            		    
            			
		                //section inner
		                //header
		                var coor_body_inner_top = 0;
		                var inner_header_height = mmToPixel_y(printTemplateInfo.DetailSections[i].HeaderHeight);
		                var inner_cell_height = mmToPixel_y(printTemplateInfo.DetailSections[i].RowHeight);//section.RowHeight
		                
                        var inner_header_height_offset = 0;
                        if($("#printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea").size() == 0){//原来没有这个section header(可能因为原来设定的高度为0)，需要新建		                
                            if(inner_header_height != 0){//新建section header
                                //inner_header_height_offset = 1;
                            
		                        var section_body_inner_header = $("#section_cell").clone(true);
		                        section_body_inner_header.attr({"id":"printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea"})
		                                    .css({"z-index":"10002","width":template_width,"top":coor_body_inner_top,"height":inner_header_height,"border":"solid rgb(204,204,204) 0px"})
		                                    .removeClass("nodisplay")
					                        .selectable(selectable_obj)
					                        .droppable(droppable_obj)
					                        .mousemove(f_SectionHeaderMouseMove)
                                            .mouseup(f_PageMouseUp);             				    		
                                if(parseFloat(printTemplateInfo.DetailSections[i].HeaderHeight) < AREA_LIMITED_MAX_HEIGHT){
                                    section_body_inner_header.droppable("option", "tolerance", "pointer");
                                }else{
                                    section_body_inner_header.droppable("option", "tolerance", "fit");
                                }
					                        

		                        var section_body_inner_header1 = $("#section_cell").clone(true);
		                        section_body_inner_header1.attr({"id":"printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea1"})
		                                    .css({"z-index":"10001","width":template_width,"top":coor_body_inner_top,"height":inner_header_height+inner_header_height_offset,"border":"solid rgb(204,204,204) 0px","border-bottom":"solid rgb(204,204,204) 1px"})
		                                    .removeClass("nodisplay");			
                                $("#section" + index + "_body").append(section_body_inner_header1);			
                                $("#section" + index + "_body").append(section_body_inner_header);			
					                        			

                                //$("#printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea1").append(section_body_inner_header);			
                                printTemplateInfo.DetailSections[i].HeaderArea.ObjectName = "printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea";
		                    }
		                }else{//原来有这个section header
                            if(inner_header_height != 0){//修改section header
                                //inner_header_height_offset = 1;

		                        var section_body_inner_header = $("#printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea");
		                        section_body_inner_header.css({"z-index":"10002","width":template_width,"top":coor_body_inner_top,"height":inner_header_height})
                                if(parseFloat(printTemplateInfo.DetailSections[i].HeaderHeight) < AREA_LIMITED_MAX_HEIGHT){
                                    section_body_inner_header.droppable("option", "tolerance", "pointer");
                                }else{
                                    section_body_inner_header.droppable("option", "tolerance", "fit");
                                }

		                        var section_body_inner_header1 = $("#printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea1");
		                        section_body_inner_header1.css({"z-index":"10001","width":template_width,"top":coor_body_inner_top,"height":inner_header_height+inner_header_height_offset})
		                    }else{//除掉section header
	                            $("#printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea").replaceWith("");
	                        }
		                    
                        }
                        //cells
                        //cell top
		                coor_body_inner_top = coor_body_inner_top + parseFloat(inner_header_height) + inner_header_height_offset;
		                //cell left
		                var body_inner_cell_left = 0;
		                //得到每个cell的宽度
		                var body_inner_cell_width = getWidthPerColumn(template_width, num_Column);
		                //循环创建每个cell
		                mytop[i].length = 0;
    		            coor_body_inner_top = coor_body_inner_top - inner_cell_height - inner_cell_height_offset;
        		        var count_x;
                        if($("#printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a0_b").size() == 0){//原来没有section cell(可能因为原来设定的高度为0)		                
    		                if(inner_cell_height != 0){//新建cells
		                        for(var j=0;j<num_DataSet;j++){
		                            if(j%num_Column == 0){
		                                count_x = 0;
                                        coor_body_inner_top = coor_body_inner_top + inner_cell_height + inner_cell_height_offset;
	                                    mytop[i][parseInt(mytop[i].length)] = coor_body_inner_top;
		                            }else{
		                                count_x = count_x + 1;
	                                    mytop[i][parseInt(mytop[i].length)] = coor_body_inner_top;
		                            }

		                            //cell的left
		                            body_inner_cell_left = count_x * (body_inner_cell_width + inner_cell_width_offset); 
		                            //创建cell对象
		                            var section_body_inner_cell = $("#section_cell").clone(true);
		                            //创建cell的唯一id
		                            section_body_inner_cell.attr("id","printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+j+"_b")
		                                        .css({"z-index":"10002","width":body_inner_cell_width,"top":coor_body_inner_top,"left":body_inner_cell_left,"height":inner_cell_height,"border":"solid rgb(204,204,204) 0px"})
		                                        .removeClass("nodisplay")
					                            .selectable(selectable_obj)
					                            .droppable(droppable_obj)
					                            .mousemove(f_SectionCellMouseMove)			
                                                .mouseup(f_PageMouseUp);             				    		
                                    if(parseFloat(printTemplateInfo.DetailSections[i].RowHeight) < AREA_LIMITED_MAX_HEIGHT){
                                        section_body_inner_cell.droppable("option", "tolerance", "pointer");
                                    }else{
                                        section_body_inner_cell.droppable("option", "tolerance", "fit");
                                    }

	                                var section_body_inner_cell1 = $("#section_cell").clone(true);
                                    section_body_inner_cell1.attr("id","printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+ j +"_b1")
                                                .css({"z-index":"10001","width":body_inner_cell_width+inner_cell_width_offset,"top":coor_body_inner_top,"left":body_inner_cell_left,"height":inner_cell_height+inner_cell_height_offset,"border":"solid rgb(204,204,204) 0px","border-bottom":"solid rgb(204,204,204) 1px","border-right":"solid rgb(204,204,204) 1px"})
                                                .removeClass("nodisplay");
            			                        			                        
                                    $("#section" + index + "_body").append(section_body_inner_cell1);	
                                    $("#section" + index + "_body").append(section_body_inner_cell);	


                                    //$("#printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+ j +"_b1").append(section_body_inner_cell);	
                                    printTemplateInfo.DetailSections[i].Cells[j].ObjectName = "printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+j+"_b";
                    		        
                                }    		            
    		                }
                        }else{//原来有section cell
                            var tmp_Cells = printTemplateInfo.DetailSections[i].Cells;
                            var count_x;
    		                if(inner_cell_height != 0){//修改cells
	                            for(var j=0;j<tmp_Cells.length;j++){
	                                if(j%num_Column == 0){
	                                    count_x = 0;
                                        coor_body_inner_top = coor_body_inner_top + inner_cell_height + inner_cell_height_offset;
                                        mytop[i][parseInt(mytop[i].length)] = coor_body_inner_top;
	                                }else{
	                                    count_x = count_x + 1;
                                        mytop[i][parseInt(mytop[i].length)] = coor_body_inner_top;
	                                }

	                                //cell的left
	                                body_inner_cell_left = count_x * (body_inner_cell_width + inner_cell_width_offset); 
	                                //创建cell对象
	                                var section_body_inner_cell = $("#"+tmp_Cells[j].ObjectName);
	                                //
	                                section_body_inner_cell.css({"z-index":"10002","width":body_inner_cell_width,"top":coor_body_inner_top,"left":body_inner_cell_left,"height":inner_cell_height})
                                    if(parseFloat(printTemplateInfo.DetailSections[i].RowHeight) < AREA_LIMITED_MAX_HEIGHT){
                                        section_body_inner_cell.droppable("option", "tolerance", "pointer");
                                    }else{
                                        section_body_inner_cell.droppable("option", "tolerance", "fit");
                                    }


	                                var section_body_inner_cell1 = $("#"+tmp_Cells[j].ObjectName+"1");
	                                //
	                                section_body_inner_cell1.css({"z-index":"10001","width":body_inner_cell_width+inner_cell_width_offset,"top":coor_body_inner_top,"left":body_inner_cell_left,"height":inner_cell_height+inner_cell_height_offset})
                                }    		                
                            }else{//删除cells
	                            for(var j=0;j<tmp_Cells.length;j++){
	                                $("#"+tmp_Cells[j].ObjectName).replaceWith("");
                                }                            
                            }
                        }//原来没有section cell(可能因为原来设定的高度为0) end
                    }//修改一个已有的section end
                }
                last_area_height = pixel_DetailSectionsAreaHeight;	                
                
            }


            //page footer
		    if(pixel_PageFooterAreaHeight == 0){
                $("#page_footer_title").replaceWith("");
                $("#printTemplateInfo_dot_PageFooter1").replaceWith("");
            }else{
                if($("#page_footer_title").size() == 0){//表示之前没有这个区域
		            coor_top = coor_top + last_area_height;
		            var footer_title = $("#area_title").clone(true);
		            footer_title.attr({"id":"page_footer_title"})
		                        .css({"width":template_width,"height":title_height,"top":coor_top})
		                        .removeClass("nodisplay")
		                        .html("Page Footer");
                    $("#editarea").append(footer_title);
                    
		            coor_top = coor_top + title_height;
		            var footer_body = $("#area_body").clone(true);
		            footer_body.attr({"id":"printTemplateInfo_dot_PageFooter"})
		                        .css({"width":template_width,"top":0,"height":pixel_PageFooterAreaHeight})//PageHeader.AreaHeight
		                        .removeClass("nodisplay")
					            .selectable(selectable_obj)
					            .droppable(droppable_obj)
					            .mousemove(f_PageFooterMouseMove)
				                .mousedown(function(e){
				                    //loseFocus();
				                })			
                                .mouseup(f_PageMouseUp);             				    		
            				    			
	                var section_body = $("#area_body").clone(true);
	                section_body.attr({"id":"printTemplateInfo_dot_PageFooter1"})
	                            .css({"border-width":"0","width":template_width,"top":coor_top,"height":pixel_PageFooterAreaHeight})//AreaHeight
	                            .removeClass("nodisplay");
                    $("#editarea").append(section_body);


                    $("#printTemplateInfo_dot_PageFooter1").append(footer_body);            
                    printTemplateInfo.PageFooter.ObjectName = "printTemplateInfo_dot_PageFooter";
                }else{
		            coor_top = coor_top + last_area_height;
                    $("#page_footer_title").css({"width":template_width,"height":title_height,"top":coor_top});
		            coor_top = coor_top + title_height;
                    $("#printTemplateInfo_dot_PageFooter1").css({"width":template_width,"top":coor_top,"height":pixel_PageFooterAreaHeight})    
                    $("#printTemplateInfo_dot_PageFooter").css({"width":template_width,"height":pixel_PageFooterAreaHeight});
                }
            }
            //重新生成barcode
            regenerateAllBarcodes();
            
            //如果focus_container在模板设定中被改掉,需要判断是否还有选中的object
            if((focus_container != "") && ($("#"+focus_container+" div").filter(".ui-selected").size() == 0)){
                g_CountAll = 0;
                set_btn_state(g_CountAll);
            }
            treeObjects.freshRootNode();
            treeObjects.freshPath = "";
            
        }
    }   
    
   
    			
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	createArea
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	10/2009
    //| Description	:	set template模板设定后，按照新的dpi重新生成barcode
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function regenerateAllBarcodes(){
        //g_arrBarcodeWidthHeight = new Array();
        $("div[id*='BarcodeObjects']").each(function(){
            var objId = idToObj($(this).attr("id"));
            
            var ret = webroot_aspx_main_visualEditPanel.getImage(objId,printTemplateInfo.PrinterDpi);
            if (ret.error != null) {
                alert(ret.error.Message);
                return;
            }else{
                var strRtn = ret.value;
                var arrImageSize = strRtn.split("&");
                var width = arrImageSize[0];
                var height = arrImageSize[1];
                var verticalResolution = arrImageSize[2];
                var horizontalResolution = arrImageSize[3];
                
                
                var myWidth=width*(pixel_per_inch_x/horizontalResolution);//width+"mm";
                var myHeight=height*(pixel_per_inch_y/verticalResolution);//height+"mm";

                /*
                var objTmp = new Object();
                //alert(objId);
                objTmp.id = objId;
                objTmp.width = pixelToMm_x(myWidth);
                objTmp.height = pixelToMm_y(myHeight);
                g_arrBarcodeWidthHeight.push(objTmp);
                //var myWidth=width;
                //var myHeight=height;
                */
                var createimg = "../../../webroot/aspx/main/MyCreateBarcode.aspx?BarcodeValue=" + objId.BarcodeValue +"&IsPageNum="+objId.IsPageNum +"&HumanReadable="+objId.HumanReadable+"&NarrowBarWidthPixel="+objId.NarrowBarWidthPixel+"&Angle="+objId.Angle+"&Symbology="+objId.Symbology+ "&Ratio="+objId.Ratio+"&Height="+objId.Height+"&PrinterDpi="+printTemplateInfo.PrinterDpi+"&font="+objId.TextFont+"&fontsize="+objId.TextSize+"&fontstyle="+objId.TextStyle+"&inverse="+objId.Inverse;


                $(this).css({"width":"1","height":"1"});
                /*
                document.getElementById("imgBarcode").style.width=myWidth;//width+"mm";
                document.getElementById("imgBarcode").style.height=myHeight;//height+"mm";
                document.getElementById("imgBarcode").src = createimg;
                */
                $("img", this).each(function(){
                    $(this).attr("src","");
                
                    $(this).css({"width":myWidth,"height":myHeight});
                    $(this).attr("src",createimg);
                    //$(this).css("background","url(" + createimg +")");
                })
            }
            
        })
    }
    				        
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	createArea
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	set template模板设定后，创建编辑区域
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function createArea(){
    /*
            //初始化模板结构begin
            //var printTemplateInfo;
            var rtn = com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
            if (rtn.error != null) {
                alert(rtn.error.Message);
            } else {

                //获得模板结构
                printTemplateInfo = rtn.value;
            }	        


            
            printTemplateInfo.TemplateWidth = "92";//TemplateWidth 350
            printTemplateInfo.TemplateHeight = "79";//TemplateWidth 300
            
            printTemplateInfo.PageHeader.AreaHeight = "13";//50
            printTemplateInfo.PageFooter.AreaHeight = "13";//50

            
            //sections
            for(var i=0;i<2;i++){
                var sectionRtn = com.inventec.template.manager.TemplateManager.getSectionFromStructure();
                if (sectionRtn.error != null) {
                    alert(sectionRtn.error.Message);
                } else {
                    var section = sectionRtn.value;
                    section.AreaHeight = "39";//150
                    section.Index = i+"";
                    section.HeaderHeight = "5";//20
                    section.RowHeight = "8";//30
                    section.DateSetNum = "3";
                    section.ColumnNum = "1";
                    for(var k = 0; k < parseFloat(section.DateSetNum); k++){
                        var sectioncell;
                        var sectioncellRtn = com.inventec.template.manager.TemplateManager.getSectionCellFromStructure();
                        if (sectioncellRtn.error != null) {
                            alert(sectioncellRtn.error.Message);
                        } else {
                            sectioncell = sectioncellRtn.value;
                        }
                        section.Cells.push(sectioncell);
                    }
                    printTemplateInfo.DetailSections.push(section);
                }
            }
            //初始化模板结构end
            */
            
            //创建页面
            var pixel_TemplateWidth = mmToPixel_x(printTemplateInfo.TemplateWidth);
            var pixel_TemplateHeight = mmToPixel_y(printTemplateInfo.TemplateHeight);
            var pixel_PageHeaderAreaHeight = mmToPixel_y(printTemplateInfo.PageHeader.AreaHeight);
            var pixel_PageFooterAreaHeight = mmToPixel_y(printTemplateInfo.PageFooter.AreaHeight);
		    var template_width = pixel_TemplateWidth;//printTemplateInfo.TemplateWidth
		    //var inner_header_height = 30;//section.HeaderHeight
    //			var inner_cell_height = 200;//section.RowHeight
    		
    		
    		
		    //page header
		    if(pixel_PageHeaderAreaHeight != 0){
		        var header_title = $("#area_title").clone(true);
		        header_title.attr({"id":"page_header_title"})
		                    .css({"width":template_width,"height":title_height,"top":coor_top})
		                    .removeClass("nodisplay")
		                    .html("Page Header");
                $("#editarea").append(header_title);
                
		        coor_top = coor_top + title_height;
		        var header_body = $("#area_body").clone(true);
		        header_body.attr({"id":"printTemplateInfo_dot_PageHeader"})
		                    .css({"width":template_width,"top":0,"height":pixel_PageHeaderAreaHeight})//PageHeader.AreaHeight
		                    .removeClass("nodisplay")
					        .selectable(selectable_obj)
					        .droppable(droppable_obj)
					        .mousemove(f_PageHeaderMouseMove)
				            .mousedown(function(e){
				                //loseFocus();
				            })
                            .mouseup(f_PageMouseUp);                    
				            
        				    			
	            var section_body = $("#area_body").clone(true);
	            section_body.attr({"id":"printTemplateInfo_dot_PageHeader1"})
	                        .css({"border-width":"0","width":template_width,"top":coor_top,"height":pixel_PageHeaderAreaHeight})//AreaHeight
	                        .removeClass("nodisplay");
                $("#editarea").append(section_body);

                $("#printTemplateInfo_dot_PageHeader1").append(header_body);
                printTemplateInfo.PageHeader.ObjectName = "printTemplateInfo_dot_PageHeader";
            }
            
            
            //sections
            var last_area_height = pixel_PageHeaderAreaHeight;//parseFloat(printTemplateInfo.PageHeader.AreaHeight);
            for(var i = 0;i < printTemplateInfo.DetailSections.length;i++){

                var pixel_DetailSectionsAreaHeight = mmToPixel_y(printTemplateInfo.DetailSections[i].AreaHeight);			    
                if(pixel_DetailSectionsAreaHeight == 0){
                    continue;
                }
			    var num_DataSet = parseInt(printTemplateInfo.DetailSections[i].DateSetNum);//section.DataSetNum
			    var num_Column = parseInt(printTemplateInfo.DetailSections[i].ColumnNum);//section.ColumnNum
                
		        coor_top = coor_top + last_area_height;
		        var index = parseInt(printTemplateInfo.DetailSections[i].Index);
		        var section_title = $("#area_title").clone(true);
		        section_title.attr({"id":"section" + index + "_title"})
		                    .css({"width":template_width,"top":coor_top})
		                    .removeClass("nodisplay")
		                    .html("Detail(Section " + (index+1) + ")");
                $("#editarea").append(section_title);
    			
            
		        coor_top = coor_top + title_height;
		        var section_body = $("#area_body").clone(true);
		        section_body.attr({"id":"section" + index + "_body"})
		                    .css({"width":template_width,"top":coor_top,"height":pixel_DetailSectionsAreaHeight,"background-color":"white"})//AreaHeight
		                    .removeClass("nodisplay")
                $("#editarea").append(section_body);
    		    
    			
		        //section inner
		        //header
		        var coor_body_inner_top = 0;
		        var inner_header_height = mmToPixel_y(printTemplateInfo.DetailSections[i].HeaderHeight);
	            var inner_cell_height = mmToPixel_y(printTemplateInfo.DetailSections[i].RowHeight);//section.RowHeight
	            var inner_header_height_offset = 0;
		        if(inner_header_height != 0){
                    //inner_header_height_offset = 1;
                    
		            var section_body_inner_header = $("#section_cell").clone(true);
		            section_body_inner_header.attr({"id":"printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea"})
		                        .css({"z-index":"10002","width":template_width,"top":coor_body_inner_top,"height":inner_header_height,"border":"solid rgb(204,204,204) 0px"})
		                        .removeClass("nodisplay")
					            .selectable(selectable_obj)
					            .droppable(droppable_obj)
					            .mousemove(f_SectionHeaderMouseMove)		
                                .mouseup(f_PageMouseUp);                    
                    if(parseFloat(printTemplateInfo.DetailSections[i].HeaderHeight) < AREA_LIMITED_MAX_HEIGHT){
                        section_body_inner_header.droppable("option", "tolerance", "pointer");
                    }else{
                        section_body_inner_header.droppable("option", "tolerance", "fit");
                    }
				                	
		            var section_body_inner_header1 = $("#section_cell").clone(true);
		            section_body_inner_header1.attr({"id":"printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea1"})
		                        .css({"z-index":"10001","width":template_width,"top":coor_body_inner_top,"height":inner_header_height+inner_header_height_offset,"border":"solid rgb(204,204,204) 0px","border-bottom":"solid rgb(204,204,204) 1px"})
		                        .removeClass("nodisplay");			
                    $("#section" + index + "_body").append(section_body_inner_header1);			
                    $("#section" + index + "_body").append(section_body_inner_header);			
		                        
                    //$("#" + "printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea1").append(section_body_inner_header);			
                    printTemplateInfo.DetailSections[i].HeaderArea.ObjectName = "printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_HeaderArea";
                }
                //cells
                //cell top
		        coor_body_inner_top = coor_body_inner_top + parseFloat(inner_header_height) + inner_header_height_offset;
		        //cell left
		        var body_inner_cell_left = 0;
		        //得到每个cell的宽度
		        var body_inner_cell_width = getWidthPerColumn(template_width, num_Column);

		        //得到每个cell的高度
		        //var body_inner_cell_height = parseInt(printTemplateInfo.DetailSections[i].RowHeight);//section.RowHeight
		        //循环创建每个cell
		        mytop[i].length = 0;
		        coor_body_inner_top = coor_body_inner_top - inner_cell_height - inner_cell_height_offset;
		        var count_x;

	            if(inner_cell_height != 0){
		            
		            for(var j=0;j<num_DataSet;j++){
		                if(j%num_Column == 0){
		                    count_x = 0;
                            coor_body_inner_top = coor_body_inner_top + inner_cell_height + inner_cell_height_offset;
	                        mytop[i][parseInt(mytop[i].length)] = coor_body_inner_top;
		                }else{
		                    count_x = count_x + 1;
	                        mytop[i][parseInt(mytop[i].length)] = coor_body_inner_top;
		                }
    		            
		                //cell的left
		                body_inner_cell_left = count_x * (body_inner_cell_width + inner_cell_width_offset); 
    		            
	                    //创建cell对象
	                    var section_body_inner_cell = $("#section_cell").clone(true);
	                    //创建cell的唯一id
	                    section_body_inner_cell.attr("id","printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+ j +"_b")
	                                .css({"z-index":"10002","width":body_inner_cell_width,"top":coor_body_inner_top,"left":body_inner_cell_left,"height":inner_cell_height,"border":"solid rgb(204,204,204) 0px"})
	                                .removeClass("nodisplay")
				                    .selectable(selectable_obj)
				                    .droppable(droppable_obj)
				                    .mousemove(f_SectionCellMouseMove)
                                    .mouseup(f_PageMouseUp);                    
                        if(parseFloat(printTemplateInfo.DetailSections[i].RowHeight) < AREA_LIMITED_MAX_HEIGHT){
                            section_body_inner_cell.droppable("option", "tolerance", "pointer");
                        }else{
                            section_body_inner_cell.droppable("option", "tolerance", "fit");
                        }
			                        
                        			                        
	                    //创建cell对象
	                    var section_body_inner_cell1 = $("#section_cell").clone(true);
	                    //创建cell的唯一id
                        section_body_inner_cell1.attr("id","printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+ j +"_b1")
                                    .css({"z-index":"10001","width":body_inner_cell_width+inner_cell_width_offset,"top":coor_body_inner_top,"left":body_inner_cell_left,"height":inner_cell_height+inner_cell_height_offset,"border":"solid rgb(204,204,204) 0px","border-bottom":"solid rgb(204,204,204) 1px","border-right":"solid rgb(204,204,204) 1px"})
                                    .removeClass("nodisplay");
			                        			                        
                        $("#section" + index + "_body").append(section_body_inner_cell1);	
                        
        		        
                        //$("#" + "printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+ j +"_b1").append(section_body_inner_cell);	
                        $("#section" + index + "_body").append(section_body_inner_cell);	
                        
                        printTemplateInfo.DetailSections[i].Cells[j].ObjectName = "printTemplateInfo_dot_DetailSections_a" + index + "_b_dot_Cells_a"+j+"_b";
        		        
                    }
                }
                last_area_height = pixel_DetailSectionsAreaHeight;
            }
            
            
            
		    //page footer
		    if(pixel_PageFooterAreaHeight != 0){
		        coor_top = coor_top + last_area_height;
		        var footer_title = $("#area_title").clone(true);
		        footer_title.attr({"id":"page_footer_title"})
		                    .css({"width":template_width,"height":title_height,"top":coor_top})
		                    .removeClass("nodisplay")
		                    .html("Page Footer");
                $("#editarea").append(footer_title);
                
		        coor_top = coor_top + title_height;
		        var footer_body = $("#area_body").clone(true);
		        footer_body.attr({"id":"printTemplateInfo_dot_PageFooter"})
		                    .css({"width":template_width,"top":0,"height":pixel_PageFooterAreaHeight})//PageHeader.AreaHeight
		                    .removeClass("nodisplay")
					        .selectable(selectable_obj)
					        .droppable(droppable_obj)
					        .mousemove(f_PageFooterMouseMove)
				            .mousedown(function(e){
				                //loseFocus();
				            })
                            .mouseup(f_PageMouseUp);                    
        				    			
	            var section_body = $("#area_body").clone(true);
	            section_body.attr({"id":"printTemplateInfo_dot_PageFooter1"})
	                        .css({"border-width":"0","width":template_width,"top":coor_top,"height":pixel_PageFooterAreaHeight})//AreaHeight
	                        .removeClass("nodisplay");
                $("#editarea").append(section_body);


                $("#printTemplateInfo_dot_PageFooter1").append(footer_body);            
                printTemplateInfo.PageFooter.ObjectName = "printTemplateInfo_dot_PageFooter";
            }
    }





    //判断拖拽进来的obj的left/top是否在这个区域的某个area内
    function ifWithinArea(strTemplateRef, cloneObj){

        var X = pixelToMm_x(parseFloat((cloneObj.css("left")).replace("px","")));
        var Y = pixelToMm_y(parseFloat((cloneObj.css("top")).replace("px","")));
        strTemplateRef = strTemplateRef + ".ArearObjects";
        var objTemplateRef = eval(strTemplateRef);
        var left,top,right,bottom;
        for(var i=0;i<objTemplateRef.length;i++){
            left = objTemplateRef[i].X;
            top = objTemplateRef[i].Y;
            right = parseFloat(left) + parseFloat(objTemplateRef[i].Width);
            bottom = parseFloat(top) + parseFloat(objTemplateRef[i].Height);
            if((left < X) && (right > X) && (top < Y) && (bottom > Y)){
                return objTemplateRef[i].ObjectName;
            }
        }
        return false;
        
    }    


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	getWidthHeightById
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2010
    //| Description	:	取得对象的宽高，用于确定选择区域的大小
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 

    function getWidthHeightById(id){
        var objWidth, objHeight;
        var objId = idToObj(id);
        if(id.indexOf("LineObjects") > 0){
	    
            if(objId.Angle == "<%=Constants.ANGLE_90%>" || objId.Angle == "<%=Constants.ANGLE_270%>"){
            
                objWidth = objId.Thickness;
                objHeight = objId.Length;
                       
            }else{

                objWidth = objId.Length;
                objHeight = objId.Thickness;

            }
            
        }else if(id.indexOf("TextObjects") > 0 || id.indexOf("PictureObjects") > 0){
	    
            if(objId.Angle == "<%=Constants.ANGLE_90%>" || objId.Angle == "<%=Constants.ANGLE_270%>"){

                objWidth = objId.Height;
                objHeight = objId.Width;

            }else{

                objWidth = objId.Width;
                objHeight = objId.Height;

            }
	        
        }else if(id.indexOf("BarcodeObjects") > 0){
        //var aa=bb;
            var tmpWidth = pixelToMm_x($("#" + id).children("img").css("width"));
            var tmpHeight = pixelToMm_y($("#" + id).children("img").css("height"));
        
            /*
	        var tmpWidth, tmpHeight;
            $.each(g_arrBarcodeWidthHeight, function(index,callback){
                if(g_arrBarcodeWidthHeight[index].id == id){
                    tmpWidth = g_arrBarcodeWidthHeight[index].width;
                    tmpHeight = g_arrBarcodeWidthHeight[index].height;
                    return false;
                }
            });*/

            if(objId.Angle == "<%=Constants.ANGLE_90%>" || objId.Angle == "<%=Constants.ANGLE_270%>"){

                objWidth = tmpHeight;
                objHeight = tmpWidth;

            }else{

                objWidth = tmpWidth;
                objHeight = tmpHeight;

            }     

        }else if(id.indexOf("RectangleObjects") > 0 || id.indexOf("ArearObjects") > 0){

                objWidth = objId.Width;
                objHeight = objId.Height;
	        
        }
        
        return objWidth + "&" + objHeight;
    }
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	getSelectedBoxPosition
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2010
    //| Description	:	以下是取得所有复制对象相对于0，0点的基准坐标.计算选中区域的左上和右下坐标，宽高，为了设定虚拟矩形区域的大小和位置

    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
    function getSelectedBoxPosition(curObj, curId){
        //以下是取得所有复制对象相对于0，0点的基准坐标
        //计算选中区域的左上和右下坐标，宽高，为了设定虚拟矩形区域的大小和位置
        No++;
        //使用第一个对象的x,y初始化选中区域的左上和右下坐标
        if(No == 1){
            g_MinX = parseFloat(curObj.X);
            g_MinY = parseFloat(curObj.Y);
            
            var tmp = getWidthHeightById(curId).split("&");
            g_MaxX = g_MinX + parseFloat(tmp[0]);//width
            g_MaxY = g_MinY + parseFloat(tmp[1]);//height
            
        }else{
            //计算选中区域的左上
            if(parseFloat(curObj.X) < g_MinX){
                g_MinX = parseFloat(curObj.X);
            }
            if(parseFloat(curObj.Y) < g_MinY){
                g_MinY = parseFloat(curObj.Y);
            }

            ////计算选中区域的右下
            var tmp = getWidthHeightById(curId).split("&");
            if((parseFloat(curObj.X) + parseFloat(tmp[0])) > g_MaxX){
                g_MaxX = parseFloat(curObj.X) + parseFloat(tmp[0]);
            }
            if((parseFloat(curObj.Y) + parseFloat(tmp[1])) > g_MaxY){
                g_MaxY = parseFloat(curObj.Y) + parseFloat(tmp[1]);
            }
        }   
    }    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	generateLines
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2010
    //| Description	:	
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
   
    function generateLines(){
         var cloneLineLeft = $("#lineV").clone(true);
         cloneLineLeft.appendTo($("#"+focus_container));
         cloneLineLeft.css({"left":g_MinX + "mm", "top": g_MinY+"mm", "height": theHeight+"mm","width":"0px"})
                       .removeClass("nodisplay")
                       .addClass("son")
                       .attr("id", focus_container + "_LineLeft");

         var cloneLineRight = $("#lineV").clone(true);
         cloneLineRight.appendTo($("#"+focus_container));
         cloneLineRight.css({"left":g_MaxX + "mm", "top": g_MinY+"mm", "height": theHeight+"mm","width":"0px"})
                       .removeClass("nodisplay")
                       .addClass("son")
                       .attr("id", focus_container + "_LineRight");

         var cloneLineTop = $("#lineH").clone(true);
         cloneLineTop.appendTo($("#"+focus_container));
         cloneLineTop.css({"left":g_MinX + "mm", "top": g_MinY+"mm", "width": theWidth+"mm","height":"1px"})
                       .removeClass("nodisplay")
                       .addClass("son")
                       .attr("id", focus_container + "_LineTop");
         
         var cloneLineBottom = $("#lineH").clone(true);
         cloneLineBottom.appendTo($("#"+focus_container));
         cloneLineBottom.css({"left":g_MinX + "mm", "top": g_MaxY+"mm", "width": theWidth+"mm","height":"1px"})
                       .removeClass("nodisplay")
                       .addClass("son")
                       .attr("id", focus_container + "_LineBottom");
    }    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	selectable_obj
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	selectable
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
    var startX, startY;
    var g_MinX = 0, g_MinY = 0, g_MaxX = 0, g_MaxY = 0, No = 0, theWidth = 0, theHeight = 0;//选择区域的左上、右下坐标，以及宽度和高度
    var arrObjX = new Array(), arrObjY = new Array();
    var g_CountAll = 0;
    selectable_obj = {
        tolerance: 'fit',
        filter: '.son',
        //cancel: 'div.grandson',
        start: function(e){
	        
            //删除虚拟选择区域
            if(focus_container == $(this).attr("id")){

                $("#" + focus_container + "_selectedRectangleArea").replaceWith("");
                $("#" + focus_container + "_LineLeft").replaceWith("");
                $("#" + focus_container + "_LineRight").replaceWith("");
                $("#" + focus_container + "_LineTop").replaceWith("");
                $("#" + focus_container + "_LineBottom").replaceWith("");
            }
            //alert("start");
        },
        stop: function(e){//选择动作结束
    	    //debugger;
            count = $(".ui-selected",this).size();
            //alert(g_CountAll);
            if(count == 0){
                g_CountAll = 0;
                loseFocus();
                //set_btn_state(g_CountAll);
                //清空坐标显示
                document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):");
                document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):");
                return false;
            }else{//count>0
                if(focus_container != ""){
                
                    if(focus_container != $(this).attr("id")){
                        //没有按下ctrl,则更换容器，否则不换，提示不能在多个容器中多选
                        if(e.ctrlKey == true){
                            alert("<%=Resources.VisualTemplate.MultiSelect%>");
                            return false;
                        }else{//更换容器
                            loseFocus();
                            focus_container = $(this).attr("id");
                        }
                    }
                }else{
                    focus_container = $(this).attr("id");
                }

                g_CountAll = count;
                //自动设置一个高亮的对象。
                //条件：通过鼠标左键按下滑动，同时选中多个，并且如果没有用户自己选择的高亮
	            var hasTheFirst = $("#" + focus_container + " .thefirst").size();
	            if(!hasTheFirst){
	                $("#" + $(".ui-selected",this)[0].id).addClass("thefirst");
	            }
                
            }
            set_btn_state(g_CountAll);
            
            //以下循环，需要计算选择区域的左上、右下坐标，以及宽度和高度
            No = 0;
            $(".ui-selected",this).each(function(){
                var curId = $(this).attr("id");         //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
                var curObj = idToObj(curId);
                
                
                if(g_CountAll == 1){
                    
                    //得到焦点，但是不可以拖拽
                    $(this).draggable('enable')
                           .css({cursor:"move"})
                           .addClass("yes");
                           
                    g_MinX = parseFloat(curObj.X);
                    g_MinY = parseFloat(curObj.Y);
                    
                    var tmp = getWidthHeightById(curId).split("&");
                    g_MaxX = g_MinX + parseFloat(tmp[0]);//width
                    g_MaxY = g_MinY + parseFloat(tmp[1]);//height

                }else{//g_CountAll > 1
                
                    getSelectedBoxPosition(curObj, curId);
                    
                    
                    //alert("father:selected:in::"+focus_container+"::"+$(this).attr("id"));
                    
                    //得到焦点，但是不可以拖拽
                    $(this).draggable('disable')
                           .css({cursor:"move"})
                           .addClass("yes");
                           //.trigger("blur",["blur"]); 

                    ////////////////////this.filters[0].addAmbient(255,187,119,100);//选中后的透明效果
                    //$(this).css({cursor:"move","z-index":10});//鼠标移入，变型
                    ////////////////////$(this).css({"background-color":"black"});//对应属性反白显示
                    ////////////////////$(this).css({"color":"white"});
                    ////////////////////$(this).css({"font-size":"30px"});//对应属性字体大小
                    ////////////////////this.innerHTML = this.innerHTML.replace(this.innerText,"dddddddd");//对应属性，动态修改内容                
                
                }
                var cssobj;
                if($(this).hasClass("thefirst")){//第一个
                    cssobj = {"visibility":"visible","background-color":"rgb(0,255,0)"}

                    //////////从结构里面取（drag——stop记录的）坐标，刷新坐标显示，同时定位这个object,begin               
                    var x = curObj.X;
                    var y = curObj.Y;
                    
                    $(this).css("left",mmToPixel_x(x));
                    $(this).css("top",mmToPixel_y(y));
                    
                    document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):"+x)				            
                    document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):"+y)				            

                    //////////从结构里面取（drag——stop记录的）坐标，刷新坐标显示，同时定位这个object,end
                    
                    //tree.searchInChildNodes("uuid", id);;
                    
                    
                }else{
                    cssobj = {"visibility":"visible","background-color":"transparent"}
                }
                $("span", this).css(cssobj)
                               .addClass("yes");
                               
                               
            });
            //alert(g_CountAll);
            //以下设定虚拟矩形选择区域的大小和位置
            theWidth = g_MaxX - g_MinX;
            theHeight = g_MaxY - g_MinY;
            if(g_CountAll > 1){
                if(!$("#" + focus_container + "_selectedRectangleArea").size()){//size == 0
                
                    var cloneObj = $("#selectedRectangleArea").clone(true);
                    cloneObj.appendTo($("#"+focus_container));


                     cloneObj//.addClass("ui-selected")
                             .addClass("son")
                             .removeClass("nodisplay");
                             //.draggable('enable')
                             //.css({cursor:"move"});

                     cloneObj.css("position", "absolute");
                     cloneObj.css("left",g_MinX+"mm");
                     cloneObj.css("top",g_MinY+"mm");
                     cloneObj.css("width",theWidth+"mm");
                     cloneObj.css("height",theHeight+"mm");
                     cloneObj.css({"visibility":"visible","background-color":"transparent","border-color":"rgb(0,256,0)"});
                             
                     //将该区域置底
                     cloneObj.css({"z-index":"-11"});
                     cloneObj.attr("id",focus_container+"_selectedRectangleArea");
                     
                 }else{//size==1
                 /*
                     $("#" + focus_container + "_selectedRectangleArea").css("left",g_MinX+"mm")
                                                                        .css("top",g_MinY+"mm")
                                                                        .css("width",theWidth+"mm")
                                                                        .css("height",theHeight+"mm");

                     $("#" + focus_container + "_LineLeft").css("left",g_MinX+"mm").css("top",g_MinY+"mm").css("width","0px")
                                                                        .css("height",theHeight+"mm");
                     $("#" + focus_container + "_LineRight").css("left",g_MaxX+"mm").css("top",g_MinY+"mm").css("width","0px")
                                                                        .css("height",theHeight+"mm");
                     $("#" + focus_container + "_LineTop").css("left",g_MinX+"mm").css("top",g_MinY+"mm").css("width",theWidth+"mm")
                                                                        .css("height","1px");
                     $("#" + focus_container + "_LineBottom").css("left",g_MinX+"mm").css("top",g_MaxY+"mm").css("width",theWidth+"mm")
                                                                        .css("height","1px");
                                                                        alert( $("#" + focus_container + "_LineLeft").size());*/
                 }

             }




            //alert("stop end");
        },
    	
        unselected: function(event,ui){
    	
    	    //debugger;
            //alert("unselected");
            if($("div span.yes",this).size() == 0){
                return false;
            }
            $("div span.yes",this).css({"visibility":"hidden", "background-color":"transparent"})
                                  .removeClass("yes");

            //失去焦点，不可拖拽
            $("div.yes",this).draggable('disable')
                             .removeClass("yes")
                             .removeClass("thefirst")
                             .css({cursor:"auto"});
            return false;
        },
        
        selected: function(event,ui){
    	    //alert($(this).is(".ui-selected") == true);
      	    //debugger;
            
    	    if(event.ctrlKey == true){
                if($(ui.selected).is(".yes") == true){
                    $(ui.selected).each(function(){
                        $(this).removeClass("ui-selected");
                        $("span",this).css({"visibility":"hidden", "background-color":"transparent"})
                                              .removeClass("yes");

                        //失去焦点，不可拖拽
                        $(this).draggable('disable')
                                         .removeClass("yes")
                                         .removeClass("thefirst")
                                         .css({cursor:"auto"});
                    })
                    return false;
                }
    	    }
        }

    };
    //以下，打开已有的模板，调入html后，对各个分区以及分区内的对象，需要重新附加事件
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	f_ObjectMouseDown
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2010
    //| Description	:	对象的Mousedown事件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    
    var f_ObjectMouseDown = function(e){
                        if(e.ctrlKey == false){
                        
                            if($(this).parent().attr("id") != focus_container){
                                return true;
                            }
                            
                            if(g_CountAll > 1){
	                            var coor_x = e.pageX;
	                            var coor_y = e.pageY;
	                            var parent_y = $(this).offsetParent().offset().top;
	                            var parent_x = $(this).offsetParent().offset().left;
                                startY = pixelToMm_XY_Y(coor_y - parent_y);//鼠标的y
                                startX = pixelToMm_XY_X(coor_x - parent_x);//鼠标的x
                                var container_width = pixelToMm_x(parseFloat(($("#" + focus_container).css("width")).replace("px","")));
                                var container_height = pixelToMm_x(parseFloat(($("#" + focus_container).css("height")).replace("px","")));

                                if(g_MaxX > container_width || g_MaxY > container_height || g_MinX < 0 || g_MinY < 0){
                                    return true;
                                }
                                if(checkIfInSelectedBox(startX, startY)){
                                    //alert("object mousedown in");
                                    $(this).parent().selectable('disable');
                                    
                                    generateLines();
                                    
                                    return false;
                                }else{
                                    //alert("object mousedown out");
                                    return true;
                                }
                            }else{//g_CountAll==1
                            //alert("fff");
                                //loseFocus();
                            }
                            
                        }
                    };

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	f_ObjectMouseUp
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2010
    //| Description	:	对象的Mouseup事件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    
    var f_ObjectMouseUp = function(e){
                        if(e.ctrlKey == false){
                           // e.cancelBubble = true;
                            $(this).parent().selectable('enable');

                            $("#" + focus_container + "_LineLeft").replaceWith("");
                            $("#" + focus_container + "_LineRight").replaceWith("");
                            $("#" + focus_container + "_LineTop").replaceWith("");
                            $("#" + focus_container + "_LineBottom").replaceWith("");

                            if(g_bMouseMove){
                                return true;
                            }else{
                                return false;
                            };
                        }
                    };


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	f_ObjectDblClick
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2010
    //| Description	:	对象的DblClick事件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    
    var f_ObjectDblClick = function(e){
                        //是当前唯一一个选中的object
                        if((e.ctrlKey == false) && (g_CountAll == 1)){
                            setProperty();
                        }
                    };
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	AttachEventsToObjects
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	对某个分区内的对象,重新附加事件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    //bug no:ITC-992-0087 
    //reason:没有mousedown处理，导致有多个高亮object
    function AttachEventsToObjects(strContainerID){
        $("#"+strContainerID+" div").filter(".ui-draggable").each(function(){
            $(this).draggable(draggable_obj)
                   .draggable("disable")
                   .dblclick(f_ObjectDblClick)
                   .mousedown(f_ObjectMouseDown)
                   .mouseup(f_ObjectMouseUp)                 
                   .bind("setBottom", function(event,a){
                   
                        //置底(ArearObject/RectangleObject除外)，在loseFocus中trigger
                        var id = $(this).attr("id");
                        //if(!(id.indexOf("ArearObject") > 0 || id.indexOf("RectangleObject") > 0) && ($(this).css("z-index") == "1")){
                        if(($(this).css("z-index") == 1)){
                            $(this).css({"cursor":"auto","z-index":"0"});
                            $(this).children("span").css({"visibility":"hidden","background-color":"transparent","z-index":"0"});//设置四个得焦点标志
                        }
                    });
        });

    }
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	f_PageHeaderMouseMove
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	PageHeader区域的MouseMove事件处理，鼠标左键按下时，取鼠标点位置
    //| 以下两种情况从mousemove事件返回：1、鼠标左键没有按下；2、没有“选择区域”
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    var g_DiffY = 0, g_DiffX = 0;//mm
    var g_bMouseMove = false;
    f_PageHeaderMouseMove = function(e){
                            //if(!bUpdateXY) return;
                            if((e.button != 1)) return;//e.button == 1, 表示鼠标左键按下
                            if($(this).attr("id") != focus_container) return;//不是当前的焦点容器，返回
                            if(!$("#" + focus_container + "_selectedRectangleArea").size()) return;//焦点容器没有选择区域，返回
                            g_bMouseMove = true;
					        var coor_x = e.pageX;
					        var coor_y = e.pageY;
					        var parent_y = $(this).offsetParent().offset().top;
					        var parent_x = $(this).offsetParent().offset().left;
				            var y = pixelToMm_XY_Y(coor_y - parent_y);//鼠标的y
				            var x = pixelToMm_XY_X(coor_x - parent_x);//鼠标的x
				            var old_DiffX, old_DiffY;
				            
				            old_DiffX = g_DiffX;
				            old_DiffY = g_DiffY;
				            
				            g_DiffY = y - startY;
				            g_DiffX = x - startX;
 					        //alert(g_DiffX);
                            var mm_TemplateWidth = printTemplateInfo.TemplateWidth;
                            var mm_PageHeaderAreaHeight = printTemplateInfo.PageHeader.AreaHeight;
                            //如果X方向越界，恢复上次的
                            if((mm_TemplateWidth <= (g_MaxX+g_DiffX)) 
                                || (0 >= (g_MinX+g_DiffX))){
                                g_DiffX = old_DiffX;    
                            }
                            //如果Y方向越界，恢复上次的
                            if((mm_PageHeaderAreaHeight <= (g_MaxY+g_DiffY)) 
                                || (0 >= (g_MinY+g_DiffY))){
                                g_DiffY = old_DiffY;
                            }
                            //重置选择区域的位置
				            var objSelectBox = $("#" + focus_container + "_selectedRectangleArea");
				            objSelectBox.css({"left":mmToPixel_x(g_MinX+g_DiffX),"top":mmToPixel_y(g_MinY+g_DiffY)});

                            //使用绿色高亮对象的坐标刷新右上坐标显示
                            var objTheFirst = $("#" + focus_container + " .thefirst");
                            var X_objTheFirst = pixelToMm_XY_X((objTheFirst.css("left")).replace("px",""));
                            var Y_objTheFirst = pixelToMm_XY_Y((objTheFirst.css("top")).replace("px",""));
                            document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):"+(Math.ceil((X_objTheFirst + g_DiffX)*10)/10))				            
                            document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):"+(Math.ceil((Y_objTheFirst + g_DiffY)*10)/10))				            

				        };    

    var f_PageMouseUp = function(e){
            if(e.ctrlKey == false){
            //alert("mouseup");
                if(!g_bMouseMove) return;
                g_bMouseMove = false;

                window.parent.frames("menu").bTemplateChanged = true;
                
                g_MinX = g_MinX + g_DiffX;
                g_MinY = g_MinY + g_DiffY;
                g_MaxX = g_MaxX + g_DiffX;
                g_MaxY = g_MaxY + g_DiffY;
                
                if(focus_container != ""){
                    $("#"+focus_container+" div").filter(".ui-selected").each(function(){
                        var srcId = $(this).attr("id");         //"printTemplateInfo.DetailSections_a0_b.Cells_a2_b.LineObjects_a0_b"
                        var srcObj = idToObj(srcId);
                        
                        //做四舍五入
                        srcObj.X = Math.round((parseFloat(srcObj.X) + g_DiffX)*10)/10 + "";
                        srcObj.Y = Math.round((parseFloat(srcObj.Y) + g_DiffY)*10)/10 + "";

                        $(this).css("left", mmToPixel_x(srcObj.X));
                        $(this).css("top", mmToPixel_y(srcObj.Y));
                    })

                    $("#" + focus_container + "_LineLeft").replaceWith("");
                    $("#" + focus_container + "_LineRight").replaceWith("");
                    $("#" + focus_container + "_LineTop").replaceWith("");
                    $("#" + focus_container + "_LineBottom").replaceWith("");
                                                                        
                }

                $(this).selectable('enable');
                g_DiffX = 0;
                g_DiffY = 0;
                
            }
        };


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	f_SectionHeaderMouseMove
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	SectionHeader区域的MouseMove事件处理，取鼠标点位置
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    f_SectionHeaderMouseMove = function(e){
                                //alert(headerHeight);
                                if((e.button != 1)) return;//e.button == 1, 表示鼠标左键按下。鼠标左键没有按下，返回
                                if($(this).attr("id") != focus_container) return;//不是当前的焦点容器，返回
                                if(!$("#" + focus_container + "_selectedRectangleArea").size()) return;//焦点容器没有选择区域，返回
                                g_bMouseMove = true;
					            var coor_x = e.pageX;
					            var coor_y = e.pageY;
					            var parent_y = $(this).offsetParent().offset().top;
					            var parent_x = $(this).offsetParent().offset().left;
				                var y = pixelToMm_XY_Y(coor_y - parent_y);//鼠标的y
				                var x = pixelToMm_XY_X(coor_x - parent_x);//鼠标的x
				                var old_DiffX, old_DiffY;
				                old_DiffX = g_DiffX;
				                old_DiffY = g_DiffY;
    				            
				                g_DiffY = y - startY;
				                g_DiffX = x - startX;
     					        

		                        var id = $(this).attr("id");
		                        var arrId = id.split("_dot_");
		                        arrId.splice(arrId.length-1, arrId.length-1);
		                        id = arrId.join("_dot_");
		                        var objId = idToObj(id);
                                var mm_TemplateWidth = printTemplateInfo.TemplateWidth;
                                var mm_SectionHeaderHeight = objId.HeaderHeight;
                                
                                //如果X方向越界，恢复上次的
                                if((mm_TemplateWidth <= (g_MaxX+g_DiffX)) 
                                    || (0 >= (g_MinX+g_DiffX))){
                                    g_DiffX = old_DiffX;    
                                }
                                //如果Y方向越界，恢复上次的
                                if((mm_SectionHeaderHeight <= (g_MaxY+g_DiffY)) 
                                    || (0 >= (g_MinY+g_DiffY))){
                                    //alert(g_MinY+g_DiffY+theHeight);
                                    g_DiffY = old_DiffY;
                                }
                                //重置选择区域的位置
				                var objSelectBox = $("#" + focus_container + "_selectedRectangleArea");
				                objSelectBox.css({"left":mmToPixel_x(g_MinX+g_DiffX),"top":mmToPixel_y(g_MinY+g_DiffY)});

                                //使用绿色高亮对象的坐标刷新右上坐标显示
                                var objTheFirst = $("#" + focus_container + " .thefirst");
                                var X_objTheFirst = pixelToMm_XY_X((objTheFirst.css("left")).replace("px",""));
                                var Y_objTheFirst = pixelToMm_XY_Y((objTheFirst.css("top")).replace("px",""));
                                document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):"+(Math.ceil((X_objTheFirst + g_DiffX)*10)/10))				            
                                document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):"+(Math.ceil((Y_objTheFirst + g_DiffY)*10)/10))				            


				            };
    				        
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	f_SectionCellMouseMove
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	SectionCell区域的MouseMove事件处理，取鼠标点位置
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    f_SectionCellMouseMove = function(e){
                                    /*
                                    if(!bUpdateXY) return;
					                var coor_x = e.pageX;
					                var coor_y = e.pageY;
					                var parent_y = $(this).offsetParent().offset().top;
					                var parent_x = $(this).offsetParent().offset().left + parseInt($(this).css("left"));
    					            

                                    var myReg = /_a\d+_b/ig;
                                    var dimension = ($(this).attr("id")).match(myReg);//"_a0_b"
                                    var section_no = dimension[0].replace("_a","");
                                    section_no = section_no.replace("_b","");     //detailsection"0"
                                    section_no = parseInt(section_no);            //0
                                    var tmp = dimension[dimension.length-1].replace("_a","");
                                    tmp = tmp.replace("_b","");     //"0"
                                    tmp = parseInt(tmp);            //0
    					            //alert(section_no);
    					            //alert(tmp);
					                var y = pixelToMm_XY_Y(coor_y - parent_y - mytop[section_no][tmp]);
					                var x = pixelToMm_XY_X(coor_x - parent_x);
				            
                                    //document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):"+x)				            
                                    //document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):"+y)				            
                                    */

                                    if((e.button != 1)) return;//e.button == 1, 表示鼠标左键按下
                                    if($(this).attr("id") != focus_container) return;//不是当前的焦点容器，返回
                                    if(!$("#" + focus_container + "_selectedRectangleArea").size()) return;//焦点容器没有选择区域，返回
                                    g_bMouseMove = true;
					                var coor_x = e.pageX;
					                var coor_y = e.pageY;
					                var parent_y = $(this).offsetParent().offset().top;
					                var parent_x = $(this).offsetParent().offset().left + parseInt($(this).css("left"));
                                    //alert(focus_container);
                                    var myReg = /_a\d+_b/ig;
                                    var dimension = ($(this).attr("id")).match(myReg);//"_a0_b"
                                    var section_no = dimension[0].replace("_a","");
                                    section_no = section_no.replace("_b","");     //detailsection"0"
                                    section_no = parseInt(section_no);            //0
                                    var tmp = dimension[dimension.length-1].replace("_a","");
                                    tmp = tmp.replace("_b","");     //"0"
                                    tmp = parseInt(tmp);            //0
				                    var y = pixelToMm_XY_Y(coor_y - parent_y - mytop[section_no][tmp]);//鼠标的y
				                    var x = pixelToMm_XY_X(coor_x - parent_x);//鼠标的x
				                    var old_DiffX, old_DiffY;
				                    old_DiffX = g_DiffX;
				                    old_DiffY = g_DiffY;
				                    g_DiffY = y - startY;
				                    g_DiffX = x - startX;
         					        
         					        
		                            var id = $(this).attr("id");
		                            var arrId = id.split("_dot_");
		                            arrId.splice(arrId.length-1, arrId.length-1);
		                            id = arrId.join("_dot_");
		                            var objId = idToObj(id);
                			        var num_Column = parseInt(objId.ColumnNum);//section.ColumnNum
                                    var mm_TemplateWidth = getWidthPerColumn(printTemplateInfo.TemplateWidth, num_Column);
                                    var mm_SectionCellHeight = objId.RowHeight;
                                    //如果X方向越界，恢复上次的
                                    if((mm_TemplateWidth <= (g_MaxX+g_DiffX)) 
                                        || (0 >= (g_MinX+g_DiffX))){
                                        g_DiffX = old_DiffX;    
                                    }
                                    //如果Y方向越界，恢复上次的
                                    if((mm_SectionCellHeight <= (g_MaxY+g_DiffY)) 
                                        || (0 >= (g_MinY+g_DiffY))){
                                        g_DiffY = old_DiffY;
                                    }
                                    //重置选择区域的位置
				                    var objSelectBox = $("#" + focus_container + "_selectedRectangleArea");
				                    objSelectBox.css({"left":mmToPixel_x(g_MinX+g_DiffX),"top":mmToPixel_y(g_MinY+g_DiffY)});

                                    //使用绿色高亮对象的坐标刷新右上坐标显示
                                    var objTheFirst = $("#" + focus_container + " .thefirst");
                                    var X_objTheFirst = pixelToMm_XY_X((objTheFirst.css("left")).replace("px",""));
                                    var Y_objTheFirst = pixelToMm_XY_Y((objTheFirst.css("top")).replace("px",""));
                                    document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):"+(Math.ceil((X_objTheFirst + g_DiffX)*10)/10))				            
                                    document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):"+(Math.ceil((Y_objTheFirst + g_DiffY)*10)/10))				            



				                };
    				            
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	f_PageFooterMouseMove
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	PageFooter区域的MouseMove事件处理，取鼠标点位置
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    f_PageFooterMouseMove = function(e){
                            if((e.button != 1)) return;//e.button == 1, 表示鼠标左键按下
                            if($(this).attr("id") != focus_container) return;//不是当前的焦点容器，返回
                            if(!$("#" + focus_container + "_selectedRectangleArea").size()) return;//焦点容器没有选择区域，返回
                            g_bMouseMove = true;
					        var coor_x = e.pageX;
					        var coor_y = e.pageY;
					        var parent_y = $(this).offsetParent().offset().top;
					        var parent_x = $(this).offsetParent().offset().left;
				            var y = pixelToMm_XY_Y(coor_y - parent_y);//鼠标的y
				            var x = pixelToMm_XY_X(coor_x - parent_x);//鼠标的x
				            var old_DiffX, old_DiffY;
				            
				            old_DiffX = g_DiffX;
				            old_DiffY = g_DiffY;
				            
				            g_DiffY = y - startY;
				            g_DiffX = x - startX;
 					        
                            var mm_TemplateWidth = printTemplateInfo.TemplateWidth;
                            var mm_PageFooterAreaHeight = printTemplateInfo.PageFooter.AreaHeight;
                            //如果X方向越界，恢复上次的
                            if((mm_TemplateWidth <= (g_MaxX+g_DiffX)) 
                                || (0 >= (g_MinX+g_DiffX))){
                                g_DiffX = old_DiffX;    
                            }
                            //如果Y方向越界，恢复上次的
                            if((mm_PageFooterAreaHeight <= (g_MaxY+g_DiffY)) 
                                || (0 >= (g_MinY+g_DiffY))){
                                g_DiffY = old_DiffY;
                            }
                            //重置选择区域的位置
				            var objSelectBox = $("#" + focus_container + "_selectedRectangleArea");
				            objSelectBox.css({"left":mmToPixel_x(g_MinX+g_DiffX),"top":mmToPixel_y(g_MinY+g_DiffY)});

                            //使用绿色高亮对象的坐标刷新右上坐标显示
                            var objTheFirst = $("#" + focus_container + " .thefirst");
                            var X_objTheFirst = pixelToMm_XY_X((objTheFirst.css("left")).replace("px",""));
                            var Y_objTheFirst = pixelToMm_XY_Y((objTheFirst.css("top")).replace("px",""));
                            document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):"+(Math.ceil((X_objTheFirst + g_DiffX)*10)/10))				            
                            document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):"+(Math.ceil((Y_objTheFirst + g_DiffY)*10)/10))				            


				        };	

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	attachEventsToAllPartsAndObjects
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	对各个分区以及分区内的对象,重新附加事件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function attachEventsToAllPartsAndObjects(){
        var strTemplateRef, objTemplateRef;

        //page header
        $("#printTemplateInfo_dot_PageHeader")
	    .selectable(selectable_obj)
	    .droppable(droppable_obj)
	    .mousemove(f_PageHeaderMouseMove)
        .mousedown(function(e){
            if(e.ctrlKey == false){

            }
            
        })
        .mouseup(f_PageMouseUp);                    

        
        
        var strContainerID = "printTemplateInfo_dot_PageHeader";
        AttachEventsToObjects(strContainerID);
	   // coor_top = coor_top + title_height;
       // coor_top = coor_top + mmToPixel(eval(strTemplateRef).AreaHeight);

        
        
        
        //sections
        strTemplateRef = "printTemplateInfo_dot_DetailSections";
        for(var i = 0;i < printTemplateInfo.DetailSections.length;i++){
	        var inner_header_height = mmToPixel_y(printTemplateInfo.DetailSections[i].HeaderHeight);
	        var inner_cell_height = mmToPixel_y(printTemplateInfo.DetailSections[i].RowHeight);//section.RowHeight
		    var num_DataSet = parseInt(printTemplateInfo.DetailSections[i].DateSetNum);//section.DataSetNum
		    var num_Column = parseInt(printTemplateInfo.DetailSections[i].ColumnNum);//section.ColumnNum
	        var coor_body_inner_top = 0;
        
            //hearder
		    var strTemp = strTemplateRef + "_a" + i + "_b_dot_HeaderArea";//"printTemplateInfo.DetailSections[num].HeaderArea"
		    $("#"+strTemp).selectable(selectable_obj)
                           .droppable(droppable_obj)
                           .mousemove(f_SectionHeaderMouseMove)
                           .mousedown(function(e){
                               //loseFocus();
                           })
                           .mouseup(f_PageMouseUp);
            if(parseFloat(printTemplateInfo.DetailSections[i].HeaderHeight) < AREA_LIMITED_MAX_HEIGHT){
                $("#"+strTemp).droppable("option", "tolerance", "pointer");
            }else{
                $("#"+strTemp).droppable("option", "tolerance", "fit");
            }


            AttachEventsToObjects(strTemp);
	        coor_body_inner_top = coor_body_inner_top + inner_header_height;
            
            //cells
            coor_body_inner_top = coor_body_inner_top - inner_cell_height;
	        mytop[i].length = 0;
            for(var j = 0;j <= num_DataSet;j++){
	            if(j%num_Column == 0){
                    coor_body_inner_top = coor_body_inner_top + inner_cell_height;
                    mytop[i][parseInt(mytop[i].length)] = coor_body_inner_top;
	            }else{
                    mytop[i][parseInt(mytop[i].length)] = coor_body_inner_top;
	            }

                var strTemp = strTemplateRef + "_a" + i + "_b_dot_Cells_a" + j + "_b";
		        $("#"+strTemp).selectable(selectable_obj)
                               .droppable(droppable_obj)
                               .mousemove(f_SectionCellMouseMove)
                               .mousedown(function(e){
                                   //loseFocus();
                               })
                               .mouseup(f_PageMouseUp);
                if(parseFloat(printTemplateInfo.DetailSections[i].RowHeight) < AREA_LIMITED_MAX_HEIGHT){
                    $("#"+strTemp).droppable("option", "tolerance", "pointer");
                }else{
                    $("#"+strTemp).droppable("option", "tolerance", "fit");
                }

                AttachEventsToObjects(strTemp);

            }
        }
        
        
        //page footer
        //coor_top = coor_top + title_height;
        $("#printTemplateInfo_dot_PageFooter")
	    .selectable(selectable_obj)
	    .droppable(droppable_obj)
	    .mousemove(f_PageFooterMouseMove)
        .mousedown(function(e){
            //loseFocus();
        })
        .mouseup(f_PageMouseUp);

        
        strContainerID = "printTemplateInfo_dot_PageFooter";
        AttachEventsToObjects(strContainerID);
        
        
    }



    //以下为dataset的设定
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	addOrEditDataSet
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	新增编辑dateset
    //| Input para.	:	method:add/edit
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function addOrEditDataSet(method){
        window.parent.frames("menu").bTemplateChanged = true;
    
        var diagArgs = new Object();
        diagArgs.method = method;
        diagArgs.dataset = document.getElementById("selNodeId").value;
        //保存结构到session
        printTemplateXMLAndHtmlInfo.toJSON = function(){return toJSON(this);};
        var ret = com.inventec.template.manager.TemplateManager.saveSession(printTemplateXMLAndHtmlInfo);
        if (ret.error != null) {
            alert(ret.error.Message);
            return;
        } 

        
        var ret = window.showModalDialog("../dataset/datasetedit.aspx", diagArgs, "dialogWidth:650px;dialogHeight:550px;center:yes;scroll:off;status:no;help:no")
        if (typeof(ret) != "undefined"){

            //debugger;
            var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
            if (ret.error != null) {
                alert(ret.error.Message);
                return;
            } else{
                printTemplateXMLAndHtmlInfo = ret.value; 
                printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;
            }

            if(method == "add"){
                //刷新树节点
                tree.freshCurrentNode(0); 
                tree.freshPath[0] = (printTemplateInfo.DatasettingList.length-1)+"";
            }else{//edit
                //刷新树节点
                tree.freshRootNode();                 
                //tree.searchInChildNodes("uuid", document.getElementById("selUUID").value, false);		
            }

        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	deleteDataSet
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	删除dateset
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function deleteDataSet(){
        window.parent.frames("menu").bTemplateChanged = true;
    
        var bInUse = false;
        for(var i = 0;i < printTemplateInfo.DatasettingList.length;i++){
            if(printTemplateInfo.DatasettingList[i].DataSetName == document.getElementById("selNodeId").value){
                for (j = 0; j < printTemplateInfo.DataObjects.length; j++){
                    //如果被使用中
                    if (((printTemplateInfo.DataObjects[j].Source == "<%=Constants.DATA_SOURCE_DATA_SET%>") && (printTemplateInfo.DataObjects[j].DataSet == printTemplateInfo.DatasettingList[i].DataSetName))
                            || (printTemplateInfo.DataObjects[j].DataSetDep == printTemplateInfo.DatasettingList[i].DataSetName)){
                        alert("Fail to delete the dataset because it is in use!");
                        bInUse = true;
                        break;
                    }
    	        }
                if(bInUse == false){
                    //结构里面删除
                    printTemplateInfo.DatasettingList.splice(i,1);    
                }
                break;
            }
        }
        tree.freshCurrentNode(1); 
    }
  
</script>
<script>
    parentid = '<%= Request.QueryString["parentid"]%>';    
    method = '<%= Request.QueryString["method"]%>'; //add,edit   
    nodeuuid = '<%= Request.QueryString["nodeuuid"]%>'; //edit/targetId  //templateid
    uuid = '<%= Request.QueryString["uuid"]%>'; //edit/treeId  
    type = '<%= Request.QueryString["type"]%>'; //type  
    userName = document.getElementById("<%=user.ClientID%>").value; 
    var success = true;

    //设定对象的缺省宽度和高度
    $("#draggableText").css({"font-size":"<%=Constants.TEXT_FONT_SIZE%>"+"pt","font-family":"<%=Constants.TEXT_FONT%>","font-style":"<%=Constants.TEXT_FONT_STYLE%>"});
    $("#draggableLine").css({"width":"<%=Constants.LINE_LENGTH%>"+"mm","height":"<%=Constants.LINE_THICKNESS%>"+"mm"});
    $("#draggableRectangle").css({"width":"<%=Constants.RECTANGLE_WIDTH%>"+"mm","height":"<%=Constants.RECTANGLE_HEIGHT%>"+"mm"});
    $("#draggableArea").css({"width":"<%=Constants.AREA_WIDTH%>"+"mm","height":"<%=Constants.AREA_HEIGHT%>"+"mm"});
    $("#draggableBarcode").css({"width":"<%=Constants.BARCODE_WIDTH%>"+"mm","height":"<%=Constants.BARCODE_HEIGHT%>"+"mm"});
    $("#draggablePicture").css({"width":"<%=Constants.PICTURE_WIDTH%>"+"mm","height":"<%=Constants.PICTURE_HEIGHT%>"+"mm"});


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	$
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	jquery的ready事件，初始化toolbox的工具，以及显示模版
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
	$(function() {
	
		$("#draggableText_tool").draggable({
			opacity: 0.7,
            cursorAt: {left: 0, top: 0 },
			helper: function(){
			    return $('#draggableText').clone().css({"visibility":"visible"});
			}
		});

		$("#draggableLine_tool").draggable({
			opacity: 0.7,
            cursorAt: {left: 0, top: 0 },
			helper: function(){
			    return $('#draggableLine').clone().css({"visibility":"visible"});
			}
		});

		$("#draggableRectangle_tool").draggable({
			opacity: 0.7,
            cursorAt: {left: 0, top: 0 },
			helper: function(){
			    return $('#draggableRectangle').clone().css({"visibility":"visible"});
			}
		});

		$("#draggableArea_tool").draggable({
			opacity: 0.7,
            cursorAt: {left: 0, top: 0 },
			helper: function(){
			    return $('#draggableArea').clone().css({"visibility":"visible"});
			}
		});

		$("#draggableBarcode_tool").draggable({
			opacity: 0.7,
            cursorAt: {left: 0, top: 0 },
			helper: function(){
			    return $('#draggableBarcode').clone().css({"visibility":"visible"});
			}
		});

		$("#draggablePicture_tool").draggable({
			opacity: 0.7,
            cursorAt: {left: 0, top: 0 },
			helper: function(){
			    return $('#draggablePicture').clone().css({"visibility":"visible"});
			}
		});	
	

        
        //debugger;
        if(success == true){
            if(method == "add"){
                document.getElementById("title").innerText = printTemplateInfo.FileName;
                createArea();
            }else{//edit/import，或者从树进入编辑/导入
                document.getElementById("title").innerText = printTemplateInfo.FileName;
                //document.getElementById("editarea").innerHTML = printTemplateXMLAndHtmlInfo.HtmlInfo;
                $("#editarea").html(printTemplateXMLAndHtmlInfo.HtmlInfo);
                attachEventsToAllPartsAndObjects();
            }
        }
    })
    
    if(method == "add"){
        var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
        if (ret.error != null) {
            success = false;
            alert(ret.error.Message);
        } else{
            printTemplateXMLAndHtmlInfo = ret.value; 
            printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;
            success = true;
            window.parent.frames("menu").bTemplateChanged = true;
        }


        /*
        printTemplateXMLAndHtmlInfo = window.parent.frames("menu").templateXmlAndHtml;
        if(printTemplateXMLAndHtmlInfo != undefined){
            printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;
            success = true;
            //createArea();
        }else{
            success = false;
        }	*/
    }else if(method == "import"){
        var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
        if (ret.error != null) {
            success = false;
            alert(ret.error.Message);
        } else{
            printTemplateXMLAndHtmlInfo = ret.value; 
            printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;
            //document.getElementById("editarea").innerHTML = printTemplateXMLAndHtmlInfo.HtmlInfo;
            success = true;
            window.parent.frames("menu").bTemplateChanged = true;
        }
    }else{//edit，或者从树进入编辑
        var rtn = com.inventec.template.manager.TemplateManager.getTemplateXmlAndHtml(nodeuuid);//templateid//"ddd104dcb6e44395b13870f992a469c6"
        if (rtn.error != null){
            alert(rtn.error.Message);
            success = false;
        }else{
            printTemplateXMLAndHtmlInfo = rtn.value;
            printTemplateInfo = printTemplateXMLAndHtmlInfo.PrintTemplateInfo;
            //document.getElementById("editarea").innerHTML = printTemplateXMLAndHtmlInfo.HtmlInfo;
            success = true;
            //attachEventsToAllPartsAndObjects();
        }
    }
    
    //设定所有操作object的按钮灰掉
    set_btn_state(0);
    
    
    
    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	fillup_structure
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	6/2009
    //| Description	:	object拖入后，填充结构
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
	function fillup_structure(focus_container, objType, cloneObj){
	    var strTemplateRef = focus_container;
        var objTemplateRef;
        strTemplateRef = strTemplateRef.replace(new RegExp("_a","g"),"[");
        strTemplateRef = strTemplateRef.replace(new RegExp("_b","g"),"]");
        strTemplateRef = strTemplateRef.replace(new RegExp("_dot_","g"),".");
        
        /*
        var tmp_focus_container = focus_container;
        
        tmp_focus_container = tmp_focus_container.replace(new RegExp("_a","g"),"[");
        tmp_focus_container = tmp_focus_container.replace(new RegExp("_b","g"),"]");
        alert(focus_container);
        if(focus_container == "page_header_body"){
            strTemplateRef += ".PageHeader";//printTemplateInfo.PageHeader
        }else if(focus_container == "page_footer_body"){
            strTemplateRef += ".PageFooter";//printTemplateInfo.PageFooter
        }else{
            strTemplateRef += ".DetailSections";//printTemplateInfo.DetailSections
            var myReg = /\[\d+]/ig;
            
            var dimension = tmp_focus_container.match(myReg);
            if(dimension.length == 1){
                strTemplateRef += dimension[0];//printTemplateInfo.DetailSections[number]
                strTemplateRef += ".HeaderArea";//printTemplateInfo.DetailSections[number].HeaderArea
            }else{
                strTemplateRef += dimension[0];//printTemplateInfo.DetailSections[number]
                strTemplateRef += ".Cells";//printTemplateInfo.DetailSections[number].Cells
                strTemplateRef += dimension[1];//printTemplateInfo.DetailSections[number].Cells[number]
            }
        }

        */

        var array_obj_length;
        var str_obj_type;
        var objStructure;
        var containArea;//false/具体的area的objectname

        //判断是否在area范围内
        //containArea = ifWithinArea(strTemplateRef,cloneObj);

        switch(objType){
            case constText:
                str_obj_type = "TextObjects";
                var objStructureRtn = com.inventec.template.manager.TemplateManager.getTextFromStructure();
                if (objStructureRtn.error != null) {
                    alert(objStructureRtn.error.Message);
                } else {
                    objStructure = objStructureRtn.value;
                }
                //填充对象缺省属性
                //    <bug>
                //        BUG NO:ITC-992-0015 
                //        REASON:没有设定缺省的fontstyle。
                //    </bug>
                objStructure.Font = "<%=Constants.TEXT_FONT%>";
                objStructure.TextStyle = "<%=Constants.FONT_STYLE_NORMAL%>";
                objStructure.Size = "<%=Constants.TEXT_FONT_SIZE%>";
                objStructure.Inverse = "<%=Constants.TEXT_INVERSE%>";
                objStructure.Width = "<%=Constants.TEXT_WIDTH%>";
                objStructure.Height = "<%=Constants.TEXT_HEIGHT%>";

                break;
            case constLine:
                str_obj_type = "LineObjects";
                var objStructureRtn = com.inventec.template.manager.TemplateManager.getLineFromStructure();
                if (objStructureRtn.error != null) {
                    alert(objStructureRtn.error.Message);
                } else {
                    objStructure = objStructureRtn.value;
                }
                //填充对象缺省属性
                objStructure.Thickness = "<%=Constants.LINE_THICKNESS%>";
                objStructure.Length = "<%=Constants.LINE_LENGTH%>";
                
                break;
            case constRectangle:
                str_obj_type = "RectangleObjects";
                var objStructureRtn = com.inventec.template.manager.TemplateManager.getRectangleFromStructure();
                if (objStructureRtn.error != null) {
                    alert(objStructureRtn.error.Message);
                } else {
                    objStructure = objStructureRtn.value;
                }
                //填充对象缺省属性
                objStructure.Border = "<%=Constants.RECTANGLE_THICKNESS%>";
                objStructure.Width = "<%=Constants.RECTANGLE_WIDTH%>";
                objStructure.Height = "<%=Constants.RECTANGLE_HEIGHT%>";
                objStructure.BackColor = "<%=Constants.RECTANGLE_BACKCOLOR%>";

                break;
            case constBarcode:
                str_obj_type = "BarcodeObjects";
                var objStructureRtn = com.inventec.template.manager.TemplateManager.getBarcodeFromStructure();
                if (objStructureRtn.error != null) {
                    alert(objStructureRtn.error.Message);
                } else {
                    objStructure = objStructureRtn.value;
                }
                //填充对象缺省属性
                objStructure.Symbology = "<%=Constants.BARCODE_SYMBOLOGY%>";
                objStructure.NarrowBarWidth = "<%=Constants.BARCODE_WIDTH_RATIO%>";
                objStructure.Width = "<%=Constants.BARCODE_WIDTH%>";
                objStructure.Height = "<%=Constants.BARCODE_HEIGHT%>";
                objStructure.Ratio = "<%=Constants.BARCODE_RATIO%>";
                objStructure.Inverse = "<%=Constants.BARCODE_INVERSE%>";   //added by itc207024 barcode反显属性              
                objStructure.NarrowBarWidthPixel = "<%=Constants.NARROW_BAR_WIDTH_PIXEL%>";
                //objStructure.HumanReadable = "<%=Constants.BARCODE_HUMANREADABLE%>";

                break;
            case constPicture:
                str_obj_type = "PictureObjects";
                var objStructureRtn = com.inventec.template.manager.TemplateManager.getPictureFromStructure();
                if (objStructureRtn.error != null) {
                    alert(objStructureRtn.error.Message);
                } else {
                    objStructure = objStructureRtn.value;
                }
                objStructure.Width = "<%=Constants.PICTURE_WIDTH%>";
                objStructure.Height = "<%=Constants.PICTURE_HEIGHT%>";

                break;
            case constArea:
                //不再做这个判断，存盘时不考虑area在另外一个area里的情况，也就是说，area可以叠在另外的area上
                /*
                if(containArea != false){
                    alert("you cannot put an Area Object within other Area Objects!");
                    return false;
                }
                */
                str_obj_type = "ArearObjects";
                var objStructureRtn = com.inventec.template.manager.TemplateManager.getAreaFromStructure();
                if (objStructureRtn.error != null) {
                    alert(objStructureRtn.error.Message);
                } else {
                    objStructure = objStructureRtn.value;
                }
                objStructure.Width = "<%=Constants.AREA_WIDTH%>";
                objStructure.Height = "<%=Constants.AREA_HEIGHT%>";

                break;
                
        }
        //以下填充每一个对象都会有的属性
        
        //////////刷新坐标显示，并向结构里面写入坐标,begin
        //使用被拖动object的左上角坐标刷新右上的坐标显示
        var x = pixelToMm_XY_X((cloneObj.css("left")).replace("px","")) + "";
        var y = pixelToMm_XY_Y((cloneObj.css("top")).replace("px","")) + "";


        //向结构里面写x,y坐标
        objStructure.X = x;//pixelToMm_XY_X(($(this).css("left")).replace("px","")) + "";
        objStructure.Y = y;//pixelToMm_XY_Y(($(this).css("top")).replace("px","")) + "";
        //////////刷新坐标显示，并向结构里面写入坐标,end        

        //objStructure.X = pixelToMm_XY_X((cloneObj.css("left")).replace("px","")) + "";
        //objStructure.Y = pixelToMm_XY_Y((cloneObj.css("top")).replace("px","")) + "";
        objStructure.Angle = "0";
        
        
        //".....TextObjects"
        strTemplateRef = strTemplateRef + "." + str_obj_type;
        
        //.....TextObjects
        objTemplateRef = eval(strTemplateRef);

        //.....TextObjects.length
        array_obj_length = objTemplateRef.length;
        
        //".....TextObjects[length]"
        obj_id = strTemplateRef + "[" + array_obj_length + "]";
        //对象的唯一id
        obj_id = obj_id.replace(new RegExp("\\[","g"),"_a")
        obj_id = obj_id.replace(new RegExp("]","g"),"_b")
        obj_id = obj_id.replace(new RegExp("\\.","g"),"_dot_")

        var obj_id_in_structure = str_obj_type + "[" + array_obj_length + "]";//TextObjects[length]
        obj_id_in_structure = obj_id_in_structure.replace(new RegExp("\\[","g"),"_a")
        obj_id_in_structure = obj_id_in_structure.replace(new RegExp("]","g"),"_b")//TextObjects_alength_b

        //bug no:ITC-992-0060
        //reason:生成的objectname有重复
        //生成随机数 begin
        /*var uuid;//document.uniqueID;//结构里面的objectname等于dataobject的objectname
        var   url="getUUID.aspx";   
        var   objXml= new ActiveXObject("Microsoft.Xmlhttp");   
        objXml.open("GET", url, false);     
        objXml.send();     
        if(objXml.status == 200)     
        {   
            uuid=objXml.responseText;   
        }else{   
            alert("<%=Resources.VisualTemplate.UUID_Object_Name%>");     
        }     */
        var uuid="";
        var rtn = webroot_aspx_main_visualEditPanel.getUUID();
        if (rtn.error!=null) {
            alert("<%=Resources.VisualTemplate.UUID_Object_Name%>");
            return;
        } else {
            uuid = rtn.value;
        }        
        //生成随机数 end
        
        objStructure.ObjectName = uuid;
        //.....TextObjects.push
        objTemplateRef.push(objStructure);
        //printTemplateInfo.dataobjects
        if(str_obj_type == "TextObjects" || str_obj_type == "BarcodeObjects" || str_obj_type == "PictureObjects" || str_obj_type == "RectangleObjects"){
            var dataRtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
            var dataObject;
            if (dataRtn.error != null) {
                alert(dataRtn.error.Message);
            } else {
                dataObject = dataRtn.value;
            }
            dataObject.ObjectName = uuid;
            dataObject.Source = "<%=Constants.DATA_SOURCE_SCREEN_DATA%>";
            switch(str_obj_type){
                case "TextObjects":
                    dataObject.DisplayTxt = "<%=Constants.TEXT_DEFAULT_TEXT%>";
                    break;
                case "BarcodeObjects":
                    dataObject.DisplayTxt = "<%=Constants.BARCODE_DEFAULT_TEXT%>";
                    break;
            }
            printTemplateInfo.DataObjects.push(dataObject);
        }
        
        return obj_id;
	}
</script>
<script>

//////////////以下为tab页
    //调用起来
    createBtnTabs();

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	createBtnTabs
    //| Author		:	itc98079/刘晓玲
    //| Create Date	:	4/24/2009
    //| Description	:	创建tab
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function createBtnTabs()
    {
        tabs = new clsButtonTabs("tabs");

	    tabs.preSelectChanged = preCallback;
	    tabs.selectChanged = callback;
    		 
	    for (var i=0; i<2; i++)
	    {
	        var temp = new clsButton("tabs");
	        if (i == 0) {
	            temp.normalPic = "../../images/Objects" +"-1.gif";
		        temp.selPic = "../../images/Objects" +"-2.gif";
		        temp.disablePic = "../../images/Objects" +"-3.gif";
	        } else {
		        temp.normalPic = "../../images/Dataset" +"-1.gif";
		        temp.selPic = "../../images/Dataset" +"-2.gif";
		        temp.disablePic = "../../images/Dataset" +"-3.gif";
		    }	
		    tabs.addButton(temp);
    		
	    }
    		
	    //tabs.diableTab(0,true);
	    divTab.innerHTML = tabs.toString();
	    tabs.initSelect(0);

    }


    function preCallback(){
        return true;
    }
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	callback
    //| Author		:	98079
    //| Create Date	:	4/30/2009
    //| Description	:	切换Tab页
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 	
    function callback(index)
    {  
        pageIndex = index;

        switch(index){
	        case 0: 
                document.getElementById("divObjectTree").style.display = "";
		 	    document.getElementById("divDataset").style.display = "none";
    		 	
		        break;
	        case 1: 
                
		        document.getElementById("divObjectTree").style.display = "none";
		 	    document.getElementById("divDataset").style.display = "";
		        break;
           
             
            default:
                break;	
         }
             
         return true;
    }


////////////////////以下为Dataset tree
var tree=null;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	createTree
//| Author		:	itc98079/刘晓玲
//| Create Date	:	4/24/2009
//| Description	:	创建树
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
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
		var type =  currentNode.type;
        var text = currentNode.text;
        var nodeuuid = currentNode.nodeuuid;
        if(type == "NODE_ROOT"){
            //当点击书节点时,将所选节点id保存到hidden控件
            document.getElementById("selNodeId").value = "";

            document.getElementById("add").disabled = false;
            document.getElementById("edit").disabled = true;
            document.getElementById("delete").disabled = true;
        }else{
            //当点击书节点时,将所选节点id保存到hidden控件
            document.getElementById("selNodeId").value = text;

            document.getElementById("add").disabled = true;
            document.getElementById("edit").disabled = false;
            document.getElementById("delete").disabled = false;
        }
    }	
    document.getElementById('treeviewarea').innerHTML = tree.toString();
    HideWait();
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	useSOPMngIcon
//| Author		:	itc98079/刘晓玲
//| Create Date	:	4/24/2009
//| Description	:	根据节点类型设置图标
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function useSOPMngIcon(node, treeObj)
{
    var strIconName = "";
	var strPrefix = "../../images/";
	
	switch (node.type){
		case "NODE_ROOT":
			strIconName = "dataserver.gif";
			break;
		case "<%=Constants.NODE_TYPE_REPORT%>":
			strIconName = "database.gif";
			break;
	   
		default:
		break;
	}
	return strPrefix +strIconName;
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Load
//| Author		:	itc98079/刘晓玲
//| Create Date	:	4/24/2009
//| Description	:	加载树节点数据
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~   
function Load()
{
    var source= tree.nodes[tree.expandingNode.sourceIndex]
    var uuid = tree.getAttribute(source, "uuid")
    var type = tree.getAttribute(source, "type");
    //setTimeout("getNodeData()",0);
    getNodeData();
}
        
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getNodeData
//| Author		:	itc98079/刘晓玲
//| Create Date	:	4/24/2009
//| Description	:	获取所选节点下的第一层节点(folder类型)
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~       
function getNodeData()
{
	ShowWait();
	//InitChild(printTemplateInfo.DatasettingList);
	setTimeout("InitChild()",10);
}
   
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	InitChild
//| Author		:	itc98079/刘晓玲
//| Create Date	:	4/24/2009
//| Description	:	创建树节点
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function InitChild()
{
       
    document.getElementById("add").disabled = false;
    document.getElementById("edit").disabled = true;
    document.getElementById("delete").disabled = true;


    var treeNodeInfo = printTemplateInfo.DatasettingList;
    if (treeNodeInfo.length < 1)
    {
        //HideWait();
        tree.emptyNode(tree.expandingNode);
        setTimeout("HideWait()",500);
        return;
    }

    if(treeNodeInfo != null && typeof(treeNodeInfo) == "object")
    {
        var arrName = new Array();
        for (var i=0; i < treeNodeInfo.length; i++)
        {
            //alert(treeNodeInfo.Rows[i]["nodeuuid"]);
            //alert(i);
            var arrNode = new Array();
	        arrNode[arrNode.length] = 'uuid' + tree.attribute_suffix + i;
		       // alert(treeNodeInfo.Rows[i][arrName[1]]);
	        //arrNode[arrNode.length] = 'parentId' + tree.attribute_suffix + treeNodeInfo.Rows[i][arrName[1]];
	        arrNode[arrNode.length] = 'text' + tree.attribute_suffix + treeNodeInfo[i].DataSetName;
	        arrNode[arrNode.length] = 'type' + tree.attribute_suffix + "<%=Constants.NODE_TYPE_REPORT%>";
	        arrNode[arrNode.length] = 'nodeuuid' + tree.attribute_suffix + "root";
    	
    	
	        hasChild = "false"; 
	        arrNode[arrNode.length] = 'hasChild' + tree.attribute_suffix + hasChild;
	        tree.nodes[tree.expandingNode.sid + '_' + ++tree.total] = arrNode.join(tree.attribute_d);
        }
     }

      tree.dataFormat();
      tree.load(tree.expandingNode.id);
      tree.buildNode(tree.expandingNode.id);
            
        //new add by lzy to expand the last visiting node.
      tree.locateNode("UUID");
    	
	  setTimeout("HideWait()",500);
      //HideWait();
   
   
}


//以下为objects tree
var treeObjects=null;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	createTree_Objects
//| Author		:	itc98079/刘晓玲
//| Create Date	:	4/24/2009
//| Description	:	创建objecs树
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function createTree_Objects()
{	
	
	treeObjects = new MzTreeView("treeObjects");
	treeObjects.useArrow = false;
	treeObjects.LoadData = "Load_Objects()";
	treeObjects.icons["property"] = "property.gif";
	treeObjects.icons["css"] = "collection.gif";
	treeObjects.icons["book"]  = "book.gif";
	treeObjects.iconsExpand["book"] = "bookopen.gif"; 
	treeObjects.setIconPath("./"); 
	treeObjects.nodes["-1_" + treeObjects.total++] = "uuid" + treeObjects.attribute_suffix + "-1"+treeObjects.attribute_d+"text" + treeObjects.attribute_suffix + "Root"+treeObjects.attribute_d+" hasChild" + treeObjects.attribute_suffix + "true"+treeObjects.attribute_d+" type" + treeObjects.attribute_suffix + "NODE_ROOT" +treeObjects.attribute_d+"nodeuuid" + treeObjects.attribute_suffix + "-1" ;
	treeObjects.customIconFun = useSOPMngIcon_Objects;

    //覆盖tree控件的dblClickHandle，增加功能：双击叶子节点后，弹出属性对话框
    treeObjects.dblClickHandle = function dblClickNode(e){
	//alert("dblClickHandle11111"); 
        this.findOrAllFlag = "1";//add only for dfx 1.1, show all info on clicking tree root node.
        	
        e = window.event || e; 
        e = e.srcElement || e.target;
        if((e.tagName=="A" || e.tagName=="IMG")&& e.id)
        {
            var id = e.id.substr(e.id.lastIndexOf("_") + 1);
            if(this.node[id].hasChild) 
            {
                this.expand(id);
            }else{
                setProperty();
            }
        }
    }

	treeObjects.nodeClick = function clicknode()
	{
		var currentNode = treeObjects.currentNode; 
		var uuid = currentNode.uuid;
		var pid = currentNode.parentId;
		var type =  currentNode.type;
        var text = currentNode.text;
        var nodeuuid = currentNode.nodeuuid;

        
        switch (type){
		    case "BarcodeObject":
		    case "TextObject":
		    case "RectangleObject":
		    case "ArearObject":
		    case "PictureObject":
		    case "LineObject":
                //alert(uuid);	
                //根据树节点，高亮相应的对象，并置顶(ArearObject/RectangleObject除外)
		        var arrUuid = uuid.split("_dot_");
		        arrUuid.splice(arrUuid.length-1, arrUuid.length-1);
		        var strUuid = arrUuid.join("_dot_");
                loseFocus();
		        focus_container = strUuid;
		        
                //得到焦点，可以拖拽
	             $("#"+uuid).draggable('enable')
	                     .addClass("ui-selected")
	                     .addClass("yes")
	                     .addClass("thefirst")
	                     .addClass("son")
	                     .removeClass("nodisplay")
	                     .css({"cursor":"move"});
	             $("#"+uuid).children("span").addClass("yes")
	                                      .css({"visibility":"visible","background-color":"rgb(0,255,0)"});//设置四个得焦点标志

                //置顶(ArearObject/RectangleObject除外)，置底由AttachEventsToObjects绑定的函数bind(setBottom)在loseFocus时trigger实现
                if(!(type == "ArearObject" || type == "RectangleObject")){
                    $("#"+uuid).css({"z-index":"1"})
	                 $("#"+uuid).children("span").css({"z-index":"1"});//设置四个得焦点标志
                }
                
                //设置右上的x/y坐标显示
                var x = (idToObj(uuid)).X;
                var y = (idToObj(uuid)).Y;
                document.getElementById("rainX").innerHTML = document.getElementById("rainX").innerHTML.replace(document.getElementById("rainX").innerText,"X(mm):"+x)				            
                document.getElementById("rainY").innerHTML = document.getElementById("rainY").innerHTML.replace(document.getElementById("rainY").innerText,"Y(mm):"+y)				            
                
                //设定操作object的按钮
                set_btn_state(1);

		        break;
            default:
                if(bFreshForPaste){
                    bFreshForPaste = false;
                    break;
                }
                if(focus_container != ""){
                    //除了上述六种类型，其他的节点都做失焦点处理
                    loseFocus();
                    //设定操作object的按钮
                    //set_btn_state(0);
                }
		        focus_container = "";
		        break;
        }        
    }	
    document.getElementById('treeviewobject').innerHTML = treeObjects.toString();
    HideWait();
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	useSOPMngIcon_Objects
//| Author		:	itc98079/刘晓玲
//| Create Date	:	4/24/2009
//| Description	:	根据节点类型设置图标
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function useSOPMngIcon_Objects(node, treeObj)
{
    var strIconName = "";
	var strPrefix = "../../images/";
	
	switch (node.type){
		case "NODE_ROOT":
			strIconName = "reportpublish.gif";
			break;
		case "printTemplateInfo_dot_PageHeader":
		case "printTemplateInfo_dot_PageFooter":
        case "printTemplateInfo_dot_DetailSections_dot_HeaderArea":
        case "printTemplateInfo_dot_DetailSections_dot_Cells":
        case "printTemplateInfo_dot_DetailSections":
			strIconName = "reportfolder.gif";
			break;
		case "BarcodeObjects":
		case "BarcodeObject":
			strIconName = "Toolbox Barcode.gif";
			break;
		case "TextObjects":
		case "TextObject":
			strIconName = "Toolbox Text.gif";
			break;
		case "RectangleObjects":
		case "RectangleObject":
			strIconName = "Toolbox Rectangle.gif";
			break;
		case "ArearObjects":
		case "ArearObject":
			strIconName = "Toolbox Area.gif";
			break;
		case "PictureObjects":
		case "PictureObject":
			strIconName = "Toolbox Picture.gif";
			break;
		case "LineObjects":
		case "LineObject":
			strIconName = "Toolbox Line.gif";
			break;
	   
		default:
		break;
	}
	return strPrefix +strIconName;
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Load_Objects
//| Author		:	itc98079/刘晓玲
//| Create Date	:	4/24/2009
//| Description	:	加载树节点数据
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~   
function Load_Objects()
{
    var source= treeObjects.nodes[treeObjects.expandingNode.sourceIndex]
    var uuid = treeObjects.getAttribute(source, "uuid")
    var type = treeObjects.getAttribute(source, "type");
    //setTimeout("getNodeData_Objects()",0);
    setTimeout("getNodeData_Objects('"+type+"','"+uuid+"')",0);
    //getNodeData(type,uuid);
}
        
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getNodeData_Objects
//| Author		:	itc98079/刘晓玲
//| Create Date	:	4/24/2009
//| Description	:	获取所选节点下的第一层节点(folder类型)
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~       
function getNodeData_Objects(type, uuid)
{
	ShowWait();
	//var rtn = com.inventec.template.manager.TemplateManager.getFirLevelFolders(uuid, InitChild);
	if(type == "NODE_ROOT"){
	    InitFourSections();
	}else{
    	InitChild_Objects(type, uuid);
	}
}

function InitFourSections(){

    if(printTemplateInfo.PageHeader.AreaHeight != "0"){
        var arrNode = new Array();
        arrNode[arrNode.length] = 'uuid' + treeObjects.attribute_suffix + "printTemplateInfo_dot_PageHeader";
        arrNode[arrNode.length] = 'text' + treeObjects.attribute_suffix + "Page Header";
        arrNode[arrNode.length] = 'type' + treeObjects.attribute_suffix + "printTemplateInfo_dot_PageHeader";
        arrNode[arrNode.length] = 'nodeuuid' + treeObjects.attribute_suffix + "printTemplateInfo_dot_PageHeader";
        arrNode[arrNode.length] = 'hasChild' + treeObjects.attribute_suffix + "true";
        treeObjects.nodes[treeObjects.expandingNode.sid + '_' + ++treeObjects.total] = arrNode.join(treeObjects.attribute_d);
    }
    
    for(var i=0;i<printTemplateInfo.DetailSections.length;i++){
        if(printTemplateInfo.DetailSections[i].AreaHeight != "0"){
            var index = parseInt(printTemplateInfo.DetailSections[i].Index);//start from 0
            var arrNode = new Array();
            arrNode[arrNode.length] = 'uuid' + treeObjects.attribute_suffix + "printTemplateInfo_dot_DetailSections_a" + index + "_b";
            arrNode[arrNode.length] = 'text' + treeObjects.attribute_suffix + "Detail(Section " + (index + 1) + ")";
            arrNode[arrNode.length] = 'type' + treeObjects.attribute_suffix + "printTemplateInfo_dot_DetailSections";
            arrNode[arrNode.length] = 'nodeuuid' + treeObjects.attribute_suffix + "printTemplateInfo_dot_DetailSections_a" + index + "_b";
            arrNode[arrNode.length] = 'hasChild' + treeObjects.attribute_suffix + "true";
            treeObjects.nodes[treeObjects.expandingNode.sid + '_' + ++treeObjects.total] = arrNode.join(treeObjects.attribute_d);
        }
    }

    if(printTemplateInfo.PageFooter.AreaHeight != "0"){
        var arrNode = new Array();
        arrNode[arrNode.length] = 'uuid' + treeObjects.attribute_suffix + "printTemplateInfo_dot_PageFooter";
        arrNode[arrNode.length] = 'text' + treeObjects.attribute_suffix + "Page Footer";
        arrNode[arrNode.length] = 'type' + treeObjects.attribute_suffix + "printTemplateInfo_dot_PageFooter";
        arrNode[arrNode.length] = 'nodeuuid' + treeObjects.attribute_suffix + "printTemplateInfo_dot_PageFooter";
        arrNode[arrNode.length] = 'hasChild' + treeObjects.attribute_suffix + "true";
        treeObjects.nodes[treeObjects.expandingNode.sid + '_' + ++treeObjects.total] = arrNode.join(treeObjects.attribute_d);
    }

    treeObjects.dataFormat();
    treeObjects.load(treeObjects.expandingNode.id);
    treeObjects.buildNode(treeObjects.expandingNode.id);
        
    //new add by lzy to expand the last visiting node.
    treeObjects.locateNode("UUID");

    HideWait();
}
   
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	createEachNode
//| Author		:	itc98079/刘晓玲
//| Create Date	:	4/24/2009
//| Description	:	创建每个树节点，被createNodesForEachArea,InitChild_Objects调用
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function createEachNode(uuid, text, type, nodeuuid, hasChild){

    var arrNode = new Array();
    arrNode[arrNode.length] = 'uuid' + treeObjects.attribute_suffix + uuid;
    arrNode[arrNode.length] = 'text' + treeObjects.attribute_suffix + text;
    arrNode[arrNode.length] = 'type' + treeObjects.attribute_suffix + type;
    arrNode[arrNode.length] = 'nodeuuid' + treeObjects.attribute_suffix + nodeuuid;
    arrNode[arrNode.length] = 'hasChild' + treeObjects.attribute_suffix + hasChild;
    treeObjects.nodes[treeObjects.expandingNode.sid + '_' + ++treeObjects.total] = arrNode.join(treeObjects.attribute_d);                 
    
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	createNodesForEachArea
//| Author		:	itc98079/刘晓玲
//| Create Date	:	4/24/2009
//| Description	:	创建每个区域下六种类型的节点，被InitChild_Objects调用
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    

function createNodesForEachArea(obj, uuid){
    if((obj.ArearObjects.length == 0) && (obj.BarcodeObjects.length == 0) && (obj.LineObjects.length == 0)
            && (obj.TextObjects.length == 0) && (obj.PictureObjects.length == 0) && (obj.RectangleObjects.length == 0))
    {
        treeObjects.emptyNode(treeObjects.expandingNode);
    }
    if(obj.ArearObjects.length != 0){
        createEachNode(uuid + "_dot_ArearObjects", "Areas", "ArearObjects", uuid + "_dot_ArearObjects", "true");
    }

    if(obj.BarcodeObjects.length != 0){
        createEachNode(uuid + "_dot_BarcodeObjects", "Barcodes", "BarcodeObjects", uuid + "_dot_BarcodeObjects", "true");
    }


    if(obj.LineObjects.length != 0){
        createEachNode(uuid + "_dot_LineObjects", "Lines", "LineObjects", uuid + "_dot_LineObjects", "true");
    }
    
    if(obj.TextObjects.length != 0){
        createEachNode(uuid + "_dot_TextObjects", "Texts", "TextObjects", uuid + "_dot_TextObjects", "true");
    }
    
    if(obj.PictureObjects.length != 0){
        createEachNode(uuid + "_dot_PictureObjects", "Pictures", "PictureObjects", uuid + "_dot_PictureObjects", "true");
    }
                
    if(obj.RectangleObjects.length != 0){
        createEachNode(uuid + "_dot_RectangleObjects", "Rectangles", "RectangleObjects", uuid + "_dot_RectangleObjects", "true");
    }    
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	InitChild_Objects
//| Author		:	itc98079/刘晓玲
//| Create Date	:	4/24/2009
//| Description	:	创建树节点
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function getRealObjectName(RealObjectName,type){
    var tmpName;
    if(RealObjectName == ""){
        tmpName = type;
    }else{
        tmpName = type + "(" + RealObjectName +")";
    }
    return tmpName;
}
function InitChild_Objects(type, uuid)
{
    var obj = idToObj(uuid);

    
    switch(type){
        case "printTemplateInfo_dot_PageHeader":
        case "printTemplateInfo_dot_PageFooter":
        case "printTemplateInfo_dot_DetailSections_dot_HeaderArea":
        case "printTemplateInfo_dot_DetailSections_dot_Cells":
            createNodesForEachArea(obj, uuid);
            break;

        case "printTemplateInfo_dot_DetailSections":

            if(obj.HeaderHeight != "0"){
                createEachNode(uuid + "_dot_HeaderArea", "Header", "printTemplateInfo_dot_DetailSections_dot_HeaderArea", uuid + "_dot_HeaderArea", "true");
            }

            if(obj.RowHeight != "0"){
                for(var i = 0;i< obj.Cells.length;i++){
                    createEachNode(uuid + "_dot_Cells_a" + i + "_b", "Cell_" + (i+1), "printTemplateInfo_dot_DetailSections_dot_Cells", uuid + "_dot_Cells_a" + i + "_b", "true");
	            }
            }
            break;
            
                        
        case "BarcodeObjects":
            for(var i=0;i<obj.length;i++){
                createEachNode(uuid+"_a"+i+"_b", getRealObjectName(obj[i].RealObjectName, "Barcode"), "BarcodeObject", obj[i].ObjectName, "false");
            }
            break;

        case "RectangleObjects":
            for(var i=0;i<obj.length;i++){
                createEachNode(uuid+"_a"+i+"_b", getRealObjectName(obj[i].RealObjectName, "Rectangle"), "RectangleObject", obj[i].ObjectName, "false");
            }
            break;

        case "ArearObjects":
            for(var i=0;i<obj.length;i++){
                createEachNode(uuid+"_a"+i+"_b", getRealObjectName(obj[i].RealObjectName, "Area"), "ArearObject", obj[i].ObjectName, "false");
            }
            break;

        case "PictureObjects":
            for(var i=0;i<obj.length;i++){
                createEachNode(uuid+"_a"+i+"_b", getRealObjectName(obj[i].RealObjectName, "Picture"), "PictureObject", obj[i].ObjectName, "false");
            }
            break;

        case "LineObjects":
            for(var i=0;i<obj.length;i++){
                createEachNode(uuid+"_a"+i+"_b", getRealObjectName(obj[i].RealObjectName, "Line"), "LineObject", obj[i].ObjectName, "false");
            }
            break;

        case "TextObjects":
            for(var i=0;i<obj.length;i++){
                createEachNode(uuid+"_a"+i+"_b", getRealObjectName(obj[i].RealObjectName, "Text"), "TextObject", obj[i].ObjectName, "false");
            }
            break;
    }
    
    treeObjects.dataFormat();
    treeObjects.load(treeObjects.expandingNode.id);
    treeObjects.buildNode(treeObjects.expandingNode.id);
        
    //new add by lzy to expand the last visiting node.
    treeObjects.locateNode("UUID");

    HideWait();

}
createTree();
createTree_Objects();

</script>

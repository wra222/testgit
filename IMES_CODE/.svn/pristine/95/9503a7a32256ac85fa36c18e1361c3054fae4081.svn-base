
<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: ģ���趨�򵼿��ҳ��
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)  create
' Known issues:Any restrictions about this file
--%> 
<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_main_EditPanel, App_Web_editpanel.aspx.39cd9290" theme="MainTheme" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"  runat="server">
	<title>Edit Panel</title>
	<script type="text/javascript" src="jquery/jquery-1.3.2.js"></script>
	<script type="text/javascript" src="jquery/ui/ui.core.js"></script>
	<script type="text/javascript" src="jquery/ui/ui.draggable.js"></script>
	<script type="text/javascript" src="jquery/ui/ui.droppable.js"></script>
	<script type="text/javascript" src="jquery/ui/ui.selectable.js"></script>
	<style type="text/css">
	.nodisplay { visibility:hidden; }
	span {
		position:absolute;
		border: 1px solid black;
		width:4px;
		height:4px;
		background-color:black;
		font-size:0;
	}

 	span.north-resize	  {border-color:black; left:50%;top:0;	margin-left:-3px; margin-top:-1px;}
	span.west-resize	  {border-color:black; left:0;top:50%;	margin-left:-1px; margin-top:-3px;}
	span.east-resize	  {border-color:black; right:0;top:50%; margin-right:-1px;margin-top:-3px;}
	span.south-resize	  {border-color:black; left:50%;bottom:0;margin-left:-3px; margin-bottom:-1px;}


	.draggable_tool { width: 100px; height: 30px; padding: 0em; margin: 0 0 0 0; border: 0px solid #CCC;z-index:0;vertical-align:middle;z-index:20000;}

	.draggableText { background:url("text.jpg");position:absolute; width: 50px; height: 24px; padding: 0em; margin: 0 0 0 0; overflow:visible;white-space:nowrap;  border: 1px dashed black;z-index:10000;}

	.draggableLine { background:url("line.jpg");font-size:0px;position:absolute; width: 50px; height: 10px; padding: 0em; margin: 0 0 0 0; border: 1px dashed black; z-index:10000;}

	.draggableRectangle { background:url("rectangle.jpg");position:absolute; width: 40px; height: 20px; padding: 0em; margin: 0 0 0 0; overflow:visible;white-space:nowrap;  border: 1px solid black;z-index:10000;}

	.draggableArea { background:url("area.jpg");position:absolute; width: 60px; height: 40px; padding: 0em; margin: 0 0 0 0; overflow:visible;white-space:nowrap;  border: 1px dashed black;z-index:10000;}
	
	.draggableBarcode { background:url("barcode.gif");position:absolute; width: 40px; height: 20px; padding: 0em; margin: 0 0 0 0; overflow:visible;white-space:nowrap;  border: 1px dashed black;z-index:10000;}

	.draggablePicture { background:url("picture.jpg"); position:absolute; width: 40px; height: 20px; padding: 0em; margin: 0 0 0 0; overflow:visible;white-space:nowrap;  border: 1px dashed black;z-index:10000;}

	.ui-state-hover {background-color:Blue}

	.ui-state-active {background-color:LightBlue}
	
	#page_header_body .ui-selecting, #section1_body .ui-selecting, #section2_body .ui-selecting,#page_footer_body .ui-selecting { background: #FECA40; }
	#page_header_body .ui-selected, #section1_body .ui-selected, #section2_body .ui-selected,#page_footer_body .ui-selected { background: #F39814; color: white; }
		
	.area_title{
		position: absolute;
		background-color:red;
		border-bottom:solid red 0px;
		border-left:solid red 0px;
		border-top:solid red 0px;
		border-right:solid red 0px;
	}

	.area_body {
		position: absolute;
		border: solid red 0px;
		border-bottom:solid red 1px;
		border-left:solid red 1px;
		border-top:solid red 0px;
		border-right:solid red 1px;
		z-index: 10000;
	}
	
	.area_body_cell {
		position: absolute;
		border-bottom:solid red 1px;
		border-left:solid red 1px;
		border-top:solid red 1px;
		border-right:solid red 1px;
		z-index: 10000;
	}


	button .menu {
		position:absolute;
		width:20px;
	}		
	
	
	
.title { background-color: rgb(156,192,248);height:22px;font:normal normal bold 9pt Verdana;width:100%;FILTER: progid:DXImageTransform.Microsoft.Alpha( style=3,opacity=25,finishOpacity=100);}
	

	</style>
	

	
	<script type="text/javascript">
//	#page_header_body .ui-selected, #section1_body .ui-selected, #section2_body .ui-selected,#page_footer_body .ui-selected { background: #F39814; color: white; }
//	#page_header_body .ui-selected, #section1_body .ui-selected, #section2_body .ui-selected,#page_footer_body .ui-selected	{filter:progid:DXImageTransform.Microsoft.Light();}

	var panel_title_height = 20;
	var pixel_per_inch = 96;//96pixels/inch
	var mm_per_inch = 25.4;//25.4mm/inch
	var focus_container;
	var AREA_TITLE_HEIGHT = 20;
	var selectable_obj, droppable_obj;
	var g_elementID = 0;


	function delSelected(){
		//////////////////////////////alert($("#header_container").html());
		alert(focus_container);
		$("#"+focus_container+" .ui-selected").each(function(){
			$(this).replaceWith("");
		})
	}
	function alignBottom(){
		var maxIndex = $("#"+focus_container+" div").filter(".ui-selected").size() - 1;
		var reference = $("#"+focus_container+" div").filter(".ui-selected").eq(maxIndex);
		var ref_top = reference.offset().top;
		var ref_bottom = ref_top + reference.height();
		$("#"+focus_container+" div").filter(".ui-selected").each(function(){
			if($("#"+focus_container+" div").filter(".ui-selected").index(this) == maxIndex){
				return false;
			}
			var each_top = $(this).offset().top;
			var each_bottom = each_top + $(this).height();
			var diff_bottom = ref_bottom - each_bottom;
			each_top = each_top + diff_bottom;
			$(this).css("top",each_top-$(this).offsetParent().offset().top);
		})
	}
	
	function alignTop(){
		var maxIndex = $("#"+focus_container+" div").filter(".ui-selected").size() - 1;
		var reference = $("#"+focus_container+" div").filter(".ui-selected").eq(maxIndex);
		var ref_top = reference.offset().top;
		$("#"+focus_container+" div").filter(".ui-selected").each(function(){
			if($("#"+focus_container+" div").filter(".ui-selected").index(this) == maxIndex){
				return false;
			}
			var each_top = $(this).offset().top;
			var diff_top = ref_top - each_top;
			each_top = each_top + diff_top;
			$(this).css("top",each_top-$(this).offsetParent().offset().top);
		})
	}

	function alignLeft(){
		var maxIndex = $("#"+focus_container+" div").filter(".ui-selected").size() - 1;
		var reference = $("#"+focus_container+" div").filter(".ui-selected").eq(maxIndex);
		var ref_left = reference.offset().left;
		$("#"+focus_container+" div").filter(".ui-selected").each(function(){
			if($("#"+focus_container+" div").filter(".ui-selected").index(this) == maxIndex){
				return false;
			}
			var each_left = $(this).offset().left;
			var diff_left = ref_left - each_left;
			each_left = each_left + diff_left;
			$(this).css("left",each_left-$(this).offsetParent().offset().left);
		})
	}
	
	function alignRight(){
		var maxIndex = $("#"+focus_container+" div").filter(".ui-selected").size() - 1;
		var reference = $("#"+focus_container+" div").filter(".ui-selected").eq(maxIndex);
		var ref_left = reference.offset().left;
		var ref_right = ref_left + reference.width();
		$("#"+focus_container+" div").filter(".ui-selected").each(function(){
			if($("#"+focus_container+" div").filter(".ui-selected").index(this) == maxIndex){
				return false;
			}
			var each_left = $(this).offset().left;
			var each_right = each_left + $(this).width();
			var diff_right = ref_right - each_right;
			each_left = each_left + diff_right;
			$(this).css("left",each_left-$(this).offsetParent().offset().left);
		})
	}

	function alignCenter(){
		var maxIndex = $("#"+focus_container+" div").filter(".ui-selected").size() - 1;
		var reference = $("#"+focus_container+" div").filter(".ui-selected").eq(maxIndex);
		var ref_left = reference.offset().left;
		var ref_center = ref_left + reference.width()/2;
		$("#"+focus_container+" div").filter(".ui-selected").each(function(){
			if($("#"+focus_container+" div").filter(".ui-selected").index(this) == maxIndex){
				return false;
			}
			var each_left = $(this).offset().left;
			var each_center = each_left + $(this).width()/2;
			var diff_center = ref_center - each_center;
			each_left = each_left + diff_center;
			$(this).css("left",each_left-$(this).offsetParent().offset().left);
		})
	}
	
	function alignMiddle(){
		var maxIndex = $("#"+focus_container+" div").filter(".ui-selected").size() - 1;
		var reference = $("#"+focus_container+" div").filter(".ui-selected").eq(maxIndex);
		var ref_top = reference.offset().top;
		var ref_middle = ref_top + reference.height()/2;
		$("#"+focus_container+" div").filter(".ui-selected").each(function(){
			if($("#"+focus_container+" div").filter(".ui-selected").index(this) == maxIndex){
				return false;
			}
			var each_top = $(this).offset().top;
			var each_middle = each_top + $(this).height()/2;
			var diff_middle = ref_middle - each_middle;
			each_top = each_top + diff_middle;
			$(this).css("top",each_top-$(this).offsetParent().offset().top);
		})
	}
	
	function eddy(){
		var strFilter_90 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=1)";
		var strFilter_180 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=2)";
		var strFilter_270 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=3)";
		var strFilter_0 = "progid:DXImageTransform.Microsoft.BasicImage(Rotation=0)";
		
		$("#"+focus_container+" div").filter(".ui-selected").each(function(){
			if(this.style.filter == "" || this.style.filter == strFilter_0){
				this.style.filter = strFilter_90;
			}else if(this.style.filter == strFilter_90){
				this.style.filter = strFilter_180;
			}else if(this.style.filter == strFilter_180){
				this.style.filter = strFilter_270;
			}else if(this.style.filter == strFilter_270){
				this.style.filter = strFilter_0;
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

    var g_SelectedGrandson = false;
	$(function() {
		$("#draggableText_tool").draggable({
			opacity: 0.7,
			helper: function(){
			    return $('#draggableText').clone().css({"visibility":"visible"});
			}
		});

		$("#draggableLine_tool").draggable({
			opacity: 0.7,
			helper: function(){
			    return $('#draggableLine').clone().css({"visibility":"visible"});
			}
		});

		$("#draggableRectangle_tool").draggable({
			opacity: 0.7,
			helper: function(){
			    return $('#draggableRectangle').clone().css({"visibility":"visible"});
			}
		});

		$("#draggableArea_tool").draggable({
			opacity: 0.7,
			helper: function(){
			    return $('#draggableArea').clone().css({"visibility":"visible"});
			}
		});

		$("#draggableBarcode_tool").draggable({
			opacity: 0.7,
			helper: function(){
			    return $('#draggableBarcode').clone().css({"visibility":"visible"});
			}
		});

		$("#draggablePicture_tool").draggable({
			opacity: 0.7,
			helper: function(){
			    return $('#draggablePicture').clone().css({"visibility":"visible"});
			}
		});

		selectable_obj = {
			tolerance: 'fit',
			filter: '.son',
			//cancel: 'div.grandson',
			start: function(){
			    //focus_container = $(this).attr("id");
				//alert("selectable_obj:start:"+focus_container);
			},
			stop: function(){//ѡ��������
				//alert("selectable_obj:stop:out:"+focus_container);
				/*
			    if(focus_container.indexOf("draggableArea") != -1){//ֱ�ӷ��أ�������Ӧ����Ϊ�����ǰ��������Area(����)�������Լ���selectable_obj_1�д�������Բ�Ҫ�ٴ��������ף�selectable_obj
			        return false;
			    }*/
		        /*$("#" + focus_container + " .ui-selected span").removeClass("yes")
		                                                       .css({"visibility":"hidden","background-color":"transparent"});
		        $("#" + focus_container + " .ui-selected").removeClass("ui-selected")
		                                                  .removeClass("yes")
		                                                  .draggable('disable')
		                                                  .css({cursor:"auto"});*/
		        //alert("stop start");
		        //����draggableArea�����������grandson��Ҳ����û�У����g_SelectedGrandson��Ҫ�����渳ֵ
			    
                var count = $(".ui-selected",this).size();
                if(count > 0){
		            focus_container = $(this).attr("id");
                }
		        $(".ui-selected",this).each(function(){
	                //alert($(this).attr("class"));
		            //alert("father:selected:in::"+focus_container+"::"+$(this).attr("id"));
	                //�õ����㣬������ק
		            $(this).draggable('enable')
		                   .css({cursor:"move"})
		                   .addClass("yes");
		            ////////////////////this.filters[0].addAmbient(255,187,119,100);//ѡ�к��͸��Ч��
		            //$(this).css({cursor:"move","z-index":10});//������룬����
		            ////////////////////$(this).css({"background-color":"black"});//��Ӧ���Է�����ʾ
		            ////////////////////$(this).css({"color":"white"});
		            ////////////////////$(this).css({"font-size":"30px"});//��Ӧ���������С
		            ////////////////////this.innerHTML = this.innerHTML.replace(this.innerText,"dddddddd");//��Ӧ���ԣ���̬�޸�����

	                var cssobj;
	                if(count == 1){//���һ��
	                    cssobj = {"visibility":"visible","background-color":"black"}
	                }else{
	                    cssobj = {"visibility":"visible","background-color":"transparent"}
	                }
                    $("span", this).css(cssobj)
	                               .addClass("yes");
                    
		            count = count - 1;
		        });

		        //alert("stop end");
			},
			
			unselected: function(event,ui){
			
	            //alert("unselected");
		        if($("div span.yes",this).size() == 0){
			        return false;
		        }
		        $("div span.yes",this).css({"visibility":"hidden", "background-color":"transparent"})
			                          .removeClass("yes");

		        //ʧȥ���㣬������ק
		        $("div.yes",this).draggable('disable')
			                     .removeClass("yes")
			                     .css({cursor:"auto"});
			},
			selected: function(event,ui){
			/*
			    if($(this).is(".son")){
			        focus_container = $(this).attr("id");
			        g_SelectedGrandson = true;
			    }
			    if(g_SelectedGrandson == false && $(this).is(".father")){
			    
			    }
			    */
			    /*
			alert("selected start");
			alert($(".ui-selected", this).size());
			alert($(this).attr("id"));
			    $(".ui-selected", this).each(function(){
			        alert($(this).is(".grandson"));
				    if($(this).is(".grandson")){
                            alert("selectedgrandson::"+$(this).attr("id"));
				        g_SelectedGrandson = true;
				        return;
                    }
                    if(g_SelectedGrandson == true){
                        if($(this).is(".son")){
                            alert("selectedson::"+$(this).attr("id"));
	                        $(this).removeClass("ui-selected")
                                   .removeClass("yes")
                                   .draggable('disable')
                                   .css({cursor:"auto"});
                        }
                    }
                });
                alert("selected end");
                */
			}
		
		};
//		$("#header_container").selectable(selectable_obj);
//		$("#detail_container").selectable(selectable_obj);

		

		droppable_obj = {
			accept: '#toolbox > div',
			tolerance: 'fit',
			//activeClass: 'ui-state-hover',
			hoverClass: 'ui-state-active',
			drop: function(ev, ui) {
				focus_container = this.id;
				var obj_clone;
				if(ui.draggable.attr("id") == "draggableText_tool"){
					obj_clone = "#draggableText";
				}else if(ui.draggable.attr("id") == "draggableLine_tool"){
					obj_clone = "#draggableLine";
				}else if(ui.draggable.attr("id") == "draggableRectangle_tool"){
					obj_clone = "#draggableRectangle";
				}else if(ui.draggable.attr("id") == "draggableArea_tool"){
					obj_clone = "#draggableArea";
				}else if(ui.draggable.attr("id") == "draggableBarcode_tool"){
					obj_clone = "#draggableBarcode";
				}else if(ui.draggable.attr("id") == "draggablePicture_tool"){
					obj_clone = "#draggablePicture";
				}
				var cloneObj = $(obj_clone).clone(true);//ui.draggable.clone();

                cloneObj.attr("id",cloneObj.attr("id") + g_elementID);
                g_elementID = g_elementID + 1;

				cloneObj.appendTo($(this));
				cloneObj.draggable({
					containment: 'parent',
					cursor:'move',
					start:function(){
						$(this).parent().selectable('disable');//��ý������꿪ʼ�϶�������select����
					},
					stop:function(){
						$(this).parent().selectable('enable');//���ֹͣ�϶�������select����
						//alert($(this).attr("class"));
						$(this).addClass("ui-selected");
					}
				});
				
				 cloneObj.addClass("ui-selected")
				         .addClass("yes")
				         .addClass("son")
				         .removeClass("nodisplay");
	            ////////////////////////////document.getElementById(cloneObj.attr("id")).filters[0].addAmbient(255,187,119,100);//ѡ�к��͸��Ч��
				         
				 cloneObj.css("position", "absolute");
				 cloneObj.css("top",  ui.offset.top - cloneObj.offsetParent().offset().top);//drop�󣬶���λ�õ�top
				 cloneObj.css("left", ui.offset.left - cloneObj.offsetParent().offset().left);//drop�󣬶���λ�õ�left
				 cloneObj.css({"cursor":"move"});

				 cloneObj.children("span").addClass("yes");
				 cloneObj.children("span").css({"visibility":"visible","background-color":"black"});//�����ĸ��ý����־
				 
				 
				 //cloneObj.addClass("last");
				//alert(ui.position.top);
				//alert(ui.position.left);
				//alert(cloneObj.offsetParent().offset().top);
				//alert(cloneObj.offsetParent().offset().left);
				//cloneObj.css("position: absolute; top: 50; left: 50;");
				//alert(cloneObj.offsetParent().attr("id"));
				//cloneObj.offsetParent().css("color","green");
			}
		};
		

		/*
		$("#header_container").selectable(selectable_obj).droppable(droppable_obj).mousemove(function(e){
								        $("#rain").html(e.clientX+' , '+e.clientY);
									});
		$("#detail_container").selectable(selectable_obj).droppable(droppable_obj).mousemove(function(e){
								        $("#rain").html(e.pageX+' , '+e.pageY);
									});
		*/
		
	});

	function highlight_tool(tool){
		$("#toolbox .draggable_tool").each(function(){	
				$(this).css({"border":"0px solid red"});
		
		});
		$(tool).css({"border":"1px solid red"});
		
		//����toolbox��ĳ������ʱ����Ҫ����������Ľ���ʧȥ���������ʧ����Ĵ���,ע�⣺ʧ�����ͬʱҲ�Ͳ�����ק��
		$("#" + focus_container + " .ui-selected > span").removeClass("yes")
		                                               .css({"visibility":"hidden","background-color":"transparent"});
		$("#" + focus_container + " .ui-selected").removeClass("ui-selected")
		                                          .removeClass("yes")
		                                          .draggable('disable')
		                                          .css({cursor:"auto"});
		
	}

	
	function createArea(){
	        //getPrintTemplateInfoFromStructure
            var rtn = com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
            if (rtn.error != null) {
                alert(rtn.error.Message);
            } else {

                //���ģ��ṹ
                printTemplateInfo = rtn.value;
            }	        
	        
			var strAreasID;
			//////////////////strAreasID = "#page_header_title,#page_header_body";
			var coor_top = 0;
			var area_width = 300;
			var inner_header_height = 30;
			var inner_cell_height = 200;
			var num_DataSet = 3;
			
			//page header
			var header_title = $("#area_title").clone(true);
			header_title.attr({"id":"page_header_title"})
			            .css({"width":area_width,"height":"20","top":coor_top})
			            .removeClass("nodisplay")
			            .html("Page Header");
            $("#editarea").append(header_title);
            
			coor_top = coor_top + 20;
			var header_body = $("#area_body").clone(true);
			header_body.attr({"id":"page_header_body"})
			            .css({"width":area_width,"top":coor_top,"height":"150"})
			            .removeClass("nodisplay")
						.selectable(selectable_obj)
						.droppable(droppable_obj)
						.mousemove(function(e){
						    var coor_x = e.pageX;
						    var coor_y = e.pageY;
						    var parent_y = $(this).offsetParent().offset().top;
						    var parent_x = $(this).offsetParent().offset().left;
						    var y = Math.round((coor_y - parent_y - panel_title_height)/pixel_per_inch*mm_per_inch*10)/10;
						    var x = Math.round((coor_x - parent_x)/pixel_per_inch*mm_per_inch*10)/10;
						    
					        $("#rain").html('X:'+x+' , Y:'+y);
					    })
					    .mousedown(function(e){
					        highlight_tool(this);
					    });			
					    			
            $("#editarea").append(header_body);

            //section
			coor_top = coor_top + 150;
			var section1_title = $("#area_title").clone(true);
			section1_title.attr({"id":"section1_title"})
			            .css({"width":area_width,"top":coor_top})
			            .removeClass("nodisplay")
			            .html("Detail(Section 1)");
            $("#editarea").append(section1_title);
			
        
			coor_top = coor_top + 20;
			var section1_body = $("#area_body").clone(true);
			section1_body.attr({"id":"section1_body"})
			            .css({"width":area_width,"top":coor_top,"height":"350"})
			            .removeClass("nodisplay")
            $("#editarea").append(section1_body);
			//Detail(Section 2),Page Footer
			
			//area inner
			var coor_body_inner_top = 0;
			var section1_body_inner_header = $("#area_body_cell").clone(true);
			section1_body_inner_header.attr({"id":"area_body_inner_header"})
			            .css({"width":area_width,"top":coor_body_inner_top,"height":inner_header_height,"border":"solid red 0px"})
			            .removeClass("nodisplay")
						.selectable(selectable_obj)
						.droppable(droppable_obj)
						.mousemove(function(e){
						    var coor_x = e.pageX;
						    var coor_y = e.pageY;
						    var parent_y = $(this).offsetParent().offset().top;
						    var parent_x = $(this).offsetParent().offset().left;
						    var y = Math.round((coor_y - parent_y)/pixel_per_inch*mm_per_inch*10)/10;
						    var x = Math.round((coor_x - parent_x)/pixel_per_inch*mm_per_inch*10)/10;
						    
					        $("#rain").html('X:'+x+' , Y:'+y);
					    })
					    .mousedown(function(e){
					        highlight_tool(this);
					    });			
            $("#section1_body").append(section1_body_inner_header);			
            
			coor_body_inner_top = coor_body_inner_top + inner_header_height;
			var body_inner_cell_left = 0;
			//�õ�ÿ��cell�Ŀ��
			var body_inner_cell_width = area_width/num_DataSet;
			//ѭ������ÿ��cell
			for(var i=1;i<=num_DataSet;i++){
			    //cell��left
			    body_inner_cell_left = (i-1)*body_inner_cell_width;
			    //����cell����
			    var section1_body_inner_cell = $("#area_body_cell").clone(true);
			    //����cell��Ψһid
			    section1_body_inner_cell.attr("id","area_body_inner_cell"+i)
			                .css({"width":body_inner_cell_width,"top":coor_body_inner_top,"left":body_inner_cell_left,"height":inner_cell_height,"border-left":"solid red 1px","border-right":"solid red 1px"})
			                .removeClass("nodisplay")
						    .selectable(selectable_obj)
						    .droppable(droppable_obj)
						    .mousemove(function(e){
						        var coor_x = e.pageX;
						        var coor_y = e.pageY;
						        var parent_y = $(this).offsetParent().offset().top;
						        var parent_x = $(this).offsetParent().offset().left + parseInt($(this).css("left"));
						        var y = Math.round((coor_y - parent_y - coor_body_inner_top)/pixel_per_inch*mm_per_inch*10)/10;
						        var x = Math.round((coor_x - parent_x)/pixel_per_inch*mm_per_inch*10)/10;
    						    
					            $("#rain").html('X:'+x+' , Y:'+y);
					        })
					        .mousedown(function(e){
					            highlight_tool(this);
					        });			
                $("#section1_body").append(section1_body_inner_cell);	
            }		

            
	}
	

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
			document.getElementById("dataset_button").style.width = "3%";
			document.getElementById("editarea").style.width = "97%";
			document.getElementById("tableTitle").style.display="none";
			document.getElementById("table_dataset_title").style.display="none";
			$("#toolbox").css({"overflow":"hidden"});
			$("#dataset").css({"overflow":"hidden"});
			$(".dataset_button").addClass("nodisplay");
			$(".draggable_tool").addClass("nodisplay");
			
	    }
	    else
		{
			document.getElementById("shleft").src="../../images/nav.gif";
			document.getElementById("toolbox").style.width = "20%";
			document.getElementById("dataset").style.width = "20%";
			document.getElementById("dataset_button").style.width = "20%";
			document.getElementById("editarea").style.width = "80%";
			document.getElementById("tableTitle").style.display="";
			document.getElementById("table_dataset_title").style.display="";
			$("#toolbox").css({"overflow":"visible"});
			$("#dataset").css({"overflow":"visible"});
			$(".dataset_button").removeClass("nodisplay");
			$(".draggable_tool").removeClass("nodisplay");
		}	
	}
	
	
	</script>

</head>

<%--load all js files--%>
<fis:header id="header1" runat="server"/>

<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
<div id="toolbar" style="overflow:hidden;position:absolute;height:5%;width:100%;left:0;top:0;border:1px solid black;">


<input type="button" value="del" onclick="delSelected();" class="menu">
<input type="button" value="Bottom" onclick="createArea();" class="menu">
<input type="button" value="Top" onclick="alignTop();" class="menu">
<input type="button" value="Left" onclick="alignLeft();" class="menu">
<input type="button" value="Right" onclick="alignRight();" class="menu">
<input type="button" value="Center" onclick="alignCenter();" class="menu">
<input type="button" value="Middle" onclick="alignMiddle();" class="menu">
<input type="button" value="eddy" onclick="eddy();" class="menu">
<h2 id="rain" style="position:absolute;right:0px;">X: ,Y: </h2>
</div><!-- End toolbar -->


<div id="editarea" style="overflow:scroll;position:absolute;width: 80%; top:5%;height: 90%; left: 0px; right: auto;border:1px solid black;border-top:0px solid blue;">

<div id="area_title" class="area_title nodisplay"></div>
<div id="area_body" class="area_body nodisplay"></div>

<div id="area_body_cell" class="area_body_cell nodisplay"></div>

</div><!-- End editarea -->




<div id="toolbox" style="position:absolute;left: auto;top:5%; right: 0px;width:20%;height:45%;border:0px solid black;">
	<table  style="width:100%; border:0px" id="tableTitle">
	    <tr>
	    <td class="title" style=" border:0px; word-wrap:normal;">Object</td>
	    </tr>
	</table>
    <div style= "position:absolute;right:0px;top:0px;">
        <INPUT style="" type="image" src="../../images/nav.gif" value="button" id="shleft" name="shleft" onclick="OpenClose()">
    </div>    
	<div id="draggableText_tool" class="draggable_tool" onmousedown="highlight_tool(this);">
	<img src="toolbox.jpg">Text
	</div>
	<div id="draggableLine_tool" class="draggable_tool" onmousedown="highlight_tool(this);">
	<img src="toolbox.jpg">Line
	</div>
	<div id="draggableRectangle_tool" class="draggable_tool" onmousedown="highlight_tool(this);">
	<img src="toolbox.jpg">Rectangle
	</div>
	<div id="draggableArea_tool" class="draggable_tool" onmousedown="highlight_tool(this);">
	<img src="toolbox.jpg">Area
	</div>
	<div id="draggableBarcode_tool" class="draggable_tool" onmousedown="highlight_tool(this);">
	<img src="toolbox.jpg">Barcode
	</div>
	<div id="draggablePicture_tool" class="draggable_tool" onmousedown="highlight_tool(this);">
	<img src="toolbox.jpg">Picture
	</div>


    <div id="draggableText" class="draggableText nodisplay yes">
	    <span class="north-resize"></span>
	    <span class="west-resize"> </span>
	    <span class="east-resize"> </span>
	    <span class="south-resize"></span>
    </div>

    <div id="draggableLine" class="draggableLine nodisplay yes">
	    <span class="west-resize"> </span>
	    <span class="east-resize"> </span>
    </div>

    <div id="draggableRectangle" class="draggableRectangle nodisplay yes">
	    <span class="north-resize"></span>
	    <span class="west-resize"> </span>
	    <span class="east-resize"> </span>
	    <span class="south-resize"></span>
    </div>

    <div id="draggableArea" class="draggableArea nodisplay yes">
	    <span class="north-resize"></span>
	    <span class="west-resize"> </span>
	    <span class="east-resize"> </span>
	    <span class="south-resize"></span>
    </div>

    <div id="draggableBarcode" class="draggableBarcode nodisplay yes">
	    <span class="north-resize"></span>
	    <span class="west-resize"> </span>
	    <span class="east-resize"> </span>
	    <span class="south-resize"></span>
    </div>

    <div id="draggablePicture" class="draggablePicture nodisplay yes">
	    <span class="north-resize"></span>
	    <span class="west-resize"> </span>
	    <span class="east-resize"> </span>
	    <span class="south-resize"></span>
    </div>

</div><!-- End toolbox -->

<div id="dataset" style="position:absolute;left: auto;top:50%; right: 0px;width:20%;height:40%;border:0px solid black;border-bottom:1px solid black;border-top:1px solid black;">
	<table  style="width:100%; border:0px" id="table_dataset_title">
	    <tr>
	    <td class="title" style=" border:0px; word-wrap:normal;">DataSet</td>
	    </tr>
	</table>
	<div style="overflow:hidden;width:100%;border:0px solid red;"></div>
</div>

<div id="dataset_button" align="center" style="position:absolute;left:auto;top:90%;right: 0px;width:20%;height:5%;border:0px solid red;border-bottom:1px solid black;">
<input type="button" value="Add" onclick="save();" class="dataset_button">
<input type="button" value="Edit" onclick="alignBottom();" class="dataset_button">
<input type="button" value="Del" onclick="alignTop();" class="dataset_button">
</div><!-- End dataset_button -->


<div id="button_area" align=right style="overflow:hidden;position:absolute;height:10%;width:100%;left:0;top:95%;border:0px solid red;">
<input type="button" value="Save" onclick="save();">
<input type="button" value="Preview" onclick="alignBottom();">
<input type="button" value="Print" onclick="alignTop();">
<input type="button" value="Template Setting" onclick="alignLeft();">
</div><!-- End button_area -->


</body>
<%--set style for readonly input controller--%>
<fis:footer id="footer1" runat="server"/> 

</html>

$(document).ready(function(){

$(".closeParent").click(function(){$(this).parent().hide();});
//$('#mainNav a:first-child').addClass('nobg');
$("ul.tabMenu li:first-child").addClass("current");
$("div.tabContent article:first-child").show();
/*$(".tabContent article").attr("id", function(){return "No" + $(".tabContent article").index(this)});
$(".tabMenu li").hover(function(){
	$(this).addClass("current").siblings().removeClass("current");
	var c = $(".tabMenu li");
	var j = c.index(this);
	var objname = "No" + j;
	$("#"+objname).siblings().hide();
	$("#"+objname).fadeIn();
},function(){});*/
$("ul.tabMenu li").each(function(index){
$(this).hover(function(){
var tabin = $(this); 
timeoutid = setTimeout(function(){
tabin.addClass("current").siblings().removeClass("current");
$(".tabContent article:eq(" + index + ")").fadeIn().siblings("article").hide();
},150);
},function(){clearTimeout(timeoutid);});
});

$("#tabMenu2 li:first-child").addClass("current");
$("#tabContent2 article:first-child").show();
$("#tabContent2 article").attr("id", function(){return "Noid" + $("#tabContent2 article").index(this)});
$("#tabMenu2 li").click(function(){
	$(this).addClass("current").siblings().removeClass("current");
	var c = $("#tabMenu2 li");
	var j = c.index(this);
	var objname = "Noid" + j;
	$("#"+objname).siblings().hide();
	$("#"+objname).show();
});

//20130131 topmenu
$("#jq_topmenu li").hover(function(){$(this).addClass("hover").find("div.jq_hidebox").show();},function(){$(this).removeClass("hover").find("div.jq_hidebox").hide();});


$('.list04 li:even').addClass('bgcolor');
var _wrap=$('ul.mulitline');
var _interval=4000;
var _moving;
_wrap.hover(function(){clearInterval(_moving);},function(){
	_moving=setInterval(function(){
		var _field=_wrap.find('li:first');
		var _h=_field.innerHeight();
		_field.animate({marginTop:-_h+'px'},600,function(){
			_field.css('marginTop',0).appendTo(_wrap);
			})
		},_interval)
	}).trigger('mouseleave');
var _wrap02=$('ul.mulitline02');
var _interval02=2000;
var _moving02;
_wrap02.hover(function(){clearInterval(_moving02);},function(){
	_moving02=setInterval(function(){
		var _field02=_wrap02.find('li:first');
		var _h02=_field02.innerHeight();
		_field02.animate({marginTop:-_h02+'px'},600,function(){
			_field02.css('marginTop',0).appendTo(_wrap02);
			})
		},_interval02)
	}).trigger('mouseleave');
$('#jq_moreCity').hover(function(){$('#hideCity').slideDown(),$('#jq_moreCity span').addClass('hover');$("#tabContent2").hide();},
						function(){$('#hideCity').hide(),$('#jq_moreCity span').removeClass('hover');$("#tabContent2").show();}
						);
$('#jq_allschoolh').hover(function(){$('#jq_allschool').show();},function(){$('#jq_allschool').hide();});
//$('#jq_menuArea li').hover(function(){$('#jq_menuArea div.boxHide').hide();$(this).addClass('corrent').find('div:first').fadeIn();},function(){$(this).removeClass('corrent').find('div:first').hide();});
$("#jq_menuArea li").each(function(index){
$(this).hover(function(){
var tabin = $(this); 
timeoutid = setTimeout(function(){
tabin.addClass("corrent").siblings().removeClass("corrent");
$('#jq_menuArea div.boxHide').hide();
tabin.find('div:first').fadeIn();
},150);
},function(){var tabin = $(this); clearTimeout(timeoutid);$('#jq_menuArea div.boxHide').hide();tabin.removeClass("corrent");});
});
$('#lessonNav dt,#lessonNav dd').show();
$('#lessonNav dt.jq_dtview').hover(function(){$(this).addClass('corrent').find('div:first').fadeIn();},function(){$(this).removeClass('corrent').find('div:first').hide();});
$("#lessonNav").delegate(".jq_close","click",function(){$("#lessonNav .boxHide").hide();});
$('#jq_menuArea li#a0').hover(function(){$(this).find('div').load('/templets/default/2012moban/fenxiaoyincang.html');});
$('#jq_menuArea li#a1').hover(function(){$(this).find('div').load('/templets/default/2012moban/menuArea.html #b1');});
$('#jq_menuArea li#a2').hover(function(){$(this).find('div').load('/templets/default/2012moban/menuArea.html #b2');});
$('#jq_menuArea li#a3').hover(function(){$(this).find('div').load('/templets/default/2012moban/menuArea.html #b3');});
$('#jq_menuArea li#a4').hover(function(){$(this).find('div').load('/templets/default/2012moban/menuArea.html #b4');});
$('#jq_menuArea li#a5').hover(function(){$(this).find('div').load('/templets/default/2012moban/menuArea.html #b5');});
$('#jq_menuArea li#a6').hover(function(){$(this).find('div').load('/templets/default/2012moban/menuArea.html #b6');});
$('#jq_menuArea li#a7').hover(function(){$(this).find('div').load('/templets/default/2012moban/menuArea.html #b7');});
$('#jq_menuArea li#a8').hover(function(){$(this).find('div').load('/templets/default/2012moban/menuArea.html #b8');});
$('#jq_menuArea li#a9').hover(function(){$(this).find('div').load('/templets/default/2012moban/menuArea.html #b9');});
$('#lessonNav dt.dt02').hover(function(){$(this).find('div').load('/templets/default/2012moban/menuArea.html #b10');});
$('#lessonNav dt.dt03').hover(function(){$(this).find('div').load('/templets/default/2012moban/menuArea.html #b11');});
$('#subject dt').hover(function(){$(this).find('div').fadeIn();},
						function(){$(this).find('div').hide();});
$.fn.WIT_SetTab=function(iSet){
		iSet=$.extend({Nav:null,Field:null,K:0,CurCls:'cur',Auto:false,AutoTime:9000,OutTime:100,InTime:150,CrossTime:60},iSet||{});
		var acrossFun=null,hasCls=false,autoSlide=null;
		function changeFun(n){
			iSet.Field.filter(':visible').fadeOut(iSet.OutTime, function(){
				iSet.Field.eq(n).fadeIn(iSet.InTime).siblings().hide();
			});
			iSet.Nav.eq(n).addClass(iSet.CurCls).siblings().removeClass(iSet.CurCls);
		}
		changeFun(iSet.K);
		iSet.Nav.hover(function(){
			iSet.K=iSet.Nav.index(this);
			if(iSet.Auto){
				clearInterval(autoSlide);
			}
			hasCls = $(this).hasClass(iSet.CurCls);
			acrossFun=setTimeout(function(){
				if(!hasCls){
					changeFun(iSet.K);
				}
			},iSet.CrossTime);
		},function(){
			clearTimeout(acrossFun);
			if(iSet.Ajax){
				iSet.AjaxFun();
			}
			if(iSet.Auto){
				autoSlide = setInterval(function(){
		            iSet.K++;
		            changeFun(iSet.K);
		            if (iSet.K == iSet.Field.size()) {
		                changeFun(0);
						iSet.K=0;
		            }
		        }, iSet.AutoTime)
			}
		}).eq(0).trigger('mouseleave');
	}
$(document).WIT_SetTab({
		Nav:$('#jq_scrlFocusList>li'),
		Field:$('#jq_scrlFocusCon>li'),
		Auto:true,
		CurCls:'hover'
	});


$('#map section').hover(function(){$(this).addClass('index998').find('aside').fadeIn().addClass('index999');},function(){$(this).removeClass('index998').find('aside').fadeOut().removeClass('index999');});
//煦苺忑珜粟堤敦
var $bodyh=$(document).height();
var $coverdiv=$('<div id="coverdiv"></div>');
$("#coverdiv").css({'height':$bodyh+'px'});
$("#jq_closeparent").click(function(){$(this).parent().hide();$("#jq_btnshow").show();$("#coverdiv").hide();});
$("#jq_btnshow").click(function(){$("#fixgk2013").show();$("#coverdiv").show().css({'height':$bodyh+'px'});$("#jq_btnshow").hide();});
//20121029煦苺階窒嫘豢壽敕
//$("#httopad span").click(function(){$("#httopad").hide();$(".logBox").css({position:"fixed"});$(".proNav").css({"padding-top":"31px"});$("#gg_full").css({top:"31px"});});
$("#httopad span").click(function(){$("#httopad").hide();$(".logBox").attr("style","position:fixed;_position:ablolute");$(".proNav").attr("style","padding-top:31px;_padding-top:0");$("#gg_full").css({top:"31px"});});

$(".jq_morecity02").hover(function(){$(this).find("span").show();},function(){$(this).find("span").hide();});

	
}); 
function MM_jumpMenu(selObj,restore){ 
window.open(selObj.options[selObj.selectedIndex].value);
}
//function djcishu(id,mid){
// alert(mid);
////plg氝樓衾20120801
//            var sjs=Math.random();
// 			$.ajax({
//				  type: "GET",
//				  //dataType:'json',
//				  //async:false,
//				  url: "http://www.htexam.com/plus/count1.php",
//				  data: "view=yes&aid="+id+"&mid="+mid+"&sjs="+sjs,
//				  success: function(data) {
//				    alert(data);
//					$("#cishu").html(data);
//				  }
//				}); 
////plgend
//}
//setTimeout("showNotis()",10000);function showNotis(){document.getElementById('notis').style.display="none";}
//function SetHome(url){
//    if (document.all) {
//        document.body.style.behavior='url(#default#homepage)';
//        document.body.setHomePage(url);
//    }else{
//            alert("Sorry ,Da ye!");
//    }
//}


// ?置?主? 
function SetHome(obj, vrl) {
    try {
        obj.style.behavior = 'url(#default#homepage)'; obj.setHomePage(vrl);
    }
    catch (e) {
        if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            }
            catch (e) {
                alert("抱歉，此操作被瀏覽器拒絕!\n\n請在瀏覽器地址欄輸入“about:config”并回車然后將[signed.applets.codebase_principal_support]設置為'true'");
            }
            var prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch);
            prefs.setCharPref('browser.startup.homepage', vrl);
        } else {
            alert("抱歉，您所使用的瀏覽器無法完成此操作,\n\n您需手動將【" + url + "】設置為首頁。");
        }
    }
}
// 加入收藏 兼容360和IE6 
function shoucang(sTitle, sURL) {
    try {
        window.external.addFavorite(sURL, sTitle);
    }
    catch (e) {
        try {
            window.sidebar.addPanel(sTitle, sURL, "");
        }
        catch (e) {
            alert("抱歉，您所使用的瀏覽器無法完成此操作.\n\n加入收藏失敗,請使用Ctrl+D進行添加!");
        }
    }
}
// 代码整理：懒人之家

//获取ID
var $ = function (id) {return typeof id === "string" ? document.getElementById(id) : id};
//获取tagName
var $$ = function (tagName, oParent) {return (oParent || document).getElementsByTagName(tagName)};
//自动播放对象
var AutoPlay = function (id) {this.initialize(id)};
AutoPlay.prototype = {
	initialize: function (id)
	{
		var oThis = this;
		this.oBox = $(id);
		this.oUl = $$("ul", this.oBox)[0];
		this.aImg = $$("img", this.oBox);
		this.timer = null;
		this.autoTimer = null;
		this.iNow = 0;
		this.creatBtn();
		this.aBtn = $$("li", this.oCount);
		this.toggle();
		this.autoTimer = setInterval(function ()
		{
			oThis.next()
		}, 3000);
		this.oBox.onmouseover = function ()
		{
			clearInterval(oThis.autoTimer)
		};
		this.oBox.onmouseout = function ()
		{
			oThis.autoTimer = setInterval(function ()
			{
				oThis.next()
			}, 3000)
		};
		for (var i = 0; i < this.aBtn.length; i++)
		{
			this.aBtn[i].index = i;
			this.aBtn[i].onmouseover = function ()
			{
				oThis.iNow = this.index;
				oThis.toggle()
			}
		}
	},
	creatBtn: function ()
	{
		this.oCount = document.createElement("ul");
		this.oFrag = document.createDocumentFragment();
		this.oCount.className = "count";
		for (var i = 0; i < this.aImg.length; i++)
		{
			var oLi = document.createElement("li");
			oLi.innerHTML = i + 1;
			this.oFrag.appendChild(oLi)
		}
		this.oCount.appendChild(this.oFrag);
		this.oBox.appendChild(this.oCount)
	},
	toggle: function ()
	{
		for (var i = 0; i < this.aBtn.length; i++) this.aBtn[i].className = "";
		this.aBtn[this.iNow].className = "current";
		this.doMove(-(this.iNow * this.aImg[0].offsetHeight))
	},
	next: function ()
	{
		this.iNow++;
		this.iNow == this.aBtn.length && (this.iNow = 0);
		this.toggle()
	},
	doMove: function (iTarget)
	{
		var oThis = this;
		clearInterval(oThis.timer);
		oThis.timer = setInterval(function ()
		{
			var iSpeed = (iTarget - oThis.oUl.offsetTop) / 5;
			iSpeed = iSpeed > 0 ? Math.ceil(iSpeed) : Math.floor(iSpeed);
			oThis.oUl.offsetTop == iTarget ? clearInterval(oThis.timer) : (oThis.oUl.style.top = oThis.oUl.offsetTop + iSpeed + "px")
		}, 30)
	}
};
//window.onload = function ()
//{
//	new AutoPlay("box_lanrenzhijia");
//};
function DrawImage(ImgD) {
    var image = new Image();
    image.src = ImgD.src;
    if (image.width > 0 && image.height > 0) {
        flag = true;
        if (image.width / image.height >= 105 / 80) {
            if (image.width > 105) {
                ImgD.width = 105;
                ImgD.height = (image.height * 105) / image.width;
            } else {
                ImgD.width = image.width;
                ImgD.height = image.height;
            }
            ImgD.alt = "点击查看详细信息...";
        }
        else {
            if (image.height > 80) {
                ImgD.height = 80;
                ImgD.width = (image.width * 80) / image.height;
            } else {
                ImgD.width = image.width;
                ImgD.height = image.height;
            }
            ImgD.alt = "详细信息...";
        }
    }
}
function MyMove(div1, div2, div3, speed) {
    var tab = document.getElementById(div1); //获取id为demo的div
    var tab1 = document.getElementById(div2); //获取id为demo1的div
    var tab2 = document.getElementById(div3); //获取id为demo1的div
    tab2.innerHTML = tab1.innerHTML;
    var MyMar = setInterval(Marquee, speed);
    function Marquee() {

        if (tab2.offsetWidth - tab.scrollLeft <= 0) {
            tab.scrollLeft -= tab1.offsetWidth;
        } else {
            tab.scrollLeft++;
        }
    }
    tab.onmouseover = function() { clearInterval(MyMar) };
    tab.onmouseout = function() { MyMar = setInterval(Marquee, speed) };
}
var num = 0;
var arr = new Array();
arr[1] = "../../webroot/images/banjiang1.jpg"; //放图片地址
arr[2] = "../../webroot/images/banjiang2.jpg";
arr[3] = "../../webroot/images/banjiang3.jpg";

function AutoPalyImage() {
    setInterval(turnpic, 4000); //每隔4秒转换图片
}

function turnpic() {
    idsrc = document.getElementById("Image2");
    if (num == arr.length - 1)
        num = 0;
    else
        num += 1;
    idsrc.src = arr[num];
}
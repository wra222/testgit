
var DragResize = function (target, options) {this.init.apply(this, arguments);};
DragResize.prototype = {
	// 初始化
    init : function(target, options) {
		this.target = target;
		// 最近一个定位的父对象(target.offsetParent)元素在当前视口的相对偏移
		this.parentOffset = $(this.target.offsetParent).offset();
		this.parentOffset.left+= parseInt($(this.target.offsetParent).css('border-left-width'))||0;
		this.parentOffset.top += parseInt($(this.target.offsetParent).css('border-top-width')) ||0;
		this.setOptions(options);
		this.handlers = new Object;
		this.handlers['selecting']= this.eventMethod(this, 'onSelecting');
		this.handlers['dragging'] = this.eventMethod(this, 'onDragging');
		this.handlers['resizing'] = this.eventMethod(this, 'onResizing');
		this.handlers['complete'] = this.eventMethod(this, 'stop');
	},
	setOptions: function(options) {
		this.option = {
			minWidth : 0,	// 限制最小宽度
			minHeight: 0,	// 限制最小高度
			bound : {// 默认为限制在target.offsetParent内
				left  :this.parentOffset.left, 
				top	  :this.parentOffset.top, 
				right :this.parentOffset.left+ $(this.target.offsetParent).width(), 
				bottom:this.parentOffset.top + $(this.target.offsetParent).height()
			},   
			callback : null	// 动作进行时调用的回调函数
		};
		// 设置选项
		$.extend(this.option, options);
	},
	// 开始
	start:function(e, mode) {
		this.mode = mode;
		this.width  = $(this.target).width();
		this.height = $(this.target).height();
		// 元素四边相对于当前视口的偏移
		this.border = $(this.target).offset();
		this.border.right  = this.border.left + this.width;
		this.border.bottom = this.border.top  + this.height;
		$(document).mousemove(this.handlers[this.mode]).mouseup(this.handlers['complete']);
		// 阻止事件传播
		e.stopPropagation();
		e.preventDefault();
	},
	// 停止
	stop: function(e) {
		$(document).unbind('mousemove', this.handlers[this.mode]);
		$(document).unbind('mouseup'  , this.handlers['complete']);
		return false;
	},
	resize:function(e, direction) {
		// 设置方向
		if(!e || !this.setDirection(direction)) return false;
		this.start(e, 'resizing');
	},
	drag:function(e) {
		this.start(e, 'dragging');
		this.oPos = {x :e.pageX||0, y :e.pageY||0};// 鼠标位置
	},
	select: function(e, x, y) {
		$(this.target).height(0).width(0).show();
		this.start(e, 'selecting');
		this.oPos = {x:this.boundx(x), y:this.boundy(y)};
	},
	// 设置方向, 以向量this.vector保存方向.共八个方向
	setDirection : function(direction) {
		switch(direction) {
		case 'west'	:this.vector = {x:-1,y: 0};break;
		case 'east'	:this.vector = {x: 1,y: 0};break;
		case 'north':this.vector = {x: 0,y:-1};break;
		case 'south':this.vector = {x: 0,y: 1};break;
		case 'north-west':this.vector = {x:-1,y:-1};break; 
		case 'south-west':this.vector = {x:-1,y: 1};break; 
		case 'north-east':this.vector = {x: 1,y:-1};break; 
		case 'south-east':this.vector = {x: 1,y: 1};break;
		default:return false;
		}
		return true;
	},
	onSelecting: function(e){
		this.border = {'left':this.oPos.x,'top':this.oPos.y, 'right':this.boundx(e.pageX), 'bottom':this.boundy(e.pageY)};
		if(this.border.right <= this.border.left) {this.border.left= this.border.right ;this.border.right = this.oPos.x;}
		if(this.border.bottom <= this.border.top) {this.border.top = this.border.bottom;this.border.bottom= this.oPos.y;}
		this.adjust(this.border.left - this.parentOffset.left, this.border.top - this.parentOffset.top, 
					this.border.right - this.border.left	 , this.border.bottom- this.border.top);
		return false;
	},
	onResizing: function(e){
		// 修正X,Y
		var i = {x : this.boundx(e.pageX), y : this.boundy(e.pageY)};
		if(this.vector.x === -1) this.border.left	= Math.min(i.x, this.border.right - this.option.minWidth);
		if(this.vector.x ===  1) this.border.right	= Math.max(i.x, this.border.left  + this.option.minWidth);
		if(this.vector.y === -1) this.border.top	= Math.min(i.y, this.border.bottom- this.option.minHeight);
		if(this.vector.y ===  1) this.border.bottom = Math.max(i.y, this.border.top	  + this.option.minHeight);
		this.adjust(this.border.left - this.parentOffset.left, this.border.top - this.parentOffset.top, 
					this.border.right - this.border.left	 , this.border.bottom- this.border.top);
		return false;
	},	
	onDragging: function(e) {
		// 调整元素相对于当前视口的偏移
		this.border.left= this.boundx(this.border.left + e.pageX - this.oPos.x, -this.width);
		this.border.top = this.boundy(this.border.top  + e.pageY - this.oPos.y, -this.height);
		this.oPos = {x : e.pageX, y : e.pageY};
		this.adjust(this.border.left - this.parentOffset.left, this.border.top - this.parentOffset.top);
		return false;
	},
	// left,top以parentOffset为原点
	adjust: function(left, top, width, height) {
		this.width = width || this.width || 0;
		this.height = height || this.height || 0;
		var info = {'left':left, 'top':top, 'width':this.width, 'height':this.height};
		$(this.target).css(info);
		if($.isFunction(this.option.callback)) this.option.callback(info, this.parentOffset);
	},
	boundx: function(x, extra) {return Math.max(Math.min(x||0, this.option.bound.right + (extra||0)), this.option.bound.left);},
	boundy: function(y, extra) {return Math.max(Math.min(y||0, this.option.bound.bottom+ (extra||0)), this.option.bound.top);},
	eventMethod: function(instance, method) {return function(event) { return instance[method](event, this); };}
};
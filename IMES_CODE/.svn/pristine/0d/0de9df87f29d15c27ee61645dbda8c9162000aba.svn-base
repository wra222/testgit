function MzTreeView(Tname)
{
 
  if(typeof(Tname) != "string" || Tname == "")
    throw(new Error());
  
  this.url      = "#";
  this.target   = "_self";
  this.name     = Tname;
  this.wordLine = false;
  this.currentNode = null;
  this.useArrow = true;
  this.nodes = {};
  this.node  = new Array();
  this.names = "";
  this._d    = "\x0f";
  this.index = 0;
  this.divider   = "_";
  this.rootId = "-1";

  this.attribute_d = "\x0f";//nodes text divider	
  this.attribute_suffix = String.fromCharCode(31);//nodes text divider	
  this.total = 1; 
 //for tabledata................
  this.LoadData = null;
  this.expandingNode = null;
 //add by lzy
 this.clickCallOuterCheck = null;	
 this.customIconFun = "";
 this.freshPath = new Array();
 this.locatingNode = false;//set true if locating node.
 
 this.defaultExpandRoot = true;//not expand the rood not after created the tree
 this.focusOnInit = false;//if focusing on the root node right away after created.
 
  this.colors   =
  {
    "highLight" : "#0A246A",
    "highLightText" : "#FFFFFF",
    "mouseOverBgColor" : "#D4D0C8"
  };
  this.icons    = {
    L0        : '../../images/L0.gif',  
    L1        : '../../images/L1.gif',  
    L2        : '../../images/L2.gif', 
    L3        : '../../images/L3.gif',  
    L4        : '../../images/L4.gif',  
    PM0       : '../../images/P0.gif',  
    PM1       : '../../images/P1.gif', 
    PM2       : '../../images/P2.gif', 
    PM3       : '../../images/P3.gif',
    empty     : '../../images/L5.gif',
    root      : '../../images/root.gif',   
    folder    : '../../images/folder.gif', 
    file      : '../../images/file.gif', 
    exit      : '../../images/exit.gif'
  };
  this.iconsExpand = {  
    PM0       : '../../images/M0.gif',
    PM1       : '../../images/M1.gif',
    PM2       : '../../images/M2.gif',
    PM3       : '../../images/M3.gif',
    folder    : '../../images/folderopen.gif',

    exit      : '../../images/exit.gif'
  };
  this.getElementById = function(id)
  {
	 // alert(id);
    if (typeof(id) != "string" || id == "") 
	  {
		return null;
	  }
    if (document.getElementById) 
	  {
		return document.getElementById(id);
	  }
    if (document.all) 
	  {
		return document.all(id);
	  }
    try 
	{
		return eval(id);
	} 
	catch(e)
	{ 
		return null;
	}
  }


  this.toString = function()
  {
	
    this.browserCheck();
    this.dataFormat();
    this.setStyle();
    this.node["0"] =
    {
      "id": "0",
	  "sid":"",              //for tabledata...............
	  "uuid":"0",            //.
	  "type":"",             //.
	  "haschild":true,   
	  "text":"", 
	  "nodeuuid":"", 
	  "status":"0",
	  "fieldName":"",      
	  "fieldType":"",      //for tabledata...............
      "path": this.rootId,
      "isLoad": false,
      "childNodes": [],
      "childAppend": "",
      "sourceIndex": this.rootId
    };  
    this.load("0",false);
	var rootCN = this.node["0"].childNodes;
    var str = "<A id='"+ this.name +"_RootLink' href='#' style='DISPLAY: none'></A>";
    if(rootCN.length>0)
    {
      this.node["0"].hasChild = true;
      for(var i=0; i<rootCN.length; i++)
        str += this.nodeToHTML(rootCN[i], i==rootCN.length-1);
	 
      //setTimeout(this.name +".expand('"+ rootCN[0].id +"', true); "+ 
      //  this.name +".focusClientNode('"+ rootCN[0].id +"'); "+ this.name +".atRootIsEmpty()",10);
	      setTimeout(this.name +".atRootIsEmpty();"+ this.name +".focusClientNode('"+ rootCN[0].id +"'); ",10);
	  
    }
 
    if (this.useArrow) 
    {
      if (document.attachEvent)
          document.attachEvent("onkeydown", this.onkeydown);
      else if (document.addEventListener)
          document.addEventListener('keydown', this.onkeydown, false);
    }
    
    //add by lzy
   /* with (window.document) {
	  	oncontextmenu=onselectstart=function(){
	  		event.returnValue=true;event.cancelBubble=false;
	 	}	
  	}
    */
    
   return "<DIV class='MzTreeView' "+"onclick='"+ this.name +".clickHandle(event)' "+"ondblclick='"+ this.name 
	        +".dblClickHandle(event)' "+ ">"+ str +"</DIV>";
  };

  this.onkeydown= function(e)
  {
    e = window.event || e; var key = e.keyCode || e.which;
    switch(key)
    {
      case 37 : eval(Tname).upperNode(); break;  //Arrow left, shrink child node
      case 38 : eval(Tname).pervNode();  break;  //Arrow up
      case 39 : eval(Tname).lowerNode(); break;  //Arrow right, expand child node
      case 40 : eval(Tname).nextNode();  break;  //Arrow down
    }
  };

 
  this.setTotal = function(total)
  {
	  this.total = total;
	  
  };

}


MzTreeView.prototype.browserCheck = function()
{
	//alert("browserCheck");
  var ua = window.navigator.userAgent.toLowerCase(), bname;
  if(/msie/i.test(ua))
  {
    this.navigator = /opera/i.test(ua) ? "opera" : "";
    if(!this.navigator) 
	  {
		this.navigator = "msie";
	  }
  }
  else if(/gecko/i.test(ua))
  {
    var vendor = window.navigator.vendor.toLowerCase();
    if(vendor == "firefox") 
	  {
		this.navigator = "firefox";
	  }
    else if(vendor == "netscape") 
	  {
		this.navigator = "netscape";
	  }
    else if(vendor == "") 
	  {
		this.navigator = "mozilla";
	  }
  }
  else 
	  this.navigator = "msie";
  if(window.opera) 
	  this.wordLine = false;
};

MzTreeView.prototype.setStyle = function()
{
	//alert("setStyle");
  /*
    width: 16px; \
    height: 16px; \
    width: 20px; \
    height: 20px; \
  */
  var style = "<style>"+
  "DIV.MzTreeView DIV IMG{border: 0px solid #FFFFFF;}"+
  "DIV.MzTreeView DIV SPAN IMG{border: 0px solid #FFFFFF;}";
//  "A.MzTreeView{font:italic normal bolder 5pt Arial;}";
  if(this.wordLine)
  {
    style +="\
    DIV.MzTreeView DIV\
    {\
      height: 20px;"+
      (this.navigator=="firefox" ? "line-height: 20px;" : "" ) +
      (this.navigator=="netscape" ? "" : "overflow: hidden;" ) +"\
    }\
    DIV.MzTreeView DIV SPAN\
    {\
      vertical-align: middle; font-size: 21px; height: 20px; color: #D4D0C8; cursor: default;\
    }\
    DIV.MzTreeView DIV SPAN.pm\
    {\
      width: "+ (this.navigator=="msie"||this.navigator=="opera" ? "11" : "9") +"px;\
      height: "+ (this.navigator=="netscape"?"9":(this.navigator=="firefox"?"10":"11")) +"px;\
      font-size: 7pt;\
      overflow: hidden;\
      margin-left: -16px;\
      margin-right: 5px;\
      color: #000080; \
      vertical-align: middle;\
      border: 1px solid #D4D0C8;\
      cursor: "+ (this.navigator=="msie" ? "hand" : "pointer") +";\
      padding: 0 2px 0 2px;\
      text-align: center;\
      background-color: #F0F0F0;\
    }";
  }
  style += "<\/style>";
  /*alert(document.getElementsByTagName("HEAD")[0].innerHTML);
  if(document.body)
  {
    var head = document.getElementsByTagName("HEAD")[0];
    head.innerHTML = head.innerHTML + style;
  }
  else */ 
  document.write(style);
};


MzTreeView.prototype.atRootIsEmpty = function()
{
//begin add
	/*for show nothing at root node 
		add one line by lzy
		date:2008-4-22		
	*/
	if (this.defaultExpandRoot == false)
		return false;
		
//end add
	

  var RCN = this.node["0"].childNodes;
 
  for(var i=0; i<RCN.length; i++)
  {
    if(!RCN[i].isLoad) this.expand(RCN[i].id);
	//alert(RCN[i].id);
    if (RCN[i].text=="")
    {
      var node = RCN[i].childNodes[0], HCN  = node.hasChild;
      if(this.wordLine)
      {
        var span = this.getElementById(this.name +"_tree_"+ node.id);
        span = span.childNodes[0].childNodes[0].childNodes[0];
        span.innerHTML = RCN[i].childNodes.length>1 ? "┌" : "─";
      }
      else
      {
        node.iconExpand  =  RCN[i].childNodes.length>1 ? HCN ? "PM0" : "L0" : HCN ? "PM3" : "L3"
        this.getElementById(this.name +"_expand_"+ node.id).src = this.icons[node.iconExpand].src;
      }
    }
  }
};

MzTreeView.prototype.dataFormat = function()
{
  //alert("dataFormat");
  var a = new Array();
  for (var id in this.nodes)
	  a[a.length] = id;
  this.names = a.join(this._d + this._d);
  this.totalNode = a.length;
  //this.total = a.length;
  this.total += a.length;//modi by lzy,the number will double if the delete operation occur.
  a = null;
};

MzTreeView.prototype.load = function(id,flag)
{	

  var node = this.node[id], d = this.divider, _d = this._d;
  var sid = node.sourceIndex.substr(node.sourceIndex.indexOf(d) + d.length);
  var reg = new RegExp("(^|"+_d+")"+ sid +d+"[^"+_d+d +"]+("+_d+"|$)", "g");
  var cns = this.names.match(reg), tcn = this.node[id].childNodes; 
   
  if (cns)
  {
  	  reg = new RegExp(_d, "g"); 
      for (var i=0; i<cns.length; i++)
	  {
			tcn[tcn.length] = this.nodeInit(cns[i].replace(reg, ""), id,flag); 
	  }
   }
	  if(flag)
	 {
		  alert(id + "  " +  this.node[id].childNodes);
	 }
  node.isLoad = true;  
};

MzTreeView.prototype.nodeInit = function(sourceIndex, parentId,flag)
{
	if(!flag)
	{
		this.index++;
	}
  
	var source= this.nodes[sourceIndex], d = this.divider;
	var text  = this.getAttribute(source, "text");
	var hint  = this.getAttribute(source, "hint");
	var hasChild = this.getAttribute(source,"hasChild");//for tabledata..............
	var type = this.getAttribute(source,"type");//.
	var uuid = this.getAttribute(source,"uuid");//for tabledata..............
	var fieldName =  this.getAttribute(source,"fieldName");//for tabledata..............
	var nodeuuid = this.getAttribute(source,"nodeuuid");//for tabledata..............
	var fieldType = this.getAttribute(source,"fieldType");
	var sid   = sourceIndex.substr(sourceIndex.indexOf(d) + d.length);
	this.node[this.index] =
	{
	"id"    : this.index,
	"text"  : text,
	"hint"  : hint ? hint : text.replace(/\'/g, " "),
	//"hint"  : hint ? hint : sourceIndex,
	"icon"  : this.getAttribute(source, "icon"),
	"path"  : this.node[parentId].path + d + this.index,
	"isLoad": false,
	"isExpand": false,
	"parentId": parentId,
	"parentNode": this.node[parentId],
	"sourceIndex" : sourceIndex,
	"childAppend" : "",                     //for tabledata........
	"hasChild": hasChild == "true"? true : false,
	"type"    : type,                       //..
	"sid"     : sid,                        //..
	"uuid"    : uuid,
	"nodeuuid": nodeuuid,    
	"fieldName":fieldName,                     
	"fieldType":fieldType   //for tabledata........
	};

     this.nodes[sourceIndex] = "index:"+ this.index +this.attribute_d + source;
  if(this.node[this.index].hasChild)  
	  
     this.node[this.index].childNodes = [];

  return this.node[this.index];
};


MzTreeView.prototype.getAttribute = function(source, name)
{
	  //alert("getAttribute");
  var reg = new RegExp("(^|" +this.attribute_d + "|\\s)"+ name +"\\s*" + this.attribute_suffix + "\\s*([^"+this.attribute_d +"]*)(\\s|"+this.attribute_d +"|$)", "i"); 
  if (reg.test(source)) {
//    return RegExp.$2.replace(/[\x0f]/g, this.attribute_d ).replace(/\'/g, "&#39"+this.attribute_d +""); return "";
    return RegExp.$2.replace(/[\x0f]/g, this.attribute_d ); //changed by lzy,date:2006-1-12
  }  
};


MzTreeView.prototype.nodeToHTML = function(node, AtEnd)
{
  var source = this.nodes[node.sourceIndex];
  var target = this.getAttribute(source, "target");
  var data = this.getAttribute(source, "data");
  var url  = this.getAttribute(source, "url");
  if(!url) 
	  url = this.url;
  if(data) 
	  url += (url.indexOf("?")==-1?"?":"&") + data;
  if(!target) 
	  target = this.target;

  var id   = node.id;
  var HCN  = node.hasChild, isRoot = node.parentId=="0";
  if(isRoot && node.icon=="")
	  node.icon = "root";
  if(node.icon=="" || typeof(this.icons[node.icon])=="undefined")
     node.icon = HCN ? "folder" : "file";
       
     node.iconExpand  = AtEnd ? "└" : "├";

  var HTML = "<DIV noWrap='True'><NOBR>";
  if(!isRoot)
  {
    node.childAppend = node.parentNode.childAppend + (AtEnd ? "　" : "│");
	
    if(this.wordLine)
    {
      HTML += "<SPAN>"+ node.parentNode.childAppend + (AtEnd ? "└" : "├") +"</SPAN>";
      if(HCN) HTML += "<SPAN class='pm' id='"+ this.name +"_expand_"+ id +"'>+</SPAN>";
    }
    else
    {
      node.iconExpand  = HCN ? AtEnd ? "PM2" : "PM1" : AtEnd ? "L2" : "L1";
	 
      HTML += "<SPAN>"+ this.word2image(node.parentNode.childAppend) +"<IMG "+
        "align='absmiddle' id='"+ this.name +"_expand_"+ id +"' "+
        "src='"+ this.icons[node.iconExpand].src +"' style='cursor: "+ (!node.hasChild ? "":
        (this.navigator=="msie"||this.navigator=="opera"? "hand" : "pointer")) +"'></SPAN>";
    }
  }
  HTML += "<IMG "+
    "align='absMiddle'"+
    "id='"+ this.name +"_icon_"+ id +"' "+
   // "src='"+ this.icons[node.icon].src +"'><A "+
    "src='"+ this.getIconPath(node) +"'><A "+
    "style='font:normal 9pt 新宋体;' hideFocus;  "+
    "id='"+ this.name +"_link_"+ id +"' "+
    "href='"+ url +"' "+
    "target='"+ target +"' "+
    "title='"+ node.hint + "' "+
    "onfocus=\""+ this.name +".focusLink('"+ id +"')\" "+
    ">"+ node.text +
   // "onclick=\"return "+ this.name +".nodeClickMain('"+ id +"')\" >"+ node.text +
 //   "onfocus=\"if (event.button == 1)"+ this.name +".focusLink('"+ id +"')\" "+
 //   "onclick=\"if (event.button == 1)return "+ this.name +".nodeClickMain('"+ id +"')\" >"+ node.text +
  "</A></NOBR></DIV>";
     
  if(isRoot && node.text=="") HTML = "";

  HTML = "\r\n<SPAN id='"+ this.name +"_tree_"+ id +"'>"+ HTML 
  HTML +="<SPAN style='DISPLAY: none'></SPAN></SPAN>";
  
  return HTML;
};


MzTreeView.prototype.word2image = function(word)
{
	 //alert("word2image");
  var str = "";
  for(var i=0; i<word.length; i++)
  {
    var img = "";
    switch (word.charAt(i))
    {
      case "│" : img = "L4"; break;
      case "└" : img = "L2"; break;
      case "　" : img = "empty"; break;
      case "├" : img = "L1"; break;
      case "─" : img = "L3"; break;
      case "┌" : img = "L0"; break;
    }
    if(img!="")
      str += "<IMG align='absMiddle' src='"+ this.icons[img].src +"' height='20'>";
  }
  return str;
}

MzTreeView.prototype.buildNode = function(id)
{
  if(this.node[id].hasChild)
  {
    var tcn = this.node[id].childNodes, str = "";
	
    for (var i=0; i<tcn.length; i++)
      str += this.nodeToHTML(tcn[i], i==tcn.length-1);
	
    var temp = this.getElementById(this.name +"_tree_"+ id).childNodes;	
    temp[temp.length-1].innerHTML = str;
//	 alert(temp[temp.length-1].innerHTML);
//	alert(this.getElementById(this.name +"_tree_"+ id).innerHTML)
  }
  
  //add by lzy
  //HideWait();
  
  
};


MzTreeView.prototype.focusClientNode      = function(id, onlyLocate)
{ 
  	if(!this.currentNode)
	{
	  this.currentNode=this.node["0"];
    }
 //add by lzy 
// 	if (this.currentNode.id == id)
// 		return false;//do nothing if repeating to click the same node.
 	
 	if (this.disabled && !this.locatingNode)
 	{ 
		return false;
 	}
 //end add   
    
  var a = this.getElementById(this.name +"_link_"+ id);
  if(a)
	  {
          try{	    
		    a.focus();//ignore if the tree is invisible.
		  } catch(e){
		  
		  }
		  
		  var link = this.getElementById(this.name +"_link_"+ this.currentNode.id);
		  if(link)with(link.style)
		  {
			  color="";   
			  backgroundColor="";
		  }
	      with(a.style)
		  {
			  color = this.colors.highLightText;
			  backgroundColor = this.colors.highLight;
		  }
		  this.currentNode= this.node[id];
		   
		  if (this.focusOnInit)
		  {
		  	  this.disabled = true;		  	
		  	  
		  	  if (onlyLocate != true)	
	    	    this.nodeClick(id); 	//add by lzy
	    	    
	    	  window.setTimeout(this.name + ".disabled = false;", 700);
		  }else{
		    this.focusOnInit = true;
		  }
	    	  	 
	 //window.parent.frames("IFrame2").location = "NodeType.do?type=" + type; 
	
//        window.parent.frames("IFrame2").location ="/eSOP/jsp/eSOPedit/info.htm";
       
	   }
};

MzTreeView.prototype.focusLink= function(id)
{
	// alert("focusLink");
  if(this.currentNode && this.currentNode.id==id) 
	  return;
  this.focusClientNode(id);
};


MzTreeView.prototype.expand   = function(id, sureExpand)
{
  var node  = this.node[id];
  	
  if (sureExpand && node.isExpand) 
	  return;
  if (!node.hasChild) 
	  return;
  
  var area  = this.getElementById(this.name +"_tree_"+ id);
  if (area)   area = area.childNodes[area.childNodes.length-1];
  if (area)
  {
   
  	var Bool  = node.isExpand = sureExpand || area.style.display == "none";
 	this.switchIcon(id); 	
 	
    if(!Bool && this.currentNode.path.indexOf(node.path)==0 && this.currentNode.id!=id)
    {
      try{
		  //alert(this.getElementById(this.name +"_link_"+ id).click());
		  this.getElementById(this.name +"_link_"+ id).click();
		  }
      catch(e){this.focusClientNode(id);}
    }
    area.style.display = !Bool ? "none" : "block";//(this.navigator=="netscape" ? "block" : "");
	 //new add for get index value 
	if(!node.isLoad)
    {
/*	  this.load(id,false);
	  

	  if (this.init)
      {
		  this.load(id,false);
		  this.init = false; 
		  this.index = this.total; 
      } else {
		  this.load(id,true);
	  }


      if(node.id=="0") return;

    
	  if(node.hasChild && node.childNodes.length>200)
      {
		setTimeout(this.name +".buildNode('"+ id +"')", 1);
		var temp = this.getElementById(this.name +"_tree_"+ id).childNodes;
		temp[temp.length-1].innerHTML = "<DIV noWrap><NOBR><SPAN>"+ (this.wordLine ?
		node.childAppend +"└" : this.word2image(node.childAppend +"└")) +"</SPAN>"+
		"<IMG border='0' height='16' align='absmiddle' src='"+this.icons["file"].src+"'>"+
		"<A style='background-Color: "+ this.colors.highLight +"; color: "+
		this.colors.highLightText +"; font-size: 9pt'>请稍候...</A></NOBR></DIV>";
	  }
      else this.buildNode(id);

		//for tabledata........
//		if (id==1)
//		{
//			setTimeout(this.name + ".bulidNode('"+ id +"')",1);
//		}
//		else
//		{
*/
		var temp = this.getElementById(this.name +"_tree_"+ id).childNodes;
		temp[temp.length-1].innerHTML = "<DIV noWrap><NOBR><SPAN>"+ (this.wordLine ?
		node.childAppend +"└" : this.word2image(node.childAppend +"└")) +"</SPAN>"+
		"<IMG border='0' height='16' align='absmiddle' src='"+this.icons["file"].src+"'>"+
		"<A id='waittip' style='background-Color: "+ this.colors.highLight +"; color: "+
		this.colors.highLightText +"; font-size: 9pt'>Please wait...</A></NOBR></DIV>";
	  if(this.LoadData)
	  {
	  	  //add by lzy
		  //disableOperate();//show wait
		  try{
		  	//ShowWait();
		  }catch(e)
		  {}
		  
		  this.expandingNode = node;
		  eval(this.LoadData);
	  }
	}
  }
};

MzTreeView.prototype.nodeClickMain = function(id)
{

//add by lzy
 	if (this.currentNode.id == id || this.locatingNode == true)
 		return false;//do nothing
 
		  
 //end add   
	this.freshPath = new Array();

	if(this.clickCallOuterCheck != null)
	{		
		if (eval(this.clickCallOuterCheck) != true)
			return false;//refuse to lose the focus,do nothing		
	}
	 
	this.nodeClick();	 
	
/*		
  var source = this.nodes[this.node[id].sourceIndex];
  eval(this.getAttribute(source, "method"));
  return !(!this.getAttribute(source, "url") && this.url=="#");
  
 */
};

MzTreeView.prototype.getPath= function(sourceId)
{
	//alert("getPath");
  Array.prototype.indexOf = function(item)
  {
    for(var i=0; i<this.length; i++)
    {
      if(this[i]==item) return i;
    }
    return -1;
  };
  var _d = this._d, d = this.divider;
  var A = new Array(), id=sourceId; A[0] = id;
  while(id!="0" && id!="")
  {
    var str = "(^|"+_d+")([^"+_d+d+"]+"+d+ id +")("+_d+"|$)";
	if (new RegExp(str).test(this.names))
    {
      id = RegExp.$2.substring(0, RegExp.$2.indexOf(d));
      if(A.indexOf(id)>-1) break;
      A[A.length] = id;
    }
    else break;
  }
  return A.reverse();
};


MzTreeView.prototype.focus = function(sourceId, defer)
{
    // alert("focus");
	//var type = tree.currentNode.type;
	//window.parent.frames("IFrame2").location = "NodeType.do?type=" + type; 
	
    //window.parent.frames("IFrame2").location ="/eSOP/jsp/eSOPedit/info.htm";
  if (!defer)
  {	  
    setTimeout(this.name +".focus('"+ sourceId +"', true)", 100);
	
    return;
  }
  var path = this.getPath(sourceId);

  if(path[0]!=this.rootId)
  {
    alert(""+ sourceId +"\r\n"+
      " = "+ path.join(this.divider));
    return;
  }
  var root = this.node["0"], len = path.length;
  for(var i=1; i<len; i++)
  {
    if(root.hasChild)
    {
      var sourceIndex= path[i-1] + this.divider + path[i];
      for (var k=0; k<root.childNodes.length; k++)
      {
        if (root.childNodes[k].sourceIndex == sourceIndex)
        {
          root = root.childNodes[k];
          if(i<len - 1) 
			{
				this.expand(root.id, true);
			}
          else 
			    this.focusClientNode(root.id);
          break;
        }
      }
    }
  }
};


MzTreeView.prototype.clickHandle = function(e)
{
  e = window.event || e; e = e.srcElement || e.target;
 
  switch(e.tagName)
  {
    case "IMG" :
      if(e.id)
      {
 
        if(e.id.indexOf(this.name +"_icon_")==0)
		  {
			this.focusClientNode(e.id.substr(e.id.lastIndexOf("_") + 1));
		  }
        else if (e.id.indexOf(this.name +"_expand_")==0)
		  { 
			this.expand(e.id.substr(e.id.lastIndexOf("_") + 1));
		  }
      }
      break;
    case "A" :
      if(e.id) 
		this.focusClientNode(e.id.substr(e.id.lastIndexOf("_") + 1));
      break;
    case "SPAN" :
      if(e.className=="pm")
	  {
        this.expand(e.id.substr(e.id.lastIndexOf("_") + 1));
		//alert(e.tagName)
	  }
      break;
    default :
      if(this.navigator=="netscape") 
		e = e.parentNode;
      if(e.tagName=="SPAN" && e.className=="pm")
        this.expand(e.id.substr(e.id.lastIndexOf("_") + 1));
      break;
  }
};

MzTreeView.prototype.dblClickHandle = function(e)
{
	//alert("dblClickHandle"); 
	findOrAllFlag = "1";//add only for dfx 1.1, show all info on clicking tree root node.
	
  e = window.event || e; 
  e = e.srcElement || e.target;
  if((e.tagName=="A" || e.tagName=="IMG")&& e.id)
  {
    var id = e.id.substr(e.id.lastIndexOf("_") + 1);
    if(this.node[id].hasChild) 
	  {
		this.expand(id);
	  }
  }
};


MzTreeView.prototype.upperNode = function()
{
	//alert("upperNode"); 
  if(!this.currentNode) 
	  return;
  if(this.currentNode.id=="0" || this.currentNode.parentId=="0") 
	  return;
  if (this.currentNode.hasChild && this.currentNode.isExpand)
    {
	  this.expand(this.currentNode.id, false);
	}
  else
	{
	  this.focusClientNode(this.currentNode.parentId);
	}
};


MzTreeView.prototype.lowerNode = function()
{
	//alert("lowerNode"); 
  if (!this.currentNode) 
	  this.currentNode = this.node["0"];
  if (this.currentNode.hasChild)
  {
    if (this.currentNode.isExpand)
      this.focusClientNode(this.currentNode.childNodes[0].id);
    else this.expand(this.currentNode.id, true);
  }
}


MzTreeView.prototype.pervNode = function()
{
  if(!this.currentNode) 
	  return;
  var e = this.currentNode;
//alert("node id: " + e.id);
  if(e.id=="0") 
	  return; 
  var a = this.node[e.parentId].childNodes;
  for(var i=0; i<a.length; i++)
  {
	  if(a[i].id==e.id)
	  {
	     if(i>0)
		 {
			   e=a[i-1];
			   while(e.hasChild)
			   { 
				   this.expand(e.id, true);
				   e = e.childNodes[e.childNodes.length - 1];
				}
			   this.focusClientNode(e.id);
			   return;
		   } 
		   else 
		   {
				this.focusClientNode(e.parentId); return;
		   }
      }
  }
};


MzTreeView.prototype.nextNode = function()
{
	//alert("nextNode"); 
  var e = this.currentNode; 
  if(!e)
	{
	  e = this.node["0"];
	}
  if (e.hasChild)
 {
  this.expand(e.id, true);
  this.focusClientNode(e.childNodes[0].id); 
  return;
  }
  while(typeof(e.parentId)!="undefined")
 {
	  var a = this.node[e.parentId].childNodes;
	  for(var i=0; i<a.length; i++)
	  { 
		 if(a[i].id==e.id)
		{
			 if(i<a.length-1)
				 {
				 this.focusClientNode(a[i+1].id); 
				 return;
				 }
	         else e = this.node[e.parentId];
		 }
	   }
  }
};


MzTreeView.prototype.expandAll = function()
{
	//alert("expandAll"); 
  if(this.totalNode>500) if(
    confirm("您是否要停止展开全部节点？\r\n\r\n节点过多！展开很耗时")) return;
  if(this.node["0"].childNodes.length==0) return;
  var e = this.node["0"].childNodes[0];
  var isdo = t = false;
  while(e.id != "0")
  {
    var p = this.node[e.parentId].childNodes, pn = p.length;
    if(p[pn-1].id==e.id && (isdo || !e.hasChild))
	{
		e=this.node[e.parentId]; 
		isdo = true;
	}
    else
    {
      if(e.hasChild && !isdo)
      {
        this.expand(e.id, true), t = false;
        for(var i=0; i<e.childNodes.length; i++)
        {
          if(e.childNodes[i].hasChild)
		  {
			  e = e.childNodes[i]; 
			  t = true; 
			  break;
		  }
        }
        if(!t) isdo = true;
      }
      else
      {
        isdo = false;
        for(var i=0; i<pn; i++)
        {
            if(p[i].id==e.id) 
		    {
				e = p[i+1]; 
				break;
			}
        }
      }
    }
  }
};


MzTreeView.prototype.setIconPath  = function(path)
{
	//alert("setIconPath"); 
  var k = 0, d = new Date().getTime();
  for(var i in this.icons)
  {
    var tmp = this.icons[i];
    this.icons[i] = new Image();
    this.icons[i].src = path + tmp;
    if(k==9 && (new Date().getTime()-d)>20)
    {
//	    this.wordLine = true; 
	    this.wordLine = false; 
// 		alert("this.wordLine=" + this.wordLine);
    }
	k++;
  }
  for(var i in this.iconsExpand)
  {
    var tmp = this.iconsExpand[i];
    this.iconsExpand[i]=new Image();
    this.iconsExpand[i].src = path + tmp;
  }
};

// add by lzy,2006-11-13
MzTreeView.prototype.addNode  = function(id, data)
{	
	if (id == "")
		id = this.currentNode.id;
		
		 
	this.node[id].hasChild = true;


	if(this.node[id].childNodes != null)
	{ 
		if (!this.node[id].isLoad){			
			this.expand(id, true);			
		}	
		else{
			this.createDynNode(id, data);			
		}			
		return;		
	}
	//before no node.
	var node = this.node[id];			
	this.node[id].childNodes = [];					
	this.node[id].isExpand = true;

	this.createDynNode(id, data); 	
	this.switchIcon( id );	
	//this.focusClientNode(this.index)
	//alert(this.index + "?? " +sid + "this.currentNode =" + this.currentNode.id);


};

MzTreeView.prototype.createDynNode  = function(id, data)
{
	var node = this.node[id];
	var d = this.divider, _d = this._d;
	var sid = node.sourceIndex.substr(node.sourceIndex.indexOf(d) + d.length);// add by lzy,���ڵõ�node.sourceIndex���ӽڵ�Id����				
	var newSid = (++this.total);	
	var sourceIndex = sid + '_' + newSid;

	this.nodes[sourceIndex] = data;	
	this.dataFormat();				
	node.isLoad = true; 

	var tcn = node.childNodes; 
	tcn[tcn.length] = this.nodeInit(sourceIndex, id);		

	var str = this.nodeToHTML(tcn[tcn.length-1], true);
	//var temp = this.getElementById(this.name +"_tree_"+ id).childNodes;	
	var area  = this.getElementById(this.name +"_tree_"+ id);
	if (area)   area = area.childNodes[area.childNodes.length-1];	
	area.innerHTML += str;	
	area.style.display = 'block';	
}

MzTreeView.prototype.deleteNode  = function()
{
	var id = this.currentNode.id;
	var node = this.node[id];
	//var pid = node.sourceIndex.substr(0, node.sourceIndex.indexOf(d));// add by lzy,用于得到node.sourceIndex中父节点Id部分	
	 
	//Clear节点的HTML	
	var area  = this.getElementById(this.name +"_tree_"+ id);	
	var pNode = this.node[node.parentId];
	var prevNode;

	this.changeFocusNode(id);	
  
	//adjust childNodes[] parent node 
	//alert("bef = "+  pNode.childNodes.length)
	pNode.childNodes.remove( node );	
	//alert("aft = "+ pNode.childNodes.length)
	if (pNode.childNodes.length == 0)//If the current node is the only one, then you must switch the parent node expand icon.
	{	
		//pNode.childNodes = "";
		pNode.hasChild = false;
		pNode.isExpand = false;
//		pNode.isLoad = false;
		this.switchIcon( pNode.id );		
		area.parentNode.style.display = 'none';		
		
	}		 
	else
	{	//if you delete the last node, then the expand icon of his prev node need to switch. 
		prevNode = this.node[pNode.childNodes[pNode.childNodes.length-1].id];
		 
		if (isAtEnd( prevNode ))
		{
			//alert("pNode.childNodes.length = ", pNode.childNodes.length);
			this.switchIcon( prevNode.id );
		}
	}
	 
	//if (isAtEnd( prevNode ))	
	{		 
		//setChildPrevIconBlank( prevNode )
	}
	area.outerHTML = "";	

//adjust this.node[]
	//this.node.remove(node);
//adjust this.nodes
	delete this.nodes[ node.sourceIndex ];	

//adjust this.names
	this.dataFormat();	
		

};

MzTreeView.prototype.swapNode  = function(direct)
{
	var currentNode = this.currentNode;
	var nodeTag = this.getElementById(this.name +"_tree_"+ currentNode.id);
	var prefixLen = (this.name +"_tree_").length;
	var	otherNodeId, otherNode = null;

	if (direct == "up")
	{
		if (isAtBegin(currentNode)) return false;	
		
		otherNodeId = ((nodeTag.previousSibling).id).substr(prefixLen);
		otherNode = nodeTag.previousSibling;
	}
	else if(direct == "down")
	{
		if (isAtEnd(currentNode)) return false;		

		otherNodeId = ((nodeTag.nextSibling).id).substr(prefixLen);
		otherNode = nodeTag.nextSibling;
	}
	else
	{
		alert("Please specify the moving direction.");
		return;
	}

//swap html tag	
	nodeTag.swapNode(otherNode);

	//swap array.		
	var secondIndex = this.node[otherNodeId].sourceIndex;
	//alert(currentNode.sourceIndex+",," + secondIndex)
	//debugger;
	//this.nodes = swapNodesArray (this.nodes, currentNode.sourceIndex, secondIndex);
	currentNode.parentNode.childNodes = swapChildNodesArr( currentNode.parentNode.childNodes, currentNode.sourceIndex, secondIndex);
//for test 
/*	for (var i=0; i<currentNode.parentNode.childNodes.length; i++)
	{
		alert(currentNode.parentNode.childNodes[i].id);
	}
*/
	if (isAtEnd(currentNode) || isAtEnd(this.node[otherNodeId]))
	{		 
		this.switchIcon( currentNode.id );
		this.switchIcon( otherNodeId );
	}

}


MzTreeView.prototype.changeFocusNode = function(id)
{
  if(id=="0") 
  	return; 
  var e = this.node[id];
  var a = this.node[e.parentId].childNodes;
  
  for(var i=0; i<a.length; i++)
  {		 
		if(a[i].id==e.id)
		{
			if(i>0)//focus prv node
			{
				e=a[i-1];				 
				this.focusClientNode(e.id); 
				//this.nodeClick(e.id);
			} 
			else 
			{	
				//alert(i + ",a.length=" + a.length)
				if(i < (a.length-1))//focus next node	
				{	this.focusClientNode(a[i+1].id); 
					//this.nodeClick(a[i+1].id); 
				}
				else
				{	this.focusClientNode(e.parentId); 
					//this.nodeClick(e.parentId); 
				}
				return;
			}
		}
	}

	return;
}


Array.prototype.remove = function(obj)
{
    for ( var i=0 ; i < this.length ; ++i )
    {
        if ( this[i] == obj )
        {
            if ( i > this.length/2 )
            {
                for ( var j=i ; j < this.length-1 ; ++j )
                {
                    this[j] = this[j+1];
                }
                this.pop();
            }
            else
            {
                for ( var j=i ; j > 0 ; --j )
                {
                    this[j] = this[j-1];
                }
                this.shift();
            }    
            break;
        }
    }
};


MzTreeView.prototype.switchIcon  = function(id)
{	
	var cuNode  = this.node[id];	
	var HCN  = cuNode.hasChild;
	var isRoot = cuNode.parentId=="0";	 
	var AtEnd = isAtEnd( cuNode );
	   
	if(isRoot && cuNode.icon=="") cuNode.icon = "root";
	//if(cuNode.icon=="" || typeof(this.icons[cuNode.icon])=="undefined")
	cuNode.icon = HCN?"folder":"file";	 
	
	cuNode.iconExpand  = HCN ? AtEnd ? "PM2" : "PM1" : AtEnd ? "L2" : "L1";
	
	var area  = this.getElementById(this.name +"_tree_"+ id);
	if (area)   area = area.childNodes[area.childNodes.length-1];
	if (area)
	{
//		var icon  = this.icons[cuNode.icon];		
//		var iconE = this.iconsExpand[cuNode.icon];
//		var Bool  = cuNode.isExpand || area.style.display == "none";
		var Bool  = cuNode.isExpand;
		var img   = this.getElementById(this.name +"_icon_"+ id);
		if (img)
		{
		  //img.src = !Bool ? icon.src :typeof(iconE)=="undefined" ? icon.src : iconE.src;
		  img.src = this.getIconPath( cuNode );
		}
		var exp   = this.icons[cuNode.iconExpand];
		var expE  = this.iconsExpand[cuNode.iconExpand];
		var expand= this.getElementById(this.name +"_expand_"+ id);
		if (expand)
		{		  
		  if(this.wordLine) expand.innerHTML = !Bool ? "+"  : "-";
		  else expand.src = !Bool ? exp.src : typeof(expE) =="undefined" ? exp.src  : expE.src;
		  
		}	
	}
	
	
};


MzTreeView.prototype.getIconPath = function( node )
{
//	var path = this.icons[node.icon].src;
	var icon  = this.icons[node.icon];		
	var iconE = this.iconsExpand[node.icon];
	var Bool  = node.isExpand;
	
	var path = !Bool ? icon.src :typeof(iconE)=="undefined" ? icon.src : iconE.src;
	
	
	if (typeof(this.customIconFun) == "function")
		path = this.customIconFun(node, eval(this.name)) ;
	
	//path = useSOPMngIcon(node, eval(this.name));
	return path;

}

//删除最后一个节点的时候，前一个节点的字节点的前边的竖线要去掉
function setChildPrevIconBlank( node )
{		
	if (node.hasChild && node.isLoad)
	{ 
		if (node.childNodes.length > 0)
			{
				for (var i=0; i<node.childNodes.length; i++)
				{
					 

					eval("tree_childAppend_" + node.childNodes[i].id).innerHTML = "";
				}		
			}
	}	
//	 
}


function isAtEnd( node )
{
    
    var pLastChild = node.parentNode.childNodes[node.parentNode.childNodes.length - 1];
	return (pLastChild.id == node.id)
		
}

function isAtBegin( node )
{
    
    var pFirstChild = node.parentNode.childNodes[0];
//	alert(pFirstChild.id +";"+ node.id)
	return (pFirstChild.id == node.id);
		
}

//firstIndex and secondIndex are the sourceIndex
function swapNodesArray (nodes, firstIndex, secondIndex)
{
/*	var nodes = new Array();	 
	nodes['-1_1'] = 'text:节点 1';
	nodes['1_2'] = 'text:节点 2';
	nodes['1_3'] = 'text:节点 3';
	nodes['1_4'] = 'text:节点 4';
	nodes['1_5'] = 'text:节点 5';
*/
	if (firstIndex == secondIndex)
	{
		alert("The same two value, no need to swap.")
		return nodes;
	}
	//alert("firstIndex =" +firstIndex+"secondIndex =" +  secondIndex)
	var indexArr = new Array();
	var onePos = -1, twoPos = -1;
	for (var id in nodes) 
	{
		if (id == firstIndex)
			onePos = indexArr.length;
		if (id == secondIndex)
			twoPos = indexArr.length;
		//alert("id=" + id)
  		indexArr[indexArr.length] = id;		
		
		if (onePos !=-1 && twoPos != -1)
			break;
	}

	//need swap
	if (onePos ==-1 || twoPos == -1)
	{
		alert ("Please set a correct pos number.");
		return nodes;
	}

	var onePosData = indexArr[onePos];
	indexArr.splice (onePos, 1, indexArr[twoPos]);
	indexArr.splice (twoPos, 1, onePosData);

	var tempNodes = new Array();
	for (var i=0; i<indexArr.length; i++)
	{
		tempNodes[indexArr[i]] = nodes[indexArr[i]];
		//alert(tempNodes[indexArr[i]]);
	}

	return  tempNodes;
	 
}

function swapChildNodesArr(nodes, firstIndex, secondIndex)
{
	var onePos = -1, twoPos = -1;
	for (var i=0; i<nodes.length; i++) 
	{
		if (nodes[i].sourceIndex == firstIndex)
			onePos = i;
		if (nodes[i].sourceIndex == secondIndex)
			twoPos = i;
		  				
		if (onePos !=-1 && twoPos != -1)
			break;
	}
 
	//need swap
	if (onePos ==-1 || twoPos == -1)
	{
		alert ("Please set a correct sourceIndex.");
		return nodes;
	}

	var onePosData = nodes[onePos];
	nodes.splice (onePos, 1, nodes[twoPos]);
	nodes.splice (twoPos, 1, onePosData);
	
	return nodes;
}

//key UUID
MzTreeView.prototype.getNodePathByKey = function(key)
{ 
	this.recordNodePath(this.currentNode,1000);
	
	return this.freshPath;

}

MzTreeView.prototype.freshRootNode = function()
{	  
    var node = this.currentNode,backLayer;    
    this.freshPath = new Array();
    
	while (parseInt(node.id) > 1)
	{ 	 		
	    var source = this.nodes[node.sourceIndex];
        this.freshPath[this.freshPath.length] = this.getAttribute(source, "UUID");
			 
		node = node.parentNode; 
	 
	}
  
	this.delChildrenNodeInfo(node);
	this.delChildrenHTML(node.id);
	
	node.isLoad = false;
	node.isExpand = false;
	node.hasChild = true;
	node.childNodes = new Array();	
	this.switchIcon(node.id);
	this.expand(node.id, true);
	this.focusClientNode(node.id);
	
	return true;
}

MzTreeView.prototype.freshCurrentNode = function( backLayer )
{	  
    var node = this.currentNode,backLayer;
    
	if (typeof(backLayer) == "number")
	{		 
		node = this.recordNodePath(node,backLayer);
	} else {
	    alert("Wrong param value:backLayer=" + backLayer);
	    return false;
	}
  
	this.delChildrenNodeInfo(node);
	this.delChildrenHTML(node.id);
	
	node.isLoad = false;
	node.isExpand = false;
	node.hasChild = true;
	node.childNodes = new Array();	
	this.switchIcon(node.id);
	this.expand(node.id, true);
	this.focusClientNode(node.id);
	
	return true;
}

MzTreeView.prototype.recordNodePath = function(startNode, backLayer)
{
	var node = startNode;
	
	if (parseInt(node.id) > 1)
	{
		this.freshPath = new Array();
		for (var i=0; i<backLayer; i++)
		{	
			if (i <= backLayer - 1) {				
			    var source = this.nodes[node.sourceIndex];
                this.freshPath[this.freshPath.length] = this.getAttribute(source, "UUID");
				 
			}
			node = node.parentNode;
						
			if ((node.id + "") == "1")
				break;
		}	
	}
	
	return node;
}


MzTreeView.prototype.delChildrenNodeInfo = function( node )
{
	if (node.hasChild)
	{	 
	//alert("before = " + this.node.length);
		for (var i=0; i<node.childNodes.length; i++)
		{
			var chNode = node.childNodes[i];
			if (chNode.hasChild)
			{
				this.delChildrenNodeInfo( chNode );
				chNode.childNodes = new Array();
			}
			
		//now delete current node.
			//1.delete from this.nodes[]			 
			delete this.nodes[ chNode.sourceIndex ];
			 
			//2.delete from this.node[], Now don't delete, save this position, althogh it will be unuseful for ever.
			//this.node.remove(chNode);	
						
		}
	//alert("before = " + this.node.length);
		//fresh this.names
		//this.dataFormat();
	}
	
	return true;

}

MzTreeView.prototype.delChildrenHTML = function( id )
{
	var area  = this.getElementById(this.name +"_tree_"+ id);
	if (area)   
		area = area.childNodes[area.childNodes.length-1];	

	area.innerHTML = "";	

}

MzTreeView.prototype.locateNode = function (key)
{
	var node = null;			
	var data;
 
	if (this.freshPath.length > 0){
		data = this.freshPath.pop();//get the last one.
	}else{
		return false;
	}
	 
	this.locatingNode = true;//for rejecting other operations.

	if (key.length == 0 || this.currentNode == null)
	{
		this.freshPath = new Array();
		alert("key is null or no current node.");
		
		return false;
	}
 
	for (var i=0; i< this.currentNode.childNodes.length; i++)
	{	
		node = this.currentNode.childNodes[i];
		
		var source = this.nodes[node.sourceIndex];
		var keyVal = this.getAttribute(source, key);
		//alert(keyVal + ";" + data);
		if (keyVal == data)
		{
			this.focusClientNode(node.id, this.onlyLocate); 
			this.onlyLocate = false;//new add, this property is only useful when freshPath.length is one.
			
			if (node.hasChild && this.freshPath.length != 0)
				this.expand(node.id, true);
			
			if (this.freshPath.length == 0)
				this.locatingNode = false;//denote that all the nodes have been foun,stop locating.

			if (!node.hasChild)
			{
				this.locatingNode = false;//if a  node can't been found, then stop locating.
				this.freshPath = new Array();
			}							
			 
			return true;
		} 		
	}		
	
	if (i > this.currentNode.childNodes.length) {
		this.locatingNode = false;//if a  node can't been found, then stop locating.
		this.freshPath = new Array();
	}
		
	//alert(i+ ";" + node.text+ ",node.id= " +node.id);
		
	return false;

}

//从树根查找某节点类型为propertyKey指定值的节点，并选中。
//onlyLocate means the click event isn't raised.
MzTreeView.prototype.searchInChildNodes = function (propertyKey, propertyValue, onlyLocate)
{ 
	var node = null; 
	 
	if (this.currentNode == null)
	{
		alert("No selected node.");
		return false;
 	} 
	 
	if (!this.currentNode.hasChild)//if no child then do nothing
	{    
	    //alert("No children.");
	    return false;
	}
	
	if (this.currentNode.hasChild && !this.currentNode.isLoad)
	{
	    this.onlyLocate = onlyLocate;
	    this.freshPath[this.freshPath.length] = propertyValue;
	    this.expand(this.currentNode.id, true);
	    
		//alert("The Child nodes of begin node have not been loaded.");
		return false;
 	}   
 	
 	this.expand(this.currentNode.id, true); 
	 
	for (var i=0; i< this.currentNode.childNodes.length; i++)
	{	 
		node = this.currentNode.childNodes[i];

		var source = this.nodes[node.sourceIndex];
		var nodeType = this.getAttribute(source, propertyKey);
		
		if (nodeType.toUpperCase() == propertyValue.toUpperCase())
		{ 
		    this.focusClientNode(node.id, onlyLocate); 
		    break;
//			var nodeText = this.getAttribute(source, "text").toUpperCase();
//		 
//			if (nodeText.indexOf(data.toUpperCase()) >= 0){					
//				//found
//				var link = this.getElementById(this.name +"_link_"+ node.id);		
//				link.style.backgroundColor = "aquamarine"; 
//				
//			} 		
		}
	}			 
		
	return true;

}
 


MzTreeView.prototype.clearHilight = function (node)
{
	if (!(node.hasChild && node.isLoad))
		return false;
			 
	for (var i=0; i< node.childNodes.length; i++)
	{	
		var child = node.childNodes[i];			 
		var link = this.getElementById(this.name +"_link_"+ child.id);	
		
		if (this.currentNode.id == child.id)	
		{ 
			with(link.style)
			{
				color = this.colors.highLightText;
				backgroundColor = this.colors.highLight;
			}  
			
    	}else {
    		with(link.style)
			{
				color = "";
				backgroundColor = "";
			} 
    	}
    	
    	this.clearHilight(child);	
		
		
		 	
	}	
}

MzTreeView.prototype.findRootNode = function ()
{	
	var firstNode = this.node["0"];
	var node;
	for (var i=0; i< firstNode.childNodes.length; i++)
	{	 
		node = firstNode.childNodes[i];

		var source = this.nodes[node.sourceIndex];
		var nodeType = this.getAttribute(source, "type");
		
		if (nodeType == "NODE_ROOT")
			break;
		
	}
	
	if (i >= firstNode.childNodes.length)
	{
		alert("can't find the root node on searching.Stopped.");
		node = firstNode;
	}
	
	return node;
	
}

//set the node to no children status.
MzTreeView.prototype.emptyNode = function (node)
{	
	var id = node.id;
	
	node.hasChild = false
	this.switchIcon(node.id);
	tree.delChildrenNodeInfo(node);
	this.delChildrenHTML(node.id);

	var area  = this.getElementById(this.name +"_tree_"+ id);
	if (area)   area = area.childNodes[area.childNodes.length-1];
	if (area)
	{
	  	area.style.display = "none";
	}
	
	//HideWait();
}


//nodeType:the type to search.sample:"NODE_SOP"
//return value: null--can't find；success: the found node.
//node: this function search tree up from the current node.
MzTreeView.prototype.searchUpNodeByType = function(nodeType)
{
  var reValue = null;
  var node = this.currentNode;
  
  if (key.length == 0 || node == null)
  {
     alert("Can't find the current node,exit.");
     return reValue; 
  }
  
  var source = this.nodes[node.sourceIndex];
  
  while (this.getAttribute(source, "type") != "NODE_ROOT")
  {   
      if (this.getAttribute(source, "type") == nodeType) {   
      	 reValue = node;
         break; 
      }
      
      node = node.parentNode;  
      source = this.nodes[node.sourceIndex];
  }   
	
	if (null == reValue)
	{
		
	}
	
  return reValue; 
}

//get the node property value
MzTreeView.prototype.getNodeProperty = function(node, propertyName)
{
	if (node == null)
	{
		alert("The node param is null in treeview.getNodeProperty().");
		return null;
	}
	
	var source = this.nodes[node.sourceIndex];

	return this.getAttribute(source, key);  

}

//从当前结点向上搜索指定类型的树节点，并取回属性值。
MzTreeView.prototype.findUpNodePropertyByType = function(nodeType, propertyName)
{
	
	return this.getNodeProperty(this.searchUpNodeByType(nodeType), propertyName);  

}

/*
function createRejectLayer()
{
	var sHTML='<div id="divReject"  align="center" style="filter:Chroma(Color=skyblue); background-color: skyblue; display: block; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute"></div>';
	document.write(sHTML);
}

function enableOperate()
{ 
	divReject.style.display = "none";
}
function disableOperate()
{
	divReject.style.display = "block";
}
createRejectLayer()
*/


MzTreeView.prototype.setURL     = function(url){this.url = url;};

MzTreeView.prototype.setTarget  = function(target){this.target = target;};
// -->
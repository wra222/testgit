
//btnCell Class
	function BtnCell()
	{
		this.id = "";
		this.normal_img = "";
		this.hi_img = "";
		this.disabled_img = "";
		//method below
		this.buildHTML = createHTML;
		this.enable = "";
		this.title = ""
	}
	
	function createHTML()
	{
		var sContent = "<td id='" + this.id + "' name='" + this.id + "' width='10' style='cursor:hand;' ";		 
		sContent += "onmousedown='this.setCapture();this.style.filter=\"progid:DXImageTransform.Microsoft.BasicImage( Rotation=0,Mirror=0,Invert=0,XRay=0,Grayscale=0,Opacity=0.75)\"' ";		 
		sContent += "onmouseup='this.style.filter=\"\";this.releaseCapture();' ";		 
		sContent += ">";		 
		sContent += "<IMG  onmouseover='if(!this.parentNode.disabled)this.src=\"" + this.hi_img;
		sContent += "\"' onmouseout='if(!this.parentNode.disabled)this.src=\"" + this.normal_img;
		sContent += "\"' onmouseout='if(!this.parentNode.disabled)this.src=\"" + this.normal_img;
		sContent += "\"' SRC='" + this.normal_img + "'  BORDER=0  title='" + this.title + "'>";
		sContent += "</td>";
		
		return sContent;
	}
	
//BtnGroup Class
	function BtnGroup()
	{
		this.buttons = new Array();
		this.container = "";
		this.fun_onClick = "";
		this.splitCols = "";
		//this.splitCols = "4,7,";
		this.align = "right";
		
		//methods
		this.addBtn = addButton;
		this.addBtns = addButtons;
		this.display = displayHTML;	
		this.enableBtn = enableButton;
		this.className = "title";
		
	}

	function addButton( btnCell )
	{		 
		this.buttons[this.buttons.length] = btnCell;
	
	}
	
	function addButtons( btnArray )
	{
		this.buttons = btnArray;
		
	}
	

	function displayHTML()
	{
	  	
	   
		if (typeof(eval(this.container)) != "object")
		{
			alert("Please specify the container property value.");
			return;
		} 
		  
		if (this.fun_onClick != "")
			{var sContent = "<TABLE class='" + this.className + "' onclick='doClick(\"" + this.fun_onClick + "\")' border=0 cellspacing=0 cellpadding=0 width='100%'><TR>";
		    }
		else
			var sContent = "<TABLE class='" + this.className + "' onclick='doClick( \"\" )' border=1 cellspacing=0 cellpadding=0  width='100%'>";
		
		if (this.align == "right")
			sContent += "<TR><td disabled></td>";
		else//left align	
			sContent += "<TR>";
			
		var nBlankCount = 0;	 
		for(var i=0; i<this.buttons.length; i++)
		{
			//sContent += "<TD>" + this.buttons[i].buildHTML() + "</TD>";
			sContent += this.buttons[i].buildHTML() ;
			if (this.splitCols.indexOf(i + ",") != -1)// add blank col
			{
				sContent += "<TD width='3'>&nbsp;</TD>";
				nBlankCount ++;				
			}
		}

		if (this.align == "right")
			sContent += "<td disabled width='10'></td></TR>";
		else
			sContent += "<td disabled></td></TR>";
			
		sContent += '<TR><td height="2" colspan="' + (i + 2 + nBlankCount) + '" background="../../images/line2.jpg"style="background-repeat: repeat-x; background-position: bottom 50%"></td></TR></TABLE>'
		
		//prompt("", sContent);
		eval(this.container).innerHTML = sContent;

	}

	//data:id or number
	//state:true/false;
	function enableButton(data, state)
	{		 
		var num = 0;
		var elem;
		if (typeof(data) == "string")
		{// now the data is the button id.
			for (var i=0; i<this.buttons.length; i++)
			{
				//alert(data +":"+ this.buttons[i].id)
				if (data == this.buttons[i].id )
					break;
			}
			
			if (i >= this.buttons.length)
			{
				alert("Please set a correct id.");
				return false;
			}
			
			num = i;//found
					
		}
		else//number
		{
			if (data >= this.buttons.length)
			{
				alert("The number is out of the range.");
				return false;
			}
			num = data;			
		}	
		//alert("this.buttons.length=" + this.buttons.length)
		
		elem = eval(this.buttons[num].id);
//		elem =document.getElementsByName(this.buttons[num].id);
		//elem = getElementById(this.buttons[num].id);
		if (state)
			elem.style.cursor = "hand";
		else
			elem.style.cursor = "default";
			
		elem.disabled = !state;
		if(!state)
			elem.childNodes(0).src = this.buttons[num].disabled_img;
		else
			elem.childNodes(0).src = this.buttons[num].normal_img;
			
		 
		return;
	}


	function doClick( outerFun )
	{
	
		var elem;
		if(event.srcElement.tagName == "TD")
			elem = event.srcElement;
		else if(event.srcElement.tagName == "IMG")
			elem = event.srcElement.parentNode;		
		else
			return false;
		 
		if (elem.disabled == true)
			return false;
		if (outerFun != "")
		{
			//eval("outerFun"+"("+ elem+")");
			//doFunction(elem);
			//elem.style.filter = "progid:DXImageTransform.Microsoft.Glow(color=#ff00ff,strength=1)";
			eval(outerFun)(elem);
			 
		}	
		return ;	
	}
	
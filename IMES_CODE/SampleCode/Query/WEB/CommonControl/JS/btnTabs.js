		//Tabs control created by lzy
		
		function clsButtonTabs(Tname)
		{
			if(typeof(Tname) != "string" || Tname == "")
				throw(new Error());
				
			this.name     = Tname;
			this.buttons= new Array();
			this.selectedButton = null;
			this.imagePath = "";
			this.disabledButtons = new Array();
			this.lastSelectedIndex = 0;
			this.container = null;//new add
			this.mode = "picture";//picture/text
			this.pageCount = 0;//hided count
			this.beDynamic = true;//can work on the "text" mode.
			this.maxIndex = 0;
			
		//methods public
			this.addButton = _addButton;
			this.initSelect = _initSelect;
			this.selectButton = _selectButton;
			this.selectChanged = null;
			this.preSelectChanged = null;
			this.preTabDeleted = null;
			this.preTabAdd = null;
			
			this.tabAdded = null;
			this.tabDeleted = null;
			this.toString = _buildTabs;
			this.diableTab = _diableTab;
			this.prevMove = _prevMove;
			this.nextMove = _nextMove;
			this.dynaAddDel = _dynaAddDel;
			this.setTabText = _setTabText;//change tab text
			this.getTabText = _getTabText;//change tab text
			this.getTabOriginalIndex = _getTabOriginalIndex;
			this.enableDynamic = _enableDynamic;
        //method private				        
			this.createRealBody = _createRealBody;		
			this.freshScanner = _freshScanner;
			this.freshButtonShow = _freshButtonShow;
			this.freshOffsetByPage = _freshOffsetByPage;
			
			if (document.styleSheets[0].insertRule) {
			    document.styleSheets[0].insertRule("a {font: 11pt bold Arial; text-decoration: blink; color: blue;}", 0);
			    document.styleSheets[0].insertRule("a:hover {font: 11pt bold Arial; text-decoration: blink; color: orangered;}", 0);
			    document.styleSheets[0].insertRule("a:active {font: 11pt bold Arial;text-decoration: blink; color: orangered;}", 0);
			}   else { 
			    document.styleSheets[0].addRule("a", "font: 11pt bold Arial; text-decoration: blink; color: blue;", -1);
			    document.styleSheets[0].addRule("a:hover", "font: 11pt bold Arial; text-decoration: blink; color: orangered;", -1);
			    document.styleSheets[0].addRule("a:active", "font: 11pt bold Arial;text-decoration: blink; color: orangered;", -1);
			} 
		}
		
		function _diableTab(index, value)
		{
			this.disabledButtons[index] = value;		 
            this.buttons[index].setDisabled(value);
		}
		
		function _addButton(buttonCell)
		{
			buttonCell.index = this.buttons.length;
			buttonCell.originalIndex = this.maxIndex++;
			this.buttons[this.buttons.length] = buttonCell;
            
		}
		
		function _initSelect(index)
		{     
		    
			if (this.selectedButton != null)
			{
				//click the selected button
				if (this.selectedButton.index == index)
					return false;
					
				this.lastSelectedIndex = this.selectedButton.index;
				this.selectedButton.unSelect();
			}
			
			if (this.buttons.length > index)
			{
			    this.buttons[index].select();
			    this.selectedButton = this.buttons[index];	
			}			
    			
			this.freshButtonShow();
			this.freshScanner();
			
			return true;			 
		}	 
					
		function _selectButton(index)
		{			 
			if (this.disabledButtons[index] == true)
				return false;
				
			if (this.preSelectChanged != null)
			{
				if (!this.preSelectChanged(this.lastSelectedIndex))
				    return false;
			}
			
				
			if (!this.initSelect(index))
				return false; 
			
			//call outer function
			if (this.selectChanged != null)
				this.selectChanged(index);	
		}

		function _buildTabs()
		{ 
			var sContent = "<TABLE border=0 cellspacing=0 cellpadding=0 width='100%' height='35px'><TR class='Tree_backgroud'><td >";
			var nBlankCount = 0;	
			
//			if (this.container == null){
//			    alert("Warning,this.container property is null!");
//			    
//			}
			
			if (this.container != null && this.container.style.pixelWidth == 0)

			    alert("Please initialize the width property of the tabs container!");
			  
			sContent += "<Div id='realCarrier' name='realCarrier' style='overflow:hidden; height:100%;' >";
			sContent += this.createRealBody();  
			sContent += "</div></td>"; 
			if (this.mode == "text")
			//modify by itc207024 2009-10-13 add id for buttons
//                sContent += "<td align='right' style='width:85px;' nowrap><span id='dynOperationPan' style='display:none;'><a id='tabadd" + this.name + "' HREF='javascript:eval(\"" + this.name + "\").dynaAddDel(\"Add\")'>&nbsp;+&nbsp;</a><a id='tabdel" + this.name + "' HREF='javascript:eval(\"" + this.name + "\").dynaAddDel(\"Del\")'>&nbsp;-&nbsp;</a><span id='scrollArea'><a  id='btnPrev' HREF='javascript:eval(\"" + this.name + "\").prevMove()'>&nbsp;<&nbsp;</a><a id='btnNext' HREF='javascript:eval(\"" + this.name + "\").nextMove()'>&nbsp;>&nbsp;</a></span></span></td></TR>";				
                //iMES中不需要"+"功能
                sContent += "<td align='center' valign='middle' style='width:75px;background-color:#3E3F41;' nowrap><span id='dynOperationPan' style='display:none;'><img id='tabdel' src=\"./Images/tab-delete.jpg\" style=\"width:11px;height:11px;border:0px;cursor:hand\" onclick='javascript:eval(\"" + this.name + "\").dynaAddDel(\"Del\")' /><span id='scrollArea'>&nbsp;&nbsp;<img id='btnPrev' src=\"./Images/tab-prev.jpg\" style=\"width:13px;height:13px;border:0px;cursor:hand\" onclick='javascript:eval(\"" + this.name + "\").prevMove()' />&nbsp;&nbsp;<img id='btnNext' src=\"./Images/tab-next.jpg\" style=\"width:13px;height:13px;border:0px;cursor:hand\" onclick='javascript:eval(\"" + this.name + "\").nextMove()' /></span></span></td></TR>";
            else
                sContent += "<td >&nbsp;</td></TR>";
                
//			sContent += '<TR><td colspan="2" style=\"background-color: rgb(147,191,218);height:5px;\"></td></TR></TABLE>'
			sContent += "<TR><td colspan='2' class='Tree_backgroud' style='height:2px'></td><TR><td colspan='2' style='height:10px'></TR></td></TR></TABLE>"
              
            if (this.container != null)
            {
                this.container.innerHTML = sContent;   
                realCarrier.style.width = this.container.clientWidth - 75;
                
                this.freshScanner();
                
                if (this.mode == "text")
                    dynOperationPan.style.display = "block";
//                else
//                    dynOperationPan.style.display = "none";       

                this.enableDynamic(this.beDynamic);
                    
             }             
            
			return sContent;			
		} 
		
		function _createRealBody()
		{
		    var sContent = "<TABLE border=0 cellspacing=0 cellpadding=0 style='position:relative;width:1%' ><TR>";
			
			for(var i=0; i<this.buttons.length; i++)
			{				 
				sContent += this.buttons[i].buildHTML() ;
			}
			
			sContent += "</TR></TABLE>";
			
			return sContent;
		}
		
		function _dynaAddDel(type)
		{ 		
			if (this.container == null){
	            alert("tabs.Container can't be null if you want to add/del tab dynamicly, ignore!");
	            return;
	        } 
	        
		    if (type == "Add")
		    { 
                //call outer function
			    if (this.preTabAdd != null)
				{    
				    if (this.preTabAdd() == false)
				        return;//ignore
				 
				 }		    
		    
		        var temp = new clsButton(this.name); 
			    this.addButton(temp);  
			    
			    //call outer function
			    if (this.tabAdded != null)
				    this.tabAdded(this.buttons.length - 1);
		    }
		    else 
		    {
		        if (this.buttons.length == 0) {
		            return;
		        }

		        if (this.buttons.length == 1)
		        {
		            alert("The tab count cannot be less than one!");
		            return;
		        }
		         
		        
                if (this.pageCount == realCarrier.childNodes[0].rows[0].cells.length)
                {
                    this.pageCount --;
                } 
		         
                //call outer function
			    if (this.preTabDeleted != null)
				{    
				    if (this.preTabDeleted(this.selectedButton.index) == false)
				        return;//ignore
				 
				 }
				    		
		        for (var i=this.selectedButton.index+1; i<this.buttons.length; i++)
		            this.buttons[i].index -=1;
		        
			    this.buttons.splice(this.selectedButton.index, 1); 	
			    		    
                //call outer function
			    if (this.tabDeleted != null)
				    this.tabDeleted(this.selectedButton.index); 
				       
                this.selectedButton = null; 

		    }  
		   
			realCarrier.innerHTML = this.createRealBody();
						 
			this.freshOffsetByPage();			
		    this.selectButton(this.buttons.length - 1);
		    
		     
		}
		
		function _prevMove()
		{   
		    if (this.pageCount == 0)
		        return;
		        
		    var offsetWidth=realCarrier.childNodes[0].rows[0].cells[this.pageCount-1].clientWidth + 1; 
  	        realCarrier.childNodes[0].style.pixelLeft += offsetWidth; 
		
		    this.pageCount--; 
            
            this.freshScanner();
		}
		
		function _nextMove()
		{    
		   var offsetWidth=realCarrier.childNodes[0].rows[0].cells[this.pageCount].clientWidth + 1; 
  	        realCarrier.childNodes[0].style.pixelLeft -=parseInt(offsetWidth);
  	         
		    this.pageCount++; 
		    
		    this.freshScanner();
		}

        function _freshOffsetByPage()
        {	
            var offsetWidth = 0; 
            for (var i=0; i<=this.pageCount-1; i++){
                offsetWidth += realCarrier.childNodes[0].rows[0].cells[i].clientWidth + 1;
                 
            } 
            
  	        realCarrier.childNodes[0].style.pixelLeft = -(offsetWidth); 
        
        }

		function _freshButtonShow()
		{
		    if (this.mode != "text")
		        return;
		        
		    if (this.selectedButton == null)
		        return false;
		        
		    var wholeWidth=0; 
		
			for(var i=this.pageCount; i<=this.selectedButton.index;i++) 
			{			     
				wholeWidth += realCarrier.childNodes[0].rows[0].cells[i].clientWidth + 1;
			}
			   
			if(realCarrier.clientWidth < wholeWidth)
			{
				this.nextMove();							
				this.freshButtonShow();
				  
		    }
		    
		    if (this.pageCount > this.selectedButton.index)
		    {
		        for (var j=0; j<=this.pageCount-this.selectedButton.index; j++)
		        {
		            this.prevMove();
		        }
		        
		        this.freshButtonShow();
		    } else
		    {
		    
		    
		    }
		    
		    return false;
		}		

        function _freshScanner()
        {           
            if (this.mode != "text")
		        return;
		                
            var buttonsClientWith = 0;
            for (var i=this.pageCount; i<realCarrier.childNodes[0].rows[0].cells.length; i++){
                buttonsClientWith += realCarrier.childNodes[0].rows[0].cells[i].clientWidth + 2; 
            } 
            
            if (buttonsClientWith > realCarrier.clientWidth)
                btnNext.style.visibility = "visible";
            else
                 btnNext.style.visibility = "hidden";
                  
            if (this.pageCount > 0)
                btnPrev.style.visibility = "visible";
            else
                btnPrev.style.visibility = "hidden";
        }
        
        function _setTabText(index, data)
        {
            if (index < 0 || index > this.buttons.length)
            {
                alert("Invalid index value!(in setTabText function)");
                return false;
            }
            
            this.buttons[index].text = data;    
            realCarrier.childNodes[0].rows[0].cells[index].innerHTML = "&nbsp;&nbsp;" + htmEncode(data) + "&nbsp;&nbsp;";
            //debugger;
            this.freshOffsetByPage();
            //this.freshButtonShow();
        }
        
        function _getTabText(index)
        {
            return this.buttons[index].text;
        
        }
        
        function _getTabOriginalIndex(index)
        {
            if (index < 0)
            {
                alert("Wrong index parameter value in getTabOriginalIndex()");
                return -1;
            }
            
            return this.buttons[index].originalIndex;
        
        }
        
        function _enableDynamic(status)
        {
            if (typeof(status) != "boolean")
            {
                alert("Wrong parameter type, enableDynamic");
                return ;
            }
                
            if (status == true){
                document.getElementById("dynOperationPan").childNodes[0].style.visibility = "visible";
                document.getElementById("dynOperationPan").childNodes[1].style.visibility = "visible";
            } else {
                document.getElementById("dynOperationPan").childNodes[0].style.visibility = "hidden";
                document.getElementById("dynOperationPan").childNodes[1].style.visibility = "hidden";
            }
            
        }
        
		function clsButton(parentName)
		{
			if(typeof(parentName) != "string" || parentName == "")
				throw(new Error());
			this.containerName = parentName;

            this.text = "Empty";
  			this.normalPic = "";
			this.selPic = "";
			this.disablePic = "";
			this.moveCursor = "";
			this.index; 
            this.originalIndex;
            
			//method
			this.buildHTML = buildBody;
			this.select = doSelect;
			this.unSelect = doUnSelect;
			this.setDisabled = _changeStatus;
		}

		function buildBody()
		{
			var sContent = "";		 
			 
			if (eval(this.containerName).mode == "picture"){
			    sContent = "<td  width='10' style='cursor:hand;'>";		
			    sContent += "<IMG name='imgButton" + this.index + "' id='imgButton" + this.index + "' SRC='" + this.normalPic + "' BORDER=0  ALT='' " + 
							"onmouseup='eval(\"" + this.containerName + "\").selectButton(" + this.index+ ")'>";
		    } else{
//		        sContent = "<td  id='spanButton" + this.index + "' name='spanButton" + this.index + "' style='font: 11pt Arial} ' onmouseup='eval(\"" + this.containerName + "\").selectButton(" + this.index+ ")' style='cursor:hand;background-color:RGB(201,232,237);border-left: rgb(147,191,218) solid 1px;border-right: rgb(147,191,218) solid 1px ;border-top:RGB(205,222,235) solid 1px' nowrap>";	
                //iMES need backgroung image
//                sContent = "<td><img name='spanLeft" + this.index + "' id='spanLeft" + this.index + "' src='./Images/tab-select-left.jpg' /></td>";
//		        sContent += "<td id='spanButton" + this.index + "' name='spanButton" + this.index + "' class='Tab' style='background-image:url(./Images/tab-select-mid.jpg)' onmouseup='eval(\"" + this.containerName + "\").selectButton(" + this.index+ ")' nowrap>";
//		        sContent += " &nbsp;&nbsp;" + htmEncode(this.text) + "&nbsp;&nbsp;</td>";
//                sContent += "<td><img name='spanRight" + this.index + "' id='spanRight" + this.index + "' src='./Images/tab-select-right.jpg' />";
		        sContent = "<td id='spanButton" + this.index + "' name='spanButton" + this.index + "' class='Tab' style='height:23px;background-image:url(./Images/tab-unselect-mid.jpg);border-right: #9A7 solid 1px' onmouseup='eval(\"" + this.containerName + "\").selectButton(" + this.index+ ")' nowrap>";
		        sContent += " &nbsp;&nbsp;" + htmEncode(this.text) + "&nbsp;&nbsp; ";
            }
            
			sContent += "</td>"; 
			 
			return sContent;
		}

		function doSelect()
		{				
		    if (eval(this.containerName).mode == "picture"){			
			    //change pic below lines
			    eval("imgButton" + this.index).src = this.selPic;
			    eval("imgButton" + this.index).style.cursor = "default";
			} else {
			    eval("spanButton" + this.index).style.cursor = "default";
//			    eval("spanButton" + this.index).style.backgroundImage = "rgb(147,191,218)";
//			    eval("spanButton" + this.index).style.borderTop = "solid 1px paleturquoise";
//			    eval("spanButton" + this.index).style.borderLeft = "solid 1px paleturquoise";
//			    eval("spanButton" + this.index).style.borderRight = "outset 2px buttonshadow";

                //iMES need backgroung image
//			    document.getElementById("spanLeft" + this.index).src = "./Images/tab-select-left.jpg";
			    eval("spanButton" + this.index).style.backgroundImage = "url(./Images/tab-select-mid.jpg)";
//			    document.getElementById("spanRight" + this.index).src = "./Images/tab-select-right.jpg";
			}
		}

		function doUnSelect()
		{
		    if (eval(this.containerName).mode == "picture"){	
			    //change pic below lines
			    eval("imgButton" + this.index).src = this.normalPic;
			    eval("imgButton" + this.index).style.cursor = "hand";
			} else {
			    eval("spanButton" + this.index).style.cursor = "hand";
//			    eval("spanButton" + this.index).style.backgroundColor = "RGB(201,232,237)";
//			    eval("spanButton" + this.index).style.borderTop = "solid 1px RGB(205,222,235)";
//			    eval("spanButton" + this.index).style.borderLeft = "solid 1px rgb(147,191,218)";
//			    eval("spanButton" + this.index).style.borderRight = "solid 1px rgb(147,191,218)";

                //iMES need backgroung image
//			    document.getElementById("spanLeft" + this.index).src = "./Images/tab-unselect-left.jpg";
			    eval("spanButton" + this.index).style.backgroundImage = "url(./Images/tab-unselect-mid.jpg)";
//			    document.getElementById("spanRight" + this.index).src = "./Images/tab-unselect-right.jpg";
			}
		}
		
		//status:true/false
		function _changeStatus(status)
		{
		    if (eval(this.containerName).mode == "picture"){	
		        if (status == true){
		            eval("imgButton" + this.index).src = this.disablePic;
		            eval("imgButton" + this.index).style.cursor = "default";
		        }else{
		            eval("imgButton" + this.index).src = this.normalPic;
		            eval("imgButton" + this.index).style.cursor = "hand";
		        }
		    } else {
    		
    		
		    }
		}
		
        function htmEncode(s)
        {
        	
            if (s == null || s == ""){
    	        return "";
            } 
	        var strBuffer = "";
            var j = s.length;
            var i;
            for(i = 0; i < j; i++){
                var c = s.substring(i, i + 1);
                switch(c){
                    case '<': strBuffer += "&lt;" ; break;
                    case '>': strBuffer += "&gt;"; break;
                    case '&': strBuffer += "&amp;"; break;
                    case '\"': strBuffer += "&quot;"; break;
                    /*case 169: stringbuffer.append("&copy;"); break;
                    case 174: stringbuffer.append("&reg;"); break;
                    case 165: stringbuffer.append("&yen;"); break;
                    case 8364: stringbuffer.append("&euro;"); break;
                    case 8482: stringbuffer.append("&#153;"); break;*/
                    //	            case 32: stringbuffer.append("&nbsp;"); break;
                    default:
                        strBuffer += c;
                        break;
                }
            }
                return strBuffer;
        }		

/*
 var tabs;
 function createBtnTabs()
	{
		tabs = new clsButtonTabs("tabs");

		tabs.preSelectChanged = null;
		tabs.selectChanged = null;
	    tabs.container = con;
	    tabs.mode = "text";
		
		for (var i=0; i<10; i++)
		{
			var temp = new clsButton("tabs");
//			temp.normalPic = "../../images/fields"+ i +"-1.jpg";
//			temp.selPic = "../../images/fields"+ i +"-2.jpg";
//			temp.disablePic = "../../images/fields"+ i +"-3.jpg";
			temp.text = "dd" + i;
			tabs.addButton(temp);
		}
		// tabs.diableTab(0,true);
		 
		tabs.toString();
		
		tabs.initSelect(0);
	}
	
	createBtnTabs()
*/
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
		//methods
			this.addButton = _addButton;
			this.initSelect = _initSelect;
			this.selectButton = _selectButton;
			this.selectChanged = null;
			this.preSelectChanged = null;
			this.toString = _buildTabs;
			this.diableTab = _diableTab;
			
		}
		
		function _diableTab(index, value)
		{
			this.disabledButtons[index] = value;		 
            this.buttons[index].setDisabled(value);
		}
		
		function _addButton(buttonCell)
		{
			buttonCell.index = this.buttons.length;
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
			
			this.buttons[index].select();
			this.selectedButton = this.buttons[index];	
			
			return true;			 
		}	
					
		function _selectButton(index)
		{			 
			if (this.disabledButtons[index] == true)
				return false;
				
			if (this.preSelectChanged != null)
			{
				if (!this.preSelectChanged(index))
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
			 
			var sContent = "<TABLE border=0 cellspacing=0 cellpadding=0  width='100%' ><TR>";
				
			var nBlankCount = 0;	 
			for(var i=0; i<this.buttons.length; i++)
			{				 
				sContent += this.buttons[i].buildHTML() ;
			}
			
			if (this.imagePath != "")
				sContent += "<td style=\"background-image: url('" + this.imagePath + "/tabsblank.jpg')\"></td></TR>";
			else
				sContent += "<td></td></TR>";
			sContent += '<TR><td colspan="' + (i+1) + '" style=\"background-color: rgb(147,191,218);height:5px;\"></td></TR></TABLE>'


			return sContent;			
		} 

		function clsButton(parentName)
		{
			if(typeof(parentName) != "string" || parentName == "")
				throw(new Error());
			this.containerName = parentName;

  			this.normalPic = "";
			this.selPic = "";
			this.disablePic = "";
			this.moveCursor = "";
			this.index;


			//method
			this.buildHTML = buildBody;
			this.select = doSelect;
			this.unSelect = doUnSelect;
			this.setDisabled = _changeStatus;
		}

		function buildBody()
		{
			var sContent = "<td  width='10' style='cursor:hand;'>";		 
			sContent += "<IMG name='imgButton" + this.index + "' id='imgButton" + this.index + "' SRC='" + this.normalPic + "' BORDER=0  ALT='' " + 
							"onmouseup='eval(\"" + this.containerName + "\").selectButton(" + this.index+ ")'>";
			sContent += "</td>"; 
			 
			return sContent;
		}

		function doSelect()
		{							
			//change pic below lines
			eval("imgButton" + this.index).src = this.selPic;
			eval("imgButton" + this.index).style.cursor = "default";
		}

		function doUnSelect()
		{
			//change pic below lines
			eval("imgButton" + this.index).src = this.normalPic;
			eval("imgButton" + this.index).style.cursor = "hand";
			
		}
		
		//status:true/false
		function _changeStatus(status)
		{
		    if (status == true){
		        eval("imgButton" + this.index).src = this.disablePic;
		        eval("imgButton" + this.index).style.cursor = "default";
		    }else{
		        eval("imgButton" + this.index).src = this.normalPic;
		        eval("imgButton" + this.index).style.cursor = "hand";
		    }
		}

/*		var tabs;
		function createBtnTabs()
		{
			tabs = new clsButtonTabs("tabs");
			tabs.selectChanged = callback;
			
			for (var i=0; i<5; i++)
			{
				var temp = new clsButton("tabs");
				temp.normalPic = "D:\private_info.jpg";
				temp.selPic = "D:\private_info_sel.jpg";

				tabs.addButton(temp);
			}
			
				dd.innerHTML = tabs.toString();
			 
			tabs.selectButton(0);
			

		}
		
		function callback(index)
		{
			alert("index = " + index );
		}
		
		createBtnTabs()
*/
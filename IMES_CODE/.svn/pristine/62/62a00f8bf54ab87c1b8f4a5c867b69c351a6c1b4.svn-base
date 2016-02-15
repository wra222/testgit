	function createCounter()
	{
		if(document.getElementById && document.createTextNode)
		{
			var inputs,i,cmax,cmin,c;

			inputs=document.getElementsByTagName('input');
			for(i=0;i<inputs.length;i++)
			{
				if(/.*?_ctr_-{0,1}\d+_-{0,1}\d+/.test(inputs[i].id))
				{
					
					cmax=inputs[i].id.match(/.*?_ctr_(-{0,1}\d+)_(-{0,1}\d+)/);
					cmin=cmax[1];
					cmax=cmax[2];
					c=parseInt(inputs[i].value, 10);
					if (isNaN(c) || c<cmin) {inputs[i].value=cmin;}
					if (c>cmax) {inputs[i].value=cmax;}
					inputs[i].value=parseInt(inputs[i].value, 10)
					inputs[i].onchange=function()
					{
						
						var cmax,cmin,c;
//						<bug>
//                            BUG NO:ITC-992-0048
//                            REASON:控件修改match规则
//                        </bug>
						cmax=this.id.match(/.*?_ctr_(-{0,1}\d+)_(-{0,1}\d+)/)
						cmin=cmax[1];
						cmax=cmax[2];
						c=parseInt(this.value, 10);
						if (isNaN(c) || c<cmin) {this.value=cmin;}
						if (c>cmax) {this.value=cmax;}
						this.value=parseInt(this.value, 10);
					}

					var upBtn = document.createElement('button');
					upBtn.innerHTML = "~";
					upBtn.style.fontFamily = "Wingdings 3";
					upBtn.style.fontSize = "5pt";
					upBtn.style.float = "left";
					upBtn.style.width = "20px";
					upBtn.style.height = "12px";
					upBtn.o = inputs[i];
					upBtn.onclick=function()
					{
					
						
						
						    if(parseInt(this.o.value)<cmax)
						    {
							    this.o.value++;
						    }
						
						return false;
					}

					var downBtn = document.createElement('button');
					downBtn.innerHTML = "&#128;";
					downBtn.style.fontFamily = "Wingdings 3";
					downBtn.style.fontSize = "5pt";
					downBtn.style.clear = "both";
					downBtn.style.width = "20px";
					downBtn.style.height = "12px";
					downBtn.o = inputs[i];
					downBtn.onclick=function()
					{
						if(parseInt(this.o.value)>cmin)
						{
							this.o.value--;
						}
						return false;
					}

					var btnSpan = document.createElement('span');
					btnSpan.style.width = "24px";
					btnSpan.appendChild(upBtn);
					btnSpan.appendChild(downBtn);
					inputs[i].parentNode.insertBefore(btnSpan, inputs[i].nextSibling);
				}
			}	
		}
	}
	function judgeInput(v)
	{
	   var pattern=/^\d+$/;

        var p = parseInt(v, 10);
        if (pattern.test(v) && p >= 0 && p <= 100 && (p + '') === v)
        {
	        return true;
        }
        else 
        {
	        return false;
        }
    }
	window.onload=createCounter;	

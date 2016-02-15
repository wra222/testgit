 
function TableCtrl()
{
	this.RowDelim = "\r\n";
	this.ColDelim = ",";
	this.TextQualifier = ":";
	this.width = "98.2%";  //800
	this.isBlank = true;
	this.combineRow = [];
	this.RecordSet = null;
	
	this.arrWidth = [];
	this.arrAlign=[];
	this.arrHeadInfo=[];
	this.arrDataInfo=[];
}

TableCtrl.prototype.initial = function()
{
	var rs="";//"name:20,password:20,memo:20,name:10,password:10,memo:20;w1,s2,aasdfasdf,w1,s2,aasdfasdf;w2,s2,2006-9-1,w2,s2,2006-9-1";
	if(this.RecordSet!=null)
		rs=this.RecordSet;
	
	var arr=rs.split(this.RowDelim);
	this.arrHeadInfo = arr[0].split(this.ColDelim);
	
	for(var index = 1; index < arr.length; index++){
		this.arrDataInfo[this.arrDataInfo.length] = arr[index];
	}	
}

TableCtrl.prototype.fillHead = function(tablewidthPara)
{
	var tablewidth = "";
	tablewidth = tablewidthPara;
	
    if(tablewidth == null ||tablewidth == "")
    {
      tablewidth = "98.2%";
    }
	var strHead="";
	for(var i=0;i<this.arrHeadInfo.length;i++){
		var tArr=this.arrHeadInfo[i].split(this.TextQualifier);
		if(tArr.length==2){
			this.arrWidth[this.arrWidth.length]=tArr[1];
			this.arrAlign[this.arrAlign.length] = "left";
			if(tArr[1]=="0"){
				continue;
			}
		}
		if(tArr.length==3){
			this.arrWidth[this.arrWidth.length]=tArr[1];
			this.arrAlign[this.arrAlign.length]=tArr[2];
		}

		strHead +="<TD bgcolor=#A0BEE2 style = 'font-weight:bold' align = 'middle' width="+this.arrWidth[i]+"% >"+tArr[0]+"</TD>";
	}
	
	//strHead +=""; //<TH bgcolor=#ff9900 width=17></TH>
	strHead ="<table id=HEADTRID width='"+tablewidth+"' style='HEIGHT: 14px;word-break:break-all; font-size: 12px' cellSpacing=1  bgcolor=#889B9F><tbody><tr>"+strHead+"</tr></tbody></table>";
	//strHead ="<tbody><tr>"+strHead+"</tr></tbody>";	
	//alert(strHead)
	return strHead;
}

TableCtrl.prototype.fillData = function()
{
	var strBody="";

	//tableRows = this.arrDataInfo.length - 1;
	for(var i=0;i<this.arrDataInfo.length;i++){

		strBody +="<tr bordercolor=Silver id='tr"+i+"'";
		var arr1=this.arrDataInfo[i].split(this.ColDelim);
		var strHead="";
		var tdH=0;
		var tablewidth = "100%"

		for(var j=0;j<this.arrHeadInfo.length;j++){
            if(this.arrWidth[j]==0){
            	strBody +=" TDH"+tdH+"="+arr1[j]+" ";
            	tdH +=1;
            	continue;
            }
            
            var rowspan = 1;
			if (this.combineRow.length > 0){
				var countCol = this.getCountCol(j);
				if(countCol != ""){
					rowspan = arr1[countCol];
				}
			}
           	//if(this.isColOutput(j) == false){
           	//	continue;
           //}
           	
            if(i % 2 == 1){
            	if(rowspan != 0){
	            	strHead +="<Td  gRow="+i+" rowspan=" + rowspan + " class='divPopWindow2' height=20 align="+this.arrAlign[j]+" width="+this.arrWidth[j]+"% >"+arr1[j]+"</Td>";
	            }
            }else{
            	if(rowspan != 0){
	            	strHead +="<Td  gRow="+i+" rowspan=" + rowspan + " class='divPopWindow1' height=20 align="+this.arrAlign[j]+" width="+this.arrWidth[j]+"% >"+arr1[j]+"</Td>";
	            }
            }
            
        }
        strBody +=" TDHS="+tdH+">";
        strBody +=strHead;
        
		strBody +="</tr>";
	}
	
	if(this.isBlank == true && this.arrDataInfo.length<17){
		for(var i=this.arrDataInfo.length;i<16;i++){
			strBody +="<tr >";
			var strHead="";
	        for(var j=0;j<this.arrHeadInfo.length;j++){
				if(this.arrWidth[j]==0){
	            	continue;
	            }else{
	            	if(i % 2 == 1){
	            		strHead +="<Td class='divPopWindow2'  height=20  width="+this.arrWidth[j]+"% >&nbsp;</Td>";
	            	}else{
	            		strHead +="<Td class='divPopWindow1'   height=20 width="+this.arrWidth[j]+"% >&nbsp;</Td>";
	            	}
	        	}
	        }
			strBody +=strHead;
			strBody +="</tr>";
		}
	}
	
	strHead ="<TABLE id=BODYTABLEID width='100%'  GHLRow=1 style=' HEIGHT:50px;word-break:break-all ' cellSpacing=1 bgcolor=#000000 cellPadding=0 border=0>"+strBody+"</table>";
	
	//BODYTABLEID.outerHTML=strHead;
	//alert(BODYTABLEID.outerHTML)
	
	return strHead;
}

//whether current column needs combined
TableCtrl.prototype.getCountCol = function (_col){
	var ret = "";
	
	for(var i = 0; i < this.combineRow.length; i++){
		var arr = this.combineRow[i].split(this.RowDelim);
		var cols = arr[1] + this.ColDelim;
		if (cols.indexOf(_col + this.ColDelim) >= 0){
			ret = arr[0];
			break;
		}
	}
	
	return ret;
}

//whether current column needs output
TableCtrl.prototype.isColOutput = function (_col){
	var ret = false;
	
	for(var i = 0; i < this.combineRow.length; i++){
		var valueCol = this.combineRow[i].split(this.RowDelim)[0];
		if (valueCol != _col){
			ret = true;
			break;
		}
	}
	
	return ret;
}

function mClickRadio(_obj)
{
	var pObject=_obj.parentElement;
	if(_obj.gRow==BODYTABLEID.GHLRow){
	}else{

		var cObject=eval("tr"+BODYTABLEID.GHLRow);
		var td=cObject.cells
		for(var i=0;i<td.length;i++){
			if(BODYTABLEID.GHLRow % 2 == 1){
				td[i].className="divPopWindow1";
			}else{
				td[i].className="divPopWindow2";
			}
		}

		BODYTABLEID.GHLRow=_obj.gRow;

		td=pObject.cells
		for(var i=0;i<td.length;i++){
			td[i].className="divHPopWindow";
		}
	}

	var num=parseInt(pObject.TDHS);
	var arrHideID=[];			
}
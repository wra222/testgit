<%@ Import Namespace="com.inventec.system" %>
<script type="text/javascript">


//dataTable
function setSelectedOption(obj,datasourceObj,datasourceObjType,bSpace,bAll,defaultId){

    if(obj==null)
    {
        return;
    }
    var dataObj=datasourceObj;
    var selectObj = obj;
    while (selectObj.childNodes.length>0){
    	selectObj.removeChild(selectObj.childNodes[0]);
    }

    var oNode;
    if(datasourceObjType=="datatable" && dataObj!=null){
        if(dataObj.Rows.length > 0){
            if(bSpace==true){
	        oNode = document.createElement("Option");
	        oNode.innerText = "";
	        oNode.value = "";
	        selectObj.appendChild(oNode);
            }
            if(bAll==true){
	        oNode = document.createElement("Option");
	        oNode.innerText = "All";
	        oNode.value ="\u0016";
	        selectObj.appendChild(oNode);
            }
            for (var i = 0; i < dataObj.Rows.length; i++)
            {
                var item=dataObj.Rows[i];
                oNode = document.createElement("Option");
                oNode.innerText = trimString(item.selectValue);
                oNode.value = trimString(item.selectId);
                if(defaultId != null && defaultId == oNode.value){
                    oNode.selected = true;
                }
                selectObj.appendChild(oNode);
            }
        }
    }else{//数组
        if(dataObj.length > 0){
            if(bSpace==true){
                oNode = document.createElement("Option");
                oNode.innerText = "";
                oNode.value = "";
                selectObj.appendChild(oNode);
            }
            if(bAll==true){
                oNode = document.createElement("Option");
                oNode.innerText = "All";
                oNode.value ="\u0016";
                selectObj.appendChild(oNode);
            }
            for(var i=0; i<dataObj.length; i++){
                oNode = document.createElement("Option");
                oNode.innerText = dataObj[i][1];
                oNode.value =dataObj[i][0];
                if(defaultId != null && defaultId == dataObj[i][0]){
                    oNode.selected = true;
                }
                selectObj.appendChild(oNode);
            }
        }
    }

}

function trimString(sStr){
  //by xmzh
  if(sStr==null) return "";
  return sStr.replace(/(^\s*)|(\s*$)/ig,"");
}

String.prototype.Trim = function() 
{ 
    return this.replace(/(^\s*)|(\s*$)/g, ""); 
} 

function setPos(tableName,tablePos,objArray,index){
    document.all(tableName+tablePos).innerHTML=render(objArray[index]);  
}

function getLayoutHTML(obj){
//span={{行，列，行个数，列个数},{}}=>行数列数以0开头
    var colNum; 
    var rowNum;
    var name;
    var widths=obj.widths;
    var heights=obj.heights;

    //var width=(obj.width?obj.width:"100%");
    //var height=(obj.height?obj.height:"100%");
    
    
    //取得布局行数、列数、合并情况、布局table的id
  
    rowNum=obj.rowNum;
    if(rowNum==null){
        alert("rowNum can't be none");
        return;
    }
    delete obj.rowNum;
   
    var span=obj.span;
    if(span!=null){
	    delete obj.span;
    }

    colNum=obj.colNum;
    if(colNum==null){
        alert("colNum can't be none");
        return;
    }
    delete obj.colNum;
        
    name=obj.name;
    if(name==null){
        alert("name can't be none");
        return;
    }
    delete obj.name;

    if(widths!=null && widths.length!=null && widths.length!= colNum){
        alert("widths set invalid");
        return;    
    
    }
 
    if(heights!=null && heights.length!=null && heights.length!= rowNum){
        alert("heights set invalid");
        return;    
    
    }   

    //其余的信息遍历出来，为表格设上这些属性  
    var appendStr = "";
    for (var item in obj) {
        var property = item;
        var value = obj[item];
        appendStr+= " " + property + "=" + value;
    }
    
    
    var sbody="";
    sbody+="<table id='"+name+"' ";
    /////此是为观察设上颜色，应该去掉//////////////////////////
    //sbody+="BORDER=1 BORDERCOLOR='#0080FF' bgcolor='gray' ";
    ///////////////////////////////
    sbody+=appendStr+">";

    //计算合并情况
    var element=new Array();
    for(var i=0;i<rowNum;i++){
        element[i]=new Array();
        for(var j=0;j<colNum;j++){
            element[i][j]=1;
        }
    }
    
    //element[row][col]中-1将是被去掉的单元格
    if(span!=null){
        for(var i=0;i<span.length;i++){
            var spanInfo=span[i].split(",");
            var row=parseInt(spanInfo[0]);
            var col=parseInt(spanInfo[1]);
            var rowCount=parseInt(spanInfo[2]);
            var colCount=parseInt(spanInfo[3]);
            for(var j=row;j<row+rowCount;j++){
                for(var k=col;k<col+colCount;k++){
                    element[j][k]=-1;
                }
            }
            element[row][col]=rowCount+","+colCount;
        }
    }
    
    for(var i=0;i<rowNum;i++){
    
        sbody+="<tr ";
        if(heights!=null )
        {
            sbody+="height="
       	    sbody+=heights[i]
        }
        sbody+=" >";
        for(var j=0;j<colNum;j++){
            if(element[i][j]==1){
                /////此是为观察设上颜色，应该去掉,更换为下面一行//////////////////////////
                //sbody+="<td border=1 BGCOLOR='yellow' BORDERCOLOR='gray' NOWRAP id='"+name+i+"-"+j+"'";
                sbody+="<td NOWRAP id='"+name+i+"-"+j+"'";
                if(widths!=null){
                	sbody+=" width="+widths[j];
            	}
                sbody+="></td>";
                
            }else if(element[i][j]==-1){
                continue;
            }else{
                var spanXY=element[i][j].split(",");
                //此是为观察设上颜色，应该去掉,更换为下面一行//////////////////////////
                //sbody+="<td border=1 BGCOLOR='yellow' BORDERCOLOR='gray' id='"+name+i+"-"+j+"' rowspan="+spanXY[0]+" colspan="+spanXY[1]+"></td>";
                sbody+="<td id='"+name+i+"-"+j+"' rowspan="+spanXY[0]+" colspan="+spanXY[1]+"></td>";
                
            }
        }
        sbody+="</tr>";
    }
    sbody+="</table>";
    return sbody;

}

//render的功能是根据传入对象信息的属性中的类型和属性信息，生成形成控件的HTML脚本。
function render(obj){
    var sbody="";
    var appendStr = "";
    var appendStyleStr = "";
    var baseName;
    var text;
    var type;
    var value;

    if(obj.baseName==null){
        alert("baseName can't be empty");
        return;
    }
    baseName=obj.baseName;
    text=obj.text?obj.text:"";
    type=obj.type?obj.type:"";
    value=obj.value?obj.value:"";

    for (var item in obj) {
        var property = item;
        var propertyValue = obj[item];
      if(property=="style"){
        appendStyleStr +=  propertyValue;
      }else{        
        appendStr+= " " + property + "='" + propertyValue+"'";
       }
       
    }

    switch (type){
        case "lable":
            sbody+="<label id='dLable" +baseName+"' nowrap "

            if(appendStr!=""){
                 sbody+=appendStr;
            }
            sbody+=">";
            if(text!="")
            sbody+= "&nbsp&nbsp"+text;
            var nocolon=obj.nocolon?obj.nocolon:"no";
            if(nocolon!="yes")
            {
                sbody+= ":";
            }
            sbody+= "</label>";
            break;
        case "textLable":
            sbody+="<INPUT id='dLable" +baseName+"' style='width:100%;"+ appendStyleStr +"' readOnly=true class=textLableClass"
            if(appendStr!=""){
                 sbody+=appendStr;
            }

            if(text!="")
            sbody+= " value='&nbsp&nbsp"+text+":'";
            sbody+=">";
            sbody+= "</INPUT>";
            break;
        case "text":
            sbody+="<INPUT id='d"+ baseName +"' "+
            "name='n"+baseName+"' style='width:100%;"+ appendStyleStr +"' "
            if(appendStr!=""){
                 sbody+=appendStr;
            }
            sbody+="></INPUT>";
            break;
        case "textArea":
            sbody+="<textarea id='d"+ baseName +"' "+ "name='d"+ baseName +"' ";
            if(appendStr!=""){
                sbody+=appendStr;
            }
            if(obj.maxlength!=null){
                sbody+=" onpropertychange=\"if(this.value.length>"+obj.maxlength+"){this.value=this.value.substr(0,"+obj.maxlength+")}\"  onKeyUp=\"if(this.value.length>"+obj.maxlength+"){this.value=this.value.substr(0,"+obj.maxlength+")}\"";
            }
            sbody+="></textarea>";
            break;
        case "select":
            sbody+="<select id='d"+baseName+"' onChange='onOptionChange()'";
            if(appendStr!=""){
                 sbody+=appendStr;
            }
            sbody+=" style='width:100%;size:1;"+ appendStyleStr +"'>";
            sbody+="</select>";
            break;
        case "date":
            sbody+=getCalenderHTML(baseName,null,null,appendStr,true);
            break;
        case "button":
            sbody+="<button id='d"+baseName+"' onClick='onButtonClick()'";
            if(appendStr!=""){
                 sbody+=appendStr;
            }
            sbody+=">"+text+"</button>";
            break;
        case "partTimeSelect":        
            var maxvalue=obj.maxvalue?obj.maxvalue:"23";
            var unittext=obj.unittext?obj.unittext:"";
            sbody+=getPartTimeSelect(baseName,maxvalue,unittext);
            break;
        case "radio":
            var items=obj.items;
            sbody+=getRadio(baseName,items);
            break;
        case "checkbox":
            var item=obj.item
            sbody+=getCheckBox(baseName,item);
            break;
    }
    return sbody;
}

function getCheckBox(idName,item)
{
    var sbody="";
    sbody+="<INPUT "+
        "id='d"+idName+"' type=checkbox "
    sbody+=">"
    sbody+=item;
    sbody+="</INPUT>&nbsp&nbsp"; 
    return sbody;
}

function getCheckBoxValue(obj)
{
  if(obj==null)
  {
    return false;
  }
  return obj.checked;
}

function setCheckBoxValue(obj, value)
{
    if(obj==null)
    {
        return;
    }
    
    if(value==null)
    {
       obj.checked=false;
       return; 
    }

    if(value.toLowerCase()=="true"||value==1)
    {
        obj.checked=true;
    }
    else
    {
        obj.checked=false;
    }
}

function getRadio(idName,items)
{
  var sbody="";
  for(var i=0;i<items.length;i++)
  {
        sbody+="<INPUT "+
            "name='d"+idName+"' type=radio"
            sbody+=" value='"
            sbody+=items[i];
            sbody+="'";
        sbody+=">"
        sbody+=items[i];
        sbody+="</INPUT>&nbsp&nbsp&nbsp&nbsp";
        
  }
  return sbody;
}


function getRadioValue(obj)
{
  if(obj==null)
  {
    return false;
  }
  for(var i=0;i<obj.length;i++)   
  {
      if(obj[i].checked)   
      {
         return obj[i].value;
      }
  }   
  return false;
}

function setRadioValue(obj,value)
{
  if(obj==null)
  {
    return;
  }
  for(var i=0;i<obj.length;i++) 
  {  
      if(obj[i].value==value)   
      {
         obj[i].checked=true;
      }   
  }
  
}


function getCalenderHTML(idName, defaultValue, isObjectIntable, appenStr, isInMap){

    var sbody = "";
    sbody+="<table style='margin-left:-3'><tr ><td WIDTH=100%><INPUT id='d"+ idName+"'";
    if(isObjectIntable==true){
        sbody+=" style='HEIGHT: 19px; WIDTH: 80px;ime-mode:disabled'";
    }else{
        sbody+=" style='HEIGHT:19px; WIDTH:100%;ime-mode:disabled'";
    }
    if(defaultValue!=null && defaultValue!=""){
        sbody+=" value ='"+defaultValue+"' ";
    }
    if(appenStr!=null && appenStr!=""){
        sbody+=appenStr;
    }
    sbody+=" ondrop='return false;' onpaste='return false;' onPropertyChange=onCalChange() onkeydown='onDateKeyDown()'></INPUT>";
    sbody+="</td><td ><button id='dBtnCal"+idName;
    sbody+="' style='HEIGHT: 17px; WIDTH: 17px' onclick=onCalClick('"+"d"+ idName+"')> ";         //showCalFrame
    sbody+="<IMG src='../../images/rl.gif' /></button></td></tr></table>";

    return sbody;
}

function getPartTimeSelectValue(objName)
{
    if(document.all(objName) == null){
        alert(objName);
        return "";
    }
    if (document.all(objName).selectedIndex<0){
        return "";
    }
    return trimString(document.all(objName).options(document.all(objName).selectedIndex).value);
}

function getPartTimeSelect(idName,maxvalue,unittext)
{
    var sbody = "";
    sbody+="<table border='0' cellpadding='0' cellspacing='0' ><tr><td style='width:20%;'><select id='d";
    sbody+=idName;
    sbody+="' ";
    sbody+=" style='width:44px'>";
    
    var maxvalueNum=parseInt(maxvalue);
    for(var i=0;i<maxvalue;i++)
    {    
        var ivalue=GetFormat(i);
        sbody+="<option value='";
        sbody+=ivalue;
        sbody+="'>";
        sbody+=ivalue;
        sbody+="</option>";
    }
    
    sbody+="</select></td><td style='width:100%;'>"
    sbody+=unittext;
    sbody+="</td></tr></table>";    
    return sbody;
}

function GetFormat(num)
{
    var value=num.toString();
    if(value.length<2)
    {
        value ="0"+value;
    } 
    return value;
}


function getSelectValue(obj,flag)
{
    if(obj == null){
        return "";
    }
    if (obj.selectedIndex<0){
        return "";
    }

    if (flag=="value"){
       return trimString(obj.options(obj.selectedIndex).value);
    }
    else if (flag=="text"){
       return trimString(obj.options(obj.selectedIndex).text);
    }
    return "";
}

function getObjectValue(obj)
{
    if(obj == null){
        return "";
    }
    return obj.value;
}


function focusObject(obj){
    if(obj!=null && obj.disabled!=true){
        obj.focus();
    }
}

function htmlEncode(str)
{
    var strResult=str;
    strResult = strResult.replace(/&/g,"&amp;")
	strResult = strResult.replace(/</g, "&lt;")
    strResult = strResult.replace(/>/g, "&gt;")
    strResult = strResult.replace(/  /g, " &nbsp;")
    strResult = strResult.replace(/"/g, "&quot;")
    return strResult;
}

</script>
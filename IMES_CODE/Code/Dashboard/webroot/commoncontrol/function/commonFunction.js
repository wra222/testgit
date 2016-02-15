/*=========================
 *Description: Show waiting.
 *Author:itc205091
 *Date: 2008/4/9
 */
var gDisabledSelectIds = ""; //used by function ShowWait and HideWait.
var gIdDelim = String.fromCharCode(1);
function ShowWait(){
    try
    {
	    divWait.style.display = "block";
    	
	    gDisabledSelectIds = "";
	    var objArr = document.getElementsByTagName("select");
    	
	    for (var i = 0; i < objArr.length; i++){
		    var obj = objArr[i];
		    if (obj.disabled){
			    gDisabledSelectIds = obj.id + gIdDelim;
		    } else{
			    obj.disabled = true;	
		    }
	    }
    	
	    if (gDisabledSelectIds.length > 0){
		    gDisabledSelectIds = gIdDelim + gDisabledSelectIds;
	    }
	} catch (e){}
}

/*=========================
 *Description: Hide waiting.
 *Author:itc205091
 *Date: 2008/4/9
 */
function HideWait(){
    try
    {
	    var objArr = document.getElementsByTagName("select");
	    for (var i = 0; i < objArr.length; i++){
		    var obj = objArr[i];
		    if (gDisabledSelectIds.indexOf(gIdDelim + obj.id + gIdDelim) == -1){
			    obj.disabled = false;	
		    }
	    }
    	    divWait.style.display = "none";
    	
    } catch (e){}    	
}

//-----------------------------------
//�� ��SetCookie
//�� �ܣ�����Cookie��ֵ
//�� ��sName --Cookie�����֣�sValue--Cookie��ֵ
//�� �ߣ�wwg
//ʱ �䣺2004-9-16
//�� ע��
//�� �ģ�
//------------------------------------
function SetCookie(sName, sValue)
{
  document.cookie = sName + "=" + escape(sValue) + ";path=/;expires=Mon, 31 Dec 9999 23:59:59 UTC;";
}
//-----------------------------------
//�� ��GetCookie
//�� �ܣ��õ�Cookie��ֵ
//�� ��sName --Cookie������
//�� �ߣ�wwg
//ʱ �䣺2004-9-16
//�� ע��
//�� �ģ�
//------------------------------------
function GetCookie(sName)
{
  // cookies are separated by semicolons
  var aCookie = document.cookie.split("; ");
  for (var i=0; i < aCookie.length; i++)
  {
    // a name/value pair (a crumb) is separated by an equal sign
    var aCrumb = aCookie[i].split("=");
    if (sName == aCrumb[0])
    {
	 	 if (aCrumb.length > 1)
   			return unescape(aCrumb[1]);
  		else
   			return null;
    }
  }

  // a cookie with the requested name does not exist
  return null;
}

with (window.document) {
  //oncontextmenu=onselectstart=ondragstart=function(){
	oncontextmenu=ondragstart=function(){
		event.cancelBubble=true;
		/*if (event.srcElement.tagName != undefined) {
			 var sSrcTagName=event.srcElement.tagName.toLowerCase();
			 if (sSrcTagName=="textarea" || sSrcTagName=="input"||sSrcTagName=="editbox") return true;
       		 }*/
        	return false;
   	}
	
	onkeydown=function() {

		var charCode = event.keyCode;

		var iKeyCode=event.keyCode;
        if (iKeyCode==27 || iKeyCode==112 || (iKeyCode==78 && event.ctrlKey) || (iKeyCode==37 && event.altKey) || (iKeyCode==39 && event.altKey)) return false;

        if (iKeyCode>112 && iKeyCode < 124){
            event.keyCode = 0;
            event.cancelBubble=true;
            event.returnValue =false
            return false
        }
        if (iKeyCode == 8) {
            if (event.srcElement.tagName != undefined) {
                var sSrcTagName = event.srcElement.tagName.toLowerCase();
                if (sSrcTagName == "textarea" || sSrcTagName == "input" || event.srcElement.contentEditable) {
                    event.cancelBubble = false;
                    return true
                } else { 
                    event.cancelBubble = true;
                    return false
                }

            }
        }
    }
}

function hideWaitingTip()
{
    if ( typeof(divWait) == "object" ) divWait.style.display = "none";
    for ( i = 0; i<window.document.all.tags("select").length; i++ )
    {
        var oElement = 	window.document.all.tags("select").item(i);
        if(divWait.oSelectID.indexOf(oElement.id)!=-1)
            oElement.disabled=false;
    }
}

/**
 *add day.
 */
Date.prototype.addDays = function (day){
	var addm = 24 * 60 * 60 * 1000 * day;
	var m = this.getTime() + addm;
	return new Date(m);
}

/**
 *�����?�б��?
 *@param selId: select�ؼ�id��
 *@param jsonObjArr�����Դ��json�������顣
 *@param hasNullOption: �Ƿ����һ���ѡ�
 *@param defaultValue��Ĭ��ֵ��
 */
 function fillSelect(selId, jsonObjArr, hasNullOption, defaultValue){
	var objSel = document.getElementById(selId);
	objSel.innerHTML = "";
	
	if (hasNullOption != undefined && hasNullOption){
		var nullOption = document.createElement("option");
		nullOption.value = "";
		nullOption.text =  "";
		objSel.add(nullOption);
	}
	
	for (var i = 0; i < jsonObjArr.length; i++){
		var option = document.createElement("option");
		var jsonObj = jsonObjArr[i];
		option.value = jsonObj["id"];
		option.text =  jsonObj["name"];
		objSel.add(option);
	}
	
	if (defaultValue != undefined){
		objSel.value = defaultValue;
	}
}

/**
 *�����?�б��ÿ��option��һ��textֵ��text2ֵ��text2Ϊ�������ԡ�
 *@param selId: select�ؼ�id��
 *@param jsonObjArr�����Դ��json�������顣
 *@param hasNullOption: �Ƿ����һ���ѡ�
 *@param defaultValue��Ĭ��ֵ��
 */
 function fillSpecialSelect(selId, jsonObjArr, hasNullOption, defaultValue){
	var objSel = document.getElementById(selId);
	objSel.innerHTML = "";

	if (hasNullOption != undefined && hasNullOption){
		var nullOption = document.createElement("option");
		nullOption.value = "";
		nullOption.text =  "";
		nullOption.value2 =  "";
		objSel.add(nullOption);
	}

	for (var i = 0; i < jsonObjArr.length; i++){
		var option = document.createElement("option");
		var jsonObj = jsonObjArr[i];
		option.value = jsonObj["id"];
		option.text =  jsonObj["name"];
		option.value2 =  jsonObj["hide"];
		objSel.add(option);
	}

	if (defaultValue != undefined){
		objSel.value = defaultValue;
	}
}
/**
 **@param parBtnID: search button id
 * @param objInput: this
 * use for click enter to find result
 *  */
function onPressEnter(parBtnID , objInput)
{
	if (window.event.keyCode == 13)
	{
		eval(parBtnID).click();
		eval(parBtnID).focus();

    }
}

function isLaterDate(d){
    try{
        var now = new Date();
        var dateArr = d.split("-");
        var arrLen = dateArr.length;

        now.setFullYear(now.getYear(), now.getMonth(), now.getDate());

        var checkDate = new Date();
        checkDate.setFullYear(dateArr[0], dateArr[1]-1, dateArr[2]);

        var nowTime = now.getTime();
        var checkTime = checkDate.getTime();

        if(nowTime < checkTime){
            return true;
        }else{
            return false;
        }
    }catch(e){return false;}
}

/**
 * 检测日期的间隔时间
 * @param begin: YYYY-MM-DD
 * @param end: YYYY-MM-DD
 */
function checkDateDiff(begin, end){

    if( begin == "" || end==""){
        return "";
    }
    var beginDateArray = new Array();
    var endDateArray = new Array();
    beginDateArray   = begin.split("-");
    endDateArray     = end.split("-");

    var beginDate = new  Date(beginDateArray[0],parseInt(beginDateArray[1].replace(/^0/,""),10) ,beginDateArray[2]);
    var endDate   = new  Date(endDateArray[0],parseInt(endDateArray[1].replace(/^0/,""),10), endDateArray[2]);

    var thesecond = 24 * 60 * 60 *1000 ;
    var diffTime  = (beginDate - endDate)/thesecond;

    return diffTime;
}

/**
    ' ======== ============ =============================
    ' Description: create recordset for tablecontrol
    '              tableinfo is a DataTable
    ' Author: itc204033
    ' Side Effects:
    ' Date:2008-11-18
    ' ======== ============ =============================
*/       
function createRecordSet(tableInfo) {

    if (tableInfo != null && typeof(tableInfo) == "object")
    {
        var arrName = new Array();
        var rsSite = new ActiveXObject("ADODB.Recordset");

        for (var i = 0; i < tableInfo.Columns.length; i++)
        {
            arrName[i] = tableInfo.Columns[i].Name;
            //alert(arrName[i]+" "+tableInfo.Columns[i].__type);
            if (tableInfo.Columns[i].__type == "System.Int32" || tableInfo.Columns[i].__type == "System.Int16")
            {
                var arrFieldName = arrName[i].split(":");
                if (arrFieldName[1] && arrFieldName[1].toLowerCase() == "int"){
                    rsSite.Fields.Append(arrFieldName[0], 3, 16);
                }else{
                    rsSite.Fields.Append(arrName[i], 3, 16);
                }
                
            }
            else if (tableInfo.Columns[i].__type == "System.Char")
            {
                rsSite.Fields.Append(arrName[i], 5, 32);
            }
            else if (tableInfo.Columns[i].__type == "System.DateTime")
            {
                rsSite.Fields.Append(arrName[i], 202,500);
            }            
            else
            {
                rsSite.Fields.Append(arrName[i], 202, 500);
            }
        }

        rsSite.Open();

        for (var i = 0; i < tableInfo.Rows.length; i++)
        {
            rsSite.AddNew();

            for (var col = 0; col < tableInfo.Columns.length; col++)
            {

                try{
                    if (tableInfo.Columns[col].__type == "System.DateTime")
                    {
                        rsSite.Fields(col) = DateToString(tableInfo.Rows[i][arrName[col]]) ;
                    } else{
                        rsSite.Fields(col) = tableInfo.Rows[i][arrName[col]];
                    }
					     
				}
				catch(e)
				{
					try{
					  rsSite.Fields(col) = "";
					}
					catch(e)
					{
					}
				}
            }

            rsSite.Update();
        }

        return rsSite;
    }
}  


/*

 * The Interface1's description：change the key value of dictionary object gotten by ajaxPro
 * Parameters: 
 *      obj: dictionary object
 *      k: key
 *      v: value
 * Return Value：the index of key that in dictionary object.keys
 *      
 * Remark: 
 *      NULL
 * Output 
 *      NULL 
 */
 function setKeyValueOfDic(obj, k, v) {
    for(var i=0; i<obj.keys.length && i<obj.values.length; i++) {
	    if(obj.keys[i] == k){ obj.values[i] = v; return i;}
	    
    }
    return obj.add(k, v);
}

/**
    ' ======== ============ =============================
    ' Description: delete left and right space in string
    ' Author: itc204033
    ' Side Effects:
    ' Date:2008-11-25
    ' ======== ============ =============================
*/ 
function trimString(sStr){
  return sStr.replace(/(^\s*)|(\s*$)/ig,"");
}
/*

 * The Interface1's description：fill select menu when the data format is dataTable 
 * Parameters: 
 *      selId: select id
 *      dataTbValue: dataTable value
 *      hasNullOption: if true ,init state the first option is null
        defaultValue: the default value that you set
 * Return Value�?
 *      
 * Remark: 
 *      NULL
 * Output 
 *      NULL 
 */
function fillSelectForDatatb(selId, dataTbValue, hasNullOption, defaultValue)
	{
        var objSel = document.getElementById(selId);
        var arr = new Array();
        var dbServerValue;
        var arrName = new Array();
        objSel.innerHTML = "";

        if (hasNullOption != undefined && hasNullOption){
            var nullOption = document.createElement("option");
            nullOption.value = "";
            nullOption.text =  "";
            objSel.add(nullOption);
        }
        for(var i = 0; i < dataTbValue.Rows.length; i++)
        {
   	        var option = document.createElement("option");
            //arrName[j] = dataTbValue.Columns[i].Name;
            option.value = dataTbValue.Rows[i]["id"];
            option.text =  dataTbValue.Rows[i]["name"];
    
            objSel.add(option);
        }           
        if (defaultValue != undefined)
        {
            objSel.value = defaultValue;
        }	
}

/**
    ' ======== ============ =============================
    ' Description: fill select with datatable and set the selected value
    ' Author: itc204033
    ' selId: select id
    ' dataTbValue: dataTable value
    ' hasNullOption: if true ,init state the first option is null
    ' selectedValueArry: selected value array
    ' Date:2009-7-31
    ' ======== ============ =============================
*/  
function fillSelectWithSelectedValue(selId, dataTbValue, hasNullOption, selectedValueArry)
	{
        var objSel = document.getElementById(selId);
        var arr = new Array();
        var dbServerValue;
        var arrName = new Array();
        objSel.innerHTML = "";

        if (hasNullOption != undefined && hasNullOption){
            var nullOption = document.createElement("option");
            nullOption.value = "";
            nullOption.text =  "";
            objSel.add(nullOption);
        }
        for(var i = 0; i < dataTbValue.Rows.length; i++)
        {
   	        var option = document.createElement("option");
            //arrName[j] = dataTbValue.Columns[i].Name;
            option.value = dataTbValue.Rows[i]["id"];
            option.text =  dataTbValue.Rows[i]["name"];
            
            for(var j = 0; j < selectedValueArry.length; j++)
            {
                if(option.value == selectedValueArry[j])
                {
                    option.selected=true;
                    break;
                }
            }
            objSel.add(option);
        }           
	
}

function toJSON(object, property) {
    var type = typeof object;
    switch (type) {
      case 'undefined':
      case 'function':
      case 'unknown': return;
      case 'string': 
         if (property != undefined && property != ""){
            if (isBoolProperty(property)){
                 var _obj = object.toLowerCase();
                 if (_obj == "true"){
                    return true;
                 } else if (_obj == "false"){
                    return false;
                 } 
            }
         } 
         
         object = object.replace(/\\/ig, '\\\\');
         object = object.replace(/"/ig, '\\"');
         return ("\"" + object + "\"");
      case 'number': return object;
      case 'boolean': return object;
    }

    if (object === null) return 'null';
    
    var results = [];
    
    if (isArray(object)){
        for (var i = 0; i < object.length; i++){
            results.push(toJSON(object[i]));
        }
        return '[' + results.join(',') + ']';
    } else {
        for (var property in object) {
          var value = toJSON(object[property], property);
          if (value != undefined){
              results.push(toJSON(property) + ':' + value);
           } 
        }
        return '{' + results.join(',') + '}';
   }
}

function isBoolProperty(pro){
    var isBool = false;
    switch (pro){
        case "LeftEnableEmpty":
        case "RightEnableEmpty":
        case "BSumOneByOne":
        case "Combine":
        case "IsEmpty":
        case "IsConditionHide":
        case "IsDimDateTimeType":
        case "Bold":
        case "IsRunTimeValue":
        case "HasCategoryName":
        case "HasValue":
        case "HasSeriesName":
        case "HasTitle":
        case "HasLegend":
        case "HasDataValues":
        case "HasDatalabel":
        case "HasMajorGridlines":
        case "HasMinorGridlines":
        case "HasLabels":
        case "Visible":
         case "IsVisible":
            isBool = true;
            break;
    }
   
    return isBool;
}

function isArray(object) {
    var rtn = (object != null) && (typeof object == "object") && ( object.length != undefined) ;
    return rtn;
}

/**
    ' ======== ============ =============================
    ' Description: 将日期型数据转换为YYYY-MM-DD格式
    ' Author: itc204033
    ' Side Effects: 由于datatable把日期型的数据转换为英文格式的时�?因此生成表格时调用此方法
    ' Date:2008-12-25
    ' ======== ============ =============================
*/  
function DateToString(data){
    var s = "";
    if(null != data && data != ""){
        var date = new Date(data);
        var m = date.getMonth() + 1;
        m = AddZero(m);   
        var d = date.getDate();
        d = AddZero(d);
        s = date.getYear() + "-"; 
        s += m + "-";    
        s += d + " ";
        s += AddZero(date.getHours()) + ":";
        s += AddZero(date.getMinutes()) + ":";
        s += AddZero(date.getSeconds()) ;
    }
    return s;
}

//use for DateToString
function AddZero(str){
    str = "0" + str;
    str = str.substring(str.length-2, str.length); 
    return str;
}

//owner : 205111
function htmDecodeString(str)
{
	if (str == null || str == ""){
    	return "";
    } 
    
    str = str.replace(/&lt;/ig, "<");
    str = str.replace(/&gt;/ig, ">");
    str = str.replace(/&amp;/ig, "&");
    str = str.replace(/&quot;/ig, "\"");
    
    return str;
}

//owner : 205111
function htmEncodeString(s)
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

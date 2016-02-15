    var mm_per_inch = 25.4;//25.4mm/inch


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	getWidthPerColumn
    //| Author		:	itc98079
    //| Create Date	:	6/2009
    //| Description	:	Ä£°å¿í¶È£¬°´ÕÕÁÐÊý¾ù·Ö£¬µÃµ½ÁÐµÄ¿í¶È£¨ÏòÏÂÈ¡Õû£©
    //| Input para.	:	template_width£ºÄ£°å¿í¶È£¨px£©£»num_Column£ºÁÐÊý
    //| Ret value	:	ÁÐµÄ¿í¶È
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function getWidthPerColumn(template_width, num_Column){
        return Math.floor(parseFloat(template_width)*100/num_Column)/100;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	mmToPixel_x
    //| Author		:	itc98079
    //| Create Date	:	6/2009
    //| Description	:	¿í¶È·½Ïòmm×ªpixel
    //| Input para.	:	numMm
    //| Ret value	:	¶ÔÓ¦µÄpixel
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function mmToPixel_x(numMm){
        return Math.floor(parseFloat(numMm)*pixel_per_inch_x/mm_per_inch);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	mmToPixel_y
    //| Author		:	itc98079
    //| Create Date	:	6/2009
    //| Description	:	¸ß¶È·½Ïòmm×ªpixel
    //| Input para.	:	numMm
    //| Ret value	:	¶ÔÓ¦µÄpixel
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function mmToPixel_y(numMm){
        return Math.floor(parseFloat(numMm)*pixel_per_inch_y/mm_per_inch);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	pixelToMm_x
    //| Author		:	itc98079
    //| Create Date	:	6/2009
    //| Description	:	¿í¶È·½Ïòpixel×ªmm
    //| Input para.	:	numPixel
    //| Ret value	:	¶ÔÓ¦µÄmm
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function pixelToMm_x(numPixel){
        return Math.ceil(parseFloat(numPixel)*mm_per_inch*10/pixel_per_inch_x)/10;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	pixelToMm_y
    //| Author		:	itc98079
    //| Create Date	:	6/2009
    //| Description	:	¸ß¶È·½Ïòpixel×ªmm
    //| Input para.	:	numPixel
    //| Ret value	:	¶ÔÓ¦µÄmm
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function pixelToMm_y(numPixel){
        return Math.ceil(parseFloat(numPixel)*mm_per_inch*10/pixel_per_inch_y)/10;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	pixelToMm_XY_X
    //| Author		:	itc98079
    //| Create Date	:	6/2009
    //| Description	:	X×ø±êpixel×ªmm
    //| Input para.	:	numPixel
    //| Ret value	:	¶ÔÓ¦µÄmm
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function pixelToMm_XY_X(numPixel){
        return Math.ceil(parseFloat(numPixel)*mm_per_inch*10/pixel_per_inch_x)/10;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	pixelToMm_XY_Y
    //| Author		:	itc98079
    //| Create Date	:	6/2009
    //| Description	:	Y×ø±êpixel×ªmm
    //| Input para.	:	numPixel
    //| Ret value	:	¶ÔÓ¦µÄmm
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function pixelToMm_XY_Y(numPixel){
        return Math.ceil(parseFloat(numPixel)*mm_per_inch*10/pixel_per_inch_y)/10;
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	mmToPixel_XY_X
    //| Author		:	itc98079
    //| Create Date	:	6/2009
    //| Description	:	X×ø±êmm×ªpixel
    //| Input para.	:	numMM
    //| Ret value	:	¶ÔÓ¦µÄpixel
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function mmToPixel_XY_X(numMM){
        return Math.floor(parseFloat(numMM)*pixel_per_inch_x*10/mm_per_inch)/10
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	mmToPixel_XY_Y
    //| Author		:	itc98079
    //| Create Date	:	6/2009
    //| Description	:	Y×ø±êmm×ªpixel
    //| Input para.	:	numMM
    //| Ret value	:	¶ÔÓ¦µÄpixel
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    function mmToPixel_XY_Y(numMM){
        return Math.floor(parseFloat(numMM)*pixel_per_inch_y*10/mm_per_inch)/10;
        
    }

/*=========================
 *Description: Show waiting.
 *Author:itc205091
 *Date: 2008/4/9
 */
var gDisabledSelectIds = ""; //used by function ShowWait and HideWait.
var gIdDelim = String.fromCharCode(1);
function ShowWait(){
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
}

/*=========================
 *Description: Hide waiting.
 *Author:itc205091
 *Date: 2008/4/9
 */
function HideWait(){

	var objArr = document.getElementsByTagName("select");
	for (var i = 0; i < objArr.length; i++){
		var obj = objArr[i];
		if (gDisabledSelectIds.indexOf(gIdDelim + obj.id + gIdDelim) == -1){
			obj.disabled = false;	
		}
	}
    	divWait.style.display = "none";
}

//-----------------------------------
//ï¿½ï¿½ ï¿½ï¿½SetCookie
//ï¿½ï¿½ ï¿½Ü£ï¿½ï¿½ï¿½ï¿½ï¿½Cookieï¿½ï¿½Öµ
//ï¿½ï¿½ ï¿½ï¿½sName --Cookieï¿½ï¿½ï¿½ï¿½ï¿½Ö£ï¿½sValue--Cookieï¿½ï¿½Öµ
//ï¿½ï¿½ ï¿½ß£ï¿½wwg
//Ê± ï¿½ä£º2004-9-16
//ï¿½ï¿½ ×¢ï¿½ï¿½
//ï¿½ï¿½ ï¿½Ä£ï¿½
//------------------------------------
function SetCookie(sName, sValue)
{
  document.cookie = sName + "=" + escape(sValue) + ";path=/;expires=Mon, 31 Dec 9999 23:59:59 UTC;";
}
//-----------------------------------
//ï¿½ï¿½ ï¿½ï¿½GetCookie
//ï¿½ï¿½ ï¿½Ü£ï¿½ï¿½Ãµï¿½Cookieï¿½ï¿½Öµ
//ï¿½ï¿½ ï¿½ï¿½sName --Cookieï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
//ï¿½ï¿½ ï¿½ß£ï¿½wwg
//Ê± ï¿½ä£º2004-9-16
//ï¿½ï¿½ ×¢ï¿½ï¿½
//ï¿½ï¿½ ï¿½Ä£ï¿½
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
                if (sSrcTagName == "textarea" || sSrcTagName == "input" || sSrcTagName == "div") {
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
 *ï¿½ï¿½ï¿½ï¿½ï¿?ï¿½Ð±ï¿½ï¿?
 *@param selId: selectï¿½Ø¼ï¿½idï¿½ï¿½
 *@param jsonObjArrï¿½ï¿½ï¿½ï¿½ï¿½Ô´ï¿½ï¿½jsonï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½é¡£
 *@param hasNullOption: ï¿½Ç·ï¿½ï¿½ï¿½ï¿½Ò»ï¿½ï¿½ï¿½Ñ¡ï¿½î¡£
 *@param defaultValueï¿½ï¿½Ä¬ï¿½ï¿½Öµï¿½ï¿½
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
 *ï¿½ï¿½ï¿½ï¿½ï¿?ï¿½Ð±ï¿½ï¿½Ã¿ï¿½ï¿½optionï¿½ï¿½Ò»ï¿½ï¿½textÖµï¿½ï¿½text2Öµï¿½ï¿½text2Îªï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ô¡ï¿½
 *@param selId: selectï¿½Ø¼ï¿½idï¿½ï¿½
 *@param jsonObjArrï¿½ï¿½ï¿½ï¿½ï¿½Ô´ï¿½ï¿½jsonï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½é¡£
 *@param hasNullOption: ï¿½Ç·ï¿½ï¿½ï¿½ï¿½Ò»ï¿½ï¿½ï¿½Ñ¡ï¿½î¡£
 *@param defaultValueï¿½ï¿½Ä¬ï¿½ï¿½Öµï¿½ï¿½
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
 * æ£€æµ‹æ—¥æœŸçš„é—´éš”æ—¶é—´
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

 * The Interface1's descriptionï¼šchange the key value of dictionary object gotten by ajaxPro
 * Parameters: 
 *      obj: dictionary object
 *      k: key
 *      v: value
 * Return Valueï¼šthe index of key that in dictionary object.keys
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

 * The Interface1's descriptionï¼šfill select menu when the data format is dataTable 
 * Parameters: 
 *      selId: select id
 *      dataTbValue: dataTable value
 *      hasNullOption: if true ,init state the first option is null
        defaultValue: the default value that you set
 * Return Valueï¼?
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
    ' Description: å°†æ—¥æœŸåž‹æ•°æ®è½¬æ¢ä¸ºYYYY-MM-DDæ ¼å¼
    ' Author: itc204033
    ' Side Effects: ç”±äºŽdatatableæŠŠæ—¥æœŸåž‹çš„æ•°æ®è½¬æ¢ä¸ºè‹±æ–‡æ ¼å¼çš„æ—¶é—?å› æ­¤ç”Ÿæˆè¡¨æ ¼æ—¶è°ƒç”¨æ­¤æ–¹æ³•
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

//added by lucy liu

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	check2DecimalNotZero
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	¼ì²éÊÇ·ñÎªÕýÕûÊý»òÕß2Î»Ð¡Êý
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function check2DecimalNotZero(value)
{
    var ret = true;
    var checkDecimalDigits = /^(?:0|[1-9]\d*)(?:\.\d{1,3})?$/;
    ret = checkDecimalDigits.test(value);
    if (ret) {
        if (parseFloat(value) == 0) {
            ret = false;
        }
    }
    
    return ret;
   
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	check1DecimalNotZero
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	¼ì²éÊÇ·ñÎªÕýÕûÊý»òÕß1Î»Ð¡Êý
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function check1DecimalNotZero(value)
{
    var ret = true;
    var checkDecimalDigits = /^(?:0|[1-9]\d*)(?:\.\d{0,1})?$/;
    ret = checkDecimalDigits.test(value);
    if (ret) {
        if (parseFloat(value) == 0) {
            ret = false;
        }
    }
    return ret;
   
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkPosition
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	¼ì²é×ø±êÊÇ·ñÎª·Ç¸ºÊý»òÕß1Î»Ð¡Êý
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkPosition(value)
{
    var ret = true;
    var checkDecimalDigits = /^(?:0|[1-9]\d*)(?:\.\d{0,1})?$/;
    
    return checkDecimalDigits.test(value);
   
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkDigits
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	¼ì²éÊÇ·ñÎª·Ç0ÕûÊý
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkDigits(value)
{
    var ret = true;
    var checkDigits = /^[1-9]+$/;
    
    return checkDigits.test(value);
   
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	obj2str
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	½«¶ÔÏó×ª»»Îªstring
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 function obj2str(o)
{
    var r = [];
	//~~the ' should not be slashed 
    //if(typeof(o) =="string") return "\""+o.replace(/([\'\"\\])/g,"\\$1").replace(/(\n)/g,"\\n").replace(/(\r)/g,"\\r").replace(/(\t)/g,"\\t")+"\"";
    if(typeof(o) =="string") return "\""+o.replace(/([\"\\])/g,"\\$1").replace(/(\n)/g,"\\n").replace(/(\r)/g,"\\r").replace(/(\t)/g,"\\t")+"\"";
    
	if(typeof(o) == "object")
	{
        if(!o || !o.sort)
		{
            for(var i in o)
			{
                r.push('"'+i+'":'+obj2str(o[i]));
			}

            if(!!document.all && !/^\n?function\s*toString\(\)\s*\{\n?\s*\[native code\]\n?\s*\}\n?\s*$/.test(o.toString))
			{
                r.push("toString:"+o.toString.toString());
            }
            r="{"+r.join()+"}";
        }
		else
		{
            for(var i =0;i<o.length;i++)
			{
                r.push(obj2str(o[i]));
			}
            r="["+r.join()+"]";
        }
        return r;
    }
	else if (typeof(o) == "undefined")
	{
		o = "";
	}

    return o.toString();
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	check100Digits
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	¼ì²éÊÇ·ñÎª1-100µÄÕûÊý
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function check100Digits(value)
{
    var ret = true;
    var checkDigits = /^([1-9][0-9]?|100)$/;
    
    return checkDigits.test(value);
   
}

  function getTimer(previous) 
{
	if (typeof(previous) == 'undefined')
	{
		return (new Date()).getTime();
	}
	else 
	{
		return (new Date()).getTime() - previous;
	}
}
var fs=new ActiveXObject("Scripting.FileSystemObject"); 
function showTimer(header)
{
var d = new Date()
var vHour = d.getHours()
var vMin = d.getMinutes()
var vSec = d.getSeconds()
var vSec1 = d.getMilliseconds()

var file=fs.OpenTextFile("c:\\test.txt",8); 
file.WriteLine(header + "---" + vHour+":" + vMin+":" +vSec + ":" +vSec1); 
file.close();
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	judgeOutBound
//| Author		:	Lucy Liu
//| Create Date	:	9/23/2009
//| Description	:	ÅÐ¶Ï½çÃæÔªËØÊÇ·ñ³¬³öÄ£°å·¶Î§
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function judgeOutBound(objectType,areaHeight,convertAreaHeight,templateWidth,convertTemplateWidth,belongSecCell,judgeObject,fromStruc, objWidth, objHeight)
{

   
    var xErrorMsgFlag;
    var yErrorMsgFlag;
   
    var ret = new Object();
    ret.errorFlag = false;
    ret.prefixErr;
    ret.suffixErr;
    switch(objectType) {
        case 1:
            //text
            xErrorMsgFlag = 11;
            yErrorMsgFlag = 12;
            break;
        case 2:
            //barcode         
            xErrorMsgFlag = 21;
            yErrorMsgFlag = 22;
            break;
        case 3:
            //picture          
            xErrorMsgFlag = 31;
            yErrorMsgFlag = 32;
            break;
          case 4:
            //line          
            xErrorMsgFlag = 41;
            yErrorMsgFlag = 42;
            break;
          case 5:
            //area            
            xErrorMsgFlag = 51;
            yErrorMsgFlag = 52;
            break;
          case 6:
            //rectangle
            xErrorMsgFlag = 61;
            yErrorMsgFlag = 62;
            break; 
          default:
            break;
      }
    
    if (typeof(judgeObject.Width)  == "undefined") {
        judgeObject.Width = "0";
    }
     if (typeof(judgeObject.Height)  == "undefined") {
        judgeObject.Height = "0";
    }
    //Èç¹ûÊÇtext,judgeObjectÓÐAngle,X,Y,Width,Height
    //barcode,judgeObjectÓÐAngle,X,Y,Width,Height Width=0
    //pirture Height WidthÓÃpixelToMm_x×ªÒ»ÏÂµÃµ¥¶À³öÀ´
    //line µÃµ¥¶À³öÀ´ ÊÇthicknessºÍlength
    //area,rectangleµ¥¶À£¬ÒòÎªÃ»ÓÐ½Ç¶È£¬²»¹ý¿ÉÒÔ½«AngleÉè¶¨Îª0
    if (objectType == 3 && !fromStruc) {
        //picture
        if ((judgeObject.Angle == "0") || (judgeObject.Angle == "180")) { 
 
            if (parseFloat(judgeObject.X) + parseFloat(objWidth) > parseFloat(convertTemplateWidth)) {
                
                if (belongSecCell) {
                   ret.prefixErr =  xErrorMsgFlag;
                   ret.suffixErr = "(Section Cell Width:" + convertTemplateWidth + ")";
                  
                } else {
                    ret.prefixErr =  xErrorMsgFlag;
                    ret.suffixErr =  "(Template Width:" + templateWidth + ")";
                  
                }
                ret.errorFlag = true;
                
            } else if (parseFloat(judgeObject.Y)  + parseFloat(objHeight) > parseFloat(convertAreaHeight)) {
                
               
                ret.prefixErr =  yErrorMsgFlag;
                ret.suffixErr = "(Area Height:" + areaHeight + ")";
                ret.errorFlag = true;
                
               
            } 
        } else {
            if (parseFloat(judgeObject.X) + parseFloat(objWidth) > parseFloat(convertTemplateWidth)) {
                
                if (belongSecCell) {
                   ret.prefixErr =  xErrorMsgFlag;
                   ret.suffixErr = "(Section Cell Width:" + convertTemplateWidth + ")";
                   
                } else {
                   ret.prefixErr =  xErrorMsgFlag;
                   ret.suffixErr =  "(Template Width:" + templateWidth + ")";
                }
                ret.errorFlag = true;
                
              
            } else if (parseFloat(judgeObject.Y)  + parseFloat(objHeight) > parseFloat(convertAreaHeight)) {
                
                ret.prefixErr =  yErrorMsgFlag;
                ret.suffixErr = "(Area Height:" + areaHeight + ")";
                ret.errorFlag = true;
               
            } 
            
        }
    } else if (objectType == 4) {
         if ((judgeObject.Angle == "0") || (judgeObject.Angle == "180")) { 
            if ((parseFloat(judgeObject.X) + parseFloat(objWidth) > parseFloat(convertTemplateWidth)) ||
                (parseFloat(judgeObject.X) < 0 )) {
                if (belongSecCell) {
                   ret.prefixErr =  xErrorMsgFlag;
                   ret.suffixErr = "(Section Cell Width:" + convertTemplateWidth + ")";
                 
                } else {
                   ret.prefixErr =  xErrorMsgFlag;
                   ret.suffixErr = "(Template Width:" + templateWidth + ")";
                  
                }
                ret.errorFlag = true;
                
            } else if ((parseFloat(judgeObject.Y) + parseFloat(objHeight) > parseFloat(convertAreaHeight)) ||  
                      (parseFloat(judgeObject.Y) < 0)) { 
                ret.prefixErr =  yErrorMsgFlag;
                ret.suffixErr = "(Area Height:" + areaHeight + ")";
                ret.errorFlag = true;
            } 
        } else {
            if ((parseFloat(judgeObject.X) + parseFloat(objWidth) > parseFloat(convertTemplateWidth)) || 
                (parseFloat(judgeObject.X) < 0 )) {
                if (belongSecCell) {
                   ret.prefixErr =  xErrorMsgFlag;
                   ret.suffixErr = "(Section Cell Width:" + convertTemplateWidth + ")";
                  
                } else {
                   ret.prefixErr =  xErrorMsgFlag;
                   ret.suffixErr = "(Template Width:" + templateWidth + ")";
               
                }
                ret.errorFlag = true;
                
            } else if ((parseFloat(judgeObject.Y) + parseFloat(objHeight) > parseFloat(convertAreaHeight)) || 
                      (parseFloat(judgeObject.Y) < 0)) {
                ret.prefixErr =  yErrorMsgFlag;
                ret.suffixErr = "(Area Height:" + areaHeight + ")";
                ret.errorFlag = true;
            } 
        }
    }
    else {
    
        if ((judgeObject.Angle == "0") || (judgeObject.Angle == "180")) { 
            if ((parseFloat(judgeObject.X) + parseFloat(objWidth) > parseFloat(convertTemplateWidth)) ||
                (parseFloat(judgeObject.X) < 0)) {
                if (belongSecCell) {
                   ret.prefixErr =  xErrorMsgFlag;
                   ret.suffixErr = "(Section Cell Width:" + convertTemplateWidth + ")";
                 
                } else {
                   ret.prefixErr =  xErrorMsgFlag;
                   ret.suffixErr = "(Template Width:" + templateWidth + ")";
                  
                }
                ret.errorFlag = true;
                
            } else if ((parseFloat(judgeObject.Y) + parseFloat(objHeight) > parseFloat(convertAreaHeight)) ||
                      (parseFloat(judgeObject.Y) < 0)){
                
                ret.prefixErr =  yErrorMsgFlag;
                ret.suffixErr = "(Area Height:" + areaHeight + ")";
                ret.errorFlag = true;
            } 
        } else {
            if ((parseFloat(judgeObject.X) + parseFloat(objWidth) > parseFloat(convertTemplateWidth)) || 
               (parseFloat(judgeObject.X) < 0)) {
                if (belongSecCell) {
                   ret.prefixErr =  xErrorMsgFlag;
                   ret.suffixErr = "(Section Cell Width:" + convertTemplateWidth + ")";
                  
                } else {
                   ret.prefixErr =  xErrorMsgFlag;
                   ret.suffixErr = "(Template Width:" + templateWidth + ")";
               
                }
                ret.errorFlag = true;
                
            } else if ((parseFloat(judgeObject.Y) + parseFloat(objHeight) > parseFloat(convertAreaHeight)) ||  
                      (parseFloat(judgeObject.Y) < 0)) {
                ret.prefixErr =  yErrorMsgFlag;
                ret.suffixErr = "(Area Height:" + areaHeight + ")";
                ret.errorFlag = true;
            } 
        }
    }
   
       
   return ret;
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	searchObject
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	¸ù¾ÝobjectName(id)ÔÚprintTemplateInfo½á¹¹ÖÐÕÒµ½text¶ÔÏó
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function searchObject(id)
{
 
   

     var j;
     var key;
     var parentId;
     var parentObj;
     var rootObj;
     var rootId;
     var ret = new Object();
     ret.object = "";
     ret.areaHeight = "";
     ret.areaHeight = "";
     ret.convertAreaHeight = "";
     ret.templateWidth = "";
     ret.convertTemplateWidth = "";
     ret.belongSecCell = false;
     
     id = id.replace(new RegExp("_a","g"),"[");
     id = id.replace(new RegExp("_b","g"),"]");
     id = id.replace(new RegExp("_dot_","g"),".");
     ret.object = eval(id);
     
     rootId = id.substring(0,id.indexOf("."));
     rootObj = eval(rootId);
      
     ret.templateWidth = rootObj.TemplateWidth;
     convertTemplateWidth = pixelToMm_x(mmToPixel_x(rootObj.TemplateWidth));
     ret.convertTemplateWidth = convertTemplateWidth
     
     if (id.indexOf(".PageHeader") == id.indexOf(".")) {
         key = ".PageHeader";
         parentId = id.substring(0,id.indexOf(".PageHeader") + key.length);
         parentObj = eval(parentId);
         ret.areaHeight = parentObj.AreaHeight;
     } else if (id.indexOf(".PageFooter") == id.indexOf(".")) {
         key = ".PageFooter";
         parentId = id.substring(0,id.indexOf(".PageFooter") + key.length);
         parentObj = eval(parentId);
         ret.areaHeight = parentObj.AreaHeight;
     } else if (id.indexOf(".DetailSections") == id.indexOf(".")) {
        if (id.indexOf(".HeaderArea") == id.indexOf(".", 18)) {
            key = ".HeaderArea";
            parentId = id.substring(0,id.indexOf(".HeaderArea") );
            parentObj = eval(parentId);
            ret.areaHeight = parentObj.HeaderHeight;
        } else if (id.indexOf(".Cells") == id.indexOf(".", 18)) {
             i = id.indexOf(".Cells");
             parentId = id.substring(0,i);
             parentObj = eval(parentId);
             ret.areaHeight = parentObj.RowHeight;
             ret.convertTemplateWidth = pixelToMm_x(getWidthPerColumn(mmToPixel_x(rootObj.TemplateWidth), parentObj.ColumnNum))
             ret.templateWidth = Math.round(rootObj.TemplateWidth/parentObj.ColumnNum*100)/100;
             ret.belongSecCell = true;
             
           
             
        }
     }
   
     ret.convertAreaHeight = pixelToMm_y(mmToPixel_y(ret.areaHeight));
     
     return ret;



    
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	judgePropertyBound
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	¸ù¾ÝobjectName(id),ÅÐ¶ÏËüÊÇ·ñ³¬¹ýËùÔÚÇøÓò·¶Î§
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function judgePropertyBound(id, propertyType,fromStructure, objWidth, objHeight)
{
    var prop = searchObject(id);
    var ret = judgeOutBound(propertyType,prop.areaHeight,prop.convertAreaHeight,prop.templateWidth,prop.convertTemplateWidth,prop.belongSecCell,prop.object,true, objWidth, objHeight);
    return ret;
}


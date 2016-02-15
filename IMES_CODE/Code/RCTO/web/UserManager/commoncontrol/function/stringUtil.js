/*
common function add by Li Yisong
*/
function trimString(sStr){
  return sStr.replace(/(^\s*)|(\s*$)/ig,"");
}

/*
功能  ：将特殊HTML字符转化成&???字符。
作者  ：李翼嵩
时间  ：2001年8月
参数  ：key--待转化的字串
*/
function ChangeK(key)
{
   key = key + ""

   if (	this.UseHTML == true)
		return key;
   while(key.search("&")!=-1)
   	   key = key.replace("&", "@#$_-%$#|~~`")

   while(key.search("@#$_-%$#|~~`")!=-1)
   	   key = key.replace("@#$_-%$#|~~`","&amp;")

   while(key.search("<")!=-1)
		key = key.replace("<", "&lt;")

   while(key.search(">")!=-1)
   	   key = key.replace(">", "&gt;")

   while(key.search("  ")!=-1)
   	   key = key.replace("  ", " &nbsp;")

   while(key.search("\"")!=-1)
   	   key = key.replace("\"", "&quot;")

   return key;
}
//-----------------------------------
//函 数：isNumCharString
//功 能：检查某个字串是否是只含数字或字母的字符串
//参 数：strValue -- 被检测的字符串
//返 回：
//作 者：wwg
//时 间：2004-8-5
//备 注：
//修 改：
//------------------------------------
function isNumCharString(strValue)
{
   if(/[^a-zA-Z0-9]/.test(strValue))
	     return false;
	return true;
}
//-----------------------------------
//函 数：isRegulationLoginName
//功 能：检查某个字串是否是合法的注册或登陆名称字符串
//参 数：strValue -- 被检测的字符串
//返 回：
//作 者：wwg
//时 间：2004-8-5
//备 注：
//修 改：
//------------------------------------
function isRegulationLoginName(strValue)
{
   if(/[^a-zA-Z_0-9]/.test(strValue))
	     return false;
	return true;
}

function Encode_URL2(v_strSource)
{
    if( trimString(v_strSource) == "" )
        return "" ;

	var strMarkString = "#%&+', ?";
	var strChangeString = String.fromCharCode(21) + String.fromCharCode(22) + String.fromCharCode(23) + String.fromCharCode(24);
	strChangeString += String.fromCharCode(25) + String.fromCharCode(26) + String.fromCharCode(27) + String.fromCharCode(28);

	var strMark = "";
	var strChange = "";
	var intPosition = -1;

	var strTemp = v_strSource;
	for (intIndex = 0; intIndex < strMarkString.length; intIndex++)
	{
		strMark = strMarkString.substr(intIndex, 1);
		strChange = strChangeString.substr(intIndex, 1);

		intPosition = strTemp.indexOf(strMark);
		while(intPosition != -1)
		{
			strTemp = strTemp.replace(strMark, strChange);
			intPosition = strTemp.indexOf(strMark)
		}
	}
	return strTemp;
}

//-----------------------------------
//函 数：trimByStr
//功 能：删除字符串中含有的sStr字符串
//参 数：sStr -- 被检测的字符串
//返 回：
//作 者：wwg
//时 间：2004-10-27
//备 注：
//修 改：
//------------------------------------
function trimByStr(sStr,trimStr){
    return sStr.replace(trimStr,"");
}

function isNumber(source)
{
    var reExp = /^-?\d{1,6}.?\d{0,6}$/;
    if(reExp.exec(source))
        return true;
    else
        return false;
}

function RTrimStr(source)
{
  var num
  source=""+source;
  num=source.length;
  for(i=1;i<=source.length;i++)
      if(source.charAt(source.length-i)==" ")
          num--;
      else
          break;
  source=source.substring(0,num);

  return source;

}

function convertSpecialChar(source)
{
	if(source == "" || source == undefined){
		return "";
	}
	var dic = {"#":"%23", "&":"%26", "\"":"%22", "%":"%25", "+":"%2B", "\'":"%27", "<":"%3C", ">":"%3E"};
	var key;
	for (key in dic){
		if(source.indexOf(key) >= 0){
			//source = source.replace(/key/g, dic[key]);
			var ret = "";
			for (var i=0; i< source.length; i++){
				s = source.charAt(i);
				if (s == key){
					ret += dic[key];
				}else{
					ret += s;
				}
			}
			source = ret;
		}
	}
	return source;
}

/**
 *Decode String. char(01)-->"; char(02)-->'.
 */
function decodeString(strSource){
	var quot1 = String.fromCharCode(01);
	var quot2 = String.fromCharCode(02);
	while (strSource.indexOf(quot1) != -1 || strSource.indexOf(quot2) != -1){
			if (strSource.indexOf(quot1) != -1){
				strSource = strSource.replace(quot1, "\"");
			} else{
				strSource = strSource.replace(quot2, "\'");
			}
	}
	
	return strSource;
}

/**
 *Encode String. "-->char(01); '-->char(02).
 */
function encodeString(strSource){
	var quot1 = String.fromCharCode(01);
	var quot2 = String.fromCharCode(02);
	while (strSource.indexOf("\'") != -1 || strSource.indexOf("\"") != -1){
			if (strSource.indexOf("\"") != -1){
				strSource = strSource.replace("\"", quot1);
			} else{
				strSource = strSource.replace("\'", quot2);
			}
	}
	
	return strSource;
}

function replaceAllInfo(str,exp,replace)
{
      var pos;

		do {
			str = str.replace(exp,replace);
			pos = str.indexOf(exp, pos + 1);
		} while (pos >= 0);
		return str;
}

function spcharTest(chr) {
var i;
var spch="_-.0123456789";

for (i=0;i<13;i++) {
   if(chr==spch.charAt(i)) {
    return(1);
   }
}

return(0);
}

function charTest(chr) {
var i;
var smallch="abcdefghijklmnopqrstuvwxyz";
var bigch="ABCDEFGHIJKLMNOPQRSTUVWXYZ";

for(i=0;i<26;i++) {
  if(chr==smallch.charAt(i) || chr==bigch.charAt(i)) {
    return(1);
  }
}

return(0);
}

function emailTest(str) {
	var i,flag=0;
	var at_symbol=0;
	var dot_symbol=0;
	
	if(charTest(str.charAt(0))==0 ){
	  return false;
	}
	
	for (i=1;i<str.length;i++){
	  if(str.charAt(i)=='@') {
	    at_symbol=i;
	    break;
	  }
	}
	
	if(at_symbol==str.length - 1 || at_symbol==0){
	  return false;
	}
	
	if(at_symbol<3) {
	  return false;
	}
	
	if(at_symbol>19 ) {
	  return false;
	}
	
	for(i=1;i<at_symbol;i++) {
	  if(charTest(str.charAt(i))==0 && spcharTest(str.charAt(i))==0) {
	    return false;
	  }
	}
	
	for(i=at_symbol+1;i<str.length;i++) {
	  if(charTest(str.charAt(i))==0 && spcharTest(str.charAt(i))==0) {
	    return false;
	  }
	}
	
	for(i=at_symbol+1;i<str.length;i++) {
	  if(str.charAt(i)=='.') {
	      dot_symbol=i;
	  }
	}
	
	for(i=at_symbol+1;i<str.length;i++) {
	  if(dot_symbol==0 || dot_symbol==str.length-1) {
	  return false;
	  }
	
	}
	
	return true;
}

function replaceUnChar(strValue){
    strValue=trimString(strValue);
    for (var i=0; i<strValue.length; i++){
        if(/[^a-zA-Z_0-9]/.test(strValue.charAt(i)))
        {
                strValue = strValue.replace(strValue.charAt(i),"")
                i--;
        }
    }
    return strValue;
}

function replaceIllegalCharExcept(strValue){
    strValue=trimString(strValue);
    for (var i = 0; i < strValue.length; i++){
        if(/[\/?!^""<>]/.test(strValue.charAt(i)))
        {
            strValue = strValue.replace(strValue.charAt(i),"")
            i--;
        }
    }
    return strValue;
}

function replaceIllegalChar(strValue){
    strValue=trimString(strValue);
    for (var i = 0; i < strValue.length; i++){
        if(/[\/?!^''""<>]/.test(strValue.charAt(i)))
        {
            strValue = strValue.replace(strValue.charAt(i),"")
            i--;
        }
    }
    return strValue;
}
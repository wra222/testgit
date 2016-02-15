// ======== ============ =============================
// FileName:
// Copyright 2004 by ITC, ED2-4. All rights reserved.
// Author:������
// Date: 2004.8.4
// Description: �տ�򸶿�ʱ��������룬ֻ�����븺�š�0-9��С����
// Side Effects:
// Update:
// Date     Name         Description
// ======== ============ =============================
function ProcessKey()
{
    var arrInt=new Array(999,8,9,13,37,39,46,48,49,50,51,52,53,54,55,56,57,96,97,98,99,100,101,102,103,104,105,109,110,189,190,999);
    var charCode=(navigator.appName =="NetScape")?event.Which: event.keyCode;
    if ((arrInt.join("-").indexOf("-"+charCode+"-")!=-1))
        event.returnValue =true;
    else
        event.returnValue =false;
}

function DealPaste()
{
	var strPaste = window.clipboardData.getData("Text");

	if (isNaN(parseInt(strPaste)))
		event.returnValue = false;
}

function DealDrop()
{
	event.returnValue = false;
}


function DealFocus()
{

	event.srcElement.select();
}

function DealTextAreaChangeSlash()
{
	for (var i = 0; i<window.document.all.tags("TEXTAREA").length; i++ )
	{
		var oElement = 	window.document.all.tags("TEXTAREA").item(i);
		oElement.attachEvent("onblur",DealChangeSlash);
	}
}

function DealChangeSlash()
{
	var strChange = event.srcElement.value;

	for (var i=0; i<strChange.length; i++){
	    //alert(strChange)
        //alert(strChange.charCodeAt(i))
        if (strChange.charCodeAt(i) == 92)
	    {
        		strChange = strChange.replace(strChange.charAt(i),"")
		        i--;
	    }
    }

    event.srcElement.value = strChange;
}

function DealChange()
{
	var strChange = event.srcElement.value;

	for (var i=0; i<strChange.length; i++){
	    //alert(strChange)
        //alert(strChange.charCodeAt(i))
        if (strChange.charCodeAt(i) > 192 || strChange.charCodeAt(i) <33 || strChange.charCodeAt(i) == 92)
	    {
        		strChange = strChange.replace(strChange.charAt(i),"")
		        i--;
	    }
    }
	switch (event.srcElement.checkType)
	{
	case "int":

		if (isNaN(parseInt(strChange)))
			event.srcElement.value = "";
		else
			event.srcElement.value = parseInt(strChange);
        	break;

	case "zip":

		if (isNaN(parseInt(strChange)))
			event.srcElement.value = "";
		else
			event.srcElement.value = parseInt(strChange);
       	break;
    case "float":
		if (isNaN(parseFloat(strChange)))
			event.srcElement.value = "";
		else
			event.srcElement.value = parseFloat(strChange);
		break;
	default:
		event.srcElement.value = strChange;
        }

}

//��Ӧ���س��¼���
//�÷���������onPressEnter�ŵ������onkeypress�¼���
//������1)parBtnID:��Ҫ��Ӧ�İ�ť��id��name
//             2)objInput:Ҫȥ�������ַ���Input���󡣲���Ϊ��ȥ������ν�����ַ��ķ�Χ���replaceIllegalCharForSearch()
//added by LiuZhao
//updated on 2007-11-7
function onPressEnter(parBtnID , objInput)
{
	if (window.event.keyCode == 13)
	{
		if (objInput != undefined)
		{
			replaceIllegalCharForSearch(objInput);
		}
		eval(parBtnID + ".fireEvent('onclick');");
	}
}

/**
 * chinese char,0-9,a-Z are permit,others' character will be replace
 * itc204033
 * 2007-11-7
 * @param obj input
 */
function replaceIllegalCharForSearch(obj){
    var strValue=trimString(obj.value);
    for (var i=0; i<strValue.length; i++){
        if(/[\/?!@#$%&+=''&_~.:;`*|""<>^]/.test(strValue.charAt(i)))
        {
            strValue = strValue.replace(strValue.charAt(i),"")
            i--;
        }
    }
   obj.value=strValue;
}
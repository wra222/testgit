/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: validity check
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2009-01-20   itc204033    Create 
 * Known issues: 
 */
 
 /**
    ' ======== ============ =============================
    ' Description: 整数检查，可以为负数
    ' Author: itc204033
    ' Side Effects: 输入非整数将被置为0,(用于输入框的onblur或onfocusout事件中)
    ' Date:2009-01-20
    ' ======== ============ =============================
*/ 
function IntegerCheck(obj)
{
    var strChange = obj.value;
	var nQty = 0;
	var reExp =/^-?\d+$/;
	if (reExp.exec(strChange)){
		nQty = (new Number(strChange)).valueOf();
	}
//	else{
//	    alert ('Invalid input, please try agian!');
//	}
	obj.value = nQty;

}

 /**
    ' ======== ============ =============================
    ' Description: 整数或小数
    ' Author: itc204033
    ' Side Effects: (用于输入框的onblur或onfocusout事件中)
    ' Date:2009-01-20
    ' ======== ============ =============================
*/ 
function DecimalCheck(obj)
{
    var strChange = obj.value;
	var nQty = 0;
	var reExp = /^-?[0-9]+(\.[0-9]+)?$/;
	if (reExp.exec(strChange)){
		nQty =  (new Number(strChange)).valueOf();
	}
//	else{
//	    alert ('Invalid input, please try agian!');
//	}
	obj.value = nQty;

}

 /**
    ' ======== ============ =============================
    ' Description: 允许录入中文、英文、数字
    ' Author: itc204033
    ' Side Effects: (用于输入框的onblur或onfocusout事件中)
    ' Date:2009-01-20
    ' ======== ============ =============================
*/ 
function CharCheck(obj)
{
    var strChange = obj.value;
	var strReturn = "";
	var reExp =/^[A-Za-z0-9\wu4E00-\u9FA5]+$/;
	if (reExp.exec(strChange)){
		strReturn = strChange;
	}else{
	    alert ('Invalid input, please try agian!');
	}
	obj.value = strReturn;

}

 /**
    ' ======== ============ =============================
    ' Description: keypress时，允许录入中文、英文、数字
    ' Author: kan
    ' Side Effects: 输入框响应删除功能
    ' Date:2008-12-31
    ' ======== ============ =============================
*/ 

function checkCharPress()
{
   var key = event.keyCode;
   if((key >= 48 && key <= 57) || (key >= 65 && key <= 90) || (key >= 96 && key <= 105) || (key >= 97 && key <= 122) )
   {  
        event.returnValue =true;
   }else{
       event.cancelBubble=true;
       event.returnValue =false;
       //event.keyCode = 0;   
   }   
}

 /**
    ' ======== ============ =============================
    ' Description: int,real input check -2,147,483,648~2,147,483,647
    '              只允许录入数据字符 0-9 和小数点,负数
    ' Author: kan
    ' Side Effects: keypress时
    ' Date:2008-11-11
    ' ======== ============ =============================
*/ 
function checkDecimalPress(obj)
{ 
   var txtval=obj.value;  
  
   var key = event.keyCode;

   if((key < 48||key > 57) && key != 46 && key != 45)
   {  
    event.keyCode = 0;
   }
   else
   {
    if(key == 46)
    {
     if(txtval.indexOf(".") != -1 || txtval.length == 0)
      event.keyCode = 0;
    }
    if(key == 45)
    { 
     if(txtval.indexOf("-") != -1)
      event.keyCode = 0;
    }    
   }
}

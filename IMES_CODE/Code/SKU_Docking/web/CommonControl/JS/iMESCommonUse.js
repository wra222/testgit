﻿/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:javascript method for common use
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-10-22 itc207013             Create 
 * 2009-11-14 Tong.Zhi-Yong         Add DefectInfo    
 * 2009-11-28 Chen Xu               Add Offset Cookie
 * 2009-12-03 Chen Xu               Add Print Function
 * 2010-02-11 Tong.Zhi-Yong         Modify ITC-1103-0172
 * 2010-02-25 Li.Ming-Jun           Modify ITC-1122-0107
 * 2010-02-25 Chen Xu               Add SubStringSN
 * 2010-03-01 Tong.Zhi-Yong         Modify ITC-1122-0147
 * 2010-03-05 Li.Ming-Jun           Modify ITC-1122-0172
 * 2010-03-05 Tong.Zhi-Yong         Modify ITC-1122-0154
 * 2010-03-06 Tong.Zhi-Yong         Modify ITC-1122-0204
 * 2010-03-11 Chen Xu               Modify ITC-1103-0258
 * 2010-04-08 Chen Xu               Modify ITC-1122-0240
 * 2011-12-28 itc202017             Add function:isCustSN,isProdID,isProdIDorCustSN
 * 2012-03-10 Li.Ming-Jun           Modify ITC-1360-1273
 * Known issues:
 */
var weighttype;
var g_reversePrint = false;
var g_landscapePrint = false;

function ShowMessage(message, isPlaySound)
{
    if (arguments.length == 1)
    {
        isPlaySound = true;
    }
    
    var arrMsgs = message.split("|");
    var isDialog = false;
    if (window.dialogArguments != undefined)
    {
        //ITC-1122-0204 Tong.Zhi-Yong 2010-03-06
        message = arrMsgs.length > 1 ? arrMsgs[1] : arrMsgs[0];
        isDialog = true;
    }
    
    if (arrMsgs[0] == "NoButton" && !isDialog)
        ShowMsg(arrMsgs[1], isPlaySound);
    else
    {
        var url=getRoot()+"ErrMessageDisplay.aspx?play=" + isPlaySound;
        window.showModalDialog(url,message,'dialogWidth:650px;dialogHeight:450px;status:no;help:no;menubar:no;toolbar:no;resize:no;');
    }
}

function ShowMsg(message, isPlaySound)
{
    var url=getRoot()+"RedirectErrMsg.aspx?play=" + isPlaySound + "&Message=" + message;
    window.location = url;
}

function UpperCase(obj)
{

        obj.value = obj.value.trim().toUpperCase();
}

function DefectInfo()
{
    this.id = "";
    this.pdLine = "";
    this.testStation = "";
    this.repairID = "";
    this.type = "";
    this.defectCodeID = "";
    this.cause = "";
    this.obligation = "";
    this.component = "";
    this.site = "";
    this.majorPart = "";
    this.remark = "";
    this.vendorCT = "";
    this.partType = "";
    this.oldPart = "";
    this.oldPartSno = "";
    this.newPart = "";
    this.newPartSno = "";
    this.manufacture = "";
    this.versionA = "";
    this.versionB = "";
    this.returnSign = "";
    this.mark = "";
    this.subDefect = "";
    this.piaStation = "";
    this.distribution = "";
    this. _4M = "";
    this.responsibility = "";
    this.action = "";
    this.cover = "";
    this.uncover = "";
    this.trackingStatus = "";
    this.isManual = "";
    this.editor = "";
    this.cdt = "";
    this.udt = "";
    this.newPartDateCode = "";
    this.side = "";
    this.returnStation = "";
    this.causeDesc = "";
    this.defectCodeDesc = "";
    this.Identity = 0;
    this.location = "";
    this.mtaID = "";
    this.testStationDesc = "";
} 

function getSelectionText() 
{
	if(document.selection && document.selection.createRange) 
	{
		return document.selection.createRange().text;
	}
	
	return '';
}
function input1To100Number(con)
{
	if (getSelectionText() != "") 
	{
		document.selection.clear();
	}

	var pattern = /^([1-9][0-9]?|100)$/;
	var conValue = con.value;
	var newValue = conValue + String.fromCharCode(event.keyCode);
	
	if (pattern.test(newValue)) 
	{
		event.returnValue = true;
	}
	else 
	{
		event.returnValue = false;
	}    
}

function input1To9Number(con)
{
	if (getSelectionText() != "") 
	{
		document.selection.clear();
	}

	var pattern = /^([1-9])$/;
	var conValue = con.value;
	var newValue = conValue + String.fromCharCode(event.keyCode);
	
	if (pattern.test(newValue)) 
	{
		event.returnValue = true;
	}
	else 
	{
		event.returnValue = false;
	}    
}

function input1To99Number(con)
{
	if (getSelectionText() != "") 
	{
		document.selection.clear();
	}

	var pattern = /^([1-9][0-9]?)$/;
	var conValue = con.value;
	var newValue = conValue + String.fromCharCode(event.keyCode);
	
	if (pattern.test(newValue)) 
	{
		event.returnValue = true;
	}
	else 
	{
		event.returnValue = false;
	}    
}

function input1To999Number(con)
{
	if (getSelectionText() != "") 
	{
		document.selection.clear();
	}

	var pattern = /^([1-9][0-9]{0,2})$/;
	var conValue = con.value;
	var newValue = conValue + String.fromCharCode(event.keyCode);
	
	if (pattern.test(newValue)) 
	{
		event.returnValue = true;
	}
	else 
	{
		event.returnValue = false;
	}    
}

function input1To9999Number(con)
{
	if (getSelectionText() != "") 
	{
		document.selection.clear();
	}

	var pattern = /^([1-9][0-9]{0,3})$/;
	var conValue = con.value;
	var newValue = conValue + String.fromCharCode(event.keyCode);
	
	if (pattern.test(newValue)) 
	{
		event.returnValue = true;
	}
	else 
	{
		event.returnValue = false;
	}    
}

function input1To4Number(con)
{
	if (getSelectionText() != "") 
	{
		document.selection.clear();
	}

	var pattern = /^([1-4])$/;
	var conValue = con.value;
	var newValue = conValue + String.fromCharCode(event.keyCode);
	
	if (pattern.test(newValue)) 
	{
		event.returnValue = true;
	}
	else 
	{
		event.returnValue = false;
	}    
}

function input1To8Number(con)
{
	if (getSelectionText() != "") 
	{
		document.selection.clear();
	}

	var pattern = /^([1-8])$/;
	var conValue = con.value;
	var newValue = conValue + String.fromCharCode(event.keyCode);
	
	if (pattern.test(newValue)) 
	{
		event.returnValue = true;
	}
	else 
	{
		event.returnValue = false;
	}    
}
function input1To10000Number(con)
{
	if (getSelectionText() != "") 
	{
		document.selection.clear();
	}

	var pattern = /^([1-9][0-9]{0,3}|10000)$/;
	var conValue = con.value;
	var newValue = conValue + String.fromCharCode(event.keyCode);
	
	if (pattern.test(newValue)) 
	{
		event.returnValue = true;
	}
	else 
	{
		event.returnValue = false;
	}    
}

 function ClearGvExtTable(gvExtClientID,InitRowCount)
    {
   
      
        var table=document.getElementById (gvExtClientID);
        var rowCount=table.rows.length;
        var colCount=table.rows[0].cells.length; 
        var rowArray=new Array();
         rowArray.push(' ');
      
        for(var j=0;j<colCount-1;j++)
        {
            rowArray.push('');
        }
        
       if((InitRowCount!=null)&&(InitRowCount<rowCount)&&(InitRowCount>1))
       {
            while(rowCount-InitRowCount>0)
            {
                table.deleteRow(rowCount-1);
                rowCount=table.rows.length;
            }
       }
        for(var i=1;i<rowCount;i++)
        {
            eval("ChangeCvExtRowByIndex_"+gvExtClientID+"(rowArray,false,i)");          
        }
    }
    
  


function inputNumber(con)
{
	if (getSelectionText() != "") 
	{
		document.selection.clear();
	}

	var pattern = /^(0|[1-9]\d*)$/;
	var conValue = con.value;
	var newValue = conValue + String.fromCharCode(event.keyCode);
	
	if (pattern.test(newValue)) 
	{
		event.returnValue = true;
	}
	else 
	{
		event.returnValue = false;
	}
}

function inputPositiveNumber(con)
{
	if (getSelectionText() != "") 
	{
		document.selection.clear();
	}

	var pattern = /^([1-9]\d*)$/;
	var conValue = con.value;
	var newValue = conValue + String.fromCharCode(event.keyCode);
	
	if (pattern.test(newValue)) 
	{
		event.returnValue = true;
	}
	else 
	{
		event.returnValue = false;
	}
}


function inputNumberAndEnglishChar(con)
{
	if (getSelectionText() != "") 
	{
		document.selection.clear();
	}
    var inputContent = con.value;
    var pattern = /^[0-9a-zA-Z]*$/;
    var content = inputContent + String.fromCharCode(event.keyCode);
    
    if (pattern.test(content)) {
        event.returnValue = true;
    } else {
        event.returnValue = false;
    }
}
function inputNumberAndEnglishCharDot(con)
{
	if (getSelectionText() != "") 
	{
		document.selection.clear();
	}
    var inputContent = con.value;
    var pattern = /^[0-9a-zA-Z.]*$/;
    var content = inputContent + String.fromCharCode(event.keyCode);
    
    if (pattern.test(content)) {
        event.returnValue = true;
    } else {
        event.returnValue = false;
    }
}

//added by itc207013, string trim method
String.prototype.trim = function()
{
    if (this == null)
    {
        this == "";
    }
    return this.replace(/^\s*(.*?)[\s\n]*$/g, '$1');
} 

String.prototype.rtrim = function()
{
    if (this == null)
    {
        this == "";
    }
    return this.replace(/(.*?)[\s\n]*$/g, '$1');

} 
function showPrintSetting(station,pcode)
{
     
     var url="../PrintSetting.aspx"+"?Station=" + station + "&PCode=" + pcode;
     window.showModalDialog(url,"",'dialogWidth:800px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');  
}

    //sleep time
var waitTimeForClear=800;
//sleep    
function sleep(milisecond) 
{ 
   
   var currentDate,beginDate=new Date(); 
   var beginHour,beginMinute,beginSecond,beginMs; 
   var hourGaps,minuteGaps,secondGaps,msGaps,gaps; 
   beginHour=beginDate.getHours(); 
   beginMinute=beginDate.getMinutes(); 
   beginSecond=beginDate.getSeconds(); 
   beginMs=beginDate.getMilliseconds(); 
   do 
   { 
      currentDate=new Date(); 
      hourGaps=currentDate.getHours() - beginHour; 
      minuteGaps=currentDate.getMinutes() - beginMinute; 
      secondGaps=currentDate.getSeconds() - beginSecond; 
      msGaps=currentDate.getMilliseconds() - beginMs;      
      if(hourGaps<0)   hourGaps+=24;   
      gaps=hourGaps*3600+ minuteGaps*60+ secondGaps; 
      gaps=gaps*1000+msGaps; 
   }while(gaps<milisecond);   
}



//added by itc208014, create offset cookie,parameters:pCode, labeltype, printtemplate, printmode, rulemode, xoffset, yoffset, printer
function setOffset(pCode,labeltype,printtemplate,printmode,rulemode,x,y,printer) 
{
    if (pCode==null) 
    {
        return false;
    }
    if (labeltype==null)
    {
        return false;
    }
    if(x==null)
    {
        x="10";
    }
    if(y==null)
    {
        y="10";
    }
    if (printer==null)
    {
        printer="";
    }
    
    var key= pCode + ":";
    var labelName= labeltype + "=";
    var value=printtemplate.toString() +","+ printmode.toString()+","+ rulemode.toString() +","+ x.toString()+","+y.toString()+","+printer;
//    var secs=8000;
//    var date = new Date();
//    date.setTime(date.getTime()+(secs*1000));
//    var expires = "; expires="+date.toGMTString();
//    document.cookie = key + labelName + value + expires + "; path=/"; 
//    document.cookie = key + labelName + value + "; path=/"; 
   
    var days=365;
    var date = new Date();
    date.setDate(date.getDay()+days);
    var expires = "; expires="+date.toGMTString();
    document.cookie = key + labelName + value + expires + "; path=/";   
 
}

//added by itc208014, get offset cookie value
function getOffset(pCode, labletype)
{   
    var tagName=pCode+":"+labletype;
    var strCookie = document.cookie; 
    
    if((strCookie==null) || (strCookie==""))
    {
        return "";
    }
    else 
    {
        var arrCookie = strCookie.split(";"); 
      
        for(var i = 0; i < arrCookie.length; i++)
        { 
            var arr = arrCookie[i].split("="); 
            
            if(arr[0].trim()==tagName)
            {   
                var temp=unescape(arr[1]);
                return temp; 
            }
        } 
    }
        return ""; 
}


//added by itc206070
function setInputOrSpanValue(obj, value)
{
    if (obj.tagName.toUpperCase() == "SPAN")
    {
        obj.innerText = value;
    }
    else if (obj.tagName.toUpperCase() == "INPUT")
    {
        obj.value = value;
    }
}

function getInputOrSpanValue(obj)
{
    if (obj.tagName.toUpperCase() == "SPAN")
    {
        return obj.innerText;
    }
    else if (obj.tagName.toUpperCase() == "INPUT")
    {
        return obj.value;
    }
}

function dataInfo(partNo,qty,pqty,dataCllist,partNoCllist)
{
    this.PartNo = partNo;
    this.Qty = qty;
    this.PQty = pqty;
    this.DataCllist = dataCllist;
    this.PartNoCllist = partNoCllist;
}

function substituteInfo(partNo,dataPartNolist, dataDescrlist)
{
    this.PartNo = partNo;
    this.SubstitutePartNo = dataPartNolist;
    this.SubstituteDescr = dataDescrlist;
   
}

function ShowCollection(parameter,urlparam)
{
    //var url=getRoot()+"Operation/CollectionData.aspx";
   
    var url=getRoot()+"CollectionData.aspx";

    if(parameter==null)
    {
         ShowMessage("parameter to the page is null");
    }    
    else
    {
        if(urlparam==null)
        {
           urlparam="";
        }        
         url=url+"?page="+urlparam;
        var temp = window.showModalDialog(url,parameter,'dialogWidth:800px;dialogHeight:350px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');  
        return temp;
    }
}

function ShowSubstitute(parameter)
{
    //var url=getRoot()+"Operation/CollectionData.aspx";
   
    var url=getRoot()+"Substitute.aspx";

    if(parameter==null)
    {
         ShowMessage("parameter to the page is null");
    }    
    else
    {
      
        var temp = window.showModalDialog(url,parameter,'dialogWidth:400px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');  
        return temp;
    }
}
function disableESCOnBody()
{
	if (event == undefined || event == null) 
	{
		return;
	}

	if(event.keyCode==27)
	{
		event.returnValue = false;
	}
}

function urlExist(url)
{
     
     var xmlhttp;
    if (window.XMLHttpRequest)
    {// code for IE7+, Firefox, Chrome, Opera, Safari
		xmlhttp=new XMLHttpRequest();
	}
	else
	{// code for IE6, IE5
		xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
	}   
    
    xmlhttp.open("GET",url,false);   
    xmlhttp.send(null);   
    if (xmlhttp.readyState==4)   
    {
        if(xmlhttp.status==200)
        {
            return true;
        }
        else
        {
			return false;
        }
    }
    else
    {
		return false;
    }
   
}

function ShowMessageByUrl(url,clue)
{
    window.showModalDialog(url,clue,'dialogWidth:650px;dialogHeight:450px;status:no;help:no;menubar:no;toolbar:no;resize:no;');
}

function getRoot()
{
//    var doubleStart=document.URL.indexOf("//");
//    var singleStart=document.URL.indexOf("/",doubleStart+2);
//    var root=document.URL.indexOf("/",singleStart+1);
//    return document.URL.substring(0,root+1);  
    var location=window.location;
	/*
    var currrentUrl;
    if(location.pathname.indexOf("ErrMessageDisplay.aspx")==-1)
    {
        currrentUrl=location.protocol+"//"+location.host+location.pathname;
    }
    else
    {
        currrentUrl=location.protocol+"//"+location.host+"/"+location.pathname;
    }      
    var lastIndex=currrentUrl.lastIndexOf("/");
    var root=document.URL.substring(0,lastIndex);
 //   var fso = new ActiveXObject("Scripting.FileSystemObject");
    while(!urlExist(root+ "/ErrMessageDisplay.aspx"))
    {
          lastIndex=root.lastIndexOf("/");
		  root=root.substring(0,lastIndex);
    }
	*/
	var root =location.protocol+"//"+location.host;
	var path =location.pathname.split("/");
	if (path[0]!=null && path[0]!='')
	{
	   root=root +"/"+ path[0];
	}
	else if (path.length>1 && 
		path[1]!= null &&
		path[1]!='')
	{
	  root=root +"/"+ path[1];
	}
	
     return root+"/";
}



//add by itc-208014:********************** Print Function **********************
//Prepare for Print Start:(get: PrintLabelType,Printer,xoffset,yoffset,xdpi,ypdi) 


   var LabelTypeLst=new Array();
   var PrinterLst=new Array();
   var XDPILst = new Array ();
   var YDPILst = new Array ();
   var PrintContentLst= new Array ();

   var xOffsetValue="";
   var yOffsetValue="";
   var printers="";
   var LabelTypeLstLength="0";
   var labelTypeValue="";
   var MasterPCode="";
   var MasterStationID = "";
   var RemoteBatPath;

   function endWith(str, suffix) {
       return str.indexOf(suffix, str.length - suffix.length) !== -1;
   }
   
   function startWith(str,prefix){

    return str.indexOf(prefix) === 0;
  }
   
  function TransferMasterData(tLabelTypeLst,tPrinterLst,tXDPILst,tYDPILst,tLabelTypeLstLength,tMasterPCode,tMasterStation, tRemoteBatPath)
  {
    LabelTypeLst=tLabelTypeLst;
    PrinterLst=tPrinterLst;
    XDPILst=tXDPILst;
    YDPILst=tYDPILst;
    LabelTypeLstLength=tLabelTypeLstLength;
    MasterPCode=tMasterPCode;
    MasterStationID = tMasterStation;
    RemoteBatPath = tRemoteBatPath;
 //   printLabelTypeCheckFun();                //testing
  
 }
  
 function printLabelTypeCheckFun()       //---- The PrintPages need call this Function ----//
 { 
    var dpix;
    var dpiy;
    var offsetValue;
    var printtemplate;
    var printmode;
    var rulemode;
    var NullLabelTypeLst=new Array();
    
    if (MasterPCode==""||MasterPCode==null)
    {
//       alert ("No PCode!");
       return null;
    }
    
    else if (LabelTypeLstLength=="0"||LabelTypeLstLength==""||LabelTypeLstLength==null)
    {
//        alert("No LabelType!");
       
//        showPrintSetting(MasterStationID,MasterPCode);      // ����ӡ����ҳ��
        return null;
    }
    else
    {
        for (i=0; i<LabelTypeLstLength;i++)
        {
            dpix="";
            dpiy="";
            
            
            offsetValue = getOffset(MasterPCode,LabelTypeLst[i]);
           
            if((offsetValue!=null)&&(offsetValue != ""))
            {      
                var arr = offsetValue.split(",");       
                printtemplate=arr[0];
                printmode=arr[1];
                rulemode=arr[2];
                xOffsetValue=arr[3];
                yOffsetValue=arr[4];
                printerValue=arr[5];
                
                if (printerValue==null||printerValue=="")
                {
                 throw(PrinterOrBatNotExit);    //Ϊ���߽������һ����ӡ��ʾ���ͻ�û��ѡ���ӡ����Portʱ����ʾ�ͻ�������ֹ��ӡ
                }

                if (printmode == "1" || printmode == "3" || printmode == "4")         //template,Bartender, BartenderService mode
                {
                     var printLstFlag=true;
                     for (k=0;k<PrinterLst.length;k++)
                     { 
                        if (PrinterLst[k]==printerValue)
                        {
                            dpix=XDPILst[k];
                            dpiy=YDPILst[k];
                            
                            PrintContentLst[i]=LabelTypeLst[i] +","+ printtemplate+","+printmode+","+rulemode+","+ xOffsetValue +","+ yOffsetValue +","+ printerValue +","+ dpix +","+ dpiy;
                            printLstFlag=false;
                            break;      
                            
                        }
                     }   
                     
                     if  (printLstFlag)
                        {
                            alert ("Can not find Printer: '"+ printerValue + "' !!");
//                            return null;
                            PrintContentLst[i]=LabelTypeLst[i] +","+ printtemplate+","+printmode+","+rulemode+","+ xOffsetValue +","+ yOffsetValue +","+ "" +","+ "" +","+ "";
                            
                        }
                    
                    
                }
                
                if (printmode=="0")        //bat mode
                {
                    var batPath = ClientBatFilePath + "\\" + printtemplate;
                    if (RemoteBatPath) {
                        batPath = RemoteBatPath + "\\" + printtemplate;
                    }

                    //if(!fileObj.FileExists(ClientBatFilePath + "\\" +printtemplate))    //ITC-1103-0258     BAT Files Check 
                    if(!fileObj.FileExists(batPath))  //Vincent 2013-12-11 add check remote bat file
                    {
                          //throw(printtemplate + " " + BatFileNotExist);
						  alert ("Can not find Bat file: '"+ClientBatFilePath + "\\" +printtemplate + "' !!"); 
						 // throw(batPath+ " " + BatFileNotExist);
                    }
                    
                    PrintContentLst[i]=LabelTypeLst[i] +","+ printtemplate+","+printmode+","+rulemode+","+ xOffsetValue +","+ yOffsetValue +","+ printerValue +","+"" +","+ "";
                }
				/*
				else if (printmode=="3") {
				     //檢查Bartender file 
					if ( !startWith(printtemplate,'@')  && endWith(printtemplate.toLowerCase(),'.btw')) {
					
					    var bartenderPath = ClientBartenderFilePath + '\\' + printtemplate;
                        if (RemoteBatPath) {
                            bartenderPath = RemoteBatPath + '\\' + printtemplate;
                        }

                        if (!fileObj.FileExists(bartenderPath))
                        {
                            alert('Can not find Bartender file: ' + bartenderPath + ' !!'); 
                        }
					}
					
					//PrintContentLst[i]=LabelTypeLst[i] +","+ printtemplate+","+printmode+","+rulemode+","+ xOffsetValue +","+ yOffsetValue +","+ printerValue +","+"" +","+ "";
                }
				*/
            } 
              
           else
         {
             NullLabelTypeLst.push(LabelTypeLst[i]);
//             PrintContentLst[i]=LabelTypeLst[i] +","+ ""+","+""+","+""+","+ "" +","+ "" +","+ "" +","+ "" +","+ "";         //  ITC-1122-0106 (���� Int32 ����Чֵ)
         }
      }
      
      if (NullLabelTypeLst!="")
       {
         
//          showPrintSetting(MasterStationID,MasterPCode);      // ����ӡ����ҳ��

            if (PrintContentLst==""||PrintContentLst==null)
            {
//                alert("Please set PrintSetting first!!");
                return null;
            }
            else
            {
//                alert("Please set PrintSetting for LabelType: '"+ NullLabelTypeLst +"' first !!");  // ITC-1103-0214 ��̨Ҳ��������ʾ���ظ��ˣ�ȡ��

                return PrintContentLst;
            }
       }
        
      else
       {
    //        alert("PrintContentLst" +":"+ PrintContentLst);     /*** Add for testing ***/
         return PrintContentLst;
       }
   }
 }   
   
 //Prepare for Print End  
 //Print Start:
 
 
 var isPrintRoomForPrint;
 var SUCFlAG="SUCCESSRET";
 var printedCount=0;
 var total=0;
 var printEnded=false;
 var paramsForMainPrint;

function printStartForPrint(params)
{
   if(printEnded||((!printEnded) && (total==0)))
   {
      printStartForPrintExcute(params);
   }
  
   else
   {
     window.setTimeout(function()  
      { printStartForPrint(params);},
       50);
   }  
}


function printStartForPrintExcute(params)
{
    paramsForMainPrint=params;
    printedCount=0;
    total=0;
    printEnded=false;
    
    if((params!=null) && (params.length>0))
    {
        isPrintRoomForPrint=Boolean.parse(params[params.length-1].isPrintRoom);
        total=params.length-1;
       
        if(total==0)
        {
            printEnded=true;
        }
        
        
        beginWaitingCoverDiv();
        
        for(var i=0; i<params.length-1;i++)
        {
            if (typeof(params[i].keys) == "object")
            {
                PrintService.getPrintInfo(stationid,params[i],i,onGetPrintInfoSucceedForPrint);
            }
        }
       
    }
    else
    {
        printEnded=true;
    }
 
}

var openobj=window;

function onGetPrintInfoSucceedForPrint(result)
{
     if((result.length>1)&&(result[0]==SUCFlAG))
     {       
        var printTemplateEntity=result[1];
        
        printTemplateEntity="TemplateEntity";     /*** Add for testing ***/
       
        var i=result[2];
        var curPrintItem=result[3];
        var curLabelTypeName;
        var curXoffset;
        var curYoffset;
        var curPrinter;
        var curDPI;
       
  
        
        if(printTemplateEntity!=null)
        {
                if((curPrintItem!="")||(curPrintItem!=null))
                {
                   curLabelTypeName=curPrintItem.LabelType;
                    for (i=0;i<PrintContentLst.length;i++)
                    {
                        var temp=PrintContentLst[i].split(",");
                        if (curLabelTypeName==temp[0].trim())
                        {
                           
                           curXoffset=temp[1];
                           curYoffset=temp[2];
                           curPrinter=temp[3];
                           curDPI=temp[4];
                          
                        }
                       
                    }
                }
           
                if (typeof(curPrintItem.Keys) == "object")
                {
                     ImageService.getImageListForPrint(curPrintItem,isPrintRoomForPrint,i,curDPI,onGetImgInfoSucceedForPrint)
                }
               
            

                 if((typeof(window.dialogArguments) == "object") && (window.dialogArguments.length==2) && (window.dialogArguments[0]=="window"))
                {
                    openobj =  window.dialogArguments[1];
                }
                
               // var pathForPrint=getRoot()+"PrintLabel/";
                
                if(isSerial)
                {
                     openobj.closeModal = false
                     
                     window.showModalDialog(getRoot()+"PrintLabel/"+"PrintFrame.aspx?aspFile=" + printTemplateEntity.processFile.trim()  + "&index=" + i + "&item=" + curPrintItem.item + "&testFlag=" +curPrintItem.isTestPrint, openobj , "dialogWidth:500px;dialogHeight:500px;center:yes");
                }
                else
                {
                    openobj.open(getRoot()+"PrintLabel/" +printTemplateEntity.processFile.trim() + "?index=" + i + "&item=" + curPrintItem.item + "&testFlag=" + curPrintItem.isTestPrint , "_blank" , "height=0,width=0,top=10000,left=10000");
                }
                 AddAndJudgePrintEnd();
           }
        else
        {
            AddAndJudgePrintEnd();
        }
     }
     else
     {
        AddAndJudgePrintEnd();
     }
}

function onGetImgInfoSucceedForPrint(result)
{
    if((result.length>1)&&(result[0]==SUCFlAG))
    {                                  
        var printSettingEntity=result[1];
        var retImgList=result[2];
        var i=result[3]; 
        var printPiece=result[4];        
        var list;
        for(var j=0;j<retImgList.length;j++)
        {
            list= new Array();
            list.push(retImgList[j].ImageString);       
        }        
    }
    else
    {      
        alert(result[0]);
    }
    openobj.closeModal=true;
    AddAndJudgePrintEnd();  
}

function ExcuteSinglePrint(params,i)
{
    closeModal=false;
    isShowModal=true;
    if (typeof(params[i].keys) == "object")
    {
        PrintService.getPrintInfo1(params[i],i,onGetPrintInfoSucceedForPrint);
    }
    else
    {
        PrintService.getPrintInfo(params[i],i,onGetPrintInfoSucceedForPrint);
    }
}

function AddAndJudgePrintEnd()
{
    if(total==0)
    {
         printEnded=true;
         if(isSerial)
         {
            endWaitingCoverDiv();
         }
         return ;
    }
    
    printedCount++;
    if(isSerial)
     {
        if(printedCount==total)
        {
            window.focus();
            printEnded=true;
            getCommonInputObject().disabled=false;
            getCommonInputObject().focus(); 
            endWaitingCoverDiv();
        }
        else
        {
           ExcuteSinglePrint(paramsForMainPrint,printedCount);
        }
     }
     else if(printedCount==total)
     {
         printEnded=true;
     }
}

function WaitWebPrint()
{

    if(openobj.closeModal)
    {
       ExcuteSinglePrint(paramsForMainPrint,printedCount);
    }
    else
    {
          window.setTimeout(WaitWebPrint(),100);
    }
}



//Print End
 //********************** Print Function End**********************

  //added by tzy(206070) replace special character when onpaste event occur
function replacePaste(con) 
{
    if (event != null && event != undefined) 
    {
        var conLength = con.value.length;
        var pattern = new RegExp("[^-0-9a-zA-Z\s\*]", "g");
        event.returnValue = false;
        
        if (con.replaceExpression != "undefined" && con.replaceExpression != "")
        {
            pattern = new RegExp(con.replaceExpression, "g");
        }
        
        if(typeof document.selection !="undefined")
        {
            document.selection.createRange().text = window.clipboardData.getData("Text").replace(pattern, '');
        }
        else
        {
            con.value=con.value.substr(0,con.selectionStart) + window.clipboardData.getData("Text").replace(pattern, '') + con.value.substring(con.selectionStart,conLength);
        }
    }
}

//added by tzy(206070) replace special character when ondrop event occur
function replacePasteDrop(con)
{
    if (event != null && event != undefined) 
    {
        var conLength = con.value.length;
        var pattern = new RegExp("[^-0-9a-zA-Z\s\*]", "g");
        event.returnValue = false;
        
        if (con.replaceExpression != "undefined" && con.replaceExpression != "")
        {
            pattern = new RegExp(con.replaceExpression, "g");
        }        
        
        con.focus();
        document.selection.createRange().text = event.dataTransfer.getData('TEXT').replace(pattern, '').toUpperCase();       
    }    
}

/*
 * Date: 2010-02-01
 * Author: Tong.Zhi-Yong
 * Description: Methods for print
 */
function createFileObject() {
    var userAgent = navigator.userAgent.toLowerCase();
    if (/msie/.test(userAgent)) 
    {
        return  new ActiveXObject("Scripting.FileSystemObject");
    } else {
    return null;    
    }
}

var fileObj = createFileObject();       //new ActiveXObject("Scripting.FileSystemObject"); 



 var globalIndex = -1;
 var globalItemsForSerial;
 
 function PrintItem(printMode, ruleMode, labelType, templateName, piece, spName, parameterKeys, parameterValues, offsetX, offsetY, printerPort, dpi, batPiece,rotate180,layout)
 {
    this.PrintMode = printMode;
    this.RuleMode = ruleMode;
    this.LabelType = labelType;
    this.TemplateName = templateName;
    this.Piece = piece;
    this.SpName = spName;
    this.ParameterKeys = parameterKeys;
    this.ParameterValues = parameterValues;
    this.OffsetX = offsetX;
    this.OffsetY = offsetY;
    this.PrinterPort = printerPort;
    this.dpi = dpi;
    this.BatPiece = batPiece;
    this.Rotate180 = rotate180;
    this.Layout = layout;
 }
 
 function getPrintItemCollection(batPiece)
 {
    if (arguments.length == 0)
    {
        batPiece = 1;
    }
 
    var lstCollection = printLabelTypeCheckFun();
    var printsettingArr;
    var length;
    var printItemArr = new Array();
    
    if (lstCollection == undefined || lstCollection == null || lstCollection.length == 0)
    {
        return null;
		/* Modified by He Jiang @ 2012/01/18: make a fake NOT_FOUND_TEMPLATE instead of returning null
		var nft_labelType = "NOT_FOUND",
		nft_printTemplate = "NOT_FOUND_TEMPLATE",
		nft_printMode = 1,
		nft_runMode = 0,
		nft_offsetX = 10,
		nft_offsetY = 10;
		lstCollection = [ nft_labelType + "," + nft_printTemplate +
			"," + nft_printMode + "," + nft_runMode +
			"," + nft_offsetX + "," + nft_offsetY +
			"," + "" + "," + "" + "," + "" ];
		*/
    }
    
    length = lstCollection.length;
    
    for (var i = 0; i < length; i++)
    {
        printsettingArr = lstCollection[i].split(",");
        printItemArr[printItemArr.length] = new PrintItem(printsettingArr[2], printsettingArr[3], printsettingArr[0], printsettingArr[1], 0, "", null, null, printsettingArr[4], printsettingArr[5], printsettingArr[6], printsettingArr[7], batPiece,0,0);
    }
    
    return printItemArr;
 }
 
 function removePrintItemFromCollectionByLabelType(prtItemCollection, labelType)
 {  
    var length = prtItemCollection.length;
    
    for (var i = 0; i < length; i++)
    {
        if (prtItemCollection[i].LabelType == labelType)
        {
            for (var j = i + 1; j < length; j++)
            {
                prtItemCollection[j - 1] = prtItemCollection[j];
            }

            prtItemCollection.length--;
            length--;
            i--;
        }
    }
 }
 
 function getPrintItemByLabelType(prtItemCollection, labelType)
 {
    var length = prtItemCollection.length;

    for (var i = 0; i < length; i++)
    {
        if (prtItemCollection[i].LabelType == labelType)
        {
            return prtItemCollection[i];
        }
    }
    
    return null;   
 }
 
 function setPrintParam(printItemCollection, labelType, keyCollection, valueCollection)
 {
    //ITC-1103-0172 Tong.Zhi-Yong 2010-02-11
    if (printItemCollection == undefined || printItemCollection == null || printItemCollection.length == 0)
    {
        return;
    }    
 
    for (var i = 0; i < printItemCollection.length; i++)
    {
        if (printItemCollection[i].LabelType == labelType)
        {
            if (isGenerateNumberType())
            {
                
            }
            else 
            {
                printItemCollection[i].ParameterKeys = keyCollection;
                printItemCollection[i].ParameterValues = valueCollection;
            }
            
            return;
        }
    }
	/* Added by He Jiang @ 2012/01/18
	if (printItemCollection[0].LabelType === "NOT_FOUND")
	{
		var pi = printItemCollection[0], k = keyCollection, v = valueCollection;
		if (Object.prototype.toString.call(k) !== "[object Array]")
			k = [];
		var p = new Array(k.length);
		for (var j = 0; j < k.length; ++j)
		{
			if (Object.prototype.toString.call(v[j]) !== "[object Array]")
				v[j] = [];
			p[j] = k[j] + ": " + v[j].join(", ");
		}
		pi.ParameterKeys = ["PARA1", "PARA2", "PARA3"];
		pi.ParameterValues = [
		["No template or batch found. Please check print settings."],
		["Label Type - " + labelType],
		["Parameters - " + p.join("; ")]];
	}
	*/
 }

//Vincent 2014-06-28 add for set all parameters not filter LabelType
 function setAllPrintParam(printItemCollection, keyCollection, valueCollection) {
     if (printItemCollection == undefined || printItemCollection == null || printItemCollection.length == 0) {
         return;
     }

     for (var i = 0; i < printItemCollection.length; i++) {
        if (isGenerateNumberType()) {
        }
        else {
            printItemCollection[i].ParameterKeys = keyCollection;
            printItemCollection[i].ParameterValues = valueCollection;
        }                     
     }
 }
 
 function isGenerateNumberType()
 {
    //need modify 
    return false;
 }
 
function processSerialNoList(lstSN, prefix, startIndex)
{
    var ret = "";
    var length = -1;
    
    if (arguments.length == 1)
    {
        prefix = "SNO";
        startIndex = 1;
    }
    
    if (arguments.length == 2)
    {
        startIndex = 1;
    }
    
    if (lstSN == undefined || lstSN == null || lstSN.length == 0)
    {
        return ret;
    }
    
    length = lstSN.length;
    
    for (var i = startIndex; i < length + startIndex; i++)
    {
        ret += "\r\nSET " + prefix + i + "=" + lstSN[i - startIndex] + "\r\n";
    }
    
    return ret;
}
 
 function printLabels(printItems, isSerial)
 {
    if (printItems == undefined || printItems == null)
    {
        return;
    }
 
    if (isSerial)
    {
        serialPrint(printItems);
    }
    else
    {
        concurrentPrint(printItems);
    }
 }
  var printingItem;
 function serialPrint(printItems)
 {
    if (printItems.length == 0)
    {
        return;
    }
    globalItemsForSerial = printItems;

    printingItem = printItems[++globalIndex];

    checkAndExecuteForSerial(printingItem);
 }
 
 function concurrentPrint(printItems)
 {

    for (var i = 0; i < printItems.length; i++)
    {
        printingItem = printItems[i];
        checkAndExecuteForConcurrent(printingItem);
    }   
 }
 
 function runBat(homeDir, mainBatInfo, isLogo, isSynchronized)
 {
    if (arguments.length == 3)
    {
        isSynchronized = false;
    }
 
     try
     {
         if (createDir(homeDir))
         {
            //var fileName = "PTR" + (new Date()).getTime() + ".bat";
            var fileName = "PRT" + mainBatInfo[0].replace(/[^0-9a-zA-Z]*/g, '') + mainBatInfo[1];
            var bat = fileObj.CreateTextFile(homeDir + "\\" + fileName, true);
            
            //mainBatInfo[2] += "\r\ndel %0\r\n";
            
            bat.WriteLine(mainBatInfo[2]);
            bat.Close()
            
            wsh = new ActiveXObject("wscript.shell");
            //wsh.run("cmd /k " + getHomeDisk(homeDir) + "&cd " + homeDir + "&" + fileName + "&exit", 2, true);
            //ITC-1122-0154 Tong.Zhi-Yong 2010-03-05
            wsh.run("cmd /k " + getHomeDisk(homeDir) + "&cd " + homeDir + "&" + fileName + "&exit", 2, isSynchronized);        
         }
     }
	catch(e)
	{
		alert(e.description);
	}     
 }
 
 function getBatContentForSerialSuccCallBack(result)
 {
    runBat(ClientBatFilePath, result, false, true);
    
    postProcessForSerial();
 }
 
 function getBatContentForSerialFailCallBack(result)
 {
    ShowMessage(result.get_message());
    ShowInfo(result.get_message());
    globalIndex = -1;   
 }
 
 function getFailCallBack(result)
 {
    ShowMessage(result.get_message());
    ShowInfo(result.get_message());
    globalIndex = -1;
}
 
 function getNameValueTextFileType(arg) {
     var ret = [];     
     for (var i = 0; i < arg.length; i++) {
         if (arg[i].DataType == '2') {
             ret.push(arg[i]);            
         }
     }
     return ret;
 }

 function findNameValueTextFileType(arg, name) {
     
     for (var i = 0; i < arg.length; i++) {
         if (arg[i].Name == name) {
             return arg[i].Value;
         }
     }
     return '';
 }

 function writeTextFile(file,data) {
     var bat = fileObj.CreateTextFile(file, true);
     bat.WriteLine(data);
     bat.Close()
 }
 
 function runBartender(mainBatInfo, isSynchronized)
 {
    if (arguments.length == 3)
    {
        isSynchronized = false;
    }
	// lblType , btwName , sqlParameter , btwFilePath , piece , Printer
    try
    {
        var homeDir = mainBatInfo.FilePath;
        var pathBtw = homeDir + "\\" + mainBatInfo.Template;
        var arg = mainBatInfo.SpReturn;
        var piece = mainBatInfo.Piece;
        var btPrinter = mainBatInfo.Printer;
		
		var btApp, btFmt;
		btApp = new ActiveXObject("BarTender.Application");
		btApp.Visible = false;
		btFmt = btApp.Formats.Open(pathBtw, false, btPrinter);
		btFmt.EnablePrompting = false;
		
		var btwLackParam = "";
		var oriNamedSubStrings = btFmt.NamedSubStrings.GetAll("=", "~").split("~");
		for (var j = 0; j < oriNamedSubStrings.length; j++) {
			var oriName = oriNamedSubStrings[j].split("=")[0];
			if (""==oriName)
				continue;
			
			var bExistParam = false;
			for (var i = 0; i < arg.length; i++) {
				var p = arg[i];
				if (oriName == p.Name && p.DataType=='1' ) {
					btFmt.SetNamedSubStringValue(p.Name, p.Value);
					bExistParam = true;
					break;
				}
			}
			if (!bExistParam){
				if (""==btwLackParam) btwLackParam = oriName;
				else btwLackParam += ", " + oriName;
			}
		}
		if (""!=btwLackParam){
			btFmt.Close(1); // btDoNotSaveChanges
			btApp.Quit(1);
			throw 'Fail Printing. BTW file need Parameter= ' + btwLackParam;
		}

		//檢查Text file case
		var textFiles=getNameValueTextFileType(arg);
		var dbArray= btFmt.Databases
		var count = dbArray.Count;
		if (count > 0 && textFiles.length>0) {		    
		    for (var i = 1; i <= count; i++) {
		        var db = dbArray.GetDatabase(i);
		        if (db.Type == 0) //Text file type
		        {
		            var textData=''
		            if (count == 1) {
		                var textData = textFiles[0].Value;
		            } else {
		                var textData = findNameValueTextFileType(textFiles, db.Name);
		            }
		            if ( textData!= '') {
		                // writefile fileObj
		                var fileName=ClientBatFilePath+'\\'+db.Name+'.txt';
		                writeTextFile(fileName, textData);
		                db.TextFile.FileName = fileName;	//Set FileName to Label	                
		            }		        
		        }
		    }		
		}
		
		btFmt.IdenticalCopiesOfLabel = piece;
		btFmt.PrintOut(false,false); // ShowStatusWindow, ShowPrintDialog
		
		btFmt.Close(1); // btDoNotSaveChanges
		btApp.Quit(1);
		
    }
	catch(e)
	{
		if (null != e.description)
			alert(e.description);
		else
			alert(e);
	}     
 }
 
 function getBtwNameValueListCallBack(result)
 {
    runBartender(result, true);
 }
 
 function getBartenderXmlContentCallBack(result)
 {
    // can't use. XMLScript has parameter with type out.
 }
 
 //ITC-1122-0147 Tong.Zhi-Yong 2010-03-01
 function getImageStringArray(imgList)
 {
    if (imgList == undefined || imgList == null || imgList.length == 0)
    {
        return;
    }
 
    var arr = new Array();
    
    for (var i = 0; i < imgList.length; i++)
    {
        arr[i] = imgList[i].ImageString;
    }
    
    return arr;
 }
 
 function getImageContentForSerialSuccCallBack(result)
 {
    var printingItem = globalItemsForSerial[globalIndex];
    document.getElementById("objPrint").printWithRotateAndPaperLayout(printingItem.PrinterPort, getImageStringArray(result), result.length, printingItem.Piece, printingItem.OffsetX, printingItem.OffsetY, result[0].ImageWidthPX, result[0].ImageHeightPX, result[0].ImagePixPerM,
           		result.Rotate180 || g_reversePrint,
           		(result.Layout || g_landscapePrint) ? 2 : 1);
    postProcessForSerial();
 }
 
 function getImageContentForSerialFailCallBack(result)
 {
    ShowMessage(result.get_message());
    ShowInfo(result.get_message());
    globalIndex = -1;
 }
 
 function getBatContentForConcurrentSuccCallBack(result)
 {
    runBat(ClientBatFilePath, result, false);
 }
 
 function getBatContentForConcurrentFailCallBack(result)
 {
    ShowMessage(result.get_message());   
    ShowInfo(result.get_message());     
 }
 
 function getImageContentForConcurrentSuccCallBack(result)
 {
     document.getElementById("objPrint").printWithRotateAndPaperLayout(result.PrinterPort, getImageStringArray(result), result.length, result.Piece, result.OffsetX, result.OffsetY, result[0].ImageWidthPX, result[0].ImageHeightPX, result[0].ImagePixPerM, result.Rotate180 || g_reversePrint,
           		(result.Layout || g_landscapePrint) ? 2 : 1);
 }
 
 function getImageContentForConcurrentFailCallBack(result)
 {
    ShowMessage(result.get_message());   
    ShowInfo(result.get_message());      
 }
 
 function checkAndExecuteForSerial(printItem)
 {
    var printMode;
    
    printMode = printItem.PrintMode;
 
    if (printMode == 0)
    {
//        if(!fileObj.FileExists(ClientBatFilePath + "\\" +printItem.TemplateName)){
//            ShowMessage(printItem.TemplateName +BatFileNotExist);   
//            ShowInfo(printItem.TemplateName +BatFileNotExist);
//            return;
//        }
        //bat print
        PrintService.getBatContent(printItem, MasterPCode, ClientBatFilePath, getBatContentForSerialSuccCallBack, getBatContentForSerialFailCallBack);
    }
    else if (printMode == 1 || printMode == 4) 
    {
        //template print
        PrintService.getImageContent(printItem, function(rv) {
        rv.PrinterPort = printItem.PrinterPort;
        rv.Piece = printItem.Piece;
        rv.OffsetX = printItem.OffsetX;
        rv.OffsetY = printItem.OffsetY;
        
        rv.Rotate180 = printItem.Rotate180;
        rv.Layout = printItem.Layout;
		
        getImageContentForSerialSuccCallBack(rv);
                         }, getImageContentForSerialFailCallBack);
    }    
	else if (printMode == 3)
    {
		PrintService.getBtwNameValueList(printItem, MasterPCode, getBtwNameValueListCallBack, getFailCallBack);
	}
 }
 
 function checkAndExecuteForConcurrent(printItem)
 {
    var printMode;  
    
    printMode = printItem.PrintMode;
    
    if (printMode == 0)
    {
//        if(!fileObj.FileExists(ClientBatFilePath + "\\" +printItem.TemplateName)){
//            ShowMessage(printItem.TemplateName +BatFileNotExist);   
//            ShowInfo(printItem.TemplateName +BatFileNotExist);
//            return;
//        }
        //bat print
        PrintService.getBatContent(printItem, MasterPCode, ClientBatFilePath, getBatContentForConcurrentSuccCallBack, getBatContentForConcurrentFailCallBack);
    }
    else if (printMode == 1 || printMode == 4) 
    {
        //template print
        PrintService.getImageContent(printItem, function(rv) {
        rv.PrinterPort = printItem.PrinterPort;
        rv.Piece = printItem.Piece;
        rv.OffsetX = printItem.OffsetX;
        rv.OffsetY = printItem.OffsetY;
        
        rv.Rotate180 = printItem.Rotate180;
        rv.Layout = printItem.Layout;
                         	getImageContentForConcurrentSuccCallBack(rv);
                         }, getImageContentForConcurrentFailCallBack);
    }     
	else if (printMode == 3)
    {
		PrintService.getBtwNameValueList(printItem, MasterPCode, getBtwNameValueListCallBack, getFailCallBack);
	}
 }
 
 function postProcessForSerial()
 {
    if (globalItemsForSerial.length > (globalIndex + 1))
    {
        checkAndExecuteForSerial(globalItemsForSerial[++globalIndex]);
    }
    else
    {
        globalIndex = -1;
        globalItemsForSerial = null;     
    }   
 }
 
 function getHomeDisk(dirName)
 {
	var disk = "";
	try
	{
		var dirArray = dirName.split("\\");
	    disk = dirArray[0]
	}
	catch(e)
	{
		alert(e.description )

	}
	return disk
}
 
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	createDir
//| Author		:	Lucy Liu
//| Create Date	:	11/26/2008
//| Description	:	create folders, recursive
//| Input para.	:	folder string
//| Ret value	:	success?true:false
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function createDir(dirName)
{
	var strErr = "";
	try
	{
		var targetDir = dirName;
		var dirArray = targetDir.split("\\");
		tempDirName = dirArray[0];
		for(var i=1; i<dirArray.length; i++)
		{
			if("" == dirArray[i])
			{//~~skip blank dir name
				continue;
			}

			tempDirName += "\\" + dirArray[i];
			if(fileObj.FolderExists(tempDirName))
			{//~~skip existed dir name
				//fileObj.deleteFolder(tempDirName)
//				if (i == (dirArray.length-1)) {
//				    fileObj.deleteFolder(tempDirName)
//				} else {
				   continue;
//				}
				
			}
			else 
			{
			    if (!fileObj.CreateFolder(tempDirName))
			    {
				    strErr = "Creat directory " + tempDirName + " Failure!!!";
				    throw strErr;
			    }			    
			}
		}
	
		return true;
	}
	catch(e)
	{
		if (typeof(globalDebug) != "undefined")
		{
			if(strErr == "")
			{
				strErr = e.description;
			}
			alert("createDir:\n" + strErr);
		}
		alert(e.description )
	
		return false;
	}
}

 function generateArray(val)
{
    var ret = new Array();
    
    ret[0] = val;
    
    return ret;
}

//Document onkeydown envent
var isFrame = false;
var isErrMsg = false;
var scanValue = "";

//function document.onkeydown(e)
//{
//    return KeyDownEvent(e);
//}
document.onkeydown = function(e) {
    return KeyDownEvent(e);
};

function KeyDownEvent(e)
 {
    var event;
    if (window.event) {
        event = window.event;
    }
    //FF uses this
    else {
        event = e;
    } 


    if ((!isFrame)&&(event.ctrlKey)&&(event.keyCode == 116)) //Ctrl + F5, reset page
    {
        try
        {
            window.location.reload();
        }
        catch (err) {}

        return false;
    }
    else if (event.keyCode == 116||event.keyCode == 117) //Disable F5 and F6
    {
        event.keyCode = 0;
        event.cancelBubble = true;
        return false;
    }
    else if (isErrMsg) //Scan 'enter', close error message window
    {
        if (event.keyCode == 13||event.keyCode == 9)
        {
            //Defined 'scanVal' in ErrMessageDisplay.aspx
            scanVal = scanVal.replace(/[^0-9a-zA-Z]*/g, '')
            if (scanVal.toUpperCase() == "ENTER")
                window.close();

            scanVal = "";
        }
        else
            scanVal += String.fromCharCode(event.keyCode);

        return false;
    }
    else if (event.keyCode == 13||event.keyCode == 9)
    {
        //If escape, return false
        if (scanValue == 27)
        {
            scanValue = "";
            return false;
        }

        scanValue = "";
    }
    else
    {
        if(event.keyCode == 27) //Escape, change tab
            SwichTab();

        scanValue += event.keyCode;
    }

    return true;
}

function SwichTab()
{
    if (isFrame)
        changeTab();
    else
        try {
            window.parent.changeTab();
        }
        catch (e) {}
}

function SubStringSN(sn,Type)           
{
  sn=sn.trim().toUpperCase();
  switch(Type)
  
    {   case("ProdId"): {
                        return sn.substring(0,9);
                        break;}
                    case ("MB"):
                        {
                            //return sn.substring(0,10);
                            if ((sn.length == 11) && (sn.substr(5, 1) == "M" || sn.substr(5, 1) == "B")) {
                                return sn.substring(0, 11);
                            }
                            else if ((sn.length == 10) && (sn.substr(4, 1) == "M" || sn.substr(4, 1) == "B")) {
                                return sn.substring(0, 10);
                            }
                            else if ((sn.length == 11) && (sn.substr(4, 1) == "M" || sn.substr(4, 1) == "B")) {
                                return sn.substring(0, 10);
                            }
                            else {
                                return sn.substring(0, 10); 
                            }

                            break;
                        }
        case("VB"): {
                    return sn.substring(0,10);
                    break;}
        case("SVB"): {
                    return sn.substring(0,10);
                    break;
                }
// ================ Mark in 2011/5/18 ~ by Benson
//       case("CustSN"): { 
//                        return sn.substring(0,10);
//                        break;}
        default: {return sn;}
    }
}


//log

    var timeES;
    var bomtime;
    var MVSfso = createFileObject();   //new ActiveXObject("Scripting.FileSystemObject");  
    var wf;
    var logfile="C:\\fislog\\log.txt";
    var logfolder="C:\\fislog";
    var logEnabled=true;
    
    
    function LogInfo(message)
    {
        if(!logEnabled)
        {
            return;
        }
        if(!MVSfso.FolderExists(logfolder))
        {
            MVSfso.CreateFolder(logfolder);
        }

        if(MVSfso.FileExists(logfile))  
        {
             wf  = MVSfso.OpenTextFile(logfile,8);   
        }
        else
        {
            wf = MVSfso.CreateTextFile(logfile);        
        }
       // wf  = MVSfso.OpenTextFile(logfile,8);   
        timeES = new Date();
        wf.WriteLine(message+formatDate(timeES).toString());   
        wf.Close();   
    }
    
function   formatDate(timeUTC) // YYYY-MM-DD   HH:MM:SS MSE 
 {   
  var   year=timeUTC.getYear();   
  var   month=timeUTC.getMonth()+1;   
  var   date=timeUTC.getDate();   
  var   hour=timeUTC.getHours();   
  var   minute=timeUTC.getMinutes();   
  var   second=timeUTC.getSeconds();  
  var   msecond=timeUTC.getMilliseconds(); 
  return   " "+year+"-"+month+"-"+date+"   "+hour+":"+minute+":"+second + " " + msecond;   
  }
  
function formateDateTime_YMDHMS(timeUTC)    // YYYY-MM-DD HH:MM:SS
{
  var   year=timeUTC.getYear();   
  var   month=timeUTC.getMonth()+1;   
  var   date=timeUTC.getDate();   
  var   hour=timeUTC.getHours();   
  var   minute=timeUTC.getMinutes();   
  var   second=timeUTC.getSeconds();  
  return   year+"-"+month+"-"+date+" "+hour+":"+minute+":"+second ;
 
}  

  //for gridview paging
function GoToPageByIndex(gvtarget,txtID,pageCount)
{
    var index=document.getElementById(txtID).value.toString().trim();
    var patt1 = new RegExp( /^[1-9][0-9]*$/);
    
     if(index.length>pageCount.toString().length)
     {
        showErrorIndexInfo();
     }
     else if(patt1.test(index))
     {
        if((index.length==pageCount.toString().length) && (parseInt(index)>pageCount))
        {
            showErrorIndexInfo();
        }
        else
        {
             __doPostBack(gvtarget,'Page$' +index);
        }
     }
     else
     {
        showErrorIndexInfo();
     }

}

function showErrorIndexInfo()
{
    alert("Invalid page index input!");
}

//add by xmzh
function Encode_URL2(v_strSource)
{
    if( trim(v_strSource) == "" )
        return "" ;

    var strMarkString = "#%&+', ";
    var strChangeString = String.fromCharCode(21) + String.fromCharCode(22) + String.fromCharCode(23) + String.fromCharCode(24);
    strChangeString += String.fromCharCode(25) + String.fromCharCode(26) + String.fromCharCode(27);

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

function trim(sStr){
  return sStr.replace(/(^\s*)|(\s*$)/ig,"");
}

//for tooltip on client
 var oPopup;
 function revertStr(oMessage)
{
    if(oPopup==null)
    {
      oPopup = window.createPopup();
    }
    if(oPopup.isOpen == false)
    {
        var oPopupBody = oPopup.document.body;
        var lefter = event.x + 25;
        var topper = event.y + 160;

        oPopupBody.style.color = "#000000";
        oPopupBody.style.fontSize = "9pt";
        oPopupBody.style.padding = "2";
        oPopupBody.style.backgroundColor="#FFFFCC";
        oPopupBody.style.border="1px solid black";
        oPopupBody.style.fontFamliy ="Arial";

        if (oMessage != null)
        {
//            oMessage = oMessage.replace(/\</g, "&lt;");
//            oMessage = oMessage.replace(/\>/g, "&gt;");
        	////debugger
            oPopupBody.innerHTML = oMessage;
            //alert(oMessage.length+"vs"+getLength(oMessage));
            oPopup.show(lefter, topper,getLength(oMessage)+5 , 20, document.body);
        }
    }
}

function closePopup()
{
    if (oPopup != null)
    {
        oPopup.hide();
    }
}

function getLength(str)
{
    var len = 0;

    if ((str==null)||(str.trim()==""))
    {
        return 0;
    }

    for (var i = 0; i < str.length; i++)
    {
        var charCode = str.charCodeAt(i);
        //BCS
        if (charCode < 0 || charCode > 255) 
        {
            len += 15;
        }
        //upper case
        else if (charCode >= 65 && charCode <= 90) 
        {
            //len += 11;
            len += 8;
        }
        else
        {
            len += 8;
        }
    }
    
    return len;
}


//For PAK weight
function setobjMSCommPara2()
{
    var objMSComm =document.getElementById("objMSComm");
//    alert("2");
    //	alert(objMSComm.OutBufferSize);
    if (objMSComm.CommPort != 1) {
        if (objMSComm.PortOpen) {
            objMSComm.PortOpen = false;
        }    
    
        objMSComm.CommPort = 1;
    }
   
    objMSComm.Settings = "9600,n,8,1";
    objMSComm.RThreshold = 40;
    objMSComm.SThreshold = 1;
    objMSComm.Handshaking = 0;
    
	try {
	    if (objMSComm.PortOpen == false)
		    objMSComm.PortOpen = true;
	} catch (e) {
	      alert(e.description);
    }
}

function setobjMSCommPara1()
{
    var objMSComm =document.getElementById("objMSComm");
//    alert("1");
//	alert(objMSComm.OutBufferSize);
    if (objMSComm.CommPort != 1) {
        if (objMSComm.PortOpen) {
            objMSComm.PortOpen = false;
        }

        objMSComm.CommPort = 1;
    }
    objMSComm.Settings = "2400,E,7,1";
    objMSComm.RThreshold = 1;
    objMSComm.SThreshold = 1;
    objMSComm.Handshaking = 0;
    
	try {
	    if (objMSComm.PortOpen == false)
		    objMSComm.PortOpen = true;
	} catch (e) {
	      alert(e.description);
    }
}

function ReloadImesPage()
{
    try {
            window.location.replace(window.location.href);
        }
        catch (err) 
        {}
        return false;
}

function setobjMSCommPara(port, setting, rThres, sThres, handShaking)
{
    var objMSComm =document.getElementById("objMSComm");

    if (objMSComm.CommPort != port) {
        if (objMSComm.PortOpen) {
            objMSComm.PortOpen = false;
        }

        objMSComm.CommPort = port;
    }
    objMSComm.Settings = setting;
    objMSComm.RThreshold = rThres;
    objMSComm.SThreshold = sThres;
    objMSComm.Handshaking = handShaking;
    
	try {
	    if (objMSComm.PortOpen == false)
		    objMSComm.PortOpen = true;
		else
		{
		    objMSComm.PortOpen = false;
		    objMSComm.PortOpen = true;
		}
		return true;
	} catch (e) {
	    //  alert(e.description);
		return false;
    }
}

function setCommPara_PassOrFail(FilePath, type)
{
	if (type == undefined || type == null || type == "")
	{
		alert("Please send type parameter!");
	}
	var port = -1;
	var setting = "";
	var rThres = 0;
	var sThres = 0;
	var handShaking = 0;
	var settingflag = false;
	var coll = null;

	var retComm = null;
	var ret = new Array(4);
	var signalPass = "";
	var signalFail = "";
	
	var filename = type + ".ini";
	if (!fileObj.FileExists(FilePath + filename)) {
		createFile(FilePath, filename, getIniFileContent(type));
		retComm = setobjMSCommPara(1, "9600,n,8,1", 40, 1, 0);
		signalPass = "Pass";
		signalFail = "Fail";
		settingflag = true;
	}
	else {
		coll = getFileContentCollection(FilePath, filename);
		signalPass = getValue("SignalPass", coll);
		signalFail = getValue("SignalFail", coll);
	}
	
	if (!settingflag)
	{
		port = getValue("CommPort", coll);
		setting = getValue("Settings", coll);
		rThres = getValue("RThreshold", coll);
		sThres = getValue("SThreshold", coll);
		handShaking = getValue("Handshaking", coll);
		retComm = setobjMSCommPara(port, setting, rThres, sThres, handShaking);			
	}
	
	ret[0] = retComm;
	ret[1] = signalPass;
	ret[2] = signalFail;
	return ret;
}

function setCommPara(type, WeightFilePath)
{
	if (type == undefined || type == null || type == "")
	{
		alert("Please send type parameter!");
	}

	var port = -1;
	var setting = "";
	var rThres = 0;
	var sThres = 0;
	var handShaking = 0;
	var settingflag = false;
	var coll = null;

	var ret = false;
	
	if (type == "P")
	{
	    if (!fileObj.FileExists(WeightFilePath + "PltWeight.ini"))
		{
		    createFile(WeightFilePath, "PltWeight.ini", getIniFileContent(type));
			ret = setobjMSCommPara(1, "2400,E,7,1", 1, 1, 0);
			settingflag = true;
		}
		else
		{
		    coll = getFileContentCollection(WeightFilePath, "PltWeight.ini");
		}
    } else if (type == "U") {
    if (!fileObj.FileExists(WeightFilePath + "UnitWeight.ini")) {
        createFile(WeightFilePath, "UnitWeight.ini", getIniFileContent(type));
            ret = setobjMSCommPara(1, "9600,n,8,1", 40, 1, 0);
            settingflag = true;
        }
        else {
            coll = getFileContentCollection(WeightFilePath, "UnitWeight.ini");
            weighttype = getValue("WeightType", coll);
        }
    } else if (type == "R") {
        if (!fileObj.FileExists(WeightFilePath + "RCTOWeight.ini")) {
            createFile(WeightFilePath, "RCTOWeight.ini", getIniFileContent(type));
            ret = setobjMSCommPara(1, "9600,n,8,1", 40, 1, 0);
            settingflag = true;
        }
        else {
            coll = getFileContentCollection(WeightFilePath, "RCTOWeight.ini");
            weighttype = getValue("WeightType", coll);
        }
	}
	else
	{
	    if (!fileObj.FileExists(WeightFilePath + "CtnWeight.ini"))
		{
		    createFile(WeightFilePath, "CtnWeight.ini", getIniFileContent(type));
			ret = setobjMSCommPara(1, "9600,n,8,1", 40, 1, 0);
			weighttype= "";
			settingflag = true;
		}
		else
		{
		    coll = getFileContentCollection(WeightFilePath, "CtnWeight.ini");				
			weighttype = getValue("WeightType", coll);
		}
	}

	if (!settingflag)
	{
		port = getValue("CommPort", coll);
		setting = getValue("Settings", coll);
		rThres = getValue("RThreshold", coll);
		sThres = getValue("SThreshold", coll);
		handShaking = getValue("Handshaking", coll);
		ret = setobjMSCommPara(port, setting, rThres, sThres, handShaking);			
	}
	
	return ret;
}

function createFile(path, fileName, fileContent)
{
	 try
	 {
		 if (createDir(path))
		 {
			var bat = fileObj.CreateTextFile(path + "\\" + fileName, true);
			
			bat.WriteLine(fileContent);
			bat.Close();
		 }
	 }
	catch(e)
	{
		alert(e.description);
	}		
}

function getIniFileContent(type)
{
	var str = "";

	if (type == "P")
	{
		str += "CommPort=1\r\n";
		str += "Settings=2400,E,7,1\r\n";
		str += "RThreshold=1\r\n";
		str += "SThreshold=1\r\n";
		str += "Handshaking=0\r\n";
	}
	else if (type == "PassOrFail")
	{
		str += "CommPort=1\r\n";
		str += "Settings=9600,n,8,1\r\n";
		str += "RThreshold=40\r\n";
		str += "SThreshold=1\r\n";
		str += "Handshaking=0\r\n";
		str += "SignalPass=Pass\r\n";
		str += "SignalFail=Fail\r\n";
	}
	else
	{
		str += "CommPort=1\r\n";
		str += "Settings=9600,n,8,1\r\n";
		str += "RThreshold=40\r\n";
		str += "SThreshold=1\r\n";
		str += "Handshaking=0\r\n";
		str += "WeightType=\r\n";			    //add by Shao for Carton Weight
	}

	return str;
}

function getValue(key, coll)
{
	if (coll == undefined || coll == null)
	{
		return "";
	}

	for (var i = 0; i < coll.length; i++)
	{
		if (coll[i].indexOf(key) != -1)
		{
			var arr = coll[i].split("=");

			if (arr.length > 1)
			{
				return arr[1];
			}
		}
	}

	return "";
}

function getFileContentCollection(path, fileName)
{   
	var f = fileObj.OpenTextFile(path + "\\" + fileName);
	var coll = new Array();
    
	while (!f.AtEndOfStream)
	{
		var r = f.ReadLine();
		
		coll.push(r);
	}
    
	f.Close();	

	return coll;
}

//<<CI-MES12-SPEC-000-UC Common Rule>> 2.27
function isPCARepairMBSno(input) {
    if (input.length != 10 && input.length != 11) return false;
    pattMBSno1 = /^.{4}M.{5,6}$/;
    pattMBSno2 = /^.{5}M.{5}$/;
    pattMBSno3 = /^.{4}B.{5,6}$/;
    pattMBSno4 = /^.{5}B.{5}$/;
    if (pattMBSno1.exec(input) || pattMBSno2.exec(input)) return true;
    else if (pattMBSno3.exec(input) || pattMBSno4.exec(input)) return true;
    else return false;
}

function getMBSno(input) {
    if (input.length != 10 && input.length != 11) return "";
    pattMBSno1 = /^.{4}M.{5}$/;
    pattMBSno2 = /^.{5}M.{5}$/;
    pattMBSno3 = /^.{4}M.{6}$/;

    pattMBSno4 = /^.{4}B.{5}$/;
    pattMBSno5 = /^.{5}B.{5}$/;
    pattMBSno6 = /^.{4}B.{6}$/;
    if (pattMBSno1.exec(input) || pattMBSno2.exec(input)) return input;
    if (pattMBSno4.exec(input) || pattMBSno5.exec(input)) return input;
    if (pattMBSno3.exec(input) || pattMBSno6.exec(input)) return input.substring(0, 10);
    else return "";
}

function isCustSN(input) {
    if (input.length != 10) return false;
    pattCustSN1 = /^CN.{8}$/;
    pattCustSN2 = /^5C.{8}$/;
    pattCustSN3 = /^8C.{8}$/;
    if (pattCustSN1.exec(input) || pattCustSN2.exec(input) || pattCustSN3.exec(input) ) return true;
    else return false;
}

function isProdID(input, line) {
    if (input.length != 9 && input.length != 10) return false;
    pattProdID = new RegExp("^[A-Z][0-9][1-9ABC][0-9]{3}.{3,4}$", "i");
    if (pattProdID.exec(input)) return true;
    else return false;
}

function isProdIDorCustSN(input, line) {
    return isCustSN(input) || isProdID(input, line);
}

//EDITS Function use
//-----------------------------------------------------------------
//Create sub dir
function CheckMakeDir(strPathName) {
    try {
        var astrPath = "";
        var ulngPath = "";
        var i = 0;
        var strTmpPath = "";
        var strPath = "";
        var ldir = strPathName.lastIndexOf("\\");
        strPath = strPathName.substring(0, ldir);
        var fso = new ActiveXObject("Scripting.FileSystemObject");

        if (fso.FolderExists(strPath)) return true;

        astrPath = strPath.split("\\");
        ulngPath = astrPath.length;
        strTmpPath = "";
        for (i = 0; i < ulngPath; i++) {
            strTmpPath += astrPath[i] + "\\";
            if ((astrPath[0].length == 2) && (i < 1))
                continue;
            else if ((astrPath[0].length == 0) && (astrPath[1].length == 0) && (i < 4))
                continue;
            if (!fso.FolderExists(strTmpPath))
                fso.CreateFolder(strTmpPath);
        }
        return true;
    } catch (e) {
        return false;
    }
}

 function cmdCpfile(file1, file2) { 

     var sbParm1 = file1;
     var sbParm2 = file2;
     //addNet();
     fso = new ActiveXObject("Scripting.FileSystemObject");
     fso.CopyFile(sbParm1, sbParm2)
/*
   try       
    {       
        var objShell = new ActiveXObject("wscript.shell");       
        objShell.Run("Copy " + sbParm1 + " " + sbParm2);       
        objShell = null;       
    }       
    catch(e)    
    {    
         
    }       
*/

 }

//Create PDF File -Asynchronous, Return immediately -true:  (hide-window)
//Do not know whether PDF file was created successfully
function CreatePDFfileAsyn(FopCommandpathfile, XmlFilename, TemplateFilename, PdfFilename,islocalCreate) {

    //DEBUG Mode=True , Get Xml,Template,Pdf File Root Path from Web.conf
    //DEBUG Mode=False, Incoming parameters: XmlFilename,TemplateFilename,PdfFilename is full path name

    var jsQuote = "\""; //fop install path maybe contains spaces-" ",add protection 
    var fullXmlFilename = "";
    var fullTemplateFilename = "";
    var fullPdfFilename = "";
    if (GetDebugMode()) {
        //ITC Debug , copy sample template to target template file
        var fullTemplateFilename = GetTEMPLATERootPath() + TemplateFilename; //"TemplateShip_Label.xslt";
        try {
            cmdCpfile(GetTEMPLATERootPath() + "TemplateShip_Label.xslt", fullTemplateFilename);
        }
        catch (e) {
            //alert("Please check access " + GetTEMPLATERootPath() + " permission!");
            return false;
        }
        fullXmlFilename = GetCreateXMLfileRootPath() + XmlFilename;
        if (islocalCreate)
            fullPdfFilename = GetCreatePDFfileClientPath() + PdfFilename;
        else
            fullPdfFilename = GetCreatePDFfileRootPath() + PdfFilename;
    }
    else {
        fullTemplateFilename = TemplateFilename;
        fullXmlFilename = XmlFilename;
        fullPdfFilename = PdfFilename;
    }
    CheckMakeDir(fullXmlFilename);
    CheckMakeDir(fullPdfFilename);

    //var fopexe = jsQuote + GetFopCommandPathfile() +".bat" + jsQuote;

    var fopexe = jsQuote + FopCommandpathfile + jsQuote;
    //Check local fop command Exist??
    var strfop = FopCommandpathfile + ".bat";
    var fso = new ActiveXObject("Scripting.FileSystemObject");
    if (!(fso.FileExists(strfop))) {
        strfop += " doesn't exist.";
        //alert(strfop);
        return false;
    }
    //Create PDF File
    CheckMakeDir(fullPdfFilename);
    WshShell = new ActiveXObject("Wscript.Shell")
    var strArgument = "";
    strArgument = " -xml " + jsQuote + fullXmlFilename + jsQuote + " -xsl " + jsQuote + fullTemplateFilename + jsQuote + " -pdf " + jsQuote +fullPdfFilename + jsQuote;
    var IsSuccess = WshShell.Run(fopexe + strArgument, 0, false);
    if (IsSuccess != 0) {
        //alert("Create PDF Failed! Please check the permission and parameters!");
        return false;
    }
    return true;
}

//Create PDF File -Synchronization, return true--CreatePDF Finished! -- min-window
function CreatePDFfile(FopCommandpathfile, XmlFilename, TemplateFilename, PdfFilename,islocalCreate) {

    //DEBUG Mode=True , Get Xml,Template,Pdf File Root Path from Web.conf
    //DEBUG Mode=False, Incoming parameters: XmlFilename,TemplateFilename,PdfFilename is full path name

    var jsQuote = "\""; //fop install path maybe contains spaces-" ",add protection 
    var fullXmlFilename = "";
    var fullTemplateFilename = "";
    var fullPdfFilename = "";
    if (GetDebugMode()) {
        //ITC Debug , copy sample template to target template file
        var fullTemplateFilename = GetTEMPLATERootPath() + TemplateFilename; //"TemplateShip_Label.xslt";
        try {
            cmdCpfile(GetTEMPLATERootPath() + "TemplateShip_Label.xslt", fullTemplateFilename);
        }
        catch (e) {
            //alert("Please check access " + GetTEMPLATERootPath() + " permission!");
            return false;
        }
        fullXmlFilename = GetCreateXMLfileRootPath() + XmlFilename;
        if (islocalCreate)
            fullPdfFilename = GetCreatePDFfileClientPath() + PdfFilename;
        else
            fullPdfFilename = GetCreatePDFfileRootPath() + PdfFilename;
    }
    else {
        fullTemplateFilename = TemplateFilename;
        fullXmlFilename = XmlFilename;
        fullPdfFilename = PdfFilename;
    }
    CheckMakeDir(fullXmlFilename);
    CheckMakeDir(fullPdfFilename);

    //var fopexe = jsQuote + GetFopCommandPathfile() +".bat" + jsQuote;

    var fopexe = jsQuote + FopCommandpathfile + jsQuote;
    //Check local fop command Exist??
    var strfop = FopCommandpathfile + ".bat";
    var fso = new ActiveXObject("Scripting.FileSystemObject");
    if (!(fso.FileExists(strfop))) {
        strfop += " doesn't exist.";
        //alert(strfop);
        return false;
    }
    //Create PDF File
    CheckMakeDir(fullPdfFilename);
    WshShell = new ActiveXObject("Wscript.Shell")
    var strArgument = "";
    strArgument = " -xml " + jsQuote + fullXmlFilename + jsQuote + " -xsl " + jsQuote + fullTemplateFilename + jsQuote + " -pdf " + jsQuote +fullPdfFilename + jsQuote;
    var IsSuccess = WshShell.Run(fopexe + strArgument, 2, true);
    if (IsSuccess != 0) {
        //alert("Create PDF Failed! Please check the permission and parameters!");
        return false;
    }
    return true;
}
//Call EDITS Function
function invokeEDITSFunc(WEBEDITSAddr, EDITSFunctionName, Parameters) {
    var data;
    
    data = '<?xml version="1.0" encoding="utf-8"?>';
    data = data + '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'; //�̶�
    data = data + '<soap:Body>';
    data = data + '<' + EDITSFunctionName + ' xmlns="http://tempuri.org/">';
    for (var i = 1; i <= Parameters.paraCount(); i++) {
        data = data + '<' + Parameters.GetparaName(i) + '>' + Parameters.GetparaValue(Parameters.GetparaName(i)) + '</' + Parameters.GetparaName(i) + '>';
    }
    data = data + '</' + EDITSFunctionName + '>';
    data = data + '</soap:Body>';
    data = data + '</soap:Envelope>';

    //var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	var xmlhttp;
    if (window.XMLHttpRequest)
    {// code for IE7+, Firefox, Chrome, Opera, Safari
		xmlhttp=new XMLHttpRequest();
	}
	else
	{// code for IE6, IE5
		xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
	}
    var URL = WEBEDITSAddr;
    xmlhttp.open("POST", URL, false);
    xmlhttp.setRequestHeader("Content-Type", "text/xml; charset=utf-8");
    xmlhttp.setRequestHeader("SOAPAction", "http://tempuri.org/" + EDITSFunctionName);
    xmlhttp.send(data);
    var result = xmlhttp.status;
    //OK
    if (result != 200) {
        //document.write(xmlhttp.responseText); 
        //alert(xmlhttp.responseText);
        return false;
    }
    else {
        return true;
    }
}
//Add EDITS Para
function EDITSFuncParameters() {
    var _pl = new Array();
    var paraname = new Array();
    this.add = function(index, name, value) {
        _pl[name] = value;
        paraname[index] = name;
        return this;
    }
    this.GetparaName = function(index) {
        return paraname[index];
    }
    this.GetparaValue = function(name) {
        return _pl[name];
    }
    this.paraCount = function() {
        return paraname.length - 1;
    }
}
// **************** PDF Create For BSam ****************

function CreatePDFfileAsynGenPDF_BSam(WEBEDITSAddr, XmlFilename, TemplateFilename, PdfFilename, islocalCreate) {

    //DEBUG Mode=True , Get Xml,Template,Pdf File Root Path from Web.conf
    //DEBUG Mode=False, Incoming parameters: XmlFilename,TemplateFilename,PdfFilename is full path name
    var strErrtext = "";
    var Paralist = new EDITSFuncParameters();
    Paralist.add(1, "xslFile", TemplateFilename);
    Paralist.add(2, "xmlFile", XmlFilename);
    Paralist.add(3, "pdfFile", PdfFilename);
    Paralist.add(4, "ErrText", strErrtext);
    var result = invokeEDITSFuncAsync_BSam(WEBEDITSAddr, "GenPDF", Paralist);
    return result;
}

function invokeEDITSFuncAsync_BSam(WEBEDITSAddr, EDITSFunctionName, Parameters) {
    var data;

    data = '<?xml version="1.0" encoding="utf-8"?>';
    data = data + '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'; //�̶�
    data = data + '<soap:Body>';
    data = data + '<' + EDITSFunctionName + ' xmlns="http://tempuri.org/">';
    for (var i = 1; i <= Parameters.paraCount(); i++) {
        data = data + '<' + Parameters.GetparaName(i) + '>' + Parameters.GetparaValue(Parameters.GetparaName(i)) + '</' + Parameters.GetparaName(i) + '>';
    }
    data = data + '</' + EDITSFunctionName + '>';
    data = data + '</soap:Body>';
    data = data + '</soap:Envelope>';

    var xmlhttp;
    if (window.XMLHttpRequest)
    {// code for IE7+, Firefox, Chrome, Opera, Safari
		xmlhttp=new XMLHttpRequest();
	}
	else
	{// code for IE6, IE5
		xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
	}
	
    var URL = WEBEDITSAddr;
    xmlhttp.open("POST", URL, false);
    xmlhttp.setRequestHeader("Content-Type", "text/xml; charset=utf-8");
    xmlhttp.setRequestHeader("SOAPAction", "http://tempuri.org/" + EDITSFunctionName);
    xmlhttp.send(data);
    //var s = xmlHTTP.responseText;
    var xmlDoc = xmlhttp.responseXML;
    var status = xmlhttp.status;
    //OK

    var result = new Array();
    var msg1;
   
    if (status != 200) {
        result[0] = false;
        result[1] = "Call Edits webservice failed,status :" + status;


    }
    else {
        if (EDITSFunctionName == "BoxShipToShipmentLabel" || EDITSFunctionName == "BoxShipToShipmentMRPLabel") {
            var dn = Parameters.GetparaValue("Dn");
            //   msg1 = xmlDoc.getElementsByTagName("BoxShipToShipmentLabelResult")[0].childNodes[0].nodeValue;
            var tmp = EDITSFunctionName + "Result";
            msg1 = xmlDoc.getElementsByTagName(tmp)[0].childNodes[0].nodeValue;
            if (msg1 == dn) {
                result[0] = true;
            }
            else {
                result[0] = false;
                result[1] = msg1;
            }

        }
        else if (EDITSFunctionName == "GenPosXML") {
			var truckid = Parameters.GetparaValue("truckId");
            //   msg1 = xmlDoc.getElementsByTagName("BoxShipToShipmentLabelResult")[0].childNodes[0].nodeValue;
            var tmp = EDITSFunctionName + "Result";
            msg1 = xmlDoc.getElementsByTagName(tmp)[0].childNodes[0].nodeValue;
            if (msg1 == truckid) {
                result[0] = true;
            }
            else {
                result[0] = false;
                result[1] = msg1;
            }
		}
		else //GenPDFResult
        {
            msg1 = xmlDoc.getElementsByTagName("GenPDFResult")[0].childNodes[0].nodeValue;
            if (msg1 == "false") {
                result[0] = false;
                result[1] = xmlDoc.getElementsByTagName("ErrText")[0].childNodes[0].nodeValue; 
            }
            else {
                result[0] = true;
            }
        }
     
    
    }
    return result;
//    if (EDITSFunctionName == "BoxShipToShipmentLabel") {
//        result[0] = xmlDoc.getElementsByTagName("BoxShipToShipmentLabelResult")[0].childNodes[0].nodeValue;
////        if (result[0] == "false" && xmlDoc.getElementsByTagName("ErrText")[0].childNodes[0].nodeValue != null)
////        { result[1] = xmlDoc.getElementsByTagName("ErrText")[0].childNodes[0].nodeValue }
//        return result;
//    }
//    else {
//        result[0] = xmlDoc.getElementsByTagName("GenPDFResult")[0].childNodes[0].nodeValue;
//        if (result[0] == "false" && xmlDoc.getElementsByTagName("ErrText")[0].childNodes[0].nodeValue != null)
//        { result[1] = xmlDoc.getElementsByTagName("ErrText")[0].childNodes[0].nodeValue }
//        return result;
//    }
   

}



// **************** PDF Create For BSam ****************

//------------------ PDF Create ->invoke Webservice ------------------------------
//Create PDF File -Asynchronous, Return immediately -true:  (hide-window)
//Do not know whether PDF file was created successfully
function CreatePDFfileAsynGenPDF(WEBEDITSAddr, XmlFilename, TemplateFilename, PdfFilename, islocalCreate) {

    //DEBUG Mode=True , Get Xml,Template,Pdf File Root Path from Web.conf
    //DEBUG Mode=False, Incoming parameters: XmlFilename,TemplateFilename,PdfFilename is full path name
    var strErrtext = "";
    var Paralist = new EDITSFuncParameters();
    Paralist.add(1, "xslFile", TemplateFilename);
    Paralist.add(2, "xmlFile", XmlFilename);
    Paralist.add(3, "pdfFile", PdfFilename);
    Paralist.add(4, "ErrText", strErrtext);
    var IsSuccess = invokeEDITSFuncAsync(WEBEDITSAddr, "GenPDF", Paralist);
    return IsSuccess;
}

//Create PDF File -Synchronization, return true--CreatePDF Finished! -- min-window
function CreatePDFfileGenPDF(WEBEDITSAddr, XmlFilename, TemplateFilename, PdfFilename, islocalCreate) {
    var strErrtext = "";
    var Paralist = new EDITSFuncParameters();
    Paralist.add(1, "xslFile", TemplateFilename);
    Paralist.add(2, "xmlFile", XmlFilename);
    Paralist.add(3, "pdfFile", PdfFilename);
    Paralist.add(4, "ErrText", strErrtext);
    var IsSuccess = invokeEDITSFunc(WEBEDITSAddr, "GenPDF", Paralist);
    return IsSuccess;
}

function invokeEDITSFuncAsync(WEBEDITSAddr, EDITSFunctionName, Parameters) {
    var data;
    
    data = '<?xml version="1.0" encoding="utf-8"?>';
    data = data + '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'; //�̶�
    data = data + '<soap:Body>';
    data = data + '<' + EDITSFunctionName + ' xmlns="http://tempuri.org/">';
    for (var i = 1; i <= Parameters.paraCount(); i++) {
        data = data + '<' + Parameters.GetparaName(i) + '>' + Parameters.GetparaValue(Parameters.GetparaName(i)) + '</' + Parameters.GetparaName(i) + '>';
    }
    data = data + '</' + EDITSFunctionName + '>';
    data = data + '</soap:Body>';
    data = data + '</soap:Envelope>';

    //var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	var xmlhttp;
    if (window.XMLHttpRequest)
    {// code for IE7+, Firefox, Chrome, Opera, Safari
		xmlhttp=new XMLHttpRequest();
	}
	else
	{// code for IE6, IE5
		xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
	}
    var URL = WEBEDITSAddr;
    xmlhttp.open("POST", URL, true);
    xmlhttp.setRequestHeader("Content-Type", "text/xml; charset=utf-8");
    xmlhttp.setRequestHeader("SOAPAction", "http://tempuri.org/" + EDITSFunctionName);
    xmlhttp.send(data);
    return true;

}

function GetAndSetinnerText(obj, value) {
    if (document.all) {
        if (typeof (value) == "undefined") {
            return obj.innerText;
        } else {
            obj.innerText = value;
        }
    } else {
        if (typeof (value) == "undefined") {
            return obj.textContent;
        } else {
            return obj.textContent = value;
        }
    }
}

function CheckCustomerSN(sn) {
    var regxCQ = /^5CG[0-9]{3}[A-Z0-9]{4}$/;
    var regxCQ2 = /^A5CG[0-9]{3}[A-Z0-9]{4}$/;
    var regxSH = /^CNU[A-Z0-9]{7}$/;
    var regxSH7cd = /^7CD[A-Z0-9]{7}$/;
    var regxCQ8cn = /^8CN[A-Z0-9]{7}$/;
    var regxCQ8cq = /^8CQ[A-Z0-9]{7}$/;
	if(sn.length==11 && sn.substr(0,1)=='S')
		sn = sn.substr(1,10);
return (regxSH.test(sn) || regxSH7cd.test(sn) || regxCQ.test(sn) || regxCQ2.test(sn) || regxCQ8cn.test(sn) || regxCQ8cq.test(sn));
}
function CheckCustomerSNinPizza(sn) {
    var regxSH = /^[PA]CNU[0-9]{3}[^WXYZ][A-Z0-9]{3}$/;
    var regxSH7cd = /^[PA]7CD[0-9]{3}[^WXYZ][A-Z0-9]{3}$/;
    var regxCQ8cn = /^[PA]8CN[0-9]{3}[^WXYZ][A-Z0-9]{3}$/;
    var regxCQ8cq = /^[PA]8CQ[0-9]{3}[^WXYZ][A-Z0-9]{3}$/;
    return (regxSH.test(sn) || regxSH7cd.test(sn) || regxCQ8cn.test(sn) || regxCQ8cq.test(sn))
}
function CheckCustomerSNinPizzaForCQ(sn) {

    var regxCQPizza = /^[SAP]{1}5CG[0-9]{3}[A-Z0-9]{4}$/;
    return regxCQPizza.test(sn);

}

String.prototype.right = function(len) {
    return (len > this.length) ? this : this.substring(this.length - len);
}

function updateCalendarFields(cal) {
    var date = cal.selection.get();
    if (date) {
        date = Calendar.intToDate(date);
        cal.inputField.value = Calendar.printDate(date, "%Y-%m-%d") +
             " " + ("0" + cal.getHours()).right(2) + ":" + ("0" + cal.getMinutes()).right(2);
    }
}

function updateCalendarFieldswithSeconds(cal) {
    var date = cal.selection.get();
    if (date) {
        date = Calendar.intToDate(date);
        cal.inputField.value = Calendar.printDate(date, "%Y-%m-%d") +
             " " + ("0" + cal.getHours()).right(2) + ":" + ("0" + cal.getMinutes()).right(2) + ":00";
    }
}
function Get2DCodeCustSN(input) {
    var arr = input.split(',');
    if (arr.length > 0) {
        if (isCustSN(arr[0]))
            return arr[0];
        else
            return input;
    }
    else
        return input;
}
//------------------------------------------------------------------------------

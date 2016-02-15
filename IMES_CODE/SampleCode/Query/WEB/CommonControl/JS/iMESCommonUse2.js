/*
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
 * Known issues:
 */

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
     
      var   xmlhttp   =   new   ActiveXObject("Microsoft.XMLHTTP");   
    
      xmlhttp.open("GET",url,false);   
      xmlhttp.send(null);   
      if (xmlhttp.readyState==4)   
      {
          if(xmlhttp.Status==200)
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
   var MasterStationID=""; 
  

  function TransferMasterData(tLabelTypeLst,tPrinterLst,tXDPILst,tYDPILst,tLabelTypeLstLength,tMasterPCode,tMasterStation)
  {
    LabelTypeLst=tLabelTypeLst;
    PrinterLst=tPrinterLst;
    XDPILst=tXDPILst;
    YDPILst=tYDPILst;
    LabelTypeLstLength=tLabelTypeLstLength;
    MasterPCode=tMasterPCode;
    MasterStationID=tMasterStation;
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
       
//        showPrintSetting(MasterStationID,MasterPCode);      // 调打印设置页面
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
                 throw(PrinterOrBatNotExit);    //为产线紧急增加一个打印提示：客户没有选择打印机或Port时，提示客户，并终止打印
                }
                
                if (printmode=="1")         //template mode
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
                
                else if (printmode=="0")        //bat mode
                {
                    if(!fileObj.FileExists(ClientBatFilePath + "\\" +printtemplate))    //ITC-1103-0258     BAT Files Check 
                    {
<<<<<<< .mine
                          throw(printtemplate+"  " + BatFileNotExist);
=======
                          throw(printtemplate + " " + BatFileNotExist);
>>>>>>> .r1384
                    }
                    
                    PrintContentLst[i]=LabelTypeLst[i] +","+ printtemplate+","+printmode+","+rulemode+","+ xOffsetValue +","+ yOffsetValue +","+ printerValue +","+"" +","+ "";
                    
                }
            } 
              
           else
         {
             NullLabelTypeLst.push(LabelTypeLst[i]);
//             PrintContentLst[i]=LabelTypeLst[i] +","+ ""+","+""+","+""+","+ "" +","+ "" +","+ "" +","+ "" +","+ "";         //  ITC-1122-0106 (不是 Int32 的有效值)
         }
      }
      
      if (NullLabelTypeLst!="")
       {
         
//          showPrintSetting(MasterStationID,MasterPCode);      // 调打印设置页面

            if (PrintContentLst==""||PrintContentLst==null)
            {
//                alert("Please set PrintSetting first!!");
                return null;
            }
            else
            {
//                alert("Please set PrintSetting for LabelType: '"+ NullLabelTypeLst +"' first !!");  // ITC-1103-0214 后台也有类型提示，重复了，取消

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
 var fileObj = new ActiveXObject("Scripting.FileSystemObject"); 
 var globalIndex = -1;
 var globalItemsForSerial;
 
 function PrintItem(printMode, ruleMode, labelType, templateName, piece, spName, parameterKeys, parameterValues, offsetX, offsetY, printerPort, dpi, batPiece)
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
    }
    
    length = lstCollection.length;
    
    for (var i = 0; i < length; i++)
    {
        printsettingArr = lstCollection[i].split(",");
        printItemArr[printItemArr.length] = new PrintItem(printsettingArr[2], printsettingArr[3], printsettingArr[0], printsettingArr[1], 0, "", null, null, printsettingArr[4], printsettingArr[5], printsettingArr[6], printsettingArr[7], batPiece);
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
                printItemCollection[i].parameterKeys = keyCollection;
                printItemCollection[i].parameterValues = valueCollection;
            }
            
            break;
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
    document.getElementById("objPrint").print(printingItem.PrinterPort, getImageStringArray(result), result.length, printingItem.Piece, printingItem.OffsetX, printingItem.OffsetY, result[0].ImageWidthPX, result[0].ImageHeightPX, result[0].ImagePixPerM);
    
    postProcessForSerial();
 }
 
 function getImageContentForSerialFailCallBack(result)
 {
    ShowMessage(result.get_message());   
    ShowInfo(result.get_message());    
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
     document.getElementById("objPrint").print(printingItem.PrinterPort, getImageStringArray(result), result.length, printingItem.Piece, printingItem.OffsetX, printingItem.OffsetY, result[0].ImageWidthPX, result[0].ImageHeightPX, result[0].ImagePixPerM);
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
        PrintService.getBatContent(printItem, getBatContentForSerialSuccCallBack, getBatContentForSerialFailCallBack);
    }
    else if (printMode == 1) 
    {
        //template print
        PrintService.getImageContent(printItem, getImageContentForSerialSuccCallBack, getImageContentForSerialFailCallBack);
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
        PrintService.getBatContent(printItem, getBatContentForConcurrentSuccCallBack, getBatContentForConcurrentFailCallBack);
    }
    else if (printMode == 1) 
    {
        //template print
        PrintService.getImageContent(printItem, getImageContentForConcurrentSuccCallBack, getImageContentForConcurrentFailCallBack);
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

function document.onkeydown()
{
    return KeyDownEvent();
}

function KeyDownEvent()
{
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
        window.parent.changeTab();
}

function SubStringSN(sn,Type)           
{
  sn=sn.trim().toUpperCase();
  switch(Type)
  
    {   case("ProdId"): {
                        return sn.substring(0,9);
                        break;}
        case("MB"): {
                    return sn.substring(0,10);
                    break;}
        case("VB"): {
                    return sn.substring(0,10);
                    break;}
        case("SVB"): {
                    return sn.substring(0,10);
                    break;}
     //  case("CustSN"): {  -- Mark in 5/18 ~By Benson
                   //     return sn.substring(0,10);
                       // break;}
        default: {return sn;}
    }
}


//log
    var timeES;
    var bomtime;
    var MVSfso = new ActiveXObject("Scripting.FileSystemObject");  
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
    objMSComm.CommPort = 1;
    objMSComm.Settings = "9600,n,8,1";
    objMSComm.RThreshold = 120;
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
    objMSComm.CommPort = 1;
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
    try
        {
            window.location.reload();
        }
        catch (err) 
        {}
        return false;
}
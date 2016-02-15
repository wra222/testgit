
<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Print Setting
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-11-25  Lucy Liu(EB2)        Create 
 2010-01-07  Lucy Liu(EB2)       Modify:   ITC-1122-0091 
 2010-01-07  Lucy Liu(EB2)       Modify:   ITC-1103-0176 
 2010-01-07  Lucy Liu(EB2)       Modify:   ITC-1103-0097 
 2010-01-07  Lucy Liu(EB2)       Modify:   ITC-1103-0187 
 2010-01-07  Lucy Liu(EB2)       Modify:   ITC-1122-0228 
 2010-01-07  Lucy Liu(EB2)       Modify:   ITC-1103-0314 
 2010-04-07  Lucy Liu(EB2)       Modify:   ITC-1122-0197
 
 Known issues:
 --%>
 
<%@ Page Language="C#" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PrintSetting.aspx.cs" Inherits="_PrintSetting" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Print Setting</title>
    
</head>
<script type="text/javascript" src="CommonControl/JS/iMESCommonUse.js"></script>

<object classid="clsid:{24BB7CDB-562E-4D60-8A56-4DD1DD77BE48}" codebase="../CommonControl/print.cab#version=<%=WebConstant.version%>"
        id="printWB" style="display:none"></object>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
          <asp:ServiceReference Path="Service/PrintSettingService.asmx" />
        </Services>
    </asp:ScriptManager>
<SCRIPT LANGUAGE="JavaScript">
<!--

var gvClientID = "<%=gridview.ClientID%>"
var printerlList;
var portList = new Array();

var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var printInfo;
var msgNoSetPrinter='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoSetPrinter") %>';
var msgNoSetOffset='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoSetOffset") %>';
var msgNoSetBat='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoSetBat") %>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgNoSetPort='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoSetPort") %>';
var msgIllegalXOffset='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgIllegalXOffset") %>';
var msgIllegalYOffset='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgIllegalYOffset") %>';
var hilightRowIndex = -1;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onload
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	获取所有打印机列表
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
document.body.onload = function ()
{
    var count = <%=portNumber%>;
    
    for(var i=1; i <= parseInt(count); i++) {
        portList.push("COM" + i );
    }
    for(var i=1; i <= parseInt(count); i++) {
        portList.push("LPT" + i );
    }

    prints = printWB.getPrintList();    
    printerlList = prints.split(";");
    //获取Template/BAT单元格中所有下拉框的内容
    PrintSettingService.GetPrintInfo(document.getElementById("<%=pCodeHidden.ClientID%>").value,onSucceed,onFail);
    
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onSucceed
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	调用web service成功
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onSucceed(result)
{
   try {
        
        if(result==null)
        {
            var content = msgSystemError;
            alert(content);   
          
        }
        else if((result.length==2)&&(result[0]==SUCCESSRET))
        {
            printInfo = result[1];
//            if (document.getElementById("<%=firstLineMode.ClientID%>").value == "0") {

//            } else {
//                 
//            }
//            
    
          
        }
        else 
        {
            alert(result[0]);
            var content =result[0];          
        
        }
   } catch(e) {
        alert(e.description);
        
   }
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onFail
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	调用web service失败
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onFail(error)
{
   try {
       
        alert(error.get_message());
      
    } catch(e) {
        alert(e.description);
       
    }
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	locateCell
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	在cell里面变为编辑框或者下拉框
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function locateCell(currentCell)
{
   
    if(currentCell.tagName=="TD")
    {
  
        var wid = currentCell.clientWidth;
        var hid = currentCell.clientHeight; 
        
        var pNode = currentCell.parentNode;
        var rowIndex = pNode.rowIndex;
        var cellIndex = currentCell.cellIndex;
       
        if (pNode.cells[0].innerHTML == "&nbsp;") {
            return;
        }
        if ((currentCell == pNode.cells[0]) || (currentCell == pNode.cells[2]))
        {
            return;
        }
//        if ((currentCell == pNode.cells[5]) && (pNode.cells[7].innerHTML == "0"))
//        {
//            return;
//        }
         
        if ((currentCell == pNode.cells[1]) || (currentCell == pNode.cells[5])) {
            //用单元格的值来填充select的值
           var sel = document.createElement("select"); 
           sel.id = "row:"+ rowIndex + ";cell:" + cellIndex + "sel";
           sel.style.width = (wid-3).toString()+'px';
           sel.style.height = (hid-3).toString()+'px';
           sel.options.length=0
           var currentValue = GetAndSetinnerText(currentCell); //currentCell.innerText;
           var selectedIndex = 0;
       
           if (currentCell == pNode.cells[5]) {
                 
               if (pNode.cells[7].innerHTML == "0") {
                   for(i=0; i<portList.length; i++)
                   {
                        if (currentValue == portList[i]) {
                            selectedIndex = i;                           
                        }
                        sel.options.add(new Option(portList[i], portList[i]));                       
                      
                   }
                } else {
                   for(i=0; i<printerlList.length; i++)
                   {
                        
                        if (currentValue == printerlList[i]) {
                            selectedIndex = i;                           
                        }
                        sel.options.add(new Option(printerlList[i], printerlList[i]));                       
                      
                   }
               }
           } else {
          
               
                for (i=0; i<printInfo.length; i++)
                {
                   if (pNode.cells[0].innerHTML == printInfo[i].labelType) {
                       for(j=0; j<printInfo[i].templateList.length; j++)
                       {
                            
                            if (currentValue == printInfo[i].templateList[j].id) {
                                selectedIndex = j;                               
                            }
                           
                            sel.options.add(new Option(printInfo[i].templateList[j].id, printInfo[i].templateList[j].id));                           
                          
                       }
                       break;
                   }
               }
              
           }
           
          
           sel.selectedIndex = selectedIndex;
           currentCell.innerHTML="";
           currentCell.appendChild(sel);
//            alert("'"+ currentCell.innerHTML + "'");
         
      

            
        } else  {
            
           //用单元格的值来填充文本框的值
            var input = document.createElement("textarea");
            input.id = "row:"+ rowIndex + ";cell:" + cellIndex + "input";
            input.style.width = (wid-10).toString()+'px';
//            input.style.height = hid;
            
            input.value=currentCell.innerHTML.trim();
//            alert("'"+ input.value + "'");
        
            if (input.value == "&nbsp;")
            {
                input.value = "";
            }
            currentCell.innerHTML="";
            //把文本框加到当前单元格上.
            currentCell.appendChild(input);
     
//            alert("'"+ currentCell.innerHTML + "'");
     
                        

           
       } 
    }
}

      
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	editCell
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	编辑表格单元格
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

function editCell(event)
{
   
   var currentCell=window.event.srcElement;
    var pNode = currentCell.parentNode;
   locateCell(currentCell); 
}  
   
    
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	blurSel
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	编辑完毕单元格中的下拉框
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     
function blurSel(currentCell,selId)
{
         
    //充文本框的值给当前单元格
    var obj=document.getElementById(selId);
    var index=obj.selectedIndex; //序号，取当前选中选项的序号
   
    if (index != -1)
    {
        var val = obj.options[index].text;        
       
        if(val == "")
        {
            currentCell.innerHTML="";
            currentCell.title = "";
        }
        else
        {
//        <bug>
//            BUG NO:ITC-1122-0228
//            REASON:网页会把多个连续空格替换成一个空格
//        </bug>
            currentCell.innerHTML = val.replace(/ /g, '&nbsp;');
            currentCell.title = val;
        }
     } else {
        currentCell.innerHTML="";
        currentCell.title = "";
    }
}  
    
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	blurInput
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	编辑完毕单元格中的文本框
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function blurInput(currentCell,inputId)
{
    //充文本框的值给当前单元格
    var input = document.getElementById(inputId);
    var temp = input.value;
    if(temp == "")
    {
        currentCell.innerHTML="";
        currentCell.title = "";
    }
    else
    {
        currentCell.innerHTML = temp;
        currentCell.title = temp;
    }
}  

    
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ok
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	点击OK按钮
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  
function ok()
{
     var rowCount=document.getElementById (gvClientID).rows.length;
     var labelName;
     var xOffset;
     var yOffset;
     var printer;
     var ruleMode;
     var printMode;
     var template;
     var errorFlag = false;
     
    
     //第一次循环是做校验 
     for (i=0; i<rowCount-1; i++) {
      

        if (document.getElementById (gvClientID+'_'+i).cells[0].innerText.trim() != "") {
            labelName = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[0]); //.innerText;
            template = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[1]).trim();//.innerText.trim();
            xOffset = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[3]).trim();   //.innerText.trim();
            yOffset = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[4]).trim();    //.innerText.trim();
            printer = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[5]).trim(); //  .innerText.trim();
            ruleMode = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[6]);//.innerText;
            printMode = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[7]);//.innerText;
            //alert(labelName + ":" + template + ":" + xOffset + ":" + yOffset + ":" + printer + ":" + ruleMode + ":" + printMode);
            if (printMode == "0") {
                //BAT方式
              
                if (template == "") {
                    alert(msgNoSetBat);                
                    errorFlag = true;
                } else if (xOffset == "") {
                    alert(msgNoSetOffset);
                    errorFlag = true;                  
                } else if (yOffset == "") {
                    alert(msgNoSetOffset);
                    errorFlag = true;                
                } else if (checkOffset(xOffset)) {
                    alert(msgIllegalXOffset);                 
                    errorFlag = true;
                } else if (checkOffset(yOffset)) {
                    alert(msgIllegalYOffset);
                    errorFlag = true;               
                } else if (printer == "") {
                    alert(msgNoSetPort);
                    errorFlag = true;
                }
                

               
            } else {
                //模板方式
                if (xOffset == "") {
                    alert(msgNoSetOffset);
                    errorFlag = true;                 
                } else if (yOffset == "") {
                    alert(msgNoSetOffset);
                    errorFlag = true;                 
                } else if (checkOffset(xOffset)) {
                    alert(msgIllegalXOffset);
                    errorFlag = true;                  
                } else if (checkOffset(yOffset)) {
                    alert(msgIllegalYOffset);
                    errorFlag = true;                   
                } else if (printer == "") {
                    alert(msgNoSetPrinter);
                    errorFlag = true;                    
                }
            }
            
            if (errorFlag) {
                
               if (hilightRowIndex != i+1) {
                    SetOtherHilightRow(i+1);
                    eval("setScrollTopForGvExt_"+gvClientID+"('"+labelName+"',0)"); 
               }
               
                        
                break;
            } 
        }
     } 
     if (!errorFlag) {
       //1.校验无误，将当前高亮行control的数据写回cell中
       if (hilightRowIndex != -1) {
             setInvalidHilightRow();
         }
      
        //3.,第二次循环进行cookie设定
        eval("setRowNonSelected_"+gvClientID+"()"); 
        for (i=0; i<rowCount-1; i++) {
         
            if (GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[0]).trim() != "") {
                labelName = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[0]);//.innerText;
                template = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[1]);//.innerText;
                xOffset = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[3]);//.innerText;
                yOffset = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[4]);//.innerText;
                printer = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[5]); //.innerText;
                ruleMode = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[6]);//.innerText;
                printMode = GetAndSetinnerText(document.getElementById (gvClientID+'_'+i).cells[7]);//.innerText;
                if (printMode == "0") {
                    //BAT方式                 
//                    printer = "";
                } else {
                    //模板方式
                    template = "";                       
                }
             
                setOffset("<%=pcode%>",labelName,template,printMode,ruleMode,parseInt(xOffset,10),parseInt(yOffset,10),escape(printer));
             
               
            }
    
        }
        window.close();
          
    }

}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	cancel
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	点击Cancel按钮
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~     
function cancel()
{
    window.close();

}
 

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkOffset
//| Author		:	Lucy Liu
//| Create Date	:	01/07/2010
//| Description	:	check Offset的合法性
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkOffset(value)
{
	var errorFlag = false;
	try 
    {
//    <bug>
//        BUG NO:ITC-1103-0176
//        REASON:整数
//    </bug>
	   errorFlag = !(parseInt(+value)+'' == value);
	   return errorFlag;
	}
    catch (e)
    {
	    alert(e.description);
    }   
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	HighLightNewMOList
//| Author		:	Lucy Liu
//| Create Date	:	02/21/2010
//| Description	:	高亮NewMO List的第一行
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function HighLightNewMOList()
{
    eval("setRowSelectedByIndex_" + gvClientID + "(0,false,gvClientID);");
    
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	CellToControl
//| Author		:	Lucy Liu
//| Create Date	:	02/21/2010
//| Description	:	cell变为control
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function CellToControl(rowIndex,cellIndex)
{
   var table=document.getElementById(gvClientID); 
   var currentCell = table.rows[rowIndex].cells[cellIndex];
   locateCell(currentCell);    
    
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ControlToCell
//| Author		:	Lucy Liu
//| Create Date	:	02/21/2010
//| Description	:	control变为cell
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ControlToCell(rowIndex,cellIndex,inputType)
{
   var table=document.getElementById(gvClientID); 
   var currentCell = table.rows[rowIndex].cells[cellIndex];
   var controlId ;
   if (inputType == "input" ) {
        controlId = "row:" + rowIndex + ";cell:" + cellIndex + "input";
        blurInput(currentCell,controlId); 
   } else {
        controlId = "row:" + rowIndex + ";cell:" + cellIndex + "sel";
        blurSel(currentCell,controlId);
   }

 
    
}
////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
////| Name		:	onMORowClick
////| Author		:	Lucy Liu
////| Create Date	:	02/21/2010
////| Description	:	单击MO List表格一行
////| Input para.	:	
////| Ret value	:	
////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//function onMORowClick(row)
//{
//    
//    var key = row.cells[0].innerText.trim();
//    var printMode = row.cells[7].innerText.trim();
//    
//    
//    if (key != "") {
//        var currentHiRowIndex = parseInt(row.index,10)+1;
//        if (hilightRowIndex != currentHiRowIndex) {
//            //当所选的高亮行不是原来的高亮行时，执行以下动作
//            
//            //1.置新的高亮行状态
//            eval("setCvExtRowSelected_" + gvClientID + "(row);");
//           
//            //置这行每列的显示控件，将这行tabel cell的值写入各自的控件中
//            if (printMode == "0") {
//                CellToControl(currentHiRowIndex,1);
//                CellToControl(currentHiRowIndex,3);
//                CellToControl(currentHiRowIndex,4);
//              
//            } else {
//                CellToControl(currentHiRowIndex,1);
//                CellToControl(currentHiRowIndex,3);
//                CellToControl(currentHiRowIndex,4);
//                CellToControl(currentHiRowIndex,5);
//                
//            }
//            
//            //2.将原来的高亮行控件值写回table cell中
//            if (hilightRowIndex != -1) {
//                var table=document.getElementById(gvClientID); 
//                var lastPrintMode = table.rows[hilightRowIndex].cells[7].innerText.trim();
//                if (lastPrintMode == "0") {
//                    ControlToCell(hilightRowIndex,1,"sel");
//                    ControlToCell(hilightRowIndex,3,"input");
//                    ControlToCell(hilightRowIndex,4,"input");                   
//                 } else {
//                    ControlToCell(hilightRowIndex,1,"sel");
//                    ControlToCell(hilightRowIndex,3,"input");
//                    ControlToCell(hilightRowIndex,4,"input");
//                    ControlToCell(hilightRowIndex,5,"sel");
//                 }
//            }
//            //3.保存当前高亮行数至hilightRowIndex变量中
//            hilightRowIndex = currentHiRowIndex;
//         } else {
//           //当所选的高亮行是原来的高亮行时，不执行任何
//         }
//    
//      
//    } else {
//        //当所选的高亮行不是有效数据时，执行以下动作
//        //1.将原来的高亮行控件值写回table cell中
//         if (hilightRowIndex != -1) {
//              var table=document.getElementById(gvClientID); 
//              var lastPrintMode = table.rows[hilightRowIndex].cells[7].innerText.trim();
//              if (lastPrintMode == "0") {
//                ControlToCell(hilightRowIndex,1,"sel");
//                ControlToCell(hilightRowIndex,3,"input");
//                ControlToCell(hilightRowIndex,4,"input");                   
//             } else {
//                ControlToCell(hilightRowIndex,1,"sel");
//                ControlToCell(hilightRowIndex,3,"input");
//                ControlToCell(hilightRowIndex,4,"input");
//                ControlToCell(hilightRowIndex,5,"sel");
//             }
//         }
//         //2.hilightRowIndex=-1;
//        hilightRowIndex = -1;
//        //3.表中无任何高亮行可选择
//        eval("setRowNonSelected_"+gvClientID+"()"); 
//        
//    }
//   
//   
//}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	SetOtherHilightRow
//| Author		:	Lucy Liu
//| Create Date	:	02/21/2010
//| Description	:	当选择的有效高亮行不是之前那一行时所作的处理
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function SetOtherHilightRow(currentHiRowIndex) 
{
	
   var table=document.getElementById(gvClientID); 
   var  printMode = GetAndSetinnerText(table.rows[currentHiRowIndex].cells[7]).trim(); //.innerText.trim();
    //1.置这行每列的显示控件，将这行tabel cell的值写入各自的控件中
    if (printMode == "0") {
        CellToControl(currentHiRowIndex,1);
        CellToControl(currentHiRowIndex,3);
        CellToControl(currentHiRowIndex,4);
        CellToControl(currentHiRowIndex,5);
      
    } else {
//        CellToControl(currentHiRowIndex,1);
        CellToControl(currentHiRowIndex,3);
        CellToControl(currentHiRowIndex,4);
        CellToControl(currentHiRowIndex,5);
        
    }
    
    //2.将原来的高亮行控件值写回table cell中
    if (hilightRowIndex != -1) {
        var lastPrintMode = GetAndSetinnerText(table.rows[hilightRowIndex].cells[7]).trim(); //.innerText.trim();
        if (lastPrintMode == "0") {
            ControlToCell(hilightRowIndex,1,"sel");
            ControlToCell(hilightRowIndex,3,"input");
            ControlToCell(hilightRowIndex,4,"input");                   
            ControlToCell(hilightRowIndex,5,"sel");                   
         } else {
//            ControlToCell(hilightRowIndex,1,"sel");
            ControlToCell(hilightRowIndex,3,"input");
            ControlToCell(hilightRowIndex,4,"input");
            ControlToCell(hilightRowIndex,5,"sel");
         }
    }
    //3.保存当前高亮行数至hilightRowIndex变量中
    hilightRowIndex = currentHiRowIndex;
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	setInvalidHilightRow
//| Author		:	Lucy Liu
//| Create Date	:	02/21/2010
//| Description	:	当选择的不是有效的高亮行时所作的处理
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function setInvalidHilightRow()
{
	if (hilightRowIndex != -1) {
          var table=document.getElementById(gvClientID); 
          var lastPrintMode = GetAndSetinnerText(table.rows[hilightRowIndex].cells[7]).trim();  //.innerText.trim();
          if (lastPrintMode == "0") {
            ControlToCell(hilightRowIndex,1,"sel");
            ControlToCell(hilightRowIndex,3,"input");
            ControlToCell(hilightRowIndex,4,"input");                   
            ControlToCell(hilightRowIndex,5,"sel");                   
         } else {
//            ControlToCell(hilightRowIndex,1,"sel");
            ControlToCell(hilightRowIndex,3,"input");
            ControlToCell(hilightRowIndex,4,"input");
            ControlToCell(hilightRowIndex,5,"sel");
         }
     }
    //2.hilightRowIndex=-1;
    hilightRowIndex = -1;
   
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onMORowClick
//| Author		:	Lucy Liu
//| Create Date	:	02/21/2010
//| Description	:	单击MO List表格一行
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onMORowClick(row)
{
    
    //        <bug>
    //            BUG NO:ITC-1103-0314
    //            REASON:单击表格一行，显示控件
    //        </bug>
    var key = GetAndSetinnerText(row.cells[0]).trim(); //.innerText.trim();
    var printMode = GetAndSetinnerText(row.cells[7]).trim(); //.innerText.trim();
    
    
    if (key != "") {
        var currentHiRowIndex = parseInt(row.index,10)+1;
        if (hilightRowIndex != currentHiRowIndex) {
            //当所选的高亮行不是原来的高亮行时，执行以下动作
            
            //置这行每列的显示控件，将这行tabel cell的值写入各自的控件中
            SetOtherHilightRow(currentHiRowIndex);
            //置新的高亮行状态
            eval("setCvExtRowSelected_" + gvClientID + "(row);");
           
	        
         
         } else {
           //当所选的高亮行是原来的高亮行时，不执行任何
         }
    
      
    } else {
        //当所选的高亮行不是有效数据时，执行以下动作
        //1.将原来的高亮行控件值写回table cell中
	    setInvalidHilightRow();
        //2.表中无任何高亮行可选择
        eval("setRowNonSelected_"+gvClientID+"()"); 
        
    }
   
   
}
function setCookie(labelname,type,value) 
{
    var secs=8000;
    
    var date = new Date();
    date.setTime(date.getTime()+(secs*1000));
    var expires = "; expires="+date.toGMTString();
    //modified by itc207013,fix cookie name 
    document.cookie ="FIS" + "<%=pcode%>" + labelname + type + "="+escape(value)+expires+"; path=/"; 
  
 }  


function getCookie(labelname, type){ 
    var strCookie = document.cookie; 
    var arrCookie = strCookie.split(";"); 
    var name = "FIS" + "<%=pcode%>" + labelname + type;
    alert(name);
    for(var i = 0; i < arrCookie.length; i++)
    { 
        var arr = arrCookie[i].split("="); 
       
        if(arr[0].trim()==name)
        {
            return unescape(arr[1]); 
        }
    } 
    return ""; 
}

//-->
</SCRIPT>     

    
    <div>
        <TABLE cellpadding="0px" cellspacing="5px" border="0" width = "100%" height="100%">
<%--   <bug>
        BUG NO:ITC-1103-0187
        REASON:去掉站名提示
  </bug>--%>
       <%--<%-- <TR>
	        <TD><asp:Label ID="title" runat="server"   CssClass="iMes_label_13pt"></asp:Label><asp:Label ID="station" runat="server"   CssClass="iMes_label_13pt"></asp:Label></TD>
        </TR>--%>
        <TR>
	        <TD>
	       <%-- <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true"
                    GetTemplateValueEnable="False" GvExtHeight="240px" GvExtWidth="100%" OnGvExtRowClick="" Width="99.9%" 
                    OnGvExtRowDblClick="" SetTemplateValueEnable="False" HighLightRowPosition="1"  Height="230px"
                     onrowdatabound="GridViewExt1_RowDataBound" >--%>
            <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true" GvExtWidth="100%"  GvExtHeight="240px" 
                  Width="99.9%" Height="230px" OnGvExtRowClick="onMORowClick(this)"
                SetTemplateValueEnable="true"  GetTemplateValueEnable="true"  HighLightRowPosition="3" 
               onrowdatabound="GridViewExt1_RowDataBound" HorizontalAlign="Left" >
                    <Columns>
                        <asp:BoundField DataField="LabelType"  />
                        <asp:BoundField DataField="Template"  />
                        <asp:BoundField DataField="Mode"  />
                        <asp:BoundField DataField="X"  />
                        <asp:BoundField DataField="Y"  />
                        <asp:BoundField DataField="Printer"  />
                        <asp:BoundField DataField="RuleMode"  />
                        <asp:BoundField DataField="PrintMode"  />
                    </Columns>
                 </iMES:GridViewExt>
	        </TD>
        </TR>
         <TR>
	        <TD>
	        <asp:Label ID="lblSettingTip1" runat="server"   CssClass="iMes_label_13pt"></asp:Label><br />
	        <asp:Label ID="lblSettingTip2" runat="server"   CssClass="iMes_label_13pt"></asp:Label>
	        </TD>
        </TR>
        <TR>
	        <TD align="right">
	         <input id="btnOK" type="button"  onclick="ok()" class="iMes_button" runat="server" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />&nbsp;&nbsp;
             <input id="btnCancel" type="button"   onclick="cancel()" class="iMes_button" runat="server" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
             
	        </TD>
        </TR>
        </TABLE>
    </div>                    
       
    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
        <ContentTemplate>
             <input type="hidden" runat="server" id="stationHidden" />
             <input type="hidden" runat="server" id="pCodeHidden" />
             <input type="hidden" runat="server" id="firstLineMode" />
        </ContentTemplate>   
    </asp:UpdatePanel> 
    </form>
    
     
</body>
</html>



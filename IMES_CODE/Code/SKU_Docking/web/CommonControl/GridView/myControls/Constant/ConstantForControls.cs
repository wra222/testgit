/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description:gridviewExt control
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2008-10-10  Zhao Meili(EB2)        Create 
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace myControls.Constant
{
    class ConstantForControls
    {
        //默认高亮滚动条位置定位
        internal const string ScrollDefaultPosition = "1";
 
        internal const string JSSetScrollTopByColValueForGvExt= @"<script type=""text/javascript"">   
      
        var fixPosition='" + ScrollDefaultPosition + "';" + @"

        function setScrollTopForGvExt(inputValue,colname,controlType,spliter,secondColIndex,maxcount,overflowMessage)
        {
            var gvClientID='ForReplace';
            var lightPosition;            
            var headerHeight=document.getElementById (gvClientID).rows[0].clientHeight;
            var dataRowHeight=document.getElementById (gvClientID).rows[1].clientHeight;  
            //check if fixPosition is a number
            if(IsNumber(fixPosition))
            {
                lightPosition=eval(fixPosition);
            }
            
            var HDMultiple=headerHeight/dataRowHeight;
            var row;
            if(spliter==null)
            {
                row=getSearchedRow(inputValue,colname,gvClientID,controlType);
                if(row!=null)
                {
                    setCvExtRowSelected(row);
                }
            }
            else if(spliter=='MUTISELECT')
            {
                row=getSearchedRow(inputValue,colname,gvClientID,controlType);
                if(row!=null)
                {
                    setCvExtRowSelected(row,true);
                }
            }
            else if(spliter=='SHIPPING20')
            {
                //set former row non selected
                setRowNonSelected();
                //search and set row selected, return the last row that found
                row=setMultiSearchedRowAndReturnLastRow(inputValue,colname,gvClientID,controlType);              
            }
            else
            {
                row=getArrayValueSearchedRow(inputValue,colname,gvClientID,controlType,spliter,secondColIndex,maxcount,overflowMessage);
                 if(row!=null)
                {
                    setCvExtRowSelected(row);
                }
            }
            if(row!=null)
            {
                 //set row highlight
                var num=eval(row.index);      
                //setCvExtRowSelected(row);
                //set scrollTop
                var rowCount=document.getElementById (gvClientID).rows.length;
                var sHeight=document.getElementById ('div_'+gvClientID).scrollHeight;
                var perRowScolMove=sHeight/(HDMultiple+rowCount-1);
                //fix highlight row positon,-1 then move downwards,+1 upwards
                document.getElementById ('div_'+gvClientID).scrollTop =perRowScolMove*(num+HDMultiple+(-lightPosition+1));
            }
            return row;
        }

        function setSrollByIndex(rowIndex,isHighlight,gdClientID)
        {
            var gvClientID='ForReplace';
            if(gdClientID!=null)
            {
                gvClientID=gdClientID;
            }
            var lightPosition;            
            var headerHeight=document.getElementById (gvClientID).rows[0].clientHeight;
            var dataRowHeight=document.getElementById (gvClientID).rows[1].clientHeight;  
            //check if fixPosition is a number
            if(IsNumber(fixPosition))
            {
                lightPosition=eval(fixPosition);
            }
            
            var HDMultiple=headerHeight/dataRowHeight;
            var num=eval(rowIndex); 
            if((isHighlight!=null)&&(isHighlight))
            {
                setRowSelectedByIndex(num,false,gdClientID);
            }     
            //set scrollTop
            var rowCount=document.getElementById (gvClientID).rows.length;
            var sHeight=document.getElementById ('div_'+gvClientID).scrollHeight;
            var perRowScolMove=sHeight/(HDMultiple+rowCount-1);
            //fix highlight row positon,-1 then move downwards,+1 upwards
            document.getElementById ('div_'+gvClientID).scrollTop =perRowScolMove*(num+HDMultiple+(-lightPosition+1));
        
        }
           //]]>
    </script>
    ";

    internal const string JsForGvExtRevokeMethod = @"<script type=""text/javascript"">  

        //judge if a string value is make up with  number character
        function IsNumber(str)
        {
             var  pattern=/^[\d]+$/;
             return !(!pattern.exec(str));
        }

        //search the row that contain inputValue,and return the result row
        function getSearchedRow(inputValue,colname,gvClientID,controlType)
        {
            var row=null;
            var rowCount=document.getElementById (gvClientID).rows.length;
            if((controlType==null) || (controlType==''))
            {
                var cellsValue;
                for(var i=0;i<rowCount-1;i++)
                { 
                    cellsValue=document.getElementById (gvClientID+'_'+i).cells[colname].innerText
                    if(cellsValue==inputValue)
                    {
                        row=document.getElementById (gvClientID+'_'+i);
                        break;
                    }           
                }   
            }
            else
            {
                var cellNamePrefix=""" + "document.getElementById (gvClientID+'_'+i+'_'+colname)." + @""";
                var valueProperty=getValuePropertyName(controlType);

                for(var i=0;i<rowCount-1;i++)
                {  
                    if(eval(cellNamePrefix+valueProperty)==inputValue)
                    {
                        row=document.getElementById (gvClientID+'_'+i);
                        break;
                    }           
                }
            }
            return row;
        }

        function setMultiSearchedRowAndReturnLastRow(inputValue,colname,gvClientID,controlType)
        {
            var row=null;
            var rowCount=document.getElementById (gvClientID).rows.length;
            if((controlType==null) || (controlType==''))
            {
                var cellsValue;
                for(var i=0;i<rowCount-1;i++)
                { 
                    cellsValue=document.getElementById (gvClientID+'_'+i).cells[colname].innerText
                    if(cellsValue==inputValue)
                    {
                        row=document.getElementById (gvClientID+'_'+i);
                        setCvExtRowSelected(row,true);
                    }           
                }   
            }
            else
            {
                var cellNamePrefix=""" + "document.getElementById (gvClientID+'_'+i+'_'+colname)." + @""";
                var valueProperty=getValuePropertyName(controlType);

                for(var i=0;i<rowCount-1;i++)
                {  
                    if(eval(cellNamePrefix+valueProperty)==inputValue)
                    {
                        row=document.getElementById (gvClientID+'_'+i);
                        setCvExtRowSelected(row,true);
                    }           
                }
            }
            return row;
        }
 
        function getArrayValueSearchedRow(inputValue,colname,gvClientID,controlType,spliter,secondColIndex,maxcount,overflowMessage)
        {
            var row=null;
            var rowCount=document.getElementById (gvClientID).rows.length;
            if((controlType==null) || (controlType==''))
            {
                var cellsValue;
                for(var i=0;i<rowCount-1;i++)
                { 
                    cellsValue=document.getElementById (gvClientID+'_'+i).cells[colname].innerText
                    var valueArray=cellsValue.split(spliter);
                    var usedCntArray=document.getElementById (gvClientID+'_'+i).cells[secondColIndex].innerText.split(spliter);
                    for(var arrayIndex=0;arrayIndex<valueArray.length;arrayIndex++)
                    {               
                        if(valueArray[arrayIndex]==inputValue)
                        {
                            row=document.getElementById (gvClientID+'_'+i);
                            if(!(usedCntArray[arrayIndex]<maxcount))
                            {
                                ShowMessage(overflowMessage);
                                TextAreaChange(overflowMessage);
                            }
                            return row;
                        }        
                    }   
                }   
            }
            else
            {
                var cellsValue;
                var cellNamePrefix=""" + "cellsValue=document.getElementById (gvClientID+'_'+i+'_'+colname)." + @""";
                var valueProperty=getValuePropertyName(controlType);

                for(var i=0;i<rowCount-1;i++)
                {  
                    eval(cellNamePrefix+valueProperty);
                    var valueArray=cellsValue.split(spliter);
                    for(var arrayIndex=0;arrayIndex<valueArray.length;arrayIndex++)
                    {               
                        if(valueArray[arrayIndex]==inputValue)
                        {
                            row=document.getElementById (gvClientID+'_'+i);
                            return row;
                        }  
                    }         
                }
            }
          return row;
        }

        function getValuePropertyName(controlType)
        {
            var valuePropertyName='';
            if(controlType==null || controlType==''|| controlType.toLowerCase()=='label' )
            {
                valuePropertyName='innerText';
            }
            else if(controlType.toLowerCase()=='textbox'||controlType.toLowerCase()=='hidden')
            {
                valuePropertyName='value';
            }
            else
            {
                 alert('The control type:'+controlType +'is not supported');
            }
            return valuePropertyName;
        }

           //]]>
    </script>
    ";

    internal const string JSsetTemplateCellValue = @"<script type=""text/javascript"">  
    function setTemplateCellValueToCvExt(dataRowIndex,colName,theValue,controlType)
    { 
        if(theValue!=null)
        { 
            var gvClientID='ForReplace';
            var cellNamePrefix=""" + "document.getElementById (gvClientID+'_'+dataRowIndex+'_'+colName)." + @""";
            var origalCssName=document.getElementById (gvClientID+'_'+dataRowIndex).className;
            var valueProperty=getValuePropertyName(controlType);
            if(eval(cellNamePrefix+valueProperty)!=null)
            {
                eval(cellNamePrefix+valueProperty+""='""+theValue+""'"");
                document.getElementById (gvClientID+'_'+dataRowIndex).className=origalCssName;
            }
        }
    } 
           //]]>
    </script>
    ";

    internal const string JSgetTemplateCellValue = @"<script type=""text/javascript"">  
    function getTemplateCellValueFromCvExt(dataRowIndex,colName,controlType)
    { 
        var retValue=null; 
        var gvClientID='ForReplace';
        var cellNamePrefix=""" + "document.getElementById (gvClientID+'_'+dataRowIndex+'_'+colName)." + @""";
        var valueProperty=getValuePropertyName(controlType);
        if(eval(cellNamePrefix+valueProperty)!=null)
        {
            retValue=eval(cellNamePrefix+valueProperty);
        } 
        return retValue;
    } 
           //]]>
    </script>
    ";
    internal const string JSsetCvExtRowSelected = @"<script type=""text/javascript"">  
    function setRowSelectedByIndex(dataRowIndex,mutiSelected,gdClientID)
    {
        var gvClientID='ForReplace';
        if(gdClientID!=null)
        {
            gvClientID=gdClientID;
        }
        var alterRowCssNameForGvExt = 'ForReplace';
        var rowCssNameForGvExt ='ForReplace';
        var selectedRowCssNameForGvExt = 'ForReplace';
        var rowCount=document.getElementById (gvClientID).rows.length;
        for(var i=1;i<rowCount;i++)
        {
            if((mutiSelected==null)||(!mutiSelected))
            {
                if(document.getElementById (gvClientID).rows[i].className==selectedRowCssNameForGvExt)
                {
                    if(i%2==1)
                    {
                        document.getElementById (gvClientID).rows[i].className=rowCssNameForGvExt;
                    }
                    else
                    {
                        document.getElementById (gvClientID).rows[i].className=alterRowCssNameForGvExt;
                    }
                    break;
                }
            }
        }
        var index =eval(dataRowIndex);
        document.getElementById (gvClientID).rows[index+1].className=selectedRowCssNameForGvExt;
    } 

    function setRowSelectedOrNotSelectedByIndex(dataRowIndex,isSelected,gdClientID)
    {
        var gvClientID='ForReplace';
        if(gdClientID!=null)
        {
            gvClientID=gdClientID;
        }
        var alterRowCssNameForGvExt = 'ForReplace';
        var rowCssNameForGvExt ='ForReplace';
        var selectedRowCssNameForGvExt = 'ForReplace';
        var rowCount=document.getElementById (gvClientID).rows.length;
        var index =eval(dataRowIndex);
        if(isSelected)
        {
                document.getElementById (gvClientID).rows[index+1].className=selectedRowCssNameForGvExt;
        }
        else if((index+1)%2==1)
        {
            document.getElementById (gvClientID).rows[index+1].className=rowCssNameForGvExt;
        }
        else
        {
            document.getElementById (gvClientID).rows[index+1].className=alterRowCssNameForGvExt;
        }  
    } 

    function setCvExtRowSelected(row,mutiSelected)
    {
        if(row!=null)
        {
            setRowSelectedByIndex(row.index,mutiSelected);
        }
    }
    
    function AddCvExtRowToBottom(rowArray,setSelected,encodeColIndexArray,enableWrap,enableToolTip)
    {   
        if(rowArray!=null)
        {
            var gvClientID='ForReplace';
            var alterRowCssNameForGvExt = 'ForReplace';
            var rowCssNameForGvExt ='ForReplace';
            var selectedRowCssNameForGvExt = 'ForReplace';
            var onclickEvent = 'ForReplace';
            var ondblclickEvent = 'ForReplace';
            var table=document.getElementById (gvClientID);
            var rowCount=table.rows.length;
            var colCount=table.rows[0].cells.length; 
            var newgvRow=table.insertRow();
            if(enableWrap==null)
            {
                enableWrap=true;
            }
            if(enableToolTip==null)
            {
                enableToolTip=false;
            }
            for(var i=0;i<colCount;i++)
            {
                newgvRow.insertCell();
                if(encodeColIndexArray!=null)
                {   
                    if(isEncodeIndex(i,encodeColIndexArray))
                    {
                        newgvRow.cells[i].innerHTML=rowArray[i].toString();
                    }
                    else
                    {
                         newgvRow.cells[i].innerText=rowArray[i].toString();
                         if(enableToolTip)
                         {
                             newgvRow.cells[i].title=rowArray[i].toString();
                         }
                        if(enableWrap)
                        {
                             newgvRow.cells[i].runtimeStyle.wordBreak='break-all'; 
                             newgvRow.cells[i].runtimeStyle.wordWrap='break-word';
                
                        }
                        else
                        {
                             newgvRow.cells[i].runtimeStyle.wordBreak='keep-all'; 
                             newgvRow.cells[i].runtimeStyle.wordWrap='normal';
                             newgvRow.cells[i].runtimeStyle.whiteSpace='nowrap';
                        }
                    }
                }
                else
                {
                     newgvRow.cells[i].innerText=rowArray[i].toString();
                     if(enableToolTip)
                     {
                         newgvRow.cells[i].title=rowArray[i].toString();
                     }
                    if(enableWrap)
                    {
                         newgvRow.cells[i].runtimeStyle.wordBreak='break-all'; 
                         newgvRow.cells[i].runtimeStyle.wordWrap='break-word';
                    }
                    else
                    {
                         newgvRow.cells[i].runtimeStyle.wordBreak='keep-all'; 
                         newgvRow.cells[i].runtimeStyle.wordWrap='normal';
                         newgvRow.cells[i].runtimeStyle.whiteSpace='nowrap';
                    }
                }
            }
            if(setSelected)
            {
                for(var i=1;i<rowCount;i++)
                {
                    if(document.getElementById (gvClientID).rows[i].className==selectedRowCssNameForGvExt)
                    {
                        if(i%2==1)
                        {
                            document.getElementById (gvClientID).rows[i].className=rowCssNameForGvExt;
                        }
                        else
                        {
                            document.getElementById (gvClientID).rows[i].className=alterRowCssNameForGvExt;
                        }
                        break;
                    }
                }
                newgvRow.className=selectedRowCssNameForGvExt;
            }
            else
            {
                if((rowCount)%2==1)
                {
                    newgvRow.className=rowCssNameForGvExt;
                }
                else
                {
                    newgvRow.className=alterRowCssNameForGvExt;
                }
            }
            newgvRow.onclick= function(){   
                eval(onclickEvent); 
              }   

            newgvRow.ondblclick= function(){   
                eval(ondblclickEvent); 
              }   
            newgvRow.setAttribute('id',gvClientID + '_'+(rowCount-1).toString());
            newgvRow.setAttribute('index',(rowCount-1).toString());
            return newgvRow;
        }
    } 

    function ChangeCvExtRowByIndex(rowArray,setSelected,rowIndex,encodeColIndexArray)
    {        
         if (rowArray != null) 
        {
            var gvClientID='ForReplace';
            var selectedRowCssNameForGvExt = 'ForReplace';
            var table=document.getElementById (gvClientID);
            var rowCount=table.rows.length;
            var colCount=table.rows[0].cells.length;
            var newgvRow=table.rows[rowIndex];
            var origalCssName=table.rows[rowIndex].className;
            for(var i=0;i<colCount;i++)     
            {
                if(encodeColIndexArray!=null)
                {   
                    if(isEncodeIndex(i,encodeColIndexArray))
                    {
                        newgvRow.cells[i].innerHTML=rowArray[i].toString();
                    }
                    else
                    {
                         newgvRow.cells[i].innerText=rowArray[i].toString();
                         newgvRow.cells[i].runtimeStyle.wordBreak='break-all'; 
                         newgvRow.cells[i].runtimeStyle.wordWrap='break-word';
                    }
                }
                else
                {
                     newgvRow.cells[i].innerText=rowArray[i].toString();
                     newgvRow.cells[i].runtimeStyle.wordBreak='break-all'; 
                     newgvRow.cells[i].runtimeStyle.wordWrap='break-word';
                }
            }
                         
            if(setSelected) 
            {
                setRowSelectedByIndex(newgvRow.index);
            }
            else
            {
                 newgvRow.className=origalCssName;
            }
            return newgvRow;
        }
    } 

    function isEncodeIndex(index,encodeColIndexArray)
    {
        for(var j=0;j<encodeColIndexArray.length;j++)     
        {
            if(encodeColIndexArray[j]==index)
            {
               return true;
            }
        }
        return false;
    }
    function setRowNonSelected()
    {
        var gvClientID='ForReplace';
        var alterRowCssNameForGvExt = 'ForReplace';
        var rowCssNameForGvExt ='ForReplace';
        var selectedRowCssNameForGvExt = 'ForReplace';
        var rowCount=document.getElementById (gvClientID).rows.length;
        for(var i=1;i<rowCount;i++)
        {
            if(document.getElementById (gvClientID).rows[i].className==selectedRowCssNameForGvExt)
            {
                if(i%2==1)
                {
                    document.getElementById (gvClientID).rows[i].className=rowCssNameForGvExt;
                }
                else
                {
                    document.getElementById (gvClientID).rows[i].className=alterRowCssNameForGvExt;
                }
            }
        }
    } 
           //]]>
    </script>
    ";
    
    }
}

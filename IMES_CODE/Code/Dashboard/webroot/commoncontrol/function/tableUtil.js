	//get all row fields .
	function getAllRowFieldValue(tableObj, fieldsArr, colDelim)
	{ 		  
		var rs = tableObj.rs_main;	   	 
		var allRows = new Array();
		
		if (tableObj.IsEmpty)
			return "";
 
		rs.moveFirst();	  
	 
		while(!rs.EOF)
		{ 
			allRows[allRows.length] = getRowFieldsValue(rs, fieldsArr, colDelim);		  	 
			rs.moveNext();			 
			
	    }
	     
		return allRows;

	} 
	
	//get hilight row fields.
	function getHilightRowFieldsValue(tableObj, fieldsArr, colDelim)
	{ 		  
		var rs = tableObj.rs_main;	   	 
		
		if (tableObj.IsEmpty)
			return ""; 
		
		if (tableObj.GetRowNumber() == -1)
		{
			alert("Please select one row!");
			return "";
		}		 
	    
		if(tableObj.GetRowNumber() >= rs.recordcount)
 				return false;
 					    
	    rs.AbsolutePosition = tableObj.GetRowNumber() +1;
		return getRowFieldsValue(rs, fieldsArr, colDelim);		  	 

	} 

	function getRowFieldsValue(rs, fieldsArr, colDelim)
	{
		var rowValues = "";
		
		for (var i =0; i< fieldsArr.length; i++)
		{
			if (i == fieldsArr.length-1)//the last one
				rowValues += rs.Fields(fieldsArr[i]);
			else
				rowValues += rs.Fields(fieldsArr[i]) + colDelim;
		 
		}		
		
		return rowValues;
	}
	
	function swapUp(tableObj, orderFldNum, colDelim)
	{
		var rs = tableObj.rs_main; 
		
		if (orderFldNum != null)
		{
		    if (typeof(orderFldNum) != "number")
		    {
			    alert("orderFldNum in swapDown() is not number.");
			    return false;
		    }
    		
		    if (orderFldNum > rs.Fields.Count)
		    {
			    alert("orderFldNum is out of range.");
			    return false;
		    }
		}
		
		if (noSelectedLine(tableObj))
			return false;
					
		var currentRowNum = tableObj.GetRowNumber();
		
		if (currentRowNum == 0)
			return false;					
			
	    //rs.AbsolutePosition = currentRowNum;
	    
	    var upRowNum = currentRowNum -1;	    
	    var currentRowString = tableObj.RowStr;
	    if(currentRowString==null||currentRowString=="")
	    {
	    	return false;
	    }
	    else
	    {
	    	var cuRowFields = currentRowString.split(colDelim);
	    }
	    //rs.AbsolutePosition = upRowNum;
	    tableObj.HighLightRow(upRowNum);
	    var upRowString = tableObj.RowStr;
	    if(upRowString==null||upRowString=="")
	    {
	    	return false;
	    }
	    else
	    {
	    	var upRowFields = upRowString.split(colDelim);	    
	    }
	    
//switch	  
		if (orderFldNum != null)
		{  
		    var tempOrderData = cuRowFields[orderFldNum];
	    //	alert("upRowFields[orderFldNum];=" + upRowFields[orderFldNum])
		    cuRowFields[orderFldNum] = upRowFields[orderFldNum];
		    upRowFields[orderFldNum] = tempOrderData;
    		
		    currentRowString = cuRowFields.join(colDelim);
		    upRowString =  upRowFields.join(colDelim);
		}
	//	alert(currentRowString)
//end switch	
	    
	    tableObj.HighLightRow(currentRowNum);
	    tableObj.SetRow(upRowString);
	    tableObj.HighLightRow(upRowNum);
   	    tableObj.SetRow(currentRowString);
	    
	    return true;	    
	}
	
	function swapDown(tableObj, orderFldNum, colDelim)
	{
		var rs = tableObj.rs_main; 
		
		if (orderFldNum != null)
		{ 		
		    if (typeof(orderFldNum) != "number")
		    {
			    alert("orderFldNum in swapDown() is not number.");
			    return false;
		    }
 
		    if (orderFldNum > rs.Fields.Count)
		    {
			    alert("orderFldNum is out of range.");
			    return false;
		    }
		    
		}    
		
	    if (noSelectedLine(tableObj))
		    return false;	
	    var currentRowNum = tableObj.GetRowNumber();
		  
	    if (currentRowNum >= rs.RecordCount-1)
		    return false;					
    		
	    //rs.AbsolutePosition = currentRowNum;
	       
	    var upRowNum = currentRowNum + 1;	
	     
	    var currentRowString = tableObj.RowStr;

	    //rs.AbsolutePosition = upRowNum;
	    tableObj.HighLightRow(upRowNum);
	    var upRowString = tableObj.RowStr;
		
//switch	 
		if (orderFldNum != null)
		{     
	        var upRowFields = upRowString.split(colDelim);	    
	        var cuRowFields = currentRowString.split(colDelim);
		    var tempOrderData = cuRowFields[orderFldNum];
		    //alert("upRowFields[orderFldNum];=" + upRowFields[orderFldNum])
		    cuRowFields[orderFldNum] = upRowFields[orderFldNum];
		    upRowFields[orderFldNum] = tempOrderData;
    		
		    currentRowString = cuRowFields.join(colDelim);
		    upRowString =  upRowFields.join(colDelim);
		}
		//alert(currentRowString)
//end switch		
	    
	    tableObj.HighLightRow(currentRowNum);
	    tableObj.SetRow(upRowString);
	    tableObj.HighLightRow(upRowNum);
   	    tableObj.SetRow(currentRowString);
	    
	    return true;	    
	}	
	
	function noSelectedLine(tableObj)
	{
		if (tableObj.GetRowNumber() == -1)
		{			 
			alert("Please select a row!");
			return true;
		}

		return false;
	}

//High light a row by id.
function fHighLightRow(id, colIndex, table){
	if (table == null || table == undefined){
		return;
	}
	
	if (id == "" || id == undefined){
		if (table.rs_main.recordcount > 0){
			table.HighLightRow(0);
		}
	} else{
		var row = fGetRowIndex(id, colIndex, table);
		if (row >= 0){
			table.HighLightRow(row);
		}
	}
}

//Get row index by id.
function fGetRowIndex(id, colIndex, table){
	var rtnRowIndex = -1;
	var rsData = table.rs_main;
	
	rsData.moveFirst();
	for (var i = 0; i < rsData.recordcount; i++){
		if (rsData.fields(colIndex).value == id){
			rtnRowIndex = i;
			break;
		}
		
		rsData.moveNext();
	}
	
	return rtnRowIndex;
}

//get field value of highlight row.
function getFieldValue(columnIndex, table){
	if (table == null || table == undefined){
		return;
	}
	
	var fiedValue = "";
	
	var nRowNO = table.GetRowNumber();
    if(table.IsEmpty || nRowNO > (table.rs_main.recordcount - 1) || nRowNO < 0 ){
    	return "";
    }
    
    table.rs_main.absolutePosition = table.GetRowNumber() * 1 + 1;
	fiedValue = table.rs_main.fields(columnIndex).value;
	
	return fiedValue;
}

//取得所有被勾选了checkbox栏的行record
function fGetTableCheckedString(pTable, pCheckBoxCol, pModifyCol, pColDelimiter, pRowDelimiter)
{
    var str = "";
    var strCheck = "";
    var i = 0;
    var arrCol = new Array();
    var arrModifyCol = pModifyCol.split(",");
    var iModifyColNum = arrModifyCol.length;
    //pTable.SaveRow("1");

    if (pTable.rs_main.recordCount == 0)
        return "";

    pTable.rs_main.moveFirst();
    while (!pTable.rs_main.EOF)
    {
        if (pTable.rs_main.fields(pCheckBoxCol).value == "checked")
        {
            arrCol[i] = new Array();
            /*if (pAMDFlag == true)
            {
                if (pTable.rs_main.fields(0) == "")
                    arrCol[i][0] = "A";
                else
                    arrCol[i][0] = "M";
            }*/

            for (var k = 0; k < iModifyColNum; k++)
            {
                /*if (pAMDFlag == true)
                    arrCol[i][k + 1] = pTable.rs_main.fields(parseInt(arrModifyCol[k])).value;
                else*/
                    arrCol[i][k] = pTable.rs_main.fields(parseInt(arrModifyCol[k])).value;
            }
            i++;
        }
        pTable.rs_main.moveNext();
    }

    for (i = 0; i < arrCol.length; i++)
        arrCol[i] = arrCol[i].join(pColDelimiter);

    strCheck = arrCol.join(pRowDelimiter);
    if (strCheck == "")
        return "";
    /*if (pAMDFlag == true)
    {
        var nameTypeArr = pNameAndTypeStr.split(this.rowDelimiter);
        str += "operationType" + this.colDelimiter + nameTypeArr[0] + this.rowDelimiter;
        str += this.dataType + this.colDelimiter + nameTypeArr[1];
    } else
        str = pNameAndTypeStr;
    str += this.rowDelimiter;
    str += strCheck;*/

    return strCheck;
}
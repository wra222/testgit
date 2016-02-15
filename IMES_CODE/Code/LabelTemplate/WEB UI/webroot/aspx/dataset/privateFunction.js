// JScript File
	//create dataset parameter info.
	//return parammeterDef array.
    function getParameterValues(tableObj)
    {

        var fieldName = new Array();
        fieldName[fieldName.length] = 0;
        fieldName[fieldName.length] = 1;
        fieldName[fieldName.length] = 2;
        fieldName[fieldName.length] = 3;
       fieldName[fieldName.length] = 4;
     
//        try{   
//            tableObj.Refresh();
//        } catch(e)
//        {}
        
        var rowsValue = getAllRowFieldValue(tableObj, fieldName, tableObj.Divide);
        var paramArr = new Array();
        
        for (var i=0; i<rowsValue.length; i++)
        {
            var tempParam = null;
            var rtn = com.inventec.fisreport.manager.DatasetManager.GetEmptyParameterDef();             
            if(rtn.error != null)
            {
                alert(rtn.error.Message);
            } else {
                tempParam = rtn.value;
            }
            
            tempParam.ConditionName =  rowsValue[i].split(tableObj.Divide)[0];
            tempParam.Parameter =  rowsValue[i].split(tableObj.Divide)[3];
            tempParam.Operate = rowsValue[i].split(tableObj.Divide)[2]; 
            tempParam.Type = rowsValue[i].split(tableObj.Divide)[1];
            
            tempParam.Values = new Array();
            var valueFieldText = rowsValue[i].split(tableObj.Divide)[4];
            var values = valueFieldText.split(", ");
            for (var j=0; j<values.length; j++)
                tempParam.Values[j] = values[j];
              
            paramArr[paramArr.length] = tempParam;
        }  
        
        return paramArr;
    }  
    
    function nowIputParamValue(tableName)
    {   
        var tableObj = eval(tableName);
        var operator = null;
        var fieldName = new Array(); 
        fieldName[fieldName.length] = 2; 
        
        if (typeof(optList) == "object")
            operator = document.getElementById("optList").value;
        else {
            var fieldsNum = new Array();
            fieldsNum[fieldsNum.length] = 2;
            
            operator = getHilightRowFieldsValue(eval(tableName), fieldsNum, eval(tableName).Divide);
             
        }
                 
//        if (operator == "="){ 
            var aimValue = btnParamValue.previousSibling.value;
            	
        	if (aimValue.indexOf("'") == 0)
        	{
        	    aimValue = aimValue.substr(1, aimValue.length-2);
        	 }
            	    
            txtValue.value = aimValue;  
            dlgN.style.display = "block";
            
//        } else {
//            var values = btnParamValue.previousSibling.value.split(", ");
//            valueListOfIN.innerHTML = ""
//            
//            if (btnParamValue.previousSibling.value.length != 0)
//            { 
//                for (var i=0; i<values.length; i++)	
//                {
//            	    var newOption = document.createElement("option");
//            	    var aimValue = values[i];
//                	
//            	    if (aimValue.indexOf("'") == 0)
//            	    {
//            	        aimValue = aimValue.substr(1, aimValue.length-2);
//            	        types_String.checked = true;
//            	    } else {
//            	        types_Number.checked = true;
//            	    }
//                	
//		            newOption.value = aimValue;
//		            newOption.text =  aimValue;
//		            valueListOfIN.add(newOption);
//                }
//            }
//            
//            dlgIN.style.display = "block";
//        }
    }
    
    /*
     ======== ============ =============================
     Description: called on clicking OK button for n.
     Author: itc98047
     Side Effects:
     Date:2008-12-3
     ======== ============ =============================
    */       
    function completeInputValue(type)
    {
        var data = ""; 
        
        switch (type)
        { 
            case "N":
                data = txtValue.value;
                
                if(dataType_n.checked){
                    if (!isNumber(data))
                    {
                        alert("Invalid number");
                        return false;
                    }
                }else {
                    data = "'" + data + "'";
                }
                
                dlgN.style.display="none";            
                txtValue.value = ""; 
                break; 
            case "IN":   
                data = new Array();
                for (var i=0; i<valueListOfIN.childNodes.length; i++)
                {
                    if (types_String.checked)
                        data[data.length] = "'" + valueListOfIN.childNodes[i].value + "'";
                    else
                        data[data.length] = valueListOfIN.childNodes[i].value;
                }
                                
                data = data.join(", ");                
                dlgIN.style.display="none";
                types_String.checked = true;
                valueListOfIN.innerHTML = "";
                 
                break;
             
            default:
                alert("Wrong type!");    
                
                        
        }  
        
        btnParamValue.previousSibling.value = data; 
        return;     
    } 


    function addItem_IN()
    {
        if (txtValue_IN.value.length == 0)
            return false;
             
        if (types_Number.checked && !isNumber(txtValue_IN.value))
        {
            alert("Invalid number");
            return false;
        }
                    
        var oOption = document.createElement("OPTION");
        
        valueListOfIN.options.add(oOption);
        oOption.innerText = txtValue_IN.value;
        oOption.value = txtValue_IN.value;
        txtValue_IN.value = "";
        txtValue_IN.focus();
    }

    function delItem_IN()
    {   if (valueListOfIN.selectedIndex == -1)
            return false;
            
        valueListOfIN.options.remove(valueListOfIN.selectedIndex);
    }


    function doOperatorChanged(tableName)
    {
       // document.getElementById(tableName + "Text3").value = "";
    
    }
    
    function isParamHasValue(tableObj)
    {  
		
		var rs = tableObj.rs_main;	   	 
		var allRows = new Array();
		
		if (tableObj.IsEmpty)
			return true;
 
		rs.moveFirst();	  
	    
	    var fieldsArr = new Array();
	    fieldsArr[fieldsArr.length] = 4;
	    
	    
	    
		while(!rs.EOF)
		{ 
		    	
			var fieldValue = getRowFieldsValue(rs, fieldsArr, tableObj.Divid);	
			
			if (trimString(fieldValue) == "")
			{
			    alert("There are parameters which have no values.");
			    return false; 
			
			}
			rs.moveNext();			 
			
	    }
	     
		return true; 
    
    }
var IMESUti = IMESUti || {};
IMESUti.Print=( function() {
    
   //********** private ********** 
  var setPrintParam= function (printItemCollection, labelType, keyCollection, valueCollection) {
        if (printItemCollection == undefined || printItemCollection == null || printItemCollection.length == 0) {
           return;
        }
        for (var i = 0; i < printItemCollection.length; i++) {
           if (printItemCollection[i].LabelType == labelType) {
              printItemCollection[i].ParameterKeys = keyCollection;
              printItemCollection[i].ParameterValues = valueCollection;
              return;
           }
        }
    }

   var generateArray= function (val) {
       var ret = new Array();
        ret[0] = val;
        return ret;
     }
     //********** private ********** 
    return  {
           DoPrint:function(printItemList,obj) {
            if(printItemList!=null && printItemList.length>0)
                     {
					    for (var i = 0; i < printItemList.length; i++)
                        {
                    	    var labelCollection = [];
                            labelCollection.push(printItemList[i]);
                            this.SetPrintItemListParamForObj(labelCollection, printItemList[i].LabelType,obj);
                        //    setPrintItemListParam(labelCollection, result[1][i].LabelType, customerSN);
                            printLabels(labelCollection, false);
                        }
                    }
            },
           SetPrintItemListParamForObj:function(printItemList, labelType,obj) {
            var lstPrtItem = printItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            var i=0;
            for(var prop in obj){
               // alert(propt + ': ' + obj[prop]);
                 keyCollection[i]=prop;
                 valueCollection[i]=generateArray(obj[prop]);
                 i++;
            }
            setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
        },
           
    
    
           SetPrintItemListParam:function(printItemList, labelType,keyArr,valueArr) {
            var lstPrtItem = printItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            for (var i = 0; i < keyArr.length; i++)
            {
               keyCollection[i]=keyArr[i];
               valueCollection[i]=generateArray(valueArr[i]);
            }
            setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
        },
            CheckHaveLabel:function(pcode,btnSetId,btnReprintId) {
        
              var _have=false;
              $.ajax({
                            type: "POST",
                            url: "../Service/PrintSettingService.asmx/CheckHaveLabel",
                            data: '{pcode: "' + pcode+ '" }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success:succ,
                            error: function(error) {
                              _have=false;
                        }
                });
                function succ(r)
                {
                   _have= r.d;
                   var _s='#' +btnSetId;
                   var _r='#' +btnReprintId;
                   if($(_s).length>0 )
                    { $(_s).attr('disabled',!_have);}
                   if($(_r).length>0)
                   { $(_r).attr('disabled', !_have);}
                }
                return _have;
      
        }
    }
}
)();

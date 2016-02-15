var IMESUti = IMESUti || {};

IMESUti.GetData=( function() {
    
   //********** private ********** 
  
    var _url=IMESUti.Url;
    return  {
           BindLine:function(id) {
    	    var _param={
				station:_url?_url.GetUserInfo().Station:$.url.param("Station")||"",
				customer:_url?_url.GetUserInfo().Customer:$.url.param("Customer")||"",
				stage:_url?_url.GetParamByName('Stage'):$.url.param("Stage")||""
			  }; 
		
            // data:JSON.stringify(_param)   
            //  var _lineJ='{"station":'+'"' + _s +'",' + '"customer":' +'"'+_c + '",' +'"stage":' +'"'+_st +'"}';
             $.ajax({
                            type: "POST",
                            url: "../Service/GetDataService.asmx/GetLine",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data:JSON.stringify(_param),
                            success: function (r) {
                                var ddlLine = $("#"+id);
                                ddlLine.empty().append('<option selected="selected" value=""></option>');
                                $.each(r.d, function () {
                                    ddlLine.append($("<option></option>").val(this['Value']).html(this['Text']));
                                });
                            },
                            error: function(error) {
                                                alert("Bind Line Error");
                                            }
                        });
            },
    
           GetSelectLine:function(id) {
               return $('#'+id).val()
             }
   }
 }
)();

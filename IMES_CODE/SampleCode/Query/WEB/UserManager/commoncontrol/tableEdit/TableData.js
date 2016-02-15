function TableData()
{

	this.object = this;
	this.DataURL = "";
	this.Sort = "";
	this.Filter = "";

	this._DataURL = "";
	this._Sort = "";
	this._recordsetSort = "";
	this._Filter = "";
	
	this.request = false;
	this.onDataOk = "";
	this.RowDelim = "\r\n";	
	this.FieldDelim = ",";
	this.TextQualifier = '"';
	this.recordset = null;
	this.UseHeader = true;
	
	this.errorCallback = "";

}

function TableData_getName(rs,name)
{
	try {
		while (rs.Fields(name))
			name = name + " ";
	}
	catch(e)
	{
		return name;
	}
	return name;	
}

TableData.prototype.reset=function() {

//Sort
if (this.recordset && this.Sort != this._Sort) {
	
	this._Sort = this.Sort;
	this._recordsetSort = this.Sort;
	if (this.Sort.indexOf("-") >=0) {
		this._recordsetSort = this.Sort.replace("-","") + " DESC";
	}
	
	this.recordset.Sort = this._recordsetSort;
	return;
		
}

if (this.DataURL == "")
{	
	return;

}
if (window.XMLHttpRequest) { // Mozilla, Safari,...
	this.request = new XMLHttpRequest();
	if (this.request.overrideMimeType) {
		this.request.overrideMimeType('text/xml');
	}
	} else if (window.ActiveXObject) { // IE
try {
	this.request = new ActiveXObject("Msxml2.XMLHTTP");

} catch (e) {
	try {
		this.request = new ActiveXObject("Microsoft.XMLHTTP");
	} catch (e) {}
	}
}

if (!this.request) {
	alert('Cannot create TableData XMLHTTP instance!');
	return;
}	

var dataObject=this;
	
this.request.onreadystatechange =function () {

	if (dataObject.request.readyState == 4) {
		if (dataObject.request.status == 200) {
			//Field Names;
			var strText = (dataObject.request.responseText);
			
			//start session timeout. for mpms1.0
			var flag_customized = "CUSTOMIZED_EXCEPTION</label>";
			var pos = strText.indexOf(flag_customized);
			if (pos != -1){
				strText = strText.substring(pos + (flag_customized.length));
				alert(strText);

                if (dataObject.errorCallback != "")
                    eval(dataObject.errorCallback+"()");

                return;
			}//end
			
            var arrRows = strText.split(dataObject.RowDelim);
			if(arrRows[0]==""){
                return;
            }
			var arrFields = arrRows[0].split(dataObject.FieldDelim);

			var rsSite =  new ActiveXObject("ADODB.Recordset");

			for(var i=0; i<arrFields.length; i++) {
				if (dataObject.UseHeader) {
					var arrFieldName = arrFields[i].split(":");
					var strColumn = TableData_getName(rsSite,arrFieldName[0].replace(/(^\s*)|(\s*$)/g, ""));

                    if (arrFieldName[1] && arrFieldName[1].toLowerCase() == "int")
						rsSite.Fields.Append(strColumn , 3, 16);
					else if (arrFieldName[1] && arrFieldName[1].toLowerCase() == "float")
						rsSite.Fields.Append(strColumn, 5, 32);
					else
						rsSite.Fields.Append(strColumn, 202,500);
				}
				else
				{
					rsSite.Fields.Append("Column" + i,202,500);
				}
			}

            rsSite.Open();

			//Add Rows
			var iFrom = 1;
			if (!dataObject.UseHeader)
				iFrom = 0;
            for (var ri=iFrom; ri<arrRows.length; ri++) {
                arrFields = arrRows[ri].split(dataObject.FieldDelim);
				rsSite.AddNew();
				for(var i=0; i<arrFields.length; i++) {
					try {
						rsSite.Fields(i) = arrFields[i];
					}
					catch(e)
					{
						try{
						  rsSite.Fields(i) = 0
						}
						catch(e)
						{
						}
					}
				}
			}
			if (arrRows.length-iFrom>0)
				rsSite.Update();
			dataObject.recordset = rsSite;
			dataObject.recordset.Sort = dataObject._recordsetSort;
			eval(dataObject.onDataOk+"()");
        } else
		{
			alert('There was a problem with the request. State = "' + dataObject.request.statusText + '"');
		}
	}

}
     //lys
    //this.request.open('get', this.DataURL, true );
    //this.request.send(null);



    //wwg
   // alert(this.DataURL)
    var pMap=this.DataURL.split("?");
    this.request.open('post', pMap[0], true );
    this.request.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
    this.request.send(pMap[1]);


}


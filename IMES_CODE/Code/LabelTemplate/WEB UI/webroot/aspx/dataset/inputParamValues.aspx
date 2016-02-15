<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_dataset_inputParamValues, App_Web_inputparamvalues.aspx.b52700e8" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>
 
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Parameter Value Setting</title>
    <fis:header id="header2" runat="server"/>
    <script type="text/javascript" language="javascript" src="privateFunction.js"></script>
</head>
<body class="dialogBody">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
    <div style="margin-left:10px;margin-right:10px">
    <TABLE  border="0"  cellpadding="0" cellspacing="0">
        <TR>
	        <TD  align="right"><div id="Div1" style=width:10%"></div></TD>
	    </TR>
	    <TR style="height:2px">
	        <TD></TD>
	    </TR>
        <TR>
	        <TD align="right" > <button onclick='getParamValues();' ><%=Resources.Template.okButton%></button>&nbsp;&nbsp; 
            <button onclick='window.returnValue=null;window.close();'><%=Resources.Template.cancelButton%></button>&nbsp;&nbsp;</TD>
	    </TR>
        
    </TABLE>
    <div>
    
    
     
    
    <!-- #include file="simulativeDialog.aspx" -->
</body>
</html>
<script language="javascript" type="text/javascript">
    var paramArray = window.dialogArguments; //parameterDef Array. 
    var paramTable = null;  
    
    //Condition, TYPE_NAME(p.user_type_id) AS Type, 'Single-Value' as Operator, '' as Value, '' as # " +
    function showParamTable()
    {   paramArray.toJSON = function(){return toJSON(this);};
    
        var retValue = com.inventec.fisreport.manager.DatasetManager.GetParameterTable(paramArray);
         if(retValue.error != null)
	    {
	        alert(retValue.error.Message);
	        return false;
	    }
        
        showTable(createRecordSet(retValue.value)); 
    
    }
    
    
    function showTable(rs, style)
    {
        var tableHeight = document.body.clientHeight * 0.25; 
        var tableWidth = 379; 
        var clientW = tableWidth-17;
        
        if (paramTable == null)
        {
            paramTable = new clsTable(rs, "paramTable");
            paramTable.Height = 130;
            paramTable.TableWidth = tableWidth;
            paramTable.AfterNew = "callOnClickTable()";
            //paramTable.Container = Div1;
            // paramTable.adjustRateW = 1.006;
            // paramTable.UseSort = "TDCData";
            paramTable.ControlPath = "../commoncontrol/tableEdit/";
            paramTable.Widths = new Array(clientW * 4.7/10,  clientW * 0.1/10, clientW * 0.1/10, clientW * 0.1/10, clientW * 5/10 ); 
            paramTable.FieldsType = new Array(0, 0, 0, 0, 0); 
            paramTable.modi = new Array(false, false, false, false, true);
            paramTable.HideColumn = new Array(true, false, false, false,true);              
                   
            var fieldNameCtrl = "<button id='btnParamValue' style='width:20px;align:right;' text='&' onclick='nowIputParamValue(\"paramTable\");'>...</button>";
            paramTable.UseControl(4, fieldNameCtrl);
            
            if (style)//for sql 
                paramTable.AddDelete=true;     
                
             
            paramTable.Divide = "<%=Constants.COL_DELIM%>";
            paramTable.UseHTML = true;  //??
            //paramTable.NotEmpty = 0;
            paramTable.ScreenWidth = -1;   
        }
        
        paramTable.rs_main = rs;	
        Div1.innerHTML = paramTable.Display();	
    }  
    
    function callOnClickTable()
	{
	     var paramNameCtl = document.getElementById("paramTableText3");
	    
         if (paramNameCtl != null && typeof(paramNameCtl) == "object"){
             paramNameCtl.style.borderStyle = "groove"; 
             paramNameCtl.disabled = true;
	    }
	}
    
    function getParamValues()
    { 
        paramTable.Refresh();
        
        if (!isParamHasValue(paramTable))
            return false;
            
        window.returnValue = getParameterValues(paramTable);
        window.close();
    
    }  
    
    showParamTable();

</script>
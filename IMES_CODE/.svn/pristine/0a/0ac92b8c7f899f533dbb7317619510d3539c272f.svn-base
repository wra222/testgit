<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_dataset_dataset, App_Web_datasetedit.aspx.b52700e8" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>
<script type="text/javascript" src="../../commoncontrol/btnTabs.js"></script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id=Head1 runat="server">
    <title>Dataset Edit Page</title>
</head>
<%--<bug>
    BUG NO:ITC-992-0039
    REASON:将activeX设置成隐藏
</bug>--%>
<%--<object classid="clsid:{F9043C85-F6F2-101A-A3C9-08002B2F49FB}" codebase="../../activex/comdlg32.cab#version=<%=Constants.commonVersion%>"
id="commonDlg" style="display:none" > --%>
<OBJECT classid=clsid:F9043C85-F6F2-101A-A3C9-08002B2F49FB VIEWASTEXT id="commonDlg" style="display:none" >
<PARAM NAME="_ExtentX" VALUE="847">
<PARAM NAME="_ExtentY" VALUE="847">
<PARAM NAME="_Version" VALUE="393216">
<PARAM NAME="CancelError" VALUE="0"><PARAM NAME="Color" VALUE="0">
<PARAM NAME="Copies" VALUE="1"><PARAM NAME="DefaultExt" VALUE="">
<PARAM NAME="DialogTitle" VALUE=""><PARAM NAME="FileName" VALUE="">
<PARAM NAME="Filter" VALUE=""><PARAM NAME="FilterIndex" VALUE="0">
<PARAM NAME="Flags" VALUE="1"><PARAM NAME="FontBold" VALUE="0">
<PARAM NAME="FontItalic" VALUE="0"><PARAM NAME="FontName" VALUE="">
<PARAM NAME="FontSize" VALUE="8"><PARAM NAME="FontStrikeThru" VALUE="0">
<PARAM NAME="FontUnderLine" VALUE="0"><PARAM NAME="FromPage" VALUE="0">
<PARAM NAME="HelpCommand" VALUE="0"><PARAM NAME="HelpContext" VALUE="0">
<PARAM NAME="HelpFile" VALUE=""><PARAM NAME="HelpKey" VALUE="">
<PARAM NAME="InitDir" VALUE=""><PARAM NAME="Max" VALUE="0">
<PARAM NAME="Min" VALUE="0"><PARAM NAME="MaxFileSize" VALUE="260">
<PARAM NAME="PrinterDefault" VALUE="1">
<PARAM NAME="ToPage" VALUE="0">
<PARAM NAME="Orientation" VALUE="1">
</object>



<fis:header id="header2" runat="server"/>
<script type="text/javascript" language="javascript" src="privateFunction.js"></script>
<body >
    <form id="form1" runat="server">
    </form>


    
    <table width="100%" height="100%" border="0" style="background-color: rgb(147,191,218)"> 
       <%--<tr >
	    <td class="title"> <label id="head">
            New Dataset - Edit</label></td>
        </tr>--%>
        <%--<bug>
            BUG NO:ITC-992-0008 
            REASON:Test Sql时捕获异常
      </bug>--%>
        <tr valign="top;" style="margin-top:10px">
            <td>
                <fieldset id="fieldset1" style="padding-left:0pt;padding-right:0pt;">
                <legend>Query Set</legend> 
                    <div id="con" style="margin-top:5px"></div>
                    <div id="divQuery" class="propertyDialogContent1" style="height:270px;">
                    <table style="width:100%;height:100%" cellpadding="0" cellspacing="5" border="0">
                        <tr height="1">
                            <td width="15%" nowrap class="inputTitle"><%=Resources.Template.dataSetName%>:</td>
                            <td width="40%">
                            <%--<bug>
                                    BUG NO:ITC-992-0058
                                    REASON:将最大长度缩小，否则在属性页面显示不全
                            </bug>--%>
                             <input type="text" id="datasetName" style="width:180px" maxlength="25" /> 
                            </td>
                            <td width="10%">
                            </td>
                            <td width="40%">
                            </td>
                        </tr>
                        <tr height="1">
                            <td nowrap class="inputTitle"><%=Resources.Template.datasetServer%>:</td>
                            <td>
                            <select id="serverList" style="width:180px" onchange="freshDatabaseList()"></select> 
                            </td>
                            <td nowrap><%=Resources.Template.database%>:
                            </td>
                            <td>
                            
                             <select id="databaseList" style="width:180px"  onchange="freshContent()"></select> 
                            </td>
                        </tr>
                        <tr height="1">
                            <td class="inputTitle"><%=Resources.Template.queryType%>:</td>
                            <td onclick="changeQueryType();">
                                 <input type="radio" id="queryType"  name ="sqlType" checked /><%=Resources.Template.sqlText%>
                                <br>
                                <%--<bug>
                                    BUG NO:ITC-992-0041
                                    REASON:点选sp radio时，再查找所有的存储过程
                                </bug>--%>
                                <input type="radio"  name ="sqlType" id="spType" onclick="freshSPList()" /><%=Resources.Template.procedure%>
                            </td>
                            <td> 
                            <button id="import" onclick ="impotSQL()"><%=Resources.Template.import%></button>
                            </td>
                            <td align="right">
                             
                            </td>
                        </tr>
                        <tr height="1">
                            <td nowrap colspan="4" >
                                <span id="spTip" style="display:none"><%=Resources.Template.chooseProcedure%>: <select id="spList" style="width:333px"  onchange="showSPSqlText()"></select>&nbsp;&nbsp;&nbsp;&nbsp;<button id="refresh" onclick ="refreshSp()" style="display:none"><%=Resources.Template.refresh%></button></span>
                            </td>
                           
                            
                            
                        </tr>
                        <tr height="1">
                            <td colspan="4" > 
                           <span id="sqlTip"><%=Resources.Template.inputSql%>:</span>
 
                            </td> 
                        </tr>
                        <tr>
                            <td colspan="4" > 
                           <textarea id="sqlContainer" style="width:560px;height:100%;background-color:white;" contentEditable="true"></textarea>
 
                            </td> 
                        </tr>
                        
                      
                    </table>
                    </div>
                    
                   <!-- #include file="conditionTab.aspx" -->
                </fieldset><br />
            </td>
        </tr>
      <%-- <tr >
            <td style="height:10px" align="right"> 
                
                
            </td>          
        </tr> --%>
       <tr >
            <td> 
                <FIELDSET id="fieldset2" style="width:100%;;padding-left:0pt;padding-right:0pt;">
                <legend><%=Resources.Template.queryTest%></legend> 
                    <div id="div1" class="propertyDialogContent" style="border:0;height:150px">  
                    <table style="width:100%;height:100%">
                        <tr style="height:1px">
                            <td>
                            <button id="testBtn" onclick="testbtnClick()" style="width:120px" title="Test SQL/SP"><%=Resources.Template.test%></button>&nbsp;&nbsp;<%=Resources.Template.topRecords%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <div id="resultContainer" style="width:560px;height:100%;background-color:White" ></div>
                            </td>
                        </tr>
                    </table>
                    </div>
                </FIELDSET>
            </td>          
        </tr>
        <tr >
            <td style="height:30px" align="right"> 
                
                 <button id="Button2" onclick="saveDataSet();"><%=Resources.Template.okButton%></button>&nbsp;&nbsp;
                 <button id="Button3" onclick="closePage()"><%=Resources.Template.cancelButton%></button>&nbsp;&nbsp;&nbsp;
            </td>          
        </tr>
    </table>
    
<!-- #include file="simulativeDialog.aspx" -->
 
</body>
 <fis:footer id="footer1" runat="server"/> 
 <script for="paramTableText0" language="javascript" event="onblur">
    //判断输入的参数名不能与表格中的已有记录重名
	if (checkUnique(this.value)){
	    this.select();
	}
</script>

<script type="text/javascript">
    var datasetId = "<%=(String)Request.Params.Get("datasetId")%>";  
    //var datasetId = "bcbdfbbe086246b9a544062095f472f8";  
    var tabs = null;
    var paramTable = null; 
    var isNewOne = true;
    var datasetStruct = "";
    var currentQueryType = 0;
    var method = "";
    var templatePram;
    var templateXmlandHtml;
    //记录开始参数的数据类型
    var sourceDataType = "";
    //记录sp parameter类型是否正确的标志
    var spParamErrorflag = false;
    function searchDataset(name,value)
    {
        for (i = 0; i < printTemplateInfo.DatasettingList.length; i++)
        {
            if (printTemplateInfo.DatasettingList[i].DataSetName == name) 
            {
                if (value == "") {
                    datasetStruct = printTemplateInfo.DatasettingList[i];
                } else {
                    printTemplateInfo.DatasettingList[i] = value;
                }
                break;
            }
       
        }
    }
    function initPage()
    {  

       createBtnTabs();
       fillServerList();    
         
        //获取传过来的printTemplateInfo对象
       // printTemplateInfo = window.dialogArguments.printTemplate;
        var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
        if (ret.error != null) {
         
            alert(ret.error.Message);
        } else{
            templateXmlandHtml = ret.value;
            printTemplateInfo = templateXmlandHtml.PrintTemplateInfo;
           
        }
        templatePram = printTemplateInfo.InputParas;
        method = window.dialogArguments.method;
        if (method == "add") {
             
            var rtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();          
            if(rtn.error != null)
            {
                alert(rtn.error.Message);
                return ;
            } else {
                datasetStruct = rtn.value;
            
            }
        } else {
            //获取要编辑的dataset对象
            searchDataset(window.dialogArguments.dataset, "");
        }
           
        //set title
        if (method == "add"){//new
//            document.getElementById("head").innerText = "New Dataset - Edit";
            isNewOne = true;
            
        } else {    //edit ,then wait for get dataset name 
            isNewOne = false;
            
//            var rtn = com.inventec.fisreport.manager.DatasetManager.GetDatasetStructObj(datasetId);  
//            
//            if(rtn.error != null)
//            {
//                alert(rtn.error.Message);
//                return ;
//            } else {
//                datasetStruct = rtn.value;
//            
//            }
            
            datasetName.value = datasetStruct.DataSetName;
      
            
        //select server,find by name, so it's necessary to loop          
            for (var i=0; i<serverList.childNodes.length; i++)
            {
                if (serverList.childNodes[i].text ==  datasetStruct.DatasetInfo.DatabaseServer)
                {
                    serverList.value = serverList.childNodes[i].value;
                    freshDatabaseList()
                    break;
                }                
            }
       //select database     
            for (var i=0; i<databaseList.childNodes.length; i++)
            {
                if (databaseList.childNodes[i].text ==  datasetStruct.DatasetInfo.Database)
                {
                    databaseList.value = databaseList.childNodes[i].value;
//                    freshSPList()
                    break;
                }                
            }
            
                  
        //query type 
            if (datasetStruct.QueryType == "<%=Constants.DATASET_QUERY_TYPE_PROCEDURE %>")
            {
                sqlType[1].checked = true;
                sqlType[1].click();
                for (var i=0; i<spList.childNodes.length; i++)
                {
                    if (spList.childNodes[i].text ==  datasetStruct.ProcedureName)
                    {
                        spList.value = datasetStruct.ProcedureName;
                        showSPSqlText()
                        break;
                    }                
                }
            } else {
                sqlContainer.innerText = datasetStruct.SqlTextString;
            }
             
          
             
            
        //create parameter table            
           //getConditionTable(); 
           getInitConditionTable();
        
        } 
    }
    
    function createBtnTabs()
	{
		tabs = new clsButtonTabs("tabs");
		tabs.selectChanged = tabCallback;
//		tabs.container = con;
//	    tabs.mode = "text";
//		tabs.beDynamic = false;
		tabs.preSelectChanged = checkTabs;
		for (var i=0; i<2; i++)
		{
			var temp = new clsButton("tabs");
			if (i == 0) {
			    temp.normalPic = "../../images/query" +"-1.jpg";
			    temp.selPic = "../../images/query" +"-2.jpg";
			    temp.disablePic = "../../images/query" +"-3.jpg";
			} else {
			    temp.normalPic = "../../images/para" +"-1.jpg";
			    temp.selPic = "../../images/para" +"-2.jpg";
			    temp.disablePic = "../../images/para" +"-3.jpg";
			}
//            if (i == 0)
//			    temp.text = "Query";
//			else
//			    temp.text = "Condition";
//			temp.text = "";    
			tabs.addButton(temp); 
		}
		con.innerHTML = tabs.toString();
	   
		tabs.initSelect(0);
	}
	
	function checkTabs()
	{ 

	    //    <bug>
        //        BUG NO:ITC-992-0035 
        //        REASON:判断是否选择了db，否则不能切换页面
        //    </bug>
	    
	    if (tabs.selectedButton.index == 0)
	    {
	        if (sqlType[1].checked && spList.selectedIndex == -1)
	        {
	            return false;
	        } else if (sqlType[0].checked && trimString(sqlContainer.innerText) == "")
	        {
	            return false;
	        } else if (databaseList.selectedIndex == -1)
	        {
	            return false;
	        } 
	    } else {
	       
	        paramTable.Refresh();
	        
	    }
	    
	    return true;
	}
	
	function tabCallback(index)
	{
    
//        <bug>
//            BUG NO:ITC-992-0075 
//            REASON:程序逻辑有误
//        </bug>
	    pageFlag = index;
		switch(index){
		    case 0:
		        divQuery.style.display = "";
		        conditionArea.style.display = "none";
		        break;
	        case 1:
	            divQuery.style.display = "none";
	            conditionArea.style.display = "";
    	        
	            if (paramTable == null)
	                getConditionTable();
    	            
		        break;
            default:
                break;	
        }
    }	

    //create DB server list
    function fillServerList()
	{
	    var parentId = "";
	    var i=0;
	    var dbServerDtRtn = com.inventec.template.manager.TemplateManager.getDataSourceListForSelect();// we can get alias and username psd from it
	  
	    if(dbServerDtRtn.error != null)
	    {
	        alert(dbServerDtRtn.error.Message);
	    }
	    else
	    {
	        //getDbServerValue(dbServerDtRtn.value);
	        fillSelectForDatatb("serverList", dbServerDtRtn.value, null);
	        serverList.selectedIndex = -1;
	    }
	} 
	
	function fillSelectForDatatb(selId, dataTbValue, hasNullOption, defaultValue)
	{
        var objSel = document.getElementById(selId);
        var arr = new Array();
        var dbServerValue;
        var arrName = new Array();
        objSel.innerHTML = "";

        if (hasNullOption != undefined && hasNullOption){
            var nullOption = document.createElement("option");
            nullOption.value = "";
            nullOption.text =  "";
            objSel.add(nullOption);
        }
       
        for(var i = 0; i < dataTbValue.Rows.length; i++)
        {
   	        var option = document.createElement("option");
            //arrName[j] = dataTbValue.Columns[i].Name;
            option.value = dataTbValue.Rows[i]["id"];
            option.text =  dataTbValue.Rows[i]["name"];
            objSel.add(option);
        }          
        if (defaultValue != undefined)
        {
            objSel.value = defaultValue;
        }
	}
	
	
	function freshDatabaseList()
	{
	    serverId = document.getElementById("serverList").value;
	    var rtnDb = com.inventec.template.manager.TemplateManager.getDataSourceDatabaseListForSelect(serverId);
	   
	    if(rtnDb.error != null)
	    {
	        alert(rtnDb.error.Message);
	    }
	    else
	    {
	         for(var i=0; i < rtnDb.value.Columns.length; i++)
	         {
	                 //alert(rtnDb.value.Columns[i].Name) ;
	         }
	         fillSelectForDatatb("databaseList", rtnDb.value, null);
	         databaseList.selectedIndex = -1;
	        // databaseId = document.getElementById("databaseList").value; 
	    }
	    
	    sqlType[0].checked = true;
	    sqlContainer.contentEditable = true;
	    document.getElementById("spTip").style.display = "none";
	    document.getElementById("sqlTip").style.display = "block";
	    document.getElementById("import").style.visibility = "visible";
	    document.getElementById("refresh").style.display = "none";
	    document.getElementById("sqlContainer").innerText = "";
	    currentQueryType = 0;
	    
	    
	}    
	    
    
    function getInitConditionTable()
    {
        
        var rtn = null;
        //create param table by the old dataset parameters.
        var paramStruct = datasetStruct.Parameters;
                
	    paramStruct.toJSON = function(){return toJSON(this);};   
        rtn = com.inventec.fisreport.manager.DatasetManager.GetParameterTable(paramStruct);
        if(rtn.error != null)
        {
            alert(rtn.error.Message);
            return false;
        }  
          
       if (sqlType[1].checked)
       {
            
            
            showInputParamTable(createRecordSet(rtn.value), false);
            
        } else { 
           
                      
            showInputParamTable(createRecordSet(rtn.value), true); 
            
        } 
        
    }
    
    function getConditionTable()
    {
        
        var rtn = null;
          
       if (sqlType[1].checked)
       {
//            if (method == "add")
//            {
                rtn = com.inventec.fisreport.manager.DatasetManager.getSpParameter(serverList.value, databaseList.options(databaseList.selectedIndex).text, spList.options[spList.selectedIndex].value, "0");
                if (rtn.error != null) 
                {
                    alert(rtn.error.Message);
                    //显示空表
                    var paramStruct = new Array();
                    paramStruct.toJSON = function(){return toJSON(this);};
                    rtn = com.inventec.fisreport.manager.DatasetManager.GetParameterTable(paramStruct);
                    if(rtn.error != null)
                    {
                        alert(rtn.error.Message);
                        return false;
                    }       
                      
                    showInputParamTable(createRecordSet(rtn.value), true); 
                   
                    return false;
                } 
//            } else {
//                //create param table by the old dataset parameters.
//                var paramStruct = new Array();
//               
//                if (!(method == "add"))
//                {
//                    paramStruct = datasetStruct.Parameters;

//	            } 
//	            paramStruct.toJSON = function(){return toJSON(this);};   
//                 var rtn = com.inventec.fisreport.manager.DatasetManager.GetParameterTable(paramStruct);
//                if(rtn.error != null)
//                {
//                    alert(rtn.error.Message);
//                    return false;
//                }  
//            } 
            
            showInputParamTable(createRecordSet(rtn.value), false);
            
        } else { 
            var paramStruct = new Array();
//            if (!(method == "add"))
//            {
//                paramStruct = datasetStruct.Parameters;

//	        }    
           paramStruct.toJSON = function(){return toJSON(this);};
            var rtn = com.inventec.fisreport.manager.DatasetManager.GetParameterTable(paramStruct);
            if(rtn.error != null)
            {
                alert(rtn.error.Message);
                return false;
            }       
                      
            showInputParamTable(createRecordSet(rtn.value), true); 
            
        } 
        
    }
    
    
    function showInputParamTable(rs, style)
    {
        var tableHeight = document.body.clientHeight * 0.25;

        
        var tableWidth = document.body.clientWidth-80; 
        var clientW = tableWidth-10;
        
      if (paramTable == null)
        {
            paramTable = new clsTable(rs, "paramTable");
            paramTable.Height = 220;
            paramTable.TableWidth = tableWidth;
            //paramTable.Container = DivCondition;
            // paramTable.adjustRateW = 1.006;
            // paramTable.UseSort = "TDCData";
            paramTable.ControlPath = "../commoncontrol/tableEdit/";
            paramTable.Widths = new Array(clientW * 3/10, clientW * 2/10, clientW * 2/10, clientW * 2.9/10,clientW * 0.1/10); 
            paramTable.FieldsType = new Array(0, 0, 0, 0, 0); 
            paramTable.modi = new Array(false, false, false, false,true);
            paramTable.HideColumn = new Array(true, true, true, true,false);              
            DivCondition.style.width = "100%";
            DivCondition.style.height = "100%";          
            var fieldNameCtrl = "<button id='btnParamValue' style='width:20px;align:right;' text='&' onclick='nowIputParamValue(\"paramTable\");'>...</button>";
            paramTable.UseControl(4, fieldNameCtrl);
//            paramTable.AfterNew = "callOnClickTable()";
            paramTable.AfterNew = "MyAfterNew()";
            //paramTable.BeforeSave = "DealBeforeSave()"
            if (style)//for sql 
            {
                
                paramTable.modi = new Array(true, false, false, false, true);
                paramTable.AddDelete=true;    
                paramTable.outerSelect = true; 
                
                var typeList = null;
                var rtn = com.inventec.fisreport.manager.DatasetManager.GetSqlTypeList();          
                if(rtn.error != null)
                {
                    alert(rtn.error.Message);
                    return ;
                } else {
                    typeList = rtn.value;
                
                }
                
                var selControl = "<select id='typeList'  style='width:95%;' onChange='changeType(this.value)'>";
                for (var i=0; i< typeList.length; i++)                
                    selControl += '<option value="'+typeList[i]+'">' +typeList[i] + '</option>';
                                
                selControl += "</select>";
                paramTable.UseControl(1, selControl); 
                
                selControl = "<select id='optList'  style='width:95%;' onchange='doOperatorChanged(\"paramTable\");'>";                          
                selControl += "<option value='" + "<%=Constants.EDIT_CONDITION_OPERATE_EQUAL%>" + "' >" + "<%=Constants.EDIT_CONDITION_OPERATE_EQUAL%>" + "</option>"; 
                selControl += "</select>";
                paramTable.UseControl(2, selControl); 
                
                var selControl = "<select id='templateParam' name='templateParam' style='width:95%;'>";
//                for (var i=0; i< templatePram.length; i++) {
//                    
//                    selControl += '<option value="'+templatePram[i].ParaName+'">' +templatePram[i].ParaName + '</option>';
//                }                
                selControl += "</select>";
                 paramTable.UseControl(3, selControl); 
                 
              
             } 
             else {  
              
                paramTable.modi = new Array(false, false, false, false, true);
                paramTable.AddDelete=true;    
                paramTable.outerSelect = true; 
                
            
              
              
                
                var selControl = "<select id='templateParam' name='templateParam'  style='width:95%;'>";
//                for (var i=0; i< templatePram.length; i++)                
//                    selControl += '<option value="'+templatePram[i].ParaName+'">' +templatePram[i].ParaName + '</option>';
                                
                selControl += "</select>";
                 paramTable.UseControl(3, selControl); 
               
               
             
             }
            paramTable.Divide = "<%=Constants.COL_DELIM%>";
            paramTable.UseHTML = true;  //??
            paramTable.NotEmpty = 0;
            paramTable.NoKey = true;
            paramTable.ScreenWidth = -1;   
        }
        
        paramTable.rs_main = rs;	
        DivCondition.innerHTML = paramTable.Display();	
    }  
 
	function callOnClickTable()
	{
	     var paramNameCtl = document.getElementById("paramTableText3");
	    
         if (paramNameCtl != null && typeof(paramNameCtl) == "object"){
             paramNameCtl.style.borderStyle = "groove"; 
             paramNameCtl.disabled = true;
	    }
	}

	function showSPSqlText()
	{
        var spName = document.getElementById("spList").value; 
        
        var rtnSpContent = com.inventec.fisreport.manager.DatasetManager.getSpSqlText(serverList.value, databaseList.options(databaseList.selectedIndex).text, spName); 
        var spContentStr = ""; 
    
        if(rtnSpContent.error != null)
        {
            alert(rtnSpContent.error.Message);
        }
        else
        {     
            var rtnSpContentValue = rtnSpContent.value;
            
            for(var i = 0; i < rtnSpContentValue.Rows.length; i++)
            {
                spContentStr += rtnSpContentValue.Rows[i]["Text"] + "<%=Constants.COL_DELIM%>";
            }       
            
            document.getElementById("sqlContainer").innerText = spContentStr;
        }
        
        getConditionTable();

	} 
	
	function freshSPList()
	{

	    if (databaseList.selectedIndex == -1)
	    {
	        alert("<%=Resources.Template.chooseDb%>");
	        return false;
	    }
	    
	    var serverId = serverList.value;
	    var databaseName = databaseList.options[databaseList.selectedIndex].text ;
	    
	    var rtnSpDb = com.inventec.fisreport.manager.DatasetManager.getSpList(serverId, databaseName);
	     
	    if(rtnSpDb.error != null)
	    {
	        alert(rtnSpDb.error.Message);
	    }
	    else
	    {
	        spList.innerHTML = "";
	        fillSelectForDatatb("spList", rtnSpDb.value, null);
	        spList.selectedIndex = -1;
	    }
	    
	}
	
	function freshContent()
	{
	    sqlType[0].checked = true;
	    sqlContainer.contentEditable = true;
	    document.getElementById("spTip").style.display = "none";
	    document.getElementById("sqlTip").style.display = "block";
	    document.getElementById("import").style.visibility = "visible";
	    document.getElementById("refresh").style.display = "none";
	    document.getElementById("sqlContainer").innerText = "";
	    //sqlContainer.innerText = "";
	    sqlContainer.focus();
	    currentQueryType = 0;
	    // paramTable = null;	
	    DivCondition.innerHTML = "";
	    paramTable = null;	
	    
	}
	function changeQueryType()
	{ 

        if (event.srcElement.tagName == "INPUT")	    
        {
            if (sqlType[0].checked && currentQueryType != 0)
            {
                sqlContainer.contentEditable = true;
	            document.getElementById("spTip").style.display = "none";
	            document.getElementById("sqlTip").style.display = "block";
	            document.getElementById("import").style.visibility = "visible";
	            document.getElementById("refresh").style.display = "none";
	            sqlContainer.innerText = "";
	            sqlContainer.focus();
	            currentQueryType = 0;
            }
            else if(sqlType[1].checked && currentQueryType != 1)
            {
                sqlContainer.contentEditable = false;
	            document.getElementById("import").style.visibility = "hidden";
	            document.getElementById("spTip").style.display = "block";
	            document.getElementById("sqlTip").style.display = "none";
	            document.getElementById("refresh").style.display = "";
	            sqlContainer.innerText = "";
	            currentQueryType = 1;
	            spList.selectedIndex = -1;
            
            }else {}
            
            DivCondition.innerHTML = "";
	        paramTable = null;	
        }
	
	   
	} 
	
	function testbtnClick()
	{  

	    if (databaseList.selectedIndex == -1)
	    {
	        alert("<%=Resources.Template.selectDb%>");
	        document.getElementById("databaseList").focus();
	        return false;
	    } 
	    
	    if (sqlType[1].checked && spList.selectedIndex == -1){
            alert("<%=Resources.Template.selectPro%>");  
            document.getElementById("spList").focus();              
            return false;   
        } 
	     if (trimString(sqlContainer.innerText) == ""){
            alert("<%=Resources.Template.sqlEmpty%>");   
            document.getElementById("sqlContainer").focus();         
            return false;   
        }
        

        
	    if (paramTable == null)    
            getConditionTable(); 
       if (tabs.selectedButton.index == 0)
	    {
	       
	    } else {
	       paramTable.Refresh();
	       
	    }   
	    waitForParamValue(paramTable); 
	      
	} 
	
	
	function waitForParamValue(tableObj)
	{

	    var sFeature1 = "dialogHeight:220px;dialogWidth:400px;center:yes;status:no;help:no;scroll:no";
        var param = new Array(); 
         
        param = getParameterValues(tableObj);
        
        var retParam = window.showModalDialog("inputParamValues.aspx", param, sFeature1);   
        
        if (retParam == null)
            return false;

        param = appendOutputParam(retParam);  
        if (!spParamErrorflag)   { 
	         var dsStruct = initDatasetStruct(param);
	        dsStruct.toJSON = function(){return toJSON(this);};
    	   
	        var rpt = com.inventec.fisreport.manager.DatasetManager.GetDatasetSqlResult(serverList.value, dsStruct, databaseList.value);// we can get alias and username psd from it
            
            if(rpt.error != null)
            {
                alert(rpt.error.Message);
                return;
            }
            var tableInfo =  rpt.value;
            if ((tableInfo != null) && (typeof(tableInfo) == "object") && (tableInfo.Columns.length > 0))
            {
                showResultTable(createRecordSet(rpt.value)); 
             }
	    }
	} 
    
	 //tableName: tableedit control object.
    function initDatasetStruct(paramValue)
    { 
        var dsStruct = null;
        var rtn = com.inventec.fisreport.manager.DatasetManager.GetEmptyDatasetStruct();          
        if(rtn.error != null)
        {
            alert(rtn.error.Message);
            return ;
        } else {
            dsStruct = rtn.value;
        
        }
            
        //datasetStruct.Version = "1.0";
        dsStruct.DatasetInfo.DatabaseServer = serverList.options(serverList.selectedIndex).text;
        dsStruct.DatasetInfo.DatabaseServerAlias = serverList.options(serverList.selectedIndex).text;
        dsStruct.DatasetInfo.Database = databaseList.options(databaseList.selectedIndex).text;
        dsStruct.DatasetInfo.DatabaseUser = "";
        dsStruct.DatasetInfo.Editor = "";
        dsStruct.DatasetInfo.DbId = databaseList.options(databaseList.selectedIndex).value;
        dsStruct.DataSetName = trimString(datasetName.value);
        
        if (sqlType[0].checked) {
            dsStruct.QueryType = "<%=Constants.DATASET_QUERY_TYPE_SQL %>"; 
            dsStruct.ProcedureName = "";
            dsStruct.SqlTextString = sqlContainer.innerText;
        }
        else {
            dsStruct.QueryType = "<%=Constants.DATASET_QUERY_TYPE_PROCEDURE %>";
            dsStruct.ProcedureName = spList.options(spList.selectedIndex).text;
        }    
         
        
        
        dsStruct.Parameters = paramValue;         
         
        return dsStruct;
    }
  
   
  
    var resultTable = null;
    function showResultTable(rs)
    { 
      
        var clientW = document.body.clientWidth;
        var tableWidth = clientW - 85;
        var tableHeight = 90;
        var tableWidth1 = (tableWidth) > 17 ? (tableWidth - 17) : tableWidth;
        
//        if (resultTable == null) {
            resultTable = new clsTable(rs, "resultTable");
            resultTable.Height = tableHeight;
            resultTable.TableWidth = tableWidth;
            resultTable.adjustRateW = 1.006;
            //resultTable.UseSort = "TDCData";
            resultTable.ScreenWidth = -1;
            resultTable.ControlPath = "../../commoncontrol/"; 
            
            var widths = new Array();
            var types = new Array();
            var hideCols = new Array();
            for (var i=0; i<rs.Fields.Count; i++)
            {
                 widths[widths.length] = 130;
                 types[types.length] = 0;     
                 hideCols[hideCols.length] = true;  
            }
            resultTable.Widths = widths; 
            resultTable.FieldsType = types;             
            resultTable.HideColumn = hideCols;         
                
            DivCondition.style.width = "100%";
            DivCondition.style.height = "100%";          
             
             
            resultTable.Divide = "<%=Constants.COL_DELIM%>";
            resultTable.UseHTML = true;  //??
            resultTable.NotEmpty = 0;
            resultTable.ScreenWidth = -1;   
//        }

        resultTable.rs_main = rs;	
        resultContainer.innerHTML = resultTable.Display();	
      
    }

    
    function impotSQL()
    {  
//        <bug>
//            BUG NO:ITC-992-0088 
//            REASON:只打开sql类型文件
//        </bug> 

        commonDlg.Filter = "Recognised Files (*.sql)|*.sql" 
        commonDlg.DefaultExt = ".sql"
		commonDlg.ShowOpen(); 
    
        readSQLFile(commonDlg.FileName);
        getConditionTable();
    }
    
    function readSQLFile(fileName)
    { 
        if (fileName == "")
            return false;
            
        var fso, f1, ts, s;
        var ForReading = 1;
        
        fso = new ActiveXObject("Scripting.FileSystemObject"); 
//        <bug>
//            BUG NO:ITC-992-0077 
//            REASON:打开Unicode文件，而不是ANSI编码文件，注意最后一个参数就是Unicode编码打开
//        </bug> 
        try {
            ts = fso.OpenTextFile(fileName, ForReading, false, -1);
            s = ts.ReadAll();
            ts.Close();
            
            sqlContainer.innerText = s;  
        } catch(e) {
           alert(e.description);
        }
    }
    
    function saveDataSet()
    {
            
        var errorFlag = false;
        //    <bug>
        //        BUG NO:ITC-992-0009 
        //        REASON:将判断方法前移
        //    </bug>
        if (!checkBeforeSave())
            return; 
        if (paramTable == null)    
            getConditionTable(); 
//        try{   
//            paramTable.Refresh();
//        } catch(e)
//        {}
//       
        if (tabs.selectedButton.index == 0)
	    {
	       
	    } else {
	        paramTable.Refresh();
	    }
	    
        
        
        var value = getAllParameterInfo(paramTable);
        if (!spParamErrorflag) {
            var dsStruct = initDatasetStruct(value);
             dsStruct.toJSON = function(){return toJSON(this);};
             var rpt = com.inventec.fisreport.manager.DatasetManager.GetDatasetTableInfo(serverList.value, dsStruct, databaseList.value);// we can get alias and username psd from it
            
            if(rpt.error != null)
            {
                alert(rpt.error.Message);
                return;
            }  

            if (method == "add") {
                //push
                printTemplateInfo.DatasettingList.push(rpt.value);
                
               
            } else {
       
               searchDataset(window.dialogArguments.dataset, rpt.value);
               
                
            }
            
            //如果datasetname变更了同时又其他object引用，则要改变他们的dataobject的name
            if (method != "add") 
            {
                var sourceName =  datasetStruct.DataSetName;
                if (sourceName != dsStruct.DataSetName) {
                    for (i = 0; i < printTemplateInfo.DataObjects.length; i++)
                    {
                        
                        if ((printTemplateInfo.DataObjects[i].Source == "<%=Constants.DATA_SOURCE_DATA_SET%>") && (printTemplateInfo.DataObjects[i].DataSet == datasetStruct.DataSetName))
                        {
                            printTemplateInfo.DataObjects[i].DataSet = dsStruct.DataSetName;
                           
                        }
                    } 
              }
                                 
            }
            
            templateXmlandHtml.toJSON = function(){return toJSON(this);};
	        var ret = com.inventec.template.manager.TemplateManager.saveSession(templateXmlandHtml);
	        if (ret.error != null) {
                alert(ret.error.Message);
            } else{
                   
                 window.returnValue = rpt.value.DataSetName;
                window.close();  
            }
      }
//          
//    
    }
    
    function testSPParameter()
    {
       
        var rtn = com.inventec.fisreport.manager.DatasetManager.getSpParameter(serverList.value, 
                databaseList.options(databaseList.selectedIndex).text, 
                spList.options[spList.selectedIndex].value, "0");
        if (rtn.error != null) 
        {
            alert(rtn.error.Message);
            return false;
        } 
        return true;
    }
    //include input and out parameters 
    function getAllParameterInfo(paramTable)
    {
        var inputParamList = getParameterValues(paramTable);//now only include input parameters.
         
        return appendOutputParam(inputParamList);
    }	
    
    function appendOutputParam(inputParamList)
    {
        if (sqlType[1].checked)
        {
            spParamErrorflag = false;
            //判断sp参数类型是否合法
            if (!testSPParameter()) {
                spParamErrorflag = true;
                return false;
            }
            //below line get output parameters info, append to the end of input parameters.
            rtn = com.inventec.fisreport.manager.DatasetManager.getSpParameter(serverList.value, 
                databaseList.options(databaseList.selectedIndex).text, 
                spList.options[spList.selectedIndex].value, "1");
                
            if(rtn.error != null)
            {
                alert(rtn.error.Message);
               
                return false; 
            } else {
                var tableInfo = rtn.value;
                var tempParam = null;
                var rtn = com.inventec.fisreport.manager.DatasetManager.GetEmptyParameterDef();             
                
                if(rtn.error != null)
                {
                    alert(rtn.error.Message);
                    return false;
                } else {
                    tempParam = rtn.value;
                }
                
                for (var i = 0; i < tableInfo.Rows.length; i++)
                { 
                    //for (var col = 0; col < tableInfo.Columns.length; col++)
                    { 
                        tempParam.ConditionName =  tableInfo.Rows[i]["Parameter"]  ;
                        tempParam.Parameter =  ""  ;
                        tempParam.Operate = tableInfo.Rows[i]["Operator"] ; 
                        tempParam.Type = tableInfo.Rows[i]["Type"]  ;
                        tempParam.IsInputParam = false;//denote output parameter.  
                        
            
                    } 
                    
                    inputParamList[inputParamList.length] = tempParam;
                }    
            }
        }
    
        return inputParamList;
    }
    function closePage() 
    {
        window.close();
    }
    function checkBeforeSave()
    {
        var sameFlag = false;
        var refenceFlag = false;
        if (trimString(datasetName.value) == ""){
            alert("<%=Resources.Template.noInputDataset%>");
            //document.getElementById("datasetName").focus();
            return false;
        }
            
        
	    else if (databaseList.selectedIndex == -1)
	    {
	        alert("<%=Resources.Template.selectDb%>");
	        document.getElementById("databaseList").focus();
	        return false;
	    } 
	    
	    else if (sqlType[1].checked && spList.selectedIndex == -1){
            alert("<%=Resources.Template.selectPro%>");  
            document.getElementById("spList").focus();              
            return false;   
        } 
	    else if (trimString(sqlContainer.innerText) == ""){
            alert("<%=Resources.Template.sqlEmpty%>");   
            document.getElementById("sqlContainer").focus();         
            return false;   
        }
        
        else {
            //判断是否重名   
            for (i = 0; i < printTemplateInfo.DatasettingList.length; i++)
            {
                if (method == "add") {
                    if (printTemplateInfo.DatasettingList[i].DataSetName == trimString(datasetName.value))
                    {
                        sameFlag = true;
                        break;
                    }
                } else {
                     if ((printTemplateInfo.DatasettingList[i].DataSetName == trimString(datasetName.value)) && (printTemplateInfo.DatasettingList[i].DataSetName != datasetStruct.DataSetName))
                    {
                        sameFlag = true;
                        break;
                    }
                }
            } 
            if (sameFlag) 
            {
                alert("<%=Resources.Template.datasetConflic%>");
               // document.getElementById("datasetName").focus();
                 return false;  
            }  
            //判断有谁在引用
            if (method != "add") 
            {
               
                for (i = 0; i < printTemplateInfo.DataObjects.length; i++)
                {
                    
                    if ((printTemplateInfo.DataObjects[i].Source == "<%=Constants.DATA_SOURCE_DATA_SET%>") && (printTemplateInfo.DataObjects[i].DataSet == datasetStruct.DataSetName))
                    {
                        refenceFlag = true;
                        break;
                    }
                } 
                if (refenceFlag)
                {
                     if(!confirm("<%=Resources.Template.datasetUsed%>" + "(original dataset:" + datasetStruct.DataSetName + ")")) 
                     {
                            return false;
                     }
                }
                 
            }
        }
//        if (paramTable == null)
//        {
//            getConditionTable(); 
//            return false;   
//        }    
//        
//        if (!isParamHasValue(paramTable))
//            return false;
            
        return true;
    } 
   
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	checkUnique
    //| Author		:	Lucy Liu
    //| Create Date	:	4/24/2009
    //| Description	:	判断输入的参数名是否与表格中的已有记录重名
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function checkUnique(curParaName)
    {
       
     
       var errorFlag = false;
      paramTable.ClearV();
         var rowNum = paramTable.GetRowNumber();
         var checkBeginChar = /^@+/;
    
        var rs = paramTable.rs_main;
        var i = 0 ;
        if (curParaName != "") {
           
             if (!checkBeginChar.test(curParaName)) {
                errorFlag = true;
                alert("<%=Resources.Template.beginChar%>");
                      
             }
             if (!errorFlag) {
                if (rs.recordcount > 0){
                    rs.moveFirst();
                    while (!rs.EOF){
                        
                        if ((curParaName.toLowerCase() == rs.Fields(0).value.toLowerCase()) && (rowNum != i)){
                           
                                errorFlag = true;
                                alert("<%=Resources.Template.dupParaName%>");
                                break;
                          
                            
                         }
                        i = i + 1;
                        rs.moveNext();
                    } 
                }
            }
        }

        return errorFlag;
    }
	
	
	 
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	MyAfterNew
    //| Author		:	Lucy Liu
    //| Create Date	:	4/24/2009
    //| Description	:	点击param table一行时的事件
    //| Input para.	:	
    //| Ret value	:	
     //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function MyAfterNew()
    {
         //    <bug>
        //        BUG NO:ITC-992-0061 
        //        REASON:根据type类型获取符合的tempalte parameter name
        //    </bug>
        var rownum=paramTable.GetRowNumber();
        if(rownum == -1 || rownum > paramTable.rs_main.recordcount)
        {
            return;
        }
        var type;
        //获取记录中type列的内容
        if (sqlType[0].checked)
	    { 
	        type = document.getElementById("typeList").value;
	    } else {
	       var rowItemArr = paramTable.RowStr.split('<%=Constants.COL_DELIM%>');
	       type = rowItemArr[1];
	       
	       
	    }
	    if (type == "") {
	        return;
	    }
	    //根据type获取它的sql类型
	    var ret = com.inventec.template.manager.TemplateManager.getDataType(type);
        if (ret.error != null) {
            errorFlag = true;
            alert(ret.error.Message);
            return;
        } else{
            //保留起来
            sourceDataType = ret.value;
        }
        //根据type显示template parameter
        changeType(type);
    }
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	changeType
    //| Author		:	Lucy Liu
    //| Create Date	:	4/24/2009
    //| Description	:	根据所选type，显示符合的template parameter
    //| Input para.	:	
    //| Ret value	:	
     //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function changeType(type)
    {
       
       var errorFlag = false;
       var ret;
       var sourceDataType
      
        ret = com.inventec.template.manager.TemplateManager.getDataType(type);
        if (ret.error != null) {
            errorFlag = true;
            alert(ret.error.Message);
        } else{
            var dataType = ret.value;
            if (dataType == sourceDataType) {
                //如果所选的type的sql类型与原类型一样，则什么都不干
                return;
            }
            //保留起来
            sourceDataType = dataType;
//            document.getElementById("templatePram").innerHTML= "";
            //清空template parameter select内容
            for(i=templateParam.options.length-1;i>=0;i--)   
                templateParam.options.remove(i);   
            
            //按照类型添加
            if (dataType == "<%=Constants.EDIT_CONDITION_FIELDTYPE_NUMERIC%>") {
                for (var i=0; i< templatePram.length; i++) {
                    if (templatePram[i].ParaType ==  "<%=Constants.PARAM_INT_TYPE%>") {
                    
                        var oOption = document.createElement("OPTION");
		                oOption.value = templatePram[i].ParaName;
		                oOption.text  = templatePram[i].ParaName;
	                    templateParam.options.add(oOption);
                      
                    }       
               }
            } else {
               
                for (var i=0; i< templatePram.length; i++) {

                    if (templatePram[i].ParaType ==  "<%=Constants.PARAM_STRING_TYPE%>") {
                        
                        var oOption = document.createElement("OPTION");
		                oOption.value = templatePram[i].ParaName;
		                oOption.text  = templatePram[i].ParaName;
	                    templateParam.options.add(oOption);
                      
                    } 
               }
            
            }
                
        }
    
        
    }
    
    function refreshSp()
	{
        var spName = document.getElementById("spList").value; 
        if (spName != "") 
        {
        
            var rtnSpContent = com.inventec.fisreport.manager.DatasetManager.getSpSqlText(serverList.value, databaseList.options(databaseList.selectedIndex).text, spName); 
            var spContentStr = ""; 
        
            if(rtnSpContent.error != null)
            {
                alert(rtnSpContent.error.Message);
            }
            else
            {     
                var rtnSpContentValue = rtnSpContent.value;
                
                for(var i = 0; i < rtnSpContentValue.Rows.length; i++)
                {
                    spContentStr += rtnSpContentValue.Rows[i]["Text"] + "<%=Constants.COL_DELIM%>";
                }       
                
                document.getElementById("sqlContainer").innerText = spContentStr;
            }
            
            getConditionTable();
        }
	} 
	initPage(); 
</script>
</html>

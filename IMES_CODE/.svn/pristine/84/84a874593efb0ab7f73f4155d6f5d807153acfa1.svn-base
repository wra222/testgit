<%--
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: selectuser.aspx
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2008-12-19   itc98079     Create 
 * Known issues:Any restrictions about this file 
 * fix itc-1330-0281
--%>

<%@ Page Language="C#" AutoEventWireup="true" Theme="MainTheme" CodeFile="selectuser.aspx.cs" Inherits="webroot_aspx_authorities_selectuser" %>
<%@ Import Namespace="com.inventec.system" %>
<%@ Import Namespace="com.inventec.imes.manager" %>
<%@ Import Namespace="log4net" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Select User</title>
    <style>
        div{scrollbar-base-color:rgb(198,197,195);scrollbar-shadow-color:Transparent;scrollbar-highlight-color:rgb(246,247,239);}
    </style>
</head>
<fis:header id="header1" runat="server"/>
<script language="javascript" type="text/javascript">
	var ShowItemTable = null;
	var diagArgs = window.dialogArguments; //undefined
	var bFirstEnter = true;
	var editorID = diagArgs.editorID;

	//alert(diagArgs.operateType);
</script>

<script language="javascript" type="text/javascript">
	function fOnBodyLoad()
	{
		fillAllDomains();
	}


	function fillAllDomains()
	{
		var objSelDomain = document.getElementById("idSelDomain");

		var dtAllDomains = webroot_aspx_authorities_selectuser.getAllDomainsByApplication();
		
		if (dtAllDomains.error != null)
		{
			alert(dtAllDomains.error.Message);
		}
		else
		{
		    fillSelectForDomainDatatb("idSelDomain", dtAllDomains.value);
		    objSelDomain.selectedIndex = -1;
		    //onDomainChange(objSelDomain.value);
		}		
		
		getRecordSet("-1","-1","-1","-1");
	}


	function onDomainChange(par)
	{
		//alert("onDomainChange");
		var objSelDomain = document.getElementById("idSelDomain");
		var objSelCompany = document.getElementById("idSelCompany");
		var objSelDept = document.getElementById("idSelDept");
		var objSearch = document.getElementById("idSearchTxt");
		
		objSelCompany.innerHTML = "";
		objSelDept.innerHTML = "";
		
		var dtCompanies = webroot_aspx_authorities_selectuser.getCompaniesByDomain(par);
		fillSelectForCompanyDatatb("idSelCompany", dtCompanies.value);
	    objSelCompany.selectedIndex = -1;
        getRecordSet("-1","-1","-1","-1");
        //onCompanyChange(objSelCompany.value);

	}


	function onCompanyChange(par)
	{
		var objSelDomain = document.getElementById("idSelDomain");
		var objSelCompany = document.getElementById("idSelCompany");
		var objSelDept = document.getElementById("idSelDept");
		var objSearch = document.getElementById("idSearchTxt");
		
		objSelDept.innerHTML = "";

	    var dtDepartments = webroot_aspx_authorities_selectuser.getDeptsByCompany(objSelDomain.value, par);
	    fillSelectForDeptDatatb("idSelDept", dtDepartments.value);
	    objSelDept.selectedIndex = -1;
        getRecordSet("-1","-1","-1","-1");
	    //onDepartmentChange(objSelDept.value);
		
	}


	function onDepartmentChange(par)
	{
		var objSelDomain = document.getElementById("idSelDomain");
		var objSelCompany = document.getElementById("idSelCompany");
		var objSearch = document.getElementById("idSearchTxt");
		
		getRecordSet(objSelDomain.value, objSelCompany.value, par, Trim(objSearch.value));
	}

    function onClickOK(){
        if(diagArgs.interfaceType == "singleuser"){
            singleuser_onClickOK();
        }else{
            adduser_onClickOK();
        }
    }

	function singleuser_onClickOK()
	{
		//var objGroupName = document.getElementById("idGroupName");
		//var objComment = document.getElementById("idComment");
		//objGroupName.value = Trim(objGroupName.value);
		ShowWait();
		var strHeadInfo = "<%=AttributeNames.USER_ID%>" + "<%=Constants.ROW_DELIM%>" + "string";
		//fGetTableCheckedStringForSelectUser(pTable, pCheckBoxCol, pModifyCol, pColDelimiter)
		var strCollectionString = fGetTableCheckedStringForSelectUser(ShowItemTable, 0, "2", ",");

		//alert(strCollectionString);
		if ("" == strCollectionString)
		{
			alert("Please select at least one user.");
			HideWait();
			return;
}
		var result = webroot_aspx_authorities_selectuser.singleuser_onClickOK(strCollectionString, editorID);

		if (null != result.error)
		{
			alert(result.error.Message);
			HideWait();
			return;
		}
		else
		{
			//window.returnValue = "true";
			window.returnValue = result.value;
  		    window.close();
		}
	}
	
	function adduser_onClickOK()
	{
		//var objGroupName = document.getElementById("idGroupName");
		//var objComment = document.getElementById("idComment");
		//objGroupName.value = Trim(objGroupName.value); //???
		ShowWait();
		var strHeadInfo = "<%=AttributeNames.USER_ID%>" + "<%=Constants.ROW_DELIM%>" + "string";
		//fGetTableCheckedStringForSelectUser(pTable, pCheckBoxCol, pModifyCol, pColDelimiter)
		var strCollectionString = fGetTableCheckedStringForSelectUser(ShowItemTable, 0, "2", ",");

		//alert(strCollectionString);
		if ("" == strCollectionString)
		{
			alert("Please select one or more users.");
			HideWait();
			return;
		}
		
		var result = webroot_aspx_authorities_selectuser.adduser_onClickOK(strCollectionString, diagArgs.groupName, editorID);
		
		if (null != result.error)
		{
			alert(result.error.Message);
			HideWait();
			return;
		}
		else
		{
			window.returnValue =result.value;
			window.close();
		}
	}	

	function onClickCancel()
	{
		window.close();
	}

	function onClickGo()
	{
		var objSelDomain = document.getElementById("idSelDomain");
		var objSelCompany = document.getElementById("idSelCompany");
		var objSelDept = document.getElementById("idSelDept");
		var objSearch = document.getElementById("idSearchTxt");

		if (objSelDomain.selectedIndex == -1)
		{
			alert("Please select domain.");
			return;
		}
		if (objSelCompany.selectedIndex == -1)
		{
			alert("Please select company.");
			return;
		}
		if (objSelDept.selectedIndex == -1)
		{
			alert("Please select department.");
			return;
		}

		var rtn = webroot_aspx_authorities_selectuser.getTableData(objSelDomain.value, objSelCompany.value, objSelDept.value, Trim(objSearch.value));

		//后台提供的DataTable
		if (rtn.error != null)
		{
			alert(rtn.error.Message);
		}
		else
		{
			//生成记录集
			var rs = createRecordSet(rtn.value);
			//显示表格
			showTable(rs);
		}
	}

	/*
    ' ======== ============ =============================
    ' Description: 利用commonFunction.js中的createRecordSet
    '              方法获得表格的记录集
    ' Author: LZ
    ' Side Effects:
    ' Date:2008-11
    ' ======== ============ =============================
	*/  
	function getRecordSet(parDomain, parCompany, parDept, parSearch)
	{
		var rtn = webroot_aspx_authorities_selectuser.getTableData(parDomain, parCompany, parDept, parSearch);
		
		//后台提供的DataTable
		if (rtn.error != null)
		{
			alert(rtn.error.Message);
		}
		else
		{
			 //生成记录集
			 var rs = createRecordSet(rtn.value);
			 //显示表格
			 showTable(rs);
		 }
		 bFirstEnter = false;
	}

	/*
    ' ======== ============ =============================
    ' Description: display table edit
    ' Author: LZ
    ' Side Effects:
    ' Date:2008-11
    ' ======== ============ =============================
	*/ 
	function showTable(rs)
	{
		ShowWait();
		var tableWidth = document.body.clientWidth - 12;
		var tableHeight = document.body.clientHeight - 230;
		//alert(document.body.clientWidth);
		//alert(document.body.clientHeight);
		var tableWidth1 = tableWidth - 18;
		if (ShowItemTable == null)
		{
			ShowItemTable = new clsTable(rs, "ShowItemTable");

			ShowItemTable.HeadStyle("background-color:rgb(133,133,133);font-size:9pt;color:white");//表头背景
			ShowItemTable.arrColor = new Array("rgb(226,235,204)","rgb(212,218,195)");//表体间隔行的颜色
			ShowItemTable.TableLineColor = "rgb(166,166,166)";//表头线
			ShowItemTable.LightBKColor = "rgb(169,217,86)";//高亮行的背景色
			ShowItemTable.LightColor = "black";//高亮行的前景色



			ShowItemTable.Height = tableHeight;
			ShowItemTable.TableWidth = tableWidth;
			ShowItemTable.adjustRateW = 1.006;
			ShowItemTable.UseSort = "TDCData";
			ShowItemTable.ScreenWidth = -1;
			ShowItemTable.ControlPath = "../../commoncontrol/tableEdit/";
			ShowItemTable.Widths = new Array(25, tableWidth1 * 0.5 - 25, tableWidth1 * 0.5, 0, 0, 0);
			ShowItemTable.FieldsType = new Array(0, 0, 0, 0, 0, 0);
			ShowItemTable.HideColumn = new Array(true, true, true, false, false, false);
			ShowItemTable.Title = true;
			ShowItemTable.Divide = "<%=Constants.COL_DELIM%>";
			//ShowItemTable.AfterNew = "MyAfterNew()";
			ShowItemTable.checkBoxColumn = 0;
		   // var preConditionId="<select id='idSelPreCondtion' name='idSelPreCondtion' style='HEIGHT: 17px; WIDTH: 100px' ><option></option></select>"; 
		   // ShowItemTable.outerSelect = true;
		   // ShowItemTable.UseControl(colPreCondition,preConditionId);
			ShowItemTable.UseHTML = true;
		}
		ShowItemTable.rs_main = rs;	
		idDivTableEdit.innerHTML = ShowItemTable.Display();	
		HideWait();
	}


	function fillSelectForDomainDatatb(selId, dataTbValue)
	{
		var objSel = document.getElementById(selId);
		var arr = new Array();
		var dbServerValue;
		var arrName = new Array();
		objSel.innerHTML = "";

		for (var i = 0; i < dataTbValue.Rows.length; i++)
		{
			var option = document.createElement("option");
			//arrName[j] = dataTbValue.Columns[i].Name;
			option.value = dataTbValue.Rows[i]["<%=Constants.TABLE_COLUMN_DOMAIN%>"];
			option.text = dataTbValue.Rows[i]["<%=Constants.TABLE_COLUMN_DOMAIN%>"];

			objSel.add(option);
		}
		if (bFirstEnter)
		{
		    objSel.selectedIndex = 0;
		} else {
		    objSel.selectedIndex = 0;
		}
	}


	function fillSelectForCompanyDatatb(selId, dataTbValue)
	{
		var objSel = document.getElementById(selId);
		var arr = new Array();
		var dbServerValue;
		var arrName = new Array();
		objSel.innerHTML = "";

		for (var i = 0; i < dataTbValue.Rows.length; i++)
		{
			var option = document.createElement("option");
			//arrName[j] = dataTbValue.Columns[i].Name;
			option.value = dataTbValue.Rows[i]["<%=Constants.TABLE_COLUMN_COMPANY%>"];
			option.text = dataTbValue.Rows[i]["<%=Constants.TABLE_COLUMN_COMPANY%>"];

			objSel.add(option);
		}
		if (bFirstEnter)
		{
		    objSel.selectedIndex = 0;
		} else {
		    objSel.selectedIndex = 0;
		}
	}


	function fillSelectForDeptDatatb(selId, dataTbValue)
	{
		var objSel = document.getElementById(selId);
		var arr = new Array();
		var dbServerValue;
		var arrName = new Array();
		objSel.innerHTML = "";

		var optionAll = document.createElement("option");
		//arrName[j] = dataTbValue.Columns[i].Name;
		optionAll.value = "<%=Constants.ALL_OPTION%>";
		optionAll.text = "<%=Constants.ALL_OPTION%>";

		objSel.add(optionAll);
		
		for (var i = 0; i < dataTbValue.Rows.length; i++)
		{
			var option = document.createElement("option");
			//arrName[j] = dataTbValue.Columns[i].Name;
			option.value = dataTbValue.Rows[i]["<%=Constants.TABLE_COLUMN_DEPARTMENT%>"];
			option.text = dataTbValue.Rows[i]["<%=Constants.TABLE_COLUMN_DEPARTMENT%>"];

			objSel.add(option);
		}
		if (bFirstEnter)
		{
		    objSel.selectedIndex = 0;
		} else {
		    objSel.selectedIndex = 0;
		}
	}


	function fGetTableCheckedStringForSelectUser(pTable, pCheckBoxCol, pModifyCol, pColDelimiter)
	{
		var str = "";
		var strCheck = "";
		var i = 0;
		var arrCol = new Array();

		if (pTable.rs_main.recordCount == 0)
			return "";

		pTable.rs_main.moveFirst();
		while (!pTable.rs_main.EOF)
		{
			if (pTable.rs_main.fields(pCheckBoxCol).value == "checked")
			{
				arrCol[i] = pTable.rs_main.fields(parseInt(pModifyCol)).value;
				i++;
			}
			pTable.rs_main.moveNext();
		}

		strCheck = arrCol.join(pColDelimiter);

		if (strCheck == "")
			return "";

		return strCheck;
	}




	function onPressEnter(parBtnID, objInput)
	{
		if (window.event.keyCode == 13)
		{
			if (objInput != undefined)
			{
				//solve the special char,when entered!
				//replaceIllegalCharForSearch(objInput);
			}
			eval(parBtnID + ".fireEvent('onclick');");
			eval("document.body.focus();");
		}
	}
</script>
<body onload="fOnBodyLoad();" class="dialogBody" style="background-color:rgb(210,210,210);">
<table style="width:100%" border="0" cellpadding="6px">
	<tr>
		<td nowrap="true" width="1%">
			<b>Domain:</b>
		</td>
		<td>
			<select id="idSelDomain" onchange="onDomainChange(this.value);" style="width:250px">
			</select>
		</td>
	</tr>
	<tr>
		<td nowrap="true" width="1%">
			<b>Company:</b>
		</td>
		<td>
			<select id="idSelCompany" onchange="onCompanyChange(this.value);" style="width:250px">
			</select>
		</td>
	</tr>
	<tr>
		<td nowrap="true" width="1%">
			<b>Department:</b>
		</td>
		<td>
			<select id="idSelDept" onchange="onDepartmentChange(this.value);" style="width:250px"></select>
		</td>
	</tr>
	<tr>
		<td nowrap="true" width="1%">
			<b>Search:</b>
		</td>
		<td>
			<input id="idSearchTxt" type="text" style="width:225px" onkeypress="onPressEnter('idBtnGo');"/>
			<button id="idBtnGo" onclick="onClickGo();" style="width:25px">
				Go
			</button>
		</td>
	</tr>
</table>
	
	<div id="idDivTableEdit"></div>

<table style="width:100%" border="0" cellpadding="10px">
	<tr>
		<td colspan="2" nowrap="true" align="right">
			<button id="idBtnOK" onclick="onClickOK();" style="width:60px">
				OK
			</button>
			&nbsp;&nbsp;&nbsp;
			<button id="idBtnCancel" onclick="onClickCancel();" style="width:60px">
				Cancel
			</button>
		</td>
	</tr>
</table>
<fis:footer id="footer1" runat="server"/>

<form id="form1" runat="server">
</form>
</body>
</html>

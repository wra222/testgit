<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dashboardList.aspx.cs" Inherits="webroot_aspx_dashboard_dashboardList" %>
<%@ Import Namespace="com.inventec.system" %>

<script type="text/javascript" src="../../commoncontrol/tableEdit/TableData.js"></script>
<script type="text/javascript" src="../../commoncontrol/tableEdit/TableEdit.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<fis:header id="header2" runat="server"/>
<body >
<script language="javascript" type="text/javascript">

    
</script>
    <form id="form1" runat="server">
    </form>
    <table cellpadding="0" border="0" cellspacing="0"  width="100%" >
   <tr>
    <td class="title" style="height:20px">
        <label id="head">&nbsp;&nbsp;Dashboard List</label>
    </td>
	</tr>   
    <tr>
	    <td><div style="height:100%;width:100%" id="showreportdata" ondblclick="return displayClick()"></div></td>
    </tr>   
    <tr style="background-color:White">
	    <td align="right"><br/>
	     <div>
	            <button id="btnNewSMT" style="width:90px" onclick="newClick()">New SMT</button>&nbsp;&nbsp;
	            <button id="btnNewSA" style="width:90px" onclick="newClick()">New SA</button>&nbsp;&nbsp;
	            <button id="btnNewFA" style="width:106px" onclick="newClick()">New FA & PAK</button>&nbsp;&nbsp;
	            <button id="btnEdit" style="width:90px" onclick="editClick();">Edit</button>&nbsp;&nbsp;
	            <button id="btnDelete" style="width:90px" onclick="deleteClick()">Delete</button>&nbsp;&nbsp;
	            <button id="btnDisplay" style="width:90px" onclick="displayClick()">Display</button>&nbsp;&nbsp;
	        </div>
	    </td>
    </tr>
    </table>
</body>
   <fis:footer id="footer1" runat="server"/> 
       
<script type="text/javascript" language="javascript">
    
    var ShowItemTable = null;

    initPage(); //改为在treeView之后执行    
     
    function initPage()
    {
        getRecordSet();
        if(window.parent.isAuthorityDashboard=="True")
        {
            document.getElementById('btnNewSMT').disabled=false;
            document.getElementById('btnNewSA').disabled=false;
            document.getElementById('btnNewFA').disabled=false;
            document.getElementById('btnEdit').disabled=false;
            document.getElementById('btnDelete').disabled=false;
            document.getElementById('btnDisplay').disabled=false;
        }
        else 
        {
            document.getElementById('btnNewSMT').disabled=true;
            document.getElementById('btnNewSA').disabled=true;
            document.getElementById('btnNewFA').disabled=true;
            document.getElementById('btnEdit').disabled=true;
            document.getElementById('btnDelete').disabled=true;
            document.getElementById('btnDisplay').disabled=true;
        }
    }
     
    function getRecordSet()
    { 
             var rtn;    
             try
             {
                if(window.parent.isAuthorityDashboard=="True" )
                {
                    rtn = com.inventec.portal.dashboard.common.DashboardCommon.GetDashboardWindowList();
                }
                else
                {
                    rtn = com.inventec.portal.dashboard.common.DashboardCommon.GetDashboardWindowListEmpty();
                }                
             }
             catch(e)
             {
                alert("Can't get data from server.");
                return;
             }
            
            if (rtn.error != null) 
            {
                alert(rtn.error.Message);
                return;
            }
            else 
            { 
                if(rtn.value==null)
                {
                    return;
                }
                //生成记录集
                var rs = createRecordSet(rtn.value);
                //显示表格
                showTable(rs);
            }
    }
        
    function showTable(rs){

	    var tableWidth = document.body.clientWidth-5;
	    //var tableWidth1 = tableWidth - 17;
        var clientH = document.body.clientHeight;
        
        if (ShowItemTable == null){
	        ShowItemTable = new clsTable(rs, "ShowItemTable");
	        if (clientH > 100){
	    	    ShowItemTable.Height = clientH  - 88;
	        } else {
	    	    ShowItemTable.Height = clientH;
	        }
		    ShowItemTable.TableWidth = tableWidth;    	    
		    var tmpWidths = new Array(0,24,24,14,14,12);  
            ShowItemTable.Widths = getTableWidthsFixColumn(tmpWidths,tableWidth);
            ShowItemTable.FieldsType = new Array( 0, 0, 0, 0, 0, 0);
            ShowItemTable.HideColumn = new Array(false,true, true, true, true,true);
	        ShowItemTable.UseSort = "TDCData";
		    ShowItemTable.Divide = "<%=Constants.COL_DELIM%>";
		    ShowItemTable.ScreenWidth = -1;
		    //ShowItemTable.AfterNew = "MyAfterNew()";
            //ShowItemTable.BeforeSave = "MyBeforeSave()";

		    //ShowItemTable.UpDown = "fUpdateIssueDetail()";
		    ShowItemTable.UseHTML = false;
	    }
	    
	    ShowItemTable.rs_main = rs;
	    showreportdata.innerHTML = ShowItemTable.Display();
	    
	    //HideWait();
    }
    
	ShowItemTable.initTableData = function()
	{
		TDCData = new TableData();
		TDCData.UseHeader = true;
		TDCData.recordset = this.rs_main;
	}
    
    function getTableWidthsFixColumn(widths,tableWidth){

//    var winWidth =window.document.body.offsetWidth;
//    var winHeight =window.document.body.offsetHeight;
//    tableObj.ScreenWidth=-1;
//    var width =  parseInt(0.98*winWidth);
//    if(tableWidth!=null){
//    	 width=tableWidth;
//    }
//    tableObj.TableWidth = String(width);
    var width=tableWidth;
    width = width-19;
    var total=0;
    for(var i=0;i<widths.length;i++){
	    total=total+widths[i];
    }
    if(total == 0){
        total=1;
    }
    var returnArray =new Array();
    var count=0;
    var countWidth=0;
    for(var i=0;i<widths.length;i++){
        count=i;
        returnArray[i] = parseInt(widths[i]*width/total);
        countWidth+=returnArray[i];
    }
    returnArray[count]=parseInt(returnArray[count]+width-countWidth);
    return returnArray;
}
    
//function MyBeforeSave()
//{
//    var rownum=ShowItemTable.GetRowNumber();
//    if(rownum == -1 || rownum > ShowItemTable.rs_main.recordcount)
//    {
//        return false;
//    }
//    
//    return true;
//}

//function MyAfterNew()
//{

//    var rownum=ShowItemTable.GetRowNumber();
//    if(rownum == -1 || rownum > ShowItemTable.rs_main.recordcount)
//    {
//        document.getElementById("btnEdit").disabled = true;
//        document.getElementById("btnDelete").disabled = true;
//        document.getElementById("btnDisplay").disabled = true;
//    } 
//    else
//    {
//        document.getElementById("btnEdit").disabled = false;
//        document.getElementById("btnDelete").disabled = false;
//        document.getElementById("btnDisplay").disabled = false;
//    }

//}
    //create a new template
    function newClick()
    {
        switch(event.srcElement.id)
        {
            case "btnNewFA":
                 window.parent.frames("main").location.href="dashboardSetting.aspx";
                 break;
            case "btnNewSA":
                  window.parent.frames("main").location.href="dashboardSmtSetting.aspx?stageType="+"<%=Constants.SA_STAGE %>";
                 break;   
            case "btnNewSMT":                   
                 window.parent.frames("main").location.href="dashboardSmtSetting.aspx?stageType="+"<%=Constants.SMT_STAGE %>";
                 break;                     
        }
    }
    
    //edit template
    function editClick() 
    {
        var rownum=ShowItemTable.GetRowNumber();
        if(rownum <0 || rownum >= ShowItemTable.rs_main.recordcount||ShowItemTable.IsEmpty==true)
        {
            return;
        } 
        
        var rowItem = ShowItemTable.RowStr;
        if(rowItem == null)
        {
            return;
        }
        var rowItemArr = rowItem.split("<%=Constants.COL_DELIM%>"); 
        var uuid = rowItemArr[0];
       
        var stageType=getStageTypeByWinId(uuid);
    
        if(stageType=="<%=Constants.FA_STAGE%>")
        {
            //!!!check它的作用
            window.parent.frames("menu").tree.searchInChildNodes("UUID", uuid, true);       
            window.parent.frames("main").location.href="dashboardSetting.aspx?uuid=" + uuid;
        }
        else
        {
            window.parent.frames("menu").tree.searchInChildNodes("UUID", uuid, true);       
            window.parent.frames("main").location.href="dashboardSmtSetting.aspx?uuid=" + uuid+"&stageType="+String(stageType);
        }
    }

function deleteClick()
{
    var deleteFileInfo = "Are you sure you want to delete this dashboard template?";
    
    var rownum=ShowItemTable.GetRowNumber();
    if(rownum <0  || rownum >= ShowItemTable.rs_main.recordcount||ShowItemTable.IsEmpty==true)
    {
        return;
    } 
    
   var rowItem = ShowItemTable.RowStr;
   if(rowItem == null)
   {
       return;
   }
   var rowItemArr = rowItem.split("<%=Constants.COL_DELIM%>"); 
   var uuid = rowItemArr[0];

    if(!confirm(deleteFileInfo))
    {
        return;
    }
    
    //modify for publish file method  //!!!
    var stageType=getStageTypeByWinId(uuid);
    
    var deleteResult;
    if(stageType=="<%=Constants.FA_STAGE%>")
    {
        deleteResult = com.inventec.portal.dashboard.Fa.DashboardManager.DeleteDashboardWindowSetting(uuid);
    }
    else
    {
        deleteResult = com.inventec.portal.dashboard.Smt.DashboardManager.DeleteDashboardWindowSetting(uuid);
    }
    
    
    if (deleteResult.error != null) 
    {
        alert(deleteResult.error.Message);
    }
    else 
    { 
        //返回uuid

        window.parent.frames("menu").tree.freshCurrentNode(0); 
    }       
 
}

function displayClick()
{
   
    var rownum=ShowItemTable.GetRowNumber();
    if(rownum <0  || rownum >= ShowItemTable.rs_main.recordcount||ShowItemTable.IsEmpty==true)
    {
        return;
    } 
    
   var rowItem = ShowItemTable.RowStr;
   if(rowItem == null)
   {
       return;
   }
   var rowItemArr = rowItem.split("<%=Constants.COL_DELIM%>"); 
   var uuid = rowItemArr[0];
   
   window.parent.showDisplay(uuid);
      
}

function getStageTypeByWinId(winId)
{
     var rtn;    
     try
     {
        rtn = com.inventec.portal.dashboard.common.DashboardCommon.GetStageType(winId);
     }
     catch(e)
     {
        alert("Can't get data from server.");
        return;
     }
    
    if (rtn.error != null) 
    {
        alert(rtn.error.Message);
        return;
    }
    else 
    { 
        return rtn.value;
    }

}

</script> 
</html>


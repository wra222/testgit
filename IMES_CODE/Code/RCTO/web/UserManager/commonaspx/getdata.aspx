<%@ Page Language="C#" AutoEventWireup="true" CodeFile="getdata.aspx.cs" Inherits="webroot_commonaspx_getdata" %>

<%@ Import Namespace="com.inventec.system" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>getdata</title>
</head>

<script language="JavaScript" type="text/javascript" src="../commoncontrol/tableEdit/TableData.js"></script>

<script language="JavaScript" type="text/javascript" src="../commoncontrol/tableEdit/TableEdit.js"></script>

<script type="text/javascript" language="javascript" src="../commoncontrol/function/commonFunction.js"></script>

<script type="text/javascript" language="javascript"> 
var ShowItemTable = null;
var colItemID = 0;
var strItemID = "";
<%--
    ' ======== ============ =============================
    ' Description: 将表格数据源转换为recordset
    ' Author: kan
    ' Side Effects:
    ' Date:2008-11-18
    ' ======== ============ =============================
--%>
        function getRecordSet()
        {        
            //后台提供的DataTable
            var rtn = webroot_commonaspx_getdata.getTable().value;
          
            if (rtn.error != null) 
            {
                alert(rtn.error);
            }
            else 
            { 
               //生成记录集
               var rs = createRecordSet(rtn);
               //显示表格
               showTable(rs);

            }
//============若显示第二个表格===============  
//            var record = webroot_commonaspx_getdata.getTableSecond().value;          
//            if (record.error != null) 
//            {
//                alert(record.error);
//            }
//            else 
//            { 
//               var rs2 = createRecordSet(record);
//               showTable2(rs2);
//            }
            
        }
        


function showTable(rs) {     
     
    var tableWidth = 400 ;
    var tableHeight = 100;
    var tableWidth1 = tableWidth - 9;
    if (ShowItemTable == null)
    { 
        ShowItemTable = new clsTable(rs, "ShowItemTable");
        ShowItemTable.Height = tableHeight;
        ShowItemTable.TableWidth = tableWidth;
        ShowItemTable.UseSort = "TDCData";
        ShowItemTable.ControlPath = "../../commoncontrol/";
        ShowItemTable.Widths = new Array(tableWidth1 * 0.25, tableWidth1 * 0.15, tableWidth1 * 0.27, tableWidth1 * 0.1);
        ShowItemTable.FieldsType = new Array( 0, 0, 0, 0);
        ShowItemTable.HideColumn = new Array(true, true, true, true);
        ShowItemTable.Title = true;
        ShowItemTable.Divide = "<%=Constants.COL_DELIM%>";
        ShowItemTable.UseHTML = true;

    }

    ShowItemTable.rs_main = rs;	
    idDiv.innerHTML = ShowItemTable.Display();	
} 

//============若显示第二个表格=============== 
//var tableCtl;
//function showTable2(rs) {     
//     
//    var tableWidth = 400 ;
//    var tableHeight = 100;
//    var tableWidth1 = tableWidth - 9;
//    if (tableCtl == null)
//    {
// 
//        tableCtl = new clsTable(rs, "tableCtl");
//        tableCtl.Height = tableHeight;
//        tableCtl.TableWidth = tableWidth;
//        tableCtl.UseSort = "TDCData2";
//        tableCtl.ControlPath = "../../commoncontrol/";
//        tableCtl.Widths = new Array(tableWidth1 * 0.25, tableWidth1 * 0.15, tableWidth1 * 0.27, tableWidth1 * 0.1);
//        tableCtl.FieldsType = new Array( 0, 0, 0, 0);
//        tableCtl.HideColumn = new Array(true, true, true, true);
//        tableCtl.Title = true;
//        tableCtl.Divide = "<%=Constants.COL_DELIM%>";
//        tableCtl.UseHTML = true;

//    }

//    tableCtl.rs_main = rs;	    
//    tableCtl.initTableData = function(){
//        TDCData2 = new TableData();
//        TDCData2.UseHeader = true;    
//        TDCData2.recordset=this.rs_main;
//    }
//    
//    idDiv2.innerHTML = tableCtl.Display();	
//}       
</script>

<body>
    <div id="idDiv">
        aaa</div>
    <div id="idDiv2">
        aaa</div>
    <form id="Form1" method="post" runat="server">
    </form>
</body>
</html>

<script type="text/javascript" language="javascript">
getRecordSet();
</script>


<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_datasource_showTableContent, App_Web_showtablecontent.aspx.af99fe74" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Open Table</title>
    <script language="JavaScript" type="text/javascript" src="../../commoncontrol/tableEdit/TableData.js"></script>
    <script language="JavaScript" type="text/javascript"  src="../../commoncontrol/tableEdit/TableEdit.js"></script>
    <script language=javascript type="text/javascript" src="../../commoncontrol/function/commonFunction.js"></script>

</head>
<fis:header id="header2" runat="server"/>
<body class="dialogBody">
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
    <table width="100%">
    <tr>
        <td><div class="tabledatas" id="divtableid"></div></td>
    </tr>
    <tr>
        <td align=right>
            <br />
            <button id="btnOk" onclick="fOk()">OK</button>
        </td>
    </tr>
    </table>
</body>
<fis:footer id="footer1" runat="server"/>
<script type="text/javascript">
    
    var dialogPar = window.dialogArguments;
    var serverId = dialogPar.serverId;
    var databaseName=dialogPar.databaseName;
    var tableName=htmDecodeString(dialogPar.tableName);
    
    var gTable = null;
    
    ShowWait();
    document.body.onload = function ()
    {
        getServerInfo(serverId);
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: �õ�Server��Ϣ
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>       
    function getServerInfo(sId)
    {
        var rtn = com.inventec.template.manager.DataSourceManager.GetDataServerInfo(sId);
        if (rtn.error != null)
        {
            alert(rtn.error.Message);
            getEmptyTable();
            return;
        }
        
        var serverTable = rtn.value;
        if (serverTable.Rows.length > 0)
        {
            getTableInfo(serverTable);
        }
        else 
        {
            getEmptyTable();
        }
        
    }

    <%--
        ' ======== ============ =============================
        ' Description: �õ�����Ϣ
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>
    function getTableInfo(serverTable)
    {
        var _datasourceInfo = com.inventec.template.manager.DataSourceManager.GetNewDicitionary().value;

        setKeyValueOfDic(_datasourceInfo, "<%=AttributeNames.DATA_SERVER_NAME %>",serverTable.Rows[0]["name"]);
        setKeyValueOfDic(_datasourceInfo, "<%=AttributeNames.DATA_SERVER_USERNAME %>",serverTable.Rows[0]["userName"]);
        setKeyValueOfDic(_datasourceInfo, "<%=AttributeNames.DATA_SERVER_PWD %>",serverTable.Rows[0]["password"]);
        setKeyValueOfDic(_datasourceInfo, "<%=AttributeNames.DATA_BASE_NAME %>",databaseName);
        setKeyValueOfDic(_datasourceInfo, "<%=AttributeNames.DATA_TABLE_NAME %>",tableName);

        var rtnObj = com.inventec.template.manager.DataSourceManager.GetTableInfoList(_datasourceInfo);
        if (rtnObj.error != null)
        {
            alert(rtnObj.error.Message);
            getEmptyTable();
            return ;
        }
        
        var tableList = rtnObj.value;
        var columnCount = tableList.Columns.length;
        
        var rs = createRecordSet(tableList);

        //��ʾ���
        showTable(rs,columnCount); 
    }
  
    <%--
        ' ======== ============ =============================
        ' Description: �����ȡ�ձ���Ϣ
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>  
    function getEmptyTable()
    {
        var rtn = webroot_aspx_datasource_showTableContent.GetEmptyTable();
        if (rtn.error != null)
        {
            alert(rtn.error.Message);
            return;
        }
        
        var tableList = rtn.value;
        var columnCount = tableList.Columns.length;
        
        var rs = createRecordSet(tableList);

        //��ʾ���
        showTable(rs,columnCount); 
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ��ʾDataBase���ʹ�ñ��ؼ�
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%> 
    function showTable(rs,columnCount){

        var firstLineWidth = false;
        
        if (columnCount < 1)
        {
            columnCount = 1;
        }
        
        if (columnCount > 1)
        {
            firstLineWidth = true;
        }
	    var tableWidth = document.body.clientWidth-14;
	    var tableWidth1 = tableWidth - 10;
        var clientH = document.body.clientHeight-10;
        var singleWidth ;
        
        if ( firstLineWidth == true)
        {
           singleWidth = (tableWidth1-40) / (columnCount - 1 ) ;
        }
        else
        {
            singleWidth = tableWidth1 / columnCount ;
        }
        
        if (singleWidth < 80)
        {
            singleWidth = 80;
        }
        
        var widthArray = new Array(columnCount);
        var hideArray = new Array(columnCount);
        var fieldArray = new Array(columnCount); 
        for (var i = 0; i < columnCount; ++i)
        {
            widthArray[i] = singleWidth;
            hideArray[i] = true;
            fieldArray[i] = 0;
        }
        
        fieldArray[0] = "num";
        
        if (firstLineWidth == true)
        {
            widthArray[0] = 30;
        }
        
        
        if (gTable == null){
	        gTable = new clsTable(rs, "gTable");
	        if (clientH > 100){
	    	    gTable.Height = clientH - 90;
	        } else {
	    	    gTable.Height = clientH;
	        }
		    gTable.TableWidth = tableWidth;    	    
	        gTable.Widths = widthArray;
	        gTable.HideColumn = hideArray;
    	    
	        gTable.FieldsType = fieldArray;
	        gTable.UseSort = "TDCData";
		    gTable.Divide = "<%=Constants.COL_DELIM%>";
		    gTable.ScreenWidth = -1;
		    //gTable.UpDown = "fUpdateIssueDetail()";
		    gTable.UseHTML = true;
	    }
      	
	    gTable.rs_main = rs;
	    divtableid.innerHTML = gTable.Display();
	    
	    HideWait();
    }
    
    function fOk()
    {
        window.close();
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: �ж�Ϊ��
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%> 
    function changeNull(val)
    {
        if ("undefined" == val || null == val)
        {
            val = "";
        }
        
        return val;
    }</script>
</html>

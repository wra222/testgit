<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_dataserver_dataSourceSetting, App_Web_datasourcesetting.aspx.af99fe74" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language="JavaScript" type="text/javascript" src="../../commoncontrol/tableEdit/TableData.js"></script>
    <script language="JavaScript" type="text/javascript"  src="../../commoncontrol/tableEdit/TableEdit.js"></script>
    <script language=javascript type="text/javascript" src="../../commoncontrol/function/commonFunction.js"></script>
</head>
<fis:header id="header2" runat="server"/>
<body leftMargin="0" topMargin="0" rightMargin="0">
    <form id="form1" runat="server">
    </form>
    <table width="100%">
        <tr>
            <td style="width:100%;" class="title" id="title"></td>
        </tr>
        <tr>
            <td><div class="tabledatas" id="divtableid"></div></td>
        </tr>
        <tr>
            <td align=right>
            <br />
                <button id="btnNew" onclick="fAdd()">New</button>&nbsp;&nbsp;
                <button id="btnEdit" onclick="fEdit()" disabled>Edit</button>&nbsp;&nbsp;
                <button id="btnDelete" onclick="fDelete()" disabled>Delete</button>
            </td>
        </tr>
    </table>
</body>
<fis:footer id="footer1" runat="server"/>
</html>
<script type="text/javascript" language=javascript>
    var gTable = null;
    var addReturnValue = null;
    var gFeature = "center:yes; dialogHeight:420px; dialogWidth:460px; help:no; status:no; resizable:no; scroll:no;";
    var gFeatureDelete = "center:yes; dialogHeight:130px; dialogWidth:330px; help:no; status:no; resizable:no; scroll:no;";
    
    var type = changeNull("<%= Request.Params[0]%>");
    var treeId = changeNull("<%= Request.Params[1]%>");
    var text = changeNull("<%= Request.Params[2]%>");
    var nodeuuid = changeNull("<%= Request.Params[3]%>");

    ShowWait();
    document.body.onload=function()
    {
        load();
    }

    function load()
    {
        document.getElementById("title").innerText=text;
        getData();
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ��ȡDataserver�����Ϣ
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>
    function getData()
    {
        var _nodeId = treeId;
        //alert(_nodeId);
        var rtn = com.inventec.template.manager.DataSourceManager.GetDataSourceSettingList(_nodeId);

        if (rtn.error != null) 
        {
            getEmptyTable();
            alert(rtn.error.Message);
        }
        else 
        { 
           //���ɼ�¼��
           var rs = createRecordSet(rtn.value);

           //��ʾ���
           showTable(rs);
        }
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ��ȡ�ձ����Ϣ
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>
    function getEmptyTable()
    {
        var obj = webroot_aspx_dataserver_dataSourceSetting.GetEmptyDataServerList();
        if (obj.error != null)
        {
            alert(obj.error.Message);
        }
        var rtn = obj.value;
        var rs = createRecordSet(rtn);

        //��ʾ���
        showTable(rs);    
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ��ʾDataBase���ʹ�ñ��ؼ�
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%> 
    function showTable(rs){

	    var tableWidth = document.body.clientWidth-5;
	    var tableWidth1 = tableWidth - 10;
        var clientH = document.body.clientHeight;
        
        if (gTable == null){
	        gTable = new clsTable(rs, "gTable");
	        if (clientH > 100){
	    	    gTable.Height = clientH -15 - 90;
	        } else {
	    	    gTable.Height = clientH;
	        }
		    gTable.TableWidth = tableWidth;    	    
	        gTable.Widths = new Array(1,1,tableWidth1 * 0.17, tableWidth1 * 0.095, tableWidth1 * 0.185, tableWidth1 * 0.185, tableWidth1 * 0.245, tableWidth1 * 0.11);
	        gTable.HideColumn = new Array(false,false,true, true, true, true, true, true);
    	    
	        gTable.FieldsType = new Array(0,0, 0, 0, 0, 0,0, 0);
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
    
    <%--
        ' ======== ============ =============================
        ' Description: �����Ӱ�ť
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>     
    function fAdd(){
	
	    var returnValue = window.showModalDialog("addDataServerAndDatabase.aspx?treeId="+treeId+"&serverId=", "", gFeature);
        returnValue = changeNull(returnValue);
        
	    if (returnValue != ""){
		   
		    //window.parent.frames("menu").tree.freshCurrentNode(0);
		    //window.parent.frames("menu").tree.freshPath[0] = returnValue;
		    
		    freshTreeNodeAndTable();
	    }
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ����༭��ť
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>     
    function fEdit(){
    
        var serverId = getField(1);
	    if (serverId == "")
	    {
	        alert("Please select a dataserver!");
	        return;
	    }
	    
	    var returnValue = window.showModalDialog("editDataServerAndDatabase.aspx?treeId="+treeId+"&serverId=" + serverId, "", gFeature);
	    if (returnValue != ""){
		   
		    //window.parent.frames("menu").tree.freshCurrentNode(0);
		    //window.parent.frames("menu").tree.freshPath[0] = returnValue;
		    
		    freshTreeNodeAndTable();
	    }
	    
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ���ɾ����ť
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' Update Date:  2009-2-24
        ' Reason : ITC-934-0097  A02  (Data Source Setting)DataBase Delete��ȷ�Ͻ���������ɾ��������ȷ�Ͻ�����ͳһ(�ο�����)  
        ' ======== ============ =============================
    --%>     
    function fDelete(){
    
        var treeId = getField(0);
        var serverId = getField(1);
        var serverName = htmDecodeString(getField(2));
        
	    if (serverId == "")
	    {
	        alert("Please select a dataserver!");
	        return;
	    }
	    
	    var deleteFolderInfo = "Are you sure you want to delete the Database server connection ["+ serverName + "]?";
	    
	    if(!confirm(deleteFolderInfo))
        {
            return;
        }
        
//	    var isDelete = window.showModalDialog("deleteDataServerAndDatabase.aspx?serverName="+serverName, "", gFeatureDelete);

//	    if (isDelete != undefined && isDelete != "0"){
//		   
//		    return;
//	    }
	    
	    var _datasourceInfo = com.inventec.template.manager.DataSourceManager.GetNewDicitionary().value;
        setKeyValueOfDic(_datasourceInfo, "<%=AttributeNames.DATA_SERVER_ID %>",serverId);
        setKeyValueOfDic(_datasourceInfo,"<%=AttributeNames.TREE_NODE_ID %>", treeId);
	    
	    var returnValue = com.inventec.template.manager.DataSourceManager.DeleteDataSourceSetting(_datasourceInfo);
    	if (returnValue.error != null)
    	{
    	    alert(returnValue.error.Message);
    	    return;
    	}
    	
    	//ˢ��
    	freshTreeNodeAndTable();
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ת�����ַ���
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
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ˢ���ڵ�ͱ��
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>          
    function freshTreeNodeAndTable()
    {
        window.parent.frames("menu").tree.freshCurrentNode(1);
        //ˢ��
    	//getData();
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ��ȡ��������Ϣ
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>  
    function getField(columnIndex){
	    var fiedValue = "";
    	
	    var nRowNO = gTable.GetRowNumber();
        if(gTable.IsEmpty || nRowNO > (gTable.rs_main.recordcount - 1) || nRowNO < 0 ){
    	    return "";
        }
        
        gTable.rs_main.absolutePosition = gTable.GetRowNumber() * 1 + 1;
	    fiedValue = gTable.rs_main.fields(columnIndex).value;
    	
	    return fiedValue;
    }    
    </script>
    
    <script for=gTableTableBody language=javascript event=ondblclick>
        var rownum = gTable.GetRowNumber() ;

        if(!(rownum == -1 || rownum >= gTable.rs_main.recordcount))
        {
            fEdit();
            
        }
    
    </script>
    
    <script for=gTableTableBody language=javascript event=onclick>
    
    var rownum = gTable.GetRowNumber() ;

    if(!(rownum == -1 || rownum >= gTable.rs_main.recordcount))
    {
        document.getElementById("btnEdit").disabled = false;
        document.getElementById("btnDelete").disabled = false;
    }
    else 
    {
        document.getElementById("btnEdit").disabled = true;
        document.getElementById("btnDelete").disabled = true;
    }
    
    </script>

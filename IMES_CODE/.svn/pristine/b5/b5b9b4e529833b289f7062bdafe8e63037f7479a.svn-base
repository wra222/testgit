<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_datasource_showDataSource, App_Web_showdatasource.aspx.af99fe74" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language="JavaScript" type="text/javascript" src="../../commoncontrol/tableEdit/TableData.js"></script>
    <script language="JavaScript" type="text/javascript"  src="../../commoncontrol/tableEdit/TableEdit.js"></script>
    <script language=javascript type="text/javascript" src="../../commoncontrol/function/commonFunction.js"></script>

</head>
<fis:header id="header2" runat="server"/>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
    <table width="100%" >
        <tr>
            <td colspan=2  class="title" height="20px" id="title">
            </td>
        </tr>
        <tr>
            <td width="22%"><b>Database server name: </b></td>
            <td width="78%"><input id="servername" type="text" readonly style=" width:99%"/></td>
        </tr>
         <tr>
            <td><b>User name:</b></td>
            <td><input id="username" type="text" readonly style=" width:99%" /></td>
        </tr>
         <tr>
            <td><b>Password:</b></td>
            <td><input id="password" type="password" readonly style=" width:99%; background-color:rgb(239,244,250); border-style:groove"/></td>
        </tr>
         <tr>
            <td><b>Alias:</b></td>
            <td><input id="alias" type="text" readonly style=" width:99%"/></td>
        </tr>
        <tr>
            <td >&nbsp;</td>
            <td></td>            
        </tr>
        <tr>
            <td><b>Database selection:</b></td>
            <td></td>
        </tr>
        <tr>
            <td colspan=2>
                <div class="tabledatas" id="sourcediv"></div>
            </td>
        </tr>
        <tr>
            <td colspan=2 align=right>
                <br />
                <button id="Button1" onclick="fEdit()">Edit</button> 
                
            </td>
        </tr>
               
    </table>
</body>
<fis:footer id="footer1" runat="server"/>
</html>    
    <script type="text/javascript" language=javascript>
    var type = changeNull("<%= Request.Params[0]%>");
    var treeId = changeNull("<%= Request.Params[1]%>");
    //var text = changeNull("<%= Request.Params[2]%>");
    var _dataserverId = changeNull("<%= Request.Params[3]%>");
    var parentId = changeNull("<%= Request.Params[4]%>");
    var gTable = null;
    
    var gFeature = "center:yes; dialogHeight:420px; dialogWidth:460px; help:no; status:no; resizable:no; scroll:no;";

    ShowWait();
    document.body.onload = function ()
    {
        //document.getElementById("title").innerText= text;
        getDataServerInfo();
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ��ȡserver��Ϣ
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>  
    function getDataServerInfo()
    {
        com.inventec.template.manager.DataSourceManager.GetDataSourceConnectInfo(_dataserverId, callbackServerInfo);
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ����server��Ϣ
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>     
    function callbackServerInfo(response)
    {
        if (response.error != null ) 
        {
            alert("error" + response.error.message);
            return;
        }
        
        var sources = response.value;
        if(sources != null && typeof(sources) == "object")
        {
            document.getElementById("servername").value = changeNull(sources.getValue("<%=AttributeNames.DATA_SERVER_NAME %>"));
            document.getElementById("alias").value = changeNull(sources.getValue("<%=AttributeNames.DATA_SERVER_ALIAS %>"));
            document.getElementById("username").value = changeNull(sources.getValue("<%=AttributeNames.DATA_SERVER_USERNAME %>"));
            document.getElementById("password").value = changeNull(sources.getValue("<%=AttributeNames.DATA_SERVER_PWD %>"));
            
            document.getElementById("title").innerText = document.getElementById("alias").value;
        }
        //bug no:ITC-992-0019
        //reason:ajax�첽�ص�����û�����ã���Ϊ�ȴ�ajax����ֵ˳��ִ��
        var obj = com.inventec.template.manager.DataSourceManager.GetDataSourceDatabaseList(_dataserverId);
        if (obj.error != null ) 
        {
            alert(obj.error.Message);
            return;
        } 
        else 
        {
            var rtn = obj.value;
            //���ɼ�¼��
            var rs = createRecordSet(rtn);
            
            //��ʾ���
            showTable(rs);  
        }
                    
            //alert(rtn.value);
    }

    <%--
        ' ======== ============ =============================
        ' Description: ����database��Ϣ
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>  
    function callBack(bakInfo) 
    {
        if (bakInfo.error != null ) 
        {
            alert(bakInfo.error.Message);
            return;
        }
        //alert("tt");
        var rtn = bakInfo.value;
        
        //���ɼ�¼��
        var rs = createRecordSet(rtn);

        //��ʾ���
        showTable(rs);
    }
        
    <%--
        ' ======== ============ =============================
        ' Description: ��ʾ���ݣ�ʹ�ñ��ؼ�
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>      
    function showTable(rs){

        var tableWidth = document.body.clientWidth - 5;
        var tableWidth1 = tableWidth - 10;
        var clientH = document.body.clientHeight - 100;
        
        if (gTable == null){
            gTable = new clsTable(rs, "gTable");
            if (clientH > 100){
    	        gTable.Height = clientH - 30 - 90;
            } else {
    	        gTable.Height = clientH;
            }
	        gTable.TableWidth = tableWidth;

    	    gTable.HeadStyle("display:none");
            gTable.Widths = new Array(1,1,tableWidth1-10, 1,1,1);
            gTable.HideColumn = new Array(false,false,true,false,false,false);
    	    
            gTable.FieldsType = new Array(0,0, 0, 0,0,0);
            gTable.UseSort = "TDCData";
	        gTable.Divide = "<%=Constants.COL_DELIM%>";
	        gTable.ScreenWidth = -1;
	        //gTable.UpDown = "";
	        gTable.UseHTML = true;
        }
      	
        gTable.rs_main = rs;
        sourcediv.innerHTML = gTable.Display();
        
        HideWait();
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
	
	    var url = "editDataServerAndDatabase.aspx?treeId="+parentId+"&serverId=" + _dataserverId;
	    
        var returnValue = window.showModalDialog(url, "", gFeature);
       	if (returnValue != ""){
		   
		   freshTreeNodeAndTable();
		    //window.parent.frames("menu").tree.freshCurrentNode(1);
		    //window.parent.frames("menu").tree.freshPath[0] = returnValue;
	    }
    }

    <%--
        ' ======== ============ =============================
        ' Description: �Ա��ؼ��е�checkBox����
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>          
    function beChecked(seq)
    {
	    var rs = gTable.rs_main; 
        rs.AbsolutePosition = seq;
	    
        if (document.all("chk"+seq).checked)
        {
	  	    rs.Fields("checkbox1").value = "<INPUT TYPE='checkbox' id='chk" + seq + "' seq='" + seq + "' onclick=beChecked(" + seq + ") checked>";
	    }
	    else 
	    {		
	  	    rs.Fields("checkbox1").value = "<INPUT TYPE='checkbox' id='chk" + seq + "' seq='" + seq + "' onclick=beChecked(" + seq + ") >";
	    }		 
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ˢ���ڵ� �ͱ��
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>          
    function freshTreeNodeAndTable()
    {
        window.parent.frames("menu").tree.freshCurrentNode(1);
        //ˢ��
    	//getDataServerInfo();
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
    }
    </script>


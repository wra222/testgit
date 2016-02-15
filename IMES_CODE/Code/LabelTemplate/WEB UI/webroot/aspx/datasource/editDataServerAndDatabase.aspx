<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_datasource_addDataSource, App_Web_editdataserveranddatabase.aspx.af99fe74" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Database server connection setting</title>
    <script language="JavaScript" type="text/javascript" src="../../commoncontrol/tableEdit/TableData.js"></script>
    <script language="JavaScript" type="text/javascript"  src="../../commoncontrol/tableEdit/TableEdit.js"></script>
    <script language=javascript type="text/javascript" src="../../commoncontrol/function/commonFunction.js"></script>
    <script language=javascript>
        window.returnValue = "";
    </script>
</head>
<fis:header id="header2" runat="server"/>
<body style="margin:5px" class="dialogBody">
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
    
    <table width="100%">
        <tr>
            <td width="40%"><b>Database server name: </b></td>
            <td><input id="servername" type="text" style ="width:100%" readonly/></td>
        </tr>
         <tr>
            <td><b>User name:</b></td>
            <td><input id="username" type="text" style ="width:100%" onblur="fBlur('username')" /></td>
        </tr>
         <tr>
            <td><b>Password:</b></td>
            <td><input id="password" type="password" style ="width:100%" onblur="fBlur('password')" /></td>
        </tr>
         <tr>
            <td><b>Alias:</b></td>
            <td><input id="alias" type="text" style ="width:100%" maxlength=20/></td>
        </tr>
        <tr>
            <td align="right" colspan=2>
            <br/>
                <button id="dispaly" onclick="getDataSource()" style=" width:130px">Display Database</button>
            </td>
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
            <td colspan=2 align="right">
            <br/>
                <button id="ok"  onclick="fOk()" disabled>OK</button>&nbsp;&nbsp;
                <button id="cancel"  onclick="fCancel()" >Cancel</button>
            </td>
        </tr>
    </table>
</body>
<fis:footer id="footer1" runat="server"/>
</html>
 <script language="javascript">
        var winRtnValue="";
        var serverInfo;
        var serverId = "";
        var gDataSource = null;
        var gTable = null;
        var serverId = changeNull("<%= Request.Params[1]%>");
        var treeId = "<%= Request.Params[0]%>";
        var g_servername = "";
        var g_username = "";
        var g_password = "";
        var g_alias = "";
        var isLoad = true;

        ShowWait();
        document.body.onload = function ()
        {
            getDataServer();
        }
        
        <%--
            ' ======== ============ =============================
            ' Description: �ж��Ƿ��޸���server���ƣ��û���������
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-12-23
            ' ======== ============ =============================
        --%>        
        function fBlur(obj)
        {
            var info = document.getElementById(obj).value;
            switch (obj)
            {
                case 'servername' :
                    alert(g_servername);
                    if (g_servername != "" && g_servername != changeNull(info))
                    {
                        if(!confirm("Are you sure to modify the server name?"))
                        {
                            document.getElementById(obj).value = g_servername;
                        }
                        else
                        {
                            document.getElementById(obj).focus();
                            getEmptyTable();
                        }
                    }
                    break;
                case 'username' :
                    if (g_username != ""  && g_username != changeNull(info))
                    {
                        if(!confirm("Are you sure to modify the user name?"))
                        {
                            document.getElementById(obj).value = g_username;
                        }
                        else
                        {
                            document.getElementById(obj).focus();
                            getEmptyTable();
                        }
                    }
                    break;
                case 'password' :
                    if (g_password != ""  && g_password != changeNullNotTrim(info))
                    {
                        if(!confirm("Are you sure to modify the password?"))
                        {
                            document.getElementById(obj).value = g_password;
                        }
                        else
                        {
                            document.getElementById(obj).focus();
                            getEmptyTable();
                        }
                    }
                    break;                                        
            }                    
        }
        <%--
            ' ======== ============ =============================
            ' Description: ��ȡDataServer��Ϣ
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>
        function getDataServer()
        {
            var _dataserverId = changeNull(serverId);
            var response = com.inventec.template.manager.DataSourceManager.GetDataSourceConnectInfo(_dataserverId, serverCallback);
            
        }
        
        <%--
            ' ======== ============ =============================
            ' Description: ����DataServer��Ϣ
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>         
        function serverCallback(response)
        {
            if (response.error != null ) 
            {
                alert(response.error.message);
                getEmptyTable();
                return;
            }
            
            var sources = response.value;
            
            if(sources != null && typeof(sources) == "object")
            {
                serverInfo = sources;
                document.getElementById("servername").value = changeNull(sources.getValue("<%=AttributeNames.DATA_SERVER_NAME %>"));
                document.getElementById("alias").value = changeNull(sources.getValue("<%=AttributeNames.DATA_SERVER_ALIAS %>"));
                document.getElementById("username").value = changeNull(sources.getValue("<%=AttributeNames.DATA_SERVER_USERNAME %>"));
                document.getElementById("password").value = changeNullNotTrim(sources.getValue("<%=AttributeNames.DATA_SERVER_PWD %>"));
            }
            
            getDefaultDataSource();
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
            var obj = webroot_aspx_datasource_addDataSource.GetEmptyDataBaseList();
            if (obj.error != null)
            {
                alert(obj.error.Message);
                return;
            }
            var rtn = obj.value;
            var rs = createRecordSet(rtn);

            g_servername = "";
            g_username = "";
            g_password = "";
            g_alias = "";
            
            disabledButtionOK();
            //��ʾ���
            showTable(rs);    
        }
        
        <%--
            ' ======== ============ =============================
            ' Description: ʹOk button ����
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>
        function enabledButtionOK()
        {
            document.getElementById("ok").disabled = false;
        } 

        <%--
            ' ======== ============ =============================
            ' Description: ʹOk button ������
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>
        function disabledButtionOK()
        {
            document.getElementById("ok").disabled = true;
        }                
                
        <%--
            ' ======== ============ =============================
            ' Description: ��ȡdatabase��Ϣ
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>
        function getDataSource()
        {
            ShowWait();
            var sname = changeNull(document.getElementById("servername").value);
            var uname = changeNull(document.getElementById("username").value);
            var pwd = changeNullNotTrim(document.getElementById("password").value);
            var alias = changeNull(document.getElementById("alias").value);
            
            setKeyValueOfDic(serverInfo, "<%=AttributeNames.DATA_SERVER_NAME %>",sname);
            setKeyValueOfDic(serverInfo,"<%=AttributeNames.DATA_SERVER_USERNAME %>", uname);
            setKeyValueOfDic(serverInfo,"<%=AttributeNames.DATA_SERVER_PWD %>",pwd);
            
            var obj = com.inventec.template.manager.DataSourceManager.GetSourceSettingDatabaseList(serverInfo,serverId);
            
            if (obj.error != null ) 
            {
                alert(obj.error.Message);
                getEmptyTable();
                return;
            } 
            else 
            {
                var rtn = obj.value;
                //���ɼ�¼��
                var rs = createRecordSet(rtn);
                
                g_servername = sname;
                g_username = uname;
                g_password = pwd;
                g_alias = alias;
                
                //��ʾ���
                showTable(rs);  
                enabledButtionOK();    
            }
            
            //alert(rtn.value);
        }
        
        <%--
            ' ======== ============ =============================
            ' Description: ��ȡ��ʼdatabase��Ϣ
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>
        function getDefaultDataSource()
        {
            var sname = changeNull(document.getElementById("servername").value);
            var uname = changeNull(document.getElementById("username").value);
            var pwd = changeNullNotTrim(document.getElementById("password").value);
            var alias = changeNull(document.getElementById("alias").value);
            
            setKeyValueOfDic(serverInfo, "<%=AttributeNames.DATA_SERVER_NAME %>",sname);
            setKeyValueOfDic(serverInfo,"<%=AttributeNames.DATA_SERVER_USERNAME %>", uname);
            setKeyValueOfDic(serverInfo,"<%=AttributeNames.DATA_SERVER_PWD %>",pwd);
            
            var obj = com.inventec.template.manager.DataSourceManager.GetDataSourceDatabaseList(serverId);
            
            if (obj.error != null ) 
            {
                alert(obj.error.Message);
                getEmptyTable();
                return;
            } 
            else 
            {
                var rtn = obj.value;
                //���ɼ�¼��
                var rs = createRecordSet(rtn);

                g_servername = sname;
                g_username = uname;
                g_password = pwd;
                g_alias = alias;
                
                //��ʾ���
                showTable(rs);  
                if(isLoad)
                {
                    isLoad = false;
                }
                else
                {
                    enabledButtionOK(); 
                }
            }
            
            //alert(rtn.value);
        }        
        
        <%--
            ' ======== ============ =============================
            ' Description: ��ʾdataSource���ʹ�ñ��ؼ�
            ' Author: zhao,qingrong
            ' Side Effects:
            ' Date:2008-12-28
            ' ======== ============ =============================
        --%>
         function showTable(rs){

	        var tableWidth = document.body.clientWidth - 15;
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
	            gTable.Widths = new Array(1,50,tableWidth1-90, 1, 1,30,1);
	            gTable.HideColumn = new Array(false,true,true,false,false,true,false);
        	    
	            gTable.FieldsType = new Array(0,0, 0, 0, 0,0,0);
	            gTable.UseSort = "gDataSource";
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
            ' Description: ���Dataserver��Ϣ�Ƿ�����
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>  
        function fCheckServerInfo()
        {
            var sname = changeNull(document.getElementById("servername").value);
            var uname = changeNull(document.getElementById("username").value);
            var pwd = changeNullNotTrim(document.getElementById("password").value);
            var alias = changeNull(document.getElementById("alias").value);
            
            if (sname == "")
            {
                alert("Please input the server name!");
                document.getElementById("servername").focus();
                return false;
            }
            
            if (uname == "")
            {
                alert("Please input the user name!");
                document.getElementById("username").focus();
                return false;
            }
            
            if (pwd == "")
            {
                alert("Please input the password!");
                document.getElementById("password").focus();
                return false;
            }
            
            if (alias == "")
            {
                alert("Please input the server alias!");
                document.getElementById("alias").focus();
                return false;
            }
            
//            if (!charCodeAtTest(alias))
//            {
//                alert("Invalid input, please try agian!");
//                document.getElementById("alias").value = "";
//                document.getElementById("alias").focus();
//                return false;
//            }
            
            return true;
        }
                
        <%--
            ' ======== ============ =============================
            ' Description: ����Ƿ�ѡ��Database
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>         
        function fCheck()
        {
            var arrCheck = document.getElementsByTagName("input");
            
            var isCheck = false;
            for (var i=0; i < arrCheck.length; ++i)
            {
                if (arrCheck[i].type=="checkbox" )
                {
                    if (arrCheck[i].checked)
                    {
                        isCheck= true;
                        break;
                    }
                } 
            }
            
            if (!isCheck)
            {
                alert("No database selected!");
                return false;
            }
            
            return true;
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
		        rs.Fields("isSelected").value = true;
		    }
		    else 
		    {		
		  	    rs.Fields("checkbox1").value = "<INPUT TYPE='checkbox' id='chk" + seq + "' seq='" + seq + "' onclick=beChecked(" + seq + ") >";
		        rs.Fields("isSelected").value = false;
		    }		 
	    }
        
        <%--
            ' ======== ============ =============================
            ' Description: ���ȷ�ϰ�ť
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%> 	    
	    function fOk()
        {
            if(!fCheckServerInfo())
            {
                return;
            }
            
            ShowWait();
            
            var sname = changeNull(document.getElementById("servername").value);
            var uname = changeNull(document.getElementById("username").value);
            var pwd = changeNullNotTrim(document.getElementById("password").value);
            var alias = trimString(changeNull(document.getElementById("alias").value));
            
            var serverDic = com.inventec.template.manager.DataSourceManager.GetNewDicitionary().value;
            setKeyValueOfDic(serverDic, "<%=AttributeNames.TREE_NODE_ID %>",treeId);
            setKeyValueOfDic(serverDic, "<%=AttributeNames.DATA_SERVER_ID %>",serverId);
            setKeyValueOfDic(serverDic, "<%=AttributeNames.DATA_SERVER_NAME %>",sname);
            setKeyValueOfDic(serverDic, "<%=AttributeNames.DATA_SERVER_ALIAS %>",alias);
            setKeyValueOfDic(serverDic, "<%=AttributeNames.DATA_SERVER_USERNAME %>",uname);
            setKeyValueOfDic(serverDic, "<%=AttributeNames.DATA_SERVER_PWD %>",pwd);
            setKeyValueOfDic(serverDic, "<%=AttributeNames.USER_ID %>","");
            
            var arrList = new Array();//com.inventec.template.manager.DataSourceManager.GetNewList().value;  
            var isCheck = false;
            
            if(!gTable.IsEmpty && gTable.rs_main.recordcount > 0)
            {
		        var linei=0;
	            gTable.rs_main.moveFirst();
	            while(!gTable.rs_main.EOF)
	            {
    		    	//var checkId = linei+1;
    		    	
    		    	var fiedValue = gTable.rs_main.fields(6).value;
    		    	
    		    	//var chkObj = document.getElementById("chk"+checkId);
    		    	
    		    	//alert(checkId+"checkId");
    		    	if (fiedValue != null && (fiedValue == true || fiedValue == "True"))
    		    	{
    		    	    var dbDic = com.inventec.template.manager.DataSourceManager.GetNewDicitionary().value;
    		    	    setKeyValueOfDic(dbDic, "<%=AttributeNames.DATA_BASE_ID %>",gTable.rs_main.Fields(0).value);
    		    	    setKeyValueOfDic(dbDic, "<%=AttributeNames.DATA_BASE_NAME %>",gTable.rs_main.Fields(2).value);
    		    	    
    		    	    arrList.push(dbDic);
    		    	    
    		    	    isCheck = true;
    		    	}
    		    	
	                linei++;
	                gTable.rs_main.moveNext();
	            }
		    } 

            if (!isCheck)
            {
                //ITC-934-0046
                HideWait();
                alert("No database selected!");
                return ;
            }
            
		    var saveReturn = com.inventec.template.manager.DataSourceManager.SaveDataSourceSetting(serverDic,arrList);
		    if (saveReturn.error != null)
		    {
		        HideWait();
		        alert(saveReturn.error.Message);
		        return;
		    }
		    else
		    {
		        var treeServerId = saveReturn.value;
		        HideWait();
		        winRtnValue = treeServerId;
		        returnDialogValue();
		    }   
        }
        
        <%--
            ' ======== ============ =============================
            ' Description: ���ȡ����ť
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>          
        function fCancel()
        {
            returnDialogValue();
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
            if (undefined == val || null == val)
            {
                val = "";
            }
            
            return val;
        }

        <%--
            ' ======== ============ =============================
            ' Description: ת�����ַ���
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>          
        function changeNullNotTrim(val)
        {
            if (undefined == val || null == val)
            {
                val = "";
            }
            
            return val;
        }                
        
        <%--
            ' ======== ============ =============================
            ' Description: ����ֵ
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>  
        function returnDialogValue()
        {
    	    window.returnValue = winRtnValue;
    	    window.close();
        }
        
        <%--
            ' ======== ============ =============================
            ' Description: �ж��ַ����Ƿ�Ϊ�ɼ��ַ�
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>          
        function charCodeAtTest(str)
        {
          //var str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //��ʼ��������
            
            for (var i = 0; i < str.length; ++i)
            {
                var n = str.charCodeAt(i);//��ȡλ�� n ���ַ��� Unicode ֵ��
                if (n < 33 || n > 126)
                {
                    return false;
                } 
            }
          
          return true;                           //���ظ�ֵ��
        }       
 </script>


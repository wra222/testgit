<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_datasource_showTableTree, App_Web_showtabletree.aspx.af99fe74" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript" src="../../commoncontrol/treeView/treeViewControl.js"></script>
</head>
<fis:header id="header2" runat="server"/>
<body leftMargin="0" topMargin="0" rightMargin="0" bottommargin="0" scroll=no style="100%">
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
    <table width="100%" height="92%">
    <tr>
        <td class="title" id="title"></td>
    </tr>
    <tr>
        <td width="100%" height="100%">
        <div  id="_treeviewarea"  style="overflow-y:scroll;overflow-x:scroll;background-color:#FFFFFF; width:100%; height:100%" >
         <!-- #include file="../reportedit/fileTree/filetreepublic.aspx" -->

        </div>  
        </td>
    </tr>
    </table>
    
    <table width="100%">
    <tr>
        <td align="right"> <br /><button id="btnOpen"  style="width:90px" onclick="fOpen()" disabled>Open Table</button></td>
    </tr>
    </table>
</body>
   <fis:footer id="footer1" runat="server"/> 
</html>
<script>
    var databaseName = "";
    var databaseId = changeNull("<%= Request.Params[3]%>");
    var serverId = changeNull("<%= Request.Params[4]%>");
    
    var gFeature = "center:yes; dialogHeight:600px; dialogWidth:700px; help:no; status:no; resizable:no; scroll:no;";

    
    document.body.onload = function ()
    {
        //document.getElementById("title").innerText= databaseName;
        //getDataBaseName();
        
//        var tableWidth = document.body.clientWidth;
//        var clientH = document.body.clientHeight-90;
//        document.getElementById("treeviewarea").style.width=tableWidth;
//        document.getElementById("treeviewarea").style.height=clientH;
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ��ȡdatabase����Ϣ
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>   
    getDataBaseName();
    
    function getDataBaseName()
    {
        var info = webroot_aspx_datasource_showTableTree.GetDataBaseInfoById(databaseId);
        if (null != info.error)
        {
            alert(info.error.Message);
            return;
        }
        
        databaseName = info.value;
        document.getElementById("title").innerText = info.value;
    }
    
    <%--
        ' ======== ============ =============================
        ' Description: ����������
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>      
    createTree(serverId, databaseName);
    
    
    var treeClickCallBack = function treeClickCallBack()
    {
        var tyNode = type;
        //alert(type);
        if (tyNode == "table")
        {
            document.getElementById("btnOpen").disabled=false;
        }
        else
        {
            document.getElementById("btnOpen").disabled=true;
        }
        
    }

    <%--
        ' ======== ============ =============================
        ' Description: �򿪱�����
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%>     
    function fOpen()
    {
        var tyNode = type;
        if (tyNode != "table")
        {
            alert("Sorry, you cannot open it!");
            return;
        }
        
        var diagArgs = new Object();
        diagArgs.serverId = serverId;
        diagArgs.databaseId = databaseId;
        diagArgs.databaseName = databaseName;
        diagArgs.tableName = nodeName;        
//        diagArgs.editorId =  "";
//        var rtn =  window.showModalDialog("../reportedit/reportaddfolder.aspx", diagArgs, sFeatures);
//        
//        var url = "showTableContent.aspx?serverId="+serverId+"&databaseName=" + databaseName + "&tableName=" + nodeName;
	    
        window.showModalDialog("showTableContent.aspx", diagArgs, gFeature);
        //window.open(url);
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

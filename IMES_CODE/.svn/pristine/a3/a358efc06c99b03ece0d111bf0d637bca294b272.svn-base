<%@ page language="C#" autoeventwireup="true" inherits="main, App_Web_main.aspx.39cd9290" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>

    
</head>
<fis:header id="header2" runat="server"/>
<body style="background-color:#e2f1f8; border-color:#e2f1f8; border:0px;margin:0,0,0,0; padding:0,0,0,0; " scroll=no oncontextmenu="return false;">
    <form runat=server>
    </form>
    <table cellpadding="0px"  cellspacing="0px" style="background-color: #e2f1f8;  z-index:2;position:absolute;width:100%" border="0px">
        <tr>
            <td id="title" colspan=2 class="title" width="100%">
               Root
            </td>
        </tr>
        <tr>
            <td align=left colspan=2 >
                <img src="../../images/print_template_editor.jpg" height="148px" width="758" />
            </td>
        </tr>
         <tr>
            <td height="10px" colspan=2></td>
        </tr>
        <tr>
            <td align=center width="50%">
                <img src="../../images/Data Source Setting.gif" id="data" onmouseover="changeImg('data')" onmouseout="restoreImg('data')" onclick="fClick('data')"/>
            </td>
            <td>
            </td>
            
        </tr>
        <tr>
            <td align=center  >
                <img src="../../images/Print Template.gif"  id="report" onmouseover="changeImg('report')" onmouseout="restoreImg('report')" onclick="fClick('report')"/>
            </td>
            <td>
            </td>
        </tr>
    </table>

</body>
<fis:footer id="footer1" runat="server"/>
</html>
    <script type="text/javascript">
    

       // window.attachEvent("onload", correctPNG); 

        function changeImg(id)
        {
            var url = "../../images/";
            var img = "";
            
            if(window.event.srcElement.tagName == "TD")
                return false;
            
            switch(id)
            {
                case 'data':
                    img = "Data Source Setting-h.gif";
                    break;
                case 'report':
                    img = "Print Template-h.gif";
                    break;    
            }
            
            eval(id).src = url + img;
        }
        
        function restoreImg(id)
        {
            var url = "../../images/";
            var img = "";
            
            if(window.event.srcElement.tagName == "TD")
                return false;
                            
            switch(id)
            {
                case 'data':
                    img = "Data Source Setting.gif";
                    break;
                case 'report':
                    img = "Print Template.gif";
                    break;    
            }
            
           document.getElementById(id).src = url + img;
        }
        
        function fClick(id){
            switch(id){
                case 'data':
                    //bug no:ITC-992-0013
                    //reason:û�����ýڵ�
                    window.parent.frames("menu").tree.searchInChildNodes("uuid", "f0000000000000000000000000000001");
                    window.parent.frames("main").location.href = "../datasource/dataSourceSetting.aspx?type=0&uuid=f0000000000000000000000000000001&text=Data Source Setting";
                    break;
                case 'report':
                    window.parent.frames("menu").tree.searchInChildNodes("uuid", "f0000000000000000000000000000002");
                    window.parent.frames("main").location.href = "../template/TemplateMain.aspx?type=1&uuid=f0000000000000000000000000000002&text=Template";
                    break;
            }
        
        }
        
    </script>
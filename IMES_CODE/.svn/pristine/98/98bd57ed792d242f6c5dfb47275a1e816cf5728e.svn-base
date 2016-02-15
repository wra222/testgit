<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: ģ���趨��ҳ��2
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)  create
' Known issues:Any restrictions about this file
--%>

<%@ page language="C#" autoeventwireup="true" inherits="webroot_axpx_template_TemplateSetting2, App_Web_templatesetting2.aspx.7a399c77" theme="MainTheme" %>

<%@ Import Namespace="com.inventec.system" %>

<script type="text/javascript" src="../../commoncontrol/tableEdit/TableData.js"></script>

<script type="text/javascript" src="../../commoncontrol/tableEdit/TableEdit.js"></script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Template Setting-Step 2</title>
</head>
<%--load all js files--%>
<fis:header ID="header1" runat="server" />
<body onload="initPage()" class="bgBody">
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
    <div class="wizardTip">
        <span style="font-weight: bold">
            <%=Resources.Template.templateSetting%>
        </span>
        <br>
        <span style="margin-left: 50px">
            <%=Resources.Template.templateSettingTitle2%>
        </span>
        <br>
        <br>
    </div>
    <div class="wizardContent">
        <table style="width: 30%" border="0" cellpadding="5" cellspacing="5">
            <tr>
                <td align="right">
                    <div id="showreportdata">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <button id="deleteRow" onclick="deleteRow()" style="display: none">
                        <%=Resources.Template.deleteButton%>
                    </button>
                </td>
            </tr>
        </table>
    </div>
</body>
<%--set style for readonly input controller--%>
<fis:footer ID="footer1" runat="server" />

<script for="ShowItemTableText0" language="javascript" event="onfocus">
    //���Ʋ���������󳤶�
	var paramName = document.getElementById("ShowItemTableText0");
	paramName.maxLength = 15;
</script>

<script for="ShowItemTableText0" language="javascript" event="onblur">
    //�ж�����Ĳ��������������е����м�¼����
	if (checkUnique(this.value)){
	    this.select();
	}
</script>

<script for="ShowItemTableText2" language="javascript" event="onfocus">
    //������������󳤶�
	var paramDesc = document.getElementById("ShowItemTableText2");
	paramDesc.maxLength = 200;
</script>

<script language="JavaScript">
<!--
var ShowItemTable = null;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	showTable
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��ʾ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function showTable(rs){

  
    var tableWidth = 472;
    var tableWidth1 = tableWidth - 17;
    var clientH = 200;
        
    if (ShowItemTable == null){
	    ShowItemTable = new clsTable(rs, "ShowItemTable");
     
        ShowItemTable.TableWidth = tableWidth;  
        ShowItemTable.Height = clientH;  	    
        ShowItemTable.Widths = new Array(tableWidth1*0.3, tableWidth1*0.3, tableWidth1*0.4);
        ShowItemTable.FieldsType = new Array( 0, 0, 0);
        ShowItemTable.HideColumn = new Array(true, true, true);
        ShowItemTable.modi = new Array(true, false, true);
        ShowItemTable.Divide = "<%=Constants.COL_DELIM%>";
        ShowItemTable.ScreenWidth = -1;
        ShowItemTable.AddDelete=true;
        ShowItemTable.outerSelect = true;
        ShowItemTable.NotEmpty = 0;
        //ShowItemTable.UpDown = "fUpdateIssueDetail()";
        var type = "<select id='type'  style='width:95%;'>";
                type += '<option value="<%=Constants.PARAM_INT_TYPE%>"><%=Constants.PARAM_INT_TYPE%></option>';
                type += '<option value="<%=Constants.PARAM_STRING_TYPE%>"><%=Constants.PARAM_STRING_TYPE%></option>';
              
                type += "</select>";
        ShowItemTable.UseControl(1, type); 
       //ShowItemTable.UseHTML = true;  
        ShowItemTable.AfterNew = "MyAfterNew()";
    }
      	
    ShowItemTable.rs_main = rs;
    showreportdata.innerHTML = ShowItemTable.Display();
	    
    HideWait();
    document.getElementById("deleteRow").style.display = "";
}

 function MyAfterNew()
 {
    //debugger;
    var rownum=ShowItemTable.GetRowNumber();
    if(rownum == -1 || rownum > ShowItemTable.rs_main.recordcount)
    {
        return;
    }
   
    var rowItemArr = ShowItemTable.RowStr.split('<%=Constants.COL_DELIM%>');
    var name = rowItemArr[0];
    var refFlag = false;
    //�ж���������Ƿ�Dataset����  
    if (name != "") { 
       for (var i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.DatasettingList.length; i++)
       {
             for (var j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DatasettingList[i].Parameters.length; j++) {
                 if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DatasettingList[i].Parameters[j].Parameter ==  name) {
                    refFlag = true;
                    break;
                 }
            } 
            if (refFlag) {
                break;
            }        
        } 
        if (refFlag) {
            document.getElementById("ShowItemTableText0").readOnly = true;
            document.getElementById("type").disabled = true;
        }
    }
        
       
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkUnique
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	�ж�����Ĳ������Ƿ������е����м�¼����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkUnique(curParaName)
{

 
   var errorFlag = false;
    ShowItemTable.ClearV();
     var rowNum = ShowItemTable.GetRowNumber();
   
    var rs = ShowItemTable.rs_main;
    var i = 0 ;
    if (curParaName != "") {
        if (rs.recordcount > 0){
            rs.moveFirst();
            while (!rs.EOF){
                 if ((curParaName.toLowerCase() == rs.Fields(0).value.toLowerCase()) && (rowNum != i)){
                   
                        errorFlag = true;
                        alert("<%=Resources.Template.dupTempalteParaName%>");
                        break;
                  
                    
                 }
                i = i + 1;
                rs.moveNext();
            } 
        }
    }

    return errorFlag;
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	initPage
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��ȡ�����Ϣ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function initPage()
{
 
    var record = window.parent.templateXmlAndHtml.PrintTemplateInfo;
    
    record.toJSON = function(){return toJSON(this);};
  
       var rtn = com.inventec.template.manager.TemplateManager.showTemplateParamTable(record);
     
    if (rtn.error != null) {
    
        alert(rtn.error.Message);
    } else {
        // ���ɼ�¼��
       
        var rs = createRecordSet(rtn.value);
        //��ʾ���
         showTable(rs);
        
    }
    window.parent.setButtonStatus(1);
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	exitPage
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�˳���ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function exitPage()
{
  
    
    var errorFlag = false;
    //ShowItemTable.ClearV();
//    <bug>
//        BUG NO:ITC-992-0001 
//        REASON:����Refresh����ˢ�±����ȡ�õ�ǰ�༭����ʵ״̬
//    </bug>
    ShowItemTable.Refresh();
    var rs = ShowItemTable.rs_main;
    var inputParam;
    window.parent.templateXmlAndHtml.PrintTemplateInfo.InputParas.length = 0;
    if (rs.RecordCount > 0){
        rs.moveFirst();
        while (!rs.EOF){
         if ((trimString(rs.Fields(0).value)== "") || (trimString(rs.Fields(0).value)== "-1.0249E5")) {
                rs.moveNext();
                continue;
            }
            inputParam = com.inventec.template.manager.TemplateManager.getInputParaDefFromStructure();
            inputParam.value.ParaName = trimString(rs.Fields(0).value);
            inputParam.value.ParaType = trimString(rs.Fields(1).value);
            inputParam.value.ParaDesc = trimString(rs.Fields(2).value);
            window.parent.templateXmlAndHtml.PrintTemplateInfo.InputParas.push(inputParam.value);
            rs.moveNext();
        } 
       
           
        if (window.parent.templateXmlAndHtml.PrintTemplateInfo.InputParas.length == 0)
        {
            alert("<%=Resources.Template.notInputTemplatePara%>");
            errorFlag = true;
        }
       
   } else {
    
       
        alert("<%=Resources.Template.notInputTemplatePara%>");
        errorFlag = true;
      
   }
  
   return errorFlag;
   
   
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	deleteRow
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	ɾ��ѡ�е�һ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function deleteRow()
{
  // debugger;
  
    var rownum=ShowItemTable.GetRowNumber();
    if(rownum == -1 || rownum > ShowItemTable.rs_main.recordcount) {
        alert("<%=Resources.Template.notSelectRow%>");
        return;
    } 
    
    //alert(document.getElementById("ShowItemTableText0").readOnly);
    if (!document.getElementById("ShowItemTableText0").readOnly) {
        ShowItemTable.Delete(); 
    } else {
        alert("<%=Resources.Template.templateParamConflic%>");
    }
//    var rowItemArr = ShowItemTable.RowStr.split('<%=Constants.COL_DELIM%>');
//    var name = rowItemArr[0];
//    var refFlag = false;
// 
//   //�ж���������Ƿ�Dataset����  
//   if (name != "") { 
//       for (var i = 0; i < window.parent.templateXmlAndHtml.PrintTemplateInfo.DatasettingList.length; i++)
//       {
//             for (var j = 0; j < window.parent.templateXmlAndHtml.PrintTemplateInfo.DatasettingList[i].Parameters.length; i++) {
//                 if (window.parent.templateXmlAndHtml.PrintTemplateInfo.DatasettingList[i].Parameters[j].Parameter ==  name) {
//                    refFlag = true;
//                    alert("<%=Resources.Template.templateParamConflic%>");
//                    break;
//                 }
//            } 
//            if (refFlag) {
//                break;
//            }        
//        } 
//        if (!refFlag) {
//            ShowItemTable.Delete(); 
//        }
//  }
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	exitPrePage
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�˳���ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function exitPrePage()
{
  
    
    //    <bug>
    //        BUG NO:ITC-992-0002 
    //        REASON:���previous��ťʱ����У��
    //    </bug>
    ShowItemTable.Refresh();
    var rs = ShowItemTable.rs_main;
    var inputParam;
    window.parent.templateXmlAndHtml.PrintTemplateInfo.InputParas.length = 0;
    if (rs.RecordCount > 0){
        rs.moveFirst();
        while (!rs.EOF){
         if ((trimString(rs.Fields(0).value)== "") || (trimString(rs.Fields(0).value)== "-1.0249E5")) {
                rs.moveNext();
                continue;
            }
            inputParam = com.inventec.template.manager.TemplateManager.getInputParaDefFromStructure();
            inputParam.value.ParaName = trimString(rs.Fields(0).value);
            inputParam.value.ParaType = trimString(rs.Fields(1).value);
            inputParam.value.ParaDesc = trimString(rs.Fields(2).value);
            window.parent.templateXmlAndHtml.PrintTemplateInfo.InputParas.push(inputParam.value);
            rs.moveNext();
        } 
       
   } 
   
   
}
//-->
</script>

</html>

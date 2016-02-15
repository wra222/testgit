<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_TempEditMain, App_Web_tempeditmain.aspx.7a399c77" theme="MainTheme" %>

<%@ Import Namespace="com.inventec.system" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<%--load all js files--%>
<fis:header id="header1" runat="server"/>
<body onload="">
    <form id="form1" runat="server">
    <%=Request.ServerVariables["LOGON_USER"]%>
    <div>
    <img src="" id="image" >
    <input type="text" id="x" width="50px">
    <input type="text" id="y" width="50px">
    <input type="text" id="angle" width="50px">
    <input type="text" id="uuid" width="50px">
    <input type="text" id="height" width="50px">
    <input type="text" id="width" width="50px">
    </div>
    </form>
    <input type="button" value="barcode" onclick = "barcode()">
    <input type="button" value="newbarcode" onclick = "newbarcode()">
      <input type="button" value="newbarcodenodataset" onclick = "newbarcodenodataset()">
      <input type="button" value="blankdataset" onclick = "blankdataset()">
      
        <input type="button" value="newpicture" onclick = "newpicture()">
         <input type="button" value="newtext" onclick = "newtext()">
        <input type="button" value="newline" onclick = "newline()">
        <input type="button" value="newrec" onclick = "newrec()">
        <input type="button" value="newarea" onclick = "newarea()">
        <input type="button" value="celltext" onclick = "celltext()">
        <input type="button" value="set" onclick = "set()">
          <input type="button" value="setcell" onclick = "setcell()">
            <input type="button" value="save" onclick = "save()">
            <br>
            <br>
               <input type="button" value="areatext" onclick = "areatext()">
               <input type="button" value="cellareatext" onclick = "cellareatext()">
               <input type="button" value="print" onclick = "print()">
               <input type="button" value="dataset" onclick = "dataset()">
                <input type="button" value="editdataset" onclick = "editdataset()">
                 <input type="button" value="newtemplate" onclick = "newtemplate()">
</body>
</html>
<SCRIPT LANGUAGE="JavaScript">
<!--
 var userName = '<%=Request.ServerVariables["LOGON_USER"]%>'; 
 function print()
 {
     var rtn = com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
     if (rtn.error != null) {
        alert(rtn.error.Message);
    } else {

        //���ģ��ṹ
        var printTemplateInfo = rtn.value;
        printTemplateInfo.PageHeader.AreaHeight = "20"
        printTemplateInfo.TemplateWidth = "30"
        printTemplateInfo.FileName = "template";
 
        var input = com.inventec.template.manager.TemplateManager.getInputParaDefFromStructure();
        var param = input.value;
        param.ParaName = "aa"
        param.ParaType = "string"
        printTemplateInfo.InputParas.push(param)
        input = com.inventec.template.manager.TemplateManager.getInputParaDefFromStructure();
        param = input.value;
        param.ParaName = "bb"
        param.ParaType = "int"
        printTemplateInfo.InputParas.push(param)
         input = com.inventec.template.manager.TemplateManager.getInputParaDefFromStructure();
        param = input.value;
        param.ParaName = "cc"
        param.ParaType = "int"
        printTemplateInfo.InputParas.push(param)
        var ret = window.showModalDialog("Print.aspx", printTemplateInfo, "dialogWidth:400px;dialogHeight:390px;center:yes;scroll:off;status:no")
      
    }
 }

function dataset()
 {
 
     var rtn = com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
     if (rtn.error != null) {
        alert(rtn.error.Message);
    } else {

        //���ģ��ṹ
        var printTemplateInfo = rtn.value;
        printTemplateInfo.PageHeader.AreaHeight = "20"
        printTemplateInfo.TemplateWidth = "30"
        printTemplateInfo.FileName = "template";
 
        var input = com.inventec.template.manager.TemplateManager.getInputParaDefFromStructure();
        var param = input.value;
        param.ParaName = "aa"
        param.ParaType = "string"
        printTemplateInfo.InputParas.push(param)
        input = com.inventec.template.manager.TemplateManager.getInputParaDefFromStructure();
        param = input.value;
        param.ParaName = "bb"
        param.ParaType = "int"
        printTemplateInfo.InputParas.push(param)
         input = com.inventec.template.manager.TemplateManager.getInputParaDefFromStructure();
        param = input.value;
        param.ParaName = "cc"
        param.ParaType = "int"
        printTemplateInfo.InputParas.push(param)
         //����Ҫ�༭�Ķ�������
                    var diagArgs = new Object();
                    diagArgs.method = "add";
                    diagArgs.printTemplate =  printTemplateInfo;
                     diagArgs.dataset = "";
        debugger;
        var ret = window.showModalDialog("../dataset/datasetedit.aspx", diagArgs, "dialogWidth:650px;dialogHeight:550px;center:yes;scroll:off;status:no")
        var i=0;
    }
 }

function editdataset()
 {
     var rtn = com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
     if (rtn.error != null) {
        alert(rtn.error.Message);
    } else {

        //���ģ��ṹ
        var rtn1 = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject1 = rtn1.value;
    dataObject1.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
    dataObject1.Source = "<%=Constants.DATA_SOURCE_DATA_SET%>";
    dataObject1.DataSet = "DataSet1";
    dataObject1.OutputField = "f";
    dataObject1.DisplayTxt = "liuxin";
    dataObject1.DataSetDep = "DataSet3";
    dataObject1.OutputFiledDep = "c";
    dataObject1.DependencedValue = "2";
    
        var printTemplateInfo = rtn.value;
        printTemplateInfo.PageHeader.AreaHeight = "20"
        printTemplateInfo.TemplateWidth = "30"
        printTemplateInfo.FileName = "template";
 
        var input = com.inventec.template.manager.TemplateManager.getInputParaDefFromStructure();
        var param = input.value;
        param.ParaName = "aa"
        param.ParaType = "string"
        printTemplateInfo.InputParas.push(param)
        input = com.inventec.template.manager.TemplateManager.getInputParaDefFromStructure();
        param = input.value;
        param.ParaName = "bb"
        param.ParaType = "int"
        printTemplateInfo.InputParas.push(param)
         input = com.inventec.template.manager.TemplateManager.getInputParaDefFromStructure();
        param = input.value;
        param.ParaName = "cc"
        param.ParaType = "int"
        printTemplateInfo.InputParas.push(param)
        
        
         //����Ҳ��edit������dataset
         var  fieldDefRtn = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
         var  fieldDefRtn1 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
         var  fieldDefRtn2 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
         var  fieldDefRtn3 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
          
                var fieldDef = fieldDefRtn.value;
                fieldDef.FieldName1 = "a";
                fieldDef.Type = "int";
                var fieldDef1 = fieldDefRtn1.value;
                fieldDef1.FieldName1 = "b";
                fieldDef1.Type = "string";
                var fieldDef2 = fieldDefRtn2.value;
                fieldDef2.FieldName1 = "c";
                fieldDef2.Type = "string";
                var fieldDef3 = fieldDefRtn3.value;
                fieldDef3.FieldName1 = "d";
                fieldDef3.Type = "string";
               dataSetRtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               dataSetRtn1 = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
                    var dataSet = dataSetRtn.value;
                    dataSet.DataSetName = "DataSet1";
                    dataSet.Fields.push(fieldDef);
                    dataSet.Fields.push(fieldDef1);
                    
                    dataSet1 = dataSetRtn1.value;
                     dataSet1.DataSetName = "DataSet2";
                  
                    dataSet.Fields.push(fieldDef2);
                    dataSet.Fields.push(fieldDef3);
                    dataSet.QueryType == "<%=Constants.DATASET_QUERY_TYPE_SQL %>";
                    dataSet.SqlTextString = "select id, name as serverName, alias as name, userName, password from T_DataServer where lifeCycleStatus=1"
                    printTemplateInfo.DatasettingList.push(dataSet);
                    printTemplateInfo.DatasettingList.push(dataSet1);
                     printTemplateInfo.DataObjects.push(dataObject1);
             
         //����Ҫ�༭�Ķ�������
                    var diagArgs = new Object();
                    diagArgs.method = "edit";
                    diagArgs.printTemplate =  printTemplateInfo;
                     diagArgs.dataset = "DataSet1";
        var ret = window.showModalDialog("../dataset/datasetedit.aspx", diagArgs, "dialogWidth:650px;dialogHeight:550px;center:yes;scroll:off;status:no")
      
    }
 }

function set()
{
   
   var printTemplate;
    var picture;
    var pictureRtn;
     var picture1;
    var pictureRtn1;
    var templateXmlandHtml;
    
    
    
	 
   
    
 
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getTemplateXmlAndHtmlFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        templateXmlandHtml = templateRtn.value;
         var rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject = rtn.value;
    dataObject.ObjectName = "22";
       var rtn1 = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject1 = rtn1.value;
    dataObject1.ObjectName = "11";
       templateXmlandHtml.PrintTemplateInfo.DataObjects.push(dataObject1);
        templateXmlandHtml.PrintTemplateInfo.DataObjects.push(dataObject);
       
        //����һ��barcode����
        pictureRtn = com.inventec.template.manager.TemplateManager.getLineFromStructure();
        pictureRtn1 = com.inventec.template.manager.TemplateManager.getLineFromStructure();
        if (pictureRtn.error != null) {
            alert(pictureRtn.error.Message);
        } else {
            picture = pictureRtn.value;
            picture1 = pictureRtn1.value;
           //ע�������봴����dataobjectһ��
           picture.ObjectName = "11";
           picture.X = "40"
          
           templateXmlandHtml.PrintTemplateInfo.PageHeader.LineObjects.push(picture);
           
           picture1.ObjectName = "22";
           templateXmlandHtml.PrintTemplateInfo.PageHeader.LineObjects.push(picture1);
           
           //edit���������򳤿�
           templateXmlandHtml.PrintTemplateInfo.PageHeader.AreaHeight = "20"
           templateXmlandHtml.PrintTemplateInfo.TemplateWidth = "30"
         
          
                    //����Ҫ�༭�Ķ�������
                 
                   barcodeRtn = com.inventec.template.manager.TemplateManager.getTextFromStructure();
       barcodeRtn1 = com.inventec.template.manager.TemplateManager.getBarcodeFromStructure();
        if (barcodeRtn.error != null) {
            alert(barcodeRtn.error.Message);
        } else {
            barcode = barcodeRtn.value;
            barcode1 = barcodeRtn1.value;
           //ע�������봴����dataobjectһ��
           barcode.ObjectName = "11";
           barcode.X = "30";
           barcode1.ObjectName = "22";
           barcode1.X = "4"
           var cell = com.inventec.template.manager.TemplateManager.getSectionCellFromStructure();
           var secCell = cell.value;
           secCell.TextObjects.push(barcode);
           secCell.BarcodeObjects.push(barcode1);
            var sec = com.inventec.template.manager.TemplateManager.getSectionFromStructure();
           var section = sec.value;
           section.HeaderHeight = "20"
            section.ColumnNum = "3";
            section.RowHeight = "30"
            section.Cells.push(secCell);
        section.AreaHeight = "40"
            
            debugger;
           templateXmlandHtml.PrintTemplateInfo.DetailSections.push(section);  
        }
                    var diagArgs = new Object();
    diagArgs.treeId = "aa"; //get the parent id
    diagArgs.templateXmlandHtml = templateXmlandHtml;
                    var ret = window.showModalDialog("TemplateSetting.aspx", diagArgs, "dialogWidth:580px;dialogHeight:450px;center:yes;scroll:off")
                   var i = 0;
              

          
        }        
    }

   
  
}

function newpicture()
{
   
   var printTemplate;
    var picture;
    var pictureRtn;
     var picture1;
    var pictureRtn1;
    
    
    
	
      var rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject = rtn.value;
    dataObject.ObjectName = "11";
//    dataObject.Source = "<%=Constants.DATA_SOURCE_DATA_SET%>";
//    dataObject.DataSet = "DataSet3";
//    dataObject.OutputField = "f";
//    dataObject.DisplayTxt = "liuxin";
    dataObject.DataSetDep = "DataSet2";
    dataObject.OutputFiledDep = "c";
    dataObject.DependencedValue = "2";
    
   
    
    
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        printTemplate = templateRtn.value;
        printTemplate.DataObjects.push(dataObject);
       ;
       
        //����һ��barcode����
        pictureRtn = com.inventec.template.manager.TemplateManager.getPictureFromStructure();
        pictureRtn1 = com.inventec.template.manager.TemplateManager.getPictureFromStructure();
        if (pictureRtn.error != null) {
            alert(pictureRtn.error.Message);
        } else {
            picture = pictureRtn.value;
            picture1 = pictureRtn1.value;
           //ע�������봴����dataobjectһ��
           picture.ObjectName = "11";
          
           printTemplate.PageHeader.PictureObjects.push(picture);
           
           picture1.ObjectName = "22";
           printTemplate.PageHeader.PictureObjects.push(picture1);
           
           //edit���������򳤿�
           printTemplate.PageHeader.AreaHeight = "20"
           printTemplate.TemplateWidth = "30"
         
          
            fieldDefRtn = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn1 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn2 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn3 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           if (fieldDefRtn.error != null) {
                alert(fieldDefRtn.error.Message);
            } else {
                fieldDef = fieldDefRtn.value;
                fieldDef.FieldName1 = "a";
                fieldDef.Type = "int";
                fieldDef1 = fieldDefRtn1.value;
                fieldDef1.FieldName1 = "b";
                fieldDef1.Type = "string";
                fieldDef2 = fieldDefRtn2.value;
                fieldDef2.FieldName1 = "c";
                fieldDef2.Type = "string";
                fieldDef3 = fieldDefRtn3.value;
                fieldDef3.FieldName1 = "d";
                fieldDef3.Type = "string";
               dataSetRtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               dataSetRtn1 = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               if (dataSetRtn.error != null) {
                    alert(dataSetRtn.error.Message);
                } else {
                    dataSet = dataSetRtn.value;
                    dataSet.DataSetName = "DataSet1";
                    dataSet.Fields.push(fieldDef);
                    dataSet.Fields.push(fieldDef1);
                    
                    dataSet1 = dataSetRtn1.value;
                    dataSet1.DataSetName = "DataSet2";
                    dataSet1.Fields.push(fieldDef2);
                    dataSet1.Fields.push(fieldDef3);
                    
                    printTemplate.DatasettingList.push(dataSet);
                    printTemplate.DatasettingList.push(dataSet1);
//                    printTemplate.DatasettingList.push(dataSet1);
//                      printTemplate.DatasettingList.push(dataSet1)

                }
         }           
                    //����Ҫ�༭�Ķ�������
                    var diagArgs = new Object();
                    diagArgs.id = "printTemplateInfo_dot_PageHeader_dot_PictureObjects_a0_b";
                    diagArgs.printTemplate =  printTemplate;
                    
                   debugger;
                    window.showModalDialog("PictureProperty.aspx", diagArgs, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
              debugger;
                    document.getElementById("image").src = "CreateImg.aspx?name=" + diagArgs.printTemplate.PageHeader.PictureObjects[0].Uuid;
                        alert(document.getElementById("image").src);
                   
                      document.getElementById("x").value = diagArgs.printTemplate.PageHeader.PictureObjects[0].X;
                    document.getElementById("y").value = diagArgs.printTemplate.PageHeader.PictureObjects[0].Y;
                    document.getElementById("angle").value = document.getElementById("image").src;
                    document.getElementById("uuid").value = diagArgs.printTemplate.PageHeader.PictureObjects[0].Uuid;
                    document.getElementById("height").value = diagArgs.printTemplate.PageHeader.PictureObjects[0].Height;
                    document.getElementById("width").value = diagArgs.printTemplate.PageHeader.PictureObjects[0].Width;
              

          
        }        
    }

   
  
}


function newline()
{
   
   var printTemplate;
    var picture;
    var pictureRtn;
     var picture1;
    var pictureRtn1;
    
    
    
	
    
   
    
 
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        printTemplate = templateRtn.value;
        
       ;
       
        //����һ��barcode����
        pictureRtn = com.inventec.template.manager.TemplateManager.getLineFromStructure();
        pictureRtn1 = com.inventec.template.manager.TemplateManager.getLineFromStructure();
        if (pictureRtn.error != null) {
            alert(pictureRtn.error.Message);
        } else {
            picture = pictureRtn.value;
            picture1 = pictureRtn1.value;
           //ע�������봴����dataobjectһ��
           picture.ObjectName = "printTemplateInfo.PageHeader.LineObjects_a0_b";
          
           printTemplate.PageHeader.LineObjects.push(picture);
           
           picture1.ObjectName = "22";
           printTemplate.PageHeader.LineObjects.push(picture1);
           
           //edit���������򳤿�
           printTemplate.PageHeader.AreaHeight = "20"
           printTemplate.TemplateWidth = "30"
         
          
                    //����Ҫ�༭�Ķ�������
                    var diagArgs = new Object();
                    diagArgs.id = "printTemplateInfo.PageHeader.LineObjects_a0_b";
                    diagArgs.printTemplate =  printTemplate;
                    
                   debugger;
                    window.showModalDialog("LineProperty.aspx", diagArgs, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
                   var i = 0;
              

          
        }        
    }

   
  
}



function newrec()
{
   
   var printTemplate;
    var picture;
    var pictureRtn;
     var picture1;
    var pictureRtn1;
    
    
    
	
    
   
    
 
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        printTemplate = templateRtn.value;
        
       ;
       
        //����һ��barcode����
        pictureRtn = com.inventec.template.manager.TemplateManager.getRectangleFromStructure();
        pictureRtn1 = com.inventec.template.manager.TemplateManager.getRectangleFromStructure();
        if (pictureRtn.error != null) {
            alert(pictureRtn.error.Message);
        } else {
            picture = pictureRtn.value;
            picture1 = pictureRtn1.value;
           //ע�������봴����dataobjectһ��
           picture.ObjectName = "printTemplateInfo.PageHeader.RectangleObjects_a0_b";
          
           printTemplate.PageHeader.RectangleObjects.push(picture);
           
           picture1.ObjectName = "22";
           printTemplate.PageHeader.RectangleObjects.push(picture1);
           
           //edit���������򳤿�
           printTemplate.PageHeader.AreaHeight = "20"
           printTemplate.TemplateWidth = "30"
         
          
                    //����Ҫ�༭�Ķ�������
                    var diagArgs = new Object();
                    diagArgs.id = "printTemplateInfo.PageHeader.RectangleObjects_a0_b";
                    diagArgs.printTemplate =  printTemplate;
                    
                   debugger;
                    window.showModalDialog("RectangleProperty.aspx", diagArgs, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
                   var i = 0;
              

          
        }        
    }

   
  
}


function newarea()
{
   
   var printTemplate;
    var picture;
    var pictureRtn;
     var picture1;
    var pictureRtn1;
    
    
    
	
    
   
    
 
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        printTemplate = templateRtn.value;
        
       ;
       
        //����һ��barcode����
        pictureRtn = com.inventec.template.manager.TemplateManager.getAreaFromStructure();
        pictureRtn1 = com.inventec.template.manager.TemplateManager.getAreaFromStructure();
        if (pictureRtn.error != null) {
            alert(pictureRtn.error.Message);
        } else {
            picture = pictureRtn.value;
            picture1 = pictureRtn1.value;
           //ע�������봴����dataobjectһ��
           picture.ObjectName = "printTemplateInfo.PageHeader.ArearObjects_a0_b";
          
           printTemplate.PageHeader.ArearObjects.push(picture);
           
           picture1.ObjectName = "printTemplateInfo.PageHeader.ArearObjects_a1_b";
           printTemplate.PageHeader.ArearObjects.push(picture1);
           
           //edit���������򳤿�
           printTemplate.PageHeader.AreaHeight = "20"
           printTemplate.TemplateWidth = "30"
         
          
                    //����Ҫ�༭�Ķ�������
                    var diagArgs = new Object();
                    diagArgs.id = "printTemplateInfo.PageHeader.ArearObjects_a0_b";
                    diagArgs.printTemplate =  printTemplate;
                    
                   debugger;
                    window.showModalDialog("AreaProperty.aspx", diagArgs, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
                   var i = 0;
              

          
        }        
    }

   
  
}


function barcode()
{
    
    var templateRtn =  com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    var printTemplate;
    var barcode;
    var barcodeRtn;
     var barcode1;
    var barcodeRtn1;
    var fieldDefRtn;
    var fieldDef;
    var fieldDefRtn1;
    var fieldDef1;
    var fieldDefRtn2;
    var fieldDef2;
    var fieldDefRtn3;
    var fieldDef3;
    var dataSetRtn;
    var dataSet;
    var dataSetRtn1;
    var dataSet1;
    var dataObject;
    var dataObject1;
    var dataObject2;
    var dataObject3;
    
    
	                
    var rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject = rtn.value;
    dataObject.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a1_b";
    dataObject.Source = "<%=Constants.DATA_SOURCE_SCREEN_DATA%>";
  
    dataObject.DisplayTxt = "liuxin";
    
    dataObject.DataSetDep = "DataSet1";
    dataObject.OutputFiledDep = "a";
    dataObject.DependencedValue = "null";
    
    rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject1 = rtn.value;
    dataObject1.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
    dataObject1.Source = "<%=Constants.DATA_SOURCE_DATA_SET%>";
    dataObject1.DataSet = "DataSet3";
    dataObject1.OutputField = "f";
    dataObject1.DisplayTxt = "liuxin";
    dataObject1.DataSetDep = "DataSet3";
    dataObject1.OutputFiledDep = "c";
    dataObject1.DependencedValue = "2";
    
     rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject2 = rtn.value;
    dataObject2.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a2_b";
    dataObject2.Source = "<%=Constants.DATA_SOURCE_DATE%>";
    dataObject2.DataFormat = "DD.MM.YYYY";
    dataObject2.DateOffset = "11";
    dataObject2.DateOffsetUnitType = "Year"
    dataObject2.DataSetDep = "DataSet1";
    dataObject2.OutputFiledDep = "a";
    dataObject2.DependencedValue = "2";
    
    rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject3 = rtn.value;
    dataObject3.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a3_b";
    dataObject3.Source = "<%=Constants.DATA_SOURCE_PAGE%>";
    dataObject3.PageFormat = "<%=Constants.PAGE_FORMAT_2%>";
    dataObject3.DataSetDep = "DataSet1";
    dataObject3.OutputFiledDep = "a";
    dataObject3.DependencedValue = "2";
    
    
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        printTemplate = templateRtn.value;
        printTemplate.DataObjects.push(dataObject);
        printTemplate.DataObjects.push(dataObject1);
        printTemplate.DataObjects.push(dataObject2);
        printTemplate.DataObjects.push(dataObject3);
        
         //edit���������򳤿�
           printTemplate.PageHeader.AreaHeight = "20"
           printTemplate.TemplateWidth = "30"
           
        barcodeRtn = com.inventec.template.manager.TemplateManager.getBarcodeFromStructure();
        barcodeRtn1 = com.inventec.template.manager.TemplateManager.getBarcodeFromStructure();
        if (barcodeRtn.error != null) {
            alert(barcodeRtn.error.Message);
        } else {
            barcode = barcodeRtn.value;
            barcode.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
            barcode.Symbology = "<%=Constants.SYMBOLOGY_TYPE_39%>";
            barcode.NarrowBarWidth = "1";
            barcode.Height = "1";
            barcode.Ratio = "2.2";
            barcode.HumanReadable = "1";
            barcode.X = "1";
            barcode.Y = "1";
            barcode.Angle = "45";
            
            var barcode1 = barcodeRtn1.value;
            barcode1.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a1_b";
            barcode1.Symbology = "<%=Constants.SYMBOLOGY_TYPE_93%>";
            barcode1.NarrowBarWidth = "0.25";
            barcode1.Height = "2.0";
            barcode1.Ratio = "3.0";
         
            barcode1.X = "1.1";
            barcode1.Y = "1.2";
            barcode1.Angle = "135";
           
           printTemplate.PageHeader.BarcodeObjects.push(barcode);
           printTemplate.PageHeader.BarcodeObjects.push(barcode1);
           fieldDefRtn = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn1 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn2 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn3 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           if (fieldDefRtn.error != null) {
                alert(fieldDefRtn.error.Message);
            } else {
                fieldDef = fieldDefRtn.value;
                fieldDef.FieldName1 = "a";
                fieldDef.Type = "int";
                fieldDef1 = fieldDefRtn1.value;
                fieldDef1.FieldName1 = "b";
                fieldDef1.Type = "string";
                fieldDef2 = fieldDefRtn2.value;
                fieldDef2.FieldName1 = "c";
                fieldDef2.Type = "string";
                fieldDef3 = fieldDefRtn3.value;
                fieldDef3.FieldName1 = "d";
                fieldDef3.Type = "string";
               dataSetRtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               dataSetRtn1 = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               if (dataSetRtn.error != null) {
                    alert(dataSetRtn.error.Message);
                } else {
                    dataSet = dataSetRtn.value;
                    dataSet.DataSetName = "DataSet1";
                    dataSet.Fields.push(fieldDef);
                    dataSet.Fields.push(fieldDef1);
                    
                    dataSet1 = dataSetRtn1.value;
                    dataSet1.DataSetName = "DataSet2";
                    dataSet1.Fields.push(fieldDef2);
                    dataSet1.Fields.push(fieldDef3);
                    
                    
                    
                    printTemplate.DatasettingList.push(dataSet);
                    printTemplate.DatasettingList.push(dataSet1);
                     
                    var diagArgs = new Object();
                    diagArgs.id = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
                    diagArgs.printTemplate =  printTemplate;
                    
                         
                    window.showModalDialog("BarcodeProperty.aspx", diagArgs, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
                    
                }
            }

          
        }        
    }

}


//��һ�δ���barcode��Ϊ����������

function newbarcode()
{
   
    var printTemplate;
    var barcode;
    var barcodeRtn;
     var barcode1;
    var barcodeRtn1;
    var fieldDefRtn;
    var fieldDef;
    var fieldDefRtn1;
    var fieldDef1;
    var fieldDefRtn2;
    var fieldDef2;
    var fieldDefRtn3;
    var fieldDef3;
    var dataSetRtn;
    var dataSet;
    var dataSetRtn1;
    var dataSet1;
    var dataObject;
    var dataObject1;
    var dataObject2;
    var dataObject3;
    
    
	//����dataObject���󣬳�������barcode��������һ���⣬���඼Ϊ��        
    var rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject = rtn.value;
    dataObject.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
   
    
   
    
    
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        printTemplate = templateRtn.value;
        
        printTemplate.DataObjects.push(dataObject);
       
        //����һ��barcode����
        barcodeRtn = com.inventec.template.manager.TemplateManager.getBarcodeFromStructure();
       
        if (barcodeRtn.error != null) {
            alert(barcodeRtn.error.Message);
        } else {
            barcode = barcodeRtn.value;
           //ע�������봴����dataobjectһ��
           barcode.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
           printTemplate.PageHeader.BarcodeObjects.push(barcode);
           //edit���������򳤿�
           printTemplate.PageHeader.AreaHeight = "20"
           printTemplate.TemplateWidth = "30"
         
           //����Ҳ��edit������dataset
           fieldDefRtn = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn1 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn2 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn3 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           if (fieldDefRtn.error != null) {
                alert(fieldDefRtn.error.Message);
            } else {
                fieldDef = fieldDefRtn.value;
                fieldDef.FieldName1 = "a";
                fieldDef.Type = "int";
                fieldDef1 = fieldDefRtn1.value;
                fieldDef1.FieldName1 = "b";
                fieldDef1.Type = "string";
                fieldDef2 = fieldDefRtn2.value;
                fieldDef2.FieldName1 = "c";
                fieldDef2.Type = "string";
                fieldDef3 = fieldDefRtn3.value;
                fieldDef3.FieldName1 = "d";
                fieldDef3.Type = "string";
               dataSetRtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               dataSetRtn1 = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               if (dataSetRtn.error != null) {
                    alert(dataSetRtn.error.Message);
                } else {
                    dataSet = dataSetRtn.value;
                    dataSet.DataSetName = "DataSet1";
                    dataSet.Fields.push(fieldDef);
                    dataSet.Fields.push(fieldDef1);
                    
                    dataSet1 = dataSetRtn1.value;
                    dataSet1.DataSetName = "DataSet2";
                    dataSet1.Fields.push(fieldDef2);
                    dataSet1.Fields.push(fieldDef3);
                    
                    printTemplate.DatasettingList.push(dataSet);
                    printTemplate.DatasettingList.push(dataSet1);
                    printTemplate.DatasettingList.push(dataSet1);
                      printTemplate.DatasettingList.push(dataSet1);
                    
                    //����Ҫ�༭�Ķ�������
                    var diagArgs = new Object();
                    diagArgs.id = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
                    diagArgs.printTemplate =  printTemplate;
                    
                   debugger;
                    window.showModalDialog("BarcodeProperty.aspx", diagArgs, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
                    var i = 0;
                }
            }

          
        }        
    }

}


//��һ�δ���barcode��Ϊ����������,dataset��

function newbarcodenodataset()
{
   
    var printTemplate;
    var barcode;
    var barcodeRtn;
     var barcode1;
    var barcodeRtn1;
    var fieldDefRtn;
    var fieldDef;
    var fieldDefRtn1;
    var fieldDef1;
    var fieldDefRtn2;
    var fieldDef2;
    var fieldDefRtn3;
    var fieldDef3;
    var dataSetRtn;
    var dataSet;
    var dataSetRtn1;
    var dataSet1;
    var dataObject;
    var dataObject1;
    var dataObject2;
    var dataObject3;
    
    
	//����dataObject���󣬳�������barcode��������һ���⣬���඼Ϊ��        
    var rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject = rtn.value;
    dataObject.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
   
    
   
    
    
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        printTemplate = templateRtn.value;
        
        printTemplate.DataObjects.push(dataObject);
       
        //����һ��barcode����
        barcodeRtn = com.inventec.template.manager.TemplateManager.getBarcodeFromStructure();
       
        if (barcodeRtn.error != null) {
            alert(barcodeRtn.error.Message);
        } else {
            barcode = barcodeRtn.value;
           //ע�������봴����dataobjectһ��
           barcode.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
           printTemplate.PageHeader.BarcodeObjects.push(barcode);
           //edit���������򳤿�
           printTemplate.PageHeader.AreaHeight = "20"
           printTemplate.TemplateWidth = "30"
         
           //����Ҳ��edit������dataset
           fieldDefRtn = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn1 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn2 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn3 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           if (fieldDefRtn.error != null) {
                alert(fieldDefRtn.error.Message);
            } else {
                fieldDef = fieldDefRtn.value;
                fieldDef.FieldName1 = "a";
                fieldDef.Type = "int";
                fieldDef1 = fieldDefRtn1.value;
                fieldDef1.FieldName1 = "b";
                fieldDef1.Type = "string";
                fieldDef2 = fieldDefRtn2.value;
                fieldDef2.FieldName1 = "c";
                fieldDef2.Type = "string";
                fieldDef3 = fieldDefRtn3.value;
                fieldDef3.FieldName1 = "d";
                fieldDef3.Type = "string";
               dataSetRtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               dataSetRtn1 = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               if (dataSetRtn.error != null) {
                    alert(dataSetRtn.error.Message);
                } else {
                    dataSet = dataSetRtn.value;
                    dataSet.DataSetName = "DataSet1";
                    dataSet.Fields.push(fieldDef);
                    dataSet.Fields.push(fieldDef1);
                    
                    dataSet1 = dataSetRtn1.value;
                    dataSet1.DataSetName = "DataSet2";
                    dataSet1.Fields.push(fieldDef2);
                    dataSet1.Fields.push(fieldDef3);
                    
//                    printTemplate.DatasettingList.push(dataSet);
//                    printTemplate.DatasettingList.push(dataSet1);
//                    printTemplate.DatasettingList.push(dataSet1);
//                      printTemplate.DatasettingList.push(dataSet1);
                    
                    //����Ҫ�༭�Ķ�������
                    var diagArgs = new Object();
                    diagArgs.id = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
                    diagArgs.printTemplate =  printTemplate;
                    
                   
                    window.showModalDialog("BarcodeProperty.aspx", diagArgs, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
                    
                }
            }

          
        }        
    }

}


function blankdataset()
{
    
    var templateRtn =  com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    var printTemplate;
    var barcode;
    var barcodeRtn;
     var barcode1;
    var barcodeRtn1;
    var fieldDefRtn;
    var fieldDef;
    var fieldDefRtn1;
    var fieldDef1;
    var fieldDefRtn2;
    var fieldDef2;
    var fieldDefRtn3;
    var fieldDef3;
    var dataSetRtn;
    var dataSet;
    var dataSetRtn1;
    var dataSet1;
    var dataObject;
    var dataObject1;
    var dataObject2;
    var dataObject3;
    
    
	                
    var rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject = rtn.value;
    dataObject.ObjectName = "33";
    dataObject.Source = "<%=Constants.DATA_SOURCE_SCREEN_DATA%>";
  
    dataObject.DisplayTxt = "liuxin";
    
    dataObject.DataSetDep = "DataSet1";
    dataObject.OutputFiledDep = "a";
    dataObject.DependencedValue = "2";
    
    rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject1 = rtn.value;
    dataObject1.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
    dataObject1.Source = "<%=Constants.DATA_SOURCE_DATA_SET%>";
    dataObject1.DataSet = "DataSet1";
    dataObject1.OutputField = "b";
    dataObject1.DisplayTxt = "liuxin";
    dataObject1.DataSetDep = "DataSet2";
    dataObject1.OutputFiledDep = "d";
    dataObject1.DependencedValue = "2";
    
     rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject2 = rtn.value;
    dataObject2.ObjectName = "44";
    dataObject2.Source = "<%=Constants.DATA_SOURCE_DATE%>";
    dataObject2.DataFormat = "DD.MM.YYYY";
    dataObject2.DateOffset = "11";
    dataObject2.DateOffsetUnitType = "Year"
    dataObject2.DataSetDep = "DataSet1";
    dataObject2.OutputFiledDep = "a";
    dataObject2.DependencedValue = "2";
    
    rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject3 = rtn.value;
    dataObject3.ObjectName = "22";
    dataObject3.Source = "<%=Constants.DATA_SOURCE_PAGE%>";
    dataObject3.PageFormat = "<%=Constants.PAGE_FORMAT_2%>";
    dataObject3.DataSetDep = "DataSet1";
    dataObject3.OutputFiledDep = "a";
    dataObject3.DependencedValue = "b";
    
    
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        printTemplate = templateRtn.value;
        printTemplate.DataObjects.push(dataObject);
        printTemplate.DataObjects.push(dataObject1);
        printTemplate.DataObjects.push(dataObject2);
        printTemplate.DataObjects.push(dataObject3);
        
        barcodeRtn = com.inventec.template.manager.TemplateManager.getBarcodeFromStructure();
       
        if (barcodeRtn.error != null) {
            alert(barcodeRtn.error.Message);
        } else {
            barcode = barcodeRtn.value;
           barcode.ObjectName = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
           printTemplate.PageHeader.BarcodeObjects.push(barcode);
         
           fieldDefRtn = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn1 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn2 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn3 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           if (fieldDefRtn.error != null) {
                alert(fieldDefRtn.error.Message);
            } else {
                fieldDef = fieldDefRtn.value;
                fieldDef.FieldName1 = "a";
                fieldDef.Type = "int";
                fieldDef1 = fieldDefRtn1.value;
                fieldDef1.FieldName1 = "b";
                fieldDef1.Type = "string";
                fieldDef2 = fieldDefRtn2.value;
                fieldDef2.FieldName1 = "c";
                fieldDef2.Type = "string";
                fieldDef3 = fieldDefRtn3.value;
                fieldDef3.FieldName1 = "d";
                fieldDef3.Type = "string";
               dataSetRtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               dataSetRtn1 = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               if (dataSetRtn.error != null) {
                    alert(dataSetRtn.error.Message);
                } else {
                    dataSet = dataSetRtn.value;
                    dataSet.DataSetName = "DataSet1";
                    dataSet.Fields.push(fieldDef);
                    dataSet.Fields.push(fieldDef1);
                    
                    dataSet1 = dataSetRtn1.value;
                    dataSet1.DataSetName = "DataSet2";
                    dataSet1.Fields.push(fieldDef2);
                    dataSet1.Fields.push(fieldDef3);
                    
                    //printTemplate.DatasettingList.push(dataSet);
                    //printTemplate.DatasettingList.push(dataSet1);
                    var diagArgs = new Object();
                    diagArgs.id = "printTemplateInfo.PageHeader.BarcodeObjects_a0_b";
                    diagArgs.printTemplate =  printTemplate;
                    
                         
                    window.showModalDialog("BarcodeProperty.aspx", diagArgs, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
                    
                }
            }

          
        }        
    }

}

function blank()
{
window.showModalDialog("1.aspx", "", "dialogWidth:600px;dialogHeight:500px;center:yes;scroll:off");
}



function newtext()
{
   
    var printTemplate;
    var barcode;
    var barcodeRtn;
     var barcode1;
    var barcodeRtn1;
    var fieldDefRtn;
    var fieldDef;
    var fieldDefRtn1;
    var fieldDef1;
    var fieldDefRtn2;
    var fieldDef2;
    var fieldDefRtn3;
    var fieldDef3;
    var dataSetRtn;
    var dataSet;
    var dataSetRtn1;
    var dataSet1;
    var dataObject;
    var dataObject1;
    var dataObject2;
    var dataObject3;
    
    
	//����dataObject���󣬳�������barcode��������һ���⣬���඼Ϊ��        
    var rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject = rtn.value;
    dataObject.ObjectName = "11";
//    dataObject.Source = "<%=Constants.DATA_SOURCE_DATA_SET%>";
//    dataObject.DataSet = "DataSet3";
//    dataObject.OutputField = "f";
//    dataObject.DisplayTxt = "liuxin";
//    dataObject.DataSetDep = "DataSet3";
//    dataObject.OutputFiledDep = "c";
//    dataObject.DependencedValue = "2";
    
   
    
    
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        printTemplate = templateRtn.value;
        
        printTemplate.DataObjects.push(dataObject);
       
        //����һ��barcode����
        barcodeRtn = com.inventec.template.manager.TemplateManager.getTextFromStructure();
       
        if (barcodeRtn.error != null) {
            alert(barcodeRtn.error.Message);
        } else {
            barcode = barcodeRtn.value;
           //ע�������봴����dataobjectһ��
           barcode.ObjectName = "11";
           printTemplate.PageHeader.TextObjects.push(barcode);
           //edit���������򳤿�
           printTemplate.PageHeader.AreaHeight = "20"
           printTemplate.TemplateWidth = "30"
         
           //����Ҳ��edit������dataset
           fieldDefRtn = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn1 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn2 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn3 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           if (fieldDefRtn.error != null) {
                alert(fieldDefRtn.error.Message);
            } else {
                fieldDef = fieldDefRtn.value;
                fieldDef.FieldName1 = "a";
                fieldDef.Type = "int";
                fieldDef1 = fieldDefRtn1.value;
                fieldDef1.FieldName1 = "b";
                fieldDef1.Type = "string";
                fieldDef2 = fieldDefRtn2.value;
                fieldDef2.FieldName1 = "c";
                fieldDef2.Type = "string";
                fieldDef3 = fieldDefRtn3.value;
                fieldDef3.FieldName1 = "d";
                fieldDef3.Type = "string";
               dataSetRtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               dataSetRtn1 = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               if (dataSetRtn.error != null) {
                    alert(dataSetRtn.error.Message);
                } else {
                    dataSet = dataSetRtn.value;
                    dataSet.DataSetName = "DataSet1";
                    dataSet.Fields.push(fieldDef);
                    dataSet.Fields.push(fieldDef1);
                    
                    dataSet1 = dataSetRtn1.value;
                    dataSet1.DataSetName = "DataSet2";
                    dataSet1.Fields.push(fieldDef2);
                    dataSet1.Fields.push(fieldDef3);
                    
//                    printTemplate.DatasettingList.push(dataSet);
//                    printTemplate.DatasettingList.push(dataSet1);
//                    printTemplate.DatasettingList.push(dataSet1);
//                      printTemplate.DatasettingList.push(dataSet1);
                    
                    //����Ҫ�༭�Ķ�������
                    var diagArgs = new Object();
                    diagArgs.id = "printTemplateInfo_dot_PageHeader_dot_TextObjects_a0_b";
                    diagArgs.printTemplate =  printTemplate;
                    
                   debugger;
                    window.showModalDialog("TextProperty.aspx", diagArgs, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
                    debugger;
                    var i = 0;
                }
            }

          
        }        
    }

}

function areatext()
{
   
    var printTemplate;
    var barcode;
    var barcodeRtn;
     var barcode1;
    var barcodeRtn1;
    var fieldDefRtn;
    var fieldDef;
    var fieldDefRtn1;
    var fieldDef1;
    var fieldDefRtn2;
    var fieldDef2;
    var fieldDefRtn3;
    var fieldDef3;
    var dataSetRtn;
    var dataSet;
    var dataSetRtn1;
    var dataSet1;
    var dataObject;
    var dataObject1;
    var dataObject2;
    var dataObject3;
   var area;
    
    
	//����dataObject���󣬳�������barcode��������һ���⣬���඼Ϊ��        
    var rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject = rtn.value;
    dataObject.ObjectName = "printTemplateInfo.PageHeader.ArearObjects_a0_b.TextObjects_a0_b";
   
    
   
    
    
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        printTemplate = templateRtn.value;
        
        printTemplate.DataObjects.push(dataObject);
       
          var a = com.inventec.template.manager.TemplateManager.getAreaFromStructure();
          area = a.value;
        //����һ��barcode����
        barcodeRtn = com.inventec.template.manager.TemplateManager.getTextFromStructure();
       
        if (barcodeRtn.error != null) {
            alert(barcodeRtn.error.Message);
        } else {
            barcode = barcodeRtn.value;
           //ע�������봴����dataobjectһ��
           barcode.ObjectName = "printTemplateInfo.PageHeader.ArearObjects_a0_b.TextObjects_a0_b";
           area.TextObjects.push(barcode);
           printTemplate.PageHeader.ArearObjects.push(area);
           //edit���������򳤿�
           printTemplate.PageHeader.AreaHeight = "20"
           printTemplate.TemplateWidth = "30"
         
           //����Ҳ��edit������dataset
           fieldDefRtn = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn1 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn2 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn3 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           if (fieldDefRtn.error != null) {
                alert(fieldDefRtn.error.Message);
            } else {
                fieldDef = fieldDefRtn.value;
                fieldDef.FieldName1 = "a";
                fieldDef.Type = "int";
                fieldDef1 = fieldDefRtn1.value;
                fieldDef1.FieldName1 = "b";
                fieldDef1.Type = "string";
                fieldDef2 = fieldDefRtn2.value;
                fieldDef2.FieldName1 = "c";
                fieldDef2.Type = "string";
                fieldDef3 = fieldDefRtn3.value;
                fieldDef3.FieldName1 = "d";
                fieldDef3.Type = "string";
               dataSetRtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               dataSetRtn1 = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               if (dataSetRtn.error != null) {
                    alert(dataSetRtn.error.Message);
                } else {
                    dataSet = dataSetRtn.value;
                    dataSet.DataSetName = "DataSet1";
                    dataSet.Fields.push(fieldDef);
                    dataSet.Fields.push(fieldDef1);
                    
                    dataSet1 = dataSetRtn1.value;
                    dataSet1.DataSetName = "DataSet2";
                    dataSet1.Fields.push(fieldDef2);
                    dataSet1.Fields.push(fieldDef3);
                    
                    printTemplate.DatasettingList.push(dataSet);
                    printTemplate.DatasettingList.push(dataSet1);
                    printTemplate.DatasettingList.push(dataSet1);
                      printTemplate.DatasettingList.push(dataSet1);
                    
                    //����Ҫ�༭�Ķ�������
                    var diagArgs = new Object();
                    diagArgs.id = "printTemplateInfo.PageHeader.ArearObjects_a0_b.TextObjects_a0_b";
                    diagArgs.printTemplate =  printTemplate;
                    
                   debugger;
                    window.showModalDialog("TextProperty.aspx", diagArgs, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
                    var i = 0;
                }
            }

          
        }        
    }

}
function celltext()
{
   debugger;
    var printTemplate;
    var barcode;
    var barcodeRtn;
     var barcode1;
    var barcodeRtn1;
    var fieldDefRtn;
    var fieldDef;
    var fieldDefRtn1;
    var fieldDef1;
    var fieldDefRtn2;
    var fieldDef2;
    var fieldDefRtn3;
    var fieldDef3;
    var dataSetRtn;
    var dataSet;
    var dataSetRtn1;
    var dataSet1;
    var dataObject;
    var dataObject1;
    var dataObject2;
    var dataObject3;
    
    
	//����dataObject���󣬳�������barcode��������һ���⣬���඼Ϊ��        
    var rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject = rtn.value;
    dataObject.ObjectName = "11";
    dataObject.DataSet = "DataSet2";
    dataObject.Source = "<%=Constants.DATA_SOURCE_DATA_SET%>";
    dataObject.OutputField = "b";
    dataObject.DataSetDep = "DataSet4";
    dataObject.OutputFiledDep = "a";
    dataObject.DependencedValue = "2";
   
     var rtn1 = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject1 = rtn1.value;
    dataObject1.ObjectName = "22";
    dataObject1.Source = "<%=Constants.DATA_SOURCE_DATA_SET%>";
    dataObject1.DataSet = "DataSet2";
    
    
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        printTemplate = templateRtn.value;
        
        printTemplate.DataObjects.push(dataObject);
         printTemplate.DataObjects.push(dataObject1);
         printTemplate.TemplateWidth = "40";
       
       
        //����һ��barcode����
        barcodeRtn = com.inventec.template.manager.TemplateManager.getTextFromStructure();
       barcodeRtn1 = com.inventec.template.manager.TemplateManager.getBarcodeFromStructure();
        if (barcodeRtn.error != null) {
            alert(barcodeRtn.error.Message);
        } else {
            barcode = barcodeRtn.value;
            barcode1 = barcodeRtn1.value;
           //ע�������봴����dataobjectһ��
           barcode.ObjectName = "11";
           barcode1.ObjectName = "22";
//            barcode.ObjectName = "printTemplateInfo_dot_DetailSections_a0_b_dot_HeaderArea_dot_TextObjects_a0_b";
//           barcode1.ObjectName = "printTemplateInfo_dot_DetailSections_a0_b_dot_HeaderArea_dot_BarcodeObjects_a1_b";
           var cell = com.inventec.template.manager.TemplateManager.getSectionCellFromStructure();
           var secCell = cell.value;
           secCell.TextObjects.push(barcode);
           secCell.BarcodeObjects.push(barcode1);
            var sec = com.inventec.template.manager.TemplateManager.getSectionFromStructure();
           var section = sec.value;
            section.RowHeight= "20"
            section.RowHeight= "20"
            section.HeaderHeight= "30"
            section.ColumnNum = "3";
           section.Cells.push(secCell);
//            section.HeaderArea.TextObjects.push(barcode);
//            section.HeaderArea.BarcodeObjects.push(barcode);
       
           printTemplate.DetailSections.push(section);
           
           //edit���������򳤿�
          
         
           //����Ҳ��edit������dataset
           fieldDefRtn = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn1 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn2 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn3 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
        
           if (fieldDefRtn.error != null) {
                alert(fieldDefRtn.error.Message);
            } else {
                fieldDef = fieldDefRtn.value;
                fieldDef.FieldName1 = "a";
                fieldDef.Type = "int";
                fieldDef1 = fieldDefRtn1.value;
                fieldDef1.FieldName1 = "b";
                fieldDef1.Type = "string";
                fieldDef2 = fieldDefRtn2.value;
                fieldDef2.FieldName1 = "c";
                fieldDef2.Type = "string";
                fieldDef3 = fieldDefRtn3.value;
                fieldDef3.FieldName1 = "d";
                fieldDef3.Type = "string";
              
               dataSetRtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               dataSetRtn1 = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               if (dataSetRtn.error != null) {
                    alert(dataSetRtn.error.Message);
                } else {
                    dataSet = dataSetRtn.value;
                    dataSet.DataSetName = "DataSet1";
                    dataSet.Fields.push(fieldDef);
                    dataSet.Fields.push(fieldDef1);
                    
                    dataSet1 = dataSetRtn1.value;
                    dataSet1.DataSetName = "DataSet2";
                    dataSet1.Fields.push(fieldDef2);
                    dataSet1.Fields.push(fieldDef3);
                    
                    printTemplate.DatasettingList.push(dataSet);
                    printTemplate.DatasettingList.push(dataSet1);
                  
                    //����Ҫ�༭�Ķ�������
                    var diagArgs = new Object();
                    diagArgs.id = "printTemplateInfo_dot_DetailSections_a0_b_dot_Cells_a0_b_dot_TextObjects_a0_b";
                    diagArgs.printTemplate =  printTemplate;
                    
                   debugger;
                    window.showModalDialog("TextProperty.aspx", diagArgs, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
                    var i = 0;
                }
            }

          
        }        
    }

}

function cellareatext()
{
   debugger;
    var printTemplate;
    var barcode;
    var barcodeRtn;
     var barcode1;
    var barcodeRtn1;
    var fieldDefRtn;
    var fieldDef;
    var fieldDefRtn1;
    var fieldDef1;
    var fieldDefRtn2;
    var fieldDef2;
    var fieldDefRtn3;
    var fieldDef3;
    var dataSetRtn;
    var dataSet;
    var dataSetRtn1;
    var dataSet1;
    var dataObject;
    var dataObject1;
    var dataObject2;
    var dataObject3;
    var area;
    
	//����dataObject���󣬳�������barcode��������һ���⣬���඼Ϊ��        
    var rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject = rtn.value;
    dataObject.ObjectName = "printTemplateInfo.DetailSections_a0_b.Cells_a0_b..ArearObjects_a0_b.TextObjects_a0_b";
    dataObject.DataSet = "DataSet2";
    dataObject.Source = "<%=Constants.DATA_SOURCE_DATA_SET%>";
    dataObject.OutputField = "b";
    dataObject.DataSetDep = "DataSet1";
    dataObject.OutputFiledDep = "a";
    dataObject.DependencedValue = "2";
   
     var rtn1 = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject1 = rtn1.value;
    dataObject1.ObjectName = "printTemplateInfo.DetailSections_a0_b.Cells_a0_b.ArearObjects_a0_b.BarcodeObjects_a0_b";
   dataObject1.DataSet = "DataSet2";
    
    
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getPrintTemplateInfoFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        printTemplate = templateRtn.value;
        
        printTemplate.DataObjects.push(dataObject);
         printTemplate.DataObjects.push(dataObject1);
       
       
        //����һ��barcode����
        barcodeRtn = com.inventec.template.manager.TemplateManager.getTextFromStructure();
       barcodeRtn1 = com.inventec.template.manager.TemplateManager.getBarcodeFromStructure();
        if (barcodeRtn.error != null) {
            alert(barcodeRtn.error.Message);
        } else {
            barcode = barcodeRtn.value;
            barcode1 = barcodeRtn1.value;
           //ע�������봴����dataobjectһ��
           barcode.ObjectName = "printTemplateInfo.DetailSections_a0_b.Cells_a0_b.ArearObjects_a0_b.TextObjects_a0_b";
           barcode1.ObjectName = "printTemplateInfo.DetailSections_a0_b.Cells_a0_b.ArearObjects_a0_b.BarcodeObjects_a0_b";
           var cell = com.inventec.template.manager.TemplateManager.getSectionCellFromStructure();
           var secCell = cell.value;
//           secCell.TextObjects.push(barcode);
//           secCell.BarcodeObjects.push(barcode1);
            var sec = com.inventec.template.manager.TemplateManager.getSectionFromStructure();
           var section = sec.value;
           section.AreaHeight = "20"
            section.RowHeight= "20"
            var a = com.inventec.template.manager.TemplateManager.getAreaFromStructure();
          area = a.value;
         area.TextObjects.push(barcode);
          area.BarcodeObjects.push(barcode1);
          secCell.ArearObjects.push(area);
            section.Cells.push(secCell);
       
           printTemplate.DetailSections.push(section);
           
           //edit���������򳤿�
          
         
           //����Ҳ��edit������dataset
           fieldDefRtn = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn1 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn2 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn3 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
        
           if (fieldDefRtn.error != null) {
                alert(fieldDefRtn.error.Message);
            } else {
                fieldDef = fieldDefRtn.value;
                fieldDef.FieldName1 = "a";
                fieldDef.Type = "int";
                fieldDef1 = fieldDefRtn1.value;
                fieldDef1.FieldName1 = "b";
                fieldDef1.Type = "string";
                fieldDef2 = fieldDefRtn2.value;
                fieldDef2.FieldName1 = "c";
                fieldDef2.Type = "string";
                fieldDef3 = fieldDefRtn3.value;
                fieldDef3.FieldName1 = "d";
                fieldDef3.Type = "string";
              
               dataSetRtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               dataSetRtn1 = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               if (dataSetRtn.error != null) {
                    alert(dataSetRtn.error.Message);
                } else {
                    dataSet = dataSetRtn.value;
                    dataSet.DataSetName = "DataSet1";
                    dataSet.Fields.push(fieldDef);
                    dataSet.Fields.push(fieldDef1);
                    
                    dataSet1 = dataSetRtn1.value;
                    dataSet1.DataSetName = "DataSet2";
                    dataSet1.Fields.push(fieldDef2);
                    dataSet1.Fields.push(fieldDef3);
                    
                    printTemplate.DatasettingList.push(dataSet);
                    printTemplate.DatasettingList.push(dataSet1);
                  
                    //����Ҫ�༭�Ķ�������
                    var diagArgs = new Object();
                    diagArgs.id = "printTemplateInfo.DetailSections_a0_b.Cells_a0_b.ArearObjects_a0_b.TextObjects_a0_b";
                    diagArgs.printTemplate =  printTemplate;
                    
                   debugger;
                    window.showModalDialog("TextProperty.aspx", diagArgs, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
                    var i = 0;
                }
            }

          
        }        
    }

}
function setcell()
{
   debugger;
    var printTemplate;
    var barcode;
    var barcodeRtn;
     var barcode1;
    var barcodeRtn1;
    var fieldDefRtn;
    var fieldDef;
    var fieldDefRtn1;
    var fieldDef1;
    var fieldDefRtn2;
    var fieldDef2;
    var fieldDefRtn3;
    var fieldDef3;
    var dataSetRtn;
    var dataSet;
    var dataSetRtn1;
    var dataSet1;
    var dataObject;
    var dataObject1;
    var dataObject2;
    var dataObject3;
    var templateXmlandHtml;
    
    
	//����dataObject���󣬳�������barcode��������һ���⣬���඼Ϊ��        
    var rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject = rtn.value;
    dataObject.ObjectName = "11";
   
     var rtn1 = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject1 = rtn1.value;
    dataObject1.ObjectName = "22";
   dataObject1.DataSet = "DataSet2";
    
    
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getTemplateXmlAndHtmlFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        templateXmlandHtml = templateRtn.value;
        
        templateXmlandHtml.PrintTemplateInfo.DataObjects.push(dataObject);
         templateXmlandHtml.PrintTemplateInfo.DataObjects.push(dataObject1);
       
       
        //����һ��barcode����
        barcodeRtn = com.inventec.template.manager.TemplateManager.getTextFromStructure();
       barcodeRtn1 = com.inventec.template.manager.TemplateManager.getBarcodeFromStructure();
        if (barcodeRtn.error != null) {
            alert(barcodeRtn.error.Message);
        } else {
            barcode = barcodeRtn.value;
            barcode1 = barcodeRtn1.value;
           //ע�������봴����dataobjectһ��
           barcode.ObjectName = "11";
           barcode.X = "40";
           barcode1.ObjectName = "22";
           barcode1.X = "30"
           var cell = com.inventec.template.manager.TemplateManager.getSectionCellFromStructure();
           var secCell = cell.value;
           secCell.TextObjects.push(barcode);
           secCell.BarcodeObjects.push(barcode1);
            var sec = com.inventec.template.manager.TemplateManager.getSectionFromStructure();
           var section = sec.value;
           section.HeaderHeight = "20"
            section.ColumnNum = 3;
            section.RowHeight = "30"
            section.Cells.push(secCell);
       
           templateXmlandHtml.PrintTemplateInfo.DetailSections.push(section);
           
           //edit���������򳤿�
          
         
           //����Ҳ��edit������dataset
           fieldDefRtn = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn1 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn2 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn3 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
        
           if (fieldDefRtn.error != null) {
                alert(fieldDefRtn.error.Message);
            } else {
                fieldDef = fieldDefRtn.value;
                fieldDef.FieldName1 = "a";
                fieldDef.Type = "int";
                fieldDef1 = fieldDefRtn1.value;
                fieldDef1.FieldName1 = "b";
                fieldDef1.Type = "string";
                fieldDef2 = fieldDefRtn2.value;
                fieldDef2.FieldName1 = "c";
                fieldDef2.Type = "string";
                fieldDef3 = fieldDefRtn3.value;
                fieldDef3.FieldName1 = "d";
                fieldDef3.Type = "string";
              
               dataSetRtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               dataSetRtn1 = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               if (dataSetRtn.error != null) {
                    alert(dataSetRtn.error.Message);
                } else {
                    dataSet = dataSetRtn.value;
                    dataSet.DataSetName = "DataSet1";
                    dataSet.Fields.push(fieldDef);
                    dataSet.Fields.push(fieldDef1);
                    
                    dataSet1 = dataSetRtn1.value;
                    dataSet1.DataSetName = "DataSet2";
                    dataSet1.Fields.push(fieldDef2);
                    dataSet1.Fields.push(fieldDef3);
                    
                    templateXmlandHtml.PrintTemplateInfo.DatasettingList.push(dataSet);
                    templateXmlandHtml.PrintTemplateInfo.DatasettingList.push(dataSet1);
                     templateXmlandHtml.PrintTemplateInfo.Width = "30";
                     templateXmlandHtml.PrintTemplateInfo.Height = "40";
                  
                    //����Ҫ�༭�Ķ�������
                  ;
                    
                   debugger;
                    var ret = window.showModalDialog("TemplateSetting.aspx", templateXmlandHtml, "dialogWidth:480px;dialogHeight:320px;center:yes;scroll:off")
                    var i = 0;
                }
            }

          
        }        
    }

}


function save()
{
   debugger;
    var printTemplate;
    var barcode;
    var barcodeRtn;
     var barcode1;
    var barcodeRtn1;
    var fieldDefRtn;
    var fieldDef;
    var fieldDefRtn1;
    var fieldDef1;
    var fieldDefRtn2;
    var fieldDef2;
    var fieldDefRtn3;
    var fieldDef3;
    var dataSetRtn;
    var dataSet;
    var dataSetRtn1;
    var dataSet1;
    var dataObject;
    var dataObject1;
    var dataObject2;
    var dataObject3;
    var templateXmlandHtml;
    
    
	//����dataObject���󣬳�������barcode��������һ���⣬���඼Ϊ��        
    var rtn = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject = rtn.value;
    dataObject.ObjectName = "11";
   
     var rtn1 = com.inventec.template.manager.TemplateManager.getDataObjectFromStructure();
    dataObject1 = rtn1.value;
    dataObject1.ObjectName = "22";
   dataObject1.DataSet = "DataSet2";
    
    
    
     //����һ��printtemplate����(����Ķ���Ӧ���Ǵ�edit���õ��ģ��������½���)
    var templateRtn =  com.inventec.template.manager.TemplateManager.getTemplateXmlAndHtmlFromStructure();
    if (templateRtn.error != null) {
        alert(templateRtn.error.Message);
    } else {
        templateXmlandHtml = templateRtn.value;
        
        templateXmlandHtml.PrintTemplateInfo.DataObjects.push(dataObject);
         templateXmlandHtml.PrintTemplateInfo.DataObjects.push(dataObject1);
       
       
        //����һ��barcode����
        barcodeRtn = com.inventec.template.manager.TemplateManager.getTextFromStructure();
       barcodeRtn1 = com.inventec.template.manager.TemplateManager.getBarcodeFromStructure();
        if (barcodeRtn.error != null) {
            alert(barcodeRtn.error.Message);
        } else {
            barcode = barcodeRtn.value;
            barcode1 = barcodeRtn1.value;
           //ע�������봴����dataobjectһ��
           barcode.ObjectName = "11";
           barcode.X = "40";
           barcode1.ObjectName = "22";
           barcode1.X = "30"
           var cell = com.inventec.template.manager.TemplateManager.getSectionCellFromStructure();
           var secCell = cell.value;
           secCell.TextObjects.push(barcode);
           secCell.BarcodeObjects.push(barcode1);
            var sec = com.inventec.template.manager.TemplateManager.getSectionFromStructure();
           var section = sec.value;
           section.AreaHeight = "20"
            section.AreaHeight = "30"
            section.Cells.push(secCell);
       
           templateXmlandHtml.PrintTemplateInfo.DetailSections.push(section);
           
           //edit���������򳤿�
          
         
           //����Ҳ��edit������dataset
           fieldDefRtn = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn1 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn2 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
           fieldDefRtn3 = com.inventec.template.manager.TemplateManager.getFieldDefFromStructure();
        
           if (fieldDefRtn.error != null) {
                alert(fieldDefRtn.error.Message);
            } else {
                fieldDef = fieldDefRtn.value;
                fieldDef.FieldName1 = "a";
                fieldDef.Type = "int";
                fieldDef1 = fieldDefRtn1.value;
                fieldDef1.FieldName1 = "b";
                fieldDef1.Type = "string";
                fieldDef2 = fieldDefRtn2.value;
                fieldDef2.FieldName1 = "c";
                fieldDef2.Type = "string";
                fieldDef3 = fieldDefRtn3.value;
                fieldDef3.FieldName1 = "d";
                fieldDef3.Type = "string";
              
               dataSetRtn = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               dataSetRtn1 = com.inventec.template.manager.TemplateManager.getDataSetFromStructure();
               if (dataSetRtn.error != null) {
                    alert(dataSetRtn.error.Message);
                } else {
                    dataSet = dataSetRtn.value;
                    dataSet.DataSetName = "DataSet1";
                    dataSet.Fields.push(fieldDef);
                    dataSet.Fields.push(fieldDef1);
                    
                    
                    dataSet1 = dataSetRtn1.value;
                    dataSet1.DataSetName = "DataSet2";
                    dataSet1.Fields.push(fieldDef2);
                    dataSet1.Fields.push(fieldDef3);
                   
                    templateXmlandHtml.PrintTemplateInfo.DatasettingList.push(dataSet);
                    templateXmlandHtml.PrintTemplateInfo.DatasettingList.push(dataSet1);
                  
                    //����Ҫ�༭�Ķ�������
                  ;
                   templateXmlandHtml.PrintTemplateInfo.FileName = "7890"
                   debugger;
                     templateXmlandHtml.toJSON = function(){return toJSON(this);};
                  var rtn = com.inventec.template.manager.TemplateManager.createPrintTemplateInfo(templateXmlandHtml, "111", userName); 
                  alert(rtn.error.Message);
                }
            }

          
        }        
    }

}


function newtemplate()
 {
   debugger;
    var ret = window.showModalDialog("TemplateSetting.aspx", "", "dialogWidth:580px;dialogHeight:450px;center:yes;scroll:off;status:no;help:no")
    debugger;

 }
//-->
</SCRIPT>
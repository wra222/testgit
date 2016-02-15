<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="WipTrackingByDN_PAK.aspx.cs" Inherits="WipTrackingByDN_PAK" EnableViewState="false" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">


    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>
    <script type="text/javascript" src="../../js/jscal2.js"></script>

    
    <script src="../../js/lang/cn.js"></script>
    <script src="../../js/jquery.dateFormat-1.0.js"></script>
    <script type="text/javascript" src="../../js/assets/prettify.js"></script>
    <script type="text/javascript" src="../../js/jquery.multiselect.js"></script>
    <script type="text/javascript" src="../../js/jquery.multiselect.filter.js"></script>

    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
    <link rel="stylesheet" type="text/css" href="../../css/assets/style.css" />
    <link rel="stylesheet" type="text/css" href="../../css/assets/prettify.css" />


    <script type="text/javascript" src="../../js/wz_tooltip.js"></script>

    <style type="text/css">
        .querycell
        {
            background-color: yellow;
        }
        .querycell:hover
        {
            background-color: #8AF2E7;
        }
        tr.clicked
        {
            background-color: #5EFB6E;
        }
        .clicked
        {
            background-color: #8AF2E7;
        }
        .row1
        {
            background-color: #A0CFEC;
        }
        .row0
        {
            background-color: #CFECEC;
        }
        .row0:hover, .row1:hover
        {
            background-color: white;
        }
        .dn-pak
        {
            font-size: 12px;
            font-weight: bold;
            color: Red;
        }
        
        .t5  /* Model & DN  */
        {
            width:150px;
        }
       .t6  /* Other  */
        {
            width:60px;
        }
        
    </style>

    <script type="text/javascript">
  
//Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(clearDisposableItems);
  function load() {
      Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

  }

  function EndRequestHandler(sender, args) {

      $('.CheckBoxList').multiselect({ selectedList: 1, 
                                                       position: { my: 'left bottom',
                                                                          at: 'left top' }, 
                                                       noneSelectedText: 'Please Select Line ' 
                                                          //,show: ["bounce", 200],
                                                            //hide: ["explode", 1000]
                                                       }).multiselectfilter();

  // $("#mainTable").toSuperTable({ width: "98%", height: "300px", fixedCols: 2 });
  }

    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
     
    </asp:ScriptManager>
 
      
      <div style=" visibility:hidden">
      <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
       <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
      </div>
        <table  width="100%"style='border-width:2px; border-style:inset; background-color:#E3E4FA' id='menu'>
            <tr>
                <td >
                    <asp:Label ID="lblModel" runat="server" Text="Ship Date:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" >
                    <asp:TextBox ID="txtShipDate" runat="server" Width="157px" Height="20px"></asp:TextBox>
                </td>
                 <td>
                   <asp:Label ID="lblInput" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                   <asp:TextBox ID="txtModel" runat="server" Width="199px" Height="17px"></asp:TextBox>
                    <input id="BtnBrowse" type="button" value="Input" onclick="UploadModelList()" style=" width:40px" />
                </td>
                <td align="left" >
                    <asp:Label ID="Label1" runat="server" Text="Process:" CssClass="iMes_label_13pt"></asp:Label>
                    <select onchange="SetProcess()" id="dradProcess">
                        <option>ALL</option>
                        <option>FA</option>
                        <option>PAK</option>
                    </select>
                </td>
                <td>
                    <asp:Label ID="lblGrType" runat="server" Text="Type:" CssClass="iMes_label_13pt"></asp:Label>
                    <select onchange="SetType()" id="dradType">
                        <option>Model</option>
                        <option>Model+Line</option>
                    </select>
                      <select  id="droPrdType">
                        <option>PC</option>
                        <option>Tablet</option>
                        <option>RCTO</option>
                        <option>ThinClient</option>
                    </select>
                </td>
               
                <td align="right">
              
                <asp:Label ID="lblLine" runat="server" Text="Line:" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:ListBox ID="lboxPdLine" runat="server" CssClass="CheckBoxList" Height="95%"
                        SelectionMode="Multiple" Width="250px"></asp:ListBox>
                </td>
            </tr>
            
          
            <tr>
               
                <td align="left" colspan="2">
                    <asp:Label ID="lblTotalQty" runat="server" Text="TotalQty:"
                        Font-Bold="True" ForeColor="Red" Font-Size="X-Large" Font-Names="Arial"></asp:Label>
                    <asp:Label ID="lblTotalQtyCount" runat="server" Font-Size="X-Large"></asp:Label>
                </td>
                <td colspan="3">
                 
                    <asp:Label ID="lblActualQty" runat="server" Text="ActualQty(Pass 85) :" 
                        Font-Size="X-Large"></asp:Label>
                    <asp:Label ID="lblActualQtyCount" runat="server"  Font-Size="X-Large"></asp:Label>
                    </td>
                <td align="right" colspan="2">
                    <input id="btnShow85" type="button" value="No Pass85"style="display:none" onclick="ShowWait();Show85()"/>&nbsp;<input id="Button2" type="button" value="Excel" onclick="DownExcel()" />
                   
                    <input id="Button1" type="button" value="Query" onclick="GetMain()" style="display:none"/>
                    <input id="btnGetQforAjax" type="button" value="Query" onclick="GetMain()" />
               
                </td>
            </tr>
        </table>
      
   
    <br />
    <div id="tbMain">
    </div>
    <br />
    <div id="tbDetail">
    </div>
    <asp:HiddenField ID="hidModelType" runat="server" Value="1" />
    <asp:HiddenField ID="hidModelList" runat="server" />
    <asp:HiddenField ID="hidConnection" runat="server" Value='' />
    <asp:HiddenField ID="hidDBName" runat="server" Value='' />
    <asp:HiddenField ID="hidProcess" runat="server" Value="ALL" />
    <asp:HiddenField ID="hidType" runat="server" Value="Model" />
    <asp:HiddenField ID="hidExcelPath" runat="server" Value="" />
    <asp:HiddenField ID="hidReqProcess" runat="server" Value='' />
    <asp:HiddenField ID="hidDockingDB" runat="server" Value='' />

    <script type="text/javascript">          //<![CDATA[
          Calendar.setup({
              inputField: "<%=txtShipDate.ClientID%>",
              trigger: "<%=txtShipDate.ClientID%>",
              onSelect: function() { this.hide(); GetDNQty();},
              showTime: 24,
              dateFormat: "%Y-%m-%d",
              minuteStep: 1
          });


          //]]></script>

    <script type="text/javascript">
     var dbName;

     window.onload = function() {
         load();
         EndRequestHandler();
         GetDNQty();
         dbName = document.getElementById("<%=hidDBName.ClientID %>").value;
         //  WebServicePAKWipTrackingByND_PAK.GetConfigDefaultDB(onProdIdSucceed, onProdIdFailed);
         if (document.getElementById("<%=hidDockingDB.ClientID %>").value == "Y")
         { $('#droPrdType').hide(); }

     };
     function onProdIdSucceed(result) {
        try {
            if (dbName.toUpperCase().indexOf(result) >= 0)
            { $('#droPrdType').hide(); }

        }
        catch (e) {
            alert(e.description);
        }
     }
     function onProdIdFailed(result) {
         try {
         
         }
         catch (e) {
             alert(e.description);
         }
     }
    </script>

    <script type="text/javascript">
    
    
    function getParameterByName(name)
    {
      name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
      var regexS = "[\\?&]" + name + "=([^&#]*)";
      var regex = new RegExp(regexS);
      var results = regex.exec(window.location.search);
      if(results == null)
        return "";
      else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
    }
      function SetProcess()
      {
         var rbvalue ;
         rbvalue=$("#dradProcess option:selected").val();
         var processID='#'+ ConvertID("hidProcess");
          $(processID).val(rbvalue) ;
      }
       function SetType()
      {
         var rbvalue ;
          rbvalue=$("#dradType option:selected").val();
         var typeID='#'+ ConvertID("hidType");
          $(typeID).val(rbvalue) ;
      }
     

        function SelectDetail(station, line, model,dn) {
          ShowWait();
          
            $(".clicked").removeClass("clicked");
            $($(event.srcElement).parent()).addClass("clicked");
            $(event.srcElement).addClass("clicked");
              GetDetail(station,line,model,dn);
        }
        function ChangeRowColor() {
            $(".clicked").removeClass("clicked");
            $($(event.srcElement).parent()).addClass("clicked");
        }
		var SelRowIdx;
         function CR(row) {
		    var RowIndex=row.rowIndex;
            var newSelectRow= $("#mainTable > tbody > tr ")[RowIndex];
            if(SelRowIdx!=0)
            {
              $($("#mainTable > tbody > tr ")[SelRowIdx]).removeClass("clicked");
            }
            $($("#mainTable > tbody > tr ")[RowIndex]).addClass("clicked");
            SelRowIdx=RowIndex;
          //  $(".clicked").removeClass("clicked");
           // $($(event.srcElement).parent()).addClass("clicked");
        }
        function UploadModelList() {
       
            var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
            var saveasUrl = "../../UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelList.ClientID %>").value;
            var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
            if (dlgReturn) {
                dlgReturn = dlgReturn.replace(/\r\n/g, ",");
                document.getElementById("<%=hidModelList.ClientID %>").value = RemoveBlank(dlgReturn);
            }
            else {
                if (dlgReturn == "")
                { document.getElementById("<%=hidModelList.ClientID %>").value = ""; }
                return;

            }

        }
        function RemoveBlank(modelList) {
            var arr = modelList.split(",");
            var ic=arr.length;
            var model = "";
            if (modelList != "") {
            
               for(var i=0;i<arr.length;i++)
               {
                  if (arr[i].trim() != "") { 	
                        model = model + arr[i].trim() + ",";
                    }
               }

                model = model.substring(0, model.length - 1)
            }

            return model;
        }
  
        var selLine='';
        var selType='';
        function GetMain()
        {
            beginWaitingCoverDiv();
            GetDNQty();
           var inputID='#'+ ConvertID("txtShipDate");
           var shipDate = $(inputID).val();
           var txtModelID='#'+ ConvertID("txtModel");
           var process='#'+ ConvertID("lboxPdLine");
            //var dbName=document.getElementById("<%=hidDBName.ClientID %>").value ;
           var model=$(txtModelID).val()
           var modelList= document.getElementById("<%=hidModelList.ClientID %>").value 
           var connection=document.getElementById("<%=hidConnection.ClientID %>").value ;
           var lineID='#'+ ConvertID("lboxPdLine");
       
            $(lineID +" option:selected").each(function() {
                selLine = selLine + $(this).val() + ',';
            });
           var process=$("#dradProcess option:selected").val();
           var prdType = $("#droPrdType option:selected").val();
         
         
           var selType = $("#dradType option:selected").val();
           var reqProcess = document.getElementById("<%=hidReqProcess.ClientID %>").value;
          
       
           PageMethods.GetMain_WebMethod(connection, shipDate, model, modelList, selLine, process, selType, dbName, reqProcess,prdType, onSuccessForMain, onErrorForMain);
           
        
          
       }
       function onErrorForMain(error) {
           if (error != null)
               alert(error.get_message());
           endWaitingCoverDiv();
         
       }
         function onSuccessForMain(receiveData) {

            document.getElementById('tbMain').innerHTML = '';
            document.getElementById('tbMain').innerHTML = receiveData;
       
            $("#tbDetail").html('');

            endWaitingCoverDiv();
            var process = $("#dradProcess option:selected").val();
            $("#btnShow85").hide().attr('value', 'No Pass85');
            //    $("#btnShow85").attr('value', 'No Pass85');
            var reqProcess = document.getElementById("<%=hidReqProcess.ClientID %>").value;
            //  if(reqProcess=='FA' || dbName=='HPDocking')
            if (reqProcess == 'FA' || document.getElementById("<%=hidDockingDB.ClientID %>").value=='Y')
            { return; }
            if (process != 'FA')
            { setTimeout(CheckHaveNo85, 1000); }
        }
       
       function GetDetail()
       {
            ShowWait();
           $(".clicked").removeClass("clicked");
           $($(event.srcElement).parent()).addClass("clicked");
           $(event.srcElement).addClass("clicked");
           var dn=$(event.srcElement).siblings()[0].innerHTML;
           var model=$(event.srcElement).siblings()[1].innerHTML;
           var line=''
           if(selType=='Model')
           {
              line=selLine;
           }
           else
           {
              line=$(event.srcElement).siblings()[2].innerHTML;
           }
  
           var station=$("#mainTable > tbody > tr > th ")[event.srcElement.cellIndex].innerHTML;
           var connection=document.getElementById("<%=hidConnection.ClientID %>").value ;
           PageMethods.GetDetail_WebMethod(connection,dn,model,line,station, onSuccessForDetail, onError);
     
        }
       function onSuccessForDetail(receiveData)
       {
          $("#tbDetail").html('');
        
          $("#tbDetail").html(receiveData);
         HideWait();
 //     $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select Line ' }).multiselectfilter();
          
        }
     
        function GetDNQty()
        {
            // WipTrackingByDN_PAK.TESTAA('x','x','dbName', onSuccessGetQ_callback);
            var lblTotalQty = '#' + ConvertID("lblTotalQtyCount");
            var lblActualQty = '#' + ConvertID("lblActualQtyCount");
            $(lblTotalQty).text('Updating..');
            $(lblActualQty).text('Updating..');
          var inputID='#'+ ConvertID("txtShipDate");
          var shipDate = $(inputID).val();
          var connection=document.getElementById("<%=hidConnection.ClientID %>").value ;
          var dbName = document.getElementById("<%=hidDBName.ClientID %>").value;
          var txtModelID = '#' + ConvertID("txtModel");
          var model = $(txtModelID).val();
          var modelList = document.getElementById("<%=hidModelList.ClientID %>").value;
          var prdType = $("#droPrdType option:selected").val();
          PageMethods.GetDNQty_WebMethod(connection, shipDate, model, modelList, dbName, prdType,onSuccessGetQ_callback, onErrorGetQ_callback);
         
        }
       function onSuccessGetQ_callback(receiveData) {
         var dnQ=receiveData;
          var lblTotalQty='#'+ ConvertID("lblTotalQtyCount");
          var lblActualQty='#'+ ConvertID("lblActualQtyCount");
           $(lblTotalQty).text(dnQ[0]);
           $(lblActualQty).text(dnQ[1]);
       }
       function onErrorGetQ_callback(error) {
           if (error != null)
               alert(error.get_message());
       }
        
        function onSuccess(receiveData) {
         var dnQ=receiveData.value;
          var lblTotalQty='#'+ ConvertID("lblTotalQtyCount");
          var lblActualQty='#'+ ConvertID("lblActualQtyCount");
           $(lblTotalQty).text(receiveData[0]);
           $(lblActualQty).text(receiveData[1]);
        }
        
   
    function onError(error) {
        if (error != null)
            alert(error.get_message());
            HideWait();
    }
  function ShowDescr(descr)
  {
  Tip(descr,SHADOW,true,SHADOWCOLOR, '#dd99aa',
                            FONTSIZE, '12pt',SHADOWWIDTH, 2,OFFSETY,-40,OFFSETX,-25,FADEIN,300);
  
  }
   function SD(descr)
  {
  Tip(descr,SHADOW,true,SHADOWCOLOR, '#dd99aa',
                            FONTSIZE, '12pt',SHADOWWIDTH, 2,OFFSETY,-40,OFFSETX,-25,FADEIN,300);
  
  }
  function UT()
  {
    UnTip();
  }
   function SDS()
  {
     var dn=$(event.srcElement).siblings()[0].innerHTML;
     var model=$(event.srcElement).siblings()[1].innerHTML;
     var line=$(event.srcElement).siblings()[2].innerHTML;
     var station=$("#mainTable > tbody > tr > th ")[event.srcElement.cellIndex].innerHTML;
     var descr="Station:&nbsp;" +station+"<BR>"+"DN&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + dn+"<BR>"+"Model  :&nbsp;"+model;
     Tip(descr,SHADOW,true,BGCOLOR, '#FCDFFF',
                            FONTSIZE, '12pt',TITLE,'Information',SHADOWWIDTH, 2,OFFSETY,-100,OFFSETX,-100,FADEIN,300);
  
  }
  function saveCode(fileID,fileName) { 

    var dlLink="../../CommonAspx/DownloadExcel.aspx?fileID=" +fileID +"&fileName="+fileName;
    var $ifrm = $("<iframe style='display:none' />");
                        $ifrm.attr("src", dlLink);
                        $ifrm.appendTo("body");
                        $ifrm.load(function () {
                        $("body").append(
                        "<div>Failed to download <i>'" + dlLink + "'</i>!");
                     });
} 
   function DownExcel()
        {
        
          ShowWait();
           var inputID='#'+ ConvertID("txtShipDate");
           var shipDate = $(inputID).val();
           var txtModelID='#'+ ConvertID("txtModel");
           var lineID='#'+ ConvertID("lboxPdLine");
           var process='#'+ ConvertID("lboxPdLine");
            //var dbName=document.getElementById("<%=hidDBName.ClientID %>").value ;
           var model=$(txtModelID).val()
           var modelList= document.getElementById("<%=hidModelList.ClientID %>").value 
           var connection=document.getElementById("<%=hidConnection.ClientID %>").value ;
           var pdline = '';
            $(lineID +" option:selected").each(function() {
                pdline = pdline + $(this).val() + ',';
            });
           var process=$("#dradProcess option:selected").val();
           var type=$("#dradType option:selected").val();
           var path=document.getElementById("<%=hidExcelPath.ClientID %>").value ;
           var reqProcess= document.getElementById("<%=hidReqProcess.ClientID %>").value ;
           var prdType = $("#droPrdType option:selected").val();
           PageMethods.DownExcel_WebMethod(connection, shipDate, model, modelList, pdline, process, type, dbName, path, reqProcess, prdType,onSuccessForExcel, onError);
         
        }
         function onSuccessForExcel(receiveData)
         { 
             saveCode(receiveData,"WipTracking");
               HideWait();
         }
         //AjaxPro Function
         function GetMain_AjxPro()
        {
          selLine='';
           beginWaitingCoverDiv();
           var inputID='#'+ ConvertID("txtShipDate");
           var shipDate = $(inputID).val();
           var txtModelID='#'+ ConvertID("txtModel");
           var lineID='#'+ ConvertID("lboxPdLine");
           var process='#'+ ConvertID("lboxPdLine");
            //var dbName=document.getElementById("<%=hidDBName.ClientID %>").value ;
           var model=$(txtModelID).val()
           var modelList= document.getElementById("<%=hidModelList.ClientID %>").value 
           var connection=document.getElementById("<%=hidConnection.ClientID %>").value ;
            var lineID='#'+ ConvertID("lboxPdLine");
       
            $(lineID +" option:selected").each(function() {
                selLine = selLine + $(this).val() + ',';
            });
           var process=$("#dradProcess option:selected").val();
           selType=$("#dradType option:selected").val();
           //PageMethods.GetMain_WebMethod(connection,shipDate,model,modelList,pdline,process,type,dbName, onSuccessForMain, onError);
           var reqProcess= document.getElementById("<%=hidReqProcess.ClientID %>").value ;
           WipTrackingByDN_PAK.GetMain_WebMethod_Ajax(connection,shipDate,model,modelList,selLine,process,selType,dbName,reqProcess,GetMain_AjxPro_callback);
        }

          function GetMain_AjxPro_callback(res)
          {
              if (res == null || res.value == '' || res.value==null) {

                   endWaitingCoverDiv();
                   alert('請重新Query');
                  return;
              }
              if (res.error != null) {

                  endWaitingCoverDiv();
                  alert(res.error);
                  return;
              }
             //$('#tbMain').html(res.value); 
               document.getElementById('tbMain').innerHTML='';
               document.getElementById('tbMain').innerHTML=res.value;
             //  var total=$("#mainTable").attr('value1');
            //   var actuall=$("#mainTable").attr('value2');
            //   var lblTotalQty='#'+ ConvertID("lblTotalQtyCount");
            //   var lblActualQty='#'+ ConvertID("lblActualQtyCount");
           //    $(lblTotalQty).text(total);
           //    $(lblActualQty).text(actuall);
               $("#tbDetail").html('');
               endWaitingCoverDiv();
            //   RegisterTable();
            //  document.getElementById('tbMain').innerHTML=res.value;
              //$("#mainTable").toSuperTable({ width: "98%", height: "300px" });
            }
         //AjaxPro Function
             function RegisterTable()
          {
         $("#mainTable td:empty").css("background", "yellow");

//            $("#mainTable td").click(function(e) {
//            var currentCellText = $(this).text();
//            var LeftCellText = $(this).prev().text();
//            var RightCellText = $(this).next().text();
//            var RowIndex =$(this).parent().parent().children().index($(this).parent());
//            var ColIndex = $(this).parent().children().index($(this));
//            var RowsAbove = RowIndex;
//            var ColName = $(".head").children(':eq(' + ColIndex + ')').text();
//            var station=$("#mainTable > tbody > tr > th ")[e.srcElement.cellIndex].innerHTML;
//            var newSelectRow= $("#mainTable > tbody > tr ")[RowIndex];
//            if(SelRowIdx!=0)
//            {
//      
//            $($("#mainTable > tbody > tr ")[SelRowIdx+1]).removeClass("clicked");

//            }
//            $($("#mainTable > tbody > tr ")[RowIndex+1]).addClass("clicked");
//          
//           SelRowIdx=RowIndex;
       //     });
          
          
         // }

     }
     var s85 = false;
     function Show85_Action() {
         setTimeout(Show85_Action, 100);
     }
     
     function Show85() {
        
//       
//         $("#mainTable").find("tr:gt(0)").each(function() {
//          
//             var aa2 = $(this).find("td:eq(5)").text()
//             if (!s85) {
//                 if (aa2 == '0') {
//                     $(this).hide()
//               
//                 }
//             }
//             else
//             { $(this).show() }
//          

//         });
         if (!s85) {
             $("#btnShow85").attr('value', 'ALL');

             $(function() {
                
                 $("#mainTable tr:gt(0)").filter(function() {
                     return $(this).find("td:eq(5)").text() == '0';
                 }).hide();
             });

          }
          else {
              $("#btnShow85").attr('value', 'No Pass85');
              $("#mainTable tr:gt(0)").show();
          }

         s85 = !s85;
         HideWait();
     }
     function CheckHaveNo85() {
     
        s85 = false;
            var lblTotalQty = '#' + ConvertID("lblTotalQtyCount");
            var lblActualQty = '#' + ConvertID("lblActualQtyCount");
            if ($(lblTotalQty).text() != $(lblActualQty).text())
         {
         $("#btnShow85").show();
         }
          
       
      
//         $("#btnShow85").hide();
//         $("#mainTable").find("tr:gt(0)").each(function() {
//             var _85 = $(this).find("td:eq(5)").text();
//             if (_85 != '')
//             { $("#btnShow85").show(); return; }

//         });
     }
    </script>

</asp:Content>

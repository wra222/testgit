<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="WipTrackingByDN_FA.aspx.cs" Inherits="WipTrackingByDN_FA" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
   
 
<script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
<script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
<script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
<script type="text/javascript" src="../../js/jquery_fixedtable.js"></script>  
<script type="text/javascript" src="../../js/jquery-powertable.js"></script>
<script type="text/javascript" src="../../js/jscal2.js"></script>
<script src="../../js/lang/cn.js"></script>
 <script src="../../js/jquery.superTable.js"></script>
  <script src="../../js/superTables.js"></script>
 
   
<script src="../../js/jquery.dateFormat-1.0.js"></script>    
   
    <script type="text/javascript" src="../../js/assets/prettify.js"></script>
    <script type="text/javascript" src="../../js/jquery.multiselect.js"></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js"></script>     
        

       
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">

    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
    <link rel="stylesheet" type="text/css" href="../../css/assets/style.css" />
    <link rel="stylesheet" type="text/css" href="../../css/assets/prettify.css" /> 
    <link rel="stylesheet" type="text/css"  href="../../css/superTables.css"/>


        


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
    font-size:12px;font-weight: bold; color:Red;
  }


     .style1
     {
         height: 63px;
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
    <asp:ScriptManager ID="ScriptManager1" runat="server"  EnablePageMethods="true">        
  </asp:ScriptManager>
    <fieldset id="grpCarton" style="border: thin solid #000000;">

    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Wip Tracking" CssClass="iMes_label_13pt"></asp:Label></legend> 
      
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
            
                <td width ="10%">
            
                   
          <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
            
                   
                </td>                
                <td align="left" colspan="2" width ="30%">                        
                  
            <imesquery:cmbdbtype ID="CmbDBType" runat="server" />
                  
                   
                  
                </td>   
                <td>  
                
                </td>       
               <td align="left">
                    <asp:Label ID="Label1" runat="server" Text="Process:" CssClass="iMes_label_13pt" ></asp:Label> 
                    <select onchange="SetProcess()" id="dradProcess">
                      <option>ALL</option>
                      <option>FA</option>
                      <option>PAK</option>
                      
                    </select>
              
            
               </td>
               <td>
               <asp:Label ID="lblGrType" runat="server" Text="Type:" CssClass="iMes_label_13pt" ></asp:Label> 
                   <select onchange="SetType()"  id="dradType">
                      <option>Model</option>
                      <option>Model+Line</option>
                                        
                    </select>
           
               </td>
              
               <td  width ="5%">
                     
                    <asp:Label ID="lblLine" runat="server" Text="Line:" 
                       CssClass="iMes_label_13pt"></asp:Label>
                     
               </td>
               <td  width ="30%">
                    <asp:ListBox ID="lboxPdLine" runat="server" CssClass="CheckBoxList" 
                    Height="95%" SelectionMode="Multiple" Width="250px"></asp:ListBox>
               </td>
            </tr>
            <tr>
               <td width ="10%">
                   <asp:Label ID="lblModel" runat="server" Text="Ship Date:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
               <td align= "left" width ="60%" colspan="5">
                   
        
                 
                   <asp:TextBox ID="txtShipDate" runat="server" Width="157px" Height="20px"></asp:TextBox>         
                   &nbsp;
&nbsp;<asp:Label ID="lblInput" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>                                       
                  
                   <asp:TextBox ID="txtModel" runat="server" Width="199px" Height="17px"></asp:TextBox>
                   <input id="BtnBrowse" type="button" value="Input"   onclick="UploadModelList()" /></td>     
              
            
             
                       
                   
                   <td>
                       &nbsp;</td>    
                   <td aligh="left">
                       &nbsp;</td>
            </tr>
                 <tr>
                 <td class="style1">
                   <asp:Label ID="lblFamily" runat="server" Text="Family:" 
                         CssClass="iMes_label_13pt" Visible="False"></asp:Label>
                 </td>
                 <td align="left" class="style1">
              
                
                   <asp:Label ID="lblTotalQty" runat="server" Text="TotalQty:" 
                       CssClass="iMes_label_13pt" Font-Bold="True" ForeColor="Red" 
                         Font-Size="X-Large"></asp:Label> 
                    <asp:Label ID="lblTotalQtyCount" runat="server" CssClass="iMes_label_13pt" 
                         Font-Size="X-Large"></asp:Label> 
                 </td>
                 <td colspan="4" class="style1">    
              
                
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                
                    <asp:Label ID="lblActualQty" runat="server" Text="ActualQty(Pass 85) :" 
                         CssClass="iMes_label_13pt" Font-Size="X-Large"></asp:Label> 
                     <asp:Label ID="lblActualQtyCount" runat="server" CssClass="iMes_label_13pt" 
                         Font-Size="X-Large"></asp:Label> 
           
                     </td>
                  <td colspan="2" align="left" class="style1">
                         <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px; ">Excel</button> 
               <input id="Button1" type="button" value="Query" onclick="GetMain()" /> </td>
                 </tr>
         
        
         </table>
   
<br />
        </fieldset>
       <br />

  <div id="tbMain"  ></div> 
<br />
     <div id="tbDetail"></div>
       
   
      
      
   
        
       <asp:HiddenField ID="hidModelType" runat="server" Value="1" />
       <asp:HiddenField ID="hidModelList" runat="server"  />
    <asp:HiddenField ID="hidConnection" runat="server" Value='' />
        <asp:HiddenField ID="hidDBName" runat="server" Value='' />
        <asp:HiddenField ID="hidProcess" runat="server" Value="ALL" />
        <asp:HiddenField ID="hidType" runat="server" Value="Model" />
          
    
     
         
  
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
     
     window.onload = function()
     {
       load();
       EndRequestHandler();
       GetDNQty();
       dbName=document.getElementById("<%=hidDBName.ClientID %>").value ;
     
     };
    
     </script>
    <script type="text/javascript">
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
//          $(".radProcess> input").change(function() {
//          rbvalue = $("input[type=radio]:checked").val();
//          var processID='#'+ ConvertID("hidProcess");
//          $(processID).val(rbvalue) ;
//          });
//          
//           $(".radType> input").change(function() {
//          rbvalue = $("input[type=radio]:checked").val();
//          var typeID='#'+ ConvertID("hidType");
//          $(typeID).val(rbvalue) ;
//          });
        
  
     

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
        function UploadModelList() {

        
            var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
            var saveasUrl = "../../UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelList.ClientID %>").value;
            var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
            if (dlgReturn) {

                dlgReturn = dlgReturn.replace(/\r\n/g, ",");
                document.getElementById("<%=hidModelList.ClientID %>").value = RemoveBlank(dlgReturn);

                //   document.getElementById("<%=hidModelList.ClientID %>").value = dlgReturn;


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
        //GetDetail_WebMethod(string Connection, string station, string line, string model,string dn)
        function GetMain()
        {
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
           var pdline = '';
            $(lineID +" option:selected").each(function() {
                pdline = pdline + $(this).val() + ',';
            });
           var process=$("#dradProcess option:selected").val();
           var type = $("#dradType option:selected").val();


           var action = "GETMAIN_WEBMETHOD";
           PageMethods.GetMain_WebMethod(connection,shipDate,model,modelList,pdline,process,type,dbName, onSuccessForMain, onError);
//           $.ajax({
//           url: 'server/WipTrackingByDN_FA_server.aspx',
//               type: "POST",
//               data: {
//                   action: action,
//                   connection : connection,                   
//                   shipDate: shipDate,
//                   model: model,
//                   modelList: modelList,
//                   line: pdline,
//                   process: process, 
//                   type: type
//               },
//               dataType: "html",
//               success: function(response) {
//                    $("#tbMain").html(response);
//                    $("#tbDetail").html('');
//                   endWaitingCoverDiv();
//               },
//               error: function(response) {
//                   alert(response);
//                   endWaitingCoverDiv();
//               }
//           });
           //PageMethods.GetMain_WebMethod(connection,shipDate,model,modelList,pdline,process,type,dbName, onSuccessForMain, onError);
         
        }
        function GetDetail(dn,model,line,station)
        {
          ShowWait();
          var connection=document.getElementById("<%=hidConnection.ClientID %>").value ;
          PageMethods.GetDetail_WebMethod(connection,station,line,model,dn, onSuccessForDetail, onError);
        
        
        }
        
       function GetDetail()
       {
           ShowWait();
           $(".clicked").removeClass("clicked");
            $($(event.srcElement).parent()).addClass("clicked");
            $(event.srcElement).addClass("clicked");
           var dn=$(event.srcElement).siblings()[0].innerHTML;
            var model=$(event.srcElement).siblings()[1].innerHTML;
            var line=$(event.srcElement).siblings()[2].innerHTML;
            var station=$("#mainTable > tbody > tr > th ")[event.srcElement.cellIndex].innerHTML;
           var connection=document.getElementById("<%=hidConnection.ClientID %>").value ;
           PageMethods.GetDetail_WebMethod(connection,dn,model,line,station, onSuccessForDetail, onError);
     
        }
       function onSuccessForDetail(receiveData)
       {
          $("#tbDetail").html(receiveData);
         HideWait();
 //     $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select Line ' }).multiselectfilter();
          
        }
       function onSuccessForMain(receiveData)
       {
          //$("#tbMain").html(receiveData);
            document.getElementById('tbMain').innerHTML=receiveData;
          var total=$("#mainTable").attr('value1');
          var actuall=$("#mainTable").attr('value2');
          var lblTotalQty='#'+ ConvertID("lblTotalQtyCount");
          var lblActualQty='#'+ ConvertID("lblActualQtyCount");
           $(lblTotalQty).text(total);
           $(lblActualQty).text(actuall);
          $("#tbDetail").html('');
         endWaitingCoverDiv();
         //  $("#mainTable").toSuperTable({ width: "98%", height: "300px", fixedCols: 2 }); 
//lblTotalQtyCount
//lblActualQty
       }
        function GetDNQty()
        {
          var inputID='#'+ ConvertID("txtShipDate");
          var shipDate = $(inputID).val();
          var connection=document.getElementById("<%=hidConnection.ClientID %>").value ;
          var dbName=document.getElementById("<%=hidDBName.ClientID %>").value ;
          PageMethods.GetDNQty_WebMethod(connection,shipDate,dbName,'', onSuccess, onError);
         
        }
     
        function onSuccess(receiveData) {
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
   function ShowDetailDescr()
  {
     var dn=$(event.srcElement).siblings()[0].innerHTML;
     var model=$(event.srcElement).siblings()[1].innerHTML;
     var line=$(event.srcElement).siblings()[2].innerHTML;
     var station=$("#mainTable > tbody > tr > th ")[event.srcElement.cellIndex].innerHTML;
     var descr="Station:&nbsp;" +station+"<BR>"+"DN&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + dn+"<BR>"+"Model  :&nbsp;"+model;
     Tip(descr,SHADOW,true,BGCOLOR, '#FCDFFF',
                            FONTSIZE, '12pt',TITLE,'Information',SHADOWWIDTH, 2,OFFSETY,-100,OFFSETX,-100,FADEIN,300);
  
  }
  
  
    </script>
</asp:Content>


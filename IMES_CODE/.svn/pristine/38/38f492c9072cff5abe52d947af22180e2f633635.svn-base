<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="DashBoard.aspx.cs" Inherits="DashBoard" EnableViewState="false" %>

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


    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
 <style type="text/css">
       
        .title
        {
            font-size: 18px;
            font-weight: bold;
            color:White;
        }
        .titleData
        {
            font-size: 22px;
            font-weight: bold;
            color: #57FEFF;
        }
        .titleSmallData
        {
            font-size: 16px;
             color: #FFF8C6;
        }
        .blackMode
        {
         
         }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
      
    </asp:ScriptManager>
 
      
      <div style=" visibility:hidden">
      <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
       <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
      </div>
        <table id="titleTable"  width="98%"style='background-color:#000000'>
        <tr align="center">
        <td class="title" >Total Plan</td>
        <td id="totalPlan" class="titleData">0 
          </td>
        <td colspan="2" align="center">
            <asp:Label ID="Label1" runat="server" ForeColor="Yellow" Text="Dashboard" 
                Font-Size="XX-Large"></asp:Label>
        </td>
         <td  align="right" class="titleSmallData" id="updateTime">
   
           </td>
             <td align="right">
               <input id="btnChangeMode" type="button" value="歷史資料" onclick="ChangeMode()"  style="display:none"/>
              <input   id="btnSetQty" type="button" value="Set Line Qty" onclick="btnSetQty_Click()" />
             </td>
        </tr>
        <tr>
        <td class="title" style="width:16%">Total Input</td>
        <td class="titleData"> 
            <asp:Label ID="labTotalInput" runat="server" Text="0"></asp:Label></td>
       <td class="title" style="width:16%">FA Output</td>
        <td class="titleData" style="width:16%">
        <asp:Label ID="labFAOutput" runat="server"  Text="0" 
             ></asp:Label>
        </td>
        <td class="title" style="width:16%" >Pack Output</td>
        <td class="titleData" style="width:16%">
         <asp:Label ID="labPackOut" runat="server"  Text="0" 
               ></asp:Label>
        </td>
    
        </tr>           
        </table>
      
   <table id="mainTable2" class="iMes_grid_TableGvExt"  style="width: 98%;border-width:0px;height:1px;table-layout:fixed;background-color: #000000;">
                                     
<tr class="iMes_grid_HeaderRowGvExt ">
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">
     Qty</th>
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">Line</th>
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">BeginTime</th>
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">EndTime</th>
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">Input</th>
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">Fun Test</th>
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">Image D/L</th><th class="t5" style=" color: #FFFF00; font-family: Verdana">2PP&amp; Run In</th><th class="t5" style=" color: #FFFF00; font-family: Verdana">FA Output</th>
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">Pack Out</th>
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">NG Fun Test</th>
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">NG Image D/L</th>
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">NG 2PP&amp; Run In</th>
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">FPY</th>
<th class="iMes_grid_HeaderRowGvExt " style=" color: #FFFF00; font-family: Verdana">FPF</th></tr>
</table>
   <div id="tbMain">
     
    </div>


    <asp:HiddenField ID="hidConnection" runat="server" Value="1" />
    <asp:HiddenField ID="hidDbName" runat="server" Value="" />
    
    <asp:HiddenField ID="hidImgPath" runat="server" Value="1" />

  

    <script type="text/javascript">
        var dbName;
        var lineArr = new Array();
        window.onload = function() {
            GetMain();
            // setTimeout('GetMain( )', 10000)
        };
    
    </script>

    <script type="text/javascript">
       function ChangeMode()
       {
         $('#btnChangeMode').val("Resume");
         $('#mainTable').css("background-color", "#5E767E");
         $('#titleTable').css("background-color", "#5E767E");
         
       
       }
        function btnSetQty_Click() {
            //  ShowData()

            var dbName = document.getElementById("<%=hidDbName.ClientID %>").value;
         
            var dlgFeature = "dialogHeight:570px;dialogWidth:500px;center:yes;status:yes;help:no;";
            var dlgReturn = window.showModalDialog("SetLineQty.aspx?dbName=" + dbName , window, dlgFeature);
        }
        function SetValue() {
            var v1 = $("#mainTable").attr('value1');
            var v2 = $("#mainTable").attr('value2');
            var v3 = $("#mainTable").attr('value3');
            var v4 = $("#mainTable").attr('value4');

            var idxI = '#' + "<%=labTotalInput.ClientID %>";
            var idxF = '#' + "<%=labFAOutput.ClientID %>";
            var idxP = '#' + "<%=labPackOut.ClientID %>";


            $(idxI).text(v1);
            $(idxF).text(v2);

            $(idxP).text(v3);
            $("#totalPlan").html(v4); 
        }

        function ChangeRowColor() {
            $(".clicked").removeClass("clicked");
            $($(event.srcElement).parent()).addClass("clicked");
        }
        var SelRowIdx;



        var selLine = '';
        var selType = '';
        function ShowD(line, obj) {
            var _line = line + "_Data";
            var lineIconClass = line + '_Icon';
            line = "." + line + "_Data";
          
            //$('[class^=main]') 
         //   if ($(line).css("display") == "none") {
            if ($('[class^='+_line+']').css("display") == "none") {

                $('[class^=' + _line + ']').css('display', 'block');
                var src = $(obj).attr("src").replace("plus", "minus");
                $(obj).attr("src", src);
                //$('[class^=' + lineIconClass + ']').attr("src").replace("minus", "plus");
                $('[class^=' + lineIconClass + ']').attr("src", function(n, v) { return v.replace("minus", "plus"); })
                //  debugger;
                //$('[class^=A1D] [src*=minus]')
                lineArr.push(_line);
            }
            else {
                $('[class^=' + _line + ']').css('display', 'none');
                $('[class^=Model]').css('display', 'none');
               
                var src = $(obj).attr("src").replace("minus", "plus");
                $(obj).attr("src", src);
                RemoveLineFromArr(_line);
            }
        }
        function RemoveLineFromArr(line) {
            for (var i = 0; i < lineArr.length; i++) {
                if (line == lineArr[i]) {
                    lineArr.splice(i, 1); return;
                }
            }
        }
        function GetMain() {

            var connection = document.getElementById("<%=hidConnection.ClientID %>").value;
            var imgPath = document.getElementById("<%=hidImgPath.ClientID %>").value;



            PageMethods.GetMain_WebMethod(connection, imgPath,lineArr, onSuccessForMain, onErrorForMain);



        }
        function onErrorForMain(error) {
            if (error != null)
                alert(error.get_message());
            endWaitingCoverDiv();

        }
        function onSuccessForMain(receiveData) {

            document.getElementById('tbMain').innerHTML = '';
            document.getElementById('tbMain').innerHTML = receiveData;

            SetValue();
            var now = new Date();
            var t = now.format("HH:mm:ss");
            $("#updateTime").html("Last update time: " + t); //titleSmallData
            setTimeout('GetMain( )', 300000)
        }
        var selRowObj;
        function ShowModel(obj, line, fTime, eTime) {
            ShowWait();
            var modelClass = "Model"+ line + "_Data" + fTime.replace(":", "") ;
            var modelRow = $('[class^=' + modelClass + ']');
            var src;
            if (modelRow.length>0) {

                if (modelRow.css("display") == "none") {
                    modelRow.css('display', 'block');
                    src = $(obj).attr("src").replace("plus", "minus");
                    $(obj).attr("src", src);
                 }
                else {
                    modelRow.css('display', 'none');
                    src = $(obj).attr("src").replace("minus", "plus");
                    $(obj).attr("src", src);
                }
                HideWait();
             
            
            
                return;


            }
            src = $(obj).attr("src").replace("plus", "minus");
            $(obj).attr("src", src);
            
            
            var className = line + "_Data" + fTime.replace(":", "");
            selRowObj = $("." + className);
           
            var connection = document.getElementById("<%=hidConnection.ClientID %>").value;
            PageMethods.GetModelDetail_WebMethod(connection, line, fTime, eTime, onSuccessForGetModel, onErrorForMain);
          
           
        }
        function onSuccessForGetModel(receiveData) {
            var newRow;
          
        
            for (i = 0; i < receiveData.length; i++) {
             
                newRow = $(receiveData[i]);
                $(selRowObj).after(newRow);
            }
            HideWait();
            
        //    newRow = $('<tr class="DNTitle"><td>DN</td><td>Model</td><td>Qty</td><td></td></tr>');
         //   $(rowOBJ.parentNode).after(newRow);
        
        }
    </script>

</asp:Content>

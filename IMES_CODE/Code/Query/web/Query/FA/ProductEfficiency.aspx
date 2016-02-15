
<%@ Page Title="ProductEfficiency" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ProductEfficiency.aspx.cs" Inherits="Query_FA_ProductEfficiency" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 
   <script type="text/javascript" src="../../js/jquery-1.7.1.js "></script>    
    <script type="text/javascript" src="../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../../js/jquery.dataTables.min.js "></script>    
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js "></script>     
    <script src="../../js/jscal2.js "></script>
    <script src="../../js/lang/cn.js "></script>    

    <script type="text/javascript" src="../../js/jquery.multiselect.js "></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js "></script>     
    <script type="text/javascript" src="../../js/wz_tooltip.js "></script>
        
    
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.dataTables.css" />
<style type="text/css">
        
    tr.clicked
    {
        background-color: white; 
    }
    .querycell
    {
        background-color: yellow;
     
    }
    .querycell:hover , .querycell.clicked
    {
        background-color: Blue;                       
    }
    .lesson1, .lesson4
    {
        background-color: #FFFF00;                
    }
    .lesson2, .lesson5
    {
        background-color: #FFD5B4;
    }
    .lesson3, .lesson6
    {
        background-color: #C5D9F1;
    }
    
    .lesson_sum
    {
        background-color:#EEEEEE;
    }
    
    .viewed td
    {        
        border-style: solid;
        border-width: 2px;
        border-color: Black;                            
    }  
        
    .lb_lesson, .lb_lesson_all
    {
        margin: 0 2px 0 2px;
        cursor: pointer;
        text-decoration: underline;
    }
    
    .lb_lesson.clicked, .lb_lesson_all.clicked
    {
        color: blue;
    }
</style>   
                      

<script type="text/javascript">
 </script>
 
 <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>        
             <script language="javascript" type="text/javascript">
                </script>
    </ContentTemplate>    
  </asp:UpdatePanel>  
   <body>                  
<center>
    <div style="position: fixed; top: 10px; left: auto; right: 0; width: 100%; text-align: center; z-index: 9999">        
        <div style="float:right ;background-color: #FFF4C8; width: 40%">
            <asp:Label ID="lb_lesson_all" runat="server" Class="lb_lesson_all" Text="[All]" Title="All"></asp:Label>
            <asp:Label ID="lb_lesson1" runat="server" Class="lb_lesson" Text="[Lesson1]" Title="lesson1"></asp:Label>
            <asp:Label ID="lb_lesson2" runat="server" Class="lb_lesson" Text="[Lesson2]" Title="lesson2"></asp:Label>
            <asp:Label ID="lb_lesson3" runat="server" Class="lb_lesson" Text="[Lesson3]" Title="lesson3"></asp:Label>
            <asp:Label ID="lb_lesson4" runat="server" Class="lb_lesson" Text="[Lesson4]" Title="lesson4"></asp:Label>
            <asp:Label ID="lb_lesson5" runat="server" Class="lb_lesson" Text="[Lesson5]" Title="lesson5"></asp:Label>
            <asp:Label ID="lb_lesson6" runat="server" Class="lb_lesson" Text="[Lesson6]" Title="lesson6"></asp:Label>
        </div>        
    </div>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="ProductEfficiency" CssClass="iMes_label_13pt"></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="5%">
                      <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td width="35%">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>                
                <td width ="5%">
                    <asp:Label ID="lblPdLine" runat="server" Text="PdLine:" CssClass="iMes_label_13pt"></asp:Label>
                <td width="35%">                                       
                    <asp:ListBox ID="lboxPdLine" runat="server" SelectionMode="Multiple" Height="95%" 
                                    Width="200px" CssClass="CheckBoxList">
                    </asp:ListBox>                                                                    
               </td>
                <td rowspan="3" width="20%" >
                    
                    
                    <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="beginWaitingCoverDiv();" 
                                    style="width: 100px; display: none">Query</button>
                    <input type="button" value="Query" onclick = "query();" style="width: 100px" />
                    
                    <br />
                    <button id="btnExport"  runat="server" onserverclick="btnExport_Click"
                                    style="width: 100px; display: none;">Export</button>
                    <input type="button" value="Export" onclick = "queryexcel();" style="width: 100px" />


                </td>
            </tr>
            <tr>
               <td>
                   <asp:Label ID="lblDate" runat="server" Text="Date:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
               <td>
                    <input type="radio" name="rbPeriod" class="rbPeriod" id="rbPeriodByDay" value="GET_DAY" checked="checked" />
                    <asp:TextBox id="txtFromDate" runat="server" Width="150px" Height="20px"></asp:TextBox>                      
                    <button id="btnFromDate" type="button" style="width: 20px">...</button>
                    <br />
                    <input type="radio" name="rbPeriod" class="rbPeriod" id="rbPeriodByPeriod" value="GET_PERIOD" />
                    <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
                    <asp:TextBox id="txtPeriodFromDate" class="txtPeriod" runat="server" Width="120px" Height="20px"></asp:TextBox>                            
                    <button id="btnPeriodFromDate" type="button" style="width: 20px">...</button>
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtPeriodToDate" class="txtPeriod" runat="server" Width="120px" Height="20px"></asp:TextBox>
                    <button id="btnPeriodToDate" type="button" style="width: 20px">...</button>                         
               </td>            
                 <td width ="5%">
                    <asp:Label ID="lblStation" runat="server" Text="Station:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="35%">
                    <asp:ListBox ID="lboxStation" runat="server" SelectionMode="Multiple" Width="300px" class="CheckBoxList" >
                    </asp:ListBox>                 
                </td>
            </tr>
            <tr>
               <td >
                    <asp:Label ID="lblFamily" runat="server" Text="Family:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
               <td >                   
                   <asp:DropDownList ID="ddlFamily" runat="server" Width="300px" 
                       onselectedindexchanged="ddlFamily_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
               </td>            
               <td >
                    <asp:Label ID="lblModel" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
               <td >
                   <!--<asp:TextBox ID="txtModel" runat="server" Width="300px" isvalid="false" ></asp:TextBox>-->
                   <input type="text" id="txtModel" style="width: 300px" maxlength="14" class="iMes_textbox_input_Yellow" />
                   <input id="BtnBrowse" type="button" value="Browse"  onclick="UploadModelList()" />               
                   <asp:HiddenField ID="hfModel" runat="server" />
               </td>
            </tr>
         </table>
</fieldset> 


    <div id="pnl_query" style="width: 98%"></div>
    <div id="pnl_excel"></div>
    
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>        
       <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="450px" 
            Width="98%" GvExtWidth="98%" Height="1px" ShowFooter="True" onrowdatabound="gvResult_RowDataBound" >            
        </iMES:GridViewExt>

       <iMES:GridViewExt ID="gvDetail" runat="server" AutoGenerateColumns="true" GvExtHeight="450px" 
            Width="98%" GvExtWidth="98%" Height="1px" ShowFooter="True" onrowdatabound="gvDetail_RowDataBound" >            
        </iMES:GridViewExt>
      
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
    </Triggers>
    </asp:UpdatePanel>  

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
            <ContentTemplate>
                <asp:HiddenField ID="hidModelList" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlFamily" EventName="SelectedIndexChanged" />                
            </Triggers>
        </asp:UpdatePanel>
    </center>
<script type="text/javascript">
    function query() {
        beginWaitingCoverDiv();
        var station = '';
        if ($("#<%=lboxStation.ClientID %> option:selected").length > 0) {
            $("#<%=lboxStation.ClientID %> option:selected").each(function() {
                station = station + $(this).val() + ',';
            });
        } else {
            $("#<%=lboxStation.ClientID %> option").each(function() {
                station = station + $(this).val() + ',';
            });
        }
        
        var pdline = '';
        $("#ctl00_iMESContent_lboxPdLine option:selected").each(function() {
            pdline = pdline + $(this).val() + ',';
        });
        
        var txtFromdata = $("#<%=txtFromDate.ClientID %>").val();
        var txtPeriodFromDate = $("#<%=txtPeriodFromDate.ClientID %>").val();
        var txtPeriodToDate = $("#<%=txtPeriodToDate.ClientID %>").val();
        var family = $("#<%=ddlFamily.ClientID %>").val();
        var model = $("#<%=hfModel.ClientID %>").val();
        var dbtype = $("#ctl00_iMESContent_CmbDBType_ddlDBType option:selected").val();        
        var dbname = $("#ctl00_iMESContent_CmbDBType_ddlDB option:selected").val();

        var action = $(".rbPeriod:checked").val()+"_QUERY";
        
        $.ajax({
        url: 'server/productefficiency.aspx',
            type: "POST",
            data: {
                action: action,
                txtFromDate: txtFromdata,
                txtPeriodFromDate: txtPeriodFromDate,
                txtPeriodToDate: txtPeriodToDate,
                station: station,
                family: family,
                model: model,
                pdline : pdline,
                dbtype : dbtype ,
                dbname : dbname},
            dataType: "html",
            success: function(response) {
                $("#pnl_query").html(response);
                endWaitingCoverDiv();
            },
            error: function(response){
                alert(response);
                endWaitingCoverDiv();
             }
        });
        
        }

        function queryexcel() {
            //beginWaitingCoverDiv();
            var station = '';
            if ($("#<%=lboxStation.ClientID %> option:selected").length > 0) {
                $("#<%=lboxStation.ClientID %> option:selected").each(function() {
                    station = station + $(this).val() + ',';
                });
            } else {
                  $("#<%=lboxStation.ClientID %> option").each(function() {
                    station = station + $(this).val() + ',';
                });
            }
            
            var pdline = '';
            $("#ctl00_iMESContent_lboxPdLine option:selected").each(function() {
                pdline = pdline + $(this).val() + ',';
            });

            var txtFromdate = $("#<%=txtFromDate.ClientID %>").val();
            var dbtype = $("#ctl00_iMESContent_CmbDBType_ddlDBType option:selected").val();
            var dbname = $("#ctl00_iMESContent_CmbDBType_ddlDB option:selected").val();

            var family = $("#<%=ddlFamily.ClientID %>").val();
            var model = $("#<%=hfModel.ClientID %>").val();
            
            var action = $(".rbPeriod:checked").val() + "_EXCEL";

            var key = ["action", "txtFromDate", "station", "family", "model", "pdline", "dbtype", "dbname"];
            var data = { "action": action , 
                         "txtFromDate": txtFromdate ,
                         "station": station ,
                         "family": family ,
                         "model": model ,
                         "pdline": pdline ,
                         "dbtype": dbtype ,
                         "dbname": dbname
                        };
            var url = "server/productefficiency.aspx";
            $("#pnl_excel").html("<iframe width='0' height='0' frameborder='0' src='" + getposturl(key, data, url) + "'></iframe>");
            
            //this.location.href = getposturl(key, data, url);
            
        }

    function getposturl(parakey, paravalue, url) {
        url += "?";
        for (i = 0; i < parakey.length; i++) {
            url += parakey[i] + "=" + paravalue[parakey[i]] + "&";
        }
        return url; 
    }

    function EndRequestHandler(sender, args) {

        $('.CheckBoxList').multiselect({ selectedList: 1, maxselect: 6, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
        $("#<%=gvResult.ClientID%> tr:nth-child(n+2) ").click(function() { rowclick(); });

        Calendar.setup({
            trigger: "btnFromDate",
            inputField: "<%=txtFromDate.ClientID%>",
            onFocus: function() { $("#rbPeriodByDay").attr("checked", "true"); }, 
            onSelect: function() { this.updateCalFields, $("#rbPeriodByDay").attr("checked", "true"); },
            dateFormat: "%Y-%m-%d"
        });

        Calendar.setup({
            trigger: "btnPeriodFromDate",
            inputField: "<%=txtPeriodFromDate.ClientID%>",
            onSelect: function() { updateCalendarFields, $("#rbPeriodByPeriod").attr("checked", "true"); },
            onTimeChange: function() { updateCalendarFields, $("#rbPeriodByPeriod").attr("checked", "true"); },
            showTime: 24,
            dateFormat: "%Y-%m-%d %H:%M",
            minuteStep: 30
        });
        
        Calendar.setup({
        trigger: "btnPeriodToDate",
        inputField: "<%=txtPeriodToDate.ClientID%>",
        onSelect: function() { updateCalendarFields, $("#rbPeriodByPeriod").attr("checked", "true"); },
        onTimeChange: function() { updateCalendarFields, $("#rbPeriodByPeriod").attr("checked", "true"); },
        showTime: 24,
            dateFormat: "%Y-%m-%d %H:%M",
            minuteStep: 30
        });

        $("span.lb_lesson").click(function() {
            $(".lb_lesson").removeClass("clicked");
            $(".lb_lesson_all").removeClass("clicked");
            $(this).addClass("clicked");

            var lessoncss = "." + this.title;
            $("#gvResult > tbody > tr").removeClass("viewed").filter(lessoncss).addClass("viewed");
            //$("#ctl00_iMESContent_gvResult > tbody > tr > td").removeClass("view").parent("tr").filter(lessoncss).children("td").addClass("view");
            $("#gvDetail > tbody > tr:nth-child(n+2)").hide().filter(lessoncss).show();

        });

        $("#<%=txtFromDate.ClientID%>").focus(function() {
            $("#rbPeriodByDay").attr("checked", "true");
        });

        $("#<%=txtPeriodFromDate.ClientID %>").focus(function() {
            $("#rbPeriodByPeriod").attr("checked", "true");
        });
        $("#<%=txtPeriodToDate.ClientID %>").focus(function() {
            $("#rbPeriodByPeriod").attr("checked", "true");
        });            
        
        $("span.lb_lesson_all").click(function() {
            $(".lb_lesson").removeClass("clicked");
            $(this).addClass("clicked");
            $("#gvResult > tbody > tr").removeClass("viewed");
            $("#gvDetail > tbody > tr:nth-child(n+2)").show();
        });

    };
    
    function updateCalFields(cal) {
        var date = cal.selection.get();
        if (date) {
            debugger;
            date = Calendar.intToDate(date);
            cal.inputField.value = Calendar.printDate(date, "%Y-%m-%d")
        }
    };


    function rowclick() {
        $(".clicked").removeClass("clicked");
        $(event.srcElement).addClass("clicked");
        $(event.srcElement.parentNode).addClass("clicked");
    }

    function UploadModelList() {
        var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelList.ClientID %>").value;
        var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
        if (dlgReturn) {

            dlgReturn = dlgReturn.replace(/\r\n/g, ",");
            document.getElementById("<%=hfModel.ClientID %>").value = RemoveBlank(dlgReturn);

            //   document.getElementById("<%=hidModelList.ClientID %>").value = dlgReturn;


        }
        else {
            if (dlgReturn == "")
            { document.getElementById("<%=hfModel.ClientID %>").value = ""; }
            return;
        }

    }
    function RemoveBlank(modelList) {
        var arr = modelList.split(",");
        var model = "";
        if (modelList != "") {
            for (var m in arr) {
                if (arr[m] != "") {
                    model = model + arr[m] + ",";
                }


                // content += key + ' : ' + myarr[key] + '<br />';

            }
            model = model.substring(0, model.length - 1)
        }

        return model;
    }

    $("#txtModel").change(function() {
        var v = $(this).val();
        if (v.length > 10 || v.length == 0) {
            $("#<%=hfModel.ClientID %>").val(v);
        }
    });
    function buildHtmldt(dataTable) {
        var headers = [];
        var rows = [];
        headers.push("<tr>");
        //Column標題
        for (var column in dataTable[0])
            headers.push("<th>" + column + "</th>");
        headers.push("</tr>");
        //row資料
        for (var row in dataTable) {
            rows.push("<tr>");
            for (var column in dataTable[row]) {
                rows.push("<td>");
                rows.push(dataTable[row][column]);
                rows.push("</td>");
            }
            rows.push("</tr>");
        }
        var top = "<table border='1' class='gridview'>";
        var bottom = "</table>";
        return top + headers.join("") + rows.join("") + bottom;
    }

    window.onload = function() {
        EndRequestHandler();
    };


</script>
</asp:Content>

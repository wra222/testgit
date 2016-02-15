<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="QueryDtPallet.aspx.cs" Inherits="Query_PAK_QueryDtPallet"  %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<%@ Register src="../../CommonControl/CmbStation.ascx" tagname="CmbStation" tagprefix="uc1" %>

<%@ Register assembly="myControls" namespace="myControls" tagprefix="iMES" %>
<%@ Register src="../../CommonControl/CmbStation.ascx" tagname="CmbStation" tagprefix="iMES" %>
<%@ Register src="../../CommonControl/CmbDBType.ascx" tagname="CmbDBType" tagprefix="iMESQuery" %>
<%@ Register src="../../CommonControl/CmbPdLine.ascx" tagname="CmbPdLine" tagprefix="iMES" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 

    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../../js/jscal2.js"></script>
    <script type="text/javascript" src="../../js/lang/cn.js"></script>
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
     <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>   
     <style type="text/css">
    
    .querycell:hover
    {
        background-color: #8AF2E7;
        cursor: hand;
    }
  
     tr.clicked
    {
        background-color: white; 
    }
    .clicked
    {
        background-color: #8AF2E7;
    }
  .row1
    {
  	  background-color: #C3FDB8;
  	  }
    .row2
    {
      background-color: #64E986;
     }
</style>    
  <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Query DT Pallet" 
            CssClass="iMes_label_13pt" ></asp:Label></legend> 
         <table border="0" width="100%" >                    
            <tr style=" vertical-align:bottom "  >
             <td width ="5%" align="right">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt" 
                        meta:resourcekey="lblDBResource1"></asp:Label></td>                
                <td width ="25%" align="left">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>       
                <td align="left"  colspan="3">
                     <asp:CheckBoxList ID="chkState" runat="server" RepeatDirection="Horizontal" 
                         Font-Size="12pt">
                        <asp:ListItem Selected="True" Value="DT">已刷入DT</asp:ListItem>
                        <asp:ListItem Selected="True" Value="RW">退回產線</asp:ListItem>
                        <asp:ListItem Selected="True" Value="IN">已刷入W/H</asp:ListItem>
                        <asp:ListItem Selected="True" Value="OT">已刷出車管系統</asp:ListItem>
                        <asp:ListItem Selected="True" Value="RD">退回產線后再刷DT</asp:ListItem>
                        <asp:ListItem Selected="True" Value="">未刷過DT</asp:ListItem>
                    </asp:CheckBoxList>
                                        
                </td>       
                
           
            </tr>
            <tr>
              <td width ="10%" align="right">
                   <asp:RadioButton ID="radPallet" runat="server" Checked="True" GroupName="radGr" />
                   <asp:Label ID="lblModel" runat="server" Text="Pallet:" CssClass="iMes_label_13pt" ></asp:Label>
                </td>
                 <td width ="25%" align="left"> 
                
                    <asp:TextBox ID="txtPallet" runat="server" Height="19px" Width="200px"></asp:TextBox>
                
            <input id="BtnBrowse" type="button" value="Browse"  onclick="UploadModelList()" /><input type="file" name="inputDNFile" id="inputDNFile" style="display: none" onchange="changeFile(this)" />
                </td>
            
       
                <td width ="40%" align="left">
                    <asp:RadioButton ID="radShipdate" runat="server" GroupName="radGr" />
                 <asp:Label ID="lblDate" runat="server" Text="Ship Date:" CssClass="iMes_label_13pt"></asp:Label>
                 &nbsp;<asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>
                   <asp:TextBox ID="txtFromDate" runat="server" Width="150px" Height="20px"></asp:TextBox>                                                         
                                               
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" Height="20px"></asp:TextBox>
                </td> 
             
                     <td width ="5%">
                  
                           <input id="btnExcel" type="button" value="Excel" onclick="DownExcel()"  style=" display:none" /> 
                    </td>
                  <td width ="5%">
                    <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" OnClientClick="return  CompareDate()"/>
                  </td>
            </tr>
     
         </table>
            &nbsp;</fieldset> 

   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
      
       <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="300px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" 
          OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False" onrowdatabound="gvResult_RowDataBound">
        </iMES:GridViewExt>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
  <br />
  <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
     <ContentTemplate>      
       <iMES:GridViewExt ID="gvDetail" runat="server" AutoGenerateColumns="true" GvExtHeight="40%" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" onrowdatabound="gvDetail_RowDataBound" 
            >            
           <HeaderStyle Font-Size="Smaller" Width="50px" />
        </iMES:GridViewExt>                        
    </ContentTemplate>
    <Triggers>                 
         <asp:AsyncPostBackTrigger ControlID="btnQueryDetail" EventName="ServerClick" />
         
    </Triggers>  
  </asp:UpdatePanel>
   <asp:HiddenField ID="hidPalletList" runat="server" />
     <asp:HiddenField ID="hidPalletNo" runat="server" />
     <asp:HiddenField ID="hidExcelPath" runat="server" />

      <button id="btnQueryDetail" runat="server"  onserverclick="QueryDetailClick" style="display: none">QueryDetail</button>   
</center>

<script type="text/javascript">    //<![CDATA[
    Calendar.setup({
    inputField: ConvertID("txtFromDate"),
    trigger: ConvertID("txtFromDate"),
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d ",
        minuteStep: 1
    });
    Calendar.setup({
        inputField: ConvertID("txtToDate"),
        trigger: ConvertID("txtToDate"),
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d ",
        minuteStep: 1
    });
    function CompareDate() {
        //beginWaitingCoverDiv();
        var obj = document.getElementById(ConvertID("radShipdate"));
        //alert(obj.checked);
        //return false;
        var Date1 = document.getElementById(ConvertID("txtFromDate")).value.replace("-", "/");
        var Date2 = document.getElementById(ConvertID("txtToDate")).value.replace("-", "/");
        var fromDate = new Date(Date1);
        var toDate = new Date(Date2);
        if (obj.checked) {
            if (fromDate > toDate)
            { ShowMessage('日期範圍錯誤'); return false; }
            else
            { beginWaitingCoverDiv();  }
        }
        else {
            var p1 = document.getElementById(ConvertID("txtPallet")).value;
            var p2 = document.getElementById(ConvertID("hidPalletList")).value;
            if (trim(p1) == "" && trim(p2) == "")
            { ShowMessage('Please input pallet No.'); return false; }
            else
            { beginWaitingCoverDiv(); }
          //  return true;
        }

        return true;
    }
    function UploadModelList() {
     
        var dlgFeature = "dialogHeight:650px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidPalletList.ClientID %>").value;
        var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
        if (dlgReturn) {

            dlgReturn = dlgReturn.replace(/\r\n/g, ",");
            document.getElementById("<%=hidPalletList.ClientID %>").value = RemoveBlank(dlgReturn);

            //   document.getElementById("<%=hidPalletList.ClientID %>").value = dlgReturn;


        }
        else {
            if (dlgReturn == "")
            { document.getElementById("<%=hidPalletList.ClientID %>").value = ""; }
            return;
        
         }

     }
     function trim(stringToTrim) {
         return stringToTrim.replace(/^\s+|\s+$/g, "");
     }
    function RemoveBlank(modelList) {
        var arr = modelList.split(",");
        var model = "";
        if (modelList != "") {
            for (var m in arr) {
                if (trim(arr[m]) != "") {
                    model = model + trim(arr[m]) + ",";
                }


                // content += key + ' : ' + myarr[key] + '<br />';

            }
            model = model.substring(0, model.length - 1)
        }

        return model;
    }
    function SelectDetail(pallet) {
        beginWaitingCoverDiv();
        document.getElementById("<%=hidPalletNo.ClientID%>").value = pallet;
      
        $(".clicked").removeClass("clicked");
        $($(event.srcElement).parent()).addClass("clicked");
        $(event.srcElement).addClass("clicked");
        document.getElementById("<%=btnQueryDetail.ClientID%>").click();

    }


    function DownExcel() {
        ShowWait();

        var path = document.getElementById("<%=hidExcelPath.ClientID %>").value;
        PageMethods.DownExcel_WebMethod(path, onSuccessForExcel, onError);

    }
    function onSuccessForExcel(receiveData) {
        saveCode(receiveData, "QueryDtPallet");
        HideWait();
    }
    function saveCode(fileID, fileName) {

        var dlLink = "../../CommonAspx/DownloadExcel.aspx?fileID=" + fileID + "&fileName=" + fileName;
        var $ifrm = $("<iframe style='display:none' />");
        $ifrm.attr("src", dlLink);
        $ifrm.appendTo("body");
        $ifrm.load(function() {
            $("body").append(
                                "<div>Failed to download <i>'" + dlLink + "'</i>!");
        });
    }
    function onError(error) {
        if (error != null)
            alert(error.get_message());
        HideWait();
    }
    //]]></script>


</asp:Content>


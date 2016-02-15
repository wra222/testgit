<%@ Page Title="未结单报表" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="UnShipSnList.aspx.cs" Inherits="Query_PAK_UnShipSnList"  %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>



<%@ Register src="../../CommonControl/CmbDBType.ascx" tagname="CmbDBType" tagprefix="iMESQuery" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 

    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
<script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
<script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
<script src="../../js/jscal2.js"></script>
<script src="../../js/lang/cn.js"></script>
    
<script src="../../js/jquery.dateFormat-1.0.js"></script>    
    <script type="text/javascript" src="../../js/assets/prettify.js"></script>
    <script type="text/javascript" src="../../js/jquery.dataTables.js"></script>
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../../css/assets/style.css" />
    <link rel="stylesheet" type="text/css" href="../../css/assets/prettify.css" /> 
  <asp:ScriptManager ID="ScriptManager1" runat="server"  EnablePageMethods="true">        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="UnShip SN List" 
            CssClass="iMes_label_13pt" ></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma;">                    
            <tr>
                <td width ="10%" align="right">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label></td>                
                <td width ="30%" align="left">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>        
                <td width ="60%" align="left" colspan="5">
             
                </td>       
                
           
            </tr>
            <tr>
              <td width ="10%" align="right">
                   <asp:Label ID="lblModel" runat="server" Text="Model:" CssClass="iMes_label_13pt" ></asp:Label>
                </td>
                 <td width ="15%" align="left"> 
                
                    <asp:TextBox ID="txtModel" runat="server" Height="19px" Width="200px"></asp:TextBox>
                
            <input id="BtnBrowse" type="button" value="Browse"  onclick="UploadModelList()" /><input type="file" name="inputDNFile" id="inputDNFile" style="display: none" onchange="changeFile(this)" />
                </td>
            
       
                <td width ="40%" align="left">
                 <asp:Label ID="lblShipDate" runat="server" Text="Ship Date:" CssClass="iMes_label_13pt"></asp:Label>
              
                   <asp:TextBox ID="txtShipDate" runat="server" Width="150px" Height="20px"></asp:TextBox>                                                         
              
                    <input id="chx1" type="checkbox" value="1" />只查詢未出貨</td> 
                <td >
                    &nbsp;</td>
                     <td width ="5%">
                          
                            <input id="Button2" type="button" value="Excel" onclick="DownExcel()" />
                    </td>
                  <td width ="5%">
                    <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" OnClientClick="ClientClick()" />
                  </td>
            </tr>
     
         </table>
            </fieldset> 
            <br />

   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
      
       <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="400px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" 
            meta:resourcekey="gvResultResource1" OnGvExtRowClick="" OnGvExtRowDblClick=""  onrowdatabound="gvResult_RowDataBound"
            SetTemplateValueEnable="False">
        </iMES:GridViewExt>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
   <asp:HiddenField ID="hidModelList" runat="server" />
    <asp:HiddenField ID="hidExcelPath" runat="server" />
    <asp:HiddenField ID="hidConnection" runat="server" />
    <asp:HiddenField ID="hidDBName" runat="server" />
    <asp:HiddenField ID="hidIsCheck" runat="server" />
</center>

  
<script type="text/javascript">    //
    Calendar.setup({
    inputField: "<%=txtShipDate.ClientID%>",
    trigger: "<%=txtShipDate.ClientID%>", 
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });
    var path = document.getElementById("<%=hidExcelPath.ClientID %>").value;
    
    function UploadModelList() {
     
        var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelList.ClientID %>").value;
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
        var model = "";
        if (modelList != "") {
            for (var m in arr) {
                if (arr[m] != "") {
                    model = model + arr[m] + ",";
                }

           }
            model = model.substring(0, model.length - 1)
        }

        return model;
    }
    function ClientClick() {
        if ($('#chx1').is(':checked'))
        { document.getElementById("<%=hidIsCheck.ClientID %>").value = "1"; }
        else
        { document.getElementById("<%=hidIsCheck.ClientID %>").value = "0"; }
        beginWaitingCoverDiv();
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
    function DownExcel() {
        var isCheck = false;

        if ($('#chx1').is(':checked'))
        {isCheck = true; }
     

        ShowWait();
        var inputID = '#' + ConvertID("txtShipDate");
        var shipDate = $(inputID).val();
        var txtModelID = '#' + ConvertID("txtModel");
        
        var model = $(txtModelID).val()
        var modelList = document.getElementById("<%=hidModelList.ClientID %>").value
        var connection = document.getElementById("<%=hidConnection.ClientID %>").value;
     
        var path = document.getElementById("<%=hidExcelPath.ClientID %>").value;
      
   
        PageMethods.DownExcel_WebMethod(connection, shipDate, model, modelList,path,isCheck,onSuccessForExcel, onError);

    }
    function onSuccessForExcel(receiveData) {
        saveCode(receiveData, "UnShipSnList");
        HideWait();
    }
    function onError(error) {
        if (error != null)
            alert(error.get_message());
        HideWait();
    }
    
    
    </script>


</asp:Content>


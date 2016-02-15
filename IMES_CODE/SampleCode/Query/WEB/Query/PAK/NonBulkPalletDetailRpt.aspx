<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="NonBulkPalletDetailRpt.aspx.cs" Inherits="Query_PAK_NonBulkPalletDetailRpt" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 
    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
         <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
    <script type="text/javascript" src="../../js/jscal2.js"></script>
    <script type="text/javascript" src="../../js/lang/en.js"></script>
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    
  <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">        
  </asp:ScriptManager>


 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="結板報表(非散裝)-Detail" 
            CssClass="iMes_label_13pt" ></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="5%" align="right">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt" 
                        meta:resourcekey="lblDBResource1"></asp:Label></td>                
                <td width ="20%" align="left">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>        
            
                
                <td width ="15%" align="left">
                 <asp:Label ID="lblShipDate" runat="server" Text="Ship Date:" 
                        CssClass="iMes_label_13pt" ></asp:Label>
                        <asp:TextBox ID="txtShipDate" runat="server" Width="105px" Height="20px" ></asp:TextBox>
                        ~<asp:TextBox ID="txtToDate" runat="server" Width="105px" Height="20px" ></asp:TextBox>
                </td>       
              <td width ="15%" align="left">
                                 <iMESQuery:ChxLstProductType ID="ChxLstProductType1" runat="server"   />
                    </td> 
            
        
            
              
                <td width ="5%">
            
                <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" OnClientClick=" return CheckInput()"/>
                </td>
            </tr>
      <asp:Button ID="btnShowDetail" runat="server" onclick="btnShowDetail_Click" 
                        Text="Button" style="display: none"  />
         </table>
</fieldset> 

<input id="btnMainExcel" type="button" value="Excel" onclick="DownExcel('Main')"  style=" display:none" /> 
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
           
       <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="200px" 
            Width="98%" GvExtWidth="98%" Height="1px" 
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

  
  <input id="btnDetailExcel" type="button" value="Excel" onclick="DownExcel('Sub')"  style=" display:none" /> 
  <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
  <iMES:GridViewExt ID="grvDetail" runat="server" GvExtHeight="200px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" 
            meta:resourcekey="gvResultResource1" OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False">
        </iMES:GridViewExt>
         </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnShowDetail" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>

  
<asp:HiddenField ID="hidExcelPath" runat="server" />
<asp:HiddenField ID="hidSelNo" runat="server" />
<asp:HiddenField ID="hidCol" runat="server" />
<asp:HiddenField ID="hidModel" runat="server" />

<script type="text/javascript">    //<![CDATA[
    Calendar.setup({
        inputField: ConvertID("txtShipDate"),
        trigger: ConvertID("txtShipDate"),
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });
    Calendar.setup({
    inputField: ConvertID("txtToDate"),
    trigger: ConvertID("txtToDate"),
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });
    function CheckInput() {
            beginWaitingCoverDiv();
       
    
    }
    function trim(stringToTrim) {
        return stringToTrim.replace(/^\s+|\s+$/g, "");
    }
    function ShowDetail(col, no,model) {

        document.getElementById(ConvertID("hidCol")).value = col;
        document.getElementById(ConvertID("hidSelNo")).value = no;
        document.getElementById(ConvertID("hidModel")).value = model;
    
        beginWaitingCoverDiv();
        document.getElementById(ConvertID("btnShowDetail")).click();
 
    }
    var itemID;
    function DownExcel(item)
        {
          ShowWait();
        
           var path=document.getElementById("<%=hidExcelPath.ClientID %>").value ;
           PageMethods.DownExcel_WebMethod(item,path, onSuccessForExcel, onError);
         
        }
         function onSuccessForExcel(receiveData)
         { 
              saveCode(receiveData,"NonBulkPalletDetail");
               HideWait();
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
        function onError(error) {
        if (error != null)
            alert(error.get_message());
            HideWait();
    }
   </script>

 



</asp:Content>


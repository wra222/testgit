<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="StatusByDeliveryNo.aspx.cs" Inherits="Query_PAK_StatusByDeliveryNo" EnableEventValidation="false"  %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 
    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
    
    

    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
    <script type="text/javascript" src="../../js/jscal2.js"></script>
    <script type="text/javascript" src="../../js/lang/en.js"></script>
    
    <script type="text/javascript" src="../../js/jquery.dateFormat-1.0.js"></script>    
    <script type="text/javascript" src="../../js/assets/prettify.js"></script>

        
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">

  

    

  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Query Status By Delivery No" 
            CssClass="iMes_label_13pt" ></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
        
            <tr>
              <td width ="5%" align="right">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                 <td width ="25%" align="left"> 
                
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                
                </td>
            
        
                <td width ="35%" align="left">
                  <asp:RadioButton ID="rad1" runat="server" Checked="True" GroupName="radGr" Text="Delivery No:" />
                 <asp:TextBox ID="txtDeliveryNo" runat="server" Height="19px" Width="200px"></asp:TextBox>
                 <input id="BtnBrowse" type="button" value="Input DN"  onclick="UploadDNList()" />
                 </td> 
                
               <td width ="35%" align="left">
                      <asp:RadioButton ID="rad2" runat="server" GroupName="radGr" Text="Ship Date" />
                     <asp:TextBox ID="txtShipDate" runat="server" Width="150px" Height="20px" 
                          Enabled="False"></asp:TextBox>              
                 </td>
                   
           </tr>
            <tr>
        
            <td colspan="4" align="right">
                  <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="Excel" style=" display:none"/>
              <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" 
                        onclick="btnQuery_Click"  OnClientClick="return Query()"  Height="21px"/>      
                
             
             
           
                      
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
            meta:resourcekey="gvResultResource1" OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False" onrowdatabound="gvResult_RowDataBound">
        </iMES:GridViewExt>
  </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
      
    </Triggers>
    </asp:UpdatePanel>
   <asp:HiddenField ID="hidDNList" runat="server" />
    
</center>

<script type="text/javascript">    //<![CDATA[
 $("input[type=radio]").change(function() {
 
 
   var rbvalue = $("input[type=radio]:checked").val(); //BtnBrowse

   if(rbvalue!='rad2')
   {
      $("#<%=txtShipDate.ClientID%>").attr("disabled","disabled");
   
      $("#<%=txtDeliveryNo.ClientID%>").removeAttr("disabled");
      $("#<%=txtDeliveryNo.ClientID%>").css("background-color","#E0FCC9");
      $("#BtnBrowse").removeAttr("disabled");
     
   }
   else
   { $("#<%=txtDeliveryNo.ClientID%>").attr("disabled","disabled");
      $("#<%=txtShipDate.ClientID%>").removeAttr("disabled"); 
       $("#BtnBrowse").attr("disabled","disabled");
      $("#<%=txtDeliveryNo.ClientID%>").css("background-color","#C0C0C0");
   }
 
});
    Calendar.setup({
    inputField: ConvertID("txtShipDate"),
    trigger: ConvertID("txtShipDate"),
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d ",
        minuteStep: 1
    });
     function Query() {
      var rbvalue = $("input[type=radio]:checked").val(); //BtnBrowse
      if(rbvalue=='rad1')
      {
          var inputID='#'+ ConvertID("txtDeliveryNo");
          var inputValue = $(inputID).val();
          var hidDN=document.getElementById("<%=hidDNList.ClientID %>").value;
          if(inputValue.trim()=="" && hidDN=="")
          {
             alert('Pleae input DN');return false;
          }
   
      }
      beginWaitingCoverDiv();
    }
//    function ChcekDN() {
//        
//        var dn = document.getElementById(ConvertID("txtDeliveryNo")).value;
////        if (trim(dn) == "")
////        { ShowMessage('½Ð¿é¤J Delivery No!'); return false; }
////        else
//        beginWaitingCoverDiv(); 
//    }
    function trim(stringToTrim) {
        return stringToTrim.replace(/^\s+|\s+$/g, "");
    }
    function UploadDNList() {
 
        var dlgFeature = "dialogHeight:600px;dialogWidth:320px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "UploadDNList.aspx?DNList=" + document.getElementById("<%=hidDNList.ClientID %>").value;
        var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
        if (dlgReturn) {

            dlgReturn = dlgReturn.replace(/\r\n/g, ",");
            document.getElementById("<%=hidDNList.ClientID %>").value = RemoveBlank(dlgReturn);

            


        }
        else {
            if (dlgReturn == "")
            {document.getElementById("<%=hidDNList.ClientID %>").value = ""; }
         return; }

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

 




</script>


</asp:Content>


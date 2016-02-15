<%@ Page Title="Goods Readiness Report" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="GoodsReadinessReport.aspx.cs" Inherits="GoodsReadinessReport" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 

    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
    <script src="../../js/jscal2.js"></script>
    <script src="../../js/lang/en.js"></script>
    
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
     
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>


<fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Goods Readiness Report" CssClass="iMes_label_13pt"></asp:Label></legend> 
    
       <table border="0" width="100%" style="font-family: Tahoma">  
     <tr>
       <td width ="35%" align="left" >
           <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"  ></asp:Label> 
                 <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
       </td>
     <td  width ="10%" align="right">
        <asp:RadioButtonList ID="radItem" runat="server" RepeatDirection="Horizontal" Width="100px">
               <asp:ListItem Selected="True">DN</asp:ListItem>
               <asp:ListItem>BOL</asp:ListItem>
           </asp:RadioButtonList>
     </td>
       <td width ="55%" align="left">
   
          <asp:TextBox ID="txtDN" runat="server" Width="160px"></asp:TextBox>
             <input id="BtnBrowse" type="button" value="Input" onclick="UploadDNList()" />
                <asp:Label ID="lblShipDate" runat="server" Text="Ship Date:" 
                        CssClass="iMes_label_13pt" ></asp:Label>
                   <asp:TextBox ID="txtShipDate" runat="server" Width="105px" Height="20px" ></asp:TextBox> 
       
       </td>
    
     </tr>
    <tr>
    <td align="right" colspan="3">  <asp:Button ID="btnQuery" runat="server" onclick="btnQuery_Click"  Text="Query" OnClientClick="beginWaitingCoverDiv();return CheckInput()" />
           <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" style=" display:none"      Text="Excel" /></td>
    </tr>
    </table>
                                 
                                      
                
               
      
     &nbsp;
&nbsp;

   
 
</fieldset> 
        
          <br />
   
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
      
       <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="400px" 
            Width="100%" GvExtWidth="100%" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" 
            OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False">
        </iMES:GridViewExt>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
   
      <asp:HiddenField ID="hidDNList" runat="server" />     
    



<script type="text/javascript">    //<![CDATA[
    Calendar.setup({
        inputField: ConvertID("txtShipDate"),
        trigger: ConvertID("txtShipDate"),
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });
    function CheckInput() {
        
        var input = document.getElementById("<%=txtDN.ClientID %>").value;
        var inputList = document.getElementById("<%=hidDNList.ClientID %>").value;
        if (input.trim() == '' && inputList.trim() == '')
        {endWaitingCoverDiv(); alert('Please input DN or BOL'); return false; }
        else
        {  return true; }
    
    } 

    function UploadDNList() {

        var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "../../UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidDNList.ClientID %>").value;
        var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
        if (dlgReturn) {
            dlgReturn = dlgReturn.replace(/\r\n/g, ",");
            document.getElementById("<%=hidDNList.ClientID %>").value = RemoveBlank(dlgReturn);
        }
        else {
            if (dlgReturn == "")
            { document.getElementById("<%=hidDNList.ClientID %>").value = ""; }
            return;

        }

    }
    function RemoveBlank(modelList) {
        var arr = modelList.split(",");
        var ic = arr.length;
        var model = "";
        if (modelList != "") {

            for (var i = 0; i < arr.length; i++) {
                if (arr[i].trim() != "") {
                    model = model + arr[i].trim() + ",";
                }
            }

            model = model.substring(0, model.length - 1)
        }

        return model;
    }
  </script>


                  
</asp:Content>

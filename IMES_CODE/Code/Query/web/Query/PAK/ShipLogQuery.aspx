<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ShipLogQuery.aspx.cs" Inherits="Query_PAK_ShipLogQuery"  %>
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
    
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Pak Buffer Query" 
            CssClass="iMes_label_13pt" ></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="10%" align="right">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label></td>                
                <td width ="30%" align="left">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>        
                <td colspan="4">  
                  <iMESQuery:ChxLstProductType ID="ChxLstProductType1" runat="server"   />
                </td>       
            
           
            </tr>
            <tr>
              <td width ="10%" align="right">
                   <asp:Label ID="lblModel" runat="server" Text="Model:" CssClass="iMes_label_13pt" ></asp:Label>
                </td>
                 <td width ="15%" align="left"> 
                
                    <asp:TextBox ID="txtModel" runat="server" Height="19px" Width="200px"></asp:TextBox>
                    &nbsp;<input id="BtnBrowse" type="button" value="Browse"  onclick="UploadModelList()" />
                
                </td>
            
             <td width ="10%" align="right">
                 <asp:Label ID="lblDate" runat="server" Text="Date:" CssClass="iMes_label_13pt"></asp:Label>
                </td>       
                <td width ="40%" align="left">
                   <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>
                   <asp:TextBox ID="txtFromDate" runat="server" Width="150px" Height="20px"></asp:TextBox>                                                         
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" Height="20px"></asp:TextBox>
                </td> 
                
                     <td width ="5%">
                       <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="Excel" style=" display:none"/>
                     </td>   
                   <td width ="5%">
                     <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" OnClientClick="return  CompareDate()"/>      
                  </td>
            </tr>
    
         </table>
</fieldset> 

   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
      
       <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="400px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" 
            meta:resourcekey="gvResultResource1" OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False">
        </iMES:GridViewExt>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
     <asp:HiddenField ID="hidModelList" runat="server" />
</center>

<script type="text/javascript">    //<![CDATA[
    Calendar.setup({
        inputField: ConvertID("txtFromDate"),
        trigger: ConvertID("txtFromDate"),
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
    function CompareDate() {


        var Date1 = document.getElementById(ConvertID("txtFromDate")).value.replace("-", "/");
        var Date2 = document.getElementById(ConvertID("txtToDate")).value.replace("-", "/");
        var fromDate = new Date(Date1);
        var toDate = new Date(Date2);
        if (fromDate > toDate)
        { ShowMessage('日期範圍錯誤'); return false; }
        else
        { beginWaitingCoverDiv(); return true; }

    }
    //]]>
    function UploadModelList() {
        var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "../../Query/PAK/UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelList.ClientID %>").value;
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


                // content += key + ' : ' + myarr[key] + '<br />';

            }
            model = model.substring(0, model.length - 1)
        }
        
        return model;
    }
    
    </script>


</asp:Content>


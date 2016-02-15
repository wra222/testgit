<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="BsamGetByCartonSN.aspx.cs" Inherits="Query_PAK_BsamGetByCartonSN"  %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<%@ Register src="../../CommonControl/CmbStation.ascx" tagname="CmbStation" tagprefix="uc1" %>

<%@ Register assembly="myControls" namespace="myControls" tagprefix="iMES" %>
<%@ Register src="../../CommonControl/CmbStation.ascx" tagname="CmbStation" tagprefix="iMES" %>
<%@ Register src="../../CommonControl/CmbDBType.ascx" tagname="CmbDBType" tagprefix="iMESQuery" %>
<%@ Register src="../../CommonControl/CmbPdLine.ascx" tagname="CmbPdLine" tagprefix="iMES" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 
 <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Query Bsam by Carton SN" 
            CssClass="iMes_label_13pt" ></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma;">                    
            <tr>
                <td width ="10%" align="right">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label></td>                
                <td width ="30%" align="left">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>        
                <td width ="35%" align="left" >
                     
                    <asp:Label ID="lblCartonSN" runat="server" Text="Carton SN:" CssClass="iMes_label_13pt" ></asp:Label>
                
                    <asp:TextBox ID="txtCartonSN" runat="server" Height="19px" Width="200px"></asp:TextBox>
                                   <input id="BtnBrowse" type="button" value="input CartonSN"  onclick="UploadCartonSNList()" />
        
                                          
                </td>       
                <td width ="25%" align="right">
 
                    <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" OnClientClick=" beginWaitingCoverDiv();" />
                   <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="Excel" style=" display:none"/>
                </td>
           
            </tr>
           
         </table>
            &nbsp;</fieldset> 
<br />
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
    
    function UploadCartonSNList() {
     
        var dlgFeature = "dialogHeight:650px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "UploadCartonSNList.aspx?CartonSN=" + document.getElementById("<%=hidModelList.ClientID %>").value;
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
 </script>


</asp:Content>


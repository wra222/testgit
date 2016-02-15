<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="NonBulkPalletSummaryRpt.aspx.cs" Inherits="Query_PAK_NonBulkPalletSummaryRpt"  ValidateRequest="false"%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server"  > 
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


 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="結板報表(非散裝)-Summary" 
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
                        <asp:TextBox ID="txtShipDate" runat="server" Width="105px" Height="20px" ></asp:TextBox>~
                        <asp:TextBox ID="txtToDate" runat="server" Width="105px" Height="20px" ></asp:TextBox>
                </td>       
              <td width ="15%" align="left">
                             <iMESQuery:ChxLstProductType ID="ChxLstProductType1" runat="server"   /></td> 
            
        
            
              
                <td width ="5%">
            
                <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" OnClientClick=" return CheckInput()"/>
                </td>
            </tr>
      <asp:Button ID="btnShowDetail" runat="server" onclick="btnShowDetail_Click" 
                        Text="Button" style="display: none"  />
         </table>
</fieldset> 
 <table border="0" width="100%"   >                    
        <tr>   
            <td width="50%">
            <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="Excel" style=" display:none"/>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" RenderMode="Inline">
    <ContentTemplate>
           
       <iMES:GridViewExt ID="gvResult" runat="server" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px"  GvExtHeight="400px"
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" 
            meta:resourcekey="gvResultResource1" OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False" onrowdatabound="gvResult_RowDataBound">
        </iMES:GridViewExt>
        <asp:HiddenField ID="hidGrv1" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
            </td>
            <td width="50%">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always" RenderMode="Inline">
    <ContentTemplate>
           
       <iMES:GridViewExt ID="grvDetail" runat="server" 
            Width="98%" GvExtWidth="98%" GvExtHeight="400px" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" 
            meta:resourcekey="gvResultResource1" OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False" >
        </iMES:GridViewExt>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
            
            </td>
            </tr>
            
               
         
            
            </table>

  <br />
  
  <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" RenderMode="Inline">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnShowDetail" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>

<asp:HiddenField ID="hidSelNo" runat="server" />
<asp:HiddenField ID="hidCol" runat="server" />
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
    function ShowDetail(col, no) {

        document.getElementById(ConvertID("hidCol")).value = col;
        document.getElementById(ConvertID("hidSelNo")).value = no;
        beginWaitingCoverDiv();
        document.getElementById(ConvertID("btnShowDetail")).click();
        
//        if (row != null) {
//            var dn = row.cells[0].innerText.trim();
//            document.getElementById(ConvertID("hidDN")).value = dn;
//            beginWaitingCoverDiv();
//            document.getElementById(ConvertID("btnShowDetail")).click();
//        }
      
        // alert(LocID); 
    }
   </script>

<style type="text/css">
  
.rowclient
{
    cursor : pointer;
}

</style>
 



</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="PackingDailyReport.aspx.cs" Inherits="Query_PAK_PackingDailyReport"  EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto"%>
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
       <asp:Label ID="lblTitle" runat="server" Text="Packing daily report" 
            CssClass="iMes_label_13pt" meta:resourcekey="lblTitleResource1"></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="10%" align="right">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt" 
                        meta:resourcekey="lblDBResource1"></asp:Label></td>                
                <td width ="35%" align="left">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>        
                <td width ="10%" align="right">
                 <asp:Label ID="lblDate" runat="server" Text="Ship Date:" CssClass="iMes_label_13pt" 
                        meta:resourcekey="lblRDResource1"></asp:Label>
                </td>       
                <td width ="35%" align="left">
                   <asp:TextBox ID="txtFromDate" runat="server" Width="105px" Height="20px" 
                        meta:resourcekey="txtFromDateResource1"></asp:TextBox>                                                         
                                               
                </td> 
                <td width ="5%" >
                    <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="Excel" style=" display:none"/>
                </td>
                <td width ="5%">
                   <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" OnClientClick="CheckInput()"/>
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
            SetTemplateValueEnable="False" onrowdatabound="gvResult_RowDataBound">
        </iMES:GridViewExt>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
</center>

<script type="text/javascript">    //
    Calendar.setup({
    inputField: "<%=txtFromDate.ClientID%>",
    trigger: "<%=txtFromDate.ClientID%>",
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });

    //]]>
    function CheckInput() {
        beginWaitingCoverDiv(); return true;
    }
    </script>


</asp:Content>


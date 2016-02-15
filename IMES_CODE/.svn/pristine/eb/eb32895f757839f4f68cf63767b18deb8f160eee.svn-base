<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ProductProgress.aspx.cs" Inherits="Query_PAK_ProductProgress"  %>
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
<script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
<script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
<script src="../../js/jscal2.js"></script>
<script src="../../js/lang/cn.js"></script>
    
<script src="../../js/jquery.dateFormat-1.0.js"></script>    
    <script type="text/javascript" src="../../js/assets/prettify.js"></script>
    <script type="text/javascript" src="../../js/jquery.multiselect.js"></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js"></script>     
    <script type="text/javascript" src="../../js/wz_tooltip.js"></script>
    <script type="text/javascript" src="../../js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="../../js/FixedColumns.js"></script>
    <script type="text/javascript" src="../../js/FixedHeader.js"></script>
    
        
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">

    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
    <link rel="stylesheet" type="text/css" href="../../css/assets/style.css" />
    <link rel="stylesheet" type="text/css" href="../../css/assets/prettify.css" /> 
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="生產進度查詢" 
            CssClass="iMes_label_13pt" ></asp:Label></legend> 
         <table border="0" width="100%" >                    
            <tr>
                <td width ="10%" align="right">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label></td>                
                <td width ="30%" align="left">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>        
                <td width ="40%" align="left" >
                       &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblDate" runat="server" Text="Date:" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>
                                          
                       <asp:TextBox ID="txtFromDate" runat="server" Width="150px" Height="20px" 
                       ></asp:TextBox>                                                         
                                               
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" Height="20px"></asp:TextBox>
                                          
                </td>       
                <td width ="10%">
                            <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="Excel" style=" display:none"/>
                            <asp:DropDownList ID="dro" runat="server">
                                <asp:ListItem>a</asp:ListItem>
                                <asp:ListItem>b</asp:ListItem>
                            </asp:DropDownList>
                    </td>
                  <td width ="10%">
                    <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" OnClientClick="  beginWaitingCoverDiv()"/>
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

<script type="text/javascript">    //<![CDATA[ <%=txtToDate.ClientID%>
    Calendar.setup({
    inputField: "<%=txtFromDate.ClientID%>",
    trigger: "<%=txtFromDate.ClientID%>", 
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });
    Calendar.setup({
        inputField: "<%=txtToDate.ClientID%>",
        trigger: "<%=txtToDate.ClientID%>",
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });
  
  
   $(window).load(function() {
           var a='#'+"<%=dro.ClientID%>";
           $(a).change(function() {
        alert('Handler for .change() called.');
});
           
           
        });
  </script>


</asp:Content>


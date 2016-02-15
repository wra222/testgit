<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="Query87Product.aspx.cs" Inherits="Query_PAK_Query87Product"  EnableEventValidation="false"  uiculture="auto"%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<%@ Register assembly="myControls" namespace="myControls" tagprefix="iMES" %>
<%@ Register src="../../CommonControl/ChxLstProductType.ascx" tagname="ChxLstProductType" tagprefix="iMESQuery" %>
<%@ Register src="../../CommonControl/CmbDBType.ascx" tagname="CmbDBType" tagprefix="iMESQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 
   
     <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
<script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
 <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
<script src="../../js/jscal2.js"></script>
<script src="../../js/lang/cn.js"></script>


<link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
<link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
<link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
  <link href="../../js/superTables.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
<script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
<script src="../../js/jscal2.js"></script>
<script src="../../js/lang/en.js"></script>
   <script src="../../js/superTables.js"></script>
   <script src="../../js/jquery.superTable.js"></script>
   
<script src="../../js/jquery.dateFormat-1.0.js"></script>    
   
    <script type="text/javascript" src="../../js/assets/prettify.js"></script>
    <script type="text/javascript" src="../../js/jquery.multiselect.js"></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js"></script>     
        
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">

    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
        

    <link rel="stylesheet" type="text/css" href="../../css/assets/style.css" />
    <link rel="stylesheet" type="text/css" href="../../css/assets/prettify.css" />  
    <script type="text/javascript" src="../../js/wz_tooltip.js"></script>
    <script type="text/javascript">


        function load() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        }

        function EndRequestHandler(sender, args) {

            $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select Line ' }).multiselectfilter();


        }
    </script>
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Query 87 Product" 
            CssClass="iMes_label_13pt" meta:resourcekey="lblTitleResource1"></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="5%" align="right">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt" 
                        meta:resourcekey="lblDBResource1"></asp:Label></td>                
                <td width ="25%" align="left">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server"   />
                </td>       
                  <td width ="15%" align="left">                        
                    <iMESQuery:ChxLstProductType ID="ChxLstProductType1" runat="server"   />
                    
                </td>     
                <td width ="7%" align="right">
                 <asp:Label ID="lblDate" runat="server" Text="Ship Date:" CssClass="iMes_label_13pt"  ></asp:Label>
                </td>       
                <td width ="10%" align="left">
                   <asp:TextBox ID="txtShipDate" runat="server" Width="105px" Height="20px" ></asp:TextBox>                                                         
                                               
                </td> 
                <td width ="25%" align="left">
                    <asp:Label ID="Label1" runat="server" Text="Line:" CssClass="iMes_label_13pt"  ></asp:Label>
                <asp:ListBox ID="lboxPdLine" runat="server"  SelectionMode="Multiple" Height="95%" 
                            Width="250px" CssClass="CheckBoxList"></asp:ListBox>
                </td>
                <td width ="22%" >
                <asp:RadioButtonList ID="radPallet" runat="server" 
                        RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True">ALL</asp:ListItem>
                        <asp:ListItem Value="NA">散裝</asp:ListItem>
                        <asp:ListItem Value="NoNA">非散裝</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
               
            </tr>
    <tr>
    <td colspan="3" align="left" >
     
    </td>
    <td colspan="5" align="right">
         
    <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="Excel" style=" display:none"/>
    <asp:Button ID="btnMail" runat="server" onclick="btnMail_Click" Text="Mail" style=" display:none" OnClientClick="beginWaitingCoverDiv()"/>
              <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" OnClientClick="CheckInput()"/>
    
         <input   id="btnSetMail" type="button" value="設定收件人" onclick="btnSetMail_Click()" />

     &nbsp;</td>
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
        <asp:AsyncPostBackTrigger ControlID="btnMail" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
       <input type="hidden" id="hidDbName" runat="server" />
        <input type="hidden" id="hidRptName" runat="server" />
            <input type="hidden" id="hidConnection" runat="server" />
</center>

<script type="text/javascript">    //<![CDATA[
    Calendar.setup({
    inputField: ConvertID("txtShipDate"),
    trigger: ConvertID("txtShipDate"),
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });

    //]]>
    function CheckInput() {
        beginWaitingCoverDiv(); return true;
    }
    function btnSetMail_Click() {
        //  ShowData()
         
        dbName = document.getElementById("<%=hidDbName.ClientID %>").value;
        reportName = "87ReportMailList";
        var dlgFeature = "dialogHeight:570px;dialogWidth:500px;center:yes;status:yes;help:no;";
        var dlgReturn = window.showModalDialog("SetMailList.aspx?dbName=" + dbName + "&reportName=" + reportName , window, dlgFeature);
    }
      //   EndRequestHandler();
       
      $(window).load(function() {
        EndRequestHandler();
     

        });
   
    </script>


</asp:Content>


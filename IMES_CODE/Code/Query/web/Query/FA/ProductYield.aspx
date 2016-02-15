﻿<%@ Page Title="ProductDistribute" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ProductYield.aspx.cs" Inherits="Query_FA_ProductYield" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 
 <script type="text/javascript" src="../../js/jquery-1.7.1.js "></script>    
    <script type="text/javascript" src="../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js "></script>     
    <script src="../../js/jscal2.js "></script>
    <script src="../../js/lang/cn.js "></script>    

    <script type="text/javascript" src="../../js/jquery.multiselect.js "></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js "></script>     
    <script type="text/javascript" src="../../js/wz_tooltip.js "></script>
        
    
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
       

<script type="text/javascript" language="javascript">

  
</script>
      
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
   <body>                  
<center>
<fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Product_Distribute" CssClass="iMes_label_13pt"></asp:Label></legend> 
        <table border="0" width="100%" style="font-family: Tahoma">                    
    <tr>
        <td width ="10%">
              <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
        </td>                
        <td width="40%">                        
            <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
        </td>
        <td width="10%">
            <asp:Label ID="lblPdLine" runat="server" Text="PdLine:" CssClass="iMes_label_13pt"></asp:Label>         
        </td>                
        <td width="40%">        
             
                    <asp:ListBox ID="lboxPdLine" runat="server" SelectionMode="Multiple" Height="95%" 
                            Width="300px" CssClass="CheckBoxList">
                    </asp:ListBox>
                
        </td>
    </tr>
    <tr>
       <td width ="10%">
           <asp:Label ID="lblRD" runat="server" Text="Period:" CssClass="iMes_label_13pt"></asp:Label>
       </td>
       <td width ="40%">
            <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
            <asp:TextBox id="txtFromDate" runat="server" Width="140px" Height="20px"></asp:TextBox>                            
            <button id="btnFromDate" type="button" style="width: 20px">...</button>
            <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
            <asp:TextBox ID="txtToDate" runat="server" Width="140px" Height="20px"></asp:TextBox>
            <button id="btnToDate" type="button" style="width: 20px">...</button>          
       </td>            
       <td width ="10%">
            <asp:Label ID="lblStation" runat="server" Text="Station:" CssClass="iMes_label_13pt"></asp:Label>
       </td>
       <td width ="40%">
            
               <asp:ListBox ID="ddlStation" runat="server" SelectionMode="Multiple" Height="95%" 
                            Width="300px" CssClass="CheckBoxList">
                    </asp:ListBox>
       </td>
    </tr>
    <tr>
       <td width ="10%">
            <asp:Label ID="lblFamily" runat="server" Text="Family:" CssClass="iMes_label_13pt"></asp:Label>
       </td>
       <td width ="40%">            
            <asp:DropDownList ID="ddlFamily" runat="server" Width="300px" ></asp:DropDownList>
       </td>            
       <td width ="10%">
            <asp:Label ID="lblModel" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>

       </td>
       <td width ="40%">
            <asp:TextBox ID="txtModel" runat="server" Width="300px" ></asp:TextBox>         
       </td>
    </tr>
    <tr>               
        <td><asp:Label ID="lblModelCategory" runat="server" Text="Model Category:" CssClass="iMes_label_13pt"></asp:Label>
        </td>
        <td><iMESQuery:ChxLstProductType ID="ChxLstProductType1" runat="server"   />
        </td>
        <td colspan="2" align="center">                    
            <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" 
                    onclick="beginWaitingCoverDiv();" style="width: 100px">Query</button>
            &nbsp;&nbsp;&nbsp;                    
            <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px; display: none;">Export</button>
            &nbsp;&nbsp;&nbsp;       
            <button id="btnExportDetail"  runat="server" onserverclick="btnExportDetail_Click" 
                        style="width: 100px; display: none;">ExportDetail</button>
            <button id="btnQueryDetail"  runat="server" onserverclick="btnQueryDetail_Click" 
                        style="width: 100px; display: none;">Query_Detail</button>
        </td>
        
    </tr>
 </table>
</fieldset> 


   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>          
        
        <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="250px" 
            Width="98%" GvExtWidth="98%" Height="1px" ShowFooter="True" 
            style="top: 0px; left: 1px" >            
        </iMES:GridViewExt>
        <br />
        
        

    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
    </Triggers>
  
  </asp:UpdatePanel>
  
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>          
            <iMES:GridViewExt ID="gvDetailResult" runat="server" AutoGenerateColumns="true" GvExtHeight="250px" 
                                Width="98%" GvExtWidth="98%" Height="1px" ShowFooter="True" 
                                style="top: 0px; left: 1px" >            
            </iMES:GridViewExt>
            <br />
        </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQueryDetail" EventName="ServerClick" />
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
        </Triggers>

    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>          
            <asp:Chart ID="chart1" runat="server"  Height="600px" Visible="False" 
                Width="1300px" ImageLocation="~\tmp\ChartPic_#SEQ(20,3)">
            <Series>
                <asp:Series Name="Series1"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
        </Triggers>
    </asp:UpdatePanel>
  
  <asp:HiddenField ID="hidLine" runat="server" />
  <asp:HiddenField ID="hidFamily" runat="server" />
  <asp:HiddenField ID="hidModel" runat="server" />
  <asp:HiddenField ID="hidStation" runat="server" />
  <asp:HiddenField ID="hidprdType" runat="server" />
<asp:HiddenField ID="hidType" runat="server" />

</center>
<script type="text/javascript">
    function EndRequestHandler(sender, args) {

        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please select... ' }).multiselectfilter();
        $("#<%=gvResult.ClientID%> tr:nth-child(n+2) ").click(function() { rowclick(); });

        Calendar.setup({
            trigger: "btnFromDate",
            inputField: "<%=txtFromDate.ClientID%>",
            onSelect: updateCalendarFields,
            onTimeChange: updateCalendarFields,
            showTime: 24,
            dateFormat: "%Y-%m-%d %H:%M",
            minuteStep: 1
        });
        Calendar.setup({
            inputField: "<%=txtToDate.ClientID%>",
            trigger: "btnToDate",
            onSelect: updateCalendarFields,
            onTimeChange: updateCalendarFields,
            showTime: 24,
            dateFormat: "%Y-%m-%d %H:%M",
            minuteStep: 1
        });
    };

     window.onload = function() {
          EndRequestHandler();
          Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

      };

        function SelectDetail(Types,Family, Model, Line, Station, prdType) {
            document.getElementById("<%=hidLine.ClientID%>").value = Line;
            document.getElementById("<%=hidFamily.ClientID%>").value = Family;
            document.getElementById("<%=hidModel.ClientID%>").value = Model;
            document.getElementById("<%=hidStation.ClientID%>").value = Station;
            document.getElementById("<%=hidprdType.ClientID%>").value = prdType;
            document.getElementById("<%=hidType.ClientID%>").value = Types;
            document.getElementById("<%=btnQueryDetail.ClientID%>").click();
        }
</script>
</asp:Content>
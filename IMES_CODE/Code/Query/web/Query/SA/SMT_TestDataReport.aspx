<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="SMT_TestDataReport.aspx.cs" Inherits="SMT_TestDataReport" Title="Untitled Page" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
  <script type="text/javascript" src="../../js/jquery-1.7.1.js "></script>    
    <script type="text/javascript" src="../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js "></script>     
    <script src="../../js/jscal2.js "></script>
    <script src="../../js/lang/cn.js "></script>    

    <script type="text/javascript" src="../../js/jquery.multiselect.js "></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js "></script>     
    <script type="text/javascript" src="../../js/wz_tooltip.js "></script>
    
 <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
    <ContentTemplate>        
             <script language="javascript" type="text/javascript">
             
                </script>
                            

    </ContentTemplate>    
</asp:UpdatePanel>  
    

    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
    <script src="../../js/jscal2.js"></script>
    <script src="../../js/lang/cn.js "></script>    

    <script type="text/javascript" src="../../js/jquery.multiselect.js"></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js"></script>     
    <script type="text/javascript" src="../../js/wz_tooltip.js"></script>
    <script type="text/javascript" src="../../js/ui.dropdownchecklist.js"></script>

    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
    <link rel="stylesheet" type="text/css" href="../../css/ui.dropdownchecklist.themeroller.css" />   
<div id="ToolBar" style="height:25px;width:100%; float:left;  ">
        <fieldset style="border: solid #000000">
            <legend>SMT_TestDataReport </legend>
<div id="Tl_Center" style="margin-left:1%">
<asp:Label ID="Label1" Text="DBName:  " runat="server" meta:resourcekey="LabelResource1"></asp:Label>
<iMESQuery:CmbDBType id="CmbDBType" runat="server"  />      
                           
                        <asp:Label ID="lblFrom" runat="server" Text="From" 
        CssClass="iMes_label_13pt" meta:resourcekey="lblFromResource1"></asp:Label>
                        <asp:TextBox id="txtFromDate" runat="server" 
        Width="140px"   Height="20px" onchange="changetime();" 
        meta:resourcekey="txtFromDateResource1" ></asp:TextBox>  
                        <button id="btnFromDate" type="button" style="width: 20px" >...</button>
                        <asp:Label ID="lblTo" runat="server" Text="To" 
        CssClass="iMes_label_13pt" meta:resourcekey="lblToResource1"></asp:Label>
                        <asp:TextBox ID="txtToDate" runat="server" Width="140px" 
        Height="20px" onchange="changetime();" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                        <button id="btnToDate" type="button" style="width: 20px">...</button> 

                    <asp:Label ID="Label4" runat="server" Text="PdLine:   " 
        CssClass="iMes_label_13pt" Width="40px" meta:resourcekey="Label4Resource1" ></asp:Label>
         
                        <asp:DropDownList ID="DL_Pdline" runat="server" 
        Width="150px" Height="95%" meta:resourcekey="DL_PdlineResource1" AutoPostBack="True" 
                            onselectedindexchanged="DL_Pdline_SelectedIndexChanged"></asp:DropDownList>
               <asp:Label ID="Label2" runat="server" 
        Text="Family:" CssClass="iMes_label_13pt" Width="50px" 
        meta:resourcekey="Label1Resource1" ></asp:Label>
     <%--                   <asp:DropDownList ID="DL_Model" runat=server Height="95%" 
                                    Width="150px" 
        meta:resourcekey="DL_ModelResource1" >
                                                            </asp:DropDownList>--%>
                                                                <asp:ListBox ID="lboxStation" 
                            runat="server" CssClass="CheckBoxList" 
        SelectionMode="Multiple"  Width="50px" Height="28px">
    </asp:ListBox>
                            <asp:Label ID="Label10" runat="server" 
        Text="Station:   " CssClass="iMes_label_13pt" 
        meta:resourcekey="Label10Resource1"></asp:Label>    
                                        <asp:DropDownList ID="DL_Station" 
        runat="server" Width="120px" Height="95%" 
        meta:resourcekey="DL_StationResource1">
                                            <asp:ListItem meta:resourcekey="ListItemResource4">0A</asp:ListItem>
                                            <asp:ListItem meta:resourcekey="ListItemResource5">0B</asp:ListItem>
                                            <asp:ListItem meta:resourcekey="ListItemResource6">0C</asp:ListItem>
                                            <asp:ListItem meta:resourcekey="ListItemResource7">0D</asp:ListItem>
                                  </asp:DropDownList>
                                                      <asp:Label ID="Label3" Text="Query Select: "  runat="server" 
        meta:resourcekey="LabelResource2"></asp:Label>
                                               <asp:DropDownList ID="DL_Type" 
        runat="server" Height="95%" Width="80px" meta:resourcekey="DL_TypeResource1">
                                                   <asp:ListItem meta:resourcekey="ListItemResource1">Day</asp:ListItem>
                                                   <asp:ListItem meta:resourcekey="ListItemResource2">Week</asp:ListItem>
                                                   <asp:ListItem meta:resourcekey="ListItemResource3">Month</asp:ListItem>
                                  </asp:DropDownList>
 
    <div id="Bt_Q" style="float:right; margin-right:5%; margin-top:1px;">
        <button id="BT_Query" runat="server" onserverclick="BT_Query_Click" 
            style="width:50PX; ">
            Query
        </button>
        
    </div>
 
</div>
</fieldset>
</div>

<div id="Span" style=" width:100%; height:80px; float:left; "   >

</div>
<div id="Table" style="margin-top:4px;margin-left:1%; float:left; width:49%; height:220px; "   >
<%--<asp:DataGrid ID="DG1" runat="server" Height="147px" CellPadding="1" ForeColor="#333333" 
        GridLines="None" BorderStyle="Solid" BorderWidth="2px" 
        HorizontalAlign="Center" Width="482px">
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <EditItemStyle BackColor="#2461BF" BorderStyle="Solid" BorderWidth="2px" />
    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <AlternatingItemStyle BackColor="White" />
    <ItemStyle BackColor="#EFF3FB" />

    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    </asp:DataGrid>--%>
 <div style="margin-left:40%;"><asp:label ID="LB_Table" runat="server" Text="MB TestCount&FailCount" 
         meta:resourcekey="LabelResource3"></asp:label></div>
 <div style="width:100%;height:20px;"></div>
    <asp:GridView ID="GridView1" runat="server" BackColor="White" 
        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        GridLines="Vertical" Width="421px" HorizontalAlign="Center" Height="20px" 
        onselectedindexchanged="GridView1_SelectedIndexChanged" 
        meta:resourcekey="GridView1Resource1">
        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="Gainsboro" />
    </asp:GridView>
</div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
    <ContentTemplate>        
<div id="Defect_Rate" style="margin-top:4px;margin-left:1%; float:left; width:49%  ; height:220px;" >
<asp:Panel ID="P_Defect_Rate" runat="server" Height="220px" 
        meta:resourcekey="P_Defect_RateResource1" >
   <div style="margin-left:10px;">
    <asp:Chart ID="chart1" runat="server" Height="218px" Width="465px" 
           meta:resourcekey="chart1Resource1" Palette="Chocolate">
        <Series>
            <asp:Series Name="Rate" ChartArea="ChartArea1" 
                CustomProperties="PointWidth=0.4" >
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </ChartAreas>
               <Titles>
                   <asp:Title Font="Microsoft Sans Serif, 12pt" Name="Rate" Text="Defect_Rate">
                   </asp:Title>
               </Titles>
    </asp:Chart>
    </div>
    </asp:Panel>
</div>
</ContentTemplate>
</asp:UpdatePanel>
<div id="Span1" style=" width:100%; height:30px; float:left;"   >
</div>
<div id="Defect_Top" style="margin-top:4px;margin-left:1%; float:left; width:49% ">
<asp:Panel ID="P_Defect_Top" runat="server" Height="220px" 
        meta:resourcekey="P_Defect_TopResource1">
      <div style="margin-left:100px;">
    <asp:Chart ID="Chart2" runat="server" Height="218px" Width="465px" 
              meta:resourcekey="Chart2Resource1" onclick="Chart2_Click" Palette="Fire">
        <Series>
            <asp:Series ChartArea="ChartArea1" CustomProperties="PointWidth=0.4" 
                Name="Location_Top" YValueType="Int32">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </ChartAreas>
        <Titles>
            <asp:Title Name="Location_Top" Text="Location_Top5"   Font="Microsoft Sans Serif, 12pt">
            </asp:Title>
        </Titles>
    </asp:Chart>
    </div>
    </asp:Panel>

</div>
<div id="Defect_Analysis" style="margin-top:4px;margin-left:1%; float:left; width:49% ">
<asp:Panel ID="P_Defect_Analysis" runat="server" Height="220px" 
        meta:resourcekey="P_Defect_AnalysisResource1">
   <div style="margin-left:10px;">
       <asp:Chart ID="Chart3" runat="server" Height="218px" Width="465px" 
           meta:resourcekey="Chart3Resource1" onclick="Chart3_Click" 
           Palette="SemiTransparent">
               <Series>
                   <asp:Series ChartArea="ChartArea1" CustomProperties="PointWidth=0.4" 
                       Name="Cause_Top" YValuesPerPoint="4" YValueType="Int32">
                   </asp:Series>
               </Series>
               <ChartAreas>
                   <asp:ChartArea Name="ChartArea1">
                   </asp:ChartArea>
               </ChartAreas>
               <Titles>
            <asp:Title  Name="cause_Top" Text="DefectCode_Top5" Font="Microsoft Sans Serif, 12pt" >
            </asp:Title>
        </Titles>
    </asp:Chart>
    </div>
    </asp:Panel>
</div>

  
          <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>        
                <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="250px"
                Width="98%" GvExtWidth="98%" Height="1px">
                </iMES:GridViewExt>
          
            
             </ContentTemplate>
              <Triggers>
                
              </Triggers>
        </asp:UpdatePanel>
<%--<asp:UpdatePanel runat="server" >
<ContentTemplate>
            <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="250px" Width="98%" GvExtWidth="98%" Height="1px" ShowFooter="True" style="top: 0px; left: 0px" meta:resourcekey="gvResultResource1" OnGvExtRowClick="UpdataDetail(this)" OnGvExtRowDblClick="" SetTemplateValueEnable="False" onrowdatabound="gvResult_RowDataBound"></iMES:GridViewExt>     
         </ContentTemplate>
</asp:UpdatePanel>--%>
    <script language="javascript" type="text/javascript">
// <!CDATA[

        var inputObj;



        function bind() {
            //beginWaitingCoverDiv();
            //
        }

        function processFun(backData) {
            ShowInfo("");
            beginWaitingCoverDiv();
            //document.getElementById("<%=BT_Query.ClientID%>").click();
        }

        function initPage() {
            clearData();
            inputObj.value = "";
            getAvailableData("processFun");
            inputObj.focus();
        }

        function setCommonFocus() {
            endWaitingCoverDiv();
            inputObj.focus();
            inputObj.select();
            // window.onload();
        }

        function EndRequestHandler(sender, args) {

            $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select...' }).multiselectfilter();

            Calendar.setup({
                inputField: "<%=txtFromDate.ClientID%>",
                trigger: "btnFromDate",
                onSelect: updateCalendarFieldswithSeconds,
                onTimeChange: updateCalendarFieldswithSeconds,
                showTime: 24,
                dateFormat: "%Y-%m-%d 00:00:00",
                minuteStep: 1
            });
            Calendar.setup({
                inputField: "<%=txtToDate.ClientID%>",
                trigger: "btnToDate",
                onSelect: updateCalendarFieldswithSeconds,
                onTimeChange: updateCalendarFieldswithSeconds,
                showTime: 24,
                dateFormat: "%Y-%m-%d %H:%M:00",
                minuteStep: 1
            });
            //yyyy = year
            //MM = month
            //dd = day
            //hh = hour in am/pm (1-12)
            //HH = hour in day (0-23)
            //mm = minute
            //ss = second
            //a = Am/pm marker


            //<![CDATA[
            //]]>

        };
        window.onload = function() {
            EndRequestHandler();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            //inputObj = getCommonInputObject();
            //getAvailableData("processFun");
        };
// ]]>
    </script>
</asp:Content>


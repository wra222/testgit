<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="PCBTestReport.aspx.cs" Inherits="Query_SA_PCBTestReport" %>
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
    <style type="text/css">
        tr.clicked
        {
            background-color: white; 
        }
        .querycell
        {
            background-color: yellow;
         
        }
        .querycell:hover , .querycell.clicked
        {
            background-color: Blue;                       
        }
        .style1
        {
            width: 4%;
        }
        .style2
        {
            width: 35%;
        }
    </style> 
    <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>        
            <script language="javascript" type="text/javascript"></script>
        </ContentTemplate>    
    </asp:UpdatePanel>    
<center>
    <fieldset id="grpCarton" style="border: thin solid #000000;">
        <legend align ="left" style ="height :20px" >
            <asp:Label ID="lblTitle" runat="server" Text="Test Query" CssClass="iMes_label_13pt"></asp:Label>
        </legend> 
            <table border="0" width="100%" style="font-family: Tahoma"> 
                <tr>
                    <td width ="5%">
                        <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="35%">
                        <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                    </td>
                    <td class="style1">
                        
                    </td>
                    <td width ="5%">
                        <asp:Label ID="lblFamily" runat="server" Text="Family:" CssClass="iMes_label_13pt"></asp:Label>
                        
                    </td>
                    <td class="style2">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:ListBox ID="lboxFamily" runat="server" SelectionMode="Multiple" Width="150px" CssClass="CheckBoxList"></asp:ListBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnchangetime" EventName="serverclick" />
                        </Triggers>
                    </asp:UpdatePanel>         
                    </td>
                    <td rowspan="3" width="10%">
                        <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="beginWaitingCoverDiv();" style="width: 100px">Query</button>
                        <br />
                        <button id="btnExport"  runat="server" onserverclick="btnExport_Click" style="width: 100px; display: none;">Export</button>
                        <br />
                        <button id="btnDetailExport"  runat="server" onserverclick="btnDetailExport_Click" style="width: 100px; display: none;">DetailExport</button>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDate" runat="server" Text="StartDate:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>
                        <asp:TextBox id="txtFromDate" runat="server" Width="140px" Height="20px" onchange="changetime();"></asp:TextBox>  
                        <button id="btnFromDate" type="button" style="width: 20px" >...</button>
                        <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                        <asp:TextBox ID="txtToDate" runat="server" Width="140px" Height="20px" onchange="changetime();"></asp:TextBox>
                        <button id="btnToDate" type="button" style="width: 20px">...</button>
                    </td>
                    <td class="style1">
                        
                    </td>
                    <td>
                        <asp:Label ID="lblTestItem" runat="server" Text="TestItem:" CssClass="iMes_label_13pt"></asp:Label>
                        
                    </td>
                    <td class="style2">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:ListBox ID="lboxTestItem" runat="server" SelectionMode="Multiple" Width="150px" CssClass="CheckBoxList">
                                <asp:ListItem Value="M/B">M/B(¥þ´ú)</asp:ListItem>
                                <asp:ListItem Value="MB">MB(Â²´ú)</asp:ListItem>
                                </asp:ListBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnchangetime" EventName="serverclick" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td> &nbsp; </td>
                    <td> &nbsp; </td>
                    <td class="style1"> &nbsp; </td>
                    <td>
                        <asp:Label ID="lblFixtureID" runat="server" Text="FixtureID:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:ListBox ID="lboxFixtureID" runat="server" SelectionMode="Multiple" Width="150px" CssClass="CheckBoxList">
                                </asp:ListBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnchangetime" EventName="serverclick" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    
                </tr>
                <tr>
                    <td> &nbsp; </td>
                    <td> &nbsp; </td>
                    <td class="style1"> &nbsp; </td>
                    <td>
                        <asp:Label ID="lblStation" runat="server" Text="Station:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <asp:ListBox ID="lboxStation" runat="server" SelectionMode="Multiple" Width="150px" CssClass="CheckBoxList">
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="ICT">ICT</asp:ListItem>
                                </asp:ListBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnchangetime" EventName="serverclick" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    
                </tr>
                <tr>
                    <td> &nbsp; </td>
                    <td> &nbsp; </td>
                    <td class="style1"> &nbsp; </td>
                    <td>
                        <asp:Label ID="lblOP" runat="server" Text="Test OP:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <asp:ListBox ID="lboxOP" runat="server" SelectionMode="Multiple" Width="150px" CssClass="CheckBoxList">
                                </asp:ListBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnchangetime" EventName="serverclick" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    
                </tr>
                    <Button ID="btnShowDetail" runat="server" onserverclick="btnShowDetail_Click" onclick="beginWaitingCoverDiv();" style="display: none" ></Button>
                    <Button ID="btnchangetime" runat="server" onserverclick="btnchangetime_Click" style="display: none" ></Button>
                    
            </table>
    </fieldset>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="250px" Width="98%" GvExtWidth="98%" Height="1px" ShowFooter="True" style="top: 0px; left: 0px" meta:resourcekey="gvResultResource1" OnGvExtRowClick="UpdataDetail(this)" OnGvExtRowDblClick="" SetTemplateValueEnable="False" onrowdatabound="gvResult_RowDataBound"></iMES:GridViewExt>     
            <iMES:GridViewExt ID="grvDetail" runat="server" AutoGenerateColumns="true" GvExtHeight="200px" Width="98%" GvExtWidth="98%" Height="1px" ShowFooter="True" style="top: 0px; left: 0px"></iMES:GridViewExt>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="serverclick" />
            <asp:AsyncPostBackTrigger ControlID="btnShowDetail" EventName="serverclick" />
        </Triggers>
    </asp:UpdatePanel>
</center>
<asp:HiddenField ID="hidCol" runat="server" />
<script type="text/javascript">
   
    function EndRequestHandler(sender, args) {

        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
        $("#<%=gvResult.ClientID%> tr:nth-child(n+2) ").click(function() { rowclick(); });
        $("#<%=grvDetail.ClientID%> tr:nth-child(n+2) ").click(function() { rowclick(); });

        Calendar.setup({
            trigger: "btnFromDate",
            inputField: "<%=txtFromDate.ClientID%>",
            onSelect: function() {updateCalendarFieldswithSeconds(this); changetime(); },
            onTimeChange: function() { updateCalendarFieldswithSeconds(this); changetime(); },
            showTime: 24,
            dateFormat: "%Y-%m-%d 00:00:00",
            minuteStep: 1
        });
        Calendar.setup({
            trigger: "btnToDate",
            inputField: "<%=txtToDate.ClientID%>",
            onSelect: function() { updateCalendarFieldswithSeconds(this); changetime(); },
            onTimeChange: function() { updateCalendarFieldswithSeconds(this); changetime(); },
            showTime: 24,
            dateFormat: "%Y-%m-%d %H:%M:00",
            minuteStep: 1
        });
    };
    function UpdataDetail(com) {
        com.cells[1].innerText.trim();
    };
    function ShowDetail(col) {
        document.getElementById(ConvertID("hidCol")).value = col;
        beginWaitingCoverDiv();
        document.getElementById(ConvertID("btnShowDetail")).click();
    }
   
    function changetime() {
       // beginWaitingCoverDiv();
       // document.getElementById(ConvertID("btnchangetime")).click();
        
    };

    $(window).load(function() {
        EndRequestHandler();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });

</script>

</asp:Content>
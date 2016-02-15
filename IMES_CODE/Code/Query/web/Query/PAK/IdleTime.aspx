<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="IdleTime.aspx.cs" Inherits="Query_PAK_IdleTime" %>
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
            <asp:Label ID="lblTitle" runat="server" Text="IdleTime Query" CssClass="iMes_label_13pt"></asp:Label>
        </legend> 
            <table border="0" width="100%" style="font-family: Tahoma"> 
                <tr>
                    <td width ="5%">
                        <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="35%">
                        <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                    </td>
                    <td width="10%">
                        
                    </td>
                    <td width ="10%">
                        <asp:Label ID="lblStrandedTime" runat="server" Text="滯留時間(小時):"></asp:Label>
                    </td>
                    <td width ="30%">
                        <%--<iMES:Input ID="IptHours" runat="server" CanUseKeyboard="true" IsPaste="true"  MaxLength="3"  InputRegularExpression="^[-0-9\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9\+\s\*]" Width="15%"/>--%>
                        <asp:DropDownList ID="lboxHours" runat="server" Width="12%" ></asp:DropDownList>
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
                        <asp:Label ID="lblShipDate" runat="server" Text="ShipDate:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                         <asp:DropDownList ID="lboxSelectDay" runat="server" Width="25%" ></asp:DropDownList>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td> &nbsp; </td>
                    <td> &nbsp; </td>
                    <td> &nbsp; </td>
                    <td></td>
                    <td></td>
                </tr>
                    <Button ID="btnShowDetail" runat="server" onserverclick="btnShowDetail_Click" onclick="beginWaitingCoverDiv();" style="display: none" ></Button>
            </table>
    </fieldset>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="250px" 
            Width="98%" GvExtWidth="98%" Height="1px" ShowFooter="True" style="top: 0px; left: 0px" 
            meta:resourcekey="gvResultResource1" OnGvExtRowClick="UpdataDetail(this)" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False" onrowdatabound="gvResult_RowDataBound" ></iMES:GridViewExt>     
            <iMES:GridViewExt ID="grvDetail" runat="server" AutoGenerateColumns="true" GvExtHeight="200px" 
            Width="98%" GvExtWidth="98%" Height="1px" ShowFooter="True" style="top: 0px; left: 0px"></iMES:GridViewExt>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="serverclick" />
            <asp:AsyncPostBackTrigger ControlID="btnShowDetail" EventName="serverclick" />
        </Triggers>
    </asp:UpdatePanel>
</center>
<asp:HiddenField ID="hidCol" runat="server" />
<asp:HiddenField ID="hidCol2" runat="server" />
<script type="text/javascript">
   
    function UpdataDetail(com) {
        com.cells[1].innerText.trim();
    }
    function ShowDetail(col, col2) {
        document.getElementById(ConvertID("hidCol")).value = col;
        document.getElementById(ConvertID("hidCol2")).value = col2;
        beginWaitingCoverDiv();
        document.getElementById(ConvertID("btnShowDetail")).click();
    }
    function EndRequestHandler(sender, args) {
        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
        $("#<%=gvResult.ClientID%> tr:nth-child(n+2) ").click(function() { rowclick(); });
        $("#<%=grvDetail.ClientID%> tr:nth-child(n+2) ").click(function() { rowclick(); });
    }
    $(window).load(function() {
        EndRequestHandler();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });
   
</script>

</asp:Content>
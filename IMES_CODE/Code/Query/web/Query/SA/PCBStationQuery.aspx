<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="PCBStationQuery.aspx.cs" Inherits="Query_PCBStationQuery" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>


<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
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
    .querycell:hover
    {
        background-color: Blue;
        cursor:pointer;                 
    }
    
    .querycell.clicked
    {
        background-color: Blue;
    }

</style>
    
    <script type="text/javascript">
        function SelectDetail(family) {
            //beginWaitingCoverDiv();
            $(".clicked").removeClass("clicked");
            $(event.srcElement.parentNode).addClass("clicked");
            $(event.srcElement).addClass("clicked");
            document.getElementById("<%=hfFamily.ClientID%>").value = family;
            document.getElementById("<%=btnQueryDetail.ClientID%>").click();
        }
    </script>
                     
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
                
        </asp:ScriptManager>
 <center>
 
        <fieldset style="border: solid #000000">
            <legend>PCBStation Query</legend>
            <table border="0" width="100%" style="border-width:thin;">
                <tr>
                    <td width="10%">
                        <asp:Label ID="Label3" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="40%">
                        <iMESQuery:CmbDBType id="CmbDBType" runat="server"  />                                                
                    </td>
                    <td  width="10%">
                        <asp:Label ID="Label4" runat="server" Text="PdLine:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td  width="40%">
                        <asp:ListBox ID="lboxPdLine" runat="server" SelectionMode="Multiple" Height="95%" 
                                    Width="300px" CssClass="CheckBoxList">
                        </asp:ListBox>
                    </td>
                </tr>
            <tr>                                    
                <td width="10%">
                         <asp:Label ID="Label8" runat="server" Text="Date:" CssClass="iMes_label_13pt"></asp:Label>                   
                </td>
                <td width="40%">
                    <asp:Label ID="Label7" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
                    <asp:TextBox id="txtStartTime" runat="server" Width="150px" Height="20px"></asp:TextBox>
                    <button id="btnFromDate" type="button" style="width: 20px">...</button>                           
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtEndTime" runat="server" Width="150px" Height="20px"></asp:TextBox>
                    <button id="btnToDate" type="button" style="width: 20px">...</button>
                </td>
                <td width="10%">
                    <asp:Label ID="Label10" runat="server" Text="Station:" CssClass="iMes_label_13pt"></asp:Label>                                      
                </td>
                <td width="40%">
                    <asp:ListBox ID="lboxStation" runat="server" SelectionMode="Multiple" Width="150px"  CssClass="CheckBoxList">                     
                    </asp:ListBox>
                </td>   
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Family:" CssClass="iMes_label_13pt"></asp:Label>                   
                </td>
                <td>
                    <asp:DropDownList ID="ddlFamily" runat="server"  Width="220px"
                            onselectedindexchanged="ddlFamily_SelectedIndexChanged" AutoPostBack="true">
                     </asp:DropDownList>        
                </td>
                <td> 
                    <asp:Label ID="Label9" runat="server" Text="MB Code:" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td> 
                    <asp:ListBox ID="lboxModel" runat="server" SelectionMode="Multiple" Width="150px" CssClass="CheckBoxList">
                    </asp:ListBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="beginWaitingCoverDiv();"  
                        style="width: 100px">Query</button>
                    &nbsp;&nbsp;&nbsp                   
                    <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px;">Export</button>
                    &nbsp;&nbsp;&nbsp                   
                    <button id="btnDetailExport"  runat="server" onserverclick="btnDetailExport_Click" 
                        style="width: 100px;">DetailExport</button>

                </td>                    
            </tr>
        </table>
        
        </fieldset>
  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>        
                <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="250px"
                Width="98%" GvExtWidth="98%" Height="1px">
                </iMES:GridViewExt>
          
                <asp:LinkButton ID="lbtFreshPage" runat="server" 
                    OnClientClick="return ReloadImesPage();" onclick="lbtFreshPage_Click"></asp:LinkButton>
             </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
              </Triggers>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hfFamily" runat="server" />
        <button id="btnQueryDetail" runat="server"  onserverclick="btnQueryDetail_Click" style="display: none"></button>
        <div style="padding: 5px 0 0 0 ">
           <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>      
                     <iMES:GridViewExt ID="gvStationDetail" runat="server" AutoGenerateColumns="true"  GvExtHeight="200px" 
                            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" Visible="false" >            
                         <HeaderStyle Font-Size="Smaller" Width="50px" Height="14px" />
                     </iMES:GridViewExt>                        
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQueryDetail" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
</center>
       <asp:HiddenField ID="hidUser" runat="server" />       
       <asp:HiddenField ID="hidprocess" runat="server" />              
       <asp:HiddenField ID="hidsource" runat="server" />              
    </div>


    <script language="javascript" type="text/javascript">
        var inputObj;
        

    
        function bind() 
        {
            //beginWaitingCoverDiv();
            //
        }

        function processFun(backData) {
            ShowInfo("");
            beginWaitingCoverDiv();            
            document.getElementById("<%=btnQuery.ClientID%>").click();
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
            window.onload();
        }

        function EndRequestHandler(sender, args) {

            $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select...' }).multiselectfilter();

            Calendar.setup({
                inputField: "<%=txtStartTime.ClientID%>",
                trigger: "btnFromDate",
                onSelect: updateCalendarFieldswithSeconds,
                onTimeChange: updateCalendarFieldswithSeconds,
                showTime: 24,
                dateFormat: "%Y-%m-%d 00:00:00",
                minuteStep: 1
            });
            Calendar.setup({
                inputField: "<%=txtEndTime.ClientID%>",
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
        
    </script>
    <script type="text/javascript">
          //EndRequestHandler();
    </script>
</asp:Content>
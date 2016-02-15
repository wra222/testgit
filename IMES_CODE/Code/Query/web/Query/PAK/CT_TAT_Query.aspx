<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="CT_TAT_Query.aspx.cs" Inherits="Query_PAK_CT_TAT_Query" Title="Untitled Page" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    
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
    
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
  <div style="margin-top:10px; padding-left:10px; padding-right:20px;">
  <asp:Label ID="LabDB" Text="DBName:  " runat="server" meta:resourcekey="LabelResource1"></asp:Label>
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
                        
       <label id= "lbPlanFile" CssClass="iMes_label_13pt" align="right">QueryFile: </label>
        <asp:FileUpload ID="FileUp" runat="server" 
          style="background-color:RGB(242,254,230);Width:19%;height:24px" 
          meta:resourcekey="FileUpResource1" />
        <button type="button" style ="width:60px;  " id="btnUpload"  onclick=" clickUpload();"   >
                     Upload
                    </button>
                            <button type="button" style ="width:60px;  " runat="server"  id="Query"  onserverclick="Query_Click"  >
                     Query
                    </button>
                    <button id="btnExport" runat="server" onserverclick="btnExport_Click" 
                        style="width: 60px;"  >Export</button>
                        </div>
                                            <fieldset style="width:99%" align="center">
    <legend id="lblCreatedMOList" runat="server" style="color:Blue" class="iMes_label_13pt">
        CT TAT Query</legend>
        <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr>
                <td align="center" colspan="6" >
                
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                       <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="400px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" 
            meta:resourcekey="gvResultResource1" OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False">
        </iMES:GridViewExt>
                        <input type="hidden" id="selectdate" runat="server" >
                            <input id="selectline" runat="server" type="hidden"></input>
                            </input>
                            </input>
                        </ContentTemplate>
<%--                        <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="serverclick" />
                        </Triggers>--%>
                     </asp:UpdatePanel>
               
                </td>
            </tr>

            <tr>
                <td>
                    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </fieldset> 
        <button id="btnUploadList" runat="server" type="button" onserverclick="btnUploadList_ServerClick" style="display: none" />
  
                        <script type="text/javascript" language="javascript">
                          
                            function clickUpload() {
                                ShowInfo("");
                                fn = document.getElementById("<%=FileUp.ClientID%>").value;

                                if (fn == "") {
                                    alert("请选择Excel文件！");
                                    return;
                                }

//                                if (fn.substring(fn.length - 4).toUpperCase() != ".XLS" && fn.substring(fn.length - 5).toUpperCase() != ".XLSX") {
//                                    alert("文件应为Excel文件！");
//                                    return;
//                                }

                                if (fn.substring(fn.length - 4).toUpperCase() != ".XLS" ) {
                                    alert("文件应为Excel 2003版 文件！");
                                    return;
                                }

                                try {
                                    sfso = new ActiveXObject("Scripting.FileSystemObject");
                                }
                                catch (err) {
                                    errmsg = "new ActiveXObject(\"Scripting.FileSystemObject\"):" + err.description;
                                    ShowMessage(errmsg);
                                    ShowInfo(errmsg);
                                    return;
                                }
                                beginWaitingCoverDiv();
                                document.getElementById('<%=btnUploadList.ClientID%>').click();
                                return;
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

  

                        </script>
</asp:Content>


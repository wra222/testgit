<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for RepairInfoQuery Page
 *             
 * UI:CI-MES12-SPEC-FA-UI RepairInfo Query.docx
 * UC:CI-MES12-SPEC-FA-UC RepairInfo Query.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-30  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="RepairInfoQuery.aspx.cs" Inherits="FA_RepairInfoQuery" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
            <Services>
            </Services>
        </asp:ScriptManager>
        <center>
            <table width="95%">
                <tr>
                    <td width="15%"><asp:Label ID="lblFile" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td width="70%"><asp:FileUpload ID="FileUp" runat="server" style="background-color:RGB(242,254,230);Width:100%;height:24px" /></td>
                    <td align="right">
                        <button type="button" style ="width:110px; height:24px;" id="btnUpload" onclick="clickUpload()">
                            <%=this.GetLocalResourceObject(Pre + "_btnUpload").ToString()%>
                        </button>                        
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td align="right">
                        <button type="button" runat="server" onserverclick="btnToExcel_ServerClick" style ="width:110px; height:24px;" id="btnExport" />
                    </td>
                    <td align="right">
                        <button type="button" style ="width:110px; height:24px;" id="btnEdit" onclick="clickEdit()">
                            <%=this.GetLocalResourceObject(Pre + "_btnEdit").ToString()%>
                        </button>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:UpdatePanel runat="server" ID="up" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="gve" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="360px" OnGvExtRowClick="selectRow(this)" OnGvExtRowDblClick="clickEdit()"
                                    Width="99.9%" Height="350px" OnRowDataBound="gd_DataBound" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left">
                                    <Columns>
                                        <asp:BoundField DataField="IssueDate" />
                                        <asp:BoundField DataField="Line"  />
                                        <asp:BoundField DataField="Family"  />
                                        <asp:BoundField DataField="Model"  />
                                        <asp:BoundField DataField="SN"  />
                                        <asp:BoundField DataField="Defect"  />
                                        <asp:BoundField DataField="Descr"  />
                                        <asp:BoundField DataField="Cause"  />
                                        <asp:BoundField DataField="Action"  />
                                        <asp:BoundField DataField="Owner"  />
                                        <asp:BoundField DataField="Mark"  />
                                        <asp:BoundField DataField="hidCol" />
                                    </Columns>
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>     
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                </ContentTemplate>
            </asp:UpdatePanel> 
            <asp:UpdatePanel ID="upHidden" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <input type="hidden" runat="server" id="hidSN" />
                </ContentTemplate>
            </asp:UpdatePanel> 
            <button id="btnQueryData" runat="server" type="button" onserverclick="btnQueryData_ServerClick" style="display: none" />            
            <button id="btnReQueryData" runat="server" type="button" onserverclick="btnReQueryData_ServerClick" style="display: none" />            
            <input type="hidden" id="hidSelectedData" />
        </center>
    </div>
    
    <script type="text/javascript">

        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var selectedRowIndex = -1;
        var feature = "dialogHeight:450px;dialogWidth:800px;center:yes;status:no;help:no";
        var url = "RepairInfoEdit.aspx?Customer=<%=Customer %>";
        var ValueTransToChild = "";
        var flagModify = false;
        var editor = "<%=UserId %>";

        document.body.onload = function() {
        }

        function selectRow(row) {
            if (selectedRowIndex == row.index) {
                return;
            }
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, "<%=gve.ClientID %>");
            selectedRowIndex = row.index;
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, true, "<%=gve.ClientID %>");

            if (document.getElementById("hidSelectedData").value == row.cells[11].innerText.trim()) {
                return;
            }
            document.getElementById("hidSelectedData").value = row.cells[11].innerText.trim();
        }

        function clickEdit() {
            if (document.getElementById("hidSelectedData").value.trim() == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoSelectData").ToString()%>');
                return;
            }
            ValueTransToChild = document.getElementById("hidSelectedData").value.trim();
            flagModify = false;
            window.showModalDialog(url, window, feature);
            if (flagModify == false) return;
            document.getElementById('<%=btnReQueryData.ClientID%>').click();
        }

        function clickUpload() {
            ShowInfo("");
            fn = document.getElementById("<%=FileUp.ClientID%>").value;

            if (fn == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoFile").ToString()%>');
                return;
            }

            if (fn.substring(fn.length - 4).toUpperCase() != ".XLS" && fn.substring(fn.length - 5).toUpperCase() != ".XLSX") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgBadFileName").ToString()%>');
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

            if (!sfso.FileExists(fn)) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgFileNotExist").ToString()%>');
                return;
            }

            selectedRowIndex = -1;
            document.getElementById("hidSelectedData").value = "";
            beginWaitingCoverDiv();
            document.getElementById('<%=btnQueryData.ClientID%>').click();
            return;
        }
    </script>

</asp:Content>

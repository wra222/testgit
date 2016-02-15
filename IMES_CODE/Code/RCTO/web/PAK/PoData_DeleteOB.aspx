<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PoData(Delete for PL user) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI PO Data.docx –2011/11/08 
 * UC:CI-MES12-SPEC-PAK-UC PO Data.docx –2011/11/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-09  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="PoData_DeleteOB.aspx.cs" Inherits="PAK_PoData_DeleteOB" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
            </Services>
        </asp:ScriptManager>
        <center>
            <fieldset style="width:95%">
                <legend id="lgDeleteDN" runat="server" align="left" style="font-weight:bold" class="iMes_label_13pt">
                    <%=this.GetLocalResourceObject(Pre + "_lgDeleteDN").ToString()%>
                </legend>
                <table width="100%">
                    <tr>
                            <td width="10%" align="right">
                                <asp:Label ID="lblInput" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                            </td>
                            <td width="60%" align="left">
                                <asp:TextBox ID="txtInput" runat="server" Width="99%"
                                    CanUseKeyboard="true" IsPaste="true" MaxLength="50" onkeyup="value=value.replace(/[^-a-zA-Z0-9\/]/g,'')" />
                            </td>
                            <td width="15%" align="right">
                                <button type="button" style ="width:110px; height:24px;" id="btnQuery" onclick="clickQuery()">
                                    <%=this.GetLocalResourceObject(Pre + "_btnQuery").ToString()%>
                                </button>
                            </td>
                            <td width="15%" align="right">
                                <button type="button" style ="width:110px; height:24px;" id="btnDelete" onclick="clickDelete()">
                                    <%=this.GetLocalResourceObject(Pre + "_btnDelete").ToString()%>
                                </button>
                            </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblInputTip" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="gve1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="100%" GvExtHeight="210px" OnGvExtRowClick="selectRow(this)" OnGvExtRowDblClick=""
                                        Width="99.9%" Height="200px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                        HighLightRowPosition="3" HorizontalAlign="Left">
                                        <Columns>
                                            <asp:BoundField DataField="DN" />
                                            <asp:BoundField DataField="ShipNo"  />
                                            <asp:BoundField DataField="WaybillNo"  />
                                            <asp:BoundField DataField="Carrier"  />
                                        </Columns>
                                     </iMES:GridViewExt>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width:95%">
                <legend id="lgBatchDelete" runat="server" align="left" style="font-weight:bold" class="iMes_label_13pt">
                    <%=this.GetLocalResourceObject(Pre + "_lgBatchDelete").ToString()%>
                </legend>
                <table width="100%">
                    <tr>
                            <td width="10%" align="right">
                                <asp:Label ID="lblDNFile" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                            </td>
                            <td width="75%" align="left">
                                <input type="file" ID="bsrFile" style="Width:100%" />
                            </td>
                            <td width="15%" align="right">
                                <button type="button" style ="width:110px; height:24px;" id="btnBatchDelete" onclick="clickBatchDelete()">
                                    <%=this.GetLocalResourceObject(Pre + "_btnBatchDelete").ToString()%>
                                </button>
                            </td>
                    </tr>
                </table>
            </fieldset>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <button id="btnQueryData" runat="server" type="button" onclick="" style="display: none" />
                    <button id="btnDeleteDN" runat="server" type="button" onclick="" style="display: none" />
                    <button id="btnDeleteBatchDN" runat="server" type="button" onclick="" style="display: none" />
                </ContentTemplate>
            </asp:UpdatePanel> 
            <input type="hidden" runat="server" id="hidSelectedDN" />
            <input type="hidden" id="hidGUID" runat="server" />
        </center>
    </div>

    <script type="text/javascript">

        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var selectedRowIndex = -1;

        document.body.onload = function() {
            document.getElementById("<%=txtInput.ClientID%>").focus();
        }
        
        function selectRow(row) {
            if (selectedRowIndex == row.index) {
                return;
            }
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, "<%=gve1.ClientID %>");
            selectedRowIndex = row.index;
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, true, "<%=gve1.ClientID %>");

            if (document.getElementById("<%=hidSelectedDN.ClientID%>").value == row.cells[0].innerText.trim()) {
                return;
            }
            document.getElementById("<%=hidSelectedDN.ClientID%>").value = row.cells[0].innerText.trim();
        }

        function clickQuery() {
            selectedRowIndex = -1;
            beginWaitingCoverDiv();
            document.getElementById('<%=btnQueryData.ClientID%>').click();
            return;
        }

        function clickDelete() {
            if (document.getElementById("<%=hidSelectedDN.ClientID%>").value == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoRecordSelected").ToString()%>');
                return;
            }
            if (!confirm('<%=this.GetLocalResourceObject(Pre + "_msgConfirmDelete").ToString()%>' + document.getElementById("<%=hidSelectedDN.ClientID%>").value + "?")) {
                return;
            }
            selectedRowIndex = -1;
            beginWaitingCoverDiv();
            document.getElementById('<%=btnDeleteDN.ClientID%>').click();
            return;
        }

        function clickBatchDelete() {
            fn = document.getElementById("bsrFile").value;
            
            //检查是否选择了DN文件
            if (fn == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoFile").ToString()%>');
                return;
            }
            if (fn.substring(fn.length - 4).toUpperCase() != ".TXT") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgBadFileName").ToString()%>');
                return;
            }
            
            //检查DN文件是否存在
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
                alert('<%=this.GetLocalResourceObject(Pre + "_msgDNFileNotExist").ToString()%>');
                return;
            }
            
            //确认继续操作
            if (!confirm('<%=this.GetLocalResourceObject(Pre + "_msgConfirmBatchDelete").ToString()%>')) {
                return;
            }
            
            beginWaitingCoverDiv();
            uploadFiles(sfso, fn);
            document.getElementById('<%=btnDeleteBatchDN.ClientID%>').click();
            return;
        }
        
        function uploadFiles(objFS, fn) {
            try {
                DTSServerIP = '<%=System.Configuration.ConfigurationManager.AppSettings["DTSFtpServerIP"]%>';
                DTSServerPort = '<%=System.Configuration.ConfigurationManager.AppSettings["DTSFtpServerPort"]%>';
                dnFolderPath = fn.substring(0, fn.lastIndexOf("\\"));
                dnHomeDisk = getHomeDisk(dnFolderPath);
                uploadID = document.getElementById("<%=hidGUID.ClientID %>").value;
                batFileName = uploadID + ".bat";
                txtFileName = uploadID + ".txt";
                uploadFn = uploadID + ".TXT";

                batFile = fileObj.CreateTextFile(dnFolderPath + "\\" + batFileName, true);
                batFile.WriteLine(dnHomeDisk);
                batFile.WriteLine("cd " + dnHomeDisk + "\\");
                batFile.WriteLine("cd " + dnFolderPath);
                batFile.WriteLine("FTP -A -i -s:\"" + dnFolderPath + "\\" + txtFileName + "\"");
                batFile.Close();

                txtFile = fileObj.CreateTextFile(dnFolderPath + "\\" + txtFileName, true);
                txtFile.WriteLine("open " + DTSServerIP + " " + DTSServerPort);
                txtFile.WriteLine("put \"" + fn + "\" " + uploadID + ".TXT");
                txtFile.WriteLine("disconnect");
                txtFile.WriteLine("quit");
                txtFile.Close();

                wsh = new ActiveXObject("wscript.shell");
                wsh.run("cmd /k " + dnHomeDisk + "&cd " + dnHomeDisk + "\\&cd " + dnFolderPath + " &" + batFileName + "&exit", 2, true);
                objFS.DeleteFile(dnFolderPath + "\\" + batFileName, true);
                objFS.DeleteFile(dnFolderPath + "\\" + txtFileName, true);
            }
            catch (err) {
                ShowMessage(err.description);
                ShowInfo(err.description);
            }
        }
        
    </script>

</asp:Content>

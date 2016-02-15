<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for UploadShipmentData Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI Upload Shipment Data to SAP.docx –2011/10/26 
 * UC:CI-MES12-SPEC-PAK-UC Upload Shipment Data to SAP.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-11  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="UploadShipmentData.aspx.cs" Inherits="PAK_UploadShipmentData" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../CommonControl/calendar/calendar.js"></script>
    <script type="text/VBscript" src="../CommonControl/calendar/calendar.vbs"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <%--<asp:ServiceReference Path="Service/WebServiceUploadShipmentData.asmx" />--%>
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td colspan=2>
                        <fieldset>
                            <table width="100%">
                                <tr>
                                    <td align="right" style="width:15%">
                                        <asp:Label ID="lblFrom" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td align="left" style="width:35%">
                                        <input type="text" id="txtDateFrom" readonly="readonly" onfocus="CalDisappear();" />
	                                    <button type="button" id="btnFrom" onclick="showCalFrame(txtDateFrom)">...</button>
                                    </td>
                                    <td style="width:40%" />
                                    <td style="width:10%" />
                                </tr>                
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <input type="text" id="txtDateTo" readonly="readonly" onfocus="CalDisappear();" />
	                                    <button type="button" id="btnTo" onclick="showCalFrame(txtDateTo)">...</button>
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="chkAllData" runat="server" CssClass="iMes_label_13pt" />
                                    </td>
                                    <td>
                                        <button  id ="btnQuery"  style ="width:110px; height:24px;" type='button' onclick="onClickQuery()">
                                            <%=this.GetLocalResourceObject(Pre + "_btnQuery").ToString()%>
                                        </button>
                                    </td>
                                </tr>                             
                            </table>
                        </fieldset>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="2" align="right">
                        <button  id ="btnSave"  style ="width:110px; height:24px;" type='button' onclick="onClickSave()" >
                            <%=this.GetLocalResourceObject(Pre + "_btnSave").ToString()%>
                        </button>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="240px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                    Width="99.9%" Height="230px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left" onrowdatabound="GridView1_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox id="chk" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Date" />
                                        <asp:BoundField DataField="DN" />
                                        <asp:BoundField DataField="PN" />
                                        <asp:BoundField DataField="Model" />
                                        <asp:BoundField DataField="Qty" />
                                        <asp:BoundField DataField="Status" />
                                        <asp:BoundField DataField="Pack" />
                                        <asp:BoundField DataField="PAQC" />
                                        <asp:BoundField DataField="Flag" />
                                    </Columns>
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                
                <tr>
                    <td align="left">
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnQueryData" runat="server" type="button" style="display: none" />
                                <button id="btnSaveData" runat="server" type="button" style="display: none" />
                                <button id="btnComplete" runat="server" type="button" style="display: none" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <input id="hidSelectId" runat="server" type="hidden" />
                        <input id="hidFromDate" runat="server" type="hidden" />
                        <input id="hidToDate" runat="server" type="hidden" />
                        <input type="hidden" id="hidGUID" runat="server" />
                    </td>
                </tr>
            </table>
        </center>
    </div>

    <script type="text/javascript">

        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgNoSelData = '<%=this.GetLocalResourceObject(Pre + "_msgNoSelData").ToString()%>';
        var msgOverWrite = '<%=this.GetLocalResourceObject(Pre + "_msgOverWrite").ToString()%>';
        var ctlSelectedId = document.getElementById("<%=hidSelectId.ClientID%>");
        var bOverWrite = false;
        var targetDrive = "C:";
        var targetPath = "C:\\";

        window.onload = function() {
            d = new Date();
            now_year = d.getYear();
            now_month = d.getMonth() + 1;
            now_month = now_month >= 10 ? now_month : "0" + now_month;
            now_date = d.getDate();
            now_date = now_date >= 10 ? now_date : "0" + now_date;
            formattedDate = now_year + "-" + now_month + "-" + now_date;
            document.getElementById("txtDateFrom").value = formattedDate;
            document.getElementById("txtDateTo").value = formattedDate;
            document.getElementById("<%=hidFromDate.ClientID%>").value = formattedDate;
            document.getElementById("<%=hidToDate.ClientID%>").value = formattedDate;
        }
        initCalFrame("../CommonControl/calendar/");

        function onClickQuery() {
            ShowInfo("");
            CalDisappear();
            ctlSelectedId.value = "";
            document.getElementById("<%=hidFromDate.ClientID%>").value = document.getElementById("txtDateFrom").value;
            document.getElementById("<%=hidToDate.ClientID%>").value = document.getElementById("txtDateTo").value;
            beginWaitingCoverDiv();
            document.getElementById("<%=btnQueryData.ClientID%>").click();
        }

        function onClickSave() {
            ShowInfo("");
            CalDisappear();
            if (ctlSelectedId.value == "") {
                alert(msgNoSelData);
                return;
            }
            
            try
            {
                sfso = new ActiveXObject("Scripting.FileSystemObject");
            }
            catch (err) {
                errmsg = "new ActiveXObject(\"Scripting.FileSystemObject\"):" + err.description;
                ShowMessage(errmsg);
                ShowInfo(errmsg);
                return;
            }
            
            fPath = targetPath + "SERIAL.txt";
            if (sfso.FileExists(fPath) && !confirm(msgOverWrite)) {
                bOverWrite = false;
            }
            else {
                bOverWrite = true;
            }

            beginWaitingCoverDiv();
            document.getElementById('<%=btnSaveData.ClientID%>').click();
        }

        function setSelectVal(spanckb, id) {
            CalDisappear();
            thebox = spanckb;
            oState = thebox.checked;
            if (oState) {
                attachVal(id);
            }
            else {
                detachVal(id);
            }
        }

        function attachVal(id) {
            selValue = ',' + ctlSelectedId.value;
            temp = ',' + id + ',';
            if (selValue.indexOf(temp) == -1) {
                ctlSelectedId.value = ctlSelectedId.value + id + ',';
            }
        }

        function detachVal(id) {
            selValue = ',' + ctlSelectedId.value;
            temp = ',' + id + ',';
            selValue = selValue.replace(temp, ',');
            ctlSelectedId.value = selValue.substr(1);
        }

        function downloadFile() {
            try {
                fileObj = new ActiveXObject("Scripting.FileSystemObject");
                DTSServerIP = '<%=System.Configuration.ConfigurationManager.AppSettings["DTSFtpServerIP"]%>';
                DTSServerPort = '<%=System.Configuration.ConfigurationManager.AppSettings["DTSFtpServerPort"]%>';

                guid = document.getElementById("<%=hidGUID.ClientID %>").value;
                batFileName = guid + ".bat";
                txtFileName = guid + ".txt";

                batFile = fileObj.CreateTextFile(targetPath + batFileName, true);
                batFile.WriteLine(targetDrive);
                batFile.WriteLine("cd " + targetPath);
                batFile.WriteLine("FTP -A -i -s:\"" + targetPath + txtFileName + "\"");
                batFile.Close();

                txtFile = fileObj.CreateTextFile(targetPath + txtFileName, true);
                txtFile.WriteLine("open " + DTSServerIP + " " + DTSServerPort);
                txtFile.WriteLine("get " + guid + "-tmp.TXT");
                txtFile.WriteLine("del " + guid + "-tmp.TXT");
                txtFile.WriteLine("disconnect");
                txtFile.WriteLine("quit");
                txtFile.Close();

                wsh = new ActiveXObject("wscript.shell");
                wsh.run("cmd /k " + targetDrive + "&cd " + targetPath + "&" + batFileName + "&exit", 2, true);
                fileObj.DeleteFile(targetPath + batFileName, true);
                fileObj.DeleteFile(targetPath + txtFileName, true);

                tmpFn = targetPath + guid + "-tmp.TXT";
                finalFn = targetPath + "SERIAL.TXT";
                if (bOverWrite) {
                    if (fileObj.FileExists(finalFn)) {
                        fileObj.DeleteFile(finalFn);
                    }
                    fileObj.MoveFile(tmpFn, finalFn);
                }
                else {
                    srcFile = fileObj.OpenTextFile(tmpFn, 1);
                    strContents = srcFile.ReadAll();
                    srcFile.Close();

                    objFile = fileObj.OpenTextFile(finalFn, 8, true);
                    objFile.Write(strContents);
                    objFile.Close();

                    fileObj.DeleteFile(tmpFn);
                }

                /*
                * Answer to: ITC-1360-0880
                * Description: Use ShowMessage & ShowInfo instead of alert.
                */
                //alert('<%=this.GetLocalResourceObject(Pre + "_msgSaved").ToString()%>');
                ShowMessage('<%=this.GetLocalResourceObject(Pre + "_msgSaved").ToString()%>');
                ShowInfo('<%=this.GetLocalResourceObject(Pre + "_msgSaved").ToString()%>', 'green');
                document.getElementById('<%=btnComplete.ClientID %>').click();
            }
            catch (err) {
                ShowMessage(err.description);
                ShowInfo(err.description);
                ctlSelectedId.value = "";
                document.getElementById('<%=btnQueryData.ClientID %>').click();
            }
        }
        
    </script>

</asp:Content>

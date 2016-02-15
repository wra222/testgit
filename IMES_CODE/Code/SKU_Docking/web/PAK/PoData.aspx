<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PoData Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI PO Data.docx –2011/11/08 
 * UC:CI-MES12-SPEC-PAK-UC PO Data.docx –2011/11/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-09  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
/*
 * Answer to: ITC-1413-0039
 * Description: [新需求]增加按照文件中提供的DN 批量删除的功能
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="PoData.aspx.cs" Inherits="PAK_PoData" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../CommonControl/calendar/calendar.js"></script>
    <script type="text/VBscript" src="../CommonControl/calendar/calendar.vbs"></script>

    <div>
        <!--
            /*
            * Answer to: ITC-1360-1578
            * Description: Set page request timeout to no-limit.
            */
        -->
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
            <Services>
            </Services>
        </asp:ScriptManager>
        <center>
            <fieldset style="width:98%">
                <legend id="lgUpload" runat="server" align="center" style="font-weight:bold" class="iMes_label_13pt"></legend>
                <table width="100%">
                    <tr>
                        <td width="15%"><asp:Label ID="lblFile" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td width="70%"><input type="file" ID="bsrFile" style="Width:100%" onfocus="CalDisappear();" /></td>
                        <td align="right">
                            <button type="button" style ="width:110px; height:24px;" id="btnUpload" onclick="clickUpload()">
                                <%=this.GetLocalResourceObject(Pre + "_btnUpload").ToString()%>
                            </button>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width:98%">
                <legend id="lgQuery" runat="server" align="center" style="font-weight:bold" class="iMes_label_13pt"></legend>
                <table width="100%">
                    <tr>
                        <td width="15%"><asp:Label ID="lblDN" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td width="20%"><asp:TextBox ID="txtDN" runat="server" Width="99%" onkeyup="value=value.replace(/[^a-zA-Z0-9]/g,'')" MaxLength="16" onfocus="CalDisappear();"></asp:TextBox></td>
                        <td width="10%"></td>
                        <td width="15%"><asp:Label ID="lblPO" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td width="20%"><asp:TextBox ID="txtPO" runat="server" Width="99%" MaxLength="20" onfocus="CalDisappear();"></asp:TextBox></td>
                        <td width="5%"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblFrom" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td>
                            <input type="text" id="txtDateFrom" readonly="readonly" onkeyup="toClearDate('txtDateFrom')" onfocus="CalDisappear();" />
	                        <button type="button" id="btnFrom" onclick="showCalFrame(txtDateFrom)">...</button>
                        </td>
                        <td></td>
                        <td><asp:Label ID="lblTo" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td>
                            <input type="text" id="txtDateTo" readonly="readonly" onkeyup="toClearDate('txtDateTo')" onfocus="CalDisappear();" />
	                        <button type="button" id="btnTo" onclick="showCalFrame(txtDateTo)">...</button>
                        </td>
                        <td></td>
                        <td align="right">
                            <button type="button" style ="width:110px; height:24px;" id="btnQuery" onclick="clickQuery()">
                                <%=this.GetLocalResourceObject(Pre + "_btnQuery").ToString()%>
                            </button>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td><asp:TextBox ID="txtModel" runat="server" Width="99%" MaxLength="20" onfocus="CalDisappear();"></asp:TextBox></td>
                        <td></td>
                        <td><asp:Label ID="lblDNInfo" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td><asp:TextBox ID="txtDNInfo" runat="server" Width="99%" style="text-transform:none" MaxLength="50" onfocus="CalDisappear();"></asp:TextBox></td>
                        <td></td>
                        <td align="right">
                            <button type="button" style ="width:110px; height:24px;" id="btnClear" onclick="clickClear()">
                                <%=this.GetLocalResourceObject(Pre + "_btnClear").ToString()%>
                            </button>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width:98%">
                <table width="100%">
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblDNList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="gve1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="100%" GvExtHeight="210px" OnGvExtRowClick="showDetailInfo(this)" OnGvExtRowDblClick=""
                                        Width="99.9%" Height="200px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                        HighLightRowPosition="3" HorizontalAlign="Left">
                                        <Columns>
                                            <asp:BoundField DataField="DN" />
                                            <asp:BoundField DataField="ShipNo"  />
                                            <asp:BoundField DataField="PoNo"  />
                                            <asp:BoundField DataField="Model"  />
                                            <asp:BoundField DataField="ShipDate"  />
                                            <asp:BoundField DataField="Qty"  />
                                            <asp:BoundField DataField="Status"  />
                                            <asp:BoundField DataField="CDate"  />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td width="48%">
                            <asp:Label ID="lblDNInfoList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="4%"></td>
                        <td width="48%">
                            <asp:Label ID="lblPalletList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="gve2" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="100%" GvExtHeight="210px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        Width="99.9%" Height="200px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                        HighLightRowPosition="3" HorizontalAlign="Left">
                                        <Columns>
                                            <asp:BoundField DataField="AttrName"  />
                                            <asp:BoundField DataField="AttrValue"  />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td></td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="up3" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="gve3" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="100%" GvExtHeight="210px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        Width="99.9%" Height="200px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                        HighLightRowPosition="3" HorizontalAlign="Left">
                                        <Columns>
                                            <asp:BoundField DataField="PNo"  />
                                            <asp:BoundField DataField="UCC"  />
                                            <asp:BoundField DataField="Qty"  />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>                
                </table>
            </fieldset>       
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <button id="btnUploadData" runat="server" type="button" onclick="" style="display: none" />
                    <button id="btnQueryData" runat="server" type="button" onclick="" style="display: none" />
                    <button id="btnShowDetail" runat="server" type="button" onclick="" style="display: none" />
                </ContentTemplate>
            </asp:UpdatePanel> 
            <input type="hidden" runat="server" id="hidStation" />
            <input type="hidden" runat="server" id="hidProdId" />
            <input type="hidden" runat="server" id="hidDateFrom" />
            <input type="hidden" runat="server" id="hidDateTo" />
            <input type="hidden" runat="server" id="hidSelectedDN" />
            <input type="hidden" id="hidGUID" runat="server" />
        </center>
    </div>

    <script type="text/javascript">

        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var selectedRowIndex = -1;

        document.body.onload = function() {
            d = new Date();
            now_year = d.getYear();
            now_month = d.getMonth() + 1;
            now_month = now_month >= 10 ? now_month : "0" + now_month;
            now_date = d.getDate();
            now_date = now_date >= 10 ? now_date : "0" + now_date;
            formattedDate = now_year + "-" + now_month + "-" + now_date;
            document.getElementById("txtDateFrom").value = formattedDate;
        }
        initCalFrame("../CommonControl/calendar/");

        function clickClear() {
            CalDisappear();
            document.getElementById("<%=txtDNInfo.ClientID%>").value = "";
            document.getElementById("<%=txtDN.ClientID%>").value = "";
            document.getElementById("<%=txtPO.ClientID%>").value = "";
            document.getElementById("<%=txtModel.ClientID%>").value = "";
            document.getElementById("txtDateFrom").value = "";
            document.getElementById("txtDateTo").value = "";
            return;
        }
        
        /*
        * Answer to: ITC-1360-1030
        * Description: Support backspace/delete to clean date text.
        */
        function toClearDate(id) {
            if (event.keyCode == 8 || event.keyCode == 46) document.getElementById(id).value = "";
        }
        
        function showDetailInfo(row) {
            CalDisappear();
            ShowInfo("");
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
            beginWaitingCoverDiv();
            document.getElementById('<%=btnShowDetail.ClientID%>').click();
        }

        function clickQuery() {
            CalDisappear();

            /*
            * Answer to: ITC-1360-1312
            * Description: Only empty string/10-bit/16-bit are allowed for DN.
            */
            valDN = document.getElementById("<%=txtDN.ClientID%>").value;
            pattDN1 = /^$/;
            pattDN2 = /^[0-9A-Za-z]{10}$/;
            pattDN3 = /^[0-9A-Za-z]{16}$/;
            if (!pattDN1.exec(valDN) && !pattDN2.exec(valDN) && !pattDN3.exec(valDN)) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgBadDN").ToString()%>');
                document.getElementById("<%=txtDN.ClientID%>").select();
                return;
            }
            
            valDNInfo = document.getElementById("<%=txtDNInfo.ClientID%>").value.trim();
            pattDNInfo = /^[-0-9a-zA-Z\/\s]*$/;
            if (!pattDNInfo.exec(valDNInfo)) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgBadDNInfo").ToString()%>');
                document.getElementById("<%=txtDNInfo.ClientID%>").select();
                return;
            }
            
            ShowInfo("");
            document.getElementById("<%=hidDateFrom.ClientID%>").value = document.getElementById("txtDateFrom").value;
            document.getElementById("<%=hidDateTo.ClientID%>").value = document.getElementById("txtDateTo").value;
            selectedRowIndex = -1;
            document.getElementById("<%=hidSelectedDN.ClientID%>").value = "";
            beginWaitingCoverDiv();
            document.getElementById('<%=btnQueryData.ClientID%>').click();                        
            return;
        }

        function clickUpload() {
            CalDisappear();
            ShowInfo("");
            fn = document.getElementById("bsrFile").value;
            
            //DN文件名合法性检查
            if (fn == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoFile").ToString()%>');
                return;
            }
            if (fn.substring(fn.length - 8).toUpperCase() != "COMN.TXT") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgBadFileName").ToString()%>');
                return;
            }
            palletFn = fn.substring(0, fn.length - 8).concat("PALTNO.TXT");

            //检查DN、PN文件是否存在

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
            if (!sfso.FileExists(palletFn)) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgPNFileNotExist").ToString()%>');
                return;
            }

            //确认继续操作
            if (!confirm('<%=this.GetLocalResourceObject(Pre + "_msgConfirmUpload").ToString()%>')) {
                return;
            }

            selectedRowIndex = -1;
            document.getElementById("<%=hidSelectedDN.ClientID%>").value = "";
            beginWaitingCoverDiv();
            uploadFiles(sfso, fn, palletFn);
            document.getElementById('<%=btnUploadData.ClientID%>').click();
            return;
        }

        /*
         * Answer to: ITC-1360-0027
         * Description: Use ftp protocol to upload files from client to server.
         */
        function uploadFiles(objFS, fn1, fn2) {
            try
            {
                DTSServerIP = '<%=System.Configuration.ConfigurationManager.AppSettings["DTSFtpServerIP"]%>';
                DTSServerPort = '<%=System.Configuration.ConfigurationManager.AppSettings["DTSFtpServerPort"]%>';
                dnFolderPath = fn1.substring(0, fn1.lastIndexOf("\\"));
                dnHomeDisk = getHomeDisk(dnFolderPath);
                uploadID = document.getElementById("<%=hidGUID.ClientID %>").value;
                batFileName = uploadID + ".bat";
                txtFileName = uploadID + ".txt";
                uploadFn1 = uploadID + "COMN.TXT";
                uploadFn2 = uploadID + "PALTNO.TXT";
                
                batFile = fileObj.CreateTextFile(dnFolderPath + "\\" + batFileName, true);
                batFile.WriteLine(dnHomeDisk);
                batFile.WriteLine("cd " + dnHomeDisk + "\\");
                batFile.WriteLine("cd " + dnFolderPath);
                batFile.WriteLine("FTP -A -i -s:\"" + dnFolderPath + "\\" + txtFileName + "\"");
                batFile.Close();

                txtFile = fileObj.CreateTextFile(dnFolderPath + "\\" + txtFileName, true);
                txtFile.WriteLine("open " + DTSServerIP + " " + DTSServerPort);
                txtFile.WriteLine("put \"" + fn1 + "\" " + uploadID + "COMN.TXT");
                txtFile.WriteLine("put \"" + fn2 + "\" " + uploadID + "PALTNO.TXT");
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

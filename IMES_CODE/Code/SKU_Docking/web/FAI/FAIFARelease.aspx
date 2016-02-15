<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FAIFARelease.aspx.cs" Inherits="FA_FAIFARelease" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>

    
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Services>
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 99%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td style="width:7%">
                        <asp:Label ID="lblState" runat="server" CssClass="iMes_label_13pt">State:</asp:Label>
                        
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbState" runat="server" Width="98%">
                            <asp:ListItem Text="Waiting" Value="Waiting"></asp:ListItem>
                            <asp:ListItem Text="Approval" Value="Approval"></asp:ListItem>
                            <asp:ListItem Text="Release" Value="Release"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width:25%">
                        <asp:Label ID="lblFrom" runat="server" CssClass="iMes_label_13pt" Text="Date From:"></asp:Label>&nbsp;&nbsp;
                        <input type="text" id="txtDateFrom" readonly="readonly" onkeyup="toClearDate('txtDateFrom')" onfocus="CalDisappear();" onpropertychange="checkselectdatetime()" />
	                    <button type="button" id="btnFrom" onclick="showCalendar(txtDateFrom)">...</button>
                    </td>
                    <td style="width:25%">
                        <asp:Label ID="lblTo" runat="server" CssClass="iMes_label_13pt" Text="To:"></asp:Label>&nbsp;&nbsp;
                        <input type="text" id="txtDateTo" readonly="readonly" onkeyup="toClearDate('txtDateTo')" onfocus="CalDisappear();" onpropertychange="checkselectdatetime()"/>
	                    <button type="button" id="btnTo" onclick="showCalendar(txtDateTo)">...</button>
                    </td>
                    <td>
                        <input type="button" id="btnQuery" runat="server" value="Query" onclick="if(query())" onserverclick="btnQuery_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                    </td>
                    <td>
                        <input type="button" id="btnExport" runat="server" value="Export" onserverclick="btnToExcel_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                    </td>
                </tr>
                <tr>
                    <td>
                         <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt">FAI Model:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtModel" runat="server" ></asp:TextBox>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <asp:Panel ID="Panel3" runat="server" Width="100%"> 
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <%--<td align="center">
                            <input type="button" id="btnRelease" runat="server" value="Release" onserverclick="btnRelease_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>&nbsp;&nbsp;
                            <input type="button" id="btnHold" runat="server" value="Hold" onserverclick="btnHole_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                        </td>--%>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                                <ContentTemplate>
                                    <div id="div2" style="height:155">
                                        <iMES:GridViewExt ID="gd" runat="server" 
                                            AutoGenerateColumns="true" 
                                            GvExtHeight="150"
                                            GvExtWidth="100%" 
                                            style="top: 0px; left: 0px" 
                                            Height="140" 
                                            SetTemplateValueEnable="False" 
                                            HighLightRowPosition="3" 
                                            AutoHighlightScrollByValue="True"
                                            onrowdatabound="gd_RowDataBound"
                                            OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'>
                                            
                                        </iMES:GridViewExt> 
                                        
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <%--<asp:AsyncPostBackTrigger ControlID="btnUpload" EventName="ServerClick" />--%>
                                </Triggers>
                            </asp:UpdatePanel>
                            
                        </td>
                    </tr> 
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                                <ContentTemplate>
                                    <div id="div3" style="height:155">
                                        <iMES:GridViewExt ID="gd2" runat="server" 
                                            AutoGenerateColumns="true" 
                                            GvExtHeight="150"
                                            GvExtWidth="100%" 
                                            style="top: 0px; left: 0px" 
                                            Height="140" 
                                            SetTemplateValueEnable="False" 
                                            HighLightRowPosition="3" 
                                            AutoHighlightScrollByValue="True"
                                            onrowdatabound="gd2_RowDataBound">
                                            
                                        </iMES:GridViewExt> 
                                    </div>
                                    <input type="button" id="btnquerygd2" runat="server" onserverclick="btnQuerygd2_ServerClick" class="iMes_button" style="display: none"/>
                                    <input id="hidoldguid" type="hidden" runat="server" />
                                    <input id="hidoldfilename" type="hidden" runat="server" />
                                </ContentTemplate>
                                <Triggers>
                                    
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr> 
                </table>
             </asp:Panel>
             <table width="100%" border="0" style="table-layout: fixed;">
                <tr> 
                    <td style="width:8%">
                        <asp:Label ID="lblFAIModel" runat="server" CssClass="iMes_label_13pt" Text="FAIModel:"></asp:Label>
                    </td>
                    <td style="width:25%" align="left">
                        <asp:Label ID="lblFAIModelContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:8%">
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt" Text="Family:"></asp:Label>
                        
                    </td>
                    <td style="width:30%" align="left">
                        <asp:Label ID="lblFamilyContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:8%">
                        <asp:Label ID="lblModelType" runat="server" CssClass="iMes_label_13pt" Text="Model Type:"></asp:Label>
                        
                    </td>
                    <td style="width:25%" align="left">
                        <asp:Label ID="lblModelTypeContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr> 
                    <td>
                        <asp:Label ID="lblDepartment" runat="server" CssClass="iMes_label_13pt" Text="Department:"></asp:Label>
                        
                    </td>
                    <td>
                        <asp:Label ID="txtDepartment" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2" align="left">
                        <asp:FileUpload ID="FileUpload1" runat="server" Width="90%" />
                    </td>
                    <td colspan="2" align="left">
                        <input type="button" id="btnUpload" runat="server" value="Upload" onclick="if(upload())" onserverclick="btnUpload_Click" 
                        class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>                            
                    </td>
                    <td></td>
                </tr>
                <tr> 
                    <td >
                        <asp:Label ID="lblComment" runat="server" CssClass="iMes_label_13pt" Text="Comment:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:UpdatePanel ID="updatePanel6" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>
                                <asp:TextBox ID="txtComment" runat="server" Width="98%"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td colspan="2" align="left">
                        <asp:UpdatePanel ID="updatePanel5" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>
                                <input type="button" id="btnApprove" runat="server" value="Approve" onclick="if(approve())" onserverclick="btnApprove_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>&nbsp;&nbsp;
                                <input type="button" id="btnRemove" runat="server" value="Remove" onserverclick="btnRemove_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                                <input id="hidfilename" type="hidden" runat="server" />
                                <input id="hidapprovalStatusID" type="hidden" runat="server" />
                                <input id="hidapprovalItemID" type="hidden" runat="server" />
                                <input id="hidIsNeedUploadFile" type="hidden" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                    </td>
                    <td></td>
                </tr>
            </table>              
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>
        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updHidden" runat="server" RenderMode="Inline" UpdateMode="Always">
            <ContentTemplate>
                <input id="TypeValue" type="hidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <input id="hidQueryValue" type="hidden" runat="server" />
        <input id="hideditor" type="hidden" runat="server" />
        <input id="hidcustomer" type="hidden" runat="server" />
        <input id="hidmodel" type="hidden" runat="server" />
        <input id="hidfastatus" type="hidden" runat="server" />
        <input id="hidReUpload" type="hidden" runat="server" />
        <input id="hidDateFrom" type="hidden" runat="server"  />
        <input id="selectfromdate" type="hidden" runat="server" />
        <input id="selecttodate" type="hidden" runat="server" />
        <input id="hidFamily" type="hidden" runat="server" />
        <input id="hidNewFileName" type="hidden" runat="server" />
        
        
    </div>   
    <script language="javascript" type="text/javascript">
        var editor;
        var tbl;
        var DEFAULT_ROW_NUM = 5;
        var customer;
        var stationId;
        var showMsgList = "";
        var accountid = '<%=AccountId%>';
        var login = '<%=Login%>';
        var department = '<%=Department%>';
        window.onload = function() {
            d = new Date();
            now_year = d.getYear();
            now_month = d.getMonth() + 1;
            now_month = now_month >= 10 ? now_month : "0" + now_month;
            now_date = d.getDate();
            now_date = now_date >= 10 ? now_date : "0" + now_date;

            toattedDate = now_year + "-" + now_month + "-" + now_date;
            document.getElementById("txtDateTo").value = toattedDate;
            d.setDate(d.getDate() - 2);
            now_date = d.getDate();
            now_month = d.getMonth() + 1;
            now_year = d.getYear();

            formattedDate = now_year + "-" + now_month + "-" + now_date;
            //formattedDate = now_year + "-" + '09' + "-" + now_date;
            document.getElementById("txtDateFrom").value = formattedDate;

            tbl = "<%=gd.ClientID %>";
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
        };

        function clickTable(con) {
            //beginWaitingCoverDiv();
            setGdHighLight(con);
            ShowRowEditInfo(con);
        }

        var iSelectedRowIndex = null; 
        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }
        
        function getcmbStateObj() {
            return document.getElementById("<%=cmbState.ClientID %>");
        }

        function query() {
            ShowInfo("");
            var state = getcmbStateObj().options[getcmbStateObj().selectedIndex].value;
            if (state == "") {
                alert("請選擇State...");
                return false;
            }
            var startDate = document.getElementById('txtDateFrom').value;
            var endDate = document.getElementById('txtDateTo').value;
            if (Date.parse(startDate.replace(/-/g, "/")).valueOf() > Date.parse(endDate.replace(/-/g, "/")).valueOf()) {
                alert("注意開始時間不能晚於結束時間！");
                return false;
            }
            beginWaitingCoverDiv();
            return true;
        }

        function checkselectdatetime() {
            document.getElementById("<%=selectfromdate.ClientID %>").value = document.getElementById('txtDateFrom').value;
            document.getElementById("<%=selecttodate.ClientID %>").value = document.getElementById('txtDateTo').value;
        }

        function approve() {
            ShowInfo("");
            var filename = document.getElementById("<%=hidfilename.ClientID %>").value;
            var isNeedUploadFile = document.getElementById("<%=hidIsNeedUploadFile.ClientID %>").value;
//            if (isNeedUploadFile == "Y") {
//                if (filename == "") {
//                    alert("請先上傳檔案...");
//                    return false;
//                }
//            }
            beginWaitingCoverDiv();
            return true;
        }

        function upload() {
            ShowInfo("");
            var faiModel = document.getElementById("<%=lblFAIModelContent.ClientID %>").innerText;
            document.getElementById("<%=hidmodel.ClientID %>").value = faiModel;
            var oldfilename = document.getElementById("<%=hidfilename.ClientID %>").value;
            var reupload = document.getElementById("<%=hidReUpload.ClientID %>").value;
            var filename = document.getElementById("<%=FileUpload1.ClientID %>").value;
            document.getElementById("<%=hidNewFileName.ClientID %>").value = filename;
            if (filename == "") {
                alert("請選擇檔案...");
                return false;
            }
            if (oldfilename != "" && reupload == "N") {
                alert("檔案已經存在，無法上傳");
                return false;
            }
            beginWaitingCoverDiv();
            return true;
        }       

        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }
            if (con.cells[0].innerText.trim() == "") {
                setNewItemValue();
                return;
            }
            document.getElementById("<%=lblFAIModelContent.ClientID %>").innerText = con.cells[0].innerText.trim();
            document.getElementById("<%=hidmodel.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=lblFamilyContent.ClientID %>").innerText = con.cells[1].innerText.trim();
            document.getElementById("<%=hidFamily.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=lblModelTypeContent.ClientID %>").innerText = con.cells[3].innerText.trim();
            document.getElementById("<%=hidfastatus.ClientID %>").value = con.cells[6].innerText.trim();
            document.getElementById("<%=txtDepartment.ClientID %>").innerText = department;
            document.getElementById("<%=btnquerygd2.ClientID %>").click();
            document.getElementById("<%=btnExport.ClientID %>").disabled = false;
        }

        function setNewItemValue() {
            
            document.getElementById("<%=lblFAIModelContent.ClientID %>").innerText = "";
            document.getElementById("<%=hidmodel.ClientID %>").value = "";
            document.getElementById("<%=lblModelTypeContent.ClientID %>").innerText = "";
            document.getElementById("<%=txtComment.ClientID %>").value = "";
            document.getElementById("<%=hidfastatus.ClientID %>").value = "";
            document.getElementById("<%=hidfilename.ClientID %>").value = "";
            document.getElementById("<%=hidapprovalStatusID.ClientID %>").value = "";
            document.getElementById("<%=hidapprovalItemID.ClientID %>").value = "";
            document.getElementById("<%=hidIsNeedUploadFile.ClientID %>").value = "";
            document.getElementById("<%=txtDepartment.ClientID %>").innerText = "";
            document.getElementById("<%=lblFamilyContent.ClientID %>").innerText = "";
            document.getElementById("<%=hidFamily.ClientID %>").value = "";
            document.getElementById("<%=btnApprove.ClientID %>").disabled = true;
            document.getElementById("<%=btnRemove.ClientID %>").disabled = true;
            document.getElementById("<%=btnExport.ClientID %>").disabled = false;
            document.getElementById("<%=btnquerygd2.ClientID %>").click();

        }

        function showsuccessMsg() {
            ShowInfo("Success!!","green");
        }
       
        function initPage()
        {
            tbl = "<%=gd.ClientID %>";
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            tbl = "<%=gd2.ClientID %>";
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            setSrollByIndex(0 ,false);
        }

        window.onbeforeunload = function()
        {
            
        };   
        
        function ExitPage(){
        
        }
        
        function ResetPage(){
            ExitPage();
            initPage();
            ShowInfo("");
        }
    </script>
</asp:Content>


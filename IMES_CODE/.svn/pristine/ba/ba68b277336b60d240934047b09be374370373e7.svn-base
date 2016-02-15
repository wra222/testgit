<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FAIPAKRelease.aspx.cs" Inherits="FA_FAIPAKRelease" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <script type="text/javascript" src="../js/jquery-1.7.1.js "></script>    
    <script type="text/javascript" src="../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.13.custom.min.js "></script>     
    <script src="../js/jscal2.js "></script>
    <script src="../js/lang/cn.js "></script>    
    <script type="text/javascript" src="../js/superTables.js"></script>
    <script type="text/javascript" src="../js/jquery.superTable.js"></script>
    <script type="text/javascript" src="../js/jquery.multiselect.js "></script>     
    <script type="text/javascript"  src="../js/jquery.multiselect.filter.js "></script>     
    <script type="text/javascript" src="../js/wz_tooltip.js "></script>
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.multiselect.filter.css" />
    <link rel="stylesheet" type="text/css" href="../css/superTables.css" />
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Services>
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 99%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td style="width:5%">
                        <asp:Label ID="lblState" runat="server" CssClass="iMes_label_13pt">State:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbState" runat="server" Width="98%">
                            <asp:ListItem Text="Hold" Value="Hold"></asp:ListItem>
                            <asp:ListItem Text="Release" Value="Release"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width:6%">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt">FAI Model:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtModel" runat="server" ></asp:TextBox>
                    </td>
                    <td>
                        <input type="button" id="btnQuery" runat="server" value="Query" onclick="if(query())" onserverclick="btnQuery_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                    </td>
                    <td>
                        
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel3" runat="server" Width="100%"> 
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                                <ContentTemplate>
                                    <div id="div2" style="height:225px">
                                        <iMES:GridViewExt ID="gd" runat="server" 
                                            AutoGenerateColumns="true" 
                                            GvExtHeight="220px"
                                            GvExtWidth="100%" 
                                            style="top: 0px; left: 0px" 
                                            Height="210px" 
                                            SetTemplateValueEnable="False" 
                                            HighLightRowPosition="3" 
                                            AutoHighlightScrollByValue="True"
                                            onrowdatabound="gd_RowDataBound"
                                            OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'>
                                            
                                        </iMES:GridViewExt> 
                                    </div>
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
                <tr style="width:120px"> 
                    <td>
                        <asp:Label ID="lblFAIModel" runat="server" CssClass="iMes_label_13pt" Text="FAIModel:"></asp:Label>&nbsp;&nbsp;
                        <asp:Label ID="lblFAIModelContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblModelType" runat="server" CssClass="iMes_label_13pt" Text="Model Type:"></asp:Label>&nbsp;&nbsp;
                        <asp:Label ID="lblModelTypeContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblDB" runat="server" Text="upload:" CssClass="iMes_label_13pt"></asp:Label>&nbsp;&nbsp;
                        <asp:FileUpload ID="FileUpload1" runat="server" Width="65%"/>
                        <input type="button" id="btnUpload" runat="server" value="Upload" onclick="if(upload())" onserverclick="btnUpload_Click" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>        
                    </td>
                </tr>
                <tr style="width:120px"> 
                    <td>
                        <asp:Label ID="lblPAKQty" runat="server" CssClass="iMes_label_13pt" Text="PAKQty:"></asp:Label>&nbsp;&nbsp;
                        <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>
                                <asp:TextBox ID="txtPAKQty" runat="server"></asp:TextBox>   
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblStartDate" runat="server" CssClass="iMes_label_13pt" Text="StartDate:"></asp:Label>&nbsp;&nbsp;
                        <%--<input type="text" id="dCalShipdate" style="width:100px;" readonly="readonly" class="iMes_textbox_input_Yellow"/>
                        <input id="btnCal" type="button" value=".." onclick="showCalendar('dCalShipdate')"  style="width: 17px" class="iMes_button"  />--%>
                        <asp:TextBox ID="dCalShipdate" runat="server" Width="140px" Height="20px"></asp:TextBox>
                        <button id="btnCal" type="button" style="width: 20px">...</button>
                    </td>
                    <td align="center">
                        <asp:UpdatePanel ID="updatePanel5" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>
                                <input type="button" id="btnSave" runat="server" value="Save" onclick="if(Save())" onserverclick="btnSave_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>&nbsp;&nbsp;
                                <input type="button" id="btnRelease" runat="server" value="Release" onclick="if(Release())" onserverclick="btnRelease_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                                <input id="hidfilename" type="hidden" runat="server" />
                                <input id="hidapprovalStatusID" type="hidden" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td></td>
                </tr>
            </table>              
        </div>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" UpdateMode="Always" RenderMode="Inline" Visible="true">
            <ContentTemplate>
                <button id="btnDateChange" runat="server" style="display: none" />
                <input type="hidden" id="hidshipdate" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnRelease" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
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
        <input id="hidStartDate" type="hidden" runat="server" />
        <input id="hidOldStartDate" type="hidden" runat="server" />
        <input id="hidPAKQty" type="hidden" runat="server" />
        
        
    </div>   
    <script language="javascript" type="text/javascript">
        var editor;
        var tbl;
        var DEFAULT_ROW_NUM = 7;
        var customer;
        var stationId;
        var showMsgList = "";
        var accountid = '<%=AccountId%>';
        var login = '<%=Login%>';
        var toDate = document.getElementById("<%=dCalShipdate.ClientID %>");



        window.onload = function() {
            EndRequestHandler();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            toDate.value = "<%=today%>";
            tbl = "<%=gd.ClientID %>";
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
        };

        function EndRequestHandler(sender, args) {
            Calendar.setup({
                trigger: "btnCal",
                inputField: "<%=dCalShipdate.ClientID%>",
                onSelect: updateCalendarFields,
                onTimeChange: updateCalendarFields,
                showTime: 24,
                dateFormat: "%Y-%m-%d %H:%M",
                minuteStep: 1
            });
        };

        function updateFields(cal) {
            var date = cal.selection.get();
            if (date) {
                date = Calendar.intToDate(date);
                cal.inputField.value = Calendar.printDate(date, "%Y-%m-%d") +
             " " + ("0" + cal.getHours()).right(2) + ":" + ("0" + cal.getMinutes()).right(2);
            }
        };

        function clickTable(con) {
            ShowInfo("");
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
            beginWaitingCoverDiv();
            return true;
        }

        function Save() {
            ShowInfo("");
            beginWaitingCoverDiv();
            //SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
//            Date tempDate = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").parse("09/12/2014");
//            if (Date.parse(new Date()).valueOf() > Date.parse(toDate.value).valueOf()) {
//                alert("注意開始時間不能晚於結束時間！");
//                return false;
            //            }
            document.getElementById("<%=hidStartDate.ClientID %>").value = toDate.value;
//            if (isNeedUploadFile == "Y") {
//                if (filename == "") {
//                    alert("Please Upload File and then Save it...");
//                    return false;
//                }
//            }
            return true;
        }

        function Release() {
            ShowInfo("");
            var filename = document.getElementById("<%=hidfilename.ClientID %>").value;
            document.getElementById("<%=hidStartDate.ClientID %>").value = toDate.value;
            beginWaitingCoverDiv();
            return true;
        }

        function showsuccessMsg() {
            ShowInfo("Success!!", "green");
        }

        function upload() {
            ShowInfo("");
            var faiModel = document.getElementById("<%=lblFAIModelContent.ClientID %>").innerText;
            document.getElementById("<%=hidmodel.ClientID %>").value = faiModel;
            var filename = document.getElementById("<%=hidfilename.ClientID %>").value;
            if (faiModel == "") {
                alert("FAIModel is null, can not upload");
                return false;
            }
//            if (filename == "") {
//                alert("FileName is null, can not upload");
//                return false;
//            }
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
            document.getElementById("<%=lblModelTypeContent.ClientID %>").innerText = con.cells[1].innerText.trim();
            document.getElementById("<%=txtPAKQty.ClientID %>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=hidPAKQty.ClientID %>").value = con.cells[2].innerText.trim();

            toDate.value = con.cells[4].innerText.trim();
            
            document.getElementById("<%=hidStartDate.ClientID %>").value = con.cells[4].innerText.trim();
            document.getElementById("<%=hidOldStartDate.ClientID %>").value = con.cells[4].innerText.trim();
            document.getElementById("<%=hidfastatus.ClientID %>").value = con.cells[5].innerText.trim();
            document.getElementById("<%=hidfilename.ClientID %>").value = con.cells[7].innerText.trim();

            document.getElementById("<%=hidapprovalStatusID.ClientID %>").value = con.cells[11].innerText.trim();
            if (document.getElementById("<%=hidfastatus.ClientID %>").value == "Hold") {
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
                document.getElementById("<%=btnRelease.ClientID %>").disabled = false;
            }
        }

        function setNewItemValue() {
            
            document.getElementById("<%=lblFAIModelContent.ClientID %>").innerText = "";
            document.getElementById("<%=hidmodel.ClientID %>").value = "";
            document.getElementById("<%=lblModelTypeContent.ClientID %>").innerText = "";
            document.getElementById("<%=txtPAKQty.ClientID %>").value = "";
            document.getElementById("<%=hidPAKQty.ClientID %>").value = "";
            document.getElementById("<%=hidfastatus.ClientID %>").value = "";
            document.getElementById("<%=hidfilename.ClientID %>").value = "";
            document.getElementById("<%=hidapprovalStatusID.ClientID %>").value = "";
            document.getElementById("<%=hidOldStartDate.ClientID %>").value = "";
            
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            document.getElementById("<%=btnRelease.ClientID %>").disabled = true;
        }
       
        function initPage()
        {
            tbl = "<%=gd.ClientID %>";
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


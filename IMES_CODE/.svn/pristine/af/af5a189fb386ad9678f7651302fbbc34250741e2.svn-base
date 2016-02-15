<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ProductionPlan.aspx.cs" Inherits="FA_ProductionPlan"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    <Services>
        <asp:ServiceReference Path=  "~/FA/Service/WebServiceVirtualMO.asmx"/>
    </Services>
    </asp:ScriptManager>
    <link rel="stylesheet" type="text/css" href="../CommonControl/jquery/css/smoothness/jquery-ui-1.8.18.custom.css" /> 
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>


<div>
    <center >
    <br />
        <table  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr>
                <td width="7%" align="right">
                    <label id= "lbPdline" CssClass="iMes_label_13pt">PdLine:</label>    
                </td>
                <td width="10%" align="left">
                    <asp:DropDownList ID="cmbPdLine" runat="server" Width="100"></asp:DropDownList>
                </td>
                <!--<td></td>-->
                <td align="right" width="7%">
                    <label id= "lbDate" CssClass="iMes_label_13pt">ShipDate:</label>    
                </td>
                <td width="10%">
                    <input type="text" id="dCalShipdate" style="width:90px;" readonly="readonly" />
                    <input id="btnCal" type="button" value=".." 
                        onclick="showCalendar('dCalShipdate')" style="width: 17px" class="iMes_button"  />
                </td>
                <td align="center" width="17%">
                    <input type="radio" name="selectdata" id="Collect" checked runat="server" /><asp:Label ID="lbCollect" runat="server" CssClass="iMes_label_11pt" Text="彙總"></asp:Label>&nbsp;&nbsp;
                    <input type="radio" name="selectdata" id="Scheduling" runat="server" /><asp:Label ID="lbScheduling" runat="server" CssClass="iMes_label_11pt" Text="排產"></asp:Label>&nbsp;&nbsp;
                    <input type="radio" name="selectdata" id="subtractsingle" runat="server" /><asp:Label ID="lbsubtractsingle" runat="server" CssClass="iMes_label_11pt" Text="加減單"></asp:Label>
                </td>
                <td align="left">
                    <input id="btnQuery" type="button" runat="server" value="Query" onserverclick="btnQuery_serverclick" 
                        onclick="check();beginWaitingCoverDiv();"/>
                </td>
            </tr>
			<tr><td colspan ="6">&nbsp;</td></tr>
            <tr>
                <td width="7%" align="right">
                    <label id= "lbPlanFile" CssClass="iMes_label_13pt" align="right">PlanFile: </label>
                </td>
                <td width="11%" align="left" colspan ="4" >
                    
                    <asp:FileUpload ID="FileUp" runat="server" style="background-color:RGB(242,254,230);Width:80%;height:24px" />
                    
                </td>                
                <td>
                    <button type="button" style ="width:85; height:24px;" id="btnUpload"  onclick="check();clickUpload();" >
                    排產/加印上傳
                    </button>
                    <button type="button" style ="width:85; height:24px;" id="btnUpload2"  onclick="check();clickUpload_Revise();" >
                    客戶加/拉單上傳
                    </button>					
                </td>
				<!--
                <td align="left">&nbsp;
                </td>-->
            </tr>  
        </table>
     
    <hr class="footer_line" style="width:95%"/>
     
    <fieldset style="width:99%" align="center">
    <legend id="lblCreatedMOList" runat="server" style="color:Blue" class="iMes_label_13pt"></legend>
        <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr>
                <td align="center" colspan="6" >
                
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                        <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="400px" 
                            Width="100%" GvExtWidth="100%" Height="1px" ShowFooter="True" onrowdatabound="gvResult_RowDataBound"
                                style="top: 0px; left: -8px">            
                        </iMES:GridViewExt>
                        <input type="hidden" id="selectdate" runat="server" />
                        <input type="hidden" id="selectline" runat="server" />        
                        </ContentTemplate>
                        <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="serverclick" />
                        </Triggers>
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
    <br /> 
    
    <button id="btnUploadList" runat="server" type="button" onserverclick="btnUploadList_ServerClick" style="display: none" />
    <button id="btnUploadList_Revise" runat="server" type="button" onserverclick="btnUploadList_Revise_ServerClick" style="display: none" /> 
    </center>
</div>
<script>
   
</script>
    <script type="text/javascript" language="javascript">
        var toDate = document.getElementById('dCalShipdate');
        document.body.onload = function() {
            toDate.value = "<%=today%>";
        }
        function clickUpload() {
            ShowInfo("");
            fn = document.getElementById("<%=FileUp.ClientID%>").value;

            if (fn == "") {
                alert("请选择Excel文件！");
                return;
            }

            if (fn.substring(fn.length - 4).toUpperCase() != ".XLS" && fn.substring(fn.length - 5).toUpperCase() != ".XLSX") {
                alert("文件应为Excel文件！");
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


        function clickUpload_Revise() {
            ShowInfo("");
            fn = document.getElementById("<%=FileUp.ClientID%>").value;

            if (fn == "") {
                alert("请选择Excel文件！");
                return;
            }

            if (fn.substring(fn.length - 4).toUpperCase() != ".XLS" && fn.substring(fn.length - 5).toUpperCase() != ".XLSX") {
                alert("文件应为Excel文件！");
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
            document.getElementById('<%=btnUploadList_Revise.ClientID%>').click();
            return;
        }
        function check() {
            document.getElementById("<%=selectdate.ClientID %>").value = document.getElementById('dCalShipdate').value;
            document.getElementById("<%=selectline.ClientID %>").value = document.getElementById("<%=cmbPdLine.ClientID %>").value;
        }
    </script>


</asp:Content>
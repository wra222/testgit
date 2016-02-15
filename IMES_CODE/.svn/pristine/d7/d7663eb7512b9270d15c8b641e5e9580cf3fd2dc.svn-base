<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="VirtualMOQuery.aspx.cs" Inherits="FA_VirtualMOQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    <Services>
        <asp:ServiceReference Path = "Service/WebServiceVirtualMO.asmx"/>
    </Services>
    </asp:ScriptManager>
    
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>

<div>
    <center >
    <br />
    <table  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
      <tr>
	    <td style="width:12%;height:30px" align="left">
	        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td align="left">
            <iMES:CmbFamilyForDocking ID="CmbFamily1" runat="server"  Width="99" IsPercentage="true"/>
        </td>
        
        <td style="width:12%" align="left">
	        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
        <td align="left" colspan="2">
	        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <input type="text" id="txtModel" style="width:99%;text-transform:uppercase" class="iMes_textbox_input_Yellow"
                     onkeypress="inputNumberAndEnglishChar(this)"    onkeydown=""
                    runat="server" width="99%"  />
                </ContentTemplate>                                  
            </asp:UpdatePanel>
	    </td>
      </tr>
      
    <tr>
        <td style="width:12%;height:30px" align="left">
	        <asp:Label ID="lblStart" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td style="width:30%"  align="left">
            <input type="text" id="StartDate" style="width:100px;" readonly="readonly" class="iMes_textbox_input_Yellow"/>
            <input id="btnStart" type="button" value=".." 
              onclick="showCalendar('StartDate')"  style="width: 17px" class="iMes_button"  />
        </td>
        
        <td style="width:12%" align="left">
	        <asp:Label ID="lblEnd" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td style="width:30%"  align="left" colspan="2">
            <input type="text" id="EndDate" style="width:100px;" readonly="readonly" class="iMes_textbox_input_Yellow"/>
            <input id="btnEnd" type="button" value=".." 
              onclick="showCalendar('EndDate')"  style="width: 17px" class="iMes_button"  />
        </td>
    </tr>
    
    <tr>
	    <td style="width:12%;height:30px" align="left">
	        <asp:Label ID="lblMO" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td align="left">
	        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <input type="text" id="txtMO" style="width:90%;text-transform:uppercase" class="iMes_textbox_input_Yellow"
                     onkeypress="inputNumberAndEnglishChar(this)"    onkeydown=""
                    runat="server" width="99%"  />
                </ContentTemplate>                                  
            </asp:UpdatePanel>
	    </td>
      
	    <td >
            <input id="btnQuery" type="button"  runat="server" onclick="query()" 
            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                style="width:100px; height:auto"  class=" iMes_button" />&nbsp;
        </td>
        <td>
            <input id="btnDel" type="button"  runat="server" onclick="delMo()" 
            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                style="width:100px; height:auto"  class=" iMes_button" />&nbsp;
        </td>
        <td>                
            <input id="btnExecl" type="button" runat="server" onclick="getDateTime();" 
                onserverclick="excelClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'"/>
        </td>
    </tr>
     
    </table>
     
    <hr class="footer_line" style="width:95%"/>
     
    <fieldset style="width:95%">
        <legend id="lblCreatedMOList" runat="server" style="color:Blue" class="iMes_label_13pt"></legend>
        <table width="99%" cellpadding="0" cellspacing="0" border="0" align="center">
        <tr style="width:99%" align="center">
            <td width="99%" align="center" >
                  <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                       <triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGridFresh" EventName="ServerClick" />
                            <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick"></asp:AsyncPostBackTrigger>
                            
                            
                       </triggers>
                       <ContentTemplate>
                           <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" 
                               AutoHighlightScrollByValue="true" GetTemplateValueEnable="true" 
                               GvExtHeight="240px" GvExtWidth="98%" Width="98%" HighLightRowPosition="3" 
                               
                               onrowdatabound="GridViewExt1_RowDataBound" SetTemplateValueEnable="true">
                               <Columns>
                               
                                <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="RowChk" runat="server" onclick="CheckClick(this)"/>
                                </ItemTemplate>
                                </asp:TemplateField>
                                   <asp:BoundField DataField="MO" HeaderStyle-Width="16%" />
                                   <asp:BoundField DataField="Model" HeaderStyle-Width="16%" />
                                   <asp:BoundField DataField="CreateDate" HeaderStyle-Width="20%" />
                                   <asp:BoundField DataField="StartDate" HeaderStyle-Width="20%" />   
                                   <asp:BoundField DataField="Qty"  HeaderStyle-Width="5%"/>
                                   <asp:BoundField DataField="PQty" HeaderStyle-Width="8%" />
                                   <asp:BoundField DataField="CustomerSN_Qty" HeaderStyle-Width="15%" />
                               </Columns>
                           </iMES:GridViewExt>
                       </ContentTemplate>
                </asp:UpdatePanel> 
            </td>
        </tr>
        
        <tr>
            <td>
               <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                      <button id="btnGridFresh" runat="server" type="button" onclick="" style="display:none" onserverclick="FreshGrid"></button>
                      <button id="btnGridClear" runat="server" type="button" onclick="" style="display:none" onserverclick="clearGrid"></button>
              
                      <input type="hidden" runat="server" id="hidStart" />
                      <input type="hidden" runat="server" id="hidEnd" />
                    </ContentTemplate>   
                </asp:UpdatePanel>
            </td>
        </tr>
        </table>
    </fieldset>    
    </center>
</div>

 <script type="text/javascript" language="javascript">

    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgNoFamily = '<%=this.GetLocalResourceObject(Pre + "_msgNoSelectFamily").ToString()%>';
    var msgNoModel = '<%=this.GetLocalResourceObject(Pre + "_msgNoSelectModel").ToString()%>';
    var msgModelNull = '<%=this.GetLocalResourceObject(Pre + "_msgModelNull").ToString()%>';
    var msgNoQueryCond = '<%=this.GetLocalResourceObject(Pre + "_msgNoQueryCond").ToString()%>';
    var msgCheckModel = '<%=this.GetLocalResourceObject(Pre + "_msgCheckModel").ToString()%>';
    var SUCCESSRET ="SUCCESSRET";
    var editor = "<%=UserId%>";
    var customer = "<%=Customer%>";
    var station = '<%=Request["Station"] %>';
    var pCode = '<%=Request["PCode"] %>';
    var accountId = '<%=Request["AccountId"] %>';
    var login = '<%=Request["Login"] %>';
    var model = "";
    var gvClientID = "<%=GridViewExt1.ClientID %>";
    var startDate = document.getElementById('StartDate');
    var endDate = document.getElementById('EndDate');
    var mesNoCheck = '<%=this.GetLocalResourceObject(Pre + "_mesNoCheck").ToString()%>';
    var mesNoDate = '<%=this.GetLocalResourceObject(Pre + "_mesNoDate").ToString()%>';
    var mesDelConfirm = '<%=this.GetLocalResourceObject(Pre + "_mesDelConfirm").ToString()%>';

    window.onload = function() {
        curDay = new Date();
        curDayString = curDay.getFullYear().toString() + '-' + (curDay.getMonth() + 1).toString() + '-' + curDay.getDate().toString();
        startDate.value = endDate.value = curDayString;
        DisplayInfoText();
    }

    function query() {
        ShowInfo("");
        if (startDate.value.toString() == "" || endDate.value.toString() == "") {
            alert(mesNoDate);
            return;
        }
        document.getElementById("<%=hidStart.ClientID%>").value = startDate.value.toString();
        document.getElementById("<%=hidEnd.ClientID%>").value = endDate.value.toString();
        document.getElementById("<%=btnGridFresh.ClientID%>").click();
    }


    function getDateTime() {
        document.getElementById("<%=hidStart.ClientID%>").value = startDate.value.toString();
        document.getElementById("<%=hidEnd.ClientID%>").value = endDate.value.toString();   
    }


    function CheckClick(singleChk, id) {
        try {
            gvTable = document.getElementById(gvClientID);
            if (singleChk.checked) {
                for (var i = 0; i < gvTable.rows.length - 1; i++) {
                    if (gvTable.rows[i + 1].cells[1].innerText != " ") {
                        if (!document.getElementById(gvClientID + "_" + i + "_RowChk").checked) {
                            return;
                        }
                    }
                }
            }
            else {
            }
        } catch (e) {
            alert(e.description);
        }
    }


    function delMo() {
        var checked = false;
        beginWaitingCoverDiv();
        
        gvTable = document.getElementById(gvClientID);
        for (var i = 0; i < gvTable.rows.length - 1; i++) {
            if (document.getElementById(gvClientID + "_" + i + "_RowChk").checked) {
                if (gvTable.rows[i + 1].cells[1].innerHTML != "&nbsp;") {
                    checked = true;
                }
            }
        }
        if (checked == false) {
            endWaitingCoverDiv();
            alert(mesNoCheck);
            return;
        }
        else {
            if (confirm(mesDelConfirm)) {
                for (var i = 0; i < gvTable.rows.length - 1; i++) {
                    if (document.getElementById(gvClientID + "_" + i + "_RowChk").checked) {
                        if (gvTable.rows[i + 1].cells[1].innerHTML != "&nbsp;") {
                            var mo = gvTable.rows[i + 1].cells[1].innerText.trim();
                            WebServiceVirtualMO.DeleteMO(mo, editor, station, customer, onDelSucceed, onDelFail);
                        }
                    }
                }
         
            }
            else {
                endWaitingCoverDiv();
            }
        }
    }

    function onDelSucceed(result) {
        try {
            if (result == null) {
                //service方法没有返回
                endWaitingCoverDiv();
                alert(msgSystemError);
            }
            else if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();
                //ShowSuccessfulInfo(true, "[" + model + "]" + " " + msgSuccess);
                ShowSuccessfulInfo(true);
            }
            else {
                endWaitingCoverDiv();
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
            getMObyModel();
        }
        catch (e) {
            endWaitingCoverDiv();
            alert(e.description);
        }
    }

    function getMObyModel() {
        document.getElementById("<%=btnGridFresh.ClientID%>").click();
    }

    function onDelFail(error) {
        try {
            endWaitingCoverDiv();
            //ResetPage();
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
        }
        catch (e) {
            endWaitingCoverDiv();
            alert(e.description);
        }
    }

    
    function ExitPage()
    {}

    function ResetPage()
    {
        ExitPage();
    }

    function clearTabel() {
        document.getElementById("<%=btnGridClear.ClientID%>").click();
    }

    function MoFocus()
    {
        setMOCmbFocus();
    }

   
    function alertSelectFamily() {
        alert(msgNoFamily);
    }

   
    function alertSelectModel() {
        alert(msgNoModel);
    }

  
    function alertModelNull() {
        alert(msgModelNull);
    }

   
    function alertNoQueryCondAndFocus() {
        alert(msgNoQueryCond);
        setFamilyCmbFocus();
    }

    
     
   </script>


</asp:Content>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="VirtualMO.aspx.cs" Inherits="FA_VirtualMO" %>

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
	    <td style="width:15%; height:30px" align="left" valign="bottom">
	        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td colspan="3" align="left">
            <iMES:CmbFamilyForDocking ID="CmbFamily1" runat="server"  Width="99" IsPercentage="true"/>
        </td>       
      </tr>
      <tr>
	    <td style="width:15%; height:30px" align="left"  valign="bottom">
	        <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td colspan="3" align="left">
	    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
              <triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGridFresh" EventName="ServerClick" />
              </triggers>
                <ContentTemplate>
	        <iMES:CmbModelForDocking ID="CmbModel1" runat="server"  Width="99" IsPercentage="true"/>  	 
	             
	         </ContentTemplate>                                  
         </asp:UpdatePanel>
        </td>
      </tr>
      
      <tr>
	    <td style="width:15%; height:30px" align="left"  valign="bottom">
	        <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td style="width:35%" align="left">
	        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <input type="text" id="txtQty" style="width:95%" class="iMes_textbox_input_Yellow"
                    onkeypress="input1To10000Number(this)" runat="server"  />
                </ContentTemplate>                                  
            </asp:UpdatePanel>
	    </td>
	
        <td style="width:15%"  align="left">
            <asp:Label ID="lblShipdate" runat="server" CssClass="iMes_label_13pt"></asp:Label>
        </td>
        <td style="width:35%"  align="left">
            <input type="text" id="StartDate" style="width:100px;" readonly="readonly" class="iMes_textbox_input_Yellow"/>
            <input id="btnCal" type="button" value=".." 
              onclick="showCalendar('StartDate')"  style="width: 17px" class="iMes_button"  />
             
        </td>
      </tr>
    </table>
     
    <hr class="footer_line" style="width:95%"/>
     
     <table  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
          <tr>
	      <td colspan="1"></td>  
	        <td colspan="3"  align="right">
                <input id="btnQuery" type="button"  runat="server" onclick="QueryMO()" 
                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                    style="width:100px; height:auto"  class=" iMes_button" />&nbsp;
            
                <input id="btnCreate" type="button"  runat="server" onclick="create()" 
                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                    style="width:100px; height:auto"  class=" iMes_button" />&nbsp;&nbsp;
                    
                <input id="btnDel" type="button" runat="server" onclick="delMo()"  
                    onmouseover="this.className='iMes_button_onmouseover'" 
                    onmouseout="this.className='iMes_button_onmouseout'" class="iMes_button" visible="False" />
                    
                <input id="btnToExl" type="button" runat="server" onclick="" 
                onserverclick="excelClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" visible="False" />

             </td>
        </tr>
     </table>
     
     
    <fieldset style="width:95%" align="center">
        <legend id="lblCreatedMOList" runat="server" style="color:Blue" class="iMes_label_13pt"></legend>
        <table width="99%" cellpadding="0" cellspacing="0" border="0" align="center">
        <tr>
            <td align="center" colspan="6">
                  <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                       <triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGridFresh" EventName="ServerClick" />
                       </triggers>
                        <triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick" />
                        </triggers>
                        <triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGridClearGirdview" EventName="ServerClick" />
                        </triggers>
                       <ContentTemplate>
                           <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" 
                               AutoHighlightScrollByValue="true" GetTemplateValueEnable="true" 
                               GvExtHeight="240px" GvExtWidth="98%" Width="98%" HighLightRowPosition="3"
                               OnGvExtRowClick="clickTable(this)" 
                               OnGvExtRowDblClick="" 
                               onrowdatabound="GridViewExt1_RowDataBound" SetTemplateValueEnable="true">
                               <Columns>
                                   <asp:BoundField DataField="MO" HeaderStyle-Width="20%" />
                                   <asp:BoundField DataField="Model" HeaderStyle-Width="20%" />
                                   <asp:BoundField DataField="CreateDate" HeaderStyle-Width="20%" />
                                   <asp:BoundField DataField="StartDate" HeaderStyle-Width="20%" />   
                                   <asp:BoundField DataField="Qty"  HeaderStyle-Width="10%"/>
                                   <asp:BoundField DataField="PQty" HeaderStyle-Width="10%" />
                               </Columns>
                           </iMES:GridViewExt>
                       </ContentTemplate>
                </asp:UpdatePanel> 
            </td>
        </tr>
        
        <tr>
            <td>
               <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
                    <ContentTemplate>
                      <button id="btnGridFresh" runat="server" type="button" onclick="" style="display:none" onserverclick="FreshGrid"></button>
                      <button id="btnGridClear" runat="server" type="button" onclick="" style="display:none" onserverclick="clearGrid"></button>
                      <button id="btnGridClearGirdview" runat="server" type="button" onclick="" style="display:none" onserverclick="clearGridView"></button>
                      <%--<button id="btnGridDownloadMO" runat="server" type="button" onclick="" style="display:none" onserverclick="DownloadMOGrid"></button>--%>                     
                      <%--<button id="btnSetModelFocus" runat="server" type="button" onclick="" style="display:none" onserverclick="SetModelFocus"></button>--%>
                    </ContentTemplate>   
                </asp:UpdatePanel>
            </td>
        </tr>
        </table>
    </fieldset>   
    <br /> 
    </center>
</div>
<script>
   
</script>
 <script type="text/javascript" language="javascript">

     var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgNoFamily = '<%=this.GetLocalResourceObject(Pre + "_msgNoSelectFamily").ToString()%>';
    var msgNoModel = '<%=this.GetLocalResourceObject(Pre + "_msgNoSelectModel").ToString()%>';
    var msgNoQty = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputQty").ToString()%>';
    var msgDownloadSucceed = '<%=this.GetLocalResourceObject(Pre + "_msgDownloadSucceed").ToString()%>';
    var msgModelNull = '<%=this.GetLocalResourceObject(Pre + "_msgModelNull").ToString()%>';
    var msgWrongDate = '<%=this.GetLocalResourceObject(Pre + "_msgWrongDate").ToString()%>';
    var msgCheckModel = '<%=this.GetLocalResourceObject(Pre + "_msgCheckModel").ToString()%>';
    var SUCCESSRET ="SUCCESSRET";
    var editor = "<%=UserId%>";
    var customer = "<%=Customer%>";
    var station = '<%=Request["Station"] %>';
    var pCode = '<%=Request["PCode"] %>';
    var accountId = '<%=Request["AccountId"] %>';
    var login = '<%=Request["Login"] %>';
    var family = "";
    var model = "";
    var todayDate = "<%=today%>";
    var startDate = "";
    var selectedRowIndex = -1;
    var mesDelConfirm = '<%=this.GetLocalResourceObject(Pre + "_mesDelConfirm").ToString()%>';
    var emptyPattern = /^\s*$/;
    var msgNoSelectMo = '<%=this.GetLocalResourceObject(Pre + "_msgNoSelectMo").ToString()%>';

    var checkModelFlag = false;

    window.onload = function() {

    document.getElementById("StartDate").value = "<%=today%>";

    }

    function clickTable(con) {
        if ((selectedRowIndex != null) && (selectedRowIndex != parseInt(con.index, 10))) {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, "<%=GridViewExt1.ClientID %>");
        }

        setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=GridViewExt1.ClientID %>");
        selectedRowIndex = parseInt(con.index, 10);
    }
     
     function Modelclear() {
         getModelCmbObj().value = "";
     }

     function ModelFocus() {
         setModelCmbFocus();
     }

     //function oncheckSucc(result) {
     //    try {
     //            endWaitingCoverDiv();
     //            
     //            if (result == null) {
     //                //service方法没有返回
     //                alert(msgSystemError);
     //                Modelclear();
     //                ModelFocus();
     //            }
     //            else if (result== SUCCESSRET) {
     //                checkModelFlag = true;
     //                qtyFocus();
     //            }
     //            else {
     //                var content = result;
     //                ShowMessage(content);
     //                ShowInfo(content);
     //                Modelclear();
     //                ModelFocus();
     //            }
     //    }
     //    catch (e) {
     //        endWaitingCoverDiv();
     //        alert(e.description);
     //        Modelclear();
     //        ModelFocus();
     //    }
     //}

     function oncheckFail(error) {
         try {
             endWaitingCoverDiv();
          
             ShowMessage(error.get_message());
             ShowInfo(error.get_message());
         }
         catch (e) {
             endWaitingCoverDiv();
             alert(e.description);
         }
         
         Modelclear();
         ModelFocus();
         return false;
     }

     function CheckModelForCreate() {
         model = getModelCmbValue();
         model = model.toUpperCase();
         WebServiceVirtualMO.checkModelinFamily(getFamilyCmbValue(), model, oncheckForCreateSucc, oncheckFail);
     }

     function oncheckForCreateSucc(result) {
         try {
             endWaitingCoverDiv();

             if (result == null) {
                 //service方法没有返回
                 alert(msgSystemError);
                 Modelclear();
                 ModelFocus();

             }
             else if (result == SUCCESSRET) {
                 checkModelFlag = true;
                 createMO();
             }
             else {
                 var content = result;
                 ShowMessage(content);
                 ShowInfo(content);
                 Modelclear();
                 ModelFocus();

             }

         }
         catch (e) {
             endWaitingCoverDiv();
             alert(e.description);
             Modelclear();
             ModelFocus();

         }

     }

    function Calendar_Check() {
        
        startDate = document.getElementById("StartDate").value;
        
        var d1 = "";
        var d2 = "";

        var dateFlag = false;


        d1 = todayDate.substring(0, 4);
        d2 = startDate.substring(0, 4);
        if (d1 > d2) {
            dateFlag=true;
        }
        else if (d1 == d2) {
            d1 = todayDate.substring(5, 7);
            d2 = startDate.substring(5, 7);
            if (d1 > d2) {
                dateFlag=true;
            }
            else if (d1 == d2) {
                d1 = todayDate.substring(8, 10);
                d2 = startDate.substring(8, 10);
                if (d1 > d2) {
                    dateFlag=true;
                }
            }
        }
        if (dateFlag)
        {
         //   document.getElementById("StartDate").value = todayDate;
            startDate = todayDate;
            alert(msgWrongDate);
            return false;
        }
        
        return true;
    }


    function delMo() {
        tblObj = document.getElementById("<%=GridViewExt1.ClientID %>");
        if (selectedRowIndex == -1 || emptyPattern.test(tblObj.rows[selectedRowIndex + 1].cells[0].innerText)) {
            alert(msgNoSelectMo);
            return;
        }     
        
        var mo = tblObj.rows[selectedRowIndex + 1].cells[0].innerText.trim();
        if (confirm(mesDelConfirm)) {
            beginWaitingCoverDiv();
            WebServiceVirtualMO.DeleteMO(mo, editor, station, customer, onDelSucceed, onDelFail);
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
                selectedRowIndex = -1;
                //ShowSuccessfulInfo(true, "[" + model + "]" + " " + msgSuccess);
                ShowSuccessfulInfo(true, "[" + result[1] + "]" + " " + msgSuccess);
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
    

    function create()
    {
        if (Calendar_Check()) {
            var qty = document.getElementById("<%=txtQty.ClientID%>").value;
            
            if (getFamilyCmbValue() == "") {
                alert(msgNoFamily);
                setFamilyCmbFocus();
            }
            else if (qty == "") {
                alert(msgNoQty);
                qtyFocus();
            }
            else if (getModelCmbValue() == "") {
                    alert(msgCheckModel);
                    setModelCmbFocus();
                    ModelFocus();               
            }
            else {
                createMO();
            }
        }
        else {
            return; 
        }
    }

    function createMO() {
        var qty = document.getElementById("<%=txtQty.ClientID%>").value;
        beginWaitingCoverDiv();      
        WebServiceVirtualMO.create(getModelCmbValue(), qty, pCode, startDate, editor, station, customer, onSucceed, onFail);
        //WebServiceVirtualMO.create(model, qty, pCode, startDate, editor, station, customer, onSucceed, onFail);
    }
    
    function onSucceed(result) {
        checkModelFlag = false;
        try 
        {
            if(result==null)
            {
                //service方法没有返回
                endWaitingCoverDiv();
                alert(msgSystemError);                   
            }
            else if((result.length==1)&&(result[0]==SUCCESSRET))
            {      
                endWaitingCoverDiv();
                //ShowSuccessfulInfo(true, "[" + model + "]" + " " + msgSuccess);
                ShowSuccessfulInfo(true, "[" + getModelCmbValue() + "]" + " " + msgSuccess);
            }
            else
            {
                endWaitingCoverDiv();    
                var content =result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
            document.getElementById("<%=txtQty.ClientID%>").value = "";
            
            getMObyModel();

        }
        catch(e) 
        {
            endWaitingCoverDiv();
            alert(e.description);
        }

    }
    
    function onFail(error) {
        checkModelFlag = false;
       try
       {
            endWaitingCoverDiv(); 
            ResetPage();
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
           
        } 
        catch(e) 
        {
            endWaitingCoverDiv();
            alert(e.description);
        }        
    }

    function QueryMO() {
        var url = "VirtualMOQuery.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login;
        window.showModalDialog(url, "", 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ExitPage
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	中断Session
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ExitPage()
    {}

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ResetPage
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	页面重置
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    function ResetPage()
    {
        ExitPage();
        endWaitingCoverDiv();
        ShowInfo("");
        model = "";
        getModelCmbObj().value = "";
        document.getElementById("<%=txtQty.ClientID%>").value = "";
        document.getElementById("<%=btnGridClear.ClientID%>").click();
        
    }

    function getMObyModel() 
    {
        document.getElementById("<%=btnGridFresh.ClientID%>").click();
    }

    function qtyFocus()
    {
        document.getElementById("<%=txtQty.ClientID%>").focus();
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

    function clearTabel() {
        document.getElementById("<%=btnGridClearGirdview.ClientID%>").click();
    }
    
    </script>


</asp:Content>
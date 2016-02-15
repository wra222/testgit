<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Virtual MO
 * UI:CI-MES12-SPEC-FA-UI Virtual MO.docx 
 * UC:CI-MES12-SPEC-FA-UC Virtual MO.docx          
 * Update: 
 * Date        Name                 Reason 
 * ==========  ==================== =====================================
 * 2011-11-30   Chen Xu             Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	Add Qty of Virtual MO
 *                2.	Add New Virtual MO
 * UC Revision: 6789
 */
--%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="VirtualMO.aspx.cs" Inherits="FA_VirtualMO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    <Services>
        <asp:ServiceReference Path=  "~/FA/Service/WebServiceVirtualMO.asmx"/>
    </Services>
    </asp:ScriptManager>
    <link rel="stylesheet" type="text/css" href="../CommonControl/jquery/css/smoothness/jquery-ui-1.8.18.custom.css" /> 
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
<%--    <script language="JavaScript" type="text/javascript" src= "../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/jquery/js/jquery-ui-1.8.18.custom.min.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/jquery/js/jquery-ui-combobox.src.js"></script>  
   
 <style>
     input.ui-combobox
     {
        width: 95%;
        color:Black;
	    font-family:Verdana;
	    font-size: 9pt;
	    background-color:RGB(242,254,230);
        border-style:none;
        text-align: left;
        vertical-align:middle;
        height: 20px;
        line-height: 20px;
	    padding:1px;
	    margin-top:1px;
	    margin-bottom:1px;
	    margin-right:1px;
	    margin-left:1px;
	    text-transform:uppercase;
     }
    button.ui-combobox
    {
        height: 1.40em;
        width: 1.5em;
        vertical-align:middle;
        padding:1px;
	    margin-top:1px;
	    margin-bottom:1px;
	    margin-right:1px;
	    margin-left:1px;
    }
</style>  
<script>

    $(function() {
    $("#<%=CmbModel1.ClientID%>_DropDownList1").combobox();
    });
</script>--%>

 <div>
    <center >
    <br />
    <table  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
      <tr>
	    <td style="width:15%; height:30px" align="left" valign="bottom">
	        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td colspan="3" align="left">
            <iMES:CmbFamily ID="CmbFamily1" runat="server"  Width="99" IsPercentage="true"/>
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
	        <%--<iMES:CmbModel ID="CmbModel1" runat="server"  Width="99" IsPercentage="true"/>  	--%> 
	        <input type="text" id="txtModel" style="ime-mode:disabled;width:300px" class="iMes_textbox_input_Yellow"
                    MaxLength="50" onkeypress="inputNumberAndEnglishChar(this)"  onkeydown="TabModel()"             
                    runat="server" width="99%"  />
	             
	         </ContentTemplate>                                  
         </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
	    <td style="width:15%; height:30px" align="left"  valign="bottom">
	        <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td style="width:35%" align="left">
	        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                <ContentTemplate>
                  <input type="text" id="txtQty" style="ime-mode:disabled;width:300px" class="iMes_textbox_input_Yellow"
                    onkeypress="input1To10000Number(this)" 
                    runat="server" width="99%"  />
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
                
                <input id="btnDownloadMO" type="button"  runat="server" onclick="DownloadMO()" 
                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                    style="width:150px; height:auto"  class=" iMes_button" />&nbsp;
            
                <input id="btnCreate" type="button"  runat="server" onclick="create()" 
                onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                    style="width:100px; height:auto"  class=" iMes_button" />&nbsp;&nbsp;
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
                      <%--<input id="hidMOPrefix" type="hidden" runat="server" />--%>
                      
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
    var MOPrefix = '<%=Request["MOPrefix"] %>';
    var jobname = "<%=JobName%>";
    var checkModelFlag = false;

    window.onload = function() {

    document.getElementById("StartDate").value = "<%=today%>";

    }

    
    function TabModel() {
        checkModelFlag = false;
         if (getFamilyCmbValue() == "") {
                alert(msgNoFamily);
                setFamilyCmbFocus();
                event.returnValue = false;
         }
         else if (event.keyCode == 9 || event.keyCode==13) {
            if (document.getElementById("<%=txtModel.ClientID%>").value != "") {
                CheckModel();
                event.returnValue = false;
              }
         }
     }

     function CheckModel() {
         model = document.getElementById("<%=txtModel.ClientID%>").value;
         model = model.toUpperCase();
         WebServiceVirtualMO.checkModelinFamily(getFamilyCmbValue(), model, oncheckSucc, oncheckFail);
     }
     
     function Modelclear() {
         document.getElementById("<%=txtModel.ClientID%>").value = "";
     }

     function ModelFocus() {
         document.getElementById("<%=txtModel.ClientID%>").focus();
     }

     function oncheckSucc(result) {
         try {
                 endWaitingCoverDiv();
                 
                 if (result == null) {
                     //service方法没有返回
                     alert(msgSystemError);
                     Modelclear();
                     ModelFocus();
                    
                 }
                 else if (result== SUCCESSRET) {
                     checkModelFlag = true;
                     qtyFocus();
                    
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
         model = document.getElementById("<%=txtModel.ClientID%>").value;
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

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	Calendar_Check
    //| Author		:	Chen Xu
    //| Create Date	:	2/17/2012
    //| Description	:	日历检查
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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

    

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	create
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	创建虚拟MO，供临时或紧急生产时使用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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
            //             else if (getModelCmbValue() == "") {
            else if (!checkModelFlag) {
                if (document.getElementById("<%=txtModel.ClientID%>").value == "") {
                    alert(msgCheckModel);
                    // setModelCmbFocus();
                    ModelFocus();
                }
                else  {
                    CheckModelForCreate();
                }
                
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
        //var aaa = MOPrefix;
        //  WebServiceVirtualMO.create(getModelCmbValue(), qty, pCode, startDate, editor, station, customer, onSucceed, onFail);
        WebServiceVirtualMO.create(model, qty, pCode, startDate, editor, station, customer, MOPrefix, onSucceed, onFail);
    }
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSucceed
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	create调用web service成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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
                ShowSuccessfulInfo(true, "[" + model + "]" + " " + msgSuccess);
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
        
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFail
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	create调用web service失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	QueryMO
    //| Author		:	Chen Xu
    //| Create Date	:	2/21/2012
    //| Description	:	弹出查询窗口，查询条件为 Family, Model,MO,查询结果只显示未打印完MO
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function QueryMO() {
        var url = "VirtualMOQuery.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login;
        window.showModalDialog(url, "", 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	Auto Download MO
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	调用产线提供的Download Mo接口（ .dtsx格式 ），把最新的Mo上传到IMES系统
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function DownloadMO() {
       
        beginWaitingCoverDiv();
        WebServiceVirtualMO.autoDownloadMO(jobname, onDownloadSucceed, onDownloadFail);
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onDownloadSucceed
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	提示Download Succeed信息
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onDownloadSucceed(result) {
        try {
            endWaitingCoverDiv();
            if (result == null) {
                //service方法没有返回
                alert(msgSystemError);

            }
            else if (result == SUCCESSRET) {
            ShowSuccessfulInfo(true, msgSuccess);
            }
            else {
                ShowMessage(result[0]);
                ShowInfo(result[0]);
            }
        } 
        catch(e) 
        {
            endWaitingCoverDiv();
            alert(e.description);
        } 
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onDownloadFail
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	create调用web service失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onDownloadFail(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

        }
        catch (e) {
            endWaitingCoverDiv();
            alert(e.description);
        }
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
        document.getElementById("<%=txtModel.ClientID%>").value = "";
        document.getElementById("<%=txtQty.ClientID%>").value = "";
        document.getElementById("<%=btnGridClear.ClientID%>").click();
        
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	getMObyModel
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	刷新表格
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function getMObyModel() 
    {

        document.getElementById("<%=btnGridFresh.ClientID%>").click();
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	qtyFocus
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	置焦点到QTY
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function qtyFocus()
    {
        document.getElementById("<%=txtQty.ClientID%>").focus();
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	alertSelectFamily
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	提示错误信息
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function alertSelectFamily() {
        alert(msgNoFamily);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	alertSelectModel
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	提示错误信息
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function alertSelectModel() {
        alert(msgNoModel);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	alertModelNull
    //| Author		:	Chen Xu
    //| Create Date	:	12/02/2011
    //| Description	:	提示错误信息
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function alertModelNull() {
        alert(msgModelNull);
    }

    function clearTabel() {
        document.getElementById("<%=btnGridClearGirdview.ClientID%>").click();
    }
    
    </script>


</asp:Content>
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
 * UC 具体业务：  1.	Add Mo of Virtual MO
 *                2.	Add New Virtual MO
 * UC Revision: 6789
 */
--%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="VirtualMOQuery.aspx.cs" Inherits="FA_VirtualMOQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    <Services>
        <asp:ServiceReference Path=  "~/FA/Service/WebServiceVirtualMO.asmx"/>
    </Services>
    </asp:ScriptManager>
    <link rel="stylesheet" type="text/css" href="../CommonControl/jquery/css/smoothness/jquery-ui-1.8.18.custom.css" /> 
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <%--<script language="JavaScript" type="text/javascript" src= "../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
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
     };
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
</script>
--%>

<div>
    <center >
    <br />
    <table  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
      <tr>
	    <td style="width:15%; height:30px" align="left" valign="bottom">
	        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td colspan="2" align="left">
            <iMES:CmbFamily ID="CmbFamily1" runat="server"  Width="99" IsPercentage="true"/>
        </td>       
    </tr>
    <tr>
	    <td style="width:15%; height:30px" align="left"  valign="bottom">
	        <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td colspan="2" align="left">
	    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
               <triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGridFresh" EventName="ServerClick" />
              </triggers>
                <ContentTemplate>
	        <%--<iMES:CmbModel ID="CmbModel1" runat="server"  Width="99" IsPercentage="true"/>--%>  
	                <input type="text" id="txtModel" style="ime-mode:disabled;width:300px" class="iMes_textbox_input_Yellow"
                    MaxLength="50" onkeypress="inputNumberAndEnglishChar(this)"            
                    runat="server" width="99%"  />
	         </ContentTemplate>                                  
         </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
	    <td style="width:15%; height:30px" align="left"  valign="bottom">
	        <asp:Label ID="lblMO" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </td>
	    <td style="width:35%" align="left">
	        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                <ContentTemplate>
                  <input type="text" id="txtMO" style="ime-mode:disabled;width:300px" class="iMes_textbox_input_Yellow"
                    MaxLength="50"  onkeypress="inputNumberAndEnglishChar(this)"    onkeydown="CheckMO()"
                    runat="server" width="99%"  />
                </ContentTemplate>                                  
         </asp:UpdatePanel>
	   </td>
      
	    <td align="right">
            <input id="btnQuery" type="button"  runat="server" onclick="query()" 
            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                style="width:100px; height:auto"  class=" iMes_button" />&nbsp;
         </td>
    </tr>
    <tr>
        <td colspan="4"> &nbsp; </td>
    </tr>
     
    </table>
     
    <hr class="footer_line" style="width:95%"/>
     
    <fieldset style="width:95%" align="center">
        <legend id="lblCreatedMOList" runat="server" style="color:Blue" class="iMes_label_13pt"></legend>
        <table width="99%" cellpadding="0" cellspacing="0" border="0" align="center">
        <tr width="99%" align="center">
            <td width="99%" align="center" >
                  <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                       <triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGridFresh" EventName="ServerClick" />
                       </triggers>
                       <triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick" />
                        </triggers>
                       <ContentTemplate>
                           <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" 
                               AutoHighlightScrollByValue="true" GetTemplateValueEnable="true" 
                               GvExtHeight="240px" GvExtWidth="98%" Width="98%" HighLightRowPosition="3" 
                               
                               onrowdatabound="GridViewExt1_RowDataBound" SetTemplateValueEnable="true">
                               <Columns>
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
               <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
                    <ContentTemplate>
                      <button id="btnGridFresh" runat="server" type="button" onclick="" style="display:none" onserverclick="FreshGrid"></button>
                      <button id="btnGridClear" runat="server" type="button" onclick="" style="display:none" onserverclick="clearGrid"></button>
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
    
    window.onload = function() {
        DisplayInfoText();
    }

    function CheckMO() {
        //  ITC-1360-1197
        if (event.keyCode == 9 || event.keyCode == 13) {
            query();
            event.returnValue = false;
        }
    }

    function CheckModel() {
        if (event.keyCode == 9 || event.keyCode == 13) {
            if (getFamilyCmbValue() == "") {
              //  alert(msgNoFamily);
                //  setFamilyCmbFocus();
                WebServiceVirtualMO.checkModel( model, oncheckSucc, oncheckFail);
                event.returnValue = false;
            }
            else if (document.getElementById("<%=txtModel.ClientID%>").value != "") {
                model = document.getElementById("<%=txtModel.ClientID%>").value;
                model = model.toUpperCase();
                WebServiceVirtualMO.checkModelinFamily(getFamilyCmbValue(), model, oncheckSucc, oncheckFail);
                event.returnValue = false;
            }
        }
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
            else if (result == SUCCESSRET) {
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
            //   ResetPage();
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

        }
        catch (e) {
            endWaitingCoverDiv();
            alert(e.description);
        }

        Modelclear();
        ModelFocus();
    }
    
   
    function query() {
        
        document.getElementById("<%=btnGridFresh.ClientID%>").click();
    }
    
  
    function ExitPage()
    {}

   
    function ResetPage()
    {
        ExitPage();
    }

 
    function getMO() 
    {
        document.getElementById("<%=btnGridFresh.ClientID%>").click();
    }

    function clearTabel() {
        document.getElementById("<%=btnGridClear.ClientID%>").click();
    }

    function MoFocus()
    {
        document.getElementById("<%=txtMO.ClientID%>").focus();
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
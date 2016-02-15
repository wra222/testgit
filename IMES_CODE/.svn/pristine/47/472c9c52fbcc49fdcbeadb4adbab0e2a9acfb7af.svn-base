<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * 
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PCAOQCCollection.aspx.cs" Inherits="PCAOQCCollection" Title="PCA OQC Collection" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
	<script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
             <table width="100%" border="0">
             
                <tr>
                    <td style="width:10%" align="left">
                        <asp:Label ID="lblPdline" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100"  IsPercentage="true" />
                    </td>
                </tr> 
                <tr>
                  <td style="width:10%" align="left">
                          <asp:Label ID="lblPassQty" runat="server" CssClass="iMes_label_13pt" Text="OQC Type:"></asp:Label>
                    </td> 
                    <td colspan="3">
                         <select id="selType" style="width:100%">
                            <option>SA OQC</option>
                             <option>FRU</option>
                             <option>RCTO</option>
                        </select>
                    </td>
                           
                </tr>
                <tr>
                   <td style="width:10%" align="left">
                          <asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt" Text="MBSno:"></asp:Label>
                   </td>
                   <td style="width:30%" align="left" id="tdMBSno" >
                   
                   </td>
                   <td style="width:10%" align="left">
                          <asp:Label ID="lblECR" runat="server" CssClass="iMes_label_13pt" Text="ECR:"></asp:Label>
                   </td>
                   <td style="width:50%" align="left" id="tdECR" >
                   
                   </td>
                </tr>  
                 <tr>
                   <td style="width:10%" align="left">
                          <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt" Text="Model:"></asp:Label>
                   </td>
                   <td align="left" id="tdModel" style="width:30%">
                   
                   </td>
                   <td style="width:10%" align="left">
                          <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt" Text="Family:"></asp:Label>
                   </td>
                   <td style="width:50%" align="left" id="tdFamily" >
                   
                   </td>
                </tr>  
                <tr>
                    <td style="width:10%" align="left">
                     <asp:Label ID="lblNGOK" runat="server" CssClass="iMes_label_13pt" Text="NG/OK:"></asp:Label>
                  </td>
                    <td align="left" style="width:30%">
                     <select id="selNGOK" style="width:100%">
                            <option>NG</option>
                             <option>OK</option>
                       </select>
                  </td>
                  <td style="width:10%" align="left">
                        <asp:Label ID="lblCause" runat="server" CssClass="iMes_label_13pt" Text="Cause:"></asp:Label>
                  </td>
                  <td style="width:50%" align="left">
                   <iMES:CmbDefect ID="CmbDefect" runat="server" Width="99%" />
                  </td>
                  
                </tr>
         <tr>
           <td style="width:10%" align="left">
                     <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt" Text="Remark:"></asp:Label>
          </td>
          <td colspan="3">
              <input id="txtRemark" type="text" style="width:99%" />
          </td>
         
         </tr>
             </table>
             
                        
        </div>
        <div id="div3">
             <table width="100%">
               
                <tr>
                    <td style="width:10%" align="left">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel" Text="Data Entry:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                  
                </tr>
             </table>   
        </div> 
       
        
        <asp:UpdatePanel ID="updHidden" runat="server" RenderMode="Inline" UpdateMode="Always">
       
            <ContentTemplate>
                <input id="TypeValue" type="hidden" runat="server" />
                <input id="LimitSpeed" type="hidden" runat="server" />
                <input id="HoldStation" type="hidden" runat="server" />
                <input id="SpeedExpression" type="hidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>   
   
    <script language="javascript" type="text/javascript">
       
        var editor;
        var customer;
		var station;
        var mbNo="";
        //error message
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputDefect").ToString() %>';
        var msgPrestationError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrestationError").ToString() %>';
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoSelectPdLine").ToString() %>';
        var msgPcidCheck = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPcidCheck").ToString() %>'

        
        window.onload = function()
        {
    
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
    
            inputObj.focus();
        };

        function input(data) 
        {
            ShowInfo("");
         
            if (getPdLineCmbValue() == "") 
            {
                alert(mesNoSelPdLine);
                setPdLineCmbFocus();
                getAvailableData("input");
                return;
            }
            var line = getPdLineCmbValue();
            if (mbNo == "") {
                mbNo = data;
                beginWaitingCoverDiv();
                PageMethods.InputMBNo(data, line, editor, station, customer, onInputMBNoSucess, onInputMBNoError);
            }
            else
            {
                if (data != "9999") {
                    alert("Please scan 9999");
                    getAvailableData("input");
                  
                }
                else {
                    var ngok = $("#selNGOK :selected").text();
                    if (ngok=="NG" && getDefectCmbText() == "") {
                        alert("Please select defect code!");
                        getAvailableData("input"); 
                        }
                        else {
                            beginWaitingCoverDiv();
                            Save(); }
                 }
            }
        }

        function onInputMBNoSucess(result) {
            //ECR Model Family
            ShowInfo("");
            endWaitingCoverDiv();
            tdMBSno
            $("#tdMBSno").text(mbNo);
            $("#tdECR").text(result[0]);
            $("#tdModel").text(result[1]);
            $("#tdFamily").text(result[2]);
            ShowInfo("Please scan 9999", "green");
//       
            getAvailableData("input");
            inputObj.focus();
        }

        function onInputMBNoError(result) {
            mbNo = "";
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
            inputObj.focus();
        }
        
     

        function Save() 
        {
            var type = $("#selType :selected").text(); //getDefectCmbValue()
            var ngok = $("#selNGOK :selected").text();
            var defect = getDefectCmbValue();
            var remark = $("#txtRemark").val();
            PageMethods.Save(mbNo, type, ngok, defect, remark,onSaveSucess, onSaveError);
        
        }

        function onSaveSucess(result) 
        {
            
            
            endWaitingCoverDiv();
            var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
            ShowInfo(msgSuccess, "green");
         
            
            initPage();
            getAvailableData("input");
            inputObj.focus();
        }
        
        function initPage()
        {
            $("#tdMBSno").text("");
            $("#tdECR").text("");
            $("#tdModel").text("");
            $("#tdFamily").text("");
            mbNo = "";
            $("#txtRemark").val("");
        }

        function onSaveError(result)
        {
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            getAvailableData("input");
          //  initPage();
            inputObj.focus();
        }
        
       
        
        window.onbeforeunload = function()
        {
            if (mbNo!="")
            {
                OnCancel();
            }
        };   
        
        function OnCancel()
        {
            PageMethods.Cancel(mbNo);
            ResetPage();
        }
    
        function ResetPage(){
         
            initPage();
            ShowInfo("");
        }                  
  
  
       
    </script>
</asp:Content>


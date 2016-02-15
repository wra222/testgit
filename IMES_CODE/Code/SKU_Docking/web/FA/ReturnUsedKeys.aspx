<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Final Scan page
 * UI:CI-MES12-SPEC-PAK-UI Final Scan.docx --2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC Final Scan.docx --2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-10  Zhang.Kai-sheng       (Reference Ebook SourceCode) Create
 * TODO:

 * Known issues:
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReturnUsedKeys.aspx.cs" Inherits="PAK_ReturnUsedKeys" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" ></bgsound>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/FA/Service/ReturnUsedKeysService.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                
                <tr>
                 <td  style="width: 25%;">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td style="width: 75%;">
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="90%" IsClear="true" IsPaste="true" />
                    </td>
                </tr>
                <tr>
                <td  style="width: 25%;">
                        <asp:Label ID="lbInserFile" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    
                    <td  style="width:75%"> 
                         <iframe name="action" id="action" src="ReturnUsedKeys_Upload.aspx"  scrolling="no"  frameborder="0" width="90%" height="50px" ></iframe>
                    </td>
                </tr>
            </table>
            <hr />
            <div>       
                    <table width="100%" border="0" style="table-layout: fixed;margin: 0 auto;">
                        <tr>
                            <td style="width: 110px;">
                                <asp:Label ID="lblCustList" runat="server" CssClass="iMes_label_13pt" 
                                    Font-Bold="True" Font-Size="15px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="upPanl" runat="server">
                                    <ContentTemplate>
                                        <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                                            GvExtWidth="100%" GvExtHeight="228px" style="top: 0px; left: 0px" Width="100%" Height="220px" SetTemplateValueEnable="False" HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                                        </iMES:GridViewExt> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                        <td>
                    <input type="hidden" runat="server" id="hiddenCostCenter" name="hiddenCostCenter" />
                    </td>
                    </tr>
                    </table>                
                 
            </div>              
        </div>
        <div>
            <table width="100%">
                <tr>
                   <tr>
                    <td style="width: 20%;">
                        
                    </td>
                     
                  
                     <td style="width: 40%;">
                    </td>  
              
                     <td colspan="4" align ="left" style="width: 30%;">
                         <input id="refresh" type="button" runat="server" class="iMes_button" onclick="refresh()" style="width:150px"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                        <input id="returnKey" type="button" runat="server" class="iMes_button" onclick="save()" style="width:150px"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                        
                    </td>                    
                </tr>
                    
                </tr>
            </table>            
        </div>      
            
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Always">
            <ContentTemplate>
                <input id="btnUploadOver" type="button" onclick="" onserverclick="uploadOver" style="display:
                none" runat="server" />
                 <input type="hidden" id="hidRowCount" runat="server" />

            </ContentTemplate>   
            </asp:UpdatePanel>  
    </div>    
    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var editor;
        var tbl;
        var DEFAULT_ROW_NUM = 9; //13; //9?
        var strRowsCount = "<%=DEFAULT_ROWS%>";
        var initRowsCount = DEFAULT_ROW_NUM;
        var customer;
        var stationId;
        var inputObj;     
        var emptyPattern = /^\s*$/;
        var emptyString = "";
        var gKeyPickID = "";
        var gKeyPalletNo = "";
        var gKeychepPallet = "";
        var bNeedCheckChep = false;
        var msgExistPalletNo = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgExistPalletNo").ToString() %>';
        var msgWrongCode = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgWrongCode").ToString() %>';
        var msgNotExistChep = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgNotExistChep").ToString() %>';
        var msgInputChepplt = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputChepplt").ToString() %>';
        var msgOutpalletNo = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgOutpalletNo").ToString() %>';
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSuccess") %>';
        var index = 1;
        ///---------------------------------------------------
        ///| Name		    :	window.onload
        ///| Description	:	
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
        window.onload = function()
        {
            //Get env value Setup Input Object Event;
            tbl = "<%=gd.ClientID %>";
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';

            inputObj = getCommonInputObject();
            getAvailableData("Input_txtEntry");

        };
        
        var iSelectedRowIndex = null;
        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) ) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");     //去掉过去高亮行           
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");     //设置当前高亮行
            iSelectedRowIndex = parseInt(con.index, 10);    //记住当前高亮行
        }

        function checkInput(data) {
            if (data.length == 10) {  //10
                //if (data.substring(0, 3) == "CNU")
				if (CheckCustomerSN(data))
                    return data;
            }
            return '0';
        }
        function Input_txtEntry(data)
        {           
            if(data == "7777")
            {
                ResetPage();
                ShowInfo("");
                resetUI();
                getAvailableData("Input_txtEntry");
                inputObj.focus();
                return true;                
            }
            strCustSN = checkInput(data);
            if (strCustSN == '0') {
                alert("wrong code");
                ShowInfo(msgWrongCode);
                getAvailableData("Input_txtEntry");
                inputObj.focus();
                return true;
            }  

            ShowInfo("");
            beginWaitingCoverDiv();
            ReturnUsedKeysService.Check(strCustSN, editor, stationId, customer, CheckSucc, CheckFail);

        }


        function CheckSucc(result) 
        {
            endWaitingCoverDiv();
            if (result[0] == null || result[0].length == 0)   //return NULL
            {
                getAvailableData("Input_txtEntry");
                inputObj.focus();
            }
            else 
            {
                ShowInfo("");
                if (isInTable(result[0])) {
                    ShowInfo("CustSn:"+result[0]+msgExistPalletNo);
                }
                else
                    addOrChange(result);

                getAvailableData("Input_txtEntry");
                inputObj.focus();    
            }
        }

        function CheckFail(error) 
        {
            endWaitingCoverDiv();
            //ShowMessage(msgExistPalletNo);
            //PlaySoundFail();
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
            getAvailableData("Input_txtEntry");
            inputObj.focus();
        }

        function finish(param) {
            //writeToSuccessMessage(errorMsg);
            document.getElementById("<%=hiddenCostCenter.ClientID%>").value = param;
            document.getElementById("<%=btnUploadOver.ClientID%>").click();
        }

  
        var GridViewExt1ClientID = "<%=gd.ClientID %>";

        ///---------------------------------------------------
        ///| Name		    :	addOrChange
        ///| Description	:	update table Info 
        ///| Input para.	:	
        ///| Ret value      :
        ///---------------------------------------------------
        function addOrChange(lstRow)
        {
            
            var rowArray = new Array();

            rowArray.push(lstRow[0]);
            rowArray.push(lstRow[1]);
            rowArray.push("");

            AddRowInfo(rowArray);

            var gdObj = document.getElementById("<%=gd.ClientID %>");
            var con = gdObj.rows[index-1]; //index从1开始
             //高亮当前行
            setGdHighLight(con);

        }
        function AddRowInfo(RowArray) {
            try {
                if (index < initRowsCount) {
                    eval("ChangeCvExtRowByIndex_" + GridViewExt1ClientID + "(RowArray,false, index)");
                }
                else {
                    eval("AddCvExtRowToBottom_" + GridViewExt1ClientID + "(RowArray,false)");
                }
                setSrollByIndex(index, false);
                index++;

            }
            catch (e) {
                ShowInfo(e.description);
                PlaySound();
            }
        }

        
        function isInTable(str)
        {
            //judge custom sn in Table?
            var tblObj = document.getElementById("<%=gd.ClientID %>");
            var length = tblObj.rows.length;
            
            for (var i = 1; i < length; i++)
            {
                if (tblObj.rows[i].cells[0].innerText.trim() == str)
                {
                    return true;
                }
            }
            return false;
        }

        function getTableContent() {
            var content = new Array( );
            var custList = new Array();
            var pnList = new Array();
            var errList = new Array();
            
            var tblObj = document.getElementById("<%=gd.ClientID %>");
            var length = tblObj.rows.length;

            for (var i = 1; i < length; i++) {
                if (tblObj.rows[i].cells[0].innerText.trim() == "") {
                    break;
                }
                custList.push(tblObj.rows[i].cells[0].innerText.trim());
                pnList.push(tblObj.rows[i].cells[1].innerText.trim());
                errList.push("Pass");
            }
            content.push(custList);
            content.push(pnList);
            content.push(errList);
            return content;
        }
        
        function refresh() {
            
            ShowInfo("");
            resetUI();
            getAvailableData("Input_txtEntry");
            inputObj.focus();
        }

        function save() {
            if (confirm("真的要回退ECOA?")) {
                var content = getTableContent();
                var errList = new Array();
                if (content[0].length > 0) {
                    beginWaitingCoverDiv();
                    ReturnUsedKeysService.Save(content[0], content[1], content[2], editor, stationId, customer, SaveSucc, SaveFail);
                }
                else {
                    ShowInfo("请Load CustSN!");
                    getAvailableData("Input_txtEntry");
                    inputObj.focus();
                }
            }
            else {
                ShowInfo("");
                getAvailableData("Input_txtEntry");
                inputObj.focus();
            }
        }
        function SaveSucc(result) {
            endWaitingCoverDiv();
            resetUI();
            iSelectedRowIndex = null;    
            ShowInfo("return used key successed!");
        }

        function SaveFail(error) {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
            iSelectedRowIndex = null;    
            getAvailableData("Input_txtEntry");
            inputObj.focus();
        }
        function resetUI()
        {
            //Set Input symbol
            inputFlag = false;
            index = 1;
            iSelectedRowIndex = null;    
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            inputObj.focus();
        }
        function fileProcess() {
          
            endWaitingCoverDiv();
        }
        function ClearIndex(n) {

            index = parseInt(n) + 1;
            iSelectedRowIndex = null;     
        }
        function ClearTable() {
            index = 1;
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
        }
        function fileProcess2() {
            ShowMessage("Upload Success!");
            ShowSuccessfulInfo(true, "Upload Success!");
        }

        window.onbeforeunload = function()
        {
            if (inputFlag)
            {
                OnCancel();
            }
        };
        
        function OnCancel()
        {
            
        }

        function ExitPage()
        {
            OnCancel();
        }

        function ResetPage()
        {
            ExitPage();
            ShowInfo("");
        }
       

    </script>
</asp:Content>


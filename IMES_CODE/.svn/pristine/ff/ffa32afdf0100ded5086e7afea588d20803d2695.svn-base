<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Iutput
* UI:CI-MES12-SPEC-PAK-UC PAQC Input.docx –2011/10/20 
* UC:CI-MES12-SPEC-PAK-UC PAQC Input.docx –2011/10/20            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   liuqingbiao           Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="COARemoval.aspx.cs" Inherits="PAK_COARemoval" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="Service/WebServiceCOARemoval.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%">
                <tr>
                    <td style="width: 15%">
                        <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td width="65%">
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                            Width="60%" IsClear="true" IsPaste="true" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  
                            ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"  />
                    </td>
                                     
                </tr>
                <tr><td style="width:15%">&nbsp;</td><td><input type="hidden" runat="server" id="hiddenCostCenter" name="hiddenCostCenter" /></td></tr>

 			    <tr>					   	    
                    <td style="width: 15%">
                        <asp:Label ID="lblFile" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                    </td>
	                <td style="width:65%" align="left">
                        <iframe name="action" id="action" src="COARemove_Upload.aspx"  scrolling="no"  frameborder="0" width="100%" height="50px" ></iframe>
	                </td>
                </tr>				
               
                
                
            </table>

            <hr />
            
            <asp:Label ID="lblRepairList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate>
	                       
	                                <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" 
                                        GetTemplateValueEnable="False" GvExtHeight="300px" Height="300px" 
                                        GvExtWidth="100%" OnGvExtRowClick=""
                                        OnGvExtRowDblClick="" SetTemplateValueEnable="False" 
                                        HighLightRowPosition="1" HorizontalAlign="Left"
                                        UseAccessibleHeader="False" ShowHeader="True">                                     
                                        <Columns>
                                            <asp:BoundField DataField="Action" />
                                            <asp:BoundField DataField="COANo" />
                                            <asp:BoundField DataField="Cause" />
                                       </Columns>
                                    </iMES:GridViewExt>

                            </ContentTemplate>   
                            </asp:UpdatePanel>
                            
                        </td>
                        <td align="left">
                        <fieldset style="border-color:InactiveCaption; border-width:thin" class="iMes_div_MainTainEdit">
                <legend><asp:Label ID="lblChooseAction" runat="server" CssClass="iMes_label_13pt">  </asp:Label> </legend>

                <table border="0" style="height: 70px; width: 100%" >
                    <tr>
                        <%--cal1--%>
                         <%--modify ITC-1413-0096 BUG--%>
                     
                        <td>
                             <input type="radio" name="optProduct" id="radScrap" checked runat="server" /><asp:Label ID="lblScrap" runat="server" CssClass="iMes_label_13pt" ></asp:Label>&nbsp;&nbsp;
                             <input type="radio" name="optProduct" id="radRemove" runat="server" /><asp:Label ID="lblRemove" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <%--
                        <td>
                            <input type="radio" id="radScrap" name="optProduct" runat="server"  onclick="" />
                        </td>
                        <td style="width: 65px">
                            <asp:Label ID="lblScrap" runat="server" CssClass="iMes_label_13pt" ></asp:Label>           
                        </td>
                        <td style="width: 25px">
                            <input type="radio" id="radRemove" name="optProduct" runat="server"  onclick="" />
                        </td>
                        <td style="width: 240px">
                            <asp:Label ID="lblRemove" runat="server" CssClass="iMes_label_13pt"></asp:Label>
 
                        </td>
                        --%>
                    </tr>
                </table>
                
                <table border="0" width: "100%">
                    <tr>
                        <td width="15%">
                        <asp:Label ID="lblCause" runat="server" class="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="2" align="right">
                    
                        <iMES:CmbCauseItem ID="cmbCauseItem" runat="server" Width="80%"/>
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                        </td>
                     
                        <td colspan="2" align="right">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input id="btnSave" type="button" runat="server" 
                                    class="iMes_button" onclick="OnButtonSave()" 
                                    onmouseover="" onmouseout="" align="bottom" />
                         </td>                      
                    </tr>
                </table>
            </fieldset>
                        </td>
                    </tr>
                    <tr>

                            <td>
                                <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Always">
                                <ContentTemplate>
                                <input id="btnUploadOver" type="button" onclick="" onserverclick="uploadOver" style="display:
                                none" runat="server" />

                                </ContentTemplate>   
                                </asp:UpdatePanel> 
  
                            </td>    
                    </tr>
                </table>
                
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var msgInputWrongCode = '<%=this.GetLocalResourceObject(Pre + "_msgInputWrongCode").ToString()%>';
        var msgDuplicatedData = '<%=this.GetLocalResourceObject(Pre + "_msgDuplicatedData").ToString()%>';
        var msgSaveEmptyList = '<%=this.GetLocalResourceObject(Pre + "_msgSaveEmptyList").ToString()%>';
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgNoSelectCause = '<%=this.GetLocalResourceObject(Pre + "_msgNoSelectCause").ToString()%>';
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var editor = "";
        var customer = "";
        var station = "";
        var pdLine = "";
        var inputObj;
        var emptyPattern = /^\s*$/;
        var COANo = "";
        var COANo_input = "";
        var defectCount = 0;
        var defectInTable = [];
        var index = 0;

        var defectActionInTable = [];
        var defectCauseInTable = [];


        window.onload = function() 
        {
            tbl = "<%=gridview.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            station = '<%=Request["Station"] %>';
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";

            document.getElementById("<%=radScrap.ClientID%>").checked = true;
        };

        window.onbeforeunload = function() 
        {
            OnCancel();
        };
        
        function initPage() 
        {
            tbl = "<%=gridview.ClientID %>";
            eval("setRowNonSelected_" + tbl + "()");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            COANo = "";
            COANo_input = "";
            defectCount = 0;
            defectInTable = [];
            defectActionInTable = [];
            defectCauseInTable = [];
            
            
            index = 0;
        }


        function input(inputData) 
        {
            try 
            {
                if (IsCOANumberFormat(inputData) == false) 
                {
                    alert(msgInputWrongCode);
                    ShowInfo(msgInputWrongCode);
                    callNextInput();
                }
                else {
                    if (index == 0) {
                        eval("setRowNonSelected_" + tbl + "()");
                        ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
                        defectCount = 0;
                        defectInTable = [];
                        defectActionInTable = [];
                        defectCauseInTable = [];
                 
                    }
                    ShowInfo("");
                    //station = "21";
                    //inputData = "LYFP0000031212";
                    COANo_input = inputData.toString();
                    if (coa_already_exist_in_list(COANo_input) == false) {
                        var this_prodId = SubStringSN(inputData, "ProdId");
                        var this_uid = "<%=UserId%>";
                        beginWaitingCoverDiv();
                        var action = "";
                        var cause = "";

                        if (document.getElementById("<%=radScrap.ClientID%>").checked) {
                            action = "scrap";
                            if (getCauseItemCmbValue() == "") {
                                index = 0;
                                defectCount = 0;
                                defectInTable = [];
                                defectActionInTable = [];
                                defectCauseInTable = [];

                                ShowMessage(msgNoSelectCause);
                                ShowInfo(msgNoSelectCause);
                                endWaitingCoverDiv();
                               
                                callNextInput();
                                return;

                            }
                            else {
                                cause = getCauseItemCmbValue();
                                //var i = cause.indexOf(" ");
                                //cause = cause.substring(0, i);
                                WebServiceCOARemoval.inputCOANumber(COANo_input, pdLine, this_prodId, editor, station, customer, action, cause, inputCOANoSucc, inputCOANoFail);

                            }


                        }
                        else {
                            WebServiceCOARemoval.inputCOANumber(COANo_input, pdLine, this_prodId, editor, station, customer, action, cause, inputCOANoSucc, inputCOANoFail);
                       
                        }
    
                    }
                    else 
                    {
                        alert(msgDuplicatedData);
                        callNextInput();
                        return;
                    }
                }
            }
            catch (e) 
            {
                alert(e.description);
            }
        }

        function coa_already_exist_in_list(coa_in) 
        {
            var ret = false;
            for (var i=0; i<defectCount; i++)
            {
                if (coa_in == defectInTable[i])
                {
                    ret = true; break;
                }
            }
            
            return ret;
        }

        function inputCOANoSucc(result) 
        {
            try
            {
                endWaitingCoverDiv();
                if ((result.length == 4) && (result[0] == "OK!")) {
                    if (result[2] == "scrap")
                        result[2] = "Scrap";

                    setInfo(result[1], result[2], result[3]);
                    callNextInput();
                }
                else 
                {
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                    callNextInput();
                }
            }
            catch (e) 
            {
                alert(e.description);
                endWaitingCoverDiv();
            }
        }

        function inputCOANoFail(result) 
        {
            try
            {
                ShowMessage(result.get_message());
                ShowInfo(result.get_message());
                WebServiceCOARemoval.cancel(COANo_input);
                endWaitingCoverDiv();
                callNextInput();
            }
            catch (e) 
            {
                alert(e.description);
                endWaitingCoverDiv();
            }
        }

        function OnButtonSave() {
            var flag = "";
            var table = document.getElementById("<%=gridview.ClientID%>");
            for (var i = 1; i < table.rows.length; i++) {
                if (table.rows[1].cells[0].innerText == " " || table.rows[1].cells[0].innerText == "") {
                    flag = "1";
                    ShowMessage("COA List is blank!");
                    ShowInfo("COA List is blank!");
                    endWaitingCoverDiv();
                    break;

                }
            }
            if (flag != "1") {
                if (defectInTable.length > 0) {
                    var this_prodId = "";
                    var this_cus = "<%=Customer%>";
                    var this_uid = "<%=UserId%>";
                    beginWaitingCoverDiv();
                    var action = "";
                    if (document.getElementById("<%=radScrap.ClientID%>").checked) {
                        action = "scrap";

                    }
                    var cause = getCauseItemCmbValue();
                    var i = cause.indexOf(" ");
                    cause = cause.substring(0, i);


                    WebServiceCOARemoval.saveProc(pdLine, this_prodId, editor, station, this_cus, defectInTable, defectActionInTable, defectCauseInTable, action, cause, onProcSaveSuccess, onProcSaveFail);
                }
                else {
                    ///////////////////////////////////////

                    var table = document.getElementById("<%=gridview.ClientID%>");
                    for (var i = 1; i < table.rows.length; i++) {
                        if (table.rows[i].cells[1].innerText != " ") {
                            defectInTable[defectCount] = table.rows[i].cells[1].innerText;
                            defectActionInTable[defectCount] = table.rows[i].cells[0].innerText;
                            defectCauseInTable[defectCount] = table.rows[i].cells[2].innerText;



                            defectCount++;

                        }
                    }

                    ///////////////////////////////////////
                    if (defectInTable.length > 0) {
                        var this_prodId = "";
                        var this_cus = "<%=Customer%>";
                        var this_uid = "<%=UserId%>";
                        beginWaitingCoverDiv();
                        var action = "";
                        var cause = "";
                        if (document.getElementById("<%=radScrap.ClientID%>").checked) {
                            action = "scrap";
                            if (getCauseItemCmbValue() == "") {
                                index = 0;
                                defectCount = 0;
                                defectInTable = [];
                                defectActionInTable = [];
                                defectCauseInTable = [];

                                ShowMessage(msgNoSelectCause);
                                ShowInfo(msgNoSelectCause);
                                callNextInput();

                            }
                            else {
                                cause = getCauseItemCmbValue();
                                var i = cause.indexOf(" ");
                                cause = cause.substring(0, i);

                            }

                        }

                        WebServiceCOARemoval.saveProc(pdLine, this_prodId, editor, station, this_cus, defectInTable, defectActionInTable, defectCauseInTable, action, cause, onProcSaveSuccess, onProcSaveFail);
                    }
                    else {
                        alert(msgSaveEmptyList);
                        ShowInfo(msgSaveEmptyList);
                    }
                }
            }
        }

        function onProcSaveSuccess(result) 
        {
            var RowArray = new Array();
            var rw;

            RowArray.push(" ");
        
            endWaitingCoverDiv();
            eval("setRowNonSelected_" + tbl + "()");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            //eval("ChangeCvExtRowByIndex_" + tbl + "(RowArray,false, 0)");
            index = 0;
            defectCount = 0;
            defectInTable = [];
            defectActionInTable = [];
            defectCauseInTable = [];
            ShowSuccessfulInfo(true, "Save success!");
            callNextInput();
        }

        function onProcSaveFail(result) 
        {
            endWaitingCoverDiv();
            ShowInfo(result.get_message());
            callNextInput();
        }

        function setInfo(_back_COA, _back_Action, _back_Cause) 
        {
            //set value to the label
            COANo = _back_COA;
            defectInTable[defectCount] = _back_COA;
            defectActionInTable[defectCount] = _back_Action;
            defectCauseInTable[defectCount] = _back_Cause;
            
            defectCount++;
            setTable(_back_COA);
        }

        function AddRowInfo(RowArray) {
        
        
            if (index < 12) {
                //if (index == 0)
                //    index++;
                eval("ChangeCvExtRowByIndex_" + tbl + "(RowArray,false, index+1)");
                /*if (index == 11) 
                {
                    setSrollByIndex(index - 1, false);
                }*/
            }
            else 
            {
                eval("AddCvExtRowToBottom_" + tbl + "(RowArray,false)");
                //setSrollByIndex(index - 1, false);
            }
            //setSrollByIndex(index - 1, false);
            setSrollByIndex(index, false , tbl);
            index++;


        }

        function setTable(_back_COA) 
		{
		    //ClearGvExtTable(tbl, DEFAULT_ROW_NUM);

		    var rowArray = new Array();
		    var rw;

		    var action = "";
		    var cause = "";
		    if (document.getElementById("<%=radScrap.ClientID%>").checked) {
		        action = "Scrap";
		        if (getCauseItemCmbValue() == "") {
		            index = 0;
		            defectCount = 0;
		            defectInTable = [];
		            defectActionInTable = [];
		            defectCauseInTable = [];

		            ShowMessage(msgNoSelectCause);
		            ShowInfo(msgNoSelectCause);
		            callNextInput();

		        }
		        else {
		            cause = getCauseItemCmbValue();
		            rowArray.push(action);
		            rowArray.push(_back_COA);
		            rowArray.push(cause);

		            AddRowInfo(rowArray);		            
		        }
		    }
		    else {
		        action = "Remove";
		        rowArray.push(action);
		        rowArray.push(_back_COA);
		        rowArray.push(cause);
		    
		        AddRowInfo(rowArray);
		    }
		}
		    
        function OnCancel() 
        {
            if (COANo != "") 
            {
                WebServiceCOARemoval.cancel(COANo);
            }
        }

        function callNextInput() 
        {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }

        function ExitPage() 
        {
            OnCancel();
        }

        function ResetPage() 
        {
            ExitPage();
            initPage();
            ShowInfo("");
            callNextInput();
        }

        function IsCOANumberFormat(data) 
        {
            /*if (data.toString().length == 14) 
            {
                return true;
            }*/

            return true;
        }

        function clientUpload() {
            beginWaitingCoverDiv();
        }

        function finish(param) {
            //writeToSuccessMessage(errorMsg);
            initPage();
            if (document.getElementById("<%=radScrap.ClientID%>").checked) {

                if (getCauseItemCmbValue() == "") {

                    ShowMessage(msgNoSelectCause);
                    ShowInfo(msgNoSelectCause);

                }
                else {
                    document.getElementById("<%=hiddenCostCenter.ClientID%>").value = param;
                    document.getElementById("<%=btnUploadOver.ClientID%>").click();
                }
            }
            else {
                document.getElementById("<%=hiddenCostCenter.ClientID%>").value = param;
                document.getElementById("<%=btnUploadOver.ClientID%>").click();
            }
        }
        

                                                                                                                               
    </script>

</asp:Content>

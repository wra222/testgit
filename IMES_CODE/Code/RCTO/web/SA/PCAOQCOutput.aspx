<%--
/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description: PCA OQC Output
 * UI:CI-MES12-SPEC-SA-UI PCA OQC Output.docx 
 * UC:CI-MES12-SPEC-SA-UC PCA OQC Output.docx           
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13  Chen Xu               Create
 * 2012-01-19  Chen Xu               Modify: ITC-1360-0151, ITC-1360-0156, ITC-1360-0157, ITC-1360-0158, ITC-1360-0162,ITC-1360-0163,ITC-1360-0164,ITC-1360-0166
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	记录SAOQC 结果，若有不良，则记录不良信息
 *              
 * UC Revision:  3382
 */
--%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PCAOQCOutput.aspx.cs" Inherits="SA_PCAOQCOutput" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    <Services>
        <asp:ServiceReference Path=  "~/SA/Service/WebServicePCAOQCOutput.asmx"/>
    </Services>
    </asp:ScriptManager>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" language="javascript">

    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgPdLineNull = '<%=this.GetLocalResourceObject(Pre + "_msgPdLineNull").ToString()%>';
    var msgMBSnoNull = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoNull").ToString()%>';
    var msgDefectNull = '<%=this.GetLocalResourceObject(Pre + "_msgDefectNull").ToString()%>';
    var msgMBSnoLength = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoLength").ToString()%>';
    var msgMBSnoBit = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoBit").ToString()%>';
    var msgDuplicateData = '<%=this.GetLocalResourceObject(Pre + "_msgDuplicateData").ToString()%>';
    var msgInputValidDefect = '<%=this.GetLocalResourceObject(Pre + "_msgInputValidDefect").ToString()%>';

    var SUCCESSRET ="SUCCESSRET";
    var editor ="";
    var customer = "";
    var station = "";
    var pCode = "";
    var MBSno = "";
    var passQty = 0;
    var failQty = 0;
    var scanFlag = false;
    var defectCount = 0;
    var defectInTable = [];
    var DEFAULT_ROW_NUM = 13;
    var defectCache;
    var tbl;
    var inputDefectFlag  = false;
    
    window.onload = function() {
        tbl = "<%=GridViewExt1.ClientID %>";
        editor = "<%=UserId%>";
        customer = "<%=Customer%>";
        station = '<%=Request["Station"] %>';
        pCode = '<%=Request["PCode"] %>';

        inputSNControl = getCommonInputObject();
        inputSNControl.disabled = true;
        setStart();
    }

    function setStart() {
        endWaitingCoverDiv();
        inputSNControl.disabled = "";
        setInputFocus();
        document.getElementById("txtPassQty").value = "0";
        document.getElementById("txtFailQty").value = "0";
        getAvailableData("processDataEntry");

    }

    function setInputFocus() {
        if ((inputSNControl.disabled == false) || (inputSNControl.disabled == "")) {
            getCommonInputObject().focus();
        }
        else {
            inputSNControl.disabled = false;
            getCommonInputObject().focus();
        }
    }


    function checkMBSno(inputData) {
        MBSno = inputData.trim();
        if (MBSno == "" || MBSno == null) {
            alert(msgMBSnoNull);
        //    ShowInfo(msgMBSnoNull);
            return false;
        }
        if (!(MBSno.length == 10 || MBSno.length == 11)) {
            alert(msgMBSnoLength);
       //     ShowInfo(msgMBSnoLength);
            return false;
        }
        //if (MBSno.substr(4, 1) != "M") {
        if ((MBSno.substr(4, 1) != "M") && (MBSno.substr(5, 1) != "M")) {
            alert(msgMBSnoBit);
        //    ShowInfo(msgMBSnoBit);
            return false;
        }
        //        MBSno = MBSno.substr(0, 10);
        MBSno = SubStringSN(MBSno, "MB");
        return true;
    }

    function processDataEntry(inputData) {
        ShowInfo("");
        //scanFlag = document.getElementById("<%=nineSelect.ClientID%>").checked;
        if (inputData == "7777") {
            clearTable();
            callNextInput();
        }
        else if (inputData == "9999") {
//            if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
//                alert(msgPdLineNull);
//             //   ShowInfo(msgPdLineNull);
//                setPdLineCmbFocus();
//                callNextInput();
//            }
        //            else
            if (defectInTable.length > 0) {
                ShowInfo("");
                save();
            }
            else if (inputDefectFlag) {
                alert(msgDefectNull);
                callNextInput();
            }
            else {
                callNextInput();
            }
        }
        else if (inputDefectFlag) {
            inputDefect(inputData);
        }
        else if (checkMBSno(inputData)) {
//            if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
//                alert(msgPdLineNull);
//           //     ShowInfo(msgPdLineNull);
//                setPdLineCmbFocus();
//                callNextInput();
//            }
//            else {
                beginWaitingCoverDiv();
                WebServicePCAOQCOutput.InputMBSno(MBSno, editor, station, customer, onInputSucceeded, onFailed);
//            }
        }
        else {
            callNextInput();
        }
    }
  
   
  
    function onInputSucceeded(result)
    {
        try 
        {
            endWaitingCoverDiv();
            
            if(result==null)
            {
                //service方法没有返回
                endWaitingCoverDiv();
                alert(msgSystemError);  
                 
            }
            else if(result[0]==SUCCESSRET)
            {
                document.getElementById("txtMBSno").value = MBSno;
                document.getElementById("txtModel").value = result[1];
                document.getElementById("txtPdLine").value = result[3].line + " " + result[3].lineDesc;
                //set defectCache value
                defectCache = result[2];    
             
                inputDefectFlag = true;

                if (scanFlag) {
                    ShowInfo("");
                    save();
                }
            }
            else
            {
                endWaitingCoverDiv();    
                var content =result[0];
                ShowMessage(content);
                ShowInfo(content);
            }

            callNextInput();      

        }
        catch(e) 
        {
            endWaitingCoverDiv();
            alert(e.description);
        }
        
    }
 
    function onFailed(error)
    {
       try
       {
            endWaitingCoverDiv();
            ExitPage(); //ITC-1360-1831
            ClearData();
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
            getAvailableData("processDataEntry");
        } 
        catch(e) 
        {
            endWaitingCoverDiv();
            alert(e.description);
        }
   
    }

    function save() {
     //   beginWaitingCoverDiv();

        WebServicePCAOQCOutput.save(MBSno, defectInTable, saveSucc, onFailed);
    }

    function saveSucc(result) {
     try  {
            endWaitingCoverDiv();
            

            if (isPass()) {
                passQty++;
                document.getElementById("txtPassQty").value = passQty;
            }
            else {
                failQty++;
                document.getElementById("txtFailQty").value = failQty;
            }
            var SuccessItem = "[" + MBSno + "]"; 
            ClearData(); 
            ShowSuccessfulInfo(true, SuccessItem + " " + msgSuccess);
            getAvailableData("processDataEntry");
         }
         catch (e) {
            endWaitingCoverDiv();
            alert(e.description);
         }
    }

    function isPass() {
        if (defectCount == 0) {
            return true;
        }
        return false;
    }

    function inputDefect(data) {
        if (data.length != 4) {
            alert(msgInputValidDefect);
            callNextInput();
            return;
        }
        if (isExistInTable(data)) {
            alert(msgDuplicateData);
            callNextInput();
            return;
        }

        if (isExistInCache(data)) {
            var desc = getDesc(data);
            var rowArray = new Array();
            var rw;

            rowArray.push(data);
            rowArray.push(desc);

            //add data to table
            if (defectInTable.length < 12) {
                eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, defectInTable.length + 1);");
                setSrollByIndex(defectInTable.length, true, tbl);
            }
            else {
                eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                setSrollByIndex(defectInTable.length, true, tbl);
                rw.cells[1].style.whiteSpace = "nowrap";
            }

            defectInTable[defectCount++] = data;
            callNextInput();
        }
        else {
            alert(msgInputValidDefect);
            callNextInput();
        }
    }

    function isExistInTable(data) {
        if (defectInTable != undefined && defectInTable != null) {
            for (var i = 0; i < defectInTable.length; i++) {
                if (defectInTable[i] == data) {
                    return true;
                }
            }
        }

        return false;
    }

    function isExistInCache(data) {
        if (defectCache != undefined && defectCache != null) {
            for (var i = 0; i < defectCache.length; i++) {
                if (defectCache[i]["id"] == data) {
                    return true;
                }
            }
        }

        return false;
    }

    function getDesc(data) {
        if (defectCache != undefined && defectCache != null) {
            for (var i = 0; i < defectCache.length; i++) {
                if (defectCache[i]["id"] == data) {
                    return defectCache[i]["description"];
                }
            }
        }

        return "";
    }

    
    function ExitPage() {
        if (MBSno != "") {
            WebServicePCAOQCOutput.Cancel(MBSno);
            sleep(waitTimeForClear);
        }
    }

    window.onunload = function() {
        ResetPage();
    };

    window.onbeforeunload = function() {
        ResetPage();

    };
    
    function ResetPage()
    {
        ExitPage();
        endWaitingCoverDiv();
        ShowInfo("");
        ClearData();
        passQty = 0;
        failQty = 0;
        document.getElementById("txtPassQty").value = "0";
        document.getElementById("txtFailQty").value = "0";
    }


   

    function ClearData() {
        ShowInfo("");
        clearTable();
        inputDefectFlag = false;
        scanFlag = false;
        MBSno = "";
        document.getElementById("txtMBSno").value = "";
        document.getElementById("txtModel").value = "";
        document.getElementById("txtPdLine").value = "";
        inputSNControl.value = "";
        inputSNControl.focus();
    }

    function clearTable() {
        try {
            tbl = "<%=GridViewExt1.ClientID %>";
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            setSrollByIndex(0, false);
            eval("setRowNonSelected_" + tbl + "()");
            
            defectCount = 0;
            defectInTable = [];
        }
        catch (e) {
            alert(e.description);
        }

    }

    function callNextInput() {

        inputSNControl.value = "";
        inputSNControl.focus();
        getAvailableData("processDataEntry");
    }

        
     function alertNoPdLine() {
         alert(msgPdLineNull);
     }

     
    
    </script>
    <div>
    <center >
    <br />
    <TABLE  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
   
    <TR>
	    <TD style="width:15%; height:30px" align="left" valign="middle">
	        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </TD>
	    <TD colspan="4"  align="left">
           <%-- <iMES:CmbPdLine ID="CmbPdLine1" runat="server"  Width="99" IsPercentage="true"/>--%>
           <input id="txtPdLine" style="width:98%; height: 20;" type="text" readonly="readonly"
            class="iMes_textbox_input_Disabled" /> 
        </TD>       
    </TR>
    
     <TR>
	    <TD style="width:15%; height:30px" align="left"  valign="middle">
	        <asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </TD>
	    <TD colspan="4" align="left">
            <input id="txtMBSno" style="width:98%; height: 20;" type="text" readonly="readonly"
            class="iMes_textbox_input_Disabled" /> 
        </TD>
	   </TR>
	   
	   <TR>
	    <TD style="width:15%; height:30px" align="left"  valign="middle">
	        <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </TD>
	    <TD colspan="4" align="left">	 
              <input id="txtModel" style="width:98%; height: 20;" type="text" readonly="readonly"
            class="iMes_textbox_input_Disabled" />
        </TD>
    </TR>
    
    <TR>
	    <TD style="width:15%; height:30px" align="left"  valign="middle">
	        <asp:Label ID="lblPassQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </TD>
	    <TD  style="width:30%; height:30px" align="left"  valign="middle">
            <input id="txtPassQty" style="width:98%; height: 20;" type="text" readonly="readonly"
            class="iMes_textbox_input_Disabled" /> 
        </TD>
        <TD style="width:10%; height:30px" align="left"  valign="middle">
	    </TD>
        <TD style="width:15%; height:30px" align="left"  valign="middle">
	        <asp:Label ID="lblFailQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </TD>
	    <TD style="width:30%; height:30px" align="left"  valign="middle">
            <input id="txtFailQty" style="width:95%; height: 20; vertical-align:middle; " type="text" readonly="readonly" 
            class="iMes_textbox_input_Disabled" />  
        </TD>
        
	   </TR>
    
    </TABLE>
    
    <hr class="footer_line" style="width:95%"/>
 	  
	  <fieldset style="width: 95%" align="center">
        <legend id="lblDefectList" runat="server" style="color:Blue" class="iMes_label_13pt"></legend>
        
	        <table width="99%"  cellpadding="0" cellspacing="0" border="0" align="center">
                <tr >
                    <td colspan="2" align="center" width="99.8%">
                        <div id="divGrid" style="z-index: 0">
                            <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false"  AutoHighlightScrollByValue="true" 
                                        GetTemplateValueEnable="true" SetTemplateValueEnable="true"
                                        GvExtHeight="280px" GvExtWidth="98%" Width="98%" HighLightRowPosition="3" 
                                        onrowdatabound="GridViewExt1_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="DefectCode" HeaderStyle-Width="30%" />
                                            <asp:BoundField DataField="Description" HeaderStyle-Width="70%" />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
	  </fieldset>
   <br />
   <TABLE  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
        <TR>
            <TD  align="left">
                 <asp:Label id="lblDataEntry" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
            </TD>
            <TD  width="85%" align="left" >
                   <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                    CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                    InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
           </TD>
            <!--<td  align="right">
                        <asp:CheckBox runat="server" ID="nineSelect"  AutoPostBack="false" 
                            CssClass="iMes_label_11pt" BackColor="Transparent" BorderStyle="None" 
                            oncheckedchanged="nineSelect_CheckedChanged"></asp:CheckBox>
                       &nbsp; &nbsp;
             </td>-->
        </TR>
        
        <tr>
            <td colspan="3">
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
                <ContentTemplate>
                            <input type="hidden" runat="server" id="pCode" />
                </ContentTemplate>
            </asp:UpdatePanel> 
            </td>
           
        </tr>
   </TABLE>   
   <br />
    </center>
</div>

</asp:Content>
<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:MB Split
 * UI:CI-MES12-SPEC-SA-UI MB Split.docx 
 * UC:CI-MES12-SPEC-SA-UC MB Split.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13  Chen Xu               Create
 * 2012-02-10  Chen Xu               Modify: ITC-1360-0364
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	连板切割入口，实现连板的切割
 *                2.	打印子板标签
 * UC Revision: 3924
 */
--%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="MBSplit.aspx.cs" Inherits="SA_MBSplit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    <Services>
        <asp:ServiceReference Path=  "~/SA/Service/WebServiceMBSplit.asmx"/>
    </Services>
    </asp:ScriptManager>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
 <div>
    <center >
    <TABLE  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
   <br />
   <TR>
	    <TD style="width:15%; height:30px" align="left" valign="bottom">
	        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </TD>
	    <TD colspan="4" align="left">
            <iMES:CmbPdLine ID="CmbPdLine1" runat="server"  Width="99" IsPercentage="true"/>
        </TD>       
    </TR>
    
    <TR>
	    <TD style="width:15%; height:30px" align="left"  valign="bottom">
	        <asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </TD>
	    <TD colspan="4" align="left">
            <input id="txtMBSno" style="width:98%; height: 20;" type="text" readonly="readonly"
            class="iMes_textbox_input_Disabled" /> 
        </TD>
	   </TR>
	   
	   <TR>
	    <TD style="width:15%; height:30px" align="left"  valign="bottom">
	        <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    </TD>
	    <TD colspan="4" align="left">	 
              <input id="txtModel" style="width:98%; height: 20;" type="text" readonly="readonly"
            class="iMes_textbox_input_Disabled" />
        </TD>
    </TR>
    </TABLE>
      
	  <br />
	  <br />
	  <br />
	  
   
   <TABLE  width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
        <TR>
            <TD  style="width:15%; height:30px">
                 <asp:Label id="lblDataEntry" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
            </TD>
            <TD  width="50%" align="left" >
                   <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                    CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                    InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
           </TD>
            <TD  align="right">
                <input id="btnPrintSetting" style="height:auto" type="button"  runat="server" 
                            onclick="showPrintSettingDialog()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"/> 
                            &nbsp; 
             </TD>
	        <TD  align="right">
                <input id="btnReprint" style="height:auto" type="button"  runat="server" 
                            onclick="rePrint()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"/> 
                            &nbsp; 
             </TD>
        </TR>
        <tr>
            <td>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
                <ContentTemplate></ContentTemplate>
            </asp:UpdatePanel>  
            </td>
        </tr>
   </TABLE>  
   
   <br />
    </center>
</div>



    <script type="text/javascript" language="javascript">

        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgPdLineNull = '<%=this.GetLocalResourceObject(Pre + "_msgPdLineNull").ToString()%>';
    var msgMBSnoNull = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoNull").ToString()%>';
    var msgMBSnoLength = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoLength").ToString()%>';
    var msgMBSnoBit = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoBit").ToString()%>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var SUCCESSRET ="SUCCESSRET";
    var editor ="";
    var customer = "";
    var station = "";
    var pCode = "";
    var MBSno = "";
   
    var AccountId='<%=Request["AccountId"] %>';
    var Login ='<%=Request["Login"] %>';
    
    window.onload = function()
    {
        editor = "<%=UserId%>";
        customer = "<%=Customer%>";
        station = '<%=Request["Station"] %>';
        pCode = '<%=Request["PCode"] %>';
        
        inputSNControl = getCommonInputObject();
        inputSNControl.disabled = true;
        setStart();
    }

    function setStart() {
        inputSNControl.disabled = "";
        setInputFocus();
        getAvailableData("processDataEntry");

    }

    function setInputFocus() {
        if ((inputSNControl.disabled == false) || (inputSNControl.disabled == "")) {
            inputSNControl.focus();
        }
        else {
            inputSNControl.disabled = false;
            inputSNControl.focus();
        }
    }

    
    function checkMBSno() {
        if (MBSno == "" || MBSno == null) {
            alert(msgMBSnoNull);
//            ShowInfo(msgMBSnoNull);
            return false;
        }
        if (!(MBSno.length == 10 || MBSno.length == 11)) {
            alert(msgMBSnoLength);
//            ShowInfo(msgMBSnoLength);
            return false;
        }
        //if (MBSno.substr(4, 1) != "M") {
        if ((MBSno.substr(4, 1) != "M") && (MBSno.substr(5, 1) != "M")) {
            alert(msgMBSnoBit);
//            ShowInfo(msgMBSnoBit);
            return false;
        }
        //        MBSno = MBSno.substr(0, 10);
        MBSno = SubStringSN(MBSno, "MB");
        return true;
    }
    
    function processDataEntry(mbsno) {
        ShowInfo("");
        MBSno = mbsno.trim();
        if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
            alert(msgPdLineNull);
//            ShowInfo(msgPdLineNull);
            setPdLineCmbFocus();
            getAvailableData("processDataEntry");
        }
        else if (checkMBSno()) {
            lstPrintItem = getPrintItemCollection();
            if (lstPrintItem == null)                 //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
            {
                alert(msgPrintSettingPara);
                ResetPage();
                getAvailableData("processDataEntry");
                return;
            }
            else {
                beginWaitingCoverDiv();
                WebServiceMBSplit.InputMBSno(getPdLineCmbValue(), MBSno, editor, station, customer, lstPrintItem, onSucceeded, onFailed);
            }
        } 
        else {
            callNextInput();
        }
    }
  
   
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSucceeded
    //| Author		:	Chen Xu
    //| Create Date	:	01/13/2012
    //| Description	:	inputMBSno调用web service成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSucceeded(result)
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
            else if((result.length==4)&&(result[0]==SUCCESSRET))
            {
                document.getElementById("txtMBSno").value = MBSno;
                document.getElementById("txtModel").value = result[2];

                var SuccessItem = "[" + MBSno + "]";
                ShowSuccessfulInfo(true, SuccessItem + " " + msgSuccess);

                // setPrintItemListParam(result[1],result[3]);
                // printLabels(result[1], false);
                // ITC-1360-0364

                // 改为串行打印：
                var lst = new Array();
                for (var i = 0; i < result[3].length; i++) {
                    setPrintItemListParam1(result[1], result[3][i]);

//                    for (var j = 0; j < result[1][i].length; j++) {
//                        lst[lst.length] = result[1][i][j];
//                    }
                    printLabels(result[1], false);
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
        
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFailed
    //| Author		:	Chen Xu
    //| Create Date	:	01/13/2012
    //| Description	:	inputMBSno调用web service失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFailed(error)
    {
       try
       {
            endWaitingCoverDiv();
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


    function setPrintItemListParam(lstPrtItem,MBSnoList) {
        //============================================generate PrintItem List==========================================
        //        /*
        //         * Function Name: setPrintParam
        //         * @param: printItemCollection
        //         * @param: labelType
        //         * @param: keyCollection(Client: Array of string.    Server: List<string>)
        //         * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
        //         */
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@MBSno";

        valueCollection[0] = generateArray(processSerialNoList(MBSnoList));
        
        setPrintParam(lstPrtItem, "Unit MB Label", keyCollection, valueCollection);

    }

    function setPrintItemListParam1(backlstPrtItem,iMBSno) {
        //============================================generate PrintItem List==========================================
        //        /*
        //         * Function Name: setPrintParam
        //         * @param: printItemCollection
        //         * @param: labelType
        //         * @param: keyCollection(Client: Array of string.    Server: List<string>)
        //         * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
        //         */
     //   var lstPrtItem = new Array();  
        var keyCollection = new Array();
        var valueCollection = new Array();

   //     lstPrtItem[0] = backlstPrtItem;

        var lstPrtItem = backlstPrtItem;
        
        keyCollection[0] = "@MBSno";

        valueCollection[0] = generateArray(iMBSno);

        setPrintParam(lstPrtItem, "Unit MB Label", keyCollection, valueCollection);

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ExitPage
    //| Author		:	Chen Xu
    //| Create Date	:	01/13/2012
    //| Description	:	中断Session
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ExitPage() {
        if (MBSno != "") {
            WebServiceMBSplit.Cancel(MBSno);
            sleep(waitTimeForClear);
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ResetPage
    //| Author		:	Chen Xu
    //| Create Date	:	01/13/2012
    //| Description	:	页面重置
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    function ResetPage()
    {
        ExitPage();
        endWaitingCoverDiv();
        ShowInfo("");
        ClearData();
        getPdLineCmbObj().selectedIndex = 0;
        setPdLineCmbFocus();  
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ClearData
    //| Author		:	Chen Xu
    //| Create Date	:	01/13/2012
    //| Description	:	清空页面（除PdLine)
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    function ClearData() {
        ShowInfo("");
        MBSno = "";
        document.getElementById("txtMBSno").value = "";
        document.getElementById("txtModel").value = "";
        inputSNControl.value = "";

        if (getPdLineCmbObj().selectedIndex == 0) {
            setPdLineCmbFocus();
        }
        else {
            inputSNControl.focus();
        }
    }

    function callNextInput() {

        inputSNControl.value = "";
        inputSNControl.focus();
        getAvailableData("processDataEntry");
    }


    function showPrintSettingDialog() {
        showPrintSetting(station, pCode);
    }
     
     function rePrint() {
         var url = "ReprintMBSplit.aspx?Station=" + station+ "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer +"&AccountId=" + AccountId +  "&Login=" + Login;
         window.showModalDialog(url, "", 'dialogWidth:950px;dialogHeight:600px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:no');

     }
     
     function alertNoPdLine() {
         alert(msgPdLineNull);
     }
    
    
    </script>
    
</asp:Content>
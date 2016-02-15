<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for Image Download Page
 * UI:CI-MES12-SPEC-FA-UI Image Download.docx –2011/11/21 
 * UC:CI-MES12-SPEC-FA-UC Image Download.docx –2011/10/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-4   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0092, Jessica Liu, 2012-1-19
* ITC-1360-1857, Jessica Liu, 2012-6-28
*/
--%>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ImageDownload.aspx.cs" Inherits="FA_ImageDownload" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<style type="text/css">
</style>
 
<script type="text/javascript">

    var msgNoInputCPQSNO = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputCPQSNO").ToString() %>';
    var msgNoInputBIOS = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputBIOS").ToString() %>';
    var msgNoInputImage = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputImage").ToString() %>';
    var msgInputBIOSAndImage = '<%=this.GetLocalResourceObject(Pre + "_msgInputBIOSAndImage").ToString() %>';
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    var inputObj = "";
    var inputData = "";
    var station = "";
    var GridViewClientID = "<%=gridview.ClientID%>";
    var index = 1;
    var strRowsCount = "<%=initRowsCount%>";    
    var initRowsCount = parseInt(strRowsCount, 10) + 1;
    var DataLogArray = new Array();
    var saveDataLogArray = new Array();
    var CPQSNOInput = false;
    var oldCPQSNO = "";
    var currentProdID = "";
    var flag = false;
    //ITC-1360-1857, Jessica Liu, 2012-6-28
    var saveClickFlag = false;

    //2012-4-11
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onload
    //| Author		:	Jessica Liu
    //| Create Date	:	10/11/2011
    //| Description	:	加载接受输入数据事件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    document.body.onload = function() {
        try {
            inputObj = getCommonInputObject();
            ShowInfo("");
            inputData = inputObj.value; 
            inputObj.focus();
            station = document.getElementById("<%=hiddenStation.ClientID %>").value;
            getAvailableData("processDataEntry")

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }

    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	processDataEntry
    //| Author		:	Jessica Liu
    //| Create Date	:	10/12/2011
    //| Description	:	检测并进行数据检索及显示
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function processDataEntry(inputData) 
    {
        ShowInfo("");
        
        var uutInput = true;
        
        if (CPQSNOInput == true)
        {
            WebServiceImageChange.Cancel(oldCPQSNO);
            CPQSNOInput = false;   
        }

        if (inputData == "") 
        {
            alert(msgNoInputCPQSNO);
            inputObj.focus();
            uutInput = false;
            
            getAvailableData("processDataEntry");
        }

        if (uutInput) 
        {
            getAvailableData("processDataEntry");
                             
            beginWaitingCoverDiv();
            
            oldCPQSNO = inputObj.value;
            var line = ""; 

            WebServiceImageDownload.checkCPQSNO(inputObj.value, line, "<%=UserId%>", station, "<%=Customer%>", onFSNSucceed, onFSNFail);         
        }      
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFSNSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	11/4/2011
    //| Description	:	第一次SN扫入成功，获取到Model
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFSNSucceed(result) 
    {       
        try {
             if (result == null) 
            {
                SetInitStatus();
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 3) && (result[0] == SUCCESSRET))     
            {
                SetInitStatusNoReset();
                
                CPQSNOInput = true;
                
                document.getElementById("<%=txtModel.ClientID%>").innerText = result[1];
                currentProdID = result[2];

                //ITC-1360-1857, Jessica Liu, 2012-6-28
                //ShowInfo(msgInputBIOSAndImage);
                if (saveClickFlag == false) {
                    ShowInfo(msgInputBIOSAndImage);
                }
                else {
                    saveClickFlag = false;
                    DoSave();
                }
                
            }
            else 
            {
                SetInitStatus();
                
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
            }
        } catch (e) {
            SetInitStatus();
            
            alert(e);
            callNextInput();
        }
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFSNFail
    //| Author		:	Jessica Liu
    //| Create Date	:	11/4/2011
    //| Description	:	第一次SN扫入处理失败，置错误信息
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFSNFail(error) 
    {
        SetInitStatus();
        CPQSNOInput = false;
        
        try {
            ShowInfo("");
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            callNextInput();

        } catch (e) {
            alert(e.description);
        }

    }

 
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	callNextInput
    //| Author		:	Jessica Liu
    //| Create Date	:	10/11/2011
    //| Description	:	等待快速控件继续输入
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function callNextInput() 
    {
        inputObj.focus();   

        getAvailableData("processDataEntry");
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	SetInitStatusNoReset
    //| Author		:	Jessica Liu
    //| Create Date	:	11/4/2011
    //| Description	:	处理底层调用返回时，取消hold界面等初始处理
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function SetInitStatusNoReset() 
    {
        ShowInfo("");

        endWaitingCoverDiv();

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	SetInitStatus
    //| Author		:	Jessica Liu
    //| Create Date	:	11/4/2011
    //| Description	:	处理底层调用返回时，做的控件清空、取消hold界面等初始处理
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function SetInitStatus() 
    {
        ShowInfo("");
        
        endWaitingCoverDiv();

        resetAllNoTable();

        //ITC-1360-1857, Jessica Liu, 2012-6-28
        saveClickFlag = false;

    }
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	DoSave
    //| Author		:	Jessica Liu
    //| Create Date	:	11/4/2011
    //| Description	:	点击Save按钮时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function DoSave()
    {
        ShowInfo("");

        var uutInput = true;
        
        var bios = document.getElementById("txtBIOS").value;
        var image = document.getElementById("txtImage").value;
       
        if (inputObj.value == "") 
        {
            alert(msgNoInputCPQSNO);
            inputObj.focus();
            uutInput = false;
            getAvailableData("processDataEntry");
        }
        
        //ITC-1360-1857, Jessica Liu, 2012-6-28
        var currentModel = document.getElementById("<%=txtModel.ClientID%>").innerText;
        if (currentModel == "") {
            saveClickFlag = true;
            processDataEntry(inputObj.value);
            return;
        }

        if (bios == "") 
        {
            alert(msgNoInputBIOS);
            document.getElementById("txtBIOS").focus();
            uutInput = false;
        }

        if (image == "") {
            alert(msgNoInputImage);
            document.getElementById("txtImage").focus();
            uutInput = false;
        }

        if (uutInput == true)
        {
            getAvailableData("processDataEntry");
                             
            beginWaitingCoverDiv();
            
            var line = "";            

            WebServiceImageDownload.doSave(inputObj.value, bios, image, line, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFail); 
        }        
    }
    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	11/4/2011
    //| Description	:	调用web service doSave成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSucceed(result) 
    {
        CPQSNOInput = false;
               
        try {

            if (result == null) 
            {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);            
            }
            else if ((result.length == 1) && (result[0] == SUCCESSRET)) 
            {
                fillTable();
                
                //2012-4-11
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, "[" + oldCPQSNO + "] " + msgSuccess);             
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
            }

            SetInitStatus();
            
            callNextInput();
        } catch (e) {
            SetInitStatus();
            
            alert(e.description);
        }

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFail
    //| Author		:	Jessica Liu
    //| Create Date	:	11/4/2011
    //| Description	:	调用web service doSave失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFail(error) 
    {
        SetInitStatus();
        
        CPQSNOInput = false;
            
        try {
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            callNextInput();

        } catch (e) {
            alert(e.description);
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	fillTable
    //| Author		:	Jessica Liu
    //| Create Date	:	1/19/2012
    //| Description	:	为table添加新行内容实现
    //| Input para.	:	
    //| Ret value	:
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function AddRowInfo(RowArray) 
    {
        if (index < initRowsCount) 
        {
            eval("ChangeCvExtRowByIndex_" + GridViewClientID + "(RowArray,false, index)");
        }
        else 
        {
            eval("AddCvExtRowToBottom_" + GridViewClientID + "(RowArray,false)");
        }
        
        index++;
        setSrollByIndex(index, false);
    }
    
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	fillTable
    //| Author		:	Jessica Liu
    //| Create Date	:	11/4/2011
    //| Description	:	为table添加新行内容
    //| Input para.	:	
    //| Ret value	:
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //ITC-1360-0092, Jessica Liu, 2012-1-19
    function fillTable(dataLogList) 
    {
        var rowInfo = new Array();
        rowInfo.push(currentProdID);
        rowInfo.push(document.getElementById("<%=txtModel.ClientID%>").innerText);
        rowInfo.push(document.getElementById("txtBIOS").value);
        rowInfo.push(document.getElementById("txtImage").value);
        AddRowInfo(rowInfo);        
    }
    

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ExitPage
    //| Author		:	Jessica Liu
    //| Create Date	:	10/11/2011
    //| Description	:	退出页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ExitPage() 
    {

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	clearTable
    //| Author		:	Jessica Liu
    //| Create Date	:	11/4/2011
    //| Description	:	将Data Log table的内容清空
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function clearTable() 
    {
        try {
            ClearGvExtTable("<%=gridview.ClientID%>", initRowsCount);

            index = 1;

            saveDataLogArray.length = 0;

        } catch (e) {
            alert(e.description);
        }
        
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	resetAllNoTable
    //| Author		:	Jessica Liu
    //| Create Date	:	10/11/2011
    //| Description	:	除了table重置所有控件到初始状态
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function resetAllNoTable() 
    {
        inputObj.value = "";
        document.getElementById("<%=txtModel.ClientID%>").innerText = "";
        document.getElementById("txtBIOS").value = "";
        document.getElementById("txtImage").value = "";

        currentProdID = "";
        
        inputObj.focus();   
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	resetAll
    //| Author		:	Jessica Liu
    //| Create Date	:	10/11/2011
    //| Description	:	重置所有控件到初始状态
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function resetAll() 
    {
        inputObj.value = "";
        document.getElementById("<%=txtModel.ClientID%>").innerText = "";
        document.getElementById("txtBIOS").value = "";
        document.getElementById("txtImage").value = "";

        currentProdID = "";
        
        clearTable();
        
        inputObj.focus();    
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ResetPage
    //| Author		:	Jessica Liu
    //| Create Date	:	10/11/2011
    //| Description	:	刷新页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ResetPage() 
    {
        ExitPage();
        resetAll();
    }    
    
</script>
    
        
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceImageDownload.asmx" />
            </Services>
        </asp:ScriptManager>
         
        <center>
            
        <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">
  
            <tr><td>&nbsp;</td><td></td><td></td></tr>
            
            <tr >
                <td style="width:20%" align="left" >
                    <asp:Label ID="lblCPQSNO" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <%-- ITC-1360-0058,Jessica Liu, 2012-1-16--%>
                <td style="width:65%" align="left" >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
             CanUseKeyboard="true" IsPaste="true" MaxLength="50" CssClass="iMes_textbox_input_Yellow"/>
                        </ContentTemplate>   
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>                                     
                    </asp:UpdatePanel>
                </td>     
                <td style="width:15%" align="center">   
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>                        
                            <input id="btnSave" style="height:auto" type="button"  runat="server" 
                            onclick="DoSave()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"/> 
                            &nbsp; 
                        </ContentTemplate>  
                    </asp:UpdatePanel>
                </td>
  
            </tr>
            
            <tr><td>&nbsp;</td><td></td><td></td></tr>
            
            <tr>
                <td align="left" >
                    <asp:Label ID="lblModel" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="2">
                    <asp:Label ID="txtModel" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
                </td>
            </tr>  
            
            <tr><td>&nbsp;</td><td></td><td></td></tr>
     
            <tr>
                <td align="left" >
                    <asp:Label ID="lblBIOS" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>    
                            <input type="text" ID="txtBIOS" class="iMes_textbox_input_Normal"
                             MaxLength="50" style="width:98%"/>
                        </ContentTemplate>  
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>        
                    </asp:UpdatePanel>
                </td>
            </tr>     
                    
            <tr><td>&nbsp;</td><td></td><td></td></tr>

            <tr>
                <td align="left">
                    <asp:Label ID="lblImage" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>                 
                            <Input type="text" ID="txtImage" class="iMes_textbox_input_Normal" 
                            MaxLength="50" style="width:98%"/>
                        </ContentTemplate>  
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>        
                    </asp:UpdatePanel>
                </td>
            </tr>     
            
            <tr>
                <td colspan="3">
                    <hr>
                </td>
            </tr>
            
            <tr>
                <td align="left"  colspan="3">
                    <asp:Label ID="lblDataLog" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
            </tr>
            
            <tr>
                <td colspan="3">
                    <asp:UpdatePanel ID="updatePanel5" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" 
                            GetTemplateValueEnable="False" GvExtHeight="320px" Height="310px" GvExtWidth="100%" OnGvExtRowClick=""
                            OnGvExtRowDblClick="" SetTemplateValueEnable="False" HighLightRowPosition="1"
                            HorizontalAlign="Left">
                                <Columns>
                                    <asp:BoundField DataField="ProdID" SortExpression="ProdID" />
                                    <asp:BoundField DataField="Model" SortExpression="Model" />
                                    <asp:BoundField DataField="BIOS" SortExpression="BIOS" />
                                    <asp:BoundField DataField="Image" SortExpression="Image" />
                                </Columns>
                            </iMES:GridViewExt>
                        </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
            </tr>
            
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <button id="btnHidden" runat="server" onserverclick="btnHidden_Click" style="display: none" >
                            </button>
                            <input id="pCode" type="hidden" runat="server" /> 
                            <input id="hiddenStation" type="hidden" runat="server" />
                            <button id="hiddenbtn" runat="server" style="display: none">
                            </button>                           
                        </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
            </tr>
        
        </table>
        
        </center>
    </div>
    
</asp:Content>

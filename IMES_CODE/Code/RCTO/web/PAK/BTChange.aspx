<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for BT Change Page
 * UI:CI-MES12-SPEC-PAK-BT_CHANGE.docx –2011/10/28 
 * UC:CI-MES12-SPEC-PAK-BT_CHANGE.docx –2011/10/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-28   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-1379, Jessica Liu, 2012-3-12
*/
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="BTChange.aspx.cs" Inherits="PAK_BTChange" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<style type="text/css">
</style>
 
<script type="text/javascript">


    var msgDataEntryField = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryField").ToString() %>';
    var msgNotifyInput9999 = '<%=this.GetLocalResourceObject(Pre + "_msgNotifyInput9999").ToString()%>';
    var msgSuccess = '<%=this.GetLocalResourceObject(Pre + "_msgSuccess").ToString()%>'; //'<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgChooseChange = '<%=this.GetLocalResourceObject(Pre + "_msgChooseChange").ToString() %>';
    var msgNoInput = '<%=this.GetLocalResourceObject(Pre + "_msgNoInput").ToString() %>';
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    var inputObj = "";
    var inputData = "";
    var station = "";   
    var flag = false;
    //2012-4-11
    //firstInput = false;
    var firstInput = false;
    var key = "";

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onload
    //| Author		:	Jessica Liu
    //| Create Date	:	10/28/2011
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

            key = "";
            
            getAvailableData("processDataEntry");

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }

    }



    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	processDataEntry
    //| Author		:	Jessica Liu
    //| Create Date	:	10/28/2011
    //| Description	:	检测并进行数据检索及显示
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function processDataEntry(inputData) 
    {   
        if(!(document.getElementById("<%=radBTToUnBT.ClientID %>").checked) && 
            !(document.getElementById("<%=radUnBTToBT.ClientID %>").checked))
        {
            alert(msgChooseChange);
            
            getAvailableData("processDataEntry");
            
            return ;
        }     
        
        var uutInput = true;

        if (inputData == "") 
        {
            alert(msgNoInput);
            inputObj.focus();
            uutInput = false;
            
            getAvailableData("processDataEntry");
        }

        if (uutInput) 
        {
            if (firstInput == false)    
            {              
                getAvailableData("processDataEntry");
                
                beginWaitingCoverDiv();

                var line = "";

                key = inputObj.value;
  
                WebServiceBTChange.inputModel(inputObj.value, line, "<%=UserId%>", station, "<%=Customer%>", onMODSucceed, onMODFail);
                
            }
            else    
            {
                getAvailableData("processDataEntry");
      
                if (inputObj.value != "9999")  
                {
                    alert(msgNotifyInput9999);

                    inputObj.value = "";
                    
                    getAvailableData("processDataEntry");
                }
                else 
                {
                    beginWaitingCoverDiv();

                    var line1 = "";

                    var Model = document.getElementById("<%=txtModel.ClientID%>").innerText;
                    
                    var BTTOUnBT = true;
                    if(document.getElementById("<%=radBTToUnBT.ClientID %>").checked)
                    {
                        BTTOUnBT = true;
                    }
                    else if (document.getElementById("<%=radUnBTToBT.ClientID %>").checked)
                    {
                        BTTOUnBT = false;
                    }

                    WebServiceBTChange.DoBTChange(Model, BTTOUnBT, line1, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFail);                                                                    
                }               
            }                    
        }       
    }
        

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onMODSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	10/28/2011
    //| Description	:	第一次Model扫入有效，显示Model
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onMODSucceed(result) 
    {
        var inputModel = inputObj.value;
        
        SetInitStatus();
        
        try {
            if (result == null) 
            {
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 1) && (result[0] == SUCCESSRET)) 
            {
                firstInput = true;
                
                document.getElementById("<%=txtModel.ClientID%>").innerText = inputModel;               
                
                ShowInfo(msgNotifyInput9999); 
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
            }
        } catch (e) {
            alert(e);
            callNextInput();
        }
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onMODFail
    //| Author		:	Jessica Liu
    //| Create Date	:	10/28/2011
    //| Description	:	第一次Model扫入无效，置错误信息
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onMODFail(error) 
    {
        SetInitStatus();
       
        firstInput = false;
        
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
    //| Name		:	onSucceed
    //| Author		:	Jessica Liu
    //| Create Date	:	10/28/2011
    //| Description	:	调用web service inputCustSNOnPizza成功
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onSucceed(result) 
    {
        SetInitStatus();
        
        firstInput = false;
               
        try {

            if (result == null) 
            {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);            
            }
            else if ((result.length == 1) && (result[0] == SUCCESSRET)) 
            {
                //2012-4-11
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, "[" + key + "] " + msgSuccess);                
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
            }
           
            callNextInput();
        } catch (e) {
            alert(e.description);
        }

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onFail
    //| Author		:	Jessica Liu
    //| Create Date	:	10/28/2011
    //| Description	:	调用web service inputCustSNOnPizza失败
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function onFail(error) 
    {
        SetInitStatus();
        
        firstInput = false;
        
            
        try {
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
    //| Create Date	:	10/28/2011
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
    //| Name		:	SetInitStatus
    //| Author		:	Jessica Liu
    //| Create Date	:	10/28/2011
    //| Description	:	处理底层调用返回时，做的控件清空、取消hold界面等初始处理
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function SetInitStatus() 
    {
        ShowInfo("");
        
        endWaitingCoverDiv();
        
        resetAll();

    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onbeforeunload
    //| Author		:	Jessica Liu
    //| Create Date	:	3/12/2012
    //| Description	:	onbeforeunload时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    window.onbeforeunload = function() {
        ExitPage();
    } 

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	ExitPage
    //| Author		:	Jessica Liu
    //| Create Date	:	10/28/2011
    //| Description	:	退出页面时调用
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function ExitPage() 
    {
        //ITC-1360-1379, Jessica Liu, 2012-3-12
        if (firstInput == true)
        {
            WebServiceBTChange.Cancel(key, station);
            sleep(waitTimeForClear);
            firstInput = false;
            key = "";
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	resetAll
    //| Author		:	Jessica Liu
    //| Create Date	:	10/28/2011
    //| Description	:	重置所有控件到初始状态
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function resetAll() 
    {
        inputObj.value = "";
        
        document.getElementById("<%=txtModel.ClientID%>").innerText = "";

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
                <asp:ServiceReference Path="Service/WebServiceBTChange.asmx" />
            </Services>
        </asp:ScriptManager>
         
        <center>
            
        <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">
  
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr >
                <td style="width:20%" align="left" >
                    <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td align="left" >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
             CanUseKeyboard="true" IsPaste="true" MaxLength="50"/>             
                        </ContentTemplate>   
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>                                     
                    </asp:UpdatePanel>
                </td>     
            </tr>
            
            <tr><td>&nbsp;</td><td></td></tr>
     
            <tr>
                <td align="left" >
                    <asp:Label ID="lblModel" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" >
                    <asp:Label ID="txtModel" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
                </td>
            </tr>     
                    
            <tr><td>&nbsp;</td><td></td></tr>

            <tr>
                <td align="left" >
                    <input type="radio" id="radBTToUnBT" name="radAll" runat="server" onclick="" />
                    <asp:Label ID="lblBTToUnBT" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    <br />
                    <input type="radio" id="radUnBTToBT" name="radAll" runat="server" onclick="" />
                    <asp:Label ID="lblUnBTToBT" runat="server" CssClass="iMes_label_13pt"></asp:Label>
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

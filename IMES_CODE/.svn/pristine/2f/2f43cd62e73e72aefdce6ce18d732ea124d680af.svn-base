<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for MB Combine CPU
 * UI:CI-MES12-SPEC-SA-UI MB Combine CPU.docx –2011/12/9 
 * UC:CI-MES12-SPEC-SA-UC MB Combine CPU.docx –2011/12/9            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-1-4   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0202, Jessica Liu, 2012-1-30
* ITC-1360-0203, Jessica Liu, 2012-1-30
* ITC-1360-0345, Jessica Liu, 2012-2-13
* ITC-1360-1663, Jessica Liu, 2012-4-11
* ITC-1360-1675, Jessica Liu, 2012-4-11
* ITC-1360-1675, Jessica Liu, 2012-4-11
* ITC-1360-1860, Jessica Liu, 2012-7-2
*/
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"  ContentType="text/html;Charset=UTF-8" CodeFile="MBCombineCPU.aspx.cs" Inherits="SA_MBCombineCPU" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content2" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<script type="text/javascript">  

    var msgPdLineNull='<%=this.GetLocalResourceObject(Pre + "_msgPdLineNull").ToString() %>';
    var msgMBNull='<%=this.GetLocalResourceObject(Pre + "_msgMBNull").ToString() %>';
    var msgCPUVendorSNNull='<%=this.GetLocalResourceObject(Pre + "_msgCPUVendorSNNull").ToString() %>';
	var msgInvalidPattern='<%=this.GetLocalResourceObject(Pre + "_msgInvalidPattern").ToString() %>';
	var msgInvalidMBSN='<%=this.GetLocalResourceObject(Pre + "_msgInvalidMBSN").ToString() %>';
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var SUCCESSRET ="<%=WebConstant.SUCCESSRET%>";
//    var strPdLine="";
    var strMBDisplay="";
    var strCPUVendorSNDisplay="";          
    var strDataEntry="";
    var station="";
    
    var SessionReStartFlag=false;
    
    document.body.onload = function ()
    {
        station=document.getElementById("<%=station.ClientID %>").value.trim();
        //setPdLineCmbFocus();
        ShowInfo("");
    }

    function displayFun()
    {
        strDataEntry = document.getElementById("<%=txtDataEntry.ClientID %>").value.trim();
        
        strDataEntry = strDataEntry.replace(/^[^0-9a-zA-Z]*/g, '');        
        strDataEntry = strDataEntry.trim().toUpperCase();

        if ((strMBDisplay == "") && (strDataEntry.substring(4, 5) == 'M') && ((strDataEntry.length == 10) || (strDataEntry.length == 11)))         
        {           
        	strDataEntry = SubStringSN(strDataEntry,"MB");    

            MBSNoSFC();  
            return ;
        }

        //ITC-1360-1860, Jessica Liu, 2012-7-2
        /*
        if ((strMBDisplay == "") && (strDataEntry.substring(4, 5) != 'M') && ((strDataEntry.length != 10) || (strDataEntry.length != 11))) {
            alert(msgInvalidMBSN);
            document.getElementById("<%=txtDataEntry.ClientID%>").value = "";
            document.getElementById("<%=txtDataEntry.ClientID%>").focus();
            return;
        }
        */
        if ((strMBDisplay == "") && (strDataEntry.substring(5, 6) == 'M') && (strDataEntry.length == 11)) {
            MBSNoSFC();
            return;
        }
        
        var isMBSN = true;
        if ((strMBDisplay == "") && (strDataEntry.length != 10) && (strDataEntry.length != 11)) {
            isMBSN = false;
        }
        else if ((strMBDisplay == "") && (strDataEntry.length == 10) && (strDataEntry.substring(4, 5) != 'M')) {
            isMBSN = false;
        }
        else if ((strMBDisplay == "") && (strDataEntry.length == 11) && (strDataEntry.substring(4, 5) != 'M') && (strDataEntry.substring(5, 6) != 'M')) {
            isMBSN = false;
        }
    
        if (isMBSN == false)
        {
            alert(msgInvalidMBSN);
            document.getElementById("<%=txtDataEntry.ClientID%>").value = "";
            document.getElementById("<%=txtDataEntry.ClientID%>").focus();
            return;
        }
        

        if ((strMBDisplay != "") && (strCPUVendorSNDisplay == "") && (strDataEntry != ""))
        {
    		if (strDataEntry.length == 14)
    		{
            	document.getElementById("<%=lblCPUVendorSNDisplay.ClientID %>").innerText = strDataEntry;
            	strCPUVendorSNDisplay = strDataEntry;
            	document.getElementById("<%=txtDataEntry.ClientID%>").value = "";
            	document.getElementById("<%=txtDataEntry.ClientID%>").focus();
            	
            	saveFun();
            	return;
            }
            else
            {
            	alert(msgCPUVendorSNNull);	//？？？这么提示是否合适
	            document.getElementById("<%=txtDataEntry.ClientID%>").value = "";
	            document.getElementById("<%=txtDataEntry.ClientID%>").focus();
	            return;
            }
        }
     }
 
    function MBSNoSFC()
    {
        //beginWaitingCoverDiv();
        WebServiceMBCombineCPU.mbsnoSFC(getPdLineCmbValue(),strDataEntry,"<%=UserId%>",station, "<%=Customer%>" ,onMBSNoDisplay,onDisplayFail);
    } 
  
    function onMBSNoDisplay(result)        
    {   
        ShowInfo("");
        //   endWaitingCoverDiv();
        try 
        {
            if(result == null)
            {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if(result == SUCCESSRET)
            {
                strDataEntry = SubStringSN(strDataEntry,"MB");   
                document.getElementById("<%=lblMBDisplay.ClientID %>").innerText = strDataEntry;
                strMBDisplay = strDataEntry;
                document.getElementById("<%=txtDataEntry.ClientID%>").disabled = false;       
                //            debugger 
                //      window.focus();
                // alert("");
            } 
            else 
            {
                var content =result;
                ShowMessage(content);
                ShowInfo(content);
            } 
        } 
        catch(e)
        {
            alert(e.description);
        }

        document.getElementById("<%=txtDataEntry.ClientID%>").value="";
        // document.getElementById("<%=txtDataEntry.ClientID%>").focus();
        SetDataEntryFocus();
    }

    function onDisplayFail(error)
    {
        ShowInfo("");
        //        endWaitingCoverDiv();
        try
        {
            ShowMessage(error.get_message());
            ShowInfo (error.get_message());

        } 
        catch(e) 
        {
            alert(e.description);
        }

        clearCVSN();  
        SessionReStartFlag=false; 
    }	

    function SetDataEntryFocus()
    {
        document.getElementById("<%=txtDataEntry.ClientID%>").disabled=false;
        document.getElementById("<%=txtDataEntry.ClientID%>").focus();
    }

    function saveFun()
    {
        if(checkInput())
        {  
            beginWaitingCoverDiv();
            WebServiceMBCombineCPU.MBCombineCPU(getPdLineCmbValue(), strMBDisplay, strCPUVendorSNDisplay, "<%=UserId%>", station, "<%=Customer%>", SessionReStartFlag, onSucceed, onFail);
        }
    }
   
    function checkInput()
    {
        if (!checkPdLineCmb())
        { 
            alert(msgPdLineNull);
            setPdLineCmbFocus();
            return false;
        }

        if(getPdLineCmbValue() == "" || getPdLineCmbValue() == null)
        {
            alert(msgPdLineNull);
            clearAll();
            setPdLineCmbFocus();
            return false;
        }

        if(strMBDisplay == "")
        {
            alert(msgMBNull);
            document.getElementById("<%=txtDataEntry.ClientID%>").focus();
            return false;
        } 

        if(strCPUVendorSNDisplay == "")
        {
            alert(msgCPUVendorSNNull);
            document.getElementById("<%=txtDataEntry.ClientID%>").focus();
            return false;
        }

        return true;

    }
    
    function EnterOrTab() 
    {
        var inputContent = document.getElementById("<%=txtDataEntry.ClientID%>").value;

        if (event.keyCode == 9 || event.keyCode == 13 ) 
        { 
            inputContent = inputContent.toUpperCase();
            document.getElementById("<%=txtDataEntry.ClientID%>").value = inputContent.trim();
                                 
            if((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null))   
            {
                alert(msgPdLineNull);
                setPdLineCmbFocus();
                document.getElementById("<%=txtDataEntry.ClientID%>").value = "";
                event.returnValue = false;

                //ITC-1360-0202, Jessica Liu, 2012-1-30
                return;
            }

            if (inputContent == null || inputContent == "")
            {
                //ITC-1360-0203, Jessica Liu, 2012-1-30
                if (strMBDisplay == "") 
                {
                    alert(msgMBNull);
                }
                else if ((strMBDisplay != "") && (strCPUVendorSNDisplay == "")) 
                {
                    alert(msgCPUVendorSNNull);
                }
                
                document.getElementById("<%=txtDataEntry.ClientID%>").value = "";
                document.getElementById("<%=txtDataEntry.ClientID%>").focus();
                event.returnValue = false;
            }
            else
            { 
                document.getElementById("<%=displaybtn.ClientID %>").click();
                event.returnValue = false;  
            }
        } 
    }

    
    function onSucceed(result)
    {
        ShowInfo("");  
        endWaitingCoverDiv();
        SessionReStartFlag = false; 
        try 
        {
            if(result == null)
            {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                clearAll();
            }

            else if(result == SUCCESSRET)
            {
                //ITC-1360-1675, Jessica Liu, 2012-4-11
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, "[" + strMBDisplay + "] " + msgSuccess);
                
                clearAll();
            } 
            else 
            {
                var content = result;
                ShowMessage(content);
                ShowInfo(content);
                clearCVSN();
                SessionReStartFlag = true;  
            } 
        } 
        catch(e)
        {
            alert(e.description);
            clearAll();
        }
    }
    
    function onFail(error)
    {
        ShowInfo("");
        endWaitingCoverDiv();
        try
        {
            ShowMessage(error.get_message());
            ShowInfo (error.get_message());
        } 
        catch(e) 
        {
            alert(e.description);
        }

        clearCVSN();  
        SessionReStartFlag = false; 
    }

    
    function clearAll()         
    {
        document.getElementById("<%=txtDataEntry.ClientID%>").value = "";
        //        getPdLineCmbObj().value = "";       
        strMBDisplay = "";
        document.getElementById("<%=lblMBDisplay.ClientID %>").innerText = "";
        strCPUVendorSNDisplay = "";
        document.getElementById("<%=lblCPUVendorSNDisplay.ClientID %>").innerText = "";
        //        document.getElementById("<%=hiddenbtn.ClientID %>").click();  
        //        setPdLineCmbFocus();         
        document.getElementById("<%=txtDataEntry.ClientID%>").focus();
    }

    function clearCVSN()      
    {
        strCPUVendorSNDisplay = "";
        document.getElementById("<%=lblCPUVendorSNDisplay.ClientID %>").innerText = "";
        document.getElementById("<%=txtDataEntry.ClientID%>").value = "";
        document.getElementById("<%=txtDataEntry.ClientID%>").focus();
    }
  

    window.onbeforeunload = function() 
    {
        ExitPage();
    } 

    function ExitPage()
    {
        if(document.getElementById("<%=lblMBDisplay.ClientID %>").innerText != "")
        {
            WebServiceMBCombineCPU.Cancel(document.getElementById("<%=lblMBDisplay.ClientID %>").innerText, station, onClearSucceeded, onClearFailed);
            sleep(waitTimeForClear);
            SessionReStartFlag = false;
        }   
    }

    function onClearSucceeded(result)
    {
        try
        {
            if(result == null)
            {
                ShowInfo("");
                
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if(result == SUCCESSRET)
            {
                ShowInfo("");
            }
            else 
            {
                ShowInfo("");
                var content = result;
                ShowMessage(content);
                ShowInfo(content);
            } 
        }
        catch (e) 
        {
            alert(e.description);
        }
        
        clearAll();
        //     window.setTimeout("SetDataEntryFocus()", 100);
    }

    function onClearFailed(error)
    {
        try
        {
            ShowInfo("");
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
        }
        catch (e)
        {
            alert(e.description);
        } 
        
        clearAll();
    } 


    function ResetPage()
    {
        ExitPage();
        clearAll();
        ShowInfo("");
        document.getElementById("<%=hiddenbtn.ClientID %>").click(); 
    }
 
 
</script>
    
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceMBCombineCPU.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <center >
             
        <table  width="95%"  style="vertical-align:middle" cellpadding="0" cellspacing="0"  >
            <tr><td>&nbsp; </td><td></td></tr>
            <tr >
                <td style="width:20%" align="left" >
                    <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt" Stage="SA"></asp:Label>
                </td>
                <td style="width:75%" align="left" >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <!-- ITC-1360-0345, Jessica Liu, 2012-2-13-->
                            <iMES:CmbPdLine ID="CmbPdLine1" runat="server"  IsPercentage="true" Width="99" />
                        </ContentTemplate>   
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>                                     
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr><td>&nbsp;</td><td></td></tr>
        </table>
            
        <hr style="width:95%" />

        <table width="95%" style=" vertical-align:middle" cellpadding="0" cellspacing="0">
            <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td style="width:20%" align="left" >
                    <asp:Label ID="lblMB" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                </td>
                <td style="width:75%" align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMBDisplay" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
                        </ContentTemplate>   
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>                                     
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr >
                <td align="left" >
                    <asp:Label ID="lblCPUVendorSN" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" >
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblCPUVendorSNDisplay" runat="server" CssClass="iMes_label_11pt" ></asp:Label>
                        </ContentTemplate>   
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>                                     
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr >
                <td align="left" >
                    <%--ITC-1360-1663, Jessica Liu, 2012-4-11--%>
                    <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td align="left" >
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%--ITC-1360-1663, Jessica Liu, 2012-4-11--%>
                            <asp:TextBox ID="txtDataEntry" runat="server"  Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red" Width="98%" Visible="True" MaxLength="35" onkeydown="EnterOrTab()" > </asp:TextBox> 
                        </ContentTemplate>   
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
                        </Triggers>                                     
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td >&nbsp;</td>
                <td> 
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server"  RenderMode="Inline">
                        <ContentTemplate>
                            <button id="hiddenbtn"  runat="server" onserverclick="hiddenbtn_Click" style="display: none" ></button> 
                            <asp:Button ID="displaybtn" runat="server" style="width: 0; display: none;" 
                        OnClientClick="displayFun()" Visible="true"  UseSubmitBehavior="false" />  
                            <input type="hidden" id="hidPdLine" runat="server" />
                            <input type="hidden" id="hidDataEntry" runat="server" />
                            <input type="hidden" runat="server" id="station" /> 
                        </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
            </tr>
        </table>
    </center>
</div>

    

</asp:Content>

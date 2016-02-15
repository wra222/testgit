<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MBAssignTestType.aspx.cs" Inherits="SA_MBAssignTestType" Title="Untitled Page" %>

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
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    var msgeasytest = '<%=this.GetLocalResourceObject(Pre + "_msgeasytest").ToString() %>';
    var msgalltest = '<%=this.GetLocalResourceObject(Pre + "_msgalltest").ToString() %>';
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
        if (!checkPdLineCmb()) {
            alert(msgPdLineNull);
            setPdLineCmbFocus();
            return false;
        }

        if (getPdLineCmbValue() == "" || getPdLineCmbValue() == null) {
            alert(msgPdLineNull);
            clearAll();
            setPdLineCmbFocus();
            return false;
        }
        strDataEntry = document.getElementById("<%=txtDataEntry.ClientID %>").value.trim();  
        strDataEntry = strDataEntry.replace(/^[^0-9a-zA-Z]*/g, '');        
        strDataEntry = strDataEntry.trim().toUpperCase();

        if ((strMBDisplay == "") && (strDataEntry.substring(4, 5) == 'M' || strDataEntry.substring(4, 5) == 'B') && ((strDataEntry.length == 10) || (strDataEntry.length == 11))) {
            strDataEntry = SubStringSN(strDataEntry, "MB");
            document.getElementById("<%=lblMBDisplay.ClientID %>").innerText = strDataEntry;
            MBSNoSFC();
            return;
        }
        else {
            ShowInfo(msgInvalidMBSN);
            clearAll();    
            return;
        }
       
     }
 
    function MBSNoSFC()
    {
        beginWaitingCoverDiv();
        WebServiceMBAssignTestType.mbsnoSFC(getPdLineCmbValue(), strDataEntry, "<%=UserId%>", station, "<%=Customer%>", onMBSNoDisplay, onDisplayFail);
    } 
  
    function onMBSNoDisplay(result)        
    {   
        ShowInfo("");
           endWaitingCoverDiv();
        try 
        {
            if(result == null)
            {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if(result[0] == SUCCESSRET)
            {
                var TestType = result[1];
                strMBDisplay = strDataEntry;
                document.getElementById("<%=txtDataEntry.ClientID%>").disabled = false;
                if (TestType == "1") {
                    ShowSuccessfulInfoWithColor(false, msgSuccess+"  [" + strMBDisplay + "]    " + msgalltest, null, "red");
                }
                else {
                    ShowSuccessfulInfo(true, msgSuccess+"  [" + strMBDisplay + "]    " + msgeasytest);
                }
                

                clearAll();    
              
            } 
            else 
            {
                var content =result[0];
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

        clearAll();         
        SessionReStartFlag=false; 
    }	

    function SetDataEntryFocus()
    {
        document.getElementById("<%=txtDataEntry.ClientID%>").disabled=false;
        document.getElementById("<%=txtDataEntry.ClientID%>").focus();
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

    
  
    
    function clearAll()         
    {
        document.getElementById("<%=txtDataEntry.ClientID%>").value = "";
        strMBDisplay = "";
        document.getElementById("<%=lblMBDisplay.ClientID %>").innerText = "";
        document.getElementById("<%=txtDataEntry.ClientID%>").focus();
    }

    function clearCVSN()      
    {
        strCPUVendorSNDisplay = "";
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
            WebServiceMBAssignTestType.Cancel(document.getElementById("<%=lblMBDisplay.ClientID %>").innerText, station, onClearSucceeded, onClearFailed);
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
            <asp:ServiceReference Path="Service/WebServiceMBAssignTestType.asmx" />
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
                    &nbsp;</td>
                <td align="left" >
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
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




<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CNCardReceive.aspx.cs" Inherits="PAK_CNCardReceive" Title="Untitled Page" %>



<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
         <Services>
             <asp:ServiceReference Path= "Service/WebServiceCNCardReceive.asmx" />
        </Services>
    </asp:ScriptManager>
<div>
   <center >
   <TABLE border="0" width="95%">
    <TR> <%-- ITC-1360-0131 --%>
	    <TD style="width:12%" align="left"><asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label></TD>
	    <TD style="width:85%" align="left"> 
	        <iMES:Input ID="txtInput" runat="server" ProcessQuickInput="true" Width="98%" CanUseKeyboard="true" IsPaste="true" IsClear="true" />	     
	   </TD>    
    </TR>
        
    <TR>
	    <TD style="width:12%" align="left"><asp:Label ID="lbPartNo" runat="server" CssClass="iMes_label_11pt"></asp:Label></TD>
	    <TD width="15%" align="left">
	        <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <asp:Label ID="Label1" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
           </asp:UpdatePanel>
	    </TD>	    
    </TR>
    
    <TR>
	    <TD style="width:12%" align="left"><asp:Label ID="lbBegNo" runat="server" CssClass="iMes_label_11pt"></asp:Label></TD>
	    <TD width="15%" align="left">
	        <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <asp:Label ID="Label2" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
           </asp:UpdatePanel>
	    </TD>	    
    </TR>
        
    <TR>
	    <TD style="width:12%" align="left"><asp:Label ID="lbEndNo" runat="server" CssClass="iMes_label_11pt"></asp:Label></TD>
	    <TD width="15%" align="left">
	        <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <asp:Label ID="Label3" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
           </asp:UpdatePanel>
	    </TD>	    
    </TR>    
    
    <TR>
	    <TD style="width:12%" align="left"><asp:Label ID="lbCount" runat="server" CssClass="iMes_label_11pt"></asp:Label></TD>
	    <TD width="15%" align="left">
	        <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <asp:Label ID="Label4" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
           </asp:UpdatePanel>
	    </TD>	    
    </TR> 
        
        
    <TR>
        <td style="width:12%">&nbsp;</td>
	   <td style="width:85%" align="right"><input id="btsave" type="button"  runat="server" 
               class="iMes_button" onclick="bt_save()" 
               onmouseover="this.className='iMes_button_onmouseover'" 
               onmouseout="this.className='iMes_button_onmouseout'"/></td>   
	</TR>    
        
        
       </TABLE> 
  </center>
</div>


<script type="text/javascript">
    var dataentry;
    var partno = "";
    var begno = "";
    var endno = "";
    var inputObj;
    var editor;
    var customer;
    var stationId;
    //ITC-1360-0132
    var mesWrongCode = '<%=this.GetLocalResourceObject(Pre + "_mesWrongCode").ToString()%>';

    document.body.onload = function() {
        inputObj = getCommonInputObject();
        inputObj.focus();
        editor = "<%=UserId%>";
        customer = "<%=Customer%>";
        stationId = '<%=Request["Station"]%>';
        getAvailableData("processInput");
    }

    function processInput(dataentry) {
        var substring;
        var index;
        var len = dataentry.length;
        if (len == 5) {
            //ITC-1360-0132
            index = dataentry.indexOf("-");
            if (index != 3) {
                alert(mesWrongCode);
                getAvailableData("processInput");
            }
            else {
                partno = dataentry;
                beginWaitingCoverDiv();
                WebServiceCNCardReceive.CheckPartNo(partno, onSucceed, onFail);
                getAvailableData("processInput");
            }
        }
        else if (len == 14) {
            index = dataentry.indexOf("HPNB");
            substring = dataentry.substring(4);
            if (index != 0 || !checkNumber(substring)) {
                alert(mesWrongCode);
                getAvailableData("processInput");
            }
            else {
                begno = document.getElementById("<%=Label2.ClientID%>").innerHTML;
                if (begno.length == 0) {
                    begno = dataentry;
                    beginWaitingCoverDiv();
                    WebServiceCNCardReceive.CheckBegNo(partno, begno, onSucceed1, onFail1);
                    getAvailableData("processInput");
                }
                else {
                    endno = dataentry;
                    if (begno.substring(0, 8) != endno.substring(0, 8)) {
                        alert("两个No的前8位应该相同！");
                        inputObj.focus();
                    }
                    else {
                        beginWaitingCoverDiv();
                        WebServiceCNCardReceive.CheckEndNo(partno, begno, endno, onSucceed2, onFail2);
                    }
                    getAvailableData("processInput");
                }
            }
        }
        //ITC-1360-0145
        else if (dataentry == "9999") {
            beginWaitingCoverDiv();
            WebServiceCNCardReceive.Save_Click(partno, begno, endno, stationId, editor, "", customer, onSucceed3, onFail3);
            getAvailableData("processInput");  
        }
        else {
            alert(mesWrongCode);
            ShowInfo("");
            getAvailableData("processInput");
        }             
    }


    function bt_save() {
        try {
            beginWaitingCoverDiv();
            WebServiceCNCardReceive.Save_Click(partno, begno, endno, stationId, editor, "", customer, onSucceed3, onFail3);
            getAvailableData("processInput");
        } catch (e) {
            alert(e);
            getAvailableData("processInput"); 
        }
    }

    function clearInfo() {
        document.getElementById("<%=Label1.ClientID%>").innerHTML = "";
        document.getElementById("<%=Label2.ClientID%>").innerHTML = "";
        document.getElementById("<%=Label3.ClientID%>").innerHTML = "";
        document.getElementById("<%=Label4.ClientID%>").innerHTML = "";
        partno = "";
        begno = "";
        endno = "";
    }

    function onSucceed3(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                clearInfo();
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result[0] == SUCCESSRET)) {
                endWaitingCoverDiv();
                //ITC-1360-0142
                clearInfo();
                inputObj.focus();
                ShowSuccessfulInfo(true);
            }
            else {
                endWaitingCoverDiv();
                clearInfo();
                ShowInfo("");
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
        }
        catch (e) {        
            alert(e.description);
            endWaitingCoverDiv();
        }
    }

    function onFail3(error) {
        try {
            ShowInfo("");
            ShowInfo(error.get_message());
            ShowMessage(error.get_message());            
            endWaitingCoverDiv();
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }
    
//for check partNo
    function onSucceed(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result[0] == SUCCESSRET)) {
                endWaitingCoverDiv();
                //ITC-1360-0954
                ShowInfo("");
                document.getElementById("<%=Label1.ClientID%>").innerHTML = result[1];
                inputObj.value = "";
                inputObj.focus();                              
            }
            else {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
        }
        catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }

    function onFail(error) {
        try {
            endWaitingCoverDiv();
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }



    function onSucceed1(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);

            }
            else if ((result[0] == SUCCESSRET)) {
                document.getElementById("<%=Label2.ClientID%>").innerHTML = result[1];
                inputObj.value = "";
                inputObj.focus();
                endWaitingCoverDiv();
                ShowInfo("");
            }
            else {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
        }
        catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }

    function onFail1(error) {
        try {
            endWaitingCoverDiv();
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }



    function onSucceed2(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);

            }
            else if ((result[0] == SUCCESSRET)) {
                document.getElementById("<%=Label2.ClientID%>").innerHTML = result[1];
                document.getElementById("<%=Label3.ClientID%>").innerHTML = result[2];
                document.getElementById("<%=Label4.ClientID%>").innerHTML = result[3];
                inputObj.value = "";
                inputObj.focus();
                endWaitingCoverDiv();
            }
            else {
                endWaitingCoverDiv();
                ShowInfo("");
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
        }
        catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }

    function onFail2(error) {
        try {
            endWaitingCoverDiv();
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
        }
    }                   
    //ITC-1360-0133
    function checkNumber(value) {
        var errorFlag = false;
        try {
            var pattern = /^[0-9]{10}$/;
            if (!pattern.test(value)) {
                errorFlag = false;
            }
            else {
                errorFlag = true;
            }
            return errorFlag;
        }
        catch (e) {
            alert(e.description);
        }
    }
    
    
</script>

</asp:Content>
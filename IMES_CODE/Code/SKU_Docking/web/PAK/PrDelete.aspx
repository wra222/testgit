
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PrDelete.aspx.cs" Inherits="PAK_PrDelete" Title="Untitled Page" %>



<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
         <Services>
             <asp:ServiceReference Path= "Service/WebServicePrDelete.asmx" />
        </Services>
    </asp:ScriptManager>
<div>
   <center >
   <TABLE border="0" width="95%">
    <TR>
	    <TD style="width:10%" align="left"><asp:Label ID="lbPrsn" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="7" align="left"> 
            <input type="text" id="txt1" style="width:99%" class="iMes_textbox_input_Yellow"
                    onkeydown="processInput()"  runat="server"  />
	   </TD>    
    </TR>
    
    
    <TR>
	    <TD style="width:10%" align="left"><asp:Label ID="lbProId" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD width="15%" align="left">
	        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <asp:Label ID="Label1" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
           </asp:UpdatePanel>
	    </TD>
	    <TD style="width:5%">&nbsp;</TD>    
    </TR>
    
    <TR>
	    <TD style="width:10%" align="left"><asp:Label ID="lbProSn" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD width="15%" align="left">
	        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <asp:Label ID="Label2" runat="server" CssClass="iMes_label_11pt"></asp:Label>                       
                </ContentTemplate>                                        
           </asp:UpdatePanel>
	    </TD>
	    <TD style="width:5%">&nbsp;</TD>    
    </TR>
    
    <TR>
	    <TD style="width:10%" align="left"><asp:Label ID="lblProdSN" runat="server" CssClass="iMes_label_13pt"></asp:Label></TD>
	    <TD colspan="7" align="left">             
             <input type="text" id="txt2" style="width:99%" class="iMes_textbox_input_Yellow"
                    onkeydown="inputtxt2()"  runat="server"  />
	   </TD>    
    </TR>
    
    <TR>
	   <td align="right" colspan="7" ><input id="btdel" type="button"  runat="server" 
               class="iMes_button" onclick="bt_delete()" 
               onmouseover="this.className='iMes_button_onmouseover'" 
               onmouseout="this.className='iMes_button_onmouseout'" align="right" 
               visible="False"/></td>   
	</TR>
	<tr>
	    <td>
	        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <input type="hidden" runat="server" id="station1" />
                <input type="hidden" runat="server" id="editor1" />
                <input type="hidden" runat="server" id="customer1" /> 
                
                <input type="hidden" runat="server" id="hidModel" />
                <input type="hidden" runat="server" id="hidPartNo" />    
            </ContentTemplate>   
            </asp:UpdatePanel>  
	    </td>
	</tr>
        
        
     </TABLE>   
        
  </center>
</div>


<script type="text/javascript">

    var inputSN;
    var productID;
    var productSN;
    var inputControl;
    var station;
    var editor;
    var customer;
    var checkProductSN;
    var partNo;
    var model;
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var mesInputErrorProductSN = '<%=this.GetLocalResourceObject(Pre + "_mesInputErrorProductSN").ToString()%>';
    var mesNoProductSN = '<%=this.GetLocalResourceObject(Pre + "_mesNoProductSN").ToString()%>';
    var mesDeleteSuccess = '<%=this.GetLocalResourceObject(Pre + "_deleteSuccess").ToString()%>';

    document.body.onload = function() {
        try {
            inputControl = document.getElementById("<%=txt1.ClientID%>");
            inputControl.focus();
            station = document.getElementById("<%=station1.ClientID%>").value;
            editor = document.getElementById("<%=editor1.ClientID%>").value;
            customer = document.getElementById("<%=customer1.ClientID%>").value;
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }
    function inputtxt2() {
        if (event.keyCode == 13 || event.keyCode == 9) {            
            bt_delete();
        }
    }

    function alertAndCallNext(message) {
        endWaitingCoverDiv();
        alert(message);
    }


    function processInput() {
        if (event.keyCode == 13 || event.keyCode == 9) {
            inputSN = document.getElementById("<%=txt1.ClientID%>").value.trim();
            beginWaitingCoverDiv();
            WebServicePrDelete.FindInfoForPrSN(inputSN, onSucceed, onFail);
        }    
    }


    function bt_delete() {
        try {
            productID = document.getElementById("<%=Label1.ClientID%>").innerHTML;
            productSN = document.getElementById("<%=Label2.ClientID%>").innerHTML;
            checkProductSN = document.getElementById("<%=txt2.ClientID%>").value.trim();
            checkProductSN = checkProductSN.toUpperCase();
            model = document.getElementById("<%=hidModel.ClientID%>").value;
            partNo = document.getElementById("<%=hidPartNo.ClientID%>").value;
            if (checkProductSN == "") {
                alert(mesNoProductSN);
                ShowInfo(mesNoProductSN);       
                return;
            }
            if (checkProductSN != productSN) {
                alert(mesInputErrorProductSN);
                clearUI();
                ShowInfo(mesInputErrorProductSN);
                inputControl.focus();
            }
            else {
                beginWaitingCoverDiv();
                WebServicePrDelete.DelPrSN(inputSN, productID, productSN, checkProductSN, partNo, model,
                                        station, editor, "", customer, onSucceed1, onFail1);
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function clearUI() {
        inputControl.value = "";
        document.getElementById("<%=txt2.ClientID%>").value = "";
        document.getElementById("<%=Label1.ClientID%>").innerHTML = "";
        document.getElementById("<%=Label2.ClientID%>").innerHTML = "";
        document.getElementById("<%=hidModel.ClientID%>").value = "";
        document.getElementById("<%=hidPartNo.ClientID%>").value = "";
    }

    function onSucceed1(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                ShowInfo("");
                clearUI();
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();
                clearUI();
                inputControl.focus();
                ShowSuccessfulInfo(true, mesDeleteSuccess);                    
            }
            else {
                endWaitingCoverDiv();
                ShowInfo("");
                clearUI();               
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                inputControl.focus();
            }
        }
        catch (e) {
            endWaitingCoverDiv();
            clearUI();
            alert(e.description);
        }
    }

    function onFail1(error) {
        try {
            endWaitingCoverDiv();
            clearUI();
            ShowInfo("");
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());        
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();        
        }
    }

    function onSucceed(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                clearUI();
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);            
                inputControl.focus();            
            }
            else if (result[0] == SUCCESSRET) {
                endWaitingCoverDiv();
                ShowSuccessfulInfo(true, mesNoProductSN);
                inputControl.value = "";
                document.getElementById("<%=txt2.ClientID%>").focus();                  
                document.getElementById("<%=Label1.ClientID%>").innerHTML = result[1][0];
                document.getElementById("<%=Label2.ClientID%>").innerHTML = result[1][1];
                document.getElementById("<%=hidModel.ClientID%>").value = result[1][2];
                document.getElementById("<%=hidPartNo.ClientID%>").value = result[1][3];                        
            }
            else {
                endWaitingCoverDiv();
                clearUI();
                ShowInfo("");
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);            
                inputControl.focus();            
            }               
        }
        catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
            clearUI();        
        }
    }

    function onFail(error) {
        try {
            endWaitingCoverDiv();
            clearUI();
            ShowInfo("");
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());       
            inputControl.focus();       
        } catch (e) {
            alert(e.description);
            endWaitingCoverDiv();
            clearUI();
        }
    }
</script>

</asp:Content>
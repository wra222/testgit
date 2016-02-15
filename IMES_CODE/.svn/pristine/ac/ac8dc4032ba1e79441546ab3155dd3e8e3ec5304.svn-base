 <%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for REV Label Print For Docking Page
 * UI:CI-MES12-SPEC-FA-UI REV Label Print For Docking.docx –2012/5/28 
 * UC:CI-MES12-SPEC-FA-UC REV Label Print For Docking.docx –2012/5/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-30   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1414-0142, Jessica Liu, 2012-6-13
* ITC-1414-0151、ITC-1414-0152，Jessica Liu, 2012-6-14
* ITC-1414-0178, Jessica Liu, 2012-6-18
* ITC-1414-0179, Jessica Liu, 2012-6-18
*/
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="REVLabelPrintForDocking.aspx.cs" Inherits="Docking_REVLabelPrintForDocking" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<style type="text/css">
</style>
 
<script type="text/javascript">

    var msgSelectDCode = '<%=this.GetLocalResourceObject(Pre + "_msgSelectDCode").ToString() %>';
    //ITC-1414-0142, Jessica Liu, 2012-6-13
    var msgSelectFamily = '<%=this.GetLocalResourceObject(Pre + "_msgSelectFamily").ToString() %>';
    var msgErrorProcess = '<%=this.GetLocalResourceObject(Pre + "_msgErrorProcess").ToString() %>';
    var msgInputQty = '<%=this.GetLocalResourceObject(Pre + "_msgInputQty").ToString() %>';
    var msgBadQty = '<%=this.GetLocalResourceObject(Pre + "_msgBadQty").ToString() %>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    var station = "";
    var editor = "";
    var flag = false;   
    
    
    document.body.onload = function() {
        try {
            ShowInfo("");

            station = document.getElementById("<%=hiddenStation.ClientID %>").value;
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";

        } 
        catch (e) 
        {
            alert(e.description);
            
        }

    }

    function print() 
    {
        try {
            var errorFlag = false;
            var family = getFamilyCmbValue();
            var dcode = getDecodeTypeValue();
            var qty = document.getElementById("<%=txtQty.ClientID %>").value;
            var PattDigit = /^\d*$/;

            if (family == "") 
            {
                errorFlag = true;
                alert(msgSelectFamily);
                setFamilyCmbFocus();
            }
            else if (dcode == "") 
            {
                errorFlag = true;
                alert(msgSelectDCode);
                setDecodeTypeCmbFocus();
            }
            else if (qty == "")
            {
                errorFlag = true;
                alert(msgInputQty);
                document.getElementById("<%=txtQty.ClientID %>").select();
            }
            else if (!PattDigit.exec(qty) || qty < 1 || qty > 100)
            {
                errorFlag = true;
                alert(msgBadQty);
                document.getElementById("<%=txtQty.ClientID %>").select();
            }

            if (!errorFlag) {
                qty = parseInt(qty, 10);
                document.getElementById("<%=txtQty.ClientID %>").innerText = qty;
                var lstPrintItem = getPrintItemCollection();
                
                if (lstPrintItem == null) {
                    alert(msgPrintSettingPara);
                    return;
                }

                beginWaitingCoverDiv();
                WebServiceREVLabelPrintForDocking.print(family, dcode, qty, station, editor, customer, lstPrintItem, onSucceed, onFail);
            }
        } 
        catch (e) 
        {
            alert(e.description);
        }
    }


    function setPrintItemListParam(backPrintItemList, family, dcode) 
    {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();

        keyCollection[0] = "@Family";
        valueCollection[0] = generateArray(family);
        keyCollection[1] = "@Dcode";
        valueCollection[1] = generateArray(dcode);

        /*
        * Function Name: setPrintParam
        * @param: printItemCollection
        * @param: labelType
        * @param: keyCollection(Client: Array of string.    Server: List<string>)
        * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
        */
        setPrintParam(lstPrtItem, "DK_REV_Label", keyCollection, valueCollection);
    }


    function onSucceed(result) 
    {
        ShowInfo("");
        endWaitingCoverDiv();
        
        try {

            if (result == null) 
            {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                
                resetAll();
            }
            else if ((result.length == 2) && (result[0] == SUCCESSRET)) //ITC-1414-0151、ITC-1414-0152，Jessica Liu, 2012-6-14
            {
                //ITC-1414-0178, Jessica Liu, 2012-6-18
                var dcode = getDecodeTypeValue();
                var family = getFamilyCmbValue();
                var retDCode = result[1][0];
                var printQty = result[1][1];
                var printLst = result[1][2];
                setPrintItemListParam(printLst, family, retDCode);
                
                /*
                * Function Name: printLabels
                * @param: printItems
                * @param: isSerial
                */
                for (i = 0; i < printQty; i++)
                {
                    printLabels(printLst, false);
                }

                //ITC-1414-0179, Jessica Liu, 2012-6-18
                ShowSuccessfulInfo(true, "[" + family + ", " + retDCode + "] " + msgSuccess);

                //resetAll();
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);

                resetAll();
            }

        } catch (e) {
            alert(e.description);
        }

    }


    function onFail(error) 
    {
        ShowInfo("");
        endWaitingCoverDiv();
    
        try {
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            resetAll(); 

        } catch (e) {
            alert(e.description);

        }
    }


    function showPrintSettingDialog() 
    {
        showPrintSetting(station, document.getElementById("<%=pCode.ClientID%>").value);
    }


    function ExitPage() 
    {

    }


    function resetAll()
    {
        document.getElementById("<%=txtQty.ClientID%>").innerText = "1";
        document.getElementById("<%=btnHidden.ClientID%>").click();    
    }


    function ResetPage() 
    {
        ExitPage();
        resetAll();
    }    
    
</script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceREVLabelPrintForDocking.asmx" />
            </Services>
        </asp:ScriptManager>
         
        <center>
            
        <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">
  
            <tr><td>&nbsp;</td><td></td></tr>
            
            <tr>
	    		<td align="left" >
	        		<asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    		</td>
	    		<td colspan="3">
	        		<iMES:CmbFamily ID="cmbFamily" runat="server" Width="100" IsPercentage="true"/>
	    		</td>   
    		</tr>
     
            <tr><td>&nbsp;</td><td></td></tr>

            <tr>
	    		<td align="left" >
	        		<asp:Label ID="lblDCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    		</td>
	    		<td colspan="3">
	        		<iMES:CmbDCodeType ID="cmbDataCode" runat="server" Width="100" IsPercentage="true" IsDK="true"/>
	    		</td>   
    		</tr>
     
            <tr><td>&nbsp;</td><td></td></tr>

            <tr>
	    		<td align="left" >
	        		<asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
	    		</td>
	    		<td colspan="3">
	        		<asp:textbox ID="txtQty" runat="server" Width="80" MaxLength="3">1</asp:textbox>
	        		<asp:Label CssClass="iMes_label_13pt" runat="server" Font-Bold="true" style="color:Red">(1-100)</asp:Label>
	    		</td>   
    		</tr>
    		
    		<tr><td>&nbsp;</td><td></td></tr>
    		           
            <tr>
                <td>&nbsp;</td>
                <td align="right">   
                    <input id="btnPrintSetting" style="height:auto" type="button"  runat="server" 
                            onclick="showPrintSettingDialog()" class="iMes_button" 
                            onmouseover="this.className='iMes_button_onmouseover'" 
                            onmouseout="this.className='iMes_button_onmouseout'"/> 
                    &nbsp; 
                    <input id="btnPrint" type="button"  runat="server" class="iMes_button" onclick="print()" />
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

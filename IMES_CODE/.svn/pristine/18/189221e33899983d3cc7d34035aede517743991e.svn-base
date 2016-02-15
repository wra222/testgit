<%--
/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:ECR Version Maintain
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2010-04-27  Tong.Zhi-Yong         Create     
 * Known issues:
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="SmallBoardECR.aspx.cs" Inherits="DataMaintain_SmallBoardECR" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <style type="text/css">
        table.edit
        {
            border: thin solid Black; 
            background-color: #99CDFF;
            margin:0 0 10 0;
        }
        
    .iMes_textbox_input_Normal
    {
	border-width:1px;
	border-style:ridge;
	border-left-color:Black;
	border-right-color:Gray;
	border-bottom-color:Black;
	border-top-color:Gray;
	height: 20px;
    line-height: 20px;
	font-family:Verdana;
	font-size: 9pt;
    text-align:left;
    vertical-align:middle;
    padding-left:2px;
    padding-right:2px;
    margin-left:1px;
    margin-right:1px;
    word-break: break-all;
	word-wrap: break-word;
    }
    </style>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <link rel="stylesheet" type="text/css" href="../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.multiselect.filter.css" />
    <script src="../js/jscal2.js "></script>
    <script src="../js/lang/cn.js "></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%"class="iMes_div_MainTainEdit">
                <tr >
                    <td width="200px">
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <%--<iMESMaintain:CmbMaintainFamilyForSA ID="CmbMaintainFamilyForECRVersion" runat="server" Width="380px" />--%>
                        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbFamilyTop" runat="server" Width="82%"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr> 
            </table>
            
         </div>   
         <div id="div1" class="iMes_div_MainTainDiv1">
               <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblEcrVersionList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                                    
                    <td width="32%" align="right">
                   <input type="button" id="btnDelete" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="true" onclick="if(clkDelete())" onserverclick="Delete_ServerClick"/>
                    </td>    
                </tr>
             </table>                                     
        </div>
            <div id="div2">
                <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                    <Triggers>
                     
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnFamilyChange" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnLoadFirstDataList" EventName="ServerClick" />
                    </Triggers>
                    
                    <ContentTemplate>
                       
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <ContentTemplate>
                 <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="130%" GvExtWidth="100%" GvExtHeight="390px"  onrowdatabound="gd_RowDataBound" OnGvExtRowClick="clickTable(this)" OnDataBound="gd_DataBound" RowStyle-Height="20">
                 
                 </iMES:GridViewExt>
                 </ContentTemplate>
                 </asp:UpdatePanel>
            </div>
            <div id="div3">
            <table width="100%" border="0" style="table-layout:fixed;" class="edit">
                <colgroup>
	                <col style="width:90px;" />
	                <col />
	                <col style="width:100px;" />
	                <col />
	                <col style="width:110px;" />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblFamily2" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <%--<iMESMaintain:CmbMasterLabelFamilyMaintain2 ID="CmbMaintainFamilyForECRVersion2" runat="server" Width="82%" />--%>
                        <asp:DropDownList ID="cmbFamily" runat="server" Width="82%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblMBCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtMBCode" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="2" />
                    </td>
                    <td>
                        
                    </td>                                        
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMBType" runat="server" CssClass="iMes_label_13pt" Text="MBType:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbMBType" runat="server" Width="82%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblECR" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtECR" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="5"/>
                    </td>
                    <td></td>
                </tr>
                 
                <tr>
                    <td>
                        <asp:Label ID="lblIECVersion" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtIECVersion" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="4"/>  
                    </td>
                    <td>
                        <asp:label ID="lblfrom" Text="EffectiveDate:" runat="server"></asp:label>&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="dCalShipdate" runat="server" Width="140px" Height="20px"></asp:TextBox>
                        <button id="btnCal" type="button" style="width: 20px">...</button>
                    </td>    
                    <td></td>                                   
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                       <input type="text" id="ttRemark"  runat="server"  class="iMes_textbox_input_Normal" style="width: 91%;" maxlength="50"/>
                    </td>
                    <td>
                        <input type="button" id="btnSave" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkSave())" onserverclick="Save_ServerClick"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>    
    <input type="hidden" id="hidFamily" runat="server" />
    <input type="hidden" id="hidFamily2" runat="server" />
    <input type="hidden" id="hidDeleteID" runat="server" />
    <input type="hidden" id="dTableHeight" runat="server" />
    <input id="hidStartDate" type="hidden" runat="server" />
    
    <button id="btnLoadFirstDataList" runat="server" type="button" style="display:none" onserverclick ="btnLoadFirstDataList_ServerClick"> </button>
    <button id="btnFamilyChange" runat="server" type="button" style="display:none" onserverclick ="btnFamilyChange_ServerClick"> </button>
    <asp:UpdatePanel ID="updatePanel1" runat="server" RenderMode="Inline" Visible="false" >
    </asp:UpdatePanel>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr></table></div>
    <script type="text/javascript" language="javascript">
        var customer;
        var editor;
        var emptyPattern = /^\s*$/;
        var mbCodePattern = /^[0-9A-Z]{2}$/;
        var ecrPattern = /^[0-9A-Z]{5}$/;
        var iecVersionPattern = /^[0-9]\.[0-9]{2}$/;
        var custVersionPattern = /^[0-9A-Z]{3}$/;
        var emptyString = "";
        var selectedRowIndex = -1;
        var toDate = document.getElementById("<%=dCalShipdate.ClientID %>");
        
        //error message
        var msgInputFamily = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputFamily").ToString() %>';
        var msgInputMBCode = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMBCode").ToString() %>';
        var msgInputECR = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputECR").ToString() %>';
        var msgInputIECVersion = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputIECVersion").ToString() %>';
        var msgMBCodeFormatError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMBCodeFormatError").ToString() %>';
        var msgECRFormatError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgECRFormatError").ToString() %>';
        var msgIECVersionFormatError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgIECVersionFormatError").ToString() %>';
        var msgCustVersionFormatError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgCustVersionFormatError").ToString() %>';
        var msgConfirmDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgConfirmDelete").ToString() %>';
        
        window.onload = function()
        {
            resetTableHeight();
            initContorls();
            //showFirstFamilyDataList();
            EndRequestHandler();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            toDate.value = "<%=today%>";
        };

        function EndRequestHandler(sender, args) {
            Calendar.setup({
                trigger: "btnCal",
                inputField: "<%=dCalShipdate.ClientID%>",
                onSelect: updateCalendarFields,
                onTimeChange: updateCalendarFields,
                showTime: 24,
                dateFormat: "%Y-%m-%d %H:%M",
                minuteStep: 1
            });
        };
        
        function showFirstFamilyDataList()
        {
            document.getElementById("<%=btnLoadFirstDataList.ClientID%>").click();
        }
        
        function initContorls()
        {
            var tempname = document.getElementById("<%=cmbFamilyTop.ClientID%>");
            tempname.onchange=cmbFamilyTopChange;
        }
         function cmbFamilyTopChange()
        {
            clkQuery();
            document.getElementById("<%=btnFamilyChange.ClientID%>").click();
           
        }
        
        function resetTableHeight()
        {
        //动态调整表格的高度
        var adjustValue=60;     
        var marginValue=10;  
        var tableHeigth=300;

        
        try{
            tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div4.offsetHeight-div3.offsetHeight-adjustValue;
        }
        catch(e){
            //ignore
        }                
        //为了使表格下面有写空隙
        var extDivHeight=tableHeigth+marginValue;
       
        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
        //alert(document.getElementById("div_<%=gd.ClientID %>").style.height)
        div2.style.height=extDivHeight+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
        }
        
        function clickTable(con)
        {
            if((selectedRowIndex!=null) && (selectedRowIndex!=parseInt(con.index, 10)))
            {
                setRowSelectedOrNotSelectedByIndex(selectedRowIndex,false, "<%=gd.ClientID %>");                
            }
            
            setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
            selectedRowIndex=parseInt(con.index, 10);
            
            setDetailInfo();      
            
            if (hasEditData())
            {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
            }     
            else
            {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            } 
        }
        
        function hasEditData()
        {
            var tblObj = document.getElementById("<%=gd.ClientID %>");
        
            if (selectedRowIndex == -1 || emptyPattern.test(tblObj.rows[selectedRowIndex + 1].cells[0].innerText))
            {
                return false;
            }
            
            return true;
        }        
        
        function clkQuery()
        {
            var familyValue = document.getElementById("<%=cmbFamilyTop.ClientID %>").value;
            
            if (emptyPattern.test(familyValue))
            {
                alert(msgInputFamily);
                document.getElementById("<%=cmbFamilyTop.ClientID %>").focus();
                return false;
            }
            
            selectedRowIndex = -1;
            document.getElementById("<%=hidFamily.ClientID %>").value = familyValue;
            
            clearDetailInfo();
            
   //         beginWaitingCoverDiv();
            ShowWait();
            return true;
        }
        
        function setDetailInfo()
        {
            var tblObj = document.getElementById("<%=gd.ClientID %>");
            var row = tblObj.rows[selectedRowIndex + 1];

            if(row.cells[1].innerText.trim()=="")
            {

                document.getElementById("<%=cmbFamily.ClientID %>").value = "";
                document.getElementById("<%=txtMBCode.ClientID %>").value = "";
                document.getElementById("<%=cmbMBType.ClientID %>").value = "";
                document.getElementById("<%=txtECR.ClientID %>").value =  "";
                document.getElementById("<%=txtIECVersion.ClientID %>").value = "";
                toDate.value = "<%=today%>";
                document.getElementById("<%=ttRemark.ClientID %>").value =  "";
            }
            else
            {
                document.getElementById("<%=cmbFamily.ClientID %>").value = row.cells[1].innerText;
                document.getElementById("<%=txtMBCode.ClientID %>").value = row.cells[2].innerText;
                document.getElementById("<%=cmbMBType.ClientID %>").value = row.cells[3].innerText;
                document.getElementById("<%=txtECR.ClientID %>").value = row.cells[4].innerText;
                document.getElementById("<%=txtIECVersion.ClientID %>").value = row.cells[5].innerText;
                toDate.value = row.cells[6].innerText;
                document.getElementById("<%=ttRemark.ClientID %>").value = row.cells[7].innerText;
            }
           
    //        
        }
        
        function clearDetailInfo()
        {
            document.getElementById("<%=cmbFamily.ClientID %>").value = "";
            document.getElementById("<%=txtMBCode.ClientID %>").value = emptyString;
            document.getElementById("<%=txtECR.ClientID %>").value = emptyString;
            document.getElementById("<%=txtIECVersion.ClientID %>").value = emptyString;
            toDate.value = "<%=today%>";
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        }
        
        function clkSave()
        {
            if (checkInput())
            {
                var familyValue = document.getElementById("<%=cmbFamily.ClientID %>").value;
                document.getElementById("<%=hidFamily2.ClientID %>").value = familyValue;
                
                if (hasEditData())
                {
                    var tblObj = document.getElementById("<%=gd.ClientID %>");
                    var row = tblObj.rows[selectedRowIndex + 1];
             
                    document.getElementById("<%=hidDeleteID.ClientID %>").value = row.cells[0].innerText;
                }
                else
                {
                    document.getElementById("<%=hidDeleteID.ClientID %>").value = emptyString;
                }
                document.getElementById("<%=hidStartDate.ClientID %>").value = toDate.value;
                selectedRowIndex = -1;
     //           beginWaitingCoverDiv();
                ShowWait();
                return true;
            }
            
            return false;           
        }
        
        function checkInput()
        {
            var familyValue = document.getElementById("<%=cmbFamily.ClientID %>").value;
            
            if (emptyPattern.test(familyValue))
            {
                alert(msgInputFamily);
                document.getElementById("<%=cmbFamily.ClientID %>").focus();
                return false;
            }
            
            var mbCodeObj = document.getElementById("<%=txtMBCode.ClientID %>");
            
            if (emptyPattern.test(mbCodeObj.value))
            {
                alert(msgInputMBCode);
                mbCodeObj.value = emptyString;
                mbCodeObj.focus();
                return false;
            }
            
            if (!mbCodePattern.test(mbCodeObj.value.toUpperCase()))
            {
                alert(msgMBCodeFormatError);
                mbCodeObj.focus();
                return false;
            }
            
            var ecrObj = document.getElementById("<%=txtECR.ClientID %>");
            
            if (emptyPattern.test(ecrObj.value))
            {
                alert(msgInputECR);
                ecrObj.value = emptyString;
                ecrObj.focus();
                return false;
            }
            
            if (!ecrPattern.test(ecrObj.value.toUpperCase()))
            {
                alert(msgECRFormatError);
                ecrObj.focus();
                return false;
            }

            var mbTypeObj = document.getElementById("<%=cmbMBType.ClientID %>");

            if (emptyPattern.test(mbTypeObj.value))
            {
                alert("Please Select MBType");
                mbTypeObj.focus();
                return false;
            }
            return true;      
        }
        
        function clkDelete()
        {
            if (confirm(msgConfirmDelete))
            {
                var tblObj = document.getElementById("<%=gd.ClientID %>");
                var row = tblObj.rows[selectedRowIndex + 1];
             
                document.getElementById("<%=hidDeleteID.ClientID %>").value = row.cells[0].innerText;
                selectedRowIndex = -1;
                clearDetailInfo();
       //         beginWaitingCoverDiv();
                ShowWait();
                return true;
            }
            
            return false;
        }
    </script>
</asp:Content>


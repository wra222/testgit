<%--
/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:ECR Version Maintain
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * Known issues:
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="SMTLineSpeedMaintain.aspx.cs" Inherits="DataMaintain_SMTLineSpeedMaintain" Title="" %>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%"class="iMes_div_MainTainEdit">
                <tr >
                    <td width="50px">
                        <asp:Label ID="lblLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMESMaintain:CmbMaintainLineForSMT ID="CmbMaintainLineForSMT" runat="server" Width="380px" />
                    </td>
                    
                    <td width="50px">
                        <asp:Label ID="lblMode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtMode" runat="server" class="iMes_textbox_input_Normal" Width="20px" maxlength="10" onkeydown="SetHight()"/>
                    </td>
                </tr> 
            </table>
            
         </div>   
         <div id="div1" class="iMes_div_MainTainDiv1">
               <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblSMTLineSpeed" runat="server" CssClass="iMes_label_13pt"></asp:Label>
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
                        <asp:AsyncPostBackTrigger ControlID="btnFamilyTextChange" EventName="ServerClick" />
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
                        <asp:Label ID="lblMode1" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <iMESMaintain:CmbMaintainFamilyForSMT ID="CmbMaintainFamilyForSMT" runat="server" Width="82%" />
                    </td>
                    <td>
                        <asp:Label ID="lblSpeed" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtSpeed" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="5" />秒
                    </td>
                    <td>
                        <input type="button" id="btnAdd" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkAdd())" onserverclick="Add_ServerClick"/>
                    </td>                                         
                </tr> 
                <tr>
                    <td>
                        <asp:Label ID="lblRate" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <input type="text" id="txtRate" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="5"/>%
                    </td>    
                    <td>
                        <asp:Label ID="lblOther" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                       <input type="text" id="txtOther" runat="server" class="iMes_textbox_input_Normal" style="width: 80%;" maxlength="100"/> 
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
    <input type="hidden" id="hidMode" runat="server" />

    
    <button id="btnLoadFirstDataList" runat="server" type="button" style="display:none" onserverclick ="btnLoadFirstDataList_ServerClick"> </button>
    <button id="btnFamilyChange" runat="server" type="button" style="display:none" onserverclick ="btnFamilyChange_ServerClick"> </button>
     <button id="btnFamilyTextChange" runat="server" type="button" style="display:none" onserverclick ="btnFamilyTextChange_ServerClick"> </button>

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
            showFirstFamilyDataList();
        };
        
        function showFirstFamilyDataList()
        {
            document.getElementById("<%=btnLoadFirstDataList.ClientID%>").click();
        }
        
        function initContorls()
        {
            var tempname = eval("<%=CmbMaintainLineForSMT.ClientID %>getFamilyCmbObj()");
            tempname.onchange = cmbFamilyTopChange;
            
        }
        function cmbFamilyTopChange()
        {
            clkQuery();
            document.getElementById("<%=btnFamilyChange.ClientID%>").click();
           
        }
        function cmbFamilyTextChange() {
            //clkQuery();
            alert("run this\n");
            document.getElementById("<%=btnFamilyTextChange.ClientID%>").click();            
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
            var LineValue = eval("<%=CmbMaintainLineForSMT.ClientID %>getFamilyCmbObj().value;");

            if (emptyPattern.test(LineValue))
            {
                alert(msgInputFamily);
                eval("<%=CmbMaintainLineForSMT.ClientID %>getFamilyCmbObj().focus();");
                return false;
            }
            
            selectedRowIndex = -1;
            document.getElementById("<%=hidFamily.ClientID %>").value = LineValue;
            
            clearDetailInfo();
            
   //         beginWaitingCoverDiv();
            ShowWait();
            return true;
        }
        function setDetailInfoNew(row) {
           
            {
                eval("<%=CmbMaintainFamilyForSMT.ClientID %>getFamilyCmbObj().value='" + row.cells[1].innerText + "'");
                document.getElementById("<%=txtSpeed.ClientID %>").value = row.cells[3].innerText;
                document.getElementById("<%=txtRate.ClientID %>").value = row.cells[2].innerText;
                document.getElementById("<%=txtOther.ClientID %>").value = row.cells[4].innerText;
            }

            //        
        }
        
        function setDetailInfo()
        {
            var tblObj = document.getElementById("<%=gd.ClientID %>");
            var row = tblObj.rows[selectedRowIndex + 1];

            /*if(row.cells[1].innerText.trim()=="")
            {

                eval("<%=CmbMaintainFamilyForSMT.ClientID %>getFamilyCmbObj().value=''");
                document.getElementById("<%=txtSpeed.ClientID %>").value = "";
                document.getElementById("<%=txtRate.ClientID %>").value =  "";
                document.getElementById("<%=txtOther.ClientID %>").value =  "";
            }
            else*/
            {
                eval("<%=CmbMaintainFamilyForSMT.ClientID %>getFamilyCmbObj().value='" + row.cells[1].innerText + "'");
                 document.getElementById("<%=txtSpeed.ClientID %>").value = row.cells[3].innerText;
                 document.getElementById("<%=txtRate.ClientID %>").value = row.cells[2].innerText;
                document.getElementById("<%=txtOther.ClientID %>").value = row.cells[4].innerText;
            }
           
    //        
        }
        
        function clearDetailInfo()
        {
            eval("<%=CmbMaintainFamilyForSMT.ClientID %>getFamilyCmbObj().value=''");
            document.getElementById("<%=txtSpeed.ClientID %>").value = emptyString;
            document.getElementById("<%=txtRate.ClientID %>").value = emptyString;
            document.getElementById("<%=txtOther.ClientID %>").value = emptyString;
     
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        }
        
        function clkSave()
        {
            if (checkInput()) {

                var familyValue = eval("<%=CmbMaintainFamilyForSMT.ClientID %>getFamilyCmbObj().value;");
                document.getElementById("<%=hidFamily2.ClientID %>").value = familyValue;
                
                if (hasEditData())
                {
                    var tblObj = document.getElementById("<%=gd.ClientID %>");
                    var row = tblObj.rows[selectedRowIndex + 1];
             
                    document.getElementById("<%=hidDeleteID.ClientID %>").value = row.cells[7].innerText;
                }
                else
                {
                    document.getElementById("<%=hidDeleteID.ClientID %>").value = emptyString;
                }

              
                selectedRowIndex = -1;
     //           beginWaitingCoverDiv();
                ShowWait();
                return true;
            }
            
            return false;
        }
        
        function clkAdd() {
            if (checkInput()) {
                var familyValue = eval("<%=CmbMaintainFamilyForSMT.ClientID %>getFamilyCmbObj().value;");
                document.getElementById("<%=hidFamily2.ClientID %>").value = familyValue;
                
                var LineValue = eval("<%=CmbMaintainLineForSMT.ClientID %>getFamilyCmbObj().value;");           
                document.getElementById("<%=hidFamily.ClientID %>").value = LineValue;

                if (hasEditData()) {
                    var tblObj = document.getElementById("<%=gd.ClientID %>");
                    var row = tblObj.rows[selectedRowIndex + 1];

                    document.getElementById("<%=hidDeleteID.ClientID %>").value = row.cells[7].innerText;
                }
                else {
                    document.getElementById("<%=hidDeleteID.ClientID %>").value = emptyString;
                }

                selectedRowIndex = -1;
                //           beginWaitingCoverDiv();
                ShowWait();
                return true;
            }

            return false;
        }
        function checkInput()
        {
           // var familyValue = eval("<%=CmbMaintainFamilyForSMT.ClientID %>getFamilyCmbObj().value;");
            var s = document.getElementById("<%=txtSpeed.ClientID %>").value;
            
              var str =/^\d{1,4}(\.\d)?$/ ;
              if (str.test(s)) {
              
              return true;
              }
              else {
                  alert("线速数据输入错误");
                  document.getElementById("<%=txtSpeed.ClientID %>").value= "";
              return false;
              }      
                             
            return true;      
        }
        function SetHight() {
            if (event.keyCode == 13) {
                if ((selectedRowIndex != null) ) {
                    setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, "<%=gd.ClientID %>");
                }
                var i = CheckInTable(document.getElementById("<%=txtMode.ClientID %>").value);

                setRowSelectedOrNotSelectedByIndex(i - 1, true, "<%=gd.ClientID %>");
                selectedRowIndex = i - 1;
                setDetailInfo();
             }
        }

        function CheckInTable(family) {
            var tbl = "<%=gd.ClientID %>";
            var table = document.getElementById(tbl);
            var subFindFlag = false;
            for (var i = 1; i < table.rows.length; i++) {
                //alert(table.rows[i].cells[0].innerText.trim());
                if ((table.rows[i].cells[1].innerText.trim() == family)) {
                    //subFindFlag = true;
                    break;
                }
            }
            return i;
        }
    
        function clkDelete()
        {
            if (confirm(msgConfirmDelete))
            {
                var tblObj = document.getElementById("<%=gd.ClientID %>");
                var row = tblObj.rows[selectedRowIndex + 1];

                document.getElementById("<%=hidDeleteID.ClientID %>").value = row.cells[7].innerText;
                document.getElementById("<%=hidMode.ClientID %>").value = row.cells[1].innerText;
                selectedRowIndex = -1;
                clearDetailInfo();
       //         beginWaitingCoverDiv();
                ShowWait();
                return true;
            }
            
            return false;
        }


        function AddUpdateComplete(id) {


            var gdObj = document.getElementById("<%=gd.ClientID %>");

            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[7].innerText == id) {
                    selectedRowIndex = i;
                }
            }

            if (selectedRowIndex < 0) {
                eval("<%=CmbMaintainFamilyForSMT.ClientID %>getFamilyCmbObj().value=''");
                document.getElementById("<%=txtSpeed.ClientID %>").value = emptyString;
                document.getElementById("<%=txtRate.ClientID %>").value = emptyString;
                document.getElementById("<%=txtOther.ClientID %>").value = emptyString;
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
              //  setSrollByIndex(selectedRowIndex, true, "<%=gd.ClientID%>");
                setDetailInfoNew(con);
            }
        }
        
        
        function setGdHighLight(con) {
            if ((selectedRowIndex != null) && (selectedRowIndex != parseInt(con.index, 10))) {
                //去掉过去高亮行
                setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, "<%=gd.ClientID %>");
            }
            //设置当前高亮行   
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            //记住当前高亮行
            iSelectedRowIndex = parseInt(con.index, 10);
            //selectedRowIndex = parseInt(con.index, 10);
        }
    </script>
</asp:Content>


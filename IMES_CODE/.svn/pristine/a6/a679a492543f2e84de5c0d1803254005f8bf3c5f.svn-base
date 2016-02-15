<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="DeptMaintainForRCTO.aspx.cs" Inherits="DataMaintain_DeptMaintainForRCTO" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="iMes_div_MainTainEdit" >            
                <tr >
                    <td style="width: 200px;">
                        <asp:Label ID="lblDept" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="42%">
                        <iMESMaintain:CmbMaintainDept runat="server" ID="cmbMaintainDept" Width="66%" ></iMESMaintain:CmbMaintainDept>
                    </td>                                    
                    <td style="width: 80px;">
                        <asp:Label ID="lblSection" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="42%">
                        <iMESMaintain:CmbMaintainSection runat="server" ID="cmbMaintainSection" Width="36%" ></iMESMaintain:CmbMaintainSection>
                    </td>    
           
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblLineList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="40%"></td> 
                    <td align="right">
                       <input type="button" id="btnDelete" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnDelete_ServerClick"></input>
                    </td>            
                </tr>
             </table>  
        </div>


        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" ChildrenAsTriggers="false">
        <ContentTemplate>
        <div id="div2" style="height:400px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%" RowStyle-Height="20"
                        GvExtWidth="100%" GvExtHeight="390px" Height="382px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel> 
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>
    
                        <td style="width: 80px;">
                            <asp:Label ID="lblLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td> 
                        <td width="20%">
                            <asp:TextBox ID="txtLine" runat="server" MaxLength="4" CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>
                        </td>       
                        <td style="width: 100px;">
                            <asp:Label ID="lblStart" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="20%">
                         <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtStart" runat="server" MaxLength="10"  CssClass="iMes_textbox_input_Yellow" /></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                       
                        </td>                      
                        <td style="width: 80px;">
                            <asp:Label ID="lblEnd" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="20%" >
                         <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEnd" runat="server" MaxLength="10"  CssClass="iMes_textbox_input_Yellow" /></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>                        
                        </td>                      
                                  
                    </tr>
                    <tr>

                        <td style="width: 80px;">
                            <asp:Label ID="lblFISLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="20%">
                            <asp:TextBox ID="txtFISLine" runat="server" MaxLength="4" CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>
                        </td>
                        <td style="width: 100px;">
                            <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="20%">
                            <asp:TextBox ID="txtRemark" runat="server" MaxLength="50" SkinId="textBoxSkin" ></asp:TextBox>
                        </td>                          
                        <td style="width: 80px;">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></input>
                        </td>           
                    </tr>                    
                      
            </table> 
        </div>  
   
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
            <asp:AsyncPostBackTrigger ControlID="btnDeptChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSectionChange" EventName="ServerClick" />
        </Triggers>                      
        </asp:UpdatePanel>
         
           <input type="hidden" id="HiddenUserName" runat="server" />
           <input type="hidden" id="dTableHeight" runat="server" />   
           <input type="hidden" id="dOldDept" runat="server" />   
            <input type="hidden" id="dOldSection" runat="server" />   
           <input type="hidden" id="dOldLine" runat="server" />   
            <input type="hidden" id="dOldID" runat="server" />   
         <input type="button" id="btnDeptChange" runat="server" style="display:none" onserverclick ="btnDeptChange_ServerClick"> </input>
           <input type="button" id="btnSectionChange" runat="server" style="display:none" onserverclick ="btnSectionChange_ServerClick"> </input>
    
    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    <script language="javascript" type="text/javascript">

        var msgSelectOne = "";
        var msgDelConfirm = "";
        var msgSelDept = "";
        var msgSelSection = "";
        var msgInputLine = "";
        var msgInputFISLine = "";
        var msgInputStart = "";
        var msgInputEnd = "";

        function clkButton() {
            switch (event.srcElement.id) {
                case "<%=btnSave.ClientID %>":

                    if (clkSave() == false) {
                        return false;
                    }
                    break;

                case "<%=btnDelete.ClientID %>":

                    if (clkDelete() == false) {
                        return false;
                    }
                    break;
            }
            ShowWait();
            return true;
        }

        var iSelectedRowIndex = null;
        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }


        function initControls() {
            getMaintainDeptCmbObj().onchange = DeptSelectOnChange;
            getMaintainSectionCmbObj().onchange = SectionSelectOnChange;

        }

        function SectionSelectOnChange() {
            document.getElementById("<%=btnSectionChange.ClientID%>").click();
            ShowWait();
        }

        function DeptSelectOnChange() {
            document.getElementById("<%=btnDeptChange.ClientID%>").click();
            ShowWait();
        }

        window.onload = function() {

            msgSelectOne = "<%=strmsgSelectOne%>";
            msgDelConfirm = "<%=strmsgDelConfirm%>";
            msgSelDept = "<%=strmsgSelDept%>";
            msgSelSection = "<%=strmsgSelSection%>";
            msgInputLine = "<%=strmsgSelLine%>";
            msgInputFISLine = "<%=strmsgSelFISLine%>";
            msgInputStart = "<%=strmsgSelStart%>";
            msgInputEnd = "<%=strmsgSelEnd%>";

            document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";
            initControls();
            ShowRowEditInfo(null);
            //设置表格的高度  
            resetTableHeight();

        };

        //设置表格的高度  
        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 55;
            var marginValue = 12;
            var tableHeigth = 300;
            //var tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
            try {
                tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
            }
            catch (e) {

                //ignore
            }
            //为了使表格下面有写空隙
            var extDivHeight = tableHeigth + marginValue;
            div2.style.height = extDivHeight + "px";
            document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
        }

        function clkDelete() {
            var gdObj = document.getElementById("<%=gd.ClientID %>")
            var curIndex = gdObj.index;
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            if (curIndex >= recordCount) {
                alert(msgSelectOne);
                //            alert("需要先选择一条记录");
                return false;
            }

            //        var ret = confirm("确定要删除这条记录么？");
            var ret = confirm(msgDelConfirm);
            if (!ret) {
                return false;
            }

            return true;

        }

        function DeleteComplete() {
            ShowRowEditInfo(null);
        }

        function clkSave() {
            //ShowInfo("");
            return check();

        }

        function check() {
        
            var LineValue = document.getElementById("<%=txtLine.ClientID %>").value.trim();

            if (getMaintainDeptCmbObj().value.trim() == "") {
                alert(msgSelDept);
                //            alert("请选择Dept!");
                return false;
            }

            if (getMaintainSectionCmbObj().value.trim() == "") {
                alert(msgSelSection);
                setMaintainSectionCmbFocus();
                //            alert("请选择 Section!");
                return false;
            }

            if (LineValue == "") {
                alert(msgInputLine);
                document.getElementById("<%=txtLine.ClientID %>").focus();
               
                //           alert("请输入[线别]!");
                return false;
            }

            var FISLineValue = document.getElementById("<%=txtFISLine.ClientID %>").value.trim();
            if (FISLineValue == "") {
                alert(msgInputFISLine);
                document.getElementById("<%=txtFISLine.ClientID %>").focus();

                //           alert("请输入[FIS线别]!");
                return false;
            }
            
            var StartValue = document.getElementById("<%=txtStart.ClientID %>").value.trim();
            if (StartValue == "") {
                alert(msgInputStart);
                document.getElementById("<%=txtStart.ClientID %>").focus();
                
                //           alert("请输入[开线时间]!");
                return false;
            }
            
            var EndValue = document.getElementById("<%=txtEnd.ClientID %>").value.trim();
            if (EndValue == "") {
                alert(msgInputEnd);
                document.getElementById("<%=txtEnd.ClientID %>").focus();

                //           alert("请输入[结束时间]!");
                return false;
            }

            return true;

        }


        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);

        }

        function setNewItemValue() {

            document.getElementById("<%=txtLine.ClientID %>").value = ""
            document.getElementById("<%=txtFISLine.ClientID %>").value = "";
            //document.getElementById("<%=txtStart.ClientID %>").value = "";
            //document.getElementById("<%=txtEnd.ClientID %>").value = "";
            document.getElementById("<%=txtStart.ClientID %>").value = "08:00:00";
            document.getElementById("<%=txtEnd.ClientID %>").value = "20:00:00";
 
            document.getElementById("<%=txtRemark.ClientID %>").value = "";

            document.getElementById("<%=dOldDept.ClientID %>").value = "";
            document.getElementById("<%=dOldSection.ClientID %>").value = "";
            document.getElementById("<%=dOldLine.ClientID %>").value = "";

            //        document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        }
        //SELECT [Line]
        //      ,[Descr]
        //      ,[Editor]
        //      ,[Cdt]
        //      ,[Udt]
        //      ,[CustomerID]
        //      ,[Stage]
        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }

            document.getElementById("<%=txtLine.ClientID %>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=txtFISLine.ClientID %>").value = con.cells[3].innerText.trim();
            document.getElementById("<%=txtStart.ClientID %>").value = con.cells[4].innerText.trim();
            document.getElementById("<%=txtEnd.ClientID %>").value = con.cells[5].innerText.trim();
            document.getElementById("<%=txtRemark.ClientID %>").value = con.cells[7].innerText.trim();
 
            document.getElementById("<%=dOldID.ClientID %>").value = con.cells[8].innerText.trim();

            var currentDept = con.cells[0].innerText.trim();
            document.getElementById("<%=dOldDept.ClientID %>").value = currentDept;

            var currentSection = con.cells[1].innerText.trim();
            document.getElementById("<%=dOldSection.ClientID %>").value = currentSection;

            var currentLine = con.cells[2].innerText.trim();
            document.getElementById("<%=dOldLine.ClientID %>").value = currentLine;

            if (currentDept == "") {
                setNewItemValue();
            }
            else {
                //            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
            }
            //trySetFocus();
        }

        function trySetFocus() {
            var itemObj = document.getElementById("<%=txtLine.ClientID %>"); //getMaintainFamilyCmbObj();

            if (itemObj != null && itemObj != undefined && itemObj.disabled != true) {
                itemObj.focus();
            }
        }

        function AddUpdateComplete(id) {

            var gdObj = document.getElementById("<%=gd.ClientID %>");

            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[8].innerText == id) {
                    selectedRowIndex = i;
                    break;
                }
            }

            if (selectedRowIndex < 0) {
                ShowRowEditInfo(null);
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                ShowRowEditInfo(con);

            }

        }

        function DealHideWait() {
            HideWait();
            getMaintainDeptCmbObj().disabled = false;
            getMaintainSectionCmbObj().disabled = false;


        }

    </script>
</asp:Content>


<%--
/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for PAKCHN(TW)LabelLightNo Page
 *             
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/05/18
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/05/18            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-18  Kaisheng             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PAKCHN(TW)LabelLightNo.aspx.cs" Inherits="DataMaintain_PAKLabelLightNo" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:ScriptManager ID="ScriptManager1" runat="server" >       
</asp:ScriptManager>
    
     <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">                                  
             <table width="100%" border="0" >
                <tr>
                   <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lbLightNoList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="42%">  
                        <asp:TextBox ID="LightNoList" runat="server"   MaxLength="12"  Width="50%" CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)' Visible="False"></asp:TextBox>
                    </td> 
                    <td align="right">
                      <input type="button" id="btnDelete" onclick="if(clkButton())"  class="iMes_button" runat="server" onserverclick="btnDelete_ServerClick"/>
                   </td>           
                </tr>
             </table>
        </div>

        <asp:UpdatePanel ID="updatePanelAll" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <Triggers>            
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
        </Triggers>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
        <div id="div2" style="height:366px">

                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                        Width="130%" RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' 
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false" 
                        style="top: -1px; left: 23px">
                    </iMES:GridViewExt>
                    
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                    <tr>
                        <td width="8%" align="left">
                            <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="25%" align="left">
                            <asp:TextBox ID="txtModel" runat="server"  width="95%"  MaxLength="50"  CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>
                        <td width="8%" align="left">
                            <asp:Label ID="lbType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="25%" align="left">
                            <asp:TextBox ID="txtType" runat="server" SkinId="textBoxSkin" width="95%"  MaxLength="50" CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>
                        <td width="8%" align="left">
                            <asp:Label ID="lbLightNo" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="15%" align="left">
                            <asp:TextBox ID="txtLightNo" onkeypress="input0To999Number(this)" onPaste="onPaste_txtLightNo();" runat="server" SkinId="textBoxSkin" width="85%"  MaxLength="3" CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>
                        <td align="right" width="12%">
                            <input type="button" id="btnAdd" onclick="if(clkButton())"  class="iMes_button" runat="server" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnAdd_ServerClick"/>
                            </td>           
                    </tr>
                    <tr>
                        <td width="8%" align="left">
                            <asp:Label ID="lbPartNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="25%" align="left">
                            <asp:TextBox ID="txtPartNo" runat="server" width="95%" SkinId="textBoxSkin"  MaxLength="50"  CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>
                        <td width="8%" align="left">
                            <asp:Label ID="lbDescr" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="25%" align="left" colspan="3">
                            <iMESMaintain:CmbLightDescrMaintain ID="ddlLightDescr" runat="server" Width="50%" AutoPostBack="false"></iMESMaintain:CmbLightDescrMaintain>

                        </td>
                        <td align="right" width="12%">
                            <input type="button" id="btnSave" onclick="if(clkButton())"  class="iMes_button" runat="server" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"/>
                            </td>           
                    </tr>                   
                    
                    
            </table> 
        </div>  
         <input type="hidden" id="itemId" runat="server" />
         <input type="hidden" id="HiddenUserName" runat="server" />
         <input type="hidden" id="dTableHeight" runat="server" />    
    </div>
   <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
       <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
           <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
       </table>
   </div>  
    <script language="javascript" type="text/javascript">

        //var familyObj;
        // var descriptionObj;

        var msg1 = "";
        var msg2 = "";
        var msg3 = "";
        var msg4 = "";
        var msg5 = "";
        var msg6 = "";
        var msg7 = "";
        var msg8 = "";
        var msg9 = "";
        var msg10 = "";
        var msg11 = "";
        var msg12 = "";
        var msg13 = "";
        window.onload = function() {
            //customerObj = getCustomerCmbObj();
            //customerObj.onchange = addNew;

            msg1 = "<%=pmtMessage1%>";
            msg2 = "<%=pmtMessage2%>";
            msg3 = "<%=pmtMessage3%>";
            msg4 = "<%=pmtMessage4%>";
            msg5 = "<%=pmtMessage5%>";
            msg6 = "<%=pmtMessage6%>";
            //ITC-1361-0094 itc210012  2012-02-17
            msg7 = "<%=pmtMessage7 %>";
            msg8 = "<%=pmtMessage8 %>";
            msg9 = "<%=pmtMessage9 %>";
            msg10 = "<%=pmtMessage10 %>";
            msg11 = "<%=pmtMessage11 %>";
            msg12 = "<%=pmtMessage12 %>";
            //ITC-1361-0141
            msg13 = "<%=pmtMessage13 %>";
            document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";
            resetTableHeight();
            ShowRowEditInfo(null);

        };

        //设置表格的高度  
        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 60;
            var marginValue = 12;
            var tableHeigth = 300;
            var a = document.body.parentElement.offsetHeight;
            try {
                var tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
            }
            catch (e) {
                //ignore
            }
            //为了使表格下面有写空隙
            var extDivHeight = tableHeigth + marginValue;

            document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
            div2.style.height = extDivHeight + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";

        }
        //设置高亮行
        var iSelectedRowIndex = null;
        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }

        //按键事件
        function OnKeyPress(obj) {

            var key = event.keyCode;
            if (key == 13)//enter
            {
                if (event.srcElement.id == "<%=LightNoList.ClientID %>") {
                    var value = document.getElementById("<%=LightNoList.ClientID %>").value.trim().toUpperCase();
                    if (value != "") {
                        findPakChnTwLightInfo(value, true);
                    }

                }
            }

        }

        //在列表里查找List输入项
        function findPakChnTwLightInfo(searchValue, isNeedPromptAlert) {
            var gdObj = document.getElementById("<%=gd.ClientID %>");
            searchValue = searchValue.toUpperCase();
            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (searchValue.trim() != "" && gdObj.rows[i].cells[0].innerText.toUpperCase().indexOf(searchValue) == 0) {
                    selectedRowIndex = i;
                    break;
                }
            }

            if (selectedRowIndex < 0) {
                if (isNeedPromptAlert == true) {
                    alert(msg7);
                    //alert("找不到列表中与Family栏位匹配的项");     
                }
                else if (isNeedPromptAlert == null) {
                    ShowRowEditInfo(null);
                }
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                ShowRowEditInfo(con);
            }
        }
        //所有button的前台检查
        function clkButton() {
            switch (event.srcElement.id) {
                case "<%=btnDelete.ClientID %>":
                    if (clkDelete() == false) {
                        return false;
                    }
                    break;
                case "<%=btnSave.ClientID %>":
                    if (clkSave() == false) {
                        return false;
                    }
                    break;
                case "<%=btnAdd.ClientID %>":
                    if (clkAdd() == false) {
                        return false;
                    }
                    break;
            }
            ShowWait();
            return true;
        }
        //delete的前台检查
        function clkDelete() {
            ShowInfo("");
            var gdObj = document.getElementById("<%=gd.ClientID %>")
            var curIndex = gdObj.index;
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            if (curIndex >= recordCount) {
                //alert("Please select one row!");   //2
                alert(msg1);
                return false;
            }

            //var ret = confirm("Do you really want to delete this item?");  //3
            var ret = confirm(msg2);  //3
            if (!ret) {
                return false;
            }
            return true;

        }
        //后台执行完删除操作后，前台显示
        function DeleteComplete() {
            ShowRowEditInfo(null);
        }
        //Add Button的前台检查
        function clkAdd() {
            return clkCheck();
        }

        //
        function clkSave() {
            return clkCheck();
        }
        
        function clkCheck(){
            var sModel = document.getElementById("<%=txtModel.ClientID %>").value.toString();
            var sPartNo = document.getElementById("<%=txtPartNo.ClientID %>").value.toString();
            var sType = document.getElementById("<%=txtType.ClientID %>").value.toString();
            var objDescr = getLightDescrCmbObj();
            var sLightNo = document.getElementById("<%=txtLightNo.ClientID %>").value.toString();
            if (sModel.trim() == "") {
                //alert("Please input [Model] first!!");  //4
                alert(msg3);
                document.getElementById("<%=txtModel.ClientID %>").focus();
                return false;
            }
            if (sPartNo.trim() == "") {
                //alert("Please input [Model] first!!");  //4
                alert(msg4);
                document.getElementById("<%=txtPartNo.ClientID %>").focus();
                return false;
            }
            if (sType.trim() == "") {
                //alert("Please input [Type] first!!");  
                alert(msg8);
                document.getElementById("<%=txtType.ClientID %>").focus();
                return false;
            }
            else {
                //DEBUG ITC1361-0143 add limit
                var getType = sType.trim();
                if  (getType.length >20) {
                    alert(msg12);
                    document.getElementById("<%=txtType.ClientID %>").focus();
                    return false;
                }
                
            }
            if (objDescr.value.trim() == "") {
                //alert("Please Select [Descr] first!!");  
                alert(msg9);
                objDescr.focus();
                return false;
            }
            if (sLightNo.trim() == "") {
                //alert("Please input [lightNo] first!!");  
                alert(msg10);
                document.getElementById("<%=txtLightNo.ClientID %>").focus();
                return false;
            }
            else {
                var valLightNo = parseInt(sLightNo.trim());
                if ((valLightNo <0) || (valLightNo >=1000)) {
                    alert(msg13);
                    document.getElementById("<%=txtLightNo.ClientID %>").focus();
                    return false;
                }
                
            }
            
            if (CheckInTable(sModel,sPartNo,sType,objDescr.value,sLightNo))
            {
                alert(msg11);
                document.getElementById("<%=txtModel.ClientID %>").focus();
                return false;
            }
            
            /*
            var modelTest = /^[a-zA-Z]([0-9]|[a-zA-Z])*$/g;
            if (!modelTest.test(sModel)) {
            alert(msg5);
            document.getElementById("<%=txtModel.ClientID %>").focus();
            return false;
            }
            
            var PartNoTest = /^[0-9]{6}\-[0-9]{3}$/g;
            if (!PartNoTest.test(sPartNo)) {
              
            alert(msg6);
            document.getElementById("<%=txtPartNo.ClientID %>").focus();
            return false;
            }
            */
            return true;
        }
        function input0To999Number(con) {
            //var pattern = /[1-9]\d{1,2}|\d/;
            var pattern = /^[0-9]{0,3}$/gi; 
            var conValue = con.value;
            var newValue = conValue + String.fromCharCode(event.keyCode);

            if (pattern.test(newValue)) {
                event.returnValue = true;
            }
            else {
                event.returnValue = false;
            }
        }
        function onPaste_txtLightNo() {
            event.returnValue = false;
        }
        //单击行事件
        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);
        }
        //点击高亮后，显示行信息
        function ShowRowEditInfo(con) {
            
            if (con == null) {
                document.getElementById("<%=txtModel.ClientID %>").value = "";
                document.getElementById("<%=txtPartNo.ClientID %>").value = "";
                document.getElementById("<%=txtType.ClientID %>").value = "";
                document.getElementById("<%=txtLightNo.ClientID %>").value = "";
                getLightDescrCmbObj().value = "";
                document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                return;
            }

            var sModel = con.cells[0].innerText.trim();
            var curDescr = con.cells[3].innerText.trim();
            document.getElementById("<%=txtModel.ClientID %>").value = sModel;
            document.getElementById("<%=txtPartNo.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=txtType.ClientID %>").value = con.cells[2].innerText.trim();
            getLightDescrCmbObj().value = curDescr;
            document.getElementById("<%=txtLightNo.ClientID %>").value = con.cells[4].innerText.trim();
            document.getElementById("<%=itemId.ClientID %>").value = con.cells[8].innerText.trim(); ;
            if (sModel == "") {
                document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            }
            else {
                document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
            }
        }

        //添加和更新操作完成后的前台设置
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
        }
        function CheckInTable(sModel,sPartNo,sType,sDescr,sLightNo) 
        {
           var tbl = "<%=gd.ClientID %>";
           var table = document.getElementById(tbl);
           var subFindFlag = false;
           for (var i = 1; i < table.rows.length; i++) 
            {
                if ((table.rows[i].cells[0].innerText.trim() == "") &&
                    (table.rows[i].cells[1].innerText.trim() == ""))
                    break;
                if ((table.rows[i].cells[0].innerText.trim() == sModel.trim()) &&
                    (table.rows[i].cells[1].innerText.trim() == sPartNo.trim()) && 
                    (table.rows[i].cells[2].innerText.trim() == sType.trim()) && 
                    (table.rows[i].cells[3].innerText.trim() == sDescr.trim()) &&
                    (parseInt(table.rows[i].cells[4].innerText.trim(), 10) == parseInt(sLightNo.trim(), 10)))
                {
                    subFindFlag = true;
                    break;
                }
            }
           return  subFindFlag;
        }
      
   
    </script>
</asp:Content>


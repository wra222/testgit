<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ICASA.aspx.cs" Inherits="DataMaintain_ICASA" ValidateRequest="false" %>
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
                        <asp:Label ID="lbICASAList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="42%">  
                        <asp:TextBox ID="ICASAList" runat="server"   MaxLength="12"  Width="50%" CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>
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
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="130%" RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
                    
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                    <tr>
                        <td width="5%" align="left">
                            <asp:Label ID="lbVC" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="18%" align="left">
                            <asp:TextBox ID="vc" runat="server"  width="80%"  MaxLength="5" onkeypress="VCInputCheck(this)" CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>
                        <td width="8%" align="left">
                            <asp:Label ID="lbAntel1" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="25%" align="left">
                            <asp:TextBox ID="antel1" runat="server" SkinId="textBoxSkin" width="89%"  MaxLength="25" CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>
                        <td width="8%" align="left">
                            <asp:Label ID="lbIcasa" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="25%" align="left">
                            <asp:TextBox ID="icasa" runat="server" SkinId="textBoxSkin" width="85%"  MaxLength="25" CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>
                        <td align="right" width="12%">
                            <input type="button" id="btnAdd" onclick="if(clkButton())"  class="iMes_button" runat="server" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnAdd_ServerClick"/>
                            </td>           
                    </tr>
                    <tr>
                        <td width="5%" align="left">
                            <asp:Label ID="lbAV" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="18%" align="left">
                            <asp:TextBox ID="av" runat="server" width="80%" SkinId="textBoxSkin"  MaxLength="12" onkeypress="AVInputCheck(this)" CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>
                        <td width="8%" align="left">
                            <asp:Label ID="lbAntel2" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="25%" align="left" colspan="3">
                            <asp:TextBox ID="antel2" runat="server" width="38%" SkinId="textBoxSkin" MaxLength="25" CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
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
        var msg7="";

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
            msg7="<%=pmtMessage7 %>";
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
                if (event.srcElement.id == "<%=ICASAList.ClientID %>") {
                    var value = document.getElementById("<%=ICASAList.ClientID %>").value.trim().toUpperCase();
                    if (value != "") {
                        findICASAInfo(value, true);
                    }

                }
            }

        }

        //在列表里查找List输入项
        function findICASAInfo(searchValue, isNeedPromptAlert) {
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
                    //                alert("找不到列表中与Family栏位匹配的项");     
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
                //            alert("Please select one row!");   //2
                alert(msg1);
                return false;
            }

            //         var ret = confirm("Do you really want to delete this item?");  //3
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
            var vc = document.getElementById("<%=vc.ClientID %>").value.toString();
            var av = document.getElementById("<%=av.ClientID %>").value.toString();
            if (vc.trim() == "") {
                //           alert("Please input [VC] first!!");  //4
                alert(msg3);
                document.getElementById("<%=vc.ClientID %>").focus();
                return false;
            }
            if (av.trim() == "") {
                //           alert("Please input [VC] first!!");  //4
                alert(msg4);
                document.getElementById("<%=av.ClientID %>").focus();
                return false;
            }
            var vcTest = /^[a-zA-Z]([0-9]|[a-zA-Z])*$/g;
            if (!vcTest.test(vc)) {
                alert(msg5);
                document.getElementById("<%=vc.ClientID %>").focus();
                return false;
            }
            /*
            var avTest = /^[0-9]{6}\-[0-9]{3}$/g;
            if (!avTest.test(av)) {
              
                alert(msg6);
                document.getElementById("<%=av.ClientID %>").focus();
                return false;
            }
            */
            return true;
        }
        
        function VCInputCheck() {
            //VC的输入限制，只能输入数字和字母
            var key = event.keyCode;
            //itc-1361-0121
            if (!((key >= 48 && key <= 57) || (key >= 97 && key <= 122)|| (key >= 65 && key <= 90))) {
                event.keyCode = 0;
            }
        }
        
        //AV框中只能输入数字和下划线
       //ITC-1361-0103  itc21001  2012-02-27
        function AVInputCheck() {
            //AV框中只能输入数字和下划线
//            var key = event.keyCode;
//            if (!((key >= 48 && key <= 57) || key == 45))
//            {
//                event.keyCode = 0;
//            }
        }
        
        //单击行事件
        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);

        }
        //点击高亮后，显示行信息
        function ShowRowEditInfo(con) {

            if (con == null) {
                document.getElementById("<%=vc.ClientID %>").value = "";
                document.getElementById("<%=av.ClientID %>").value = "";
                document.getElementById("<%=antel1.ClientID %>").value = "";
                document.getElementById("<%=antel2.ClientID %>").value = "";
                document.getElementById("<%=icasa.ClientID %>").value = "";
                document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                return;
            }

            var vc = con.cells[0].innerText.trim();
            document.getElementById("<%=vc.ClientID %>").value = vc;
            document.getElementById("<%=av.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=antel1.ClientID %>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=antel2.ClientID %>").value = con.cells[3].innerText.trim();
            document.getElementById("<%=icasa.ClientID %>").value = con.cells[4].innerText.trim();
            document.getElementById("<%=itemId.ClientID %>").value = con.cells[8].innerText.trim(); ;
            if (vc == "") {
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
   
    </script>
</asp:Content>


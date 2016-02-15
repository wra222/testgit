<%--
/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for APT maintain Page
 *             
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-18  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ActualProductionTime.aspx.cs" Inherits="DataMaintain_APT" Title="APT Maintain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<style type="text/css">

.iMes_div_MainTainEdit
{
    border: thin solid Black; 
    background-color: #99CDFF;
    margin:0 0 20 0;
    
}

</style>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/calendar/calendar.js"></script>
    <script type="text/VBscript" src="../CommonControl/calendar/calendar.vbs"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div align="center">
            <asp:Label ID="lblPageTitle" runat="server" CssClass="iMes_label_13pt" Font-Bold="true" Font-Size="Larger"></asp:Label>
        </div>
        <!--Date & Line input fields. -->
        <div id="div4">        
            <table width="100%" class="iMes_div_MainTainEdit" >
                <tr>
                    <td style="width:80px;">
                        <asp:Label ID="lblDate" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:180px;">
                        <input type="text" id="txtDate" readonly="readonly" onfocus="CalDisappear();" onpropertychange="changeDate()" />
	                    <button type="button" id="btnDate" onclick="showCalFrame(txtDate)">...</button>
                    </td>
                    <td style="width:50px;"></td>
                    <td style="width:60px;">
                        <asp:Label ID="lblLine1" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:100px;">
                        <asp:TextBox ID="txtLine" SkinId="textBoxSkin" runat="server" MaxLength="10" Width="100%" style="text-transform:uppercase;" onpropertychange="changeCondition()"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>             
            </table> 
        </div>  
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label>                        
                    </td> 
                    <td>                       
                    </td>                                   
                    <td width="32%" align="right">
                        <input type="button" id="btnDel" runat="server" class="iMes_button"  onclick="if(clkDelete())" onserverclick="btnDelete_ServerClick"/>                         
                    </td>    
                </tr>
             </table>                                                    
        </div>

        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
            <Triggers> 
                <asp:AsyncPostBackTrigger ControlID="btnDel" EventName="ServerClick" />            
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div2">
            <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <ContentTemplate>
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                        Width="100%" RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="350px" AutoHighlightScrollByValue ="true" 
                        HighLightRowPosition="3"  
                        OnGvExtRowClick='selectRow(this)' 
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false" style="top: -350px; left: 24px"  
                        >
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>   
        </div>
        
        <div id="div3">        
            <table width="100%" class="iMes_div_MainTainEdit" >
                <tr>
                    <td width="12%">
                        <asp:Label ID="lblLine2" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2">
                        <select id="cmbLine" style="width:98%" onchange="changeLine()">
                            <option value=""></option>
                        </select>
                    </td>
                    <td width="3%"></td>
                    <td width="13%">
                        <asp:Label ID="lblAPT" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="10%">
                        <input ID="txtAPT" style="width:100%" SkinId="textBoxSkin" MaxLength="4" onBlur="txtAPT_Changed()"></input>
                    </td>
                    <td width="4%">
                        <asp:Label ID="lblHr" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="3%"></td>
                    <td width="12%">
                        <asp:Label ID="lblCause" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2">
                        <select id="cmbCause" style="width:98%">
                            <!--<option value=""></option>-->
                        </select>
                    </td>
                    <td width="3%"></td>
                    <td width="12%">
                        <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr>                   
                    <td>
                        <asp:Label ID="lblOLT1" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td width="10%">
                        <input ID="txtOLT1" style="width:100%" SkinId="textBoxSkin" MaxLength="4" onBlur="txtOLT1_Changed()"></input>
                    </td>                        
                    <td width="4%">
                        <asp:Label ID="lblHr1" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>   
                    <td></td>            
                    <td>
                        <asp:Label ID="lblOLT2" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td>
                        <input ID="txtOLT2" style="width:100%" SkinId="textBoxSkin" MaxLength="4" onBlur="txtOLT2_Changed()"></input>
                    </td>                        
                    <td>
                        <asp:Label ID="lblHr2" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td></td>                              
                    <td>
                        <asp:Label ID="lblOLT3" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td width="10%">
                        <input ID="txtOLT3" style="width:100%" SkinId="textBoxSkin" MaxLength="4" onBlur="txtOLT3_Changed()"></input>
                    </td>                        
                    <td width="4%">
                        <asp:Label ID="lblHr3" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>  
                    <td></td>                              
                    <td>
                        <input ID="txtRemark" SkinId="textBoxSkin" Width="95%" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>        
                <tr>                   
                    <td>
                        <asp:Label ID="lblCause1" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td colspan="2">
                        <select id="cmbCause1" style="width:98%">
                            <option value=""></option>
                        </select>
                    </td>    
                    <td></td>                          
                    <td>
                        <asp:Label ID="lblCause2" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td colspan="2">
                        <select id="cmbCause2" style="width:98%">
                            <option value=""></option>
                        </select>
                    </td>    
                    <td></td>                                     
                    <td>
                        <asp:Label ID="lblCause3" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td colspan="2">
                        <select id="cmbCause3" style="width:98%">
                            <option value=""></option>
                        </select>
                    </td>   
                    <td></td>            
                    <td align="center">
                        <input type="button" id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"/>
                    </td>           
                </tr>                
            </table> 
        </div>  
        
        <input type="hidden" id="hidSelectedId" runat="server" />         
        <input type="hidden" id="hidTableHeight" runat="server" />
        <input type="hidden" id="hidDate" runat="server" />
        <input type="hidden" id="hidContent" runat="server" />
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <button id="btnProcess" runat="server" type="button" onclick="" style="display: none" />
            </ContentTemplate>
        </asp:UpdatePanel>
   
    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr>
                <td align="center">
                    <img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/>
                </td>
                <td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">
                    Please wait.....
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        var selectedRowIndex = -1;
        var SUCCESSRET = "<%=SUCCESSRET%>";
        var objLine = document.getElementById("cmbLine");
        var objCause = document.getElementById("cmbCause");
        var objCause1 = document.getElementById("cmbCause1");
        var objCause2 = document.getElementById("cmbCause2");
        var objCause3 = document.getElementById("cmbCause3");
        var objAPT = document.getElementById("txtAPT");
        var objOLT1 = document.getElementById("txtOLT1");
        var objOLT2 = document.getElementById("txtOLT2");
        var objOLT3 = document.getElementById("txtOLT3");
        var objRemark = document.getElementById("txtRemark");
        var objHidContent = document.getElementById("<%=hidContent.ClientID %>");
        var msgWrongTime = '<%=this.GetLocalResourceObject(Pre + "_msgWrongTime").ToString()%>';
        var msgExceedTimeAutoAdjust = '<%=this.GetLocalResourceObject(Pre + "_msgExceedTimeAutoAdjust").ToString()%>';
        var msgExceedTimeNeedCheck = '<%=this.GetLocalResourceObject(Pre + "_msgExceedTimeNeedCheck").ToString()%>';
        var pattTime1 = /^[0-9]+$/;
        var pattTime2 = /^[0-9]+\.[0-9]$/;
        var FACTOR = 10;    //Used in converting number with a decimal place to integer, JS issue.
        var obTime = 0;

        window.onload = function() {
            showEmptyContent();
            resetTableHeight();
            PageMethods.GetPageInfo(OnGetPageInfoSuccess, OnGetPageInfoFail);
        };
    initCalFrame("../CommonControl/calendar/");

    function OnGetPageInfoSuccess(result)
    {
        try {
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 4) && (result[0] == SUCCESSRET)) {
                var deptList = result[1];
                var lineList = result[2];
                for (var i = 0; i < deptList.length; i++) {
                    for (var j = 0; j < lineList.length; j++) {
                        if (deptList[i].line == lineList[j].Line) {
                            objLine.options.add(new Option(deptList[i].line + " " + deptList[i].remark, lineList[j].ObjectiveTime + "+" + deptList[i].line));
                            break;
                        }
                    }
                }
                
                var causeList = result[3];
                for (var i = 0; i < causeList.length; i++) {
                    if (causeList[i].name.trim() == "001") {
                        objCause.options.add(new Option(causeList[i].name + " : " + causeList[i].value, causeList[i].name));
                    }
                    else {
                        objCause1.options.add(new Option(causeList[i].name + " : " + causeList[i].value, causeList[i].name));
                        objCause2.options.add(new Option(causeList[i].name + " : " + causeList[i].value, causeList[i].name));
                        objCause3.options.add(new Option(causeList[i].name + " : " + causeList[i].value, causeList[i].name));
                    }
                }
            }
            else {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
            }

        } catch (e) {
            alert(e.description);
        }
    }

    function OnGetPageInfoFail(error)
    {
        try {
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());
        } catch (e) {
            alert(e.description);
        }
    }
    
    function resetTableHeight()
    {
        //动态调整表格的高度
        var adjustValue=70;     
        var marginValue=10;  
        var tableHeigth=300;
        
        try{
            tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
        }
        catch(e){
            //ignore
        }                
        //为了使表格下面有写空隙
        var extDivHeight=tableHeigth+marginValue;
       
        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
        //alert(document.getElementById("div_<%=gd.ClientID %>").style.height)
        
        div2.style.height=extDivHeight+"px";
        document.getElementById("<%=hidTableHeight.ClientID %>").value=tableHeigth+"px";
    }

    function changeCondition() {
        gvTable = document.getElementById("<%=gd.ClientID%>");
        condStr = document.getElementById("<%=txtLine.ClientID %>").value;
        if (condStr == "") {
            return;
        }

        for (k = 1; k <= parseInt(document.getElementById("<%=hidRecordCount.ClientID %>").value); k++) {
            if (gvTable.rows[k].cells[0].innerText.trim().toUpperCase().indexOf(condStr.toUpperCase()) == 0) {
                selectRow(gvTable.rows[k]);
                return;
            }
        }    
    }

    function changeLine() {
        gvTable = document.getElementById("<%=gd.ClientID%>");
        condStr = objLine.value.substring(objLine.value.indexOf("+") + 1);
        if (condStr != "") {
            for (k = 1; k <= parseInt(document.getElementById("<%=hidRecordCount.ClientID %>").value); k++) {
                if (gvTable.rows[k].cells[0].innerText.trim() == condStr) {
                    selectRow(gvTable.rows[k]);
                    return;
                }
            }
        }
        objAPT.value = "";
        objOLT1.value = "";
        objOLT2.value = "";
        objOLT3.value = "";
        objCause1.selectedIndex = 0;
        objCause2.selectedIndex = 0;
        objCause3.selectedIndex = 0;
        objRemark.value = "";
        lineVal = objLine.value;
        if (lineVal == "") {
            obTime = 0;
        }
        else {
            obTime = parseFloat(lineVal.substring(0, lineVal.indexOf("+")));
        }
        objAPT.value = obTime.toString();
    }

    function changeDate() {
        if (document.getElementById("<%=hidDate.ClientID%>").value == document.getElementById("txtDate").value) {
            return;
        }
        document.getElementById("<%=hidDate.ClientID%>").value = document.getElementById("txtDate").value;
        showEmptyContent();
        selectedRowIndex = -1;
        document.getElementById("<%=btnProcess.ClientID%>").click();
    }

    function txtAPT_Changed() {
        strApt = objAPT.value;
        if (strApt == "") {
            return;
        }

        //输入合法性判断：整数或一位小数
        if (!pattTime1.exec(strApt) && !pattTime2.exec(strApt)) {
            alert(msgWrongTime);
            objAPT.select();
            return;
        }

        //修正显示值为正确格式（去除前导0）
        valApt = parseFloat(strApt);
        objAPT.value = valApt.toString();

        //判断是否超过最大值
        valOLT1 = (objOLT1.value == "" ? 0 : parseFloat(objOLT1.value));
        valOLT2 = (objOLT2.value == "" ? 0 : parseFloat(objOLT2.value));
        valOLT3 = (objOLT3.value == "" ? 0 : parseFloat(objOLT3.value));
        if (obTime != 0 && valApt + valOLT1 + valOLT2 + valOLT3 > obTime) {
            alert(msgExceedTimeNeedCheck);
            objAPT.value = 0;
            objAPT.select();
            return;
        }

        //需要自动填充
        if (obTime != 0 && objOLT1.value == "" && objOLT2.value == "" && objOLT3.value == "") {
            //清空未开线时间1/2/3信息和Remark信息
            objOLT1.value = "";
            objCause1.selectedIndex = 0;
            objOLT2.value = "";
            objCause2.selectedIndex = 0;
            objOLT3.value = "";
            objCause3.selectedIndex = 0;
            objRemark.value = "";

            //剩余时间填入未开线时间1
            if (valApt < obTime) {
                objOLT1.value = (obTime * FACTOR - valApt * FACTOR) / FACTOR;
            }
        }
        
        return;
    }

    function txtOLT1_Changed() {
        strOLT1 = objOLT1.value;
        if (strOLT1 == "") {
            return;
        }        

        //输入合法性判断：整数或一位小数
        if (!pattTime1.exec(strOLT1) && !pattTime2.exec(strOLT1)) {
            alert(msgWrongTime);
            objOLT1.select();
            return;
        }

        //修正显示值为正确格式（去除前导0）
        valOLT1 = parseFloat(strOLT1);
        objOLT1.value = valOLT1.toString();

        //判断是否超过最大值
        valApt = (objAPT.value == "" ? 0 : parseFloat(objAPT.value));
        valOLT2 = (objOLT2.value == "" ? 0 : parseFloat(objOLT2.value));
        valOLT3 = (objOLT3.value == "" ? 0 : parseFloat(objOLT3.value));
        if (obTime != 0 && valApt + valOLT1 + valOLT2 + valOLT3 > obTime) {
            alert(msgExceedTimeNeedCheck);
            objOLT1.value = 0;
            objOLT1.select();
            return;
        }

        //需要自动填充
        if (obTime != 0 && objAPT.value != "" && objOLT2.value == "" && objOLT3.value == "") {
            //清空未开线时间2/3信息
            objOLT2.value = "";
            objCause2.selectedIndex = 0;
            objOLT3.value = "";
            objCause3.selectedIndex = 0;

            //剩余时间填入未开线时间2
            if (valApt + valOLT1 < obTime) {
                objOLT2.value = (obTime * FACTOR - valApt * FACTOR - valOLT1 * FACTOR) / FACTOR;
            }
        }

        return;
    }

    function txtOLT2_Changed() {
        strOLT2 = objOLT2.value;
        if (strOLT2 == "") {
            return;
        }

        //输入合法性判断：整数或一位小数
        if (!pattTime1.exec(strOLT2) && !pattTime2.exec(strOLT2)) {
            alert(msgWrongTime);
            objOLT2.select();
            return;
        }

        //修正显示值为正确格式（去除前导0）
        valOLT2 = parseFloat(strOLT2);
        objOLT2.value = valOLT2.toString();

        //判断是否超过最大值
        valApt = (objAPT.value == "" ? 0 : parseFloat(objAPT.value));
        valOLT1 = (objOLT1.value == "" ? 0 : parseFloat(objOLT1.value));
        valOLT3 = (objOLT3.value == "" ? 0 : parseFloat(objOLT3.value));
        if (obTime != 0 && valApt + valOLT1 + valOLT2 + valOLT3 > obTime) {
            alert(msgExceedTimeNeedCheck);
            objOLT2.value = 0;
            objOLT2.select();
            return;
        }

        //是否需要自动填充
        if (obTime != 0 && objAPT.value != "" && objOLT1.value != "" && objOLT3.value == "") {
            //清空未开线时间3信息
            objOLT3.value = "";
            objCause3.selectedIndex = 0;

            //剩余时间填入未开线时间3
            if (valApt + valOLT1 + valOLT2 < obTime) {
                objOLT3.value = (obTime * FACTOR - valApt * FACTOR - valOLT1 * FACTOR - valOLT2 * FACTOR) / FACTOR;
            }
        }
        
        return;
    }

    function txtOLT3_Changed() {
        strOLT3 = objOLT3.value;
        if (strOLT3 == "") {
            return;
        }

        //输入合法性判断：整数或一位小数
        if (!pattTime1.exec(strOLT3) && !pattTime2.exec(strOLT3)) {
            alert(msgWrongTime);
            objOLT3.select();
            return;
        }

        //修正显示值为正确格式（去除前导0）
        valOLT3 = parseFloat(strOLT3);
        objOLT3.value = valOLT3.toString();

        //判断是否超过最大值
        valApt = (objAPT.value == "" ? 0 : parseFloat(objAPT.value));
        valOLT1 = (objOLT1.value == "" ? 0 : parseFloat(objOLT1.value));
        valOLT2 = (objOLT2.value == "" ? 0 : parseFloat(objOLT2.value));
        if (obTime != 0 && valApt + valOLT1 + valOLT2 + valOLT3 > obTime) {
            alert(msgExceedTimeNeedCheck);
            objOLT3.value = 0;
            objOLT3.select();
            return;
        }

        return;
    }
    
    function clkDelete()
    {
        ShowInfo("");
        var gdObj = document.getElementById("<%=gd.ClientID %>");
        var curIndex = gdObj.index;
        var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;            
        if(curIndex >= recordCount)
        {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgNoSelectRecord").ToString()%>');
            return false;
        }

        if (!confirm('<%=this.GetLocalResourceObject(Pre + "_msgConfirmDelete").ToString()%>')) {
            return false;
        }

        selectedRowIndex = -1;
        ShowWait();
        return true;        
    }
   
    function clkSave() {
        ShowInfo("");
        if (checkInputValues()) {
            ShowWait();
            return true;
        }
        else {
            return false;
        }
    }

    function checkInputValues() {
        if (document.getElementById("<%=hidDate.ClientID %>").value == "") {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgNoDate").ToString()%>');
            document.getElementById('btnDate').click();
            return false;
        }
        
        if (objLine.value == "") {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgNoLine").ToString()%>');
            objLine.focus();
            return false;
        }

        if (objAPT.value == "" || (!pattTime1.exec(objAPT.value) && !pattTime2.exec(objAPT.value)) || parseFloat(objAPT.value) == 0) {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgNoAPT").ToString()%>');
            objAPT.select();
            return false;
        }

        if (objCause.value == "") {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgNoCause").ToString()%>');
            objCause.focus();
            return false;
        }

        if (objOLT1.value != "" && !pattTime1.exec(objOLT1.value) && !pattTime2.exec(objOLT1.value)) {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgBadOFT").ToString()%>');
            objOLT1.select();
            return false;
        }

        if (objOLT2.value != "" && !pattTime1.exec(objOLT2.value) && !pattTime2.exec(objOLT2.value)) {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgBadOFT").ToString()%>');
            objOLT2.select();
            return false;
        }

        if (objOLT3.value != "" && !pattTime1.exec(objOLT3.value) && !pattTime2.exec(objOLT3.value)) {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgBadOFT").ToString()%>');
            objOLT3.select();
            return false;
        }

        valAPT = parseFloat(objAPT.value);
        valOLT1 = (objOLT1.value == "" ? 0 : parseFloat(objOLT1.value));
        valOLT2 = (objOLT2.value == "" ? 0 : parseFloat(objOLT2.value));
        valOLT3 = (objOLT3.value == "" ? 0 : parseFloat(objOLT3.value));

        if (valOLT1 != 0 && objCause1.value == "") {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgNoOfflineCause").ToString()%>');
            objCause1.focus();
            return false;
        }

        if (valOLT2 != 0 && objCause2.value == "") {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgNoOfflineCause").ToString()%>');
            objCause2.focus();
            return false;
        }

        if (valOLT3 != 0 && objCause3.value == "") {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgNoOfflineCause").ToString()%>');
            objCause3.focus();
            return false;
        }

        if (valAPT + valOLT1 + valOLT2 + valOLT3 > obTime) {
            alert('<%=this.GetLocalResourceObject(Pre + "_msgExceedTimeNeedCheck").ToString()%>');
            objAPT.select();
            return false;
        }

        if ((valOLT1 == 0 && objCause1.value != "") || (valOLT2 == 0 && objCause2.value != "") || (valOLT3 == 0 && objCause3.value != "")) {
            if (!confirm('<%=this.GetLocalResourceObject(Pre + "_msgOmitOfflineCause").ToString()%>')) {
                return false;
            }
        }
        
        objHidContent.value = objLine.value.substring(objLine.value.indexOf("+") + 1) + "\u0003"
                            + valAPT + "\u0003"
                            + objCause.value + "\u0003"
                            + valOLT1 + "\u0003"
                            + (valOLT1 == 0 ? "" : objCause1.value) + "\u0003"
                            + valOLT2 + "\u0003"
                            + (valOLT2 == 0 ? "" : objCause2.value) + "\u0003"
                            + valOLT3 + "\u0003"
                            + (valOLT3 == 0 ? "" : objCause3.value) + "\u0003"
                            + objRemark.value;
        return true;
    }

    function selectRow(row) {
        ShowInfo("");
        if (selectedRowIndex == row.index) {
            return;
        }
        setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, "<%=gd.ClientID %>");
        selectedRowIndex = row.index;
        setRowSelectedOrNotSelectedByIndex(selectedRowIndex, true, "<%=gd.ClientID %>");

        if (parseInt(selectedRowIndex) < parseInt(document.getElementById("<%=hidRecordCount.ClientID %>").value)) {
            showRowContent(row);
        }
        else
        {
            showEmptyContent();
        }
    }

    function setLine(val) {
        for (var i = 0; i < objLine.options.length; i++) {
            opVal = objLine.options[i].value;
            if (opVal.substring(opVal.indexOf("+") + 1) == val) {
                objLine.selectedIndex = i;
                return;
            }
        }
        objLine.selectedIndex = 0;
    }

    function setCause(obj, val) {
        if (val == "----") {
            obj.selectedIndex = 0;
            return;
        }
        
        for (var i = 0; i < obj.options.length; i++) {
            opVal = obj.options[i].value;
            if (opVal == val || val.indexOf(opVal + ":") == 0) {
                obj.selectedIndex = i;
                return;
            }
        }
        obj.selectedIndex = 0;
    }

    function showRowContent(row) {
        document.getElementById("<%=hidSelectedId.ClientID %>").value = row.cells[0].innerText.trim();
        setLine(row.cells[0].innerText.trim());
        objAPT.value = parseFloat(row.cells[2].innerText.trim());
        objOLT1.value = parseFloat(row.cells[4].innerText.trim());
        objOLT2.value = parseFloat(row.cells[6].innerText.trim());
        objOLT3.value = parseFloat(row.cells[8].innerText.trim());
        //objCause1.value = row.cells[5].innerText.trim();
        //objCause2.value = row.cells[7].innerText.trim();
        //objCause3.value = row.cells[9].innerText.trim();
        objRemark.value = row.cells[10].innerText.trim();
        setCause(objCause1, row.cells[5].innerText.trim());
        setCause(objCause2, row.cells[7].innerText.trim());
        setCause(objCause3, row.cells[9].innerText.trim());
        document.getElementById("<%=btnDel.ClientID %>").disabled = false;
        obTime = parseFloat(objLine.value.substring(0, objLine.value.indexOf("+"))); 
    }

    function showEmptyContent() {
        objLine.selectedIndex = 0;
        objAPT.value = "";
        objOLT1.value = "";
        objOLT2.value = "";
        objOLT3.value = "";
        objCause1.selectedIndex = 0;
        objCause2.selectedIndex = 0;
        objCause3.selectedIndex = 0;
        objRemark.value = "";
        document.getElementById("<%=btnDel.ClientID %>").disabled = true;
        obTime = 0;
    }   
   
    function AddUpdateComplete(id)
    {        
        var gdObj = document.getElementById("<%=gd.ClientID %>");

        for (var i = 0; i < document.getElementById("<%=hidRecordCount.ClientID %>").value; i++) 
        {
           if(gdObj.rows[i + 1].cells[0].innerText == id)
           {
               selectedRowIndex = i;
               break;
           }
       }

       if (selectedRowIndex < document.getElementById("<%=hidRecordCount.ClientID %>").value) {
           setSrollByIndex(selectedRowIndex, true, "<%=gd.ClientID%>");
           showRowContent(gdObj.rows[selectedRowIndex + 1]);
       }
       else {
           showEmptyContent();
       }
    }
    </script>
</asp:Content>


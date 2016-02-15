<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="HoldRule.aspx.cs" Inherits="DataMaintain_HoldRule" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
        <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
	<script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>
	

    
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%"class="iMes_div_MainTainEdit">
                <tr >
                    <td style="width:10%">
                        <asp:Label ID="lblLineTop" runat="server" CssClass="iMes_label_13pt">Line:</asp:Label>
                    </td>
                    <td style="width:20%">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel6" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbLineTop" runat="server" Width="98%"></asp:DropDownList>            
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td >
                    <td style="width:10%">
                        <asp:Label ID="lblFamilyTop" runat="server" CssClass="iMes_label_13pt">Family:</asp:Label>
                    </td>
                    <td style="width:20%">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Always" >
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbFamilyTop" runat="server" Width="98%"></asp:DropDownList>            
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width:10%">
                        <asp:Label ID="lblModelTop" runat="server" CssClass="iMes_label_13pt">Model:</asp:Label>
                    </td>
                    <td style="width:20%">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Always" >
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbModelTop" runat="server" Width="98%"></asp:DropDownList>    
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td >
                    <td align="right">
                       <input type="button" id="btnQuery" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkQuery())" onserverclick="Query_ServerClick"/>
                    </td> 
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCUSTSNTop" runat="server" CssClass="iMes_label_13pt">CUSTSN:</asp:Label>
                    </td>
                    <td>
                        <%--<input type="text" id="txtCUSTSNTop" runat="server" Width="98%" />--%>
                        <asp:TextBox ID="txtCUSTSNTop" runat="server" Width="90%" MaxLength="30"></asp:TextBox>
                        
                    </td >
                    <td>
                     <input id="Button1" type="button" value="Browse" onclick="UploadSNListQuery()" />
                      
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr> 
            </table>
        </div>   
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblMasterLabelList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                                    
                    <td width="32%" align="right">
                        <input type="button" id="btnDelete" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="false" onclick="if(clkDelete())" onserverclick="Delete_ServerClick"/>
                        <input type='checkbox' id='chxDNALL' onclick='checkAll(this)'  
                            name='checkboxALL' > All
                       
                    </td>    
                </tr>
            </table>                                     
        </div>
        <div id="div2">
            <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                </Triggers>
                <ContentTemplate>
                 <imes:gridviewext ID="gd" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="True"
                                    GvExtWidth="100%" GvExtHeight="320px" 
                                    Width="99.9%" Height="300px" SetTemplateValueEnable="True" GetTemplateValueEnable="True"
                                      onrowdatabound="gd_RowDataBound" 
                                    
                                    OnDataBound="gd_DataBound"
                                    HighLightRowPosition="3" HorizontalAlign="Left" 
                        HiddenColCount="0" >
                                    <Columns>
                                       
                                        <asp:BoundField DataField="ID"  />
                                        <asp:BoundField DataField="Line" />
                                        <asp:BoundField DataField="Family" />
                                        <asp:BoundField DataField="Model" />
                                        <asp:BoundField DataField="CUSTSN" />
                                        <asp:BoundField DataField="CheckINStation" />
                                        <asp:BoundField DataField="HoldStation" />
                                        <asp:BoundField DataField="HoldCode" />
                                        <asp:BoundField DataField="HoldDescr" />
                                          <asp:BoundField DataField="Editor" />   
                                          <asp:BoundField DataField="Cdt" />
                                          <asp:BoundField DataField="Udt" />    
                                           <asp:TemplateField HeaderText="">

                                            <ItemTemplate>
                                                <asp:CheckBox id="chk" runat="server"  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </imes:gridviewext>
                                
                   <%--             
                    <iMES:GridViewExt ID="gd" runat="server" 
                        HighLightRowPosition="3" Width="100%" GvExtWidth="100%" GvExtHeight="390px"  
                        onrowdatabound="gd_RowDataBound" 
                        OnGvExtRowClick="clickTable(this)" 
                        OnDataBound="gd_DataBound" 
                        RowStyle-Height="20" 
                        AutoHighlightScrollByValue ="True" GetTemplateValueEnable="False" 
                        HiddenColCount="0" SetTemplateValueEnable="False">
                           <Columns>
                                     <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:CheckBox id="chk" runat="server" />
                                                        </ItemTemplate>
                                       </asp:TemplateField>
                                      <asp:BoundField DataField="ID" />
                                        <asp:BoundField DataField="Line" />
                                        <asp:BoundField DataField="Family" />
                                        <asp:BoundField DataField="Model" />
                                        <asp:BoundField DataField="CUSTSN" />
                                        <asp:BoundField DataField="CheckINStation" />
                                        <asp:BoundField DataField="HoldStation" />
                                        <asp:BoundField DataField="HoldCode" />
                                        <asp:BoundField DataField="HoldDescr" />
                                          <asp:BoundField DataField="Editor" />   
                                          <asp:BoundField DataField="Cdt" />
                                          <asp:BoundField DataField="Udt" />    
                            </Columns>
                    </iMES:GridViewExt>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
            <table width="100%" border="0"  class="iMes_div_MainTainEdit">
                <tr>
                    <td>
                        <asp:Label ID="lblCUSTSN" runat="server" CssClass="iMes_label_13pt">CUSTSN:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCUSTSN" runat="server" Width="75%" MaxLength="30"></asp:TextBox>
                        <input id="BtnBrowse" type="button" value="Browse"  onclick="UploadCUSTSNList()" />               
                            <asp:HiddenField ID="hfCUSTSN" runat="server" />
                    </td>
                    
                    <td style="width:10%"></td>
                    <td style="width:30%"></td>
                    <td style="width:20%"></td>
                </tr>
                <tr>
                    <td style="width:10%">
                        <asp:Label ID="lblLine" runat="server" CssClass="iMes_label_13pt">Line:</asp:Label>
                    </td>
                    <td style="width:30%">
                        <asp:DropDownList ID="cmbLine" runat="server" Width="98%" ></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt">Model:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtModel" runat="server" Width="96%" ></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt">Family:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbFamily" runat="server" Width="98%"></asp:DropDownList>           
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCheckINStation" runat="server" CssClass="iMes_label_13pt">CheckINStation:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbCheckINStation" runat="server" Width="98%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblHoldCode" runat="server" CssClass="iMes_label_13pt">HoldCode:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbHoldCode" runat="server" Width="98%"></asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblHoldStation" runat="server" CssClass="iMes_label_13pt">HoldStation:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbHoldStation" runat="server" Width="98%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblHoldDescr" runat="server" CssClass="iMes_label_13pt">HoldDescr:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtHoldDescr" runat="server" Width="96%" MaxLength="255"></asp:TextBox>
                    </td>
                    <td align="right">
                        <input type="button" id="btnSave" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkSave())" onserverclick="Save_ServerClick"/>
                    </td>
                </tr>  
            </table>
        </div>
    </div>    
    <input type="hidden" id="hidFamily" runat="server" />
    <input type="hidden" id="hidDeleteID" runat="server" />
    <input type="hidden" id="dTableHeight" runat="server" />
    <input type="hidden" id="HiddenUserName" runat="server" />
    <input type="hidden" id="hidModelList" runat="server" />
     <asp:HiddenField ID="HiddenSNQuery" runat="server" />
     <input id="hidSelectId" runat="server" type="hidden" />
    
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
        
        var vcPattern=/^[0-9A-Z]{1,}$/;
        var codePattern=/^[0-9A-Z]+$/;
        
        var emptyString = "";
        var selectedRowIndex = -1;
        var msgConfirmDelete;
  //      var msgConfirmDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgConfirmDelete").ToString() %>';
        var msgConfirmDelete="";
        var noEmptyVc="";
        var noEmptyCode="";
        var msgInputFamily;
        
        var vcFormat;
        var codeFormat;
        var tblDN;
        var tblDNObj;
        var ctlSelectedId = document.getElementById("<%=hidDeleteID.ClientID%>");
        window.onload = function() {
            msgConfirmDelete = "<%=pmtMessage1 %>";
            noEmptyVc = "<%=pmtMessage2 %>";
            noEmptyCode = "<%=pmtMessage3 %>";
            msgInputFamily = "<%=pmtMessage4 %>";
            vcFormat = "<%=pmtMessage5 %>";
            codeFormat = "<%=pmtMessage6 %>";
            document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=UserId%>";
            resetTableHeight();
            blDN = "<%=gd.ClientID %>";
            tblDNObj = $("#" + "<%=gd.ClientID %>");
           RestTable();

        };
        function RestTable() {

            ClearGvExtTable(tblDN, 13);
            //     return;
            //
            tblDNObj.find(" tr").each(function(i, obj) {
                var tr = $(this);
                if (i > 10) {
                    tr.remove();
                }

            });
          

         }
         function checkAll(header) {
             $('#<%= gd.ClientID %> input[type=checkbox]').prop("checked", header.checked);
             document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
         }
         function setSelectVal(spanckb, id) {
             //CalDisappear();
             thebox = spanckb;
             oState = thebox.checked;
             if (oState) {
                 // attachVal(id);
                 document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
             }
             else {
                // detachVal(id);
             }
         }

         function attachVal(id) {
             selValue = ',' + ctlSelectedId.value;
             temp = ',' + id + ',';
             if (selValue.indexOf(temp) == -1) {
                 ctlSelectedId.value = ctlSelectedId.value + id + ',';
             }
         }

         function detachVal(id) {
             selValue = ',' + ctlSelectedId.value;
             temp = ',' + id + ',';
             selValue = selValue.replace(temp, ',');
             ctlSelectedId.value = selValue.substr(1);
         
         }
        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 60;
            var marginValue = 10;
            var tableHeigth = 300;


            try {
                tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div4.offsetHeight - div3.offsetHeight - adjustValue;
            }
            catch (e) {
                //ignore
            }
            //为了使表格下面有写空隙
            var extDivHeight = tableHeigth + marginValue;

            document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
            //alert(document.getElementById("div_<%=gd.ClientID %>").style.height)
            div2.style.height = extDivHeight + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
           
        }
        
        function clickTable(con)
        {
        
            if((selectedRowIndex!=null) && (selectedRowIndex!=parseInt(con.index, 10)))
            {
                setRowSelectedOrNotSelectedByIndex(selectedRowIndex,false, "<%=gd.ClientID %>");                
            }
            
            setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
            selectedRowIndex=parseInt(con.index, 10);

            setDetailInfo(con);      
            
            if (hasEditData())
            {
                
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
                document.getElementById("<%=txtCUSTSN.ClientID %>").disabled = true;
                document.getElementById('BtnBrowse').disabled = true;
               
                switch(updateFlag) {
                    case "CUSTSN":
                        document.getElementById("<%=cmbLine.ClientID %>").disabled = true;
                        document.getElementById("<%=cmbFamily.ClientID %>").disabled = true;
                        document.getElementById("<%=txtModel.ClientID %>").disabled = true;
                        break;
                    case "FamilyandModel":
                        document.getElementById("<%=cmbFamily.ClientID %>").disabled = true;
                        document.getElementById("<%=txtModel.ClientID %>").disabled = true;
                        document.getElementById("<%=cmbLine.ClientID %>").disabled = false;
                        
                        break;
                    case "Line":
                        document.getElementById("<%=cmbLine.ClientID %>").disabled = true;
                        document.getElementById("<%=cmbFamily.ClientID %>").disabled = false;
                        document.getElementById("<%=txtModel.ClientID %>").disabled = false;
                        break;
                    default:
                        document.getElementById("<%=cmbLine.ClientID %>").disabled = true;
                        document.getElementById("<%=cmbFamily.ClientID %>").disabled = true;
                        document.getElementById("<%=txtModel.ClientID %>").disabled = true;
                }
                document.getElementById("<%=hidDeleteID.ClientID %>").value = con.cells[0].innerText;
            }     
            else
            {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
                document.getElementById("<%=cmbLine.ClientID %>").disabled = false;
                document.getElementById("<%=cmbFamily.ClientID %>").disabled = false;
                document.getElementById("<%=txtModel.ClientID %>").disabled = false;
                document.getElementById("<%=txtCUSTSN.ClientID %>").disabled = false;
                document.getElementById('BtnBrowse').disabled = false;
                document.getElementById("<%=hidDeleteID.ClientID %>").value = "";
            } 
        }

        function AddUpdateComplete(id) {

            var gdObj = document.getElementById("<%=gd.ClientID %>");

            selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[0].innerText == id) {
                    selectedRowIndex = i;
                }
            }
            if (selectedRowIndex < 0) {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                clearDetailInfo();
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            }
        }
        var iSelectedRowIndex=null;

        function DeleteComplete() {
            document.getElementById("<%=hidDeleteID.ClientID %>").value = "";
        }
       
        function setGdHighLight(con)
        {
            if((iSelectedRowIndex!=null) && (iSelectedRowIndex!=parseInt(con.index, 10)))
            {
                //去掉过去高亮行
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex,false, "<%=gd.ClientID %>");                
            }     
            //设置当前高亮行   
            setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
            //记住当前高亮行
            iSelectedRowIndex=parseInt(con.index, 10);
            selectedRowIndex=parseInt(con.index, 10);
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
            var familyValue = "";
            selectedRowIndex = -1;
            document.getElementById("<%=hidFamily.ClientID %>").value = familyValue; 
            clearDetailInfo();
            ShowWait();
            return true;
        }
        var updateFlag = "";
        function setDetailInfo(con)
        {
            if (con.cells[0].innerText.trim() == "") {
                document.getElementById("<%=cmbLine.ClientID %>").value = "";
                document.getElementById("<%=cmbFamily.ClientID %>").value = "";
                document.getElementById("<%=txtModel.ClientID %>").value = "";
                document.getElementById("<%=txtCUSTSN.ClientID %>").value = "";
                document.getElementById("<%=cmbCheckINStation.ClientID %>").value = "";
                document.getElementById("<%=cmbHoldStation.ClientID %>").value = "";
                document.getElementById("<%=cmbHoldCode.ClientID %>").value = "";
                document.getElementById("<%=hidFamily.ClientID %>").value = "";
                document.getElementById("<%=txtHoldDescr.ClientID %>").value = "";
                
            }
            else {
                document.getElementById("<%=cmbLine.ClientID %>").value = con.cells[1].innerText.trim();
                document.getElementById("<%=cmbFamily.ClientID %>").value = con.cells[2].innerText.trim();
                document.getElementById("<%=hidFamily.ClientID %>").value = con.cells[2].innerText.trim();
                document.getElementById("<%=txtModel.ClientID %>").value = con.cells[3].innerText.trim();
                document.getElementById("<%=txtCUSTSN.ClientID %>").value = con.cells[4].innerText.trim();
                document.getElementById("<%=cmbCheckINStation.ClientID %>").value = con.cells[5].innerText.trim();
                document.getElementById("<%=cmbHoldStation.ClientID %>").value = con.cells[6].innerText.trim();
                document.getElementById("<%=cmbHoldCode.ClientID %>").value = con.cells[7].innerText.trim();
                document.getElementById("<%=txtHoldDescr.ClientID %>").value = con.cells[8].innerText.trim();
                if (document.getElementById("<%=txtCUSTSN.ClientID %>").value != "") {
                    updateFlag = "CUSTSN";
                }
                else if (document.getElementById("<%=txtModel.ClientID %>").value != "" || document.getElementById("<%=hidFamily.ClientID %>").value != "") {
                    updateFlag = "FamilyandModel";
                }
                else if (con.cells[1].innerText.trim() != "") {
                    updateFlag = "Line";
                }
            }
        }
        
        function clearDetailInfo()
        {
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            document.getElementById("<%=cmbLine.ClientID %>").value = "";
            document.getElementById("<%=cmbFamily.ClientID %>").value = "";
            document.getElementById("<%=txtModel.ClientID %>").value = "";
            document.getElementById("<%=txtCUSTSN.ClientID %>").value = "";
            document.getElementById("<%=hfCUSTSN.ClientID %>").value = "";
            document.getElementById("<%=cmbCheckINStation.ClientID %>").value = "";
            document.getElementById("<%=cmbHoldStation.ClientID %>").value = "";
            document.getElementById("<%=cmbHoldCode.ClientID %>").value = "";
            document.getElementById("<%=hidFamily.ClientID %>").value = "";
            document.getElementById("<%=txtHoldDescr.ClientID %>").value = "";
            document.getElementById("<%=cmbLine.ClientID %>").disabled = false;
            document.getElementById("<%=cmbFamily.ClientID %>").disabled = false;
            document.getElementById("<%=txtModel.ClientID %>").disabled = false;
            document.getElementById("<%=txtCUSTSN.ClientID %>").disabled = false;
            document.getElementById('BtnBrowse').disabled = false;
        }
        
        function clkSave()
        {
            if (checkInput())
            {
                selectedRowIndex = -1;
                ShowWait();
                return true;
            }
            return false;
        }
        function UploadSNListQuery() {
            var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
            var saveasUrl = "UploadModelList.aspx?ModelList=" + document.getElementById("<%=txtCUSTSNTop.ClientID %>").value;
            var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
           
            if (dlgReturn) {

                dlgReturn = dlgReturn.replace(/\r\n/g, ",");

                document.getElementById("<%=HiddenSNQuery.ClientID %>").value = RemoveBlank(dlgReturn);
                var arr = document.getElementById("<%=HiddenSNQuery.ClientID %>").value.split(',');
                alert("Total:"+ arr.length);
            }
            else {
                if (dlgReturn == "")
                { document.getElementById("<%=HiddenSNQuery.ClientID %>").value = ""; }
                return;
            }

        }
        function UploadCUSTSNList() {
            var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
            var saveasUrl = "UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelList.ClientID %>").value;
            var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
            if (dlgReturn) {
                dlgReturn = dlgReturn.replace(/\r\n/g, ",");
                document.getElementById("<%=hfCUSTSN.ClientID %>").value = RemoveBlank(dlgReturn);
                var arr = document.getElementById("<%=hfCUSTSN.ClientID %>").value.split(',');
                alert("Total:" + arr.length);
              
            }
            else {
                if (dlgReturn == "")
                { document.getElementById("<%=hfCUSTSN.ClientID %>").value = ""; }
                return;
            }
        }

        function RemoveBlank(modelList) {
            var arr = modelList.split(",");
            var model = "";
            if (modelList != "") {
                for (var m in arr) {
                    if (arr[m] != "") {
                        model = model + arr[m] + ",";
                    }
                }
                model = model.substring(0, model.length - 1)
            }
            return model;
        }

        function checkInput() {
            var line = document.getElementById("<%=cmbLine.ClientID %>").options[document.getElementById("<%=cmbLine.ClientID %>").selectedIndex].value;
            var family = document.getElementById("<%=cmbFamily.ClientID %>").options[document.getElementById("<%=cmbFamily.ClientID %>").selectedIndex].value;
            var model = document.getElementById("<%=txtModel.ClientID %>").value;
            if (document.getElementById("<%=txtCUSTSN.ClientID %>").value != "") {
                document.getElementById("<%=hfCUSTSN.ClientID %>").value = document.getElementById("<%=txtCUSTSN.ClientID %>").value;
            }
            var custsn = document.getElementById("<%=hfCUSTSN.ClientID %>").value
            if (custsn != "") {
                if (line != "" || family != "" || model != "") {
                    alert("line,family,model should be Empty ");
                    return false;
                }
            }
            if (family != "" && model != "") {
                alert("Family , Model can not have a value in same time");
                return false;
            }
            if (line == "" && family == "" && model == "" && custsn == "") {
                alert("line,family,model,custsn can't all Empty ");
                return false;
            }
            var checkInStation = document.getElementById("<%=cmbCheckINStation.ClientID %>").options[document.getElementById("<%=cmbCheckINStation.ClientID %>").selectedIndex].value;
            var holdStation = document.getElementById("<%=cmbHoldStation.ClientID %>").options[document.getElementById("<%=cmbHoldStation.ClientID %>").selectedIndex].value;
            var holdCode = document.getElementById("<%=cmbHoldCode.ClientID %>").options[document.getElementById("<%=cmbHoldCode.ClientID %>").selectedIndex].value;
            var holdDescr = document.getElementById("<%=txtHoldDescr.ClientID %>").value

            if (checkInStation == "") {
                alert("checkInStation can't Empty ");
                return false;
            }
            if (holdStation == "") {
                alert("holdStation can't Empty ");
                return false;
            }
            if (holdCode == "") {
                alert("holdCode can't Empty ");
                return false;
            }
            if (holdDescr == "") {
                alert("holdDescr can't Empty ");
                return false;
            }
            
            return true;
        }
        
        function clkDelete()
        {
            

                var _c = 0;
                var dnArr = new Array();
                var dnUdtArr = [];
                $("#" + "<%=gd.ClientID %>" + " tr:gt(0)").each(
                       function(i, obj) {
                           //tblDNObj.each.find("tr:gt(0)")(function(i, obj) {
                           var tr = $(this);
                           var _id = tr.find("td:eq(0)").text();
                           var _ox = tr.find("td:eq(12)").find('input:checkbox');
                           if (_ox.length > 0 && _ox.attr("checked") == "checked") {
                              // ctlSelectedId.value = ctlSelectedId.value + _id + ',';
                               dnArr.push(_id);
                           }
                       });
                       if (dnArr.length == 0) {
                           alert("Please Select");
                           return false;
                       }
                       if (confirm("确定要删除 " + dnArr.length + " 条记录么?")) {
                           var idlist = "";
                           for (var m in dnArr) {
                               if (dnArr[m] != "") {
                                   ctlSelectedId.value = ctlSelectedId.value + dnArr[m] + ',';
                               }
                           }
                       selectedRowIndex = -1;
                       clearDetailInfo();
                       ShowWait();
                      return true;
                   }  
            return false;
        }
      
      
    </script>
</asp:Content>


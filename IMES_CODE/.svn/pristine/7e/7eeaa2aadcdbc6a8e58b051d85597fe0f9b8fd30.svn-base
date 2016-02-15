<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="SpecialOrder.aspx.cs" Inherits="DataMaintain_SpecialOrder" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%-- ITC-1361-0033 --%>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1"  class="iMes_div_MainTainDiv1">
             <table width="100%" class="iMes_div_MainTainEdit" >
                <tr >
                    <td>
                        <asp:Label ID="lblFactoryPOTOP" runat="server" CssClass="iMes_label_13pt" Text="FactoryPO:"></asp:Label>
                    </td>
                    <td width="35%" colspan="3">
                        <input id="txtFactoryPOTOP" runat="server" maxlength="30"  class="iMes_input_losercase" />
                    </td>
                    <td></td>    
                    <td width="48%" align="right">
                      <input type="button" id="btnQuery" runat="server" onclick="if(clkButton())" onserverclick="btnQuery_ServerClick" class="iMes_button" value ="Query" />
                    </td>           
                </tr>
                <tr >
                    <td>
                        <asp:Label ID="lblCategoryTOP" runat="server" CssClass="iMes_label_13pt" Text="Category:"></asp:Label>
                    </td>
                    <td width="35%" colspan="3">
                        <asp:DropDownList ID="cmbCategoryTOP" runat="server" onChange="javascript:ClearFactoryPOTopValue()" ></asp:DropDownList>
                    </td>
                    <td></td>    
                    <td width="48%" align="right">
                      <input type="button" id="btnUpLoad" runat="server" class="iMes_button" value="UpLoad" onclick="clkButton()" />
                    </td>           
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStatusTOP" runat="server" CssClass="iMes_label_13pt" Text="Status:"></asp:Label>
                    </td>
                    <td width="35%" colspan="3">
                        <asp:DropDownList ID="cmbStatusTop" runat="server" onChange="javascript:ClearFactoryPOTopValue()">
                            <asp:ListItem Text="Created" Value="Created" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                            <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                        </asp:DropDownList>   
                    </td>
                    <td></td>
                    <td  align="right">
                      <input type="button" id="btnDelete" runat="server" class="iMes_button" onclick="if(clkButton())" onserverclick="btnDelete_ServerClick" />
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="lblStartTime" runat="server" CssClass="iMes_label_13pt" Text="StartTime:"></asp:Label>
                    </td>
                    <td>
                        <input type="text" name="ttStartTime" id="ttStartTime" style="width:100px;" readonly="readonly" class="iMes_textbox_input_Yellow" />
                        <input id="btnCalStart" type="button" value=".." onclick="ClearFactoryPOTopValue();showCalendar('ttStartTime')"  style="width: 17px" class="iMes_button"  />
                    </td>
                    
                    <td>
                        <asp:Label ID="lblEndTime" runat="server" CssClass="iMes_label_13pt" Text="EndTime"></asp:Label>
                    </td>
                    <td > 
                        <input type="text" name="ttEndTime" id="ttEndTime" style="width:100px;" readonly="readonly" class="iMes_textbox_input_Yellow" />
                        <input id="btnCalEnd" type="button" value=".." onclick="ClearFactoryPOTopValue();showCalendar('ttEndTime')"  style="width: 17px" class="iMes_button"  />                           
                    </td>
                </tr>
             </table>
             <table>
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%">SpecialOrder List:</asp:Label>
                    </td>
                </tr>
             </table> 
        </div>
        
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
        <div id="div2" style="height :428px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%"  RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="418px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' 
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>   
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>
                        <td style="width:10%; padding-left:2px;">
                            <asp:Label ID="lblFactoryPO" runat="server" CssClass="iMes_label_13pt" Text="FactoryPO:" ></asp:Label>
                        </td>
                        <td width="34%">
                            <input id="txtFactoryPO" runat="server" maxlength="30" style="width:96%" class="iMes_input_losercase" />
                        </td>    
                        <td width="10%">
                            <asp:Label ID="lblStatus" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="35%" >
                             <asp:Label ID="lblStatusValue" runat="server" CssClass="iMes_label_13pt" Width="96%" ></asp:Label>
                        </td>                           
                        <td align="right" width="11%">
                            <input type="button" id="btnAdd" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAdd_ServerClick" />
                        </td>           
                    </tr>
                    <tr>
                        <td >
                            <asp:Label ID="lblCategory" runat="server" CssClass="iMes_label_13pt" Text="Category:"></asp:Label>
                        </td>
                        <td >
                            <asp:DropDownList ID="cmbCategory" runat="server"></asp:DropDownList>
                        </td>  
                        <td >
                            <asp:Label ID="lblAssetTag" runat="server" CssClass="iMes_label_13pt" Text="AssetTag:"></asp:Label>
                        </td>
                        <td >
                            <input id="txtAssetTag" runat="server" maxlength="12" style="width:96%" class="iMes_input_losercase" />
                        </td>                            
                        <td align="right">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" style="background-color :Green " class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick" />
                            <input type="button" id="btnUploadQuery" runat="server" style="display:none" class="iMes_button" onserverclick="btnUpLoadQuery_ServerClick" />
                            <input type="hidden" id="dStatus" runat="server" />                            
                        </td>           
                    </tr>
                    <tr>
                        <td >
                            <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt" Text="Qty:"></asp:Label>
                        </td>
                        <td >
                            <input id="txtQty" runat="server" maxlength="12" style="width:96%" class="iMes_input_losercase"
                            onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" />
                        </td>  
                        <td >
                            <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt" Text="Remark:"></asp:Label>
                        </td>
                        <td >
                            <input id="txtRemark" runat="server" style="width:96%" class="iMes_input_losercase" />
                        </td>                            
                        <td></td>           
                    </tr>               
            </table> 
        </div>  
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnUploadQuery" EventName="ServerClick" />
        </Triggers>                      
        </asp:UpdatePanel>   
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="dUploadResultData" runat="server" />
        <input type="hidden" id="dCategoryTOP" runat="server" />
        <input type="hidden" id="dStatusTOP" runat="server" />  
        <input type="hidden" id="dStartTime" runat="server" />
        <input type="hidden" id="dEndTime" runat="server" /> 
        <input type="hidden" id="dCategory" runat="server" />
        <input type="hidden" id="dFactoryPO" runat="server" />
    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
    
    var msg1="";
    var msg2="";
    var msg3="";
    var msg4="";
    var msg5="";
    var msg6="";
    var msg7="";
    var msg8="";
    var msg9="";
    var msg10="";
    
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";
        msg6 ="<%=pmtMessage6%>";
        msg7 ="<%=pmtMessage7%>";
        msg8 ="<%=pmtMessage8%>";
        msg9 ="<%=pmtMessage9%>";
        msg10 ="<%=pmtMessage10%>";
        
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        ShowRowEditInfo(null);
        //设置表格的高度  
        resetTableHeight();
    };
    
   //设置表格的高度  
    function resetTableHeight()
    {    
        //动态调整表格的高度
        var adjustValue=60;     
        var marginValue=12; 
        var tableHeigth=300;
        try{
            tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
        }
        catch(e){
            //ignore
        }                
        //为了使表格下面有写空隙
        var extDivHeight=tableHeigth+marginValue;
        div2.style.height=extDivHeight+"px";
        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
    }
    
    function clkDelete() {
        var ret = confirm(msg3); 
        if (!ret) {
            return false;
        }
        return true;
    }

    function DeleteComplete()
    {   
       ShowRowEditInfo(null);
    }

    function clkSave()  {
       return check();
    }

    function clkQuery() {
        if (document.getElementById("<%=txtFactoryPOTOP.ClientID %>").value != "") {
            
        }
        else {
            if (document.getElementById("<%=cmbCategoryTOP.ClientID %>").selectedIndex == 0) {
                alert("請選擇Category...");
                return false;
            }
            if (document.getElementById("ttStartTime").value == "") {
                alert("請選擇開始時間...");
                return false;
            }
            if (document.getElementById("ttEndTime").value == "") {
                alert("請選擇結束時間...");
                return false;
            }
            document.getElementById("<%=dCategoryTOP.ClientID %>").value = document.getElementById("<%=cmbCategoryTOP.ClientID %>").value;
            document.getElementById("<%=dStatusTOP.ClientID %>").value = document.getElementById("<%=cmbStatusTop.ClientID %>").value;
            document.getElementById("<%=dStartTime.ClientID %>").value = document.getElementById("ttStartTime").value;
            document.getElementById("<%=dEndTime.ClientID %>").value = document.getElementById("ttEndTime").value;
        }
        
        return true;
    }

    function clkAdd() {
       return check();
    }

    function check() {
        if (document.getElementById("<%=txtFactoryPO.ClientID %>").value == "") {
            alert("請輸入FactoryPO");
            document.getElementById("<%=txtFactoryPO.ClientID %>").focus();
            return false;
        }
        if (document.getElementById("<%=cmbCategory.ClientID %>").value == "") {
            alert("請選擇Category");
            document.getElementById("<%=cmbCategory.ClientID %>").focus();
            document.getElementById("<%=dCategory.ClientID %>").value = "";
            return false;
        }
        document.getElementById("<%=dCategory.ClientID %>").value = document.getElementById("<%=cmbCategory.ClientID %>").value;
        if (document.getElementById("<%=txtAssetTag.ClientID %>").value == "") {
            alert("請輸入txtAssetTag");
            document.getElementById("<%=txtAssetTag.ClientID %>").focus();
            return false;
        }
        if (document.getElementById("<%=txtQty.ClientID %>").value == "") {
            alert("請輸入txtQty");
            document.getElementById("<%=txtQty.ClientID %>").focus();
            return false;
        }
        return true;
    }

    function clkUpLoad() {
        if (document.getElementById("<%=cmbCategoryTOP.ClientID %>").selectedIndex == 0) {
            alert("請選擇Category...");
            return false;
        }
        return true;
    }

    function clkButton()
    {
        switch (event.srcElement.id) 
        {
            case "<%=btnQuery.ClientID %>":
                if (clkQuery() == false) {
                    return false;
                }
                break;
                
            case "<%=btnSave.ClientID %>":
                if(clkSave()==false) {
                    return false;
                }
                break;
                
            case "<%=btnDelete.ClientID %>":
                if(clkDelete()==false) {
                    return false;
                }
                break;
                
            case "<%=btnAdd.ClientID %>": 	  
                if(clkAdd()==false) {        
                    return false;
                }
                break;

            case "<%=btnUpLoad.ClientID %>":
                if (clkUpLoad() == false) {
                    return false;
                }
                var editor = document.getElementById("<%=HiddenUserName.ClientID %>").value;
                var Category = document.getElementById("<%=cmbCategoryTOP.ClientID %>").value;
                var dlgFeature = "dialogHeight:260px;dialogWidth:424px;center:yes;status:no;help:no";
                editor = encodeURI(Encode_URL2(editor));
                Category = encodeURI(Encode_URL2(Category));
                var dlgReturn = window.showModalDialog("SpecialOrderUploadDlg.aspx?userName=" + editor + "&Category=" + Category, window, dlgFeature);
                if (dlgReturn == "OK") {
                    document.getElementById("<%=dUploadResultData.ClientID %>").value = uploadResultDataString;
                    document.getElementById("<%=dCategoryTOP.ClientID %>").value = Category;
                    document.getElementById("<%=btnUploadQuery.ClientID %>").click();
                    ShowWait();
                }
                return;
                break; 
        }   
        ShowWait();
        return true;
    }

    function ClearFactoryPOTopValue() {
        document.getElementById("<%=txtFactoryPOTOP.ClientID %>").value = "";
    }
    
    function clickTable(con) {
         setGdHighLight(con);         
         ShowRowEditInfo(con);
    }
    
    function ShowRowEditInfo(con) {
        if(con==null)
        {
            document.getElementById("<%=txtFactoryPO.ClientID %>").value = "";
            document.getElementById("<%=txtAssetTag.ClientID %>").value = "";
            document.getElementById("<%=txtQty.ClientID %>").value = "";
            document.getElementById("<%=txtRemark.ClientID %>").value = "";
            setInputOrSpanValue(document.getElementById("<%=lblStatusValue.ClientID %>"), "");
            document.getElementById("<%=cmbCategory.ClientID %>").selectedIndex = 0
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
            return;    
        }
        var FactoryPO = con.cells[0].innerText.trim();
        if (FactoryPO == "")
        {
            document.getElementById("<%=txtFactoryPO.ClientID %>").value = "";
            document.getElementById("<%=txtAssetTag.ClientID %>").value = "";
            document.getElementById("<%=txtQty.ClientID %>").value = "";
            document.getElementById("<%=txtRemark.ClientID %>").value = "";
            setInputOrSpanValue(document.getElementById("<%=lblStatusValue.ClientID %>"), "");
            document.getElementById("<%=cmbCategory.ClientID %>").selectedIndex = 0
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true; 
            return;    
        }
        else {
            document.getElementById("<%=txtFactoryPO.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=dFactoryPO.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=txtAssetTag.ClientID %>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=txtQty.ClientID %>").value = con.cells[3].innerText.trim();
            document.getElementById("<%=txtRemark.ClientID %>").value = con.cells[5].innerText.trim();
            document.getElementById("<%=cmbCategory.ClientID %>").value = con.cells[1].innerText.trim();
            setInputOrSpanValue(document.getElementById("<%=lblStatusValue.ClientID %>"), con.cells[4].innerText.trim()); 
            var Status = con.cells[4].innerText.trim()
            if (Status == "Created") {
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;    
            }
            else {
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;    
            }        
        }
    }
   
   
    function AddUpdateComplete(FactoryPO)
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
            if (gdObj.rows[i].cells[0].innerText == FactoryPO)
            {
                selectedRowIndex=i;  
            }        
        }
        if(selectedRowIndex<0)
        {
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;        
            return;
        }
        else
        {  
            var con=gdObj.rows[selectedRowIndex];
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
        }        
    }
    
    var iSelectedRowIndex=null; 
    function setGdHighLight(con)
    {
        if((iSelectedRowIndex!=null) && (iSelectedRowIndex!=parseInt(con.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex,false, "<%=gd.ClientID %>");                
        }        
        setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
        iSelectedRowIndex=parseInt(con.index, 10);    
    }
    
    function DealHideWait()
    {
        HideWait();   
    }
    
    function disposeTree(sender, args) {
        var elements = args.get_panelsUpdating();
        for (var i = elements.length - 1; i >= 0; i--) {
            var element = elements[i];
            var allnodes = element.getElementsByTagName('*'),
            length = allnodes.length;
            var nodes = new Array(length)
            for (var k = 0; k < length; k++) {
                nodes[k] = allnodes[k];
            }
            for (var j = 0, l = nodes.length; j < l; j++) {
                var node = nodes[j];
                if (node.nodeType === 1) {
                    if (node.dispose && typeof (node.dispose) === "function") {
                        node.dispose();
                    }
                    else if (node.control && typeof (node.control.dispose)=== "function") {
                        node.control.dispose();
                    }

                    var behaviors = node._behaviors;
                    if (behaviors) {
                        behaviors = Array.apply(null, behaviors);
                        for (var k = behaviors.length - 1; k >= 0; k--) {
                            behaviors[k].dispose();
                        }
                    }
                }
            }
            element.innerHTML = "";
        }

    }
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(disposeTree);
   
    </script>
</asp:Content>


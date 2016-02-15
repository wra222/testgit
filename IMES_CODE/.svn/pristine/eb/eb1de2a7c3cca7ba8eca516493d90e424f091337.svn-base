<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ConstValueType.aspx.cs" Inherits="DataMaintain_ConstValueType" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="iMes_div_MainTainEdit" >             
                <tr >
                    <td style="width:10%">
                        <asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:30%" align ="left" >
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                        <ContentTemplate>                    
                            <iMESMaintain:CmbConstValueTypeForType runat="server" ID="cmbConstValueType" Width="100%" IsSelectFirstNotNull="true"></iMESMaintain:CmbConstValueTypeForType>
                        </ContentTemplate>
                        </asp:UpdatePanel>                            
                    </td>
                    <td style="width:60%" align="left">
                        <asp:Label ID="lblDesInfo" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>       
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="20%" align="right">
                       <input type="button" id="btnDelete" runat="server" class="iMes_button" onclick="if(clkButton())" onserverclick="btnDelete_ServerClick"></input>
                    </td>      
                    <td width="20%" align="right">
                       <input type="button" id="btnAddType" runat="server" class="iMes_button" onclick="if(clkButton())" onserverclick="btnAddType_ServerClick" visible=false></input>
                    </td>        
                </tr>
             </table>  
        </div>


        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
            <div id="div2" style="height:366px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%" RowStyle-Height="20"
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="CheckAll" runat="server" onclick="javascript: SelectAllCheckboxes(this);" ToolTip="" /> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox id="chk" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </iMES:GridViewExt>
            </div>
            <input type="hidden" id="HidValueList" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel> 
        
        
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>    
                        
                        <td style="width:10%">
                            <asp:Label ID="lblValue" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td style="width:80%">
                            <asp:TextBox ID="dValue" runat="server" onkeypress="OnKeyPress(this)"  MaxLength="3000" Width="90%" SkinId="textBoxSkin" ></asp:TextBox>
                            &nbsp;<input id="BtnBrowse" type="button" value="Browse"  onclick="UploadModelList()" />
                        </td>
                        <td style="width:10%"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescr" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="dDescr" runat="server"   MaxLength="255"  Width="90%"  SkinId="textBoxSkin"></asp:TextBox>
                        </td>                      
                        <td align="right">
                           <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></input>
                        </td>           
                    </tr>                                                            
    
            </table>
        </div>  
   
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
                <asp:AsyncPostBackTrigger ControlID="btnConstValueTypeChange" EventName="ServerClick" />
            </Triggers>                      
        </asp:UpdatePanel>         
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="HidID" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="selecttype" runat="server" />
        <input type="hidden" id="hidSelectId" runat="server"  />
        <input type="hidden" id="hidOffInsert" runat="server"  />
        <input type="hidden" id="hidOffDelete" runat="server"  />
        <button id="btnConstValueTypeChange" runat="server" type="button" style="display:none" onserverclick ="btnConstValueTypeChange_ServerClick"> </button>           
        <button id="btnTypeListUpdate" runat="server" type="button" style="display:none" onserverclick ="btnTypeListUpdate_ServerClick"> </button>           
    </div>
    
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    <script language="javascript" type="text/javascript">


    var msg1 = "";
    var msg2 = "";
    var msg3 = "";
    var msg4 = "";
    var msg5 = "";
    var msg6 = "";
    var msg7 = "";
    var msg8 = "";
    var ctlSelectedId = document.getElementById("<%=hidSelectId.ClientID%>");
    var url = IMESUti.Url;

    function clkButton()
    {
       switch(event.srcElement.id)
       {
           case "<%=btnSave.ClientID %>":
                if(clkSave() == false)
                {                
                    return false;
                }
 	            break;
 	            
           case "<%=btnDelete.ClientID %>":
           
                if(clkDelete()==false)
                {                
                    return false;
                }
                break;

            case "<%=btnAddType.ClientID %>":
                var editor = document.getElementById("<%=HiddenUserName.ClientID %>").value;
                var dlgFeature = "dialogHeight:260px;dialogWidth:424px;center:yes;status:no;help:yes";
                editor = encodeURI(Encode_URL2(editor));

                var dlgReturn = window.showModalDialog("ConstValueTypeDlg.aspx?userName=" + editor, window, dlgFeature);
                if (dlgReturn == "OK") {
                    document.getElementById("<%=btnTypeListUpdate.ClientID %>").click();
                }
                else {
                    return false;
                }
                break;
        }   
    	
        ShowWait();
        return true;
    }
  
    var iSelectedRowIndex = null; 
    function setGdHighLight(con)
    {
        if((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex,false, "<%=gd.ClientID %>");
        }        
        
        setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
        iSelectedRowIndex=parseInt(con.index, 10);    
    }
        
    function initControls() {
        document.getElementById("<%=selecttype.ClientID%>").value = getConstValueTypeCmbText();
        getConstValueTypeCmbObj().onchange = ConstValueTypeSelectOnChange;
        /*
        var index = getConstValueTypeCmbObj().getFirstNoNullIndex();
        if (index > 0) {
            getConstValueTypeCmbObj().setSelected(index);
        }
        */
    }

    function ConstValueTypeSelectOnChange() {
        document.getElementById("<%=selecttype.ClientID%>").value = getConstValueTypeCmbText();
        document.getElementById("<%=btnConstValueTypeChange.ClientID%>").click();
        ShowWait();
    }
    
    
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";
        msg6 = "<%=pmtMessage6%>";
        msg7 = "<%=pmtMessage7%>";
        msg8 = "<%=pmtMessage8%>";
        //document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        initControls();  
        ShowRowEditInfo(null);
      
        //设置表格的高度  
        resetTableHeight();
        var insert = url.GetParamByName("OffInsert");
        if (insert == "Y") {
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }
        var btndel = url.GetParamByName("OffDelete");
        if (btndel == "Y") {
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        }
    };

    //设置表格的高度  
    function resetTableHeight()
    {    
        //动态调整表格的高度
        var adjustValue = 55;     
        var marginValue = 12; 
        var tableHeigth = 300;
        
        try{
            tableHeigth = document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
        }
        catch(e){        
            //ignore
        }        
                
        //为了使表格下面有些空隙
        var extDivHeight = tableHeigth + marginValue;
        div2.style.height = extDivHeight + "px";
        document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
        document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
    }

    function SelectAllCheckboxes(spanChk) {
        elm = document.forms[0];
        for (i = 0; i <= elm.length - 1; i++) {
            if (elm[i].type == "checkbox" && elm[i].id != spanChk.id) {
                if (elm.elements[i].checked != spanChk.checked)
                    elm.elements[i].click();
            }
        }
    }

    function setSelectVal(spanckb, id) {
        thebox = spanckb;
        oState = thebox.checked;
        if (oState) {
            attachVal(id);
        }
        else {
            detachVal(id);
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

    function clkDelete() {

        var value = document.getElementById("<%=dValue.ClientID %>").value.trim();
        if (ctlSelectedId.value != "") {
            var check = ctlSelectedId.value.split(',');
            var flag = false;
            for (var i = 0; i < check.length; i++) {
                if (check[i] != "") {
                    flag = true;
                }
            }
            if (!flag) {
                alert("Please select Data");
                return false;
            }
        }
        if (value == "" && ctlSelectedId.value == "") {
            alert("Please select Data");
            return false;
        }


        var valuelist = document.getElementById("<%=HidValueList.ClientID %>").value.trim();
        if (valuelist == "") {
            var ret = confirm(msg4);
            if (!ret) {
                return false;
            }
            return true;
        }
        else {
            var ret = confirm(msg8);
            if (!ret) {
                return false;
            }
            return true;
        }
    }
   
   function DeleteComplete()
   {   
       ShowRowEditInfo(null);
   }
   
   function clkSave()
   {
       //ShowInfo("");
       return check();        
   }
   
   function check()
   {
        var value = document.getElementById("<%=dValue.ClientID %>").value.trim();
        var valuelist = document.getElementById("<%=HidValueList.ClientID %>").value.trim();
        if (getConstValueTypeCmbObj().value.trim() == "")
        {
            alert(msg5);    //"请选择[Type]"
            return false;    
        }

        if (value == "" && valuelist =="") {
            alert(msg1);    //"请输入[Value]"
            return false;
        }        

        return true;
   }
   
    function clickTable(con)
    {
        setGdHighLight(con);         
        ShowRowEditInfo(con);
    }
    
    function setNewItemValue()
    {
        //getConstValueTypeCmbObj().selectedIndex = 0;
        
        document.getElementById("<%=dValue.ClientID %>").value = ""
        document.getElementById("<%=dDescr.ClientID %>").value = "";
        document.getElementById("<%=HidID.ClientID %>").value = "";
    }
    
    function ShowRowEditInfo(con)
    {
         if(con == null)
         {
            setNewItemValue();
            return;    
         }

         document.getElementById("<%=dValue.ClientID %>").value = con.cells[1].innerText.trim();
         document.getElementById("<%=dDescr.ClientID %>").value = con.cells[2].innerText.trim();
         document.getElementById("<%=HidID.ClientID %>").value = con.cells[6].innerText.trim();
         //getConstValueTypeCmbObj().value = con.cells[1].innerText.trim(); //Type
         var currentId = con.cells[6].innerText.trim(); 
  
         if(currentId == "")
         {
            setNewItemValue();
            return;
         }
    }
    function UploadModelList() {
        var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "../DataMaintain/ConstValueTypeListDlg.aspx?ModelList=" + document.getElementById("<%=HidValueList.ClientID %>").value;
        var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
        if (dlgReturn) {

            dlgReturn = dlgReturn.replace(/\r\n/g, ",");
            document.getElementById("<%=HidValueList.ClientID %>").value = RemoveBlank(dlgReturn);
        }
        else {
            if (dlgReturn == "")
            { document.getElementById("<%=HidValueList.ClientID %>").value = ""; }
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

    function UpdatalblDescription(Descrip) {
        setInputOrSpanValue(document.getElementById("<%=lblDesInfo.ClientID %>"), Descrip);
    }

    function AddUpdateComplete(id)
    {
        var gdObj = document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex = -1;
        for(var i = 1; i < gdObj.rows.length; i++)
        {
           if(gdObj.rows[i].cells[0].innerText == id)
           {
               selectedRowIndex = i; 
               break; 
           }        
        }
        
        if(selectedRowIndex < 0)
        {
            ShowRowEditInfo(null);    
            return;
        }
        else
        {            
            var con = gdObj.rows[selectedRowIndex];
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
        }               
    }

     
    function DealHideWait()
    {
        HideWait();
        getConstValueTypeCmbObj().disabled = false; 
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


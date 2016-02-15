<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="CheckItemType.aspx.cs" Inherits="DataMaintain_CheckItemType" ValidateRequest="false" %>
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
                    <td style="width: 200px;">
                        <asp:Label ID="lbltopName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="85%" align ="left" >
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Always" RenderMode="Inline" Visible="true" >
                        <ContentTemplate>                    
                            <asp:DropDownList ID="cmbCheckItemType" runat="server" Width="30%">
                            </asp:DropDownList>
                        </ContentTemplate>
                        </asp:UpdatePanel>                            
                    </td>    
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="20%" align="right">
                       <input type="button" id="btnDelete" runat="server" class="iMes_button" onclick="if(clkButton())" onserverclick="btnDelete_ServerClick"></ input>
                    </td>      
                    <td width="20%" align="right">
                    </td>        
                </tr>
             </table>  
        </div>
        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
            <div id="div2" style="width: 105%; height: 370px;  overflow: auto;">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="2500px" RowStyle-Height="20"
                         GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
            </div>
            <input type="hidden" id="HidValueList" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel> 
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                <tr>    
                    <td style="width:10%">
                        <asp:Label ID="lblCheckItemType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="txtCheckItemType" runat="server"   MaxLength="20" Width="99%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td style="width:10%">
                        <asp:Label ID="lblDisPlayName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="txtDisPlayName" runat="server"   MaxLength="50" Width="99%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td width="20%"></td>
                </tr>
                <tr>    
                    <td style="width:10%">
                        <asp:Label ID="lblFilterModule" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan = "3">
                        <asp:TextBox ID="txtFilterModule" runat="server"   MaxLength="128" Width="99%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td width="20%"></td>
                </tr>
                <tr>    
                    <td style="width:10%">
                        <asp:Label ID="lblMatchModule" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan = "3">
                        <asp:TextBox ID="txtMatchModule" runat="server"   MaxLength="128" Width="99%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td width="20%"></td>
                </tr>
                <tr>    
                    <td style="width:10%">
                        <asp:Label ID="lblCheckModule" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan = "3">
                        <asp:TextBox ID="txtCheckModule" runat="server"   MaxLength="128" Width="99%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td width="20%"></td>
                </tr>
                <tr>    
                    <td style="width:10%">
                        <asp:Label ID="lblSaveModule" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan = "3">
                        <asp:TextBox ID="txtSaveModule" runat="server"   MaxLength="128" Width="99%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td width="20%"></td>
                </tr>
                <tr>    
                    <td style="width:10%">
                        <asp:Label ID="lblNeedUniqueCheck" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:DropDownList ID="cmbNeedUniqueCheck" runat="server"></asp:DropDownList>
                    </td>
                    <td style="width:10%">
                        <asp:Label ID="lblNeedCommonSave" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:DropDownList ID="cmbNeedCommonSave" runat="server"></asp:DropDownList>
                    </td>
                    <td width="20%" align="right">
                        <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></ input>
                    </td>
                </tr>
                <tr>    
                    <td style="width:10%">
                        <asp:Label ID="lblNeedPartForbidCheck" runat="server" CssClass="iMes_label_13pt" Text="NeedPartForbidCheck:"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:DropDownList ID="cmbNeedPartForbidCheck" runat="server"></asp:DropDownList>
                    </td>
                    <td style="width:10%"></td>
                    <td width="30%"></td>
                    <td width="20%" align="right"></td>
                </tr>
            </table> 
        </div>  
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
                <asp:AsyncPostBackTrigger ControlID="btnCheckItemTypeChange" EventName="ServerClick" />
            </Triggers>                      
        </asp:UpdatePanel>         
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="hidChcekItemType" runat="server" />
        <%--<input type="hidden" id="hidcmbNeedUniqueCheck" runat="server" />
        <input type="hidden" id="hidcmbNeedCommonSave" runat="server" />--%>
        <input type="hidden" id="dTableHeight" runat="server" />
        
        <button id="btnCheckItemTypeChange" runat="server" type="button" style="display:none" onserverclick ="btnCheckItemTypeChange_ServerClick"> </button>           
        <button id="btnTypeListUpdate" runat="server" type="button" style="display:none" onserverclick ="btnTypeListUpdate_ServerClick"> </button>           
    </div>
    
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    <script language="javascript" type="text/javascript">
    var msg4 = "";
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

    function getCheckItemTypeCmbObj() {
        return document.getElementById("<%=cmbCheckItemType.ClientID %>");
    }
        
    function initControls()
    {
        getCheckItemTypeCmbObj().onchange = CheckItemTypeSelectOnChange;
    }

    function CheckItemTypeSelectOnChange()
    {
        document.getElementById("<%=btnCheckItemTypeChange.ClientID%>").click();
        ShowWait();
    }
    
    window.onload = function()
    {
        msg4 ="<%=pmtMessage4%>";
        initControls();  
        ShowRowEditInfo(null);
        //设置表格的高度  
        resetTableHeight();
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
    
    function clkDelete()
    {   
         var ret = confirm(msg4);
         if (!ret) 
         {
             return false;
         }
         
         return true;        
    }
   
    function DeleteComplete()
    {   
       ShowRowEditInfo(null);
    }
   
    function clkSave()
    {
        return check();        
    }

    function check() 
    {
        var checkItemType = document.getElementById("<%=txtCheckItemType.ClientID %>").value.trim();
        var filterModule =  document.getElementById("<%=txtFilterModule.ClientID %>").value.trim();
        var matchModule =   document.getElementById("<%=txtMatchModule.ClientID %>").value.trim();
        var checkModule =   document.getElementById("<%=txtCheckModule.ClientID %>").value.trim();
        var saveModule = document.getElementById("<%=txtSaveModule.ClientID %>").value.trim();
        
        if (checkItemType == "") {
            alert("請輸入CheckItemType");
            return false;
        }
        if (filterModule == "" && matchModule == "" && checkModule == "" && saveModule == "") {
            alert("FilterModule, MatchModule, CheckModule, SaveModule,至少需一欄有值");
            return false;
        }        
        return true;
    }
   
    function clickTable(con)
    {
        setGdHighLight(con);         
        ShowRowEditInfo(con);
    }
    
    function setNewItemValue() {
        document.getElementById("<%=txtCheckItemType.ClientID %>").value = "";
        document.getElementById("<%=txtDisPlayName.ClientID %>").value = "";
        document.getElementById("<%=txtFilterModule.ClientID %>").value = "";
        document.getElementById("<%=txtMatchModule.ClientID %>").value = "";
        document.getElementById("<%=txtCheckModule.ClientID %>").value = "";
        document.getElementById("<%=txtSaveModule.ClientID %>").value = "";
        document.getElementById("<%=cmbNeedUniqueCheck.ClientID %>").SelectedIndex = 0;
        document.getElementById("<%=cmbNeedCommonSave.ClientID %>").SelectedIndex = 0;
        document.getElementById("<%=cmbNeedPartForbidCheck.ClientID %>").SelectedIndex = 0;
        document.getElementById("<%=hidChcekItemType.ClientID %>").value = "";
        document.getElementById("<%=btnDelete.ClientID %>").disabled = true; 
    }
    
    function ShowRowEditInfo(con)
    {
        if(con == null)
        {
            setNewItemValue();
            return;
        }
        document.getElementById("<%=txtCheckItemType.ClientID %>").value = con.cells[0].innerText.trim();
        document.getElementById("<%=txtDisPlayName.ClientID %>").value = con.cells[1].innerText.trim();
        document.getElementById("<%=txtFilterModule.ClientID %>").value = con.cells[2].innerText.trim();
        document.getElementById("<%=txtMatchModule.ClientID %>").value = con.cells[3].innerText.trim();
        document.getElementById("<%=txtCheckModule.ClientID %>").value = con.cells[4].innerText.trim();
        document.getElementById("<%=txtSaveModule.ClientID %>").value = con.cells[5].innerText.trim();
        document.getElementById("<%=cmbNeedUniqueCheck.ClientID %>").value = con.cells[6].innerText.trim();
        document.getElementById("<%=cmbNeedCommonSave.ClientID %>").value = con.cells[7].innerText.trim();
        document.getElementById("<%=cmbNeedPartForbidCheck.ClientID %>").value = con.cells[8].innerText.trim(); 
        document.getElementById("<%=hidChcekItemType.ClientID %>").value = con.cells[0].innerText.trim();
        var currentCheckItemType = con.cells[0].innerText.trim();
        if (currentCheckItemType == "")
        {
            setNewItemValue();
            return;
        }
        else
        {
            document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
        }         
    }

    function AddUpdateComplete(CheckItemType)
    {
        var gdObj = document.getElementById("<%=gd.ClientID %>");
        var selectedRowIndex = -1;
        for(var i = 1; i < gdObj.rows.length; i++)
        {
            if (gdObj.rows[i].cells[0].innerText == CheckItemType)
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
        getCheckItemTypeCmbObj().disabled = false;
    }

//    function disposeTree(sender, args) {
//            var elements = args.get_panelsUpdating();
//            for (var i = elements.length - 1; i >= 0; i--) {
//                var element = elements[i];
//                var allnodes = element.getElementsByTagName('*'),
//                length = allnodes.length;
//                var nodes = new Array(length)
//                for (var k = 0; k < length; k++) {
//                    nodes[k] = allnodes[k];
//                }
//                for (var j = 0, l = nodes.length; j < l; j++) {
//                    var node = nodes[j];
//                    if (node.nodeType === 1) {
//                        if (node.dispose && typeof (node.dispose) === "function") {
//                            node.dispose();
//                        }
//                        else if (node.control && typeof (node.control.dispose)=== "function") {
//                            node.control.dispose();
//                        }

//                        var behaviors = node._behaviors;
//                        if (behaviors) {
//                            behaviors = Array.apply(null, behaviors);
//                            for (var k = behaviors.length - 1; k >= 0; k--) {
//                                behaviors[k].dispose();
//                            }
//                        }
//                    }
//                }
//                element.innerHTML = "";
//            }

//        }
//        Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(disposeTree);
    </script>
</asp:Content>


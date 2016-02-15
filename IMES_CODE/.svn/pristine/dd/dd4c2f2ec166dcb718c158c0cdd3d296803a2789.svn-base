<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="BSamLocation.aspx.cs" Inherits="DataMaintain_BSamLocation" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%--
 ITC-1361-0016
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="iMes_div_MainTainEdit" >            
           
                <tr>
                    <td width = "40%">
                        <input type="radio" name="selectdata" id="SelectLocation" checked runat="server" />
                        <asp:Label ID="lbLocation" runat="server" CssClass="iMes_label_11pt" Text="庫位狀態："></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="cmbLocation" runat="server" Width="250px">
                            <asp:ListItem Text="全部" Value="All"></asp:ListItem>
                            <asp:ListItem Text="空庫位" Value="Empty"></asp:ListItem>
                            <asp:ListItem Text="使用中庫位" Value="Occupy"></asp:ListItem>
                            <asp:ListItem Text="禁用庫位" Value="Hold"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width = "10%"></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td> 
                        <input type="radio" name="selectdata" id="SelectModel" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lbModel" runat="server" CssClass="iMes_label_11pt" Text="Model:"></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="cmbModel" runat="server" Width="250px"></asp:DropDownList>
                    </td>
                    <td>
                        <input id="btnQuery" type="button" runat="server" value="Query" onserverclick="btnQuery_serverclick" onclick="checkQuery();beginWaitingCoverDiv();"/>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%" Text="BSam Location List:"></asp:Label>
                    </td>
                    <td width="40%"></td> 
                    <td align="right">
                    </td>            
                </tr>
             </table>  
        </div>


        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div id="div2" style="height:400px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%" RowStyle-Height="20"
                        GvExtWidth="100%" GvExtHeight="390px" Height="1px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'  EnableViewState= "false" onrowdatabound="GridView1_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="CheckAll" runat="server" onclick="javascript: SelectAllCheckboxes(this);" ToolTip="按一次全選，再按一次取消全選" /> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox id="chk" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </iMES:GridViewExt>
                    <input type="hidden" id="selectlocationname" runat="server" />
                    <input type="hidden" id="selectmodelname" runat="server" />
                    <input type="hidden" id="select_in_out" runat="server" />
                    <input type="hidden" id="select_y_n" runat="server" />
                    
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="serverclick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="serverclick" />
            </Triggers>
        </asp:UpdatePanel> 
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                <tr>
                    <td align="right" width="10%">
                        <asp:Label CssClass="iMes_label_13pt">入/出庫：</asp:Label>
                    </td>
                    <td width="10%">
                        <asp:DropDownList ID="cmbIn_Out" runat="server" Width="100px">
                        <asp:ListItem Text="入庫" Value="in"></asp:ListItem>
                        <asp:ListItem Text="出庫" Value="out"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right" width="10%">
                        <asp:Label CssClass="iMes_label_13pt">是否禁用：</asp:Label>
                    </td>
                    <td width="10%">
                        <asp:DropDownList ID="cmb_Y_N" runat="server" Width="100px">
                        <asp:ListItem Text="准許" Value="N"></asp:ListItem>
                        <asp:ListItem Text="禁止" Value="Y"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="10%">
                        <input id="btnSave" type="button" runat="server" value="Save" onserverclick="btnSave_serverclick" onclick="if(checkSave())" />
                    </td>
                    <td width="50%"></td>
                </tr>                             
            </table> 
        </div>  
   
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
           
        </Triggers>                      
        </asp:UpdatePanel>

           <input type="hidden" id="hidSelectId" runat="server"  />
           <input type="hidden" id="HiddenUserName" runat="server" />
           <input type="hidden" id="dTableHeight" runat="server" />   
           <input type="hidden" id="dOldId" runat="server" />   
    
    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    <script language="javascript" type="text/javascript">
        var ctlSelectedId = document.getElementById("<%=hidSelectId.ClientID%>");

    function SelectAllCheckboxes(spanChk) {
        elm = document.forms[0];
        for (i = 0; i <= elm.length - 1; i++) {
            if (elm[i].type == "checkbox" && elm[i].id != spanChk.id) {
                if (elm.elements[i].checked != spanChk.checked)
                    elm.elements[i].click();
            }
        }
    }
    function checkQuery() {
        document.getElementById("<%=selectlocationname.ClientID %>").value = document.getElementById("<%=cmbLocation.ClientID %>").value;
        document.getElementById("<%=selectmodelname.ClientID %>").value = document.getElementById("<%=cmbModel.ClientID %>").value;
        ctlSelectedId.value = "";
    }
    function checkSave() {
        document.getElementById("<%=select_in_out.ClientID %>").value = document.getElementById("<%=cmbIn_Out.ClientID %>").value;
        document.getElementById("<%=select_y_n.ClientID %>").value = document.getElementById("<%=cmb_Y_N.ClientID %>").value;
        if (ctlSelectedId.value == "") {
            alert("請至少勾選一項...");
            return false;
        }
        beginWaitingCoverDiv();
        return true;
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
    
    function clkButton()
    {
       switch(event.srcElement.id)
       {
           case "<%=btnSave.ClientID %>":
                
                if(clkSave()==false)
                {                
                    return false;
                }
 	            break;
    	}   
        ShowWait();
        return true;
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
    
    

       
    window.onload = function()
    {
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
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
    
     function clickTable(con)
    {
         setGdHighLight(con);         
    }
    
   
   
    </script>
</asp:Content>


<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="AssemblyVC.aspx.cs" Inherits="DataMaintain_AssemblyVC" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%--
 ITC-1361-0016
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/DataMaintain/service/WebServiceAssemblyVC.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="iMes_div_MainTainEdit" >            
                <tr >
                    <td style="width: 200px;">
                        <asp:Label ID="lblVCTop" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="32%">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Always">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbVC" runat="server" Width="98%" AutoPostBack="True"></asp:DropDownList>    
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>                                    
                    <td style="width: 80px;">
                        <asp:Label ID="lblCombineVCTop" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="32%">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" >
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbCombineVC" runat="server" Width="98%" AutoPostBack="True"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cmbVC" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" RenderMode="Inline" >
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cmbCombineVC" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>    
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="40%"></td> 
                    <td align="right">
                       <input type="button" id="btnDelete" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnDelete_ServerClick"></input>
                    </td>            
                </tr>
             </table>  
        </div>


        <asp:UpdatePanel ID="updatePanel" runat="server" RenderMode="Inline" Visible="true" >
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
                        <td width="10%">
                            <asp:Label ID="lblVC" runat="server" CssClass="iMes_label_13pt" Width ="98%"></asp:Label>
                        </td>
                        <td width="20%">
                            <input type="text" id="txtVC" runat="server" onblur="getPartNO();ShowWait();" style="width:96%" maxlength="64" />
                        </td>       
                        <td width="10%">
                            <asp:Label ID="lblPartNO" runat="server" CssClass="iMes_label_13pt" Width="98%"></asp:Label>                           
                        </td>                      
                        <td width="20%">
                            <select id="cmbPartNO" style="width:98%" runat="server" ></select>
                        </td>
                        <td width="10%"></td>   
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCombineVC" runat="server" CssClass="iMes_label_13pt" Width ="98%"></asp:Label>
                        </td>
                        <td>
                            <input type="text" id="txtCombineVC" runat="server" onblur="getCombinePartNO();ShowWait();" style="width:96%" maxlength="64" />
                        </td>       
                        <td>
                            <asp:Label ID="lblCombinePartNO" runat="server" CssClass="iMes_label_13pt" Width="98%"></asp:Label>                           
                        </td>                      
                        <td>
                            <select id="cmbCombinePartNO" style="width:98%" runat="server"></select>
                        </td>
                        <td></td>   
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt" Width ="98%"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="cmbFamily" runat="server" Width="98%"></asp:DropDownList>
                        </td>       
                        <td colspan="3"></td>   
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt" Width ="98%"></asp:Label>
                        </td>
                        <td colspan="3">
                            <input type="text" id="txtRemark" runat="server" style="width:99%" maxlength="255" />
                        </td>
                        <td  align="right">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></input>
                        </td>
                    </tr>                    
                      
            </table> 
        </div>  
   
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
            <ContentTemplate>
            </ContentTemplate> 
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
            </Triggers>                      
        </asp:UpdatePanel>
         
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />   
        <input type="hidden" id="dOldId" runat="server" />   
        <input type="hidden" id="hidPartNO" runat="server" />
        <input type="hidden" id="hidCombinePartNO" runat="server" />
        <input type="hidden" id="hidAddInsertPartNO" runat="server" />
        <input type="hidden" id="hidAddInsertCombinePartNO" runat="server" />
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
    
    function clkButton()
    {
       switch(event.srcElement.id)
       {
            case "<%=btnSave.ClientID %>":
                
                if(!clkSave())
                {                
                    return false;
                }
                break;
 	            
            case "<%=btnDelete.ClientID %>":
           
                if(!clkDelete())
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

    function getVCCmbObj() {
        return document.getElementById("<%=cmbVC.ClientID %>");
    }

    function getCombineVCCmbObj() {
        return document.getElementById("<%=cmbCombineVC.ClientID %>");
    }

    function getFamilyCmbObj() {
        return document.getElementById("<%=cmbFamily.ClientID %>");
    }

    function getPartNOCmbObj() {
        return document.getElementById("<%=cmbPartNO.ClientID %>");
    }

    function getCombinePartNOCmbObj() {
        return document.getElementById("<%=cmbCombinePartNO.ClientID %>");
    }

    function getPartNO() {
        WebServiceAssemblyVC.GetPartNoList(document.getElementById("<%=txtVC.ClientID %>").value, onPartSucceeded, onPartFailed);
    }

    function onPartSucceeded(result) {
        try {
            DealHideWait();
            if (result == null) {
                endWaitingCoverDiv();
                alert(msgSystemError);
            }
            else if ((result.length == 2) && (result[0] == "SUCCESSRET")) {
                var o = null;
                var item = result[1];
                getPartNOCmbObj().length = 0;
                getPartNOCmbObj().options.add(firstoption());
                for (var i in item) {
                    o = document.createElement('option');
                    o.text = item[i];
                    o.value = item[i].split('-')[0].trim();
                    getPartNOCmbObj().add(o);
                }
                getPartNOCmbObj().value = document.getElementById("<%=hidPartNO.ClientID %>").value;
            }
            else {
                DealHideWait();
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
        }
        catch (e) {
            DealHideWait();
            alert(e.description);
        }
    }

    function onPartFailed(error) {
        try {
            endWaitingCoverDiv();
            getPartNOCmbObj().options.add(firstoption());
            ClearData();
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            getAvailableData("processDataEntry");
        }
        catch (e) {
            endWaitingCoverDiv();
            alert(e.description);
        }
    }

    function getCombinePartNO() {
        WebServiceAssemblyVC.GetPartNoList(document.getElementById("<%=txtCombineVC.ClientID %>").value, onCombinePartSucceeded, onCombinePartFailed);
    }

    function onCombinePartSucceeded(result) {
        try {
            DealHideWait();
            if (result == null) {
                endWaitingCoverDiv();
                alert(msgSystemError);
            }
            else if ((result.length == 2) && (result[0] == "SUCCESSRET")) {
                var o = null;
                var cmbtest = document.getElementById('cmbCombinePartNO');
                var item = result[1];
                getCombinePartNOCmbObj().length = 0;
                getCombinePartNOCmbObj().options.add(firstoption());
                for (var i in item) {
                    o = document.createElement('option');
                    o.text = item[i];
                    o.value = item[i].split('-')[0].trim();
                    getCombinePartNOCmbObj().add(o);
                }
                getCombinePartNOCmbObj().value = document.getElementById("<%=hidCombinePartNO.ClientID %>").value;
                
            }
            else {
                DealHideWait();
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
        }
        catch (e) {
            DealHideWait();
            alert(e.description);
        }
    }

    function onCombinePartFailed(error) {
        try {
            DealHideWait();
            getCombinePartNOCmbObj().options.add(firstoption());
            ClearData();
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            getAvailableData("processDataEntry");
        }
        catch (e) {
            DealHideWait();
            alert(e.description);
        }
    }

    function firstoption() {
        var o = document.createElement('option');
        o.text = '';
        o.value = "";
        return o;
    }

    window.onload = function() {
        msg1 = "<%=pmtMessage1%>"; //请选择一条记录
        msg2 = "<%=pmtMessage2%>"; //确定要删除这条记录么？
        msg3 = "<%=pmtMessage3%>"; //请选择[Part NO]
        msg4 = "<%=pmtMessage4%>"; //请选择[Combine Part NO]
        msg5 = "<%=pmtMessage5%>"; //请输入[VC]
        msg6 = "<%=pmtMessage6%>"; //请选择[Family]
        msg7 = "<%=pmtMessage7%>"; //请输入[Combine VC]
        document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";
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
    
    function clkDelete()
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>")
        var curIndex = gdObj.index;
        var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;            
        if(curIndex>=recordCount)
        {
            alert(msg1);
            return false;
        }          
        var ret = confirm(msg2);
        if (!ret) {
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
        if (document.getElementById("<%=txtVC.ClientID %>").value == "") {
            alert(msg5);
            return false;   
        }
        if (document.getElementById("<%=txtCombineVC.ClientID %>").value == "") {
            alert(msg7);
            return false;
        }
        document.getElementById("<%=hidAddInsertPartNO.ClientID %>").value = getPartNOCmbObj().value;
        document.getElementById("<%=hidAddInsertCombinePartNO.ClientID %>").value = getCombinePartNOCmbObj().value;
        return true;
    }

    function clickTable(con) {
        setGdHighLight(con);         
        ShowRowEditInfo(con);
    }
    
    function setNewItemValue() {
        //ShowWait();
        document.getElementById("<%=txtVC.ClientID %>").value = "";
        document.getElementById("<%=txtCombineVC.ClientID %>").value = "";
        getPartNO();
        getCombinePartNO();
        document.getElementById("<%=txtRemark.ClientID %>").value = "";
        document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
    }
    
    function ShowRowEditInfo(con)
    {
        if(con==null)
        {
            setNewItemValue();
            return;
        }
        //ShowWait();
         document.getElementById("<%=txtVC.ClientID %>").value = con.cells[0].innerText.trim();
         document.getElementById("<%=txtCombineVC.ClientID %>").value = con.cells[2].innerText.trim();
         document.getElementById("<%=hidPartNO.ClientID %>").value = con.cells[1].innerText.trim();
         document.getElementById("<%=hidCombinePartNO.ClientID %>").value = con.cells[3].innerText.trim(); 
         getPartNO();
         getCombinePartNO();
         document.getElementById("<%=txtRemark.ClientID %>").value = con.cells[5].innerText.trim();
         getFamilyCmbObj().value = con.cells[4].innerText.trim(); 
         var currentId=con.cells[9].innerText.trim(); 
         document.getElementById("<%=dOldId.ClientID %>").value = currentId;
         if(currentId=="")
         {
            setNewItemValue();
         }
         else
         {
            document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
        }
        
    }
   
    function AddUpdateComplete(id)
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        var selectedRowIndex=-1;
        for (var i = 1; i < gdObj.rows.length; i++) {
            if (gdObj.rows[i].cells[0].innerText == id) {
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
        
    function DealHideWait()
    {
        HideWait();
        getVCCmbObj().disabled = false;
        getCombineVCCmbObj().disabled = false; 
    }

    </script>
</asp:Content>


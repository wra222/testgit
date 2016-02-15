<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="LabelTypeRule.aspx.cs" Inherits="DataMaintain_LabelTypeRule" ValidateRequest="false" %>
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
                <tr >
                    <td style="width: 200px;">
                        <asp:Label ID="lblSubSystem" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="15%">
                        <asp:DropDownList ID="cmbSubSystem" runat="server"></asp:DropDownList>
                    </td>                                    
                    <td style="width: 80px;">
                        <asp:Label ID="lblUI" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="15%">
                    <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                        <ContentTemplate>
                            <asp:DropDownList ID="cmbUI" runat="server" ></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                        
                    </td>
                    <td width="50%"></td>
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="40%"></td> 
                    <td align="right">
                        <input type="button" id="btnModelInfo" runat="server" onclick="if(clkWindowDialog())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnRefash_ServerClick" />
                        <input type="button" id="btnDeliveryInfo" runat="server" onclick="if(clkWindowDialog())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnRefash_ServerClick" />
                        <input type="button" id="btnPartInfo" runat="server" onclick="if(clkWindowDialog())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnRefash_ServerClick" />
                        <input type="button" id="btnDelete" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnDelete_ServerClick" />
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
        <div id="div3" class="iMes_div_MainTainEdit">
            <table width="100%">
                    
                    <tr>
                        <td width="5%">
                            <asp:Label ID="lblLabelType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="20%" colspan="5">
                            <asp:Label ID="lblLabelTypeName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="5%"></td>
                        <td width="10%"></td>   
                        <td width="10%"></td>   
                    </tr>
                    <tr>
                        <td width="5%">
                            <asp:Label ID="lblStation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="20%" colspan="5">
                            <%--<asp:TextBox ID="txtStation" runat="server" MaxLength="255" Width="98%"></asp:TextBox>--%>
                            <input type="text" id="txtStation" runat="server" maxlength="255" style="width:98%" />
                        </td>
                        <td width="5%"></td>
                        <td width="10%"></td>   
                        <td width="10%"></td>   
                    </tr>
                    <tr>
                        <td width="5%">
                            <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="20%" colspan="5">
                            <input type="text" id="txtFamily" runat="server" maxlength="255" style="width:98%" />
                        </td>
                        <td width="5%"></td>
                        <td width="10%"></td>   
                        <td width="10%"></td>   
                    </tr>
                    <tr>
                        <td width="5%">
                            <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="20%" colspan="5">
                            <input type="text" id="txtModel" runat="server" maxlength="255" style="width:98%" />
                        </td>
                        <td width="5%"></td>
                        <td width="10%"></td>   
                        <td width="10%"></td>   
                    </tr>
                    <tr>
                        <td width="5%">
                            <asp:Label ID="lblBomLevel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="20%">
                            <%--<asp:TextBox ID="txtBomLevel" runat="server" MaxLength="2" Width="94%" Text="-1"  
                            OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}">
                            </asp:TextBox>--%>
                            <input type="text" id="txtBomLevel" runat="server" maxlength="2" style="width:94%" value="-1"
                            onkeypress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" />
                        </td>
                        <td width="10%"></td>
                        <td width="5%">
                            <asp:Label ID="lblBomNodeType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="20%" colspan="2">
                            <input type="text" id="txtBomNodeType" runat="server" maxlength="16" style="width:94%" />
                        </td>
                        
                        <td width="5%"></td>
                        <td width="10%"></td>   
                        <td width="10%"></td>   
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtPartNo" runat="server" maxlength="255" style="width:98%" />
                        </td>
                        <td width="5%"></td>
                        <td width="10%"></td>   
                        <td width="10%"></td>   
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPartDescr" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtPartDescr" runat="server" maxlength="255" style="width:98%" />
                        </td>
                        <td width="5%"></td>
                        <td width="10%"></td>   
                        <td width="10%"></td>   
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPartType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtPartType" runat="server" maxlength="255" style="width:98%" />
                        </td>
                        <td width="5%"></td>
                        <td width="10%"></td>   
                        <td width="10%"></td>   
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt" Width ="98%"></asp:Label>
                        </td>
                        <td  colspan="5">
                            <input type="text" id="txtRemark" runat="server" maxlength="255" style="width:98%" />
                        </td>
                        <td width="5%"></td>
                        <td>
                            <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></input>
                        </td>
                        <td>
                            <input type="button" id="btntestRE" runat="server" onclick="ShowDialog()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" ></input>
                        </td>
                    </tr>
                               
                      
            </table> 
        </div>  
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
            <asp:AsyncPostBackTrigger ControlID="btnSubSystemChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnUIChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnModelInfo" EventName="ServerClick" /> 
            <asp:AsyncPostBackTrigger ControlID="btnDeliveryInfo" EventName="ServerClick" /> 
            <asp:AsyncPostBackTrigger ControlID="btnPartInfo" EventName="ServerClick" /> 
        </Triggers>                      
        </asp:UpdatePanel>
            <input id="hidUI" type="hidden" runat="server" />
            <input id="hidLabelType" type="hidden" runat="server" />
            <input id="hidpcode" type="hidden" runat="server" />
            <input id="hidValueList" type="hidden" runat="server" />
            <input id="hidModelConstValue" type="hidden" runat="server" />
            <input id="hidDeliveryConstValue" type="hidden" runat="server" />
            <input id="hidPartConstValue" type="hidden" runat="server" />
            <input id="hidretvalue" type="hidden" runat="server" />
            <input type="hidden" id="HiddenUserName" runat="server" />
            <input type="hidden" id="dTableHeight" runat="server" />   
            <input type="button" id="btnSubSystemChange" runat="server" style="display:none" onserverclick ="btnSubSystemChange_ServerClick" />
            <input type="button" id="btnUIChange" runat="server" style="display:none" onserverclick ="btnUIChange_ServerClick" />
    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    <script language="javascript" type="text/javascript">

    var msg1 = ""; 
    var msg2="";
    var msg3="";
    var msg4="";
    var msg5="";
    var msg6="";
    var msg7="";
    var msg8="";
    var msg9="";
    var msg10="";
    var BomLevel;
    var editor = '<%=Request["UserId"] %>';
   
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
    
    function clkWindowDialog() {
        switch (event.srcElement.id) {
            case "<%=btnModelInfo.ClientID %>":
                ClkModelInfo();
                document.getElementById("<%=hidModelConstValue.ClientID %>").value = document.getElementById("<%=hidretvalue.ClientID %>").value
                break;
            case "<%=btnDeliveryInfo.ClientID %>":
                ClkDeliveryInfo();
                document.getElementById("<%=hidDeliveryConstValue.ClientID %>").value = document.getElementById("<%=hidretvalue.ClientID %>").value
                break;
            case "<%=btnPartInfo.ClientID %>":
                ClkPartInfo();
                document.getElementById("<%=hidPartConstValue.ClientID %>").value = document.getElementById("<%=hidretvalue.ClientID %>").value
                break;
        }
        ShowWait();
        return true;
    }

    function ClkModelInfo() {
        var saveasUrl = "../DataMaintain/LabelTypeRuleDialog.aspx?AccountId=111&LabelType=Model_" + document.getElementById("<%=hidLabelType.ClientID %>").value + "&UserName=" + document.getElementById("<%=HiddenUserName.ClientID %>").value + "&UserId=" + editor;
        ShowInfoDialog(saveasUrl);
    }

    function ClkDeliveryInfo() {
        var saveasUrl = "../DataMaintain/LabelTypeRuleDialog.aspx?AccountId=111&LabelType=Delivery_" + document.getElementById("<%=hidLabelType.ClientID %>").value + "&UserName=" + document.getElementById("<%=HiddenUserName.ClientID %>").value + "&UserId=" + editor;
        ShowInfoDialog(saveasUrl);
    }

    function ClkPartInfo() {
        var saveasUrl = "../DataMaintain/LabelTypeRuleDialog.aspx?AccountId=111&LabelType=PartInfo_" + document.getElementById("<%=hidLabelType.ClientID %>").value + "&BomLevel=" + BomLevel + "&UserName=" + document.getElementById("<%=HiddenUserName.ClientID %>").value + "&UserId=" + editor;
        ShowInfoDialog(saveasUrl);
    }
    
    function ShowInfoDialog(saveasUrl) {
        var dlgFeature = "dialogHeight:560px;dialogWidth:1000px;center:yes;status:no;help:no;scroll:no";
        var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
        if (dlgReturn) {
            document.getElementById("<%=hidretvalue.ClientID %>").value = dlgReturn
        }
        else {
            if (dlgReturn == "")
            { document.getElementById("<%=hidretvalue.ClientID %>").value = ""; }
        }
        //updatelist
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

    function SubSystemChange(obj) {
        ShowWait();
        document.getElementById("<%=hidUI.ClientID%>").value = obj.value;
        document.getElementById("<%=btnSubSystemChange.ClientID%>").click();
    }

    function UIChange(obj2) {
        ShowWait();
        document.getElementById("<%=hidpcode.ClientID%>").value = obj2.value;
        document.getElementById("<%=btnUIChange.ClientID%>").click();
    }
    
    
       
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5  ="<%=pmtMessage5%>";
        msg10 ="<%=pmtMessage10%>";   
            
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        //initControls();  
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
        try {
            tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
        }
        catch (e) {

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
   
    function check() {

        //check input
        if (document.getElementById("<%=txtBomLevel.ClientID %>").value == "") {
            alert("BomLevel必須要有值...");  
            return false;
        }
        var BomLevelValue = document.getElementById("<%=txtBomLevel.ClientID %>").value;

        if (BomLevelValue <= 0) {
            alert("BomLevel必須要大於0...");
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
        document.getElementById("<%=btnModelInfo.ClientID %>").disabled = true;
        document.getElementById("<%=btnDeliveryInfo.ClientID %>").disabled = true;
        document.getElementById("<%=btnPartInfo.ClientID %>").disabled = true;
        document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        document.getElementById("<%=hidLabelType.ClientID %>").value = "";
        //set null value to input  btnPartInfo
        document.getElementById("<%=lblLabelTypeName.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtStation.ClientID %>").value = "";
        document.getElementById("<%=txtFamily.ClientID %>").value = "";
        document.getElementById("<%=txtModel.ClientID %>").value = "";
        document.getElementById("<%=txtBomLevel.ClientID %>").value = "-1";
        document.getElementById("<%=txtBomNodeType.ClientID %>").value = "";
        document.getElementById("<%=txtPartNo.ClientID %>").value = "";
        document.getElementById("<%=txtPartDescr.ClientID %>").value = "";
        document.getElementById("<%=txtPartType.ClientID %>").value = "";
        document.getElementById("<%=txtRemark.ClientID %>").value = "";

        document.getElementById("<%=hidModelConstValue.ClientID %>").value = "";
        document.getElementById("<%=hidDeliveryConstValue.ClientID %>").value = "";
        document.getElementById("<%=hidPartConstValue.ClientID %>").value = "";
        
    }
    
    function ShowRowEditInfo(con)
    {
        if(con==null)
        {
            setNewItemValue();
            return;
        }
        //set table value to input
        var currentLabelType = con.cells[0].innerText.trim();
        if (currentLabelType == "") {
            setNewItemValue();
        }
        else {
            document.getElementById("<%=hidLabelType.ClientID %>").value = currentLabelType;
            document.getElementById("<%=btnModelInfo.ClientID %>").disabled = false;
            document.getElementById("<%=btnDeliveryInfo.ClientID %>").disabled = false;
            BomLevel = con.cells[6].innerText.trim();
            if (BomLevel == "-1") {
                document.getElementById("<%=btnPartInfo.ClientID %>").disabled = true;
            }
            else {
                document.getElementById("<%=btnPartInfo.ClientID %>").disabled = false;
            }
            document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
            document.getElementById("<%=btnSave.ClientID %>").disabled = false;
            document.getElementById("<%=lblLabelTypeName.ClientID %>").innerHTML = con.cells[0].innerText.trim();
            document.getElementById("<%=txtStation.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=txtFamily.ClientID %>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=txtModel.ClientID %>").value = con.cells[3].innerText.trim();
            document.getElementById("<%=txtBomLevel.ClientID %>").value = con.cells[6].innerText.trim();
            document.getElementById("<%=txtBomNodeType.ClientID %>").value = con.cells[8].innerText.trim();
            document.getElementById("<%=txtPartNo.ClientID %>").value = con.cells[7].innerText.trim();
            document.getElementById("<%=txtPartDescr.ClientID %>").value = con.cells[9].innerText.trim();
            document.getElementById("<%=txtPartType.ClientID %>").value = con.cells[10].innerText.trim();
            document.getElementById("<%=txtRemark.ClientID %>").value = con.cells[12].innerText.trim();

            document.getElementById("<%=hidModelConstValue.ClientID %>").value = con.cells[4].innerText.trim();
            document.getElementById("<%=hidDeliveryConstValue.ClientID %>").value = con.cells[5].innerText.trim();
            document.getElementById("<%=hidPartConstValue.ClientID %>").value = con.cells[11].innerText.trim();
      
        }
    }

    function ChengeDrp(obj,value) {
        var ddl = obj;
        var opts = ddl.options.length;
        for (var i = 0; i < opts; i++) {
            if (ddl.options[i].value == value) {
                ddl.options[i].selected = true;
                break;
            }
        }
    }
   
    function AddUpdateComplete(LabelType)
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        var selectedRowIndex=-1;
        for (var i = 1; i < gdObj.rows.length; i++) {
            if (gdObj.rows[i].cells[0].innerText == LabelType) {
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
    }

    function ShowDialog() {
        var ret = window.showModalDialog("CheckItemTypeRuleforTestREDialog.aspx?AccountId=111"+ "&UserId=" + editor, 0, "dialogwidth:1000px; dialogheight:200px;status:no;help:no; ");
    }

    </script>
</asp:Content>


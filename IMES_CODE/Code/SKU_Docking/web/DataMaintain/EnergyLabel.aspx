
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="EnergyLabel.aspx.cs" Inherits="DataMaintain_EnergyLabel" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="iMes_div_MainTainEdit" >             
                <tr >
                    <td style="width: 150px;">
                        <asp:Label ID="lblEnergyLabelFamilyListTop" runat="server" CssClass="iMes_label_13pt">Family:</asp:Label>
                    </td>
                    <td width="15%" align ="left" >
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                        <ContentTemplate>                    
                            <asp:DropDownList ID="cmbEnergyLabelFamilyListTop" runat="server" Width="98%" AutoPostBack="True"></asp:DropDownList>
                        </ContentTemplate>
                        </asp:UpdatePanel>                            
                    </td>
                    <td width="70%" align="left">
                        <%--<asp:Label ID="lblDesInfo" runat="server" CssClass="iMes_label_13pt" ></asp:Label>--%>
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
                    </iMES:GridViewExt>
            </div>
        </ContentTemplate>
        </asp:UpdatePanel> 
        
        
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                <tr>    
                    <td style="width: 80px;">
                        <asp:Label ID="lblEnergyLabelFamilyList" runat="server" CssClass="iMes_label_13pt">Family:</asp:Label>
                    </td>
                    <td  width="30%">
                        <asp:DropDownList ID="cmbEnergyLabelFamilyList" runat="server" Width="98%" ></asp:DropDownList>
                    </td> 
                    <td style="width: 80px;">
                        <asp:Label ID="lblSeriesName" runat="server" CssClass="iMes_label_13pt">SeriesName:</asp:Label>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="txtSeriesName" runat="server"   MaxLength="64" Width="98%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td></td>           
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblLevel" runat="server" CssClass="iMes_label_13pt">Level:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLevel" runat="server"   MaxLength="2" Width="98%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblChinaLevel" runat="server" CssClass="iMes_label_13pt">ChinaLevel:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbChinaLevel" runat="server" Width="98%" ></asp:DropDownList>
                    </td>
                    <td align="right">
                        <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></input>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSaveEnergy" runat="server" CssClass="iMes_label_13pt">SaveEnergy:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSaveEnergy" runat="server"   MaxLength="16" Width="98%" SkinId="textBoxSkin" 
                        onkeyup="return ValidateFloat(this,value)"></asp:TextBox>
                    </td>
                    <td colspan="3"></td>
                </tr>                                                            
            </table> 
        </div>  
   
        <asp:UpdatePanel ID="updatePanel3" runat="server" RenderMode="Inline" Visible="false" ></asp:UpdatePanel>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false" > 
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
                <asp:AsyncPostBackTrigger ControlID="btnConstValueTypeChange" EventName="ServerClick" />
            </Triggers>         
        </asp:UpdatePanel>         
        <input type="hidden" id="HiddenUserName" runat="server" />
        <%--<input type="hidden" id="dOldCode" runat="server" />--%>
        <input type="hidden" id="dTableHeight" runat="server" />   
        <input type="hidden" id="selecttype" runat="server" />
        <input type="hidden" id="hidID" runat="server" />
        <input type="hidden" id="hidTbUser" runat="server" />
        <button id="btnConstValueTypeChange" runat="server" type="button" style="display:none" onserverclick ="cmbEnergyLabelFamilyListTop_Load"> </button>           
        <button id="btnTypeListUpdate" runat="server" type="button" style="display:none" onserverclick ="btnTypeListUpdate_ServerClick"> </button>           
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
    var msg6 = "";
    var msg7 = "";
    var msg8 = "";
    var msg9 = "";

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

    function ValidateFloat(e, pnumber) {
        if (!/^\d+[.]?\d*$/.test(pnumber)) {
            var newValue = /^\d+[.]?\d*/.exec(e.value);
            if (newValue != null) {
                e.value = newValue;
            }
            else {
                e.value = "";
            }
        }
        return false;
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

    function getEnergyLabelFamilyListTopCmbObj() {
        return document.getElementById("<%=cmbEnergyLabelFamilyListTop.ClientID%>");
    }

    function getEnergyLabelFamilyListCmbObj() {
        return document.getElementById("<%=cmbEnergyLabelFamilyList.ClientID%>");
    }

    function initControls() {
        getEnergyLabelFamilyListTopCmbObj().onchange = EnergyLabelFamilyListTopSelectOnChange;
    }

    function EnergyLabelFamilyListTopSelectOnChange() {
        getEnergyLabelFamilyListCmbObj().value = getEnergyLabelFamilyListTopCmbObj().value;
        document.getElementById("<%=btnConstValueTypeChange.ClientID%>").click();
    }
    
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5  ="<%=pmtMessage5%>";
        msg6 = "<%=pmtMessage6%>";
        msg7 = "<%=pmtMessage7%>";
        msg8 = "<%=pmtMessage8%>";
        msg9 = "<%=pmtMessage9%>";

        //document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
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
       //ShowInfo("");
       return check();        
   }
   
   function check() {

       var seriesName = document.getElementById("<%=txtSeriesName.ClientID %>").value.trim();
       var level = document.getElementById("<%=txtLevel.ClientID %>").value.trim();
       var saveEnergy = document.getElementById("<%=txtSaveEnergy.ClientID %>").value.trim();
       var energyLabelFamilyList = document.getElementById("<%=cmbEnergyLabelFamilyList.ClientID %>").options[document.getElementById("<%=cmbEnergyLabelFamilyList.ClientID %>").selectedIndex].value;
       var chinaLevel = document.getElementById("<%=cmbChinaLevel.ClientID %>").options[document.getElementById("<%=cmbChinaLevel.ClientID %>").selectedIndex].value;

       if (seriesName == "")
        {
            alert(msg1);
            return false;
        }
        
        if (level == "") {
            alert(msg7);
            return false;
        }

        if (saveEnergy == "") {
            alert(msg8);
            return false;
        }

        if (energyLabelFamilyList == "") {
            alert(msg9);
            return false;
        }

        if (chinaLevel == "") {
            alert(msg5);
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
        
        document.getElementById("<%=txtSeriesName.ClientID %>").value = "";
        document.getElementById("<%=txtLevel.ClientID %>").value = "";
        document.getElementById("<%=cmbChinaLevel.ClientID %>").selectedIndex = 0;
        document.getElementById("<%=txtSaveEnergy.ClientID %>").value = "";
        document.getElementById("<%=hidTbUser.ClientID %>").value = "";
        document.getElementById("<%=btnDelete.ClientID %>").disabled = true; 
    }
    
    function ShowRowEditInfo(con)
    {
        if(con == null)
        {
            setNewItemValue();
            return;    
        }

        var currentId = con.cells[7].innerText.trim(); 

        if(currentId == "")
        {
            setNewItemValue();
            return;
        }
        else {
            document.getElementById("<%=cmbEnergyLabelFamilyList.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=txtSeriesName.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=txtLevel.ClientID %>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=cmbChinaLevel.ClientID %>").value = con.cells[3].innerText.trim();
            document.getElementById("<%=txtSaveEnergy.ClientID %>").value = con.cells[4].innerText.trim();
            document.getElementById("<%=hidTbUser.ClientID %>").value = con.cells[5].innerText.trim();
            document.getElementById("<%=hidID.ClientID %>").value = con.cells[8].innerText.trim();
            document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
        }         
    }

    function AddUpdateComplete(family)
    {
        var gdObj = document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex = -1;
        for(var i = 1; i < gdObj.rows.length; i++)
        {
            if (gdObj.rows[i].cells[0].innerText == family)
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


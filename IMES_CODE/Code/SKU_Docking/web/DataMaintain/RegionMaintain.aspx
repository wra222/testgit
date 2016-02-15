<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for Const Value Maintain Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1              
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-8-6     Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/
--%>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="RegionMaintain.aspx.cs" Inherits="DataMaintain_Region" ValidateRequest="false" %>
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
                        <asp:Label ID="lblcustomerTop" runat="server" CssClass="iMes_label_13pt" Text="Customer:"></asp:Label>
                    </td>
                    <td width="15%" align ="left" >
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                        <ContentTemplate>                    
                            <iMESMaintain:CmbCustomer runat="server" ID="cmbCustomerTop" Width="100%" ></iMESMaintain:CmbCustomer>
                        </ContentTemplate>
                        </asp:UpdatePanel>                            
                    </td>
                    <td width="70%" align="left">
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
                        <asp:Label ID="lblCustomer" runat="server" CssClass="iMes_label_13pt" Text="Customer:"></asp:Label>
                    </td>
                    <td  width="25%">
                        <asp:DropDownList ID="cmbCustomer" runat="server" Width="100%"></asp:DropDownList>
                    </td> 
                    <td style="width: 80px;">
                        <asp:Label ID="lblRegion" runat="server" CssClass="iMes_label_13pt" Text="Region:"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="txtRegion" runat="server"   MaxLength="50" Width="99%" SkinId="textBoxSkin" 
                        onkeypress="if ((event.keyCode < 48 || event.keyCode >57)&&(event.keyCode < 65 || event.keyCode >90)) event.returnValue = false;"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>    
                <tr>
                    <td>
                        <asp:Label ID="lblRegionCode" runat="server" CssClass="iMes_label_13pt" Text="RegionCode:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRegionCode" runat="server"   MaxLength="50" Width="99%" SkinId="textBoxSkin" 
                        onkeypress="if ((event.keyCode < 48 || event.keyCode >57)&&(event.keyCode < 65 || event.keyCode >90)) event.returnValue = false;"></asp:TextBox>
                    </td> 
                    <td>
                        <asp:Label ID="lblDescr" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescr" runat="server"   MaxLength="128"  Width="98%"  SkinId="textBoxSkin"></asp:TextBox>
                    </td>                      
                    <td align="right">
                       <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></input>
                    </td>
                </tr>                                                        
            </table> 
        </div>  
   
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false" > 
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
                <asp:AsyncPostBackTrigger ControlID="btnRegionChange" EventName="ServerClick" />
            </Triggers>         
        </asp:UpdatePanel>         
        <input type="hidden" id="HiddenUserName" runat="server" />
        <%--<input type="hidden" id="dOldCode" runat="server" />--%>
        <input type="hidden" id="dTableHeight" runat="server" />   
        <input type="hidden" id="selecttype" runat="server" />
        <button id="btnRegionChange" runat="server" type="button" style="display:none" onserverclick ="btnRegionChange_ServerClick"> </button>           
        <button id="btnRegionTopListUpdate" runat="server" type="button" style="display:none" onserverclick ="btnRegionTopListUpdate_ServerClick"> </button>           
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
        
    function initControls()
    {
        getCustomerCmbObj().onchange = RegionSelectOnChange;
    }

    function RegionSelectOnChange() {
        document.getElementById("<%=btnRegionChange.ClientID%>").click();
        ShowWait();
    }
    
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5  ="<%=pmtMessage5%>";
        msg6  ="<%=pmtMessage6%>";
        initControls();  
        ShowRowEditInfo(null);
        resetTableHeight();
    };

    function resetTableHeight()
    {    
        var adjustValue = 55;     
        var marginValue = 12; 
        var tableHeigth = 300;
        
        try{
            tableHeigth = document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
        }
        catch(e){        
        }        
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
   
   function check() {
        var customer = document.getElementById("<%=cmbCustomer.ClientID %>")[document.getElementById("<%=cmbCustomer.ClientID %>").selectedIndex].text;
        var region = document.getElementById("<%=txtRegion.ClientID %>").value.trim();
        var regionCode = document.getElementById("<%=txtRegionCode.ClientID %>").value.trim();
        if (customer == "")
        {
            alert("Please Select Customer");    //"请选择[Type]"
            return false;    
        }

        if (region == "")
        {
            alert("Please Input Region");    //"请输入[Name]"
            return false;
        }

        if (regionCode == "") {
            alert("Please Input RegionCode");    //"请输入[Name]"
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
        document.getElementById("<%=cmbCustomer.ClientID %>").value = "";
        document.getElementById("<%=txtRegion.ClientID %>").value = "";
        document.getElementById("<%=txtRegionCode.ClientID %>").value = "";     
        document.getElementById("<%=txtDescr.ClientID %>").value = "";  
        document.getElementById("<%=btnDelete.ClientID %>").disabled = true; 
    }
    
    function ShowRowEditInfo(con)
    {
         if(con == null)
         {
            setNewItemValue();
            return;    
         }

         document.getElementById("<%=cmbCustomer.ClientID %>").value = con.cells[0].innerText.trim();
         document.getElementById("<%=txtRegion.ClientID %>").value = con.cells[1].innerText.trim();
         document.getElementById("<%=txtRegionCode.ClientID %>").value = con.cells[2].innerText.trim();
         document.getElementById("<%=txtDescr.ClientID %>").value = con.cells[3].innerText.trim();
         var currentId = con.cells[0].innerText.trim(); 
  
         if(currentId == "")
         {
            setNewItemValue();
            return;
         }
         else
         {
            document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
         }         
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


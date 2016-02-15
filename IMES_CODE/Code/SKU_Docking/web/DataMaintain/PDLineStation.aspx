<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: MB Label Print(SA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-04-12  liu jing-ke        Create 
 2010-04-01 liu,jingke          fix bug ITC-1136-0155
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PDLineStation.aspx.cs" Inherits="DataMaintain_PDLineStation" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="iMes_div_MainTainEdit" >      
                <tr>                                    
                    <td style="width: 200px;">
                        <asp:Label ID="lblCustomer" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left" width="25%">
                        <iMESMaintain:CmbCustomer runat="server" ID="cmbCustomer" Width="120px"></iMESMaintain:CmbCustomer>
                    </td>    
                    <td style="width: 200px;">
                        <asp:Label ID="lblStage" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left" width="25%">
                        <iMESMaintain:CmbMaintainLineStage runat="server" ID="cmbStage" Width="120px"></iMESMaintain:CmbMaintainLineStage>
                    </td>     
                    <td style="width: 200px;">
                        <asp:Label ID="lblLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left" width="25%">
                        <iMESMaintain:CmbMaintainLine runat="server" ID="CmbMaintainLine" Width="120px"></iMESMaintain:CmbMaintainLine>
                    </td>                            
                </tr>
             </table>                 
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblLineStationList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>  
                    <td width="40%" align="right">
                        <input type="button" id="btnDelete" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnDelete_ServerClick"></input>    
                    </td>
                </tr>
             </table>  
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" ChildrenAsTriggers="false">
        <ContentTemplate>
        <div id="div2" style="height:400px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="125%" RowStyle-Height="20"
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel> 
       
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>
                        <td align="left" width="33%">
                            <asp:Label ID="lblStation" runat="server" Width="60px" CssClass="iMes_label_13pt"></asp:Label>
                            <iMESMaintain:CmbMaintainStationPDLineStation runat="server" ID="cmbStation" Width="120px"></iMESMaintain:CmbMaintainStationPDLineStation>                                                                                                           
                        </td>
                        <td  align="left" width="33%">
                            <asp:Label ID="lblStatus" runat="server" Width="60px" CssClass="iMes_label_13pt"></asp:Label>
                            <asp:DropDownList ID="DropDownList_MyStatus" Width="120px" runat="server" />
                        </td>     
                        <td align="right">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkButton())"  class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></input>
                                              
                            <input type="hidden" id="ID" runat="server" />
                            <input type="hidden" id="oldStation" runat="server" />            
                        </td>           
                    </tr> 
            </table> 
        </div> 
         
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnChange" EventName="ServerClick" />  
            <asp:AsyncPostBackTrigger ControlID="btnStageChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnCustomerChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnLineChange" EventName="ServerClick" />
                                          
        </Triggers>
        </asp:UpdatePanel>  
           
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" /> 
        <input type="button" id="btnChange" runat="server" style="display:none" onserverclick ="cmbCustomer_Selected"> </input>           
        <input type="button" id="btnStageChange" runat="server" style="display:none" onserverclick ="btnStageChange_ServerClick"> </input>
        <input type="button" id="btnCustomerChange" runat="server" style="display:none" onserverclick ="btnCustomerChange_ServerClick"> </input>
        <input type="button" id="btnLineChange" runat="server" style="display:none" onserverclick ="btnLineChange_ServerClick"> </input>
    </div>
    
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    <script language="javascript" type="text/javascript">
    
    window.onload = function()
    {
        msg2 ="<%=pmtMessage2%>";        

        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        ShowRowEditInfo(null);

        document.getElementById("<%=btnChange.ClientID %>").click();               
    
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

    function initControls() {
        getCustomerCmbObj().onchange = CustomerSelectOnChange;
        getMaintainLineStageCmbObj().onchange = MaintainStageSelectOnChange;
        getMaintainLineCmbObj().onchange = MaintainLineSelectOnChange;
    }
    
    function CustomerSelectOnChange() {
        document.getElementById("<%=btnCustomerChange.ClientID%>").click();
        ShowWait();
    }
    function MaintainStageSelectOnChange() {
        document.getElementById("<%=btnStageChange.ClientID%>").click();
        ShowWait();
    }
    function MaintainLineSelectOnChange() {
        document.getElementById("<%=btnStageChange.ClientID%>").click();
        ShowWait();
    }
    
    function clkDelete() {
         var gdObj = document.getElementById("<%=gd.ClientID %>")
         var curIndex = gdObj.index;
         var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
         if (curIndex >= recordCount) {
             alert(msg1);
             //            alert("需要先选择一条记录");
             return false;
         }

         //        var ret = confirm("确定要删除这条记录么？");
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

    function clickTable(con) {
       setGdHighLight(con);
       ShowRowEditInfo(con);

    }
    var iSelectedRowIndex = null;
    function setGdHighLight(con) {
       if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
           setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
       }
       setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
       iSelectedRowIndex = parseInt(con.index, 10);
    }
    
    var msg2="";
    
    function ShowRowEditInfo(con)
    {
         if(con==null)
         {                                
            document.getElementById("<%=ID.ClientID %>").value = "";
            document.getElementById("<%=oldStation.ClientID %>").value = "";                  
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;  
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
            document.getElementById("<%=DropDownList_MyStatus.ClientID %>").SelectedIndex=1;   
            getMaintainStationCmbObj().SelectedIndex=0;                     
            return;    
         }
           
         document.getElementById("<%=ID.ClientID %>").value = con.cells[0].innerText.trim();
         document.getElementById("<%=oldStation.ClientID %>").value = con.cells[1].innerText.trim();         
         if(con.cells[0].innerText.trim()=="")
         {
             document.getElementById("<%=DropDownList_MyStatus.ClientID %>").value = "Normal";      
            getMaintainStationCmbObj().SelectedIndex=0;               
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;
         }
         else
         {
            document.getElementById("<%=DropDownList_MyStatus.ClientID %>").value=con.cells[3].innerText.trim();   
            getMaintainStationCmbObj().value=con.cells[1].innerText.trim();                    
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
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

    function clkButton() {
        switch (event.srcElement.id) {
            case "<%=btnSave.ClientID %>":               
                break;

            case "<%=btnDelete.ClientID %>":

                if (clkDelete() == false) {
                    return false;
                }
                break;
        }
        ShowWait();
        return true;
    }
    
    function DealHideWait()
    {
        HideWait();   
        //getMaintainStationCmbObj().disabled = false; 
       // getMaintainLineStageCmbObj().disabled = false; 

    }
    </script>
</asp:Content>


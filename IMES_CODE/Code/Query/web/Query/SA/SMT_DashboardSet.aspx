<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="SMT_DashboardSet.aspx.cs" Inherits="Query_SA_SMT_DashboardSet" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div style=" visibility:hidden">
      <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
       <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
      </div>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="iMes_div_MainTainEdit" >             
                <tr >
                    <td style="width: 100px;">
                        <asp:Label ID="lbltopName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="25%" align ="left" >
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Always" RenderMode="Inline" Visible="true" >
                        <ContentTemplate>
                            <iMES:CmdMaintainSMTDashboardLine runat="server" ID="cmbSMTMantainTableLine"  Width="100%"></iMES:CmdMaintainSMTDashboardLine>                  
                        </ContentTemplate>
                        </asp:UpdatePanel>                            
                    </td>   
                    <td style="width: 100px;"> 
                        <asp:Label ID="lbltopRefreshTime" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    
                    </td>
                    <td width="25%" align ="left">
                        <%--<asp:DropDownList ID="dRefreshTime" runat="server"  Width="50%" >
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                            </asp:DropDownList>--%>
                            <iMES:CmdMaintainSMTDashboardRefreshTime runat="server" ID="cmdMaintainSMTDashboardRefreshTime"  Width="100%"></iMES:CmdMaintainSMTDashboardRefreshTime>                  
                    </td>
                    <td style="width: 100px;"> 
                        <asp:Label ID="lbltopStation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    
                    </td>
                    <td width="25%" align ="left">
                        <iMES:CmdMaintainSMTDashboardStation runat="server" ID="cmdMaintainSMTDashboardStation"  Width="100%"></iMES:CmdMaintainSMTDashboardStation>                  
                    </td>
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="20%" align="right">
                       <input type="button" id="btnDelete" runat="server" class="iMes_button" 
                            onclick="if(clkButton())" style="display:none" ></ input>
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
            <input type="hidden" id="HidValueList" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel> 
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>    
                        <td style="width: 80px;" align="center">
                            <asp:Label ID="lblDurTime" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td  width="35%">
                            <asp:DropDownList ID="dDurTime" runat="server"   MaxLength="255"  Width="98%"  >
                                <asp:ListItem>08:00--10:00</asp:ListItem>
                                <asp:ListItem>10:00--12:00</asp:ListItem>
                                <asp:ListItem>12:00--14:00</asp:ListItem>
                                <asp:ListItem>14:00--16:00</asp:ListItem>
                                <asp:ListItem>16:00--18:00</asp:ListItem>
                                <asp:ListItem>18:00--20:30</asp:ListItem>
                                <asp:ListItem>20:30--22:00</asp:ListItem>
                                <asp:ListItem>22:00--00:00</asp:ListItem>
                                <asp:ListItem>00:00--02:00</asp:ListItem>
                                <asp:ListItem>02:00--04:00</asp:ListItem>
                                <asp:ListItem>04:00--06:00</asp:ListItem>
                                <asp:ListItem>06:00--08:00</asp:ListItem>
                            </asp:DropDownList>
                        </td> 
                        <td style="width: 80px;" align="center">
                            <asp:Label ID="lblStandardOutput" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td  width="35%">
                            <asp:TextBox ID="dStandardOutput" runat="server"   MaxLength="255"  Width="98%"  SkinId="textBoxSkin"  onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" ></asp:TextBox>
                        </td>
                        
                    </ tr>
                    <tr>
                        <td></td><td></td><td></td>
                        <td align="right">
                           <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></ input>
                        </td> 
                        
                    </ tr>
            </table> 
        </div>  
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
                <asp:AsyncPostBackTrigger ControlID="btnSysSettingNameChange" EventName="ServerClick" />
                
            </Triggers>                      
        </asp:UpdatePanel>         
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="HidID" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="Hcdt" runat="server" />
        
        <button id="btnSysSettingNameChange" runat="server" type="button" style="display:none" onserverclick ="btnSysSettingNameChange_ServerClick"> </button>                  
    
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
        beginWaitingCoverDiv();
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
        getMaintainLineCmbObj().onchange = SysSettingNameSelectOnChange;
    }

    function SysSettingNameSelectOnChange()
    {
        document.getElementById("<%=btnSysSettingNameChange.ClientID%>").click();
        
        beginWaitingCoverDiv();
        
    }

    
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";
        msg6 ="<%=pmtMessage6%>";
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
//        var durTime=document.getElementById("<%=dDurTime.ClientID %>").value.trim();
//        var refreshTime = document.getElementById("<%=cmdMaintainSMTDashboardRefreshTime.ClientID %>").value.trim();
//        var StandardOutput = document.getElementById("<%=dStandardOutput.ClientID %>").value.trim(); 
//        if (durTime == "") {
//            alert(msg2);
//            return false;
//        } 
//        if (StandardOutput == "") {
//            alert(msg4);
//            return false;
//        }        
        return true;
   }
   
    function clickTable(con)
    {
        setGdHighLight(con);         
        ShowRowEditInfo(con);
    }
    
    function setNewItemValue()
    {
        //document.getElementById("<%=cmdMaintainSMTDashboardRefreshTime.ClientID %>").value = ""
        document.getElementById("<%=dDurTime.ClientID %>").value = "";
        document.getElementById("<%=dStandardOutput.ClientID %>").value = "";
        document.getElementById("<%=btnDelete.ClientID %>").disabled = true; 
    }
    
    function ShowRowEditInfo(con)
    {
         if(con == null)
         {
            setNewItemValue();
            return;    
         }
       
         document.getElementById("<%=dDurTime.ClientID %>").value = con.cells[1].innerText.trim(); 
         document.getElementById("<%=dStandardOutput.ClientID %>").value = con.cells[2].innerText.trim();
        // document.getElementById("<%=cmdMaintainSMTDashboardRefreshTime.ClientID %>").value = con.cells[3].innerText.trim();
         
         document.getElementById("<%=Hcdt.ClientID %>").value = con.cells[4].innerText.trim();
         document.getElementById("<%=HidID.ClientID %>").value = con.cells[6].innerText.trim();
         
         var currentId = con.cells[0].innerText.trim(); 
  
         if(currentId == "")
         {
            setNewItemValue();
            document.getElementById("<%=dDurTime.ClientID %>").disabled=false;
            return;
         }
         else
         {
            document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
            document.getElementById("<%=dDurTime.ClientID %>").disabled=true;
         }         
    }

    function AddUpdateComplete(id)
    {
        var gdObj = document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex = -1;
        for(var i = 1; i < gdObj.rows.length; i++)
        {
           if(gdObj.rows[i].cells[3].innerText == id)
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
        endWaitingCoverDiv();
        initControls();
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
        
        
        function btnDelete_onclick() {

        }

        function btnDelete_onclick() {

        }

    </script>
</asp:Content>
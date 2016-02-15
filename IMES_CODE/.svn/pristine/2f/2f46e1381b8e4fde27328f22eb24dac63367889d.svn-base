<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="MBMaintain.aspx.cs" Inherits="RCTO_MBMaintain" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div>
             <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width:12%"><asp:Label ID="lblFamily1" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td style="width:35%"><iMESMaintain:CmbMBFamily ID="Family1" Width="100" IsPercentage="true" runat="server"/></td>
                    <td>&nbsp;</td>
                </tr>
             </table>                                  
        </div>
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="">
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblSetting" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                                   
                    <td width="32%" align="right">
                    <input type="button" id="btnDel" runat="server" class="iMes_button"  onclick="if(clkDelete())" onserverclick="btnDelete_ServerClick"/>                         
                    </td>    
                </tr>
             </table>                                  
        </div>

        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDel" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
        </Triggers>
       <ContentTemplate>
       </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div2">
              <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <ContentTemplate>
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                        Width="100%" RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="376px" AutoHighlightScrollByValue ="true" 
                        HighLightRowPosition="3"  
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' 
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false" style="top: -365px; left: 24px"  
                        >
                    </iMES:GridViewExt>
                 </ContentTemplate>
               </asp:UpdatePanel>   
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>
                        <td style="width:10%"><asp:Label ID="lblFamily2" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:20%"><iMESMaintain:CmbMBFamily2 ID="Family2" Width="100" IsPercentage="true" runat="server"/></td>
                        <td style="width:10%"><asp:Label ID="lblCode" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:20%"><iMESMaintain:CmbMBCode ID="CmbMBCode" Width="100" IsPercentage="true" runat="server"/></td>         
                        <td style="width:10%"><asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:20%"><iMESMaintain:CmbMBType ID="CmbMBType" Width="100" IsPercentage="true" runat="server"/></td>                        
                        
                    </tr>
                    <tr>
                        <td style="width:10%"><asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:20%" colspan="5">
                            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                                <ContentTemplate>
                                <asp:TextBox ID="txtRemark" MaxLength="100" runat="server" Width="99%" SkinID="textBoxSkin"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td align="right">
                            <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnAdd_ServerClick"/>
                        </td>           
                    </tr>
            </table> 
        </div>  
       
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="hidFamily" runat="server" />
        <input type="hidden" id="hidCode" runat="server" />
        <input type="hidden" id="hidType" runat="server" />
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
    var msg10 = "";

    window.onload = function() {
        msg1 = "<%=pmtMessage1%>";
        msg2 = "<%=pmtMessage2%>";
        msg3 = "<%=pmtMessage3%>";
        msg4 = "<%=pmtMessage4%>";
        msg5 = "<%=pmtMessage5%>";
        msg6 = "<%=pmtMessage6 %>";
        msg7 = "<%=pmtMessage7 %>";
        msg8 = "<%=pmtMessage8 %>";
        msg9 = "<%=pmtMessage9 %>";
        msg10 = "<%=pmtMessage10 %>";
        document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";

        document.getElementById("<%=btnDel.ClientID %>").disabled = true;
        if(getMBFamilyCmbObj().value == "")
            ShowRowEditInfo(null);
        resetTableHeight();

    };
    
    function resetTableHeight()
    {
        //动态调整表格的高度
        var adjustValue=70;     
        var marginValue=10;  
        var tableHeigth=300;

        
        try{
            tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
        }
        catch(e){
            //ignore
        }                
        //为了使表格下面有写空隙
        var extDivHeight=tableHeigth+marginValue;
       
        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
        div2.style.height=extDivHeight+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
    }
    
    function clkDelete()
    {
        ShowInfo("");
        var ret = confirm(msg3);
        if (!ret) {
            return false;
        }
        ShowWait();
        return true;
    }
   
    function DeleteComplete()
    {   
        ShowRowEditInfo(null);
    }
   
    function clkAdd()
    {
        var flag = checkAdaptorInfo();
        return flag;
    }
   
    function checkAdaptorInfo(){
        ShowInfo("");

        var family = getMBFamily2CmbValue();

        if (family == "") {
            alert(msg1);
            setMBFamily2CmbFocus();
            return false;
        }
        var code = getMBCodeCmbValue();
        if(code == "")
        {
            alert(msg2);
            setMBCodeCmbFocus();
            return false;
        }
        var type = getMBTypeCmbValue();
        if(type == "")
        {
            alert(msg4);
            setMBTypeCmbFocus();
            return false;
        }
        
        ShowWait();
        return true;
    }
   
    var iSelectedRowIndex=null; 
    function setGdHighLight(con)
    {
        if((iSelectedRowIndex!=null) && (iSelectedRowIndex!=parseInt(con.index, 10)))
        {
            //去掉过去高亮行
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex,false, "<%=gd.ClientID %>");                
        }     
        //设置当前高亮行   
        setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
        //记住当前高亮行
        iSelectedRowIndex=parseInt(con.index, 10);    
    }

   
    function clickTable(con)
    {
         ShowInfo("");
         var selectedRowIndex = con.index;
         setGdHighLight(con);         
         ShowRowEditInfo(con);
     }

     function disableBtn() {
         document.getElementById("<%=btnDel.ClientID %>").disabled = true;
     }

     function updateFamily2() {
         getMBFamily2CmbObj().value = getMBFamilyCmbObj().value;
     }
    
    function ShowRowEditInfo(con)
    {
        if(con == null)
        {
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            getMBFamily2CmbObj().value = "";
            getMBCodeCmbObj().value = "";
            getMBTypeCmbObj().value = "";
            document.getElementById("<%=txtRemark.ClientID %>").value = "";    
            return;
        }
        
        if(con.cells[0].innerText.trim() == "" || con.cells[1].innerText.trim() == "" || con.cells[2].innerText.trim() == "")
        {
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            getMBFamily2CmbObj().value = "";
            getMBCodeCmbObj().value = "";
            getMBTypeCmbObj().value = "";
            document.getElementById("<%=txtRemark.ClientID %>").value = "";
        }
        else
        {
            document.getElementById("<%=btnDel.ClientID %>").disabled = false;
            getMBFamily2CmbObj().value = con.cells[2].innerText.trim();
            getMBCodeCmbObj().value = con.cells[0].innerText.trim();
            getMBTypeCmbObj().value = con.cells[1].innerText.trim();
            document.getElementById("<%=txtRemark.ClientID %>").value = con.cells[3].innerText.trim();

            document.getElementById("<%=hidFamily.ClientID %>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=hidCode.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=hidType.ClientID %>").value = con.cells[1].innerText.trim();
        }
    }
   
    function AddUpdateComplete()
    {    
        var gdObj = document.getElementById("<%=gd.ClientID %>"); 
        var selectedRowIndex = -1;
        selectedRowIndex = parseInt(document.getElementById("<%=hidRecordCount.ClientID %>").value, 10); 
        if(selectedRowIndex <= 0)
        {
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            return;
        }
        else
        {            
            var con = gdObj.rows[selectedRowIndex];
            //去掉标题行
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
            getMBFamilyCmbObj().value = getMBFamily2CmbObj().value;
        }        
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


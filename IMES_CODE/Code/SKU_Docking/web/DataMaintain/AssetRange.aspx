<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="AssetRange.aspx.cs" Inherits="DataMaintain_AssetRange" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<style type="text/css">

.iMes_div_MainTainEdit
{
    border: thin solid Black; 
    background-color: #99CDFF;
    margin:0 0 20 0;
    
}




    .iMes_textbox_input_Yellow
    {}

    #btnDel
    {
        width: 14px;
    }

</style>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="iMes_div_MainTainEdit">            
                <tr >
                    <%--<td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblAssetRangeList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="ttAssetRange" runat="server"   MaxLength="50"  Width="56%" 
                            CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>
                    </td>--%>
                    <td width="10%">
                        <asp:Label ID="lblCodeTop" runat="server" CssClass="iMes_label_13pt" Text="Code:"></asp:Label>
                    </td>
                    <td width="15%">
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline" Visible="true">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbCodeSelect" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="cmbCode_Selected"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>          
                    <td width="10%">
                        <asp:Label ID="lblCodeInput" runat="server" CssClass="iMes_label_13pt" Text="Code Select:"></asp:Label>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="txtCodeInput" runat="server"   MaxLength="50"  Width="100%" 
                            CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>
                    </td>
                    <td width="18%"></td>                         
                    <td width="32%" align="right">
                        <input type="button" id="btnC" runat="server" class="iMes_button" onserverclick="btnCloseRange_ServerClick" value="CloseRange"/>
                        <input type="button" id="btnDel" runat="server" class="iMes_button"  onclick="if(clkDelete())" onserverclick="btnDelete_ServerClick"/>                         
                    </td>    
                </tr>
             </table>  
                                                  
        </div>

        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRefreshFamilyList" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnC" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnDel" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
        </Triggers>
        <ContentTemplate>
         </ContentTemplate>
        </asp:UpdatePanel> 
        <div id="div2">
              <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <ContentTemplate>
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                        Width="130%" RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="376px" AutoHighlightScrollByValue ="true" 
                        HighLightRowPosition="3"  
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' 
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false"  
                        >
                    </iMES:GridViewExt>
                 </ContentTemplate>
               </asp:UpdatePanel>   
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                    <tr >
                        <td style="width: 110px;">
                            <asp:Label ID="lblCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="180px">
                            <asp:TextBox ID="ttCode" runat="server"   MaxLength="20"   Width="96%" SkinId="textBoxSkin" ></asp:TextBox>
                        </td>
                        
                         <td style="width: 110px;">
                            <asp:Label ID="lblBegin" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="180px">
                            <asp:TextBox ID="ttBegin" runat="server"   MaxLength="20"   Width="96%"  
                               SkinId="textBoxSkin"  ></asp:TextBox>
                        </td>
                        
                        <td style="width: 110px;">
                            <asp:Label ID="lblEnd" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="180px">
                            <asp:TextBox ID="ttEnd" runat="server"   MaxLength="20"   Width="96%" SkinId="textBoxSkin" ></asp:TextBox>
                        </td>
                           
                        <td align="right">
                            <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnAdd_ServerClick"/>
                        </td>           
                    </tr>
                    <tr >
                        <td style="width: 110px;">
                            <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                       
                         <td  width="850" colspan="3">
                            <asp:TextBox ID="ttRemark" SkinId="textBoxSkin" runat="server"   MaxLength="255"  Width="95%"  CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                         </td>
                         <td>
                            <%--<asp:Label ID="lblPrefix" Text="Prefix:" runat="server" CssClass="iMes_label_13pt"></asp:Label>--%>
                         </td>   
                         <td>
                           <%-- <asp:DropDownList ID="cmbPerfix" runat="server">
                            
                            </asp:DropDownList>--%>
                         </td>
                        <td align="right">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"/>
                            <input type="hidden" id="dOldAssemblyId" runat="server" />
                            <input type="hidden" id="dOldAssemblyCdt" runat="server" />
                            <input type="hidden" id="dTableHeight" runat="server" />
                            <input type="hidden"  id="dOldId" runat="server" />
                        </td>           
                    </tr>                    
                    
                    
            </table> 
        </div>  
            
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="HiddenCustomerName" runat="server" />
        <input type="hidden" id="hidCode" runat="server" />
        <input type="hidden" id="hidStatus" runat="server" />
        <input type="hidden" id="hidCodeInput" runat="server" />
        <button id="btnFamilyChange" runat="server" type="button" style="display:none"> </button>
        <button id="btnRefreshFamilyList" runat="server" type="button" onclick="" style="display: none" ></button>
        <button id="btnRefreshCustomerList" runat="server" type="button" onclick="" style="display: none" ></button>
        <button id="btnCodeInput" runat="server" type="button" onserverclick="Code_Input" style="display: none" ></button>
    </div>
       <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
       </div>
    <script language="javascript" type="text/javascript">

    function OnKeyPress(obj) {
        var key = event.keyCode;

        if (key == 13)//enter
        {
            if (event.srcElement.id == "<%=txtCodeInput.ClientID %>") {
                var value = document.getElementById("<%=txtCodeInput.ClientID %>").value.trim().toUpperCase();
                if (value != "") {
                    //findCode(value, true);
                    document.getElementById("<%=hidCodeInput.ClientID %>").value = value;
                    document.getElementById("<%=btnCodeInput.ClientID %>").click();
                }
            }
        }
    }

    function findCode(searchValue, isNeedPromptAlert)
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(searchValue.trim()!="" && gdObj.rows[i].cells[0].innerText.toUpperCase().indexOf(searchValue)==0)
           {
               selectedRowIndex=i;
               break;  
           }        
        }
        
        if(selectedRowIndex<0)
        {
            if(isNeedPromptAlert==true)
            {
//                alert("Cant find that match assembly.");   //1  
                alert(msg1);     
            }
            else if(isNeedPromptAlert==null)
            {
                ShowRowEditInfo(null);                 
            }
            return;
        }
        else
        {            
            var con=gdObj.rows[selectedRowIndex];
            //去掉标题行
//            selectedRowIndex=selectedRowIndex-1;
//            setRowSelectedByIndex_<%=gd.ClientID%>(selectedRowIndex, false, "<%=gd.ClientID%>");
            //            setSrollByIndex(selectedRowIndex, true, "<%=gd.ClientID%>");
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
        }    
    }
   
    //var familyObj;
    var descriptionObj;
    
    var msg1="";
    var msg2="";
    var msg3="";
    var msg4="";
    var msg5="";
    var msg6="";
    var msg7="";
    var msg8="";
    var msg9="";
    
    
    window.onload = function()
    {
 //       customerObj = getCustomerCmbObj();
//        customerObj.onchange = addNew;
        
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";
        msg6="<%=pmtMessage6 %>";
        msg7="<%=pmtMessage7 %>";
        msg8="<%=pmtMessage8 %>";
        msg9="<%=pmtMessage9 %>";
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
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
        //alert(document.getElementById("div_<%=gd.ClientID %>").style.height)
        
        div2.style.height=extDivHeight+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
    }
    
    function clkDelete()
    {
        ShowInfo("");
        var gdObj=document.getElementById("<%=gd.ClientID %>")
        var curIndex = gdObj.index;
        var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
        document.getElementById("<%=hidCode.ClientID %>").value = document.getElementById("<%=ttCode.ClientID %>").value;            
        if(curIndex>=recordCount)
        {
//            alert("Please select one row!");   //2
            alert(msg2);
            return false;
        }
        
//         var ret = confirm("Do you really want to delete this item?");  //3
        var ret = confirm("Do you really want to delete this item?");  //3
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

    function clkSave() 
    {

        if (document.getElementById("<%=hidStatus.ClientID %>").value == "R") {
            return checkAssetRangeInfo();
        }
        else {
            alert("該數據[Status]不為[R],無法更新...");
            return false;
        }
    }

    function clkAdd() 
    {
     var flag=checkAssetRangeInfo();
     return flag;
    }
    function checkAssetRangeInfo(){
        ShowInfo("");
      
        var code = document.getElementById("<%=ttCode.ClientID %>"); 
        var begin = document.getElementById("<%=ttBegin.ClientID %>");  
        var end= document.getElementById("<%=ttEnd.ClientID %>");
        var remark= document.getElementById("<%=ttRemark.ClientID %>");
        document.getElementById("<%=hidCode.ClientID %>").value = document.getElementById("ctl00_iMESContent_ttCode").value;
        if(code.value.trim()=="")
        {
        //           alert("Please input [Family] first!!");  //4
            alert(msg4);
            code.focus();
            return false;
        }
        else if(begin.value.trim()=="")
        {
            alert(msg5);
            begin.focus();
            return false;
        }
        else if(end.value.trim()=="")
        {
            alert(msg6);
            end.focus();
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

         document.getElementById("<%=hidStatus.ClientID %>").value = con.cells[3].innerText.trim();
 //        setRowSelectedByIndex_<%=gd.ClientID%>(con.index, false, "<%=gd.ClientID%>");
         setGdHighLight(con);         
         ShowRowEditInfo(con);
       
    }
    
    function ShowRowEditInfo(con)
    {
//        customerObj = getCustomerCmbObj();
//        customerObj.onchange = addNew;

         if(con==null)
         {
            document.getElementById("<%=ttCode.ClientID %>").value =""; 
            document.getElementById("<%=ttBegin.ClientID %>").value = "";  
            document.getElementById("<%=ttEnd.ClientID %>").value = "";
            document.getElementById("<%=ttRemark.ClientID %>").value = "";
            
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            document.getElementById("<%=btnC.ClientID %>").disabled = true;  
            return;    
         }
    
         var code= con.cells[0].innerText.trim();
         document.getElementById("<%=ttCode.ClientID %>").value =code ;
         document.getElementById("<%=ttBegin.ClientID %>").value = con.cells[1].innerText.trim(); 
         document.getElementById("<%=ttEnd.ClientID %>").value = con.cells[2].innerText.trim();
         document.getElementById("<%=ttRemark.ClientID %>").value = con.cells[4].innerText.trim();
         
         
         //记录下将要被删除的Assembly的值     
//         document.getElementById("<%=dOldAssemblyId.ClientID %>").value = curAssembly;
//          document.getElementById("<%=dOldAssemblyCdt.ClientID %>").value = con.cells[7].innerText.trim();
            document.getElementById("<%=dOldId.ClientID %>").value=con.cells[9].innerText.trim();
         if(code=="")
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            document.getElementById("<%=btnC.ClientID %>").disabled = true;
         }
         else
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled = false;
            if (con.cells[3].innerText.trim() == "R") {
                document.getElementById("<%=btnDel.ClientID %>").disabled = false;
            }
            else {
                document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            }
            if (con.cells[3].innerText.trim() == "A") {
                document.getElementById("<%=btnC.ClientID %>").disabled = false;
            }
            else {
                document.getElementById("<%=btnC.ClientID %>").disabled = true;
            }
         }
//         document.getElementById("<%=ttCode.ClientID %>").focus();
    }
   
   
    function AddUpdateComplete(id)
    {
        
        var gdObj=document.getElementById("<%=gd.ClientID %>");
      
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(gdObj.rows[i].cells[7].innerText==id)
           {
               selectedRowIndex=i;  
           }        
        }
        
        if(selectedRowIndex<0)
        {
           document.getElementById("<%=ttCode.ClientID %>").value=""; 
           document.getElementById("<%=ttBegin.ClientID %>").value="";  
           document.getElementById("<%=ttEnd.ClientID %>").value="";
           document.getElementById("<%=ttRemark.ClientID %>").value="";
        
           document.getElementById("<%=btnSave.ClientID %>").disabled=true;   
           document.getElementById("<%=btnDel.ClientID %>").disabled=true;          
           return;
        }
        else
        {            
            var con=gdObj.rows[selectedRowIndex];
            //去掉标题行
//            selectedRowIndex=selectedRowIndex-1;
//            setRowSelectedByIndex_<%=gd.ClientID%>(selectedRowIndex, false, "<%=gd.ClientID%>");
//            //eval("ChangeCvExtRowByIndex_"+"<%=gd.ClientID%>"+"(RowArray,"+true+", "+selectedRowIndex+")");
//            setSrollByIndex(selectedRowIndex, true, "<%=gd.ClientID%>");
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
            
        }        
        
    }
    
    function NoMatchFamily()
    {
//         alert("Cant find that match family.");    //5 
         alert(msg5);     
         return;   
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


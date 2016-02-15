<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="COAReceiving.aspx.cs" Inherits="DataMaintain_COAReceiving" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div4" class="iMes_div_MainTainDiv1">
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <ContentTemplate>
            <table width="100%">
                <tr >
                    <td style="width:90px;">
                        <asp:Label ID="lblPO" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:180px;">
                       <asp:Label ID="lblPOValue" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td >
                    <td style="width:100px;">
                        <asp:Label ID="lblShippingDate" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:180px;">
                        <asp:Label ID="lblShippingDateVal" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>  
                </tr> 
                <tr >
                    <td style="width:90px;">
                        <asp:Label ID="lblCUSTPN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:180px;">
                       <asp:Label ID="lblCustPNVal" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td >
                    <td style="width:100px;">
                        <asp:Label ID="lblIECPN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:180px;">
                        <asp:Label ID="lblIecPNVal" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>  
                </tr> 
                <tr >
                    <td style="width:90px;">
                        <asp:Label ID="lblDescription" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:180px;">
                       <asp:Label ID="lblDescriptionVal" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td >
                    <td style="width:100px;">
                        
                    </td>
                    <td style="width:180px;">
                        
                    </td>  
                </tr> 
            </table>
            </ContentTemplate>
            </asp:UpdatePanel>
         </div>   
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%">            
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblCOARangeLst" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="8%" align="right">
                        <input type="button" ID="btnUpLoad" runat="server"  class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="clickUpload()"/>
                    </td>
                     <td width="8%" align="right">
                       <input type="button" id="btnSave" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(saveFun())" onserverclick="Save_Click"/>
                    </td>                                    
                    <td width="8%" align="right">
                        <input type="button" id="btnClear" runat="server" class="iMes_button" onclick="if(deleteFun())" onserverclick="Clear_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>                         
                    </td>    
                </tr>
             </table>  
                                                  
        </div>
        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRefreshTableList" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="ServerClick" />
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
                        Width="100%" RowStyle-Height="20" 
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
            <table width="100%">
               <tr>
                        <td align="right">
                            
                            
                           
                            <input type="hidden" id="dTableHeight" runat="server" />
                            
                            <input type="hidden"  id="dOldId" runat="server" />
                        </td>           
                    </tr>                    
                    
            </table> 
        </div>  
            
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="hidFileName" runat="server" />
        <button id="btnFamilyChange" runat="server" type="button" style="display:none"> </button>
        <button id="btnRefreshTableList" runat="server" type="button" onclick="" style="display: none" onserverclick="Show_Click"></button>
        <button id="btnRefreshCustomerList" runat="server" type="button" onclick="" style="display: none" ></button>
        
    </div>
       <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
       </div>
    <script language="javascript" type="text/javascript">
    var customerObj;
    
    var emptyPattern = /^\s*$/;
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
//        ShowRowEditInfo(null);
       
        resetTableHeight();
    };
    
    function saveFun()
    {
        ShowWait();
        return true;
    }
    
   function deleteFun()
   {
         ShowWait();
         return true;
   }
   function showSuccess()
   {
        alert(msg1);
   }
    
   function resetTableHeight()
    {
        //动态调整表格的高度
        var adjustValue=70;     
        var marginValue=10;  
        var tableHeigth=300;

        
        try{
            tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div4.offsetHeight-div3.offsetHeight-adjustValue;
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
    
    function clickUpload() {
     
            
            var dlgFeature = "dialogHeight:200px;dialogWidth:424px;center:yes;status:no;help:no";
            var editor=document.getElementById("<%=HiddenUserName.ClientID %>").value;
             //issuecode
        //ITC-1361-0061   itc210012   2012-02-01
            var dlgReturn = window.showModalDialog("COAReceivingUploadFile.aspx?userName="+editor, window, dlgFeature);
            if (dlgReturn != null) {
                document.getElementById("<%=hidFileName.ClientID %>").value = dlgReturn;
                ShowWait();
                document.getElementById("<%=btnRefreshTableList.ClientID %>").click();
            }
            return;

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
        
//         var selectedRowIndex = con.index;
//         setGdHighLight(con);          
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


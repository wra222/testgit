<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="LightStation.aspx.cs" Inherits="DataMaintain_LightStation" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <style type="text/css">

    </style>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblLightStationLst" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        
                    </td> 
                    <td>
                        <asp:TextBox ID="ttLightStationLst" runat="server"   MaxLength="50"  Width="180px" 
                            CssClass="iMes_textbox_input_Yellow" SkinId="textBoxSkin" onkeypress='OnKeyPress(this)'></asp:TextBox>
                    </td>                                   
                    <td width="32%" align="right">
                    <input type="button" id="btnDel" runat="server" class="iMes_button"  onclick="if(clkDelete())" onserverclick="btnDelete_ServerClick"/>                         
                    </td>    
                </tr>
             </table>  
                                                  
        </div>

        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRefreshFamilyList" EventName="ServerClick" />
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
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false" style="top: -365px; left: 24px"  
                        >
                    </iMES:GridViewExt>
                 </ContentTemplate>
               </asp:UpdatePanel>   
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr >
                        <td style="width: 110px;">
                            <asp:Label ID="lblIECPN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="380px">
                            <asp:TextBox ID="ttIECPN" SkinId="textBoxSkin" runat="server"   MaxLength="16"   Width="92%"  CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>
                        
                         <td style="width: 110px;">
                            <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="380px">
                            <iMESMaintain:CmbMaintainGradeHPFamily ID="ddlFamily"  runat="server" Width="92%"></iMESMaintain:CmbMaintainGradeHPFamily>
                        </td>
 
                        <td align="right">
                            <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnAdd_ServerClick"/>
                        </td>           
                    </tr>
                    <tr >
                        <td style="width: 110px;">
                            <asp:Label ID="lblLightStation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="380px">
                            
                            <iMESMaintain:CmbMaintainLigthStation ID="ddlLightStation"  runat="server" Width="92%"></iMESMaintain:CmbMaintainLigthStation>
                        </td>
                        
                        <td style="width: 110px;">
                        </td>
                        <td width="380px">
 
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
        <button id="btnFamilyChange" runat="server" type="button" style="display:none"> </button>
        <button id="btnRefreshFamilyList" runat="server" type="button" onclick="" style="display: none" ></button>
        <button id="btnRefreshCustomerList" runat="server" type="button" onclick="" style="display: none" ></button>
   
    </div>
       <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
       </div>
    <script language="javascript" type="text/javascript">
    var customerObj;
    
    function OnKeyPress(obj)
    {
        var key = event.keyCode;
        
        if (key==13)//enter
        {     
            if(event.srcElement.id=="<%=ttLightStationLst.ClientID %>")
            {
                var value=document.getElementById("<%=ttLightStationLst.ClientID %>").value.trim().toUpperCase();
                if(value!="")
                {
                    findFamily(value, true);
                }
            }
        }
    }
    
    function findFamily(searchValue, isNeedPromptAlert)
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
//             setSrollByIndex(selectedRowIndex, true, "<%=gd.ClientID%>");
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
        if(curIndex>=recordCount)
        {
//            alert("Please select one row!");   //2
            alert(msg2);
            return false;
        }
        
//         var ret = confirm("Do you really want to delete this item?");  //3
         var ret = confirm(msg3);  //3
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
     return checkAdaptorInfo();  
   }
   
   function clkAdd()
   {
   var flag=checkAdaptorInfo();
     return flag;
   }
   function checkAdaptorInfo(){
          ShowInfo("");
      
         var iecPN=document.getElementById("<%=ttIECPN.ClientID %>");
       if(iecPN.value.trim()=="")
       {
//           alert("Please input [Family] first!!");  //4
           alert(msg4);
           iecPN.focus();
           return false;
       }
       else if(getGradeFamiyCmbObj().value=="")
       {
         alert(msg5);
         getGradeFamiyCmbObj().focus();
         return false;
       }
       else if(getLightStationCmbObj().value=="")
       {
        alert(msg6);
        getLightStationCmbObj().focus();
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
            document.getElementById("<%=ttIECPN.ClientID %>").value =""; 
            
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
            document.getElementById("<%=btnDel.ClientID %>").disabled=true; 
            return;    
         }
        
         var curIECPN= con.cells[0].innerText.trim();
         document.getElementById("<%=ttIECPN.ClientID %>").value =curIECPN ;
         getGradeFamiyCmbObj().value=con.cells[1].innerText.trim();
         getLightStationCmbObj().value=con.cells[2].innerText.trim();
         document.getElementById("<%=dOldId.ClientID %>").value=con.cells[6].innerText.trim();
         if(curIECPN=="")
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
            document.getElementById("<%=btnDel.ClientID %>").disabled=true;
         }
         else
         {
            
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            document.getElementById("<%=btnDel.ClientID %>").disabled=false;
         }
         // itc-1361-0050  itc210012 2012-01-30
//         document.getElementById("<%=ttIECPN.ClientID %>").focus();
    }
   
   
    function AddUpdateComplete(id)
    {
        
        var gdObj=document.getElementById("<%=gd.ClientID %>");
      
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(gdObj.rows[i].cells[6].innerText==id)
           {
               selectedRowIndex=i;  
           }        
        }
        
        if(selectedRowIndex<0)
        {
           document.getElementById("<%=ttIECPN.ClientID %>").value=""; 
          
        
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;   
            document.getElementById("<%=btnDel.ClientID %>").disabled=true;          
            return;
        }
        else
        {            
            var con=gdObj.rows[selectedRowIndex];

            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
            
        }        
        
    }
    
    function NoMatchFamily()
    {
//         alert("Cant find that match family.");    //5 
   //      alert(msg5);     
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


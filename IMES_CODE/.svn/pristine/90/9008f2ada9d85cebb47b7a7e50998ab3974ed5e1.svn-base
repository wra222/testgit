<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="PAKFloatLocation.aspx.cs" Inherits="DataMaintain_PAKitLoc" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <style type="text/css">

.iMes_div_MainTainEdit
{
    border: thin solid Black; 
    background-color: #99CDFF;
    margin:0 0 20 0;
    
}

</style>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer"> 
         <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%" class="iMes_div_MainTainEdit">
                    <tr>
                        <td style="width:200px">
                            <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                        <iMESMaintain:CmbPAKitLocPdLineMaintain ID="cmbPdLine" runat="server" Width="380px"></iMESMaintain:CmbPAKitLocPdLineMaintain>             
                        </td> 
                    </tr>
            </table> 
        </div>  
        
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblPAKitLoc" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        
                    </td> 
                    <td>
                    <asp:TextBox ID="ttPartNoList" runat="server"   MaxLength="50"  Width="49%" 
                            CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>
                    </td>                                   
                    <td width="32%" align="right">
                    <input type="button" id="btnDel" runat="server" class="iMes_button"  onclick="if(clkDelete())" onserverclick="btnDelete_ServerClick"/>                        
                    </td>    
                </tr>
             </table>  
                                                  
        </div>

        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRefreshPartNoList" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnShowSelectedInfo" EventName="ServerClick" />
            
            <asp:AsyncPostBackTrigger ControlID="btnDel" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
             <asp:AsyncPostBackTrigger ControlID="btnPdLineChange" EventName="ServerClick" />
        </Triggers>
        <ContentTemplate>
        </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div2">
              <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <ContentTemplate>
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt SkinID="clearStyle"  ID="gd" runat="server" AutoGenerateColumns="true" 
                        Width="100%"  GvExtHeight="676px" RowStyle-Height="20"
                        GvExtWidth="100%"  AutoHighlightScrollByValue ="true" 
                        HighLightRowPosition="3"  
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' 
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false"  
                        style="top: -163px; left: -286px">
                    </iMES:GridViewExt>
                 </ContentTemplate>
               </asp:UpdatePanel>   
        </div>
        
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr >
                        <td style="width: 90px;">
                            <asp:Label ID="lblTypeDescrDdl" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="33%">
                             
                             <iMESMaintain:CmbPAKitLocDescrMaintain ID="ddlDescr"  runat="server" Width="82%"></iMESMaintain:CmbPAKitLocDescrMaintain>
                          
                         </td>
                         <td style="width: 90px;">
                          <asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                         </td>
                          
                          <td width="33%">
                            <iMESMaintain:CmbPAKitLocPartNoMaintain ID="ddlPartNo" runat="server" Width="82%"></iMESMaintain:CmbPAKitLocPartNoMaintain>
                           </td>
                          
                        <td align="right">
                            <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnAdd_ServerClick"/>
                        </td>           
                    </tr>
        
                    <tr >
                        <td style="width: 90px;">
                            <asp:Label ID="lblStationDdl" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                            <td width="33%">
                                
                                <iMESMaintain:CmbPAKitLocStationMaintain ID="ddlStation" runat="server" Width="82%"></iMESMaintain:CmbPAKitLocStationMaintain>
                          
                            </td>
                            <td style="width: 90px;">
                          <asp:Label ID="lblLocation" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                         </td>
                             <td width="33%">
                            <asp:TextBox ID="ttLocation" runat="server" onpaste="return false"  MaxLength="30"   Width="82%"  
                                CssClass="iMes_textbox_input_Yellow" onkeypress='checkCodePress(this)' ></asp:TextBox>
                           </td>
                            <td align="right">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"/>
                            <input type="hidden" id="dOldPartNo" runat="server" />
                            <input type="hidden" id="dOldPdLine" runat="server" />
                            <input type="hidden" id="dOldGrade" runat="server" />
                            <input type="hidden" id="dTableHeight" runat="server" />
                            <input type="hidden" id="HiddenTypeDescr" runat="server" />
                            <input type="hidden" id="HiddenPartNo" runat="server" />
                            <input type="hidden" id="HiddenStation" runat="server" />
                            <input type="hidden" id="HiddenLocation" runat="server" />
                        </td>           
                    </tr>                    
                    
            </table> 
        </div>  
      
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="HiddenCustomerName" runat="server" />
        <input type="hidden" id="HiddenSelectedId" runat="server" />
        <button id="btnPdLineChange" runat="server" type="button" style="display:none" onserverclick ="btnPdLineChange_ServerClick"> </button>
        <button id="btnRefreshPartNoList" runat="server" type="button"  style="display: none" onserverclick ="btnRefreshPartNoList_ServerClick"></button>
        <button id="btnShowSelectedInfo" runat="server" type="button"  style="display: none" onserverclick ="btnDisplaySelectedInfo_ServerClick"></button>
        
        <button id="btnRefreshCustomerList" runat="server" type="button" style="display: none" ></button>

    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr></table></div><script language="javascript" type="text/javascript">
    var customerObj;
    var selectedCon=null;
    var indexSelected;

    function OnKeyPress(obj)
    {
        var key = event.keyCode;
        
        if (key==13)//enter
        {   
            
              
            if(event.srcElement.id=="<%=ttPartNoList.ClientID %>")
            {
                var value=document.getElementById("<%=ttPartNoList.ClientID %>").value.trim().toUpperCase();
                if(value!="")
                {
                    findFamily(value, true);
                }
            }
        }
    }
    
    function checkCodePress(obj)
   { 
		var key = event.keyCode;
        
		if(obj.value==""&&key==126)
		{
			event.keyCode = 0;
			return;
		}
       
       if(key!=126&&!(key >= 48 && key <= 57))
       {  
           event.keyCode = 0;
       }
 
     }
    
    function findFamily(searchValue, isNeedPromptAlert)
    {
        
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(searchValue.trim()!="" && gdObj.rows[i].cells[0].innerText.toUpperCase().indexOf(searchValue.toUpperCase())==0)
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
    //      setRowSelectedByIndex_<%=gd.ClientID%>(con.index, false, "<%=gd.ClientID%>");
    //        setGdHighLight(con);
            selectedCon=con;
            indexSelected=selectedRowIndex;
            markSelectedRecord(con);
            
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
    var msg10="";
  
    window.onload = function()
    {

        
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";
        msg6="<%=pmtMessage6 %>";
        msg7="<%=pmtMessage7 %>";
        msg8="<%=pmtMessage8 %>";
        msg9="<%=pmtMessage9 %>";
        msg10="<%=pmtMessage10 %>";
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
         ShowRowEditInfo(null);
        initContorls();
        resetTableHeight();
    }
    function initContorls()
    {
    
 
       getPAKitLocPdLineCmbObj().onchange=cmbFamilyTopChange;
       getPAKitLocDescrCmbObj().onchange=cmbRefreshPartNoChange;
    }
    function cmbRefreshPartNoChange()
    {
       
        document.getElementById("<%=btnRefreshPartNoList.ClientID%>").click();
        ShowWait();
    }
    function showSelectedInfo()
    {
        document.getElementById("<%=btnShowSelectedInfo.ClientID%>").click();
        ShowWait();
    }
    function cmbFamilyTopChange()
    {
        
        document.getElementById("<%=btnPdLineChange.ClientID%>").click();
        ShowWait();
    }
    function resetTableHeight()
    {
        //动态调整表格的高度
        var adjustValue=80;     
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
     return checkAdaptorInfo();
   }
   function checkAdaptorInfo(){
          ShowInfo("");
       var series = document.getElementById("<%=ttLocation.ClientID %>"); 
        var reg=/^\d+~\d+$/;
       if(getPAKitLocPdLineCmbObj().value.trim()=="")
       {
        alert(msg8);
        getPAKitLocPdLineCmbObj().focus();
        return false;
       }
       else if(getPAKitLocDescrCmbObj().value.trim()=="")
       {
        alert(msg9);
        getPAKitLocDescrCmbObj().focus();
        return false;
       }
       else if(getPAKitLocPartNoCmbObj().value.trim()=="")
        {
            alert(msg5);
            getPAKitLocPartNoCmbObj().focus();
            return false;
        }
      else  if(series.value.trim()=="")
       {
//           alert("Please input [Family] first!!");  //4
           alert(msg4);
           series.focus();
           return false;
       }
       else if(!reg.test(series.value.trim()))
       {
        alert(msg6);
        series.focus();
        return false;
       }
       //add rigth >= left
      
       if(reg.test(series.value.trim()))
       {
            var arr=series.value.trim().split("~");
            if(parseInt(arr[1])<parseInt(arr[0]))
            {
                alert(msg10);
                series.focus();
                return false;
            }
       }
       ShowWait();
       return true;
   }
    function clickTable(con)
    {
         ShowInfo("");
         var selectedRowIndex = con.index;
         selectedCon=con;
    //     setRowSelectedByIndex_<%=gd.ClientID%>(con.index, false, "<%=gd.ClientID%>"); 
        setGdHighLight(con);  
         markSelectedRecord(con);  
         ShowRowEditInfo(con);
       
    }
    function markSelectedRecord(con)
    {
        document.getElementById("<%=HiddenTypeDescr.ClientID %>").value=con.cells[1].innerText.trim();
        document.getElementById("<%=HiddenPartNo.ClientID %>").value=con.cells[0].innerText.trim();
        document.getElementById("<%=HiddenStation.ClientID %>").value=con.cells[2].innerText.trim();
        document.getElementById("<%=HiddenLocation.ClientID %>").value=con.cells[3].innerText.trim(); 
        document.getElementById("<%=HiddenSelectedId.ClientID %>").value=con.cells[7].innerText.trim(); 
    }
    
    
    function ShowRowEditInfo(con)
    {
//        customerObj = getCustomerCmbObj();
//        customerObj.onchange = addNew;
         if(con==null)
         {
            document.getElementById("<%=ttLocation.ClientID %>").value = "";
            getPAKitLocPartNoCmbObj().selectedIndex = 0;
            getPAKitLocStationCmbObj().selectedIndex = 0;
            getPAKitLocDescrCmbObj().selectedIndex=0;
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
            document.getElementById("<%=btnDel.ClientID %>").disabled=true; 
            return;    
         }
         var curPartNo= con.cells[0].innerText.trim();
        if(curPartNo!="")
         {
            showSelectedInfo();
         }
         else
         {
            
             var curDescr=con.cells[1].innerText.trim();
             var curStation=con.cells[2].innerText.trim();
             var curLocation=con.cells[3].innerText.trim();
             getPAKitLocDescrCmbObj().value=curDescr;
             document.getElementById("<%=ttLocation.ClientID %>").value = curLocation;
             getPAKitLocStationCmbObj().value = curStation;
             getPAKitLocPartNoCmbObj().value=curPartNo;
             document.getElementById("<%=btnSave.ClientID %>").disabled=true;
            document.getElementById("<%=btnDel.ClientID %>").disabled=true;
         }
    }
    function showSelected()
    {
       if(selectedCon!=null)
       setGdHighLight(selectedCon);
   //    ShowRowEditInfo(selectedCon); 
    }
    
   function displaySelectedItem(descr,partno,station,location)
   {
       
       
 //       setRowSelectedByIndex_<%=gd.ClientID%>(selectedCon, false, "<%=gd.ClientID%>");
        setGdHighLight(selectedCon);
        setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
//         var curPartNo= con.cells[0].innerText.trim();
//         alert(curPartNo);
//         var curDescr=con.cells[1].innerText.trim();
//         var curStation=con.cells[2].innerText.trim();
//         var curLocation=con.cells[3].innerText.trim();
         getPAKitLocDescrCmbObj().value=descr;
         document.getElementById("<%=ttLocation.ClientID %>").value = location;
         getPAKitLocStationCmbObj().value = station;
         getPAKitLocPartNoCmbObj().value=partno;
         
//         document.getElementById("<%=HiddenSelectedId.ClientID %>").value=con.cells[7].innerText.trim();
         
//         if(partno=="")
//         {
//            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
//            document.getElementById("<%=btnDel.ClientID %>").disabled=true;
//         }
//         else
//         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            document.getElementById("<%=btnDel.ClientID %>").disabled=false;
//         }
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
            getPAKitLocPartNoCmbObj().selectedIndex =0; 
            getPAKitLocStationCmbObj().selectedIndex = 0;  
            getPAKitLocDescrCmbObj().selectedIndex = 0;  
            document.getElementById("<%=ttLocation.ClientID %>").value = "";
          
        
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;   
            document.getElementById("<%=btnDel.ClientID %>").disabled=true;          
            return;
        }
        else
        {            
            var con=gdObj.rows[selectedRowIndex];
            //去掉标题行
           
      //      setGdHighLight(con);
            markSelectedRecord(con);
            selectedCon=con;
            indexSelected=selectedRowIndex;
            ShowRowEditInfo(con);
            
        }        
        
    }
    
    function NoMatchFamily()
    {
        
        
//         alert("Cant find that match family.");    //5 
         alert(msg7);     
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
</script></asp:Content>


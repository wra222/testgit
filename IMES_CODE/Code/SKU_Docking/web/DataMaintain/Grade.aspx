<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="Grade.aspx.cs" Inherits="DataMaintain_Grade" %>

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

    .style1
    {
        width: 5px;
    }

</style>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer"> 
         <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr >
                        <td style="width: 200px;">
                            <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td> 
                        <td>
                         <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                         <ContentTemplate>
                             <iMESMaintain:CmbGradeFamilyTopMaintain ID="cmbGradeFamilyTop" runat="server" Width="380px"></iMESMaintain:CmbGradeFamilyTopMaintain>             
                        </ContentTemplate>
                        </asp:UpdatePanel>  
                        </td>
                    </tr>
            </table> 
        </div>  
        
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblGradeList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        
                    </td> 
                    <td>
                    <asp:TextBox ID="ttGradeList" runat="server"   MaxLength="50"  Width="49%" 
                            CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>
                    </td>                                   
                    <td width="32%" align="right">
                    <input type="button" id="btnDel" runat="server" class="iMes_button"  onclick="if(clkDelete())" onserverclick="btnDelete_ServerClick"></button>                         
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
             <asp:AsyncPostBackTrigger ControlID="btnFamilyChange" EventName="ServerClick" />
        </Triggers>
        <ContentTemplate>
         </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div2">
              <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <ContentTemplate>
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt SkinID="clearStyle"  ID="gd" runat="server" AutoGenerateColumns="true" 
                        Width="120%"  GvExtHeight="676px" RowStyle-Height="20"
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
                        <td style="width: 110px;">
                            <asp:Label ID="lblFamilyDdl" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="110px">
                             <iMESMaintain:CmbMaintainGradeHPFamily ID="ddlFamily"  runat="server" Width="380px"></iMESMaintain:CmbMaintainGradeHPFamily>
                             
                         </td>
                         <td width="110px">
                          <asp:Label ID="lblSeries" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                         </td>
                          <td width="390px" colspan="3">
                            <asp:TextBox ID="ttSeries" runat="server"   MaxLength="30"   Width="100%"  
                                CssClass="iMes_textbox_input_Yellow" SkinId="textBoxSkin"></asp:TextBox>
                           </td>
                        <td align="right">
                            <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnAdd_ServerClick"/>
                        </td>           
                    </tr>
                    <tr >
                        <td style="width: 110px;">
                            <asp:Label ID="lblGradeDdl" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                            <td width="110px">
                                <iMESMaintain:CmbGradeMaintain ID="ddlGrade" runat="server" Width="380px"></iMESMaintain:CmbGradeMaintain>
                            </td>
                            <td style="width: 110px;">
                            <asp:Label ID="lblEnergia" runat="server" CssClass="iMes_label_13pt"></asp:Label></td><td width="140px" align="center">
                            
           <!-SkinId="textBoxSkin"->
                            <asp:TextBox ID="ttEnergia" SkinId="textBoxSkin" runat="server"   MaxLength="10"  Width="96%"  CssClass="iMes_textbox_input_Yellow"></asp:TextBox></td><td style="width: 110px;" align="center">
                            <asp:Label ID="lblEspera" runat="server" CssClass="iMes_label_13pt"></asp:Label></td><td width="140px" align="center">
                            
                            <asp:TextBox ID="ttEspera" SkinId="textBoxSkin" runat="server"   MaxLength="10"  Width="96%"  CssClass="iMes_textbox_input_Yellow"></asp:TextBox></td><td align="right">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"/>
                            <input type="hidden" id="dOldFamilyId" runat="server" />
                            <input type="hidden" id="dOldSeries" runat="server" />
                            <input type="hidden" id="dOldGrade" runat="server" />
                            <input type="hidden" id="dTableHeight" runat="server" />
                        </td>           
                    </tr>                    
                    
                    
            </table> 
        </div>  
             
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="HiddenCustomerName" runat="server" />
        <input type="hidden" id="HiddenSelectedId" runat="server" />
        <button id="btnFamilyChange" runat="server" type="button" style="display:none" onserverclick ="btnFamilyChange_ServerClick"> </button>
        <button id="btnRefreshFamilyList" runat="server" type="button" onclick="" style="display: none" ></button>
        <button id="btnRefreshCustomerList" runat="server" type="button" onclick="" style="display: none" ></button>

    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr></table></div><script language="javascript" type="text/javascript">
    var customerObj;
    
    function OnKeyPress(obj)
    {
        var key = event.keyCode;
        
        if (key==13)//enter
        {   
            
              
            if(event.srcElement.id=="<%=ttGradeList.ClientID %>")
            {
                var value=document.getElementById("<%=ttGradeList.ClientID %>").value.trim().toUpperCase();
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
  //      document.getElementById("div_<%=gd.ClientID %>").style.height="616px";
  //      document.getElementById("div2").style.height="800px";
        initContorls();
        resetTableHeight();
    };
    function initContorls()
    {
      getGradeFamiyTopCmbObj().onchange=cmbFamilyTopChange;
    }
    function cmbFamilyTopChange()
    {
        document.getElementById("<%=btnFamilyChange.ClientID%>").click();
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
       var series = document.getElementById("<%=ttSeries.ClientID %>"); 
        var family=getGradeFamiyCmbObj();
        var grade=getGradeCmbObj();
       if(series.value.trim()=="")
       {
//           alert("Please input [Family] first!!");  //4
           alert(msg4);
           series.focus();
           return false;
       }
       if(family.value.trim()=="")
       {
            alert(msg8);
            family.focus();
            return false;
            
       }
       if(grade.value.trim()=="")
       {
            alert(msg9);
            grade.focus();
            return false;
       } 
       ShowWait();
       return true;
   }
    function clickTable(con)
    {
         ShowInfo("");
         var selectedRowIndex = con.index;
   //      setRowSelectedByIndex_<%=gd.ClientID%>(con.index, false, "<%=gd.ClientID%>");
        setGdHighLight(con);          
         ShowRowEditInfo(con);
       
    }
    
    function ShowRowEditInfo(con)
    {
//        customerObj = getCustomerCmbObj();
//        customerObj.onchange = addNew;

         if(con==null)
         {
            getGradeFamiyCmbObj().selectedIndex =0; 
            getGradeCmbObj().selectedIndex = 0;  
            document.getElementById("<%=ttSeries.ClientID %>").value = "";
            document.getElementById("<%=ttEnergia.ClientID %>").value = "";
            document.getElementById("<%=ttEspera.ClientID %>").value = "";
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
            document.getElementById("<%=btnDel.ClientID %>").disabled=true; 
            return;    
         }
    
         var curFamily= con.cells[0].innerText.trim();
         var curSeries=con.cells[1].innerText.trim();
         var curGrade=con.cells[2].innerText.trim();
         getGradeFamiyCmbObj().value=curFamily;
         document.getElementById("<%=ttSeries.ClientID %>").value = curSeries;
         getGradeCmbObj().value = curGrade;
         document.getElementById("<%=ttEnergia.ClientID %>").value = con.cells[3].innerText.trim();
         document.getElementById("<%=ttEspera.ClientID %>").value = con.cells[4].innerText.trim();
         
         //记录下将要被删除的Assembly的值     
         document.getElementById("<%=dOldFamilyId.ClientID %>").value = curFamily;
         document.getElementById("<%=dOldSeries.ClientID %>").value = curSeries;
         document.getElementById("<%=dOldGrade.ClientID %>").value = curGrade;
         document.getElementById("<%=HiddenSelectedId.ClientID %>").value=con.cells[8].innerText.trim();
         if(curFamily=="")
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
            document.getElementById("<%=btnDel.ClientID %>").disabled=true;
         }
         else
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            document.getElementById("<%=btnDel.ClientID %>").disabled=false;
         }
  
    }
   
   function clear()
   {
         document.getElementById("<%=ttSeries.ClientID %>").value = "";
         document.getElementById("<%=ttEnergia.ClientID %>").value = "";
         document.getElementById("<%=ttEspera.ClientID %>").value = "";
   }
   
    function AddUpdateComplete(id)
    {
       
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(gdObj.rows[i].cells[8].innerText==id)
           {
               selectedRowIndex=i;  
           }        
        }
        
        if(selectedRowIndex<0)
        {
            getGradeFamiyCmbObj().selectedIndex =0; 
            getGradeCmbObj().selectedIndex = 0;  
            document.getElementById("<%=ttSeries.ClientID %>").value = "";
            document.getElementById("<%=ttEnergia.ClientID %>").value = "";
            document.getElementById("<%=ttEspera.ClientID %>").value = "";
        
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;   
            document.getElementById("<%=btnDel.ClientID %>").disabled=true;          
            return;
        }
        else
        {            
            var con=gdObj.rows[selectedRowIndex];
            //去掉标题行
           
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
       
    </script></asp:Content>
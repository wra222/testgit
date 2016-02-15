<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="RepairInfoMaintain.aspx.cs" Inherits="DataMaintain_RepairInfoMaintain" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer"> 
         <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr >
                        <td style="width: 200px;">
                            <asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td> 
                        <td>
                         <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                         <ContentTemplate>
                             <iMESMaintain:CmbMaintainRepairInfoType ID="cmbRepairInfoType" runat="server" Width="380px"></iMESMaintain:CmbMaintainRepairInfoType>             
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
                        <asp:Label ID="lblRepairInfoList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        
                    </td> 
                    <td>
                    <asp:TextBox ID="ttRepairInfoList" runat="server"   MaxLength="10"  Width="49%" 
                            SkinId="textBoxSkin" onkeypress='OnKeyPress(this)'></asp:TextBox>
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
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnTypeChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnTypeFirstOption" EventName="ServerClick" />
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
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
                 </ContentTemplate>
               </asp:UpdatePanel>   
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr >
                        <td style="width: 110px;">
                            <asp:Label ID="lblCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td style="width: 110px;">
                             <asp:TextBox ID="ttCode" runat="server" SkinId="textBoxSkin"  MaxLength="10"   Width="100%"  
                                CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>
                         </td>
                         <td style="width: 110px;" align="center">
                          <asp:Label ID="lblDescription" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                         </td>
                          <td style="width: 360px;" colspan="3">
                            <asp:TextBox ID="ttDescription" runat="server" SkinId="textBoxSkin"  MaxLength="80"   Width="100%"  
                                CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>
                           </td>
                        <td align="right">
                            
                        </td>           
                    </tr>
                    <tr >
                        <td style="width: 110px;">
                            
                        </td>
                        <td style="width: 110px;">
                             
                         </td>
                         <td style="width: 110px;" align="center">
                          <asp:Label ID="lblEngDesc" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                         </td>
                          <td style="width: 360px;" colspan="3">
                            <asp:TextBox ID="ttEngDesc" runat="server" SkinId="textBoxSkin"  MaxLength="80"   Width="100%"  
                                CssClass="iMes_textbox_input_Yellow" onpaste="return false" ondragenter="return false" onkeyup="this.value=this.value.replace(/[\u0391-\uffe5]/gi,'')"></asp:TextBox>
                           </td>
                        <td align="right">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkAdd())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"/>
                        </td>           
                    </tr>
            </table> 
        </div>  
             
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="HiddenCustomerName" runat="server" />
        <input type="hidden" id="HiddenSelectedId" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="DelCode" runat="server" />
        <input type="hidden" id="OldType" runat="server" />
        <input type="hidden" id="OldDesc" runat="server" />
        <input type="hidden" id="OldID" runat="server" />
        <button id="btnTypeChange" runat="server" type="button" style="display:none" onserverclick ="btnTypeChange_ServerClick"> </button>
        <button id="btnTypeFirstOption" runat="server" type="button" onclick="" style="display: none" onserverclick ="btnTypeFirstOption_ServerClick"></button>
        <button id="btnRefreshCustomerList" runat="server" type="button" onclick="" style="display: none" ></button>

    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr></table></div><script language="javascript" type="text/javascript">
    var customerObj;
    var codeCodePattern = /^[0-9A-Z]*$/;
    function OnKeyPress(obj)
    {
        var key = event.keyCode;
        
        if (key==13)//enter
        {   
            
              
            if(event.srcElement.id=="<%=ttRepairInfoList.ClientID %>")
            {
                var value=document.getElementById("<%=ttRepairInfoList.ClientID %>").value.trim().toUpperCase();
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
    
    var msg10="";
    var msg11="";
  
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
        
        msg10="<%=pmtMessage10 %>";
        msg11="<%=pmtMessage11 %>";
        
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        ShowRowEditInfo(null);
  //      document.getElementById("div_<%=gd.ClientID %>").style.height="616px";
  //      document.getElementById("div2").style.height="800px";
        initContorls();
        resetTableHeight();
    };
    function initContorls()
    {
      getMaintainRepairInfoTypeCmbObj().onchange=cmbTypeChange;
      
    }
    function initConditionData()
    {
        document.getElementById("<%=btnTypeFirstOption.ClientID %>").click();
    }
    function cmbTypeChange()
    {
        document.getElementById("<%=btnTypeChange.ClientID%>").click();
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
     return checkInfo();  
   }
   
   function clkAdd()
   {
     return checkInfo();
   }
   function checkInfo(){
          ShowInfo("");
       var code = document.getElementById("<%=ttCode.ClientID %>"); 
        var descr=document.getElementById("<%=ttDescription.ClientID %>");
        var engDescr=document.getElementById("<%=ttEngDesc.ClientID %>");
        
       if(code.value.trim()=="")
       {
//           alert("Please input [Family] first!!");  //4
           alert(msg4);
           code.focus();
           return false;
       }
       else if(!codeCodePattern.test(code.value.toUpperCase()))
       {
            alert(msg2);
            code.focus();
            return false;
       }
       else if(descr.value.trim()=="")
       {
            alert(msg10);
            descr.focus();
            return false;
       }
       else if(engDescr.value.trim()=="")
       {
            alert(msg11);
            engDescr.focus();
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
         if(con==null)
         {
            document.getElementById("<%=ttCode.ClientID %>").value = "";
            document.getElementById("<%=ttDescription.ClientID %>").value = "";
            document.getElementById("<%=ttEngDesc.ClientID %>").value="";
            
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;  
            document.getElementById("<%=btnDel.ClientID %>").disabled=true; 
            return;    
         }
    
         var curCode= con.cells[0].innerText.trim();
         var curDesc=con.cells[1].innerText.trim();
         var curEngDescr=con.cells[2].innerText.trim();
         document.getElementById("<%=DelCode.ClientID %>").value=curCode;
         document.getElementById("<%=ttCode.ClientID %>").value = curCode;
        
         document.getElementById("<%=ttDescription.ClientID %>").value = curDesc;
         document.getElementById("<%=ttEngDesc.ClientID %>").value=curEngDescr;
         
         document.getElementById("<%=OldID.ClientID %>").value = con.cells[6].innerText.trim();
         //记录下将要被删除的Assembly的值     
         document.getElementById("<%=OldType.ClientID %>").value = curCode;
         document.getElementById("<%=OldDesc.ClientID %>").value = curDesc;
            
         if(curCode=="")
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            document.getElementById("<%=btnDel.ClientID %>").disabled=true;
         }
         else
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            document.getElementById("<%=btnDel.ClientID %>").disabled=false;
         }
  
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
            document.getElementById("<%=ttCode.ClientID %>").value = "";
            document.getElementById("<%=ttDescription.ClientID %>").value = "";
            document.getElementById("<%=ttEngDesc.ClientID %>").value="";
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;   
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
       
    </script>
</asp:Content>


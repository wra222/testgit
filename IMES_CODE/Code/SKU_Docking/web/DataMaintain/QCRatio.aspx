<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="QCRatio.aspx.cs" Inherits="DataMaintain_QCRatio" ValidateRequest="false" %>
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

                    <td class="iMes_div_MainTainListLable" >
                        <asp:Label ID="lblFamilyTop" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="dFamilyTop" runat="server"   MaxLength="50"  Width="96%" CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)' ></asp:TextBox>
                    </td>                                    
                    <td style="width:9%">
                        <asp:Label ID="lblCustomer" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="39%">
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>                    
                        <iMESMaintain:CmbCustomer runat="server" ID="cmbCustomer" Width="99%" ></iMESMaintain:CmbCustomer>
        </ContentTemplate>
        </asp:UpdatePanel>                            
                    </td>    
           
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable" >
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td width="30%" >  
                        <asp:TextBox ID="dSearch" runat="server"   MaxLength="50"  Width="96%" CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>                      
                    </td>    
                    <td width="48%" align="right" >
                       <input type="button" id="btnDelete" runat="server" onclick="if(clkButton())" class="iMes_button" onserverclick="btnDelete_ServerClick"></input>
                    </td>           
                </tr>
             </table>  
        </div>


        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
        <div id="div2" style="height:366px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="125%" 
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel> 
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>
    
                        <td style="width:11%; padding-left:2px;">
                            <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td  width="34%">
        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>                        
                            <iMESMaintain:CmbMaintainFamilyAddline runat="server" ID="cmbMaintainFamily" Width="98%" ></iMESMaintain:CmbMaintainFamilyAddline>
        </ContentTemplate>
        </asp:UpdatePanel>                            
                        </td> 
                        <td  style="width:10%;">
                            <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td  width="33%">
                            <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>                        
                                <iMESMaintain:CmbMaintainModelByFamily runat="server" ID="cmbMaintainModelByFamily" Width="98%" ></iMESMaintain:CmbMaintainModelByFamily>
                            </ContentTemplate>
                            </asp:UpdatePanel>                            
                        </td>                         
                                               
                        <td align="right">
                             <input type="button" id="btnAdd" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAdd_ServerClick"></input>
                        </td>           
                    </tr>
                    <tr>

                        <td >
                            <asp:Label ID="lblQCRatio" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="dQCRatio" runat="server"   MaxLength="9"   Width="96%"   SkinId="textBoxSkin" onkeypress='checkCodePress(this)' style='ime-mode:disabled;' onpaste="return false" ></asp:TextBox>
                        </td>
                        
                        <td >
                            <asp:Label ID="lblEQQCRatio" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td >                            
                            <asp:TextBox ID="dEQQCRatio" runat="server"   MaxLength="9"   Width="96%"  SkinId="textBoxSkin" onkeypress='checkCodePress(this)' style='ime-mode:disabled;' onpaste="return false" ></asp:TextBox>
                        </td> 
                             
                        <td align="right">
                           <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></input>
                        </td>           
                    </tr>     
                    <tr>

                        <td >
                            <asp:Label ID="lblPAQCRatio" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="dPAQCRatio" runat="server"   MaxLength="9"   Width="96%"   SkinId="textBoxSkin" onkeypress='checkCodePress(this)' style='ime-mode:disabled;' onpaste="return false" ></asp:TextBox>
                        </td>
                                                
                        <td >
                             <asp:Label ID="lblRPAQCRatio" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td >
                              <asp:TextBox ID="dRPAQCRatio" runat="server"   MaxLength="9"   Width="96%"   SkinId="textBoxSkin" onkeypress='checkCodePress(this)' style='ime-mode:disabled;' onpaste="return false" ></asp:TextBox>                           
                        </td> 
          
                    </tr>                                     
                      
            </table> 
        </div>  
   
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
            <asp:AsyncPostBackTrigger ControlID="btnFamilyChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnCustomerChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnComBoxFamilyChange" EventName="ServerClick" />
        </Triggers>                      
        </asp:UpdatePanel>         
           <input type="hidden" id="HiddenUserName" runat="server" />
           <input type="hidden" id="dOldId" runat="server" /> 
           <input type="hidden" id="dModelValue" runat="server" />     
           <input type="hidden" id="dTableHeight" runat="server" />    
           <button id="btnFamilyChange" runat="server" type="button" style="display:none" onserverclick ="btnFamilyChange_ServerClick"> </button>
           <button id="btnCustomerChange" runat="server" type="button" style="display:none" onserverclick ="btnCustomerChange_ServerClick"> </button>
           <button id="btnComBoxFamilyChange" runat="server" type="button" style="display:none" onserverclick ="btnComBoxFamilyChange_ServerClick"> </button>
          
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
    var msg10="";
    var msg11="";
    var msg12 = "";
    var msg13 = "";
    function clkButton()
    {
       switch(event.srcElement.id)
       {
           case "<%=btnSave.ClientID %>":
                
                if(clkSave()==false)
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
           case "<%=btnAdd.ClientID %>": 	  
                if(clkAdd()==false)
                {                
                    return false;
                }
 	            break;     
    	}   
        ShowWait();
        return true;
    }
  
    var iSelectedRowIndex=null; 
    function setGdHighLight(con)
    {
        if((iSelectedRowIndex!=null) && (iSelectedRowIndex!=parseInt(con.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex,false, "<%=gd.ClientID %>");                
        }        
        setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
        iSelectedRowIndex=parseInt(con.index, 10);    
    }
    
    function OnKeyPress(obj)
    {
        var key = event.keyCode;
        
        if (key==13)//enter
        {
            if(event.srcElement.id=="<%=dFamilyTop.ClientID %>")
            {
                if(document.getElementById("<%=dFamilyTop.ClientID %>").value.trim()!="")
                {
                    document.getElementById("<%=btnFamilyChange.ClientID %>").click();
                    ShowWait();
                }
                
            }
            else if(event.srcElement.id=="<%=dSearch.ClientID %>")
            {
                var value=document.getElementById("<%=dSearch.ClientID %>").value.trim().toUpperCase();
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
        searchValue=searchValue.toUpperCase(); 
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
                alert(msg1);
//                alert("找不到列表中与Family栏位匹配的项");     
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
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
        }    
    }
    
    function NoMatchFamily()
    {
         alert(msg2);
//         alert("不存在查询的[Family]");     
         return;   
    }
    
    
    function initControls()
    {
        getCustomerCmbObj().onchange=CustomerSelectOnChange;   
        getMaintainFamilyCmbObj().onchange=FamilySelectOnChange; 
    }
    
    function CustomerSelectOnChange()
    {       
        document.getElementById("<%=btnCustomerChange.ClientID%>").click();
        ShowWait();
    }
    
    function FamilySelectOnChange()
    {       
        document.getElementById("<%=dModelValue.ClientID %>").value = ""; 
        DealFamilySelectOnChange();
    }
    
    function DealFamilySelectOnChange()
    {       
        document.getElementById("<%=btnComBoxFamilyChange.ClientID%>").click();
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
        msg7  ="<%=pmtMessage7%>";
        msg8  ="<%=pmtMessage8%>";
        msg9  ="<%=pmtMessage9%>";
        msg10  ="<%=pmtMessage10%>";
        msg11  ="<%=pmtMessage11%>";
        msg12 = "<%=pmtMessage12%>";
        msg13 = "<%=pmtMessage13%>"; 
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        initControls();  
        ShowRowEditInfo(null);
      
        //设置表格的高度  
        resetTableHeight();
        
    };

    //设置表格的高度  
    function resetTableHeight()
    {    
        //动态调整表格的高度
        var adjustValue=55;     
        var marginValue=12; 
        var tableHeigth=300;
        //var tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
        try{
            tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
        }
        catch(e){
        
            //ignore
        }                
        //为了使表格下面有写空隙
        var extDivHeight=tableHeigth+marginValue;
        div2.style.height=extDivHeight+"px";
        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
    }
    
    function clkDelete()
    {   
//         var ret = confirm("确定要删除这条记录么？");
         var ret = confirm(msg4);
         if (!ret) {
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
       //ShowInfo("");
       return check();
        
   }
   
   function check()
   {
       var QCRatioValue=document.getElementById("<%=dQCRatio.ClientID %>").value.trim();
       var EQQCRatioValue=document.getElementById("<%=dEQQCRatio.ClientID %>").value.trim();
       var PAQCRatioValue = document.getElementById("<%=dPAQCRatio.ClientID %>").value.trim();
       var RPAQCRatioValue = document.getElementById("<%=dRPAQCRatio.ClientID %>").value.trim();
       
        
       if(getMaintainFamilyCmbObj().value.trim()=="")
       {
            alert(msg5);
//            alert("需要选择[family]");
            return false;    
       } 
        
       if(QCRatioValue=="" )
       {
            alert(msg6);
//            alert("需要输入[OQC Ratio]");
            return false;
       }
       if(parseInt(QCRatioValue).toString()=="0" )
       {
           //UC确认,可以输入0 Modify 2012/07/25
           if (QCRatioValue < 0) {
               alert(msg7);
               //            alert("[OQC Ratio]应该是大于0的数");
               return false;
           }
       }
       
       if(EQQCRatioValue=="")
       {
            alert(msg8);
//            alert("需要输入[EOQC Ratio]");
            return false;
       }



       if (QCRatioValue != 0) {
            if(!(parseInt(QCRatioValue)<parseInt(EQQCRatioValue)))
            {
                alert(msg9);
                //            alert("[EOQC Ratio]应该比[OQC Ratio]大");
                return false;      
            }
       }
       
       
       
       
       
       if(PAQCRatioValue=="" )
       {
            alert(msg10);
//            alert("需要输入[PAQC Ratio]");
            return false;
       }  
       
       if(parseInt(PAQCRatioValue).toString()=="0" )
       {
           ////UC确认,可以输入0 Modify 2012/07/25
           if (PAQCRatioValue < 0) {
               alert(msg11);
               //            alert("[PAQC Ratio]应该是大于0的数");
               return false;
           }
       }

       if (RPAQCRatioValue == "") {
           alert(msg12);
           //alert("需要输入[RPAQC Ratio]");
           return false;
       }
       if (parseInt(RPAQCRatioValue).toString() == "0") {
           ////UC确认,可以输入0 Modify 2012/07/25
           if (RPAQCRatioValue < 0) {
               alert(msg13);
               //            alert("[RPAQC Ratio]应该是大于0的数");
               return false;
           }
       }

     
       return true;
   }
   
   function clkAdd()
   {
        //ShowInfo("");
        return check();
   }
   
     function clickTable(con)
    {
         setGdHighLight(con);         
         ShowRowEditInfo(con);
       
    }
    
    function setNewItemValue()
    {
        getMaintainFamilyCmbObj().selectedIndex=0;
        getMaintainModelByFamilyCmbObj().selectedIndex=0;
        getMaintainModelByFamilyCmbObj().disabled = true; 
        
        document.getElementById("<%=dQCRatio.ClientID %>").value = ""
        document.getElementById("<%=dEQQCRatio.ClientID %>").value ="";
        document.getElementById("<%=dPAQCRatio.ClientID %>").value = "";
        document.getElementById("<%=dRPAQCRatio.ClientID %>").value = "";
        document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
        document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
    }
    
    function ShowRowEditInfo(con)
    {
         if(con==null)
         {
            setNewItemValue();
            return;    
         }
         //C.NewFamily,A.[QCRatio],A.[EOQCRatio],A.[PAQCRatio],A.Editor,C.Model,A.Cdt,A.Udt, C.FamilyKey ,C.Flag 
         getMaintainFamilyCmbObj().value =con.cells[0].innerText.trim();          
         document.getElementById("<%=dQCRatio.ClientID %>").value=con.cells[2].innerText.trim();       //2
         document.getElementById("<%=dEQQCRatio.ClientID %>").value=con.cells[3].innerText.trim();     //3
         document.getElementById("<%=dPAQCRatio.ClientID %>").value = con.cells[4].innerText.trim();     //4
         document.getElementById("<%=dRPAQCRatio.ClientID %>").value = con.cells[5].innerText.trim();     //4
         //getMaintainModelByFamilyCmbObj().value=con.cells[1].innerText.trim(); 
         document.getElementById("<%=dModelValue.ClientID %>").value = con.cells[1].innerText.trim();     //1     
                 
         var currentId=con.cells[9].innerText.trim(); 
         document.getElementById("<%=dOldId.ClientID %>").value = currentId;
  
         if(currentId=="")
         {
//            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
//            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;
            setNewItemValue();
            return;
         }
         else
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
         }
         
         DealFamilySelectOnChange();
         
         //trySetFocus();
    }
   
    function trySetFocus()
    {
         var itemObj=document.getElementById("<%=dQCRatio.ClientID %>");//getMaintainFamilyCmbObj();
         
         if(itemObj!=null && itemObj!=undefined && itemObj.disabled!=true)
         {
            itemObj.focus();
         }
    }
   
    function AddUpdateComplete(id)
    {
  
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(gdObj.rows[i].cells[9].innerText==id)
           {
               selectedRowIndex=i; 
               break; 
           }        
        }
        
        if(selectedRowIndex<0)
        {
            ShowRowEditInfo(null);    
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
   
   
    function checkCodePress(obj)
    { 
       var key = event.keyCode;

       if(!(key >= 48 && key <= 57))
       {  
           event.keyCode = 0;
       }
 
     }
     
    function DealHideWait()
    {
        HideWait();   
        getCustomerCmbObj().disabled = false; 
        getMaintainFamilyCmbObj().disabled = false; 
        getMaintainModelByFamilyCmbObj().disabled = false;
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


<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="AssetCheckRule.aspx.cs" Inherits="DataMaintain_AssetCheckRule" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable" >
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%" ></asp:Label>
                    </td>
                    <td width="35%">  
                        <asp:TextBox ID="dSearch" runat="server"   MaxLength="20"  Width="90%" SkinId="textBoxSkin" onkeypress='OnKeyPress(this)'></asp:TextBox>                      
                    </td>    
                    <td style="width:47%" align="right">
                      <input type="button" id="btnDelete" runat="server" onclick="if(clkButton())" class="iMes_button" onserverclick="btnDelete_ServerClick"></input>
                    </td>           
                </tr>
             </table>  
        </div>
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
            <ContentTemplate>
                <div id="div2" style="height:366px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="118%" 
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel> 
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                <tr>
                    <td style="width:10%">
                        <asp:Label ID="lblCheckStation" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td style="width:20%">
                        <iMESMaintain:CmbMaintainAssetRuleCheckStation runat="server" ID="cmbMaintainAssetRuleCheckStation" Width="98%" ></iMESMaintain:CmbMaintainAssetRuleCheckStation>
                    </td> 
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt" Width="100%" Text="Check Type:" ></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>                        
                                <asp:DropDownList runat="server" ID="cmbCheckType" Width="98%" >
                                    <asp:ListItem Text="Value" Value="Value" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="PrintLog" Value="PrintLog"></asp:ListItem>
                                    <asp:ListItem Text="ProductLog" Value="ProductLog"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                    </td>
                    <td>
                        <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"  Width="100%" Text="Remark:"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtRemark" runat="server"   MaxLength="250" Width="100%" CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>
                    </td>
                    <td></td>
                </tr>   
                <tr>
                    <td  style="width:10%">
                        <asp:Label ID="lblTp" runat="server" CssClass="iMes_label_13pt" Width="100%" Text="Tp:" ></asp:Label>
                    </td>    
                    <td style="width:20%">
                        <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>                        
                                <asp:DropDownList runat="server" ID="cmbTp" Width="98%" ></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                    </td> 
                    
                    <td style="width:10%">
                        <asp:Label ID="lblCustName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td style="width:20%">
                        <asp:UpdatePanel ID="updatePanel5" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>                        
                                <asp:DropDownList runat="server" ID="cmbAVPart" Width="98%"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>     
                    </td>&nbsp;
                    <td></td>
                    <td></td>
                    <td align="right" >
                        <input type="button" id="btnAdd" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAdd_ServerClick" />
                    </td>  
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblASTType" runat="server" CssClass="iMes_label_13pt" Width="100%" ></asp:Label>
                    </td>
                    <td>                     
                        <iMESMaintain:CmbMaintainAssetRuleAstType runat="server" ID="cmbMaintainAssetRuleAstType" Width="98%" ></iMESMaintain:CmbMaintainAssetRuleAstType>                         
                    </td> 
                    <td>                            
                        <asp:Label ID="lblCheckItem" runat="server" CssClass="iMes_label_13pt"  Width="100%"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>                        
                                <asp:DropDownList runat="server" ID="cmbMaintainAssetRuleCheckItem" Width="98%"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>                            
                    </td>
                    
                    <td></td>
                </tr>
            </table> 
        </div>  
   
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAstTypeChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnTpChange" EventName="ServerClick" />
        </Triggers>                      
        </asp:UpdatePanel>         
           <input type="hidden" id="HiddenUserName" runat="server" />
           <input type="hidden" id="dOldId" runat="server" /> 
           <input type="hidden" id="dTableHeight" runat="server" /> 
           <input type="hidden" id="dCheckItemValue" runat="server" />     
           <button id="btnAstTypeChange" runat="server" type="button" style="display:none" onserverclick ="btnAstTypeChange_ServerClick"> </button>
           <button id="btnTpChange" runat="server" type="button" style="display:none" onserverclick ="btnTpChange_ServerClick"> </button>
 
           <input type="hidden" id="hCode" runat="server" /> 
           <input type="hidden" id="hCheckTp" runat="server" /> 
           <input type="hidden" id="hStation" runat="server" /> 
         
    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
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

    function clkButton()
    {
       switch(event.srcElement.id)
       {
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
            if(event.srcElement.id=="<%=dSearch.ClientID %>")
            {
                var value=document.getElementById("<%=dSearch.ClientID %>").value.trim();//.toUpperCase();
                if(value!="")
                {
                    findAstType(value, true);
                }
            }
        }       
    }
     
    function findAstType(searchValue)
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        searchValue=searchValue.toUpperCase(); 
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(searchValue.trim()!="" && gdObj.rows[i].cells[2].innerText.toUpperCase().indexOf(searchValue)==0)//!!! cells[1]
           {
               selectedRowIndex=i;
               break;  
           }        
        }
        
        if(selectedRowIndex<0)
        {
            alert(msg7);
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
    
    
    function initControls()
    {
        getMaintainAssetRuleAstTypeCmbObj().onchange = AssetRuleAstTypeCmbObjOnChange;
        document.getElementById("<%=cmbCheckType.ClientID%>").onchange = AssetRuleAstTypeCmbObjOnChange;
        document.getElementById("<%=cmbTp.ClientID%>").onchange = TpCmbObjOnChange;
    }
    
    function AssetRuleAstTypeCmbObjOnChange()
    {       
        document.getElementById("<%=btnAstTypeChange.ClientID%>").click();
        ShowWait();
    }

    function TpCmbObjOnChange() {
        document.getElementById("<%=btnTpChange.ClientID%>").click();
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
        try{
            tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
        }
        catch(e){
        
            //ignore
        }
        //为了使表格下面有写空隙
        var extDivHeight=tableHeigth+marginValue;
    
        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
        div2.style.height=extDivHeight+"px";
    }
    
    function clkDelete()
    {        
//       var ret = confirm("确定要删除这条记录么？");
         var ret = confirm(msg6);
         if (!ret) {
             return false;
         }         
         return true;        
    }
   
   function DeleteComplete()
   {   
       ShowRowEditInfo(null);
   }
   
   function check()
   {
       var astTypeValue=getMaintainAssetRuleAstTypeCmbObj().value.trim();
     
       if(astTypeValue=="")
       {
            alert(msg1);
            return false;    
       }
       
       if(getMaintainAssetRuleCheckStationCmbObj().value.trim()=="")
       {
            alert(msg3);
            return false;    
       }
            
//       if(astTypeValue!="<%=ASTTYPE_ATSN1%>")
//       {
//           if (document.getElementById("<%=cmbAVPart.ClientID %>").value.trim() == "")
//           {
//               alert(msg4);
//               return false;
//           }       
//       } 
   
       
       if(astTypeValue=="<%=ASTTYPE_ATSN4%>"||astTypeValue=="<%=ASTTYPE_ATSN7%>")
       {
           if (document.getElementById("<%=cmbMaintainAssetRuleCheckItem.ClientID %>").value.trim() == "")
           {
               //Please select Check Item!
               alert(msg5);
               return false;
           }       
       } 
       return true;
   }
   
   function clkAdd()
   {
        return check();
   }
   
     function clickTable(con)
    {
         setGdHighLight(con);         
         ShowRowEditInfo(con);
       
    }
    
    function setNewItemValue()
    {
        getMaintainAssetRuleAstTypeCmbObj().selectedIndex=0;
        //getMaintainAssetRuleCheckItemCmbObj().selectedIndex=0;
        document.getElementById("<%=cmbMaintainAssetRuleCheckItem.ClientID %>").selectedIndex = 0;
        document.getElementById("<%=cmbTp.ClientID %>").selectedIndex = 0;
        getMaintainAssetRuleCheckStationCmbObj().selectedIndex=0;
        //getMaintainAssetRuleCheckTypeCmbObj().selectedIndex=0;
        document.getElementById("<%=cmbCheckType.ClientID %>").selectedIndex = 0;
        document.getElementById("<%=cmbAVPart.ClientID %>").selectedIndex = 0;
        document.getElementById("<%=txtRemark.ClientID %>").value = "";
        document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
    }
    
    function ShowRowEditInfo(con) {   
        if(con==null)
        {
            setNewItemValue();
            return;    
        }
         //C.NewFamily,A.[QCRatio],A.[EOQCRatio],A.[PAQCRatio],A.Editor,C.Model,A.Cdt,A.Udt, C.FamilyKey ,C.Flag

        getMaintainAssetRuleCheckStationCmbObj().text = con.cells[0].innerText.trim()                      //0

        document.getElementById("<%=cmbTp.ClientID %>").value = con.cells[1].innerText.trim();      //1


        getMaintainAssetRuleAstTypeCmbObj().value = con.cells[2].innerText.trim();                          //2
        document.getElementById("<%=hCode.ClientID %>").value = con.cells[2].innerText.trim();              //2
        
        document.getElementById("<%=cmbCheckType.ClientID %>").value = con.cells[3].innerText.trim();       //3
        document.getElementById("<%=cmbAVPart.ClientID %>").value = con.cells[4].innerText.trim();            //4
        document.getElementById("<%=dCheckItemValue.ClientID %>").value = con.cells[5].innerText.trim();    //5
        document.getElementById("<%=txtRemark.ClientID %>").value = con.cells[6].innerText.trim();          //6
        document.getElementById("<%=HiddenUserName.ClientID %>").value = con.cells[7].innerText.trim();     //7
        var currentId = con.cells[10].innerText.trim();
        var ID = con.cells[10].innerText.trim();
        document.getElementById("<%=dOldId.ClientID %>").value = ID;

        if(currentId=="") {
            setNewItemValue();
            return;
        }
        else {
            document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
        }         
        AssetRuleAstTypeCmbObjOnChange();         
         //trySetFocus();
    }
   
    function trySetFocus()
    {
         var itemObj=getMaintainAssetRuleAstTypeCmbObj;//getMaintainFamilyCmbObj();
         
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
           if(gdObj.rows[i].cells[10].innerText==id)
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
        
    function DealHideWait()
    {
        HideWait();   
        getMaintainAssetRuleAstTypeCmbObj().disabled = false;
        //getMaintainAssetRuleCheckItemCmbObj().disabled = false;
        document.getElementById("<%=cmbMaintainAssetRuleCheckItem.ClientID %>").disabled = false;
        getMaintainAssetRuleCheckStationCmbObj().disabled = false;
        //getMaintainAssetRuleCheckTypeCmbObj().disabled = false;
        
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


<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="Warranty.aspx.cs" Inherits="DataMaintain_Warranty" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="iMes_div_MainTainEdit" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblCustomer" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="30%">
                        <iMESMaintain:CmbCustomer runat="server" ID="cmbCustomer" Width="100%" ></iMESMaintain:CmbCustomer>
                    </td>    
                    <td width="48%">
                    </td>           
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="40%">                        
                    </td>    
                    <td width="38%" align="right">
                      <input type="button" id="btnDelete" runat="server" onclick="if(clkButton())" class="iMes_button" onserverclick="btnDelete_ServerClick" />
                    </td>           
                </tr>
             </table>  
        </div>
        
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
        <div id="div2" style="height:366px">

                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="128%" RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState="false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>         
        
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>
    
                        <td style="width:14%; padding-left:2px;">
                            <asp:Label ID="lblDesc" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="dDesc" runat="server"   MaxLength="80"   Width="98%" TabIndex="0"  SkinId="textBoxSkin" ></asp:TextBox>
                        </td>                          
                        <td align ="right" width="13%" >
                            <input type="button" id="btnAdd" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAdd_ServerClick" />
                        </td>           
                    </tr>
                    <tr>

                        <td  width="14%">
                            <asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="31%">
                            <iMESMaintain:CmbWarrantyType runat="server" ID="cmbWarrantyType" Width="97%" ></iMESMaintain:CmbWarrantyType>
                        </td>
                        
                        <td  width="13%">
                            <asp:Label ID="lblShipTypeCode" Width="100%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="31%">                            
                            <asp:TextBox ID="dShipTypeCode" runat="server"   MaxLength="2"   Width="95%" TabIndex="0"  CssClass="iMes_textbox_input_Yellow" onkeypress='checkWarrantyCodePress(this)' style='ime-mode:disabled;' onpaste="return false" ></asp:TextBox>
                        </td> 
                                                             
                        <td align="right" width="11%" >
                            <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick" />
                            <input type="hidden" id="dOldId" runat="server" />
                        </td>           
                    </tr>                    
                    
                     <tr>
                        <td >   
                            <asp:Label ID="lblFormat" runat="server" CssClass="iMes_label_13pt"></asp:Label>                         
                        </td>
                        <td >  
                            <iMESMaintain:CmbWarrantyFormat runat="server" ID="cmbWarrantyFormat" Width="97%" ></iMESMaintain:CmbWarrantyFormat>                          
                        </td>  
                        <td >
                            <asp:Label ID="lblWarrantyCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="dWarrantyCode" runat="server"   MaxLength="1"   Width="95%" TabIndex="0"  CssClass="iMes_textbox_input_Yellow" onkeypress='checkWarrantyCodePress(this)' style='ime-mode:disabled;' onpaste="return false" ></asp:TextBox>
                        </td> 
                                                 
                        <td >
                        </td>           
                    </tr>                     
            </table> 
        </div>  
   
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
             <asp:AsyncPostBackTrigger ControlID="btnCustomerChange" EventName="ServerClick" />
        </Triggers>                      
        </asp:UpdatePanel>   
         
           <input type="hidden" id="HiddenUserName" runat="server" />
           <input type="hidden" id="dTableHeight" runat="server" />    
           <button id="btnCustomerChange" runat="server" type="button" style="display:none" onserverclick ="btnCustomerChange_ServerClick"> </button>

    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div> 
    <script language="javascript" type="text/javascript">
    
//    function radioClick()
//    {
//        setControlState();
//    }
   
//    function setControlState()
//    {
//        if(document.getElementById("=dShipType.ClientID >").checked==true)
//        {      
//            getWarrantyFormatCmbObj().selectedIndex=0;
//            document.getElementById("=dWarrantyCode.ClientID >").value = ""; 
//            getWarrantyFormatCmbObj().disabled=true;
//            document.getElementById("=dWarrantyCode.ClientID >").disabled=true;
//            getWarrantyCodeCmbObj().disabled=false;
//            
//        }
//        else
//        {
//            getWarrantyCodeCmbObj().selectedIndex=0;
//            getWarrantyCodeCmbObj().disabled=true;
//            getWarrantyFormatCmbObj().disabled=false;
//            document.getElementById("=dWarrantyCode.ClientID >").disabled=false;            
//        }
//    }
   
    var msg1="";
    var msg2="";
    var msg3="";
    var msg4="";
    var msg5="";

   
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";
        
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
        var adjustValue=60;     
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
        
//         var ret = confirm("Do you really want to delete this item?"); //3
         var ret = confirm(msg3); //3
         if (!ret) {
             return false;
         }
         
         return true;
        
    }
   
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
   
   function DeleteComplete()
   {   
       ShowRowEditInfo(null);
   }
   
   function clkSave()
   {
//       ShowInfo("");

       return check();
        
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
   
   function check()
   {
        var descValue=document.getElementById("<%=dDesc.ClientID %>").value.trim();
        
       if(descValue=="")
       {
//            alert("Please input [Description] first!!"); //1
            alert(msg1);
            return false;
       }
       
//       var typeValue=getWarrantyTypeCmbObj().value.trim();
//       if(typeValue=="")
//       {
//            alert("Please select [Type] first!!");
//            return false;
//       }
       
       var shipTypeCodeValue=document.getElementById("<%=dShipTypeCode.ClientID %>").value.trim();
       if(shipTypeCodeValue=="")
       {
//            alert("Please input [ShipType Code] first!!");  //4
            alert(msg4);
            return false;
       }
       
//       var warrantyFormatValue=getWarrantyFormatCmbObj().value.trim();
//       if(warrantyFormatValue=="")
//       {
//            alert("Please select [Warranty Format] first!!");
//            return false;
//       }
       
       var warrantyCodeValue=document.getElementById("<%=dWarrantyCode.ClientID %>").value.trim();
       if(warrantyCodeValue=="")
       {
//            alert("Please input [Warranty Code] first!!");  //5
            alert(msg5);
            return false;
       }      
       return true;
   }
   
   function clkAdd()
   {
   
//       ShowInfo("");
        return check();

   }
   
    function clickTable(con)
    {
         setGdHighLight(con);         
         ShowRowEditInfo(con);
    }
    
    function setNewItemValue()
    {
        getWarrantyFormatCmbObj().selectedIndex=0;
        getWarrantyTypeCmbObj().selectedIndex=0;
        
        document.getElementById("<%=dShipTypeCode.ClientID %>").value = ""
//        document.getElementById("=dShipType.ClientID >").checked=true;
        document.getElementById("<%=dDesc.ClientID %>").value ="";             
        document.getElementById("<%=dWarrantyCode.ClientID %>").value = ""; 
                   
        document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
        document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
        //setControlState();
    }
    
    function ShowRowEditInfo(con)
    {
         if(con==null)
         {
            setNewItemValue();
            return;    
         }

         document.getElementById("<%=dDesc.ClientID %>").value =con.cells[0].innerText.trim();          
         document.getElementById("<%=dWarrantyCode.ClientID %>").value = con.cells[4].innerText.trim();                
         
         getWarrantyTypeCmbObj().value=con.cells[9].innerText.trim(); 
         document.getElementById("<%=dShipTypeCode.ClientID %>").value=con.cells[10].innerText.trim(); 
         
         
         getWarrantyFormatCmbObj().value=con.cells[11].innerText.trim(); 
        
//         var codeType=con.cells[9].innerText.trim();
//         if(codeType=="=DATACODE_TYPE_SHIPTYPE >")
//         {
//             document.getElementById("=dShipType.ClientID >").checked=true;
//         }
//         else
//         {
//             document.getElementById("=dWarranty.ClientID >").checked=true;
//         }
                 
         var currentId=con.cells[8].innerText.trim(); 
         document.getElementById("<%=dOldId.ClientID %>").value = currentId;
  
         if(currentId=="")
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;
            setNewItemValue();
         }
         else
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
         }
         //setControlState();
         //trySetFocus();

    }
   
    function trySetFocus()
    {
         var descObj=document.getElementById("<%=dDesc.ClientID %>");
         
         if(descObj!=null && descObj!=undefined)
         {
            descObj.focus();
         }
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
   
   
    function checkWarrantyCodePress(obj)
    { 
       var key = event.keyCode;

       if(!((key >= 48 && key <= 57) || (key >= 65 && key <= 90) || (key >= 97 && key <= 122) ))
       {  
           event.keyCode = 0;
       }
 
     }
     
    function DealHideWait()
    {
        HideWait();   
        getCustomerCmbObj().disabled = false; 
        getWarrantyFormatCmbObj().disabled = false; 
        getWarrantyTypeCmbObj().disabled = false;
    }

    function initControls()
    {
        getCustomerCmbObj().onchange=CustomerSelectOnChange;   

    }
    function CustomerSelectOnChange()
    {       
        document.getElementById("<%=btnCustomerChange.ClientID%>").click();
        ShowWait();
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


﻿<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ModelBOM.aspx.cs" Inherits="DataMaintain_ModelBOM" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<%--ITC-1136-0132--%>
<%--ITC-1136-0156--%>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<style type="text/css">

.iMes_div_MainTainEdit
{
    border: thin solid Black; 
    background-color: #99CDFF;
    margin:0 0 20 0;
}

.iMes_button_MainTainModelBOM1
{
	cursor:hand; 
	height: 23px;
	width:42px;
}

.iMes_button_MainTainModelBOM2
{
	cursor:hand; 
	height: 23px;
	width:78px;
}

.iMes_button_MainTainModelBOM3
{
	cursor:hand; 
	height: 23px;
	width:110px;
}

.Maintain_TreeNodeNomal
{
   font-weight:normal;
   font-size :small ;
   color:Black;
       
}

.Maintain_TreeNodeHighLight
{
   font-weight:bold; 
   font-size :small ; 
   color:Blue;  
}
</style>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/DataMaintain/Service/Family.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 99%; border: solid 0px red; margin: 0 auto;">
        
        <table width="100%">        
        <tr>
        <td width="20%">
      <%--  left table  --%>      
        <div id="div1"  style ="margin-top:3px" >
             <table width="100%" class="iMes_div_MainTainEdit" >
                <tr style ="height :24px">
                    <td style="width: 90px; ">
                        <asp:Label ID="lblModelPN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="76%">
                        <asp:TextBox ID="dModelPN" runat="server"   MaxLength="50"  Width="92%" CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)' ></asp:TextBox>
                    </td>    
        
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr style ="height :24px">
                    <td width="110px;">
                        <asp:Label ID="lblBOMTree" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="50%">  
                        <asp:TextBox ID="dSearch" runat="server"   MaxLength="50"  Width="95%" CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>                      
                    </td>    
                    <td >
                      <button id="btnFind" runat="server" class ="iMes_button_MainTainModelBOM1" disabled></button>
                    </td>           
                </tr>
             </table>  
        </div>     
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>  
        <div id="div2" style="height:370px;width:250px; overflow:scroll; margin-top:5px" >    
            <input type="hidden" id="dTreeIdIndexMax" runat="server" />     
            <iMESMaintain:MaintainTreeViewExt ID="dTree" runat="server" Width ="140%" Height="97%"  ShowLines="true" EnableClientScript="true" BorderWidth ="1" BorderColor=  "#8f8f8f" BackColor ="#D2D2D2">
            </iMESMaintain:MaintainTreeViewExt>
        </div>                  
         </ContentTemplate>
        </asp:UpdatePanel>  
                   
         <div id="div3" style ="margin-top:12px">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr style ="height :26px">
    
                        <td align="left">
                            <button id="btnExp" runat="server" onclick="if(clkButton())" class="iMes_button_MainTainModelBOM2"  onserverclick="btnExp_ServerClick"  ></button>
                        </td>
                        <td align="center">
                            <button id="btnRep" runat="server" onclick="if(clkButton())" class="iMes_button_MainTainModelBOM2" onserverclick="btnRep_ServerClick"   ></button>
                        </td>                          
                        <td align="right">
                        </td>   
                    </tr>
                    <tr style ="height :26px">
    
                        <td align="left">
                            <button id="btnParent" runat="server" onclick="clkButton()" class="iMes_button_MainTainModelBOM2"    ></button>
                        </td>
                        <td align="center">
                            <button id="btnAllParent" runat="server" onclick="clkButton()" class="iMes_button_MainTainModelBOM2"    ></button>
                        </td>                          
                        <td align="right">
                            <button id="btnToExcel" runat="server" onclick="clkButton()" class="iMes_button_MainTainModelBOM2" disabled visible=false></button>
                        </td>           
                    </tr>          
            </table> 
        </div>  
        
        <%--  left table end --%>      
        </td>
        <td width="80%">
        <%--  right table  --%>    
    
         <div id="div4">
            <table width="100%" >
                    <tr>
                        <td align="left">
                            <button id="btnShowBOM" runat="server" onclick="DealShowBom()" class="iMes_button_MainTainModelBOM2"   ></button>
                        </td>
                        <td align="center">
                            <button id="btnSaveAs" runat="server" onclick="clkButton()" class="iMes_button_MainTainModelBOM2"    ></button>
                        </td>          
                        <td align="center">
                            <button id="btnSaveAsDummy" runat="server" onclick="clkButton()" class="iMes_button_MainTainModelBOM2" style=" display:none; width:auto" ></button>
                        </td>                 
                        <td align="center">
        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>                        
                            <button id="btnApproveModel" runat="server" onclick="if(clkButton())" class="iMes_button_MainTainModelBOM3"    onserverclick="btnApproveModel_ServerClick" disabled visible=false ></button>
        </ContentTemplate>
        </asp:UpdatePanel>                            
                        </td>  
                        <td align="center">
                            <button id="btnRefreshMOBOM" runat="server" onclick="clkButton()" class="iMes_button_MainTainModelBOM3"  visible=false  ></button>
                        </td>    
                        <td align="center" width="50%">
        <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>  
                            <input type="hidden" id="dOldPartNo" runat="server" />                      
                            <asp:Label ID="lblApprove" runat="server" CssClass="iMes_label_13pt"></asp:Label>
        </ContentTemplate>
        </asp:UpdatePanel>                               
                        </td>                                                 
                    </tr>
                    </table> 
        </div> 
   
        
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
        <div id="div5" style="height:375px;width:100%;  overflow:scroll" >

                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%" RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="375px" Height="359px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState="false"
                        OnGvExtRowDblClick="dblClickGd(this)">
                      <Columns>   
                      <asp:TemplateField >   
                      <ItemTemplate>   
                      <asp:CheckBox id="gdCheckBox" runat="server" ></asp:CheckBox>
                      </ItemTemplate>   
                      </asp:TemplateField>   
                      </Columns> 
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div6" style="height:50px">
            <table width="100%" >
            <tr style="height:10px">
            <td>
            </td>
            </tr>  
                    <tr>
                        <td  width="70%">
                        </td>
                        <td width="14%" align="center">
                            <button id="btnDelete" runat="server" onclick="if(clkDelete())" onserverclick="btnDelete_ServerClick" class="iMes_button_MainTainModelBOM3"></button>
                        </td>                          
                        <td width="16%" align="center">
                            <button id="btnGroupNo" runat="server" onclick="if(clkButton())" class="iMes_button_MainTainModelBOM3"    onserverclick="btnGroupNo_ServerClick"></button>
                        </td>  
                    </tr>       
            </table> 

        </div>
        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
        <div id="div7">
            <table width="100%" >
                    <tr >
                        <td width="10%" style="background-color:#D2D2D2;">
                            <asp:Label ID="lblPartType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="55%" style="background-color:#D2D2D2;">
<asp:UpdatePanel ID="updatePanel_PartType" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                            Visible="true">
                            <ContentTemplate>
                            <asp:DropDownList ID="CmbMaintainPartTypeAll" runat="server" 
                                AutoPostBack="true" 
                                onselectedindexchanged="CmbMaintainPartTypeAll_SelectedIndexChanged" 
                                Width="30%">
                            </asp:DropDownList>
                            <asp:DropDownList ID="CmbMaintainPart" runat="server" 
                                AutoPostBack="true" onchange="ChangePartCode(this.options[this.options.selectedIndex].value)" 
                                Width="60%">
                            </asp:DropDownList>
                            <asp:Label ID="lblBomNode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
</ContentTemplate>
</asp:UpdatePanel>
                        <input type="hidden" id="changedPartTypeAllIndex" runat="server" />
                        <button id="btnChangedPartTypeAllIndex" runat="server" type="button" style="display:none" onserverclick ="btnChangedPartTypeAllIndex_ServerClick"> </button>
                        </td>
                        
                        <td width="10%" class="iMes_div_MainTainEdit" >
                            <asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="30%" class="iMes_div_MainTainEdit">
                            <asp:TextBox ID="dPartNo" runat="server"   MaxLength="255"   Width="96%" TabIndex="0" CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>
                        </td>
                        
                        <td align="center" class="iMes_div_MainTainEdit">
                            <button id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button_MainTainModelBOM2"    onserverclick="btnAdd_ServerClick"></button>
                        </td>
                    </tr>
                    <tr class="iMes_div_MainTainEdit" >

              <%--          <td width="150px;">
                            <asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="30%">
                            <asp:TextBox ID="dType" runat="server"   MaxLength="255"   Width="97%" TabIndex="0" SkinId="textBoxSkin" ></asp:TextBox>
                        </td>--%>
                        <td width="80px" >
                            <asp:Label ID="lblPriority" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="dPriority" runat="server"   MaxLength="255" Width="150px" TabIndex="0" SkinId="textBoxSkin" ></asp:TextBox>
                        </td> 
                        
                        <td >
                            <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="dQty" runat="server"   MaxLength="9"   Width="97%" TabIndex="0" CssClass="iMes_textbox_input_Yellow" onkeypress='checkPress(this)' style='ime-mode:disabled;' onpaste="return false" ></asp:TextBox>
                        </td>
                        
                        <td align="center">
                            <button id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button_MainTainModelBOM2"   onserverclick="btnSave_ServerClick"></button>
                        </td>           
                    </tr>                    
                     
            </table> 
        </div>  
        </ContentTemplate>
        </asp:UpdatePanel>  
            
        <%--  right table--end--%>   
        </td>
        </tr>            
        </table>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnPartNoChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnChangedPartTypeAllIndex" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnExp" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnRep" EventName="ServerClick" /> 
            <asp:AsyncPostBackTrigger ControlID="btnTreeNodeChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnGroupNo" EventName="ServerClick" />
        </Triggers>                      
            </asp:UpdatePanel>
           <input type="hidden" id="dOldId" runat="server" />
           <input type="hidden" id="dOldPartNoInTable" runat="server" />
           <input type="hidden" id="HiddenUserName" runat="server" />
           <input type="hidden" id="dSelectedTreeNodeId" runat="server" />
           <input type="hidden" id="gdCheckBoxStatusArray" runat="server" />
           <button id="btnPartNoChange" runat="server" type="button" style="display:none" onserverclick ="btnPartNoChange_ServerClick"> </button>
           <button id="btnTreeNodeChange" runat="server" type="button" style="display:none" onserverclick ="btnTreeNodeChange_ServerClick"> </button>
           

    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
    <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
        <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
    </table>
</div>
    <script language="javascript" type="text/javascript">
//    dOldPartNo,树上根节点PartNo
//    dOldPartNoInTable, 表格中的选中行PartNo
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
    var msg12="";
    var msg13="";
    var msg14="";
    var msg15="";



   var isDoing;
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
        msg10 ="<%=pmtMessage10%>";
        msg11 ="<%=pmtMessage11%>";
        msg12 ="<%=pmtMessage12%>";
        msg13 ="<%=pmtMessage13%>";
        msg14 ="<%=pmtMessage14%>";
        msg15 ="<%=pmtMessage15%>";
        
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        ShowRowEditInfo(null);
        isDoing=false;
        
    };
    
    function clkButton()
    {
       switch(event.srcElement.id)
       {
           case "<%=btnExp.ClientID %>":
           
 	            break; 
           case "<%=btnRep.ClientID %>":
 	            break;
           case "<%=btnParent.ClientID %>":

               
                if(currentHighLightObj==null)
                {

                    alert(msg1);
//                    alert("需要在树上选择当前节点");
                    return;
                }
                var dlgFeature = "dialogHeight:540px;dialogWidth:600px;center:yes;status:no;help:no";
                var currentPartNo=currentHighLightObjPartNo.trim();
                if(currentPartNo!="")
                {
                    currentPartNo=encodeURI(Encode_URL2(currentPartNo));
                    var dlgReturn=window.showModalDialog("ModelBOMParent.aspx?currentPartNo="+currentPartNo+"&flag=Parent", window, dlgFeature);
                }
 	            break;
           case "<%=btnAllParent.ClientID %>":
                
                if(currentHighLightObj==null)
                {
                    alert(msg1);
//                    alert("需要在树上选择当前节点");
                    return;
                }
                var dlgFeature = "dialogHeight:540px;dialogWidth:600px;center:yes;status:no;help:no";
                var currentPartNo=currentHighLightObjPartNo.trim();
                if(currentPartNo!="")
                {
                    currentPartNo=encodeURI(Encode_URL2(currentPartNo));
                    var dlgReturn=window.showModalDialog("ModelBOMParent.aspx?currentPartNo="+currentPartNo+"&flag=AllParent", window, dlgFeature);
                }
 	            break;
           case "<%=btnToExcel.ClientID %>":
                return false; 
 	            break;

           case "<%=btnSaveAs.ClientID %>":
                var dlgFeature = "dialogHeight:200px;dialogWidth:400px;center:yes;status:no;help:no";
                var oldPartNoValue=document.getElementById("<%=dOldPartNo.ClientID %>").value.trim().toUpperCase();
                if(oldPartNoValue!="")
                {
                    oldPartNoValue=encodeURI(Encode_URL2(oldPartNoValue));
                    var user=document.getElementById("<%=HiddenUserName.ClientID %>").value;
                    user=encodeURI(Encode_URL2(user));
                    var dlgReturn = window.showModalDialog("MaintainSaveAs.aspx?OldPartNo=" + oldPartNoValue + "&userName=" + user, window, dlgFeature);
                    if(dlgReturn!=null)
 	                {
 	                    document.getElementById("<%=dModelPN.ClientID %>").value=dlgReturn;
                        DealShowBom();
 	                }
                }
                else
                {
                    alert(msg2);
//                    alert("需要成功输入显示了[Model/PN]才能调用执行[Save As]功能");
                }
                
 	            break;
           case "<%=btnApproveModel.ClientID %>":
           
                 var ret = confirm(msg3);
//                 var ret = confirm("确定要Approve Model？");  //3
                 if (!ret) {
                     return false;
                 }
 
 	            break;
           case "<%=btnRefreshMOBOM.ClientID %>":

                var dlgFeature = "dialogHeight:475px;dialogWidth:449px;center:yes;status:no;help:no";
                var user=document.getElementById("<%=HiddenUserName.ClientID %>").value;
                user=encodeURI(Encode_URL2(user)); 
                
                var dlgReturn=window.showModalDialog("ModelBOMRefreshMOBOM.aspx?userName="+user, window, dlgFeature);
 
 	            break; 	  
           case "<%=btnGroupNo.ClientID %>":
 
                 if(checkCheckBoxSelect()==false)
                 {
                    alert(msg4);
//                    alert("需要通过复选框选择要设置公用料的项目");
                    return false;
                 }
//                 var ret = confirm("确定要设置共用料？");  //5
                 var ret = confirm(msg5);  //5
                 if (!ret) {
                     return false;
                 }
                 ShowWait();
                 break;
             case "<%=btnSaveAsDummy.ClientID %>":
                 var dlgFeature = "dialogHeight:200px;dialogWidth:400px;center:yes;status:no;help:no";
                 var custmer = '<%=Request["Customer"] %>';
                 var oldPartNoValue = document.getElementById("<%=dOldPartNo.ClientID %>").value.trim().toUpperCase();
                 if (oldPartNoValue != "") {
                     oldPartNoValue = encodeURI(Encode_URL2(oldPartNoValue));
                     var user = document.getElementById("<%=HiddenUserName.ClientID %>").value;
                     user = encodeURI(Encode_URL2(user));
                     var dlgReturn = window.showModalDialog("MaintainSaveAs.aspx?OldPartNo=" + oldPartNoValue + "&userName=" + user + "&IsSaveDummy=Y" + "&Customer=" + custmer, window, dlgFeature);
                     if (dlgReturn != null) {
                         document.getElementById("<%=dModelPN.ClientID %>").value = dlgReturn;
                         DealShowBom();
                     }
                 }
                 else {
                     alert(msg2);
                 }
                 break;    	                       	             	             	            
    	}
    	return true; 
    }
    
    function checkCheckBoxSelect()
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>")
        
        var statusArray=new Array();
        var isChecked=false;
        var iCount=0;
        for(var i=0;i<gdObj.rows.length;i++)  //hidRecordCount
        {
            if(gdObj.rows[i].cells[0].children[0]!=null && gdObj.rows[i].cells[0].children[0].children[0]!=null)
            {
                if(gdObj.rows[i].cells[0].children[0].children[0].checked==true )
                {
                    gdObj.rows[i].cells[12].innerText="True";
                    isChecked=true;
                    statusArray[iCount]=gdObj.rows[i].cells[11].innerText;
                    iCount=iCount+1;
                }
                else
                {
                    gdObj.rows[i].cells[12].innerText="False";
                }
            }
        }        
        var statusStr=statusArray.join(",");
        document.getElementById("<%=gdCheckBoxStatusArray.ClientID %>").value=statusStr;        
        return isChecked;
    }
    
    function clkDelete()
    {
         if(checkCheckBoxSelect()==false)
         {
            alert(msg6);
//            alert("需要通过复选框选择要删除的项目");
            return false;
         }
       
//         var ret = confirm("确定要删除这些记录么？"); //7
         var ret = confirm(msg7); //7
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
       return check();
   }
   
   function check()
   {
       var partNo=document.getElementById("<%=dPartNo.ClientID %>").value.trim();
        
       if(partNo=="")
       {
            alert(msg8);
//            alert("需要输入[Part No]");
            return false;
       }
       
       var qty=document.getElementById("<%=dQty.ClientID %>").value.trim();
        
       if(qty=="")
       {
            alert(msg9);
//            alert("需要输入[Qty]");
            return false;
       }
       
       if(parseInt(qty).toString()=="0" )
       {
            alert(msg10);
//            alert("[Qty]应该是大于0的数");
            return false;
       }
       
//       var priority=document.getElementById("<%=dPriority.ClientID %>").value.trim();
//        
//       if(priority=="")
//       {
//            alert(msg11);
////            alert("需要输入[Priority]");
//            return false;
//       }
       ShowWait();
       return true;
   }
   
   function clkAdd()
   {
       if(currentHighLightObj==null)
       {
           alert(msg12);
//           alert("需要选择树上的父节点");
           return false;
       }
       return check();
   }   
    
    var iSelectedRowIndex=null;
    function clickTable(con)
    {
//         var selectedRowIndex = con.index;
//         setRowSelectedByIndex_<%=gd.ClientID%>(con.index, false, "<%=gd.ClientID%>");         
//         ShowRowEditInfo(con);       
        setGdHighLight(con);
        ShowRowEditInfo(con);
    }
    
    function setGdHighLight(con)
    {
        if((iSelectedRowIndex!=null) && (iSelectedRowIndex!=parseInt(con.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex,false, "<%=gd.ClientID %>");                
        }        
        setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
        iSelectedRowIndex=parseInt(con.index, 10);    
    }
    
    function setNewItemValue()
    {
         document.getElementById("<%=dPartNo.ClientID %>").value="";  
         document.getElementById("<%=dQty.ClientID %>").value=""; 
         document.getElementById("<%=dPriority.ClientID %>").value="";

         document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
         
//         document.getElementById("<%=btnDelete.ClientID %>").disabled=true;          
    }
    
    // SELECT a.Component as Material, b.PartType, a.Quantity, 
    //a.Alternative_item_group, a.Priority 
    //,a.[Editor],a.[Cdt],a.[Udt],a.ID
    function ShowRowEditInfo(con)
    {

         if(con==null)
         {
            setNewItemValue();
            return;    
         }         
         
         document.getElementById("<%=dPartNo.ClientID %>").value=con.cells[1].innerText.trim();  
         document.getElementById("<%=dQty.ClientID %>").value=con.cells[5].innerText.trim(); 
         document.getElementById("<%=dPriority.ClientID %>").value=con.cells[7].innerText.trim();  
        
         var currentId=con.cells[11].innerText.trim(); 
         document.getElementById("<%=dOldId.ClientID %>").value = currentId;
         document.getElementById("<%=dOldPartNoInTable.ClientID %>").value = con.cells[1].innerText.trim(); 
  
         if(currentId=="")
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
            //document.getElementById("<%=btnDelete.ClientID %>").disabled=true;
            setNewItemValue();
         }
         else
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            //document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
         }
         //setControlState();
         trySetFocus();

         var NodeType = con.cells[2].innerText.trim();
         var NodeDescr = con.cells[3].innerText.trim();
         var ddlType = document.getElementById("<%=CmbMaintainPartTypeAll.ClientID %>");
         var FoundType = false;
         for (var i = 0; i < ddlType.options.length; i++) {
             if (ddlType.options[i].value == NodeDescr) {
                 //ddlType.selectedIndex = i;
                 document.getElementById("<%=changedPartTypeAllIndex.ClientID %>").value = i;
                 FoundType = true;
                 break;
             }
         }
         if (!FoundType) {
             for (var i = 0; i < ddlType.options.length; i++) {
                 if (ddlType.options[i].value == NodeType) {
                     //ddlType.selectedIndex = i;
                     document.getElementById("<%=changedPartTypeAllIndex.ClientID %>").value = i;
                     FoundType = true;
                     break;
                 }
             }
         }
         if (FoundType) {
             document.getElementById("<%=btnChangedPartTypeAllIndex.ClientID %>").click();
         }

    }

   
    function trySetFocus()
    {
         var descObj=document.getElementById("<%=dPartNo.ClientID %>");
         
         if(descObj!=null && descObj!=undefined)
         {
            descObj.focus();
         }
    }
   
    function AddUpdateComplete(id)
    {
   
        
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=0;i<gdObj.rows.length;i++)
        {
           if(gdObj.rows[i].cells[11].innerText==id)
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
            //去掉标题行
            //selectedRowIndex=selectedRowIndex-1;
//            setRowSelectedByIndex_<%=gd.ClientID%>(selectedRowIndex, false, "<%=gd.ClientID%>");
          
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
            
        }        
        
    }
   
   
    function checkPress(obj)
    { 
       var key = event.keyCode;

       if(!(key >= 48 && key <= 57))
       {  
           event.keyCode = 0;
       }
 
     }

    function DealShowBom()
    {
        var value=document.getElementById("<%=dModelPN.ClientID %>").value.trim();
        
        if(value=="")
        {
            alert(msg13);
//            alert("需要输入[Model/PN]");
            return;
        }
                ShowWait();
        document.getElementById("<%=btnPartNoChange.ClientID %>").click(); 
    }
  
    function OnKeyPress(obj)
    {
        var key = event.keyCode;

        if (key==13)//enter
        {
            if(event.srcElement.id=="<%=dModelPN.ClientID %>")
            {
                DealShowBom();
            }
        }       
    }

    var currentHighLightObj=null;
    var currentHighLightObjPartNo="";
    function OnTreeNodeChange(value, partNo)
    {

        if(currentHighLightObj!=null)
        {
            currentHighLightObj.style.color="Black";
            currentHighLightObj.style.fontWeight="normal";
        }
//        var obj=event.srcElement;
        var spanId="dNode"+value;
        var obj=document.getElementById(spanId);
        obj.style.color="Blue";
        obj.style.fontWeight="bold"; 
        currentHighLightObj= obj;
        currentHighLightObjPartNo=partNo;
        document.getElementById("<%=dSelectedTreeNodeId.ClientID %>").value=value;
        ShowWait();
        document.getElementById("<%=btnTreeNodeChange.ClientID %>").click();  
              
    }
    
    //从后台增、删、改树上的节点后刷新页面重置高亮选择行
    function HighLightTreeNode(value)
    {
        if(currentHighLightObj!=null)
        {
            currentHighLightObj.style.color="Black";
            currentHighLightObj.style.fontWeight="normal";
        }
        var spanId="dNode"+value;
        var obj=document.getElementById(spanId);
        obj.style.color="Blue";
        obj.style.fontWeight="bold"; 
        currentHighLightObj= obj;
    }
    
    function initSelectTreeNode()
    {
        currentHighLightObj=null;
        currentHighLightObjPartNo="";      
    }
    
    function OnSelectNode(value)
    {
//        document.getElementById("<%=dSelectedTreeNodeId.ClientID %>").value=value;
//        document.getElementById("<%=btnTreeNodeChange.ClientID %>").click();
       
    }
    
    function showSelectedNode()
    { 
    //LoadEvent();
//　    try 
//　    { 
//　      
//　    var elem = document.getElementById('dTree_SelectedNode'); 
//　　    if(elem != null ) 
//　　    { 
//　　　    var node = document.getElementById(elem.value); 
//　　　    if(node != null) 
//　　　    { 
//　　　　    node.scrollIntoView(true); 
//　　　　    updatePanel.scrollLeft = 0; 
//　　　    } 
//　　    } 
//　    } 
//　    catch(oException) 
//　    {}
}
function dblClickGd(row) {
    if ('' != row.cells[1].innerText.replace(/(^\s*)|(\s*$)/g, "")) {
        var dlgFeature = "dialogHeight:505px;dialogWidth:903px;center:yes;status:no;help:no";
        window.showModalDialog("ModelBOM_PartInfo.aspx?username=<%=Master.userInfo.UserId%>&partNo=" + row.cells[1].innerText + "&nodeType=" + row.cells[3].innerText, window, dlgFeature);
    }
}
function getPartTypeObj() {
    return document.getElementById("<%=CmbMaintainPartTypeAll.ClientID %>");
}
function ChangePartCode(v) {
    document.getElementById("<%=dPartNo.ClientID %>").value = v;
}
    </script>
</asp:Content>


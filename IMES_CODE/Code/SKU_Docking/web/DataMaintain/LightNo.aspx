<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="LightNo.aspx.cs" Inherits="DataMaintain_LightNo" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
<style type="text/css">

.iMes_buttonShort
{
	border:Gray 1px ridge;
	padding:2px;
	margin-left:2px;
	margin-right:2px;
	cursor:hand; 
	height: 20px;
	width:90%;
	font-size: 9pt;
	text-align:center;
	font-family:Verdana;
    font-weight:lighter;
	vertical-align:middle;
    color: black;
	FILTER:progid:DXImageTransform.Microsoft.Gradient(GradientType=0,StartColorStr=#CCCCCC, EndColorStr=#FFFFFF);   
}

</style>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblKittingType" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td  width="78%">
                       <iMESMaintain:CmbMaintainLightNoKittingType runat="server" ID="cmbMaintainLightNoKittingType" Width="30%" ></iMESMaintain:CmbMaintainLightNoKittingType>
                    </td> 
                </tr>
                <tr style="height:3px;"><td></td></tr>
                <tr>
                    <td style="width: 90%; padding-left: 2px;height:152px" colspan ="2">
                       
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                        <ContentTemplate>
                        <input id="hidRecordCount1" type="hidden" runat="server" />
                        <iMES:GridViewExt ID="gd1" runat="server" AutoGenerateColumns="true"  RowStyle-Height="20"  
                            GvExtWidth="100%" GvExtHeight="171px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" Width="100%" 
                            OnGvExtRowClick='if(typeof(clickTable1)=="function") clickTable1(this)' onrowdatabound="gd_RowDataBound1" EnableViewState="false">
                        </iMES:GridViewExt>
                        </ContentTemplate>
                        </asp:UpdatePanel> 
                        
                    </td>    
                 </tr>
             </table> 
        </div>
        <%--<div id="div3" style="width :100%;height:6px;"></div>--%>
        <div id="div2" style="margin-top:0px;">
                                   
             <table width="100%" border="0" >  
                <tr>
                <td align="left" style="width: 100px;" >
                    <asp:Label ID="lblList2" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="55%">  
<%--                     <asp:TextBox ID="dSearch" runat="server"   MaxLength="50"  Width="70%" SkinId="textBoxSkin" onkeypress='OnKeyPress(this)'></asp:TextBox>                      
--%>                </td> 
                
                <td width="30%" align="right">
                   
                   <input type="button" id="btnDelete" class="iMes_button" runat="server" onclick="if(clkButton())" onserverclick="btnDelete_ServerClick"></input>
                </td>    
                </tr> 
                </table>
          </div>                
                 
         <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
         <ContentTemplate>
         <div id="div4" style="height:366px">
         <input id="hidRecordCount2" type="hidden" runat="server" />
         <iMES:GridViewExt ID="gd2" runat="server" AutoGenerateColumns="true" Width="175%" RowStyle-Height="20" 
             GvExtWidth="100%" GvExtHeight="169px" AutoHighlightScrollByValue ="true" 
                HighLightRowPosition="3" 
                OnGvExtRowClick='if(typeof(clickTable2)=="function") clickTable2(this)' onrowdatabound="gd_RowDataBound2" EnableViewState="false" >
         </iMES:GridViewExt>
         </div>
         </ContentTemplate>
         </asp:UpdatePanel> 
         <div id="div3" >             
             <table width="100%" class="iMes_div_MainTainEdit">
                  <tr>
                    <td style="width:9%; padding-left:2px;">
                        <asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt" Width ="100%"></asp:Label>
                    </td>
                    <td width="24%" >
                        <asp:TextBox ID="dPartNo" runat="server" MaxLength="20" Width="96%" SkinId="textBoxSkin"  ></asp:TextBox>
                    </td>    
                    <td width="10%" >
                        <asp:Label ID="lblSubstitution" runat="server" CssClass="iMes_label_13pt"  Width ="100%"></asp:Label>
                    </td>
                    <td width="24%" >                        
                        <asp:TextBox ID="dSubstitution" runat="server"   MaxLength="20"  Width="95%" CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>
                    </td>
                     <td width="9%">
                        <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt" Width ="100%"></asp:Label>
                    </td>
                    <td width="24%" >                        
                        <asp:TextBox ID="dRemark" runat="server"   MaxLength="20"  Width="95%" SkinId="textBoxSkin" ></asp:TextBox>
                    </td>                    
                          
                  </tr>
                  <tr>
                    <td >
                        <asp:Label ID="lblLightNo" runat="server" CssClass="iMes_label_13pt"  Width ="100%"></asp:Label>
                    </td>
                    <td >
                        <asp:TextBox ID="dLightNo" runat="server"   MaxLength="4"   Width="96%"  SkinId="textBoxSkin" onkeypress='CheckInput(this)' style='ime-mode:disabled;' onpaste="return false"></asp:TextBox>
                    </td>    
                    <td >
                        <asp:Label ID="lblSafeStock" runat="server" CssClass="iMes_label_13pt"  Width ="100%"></asp:Label>
                    </td>
                    <td >                        
                        <asp:TextBox ID="dSafeStock" runat="server"   MaxLength="9"  Width="95%" SkinId="textBoxSkin" onkeypress='CheckInput(this)' style='ime-mode:disabled;' onpaste="return false"></asp:TextBox>
                    </td>
                    <td >
                        <asp:Label ID="lblStation" runat="server" CssClass="iMes_label_13pt" Width ="100%"></asp:Label>
                    </td>
                    <td  >
                       <iMESMaintain:CmbMaintainLightNoStation runat="server" ID="cmbMaintainLightNoStation" Width="98%" ></iMESMaintain:CmbMaintainLightNoStation>
                    </td> 
                  </tr>
                  <tr>
                    <td >
                        <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"  Width ="100%"></asp:Label>
                    </td>
                    <td >
                        <asp:TextBox ID="dQty" runat="server"   MaxLength="9"   Width="96%" SkinId="textBoxSkin" onkeypress='CheckInput(this)' style='ime-mode:disabled;' onpaste="return false"></asp:TextBox>
                    </td>    
                    <td >
                        <asp:Label ID="lblMaxStock" runat="server" CssClass="iMes_label_13pt"  Width ="100%"></asp:Label>
                    </td>
                    <td >                        
                        <asp:TextBox ID="dMaxStock" runat="server"   MaxLength="9"  Width="95%" SkinId="textBoxSkin" onkeypress='CheckInput(this)' style='ime-mode:disabled;' onpaste="return false" ></asp:TextBox>
                    </td>
                    <td >
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt" Width ="50px"></asp:Label>
                    </td>
                    <td >                         
                        <table width="100%" >
                        <tr style="width:100%"><td align="left" style="width:36%">
                        <iMESMaintain:CmbMaintainLightNoPdLine  runat="server" ID="cmbMaintainLightNoPdLine" Width="90%"   IsAddEmpty="true"  Stage="FA" ></iMESMaintain:CmbMaintainLightNoPdLine >
                        </td><td style="width:32%">
                        <input type="button" id="btnAdd" runat="server" onclick="if(clkButton())" class="iMes_buttonShort" onserverclick="btnAdd_ServerClick" />
                        </td ><td align="right" style="width:32%" >
                        <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_buttonShort" onserverclick="btnSave_ServerClick" />
                        </td></tr></table>
                    </td>                         
                  </tr> 
            </table>  
                  
        </div>
                
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnList1Change" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnComBoxKittingTypeChange" EventName="ServerClick" />
        </Triggers>                      
            </asp:UpdatePanel>
           <input type="hidden" id="HiddenUserName" runat="server" />
           <input type="hidden" id="dOldCode" runat="server" />
           <input type="hidden" id="dOldIsLine" runat="server" />
           <input type="hidden" id="dOldId" runat="server" />
           <input type="hidden" id="dTableHeight" runat="server" />  
           <button id="btnList1Change" runat="server" type="button" style="display:none" onserverclick ="btnList1Change_ServerClick"> </button>
           <button id="btnComBoxKittingTypeChange" runat="server" type="button" style="display:none" onserverclick ="btnComBoxKittingTypeChange_ServerClick"> </button>


    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px//存TMPKIT表中的Type, char(4) solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>        
    
    <script language="javascript" type="text/javascript">    
      
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function(sender, e)
    {
        e.set_errorHandled(true); //表示自定义显示错误, 将默认的alert提示禁止掉.
        if(e.get_error()!=null && e.get_error()!="")
        {
            alert("Time out, please login again.");
            DealHideWait();                
        }
    });
      
      
    var msg1="";
    var msg2="";
    var msg3="";
    var msg4="";
    var msg5="";
    var msg6="";
    var msg7="";
    
    var KittingTypeFAKitting;
    var KittingTypePAKKitting;
    var KittingTypeFALabel;
    var KittingTypeFALabel; 
    
    var btnSaveEnableBylist1=false;
    var btnSaveEnableBylist2=false;
    
    var KittingTypeValue;
       
    window.onload = function()
    {
    
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";
        msg6 ="<%=pmtMessage6%>";
        msg7 ="<%=pmtMessage7%>";
        
        
        KittingTypeFAKitting="<%=KittingTypeFAKitting%>"; 
        KittingTypePAKKitting="<%=KittingTypePAKKitting%>"; 
        KittingTypeFALabel="<%=KittingTypeFALabel%>"; 
        KittingTypeFALabel="<%=KittingTypePAKLabel%>"; 

        
        ShowRowEditInfo2(null);
        initControls();
        DealKitTypeFA();
        //ShowRowEditInfo1(null);
        //设置表格的高度  
        resetTableHeight();
    };
    
    //设置表格的高度  
    function resetTableHeight()
    {    
        //动态调整表格的高度
        var adjustValue=58;     
        var marginValue=12; 
        var tableHeigth=300;
        //var tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
        try{
            tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div2.offsetHeight-div3.offsetHeight-adjustValue;
        }
        catch(e){
        
            //ignore
        }                
        //为了使表格下面有写空隙
        var extDivHeight=tableHeigth+marginValue;
        div4.style.height=extDivHeight+"px";
        document.getElementById("div_<%=gd2.ClientID %>").style.height=tableHeigth+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
    }
    
    //重选择kittingType,及上面的表格时调用
    function DealKitTypeFA()//ok
    {
        ShowRowEditInfo1(null);
        document.getElementById("<%=dSubstitution.ClientID%>").disabled = false;
        document.getElementById("<%=dRemark.ClientID%>").disabled = false;
        document.getElementById("<%=dSafeStock.ClientID%>").disabled = false;
        document.getElementById("<%=dQty.ClientID%>").disabled = false;
        document.getElementById("<%=dMaxStock.ClientID%>").disabled = false;
        
         
        getMaintainLightNoStationCmbObj().selectedIndex=0;
        getMaintainLightNoStationCmbObj().disabled = true;
        //getMaintainLightNoPartTypeCmbObj().disabled = false;
        
        
        
    }
     
    //重选择kittingType,及上面的表格时调用
    function DealKitTypePAK()//ok
    {
        ShowRowEditInfo1(null);
        document.getElementById("<%=dSubstitution.ClientID%>").disabled = false;
        document.getElementById("<%=dRemark.ClientID%>").disabled = false;
        document.getElementById("<%=dSafeStock.ClientID%>").disabled = false;
        document.getElementById("<%=dQty.ClientID%>").disabled = false;
        document.getElementById("<%=dMaxStock.ClientID%>").disabled = false;
       
        
        getMaintainLightNoStationCmbObj().disabled = false;
        //getMaintainLightNoPdLineCmbObj().disabled = false;
        //getMaintainLightNoPartTypeCmbObj().disabled = true;
        
        
    } 
    
    //重选择kittingType,及上面的表格时调用
    function DealKitTypeFALabel()
    {
        ShowRowEditInfo1(null);
        document.getElementById("<%=dSubstitution.ClientID%>").disabled = true;
        document.getElementById("<%=dRemark.ClientID%>").disabled = true;
        document.getElementById("<%=dSafeStock.ClientID%>").disabled = true;
        document.getElementById("<%=dQty.ClientID%>").disabled = true;
        document.getElementById("<%=dMaxStock.ClientID%>").disabled = true;
      
        
        getMaintainLightNoStationCmbObj().selectedIndex=0;
        getMaintainLightNoStationCmbObj().disabled = true;
        //getMaintainLightNoPdLineCmbObj().disabled = true;
        //getMaintainLightNoPartTypeCmbObj().disabled = true;   
        

    }
    
    //重选择kittingType,及上面的表格时调用
    function DealKitTypePAKLabel()//ok
    {
        ShowRowEditInfo1(null);
        document.getElementById("<%=dSubstitution.ClientID%>").disabled = true;
        document.getElementById("<%=dRemark.ClientID%>").disabled = true;
        document.getElementById("<%=dSafeStock.ClientID%>").disabled = true;
        document.getElementById("<%=dQty.ClientID%>").disabled = true;
        document.getElementById("<%=dMaxStock.ClientID%>").disabled = true;
      
        getMaintainLightNoStationCmbObj().selectedIndex=0;
        getMaintainLightNoStationCmbObj().disabled = true;
        //getMaintainLightNoPdLineCmbObj().disabled = true;
        //getMaintainLightNoPartTypeCmbObj().disabled = true;
        
              
    }
    
    
    function checkSetEnableCombox()
    {
        KittingTypeValue=getMaintainLightNoKittingTypeCmbObj().value.trim();
        if(KittingTypeValue==KittingTypeFAKitting)
        {
            DealKitTypeFA();
        }
        else if(KittingTypeValue==KittingTypePAKKitting)
        {
            DealKitTypePAK();
        }
        else if(KittingTypeValue==KittingTypeFALabel)
        {
            DealKitTypeFALabel();
        }
        else //KittingTypePAKLabel
        {
            DealKitTypePAKLabel();
        }
    
    }
    
    function initControls()//ok
    {
        getMaintainLightNoKittingTypeCmbObj().onchange=LightNoKittingTypeOnChange; 
    }
    
    function LightNoKittingTypeOnChange()//ok
    {       
        document.getElementById("<%=btnComBoxKittingTypeChange.ClientID%>").click();
        ShowWait();
    }
    
    function isRowSelect()
    {
        if(iSelectedRowIndex1==null)
        {
            return false;
        }  
        var gdObj=document.getElementById("<%=gd1.ClientID %>");
        //因为有头
        var tmpIndex=iSelectedRowIndex1+1;
        var selectCode=gdObj.rows[tmpIndex].cells[0].innerText.trim();
        if(selectCode=="")
        {
            return false;
        }
        return true;
    }
    
    function saveCheck()
    {
//        if(isRowSelect()==false)
//        {
//            //alert("需要选择Kitting Code");
//            alert(msg1);
//            return false;
//        }
//        
        var partNo=document.getElementById("<%=dPartNo.ClientID %>").value.trim();
        if(partNo=="")
        {
          //alert("请输入Part No!");
            alert(msg1);
            trySetFocusPartNo();
            return false;
        } 
        
        var lightNo=document.getElementById("<%=dLightNo.ClientID %>").value.trim(); 
        if(lightNo=="")   
        {
            alert(msg2);
//            alert("请输入Light No!");
            trySetFocusLightNo();
            return false;
        }        

        var qty=document.getElementById("<%=dQty.ClientID %>").value.trim(); 
        if(qty!="" &&  parseInt(qty).toString()=="0" )   
        {
            alert(msg5);
            //[Qty] should be greater than 0!
            trySetFocusQty();
            return false;
        } 

        var qty=document.getElementById("<%=dQty.ClientID %>").value.trim(); 
        if(qty=="")   
        {
            document.getElementById("<%=dQty.ClientID %>").value="1";
        } 
      
          
        var safetyStock=document.getElementById("<%=dSafeStock.ClientID %>").value.trim(); 
        if(safetyStock=="")
        {
            document.getElementById("<%=dSafeStock.ClientID %>").value="0";
        }
        
        var maxStock=document.getElementById("<%=dMaxStock.ClientID %>").value.trim(); 
        if(maxStock=="")
        {
            document.getElementById("<%=dMaxStock.ClientID %>").value="0";
        }        
       
        return true;
    }
    
    
    
//    function InfoErrorPartNo()
//    {   
//        //alert("请输入正确的Part No!");
//        trySetFocusPartNo();
//          
//    }

//    function trySetFocusMaxStock()
//    {
//         var descObj=document.getElementById("<%=dMaxStock.ClientID %>");
//         
//         if(descObj!=null && descObj!=undefined)
//         {
//            descObj.focus();
//         }
//    }
    
//    function trySetFocusSafetyStock()
//    {
//         var descObj=document.getElementById("<%=dSafeStock.ClientID %>");
//         
//         if(descObj!=null && descObj!=undefined)
//         {
//            descObj.focus();
//         }
//    }
    
    function trySetFocusQty()
    {
         var descObj=document.getElementById("<%=dQty.ClientID %>");
         
         if(descObj!=null && descObj!=undefined && descObj.disabled!=true)
         {
            descObj.focus();
         }
    }
     
    function trySetFocusLightNo()
    {
         var descObj=document.getElementById("<%=dLightNo.ClientID %>");
         
         if(descObj!=null && descObj!=undefined && descObj.disabled!=true)
         {
            descObj.focus();
         }
    }
    
    function trySetFocusPartNo() //ok
    {
         var descObj=document.getElementById("<%=dPartNo.ClientID %>");
         
         if(descObj!=null && descObj!=undefined && descObj.disabled!=true)
         {
            descObj.focus();
         }
    }
    

        
    function clkButton()
    {
       //ShowInfo("");
       switch(event.srcElement.id)
       {
           
           case "<%=btnSave.ClientID %>":
                
                if(saveCheck()==false)
                {                
                    return false;
                }
 	            break;
           case "<%=btnAdd.ClientID %>":
                
                if(saveCheck()==false)
                {                
                    return false;
                }
 	            break; 	            
 	            
           case "<%=btnDelete.ClientID %>":

//                var ret = confirm("您确实要删除该记录吗？");  
                var ret = confirm(msg3);  
                if (!ret) {
                    return false;
                }               
                         
 	            break;
	                       	             	             	            
    	}   
        ShowWait();
        return true;
    }
    
    
    var iSelectedRowIndex1=null;
    
    //手动点击时调用
    function clickTable1(con)  //ok
    {
        setGdHighLight1(con);        
        ShowRowEditInfo1(con);
        document.getElementById("<%=btnList1Change.ClientID %>").click();
//        ShowInfo("");
        ShowWait();
    }
    
    function SetButtonSaveState()  //ok
    {
       if(btnSaveEnableBylist1==true && btnSaveEnableBylist2==true)
       {
            document.getElementById("<%=btnSave.ClientID %>").disabled=false; 
       }
       else
       {
            document.getElementById("<%=btnSave.ClientID %>").disabled=true; 
       }    
    }
    
    function ShowRowEditInfo1(con) //ok
    {
         if(con==null)
         {
            document.getElementById("<%=btnAdd.ClientID %>").disabled=true; 
            btnSaveEnableBylist1=false;
            SetButtonSaveState();

            //document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
            document.getElementById("<%=dOldCode.ClientID %>").value = "";
            document.getElementById("<%=dOldIsLine.ClientID %>").value = "";
            SetLineState();
            
            return;    
         }
    
         var currentCode=con.cells[0].innerText.trim(); 
         document.getElementById("<%=dOldCode.ClientID %>").value = currentCode;         
         document.getElementById("<%=dOldIsLine.ClientID %>").value = con.cells[2].innerText.trim(); 
         
         if(currentCode=="")
         {
            document.getElementById("<%=btnAdd.ClientID %>").disabled=true; 
            btnSaveEnableBylist1=false;
            SetButtonSaveState();
            //document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
         }
         else
         {
            document.getElementById("<%=btnAdd.ClientID %>").disabled=false; 
            btnSaveEnableBylist1=true;
            SetButtonSaveState(); 
            //document.getElementById("<%=btnDelete.ClientID %>").disabled=false; 
         }
         SetLineState();
         //trySetFocusPartNo();   
    
    }
    
    function setGdHighLight1(con) //ok
    {
        if((iSelectedRowIndex1!=null) && (iSelectedRowIndex1!=parseInt(con.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex1,false, "<%=gd1.ClientID %>");                
        }        
        setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd1.ClientID %>");
        iSelectedRowIndex1=parseInt(con.index, 10);    
        
    }
    
    var iSelectedRowIndex2=null;
    function clickTable2(con)
    {
        setGdHighLight2(con);        
        ShowRowEditInfo2(con);
    }
    
    function setGdHighLight2(con)
    {
        if((iSelectedRowIndex2!=null) && (iSelectedRowIndex2!=parseInt(con.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex2,false, "<%=gd2.ClientID %>");                
        }        
        setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd2.ClientID %>");
        iSelectedRowIndex2=parseInt(con.index, 10);    
    }
    
    function ShowRowEditInfo2(con)
    {   
          
    
         if(con==null)
         {
             document.getElementById("<%=dPartNo.ClientID %>").value ="";
             document.getElementById("<%=dLightNo.ClientID %>").value ="";
             document.getElementById("<%=dQty.ClientID %>").value ="1";
             document.getElementById("<%=dSubstitution.ClientID %>").value="";
             document.getElementById("<%=dSafeStock.ClientID %>").value ="0";  
             document.getElementById("<%=dMaxStock.ClientID %>").value="0";     
             document.getElementById("<%=dRemark.ClientID %>").value="";
             document.getElementById("<%=dOldId.ClientID %>").value="";
             
             getMaintainLightNoStationCmbObj().selectedIndex=0;
             getMaintainLightNoPdLineCmbObj().selectedIndex=0;
            
            btnSaveEnableBylist2=false;
            SetButtonSaveState();                   
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
            return;    
         }
    
        //select distinct b.PartNo, b.Tp, b.Line, b.Station, convert(int,b.LightNo), b.Qty, b.Sub,b.Safety_Stock, b.Max_Stock, b.Remark,b.Editor,b.Cdt,b.Udt,b.ID 
         var currentId=con.cells[13].innerText.trim();
         document.getElementById("<%=dPartNo.ClientID %>").value =con.cells[0].innerText.trim();
         document.getElementById("<%=dLightNo.ClientID %>").value = con.cells[4].innerText.trim();
         document.getElementById("<%=dQty.ClientID %>").value = con.cells[5].innerText.trim();
         document.getElementById("<%=dSubstitution.ClientID %>").value = con.cells[6].innerText.trim(); 
         document.getElementById("<%=dSafeStock.ClientID %>").value = con.cells[7].innerText.trim();        
         document.getElementById("<%=dMaxStock.ClientID %>").value = con.cells[8].innerText.trim();         
         document.getElementById("<%=dRemark.ClientID %>").value = con.cells[9].innerText.trim();
         document.getElementById("<%=dOldId.ClientID %>").value = currentId;
         
         getMaintainLightNoPdLineCmbObj().value=con.cells[2].innerText.trim();
         getMaintainLightNoStationCmbObj().value =con.cells[3].innerText.trim();
          
         if(currentId=="")
         {
             document.getElementById("<%=btnDelete.ClientID %>").disabled=true;
             btnSaveEnableBylist2=false;
             SetButtonSaveState();
            
             document.getElementById("<%=dQty.ClientID %>").value ="1";
             document.getElementById("<%=dSafeStock.ClientID %>").value ="0";  
             document.getElementById("<%=dMaxStock.ClientID %>").value="0";     
               
         }
         else
         {
            document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
            btnSaveEnableBylist2=true;
            SetButtonSaveState(); 
         }
         //trySetFocusPartNo();
    }
    
    
    function AddUpdateComplete(id)
    {
        
        var gdObj=document.getElementById("<%=gd2.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(gdObj.rows[i].cells[13].innerText==id)
           {
               selectedRowIndex=i;  
               break;
           }        
        }
        
        if(selectedRowIndex<0)
        {
            ShowRowEditInfo2();
            //trySetFocusPartNo();
            return;
        }
        else
        {            
            var con=gdObj.rows[selectedRowIndex];
            setGdHighLight2(con);
            setSrollByIndex(iSelectedRowIndex2, true, "<%=gd2.ClientID%>");            
            ShowRowEditInfo2(con);
        }        
    }
   
    function DeleteComplete()
    {   
        ShowRowEditInfo2(null);
    }

    function CheckInput()
    {
       var key = event.keyCode;

       if(!(key >= 48 && key <= 57))
       {  
           event.keyCode = 0;
       }
       
    }
    
//    function OnKeyPress(obj)
//    {
//        var key = event.keyCode;
//        
//        if (key==13)//enter
//        {
//            if(event.srcElement.id=="=dSearch.ClientID ")
//            {
//                var value=document.getElementById(" =dSearch.ClientID  ").value.trim();
//                if(value!="")
//                {
//                    findLightNo(value, true);
//                }
//            }
//        }       

//    }
//    
    function findLightNo(searchValue, isNeedPromptAlert)
    {
        var gdObj=document.getElementById("<%=gd2.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(searchValue.trim()!="" && gdObj.rows[i].cells[0].innerText.indexOf(searchValue)==0)
           {
               selectedRowIndex=i;
               break;  
           }        
        }
        
        if(selectedRowIndex<0)
        {
            if(isNeedPromptAlert==true)
            {
//                alert("Cant find matched Light No.");    //10  
                 alert(msg4);  
            }
            return;
        }
        else
        {            
            var con=gdObj.rows[selectedRowIndex];
            setGdHighLight2(con);
            setSrollByIndex(iSelectedRowIndex2, true, "<%=gd2.ClientID%>");
            ShowRowEditInfo2(con);
            
        }    
    }
    
    
    function DealHideWait()
    {
        HideWait();  
        getMaintainLightNoKittingTypeCmbObj().disabled = false; 
        
        var kittingType=getMaintainLightNoKittingTypeCmbObj().value.trim();
        
        if(kittingType==KittingTypeFAKitting)
        {
            getMaintainLightNoStationCmbObj().selectedIndex=0;
            getMaintainLightNoStationCmbObj().disabled = true;
        }
        else if(kittingType==KittingTypePAKKitting)
        {
            getMaintainLightNoStationCmbObj().disabled = false;
        }
        else if(kittingType==KittingTypeFALabel)
        {
            getMaintainLightNoStationCmbObj().selectedIndex=0;
            getMaintainLightNoStationCmbObj().disabled = true;
        }
        else //KittingTypePAKLabel
        {
            getMaintainLightNoStationCmbObj().selectedIndex=0;
            getMaintainLightNoStationCmbObj().disabled = true;
        }          
         
    }   

    function SetLineState() 
    {
        KittingTypeValue=getMaintainLightNoKittingTypeCmbObj().value.trim();
        if(KittingTypeValue==KittingTypeFAKitting && document.getElementById("<%=dOldIsLine.ClientID %>").value!="Yes" && document.getElementById("<%=dOldCode.ClientID %>").value!="")
        {
            getMaintainLightNoPdLineCmbObj().disabled = false;
        }
        else
        {
            getMaintainLightNoPdLineCmbObj().selectedIndex=0;
            getMaintainLightNoPdLineCmbObj().disabled = true;     
        }
    
    }

    </script>
</asp:Content>


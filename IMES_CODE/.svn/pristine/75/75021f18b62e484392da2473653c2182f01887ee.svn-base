<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="Line.aspx.cs" Inherits="DataMaintain_Line" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%--
 ITC-1361-0016
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="iMes_div_MainTainEdit" >            
                <tr >
                    <td style="width: 200px;">
                        <asp:Label ID="lblCustomer" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="42%">
                        <iMESMaintain:CmbCustomer runat="server" ID="cmbCustomer" Width="66%" ></iMESMaintain:CmbCustomer>
                    </td>                                    
                    <td style="width: 80px;">
                        <asp:Label ID="lblStage" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="42%">
                        <iMESMaintain:CmbMaintainLineStage runat="server" ID="cmbMaintainStage" Width="36%" ></iMESMaintain:CmbMaintainLineStage>
                    </td>    
           
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="40%"></td> 
                    <td align="right">
                       <input type="button" id="btnDelete" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnDelete_ServerClick"></input>
                    </td>            
                </tr>
             </table>  
        </div>


        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" ChildrenAsTriggers="false">
        <ContentTemplate>
        <div id="div2" style="height:400px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%" RowStyle-Height="20"
                        GvExtWidth="100%" GvExtHeight="390px" Height="382px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel> 
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>
    
                        <td width="10%">
                            <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt" Width ="98%"></asp:Label>
                        </td>
                        <td width="20%">
                            <asp:TextBox ID="dPdLine" runat="server" MaxLength="30" Width="98%"></asp:TextBox>
                        </td>       
                        <td width="10%">
                            <asp:Label ID="lblAvgSpeed" runat="server" CssClass="iMes_label_13pt" Width="98%" Text="AvgSpeed:"></asp:Label>                           
                        </td>                      
                        <td width="20%">
                            <asp:TextBox ID="dAvgSpeed" runat="server" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" Width="98%"></asp:TextBox>
                        </td>
                        <td width="10%">
                            <asp:Label ID="lblOwner" runat="server" CssClass="iMes_label_13pt" Width="98%" Text="Owner:"></asp:Label>     
                        </td>
                        <td width="20%">
                            <asp:TextBox ID="dOwner" runat="server" MaxLength="64" Width="98%"></asp:TextBox>
                        </td>
                        <td width="10%"></td>   
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescription" runat="server" CssClass="iMes_label_13pt" Width ="98%"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="dDescription" runat="server" MaxLength="30" Width="98%" SkinId="textBoxSkin" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblAvgManPower" runat="server" CssClass="iMes_label_13pt" Width="98%" Text="AvgManPower:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="dAvgManPower" runat="server" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" Width="98%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblIEOwner" runat="server" CssClass="iMes_label_13pt" Width="98%" Text="IEOwner:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="dIEOwner" runat="server" MaxLength="64" Width="98%"></asp:TextBox>
                        </td>
                        <td></td>           
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAliasLine" runat="server" CssClass="iMes_label_13pt" Width ="98%" Text="AliasLine:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="dAliasLine" runat="server" MaxLength="30" Width="98%" SkinId="textBoxSkin" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblAvgStationQty" runat="server" CssClass="iMes_label_13pt" Width ="98%" Text="AvgStationQty:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="dAvgStationQty" runat="server" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" Width="98%" SkinId="textBoxSkin" ></asp:TextBox>
                        </td>
                        <td></td>
                        <td></td>
                        <td  align="right">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></input>
                        </td>
                    </tr>                    
                      
            </table> 
        </div>  
   
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
            <asp:AsyncPostBackTrigger ControlID="btnStageChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnCustomerChange" EventName="ServerClick" />
        </Triggers>                      
        </asp:UpdatePanel>
         
           <input type="hidden" id="HiddenUserName" runat="server" />
           <input type="hidden" id="dTableHeight" runat="server" />   
           <input type="hidden" id="dOldId" runat="server" />   
           <input type="button" id="btnStageChange" runat="server" style="display:none" onserverclick ="btnStageChange_ServerClick"> </input>
           <input type="button" id="btnCustomerChange" runat="server" style="display:none" onserverclick ="btnCustomerChange_ServerClick"> </input>
    
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
    
    function clkButton()
    {
       switch(event.srcElement.id)
       {
           case "<%=btnSave.ClientID %>":
                
                if(!clkSave())
                {                
                    return false;
                }
 	            break;
 	            
           case "<%=btnDelete.ClientID %>":
           
                if(!clkDelete())
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
    
    
    function initControls()
    {
        getCustomerCmbObj().onchange=CustomerSelectOnChange;   
        getMaintainLineStageCmbObj().onchange=MaintainLineStageSelectOnChange; 
    }
    
    function MaintainLineStageSelectOnChange()
    {
        document.getElementById("<%=btnStageChange.ClientID%>").click();
        ShowWait();   
    }
    
    function CustomerSelectOnChange()
    {       
        document.getElementById("<%=btnCustomerChange.ClientID%>").click();
        ShowWait();
    }
       
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5  ="<%=pmtMessage5%>";
        msg10 ="<%=pmtMessage10%>";   
            
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        initControls();  
        ShowRowEditInfo(null);
        //设置表格的高度  
        resetTableHeight();

    };

    //设置表格的高度  
    function resetTableHeight() {
        //动态调整表格的高度
        var adjustValue = 55;
        var marginValue = 12;
        var tableHeigth = 300;
        //var tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
        try {
            tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
        }
        catch (e) {

            //ignore
        }
        //为了使表格下面有写空隙
        var extDivHeight = tableHeigth + marginValue;
        div2.style.height = extDivHeight + "px";
        document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
        document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
    }
    
    function clkDelete()
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>")
        var curIndex = gdObj.index;
        var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;            
        if(curIndex>=recordCount)
        {
            alert(msg1);
            return false;
        }          
        
        var ret = confirm(msg2);
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
       return check();
        
   }
   
   function check()
   {
       var LineValue=document.getElementById("<%=dPdLine.ClientID %>").value.trim();
        
       if(getCustomerCmbObj().value.trim()=="") {
            alert(msg3);
            return false;    
       } 
       
       if(getMaintainLineStageCmbObj().value.trim()=="") {
            alert(msg4);
            return false;    
       }
        
       if(LineValue=="" ) {
           alert(msg5);      
           return false;
       }  
       
       var DescrValue=document.getElementById("<%=dDescription.ClientID %>").value.trim();
       if(DescrValue=="") {
           alert(msg10);      
           return false;
       }

       var AliasLineValue = document.getElementById("<%=dAliasLine.ClientID %>").value.trim();
       if (AliasLineValue == "") {
           alert("需要輸入[AliasLine]");
           return false;
       }
       
       return true;

   }
   
   
     function clickTable(con)
    {
         setGdHighLight(con);         
         ShowRowEditInfo(con);
       
    }
    
    function setNewItemValue()
    {
               
        document.getElementById("<%=dPdLine.ClientID %>").value = ""
        document.getElementById("<%=dDescription.ClientID %>").value ="";  
        document.getElementById("<%=dOldId.ClientID %>").value="";
                   
//        document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
        document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
    }
    //SELECT [Line]
        //      ,[Descr]
        //      ,[Editor]
        //      ,[Cdt]
        //      ,[Udt]
        //      ,[CustomerID]
        //      ,[Stage]
    function ShowRowEditInfo(con)
    {
         if(con==null)
         {
            setNewItemValue();
            return;    
         }
         
         document.getElementById("<%=dPdLine.ClientID %>").value=con.cells[0].innerText.trim();  
         document.getElementById("<%=dDescription.ClientID %>").value=con.cells[3].innerText.trim();
         document.getElementById("<%=dAliasLine.ClientID %>").value = con.cells[4].innerText.trim();
         document.getElementById("<%=dAvgManPower.ClientID %>").value = con.cells[5].innerText.trim();
         document.getElementById("<%=dAvgSpeed.ClientID %>").value = con.cells[6].innerText.trim();
         document.getElementById("<%=dAvgStationQty.ClientID %>").value = con.cells[7].innerText.trim();
         document.getElementById("<%=dIEOwner.ClientID %>").value = con.cells[8].innerText.trim();
         document.getElementById("<%=dOwner.ClientID %>").value = con.cells[9].innerText.trim();       
         var currentId=con.cells[0].innerText.trim(); 
         document.getElementById("<%=dOldId.ClientID %>").value = currentId;
  
         if(currentId=="")
         {
            setNewItemValue();
         }
         else
         {
//            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
         }
         //trySetFocus();
    }
   
    function trySetFocus()
    {
         var itemObj=document.getElementById("<%=dPdLine.ClientID %>");//getMaintainFamilyCmbObj();
         
         if(itemObj!=null && itemObj!=undefined && itemObj.disabled!=true)
         {
            itemObj.focus();
         }
    }
   
    function AddUpdateComplete(id)
    {
  
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for (var i = 1; i < gdObj.rows.length; i++) {
            if (gdObj.rows[i].cells[0].innerText == id) {
                selectedRowIndex = i;
                break;
            }
        }

        if (selectedRowIndex < 0) {
            ShowRowEditInfo(null);
            return;
        }
        else {
            var con = gdObj.rows[selectedRowIndex];
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);

        }  
        
    }
        
    function DealHideWait()
    {
        HideWait();   
        getCustomerCmbObj().disabled = false; 
        getMaintainLineStageCmbObj().disabled = false; 
        

    }

    </script>
</asp:Content>


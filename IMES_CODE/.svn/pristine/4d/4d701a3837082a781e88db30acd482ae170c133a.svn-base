<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for ITCND Check QC Hold Setting Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/5/10 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/5/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-10   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/
--%>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="LotSetting.aspx.cs" Inherits="DataMaintain_LotSetting" ValidateRequest="false" %>
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
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td> 
                    <%--2012-5-10，去掉列表查询输入框
                    <td width="65%">  
                        <asp:TextBox ID="dSearch" runat="server"   MaxLength="50"  Width="50%" onkeypress='OnKeyPress(this)' SkinId="textBoxSkin"></asp:TextBox>                      
                    </td>
                    --%>
                    <td width="20%" align="right">
                       <input type="button" id="btnDelete" runat="server" class="iMes_button" onclick="if(clkButton())" onserverclick="btnDelete_ServerClick"></input>
                    </td>           
                </tr>
            </table>  
        </div>
        

        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
            <ContentTemplate>
                <div id="div2" style="height:366px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%" RowStyle-Height="20"
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel> 
        
        
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                <tr>    
                    <td style="width: 80px;" align="right">
                        <asp:Label ID="dCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td  width="120px">
                        <iMESMaintain:CmbPdLineForMaintain ID="selLine" runat="server" Width="99%"></iMESMaintain:CmbPdLineForMaintain>                        
                    </td>
                    <td style="width: 80px;" align="right">
                        <asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 80px;" >
                        <asp:DropDownList runat="server" ID="cmbType" Width="80px">
                          <asp:Listitem value="PC">PC</asp:Listitem>    
                          <asp:Listitem value="RCTO">RCTO</asp:Listitem>   
                          <asp:Listitem value="FRU">FRU</asp:Listitem>                 
                        </asp:DropDownList>
                    </td>
                    <td style="width: 80px;" align="right">
                        <asp:Label ID="lblPassQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 80px;">
                    <asp:TextBox ID="textPassQty" runat="server"   MaxLength="200"   Width="99%" SkinId="textBoxSkin"  ></asp:TextBox>
                    </td>
                    <td style="width: 80px;" align="right">
                        <asp:Label ID="lblFailQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    
                    <td style="width: 80px;">
                      <asp:TextBox ID="textFailQty" runat="server"   MaxLength="200"   Width="99%" SkinId="textBoxSkin"  ></asp:TextBox>                      
                    </td>                         

                    <td style="width: 100px;" align="right">
                        <asp:Label ID="lblCheckQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    
                    <td style="width: 80px;">
                      <asp:TextBox ID="textCheckQty" runat="server"   MaxLength="200"   Width="99%" SkinId="textBoxSkin"  ></asp:TextBox>                      
                    </td>     
                    <td style="width: 120px;"> </td>
                    <td align="right">
                        <input type="button" id="btnAdd" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAdd_ServerClick"></input>
                    </td>    
                         <td align="right">
                        <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></input>
                    </td>               
                </tr>
                
                                                                      
            </table> 
        </div>  
   
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
            </Triggers>                      
        </asp:UpdatePanel>    
             
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="dOldCode" runat="server" />   
        <input type="hidden" id="dOldType" runat="server" />   
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="dIsHoldValue" runat="server" /> 
        <%-- 2012-5-17 --%>  
        <input type="hidden" id="dListNull" runat="server" /> 
    </div>
    
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    
<script language="javascript" type="text/javascript">
    
    var msgDelete = "";
    var msgSelectIsHold = "";
    var msgNeedCode = "";
    var msgCodeOverLength = "";
    var msgDescrOverLength = "";

    

    function clkButton()
    {
       switch(event.srcElement.id)
       {
           case "<%=btnSave.ClientID %>":

               if (clkSave() == false)
                {                
                    return false;
                }
 	            break;
 	            
           case "<%=btnDelete.ClientID %>":
           
                if(clkDelete() == false)
                {                
                    return false;
                }          
 	            break;
           case "<%=btnAdd.ClientID %>": 	  
                if(clkAdd() == false)
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
        if((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex,false, "<%=gd.ClientID %>");
        }        
        
        setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
        iSelectedRowIndex = parseInt(con.index, 10);    
    }
    


    window.onload = function() {
        msgDelete = "<%=pmtMessageDelete%>";
        msgSelectIsHold = "<%=pmtMessageSelectIsHold%>";
        msgNeedCode = "<%=pmtMessageNeedCode%>";
        msgPassQty = "<%=pmtMessagePassQty%>";
        msgFailQty = "<%=pmtMessageFailQty%>";
        msgCheckQty = "<%=pmtMessageCheckQty%>";
        msgDataBoth = "<%=pmtMessageDataBoth%>";
        document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";
        
        ShowRowEditInfo(null);

        resetTableHeight();
        

        document.getElementById("<%=dIsHoldValue.ClientID %>").value = "";
    };

    //设置表格的高度  
    function resetTableHeight()
    {    
        //动态调整表格的高度
        var adjustValue = 55;     
        var marginValue = 12; 
        var tableHeigth = 300;
        
        try{
            tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
        }
        catch(e){
        
            //ignore
        }                
        
        //为了使表格下面有些空隙
        var extDivHeight = tableHeigth+marginValue;
        div2.style.height = extDivHeight + "px";
        document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
        document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
    }
    
    function clkDelete()
    {   
         var ret = confirm(msgDelete);
         if (!ret) 
         {
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
       return check("save");
   }
   
   function check(method)
   {
       //var codeValue = document.getElementById("<%=dIsHoldValue.ClientID %>").value.trim();
       var line = getPdLineCmbObj().value;
       var Type = document.getElementById("<%=cmbType.ClientID %>").value;
       var PassQty = parseInt(document.getElementById("<%=textPassQty.ClientID %>").value.trim());
       var FailQty = parseInt(document.getElementById("<%=textFailQty.ClientID %>").value.trim());
       var CheckQty = parseInt(document.getElementById("<%=textCheckQty.ClientID %>").value.trim());   
  
        //值域检查
        if (PassQty <= 0 || isNaN(PassQty)) 
        {
            alert(msgPassQty);
            document.getElementById("<%=textPassQty.ClientID %>").select();
            return false;
        }

        if (FailQty < 0 || isNaN(FailQty)) {
            alert(msgFailQty);
            document.getElementById("<%=textFailQty.ClientID %>").select();
            return false;
        }

        if (CheckQty <= 0 || isNaN(CheckQty) || CheckQty > PassQty) {
            alert(msgCheckQty);
            document.getElementById("<%=textCheckQty.ClientID %>").select();
            return false;
        }
        /*
        if (method == "add") {
            var flag = CheckInTable(line, PassQty, FailQty);
            if (flag == true) {
                alert(msgDataBoth);
                return false;
            }
        }
        */
       return true;
   }
/*
   function CheckInTable(line, PassQty, FailQty) {
       var tbl = "<%=gd.ClientID %>";
       var table = document.getElementById(tbl);
       var subFindFlag = false;
       for (var i = 1; i < table.rows.length; i++) {
           //alert(table.rows[i].cells[0].innerText.trim());
           if ((table.rows[i].cells[0].innerText.trim() == line) ) {
               subFindFlag = true;
               break;
           }
       }
       return subFindFlag;
   }
   */
   function checkAdaptorInfo() {
       ShowInfo("");

       return true;
   } 
   
   function clkAdd()
   {
        //ShowInfo("");
        return check("add");
   }
   
     function clickTable(con)
    {
         setGdHighLight(con);         
         ShowRowEditInfo(con);
       
    }
    
    function setNewItemValue()
    {
        //getKittingCodeTypeCmbObj().selectedIndex = 0;
        getPdLineCmbObj().selectedIndex = 0;
        document.getElementById("<%=cmbType.ClientID %>").value = "PC";
        document.getElementById("<%=textPassQty.ClientID %>").value = "";
        document.getElementById("<%=textFailQty.ClientID %>").value = "";
        document.getElementById("<%=textCheckQty.ClientID %>").value = "";
        //document.getElementById("<%=dCode.ClientID %>").value = ""
 
        //if (document.getElementById("<%=dListNull.ClientID %>").value == "true") {
        //    document.getElementById("<%=btnSave.ClientID %>").disabled = true;
         //   document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        //}
       // else {
        document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        //}
    }
    
    function ShowRowEditInfo(con)
    {
         if(con == null)
         {
            setNewItemValue();
            return;    
         }
         //C.NewFamily,A.[QCRatio],A.[EOQCRatio],A.[PAQCRatio],A.Editor,C.Model,A.Cdt,A.Udt, C.FamilyKey ,C.Flag

         getPdLineCmbObj().value = con.cells[0].innerText.trim();
         document.getElementById("<%=cmbType.ClientID %>").value = con.cells[1].innerText.trim(); ;
         document.getElementById("<%=textPassQty.ClientID %>").value = con.cells[2].innerText.trim(); //1
         document.getElementById("<%=textFailQty.ClientID %>").value = con.cells[3].innerText.trim(); //2 
         document.getElementById("<%=textCheckQty.ClientID %>").value = con.cells[4].innerText.trim();//3

         
         var currentId = con.cells[0].innerText.trim();
         document.getElementById("<%=dOldCode.ClientID %>").value = currentId;
         var currentType = con.cells[1].innerText.trim();
         document.getElementById("<%=dOldType.ClientID %>").value = currentType;
  
         if(currentId == "")
         {
            //document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            //document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            setNewItemValue();
            return;
         }
         else
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled = false;
            document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
         }        
    }
   

    function AddUpdateComplete(id, strtype)
    {
        var gdObj = document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex = -1;
        for(var i = 1; i < gdObj.rows.length; i++)
        {
           if ((gdObj.rows[i].cells[0].innerText == id) && (gdObj.rows[i].cells[1].innerText ==strtype))
           {
               selectedRowIndex = i; 
               break; 
           }        
        }
        
        if(selectedRowIndex < 0)
        {
            ShowRowEditInfo(null);    
            return;
        }
        else
        {            
            var con = gdObj.rows[selectedRowIndex];
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
        }        
    }

     
    function DealHideWait()
    {
        HideWait();
        //getKittingCodeTypeCmbObj().disabled = false; 
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


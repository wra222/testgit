<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="LabelKittingCode.aspx.cs" Inherits="DataMaintain_KittingCode" ValidateRequest="false" %>
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
                    <td style="width: 200px;">
                        <asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="85%" align ="left" >
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>                    
                        <iMESMaintain:CmbKittingCodeType runat="server" ID="cmbKittingCodeType" Width="30%" ></iMESMaintain:CmbKittingCodeType>
        </ContentTemplate>
        </asp:UpdatePanel>                            
                    </td>    
           
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="65%">  
                        <asp:TextBox ID="dSearch" runat="server"   MaxLength="50"  Width="50%" onkeypress='OnKeyPress(this)' SkinId="textBoxSkin"></asp:TextBox>                      
                    </td>    
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
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="125%" RowStyle-Height="20"
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel> 
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>    
                        <td style="width: 80px;">
                            <asp:Label ID="lblCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td  width="35%">
                            <asp:TextBox ID="dCode" runat="server" MaxLength="50" Width="98%" SkinId="textBoxSkin"></asp:TextBox>
                        </td> 
                        <td width="2%">
                        </td>
                        <td style="width: 80px;">
                            <asp:Label ID="lblDescr" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td  width="35%">
                            <asp:TextBox ID="dDescr" runat="server"   MaxLength="50"  Width="98%"  SkinId="textBoxSkin" style='ime-mode:disabled;'></asp:TextBox>
                        </td>                         
                                               
                        <td align="right">
                             <input type="button" id="btnAdd" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAdd_ServerClick"></input>
                        </td>           
                    </tr>
                    <tr>

                        <td style="width: 80px;">
                            <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="dRemark" runat="server"   MaxLength="255"   Width="99%" SkinId="textBoxSkin"  ></asp:TextBox>
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
            <asp:AsyncPostBackTrigger ControlID="btnKittingCodeTypeChange" EventName="ServerClick" />
        </Triggers>                      
        </asp:UpdatePanel>         
           <input type="hidden" id="HiddenUserName" runat="server" />
           <input type="hidden" id="dOldCode" runat="server" />   
           <input type="hidden" id="dTableHeight" runat="server" />   
            <button id="btnKittingCodeTypeChange" runat="server" type="button" style="display:none" onserverclick ="btnKittingCodeTypeChange_ServerClick"> </button>           
    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    <script language="javascript" type="text/javascript">
    //ITC-1361-0017 ITC210012 2012-03-06
    var msg1="";
    var msg2="";
    var msg3="";
    var msg4="";
    var msg5="";
    var msg6="";

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
           if(event.srcElement.id=="<%=dSearch.ClientID %>")
            {
                var value=document.getElementById("<%=dSearch.ClientID %>").value.trim().toUpperCase();
                if(value!="")
                {
                    findKittingCode(value, true);
                }
            }
        }       

    }

    function findKittingCode(searchValue, isNeedPromptAlert)
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
//                alert("找不到列表中与KittingCode栏位匹配的项");     
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

    function NoMatchKittingCode()
    {
         alert(msg2);
//         alert("不存在查询的[KittingCode]");     
         return;   
    }
    
    
    function initControls()
    {
        getKittingCodeTypeCmbObj().onchange = KittingCodeTypeSelectOnChange;  
    }

    function KittingCodeTypeSelectOnChange()
    {
        document.getElementById("<%=btnKittingCodeTypeChange.ClientID%>").click();
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
        document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
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
       var codeValue = document.getElementById("<%=dCode.ClientID %>").value.trim();

       if (getKittingCodeTypeCmbObj().value.trim() == "")
       {
            alert(msg5);
//            alert("需要选择[Type]");
            return false;    
       }

       if (codeValue == "")
       {
            alert(msg6);
//            alert("需要输入[Kitting Code]");
            return false;
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
        //getKittingCodeTypeCmbObj().selectedIndex = 0;
        
        document.getElementById("<%=dCode.ClientID %>").value = ""
        document.getElementById("<%=dDescr.ClientID %>").value = "";  
        document.getElementById("<%=dRemark.ClientID %>").value = ""           
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

         document.getElementById("<%=dCode.ClientID %>").value = con.cells[0].innerText.trim();
         document.getElementById("<%=dDescr.ClientID %>").value = con.cells[1].innerText.trim();
         document.getElementById("<%=dRemark.ClientID %>").value = con.cells[2].innerText.trim();

         //getKittingCodeTypeCmbObj().value = con.cells[6].innerText.trim(); //Type
         var currentId=con.cells[0].innerText.trim();
         document.getElementById("<%=dOldCode.ClientID %>").value = currentId;
         
         
  
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
      
    }
   

    function AddUpdateComplete(id)
    {
  
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(gdObj.rows[i].cells[0].innerText==id)
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
        getKittingCodeTypeCmbObj().disabled = false; 
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


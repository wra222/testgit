<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="VendorCode.aspx.cs" Inherits="DataMaintain_VendorCode"
    ValidateRequest="false" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" border="0">
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="40%">
                        <asp:TextBox ID="dSearch" runat="server" MaxLength="20" Width="50%" CssClass="iMes_textbox_input_Yellow"
                            onkeypress='OnKeyPress(this)'></asp:TextBox>
                    </td>
                    <td width="40%" align="right">
                        <input type="button" id="btnDelete" runat="server" class="iMes_button" onclick="if(clkButton())"
                            onserverclick="btnDelete_ServerClick"></input>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline"
            Visible="true">
            <ContentTemplate>
                <div id="div2" style="height: 366px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="125%"
                        RowStyle-Height="20" GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue="true"
                        HighLightRowPosition="3" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                        OnRowDataBound="gd_RowDataBound" EnableViewState="false">
                    </iMES:GridViewExt>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width: 80px;">
                        <asp:Label ID="lblVendor" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="20%">
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                            Visible="true">
                            <ContentTemplate>
                                <iMESMaintain:CmbMaintainVendor runat="server" ID="cmbMaintainVendor" Width="80%">
                                </iMESMaintain:CmbMaintainVendor >
                                <button id="btnReset" runat="server" onserverclick="btnReset_Click" style="display: none" ></button>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 80px;">
                        <asp:Label ID="lblPartNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="dPartNo" runat="server" MaxLength="20" Width="80%" Style='ime-mode: disabled;'></asp:TextBox>
                    </td>
                    <td style="width: 80px;">
                        <asp:Label ID="lblIdex" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="dIdex" runat="server" MaxLength="2" Width="30%" onkeypress="IdexInputCheck(this)"
                            SkinID="textBoxSkin" Style='ime-mode: disabled;'></asp:TextBox>
                    </td>
                    <td style="width: 150px;">
                        <asp:Label ID="lblVendorCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="dVendorCode" runat="server" MaxLength="20" Width="80%"></asp:TextBox>
                    </td>
                    <td align="right">
                        <input type="button" id="btnAdd" runat="server" onclick="if(clkButton())" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"
                            onserverclick="btnAdd_ServerClick"></input>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="dOldId" runat="server" />
        <input type="hidden" id="dVendor" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
    </div>
    <div id="divWait" oselectid="" align="center" style="cursor: wait; filter: Chroma(Color=skyblue);
        background-color: skyblue; display: none; top: 0; width: 100%; height: 100%;
        z-index: 10000; position: absolute">
        <table style="cursor: wait; background-color: #FFFFFF; border: 1px solid #0054B9;
            font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr>
                <td align="center">
                    <img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif" />
                </td>
                <td align="center" style="color: #0054B9; font-size: 10pt; font-weight: bold;">
                    Please wait.....
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
    
    var msg1="";
    var msg2="";
    var msg3="";
    var msg4="";
    var msg5="";
    var msg6 = "";
    var msg7 = "";
    var msg8 = "";
    var msg9 = "";
    var msg10 = "";

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
                var value=document.getElementById("<%=dSearch.ClientID %>").value.trim().toUpperCase();
                if(value!="")
                {
                    findVendorCode(value, true);
                }
            }
        }       

    }

    function findVendorCode(searchValue, isNeedPromptAlert)
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
//                alert("找不到列表中与Vendor Code栏位匹配的项");     
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
    
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5  ="<%=pmtMessage5%>";
        msg6 = "<%=pmtMessage6%>";
        msg7 = "<%=pmtMessage7%>";
        msg8 = "<%=pmtMessage8%>";
        msg9 = "<%=pmtMessage9%>";
        msg10 = "<%=pmtMessage10%>";

        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
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

   function check()
   {
       var codeValue = document.getElementById("<%=dVendorCode.ClientID %>").value.trim();
       var idexValue = document.getElementById("<%=dIdex.ClientID %>").value.trim();

       if (getVendorCmbObj().value.trim() == "")
       {
            alert(msg5);
//            alert("需要选择[Vendor]");
            return false;    
       }

       if (getVendorCmbObj().value.trim() == "AST") {

           var vendorstr = document.getElementById("<%=dPartNo.ClientID %>").value.trim().toUpperCase();
           if (vendorstr == "") {
               alert(msg9);
               //alert("需要输入[PartNo]");
               return false;
           }
           if (vendorstr.length < 3 || vendorstr.substring(0, 3) != "2TG") {
               alert(msg10);
               //alert("非 AST PN");
               return false;
           }
        }
       
       if (codeValue == "")
       {
            alert(msg6);
//            alert("需要输入[Vendor Code]");
            return false;
        }
        if (idexValue == "") {
            alert(msg7);
            //            alert("需要输入[priority]");
            return false;
        }

        if (idexValue == "0" || idexValue == "00") {
            alert(msg8);
            //            alert("需要输入[priority] 1~99");
            return false;
        }      
       return true;
   }
   
   //只能输入1~99数字
   function IdexInputCheck() {
       //AV框中只能输入数字和下划线
       var key = event.keyCode;
       var idex = document.getElementById("<%=dIdex.ClientID %>").value.trim()
       if (idex == '' && key == 48) {
           event.keyCode = 0;
       }
       if (!(key >= 48 && key <= 57)) {
           event.keyCode = 0;
       }
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
        getVendorCmbObj().selectedIndex = 0;
        
        document.getElementById("<%=dVendorCode.ClientID %>").value = ""
        document.getElementById("<%=dIdex.ClientID %>").value = "";
        document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        ChangeVendor();
    }
    
    function ShowRowEditInfo(con)
    {
         if(con==null)
         {
             setNewItemValue();
             document.getElementById("<%=btnReset.ClientID%>").click();
             return;    
         }
         //C.NewFamily,A.[QCRatio],A.[EOQCRatio],A.[PAQCRatio],A.Editor,C.Model,A.Cdt,A.Udt, C.FamilyKey ,C.Flag

         document.getElementById("<%=dVendorCode.ClientID %>").value = con.cells[2].innerText.trim();
         document.getElementById("<%=dIdex.ClientID %>").value = con.cells[1].innerText.trim();
         document.getElementById("<%=dVendor.ClientID %>").value = con.cells[0].innerText.trim();     
         
         var vendorstr = con.cells[0].innerText.trim(); 
         if (vendorstr.length > 3 &&vendorstr.substring(0, 3)=="2TG")
         {
            vendorstr = "AST";
            document.getElementById("<%=dPartNo.ClientID %>").value = con.cells[0].innerText.trim(); 
         }
         getVendorCmbObj().value = vendorstr;
         if (getVendorCmbObj().selectedIndex == -1) {
             getVendorCmbObj().selectedIndex = 0;
         }
         
         var currentId=con.cells[6].innerText.trim();
         document.getElementById("<%=dOldId.ClientID %>").value = currentId;

         //ChangeVendor();
         
         if(currentId=="")
         {
            setNewItemValue();
         }
         else
         {           
            document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
        }

        document.getElementById("<%=btnReset.ClientID%>").click();
      
    }
   

    function AddUpdateComplete(id)
    {
  
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(gdObj.rows[i].cells[2].innerText==id)
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
        getVendorCmbObj().disabled = false;
    }
    
    function ChangeVendor()
    {
        var vendorstr = getVendorCmbObj().value.trim();
               
        if (vendorstr == "AST") {
            
            document.getElementById("<%=dPartNo.ClientID %>").readOnly = false;
            document.getElementById("<%=dPartNo.ClientID %>").value = document.getElementById("<%=dVendor.ClientID %>").value;             
        }
        else {
            document.getElementById("<%=dPartNo.ClientID %>").readOnly = true;
            document.getElementById("<%=dPartNo.ClientID %>").value="";
        }

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

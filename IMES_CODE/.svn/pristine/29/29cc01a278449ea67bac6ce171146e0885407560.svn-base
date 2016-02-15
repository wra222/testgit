<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PartTypeAttribute.aspx.cs" Inherits="DataMaintain_PartTypeAttribute" ValidateRequest="false" %>
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
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lbPartType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                        <ContentTemplate>             
                                 <iMESMaintain:CmbMaintainPartTypeAttribute runat="server" ID="cmbMaintainPartType" MaxLength="50" Width="93%"></iMESMaintain:CmbMaintainPartTypeAttribute>
                        </ContentTemplate>
                        </asp:UpdatePanel>                            
                    </td>  
                    <td></td> 
                </tr>
             </table>   
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lbPartTypeList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="30%">  
                        <asp:TextBox ID="dSearch" runat="server"   MaxLength="50"  Width="100%" CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>                      
                    </td>    
                    <td align="right">
                      <input type="button" id="btnDelete" runat="server" class="iMes_button" onclick="if(clkButton())" onserverclick="btnDelete_ServerClick"/>
                    </td>           
                </tr>
             </table>
        </div>

        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
        <div id="div2" style="height:366px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="140%" 
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel> 
        <div id="div3">
                <table class="iMes_div_MainTainEdit" width="100%">
                       <tr>
                            <td width="7%">
                              <asp:Label ID="lbCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                            </td>
                            <td width="40%">
                                 <asp:TextBox ID="code" runat="server"   MaxLength="6"  Width="90%" CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>
                            </td>
                            <td width="6%" align="right">
                                 <asp:Label ID="lbSite" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                            </td>
                            <td width="13%">
                                 <input id="site" type="checkbox" runat="server" onclick="ClickSiteCheckBox(this)" name="PUB" value="0" />PUB  
                            </td> 
                            <td width="6%" align="right">
                                 <asp:Label ID="lbCust" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                            </td>
                            <td width="13%">
                                 <input id="cust" type="checkbox" runat="server" onclick="ClickCustCheckBox(this)" name="PUB" value="0" />PUB  
                            </td>
                            <td width="9%">
                            </td>
                        </tr>
                        <tr>
                            <td width="7%">
                                 <asp:Label ID="lbIndex" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                            </td>
                            <td width="40%">
                                 <asp:TextBox ID="index" runat="server"   MaxLength="1"  Width="90%" CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>
                            </td>
                            <td width="6%"/>
                            <td width="13%" rowspan="2">
                              <div style="height:70px;width:100%;background-color:#ffffff;OVERFLOW-Y:scroll;">
                                 <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                                     <ContentTemplate>       
                                         <iMESMaintain:CmbMaintainPartTypeSite runat="server" ID="CmbMaintainPartTypeSite" MaxLength="50" Width="85%"></iMESMaintain:CmbMaintainPartTypeSite>
                                     </ContentTemplate>
                                 </asp:UpdatePanel>
                               </div>
                            </td>
                            <td width="6%"/>
                            <td width="13%" rowspan="2">
                               <div style="height:70px;width:100%;background-color:#ffffff;OVERFLOW-Y:scroll;">
                                 <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                                     <ContentTemplate>          
                                         <iMESMaintain:CmbMaintainPartTypeCust runat="server" ID="CmbMaintainPartTypeCust" MaxLength="50" Width="85%"></iMESMaintain:CmbMaintainPartTypeCust>
                                     </ContentTemplate>
                                 </asp:UpdatePanel> 
                               </div>
                            </td>
                            <td align="right"></td>
                            <td align="right"></td> 
                            <td align="right">
                                  <input type="button" id="btnAdd" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAdd_ServerClick"/>
                            </td>
                        </tr>
                        <tr>
                           <td width="7%">
                                <asp:Label ID="lbDescr" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                           </td>
                           <td width="40%">
                                <asp:TextBox ID="descr" runat="server"  MaxLength="50"  Width="90%" CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>
                           </td>
                           <td width="6%"/>
                           <td width="13%"/>
                           <td width="6%"/>
                           <td width="13%"/>
                           <td align="right" width="9%">
                                <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"/>
                           </td>
                       </tr>
               </table>
        </div>         
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
            <asp:AsyncPostBackTrigger ControlID="btnPartTypeChange" EventName="ServerClick" />
        </Triggers>                      
        </asp:UpdatePanel>       
           <input type="hidden" id="HiddenUserName" runat="server" />
           <input type="hidden" id="itemId" runat="server" />    
           <input type="hidden" id="dTableHeight"  runat="server" />
           <button id="btnPartTypeChange" runat="server" type="button" style="display:none" onserverclick ="btnPartTypeChange_ServerClick"> </button>  
    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    <script language="javascript" type="text/javascript">
    var pub = "<Pub>";
    var siteFlag = "Site";
    var custFlag = "Cust";
    var msg1="";
    var msg2="";
    var msg3="";
    var msg4="";
    var msg5 = "";
    
//按键的前台检查
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
  
  //行设置高亮
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
    
    //按键按下事件]
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
                    findFamily(value, true);
                }
            }
        }       

    }
    
    //根据family查找list数据
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
    
    //family的onchange事件
    function initControls() 
    {
        getPartTypeAttributeListCmbObj().onchange = PartTypeSelectOnChange;   
    }

    //
    function PartTypeSelectOnChange() 
    {
        document.getElementById("<%=btnPartTypeChange.ClientID%>").click();
        ShowWait();

    }

    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5  ="<%=pmtMessage5%>";
      
            
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
        var adjustValue=67;     
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
        div2.style.height=extDivHeight+"px";
        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
    }
    
    //CheckBox的单击事件
    function ClickSiteCheckBox() {
        var siteCheck = document.getElementById("<% =site.ClientID%>").checked;
        if (siteCheck == true) {
            getSiteListCmbObj().disabled = true;
        } else {
            getSiteListCmbObj().disabled = false;
        }
    }
    
    //CheckBox的单击事件
    function ClickCustCheckBox() {
        var custCheck = document.getElementById("<% =cust.ClientID%>").checked;
        if (custCheck == true) {
            getCustListCmbObj().disabled = true;
        } else {
            getCustListCmbObj().disabled = false;
        }
    }
    
    function clkDelete()
    {
//       var ret = confirm("确定要删除这条记录么？");
         var ret = confirm(msg3);
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
//事前检查
   function check() {
       var code = document.getElementById("<%=code.ClientID%>").value.trim();
       var index = document.getElementById("<%=index.ClientID%>").value.trim();
       var descr = document.getElementById("<%=descr.ClientID%>").value.trim();
       if (code == "")
       {
            alert(msg4);
//          alert("需要选择[code]");
            document.getElementById("<%=code.ClientID%>").focus();
            return false;    
       }
      
       if (document.getElementById("<% =index.ClientID%>").diabled == false) {
           var standar = /^([0-9]|[a-z]|[A-Z])$/g;
           if (!standar.test(index)) {
               alert(msg5);
               //alert("[index]格式错误！");
               document.getElementById("<%=index.ClientID%>").focus();
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
  
        document.getElementById("<%=code.ClientID %>").value = ""
        document.getElementById("<%=index.ClientID %>").value = "";
        document.getElementById("<%=descr.ClientID %>").value = "";
        document.getElementById("<%=site.ClientID %>").checked = false;
        document.getElementById("<%=cust.ClientID %>").checked = false;
        ShowSiteBoxList(null);
        ShowCustBoxList(null);

        ShowCheckBoxListDisabled();
        
        document.getElementById("<%=btnSave.ClientID %>").disabled=true;
        document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        document.getElementById("<%=btnAdd.ClientID %>").disabled = false; 
    }

   //根据PartYpe将控件disabled
    function ShowCheckBoxListDisabled() {
        var partType = getPartTypeAttributeListCmbText();
        if (siteFlag == partType || custFlag == partType) {
            document.getElementById("<%=index.ClientID%>").disabled = true;
            document.getElementById("<%=site.ClientID%>").disabled = true;
            document.getElementById("<%=cust.ClientID%>").disabled = true;
            getSiteListCmbObj().disabled = true;
            getCustListCmbObj().disabled = true;
        } else {
            document.getElementById("<%=index.ClientID%>").disabled = false;
            document.getElementById("<%=site.ClientID%>").disabled = false;
            document.getElementById("<%=cust.ClientID%>").disabled = false;
            getSiteListCmbObj().disabled = false;
            getCustListCmbObj().disabled = false;
        }
    }
    //显示行信息
    function ShowRowEditInfo(con) 
    {
         if(con==null)
         {
            setNewItemValue();
            return;    
         }
         document.getElementById("<%=code.ClientID %>").value = con.cells[0].innerText.trim();
         document.getElementById("<%=index.ClientID %>").value = con.cells[1].innerText.trim();
         document.getElementById("<%=descr.ClientID %>").value = con.cells[2].innerText.trim();

         //显示CheckBoxlist的信息
         var site = con.cells[3].innerText.trim();
         var cust = con.cells[4].innerText.trim();
         var partType = getPartTypeAttributeListCmbText();
         //赋值
         if (pub == site) {
             document.getElementById("<%=site.ClientID %>").checked = true;
             ShowSiteBoxList(null);
         } else {
             document.getElementById("<%=site.ClientID %>").checked = false;
             ShowSiteBoxList(site);
         }
         if (pub == cust) {
             document.getElementById("<%=cust.ClientID %>").checked = true;
             ShowCustBoxList(null);
         } else {
             document.getElementById("<%=cust.ClientID %>").checked = false;
             ShowCustBoxList(cust);
         }
         //控制是否disabled
         if (siteFlag == partType || custFlag == partType) {
             document.getElementById("<%=index.ClientID%>").disabled = true;
             document.getElementById("<%=site.ClientID%>").disabled = true;
             document.getElementById("<%=cust.ClientID%>").disabled = true;
             getSiteListCmbObj().disabled = true;
             getCustListCmbObj().disabled = true;
         } else {
             if (pub == site) {
                 getSiteListCmbObj().disabled = true;
             } else {
                 getSiteListCmbObj().disabled = false;
             }
             if (pub == cust) {
                 getCustListCmbObj().disabled = true;
             } else {
                 getCustListCmbObj().disabled = false;
             }
         }
         var currentId=con.cells[0].innerText.trim();
         //保存记录的ID
         document.getElementById("<%=itemId.ClientID %>").value = con.cells[8].innerText.trim();

         if(currentId=="")
         {
            setNewItemValue();
            return;
         }
         else
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
            document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
         }
     }

     //显示Site下的CheckBoxList的信息
     function ShowSiteBoxList(site) {
         for (var i = 0; i < getSiteListCmbObjLength(); i++) {
             getSiteListCmbObjChecked(i).checked = false;
         }
         if (site == null) 
         {
             return;
         }
        var siteArray = new Array();
        siteArray = site.split(">");
        siteArray[0] = siteArray[0].substring(1);
        for (var i = 1; i < siteArray.length - 1; i++) {
            siteArray[i] = siteArray[i].substring(1);
        }
        var a = getSiteListCmbValue();
        for (var j = 0; j < siteArray.length - 1; j++) {
            for (var i = 0; i < getSiteListCmbObjLength(); i++) {
                var siteValue = getSiteListCmbObj().rows[i].cells[0].childNodes(0).innerText;
                if (siteArray[j] == siteValue) {
                    getSiteListCmbObjChecked(i).checked = true;
                }
            }
        }
    }

    //显示Cust下的CheckBoxList的信息
    function ShowCustBoxList(cust) {
        for (var i = 0; i < getCustListCmbObjLength(); i++) {
            getCustListCmbObjChecked(i).checked = false;
        }
        if (cust == null) 
        {
            return;
        }
        var custString;
        var custArray = new Array();
        custArray = cust.split(">");
        for (var i = 0; i < custArray.length-1; i++) {
            custArray[i] = custArray[i].substring(1);
        }
        for (var j = 0; j < custArray.length - 1; j++) {
            for (var i = 0; i < getCustListCmbObjLength(); i++) {
                var custValue = getCustListCmbObj().rows[i].cells[0].childNodes(0).innerText;
                if (custArray[j] == custValue) {
                    getCustListCmbObjChecked(i).checked = true;
                }
            }
        }
    }
    
   //添加或更新之后的前台显设置
    function AddUpdateComplete(id)
    {
  
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++) {
            if (gdObj.rows[i].cells[8].innerText == id )
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
        getPartTypeAttributeListCmbObj().disabled = false;
        ShowCheckBoxListDisabled();
    }

    </script>
</asp:Content>


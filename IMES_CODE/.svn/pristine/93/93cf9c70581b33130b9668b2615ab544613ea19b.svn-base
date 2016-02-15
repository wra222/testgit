<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="FAFloatLocation.aspx.cs" Inherits="DataMaintain_FAFloatLocation" ValidateRequest="false" %>
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
                        <asp:Label ID="lbFamilyTop" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                        <ContentTemplate>             
                                 <iMESMaintain:CmbMaintainFAFloatLocationFamilyTop runat="server" ID="cmbMaintainFamilyTop" MaxLength="50" Width="93%"></iMESMaintain:CmbMaintainFAFloatLocationFamilyTop>
                        </ContentTemplate>
                        </asp:UpdatePanel>                            
                    </td>  
                    <td></td> 
           
                </tr>
             </table>   
             <table width="100%" border="0" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lbList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
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
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="135%" RowStyle-Height="20"
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel> 
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>
                        <td width="6%">
                            <asp:Label ID="lbFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td  width="33%">
                            <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>                 
                                 <iMESMaintain:CmbMaintainFAFloatLocationFamily runat="server" ID="cmbMaintainFamily" MaxLength="50" Width="93%" ></iMESMaintain:CmbMaintainFAFloatLocationFamily>
                            </ContentTemplate>
                            </asp:UpdatePanel>                    
                        </td>                      
                        <td width="6%">
                            <asp:Label ID="lbPType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="24%">
                            <asp:TextBox ID="pType" runat="server"   MaxLength="20"  Width="90%" CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>
                        </td>
                        <td width="6%">
                            <asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="15%">
                            <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                            <ContentTemplate>                 
                                <iMESMaintain:CmbMaintainFAFloatLocationPdLine runat="server" ID="cmbMaintainPdLine" MaxLength="10" Width="97%" ></iMESMaintain:CmbMaintainFAFloatLocationPdLine>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>              
                        <td width="10%" align="right">
                             <input type="button" id="btnAdd" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAdd_ServerClick"/>
                        </td>  
                                 
                    </tr>
                    <tr>
                        <td width="6%">
                            <asp:Label ID="lbLocation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="84%" colspan="5">                            
                            <asp:TextBox ID="location" SkinId="textBoxSkin" runat="server"   MaxLength="255"   Width="99%" ></asp:TextBox>
                        </td> 
                             
                        <td width="10%" align="right">
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
            <asp:AsyncPostBackTrigger ControlID="btnFamilyTopChange" EventName="ServerClick" />
        </Triggers>                      
        </asp:UpdatePanel>         
           <input type="hidden" id="HiddenUserName" runat="server" />
           <input type="hidden" id="itemId" runat="server" />
           <input type="hidden" id="dTableHeight"  runat="server" />
          <button id="btnFamilyTopChange" runat="server" type="button" style="display:none" onserverclick ="btnFamilyTopChange_ServerClick"> </button>
         
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
        getFamilyTopListCmbObj().onchange = FamilyTopSelectOnChange;   
    }

   //顶部下拉框的onchange事件
    function FamilyTopSelectOnChange()
    {       
        document.getElementById("<%=btnFamilyTopChange.ClientID%>").click();
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
        msg11="<%=pmtMessage11 %>";    
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
        div2.style.height=extDivHeight+"px";
        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
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

//执行完删除动作都得页面设置
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
       var partType = document.getElementById("<%=pType.ClientID%>").value.trim();
       var location = document.getElementById("<%=location.ClientID%>").value.trim();
       if (getFamilyListCmbObj().value.trim() == "")
       {
            alert(msg4);
//          alert("需要选择[family]");
            getFamilyListCmbObj().focus();
            return false;    
       }

       if (partType == "")
       {
            alert(msg5);
            //   alert("需要输入[PType]");
            document.getElementById("<%=pType.ClientID%>").focus();
            return false;
       }  
       
       if(location == "" )
       {
            alert(msg6);
            //            alert("需要输入[location]");
            document.getElementById("<%=location.ClientID%>").focus();
            return false;
       }

       if (getPdLineListCmbObj().value.trim() == "")
       {
            alert(msg7);
//          alert("需要选择[Line]");
            getPdLineListCmbObj().focus();
            return false;
       }

       if (partType.length > 20)
       {
            alert(msg8);
            //          alert("[PType]长度不能大于20");
            document.getElementById("<%=pType.ClientID%>").focus();
            return false;      
       }
       
       if(location.length > 255 )
       {
            alert(msg9);
            //            alert("[Location]长度不能大于255");
            document.getElementById("<%=location.ClientID%>").focus();
            return false;
        }  
       var standar = /^\d+~\d+$|^(\d+~\d+,){1,}\d+~\d+$/;
       if (!standar.test(location))
       {
            alert(msg10);
            //            alert("[Location]格式错误！");
            document.getElementById("<%=location.ClientID%>").focus();
            return false;
       }
       if(standar.test(location))
       {
            var arr=location.split(",");
            for(var elementIndex=0;elementIndex<arr.length;elementIndex++)
            {
                var arrItem=arr[elementIndex];
               
                    var temp=arrItem.split("~");
                    if(parseInt(temp[1])<parseInt(temp[0]))
                    {
                        alert(msg11);
                        document.getElementById("<%=location.ClientID%>").focus();
                        return false;
                    }
                
            }
       }
        
       return true;
   }
   
   function clkAdd()
   {
        //ShowInfo("");
        return check();
   }
   
   //GridView的行单击事件
     function clickTable(con)
    {
         setGdHighLight(con);         
         ShowRowEditInfo(con);
       
    }
    
    //点击空行时的页面设置
    function setNewItemValue()
    {
        getFamilyListCmbObj().selectedIndex = 0;
        getPdLineListCmbObj().selectedIndex = 0;
        
        document.getElementById("<%=pType.ClientID %>").value = ""
        document.getElementById("<%=location.ClientID %>").value ="";       
        document.getElementById("<%=btnSave.ClientID %>").disabled=true;
        document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        document.getElementById("<%=btnAdd.ClientID %>").disabled = false; 
    }
    
    //显示行信息
    function ShowRowEditInfo(con)
    {
         if(con==null)
         {
            setNewItemValue();
            return;    
         }
         getFamilyListCmbObj().value = con.cells[0].innerText.trim();          
         document.getElementById("<%=pType.ClientID %>").value=con.cells[1].innerText.trim();
         getPdLineListCmbObj().value = con.cells[2].innerText.trim();
         document.getElementById("<%=location.ClientID %>").value = con.cells[3].innerText.trim();        
         var currentId=con.cells[0].innerText.trim();
         //保存记录的ID
         document.getElementById("<%=itemId.ClientID %>").value = con.cells[7].innerText.trim();
         
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
   
   //添加或更新之后的前台显设置
    function AddUpdateComplete(family,pType,pdLine)
    {
  
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
            if (gdObj.rows[i].cells[0].innerText == family && gdObj.rows[i].cells[1].innerText == pType && gdObj.rows[i].cells[2].innerText == pdLine)
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
        getFamilyTopListCmbObj().disabled = false;
        getFamilyListCmbObj().disabled = false;
        getPdLineListCmbObj().disabled = false;
    }


    </script>
</asp:Content>


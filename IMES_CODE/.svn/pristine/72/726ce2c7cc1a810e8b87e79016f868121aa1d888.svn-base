<%--
/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:ECR Version Maintain
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 *      
 * issues:
    ITC-1361-0025   itc210012   2012-01-12
    
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="Station.aspx.cs" Inherits="DataMaintain_Station" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
<style type="text/css">

.iMes_div_MainTainEdit
{
    border: thin solid Black; 
    background-color: #99CDFF;
    margin:0 0 20 0;
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
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width=""></asp:Label>
                    </td>
                    <td width="">  
                        <asp:TextBox ID="ttStationList" runat="server"   MaxLength="50"  Width="56%" 
                            SkinId="textBoxSkin" onkeypress='OnKeyPress(this)'></asp:TextBox>
                    </td> 
                    <td width="32%" align="right">
                    <input type="hidden" id="hidStation" runat="server" value=""/>
                    <input type="button" id="btnDelete" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnDelete_ServerClick"/>
                    <input type="button" id="btnInfo" runat="server" onclick="onInfo('');" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled/>
                    </td>           
                </tr>
             </table>  
        </div>


        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" ChildrenAsTriggers="false" >
        <ContentTemplate>
        <div id="div2">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="125%" GvExtWidth="100%"   
                        AutoHighlightScrollByValue ="true" 
                        HighLightRowPosition="3" 
                        RowStyle-Height="20"
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' 
                        onrowdatabound="gd_RowDataBound" 
                        EnableViewState="false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel> 
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>
                        <td style="width: 110px;" >
                            <asp:Label ID="lblStation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="180px;" >
                            <asp:TextBox ID="dStation" runat="server"   MaxLength="10"   Width="95%"   SkinId="textBoxSkin"  ></asp:TextBox>
                        </td>
                        
                       <td style="width: 110px;" >
                            <asp:Label ID="lblName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="180px;" >
                            <asp:TextBox ID="dName" runat="server"   MaxLength="64"   Width="95%"   SkinId="textBoxSkin"  ></asp:TextBox>
                        </td>
                        <td style="width: 110px;" >
                            <asp:Label ID="lblType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="180px;" >  
                            <iMESMaintain:CmbMaintainStationType runat="server" ID="cmbMaintainStationType" Width="98%" ></iMESMaintain:CmbMaintainStationType>                                  
                        </td>  
                        <td align="right" >
                            
                        </td>
                    </tr>
                    <tr>        
                        <td style="width: 110px;" >
                            <asp:Label ID="lblObject" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="180px;" >                       
                            <iMESMaintain:CmbMaintainStationObject runat="server" ID="cmbMaintainStationObject" Width="97%" ></iMESMaintain:CmbMaintainStationObject>                                  
                        </td>                          
                          <td style="width: 110px;" >
                            <asp:Label ID="lblDescription" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="3" >                            
                            <asp:TextBox ID="dDescription" runat="server"   MaxLength="50"   Width="98%"  SkinId="textBoxSkin" ></asp:TextBox>
                        </td>  
                        <td align="right">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"/>
                            
                        </td>                           
                    </tr>   
            </table> 
        </div>  
   
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" /> 
        </Triggers>                      
        </asp:UpdatePanel>
           <input type="hidden" id="HiddenUserName" runat="server" />
           <input type="hidden" id="dOldId" runat="server" />   
            <input type="hidden" id="dTableHeight" runat="server" />
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
    function OnKeyPress(obj)
    {
        var key = event.keyCode;
        
        if (key==13)//enter
        {     
            if(event.srcElement.id=="<%=ttStationList.ClientID %>")
            {
                var value=document.getElementById("<%=ttStationList.ClientID %>").value.trim().toUpperCase();
                if(value!="")
                {
                    findFamily(value, true);
                }
            }
        }
    }
    
    function findFamily(searchValue, isNeedPromptAlert)
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
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
//                alert("Cant find that match assembly.");   //1  
                alert(msg8);     
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
            //去掉标题行
//            selectedRowIndex=selectedRowIndex-1;
//            setRowSelectedByIndex_<%=gd.ClientID%>(selectedRowIndex, false, "<%=gd.ClientID%>");
//            setSrollByIndex(selectedRowIndex, true, "<%=gd.ClientID%>");
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
        }    
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
       
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";
        msg6 ="<%=pmtMessage6%>";
        msg7 ="<%=pmtMessage7%>";
        msg8="<%=pmtMessage8 %>";    
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        ShowRowEditInfo(null);
        resetTableHeight();
    };
     function resetTableHeight()
    {
        //动态调整表格的高度
        var adjustValue=70;     
        var marginValue=10;  
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
        //alert(document.getElementById("div_<%=gd.ClientID %>").style.height)
        
        div2.style.height=extDivHeight+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
    }
    function clkDelete()
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>")
        var curIndex = gdObj.index;
        var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;            
        if(curIndex>=recordCount)
        {
            alert(msg1);
//            alert("需要先选择一条记录");
            return false;
        }          
//        var ret = confirm("确定要删除这条记录么？");
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
       //ShowInfo("");
       return check();
        
   }  
     
    function NameCheck(value)
    {
        var reExp = /^[_,a-z,A-Z]+[\s,_,a-z,A-Z,0-9]*$/;
	    if (reExp.exec(value)){
		    return true;
	    }
        return false;

    }
   
   function check()
   {
       var StationValue=document.getElementById("<%=dStation.ClientID %>").value.trim();
        
       if(StationValue=="" )
       {
            alert(msg3);
//            alert("需要输入[Station]");
            return false;
       }  
       
       var NameValue=document.getElementById("<%=dName.ClientID %>").value.trim();
        
       if(NameValue=="" )
       {
            alert(msg6);
//            alert("需要输入[Station]");
            return false;
       }  
       
       if(NameCheck(NameValue)==false)
       {
             alert(msg7);
//            alert("需要输入[Station]");
            return false;      
       
       }
        
       if(getMaintainStationTypeCmbObj().value.trim()=="")
       {
            alert(msg4);
//            alert("需要选择[Type]");
            return false;    
       } 
        
       if(getMaintainStationObjectCmbObj().value.trim()=="")
       {
            alert(msg5);
//            alert("需要选择[Object]");
            return false;    
       } 
    
       return true;
   }
   
   
    function clickTable(con)
    {
         setGdHighLight(con);
         ShowRowEditInfo(con);
         if (con == null) {
             document.getElementById("<%=hidStation.ClientID%>").value = "";
             document.getElementById("<%=btnInfo.ClientID%>").disabled = true;
             return;
         }
         if (con.cells[0].innerText.trim() == "") {
             document.getElementById("<%=btnInfo.ClientID%>").disabled = true;
             document.getElementById("<%=hidStation.ClientID%>").value = "";
         }
         else {
             document.getElementById("<%=btnInfo.ClientID%>").disabled = false;
             document.getElementById("<%=hidStation.ClientID%>").value = con.cells[0].innerText.trim();
         }
    }
    
    function setNewItemValue()
    {
        getMaintainStationTypeCmbObj().selectedIndex=0;
        getMaintainStationObjectCmbObj().selectedIndex=0;
        document.getElementById("<%=dStation.ClientID %>").value = "";
        document.getElementById("<%=dName.ClientID %>").value = "";
        document.getElementById("<%=dDescription.ClientID %>").value ="";   
        document.getElementById("<%=dOldId.ClientID %>").value = "";          
//        document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
        document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
    }
    
    //SELECT [Station]
        //      ,[StationType]
        //      ,[OperationObject]
        //      ,[Descr]
        //      ,[Editor]
        //      ,[Cdt]
        //      ,[Udt]
    function ShowRowEditInfo(con)
    {
         if(con==null)
         {
            setNewItemValue();
            return;    
         }
         document.getElementById("<%=dStation.ClientID %>").value = con.cells[0].innerText.trim();
         document.getElementById("<%=dName.ClientID %>").value =con.cells[1].innerText.trim();          
         getMaintainStationTypeCmbObj().value=con.cells[2].innerText.trim();   //2
         getMaintainStationObjectCmbObj().value=con.cells[8].innerText.trim(); //8
         document.getElementById("<%=dDescription.ClientID %>").value=con.cells[4].innerText.trim();  //4
         var currentId=con.cells[0].innerText.trim(); 
         document.getElementById("<%=dOldId.ClientID %>").value = currentId;
         if(currentId=="")
         {
            setNewItemValue();
         }
         else
         {
            document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
         }
    }
   
    function trySetFocus()
    {
         var itemObj=document.getElementById("<%=dStation.ClientID %>");//getMaintainFamilyCmbObj();
         
         if(itemObj!=null && itemObj!=undefined && itemObj.disabled!=true)
         {
            itemObj.focus();
         }
    }
   
    function AddUpdateComplete(id)
    {
  
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=0;i<gdObj.rows.length;i++)
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
        getMaintainStationTypeCmbObj().disabled = false; 
        getMaintainStationObjectCmbObj().disabled = false;

    }

    function onInfo(m) {
        if ('' == m) {
            m = document.getElementById("<%=hidStation.ClientID%>").value;
        }
        if ('' == m) {
            return;
        }
        var ret = window.showModalDialog("StationInfoAttribute.aspx?StationName=" + m + "&editor=<%=Master.userInfo.UserId%>", 0, "dialogwidth:1000px; dialogheight:560px; status:no;help:no;");
    }

    </script>
</asp:Content>


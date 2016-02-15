<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="Battery.aspx.cs" Inherits="DataMaintain_Battery" ValidateRequest="false" %>
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
                        <asp:Label ID="lblBatteryVCTop" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="42%">
                        <asp:TextBox ID="dBatteryVCTop" runat="server"   MaxLength="12"  Width="56%" CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>
                    </td>                                    
                    
                    <td width="40%">
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBatteryChange" EventName="ServerClick" />            
        </Triggers>       
        </asp:UpdatePanel>                            
                    </td>    
           
                </tr>
             </table>  
                                                    
             <table width="100%" >
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="42%">  
                        <asp:TextBox ID="dSearch" runat="server"   MaxLength="12"  Width="56%" CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>                      
                    </td>    
                    <td width="40%" align="right">
                        <input type="button" id="btnDelete" runat="server" onclick="if(clkDelete())"  class="iMes_button" onserverclick="btnDelete_ServerClick"></input>
                    </td>           
                </tr>
             </table>  
        </div>

        <asp:UpdatePanel ID="updatePanelAll" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <Triggers>            
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnBatteryChange" EventName="ServerClick" />
        </Triggers>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
        <div id="div2" style="height:366px">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="98%" RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>
                        <td style="width: 90px;">
                            <asp:Label ID="lblBatteryVC" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="20%" align="left">
                            <asp:TextBox ID="dBatteryVC" runat="server"   MaxLength="12"  CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>
                        <td style="width:3%">
                        </td>
                        <td style="width: 90px;"  >
                            <asp:Label ID="lblHssn" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="20%" align="left">                            
                            <asp:TextBox ID="dHssn" runat="server"   MaxLength="20" CssClass="iMes_textbox_input_Yellow" Width="220px"></asp:TextBox>
                        </td> 
                        
                        <td align="right" width="40%">
                            <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAdd_ServerClick"></input>
                        </td> 
                           
                        <td align="right" width="10%">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></input>
                            <input type="hidden" id="dOldBatteryVC" runat="server" />
                        </td>           
                    </tr>                    
                    
                    
            </table> 
        </div>  
     
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />    

        <button id="btnBatteryChange" runat="server" type="button" style="display:none" onserverclick ="btnBatteryChange_ServerClick"> </button>
       
    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    <script language="javascript" type="text/javascript">
     
    function OnKeyPress(obj)
    {
   
        var key = event.keyCode;         
        if (key==13)//enter
        {
            if(event.srcElement.id=="<%=dBatteryVCTop.ClientID %>")
            {
                if(document.getElementById("<%=dBatteryVCTop.ClientID %>").value.trim()!="")
                {
                    document.getElementById("<%=btnBatteryChange.ClientID %>").click();
                    ShowWait();
                }
                
            }
            else if(event.srcElement.id=="<%=dSearch.ClientID %>")
            {
                var value=document.getElementById("<%=dSearch.ClientID %>").value.trim().toUpperCase();
                if(value!="")
                {
                    findBattery(value, true);
                }
            }
        }       

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
    
    function findBattery(searchValue, isNeedPromptAlert)
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>");
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
//                alert("Cant find that match family.");   //1  
                alert(msg1);     
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
   
    //var familyObj;
   // var descriptionObj;
    
    var msg1="";
    var msg2="";
    var msg3="";
    var msg4="";
    var msg5="";


    window.onload = function()
    {
        //customerObj = getCustomerCmbObj();
        //customerObj.onchange = addNew;
        
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";
        msg6="<%=pmtMessage6 %>";

        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        ShowRowEditInfo(null);
                   
        //设置表格的高度  
        resetTableHeight();

     };

    //设置表格的高度  
    function resetTableHeight() {
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
        var extDivHeight = tableHeigth + marginValue;
        div2.style.height = extDivHeight + "px";
        document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
        document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";

    }
        
    function clkDelete()
    {
        ShowInfo("");
        var gdObj=document.getElementById("<%=gd.ClientID %>")
        var curIndex = gdObj.index;
        var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;            
        if(curIndex>=recordCount)
        {
//            alert("Please select one row!");   //2
            alert(msg2);
            return false;
        }
        
//         var ret = confirm("Do you really want to delete this item?");  //3
         var ret = confirm(msg3);  //3
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
       ShowInfo("");
       var batteryVCValue = document.getElementById("<%=dBatteryVC.ClientID %>").value;   
       var dHssnValue = document.getElementById("<%=dHssn.ClientID %>").value;   
       if(batteryVCValue.trim()=="")
       {
//           alert("Please input [Family] first!!");  //4
           alert(msg4);
           return false;
       } 
       else if(dHssnValue.trim()=="")
       {
            alert(msg6);
            return false;
       }  
       ShowWait();
       return true;
        
   }
   
   function clkAdd()
   {
   
       ShowInfo("");
       var batteryVCValue = document.getElementById("<%=dBatteryVC.ClientID %>").value;   
       var dHssnValue = document.getElementById("<%=dHssn.ClientID %>").value;   
       if(batteryVCValue.trim()=="")
       {
//           alert("Please input [Family] first!!");  //4
           alert(msg4);
           return false;
       }
       else if(dHssnValue.trim()=="")
       {
        alert(msg6);
        return false;
       }
       ShowWait();
       return true;
   }
   
    function clickTable(con)
    {
         setGdHighLight(con);         
         ShowRowEditInfo(con);
       
    }
    
    function ShowRowEditInfo(con)
    {
       // customerObj = getCustomerCmbObj();
       // customerObj.onchange = addNew;

         if(con==null)
         {
            document.getElementById("<%=dBatteryVC.ClientID %>").value =""; 
            document.getElementById("<%=dHssn.ClientID %>").value = "";  
            document.getElementById("<%=dOldBatteryVC.ClientID %>").value = "";
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
            return;    
         }
    
         var curBatteryVC= con.cells[0].innerText.trim();
         document.getElementById("<%=dBatteryVC.ClientID %>").value =curBatteryVC 
         document.getElementById("<%=dHssn.ClientID %>").value = con.cells[1].innerText.trim(); 
         
         document.getElementById("<%=dOldBatteryVC.ClientID %>").value = curBatteryVC;    
         if(curBatteryVC=="")
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;
         }
         else
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
         }
//         document.getElementById("<%=dBatteryVC.ClientID %>").focus();
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
    }

    function NoMatchFamily()
    {
//         alert("Cant find that match family.");    //5 
         alert(msg5);     
         return;   
    }
   
    </script>
</asp:Content>


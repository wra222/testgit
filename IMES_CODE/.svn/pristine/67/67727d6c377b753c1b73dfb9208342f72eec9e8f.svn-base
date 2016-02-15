<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="InternalCOA.aspx.cs" Inherits="DataMaintain_InternalCOA" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >       
    </asp:ScriptManager>
    
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" >   
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="50%">  
                        <asp:TextBox ID="dSearch" runat="server"   MaxLength="20"  Width="250px" SkinId="textBoxSkin" onkeypress='OnKeyPress(this)'></asp:TextBox>                      
                    </td>    
                    <td width="32%" align="right">
                      <input type="button" id="btnDelete" runat="server" onclick="if(clkDelete())"  class="iMes_button" onserverclick="btnDelete_ServerClick"></input>
                    </td>           
                </tr>
             </table>  
        </div>

        <asp:UpdatePanel ID="updatePanelAll" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <Triggers>            
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
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
                            <asp:Label ID="lblCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="80%" align="left">
                            <asp:TextBox ID="dCode" runat="server" SkinId="textBoxSkin"  MaxLength="20" Width="250px" CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>
                        <td style="width:3%">
                        </td>                        
                        
                        <td align="right" width="20%">
                            <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAdd_ServerClick"></input>
                            <input type="hidden" id="dOldCode" runat="server" />

                        </td> 
                    </tr>         
            </table> 
        </div>      
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />    

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
            if(event.srcElement.id=="<%=dSearch.ClientID %>")
            {
                var value=document.getElementById("<%=dSearch.ClientID %>").value.trim().toUpperCase();
                if(value!="")
                {
                    findCOA(value, true);
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
    
    function findCOA(searchValue, isNeedPromptAlert)
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
   
   
   function clkAdd()
   {
   
       ShowInfo("");
       var codeValue = document.getElementById("<%=dCode.ClientID %>").value;   
       if(codeValue.trim()=="")
       {
//           alert("Please input [Family] first!!");  //4
           alert(msg4);
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
            document.getElementById("<%=dCode.ClientID %>").value =""; 
            document.getElementById("<%=dOldCode.ClientID %>").value = "";
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
            return;    
         }
    
         var curCode= con.cells[0].innerText.trim();
         var curID= con.cells[5].innerText.trim();
         document.getElementById("<%=dCode.ClientID %>").value =curCode 
         
         document.getElementById("<%=dOldCode.ClientID %>").value = curID;    
         if(curCode=="")
         {
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;
         }
         else
         {
            document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
         }
//         document.getElementById("<%=dCode.ClientID %>").focus();
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
        
        if(selectedRowIndex<0)
        {
            document.getElementById("<%=dCode.ClientID %>").value =""; 
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;          
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
    }
    
    function NoMatchFamily()
    {
//         alert("Cant find that match family.");    //5 
         alert(msg5);     
         return;   
    }
   
    </script>
</asp:Content>


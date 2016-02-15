<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="MACRange.aspx.cs" Inherits="DataMaintain_MACRange" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%-- ITC-1361-0033 --%>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1"  class="iMes_div_MainTainDiv1">
             <table width="100%" border="0" >
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                    </td>
                    <td width="30%">  
                        <asp:TextBox ID="dSearch" runat="server"   MaxLength="10"  Width="100%" CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>                      
                    </td>    
                    <td width="48%" align="right">
                      <input type="button" id="btnDelete" runat="server" class="iMes_button" onclick="if(clkButton())" onserverclick="btnDelete_ServerClick" />
                    </td>           
                </tr>
             </table> 
        </div>

        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
        <div id="div2" style="height :428px">

                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%"  RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="418px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' OnGvExtRowDblClick='if(typeof(dbClickTable)=="function") dbClickTable(this)'
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>   
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>
                        <td style="width:10%; padding-left:2px;">
                            <asp:Label ID="lblCode" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
                        </td>
                        <td width="34%">
                            <asp:TextBox ID="dCode" runat="server"   MaxLength="30"   Width="96%" TabIndex="0"  CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>    
                        <td width="10%">
                            <asp:Label ID="lblStatus" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="35%" >
                            <%--<iMESMaintain:CmbMacRangeStatus runat="server" ID="cmbMacRangeStatus" Width="97%" ></iMESMaintain:CmbMacRangeStatus>--%>
                             <asp:Label ID="dMacRangeStatus" runat="server" CssClass="iMes_label_13pt" Width="96%" ></asp:Label>
                        </td>                           
                        <td align="right" width="11%">
                            <input type="button" id="btnAdd" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAdd_ServerClick" />
                        </td>           
                    </tr>
                    <tr>
                        <td >
                            <asp:Label ID="lblBeginNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td >
                           
                            <asp:TextBox ID="dBeginNo" runat="server"   MaxLength="12"  Width="96%" TabIndex="1"  CssClass="iMes_textbox_input_Yellow" onkeypress='checkKeyPress(this)' ></asp:TextBox>
                        </td>  
                        <td >
                            <asp:Label ID="lblEndNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="dEndNo" runat="server"   MaxLength="12"   Width="96%" TabIndex="0"  CssClass="iMes_textbox_input_Yellow" onkeypress='checkKeyPress(this)' ></asp:TextBox>
                        </td>                            
                        <td align="right">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkButton())" style="background-color :Green " class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick" />
                            <input type="hidden" id="dOldId" runat="server" />
                            <input type="hidden" id="dStatus" runat="server" />                            
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
    var msg9="";
    var msg10="";
    
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";
        msg6 ="<%=pmtMessage6%>";
        msg7 ="<%=pmtMessage7%>";
        msg8 ="<%=pmtMessage8%>";
        msg9 ="<%=pmtMessage9%>";
        msg10 ="<%=pmtMessage10%>";
        
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        ShowRowEditInfo(null);
        //设置表格的高度  
        resetTableHeight();
    };
    
   //设置表格的高度  
    function resetTableHeight()
    {    
        //动态调整表格的高度
        var adjustValue=60;     
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
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
    }
    
    function clkDelete()
    {
//         var ret = confirm("Do you really want to delete this item?");  //3
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

       return check();
        
   }
   
   function check()
   {
       var codeValue = document.getElementById("<%=dCode.ClientID %>").value;   
       var beginNoValue = document.getElementById("<%=dBeginNo.ClientID %>").value.trim();   
       var endNoValue = document.getElementById("<%=dEndNo.ClientID %>").value.trim();
        
//       var statusValue = getMacRangeStatusCmbObj().value; 
              
       if(codeValue.trim()=="")
       {
//           alert("Please input [Code] first!!");  //4
           alert(msg4);
           return false;
       }   
       
//       if(statusValue.trim()=="")
//       {
//           alert("Please input [Status] first!!");
//           return false;
//       } 
       
       if(beginNoValue=="")
       {
//           alert("Please input [BeginNo] first!!");  //5
           alert(msg5);
           return false;
       } 
       if(endNoValue=="")
       {
//           alert("Please input [EndNo] first!!");  //6
           alert(msg6);
           return false;
       } 

       if(beginNoValue.length<12)
       {
//           alert("[Begin No]'s length can't less than 12.")  //7
           alert(msg7);
           return false;
       } 
        
       if(endNoValue.length<12)
       {
//           alert("[End No]'s length can't less than 12.")   //8
           alert (msg8);
           return false;
       } 
            
       var beginNum=parseInt("0X"+beginNoValue);
       var endNum=parseInt("0X"+endNoValue);
       
       if(!(endNum>beginNum))
       {
//           alert("[End No] must large than [Begin No].")  //9
           alert(msg9)           
           return false;
       }
       
       return true;
   }
   
   function clkAdd()
   {
       return check();
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
    
    function clickTable(con)
    {
         setGdHighLight(con);         
         ShowRowEditInfo(con);
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
                    findMACRange(value, true);
                }
            }
        }       

    }
    
    function findMACRange(searchValue, isNeedPromptAlert)
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
//                alert("Cant find that match MACRange.");    //10  
                 alert(msg10);  
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
    
    function dbClickTable(con)
    {
        var mACRangeCode =con.cells[0].innerText.trim(); 
        var dlgFeature = "dialogHeight:315px;dialogWidth:400px;center:yes;status:no;help:no";
        var dlgReturn=window.showModalDialog("MACRangeInfo.aspx?MACRangeCode="+mACRangeCode, window, dlgFeature);
//        if(dlgReturn!=null)
//        {
//        }
    }
    
    function ShowRowEditInfo(con)
    {
         if(con==null)
         {
            document.getElementById("<%=dCode.ClientID %>").value =""; 
//            getMacRangeStatusCmbObj().selectedIndex=0;
            document.getElementById("<%=dMacRangeStatus.ClientID %>").innerText="";
            document.getElementById("<%=dBeginNo.ClientID %>").value = "";
            document.getElementById("<%=dEndNo.ClientID %>").value = "";
            
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
            return;    
         }
         
         document.getElementById("<%=dCode.ClientID %>").value =con.cells[0].innerText.trim(); 
         document.getElementById("<%=dBeginNo.ClientID %>").value = con.cells[1].innerText.trim(); 
         document.getElementById("<%=dEndNo.ClientID %>").value = con.cells[2].innerText.trim(); 
         //控件是value匹配
//         getMacRangeStatusCmbObj().value= con.cells[8].innerText.trim(); 
         document.getElementById("<%=dMacRangeStatus.ClientID %>").innerText=con.cells[3].innerText.trim(); 

         var currentId=con.cells[7].innerText.trim(); 
         document.getElementById("<%=dOldId.ClientID %>").value = currentId;
         document.getElementById("<%=dStatus.ClientID %>").value = con.cells[8].innerText.trim();
  
         if(currentId=="")
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;
//            getMacRangeStatusCmbObj().selectedIndex=0;
            document.getElementById("<%=dMacRangeStatus.ClientID %>").innerText="";
            
         }
         else
         {
            if(!(document.getElementById("<%=dStatus.ClientID %>").value=="<%=STATUS_CREATE_VALUE%>" ))
            {
                document.getElementById("<%=btnSave.ClientID %>").disabled=true;
                //spec要求警示用户
                document.getElementById("<%=btnDelete.ClientID %>").disabled=true;    
            }
            else
            {
                document.getElementById("<%=btnSave.ClientID %>").disabled=false;
                document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
            }        

         }
         document.getElementById("<%=dCode.ClientID %>").focus();
    }
   
   
    function AddUpdateComplete(id)
    {
  
        
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(gdObj.rows[i].cells[7].innerText==id)
           {
               selectedRowIndex=i;  
           }        
        }
        
        if(selectedRowIndex<0)
        {
            document.getElementById("<%=dCode.ClientID %>").value =""; 
//            getMacRangeStatusCmbObj().selectedIndex=0;
            document.getElementById("<%=dMacRangeStatus.ClientID %>").innerText="";
            document.getElementById("<%=dBeginNo.ClientID %>").value = "";
            document.getElementById("<%=dEndNo.ClientID %>").value = "";
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;        
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
    
    function checkKeyPress(obj)
    {
       var key = event.keyCode;

       if(!((key >= 48 && key <= 57) || (key >= 65 && key <= 70) || (key >= 97 && key <= 102) ))
       {  
           event.keyCode = 0;
       }    
    }
    
    function DealHideWait()
    {
        HideWait();   
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


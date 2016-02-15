<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="Forwarder.aspx.cs" Inherits="DataMaintain_Forwarder" 

ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" style="margin-top: 5px; margin-bottom:4px; ">
            <table width="100%" border="0">
                <tr>
                    <td style="width: 60%; padding-left: 2px;">
                    </td>
                    <td style="width: 160px; text-align: right;">
                        <asp:Label ID="lblUploadDate" runat="server" CssClass="iMes_label_13pt" style="width:98%;"></asp:Label>
                    </td>
                    <td style="width:170px;">
                        <asp:TextBox ID="dUploadDate" runat="server" Width="87%" SkinId="textBoxSkinDisabled"  ReadOnly ="true"  ></asp:TextBox>                                                 
                    </td>
                    <td width="120px" align="left">
                         <input type="button" id="btnUpload" style="width:90%;" runat="server" onclick="clkButton();" />
                    </td>

                </tr>
            </table>
            <fieldset style="border-color:InactiveCaption; border-width:thin" class="iMes_div_MainTainEdit">
            <legend><asp:Label ID="lblQueryCondition" runat="server" CssClass="iMes_label_13pt"></asp:Label> </legend>

                <table width="100%" border="0" >
                    <tr>
                        <td style="width: 20px; padding-left: 2px;">
                        </td>
                        <%--cal1--%>
                        <td style="width: 110px; text-align: right;">
                            <asp:Label ID="lblStartDate" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                        </td>
                        <td>
                            <input type="text" id="dCalFrom" style="width: 90px;" readonly="readonly" />
                        </td>
                        <td>
                            <input type="button" id="btnCal1" style="width: 23px;" onclick="showCalendar('dCalFrom')" value="..." />
                        </td>
                        <td style="width: 20px;">
                            &nbsp
                        </td>
                        <%--cal2--%>
                        <td style="width: 100px; text-align: right;">
                            <asp:Label ID="lblEndDate" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                        </td>
                        <td>
                            <input type="text" id="dCalTo" style="width: 90px;" readonly="readonly" />
                        </td>
                        <td>
                            <input type="button" id="btnCal2" style="width: 23px;" onclick="showCalendar('dCalTo')" value="..." />
                        </td>
                        <td width="40px">
                        </td>
                        <td width="120px" align="left">
                             <input type="button" id="btnQuery" style="width:90%;" runat="server" onclick="if(clkButton())" onserverclick="btnQuery_ServerClick" />
                        </td>
                        <td style="width: 30%">
                        </td>
                    </tr>
                    <tr style="height:5px">
                    <td></td>
                    </tr>
                </table>
            </fieldset>
             <table width="100%" border="0" >
                <tr>
                    <td style="width: 125px; padding-left: 2px;">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="50%">  
                    </td>    
                </tr>
             </table>  
        </div>        
    
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
        <div id="div2" style="height:322px;">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="130%" 
                        RowStyle-Height="20" GvExtWidth="100%" GvExtHeight="262px" AutoHighlightScrollByValue ="true" 
                        HighLightRowPosition="3" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' 
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel> 
        
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>    
                        <td style="width:8%;padding-left:2px;">
                            <asp:Label ID="lblDate" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td  width="35%">
                            <asp:TextBox ID="dDate" runat="server" Width="96%" SkinId="textBoxSkinDisabled" readonly="true" ></asp:TextBox>                    
                        </td> 
                        <td width="10%">
                            <asp:Label ID="lblForwarder" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td  width="35%">
                            <asp:TextBox ID="dForwarder" runat="server" Width="98%" SkinId="textBoxSkinDisabled" readonly="true" ></asp:TextBox>                                                 
                        </td>                         
                                               
                        <td align="right">
                            <input type="button" id="btnUpdate" runat="server" onclick="if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnUpdate_ServerClick" />
                        </td>           
                    </tr>
                    <tr>

                        <td style="width: 70px;">
                            <asp:Label ID="lblMAWB" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td colspan="3" >                            
                             <asp:TextBox ID="dMAWB" runat="server" Width="99%" SkinId="textBoxSkinDisabled" readonly="true" ></asp:TextBox> 
                        </td>                        
                        
                        <td align="right">
                            <input type="button" id="btnDelete" runat="server" onclick="this.className='iMes_button_onmouseout';if(clkButton())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnDelete_ServerClick" />
                        </td>           
                    </tr>     
                    <tr>

                        <td style="width: 70px;">
                            <asp:Label ID="lblDriver" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="34%">
                            <asp:TextBox ID="dDriver" runat="server" MaxLength="50"  Width="96%" SkinId="textBoxSkin" ></asp:TextBox>
                        </td>
                        <td style="width: 90px;">
                            <asp:Label ID="lblTruckID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="34%">
                            <asp:TextBox ID="dTruckID" runat="server" MaxLength="50"  Width="98%" CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>
                        </td> 
                        <td></td>          
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblContainerID" runat="server" CssClass="iMes_label_13pt">ContainerID:</asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="dContainerID" runat="server" MaxLength="50"  Width="96%" SkinId="textBoxSkin" ></asp:TextBox>
                        </td>
                    </tr>
            </table> 
        </div>  
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <input type="hidden" id="dCalValue1" runat="server" />
        <input type="hidden" id="dCalValue2" runat="server" />
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="dOldId" runat="server" />
        <input type="hidden" id="dDateValue" runat="server" />
        <input type="hidden" id="dForwarderValue" runat="server" />
        <input type="hidden" id="dMAWBValue" runat="server" />
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
    var msg6="";
 
    window.onload = function()
    {
        
        document.getElementById("dCalFrom").value="<%=today%>";
        document.getElementById("dCalTo").value="<%=today%>";
    
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";

        
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        ShowRowEditInfo(null);
        //设置表格的高度  
        resetTableHeight();

    };
    
    //var isSet=false;
   
     //设置表格的高度  
    function resetTableHeight()
    {
        //if(isSet==true)
//        {
//            return;
//        }
            //动态调整表格的高度
        var adjustValue=60;     
        var marginValue=12; 
        var tableHeigth=300;
        try{
            tableHeigth=document.body.parentElement.offsetHeight-document.getElementById("div1").offsetHeight-document.getElementById("div3").offsetHeight-adjustValue;
        }
        catch(e){
        
            //ignore
        }
        //isSet=true;
        //为了使表格下面有写空隙
        var extDivHeight=tableHeigth+marginValue;
        div2.style.height=extDivHeight+"px";
        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";    
    
    }
    
    
    function isRowSelect()
    {
//        if(iSelectedRowIndex==null)
//        {
//            return false;
//        }  
//        var gdObj=document.getElementById("<%=gd.ClientID %>");
//        //因为有头
//        var tmpIndex=iSelectedRowIndex+1;
//        var selectRework=gdObj.rows[tmpIndex].cells[0].innerText.trim();
//        if(selectRework=="")
//        {
//            return false;
//        }
//        return true;
    }
    
    function clkButton()
    {
       //ShowInfo("");
       switch(event.srcElement.id)
       {
           case "<%=btnQuery.ClientID %>":
                document.getElementById("<%=dCalValue1.ClientID %>").value=document.getElementById("dCalFrom").value;
                document.getElementById("<%=dCalValue2.ClientID %>").value=document.getElementById("dCalTo").value;
 	            break; 
           case "<%=btnUpload.ClientID %>":               
 
               var editor=document.getElementById("<%=HiddenUserName.ClientID %>").value;
               var dlgFeature = "dialogHeight:200px;dialogWidth:424px;center:yes;status:no;help:no";       
               
               editor=encodeURI(Encode_URL2(editor));
               var dlgReturn=window.showModalDialog("ForwarderUploadFile.aspx?userName="+editor, window, dlgFeature);
               if(dlgReturn=="OK")
               {
                   document.getElementById("<%=btnQuery.ClientID %>").click();                   
               }
               return; 
 	           break;
 	            
           case "<%=btnDelete.ClientID %>":

                var ret = confirm(msg3); 
//                var ret = confirm("确定要删除这条记录么？");  
                if (!ret) {
                    return false;
                }               
                         
 	            break;
 	            
           case "<%=btnUpdate.ClientID %>":
                if(check()==false)
                {
                    return false;
                }
 	            break;
    	}   
        ShowWait();
        return true;
    }    
    
   function check()
   {
       var DriverValue=document.getElementById("<%=dDriver.ClientID %>").value.trim();
       var TruckIDValue=document.getElementById("<%=dTruckID.ClientID %>").value.trim();    
        
       if(DriverValue.trim()=="")
       {
            alert(msg1);
//            alert("需要输入[Driver]");
            return false;    
       } 
        
       if(TruckIDValue=="" )
       {
            alert(msg2);
//            alert("需要输入[Truck ID]");
            return false;
       }
     
       return true;
   }
    
    var iSelectedRowIndex=null;    
    
   function DeleteComplete()
   {   
       ShowRowEditInfo(null);
   }
    
    function clickTable(con)
    {
         setGdHighLight(con);         
         ShowRowEditInfo(con);
       
    }
    
    function setNewItemValue()
    {
        document.getElementById("<%=dDate.ClientID %>").value = ""
        document.getElementById("<%=dForwarder.ClientID %>").value ="";  
        document.getElementById("<%=dMAWB.ClientID %>").value = ""           
        document.getElementById("<%=dDriver.ClientID %>").value="";
        document.getElementById("<%=dTruckID.ClientID %>").value="";
        
        document.getElementById("<%=dDateValue.ClientID %>").value = ""
        document.getElementById("<%=dForwarderValue.ClientID %>").value ="";
        document.getElementById("<%=dMAWBValue.ClientID %>").value = "";
        document.getElementById("<%=dContainerID.ClientID %>").value = "";
        document.getElementById("<%=btnUpdate.ClientID %>").disabled=true;  
        document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
        
    }
    
     function ShowRowEditInfo(con)
    {
         if(con==null)
         {
            setNewItemValue();
            return;    
         }
         //SELECT [Date], [Forwarder],[MAWB],[Driver],[TruckID],[Editor],CONVERT(varchar, [Cdt], 21) as [UploadDate],
         //CONVERT(char(10), [Udt], 21) as Udt, Id
        
         document.getElementById("<%=dDate.ClientID %>").value=con.cells[0].innerText.trim();  
         document.getElementById("<%=dForwarder.ClientID %>").value=con.cells[1].innerText.trim();         
         document.getElementById("<%=dMAWB.ClientID %>").value=con.cells[2].innerText.trim(); 
         document.getElementById("<%=dDriver.ClientID %>").value=con.cells[3].innerText.trim(); 
         document.getElementById("<%=dTruckID.ClientID %>").value=con.cells[4].innerText.trim();
         document.getElementById("<%=dContainerID.ClientID %>").value = con.cells[9].innerText.trim();
         
         document.getElementById("<%=dDateValue.ClientID %>").value=con.cells[0].innerText.trim();  
         document.getElementById("<%=dForwarderValue.ClientID %>").value=con.cells[1].innerText.trim();         
         document.getElementById("<%=dMAWBValue.ClientID %>").value=con.cells[2].innerText.trim();
         var currentId=con.cells[8].innerText.trim(); 
         document.getElementById("<%=dOldId.ClientID %>").value = currentId;
  
         if(currentId=="")
         {
            setNewItemValue();
         }
         else
         {
            document.getElementById("<%=btnUpdate.ClientID %>").disabled=false;
            document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
            //trySetFocus();
         }
         
    }
   
    function trySetFocus()
    {
         var itemObj=document.getElementById("<%=dDriver.ClientID %>");//getMaintainFamilyCmbObj();
         
         if(itemObj!=null && itemObj!=undefined && itemObj.disabled!=true)
         {
            itemObj.focus();
         }
    }
   
    function AddUpdateComplete(id)
    {
  
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(gdObj.rows[i].cells[8].innerText==id)
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
 
    function setGdHighLight(con)
    {
        if((iSelectedRowIndex!=null) && (iSelectedRowIndex!=parseInt(con.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex,false, "<%=gd.ClientID %>");                
        }        
        setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
        iSelectedRowIndex=parseInt(con.index, 10);    
    }
    
    function onDateKeyDown() {

        if (event.keyCode == 9) {

        } else if (event.keyCode == 8 || event.keyCode == 32 || event.keyCode == 46) {
            event.returnValue = false;
            event.srcElement.value = "";
        } else {
            event.returnValue = false;
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
                            for (var k1 = behaviors.length - 1; k1 >= 0; k1--) {
                                behaviors[k1].dispose();
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

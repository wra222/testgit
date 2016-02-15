<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="FAKittingUpload.aspx.cs" Inherits="DataMaintain_FAKittingUpload" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">

             <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width:2px; padding-left: 2px;">
                    </td>
                                            
                    <td style="width:9%;">
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td  width="25%">
                        <iMESMaintain:CmbMaintainLightNoPdLine  runat="server" ID="cmbMaintainLightNoPdLine" Width="93%" ></iMESMaintain:CmbMaintainLightNoPdLine >
                    </td> 
                    
                    <td style="width:10%;">
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td  width="25%">
                        <iMESMaintain:CmbMaintainFaKittingFamily  runat="server" ID="cmbMaintainFaKittingFamily" Width="93%" ></iMESMaintain:CmbMaintainFaKittingFamily >
                    </td> 
                    <td width="15%" >
                         <input type="button" id="btnQuery" runat="server" class="iMes_button" onserverclick="btnQuery_ServerClick" />
                    </td>                   
                    
                    <td width="16%" align="right">
                         <input type="button" id="btnUpload" class="iMes_button" runat="server" onclick="clkButton()" />
                    </td>
                 
                </tr>
                <tr style="height:5px">
                <td></td>
                </tr>
             </table>
             <table width="100%" border="0" >
                <tr>
                    <td style="width:50%; padding-left: 2px;">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                    </td>
                    <td width="50%" align="right" >
                       <input type="button" id="btnToExcel" runat="server" onclick="clkButton();" class="iMes_button" ></input>
                    </td>   
                </tr>
             </table>  
        </div>        
    
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <ContentTemplate>
        <div id="div2" style="height:510px;">
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%" 
                        RowStyle-Height="20" GvExtWidth="100%" GvExtHeight="500px" AutoHighlightScrollByValue ="true" 
                        HighLightRowPosition="3"  
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
                            <input type="hidden" id="dQueryLineValue" runat="server" /> 
                            <input type="hidden" id="dQueryFamilyValue" runat="server" /> 
        </div>
        </ContentTemplate>
        </asp:UpdatePanel> 
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
           </Triggers>
        </asp:UpdatePanel>
        <input type="hidden" id="HiddenUserName" runat="server" />
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

    <iframe name="exportframe" id="exportframe" src="FAKittingUploadExport.aspx" style="display:none;" ></iframe>  

    <script language="javascript" type="text/javascript">    
  
    var msg1="";
    var msg2="";
    var msg3="";
    var msg4="";
    var msg5="";
    var msg6="";
    window.onload = function()
    {
      //!!!修改message 
        msg1 ="<%=pmtMessage1%>";
              
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        //initControls();  
        //ShowRowEditInfo(null);
        //设置表格的高度  
        resetTableHeight();

    };
    
     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function(sender, e)
     {
        e.set_errorHandled(true); //表示自定义显示错误, 将默认的alert提示禁止掉.
        if(e.get_error()!=null && e.get_error()!="")
        {
            alert("Time out, please login again.");
            DealHideWait();                
        }
    });
    
//    function initControls()
//    {
//        getMaintainLightNoPdLineCmbObj().onchange=LightNoPdLineOnChange;   
//    }
    

    
     //设置表格的高度  
    function resetTableHeight()
    {
        //动态调整表格的高度
        var adjustValue=56;     
        var marginValue=12; 
        var tableHeigth=300;
        try{
            tableHeigth=document.body.parentElement.offsetHeight-document.getElementById("div1").offsetHeight-adjustValue;
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
    
    
    function clkButton()
    {
       //ShowInfo("");
       switch(event.srcElement.id)
       {
           case "<%=btnToExcel.ClientID %>":
               var lineValue=document.getElementById("<%=dQueryLineValue.ClientID %>").value; //getMaintainLightNoPdLineCmbObj().value.trim(); 
               if(lineValue=="")
               {
                   alert(msg1);
                   return;
               }
               
               var familyValue=document.getElementById("<%=dQueryFamilyValue.ClientID %>").value;//getMaintainFaKittingFamilyCmbObj().value.trim(); 
                if(window.frames["exportframe"].dealExport!=null)
                {
                    window.frames["exportframe"].dealExport(lineValue,familyValue);  
                }
                return; 
 	            break; 
           case "<%=btnUpload.ClientID %>":               
 
               var lineValue=getMaintainLightNoPdLineCmbObj().value.trim(); 
               if(lineValue=="")
               {
                   alert(msg1);
                   return;
               }
                         
               lineValue=encodeURI(Encode_URL2(lineValue));
               
               var editor=document.getElementById("<%=HiddenUserName.ClientID %>").value;
               var dlgFeature = "dialogHeight:260px;dialogWidth:424px;center:yes;status:no;help:no";       
               
               editor=encodeURI(Encode_URL2(editor));               
              
               var dlgReturn=window.showModalDialog("FAKittingUploadDlg.aspx?userName="+editor+"&line="+lineValue, window, dlgFeature);
               if(dlgReturn=="OK")
               {
                   document.getElementById("<%=btnQuery.ClientID %>").click();                   
                   ShowWait();
               }
               
               return; 
 	           break;
    	}  
        
        return true;
    }    
    
   
//   function DealComplete()
//   {   
//       //ShowRowEditInfo(null);
//   }
 
//    function setGdHighLight(con)
//    {
//        if((iSelectedRowIndex!=null) && (iSelectedRowIndex!=parseInt(con.index, 10)))
//        {
//            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex,false, "<%=gd.ClientID %>");                
//        }        
//        setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
//        iSelectedRowIndex=parseInt(con.index, 10);    
//    }
//    
    function DealHideWait()
    {    
        getMaintainLightNoPdLineCmbObj().disabled = false; 
        getMaintainFaKittingFamilyCmbObj().disabled = false; 
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

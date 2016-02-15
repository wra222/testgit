<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: MB Label Print(SA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-11-10  liu xiao-ling        Create 
 qa bug no:ITC-1136-0159
 --%>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="Process.aspx.cs" Inherits="Process" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<style>
    table.edit
    {
        height: 250px;
        margin: 0 0 0 0;
    }
    .iMes_wide_button
    {
	    border:Gray 1px ridge;
	    padding:2px;
	    margin-left:2px;
	    margin-right:2px;
	    cursor:hand; 
	    height: 20px;
	    width:150px;
	    font-size: 9pt;
	    text-align:center;
	    font-family:Verdana;
        font-weight:lighter;
	    vertical-align:middle;
        color: black;
	    FILTER:progid:DXImageTransform.Microsoft.Gradient(GradientType=0,StartColorStr=#CCCCCC, EndColorStr=#FFFFFF);   
    }
    
    .iMes_wide_button_onmouseout
    {
	    border:Gray 1px ridge;
	    padding:2px;
	    margin-left:2px;
	    margin-right:2px;
	    cursor:hand; 
	    height: 20px;
	    width:150px;
	    font-size: 9pt;
	    text-align:center;
	    font-family:Verdana;
        font-weight:lighter;
	    vertical-align:middle;
        color: black;
	    FILTER:progid:DXImageTransform.Microsoft.Gradient(GradientType=0,StartColorStr=#CCCCCC, EndColorStr=#FFFFFF);

    }

    .iMes_wide_button_onmouseover
    {
	    border:Gray 1px ridge;
	    padding:2px;
	    margin-left:2px;
	    margin-right:2px;
	    cursor:hand; 
	    height: 20px;
	    width:150px;
	    font-size: 9pt;
	    text-align:center;
	    font-family:Verdana;
        font-weight:lighter;
	    vertical-align:middle;
        color:ButtonHighlight;
        FILTER:progid:DXImageTransform.Microsoft.Gradient(GradientType=0,StartColorStr=#999999, EndColorStr=#FFFFFF);
    }
    
 <%--   
   fieldset{position:relative; top:0px;background-color:Transparent;  margin-left:10px; margin-right:10px;padding-bottom:3pt;padding-left:5pt;padding-right:5pt;}
    *html legend {margin: -10px 0 0 0;font-size:12pt;}--%>
    
</style>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <%--<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>--%>
    <asp:ScriptManager ID="ScriptManager2" runat="server" >
    </asp:ScriptManager>

<div>
    <div id="container" class="iMes_div_MainTainContainer">
    <div id="div1" class="iMes_div_MainTainDiv1">
        <table  width="100%" border=0 cellpadding=0 cellspacing=0 >
        <tr style="width:100%">
            <td>
                <table width="100%" class="edit" border ="0" >
                <tr style="height:5px;">  
                   <td style="width:64%"></td> 
                   <td style="width:36%"></td>              
                </tr>
                <tr style ="height:25px;">
                    <td align="left" style="width:64%">
                        <asp:Label ID="lblProcessList" runat="server" CssClass="iMes_label_13pt" />                    
                    </td>
                    <td style="width:36%">
                    </td>                 
                </tr>
                <tr >
                    <td align="left"  style="width:64%">
                       
                            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate>
                                <iMES:GridViewExt ID="gdProcess" runat="server" AutoGenerateColumns="true" Width="150%" 
                                    GvExtWidth="100%" GvExtHeight="206px" OnRowDataBound="gdProcess_RowDataBound"
                                     OnGvExtRowClick="clickProcessTable(this)" 
                                     AutoHighlightScrollByValue ="true" HighLightRowPosition="3" SetTemplateValueEnable="False"  EnableViewState= "false">
                                </iMES:GridViewExt>
                            </ContentTemplate>   
                            </asp:UpdatePanel>                         
                    </td>
                    <td style="width:36%">
                            <table style="width:100%;"  >
                            <tr>
                                <td style="width:26%; padding-left:5px;height:28px;">
                                    <asp:Label  ID="lblProcess" runat="server" CssClass="iMes_label_13pt" Width="100%"></asp:Label>
                                </td>
                                <td style="width:74%;" >
                                    <asp:TextBox id="txtProcess" runat="server" Width="97%" SkinId="textBoxSkin" MaxLength=10 ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left:5px;height:28px;">
                                    <asp:Label ID="lblDescription" runat="server" CssClass="iMes_label_13pt"  Width="100%"></asp:Label>
                                </td>
                                <td >
                                    <asp:TextBox id="txtDescription" runat="server" MaxLength=80 Width="97%" SkinId="textBoxSkin"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td style="padding-left:5px;height:28px;">
                                    <asp:Label ID="lblProcessType" runat="server" CssClass="iMes_label_13pt"  Width="100%"></asp:Label>
                                </td>
                                <td >
                                    <iMESMaintain:CmbMaintainProcessType runat="server" ID="cmbMaintainProcessType" Width="99%" ></iMESMaintain:CmbMaintainProcessType>
                                </td> 
                                                    
                            </tr>

                            <tr>
                                <td colspan ="2" align="right" style="padding-right:0px;height:28px;">
                                    <input id="btnSaveAs" type="button"  onclick="clkButton()" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                                    <input id="btnAdd1" type="button"  onclick="if(clkSave1())" onserverclick="btnAddProcess_Click"  runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                                    <input id="btnDelete1" type="button" onserverclick="btnDeleteProcess_Click" onclick="if(clkDelete())" runat="server" class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                                </td>
                            </tr>
                           <tr><td height="3px"></td></tr>                        
                           <tr>
                                <td colspan ="2" align="right" style="padding-left:5px;height:28px;">
                                    <input id="btnImport" type="button"  onclick="if(clkButton())" onserverclick="btnImport_Click" runat="server" class="iMes_wide_button" onmouseover="this.className='iMes_wide_button_onmouseover'" onmouseout="this.className='iMes_wide_button_onmouseout'"/>&nbsp;&nbsp;
                                    <input id="btnExport" type="button" onclick="clkButton();" runat="server" class="iMes_wide_button"  onmouseover="this.className='iMes_wide_button_onmouseover'" onmouseout="this.className='iMes_wide_button_onmouseout'"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan ="2" align="right" style="padding-left:5px;height:28px;">
                                    <input id="btnProcessRule" type="button"  onclick="clkButton();"  runat="server" class="iMes_wide_button" onmouseover="this.className='iMes_wide_button_onmouseover'" onmouseout="this.className='iMes_wide_button_onmouseout'"/>&nbsp;&nbsp;
                                    <input id="btnProcessRuleSet" type="button" onclick="clkButton();" runat="server" class="iMes_wide_button"  onmouseover="this.className='iMes_wide_button_onmouseover'" onmouseout="this.className='iMes_wide_button_onmouseout'"/>
                                </td>
                            </tr>
                            
                            </table>
                    </td>
                </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table border=0>
                    <tr>
                        <td>
                            <asp:Label ID="lblStationList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                    </tr>
                       
                </table>
            </td>
        </tr>
        </table>
    </div>
    
    <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
       <div id="div2" style="height:190px">
            <iMES:GridViewExt ID="gdProcessStation" runat="server" AutoGenerateColumns="true" Width="99.9%" 
                GvExtWidth="100%" GvExtHeight="180px" Height="170px" OnRowDataBound="gdProcessStation_RowDataBound"
                 OnGvExtRowClick="clickProcessStationTable(this)"
                 AutoHighlightScrollByValue ="true" HighLightRowPosition="3" SetTemplateValueEnable="False" EnableViewState= "false">
            </iMES:GridViewExt>
        </div>
    </ContentTemplate>   
    </asp:UpdatePanel>                         
    <button id="btnRefreshProcessStationList" runat="server" type="button" onserverclick="btnRefreshProcessStationList_Click" style="display: none"></button>
    
    <div id="div3">
       
      <table width="100%" class="iMes_div_MainTainEdit" >
            <tr>
                <td  style="width:11%;">
                    <asp:Label ID="lblPreStation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td  width="32%">
                    <iMESMaintain:CmbPreStationForMaintain  ID="selPreStation" runat="server" Width="96%" IsPercentage="false"/>
                </td>              
                <td style="width:11%; padding-left:2px;">
                    <asp:Label ID="lblStatus" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="32%">
                    <asp:DropDownList id="selStationStatus"  runat="server" Width="96%"></asp:DropDownList>
                </td>
                      
                <td width="14%" align="right">
                    <input id="btnSave2" type="button" onserverclick="btnSaveStation_Click" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                </td>
            </tr>
            <tr>
                <td style="width:11%; padding-left:2px;">
                    <asp:Label ID="lblStation" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td  width="32%">
                    <iMESMaintain:CmbStationForMaintain  ID="selStation" runat="server" Width="96%" IsPercentage="false"/>
                </td>
                <td></td>
                <td></td>
                <td align="right">
                    <input id="btnDelete2" type="button" runat="server" class="iMes_button" onclick="if(clkDelete())" onserverclick="btnDeleteStation_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                </td>
            </tr>
           
         </table>
       
   </div>
   
   <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnImport" EventName="ServerClick" />
        <asp:AsyncPostBackTrigger ControlID="btnExport" EventName="ServerClick" /> 
        <asp:AsyncPostBackTrigger ControlID="btnProcessRule" EventName="ServerClick" /> 
        <asp:AsyncPostBackTrigger ControlID="btnProcessRuleSet" EventName="ServerClick" />
        <asp:AsyncPostBackTrigger ControlID="btnDelete1" EventName="ServerClick" />
        <asp:AsyncPostBackTrigger ControlID="btnAdd1" EventName="ServerClick" />
        <asp:AsyncPostBackTrigger ControlID="btnSave2" EventName="ServerClick" />
        <asp:AsyncPostBackTrigger ControlID="btnDelete2" EventName="ServerClick" />
        <asp:AsyncPostBackTrigger ControlID="btnRefreshProcessStationList" EventName="ServerClick" />
        <asp:AsyncPostBackTrigger ControlID="btnSaveAsRefresh" EventName="ServerClick" />
    </Triggers>                      
   </asp:UpdatePanel>  
   <input type="hidden" id="hidProcess" runat="server" value=""/>
   <input type="hidden" id="hidType" runat="server" value=""/>
   <input type="hidden" id="dTableHeight" runat="server" />   
   <input type="hidden" id="HiddenUserName" runat="server" />
   <input type="hidden" id="hidStation" runat="server" value=""/>
   <input type="hidden" id="hidPreStation" runat="server" value=""/>
   <input type="hidden" id="hidProcessStationID" runat="server" value=""/>
   <input type="hidden" id="importProcess" runat="server" value=""/>
   <input type="button" id="btnSaveAsRefresh" runat="server" style="display:none" onserverclick ="btnSaveAsRefresh_ServerClick"/>

</div>
<div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
    <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
        <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
    </table>
</div> 
<iframe name="exportframe" id="exportframe" src="ProcessDownloadFile.aspx" style="display:none;" ></iframe>  
 
<script for=window event=onload>

    resetTableHeight();
    ShowRowEditInfoProcess(null)
    ShowRowEditInfoProcessStation(null);
    isWindowLoad=true;
</script> 

<script type="text/javascript">

     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function(sender, e)
     {
        e.set_errorHandled(true); //表示自定义显示错误, 将默认的alert提示禁止掉.
        if(e.get_error()!=null && e.get_error()!="")
        {
            alert("Time out, please login again.");
            DealHideWait();                
        }
    });

    var selectedRowIndex_Process = null;
    var selectedRowIndex_ProcessStation = null;
    var isWindowLoad=false;
    var msgDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDelete").ToString() %>';
    var msgPrompt1='<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrompt1").ToString() %>';
        
    function clkButton()
    {
       //ShowInfo("");
       switch(event.srcElement.id)
       {
          case "<%=btnImport.ClientID %>":    
               var editor=document.getElementById("<%=HiddenUserName.ClientID %>").value;
               var dlgFeature = "dialogHeight:200px;dialogWidth:424px;center:yes;status:no;help:no";       
               
               editor=encodeURI(Encode_URL2(editor));
               var dlgReturn=window.showModalDialog("ProcessUploadFile.aspx?userName="+editor, window, dlgFeature);
               //!!!need添加数据刷新
               if(dlgReturn=="")
               {
                   return false;              
               }
               else 
               { 
                   document.getElementById("<%=importProcess.ClientID%>").value=dlgReturn;
                   ShowWait();
                   return true;                   
               }
              
               break;    
          case "<%=btnExport.ClientID %>":    
               
//               var strProcessName =document.getElementById("<%=hidProcess.ClientID%>").value; 
//               if(strProcessName!="")
//               {
//                   return true;              
//               }
//               else 
//               {
//                   return false;
//               }
//               var process=document.getElementById("<%=hidProcess.ClientID %>").value.trim();
//               process=encodeURI(Encode_URL2(process));
//               
//               var oNewWindow=null; 
//               oNewWindow=window.open("ProcessDownloadFile.aspx?process="+process,"_blank",
//              "fullscreen=no,status=no,titlebar=no,toolbar=no,menubar=no,location=no,directories =no,scrollbars =no");   

               var process=document.getElementById("<%=hidProcess.ClientID %>").value.trim();
               //process=encodeURI(Encode_URL2(process)); 
               //window.frames["exportframe"].currentProcess=process;
               window.frames["exportframe"].dealExport(process);       
               return;
               //break;                    
           case "<%=btnProcessRule.ClientID %>":    
                var gdObj=document.getElementById("<%=gdProcess.ClientID %>");
                var iIndex=selectedRowIndex_Process+1;
                //gdObj.rows计算时带标题
                var con=gdObj.rows[iIndex];
                dbClickProcessTable(con);
                break;
           case "<%=btnProcessRuleSet.ClientID %>":    
           
              var userName=document.getElementById("<%=HiddenUserName.ClientID %>").value;
              userName=encodeURI(Encode_URL2(userName));
              
              var strProcessName =document.getElementById("<%=hidProcess.ClientID%>").value;
              strProcessName=encodeURI(Encode_URL2(strProcessName)); 
              //var dlgFeature = "dialogHeight:514px;dialogWidth:900px;center:yes;status:no;help:no";
              var dlgFeature = "dialogHeight:560px;dialogWidth:900px;center:yes;";
              var dlgReturn = window.showModalDialog("ModelProcessRuleSetSetting.aspx?ProcessName=" + strProcessName + "&userName=" + userName + "&UserId=" + userName + "&Customer=&AccountId=1&Login=", window, dlgFeature);
            
              break; 
              
           case "<%=btnSaveAs.ClientID %>":    
           
              var userName=document.getElementById("<%=HiddenUserName.ClientID %>").value;
        
              var strProcessName =document.getElementById("<%=hidProcess.ClientID%>").value; 
              var strType =document.getElementById("<%=hidType.ClientID%>").value; 
              
              userName=encodeURI(Encode_URL2(userName)); 
              strProcessName=encodeURI(Encode_URL2(strProcessName)); 
              strType=encodeURI(Encode_URL2(strType)); 
              
              //var dlgFeature = "dialogHeight:514px;dialogWidth:900px;center:yes;status:no;help:no";
              var dlgFeature = "dialogHeight:260px;dialogWidth:458px;center:yes;";
              var dlgReturn = window.showModalDialog("ProcessSaveAs.aspx?Process=" + strProcessName+"&userName="+userName+"&Type="+strType, window, dlgFeature);
              if(dlgReturn!=null)
              {
                  var processValue=dlgReturn;
                  document.getElementById("<%=importProcess.ClientID%>").value=processValue;
                  document.getElementById("<%=btnSaveAsRefresh.ClientID %>").click();
                  ShowWait();
              }
              break; 
                        
       }
         
       return true;    
    }
    
    function dbClickProcessTable(con)
    {
    
        var curProcessType =con.cells[0].innerText.trim();  
        var curProcess =con.cells[1].innerText.trim();
        if(curProcess=="")
        {
            return;
        }
                //alert(curProcess)
        curProcess=encodeURI(Encode_URL2(curProcess));                    
        var editor=document.getElementById("<%=HiddenUserName.ClientID %>").value;
        editor=encodeURI(Encode_URL2(editor));

        if (curProcessType == "<%=productType %>") {
            //var dlgFeature = "dialogHeight:470px;dialogWidth:860px;center:yes;status:no;help:no;help:no;scroll:no;";   
            var dlgFeature = "dialogHeight:502px;dialogWidth:860px;center:yes;";
            var dlgReturn = window.showModalDialog("ModelProcessRuleSetting.aspx?userName=" + editor + "&ProcessName=" + curProcess + "&UserId=" + editor + "&Customer=&AccountId=1&Login=", window, dlgFeature);
        }
        else if (curProcessType == "<%=pcbType %>") {
            //PCB
            var dlgFeature = "dialogHeight:480px;dialogWidth:540px;center:yes;";           
            var dlgReturn=window.showModalDialog("PCBProcessRelation.aspx?userName="+editor+"&ProcessName="+curProcess, window, dlgFeature);
        }
        else if (curProcessType == "<%=MaterialType %>") 
        {
            //Material
            
            var dlgFeature = "dialogHeight:480px;dialogWidth:540px;center:yes;";
            var dlgReturn = window.showModalDialog("MaterialProcessRelation.aspx?userName=" + editor + "&ProcessName=" + curProcess, window, dlgFeature);
        }


    }

    //设置表格的高度  
    function resetTableHeight()
    {    
        //动态调整表格的高度
        var adjustValue=62;     
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
        document.getElementById("div_<%=gdProcessStation.ClientID %>").style.height=tableHeigth+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
    }
    

    function setGdHighLightProcess(con)
    {
        if((selectedRowIndex_Process!=null) && (selectedRowIndex_Process!=parseInt(con.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex_Process,false, "<%=gdProcess.ClientID %>");                
        }        
        setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gdProcess.ClientID %>");
        selectedRowIndex_Process=parseInt(con.index, 10);    
    }
    
    function setNewItemValueProcess()
    {
        document.getElementById("<%=txtProcess.ClientID%>").value = "";
        document.getElementById("<%=hidProcess.ClientID%>").value = "";
        document.getElementById("<%=txtDescription.ClientID%>").value = "";
        getMaintainProcessTypeCmbObj().selectedIndex=0;
        
        document.getElementById("<%=btnDelete1.ClientID%>").disabled = true;
        document.getElementById("<%=btnSaveAs.ClientID%>").disabled = true;
        document.getElementById("<%=btnExport.ClientID%>").disabled = true;
        document.getElementById("<%=btnProcessRule.ClientID%>").disabled = true;        
        document.getElementById("<%=btnSave2.ClientID%>").disabled = true;
        
    }
    

    function ShowRowEditInfoProcess(con)
    {
         if(con==null)
         {
            setNewItemValueProcess();
            return;    
         }

        var currentId=con.cells[1].innerText.trim(); 
        document.getElementById("<%=txtProcess.ClientID%>").value = currentId;
        document.getElementById("<%=hidProcess.ClientID%>").value =currentId; 
        getMaintainProcessTypeCmbObj().value =con.cells[0].innerText.trim();
        document.getElementById("<%=hidType.ClientID%>").value =con.cells[0].innerText.trim();;
        document.getElementById("<%=txtDescription.ClientID%>").value = con.cells[2].innerText.trim();
         if(currentId=="")
         {
            setNewItemValueProcess();
            return;
         }
         else
         {
            document.getElementById("<%=btnDelete1.ClientID%>").disabled = false;
            document.getElementById("<%=btnSaveAs.ClientID%>").disabled = false;
            document.getElementById("<%=btnExport.ClientID%>").disabled = false;
            document.getElementById("<%=btnProcessRule.ClientID%>").disabled = false;
            
            document.getElementById("<%=btnSave2.ClientID%>").disabled = false;
         }
    }
   

    function clickProcessTable(con)
    {
        
        if(isWindowLoad!=true)
        {
           return;
        }
        
        if(con!=null)
        {
            setGdHighLightProcess(con);
        }
        if (con.cells[0].innerText.trim() == "Material") 
        {
            document.getElementById("<%=btnProcessRuleSet.ClientID%>").disabled = true;
        }
        
        ShowRowEditInfoProcess(con);
        document.getElementById("<%=btnRefreshProcessStationList.ClientID %>").click();
        ShowWait();
        //beginWaitingCoverDiv();
        
    }


   function clkDelete()
   {
       if(confirm(msgDelete))
       {
           ShowInfo("");
           return true;
       }   
       return false;
        
   }

   function clkSave1()
   {
       ShowInfo("");
       var process = document.getElementById("<%=txtProcess.ClientID %>").value;   
       if(process.trim()=="")
       {
           alert(msgPrompt1);
           return false;
       }   
       return true;
        
   }
   
    function AddProcessComplete(process)
    {
//        var con;
//        if(process=="")
//        {
//            con=null;
//        }
//        else
//        {
//            var gdProcessClientID="<%=gdProcess.ClientID%>";            //!!!0->1
//            con=eval("setScrollTopForGvExt_"+gdProcessClientID+"('"+process+"',1)");
//        }
        
        
        var gdObj=document.getElementById("<%=gdProcess.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           if(gdObj.rows[i].cells[1].innerText==process)
           {
               selectedRowIndex=i; 
               break; 
           }        
        }
        
        if(selectedRowIndex<0)
        {
             ShowRowEditInfoProcess(null);                
        }
        else
        {            
            var con=gdObj.rows[selectedRowIndex];
            setGdHighLightProcess(con);
            setSrollByIndex(selectedRowIndex_Process, true, "<%=gdProcess.ClientID%>");
            ShowRowEditInfoProcess(con);
        }        
        
        document.getElementById("<%=btnRefreshProcessStationList.ClientID %>").click();
        ShowWait();
        //clickProcessTable(con);
    }
  

    function setNewItemValueProcessStation()
    {
        
        getStationCmbObj().selectedIndex=0;
        getPreStationCmbObj().selectedIndex=0;
        document.getElementById("<%=hidStation.ClientID%>").value = "";
        document.getElementById("<%=hidPreStation.ClientID%>").value = "";
        document.getElementById("<%=selStationStatus.ClientID%>").selectedIndex=0
        document.getElementById("<%=hidProcessStationID.ClientID%>").value ="";

       document.getElementById("<%=btnDelete2.ClientID%>").disabled = true;
    
    }

    function ShowRowEditInfoProcessStation(con)
    {
         if(con==null)
         {
            setNewItemValueProcessStation();
            return;    
         }
        
        var currentId=con.cells[0].innerText.trim();
        getStationCmbObj().value = con.cells[3].innerText.trim();
        document.getElementById("<%=hidStation.ClientID%>").value = con.cells[3].innerText.trim();
        getPreStationCmbObj().value = con.cells[1].innerText.trim();
        document.getElementById("<%=hidPreStation.ClientID%>").value = con.cells[1].innerText.trim();
        document.getElementById("<%=selStationStatus.ClientID%>").value = con.cells[2].innerText.trim();
        document.getElementById("<%=hidProcessStationID.ClientID%>").value =currentId;
        
         if(currentId=="")
         {
            setNewItemValueProcessStation();
            return;
         }
         else
         {
             document.getElementById("<%=btnDelete2.ClientID%>").disabled=false;
         }
                  
    }
    
    function setGdHighLightProcessStation(con)
    {
        if((selectedRowIndex_ProcessStation!=null) && (selectedRowIndex_ProcessStation!=parseInt(con.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex_ProcessStation,false, "<%=gdProcessStation.ClientID %>");                
        }        
        setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gdProcessStation.ClientID %>");
        selectedRowIndex_ProcessStation=parseInt(con.index, 10);    
    }
   
    function clickProcessStationTable(con)
    {
        setGdHighLightProcessStation(con);
        ShowRowEditInfoProcessStation(con);
        
    }        
    
    //ok
    function AddUpdateStationComplete(id)
    {
       var gdObj=document.getElementById("<%=gdProcessStation.ClientID %>");
        
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
            ShowRowEditInfoProcessStation(null);    
            return;
        }
        else
        {            
            var con=gdObj.rows[selectedRowIndex];
            setGdHighLightProcessStation(con);
            setSrollByIndex(selectedRowIndex_ProcessStation, true, "<%=gdProcessStation.ClientID%>");
            ShowRowEditInfoProcessStation(con);
            
        }        
    }
        
    
    //!!!ok need delete
    function DeleteStationComplete()
    {
        ShowRowEditInfoProcessStation(null);
    }
    
    function DealHideWait()
    {
        getMaintainProcessTypeCmbObj().disabled = false; 
        getStationCmbObj().disabled = false; 
        getPreStationCmbObj().disabled = false; 
        document.getElementById("<%=selStationStatus.ClientID%>").disabled = false;   
        HideWait();   
    }   
    
    
//    function btnProcessRelation_Click()
//    {
//        var strProcessName = document.getElementById("<%=hidProcess.ClientID%>").value;
//        var dlgFeature = "dialogHeight:450px;dialogWidth:880px;center:yes;status:no;help:no";
//        var dlgReturn=window.showModalDialog("ProcessRelation.aspx?ProcessName="+strProcessName, window, dlgFeature);

//    }    

</script>
</asp:Content>


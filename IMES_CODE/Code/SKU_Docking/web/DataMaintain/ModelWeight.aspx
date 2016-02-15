<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ModelWeight.aspx.cs" Inherits="DataMaintain_ModelWeight" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer" style="height:94%">
        <div style="width:100%;height:20px;"></div>
        <div id="div1" style="margin-top:5px;margin-bottom:5px;">
             <table width="100%" style="background-color: #99CDFF; margin:0 0 20 0;">            
                <tr>
                    <td style="width:22%;height:32px;">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="78%">
                        <asp:TextBox ID="dModel" runat="server"   MaxLength="20"  Width="92%" SkinId="textBoxSkin" onkeypress='OnKeyPress(this)' ></asp:TextBox>
                    </td>   
                    
                </tr>
                <tr >                           
                    <td style="width:22%;height:32px;">
                        <asp:Label ID="lblStandardWeight" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="78%">                  
                        <asp:TextBox ID="dStandardWeight" runat="server"   MaxLength="8"  Width="92%" SkinId="textBoxSkin" onkeypress='OnKeyPress(this)' ></asp:TextBox>
                    </td>
                </tr>
             </table>  
                                                    
             
        </div>
   
        
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGetUnitWeight" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
        </Triggers>                      
        </asp:UpdatePanel>         
           <input type="hidden" id="HiddenUserName" runat="server" /> 
           <button id="btnGetUnitWeight" runat="server" type="button" style="display:none" onserverclick ="btnGetUnitWeight_ServerClick"> </button>
           <button id="btnSave" runat="server" type="button" style="display:none" onserverclick ="btnSave_ServerClick"> </button>
          
    </div>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    <script language="javascript" type="text/javascript">
    
    var msg1="";
    var msg2="";
    
    function OnKeyPress(obj)
    {
        var key = event.keyCode;
        
        if (key==13)//enter
        {
            if(event.srcElement.id=="<%=dModel.ClientID %>")
            {               
                document.getElementById("<%=btnGetUnitWeight.ClientID %>").click();
                ShowWait();                
            }
            else if(event.srcElement.id=="<%=dStandardWeight.ClientID %>")
            {
                if(check()==false)
                {
                    //Input data format is not correct
                    //alert("输入数据的格式不正确。")
                    alert(msg1)
                    return;
                }
                document.getElementById("<%=btnSave.ClientID %>").click();
                ShowWait();             
            }
        }       

    }
    
    //!!!
    function check()
    {
    
       var valueWeight=document.getElementById("<%=dStandardWeight.ClientID %>").value;
        //则检查本框输入内容是否满足格式要求
        var reExp = /^[0-9]+(\.[0-9]?[0-9]?)?$/;
	    if (reExp.exec(valueWeight)==null||reExp.exec(valueWeight)==false){
		    return false;
	    }
	    	    
	    if(parseFloat(valueWeight)>100)
	    {	    
	        return false;
	    }
	    
        return true;
    }
    
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
           
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        
    };

    
   function GetUnitWeightComplete(isGetOK, weightNum)
   {   
       if(isGetOK=="True")
       {
           document.getElementById("<%=dStandardWeight.ClientID %>").value =weightNum;
           trySetFocusStandardWeight()
       }
       else 
       {           
           document.getElementById("<%=dModel.ClientID %>").value ="";
           document.getElementById("<%=dStandardWeight.ClientID %>").value ="";
       }       
   }
   
    function trySetFocusModel()
    {
         var itemObj=document.getElementById("<%=dModel.ClientID %>");
         
         if(itemObj!=null && itemObj!=undefined && itemObj.disabled!=true)
         {
            itemObj.focus();
         }
    }
   
    function trySetFocusStandardWeight()
    {
         var itemObj=document.getElementById("<%=dStandardWeight.ClientID %>");
         
         if(itemObj!=null && itemObj!=undefined && itemObj.disabled!=true)
         {
            itemObj.focus();
         }
    }
   
    function SaveComplete(isSaveOK)
    {  
       if(isSaveOK=="True")
       {
       
           alert(msg2);
           //alert("Save Successfully!");
           document.getElementById("<%=dModel.ClientID %>").value ="";
           document.getElementById("<%=dStandardWeight.ClientID %>").value ="";
           trySetFocusModel();
       }        
    }
   
   
    function checkCodePress(obj)
    { 
       var key = event.keyCode;

       if(!(key >= 48 && key <= 57))
       {  
           event.keyCode = 0;
       }
 
     }
     
    function DealHideWait()
    {
        HideWait();   
    }


    </script>
</asp:Content>


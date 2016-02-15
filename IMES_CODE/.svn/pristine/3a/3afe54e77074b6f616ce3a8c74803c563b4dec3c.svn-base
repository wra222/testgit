<%@ Register src="../CommonControl/CmbSepcial.ascx" tagname="CmbSepcial" tagprefix="uc1" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" EnableEventValidation = "false" AutoEventWireup="true" CodeFile="GenerateSMTMO.aspx.cs" Inherits="DOCKING_GenerateSMTMO" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceGeneSMTMO.asmx" />
        </Services>
    </asp:ScriptManager>
   
    <center>
    <table border="0" width="95%">     
    <tr>
        <td style="width:15%" align="left"><asp:Label ID="lblMBCOCE" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td style="width:35%" align="left"><iMES:CmbMBCodeSMTMODocking ID="CmbMBCode" runat="server" Width="100" IsPercentage="true"/></td> 
        <td style="width:5%">&nbsp;</td>       
        <td style="width:15%" align="center"><asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td style="width:25%" align="left" colspan="2"><iMES:CmbModel ID="CmbModel" runat="server" Width="90" IsPercentage="true"/></td>
       
    </tr>

    <tr>
        <td style="width:18%" align="left"><asp:Label ID="lblProType" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td style="width:30%" align="left" colspan="5">
            <input type="radio" id="radQuan" name="optProduct" runat="server"  onclick="" />
            <asp:Label ID="lblQuan" runat="server" CssClass="iMes_label_13pt" ></asp:Label>           
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input type="radio" id="radTest" name="optProduct" runat="server"  onclick="" />
            <asp:Label ID="lblTest" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input type="radio" id="radTrial" name="optProduct"  runat="server" onclick=""/>            
            <asp:Label ID="lblTrial" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input type="radio" id="radPilot" name="optProduct"  runat="server" onclick=""/>            
            <asp:Label ID="lblPilot" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input type="radio" id="radTrace" name="optProduct"  runat="server" onclick=""/>            
            <asp:Label ID="lblTrace" runat="server" CssClass="iMes_label_13pt"></asp:Label>        
        </td>               
    </tr>

    <tr>
        <td style="width:18%" align="left"><asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td colspan="5"><input type="text" id="txtQty" onkeypress="inputQtyNumber(this)"  runat="server" class="iMes_textbox_input_Yellow" />
            <asp:Label ID="lblrange" runat="server" CssClass="iMes_label_11pt_Pink">></asp:Label></td>        
    </tr>

    <tr>
        <td style="width:18%" align="left">
            <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
        </td>
        <td colspan ="4" align="left">
            <textarea id="MessageTextArea1" style="width:90%; height: 30px" ></textarea><asp:Label ID="lblRemarkLimited" runat="server" CssClass="iMes_label_11pt_Pink"></asp:Label>
        </td>

        <td style="width:5%" align="left"><input id="btnGenerate" type="button" onclick="generate()"   runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/></td>
    </tr>
   

    <tr>
     
        <td colspan ="6" valign="top">
        <fieldset id="grpCarton">
            <legend align ="left" style ="height :20px" ><asp:Label ID="lblUnprintTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label></legend>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                    <ContentTemplate>
                        <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" 
                                                        AutoHighlightScrollByValue="false" GvExtWidth="100%" Width="99.9%" GvExtHeight="230px" 
                        OnGvExtRowClick="" OnGvExtRowDblClick=""  SetTemplateValueEnable="false"  GetTemplateValueEnable="false"  
                                                        HighLightRowPosition="3" onrowdatabound="GridViewExt1_RowDataBound" Height="220px" >
                            <Columns >  
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="HeaderChk"  runat="server" onclick="ChkAllClick(this)"   />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="RowChk" runat="server" onclick="CheckClick(this)"/>
                                </ItemTemplate>
                             </asp:TemplateField> 
                               <asp:BoundField DataField="MO"  />    
                               <asp:BoundField DataField="MBCODE"  />
                                <asp:BoundField DataField="Descr"  />   
                                <asp:BoundField DataField="Model"  />   
                                <asp:BoundField DataField="Qty"  />   
                                <asp:BoundField DataField="PQty"  />   
                                <asp:BoundField DataField="Remark"  />   
                                <asp:BoundField DataField="Cdt"  />   
                            </Columns>

                        </iMES:GridViewExt>
                    </ContentTemplate>   
                     <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick"  />
                        <asp:AsyncPostBackTrigger ControlID="btnHidden" EventName="ServerClick"  />
                        <asp:PostBackTrigger ControlID="btnToExl" />                      
                    </Triggers>                                     
                </asp:UpdatePanel>
                 <table width="100%"  >                    
                    <tr>
                        <td width ="70%"></td>
                        <td align ="left">
                            <input id="btnQuery" type="button" onclick="" onserverclick="queryClick" class="iMes_button" runat="server" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                        </td>
                        <td align ="center" >
                            <input id="btnDel" type="button" onclick="BtnDelClick()"  class="iMes_button" runat="server" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                        </td>
                        <td align="right"  style="width:100px">
                           <input id="btnToExl" type="button" onclick="" onserverclick="excelClick" class="iMes_button" runat="server" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />                            
                             <asp:UpdatePanel ID="updatePanelAll" runat="server"  RenderMode="Inline"   >
                                <ContentTemplate>
                                <input type="hidden" runat="server" id="station" /> 
                                <button id="btnHidden" runat="server" onserverclick="btnHidden_Click" style="display: none" >
                                </button> 
                                </ContentTemplate>   
                            </asp:UpdatePanel>
                        </td>
                        
                    </tr>
                    </table> 
         
                   
        </fieldset>
        </td>
    </tr>
    </table>
    </center> 
    
    
</div>


<script  language="javascript">
var gvClientID="<%=GridViewExt1.ClientID %>";
var gvTable=document.getElementById (gvClientID);
var gvHeaderID="<%=GridViewExt1.HeaderRow.ClientID%>";
var mesNoSelModel = '<%=this.GetLocalResourceObject(pre + "_mesNoSelectModel").ToString()%>';
var mesNoSelectMBCode = '<%=this.GetLocalResourceObject(pre + "_mesNoSelectMBCode").ToString()%>';
var mesNoInputQty = '<%=this.GetLocalResourceObject(pre + "_mesNoInputQty").ToString()%>';
var mesNoSeRecord = '<%=this.GetLocalResourceObject(pre + "_mesNoSelRecord").ToString()%>';
var mesDelConfirm = '<%=this.GetLocalResourceObject(pre + "_mesDelConfirm").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(pre + "_msgSystemError") %>';
var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(pre + "_msgSuccess") %>';
var msgRemarkLength='<%=this.GetLocalResourceObject(pre + "_mesRemarkLength").ToString()%>';
var mesQtyIllegal = '<%=this.GetLocalResourceObject(pre + "_mesQtyIllegal").ToString()%>';
var mesNoSelectProductType = '<%=this.GetLocalResourceObject(pre + "_mesNoSelectProductType").ToString()%>';
var mesGiveupCreate = '<%=this.GetLocalResourceObject(pre + "_mesGiveupCreate").ToString()%>';


var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";

document.body.onload = function () {
    document.getElementById("<%=radQuan.ClientID%>").checked = true;
}

function generate()
{
  
    var errorFlag = false;
    var qty = document.getElementById("<%=txtQty.ClientID%>").value;
    var remark = document.getElementById("MessageTextArea1").value; 
    var station = document.getElementById("<%=station.ClientID%>").value;
          
    if (getMBCodeCmbValue() == "") {
        errorFlag = true;
        alert(mesNoSelectMBCode);
        setMBCodeCmbFocus();
    } else if (getModelCmbValue() == "") {
        errorFlag = true;
        alert(mesNoSelModel);
        setModelCmbFocus();
    } else if (!document.getElementById("<%=radQuan.ClientID%>").checked &&
            !document.getElementById("<%=radTest.ClientID%>").checked && 
            !document.getElementById("<%=radTrial.ClientID %>").checked &&
            !document.getElementById("<%=radPilot.ClientID %>").checked &&
            !document.getElementById("<%=radTrace.ClientID %>").checked) {
        errorFlag = true;
        alert(mesNoSelectProductType);        
    } else if (qty == "") {
        errorFlag = true;
        alert(mesNoInputQty);
        document.getElementById("<%=txtQty.ClientID%>").focus();
    } else if (checkQtyNumber(qty)){
        errorFlag = true;
        alert(mesQtyIllegal);
        document.getElementById("<%=txtQty.ClientID%>").focus();
    } else if (remark.length > 255) {
        errorFlag = true;
        alert(msgRemarkLength);
        document.getElementById("MessageTextArea1").select();
    } 

    if (!errorFlag) {
        var isMass;
        if (document.getElementById("<%=radQuan.ClientID%>").checked) {
            isMass = "M";
        } else if (document.getElementById("<%=radTest.ClientID%>").checked) {
            isMass = "P";
        } else if (document.getElementById("<%=radTrial.ClientID%>").checked) {
            isMass = "T";
        } else if (document.getElementById("<%=radPilot.ClientID%>").checked) {
            isMass = "S";
        }else {       
            isMass = "A";
        }
       //调用web service提供的打印接口 
        beginWaitingCoverDiv();
        WebServiceGeneSMTMO.generate(getModelCmbValue(), qty, isMass, remark, "<%=userId%>", station, "<%=customer%>", onSucceed, onFail);        
    }        
}

function onSucceed(result)
{    
    try {
        endWaitingCoverDiv();
        document.getElementById("<%=radTest.ClientID%>").checked = false;
        document.getElementById("<%=radQuan.ClientID%>").checked = false;
        document.getElementById("<%=radTrial.ClientID %>").checked = false;
        document.getElementById("<%=radPilot.ClientID %>").checked = false;
        document.getElementById("<%=radTrace.ClientID %>").checked = false;
        /* Add 是否需要创建MO窗口*/
        if(result.length==1)
        {
             ShowMessage(result[0]);
             ShowInfo(result[0]);           
        }
        else
        {
            MoKEY = result[1].split(",");
            
            ShowSuccessfulInfo(true, "[" + MoKEY[0] + "] " + " is Generated");
            if (result[2] == "M") {
                document.getElementById("<%=radQuan.ClientID%>").checked = true;
            } else if (result[2] == "P") {
                document.getElementById("<%=radTest.ClientID%>").checked = true;
            } else if (result[2] == "T") {
                document.getElementById("<%=radTrial.ClientID%>").checked = true;
            } else if (result[2] == "S") {
                document.getElementById("<%=radPilot.ClientID%>").checked = true;
            } else {
                document.getElementById("<%=radTrace.ClientID %>").checked = true;
            }            
        }                
    } catch(e) {
        alert(e.description);
        endWaitingCoverDiv();
    }       
}

function onFail(error)
{
    
    try {
        endWaitingCoverDiv();
        document.getElementById("<%=radTest.ClientID%>").checked = false;
        document.getElementById("<%=radQuan.ClientID%>").checked = false;
        document.getElementById("<%=radTrial.ClientID %>").checked = false;
        document.getElementById("<%=radPilot.ClientID %>").checked = false;
        document.getElementById("<%=radTrace.ClientID %>").checked = false;
        
        ShowInfo("");
        ShowMessage(error.get_message());
        ShowInfo(error.get_message());
        
    } catch(e) {
        alert(e.description);
        endWaitingCoverDiv();
    }
}


function ChkAllClick(headChk)
{
    try {
        gvTable=document.getElementById (gvClientID);
        for (var i=0;i<gvTable.rows.length-1;i++)
        {
           document.getElementById (gvClientID+"_"+i+"_RowChk").checked=headChk.checked;
        }         
    } catch(e) {
        alert(e.description);      
    }
}

function CheckClick(singleChk,id)
{
  
    try {
        gvTable=document.getElementById (gvClientID);
        if(singleChk.checked)
        {         
            for (var i=0;i<gvTable.rows.length-1;i++)
            {
                if (gvTable.rows[i+1].cells[1].innerText != " ") {
                    if(!document.getElementById (gvClientID+"_"+i+"_RowChk").checked)
                    {
                       return;
                    }
                }                
            } 
            document.getElementById (gvHeaderID+"_HeaderChk").checked=true;
        }
        else
        {
            document.getElementById (gvHeaderID+"_HeaderChk").checked=false;
        }         
    } catch(e) {
        alert(e.description);        
    }
}

function BtnDelClick()
{
    try {    
        var DelMOArray = new Array(); 
        gvTable=document.getElementById(gvClientID);
        var station = document.getElementById("<%=station.ClientID%>").value;
     
    
        for (var i=0;i<gvTable.rows.length-1;i++)
        {
       
            if( document.getElementById (gvClientID+"_"+i+"_RowChk").checked)
            {
                if(gvTable.rows[i+1].cells[1].innerHTML != "&nbsp;")
                {
                    DelMOArray.push(gvTable.rows[i+1].cells[1].innerText);
                }
            }
        }
        if (DelMOArray.length == 0) 
        {
            alert(mesNoSeRecord);
            return;
        }
        if(confirm(mesDelConfirm))
        {           
            beginWaitingCoverDiv();
            WebServiceGeneSMTMO.deleteMo(DelMOArray,"<%=userId%>", station,"<%=customer%>",onDelSucceed,onDelFail);
        }
     } catch(e) {
        alert(e.description);       
    }
}

function onDelSucceed(result)
{
    try {      
        endWaitingCoverDiv();        
        if(result==null)
        {
            //service方法没有返回
            ShowInfo("");
            var content = msgSystemError;
            ShowMessage(content);
            ShowInfo(content);
        }
        else if((result.length==1)&&(result[0]==SUCCESSRET))
        {
           //删除成功再查询一下
//            ShowInfo(msgSuccess);
            ShowSuccessfulInfo(true);
            document.getElementById("<%=btnQuery.ClientID%>").click();                  
        }
        else 
        {
           ShowInfo("");
            var content =result[0];
            ShowMessage(content);
            ShowInfo(content);
            document.getElementById("<%=btnQuery.ClientID%>").click();            
        }                        
    } catch(e) {
        alert(e.description);
        endWaitingCoverDiv();
    }        
}

function onDelFail(error)
{   
    try {
        endWaitingCoverDiv();
        ShowInfo("");
        ShowMessage(error.get_message());
         ShowInfo(error.get_message());        
    } catch(e) {
        alert(e.description);
        endWaitingCoverDiv();
    }
}

function inputQtyNumber(con)
{    
    try 
    {
	    if (getSelectionText() != "") 
		{
			document.selection.clear();
		}
	    var inputContent = con.value;
        var content = inputContent + String.fromCharCode(event.keyCode);
        var b =  + content + '';
	    
	    if (content == b)
	    {    		
		    if (b >=0 && b <=100000)
		    {
			    event.returnValue = true;
		    } else {
		        event.returnValue = false;
		    }
	    } else {
	        event.returnValue = false;
	    }	        	
    }
    catch (e)
    {
	    alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkQtyNumber
//| Author		:	Lucy Liu
//| Create Date	:	01/07/2010
//| Description	:	check Multi Qty的合法性
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkQtyNumber(value)
{
   
    var errorFlag = false;
    try 
    {
	      var b =  + value + '';
	    
	    if (value == b)
	    {
    		
		    if (b >=0 && b <=100000)
		    {
			    errorFlag = false;
		    } else {
		        errorFlag = true;
		    }
	    } else {
	        errorFlag = true;
	    }
	    return errorFlag;
    	
    }
    catch (e)
    {
	    alert(e.description);
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ExitPage
//| Author		:	Lucy Liu
//| Create Date	:	01/24/2010
//| Description	:	退出页面时调用
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ExitPage()
{
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ResetPage
//| Author		:	Lucy Liu
//| Create Date	:	01/24/2010
//| Description	:	刷新页面时调用
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ResetPage()
{
    ExitPage();
    document.getElementById("<%=radQuan.ClientID%>").checked = true;
    document.getElementById("<%=txtQty.ClientID%>").value = "";
    document.getElementById("MessageTextArea1").value = "";
    document.getElementById("<%=btnHidden.ClientID%>").click();      
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Submit
//| Author		:	jia-li
//| Create Date	:	03/07/2011
//| Description	:	弹出是否确认创建
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function Submit()
{
    alert("")
}
</script>
</asp:Content>


<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:ESOP and AOI KB Test

 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="ESOPandAoiKbTest.aspx.cs" Inherits="PAK_ESOPandAoiKbTest" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
<style type="text/css">
.maximizeStyle { height: auto; width: auto; position: fixed; border: 1px dotted black; left: 50px; top: 50px; }
	#Button1
	{
		width: 132px;
	}
</style>
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" src="../CommonControl/JS/AOI.js"></script>
<div>
	<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
		<Services>
			  <asp:ServiceReference Path="Service/WebServiceESOPandAoiKbTest.asmx" />
		</Services>
	</asp:ScriptManager>
	<center>
	<table border="0" width="98%">
	<tr><td width="85%" valign="top">
		<table border="0" width="100%">
			<tr>
				<td align="left">
					<asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
				</td>
				<td colspan="5">
					<iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true"   />                         
				</td>
			</tr>
			<tr>
				<td align="left">
					<asp:Label ID="lblLC" runat="server" CssClass="iMes_label_13pt"></asp:Label>
				</td>
				<td colspan="5">
					<iMES:CmbLightCode ID="cmbLC" runat="server" Width="100" IsPercentage="true"   />                         
				</td>
			</tr>
			
			<tr>
				<td align="left" style="width:15%">
					<asp:Label ID="lblProId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
				</td>
				<td align="left" style="width:20%">
					<asp:UpdatePanel runat="server" ID="upProId" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:Label ID="txtProId" runat="server" CssClass="iMes_label_13pt" />
						</ContentTemplate>
					</asp:UpdatePanel>
				</td>
				<td align="left" style="width:15%">
					<asp:Label ID="lblCPQS" runat="server" CssClass="iMes_label_13pt"></asp:Label>
				</td>
				<td align="left" style="width:20%">
					<asp:UpdatePanel runat="server" ID="upCPQS" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:Label ID="txtCPQS" runat="server" CssClass="iMes_label_13pt" />
						</ContentTemplate>
					</asp:UpdatePanel>
				</td>
				<td align="left" style="width:10%">
					<asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
				</td>
				<td align="left">
					<asp:UpdatePanel runat="server" ID="upModel" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:Label ID="txtModel" runat="server" CssClass="iMes_label_13pt" />
						</ContentTemplate>
					</asp:UpdatePanel>
				</td>
			</tr>
			<tr>
			  <td><asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt" Text="Queue SN List:"></asp:Label></td>
			  <td  colspan="5" align="left"><asp:Label ID="lblSnList" runat="server" CssClass="iMes_label_13pt" /></td>
			</tr>
			<tr>
			  <td><asp:Label ID="Label2" runat="server" CssClass="iMes_label_13pt" Text="AOI Test Result:"></asp:Label></td>
			  <td  colspan="5" align="left"><asp:Label ID="lblTestResult" runat="server" CssClass="iMes_label_13pt" /></td>
			</tr>
			 
			<tr>
				<td colspan="6">
					<iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
						GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
						HighLightRowPosition="3"  AutoHighlightScrollByValue="True">
					</iMES:GridViewExt>
				</td>
			</tr>
			
			<tr>
				<td align="left">
					<asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label><br/>
				</td>
				<td align="left" colspan="4">
					<iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
						CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
						ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
					<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
						<ContentTemplate>
							<button id="btnUpdateUI" runat="server" type="button" onclick="" style="display: none" />
							<button id="btnExit" runat="server" type="button" onclick="" style="display: none" />
							<button id="btnCheckPart" runat="server" type="button" onclick="" style="display: none" />
						</ContentTemplate>
					</asp:UpdatePanel>
					<asp:UpdatePanel ID="upHidden" runat="server" UpdateMode="Conditional" RenderMode="Inline">
						<ContentTemplate>
							<input type="hidden" runat="server" id="hidStation" />
							<input type="hidden" runat="server" id="hidProdId" />
							<input type="hidden" runat="server" id="hidData2Send" />
							
							  <input type="hidden" runat="server" id="hidAoiAddr" />
							 <input type="hidden" runat="server" id="hidKbPn" />
							 <input type="hidden" runat="server" id="hidLabelPn" />
							 <input type="hidden" runat="server" id="hidIsAOILine" />
							 <input type="hidden" runat="server" id="hidRowCnt" />
							 <input type="hidden" runat="server" id="hidWantData" />
							 <input type="hidden" runat="server" id="hidInputPart" />
							 <input type="hidden" runat="server" id="hidBomPartNoItems" />
							 <input type="hidden" runat="server" id="hidModel" />
						</ContentTemplate>
					</asp:UpdatePanel>
				</td>
				<td>
					<asp:UpdatePanel runat="server" ID="upChkQuery" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:CheckBox ID="chkQuery" runat="server" Font-Size="Large" Font-Bold="true" ForeColor="Red" />
						</ContentTemplate>
					</asp:UpdatePanel>
				</td>
			</tr>
			
		 
		</table>
	</td><td width="14%" valign="top">
		<asp:UpdatePanel runat="server" ID="JpgUp" UpdateMode="Conditional">
		<ContentTemplate>
		<table border="0">
			<tr><td width="50%"><asp:Image ID="ShowImage0" runat="server" Width="154" Height="100"/></td><td>
			<asp:Image ID="ShowImage1" runat="server" Width="154" Height="100"/></td></tr>
			<tr><td><asp:Image ID="ShowImage2" runat="server" Width="154" Height="100"/></td><td>
			<asp:Image ID="ShowImage3" runat="server" Width="154" Height="100"/></td></tr>
			<tr><td><asp:Image ID="ShowImage4" runat="server" Width="154" Height="100"/></td><td>
			<asp:Image ID="ShowImage5" runat="server" Width="154" Height="100"/></td></tr>
			<tr><td><asp:Image ID="ShowImage6" runat="server" Width="154" Height="100"/></td><td>
			<asp:Image ID="ShowImage7" runat="server" Width="154" Height="100"/></td></tr>
		</table>
		</ContentTemplate>
		</asp:UpdatePanel>
	</td></tr>
	</table>
	<img id="jpgCenter" class="maximizeStyle" style="display: none;">
	</center>

</div>

<script language="javascript" for="objMSComm" event="OnComm">    
///		ProcessMSComm()///skipComn
</script>

<script type="text/javascript">
	var defectCache;
	var defectCount = 0;
	var defectInTable = [];
	var defectCode="";
	var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
	var mesNoSelLC = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectLC").ToString()%>';
	var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
	 var msgVisualFail = '<%=this.GetLocalResourceObject(Pre + "_VisualFail").ToString()%>';
	var msgNeedAOI = '<%=this.GetLocalResourceObject(Pre + "_NeedAOI").ToString()%>';
	var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
	var objMSComm = document.getElementById("objMSComm");
	var aoiTimeout;
	var aoiAddr;
	var isTestAOI;
	var editor;
	var customer;
	var station;
	var line="";
	var custsn="";
	var arrAoiParm;
	var isAOILine="";
	var defectCache;
	
	var tbl;
	var DEFAULT_ROW_NUM = 6;
	var tblTable = [];
	
	document.body.onload = function() {
		try {
			hostname = getClientHostName();
			///PageMethods.getCommSetting(hostname, "<%=UserId%>", onGetCommSettingSuccess, onGetCommSettingFail);///skipComn
			getPdLineCmbObj().setAttribute("AutoPostBack", "True");
			PageMethods.GetAOIParr(onSuccGetAOIParr,onErrorGetAOIParr);
			getAvailableData("processDataEntry");
			aoiTimeout = 5;
			editor = "<%=UserId%>";
			customer = "<%=Customer%>";
			station = '<%=Request["Station"] %>';
		//    GetDefectList();
			
			if (Boolean( <%=isTestAOI%> ))
			{
				alert("AOI in TEST!");
				isTestAOI=true;
				CallAOI();
				return;
			}
			else
			{ isTestAOI=false;}
			
			tbl = "<%=gd.ClientID %>";
		} catch (e) {
			alert(e.description);
		}
	}
	function GetAOIAddrTimeout()
	{
	     for (var i=0; i<arrAoiParm.length; i++) 
	     {
	         if(arrAoiParm[i].Name==line.substr(0,1))
	         {
	            aoiAddr=arrAoiParm[i].Value;
	            if(isInt(arrAoiParm[i].Descr))
	            {
	              aoiTimeout=parseInt(arrAoiParm[i].Descr);
	            }
	            return;
	         }
    	   
	     }
	     aoiAddr="";
	}
	function isInt(value)
    {
        var er = /^[0-9]+$/;

        return ( er.test(value) ) ? true : false;
    }
	 function onSuccGetAOIParr(result)
	 {
	  arrAoiParm=result[0][0];
	  defectCache=result[1][0];
	 }
      function onErrorGetAOIParr(error)
      {
      alert(error.get_message());
      }
	function onGetCommSettingSuccess(result) {
		if (result[0] == SUCCESSRET) {
			m_port = result[1];
			m_baud = result[2];
			m_rth = result[3];
			m_sth = result[4];
			m_hs = result[5];
			//alert(m_port);
			//alert(m_baud);
			//alert(m_rth);
			//alert(m_sth);
			//alert(m_hs);
			if (objMSComm.CommPort != m_port) {
				if (!!objMSComm.PortOpen) {
					objMSComm.PortOpen = false;
				}
				objMSComm.CommPort = m_port;
			}

			objMSComm.Settings = m_baud;
			objMSComm.RThreshold = m_rth;
			objMSComm.SThreshold = m_sth;
			objMSComm.Handshaking = m_hs;

			try {
				if (!objMSComm.PortOpen)
					objMSComm.PortOpen = true;
			} catch (e) {
				alert(e.description);
			}
			//ShowInfo("CommPort,Settings,RThreshold,SThreshold,Handshaking,PortOpen  is: "+objMSComm.CommPort+" , "+objMSComm.Settings +" , "+objMSComm.RThreshold+" , "+objMSComm.SThreshold+" , "+objMSComm.Handshaking+" , "+objMSComm.PortOpen);     

		}
		else {
			ShowMessage(result.get_message());
			ShowInfo(result.get_message());
		}
	}

	function onGetCommSettingFail(result) {
		ShowMessage(result.get_message());
		ShowInfo(result.get_message());
	}

	function transLight() {
		return; ///skipComn
		var lightString = document.getElementById("<%=hidData2Send.ClientID%>").value;
		//ShowInfo(lightString);
		if (lightString=="") return;
		var lightArray = lightString.split(",");
		if (lightArray.length<24) return;
		var lightChar = "";
		for (var i = 0; i < 24; i++) {
			lightChar += String.fromCharCode(lightArray[i]);
		}
		//alert("lightChar is " + lightChar);
		//ShowInfo("lightChar is " + lightChar);
		objMSComm.Output = lightChar;
	}

	function ProcessMSComm() {
		if (objMSComm.CommEvent == 1)//如果是发送事件   
		{
			//ShowInfo(document.getElementById("<%=hidData2Send.ClientID%>").value);
		}

		return false;
	}
	function ResetValue()
	{
		document.getElementById("<%=txtProId.ClientID%>").innerText = "";
		document.getElementById("<%=txtCPQS.ClientID%>").innerText = "";
		document.getElementById("<%=txtModel.ClientID%>").innerText = "";
		document.getElementById("<%=hidData2Send.ClientID%>").value = "";
		defectCount = 0;
		defectInTable = [];
		// the line following disable last set highlight item in the table.
		eval("setRowNonSelected_" + tbl + "()"); 
		ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
		defectCode="";
		custsn="";
		
		var objItem = document.getElementById("<%=chkQuery.ClientID%>");
		objItem.disabled = false;
		if(objItem.parentElement.tagName == 'SPAN' && objItem.parentElement.disabled == true)
            objItem.parentElement.disabled = false;
	}

	function processDataEntry(inputData) {
	
		ShowInfo("");
		line = getPdLineCmbValue();
		if(inputData=="7777")
		{
		  document.getElementById("<%=btnExit.ClientID%>").click();
		  ResetValue();
		  CallNextInput();
		}
		
		if (line == "") {
			alert(mesNoSelPdLine);
			setPdLineCmbFocus();
			getAvailableData("processDataEntry");
			return;
		}
		
		//TEST
		//TEST AOI Connection
		if("IEC1234567"==inputData)
		{	
		    alert("TEST AOI Connection");
		    CallAOI_TEST();
		    CallNextInput();
		  	return;
			}
	
		//TEST AOI Connection
		
		if(custsn=="")
		{
		   ResetValue();
		   if (inputData.length > 10 && inputData.substr(0, 3) == "5CG")
             {
                 inputData = inputData.substr(0, 10)
             }
		   if (inputData.length == 11)  {
                inputData = inputData.substring(1, 11);
            }
		   
		   if (!isCustSN(inputData)&&inputData.length!=9) {
			ShowInfo('Wrong code!!');
		   }
		   else
		   {
			 document.getElementById("<%=hidProdId.ClientID%>").value = inputData;
			 beginWaitingCoverDiv();
			 if(!defectCache || defectCache.length==0)
			 { GetDefectList();}
			 
			 var ischkQuery = document.getElementById("<%=chkQuery.ClientID %>").checked;
			 WebServiceESOPandAoiKbTest.GetProductInfo(inputData, line, editor, station, customer, ischkQuery, onGetProductInfoSucc, OnError);
		   }
			CallNextInput();
			return;
		}
		
		if (check_if_can_jian_liao_finished() == false) {
			if (inputData.length == 4) {
				if(inputData=="9999"){
					ShowInfo("Please scan PartNo or defect code");
				}
				else if (SuccesslyInputDefect(inputData)){
					ShowInfo("Please scan PartNo");
				}
				else{
					ShowInfo("Please scan PartNo or correct defect code");
				}
			}
			else {
				var prodId = document.getElementById("<%=hidProdId.ClientID%>").value;
				WebServiceESOPandAoiKbTest.jianLiao(prodId, inputData, JianLiaoSucc, JianLiaoFail);
			}
		}
		else {
			if(inputData=="9999"){
				Save();
			}
			else if (SuccesslyInputDefect(inputData)){
				ShowInfo("Please scan 9999","green");
			}
			else{
				ShowInfo("Please scan correct defect code");
			}
		}
		CallNextInput();
		return;
		
		/*
		if (document.getElementById("<%=hidWantData.ClientID%>").value != "0"  && inputData.length!=4 )
		{
			beginWaitingCoverDiv();
			document.getElementById("<%=hidInputPart.ClientID%>").value = inputData;
			document.getElementById("<%=btnCheckPart.ClientID%>").click();
			CallNextInput();
			return;
		}
		if(inputData=="9999" && document.getElementById("<%=hidWantData.ClientID%>").value != "0")
		{
		   ShowInfo("Please scan PartNo or defect code");
			CallNextInput();
			return;
		}
		else
		{  
		 	if(inputData=="9999")
		 	{
		 	   Save();
		 	   CallNextInput();
		 	   return;
		 	}
		 	else
		 	{
		 	  if(isExistInCache(inputData))
		       {
		    	    defectCode=inputData;
		    	    //goDayTitleRow.style.display = (gbShowDays) ? '' : 'none' hidWantData
		    	    var qq=	 document.getElementById("<%=hidWantData.ClientID%>").value;
		    	    var msg;
		    	    if(qq== "0")
		    	    {msg="Please scan 9999";}
		    	    else
		    	    {msg="Please scan PartNo";}
		    	    
		    	//    var msg=(("<%=hidWantData.ClientID%>").value== "0" ||!qq)?"Please scan PartNo":"Please scan 9999";
		     	    ShowInfo(msg,"green");
		            CallNextInput();
		       }
		       else
		       {
		            ShowInfo("Please scan correct defect code");
		            CallNextInput();
		       }
		 	}
	   	}
		*/
	
	}
	
	function SuccesslyInputDefect(inputData){
		if(isExistInCache(inputData)){
			defectCode=inputData;
			return true;
		}
		return false;
	}

	function onGetProductInfoSucc(result)
	{
		endWaitingCoverDiv();
		if (result == null) {
			ShowMessage(msgSystemError);
			ShowInfo(msgSystemError);
			ResetValue();
		}
		else if (result.Success==SUCCESSRET)
		{
			setInputOrSpanValue(document.getElementById("<%=txtProId.ClientID %>"), result.PrdInfo.id);
			setInputOrSpanValue(document.getElementById("<%=txtModel.ClientID %>"), result.PrdInfo.modelId);
			setInputOrSpanValue(document.getElementById("<%=hidModel.ClientID %>"), result.PrdInfo.modelId);
			setInputOrSpanValue(document.getElementById("<%=txtCPQS.ClientID %>"), result.PrdInfo.customSN);
			custsn = result.PrdInfo.customSN;
			if (custsn==""||custsn==null)//for 173 Mideo  no custsn and input is prdid
			{
			custsn = result.PrdInfo.id;
			}
			setInputOrSpanValue(document.getElementById("<%=hidIsAOILine.ClientID %>"), result.IsNeedAOI);
			setInputOrSpanValue(document.getElementById("<%=hidAoiAddr.ClientID %>"), result.AoiAddr);
			setInputOrSpanValue(document.getElementById("<%=hidKbPn.ClientID %>"), result.KbPn);
			setInputOrSpanValue(document.getElementById("<%=hidLabelPn.ClientID %>"), result.LabelPn);
			setInputOrSpanValue(document.getElementById("<%=hidBomPartNoItems.ClientID %>"), result.BomPartNoItems);
			tblTable = result.Bom;
			setTable(result.Bom, -1);
			
			document.getElementById("<%=btnUpdateUI.ClientID%>").click();
		}
		else {
			ShowInfo("");
			var content = result;
			alert(content);
			ShowInfo(content);
			ResetValue();
		}
		CallNextInput();
	}
	
	function updateTable(result)
	{
	    var ret = -1;

        var found = 0;
        for (var i = 0; i < tblTable.length; i++)
        {
            var ok = 0;
            for (var j = 0; j < tblTable[i]["parts"].length; j++)
            {
                if (tblTable[i]["parts"][j]["id"] == result["PNOrItemName"]) 
                {
                    if (tblTable[i]["type"] == result["ValueType"]) 
                    {
                        ret = i;
                        ok = 1;
                        if (tblTable[i].scannedQty < tblTable[i].qty)
                        {
                            tblTable[i].scannedQty++;
                            tblTable[i].collectionData += result["CollectionData"] + " ";
                        }
                        else
                        {
                            tblTable[i].collectionData = result["CollectionData"];
                        }
                        //setSrollByIndex(i, true, "<%=gd.ClientID%>");
                        break;
                    }
                    else 
                    {
                        var xx = 0;
                    }
                }
            }
            
            if (ok == 1) {found = 1; break;}
        }
        //if (found == 1) { ret = true; }
        
        return ret;
    }
	
	function check_if_can_jian_liao_finished()
	{
		var ret = true;
		//tblTable[0]["scannedQty"] = 1;
		for (var i = 0; i < tblTable.length; i++)
		{
			if (tblTable[i]["qty"] != tblTable[i]["scannedQty"])
			{
				ret = false; break;
			}
		}
		return ret;		   
	}

	var __current_highlight = -1;
	function JianLiaoSucc(result) 
	{
		var findIndex = updateTable(result);
		if (findIndex == -1) { ShowInfo("error!"); return; }

		if (__current_highlight >= 0) setRowSelectedOrNotSelectedByIndex(__current_highlight, false, "<%=gd.ClientID %>");
		setTable(tblTable, findIndex);
		setRowSelectedOrNotSelectedByIndex(findIndex, true, "<%=gd.ClientID %>");__current_highlight = findIndex;
		//ShowInfo("pick a matierial:" + result.PNOrItemName + " success.");
		ShowInfo("");
		var bFinished = check_if_can_jian_liao_finished();
		if (bFinished == true) {
			ShowInfo("Please scan 9999 or defect code","green");
		}
		else{
			ShowInfo("Please scan PartNo or defect code");
		}
	}

	function JianLiaoFail(result) 
	{
		ShowMessage(result.get_message());
		ShowInfo(result.get_message());
	}
	
	function setTable(info, updateIndex) {
		var bomList = info;

		for (var i = 0; i < bomList.length; i++) 
		{
			if (updateIndex == -1) 
			{
			}
			else 
			{
				if (updateIndex != i) continue;
			}
			
			var rowArray = new Array();
			var rw;
			var collection = bomList[i]["collectionData"];
			var parts = bomList[i]["parts"];
			var tmpstr = "";

			//for (var j = 0; j < parts.length; j++) {
			//    tmpstr = tmpstr + " " + parts[j]["id"];
			//}
			if (bomList[i]["PartNoItem"] == null) 
			{
				tmpstr = " ";
			}
			else 
			{
				tmpstr = bomList[i]["PartNoItem"];
			}
			//tmpstr = add_changeline_symbol(tmpstr);
			rowArray.push(tmpstr); //part no/name

			if ((bomList[i]["tp"] == null) || (bomList[i]["tp"] == ""))
			{
				rowArray.push(" ");
			}
			else
			{
				rowArray.push(bomList[i]["tp"]); //"type"// must modified into "tp";
			}
			
			if (bomList[i]["description"] == null) 
			{
				rowArray.push(" ");
			}
			else 
			{
				rowArray.push(bomList[i]["description"]);
			}
			rowArray.push(bomList[i]["qty"]);
			rowArray.push(bomList[i]["scannedQty"]);
			coll = "";
			
			tmpstr = bomList[i]["collectionData"];
			rowArray.push(tmpstr); //["collectionData"]);

			//add data to table
			if (i < 12) 
			{
				eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
				if (updateIndex != -1) 
				{
					//setSrollByIndex(i, true, tbl);
				}
			}
			else {
				eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
				if (updateIndex != -1) 
				{
					setSrollByIndex(i, true, tbl);
				}
				rw.cells[1].style.whiteSpace = "nowrap";
			}
		}

		//if ((bomList.length > 0) && (updateIndex == -1))
		//{
		//    setSrollByIndex(0, true, tbl);
		//}
	}
	
	/*
	* Answer to: ITC-1360-1087
	* Description: Focus data entry.
	*/
	function CallNextInput() {
		getCommonInputObject().focus();
		getCommonInputObject().select();
		getAvailableData("processDataEntry");
	}

	window.onbeforeunload = function() {
		document.getElementById("<%=btnExit.ClientID%>").click();
	}

	window.onunload = function() {
		document.getElementById("<%=btnExit.ClientID%>").click();
	}
	
	var emptyJpgSrc = document.getElementById("<%=ShowImage0.ClientID%>").src;
	function maximizeImage(f) {
		var jpgCenter=document.getElementById("jpgCenter");
		if(emptyJpgSrc==f.src || ''==f.src)return;
		jpgCenter.src=f.src;
		jpgCenter.style.display = 'block';
	}
	function minimizeImage(f) {
		var jpgCenter=document.getElementById("jpgCenter");
		jpgCenter.style.display = 'none';
	}

	function CallAOI() {
	//function SendAOICmd(url, sn, kbPartNo, lbPartNo, timeOut, callback)
		//var url = document.getElementById("<%=hidAoiAddr.ClientID%>").value;
		var sn = custsn; ///document.getElementById("<%=hidProdId.ClientID%>").value;
		var kbPartNo = document.getElementById("<%=hidKbPn.ClientID%>").value;
		var lbPartNo = document.getElementById("<%=hidLabelPn.ClientID%>").value;
		if(isTestAOI)
		{ aoiAddr = 'http://127.0.0.1:80/sn/';}
		 try{ 
			 SendAOICmd(aoiAddr, sn, kbPartNo, lbPartNo, aoiTimeout, CallbackForAOI);
			 
			 }
			 
			 
		 catch (e) {
		   writeLog("Call AOI error:" + e.message+" ; sn="+sn +"; url="+aoiAddr);
		   document.getElementById("<%=lblTestResult.ClientID%>").innerText ="Call AOI error:" + e.message+" ; sn="+sn +"; url="+aoiAddr;
		   
		}
	}
	function CallbackForAOI(sn, result, errorCode, errorDesc) {
		writeLog("CallbackForAOI Code: "+ errorCode+" ; Error Desc :"+errorDesc);
		document.getElementById("<%=lblTestResult.ClientID%>").innerText ="SN: " + sn+"Error Code: "+ errorCode+" ; Error Desc :"+errorDesc;
    	PageMethods.AOICallBack(sn,editor,station,line,customer, result, errorCode, errorDesc,OnAoiSuccess, OnAoiError);
	}
	//
	function SetSnList()
	{
	  document.getElementById("<%=lblSnList.ClientID%>").innerText="";
    	 var c=queue.Count();
		 var snList="";
        if(c>0)
        {
             for(var i=0;i<c;i++)
             {
     
             snList=snList +","+queue.Item(i).Key;
             }
        }
      	document.getElementById("<%=lblSnList.ClientID%>").innerText =snList;
	}
	function OnAoiSuccess()
	{writeLog("Callba AOI Success!");
	SetSnList();
	}
	function OnAoiError(error)
	{writeLog("Callba AOI error :" + error.message);SetSnList();}   
	//*********** new function for CQ Mantis 0000107: 鍵盤檢測站與ESOP站併站需求
	function Save()
	{
	   var sn=custsn; ///document.getElementById("<%=txtCPQS.ClientID%>").innerText;
	   beginWaitingCoverDiv();
	 //  PageMethods.Save(sn,defectInTable,OnSaveSuccess, OnError);
	   PageMethods.Save(sn,defectCode,OnSaveSuccess, OnError);
	}

	function OnSaveSuccess(result) {
		 endWaitingCoverDiv();
		 var aoi="";
		
	//	 var addr=document.getElementById("<%=hidAoiAddr.ClientID%>").value;
		 if( defectCode!="")
		 {
		   ShowInfo("Success,"+msgVisualFail,"green");
		 
		 }
		 else
		 {
		   if("Y"==document.getElementById("<%=hidIsAOILine.ClientID%>").value)//
		   {
		      GetAOIAddrTimeout();
			  CallAOI();
			  ShowInfo("Success,"+msgNeedAOI,"green");
		   }
		   else
		   {
			  ShowInfo("Success","green");
		   }
		 
		 }

		 ResetValue();
		 CallNextInput();
	}
	function OnError(error) {
		endWaitingCoverDiv();
		ResetValue();
		ShowALL(error.get_message());
		CallNextInput();
	}

	// PageMethods.GetDefectList(OnGdSuccess, OnGdError);
	function GetDefectList(){
	PageMethods.GetDefectList(OnGdSuccess, OnGdError);
	}
	function OnGdSuccess(result) {
		defectCache=result[0];
	}
	function OnGdError(error) {
		endWaitingCoverDiv();
		ShowALL(error.get_message());
		CallNextInput();
	}
	
	
	function isExistInCache(data)
	{
		if (defectCache != undefined && defectCache != null)
		{
			for (var i = 0; i < defectCache.length; i++)
			{
				if (defectCache[i]["id"] == data)
				{
					return true;
				}
			}
		}
		
		return false;               
	}
	
	function getDesc(data)
	{
		if (defectCache != undefined && defectCache != null)
		{
			for (var i = 0; i < defectCache.length; i++)
			{
				if (defectCache[i]["id"] == data)
				{
					return defectCache[i]["description"];
				}
			}
		}
		
		return "";
	}
//*********** new function for CQ Mantis 0000107: 鍵盤檢測站與ESOP站併站需求

function ShowALL(msg) {
	ShowMessage(msg);
	ShowInfo(msg);
}
//TEST AOI Command
 function CallAOI_TEST() {
	//function SendAOICmd(url, sn, kbPartNo, lbPartNo, timeOut, callback)
	//	var url = document.getElementById("<%=hidAoiAddr.ClientID%>").value;
    	GetAOIAddrTimeout();
		var sn = "TESTCUSTSN"; ///document.getElementById("<%=hidProdId.ClientID%>").value;
		var kbPartNo = "KB_TESTPARTNO";
		var lbPartNo =  "LB_TESTPARTNO";
		if(isTestAOI)
		{ aoiAddr = 'http://127.0.0.1:80/test?sn=12345';}
		 try{ 
		 if(aoiAddr!="")
	    	 { SendAOICmd(aoiAddr, sn, kbPartNo, lbPartNo, aoiTimeout, CallbackForAOI_TEST);}
	    	 else
	    	 {alert("No AOI address!!");}
		 }
		 catch (e) {
		   alert("Error " +e.description +"\n"+"AOI addr:"+"\n"+aoiAddr);
	       writeLog("Error " +e.description+ "AOI addr:"+aoiAddr);
		}
	}
    function CallbackForAOI_TEST(sn, result, errorCode, errorDesc) {
		alert("CallbackForAOI_TEST Code: "+ errorCode +" ; Error Desc :"+errorDesc);
		writeLog("CallbackForAOI_TEST Code: "+ errorCode +" ; Error Desc :"+errorDesc);
	}
	
//TEST AOI Command

function ResetPage(pdline_select) 
{
    //ExitPage();
    //initPage(pdline_select);
    ShowInfo("");
}

</script>

</asp:Content>

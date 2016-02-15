
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="CheckAst.aspx.cs" Inherits="FA_CheckAst" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content2" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="Service/WebServiceASTCheck.asmx" />
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="100%" >
                <tr>
                    <td style="width:53%">
                        <table border="0" style="width: 100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lbPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                                     
                                </td>
                            </tr>
                            
                            <tr>
                                <td style="width: 15%" align="left">
                                    <asp:Label ID="lbPassCnt" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td style="width: 25%" align="left">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="txtPassCnt" runat="server" CssClass="iMes_label_13pt" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                
                                <td style="width: 15%" align="left">
                                    <asp:Label ID="lblEpiaCnt" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td style="width: 25%" align="left">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="txtEpiaCnt" runat="server" CssClass="iMes_label_13pt" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="width: 10%" align="left">
                                    <!--<asp:Label ID="lblPiaCnt" runat="server" CssClass="iMes_label_13pt"></asp:Label>-->
                                </td>
                                <td align="left">
                                    <!--<asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="txtPiaCnt" runat="server" CssClass="iMes_label_13pt" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>-->
                                </td>
                            </tr>             
                            
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblProId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="txtProId" runat="server" CssClass="iMes_label_13pt" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_13pt" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="6" align="left">
                                    <asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
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
                                    <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                                </td>
                                <td align="left" colspan="5">
                                    <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                                        CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*\,]*$"
                                        ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                                    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                                        <ContentTemplate>
                                            <button id="btnUpdateUI" runat="server" type="button" onclick="" style="display: none" />
                                            <button id="btnExit" runat="server" type="button" onclick="" style="display: none" />
                                            <button id="btnComplete" runat="server" type="button" onclick="" style="display: none" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="upHidden" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                        <ContentTemplate>
                                            <input type="hidden" runat="server" id="hidStation" />
                                            <input type="hidden" runat="server" id="hidInput" />
                                            <input type="hidden" runat="server" id="hidProdId" />
                                            <input type="hidden" runat="server" id="hidWantData" />
                                            <input type="hidden" runat="server" id="hidRowCnt" />
											<input type="hidden" runat="server" id="hidModel" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                
                            </tr>
                                    <tr>
                                      <td>
                                      <asp:UpdatePanel runat="server" ID="upChkQuery" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:CheckBox ID="chkQuery" runat="server" Font-Size="Large" Font-Bold="true" 
                                ForeColor="Red" Text="Query" />
						</ContentTemplate>
					</asp:UpdatePanel>
                                      </td>
                                    </tr>
                        </table>
                    </td>
                   
                    
                    <td style="width:53%">
                        <asp:UpdatePanel ID="upESOP" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <table border="0" width="100%" style="margin-left: 14px; height: 511px;">
                                    <tr>
                                        <td style="height:105; vertical-align:middle" align="center">
                                            <img id ="Img1" runat="server" 
                                                style=" height:450px; width:95%; margin-top: 0px;" src="" 
                                                onmouseout="hiddenPic('Layer1')" alt="(No ESOP)" 
                                                onmousemove="showPic(this,'Layer1')" /> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height:105; vertical-align:middle" align="center">
                                            <img id ="Img2" runat="server" 
                                                style=" height:42px; width:100%; visibility: hidden;" src="" 
                                                onmouseout="hiddenPic('Layer1')" alt="(No ESOP)" 
                                                onmousemove="showPic(this,'Layer1')"   /> 
                                        </td>
                                    </tr>
                                   
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    
                </tr>
                
            </table>        
        </center>
    </div>

    <div id="Layer1" style="display:none;position:absolute;z-index:1"></div> 

    <script type="text/javascript">        
        
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var globalPrdID = "";
        var bInputCSN = false;
        
		var editor;
		var customer;
		var station;
		
		var tbl;
		var DEFAULT_ROW_NUM = 6;
		var tblTable = [];
		var image1attype;
		var image2attype;
		document.body.onload = function() {
            try {
                getPdLineCmbObj().setAttribute("AutoPostBack", "True");
                document.getElementById("<%=hidWantData.ClientID%>").value = "0";   //ProdId wanted
                getAvailableData("processDataEntry");
				
				editor = "<%=UserId%>";
				customer = "<%=Customer%>";
				station = '<%=Request["Station"] %>';
				image1attype = '<%=Request["Image1ATType"] %>';
				image2attype = '<%=Request["Image2ATType"] %>';
				station = '<%=Request["Station"] %>';
				tbl = "<%=gd.ClientID %>";
            } catch (e) {
                alert(e.description);
            }
        }

        function showPic(obj, Layer1) {
           // if (obj.alt == "(No ESOP)") return;
            var x, y;
            x = event.clientX;
            y = event.clientY;
            document.getElementById(Layer1).style.right = 200;
            document.getElementById(Layer1).style.top = 25;
            document.getElementById(Layer1).innerHTML = "<img src=" + obj.src + ">";
            document.getElementById(Layer1).style.display = "block";
        }
        
        function hiddenPic(Layer1) {
            document.getElementById(Layer1).innerHTML = "";
            document.getElementById(Layer1).style.display = "none";
        } 


        function processDataEntry(inputData) {
            ShowInfo("");
            if (inputData == "7777") {
                document.getElementById("<%=btnExit.ClientID%>").click();
                ResetValue();
                getAvailableData("processDataEntry");
                return;
            }
            line = getPdLineCmbValue();
            if (line == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoSelectPdLine").ToString()%>');
                setPdLineCmbFocus();
                getAvailableData("processDataEntry");
                return;
            }
            try {
                if ('' == globalPrdID) {
                    inputData = Get2DCodeCustSN(inputData); //2维码SN
                    if (!isProdIDorCustSN(inputData, line)) {
                        alert('<%=this.GetLocalResourceObject(Pre + "_msgBadProductID").ToString()%>');
                        callNextInput();
                        return;
                    }
					//globalPrdID = inputData;
					beginWaitingCoverDiv();
					var ischkQuery = document.getElementById("<%=chkQuery.ClientID %>").checked;
					WebServiceASTCheck.InputProductIDorCustSN(inputData, line, editor, station, customer, ischkQuery, image1attype, image2attype, onInputProductIDorCustSNSuccess, onFailAndRestart);
					return;
				}
				if (check_if_can_jian_liao_finished() == false) {
				    if (inputData == "9999") {
				        ShowInfo("Please scan PartNo");
				    }
				    else {
				        WebServiceASTCheck.jianLiao(globalPrdID, inputData, JianLiaoSucc, onFail);
				    }
				}
				else {
				    if (inputData == "9999") {
				       
				            Save();
				       
				    }
				    else {
				        ShowInfo("Please scan 9999 to save");
				    }
				}
            } catch (e) {
                alert(e.description);
            }
			getAvailableData("processDataEntry");




        }
        function Save() {
         var ischkQuery = document.getElementById("<%=chkQuery.ClientID %>").checked;
         if (!ischkQuery) {
             globalPrdID = '';
             beginWaitingCoverDiv();
             document.getElementById("<%=btnComplete.ClientID%>").click();
             ShowInfo("Success", "green");
             ResetValue();
         }
         else {
             ShowInfo("QueryBox had Checked,Query only");
         }
        }
        function onFail(result) {
            
			endWaitingCoverDiv();
			ShowMessage(result.get_message());
            ShowInfo(result.get_message());
			getAvailableData("processDataEntry");
        }
		
		function onFailAndRestart(result) {
            globalPrdID = '';
			endWaitingCoverDiv();
			ShowMessage(result.get_message());
            ShowInfo(result.get_message());
			getAvailableData("processDataEntry");
        }
		
		function onInputProductIDorCustSNSuccess(result) {
			endWaitingCoverDiv();
			if (result == null) {
				ShowMessage(msgSystemError);
				ShowInfo(msgSystemError);
				ResetValue();
			}
			else if (result.Success==SUCCESSRET)
			{
				setInputOrSpanValue(document.getElementById("<%=txtProId.ClientID %>"), result.Id);
				setInputOrSpanValue(document.getElementById("<%=hidProdId.ClientID %>"), result.Id);
				setInputOrSpanValue(document.getElementById("<%=txtModel.ClientID %>"), result.Model);
				setInputOrSpanValue(document.getElementById("<%=hidModel.ClientID %>"), result.Model);
				globalPrdID = result.Id;
				if (result.Bom != null) {
				    tblTable = result.Bom;
				    setTable(result.Bom, -1);
				}
				var imageUrl1 = "";
				var imageUrl2 = "";
				var RDSServer = '<%=ConfigurationManager.AppSettings["RDS_Server"].Replace("\\", "\\\\")%>';

				imageUrl1 = RDSServer + result.image1src + ".JPG";
				imageUrl2 = RDSServer + result.image2src + ".JPG";

				document.getElementById("<%=Img1.ClientID %>").src = imageUrl1;
				document.getElementById("<%=Img2.ClientID %>").src = imageUrl2;
				//document.getElementById("<%=btnUpdateUI.ClientID%>").click();
				ShowInfo("Please Scan PartNo or 9999 Save or 7777 Cancel!");
				
			}
			else {
				ShowInfo("");
				var content = result;
				alert(content);
				ShowInfo(content);
				ResetValue();
			}
			callNextInput();
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
			//globalPrdID = '';
			//beginWaitingCoverDiv();
			//document.getElementById("<%=btnComplete.ClientID%>").click();
			ShowInfo("Please scan 9999 to save");
		}
		else{
			ShowInfo("Please scan PartNo");
		}
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
        * Answer to: ITC-1360-1080
        * Description: Focus data entry.
        */
        function callNextInput() {
            getCommonInputObject().focus();
            getCommonInputObject().select();
            getAvailableData("processDataEntry");
        }

        function showQCMethod(method) {
            ShowSuccessfulInfo(true, '[' + globalPrdID + ']\n' + method);
        }

        window.onbeforeunload = function() {
            document.getElementById("<%=btnExit.ClientID%>").click();
        }

        window.onunload = function() {
            document.getElementById("<%=btnExit.ClientID%>").click();
        }
        function ResetValue() {
            document.getElementById("<%=txtProId.ClientID%>").innerText = "";
            document.getElementById("<%=hidProdId.ClientID%>").innerText = "";
            document.getElementById("<%=txtModel.ClientID%>").innerText = "";
            document.getElementById("<%=Img1.ClientID %>").src = "";
            document.getElementById("<%=Img2.ClientID %>").src = "";
            defectCount = 0;
            defectInTable = [];
            tbl = "<%=gd.ClientID %>";
            // the line following disable last set highlight item in the table.
            eval("setRowNonSelected_" + tbl + "()");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            globalPrdID = "";
            custsn = "";

            var objItem = document.getElementById("<%=chkQuery.ClientID%>");
            objItem.disabled = false;
            if (objItem.parentElement.tagName == 'SPAN' && objItem.parentElement.disabled == true)
                objItem.parentElement.disabled = false;
            getAvailableData("processDataEntry");
        }
    </script>

</asp:Content>



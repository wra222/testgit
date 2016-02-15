<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for LabelLightGuide Page
 *             
 * UI:CI-MES12-SPEC-FA-UI Label Light Guide.docx –2011/10/26 
 * UC:CI-MES12-SPEC-FA-UC Label Light Guide.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-19  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="LabelLightGuide.aspx.cs" Inherits="FA_LabelLightGuide" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
<style type="text/css">
.maximizeStyle { height: auto; width: auto; position: fixed; border: 1px dotted black; left: 50px; top: 50px; }
</style>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
                <%--<asp:ServiceReference Path="Service/WebServiceLabelLightGuide.asmx" />--%>
            </Services>
        </asp:ScriptManager>
        <center>
        <table border="0" width="98%">
		<tr><td width="85%">
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
                    <td colspan="6" align="left">
                        <asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="6">
                        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="240px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                    Width="99.9%" Height="230px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left">
                                    <Columns>
                                        <asp:BoundField DataField="PartNo"  />
                                        <asp:BoundField DataField="Type"  />
                                        <asp:BoundField DataField="LightNo" />
                                        <asp:BoundField DataField="Qty" />
                                    </Columns>
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                
                <tr>
                    <td align="left">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td align="left" colspan="5">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnGridFresh" runat="server" type="button" onclick="" style="display: none" />
                                <button id="btnExit" runat="server" type="button" onclick="" style="display: none" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="upHidden" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <input type="hidden" runat="server" id="hidStation" />
                                <input type="hidden" runat="server" id="hidProdId" />
                                <input type="hidden" runat="server" id="hidData2Send" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="4">
                        <asp:UpdatePanel runat="server" ID="upChkQuery" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkQuery" runat="server" Font-Size="Large" Font-Bold="true" ForeColor="Red" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
					<td align="right">
						<input id="btnLightAll" type="button"  onclick="LightAll()" class="iMes_button" runat="server" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" value="燈號全亮" />&nbsp;&nbsp;
						<input id="btnLightNone" type="button"  onclick="LightNone()" class="iMes_button" runat="server" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" value="燈號全關" />
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
            ProcessMSComm()
    </script>

    <script type="text/javascript">

        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectPdLine").ToString()%>';
        var mesNoSelLC = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelectLC").ToString()%>';
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var objMSComm = document.getElementById("objMSComm");

        document.body.onload = function() {
            try {
                hostname = getClientHostName();
                PageMethods.getCommSetting(hostname, "<%=UserId%>", onGetCommSettingSuccess, onGetCommSettingFail);
                getPdLineCmbObj().setAttribute("AutoPostBack", "True");
                getAvailableData("processDataEntry");
            } catch (e) {
                alert(e.description);
            }
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
            var lightString = document.getElementById("<%=hidData2Send.ClientID%>").value;
            //ShowInfo(lightString);
            var lightArray = lightString.split(",");
            var lightChar = "";
            for (var i = 0; i < 24; i++) {
                lightChar += String.fromCharCode(lightArray[i]);
            }
            //alert("lightChar is " + lightChar);
            //ShowInfo("lightChar is " + lightChar);
            objMSComm.Output = lightChar;
        }
		
		function LightAll() {
			var x,y,n,aryLight=[],toStop=false;
			for (x = 0; x < 24; x++) {
				aryLight.push(0);
			}
			for (x = 0; x < 24; x++) {
				n = 0;
				for (y = 0; y < 8; y++) {
					if (y < 7)
						n += Math.pow(2,y);
					else
						n = Math.pow(2,y);
					aryLight[x] = n;
					
					//alert( aryLight[0].toString()+' , '+aryLight[1].toString());
					var lightChar = "";
					for (var i = 0; i < 24; i++) {
						lightChar += String.fromCharCode(aryLight[i].toString());
					}
					objMSComm.Output = lightChar;
					//alert("please chech LED No." + (x*8 + y + 1));
					if (!confirm("please chech LED No." + (x*8 + y + 1) + "\n(Press OK to continue testing)"
					+ "\n(Press CANCEL to stop testing the remaining Lighting)")) {
						toStop = true;
						break;
					}
				}
				if (toStop){
					break;
				}
			}
			aryLight.splice(0, 24);
        }
		
		function LightNone() {
			var lightString = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
            var lightArray = lightString.split(",");
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

        function processDataEntry(inputData) {
            document.getElementById("<%=txtProId.ClientID%>").innerText = "";
            document.getElementById("<%=txtCPQS.ClientID%>").innerText = "";
            document.getElementById("<%=txtModel.ClientID%>").innerText = "";
            document.getElementById("<%=hidData2Send.ClientID%>").value = "";
            ShowInfo("");
            line = getPdLineCmbValue();
            if (line == "") {
                alert(mesNoSelPdLine);
                setPdLineCmbFocus();
                getAvailableData("processDataEntry");
                return;
            }
            if (getLightCodeCmbValue() == "") {
                alert(mesNoSelLC);
                setLightCodeCmbFocus();
                getAvailableData("processDataEntry");
                return;
            }
            if (inputData.length > 10 && inputData.substr(0, 3) == "5CG")
             {
                 inputData = inputData.substr(0, 10)
             }
            if (!isCustSN(inputData)) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgBadProductID").ToString()%>');
                callNextInput();
                return;
            }
            document.getElementById("<%=hidProdId.ClientID%>").value = inputData;
            beginWaitingCoverDiv();
            document.getElementById("<%=btnGridFresh.ClientID%>").click();
        }

        /*
        * Answer to: ITC-1360-1087
        * Description: Focus data entry.
        */
        function callNextInput() {
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
    </script>

</asp:Content>

<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:CombineCOAandDNReprint page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-2-7     207003               Create          
 * Known issues:
 * TODO:
 */ --%>
 
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="COAReturn.aspx.cs" Inherits="PAK_COAReturn" Title="无标题页" %>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
        </Services>
    </asp:ScriptManager>
    <script language="javascript" type="text/javascript">
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgSaveWrong = '<%=this.GetLocalResourceObject(Pre + "_msgSaveWrong").ToString() %>';
        var msgInvalidFileType = '<%=msgInvalidFileType %>';
        var msgEmptyFile = '<%=msgEmptyFile %>';
        window.onload = function() {
            
        }

        function OnKeyPress(obj) {
            var key = event.keyCode;
            if (key == 13)//enter
            {
                event.cancelBubble = true;
                return false;
            }
        }

        function Save() {

            
            var strFileName = document.getElementById("<%=hidFileName.ClientID %>").value;
            if (strFileName != "") {
                var dgData = document.getElementById("<%=gridviewE.ClientID%>");
                if (dgData.rows[1].cells[0].innerText.trim() == "") {
                    return true;
                }
                else {
                    ShowMessage(msgSaveWrong);
                    ShowInfo(msgSaveWrong);
                    document.getElementById("<%=hidFileName.ClientID %>").value = "";
                    return false;
                }
            }
            else {
                ShowMessage(msgEmptyFile);
                ShowInfo(msgEmptyFile);
                document.getElementById("<%=btnTableClear.ClientID%>").click();
                return false;
            }
        }

        function checkFileSize() {
            var filePath = document.getElementById("<%=txtBrowse.ClientID %>").value;
            if (filePath == "") {
                DisplsyMsg(msgEmptyFile);
                return false;
            }
            var extend = filePath.substring(filePath.lastIndexOf(".") + 1);
            if (extend == "") {
                DisplsyMsg(msgInvalidFileType);
                return false;
            }
            else {
                if (!((extend == "xls" || extend == "xlsx"))) {
                    DisplsyMsg(msgInvalidFileType);
                    return false;
                }
            }
            return true;
        }

        function DisplsyMsg(src) {
            ShowMessage(src);
            ShowInfo(src);
        }
        function SelectFile() {
            ShowInfo("");
            document.getElementById("<%=btnTableClear.ClientID%>").click();
            if (checkFileSize()) {
                //document.getElementById("<%=btnUpload.ClientID%>").click();
            }
            else {
                document.getElementById("<%=hidFileName.ClientID %>").value = "";
                document.getElementById("<%=txtBrowse.ClientID %>").outerHTML = document.getElementById("<%=txtBrowse.ClientID %>").outerHTML;
            }
        }
        function initPage() {
            if (document.getElementById("<%=hidFileName.ClientID %>").value != "") {
                document.getElementById("<%=btnCancel.ClientID%>").click();
                document.getElementById("<%=hidFileName.ClientID %>").value = "";
            }
            document.getElementById("<%=txtBrowse.ClientID %>").outerHTML = document.getElementById("<%=txtBrowse.ClientID %>").outerHTML;
        }
        function GetProduct() {
            if (document.getElementById("<%=hidFileName.ClientID %>").value != "") {
                return true;
            }
            else {
                return false;
            }
        }
        function clientUpload() {
            ShowInfo("");
            if (checkFileSize()) {
                return true;
            }
            else {
                document.getElementById("<%=hidFileName.ClientID %>").value = "";
                document.getElementById("<%=txtBrowse.ClientID %>").outerHTML = document.getElementById("<%=txtBrowse.ClientID %>").outerHTML;
                document.getElementById("<%=btnTableClear.ClientID%>").click();
                return false;
            }
        }
        function Success() {
            var txt = "Save:" + document.getElementById("<%=hidName.ClientID %>").value + "  Success!";
            ShowInfo(txt, "green");
            var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["PassAudioFile"] %>';
            var obj = document.getElementById("bsoundInModal");
            obj.src = sUrl;
            document.getElementById("<%=hidFileName.ClientID %>").value = "";
        }
    </script>  
        <div id="div1" style="width: 95%; border: solid 0px red; margin: 0 auto;">
                <table width="100%" border="0"> 
                    <tr>
                            <td style="width:10%">
                            <asp:Label ID="lbCOA" runat="server" CssClass="iMes_DataEntryLabel" />
                             </td>
                              <td style="width:80%">
                            <asp:FileUpload ID="txtBrowse" ame="txtBrowse" Style="width: 95%"
                                runat="server" ContentEditable="false" onkeypress='return OnKeyPress(this)' onchange="SelectFile()" />
                                </td>
                                <td style="width:10%" align="left">
                            <input id="btnUpload" type="button" onclick="if(clientUpload())" 
                                        onserverclick="uploadClick" class="iMes_button" runat="server" 
                                        onmouseover="this.className='iMes_button_onmouseover'" 
                                        onmouseout="this.className='iMes_button_onmouseout'" value="Upload"/>
                             </td>
                             </tr>
                            
                 </table>  
                 <table width="100%" border="0"> 
                  <tr>
                             <td>
                            <hr/>
                            </td>
                             </tr>
                              </table>  
                <table width="100%" border="0">
                 <tr>
                 <td style="width:40%" >
                        <asp:Label ID="lbVList" runat="server" CssClass="iMes_DataEntryLabel" />
                      </td>
                      <td style="width:10%">
                      </td >
                      <td style="width:40%" >
                        <asp:Label ID="lbEList" runat="server" CssClass="iMes_DataEntryLabel" />
                      </td>
                   </tr>
                </table>  
            <table width="100%" border="0">     
                   <tr>
                   <td>
                   <br/>
                   </td>
                   <td>
                   <br/>
                   </td>
                   <td>
                   <br/>
                   </td>
                   </tr>
                  <tr>
                        <td style="width:40%" colspan="4" align="left">
                                <asp:UpdatePanel ID="updatePanelV" runat="server" UpdateMode="Conditional" >
                                <ContentTemplate>
                                 <iMES:GridViewExt ID="gridviewV" runat="server" 
                                AutoGenerateColumns="false" 
                                AutoHighlightScrollByValue="true"
                                GvExtWidth="100%" 
                                GvExtHeight="300px" 
                                OnGvExtRowClick="" 
                                OnGvExtRowDblClick=""
                                Width="100%" 
                                Height="295px" 
                                SetTemplateValueEnable="true" 
                                GetTemplateValueEnable="true"
                                HighLightRowPosition="1" 
                                HorizontalAlign="Left"  >                                     
                                        <Columns>
                                            <asp:BoundField DataField="SN"  />
                                            <asp:BoundField DataField="COA"  />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>   
                                </asp:UpdatePanel>
                        </td>
                        <td style="width:10%" >
                        </td>
                        <td style="width:40%" colspan="4" align="left">
                                <asp:UpdatePanel ID="updatePanelE" runat="server" UpdateMode="Conditional" >
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="gridviewE" runat="server" 
                                AutoGenerateColumns="false" 
                                AutoHighlightScrollByValue="true"
                                GvExtWidth="100%" 
                                GvExtHeight="300px" 
                                OnGvExtRowClick="" 
                                OnGvExtRowDblClick=""
                                Width="100%" 
                                Height="295px" 
                                SetTemplateValueEnable="true" 
                                GetTemplateValueEnable="true"
                                HighLightRowPosition="1" 
                                HorizontalAlign="Left"  >                                           
                                        <Columns>
                                            <asp:BoundField DataField="SN"  />
                                            <asp:BoundField DataField="ERROR"  />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>   
                                </asp:UpdatePanel>
                        </td>
                </tr>
                </table>  
                <table width="100%" border="0">
                <tr>
                    <td align="right" height="48" >
                        <input type="button" id="btnSave" runat="server" class="iMes_button"
                            onclick="if(Save())"  onserverclick="btnSave_ServerClick"/>
                    </td>
                </tr>
                <tr>
                    <td>
                         <asp:UpdatePanel ID="updatePanelALL" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <input type="hidden" id="hidFileName" runat="server" />
                                <input type="hidden" id="hidName" runat="server" />
                                <button id="btnGetProductTable" runat="server" type="button" onclick="if(GetProduct())" onserverclick="btnGetProductTable_ServerClick" style="display: none" />
	                            <button id="btnCancel" runat="server" type="button" style="display: none" />
	                            <button id="btnTableClear" runat="server" type="button" style="display: none" onserverclick="btnTableClear_ServerClick"/>
	                        </ContentTemplate>   
                        </asp:UpdatePanel> 
                    </td>
                </tr>
              </table>                
        </div>    
      
</asp:Content>


<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for ReleaseHold Page
 *             
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * Known issues:
 * TODO:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="ReleaseHold.aspx.cs" Inherits="PAK_ReleaseHold" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../CommonControl/calendar/calendar.js"></script>
    <script type="text/VBscript" src="../CommonControl/calendar/calendar.vbs"></script>
    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                <td style="width:10%">
                Model:
                </td>
                   
                              <td align="left" style="width:30%">
                            <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
                            InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"
                            TabIndex="1"/>
                              </td>
                                  <td style="width:10%" align="right">
                Defect Code:
                </td>
                              <td style="width:30%">
                                 <asp:DropDownList ID="droDefect" runat="server" Height="25px" Width="98%">
                                 </asp:DropDownList>
                              </td>
                              <td style="width:20%">
                              
                               <input id="btnClSave" type="button" value="Save" onclick="Save()" style="width: 150px; display:none" />
                              </td>
                            </tr>
                            
                         
                <tr>
                    <td colspan="5">
                        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="240px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                    Width="99.9%" Height="230px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left" onrowdatabound="GridView1_RowDataBound">
                                   
                                    <Columns>
                                        <asp:TemplateField>
                                        <HeaderTemplate>
            <INPUT TYPE="checkbox"  id="nchkALL" onclick="CheckALL(this)" style=" display:none " >
        </HeaderTemplate>
                                        
                                            <ItemTemplate>
                                                <asp:CheckBox id="chk" runat="server" Checked="false" CssClass="xxxx" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Custsn" />
                                        <asp:BoundField DataField="ProductID" />
                                        <asp:BoundField DataField="PreLine" />
                                        <asp:BoundField DataField="PreStation" />
                                        <asp:BoundField DataField="HoldTime" />
                                        <asp:BoundField DataField="HoldUser" />
                                        <asp:BoundField DataField="HoldCode" />
                                        <asp:BoundField DataField="HoldCodeDescr" />
                                        <asp:BoundField DataField="HoldID" />
                                     
                                    </Columns>
                                </iMES:GridViewExt>
                                     <input id="hidModel" runat="server" type="hidden" />
                                     <input id="hidSelectID" runat="server" type="hidden" />
                                        <input id="hidDefect" runat="server" type="hidden" />
                                     
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnQueryData" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="ServerClick" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                
            </table>
         
                                        <button id="btnQueryData" runat="server" type="button" style="display: none" />
                                        <button id="btnSaveData" runat="server" type="button" style="display: none" />
        </center>
    </div>

    <script type="text/javascript">

        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
     
        var idArr;
        Array.prototype.remove = function(obj) {
            return RemoveArray(this, obj);
        };
        function RemoveArray(array, attachId) {
            for (var i = 0, n = 0; i < array.length; i++) {
                if (array[i] != attachId) {
                    array[n++] = array[i]
                }
            }
            array.length -= 1;
        }
        window.onload = function() {
            inputSNControl = getCommonInputObject();
            inputSNControl.focus();
            getAvailableData("processDataEntry");
            idArr= new Array();
        }
        function setSelectVal(spanckb, id) {
           oState = spanckb.checked;
            if (oState) {
                idArr.push(id);
            }
            else {
                RemoveArray(idArr, id);
            }
        }
        function CheckALL(obj) {

          idArr = [];
          var gvID="<%=GridViewExt1.ClientID%>";
          var isChecked = obj.checked;
          var tbl = $('table[id ='+gvID+']');
          tbl.each(function() {
              $(this).find('tr').each(function() {
                  $(this).find('td:first').each(function() {
                    $(this).find('input[type="checkbox"]').attr('checked', isChecked);
                      if (isChecked && $(this).attr('prdID')) {
                          idArr.push($(this).attr('prdID'));
                      }
                  })
              })
          })
          if (!isChecked) {
              idArr = [];
          }

      }
      function GetSelectDefect() {
          var id = "#" + "<%=droDefect.ClientID %>";
          var s = $(id).val();
          document.getElementById("<%=hidDefect.ClientID %>").value = s;
          return s;
      }
        function CallNextInput() {
            getAvailableData("processDataEntry");
            inputSNControl.value = "";
            inputSNControl.focus();
        }
        function processDataEntry(data) {
          
            if (data == "") {
                alert("Please input model!");
                getAvailableData("processDataEntry");
                inputSNControl.focus();
                return;
            }
            else {
                document.getElementById("<%=hidModel.ClientID%>").value = data;
                beginWaitingCoverDiv();
                idArr = [];
                document.getElementById("<%=btnQueryData.ClientID%>").click();

            }
            CallNextInput();
        }
       
        function Save() {
              document.getElementById("<%=hidSelectID.ClientID%>").value = idArr.join(',');
            if (document.getElementById("<%=hidSelectID.ClientID%>").value == "") {
                alert("Please select ProductID!")
                CallNextInput();
                return;
            }
            if (GetSelectDefect() == "") {
                alert("Please select defect!");
                CallNextInput();
                return;
            }
             beginWaitingCoverDiv();
             document.getElementById('<%=btnSaveData.ClientID%>').click();
             idArr = [];
             CallNextInput();
         }
        function SaveSuccess() {
            endWaitingCoverDiv();
            ShowInfo("Success", "green");
        }
      
        function ShowBtn(b) {
            if (b) {
                $('#btnClSave').show();
                $('#nchkALL').show();
                //    <button id="btnClSave" type="button" style="display: none" onclick="Save()" /> nchkALL
            }
            else {
                $('#btnClSave').hide();
                $('#nchkALL').hide();
            }
        
        }
        
    </script>

</asp:Content>

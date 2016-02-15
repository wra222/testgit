<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SAInputXRay.aspx.cs" Inherits="SA_SAInputXRay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
  <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/SA/Service/SAInputXRayWebService.asmx" />
        </Services>
    </asp:ScriptManager>
<div style="width: 96%; margin: 0 auto;">
 <table width="100%">
  <tr>
      <td style="width: 23%">
                    <asp:Label ID="PdLine" runat="server">PdLine</asp:Label>
                </td>
     <td style="width: 30%">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100"   IsPercentage="true" Stage="SA" />        
       <td style="width: 20%">
                    <asp:Label ID="Model" runat="server">Model</asp:Label>
                </td>
       <td style="width: 30%">
                     <iMES:CmbConstValueType ID="cmbConstValueType1" runat="server"  Width="100" IsPercentage="true" IsSelectFirstNotNull="true" />
                </td>   
  </tr>
  <tr>
      <td style="width: 23%">
                    <asp:Label ID="Location" runat="server">Location</asp:Label>
                </td>
       <td style="width: 30%">
                     <iMES:CmbConstValueType ID="cmbConstValueType2" runat="server"  Width="100" IsPercentage="true"  />
                </td>          
       <td style="width: 20%">
                    <asp:Label ID="Obligation" runat="server">Obligation</asp:Label>
                </td>
       <td style="width: 30%">
                     <iMES:CmbConstValueType ID="cmbConstValueType3" runat="server" Width="100"  IsPercentage="true"  />
                </td>   
  </tr>
  <tr>
  <td style="width: 23%">
                    <asp:Label ID="LabelDataEntry" runat="server" class="iMes_DataEntryLabel">DataEntry</asp:Label>
                </td>
                <td style="width: 30%">
                    <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" Width="80%" CanUseKeyboard="true"
                        IsClear="true" IsPaste="true" MaxLength="14" />
                </td>
      <td style="width: 20%">
                 <asp:Label ID="State" runat="server">State</asp:Label>
                </td>
       <td style="width: 30%">
                  <iMES:CmbConstValueType ID="cmbConstValueType4" runat="server"  Width="100" IsPercentage="true"  />
                </td>  
     </tr>   
     <tr>
             
        <td valign="top" style="width: 23%">
                  <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt">Remark</asp:Label>
               </td>
       <td colspan="3">
                  <textarea id="tareaRemark" runat="server" rows="3" cols="4" style="width: 99%;" onkeyup="OnkeyupRemark()" ></textarea>
                </td>
       </tr>
       <tr>
       <td style="width: 23%">
                    <input id="BtnQuery" type="button" value="Query" onclick="beginWaitingCoverDiv();" runat="server"
                        onserverclick="QueryList" style="width: 80px; text-align: center; cursor: pointer;" />
                </td>
       <td style="width: 15%">
                    <input id="BtnExcel" type="button" value="Excel" onclick="beginWaitingCoverDiv();" runat="server"
                        onserverclick="ToExcel" style="width: 80px; text-align: center; cursor: pointer;" />
                </td>        
                
       </tr> 
    </table>  
      <hr />
        <table width="100%">
            <tr>
                <td style="width: 100%;" align="left" colspan="4">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnQuery" EventName="ServerClick" />
                            <asp:PostBackTrigger ControlID="BtnExcel" />
                        </Triggers>
                        <ContentTemplate>
                            <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                GvExtWidth="98%" GvExtHeight="170px" Width="98%" SetTemplateValueEnable="true"
                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                <Columns>
                                    <asp:BoundField DataField="Pdline" HeaderText="Pdline" HeaderStyle-Width="8%" />
                                    <asp:BoundField DataField="Model" HeaderText="Model" HeaderStyle-Width="13%" />
                                    <asp:BoundField DataField="PCBNo" HeaderText="PCBNo" HeaderStyle-Width="9%" />
                                    <asp:BoundField DataField="Location" HeaderText="Location" HeaderStyle-Width="9%" />
                                    <asp:BoundField DataField="Obligation" HeaderText="Obligation" HeaderStyle-Width="9%" />
                                    <asp:BoundField DataField="IsPass" HeaderText="IsPass" HeaderStyle-Width="6%" />
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-Width="16%" />
                                    <asp:BoundField DataField="Editor" HeaderText="Editor" HeaderStyle-Width="15%" />
                                    <asp:BoundField DataField="Cdt" HeaderText="Cdt" HeaderStyle-Width="15%" />
                                </Columns>
                            </iMES:GridViewExt>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <input id="HiddenSelectType" type="hidden" runat="server" />
    </div>
    <script language="javascript" type="text/javascript">
        var IsPass;
        var editor;
        var customer;
        var pdline;
        var model;
        var olocation;
        var obligation;
        var remark;
        var state;
        var dataEntryObj;

        var AlertSelectModel = "please select Model ";
        var AlertSelectPdline = "please select Pdline";
        var AlertSelectLocation = "please select Location";
        var AlertSelectObligation = "please select Obligation";
        var AlertInputMBSno = "please input MB Sno!";
        var AlertSelectstate = "please Selectstate!";
        var AlertErrInput = "Wrong code,please input MB Sno!";
        var AlertSuccess = "Save successfully!";

        window.onload = function() {
            editor = '<%=Editor %>';
            customer = '<%=Customer %>';


            dataEntryObj = getCommonInputObject();
            getAvailableData("InputDataEntry");
            try {
                dataEntryObj.focus();
            } catch (e) { }
        };
        function InputDataEntry(InputData) {

            if (getPdLineCmbValue() == "")
             {
                 alert(AlertSelectPdline);
                getAvailableData("InputDataEntry");
                setPdLineCmbFocus();
				return;
            }
            // document.getElementById("<%=cmbConstValueType1.ClientID %>").value;
            // getConstValueTypeCmbText()
            if (document.getElementById("<%=cmbConstValueType1.InnerDropDownList.ClientID %>").value == "") {
                alert(AlertSelectModel);
                getAvailableData("InputDataEntry");
                setConstValueTypeFocus();
				return;
            }
            if (document.getElementById("<%=cmbConstValueType2.InnerDropDownList.ClientID %>").value == "") {
                alert(AlertSelectLocation);
                getAvailableData("InputDataEntry");
                setConstValueTypeFocus();
				return;
            }
            if (document.getElementById("<%=cmbConstValueType3.InnerDropDownList.ClientID %>").value == "") {
                alert(AlertSelectObligation);
                getAvailableData("InputDataEntry");
                setConstValueTypeFocus();
				return;
            }
            if (document.getElementById("<%=cmbConstValueType4.InnerDropDownList.ClientID %>").value == "") {
                alert(AlertSelectstate);
                getAvailableData("InputDataEntry");
                setConstValueTypeFocus();
				return;
            }
           if  (InputData.length==10 && InputData.substr(4, 1) == "B") {
               pdline = getPdLineCmbValue();
               model = document.getElementById("<%=cmbConstValueType1.InnerDropDownList.ClientID %>").value
               olocation = document.getElementById("<%=cmbConstValueType2.InnerDropDownList.ClientID %>").value
               obligation = document.getElementById("<%=cmbConstValueType3.InnerDropDownList.ClientID %>").value
               state = document.getElementById("<%=cmbConstValueType4.InnerDropDownList.ClientID %>").value
               remark = document.getElementById("<%=tareaRemark.ClientID %>").value;
               SAInputXRayWebService.Save(InputData, pdline, model, olocation, obligation, remark, state, customer, editor, SaveSucceed);
           }
         
              else
              {     alert(AlertErrInput);
                    dataEntryObj.focus();
                    getAvailableData("InputDataEntry");
            }
        }
       function SaveSucceed(result) {
   
            if (result != null) {
             ShowSuccessfulInfo(true,AlertSuccess);
   
            }
            dataEntryObj.focus();
			 getAvailableData("InputDataEntry");
                setConstValueTypeFocus();
        }


    function ShowError(result) {
        ShowMessage(result);
        ShowInfo(result);
        getAvailableData("InputDataEntry");
        dataEntryObj.focus();
    }

    function OnkeyupRemark() {
        var str = document.getElementById("<%=tareaRemark.ClientID%>").value;
    }


    </script>
    </asp:Content>


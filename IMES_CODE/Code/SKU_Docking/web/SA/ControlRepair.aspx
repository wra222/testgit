<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ControlRepair.aspx.cs" Inherits="CommonControl_1234567" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
 <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
           
        </Services>
    </asp:ScriptManager>
 <center>
    <table border="0" width="98%">
 <tr>
      <td style="width:15%" align="left"><asp:Label ID="lblMBSN" runat="server" 
              CssClass="iMes_label_13pt" >MB:</asp:Label>
      </td>
	    <td style="width:30%" align="left"><asp:Label ID="lblMBSNContext" runat="server" CssClass="iMes_label_11pt" /></td>
 </tr>
 <tr>
    <td nowrap="noWrap" style="width:15%" align="left"><asp:Label ID="lbDataEntry" runat="server" CssClass="iMes_DataEntryLabel" />DataEntry:</td>
	    <td style="width:85%" align="left"> <iMES:Input ID="txt" runat="server" IsClear="true"  ProcessQuickInput="true"  Width="99%"
             CanUseKeyboard="true" IsPaste="true"  MaxLength="50"  InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
            
            
             <td>
                    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
	                    <ContentTemplate>
                        <input type="hidden" runat="server" id="HMBSno" />  
                        <button id="btnSave" runat="server" type="button" style="display: none" />
                        
                     </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
        </td>
</tr>

</table>
 </center>
 <!--
<p>
        <asp:Label ID="MB" runat="server" BorderColor="#0033CC" 
            Font-Size="Large" Height="33px" Width="123px">MB Sno</asp:Label>
        <asp:TextBox ID="MBSno2" runat="server" Height="34px" Width="618px" 
          ></asp:TextBox>
</p>
<p>
        <asp:Label ID="lblmb" runat="server" Font-Size="Large" Height="33px" 
            Text="MB :" Width="73px"></asp:Label>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

        &nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="MBSno3" runat="server" Height="34px" Width="618px" 
          ></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
</p> 

<p>
       <asp:Button ID="Button1" runat="server" BorderColor="#9966FF" Font-Size="Large" 
            Height="35px" Text="Save" Width="125px"  />
</p>
<p>
        <asp:TextBox ID="getSuccess" runat="server" Height="105px" 
             Width="769px"></asp:TextBox>
</p>

<div id="div2">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline">

</asp:UpdatePanel>
</div>
-->
<script language="javascript" type="text/javascript">

//    window.onload = function() {
//    function MBSno2() {
//        MBSno2 = MBSno2.toUpperCase();
//        MBSno2 = MBSno2.trim();
//        if (MBSno2.length == 10) {
//                txtObj.value = "";
//                txtObj.focus();
//            }
//        }
//    }

    document.body.onload = function() {
        try {
            getAvailableData("processDataEntry");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
          
        } catch (e) {
            alert(e.description);
        }
    };
    function processDataEntry(inputData) {

        document.getElementById("<%=lblMBSNContext.ClientID%>").innerText = inputData;
        document.getElementById("<%=HMBSno.ClientID%>").value = inputData;
        document.getElementById("<%=btnSave.ClientID%>").click();
        getAvailableData("processDataEntry");

    }

    function onSaveSucess(msg) {
        alert("确认删除");
        ShowSuccessfulInfo(true, msg);
    }


</script>

</asp:Content>

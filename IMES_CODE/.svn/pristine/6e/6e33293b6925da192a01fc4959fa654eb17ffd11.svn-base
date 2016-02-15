<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="CheckWeightStation.aspx.cs" Inherits="Query_PAK_CheckWeightStation"
    EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
<script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
<script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
  

    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>

    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <center>
        <fieldset id="grpCarton" style="border: thin solid #000000;">
            <legend align="left" style="height: 20px">
                <asp:Label ID="lblTitle" runat="server" Text="Query Pallet Type" CssClass="iMes_label_13pt"
                    meta:resourcekey="lblTitleResource1"></asp:Label></legend>
            <table border="0" width="100%" style="font-family: Tahoma">
                <tr>
                    <td width="10%" align="right">
                    </td>
                    <td align="left" style="width: 25%">
                        <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td width="10%" align="right">
                        <asp:Label ID="lblDate" runat="server" Text="CUSTSN" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="85%" align="left">
                        <iMES:Input ID="txtInput" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                            ReplaceRegularExpression="" Width="95%" IsClear="true" IsPaste="true" />
                    </td>
                </tr>
               
            </table>
        </fieldset>
        <br />
        <table border="0" width="100%" style="font-family: Tahoma">
            <tr>
                <td align="left" width="10%">
                    <asp:Label ID="Label1" runat="server" Text="CUSTSN" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" width="23%">
                    <asp:Label ID="labCustsn" runat="server" Text="" class="value" CssClass="iMes_label_Result"></asp:Label>
                </td>
                <td align="left" width="10%">
                    <asp:Label ID="Label2" runat="server" Text="Product ID" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" width="23%">
                    <asp:Label ID="labProductID" runat="server" Text="" CssClass="iMes_label_Result"></asp:Label>
                </td>
                <td align="left" width="10%">
                    <asp:Label ID="Label3" runat="server" Text="Model"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" width="23%">
                    <asp:Label ID="labModel" runat="server" Text="" Class="value" CssClass="iMes_label_Result"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" width="10%">
                    <asp:Label ID="Label4" runat="server" Text="MO" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" width="23%">
                    <asp:Label ID="labMO" runat="server" Text=""  Class="value" CssClass="iMes_label_Result"></asp:Label>
                </td>
                <td align="left" width="10%">
                    <asp:Label ID="Label6" runat="server" Text="Toal" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" width="23%">
                    <asp:Label ID="labTotal" runat="server" Text="" CssClass="iMes_label_Result"></asp:Label>
                </td>
                <td align="left" width="10%">
                    <asp:Label ID="Label8" runat="server" Text="Pass" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" width="23%">
                    <asp:Label ID="labPass" runat="server" Text="" CssClass="iMes_label_Result"></asp:Label>
                </td>
            </tr>
            <tr >
             <td colspan="6" height="350px" align="center">
              <asp:Label ID="labMsg" runat="server" Font-Size="58px"></asp:Label>
             </td>
            </tr>
        </table>
        <br />
        <br />
        
    </center>
   
    <script type="text/javascript">    //<![CDATA[

   var custsn;
    window.onload = function() {
        inputObj = getCommonInputObject();
        getAvailableData("processFun");
        
      //  var obj = document.getElementById("<%=txtInput.ClientID%>"); 
       inputObj.focus();
     //   $("#<%=labMsg.ClientID%>").text("Invalid Date").css('color', '#669966');
    };

    function processFun(backData) {
        var len = backData.trim().length;

        if (len != 10 ) {
            ShowMessage('Please input correct CUSTSN');
            
           
        }
        else {
            ShowWait();
            custsn=backData;
            PageMethods.GetQueryResult(backData, onSuccess, onError);
                 
        }
        getAvailableData("processFun");
    }

    
    function onSuccess(receiveData) {

       
        if(receiveData.length==1)
        {
            var s=receiveData[0];
            $("#<%=labMsg.ClientID%>").text( receiveData[0]).css('color', '#FF0000');
           //  $("#<%=labCustsn.ClientID%>").text('');
             $('.iMes_label_Result').text('');
           
//             $("#<%=labMO.ClientID%>").text('');
//             $("#<%=labTotal.ClientID%>").text('');
//          $("#<%=labPass.ClientID%>").text('');
//          $("#<%=labModel.ClientID%>").text('');
//          $("#<%=labProductID.ClientID%>").text('');
          
        }
        else
        {
          //msgWeight MO MOQty  PassQty Model
          //a.startsWith("abc")
          var msg=receiveData[0];
          var color='#008000' ;//Green
          if(msg.startsWith("Fail")) { color='#FF0000';}
            $("#<%=labMsg.ClientID%>").text(msg).css('color', color);
           $("#<%=labCustsn.ClientID%>").text(custsn);
         
          $("#<%=labMO.ClientID%>").text(receiveData[1]);
          $("#<%=labTotal.ClientID%>").text(receiveData[2]);
          $("#<%=labPass.ClientID%>").text(receiveData[3]);
          $("#<%=labModel.ClientID%>").text(receiveData[4]);
          $("#<%=labProductID.ClientID%>").text(receiveData[5]);
     
        }
       HideWait();
    }
    // 失敗時彈出失敗訊息
    function onError(error) {
        if (error != null)
            alert(error.get_message());
            HideWait();
    }
  
   
    </script>

</asp:Content>

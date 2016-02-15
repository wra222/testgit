<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="WithdrawTest.aspx.cs" Inherits="Query_PAK_WithdrawTest"  EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 
<script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
<script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../../js/jscal2.js"></script>
    <script type="text/javascript" src="../../js/lang/en.js"></script>
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <style type="text/css">
    .querycell
    {
        background-color: yellow;
     
    }
    .querycell:hover
    {
        background-color: #8AF2E7;
     
    }
     tr.clicked
    {
        background-color: white; 
    }
    .clicked
    {
        background-color: #8AF2E7;
    }
  .row1
    {
  	  background-color: #FFF8DC
  	  }
    .row2
    {
      background-color: #98FF98;
     }
     .row2:hover, .row1:hover
{
	background-color: white;
}
</style>    
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Withdraw Test" 
            CssClass="iMes_label_13pt" ></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="10%" align="left">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt" 
                        meta:resourcekey="lblDBResource1"></asp:Label></td>                
                <td width ="25%" align="left">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>        
               <td width ="5%" align="right">
                  
                </td>
                 <td width ="15%" align="left"> 
                </td>
                
                <td width ="10%" align="right">
                    &nbsp;</td>       
              <td width ="15%" align="left">
                   &nbsp;</td> 
              
                <td width ="5%"  align="right">
            
                <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click" OnClientClick=" return CheckInput()"/>
                </td>
            </tr>
    <tr>
    <td colspan="3" align="left">
    
        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt" 
            Text="Model:"></asp:Label>
    
                    <asp:TextBox ID="txtModel" runat="server" Height="19px" Width="200px"></asp:TextBox>
                
                 <asp:Label ID="lblShipDate" runat="server" Text="Ship Date:" 
                        CssClass="iMes_label_13pt" ></asp:Label>
                   <asp:TextBox ID="txtShipDate" runat="server" Width="105px" Height="20px" ></asp:TextBox>                                                         
                                               
    </td>
    <td colspan="3" align="left">
        <asp:Label ID="Label1" runat="server" Text="Status" CssClass="iMes_label_13pt"></asp:Label>
        <asp:DropDownList ID="dropStatus" runat="server" Height="20px">
            <asp:ListItem>ALL</asp:ListItem>
            <asp:ListItem Value="Empty">未結合 (Empty)</asp:ListItem>
            <asp:ListItem Value="Partial">部分結合(Partial)</asp:ListItem>
            <asp:ListItem Value="Full">結合完成(Full)</asp:ListItem>
        </asp:DropDownList>
    </td>
    <td   align="right">
      
    </tr>
         </table>
</fieldset> 
<br />
   
   <table>
    <tr>
      <td align="left">
           <asp:Label ID="Label2" runat="server" Text="Shipment Combine Status List:" 
                        CssClass="iMes_label_13pt" ></asp:Label>
      <input id="BtnBrowse" type="button" value="Withdraw Test" style=" display:none"  onclick="WTest()" />
      </td>
   </tr>
   <tr>
   <td>
 
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
      
       <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="400px"   AutoGenerateColumns="false"
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" OnRowDataBound="gvResult_RowDataBound"
            meta:resourcekey="gvResultResource1" OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False">
              <Columns>
                                        
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox id="chk" runat="server"  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Consolidated" />
                                        <asp:BoundField DataField="PalletNo" />
                                        <asp:BoundField DataField="DeliveryNo" />
                                        <asp:BoundField DataField="CartonQty" />
                                        <asp:BoundField DataField="DeviceQty" />
                                        <asp:BoundField DataField="CombinedStatus" />
                                      
                                    </Columns>
        </iMES:GridViewExt>
        <asp:HiddenField ID="hidType" value="" runat="server" />
          <asp:HiddenField ID="hidSelectID" value="" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
    </td>
   </tr>
   </table>
   
</center>

<script type="text/javascript">    //<![CDATA[

    var idArr;
    var dn10;
    Array.prototype.remove = function(obj) {
        return RemoveArray(this, obj);
    };
    window.onload = function() {
      
        //Get Edites Params
        idArr = new Array();

    }
    function RemoveArray(array, attachId) {
        for (var i = 0, n = 0; i < array.length; i++) {
            if (array[i] != attachId) {
                array[n++] = array[i]
            }
        }
        array.length -= 1;
    }
    Calendar.setup({
        inputField: ConvertID("txtShipDate"),
        trigger: ConvertID("txtShipDate"),
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });
    function ShowBtn(b) {
        if (b=='False')
        { $("#BtnBrowse").hide(); } //BtnBrowse
        else
        { $("#BtnBrowse").show(); }
    }
    function CheckTest() {

        document.getElementById("<%=hidSelectID.ClientID%>").value = idArr.join(',');
    
    }
    function CheckInput() {
        //$('#myhidden').val(x);
      
        var txtId = '#' + ConvertID('txtModel');
        var model= $(txtId).val();
        if (model.trim() == '')
            { alert('Please input Model'); return false; }

            idArr = [];
        
        beginWaitingCoverDiv();
     

//        var model = document.getElementById(ConvertID("txtModel")).value;
//        if (trim(model) == "")
//        { ShowMessage('請輸入 Model!'); return false; }
//        else
//        { beginWaitingCoverDiv(); }
//       
    
    }
    function trim(stringToTrim) {
        return stringToTrim.replace(/^\s+|\s+$/g, "");
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
    function WTest() {
        var shipD = document.getElementById("<%=txtShipDate.ClientID%>").value; 
        var model = document.getElementById("<%=txtModel.ClientID%>").value;

        var idList = idArr.join(',');
        if (idList == "")
        {alert('Please select data to test');return;}
        var dlgFeature = "dialogHeight:600px;dialogWidth:1000px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "WithdrawTestResult.aspx?DNList=" + idList + '&ShipDate=' + shipD +'&Model='+model;
        var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
//        if (dlgReturn) {

//            dlgReturn = dlgReturn.replace(/\r\n/g, ",");
//      



//        }
//        else {
//            if (dlgReturn == "")
//            { }
//            return;
//        }

    }
   
   </script>


   



</asp:Content>


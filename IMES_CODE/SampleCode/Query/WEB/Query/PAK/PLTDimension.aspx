<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="PLTDimension.aspx.cs" Inherits="Query_PAK_PLTDimension"  EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto"%>
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
    
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="PLT Dimension" 
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
    
        <asp:RadioButton ID="radModel" runat="server" GroupName="radGr"   
            Checked="True" />
        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt" 
            Text="Model:"></asp:Label>
    
                    <asp:TextBox ID="txtModel" runat="server" Height="19px" Width="200px"></asp:TextBox>
                
                 <asp:Label ID="lblShipDate" runat="server" Text="Ship Date:" 
                        CssClass="iMes_label_13pt" ></asp:Label>
                   <asp:TextBox ID="txtShipDate" runat="server" Width="105px" Height="20px" ></asp:TextBox>                                                         
                                               
    </td>
    <td colspan="3" align="left">
      <asp:RadioButton ID="radPalletNo" runat="server" GroupName="radGr" 
            Text="Pallet No"  />
      <asp:RadioButton ID="radDN" runat="server" GroupName="radGr" Text="DN" />
    
                    <asp:TextBox ID="txtInput" runat="server" Height="19px" 
            Width="200px"></asp:TextBox>
    </td>
    <td   align="right">
       <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="Excel" style=" display:none"/></td>
    
    </tr>
         </table>
</fieldset> 
<br />
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
      
       <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="400px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            AutoHighlightScrollByValue="False" GetTemplateValueEnable="False" 
            HiddenColCount="0" HighLightRowPosition="1" 
            meta:resourcekey="gvResultResource1" OnGvExtRowClick="" OnGvExtRowDblClick="" 
            SetTemplateValueEnable="False">
        </iMES:GridViewExt>
        <asp:HiddenField ID="hidType" value="" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
    </Triggers>
  </asp:UpdatePanel>
</center>

<script type="text/javascript">    //<![CDATA[
    Calendar.setup({
        inputField: ConvertID("txtShipDate"),
        trigger: ConvertID("txtShipDate"),
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });
    function CheckInput() {
        //$('#myhidden').val(x);
        var hidTypeID = '#' + ConvertID('hidType');
        
        
        var item = $('input[type=radio]:checked').val();
        var txtId = '#' + ConvertID('txtInput');
        var txtValue = $(txtId).val();
        if (item == 'radPalletNo') {
         
            if (txtValue.trim() == '')
            { alert('Please input Pallet No'); return false; }
            $(hidTypeID).val('Pallet');
        }
        else if (item == 'radDN') {
            if (txtValue.trim() == '')
           { alert('Please input DN'); return false; }
            $(hidTypeID).val('DN');
        }
        else
        { $(hidTypeID).val('Model'); }
    
        
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
   
   </script>


     </table>



</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="MrpLabelQuery.aspx.cs" Inherits="Query_PAK_MrpLabelQuery" Title="QCPAKMRPLabelQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script src="../../js/jscal2.js"></script>
    <script src="../../js/lang/en.js"></script>
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
      
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
  <center>
 <fieldset id="grpCarton" style="border: thin solid #000000; height: 500px;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="QCMrpLabelQuery" 
            CssClass="iMes_label_13pt"></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma; height: 140px;">                    
            <tr>
                <td width ="10%">
                      <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                 <td colspan="3">                         
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                    
                </td>                
                
            </tr>
            <tr>
               <td width ="10%">
                    <asp:Label ID="lblCUSTSN" runat="server" Text="DN/CUSTSN¡G" 
                        CssClass="iMes_label_13pt"></asp:Label>                    
               </td>
               <td width ="80%">
                   <iMES:Input ID="Input1" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" ReplaceRegularExpression="" Width="80%" IsClear="true" IsPaste="true" />
               </td>            
                <td>
             <input type="hidden" id="hidProduct" runat="server" />            
            <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click"    style="width: 100px; display: none"  >Query</button>
 
                  
                </td>
            </tr>
 
         </table>
         <textarea id="Textarea1" class="iMes_Master_msgbox" readonly="readOnly" 
         tabindex="2"  runat="server" style="height: 68px"> </textarea>
</fieldset> 
    
     
       
</center>
    
<script  language="javascript">
window.onload = function() {
        inputObj = getCommonInputObject();
        getAvailableData("processFun");
    };

    function processFun(backData) {
        document.getElementById("<%=Textarea1.ClientID %>").value = "";
        document.getElementById("<%=hidProduct.ClientID %>").value = backData;
        document.getElementById("<%=btnQuery.ClientID%>").click();
        ResetPage();
    }
    function ResetPage() {
        inputObj.value = "";
        
        getAvailableData("processFun");
        inputObj.focus();
    }


</script>
</asp:Content>


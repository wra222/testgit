<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="QueryIndiaMPRLabel.aspx.cs" Inherits="Query_PAK_QueryIndiaMPRLabel"  EnableEventValidation="false"  %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 
   
     <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
<script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
 <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     

          


   
  <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" >        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Query Pallet Type" 
            CssClass="iMes_label_13pt" meta:resourcekey="lblTitleResource1"></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td width ="5%" align="right">
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt" 
                        meta:resourcekey="lblDBResource1"></asp:Label></td>                
                <td align="left" style="width: 25%">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>        
                <td width ="10%" align="right">
                 <asp:Label ID="lblDate" runat="server" Text="DN/CUSTSN" CssClass="iMes_label_13pt"  ></asp:Label>
                </td>       
                <td width ="30%" align="left">
                                                                      
                                               
                  <iMES:Input ID="txtInput" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" ReplaceRegularExpression="" Width="80%" IsClear="true" IsPaste="true"/>
              
                    <td width ="5%">
                               
            
                </td>
            </tr>
    <tr>
    <td colspan="3" align="left">
     
    </td>
    </tr>
         </table>
</fieldset> 
<br />
        <asp:TextBox ID="txtResult" runat="server" Font-Size="X-Large" Height="49px" 
            ReadOnly="True" Width="605px" 
        style="text-align: center; line-height:40px" ForeColor="#FF3300"></asp:TextBox>

</center>

<script type="text/javascript">    //<![CDATA[


    window.onload = function() {
        inputObj = getCommonInputObject();
        getAvailableData("processFun");
    };

    function processFun(backData) {
        var len = backData.trim().length;

        if (len != 10 && len != 16) {
            ShowMessage('Please input correct CUSTSN or DN');
            
           
        }
        else {
            PageMethods.GetQueryResult(backData, onSuccess, onError);
        }
        getAvailableData("processFun");
    }

    
    function onSuccess(receiveData) {

        var resultID ='#'+ ConvertID('txtResult');
        $(resultID).val(receiveData);
      
    }
    // 失敗時彈出失敗訊息
    function onError(error) {
        if (error != null)
            alert(error.get_message());
    }
  
   
    </script>


</asp:Content>


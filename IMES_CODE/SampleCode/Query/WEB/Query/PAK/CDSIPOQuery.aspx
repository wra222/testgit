<%@ Page Title="CDSIPOQuery" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="CDSIPOQuery.aspx.cs" Inherits="Query_PAK_CDSIPOQuery"   EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
        <script type="text/javascript" src="../../js/jquery-1.7.1.js "></script>
   
    <script src="../../js/jscal2.js"></script>
    <script src="../../js/lang/en.js"></script>
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
      
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
  
  <center>
<fieldset id="grpCarton" style="border: thin solid #000000;">
     <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="CDSI PO Query" CssClass="iMes_label_13pt"></asp:Label></legend>        
        <table border="0" width="100%" style="font-family: Tahoma">                    
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
                   <asp:Label ID="lblHPPO" runat="server" Text="HP PO:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
               <td width ="40%">
                   <asp:TextBox ID="txtHPPO" runat="server" Width="300px" Height="20px"></asp:TextBox>
                   <input id="BtnBrowse" type="button" value="Browse"  onclick="UploadModelList()" />                                                
               </td>        
               <td width ="10%">                  
               </td>
               <td width ="40%">                    
               </td>                       
            </tr>
             <tr>               
                <td colspan="4" align="center">                    
                    <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="beginWaitingCoverDiv();" style="width: 100px">Query</button>
                    &nbsp;&nbsp;&nbsp;                    
                    <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px; display: none;">Export</button>
                </td>                                
            </tr>                     
         </table>
         
         <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
         
    <ContentTemplate>      
       <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="300px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 6px; left: 0px">            
           <HeaderStyle Font-Size="Smaller" Width="50px" />
        </iMES:GridViewExt>     
        <asp:HiddenField ID="hidModelList" runat="server" />                   
    </ContentTemplate>
    <Triggers>                 
         <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
    </Triggers>  
  </asp:UpdatePanel>
      
         
</fieldset>
</center>
<script type="text/javascript">       
function UploadModelList() {
        var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "../../Query/FA/UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelList.ClientID %>").value;
        var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
        if (dlgReturn) {

            dlgReturn = dlgReturn.replace(/\r\n/g, ",");
            document.getElementById("<%=hidModelList.ClientID %>").value = RemoveBlank(dlgReturn);

            //   document.getElementById("<%=hidModelList.ClientID %>").value = dlgReturn;


        }
        else {
            if (dlgReturn == "")
            { document.getElementById("<%=hidModelList.ClientID %>").value = ""; }
            return;
        }

    }

    function RemoveBlank(modelList) {
        var arr = modelList.split(",");
        var model = "";
        if (modelList != "") {
            for (var m in arr) {
                if (arr[m] != "") {
                    model = model + arr[m] + ",";
                }


                // content += key + ' : ' + myarr[key] + '<br />';

            }
            model = model.substring(0, model.length - 1)
        }

        return model;
    }    
</script>
</asp:Content>


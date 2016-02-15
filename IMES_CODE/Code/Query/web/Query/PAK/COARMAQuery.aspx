﻿<%@ Page Title="ProductDistribute" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="COARMAQuery.aspx.cs" Inherits="Query_PAK_COARMAQuery" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 

 <script type="text/javascript" src="../../js/jquery-1.7.1.js "></script>    
    <script type="text/javascript" src="../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js "></script>     
    <script src="../../js/jscal2.js "></script>
    <script src="../../js/lang/cn.js "></script>    

    <script type="text/javascript" src="../../js/jquery.multiselect.js "></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js "></script>     
    <script type="text/javascript" src="../../js/wz_tooltip.js "></script>
        
    
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
<style type="text/css">
        
    tr.clicked
    {
        background-color: white; 
    }
    .querycell
    {
        background-color: yellow;
     
    }
    .querycell:hover , .querycell.clicked
    {
        background-color: Blue;                       
    }
    


    .style2
    {
        width: 5%;
    }
    


    .style3
    {
        width: 29%;
    }
    


</style>   
                      

<script type="text/javascript">

 </script>
 
 <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>        
             <script language="javascript" type="text/javascript">
                </script>
    </ContentTemplate>    
  </asp:UpdatePanel>  
   <body>                  
<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="COARMAQuery" 
            CssClass="iMes_label_13pt"></asp:Label></legend> 
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
                <td class="style2">
                      <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                </td>                
                <td class="style3">                        
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                </td>                
                <td rowspan="3" width="30%" >
                    <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" 
                                     onclick="beginWaitingCoverDiv();" style="width: 100px">Query</button>
                    <br />
                    <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                                    style="width: 100px; display: none;">Export</button>
                    <br />

                </td>
            </tr>
            <tr>
               <td class="style2" rowspan="2">
                   <asp:RadioButtonList ID="rdbutCOA" runat="server" Width="100px">
                       <asp:ListItem Selected="True" Value="RMA">RMA No：</asp:ListItem>
                       <asp:ListItem Value="COA">COA No：</asp:ListItem>
                   </asp:RadioButtonList>
               </td>
               <td class="style3">
                   <asp:TextBox ID="txtRMA" runat="server" Width="300px">
                   </asp:TextBox>
                   <input id="BtnBrowseRMA" type="button" value="Browse"  onclick="UploadRMAList()" /></td>            
            </tr>
            <tr>
               <td class="style3" >                   
                   <asp:TextBox ID="txtCOA" runat="server" Width="300px">
                   </asp:TextBox>
                   <input id="BtnBrowseCOA" type="button" value="Browse"  onclick="UploadCOAList()" />
               </td>            
            </tr>
         </table>
</fieldset> 
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
       <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="450px" 
            Width="98%" GvExtWidth="98%" Height="1px" ShowFooter="True" 
                style="top: -1px; left: 2px">            
        </iMES:GridViewExt>        
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="serverclick" />
        </Triggers>
     </asp:UpdatePanel>
     
       <asp:UpdatePanel runat="server">
        <ContentTemplate>
           <asp:HiddenField ID="hidModelListRMA" runat="server" />
           <asp:HiddenField ID="hidModelListCOA" runat="server" />        
        </ContentTemplate>
     </asp:UpdatePanel>
</center>
<script type="text/javascript">
    function UploadCOAList() {
        var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "../../Query/FA/UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelListCOA.ClientID %>").value;
        var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
        if (dlgReturn) {

            dlgReturn = dlgReturn.replace(/\r\n/g, ",");
            document.getElementById("<%=hidModelListCOA.ClientID %>").value = RemoveBlank(dlgReturn);
        }
        else {
            if (dlgReturn == "")
            { document.getElementById("<%=hidModelListCOA.ClientID %>").value = ""; }
            return;
        }

    }
     function UploadRMAList() {
        var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "../../Query/FA/UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelListRMA.ClientID %>").value;
        var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
        if (dlgReturn) {

            dlgReturn = dlgReturn.replace(/\r\n/g, ",");
            document.getElementById("<%=hidModelListRMA.ClientID %>").value = RemoveBlank(dlgReturn);
        }
        else {
            if (dlgReturn == "")
            { document.getElementById("<%=hidModelListRMA.ClientID %>").value = ""; }
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

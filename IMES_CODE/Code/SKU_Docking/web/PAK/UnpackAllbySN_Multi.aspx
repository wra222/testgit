<%--
 INVENTEC corporation (c)2011 all rights reserved. 
 Description: Unpack All by SN(Multi)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2013-04-16 Benson           Create 
 Known issues:
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"  EnableViewState="false"
    CodeFile="UnpackAllbySN_Multi.aspx.cs" Inherits="PAK_UnpackDN_Multi" Title="Multi Unpack" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="Service/UnpackService.asmx" />
        </Services>
    </asp:ScriptManager>
<div>
   <center >
      <fieldset style="width:98%">
                <legend id="ctl00_iMESContent_lgUpload" align="center"  class="iMes_label_13pt">Input CUSTSN</legend>

     <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
        
        <tr >
            
            <td  align="left" style="width:20%"  >
           
                         <input id="BtnBrowse" onclick="UploadCustsnList()" type="button"   style ="width:110px; height:24px;" 
                             value="Input" />
                <asp:Label ID="lblCount" runat="server"  CssClass="iMes_label_13pt" Text="0 筆CUSTSN"   ></asp:Label>
        </td>
        <td align="left"  >
             
          
            <input id="Button2" type="button" value="Unpack" onclick="Run()" style ="width:110px; height:24px;"/>
        </td>
        </tr>
        </table>
        </fieldset>

       <table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
     
        <tr>
       <td>
        
           <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
               <iMES:GridViewExt ID="grvSN" runat="server" AutoGenerateColumns="false" GvExtWidth="100%"
                                GvExtHeight="350px" Style="top: 0px; left: 0px" Width="98%"  SetTemplateValueEnable="False"
                                HighLightRowPosition="3" AutoHighlightScrollByValue="True" onrowdatabound="grvSN_RowDataBound">
                                 <Columns>
                                            <asp:BoundField DataField="CUSTSN"  />
                                             <asp:BoundField DataField="Result"    />
                                            <asp:BoundField DataField="Error Message"  HtmlEncode="false"/>
                                  </Columns>
                                 
                            </iMES:GridViewExt>
                                 <button id="btnHidden" runat="server" onserverclick="Button1_Click"  value="ok2" Style="display: none" >  </button>   
               <asp:HiddenField ID="hidCustsnList" runat="server" />
                   </ContentTemplate>
                        
                    </asp:UpdatePanel>  
            
      
          </td>
        </tr>
  </table>
  

            
  </center>
     
      
      <asp:HiddenField ID="hidDefaultCount" runat="server" />
      
</div>

    <script type="text/javascript">
    
        var grvSNClientID = "<%=grvSN.ClientID%>";
        var msgConfirmUnpack = '<%=this.GetLocalResourceObject(Pre + "_msgConfirmUnpack").ToString()%>';
        var maxSnCount = parseInt('<%=maxSnCount%>', 10);
        var inputCount = 0;
        window.onload = function() {

            ShowInfo("");
    
        }

       window.onbeforeunload = function() {
        ResetSn();
        }

     
     
        function ResetSn() {
            document.getElementById("<%=hidCustsnList.ClientID %>").value = "";
            document.getElementById("<%=lblCount.ClientID %>").innerHTML = "0 筆CUSTSN";
        }
    
        
        function UploadCustsnList() {

            var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
            var saveasUrl = "InputDNlist.aspx?DNList=" + document.getElementById("<%=hidCustsnList.ClientID %>").value;
            var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
            if (dlgReturn) {

                dlgReturn = dlgReturn.replace(/\r\n/g, ",");
                document.getElementById("<%=hidCustsnList.ClientID %>").value = RemoveBlank(dlgReturn);
                var arr = document.getElementById("<%=hidCustsnList.ClientID %>").value.split(',');
                inputCount = arr.length;
                document.getElementById("<%=lblCount.ClientID %>").innerHTML = inputCount + " 筆CUSTSN";
              
            }
            else {
                if (dlgReturn == "")
                { document.getElementById("<%=hidCustsnList.ClientID %>").value = ""; }
                return;

            }

        }
        function RemoveBlank(dnList) {
            var arr = dnList.split(",");
            var dn = "";
            if (dnList != "") {
                for (var m in arr) {
                    if (arr[m].trim() != "") {
                        dn = dn + arr[m].toUpperCase() + ",";
                    }

                }
                dn = dn.substring(0, dn.length - 1)
            }

            return dn;
        }
        function Run() {
            if (document.getElementById("<%=hidCustsnList.ClientID %>").value.trim() == "") {
                ShowMessage("Please input SN!");
                ShowInfo("Please input SN!");
                return false;
            }
            else if (inputCount > maxSnCount) {
            ShowMessage("不可輸入超過 " + maxSnCount +" 筆 SN ");
                ShowInfo("不可輸入超過 " + maxSnCount +" 筆 SN");
            return;
             }
             else {
                 if (confirm(msgConfirmUnpack)) {
                     ShowInfo("");
                     document.getElementById("<%=btnHidden.ClientID%>").click();
                     beginWaitingCoverDiv();
                     return true;
                 }
               
            }
        
        }
    
    </script>

</asp:Content>

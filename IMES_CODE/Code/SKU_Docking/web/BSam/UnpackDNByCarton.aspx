<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Combine Material and Lot
 * CI-MES12-SPEC-PAK-UC Unpack1.doc
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2013-04-25  IEC000043             Create
 * Known issues:
 * TODO:
*/
--%>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UnpackDNByCarton.aspx.cs" Inherits="BSam_UnpackDNByCarton"    Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>


    <script type="text/javascript" language="javascript">
     
        var msgConfirmUnpackCarton = '<%=this.GetLocalResourceObject(Pre + "_msgConfirmUnpackCarton").ToString()%>';
        var msgNullSN = '<%=this.GetLocalResourceObject(Pre + "_msgNullSN").ToString()%>';
        
         window.onload = function() {
            CallNextInput();
           
        }
        window.onbeforeunload = function() {
        }
        function ExitPage() {
        }
            
      
        function input(data) {

            CheckDN();

        }
        function CheckDN() {
            var sn = getCommonInputObject().value;
            if (sn == "") {
                alert(msgNullSN);
                CallNextInput();
                return;
            }
            ShowInfo("");
            beginWaitingCoverDiv();
            PageMethods.CheckDN(sn, OnSuccessCheckDN, OnError);
            getAvailableData("input");
        }
        //****** End Input Function  *******
        function OnSuccessCheckDN(result) {
            endWaitingCoverDiv();
       
            var dnLst = result[1];
            if (dnLst != '') {
               msgConfirmUnpackCarton = "此DN " + dnLst + " 状态是98,请再次确认是否还需要Unpack?";
           }
           if (confirm(msgConfirmUnpackCarton)) {
              beginWaitingCoverDiv();
              PageMethods.Unpack(result[0], OnSuccessUnpack, OnError);
           }
          
           CallNextInput();
        }
        function OnSuccessUnpack() {
            endWaitingCoverDiv();
            ShowInfo("Success!!","green");
            CallNextInput();
        }

        function OnError(error) {
            endWaitingCoverDiv();
            ShowALL(error.get_message());
            CallNextInput();
        }
       
        function CallNextInput() {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }

     
        function ShowALL(msg) {
            ShowMessage(msg);
            ShowInfo(msg);
        }
     
    </script>

    <div>
        <center>
          
           
            <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center" style="height: 50px">
                <tr valign="middle">
                    <td width="20%" align="left">
                        <asp:Label ID="lbDataEntry" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                   </td>
                    <td width="60%" align="left">
                        <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                            CanUseKeyboard="true" IsClear="false" IsPaste="true" />
                    </td>
                     <td width="20%" align="right">
                        <input id="Button2" onclick="CheckDN()" style="width:110px; height:24px;" type="button" value="Unpack" />
                     </td>
                </tr>
            </table>
            <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
                <tr>
                    <td>
                    </td>
                
                </tr>
            </table>
        </center>
    </div>
  
</asp:Content>

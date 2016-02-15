<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for SN Check Page
 * UI:CI-MES12-SPEC-PAK-UI SN Check.docx –2011/10/20 
 * UC:CI-MES12-SPEC-PAK-UC SN Check.docx –2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-20   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0542, Jessica Liu, 2012-2-18
* ITC-1360-0824, Jessica Liu, 2012-2-28
* ITC-1360-0824, Jessica Liu, 2012-3-7
* ITC-1360-1514, Jessica Liu, 2012-3-20
* ITC-1360-1678, Jessica Liu, 2012-4-11
* ITC-1360-1686, Jessica Liu, 2012-4-12
*/
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="~/PAK/CheckDoorSn.aspx.cs"    Inherits="PAK_CheckDoorSn" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<style type="text/css">
    .style1
    {
        width: 123px;
      
    }
    .style2
    {
        width: 143px;
    }
</style>
 
<script type="text/javascript">

    var msgDataEntryField = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryField").ToString() %>';
    //ITC-1360-0542, Jessica Liu, 2012-2-18
    var msgScanAnotherSN = '<%=this.GetLocalResourceObject(Pre + "_msgScanAnotherSN").ToString()%>'; // '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgScanAnotherSN") %>';
    var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
    var inputObj = "";
    var inputData = "";
    var station = "";
    var pCode = "";
   var firstInput = false;
    var flag = false;
    var msgNoInputCustomerSn = '<%=this.GetLocalResourceObject(Pre + "_msgNoInputCustomerSn").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var doorSn = "";
    var custsn = "";
    var custsn2 = "";
    var needInputDoorSn = false;
    var FrameSn = "";
    var needInputFrameSn = false;
//    document.getElementById("lbdoor").style.visibility = "hidden";
//    document.getElementById("lblframe").style.visibility = "hidden";
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	onload
    //| Author		:	Jessica Liu
    //| Create Date	:	10/11/2011
    //| Description	:	加载接受输入数据事件
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    document.body.onload = function() {
        try {
            inputObj = getCommonInputObject();
            ShowInfo("");
            inputData = inputObj.value;
            inputObj.focus();
            station = '<%=Request["Station"] %>';
 
            getAvailableData("processDataEntry");
             document.getElementById("lbdoor").style.visibility = "hidden";
              document.getElementById("lblframe").style.visibility = "hidden";

        } catch (e) {
            alert(e.description);

            inputObj.focus();
        }

    }
    
    function CustomerSNEnterOrTab() 
    {
        if (event.keyCode == 9 || event.keyCode == 13) 
        {
            getAvailableData("processDataEntry");
        }
    }
    function ShowALL(msg) {
        ShowMessage(msg);
        ShowInfo(msg); 
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //| Name		:	processDataEntry
    //| Author		:	Jessica Liu
    //| Create Date	:	10/12/2011
    //| Description	:	检测并进行数据检索及显示
    //| Input para.	:	
    //| Ret value	:	
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    function processDataEntry(data) {
        inputData = data;
        ShowInfo("");
        var uutInput = true;
        var line = getPdLineCmbValue();
        lstPrintItem = getPrintItemCollection();
        if (line == "") {
            ShowInfo("Please select line!");
            callNextInput();
            return;
        }
        if (custsn == "") {
            beginWaitingCoverDiv();
            WebServiceCheckDoorSn.InputCustSN(data, line, "<%=UserId%>", station, "<%=Customer%>", onFSNSucceed, onFSNFail);

        }
        else {

            if (custsn2 == "") {

                if (data != "A" + custsn && data != "P" + custsn) {
                    ShowALL("Wrong Customer SN!!"); callNextInput();
                }
                else {

                   if (!needInputDoorSn && !needInputFrameSn)
                     { 
                            Save();
                     }
                    else {
                            custsn2 = data;
                            if (needInputFrameSn && needInputDoorSn) {
                                ShowInfo("Please input HDDFrame Or Door SN!!");

                            }
                            else if (needInputFrameSn && !needInputDoorSn) {
                                ShowInfo("Please input HDDFrame SN!!");

                            }
                            else {
                                ShowInfo("Please input Door SN!!");
                            }
                            callNextInput();
                      }
                    }

                
            }
            else {  //     if (custsn2 == "")
                    if (needInputDoorSn && needInputFrameSn) {
                      if (data.toString().substring(0, 5) != doorSn.substring(0, 5) && data.toString().substring(0, 5) != FrameSn.substring(0, 5)) {
                        ShowALL("Wrong Door Or Frame SN!!"); callNextInput();
                    }
                    else if (data.toString().substring(0, 5) == doorSn.substring(0, 5)) {
                        ShowInfo("Please input HDDFrame SN!!");
                        needInputDoorSn = false;
                        document.getElementById("<%=lbl_DoorSn.ClientID %>").innerHTML = data;
                        callNextInput();
                    }
                    else if (data.toString().substring(0, 5) == FrameSn.substring(0, 5)) {
                    ShowInfo("Please input Door SN!!");
                    document.getElementById("<%=lbl_FrameSn.ClientID %>").innerHTML = data;
                        needInputFrameSn = false;
                        callNextInput();
                    }


                }
                else if (needInputFrameSn && !needInputDoorSn) {
                if (data.toString().substring(0, 5) == FrameSn.substring(0, 5)) {
                    document.getElementById("<%=lbl_FrameSn.ClientID %>").innerHTML = data;
                        Save();
                    }
                    esle
                    {
                        ShowInfo("Wrong   Frame SN!!"); callNextInput();
                    }
                }
                else if (needInputDoorSn && !needInputFrameSn) {
                if (data.toString().substring(0, 5) == doorSn.substring(0, 5)) {
                    document.getElementById("<%=lbl_DoorSn.ClientID %>").innerHTML = data;
                        Save();
                    }
                    else
                    {
                        ShowInfo("Wrong   Door SN!!"); callNextInput();
                    }
                }
            
            }
       }
   }

    function Save() {
      beginWaitingCoverDiv();
      WebServiceCheckDoorSn.Save(custsn, onSaveSucceed,onFSNFail);
    
    }
//    function callNextInput() {
//        getCommonInputObject().value = "";
//        getCommonInputObject().focus();
//        getAvailableData("input");
//    }
    
    function onFSNSucceed(result) 
    {
        endWaitingCoverDiv();
        needInputDoorSn = false;
         needInputFrameSn = false;
         custsn = inputData;
        try {
             if (result == null) 
            {
                ShowInfo("");
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
               
            }
            else if ((result.length !=1) && (result[0] == SUCCESSRET)) 
            {
                ShowInfo("Please input other Customer SN!");
                document.getElementById("lblframe").style.visibility = "hidden";
                document.getElementById("lbdoor").style.visibility = "hidden";  
//                custsn = inputData;
               
                document.getElementById("<%=lblCustSn.ClientID %>").innerHTML = custsn;

                doorSn = result[1];
                if (doorSn != "" && doorSn!=null) {
                    //custsn2 = inputData;
                    //ShowInfo("Please input Door SN!!");
                    needInputDoorSn = true;
                    document.getElementById("lbdoor").style.visibility = "visible";
                    
                }
                FrameSn = result[2];
                if (FrameSn != "" && FrameSn!=null) {
                    //custsn2 = inputData;
                    //ShowInfo("Please input Door SN!!");
                    needInputFrameSn = true;
                    document.getElementById("lblframe").style.visibility = "visible";
                   
                }
//                else {
//                    ShowInfo("Please input other Customer SN!");
//                }
           
               
            }
            else 
            {
                ShowInfo("");
                var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
                ExitPage();
            }
            callNextInput();
        } catch (e) {
            alert(e);
            callNextInput();
        }
    }


    function onFSNFail(error) 
    {
        endWaitingCoverDiv();
   
        try {
            ShowInfo("");
            ShowMessage(error.get_message());
            ShowInfo(error.get_message());

            callNextInput();

        } catch (e) {
            alert(e.description);
        }

    }

    function onSaveSucceed(result) {
        ShowInfo("");
        endWaitingCoverDiv();         
        try {

            if (result == null) {
            
                
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if ((result.length == 1) && (result[0] == SUCCESSRET)) {
            document.getElementById("<%=lblCustSn.ClientID %>").innerHTML = "";
            document.getElementById("<%=lbl_FrameSn.ClientID %>").innerHTML = "";
            document.getElementById("<%=lbl_DoorSn.ClientID %>").innerHTML = "";
                 ShowSuccessfulInfo(true, "Success!");
            }
            else {
               var content1 = result[0];
                ShowMessage(content1);
                ShowInfo(content1);
            }
            resetAll();
        } catch (e) {
          
            alert(e.description);
        }

    }

   

 
    function callNextInput() 
    {
        inputObj.focus();
        getCommonInputObject().value = "";
        getAvailableData("processDataEntry"); 
        
    }

    window.onbeforeunload = function() {
        ExitPage();
    } 

    function ExitPage() 
    {

        if (custsn != "") {
            WebServiceCheckDoorSn.Cancel(custsn);
            custsn = "";
        }
    }

    function resetAll() 
    {

        document.getElementById("<%=lblCustSn.ClientID %>").value = "";
        document.getElementById("<%=lbl_DoorSn.ClientID %>").value = "";
        document.getElementById("<%=lbl_FrameSn.ClientID %>").value = "";
        custsn = "";
        custsn2 = "";
        doorSn = "";
        needInputDoorSn = false;
        needInputFrameSn = false;
//        document.getElementById("lbdoor").style.visibility = "hidden";
//        document.getElementById("lblframe").style.visibility = "hidden";
        callNextInput();
    }

    function ResetPage() 
    {
        ExitPage();
        resetAll();
    }


</script>
    
        
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceCheckDoorSn.asmx" />
            </Services>
        </asp:ScriptManager>
         
        <center>
            
        <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">
     <tr>
                    <td  align="left">
                        <asp:Label ID="lblPdline" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td  align="left" colspan="5" >
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr> 
         
            
      

            <tr>
                <td align="left" >
                    <asp:Label ID="lblCustomerSN" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td align="left" >
                       
                            <asp:Label ID="lblCustSn" runat="server" Width="120px" CssClass="iMes_label_11pt"></asp:Label>
                 
                </td>
                 <td class="style1" align="left">
                 <div id="lbdoor">
                 <asp:Label ID="lblDoorSn" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                 </div>
                 </td>
                 <td>
                  <asp:Label ID="lbl_DoorSn" runat="server" Width="200px" CssClass="iMes_label_11pt"></asp:Label>
                 </td>
                          <td class="style2" align="left">
                            <div id="lblframe">
                             <asp:Label ID="lblFrameSn" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
                          </div>
                          </td>
                         <td>
                          <asp:Label ID="lbl_FrameSn" runat="server" Width="200px" CssClass="iMes_label_11pt"></asp:Label>
                         </td>
            </tr>   
               
          
         <tr >
                <td style="width:10%" align="left" >
                    <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td align="left" colspan=5 >
                 
                              <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="99%" IsClear="true" IsPaste="true" />
                    
                </td>     
            </tr>
          
  
        
        </table>
        
        </center>
    </div>
    
</asp:Content>

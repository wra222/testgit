<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ProductInfo.aspx.cs" Inherits="Query_FA_ProductInfo" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
<script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
<script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>

<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >        
    </asp:ScriptManager>
     <center >
 
 <table border="0" width="100%"  >                    
    <tr>               
        <td width ="23%">
              <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
        </td>                
        <td align ="left">                    
            <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
      
        </td> 
    </tr>
    <tr>               
        <td width ="23%">
              <asp:Label ID="lblProductID" runat="server" Text="Product ID / Customer SN /CT SN /MB SN/MB CT/MAC:" CssClass="iMes_label_13pt"></asp:Label>           
        </td>
       <td align ="left">            
            <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" ReplaceRegularExpression="" Width="80%" IsClear="true" IsPaste="true"/>            
            <input type="hidden" id="hidProduct" runat="server" />            
            <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="beginWaitingCoverDiv();" style="width: 100px; display: none"  >Query</button>
        </td>        
    </tr>    
 </table>
 
   <fieldset id="grpCarton" style="border: thin solid #000000; width: 98%;">           
        <legend align ="left" style ="height :20px" >
        <asp:Label ID="lblUnprintTitle" runat="server" Text="Product Status" CssClass="iMes_label_13pt"></asp:Label></legend>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                <ContentTemplate>                  
                    <iMES:GridViewExt ID="gvFAStatus" runat="server" AutoGenerateColumns="true" GvExtHeight="70px" 
                        Width="100%" GvExtWidth="98%" Height="1px" style="top: 144px; left: 28px">
                    </iMES:GridViewExt>
                </ContentTemplate>
                 <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick"  />                                
                </Triggers>
            </asp:UpdatePanel>
    </fieldset> 
    
    
     <fieldset id="Fieldset3"  style="border: thin solid #000000;  width: 98%; height: 120px;">            
        <legend align ="left" style ="height :20px" >
        <asp:Label ID="Label3" runat="server" CssClass="iMes_label_13pt" Text="Product Repair"></asp:Label></legend>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                <ContentTemplate>                  
                    <iMES:GridViewExt ID="gvFARepair" runat="server" 
                        AutoGenerateColumns="true" Width="98%" 
                    GvExtWidth="100%" GvExtHeight="100px" Height="1px">
                    </iMES:GridViewExt>
                </ContentTemplate>   
                 <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick"  />                                
                </Triggers>
            </asp:UpdatePanel>
    </fieldset>
    
    
    <fieldset id="Fieldset2" style="border: thin solid #000000; width: 98%; height: 300px;">
        <legend align="left" style="height: 20px">
            <asp:Label ID="Label2" runat="server" CssClass="iMes_label_13pt" Text="Product History"></asp:Label></legend>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <iMES:GridViewExt ID="gvFAHistory" runat="server" AutoGenerateColumns="true" Width="98%"
                    GvExtWidth="100%" GvExtHeight="260px" Height="1px">
                </iMES:GridViewExt>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
    </fieldset>
    
    <a name="info"></a>         
     <table border="0" width="99%"  >                    
        <tr>   
            <td width="30%">
                 <fieldset id="Fieldset1"  style="border: thin solid #000000; width: 98%; height: 150px;">           
                    <legend align ="left" style ="height :20px" >
                    <asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt" Text="Product Next Station"></asp:Label></legend>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                            <ContentTemplate>                  
                                <iMES:GridViewExt ID="gvFANextStation" runat="server" 
                                    AutoGenerateColumns="true" Width="98%" 
                                GvExtWidth="100%" GvExtHeight="100px" Height="1px">
                                </iMES:GridViewExt>
                            </ContentTemplate>   
                             <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick"  />                                
                            </Triggers>
                        </asp:UpdatePanel>
                </fieldset>
            </td>
            <td width="70%">
                <fieldset id="Fieldset4"  style="border: thin solid #000000; width: 98%; height: 150px;">
                    <legend align ="left" style ="height :20px" >
                    <asp:Label ID="Label4" runat="server" CssClass="iMes_label_13pt" Text="Product Info"></asp:Label></legend>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                            <ContentTemplate>                  
                                <iMES:GridViewExt ID="gvFAInfo" runat="server" AutoGenerateColumns="true" Width="98%" 
                                GvExtWidth="100%" GvExtHeight="120px" Height="1px">
                                </iMES:GridViewExt>
                            </ContentTemplate>   
                             <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick"  />                                
                            </Triggers>
                        </asp:UpdatePanel>
                </fieldset>                        
            </td>
        </tr>
     </table>
     <a name="part"></a>    
     <fieldset id="Fieldset5" style="border: thin solid #000000; width: 98%; height: 180px;">
        <legend align="left" style="height: 20px">
            <asp:Label ID="Label5" runat="server" CssClass="iMes_label_13pt" Text="Product Part & Pizza Part"></asp:Label></legend>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <iMES:GridViewExt ID="gvFAPart" runat="server" AutoGenerateColumns="true" Width="98%"
                    GvExtWidth="100%" GvExtHeight="150px" Height="1px" 
                    style="top: 21px; left: 2px">
                </iMES:GridViewExt>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
    </fieldset>
    
     <a name="unpack"></a>    
     <fieldset id="Fieldset6" style="border: thin solid #000000; width: 98%; height: 180px;">
        <legend align="left" style="height: 20px">
            <asp:Label ID="Label6" runat="server" CssClass="iMes_label_13pt" Text="Product Unpack"></asp:Label></legend>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <iMES:GridViewExt ID="gvFAUnpack" runat="server" AutoGenerateColumns="true" Width="98%"
                    GvExtWidth="100%" GvExtHeight="150px" Height="1px" 
                    style="top: 26px; left: -1px">
                </iMES:GridViewExt>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
    </fieldset>
    
     <a name="change"></a>    
     <fieldset id="Fieldset7" style="border: thin solid #000000; width: 98%; height: 180px;">
        <legend align="left" style="height: 20px">
            <asp:Label ID="Label7" runat="server" CssClass="iMes_label_13pt" Text="Change Infomation"></asp:Label></legend>
        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <iMES:GridViewExt ID="gvFAChange" runat="server" AutoGenerateColumns="true" Width="98%"
                    GvExtWidth="100%" GvExtHeight="150px" Height="1px" 
                    style="top: 26px; left: -1px">
                </iMES:GridViewExt>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
    </fieldset>
     
     
     <a name="ITCND"></a>    
     <fieldset id="Fieldset8" style="border: thin solid #000000; width: 98%; height: 180px;">
        <legend align="left" style="height: 20px">
            <asp:Label ID="Label8" runat="server" CssClass="iMes_label_13pt" Text="ITCND Infomation"></asp:Label></legend>
        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <iMES:GridViewExt ID="gvFAITCND" runat="server" AutoGenerateColumns="true" Width="98%"
                    GvExtWidth="100%" GvExtHeight="150px" Height="1px" 
                    style="top: 26px; left: -1px">
                </iMES:GridViewExt>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
    </fieldset>
    
     <a name="Cleanroom ProductPart"></a>    
     <fieldset id="Fieldset9" style="border: thin solid #000000; width: 98%; height: 180px;">
        <legend align="left" style="height: 20px">
            <asp:Label ID="Label9" runat="server" CssClass="iMes_label_13pt" Text="Cleanroom Product Part"></asp:Label></legend>
        <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <iMES:GridViewExt ID="gvCRPart" runat="server" AutoGenerateColumns="true" Width="98%"
                    GvExtWidth="100%" GvExtHeight="150px" Height="1px" 
                    style="top: 26px; left: -1px">
                </iMES:GridViewExt>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
    </fieldset>
    
    <a name="Cleanroom ProductLog"></a>    
     <fieldset id="Fieldset10" style="border: thin solid #000000; width: 98%; height: 180px;">
        <legend align="left" style="height: 20px">
            <asp:Label ID="Label10" runat="server" CssClass="iMes_label_13pt" Text="Cleanroom Product Log"></asp:Label></legend>
        <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <iMES:GridViewExt ID="gvCRLog" runat="server" AutoGenerateColumns="true" Width="98%"
                    GvExtWidth="100%" GvExtHeight="150px" Height="1px" 
                    style="top: 26px; left: -1px">
                </iMES:GridViewExt>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
    </fieldset>
    
     <br />
     
   </center>
    </div>
 
    
<script  language="javascript">

    var inputObj;
    var ProductStatus_DEFAULT_ROWS = 1;
    var ProductNextStation_DEFAULT_ROWS = 1;
    var ProductHistory_DEFAULT_ROWS = 1;
    var ProductRepair_DEFAULT_ROWS = 1;
    var ProductInfo_DEFAULT_ROWS = 1;
    var ProductPart_DEFAULT_ROWS = 1;
    var ProductUnpack_DEFAULT_ROWS = 1;
    var ProductChange_DEFAULT_ROWS = 1;
    var ProductITCND_DEFAULT_ROWS = 1;
    var ProductCRPart_DEFAULT_ROWS = 1;
    var ProductCRLog_DEFAULT_ROWS = 1;

    window.onload = function() {
        inputObj = getCommonInputObject();
        getAvailableData("processFun");
    };

    function processFun(backData) {
        ShowInfo("");
        beginWaitingCoverDiv();
        //Update by Dean 20110819 Remark 原因，因加入MBSN 的查詢條件,舊FIS的ProductID為10碼，剛好與MBSN一樣的長度，故若輸入舊FIS資料的話，請User自動刪掉最後一碼
        /*if (backData.toString().trim().length == 10) { //舊Fis 的ProductID為10碼
            backData = SubStringSN(backData, "ProdId");
        }*/
        document.getElementById("<%=hidProduct.ClientID %>").value = backData;
        document.getElementById("<%=btnQuery.ClientID%>").click();
    }

    function initPage() {
        clearData();
        inputObj.value = "";
        getAvailableData("processFun");
        inputObj.focus();
    }

    function setCommonFocus() {
        endWaitingCoverDiv();
        inputObj.focus();
        inputObj.select();
        window.onload();
    }

    function ExitPage() { 
    }

    function ResetPage() {
        initPage();
        ShowInfo("");
        ClearGvExtTable(document.getElementById("<%=gvFAStatus.ClientID %>"), ProductStatus_DEFAULT_ROWS);
        ClearGvExtTable(document.getElementById("<%=gvFANextStation.ClientID %>"), ProductNextStation_DEFAULT_ROWS);
        ClearGvExtTable(document.getElementById("<%=gvFAHistory.ClientID %>"), ProductHistory_DEFAULT_ROWS);
        ClearGvExtTable(document.getElementById("<%=gvFARepair.ClientID %>"), ProductRepair_DEFAULT_ROWS);
        ClearGvExtTable(document.getElementById("<%=gvFAInfo.ClientID %>"), ProductInfo_DEFAULT_ROWS);
        ClearGvExtTable(document.getElementById("<%=gvFAPart.ClientID %>"), ProductPart_DEFAULT_ROWS);
        ClearGvExtTable(document.getElementById("<%=gvFAUnpack.ClientID %>"), ProductUnpack_DEFAULT_ROWS);
        ClearGvExtTable(document.getElementById("<%=gvFAChange.ClientID %>"), ProductChange_DEFAULT_ROWS);
        ClearGvExtTable(document.getElementById("<%=gvFAITCND.ClientID %>"), ProductITCND_DEFAULT_ROWS);
        ClearGvExtTable(document.getElementById("<%=gvCRPart.ClientID %>"), ProductCRPart_DEFAULT_ROWS);
        ClearGvExtTable(document.getElementById("<%=gvCRLog.ClientID %>"), ProductCRLog_DEFAULT_ROWS);

    }             
      
function btnQuery_onclick() {

}

</script>
</asp:Content>


<%@ Page Title="PoWIPTracking" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="PoWIPTracking.aspx.cs" Inherits="Query_FA_PoWIPTracking" EnableEventValidation="false"%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 

    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
<script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
 <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
<script src="../../js/jscal2.js"></script>
<script src="../../js/lang/en.js"></script>

<script src="../../js/easyTooltip.js"></script>
<link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
<link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
<link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
<script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
<script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
<script src="../../js/jscal2.js"></script>
<script src="../../js/lang/en.js"></script>
    
<script src="../../js/jquery.dateFormat-1.0.js"></script>    
    <script type="text/javascript" src="../../js/assets/prettify.js"></script>
    <script type="text/javascript" src="../../js/jquery.multiselect.js"></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js"></script>     
        
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">

    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />
    <link rel="stylesheet" type="text/css" href="../../css/assets/style.css" />
    <link rel="stylesheet" type="text/css" href="../../css/assets/prettify.css" />  
    <script type="text/javascript" src="../../js/wz_tooltip.js"></script>

 <script type="text/javascript">
     function load() {
         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
       
     }

     function EndRequestHandler(sender, args) {

         $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select Line ' }).multiselectfilter();

        
     };
 </script>
 
 
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>

<center>
 <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Wip Tracking" CssClass="iMes_label_13pt"></asp:Label></legend> 
      
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
            
                <td width ="10%">
            
                   
          <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
            
                   
                </td>                
                <td align="left" colspan="2" width ="30%">                        
                  
            <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                  
                   
                  
                </td>   
                <td>   <asp:Label ID="lblProcess" runat="server" Text="製程:" CssClass="iMes_label_13pt"></asp:Label>        
                
                </td>       
               <td align="left">
                   <asp:RadioButtonList ID="radProcess" runat="server" 
                       RepeatDirection="Horizontal">
                       <asp:ListItem Selected="True" Value="ALL">ALL</asp:ListItem>
                       <asp:ListItem>FA</asp:ListItem>
                       <asp:ListItem>PAK</asp:ListItem>
                   </asp:RadioButtonList>
            
                   
            
               </td>
               <td>
               <asp:Label ID="lblGrType" runat="server" Text="分類:" CssClass="iMes_label_13pt"></asp:Label> 
             <asp:UpdatePanel ID="UpdatePanel4" runat="server" RenderMode="Inline" 
                       UpdateMode="Conditional">
    <ContentTemplate>
      <asp:DropDownList ID="droGroupType" runat="server"  Width="100px" 
                       AutoPostBack="True" onselectedindexchanged="droGroupType_SelectedIndexChanged">
                        <asp:ListItem Selected="True">Model</asp:ListItem>
                        <asp:ListItem>Line</asp:ListItem>
                        <asp:ListItem>Model+Line</asp:ListItem>
                        <asp:ListItem>ALL</asp:ListItem>
                    </asp:DropDownList>
    </ContentTemplate>
                 <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="droGroupType" 
                         EventName="SelectedIndexChanged" />
                 </Triggers>
    </asp:UpdatePanel>
                  
               </td>
              
               <td  width ="5%">
                     
                    <asp:Label ID="lblLine" runat="server" Text="Line:" 
                       CssClass="iMes_label_13pt"></asp:Label>
                     
               </td>
               <td  width ="30%">
               
                   <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                       <ContentTemplate>
                           <asp:ListBox ID="lboxPdLine" runat="server" 
    SelectionMode="Multiple" Height="95%" 
                            Width="250px" CssClass="CheckBoxList"></asp:ListBox>
                            <input id="Button1" type="button" value="Line"   onclick="ChangeLine()" />
                       </ContentTemplate>
                       <Triggers>
                           <asp:AsyncPostBackTrigger ControlID="btnChangeLine" EventName="ServerClick" />
                       </Triggers>
                   </asp:UpdatePanel>
                  
               </td>
            </tr>
            <tr>
               <td width ="10%">
                   <asp:Label ID="lblModel" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
               <td align= "left" width ="60%" colspan="5">
                <input ID="Radio1" type="radio" onclick="SetDNModel()"
            style="height: 21px; width: 23px" name="gr1"/>
                 <asp:Label ID="lblShipDate" runat="server" Text="Ship Date:" CssClass="iMes_label_13pt"></asp:Label>
                   <asp:TextBox ID="txtShipDate" runat="server" Width="157px" Height="20px"></asp:TextBox>         
                   &nbsp;<input ID="Radio2" type="radio"  onclick="SetInputModel()" 
            style="height: 21px; width: 23px" name="gr1"  />
                   <asp:Label ID="lblInput" runat="server" Text="Input:" CssClass="iMes_label_13pt"></asp:Label>                                       
                  
                   <asp:TextBox ID="txtModel" runat="server" Width="199px" Height="17px" 
                       Enabled="false" BackColor="#EEEEEE"></asp:TextBox>
                   <input id="BtnBrowse" type="button" value="Input" disabled="disabled"  onclick="UploadModelList()" /></td>     
              
            
             
                       
                   
                   <td>
                       <input id="chkDN"  type="checkbox" checked="checked"   onclick="SetDNDate()" /><asp:Label ID="Label1" runat="server" Text="DN" CssClass="iMes_label_13pt"></asp:Label>
            
                   <asp:Label ID="lblPoNo" runat="server" Text="PoNo:" CssClass="iMes_label_13pt" 
                           Visible="False"></asp:Label> 
            
                   </td>    
                   <td aligh="left">
            <asp:TextBox ID="txtDNDate" runat="server" Width="157px" Height="20px"></asp:TextBox>
                   <asp:TextBox ID="txtPoNo" runat="server" Width="72px" 
        Height="20px" Visible="False"></asp:TextBox>
            
                   </td>
            </tr>
                 <tr>
                 <td>
                   <asp:Label ID="lblFamily" runat="server" Text="Family:" CssClass="iMes_label_13pt"></asp:Label>
                 </td>
                 <td align="left">
                                 <asp:DropDownList ID="ddlFamily" runat="server" Width="180px"></asp:DropDownList>
                 </td>
                 <td colspan="4">    
                
                   <asp:Label ID="lblTotalQty" runat="server" Text="TotalQty:" 
                       CssClass="iMes_label_13pt" Font-Bold="True" ForeColor="Red"></asp:Label> 
                    <asp:Label ID="lblTotalQtyCount" runat="server" CssClass="iMes_label_13pt"></asp:Label> 
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                
                    <asp:Label ID="lblActualQty" runat="server" Text="ActualQty:" CssClass="iMes_label_13pt"></asp:Label> 
                     <asp:Label ID="lblActualQtyCount" runat="server" CssClass="iMes_label_13pt"></asp:Label> 
                
                     </td>
                  <td colspan="2" align="right">
                       <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px;display: none; ">Export</button> 
                                               <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="beginWaitingCoverDiv();" style="width: 100px"  >Query</button>
                  </td>
                 </tr>
        
         </table>
           
</fieldset> 
    <button id="btnQueryDetail" runat="server"  onserverclick="QueryDetailClick" style="display: none">QueryDetail</button>   
     <button id="btnChangeLine" runat="server"  onserverclick="ChangeLine_S" style="display: none">ChangeLine</button>  
             <asp:HiddenField ID="hfStation" runat="server" />   
    
    <asp:HiddenField ID="hfLine" runat="server" />
    <asp:HiddenField ID="hfModel" runat="server" />
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" 
        UpdateMode="Conditional">
    <ContentTemplate>
    <br />
      <input type="hidden" id="htmlHidden" runat="server" 
            style="color: #FF0000; font-size: medium; font-weight: bold" />
       <iMES:GridViewExt ID="gvResult" runat="server" AutoGenerateColumns="true" GvExtHeight="200px" 
            Width="98%" GvExtWidth="98%" Height="1px" 
            onrowdatabound="gvResult_RowDataBound" style="top: 0px; left: 0px">            
           <HeaderStyle Font-Size="Smaller" Width="50px" />
        </iMES:GridViewExt>       
          <asp:HiddenField ID="hidOriLine" runat="server"  />     
          <asp:HiddenField ID="hidOriDNDate" runat="server"  />     
          <asp:HiddenField ID="hidOriFamily" runat="server"  />    
          <asp:HiddenField ID="hidDNDate" runat="server"    />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />                
        <asp:AsyncPostBackTrigger ControlID="btnQueryDetail" EventName="ServerClick" />
    </Triggers>
  </asp:UpdatePanel>
  
  <br />

  
   <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
    <ContentTemplate>      
       <iMES:GridViewExt ID="gvStationDetail" runat="server" AutoGenerateColumns="true" GvExtHeight="300px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            >            
           <HeaderStyle Font-Size="Smaller" Width="50px" />
        </iMES:GridViewExt>                        
    </ContentTemplate>
    <Triggers>                 
         <asp:AsyncPostBackTrigger ControlID="btnQueryDetail" EventName="ServerClick" />
         
    </Triggers>  
  </asp:UpdatePanel>
  <asp:HiddenField ID="hidModelType" runat="server" Value="1" />
   <asp:HiddenField ID="hidModelList" runat="server"  />

<asp:HiddenField ID="hidOriModelList" runat="server"  />
<asp:HiddenField ID="hidOriModelType" runat="server"  />
<asp:HiddenField ID="hidOriDate" runat="server"  />
<asp:HiddenField ID="hidx" runat="server" Value="1"  />

</center>

<script type="text/javascript">    //<![CDATA[
    Calendar.setup({
        inputField: "<%=txtShipDate.ClientID%>",
        trigger: "<%=txtShipDate.ClientID%>",
        onSelect: function() { this.hide(); SetDNDateValue() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });
    Calendar.setup({
        inputField: "<%=txtDNDate.ClientID%>",
        trigger: "<%=txtDNDate.ClientID%>",
        onSelect: function() { this.hide(); SetDNDateValue2(); },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });
 
    //]]></script>

<script language="javascript" type="text/javascript">
    EndRequestHandler();
    iniRad();
    function iniRad() {
        var radioObj1 = document.getElementById("Radio1");
        var radioObj2 = document.getElementById("Radio2");
        if (document.getElementById("<%=hidModelType.ClientID %>").value == "1")
         {
            radioObj1.checked = true;
          
          }
        else {
           
        document.getElementById("<%=txtShipDate.ClientID %>").value = document.getElementById("<%=hidx.ClientID %>").value;
        radioObj2.checked = true;
        var obj2 = document.getElementById("<%=txtModel.ClientID %>");
        obj2.style.background = '#E0FCC9';
        obj2.disabled = false;
        var obj1 = document.getElementById("<%=txtShipDate.ClientID %>");
        obj1.style.background = '#eeeeee';
        obj1.disabled = true;
        document.getElementById("BtnBrowse").disabled = false;
        document.getElementById("<%=hidModelType.ClientID %>").value = "2";
    }
    
    }
    function ShowTotal(TotalQtyCount,ActualQtyCount) {
        document.getElementById("<%=lblTotalQtyCount.ClientID %>").innerText = TotalQtyCount;
        document.getElementById("<%=lblActualQtyCount.ClientID %>").innerText = ActualQtyCount;
    }

    function SelectDetail(station,line,model) {
        beginWaitingCoverDiv();
        document.getElementById("<%=hfStation.ClientID%>").value = station;
        document.getElementById("<%=hfLine.ClientID%>").value = line;
        document.getElementById("<%=hfModel.ClientID%>").value = model;
        document.getElementById("<%=hidOriDate.ClientID%>").value = document.getElementById("<%=txtShipDate.ClientID%>").value
        if (document.getElementById("<%=hidModelType.ClientID %>").value == "2")
        { document.getElementById("<%=hidOriDate.ClientID%>").value = "";
       
        document.getElementById("<%=hidOriModelList.ClientID%>").value = document.getElementById("<%=hidModelList.ClientID %>").value }
      
        document.getElementById("<%=btnQueryDetail.ClientID%>").click();        
    }
    function SetDNModel() {
        var radioObj = document.getElementById("Radio1")
        if (radioObj.checked) {
            var obj1 = document.getElementById("<%=txtModel.ClientID %>");
            obj1.style.background = '#eeeeee';
            obj1.disabled = true

            var obj2 = document.getElementById("<%=txtShipDate.ClientID %>");
            obj2.style.background = '#E0FCC9';
            obj2.disabled = false
            document.getElementById("BtnBrowse").disabled = true;
            document.getElementById("<%=hidModelType.ClientID %>").value = "1";
            
        }

    } //txtShipDate
    function SetInputModel() {
        var radioObj = document.getElementById("Radio2")
        if (radioObj.checked) {
            var obj1 = document.getElementById("<%=txtShipDate.ClientID %>");
            obj1.style.background = '#eeeeee';
            obj1.disabled = true

            var obj2 = document.getElementById("<%=txtModel.ClientID %>");
            obj2.style.background = '#E0FCC9';
            obj2.disabled = false
            document.getElementById("<%=hidx.ClientID %>").value = document.getElementById("<%=txtShipDate.ClientID %>").value;
            document.getElementById("BtnBrowse").disabled = false;
            document.getElementById("<%=hidModelType.ClientID %>").value = "2";
        }

    }
    function UploadModelList() {
      
        SetChxDN(1);
        var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
        var saveasUrl = "../../UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelList.ClientID %>").value;
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
    function ChangeLine() {
        beginWaitingCoverDiv();
        document.getElementById("<%=btnChangeLine.ClientID%>").click();

    }
    function Reset() {
     
        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select Line ' }).multiselectfilter();
    }
    function SetDNDate() {
        var obj = document.getElementById("chkDN");
    var obj1 = document.getElementById("<%=txtDNDate.ClientID %>");
    if (obj.checked == true) {

        obj1.style.background = '#E0FCC9';
        obj1.disabled = false
        document.getElementById("<%=hidDNDate.ClientID %>").value = document.getElementById("<%=txtDNDate.ClientID %>").value;
     }
    else {
        obj1.style.background = '#eeeeee';
        obj1.disabled = true
        document.getElementById("<%=hidDNDate.ClientID %>").value = "";
    }



}
function SetDNDateValue() {
    document.getElementById("<%=txtDNDate.ClientID %>").value = document.getElementById("<%=txtShipDate.ClientID %>").value;
   
    var obj = document.getElementById("chkDN");
    if (obj.checked == true)
    { document.getElementById("<%=hidDNDate.ClientID %>").value = document.getElementById("<%=txtDNDate.ClientID %>").value; }
    else
    { document.getElementById("<%=hidDNDate.ClientID %>").value = ""; }
}
function SetDNDateValue2() {
   
    document.getElementById("<%=hidDNDate.ClientID %>").value = document.getElementById("<%=txtDNDate.ClientID %>").value;

}
function SetChxDN(bo) {
    var obj = document.getElementById("chkDN");
    var obj1 = document.getElementById("<%=txtDNDate.ClientID %>");
   
    if (bo == 0) {
        obj.disabled = true;
        obj1.disabled = true;
        obj1.style.background = '#eeeeee';
     }
    else {
        obj.disabled = false;
        if (obj.checked == true) {
            obj1.disabled = false;
            obj1.style.background = '#E0FCC9';
        }
        else {
            obj1.disabled = true;
            obj1.style.background = '#eeeeee';
        }
       
    }
 
}
   </script>

<style type="text/css">
  
.rowclient
{
    cursor : pointer;
}

    .style1
    {
        width: 180px;
    }

    </style>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="WipTrackingByDN.aspx.cs" Inherits="WipTrackingByDN" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
   
 
     <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
<script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
 <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
<script src="../../js/jscal2.js"></script>
<script src="../../js/lang/cn.js"></script>




   
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
 <style type="text/css">
    .querycell
    {
        background-color: yellow;
     
    }
    .querycell:hover
    {
        background-color: #8AF2E7;
     
    }
     tr.clicked
    {
        background-color: white; 
    }
    .clicked
    {
        background-color: #8AF2E7;
    }
  .row1
    {
  	  background-color: #A0CFEC;
  	  }
    .row2
    {
      background-color: #CFECEC;
     }
     .row2:hover, .row1:hover
{
	background-color: white;
}
</style>    
  <script type="text/javascript">
  

  function load() {
      Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

  }

  function EndRequestHandler(sender, args) {

      $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select Line ' }).multiselectfilter();


  }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
    <fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="Wip Tracking" CssClass="iMes_label_13pt"></asp:Label></legend> 
      
         <table border="0" width="100%" style="font-family: Tahoma">                    
            <tr>
            
                <td width ="10%">
            
                   
          <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
            
                   
                </td>                
                <td align="left" colspan="2" width ="30%">                        
                  
            <imesquery:cmbdbtype ID="CmbDBType" runat="server" />
                  
                   
                  
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
       
      <asp:DropDownList ID="droGroupType" runat="server"  Width="100px" 
                     >
                        <asp:ListItem Selected="True">Model</asp:ListItem>
                        <asp:ListItem>Model+Line</asp:ListItem>
                    </asp:DropDownList>
    
                  
               </td>
              
               <td  width ="5%">
                     
                    <asp:Label ID="lblLine" runat="server" Text="Line:" 
                       CssClass="iMes_label_13pt"></asp:Label>
                     
               </td>
               <td  width ="30%">
               
                  
                        
                   
                   
                  
                          
                                   <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                       <ContentTemplate>
                                           <asp:ListBox ID="lboxPdLine" runat="server" CssClass="CheckBoxList" 
                                               Height="95%" SelectionMode="Multiple" Width="250px"></asp:ListBox>
                                           <asp:Button ID="btnChangeLine2" 
    runat="server" Text="Line"   onclick="ChangeLine_S"  OnClientClick=" ShowWait()" Visible="True" Width="40px" />
                                       </ContentTemplate>
                                       <Triggers>
                                           <asp:AsyncPostBackTrigger ControlID="btnChangeLine2" EventName="Click" />
                                       </Triggers>
                                   </asp:UpdatePanel>
                                
                   
                
                  
               </td>
            </tr>
            <tr>
               <td width ="10%">
                   <asp:Label ID="lblModel" runat="server" Text="Ship Date:" CssClass="iMes_label_13pt"></asp:Label>
               </td>
               <td align= "left" width ="60%" colspan="5">
                   
        
                 
                   <asp:TextBox ID="txtShipDate" runat="server" Width="157px" Height="20px"></asp:TextBox>         
                   &nbsp;
&nbsp;<asp:Label ID="lblInput" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>                                       
                  
                   <asp:TextBox ID="txtModel" runat="server" Width="199px" Height="17px"></asp:TextBox>
                   <input id="BtnBrowse" type="button" value="Input"   onclick="UploadModelList()" /></td>     
              
            
             
                       
                   
                   <td>
                       &nbsp;</td>    
                   <td aligh="left">
                       &nbsp;</td>
            </tr>
                 <tr>
                 <td>
                   <asp:Label ID="lblFamily" runat="server" Text="Family:" 
                         CssClass="iMes_label_13pt" Visible="False"></asp:Label>
                 </td>
                 <td align="left">
                                 <asp:DropDownList ID="ddlFamily" runat="server" Width="180px" Visible="False"></asp:DropDownList>
                 </td>
                 <td colspan="4">    
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>
         
                
                   <asp:Label ID="lblTotalQty" runat="server" Text="TotalQty:" 
                       CssClass="iMes_label_13pt" Font-Bold="True" ForeColor="Red"></asp:Label> 
                    <asp:Label ID="lblTotalQtyCount" runat="server" CssClass="iMes_label_13pt"></asp:Label> 
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                
                    <asp:Label ID="lblActualQty" runat="server" Text="ActualQty(Pass 85) :" CssClass="iMes_label_13pt"></asp:Label> 
                     <asp:Label ID="lblActualQtyCount" runat="server" CssClass="iMes_label_13pt"></asp:Label> 
          </ContentTemplate>
         <Triggers>
             <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
         </Triggers>
     </asp:UpdatePanel>       
                     </td>
                  <td colspan="2" align="right">
                         <button id="btnExport"  runat="server" onserverclick="btnExport_Click" 
                        style="width: 100px;display: none; ">Export</button> 
                                               <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click"  onclick="beginWaitingCoverDiv();"  style="width: 100px"  >Query</button>
                  </td>
                 </tr>
        
         </table>
           <br />
</fieldset>
<br />
     <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" RenderMode="Inline">
         <ContentTemplate>
                     <iMES:GridViewExt  ID="gvResult" runat="server" onrowdatabound="gvResult_RowDataBound" GvExtHeight="300px" 
            Width="98%" GvExtWidth="98%" Height="1px"  OnGvExtRowClick="ChangeRowColor()"   >
                 
         </iMES:GridViewExt>
             <br />
         </ContentTemplate>
         <Triggers>
             <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
             
         </Triggers>
     </asp:UpdatePanel>
     
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
    <ContentTemplate>      
       <iMES:GridViewExt ID="gvStationDetail" runat="server" AutoGenerateColumns="true" GvExtHeight="40%" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
            >            
           <HeaderStyle Font-Size="Smaller" Width="50px" />
        </iMES:GridViewExt>                        
    </ContentTemplate>
    <Triggers>                 
         <asp:AsyncPostBackTrigger ControlID="btnQueryDetail" EventName="ServerClick" />
         
    </Triggers>  
  </asp:UpdatePanel>
        <asp:HiddenField ID="hfStation" runat="server" />   
    
    <asp:HiddenField ID="hfLine" runat="server" />
    <asp:HiddenField ID="hfModel" runat="server" />
     <asp:HiddenField ID="hidModelType" runat="server" Value="1" />
   <asp:HiddenField ID="hidModelList" runat="server"  />

<asp:HiddenField ID="hidOriModelList" runat="server"  />
<asp:HiddenField ID="hidOriModelType" runat="server"  />
<asp:HiddenField ID="hidOriDate" runat="server"  />
<asp:HiddenField ID="hidOriLine" runat="server"  />     
          <asp:HiddenField ID="hidOriDNDate" runat="server"  />     
          <asp:HiddenField ID="hidOriFamily" runat="server"  />    
          <asp:HiddenField ID="hidDNDate" runat="server"    />
          <asp:HiddenField ID="hidSelectDN" runat="server"    />
    
       <button id="btnChangeLine" runat="server"  onserverclick="ChangeLine_S" style="display: none">ChangeLine</button> 
        <button id="btnQueryDetail" runat="server"  onserverclick="QueryDetailClick" style="display: none">QueryDetail</button>   
  <div id="divWait" oselectid="" align="center" style="cursor: wait; filter: Chroma(Color=skyblue);
        background-color: skyblue; display: none; top: 0; width: 100%; height: 100%;
        z-index: 10000; position: absolute">
        <table style="cursor: wait; background-color: #FFFFFF; border: 1px solid #0054B9;
            font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr>
                <td align="center">
                    <img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif" />
                </td>
                <td align="center" style="color: #0054B9; font-size: 10pt; font-weight: bold;">
                    Please wait.....
                </td>
            </tr>
        </table>
    </div>
 
 
      <script type="text/javascript">          //<![CDATA[
          Calendar.setup({
              inputField: "<%=txtShipDate.ClientID%>",
              trigger: "<%=txtShipDate.ClientID%>",
              onSelect: function() { this.hide(); },
              showTime: 24,
              dateFormat: "%Y-%m-%d",
              minuteStep: 1
          });


          //]]></script>
     <script type="text/javascript">
       load();
     
     window.onload = function()
     {
    //   load();
       EndRequestHandler();
     };
     //    load();
       //  EndRequestHandler();
     </script>
    <script type="text/javascript">
        function Reset() {
            load();
   
            
       }
     
//       function SetG() {
//           beginWaitingCoverDiv();
//       }
       function ChangeLine() {
        
            beginWaitingCoverDiv();
            document.getElementById("<%=btnChangeLine.ClientID%>").click();

        }
        function SelectDetail(station, line, model,dn) {
            beginWaitingCoverDiv();
            document.getElementById("<%=hfStation.ClientID%>").value = station;
            document.getElementById("<%=hfLine.ClientID%>").value = line;
            document.getElementById("<%=hfModel.ClientID%>").value = model;
            document.getElementById("<%=hidSelectDN.ClientID%>").value = dn;

            
            document.getElementById("<%=hidOriDate.ClientID%>").value = document.getElementById("<%=txtShipDate.ClientID%>").value
            if (document.getElementById("<%=hidModelType.ClientID %>").value == "2") {
                document.getElementById("<%=hidOriDate.ClientID%>").value = "";

                document.getElementById("<%=hidOriModelList.ClientID%>").value = document.getElementById("<%=hidModelList.ClientID %>").value
            }
            $(".clicked").removeClass("clicked");
            $($(event.srcElement).parent()).addClass("clicked");
            $(event.srcElement).addClass("clicked");
            document.getElementById("<%=btnQueryDetail.ClientID%>").click();
            
        }
        function ChangeRowColor() {
            $(".clicked").removeClass("clicked");
            $($(event.srcElement).parent()).addClass("clicked");
        }
        function UploadModelList() {

        
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
                    if (arr[m].trim() != "") {
                        model = model + arr[m].trim() + ",";
                    }
                }
                model = model.substring(0, model.length - 1)
            }

            return model;
        }
    </script>
</asp:Content>


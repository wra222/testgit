<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="Warehouse1.aspx.cs" Inherits="Query_PAK_Warehouse1" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

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
    .titleSmallData
    {
        font-size: 16px;
         color: #FFF8C6;
    }
    
    
    .querycell
    {
        background-color: yellow;
     
    }
    .querycell:hover
    {
        background-color: Blue;
        cursor:pointer;                 
    }
    
    .querycell.clicked
    {
        background-color: Blue;
    }

    .style5
    {
        height: 14px;
        width: 70%;
    }
    .style6
    {
        width: 30%;
    }

</style>    
  <script type="text/javascript">
        function SelectDetail(MAWB,Status,ShipDate1) {
            //beginWaitingCoverDiv();
            $(".clicked").removeClass("clicked");
            $(event.srcElement.parentNode).addClass("clicked");
            $(event.srcElement).addClass("clicked");
            document.getElementById("<%=hfMAWB.ClientID%>").value = MAWB;
            document.getElementById("<%=hfStatus.ClientID%>").value = Status;
            document.getElementById("<%=hfShipDate1.ClientID%>").value = ShipDate1;           
            document.getElementById("<%=btnQueryDetail1.ClientID%>").click();

        }
    </script>             

        <asp:ScriptManager ID="ScriptManager1" runat="server">   
        </asp:ScriptManager>

 <div>
 <center>
      <div style=" visibility:hidden">
      <iMESQuery:CmbDBType ID="CmbDBType" runat="server"/>
          <asp:Button ID="Button1" runat="server" Text="" style="display:none;" OnClientClick="Button1_Click()"/>
           <button id="btnQueryDetail1" runat="server"  onserverclick="btnQueryDetail_Click" style="display: none"></button>

    </div>   
        <fieldset style="border: solid #000000" style='background-color:#000000'>
            <table border="0" width="100%" style="border-width:thin;">
                <tr>
                    <td class="style5" align="left">
                                    <asp:Label ID="Label1" runat="server" ForeColor="Yellow" Text="F5 Warehouse Dashboard" 
                Font-Size="X-Large"></asp:Label></td>
                 <td>
                       <asp:Label ID="Label3" runat="server" ForeColor="Yellow" Text="" 
                Font-Size="X-Large"></asp:Label></td>
                       
                </tr>
            <tr>                                    
                <td class="style6" align="center">
                                    <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="Welcome to Warehouse" 
                Font-Size="XX-Large"></asp:Label></td>
            </tr>
            <tr>
                <td class="style6">
                    &nbsp;</td>
                <td align="right"> 
         <asp:TextBox ID="txtShipDate" runat="server" Width="6px" Height="6px" ></asp:TextBox> 
                    <asp:Button ID="Button2" runat="server" Height="10px" onclick="Button2_Click" 
                        Text="" Width="10px" />
                </td>
            </tr>
            </table>
        
        </fieldset>
 
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>        
                <iMES:GridViewExt ID="gvQuery" runat="server"
           AutoGenerateColumns="true" GvExtHeight="150px"
                Width="98%" GvExtWidth="98%" Height="100px"
           style="top: 0px; left: 0px" BackColor="Black" Font-Size="28px"
                    onselectedindexchanged="gvQuery_SelectedIndexChanged" >
                    <AlternatingRowStyle Font-Size="X-Large" />
                </iMES:GridViewExt>
                
  <button id="btnQueryDetail" runat="server"  onserverclick="btnQueryDetail_Click" style="display: none"></button>
        <div style="padding: 5px 0 0 0 ">
           <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>      
                     <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="true"  GvExtHeight="200px" 
                            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
                         Visible="false" Font-Size="28px" >            
                         <HeaderStyle Font-Size="smaller" Width="50px" Height="14px" BackColor="Black" />
                     </iMES:GridViewExt>                        
                </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnQueryDetail" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
             <asp:HiddenField ID="hfMAWB" runat="server" 
                onvaluechanged="hfMAWB_ValueChanged" />
             <asp:HiddenField ID="hfStatus" runat="server" />
             <asp:HiddenField ID="hfShipDate1" runat="server" />
                 </div>
               <%-- <asp:LinkButton ID="lbtFreshPage" runat="server" 
                    OnClientClick="return ReloadImesPage();"></asp:LinkButton>--%>
             </ContentTemplate>
              <Triggers>
              </Triggers>
        </asp:UpdatePanel>

                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>        
                <iMES:GridViewExt ID="gvQuery2" runat="server" AutoGenerateColumns="true" GvExtHeight="250px"
                Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" >
                </iMES:GridViewExt>
        </ContentTemplate>
              <Triggers>
              </Triggers>
        </asp:UpdatePanel>

        
        <div style="padding: 5px 0 0 0 ">
           <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>      
                     <iMES:GridViewExt ID="gvStationDetail" runat="server" AutoGenerateColumns="true"  GvExtHeight="200px" 
                            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" Visible="false" >            
                         <HeaderStyle Font-Size="Smaller" Width="50px" Height="14px" />
                     </iMES:GridViewExt>                        
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
        
</center>
    </div>

<script type="text/javascript">
        var inputObj;

        function myrefresh() 
        {
        window.location.href=window.location.href;
        window.location.reload();
        } 
        setTimeout('myrefresh()',300000); //指定30秒刷新一次 
        
        Calendar.setup({
        inputField: "<%=txtShipDate.ClientID%>",
        trigger: "<%=txtShipDate.ClientID%>",//"btnShipDate",
        onSelect: function() { this.hide()},
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1  
        });

//        function go ()
//        {
//            document.getElementById("Button1").click();
//        }

        function bind() 
        {
            //beginWaitingCoverDiv();
            //
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

       

        
    </script>
    <script type="text/javascript">

function btnShipDate_onclick() 
{

}


//function btnShipDate_onclick() {

//}

    </script>
</asp:Content>
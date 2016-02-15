<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="MastereSOPQuery.aspx.cs" Inherits="Query_MastereSOPQuery" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>



<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>        
    <script src="../../js/jscal2.js"></script>
    <script src="../../js/lang/en.js"></script>
        
    <script type="text/javascript" src="../../js/ui.dropdownchecklist.js"></script>
    <script src="../../js/jquery.dateFormat-1.0.js"></script>    
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../../css/ui.dropdownchecklist.themeroller.css">                         
                         
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
                
        </asp:ScriptManager>
 <center>
        <fieldset style="border: solid #000000">
            <legend>Master eSOP Query</legend>
            <table border="0" width="100%" style="border-width:thin;">
            <tr>
                <td>
                    <div style="width: 600px; padding: 10px 0 0 10px; margin: 0 auto ; float: left ; display: none">
                        <asp:Label ID="Label3" runat="server" Text="DBName:"></asp:Label>
                        <iMESQuery:CmbDBType id="CmbDBType" runat="server"  />                                                
                    </div>
                </td>
            </tr>
            <tr>                                    
                <td style=" width: 100%; height: 75px">                
                <div style="float:left; padding: 10px 0 0 10px; margin: 0 auto; width: 391px;">   
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                     <ContentTemplate>                                          
                        <%-- <asp:Label ID="lblPart" runat="server" Text="Part:" Width="70px" CssClass="iMes_label_13pt"></asp:Label>                  
                         <asp:DropDownList ID="ddlPart" runat="server"  Width="200px" AutoPostBack="true"
                             onselectedindexchanged="ddlPart_SelectedIndexChanged">
                         </asp:DropDownList> --%>   
                         &nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:Label ID="lblSn" runat="server" Text="ProID\CustSN\Model:" Width="134px" 
                             CssClass="iMes_label_13pt"></asp:Label>                  
                         <asp:TextBox ID="tbProID" runat="server" 
                             style="margin-left: 3px" Width="160px"></asp:TextBox>   
                     <br />                     
                        <%-- <asp:Label ID="lblModel" runat="server" Text="Model:" Width="70px" CssClass="iMes_label_13pt"></asp:Label>
                         <asp:DropDownList ID="ddlType" runat="server"  Width="200px">
                         </asp:DropDownList> --%>                                         
                    </ContentTemplate>                                     
                   </asp:UpdatePanel> 
                </div>                              
                <div style="float:left; padding: 10px 0 0 10px; height:100%; position: relative">                                   
                   <div style="bottom: 10px; position: absolute ; width: 220px"> 
                    <asp:Button id="btnQuery" runat="server" onserverclick="queryClick" Text="Query" 
                        onclick="btnQuery_Click" />                   
                   </div>                    
                </div>                    
                </td>                    
            </tr>
        </table>
        </fieldset>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="100px"
                Width="98%" Height="1px" Visible="true">
                </iMES:GridViewExt>
                <asp:Panel ID="pl1" runat="server"></asp:Panel>
                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                <!--<asp:Image ID="imgesop" runat="server" Width="400px" Height="400px" />-->
            </ContentTemplate>
        </asp:UpdatePanel>
        
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:LinkButton ID="lbtFreshPage" runat="server" 
                    OnClientClick="return ReloadImesPage();" onclick="lbtFreshPage_Click"></asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
           
</center>
       <asp:HiddenField ID="hidUser" runat="server" />       
       <asp:HiddenField ID="hidprocess" runat="server" />              
       <asp:HiddenField ID="hidsource" runat="server" />              
    </div>


    <script language="javascript" type="text/javascript">
        var inputObj;
        
        window.onload = function() {
            //inputObj = getCommonInputObject();
            //getAvailableData("processFun");
        };
    
        function bind() 
        {
            //beginWaitingCoverDiv();
            //
        }

        function processFun(backData) {
            ShowInfo("");
            beginWaitingCoverDiv();            
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


        
    </script>
       <script type="text/javascript">           //<![CDATA[
               
           
       </script>
</asp:Content>
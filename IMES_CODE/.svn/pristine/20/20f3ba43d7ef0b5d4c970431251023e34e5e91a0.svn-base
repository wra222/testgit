<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="KitFloatLocQuery.aspx.cs" Inherits="Query_KitFloatLocQuery" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>


<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>      
        <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>  
    <script src="../../js/jscal2.js"></script>
    <script src="../../js/lang/en.js"></script>
        
    <script type="text/javascript" src="../../js/ui.dropdownchecklist.js"></script>
    <script type="text/javascript" src="../../js/jquery.dateFormat-1.0.js"></script>        
             
                         
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
                
        </asp:ScriptManager>

        <fieldset style="border: solid #000000">
            <legend>Kitting Float Location Query</legend>
            <br />
             <asp:Label ID="Label3" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
               &nbsp;
               <iMESQuery:CmbDBType id="CmbDBType" runat="server"  />   
            <asp:Label ID="Label1" runat="server" Text="Line : "></asp:Label>
         &nbsp;<asp:DropDownList ID="droLine" runat="server"  
                Width="140px">
            </asp:DropDownList>
&nbsp;
            <asp:Label ID="labModel" runat="server" Text="Model"></asp:Label>
            &nbsp;
            <asp:TextBox ID="txtModel" runat="server" Width="195px"></asp:TextBox>  
        &nbsp;<asp:Button ID="btnQ" runat="server" Text="Query" onclick="btnQ_Click" />
        </fieldset>
        <br />
                           
                <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="500px"
                Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px">
                </iMES:GridViewExt>
        
    </div>


</asp:Content>
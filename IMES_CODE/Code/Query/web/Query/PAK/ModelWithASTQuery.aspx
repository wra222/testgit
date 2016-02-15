<%@ Page Title="ChepPallet" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ModelWithASTQuery.aspx.cs" Inherits="Model_ASTQuery" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server" > 

    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>     
    <script src="../../js/jscal2.js"></script>
    <script src="../../js/lang/en.js"></script>
    
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>
     
  <asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>


<fieldset id="grpCarton" style="border: thin solid #000000;">
    <legend align ="left" style ="height :20px" >
       <asp:Label ID="lblTitle" runat="server" Text="ModelWithASTQuery" CssClass="iMes_label_13pt"></asp:Label></legend> 
    
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"  ></asp:Label>               
                                      
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                

           <asp:FileUpload ID="FileUpload1" runat="server" Width="236px" />
      
&nbsp; 
       <asp:Button ID="btnUpload" runat="server" onclick="btnUpload_Click" OnClientClick="return Check();" Text="UpLoad" />
     
    &nbsp; 
       <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" style=" display:none"      Text="Excel" />
     &nbsp;
   
    <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="labModel" runat="server" Text="Model"></asp:Label>
&nbsp;<asp:TextBox ID="txtModel" runat="server" Width="207px" 
        ></asp:TextBox>
&nbsp;<asp:Button ID="btnQuery" runat="server" onclick="btnQuery_Click" 
        Text="Query" />
     <br />
</fieldset> 
        
          <br />
    
     
           <iMES:GridViewExt ID="gvResult" runat="server" 
        AutoGenerateColumns="true" GvExtHeight="450px" 
            Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" 
        onrowdatabound="gvResult_RowDataBound">
        </iMES:GridViewExt>
   
       
    



<script type="text/javascript">    //<![CDATA[
   
function Check() {

    var a = document.getElementById("<%=FileUpload1.ClientID%>").value;
    if (a == "")
    {alert("Please selece Excel File");  return false; }
    else
    { beginWaitingCoverDiv();  }
  
}

  </script>
                  
</asp:Content>

<%@ Page Title="ChepPallet" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="FAKeyPartsRequirementQuery.aspx.cs" Inherits="Query_FA_KeyPartsRequirementQuery" EnableEventValidation="false" %>
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
       <asp:Label ID="lblTitle" runat="server" Text="KeyParts Requirement Query" CssClass="iMes_label_13pt"></asp:Label></legend> 
    
                    <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"  ></asp:Label>               
                                      
                    <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                

           <asp:FileUpload ID="FileUpload1" runat="server" Width="236px" />
      <asp:Button ID="btnUpload" runat="server" onclick="btnUpload_Click" OnClientClick="return Check();" Text="Upload" />
&nbsp; 
            
    &nbsp; 
       <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" style=" display:none"      Text="Excel" />
     &nbsp;
    <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;<br />
</fieldset> 
        
          <br />
    
     
           <a name="info"></a>
    <table border="0" width="99%">
        <tr>
            <td width="70%">
                <fieldset id="Fieldset1" 
                    style="border: thin solid #000000; width: 98%; height: 500px;">
                    <legend align="left" style="height :20px">
                        <asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt" 
                            Text="KeyPart List"></asp:Label>
                    </legend>

                            <iMES:GridViewExt ID="gvResult" runat="server" 
                                AutoGenerateColumns="true" GvExtHeight="500px" GvExtWidth="100%" Height="1px" 
                                style="top: 100px; left: 2px" Width="98%">
                            </iMES:GridViewExt>
                </fieldset>
            </td>
            <td width="30%">
                <fieldset id="Fieldset4" 
                    style="border: thin solid #000000; width: 98%; height: 500px;">
                    <legend align="left" style="height :20px">
                        <asp:Label ID="Label4" runat="server" CssClass="iMes_label_13pt" 
                            Text="Not Exist Model List"></asp:Label>
                    </legend>

                            <iMES:GridViewExt ID="gvright" runat="server" AutoGenerateColumns="true" 
                                GvExtHeight="500px" GvExtWidth="100%" Height="1px" Width="98%" 
                                style="top: 100px; left: 2px">
                            </iMES:GridViewExt>
                </fieldset>
            </td>
        </tr>
    </table>
       
    



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

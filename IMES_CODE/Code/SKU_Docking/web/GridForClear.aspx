<%--
/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: gridview for clear page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2008-12-10  Zhao Meili(DD)        Create                    
 * Known issues:
 */
 --%>
<%@ Page Language="C#"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="GridForClear.aspx.cs" Inherits="GridForClear" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>

            <script type="text/javascript" src="CommonControl/JS/iMESCommonUse.js"></script>
                
            <script language ="javascript">
                parent.document.getElementById("hfvalue").value="";   
            function onRowClick(row)
            {
                setCvExtRowSelected_GridViewExt1(row);
                var key=row.cells[0].innerText.trim();
                var serviceName=row.cells[2].innerText.trim();
               parent.document.getElementById("hfvalue").value=key; 
               parent.document.getElementById("hfstationId").value=serviceName;
            }
            
            // exit page
         function ExitPage()
         {          
            parent.ExitPage();
         }
         
         //refresh page
         function ResetPage()
         {
      
            parent.ResetPage();
         }
            
            </script>
</head>
<body style ="margin-top :0px; margin-left :0px; margin-right :0px; " class="background_common" >
    <form id="form1" runat="server">

    <div  style=" width :100%;" >    
            <iMES:GridViewExt ID="GridViewExt1" SkinID="clearStyle" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true" GvExtWidth="100%"  GvExtHeight="276px" 
            OnGvExtRowClick="onRowClick(this)" OnGvExtRowDblClick=""  SetTemplateValueEnable="true"  GetTemplateValueEnable="true"  HighLightRowPosition="2"   AllowSorting="true"  OnRowDataBound="gv_RowDataBound"  OnSorting ="SortGrid">
                <Columns >   
                   <asp:BoundField DataField="change"  HeaderText="change"  SortExpression="change"/>
                   <asp:BoundField DataField="WC" HeaderText="WC"  SortExpression="WC"/> 
                    <asp:BoundField DataField="Service"  HeaderText="Service"  SortExpression="Service"/>    
                   <asp:BoundField DataField="PdLine" HeaderText="PdLine"  SortExpression="PdLine"/>  
                   <asp:BoundField DataField="Operator" HeaderText="Operator"  SortExpression="Operator"/>          
                   <asp:BoundField DataField="StartTime"  HeaderText="StartTime"  SortExpression="StartTime"/>    
                   <asp:BoundField DataField="StationId"  HeaderText="StationId"  SortExpression="StationId"/>    
                </Columns>
            </iMES:GridViewExt>
            
    </div>
    <asp:HiddenField  ID="hflastDirection"  runat="server"  />  
     <asp:HiddenField  ID="hflastSortExpression"  runat="server"  /> 
    </form>
</body>
</html>

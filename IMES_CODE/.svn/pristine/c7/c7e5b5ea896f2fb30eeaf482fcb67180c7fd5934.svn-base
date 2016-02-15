<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UPHDate.ascx.cs" Inherits="CommonControl_UPHDate" %>
  <div >
                   <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>                   
                     <asp:TextBox id="txtFromDate" runat="server" Width="140px" Height="20px"></asp:TextBox>                            
                     <button id="btnFromDate" type="button" style="width: 20px">...</button>
                    <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="140px" Height="20px"></asp:TextBox>
                    <button id="btnToDate" type="button" style="width: 20px">...</button>
               </div> 
 <link rel="stylesheet" type="text/css" href="../css/imes.css" />
  <script type="text/javascript" src="../js/calendar.js"></script>
 
<script language="javascript" type="text/javascript">


        $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();
        Calendar.setup({
            trigger: "btnFromDate",
            inputField: "<%=txtFromDate.ClientID%>",
            onSelect: updateCalendarFields,
            onTimeChange: updateCalendarFields,
            showTime: 24,
            dateFormat: "%Y-%m-%d %H:%M",
            minuteStep: 1
        });
        Calendar.setup({
            inputField: "<%=txtToDate.ClientID%>",
            trigger: "btnToDate",
            onSelect: updateCalendarFields,
            onTimeChange: updateCalendarFields,
            showTime: 24,
            dateFormat: "%Y-%m-%d %H:%M",
            minuteStep: 1
        });
   
  </script >  
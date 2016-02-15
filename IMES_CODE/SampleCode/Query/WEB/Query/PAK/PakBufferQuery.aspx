<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="PakBufferQuery.aspx.cs" Inherits="Query_PAK_PakBufferyQuery" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src=" ../../CommonControl/JS/Browser.js"></script>

    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../../js/jquery-1.7.1.js"></script>

    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js"></script>

    <script type="text/javascript" src="../../js/jscal2.js"></script>

    <script type="text/javascript" src="../../js/lang/en.js"></script>

    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../../css/steel/steel.css" />
    <style type="text/css">
        .DN
        {
            background-color: #CFECEC;
            text-align: right;
            font-size: 1.2em;
        }
        .DNTitle
        {
            background-color: #4E8975;
            text-align: right;
            font-size: 1.2em;
            color: White;
        }
        .pltClass:hover
        {
            cursor: pointer;
        }
        .pltClass.clicked
        {
            cursor: pointer;
            border-style: inset;
            border-width: 2px;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <center>
        <fieldset id="grpCarton" style="border: thin solid #000000;">
            <legend align="left" style="height: 20px">
                <asp:Label ID="lblTitle" runat="server" Text="棧板重量查詢" CssClass="iMes_label_13pt"
                    meta:resourcekey="lblTitleResource1"></asp:Label></legend>
            <table border="0" width="100%" style="font-family: Tahoma">
                <tr>
                    <td width="10%" align="right">
                        <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt" meta:resourcekey="lblDBResource1"></asp:Label>
                    </td>
                    <td align="left" style="width: 30%">
                        <iMESQuery:CmbDBType ID="CmbDBType" runat="server"   />
                    </td>
                    <td width="5%" align="right">
                        &nbsp;
                    </td>
                    <td width="40%" align="left">
                        &nbsp;
                    </td>
                    <td width="5%">
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Excel" Style="display: none" />
                    </td>
                    <td width="5%">
                        <asp:Button CssClass="iMesQuery_button" ID="btnQuery" runat="server" Text="Query"
                            OnClick="btnQuery_Click" OnClientClick="return Query()" />
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <asp:RadioButtonList ID="radList" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True">DN</asp:ListItem>
                            <asp:ListItem>Shipment</asp:ListItem>
                            <asp:ListItem>Pallet</asp:ListItem>
                            <asp:ListItem>Ship Date</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="iMes_label_13pt"></asp:Label>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="150px" Height="20px" Enabled="false"></asp:TextBox>
                        <asp:Label ID="lblTo" runat="server" Text="To" CssClass="iMes_label_13pt"></asp:Label>
                        <asp:TextBox ID="txtToDate" runat="server" Width="150px" Height="20px" Enabled="false"></asp:TextBox>
                    </td>
                    <td width="20%" align="left">
                        <asp:CheckBox ID="chkZero" runat="server" Text="未稱重" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <ContentTemplate>
                               <asp:Label ID="Label1" runat="server" Text="Total Weight(含棧板重):  "></asp:Label>
                                <asp:Label ID="labW" runat="server" Font-Size="Larger" ForeColor="#0000FF" Text="0"></asp:Label>
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 <asp:Label ID="Label2" runat="server" Text="Total Weight(不含棧板重):  "></asp:Label>
                                <asp:Label ID="labExclude" runat="server" Font-Size="Larger" ForeColor="#0000FF" Text="0"></asp:Label>
                                &nbsp;&nbsp;
                        <asp:Label ID="labHaveZeroPallet" runat="server" Font-Size="XX-Large" 
                                    ForeColor="#FF3300" Font-Bold="True" Visible="True"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table> 
        </fieldset>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <iMES:GridViewExt ID="gvResult" runat="server" GvExtHeight="300px" Width="98%" GvExtWidth="98%"
                    Height="1px" HiddenColCount="0" HighLightRowPosition="1" OnGvExtRowClick="" OnGvExtRowDblClick=""
                    SetTemplateValueEnable="False" OnRowDataBound="gvResult_RowDataBound" Style="top: 0px;
                    left: 0px">
                </iMES:GridViewExt>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    

    <script language="javascript" type="text/javascript">
   $("#<%=radList.ClientID%> input").change(function() {
 
 
   var rbvalue = $("input[type=radio]:checked").val();

   if(rbvalue!='Ship Date')
   {
      $("#<%=txtFromDate.ClientID%>").attr("disabled","disabled");
      $("#<%=txtToDate.ClientID%>").attr("disabled","disabled");
      $("#<%=TextBox1.ClientID%>").removeAttr("disabled");
      $("#<%=TextBox1.ClientID%>").css("background-color","#E0FCC9");
     
   }
   else
   {$("#<%=TextBox1.ClientID%>").attr("disabled","disabled");
      $("#<%=txtFromDate.ClientID%>").removeAttr("disabled"); 
      $("#<%=txtToDate.ClientID%>").removeAttr("disabled"); 
      $("#<%=TextBox1.ClientID%>").css("background-color","#C0C0C0");
   }
 
});
 Calendar.setup({
        inputField: ConvertID("txtFromDate"),
        trigger: ConvertID("txtFromDate"),
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });
    Calendar.setup({
        inputField: ConvertID("txtToDate"),
        trigger: ConvertID("txtToDate"),
        onSelect: function() { this.hide() },
        showTime: 24,
        dateFormat: "%Y-%m-%d",
        minuteStep: 1
    });
    function Query() {
     var rbvalue = $("input[type=radio]:checked").val();
      if(rbvalue!='Ship Date')
      {
          var inputID='#'+ ConvertID("TextBox1");
          var inputValue = $(inputID).val();
          if(inputValue.trim()=="")
          {
             alert('Pleae input');return false;
          }
   
      }
      beginWaitingCoverDiv();
    }
  var clickDN="";
  var rowOBJ;
  function GetDN(obj,plt)
  {
     $(".clicked").removeClass("clicked");
      if(plt==clickDN)
      {
           $(".DN").remove() ;
           $(".DNTitle").remove() ;
           clickDN="";
           return;
       }
     $(obj).addClass("clicked");
     $(obj.parentNode).addClass("clicked");
     ShowWait();
     $(".DN").remove() ;
     $(".DNTitle").remove() ;
     rowOBJ=obj;
     PageMethods.GetDNbyPalletNo(plt, onSuccess, onError);
     clickDN=plt;
   
   return;
  // $($($($(grvID)[1]).clone(true).find("td").each(function(){$(this).text('').attr('onclick','');})[0]).text('457')).parent().addClass('DN').insertAfter(obj.parentNode)
  // $($($($(grvID)[1]).clone(true).find("td").each(function(){$(this).text('').attr('onclick','');})[0]).text('458')).parent().addClass('DN').insertAfter(obj.parentNode)
   
   //clickDN=plt;
    //$($($(grvID)[1]).clone(true).find("td").each(function(){$(obj).text('')})[0]).text('123').insertAfter($(obj));
    //border-style:outset
    //border-style:outset;border-width:5px;
  }
    function onSuccess(receiveData) {

        var colArr;
        var dn;
        var  model;
        var qty;
        var newRow ;
        for( i=0;i<receiveData.length;i++)
        {
           colArr=receiveData[i].split(",");
           dn=colArr[0];
           model=colArr[1];
           qty=colArr[2];
           newRow = $('<tr class="DN"><td>'+ dn +'</td><td>'+model+'</td><td>'+qty+'</td><td></td></tr>');
           $(rowOBJ.parentNode).after(newRow);
        }
        newRow = $('<tr class="DNTitle"><td>DN</td><td>Model</td><td>Qty</td><td></td></tr>');
        $(rowOBJ.parentNode).after(newRow);
        HideWait();
    }
    // 失敗時彈出失敗訊息
    function onError(error) {
        if (error != null)
            alert(error.get_message());
            HideWait();
    }
  
    </script>

</asp:Content>

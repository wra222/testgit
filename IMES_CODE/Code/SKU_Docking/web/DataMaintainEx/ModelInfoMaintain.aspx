<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: model info maintain
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-11-10  98079                Create 
 Known issues:
qa bug no:ITC-1136-0117
 --%>

<%@ Page Language="C#" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ModelInfoMaintain.aspx.cs" Inherits="ModelInfoMaintain" Title="" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <style>
        table.edit
        {
            border: thin solid Black; 
            background-color: #99CDFF;
        }
    </style>
</head>
<body style=" position: relative; width: 100%"> 
    <form id="form1" runat="server"> 
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/DataMaintain/Service/ModelMaintainService.asmx" />
        </Services>
    </asp:ScriptManager>

    <div id="container">
    <table>
        <tr>
            <td>
                <table style="width:100%" border=0 class="edit">
                    <tr>
                        <td width=6%>
                            <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td><td>
                            <asp:TextBox ID="valModel" runat="server" CssClass="iMes_label_13pt" 
                                Width="100%" ></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table border=0>
                    <tr>
                        <td>
                            <asp:Label ID="lblInfo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" >
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshModelList" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                            </Triggers>
                            <ContentTemplate>

                                <iMES:GridViewExt ID="gdModelInfoList" runat="server" AutoGenerateColumns="true" Width="100%" 
                                    GvExtWidth="100%" GvExtHeight="410px" Height="400px" OnRowDataBound="gd_RowDataBound"
                                     OnGvExtRowClick="clickTable(this)" SetTemplateValueEnable="False" 
                                     HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                                </iMES:GridViewExt>
                                 
                            </ContentTemplate>   
                            </asp:UpdatePanel>                         
                            <button id="btnRefreshModelList" runat="server" type="button" onclick="" style="display: none" onserverclick="btnRefreshModelList_Click"></button>
                            <input type="hidden" id="hidItem" runat="server" value=""/>
                            <input type="hidden" id="hidModelInfoId" runat="server" value=""/>                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width=100% border=0 class="edit">
                    <tr>
                        <td>
                            <table width=100% border=0 cellpadding=0 cellspacing=0>
                                <tr>
                                    <td style="width:8%"><asp:Label ID="valItem"  runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td><asp:TextBox id="txtValue" MaxLength=200 runat="server" SkinId="textBoxSkin" Width=400px></asp:TextBox></td>                        
                                    <td align=right><input id="btnSave" disabled type="button"  runat="server" class="iMes_button" onserverclick="btnSave_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </div> 
    </form>

<script type="text/javascript">
    var selectedRowIndex = null;
    //error message
    var msgDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDelete").ToString() %>';

   function clkDelete()
   {
       if(confirm(msgDelete))
       {
           //ShowInfo("");
           return true;
       }   
       return false;
        
   }

    function SaveComplete(id){
        //ShowSuccessfulInfo(true);        
        //alert(id)       
        if(id == "-1" || id == ""){
            document.getElementById("<%=valItem.ClientID%>").innerText = "";
            document.getElementById("<%=txtValue.ClientID%>").value = "";
            document.getElementById("<%=hidModelInfoId.ClientID%>").value = "";
            document.getElementById("<%=hidItem.ClientID%>").value = "";
            document.getElementById("<%=btnSave.ClientID%>").disabled = true;
            return;
        }
        
        var gdModelInfoListClientID="<%=gdModelInfoList.ClientID%>";
        var row=eval("setScrollTopForGvExt_"+gdModelInfoListClientID+"('"+id+"',0,'','MUTISELECT')");
        
        if(row != null)
        {
        
            selectedRowIndex = row.rowIndex - 1;


            document.getElementById("<%=valItem.ClientID%>").innerText = row.cells[<%=indexField["Name"]%>].innerText.trim();
            document.getElementById("<%=txtValue.ClientID%>").value = row.cells[<%=indexField["Value"]%>].innerText.trim();
            document.getElementById("<%=hidModelInfoId.ClientID%>").value = row.cells[<%=indexField["Id"]%>].innerText.trim();
            document.getElementById("<%=hidItem.ClientID%>").value = row.cells[<%=indexField["Name"]%>].innerText.trim();


            //model info id
            if(row.cells[0].innerText.trim() == ""){
                document.getElementById("<%=btnSave.ClientID%>").disabled = true;
            }else{
                document.getElementById("<%=btnSave.ClientID%>").disabled = false;
            }
            //document.getElementById("<%=btnSave.ClientID%>").disabled = false;
        
        }else{
            document.getElementById("<%=valItem.ClientID%>").innerText = "";
            document.getElementById("<%=txtValue.ClientID%>").value = "";
            document.getElementById("<%=hidModelInfoId.ClientID%>").value = "";
            document.getElementById("<%=hidItem.ClientID%>").value = "";
            document.getElementById("<%=btnSave.ClientID%>").disabled = true;
        }
        
    }



    function GetModelList(){
         document.getElementById("<%=btnRefreshModelList.ClientID%>").click();
    }

    function clickTable(row)
    {
        //selectedRowIndex = parseInt(con.index, 10);
        
        if((selectedRowIndex!=null) && (selectedRowIndex!=parseInt(row.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex,false, "<%=gdModelInfoList.ClientID %>");                
        }
        
        setRowSelectedOrNotSelectedByIndex(row.index,true, "<%=gdModelInfoList.ClientID %>");
        selectedRowIndex = parseInt(row.index, 10);


        document.getElementById("<%=valItem.ClientID%>").innerText = row.cells[<%=indexField["Name"]%>].innerText.trim() + ":";
        document.getElementById("<%=txtValue.ClientID%>").value = row.cells[<%=indexField["Value"]%>].innerText.trim();
        document.getElementById("<%=hidModelInfoId.ClientID%>").value = row.cells[<%=indexField["Id"]%>].innerText.trim();
        document.getElementById("<%=hidItem.ClientID%>").value = row.cells[<%=indexField["Name"]%>].innerText.trim();


        //model info id
        if(row.cells[0].innerText.trim() == ""){
            document.getElementById("<%=btnSave.ClientID%>").disabled = true;
        }else{
            document.getElementById("<%=btnSave.ClientID%>").disabled = false;
        }
        //document.getElementById("<%=btnSave.ClientID%>").disabled = false;
    }
    
    
</script>
</body>  
</html>
 


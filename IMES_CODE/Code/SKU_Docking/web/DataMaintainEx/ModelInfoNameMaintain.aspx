<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: model info maintain
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-11-10  98079                Create 
 Known issues:

 --%>

<%@ Page Language="C#" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ModelInfoNameMaintain.aspx.cs" Inherits="ModelInfoNameMaintain" Title="" %>
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
    <table border=0>
        <tr>
            <td>
                <table border=0>
                    <tr>
                        <td>
                            <asp:Label ID="lblInfo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td align=right>
                            <input type="hidden" id="hidModelInfoNameId" runat="server" value=""/>
                            <input id="btnDelete" disabled type="button"  runat="server" class="iMes_button" onclick="if(clkDelete())" onserverclick="btnDelete_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan=2>
                            <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" >
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshModelList" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                            </Triggers>
                            <ContentTemplate>

                                <iMES:GridViewExt ID="gdModelInfoNameList" runat="server" AutoGenerateColumns="true" Width="100%" 
                                    GvExtWidth="100%" GvExtHeight="410px" Height="400px" OnRowDataBound="gd_RowDataBound"
                                     OnGvExtRowClick="clickTable(this)" SetTemplateValueEnable="False" 
                                     HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                                </iMES:GridViewExt>
                                 
                            </ContentTemplate>   
                            </asp:UpdatePanel>                         
                            <button id="btnRefreshModelList" runat="server" type="button" onclick="" style="display: none" onserverclick="btnRefreshModelList_Click"></button>
                            <input type="hidden" id="oldModelInfoName" runat="server" value=""/>

                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height:70px">
                <table width=100% style="height:100%" border=1 class="edit">
                    <tr>
                        <td>
                            <table width=100% style="height:100%" border=0 cellpadding=0 cellspacing=0>
                                <tr>
                                    <td><asp:Label ID="lblRegion" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td><iMESMaintainEx:CmbRegionForMaintain ID="CmbRegion" runat="server" Width="140" FirstNullItem="false"/></td>                        
                                    <td><asp:Label ID="lblName" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td><asp:TextBox id="txtName" MaxLength=50  runat="server" SkinId="textBoxSkin" Height="23px" Width="200px"></asp:TextBox></td>
                                    <td><input id="btnAdd" type="button"  runat="server" class="iMes_button" onclick="if(clkSave())" onserverclick="btnAdd_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/></td>
                                </tr>
                                <tr>
                                    <td><asp:Label ID="lblDesc" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td colspan=3><asp:TextBox id="txtDesc" MaxLength=80  runat="server" SkinId="textBoxSkin" Height="23px" Width="710"></asp:TextBox></td>
                                    <td><input id="btnSave" disabled type="button"  runat="server" class="iMes_button" onclick="if(clkSave())" onserverclick="btnSave_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/></td>
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
    var ObjRegion;
    var selectedRowIndex = null;
    //error message
    var msgDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDelete").ToString() %>';
    var msgAddSave = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAddSave").ToString() %>';

   function clkDelete()
   {
       if(confirm(msgDelete))
       {
           //ShowInfo("");
           return true;
       }   
       return false;
        
   }

    function test_clearInputs(){
        document.getElementById("<%=txtName.ClientID%>").value = "";
        document.getElementById("<%=txtDesc.ClientID%>").value = "";
        document.getElementById("<%=btnDelete.ClientID%>").disabled = true;
        document.getElementById("<%=btnSave.ClientID%>").disabled = true;
        document.getElementById("<%=hidModelInfoNameId.ClientID%>").value = "";        
    }

   function clkSave()
   {
       //ShowInfo("");
       var name = document.getElementById("<%=txtName.ClientID %>").value;   
       if(name.trim()=="")
       {
           //ShowMessage(msgAddSave);
           alert(msgAddSave);
           return false;
       }   
       return true;
        
   }

    function GetModelList(){
         document.getElementById("<%=btnRefreshModelList.ClientID%>").click();
    }

    function clickTable(row)
    {
        //selectedRowIndex = parseInt(con.index, 10);
        
        if((selectedRowIndex!=null) && (selectedRowIndex!=parseInt(row.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex,false, "<%=gdModelInfoNameList.ClientID %>");                
        }
        
        setRowSelectedOrNotSelectedByIndex(row.index,true, "<%=gdModelInfoNameList.ClientID %>");
        selectedRowIndex = parseInt(row.index, 10);


        document.getElementById("<%=txtName.ClientID%>").value = row.cells[2].innerText.trim();

        document.getElementById("<%=txtDesc.ClientID%>").value = row.cells[3].innerText.trim();

        //model info name id
        if(row.cells[0].innerText.trim() == ""){
            document.getElementById("<%=btnDelete.ClientID%>").disabled = true;
            document.getElementById("<%=btnSave.ClientID%>").disabled = true;
            document.getElementById("<%=hidModelInfoNameId.ClientID%>").value = "";
        }else{
            document.getElementById("<%=btnDelete.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave.ClientID%>").disabled = false;
            document.getElementById("<%=hidModelInfoNameId.ClientID%>").value = row.cells[0].innerText.trim();

            ObjRegion = getMyRegionCmbObj();
            ObjRegion.value = row.cells[1].innerText.trim();

        }
        
    }
    
    function DeleteComplete()
    {

        document.getElementById("<%=txtName.ClientID%>").value = "";
        document.getElementById("<%=txtDesc.ClientID%>").value = "";
        document.getElementById("<%=hidModelInfoNameId.ClientID%>").value = "";
        document.getElementById("<%=btnDelete.ClientID%>").disabled = true;
        document.getElementById("<%=btnSave.ClientID%>").disabled = true;

    }

    function SaveComplete(id){
        //ShowSuccessfulInfo(true);        
        //alert(id)       
        if(id == "-1"){
            document.getElementById("<%=txtName.ClientID%>").value = "";
            document.getElementById("<%=txtDesc.ClientID%>").value = "";
            document.getElementById("<%=hidModelInfoNameId.ClientID%>").value = "";
            document.getElementById("<%=btnDelete.ClientID%>").disabled = true;
            document.getElementById("<%=btnSave.ClientID%>").disabled = true;
            return;
        }
        
        var gdModelInfoNameListClientID="<%=gdModelInfoNameList.ClientID%>";
        var row=eval("setScrollTopForGvExt_"+gdModelInfoNameListClientID+"('"+id+"',0,'','MUTISELECT')");
        
        if(row != null)
        {
        
            selectedRowIndex = row.rowIndex - 1;

            ObjRegion = getMyRegionCmbObj();
            ObjRegion.value = row.cells[1].innerText.trim();

            document.getElementById("<%=txtName.ClientID%>").innerText = row.cells[2].innerText.trim();
            document.getElementById("<%=txtDesc.ClientID%>").value = row.cells[3].innerText.trim();
            document.getElementById("<%=hidModelInfoNameId.ClientID%>").value = row.cells[0].innerText.trim();


            //model info id
            if(row.cells[0].innerText.trim() == ""){
                document.getElementById("<%=btnSave.ClientID%>").disabled = true;
                document.getElementById("<%=btnDelete.ClientID%>").disabled = true;
            }else{
                document.getElementById("<%=btnSave.ClientID%>").disabled = false;
                document.getElementById("<%=btnDelete.ClientID%>").disabled = false;
            }
             
        
        }else{
            document.getElementById("<%=txtName.ClientID%>").value = "";
            document.getElementById("<%=txtDesc.ClientID%>").value = "";
            document.getElementById("<%=hidModelInfoNameId.ClientID%>").value = "";
            document.getElementById("<%=btnDelete.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave.ClientID%>").disabled = false;
        }
        
    }
    
</script>
</body>  
</html>
 


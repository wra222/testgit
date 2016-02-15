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

<%@ Page Language="C#" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="StationInfoAttribute.aspx.cs" Inherits="StationInfoAttribute" Title="" %>
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
            <asp:ServiceReference Path="~/DataMaintain/service/WebServiceStationInfoAttribute.asmx" />
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
                            <asp:Label ID="valModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
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
                                <asp:AsyncPostBackTrigger ControlID="btndel" EventName="ServerClick" />
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
                                    <td style="width:8%"><asp:Label ID="lblattrName" Text="AttrName:" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td style="width:20%">
                                        <%--<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
                                            <ContentTemplate>--%>
                                                <asp:DropDownList ID="drpattrName" runat="server" Width="160px"></asp:DropDownList>
                                           <%-- </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    
                                        <%--<asp:TextBox id="txtattrName" MaxLength=80 runat="server" SkinId="textBoxSkin"></asp:TextBox>--%>
                                    </td>
                                    <td style="width:8%"><asp:Label ID="lblattrvalue" Text="AttrValue:" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td style="width:20%"><asp:TextBox id="txtattrvalue" MaxLength=80 runat="server" SkinId="textBoxSkin"></asp:TextBox></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr></tr>
                                <tr>
                                    <td style="width:8%"><asp:Label ID="lbldescr" Text="Descr:" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td colspan="5" style="width:50%"><asp:TextBox id="txtdesce" MaxLength=150 runat="server" SkinId="textBoxSkin" Width = "86%"></asp:TextBox></td>                               
                                    <td align=right><input id="btnSave" type="button"  runat="server" class="iMes_button" onclick="if(clkSave())" onserverclick="btnSave_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/></td>
                                    <td align=right><input id="btndel" disabled type="button"  runat="server" class="iMes_button" onclick="if(clkDelete())" onserverclick="btnDelete_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hidattrname" runat="server" />
    </div> 
    </form>

<script type="text/javascript">
    var selectedRowIndex = null;
    //error message
    var msgDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDelete").ToString() %>';

    function clkDelete()
    {
        if (confirm(msgDelete)) 
        {
            document.getElementById("<%=hidattrname.ClientID%>").value = document.getElementById("<%=drpattrName.ClientID%>").value;
           return true;
        }   
        return false;

    }

    function clkSave() 
    {
        if (document.getElementById("<%=drpattrName.ClientID%>").value == "") {
            alert("請選擇AttrName...");
           
           return false;
       }
       if (document.getElementById("<%=txtattrvalue.ClientID%>").value == "") {
           alert("請輸入AttrValue...");
           return false;
       }
       if (document.getElementById("<%=txtdesce.ClientID%>").value == "") {
           alert("請輸入Descr...");
           return false;
       }
       document.getElementById("<%=hidattrname.ClientID%>").value = document.getElementById("<%=drpattrName.ClientID%>").value;
        return true;

    }

    function SaveComplete(id){
        //ShowSuccessfulInfo(true);        
        //alert(id)       
        if(id == "-1"){
            document.getElementById("<%=drpattrName.ClientID%>").value = "";
            document.getElementById("<%=txtattrvalue.ClientID%>").value = "";
            document.getElementById("<%=txtdesce.ClientID%>").value = "";
            
            document.getElementById("<%=hidModelInfoId.ClientID%>").value = "";
            document.getElementById("<%=hidItem.ClientID%>").value = "";
            
            document.getElementById("<%=btndel.ClientID%>").disabled = true;
            
            return;
        }
        
        var gdModelInfoListClientID="<%=gdModelInfoList.ClientID%>";
        var row=eval("setScrollTopForGvExt_"+gdModelInfoListClientID+"('"+id+"',0,'','MUTISELECT')");
        
        if(row != null)
        {
        
            selectedRowIndex = row.rowIndex - 1;

            document.getElementById("<%=drpattrName.ClientID%>").value = row.cells[1].innerText.trim();
            document.getElementById("<%=txtattrvalue.ClientID%>").value = row.cells[2].innerText.trim();
            document.getElementById("<%=txtdesce.ClientID%>").value = row.cells[3].innerText.trim();
            
            document.getElementById("<%=hidModelInfoId.ClientID%>").value = row.cells[0].innerText.trim();
            document.getElementById("<%=hidItem.ClientID%>").value = row.cells[1].innerText.trim();


            //model info id
            if(row.cells[0].innerText.trim() == ""){
                document.getElementById("<%=btndel.ClientID%>").disabled = true;
            } 
            else
            {
                document.getElementById("<%=btndel.ClientID%>").disabled = false;
            }


        }
        else
        {

            document.getElementById("<%=drpattrName.ClientID%>").value = "";
            document.getElementById("<%=txtattrvalue.ClientID%>").value = "";
            document.getElementById("<%=txtdesce.ClientID%>").value = "";
            
            document.getElementById("<%=hidModelInfoId.ClientID%>").value = "";
            document.getElementById("<%=hidItem.ClientID%>").value = "";
            
            document.getElementById("<%=btndel.ClientID%>").disabled = true;
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

        document.getElementById("<%=drpattrName.ClientID%>").value = row.cells[1].innerText.trim();
        document.getElementById("<%=txtattrvalue.ClientID%>").value = row.cells[2].innerText.trim();
        document.getElementById("<%=txtdesce.ClientID%>").value = row.cells[3].innerText.trim();
        
        document.getElementById("<%=hidModelInfoId.ClientID%>").value = row.cells[0].innerText.trim();
        document.getElementById("<%=hidItem.ClientID%>").value = row.cells[1].innerText.trim();


        //model info id
        if (row.cells[0].innerText.trim() == "") {
            document.getElementById("<%=btndel.ClientID%>").disabled = true;
        }
        else {
            document.getElementById("<%=btndel.ClientID%>").disabled = false;
        }
    }
    
    
</script>
</body>  
</html>
 


<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: MB Label Print(SA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-11-10  liu xiao-ling        Create 
 qa bug no:ITC-1136-0056,ITC-1136-0057,ITC-1136-0090,ITC-1136-0109,ITC-1136-0110
 --%>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="PartType.aspx.cs" Inherits="PartType" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <style>
    table.edit
    {
        border: thin solid Black; 
        background-color: #99CDFF;
        margin:0 0 10 0;
    }
</style>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/DataMaintain/Service/PartMaintainService.asmx" />
        </Services>
    </asp:ScriptManager>

    <center>
    <table border=0 width=100%>
        <tr>
            <td style="width:50%">
                <table border=0>
                    <tr>
                        <td>
                            <asp:Label ID="lblPartTypeList" runat="server" Font-Bold="true" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td align=right>
                            <input id="btnDelete1" type="button" disabled runat="server" class="iMes_button" onclick="if(clkDelete())" onserverclick="btnDeletePartType_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan=2>
                            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnDelete1" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnAdd1" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave1" EventName="ServerClick" />
                            </Triggers>
                            <ContentTemplate>

                                <iMES:GridViewExt ID="gdPartType" runat="server" AutoGenerateColumns="true" Width="98.9%" 
                                    GvExtWidth="99%" GvExtHeight="450px" Height="440px" OnRowDataBound="gdPartType_RowDataBound"
                                     OnGvExtRowClick="clickPartTypeTable(this)" SetTemplateValueEnable="False" 
                                     HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                                </iMES:GridViewExt>
                                 
                            </ContentTemplate>   
                            </asp:UpdatePanel>                         
                            <button id="btnRefreshSAPTypeAndAttributeAndDescList" runat="server" type="button" style="display: none" onserverclick="btnRefreshSAPTypeAndAttributeAndDescList_Click"></button>
                            <asp:UpdatePanel ID="updatePanel5" runat="server" UpdateMode=Always Visible=false></asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td  colspan=2>
                            <table width=99% border=0 cellpadding=0 cellspacing=0 class="edit">
                                <tr><td colspan=3 height=3px></td></tr>
                                <tr>
                                    <td style="width:15%">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblPartType" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td style="width:60%"><asp:TextBox id="txtPartType" MaxLength=50 runat="server" SkinId="textBoxSkin" Width="98%"></asp:TextBox></td>                        
                                    <td><input id="btnAdd1" type="button" runat="server" class="iMes_button" onclick="if(clkAdd1())" onserverclick="btnAddPartType_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/></td>
                               </tr>
                                <tr>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblGroup" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td><asp:TextBox id="txtGroup" MaxLength=50 runat="server" SkinId="textBoxSkin" Width="98%"></asp:TextBox></td>
                                    <td><input id="btnSave1" type="button" disabled runat="server" class="iMes_button" onclick="if(clkAdd1())" onserverclick="btnSavePartType_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/></td>
                                </tr>
                                <tr><td colspan=3 height=3px><input type="hidden" id="hidOldPartType" runat="server" value=""/></td></tr>
                            </table>
                        </td>
                     </tr>
                </table>
            </td>
            <td>
                <table border=0 bordercolor=red>
                    <tr>
                        <td>
                            <asp:Label ID="lblAttribute" runat="server" Font-Bold="true" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td align=right>
                            <input id="btnDelete3" type="button" disabled runat="server" class="iMes_button" onclick="if(clkDelete())" onserverclick="btnDeleteAttribute_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan=2>
                            <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" >
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnDelete3" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave3" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnAdd3" EventName="ServerClick" />
                                  <asp:AsyncPostBackTrigger ControlID="btnDelete1" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshSAPTypeAndAttributeAndDescList" EventName="ServerClick" />
                            </Triggers>
                            <ContentTemplate>
                                <iMES:GridViewExt ID="gdAttribute" runat="server" AutoGenerateColumns="true" Width="99.9%" 
                                    GvExtWidth="100%" GvExtHeight="250px" Height="240px" OnRowDataBound="gdAttribute_RowDataBound"
                                     OnGvExtRowClick="clickAttributeTable(this)" 
                                    SetTemplateValueEnable="False" HighLightRowPosition="3" 
                                     AutoHighlightScrollByValue="True" style="top: 33px; left: 552px">
                                </iMES:GridViewExt>
                                 
                             </ContentTemplate>   
                             </asp:UpdatePanel>                         
                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan=2>
                            <table width=98% border=0 cellpadding=0 cellspacing=0 class="edit">
                                <tr><td colspan=3 height=3px></td></tr>
                                <tr>
                                    <td style="width:15%">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblCode" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td style="width:55%"><asp:TextBox id="txtCode" MaxLength=50 runat="server" SkinId="textBoxSkin" Width="98%"></asp:TextBox></td>                        
                                    <td style="width:15%"><input id="btnAdd3" type="button" disabled runat="server" class="iMes_button" onclick="if(clkAdd3())" onserverclick="btnAddAttribute_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/></td>
                                    <td style="width:15%"><input id="btnSave3" type="button" disabled runat="server" class="iMes_button" onclick="if(clkAdd3())" onserverclick="btnSaveAttribute_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>&nbsp;&nbsp;&nbsp;</td>
                                </tr>
                                <tr> 
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblDesc" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td colspan=3><asp:TextBox id="txtDesc" MaxLength=500 runat="server" SkinId="textBoxSkin" Width="98%"></asp:TextBox></td>                        
                               </tr>
                                <tr><td colspan=3 height=3px><input type="hidden" id="hidCode" runat="server" value=""/></td></tr>
                             </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescList" runat="server" Font-Bold="true" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td align=right>
                            <input id="btnDelete4" type="button" disabled runat="server" class="iMes_button" onclick="if(clkDelete())" onserverclick="btnDeleteDesc_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                        </td>
                    </tr>
                    <tr>
                        <td  colspan=2>
                            <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional" >
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnDelete4" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave4" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnAdd4" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshSAPTypeAndAttributeAndDescList" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="btnDelete1" EventName="ServerClick" />
                            </Triggers>
                            <ContentTemplate>
                                <iMES:GridViewExt ID="gdDescription" runat="server" AutoGenerateColumns="true" Width="99.9%" 
                                    GvExtWidth="100%" GvExtHeight="130px" Height="130px" OnRowDataBound="gdDescription_RowDataBound"
                                     OnGvExtRowClick="clickDescTable(this)" SetTemplateValueEnable="False" HighLightRowPosition="3" 
                                     AutoHighlightScrollByValue="True" style="top: 0px; left: -53px">
                                </iMES:GridViewExt>
                                 
                             </ContentTemplate>   
                             </asp:UpdatePanel>                         
                        
                        </td>
                    </tr>
                    <tr>
                        <td  colspan=2>
                            <table width=99% border=0 cellpadding=0 cellspacing=0 class="edit">
                                <tr><td colspan=4 height=3px></td></tr>
                                <tr>
                                    <td style="width:15%">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblDescDesc" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                                    <td style="width:55%"><asp:TextBox id="txtDescDesc" MaxLength=30 runat="server" SkinId="textBoxSkin" Width="98%"></asp:TextBox></td>
                                    <td style="width:15%"><input id="btnAdd4" type="button" disabled runat="server" class="iMes_button" onclick="if(clkAdd4())" onserverclick="btnAddDesc_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/></td>
                                    <td style="width:15%"><input id="btnSave4" type="button" disabled runat="server" class="iMes_button" onclick="if(clkAdd4())" onserverclick="btnSaveDesc_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/></td>
                                </tr>
                                <tr><td colspan=3 height=3px><input type="hidden" id="hidDescDescID" runat="server" value=""/></td></tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </center> 
     <input type="hidden" id="HiddenUserName" runat="server" />
     <input type="hidden" id="hidPartTypeID" runat="server" />
</div>
<script for="window" language=javascript event="onresize">
    //document.getElementById("<%=gdPartType.ClientID%>").parentNode.parentNode.style.width = (document.getElementById("assemblyCodeSize").clientWidth)+"px";
    //document.getElementById("<%=gdAttribute.ClientID%>").parentNode.parentNode.style.width = (document.getElementById("attributeSize").clientWidth)+"px";
</script>

<script for=window event=onload>
    editor = "<%=editor%>";

</script>  


<script type="text/javascript">
    var selectedRowIndex_PartType = null, selectedRowIndex_Attribute = null, selectedRowIndex_SAPType = null, selectedRowIndex_Desc = null;
    var txtPartNoValue;
    var btnSearchCodePress = false;
    var ObjPartType;
    var ObjPartTypeDesc;
    var ObjFamily;
    var ObjModel;
    var ObjRegion;
    var lstCodeSelectedIndex = -1;
    var editor;

    var msgDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDelete").ToString() %>';
    var msgAdd1PartType = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAdd1PartType").ToString() %>';
    var msgAdd2SAPPType = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAdd2SAPPType").ToString() %>';
    var msgAdd3Code = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAdd3Code").ToString() %>';
    var msgAdd4Description = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAdd4Description").ToString() %>';









    function clkAdd1() {
        var partType = document.getElementById("<%=txtPartType.ClientID %>").value;
        if (partType.trim() == "") {
            alert(msgAdd1PartType);
            return false;
        }
        return true;

    }



    function clkAdd3() {
        var partType = document.getElementById("<%=txtCode.ClientID %>").value;
        if (partType.trim() == "") {
            alert(msgAdd3Code);
            return false;
        }
        //    document.getElementById("<%=txtCode.ClientID %>").value = "";
        return true;

    }

    function clkAdd4() {
        var partType = document.getElementById("<%=txtDescDesc.ClientID %>").value;
        if (partType.trim() == "") {
            alert(msgAdd4Description);
            return false;
        }
        return true;

    }


    function clkDelete() {
        if (confirm(msgDelete)) {
            return true;
        }
        return false;

    }


    function clickPartTypeTable(row) {
        //selectedRowIndex = parseInt(con.index, 10);
        if ((selectedRowIndex_PartType != null) && (selectedRowIndex_PartType != parseInt(row.index, 10))) {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex_PartType, false, "<%=gdPartType.ClientID %>");
        }

        setRowSelectedOrNotSelectedByIndex(row.index, true, "<%=gdPartType.ClientID %>");
        selectedRowIndex_PartType = parseInt(row.index, 10);


        document.getElementById("<%=hidPartTypeID.ClientID%>").value = row.cells[0].innerText.trim();
        document.getElementById("<%=txtPartType.ClientID%>").value = row.cells[1].innerText.trim();
        document.getElementById("<%=hidOldPartType.ClientID%>").value = row.cells[1].innerText.trim();
        document.getElementById("<%=txtGroup.ClientID%>").value = row.cells[2].innerText.trim();
        document.getElementById("<%=btnRefreshSAPTypeAndAttributeAndDescList.ClientID%>").click();


        //part type
        if (row.cells[1].innerText.trim() == "") {
            document.getElementById("<%=btnDelete1.ClientID%>").disabled = true;
            document.getElementById("<%=btnSave1.ClientID%>").disabled = true;
            document.getElementById("<%=btnAdd3.ClientID%>").disabled = true;
            document.getElementById("<%=btnAdd4.ClientID%>").disabled = true;
        } else {
            document.getElementById("<%=btnDelete1.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave1.ClientID%>").disabled = false;
            document.getElementById("<%=btnAdd3.ClientID%>").disabled = false;
            document.getElementById("<%=btnAdd4.ClientID%>").disabled = false;
        }

    }

    function AddSave1Complete(id) {
        if (id.length == 0) return;
        var gdPartTypeClientID = "<%=gdPartType.ClientID%>";
        var row = eval("setScrollTopForGvExt_" + gdPartTypeClientID + "('" + id + "',1)");
        clickPartTypeTable(row);
    }

    function Delete1Complete() {
        document.getElementById("<%=btnDelete1.ClientID%>").disabled = true;
        document.getElementById("<%=btnSave1.ClientID%>").disabled = true;
        document.getElementById("<%=btnAdd3.ClientID%>").disabled = true;
        document.getElementById("<%=btnAdd4.ClientID%>").disabled = true;
        document.getElementById("<%=hidPartTypeID.ClientID%>").value = "";
        document.getElementById("<%=txtGroup.ClientID%>").value = "";
        document.getElementById("<%=txtPartType.ClientID%>").value = "";
        document.getElementById("<%=hidOldPartType.ClientID%>").value = "";
        selectedRowIndex_PartType = null
    }





    function clickAttributeTable(row) {
        //selectedRowIndex = parseInt(con.index, 10);
        if ((selectedRowIndex_Attribute != null) && (selectedRowIndex_Attribute != parseInt(row.index, 10))) {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex_Attribute, false, "<%=gdAttribute.ClientID %>");
        }

        setRowSelectedOrNotSelectedByIndex(row.index, true, "<%=gdAttribute.ClientID %>");
        selectedRowIndex_Attribute = parseInt(row.index, 10);


        document.getElementById("<%=txtCode.ClientID%>").value = row.cells[0].innerText.trim();
        document.getElementById("<%=txtDesc.ClientID%>").value = row.cells[1].innerText.trim();
        document.getElementById("<%=hidCode.ClientID%>").value = row.cells[0].innerText.trim();




        //id
        if (row.cells[0].innerText.trim() == "") {
            document.getElementById("<%=btnDelete3.ClientID%>").disabled = true;
            document.getElementById("<%=btnSave3.ClientID%>").disabled = true;
        } else {
            document.getElementById("<%=btnDelete3.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave3.ClientID%>").disabled = false;
        }

    }

    function AddSave3Complete(id) {
        if (id.length == 0) return;

        var gdAttributeClientID = "<%=gdAttribute.ClientID%>";
        var row = eval("setScrollTopForGvExt_" + gdAttributeClientID + "('" + id + "',0)");
        clickAttributeTable(row);
        document.getElementById("<%=txtCode.ClientID %>").value = "";
        document.getElementById("<%=txtDesc.ClientID %>").value = "";
    }

    function Delete3Complete() {
        document.getElementById("<%=btnDelete3.ClientID%>").disabled = true;
        document.getElementById("<%=btnSave3.ClientID%>").disabled = true;

        document.getElementById("<%=txtCode.ClientID%>").value = "";
        document.getElementById("<%=txtDesc.ClientID%>").value = "";
        selectedRowIndex_Attribute == null
    }

    function clickDescTable(row) {
        //selectedRowIndex = parseInt(con.index, 10);
        if ((selectedRowIndex_Desc != null) && (selectedRowIndex_Desc != parseInt(row.index, 10))) {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex_Desc, false, "<%=gdDescription.ClientID %>");
        }

        setRowSelectedOrNotSelectedByIndex(row.index, true, "<%=gdDescription.ClientID %>");
        selectedRowIndex_Desc = parseInt(row.index, 10);


        document.getElementById("<%=hidDescDescID.ClientID%>").value = row.cells[0].innerText.trim();
        document.getElementById("<%=txtDescDesc.ClientID%>").value = row.cells[1].innerText.trim();




        //id
        if (row.cells[0].innerText.trim() == "") {
            document.getElementById("<%=btnDelete4.ClientID%>").disabled = true;
            document.getElementById("<%=btnSave4.ClientID%>").disabled = true;
        } else {
            document.getElementById("<%=btnDelete4.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave4.ClientID%>").disabled = false;
        }

    }

    function AddSave4Complete(id) {
        if (id.length == 0) return;

        var gdDescriptionClientID = "<%=gdDescription.ClientID%>";
        var row = eval("setScrollTopForGvExt_" + gdDescriptionClientID + "('" + id + "',0)");
        clickDescTable(row);
    }

    function Delete4Complete() {
        document.getElementById("<%=btnDelete4.ClientID%>").disabled = true;
        document.getElementById("<%=btnSave4.ClientID%>").disabled = true;

        document.getElementById("<%=hidDescDescID.ClientID%>").value = "";
        document.getElementById("<%=txtDescDesc.ClientID%>").value = "";
        selectedRowIndex_Desc == null
    }        
</script>
 
</asp:Content>


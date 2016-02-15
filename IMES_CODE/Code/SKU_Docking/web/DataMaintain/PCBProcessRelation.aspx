<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: model info maintain
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-11-10  98079                Create 
 //qa bug no:ITC-1281-0057
 Known issues:

 --%>

<%@ Page Language="C#" AutoEventWireup="true"  ContentType="text/html;Charset=UTF-8" CodeFile="PCBProcessRelation.aspx.cs" Inherits="PCBProcessRelation" ValidateRequest="false"  %>
   <style type ="text/css" >
        .bStyle {font-size:12pt;font-family:Verdana; font-weight:bold;}    
   </style>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>iMES-PCB Process Rule Setting</title>
    <script type="text/javascript" src=" ../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
</head>

<body style="background-color:RGB(210,210,210);position: relative; width:95%">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" >
       <center>
    <table width="90%" border=0 >
 
        <tr style="height:5px;">
            <td >
            </td>
        </tr>
        <tr style="height:25px;">
            <td align="left" colspan=3 class="bStyle">
                <asp:Label ID="lblProcessName" runat="server" CssClass="iMes_label_13pt" Font-Size="12pt"></asp:Label>
                <asp:Label ID="valProcessName" runat="server" CssClass="iMes_label_13pt" Font-Size="12pt"></asp:Label>
            </td>
        </tr>
        <tr style="height:5px;">
            <td >
            </td>
        </tr>
        <tr style="height:312px;">
            <td>		
                    <table border=0 width="100%" >
                        <tr style="height:24px">
                            <td class="bStyle">
                                <asp:Label ID="lblPCB" runat="server" CssClass="iMes_label_13pt" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height:278px">
                            <td>
                                    <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" >
                                    <ContentTemplate>
                                        <iMES:GridViewExt ID="gdPCBProcess" runat="server" AutoGenerateColumns="true" Width="100%" 
                                            GvExtWidth="100%" GvExtHeight="276px" OnRowDataBound="gdPCBProcess_RowDataBound"
                                             OnGvExtRowClick=""
                                             AutoHighlightScrollByValue ="true" HighLightRowPosition="3" SetTemplateValueEnable="False">
                                        <Columns >  
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="RowChk" runat="server"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Customer/Family/Model"/>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="PilotRunChk" runat="server"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        </iMES:GridViewExt>
                                    </ContentTemplate>   
                                    </asp:UpdatePanel>                         
                                    <input type="hidden" id="oldModelInfoName" runat="server" value=""/>
                            </td>
                        </tr>
                    </table>
            </td>
        </tr>
        <tr style="height:15px;">
            <td >
            </td>
        </tr>
        <tr style="height:35px;">
            <td align=right valign=middle colspan=3>
                <input id="btnOK" type="button"  runat="server" class="iMes_button" onclick="" onserverclick="btnOK_Click" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                <input id="btnCancel" type="button"  runat="server" class="iMes_button" onclick="onCancel()" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                <input id="btnBindDataToCBL" style="display: none" type="button"  runat="server" onserverclick="btnBindDataToCBL_Click"/>
            </td>
        </tr>        
    </table>
    </center>
    <input type="hidden" id="HiddenUserName" runat="server" />
    
    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false"> 
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="ServerClick" />
        <asp:AsyncPostBackTrigger ControlID="btnBindDataToCBL" EventName="ServerClick" />

    </Triggers>                      
   </asp:UpdatePanel>  
    
    </div> 
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div> 
    
    </form>
</body>
</html>    
   

<script type="text/javascript">
    var ObjRegion;
    var selectedRowIndex = null;
    
    
    window.onload = function()
    {
        var tHeight=document.all("container").offsetHeight+70;
        window.dialogHeight=tHeight+"px";
        ShowWait();
        //beginWaitingCoverDiv();
        document.getElementById("<%=btnBindDataToCBL.ClientID%>").click();
    }
    
    function DealHideWait()
    {
        HideWait();   
        //endWaitingCoverDiv();
    }
    function onCancel()
    {
        window.close();
    }
    
</script>

 


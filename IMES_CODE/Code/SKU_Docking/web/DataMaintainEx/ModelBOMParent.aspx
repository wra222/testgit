<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModelBOMParent.aspx.cs" Inherits="DataMaintain_ModelBOMParent" ValidateRequest="false"  %>

<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

<head id="Head1" runat="server">
    <title>iMES-查看Part上阶</title>

    <script type="text/javascript">    
      
        var isWindowLoad=false;
//        var isMessageOk=false;
//        var msgString="";
//        var currentPartNo;
      
        function btnOK_Click() 
        {
            window.returnValue = null;
            window.close();         
        }
        
        function page_load()
        {
            document.getElementById("btnParent").disabled=false;
            document.getElementById("btnAllParent").disabled=false;
            document.getElementById("btnOK").disabled=false;
        }
        
//        function ToShowMessage(msg)
//        {
//            isMessageOk=true;
//            msgString=msg;
//            DealShowMsg();
////            alert(msg);
//        }
        
//        function DealShowMsg()
//        {
//             if(isWindowLoad==true && isMessageOk==true)
//             {
//                ShowMessage(msgString);
//             }
//        }

    </script>
</head>
<body style="background-color: #ECE9D8" onload="page_load()">
    <form id="form2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="~/DataMaintain/Service/CheckItemService.asmx" />
        </Services>
    </asp:ScriptManager>

    <div style="height:2px; width: 100%;">
    </div>

    <div style="width:100%; text-align :center">
        <table style="width:94%;" >
             <tr style="height :25px">
                <td style="width:120px">
                    <asp:Label ID="lblPmt" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td style="width:80%">
                    <asp:Label ID="dCurrentPartNo" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
            </tr>
                                                                        
        </table>
         <table width="94%" border="0" >
            <tr style="height :35px">
                <td style="width: 140px; padding-left: 2px;">
                    <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td width="80%" align="right">
                </td>           
            </tr>
         </table>         
    </div>
   
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnParent" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAllParent" EventName="ServerClick" />
        </Triggers>
        <ContentTemplate>
         <div style="width:94%; text-align :center">
        <div id="div2" style="width:100%;height :342px; text-align:center; margin-left: 28px">

                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%"  RowStyle-Height="18" 
                        GvExtWidth="100%" GvExtHeight="342px" Height="326px" AutoHighlightScrollByValue ="true" HighLightRowPosition="3"                         
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false">
                    </iMES:GridViewExt>
        </div>
           </div>
        </ContentTemplate>
        </asp:UpdatePanel>  

    <div style="width: 100%; text-align:center">
        <table style="width: 94%">
        <tr style="height :60px">
        <td align="right"> 
            <button id="btnParent" runat="server" class="iMes_button"   onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnParent_ServerClick" disabled="true"></button>     
        </td>
        <td align="right">       
            <button id="btnAllParent" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAllParent_ServerClick" disabled="true"></button>     
        </td>
        <td width="60%">            
        </td>
        <td align="right">            
            <button id="btnOK" runat="server" onclick="return btnOK_Click()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="true"></button>
        </td>
         </tr>   
        </table>
    </div>

    </form>
</body>
</html>


<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: MB Label Print(SA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-11-10  liu xiao-ling        Create 
 2011-12-28   shhWang              Modified
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="LabelSetting.aspx.cs" Inherits="LabelSetting" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="content1" ContentPlaceHolderID="iMESContent" runat="server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1">
            <table class="iMes_div_MainTainListLable">
                <tr>
                    <td>
                        <asp:Label ID="lblLabelTypeList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2">
            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <iMES:GridViewExt ID="gdLabelType" runat="server" AutoGenerateColumns="true" Width="100%"
                        GvExtWidth="100%" OnRowDataBound="gdLabelType_RowDataBound" OnGvExtRowClick='if(typeof(clickLabelTypeTable)=="function") clickLabelTypeTable(this)'
                        SetTemplateValueEnable="False" HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
            <button id="btnRefreshLabelTemplateListAndTree" runat="server" type="button" style="display: none"
                onserverclick="btnRefreshLabelTemplateListAndTree_Click">
            </button>
            <asp:UpdatePanel ID="updatePanel5" runat="server" UpdateMode="Always" Visible="false">
            </asp:UpdatePanel>
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td>
                        <asp:Label ID="lblLabelType" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLabelType" MaxLength="50" runat="server" SkinID="textBoxSkin"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblPrintMode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="selPrintMode" runat="server" Width="180px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblRuleMode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="selRuleMode" runat="server" Width="90px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDescription1" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="txtDescription1" MaxLength="255" Width="100%" runat="server" SkinID="textBoxSkin"></asp:TextBox>
                    </td>
                    <td align="right">
                        <input id="btnSave1" type="button" runat="server" class="iMes_button" onclick="if(clkSave1())"
                            onserverclick="btnSaveLabelType_Click" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" />
                        <input id="btnDelete1" type="button" runat="server" class="iMes_button" onclick="if(clkDelete())"
                            onserverclick="btnDeleteLabelType_Click" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <div id="div4" style="width: 100%">
            <table>
                <tr>
                    <td style="width: 20%">
                        <table>
                            <tr>
                                <td width="60%">
                                    <asp:Label ID="lblRelatedFunction" CssClass="iMes_label_13pt" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                                <td style="height: 6px" align="right" width="40%">
                                    <input id="btnSave2" type="button" style="width: 100%" value="Save" onserverclick="btnSaveTree_Click"
                                        runat="server" class="iMes_button" onclick="clickSave2();" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="idDivSysManagement" style="height: 170px; width: 100%; overflow-y: auto;
                                        overflow-x: auto; display: block; border: solid 1px rgb(128,128,128); background-color: rgb(212,226,167);
                                        padding: 0 0 10 0; margin: 5px 0 0 0">
                                        <table id="idTable1" border="0" bordercolor="red">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Block">
                                                        <ContentTemplate>
                                                            <input id="SuccessText" type="hidden" runat="server" value="doing" />
                                                            <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="All">
                                                                <NodeStyle Width="100%" Font-Names="Verdana" Font-Size="9pt" ForeColor="black" />
                                                                <RootNodeStyle Font-Bold="True" Font-Size="9pt" />
                                                            </asp:TreeView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 80%">
                        <div id="div5">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblLabelTemplateList" runat="server" Font-Bold="true" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <input id="btnRuleSetting" type="button" runat="server" class="iMes_button" onclick="btnRuleSetting_Click()"
                                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="div6">
                            <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="gdLabelTemplate" runat="server" AutoGenerateColumns="true"
                                        Width="100%" GvExtWidth="100%" GvExtHeight="120px" Height="120px" OnRowDataBound="gdLabelTemplate_RowDataBound"
                                        OnGvExtRowClick="clickLabelTemplateTable(this)" SetTemplateValueEnable="False"
                                        HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="div7">
                            <table class="iMes_div_MainTainEdit" border=0>
                                <tr>
                                    <td width="10%">
                                        <asp:Label ID="lblTemplateName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td width="25%" colspan="2">
                                        <asp:TextBox ID="txtTemplateName" MaxLength="50" runat="server" Width="98%" SkinID="textBoxSkin"></asp:TextBox>
                                    </td>
                                    <td width="10%">
                                        <asp:Label ID="lblSPName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td width="25%" colspan="2">
                                        <asp:TextBox ID="txtSPName" MaxLength="50" runat="server" Width="95%" SkinID="textBoxSkin"></asp:TextBox>
                                    </td>
                                    <td width="10%">
                                        <input id="btnSave3" type="button" runat="server" class="iMes_button" onclick="if(clkSave3())"
                                            onserverclick="btnSaveLabelTemplate_Click" onmouseover="this.className='iMes_button_onmouseover'"
                                            onmouseout="this.className='iMes_button_onmouseout'" />
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <asp:Label ID="lblPiece" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td >
                                        <asp:TextBox ID="txtPiece" onkeypress="input1To99Number(this)" onPaste="onPaste_txtPiece();"
                                            runat="server" Width="95%" SkinID="textBoxSkin"></asp:TextBox>
                                    </td>
                                    <td >
                                        <asp:Label ID="lblLayout" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td >
                                        <asp:DropDownList ID="drpLayout" runat="server" Width="100%">
                                            <asp:ListItem Selected="True" Value="Portrait">Portrait</asp:ListItem>
                                            <asp:ListItem Value="Landscape">Landscape</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td >
                                        <asp:Label ID="lblDescription3" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                    </td>
                                    <td width="20%">
                                        <asp:TextBox ID="txtDescription3" MaxLength="255" runat="server" Width="95%" SkinID="textBoxSkin"></asp:TextBox>
                                    </td>
                                    <td width="10%">
                                        <input id="btnDelete3" type="button" runat="server" class="iMes_button" onclick="if(clkDelete())"
                                            onserverclick="btnDeleteLabelTemplate_Click" onmouseover="this.className='iMes_button_onmouseover'"
                                            onmouseout="this.className='iMes_button_onmouseout'" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" height="3px">
                                        <input type="hidden" id="hidTemplateName" runat="server" value="" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:UpdatePanel ID="updatePanelAll" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDelete3" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave3" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnRefreshLabelTemplateListAndTree" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave2" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnRefreshLabelTemplateListAndTree" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnDelete1" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave1" EventName="ServerClick" />
        </Triggers>
    </asp:UpdatePanel>
    <input type="hidden" id="dTableHeight" runat="server" />
    <input type="hidden" id="hiddenUsername" runat="server" />
    <input type="hidden" id="hidLabelType" runat="server" value="" />
    <input type="hidden" id="hidPrintMode" runat="server" value="" />
    <div id="divWait" oselectid="" align="center" style="cursor: wait; filter: Chroma(Color=skyblue);
        background-color: skyblue; display: none; top: 0; width: 100%; height: 100%;
        z-index: 10000; position: absolute">
        <table style="cursor: wait; background-color: #FFFFFF; border: 1px solid #0054B9;
            font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr>
                <td align="center">
                    <img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif" />
                </td>
                <td align="center" style="color: #0054B9; font-size: 10pt; font-weight: bold;">
                    Please wait.....
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">

        function OnCheckBoxCheckChanged(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node 
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children 
                    {
                        //check or uncheck children at all levels 
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels 
                CheckUncheckParents(src, src.checked);
            }
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked 
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked 
                }
                else //checkbox unchecked 
                {
                    checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively 
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node 
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false 
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //utility function to get the container of an element by tagname 
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;

        } 
    
    </script>

    <script type="text/javascript">
    var selectedRowIndex_LabelType = null;
    var selectedRowIndex_LabelTemplate = null;
    var ObjStation;
    var ObjPreStation;
    var bPrintTemplateMode;
    var msgDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDelete").ToString() %>';
    var msgAdd1LabelType = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAdd1LabelType").ToString() %>';
    var msgAdd3TemplateName = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAdd3TemplateName").ToString() %>';
    var msgAdd3SPName = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAdd3SPName").ToString() %>';
    var msgAdd3Piece = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgAdd3Piece").ToString() %>';

    function onPaste_txtPiece()
    {
        event.returnValue = false;
    }

    window.onload = function(){
     editor = "<%=editor%>";
        resetTableHeight();
         setNewItemValue();
        setNewItemValue1();
    }
        //设置表格的高度  
        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 300;
            var marginValue = 10;
            var tableHeigth = 150;
            //var tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
            try { 
                tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
            }
            catch (e) {

                //ignore
            }
            //为了使表格下面有写空隙
            var extDivHeight = tableHeigth + marginValue;
            div2.style.height = extDivHeight + "px";
            document.getElementById("div_<%=gdLabelType.ClientID %>").style.height = tableHeigth + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
        }
              
        function clickLabelTypeTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);
            document.getElementById("<%=btnRefreshLabelTemplateListAndTree.ClientID%>").click();
        }
        var iSelectedRowIndex = null;
           var iSelectedRowIndex1 = null;
      function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gdLabelType.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gdLabelType.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }
        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }
           document.getElementById("<%=txtLabelType.ClientID%>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=hidLabelType.ClientID%>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=selPrintMode.ClientID%>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=hidPrintMode.ClientID%>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=selRuleMode.ClientID%>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=txtDescription1.ClientID%>").value = con.cells[5].innerText.trim();
            document.getElementById("<%=btnDelete1.ClientID%>").disabled = false;

            if(con.cells[0].innerText.trim() == "0")//printmode==batch file==0
            {
                document.getElementById("<%=txtSPName.ClientID%>").disabled = "";
                document.getElementById("<%=txtSPName.ClientID%>").style.backgroundColor="white";
                bPrintTemplateMode = false;
            }
			else if(con.cells[0].innerText.trim() == "3"){
				document.getElementById("<%=txtSPName.ClientID%>").disabled = "";
                document.getElementById("<%=txtSPName.ClientID%>").style.backgroundColor="white";
				bPrintTemplateMode = true;
			}
			else if(con.cells[0].innerText.trim() == "4"){
				document.getElementById("<%=txtSPName.ClientID%>").disabled = "";
                document.getElementById("<%=txtSPName.ClientID%>").style.backgroundColor="white";
				bPrintTemplateMode = true;
			}
            else
            {
                document.getElementById("<%=txtSPName.ClientID%>").disabled = "disabled";
                //ITC-1361-0097 itc210012 2012-02-22
                document.getElementById("<%=txtSPName.ClientID%>").style.backgroundColor="gray";
                bPrintTemplateMode = true;
            }
            document.getElementById("<%=btnSave2.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave3.ClientID%>").disabled = false;
            document.getElementById("<%=btnDelete3.ClientID%>").disabled = true;
            var currentId = con.cells[2].innerText.trim();
            if (currentId == "") {
             document.getElementById("<%=txtPiece.ClientID %>").value= "1";
                setNewItemValue();
                return;
            }
            else {
           
               document.getElementById("<%=btnDelete1.ClientID %>").disabled = false;
            }
        }   
      
      
      
        
        function setNewItemValue() {
             document.getElementById("<%=txtLabelType.ClientID%>").value = "";
            document.getElementById("<%=hidLabelType.ClientID%>").value = "";
            document.getElementById("<%=selPrintMode.ClientID%>").value = "0";
            document.getElementById("<%=hidPrintMode.ClientID%>").value = "0";
            document.getElementById("<%=selRuleMode.ClientID%>").value = "0";
            document.getElementById("<%=txtDescription1.ClientID%>").value = "";
            document.getElementById("<%=btnDelete1.ClientID%>").disabled = true;
            document.getElementById("<%=btnRuleSetting.ClientID%>").disabled = true;
            document.getElementById("<%=btnSave2.ClientID%>").disabled = true;
            document.getElementById("<%=btnSave3.ClientID%>").disabled = true;
            document.getElementById("<%=btnDelete3.ClientID%>").disabled = true;
        }

   function clkDelete()
   {
      
       if(confirm(msgDelete))
       {
           ShowInfo("");
            ShowWait();
           return true;
       }   
       return false;
        
   }

   function clkSave1()
   {
       
       ShowInfo("");
       var labelType = document.getElementById("<%=txtLabelType.ClientID %>").value;   
       if(labelType.trim()=="")
       {
           alert(msgAdd1LabelType);
           DealHideWait();
           return false;
       }   
       ShowWait();
       return true;
        
   }
   
    function Add1Complete(labelType)
    {
       DealHideWait();
        if(labelType.length == 0) return;
        var gdLabelTypeClientID="<%=gdLabelType.ClientID%>";
        var row=eval("setScrollTopForGvExt_"+gdLabelTypeClientID+"('"+labelType+"',2)");
        clickLabelTypeTable(row);
    }

    function Delete1Complete()
    {
      DealHideWait();
      setNewItemValue();
    }

function clickSave2()
{
ShowWait();
}
    function clickLabelTemplateTable(con)
    {
        setGdHighLight1(con);
        ShowRowEditInfo1(con);
     }

    function setGdHighLight1(con) {
            if ((iSelectedRowIndex1 != null) && (iSelectedRowIndex1 != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex1, false, "<%=gdLabelTemplate.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gdLabelTemplate.ClientID %>");
            iSelectedRowIndex1 = parseInt(con.index, 10);
        }
        
        function ShowRowEditInfo1(con) {
            if (con == null) {
                setNewItemValue1();
                return;
            }
        document.getElementById("<%=txtTemplateName.ClientID%>").value = con.cells[0].innerText.trim();
        document.getElementById("<%=hidTemplateName.ClientID%>").value = con.cells[0].innerText.trim();
        document.getElementById("<%=txtPiece.ClientID%>").value = con.cells[1].innerText.trim();
        document.getElementById("<%=drpLayout.ClientID %>").value= con.cells[3].innerText.trim();
        if(document.getElementById("<%=txtSPName.ClientID%>").disabled == "")
        {
            document.getElementById("<%=txtSPName.ClientID%>").value = con.cells[2].innerText.trim();
        }
        document.getElementById("<%=txtDescription3.ClientID%>").value = con.cells[4].innerText.trim();            
            document.getElementById("<%=btnSave2.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave3.ClientID%>").disabled = false;
            document.getElementById("<%=btnDelete3.ClientID%>").disabled = true;
            var currentId = con.cells[0].innerText.trim();
            if (currentId == "") {
                setNewItemValue1();
                document.getElementById("<%=btnDelete3.ClientID%>").disabled = true;
                document.getElementById("<%=btnRuleSetting.ClientID%>").disabled = true;
                return;
            }
            else {
              document.getElementById("<%=btnDelete3.ClientID%>").disabled = false;
              if(bPrintTemplateMode)
              {
                document.getElementById("<%=btnRuleSetting.ClientID%>").disabled = false;
              }
            }
            
          
        }   
        
        function setNewItemValue1() {
      
        document.getElementById("<%=hidTemplateName.ClientID%>").value = "";
        document.getElementById("<%=txtTemplateName.ClientID%>").value = "";
        document.getElementById("<%=txtPiece.ClientID%>").value = "1";
        document.getElementById("<%=txtSPName.ClientID%>").value = "";
        document.getElementById("<%=txtDescription3.ClientID%>").value = "";
        document.getElementById("<%=btnRuleSetting.ClientID%>").disabled = true;
        document.getElementById("<%=btnDelete3.ClientID%>").disabled = true;
          
        }


//    function clickLabelTemplateTable(row)
//    {
//        //selectedRowIndex = parseInt(con.index, 10);
//        
//        if((selectedRowIndex_LabelTemplate!=null) && (selectedRowIndex_LabelTemplate!=parseInt(row.index, 10)))
//        {
//            setRowSelectedOrNotSelectedByIndex(selectedRowIndex_LabelTemplate,false, "<%=gdLabelTemplate.ClientID %>");                
//        }
//        
//        setRowSelectedOrNotSelectedByIndex(row.index,true, "<%=gdLabelTemplate.ClientID %>");
//        selectedRowIndex_LabelTemplate = parseInt(row.index, 10);

//        document.getElementById("<%=txtTemplateName.ClientID%>").value = row.cells[0].innerText.trim();
//        document.getElementById("<%=hidTemplateName.ClientID%>").value = row.cells[0].innerText.trim();
//        document.getElementById("<%=txtPiece.ClientID%>").value = row.cells[1].innerText.trim();
//        if(document.getElementById("<%=txtSPName.ClientID%>").disabled == "")
//        {
//            document.getElementById("<%=txtSPName.ClientID%>").value = row.cells[2].innerText.trim();
//        }
//        document.getElementById("<%=txtDescription3.ClientID%>").value = row.cells[3].innerText.trim();
//        

//        
//        //label template
//        if(row.cells[0].innerText.trim() == ""){
//            document.getElementById("<%=btnDelete3.ClientID%>").disabled = true;
//            document.getElementById("<%=btnRuleSetting.ClientID%>").disabled = true;

//        }else{
//            document.getElementById("<%=btnDelete3.ClientID%>").disabled = false;
//            if(bPrintTemplateMode)
//            {
//                document.getElementById("<%=btnRuleSetting.ClientID%>").disabled = false;
//            }
//        }
//        
//        
//    }        
    
    function clkSave3()
    {
       ShowInfo("");
       var templateName = document.getElementById("<%=txtTemplateName.ClientID %>").value;   
       var piece = document.getElementById("<%=txtPiece.ClientID %>").value;   
       var spname = document.getElementById("<%=txtSPName.ClientID %>").value;   

       if(templateName.trim()=="")
       {
           alert(msgAdd3TemplateName);
           return false;
       }   

       if(piece.trim()=="")
       {
           alert(msgAdd3Piece);
           return false;
       }   
       
       var printMode = document.getElementById("<%=hidPrintMode.ClientID%>").value;
       if(printMode == "0")//Bat
       {
           if(spname.trim()=="")
           {
               alert(msgAdd3SPName);
               return false;
           }   
       }
        ShowWait();
       return true;
        
    }
    
    
    function Add3Complete(id)
    {
        if((id == null) || (id.length == 0)) return;
    
        var selectedRowIndex=-1;
        
        var ObjGdLabelTemplate=document.getElementById("<%=gdLabelTemplate.ClientID %>");

        
        for(var i=0;i<ObjGdLabelTemplate.rows.length;i++)
        {
           if(ObjGdLabelTemplate.rows[i].cells[0].innerText==id)
           {
               selectedRowIndex=i;  
           }        
        }
        
        if(selectedRowIndex<0)
        {
            //清空所有input     
            return;
        }
        else
        {            
            var con=ObjGdLabelTemplate.rows[selectedRowIndex];
            //去掉标题行
            selectedRowIndex=selectedRowIndex-1;
            setGdHighLight1(con);
            setRowSelectedByIndex_<%=gdLabelTemplate.ClientID%>(selectedRowIndex, false, "<%=gdLabelTemplate.ClientID%>");
            setSrollByIndex(selectedRowIndex, true, "<%=gdLabelTemplate.ClientID%>");
            selectedRowIndex_LabelTemplate = selectedRowIndex;
            
            document.getElementById("<%=txtTemplateName.ClientID%>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=hidTemplateName.ClientID%>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=txtPiece.ClientID%>").value = con.cells[1].innerText.trim();
             document.getElementById("<%=drpLayout.ClientID %>").value= con.cells[3].innerText.trim();
            document.getElementById("<%=txtSPName.ClientID%>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=txtDescription3.ClientID%>").value = con.cells[4].innerText.trim();
                
            document.getElementById("<%=btnDelete3.ClientID%>").disabled = false;
            document.getElementById("<%=btnSave3.ClientID%>").disabled = false;
            //document.getElementById("<%=btnRuleSetting.ClientID%>").disabled = false;
            if(bPrintTemplateMode)
            {
                document.getElementById("<%=btnRuleSetting.ClientID%>").disabled = false;
            }
            
           
        } 
    }
    
    function Delete3Complete()
    {
     DealHideWait();
    setNewItemValue1();
    }
    
    function btnRuleSetting_Click()
    {
        var userName = document.getElementById("<%=hiddenUsername.ClientID %>").value;
        var strLabelType = document.getElementById("<%=hidLabelType.ClientID%>").value;
        var strTemplate = document.getElementById("<%=hidTemplateName.ClientID%>").value;
        var dlgFeature = "dialogHeight:470px;dialogWidth:840px;center:yes;status:no;help:no";
        var dlgReturn=window.showModalDialog("RuleSetting.aspx?LabelType="+strLabelType+"&Template="+strTemplate+"&UserName="+userName, window, dlgFeature);

    }  
    
    function DealHideWait() {
            HideWait();
        }
    </script>

</asp:Content>

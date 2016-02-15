<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Model Process Rule Set Setting 
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-04-25  Tong.Zhi-Yong         Create    
 *      
 * Known issues:
 */
 --%>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="ModelProcessRuleSetSetting.aspx.cs" Inherits="DataMaintain_ModelProcessRuleSetSetting" Title="" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <style type="text/css">
    body
    {
        background-color:RGB(210,210,210);
    }
    html
    {
      overflow:visible;
    }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div >
    <center>
    
        <table width="96%" style="table-layout:fixed;" border="0" >
            <colgroup>
                <col style="width: 60%;"/>
                <col />
                <col style="width: 105px;"/>
            </colgroup>
            <tr style="height:30px">
                <td colspan="2">
                    <asp:Label ID="lblRuleSetList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    
                </td>
            </tr>
            <tr style="height:190px">
                <td colspan="2">
                    <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>      
                             <input id="hidRowCountRuleSetList" type="hidden" runat="server" />                 
                             <iMES:GridViewExt ID="gdRuleSet" runat="server" AutoHighlightScrollByValue="true"  HighLightRowPosition="3" AutoGenerateColumns="true" Width="120%" 
                                GvExtWidth="100%" GvExtHeight="186px" Height="186px"  
                                 OnGvExtRowClick="clickRuleSetTable(this)" onrowdatabound="gdRuleSet_RowDataBound" OnDataBound="gdRuleSet_DataBound">
                            </iMES:GridViewExt>                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td valign="bottom">
                    <input type="button" id="btnUp" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" style="margin-bottom: 15px; width: 100px;" onclick="if (clkBtnUp())" disabled="true" onserverclick="btnUpDown_ServerClick"/>
                    <input type="button" id="btnDown" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" style="margin-bottom: 15px; width:100px;" disabled="true" onclick="if (clkBtnDown())" onserverclick="btnUpDown_ServerClick"/>
                    <input type="button" id="btnDelete" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="true" onclick="if (clkBtnDelete())" style="width: 100px;" onserverclick="btnDelete_ServerClick"/>
                </td>
            </tr>
            <tr style="height:30px">
                <td>
                    <asp:Label ID="lblRuleSet" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
            </tr>
            <tr style="height:190px">
                <td>
                    <div class="iMes_CheckBox" style="height: 184px; overflow-y: scroll;overflow-x: auto;" id="divCheckList">
                        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate>                      
                                <asp:CheckBoxList ID="lstChkRuleSet" runat="server" CellPadding="0" CellSpacing="0" BorderStyle="None" BorderWidth="0"></asp:CheckBoxList>
                            </ContentTemplate>
                        </asp:UpdatePanel>                      
                    </div>
                </td>
                <td valign="bottom">
                    <div><input type="button" id="btnAdd" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" style="margin-bottom: 35px; width: 100px;" onclick="if (clkAdd())" onserverclick="btnAdd_ServerClick"/></div>
                    <div>
                        <input type="button" id="btnSave" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if (clkSave())" disabled="true" style="width: 100px;" onserverclick="btnSave_ServerClick"/>
                        <input type="hidden" id="hidFld1" runat="server" />
                        <input type="hidden" id="hidConditionID" runat="server" />
                        <input type="hidden" id="hidFld2" runat="server" />
                        <input type="hidden" id="hidConditionID2" runat="server" />  
                        <input type="hidden" id="hidRuleID" runat="server" />  
                        <input type="hidden" id="hidProcess" runat="server" />  
                        <input type="hidden" id="hidtxt1" runat="server" />   
                        <input type="hidden" id="hidtxt2" runat="server" /> 
                        <input type="hidden" id="hidtxt3" runat="server" /> 
                        <input type="hidden" id="hidtxt4" runat="server" /> 
                        <input type="hidden" id="hidtxt5" runat="server" />  
                        <input type="hidden" id="HiddenUserName" runat="server" />     
                        <input type="button" id="hidBtn" style="width: 0;display:none;" runat="server" onserverclick="hidBtn_ServerClick"/>                        
                    </div>
                </td>
                <td valign="bottom">
                    <div style="margin-bottom: 35px;" >&nbsp;</div>
                    <input type="button" id="btnClose" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" style="width:100px;" onclick="closeWindow()"/>
                </td>
            </tr>
        </table>
        </center>
    </div>
    <asp:UpdatePanel ID="updatePanel" runat="server" RenderMode="Inline" Visible="false">
       <Triggers>
            <asp:AsyncPostBackTrigger ControlID="hidBtn" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnUp" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnDown" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
        </Triggers>
    </asp:UpdatePanel>
     
     <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    
    <script language="javascript" type="text/javascript">
        var emptyString = "";
        var globalAddCondition = emptyString;
        var connectSign = "+";
        //var selectedRowIndex = -1;
        var selectedRowIndex2 = -1;
        var emptyPattern = /^\s*$/;    
    
        var msgCannotMoveUp = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgCannotMoveUp").ToString() %>';    
        var msgCannotMoveDown = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgCannotMoveDown").ToString() %>';
        var msgConfirmDeleteRuleSet = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgConfirmDeleteRuleSet").ToString() %>';
        var msgSelectAtLeastOneRule = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSelectAtLeastOneRule").ToString() %>';
        var msgRuleSetCheckedNumError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgRuleSetCheckedNumError").ToString() %>';
        var msgRuleSetListDuplicateItem = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgRuleSetListDuplicateItem").ToString() %>';
    
        var isNeedRefresh=false;
        
        window.onunload =function()
        {
            window.returnValue=isNeedRefresh;        
        }
    
        function closeWindow()
        {
            window.close();
            
        }
        
        function clickRuleSetTable(con)
        {
            if((selectedRowIndex2!=null) && (selectedRowIndex2!=parseInt(con.index, 10)))
            {
                setRowSelectedOrNotSelectedByIndex(selectedRowIndex2,false, "<%=gdRuleSet.ClientID %>");                
            }
            
            setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gdRuleSet.ClientID %>");
            selectedRowIndex2=parseInt(con.index, 10);    
            
            if (hasEditDataForRuleSet())
            {
                //selectedRowIndex = -1;
                lstRuleSetChange(con); 
            }
            else
            {
                document.getElementById("<%=btnSave.ClientID %>").disabled = true; 
                document.getElementById("<%=btnUp.ClientID %>").disabled = true;
                document.getElementById("<%=btnDown.ClientID %>").disabled = true;
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;                  
                document.getElementById("<%=hidFld1.ClientID %>").value = emptyString;
                document.getElementById("<%=hidBtn.ClientID %>").click();
            }
        }
        
        function hasEditDataForRuleSet()
        {
            var tblObj = document.getElementById("<%=gdRuleSet.ClientID %>");
        
            if (selectedRowIndex2 == -1 || emptyPattern.test(tblObj.rows[selectedRowIndex2 + 1].cells[0].innerText))
            {
                return false;
            }
            
            return true;
        }
        
        function clkBtnUp()
        {
            var tblObj = document.getElementById("<%=gdRuleSet.ClientID %>");
            
            if (selectedRowIndex2 == 0)
            {
                alert(msgCannotMoveUp);
                return false;
            }
            
            //beginWaitingCoverDiv();
            isNeedRefresh=true;
            ShowWait();
            
            //do action
            var selectedText = tblObj.rows[selectedRowIndex2 + 1].cells[2].innerText;
            var selectedValue = tblObj.rows[selectedRowIndex2 + 1].cells[0].innerText;
            var selectedText2 = tblObj.rows[selectedRowIndex2].cells[2].innerText;
            var selectedValue2 = tblObj.rows[selectedRowIndex2].cells[0].innerText;         
            document.getElementById("<%=hidFld1.ClientID %>").value = selectedText;
            document.getElementById("<%=hidConditionID.ClientID %>").value = selectedValue;
            document.getElementById("<%=hidFld2.ClientID %>").value = selectedText2;
            document.getElementById("<%=hidConditionID2.ClientID %>").value = selectedValue2;
                        
            return true;
        }
        
        function clkBtnDown()
        {
            var tblObj = document.getElementById("<%=gdRuleSet.ClientID %>");
            var rowCount = document.getElementById("<%=hidRowCountRuleSetList.ClientID %>").value;
            
            if (selectedRowIndex2 + 1 == parseInt(rowCount, 10))
            {
                alert(msgCannotMoveDown);
                return false;
            }
            
            //beginWaitingCoverDiv();
            isNeedRefresh=true;
            ShowWait();
            //do action  
            var selectedText = tblObj.rows[selectedRowIndex2 + 1].cells[2].innerText;
            var selectedValue = tblObj.rows[selectedRowIndex2 + 1].cells[0].innerText;
            var selectedText2 = tblObj.rows[selectedRowIndex2 + 2].cells[2].innerText;
            var selectedValue2 = tblObj.rows[selectedRowIndex2 + 2].cells[0].innerText;    
            
            document.getElementById("<%=hidFld1.ClientID %>").value = selectedText2;
            document.getElementById("<%=hidConditionID.ClientID %>").value = selectedValue2;
            document.getElementById("<%=hidFld2.ClientID %>").value = selectedText;
            document.getElementById("<%=hidConditionID2.ClientID %>").value = selectedValue;
            
            return true;       
        }
        
        function clkBtnDelete()
        {
            if (confirm(msgConfirmDeleteRuleSet))
            {
                //beginWaitingCoverDiv();
                isNeedRefresh=true;
                ShowWait();
                //delete
                var tblObj = document.getElementById("<%=gdRuleSet.ClientID %>");
                
                document.getElementById("<%=hidConditionID.ClientID %>").value = tblObj.rows[selectedRowIndex2 + 1].cells[0].innerText;
                
                //selectedRowIndex = -1;
                //selectedRowIndex2 = -1;
                
                return true;
            }
            
            return false;
        }
        
        function clkAdd()
        {
            var chkLst = document.getElementById("<%=lstChkRuleSet.ClientID %>");
            
            if (chkLst != null)
            {
                if (!isSelectRule(chkLst))
                {
                    alert(msgSelectAtLeastOneRule);
                    return false;
                }
            
                if (!checkSelectCount(chkLst))
                {
                    alert(msgRuleSetCheckedNumError);
                    return false;
                }
                
                if (isExistInRuleSetList())
                {
                    alert(msgRuleSetListDuplicateItem);  
                    return false;          
                }
                
                //beginWaitingCoverDiv();
                isNeedRefresh=true;
                ShowWait();
                //selectedRowIndex = -1;
                //selectedRowIndex2 = -1;
                document.getElementById("<%=hidFld1.ClientID %>").value = globalAddCondition.substring(1);
                
                return true;
            }
            
            return false;
        }
        
        function isSelectRule(chkLst)
        {
            var chkboxList = chkLst.getElementsByTagName("input");
            var lstLength = chkboxList.length;
            
            for (var i = 0; i < lstLength; i++)
            {
                if (chkboxList[i].checked)
                {
                    return true;
                }
            }
            
            return false;            
        }
        
        function checkSelectCount(chkLst)
        {
            var chkboxList = chkLst.getElementsByTagName("input");
            var lblList = chkLst.getElementsByTagName("label");
            var lstLength = chkboxList.length;
            var selectedItemCount = 0;
            
            globalAddCondition = emptyString;
            
            for (var i = 0; i < lstLength; i++)
            {
                if (chkboxList[i].checked)
                {
                    selectedItemCount++;
                    globalAddCondition += (connectSign + lblList[i].innerText);
                    
                    if (selectedItemCount > 6)
                    {
                        globalAddCondition = emptyString;
                        return false;
                    }
                }
            }
            
            return true;
        }
        
        function isExistInRuleSetList()
        {
            var gdRuleSetObj = document.getElementById("<%=gdRuleSet.ClientID %>");
            var optionsLength = gdRuleSetObj.rows.length;
         
            for (var i = 0; i < optionsLength; i++) 
            {
                if (gdRuleSetObj.rows[i].cells[2].innerText == globalAddCondition.substring(1))
                {
                    return true;
                }
            }    

            return false;
        }
        
        function lstRuleSetChange(con)
        {
            var selectedText = con.cells[2].innerText;
            var selectedValue = con.cells[0].innerText;
            
            document.getElementById("divCheckList").disabled = false;
            document.getElementById("<%=btnAdd.ClientID %>").disabled = false;
            document.getElementById("<%=btnSave.ClientID %>").disabled = false; 
            document.getElementById("<%=btnUp.ClientID %>").disabled = false;
            document.getElementById("<%=btnDown.ClientID %>").disabled = false;
            document.getElementById("<%=btnDelete.ClientID %>").disabled = false;     
            document.getElementById("<%=hidFld1.ClientID %>").value = selectedText;
            document.getElementById("<%=hidConditionID.ClientID %>").value = selectedValue;
            
            document.getElementById("<%=hidBtn.ClientID %>").click();
        }
        
        function clkSave()
        {
            var chkLst = document.getElementById("<%=lstChkRuleSet.ClientID %>");
            var tblObj = document.getElementById("<%=gdRuleSet.ClientID %>");
            
            if (chkLst != null)
            {
                if (!isSelectRule(chkLst))
                {
                    alert(msgSelectAtLeastOneRule);
                    return false;
                }            
            
                if (!checkSelectCount(chkLst))
                {
                    alert(msgRuleSetCheckedNumError);
                    return false;
                }
                
                if (isExistInRuleSetList())
                {
                    if (globalAddCondition.substring(1) != tblObj.rows[selectedRowIndex2 + 1].cells[2].innerText)
                    {
                        alert(msgRuleSetListDuplicateItem);  
                        return false;                     
                    }
                }    

                //beginWaitingCoverDiv();
                isNeedRefresh=true;
                ShowWait();
                document.getElementById("<%=hidFld1.ClientID %>").value = globalAddCondition.substring(1);
                document.getElementById("<%=hidConditionID.ClientID %>").value = tblObj.rows[selectedRowIndex2 + 1].cells[0].innerText;
                
                //selectedRowIndex = -1;
                //selectedRowIndex2 = -1;      

                return true;            
            }
            
            return false;
        } 
        
        function setSroll(rowIndex)
        {
            setSrollByIndex(rowIndex,true, "<%=gdRuleSet.ClientID %>");
        }
        
        function DealHideWait()
        {
            HideWait();   
        }                                                                                                     
    </script>
</asp:Content>


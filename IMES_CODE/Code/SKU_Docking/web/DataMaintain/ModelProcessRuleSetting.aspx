<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Model Process Rule Setting
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-04-20  Tong.Zhi-Yong         Create    
 *      
 * Known issues:
 */ ITC-1361-0070 ITC-1361-0072
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ModelProcessRuleSetting.aspx.cs" Inherits="DataMaintain_ModelProcessRuleSetting" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>    
    <style>
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
    <div id="container" >
        <table width="100%" style="table-layout: fixed;" border="0">
            <colgroup>
                <col style="width: 110px;"/>
                <col style="width: 58%;"/>
                <col style="width: auto;"/>
                <col style="width: 106px;"/>
            </colgroup>
            <tr>
                <td style="height:6px" colspan="4"></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblProcessName" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Label ID="lblProcessNameValue" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <input type="button" id="btnClose" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" style="width:100px;" onclick="closeWindow()"/>
                </td>                
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRuleSetList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hidBtn2" EventName="ServerClick" />
                        </Triggers>
                        <ContentTemplate> 
                            <select id="selRuleSetList" runat="server" style="width: 98%;" onchange="changeRuleSetList(this)"></select>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td colspan="2">
                    <input type="button" id="btnSetting" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" style="width: 100px;" onclick="clkBtnSetting()"/>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblRuleList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <input type="button" id="btnDelete" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="true" onclick="if (clkBtnDeleteRule())" style="width:100px;" onserverclick="btnDeleteRule_ServerClick" />                
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>    
                            <input id="hidRecordCount" type="hidden" runat="server" />                
                            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="180%" 
                                GvExtWidth="100%" GvExtHeight="206px" Height="206px"  
                                 OnGvExtRowClick="clickTable(this)" onrowdatabound="gd_RowDataBound" OnRowCreated="gd_OnRowCreated" OnDataBound="gd_DataBound">
                            </iMES:GridViewExt>  
                        </ContentTemplate>
                    </asp:UpdatePanel>                  
                </td>
            </tr>
            <tr style="height:10px;">
            <td></td>
            </tr>            
        </table>
        <table style="border: thin solid Black;background-color: #99CDFF; table-layout: fixed;" width="100%" border="0">
            <colgroup>
                <col style="width: 90px;"/>
                <col />
                <col style="width: 90px;"/>
                <col />
                <col style="width: 105px;"/>
            </colgroup>
            <tr>
                <td>
                    <asp:Label ID="lbl1" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <input type="text" id="txt1" maxlength="200" style="width: 90%" class="iMes_textbox_input_Normal" runat="server"/>
                </td>
                <td>
                    <asp:Label ID="lbl2" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <input type="text" id="txt2" maxlength="200" style="width: 90%" class="iMes_textbox_input_Normal" runat="server"/>
                </td>
                <td rowspan="3">
                    <input type="button" id="btnAddRule" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" style="margin-bottom: 20px;" onclick="if (clkBtnAddRule())" disabled="true" onserverclick="btnAddRule_ServerClick" />
                    <input type="button" id="btnSaveRule" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if (clkBtnSaveRule())" disabled="true" onserverclick="btnSaveRule_ServerClick" /> 
                    <input type="button" id="hidBtn" style="width: 0;display:none;" runat="server" onserverclick="hidBtn_ServerClick" /> 
                    <input type="button" id="hidBtn2" style="width: 0;display:none;" runat="server" onserverclick="hidBtn2_ServerClick" />                   
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl3" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <input type="text" id="txt3" maxlength="200" style="width: 90%" class="iMes_textbox_input_Normal" runat="server"/>
                </td>
                <td>
                    <asp:Label ID="lbl4" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <input type="text" id="txt4" maxlength="200" style="width: 90%" class="iMes_textbox_input_Normal" runat="server"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl5" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <input type="text" id="txt5" maxlength="200" style="width: 90%" class="iMes_textbox_input_Normal" runat="server"/>
                </td>
                <td>
                    <asp:Label ID="lbl6" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td>
                    <input type="text" id="txt6" maxlength="200" style="width: 90%" class="iMes_textbox_input_Normal" runat="server"/>
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
                    <input type="hidden" id="hidtxt6" runat="server" />       
                    <input type="hidden" id="HiddenUserName" runat="server" />      
                    <input type="hidden" id="hidProcessInit" runat="server" />   
                </td>
            </tr>                        
        </table>
        
    <asp:UpdatePanel ID="updatePanel" runat="server" RenderMode="Inline" Visible="false">
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnAddRule" EventName="ServerClick" />   
    <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />  
    <asp:AsyncPostBackTrigger ControlID="btnSaveRule" EventName="ServerClick" /> 
    <asp:AsyncPostBackTrigger ControlID="hidBtn2" EventName="ServerClick" />
    <asp:AsyncPostBackTrigger ControlID="hidBtn" EventName="ServerClick" />
    </Triggers>   
    </asp:UpdatePanel>        
    </div>
    

    
     <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>  
    

    
    
    <script language="javascript" type="text/javascript">
        var emptyString = "";
        var globalAddCondition = emptyString;
        var connectSign = "+";
        var selectedRowIndex = -1;
        var selectedRowIndex2 = -1;
        var emptyPattern = /^\s*$/;
    
        var msgPrePleaseInput = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrePleaseInput").ToString() %>';    
        var msgConfirmDeleteRule = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgConfirmDeleteRule").ToString() %>';
        var msgRuleExist = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgRuleExist").ToString() %>';
        
        window.onload = function()
        {
            var tHeight=document.all("container").offsetHeight+100;
            window.dialogHeight=tHeight+"px";
        
            var processName =document.getElementById("<%=hidProcessInit.ClientID %>").value;
            document.getElementById("<%=lblProcessNameValue.ClientID %>").innerText = processName;
        };
        
        function changeRuleSetList(con)
        {
            var selectedText = con.options[con.selectedIndex].text;
            var selectedValue = emptyPattern.test(con.value) ? emptyString : con.value.split(",")[0];
            
            document.getElementById("<%=txt1.ClientID %>").disabled = false;
            document.getElementById("<%=txt2.ClientID %>").disabled = false;
            document.getElementById("<%=txt3.ClientID %>").disabled = false;
            document.getElementById("<%=txt4.ClientID %>").disabled = false;
            document.getElementById("<%=txt5.ClientID %>").disabled = false; 
            document.getElementById("<%=txt6.ClientID %>").disabled = false;    
            
            selectLabelAndControl(selectedText);              
            
            document.getElementById("<%=btnAddRule.ClientID %>").disabled = false;
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            document.getElementById("<%=btnSaveRule.ClientID %>").disabled = true;
            document.getElementById("<%=hidFld1.ClientID %>").value = selectedText;
            document.getElementById("<%=hidConditionID.ClientID %>").value = selectedValue;
            document.getElementById("<%=hidProcess.ClientID %>").value = document.getElementById("<%=lblProcessNameValue.ClientID %>").innerText;
            document.getElementById("<%=hidBtn.ClientID %>").click();            
        }

        function getLabelArray(isClear)
        {
            var lblArray = new Array();
            
            lblArray[0] = document.getElementById("<%=lbl1.ClientID %>");
            lblArray[1] = document.getElementById("<%=lbl2.ClientID %>");
            lblArray[2] = document.getElementById("<%=lbl3.ClientID %>");
            lblArray[3] = document.getElementById("<%=lbl4.ClientID %>");
            lblArray[4] = document.getElementById("<%=lbl5.ClientID %>");
            lblArray[5] = document.getElementById("<%=lbl6.ClientID %>");
            
            if (isClear)
            {
                var length = lblArray.length;
            
                for (var i = 0; i < length; i++)
                {
                    setLabelValueAndTip(lblArray[i], emptyString);
                }
            }
            
            return lblArray;
        }
        
        function getTxtArray(isClear)
        {
            var txtArray = new Array();
            
            txtArray[0] = document.getElementById("<%=txt1.ClientID %>");
            txtArray[1] = document.getElementById("<%=txt2.ClientID %>");
            txtArray[2] = document.getElementById("<%=txt3.ClientID %>");
            txtArray[3] = document.getElementById("<%=txt4.ClientID %>");
            txtArray[4] = document.getElementById("<%=txt5.ClientID %>");
            txtArray[5] = document.getElementById("<%=txt6.ClientID %>");
            
            if (isClear)
            {
                var length = txtArray.length;
            
                for (var i = 0; i < length; i++)
                {
                    txtArray[i].value = emptyString;
                }
            }
            
            return txtArray;            
        }
        
        function enableOrDisableEdit()
        {
            if (hasEditData())
            {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
                document.getElementById("<%=btnSaveRule.ClientID %>").disabled = false;
            }
            else 
            {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
                document.getElementById("<%=btnSaveRule.ClientID %>").disabled = true;
            }
        }         
        
        function hasEditData()
        {
            var tblObj = document.getElementById("<%=gd.ClientID %>");
        
            if (selectedRowIndex == -1 || emptyPattern.test(tblObj.rows[selectedRowIndex + 1].cells[0].innerText))
            {
                return false;
            }
            
            return true;
        }        
        
        function setLabelValueAndTip(lblIdOrObj, value, tip)
        {
            var obj;
        
            if (arguments.length == 2)
            {
                tip = value;
            }
            
            if (typeof(lblIdOrObj) == "string")
            {
                obj = document.getElementById(lblIdOrObj);
            }
            else
            {
                obj = lblIdOrObj;
            }
            
            obj.innerText = value;
            obj.title = tip;
        }        
        
        function selectLabelAndControl(selectedText)
        {
            var arrSelectText = selectedText.split(connectSign);   
            var length = arrSelectText.length;
            var lblArray = getLabelArray(true);
            var txtArray = getTxtArray(true);
            var txtArrayLength = txtArray.length;
            
            for (var i = 0; i < length; i++)
            {
                setLabelValueAndTip(lblArray[i], arrSelectText[i] + ":");
            }
            
            for (var i = length; i < txtArrayLength; i++)
            {
                txtArray[i].disabled = true;
            }       
        }         
        
        function clkBtnSetting()
        {
            var userName=document.getElementById("<%=HiddenUserName.ClientID %>").value;
        
            var strProcessName = document.getElementById("<%=lblProcessNameValue.ClientID %>").value;
//            var dlgFeature = "dialogHeight:514px;dialogWidth:900px;center:yes;status:no;help:no";
            var dlgFeature = "dialogHeight:560px;dialogWidth:900px;center:yes;";
            var dlgReturn = window.showModalDialog("ModelProcessRuleSetSetting.aspx?ProcessName=" + strProcessName+"&userName="+userName+"&UserId=&Customer=&AccountId=1&Login=", window, dlgFeature);
        
            if(dlgReturn==true)
            {
            //reload rule set list
            document.getElementById("<%=hidBtn2.ClientID %>").click();
            }
        }
        
        function clickTable(con)
        {
            if((selectedRowIndex != null) && (selectedRowIndex != parseInt(con.index, 10)))
            {
                setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, "<%=gd.ClientID %>");                
            }
            
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            selectedRowIndex = parseInt(con.index, 10);
            
            enableOrDisableEdit();
            setTextValue();
        }        
        
        function clkBtnAddRule()
        {
            if (checkInput())
            {
                if (!isInTable("A"))
                {
                     ShowWait();
                
                    //Add Rule Action                       
                    var selRuleSet = document.getElementById("<%=selRuleSetList.ClientID %>");
                    
                    document.getElementById("<%=hidConditionID.ClientID %>").value = emptyPattern.test(selRuleSet.value) ? emptyString : selRuleSet.value.split(",")[0];
                    document.getElementById("<%=hidFld1.ClientID %>").value = selRuleSet.options[selRuleSet.selectedIndex].text;                   
                    document.getElementById("<%=hidProcess.ClientID %>").value = document.getElementById("<%=lblProcessNameValue.ClientID %>").innerText;
                    document.getElementById("<%=hidtxt1.ClientID %>").value = document.getElementById("<%=txt1.ClientID %>").value;
                    document.getElementById("<%=hidtxt2.ClientID %>").value = document.getElementById("<%=txt2.ClientID %>").value;
                    document.getElementById("<%=hidtxt3.ClientID %>").value = document.getElementById("<%=txt3.ClientID %>").value;
                    document.getElementById("<%=hidtxt4.ClientID %>").value = document.getElementById("<%=txt4.ClientID %>").value;
                    document.getElementById("<%=hidtxt5.ClientID %>").value = document.getElementById("<%=txt5.ClientID %>").value;
                    document.getElementById("<%=hidtxt6.ClientID %>").value = document.getElementById("<%=txt6.ClientID %>").value;
                        
                    //selectedRowIndex = -1;
//                    getLabelArray(true);
//                    getTxtArray(true);  
                
                    return true;                
                }
                else
                {
                    alert(msgRuleExist);
                }
            }
            
            return false;
        }        
        
        function checkInput()
        {        
            //check process(Need Add code)
            var lblArray = getLabelArray(false);
            var txtArray = getTxtArray(false);
            
            for (var i = 0; i < txtArray.length; i++)
            {
                var txtObj = txtArray[i];
            
                if (!txtObj.disabled && emptyPattern.test(txtObj.value))
                {
                    alert(msgPrePleaseInput +" ["+ lblArray[i].innerText.replace(":", "]!"));
                    txtObj.value = emptyString;
                    txtObj.focus();
                    
                    return false;
                }
            }
            
            return true;
        }        
        
        function isInTable(addOrEdit)
        {
            var tblObj = document.getElementById("<%=gd.ClientID %>");
            var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;
            var columnCount = tblObj.rows[0].cells.length;
            var txtArray = getTxtArray(false);
            var tblRow;
            
            columnCount = columnCount - 4;
            
            if (addOrEdit == "A")
            {
                for (var i = 0; i < recordCount; i++)
                {
                    var inTableFlag = true; 
                    
                    tblRow = tblObj.rows[i + 1];
                
                    for (var j = 1; j < columnCount + 1; j++)
                    {
                        if (tblRow.cells[j].innerText != txtArray[j - 1].value)
                        {
                            inTableFlag = false;
                            break;
                        }                            
                    }
                    
                    if (inTableFlag)
                    {
                        return true;
                    }
                }
                
                return false;                
            }
            else
            {
                for (var i = 0; i < recordCount; i++)
                {
                    var inTableFlag = true; 
                    
                    tblRow = tblObj.rows[i + 1];
                
                    //Index need Modify=============================
                    for (var j = 1; j < columnCount + 1; j++)
                    {
                        if (tblRow.cells[j].innerText != txtArray[j - 1].value)
                        {
                            inTableFlag = false;
                            break;
                        }                            
                    }     
                    
                    if (inTableFlag && selectedRowIndex != i)
                    {
                        return true;
                    }
                }
                
                return false;                 
            }
        }        
        
        function clkBtnDeleteRule()
        {
            if (confirm(msgConfirmDeleteRule))
            {
                ShowWait();
            
                //delete
                var tblObj = document.getElementById("<%=gd.ClientID %>");
                var selRuleSet = document.getElementById("<%=selRuleSetList.ClientID %>");
                
                document.getElementById("<%=hidRuleID.ClientID %>").value = tblObj.rows[selectedRowIndex + 1].cells[0].innerText;
                document.getElementById("<%=hidConditionID.ClientID %>").value = emptyPattern.test(selRuleSet.value) ? emptyString : selRuleSet.value.split(",")[0];
                document.getElementById("<%=hidFld1.ClientID %>").value = selRuleSet.options[selRuleSet.selectedIndex].text; 
                document.getElementById("<%=hidProcess.ClientID %>").value = document.getElementById("<%=lblProcessNameValue.ClientID %>").innerText;
                //selectedRowIndex = -1;
//                getLabelArray(true);
//                getTxtArray(true);
                
                return true;
            }
            
            return false;
        }        
        
        function clkBtnSaveRule()
        {
            if (checkInput())
            {
                //Save Rule Action
                if (!isInTable("E"))
                { 
                     ShowWait();
                    
                    var selRuleSet = document.getElementById("<%=selRuleSetList.ClientID %>");
                    var tblObj = document.getElementById("<%=gd.ClientID %>");
                    
                    document.getElementById("<%=hidConditionID.ClientID %>").value = emptyPattern.test(selRuleSet.value) ? emptyString : selRuleSet.value.split(",")[0];
                    document.getElementById("<%=hidFld1.ClientID %>").value = selRuleSet.options[selRuleSet.selectedIndex].text;                   
                    document.getElementById("<%=hidProcess.ClientID %>").value = document.getElementById("<%=lblProcessNameValue.ClientID %>").innerText;
                    document.getElementById("<%=hidtxt1.ClientID %>").value = document.getElementById("<%=txt1.ClientID %>").value;
                    document.getElementById("<%=hidtxt2.ClientID %>").value = document.getElementById("<%=txt2.ClientID %>").value;
                    document.getElementById("<%=hidtxt3.ClientID %>").value = document.getElementById("<%=txt3.ClientID %>").value;
                    document.getElementById("<%=hidtxt4.ClientID %>").value = document.getElementById("<%=txt4.ClientID %>").value;
                    document.getElementById("<%=hidtxt5.ClientID %>").value = document.getElementById("<%=txt5.ClientID %>").value;                                     
                    document.getElementById("<%=hidtxt6.ClientID %>").value = document.getElementById("<%=txt6.ClientID %>").value;
                    document.getElementById("<%=hidRuleID.ClientID %>").value = tblObj.rows[selectedRowIndex + 1].cells[0].innerText;
                
                    //selectedRowIndex = -1;
//                    getLabelArray(true);
//                    getTxtArray(true);  
                
                    return true;  
                }
                else
                {
                    alert(msgRuleExist);
                }                
            }
            
            return false;
        }        
        
        function closeWindow()
        {
            window.close();
        }
        
        function setTextValue()
        {
            if (hasEditData())
            {
                var tblObj = document.getElementById("<%=gd.ClientID %>");
                var selectedRow = tblObj.rows[selectedRowIndex + 1];
                var txtArr = getTxtArray(true);
                var cellLength = selectedRow.cells.length;
                
                cellLength = (cellLength - 4);
                
                for (var i = 0; i < cellLength; i++)
                {
                    txtArr[i].value = selectedRow.cells[i + 1].innerText;
                }
            }
            else
            {
                getTxtArray(true);  
            }            
        }   
        
        function DealHideWait()
        {
            HideWait();   
            document.getElementById("<%=selRuleSetList.ClientID %>").disabled = false; 
        
        } 
        
       
        function DeleteComplete()
        {
            selectedRowIndex = -1;
        }
        
        
    </script>
</asp:Content>


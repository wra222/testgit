<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Iutput
* UI:CI-MES12-SPEC-PAK-UC PAQC Input.docx –2011/10/20 
* UC:CI-MES12-SPEC-PAK-UC PAQC Input.docx –2011/10/20            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   liuqingbiao           Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="BGAInPut.aspx.cs" Inherits="FA_BGAInput" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/SA/Service/WebServiceBGAInput.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%">
                <tr>
                    <td style="width: 20%">
                        <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                            Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                   
                </tr>
                <tr>
                    <td style="width: 20%">
                        <asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMBSnoContent" runat="server" CssClass="iMes_label_11pt" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <hr />
            
            
            <asp:Label ID="lblRepairList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td>
                            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                                GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
                                HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                            </iMES:GridViewExt>
                        </td>
                    </tr>
                </table>
           
            
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var tbl;
        var DEFAULT_ROW_NUM = 13;
        var editor = "";
        var customer = "";
        var station = "";
        var inputObj;
        var emptyPattern = /^\s*$/;
        var MBSno = "";
        var MBSno_input = "";

        var msgInputSno = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputSno") %>';
        var msgInput_translate = '<%=this.GetLocalResourceObject(Pre + "_msgInput_translate").ToString()%>';
        var msgSuccess_translate = '<%=this.GetLocalResourceObject(Pre + "_msgSuccess_translate").ToString()%>';

        window.onload = function() 
        {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            station = '<%=Request["Station"] %>';
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
        };

        window.onbeforeunload = function() 
        {
            OnCancel();
        };
        
        function initPage() 
        {
            tbl = "<%=gd.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=lblMBSnoContent.ClientID %>"), "");
            eval("setRowNonSelected_" + tbl + "()"); 
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            //reworkCount = 0;
            //reworkList = [];
            MBSno = "";
            MBSno_input = "";
        }


        function input(inputData) 
        {
            if (inputData == "") 
            {
                alert(msgInputSno);
                ShowInfo(msgInputSno);
                callNextInput();
                return;
            }
            else
            {
               ShowInfo("");
               //station = "21";
               //inputData = "LYFP0000031212";
               //MBSno_input = inputData.toString().substring(0, 10);
               MBSno_input = SubStringSN(inputData.toString(),"MB");
               beginWaitingCoverDiv();
               WebServiceBGAInput.inputSno(MBSno_input, editor, station, customer, inputSnoSucc, inputFail);
               return;
            }
        }

        function inputSnoSucc(result) 
        {
            endWaitingCoverDiv();
            setInfo(result);
            //document.getElementById("btnAddNew").disabled = "";

            WebServiceBGAInput.save(MBSno_input, saveSucc, saveFail);
            //inputData, editor, station, customer, saveSucc, saveFail);
            //callNextInput();
        }

        function saveSucc(result) 
        {
            var show_save_success_msg = "[" + MBSno_input + "] " + msgSuccess_translate;
            ExitPage();
            initPage();
            ShowSuccessfulInfo(true, show_save_success_msg);
            callNextInput();        
        }
        
        function saveFail(result)
        {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();        
        }

        function inputFail(result) 
        {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            WebServiceBGAInput.cancel(MBSno_input);
            endWaitingCoverDiv();
            callNextInput();
        }

        function setInfo(info) 
        {
            //set value to the label
            MBSno = info[1][0];
            setInputOrSpanValue(document.getElementById("<%=lblMBSnoContent.ClientID %>"), MBSno);
            setTable(info);
        }

 		function setTable(info) 
		{

		    var bomList = info[1][1];
		    //defectInTable = bomList;

		    for (var i = 0; i < bomList.length; i++) 
		    {

		        var rowArray = new Array();
		        var rw;

		        rowArray.push(bomList[i]["ReworkCode"]); //part no/name
		        rowArray.push(bomList[i]["Cdt"]);
		        rowArray.push(bomList[i]["Udt"]);

		        //add data to table
		        if (i < 12) 
		        {
		            eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
		            //setSrollByIndex(i, true, tbl);
		        }
		        else 
		        {
		            eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
		            //setSrollByIndex(i, true, tbl);
		            rw.cells[1].style.whiteSpace = "nowrap";
		        }
		    }
		}

        function OnCancel() 
        {
            if (MBSno != "") 
            {
                WebServiceBGAInput.cancel(MBSno);
            }
        }

        function callNextInput() 
        {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }

        function ExitPage() 
        {
            OnCancel();
        }

        function ResetPage() 
        {
            ExitPage();
            initPage();
            ShowInfo("");
            callNextInput();
        }

                                                                                                                               
    </script>

</asp:Content>

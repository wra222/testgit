<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for SMT Objective Time Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx –2012/7/11 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx –2012/7/11            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-7-11   Jessica Liu            Create
* Known issues:
* TODO：
* ITC-1361-0190, Jessica Liu, 2012-9-5
* ITC-1361-0191, Jessica Liu, 2012-9-6
* ITC-1361-0199, Jessica Liu, 2012-9-6
* ITC-1361-0200, Jessica Liu, 2012-9-6
* ITC-1361-0218, Jessica Liu, 2012-9-13
*/
--%>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="SMTObjectiveTime.aspx.cs" Inherits="DataMaintain_SMTObjectiveTime" ValidateRequest="false"  %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<style type="text/css">

    .iMes_div_MainTainEdit
    {
        border: thin solid Black; 
        background-color: #99CDFF;
        margin:0 0 20 0;
        
    }

    .iMes_textbox_input_Yellow
    {}

    #btnDel
    {
        width: 14px;
    }

</style>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblObTimeList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td> 
                    <td>
                        <asp:TextBox ID="ttSMTObjectiveTimeList" runat="server"   MaxLength="30"  Width="180px" 
                            CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>
                    </td>                                   
                    <td width="32%" align="right">
                        <input type="button" id="btnDel" runat="server" class="iMes_button"  onclick="if(clkDelete())" onserverclick="btnDelete_ServerClick"/>                         
                    </td>    
                </tr>
             </table>                                                    
        </div>

        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDel" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            </Triggers>
           <ContentTemplate>
           </ContentTemplate>
        </asp:UpdatePanel>
        
        <div id="div2">
              <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                <ContentTemplate>
                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                        Width="130%" RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="376px" AutoHighlightScrollByValue ="true" 
                        HighLightRowPosition="3"  
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' 
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false" style="top: -365px; left: 24px"  
                        >
                    </iMES:GridViewExt>
                 </ContentTemplate>
               </asp:UpdatePanel>   
        </div>
        
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr >
                        <td style="width: 110px;">
                            <asp:Label ID="lblLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="180px">
                            <asp:DropDownList ID="ttLine" runat="server" onchange="changeLineList()">
                            </asp:DropDownList>
                        </td>
                        
                        <td style="width: 110px;">
                            <asp:Label ID="lblObTime" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="180px">
                        <!-SkinId="textBoxSkin"->
                            <asp:TextBox ID="ttObTime" runat="server"   MaxLength="5"   Width="96%"  
                                CssClass="iMes_textbox_input_Yellow" SkinId="textBoxSkin" onkeyup="value=value.replace(/[^\d\.]/ig,'').replace(/\.(\d*\.)/ig,'$1');if(value.indexOf('.')==-1 && value.length>3)value = value.substr(0, 3);if(value.indexOf('.')>0 && value.length - value.indexOf('.') > 1)value = parseFloat(value).toFixed(1)"></asp:TextBox>
                        </td>                        
                        <td style="width: 80px;">
                            <asp:Label ID="lblHour" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        
                        <td width="110px">
                            <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="180px"></td>
                           
                        <td align="right">
                            <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnAdd_ServerClick"/>
                        </td>           
                    </tr>
                    
                    <tr >
                        <td style="width: 110px;">
                            <asp:Label ID="lblStartTime" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="180px">
                            <%-- ITC-1361-0191, Jessica Liu, 2012-9-6--%>
                            <input type="text" name="ttStartTime" id="ttStartTime" style="width:100px;" readonly="readonly" class="iMes_textbox_input_Yellow" />
                            <input id="btnCalStart" type="button" value=".." onclick="showCalendar('ttStartTime')"  style="width: 17px" class="iMes_button"  />
                        </td>
                        
                        <td style="width: 110px;">
                            <asp:Label ID="lblEndTime" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="180px"> 
                            <%-- ITC-1361-0191, Jessica Liu, 2012-9-6--%>
                            <input type="text" name="ttEndTime" id="ttEndTime" style="width:100px;" readonly="readonly" class="iMes_textbox_input_Yellow" />
                            <input id="btnCalEnd" type="button" value=".." onclick="showCalendar('ttEndTime')"  style="width: 17px" class="iMes_button"  />                           
                        </td>
                        <td style="width: 80px;"></td>
                        
                        <td width="180px" colspan="2">   
                            <%-- ITC-1361-0218, Jessica Liu, 2012-9-13--%>
                            <asp:TextBox ID="ttRemark" runat="server" MaxLength="100"  Width="96%"  CssClass="iMes_textbox_input_Yellow" style="TEXT-TRANSFORM: uppercase"></asp:TextBox>
                        </td>
                            
                        <td align="right">
                            <input type="button" id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"/>
                            <input type="hidden" id="dOldLine" runat="server" />
                            <input type="hidden" id="dTableHeight" runat="server" />
                            <input type="hidden" id="dLineCount" runat="server" />
                            <%-- ITC-1361-0191, Jessica Liu, 2012-9-6--%>
                            <input type="hidden" id="dStartTime" runat="server" />
                            <input type="hidden" id="dEndTime" runat="server" />
                            <!--<input type="hidden" id="dOldAssemblyCdt" runat="server" />-->
                        </td>           
                    </tr>                    
            </table> 
        </div>  
             
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="HiddenCustomerName" runat="server" />
        <input type="hidden" id="ttLineValue" runat="server" />
        <!--<button id="btnFamilyChange" runat="server" type="button" style="display:none"> </button>
        <button id="btnRefreshFamilyList" runat="server" type="button" onclick="" style="display: none" ></button>
        <button id="btnRefreshCustomerList" runat="server" type="button" onclick="" style="display: none" ></button>-->
   
    </div>
    
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
    </div>
    
    
    <script language="javascript" type="text/javascript" id="tableHeigth">
    //var customerObj;
    //var familyObj;
    //var descriptionObj;
    var msg1 = "";
    var msg2 = "";
    var msg3 = "";
    var msg4 = "";
    var msg5 = "";
    
    function OnKeyPress(obj)
    {
        var key = event.keyCode;
        
        if (key == 13)    //enter
        {     
            if(event.srcElement.id=="<%=ttSMTObjectiveTimeList.ClientID %>")
            {
                var value = document.getElementById("<%=ttSMTObjectiveTimeList.ClientID %>").value.trim().toUpperCase();
                if(value != "")
                {
                    findFamily(value, true);
                }
            }
        }
    }
    
    function findFamily(searchValue, isNeedPromptAlert)
    {
        var gdObj = document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex = -1;
        for(var i = 1; i < gdObj.rows.length; i++)
        {
           if(searchValue.trim() != "" && gdObj.rows[i].cells[0].innerText.toUpperCase().indexOf(searchValue) == 0)
           {
               selectedRowIndex = i;
               break;  
           }        
        }
        
        if(selectedRowIndex < 0)
        {
            if(isNeedPromptAlert == true)
            {
                //alert("Cant find that match assembly.");   //1  
                alert(msg1);     
            }
            else if(isNeedPromptAlert == null)
            {
                ShowRowEditInfo(null);
            }
            
            return;
        }
        else
        {            
            var con = gdObj.rows[selectedRowIndex];
            
            //去掉标题行
            //selectedRowIndex=selectedRowIndex-1;
            //setRowSelectedByIndex_<%=gd.ClientID%>(selectedRowIndex, false, "<%=gd.ClientID%>");
            //setSrollByIndex(selectedRowIndex, true, "<%=gd.ClientID%>");
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
        }
    }

    function changeLineList() 
    {
        var lineObj = document.all("<%=ttLine.ClientID %>");
        //ITC-1361-0190, Jessica Liu, 2012-9-5
        document.getElementById("<%=ttLineValue.ClientID %>").value = lineObj.options[lineObj.selectedIndex].value.trim();
    }
   
    window.onload = function()
    {
        //customerObj = getCustomerCmbObj();
        //customerObj.onchange = addNew;
        
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";
        document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";
        
        ShowRowEditInfo(null);
       
        resetTableHeight();
    };
    
    function resetTableHeight()
    {
        //动态调整表格的高度
        var adjustValue = 70;     
        var marginValue = 10;  
        var tableHeigth = 300;
        
        try{
            tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
        }
        catch(e){
            //ignore
        }   
                        
        //为了使表格下面有写空隙
        var extDivHeight = tableHeigth + marginValue;
       
        document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
        //alert(document.getElementById("div_<%=gd.ClientID %>").style.height)
        
        div2.style.height = extDivHeight + "px";
        document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
    }
    
    function clkDelete()
    {
        ShowInfo("");
        var gdObj = document.getElementById("<%=gd.ClientID %>")
        var curIndex = gdObj.index;
        var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;  
                  
        if(curIndex >= recordCount)
        {
            //alert("Please select one row!");   //2
            alert(msg2);
            return false;
        }
        
         //var ret = confirm("Do you really want to delete this item?");  //3
         var ret = confirm(msg3);  //3
         if (!ret) 
         {
             return false;
         }

         ShowWait();
         
         return true;        
    }
   
    function DeleteComplete()
    {   
        ShowRowEditInfo(null);
    }
   
    function clkSave()
    {
        //ITC-1361-0191, Jessica Liu, 2012-9-6
        document.getElementById("<%=dStartTime.ClientID %>").value = document.getElementById("ttStartTime").value;
        document.getElementById("<%=dEndTime.ClientID %>").value = document.getElementById("ttEndTime").value;
        
        return checkSMTLineInfo();  
    }

    function clkAdd()
    {
        //ITC-1361-0191, Jessica Liu, 2012-9-6
        document.getElementById("<%=dStartTime.ClientID %>").value = document.getElementById("ttStartTime").value;
        document.getElementById("<%=dEndTime.ClientID %>").value = document.getElementById("ttEndTime").value;
    
        return checkSMTLineInfo();
    }

    function checkSMTLineInfo()
    {
        ShowInfo("");

        var line = document.getElementById("<%=ttLineValue.ClientID %>").value;
        var obtimeObj = document.getElementById("<%=ttObTime.ClientID %>");  

        if (line.trim() == "")
        {
            alert(msg4);
            return false;
        }
        else if (obtimeObj.value.trim() == "")
        {
            alert(msg5);
            return false;
        }
        
        ShowWait();
        return true;
    }
    
    var iSelectedRowIndex = null; 
    function setGdHighLight(con)
    {
        if((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10)))
        {
            //去掉过去高亮行
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
        }    
         
        //设置当前高亮行
        setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
        
        //记住当前高亮行
        iSelectedRowIndex = parseInt(con.index, 10);    
    }

    function clickTable(con)
    {
         ShowInfo("");
         var selectedRowIndex = con.index;
         //setRowSelectedByIndex_<%=gd.ClientID%>(con.index, false, "<%=gd.ClientID%>");
         
         setGdHighLight(con);         
         ShowRowEditInfo(con);       
    }
    
    function ShowRowEditInfo(con)
    {
        //customerObj = getCustomerCmbObj();
        //customerObj.onchange = addNew;

        if (con == null) 
        {
            document.getElementById("<%=ttLine.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=ttObTime.ClientID %>").value = "";
            //ITC-1361-0191, Jessica Liu, 2012-9-5
            document.getElementById("ttStartTime").value = "<%=today%>";
            document.getElementById("ttEndTime").value = "<%=today%>";
            document.getElementById("<%=ttRemark.ClientID %>").value = "";
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;           
        }
        else 
        {
            var curLine = con.cells[0].innerText.trim();
            var lineObj = document.all("<%=ttLine.ClientID %>");
            var lineCount = parseInt(document.getElementById("<%=dLineCount.ClientID %>").value, 10);
            var i = 0;
            for (i = 0; i <= lineCount; i++)
            {
                //ITC-1361-0199, Jessica Liu, 2012-9-6
                if (lineObj.options[i].value.trim() == curLine)
                {
                    break;
                }
            }
            document.getElementById("<%=ttLine.ClientID %>").selectedIndex = i;
            //Jessica Liu, 2012-9-6
            document.getElementById("<%=ttLineValue.ClientID %>").value = curLine;

            document.getElementById("<%=ttObTime.ClientID %>").value = con.cells[1].innerText.trim();

            //ITC-1361-0200, Jessica Liu, 2012-9-6
            if (con.cells[2].innerText.trim() != "") 
            {
                document.getElementById("ttStartTime").value = con.cells[2].innerText.trim();
            }
            else 
            {
                document.getElementById("ttStartTime").value = "<%=today%>";
            }
            if (con.cells[3].innerText.trim() != "") 
            {
                document.getElementById("ttEndTime").value = con.cells[3].innerText.trim();
            }
            else 
            {
                document.getElementById("ttEndTime").value = "<%=today%>";
            }
            
            document.getElementById("<%=ttRemark.ClientID %>").value = con.cells[4].innerText.trim();

            //记录下将要被删除的Assembly的值     
            //document.getElementById("<%=dOldLine.ClientID %>").value = curAssembly;
            //document.getElementById("<%=dOldAssemblyCdt.ClientID %>").value = con.cells[7].innerText.trim();
            document.getElementById("<%=dOldLine.ClientID %>").value = con.cells[0].innerText.trim();

            if (curLine == "") 
            {
                document.getElementById("<%=btnSave.ClientID %>").disabled = true;
                document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            }
            else 
            {
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
                document.getElementById("<%=btnDel.ClientID %>").disabled = false;
            }
            
            //document.getElementById("<%=ttLine.ClientID %>").focus();
        }
    }
   
    function AddUpdateComplete(line)
    {
        var gdObj = document.getElementById("<%=gd.ClientID %>");

        var selectedRowIndex = -1;
        for(var i = 1; i < gdObj.rows.length; i++)
        {
            if (gdObj.rows[i].cells[0].innerText == line)
            {
                selectedRowIndex = i;  
            }        
        }

        if(selectedRowIndex < 0)
        {
            document.getElementById("<%=ttLine.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=ttObTime.ClientID %>").value = "";
            document.getElementById("ttStartTime").value = "<%=today%>";
            document.getElementById("ttEndTime").value = "<%=today%>";
            document.getElementById("<%=ttRemark.ClientID %>").value = "";

            document.getElementById("<%=btnSave.ClientID %>").disabled=true;   
            document.getElementById("<%=btnDel.ClientID %>").disabled=true;          
            //return;
        }
        else
        {            
            var con = gdObj.rows[selectedRowIndex];
            //去掉标题行
            //selectedRowIndex=selectedRowIndex-1;
            //setRowSelectedByIndex_<%=gd.ClientID%>(selectedRowIndex, false, "<%=gd.ClientID%>");
            ////eval("ChangeCvExtRowByIndex_"+"<%=gd.ClientID%>"+"(RowArray,"+true+", "+selectedRowIndex+")");
            //setSrollByIndex(selectedRowIndex, true, "<%=gd.ClientID%>");
            setGdHighLight(con);

            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
        }                
    }

    function disposeTree(sender, args) 
    {
        var elements = args.get_panelsUpdating();
        for (var i = elements.length - 1; i >= 0; i--) 
        {
            var element = elements[i];
            var allnodes = element.getElementsByTagName('*'),
            length = allnodes.length;
            var nodes = new Array(length)
            for (var k = 0; k < length; k++) 
            {
                nodes[k] = allnodes[k];
            }
            
            for (var j = 0, l = nodes.length; j < l; j++) 
            {
                var node = nodes[j];
                if (node.nodeType === 1) 
                {
                    if (node.dispose && typeof (node.dispose) === "function")
                    {
                        node.dispose();
                    }
                    else if (node.control && typeof (node.control.dispose)=== "function") 
                    {
                        node.control.dispose();
                    }

                    var behaviors = node._behaviors;
                    if (behaviors) 
                    {
                        behaviors = Array.apply(null, behaviors);
                        for (var k = behaviors.length - 1; k >= 0; k--) 
                        {
                            behaviors[k].dispose();
                        }
                    }
                }
            }
            element.innerHTML = "";
        }
    }
        
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(disposeTree);
    </script>
</asp:Content>


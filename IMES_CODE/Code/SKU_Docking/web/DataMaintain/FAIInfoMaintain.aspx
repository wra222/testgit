<%--   
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: FA Test Station(FA)
 * UI:CI-MES12-SPEC-FA-UI FA Test Station.docx --2011/10/20 
 * UC:CI-MES12-SPEC-FA-UC FA Test Station.docx --2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-28  Zhang.Kai-sheng       (Reference Ebook SourceCode) Create
 * TODO:
 * Known issues:
 */
 Label1 IECPN       IEC P/N
 Label2 SN          S/N
 Label3 HPQPN       HPQ P/N
 Label4 FINTime     FIN Time
 Label5 BIOSTYPE    BIOS
 Label6 KBCVer      KBC
 Label7 VDOBIOS     Video BIOS
 Label8 FDDSup      FDDSupplier
 Label9 HDDSup      HDDSupplier
 Label10 OPTSup      OPTSupplier
 Label11 NGRecord    BadRecord
 Label12 IMPRecord   Improvement 
 Label13 RECTime     REC Time 
 Label14 CHKState    CHK State
 Label15 UPCCode    UPCCode
 Label16 RAMTYPE    RAM
 Label17 BATTYPE    Battery
 --%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="FAIInfoMaintain.aspx.cs" Inherits="DataMaintain_FAIInfoMaintain" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
         <div id="div3">
         <table width="100%" >
              <tr>
                <td> <br/></td>
              </tr>
          </table>
            <table width="100%" class="iMes_div_MainTainEdit" >
                  <tr>
                        <td style="width:8%"><asp:Label ID="Label1" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox1" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width:8%"><asp:Label ID="Label2" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox2" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width:8%"><asp:Label ID="Label3" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox3" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width:8%"><asp:Label ID="Label4" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox4" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="30"></asp:TextBox>
                        </td>
                 </tr>
                 <tr>
                        <td style="width:8%"><asp:Label ID="Label5" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox5" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width:8%"><asp:Label ID="Label6" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox6" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width:8%"><asp:Label ID="Label7" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox7" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width:8%"><asp:Label ID="Label8" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox8" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                 </tr>
                 <tr>
                        <td style="width:8%"><asp:Label ID="Label9" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox9" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width:8%"><asp:Label ID="Label10" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox10" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width:8%"><asp:Label ID="Label11" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td colspan = "3"  style="width:40%">
                            <asp:TextBox ID="TextBox11" runat="server" width="98%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="100"></asp:TextBox>
                        </td>
                 </tr>
                 <tr>
                        <td style="width:9%"><asp:Label ID="Label12" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td colspan = "3" style="width:39%">
                            <asp:TextBox ID="TextBox12" runat="server" width="98%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="100"></asp:TextBox>
                        </td>
                        <td style="width:8%"><asp:Label ID="Label13" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox13" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width:9%"><asp:Label ID="Label14" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:15%">
                            <select id="CmbCHKState" name='CmbCHKState' runat="server" style="Width:99%">
                                <option value="PASS">PASS</option>
                                <option value="FAIL">FAIL</option>
                            </select> 
                        </td>
                 </tr>
                 <tr>
                        <td style="width:8%"><asp:Label ID="Label15" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox15" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width:8%"><asp:Label ID="Label16" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox16" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width:8%"><asp:Label ID="Label17" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td style="width:16%">
                            <asp:TextBox ID="TextBox17" runat="server" width="95%" SkinId="textBoxSkin"    
                                CssClass="iMes_textbox_input_Yellow" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width:8%"></td>
                        <td style="width:16%"></td>
                 </tr>
                 <tr>
                   <td style="width:8%"></td>
                   <td style="width:16%"></td>
                   <td style="width:8%"></td>
                   <td style="width:16%"></td>
                   <td style="width:8%"></td>
                   <td align="right" colspan = "3"style="width:40%">
                            <input type="button" id="Button1" runat="server"  class="iMes_button" onclick="if(clkSave())"  onserverclick="btnSave_ServerClick" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                            <input type="button" id="Button2" runat="server"  class="iMes_button" onserverclick="btnQuery_ServerClick" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                            <input type="button" id="Button3" runat="server"  class="iMes_button" onclick="btnExcel_onclick()" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                            <input type="button" id="Button4" runat="server"  class="iMes_button" onclick="onClear()"  onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                   </td>  
                 </tr>   
             </table> 
        </div>  
        <div id="div1" >
             <table width="100%">            
                <tr>
                    <td align="left">
                        <asp:Label ID="lblSetting" runat="server" CssClass="iMes_label_13pt" 
                            Font-Bold="True" Font-Size="Medium"></asp:Label>
                    </td>                                   
                </tr>
             </table>                                  
        </div>

        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
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
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false" style="top: -365px; left: 24px"   >
                    </iMES:GridViewExt>
                 </ContentTemplate>
               </asp:UpdatePanel>   
        </div>
       
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="hidhidcol" runat="server" />  
        <input type="hidden" id="HQuery" runat="server" />
        <input type="hidden" id="Hidden1" runat="server" />
        <input type="hidden" id="Hidden2" runat="server" />
        <input type="hidden" id="Hidden3" runat="server" />
        <input type="hidden" id="Hidden4" runat="server" />
        <input type="hidden" id="Hidden5" runat="server" />
        <input type="hidden" id="HOperation" runat="server" />
    </div>
       <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
        </table>
       </div>
    <script language="javascript" type="text/javascript">
 
    var msg1="";
    var msg2="";
    var msg3="";
    var msg4="";
    var msg5="";
    var msg6="";
    var msg7="";
    var msg8="";
    var msg9="";
    var msg10="";
    var msg11 = "";
    var msg12 = "";
    var fisrt = "";
    window.onload = function() {
        msg1 = "<%=pmtMessage1%>";
        msg2 = "<%=pmtMessage2%>";
        msg3 = "<%=pmtMessage3%>";
        msg4 = "<%=pmtMessage4%>";
        msg5 = "<%=pmtMessage5%>";
        msg6 = "<%=pmtMessage6 %>";
        msg7 = "<%=pmtMessage7 %>";
        msg8 = "<%=pmtMessage8 %>";
        msg9 = "<%=pmtMessage9 %>";
        msg10 = "<%=pmtMessage10 %>";
        msg11 = "<%=pmtMessage11 %>";
        msg12 = "<%=pmtMessage12 %>";
        resetTableHeight();
        if (document.getElementById("<%=this.Hidden5.ClientID%>").value == "") {
            var oDate = new Date();
            giDay = oDate.getDate();
            giMonth = oDate.getMonth() + 1;
            giYear = oDate.getYear();
            var temp = giYear + "/" + giMonth + "/" + giDay;
            document.getElementById("<%=this.TextBox4.ClientID%>").value = temp;
            document.getElementById("<%=this.TextBox13.ClientID%>").value = temp;
            document.getElementById("<%=this.CmbCHKState.ClientID%>").value = "PASS";
            document.getElementById("<%=this.Hidden5.ClientID%>").value = "1";
        }
    };
    
    function resetTableHeight()
    {
        //动态调整表格的高度
        var adjustValue=70;     
        var marginValue=10;  
        var tableHeigth=300;
        try{
            tableHeigth=document.body.parentElement.offsetHeight-div3.offsetHeight-div1.offsetHeight-adjustValue;
        }
        catch(e){
            //ignore
        }                
        //为了使表格下面有写空隙
        var extDivHeight=tableHeigth+marginValue;
       
        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
        div2.style.height=extDivHeight+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
    }

    function clkSave() {
        document.getElementById("<%=HOperation.ClientID%>").value = "";
        if (document.getElementById("<%=TextBox1.ClientID%>").value.trim() == "") {
            alert(msg4);
            return false;
        }
        if (document.getElementById("<%=TextBox4.ClientID%>").value.trim() == "") {
            alert(msg10);
            return false;
        }
        var flag = CheckInTable(document.getElementById("<%=TextBox1.ClientID%>").value.trim());
        if (flag == true) {
            document.getElementById("<%=HOperation.ClientID%>").value = "save";
        }
        else {
            document.getElementById("<%=HOperation.ClientID%>").value = "add";
        }
        return true;  
    }

    function CheckInTable(sIECPN) {
        var tbl = "<%=gd.ClientID %>";
        var table = document.getElementById(tbl);
        var subFindFlag = false;
        for (var i = 1; i < table.rows.length; i++) {
            if (table.rows[i].cells[0].innerText.trim() == sIECPN)  {
                subFindFlag = true;
                break;
            }
        }
        return subFindFlag;
    }
    function checkAdaptorInfo(){
        ShowInfo("");
       
        return true;
    }
   
    var iSelectedRowIndex=null; 
    function setGdHighLight(con)
    {
        if((iSelectedRowIndex!=null) && (iSelectedRowIndex!=parseInt(con.index, 10)))
        {
            //去掉过去高亮行
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex,false, "<%=gd.ClientID %>");                
        }     
        //设置当前高亮行   
        setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
        //记住当前高亮行
        iSelectedRowIndex=parseInt(con.index, 10);    
    }

   
    function clickTable(con)
    {
         ShowInfo("");
         var selectedRowIndex = con.index;
         setGdHighLight(con);         
         ShowRowEditInfo(con);   
    }
    
    

    function SaveUpdateComplete(id) {

        var gdObj = document.getElementById("<%=gd.ClientID %>");

        var selectedRowIndex = -1;
        for (var i = 1; i < gdObj.rows.length; i++) {
            if (gdObj.rows[i].cells[0].innerText == id) {
                selectedRowIndex = i;
            }
        }
        if (selectedRowIndex < 0) {
            //document.getElementById("<%=Button1.ClientID %>").disabled = true;
            document.getElementById("<%=Button2.ClientID %>").click();
            return;
        }
        else {
            var con = gdObj.rows[selectedRowIndex];
            //去掉标题行
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            return;
        }
    }
    function QueryUpdateComplete(id) {

        var gdObj = document.getElementById("<%=gd.ClientID %>");

        var selectedRowIndex = -1;
        for (var i = 1; i < gdObj.rows.length; i++) {
            if (gdObj.rows[i].cells[0].innerText == id) {
                selectedRowIndex = i;
            }
        }
        if (selectedRowIndex < 0) {
            document.getElementById("<%=Button1.ClientID %>").disabled = true;
            return;
        }
        else {
            var con = gdObj.rows[selectedRowIndex];
            //去掉标题行
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            return;
        }
    }
    function AddUpdateComplete(id)
    {
        var gdObj = document.getElementById("<%=gd.ClientID %>");
        var selectedRowIndex = -1;
        for (var i = 1; i < gdObj.rows.length; i++) {
            if (gdObj.rows[i].cells[0].innerText == id) {
                selectedRowIndex = i;
            }
        }
        if (selectedRowIndex < 0) {
            document.getElementById("<%=Button2.ClientID %>").click(); 
            return;
        }
        else {
            var con = gdObj.rows[selectedRowIndex];
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
        }
    }

    function onClear() {
        document.getElementById("<%=this.TextBox1.ClientID%>").value = "";
        document.getElementById("<%=this.TextBox2.ClientID%>").value = "";
        document.getElementById("<%=this.TextBox3.ClientID%>").value = "";

        var oDate = new Date();
        giDay = oDate.getDate();
        giMonth = oDate.getMonth() + 1;
        giYear = oDate.getYear();
        var temp = giYear + "/" + giMonth + "/" + giDay;
        document.getElementById("<%=this.TextBox4.ClientID%>").value = temp;
        document.getElementById("<%=this.TextBox5.ClientID%>").value = "";
        document.getElementById("<%=this.TextBox6.ClientID%>").value = "";
        document.getElementById("<%=this.TextBox7.ClientID%>").value = "";
        document.getElementById("<%=this.TextBox8.ClientID%>").value = "";
        document.getElementById("<%=this.TextBox9.ClientID%>").value = "";
        document.getElementById("<%=this.TextBox10.ClientID%>").value = "";
        document.getElementById("<%=this.TextBox11.ClientID%>").value = "";
        document.getElementById("<%=this.TextBox12.ClientID%>").value = "";
        document.getElementById("<%=this.TextBox13.ClientID%>").value = temp;
        document.getElementById("<%=this.TextBox15.ClientID%>").value = "";
        document.getElementById("<%=this.TextBox16.ClientID%>").value = "";
        document.getElementById("<%=this.TextBox17.ClientID%>").value = "";
        document.getElementById("<%=this.CmbCHKState.ClientID%>").value = "PASS";
        document.getElementById("<%=this.HQuery.ClientID%>").value = "";
        
    }
    function ShowRowEditInfo(con) {
        if (con == null) {
            document.getElementById("<%=Button1.ClientID %>").disabled = true;
            return;
        }
        

        document.getElementById("<%=this.TextBox1.ClientID%>").value = con.cells[0].innerText.trim();
        document.getElementById("<%=this.TextBox2.ClientID%>").value = con.cells[1].innerText.trim();
        document.getElementById("<%=this.TextBox3.ClientID%>").value = con.cells[2].innerText.trim();
         var demo = con.cells[3].innerText.trim();
         while(demo.indexOf("-") != -1){
             demo=demo.replace("-", "/");
        }

        document.getElementById("<%=this.TextBox4.ClientID%>").value = demo; 
        document.getElementById("<%=this.TextBox5.ClientID%>").value = con.cells[4].innerText.trim();
        document.getElementById("<%=this.TextBox6.ClientID%>").value = con.cells[5].innerText.trim();
        document.getElementById("<%=this.TextBox7.ClientID%>").value = con.cells[6].innerText.trim();
        document.getElementById("<%=this.TextBox8.ClientID%>").value = con.cells[7].innerText.trim();
        document.getElementById("<%=this.TextBox9.ClientID%>").value = con.cells[8].innerText.trim();
        document.getElementById("<%=this.TextBox10.ClientID%>").value = con.cells[9].innerText.trim();
        document.getElementById("<%=this.TextBox11.ClientID%>").value = con.cells[10].innerText.trim();
        document.getElementById("<%=this.TextBox12.ClientID%>").value = con.cells[11].innerText.trim();
        demo = con.cells[12].innerText.trim();
         while(demo.indexOf("-") != -1)
         {
             demo=demo.replace("-", "/");
        }
        document.getElementById("<%=this.TextBox13.ClientID%>").value = demo ;
        document.getElementById("<%=this.TextBox15.ClientID%>").value = con.cells[14].innerText.trim();
        document.getElementById("<%=this.TextBox16.ClientID%>").value = con.cells[15].innerText.trim();
        document.getElementById("<%=this.TextBox17.ClientID%>").value = con.cells[16].innerText.trim();
        if (con.cells[13].innerText.trim() == "PASS") {
            document.getElementById("<%=this.CmbCHKState.ClientID%>").value = "PASS";
        }
        else {
            document.getElementById("<%=this.CmbCHKState.ClientID%>").value = "FAIL";
        }
        
        
        if (con.cells[0].innerText.trim() == "" && con.cells[1].innerText.trim() == "" && con.cells[4].innerText.trim() == "") {
            document.getElementById("<%=Button1.ClientID %>").disabled = true;
        }
        else {
            document.getElementById("<%=Button1.ClientID %>").disabled = false;
        }
    }
    function btnExcel_onclick() {
        var strTitle = "";
        var dgData = document.getElementById("<%=gd.ClientID%>");
        var iStartCol = 0;
        var iEndCol = 19;
        // 定义Excel Applicaiton Object
        var appExcel = null;
        // 当前激活的工作簿
        var currentWork = null;
        var currentSheet = null;
        try {
            // 初始化application
            appExcel = new ActiveXObject("Excel.Application");
            appExcel.Visible = false;
            appExcel.DisplayFullScreen = true;
        }
        catch (e) {
            window.alert("Please Install Excel First");
            return;
        }
        
        // 获取当前激活的工作部
        currentWork = appExcel.Workbooks.Add();
        currentSheet = currentWork.ActiveSheet;
        currentSheet.Columns('A:T').ColumnWidth = 17;

        // 填充excel内容
        // 设置标题
        //currentSheet.Cells(1,1).Value = strTitle;
        // 填充内容
        for (var iRow = 0; iRow <= dgData.rows.length - 1; iRow++) {
            // 显示指定列的内容
            for (var iCol = iStartCol; iCol <= iEndCol; iCol++) {
                currentSheet.Cells(iRow + 1, iCol + 1).NumberFormatLocal = "@ "; 
                currentSheet.Cells(iRow + 1, iCol + 1).Value = dgData.rows[iRow].cells[iCol].innerText;
            }           
        }

        appExcel.Visible = true;
        return;
    }
    function onTextBox1KeyDown() {
        document.getElementById("<%=Button1.ClientID %>").disabled = false;
        if (document.getElementById("<%=this.CmbCHKState.ClientID%>").value == "") {
            document.getElementById("<%=this.CmbCHKState.ClientID%>").value = "PASS";
        }
    }
    function disposeTree(sender, args) {
            var elements = args.get_panelsUpdating();
            for (var i = elements.length - 1; i >= 0; i--) {
                var element = elements[i];
                var allnodes = element.getElementsByTagName('*'),
                length = allnodes.length;
                var nodes = new Array(length)
                for (var k = 0; k < length; k++) {
                    nodes[k] = allnodes[k];
                }
                for (var j = 0, l = nodes.length; j < l; j++) {
                    var node = nodes[j];
                    if (node.nodeType === 1) {
                        if (node.dispose && typeof (node.dispose) === "function") {
                            node.dispose();
                        }
                        else if (node.control && typeof (node.control.dispose)=== "function") {
                            node.control.dispose();
                        }

                        var behaviors = node._behaviors;
                        if (behaviors) {
                            behaviors = Array.apply(null, behaviors);
                            for (var k = behaviors.length - 1; k >= 0; k--) {
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


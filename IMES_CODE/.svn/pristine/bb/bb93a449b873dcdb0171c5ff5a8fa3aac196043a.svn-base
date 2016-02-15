<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="FamilyMBCode.aspx.cs" Inherits="DataMaintain_FamilyMBCode" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
             <table width="100%" class="">            
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblSetting" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server"  Height="25px" BackColor="#ffffa0" BorderColor="Brown"   Font-Bold="true" Font-Size="X-Large" ForeColor="Red" style="width:45%;text-transform:none "    />
           
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
                
             <tr>    
                    <td style="width:8%;padding-left:2px;">
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td  width="35%">
                        <asp:TextBox ID="TxtFamily" runat="server" Width="96%" SkinId="textBoxSkin"  MaxLength="50"  CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>                    
                    </td> 
                    <td width="10%">
                        <asp:Label ID="lblMBCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td  width="35%">
                        <asp:TextBox ID="txtMBCode" runat="server" width="95%" SkinId="textBoxSkin"  MaxLength="50"  CssClass="iMes_textbox_input_Yellow" ></asp:TextBox>                                                 
                    </td>                         
                                           
                    <td align="right">
                         <input type="button" id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnAdd_ServerClick"/>
                    </td>           
                </tr>
                <tr>

                    <td style="width: 70px;">
                        <asp:Label ID="lblDescr" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3" >                            
                         <asp:TextBox ID="txtDescr" runat="server" Width="98%" SkinId="textBoxSkin" MaxLength="100"  CssClass="iMes_textbox_input_Yellow" ></asp:TextBox> 
                    </td>                        
                    
                    <td align="right">
                        <input type="button" id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"/>
                    </td>           
                </tr>     
            </table> 
        </div>  
       
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="hidhidcol" runat="server" />  
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="HFamily" runat="server" />
        <input type="hidden" id="HMBCode" runat="server" />
        <input type="hidden" id="HDescr" runat="server" />
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
    var txtObj;
    window.onload = function()
    {
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";
        msg6="<%=pmtMessage6 %>";
        msg7="<%=pmtMessage7 %>";
        msg8="<%=pmtMessage8 %>";
        msg9="<%=pmtMessage9 %>";
        msg10 = "<%=pmtMessage10 %>";
        msg11 = "<%=pmtMessage11 %>";
        msg12 = "<%=pmtMessage12 %>";
        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        resetTableHeight();
        txtObj = document.getElementById("<%=TextBox1.ClientID%>"); 
        callNextInput();
    };
    function onTextBox1KeyDown() {
        if (event.keyCode == 9 || event.keyCode == 13) {
            ShowInfo("");
            txtObj = document.getElementById("<%=TextBox1.ClientID%>");
            var inputTextBox1 = txtObj.value;
            inputTextBox1 = inputTextBox1.trim();
            if (inputTextBox1 != "") {
                CheckInTableByFamily(inputTextBox1);
            }
            event.cancel = true;
            event.returnValue = false;
            callNextInput();
        }
    }
    function callNextInput() {
        txtObj = document.getElementById("<%=TextBox1.ClientID%>");
        txtObj.value = "";
        txtObj.focus();
    }
    function resetTableHeight()
    {
        //动态调整表格的高度
        var adjustValue=70;     
        var marginValue=10;  
        var tableHeigth=300;
        try{
            tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
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
    
    
  
    
    function clkDelete()
    {
        ShowInfo("");
        if (document.getElementById("<%=HFamily.ClientID %>").value.trim() == "" &&
            document.getElementById("<%=HMBCode.ClientID %>").value.trim() == "") {
            alert(msg5);
            return false;
        }
        var ret = confirm(msg3);
        if (!ret) {
            return false;
        }
        ShowWait();
        return true;
    }
   
    function DeleteComplete()
    {   
        ShowRowEditInfo("del");
    }
   
    function clkSave() {
        if (document.getElementById("<%=this.TxtFamily.ClientID%>").value.trim() == "")
        {
            alert(msg4);
            return false;
        }
        if (document.getElementById("<%=this.txtMBCode.ClientID%>").value.trim() == "") 
        {
            alert(msg10);
            return false;
        }
        var flag = CheckInTable(document.getElementById("<%=this.TxtFamily.ClientID%>").value,
            document.getElementById("<%=this.txtMBCode.ClientID%>").value);
        if (flag == true) {
            if (document.getElementById("<%=this.TxtFamily.ClientID%>").value.trim() == document.getElementById("<%=HFamily.ClientID %>").value.trim() &&
            document.getElementById("<%=this.txtMBCode.ClientID%>").value.trim() == document.getElementById("<%=HMBCode.ClientID %>").value.trim()) {
            }
            else {
                alert(msg12);
                return false;
            }
        }
        return true;  
    }
   
    function clkAdd()
    {
        if (document.getElementById("<%=this.TxtFamily.ClientID%>").value.trim() == "") {
            alert(msg4);
            return false;
        }
        if (document.getElementById("<%=this.txtMBCode.ClientID%>").value.trim() == "") {
            alert(msg10);
            return false;
        }
        var flag = CheckInTable(document.getElementById("<%=this.TxtFamily.ClientID%>").value,
            document.getElementById("<%=this.txtMBCode.ClientID%>").value);
        if (flag == true) {
            alert(msg11);
            return false;
        }
        return true;  
    }
    function CheckInTable(sFamily,sMBCode) {
        var tbl = "<%=gd.ClientID %>";
        var table = document.getElementById(tbl);
        var subFindFlag = false;
        for (var i = 1; i < table.rows.length; i++) {
            if ((table.rows[i].cells[0].innerText.trim() == sFamily) &&
                    (table.rows[i].cells[1].innerText.trim() == sMBCode))  {
                subFindFlag = true;
                break;
            }
        }
        return subFindFlag;
    }
    var iSelectedRowIndex=null; 
    function CheckInTableByFamily(Family) {
        var tbl = "<%=gd.ClientID %>";
        var table = document.getElementById(tbl);

        var selectedRowIndex = -1;
        for (var i = 1; i < table.rows.length; i++) {
            if (table.rows[i].cells[0].innerText.trim() == Family)
            {
                selectedRowIndex = i;
                var con = table.rows[selectedRowIndex];
                //去掉标题行
                setGdHighLight(con);
                ShowRowEditInfo(con);   
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                break;
            }
        }
    }
    function checkAdaptorInfo(){
        ShowInfo("");
        return true;
    }
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
            if (gdObj.rows[i].cells[6].innerText == id) {
                selectedRowIndex = i;
            }
        }

        if (selectedRowIndex < 0) {
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            return;
        }
        else {
            var con = gdObj.rows[selectedRowIndex];
            //去掉标题行
            setGdHighLight(con);
            setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
        }
    }
   
    function AddUpdateComplete(condition)
    {
        condition = condition.trim();
        var gdObj = document.getElementById("<%=gd.ClientID %>"); 
        var selectedRowIndex = -1;
        selectedRowIndex = parseInt(document.getElementById("<%=hidRecordCount.ClientID %>").value, 10); 
        if(selectedRowIndex < 0)
        {
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            return;
        }
        else
        {
            var tbl = "<%=gd.ClientID %>";
            var table = document.getElementById(tbl);
            var index = 1;
            for (var i = 1; i < table.rows.length; i++) {
                var temp = table.rows[i].cells[0].innerText.trim() + table.rows[i].cells[1].innerText.trim() ;
                temp = temp.trim();
                if (temp == condition) {
                    index = i;
                    break;
                }
            }
            var con = gdObj.rows[index];
            //去掉标题行
            setGdHighLight(con);
            setSrollByIndex(index - 1, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
        }        
    }
    function ShowRowEditInfo(con) {
        if (con == null) {
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            return;
        }
        if (con == "del") {
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
            document.getElementById("<%=this.TxtFamily.ClientID%>").value = "";
            document.getElementById("<%=this.txtMBCode.ClientID%>").value = "";
            return;
        }
        
        document.getElementById("<%=this.TxtFamily.ClientID%>").value = con.cells[0].innerText.trim();
        document.getElementById("<%=this.txtMBCode.ClientID%>").value = con.cells[1].innerText.trim();
        document.getElementById("<%=this.txtDescr.ClientID%>").value = con.cells[2].innerText.trim();
        document.getElementById("<%=hidhidcol.ClientID %>").value = con.cells[0].innerText.trim() + con.cells[1].innerText.trim();
        document.getElementById("<%=HFamily.ClientID %>").value = con.cells[0].innerText.trim();
        document.getElementById("<%=HMBCode.ClientID %>").value = con.cells[1].innerText.trim();
        document.getElementById("<%=HDescr.ClientID %>").value = con.cells[2].innerText.trim();
        
        
        if (con.cells[0].innerText.trim() == "" && con.cells[1].innerText.trim() == "" ) {
            document.getElementById("<%=btnDel.ClientID %>").disabled = true;
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }
        else {
            document.getElementById("<%=btnDel.ClientID %>").disabled = false;
            document.getElementById("<%=btnSave.ClientID %>").disabled = false;
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


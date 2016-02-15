<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="OfflineLabelPrint.aspx.cs" Inherits="FA_OfflineLabelPrint" Title="Untitled Page"%>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceOfflineLabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
    <tr>
        <td align="left" style="width: 12%" >
            <asp:Label ID="lbFilePath" runat="server"  CssClass="iMes_label_13pt"></asp:Label>
        </td>
        <td align="left" style="width: 66%">
            <asp:TextBox ID="txtProductID" runat="server"  
                Width="99%" IsClear="true" ReadOnly="false" style="text-transform:none" />
        </td>
    </tr>
    <tr>
	    <td style="width:12%" align="left" ><asp:Label ID="lbLabelName" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td align="left" style="width: 66%"><iMES:CmbSamplea ID="cmbLabelName" runat="server"  Width="326" IsPercentage="true" /></td>
    </tr>
    <tr>
	    <td style="width:12%" align="left"><asp:Label ID="lbLabelSpec" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td align="left" style="width: 66%"><asp:Label ID="txtLabelSpec" runat="server" CssClass="iMes_label_13pt"/></td>
    </tr>          
    <tr>
	    <td style="width:12%" align="left"><asp:Label ID="lbFileName" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
	    <td align="left" style="width: 66%"><asp:Label ID="txtFileName" runat="server" CssClass="iMes_label_13pt"/></td>
    </tr>    
    </table>
    </center>
    <tr>
    <td align="left">
        <hr />
    </td>
    </tr>
    <center>
    <table border="0" width="95%">
    <tr>
        <td style="width:12%" align="left"><asp:Label ID="lbParameter1" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td align="left" style="width: 66%">
            <asp:TextBox ID="TextBox1" runat="server"  
                             Width="99%" IsClear="false" ReadOnly="false" style="text-transform:none" />
        </td>
    </tr>
    <tr>        
        <td style="width:12%" align="left"><asp:Label ID="lbParameter2" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td align="left" style="width: 66%">
            <asp:TextBox ID="TextBox2" runat="server"  
                             Width="99%" IsClear="false" ReadOnly="false" style="text-transform:none" />
        </td>
    </tr>
    <tr>        
        <td style="width:12%" align="left"><asp:Label ID="lbParameter3" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td align="left" style="width: 66%">
            <asp:TextBox ID="TextBox3" runat="server"  
                             Width="99%" IsClear="false" ReadOnly="false" style="text-transform:none" />
        </td>
    </tr>
    <tr>        
        <td style="width:12%" align="left"><asp:Label ID="lbParameter4" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td align="left" style="width: 66%">
            <asp:TextBox ID="TextBox4" runat="server"  
                             Width="99%" IsClear="false" ReadOnly="false" style="text-transform:none" />
        </td>
    </tr>
    <tr>        
        <td style="width:12%" align="left"><asp:Label ID="lbParameter5" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td align="left" style="width: 66%">
            <asp:TextBox ID="TextBox5" runat="server"  
                             Width="99%" IsClear="false" ReadOnly="false" style="text-transform:none" />
        </td>
    </tr>
    <tr>        
        <td style="width:12%" align="left"><asp:Label ID="lbParameter6" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td align="left" style="width: 66%">
            <asp:TextBox ID="TextBox6" runat="server"  
                             Width="99%" IsClear="false" ReadOnly="false" style="text-transform:none" />
        </td>
    </tr>
    <tr>        
        <td style="width:12%" align="left"><asp:Label ID="lbParameter7" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td align="left" style="width: 66%">
            <asp:TextBox ID="TextBox7" runat="server"  
                             Width="99%" IsClear="false" ReadOnly="false" style="text-transform:none" />
        </td>
    </tr>
    <tr>        
        <td style="width:12%" align="left"><asp:Label ID="lbParameter8" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td align="left" style="width: 66%">
            <asp:TextBox ID="TextBox8" runat="server"  
                             Width="99%" IsClear="false" ReadOnly="false" style="text-transform:none" />
        </td>                                                        
    </tr> 
    </table>
    <tr>
	    <td align="right">&nbsp;<input id="btnPrint" type="button"  runat="server" 
                onclick="print()" class="iMes_button" 
                onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" visible="True"/>
        </td>
    </tr>    
    <td align="left" style="width: 12%" >
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <button id="btnHidden" runat="server" onserverclick="btnHidden_Click" style="display: none" >
                    </button> 
                    <button id="btnHidden2" runat="server" onserverclick="btnHidden2_Click" style="display: none" >
                    </button> 
                    <button id="btnHid_en_drpdownList" runat="server" onserverclick="btnHid_en_drpdownList_Click" style="display: none" >
                    </button> 
                    <input type="hidden" runat="server" id="station" />
                    <input type="hidden" runat="server" id="prodID" />
                    <input type="hidden" runat="server" id="customerSn" />
                    <input type="hidden" runat="server" id="model" />
                    <input type="hidden" runat="server" id="Tranfer_data" />
                </ContentTemplate>   
            </asp:UpdatePanel> 
    </td>    
    </center>
</div>


<script type="text/javascript">
    var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgInputPathFirstErrorTip = '<%=this.GetLocalResourceObject(Pre + "_msgInputPathFirstErrorTip").ToString()%>';
    var msgPrintReturnError = '<%=this.GetLocalResourceObject(Pre + "_msgPrintReturnError").ToString()%>';


    window.onload = function() {
        try {
            Refresh(",,,,,,,,,");
            document.getElementById("<%=txtProductID.ClientID %>").focus();
        }
        catch (e) {
            alert(e.description);
        }
    };

    window.onbeforeunload = function() 
    {
        OnCancel();
    };
        
    function OnCancel()
    {
    }

    function check() {
        //keyCode是event事件的属性,对应键盘上的按键,回车键是13,tab键是9,其它的如果不知道 ,查keyCode大全
        if ((event.keyCode == 13) &&
            ((event.srcElement.type == "text") ||
            ((event.srcElement.type == "select-one") && (event.srcElement.value != '') && (event.srcElement.value != ' ')))) {
            //srcElement是触发事件的对象,type意思是什么类型
            event.keyCode = 9;
        }
    }
    //modify ITC-1360-1296 bug
    //document.onkeydown = check;

    function Refresh(str) 
    {
        var LinesArray = str.split(",");

        if (LinesArray.length >= 1) {
            var _txtLabelSpec = document.getElementById("<%=txtLabelSpec.ClientID %>");
            if (LinesArray[0] == "") {
                if (_txtLabelSpec != null) _txtLabelSpec.innerText = "";
            }
            else {
                if (_txtLabelSpec != null) { _txtLabelSpec.innerText = LinesArray[0]; }
            }
        }

        if (LinesArray.length >= 2) {
            var _txtFileName = document.getElementById("<%=txtFileName.ClientID %>");
            if (LinesArray[1] == "") {
                if (_txtFileName != null) _txtFileName.innerText = "";
            }
            else {
                if (_txtFileName != null) { _txtFileName.innerText = LinesArray[1]; }
            }
        }
        
        
        
        if (LinesArray.length >= 3) 
        {
            var _lbParameter1 = document.getElementById("<%=lbParameter1.ClientID %>");
            var __tb1 = document.getElementById("<%=TextBox1.ClientID %>");

            if (LinesArray[2] == "") 
            {
                if (__tb1 != null) { __tb1.style.visibility = "hidden"; setInputOrSpanValue(__tb1, ""); }
                if (_lbParameter1 != null) _lbParameter1.innerText = "";
            }
            else {
                if (__tb1 != null) 
                {
                    __tb1.style.visibility = "visible"; setInputOrSpanValue(__tb1, "");
                }
                if (_lbParameter1 != null) { _lbParameter1.innerText = LinesArray[2]; }
            }
        }

        if (LinesArray.length >= 4) 
        {
            var _lbParameter2 = document.getElementById("<%=lbParameter2.ClientID %>");
            var __tb2 = document.getElementById("<%=TextBox2.ClientID %>");
            if (LinesArray[3] == "") 
            {
                if (__tb2 != null) { __tb2.style.visibility = "hidden"; setInputOrSpanValue(__tb2, ""); }
                if (_lbParameter2 != null) _lbParameter2.innerText = "";
            }
            else 
            {
                if (__tb2 != null) { __tb2.style.visibility = "visible"; setInputOrSpanValue(__tb2, ""); }
                if (_lbParameter2 != null) { _lbParameter2.innerText = LinesArray[3]; }
            }
        }

        if (LinesArray.length >= 5) {
            var _lbParameter3 = document.getElementById("<%=lbParameter3.ClientID %>");
            var __tb3 = document.getElementById("<%=TextBox3.ClientID %>");
            if (LinesArray[4] == "") {
                if (__tb3 != null) { __tb3.style.visibility = "hidden"; setInputOrSpanValue(__tb3, ""); }
                if (_lbParameter3 != null) _lbParameter3.innerText = "";
            }
            else {
                if (__tb3 != null) { __tb3.style.visibility = "visible"; setInputOrSpanValue(__tb3, ""); }
                if (_lbParameter3 != null) { _lbParameter3.innerText = LinesArray[4]; }
            }
        }

        if (LinesArray.length >= 6) {
            var _lbParameter4 = document.getElementById("<%=lbParameter4.ClientID %>");
            var __tb4 = document.getElementById("<%=TextBox4.ClientID %>");
            if (LinesArray[5] == "") {
                if (__tb4 != null) { __tb4.style.visibility = "hidden"; setInputOrSpanValue(__tb4, ""); }
                if (_lbParameter4 != null) _lbParameter4.innerText = "";
            }
            else {
                if (__tb4 != null) { __tb4.style.visibility = "visible"; setInputOrSpanValue(__tb4, ""); }
                if (_lbParameter4 != null) { _lbParameter4.innerText = LinesArray[5]; }
            }
        }

        if (LinesArray.length >= 7) {
            var _lbParameter5 = document.getElementById("<%=lbParameter5.ClientID %>");
            var __tb5 = document.getElementById("<%=TextBox5.ClientID %>");
            if (LinesArray[6] == "") {
                if (__tb5 != null) { __tb5.style.visibility = "hidden"; setInputOrSpanValue(__tb5, ""); }
                if (_lbParameter5 != null) _lbParameter5.innerText = "";
            }
            else {
                if (__tb5 != null) { __tb5.style.visibility = "visible"; setInputOrSpanValue(__tb5, ""); }
                if (_lbParameter5 != null) { _lbParameter5.innerText = LinesArray[6]; }
            }
        }

        if (LinesArray.length >= 8) {
            var _lbParameter6 = document.getElementById("<%=lbParameter6.ClientID %>");
            var __tb6 = document.getElementById("<%=TextBox6.ClientID %>");
            if (LinesArray[7] == "") {
                if (__tb6 != null) { __tb6.style.visibility = "hidden"; setInputOrSpanValue(__tb6, ""); }
                if (_lbParameter6 != null) _lbParameter6.innerText = "";
            }
            else {
                if (__tb6 != null) { __tb6.style.visibility = "visible"; setInputOrSpanValue(__tb5, ""); }
                if (_lbParameter6 != null) { _lbParameter6.innerText = LinesArray[7]; }
            }
        }

        if (LinesArray.length >= 9) {
            var _lbParameter7 = document.getElementById("<%=lbParameter7.ClientID %>");
            var __tb7 = document.getElementById("<%=TextBox7.ClientID %>");
            if (LinesArray[8] == "") {
                if (__tb7 != null) { __tb7.style.visibility = "hidden"; setInputOrSpanValue(__tb7, ""); }
                if (_lbParameter7 != null) _lbParameter7.innerText = "";
            }
            else {
                if (__tb7 != null) { __tb7.style.visibility = "visible"; setInputOrSpanValue(__tb7, ""); }
                if (_lbParameter7 != null) { _lbParameter7.innerText = LinesArray[8]; }
            }
        }

        if (LinesArray.length >= 10) {
            var _lbParameter8 = document.getElementById("<%=lbParameter8.ClientID %>");
            var __tb8 = document.getElementById("<%=TextBox8.ClientID %>");
            if (LinesArray[9] == "") {
                if (__tb8 != null) { __tb8.style.visibility = "hidden"; setInputOrSpanValue(__tb8, "");}
                if (_lbParameter8 != null) _lbParameter8.innerText = "";
            }
            else {
                if (__tb8 != null) { __tb8.style.visibility = "visible"; setInputOrSpanValue(__tb8, ""); }
                if (_lbParameter8 != null) { _lbParameter8.innerText = LinesArray[9]; }
            }
        }

        ShowInfo("");
    }


    function print() 
    {
        ShowInfo("");
        var _transfer_string = "";
        var __product = document.getElementById("<%=txtProductID.ClientID%>");
        if (__product.value == "") 
        {
            alert(msgInputPathFirstErrorTip);
        }
        else 
        {
            var _lbParameter1 = document.getElementById("<%=lbParameter1.ClientID %>");
            var _lbParameter2 = document.getElementById("<%=lbParameter2.ClientID %>");
            var _lbParameter3 = document.getElementById("<%=lbParameter3.ClientID %>");
            var _lbParameter4 = document.getElementById("<%=lbParameter4.ClientID %>");
            var _lbParameter5 = document.getElementById("<%=lbParameter5.ClientID %>");
            var _lbParameter6 = document.getElementById("<%=lbParameter6.ClientID %>");
            var _lbParameter7 = document.getElementById("<%=lbParameter7.ClientID %>");
            var _lbParameter8 = document.getElementById("<%=lbParameter8.ClientID %>");
            var __tb1 = document.getElementById("<%=TextBox1.ClientID %>");
            var __tb2 = document.getElementById("<%=TextBox2.ClientID %>");
            var __tb3 = document.getElementById("<%=TextBox3.ClientID %>");
            var __tb4 = document.getElementById("<%=TextBox4.ClientID %>");
            var __tb5 = document.getElementById("<%=TextBox5.ClientID %>");
            var __tb6 = document.getElementById("<%=TextBox6.ClientID %>");
            var __tb7 = document.getElementById("<%=TextBox7.ClientID %>");
            var __tb8 = document.getElementById("<%=TextBox8.ClientID %>");

            if (_lbParameter1.innerText != "") {
                _transfer_string += "set " + splitParam(_lbParameter1.innerText) + "=" + __tb1.value + "##";
            }
            if (_lbParameter2.innerText != "") {
                _transfer_string += "set " + splitParam(_lbParameter2.innerText) + "=" + __tb2.value + "##";
            }
            if (_lbParameter3.innerText != "") {
                _transfer_string += "set " + splitParam(_lbParameter3.innerText) + "=" + __tb3.value + "##";
            }
            if (_lbParameter4.innerText != "") {
                _transfer_string += "set " + splitParam(_lbParameter4.innerText) + "=" + __tb4.value + "##";
            }
            if (_lbParameter5.innerText != "") {
                _transfer_string += "set " + splitParam(_lbParameter5.innerText) + "=" + __tb5.value + "##";
            }
            if (_lbParameter6.innerText != "") {
                _transfer_string += "set " + splitParam(_lbParameter6.innerText) + "=" + __tb6.value + "##";
            }
            if (_lbParameter7.innerText != "") {
                _transfer_string += "set " + splitParam(_lbParameter7.innerText) + "=" + __tb7.value + "##";
            }
            if (_lbParameter8.innerText != "") {
                _transfer_string += "set " + splitParam(_lbParameter8.innerText) + "=" + __tb8.value + "##";
            }


            var __txtProductID = document.getElementById("<%=txtProductID.ClientID %>");
            var _txtFileName = document.getElementById("<%=txtFileName.ClientID %>");

            _transfer_string += "call ";
            var tmp = __txtProductID.value.trim();
            if ((tmp.length > 1) && (tmp.substring(tmp.length - 1, tmp.length) != '\\')) {
                tmp += "\\";
            }
            tmp += _txtFileName.innerText.trim();
            if (contains(tmp, " ", false) || contains(tmp, "\t", false)) {
                var x = "\"" + tmp + "\"";
                tmp = x;
            }


            _transfer_string += tmp;

            __runBat(_transfer_string, true);
        }
        
    }


    function __runBat(mainBatInfo, isSynchronized) {
        try {
            var __txtProductID = document.getElementById("<%=txtProductID.ClientID %>");
            var LinesArray = mainBatInfo.split("##");
            var file_exist_check = false;
            var msgFileNotExist = "File";
            if (LinesArray.length >= 1) {
                var file_check = LinesArray[LinesArray.length - 1];

                var first_pos = 5;
                var _pos_fix = 0;

                if (file_check.toString().substring(5, 6) == '\"') {
                    first_pos++;
                    _pos_fix = 1;
                }

                file_check = file_check.toString().substring(first_pos);
                var last = file_check.length - _pos_fix;
                file_check = file_check.toString().substring(0, last);

                var fso;
                fso = new ActiveXObject("Scripting.FileSystemObject");
                if (fso.FileExists(file_check)) {
                    file_exist_check = true;
                }
                else {
                    msgFileNotExist += ": "
                    msgFileNotExist += file_check;
                }
            }

            if (file_exist_check == false) 
            {
                msgFileNotExist += " does not exist, please check!";
                alert(msgFileNotExist);
                //ShowInfo(msgFileNotExist);
                __txtProductID.focus();
                return;
            }

            //while (1)
            //{
            //    mainBatInfoTmp = mainBatInfo.toString().replace("##", "\r\n");
            //    if (mainBatInfo == mainBatInfoTmp) 
            //    {
            //        break;
            //    }
            //    mainBatInfo = mainBatInfoTmp;
            //}
            if (createDir(ClientBatFilePath)) {
                //var fileName = "PTR" + (new Date()).getTime() + ".bat";
                //var fileName = "PRT" + mainBatInfo[0].replace(/[^0-9a-zA-Z]*/g, '') + mainBatInfo[1];
                var fileName = "offlineLableprint.bat";
                var fileObj = new ActiveXObject("Scripting.FileSystemObject");
                var bat = fileObj.CreateTextFile(ClientBatFilePath + "\\" + fileName, true);

                //mainBatInfo[2] += "\r\ndel %0\r\n";
                //bat.WriteLine(mainBatInfo);
                for (i = 0; i < LinesArray.length; i++) {
                    bat.WriteLine(LinesArray[i]);
                }
                bat.WriteLine("if %errorlevel%==0 (goto:good)\r\n");
                bat.WriteLine("else\r\n");
                bat.WriteLine("exit 1\r\n");
                bat.WriteLine(":good\r\n");
                bat.WriteLine("exit 0\r\n");
                bat.Close();

                wsh = new ActiveXObject("wscript.shell");
                //wsh.run("cmd /k " + getHomeDisk(homeDir) + "&cd " + homeDir + "&" + fileName + "&exit", 2, true);
                //ITC-1122-0154 Tong.Zhi-Yong 2010-03-05
                var _return_value = wsh.run("cmd /k " + getHomeDisk(ClientBatFilePath) + "&cd " + ClientBatFilePath + "&" + fileName + "&exit", 2, isSynchronized);
                //__reDraw_current_page();

                if (_return_value == 0) 
                {
            //        __txtProductID.value = "";
          //          Refresh(",,,,,,,,,");
        //            document.getElementById("<%=btnHid_en_drpdownList.ClientID%>").click();
                    ShowInfo(msgSuccess);
                    ShowSuccessfulInfo(true);
                }
                else 
                {
                    alert(msgPrintReturnError);
                    ShowInfo(msgPrintReturnError);
                }
                __txtProductID.focus();
            }
        }
        catch (e) {
            alert(e.description);
            ShowInfo(e.description);
        }
    }

    function contains(string,substr,isIgnoreCase)
    {
        if(isIgnoreCase)
        {
            string=string.toLowerCase();
            substr=substr.toLowerCase();
        }
        var startChar=substr.substring(0,1);
        var strLen=substr.length;
        for(var j=0;j<string.length-strLen+1;j++)
        {
            if(string.charAt(j)==startChar)//如果匹配起始字符,开始查找
            {
                if(string.substring(j,j+strLen)==substr)//如果从j开始的字符与str匹配，那ok
                {
                    return true;
                }   
            }
        }
        return false;
    }
    var separator="@";
    function splitParam(param) {
        var paramArray = param.split(separator);
        var value = paramArray.length>1?paramArray[1]:paramArray[0];
       return value;
    }



</script>
</asp:Content>



<%--
 INVENTEC corporation (c)2012 all rights reserved. 
 Description: multi common input control
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2012-01-29  Lucy Liu             Create 
 Known issues:
 --%>
<%@ Control Language="C#" AutoEventWireup="false" CodeFile="MultiCommonInputControl.ascx.cs"
    Inherits="CommonControl_MultiCommonInputControl" %>
<asp:TextBox ID="txtInputMulti" runat="server" />
<input type="hidden" id="hidKeyAllowTimeMulti" runat="server" />

<script language="javascript" type="text/javascript">

    function onDateKeyDownMulti(obj) {
        var inputContent = document.getElementById(obj.id).value;


        if (inputContent.length == 0) {
            var d = new Date();
            document.getElementById(obj.id).setAttribute("timeStart", d.toString());
        }

        if (event.keyCode == 9 || event.keyCode == 13) {

            var canUserKeyboard = document.getElementById(obj.id).getAttribute("keyboard");
            var isClear = document.getElementById(obj.id).getAttribute("isClear");

            var ETFun = document.getElementById(obj.id).getAttribute("enterOrTabFun");

            if (isClear == null) {

                document.getElementById(obj.id).setAttribute("finishInputFlag", true);
            }


            if (inputContent.length != 0 && inputContent.trim() != "") {
                if (canUserKeyboard != null) {
                    if (canUserKeyboard.toLowerCase() == "false") {
                        var isScanner = null;
                        var d = new Date();
                        document.getElementById(obj.id).setAttribute("timeStop", d.toString());

                        isScanner = CountTimeMulti(obj.id, new Date(document.getElementById(obj.id).getAttribute("timeStart")), new Date(document.getElementById(obj.id).getAttribute("timeStop")));
                        if (!isScanner) {
                            //need modify the parameter content
                            ShowMessageByUrl("../ErrMessageDisplay.aspx", "Please use scanner to input data!");

                            try {
                                TextAreaChange("Please use scanner to input data!");
                            }
                            catch (e) {
                                document.getElementById(obj.id).value = "";
                                document.getElementById(obj.id).focus();

                                return;
                            }

                            document.getElementById(obj.id).value = "";
                            document.getElementById(obj.id).focus();

                            return;
                        }
                    }
                }

                inputContent = inputContent.toUpperCase();
                document.getElementById(obj.id).value = inputContent.trim();

                if (document.getElementById(obj.id).getAttribute("ProcessQuickInput") == "true") {

                    var saveWriteIndex = parseInt(document.getElementById(obj.id).getAttribute("writeIndex"), 10) + 1;
                    document.getElementById(obj.id).setAttribute("writeIndex", saveWriteIndex);

                    if (document.getElementById(obj.id).getAttribute("sInput") == "") {

                        var temp = inputContent.trim();
                        document.getElementById(obj.id).setAttribute("sInput", temp);
                    } else {
                        var temp = document.getElementById(obj.id).getAttribute("sInput") + "|" + inputContent.trim();

                        document.getElementById(obj.id).setAttribute("sInput", temp);
                    }


                }

                if (isClear != null) {
                    document.getElementById(obj.id).value = "";
                }

                event.returnValue = false;
            } else {
                if (document.getElementById(obj.id).getAttribute("ProcessQuickInput") == "true") {
                    ShowMessageByUrl("../ErrMessageDisplay.aspx", "Please input or scan!");

                    try {
                        TextAreaChange("Please input or scan!");
                    }
                    catch (e) {
                        document.getElementById(obj.id).focus();
                        return;
                    }

                    document.getElementById(obj.id).focus();
                }

                event.returnValue = false;
            }

            if ((ETFun != null) && (ETFun != "")) {
                eval(ETFun);
            }

        } else {
            if (isClear == null) {
                if ((document.getElementById(obj.id).getAttribute("finishInputFlag")) && (event.keyCode != 17)) {
                    document.getElementById(obj.id).value = "";
                    document.getElementById(obj.id).setAttribute("finishInputFlag", false);
                }
            }

            if (document.getElementById(obj.id).onkeypress == null) {
                document.getElementById(obj.id).onkeypress = function() { keyPressMulti(this) };

            }
        }
    }

    function keyPressMulti(obj) {
        var inputContent = document.getElementById(obj.id).value;
        var pattern = /^[-0-9a-zA-Z\s\*]*$/;
        var content = inputContent + String.fromCharCode(event.keyCode);
        var inputPattern = document.getElementById(obj.id).getAttribute("expression");

        if (inputPattern != null && inputPattern != "") {
            pattern = new RegExp(inputPattern);
        }

        if (pattern.test(content)) {
            event.returnValue = true;
        } else {
            event.returnValue = false;
        }
    }


    function CountTimeMulti(controlId, timeStart1, timeStop1) {
        var flag1 = false;
        var tmp = timeStop1.getTime() - timeStart1.getTime();
        var hidTime = document.getElementById("<%=hidKeyAllowTimeMulti.ClientID %>").value;

        var allowKeyInTime = parseInt(hidTime, 10);

        if (tmp <= allowKeyInTime) {
            flag1 = true;
        }

        return flag1;
    }



    String.prototype.trim = function() {
        if (this == null) {
            this == "";
        }

        return this.replace(/^\s*(.*?)[\s\n]*$/g, '$1');
    }

    function hasAvailableDataMulti(controlId) {

        return parseInt(document.getElementById(controlId + "_txtInputMulti").getAttribute("readIndex"), 10) > parseInt(document.getElementById(controlId + "_txtInputMulti").getAttribute("writeIndex"), 10) ? false : true;
    }

    function getAvailableDataMulti(param, controlId) {


        if (!hasAvailableDataMulti(controlId)) {
            window.setTimeout("getAvailableDataMulti('" + param + "', '" + controlId + "')", 300);
            return;
        } else {
            var inputContent = document.getElementById(controlId + "_txtInputMulti").getAttribute("sInput");
            var inputArray = inputContent.split("|");
            var returnData = inputArray[parseInt(document.getElementById(controlId + "_txtInputMulti").getAttribute("readIndex"), 10)];
            var saveReadIndex = parseInt(document.getElementById(controlId + "_txtInputMulti").getAttribute("readIndex"), 10) + 1;
            document.getElementById(controlId + "_txtInputMulti").setAttribute("readIndex", saveReadIndex);

            eval(param + "('" + returnData + "');");
        }
    }

    function clearDataMulti(id) {
        document.getElementById(id + "_txtInputMulti").setAttribute("readIndex", "0");
        document.getElementById(id + "_txtInputMulti").setAttribute("writeIndex", "-1");
        document.getElementById(id + "_txtInputMulti").setAttribute("sInput", "");
    }

    function disableControlMulti(id) {

        document.getElementById(id + "_txtInputMulti").disabled = true;
    }

    function enableControlMulti(id) {
        document.getElementById(id + "_txtInputMulti").disabled = false;
    }

    function getCommonInputObjectMulti(id) {
        return document.getElementById(id + "_txtInputMulti");
    }
</script>


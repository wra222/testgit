<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowKBEsop.aspx.cs" Inherits="CommonFunction_ShowKBEsop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="pragram" content="no-cache" />
    <meta http-equiv="cache-control" content="no-cache, must-revalidate" />
    <meta http-equiv="expires" content="0" />
    <title>KB ESOP</title>
</head>
<body>
    <form id="form2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="Service/ESOPWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="ShowKBEsopDiv" style="position: absolute; top: 0px; left: 0px;">
        <img id="KB001" src="" style="display: none; z-index: 1;" alt="" />
        <div id="divKB002" style="position: absolute; top: 500px; left: 780px;">
            <img id="KB002" src="" style="display: none; z-index: 2;" alt="" />
        </div>
        <div id="divKB003" style="position: absolute; top: 500px; left: 840px;">
            <img id="KB003" src="" style="display: none; z-index: 2;" alt="" />
        </div>
        <div id="divKB004" style="position: absolute; top: 560px; left: 780px;">
            <img id="KB004" src="" style="display: none; z-index: 2;" alt="" />
        </div>
        <div id="divKB005" style="position: absolute; top: 560px; left: 840px;">
            <img id="KB005" src="" style="display: none; z-index: 2;" alt="" />
        </div>
        <div id="divKB006" style="position: absolute; top: 542px; left: 76px;">
            <img id="KB006" src="" style="display: none; z-index: 2;" alt="" />
        </div>
    </div>
    </form>

    <script type="text/javascript" language="javascript">
        var NewPicArray = new Array();
        var CurrentPicArray = Array();
        var hostName = "";
        var station = "";
        var PicLocation = "";
        var SrcArray = new Array();
        var XWidth = window.screen.width;
        var YHeight = window.screen.height;

        var KB002Top = 739;
        var KB002Left = 1103;
        var KB003Top = 739;
        var KB003Left = 1219;
        var KB004Top = 870;
        var KB004Left = 1103;
        var KB005Top = 870;
        var KB005Left = 1219;
        var KB006Top = 780;
        var KB006Left = 84;

        var imgWidth = 1506;
        var imgHeight = 1131;

        var imageArray = new Array(7);
        var WidthArray = new Array(7);
        var HeightArray = new Array(7);

        window.onload = function() {
            XwidthHeight();
            for (var j = 0; j < 7; j++) {
                SrcArray.push("");
            }
            PicLocation = '<%=PicLoc %>';
            var args = new Object();
            var query = window.location.search.substring(1);
            var pairs = query.split("&");
            for (var i = 0; i < pairs.length; i++) {
                var pos = pairs[i].indexOf('=');
                if (pos == -1) continue;
                var argname = pairs[i].substring(0, pos);
                var value = pairs[i].substring(pos + 1);
                value = decodeURIComponent(value);
                args[argname] = value;
            }

            station = args["Station"];
            hostName = args["HostName"];

            ESOPWebService.GetPicPosition(onGetPicPositionSucc);

        };

        function onGetPicPositionSucc(results) {
            if (results) {
                for (var i = 0; i < results.length; i = i + 2) {
                    switch (results[i]) {
                        case "KB001":
                            var KB001Size = results[i + 1].split(",");
                            imgWidth = KB001Size[0];
                            imgHeight = KB001Size[1];
                            break;
                        case "KB002":
                            var KB002Size = results[i + 1].split(",");
                            KB002Top = KB002Size[0];
                            KB002Left = KB002Size[1];
                            break;
                        case "KB003":
                            var KB003Size = results[i + 1].split(",");
                            KB003Top = KB003Size[0];
                            KB003Left = KB003Size[1];
                            break;
                        case "KB004":
                            var KB004Size = results[i + 1].split(",");
                            KB004Top = KB004Size[0];
                            KB004Left = KB004Size[1];
                            break;
                        case "KB005":
                            var KB005Size = results[i + 1].split(",");
                            KB005Top = KB005Size[0];
                            KB005Left = KB005Size[1];
                            break;
                        case "KB006":
                            var KB006Size = results[i + 1].split(",");
                            KB006Top = KB006Size[0];
                            KB006Left = KB006Size[1];
                            break;
                    }
                }
            }
            subscribe();
        }
        function subscribe() {
            ESOPWebService.GetMessage([{ hostName: hostName, station: station}], onSucc, onFail);
        }
        function onSucc(results) {
            // subscribe again
            subscribe();
            setPosition();

            // show eSOP
            NewPicArray = results.Content;
            if (NewPicArray && NewPicArray != CurrentPicArray) {
                CurrentPicArray = NewPicArray;
                for (var i = 1; i < 7; i++) {
                    var matchPicId = false;
                    for (var j = 0; j < CurrentPicArray.length; j = j + 2) {
                        if (CurrentPicArray[j] == "KB00" + i.toString()) {
                            if (SrcArray[i] != CurrentPicArray[j + 1]) {
                                document.getElementById(CurrentPicArray[j]).src = PicLocation + CurrentPicArray[j + 1];
                                //document.getElementById(CurrentPicArray[j]).style.display = "block";
                                callResizeImage(CurrentPicArray[j], PicLocation + CurrentPicArray[j + 1]);
                                SrcArray[i] = CurrentPicArray[j + 1];
                            }
                            matchPicId = true;
                            break;
                        }
                    }
                    if (!matchPicId) {
                        SrcArray[i] = "";
                        document.getElementById("KB00" + i.toString()).src = "";
                        document.getElementById("KB00" + i.toString()).style.display = "none";
                    }
                }
            } else {
                for (var i = 1; i < 7; i++) {
                    SrcArray[i] = "";
                    document.getElementById("KB00" + i.toString()).src = "";
                    document.getElementById("KB00" + i.toString()).style.display = "none";
                }
            }
        }
        function callResizeImage(imgID, imageName) {
            if (imgID == "KB001") {
                imageArray[1] = new Image();
                imageArray[1].onload = function() {
                    //resizeWH(img.Width, img.Height); 
                    WidthArray[1] = imageArray[1].width * XWidth / imgWidth;
                    HeightArray[1] = imageArray[1].height * YHeight / imgHeight;
                    document.getElementById("KB001").style.width = WidthArray[1];
                    document.getElementById("KB001").style.height = HeightArray[1];
                    document.getElementById("KB001").style.display = "block";
                };
                imageArray[1].src = imageName;
            }
            else if (imgID == "KB002") {
                imageArray[2] = new Image();
                imageArray[2].onload = function() {
                    //resizeWH(img.Width, img.Height); 
                    WidthArray[2] = imageArray[2].width * XWidth / imgWidth;
                    HeightArray[2] = imageArray[2].height * YHeight / imgHeight;
                    document.getElementById("KB002").style.width = WidthArray[2];
                    document.getElementById("KB002").style.height = HeightArray[2];
                    document.getElementById("KB002").style.display = "block";
                };
                imageArray[2].src = imageName;
            }
            else if (imgID == "KB003") {
                imageArray[3] = new Image();
                imageArray[3].onload = function() {
                    //resizeWH(img.Width, img.Height); 
                    WidthArray[3] = imageArray[3].width * XWidth / imgWidth;
                    HeightArray[3] = imageArray[3].height * YHeight / imgHeight;
                    document.getElementById("KB003").style.width = WidthArray[3];
                    document.getElementById("KB003").style.height = HeightArray[3];
                    document.getElementById("KB003").style.display = "block";
                };
                imageArray[3].src = imageName;
            }
            else if (imgID == "KB004") {
                imageArray[4] = new Image();
                imageArray[4].onload = function() {
                    //resizeWH(img.Width, img.Height); 
                    WidthArray[4] = imageArray[4].width * XWidth / imgWidth;
                    HeightArray[4] = imageArray[4].height * YHeight / imgHeight;
                    document.getElementById("KB004").style.width = WidthArray[4];
                    document.getElementById("KB004").style.height = HeightArray[4];
                    document.getElementById("KB004").style.display = "block";
                };
                imageArray[4].src = imageName;
            }
            else if (imgID == "KB005") {
                imageArray[5] = new Image();
                imageArray[5].onload = function() {
                    //resizeWH(img.Width, img.Height); 
                    WidthArray[5] = imageArray[5].width * XWidth / imgWidth;
                    HeightArray[5] = imageArray[5].height * YHeight / imgHeight;
                    document.getElementById("KB005").style.width = WidthArray[5];
                    document.getElementById("KB005").style.height = HeightArray[5];
                    document.getElementById("KB005").style.display = "block";
                };
                imageArray[5].src = imageName;
            }
            else if (imgID == "KB006") {
                imageArray[6] = new Image();
                imageArray[6].onload = function() {
                    //resizeWH(img.Width, img.Height); 
                    WidthArray[6] = imageArray[5].width * XWidth / imgWidth;
                    HeightArray[6] = imageArray[5].height * YHeight / imgHeight;
                    document.getElementById("KB006").style.width = WidthArray[6];
                    document.getElementById("KB006").style.height = HeightArray[6];
                    document.getElementById("KB006").style.display = "block";
                };
                imageArray[6].src = imageName;
            }
        }
        function onFail(results) {
            subscribe();
        }

        function XwidthHeight() {
            if (imgWidth / imgHeight >= XWidth / YHeight) {
                YHeight = (XWidth / imgWidth) * imgHeight + 0.5;
            } else {
                XWidth = (YHeight / imgHeight) * imgWidth + 0.5;
            }
        }

        function setPosition() {

            document.getElementById("divKB002").style.top = KB002Top * YHeight / imgHeight;
            document.getElementById("divKB002").style.left = KB002Left * XWidth / imgWidth;
            document.getElementById("divKB003").style.top = KB003Top * YHeight / imgHeight;
            document.getElementById("divKB003").style.left = KB003Left * XWidth / imgWidth;

            document.getElementById("divKB004").style.top = KB004Top * YHeight / imgHeight;
            document.getElementById("divKB004").style.left = KB004Left * XWidth / imgWidth;

            document.getElementById("divKB005").style.top = KB005Top * YHeight / imgHeight;
            document.getElementById("divKB005").style.left = KB005Left * XWidth / imgWidth;

            document.getElementById("divKB006").style.top = KB006Top * YHeight / imgHeight;
            document.getElementById("divKB006").style.left = KB006Left * XWidth / imgWidth;
        }


        var HexA0s = "%A0 %A0 %A0 %A0%A0 %A0 %A0 %A0%A0 %A0 %A0 %A0%A0 %A0 %A0 %A0%A0 %A0 %A0 %A0%A0 %A0 %A0 %A0%A0 %A0 %A0 %A0%A0 %A0 %A0 %A0";
        HexA0s = HexA0s + HexA0s + HexA0s + HexA0s + HexA0s + HexA0s + HexA0s + HexA0s + HexA0s + HexA0s + HexA0s + HexA0s + HexA0s;
        document.title = "KB ESOP" + unescape(HexA0s);
    
    </script>

</body>
</html>

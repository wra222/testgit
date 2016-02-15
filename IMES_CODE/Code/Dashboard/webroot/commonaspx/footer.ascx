<%@ Control Language="C#" AutoEventWireup="true" CodeFile="footer.ascx.cs" Inherits="webroot_commonaspx_footer" %>
<div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
    <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
        <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/webroot/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr>
    </table>
</div>

<script type="text/javascript" language="javascript">
<!--
setReadOnlyInputTextStyle();

function setReadOnlyInputTextStyle()
{
	var objArr = document.getElementsByTagName("input");
	
	for (var i = 0; i < objArr.length; i++)
	{
		var objInput = objArr[i];
		if (objInput.type == "text" && objInput.readOnly)
		{
			objInput.style.backgroundColor = "rgb(239,244,250)";
			objInput.style.borderStyle = "groove";
		}
	}
}
//-->
</script>
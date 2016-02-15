<%--
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: editgroup.aspx
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2008-12-19   itc98079     Create 
 * qa bug no:ITC-1103-0094
 * Known issues:Any restrictions about this file
--%>
<%@ Page Language="C#" AutoEventWireup="true" Theme="MainTheme" CodeFile="editgroup.aspx.cs" Inherits="webroot_aspx_authorities_editgroup" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Group</title>
</head>
<script type="text/javascript" language="javascript">
	var diagArgs = window.dialogArguments; //undefined
	var editorID = diagArgs.editorID;


    if(diagArgs.operateType == "add"){
        document.title = "Add Group";
    }else{//edit
        document.title = "Edit Group";
    }

	function fOnBodyLoad()
	{
		fillContents();
	}

	function fillContents()
	{
	    if (diagArgs.groupName != "" && diagArgs.groupName != undefined) {
	        var objGroupName = document.getElementById("idGroupName");
	        var objComment = document.getElementById("idComment");
	        var dtGroupInfo = webroot_aspx_authorities_editgroup.getGroupInfo(diagArgs.groupName);
	        if (null != dtGroupInfo.error) {
	            alert(dtGroupInfo.error.Message);
	            return;
	        }
	        objGroupName.value = dtGroupInfo.value.Rows[0]["name"];
	        objComment.value = dtGroupInfo.value.Rows[0]["comment"];

	        document.getElementById("<%=txtGroupName.ClientID%>").value = objGroupName.value;
	        document.getElementById("<%=txtOldGroupName.ClientID%>").value = objGroupName.value;
	        //document.getElementById("<%=btnQueryTreeView.ClientID%>").click();
	    }
	    else {
	        document.getElementById("<%=txtGroupName.ClientID%>").value = "";
	        var t = document.getElementById("ShowWhenAdd");
	        t.style.display = 'block';
	    }
	}

	function onClickOK()
	{
	    if (diagArgs.groupName != "" && diagArgs.groupName != undefined) {
	        onEditClickOK();
	    }
	    else {
	        onAddClickOK();
	    }
	}

	function ChkClickOK() {   
		var objGroupName = document.getElementById("idGroupName");
		var objComment = document.getElementById("idComment");

		objGroupName.value = Trim(objGroupName.value); 
		objComment.value = Trim(objComment.value);

		if (objGroupName.value == "")
		{
			objGroupName.select();
			alert("<%=Constants.PLEASE_ENTER_A_VALID%>" + "Group Name!");
			return;
		}
		if (checkTextLength("idGroupName", 50, "Group Name"))
		{
			return;
		}

		if (checkTextLength("idComment", 255, "Comment"))
		{
			return;
		}

		/*if (!isNumCharString(objGroupName.value))
		{
			objGroupName.select();
			alert("<%=Constants.PLEASE_ENTER_A_VALID%>" + "Group Name!");
			return;
		}

		if (!isNumCharString(objComment.value))
		{
			objComment.select();
			alert("<%=Constants.PLEASE_ENTER_A_VALID%>" + "Comment!");
			return;
		}*/

		document.getElementById("<%=txtGroupName.ClientID%>").value = objGroupName.value;
		document.getElementById("<%=txtComment.ClientID%>").value = objComment.value;
    }
    function onEditClickOK() {
        ChkClickOK();
        document.getElementById("<%=btnEditSaveGroup.ClientID%>").click();
    }
    function onAddClickOK() {
        ChkClickOK();
        document.getElementById("<%=btnAddSaveGroup.ClientID%>").click();
    }

    function Finish() {
        window.returnValue = '';
        window.close();
    }

	/*
	function
	*/
	function onClickCancel()
	{
		window.returnValue = "cancel";
		window.close();
	}

	/*
	function
	*/
	function submitData()
	{
		window.returnValue = "";
		window.close();
	}

	/*
	function
	*/
	function LTrim(s)
	{
		try {
			do {
				if (null == s || s.length <= 0) break;
				var whitespace = " \t\r\n";
				if (whitespace.indexOf(s.charAt(0)) >= 0)
				{
					var len = s.length;
					var index;
					for (index = 0; index < len; ++index) {
						if (whitespace.indexOf(s.charAt(index)) < 0) break;
					}
					if (index < len) s = s.substring(index); else s = "";
				}
			} while (false);
		} catch (Cb) {
			s = "";
		}
		return s;
	}

	/*
	function
	*/
	function RTrim(s) 
	{
		try {
			do {
				if (null == s || s.length <= 0) break;
				var len = s.length;
				var whitespace = " \t\r\n";
				if (whitespace.indexOf(s.charAt(len - 1)) >= 0)
				{
					var index;
					for (index = len - 1; index >= 0; --index) {
						if (whitespace.indexOf(s.charAt(index)) == -1) break;
					}
					if (index >= 0) s = s.substring(0, index + 1); else s = "";
				}
			} while (false);
		} catch (Db) {
			s = "";
		}
		return s;
	}

	/*
	function
	*/
	function Trim(s)
	{
		return RTrim(LTrim(s));
	}

	function isNumCharString(strValue)
	{
		if (/[^a-zA-Z0-9\s]/.test(strValue))
			return false;
		return true;
	}

	function charCodeAtTest(str)
	{
		//var str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //初始化变量。
		for (var i = 0; i < str.length; ++i)
		{
			var n = str.charCodeAt(i); //获取位置n 上字符的Unicode 值。
			if (n < 33 || n > 126)
			{
				return false;
			}
		}

		return true;//返回该值。
	}

	function checkTextLength(objID, iLength, fieldName)
	{
		var obj = document.getElementById(objID);
		var inputMsg = obj.value;
		if (inputMsg.length > iLength)
		{
			alert(fieldName + " cannot exceed the maximum of " + iLength + ".");
			obj.focus();
			var r = obj.createTextRange();
			r.collapse(false);
			r.select();
			return true; //exceeded
		}
		else
		{
			return false;
		}
	}
</script>
<script type="text/javascript">
    function OnCheckBoxCheckChanged(evt) {
        var src = window.event != window.undefined ? window.event.srcElement : evt.target;
        var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
        if (isChkBoxClick) {
            var parentTable = GetParentByTagName("table", src);
            var nxtSibling = parentTable.nextSibling;
            if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node 
            {
                if (nxtSibling.tagName.toLowerCase() == "div") //if node has children 
                {
                    //check or uncheck children at all levels 
                    CheckUncheckChildren(parentTable.nextSibling, src.checked);
                }
            }
            //check or uncheck parents at all levels 
            CheckUncheckParents(src, src.checked);
        }
    }
    function CheckUncheckChildren(childContainer, check) {
        var childChkBoxes = childContainer.getElementsByTagName("input");
        var childChkBoxCount = childChkBoxes.length;
        for (var i = 0; i < childChkBoxCount; i++) {
            childChkBoxes[i].checked = check;
        }
    }
    function CheckUncheckParents(srcChild, check) {
        var parentDiv = GetParentByTagName("div", srcChild);
        var parentNodeTable = parentDiv.previousSibling;

        if (parentNodeTable) {
            var checkUncheckSwitch;

            if (check) //checkbox checked 
            {
                var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                if (isAllSiblingsChecked)
                    checkUncheckSwitch = true;
                else
                    return; //do not need to check parent if any(one or more) child not checked 
            }
            else //checkbox unchecked 
            {
                checkUncheckSwitch = false;
            }

            var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
            if (inpElemsInParentTable.length > 0) {
                var parentNodeChkBox = inpElemsInParentTable[0];
                parentNodeChkBox.checked = checkUncheckSwitch;
                //do the same recursively 
                CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
            }
        }
    }
    function AreAllSiblingsChecked(chkBox) {
        var parentDiv = GetParentByTagName("div", chkBox);
        var childCount = parentDiv.childNodes.length;
        for (var i = 0; i < childCount; i++) {
            if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node 
            {
                if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                    var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                    //if any of sibling nodes are not checked, return false 
                    if (!prevChkBox.checked) {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    //utility function to get the container of an element by tagname 
    function GetParentByTagName(parentTagName, childElementObj) {
        var parent = childElementObj.parentNode;
        while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
            parent = parent.parentNode;
        }
        return parent;

    } 
    
</script>
<script language="C#" runat="server">

    static readonly char[] _slashArray = new char[] {'/'};

    void PopulateNode(Object source, TreeNodeEventArgs e)
    {
        TreeNode node = e.Node;
        System.Data.DataTable treeNodeInfo = new System.Data.DataTable();

        if (node.Value == "Root")
        {
        //    return;
        }

        treeNodeInfo = getNodeData();

        for (int i = 0; i < treeNodeInfo.Rows.Count; i++)
        {
            //alert(treeNodeInfo.Rows[i]["nodeuuid"]);
            String name = (String)treeNodeInfo.Rows[i].ItemArray[0];
            String id = (String)treeNodeInfo.Rows[i].ItemArray[1];

            TreeNode newNode = new TreeNode(name, id);
            newNode.PopulateOnDemand = false;
            newNode.NavigateUrl = "#";
            
            node.ChildNodes.Add(newNode);
        }

        return;
    }

  </script>
<body onload="fOnBodyLoad();" class="dialogBody">
<form id="idFormEditGroup" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
</asp:ScriptManager>
<table id="idMainTable" style="width:100%;height:100%;" cellpadding="10px">
	<tr>
		<td style="width:1%;" nowrap="true">
			<b>
			Group Name:
			</b>
		</td>
		<td>
			<%--<asp:TextBox ID="idGroupName" runat="server" Width="100%"></asp:TextBox>--%>
			<input id="idGroupName" type="text" style="width:100%;" maxlength="20" />
		</td>
	</tr>
	<tr>
		<td colspan="2" nowrap="true">
			<b>
			Comment:
			</b>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<%--<asp:TextBox ID="idComment" runat="server" TextMode="MultiLine" Rows="5" Columns="6" Width="100%"></asp:TextBox>--%>
			<textarea id="idComment" rows="5" cols="6" style="width:100%;"></textarea>
		</td>
	</tr>
	
	<tr id="ShowWhenAdd" style="display: none;">
	<td colspan="2">
	<div id="idDivSysManagement" style="height:250px;overflow:auto;display:block;">
	<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" RenderMode="Block">
	<ContentTemplate>
	<asp:TreeView id="TreeView1" runat="server" ShowCheckBoxes=All OnTreeNodePopulate="PopulateNode" ExpandDepth="1">

        <NodeStyle Width=100% Font-Names="Arial" Font-Size="8pt" ForeColor="DarkBlue"/>
        <RootNodeStyle Font-Bold="True" Font-Size="9pt"/>
    </asp:TreeView>
	<input id="btnQueryTreeView" type="button" style="display:none" onclick="" onserverclick="QueryTreeViewClick" runat="server"/>
	<input id="txtGroupName" type="hidden" runat="server"/>
	<input id="txtComment" type="hidden" runat="server"/>
	<input id="txtOldGroupName" type="hidden" runat="server"/>
	<input id="btnAddSaveGroup" type="button" style="display:none" onclick="" onserverclick="AddSaveGroupClick" runat="server"/>
	<input id="btnEditSaveGroup" type="button" style="display:none" onclick="" onserverclick="EditSaveGroupClick" runat="server"/>
	</ContentTemplate></asp:UpdatePanel>
	</div></td>
	</tr>
	
	<tr>
		<td colspan="2" nowrap="true" align="right">
			<button id="idBtnOK" onclick="onClickOK();">OK</button>
			<%--<asp:Button ID="idBtnOK" runat="server" Text="OK" OnClick="onClickOK" Width="60px" />--%>
			&nbsp;&nbsp;&nbsp;
			<button id="idBtnCancel" onclick="onClickCancel();">Cancel</button>
			&nbsp;&nbsp;&nbsp;
		</td>
	</tr>
</table>
</form>
</body>
</html>
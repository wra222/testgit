<%--
 * INVENTEC corporation (c)2008 all rights reserved.
 * Description: accountauthority.aspx
 * Update:
 * Date         Name            Reason
 * ========== ================= =====================================
 * 2009-10-1   itc98079        Create
 * qa bug no:ITC-1103-0097,ITC-1330-0079,ITC-1330-0077,itc-1330-0212
 * Known issues:Any restrictions about this file
--%>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="accountauthority.aspx.cs" Inherits="webroot_aspx_authorities_accountauthority" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ Import Namespace="com.inventec.system" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<style>
    input.userproperties
    {
     word-wrap:normal;
     border-width:1px;
     margin:0 0 0 0;
     width:100%;
     background-color:LightGrey;
    }
    
    b.condition
    {
    	margin-left:5px;
    }
    .customPointer { cursor: pointer; } 

    .myFieldsetStyle{border:solid 1px rgb(182,182,182);position:relative; top:6px;width:95%;background-color: rgb(210,210,210);  margin-left:10px; margin-right:10px; margin-top:20px; padding-bottom:3pt;padding-left:5pt;padding-right:5pt;}
    .myLegend {color:rgb(60,64,67);margin: -10px 0 0 0;position:relative;font-size:12pt;font-family:Verdana; font-weight:bold;} 

    .title { background-color: rgb(156,192,248);height:22px;font:normal normal bold 9pt Verdana;width:100%;}

</style>
    <script type="text/javascript" src="../../../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../../../CommonControl/JS/Browser.js"></script>

<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>


<table border="0"  bordercolor="red" cellpadding="0" cellspacing="0" style="height:93%;">
<tr>
<td id="Td1" class="title" style="background-color:rgb(101,107,112); color:White">
&nbsp;Group Authority
</td>
<td style="width:10px;background-color:rgb(179,179,179)">&nbsp;</td>
<td id="idTitle"class="title" style="background-color:rgb(101,107,112)">
&nbsp;
</td>
</tr>
<tr style="height:100%;">
<td style="width:99%;height:100%">
	<table id="idMainTable" border="0" bordercolor="olive" cellpadding="0" cellspacing="0" style="height:100%;width:100%">
		<tr>
			<td id="idTDUserGroup" colspan="2" align="center" style="padding:10px 10px 10px 10px;">
				<fieldset class="myFieldsetStyle" style="width:99%;">
					<legend class="myLegend">
						Group
					</legend>
					<!--<div>-->
						<table id="idTableUserGroup" border="0" bordercolor="red" cellpadding="0" cellspacing="0" style="width:100%">
							<tr style="height:5px">
								<td></td>
							</tr>
							<tr>
								<td>

                                    <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" >
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="idBtnAddGroup" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnEditGroup" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnDelete" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnDeleteUser" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnSave" EventName="ServerClick" />
                                    </Triggers>
                                    <ContentTemplate>

                                        <iMES:GridViewExt ID="gdGroupsInSubSystem" runat="server" AutoGenerateColumns="true" Width="100%" 
                                            GvExtWidth="100%" GvExtHeight="190px" Height="180px" OnRowDataBound="gdGroupsInSubSystem_RowDataBound"
                                             OnGvExtRowClick="clickGroupsTable(this)" SetTemplateValueEnable="False" 
                                             HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                                        </iMES:GridViewExt>
                                        
                                        <input type="hidden" id="hidEditorId" runat="server" value=""/>
                                    </ContentTemplate>   
                                    </asp:UpdatePanel>  								
								    <input type="hidden" id="hidGroupType" runat="server" value=""/>
								    <input type="hidden" id="hidGroupName" runat="server" value=""/>
								    <input type="hidden" id="hidGroupId" runat="server" value=""/>
								    <input type="hidden" id="hidGroupComment" runat="server" value=""/>

								</td>
							</tr>
							<tr>
								<td style="height:6px">
								</td>
							</tr>
							<tr>
								<td align="right">
                                    <input id="idBtnAddGroup" type="button" style="width:110px;" value="Add Group" onclick="if(onEditGroup('add'))" onserverclick="addGroup_click" runat="server"/>
									&nbsp;&nbsp;
                                    <input id="idBtnEditGroup" type="button" style="width:110px;" value="Edit Group" onclick="if(onEditGroup('edit'))" onserverclick="editGroup_click" runat="server" disabled="disabled"/>
									&nbsp;&nbsp;
                                    <input id="idBtnDelete" type="button" style="width:110px;" value="Delete" onclick="if(onDeleteGroup())" onserverclick="deleteGroup_click" runat="server" disabled="disabled"/>
								</td>
							</tr>
						</table>
					<!--</div>-->
				</fieldset>
			</td>
		</tr>
		<tr>
			<td align="center" style="padding:10px 10px 10px 10px;">
				<fieldset class="myFieldsetStyle" style="width:99%">
					<legend class="myLegend">
						Group Members
					</legend>
					<!--<div style="padding:5px">-->
						<table id="idTableUserList" align="center" border="0" cellpadding="0" cellspacing="0" Width="97%">
							<tr style="height:5px">
								<td></td>
							</tr>
							<tr>
								<td>
								
                                    <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional" >
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnRefreshUsersAndPermissionInGroup" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnAddUser" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnDeleteUser" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnSave" EventName="ServerClick" />
                                    </Triggers>
                                    <ContentTemplate>

                                        <iMES:GridViewExt ID="gdUsersInGroup" runat="server" AutoGenerateColumns="true" Width="100%" 
                                            GvExtWidth="100%" GvExtHeight="190px" Height="180px" OnRowDataBound="gdUsersInGroup_RowDataBound"
                                             OnGvExtRowClick="clickUsersInGroupTable(this)" SetTemplateValueEnable="False" 
                                             HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                                        </iMES:GridViewExt>
                                         
                                    </ContentTemplate>   
                                    </asp:UpdatePanel>  								
								    <input type="hidden" id="hidUserLoginInGroup" runat="server" value=""/>
								    <input type="hidden" id="hidUserTypeInGroup" runat="server" value=""/>
								    <input type="hidden" id="hidUserNameInGroup" runat="server" value=""/>
								
								</td>
							</tr>
							<tr>
								<td style="height:6px">
								</td>
							</tr>
							<tr>
								<td align="right">
                                    <input id="idBtnAddUser" type="button" style="width:90px;" value="Add User" onclick="if(onAddUserInGroup())" onserverclick="addUserInGroup_click" runat="server" disabled="disabled"/>
									&nbsp;
                                    <input id="idBtnDeleteUser" type="button" style="width:90px;" value="Delete" onclick="if(onDeleteUserOrDeptInGroup())" onserverclick="deleteUserOrDeptInGroup_click" runat="server" disabled="disabled"/>
									<input id="btnRefreshUsersAndPermissionInGroup" type="button" onserverclick="btnRefreshUsersAndPermissionInGroup_Click" style="display:none" onclick="" runat="server"/>

								</td>
							</tr>
						</table>
					<!--</div>-->
				</fieldset>
			</td>
			<td align="center">
				<fieldset class="myFieldsetStyle" style="left:-15px;width:98%; padding-right:3px">
					<legend class="myLegend">
						Menu
					</legend>
                            <iMES:WaitingCoverDiv ID="divCover" runat="server"  KeyDownFun="KeyDownEvent()"  />
						    <table id="idTableAuthority" border="0" bordercolor="green" cellpadding="0" cellspacing="0" style="margin:0px 0px 0px 0px" Width="97%">
							    <tr>
								    <td>
                                        <div id="idDivSysManagement" style="width:100%; height:192px;overflow:auto; border:solid 1px rgb(128,128,128); background-color:rgb(212,226,167);padding:0 0 10 0; margin: 0 0 0 0">
									        <table id="idTable1" border="0" bordercolor="red" Width="97%">
										        <tr>
											        <td align="left" valign="top">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" RenderMode="Block">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnRefreshUsersAndPermissionInGroup" EventName="ServerClick" />
                                                                <asp:AsyncPostBackTrigger ControlID="idBtnSave" EventName="ServerClick" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="All">

                                                                    <NodeStyle Width="100%" Font-Names="Verdana" Font-Size="9pt" ForeColor="black"/>
                                                                    <RootNodeStyle Font-Bold="True" Font-Size="9pt"/>
                                                                </asp:TreeView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>   
                                                        
											        </td>
										        </tr>
									        </table>
								        </div> 
								    </td>
							    </tr>
							    <tr>
								    <td style="height:7px;font-size:0px">
								    </td>
							    </tr>
							    <tr>
								    <td style="height:6px" align="right">
                                            <input id="idBtnSave" disabled="disabled" type="button" style="width:90px;" value="Save" onserverclick="saveClick" runat="server"/>
								    </td>
							    </tr>
						    </table>
				</fieldset>
			</td>
		</tr>
	</table>
</td></tr>
</table>

</div>

<script language="javascript" type="text/javascript">
	var ShowUserGroupTable = null;
	var ShowUserListTable = null;
	var ShowReportTable = null;
	var ShowChartTable = null;
	var ShowNewUserListTable = null;
	var TDCData1 = null;
	var TDCData2 = null;
	var TDCData3 = null;
	var TDCData4 = null;
	var TDCDataNewUser = null;
	var sFeature_group = "dialogHeight:580px;dialogWidth:453px;center:yes;status:no;help:no;scroll:no;resizable:no";
	var sFeature_groupSmall = "dialogHeight:250px;dialogWidth:453px;center:yes;status:no;help:no;scroll:no;resizable:no";
	var sFeature_user = "dialogHeight:600px;dialogWidth:600px;center:yes;status:no;help:no;scroll:no;resizable:no";
	var diagArgs_group = new Object();
	var diagArgs_user = new Object();
	var diagArgs_dept = new Object();
	var tabs = null;
	
	var objSelDomain;
	var objSelCompany;
	var objSelDept;
	var objSearch;
	
	var strSelDomain;
	var strSelCompany;
	var strSelDept;
	var strSearch;
</script>

<script language="javascript" type="text/javascript" for="window" event="onload">

 //document.getElementById("idDivSysManagement").style.width = document.body.clientWidth * 0.28;

</script>

<script language="javascript" type="text/javascript">
    var selectedRowIndex_UsersInSubSystem = null, selectedRowIndex_Groups = null, selectedRowIndex_UsersInGroup = null;

    function ShowMessage(errorMsg){
        alert(errorMsg);
    }

	function onBodyLoad()
	{
		//fillAllDomains();
		//onDomainChange();

	}
	
	//右边区域
    //保存permission，查询存盘状态，server端置状态doing-->success，报成功。
    function saveClick_Complete()
    {
        alert("Save successfully!");
    }

    //右边区域
    

    function HighLightGroupTable(isAddNew)
    {
        var gdGroupsInSubSystemClientID="<%=gdGroupsInSubSystem.ClientID%>";
        var row=eval("setScrollTopForGvExt_"+gdGroupsInSubSystemClientID+"('"+document.getElementById("<%=hidGroupId.ClientID%>").value+"',2,'','MUTISELECT')");

        if(row != null)
        {
            selectedRowIndex_Groups = row.rowIndex - 1;
            if(isAddNew==true)
            {
                clickGroupsTable(row);
            }
            document.getElementById("<%=hidGroupId.ClientID%>").value = row.cells[7].innerText.trim();
            document.getElementById("<%=hidGroupName.ClientID%>").value = row.cells[1].innerText.trim();
            document.getElementById("<%=hidGroupType.ClientID%>").value = row.cells[8].innerText.trim();
            document.getElementById("<%=hidGroupComment.ClientID%>").value = row.cells[6].innerText.trim();
        }
        else{
            selectedRowIndex_Groups = null;
            document.getElementById("<%=hidGroupId.ClientID%>").value = "";
            document.getElementById("<%=hidGroupName.ClientID%>").value = "";
            document.getElementById("<%=hidGroupType.ClientID%>").value = "";


            document.getElementById("<%=btnRefreshUsersAndPermissionInGroup.ClientID%>").click();
            
            
		    var objBtnEditGroup = document.getElementById("<%=idBtnEditGroup.ClientID%>");
		    var objBtnDelete = document.getElementById("<%=idBtnDelete.ClientID%>");
		    var objBtnAddUser = document.getElementById("<%=idBtnAddUser.ClientID%>");
		    var objBtnDelUser = document.getElementById("<%=idBtnDeleteUser.ClientID%>");
		    var objBtnSave = document.getElementById("<%=idBtnSave.ClientID%>");

            
		    objBtnEditGroup.disabled = true;
		    objBtnDelete.disabled = true;
		    objBtnAddUser.disabled = true;
		    objBtnDelUser.disabled = true;
		    objBtnSave.disabled = true;
		}    
    }

    function dblClickGroupsTable(row) {
    
        if (document.getElementById("<%=hidGroupType.ClientID%>").value == "<%=Constants.RBPC_ACCOUNT_TYPE_GROUP%>")
        {
            document.getElementById("<%=idBtnEditGroup.ClientID%>").click();
            
            //onEditGroup("edit");
        }
    }    
        

	function onEditGroup(parOpType)
	{
        //判断是否超时
		var systimeout = webroot_aspx_authorities_accountauthority.getSystemTimeOut();
	    if (systimeout.error != null)
	    {
		    alert(systimeout.error.Message);
		    return;
	    }	
	
	
	
		var objBtnEditGroup = document.getElementById("<%=idBtnEditGroup.ClientID%>");
		var objBtnDelete = document.getElementById("<%=idBtnDelete.ClientID%>");
	    var objBtnAddUser = document.getElementById("<%=idBtnAddUser.ClientID%>");
	    var objBtnDelUser = document.getElementById("<%=idBtnDeleteUser.ClientID%>");
		var objBtnSave = document.getElementById("<%=idBtnSave.ClientID%>");
		
		if (parOpType == "edit")
		{
			diagArgs_group.operateType = parOpType;
			diagArgs_group.editorID = document.getElementById("<%=hidEditorId.ClientID%>").value;
			diagArgs_group.groupID = document.getElementById("<%=hidGroupId.ClientID%>").value;
			diagArgs_group.groupName = document.getElementById("<%=hidGroupName.ClientID%>").value;
			diagArgs_group.groupComment = document.getElementById("<%=hidGroupComment.ClientID%>").value;
			var editedGroupID = window.showModalDialog("editgroup.aspx", diagArgs_group, sFeature_groupSmall);
			if (editedGroupID == undefined || editedGroupID == "cancel"){
			    return false;
			}else{
				return true;
			}
		}
		else if (parOpType == "add")
		{
			diagArgs_group.operateType = parOpType;
			diagArgs_group.groupID = "";
			diagArgs_group.groupName = "";
			diagArgs_group.groupComment = "";
			diagArgs_group.editorID = document.getElementById("<%=hidEditorId.ClientID%>").value;

			var addedGroupID = window.showModalDialog("editgroup.aspx", diagArgs_group, sFeature_group);
			if (addedGroupID == undefined || addedGroupID == "cancel"){
			    return false;
			}else{
		        document.getElementById("<%=hidGroupId.ClientID%>").value=addedGroupID;
				return true;
			}
		}
		else
		{
			alert("Sorry, error occurred! Please refresh the main window.");
			return false;
		}
	}


	function onDeleteGroup()
	{

		var notice = "Are you sure you want to delete the group '" + document.getElementById("<%=hidGroupName.ClientID%>").value + "'?"; 
		if (confirm(notice))
		{
		    return true;
		}
		else
		{
		    return false;
		}
	}
	

    function clickGroupsTable(row)
    {
        //selectedRowIndex = parseInt(con.index, 10);
        if((selectedRowIndex_Groups!=null) && (selectedRowIndex_Groups!=parseInt(row.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex_Groups,false, "<%=gdGroupsInSubSystem.ClientID %>");                
        }
        
        setRowSelectedOrNotSelectedByIndex(row.index,true, "<%=gdGroupsInSubSystem.ClientID %>");
        selectedRowIndex_Groups = parseInt(row.index, 10);


        document.getElementById("<%=hidGroupId.ClientID%>").value = row.cells[7].innerText.trim();
        document.getElementById("<%=hidGroupName.ClientID%>").value = row.cells[1].innerText.trim();
        document.getElementById("<%=hidGroupType.ClientID%>").value = row.cells[8].innerText.trim();
        document.getElementById("<%=hidGroupComment.ClientID%>").value = row.cells[6].innerText.trim();


        document.getElementById("<%=btnRefreshUsersAndPermissionInGroup.ClientID%>").click();
        
        
		var objBtnEditGroup = document.getElementById("<%=idBtnEditGroup.ClientID%>");
		var objBtnDelete = document.getElementById("<%=idBtnDelete.ClientID%>");
	    var objBtnAddUser = document.getElementById("<%=idBtnAddUser.ClientID%>");
	    var objBtnDelUser = document.getElementById("<%=idBtnDeleteUser.ClientID%>");
		var objBtnSave = document.getElementById("<%=idBtnSave.ClientID%>");

        
		if (document.getElementById("<%=hidGroupName.ClientID%>").value == "")
		{
			objBtnEditGroup.disabled = true;
			objBtnDelete.disabled = true;
			objBtnAddUser.disabled = true;
			objBtnDelUser.disabled = true;
			objBtnSave.disabled = true;
		}
		else
		{ 
		    if (document.getElementById("<%=hidGroupType.ClientID%>").value == "<%=Constants.RBPC_ACCOUNT_TYPE_SINGLE_USER_GROUP%>")
		    {
			    objBtnEditGroup.disabled = true;
			    objBtnDelete.disabled = false;
			    objBtnAddUser.disabled = true;
			    objBtnDelUser.disabled = true;
			    objBtnSave.disabled = false;
		    }
		    else
		    {
			    objBtnEditGroup.disabled = false;
			    objBtnDelete.disabled = false;
			    objBtnAddUser.disabled = false;
			    objBtnDelUser.disabled = true;
			    objBtnSave.disabled = false;
		    }
		}
		var grouptype = document.getElementById("<%=hidGroupType.ClientID%>").value;
		if (grouptype == "0")
		    objBtnEditGroup.disabled = false;
		else
		    objBtnEditGroup.disabled = true;
		if ((grouptype == "1") || (grouptype == "0"))
		    objBtnDelete.disabled = false;
		else
		    objBtnDelete.disabled = true;
    }
    
	
    //右边区域
    //下面的表格user by group
	function onAddUserInGroup()
	{
        //判断是否超时
		var systimeout = webroot_aspx_authorities_accountauthority.getSystemTimeOut();
	    if (systimeout.error != null)
	    {
		    alert(systimeout.error.Message);
		    return;
	    }
	    
	    	
		var strGroupName = document.getElementById("<%=hidGroupName.ClientID%>").value;
		diagArgs_user.groupName = strGroupName;
        diagArgs_user.interfaceType = "adduser";
        diagArgs_user.editorID = document.getElementById("<%=hidEditorId.ClientID%>").value;

		var result = window.showModalDialog("selectuser.aspx", diagArgs_user, sFeature_user);
		if (result != undefined && result != "")
		{
			//alert(userIDs);
			//getUserListRecordSetByGroup(diagArgs_user.groupName);
			return true;
		}
		else
		{
			//user canceled
			return false;
		}
	}


	function onDeleteUserOrDeptInGroup()
	{

		var notice = "Are you sure you want to delete the item?";
		if (confirm(notice))
		{

			return true;
		}
		else
		{
		    return false;
		}
	}



    function clickUsersInGroupTable(row)
    {

        if((selectedRowIndex_UsersInGroup!=null) && (selectedRowIndex_UsersInGroup!=parseInt(row.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex_UsersInGroup, false, "<%=gdUsersInGroup.ClientID %>");                
        }
        
        setRowSelectedOrNotSelectedByIndex(row.index,true, "<%=gdUsersInGroup.ClientID %>");
        selectedRowIndex_UsersInGroup = parseInt(row.index, 10);


        document.getElementById("<%=hidUserNameInGroup.ClientID%>").value = row.cells[1].innerText.trim();
        document.getElementById("<%=hidUserLoginInGroup.ClientID%>").value = row.cells[2].innerText.trim();
        document.getElementById("<%=hidUserTypeInGroup.ClientID%>").value = row.cells[5].innerText.trim();


		var objBtnAddUser = document.getElementById("<%=idBtnAddUser.ClientID%>");
		var objBtnDeleteUser = document.getElementById("<%=idBtnDeleteUser.ClientID%>");

        
		if (document.getElementById("<%=hidUserLoginInGroup.ClientID%>").value == "" || document.getElementById("<%=hidGroupType.ClientID%>").value == "<%=Constants.RBPC_ACCOUNT_TYPE_SINGLE_USER_GROUP%>")
		{
			objBtnDeleteUser.disabled = true;
		}
		else
		{ 
			objBtnDeleteUser.disabled = false;
		}
		
		if (document.getElementById("<%=hidUserLoginInGroup.ClientID%>").value == "admin"){
			objBtnDeleteUser.disabled = true;
		}
    }	
    
    
    function HighLightUsersInGroupTable()
    {
        var gdUsersInGroupClientID="<%=gdUsersInGroup.ClientID%>";
        var row=eval("setScrollTopForGvExt_"+gdUsersInGroupClientID+"('"+document.getElementById("<%=hidUserLoginInGroup.ClientID%>").value+"',2,'','MUTISELECT')");

        if(row != null)
        {
        
            selectedRowIndex_UsersInGroup = row.rowIndex - 1;
        }
        else{
            selectedRowIndex_UsersInGroup = null;
            
            document.getElementById("<%=hidUserNameInGroup.ClientID%>").value = "";
            document.getElementById("<%=hidUserLoginInGroup.ClientID%>").value = "";
            document.getElementById("<%=hidUserTypeInGroup.ClientID%>").value = "";
            
		    var objBtnDeleteUser = document.getElementById("<%=idBtnDeleteUser.ClientID%>");
            
		    objBtnDeleteUser.disabled = true;
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
<script type="text/javascript" language="javascript">
	//getUserGroupRecordSet();
	
    //document.getElementById("idDivSysManagement").style.height = document.body.clientHeight * 0.335;
    //document.getElementById("idDivSysManagement").style.width = document.body.clientWidth * 0.31;
	
    //getUserListRecordSetByGroup("");		
</script>
</asp:Content>
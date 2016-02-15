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
<td id="Td1" class="title" style="background-color:rgb(101,107,112)">
&nbsp;User & Department
</td>
<td style="width:10px;background-color:rgb(179,179,179)">&nbsp;</td>
<td id="idTitle"class="title" style="background-color:rgb(101,107,112)">
&nbsp;Group Authority
</td>
</tr>
<tr style="height:100%;"><td align="center">
	<table id="Table1" border="0" bordercolor="green" cellpadding="0" cellspacing="0" style="height:100%;width:95%">

		<tr style="height:5px">
			<td colspan="2"></td>
		</tr>
        <tr>
            <td>
                <b class="condition">Search:</b>
            </td>
            <td>
                <asp:TextBox id="idSearchTxt" MaxLength="50" style="width:150;" runat="server"  onkeypress="onPressEnter(event);" SkinId="textBoxSkin"></asp:TextBox>
                <button id="idBtnGo" onclick="onClickGo();" style="width:25px; vertical-align:bottom">
	                Go
                </button>
            </td>
        </tr>
		<tr style="height:5px">
			<td colspan="2"></td>
		</tr>
		<tr>
		    <td colspan="2" style="vertical-align:top;" align="center">

                <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnRefreshUserInSubSystem" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="idBtnDelete" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="idBtnAddLocal" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="idBtnEditLocal" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="idBtnAddSingleUser" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="idBtnDeleteUserInSubSystem" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="idBtnAddDept" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="idBtnAddUser" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="idBtnDeleteUser" EventName="ServerClick" />
                </Triggers>
                <ContentTemplate>

                    <iMES:GridViewExt ID="gdUsersInSubSystem" runat="server" AutoGenerateColumns="true" Width="100%" 
                        GvExtWidth="100%" GvExtHeight="280px" Height="270px" OnRowDataBound="gdUsersInSubSystem_RowDataBound"
                         OnGvExtRowClick="clickUsersInSubSystemTable(this)" OnGvExtRowDblClick="dblClickTable(this)" SetTemplateValueEnable="False" 
                         HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                    </iMES:GridViewExt>
			        <input type="hidden" id="hidUserId" runat="server" value=""/>
			        <input type="hidden" id="hidUserType" runat="server" value=""/>
			        <input type="hidden" id="hidEditorId" runat="server" value=""/>
                     
                </ContentTemplate>   
                </asp:UpdatePanel>   		    
		    </td>
		</tr>
		<tr>
		    <td colspan="2" style="padding:10px 10px 10px 10px;">
				<fieldset class="myFieldsetStyle" style="width:100%;left:-20px">
					<legend class="myLegend">
						Property
					</legend>
					<!--<div>-->
						<table id="tblUserProperties" align="center" border="0" bordercolor="red" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td>Name:
		                        </td>
								<td style="width:100%"><asp:TextBox id="userName" runat="server" SkinId="textBoxSkin" readOnly="true" Width="90%"></asp:TextBox>
		                        </td>
		                    </tr>
                            <tr><td colspan="2" style="height:2px"></td></tr>
		                    <tr>
		                        <td>
                                    Login:
		                        </td>
		                        <td>
                                    <asp:TextBox id="userLogin" MaxLength="50" runat="server" SkinId="textBoxSkin" readOnly="true" Width="90%"></asp:TextBox>
		                        </td>
		                    </tr>
                            <tr><td colspan="2" style="height:2px"></td></tr>
		                    <tr>
		                        <td>
                                    Descr:
		                        </td>
		                        <td>
                                    <asp:TextBox id="userDescr" MaxLength="50" runat="server" SkinId="textBoxSkin"  readOnly="true" Width="90%"></asp:TextBox>
		                        </td>
		                    </tr>
                            <tr><td colspan="2" style="height:2px"></td></tr>
		                    <tr>
		                        <td>
                                    Group:
								</td>
		                        <td>
                                    <asp:TextBox id="userGroup" MaxLength="50" runat="server" SkinId="textBoxSkin" readOnly="true" Width="90%"></asp:TextBox>
								</td>
							</tr>
						</table>
					<!--</div>-->
				</fieldset>		    
		    </td>
		</tr>
        <tr><td colspan="2" style="height:5px"></td></tr>
		<tr>
		    <td colspan="2" align="center">
                <table cellpadding="0" cellspacing="0" border="0" bordercolor="green" width="100%">
                    <tr><td align="right">
                        <input id="idBtnAddLocal" type="button" style="width:100px;" value="Add Local User" onclick="if(onAddEditLocalUser('add'))" onserverclick="addLocalUser_click" runat="server"/>
                    </td><td align="center">
                        <input id="idBtnEditLocal" type="button" style="width:100px;" value="Edit Local User" onclick="if(onAddEditLocalUser('edit'))" onserverclick="editLocalUser_click" runat="server" disabled="disabled"/>
                    </td></tr>
                    <tr><td colspan="2" style="height:5px"></td></tr>
                    <tr><td>&nbsp;</td><td align="center">
                        <input id="idBtnDeleteUserInSubSystem"  type="button" style="width:100px;" value="Delete" onclick="if(onDeleteUserInSubSystem())" onserverclick="deleteUserInSubSystem_click" runat="server" disabled="disabled"/>
                        <input id="btnRefreshUserInSubSystem"  type="button" style="display:none" onserverclick="refreshUserInSubSystem_click" runat="server"/>
                    </td></tr>
                </table>
		    </td>
		</tr>
	</table>
</td>
<td style="width:10px;background-color:rgb(179,179,179)">&nbsp;</td>
<td style="width:69%;height:100%">
	<table id="idMainTable" border="0" bordercolor="olive" cellpadding="0" cellspacing="0" style="height:100%;width:100%">
		<tr>
			<td id="idTDUserGroup" colspan="2" align="center" style="padding:10px 10px 10px 10px;">
				<fieldset class="myFieldsetStyle" style="width:94%;">
					<legend class="myLegend">
						User group
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
                                        <asp:AsyncPostBackTrigger ControlID="idBtnAddSingleUser" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnAddGroup" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnEditGroup" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnDelete" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnEditLocal" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnDeleteUserInSubSystem" EventName="ServerClick" />
                                    </Triggers>
                                    <ContentTemplate>

                                        <iMES:GridViewExt ID="gdGroupsInSubSystem" runat="server" AutoGenerateColumns="true" Width="100%" 
                                            GvExtWidth="100%" GvExtHeight="190px" Height="180px" OnRowDataBound="gdGroupsInSubSystem_RowDataBound"
                                             OnGvExtRowClick="clickGroupsTable(this)" OnGvExtRowDblClick="dblClickGroupsTable(this)" SetTemplateValueEnable="False" 
                                             HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                                        </iMES:GridViewExt>
                                         
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
                                    <input id="idBtnAddSingleUser" type="button" style="width:110px;" value="Add Single User" onclick="if(onAddSingleUser('singleuser'))" onserverclick="addSingleUser_click" runat="server"/>
									&nbsp;&nbsp;
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
				<fieldset class="myFieldsetStyle" style="width:90%">
					<legend class="myLegend">
						Group Members
					</legend>
					<!--<div style="padding:5px">-->
						<table id="idTableUserList" align="center" border="0" cellpadding="0" cellspacing="0">
							<tr style="height:5px">
								<td></td>
							</tr>
							<tr>
								<td>
								
                                    <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional" >
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnRefreshUsersAndPermissionInGroup" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnDeleteUserInSubSystem" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnAddDept" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnAddUser" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="idBtnDeleteUser" EventName="ServerClick" />
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
                                    <input id="idBtnAddDept" type="button" style="width:110px;" value="Add Department" onclick="if(onAddDeptInGroup())" onserverclick="addDeptInGroup_click" runat="server" disabled="disabled"/>
									&nbsp;
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
				<fieldset class="myFieldsetStyle" style="left:-15px;width:90%; padding-right:3px">
					<legend class="myLegend">
						Permission
					</legend>
                            <iMES:WaitingCoverDiv ID="divCover" runat="server"  KeyDownFun="KeyDownEvent()"  />
						    <table id="idTableAuthority" border="0" bordercolor="green" cellpadding="0" cellspacing="0" style="margin:0px 0px 0px 0px">
							    <tr>
								    <td>
								        <div id="idDivSysManagement" style="height:192px;overflow:auto; border:solid 1px rgb(128,128,128); background-color:rgb(212,226,167);padding:0 0 10 0; margin: 0 0 0 0">
									        <table id="idTable1" border="0" bordercolor="red">
										        <tr>
											        <td align="left" valign="top">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" RenderMode="Block">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnRefreshUsersAndPermissionInGroup" EventName="ServerClick" />
                                                                <asp:AsyncPostBackTrigger ControlID="idBtnSave" EventName="ServerClick" />
                                                                <asp:AsyncPostBackTrigger ControlID="idBtnDeleteUserInSubSystem" EventName="ServerClick" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="All">

                                                                    <NodeStyle Width="100%" Font-Names="Verdana" Font-Size="9pt" ForeColor="black"/>
                                                                    <RootNodeStyle Font-Bold="True" Font-Size="9pt"/>
                                                                </asp:TreeView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>   
                                                        
                                                        <input id="txtGroupId" type="hidden" runat="server"/>
                                                        <input id="txtGroupName" type="hidden" runat="server"/>
                                                        <input id="btnQuery" type="button" style="display:none" onclick="" onserverclick="queryClick" runat="server"/>
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
	var sFeature_group = "dialogHeight:280px;dialogWidth:453px;center:yes;status:no;help:no;scroll:no;resizable:no";
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

 document.getElementById("idDivSysManagement").style.width = document.body.clientWidth * 0.28;

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


    //左边区域
	function onPressEnter(e)
	{
        if(e.keyCode == 13)
        {
            document.getElementById("<%=btnRefreshUserInSubSystem.ClientID%>").click();
        }
	}
	
	function onClickGo()
	{
        document.getElementById("<%=btnRefreshUserInSubSystem.ClientID%>").click();
	}
	
	
	//左边的local user表格
    function dblClickTable(row) 
    {
		var objBtnEditLocal = document.getElementById("<%=idBtnEditLocal.ClientID%>");
        if(objBtnEditLocal.disabled == false)
        {
            document.getElementById("<%=idBtnEditLocal.ClientID%>").click();
            //onAddEditLocalUser("edit");
        }
    }     	
	
    function clickUsersInSubSystemTable(row)
    {

        if((selectedRowIndex_UsersInSubSystem!=null) && (selectedRowIndex_UsersInSubSystem!=parseInt(row.index, 10)))
        {
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex_UsersInSubSystem,false, "<%=gdUsersInSubSystem.ClientID %>");                
        }
        
        setRowSelectedOrNotSelectedByIndex(row.index,true, "<%=gdUsersInSubSystem.ClientID %>");
        selectedRowIndex_UsersInSubSystem = parseInt(row.index, 10);


        document.getElementById("<%=userName.ClientID%>").value = row.cells[1].innerText.trim();
        document.getElementById("<%=userName.ClientID%>").title = row.cells[1].innerText.trim();
        document.getElementById("<%=userLogin.ClientID%>").value = row.cells[2].innerText.trim();
        document.getElementById("<%=userLogin.ClientID%>").title = row.cells[2].innerText.trim();
        document.getElementById("<%=userDescr.ClientID%>").value = row.cells[3].innerText.trim();
        document.getElementById("<%=userDescr.ClientID%>").title = row.cells[3].innerText.trim();
        document.getElementById("<%=userGroup.ClientID%>").value = row.cells[4].innerText.trim();
        document.getElementById("<%=userGroup.ClientID%>").title = row.cells[4].innerText.trim();
        document.getElementById("<%=hidUserType.ClientID%>").value = row.cells[5].innerText.trim();
        document.getElementById("<%=hidUserId.ClientID%>").value = row.cells[6].innerText.trim();


        
		var objBtnAddLocal = document.getElementById("<%=idBtnAddLocal.ClientID%>");
		var objBtnEditLocal = document.getElementById("<%=idBtnEditLocal.ClientID%>");
		var objBtnDeleteUserInSubSystem = document.getElementById("<%=idBtnDeleteUserInSubSystem.ClientID%>");

        
		if (document.getElementById("<%=hidUserId.ClientID%>").value == "")
		{
			objBtnEditLocal.disabled = true;
			objBtnDeleteUserInSubSystem.disabled = true;
		}
		else
		{ 
			objBtnDeleteUserInSubSystem.disabled = false;
		    if (document.getElementById("<%=hidUserType.ClientID%>").value == "<%=Constants.DOMAIN_SELECT_ITEM_LOCAL%>")
		    {
    			objBtnEditLocal.disabled = false;
		    }
		    else
		    {
    			objBtnEditLocal.disabled = true;
		    }        
		}
    }	
	
	function onDeleteUserInSubSystem(){
	    var notice = "If the department or user is deleted, they will lose their own authorities. Be sure to delete it?"; 
		if (confirm(notice))
		{
   	        return true;
		}
		else
		{
		    return false;
		}
	}
	
	
    function HighLightUsersInSubSystemTable()
    {
        var gdUsersInSubSystemClientID="<%=gdUsersInSubSystem.ClientID%>";
        var row=eval("setScrollTopForGvExt_"+gdUsersInSubSystemClientID+"('"+document.getElementById("<%=hidUserId.ClientID%>").value+"',6,'','MUTISELECT')");

        if(row != null)
        {
        
            selectedRowIndex_UsersInSubSystem = row.rowIndex - 1;
        }
        else{
            selectedRowIndex_UsersInSubSystem = null;
            document.getElementById("<%=userName.ClientID%>").value = "";
            document.getElementById("<%=userName.ClientID%>").title = "";
            document.getElementById("<%=userLogin.ClientID%>").value = "";
            document.getElementById("<%=userLogin.ClientID%>").title = "";
            document.getElementById("<%=userDescr.ClientID%>").value = "";
            document.getElementById("<%=userDescr.ClientID%>").title = "";
            document.getElementById("<%=userGroup.ClientID%>").value = "";
            document.getElementById("<%=userGroup.ClientID%>").title = "";
            document.getElementById("<%=hidUserType.ClientID%>").value = "";
            document.getElementById("<%=hidUserId.ClientID%>").value = "";


            
		    var objBtnAddLocal = document.getElementById("<%=idBtnAddLocal.ClientID%>");
		    var objBtnEditLocal = document.getElementById("<%=idBtnEditLocal.ClientID%>");
		    var objBtnDeleteUserInSubSystem = document.getElementById("<%=idBtnDeleteUserInSubSystem.ClientID%>");

            
		    objBtnAddLocal.disabled = false;
		    objBtnEditLocal.disabled = true;
		    objBtnDeleteUserInSubSystem.disabled = true;
		}    
    }	
	
    function OnDblClick_NewUser()
    {
		var objSelDomain = document.getElementById("idSelDomain");
        if(objSelDomain.value == "<%=Constants.DOMAIN_SELECT_ITEM_LOCAL%>")
        {
	        if(ShowNewUserListTable.EventRow() == "Head"  || ShowNewUserListTable.EventRow() == undefined || ShowNewUserListTable.IsEmpty)

		        return;
            if(ShowNewUserListTable.GetRowNumber() == -1 || ShowNewUserListTable.GetRowNumber() >= ShowNewUserListTable.rs_main.recordcount)
            {
                return;
            }
	        onAddEditLocalUser("add");
	    }
    }
    

    
    function onAddEditLocalUser(interfaceType){
        //判断是否超时
		var systimeout = webroot_aspx_authorities_accountauthority.getSystemTimeOut();
	    if (systimeout.error != null)
	    {
		    alert(systimeout.error.Message);
		    return;
	    }
    
    	var sFeature_localuser = "dialogHeight:400px;dialogWidth:380px;center:yes;status:no;help:no;scroll:no;resizable:no";
		diagArgs_user.localUserID = document.getElementById("<%=hidUserId.ClientID%>").value;
		diagArgs_user.interfaceType = interfaceType;
		diagArgs_user.editorID = document.getElementById("<%=hidEditorId.ClientID%>").value;


		var result = window.showModalDialog("addeditlocaluser.aspx", diagArgs_user, sFeature_localuser);
		if (result != undefined && result != "cancel")
		{
		    return true;
		}
		else
		{
		    return false;
		}
    }
 

		
	
	//右边区域
    //保存permission，查询存盘状态，server端置状态doing-->success，报成功。
    function saveClick_Complete()
    {
        alert("Save successfully!");
    }

    //右边区域
    //上面的表格group
	function onAddSingleUser(parUser)
	{
        //判断是否超时
		var systimeout = webroot_aspx_authorities_accountauthority.getSystemTimeOut();
	    if (systimeout.error != null)
	    {
		    alert(systimeout.error.Message);
		    return;
	    }
	    
	    
		diagArgs_user.operateType = parUser;
		diagArgs_user.interfaceType = "singleuser";
		diagArgs_user.editorID = document.getElementById("<%=hidEditorId.ClientID%>").value;
		var userIDs = window.showModalDialog("selectuser.aspx", diagArgs_user, sFeature_user);
		
		if (userIDs != undefined)
		{
			//alert(userIDs);
			//getUserGroupRecordSet();
			document.getElementById("<%=hidGroupId.ClientID%>").value=userIDs;
			return true;
			
		}
		else
		{
			//user canceled
			return false;
		}
	}

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
            document.getElementById("<%=hidGroupId.ClientID%>").value = row.cells[2].innerText.trim();
            document.getElementById("<%=hidGroupName.ClientID%>").value = row.cells[1].innerText.trim();
            document.getElementById("<%=hidGroupType.ClientID%>").value = row.cells[8].innerText.trim();
            document.getElementById("<%=hidGroupComment.ClientID%>").value = row.cells[7].innerText.trim();
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
   		    var objBtnAddDept = document.getElementById("<%=idBtnAddDept.ClientID%>");
		    var objBtnDelUser = document.getElementById("<%=idBtnDeleteUser.ClientID%>");
		    var objBtnSave = document.getElementById("<%=idBtnSave.ClientID%>");

            
		    objBtnEditGroup.disabled = true;
		    objBtnDelete.disabled = true;
		    objBtnAddUser.disabled = true;
		    objBtnAddDept.disabled = true;
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
	    var objBtnAddDept = document.getElementById("<%=idBtnAddDept.ClientID%>");
	    var objBtnDelUser = document.getElementById("<%=idBtnDeleteUser.ClientID%>");
		var objBtnSave = document.getElementById("<%=idBtnSave.ClientID%>");
		
		if (parOpType == "edit")
		{
		/*
			var strGroupID = document.getElementById("<%=hidGroupId.ClientID%>").value;
			if (strGroupID == "")
			{
				return;
			}
			*/
			//var arrGroupInfo = getGroupInfoByID(strGroupID);
			diagArgs_group.operateType = parOpType;
			diagArgs_group.editorID = document.getElementById("<%=hidEditorId.ClientID%>").value;
			diagArgs_group.groupID = document.getElementById("<%=hidGroupId.ClientID%>").value;
			diagArgs_group.groupName = document.getElementById("<%=hidGroupName.ClientID%>").value;
			diagArgs_group.groupComment = document.getElementById("<%=hidGroupComment.ClientID%>").value;
			var editedGroupID = window.showModalDialog("editgroup.aspx", diagArgs_group, sFeature_group);
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


        document.getElementById("<%=hidGroupId.ClientID%>").value = row.cells[2].innerText.trim();
        document.getElementById("<%=hidGroupName.ClientID%>").value = row.cells[1].innerText.trim();
        document.getElementById("<%=hidGroupType.ClientID%>").value = row.cells[8].innerText.trim();
        document.getElementById("<%=hidGroupComment.ClientID%>").value = row.cells[7].innerText.trim();


        document.getElementById("<%=btnRefreshUsersAndPermissionInGroup.ClientID%>").click();
        
        
		var objBtnEditGroup = document.getElementById("<%=idBtnEditGroup.ClientID%>");
		var objBtnDelete = document.getElementById("<%=idBtnDelete.ClientID%>");
	    var objBtnAddUser = document.getElementById("<%=idBtnAddUser.ClientID%>");
	    var objBtnAddDept = document.getElementById("<%=idBtnAddDept.ClientID%>");
	    var objBtnDelUser = document.getElementById("<%=idBtnDeleteUser.ClientID%>");
		var objBtnSave = document.getElementById("<%=idBtnSave.ClientID%>");

        
		if (document.getElementById("<%=hidGroupName.ClientID%>").value == "")
		{
			objBtnEditGroup.disabled = true;
			objBtnDelete.disabled = true;
			objBtnAddUser.disabled = true;
			objBtnAddDept.disabled = true;
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
			    objBtnAddDept.disabled = true;
			    objBtnDelUser.disabled = true;
			    objBtnSave.disabled = false;
		    }
		    else
		    {
			    objBtnEditGroup.disabled = false;
			    objBtnDelete.disabled = false;
			    objBtnAddUser.disabled = false;
			    objBtnAddDept.disabled = false;
			    objBtnDelUser.disabled = true;
			    objBtnSave.disabled = false;
		    }        
		}
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


	function onAddDeptInGroup()
	{
        //判断是否超时
		var systimeout = webroot_aspx_authorities_accountauthority.getSystemTimeOut();
	    if (systimeout.error != null)
	    {
		    alert(systimeout.error.Message);
		    return;
	    }
        	
        diagArgs_dept.groupID = document.getElementById("<%=hidGroupId.ClientID%>").value;
        diagArgs_dept.editorID = document.getElementById("<%=hidEditorId.ClientID%>").value;
		
		var result = window.showModalDialog("selectdepartment.aspx", diagArgs_dept, sFeature_user);
		if (result != undefined && result != "")
		{
			return true;
		}
		else
		{
		    return false;
			//user canceled
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


		var objBtnAddDept = document.getElementById("<%=idBtnAddDept.ClientID%>");
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

	function getField(columnIndex, ItemTableString)
	{
		var fiedValue = "";
		var ShowItemTable = eval(ItemTableString);
		var nRowNO = ShowItemTable.GetRowNumber();
		if (ShowItemTable.IsEmpty || nRowNO > (ShowItemTable.rs_main.recordcount - 1) || nRowNO < 0)
		{
			return "";
		}

		ShowItemTable.rs_main.absolutePosition = ShowItemTable.GetRowNumber() * 1 + 1; //将表格数据集定位到高亮行
		fiedValue = ShowItemTable.rs_main.fields(columnIndex).value;

		return fiedValue;
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
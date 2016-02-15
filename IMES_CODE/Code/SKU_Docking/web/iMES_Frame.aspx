<%--
 INVENTEC corporation (c)2009 all rights reserved. 
 Description: iMES Frame Page
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-28  Li.MingJun (eB1)     Create 
 2010-01-06  Li.MingJun (eB1)     Modify: ITC-1103-0010 
 2010-01-07  Li.ZhuoYin (eB1)     Modify: ITC-1103-0020 
 2010-01-07  Li.MingJun (eB1)     Modify: ITC-1103-0033 
 2010-01-11  Li.MingJun (eB1)     Modify: ITC-1103-0028 
 2010-01-19  Li.MingJun (eB1)     Modify: ITC-1103-0109 
 2010-01-20  Li.ZhuoYin (eB1)     Modify: ITC-1103-0095 
 2010-01-20  Li.ZhuoYin (eB1)     Modify: ITC-1103-0116 
 2010-02-04  Li.MingJun (eB1)     Modify: ITC-1103-0147 
 2010-02-26  Li.MingJun (eB1)     Modify: ITC-1122-0144
 Known issues:
 --%>
<%@ Page Language="C#" Theme="FrameCSS" AutoEventWireup="true"  CodeFile="iMES_Frame.aspx.cs" Inherits="iMES_Frame" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>iMES Framework</title>
    <script type="text/javascript" src="./CommonControl/JS/btnTabs.js"></script>
    <script type="text/javascript" src="./CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="./CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
</head>
<body >
    <table width="100%" cellpadding="0px" cellspacing="0px" border="0px">
        <tr>
            <td width="254px">
                <table cellpadding="0px" cellspacing="0px" border="0px" class="PCode_backgroud">
                    <tr>
                        <td rowspan="2" class="PCode_ImgLeft"></td>
                        <td width="60px" align="center" class="PCode" height="27px">P Code: </td>
                        <td width="170px" align="center" valign="middle">
                            <input id="txtPCode" style="width:90%;height:20px;" onkeyup="txtPCode_OnKeyup();" onkeypress="inputNumberAndEnglishChar(this);" />
                        </td>
                        <td rowspan="2" class="PCode_ImgRight"></td>
                    </tr>
                    <tr>
                        <td width="230px" colspan="2" class="Tree_backgroud" height="8px"></td>
                    </tr>
                </table>
            </td>
            <td>
                <table width="100%" cellpadding="0px" cellspacing="0px" border="0px">
                    <tr>
                        <td class="Tree_backgroud" style="height:23px;"></td>
                    </tr>
                    <tr>
                        <td class="Tab_Img"></td>
                    </tr>
                </table>
            </td>
            <td>
                <table width="100%" cellpadding="0px" cellspacing="0px" border="0px">
                    <tr>
                        <td>
                            <div id="divTab" style="height:100%;"></div>
                        </td>
                    </tr>
                </table>
            </td>            
        </tr>
    </table>

    <table id="tblTreeLeft"  width="100%" cellpadding="0px" cellspacing="0px" border="0px">
        <tr>
            <td width="7px" rowspan="3" class="PCode_backgroud" onmouseover="OpenTree();" onmouseout="CloseTree();"></td>
            <td id="tdTreeLeft" width="7px" class="Tree_backgroud" onmouseover="OpenTree();" onmouseout="CloseTree();"></td>
            <td rowspan="3" id="tdFunction"></td>
        </tr>
        <tr>
            <td class="TreeLeft_ImgBottom"></td>
        </tr>
        <tr>
            <td height="10px" class="PCode_backgroud"></td>
        </tr>
    </table>

    <form id="frmMain" runat="server">
        <div id="divTreeContainer" class="divTreeContainer" onmouseover="OpenTree();" onmouseout="();">
            <table id="tblTree" width="100%" cellpadding="0px" cellspacing="0px" border="0px">
                <tr>
                    <td id="tdTree">
                    
                    
                        <div id="divTree" class="Tree_backgroud" style="overflow:auto;width:310px;height:100%;">
                            <asp:TreeView ID="treeFunction" NodeIndent="16" NodeStyle-CssClass="TreeNode" 
                                CollapseImageUrl="./Images/node-close.gif" 
                                ExpandImageUrl="./Images/node-open.gif" runat="server"
								onClick="disableOnBeforeUnload();"></asp:TreeView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height="10px" class="PCode_backgroud"></td>
                </tr>
            </table>
            <iframe id="iframe_hide" src="javascript:false" class="frameHide" frameborder="0" marginwidth="0" marginheight="0" scrolling="no"></iframe>
        </div>
    </form>
    
    <div id="divTreeControl" class="divTreeControl" onmouseover="OpenTree();" onmouseout="CloseTree();">
        <table id="tblTreeControl" width="100%" cellpadding="0px" cellspacing="0px" border="0px">
            <tr>
                <td height="100%">
              
                </td>
            </tr>
        </table>
    </div>
</body>

<script type="text/javascript">
    function S() {
        document.getElementById("divTreeContainer").style.height = "877px";
    }
if ("<% =Domain %>" != "")
    document.domain = "<% =Domain %>";
var _tblTreeH = 0;
isFrame = true;
isAddTab = true;

window.onload = function() {
    var stdHeight = (treeModleHeight - 17).toString() + 'px';
  
    $('#divTreeContainer').css('overflowY', 'scroll');
    $('#divTreeContainer').css('overflowX', 'hidden');
    document.getElementById("divTreeContainer").style.height = stdHeight;
    if ($('#tblTree').height() < _tblTreeH) {
         $('#tblTree').css('height', _tblTreeH.toString() + "px");
        $('#tdTree').css('height', _tblTreeH.toString() + "px");
    }

}
_tblTreeH = $('#tblTree').height();


var treeModleHeight = window.screen.availHeight - (window.screenTop ? window.screenTop : window.screenY) - 35;
   //var treeModleHeight = (window.screen.availHeight == 0 ? getDocHeight() : window.screen.availHeight) - window.screenTop - 35;
   tblTreeLeft.height = treeModleHeight.toString() + 'px';
   tblTree.height = treeModleHeight.toString() + 'px';
   tblTreeControl.height = treeModleHeight.toString() + 'px';
   tdTreeLeft.height = (treeModleHeight - 17).toString()+'px';
   tdTree.height = (treeModleHeight - 112).toString()+'px';
  //document.all.iframe_hide.height = treeModleHeight;
  document.getElementsByTagName("*").iframe_hide.height = treeModleHeight.toString()+'px';

 //Set tab area width
 divTab.style.width = (window.screen.availWidth- 279).toString()+'px';

 //Set function area width
 tdFunction.width = (window.screen.availWidth - 14).toString()+'px';

//Control for tree open or close
function CloseTree()
{
    divTreeContainer.style.display = "none";
    divTreeControl.className = "divTreeControlClose";
}

function OpenTree()
{
    divTreeContainer.style.display = "";
    divTreeControl.className = "divTreeControl";
    //解决动态的树在IE8下Div滚动条总回到顶端的问题，奇怪的是只需要读取一下滚动条的位置就正常了
    var scrollTop = document.getElementById("divTree").scrollTop;
}

//Control for tab
var tabsObj;
var tabsArr = new Array();
var isSelectTab = false;

createBtnTabs();

function createBtnTabs()
{
	tabsObj = new clsButtonTabs("tabsObj");

    tabsObj.selectChanged = tab_click;
    tabsObj.preTabAdd = beforeAddTab;
    tabsObj.preTabDeleted = beforeDeleteTab;
    tabsObj.tabDeleted = afterDeleteTab;
    tabsObj.container = divTab;
    tabsObj.mode = "text";
	
	tabsObj.toString();
}

//Tree node click
function node_click(spanObj, tabText)
{
    var linkObj = spanObj.parentNode;
    var url = linkObj.href;

    linkObj.onclick = function() {return false;}

    handleTab(tabText, url);
}

//Handle tab
function handleTab(tabText, url)
{
    var currentDiv = replaceAllSpace("div" + tabText);
    var currentFra = replaceAllSpace("ifra" + tabText);

    //判断当前功能在tab集合中是否已经存在，存在则直接显示该tab，不存在则增加该tab
    if (!showTab(tabText, currentDiv))
    {
        addTab(tabText, currentDiv, currentFra, url);
    }
    
    //Modify: ITC-1103-0028
    CloseTree();
}

//Tab click
function tab_click()
{
    if (isSelectTab)
        isSelectTab = false;
    else
    {
        var tabText = tabsObj.getTabText(tabsObj.selectedButton.index);
        var currentDiv = replaceAllSpace("div" + tabText);
        
        showTab(tabText, currentDiv);
    }
    
    //Modify: ITC-1103-0028
    CloseTree();
}

//P Code enter
function txtPCode_OnKeyup()
{
    if (event.keyCode == 13)
    {
        var retVal = iMES_Frame.getFunctionByPcode(txtPCode.value);
        if (retVal.error != null)
            alert(retVal.error.Message);
        else
        {
            var arr = (retVal.value).split("|");
            if (arr[0] != "Error")
            {
                var tabText = arr[0];
                var url = arr[1];

                handleTab(tabText, url);
        
                //Modify: ITC-1103-0010
                txtPCode.value = "";
            }
            else
                alert(arr[1]);
        }
    }
}

//Show tab
function showTab(tabText, currentDiv)
{
    var divObj;
    var isExist = false;
    var tabCount = tabsObj.buttons.length;

    for (var i = 0; i < tabCount; i++)
    {
        if (tabText == tabsObj.buttons[i].text)
        {
            isExist = true;

            var oldTabText = tabsObj.getTabText(tabsObj.lastSelectedIndex);
            var oldFra = replaceAllSpace("ifra" + oldTabText);
            if (window.frames[oldFra].document.readyState != "complete" || window.frames[oldFra].IsCoverDivWaiting())
            {
                tabsObj.selectButton(tabsObj.lastSelectedIndex);
            }
            else
            {
                tabsObj.selectButton(i);
                
                for (var j = 0; j < tabsArr.length; j++)
                {
                    if (tabsArr[j] != currentDiv)
                    {
                        divObj = document.getElementById(tabsArr[j]);
                        divObj.style.display = "none";
                    }
                }

                divObj = document.getElementById(currentDiv);
                divObj.style.display = "block";

                var currentFra = replaceAllSpace("ifra" + tabText);
                try
                {
                    window.frames[currentFra].window.getCommonInputObject().AllowPrompt = false;
                    window.frames[currentFra].window.getCommonInputObject().focus();
                }
                catch (e)
                {
                    window.frames[currentFra].window.focus();
                }

                break;
            }
        }
    }
    
    return isExist;
}

//Add tab
function addTab(tabText, currentDiv, currentFra, url)
{
    var divObj;
    isSelectTab = true;

    tabsObj.dynaAddDel("Add");
    if (isAddTab)
    {
        tabsObj.setTabText(tabsObj.selectedButton.index, tabText);
        
        for (var i = 0; i < tabsArr.length; i++)
        {
            divObj = document.getElementById(tabsArr[i]);
            divObj.style.display = "none";
        }
        
        //Modify: ITC-1103-0033
        var newDiv = document.createElement('Div');
        newDiv.id = currentDiv;
        newDiv.style.top = "0px";
        newDiv.style.left = "0px";
        newDiv.style.width = "100%";
        newDiv.style.height = treeModleHeight.toString() + "px";
        newDiv.innerHTML = '<iframe id=\"' + currentFra + '" name="' + currentFra + '" style="width:100%;height:' + treeModleHeight.toString() + 'px;border:0px;"  frameborder="0px" allowtransparency="true" src="' + url + '"></iframe></div>'; 
        tdFunction.appendChild(newDiv);
        
        tabsArr.push(currentDiv);
    }
    else
        isAddTab = true;
}

//before delete tab
function beforeDeleteTab(tabIndex)
{
    isSelectTab = true;
    var tabText = tabsObj.getTabText(tabIndex);
    var currentDiv = replaceAllSpace("div" + tabText);
    var currentFra = replaceAllSpace("ifra" + tabText);

    //Clear session for current tab
    //Modify: ITC-1103-0147
    //Modify: ITC-1122-0144
//    try
//    {
//        window.frames[currentFra].ExitPage();
//    }
//    catch (err) {}
    document.getElementById(currentFra).src = "";
    
    //Remove current div
    var divObj = document.getElementById(currentDiv);
    //divObj.removeNode(true);
    divObj.parentNode.removeChild(divObj);
    
    //Remove current div record from tabs array
    for (var i = 0; i < tabsArr.length; i++)
        if (tabsArr[i] == currentDiv)
            break;

    tabsArr.splice(i, 1);
}

//After delete tab
function afterDeleteTab(tabIndex)
{
    var tabText = tabsObj.getTabText(tabsObj.buttons.length - 1);
    var currentDiv = replaceAllSpace("div" + tabText);
    var divObj = document.getElementById(currentDiv);
    divObj.style.display = "block";
    
    //Modify: ITC-1103-0028
    CloseTree();
}

//Before add tab
function beforeAddTab()
{
    var count = tabsObj.buttons.length;
    if (count > 0)
    {
        var tabText = tabsObj.getTabText(tabsObj.selectedButton.index);
        var currentFra = replaceAllSpace("ifra" + tabText);

        if (window.frames[currentFra].document.readyState != "complete" || window.frames[currentFra].IsCoverDivWaiting())
        {
            isAddTab = false;
            return false;
        }
    }
    return true;
}

//Change tab
function changeTab()
{
    var tabIndex = tabsObj.selectedButton.index;
    var tabCount = tabsObj.buttons.length;
    
    if (tabIndex != tabCount -1)
        tabsObj.selectButton(tabIndex + 1);
    else
        tabsObj.selectButton(0);
}

//Replace all space
function replaceAllSpace(text)
{
    return text.replace(/\s+/g,"");
}

//Vincent add handle treeView expand or collapse
 
var OnBeforeUnloadDisabled = false;
function disableOnBeforeUnload()
{
  OnBeforeUnloadDisabled = true;
  setTimeout("OnBeforeUnloadDisabled = false;", 1000);
  return true;
}


window.onbeforeunload = windowClose;

function windowClose() {
    var closeEvent = false;
    
    //if (event.clientY < 0)
	if (!OnBeforeUnloadDisabled ||event.clientY < 0){
	        closeEvent = true;
	}
	
	OnBeforeUnloadDisabled=false;
	
    if ((closeEvent)) {
        //alert(event.clientY);

        iMES_Frame.ClearSessionId('<%= Request["SessionId"] %>');            
    }
	
	
}
</script>

</html>

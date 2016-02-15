<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Data Collection
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-11-25  Lucy Liu(EB2)        Create 
 Known issues:
 --%>
 
<%@ Page Language="C#" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="CollectionData.aspx.cs" Inherits="_CollectionData" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Data Collection</title>
    
</head>
<script type="text/javascript" src="CommonControl/JS/iMESCommonUse.js"></script>

<body onload="initCollectionData()"  class="background_common" onkeydown="disableESCOnBody()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="100%">
                    
                    <tr>
                        <td>
                        </td>
                        <td>
                         
         
             
                        <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoHighlightScrollByValue="true" GetTemplateValueEnable="False"
                            GvExtHeight="240px" GvExtWidth="100%" Width="98%" Height="230px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                            SetTemplateValueEnable="False" AutoGenerateColumns="False" HighLightRowPosition="3">
                        <Columns >   
                            
                    
                            <asp:BoundField DataField="No">
                                <itemstyle width="20%" />
                                <headerstyle width="20%" />
                            </asp:BoundField>
                             <asp:BoundField DataField="PartNo">
                                <itemstyle />
                                <headerstyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="Data">
                                <itemstyle />
                                <headerstyle />
                            </asp:BoundField>
                        </Columns>
                        </iMES:GridViewExt>
                        </td>
                        <td>
                        </td>
                    </tr>                    
                </table>     
                <table style="width:100%;">
                    <tr>
                        <td>
                        </td>
                        <td align="right">
                            <asp:Button ID="Close" runat="server" CssClass="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
<script language="javascript" type="text/javascript">

var strRowsCount = "<%=initRowsCount%>";
var initRowsCount = parseInt(strRowsCount,10) ;
var index = 1;
var GridViewExt1ClientID = "<%=GridViewExt1.ClientID%>";

function initCollectionData()
{
    var data = new dataInfo();
    data = window.dialogArguments;
    
    if (undefined == data || undefined == data.PartNo || undefined == data.DataCllist || undefined == data.PartNoCllist)
    {
        var message = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_ParamNull") %>';
        ShowMessageByUrl("ErrMessageDisplay.aspx",message);
        window.close();
        return;
    }
    
    
    var dataCllist = new Array();
    dataCllist = data.DataCllist;
    var dataClPartNolist = new Array();
    dataClPartNolist = data.PartNoCllist;
    
    var table=document.getElementById("GridViewExt1"); 
    
//    for(var i = 1; i < table.rows.length; i)
//    {
//        table.deleteRow(i);
//    }
        
    for(var i = 0; i < dataCllist.length; i++)
    {
//        var arr = new Array();
//        arr[0] = i+1;
//        arr[1] = datalist[i];
//        AddCvExtRowToBottom_GridViewExt1(arr,false);
          var rowInfo=new Array();
          rowInfo.push(i+1);
          rowInfo.push(dataClPartNolist[i]);
          rowInfo.push(dataCllist[i]);
          AddRowInfo(rowInfo); 
    }

//    for(var i = table.rows.length; i < 12; i++)
//    {
//        var arr = new Array();
//        arr[0] = " ";
//        arr[1] = " ";
//        AddCvExtRowToBottom_GridViewExt1(arr,false);
//    }
    
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	AddRowInfo
//| Author		:	Lucy Liu
//| Create Date	:	10/27/2009
//| Description	:	向表格中添加一行
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function AddRowInfo(RowArray)
{
    try {
        if (index <= initRowsCount) {
            eval("ChangeCvExtRowByIndex_" +GridViewExt1ClientID+"(RowArray,false, index)");
        } else 
        {
            eval("AddCvExtRowToBottom_"+GridViewExt1ClientID+"(RowArray,false)");
        }
        setSrollByIndex(index ,false);
        index++;       
        
     }catch (e) {
        alert(e.description);
       
    }
}

function OnCloseClick()
{
    window.close();
}
</script>    
</body>
</html>

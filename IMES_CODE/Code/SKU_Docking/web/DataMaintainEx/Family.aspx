<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="Family.aspx.cs" Inherits="DataMaintain_Family" ValidateRequest="false" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>



<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<style type="text/css">

.iMes_div_MainTainEdit
{
    border: thin solid Black; 
    background-color: #99CDFF;
    margin:0 0 20 0;
    
}

</style>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/DataMaintain/Service/Family.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1" style="margin-top:15;">
             <table width="100%" class="iMes_div_MainTainEdit" >            
                <tr >
                    <td style="width: 70px;">
                        <asp:Label ID="lblFamilyTop" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="42%">
                        <asp:TextBox ID="dFamilyTop" runat="server"   MaxLength="50"  Width="86%" CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)' ></asp:TextBox>
                    </td>                                    
                    <td style="width: 80px;">
                        <asp:Label ID="lblCustomer" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="42%">
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnFamilyChange" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnRefreshCustomerList" EventName="ServerClick" />
        </Triggers>
        <ContentTemplate>                    
                        <iMESMaintain:CmbCustomer runat="server" ID="cmbCustomer" Width="86%" ></iMESMaintain:CmbCustomer>
        </ContentTemplate>
        </asp:UpdatePanel>                            
                    </td>    
           
                </tr>
             </table>  
                                                    
             <table width="100%" border="0" >
                <tr>
                    <td style="width: 100px; padding-left: 2px;">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="50%">  
                        <asp:TextBox ID="dSearch" runat="server"   MaxLength="50"  Width="70%" CssClass="iMes_textbox_input_Yellow" onkeypress='OnKeyPress(this)'></asp:TextBox>                      
                    </td>    
                    <td width="40%" align="right">
                      <input type="hidden" id="hidModelName" runat="server" value=""/>
                      <input id="btnDelete" disabled type="button"  runat="server" class="iMes_button" onclick="if(clkDelete())" onserverclick="btnDelete_ServerClick" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                      <input id="btnInfoName" type="button"  runat="server" class="iMes_button" onclick="onInfoName();" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                      <input id="btnInfo" disabled type="button"  runat="server" class="iMes_button" onclick="onInfo('');" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                    </td>           
                </tr>
             </table>  
        </div>

        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRefreshFamilyList" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
        </Triggers>
        <ContentTemplate>
        <div id="div2" style="height:390px">

                    <input id="hidRecordCount" type="hidden" runat="server" />
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
                        Width="100%" RowStyle-Height="20" 
                        GvExtWidth="100%" GvExtHeight="376px" AutoHighlightScrollByValue ="true" 
                        HighLightRowPosition="3"  Height="366px" 
                        OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)' 
                        OnGvExtRowDblClick="dblClickTable(this)"
                        onrowdatabound="gd_RowDataBound" EnableViewState= "false" 
                        style="top: 0px; left: 26px">
                    </iMES:GridViewExt>
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit" >
                    <tr>
                        <td style="width: 90px;">
                            <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="80%">
                            <asp:TextBox ID="dFamily" runat="server"   MaxLength="50"   Width="96%"  CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                        </td>    
                        <td align="center">
                            <button id="btnAdd" runat="server" onclick="if(clkAdd())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"  onserverclick="btnAdd_ServerClick"></button>
                        </td>           
                    </tr>
                    <tr>
                        <td style="width: 90px;">
                            <asp:Label ID="lblDescription" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="80%">
                            
                            <asp:TextBox ID="dDescription" runat="server"   MaxLength="50"  Width="96%" SkinId="textBoxSkin" ></asp:TextBox>
                        </td>    
                        <td align="center">
                            <button id="btnSave" runat="server" onclick="if(clkSave())" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onserverclick="btnSave_ServerClick"></button>
                            <input type="hidden" id="dOldFamilyId" runat="server" />
                        </td>           
                    </tr>                    
                    
                    <tr>
                        <td style="width: 90px;">
                            <asp:Label ID="lblcustomers" runat="server" CssClass="iMes_label_13pt" Text="Customer:"></asp:Label>
                        </td>
                        <td width="80%">
                            <asp:DropDownList ID="cmbcustom" runat="server" Width="97%"></asp:DropDownList>
                        </td>    
                        <td align="center">
                        
                        </td>              
                    </tr>           
                    
            </table> 
        </div>  
        </ContentTemplate>
        </asp:UpdatePanel>      
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="HiddenCustomerName" runat="server" />
        <button id="btnFamilyChange" runat="server" type="button" style="display:none" onserverclick ="btnFamilyChange_ServerClick"> </button>
        <button id="btnRefreshFamilyList" runat="server" type="button" onclick="" style="display: none" onserverclick="btnRefreshFamilyList_Click"></button>
        <button id="btnRefreshCustomerList" runat="server" type="button" onclick="" style="display: none" onserverclick="btnRefreshCustomerList_Click"></button>

    </div>
    
    <script language="javascript" type="text/javascript">
    var customerObj;
    
    function OnKeyPress(obj)
    {
        var key = event.keyCode;
        
        if (key==13)//enter
        {
            if(event.srcElement.id=="<%=dFamilyTop.ClientID %>")
            {
                if(document.getElementById("<%=dFamilyTop.ClientID %>").value.trim()!="")
                {
                    document.getElementById("<%=btnFamilyChange.ClientID %>").click();
                }
                
            }
            else if(event.srcElement.id=="<%=dSearch.ClientID %>")
            {
                var value=document.getElementById("<%=dSearch.ClientID %>").value.trim().toUpperCase();
                if(value!="")
                {
                    findFamily(value, true);
                }
            }
        }       

    }
    
    function findFamily(searchValue, isNeedPromptAlert)
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=0;i<gdObj.rows.length;i++)
        {
           if(searchValue.trim()!="" && gdObj.rows[i].cells[0].innerText.indexOf(searchValue)==0)
           {
               selectedRowIndex=i;
               break;  
           }        
        }
        
        if(selectedRowIndex<0)
        {
            if(isNeedPromptAlert==true)
            {
//                alert("Cant find that match family.");   //1  
                alert(msg1);     
            }
            else if(isNeedPromptAlert==null)
            {
                ShowRowEditInfo(null);                 
            }
            return;
        }
        else
        {            
            var con=gdObj.rows[selectedRowIndex];
            //去掉标题行
            selectedRowIndex=selectedRowIndex-1;
            setRowSelectedByIndex_<%=gd.ClientID%>(selectedRowIndex, false, "<%=gd.ClientID%>");
            setSrollByIndex(selectedRowIndex, true, "<%=gd.ClientID%>");
            ShowRowEditInfo(con);
        }    
    }
   
    //var familyObj;
    var descriptionObj;
    
    var msg1="";
    var msg2="";
    var msg3="";
    var msg4="";
    var msg5="";


    window.onload = function()
    {
        customerObj = getCustomerCmbObj();
        customerObj.onchange = addNew;
        
        msg1 ="<%=pmtMessage1%>";
        msg2 ="<%=pmtMessage2%>";
        msg3 ="<%=pmtMessage3%>";
        msg4 ="<%=pmtMessage4%>";
        msg5 ="<%=pmtMessage5%>";

        document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=userName%>";
        ShowRowEditInfo(null);
        
    };
    
    function addNew()
    {
        if(getCustomerCmbValue() == "$$$")
        {
            //alert(getCustomerCmbValue());

            var dlgFeature = "dialogHeight:250px;dialogWidth:450px;center:yes;status:no;help:no";
            var dlgReturn=window.showModalDialog("FamilyCreateCustomer.aspx?editor=<%=Master.userInfo.UserId%>", window, dlgFeature);

            if(dlgReturn)
            {
                document.getElementById("<%=HiddenCustomerName.ClientID %>").value = dlgReturn;
                document.getElementById("<%=dFamilyTop.ClientID%>").value = "";
                document.getElementById("<%=dSearch.ClientID%>").value = "";
                document.getElementById("<%=btnRefreshCustomerList.ClientID%>").click();
            }else{
                customerObj.selectedIndex = 0;
                document.getElementById("<%=btnRefreshFamilyList.ClientID%>").click();
            }
        }else{

            document.getElementById("<%=btnRefreshFamilyList.ClientID%>").click();
        
        }
        
    }
   
//    String.prototype.Trim = function() 
//    { 
//        return this.replace(/(^\s*)|(\s*$)/g, ""); 
//    } 
    
    function clkDelete()
    {
        ShowInfo("");
        var gdObj=document.getElementById("<%=gd.ClientID %>")
        var curIndex = gdObj.index;
        var recordCount = document.getElementById("<%=hidRecordCount.ClientID %>").value;            
        if(curIndex>=recordCount)
        {
//            alert("Please select one row!");   //2
            alert(msg2);
            return false;
        }
        
//         var ret = confirm("Do you really want to delete this item?");  //3
         var ret = confirm(msg3);  //3
         if (!ret) {
             return false;
         }
         
         return true;
        
    }
   
   function DeleteComplete()
   {   
       ShowRowEditInfo(null);
   }
   
   function clkSave()
   {
       ShowInfo("");
       var familyValue = document.getElementById("<%=dFamily.ClientID %>").value;   
       var descriptionValue = document.getElementById("<%=dDescription.ClientID %>").value;   
       var customer = document.getElementById("<%=cmbcustom.ClientID %>").value;   
       if(familyValue.trim()=="")
       {
//           alert("Please input [Family] first!!");  //4
           alert(msg4);
           return false;
       }
        if(customer.trim()=="")
        {
            alert("Please Select [Customer]");
            return false;
        }  
        return true;
   }
   
   function clkAdd()
   {
        ShowInfo("");
       var familyValue = document.getElementById("<%=dFamily.ClientID %>").value;   
       var descriptionValue = document.getElementById("<%=dDescription.ClientID %>").value;
       var customer = document.getElementById("<%=cmbcustom.ClientID %>").value;   
       if(familyValue.trim()=="")
       {
//           alert("Please input [Family] first!!");  //4
           alert(msg4);
           return false;
       }
       
       if(CheckIsDup(familyValue.trim()) )
       {
           alert("Family已存在");
           return false;
       }
       if(customer.trim()=="")
        {
            alert("Please Select [Customer]");
            return false;
        }  
       return true;

   }
   
    function clickTable(row)
    {
         ShowInfo("");
        
         var selectedRowIndex = row.index;
         setRowSelectedByIndex_<%=gd.ClientID%>(row.index, false, "<%=gd.ClientID%>");         
         ShowRowEditInfo(row);
       if(row == null){
           document.getElementById("<%=hidModelName.ClientID%>").value = "";
           document.getElementById("<%=btnInfo.ClientID%>").disabled = true;
           return;
       }
       if(row.cells[0].innerText.trim() == ""){
            document.getElementById("<%=btnInfo.ClientID%>").disabled = true;
            document.getElementById("<%=hidModelName.ClientID%>").value = "";
       }
       else{
            document.getElementById("<%=btnInfo.ClientID%>").disabled = false;
            document.getElementById("<%=hidModelName.ClientID%>").value = row.cells[0].innerText.trim();
       }
    }
    
    function dblClickTable(row) {
        onInfo(document.getElementById("<%=hidModelName.ClientID%>").value);
    }
    
    function onInfo(m){
        if(''==m){
            m=document.getElementById("<%=hidModelName.ClientID%>").value;
        }
        if(''==m){
            return;
        }
        var ret = window.showModalDialog("FamilyInfoMaintain.aspx?modelname=" + m + "&editor=<%=Master.userInfo.UserId%>", 0, "dialogwidth:1000px; dialogheight:560px; status:no;help:no;");
    }

    function onInfoName(){

        var ret = window.showModalDialog("FamilyInfoNameMaintain.aspx?editor=<%=Master.userInfo.UserId%>", 0, "dialogwidth:1000px; dialogheight:560px; status:no;help:no;");
    
    }
    
    function ShowRowEditInfo(con)
    {
        customerObj = getCustomerCmbObj();
        customerObj.onchange = addNew;

         if(con==null)
         {
            document.getElementById("<%=dFamily.ClientID %>").value =""; 
            document.getElementById("<%=dDescription.ClientID %>").value = "";  
            document.getElementById("<%=dOldFamilyId.ClientID %>").value = "";
            document.getElementById("<%=cmbcustom.ClientID %>").value="";
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;  
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;
            document.getElementById("<%=btnInfo.ClientID%>").disabled = true;
            document.getElementById("<%=hidModelName.ClientID%>").value = "";
            return;    
         }
    
         var curFamilyId= con.cells[0].innerText.trim();
         document.getElementById("<%=dFamily.ClientID %>").value =curFamilyId;
         document.getElementById("<%=dDescription.ClientID %>").value = con.cells[1].innerText.trim(); 
         document.getElementById("<%=cmbcustom.ClientID %>").value = con.cells[2].innerText.trim(); 
         //瀛樻斁鍘熷鐨刦amilyId     
         document.getElementById("<%=dOldFamilyId.ClientID %>").value = curFamilyId;    
         if(curFamilyId=="")
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;
         }
         else
         {
            document.getElementById("<%=btnSave.ClientID %>").disabled=false;
            document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
         }
         document.getElementById("<%=dFamily.ClientID %>").focus();
    }
   
   
    function AddUpdateComplete(id)
    {
        
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        
        var selectedRowIndex=-1;
        for(var i=0;i<gdObj.rows.length;i++)
        {
           if(gdObj.rows[i].cells[0].innerText==id)
           {
               selectedRowIndex=i;  
           }        
        }
        
        if(selectedRowIndex<0)
        {
            document.getElementById("<%=dFamily.ClientID %>").value ="";
            document.getElementById("<%=dDescription.ClientID %>").value = "";
            document.getElementById("<%=cmbcustom.ClientID %>").value="";
            document.getElementById("<%=btnSave.ClientID %>").disabled=true;
            document.getElementById("<%=btnDelete.ClientID %>").disabled=true;
            document.getElementById("<%=btnInfo.ClientID%>").disabled = true;
            document.getElementById("<%=hidModelName.ClientID%>").value = "";
            return;
        }
        else
        {            
            var con=gdObj.rows[selectedRowIndex];
            //去掉标题行
            selectedRowIndex=selectedRowIndex-1;
            setRowSelectedByIndex_<%=gd.ClientID%>(selectedRowIndex, false, "<%=gd.ClientID%>");
            //eval("ChangeCvExtRowByIndex_"+"<%=gd.ClientID%>"+"(RowArray,"+true+", "+selectedRowIndex+")");
            setSrollByIndex(selectedRowIndex, true, "<%=gd.ClientID%>");
            
            
            ShowRowEditInfo(con);
            
            document.getElementById("<%=btnInfo.ClientID%>").disabled = false;
            document.getElementById("<%=hidModelName.ClientID%>").value = con.cells[0].innerText.trim();
        }        
        
    }
    
    function NoMatchFamily()
    {
//         alert("Cant find that match family.");    //5 
         alert(msg5);     
         return;   
    }
   //Add by Benson at 2011/10/13
    function CheckIsDup(inputFamily)
    {
        var gdObj=document.getElementById("<%=gd.ClientID %>");
        var family;
        for(var i=1;i<gdObj.rows.length;i++)
        {
           family=gdObj.rows[i].cells[0].innerText;
           if(family==inputFamily)
           { return true;}
        }
       return false;
    }
    
   //Add by Benson at 2011/10/13
    </script>
</asp:Content>


<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="MasterLabel.aspx.cs" Inherits="DataMaintain_MasterLabel" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div4" class="iMes_div_MainTainDiv1">
            <table width="100%"class="iMes_div_MainTainEdit">
                
                <tr >
                    <td style="width:90px;">
                        <asp:Label ID="lblVc" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:180px;">
                        <asp:UpdatePanel ID="updatePanel6" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                        <ContentTemplate>
                       <input type="text" id="ttVc" runat="server" class="iMes_textbox_input_Yellow" style="width: 80%;" maxlength="6"/>
                       </ContentTemplate>
                       </asp:UpdatePanel> 
                    </td >
                    <td style="width:100px;">
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:280px;">
                        <iMESMaintain:CmbMasterLabelFamilyMaintain ID="cmbFamily1" runat="server" Width="82%" />
                    </td>
                   <td style="width:110px;">
                        
                    </td>
                    <td style="width:300px;">
                        
                    </td>
                    <td align="right">
                       <input type="button" id="btnQuery" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkQuery())" onserverclick="Query_ServerClick"/>
                    </td> 
                </tr> 
            </table>
            
         </div>   
         <div id="div1" class="iMes_div_MainTainDiv1">
               <table width="100%" class="">            
                <tr >
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblMasterLabelList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>                                    
                    <td width="32%" align="right">
                   <input type="button" id="btnDelete" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" disabled="true" onclick="if(clkDelete())" onserverclick="Delete_ServerClick"/>
                    </td>    
                </tr>
             </table>                                     
        </div>
            <div id="div2">
                <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
                    <Triggers>
                     
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                    </Triggers>
                    
                    <ContentTemplate>
                        <iMES:GridViewExt ID="gd" runat="server" HighLightRowPosition="3" AutoGenerateColumns="true" Width="100%" GvExtWidth="100%" GvExtHeight="390px"  onrowdatabound="gd_RowDataBound" OnGvExtRowClick="clickTable(this)" OnDataBound="gd_DataBound" RowStyle-Height="20" AutoHighlightScrollByValue ="true">
                        </iMES:GridViewExt>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="div3">
            <table width="100%" border="0"  class="iMes_div_MainTainEdit"> 
                <tr>
                    <td style="width:90px;">
                        <asp:Label ID="lblVcBottom" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:180px;">
                        <input type="text" id="ttVcBottom" runat="server" class="iMes_textbox_input_Yellow" style="width: 80%;" maxlength="6" />
                    </td>
                    <td style="width:100px;">
                        <asp:Label ID="lblFamilyBottom" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:280px;">
                        <iMESMaintain:CmbMasterLabelFamilyMaintain ID="cmbFamily2" runat="server" Width="82%" />
                    </td>
                    <td style="width:110px;">
                        <asp:Label ID="lblCode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:300px;">
                        <input type="text" id="ttCode" runat="server" class="iMes_textbox_input_Yellow" style="width: 80%;" maxlength="25" />
                    </td>
                    <td align="right">
                       <input type="button" id="btnSave" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="if(clkSave())" onserverclick="Save_ServerClick"/>
                    </td>                                        
                </tr>                               
            </table>
        </div>
    </div>    
    <input type="hidden" id="hidFamily" runat="server" />
    <input type="hidden" id="hidFamily2" runat="server" />
    <input type="hidden" id="hidDeleteID" runat="server" />
    <input type="hidden" id="dTableHeight" runat="server" />
    <input type="hidden" id="HiddenUserName" runat="server" />
    <asp:UpdatePanel ID="updatePanel1" runat="server" RenderMode="Inline" Visible="false" >
    </asp:UpdatePanel>
    <div id="divWait"  oSelectID="" align="center" style="cursor:wait;filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
        <table style="cursor:wait;background-color:#FFFFFF; border: 1px solid #0054B9; font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr><td align="center"><img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif"/></td><td align="center" style="color:#0054B9;font-size:10pt;font-weight: bold;">Please wait.....</td></tr></table></div>
    <script type="text/javascript" language="javascript">
        var customer;
        var editor;
        var emptyPattern = /^\s*$/;
        var mbCodePattern = /^[0-9A-Z]{2}$/;
        var ecrPattern = /^[0-9A-Z]{5}$/;
        var iecVersionPattern = /^[0-9]\.[0-9]{2}$/;
        var custVersionPattern = /^[0-9A-Z]{3}$/;
        
        var vcPattern=/^[0-9A-Z]{1,}$/;
        var codePattern=/^[0-9A-Z]+$/;
        
        var emptyString = "";
        var selectedRowIndex = -1;
        var msgConfirmDelete;
  //      var msgConfirmDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgConfirmDelete").ToString() %>';
        var msgConfirmDelete="";
        var noEmptyVc="";
        var noEmptyCode="";
        var msgInputFamily;
        
        var vcFormat;
        var codeFormat;
        window.onload = function()
        {
            msgConfirmDelete="<%=pmtMessage1 %>";
            noEmptyVc="<%=pmtMessage2 %>";
            noEmptyCode="<%=pmtMessage3 %>";
            msgInputFamily="<%=pmtMessage4 %>";
            vcFormat="<%=pmtMessage5 %>";
            codeFormat="<%=pmtMessage6 %>";
            document.getElementById("<%=HiddenUserName.ClientID %>").value="<%=UserId%>";
            resetTableHeight();
           
        };
         
        function resetTableHeight()
        {
        //动态调整表格的高度
        var adjustValue=60;     
        var marginValue=10;  
        var tableHeigth=300;

        
        try{
            tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div4.offsetHeight-div3.offsetHeight-adjustValue;
        }
        catch(e){
            //ignore
        }                
        //为了使表格下面有写空隙
        var extDivHeight=tableHeigth+marginValue;
       
        document.getElementById("div_<%=gd.ClientID %>").style.height=tableHeigth+"px";
        //alert(document.getElementById("div_<%=gd.ClientID %>").style.height)
        div2.style.height=extDivHeight+"px";
        document.getElementById("<%=dTableHeight.ClientID %>").value=tableHeigth+"px";
        }
        
        function clickTable(con)
        {
        
            if((selectedRowIndex!=null) && (selectedRowIndex!=parseInt(con.index, 10)))
            {
                setRowSelectedOrNotSelectedByIndex(selectedRowIndex,false, "<%=gd.ClientID %>");                
            }
            
            setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
            selectedRowIndex=parseInt(con.index, 10);
            
            setDetailInfo();      
            
            if (hasEditData())
            {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
            }     
            else
            {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            } 
        }
        
        function AddUpdateComplete(vc,family)
        {
        
            var gdObj=document.getElementById("<%=gd.ClientID %>");
          
            selectedRowIndex=-1;
            for(var i=1;i<gdObj.rows.length;i++)
            {
               if(gdObj.rows[i].cells[1].innerText.toUpperCase()==vc.toUpperCase()&&gdObj.rows[i].cells[2].innerText.toUpperCase()==family.toUpperCase())
               {
                    selectedRowIndex=i;  
               }        
            }
            
            if(selectedRowIndex<0)
            {
               document.getElementById("<%=ttVcBottom.ClientID %>").value=""; 
               var familyValue = eval("<%=cmbFamily2.ClientID %>getFamilyCmbObj()");
               familyValue.value="";
               document.getElementById("<%=ttCode.ClientID %>").value="";
               
               document.getElementById("<%=btnSave.ClientID %>").disabled=false;   
               document.getElementById("<%=btnDelete.ClientID %>").disabled=true;          
               return;
            }
            else
            {            
                var con=gdObj.rows[selectedRowIndex];
                
//                setGdHighLight(con);
//                iSelectedRowIndex=parseInt(con.index, 10); 
//                setSrollByIndex(iSelectedRowIndex-1, true, "<%=gd.ClientID%>");
                //issue code
                //ITC-1361-0088 itc210012  2012-2-16
                //ITC-1361-0087 itc210012  2012-2-16
               
                setGdHighLight(con);
               
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                
                var bottomVc=con.cells[1].innerText.trim();
                document.getElementById("<%=ttVcBottom.ClientID %>").value=bottomVc; 
                var familyValue = eval("<%=cmbFamily2.ClientID %>getFamilyCmbObj()");
                familyValue.value=con.cells[2].innerText.trim();
                document.getElementById("<%=ttCode.ClientID %>").value=con.cells[3].innerText.trim();
               if(bottomVc=="")
               {
                     document.getElementById("<%=btnSave.ClientID %>").disabled=false;   
                     document.getElementById("<%=btnDelete.ClientID %>").disabled=true; 
               }
               else
               {
                    document.getElementById("<%=btnSave.ClientID %>").disabled=false;   
                    document.getElementById("<%=btnDelete.ClientID %>").disabled=false;
               }
            }        
        }
        var iSelectedRowIndex=null; 
      
       
        function setGdHighLight(con)
        {
            if((iSelectedRowIndex!=null) && (iSelectedRowIndex!=parseInt(con.index, 10)))
            {
                //去掉过去高亮行
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex,false, "<%=gd.ClientID %>");                
            }     
            //设置当前高亮行   
            setRowSelectedOrNotSelectedByIndex(con.index,true, "<%=gd.ClientID %>");
            //记住当前高亮行
            iSelectedRowIndex=parseInt(con.index, 10);
            selectedRowIndex=parseInt(con.index, 10);
        }
        
        function hasEditData()
        {
            var tblObj = document.getElementById("<%=gd.ClientID %>");
        
            if (selectedRowIndex == -1 || emptyPattern.test(tblObj.rows[selectedRowIndex + 1].cells[0].innerText))
            {
                return false;
            }
            
            return true;
        }        
        
        function clkQuery()
        {
            var familyValue = eval("<%=cmbFamily1.ClientID %>getFamilyCmbObj().value;");
            selectedRowIndex = -1;
            document.getElementById("<%=hidFamily.ClientID %>").value = familyValue; 
            clearDetailInfo();
            
            ShowWait();
            return true;
        }
        
        function setDetailInfo()
        {
            var tblObj = document.getElementById("<%=gd.ClientID %>");
            var row = tblObj.rows[selectedRowIndex + 1];
            //ITC-1361-0120 
            if(row.cells[1].innerText.trim()==""){
                eval("<%=cmbFamily2.ClientID %>getFamilyCmbObj().value='" + row.cells[2].innerText + "'");
                document.getElementById("<%=ttVcBottom.ClientID %>").value = "";
                document.getElementById("<%=ttCode.ClientID %>").value = "";
            }
            else{
                eval("<%=cmbFamily2.ClientID %>getFamilyCmbObj().value='" + row.cells[2].innerText + "'");
                document.getElementById("<%=ttVcBottom.ClientID %>").value = row.cells[1].innerText;
                document.getElementById("<%=ttCode.ClientID %>").value = row.cells[3].innerText;
            }
            
           
        }
        
        function clearDetailInfo()
        {
            eval("<%=cmbFamily2.ClientID %>getFamilyCmbObj().value=''");
            document.getElementById("<%=ttVcBottom.ClientID %>").value = emptyString;
            document.getElementById("<%=ttCode.ClientID %>").value = emptyString;
            
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        }
        
        function clkSave()
        {
            if (checkInput())
            {
                var familyValue = eval("<%=cmbFamily2.ClientID %>getFamilyCmbObj().value;");
                document.getElementById("<%=hidFamily2.ClientID %>").value = familyValue;
                
                if (hasEditData())
                {
                    var tblObj = document.getElementById("<%=gd.ClientID %>");
                    var row = tblObj.rows[selectedRowIndex + 1];
             
                    document.getElementById("<%=hidDeleteID.ClientID %>").value = row.cells[0].innerText;
                }
                else
                {
                    document.getElementById("<%=hidDeleteID.ClientID %>").value = emptyString;
                }
                
                selectedRowIndex = -1;
     //           beginWaitingCoverDiv();
                ShowWait();
                return true;
            }
            
            return false;           
        }
        
        function checkInput()
        {
            var familyValue = eval("<%=cmbFamily2.ClientID %>getFamilyCmbObj().value;");
            var vc=document.getElementById("<%=ttVcBottom.ClientID %>");
            var code=document.getElementById("<%=ttCode.ClientID %>");
            if (emptyPattern.test(familyValue))
            {
                alert(msgInputFamily);
                eval("<%=cmbFamily2.ClientID %>getFamilyCmbObj().focus();");
                return false;
            }
            if(emptyPattern.test(vc.value))
            {
                alert(noEmptyVc);
                vc.focus();
                return false;
            }
            else
            {
                
                if(!vcPattern.test(vc.value.toUpperCase()))
                {
                    alert(vcFormat);
                    vc.focus();
                    return false;
                }
            }   
            if(emptyPattern.test(code.value))
            {
                alert(noEmptyCode);
                code.focus();
                return false;
            }
            else
            {
                if(!codePattern.test(code.value.toUpperCase()))
                {
                    alert(codeFormat);
                    code.focus();
                    return false;
                }   
            }
            return true;      
        }
        
        function clkDelete()
        {
            if (confirm(msgConfirmDelete))
            {
                var tblObj = document.getElementById("<%=gd.ClientID %>");
                var row = tblObj.rows[selectedRowIndex + 1];
             
                document.getElementById("<%=hidDeleteID.ClientID %>").value = row.cells[0].innerText;
                selectedRowIndex = -1;
                clearDetailInfo();
       //         beginWaitingCoverDiv();
                ShowWait();
                return true;
            }
            
            return false;
        }
    </script>
</asp:Content>


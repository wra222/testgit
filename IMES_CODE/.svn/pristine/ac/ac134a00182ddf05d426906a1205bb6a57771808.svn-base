<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="UploadFile.aspx.cs" Inherits="Query_SA_UploadFile" Title="Untitled Page" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <style type="text/css">
         body {padding: 0px; margin:0px; border:0px;   }
         .Lb { text-align:center;   padding-top:10px; padding-bottom:10px; }
          .Lb1 { text-align:center;  padding-top:10px; padding-bottom:10px;}
          .Lb2 { text-align:right;  padding-top:10px; padding-bottom:10px;}
          .Upload { margin-left:40%;}
          .FU{ float:left; margin-left:20%;}
          .Status{ width:100%; text-align:center; font-family:@新宋体;  color:Green;}
          .FL{float:left; margin-left:35%;}
          .Dl_list{ border-style:none; text-align:center;}
    </style>
      <script type="text/javascript" src="../../js/jquery-1.7.1.js "></script>    
      <script language="javascript" type="text/javascript">
//<!--
          //确认：
          //HiddenFieldClientID为隐藏控件HiddenField1的客户端ID
          function test(HiddenFieldClientID) {
              //确认
              if (confirm("確定要刪除該文件?")) {
                  //确认后给个字符串值"true"
                  document.getElementById(HiddenFieldClientID).value = "true";

              }
              else {
                  //确认后给个字符串值"false"
                  document.getElementById(HiddenFieldClientID).value = "false";
              }
        
  var   WshShell   =new   ActiveXObject("WScript.Shell");  
  alert(WshShell.ExpandEnvironmentStrings("%COMPUTERNAME%"));  
  //这一句是用来得到用户的计算机名称  
  alert(WshShell.ExpandEnvironmentStrings("%USERNAME%"));  
  //这一句是用来得到用户名  

              return true;

          }
//-->
</script>
<script language="javascript" type="text/javascript">
   
</script>
<asp:ScriptManager ID="ScriptManager1" runat="server" >        
  </asp:ScriptManager>
<asp:Panel ID="Panel1" runat="server" meta:resourcekey="Panel1Resource1"  >
<iMESQuery:CmbDBType ID="CmbDBType" runat="server" Visible=false   />
    <div>
    <div style="width:100%;height:3px; background-color:Blue;"></div>
    <div style=" margin-top:20px; float:left; width:100%; background-color:GrayText;height:40px;  " >
    <label style=" padding-left:45%; font-family:Book Antiqua; color:Blue; margin-top:20px; " id="lbTitle" runat="server">文件上傳與下載</label>
    </div>
    <div   style=" width:50%; float:left; height:300px; border-color: Black; border-width:medium;  ">
    <br />
        
    <br />
    <br />
    <br />
    <asp:Label ID="lblFileTitle" runat="server" Width="100%" CssClass="Lb1" 
            meta:resourcekey="Label1Resource1" >選擇資料存放目錄</asp:Label>
    <asp:Label ID="lblFile" runat="server" Width="45%"  CssClass="Lb2" 
            meta:resourcekey="Label2Resource1">文件目錄：</asp:Label>
    <asp:DropDownList  ID="DL_F"  runat="server" Width="20%" AutoPostBack="True" 
     
     style="margin-bottom: 0px" 
            meta:resourcekey="DL_FResource1" 
            onselectedindexchanged="DL_F_SelectedIndexChanged" 
            ontextchanged="DL_F_TextChanged" ></asp:DropDownList>
       <%-- <asp:TextBox ID="Username" runat="server"></asp:TextBox>--%>
    </div>
  
    <div style=" width:47%; float:right; margin-right:2%" >
    <asp:Label  runat="server" Height="20px"  CssClass="Lb" Width="100%" ID="lblfileQuery" 
            meta:resourcekey="LB_FileResource1" >資料檢索</asp:Label> 
                    <asp:ListBox ID="ListBox1" runat="server" Width="100%" 
            Height="350px"  CssClass="Dl_list"
                    onselectedindexchanged="ListBox1_SelectedIndexChanged" 
            meta:resourcekey="ListBox1Resource1"></asp:ListBox>
                   <asp:Button ID="btquery" runat="server" onclick="BT_Query_Click" 
            Text="检索" CssClass=FL meta:resourcekey="BT_QueryResource1" />
                   <br />
                   <asp:Button ID="btdel" runat="server"   Text="删除" 
            onclick="Del_Click" meta:resourcekey="DelResource1"  />
                <asp:LinkButton ID="lbdownload" runat="server" 
            onclick="LinkButton1_Click" meta:resourcekey="LinkButton1Resource1" >點擊下載</asp:LinkButton>

    </div>
      <div Cssclass="Upload" style=" width:50%; float:left;">
    <h4 style="text-align:center;">Select a file to upload:</h4>
        <asp:FileUpload id="FileUpload2"                 
            runat="server" CssClass=FU meta:resourcekey="FileUpload2Resource1" />
 
        <asp:Button id="UploadBtn" 
            Text="Upload file"
            OnClick="UploadBtn_Click"
            runat="server" CssClass=Fl2 meta:resourcekey="UploadBtnResource1">
        </asp:Button>    
        <br />
        <br />
        <asp:Label id="UploadStatusLabel"
            runat="server" CssClass="Status" Width=100% 
              meta:resourcekey="UploadStatusLabelResource1"></asp:Label>     
    </div>
       <asp:HiddenField ID="HiddenField1" runat="server" />
    </div>
                       <iMES:GridViewExt ID="gvQuery" runat="server" 
        AutoGenerateColumns="true" GvExtHeight="250px" GvExtWidth="98%" 
        style="top: 0px; left: 0px; height: 280px; width: 99%">
                </iMES:GridViewExt>
        </asp:Panel>
                                
</asp:Content>


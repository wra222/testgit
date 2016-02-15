<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="DownloadBat.aspx.cs" Inherits="CommonFunction_DownloadBat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 95%; margin: 0 auto;">
        <table>
            <tr style="height: 80px;">
                <td align="center">
                    <asp:UpdatePanel ID="updatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:LinkButton ID="BtnDownloadBat" runat="server" OnClientClick="return DownLoadAllBat();"></asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr style="height: 80px;">
                <td align="center">
                    <asp:Label ID="LabelDownLoadAlert" runat="server" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        var ClientBatFilePath = '<%=System.Configuration.ConfigurationManager.AppSettings["ClientBatFilePath"]%>';
        var FtpServerIP = '<%=System.Configuration.ConfigurationManager.AppSettings["FtpServerIP"]%>';
        
        function DownLoadAllBat() {

            var fileObj = new ActiveXObject("Scripting.FileSystemObject");
            if (fileObj.FolderExists(ClientBatFilePath)) {
                fileObj.DeleteFolder(ClientBatFilePath);
            }
            createDir(ClientBatFilePath);

            var batFile = fileObj.CreateTextFile(ClientBatFilePath + "\\DownloadAllBat.bat", true);

            batFile.WriteLine(getHomeDisk(ClientBatFilePath));
            batFile.WriteLine("cd " + ClientBatFilePath);
            batFile.WriteLine("FTP -A -i -s:" + ClientBatFilePath + "\\DownloadAllBat.txt");
            batFile.Close()

            var txtFile = fileObj.CreateTextFile(ClientBatFilePath + "\\DownloadAllBat.txt", true);
            txtFile.WriteLine("open " + FtpServerIP);
            txtFile.WriteLine("mget *.*");
            txtFile.WriteLine("disconnect");
            txtFile.WriteLine("quit");
            txtFile.Close()

            wsh = new ActiveXObject("wscript.shell");
            wsh.run("cmd /k " + getHomeDisk(ClientBatFilePath) + "&cd " + ClientBatFilePath + " &DownloadAllBat.bat&exit", 2, false); //ITC-1360-1553

            return false;

        }
    </script>

</asp:Content>

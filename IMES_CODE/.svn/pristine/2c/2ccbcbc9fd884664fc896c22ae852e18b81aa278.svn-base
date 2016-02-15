using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;
using IMES.Infrastructure;
using System.Text;

public partial class Query_SA_UploadFile : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IUploadFile iUpload = ServiceAgent.getInstance().GetObjectByName<IUploadFile>(WebConstant.IUploadFile);
    ////string  DBConnection = CmbDBType.ddlGetConnection();
    string DBConnection = "";
    string Location ="";//虚拟目录：Files
    string Location_1 = "";//虚拟目录：Files1

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        DataTable FilePath = iUpload.GetFilePath(DBConnection, "UploadPath");
        Location = @FilePath.Rows[0]["Value"].ToString();
        if (!Page.IsPostBack)
        {
            //Username.Text = System.Environment.UserName;
            //Username.Text = User.Identity.Name;
            InitPage();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            string serverpath = Server.MapPath("~/");
            DirectoryInfo dir_1 = new DirectoryInfo(Location);
            DataTable Dir = new DataTable();
            Dir.Columns.Add(new DataColumn("Name", typeof(string)));
            foreach (DirectoryInfo filename in dir_1.GetDirectories())
            {
                DataRow dr = Dir.NewRow();
                dr[0] = filename;
                Dir.Rows.Add(dr);
            }
          
            DL_F.DataSource = Dir;
            DL_F.DataTextField = "Name";
            DL_F.DataValueField = "Name";
            DL_F.DataBind();

            Location_1 = Location + DL_F.SelectedValue + "/";
            DirectoryInfo dir = new DirectoryInfo(Location_1);

            foreach (FileInfo fileName in dir.GetFiles())
            {
                DataRow dr = dt.NewRow();
                dr[0] = fileName;
                dt.Rows.Add(dr);
            }
            ListBox1.DataSource = dt;
            ListBox1.DataTextField = "Name";
            ListBox1.DataValueField = "Name";
            ListBox1.DataBind();

            this.btdel.Attributes.Add("onclick", "return test('" + this.HiddenField1.ClientID + "');");
            #region
            /*             DataTable dt = new DataTable();
                        dt.Columns.Add(new DataColumn("Name", typeof(string)));
                        string serverpath = Server.MapPath("~/");
                        DirectoryInfo dir = new DirectoryInfo(Location_1);
            
                        foreach (FileInfo fileName in dir.GetFiles())
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = fileName;
                            dt.Rows.Add(dr);
                        }
                        ListBox1.DataSource = dt;
                        ListBox1.DataTextField = "Name";
                        ListBox1.DataValueField = "Name";
                        ListBox1.DataBind();

                        this.Del.Attributes.Add("onclick", "return test('" + this.HiddenField1.ClientID + "');");
                        DataTable Dir= new DataTable();
                        Dir.Columns.Add(new DataColumn("Name", typeof(string)));
                        foreach (DirectoryInfo filename in dir.GetDirectories())
                        {
                            DataRow dr = Dir.NewRow();
                            dr[0] = filename;
                            Dir.Rows.Add(dr);
                        }
                        DL_F.DataSource = Dir;
                        DL_F.DataTextField = "Name";
                        DL_F.DataValueField = "Name";
                        DL_F.DataBind(); */
            #endregion
        }
    }
    protected void BT_Query_Click(object sender, EventArgs e)
    {
        Location_1 = Location + DL_F.SelectedValue + "/";
        if (Directory.Exists(Location_1) == false)
        {
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tixin", "alert('该文件不存在或路径错误！')", true);
        }
        else
        {
            DirectoryInfo di = new DirectoryInfo(Location_1);
            FileSystemInfo[] dis = di.GetFileSystemInfos();//.ToString() + Convert.ToString(di.CreationTime);
            if (dis.Length < 1)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tixin", "alert('该文件夹是空文件夹！')", true);
            }
            else
            {
                ListBox1.DataSource = dis;
                ListBox1.DataBind();
                if (Session["wenjian"] != null)
                {

                    Session.Remove("wenjian");
                }
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tixin", "alert('检索成功，列表为该路径的文件和目录！')", true);
            }
        }
    }

    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["wenjian"] = ListBox1.SelectedValue.ToString();
    }
    private void InitPage()
    {
        this.lbTitle.InnerText = this.GetLocalResourceObject(Pre + "_lbTitle").ToString();
        this.lblFileTitle.Text = this.GetLocalResourceObject(Pre + "_lblFileTitle").ToString();
        this.lbdownload.Text = this.GetLocalResourceObject(Pre + "_lbdownload").ToString();
        this.lblFile.Text = this.GetLocalResourceObject(Pre + "_lblFile").ToString();
        this.lblfileQuery.Text = this.GetLocalResourceObject(Pre + "_lblfileQuery").ToString();
        this.btdel.Text = this.GetLocalResourceObject(Pre + "_btdel").ToString();
        this.btquery.Text = this.GetLocalResourceObject(Pre + "_btquery").ToString();

    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        if (Session["wenjian"] != null)
        {
            if (Session["wenjian"].ToString() != "" && Session["wenjian"] != null)
            {
                //string path = Server.MapPath("~/") + Session["wenjian"].ToString();//
                Location_1 = Location + DL_F.SelectedValue + "/";
                string path = Location_1 + Session["wenjian"].ToString();//
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(fi.Name));//
                    Response.AddHeader("Content-Length", fi.Length.ToString());
                    Response.ContentType = "appilcation/octet-stream;charset=gb2312";
                    Response.Filter.Close();
                    Response.WriteFile(fi.FullName);
                    Response.End();
                }
                else
                {
                    Response.Write("<script language=javascript> alert('Test'); </script>");
                }
            }
        }
        else
        {
            Response.Write("<script language=javascript> alert('  You May Select One files!'); </script>");
        }
    }
    protected void UploadBtn_Click(object sender, EventArgs e)
    {
        // Specify the path on the server to
        // save the uploaded file to.
        Location_1 = Location + DL_F.SelectedValue + "/";
        string savePath = Location_1;

        // Before attempting to save the file, verify
        // that the FileUpload control contains a file.
        if (FileUpload2.HasFile)
        {
            // Get the name of the file to upload.
            string fileName = Server.HtmlEncode(FileUpload2.FileName);

            // Get the extension of the uploaded file.
            string extension = System.IO.Path.GetExtension(fileName);

            // Allow only files with .doc or .xls extensions
            // to be uploaded.
            //if ((extension == ".doc") || (extension == ".xls") || (extension == ".txt") || (extension == ".exe") || (extension == ".msi"))
             if( checkex(extension))
            {
                // Append the name of the file to upload to the path.
                savePath += fileName;

                // Call the SaveAs method to save the 
                // uploaded file to the specified path.
                // This example does not perform all
                // the necessary error checking.               
                // If a file with the same name
                // already exists in the specified path,  
                // the uploaded file overwrites it.
                FileUpload2.SaveAs(savePath);

                // Notify the user that their file was successfully uploaded.
                UploadStatusLabel.Text = "Your file was uploaded successfully.";
                BT_Query_Click(sender, e);
            }
            else
            {
                // Notify the user why their file was not uploaded.
                UploadStatusLabel.Text = "Your file was not uploaded because " +
                                         "it does not have a .doc or .xls extension.";
            }

        }

    }

    protected bool checkex(string extension)
    {
        bool result = false;
             DataTable dt=  iUpload.GetFilePath(DBConnection, "Allow_ex");
             String[] Temp = dt.Rows[0]["Value"].ToString().Split(',');
             for (int i = 0; i < Temp.Length; i++)
             {
                 if (Temp[i].Contains(extension))
                 {
                     result = true;
                     break;
                 }
             }
                 return result;
    }
    protected void Del_Click(object sender, EventArgs e)
    {
        //if(Del.Attributes.Add("onclick", " Return confirm('確定要刪除該文件?')")) 
        //得到确实的值
        Location_1 = Location + DL_F.SelectedValue + "/";
        string temp = this.HiddenField1.Value;
        if (temp == "true")
        {
            if (Session["wenjian"] != null)
            {
                if (Session["wenjian"].ToString() != "" && Session["wenjian"] != null)
                {
                    //string path = Server.MapPath("~/") + Session["wenjian"].ToString();//
                    string path = Location_1 + Session["wenjian"].ToString();//
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists)
                    {
                        File.Delete(path);
                    }
                    else
                    {
                        Response.Write("<script language=javascript> alert('您選擇的文件不存在'); </script>");
                    }
                }
            }
            else
            {
                Response.Write("<script language=javascript> alert('  You May Select One files!'); </script>");
            }
            BT_Query_Click(sender, e);
        }
        else
        {
            BT_Query_Click(sender, e);
        }

    }
    protected void DL_F_SelectedIndexChanged(object sender, EventArgs e)
    {
        BT_Query_Click(sender, e);
    }
    protected void DL_F_TextChanged(object sender, EventArgs e)
    {
        //Response.Write("<script language=javascript> alert('  You May Select One files!'); </script>");
        BT_Query_Click(sender, e);
    }


 
}

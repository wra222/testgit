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

public partial class CommonAspx_DownloadExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
         

         string fileID = Request["fileID"].ToString();
         string fileName = HttpUtility.UrlPathEncode(Request["fileName"].ToString());
        
        string filePath=MapPath(@"~\TmpExcel\" +fileID +".xls");
                Response.AddHeader("content-disposition",
                    "attachment;filename=" + fileName+".xls");
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(filePath);
          
            Response.End();
       
    }
}

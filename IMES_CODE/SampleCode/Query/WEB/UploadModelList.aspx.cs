using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PAK_UploadModelList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
     //   string m = Request["ModelList"].ToString();
        hidModelList.Value = Request["ModelList"].ToString();
     //  hidModelList.Value = m;
     //   m=m.Replace("ABC123456", "\r\n");
       // string[] modelArr = m.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

       // TextBox1.Text = m;
    }
}

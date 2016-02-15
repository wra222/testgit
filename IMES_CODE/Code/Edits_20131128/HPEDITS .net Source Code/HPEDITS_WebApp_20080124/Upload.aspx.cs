using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Upload : System.Web.UI.Page
{
    //const string UploadPath = @"E:\Project\HP_EDITS\UploadFile\";
    const string UploadPath = @"D:\Edits_test";
	protected void Page_Load(object sender, EventArgs e)
	{
		this.fuComn.ConnectionString	= ConfigurationManager.AppSettings["Database"];
		this.fuComn.DataDictionaryPath	= Server.MapPath(String.Format(@"~\{0}", ConfigurationManager.AppSettings["DataDictionary"]));
        this.fuComn.SavePath = Server.MapPath(String.Format(@"~\{0}", ConfigurationManager.AppSettings["SAPFlatFileSavePath"]));

		this.fuPaltno.ConnectionString		= ConfigurationManager.AppSettings["Database"];
        this.fuPaltno.DataDictionaryPath = Server.MapPath(String.Format(@"~\{0}", ConfigurationManager.AppSettings["DataDictionary"]));
        this.fuPaltno.SavePath = Server.MapPath(String.Format(@"~\{0}", ConfigurationManager.AppSettings["SAPFlatFileSavePath"]));

		this.fuEdi850raw.ConnectionString	= ConfigurationManager.AppSettings["Database"];
        this.fuEdi850raw.DataDictionaryPath = Server.MapPath(String.Format(@"~\{0}", ConfigurationManager.AppSettings["DataDictionary"]));
        this.fuEdi850raw.SavePath = Server.MapPath(String.Format(@"~\{0}", ConfigurationManager.AppSettings["SAPFlatFileSavePath"]));

        this.fuEdi850rawINSTR.ConnectionString = ConfigurationManager.AppSettings["Database"];
        this.fuEdi850rawINSTR.DataDictionaryPath = Server.MapPath(String.Format(@"~\{0}", ConfigurationManager.AppSettings["DataDictionary"]));
        this.fuEdi850rawINSTR.SavePath = Server.MapPath(String.Format(@"~\{0}", ConfigurationManager.AppSettings["SAPFlatFileSavePath"]));
	}
}

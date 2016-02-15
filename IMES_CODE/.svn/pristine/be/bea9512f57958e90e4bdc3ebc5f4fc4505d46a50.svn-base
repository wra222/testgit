using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
       // String token = HttpContext.Current.Request.QueryString["Token"];
        String token = "AAEAAAD/////AQAAAAAAAAAMAgAAAF5jb20uaW52ZW50ZWMuUkJQQy5OZXQuZGF0YW1vZGVsLlNlc3Npb24sIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsBQEAAAAtY29tLmludmVudGVjLlJCUEMuTmV0LmRhdGFtb2RlbC5TZXNzaW9uLlRva2VuAgAAAAF0CElUb2tlbit0AwMLU3lzdGVtLkd1aWQLU3lzdGVtLkd1aWQCAAAABP3///8LU3lzdGVtLkd1aWQLAAAAAl9hAl9iAl9jAl9kAl9lAl9mAl9nAl9oAl9pAl9qAl9rAAAAAAAAAAAAAAAIBwcCAgICAgICAtjrKszrwOZJsJWTbAW7B8QB/P////3////Y6yrM68DmSbCVk2wFuwfECw==";

        token = token.Replace("+", "%2B");
        Response.Redirect("~/webroot/aspx/dashboard/dashboardmain.aspx?Token=" + token);
        //String UserName = HttpContext.Current.Request.QueryString["UserName"];
        //UserName = UserName.Replace("+", "%2B");
        //Response.Redirect("~/webroot/aspx/dashboard/dashboardmain.aspx?UserName=" + UserName);
    }
}

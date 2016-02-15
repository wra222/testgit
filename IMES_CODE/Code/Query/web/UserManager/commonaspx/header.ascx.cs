/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: header.ascx.cs
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2008-11-17   itc204011     Create 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class webroot_commonaspx_header : System.Web.UI.UserControl
{
	private static readonly log4net.ILog log = log4net.LogManager.GetLogger("header.ascx");

    protected void Page_Load(object sender, EventArgs e)
    {
		//string strContextPath = Request.ApplicationPath;
		//log.Debug("strContextPath=" + strContextPath);
    }
}

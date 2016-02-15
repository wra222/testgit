using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using log4net;

/// <summary>
/// Summary description for AuthenticateModule
/// </summary>
/// 
namespace com.inventec.system.module
{
    public class AuthenticateModule : IHttpModule
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AuthenticateModule));
        public AuthenticateModule()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public String ModuleName
        {
            get { return "AuthenticateModule"; }
        }

        // In the Init function, register for HttpApplication 
        // events by adding your handlers.
        public void Init(HttpApplication application)
        {
            application.AuthenticateRequest +=
                (new EventHandler(this.Application_AuthenticateRequest));
             
        }

        private void Application_AuthenticateRequest(Object source,
             EventArgs e)
        {
             //Create HttpApplication and HttpContext objects to access
            // //request and response properties.
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;

            //if (context.Session == null)
            //    log.Debug("**context.Session is null:");
            //else
            //    log.Debug("++context.Session is not null:");

            if (context.Request.IsAuthenticated)
                log.Debug("++IsAuthenticated= true");
            else
                log.Debug("++IsAuthenticated= false");

            ////log.Debug("**context.Session[AttributeNames.USER_ID]:" + context.Session[AttributeNames.USER_ID]);

            //if (context.Session == null || context.Session[AttributeNames.USER_ID] == null)
            //{
            //    String requestURL = context.Request.CurrentExecutionFilePath;

            //    //only filter aspx extension
            //    if (requestURL.IndexOf(".aspx", 0) == -1)
            //        return;

            //    if (context.Request.CurrentExecutionFilePath.IndexOf("/webroot/aspx/logon/login.aspx", 0) == -1)
            //    {
            //        log.Debug("*redirected from:" + requestURL + "********************************");
            //        context.Response.BufferOutput = true;
            //        context.Response.Redirect("~/webroot/aspx/logon/login.aspx");
            //        //context.Server.Transfer("~/webroot/aspx/logon/login.aspx", true); 
            //        //context.RewritePath("~/webroot/aspx/logon/login.aspx");
            //    }
            //} 
             
        }
         

        public void Dispose()
        {
        }

    }
}
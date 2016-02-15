<%@ Application Language="C#" %>
<script runat="server">
    
   // ImportUserScheduler importUsreSch = new ImportUserScheduler();
    log4net.ILog log = log4net.LogManager.GetLogger("Global.asax");
    
    void Application_Start(object sender, EventArgs e) 
    {
        //在应用程序启动时运行的代码
		log4net.Config.DOMConfigurator.Configure();

  //      importUsreSch.start();


    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //在应用程序关闭时运行的代码
     //  在应用程序关闭时运行的代码
   //     importUsreSch.end();

    }
    
    void Application_Error(object sender, EventArgs e) 
    { 
        //在出现未处理的错误时运行的代码
        //Session["CurrentError"] = "Global: " +
        //Server.GetLastError().Message;
        //Server.Transfer("errorpage.aspx");


    }

    void Session_Start(object sender, EventArgs e) 
    {
        /*
        //在新会话启动时运行的代码
        log.Debug("Sesssion start....");
        String internalLoginUrl = "/webroot/aspx/logon/login.aspx".ToLower();
        String externalLoginUrl = "/webroot/aspx/maindisplay/displayLogin.aspx".ToLower();
        String preExtLoginUrl = "/display.aspx".ToLower();
        String preIntLoginUrl = "/template.aspx".ToLower();
        String visitingUrl = HttpContext.Current.Request.CurrentExecutionFilePath.ToLower();

        //ajax method has not session timeout.
        if (visitingUrl.IndexOf(".ashx", visitingUrl.Length - 5) != -1)
        {
            log.Debug("Ajax time out.");
            return;
        }
        
        log.Debug("HttpContext.Current.Request.CurrentExecutionFilePath=" + HttpContext.Current.Request.CurrentExecutionFilePath);
        if (HttpContext.Current.Session[com.inventec.system.AttributeNames.USER_CODE] == null)
        {
            if (visitingUrl.IndexOf("/webroot/aspx/logon/importuser.aspx", 0) == -1)
            {
                if (visitingUrl.IndexOf(internalLoginUrl, 0) == -1 &&
                    visitingUrl.IndexOf(externalLoginUrl, 0) == -1 &&
                    visitingUrl.IndexOf(preExtLoginUrl, 0) == -1 &&
                    visitingUrl.IndexOf(preIntLoginUrl, 0) == -1)
                {
                    log.Debug("Sesssion timeout!!!");
                    Response.BufferOutput = true;

                    HttpCookie MyCookie = Request.Cookies[com.inventec.system.Constants.LOGIN_ENTRY];

                    if (MyCookie != null && MyCookie.Value != null)
                    {
                        if (MyCookie.Value.ToString().Equals("Internal"))
                            Response.Redirect("~" + internalLoginUrl);
                        else
                            Response.Redirect("~" + externalLoginUrl);
                    } 
                    else
                        Response.Redirect("~" + externalLoginUrl);
                    //Response.Redirect("~" + internalLoginUrl);//for internal test
                }
            } 
            
        }  */
    }

    void Session_End(object sender, EventArgs e) 
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。
        
    }
       
</script>

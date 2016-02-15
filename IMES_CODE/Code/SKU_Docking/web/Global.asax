<%@ Application Language="C#" %>

<script runat="server">
    
  
    public override void Init()
    {
        base.Init();
        log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        logger.InfoFormat("BEGIN: {0}()", methodName);
        try
        {
            logger.Debug("Current AppDomainId:" + HttpRuntime.AppDomainId);
            logger.Debug("Current AppDomainAppPath:" + HttpRuntime.AppDomainAppPath);
            logger.Debug("Current AppDomainAppId:" + HttpRuntime.AppDomainAppId);
        }
        catch (Exception ex)
        {
            logger.Error(methodName, ex);
        }
        finally
        {
            logger.InfoFormat("END: {0}()", methodName);
        }      
        ////設置 appDomain Name 成為相同，可以跨application or Site 送Session 資料, 換到SharedSessionModule Init 才做
        //string appName = ConfigurationManager.AppSettings["WebApplicationName"];
        //if (string.IsNullOrEmpty(appName))
        //    return;
        //foreach (string moduleName in this.Modules)
        //{
            
        //    IHttpModule module = this.Modules[moduleName];            
        //    SessionStateModule ssm = module as SessionStateModule;
        //    if (ssm != null)
        //    {
        //        System.Reflection.FieldInfo storeInfo = typeof(SessionStateModule).GetField("_store", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //        SessionStateStoreProviderBase store = (SessionStateStoreProviderBase)storeInfo.GetValue(ssm);
                
        //        if (store == null) //In IIS7 Integrated mode, module.Init() is called later
        //        {                                      
        //            System.Reflection.FieldInfo runtimeInfo = typeof(HttpRuntime).GetField("_theRuntime", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        //            HttpRuntime theRuntime = (HttpRuntime)runtimeInfo.GetValue(null);
        //            System.Reflection.FieldInfo appNameInfo = typeof(HttpRuntime).GetField("_appDomainAppId", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //            appNameInfo.SetValue(theRuntime, appName);
        //        }
        //        else
        //        {
        //            Type storeType = store.GetType();
        //            if (storeType.Name.Equals("OutOfProcSessionStateStore"))
        //            {
        //                System.Reflection.FieldInfo uribaseInfo = storeType.GetField("s_uribase", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        //                uribaseInfo.SetValue(storeType, appName);
        //            }
        //        }
        //    }
        //}
       
    }

    void Application_Start(object sender, EventArgs e) 
    {
      
           // System.IO.FileInfo fileInfo = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("~/") + "fisLog.config");

           // log4net.Config.XmlConfigurator.ConfigureAndWatch(fileInfo);
           string log4netPath = Server.MapPath("~/web.config");
           log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(log4netPath));
        
            log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.Debug("Current AppDomainId:" + HttpRuntime.AppDomainId);
                logger.Debug("Current AppDomainAppPath:" + HttpRuntime.AppDomainAppPath);
                logger.Debug("Current AppDomainAppId:" + HttpRuntime.AppDomainAppId);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            finally
            {
                logger.InfoFormat("END: {0}()", methodName);
            }          

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
        log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        logger.InfoFormat("BEGIN: {0}()", methodName);
        try
        {   
                     
            logger.Debug("Current AppDomainId:" + HttpRuntime.AppDomainId);
            logger.Debug("Current AppDomainAppPath:" + HttpRuntime.AppDomainAppPath);
            logger.Debug("Current AppDomainAppId:" + HttpRuntime.AppDomainAppId);
        }
        catch (Exception ex)
        {
            logger.Error(methodName, ex);
        }
        finally
        {
            logger.InfoFormat("END: {0}()", methodName);
        }          

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
        log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        logger.InfoFormat("BEGIN: {0}()", methodName);
        try
        {

            logger.Debug("Current AppDomainId:" + HttpRuntime.AppDomainId);
            logger.Debug("Current AppDomainAppPath:" + HttpRuntime.AppDomainAppPath);
            logger.Debug("Current AppDomainAppId:" + HttpRuntime.AppDomainAppId);
            Exception ex1 = HttpContext.Current.Server.GetLastError();
            logger.Error(ex1);
            
        }
        catch (Exception ex)
        {
            logger.Error(methodName, ex);
        }
        finally
        {
            logger.InfoFormat("END: {0}()", methodName);
        }          

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using IMES.Query.DB;
using log4net;

[assembly: log4net.Config.XmlConfigurator( Watch = true)]

namespace IMES.Service.MO
{
    class ConfirmMO
    {
        static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {            
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {

                //1.Check Normal confirm MO or Rework confirm MO from args variable
                if (args.Length > 0 && args[0].Trim() == "Rework")
                    Execute.ProcessRewok();
                else if (args.Length > 1 && 
                            !string.IsNullOrEmpty(args[0]) &&
                            !string.IsNullOrEmpty(args[1]))
                {
                    string isSync = (args[0].Trim() == "Y" || args[0].Trim() == "SYNC" ? "X" : "");
                    Execute.Process(isSync, args[1].Trim());
                }
                else
                    Execute.Process("","");
            }
            catch(Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
           
        }
    }
}

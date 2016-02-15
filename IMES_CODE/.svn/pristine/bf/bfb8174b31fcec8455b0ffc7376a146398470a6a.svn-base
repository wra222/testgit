using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using log4net;
using System.Reflection;
using IMES.Query.DB;

namespace IMES.SAP.Implementation
{
    public enum enumMsgType
    {
        Delivery =0,
        Shippment,
        Standard,
        Pallet,
    }

    public class SAPWorker
    {
        //private readonly object[] padlock = new object[3]{new object(), new object() ,new object()};
        private readonly object padlock = new object();
        static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        // This method will be called when the thread is started.
        public void DoWork(object msgType)
        {
             string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            string ServiceName = ConfigurationManager.AppSettings["ServiceName"];
            enumMsgType dataType = (enumMsgType)msgType;
            string interval = ConfigurationManager.AppSettings["TimerInterval"];
           
            try
            {              

                logger.Info("Interval=" + interval + " MsgType=" + dataType.ToString());
                List<SAPWeightDef> DefList = null;

                #region Get Basic definition handle
                while (!_shouldStop )
                {
                    try
                    {
                        DefList = SQL.GetSAPWeightDef(dataType);
                        logger.Info("SQL.GetSAPWeightDef OK!!");
                        break;
                    }
                    catch (Exception e)
                    {
                        DefList = null;
                        BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);                   
                     }

                    if (_shouldStop) break;
                    lock (padlock)
                    {
                        Monitor.Wait(padlock, int.Parse(interval));
                    }
                }
                #endregion

                #region main while loop
                while (!_shouldStop)
                {
                    try
                    {
                        #region process SAP Weight by SAPWeightDef table
                        if (dataType == enumMsgType.Pallet)
                           SAPWeight.ProcessPalletWeight(DefList);
                        else if (dataType == enumMsgType.Standard)
                            SAPWeight.ProcessMasterWeight(DefList);
                        else
                            SAPWeight.ProcessDeliveryWeight(DefList);
                        #endregion
                    }
                    catch (Exception e)
                    {
                        BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                    }
                    //Thread.Sleep(int.Parse(interval));
                    //int indx = (int)dataType;
                    //lock(padlock[indx])
                    //{
                    //    Monitor.Wait(padlock[indx], int.Parse(interval));
                    //}
                   
                    if (_shouldStop) break;

                    lock (padlock)
                    {
                        Monitor.Wait(padlock, int.Parse(interval));
                    }

                }
                #endregion
            }
            catch (Exception e)
            {
                 BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }
        //public void RequestStop(enumMsgType msgType)
        public void RequestStop()
        {
            _shouldStop = true;
            //int indx = (int)msgType;
            //lock (padlock[indx])
            //{
            //    Monitor.Pulse(padlock[indx]);
            //}
            lock (padlock)
            {
                Monitor.PulseAll(padlock);                
            }
        }
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        private volatile bool _shouldStop = false;
    }
}

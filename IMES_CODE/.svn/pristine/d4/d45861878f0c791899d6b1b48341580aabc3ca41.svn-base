/*
* INVENTEC corporation ©2016 all rights reserved. 
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Station.Implementation;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using System.Linq;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.ReprintLog;
using IMES.Infrastructure.Extend;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// ICollectCPU接口的实现类
    /// </summary>
    public class CollectCPU : MarshalByRefObject, ICollectCPU
    {
        private static readonly  ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.MB;
        private const string WFfile = "SMTCollectCPU.xoml";
        private const string Rulesfile = "";
       
        public void CollectSN(string mbSN, string line, string tool, string station,
                                          string editor, string customer, IList<string> cpuSNList, 
                                          string rawData, string txnId)
        {
            logger.Debug("(CollectCPU)CollectSN start, mb SN:" + mbSN
                + " Serial Number:" + string.Join(",", cpuSNList.ToArray()));

            try
            {
                string sessionKey = mbSN;
                Dictionary<string, object> sessionKeyValue = new Dictionary<string, object>();
                sessionKeyValue.Add(ExtendSession.SessionKeys.TxnId, txnId);
                sessionKeyValue.Add(ExtendSession.SessionKeys.TxnRawData, rawData);
                sessionKeyValue.Add(ExtendSession.SessionKeys.CPUSnList, cpuSNList);
                sessionKeyValue.Add(Session.SessionKeys.FixtureID, tool);

                Session currentSession=  WorkflowUtility.InvokeWF(sessionKey, 
                                                                                              station, 
                                                                                              line, 
                                                                                              customer, 
                                                                                              editor, 
                                                                                              SessionType, 
                                                                                              WFfile, 
                                                                                              Rulesfile, 
                                                                                              sessionKeyValue);             
             
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CollectCPU)CollectSN end,  mbSN:" + mbSN);
            }

        }
     

    }
}

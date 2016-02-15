/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{
    public class _Print1397Label : MarshalByRefObject, IPrint1397Label, I1397Level, IFamily
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IPrint1397Label Members
        //private static readonly string station;
        private static readonly Session.SessionType theType = Session.SessionType.Common ;

        public IList<PrintItem> Print1397Label(
          string pdLine,
          string _1397,
          string VGA,
          string FAN,
          int qty,
          string editor,
          string stationId, string customerId, IList<PrintItem> printItems)
        {

            logger.Debug("(Print1397Label)Print start, pdLine:" + pdLine + "_1397:" + _1397 + " VGA:" + VGA + " FAN:" + FAN + " qty:" + qty.ToString() + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            List<PrintItem> printList;
            try
            {
                string sessionKey = Guid.NewGuid().ToString();
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType,editor,stationId,pdLine,customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "008Print1397Label.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    
                    Session.SetInstance(instance);
                                  
                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    Session.AddValue(Session.SessionKeys._1397No, _1397);
                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }

                IList<PrintItem> returnList = this.getPrintList(Session);
                return returnList;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(Print1397Label)Print end, pdLine:" + pdLine + "_1397:" + _1397 + " VGA:" + VGA + " FAN:" + FAN + " qty:" + qty.ToString() + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
        }

        public Get1397InfoResult Get1397Info(string _1397)
        {
            logger.Debug("(Print1397Label)Get1397Info start, _1397:" + _1397);
            try
            {
                Get1397InfoResult returnValue = new Get1397InfoResult();
                returnValue.FAN = new List<string>();
                returnValue.VGA = new List<string>();
                returnValue.CPUAssy = new List<string>();

                IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository, BOM>();
                BOM modelBom = bomRep.GetModelBOM(_1397);
                IList<IBOMPart> allPart = modelBom.GetAllPart();
                foreach (IBOMPart item in allPart)
                {
                    if (item.Type.ToUpper() == "MB")
                    {
                        returnValue._111Level = item.PN;
                    }
                    else if (item.Type.ToUpper() == "CPU")
                    {
                        returnValue.CPUAssy = item.AssemblyCode;

                        //returnValue.CPUAssy = CPUAssy;
                        returnValue.CPUPN = item.PN;
                    }
                    else if (item.Type.ToUpper() == "VB")
                    {
                        returnValue.VGA.Add(item.PN);
                    }
                    else if (item.Type.ToUpper() == "FAN")
                    {
                        returnValue.FAN.Add(item.PN);
                    }
                }
                List<string> CPUAssyForSort = (List<string>)returnValue.CPUAssy;
                CPUAssyForSort.Sort();
                returnValue.CPUAssy = CPUAssyForSort;

                List<string> VGAForSort = (List<string>)returnValue.VGA;
                VGAForSort.Sort();
                returnValue.VGA = VGAForSort;

                List<string> FANForSort = (List<string>)returnValue.FAN ;
                FANForSort.Sort();
                returnValue.FAN = FANForSort;
                return returnValue;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(Print1397Label)Get1397Info end, _1397:" + _1397);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                }
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
                logger.Debug("Cancel end, sessionKey:" + sessionKey);
            }
        }

        #endregion
        private IList<PrintItem> getPrintList(Session session)
        {   
            try
            {
                object printObject = session.GetValue(Session.SessionKeys.PrintItems);
                session.RemoveValue(Session.SessionKeys.PrintItems);
                if (printObject == null)
                {
                    return null;
                }
                IList<PrintItem> printItems = (IList<PrintItem>)printObject;

                return printItems;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
                //throw new  SysException(e);
            }
        }


        #region I1397Level Members

        public IList<_1397LevelInfo> Get1397LevelList(string familyId)
        {
            IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            return modelRepository.Get1397ListFor008(familyId);
        }

        #endregion

        //#region IFamily Members

        //public IList<FamilyInfo> GetFamilyList()
        //{
        //    IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
        //    return modelRepository.GetFamilyListFor008();
        //}

        //#endregion

        #region IFamily Members

        public IList<FamilyInfo> GetFamilyList(string customerId)
        {
            IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            return modelRepository.GetFamilyListFor008();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
//using IMES.Station.Interface.CommonIntf;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.Part;
using System.Collections;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// </summary>
    public partial class ScanningListImpl : MarshalByRefObject, ScanningList, ScanningListForControl
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
      


        #region ScanningList Members
        public void Print(string sessionKey,List<string> printlist)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                logger.Debug("ScanningList Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();

                    if (currentSession.Exception != null)
                    {
                        if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            currentSession.ResumeWorkFlow();
                        }

                        throw currentSession.Exception;
                    }
                    return;
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
                logger.Debug("ScanningList Cancel end, sessionKey:" + sessionKey);
            }
        }

        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("ScanningList Cancel start, sessionKey:" + sessionKey);
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                if (currentSession != null)
                {
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
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
                logger.Debug("ScanningList Cancel end, sessionKey:" + sessionKey);
            }
        }



        public ArrayList ScanningListForCheck(string pdline, string station, string editor, string customer, 
                                string data, string doc_type)
        {
            string currentSessionKey = Guid.NewGuid().ToString();
            ArrayList ret = new ArrayList();
            try
            {
                Session currentCommonSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Common);
                if (currentCommonSession == null)
                {
                    currentCommonSession = new Session(currentSessionKey, Session.SessionType.Common, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentCommonSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ScanningList.xoml", "", wfArguments);
                    currentCommonSession.AddValue(Session.SessionKeys.DeliveryNo, data);
                    currentCommonSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentCommonSession.AddValue(Session.SessionKeys.COASN, doc_type);
                    
                    currentCommonSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentCommonSession))
                    {
                        currentCommonSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentCommonSession.WorkflowInstance.Start();
                    currentCommonSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(currentSessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentCommonSession.Exception != null)
                {
                    if (currentCommonSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentCommonSession.ResumeWorkFlow();
                    }

                    throw currentCommonSession.Exception;
                }
                IList<string> xmlParm = (IList<string>)currentCommonSession.GetValue(Session.SessionKeys.GiftScanPartList);
                IList<string> pdfParm = (IList<string>)currentCommonSession.GetValue(Session.SessionKeys.GiftPartNoList);
                string path = (string)currentCommonSession.GetValue(Session.SessionKeys.CN);
                string DeliveryNo = (string)currentCommonSession.GetValue(Session.SessionKeys.DeliveryNo);
                ret.Add(xmlParm);
                ret.Add(pdfParm);
                ret.Add(path);
                ret.Add(doc_type);
                ret.Add(currentSessionKey);
                ret.Add(DeliveryNo);
                return ret;
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
                logger.Debug("(ScanningList)ScanningListForOBCheck end, DN:" + data + " Doc Type:" + doc_type);
            }            
        }

        public ArrayList ScanningCopyFile()
        {
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            ArrayList ret = new ArrayList();
            string OAEditsURL_string="";
            string OAEditsTemplate_string="";
            IList<string> OAEditsURL = new List<string>();
            OAEditsURL = partRepository.GetValueFromSysSettingByName("OAEditsURL");
            if (OAEditsURL != null && OAEditsURL.Count > 0)
            {
                OAEditsURL_string = OAEditsURL[0];
            }

            IList<string> OAEditsTemplate = new List<string>();
            OAEditsTemplate = partRepository.GetValueFromSysSettingByName("OAEditsTemplate");
            if (OAEditsTemplate != null && OAEditsTemplate.Count > 0)
            {
                OAEditsTemplate_string = OAEditsTemplate[0];
            }

            string OAEditsXML_string = "";
            IList<string> OAEditsXML = new List<string>();
            OAEditsXML = partRepository.GetValueFromSysSettingByName("OAEditsXML");
            if (OAEditsXML != null && OAEditsXML.Count > 0)
            {
                OAEditsXML_string = OAEditsXML[0];
            }

            string OAEditsPDF_string = "";
            IList<string> OAEditsPDF = new List<string>();
            OAEditsPDF = partRepository.GetValueFromSysSettingByName("OAEditsPDF");
            if (OAEditsPDF != null && OAEditsPDF.Count > 0)
            {
                OAEditsPDF_string = OAEditsPDF[0];
            }

            string OAEditsImage_string = "";
            IList<string> OAEditsImage = new List<string>();
            OAEditsImage = partRepository.GetValueFromSysSettingByName("OAEditsImage");
            if (OAEditsImage != null && OAEditsImage.Count > 0)
            {
                OAEditsImage_string = OAEditsImage[0];
            }

            string FOPFullFileName_string = "";
            IList<string> FOPFullFileName = new List<string>();
            FOPFullFileName = partRepository.GetValueFromSysSettingByName("FOPFullFileName");
            if (FOPFullFileName != null && FOPFullFileName.Count > 0)
            {
                FOPFullFileName_string = FOPFullFileName[0];
            }

            string PDFPrintPath_string = "";
            IList<string> PDFPrintPath = new List<string>();
            PDFPrintPath = partRepository.GetValueFromSysSettingByName("PDFPrintPath");
            if (PDFPrintPath != null && PDFPrintPath.Count > 0)
            {
                PDFPrintPath_string = PDFPrintPath[0];
            }

            ret.Add(OAEditsURL_string);
            ret.Add(OAEditsTemplate_string);
            ret.Add(OAEditsXML_string);
            ret.Add(OAEditsPDF_string);
            ret.Add(OAEditsImage_string);
            ret.Add(FOPFullFileName_string);
            ret.Add(PDFPrintPath_string);

            return ret;
        }
        #endregion



        #region ScanningListForControl Members
        public IList<string> GetDocTypeList()
        {
            IList<string> res = new List<string>();
            IPizzaRepository repPizza =
                    RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            res = repPizza.GetDocCatsFromPakPakRT("Waybill");

            return res;
        }

   
        #endregion


    }
}

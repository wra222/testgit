/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service implement for LabelLightGuide Page
 *             
 * UI:CI-MES12-SPEC-FA-UI Label Light Guide.docx –2011/10/26 
 * UC:CI-MES12-SPEC-FA-UC Label Light Guide.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-19  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System;
using System.Workflow.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using IMES.DataModel;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.Repository.Common;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Part;
using System.Linq;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for LabelLightGuide.
    /// </summary>
    public class ESOPandAoiKbTest : MarshalByRefObject, IESOPandAoiKbTest
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

        #region ILabelLightGuide Members
        /// <summary>
        /// 根据ProductID获取product
        /// </summary>
        public ArrayList GetProductInfo(string prodId, string line, string editor, string station, string customer, bool bQuery)
        {
            logger.Debug("(_ESOPandAoiKbTest)GetProductInfo start, productId:" + prodId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = prodId;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ESOPandAoiKbTest.xoml", "ESOPandAoiKbTest.rules", wfArguments);
                    currentSession.AddValue("bOnlyQuery", false);

                    if (!bQuery)
                        currentSession.AddValue(Session.SessionKeys.IsComplete, false);

                    currentSession.SetInstance(instance);

                    //     bool bNeedSave = true;
                    //       if (bQuery) bNeedSave = false;
                    //4/24 UC中删除此段处理逻辑
                    /*
                    else
                    {
                        //若ProductLog中存在Station=64的Log，且最近的一次过64站之后，没有（Station=6A and Status=0）的Log记录，则不进行任何操作。
                        IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                        IList<ProductLog> logList64 = productRepository.GetProductLogsOrderByCdtDesc(prodId, "64");
                        IList<ProductLog> logList6A = productRepository.GetProductLogsOrderByCdtDesc(prodId, "6A", 0);

                        if (logList64.Count > 0)
                        {
                            if (logList6A.Count <= 0) bNeedSave = false;
                            if (logList6A.Count > 0)
                            {
                                if (logList64[0].Cdt > logList6A[0].Cdt)
                                {
                                    bNeedSave = false;
                                }
                            }
                        }
                    }
                    */

                    currentSession.AddValue("bOnlyQuery", bQuery);
					if (bQuery)
					{
                        currentSession.AddValue("IsAOILine", "N");
					}
                    //       currentSession.AddValue("bNeedSave", bNeedSave);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

				/*
                IList<BomItemInfo> bom = currentSession.GetValue(Session.SessionKeys.SessionBom) as List<BomItemInfo>;
                IList<BomItemInfo> retBom = new List<BomItemInfo>();
				foreach (BomItemInfo tmp in bom)
                {
                    retBom.Add(tmp);
                }
				*/
				
				IProduct p = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                ArrayList arr = new ArrayList();
                arr.Add(p.ToProductInfo());
                string aliasLine = GetLineAlias(line);
                arr.Add((string)currentSession.GetValue("IsAOILine"));
                if ((string)currentSession.GetValue("IsAOILine") == "Y")
                {
                    string addr = GetAOI_Addr(aliasLine);
                    arr.Add(addr);
                    arr.Add((string)currentSession.GetValue("AOIKBPn")); // KB Pn
                    arr.Add((string)currentSession.GetValue("AOILabelList"));// Label Pn;
                 }
                else
                {
                    arr.Add("");
                    arr.Add(""); // KB Pn
                    arr.Add("");// Label Pn;
                }
			    
				//arr.Add(retBom);
				//get bom
                //IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, TheType);
                IFlatBOM CurrenBom = (IFlatBOM)currentSession.GetValue(Session.SessionKeys.SessionBom);
                IList<IMES.DataModel.BomItemInfo> bomItemList = new List<BomItemInfo>();
                if (CurrenBom != null)
                {
                    bomItemList = CurrenBom.ToBOMItemInfoList();
                }
                arr.Add(bomItemList);
				
                return arr; //0:ProductInfo 1:need aoi 2:addr 3:kb pn 4:label pn;
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
                logger.Debug("(_ESOPandAoiKbTest)GetProductInfo end, productId:" + prodId);
            }
        }
		
		public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            return PartCollection.TryPartMatchCheck(sessionKey, TheType, checkValue);
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(string custsn, IList<string> defectList)
        {
            logger.Debug("(_ESOPandAoiKbTest)save start");
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(custsn, TheType);
                if (session == null)
                {
                    erpara.Add(custsn);
                    ex = new FisException("CHK194", erpara);
                    throw ex;
                }
                if (defectList.Count == 0)
                { defectList = null; }
                session.AddValue(Session.SessionKeys.DefectList, defectList);
                session.Exception = null;
                session.AddValue(Session.SessionKeys.IsComplete, true);

                session.SwitchToWorkFlow();

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }
                //CheckEPIA = (string)session.GetValue(ExtendSession.SessionKeys.FAQCStatus);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(_ESOPandAoiKbTest)save end.");
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        public void Cancel(string sesKey)
        {
            logger.Debug("(_ESOPandAoiKbTest)Cancel start.");

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sesKey, TheType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(_ESOPandAoiKbTest)Cancel end.");
            }
        }

        /// <summary>
        /// 根据model和Light Code获取LightGuide列表
        /// </summary>
        public IList<WipBuffer> getBomData(string model, string code)
        {
            logger.Debug("(_ESOPandAoiKbTest)getBomData start, model:" + model + " KittingCode:" + code);
            List<string> erpara = new List<string>();

            try
            {
                IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                //IList<WipBuffer> wbList = productRepository.GetWipBufferListFromWipBuffer(code, "FA Label");
                string labelLightSetting = "LabelLightGuidType";
                string labelLight = "PAK Label";
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> values = partRep.GetValueFromSysSettingByName(labelLightSetting);
                if (values != null && values.Count > 0)
                {
                    labelLight = values[0];
                }
                else
                {
                    erpara.Add(labelLightSetting);
                    FisException ex = new FisException("CQCHK0020", erpara);
                    throw ex;
                }
                IList<WipBuffer> wbList = productRepository.GetWipBufferListFromWipBuffer(code, labelLight);

                IHierarchicalBOM bom = bomRepository.GetHierarchicalBOMByModel(model);
                IList<IBOMNode> nodes = bom.FirstLevelNodes;

                string syscode = "";
                IList<string> codes = bomRepository.GetOsCodeFromBomCode(model);
                if (codes.Count > 0) syscode = codes[0];
                char bc_code = 'a';
                if (model.Length >= 7) bc_code = model[6];

                IList<string> pnList = new List<string>();
                if (model.EndsWith("A") || model.EndsWith("B") || syscode == "00010" || syscode == "00011" || (bc_code >= '0' && bc_code <= '9'))
                {
                    foreach (IBOMNode tmp in nodes)
                    {
                        pnList.Add(tmp.Part.PN);
                    }
                }
                else
                {
                    foreach (IBOMNode tmp in nodes)
                    {
                        if (tmp.Part.PN != "6060B0232501" && tmp.Part.PN != "6060B0153901")
                        {
                            pnList.Add(tmp.Part.PN);
                        }
                    }
                }

                IList<WipBuffer> ret = new List<WipBuffer>();

                foreach (WipBuffer ele in wbList)
                {
                    if (pnList.Contains(ele.PartNo))
                    {
                        ret.Add(ele);
                    }
                }

                return ret;
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
                logger.Debug("(_ESOPandAoiKbTest)GetTableData end, model:" + model + " KittingCode:" + code);
            }
        }

        /// <summary>
        /// 根据hostname获取Comm设置列表
        /// </summary>
        public IList<COMSettingInfo> getCommSetting(string hostname, string editor)
        {
            logger.Debug("(_ESOPandAoiKbTest)getCommSetting start, hostname:" + hostname);
            try
            {

                IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IList<COMSettingInfo> commSettingList = new List<COMSettingInfo>();
                commSettingList = iPalletRepository.FindCOMSettingByName(hostname);

                if (commSettingList == null || commSettingList.Count <= 0)
                {
                    COMSettingInfo ele = new COMSettingInfo();
                    ele.name = hostname;
                    ele.editor = editor;
                    ele.commPort = "1";
                    ele.baudRate = "9600,n,8,1";
                    ele.rthreshold = 1;
                    ele.sthreshold = 1;
                    ele.handshaking = 0;
                    iPalletRepository.AddCOMSettingItem(ele);
                    commSettingList.Add(ele);
                }
                return commSettingList;
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
                logger.Debug("(_ESOPandAoiKbTest)getCommSetting end, hostname:" + hostname);
            }
        }

        private string GetLineAlias(string Line)
        {
            ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
            Line line = lineRepository.Find(Line);
            if (line.LineEx != null && !string.IsNullOrEmpty(line.LineEx.AliasLine))
            {return line.LineEx.AliasLine.Trim(); }
            else
            { return ""; }
        }
        private string GetAOI_Addr(string line)
        {
            //    各線別的AOI機台IP 請抓取 ConstValue Type='AOIServerIP' and Name='LineAlias' 
            CommonImpl cmi = new CommonImpl();
            IList<ConstValueInfo> lstConst = cmi.GetConstValueListByType("AOIServerIP", "Name").Where(y => y.name == line).ToList();
            return lstConst.Count == 0 ? "" : lstConst[0].value.Trim();
        }
    

        public void AOICallBack(string sn, string editor, string station,
                                                string line, string customer, string result, string errorCode, string errorDesc)
        {
            logger.Debug("(AOICallBack)GetProductInfo start, CUSTSN:" + sn);

            FisException ex;
            List<string> erpara = new List<string>();
            // Fail -> errorCode : KPAOI
            try
            {
                string sessionKey = sn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, station, line, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("AOICallBack.xoml", "AOICallBack.rules", wfArguments);
                    result=result.Trim().ToUpper();
                    currentSession.AddValue("AOITestResult", result);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }
                    //TestLogActionName
                    currentSession.AddValue(ExtendSession.SessionKeys.TestLogActionName, "KBAOI");
                    if(result!="PASS")
                    {
                        //result
                        currentSession.AddValue(ExtendSession.SessionKeys.TestLogErrorCode, errorCode);
                        currentSession.AddValue(ExtendSession.SessionKeys.TestLogDescr, errorDesc);
                    //  IList<string> defectList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.DefectList);
                        IList<string> defectList = new List<string>() { "KBAOI" };
                        currentSession.AddValue(Session.SessionKeys.DefectList, defectList);
                
                    }
                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
                    throw ex;
                }
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }


            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
            }
            finally
            {
                logger.Debug("(AOICallBack)CUSTSN:" + sn);
            }
        }

        /// <summary>
        /// 处理输入资产标签，返回结合的PartNo
        /// </summary>
        public string InputASTLabel(string sesKey, string ast)
        {
            logger.Debug("(ESOPandAoiKbTest)InputASTLabel start, ast:" + ast);

            FisException ex;
            List<string> erpara = new List<string>();


            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sesKey, TheType);

                if (currentSession == null)
                {
                    erpara.Add(sesKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }

                currentSession.AddValue(Session.SessionKeys.VCode, ast);

                currentSession.SwitchToWorkFlow();

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                return currentSession.GetValue(Session.SessionKeys.MatchedCheckItem) as string;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(ESOPandAoiKbTest)InputASTLabel end, ast:" + ast);
            }
        }

    }

}
        #endregion
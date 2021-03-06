﻿/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Output
* UI:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20 
* UC:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.Route;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// PAQCOutput接口的实现类
    /// </summary>
    public class PAQCOutputImpl : MarshalByRefObject, PAQCOutput
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region members

        /// <summary>
        /// 刷uutSn，启动工作流，检查输入的uutSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="uutSn"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ProductModel</returns>
        public ArrayList InputSN(string uutSn, string line, string editor, string station, string customer)
        {
            logger.Debug("(PAQCOutputImpl)InputSN start, uutSn:" + uutSn);

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(uutSn, CommonImpl.InputTypeEnum.CustSN);
                //****** add by Benson at 2013/05/10 for Unit Weight当Non BT 产品时，只能由Pass Ship to Station(95) 进入
                /* Move the logic form SP [IMES_SFC_Product] to here
                 * else if (@CurrentStation ='PO'and @PreStation ='85')
                     begin
                         if (@NeedPODLabel ='Y' OR @NeedConfigLabel ='Y')
                         begin
                             select 'SFC014',@Key
                             return
                         end
                     end */
                string site = IMES.Infrastructure.Utility.Common.CommonUti.GetSite();
                if (site != "ICC")
                {
                    IList<ProductLog> lstLog = currentProduct.ProductLogs.OrderByDescending(x => x.Cdt).ToList();
                    string preStation = lstLog[0].Station;
                    string label = "";
                    if (station == "PO" && preStation == "85")
                    {
                        CommonImpl2 cm2 = new CommonImpl2();
                        label = cm2.CheckPodLabel(uutSn);
                        if (label == "")
                        { label = cm2.CheckConfigLabel(uutSn); }
                        if (string.IsNullOrEmpty(label))
                        {
                            throw new FisException("SFC014", new string[] { });
                        }
                    }
                }
              


                //****** add by Benson at 2013/05/10

                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "043PAQCOutput.xoml", "043PAQCOutput.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
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
                //==============================================================================
                ArrayList retList = new ArrayList();
                //Get infomation
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                IMES.DataModel.ProductModel currentModel = new IMES.DataModel.ProductModel();

                //a.	如果Product 在QCStatus 中不存在记录，则报告错误：“此Product 在QCStatus 中不存在记录，请联系相关人员”

                if (curProduct.QCStatus.Count == 0)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("PAK110", erpara);//此Product 在QCStatus 中不存在记录，请联系相关人员
                    throw ex;

                }

                currentModel.CustSN = curProduct.CUSTSN;
                currentModel.ProductID = curProduct.ProId;
                currentModel.Model = curProduct.Model;

                retList.Add(currentModel);


                //select b.Line + ' ' + b.Descr from ProductStatus a (nolock), Line b (nolock)
                //WHERE a.ProductID = @ProductId
                //AND a.Line = b.Line
                ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
                Line pdline = lineRepository.Find(curProduct.Status.Line);

                string strline = "";
                strline = pdline.Id + " " + pdline.Descr;
                retList.Add(strline);
                //===============================================================================

                return retList;

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
                logger.Debug("(PAQCOutputImpl)InputSN end, uutSn:" + uutSn);
            }
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        public void save(string prodId, string line, string editor, IList<string> defectCodeList)
        {
            logger.Debug("(PAQCOutputImpl)save start,"
                + " [prodId]: " + prodId
                + " [defectList]:" + defectCodeList);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.DefectList, defectCodeList);
                    session.AddValue(Session.SessionKeys.HasDefect, (defectCodeList != null && defectCodeList.Count != 0) ? true : false);
                    session.Exception = null;
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

                    if (defectCodeList.Count == 0)
                    {
                        return;
                    }


                    //a.	获取PAQC Sorting 支持的站点
                    Product curProduct = (Product)session.GetValue(Session.SessionKeys.Product);
                    IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                    IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

                    IList<string> valueList = new List<string>();
                    valueList = partRep.GetValueFromSysSettingByName("PAQCSortingStation");

                    string[] lineArray = line.Split(' ');
                    string curLine = lineArray[0];

                    //b.	对于PAQC Sorting 支持的每一个站点，Insert PAQCSorting
                    //INSERT INTO PAQCSorting([Station],[Line],[Status],[PreviousFailTime],[Remark],[Editor],[Cdt],[Udt])
                    //VALUES(@Station, @Line, 'I', GETDATE(), 'Automatically add at PAQC Output', @Editor, GETDATE(), GETDATE())
                    //@Line – ProductStatus.Line
                    //@Station - PAQC Sorting 支持的某一个站点

                    string[] stationArray = valueList[0].Split(',');
                    foreach (string stationID in stationArray)
                    {
                        IPAQCSorting sorting = new PAQCSortingImpl();
                        IList<PaqcsortingInfo> sortingList = modelRep.GetPreviousFailTimeList(curLine, stationID);
                        PaqcsortingInfo udata = new PaqcsortingInfo();
                        udata.station = stationID;
                        udata.line = curLine;
                        udata.status = "I";
                        udata.previousFailTime = DateTime.Now;
                        udata.remark = "Automatically add at PAQC Output";
                        udata.editor = editor;
                        udata.cdt = DateTime.Now;
                        udata.udt = DateTime.Now;

                        modelRep.InsertPqacSortingInfo(udata);

                        //c.	对于上一步Insert PAQCSorting 表的每一条记录，
                        //都需要在PAQCSorting_Product Insert 与当前Product 的结合记录
                        //INSERT INTO [PAQCSorting_Product]([PAQCSortingID],[CUSTSN],[Status],[Editor],[Cdt])
                        //VALUES(@PAQCSortingID, @CustomerSN, 2, @Editor, GETDATE())
                        //PAQCSortingID - 上一步Update / Insert PAQCSorting 表的某一条记录的ID
                        //@Customer – Product.CUSTSN

                        PaqcsortingProductInfo pitem = new PaqcsortingProductInfo();
                        pitem.paqcsortingid = udata.id;
                        pitem.custsn = curProduct.CUSTSN;
                        pitem.status = 2;
                        pitem.editor = editor;
                        pitem.cdt = DateTime.Now;

                        modelRep.InsertPqacSortingProductInfo(pitem);                      
                        
                    }

                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(PAQCOutputImpl)save end,"
                   + " [prodId]: " + prodId
                   + " [defectList]:" + defectCodeList);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void cancel(string prodId)
        {
            logger.Debug("(PAQCOutputImpl)Cancel start, [prodId]:" + prodId);
            //FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(PAQCOutputImpl)Cancel end, [prodId]:" + prodId);
            }
        }
        #endregion

    }
}

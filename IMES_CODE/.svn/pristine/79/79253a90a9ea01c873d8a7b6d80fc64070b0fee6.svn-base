/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 02
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 2.docx –2011/11/15 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 2.docx –2011/11/15            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-15   Du.Xuan               Create   
* ITC-1413-0006 增加Consumer Family / Commercial Family判断
* Known issues:
* TODO：
* 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure.Extend;
using log4net;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using System.Data;
using System.Linq;
using IMES.Station.Interface.CommonIntf;
using IMES.Common;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPizzaKitting接口的实现类
    /// </summary>
    public class PizzaKitting : MarshalByRefObject, IPizzaKitting
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region members

        /// <summary>
        /// 刷custSn，启动工作流，检查输入的custSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ProductModel</returns>
        public ArrayList InputSN(string custSN, string line, string curStation,
                                                    string editor, string station, string customer)
        {
            
			string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(custSn:{1} Station:{2} Line:{3} Editor:{4} Customer:{5})", methodName, custSN, curStation, line, editor, customer);
			try	 
            {   
                #region Declare variable
                ArrayList retLst = new ArrayList();
                string wfName="PizzaKitting.xoml"; 
                string wfRule="PizzaKitting.rules";
                string sessionKey = null;
                Dictionary<string,object> sessionKeyValueList = new Dictionary<string,object>();
                Session currentSession = null;
                #endregion

                #region Set SessionKey
                //Get SessionKey
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                sessionKey = currentProduct.ProId;
                #endregion

                #region Set SessionKeyValue
                //Add by Benson for Mantis 0001633
                IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                     
                IList<string> sttingLst = new List<string>();
                sttingLst = ipartRepository.GetValueFromSysSettingByName("BatteryCtCheck");
                if (sttingLst.Count>0 && sttingLst[0].ToString().ToUpper().Trim() == "Y")
                { 
                        sessionKeyValueList.Add("EnableBatteryCtCheck", true); 
                 }
                else
                { 
                    sessionKeyValueList.Add("EnableBatteryCtCheck", false); 
                }
                IList<string> sttingLst2 = new List<string>(); //OnlyCheckOneBattery
                sttingLst2 = ipartRepository.GetValueFromSysSettingByName("OnlyCheckOneBattery");
                if (sttingLst2.Count>0 && sttingLst2[0].ToString().ToUpper().Trim() == "Y")
                { 
                    sessionKeyValueList.Add("OnlyCheckOneBattery", true); 
                }
                else
                { 
                    sessionKeyValueList.Add("OnlyCheckOneBattery", false); 
                }
                //Add by Benson for Mantis 0001633 a
                sessionKeyValueList.Add(Session.SessionKeys.IsComplete, false);
                #endregion

                #region Set workflow  name and rule
                string site = ActivityCommonImpl.Instance.GetSite(null);
                if (curStation != "PKCK")
               {
                    if (site == "ICC")
                    {
                        wfName="PizzaKitting_CQ.xoml"; 
                        wfRule="PizzaKitting_CQ.rules";   
                    }                      
                }
                else
               {
                    wfName="PizzaKittingCheck.xoml"; 
                    wfRule="PizzaKittingCheck.rules";
               }
                #endregion
                
                //executing workflow, if fail then throw error 
                currentSession =WorkflowUtility.InvokeWF(sessionKey, curStation, line, customer, editor, SessionType, wfName, wfRule, sessionKeyValueList);

                #region After workflow executed, setting return UI Data from workflow result's session object
                //get product data for UI
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                DataModel.ProductInfo prodInfo = curProduct.ToProductInfo();
                retLst.Add(prodInfo); // retLst idx 0

				string IsBSamModel = currentSession.GetValue(ExtendSession.SessionKeys.IsBSamModel) as string;

                if ("Y".Equals(IsBSamModel) && "PK04".Equals(curStation))
                {
                    IList<WipBufferDef> wipBufferList = (IList<WipBufferDef>)currentSession.GetValue("WipBuffer");
                    retLst.Add(wipBufferList); // retLst idx 1
                }
                else
                {
                    //get bom
                    IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, SessionType);
                    //IList<BomItemInfo> bomItemList = new List<BomItemInfo>() ;
                    /*for (int i = 1; i < 14; i++)
                    {
                        BomItemInfo bom =new BomItemInfo();
                        pUnit unit = new pUnit();
                        PartNoInfo info = new PartNoInfo();
                        unit.pn = "unitpn"+Convert.ToString(i);
                        info.id = "partid" + Convert.ToString(i);
                        bom.parts = new List<PartNoInfo>();
                        bom.parts.Add(info);
                        bom.type = "type"+Convert.ToString(i);
                        bom.description="description"+Convert.ToString(i);
                        bom.qty = i;
                        bom.scannedQty = i;
                        bom.collectionData = new List<pUnit>();
                        bom.collectionData.Add(unit);
                        bomItemList.Add(bom);
						
                    }*/


                    if ("Y".Equals(IsBSamModel) && "PKOK".Equals(curStation))
                    {
                        IList<BomItemInfo> removeFromBom = new List<BomItemInfo>();
                        foreach (var bom in bomItemList)
                        {
                            bool doneChk = false;
                            foreach (var part in bom.parts)
                            {
                                foreach (var p in part.properties)
                                {
                                    if ("TYPE".Equals(p.Name) && "BOX".Equals(p.Value))
                                    {
                                        doneChk = true;
                                        break;
                                    }
                                }
                                if (doneChk)
                                    break;
                            }
                            if (doneChk)
                                removeFromBom.Add(bom);
                        }
                        foreach (BomItemInfo bom in removeFromBom)
                        {
                            bomItemList.Remove(bom);
                        }
                    }
                    //Remove Carton Part :Mantis1816  将Kitting里的Carton料号check调换到SN CHECK站。原来展BOM CHECK逻辑不变。
                    retLst.Add(bomItemList);
                }
                retLst.Add(IsBSamModel); // retLst idx 2
                //============================================================================
                #endregion

                return retLst;
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
				logger.DebugFormat("END: {0}()", methodName);
            }

            #region rearrange code and disable old code
            //try
            //{  
            //    ArrayList retLst = new ArrayList();

            //    var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);

            //    string sessionKey = currentProduct.ProId;
            //    Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

            //    //用ProductID启动工作流，将Product放入工作流中
            //    if (currentSession == null)
            //    {
            //        currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

            //        Dictionary<string, object> wfArguments = new Dictionary<string, object>();
            //        wfArguments.Add("Key", sessionKey);
            //        wfArguments.Add("Station", curStation);
            //        wfArguments.Add("CurrentFlowSession", currentSession);
            //        wfArguments.Add("Editor", editor);
            //        wfArguments.Add("PdLine", line);
            //        wfArguments.Add("Customer", customer);
            //        wfArguments.Add("SessionType", SessionType);
            //        //Add by Benson for Mantis 0001633
            //        IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                     
            //        IList<string> sttingLst = new List<string>();
            //        sttingLst = ipartRepository.GetValueFromSysSettingByName("BatteryCtCheck");
            //        if (sttingLst.Count>0 && sttingLst[0].ToString().ToUpper().Trim() == "Y")
            //        { 
            //            currentSession.AddValue("EnableBatteryCtCheck", true); 
            //        }
            //        else
            //        { 
            //            currentSession.AddValue("EnableBatteryCtCheck", false); 
            //        }
            //        IList<string> sttingLst2 = new List<string>(); //OnlyCheckOneBattery
            //        sttingLst2 = ipartRepository.GetValueFromSysSettingByName("OnlyCheckOneBattery");
            //        if (sttingLst2.Count>0 && sttingLst2[0].ToString().ToUpper().Trim() == "Y")
            //        { 
            //            currentSession.AddValue("OnlyCheckOneBattery", true); 
            //        }
            //        else
            //        { 
            //            currentSession.AddValue("OnlyCheckOneBattery", false); 
            //        }
                   
            //        //Add by Benson for Mantis 0001633 a

            //        CommonImpl2 cm2 = new CommonImpl2();
            //        string site = cm2.GetSite();

            //        string wfName, rlName;
            //        if (curStation != "PKCK")
            //        {
            //            if (site == "ICC")
            //            {
            //                RouteManagementUtils.GetWorkflow(station, "PizzaKitting_CQ.xoml", "PizzaKitting_CQ.rules", out wfName, out rlName);
                        
            //            }
            //            else
            //            {
            //                RouteManagementUtils.GetWorkflow(station, "PizzaKitting.xoml", "PizzaKitting.rules", out wfName, out rlName);
                        
            //            }
            //        }
            //        else
            //        {
            //            RouteManagementUtils.GetWorkflow(station, "PizzaKittingCheck.xoml", "PizzaKittingCheck.rules", out wfName, out rlName);
            //        }
            //        WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

            //        currentSession.AddValue(Session.SessionKeys.IsComplete, false);
            //        currentSession.SetInstance(instance);

            //        if (!SessionManager.GetInstance.AddSession(currentSession))
            //        {
            //            currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
            //            FisException ex;
            //            List<string> erpara = new List<string>();
            //            erpara.Add(sessionKey);
            //            ex = new FisException("CHK020", erpara);
            //            throw ex;
            //        }

            //        currentSession.WorkflowInstance.Start();
            //        currentSession.SetHostWaitOne();
            //    }
            //    else
            //    {
            //        FisException ex;
            //        List<string> erpara = new List<string>();
            //        erpara.Add(sessionKey);
            //        ex = new FisException("CHK020", erpara);
            //        throw ex;
            //    }

            //    if (currentSession.Exception != null)
            //    {
            //        if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
            //        {
            //            currentSession.ResumeWorkFlow();
            //        }

            //        throw currentSession.Exception;
            //    }

            //    //============================================================================

              


            //    //get product data for UI
            //    Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
            //    DataModel.ProductInfo prodInfo = curProduct.ToProductInfo();
            //    retLst.Add(prodInfo); // retLst idx 0

            //    string IsBSamModel = currentSession.GetValue(ExtendSession.SessionKeys.IsBSamModel) as string;
				
            //    if ("Y".Equals(IsBSamModel) && "PK04".Equals(curStation)){
            //        IList<WipBufferDef> wipBufferList = (IList<WipBufferDef>)currentSession.GetValue("WipBuffer");
            //        retLst.Add(wipBufferList); // retLst idx 1
            //    }
            //    else {
            //        //get bom
            //        IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, SessionType);
            //        //IList<BomItemInfo> bomItemList = new List<BomItemInfo>() ;
            //        /*for (int i = 1; i < 14; i++)
            //        {
            //            BomItemInfo bom =new BomItemInfo();
            //            pUnit unit = new pUnit();
            //            PartNoInfo info = new PartNoInfo();
            //            unit.pn = "unitpn"+Convert.ToString(i);
            //            info.id = "partid" + Convert.ToString(i);
            //            bom.parts = new List<PartNoInfo>();
            //            bom.parts.Add(info);
            //            bom.type = "type"+Convert.ToString(i);
            //            bom.description="description"+Convert.ToString(i);
            //            bom.qty = i;
            //            bom.scannedQty = i;
            //            bom.collectionData = new List<pUnit>();
            //            bom.collectionData.Add(unit);
            //            bomItemList.Add(bom);
						
            //        }*/
              

            //        if ("Y".Equals(IsBSamModel) && "PKOK".Equals(curStation))
            //        {
            //            IList<BomItemInfo> removeFromBom = new List<BomItemInfo>();
            //            foreach (var bom in bomItemList)
            //            {
            //                bool doneChk = false;
            //                foreach (var part in bom.parts)
            //                {
            //                    foreach (var p in part.properties){
            //                        if ("TYPE".Equals(p.Name) && "BOX".Equals(p.Value)){
            //                            doneChk = true;
            //                            break;
            //                        }
            //                    }
            //                    if (doneChk)
            //                        break;
            //                }
            //                if (doneChk)
            //                    removeFromBom.Add(bom);
            //            }
            //            foreach (BomItemInfo bom in removeFromBom)
            //            {
            //                bomItemList.Remove(bom);
            //            }
            //        }
            //        //Remove Carton Part :Mantis1816  将Kitting里的Carton料号check调换到SN CHECK站。原来展BOM CHECK逻辑不变。
              

            //        retLst.Add(bomItemList);
            //    }
            //    retLst.Add(IsBSamModel); // retLst idx 2
            //    //============================================================================
            //    return retLst;

            //}
            //catch (FisException e)
            //{
            //    logger.Error(e.mErrmsg, e);
            //    throw new Exception(e.mErrmsg);
            //}
            //catch (Exception e)
            //{
            //    logger.Error(e.Message, e);
            //    throw new SystemException(e.Message);
            //}
            //finally
            //{
            //    logger.Debug("(PizzaKitting)InputSN end,  custSn:" + custSN);
            //}
            #endregion
        }


        public ArrayList InputPizzaCheckSN(string custSN, string line, string curStation,
                                                                   string editor, string station, string customer)
        {
            
			string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(custSn:{1} Station:{2} Line:{3} Editor:{4} Customer:{5})", methodName, custSN, curStation, line, editor, customer);
			try	 
            {
                #region Declare variable
                ArrayList retLst = new ArrayList();
                string wfName = "PizzaKittingCheck.xoml";
                string wfRule = "PizzaKittingCheck.rules";
                string sessionKey = null;
                Dictionary<string, object> sessionKeyValueList = new Dictionary<string, object>();
                Session currentSession = null;
                #endregion

                #region Set SessionKey
                //Get SessionKey
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                sessionKey = currentProduct.ProId;
                #endregion

                #region Set SessionKeyValue
                //Add by Benson for Mantis 0001633
                IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

                IList<string> sttingLst = new List<string>();
                sttingLst = ipartRepository.GetValueFromSysSettingByName("BatteryCtCheck");
                if (sttingLst.Count > 0 && sttingLst[0].ToString().ToUpper().Trim() == "Y")
                {
                    sessionKeyValueList.Add("EnableBatteryCtCheck", true);
                }
                else
                {
                    sessionKeyValueList.Add("EnableBatteryCtCheck", false);
                }
                IList<string> sttingLst2 = new List<string>(); //OnlyCheckOneBattery
                sttingLst2 = ipartRepository.GetValueFromSysSettingByName("OnlyCheckOneBattery");
                if (sttingLst2.Count > 0 && sttingLst2[0].ToString().ToUpper().Trim() == "Y")
                {
                    sessionKeyValueList.Add("OnlyCheckOneBattery", true);
                }
                else
                {
                    sessionKeyValueList.Add("OnlyCheckOneBattery", false);
                }
                //Add by Benson for Mantis 0001633 a
                sessionKeyValueList.Add(Session.SessionKeys.IsComplete, false);
                #endregion

                //executing workflow, if fail then throw error 
                currentSession = WorkflowUtility.InvokeWF(sessionKey, curStation, line, customer, editor, SessionType, wfName, wfRule, sessionKeyValueList);

                #region After workflow executed, setting return UI Data from workflow result's session object
                //get product data for UI
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                DataModel.ProductInfo prodInfo = curProduct.ToProductInfo();
                retLst.Add(prodInfo); // retLst idx 0

                string IsBSamModel = currentSession.GetValue(ExtendSession.SessionKeys.IsBSamModel) as string;

                if ("Y".Equals(IsBSamModel) && "PK04".Equals(curStation))
                {
                    IList<WipBufferDef> wipBufferList = (IList<WipBufferDef>)currentSession.GetValue("WipBuffer");
                    retLst.Add(wipBufferList); // retLst idx 1
                }
                else
                {
                    //get bom
                    IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, SessionType);
                    //IList<BomItemInfo> bomItemList = new List<BomItemInfo>() ;
                    /*for (int i = 1; i < 14; i++)
                    {
                        BomItemInfo bom =new BomItemInfo();
                        pUnit unit = new pUnit();
                        PartNoInfo info = new PartNoInfo();
                        unit.pn = "unitpn"+Convert.ToString(i);
                        info.id = "partid" + Convert.ToString(i);
                        bom.parts = new List<PartNoInfo>();
                        bom.parts.Add(info);
                        bom.type = "type"+Convert.ToString(i);
                        bom.description="description"+Convert.ToString(i);
                        bom.qty = i;
                        bom.scannedQty = i;
                        bom.collectionData = new List<pUnit>();
                        bom.collectionData.Add(unit);
                        bomItemList.Add(bom);
						
                    }*/


                    if ("Y".Equals(IsBSamModel) && "PKOK".Equals(curStation))
                    {
                        IList<BomItemInfo> removeFromBom = new List<BomItemInfo>();
                        foreach (var bom in bomItemList)
                        {
                            bool doneChk = false;
                            foreach (var part in bom.parts)
                            {
                                foreach (var p in part.properties)
                                {
                                    if ("TYPE".Equals(p.Name) && "BOX".Equals(p.Value))
                                    {
                                        doneChk = true;
                                        break;
                                    }
                                }
                                if (doneChk)
                                    break;
                            }
                            if (doneChk)
                                removeFromBom.Add(bom);
                        }
                        foreach (BomItemInfo bom in removeFromBom)
                        {
                            bomItemList.Remove(bom);
                        }
                    }
                    //Remove Carton Part :Mantis1816  将Kitting里的Carton料号check调换到SN CHECK站。原来展BOM CHECK逻辑不变。
                    retLst.Add(bomItemList);
                }
                retLst.Add(IsBSamModel); // retLst idx 2
                //============================================================================
                #endregion

                return retLst;
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
				logger.DebugFormat("END: {0}()", methodName);
            }

            #region rearrange code and disable old code
            //logger.Debug("(InputPizzaCheckSN)InputSN start, custSn:" + custSN
            //    + "Station:" + curStation);

            //try
            //{
            //    ArrayList retLst = new ArrayList();

            //    var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);

            //    string sessionKey = currentProduct.ProId;
            //    Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

            //    //用ProductID启动工作流，将Product放入工作流中
            //    if (currentSession == null)
            //    {
            //        currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

            //        Dictionary<string, object> wfArguments = new Dictionary<string, object>();
            //        wfArguments.Add("Key", sessionKey);
            //        wfArguments.Add("Station", curStation);
            //        wfArguments.Add("CurrentFlowSession", currentSession);
            //        wfArguments.Add("Editor", editor);
            //        wfArguments.Add("PdLine", line);
            //        wfArguments.Add("Customer", customer);
            //        wfArguments.Add("SessionType", SessionType);
            //        //Add by Benson for Mantis 0001633
            //        IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            //        IList<string> sttingLst = new List<string>();
            //        sttingLst = ipartRepository.GetValueFromSysSettingByName("BatteryCtCheck");
            //        if (sttingLst.Count > 0 && sttingLst[0].ToString().ToUpper().Trim() == "Y")
            //        { currentSession.AddValue("EnableBatteryCtCheck", true); }
            //        else
            //        { currentSession.AddValue("EnableBatteryCtCheck", false); }
            //        IList<string> sttingLst2 = new List<string>(); //OnlyCheckOneBattery
            //        sttingLst2 = ipartRepository.GetValueFromSysSettingByName("OnlyCheckOneBattery");
            //        if (sttingLst2.Count > 0 && sttingLst2[0].ToString().ToUpper().Trim() == "Y")
            //        { currentSession.AddValue("OnlyCheckOneBattery", true); }
            //        else
            //        { currentSession.AddValue("OnlyCheckOneBattery", false); }

            //        //Add by Benson for Mantis 0001633 a


            //        string wfName, rlName;
                    
            //        RouteManagementUtils.GetWorkflow(station, "PizzaKittingCheck.xoml", "PizzaKittingCheck.rules", out wfName, out rlName);
                   
            //        WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

            //        currentSession.AddValue(Session.SessionKeys.IsComplete, false);
            //        currentSession.SetInstance(instance);

            //        if (!SessionManager.GetInstance.AddSession(currentSession))
            //        {
            //            currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
            //            FisException ex;
            //            List<string> erpara = new List<string>();
            //            erpara.Add(sessionKey);
            //            ex = new FisException("CHK020", erpara);
            //            throw ex;
            //        }

            //        currentSession.WorkflowInstance.Start();
            //        currentSession.SetHostWaitOne();
            //    }
            //    else
            //    {
            //        FisException ex;
            //        List<string> erpara = new List<string>();
            //        erpara.Add(sessionKey);
            //        ex = new FisException("CHK020", erpara);
            //        throw ex;
            //    }

            //    if (currentSession.Exception != null)
            //    {
            //        if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
            //        {
            //            currentSession.ResumeWorkFlow();
            //        }

            //        throw currentSession.Exception;
            //    }

            //    //============================================================================

            //    //get product data for UI
            //    Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
            //    DataModel.ProductInfo prodInfo = curProduct.ToProductInfo();
            //    retLst.Add(prodInfo); // retLst idx 0

            //    string IsBSamModel = currentSession.GetValue(ExtendSession.SessionKeys.IsBSamModel) as string;

            //    if ("Y".Equals(IsBSamModel) && "PK04".Equals(curStation))
            //    {
            //        IList<WipBufferDef> wipBufferList = (IList<WipBufferDef>)currentSession.GetValue("WipBuffer");
            //        retLst.Add(wipBufferList); // retLst idx 1
            //    }
            //    else
            //    {
            //        //get bom
            //        IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, SessionType);
            //        //IList<BomItemInfo> bomItemList = new List<BomItemInfo>() ;
            //        /*for (int i = 1; i < 14; i++)
            //        {
            //            BomItemInfo bom =new BomItemInfo();
            //            pUnit unit = new pUnit();
            //            PartNoInfo info = new PartNoInfo();
            //            unit.pn = "unitpn"+Convert.ToString(i);
            //            info.id = "partid" + Convert.ToString(i);
            //            bom.parts = new List<PartNoInfo>();
            //            bom.parts.Add(info);
            //            bom.type = "type"+Convert.ToString(i);
            //            bom.description="description"+Convert.ToString(i);
            //            bom.qty = i;
            //            bom.scannedQty = i;
            //            bom.collectionData = new List<pUnit>();
            //            bom.collectionData.Add(unit);
            //            bomItemList.Add(bom);
						
            //        }*/

            //        if ("Y".Equals(IsBSamModel) && "PKOK".Equals(curStation))
            //        {
            //            IList<BomItemInfo> removeFromBom = new List<BomItemInfo>();
            //            foreach (var bom in bomItemList)
            //            {
            //                bool doneChk = false;
            //                foreach (var part in bom.parts)
            //                {
            //                    foreach (var p in part.properties)
            //                    {
            //                        if ("TYPE".Equals(p.Name) && "BOX".Equals(p.Value))
            //                        {
            //                            doneChk = true;
            //                            break;
            //                        }
            //                    }
            //                    if (doneChk)
            //                        break;
            //                }
            //                if (doneChk)
            //                    removeFromBom.Add(bom);
            //            }
            //            foreach (BomItemInfo bom in removeFromBom)
            //            {
            //                bomItemList.Remove(bom);
            //            }
            //        }

            //        retLst.Add(bomItemList);
            //    }
            //    retLst.Add(IsBSamModel); // retLst idx 2
            //    //============================================================================
            //    return retLst;

            //}
            //catch (FisException e)
            //{
            //    logger.Error(e.mErrmsg, e);
            //    throw new Exception(e.mErrmsg);
            //}
            //catch (Exception e)
            //{
            //    logger.Error(e.Message, e);
            //    throw new SystemException(e.Message);
            //}
            //finally
            //{
            //    logger.Debug("(InputPizzaCheckSN)InputSN end,  custSn:" + custSN);
            //}
            #endregion

        }

        public void InputPizzaID(string productID, string pizzaID, string line, string curStation, string model,
                                        string editor, string station, string customer)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(ProductID:{1},PizzaID:{2},Line:{3},CurStation:{4},Model:{5},Editor:{6},Station:{7},Customer{8})",
                                                 methodName,productID, pizzaID, line, curStation, model,editor, station, customer);
            try
            {
                #region Declare variable
                 Session currentSession = null;
                #endregion

                currentSession = WorkflowUtility.GetSession(productID, SessionType,true);

                #region Check Input condition and Session data

                //a.	如果1st Pizza ID 在数据库(IMES_PAK..Pizza.PizzaID)中不存在，
                //      则报告错误：“非法的Pizza ID!”
                IPizzaRepository pizzaRep = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                Pizza pizza = pizzaRep.Find(pizzaID);
                if (pizza == null)
                {
                    //todo: throw FisException
                    throw new FisException("CHK852", new List<string>());
                }

                IMES.FisObject.FA.Product.IProduct product = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                if (pizzaID.CompareTo(product.PizzaID) != 0)
                {
                    //todo: throw FisException
                    throw new FisException("CHK851", new List<string>());
                }
                #endregion

                WorkflowUtility.SwitchToWF(currentSession, null);
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
                logger.DebugFormat("END: {0}()", methodName);
            }

            #region rearrange code and disable old code
           // logger.Debug("(PizzaKitting)InputPizzaID start, PizzaID:" + pizzaID
           //     + "Station:" + station);

           // //return;
           // //a.	如果1st Pizza ID 在数据库(IMES_PAK..Pizza.PizzaID)中不存在，
           // //      则报告错误：“非法的Pizza ID!”
           // IPizzaRepository pizzaRep = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
           // Pizza pizza = pizzaRep.Find(pizzaID);
           // if (pizza == null)
           // {
           //     //todo: throw FisException
           //     throw new FisException("CHK852", new List<string>());
           // }
            
           // //b.	如果1st Pizza ID 与和Product 结合的1st Pizza ID(IMES_FA..Product.PizzaID) 不同，
           // //      则报告错误：“错误的Pizza ID!”
           // Session session = SessionManager.GetInstance.GetSession(productID, SessionType);
           // if (session == null)
           // {
           //     FisException ex;
           //     List<string> erpara = new List<string>();
           //     erpara.Add(productID);
           //     ex = new FisException("CHK021", erpara);
           //     throw ex;
           // }

           // IMES.FisObject.FA.Product.IProduct product = (Product) session.GetValue(Session.SessionKeys.Product);
           // if (pizzaID.CompareTo(product.PizzaID) != 0)
           // {
           //     //todo: throw FisException
           //     throw new FisException("CHK851", new List<string>());
           // }
           // else
           // {
           //     //if (IMES.Infrastructure.Utility.Common.CommonUti.GetSite() != "ICC")
           //     //{
           //         session.Exception = null;
           //         session.SwitchToWorkFlow();
           //         if (session.Exception != null)
           //         {
           //             if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
           //             {
           //                 session.ResumeWorkFlow();
           //             }

           //             throw session.Exception;
           //         }
                
           ////     }

            
                   
           // }
           // return;
            #endregion

        }
        public ArrayList InputPizzaID(string productID, string pizzaID, string line, string curStation, string model,
                                       string editor, string station, string customer, IList<PrintItem> printItems)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(ProductID:{1},PizzaID:{2},Line:{3},CurStation:{4},Model:{5},Editor:{6},Station:{7},Customer{8})",
                                                 methodName, productID, pizzaID, line, curStation, model, editor, station, customer);
            try
            {
                #region Declare variable
                Session currentSession = null;
                #endregion

                currentSession = WorkflowUtility.GetSession(productID, SessionType, true);

                #region Check Input condition and Session data

                //a.	如果1st Pizza ID 在数据库(IMES_PAK..Pizza.PizzaID)中不存在，
                //      则报告错误：“非法的Pizza ID!”
                IPizzaRepository pizzaRep = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                Pizza pizza = pizzaRep.Find(pizzaID);
                if (pizza == null)
                {
                    //todo: throw FisException
                    throw new FisException("CHK852", new List<string>());
                }

                IMES.FisObject.FA.Product.IProduct product = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                if (pizzaID.CompareTo(product.PizzaID) != 0)
                {
                    //todo: throw FisException
                    throw new FisException("CHK851", new List<string>());
                }
                #endregion
                Dictionary<string, object> sessionKeyValueList = new Dictionary<string, object>();
                sessionKeyValueList.Add(Session.SessionKeys.PrintItems, printItems);
                WorkflowUtility.SwitchToWF(currentSession, sessionKeyValueList);

             
             

                string coolabel = (string)currentSession.GetValue("COOLabel");
                string wlabel = (string)currentSession.GetValue("WLabel");
                string clabel = (string)currentSession.GetValue("LanguageLabel");
                string cmessage = (string)currentSession.GetValue("LanguageMessage");
                string llabel = (string)currentSession.GetValue("LANOMLabel");
                string win8label = (string)currentSession.GetValue("Win8BoxLabel");

                IList<string> labelList = new List<string>();
                if (!string.IsNullOrEmpty(coolabel)) labelList.Add(coolabel);
                if (!string.IsNullOrEmpty(wlabel)) labelList.Add(wlabel);
                if (!string.IsNullOrEmpty(clabel)) labelList.Add(clabel);
                if (!string.IsNullOrEmpty(cmessage)) labelList.Add(cmessage);
                if (!string.IsNullOrEmpty(llabel)) labelList.Add(llabel);
                if (!string.IsNullOrEmpty(win8label)) labelList.Add(win8label);

                ArrayList retList = new ArrayList();
                IList<PrintItem> printlist = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                retList.Add(printlist);
                retList.Add(labelList);

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
                logger.DebugFormat("END: {0}()", methodName);
            }

            #region rearrange code and disable old code
            // logger.Debug("(PizzaKitting)InputPizzaID start, PizzaID:" + pizzaID
            //     + "Station:" + station);

            // //return;
            // //a.	如果1st Pizza ID 在数据库(IMES_PAK..Pizza.PizzaID)中不存在，
            // //      则报告错误：“非法的Pizza ID!”
            // IPizzaRepository pizzaRep = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            // Pizza pizza = pizzaRep.Find(pizzaID);
            // if (pizza == null)
            // {
            //     //todo: throw FisException
            //     throw new FisException("CHK852", new List<string>());
            // }

            // //b.	如果1st Pizza ID 与和Product 结合的1st Pizza ID(IMES_FA..Product.PizzaID) 不同，
            // //      则报告错误：“错误的Pizza ID!”
            // Session session = SessionManager.GetInstance.GetSession(productID, SessionType);
            // if (session == null)
            // {
            //     FisException ex;
            //     List<string> erpara = new List<string>();
            //     erpara.Add(productID);
            //     ex = new FisException("CHK021", erpara);
            //     throw ex;
            // }

            // IMES.FisObject.FA.Product.IProduct product = (Product) session.GetValue(Session.SessionKeys.Product);
            // if (pizzaID.CompareTo(product.PizzaID) != 0)
            // {
            //     //todo: throw FisException
            //     throw new FisException("CHK851", new List<string>());
            // }
            // else
            // {
            //     //if (IMES.Infrastructure.Utility.Common.CommonUti.GetSite() != "ICC")
            //     //{
            //         session.Exception = null;
            //         session.SwitchToWorkFlow();
            //         if (session.Exception != null)
            //         {
            //             if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
            //             {
            //                 session.ResumeWorkFlow();
            //             }

            //             throw session.Exception;
            //         }

            ////     }



            // }
            // return;
            #endregion

        }
        public string GetResetLevel()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()",methodName);
            try
            {
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> valueList = new List<string>();
                valueList = partRepository.GetValueFromSysSettingByName("PizzaKittingResetLevel");
                 if (valueList.Count == 0 ||  (valueList[0]!="1" && valueList[0]!="8") )
                {
                    return "0";
                }
                return valueList[0];
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
                //logger.Debug("(PizzaKitting)GetResetLevel");
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue,string resetLevel)
        {
            //MatchedPartOrCheckItem item = new MatchedPartOrCheckItem();
            //item.PNOrItemName = "01C2PNPK110001";
            //return item;
      //      string level = GetResetLevel(); // 0: throw error; 1: throw error & reset other part 2:throw error & reset all
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(sessionKey:{1}, checkValue:{2}, resetLevel:{3})", methodName, sessionKey, checkValue, resetLevel);
            try
            {
                return PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                if (resetLevel != "0")
                {
                    var bom = (IFlatBOM)currentSession.GetValue(Session.SessionKeys.SessionBom);
                    bom.ClearCheckedPart();
                }
                throw ex;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }

            //   return PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
        }
        private void CheckPizzaPart(string sessionKey, string checkValue)
        {
            //Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
            //if (currentSession == null)
            //{
            //    FisException ex;
            //    List<string> erpara = new List<string>();
            //    erpara.Add(sessionKey);
            //    ex = new FisException("CHK021", erpara);
            //    logger.Error(ex.Message, ex);
            //    throw ex;
            //}
            Session currentSession = WorkflowUtility.GetSession(sessionKey, SessionType, true);
            if (currentSession.Station == "PKCK")
            {
                IMES.FisObject.FA.Product.IProduct product = (IMES.FisObject.FA.Product.IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                Pizza p = product.PizzaObj;
                IList<IProductPart> lstPrPart= product.ProductParts;
                bool bPizza = p.PizzaParts.Any(x => x.PartID == checkValue || x.PartID == checkValue.Replace("DIB", "") ||
                                           x.PartID == checkValue.Replace("MMI", "") || x.PartSn == checkValue ||
                                           x.PartID == "DIB" + checkValue || x.PartID == "MMI" + checkValue);
                bool bProduct = lstPrPart.Any(x => x.PartID == checkValue || x.PartID == checkValue.Replace("DIB", "") ||
                                           x.PartID == checkValue.Replace("MMI", "") || x.PartSn == checkValue ||
                                           x.PartID == "DIB" + checkValue || x.PartID == "MMI" + checkValue);
               if(!bPizza && !bProduct)
                {
                    throw new FisException("This part does not exist in pizza part or product part!");
                }
            }
          
        }

        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string site =ActivityCommonImpl.Instance.GetSite("");
                if ("ICC".Equals(site))
                {
                    CheckPizzaPart(sessionKey, checkValue);
                }

                return PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
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
                logger.DebugFormat("END: {0}()", methodName);
            }

            #region rearrange code and disable code 
            //IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            //IList<string> valueList = partRepository.GetValueFromSysSettingByName("Site");
            //string site = valueList.Count > 0 ? valueList[0] : "";
            //if ("ICC".Equals(site))
            //{
            //    CheckPizzaPart(sessionKey, checkValue);
            //}
            // return PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
            #endregion
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        public void Save(string prodId)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(prodId:{1})", methodName, prodId);
            try
            {
                Dictionary<string, object> sessionKeyValueList = new Dictionary<string, object>();
                sessionKeyValueList.Add(Session.SessionKeys.IsComplete, true);
                Session session = WorkflowUtility.SwitchToWF(prodId, SessionType, sessionKeyValueList); 
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
                logger.DebugFormat("END: {0}()", methodName);
            }

            #region rearrange code and disable code
            //logger.Debug("(PizzaKittingv)save start,"
            //    + " [prodId]: " + prodId);
            //FisException ex;
            //List<string> erpara = new List<string>();
            //string sessionKey = prodId;
            
            //try
            //{
            //    Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

            //    if (session == null)
            //    {
            //        erpara.Add(sessionKey);
            //        ex = new FisException("CHK021", erpara);
            //        //ex.logErr("", "", "", "", "83");
            //        //logger.Error(ex);
            //        throw ex;
            //    }

            //    session.AddValue(Session.SessionKeys.IsComplete, true);
            //    session.Exception = null;
            //    session.SwitchToWorkFlow();

            //    //check workflow exception
            //    if (session.Exception != null)
            //    {
            //        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
            //        {
            //            session.ResumeWorkFlow();
            //        }

            //        throw session.Exception;
            //    }
            //}
            //catch (FisException e)
            //{
            //    logger.Error(e.mErrmsg);
            //    throw;
            //}
            //catch (Exception e)
            //{
            //    logger.Error(e.Message);
            //    throw;
            //}
            //finally
            //{
            //    logger.Debug("(PizzaKitting)save end,"
            //       + " [prodId]: " + prodId);
            //}
            #endregion
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void Cancel(string prodId)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                if (WorkflowUtility.CancelSession(prodId, SessionType))
                {
                    logger.WarnFormat("Cancel seesion key:{0}", prodId);
                }
                else
                {
                    logger.WarnFormat("Cancel no seesion key:{0} found", prodId);
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
                logger.DebugFormat("END: {0}()", methodName);
            }

            #region rearrange code and disable code
            //logger.Debug("(PizzaKitting)Cancel start, [prodId]:" + prodId);
            //FisException ex;
            //List<string> erpara = new List<string>();
            //string sessionKey = prodId;

            //try
            //{
            //    Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

            //    if (session != null)
            //    {
            //        SessionManager.GetInstance.RemoveSession(session);
            //    }
            //}
            //catch (FisException e)
            //{
            //    logger.Error(e.mErrmsg);
            //    throw e;
            //}
            //catch (Exception e)
            //{
            //    logger.Error(e.Message);
            //    throw e;
            //}
            //finally
            //{
            //    logger.Debug("(PizzaKitting)Cancel end, [prodId]:" + prodId);
            //}
            #endregion
        }

        public ArrayList Print(string productID, string line, string code, string floor,
                                      string editor, string station, string customer, IList<PrintItem> printItems)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(productID:{1},line:{2},code:{3},floor:{4},editor:{5},station:{6},customer:{7})",
                                                methodName, productID, line, code, floor,
                                                editor, station, customer);
            try
            {
                Dictionary<string, object> sessionKeyValueList = new Dictionary<string, object>();
                sessionKeyValueList.Add(Session.SessionKeys.PrintItems, printItems);
                sessionKeyValueList.Add(Session.SessionKeys.IsComplete, true);
                Session currentSession=WorkflowUtility.SwitchToWF(productID, SessionType, sessionKeyValueList);

                string coolabel = (string)currentSession.GetValue("COOLabel");
                string wlabel = (string)currentSession.GetValue("WLabel");
                string clabel = (string)currentSession.GetValue("LanguageLabel");
                string cmessage = (string)currentSession.GetValue("LanguageMessage");
                string llabel = (string)currentSession.GetValue("LANOMLabel");
                string win8label = (string)currentSession.GetValue("Win8BoxLabel");

                IList<string> labelList = new List<string>();
                if (!string.IsNullOrEmpty(coolabel)) labelList.Add(coolabel);
                if (!string.IsNullOrEmpty(wlabel)) labelList.Add(wlabel);
                if (!string.IsNullOrEmpty(clabel)) labelList.Add(clabel);
                if (!string.IsNullOrEmpty(cmessage)) labelList.Add(cmessage);
                if (!string.IsNullOrEmpty(llabel)) labelList.Add(llabel);
                if (!string.IsNullOrEmpty(win8label)) labelList.Add(win8label);

                ArrayList retList = new ArrayList();
                IList<PrintItem> printlist = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                retList.Add(printlist);
                retList.Add(labelList);

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
                logger.DebugFormat("END: {0}()", methodName);
            }

            #region arrange code and disable code
            //logger.Debug("(PizzaKitting)Print start, ProductID:" + productID + " pdLine:" + line + " stationId:" + station + " editor:" + editor);

            //try
            //{
            //    Session currentSession = SessionManager.GetInstance.GetSession(productID, SessionType);

            //    if (currentSession == null)
            //    {
            //        FisException ex;
            //        List<string> erpara = new List<string>();
            //        erpara.Add(productID);
            //        ex = new FisException("CHK021", erpara);
            //        logger.Error(ex.Message, ex);
            //        throw ex;
            //    }
            //    else
            //    {
            //        currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
            //        currentSession.AddValue(Session.SessionKeys.IsComplete, true);
            //        currentSession.Exception = null;
            //        currentSession.SwitchToWorkFlow();
            //    }

            //    if (currentSession.Exception != null)
            //    {
            //        if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
            //        {
            //            currentSession.ResumeWorkFlow();
            //        }

            //        throw currentSession.Exception;
            //    }
				
            //    /*
            //    //Consumer Family / Commercial Family 使用不同的Template
            //    //Consumer Family 使用COO Label-2模板
            //    //Commercial Family 使用COO Label模板

            //    Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
            //    IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            //    Model curModel = modelRep.Find(curProduct.Model);
            //    string family = curModel.FamilyName;
            //    string labelTemp = "COO Label";
            //    //下列Family 为Consumer Family:
            //    if (family == "HARBOUR 1.0" || family == "HARBOUR 1.1" || family == "ST133I 1.0" || family == "ST133I 1.1"
            //        || family == "ST133I 1.2" || family == "ST145A 1.0" || family == "ST145A 1.1" || family == "ST145A 1.2"
            //        || family == "ST145I 1.0" || family == "ST145I 1.1" || family == "ST145I 1.2" || family == "ROMEO 1.0"
            //        || family == "ROMEO 1.1" || family == "ROMEO 1.2" || family == "ROMEO 2.0" || family == "ZIDANE 1.0"
            //        || family == "ZIDANE 1.1" || family == "ZIDANE 1.2" || family == "ZIDANE 2.0" || family == "ZIDANE 2.1"
            //        || family == "MURRAY 1.1" || family == "MURRAY 1.2" || family == "MURRAY 1.2" || family == "JIXI 1.0"
            //        || family == "JIXI 2.0" || family == "JIXI 2.1" || family == "JIXI 2.2" || family == "JACKMAND 1.0"
            //        || family == "JACKMANU 1.0" || family == "JACKMAND 1.1" || family == "JACKMANU 1.1" || family == "JACKMAND 1.2"
            //        || family == "JACKMANU 1.2" || family == "PARKERU 1.0" || family == "PARKERD 1.0" || family == "PARKERU 1.1"
            //        || family == "PARKERD 1.1" || family == "PARKERU 1.2" || family == "PARKERD 1.2" || family == "KITTY 1.0"
            //        || family == "HELLO 1.0" || family == "VUITTON 1.0" || family == "LAUREN 1.0" || family == "VUITTON 1.1"
            //        || family == "LAUREN 1.1")
            //    {
            //        labelTemp = "COO Label-2";
            //    }
            //    else if(family == "BANDIT 1.0" ){
            //        labelTemp = "COO Label-3";
            //    }

            //    ArrayList retList = new ArrayList();
            //    IList<PrintItem> printlist = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

            //    retList.Add(printlist);
            //    retList.Add(labelTemp);
            //    */
				
            //    string coolabel = (string)currentSession.GetValue("COOLabel");
            //    string wlabel = (string)currentSession.GetValue("WLabel");
            //    string clabel = (string)currentSession.GetValue("LanguageLabel");
            //    string cmessage = (string)currentSession.GetValue("LanguageMessage");
            //    string llabel = (string)currentSession.GetValue("LANOMLabel");
            //    string win8label = (string)currentSession.GetValue("Win8BoxLabel");
				
            //    IList<string> labelList = new List<string>();
            //    if (!string.IsNullOrEmpty(coolabel)) labelList.Add(coolabel);
            //    if (!string.IsNullOrEmpty(wlabel)) labelList.Add(wlabel);
            //    if (!string.IsNullOrEmpty(clabel)) labelList.Add(clabel);
            //    if (!string.IsNullOrEmpty(cmessage)) labelList.Add(cmessage);
            //    if (!string.IsNullOrEmpty(llabel)) labelList.Add(llabel);
            //    if (!string.IsNullOrEmpty(win8label)) labelList.Add(win8label);
				
            //    ArrayList retList = new ArrayList();
            //    IList<PrintItem> printlist = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

            //    retList.Add(printlist);
            //    retList.Add(labelList);

            //    return retList;
            //}
            //catch (FisException e)
            //{
            //    logger.Error(e.mErrmsg, e);
            //    throw new Exception(e.mErrmsg);
            //}
            //catch (Exception e)
            //{
            //    logger.Error(e.Message, e);
            //    throw new SystemException(e.Message);
            //}
            //finally
            //{
            //    logger.Debug("(PizzaKitting)Print end, ProductID:" + productID);
            //}
            #endregion

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSN"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList ReprintPizzaKitting(string customerSN, string reason, IList<PrintItem> printItems, string line, string editor, string station, string customer)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(customerSN:{1},reason:{2},line:{3},editor:{4},station:{5},customer:{6})",
                                                methodName, customerSN, reason,line, editor, station, customer);
            try
            {

                string wfName = "ReprintPizzaKitting.xoml";
                string wfRule =null;               
                string sessionKey=null;
                Dictionary<string, object> sessionKeyValueList = new Dictionary<string, object>();
                ArrayList retList = new ArrayList();

                var currentProduct = CommonImpl.GetProductByInput(customerSN, CommonImpl.InputTypeEnum.CustSN);
                sessionKey = currentProduct.ProId;

                var repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();

                IList<ProductLog> logList = repository.GetProductLogs(currentProduct.ProId,"PKOK");
                if (logList.Count == 0)
                {
                   throw new FisException("CHK860", new List<string>{sessionKey});                    
                }

                sessionKeyValueList.Add(Session.SessionKeys.Product, sessionKey);
                sessionKeyValueList.Add(Session.SessionKeys.IsComplete, false);
                 //==============================================
                sessionKeyValueList.Add(Session.SessionKeys.PrintItems, printItems);
                sessionKeyValueList.Add(Session.SessionKeys.LineCode, "PAK");
                sessionKeyValueList.Add(Session.SessionKeys.PrintLogBegNo, currentProduct.ProId);
                sessionKeyValueList.Add(Session.SessionKeys.PrintLogEndNo, currentProduct.ProId);
                sessionKeyValueList.Add(Session.SessionKeys.PrintLogName, "PizzaKitting");
                sessionKeyValueList.Add(Session.SessionKeys.PrintLogDescr, "");
                sessionKeyValueList.Add(Session.SessionKeys.Reason, reason);                
                //=====================================================
                Session currentSession= WorkflowUtility.InvokeWF(sessionKey,station,line,customer,editor,SessionType,wfName,wfRule,sessionKeyValueList);
                 
                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                retList.Add(printList);
                string coolabel = (string)currentSession.GetValue("COOLabel")??"";
                retList.Add(coolabel);               
                
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
                logger.DebugFormat("END: {0}()", methodName);
            }

            #region arrange and disable code 
            //logger.Debug("(ReprintPizzaKitting)ReprintConfigurationLabel start, customerSN:" + customerSN
            //              + "editor:" + editor + "station:" + station + "customer:" + customer);

            //try
            //{

            //    var currentProduct = CommonImpl.GetProductByInput(customerSN, CommonImpl.InputTypeEnum.CustSN);
            //    string sessionKey = currentProduct.ProId;

            //    var repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();

            //    IList<ProductLog> logList = repository.GetProductLogs(currentProduct.ProId,"PKOK");
            //    if (logList.Count == 0)
            //    {
            //        FisException ex;
            //        List<string> erpara = new List<string>();
            //        erpara.Add(sessionKey);
            //        ex = new FisException("CHK860", erpara);
            //        throw ex;
            //    }

            //    Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

            //    if (currentSession == null)
            //    {
            //        currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

            //        Dictionary<string, object> wfArguments = new Dictionary<string, object>();
            //        wfArguments.Add("Key", sessionKey);
            //        wfArguments.Add("Station", station);
            //        wfArguments.Add("CurrentFlowSession", currentSession);
            //        wfArguments.Add("Editor", editor);
            //        wfArguments.Add("PdLine", line);
            //        wfArguments.Add("Customer", customer);
            //        wfArguments.Add("SessionType", SessionType);

            //        string wfName, rlName;
            //        RouteManagementUtils.GetWorkflow(station, "ReprintPizzaKitting.xoml", null, out wfName, out rlName);
            //        //RouteManagementUtils.GetWorkflow(station, "104KPPrint.xoml", null, out wfName, out rlName);
            //        WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

            //        currentSession.AddValue(Session.SessionKeys.Product, sessionKey);
            //        currentSession.AddValue(Session.SessionKeys.IsComplete, false);
            //        currentSession.SetInstance(instance);

            //        //==============================================
            //        currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
            //        currentSession.AddValue(Session.SessionKeys.LineCode, "PAK");
            //        currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.ProId);
            //        currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.ProId);
            //        currentSession.AddValue(Session.SessionKeys.PrintLogName, "PizzaKitting");
            //        currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "");
            //        currentSession.AddValue(Session.SessionKeys.Reason, reason);
            //        //==============================================

            //        if (!SessionManager.GetInstance.AddSession(currentSession))
            //        {
            //            currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
            //            FisException ex;
            //            List<string> erpara = new List<string>();
            //            erpara.Add(sessionKey);
            //            ex = new FisException("CHK020", erpara);
            //            throw ex;
            //        }

            //        currentSession.WorkflowInstance.Start();
            //        currentSession.SetHostWaitOne();
            //    }
            //    else
            //    {
            //        FisException ex;
            //        List<string> erpara = new List<string>();
            //        erpara.Add(sessionKey);
            //        ex = new FisException("CHK020", erpara);
            //        throw ex;
            //    }


            //    if (currentSession.Exception != null)
            //    {
            //        if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
            //        {
            //            currentSession.ResumeWorkFlow();
            //        }

            //        throw currentSession.Exception;
            //    }
                
            //    //===============================================================================
            //    //Get infomation
            //    ArrayList retList = new ArrayList();
            //    //Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);

            //    //IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            //    //Model curModel = modelRep.Find(curProduct.Model);
            //    //string family = curModel.FamilyName;
            //    //string labelTemp = "COO Label";
            //    ////下列Family 为Consumer Family:
            //    //if (family == "HARBOUR 1.0" || family == "HARBOUR 1.1" || family == "ST133I 1.0" || family == "ST133I 1.1"
            //    //    || family == "ST133I 1.2" || family == "ST145A 1.0" || family == "ST145A 1.1" || family == "ST145A 1.2"
            //    //    || family == "ST145I 1.0" || family == "ST145I 1.1" || family == "ST145I 1.2" || family == "ROMEO 1.0"
            //    //    || family == "ROMEO 1.1" || family == "ROMEO 1.2" || family == "ROMEO 2.0" || family == "ZIDANE 1.0"
            //    //    || family == "ZIDANE 1.1" || family == "ZIDANE 1.2" || family == "ZIDANE 2.0" || family == "ZIDANE 2.1"
            //    //    || family == "MURRAY 1.1" || family == "MURRAY 1.2" || family == "MURRAY 1.2" || family == "JIXI 1.0"
            //    //    || family == "JIXI 2.0" || family == "JIXI 2.1" || family == "JIXI 2.2" || family == "JACKMAND 1.0"
            //    //    || family == "JACKMANU 1.0" || family == "JACKMAND 1.1" || family == "JACKMANU 1.1" || family == "JACKMAND 1.2"
            //    //    || family == "JACKMANU 1.2" || family == "PARKERU 1.0" || family == "PARKERD 1.0" || family == "PARKERU 1.1"
            //    //    || family == "PARKERD 1.1" || family == "PARKERU 1.2" || family == "PARKERD 1.2" || family == "KITTY 1.0"
            //    //    || family == "HELLO 1.0" || family == "VUITTON 1.0" || family == "LAUREN 1.0" || family == "VUITTON 1.1"
            //    //    || family == "LAUREN 1.1")
            //    //{
            //    //    labelTemp = "COO Label-2";
            //    //}
            //    //else if (family == "BANDIT 1.0")
            //    //{
            //    //    labelTemp = "COO Label-3";
            //    //}

            //    IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

            //    retList.Add(printList);
            //    string coolabel = (string)currentSession.GetValue("COOLabel")??"";
            //    retList.Add(coolabel);
            //    //retList.Add(labelTemp);

            //    //===============================================================================
                
            //    return retList;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //finally
            //{
            //    logger.Debug("(IPrintContentWarranty)ReprintPizzaKitting end, customerSN:" + customerSN);
            //}
            #endregion
        }


        public ArrayList InputPizzaCheckSNforSorting(string custSN, string line, string curStation,
                                                                 string editor, string station, string customer)
        {
            logger.Debug("(InputPizzaCheckSNforSorting)InputSN start, custSn:" + custSN
                + "Station:" + curStation);

            try
            {
                ArrayList retLst = new ArrayList();

                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (currentProduct == null)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(custSN);
                    throw new FisException("SFC002", errpara);
                }
                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                //用ProductID启动工作流，将Product放入工作流中
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", curStation);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", currentProduct.Status.Line.Trim());
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);
                    currentSession.AddValue(Session.SessionKeys.ReturnStation, station);
                    currentSession.AddValue("ForceNWCPreStation", currentProduct.Status.StationId.Trim());
                    //Session.SessionKeys.ReturnStation
                    //Add by Benson for Mantis 0001633
                    IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    IList<string> sttingLst = new List<string>();
                    sttingLst = ipartRepository.GetValueFromSysSettingByName("BatteryCtCheck");
                    if (sttingLst.Count > 0 && sttingLst[0].ToString().ToUpper().Trim() == "Y")
                    { currentSession.AddValue("EnableBatteryCtCheck", true); }
                    else
                    { currentSession.AddValue("EnableBatteryCtCheck", false); }
                    IList<string> sttingLst2 = new List<string>(); //OnlyCheckOneBattery
                    sttingLst2 = ipartRepository.GetValueFromSysSettingByName("OnlyCheckOneBattery");
                    if (sttingLst2.Count > 0 && sttingLst2[0].ToString().ToUpper().Trim() == "Y")
                    { currentSession.AddValue("OnlyCheckOneBattery", true); }
                    else
                    { currentSession.AddValue("OnlyCheckOneBattery", false); }

                    //Add by Benson for Mantis 0001633 a


                    string wfName, rlName;

                    RouteManagementUtils.GetWorkflow(station, "PAKReviewSorting.xoml", "PAKReviewSorting.rules", out wfName, out rlName);

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

                //============================================================================

                //get product data for UI
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                DataModel.ProductInfo prodInfo = curProduct.ToProductInfo();
                retLst.Add(prodInfo); // retLst idx 0
                IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, SessionType);
                retLst.Add(bomItemList);
                return retLst;

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
                logger.Debug("(InputPizzaCheckSNforSorting)InputSN end,  custSn:" + custSN);
            }

        }

        public List<string> UploadSnForSorting(List<string> snList,string station,string editor,string line,string customer)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(snList:{1},station:{2},editor:{3},line:{4},customer:{5})", 
                                                        methodName,string.Join(",",snList.ToArray()),station,editor,line,customer);
            try
            {
                List<string> rst = new List<string>();
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                string preStation = "";
                int success = 0;
                int fail = 0;
                CommonImpl com = new CommonImpl();
                List<string> failMsg = new List<string>();
                List<IMES.FisObject.FA.Product.IProduct> prdList = new List<IMES.FisObject.FA.Product.IProduct>();
                IList<IMES.DataModel.ConstValueTypeInfo> lstConst = partRep.GetConstValueTypeList("AllowSortingStation");

                foreach (string sn in snList)
                {
                    IMES.FisObject.FA.Product.IProduct product = productRepository.GetProductByIdOrSn(sn);
                    if (product == null)
                    { throw new FisException(sn + " is not correct ProductID or CustomerSN!!"); }
                    if (!lstConst.Any(x => x.value.Equals(product.Status.StationId)))
                    {
                        throw new FisException(sn + " has to do pizzakitting before sorting!!");
                    }
                    prdList.Add(product);

                }


                // foreach (string sn in snList)
                foreach (IMES.FisObject.FA.Product.IProduct prdObj in prdList)
                {
                    try
                    {
                        //    IProduct product = productRepository.GetProductByIdOrSn(sn);
                        if (prdObj != null)
                        {
                            ForceNWCInfo cond = new ForceNWCInfo();
                            cond.productID = prdObj.ProId;
                            preStation = prdObj.Status.StationId;
                            if (partRep.CheckExistForceNWC(cond))
                            {
                                partRep.UpdateForceNWCByProductID(station, preStation, prdObj.ProId);
                            }
                            else
                            {
                                ForceNWCInfo newinfo = new ForceNWCInfo();
                                newinfo.editor = editor;
                                newinfo.forceNWC = station;
                                newinfo.preStation = preStation;
                                newinfo.productID = prdObj.ProId;
                                partRep.InsertForceNWC(newinfo);

                            }
                            success++;
                        }
                        else
                        {
                            rst.Add(prdObj.CUSTSN + " Fail: Product not exist");
                            fail++;
                        }
                    }
                    catch (Exception e)
                    {
                        failMsg.Add(prdObj.CUSTSN + " error:" + e.Message);
                    }
                }
                rst.AddRange(failMsg.ToArray());


                rst.Insert(0, "Success: " + success.ToString() + " sn");
                if (fail > 0)
                { rst.Insert(1, "Fail: " + fail.ToString() + " sn"); }

                return rst;  

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
                logger.DebugFormat("END: {0}()", methodName);
            }
                
        }
        #endregion
    

        #region "methods do not interact with the running workflow"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IList<LightBomInfo> getBomByCode(string code)
        {
            logger.Debug("(PizzaKitting)getBomByCode Start[code]:" + code);
            try
            {

                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IList<LightBomInfo> retLst = productRepository.GetWipBufferInfoListByKittingCode(code);
                return retLst;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg,e);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message,e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PizzaKitting)getBomByCode End, "
                   + " [code]:" + code);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public IList<LightBomInfo> getBomByModel(string model, out string code)
        {
            logger.Debug("(PizzaKitting)getBomByModel Start [model]:" + model);

            try
            {
                IList<LightBomInfo> retLst = new List<LightBomInfo>();

                //判断系统中是否存在该Model
                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model modelObject = modelRepository.Find(model);
                if (modelObject == null)
                {
                    throw new FisException("CHK038", new string[] { model });
                }
                else
                {
                    //判断该Model是否维护Kitting Code
                    string ret = modelObject.GetAttribute("DM2");

                    logger.Debug("(PizzaKitting)Maintain Kitting Code[ret]:" + ret);
                    if (ret == null || ret.Trim() == string.Empty)
                    {
                        throw new FisException("CHK113", new string[] { model });
                    }
                    else
                    {
                        //根据model获取Kitting Code
                        code = modelRepository.GetKittingCodeByModel(model);
                        if (code == null || code.Trim() == string.Empty)
                        {
                            throw new FisException("CHK113", new string[] { model });
                        }
                        else
                        {
                            //判断查询得到的Kitting Code 是否在Kitting 表中存在
                            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                            int count = productRepository.GetCountOfKittingCodeByCode(code);
                            if (count == 0)
                            {
                                throw new FisException("CHK114", new string[] { model });
                            }
                            else
                            {
                                //获取bom
                                retLst = productRepository.GetWipBufferInfoListByKittingCodeAndModel(code, model);
                            }
                        }
                    }
                }

                return retLst;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg,e);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message,e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PizzaKitting)getBomByModel End[model]:" + model);

            }
        }

       
        #endregion
    }
}

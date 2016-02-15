// INVENTEC corporation (c)2010 all rights reserved. 
// Description: PO Data  bll
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-31   Yuan XiaoWei                 create
// 2011-03-16   Lucy Liu                     Modify:增加删除DN的功能
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Route;

//[assembly: log4net.Config.DOMConfigurator(Watch = true)]
namespace IMES.Station.Implementation
{
    /// <summary>
    /// 上传，修改 Delivery信息的BLL实现类，实现了IPOData接口
    /// </summary>
    public class POData : MarshalByRefObject, IPOData
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDeliveryRepository currentDNRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

        private const Session.SessionType currentSessionType = Session.SessionType.Common;
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IPOData Members

        IList<DNForUI> IPOData.UploadDNFile(string uploadID, string editor)
        {
           return currentDNRepository.UploadPOData(uploadID, editor);
        }

        IList<DNForUI> IPOData.UpdateDNFile(string uploadID, string editor)
        {
            return currentDNRepository.UpdatePOData(uploadID, editor);
        }


        /// <summary>
        /// 修改一条选中的DN
        /// </summary>
        /// <param name="UpdateDN"></param>
        /// <param name="editor"></param>
        void IPOData.ModifyDN(DNUpdateCondition UpdateDN, string editor)
        {
            logger.Debug("ModifyDN start, DeliveryNo:" + UpdateDN.DeliveryNo + " , editor:" + editor);

            try
            {
                currentDNRepository.UpdateDNByCondition(UpdateDN, editor);
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
                logger.Debug("ModifyDN end, DeliveryNo:" + UpdateDN.DeliveryNo + " , editor:" + editor);
            }
        }

        /// <summary>
        /// 修改一条选中的DeliveryInfo信息
        /// </summary>
        /// <param name="deliverInfoID"></param>
        /// <param name="infoValue"></param>
        /// <param name="editor"></param>
        void IPOData.ModifyDNInfo(int deliverInfoID, string infoValue, string editor)
        {
            logger.Debug("ModifyDNInfo start, deliverInfoID:" + deliverInfoID + " , InfoValue:" + infoValue + " , editor:" + editor);

            try
            {
                currentDNRepository.UpdateDeliverInfoByID(deliverInfoID, infoValue, editor);
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
                logger.Debug("ModifyDNInfo end, deliverInfoID:" + deliverInfoID + " , InfoValue:" + infoValue + " , editor:" + editor);
            }
        }

        /// <summary>
        /// 修改一条选中的DNPallet信息
        /// </summary>
        /// <param name="deliveryPalletID"></param>
        /// <param name="deliveryQty"></param>
        /// <param name="editor"></param>
        void IPOData.ModifyDNPallet(int deliveryPalletID, short deliveryQty, string editor)
        {
            logger.Debug("ModifyDNPallet start, deliveryPalletID:" + deliveryPalletID + " , deliveryQty:" + deliveryQty + " , editor:" + editor);

            try
            {
                currentDNRepository.UpdateDeliverQty(deliveryPalletID, deliveryQty, editor);
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
                logger.Debug("ModifyDNPallet end, deliveryPalletID:" + deliveryPalletID + " , deliveryQty:" + deliveryQty + " , editor:" + editor);
            }

        }

        /// <summary>
        /// 根据查询条件获取符合条件的Delivery列表
        /// </summary>
        /// <param name="MyCondition"></param>
        /// <returns></returns>
        IList<DNForUI> IPOData.getDNList(DNQueryCondition MyCondition)
        {
            logger.Debug("getDNList start, DeliveryNo:" + MyCondition.DeliveryNo + " , Model:" + MyCondition.Model + " , PONo:" + MyCondition.PONo + " , ShipDateFrom:" + MyCondition.ShipDateFrom + " , ShipDateTo:" + MyCondition.ShipDateTo);

            try
            {
                int totalLength =601;
                IList<DNForUI> result = currentDNRepository.GetDNListByCondition(MyCondition, out totalLength);
                if (result != null && result.Count > 600)
                {
                    FisException fe = new FisException("CHK110", new string[] {});
                    throw fe;
                }
                return result;
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
                logger.Debug("getDNList end, DeliveryNo:" + MyCondition.DeliveryNo + " , Model:" + MyCondition.Model + " , PONo:" + MyCondition.PONo + " , ShipDateFrom:" + MyCondition.ShipDateFrom + " , ShipDateTo:" + MyCondition.ShipDateTo);
            }

        }

        /// <summary>
        /// 根据DN获取DNInfoForUI列表
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<DNInfoForUI> IPOData.getDNInfoList(string dn)
        {
            logger.Debug("getDNInfoList start, DeliveryNo:" + dn);

            try
            {
                return currentDNRepository.GetDNInfoList(dn);
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
                logger.Debug("getDNInfoList end, DeliveryNo:" + dn);
            }

        }

        /// <summary>
        /// 根据DN获取Pallet列表
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<DNPalletQty> IPOData.getPalletList(string dn)
        {
            logger.Debug("getPalletList start, DeliveryNo:" + dn);

            try
            {
                return currentDNRepository.GetPalletList(dn);
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
                logger.Debug("getPalletList end, DeliveryNo:" + dn);
            }

        }

        /// <summary>
        /// 保存当前DN信息
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="UpdateDN"></param>
        /// <param name="deliverInfoID"></param>
        /// <param name="infoValue"></param>
        /// <param name="deliveryPalletID"></param>
        /// <param name="deliveryQty"></param>
        /// <param name="editor"></param>
        void IPOData.Save(string deliveryNo,DNUpdateCondition UpdateDN, int deliverInfoID, string infoValue, int deliveryPalletID, short deliveryQty, string editor)
        {
            logger.Debug("Save start, DeliveryNo:" + deliveryNo);

            try
            {
                IUnitOfWork dnWork = new UnitOfWork();
                if (UpdateDN != null)
                {
                    currentDNRepository.UpdateDNByConditionDefered(dnWork, UpdateDN, editor);
                }
                if (deliverInfoID > -1)
                {
                    currentDNRepository.UpdateDeliverInfoByIDDefered(dnWork, deliverInfoID, infoValue, editor);
                }
                if (deliveryPalletID > -1)
                {
                    currentDNRepository.UpdateDeliverQtyDefered(dnWork, deliveryPalletID, deliveryQty, editor);
                }

                dnWork.Commit();
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
                logger.Debug("Save end, DeliveryNo:" + deliveryNo);
            }
        }

        #endregion

        #region IPOData Members


        public string GetPAKConnectionString()
        {
           return currentDNRepository.GetPAKConnectionString();
        }


        /// <summary>
        /// 根据DN删除Delivery,DeliveryInfo,Delivery_Pallet表的相关记录
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        public void deleteDN(string dn, string line, string editor, string station, string customer)
        {
            logger.Debug("deleteDN start, DeliveryNo:" + dn);

           
            FisException ex;
            List<string> erpara = new List<string>();


            try
            {
              

                string sessionKey = dn;


                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, station, line, customer);


                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "056POData.xoml", "056POData.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.DeliveryNo , dn);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
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
                logger.Debug("deleteDN end, DeliveryNo:" + dn);
            }
          
        }
        #endregion
    }
}

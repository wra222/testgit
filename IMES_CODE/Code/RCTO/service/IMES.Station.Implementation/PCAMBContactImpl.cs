/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: OQCOutputImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-24   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using log4net;
using IMES.Route;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBChangeLog;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IOQCOutput接口的实现类
    /// </summary>
    public class PCAMBContactImpl : MarshalByRefObject, PCAMBContact
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region PCAMBContact members

        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="NewMB">New MB</param>
        /// <param name="OldMB">Old MB</param>
        /// <param name="editor">operator</param>
        /// <returns>prestation</returns>
        public string CheckMBandSave(string pdLine, string NewMB, string OldMB, string editor, string stationId, string customer)
        {
            logger.Debug("(CheckMBandSaveImpl)CheckMBandSave start, [pdLine]:" + pdLine
                + " [NewMB]: " + NewMB
                + " [OldMB]: " + OldMB
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string retvalue = "sussend";

            //string checkContactType = "OLD MB";
            string checkNewType = "VGA";
            //string sessionKey = NewMB;
            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IMBChangeLogRepository mbChangeLogRep = RepositoryFactory.GetInstance().GetRepository<IMBChangeLogRepository, MBChangeLog>();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IProduct newMbProduct = null;
            IProduct oldMbProduct = null;
            try
            {

               /* string[] checkContactValue = new string[2];
                checkContactValue[0] = NewMB;
                checkContactValue[1] = OldMB;
                IList<MBInfo> checkContactReturn = mbRepository.GetMbInfoListByInfoTypeAndPcbNoList(checkContactType, checkContactValue);*/

                MBChangeLog CheckItem = new MBChangeLog();
                CheckItem.Sn = OldMB;
                CheckItem.NewSn = NewMB;
                IList<MBChangeLog> checkContactReturn = mbChangeLogRep.GetMBChangeLogs(CheckItem);
                if (checkContactReturn.Count != 0)
                {
                    erpara.Add("This MB has been Contacted");
                    ex = new FisException("CHK217", erpara);
                    throw ex;
                }
                newMbProduct = productRepository.Find(NewMB);
                int NewMBCount = productRepository.GetProductInfoCountByInfoValue(checkNewType, NewMB);
                if (newMbProduct != null || NewMBCount != 0)
                {
                    erpara.Add("This NEW MB has been Combined PC");
                    ex = new FisException("CHK219", erpara);
                    throw ex;
                }

                oldMbProduct = productRepository.Find(OldMB);
                int OldMBCount = productRepository.GetProductInfoCountByInfoValue(checkNewType, OldMB);
                if (oldMbProduct != null || OldMBCount != 0)
                {
                    erpara.Add("This Old MB has been Combined PC");
                    ex = new FisException("CHK218", erpara);
                    throw ex;
                }
                //IMBRepository mbChangeLog = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                MBChangeLog item = new MBChangeLog(OldMB, NewMB, "客退板", editor, DateTime.Now);
                //item.
                IUnitOfWork work = new UnitOfWork();
                mbChangeLogRep.Add(item, work);
                work.Commit();
                return retvalue;

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
                logger.Debug("(CheckMBandSaveImpl)InputProdId end, [pdLine]:" + pdLine
                    + " [prodId]: " + NewMB
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string prodId)
        {
            logger.Debug("(CheckMBandSaveImpl)Cancel start, [prodId]:" + prodId);
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                logger.Debug("(CheckMBandSaveImpl)Cancel end, [prodId]:" + prodId);
            }
        }

        #endregion
    }
}

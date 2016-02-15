/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for UpdateShipDate Page            
 * CI-MES12-SPEC-PAK Update Ship Date.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc207003              Create
 * Known issues:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for UpdateShipDate.
    /// </summary>
    public class _UpdateShipDate : MarshalByRefObject, IUpdateShipDate
    { 
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeliveryRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        #region IUpdateShipDate Members
        /// <summary>
        /// 获取DN表相关信息
        /// </summary>
        /// <param name="dn">dn</param>
        public S_RowData_ShipDate GetDN(string dn)
        {
            logger.Debug("(_UpdateShipDate)GetDN start.");
            try
            {
                S_RowData_ShipDate ret = new S_RowData_ShipDate();
                Delivery reDelivery = new Delivery();
                reDelivery = currentRepository.Find(dn);
                if (null == reDelivery)
                {
                    ret.dn = "";
                    return ret;
                }
                ret.Qty = reDelivery.Qty.ToString();
                ret.ShipDate = reDelivery.ShipDate.ToString("yyyy - MM - dd");
                ret.Status = reDelivery.Status;
                ret.dn = reDelivery.DeliveryNo;
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
                logger.Debug("(_UpdateShipDate)GetDN end.");
            }
        }

        /// <summary>
        /// 更改Delivery表相关信息
        /// </summary>
        /// <param name="dn">dn</param>
        /// <param name="dnDate">dnDate</param>
        /// <param name="editor">editor</param>
        public void UpDN(string dn, string dnDate, string editor)
        { 
            logger.Debug("(_UpdateShipDate)UpDN start.");
            DNUpdateCondition myCondition = new DNUpdateCondition();
            myCondition.DeliveryNo = dn;
            myCondition.ShipDate = DateTime.Parse(dnDate);
            try
            {
                currentRepository.UpdateDNByCondition( myCondition,  editor);
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
                logger.Debug("(_UpdateShipDate)UpDN end.");
            }
        }

        #endregion
    }
}

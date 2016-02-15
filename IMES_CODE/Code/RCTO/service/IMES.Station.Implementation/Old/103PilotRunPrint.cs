/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: Pilot Run Print
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-04-21   Tong.Zhi-Yong     Create 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using log4net;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Misc;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPilotRunPrint接口的实现类
    /// </summary>
    class PilotRunPrintImpl : MarshalByRefObject, IPilotRunPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IPilotRunPrint members

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="customerSN">客户序列号</param>
        /// <param name="offsetX">offsetX</param>
        /// <param name="offsetY">offsetY</param>
        /// <param name="batchFile">批处理文件名称</param>
        /// <returns>返回要执行打印的bat字符串</returns>
        public string PrintPioltRun(string customerSN, int offsetX, int offsetY, string batchFile)
        {
            logger.Debug("(PilotRunPrintImpl)PrintPioltRun start, [customerSN]:" + customerSN
                + " [offsetX]: " + offsetX
                + " [offsetY]:" + offsetY
                + " [batchFile]:" + batchFile);

            IProduct product = null;
            IMiscRepository imr = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            IList<PilotRunPrintInfo> lstPilotRunPrintInfo = null;

            try
            {
                product = CommonImpl.GetProductByInput(customerSN, CommonImpl.InputTypeEnum.CustSN);
                lstPilotRunPrintInfo = imr.GetPilotRunPrintInfoListByModel(product.Model);

                if (lstPilotRunPrintInfo == null || lstPilotRunPrintInfo.Count == 0)
                {
                    FisException ex = new FisException("CHK124", new string[] { });
                    logger.Error(ex.mErrmsg);
                    throw ex;
                }

                return getMainBatInfo(customerSN, offsetX, offsetY, batchFile, lstPilotRunPrintInfo);
            }
            catch (FisException e)
            {
                if (string.Compare(e.mErrcode, "CHK079", true) == 0)
                {
                    FisException ex = new FisException("CHK123", new string[] { });
                    logger.Error(ex.mErrmsg);
                    throw ex;
                }
                else
                {
                    logger.Error(e.mErrmsg);
                    throw e;
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(PilotRunPrintImpl)PrintPioltRun end, [customerSN]:" + customerSN
                    + " [offsetX]: " + offsetX
                    + " [offsetY]:" + offsetY
                    + " [batchFile]:" + batchFile);
            }
        }

        private string getMainBatInfo(string customerSN, int offsetX, int offsetY, string batchFile, IList<PilotRunPrintInfo> lstPilotRunPrintInfo)
        {
            PilotRunPrintInfo firstElement = lstPilotRunPrintInfo[0];
            StringBuilder builder = new StringBuilder();
            PilotRunPrintInfo currentElement = null;
            int index = -1;

            builder.AppendLine("Set XL=" + offsetX);
            builder.AppendLine("Set YL=" + offsetY);
            builder.AppendLine("Set Family=" + (firstElement.Family == null ? string.Empty : firstElement.Family.Trim()));
            builder.AppendLine("Set MODEL=" + (firstElement.Model == null ? string.Empty : firstElement.Model.Trim()));
            builder.AppendLine("Set Build=" + (firstElement.Build == null ? string.Empty : firstElement.Build.Trim()));
            builder.AppendLine("Set SKU=" + (firstElement.SKU == null ? string.Empty : firstElement.SKU.Trim()));

            for (int i = 0; i < lstPilotRunPrintInfo.Count; i++)
            {
                currentElement = lstPilotRunPrintInfo[i];
                index = i + 1;
                builder.AppendLine("Set Tp" + index + "=" + (currentElement.Type == null ? string.Empty : currentElement.Type.Trim()));
                builder.AppendLine("Set Descr" + index + "=" + (currentElement.Descr == null ? string.Empty : currentElement.Descr.Trim()));
            }

            builder.AppendLine("Set Sno=" + customerSN);
            builder.AppendLine("CALL " + batchFile.Trim());

            return builder.ToString();
        }

        #endregion
    }
}

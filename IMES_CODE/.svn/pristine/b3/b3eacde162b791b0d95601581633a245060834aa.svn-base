/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Small Parts Upload Impl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-05-07   LuycLiu     Create 
 * 该实现文件不需要编写工作流，直接掉Repositroy就可以
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{

    public class SmallPartsUpload : MarshalByRefObject, ISmallPartsUpload
    {
       
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
     

        #region ISmallPartsUpload Members
        /// <summary>
        /// 查询smallparts upload信息
        /// </summary>
        /// <returns>smallparts upload信息列表</returns>
        public IList<SMALLPartsUploadInfo> Query()
        {
            logger.Debug("(SmallPartsUpload)GetWeight Start");
            try
            {
                IList<SMALLPartsUploadInfo> ret = null;
                IProductRepository rep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                ret = rep.QuerySmallPartsUploadInfo();
                return ret;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(SmallPartsUpload)GetWeight End");
            }
        }


        /// <summary>
        /// 保存至数据库
        /// </summary>
        /// <param name="list">smallparts upload信息列表</param>
        public void Save(IList<SMALLPartsUploadInfo> list)
        {
            logger.Debug("(SmallPartsUpload)GetWeight Start");
            try
            {
                IProductRepository rep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                rep.SaveSmallPartsUploadInfo(list);
                
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(SmallPartsUpload)GetWeight End");
            }
        }

        #endregion
    }
}
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service implement for RepairInfo Page
 *             
 * UI:CI-MES12-SPEC-FA-UI RepairInfo Query.docx
 * UC:CI-MES12-SPEC-FA-UC RepairInfo Query.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-30  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Globalization; 
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Defect;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.UnitOfWork;
using IMES.Station.Interface.StationIntf;
using log4net;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for RepairInfo.
    /// </summary>
    public class _RepairInfoImpl : MarshalByRefObject, IRepairInfo
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

        #region IRepairInfo Members
        
        /// <summary>
        /// 获取表格内容
        /// </summary>
        public DataTable GetRepairInfoList(IList<string> snList, IList<string> rsList, out IList<string> failSNList)
        {
            logger.Debug("(_RepairInfoImpl)GetRepairInfoList starts");
            try
            {
                IList<string> idList = new List<string>();
                failSNList = new List<string>();
                foreach (string sn in snList)
                {
                    IProduct p = productRepository.GetProductByCustomSn(sn);
                    if (p == null)
                    {
                        failSNList.Add(sn);
                    }
                    else
                    {
                        idList.Add(p.ProId);
                    }
                }
                return productRepository.GetProductRepairInfoListDataTable(idList, rsList);
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
                logger.Debug("(_RepairInfoImpl)GetRepairInfoList end");
            }
        }

        /// <summary>
        /// 获取表格内容
        /// </summary>
        public void EditRepairInfo(string productID, RepairInfo repInfo)
        {
            logger.Debug("(_RepairInfoImpl)EditRepairInfo starts");
            try
            {
                UnitOfWork uow = new UnitOfWork();
                productRepository.BackupProductRepairDefectInfoDefered(uow, repInfo.Identity, productID, repInfo.editor);
                repInfo.location = (repInfo.majorPart + "   ").Substring(0, 3)
                    + (repInfo.component + "  ").Substring(0, 2)
                    + repInfo.site.TrimEnd();
                RepairInfo cond = new RepairInfo();
                cond.Identity = repInfo.Identity;
                repInfo.Identity = int.MinValue;
                productRepository.UpdateProductRepairDefectInfoDefered(uow, repInfo, cond);
                uow.Commit();
                return;
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
                logger.Debug("(_RepairInfoImpl)EditRepairInfo end");
            }
        }

        /// <summary>
        /// 获取指定的维修记录
        /// </summary>
        public RepairInfo GetDefectInfo(int id)
        {
            logger.Debug("(_RepairInfoImpl)GetDefectInfo starts");
            try
            {
                RepairInfo cond = new RepairInfo();
                cond.Identity = id;
                IList<RepairInfo> diList = productRepository.GetProductRepairDefectInfo(cond);
                if (diList.Count == 1)
                {
                    return diList[0];
                }
                else
                {
                    return null;
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
                logger.Debug("(_RepairInfoImpl)GetDefectInfo end");
            }
        }

        public void UpdateProductRepair_DefectInfo_Mark(IList<int> mark_0, IList<int> mark_1, string editor)
        {
            logger.Debug("(_RepairInfoImpl)UpdateProductRepair_DefectInfo_Mark starts");
            try
            {
                productRepository.UpdateProductRepair_DefectInfo_Mark(mark_0, mark_1, editor);
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
                logger.Debug("(_RepairInfoImpl)UpdateProductRepair_DefectInfo_Mark end");
            }
        }

        //public void UpdateProductRepair_DefectInfo_MarkDefered(IList<int> mark_0, IList<int> mark_1, string editor)
        //{
        //    logger.Debug("(_RepairInfoImpl)UpdateProductRepair_DefectInfo_MarkDefered starts");
        //    IUnitOfWork uow = new UnitOfWork();
        //    try
        //    {
        //        productRepository.UpdateProductRepair_DefectInfo_MarkDefered(uow, mark_0, mark_1, editor);
        //    }
        //    catch (FisException e)
        //    {
        //        logger.Error(e.mErrmsg, e);
        //        throw new Exception(e.mErrmsg);
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e.Message, e);
        //        throw new SystemException(e.Message);
        //    }
        //    finally
        //    {
        //        uow.Commit();
        //        logger.Debug("(_RepairInfoImpl)UpdateProductRepair_DefectInfo_MarkDefered end");
        //    }
        //}


        #endregion
    }
}

/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:Service for APT maintain Page
 *             
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-18  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.DataModel;
using IMES.Infrastructure;

namespace IMES.Maintain.Implementation
{
    public class ActualProductionTimeManager : MarshalByRefObject, IActualProductionTime
    {

        #region IActualProductionTime Members
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

        public IList<ConstValueInfo> GetProductionCauseList()
        {
            logger.Debug("(ActualProductionTimeManager) GetProductionCauseList begins.");
            try
            {
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                ConstValueInfo cond = new ConstValueInfo();
                cond.type = "SMTCauseTable";
                return partRep.GetConstValueInfoList(cond);
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
                logger.Debug("(ActualProductionTimeManager) GetProductionCauseList ends.");
            }
        }

        public IList<SmttimeInfo> GetSMTTimeInfoList(DateTime date)
        {
            logger.Debug("(ActualProductionTimeManager) GetSMTTimeInfoList begins.");
            try
            {
                return mbRep.GetSMTTimeInfoList(date);
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
                logger.Debug("(ActualProductionTimeManager) GetSMTTimeInfoList ends.");
            }
        }

        public void AddSMTTimeInfo(SmttimeInfo item)
        {
            logger.Debug("(ActualProductionTimeManager) AddSMTTimeInfo begins.");
            try
            {
                mbRep.AddSMTTimeInfo(item);
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
                logger.Debug("(ActualProductionTimeManager) AddSMTTimeInfo ends.");
            }
        }

        public void UpdateSMTTimeInfo(SmttimeInfo cond, SmttimeInfo item)
        {
            logger.Debug("(ActualProductionTimeManager) UpdateSMTTimeInfo begins.");
            try
            {
                mbRep.UpdateSMTTimeInfo(item, cond);
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
                logger.Debug("(ActualProductionTimeManager) UpdateSMTTimeInfo ends.");
            }
        }

        public void DeleteSMTTimeInfo(SmttimeInfo cond)
        {
            logger.Debug("(ActualProductionTimeManager) DeleteSMTTimeInfo begins.");
            try
            {
                mbRep.DeleteSMTTimeInfo(cond);
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
                logger.Debug("(ActualProductionTimeManager) DeleteSMTTimeInfo ends.");
            }
        }

        public bool CheckExistSMTTimeInfo(SmttimeInfo cond)
        {
            logger.Debug("(ActualProductionTimeManager) CheckExistSMTTimeInfo begins.");
            try
            {
                return mbRep.CheckExistSMTTimeInfo(cond);
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
                logger.Debug("(ActualProductionTimeManager) CheckExistSMTTimeInfo ends.");
            }
        }

        public IList<SMTLineDef> GetSMTLineInfoListByLineList(IList<string> lineList)
        {
            logger.Debug("(ActualProductionTimeManager) GetSMTLineInfoListByLineList begins.");
            try
            {
                return mbRep.GetSMTLineInfoListByLineList(lineList);
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
                logger.Debug("(ActualProductionTimeManager) GetSMTLineInfoListByLineList ends.");
            }
        }

        public IList<DeptInfo> GetDeptInfoList()
        {
            logger.Debug("(ActualProductionTimeManager) GetDeptInfoList begins.");
            try
            {
                return mbRep.GetDeptInfoList();
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
                logger.Debug("(ActualProductionTimeManager) GetDeptInfoList ends.");
            }
        }
        #endregion
    }
}

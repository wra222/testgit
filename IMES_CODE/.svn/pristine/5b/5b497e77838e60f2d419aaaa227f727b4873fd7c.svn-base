using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.PCA.MB;
using log4net;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Line;
using IMES.FisObject.PCA.MBModel;
using System.Data;


namespace IMES.Maintain.Implementation
{
    public class SMTLineSpeedManager : MarshalByRefObject, SMTLineSpeed
    {

        #region SMTLineSpeed member
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IMBModelRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
        ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
        IMBRepository repMB = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

        public void AddSMTLine(SmtctInfo item)
        {
            try
            {
                mbRepository.AddSmtctInfo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<SmtctInfo> GetAllSMTLineItems(string line)
        {
            IList<SmtctInfo> dataLst = new List<SmtctInfo>();
            try
            {
                SmtctInfo cond = new SmtctInfo();
                cond.line = line;
                dataLst = mbRepository.GetSmtctInfoList(cond);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return dataLst;
        }

        public void RemoveSMTLine(SmtctInfo item)
        {
            try
            {
                mbRepository.DeleteSmtctInfo(item);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }

        public void UpdateLotSMTLine(SmtctInfo item, SmtctInfo cond)
        {
            try
            {
                mbRepository.UpdateSmtctInfo(item, cond);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }
        public IList<string> GetFamily(string family)
        {
            IList<string> lstfamily = new List<string>();
            try
            {
                lstfamily = repMB.GetFamilyListFromFamilyMb();
                
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return lstfamily;
        }

        public DataTable GetLine(string line)
        {
            DeptInfo condition =new DeptInfo();
           // condition.line = line;
            DataTable tmp ;                       
            try
            {
                tmp = repMB.GetRemarkAndLineFromDept(condition);           
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return tmp;
        }

        #endregion
    }
}

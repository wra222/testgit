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

namespace IMES.Maintain.Implementation
{
    public class LotSettingManager : MarshalByRefObject, LotSetting
    {

        #region LotSettingInfo member
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        IMBRepository MBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
        ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();

        public void AddLotSetting(IMES.DataModel.LotSettingInfo item)
        {
            try
            {
                MBRepository.InsertLotSettingInfo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.DataModel.LotSettingInfo> GetAllLotSettingItems()
        {
            IList<LotSettingInfo> dataLst = new List<LotSettingInfo>();
            try
            {
                LotSettingInfo cond = new LotSettingInfo();
                dataLst = MBRepository.GetLotSettingInfoList(cond);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return dataLst;
        }

        public void RemoveLotSetting(IMES.DataModel.LotSettingInfo item)
        {
            try
            {
                MBRepository.DeleteLotSettingInfo(item);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }

        public void UpdateLotSetting(LotSettingInfo item, LotSettingInfo cond)
        {
            try
            {
                MBRepository.UpdateLotSettingInfo(item, cond);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
        }
        public IList<string> GetLine(string stage)
        {
            IList<string> line = new List<string>();
            try
            {
                IList<LineInfo> lineInfoList = lineRepository.GetAllPdLineListByStage(stage);
                foreach (LineInfo ele in lineInfoList)
                {
                    line.Add(ele.line);
                } 
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return line;
        }

        #endregion
    }
}

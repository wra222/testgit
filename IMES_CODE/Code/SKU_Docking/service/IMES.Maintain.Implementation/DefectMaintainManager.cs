using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.Infrastructure.UnitOfWork;
namespace IMES.Maintain.Implementation
{
    public class DefectMaintainManager : MarshalByRefObject, IDefectMaintain
    {

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 根据type获取所有Defect数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<DefectCodeInfo> GetDefectCodeList(string type)
        {
            IList<DefectCodeInfo> retLst = null;
            try
            {
                IDefectRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
                retLst = itemRepository.GetDefectCodeList(type);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据type,defect获取defect数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defect"></param>
        /// <returns></returns>
        public string GetDefect(string type, string defect)
        {
            string retStr = "";
            try
            {
                IDefectRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
                retStr = itemRepository.GetDefect(type, defect);
                return retStr;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据type,defect删除一条数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defect"></param>
        public void DeleteDefectCode(string type, string defect)
        {
            try
            {
                IDefectRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
                int count = itemRepository.CheckExistsRecord(defect);
                if (count <= 0)
                {
                    //已经不存在具有相同的defectCode记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT119", erpara);
                    throw ex;
                }
                else
                {
                    IUnitOfWork unitWork = new UnitOfWork();
                    Defect defectInfo = itemRepository.Find(defect);
                    itemRepository.Remove(defectInfo, unitWork);
                    unitWork.Commit();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 更新一条defect数据
        /// </summary>
        /// <param name="dfc"></param>
        public void UpdateDefectCode(DefectCodeInfo dfc)
        {
            try
            {
                IDefectRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
                itemRepository.UpdateDefectCode(dfc);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 插入一条defect数据
        /// </summary>
        /// <param name="dfc"></param>
        public void InsertDefectCode(DefectCodeInfo dfc)
        {
            int count = 0;
            try
            {
                IDefectRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
                count = itemRepository.CheckExistsRecord(dfc.Defect);
                if (count <= 0)
                {
                    itemRepository.InsertDefectCode(dfc);
                }
                else
                {

                    //已经存在具有相同的defectCode记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT118", erpara);
                    throw ex;
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
                throw;
            }
        }

        /// <summary>
        /// 判断指定defectcode是否在数据库中
        /// </summary>
        /// <param name="Defect"></param>
        /// <returns></returns>
        public int CheckExistsRecord(string Defect)
        {
            int count = 0;
            try
            {
                IDefectRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
                count = itemRepository.CheckExistsRecord(Defect);
                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取所有defectcode数据
        /// </summary>
        /// <returns></returns>
        public IList<DefectCodeInfo> GetDefectCodeList()
        {
            IList<DefectCodeInfo> retLst = null;
            try
            {
                IDefectRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
                retLst = itemRepository.GetDefectCodeList();
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDefectCode(string defect)
        {
            try
            {
                IDefectRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
                int count = itemRepository.CheckExistsRecord(defect);
                if (count <= 0)
                {
                    //已经不存在具有相同的defectCode记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT119", erpara);
                    throw ex;
                }
                else
                {
                    itemRepository.DeleteDefectCode(defect);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

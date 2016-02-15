using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Maintain.Implementation
{
    public class FAITCNDefectCheckManager : MarshalByRefObject, IFAITCNDefectCheck
    {

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        /// <summary>
        /// 根据type获取所有Defect数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<FaItCnDefectCheckInfo> GetDefectCheckList()
        {
            IList<FaItCnDefectCheckInfo> retList = null;
            try
            {
                
                FaItCnDefectCheckInfo conf = new FaItCnDefectCheckInfo();
                retList = productRep.GetFaItCnDefectCheckInfoList(conf);
                return retList;
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
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteDefectCheck(int ID)
        {
            try
            {
                FaItCnDefectCheckInfo conItem = new FaItCnDefectCheckInfo();
                conItem.id = ID;
                productRep.DeleteFaItCnDefectCheckInfo(conItem);
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
        public void UpdateDefectCheck(FaItCnDefectCheckInfo defectItem)
        {
            try
            {
                IList<FaItCnDefectCheckInfo> defectList = new List<FaItCnDefectCheckInfo>();
                FaItCnDefectCheckInfo cond = new FaItCnDefectCheckInfo();
                cond.code = defectItem.code;
                
                bool repeatFlag = false;
                defectList = productRep.GetFaItCnDefectCheckInfoList(cond);
                foreach (var node in defectList)
                {
                    if (node.id != defectItem.id)
                    {
                        repeatFlag = true;
                        break;
                    }
                }

                if (repeatFlag)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT150", erpara);
                    throw ex;
                }

                FaItCnDefectCheckInfo conItem = new FaItCnDefectCheckInfo();
                conItem.id = defectItem.id;

                defectItem.id = int.MinValue;
                defectItem.udt = DateTime.Now;

                productRep.UpdateFaItCnDefectCheckInfo(defectItem,conItem);

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
        public string InsertDefectCheck(FaItCnDefectCheckInfo defectItem)
        {
            int count = 0;
            try
            {
                IList<FaItCnDefectCheckInfo> defectList = new List<FaItCnDefectCheckInfo>();
                FaItCnDefectCheckInfo cond = new FaItCnDefectCheckInfo();
                cond.code = defectItem.code;

                defectList = productRep.GetFaItCnDefectCheckInfoList(cond);

                if (defectList.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT150", erpara);
                    throw ex;
                }

                defectItem.cdt = DateTime.Now;
                defectItem.udt = DateTime.Now;

                productRep.InsertFaItCnDefectCheckInfo(defectItem);
                return Convert.ToString(defectItem.id);

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


        public IList<DefectCodeInfo> GetDefectCodeLst()
        {
            IList<DefectCodeInfo> retLst = null;
            try
            {
                IDefectRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
                retLst = itemRepository.GetDefectCodeLst();
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

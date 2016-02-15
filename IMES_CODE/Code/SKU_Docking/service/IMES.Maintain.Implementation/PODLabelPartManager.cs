using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.Common.Line;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;
using log4net;
namespace IMES.Maintain.Implementation
{
    public class PODLabelPartManager : MarshalByRefObject, IPODLabelPartMaintain
    {


        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// 根据partNo获取podlabelpart数据
        /// </summary>
        /// <param name="PartNo"></param>
        /// <returns></returns>
        public IList<PODLabelPartDef> GetPODLabelPartListByPartNo(string PartNo)
        {
            IList<PODLabelPartDef> partList = new List<PODLabelPartDef>();

            try
            {
                IPartRepository iplr = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                partList = iplr.GetPODLabelPartListByPartNo(PartNo);
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

            return partList;
        }

        /// <summary>
        /// 根据partno和family获取podlabel数据
        /// </summary>
        /// <param name="PartNo"></param>
        /// <param name="Family"></param>
        /// <returns></returns>
        public IList<PODLabelPartDef> GetListByPartNoAndFamily(string PartNo, string Family)
        {
            IList<PODLabelPartDef> partList = new List<PODLabelPartDef>();

            try
            {
                IPartRepository iplr = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                partList = iplr.GetListByPartNoAndFamily(PartNo, Family);
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

            return partList;
        }

        /// <summary>
        /// 获取所有podlabel数据
        /// </summary>
        /// <returns></returns>
        public IList<PODLabelPartDef> GetPODLabelPartList()
        {
            IList<PODLabelPartDef> partList = new List<PODLabelPartDef>();

            try
            {
                IPartRepository iplr = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                partList = iplr.GetPODLabelPartList();
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

            return partList;
        }

        /// <summary>
        /// 更新podlabel数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="PartNo"></param>
        public void UpdatePODLabelPart(PODLabelPartDef obj, string PartNo)
        {
            try
            {
                IPartRepository iplr = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                iplr.UpdatePODLabelPart(obj, PartNo);
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
        /// 添加podlabelpart数据
        /// </summary>
        /// <param name="obj"></param>
        public void AddPODLabelPart(PODLabelPartDef obj)
        {
            PODLabelPartDef PODLabel = obj;
            try
            {
                IPartRepository iplr = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                PODLabelPartDef item = new PODLabelPartDef();
                item.family = obj.family;
                item.partNo = obj.partNo;
                item.editor = obj.editor;
                item.cdt = obj.cdt;
                item.udt = obj.udt;
                iplr.AddPODLabelPart(item);
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
        /// 根据partNo删除podlabelpart数据
        /// </summary>
        /// <param name="PartNo"></param>
        public void DeletePODLabelPart(string PartNo)
        {
            try
            {
                IPartRepository iplr = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                iplr.DeletePODLabelPart(PartNo);

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
        /// 通过partNo和family修改podlabelpart数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="PartNo"></param>
        /// <param name="Family"></param>
        public void UpdatePODLabelPart(PODLabelPartDef obj, string PartNo, string Family)
        {
            IList<PODLabelPartDef> listPodPd = new List<PODLabelPartDef>();
            try
            {
                if (obj.partNo.Equals(PartNo))
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT075", erpara);
                    throw ex;
                }
                else
                {
                    listPodPd = this.GetPODLabelPartListByPartNo(obj.partNo);
                    if (listPodPd.Count > 0)
                    {
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("DMT075", erpara);
                        throw ex;
                    }
                    else
                    {
                        IList<PODLabelPartDef> listPodPd1 = new List<PODLabelPartDef>();
                        listPodPd1 = this.GetListByPartNoAndFamily(obj.partNo, obj.family);
                        if (listPodPd1 != null && listPodPd1.Count > 0)
                        {
                            PODLabelPartDef podGetLst = listPodPd1.First();
                            this.UpdatePODLabelPart(obj, podGetLst.partNo);
                        }
                        else
                        {
                            obj.cdt = DateTime.Now;
                            this.AddPODLabelPart(obj);
                        }
                    }
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


    }

}

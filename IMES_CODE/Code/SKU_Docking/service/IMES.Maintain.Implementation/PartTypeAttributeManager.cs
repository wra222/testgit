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
namespace IMES.Maintain.Implementation
{
    class PartTypeAttributeManager : MarshalByRefObject, IPartTypeAttribute
    {
        /// <summary>
        ///  取得PartType的所有记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<PartTypeDef></returns>
        public IList<PartTypeAttributeDef> GetPartTypeList()
        {

            IList<PartTypeAttributeDef> chepList = new List<PartTypeAttributeDef>();
            IList<PartTypeDef> listInfo = new List<PartTypeDef>();
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                listInfo = itemRepository.GetPartTypeDefList();
                foreach (PartTypeDef kitLocInfo in listInfo)
                {
                    PartTypeAttributeDef item = new PartTypeAttributeDef();
                    item.code = kitLocInfo.code;
                    item.index = kitLocInfo.indx;
                    item.description = kitLocInfo.description;
                    item.site = kitLocInfo.site;
                    item.cust = kitLocInfo.cust;
                    item.editor = kitLocInfo.editor;
                    item.id = kitLocInfo.id;
                    if (kitLocInfo.cdt == DateTime.MinValue)
                    {
                        item.cdt = "";
                    }
                    else
                    {
                        item.cdt = ((System.DateTime)kitLocInfo.cdt).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (kitLocInfo.udt == DateTime.MinValue)
                    {
                        item.udt = "";
                    }
                    else
                    {
                        item.udt = ((System.DateTime)kitLocInfo.udt).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    chepList.Add(item);
                }
                return chepList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  根据tp取得PartType的记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<PartTypeDef></returns>
        public IList<PartTypeAttributeDef> GetPartTypeListByTp(string tp) {
            IList<PartTypeAttributeDef> lstPartTypeDef = new List<PartTypeAttributeDef>();
            IList<PartTypeDef> listInfo = new List<PartTypeDef>();
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                listInfo = itemRepository.GetPartTypeListByTp(tp);
                foreach (PartTypeDef kitLocInfo in listInfo)
                {
                    PartTypeAttributeDef item = new PartTypeAttributeDef();
                    item.code = kitLocInfo.code;
                    item.index = kitLocInfo.indx;
                    item.description = kitLocInfo.description;
                    item.site = kitLocInfo.site;
                    item.cust = kitLocInfo.cust;
                    item.editor = kitLocInfo.editor;
                    item.id = kitLocInfo.id;
                    if (kitLocInfo.cdt == DateTime.MinValue)
                    {
                        item.cdt = "";
                    }
                    else
                    {
                        item.cdt = ((System.DateTime)kitLocInfo.cdt).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (kitLocInfo.udt == DateTime.MinValue)
                    {
                        item.udt = "";
                    }
                    else
                    {
                        item.udt = ((System.DateTime)kitLocInfo.udt).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    lstPartTypeDef.Add(item);
                }
                return lstPartTypeDef;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  取得一条PartType的记录数据
        /// </summary>
        /// <param name="?">String id</param>
        /// <returns>PartTypeDef</returns>
        public PartTypeAttributeDef GetPartTypeInfo(String id)
        {
            PartTypeAttributeDef partTypeDef = new PartTypeAttributeDef();
            return partTypeDef;
        }

        /// <summary>
        ///  取得所有tp的数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<string></returns>
        public IList<string> GetTPList()
        { 
            IList<string> lstTp = new List<string>(); 
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                lstTp = itemRepository.GetTPList();
                return lstTp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  取得TP = tp的code数据
        /// </summary>
        /// <param name="?">string tp</param>
        /// <returns>IList<string></returns>
        public IList<string> GetCodeListByTp(string tp)
        {
            IList<string> lstCode = new List<string>();
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                lstCode = itemRepository.GetCodeListByTp(tp);
                return lstCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 添加一条PartType的记录数据(Add)
        /// </summary>
        /// <param name="Object">PartTypeAttributeDef partTypeDef</param>
        public string AddPartType(PartTypeAttributeDef item)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                PartTypeDef partType = new PartTypeDef();
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                UnitOfWork uow = new UnitOfWork();

                IList<PartTypeDef> lstCode = itemRepository.GetPartTypeByCode(item.code);

                if (lstCode.Count != 0)
                {
                    //要添加的数据已经存在
                    ex = new FisException("DMT087", paraError);
                    throw ex;
                }
                partType.tp = item.tp;
                partType.code = item.code;
                partType.indx = item.index;
                partType.description = item.description;
                partType.site = item.site;
                partType.cust = item.cust;
                partType.editor = item.editor;
                partType.cdt = DateTime.Now;
                partType.udt = DateTime.Now;
                itemRepository.AddPartTypeDefered(uow, partType);
                uow.Commit();
                string id = "";
                foreach (PartTypeDef partTypeDef in itemRepository.GetPartTypeByCode(item.code))
                {
                    id = partTypeDef.id.ToString();
                }
                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 更新一条PartType的记录数据
        /// </summary>
        /// <param name="Object">PartTypeAttributeDef PartType,string id</param>
        public void UpdatePartType(PartTypeAttributeDef item, string id)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                UnitOfWork uow = new UnitOfWork();
                //当前信息是否存在
                PartTypeDef partType = itemRepository.GetPartTypeInfo(int.Parse(id));
                if (partType == null)
                {
                    //要更新的原始数据已被删除
                    ex = new FisException("DMT082", paraError);
                    throw ex;
                }
                IList<PartTypeDef> lstPartTypeDef = itemRepository.GetPartTypeByCode(item.code);
                foreach (PartTypeDef partTypeDef in lstPartTypeDef)
                {
                    //判断非当前记录和要更新的数据是否有重复
                    if (partTypeDef.id != int.Parse(id) & partTypeDef.code == item.code)
                    {
                        //要更新的数据已经存在于其他记录
                        ex = new FisException("DMT087", paraError);
                        throw ex;
                    }
                }
                partType.tp = item.tp;
                partType.code = item.code;
                partType.indx = item.index;
                partType.description = item.description;
                partType.site = item.site;
                partType.cust = item.cust;
                partType.editor = item.editor;
                partType.id = int.Parse(id);
                partType.cdt = DateTime.Now;
                partType.udt = DateTime.Now;
                itemRepository.UpdatePartTypeDefered(uow, partType);
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// "删除一条PartType的记录数据
        /// </summary>
        /// <param name="?">string id</param>
        public void DeletePartType(string id)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                UnitOfWork uow = new UnitOfWork();
                itemRepository.DeletePartTypeDefered(uow, int.Parse(id));
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

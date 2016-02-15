/*
 * issue
 * ITC-1361-0038  itc210012  2011-01-16
 */
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

using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;
namespace IMES.Maintain.Implementation
{
    public class FAFloatLocationManager : MarshalByRefObject, IFAFloatLocation
    {
        /// <summary>
        ///  取得KitLoc的所有记录数据
        /// </summary>
        /// <param name="?"></param>
         /// <returns>IList<FAFloatLocationDef> chepList</returns>
        public IList<FAFloatLocationDef> GetKitLocList()
        {
            IList<FAFloatLocationDef> chepList = new List<FAFloatLocationDef>();
            IList<FAFloatLocationInfo> listInfo = new List<FAFloatLocationInfo>();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                listInfo = itemRepository.GetFAFloatLocationList();
                foreach (FAFloatLocationInfo kitLocInfo in listInfo)
                {
                    FAFloatLocationDef item = new FAFloatLocationDef();
                    item.family = kitLocInfo.family;
                    item.partType = kitLocInfo.partType;
                    item.pdLine = kitLocInfo.pdLine;
                    item.location = kitLocInfo.location;
                    item.editor = kitLocInfo.editor;
                    item.id = kitLocInfo.id;
                    if (kitLocInfo.cdt == DateTime.MinValue)
                    {
                        item .cdt= "";
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
        ///  根据family取得KitLoc的记录数据
        /// </summary>
        /// <param name="?">string family</param>
        /// <returns>IList<FAFloatLocationDef> chepList</returns>
        public IList<FAFloatLocationDef> GetKitLocListByFamily(string family) {
            IList<FAFloatLocationDef> chepList = new List<FAFloatLocationDef>();
            IList<FAFloatLocationInfo> listInfo = new List<FAFloatLocationInfo>();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                listInfo = itemRepository.GetFAFloatLocListByFamily(family);
                foreach (FAFloatLocationInfo kitLocInfo in listInfo)
                {
                    FAFloatLocationDef item = new FAFloatLocationDef();
                    item.family = kitLocInfo.family;
                    item.partType = kitLocInfo.partType;
                    item.pdLine = kitLocInfo.pdLine;
                    item.location = kitLocInfo.location;
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
        ///  获取所有family记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<string> familyLst</returns>
        public IList<string> GetAllFamilys() {
            IList<string> familyLst = new List<string>();
            IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
            try
            {
                IList<Family> familyObjs = itemRepository.GetFamilyObjList();
                if (familyObjs != null && familyObjs.Count != 0)
                {
                    foreach (Family f in familyObjs)
                    {
                        familyLst.Add(f.FamilyName);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return familyLst;
        }

        /// <summary>
        ///  获取PdLine下拉框记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<string> familyObjs</returns>
        public IList<LineInfo> GetAllPdLines()
        {
            IList<LineInfo> lineLst = new List<LineInfo>();
            ILineRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
            try
            {
                lineLst = itemRepository.GetAllPdLineListByStage("FA");
            }
            catch (Exception e)
            {
                throw e;
            }
            return lineLst;
        }

        /// <summary>
        /// 保存一条KitLoc的记录数据(Add)
        /// </summary>
        /// <param name="Object"> FAFloatLocationDef item</param>
        public void AddKitLoc(FAFloatLocationDef item)
        {

            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                UnitOfWork uow = new UnitOfWork();
                FAFloatLocationInfo fAFloatLocationInfo = itemRepository.GetFAFloatLocation(item.family, item.partType, item.pdLine);
                if (fAFloatLocationInfo != null)
                {
                    //要添加的数据已经存在
                    ex = new FisException("DMT079", paraError);
                    throw ex;
               }
                FAFloatLocationInfo fAFloatLocation = new FAFloatLocationInfo();
                fAFloatLocation.family = item.family;
                fAFloatLocation.partType = item.partType;
                fAFloatLocation.pdLine = item.pdLine;
                fAFloatLocation.location = item.location;
                fAFloatLocation.editor = item.editor;
                fAFloatLocation.cdt = DateTime.Now;
                fAFloatLocation.udt = DateTime.Now;
                itemRepository.AddFAFloatLocationInfoDefered(uow, fAFloatLocation);
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 更新一条KitLoc的记录数据
        /// </summary>
        /// <param name="Object">FAFloatLocationDef item,string family,string pType,string pdLine</param>
        public void UpdateKitLoc(FAFloatLocationDef item, string itemId) 
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                UnitOfWork uow = new UnitOfWork();
                //当前信息是否存在
                FAFloatLocationInfo fAFloatLocations = itemRepository.GetFAFloatLocationInfo(int.Parse(itemId));
                if (fAFloatLocations == null)
                {
                    //要更新的原始数据已被删除
                    ex = new FisException("DMT082", paraError);
                    throw ex;
                }

                FAFloatLocationInfo lstFAFloatLocationInfo = itemRepository.GetFAFloatLocation(item.family, item.partType, item.pdLine);
                    //判断非当前记录和要更新的数据是否有重复
                //if (lstFAFloatLocationInfo.id != int.Parse(itemId))
                if (lstFAFloatLocationInfo != null && lstFAFloatLocationInfo.id != int.Parse(itemId))
                {
                    //要更新的数据已经存在于其他记录
                    ex = new FisException("DMT079", paraError);
                    throw ex;
                }
                FAFloatLocationInfo fAFloatLocation = new FAFloatLocationInfo();
                fAFloatLocation.family = item.family;
                fAFloatLocation.partType = item.partType;
                fAFloatLocation.pdLine = item.pdLine;
                fAFloatLocation.location = item.location;
                fAFloatLocation.editor = item.editor;
                fAFloatLocation.cdt = DateTime.Now;
                fAFloatLocation.udt = DateTime.Now;
                itemRepository.UpdateFAFloatLocationInfoDefered(uow, fAFloatLocation, int.Parse(itemId));
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// "删除一条记录
        /// </summary>
        /// <param name="?">FAFloatLocationDef item</param>
        public void DeleteKitLoc(FAFloatLocationDef item)
        {
            try
            {
                FAFloatLocationInfo fAFloatLocation = new FAFloatLocationInfo();
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                UnitOfWork uow = new UnitOfWork();
                itemRepository.DeleteFAFloatLocationInfoDefered(uow, item.id);
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

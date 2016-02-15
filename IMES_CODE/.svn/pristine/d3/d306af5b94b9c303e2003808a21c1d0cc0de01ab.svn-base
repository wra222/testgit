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
    class PalletQtyManager : MarshalByRefObject, IPalletQty
    {
        #region Implementation of GetQtyInfoList
        /// <summary>
        /// 取得Pallet Qty List下的Quantity数据的list(按fullQty排序)
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<PalletQtyDef></returns>
        public IList<PalletQtyDef> GetQtyInfoList()
        {
            IList<PalletQtyDef> pqList = new List<PalletQtyDef>();
            IList<PalletQtyInfo> pqDBList = new List<PalletQtyInfo>();

            try
            {
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                //访问数据的方法
                pqDBList = itemRepository.GetQtyInfoList();

                if (pqDBList != null)
                {
                    foreach (PalletQtyInfo pqDBInfo in pqDBList)
                    {
                        PalletQtyDef pqInfo = new PalletQtyDef();
                        pqInfo.fullQty = pqDBInfo.fullQty.ToString();
                        pqInfo.tireQty = pqDBInfo.tierQty.ToString();
                        pqInfo.mediumQty = pqDBInfo.mediumQty.ToString();
                        pqInfo.litterQty = pqDBInfo.litterQty.ToString();
                        pqInfo.id = pqDBInfo.id;
                        pqInfo.editor = pqDBInfo.editor;
                        if (pqDBInfo.cdt == DateTime.MinValue)
                        {
                            pqInfo.cdt = "";
                        }
                        else
                        {
                            pqInfo.cdt = ((System.DateTime)pqDBInfo.cdt).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (pqDBInfo.udt == DateTime.MinValue)
                        {
                            pqInfo.udt = "";
                        }
                        else
                        {
                            pqInfo.udt = ((System.DateTime)pqDBInfo.udt).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        pqList.Add(pqInfo);
                    }
                }

                return pqList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Implementation of AddQtyInfo
        /// <summary>
        /// 保存一条Qty的记录数据(Add)
        /// </summary>
        /// <param name="Object">alletQtyDef pqInfo</param>
        public string AddQtyInfo(PalletQtyDef palletQtyInfo)
        {
            FisException ex;
            List<string> paraError = new List<string>();
             PalletQtyInfo palletQtyDBInfo = new PalletQtyInfo();
            try
            {
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                //IList<PalletQtyInfo> lstPalletQtyInfo = itemRepository.GetPalletByFullQty(int.Parse(palletQtyInfo.fullQty));
                IList<PalletQtyInfo> lstPalletQtyInfo = itemRepository.GetPalletByFullQty(palletQtyInfo.fullQty);
                foreach(PalletQtyInfo item in lstPalletQtyInfo){
                    //if (item.fullQty == int.Parse(palletQtyInfo.fullQty))
                    if (item.fullQty == palletQtyInfo.fullQty)
                    {
                        //要添加的数据已经存在
                        ex = new FisException("DMT060", paraError);
                        throw ex;
                     }
                }
                UnitOfWork uow = new UnitOfWork();
                //palletQtyDBInfo.fullQty = int.Parse(palletQtyInfo.fullQty);
                palletQtyDBInfo.fullQty = palletQtyInfo.fullQty;
                palletQtyDBInfo.tierQty = int.Parse(palletQtyInfo.tireQty);
                palletQtyDBInfo.mediumQty = int.Parse(palletQtyInfo.mediumQty);
                palletQtyDBInfo.litterQty = int.Parse(palletQtyInfo.litterQty);
                palletQtyDBInfo.editor = palletQtyInfo.editor;
                palletQtyDBInfo.cdt = DateTime.Now;
                palletQtyDBInfo.udt = DateTime.Now;
                //添加函数
                itemRepository.AddQtyInfoDefered(uow, palletQtyDBInfo);
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
            return palletQtyInfo.fullQty;
        }
        #endregion

        #region Implementation of UpdateQtyInfo
        /// <summary>
        /// 更新一条Qty的记录数据(update),
        /// </summary>
        /// <param name="Object">PalletQtyDef pqInfo, string oldFullQty</param>
        public void UpdateQtyInfo(PalletQtyDef pqInfo, string itemId)
        {
            FisException ex;
            IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
            List<string> erpara = new List<string>();
            //
            try
            {
                //取出原始数据
                PalletQtyInfo palletQtyInfo = itemRepository.GetQtyInfo(int.Parse(itemId));
                //要更新数据的
                if (palletQtyInfo == null)
                {
                    //要更新的数据已被删除
                    ex = new FisException("DMT082", erpara);
                    throw ex;
                }
                //IList<PalletQtyInfo> lstPalletQtyInfo = itemRepository.GetPalletByFullQty(int.Parse(pqInfo.fullQty));
                IList<PalletQtyInfo> lstPalletQtyInfo = itemRepository.GetPalletByFullQty(pqInfo.fullQty);
                foreach (PalletQtyInfo palletQtyInfo2 in lstPalletQtyInfo)
                {
                    //判断非当前记录和要更新的数据是否有重复
                    //if (palletQtyInfo2.id != int.Parse(itemId) & palletQtyInfo2.fullQty == int.Parse(pqInfo.fullQty))
                    if (palletQtyInfo2.id != int.Parse(itemId) & palletQtyInfo2.fullQty == pqInfo.fullQty)
                    {
                        //throw new ApplicationException("The item is exisiting!");
                        ex = new FisException("DMT060", erpara);
                        throw ex;
                    }
                }
                UnitOfWork uow = new UnitOfWork();
                PalletQtyInfo item = new PalletQtyInfo();
                //item.fullQty = int.Parse(pqInfo.fullQty);
                item.fullQty = pqInfo.fullQty;
                item.tierQty = int.Parse(pqInfo.tireQty);
                item.mediumQty = int.Parse(pqInfo.mediumQty);
                item.litterQty = int.Parse(pqInfo.litterQty);
                item.editor = pqInfo.editor;
                item.udt = DateTime.Now;

                //更新动作
                itemRepository.UpdateQtyInfoDefered(uow, item, int.Parse(itemId));
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region Implementation of DeleteQtyInfo
        /// <summary>
        /// "删除一条Qty的记录数据
        /// </summary>
        /// <param name="?">String itemId</param>
        public void DeleteQtyInfo(String itemId)
        {
            try
            {
                //对数据操作的删除方法
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                UnitOfWork uow = new UnitOfWork();
                itemRepository.DeleteQtyInfoDefered(uow, int.Parse(itemId));
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }

    }
}

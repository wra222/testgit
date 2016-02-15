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
   public class ChepPalletManager : MarshalByRefObject, IChepPallet
    {
        /// <summary>
        ///  取得ChepPallet的所有记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<ChepPalletDef> chepList</returns>
        public IList<ChepPalletDef> GetChepPalletList() {
            IList<ChepPalletDef> chepList = new List<ChepPalletDef>();
            IList<ChepPalletInfo> listInfo = new List<ChepPalletInfo>();
            try
            {
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                listInfo = itemRepository.GetChepPalletList();
                foreach (ChepPalletInfo chepPalletInfo in listInfo)
                {
                    ChepPalletDef item = new ChepPalletDef();
                    item.palletNo = chepPalletInfo.palletNo;
                    item.editor = chepPalletInfo.editor;
                    item.id = chepPalletInfo.id;

                    if (chepPalletInfo.cdt == DateTime.MinValue)
                    {
                        item.cdt = "";
                    }
                    else
                    {
                        item.cdt = ((System.DateTime)chepPalletInfo.cdt).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (chepPalletInfo.udt == DateTime.MinValue)
                    {
                        item.udt = "";
                    }
                    else
                    {
                        item.udt = ((System.DateTime)chepPalletInfo.udt).ToString("yyyy-MM-dd HH:mm:ss");
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
        ///  取得一条ChepPallet的记录数据
        /// </summary>
        /// <param name="?">String chepPalletNo</param>
        /// <returns>ChepPalletDef chepPalletDef</returns>
        public ChepPalletDef GetChepPalletInfor(String chepPalletNo)
        {
            
            try
            {
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                ChepPalletInfo chepPalletInfo = itemRepository.GetChepPalletInfo(chepPalletNo);
                ChepPalletDef chepPalletDef = new ChepPalletDef();
                chepPalletDef.palletNo = chepPalletInfo.palletNo;
                chepPalletDef.editor = chepPalletInfo.editor;
                chepPalletDef.id = chepPalletInfo.id;
                if (chepPalletInfo.cdt == DateTime.MinValue)
                {
                    chepPalletDef.cdt = "";
                }
                else
                {
                    chepPalletDef.cdt = ((System.DateTime)chepPalletInfo.cdt).ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (chepPalletInfo.udt == DateTime.MinValue)
                {
                    chepPalletDef.udt = "";
                }
                else
                {
                    chepPalletDef.udt = ((System.DateTime)chepPalletInfo.udt).ToString("yyyy-MM-dd HH:mm:ss");
                }
                return chepPalletDef;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存一条ChepPallet的记录数据(Add)
        /// </summary>
        /// <param name="Object">ChepPalletDef item</param>
        public void AddChepPallet(ChepPalletDef item)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                ChepPalletInfo chepPalletInfo = new ChepPalletInfo();
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                UnitOfWork uow = new UnitOfWork();

                //检查要添加的信息是否已经存在
                if (itemRepository.GetChepPalletInfo(item.palletNo) == null)
                {
                    chepPalletInfo.palletNo = item.palletNo;
                    chepPalletInfo.editor = item.editor;
                    chepPalletInfo.cdt = DateTime.Now;
                    chepPalletInfo.udt = DateTime.Now;
                    itemRepository.AddGetChepPalletInfoDefered(uow, chepPalletInfo);
                    uow.Commit();
                }
                else
                {
                    //要添加的数据已经存在
                    ex = new FisException("DMT072", paraError);
                    throw ex;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


        /// <summary>
        /// "删除一条ChepPallet的记录数据
        /// </summary>
        /// <param name="?">string chepPalletNo</param>
        public void DeleteChepPallet(string itemId) {
            List<string> paraError = new List<string>();
            try
            {
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                UnitOfWork uow = new UnitOfWork();
                itemRepository.DeleteChepPalletInfoDefered(uow, int.Parse(itemId));
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

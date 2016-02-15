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
using IMES.FisObject.FA.LCM;
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
    class ICASAManager : MarshalByRefObject, IICASA
    {
        /// <summary>
        ///  取得ICASA的所有记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns>IList<ChepPalletDef></returns>
        public IList<ICASAInfo> GetICASAList() {
            IList<ICASADef> lstICASADef = new List<ICASADef>();
            IList<ICASAInfo> listInfo = new List<ICASAInfo>();
            try
            {
                ILCMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository>();
                lstICASADef = itemRepository.GetICASAList();
                foreach (ICASADef iCASADef in lstICASADef)
                {
                    ICASAInfo iCASAInfo = new ICASAInfo();
                    iCASAInfo.vc = iCASADef.vc;
                    iCASAInfo.av = iCASADef.av;
                    iCASAInfo.anatel1 = iCASADef.anatel1;
                    iCASAInfo.anatel2 = iCASADef.anatel2;
                    iCASAInfo.icasa = iCASADef.icasa;
                    iCASAInfo.edit = iCASADef.edit;
                    if (iCASADef.cdt == DateTime.MinValue)
                    {
                        iCASAInfo.cdt = "";
                    }
                    else
                    {
                        iCASAInfo.cdt = ((System.DateTime)iCASADef.cdt).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (iCASADef.udt == DateTime.MinValue)
                    {
                        iCASAInfo.udt = "";
                    }
                    else
                    {
                        iCASAInfo.udt = ((System.DateTime)iCASADef.udt).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    iCASAInfo.id = iCASADef.id;
                    listInfo.Add(iCASAInfo);
                
                }
                return listInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存一条ICASA的记录数据(Add)
        /// </summary>
        /// <param name="Object">ICASADef item</param>
        public string AddICASAInfo(ICASAInfo item)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                
                ILCMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository>();
                UnitOfWork uow = new UnitOfWork();
                ICASADef iCASADefs = itemRepository.GetICASAInfoByVC(item.vc);
                if (iCASADefs != null)
                {
                    //要添加的数据已经存在
                    ex = new FisException("DMT095", paraError);
                    throw ex;
                }
                ICASADef iCASADef = new ICASADef();
                iCASADef.vc = item.vc;
                iCASADef.av = item.av;
                iCASADef.anatel1 = item.anatel1;
                iCASADef.anatel2 = item.anatel2;
                iCASADef.icasa = item.icasa;
                iCASADef.edit = item.edit;
                iCASADef.cdt = DateTime.Now;
                iCASADef.udt = DateTime.Now;
                itemRepository.AddICASAInfoDefered(uow, iCASADef);
                uow.Commit();
                iCASADef = itemRepository.GetICASAInfoByVC(item.vc);
                return iCASADef.id.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存一条ICASA的记录数据(Add)
        /// </summary>
        /// <param name="Object">ICASADef item</param>
        public void UpdateICASAInfo(ICASAInfo item, string itemId)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                ILCMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository>();
                UnitOfWork uow = new UnitOfWork();
                //当前信息是否存在
                if (itemRepository.GetICASAInfoById(int.Parse(itemId)) == null)
                {
                    //要更新的原始数据已被删除
                    ex = new FisException("DMT082", paraError);
                    throw ex;
                }
                ICASADef iCASADefs = itemRepository.GetICASAInfoByVC(item.vc);
                if (iCASADefs != null)
                {
                    //判断非当前记录和要更新的数据是否有重复
                    if (iCASADefs.id != int.Parse(itemId))
                    {
                        //要更新的数据已经存在于其他记录
                        ex = new FisException("DMT095", paraError);
                        throw ex;
                    }
                }
                ICASADef iCASADef = new ICASADef();
                iCASADef.vc = item.vc;
                iCASADef.av = item.av;
                iCASADef.anatel1 = item.anatel1;
                iCASADef.anatel2 = item.anatel2;
                iCASADef.icasa = item.icasa;
                iCASADef.edit = item.edit;
                itemRepository.UpdateICASAInfoDefered(uow, iCASADef, int.Parse(itemId));
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除一条ICASA的记录数据
        /// </summary>
        /// <param name="?">string id</param>
        public void DeleteICASAInfo(string id) {
            try
            {
                ICASADef fAFloatLocation = new ICASADef();
                ILCMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository>();
                UnitOfWork uow = new UnitOfWork();
                itemRepository.DeleteICASAInfoDefered(uow, int.Parse(id));
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

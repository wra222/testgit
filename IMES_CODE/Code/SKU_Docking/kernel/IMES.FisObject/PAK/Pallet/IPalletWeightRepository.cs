// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-27   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.PAK.Pallet
{
    /// <summary>
    /// PalletWeight对象Repository接口
    /// </summary>
    public interface IPalletWeightRepository : IRepository<PalletWeight>
    {
        /// <summary>
        /// 获得PalletWeight列表
        /// </summary>
        /// <param name="family"></param>
        /// <param name="region"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        IList<PalletWeight> GetPltWeight(string family, string region, short qty);

        /// <summary>
        /// select Weight from IMES_PAK..FRUFISToSAPWeight where Shipment=''
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        decimal GetWeightOfFRUFISToSAPWeight(string shipment);

        /// <summary>
        /// 将记录插入或者update
        /// 没有就插入，有就更新
        /// insert into IMES_PAK..FRUFISToSAPWeight (Shipment,[Type],Weight,Status,Cdt) Values('','','','',getdate())
        /// update IMES_PAK..FRUFISToSAPWeight Set Weight='',Cdt=getdate() where Shipment=''
        /// </summary>
        /// <param name="item"></param>
        void AddOrModifyFRUFISToSAPWeight(FRUFISToSAPWeight item);
        
        /// <summary>
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PalletWeightInfo> GetPltWeightByCondition(PalletWeightInfo condition);

        #region Defered

        void AddOrModifyFRUWeightLogDefered(IUnitOfWork uow, FRUFISToSAPWeight item);

        #endregion

        #region FISTOSAP_WEIGHT table
        IList<FisToSapWeightDef> ExistsFisToSapWeightByShipment(string DBconnectionStr,IList<string> shipmentList);
        void InsertFisToSapWeight(string DBconnectionStr,FisToSapWeightDef shipWeight);
        void InsertFisToSapWeightDefered(IUnitOfWork uow,string DBconnectionStr, FisToSapWeightDef shipWeight);
        #endregion
    }
}

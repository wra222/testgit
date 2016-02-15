using System;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// IConstValueType接口
    /// </summary>
    public interface ISpecialOrder
    {

        /// <summary>
        /// insert into SpecialOrder(FactoryPO, Category, AssetTag, Qty, Status, 
        /// Remark, Editor, Cdt, Udt)
        /// values(@FactoryPO, @Category, @AssetTag, @Qty, @Status, 
        /// @Remark, @Editor, getdate(), getdate())
        /// </summary>
        /// <param name="sepcialOrder"></param>
        /// <returns></returns>
        void InsertSpecialOrder(SpecialOrderInfo sepcialOrder);

        /// <summary>
        /// update SpecialOrder
        /// set Category=@Category,
        /// AssetTag=@AssetTag,
        /// Qty=@Qty,
        /// Status=@Status,
        /// Remark=@Remark,
        /// Editor=@Editor,
        /// Udt=getdate()
        /// where FactoryPO=@FactoryPO
        /// </summary>
        /// <param name="sepcialOrder"></param>
        /// <returns></returns>
        void UpdateSpecialOrder(SpecialOrderInfo sepcialOrder);

        /// <summary>
        /// delete from SpecialOrder
        /// where FactoryPO=@FactoryPO
        /// </summary>
        /// <param name="factoryPO"></param>
        /// <returns></returns>
        void DeleteSpecialOrder(string factoryPO);

        /// <summary>
        /// select FactoryPO, Category, AssetTag, Qty, Status, 
        /// Remark, Editor, Cdt, Udt
        /// from SpecialOrder  
        /// where Category=@Category and
        /// Status =@Status and
        /// Cdt between @StartTime and @EndTime
        /// </summary>
        /// <param Category="Category" Status="Status" StartTime="StartTime" EndTime="EndTime"></param>
        /// <returns></returns>
        IList<SpecialOrderInfo> GetSpecialOrder(string category, SpecialOrderStatus status, DateTime startTime, DateTime endTime);

        /// <summary>
        /// select FactoryPO
        /// from SpecialOrder  
        /// where FactoryPO=@FactoryPO
        /// </summary>
        /// <param name="factoryPO"></param>
        /// <returns></returns>
        bool ExistSpecialOrder(string factoryPO);

        /// <summary>
        /// Get ConstValueType Value List
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        IList<ConstValueTypeInfo> GetConstValueTypeList(string Type);

        /// <summary>
        /// insert into SpecialOrder(FactoryPO, Category, AssetTag, Qty, Status, 
        /// Remark, Editor, Cdt, Udt)
        /// values(@FactoryPO, @Category, @AssetTag, @Qty, @Status, 
        /// @Remark, @Editor, getdate(), getdate())
        /// </summary>
        /// <param name="sepcialOrderList"></param>
        /// <returns></returns>
        IList<string> InsertSpecialOrder(IList<SpecialOrderInfo> sepcialOrderList);

        /// <summary>
        /// select FactoryPO, Category, AssetTag, Qty, Status, 
        ///Remark, Editor, Cdt, Udt
        ///from SpecialOrder  
        ///where FactoryPO=@FactoryPO
        /// </summary>
        /// <param FactoryPO="FactoryPO"></param>
        /// <returns></returns>
        IList<SpecialOrderInfo> GetSpecialOrderByPO(string factoryPO);

    }
}

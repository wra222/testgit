using System;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.Station
{
    ///<summary>
    /// Station类，用于表示产线的操作站
    ///</summary>
    public interface IStation : IAggregateRoot
    {
        /// <summary>
        /// Station代码
        /// </summary>
        string StationId { get; set; }

        /// <summary>
        /// StationType
        /// </summary>
        StationType StationType { get; set; }

        /// <summary>
        /// 该站操作的对象类型0:未定义 1:MB; 2:Product;
        /// </summary>
        int OperationObject { get; set; }

        /// <summary>
        /// Station描述
        /// </summary>
        string Descr { get; set; }

        /// <summary>
        /// 维护人员
        /// </summary>
        string Editor { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime Cdt { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime Udt { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }
    }
}
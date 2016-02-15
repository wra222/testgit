using System.Collections.Generic;
using IMES.Infrastructure.FisObjectBase;
using System;

namespace IMES.FisObject.PCA.MBMO
{
    /// <summary>
    /// MBMO接口
    /// </summary>
    public interface IMBMO : IAggregateRoot
    {
        ///<summary>
        /// MONo
        ///</summary>
        string MONo { get; set; }

        ///<summary>
        /// MB Family
        ///</summary>
        string Family { get; set; }

        ///<summary>
        /// Model
        ///</summary>
        string Model { get; set; }

        ///<summary>
        /// Mo包含的Mb总数
        ///</summary>
        int Qty { get; set; }

        ///<summary>
        /// 此MO中已产生MB序号的数量
        ///</summary>
        int PrintedQty { get; set; }

        ///<summary>
        /// Remark
        ///</summary>
        string Remark { get; set; }
        
        ///<summary>
        /// Status
        ///</summary>
        string Status { get; set; }

        ///<summary>
        /// Editor
        ///</summary>
        string Editor { get; set; }

        ///<summary>
        /// Cdt
        ///</summary>
        DateTime Cdt { get; }

        ///<summary>
        /// Udt
        ///</summary>
        DateTime Udt { get; }

        /// <summary>
        /// 增加打印主板标签数量
        /// </summary>
        /// <param name="byQty">新增数量</param>
        /// <returns>实际增加数量</returns>
        int IncreasePrintedQty(int byQty);

        /// <summary>
        /// 减少打印主板标签数量
        /// </summary>
        /// <param name="byQty">减少数量</param>
        /// <returns>实际减少数量</returns>
        int DecreasePrintedQty(int byQty);

        /// <summary>
        /// 回收已组装的主板。以便将来重新生产。
        /// </summary>
        /// <param name="startMBSn">起始主板序号</param>
        /// <param name="endMBSn">结束主板序号</param>
        void DismantleMB(string startMBSn, string endMBSn);

        /// <summary>
        /// 产生主板信息。
        /// </summary>
        /// <param name="qty">待产生数量</param>
        /// <returns>产生的主板序号集合</returns>
        IList<string> GenerateMB(int qty);

    }
}
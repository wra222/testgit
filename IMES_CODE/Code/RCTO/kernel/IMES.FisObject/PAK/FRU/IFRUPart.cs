using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.FisObject.PAK.FRU
{
    public interface IFRUPart
    {
        /// <summary>
        /// 记录标识
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// Carton ID/Gift ID
        /// </summary>
        string FRUID { get; set; }

        /// <summary>
        /// Pn/GiftID
        /// </summary>
        string PartID { get; }

        /// <summary>
        /// Part Sn
        /// </summary>
        string Value { get; }

        /// <summary>
        /// 维护人员
        /// </summary>
        string Editor { get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime Cdt { get; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.Part
{
    /// <summary>
    /// PartCheck的匹配规则
    /// </summary>
    public class PartCheckMatchRule
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 所属的PartCheckId
        /// </summary>
        public int PartCheckID { get; set; }

        /// <summary>
        /// 正则表达式匹配规则
        /// </summary>
        public string RegExp { get; set; }

        /// <summary>
        /// 使用Pn与匹配目标关系定义的匹配规则
        /// </summary>
        public string PnExp { get; set; }

        /// <summary>
        /// 使用Part属性与匹配目标之间关心定义的匹配规则
        /// </summary>
        public string PartPropertyExp { get; set; }

        /// <summary>
        /// 该匹配规则是否假定匹配目标带校验位
        /// </summary>
        public int ContainCheckBit { get; set; }

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor { get; set; }

        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt { get; set; }

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt { get; set; }
    }
}

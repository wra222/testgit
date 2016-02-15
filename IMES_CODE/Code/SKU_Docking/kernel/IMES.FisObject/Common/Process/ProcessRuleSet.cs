using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.Process
{
    /// <summary>
    /// 用于生成流程数据的规则数据类
    /// </summary>
    public class ProcessRuleSet
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Condition
        /// </summary>
        public string Condition1 { get; set; }
        /// <summary>
        /// Condition
        /// </summary>
        public string Condition2 { get; set; }
        /// <summary>
        /// Condition
        /// </summary>
        public string Condition3 { get; set; }
        /// <summary>
        /// Condition
        /// </summary>
        public string Condition4 { get; set; }
        /// <summary>
        /// Condition
        /// </summary>
        public string Condition5 { get; set; }
        /// <summary>
        /// Condition
        /// </summary>
        public string Condition6 { get; set; }
        /// <summary>
        /// Editor
        /// </summary>
        public string Editor { get; set; }
        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt { get; set; }
        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt { get; set; }

        /// <summary>
        /// resolve condition value in collection
        /// </summary>
        public IList<string> ConditiionValueList { get; set; }
    }
}

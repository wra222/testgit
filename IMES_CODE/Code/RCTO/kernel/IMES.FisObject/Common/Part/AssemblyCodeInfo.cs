using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.Part
{
    /// <summary>
    /// AssemblyCode熟悉你给
    /// </summary>
    public class AssemblyCodeInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// AssemblyCode
        /// </summary>
        public string AssemblyCode { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string InfoType { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string InfoValue { get; set; }

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

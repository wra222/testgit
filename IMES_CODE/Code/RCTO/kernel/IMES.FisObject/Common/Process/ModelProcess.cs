using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.Process
{
    /// <summary>
    /// ModelProcess实体，表示Model和Process之间的关系
    /// </summary>
    public class ModelProcess
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        //public string RuleType { get; set; }

        /// <summary>
        /// Model
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Process
        /// </summary>
        public string Process { get; set; }

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
        /// Line
        /// </summary>
        public string Line { get; set; }
    }
}

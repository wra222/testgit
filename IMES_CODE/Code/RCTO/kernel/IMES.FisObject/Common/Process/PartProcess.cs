using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.Process
{
    /// <summary>
    /// MB使用的流程
    /// </summary>
    public class PartProcess
    {
        /// <summary>
        /// Family of MB
        /// </summary>
        public string MBFamily { get; set; }

        /// <summary>
        /// 流程名字
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// = Y ,表示试产流程;
        /// = N ,表示量产流程.
        /// </summary>
        public string PilotRun { get; set; }

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
    }
}

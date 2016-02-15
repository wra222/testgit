using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace IMES.Infrastructure.Utility.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public class BackgroundThreadNotifier
    {
        /// <summary>
        /// 
        /// </summary>
        public static AutoResetEvent ServiceStopNotifier = new AutoResetEvent(false);
    }
}

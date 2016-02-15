using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;

namespace IMES.CheckItemModule.CQ.Nothing.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.Nothing.Filter.dll")]
    class MatchModule : IMatchModule
    {
        public object Match(string subject, object bomItem, string station)
        {

            return null;
        }
    }
}

using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.TRIS.Battery.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TRIS.Battery.Filter.dll")]
    class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
            return;
            
        }
    }
}

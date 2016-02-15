using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Material;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.CheckItemModule.CommonRules.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CommonRules.Filter.dll")]
    class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
			
        }
    }
}

// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Pizza类对应Pizza表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       create
// 2012-03-15   210003                       ITC-1360-1443
// Known issues:
//
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.OOB.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.OOB.Filter.dll")]
    class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
         
        }
    }
}

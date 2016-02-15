// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2011-12-19   210003                       ITC-1360-0886
// Known issues:

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;


namespace IMES.CheckItemModule.CQ.Nothing.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.Nothing.Filter.dll")]
    public class Filter : IFilterModule
    {
        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {            
            return null;
        }
    }

}

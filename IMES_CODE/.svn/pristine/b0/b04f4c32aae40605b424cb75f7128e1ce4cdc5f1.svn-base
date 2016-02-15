// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-02-28   210003                       ITC-1360-0460
// 2012-03-06   210003                       ITC-1360-1109
// 2012-03-06   210003                       ITC-1360-0455
// 2012-03-13   210003                       UC Checnged
// Known issues:
//

using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Process;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace IMES.CheckItemModule.TabletKP.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TabletKP.Filter.dll")]
    class CheckModule : ICheckModule
    {
  
        private const string part_check_type = "TabletKP";
        public void Check(object partUnit, object bomItem, string station)
        {
            if (partUnit != null)
            {
                Session currentSession = SessionManager.GetInstance.GetSession(((PartUnit)partUnit).Sn, Session.SessionType.Product);
                if (currentSession == null)
                {
                    currentSession =(Session)((PartUnit)partUnit).CurrentSession;
                }
                if (currentSession == null)
                {
                    throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
                }
                if (string.IsNullOrEmpty(station))
                {
                    throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.TabletKP.Filter.Check" });
                }
                string partSn = ((PartUnit)partUnit).Sn.Trim();
                string regx = (string)currentSession.GetValue(part_check_type + "Regx" + ((PartUnit)partUnit).ValueType);
                if (string.IsNullOrEmpty(regx))
                { return ; }
                Regex Regex1 = new Regex(regx);
                if (!Regex1.IsMatch(partSn))
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(partSn);
                    FisException e = new FisException("MAT010", errpara);
                    throw e;
                }
                    //string[] regxArr = regx.Split(',');
                    //foreach (string pattern in regxArr)
                    //{
                    //    Regex Regex1 = new Regex(pattern);
                    //    if (Regex1.IsMatch(partSn))
                    //    {
                    //        return;
                    //    }
                    //}
                   
            
               
            }
            else
            {
                throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.TabletKP.Filter.CheckModule.Check" });

            }
        }

       
    }
}

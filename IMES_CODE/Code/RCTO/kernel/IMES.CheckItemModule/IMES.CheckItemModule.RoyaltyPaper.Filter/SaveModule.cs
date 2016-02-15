// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-03-15   210003                       ITC-1360-1458
// Known issues:

using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.RoyaltyPaper.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.RoyaltyPaper.Filter.dll")]
    public class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
            //需要更新状态为'A1' （IMES_PAK..COAStatus.Status）并记录Log （IMES_PAK..COALog）
            if (part_unit == null)
            {
                throw new ArgumentNullException();
            }
            if (part_owner == null)
            {
                throw new ArgumentNullException();
            }
            //if (part_unit != null)
            //{
            var product = (Product)part_owner;
            string sn = ((PartUnit)part_unit).Sn.Trim();
            Session session = SessionManager.GetInstance.GetSession(((PartUnit)part_unit).ProductId, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            ICOAStatusRepository pizza_repository = 
                RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
            COAStatus coa_status = pizza_repository.GetCoaStatus(sn);
            if (coa_status != null)
            {
                coa_status.Status = "A1";
                pizza_repository.UpdateCOAStatusDefered(session.UnitOfWork, coa_status);
                var coa_log = new COALog
                                  {
                                      COASN = coa_status.COASN,
                                      StationID = "A1",
                                      Editor = session.Editor,
                                      IsDirty = coa_status.IsDirty,
                                      LineID = product.CUSTSN,
                                      Cdt = coa_status.Cdt,
                                      Tracker = coa_status.Tracker,
                                      Tp = "COA"
                                  };
                pizza_repository.InsertCOALogDefered(session.UnitOfWork, coa_log);
            }
            //}
            //else
            //{
            //    throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.RoyaltyPaper.Filter.SaveModule.Save" });
            //}
        }
    }
}

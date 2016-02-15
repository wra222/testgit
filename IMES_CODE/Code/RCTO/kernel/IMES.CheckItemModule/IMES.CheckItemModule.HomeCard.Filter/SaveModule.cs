// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-03-12   210003                       ITC-1360-1375
// Known issues:

using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;

namespace IMES.CheckItemModule.HomeCard.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.HomeCard.Filter.dll")]
    class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
            //CN Card，需要更新状态为'A1' （IMES_PAK..CSNMas.WC），IsPass 为'0'，
            //并记录Log （IMES_PAK..CSNLog）
            if (part_unit == null)
            {
                throw new ArgumentNullException();
            }
            if (part_owner == null)
            {
                throw new ArgumentNullException();
            }
//            string sn = ((PartUnit)part_unit).Sn;
            Session session = SessionManager.GetInstance.GetSession(((PartUnit)part_unit).ProductId, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            ICOAStatusRepository home_card_repository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();

            CSNMasInfo csn_mas_info = home_card_repository.GetCsnMas(((PartUnit)part_unit).Sn);
            if (csn_mas_info != null)
            {
                csn_mas_info.status = "A1";
                home_card_repository.UpdateCSNMas(csn_mas_info);

                var csn_log = new CSNLogInfo
                                  {
                                      pno = csn_mas_info.pno,
                                      cdt = DateTime.Now,
//                                      editor = csn_mas_info.editor,
                                      editor = session.Editor,
                                      isPass = 0,
//                                      pdLine = csn_mas_info.pdLine,
                                      pdLine = session.Line,
                                      snoId = csn_mas_info.csn2,
                                      tp = ((PartUnit) part_unit).Type,
                                      wc = station
                                  };
                home_card_repository.InsertCSNLog(csn_log);
            }
        }
    }
}

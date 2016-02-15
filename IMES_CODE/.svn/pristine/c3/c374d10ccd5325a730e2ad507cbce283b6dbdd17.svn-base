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

namespace IMES.CheckItemModule.OOA.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.OOA.Filter.dll")]
    class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
            //需要更新状态为’A1’ （IMES_PAK..COAStatus.Station），并记录Log （IMES_PAK..COALog）
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
                string sn = ((PartUnit)part_unit).Sn;
                //Pizza pizza = (Pizza) part_owner;
                Session session = SessionManager.GetInstance.GetSession(key, Session.SessionType.Product);
                if (session == null)
                {
                    throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
                }
                Product product = (Product)session.GetValue(Session.SessionKeys.Product);
                
                ICOAStatusRepository coastatus_repository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
                COAStatus coa_status = coastatus_repository.GetCoaStatus(sn);
                if (coa_status != null)
                {
                    coa_status.Status = "A1";
                 //   coastatus_repository.UpdateCOAStatus(coa_status);
                    coastatus_repository.UpdateCOAStatusDefered(session.UnitOfWork, coa_status);
                   
                    var coa_log = new COALog
                                      {
                                          COASN = coa_status.COASN,
                                          StationID = "A1",
                                          Editor = session.Editor,
                                          IsDirty = coa_status.IsDirty,
                                          LineID = product.CUSTSN,
                                          Cdt = coa_status.Cdt,
                                          Tracker = coa_status.Tracker,
                                          Tp = ""
                                      };
              //      coastatus_repository.InsertCOALog(coa_log);
                    coastatus_repository.InsertCOALogDefered(session.UnitOfWork, coa_log);

                }
//                IPizzaRepository pizza_repository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
//                Pizza pizza = product.PizzaObj;
//                IProductPart part = new ProductPart();
//                part.PartID = ((PartUnit)part_unit).Pn;
//                part.PartSn = ((PartUnit)part_unit).Sn;
//                part.BomNodeType = ((PartUnit)part_unit).Type;
//                part.CheckItemType = ((PartUnit)part_unit).ItemType;
//                part.PartType = ((PartUnit)part_unit).ValueType;
//                part.Station = station;
//                part.Editor = session.Editor;
//                pizza.AddPart(part);
//                pizza_repository.Update(pizza, session.UnitOfWork);
        }
    }
}

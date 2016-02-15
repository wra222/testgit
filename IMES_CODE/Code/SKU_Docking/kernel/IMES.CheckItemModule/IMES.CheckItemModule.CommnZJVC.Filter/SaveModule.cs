using System;
using System.Collections;
using System.Collections.Generic;
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
using IMES.DataModel;

namespace IMES.CheckItemModule.CommnZJVC.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CommnZJVC.Filter.dll")]
    class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
            if (part_unit == null)
            {
                throw new ArgumentNullException();
            }
            if (part_owner == null)
            {
                throw new ArgumentNullException();
            }
            string sn = ((PartUnit)part_unit).Sn;
            Session session = SessionManager.GetInstance.GetSession(key, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
            Material materialobj = materialRep.Find(sn);
            if (materialobj == null)
            {
                var material = new Material();
                material.MaterialCT = sn;
                material.MaterialType = ((PartUnit)part_unit).ValueType;
                material.Stage = "RCTO";
                material.Model = ((PartUnit)part_unit).Pn;
                material.Line = session.Line;
                material.Status = session.Station;
                material.PreStatus = "";
                material.PizzaID = key;
                material.QCStatus = "1";
                material.Editor = session.Editor;
                material.ShipMode = ((PartUnit)part_unit).ItemType;
                material.Cdt = DateTime.Now;
                material.Udt = DateTime.Now;
                materialRep.Add(material, session.UnitOfWork);
            }
            else
            {
                materialobj.Editor = session.Editor;
                materialobj.Cdt = DateTime.Now;
                materialobj.Udt = DateTime.Now;
                materialobj.QCStatus = (Convert.ToInt32(materialobj.QCStatus) + 1).ToString();
                materialRep.Update(materialobj, session.UnitOfWork);
            }
           
            materialRep.AddMultiMaterialLogDefered(session.UnitOfWork,
                                                           new List<string> { sn },
                                                              "TestKP",
                                                              "RCTO",
                                                               session.Line,
                                                                "",
                                                                session.Station,
                                                                 key,
                                                                 session.Editor);



           
           
        }
    }
}

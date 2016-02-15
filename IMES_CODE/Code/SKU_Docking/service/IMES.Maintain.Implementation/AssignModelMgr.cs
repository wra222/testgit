using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.Common.Line;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.DataModel;
//using IMES.FisObject.Common.BOM;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Maintain.Implementation
{
    public class AssignModelMgr : MarshalByRefObject, IAssignModelMgr
    {
        private IModelRepository myRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();

        public DataTable GetByModelShipDate(string model, DateTime shipDate)
        {
            try
            {
                IList<ModelChangeQtyDef> lst = myRepository.GetModelChangeQtyByModelShipDate(model, shipDate);
                DataTable ret = SQLData.ToDataTable(lst);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Import(IList<List<string>> lst, string editor)
        {
            try
            {
                foreach (IList<string> t in lst)
                {
                    ModelChangeQtyDef v = new ModelChangeQtyDef();
                    
                    v.Model = t[0];
                    v.Qty = Convert.ToInt16(t[1]);
                    v.ShipDate = Convert.ToDateTime(t[2]);
                    v.Line = t[3];

                    v.AssignedQty = 0;
                    v.Status = "A";
                    v.Editor = editor;
                    v.Cdt = DateTime.Now;
                    myRepository.AddModelChangeQty(v);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(IList<int> lst)
        {
            try
            {
                foreach (int id in lst)
                {
                    myRepository.DeleteModelChangeQty(id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

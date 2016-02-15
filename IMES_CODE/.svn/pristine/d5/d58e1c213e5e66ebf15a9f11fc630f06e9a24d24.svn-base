using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Route;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using log4net;
using IMES.FisObject.Common.Material;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.UnitOfWork;


namespace IMES.Station.Implementation
{
    public class SAInputXRay : MarshalByRefObject, ISAInputXRay
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void Save(string input, string pdline, string model, string location, string obligation,string remark,string state, string customer, string editor)
        {
            logger.Debug("Save start, MBSno:" + input);
            try
            {
                var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
                IUnitOfWork uof = new UnitOfWork();
                Material material = new Material();

                material.MaterialCT = input;
                material.MaterialType = "XRay";
                material.Model = model;
                material.Line = pdline;
                material.DeliveryNo = state;
                material.PalletNo = location;
                material.CartonSN = obligation;
                material.PreStatus = "XRay";
                material.Status = "1";
                material.ShipMode = remark;
                material.Editor = editor;
                material.Cdt = DateTime.Now;
                material.Udt = DateTime.Now;
                
                materialRep.Add(material, uof);
                uof.Commit();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug(" InputMB end, MBSno:" + input);
            }

        }
        public IList<XRay> GetMaterialByTypeList(string type)
        {
            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();

           IList<Material>  materiallist = materialRep.GetMaterialByType(type);
           IList<XRay> ret = new List<XRay>();
           foreach (Material ss in materiallist)
           {
               XRay  ele = new XRay();
               ele.PCBNo = ss.MaterialCT;
               ele.PdLine=ss.Line ;
               ele.Model=ss.Model ;
               ele.Obligation=ss.CartonSN ;
               ele.Location=ss.PalletNo;
               ele.IsPass=ss.DeliveryNo;
               ele.Remark=ss.ShipMode;
               ele.Editor=ss.Editor;
               ele.Cdt=ss.Cdt;
               ret.Add(ele);
           }

           return ret;
        }

    }

}

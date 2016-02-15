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
    public class FIXTure : MarshalByRefObject, IFIXTure
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public void Save(string input, string loc,string editor)
        {
            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
            Material material = materialRep.Find(input);
            if (material != null)
            {
                logger.Debug("Save start, CT:" + input);
                try
                {
                    IUnitOfWork uof = new UnitOfWork();
                    Material material1 = materialRep.Find(input);

                    material1.MaterialCT = input;
                    material1.MaterialType = "FIXTure";
                    material1.Line = "";
                    material1.PreStatus = "FIXTure";
                    material1.PalletNo = loc;
                    material1.Editor = editor;
                    material1.Status = "In";
                    material1.Cdt = DateTime.Now;
                    material1.Udt = DateTime.Now;
                    materialRep.Update(material1, uof);
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
                    logger.Debug(" InputCT end, CT:" + input);
                }
            }
            else
            {
                logger.Debug("Save start, CT:" + input);
                try
                {
                    IUnitOfWork uof = new UnitOfWork();
                    Material material1 = new Material();

                    material1.MaterialCT = input;
                    material1.MaterialType = "FIXTure";
                    material1.PreStatus = "FIXTure";
                    material1.PalletNo = loc;
                    material1.Editor = editor;
                    material1.Status = "In";
                    material1.Cdt = DateTime.Now;
                    material1.Udt = DateTime.Now;
                    materialRep.Add(material1, uof);
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
                    logger.Debug(" InputCT end, CT:" + input);
                }
            }
        }
        public void OutSave(string input, string pdline, string editor)
        {
            logger.Debug("Save start, CT:" + input);
            try
            {
                var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
                IUnitOfWork uof = new UnitOfWork();
                Material material = materialRep.Find(input);

                material.MaterialCT = input;
                material.PalletNo = "";
                material.Editor = editor;
                material.Status = "OUT";
                material.Line = pdline;
                material.Cdt = DateTime.Now;
                material.Udt = DateTime.Now;
                materialRep.Update(material, uof);
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
                logger.Debug(" InputCT end, CT:" + input);
            }
        }

    }
}

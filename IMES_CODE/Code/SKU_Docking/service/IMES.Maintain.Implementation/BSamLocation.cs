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
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;
using IMES.Maintain.Implementation;
using log4net;
using IMES.FisObject.PAK.BSam;

namespace IMES.Maintain.Implementation
{
    public class BSamLocation : MarshalByRefObject, IBSamLocation
    {
        private IBSamRepository BSamRepository = RepositoryFactory.GetInstance().GetRepository<IBSamRepository>();
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //IList<BSamLocation> GetBSamLocation(BSamLocationQueryType type,string model)

        public IList<BSamLocaionInfo> GetBSamLocation(BSamLocationQueryType type, string model)
        {
            logger.Debug("(BSamLocation)GetBSamLocation starts");
            try
            {
                IList<IMES.FisObject.PAK.BSam.BSamLocation> bsLocList= BSamRepository.GetBSamLocation(type, model);
                var loc = (from p in bsLocList
                         select new BSamLocaionInfo {
                             LocationId = p.LocationId,
                             Model = p.Model,
                             Qty = p.Qty,
                             RemainQty = p.RemainQty,
                             FullQty = p.FullQty,
                             FullCartonQty = p.FullCartonQty,
                             HoldInput = p.HoldInput,
                             HoldOutput = p.HoldOutput,
                             Editor = p.Editor,
                             Cdt = p.Cdt,
                             Udt = p.Udt});

                return loc.ToList();
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
                logger.Debug("(BSamLocation)GetBSamLocation end");
            }
        }

        public void UpdateHoldInLocation(IList<string> Ids, bool isHold, string editor)
        {
            logger.Debug("(BSamLocation)UpdateHoldInLocation starts");
            try
            {
                BSamRepository.UpdateHoldInLocation(Ids,isHold,editor);
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
                logger.Debug("(BSamLocation)UpdateHoldInLocation end");
            }
        }

        public void UpdateHoldOutLocation(IList<string> Ids, bool isHold, string editor)
        {
            logger.Debug("(BSamLocation)GetBSamLocation starts");
            try
            {
                BSamRepository.UpdateHoldOutLocation(Ids, isHold, editor);
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
                logger.Debug("(BSamLocation)GetBSamLocation end");
            }
        }


        public IList<string> GetAllBSamModel()
        {
            logger.Debug("(BSamLocation)GetAllBSamModel starts");
            try
            {
                return BSamRepository.GetAllBSamModel();
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
                logger.Debug("(BSamLocation)GetAllBSamModel end");
            }
        }
    }
}

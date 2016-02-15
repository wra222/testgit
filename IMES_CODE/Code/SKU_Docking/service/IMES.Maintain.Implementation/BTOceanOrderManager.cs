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
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;
using log4net;
using IMES.Infrastructure.Repository;
using IMES.FisObject.Common.FisBOM;


namespace IMES.Maintain.Implementation
{
    public class BTOceanOrderManager : MarshalByRefObject, IBTOceanOrder
    {
        #region IBTOceanOrder Members
        private IBOMRepository iplr = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IList<BTOceanOrder> getAllBTOceanOrder()
        {
            try
            {
                IList<BTOceanOrder> tmpBTOceanOrderList = iplr.GetAllBTOceanOrder();
                return tmpBTOceanOrderList;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }


        }

        public IList<BTOceanOrder> getListByPdLineAndModel(string pdLine, string model)
        {
            try
            {

                IList<BTOceanOrder> tmpBTOceanOrderList = iplr.GetListByPdLineAndModel(pdLine, model);
                return tmpBTOceanOrderList;

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        public void addBTOceanOrder(BTOceanOrder obj)
        {
            try
            {

                if (iplr.GetListByPdLineAndModel(obj.pdLine, obj.model).Count <= 0)
                {
                    BTOceanOrder item = new BTOceanOrder();
                    item.pdLine = obj.pdLine;
                    item.model = obj.model;
                    item.editor = obj.editor;
                    DateTime dnow = DateTime.Now;
                    item.cdt = dnow;
                    item.udt = dnow;
                    iplr.AddBTOceanOrder(item);
                }
                else
                {
                    //已经存在具有相同pdlien和model的BToceanOrder记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT085", erpara);
                    throw ex;
                }
            }

            catch (Exception)
            {

                throw;
            }
        }

        public void deleteBTOceanOrderByPdlineAndModel(string pdLine, string model)
        {
            try
            {
                iplr.DeleteBTOceanOrderByPdlineAndModel(pdLine, model);

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        public void updateBTOceanOrderbyPdlineAndModel(BTOceanOrder obj, string pdLine, string model)
        {
            try
            {

                if (iplr.GetListByPdLineAndModel(obj.pdLine, obj.model).Count <= 0)
                {
                    BTOceanOrder btoor = new BTOceanOrder();
                    btoor.pdLine = obj.pdLine;
                    btoor.model = obj.model;
                    btoor.editor = obj.editor;
                    btoor.udt = obj.udt;
                    iplr.UpdateBTOceanOrderbyPdlineAndModel(btoor, pdLine, model);
                }
                else
                {   //已经存在具有相同pdlien和model的BToceanOrder记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT085", erpara);
                    throw ex;
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        #endregion
    }
}

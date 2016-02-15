using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.FA.Product;
using System.Data.SqlClient;
using System.Data;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UpdateConsolidate : MarshalByRefObject, IUpdateConsolidate
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       // private static readonly Session.SessionType theType = Session.SessionType.Common;


        #region IUpdateConsolidate Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consolidate"></param>
        /// <returns></returns>
        public DataTable GetAbnormalConsolidate(string consolidate)
        {
            DataTable dt = new DataTable();

            SqlParameter[] paramsArray = new SqlParameter[1];
            paramsArray[0] = new SqlParameter("Consolidate", consolidate);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            dt = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_PAK, "op_UpdateConsolidate", paramsArray);
            
            return dt;
        }

        public ArrayList Update(string pdline, string station, string editor, string customer, string consolidate, string actqty)
        {
            try
            {
                ArrayList ret = new ArrayList();

                string newConsolidate = String.Empty;
                if (actqty.TrimEnd().Length == 1)
                {
                    newConsolidate = consolidate.TrimEnd() + "/ " + actqty.TrimEnd();
                }
                else
                {
                    newConsolidate = consolidate.TrimEnd() + "/" + actqty.TrimEnd();
                }
                
                IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                UnitOfWork uow = new UnitOfWork();
                
                DeliveryRepository.InsertDeliveryAttrLogDefered(uow, newConsolidate, editor, consolidate);
                DeliveryRepository.UpdateDeliveryInfoValueByInfoTypeAndInfoValuePrefixDefered(uow, newConsolidate, "Consolidated", consolidate, editor);
                DeliveryRepository.UpdateConsolidateQtyInDeliveryExDefered(uow, consolidate, int.Parse(actqty.Trim()), editor);
                uow.Commit();
            
                return ret;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }
        #endregion


    }
}

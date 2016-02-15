/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: OQCOutputImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-24   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using log4net;
using IMES.Route;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// SpecialModelForItcnd接口的实现类
    /// </summary>
    public class SpecialModelForItcndImpl : MarshalByRefObject, SpecialModelForItcnd 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region SpecialModelForItcnd members

        /// <summary>
        /// 输入Model 相关信息并处理
        /// </summary>
        /// <param name="Family">Family</param>
        /// <param name="Model">Model</param>
        /// <param name="Type">Type</param>
        /// <returns>prestation</returns>
        /// 
        public ArrayList Query(string Family, string Model, string Type) 
        {
            logger.Debug("(SpecialModelForItcndImpl)Query start [Family]: " + Family
                + " [Model]:" + Model
                + " [SpecialType]:" + Type);

            ArrayList lstRet = new ArrayList();

            IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            try
            {
                TsModelInfo item = new TsModelInfo();
                item.model = Model; //check
                item.mark = Type;
                IList<TsModelInfo> tmp = repProduct.GetTsModelList(item);
                /*IList<TsModelInfo> tmp = new List<TsModelInfo>();
                TsModelInfo t = new TsModelInfo();
                t.editor = "111";
                tmp.Add(t);


                if (tmp.Count != 0)  */
                    lstRet.Add(tmp);

              

                return lstRet; 
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(SpecialModelForItcndImpl)InputProdId end, [Family]: " + Family
                            + " [Model]:" + Model
                            + " [SpecialType]:" + Type);
            }
        }

        public void Insert(string Family, string Model, string Type, string user,out string qcStatus )
        {
            logger.Debug("(SpecialModelForItcndImpl)Insert start [Family]: " + Family
               + " [Model]:" + Model
               + " [SpecialType]:" + Type);

            IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        

            try
            {
                qcStatus = "";
                TsModelInfo item_select = new TsModelInfo();
                item_select.model = Model; //check
                //item_select.mark = Type;
                IList<TsModelInfo> tmp = repProduct.GetTsModelList(item_select);
                if (tmp.Count > 0) {
                    qcStatus = "false";
                   /* List<string> erpara = new List<string>();
                    erpara.Add("错误，此Model已设置，不能重复设置");
                    e = new FisException("CHK020", erpara);
                    throw e;*/
                    return ;
                }


                TsModelInfo item = new TsModelInfo();
                item.model = Model; ///check
                item.mark = Type;
                item.editor = user;
                repProduct.InsertTSModel(item);
                
            }
            catch (FisException ex)
            {
                logger.Error(ex.mErrmsg);
                throw ex;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
            finally
            {
                logger.Debug("(SpecialModelForItcndImpl)Insert end, [Family]: " + Family
                            + " [Model]:" + Model
                            + " [SpecialType]:" + Type);
            }
        }

        public void Delete(string Family, string Model, string Type)
        {
            logger.Debug("(SpecialModelForItcndImpl)delete start [Family]: " + Family
                + " [Model]:" + Model
                + " [SpecialType]:" + Type) ;

            IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            try
            {
                repProduct.DeleteTsModel(Type,Model);  //check 
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(SpecialModelForItcndImpl)delete end, [Family]: " + Family
                             + " [Model]:" + Model
                             + " [SpecialType]:" + Type);
            }

            
        }
        #endregion
    }
}

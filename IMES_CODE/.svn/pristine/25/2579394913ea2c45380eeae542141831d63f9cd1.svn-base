using System;
using System.Collections.Generic;
using System.Collections;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckBOMForWWAN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBOMForWWAN()
        {
            InitializeComponent();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
                if (product == null)
                {
                    throw new NullReferenceException("Product in session is null");
                }
                bool bExist = false;
                bExist = bomRepository.CheckExistModelBOMByMaterialAndPartDescrLike(product.Model, "WWAN");
                if (bExist == true)
                {
                    CurrentSession.AddValue(Session.SessionKeys.Action, 1);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "WWAN Label Print");
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, product.ProId);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, product.ProId);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, product.ProId);
                }
                else
                {
                    CurrentSession.AddValue(Session.SessionKeys.Action, 0);
                }
                
                return base.DoExecute(executionContext);
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

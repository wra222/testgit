using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UpdateForceNWC : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateForceNWC()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查是否满足complete条件, 更新Repair状态
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                IProduct iProductTarget = (IProduct)GetRepairTarget();
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                string returnStation = (string)CurrentSession.GetValue(Session.SessionKeys.ReturnStation);
                var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
                if (product == null)
                {
                    throw new NullReferenceException("Product in session is null");
                }
                string id = product.ProId;

                #region mark by Vincent
                //string isReturnProduct = (string)CurrentSession.GetValue("ReturnProduct");
                //if (isReturnProduct == "1")
                //{
                //    //获取上一站@NextStation
                //    //select top 1 Station from ProductLog where ProductID = @ProductID 
                //    //        and Station <>'76' and Station <>'7P' and Station<>’45’ order by Cdt desc
                //    var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                //    IList<ProductLog> list = new List<ProductLog>();
                //    ProductLog cond1 = new ProductLog();
                //    cond1.ProductID = product.ProId;
                //    string[] stationArray = new string[6];
                //    stationArray[0] = "76";
                //    stationArray[1] = "7P";
                //    stationArray[2] = "45";
                //    //Update 2012/07/23 Add 57
                //    stationArray[3] = "57";
                //    stationArray[4] = "66";
                //    stationArray[5] = "6P";
                //    list = productRepository.GetProductLogList(cond1, stationArray);
                //    if (list != null && list.Count > 0)
                //    {
                //        returnStation = list[0].Station;
                //        CurrentSession.AddValue(Session.SessionKeys.ReturnStation, returnStation);
                //    }
                //    else
                //    {
                //        //CurrentSession.AddValue(Session.SessionKeys.ReturnStation, "40");
                //        List<string> errpara = new List<string>();
                //        errpara.Add(product.ProId);
                //        throw new FisException("CHK925", errpara);
                //    }

                //}
                #endregion

                bool bExist = false;
                ForceNWCInfo cond = new ForceNWCInfo();
                cond.productID = id;
                bExist = partRep.CheckExistForceNWC(cond);
                if (bExist == true)
                {
                    partRep.UpdateForceNWCByProductID(returnStation, this.Station, id);
                }
                else
                {
                    ForceNWCInfo newinfo = new ForceNWCInfo();
                    newinfo.editor = this.Editor;
                    newinfo.forceNWC = returnStation;
                    newinfo.preStation = this.Station;
                    newinfo.productID = id;
                    partRep.InsertForceNWC(newinfo);
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

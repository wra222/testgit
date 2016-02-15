using System;
using System.Collections.Generic;
using System.Collections;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.DN;
using IMES.DataModel;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckStationForShiptocarton : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckStationForShiptocarton()
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
                IProduct currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                if (currentProduct != null)
                {
                    int flag = (int)CurrentSession.GetValue(Session.SessionKeys.IsBT);
                    

                    if(flag == 1)//BT
                    {
                        bool bFind = false;
                        IList<ProductLog> allLogs = new List<ProductLog>();
                        allLogs = currentProduct.ProductLogs;
                        if (allLogs != null && allLogs.Count > 0)
                        {
                            foreach (ProductLog temp in allLogs)
                            {
                                if (temp.Station == "97" && temp.Status == StationStatus.Pass)
                                {
                                    bFind = true;
                                    break;
                                }
                            }
                        }
                        if (bFind == false)
                        {
                            List<string> errpara = new List<string>();
                            throw new FisException("CHK842", errpara);
                        }
                        /*
                        if(!(currentProduct.Status.StationId == "97" || currentProduct.Status.StationId == "99"))
                        {
                            List<string> errpara = new List<string>();                            
                            throw new FisException("CHK842", errpara);
                        }*/
                    }
                    else
                    {
                        bool bFind = false;
                        IList<ProductLog> allLogs = new List<ProductLog>();
                        allLogs = currentProduct.ProductLogs;
                        if (allLogs != null && allLogs.Count > 0)
                        {
                            foreach (ProductLog temp in allLogs)
                            {
                                if (temp.Station == "95" && temp.Status == StationStatus.Pass)
                                {
                                    bFind = true;
                                    break;
                                }
                            }
                        }
                        if (bFind == false)
                        {
                            List<string> errpara = new List<string>();
                            throw new FisException("CHK842", errpara);
                        }
                        /*
                        if (!(currentProduct.Status.StationId == "95" || currentProduct.Status.StationId == "85" ||
                            currentProduct.Status.StationId == "99"))
                        {
                            List<string> errpara = new List<string>();                            
                            throw new FisException("CHK842", errpara);
                        }*/
                    }                         
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

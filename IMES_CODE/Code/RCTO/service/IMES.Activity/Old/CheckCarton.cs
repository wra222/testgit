// INVENTEC corporation (c)2010 all rights reserved. 
// Description:
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-04-02   Tong.Zhi-Yong                create
// 2011-04-18   Tong.Zhi-Yong                Modify ITC-1268-0109
// 2011-04-29   Tong.Zhi-Yong                Modify ITC-1268-0172
// Known issues:

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.BoxerBookData;
using IMES.FisObject.PCA.TestBoxDataLog;
using IMES.FisObject.PAK.Pallet;

namespace IMES.Activity
{
    /// <summary>
    ///   
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///			
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          
    ///     
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
	///										
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    ///         
    /// </para> 
    /// </remarks>
    
    public partial class CheckCarton: BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckCarton()
		{
			InitializeComponent();
		}

        public static DependencyProperty StopWFProperty = DependencyProperty.Register("StopWF", typeof(bool), typeof(CheckCarton));

        [DescriptionAttribute("StopWFProperty")]
        [CategoryAttribute("StopWFProperty Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool StopWF
        {
            get
            {
                return ((bool)(base.GetValue(CheckCarton.StopWFProperty)));
            }
            set
            {
                base.SetValue(CheckCarton.StopWFProperty, value);
            }
        }

        /// <summary>
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                var cartonNo = CurrentSession.GetValue(Session.SessionKeys.Carton).ToString();
                List<string> productIDList = productRepository.GetProductIDListByCarton(cartonNo);
                var deliveryNo = CurrentSession.GetValue(Session.SessionKeys.DeliveryNo).ToString();
                var palletNo = CurrentSession.GetValue(Session.SessionKeys.PalletNo).ToString();
                IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IProduct product = null;
                //BoxerBookData bookData = null;
                TestBoxDataLog testBoxDataLog = null;
                IList<IProduct> lstProduct = new List<IProduct>();
                IList<string> lstProductModel = new List<string>();

                if (!(bool)this.CurrentSession.GetValue("IsFirst"))
                {
                    IList<DeliveryPallet> dnpltlist = palletRepository.GetDeliveryPalletByDNAndPallet(deliveryNo, palletNo);
                    if (dnpltlist == null || dnpltlist.Count == 0)
                    {

                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(cartonNo);
                        ex = new FisException("CHK175", erpara);
                        throw ex;
                    }

                }
                if (productIDList == null || productIDList.Count == 0)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    //erpara.Add(sessionKey);
                    ex = new FisException("CHK105", erpara);
                    throw ex;
                }

                product = productRepository.Find(productIDList[0]);

                if (string.Compare(product.DeliveryNo, deliveryNo) != 0)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    //erpara.Add(sessionKey);
                    ex = new FisException("CHK157", erpara);
                    ex.stopWF = StopWF;
                    throw ex;
                }

                //ITC-1268-0109 Tong.Zhi-Yong 2011-04-18
                foreach (string productID in productIDList)
                {
                    product = productRepository.Find(productID);

                    if (!string.IsNullOrEmpty(product.PalletNo))
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(cartonNo);
                        ex = new FisException("CHK159", erpara);
                        throw ex;
                    }

                    lstProduct.Add(product);
                    lstProductModel.Add(product.Model);
                }

                if (!(bool)this.CurrentSession.GetValue("IsFirst"))
                {
                    List<IProduct> prdlist = (List<IProduct>)CurrentSession.GetValue(Session.SessionKeys.ProdList);
                    int dnpacked = 0;
                    IList<string> cartonlist = new List<string>();  
                    foreach (Product prod in prdlist)
                    {
                        if ((prod.DeliveryNo == product.DeliveryNo) && (!cartonlist.Contains(product.CartonSN)))
                        {
                            dnpacked = dnpacked + 1;
                            cartonlist.Add(product.CartonSN); 
                        }
                    }
                    Delivery thisDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
                    if (!(thisDelivery.GetDNPalletQty(palletNo) > dnpacked))
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(palletNo);
                        erpara.Add(deliveryNo);
                        ex = new FisException("CHK176", erpara);
                        throw ex;
                    }
                }
                
                foreach (IProduct p in lstProduct)
                {
                    //bookData = productRepository.GetNewBoxerBookDataByCustSN(p.CUSTSN);
                    //checkBookData(bookData);
                    //ITC-1268-0172 Tong.Zhi-Yong 2011-04-29
                    testBoxDataLog = productRepository.GetTestBoxDataLogByCustSN(p.CUSTSN);
                    checkTestBoxDataLog(testBoxDataLog);
                }

                if (CurrentSession.GetValue(Session.SessionKeys.ProdList) != null)
                {
                    ((List<IProduct>)CurrentSession.GetValue(Session.SessionKeys.ProdList)).AddRange(lstProduct);
                }
                else
                {
                    CurrentSession.AddValue(Session.SessionKeys.ProdList, lstProduct);
                }

                if (CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList) != null)
                {
                    ((List<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList)).AddRange(productIDList);
                }
                else
                {
                    CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, productIDList);
                }

                if (CurrentSession.GetValue(Session.SessionKeys.NewScanedProductModelList) != null)
                {
                    ((List<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductModelList)).AddRange(lstProductModel);
                }
                else
                {
                    CurrentSession.AddValue(Session.SessionKeys.NewScanedProductModelList, lstProductModel);
                }
            }
            catch (FisException ex)
            {
                ex.stopWF = StopWF;
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return base.DoExecute(executionContext);
        }

        //private void checkBookData(BoxerBookData bookData)
        //{
        //    if (bookData == null)
        //    {
        //        FisException ex;
        //        List<string> erpara = new List<string>();
        //        //erpara.Add(sessionKey);
        //        ex = new FisException("CHK158", erpara);
        //        ex.stopWF = false;
        //        throw ex;
        //    }

        //    //CartonSN/PublicKey/MACAddress/PrivateKey/EAN
        //    if (string.IsNullOrEmpty(bookData.CartonSN) || string.IsNullOrEmpty(bookData.PublicKey)
        //        || string.IsNullOrEmpty(bookData.MACAddress) || string.IsNullOrEmpty(bookData.PrivateKey)
        //        || string.IsNullOrEmpty(bookData.EAN))
        //    {
        //        FisException ex;
        //        List<string> erpara = new List<string>();
        //        //erpara.Add(sessionKey);
        //        ex = new FisException("CHK158", erpara);
        //        ex.stopWF = false;
        //        throw ex;
        //    }
        //}

        private void checkTestBoxDataLog(TestBoxDataLog testBoxDataLog)
        {
            if (testBoxDataLog == null)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                //erpara.Add(sessionKey);
                ex = new FisException("CHK158", erpara);
                ex.stopWF = false;
                throw ex;
            }

            //CartonSN/PublicKey/MACAddress/PrivateKey/EAN
            if (string.IsNullOrEmpty(testBoxDataLog.CartonSn) || string.IsNullOrEmpty(testBoxDataLog.PublicKey)
                || string.IsNullOrEmpty(testBoxDataLog.MACAddress) || string.IsNullOrEmpty(testBoxDataLog.PrivateKey)
                || string.IsNullOrEmpty(testBoxDataLog.EAN))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                //erpara.Add(sessionKey);
                ex = new FisException("CHK158", erpara);
                ex.stopWF = false;
                throw ex;
            }
        }
	}
}

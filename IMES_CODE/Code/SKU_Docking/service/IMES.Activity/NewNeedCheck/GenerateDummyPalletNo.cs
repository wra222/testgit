// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据Session中的DummyPalletCase,产生DummyPalletNo，并放到Session中
// UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
// UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx                                                 
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-28   Chen Xu (itc208014)          create
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
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.FA.Product;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Activity
{
    /// <summary>
    /// 保存产生的DummyPalletNo
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    ///         Pallet Verify FDE Only:
    ///         For Case NA Non Dummy Pallet 和Case BA Non Dummy Pallet，还需要产生Dummy Pallet No，并保存与PRODUCT 的关系至IMES_PAK..Dummy_ShipDet 表
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///        
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.VirtualMOList
    ///         Session.ModelName
    ///        
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///       Session.GenerateDummyPalletNo
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///     
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///        
    /// </para> 
    /// </remarks>
    public partial class GenerateDummyPalletNo : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public GenerateDummyPalletNo()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Generate DummyPalletNo, and Save the Relation with Product in the IMES_PAK..Dummy_ShipDet Table
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();
          //  Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
          //  string productId = currentProduct.ProId;
          //  绑定的是每一个刷入的CustSN的ProductID与DummyPalletNo的关系 
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string> ScanedProductIDList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);

            IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            string DPC = (string)CurrentSession.GetValue(Session.SessionKeys.DummyPalletCase);
            string dummyPalletNo = (string)CurrentSession.GetValue(Session.SessionKeys.DummyPalletNo);
            string generateDummyPalletNo = string.Empty;


            if (DPC == "NA" || DPC == "BA")
            {
                //DummyShipDetInfo currentDummyshipdet = (DummyShipDetInfo)CurrentSession.GetValue(Session.SessionKeys.DummyShipDet);

                //currentDummyshipdet.bol = dummyPalletNo;
                //currentDummyshipdet.editor = this.Editor;
                //currentDummyshipdet.cdt = DateTime.Now;
               
                ////update:
                //DummyShipDetInfo condDummyshipdet = new DummyShipDetInfo();
                //condDummyshipdet.snoId = currentDummyshipdet.snoId;
                //iDeliveryRepository.UpdateDummyShipDetInfo(currentDummyshipdet, condDummyshipdet);

            }

            else if (DPC == "NAN" || DPC == "BAN")
            {
                DummyShipDetInfo dummyshipdet = new DummyShipDetInfo();
                IList<DummyShipDetInfo> dummyshipdetList = null;

                //---- NumberControl ----
                INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
                NumControl currentMaxNum = numCtrlRepository.GetMaxValue("Dummy", "DummyPalletNo");
                if (currentMaxNum == null)
                {
                    // 从数据库DummyShip_Det表获取当前最大值
                    dummyshipdetList = iDeliveryRepository.GetDummyShipDetInfoList(dummyshipdet); //拿全集

                    if (dummyshipdetList == null || dummyshipdetList.Count <= 0)
                    {
                        //generateDummyPalletNo = "9000000001";
                        //产线确认修改生产号规则：流水号初始值十进制   60000000
                        generateDummyPalletNo = "90" + "60000000";
                    }
                    else
                    {
                        int max = 0;
                        foreach (DummyShipDetInfo iDSD in dummyshipdetList)
                        {
                            if (iDSD.plt != null && iDSD.plt != "")
                            {
                                if (iDSD.plt.Length != 10)
                                {
                                    erpara.Add(iDSD.plt);
                                    ex = new FisException("PAK071", erpara);
                                    throw ex;
                                }

                                if (iDSD.plt.Substring(0, 2) != "90")
                                {
                                    erpara.Add(iDSD.plt);
                                    ex = new FisException("PAK048", erpara);
                                    throw ex;
                                }

                                Boolean exitLetter = Regex.Matches(iDSD.plt.Substring(2, 8), "[a-zA-Z]").Count > 0;

                                if (exitLetter)
                                {
                                    erpara.Add(iDSD.plt);
                                    ex = new FisException("PAK109", erpara);
                                    throw ex;
                                }

                                int temp = Convert.ToInt32(iDSD.plt.Substring(2, 8));
                                if (temp > max)
                                {
                                    max = temp;
                                }
                            }
                        }
                        if (max == 0)
                        {
                            generateDummyPalletNo = "90" + "60000000";
                        }
                        else
                        {
                            max = max + 1;
                            string maxString = max.ToString();
                            int maxlength = maxString.Length;
                            if (maxlength < 8)
                            {
                                int len = 8 - maxlength;
                                for (int i = 0; i < len; i++)
                                {
                                    maxString = "0" + maxString;
                                }
                            }
                            generateDummyPalletNo = "90" + maxString;
                        }
                    }

                    currentMaxNum = new NumControl();
                    currentMaxNum.NOName = "DummyPalletNo";
                    currentMaxNum.NOType = "Dummy";
                    currentMaxNum.Value = generateDummyPalletNo;
                    currentMaxNum.Customer = this.Customer;

                    IUnitOfWork uof = new UnitOfWork();
                    numCtrlRepository.InsertNumControlDefered(uof, currentMaxNum);
                    uof.Commit();
                    SqlTransactionManager.Commit();
                }
                else
                {
                    if (currentMaxNum.Value.Length != 10)
                    {
                        erpara.Add(currentMaxNum.Value);
                        ex = new FisException("PAK071", erpara);
                        throw ex;
                    }

                    if (currentMaxNum.Value.Substring(0, 2) != "90")
                    {
                        erpara.Add(currentMaxNum.Value);
                        ex = new FisException("PAK048", erpara);
                        throw ex;
                    }

                    int temp = Convert.ToInt32(currentMaxNum.Value.Substring(2, 8));
                    temp = temp + 1;
                    string maxtemp = temp.ToString();
                    int maxlength = maxtemp.Length;
                    if (maxlength < 8)
                    {
                        int len = 8 - maxlength;
                        for (int i = 0; i < len; i++)
                        {
                            maxtemp = "0" + maxtemp;
                        }
                    }
                    generateDummyPalletNo = "90" + maxtemp;

                
                    //currentMaxNum = new NumControl();
                    //currentMaxNum.NOName = "DummyPalletNo";
                    //currentMaxNum.NOType = "Dummy";
                    currentMaxNum.Value = generateDummyPalletNo;
                    //currentMaxNum.Customer = this.Customer;

                    IUnitOfWork uof = new UnitOfWork();

                    numCtrlRepository.Update(currentMaxNum, uof);
                    uof.Commit();
                    SqlTransactionManager.Commit();
                }

                //---- NumberControl ----

                // 每一个刷入的CustSN的ProductID与都要与该生成的DummyPalletNo绑定，并且bol应记录BOL (For NAN: dummyPalletNo = ideliveryRepository.GetDeliveryInfoValue(deliveryNo, "BOL");)
                foreach (string iprodId in ScanedProductIDList)
                {
                    dummyshipdet = new DummyShipDetInfo();

                    dummyshipdet.plt = generateDummyPalletNo;
                    dummyshipdet.snoId = iprodId;
                    dummyshipdet.bol = dummyPalletNo;
                    dummyshipdet.editor = this.Editor;
                    dummyshipdet.cdt = DateTime.Now;
                    dummyshipdet.udt = DateTime.Now;

                    // insert:
                    iDeliveryRepository.InsertDummyShipDetInfoDefered(CurrentSession.UnitOfWork, dummyshipdet);
                }

                
            }
            

            CurrentSession.AddValue(Session.SessionKeys.GenerateDummyPalletNo, generateDummyPalletNo);
           
            return base.DoExecute(executionContext);
        }
	}
}

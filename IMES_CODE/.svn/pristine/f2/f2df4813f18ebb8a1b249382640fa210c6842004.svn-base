// INVENTEC corporation (c)2012 all rights reserved. 
// Description: 根据Session中保存的MB产生指定机型的MAC
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-12   Yuan XiaoWei                 create
// 2012-02-14   Yuan XiaoWei                 ITC-1360-0418
// 2012-02-14   Yuan XiaoWei                 ITC-1360-0419
// 2012-02-14   Yuan XiaoWei                 ITC-1360-0420
// Known issues:
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.UnitOfWork;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// In FA  or FA Stage get MAC
    /// </summary>
     public partial class AcquireMACInProduct: BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public AcquireMACInProduct()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  PartInfo.InfoType
        /// </summary>
        public static DependencyProperty ModelInfoNameProperty = DependencyProperty.Register("ModelInfoName", typeof(string), typeof(AcquireMACInProduct), new PropertyMetadata("WIFIMAC"));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("ModelInfo Name")]
        [CategoryAttribute("InArguments Of AcquireMACInProduct")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ModelInfoName
        {
            get
            {
                return ((string)(base.GetValue(ModelInfoNameProperty)));
            }
            set
            {
                base.SetValue(ModelInfoNameProperty, value);
            }
        }

        /// <summary>
        ///  MACRange.Code
        /// </summary>
        public static DependencyProperty MACRangeCodeProperty = DependencyProperty.Register("MACRangeCode", typeof(string), typeof(AcquireMACInProduct), new PropertyMetadata("WIFICUST"));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("MAC Range Code")]
        [CategoryAttribute("InArguments Of AcquireOtherMAC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string MACRangeCode
        {
            get
            {
                return ((string)(base.GetValue(MACRangeCodeProperty)));
            }
            set
            {
                base.SetValue(MACRangeCodeProperty, value);
            }
        }


        /// <summary>
        ///  Session Name store generated MAC
        /// </summary>
        public static DependencyProperty StoreSessionNameProperty = DependencyProperty.Register("StoreSessionName", typeof(string), typeof(AcquireMACInProduct), new PropertyMetadata("WIFIMAC"));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("Store Session Name")]
        [CategoryAttribute("InArguments Of AcquireOtherMAC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string StoreSessionName
        {
            get
            {
                return ((string)(base.GetValue(StoreSessionNameProperty)));
            }
            set
            {
                base.SetValue(StoreSessionNameProperty, value);
            }
        }

        /// <summary>
        ///  Store PCBInfoType
        /// </summary>
        public static DependencyProperty StoreProdInfoTypeProperty = DependencyProperty.Register("StoreProdInfoType", typeof(string), typeof(AcquireMACInProduct), new PropertyMetadata("WIFIMAC"));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("Store ProdInfoType")]
        [CategoryAttribute("InArguments Of AcquireOtherMAC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string StoreProdInfoType
        {
            get
            {
                return ((string)(base.GetValue(StoreProdInfoTypeProperty)));
            }
            set
            {
                base.SetValue(StoreProdInfoTypeProperty, value);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            IProduct prod = session.GetValue(Session.SessionKeys.Product) as IProduct;
            if (prod == null)
            {                
                throw new FisException("CHK1027", new List<string> {"Session",Session.SessionKeys.Product }); 
            }

            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();

            IMES.FisObject.Common.Model.Model model = (IMES.FisObject.Common.Model.Model)session.GetValue("NewModel");
            if (model == null)
            {
                model = modelRep.Find(prod.Model);
            }

            if (model == null)
            {
                throw new FisException("CHK928", new string[] { prod.Model });
            }

            session.AddValue("NewModel", model);
          
            string pcbNo = prod.PCBID;           
            if (string.IsNullOrEmpty(pcbNo))
            {
                //未結合MB,不可以分配MAC
                throw new FisException("CHK1027", new List<string> { "MB" });
            }
            string macValue = model.Attributes.Where(x => x.Name == this.ModelInfoName).Select(y => y.Value).FirstOrDefault();
            if (macValue == "T")
            {
                IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                string prodMAC = (string)prod.GetExtendedProperty(this.StoreProdInfoType);
                //if (!string.IsNullOrEmpty(prodMAC))
                // {
                //     //已分配MAC
                //    // throw new FisException("CHK160", new List<string> { this.StoreProdInfoType + " :: " + prodMAC });
                //     session.AddValue(this.StoreSessionName, prodMAC);
                //     return base.DoExecute(executionContext);
                // }

                string cust = model.Attributes.Where(x => x.Name == this.MACRangeCode).Select(y => y.Value).FirstOrDefault();
                if (string.IsNullOrEmpty(cust))
                {
                    throw new FisException("ICT016", new string[] { model.ModelName + "::" + this.MACRangeCode });
                }
                string maxMAC = ActivityCommonImpl.Instance.AstNum.CheckAndSetReleaseAstNumber(prod.ProId, cust, this.StoreProdInfoType, this.Station, this.Editor);
                 if (string.IsNullOrEmpty(maxMAC))
                 {
                      maxMAC = ActivityCommonImpl.Instance.GenMAC.GetMAC(cust, ActivityCommonImpl.Instance.GenMAC.DecideMACOldRule(model.FamilyName));
                      if (string.IsNullOrEmpty(maxMAC))
                      {
                          throw new FisException("ICT008", new string[] { });
                      }

                      CheckBoundWithProd(prod.ProId, maxMAC);
                      ActivityCommonImpl.Instance.AstNum.InsertCombinedAstNumber(session, prod.ProId, cust, this.StoreProdInfoType, maxMAC, this.Station, this.Editor);
                 }
                session.AddValue(this.StoreSessionName, maxMAC);
                prod.SetExtendedProperty(this.StoreProdInfoType, maxMAC, this.Editor);
                prodRep.Update(prod, session.UnitOfWork);

                if (!string.IsNullOrEmpty(prodMAC))
                {
                    //已分配MAC,需release MAC
                    ActivityCommonImpl.Instance.AstNum.ReleaseCombinedAstNumber(session, prod.ProId, this.StoreProdInfoType, prodMAC, this.Station, this.Editor);

                }

            }


            return base.DoExecute(executionContext);
        }

        private void CheckBoundWithProd(string productId,string mac)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            int count = prodRep.GetProductInfoCountByInfoValue(this.StoreProdInfoType, mac);
            if (count > 0)
            {
                throw new FisException("ICT007", new string[] { mac, productId });
            }
        }        

        //private string GetMAC(string cust)
        //{
        //    try
        //    {
        //        SqlTransactionManager.Begin();
        //        lock (_syncRoot_GetSeq)
        //        {
        //            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

        //            MACRange currentRange = numCtrlRepository.GetMACRange(cust, new string[] { "R", "A" });
        //            if (currentRange == null)
        //            {
        //                throw new FisException("ICT014", new string[] { });
        //            }
        //            else
        //            {
        //                NumControl currentMaxNum = numCtrlRepository.GetMaxValue("MAC", cust);
        //                if (currentMaxNum == null)
        //                {
        //                    currentMaxNum = new NumControl();
        //                    currentMaxNum.NOName = cust;
        //                    currentMaxNum.NOType = "MAC";
        //                    currentMaxNum.Value = currentRange.BegNo;
        //                    currentMaxNum.Customer = "";

        //                    IUnitOfWork uof = new UnitOfWork();
        //                    numCtrlRepository.InsertNumControlDefered(uof, currentMaxNum);
        //                    if (currentMaxNum.Value == currentRange.EndNo)
        //                    {
        //                        numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
        //                    }
        //                    else
        //                    {
        //                        numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
        //                    }
        //                    uof.Commit();
        //                    SqlTransactionManager.Commit();
        //                    return currentMaxNum.Value;

        //                }
        //                else
        //                {
        //                    if (currentMaxNum.Value == currentRange.EndNo)
        //                    {
        //                        numCtrlRepository.SetMACRangeStatus(currentRange.ID, MACRange.MACRangeStatus.Closed);
        //                        currentRange = numCtrlRepository.GetMACRange(cust, new string[] { "R" });
        //                        if (currentMaxNum.Value == currentRange.BegNo || currentMaxNum.Value == currentRange.EndNo)
        //                        {
        //                            throw new FisException("ICT018", new string[] { currentMaxNum.Value });
        //                        }
        //                    }

        //                    //0002589: 平板MAC 分配逻辑改善 & 
        //                    //   0002594: 平板在Assign Model站BTMAC和WIFIMAC分配逻辑改善
        //                    currentMaxNum.Value = ActivityCommonImpl.Instance.GetNextAndCheck16Base(currentMaxNum.Value, currentRange.BegNo, currentRange.EndNo);

                            
        //                    #region Vincent disable code  2014-09-15                           

        //                    //string change34MaxNum = Change34(currentMaxNum.Value);
        //                    //string change34BeginNo = Change34(currentRange.BegNo);
        //                    //string change34EndNo = Change34(currentRange.EndNo);
        //                    //if (string.Compare(change34MaxNum, change34BeginNo) < 0 || string.Compare(change34MaxNum, change34EndNo) > 0)
        //                    //{
        //                    //    currentMaxNum.Value = currentRange.BegNo;
        //                    //}
        //                    //else
        //                    //{
        //                    //    ISequenceConverter seqCvt = new SequenceConverterNormal("0123456789ABCDEF", 4, "FFFF", "0000", '0');
        //                    //    string sequenceNumber = currentMaxNum.Value.Substring(currentMaxNum.Value.Length - 4, 4);
        //                    //    string change34Sequence = sequenceNumber.Substring(1, 3).Insert(1, sequenceNumber.Substring(0, 1));
        //                    //    change34Sequence = seqCvt.NumberRule.IncreaseToNumber(change34Sequence, 1);
        //                    //    sequenceNumber = change34Sequence.Substring(1, 3).Insert(1, change34Sequence.Substring(0, 1));

        //                    //    currentMaxNum.Value = currentMaxNum.Value.Substring(0, currentMaxNum.Value.Length - 4) + sequenceNumber;
        //                    //}

        //                    #endregion

        //                    IUnitOfWork uof = new UnitOfWork();
        //                    numCtrlRepository.Update(currentMaxNum, uof);
        //                    if (currentMaxNum.Value == currentRange.EndNo)
        //                    {
        //                        numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
        //                    }
        //                    else
        //                    {
        //                        numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
        //                    }
        //                    uof.Commit();
        //                    SqlTransactionManager.Commit();
        //                    return currentMaxNum.Value;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw e;
        //    }
        //    finally
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }


        //}

        //private static object _syncRoot_GetSeq = new object();

        //private string Change34(string input)
        //{
        //    string sequenceNumber = input.Substring(input.Length - 4, 4);
        //    string change34Sequence = sequenceNumber.Substring(1, 3).Insert(1, sequenceNumber.Substring(0, 1));
        //    return input.Substring(0, input.Length - 4) + change34Sequence;
        //}

    }
}

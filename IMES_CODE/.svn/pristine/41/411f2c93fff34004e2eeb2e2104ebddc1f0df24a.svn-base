// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的ProductID,获取Product对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-02-15   Vincenti                 create
// Known issues:
using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的ProductID,抓取HoldInfo，并放到Session中
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有以Product为主线的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据ProductID，调用IProductRepository的Find方法，获取Product对象，添加到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：SFC002
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         this.Key
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Product
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class GetProductHoldInfo : BaseActivity
    {
        ///<summary>
        ///</summary>
        public GetProductHoldInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get Product Object and Put it into Session.SessionKeys.Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //logger.InfoFormat("GetProductActivity: Key: {0}", this.Key);
            var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var stationRep =RepositoryFactory.GetInstance().GetRepository<IStationRepository>();

            //Get Hold Station
            IList<string> inputHoldStationList = (IList<string>)CurrentSession.GetValue("HoldStationList");


            IList<string> allHoldStationList = stationRep.GetStationByAttrNameValue("IsHold", "Y");
            if (allHoldStationList == null || allHoldStationList.Count == 0)
            {
                //throw error not find Hold Station
                throw new FisException("CQCHK0007", new string[] { });
            }
            //IList<string> tmpListForHlodStationList = new List<string>();
            IList<string> holdStationList = null;
            if (inputHoldStationList != null && inputHoldStationList.Count > 0)
            {

                holdStationList = (from q in allHoldStationList
                                          where inputHoldStationList.Contains(q)
                                          select q).ToList<string>();
                if (holdStationList == null || holdStationList.Count == 0)
                {
                    throw new FisException("CQCHK0007", new string[] { });
                }
            }
            else
            {
                holdStationList = allHoldStationList;
            }

            IList<string> keyValue = null;
            IList<HoldInfo> holdInfoList = null;
            IList<string> productIdList = null;
            string modelName = null;
            switch (InputSessionKey)
            {
                case InputSessionKeyEnum.ProductID:
                    keyValue = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
                    if (keyValue == null || keyValue.Count==0)
                    {
                        throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.NewScanedProductIDList });
                    }
                    holdInfoList = productRep.GetHoldInfoByProdID(keyValue, holdStationList);
                    break;
                case InputSessionKeyEnum.CustSN:
                    keyValue = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.CustomSnList);
                    if (keyValue == null)
                    {
                        throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.CustomSnList });
                    }
                    holdInfoList = productRep.GetHoldInfoByCustSN(keyValue, holdStationList);
                    productIdList = (from p in holdInfoList
                                         select p.ProductID).Distinct().ToList();
                    CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, productIdList);
                    break;
                case InputSessionKeyEnum.ModelName:
                    modelName = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
                    if (string.IsNullOrEmpty(modelName))
                    {
                        IProduct prod = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                        if (prod != null)
                        {
                            modelName = prod.Model;
                        }
                    }

                    if (string.IsNullOrEmpty(modelName))
                    {
                        //throw error not find key value
                        throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.ModelName });
                    }
                    holdInfoList = productRep.GetHoldInfo(modelName, holdStationList);
                    productIdList = (from p in holdInfoList
                                         select p.ProductID).Distinct().ToList();
                    CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, productIdList);
                    break;
                default:
                    keyValue.Add(this.Key);
                    holdInfoList = productRep.GetHoldInfoByProdID(keyValue, holdStationList);
                    productIdList = (from p in holdInfoList
                                     select p.ProductID).Distinct().ToList();
                    CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, keyValue);
                    break;
            }

            if (holdInfoList == null || holdInfoList.Count == 0)
            {
                if (this.NotExistException)
                {
                    //throw error not find HoldInfo
                    throw new FisException("CQCHK0008", new string[] { string.Join(",", holdStationList.ToArray()) });
                }
            }
            CurrentSession.AddValue(ExtendSession.SessionKeys.ProdHoldInfoList, holdInfoList);
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 输入序号找不到MB报错信息的ErroCode,不填则默认报错SFC002
        /// </summary>
        public static DependencyProperty NotExistExceptionProperty = DependencyProperty.Register("NotExistException", 
                                                                                                        typeof(bool), 
                                                                                                        typeof(GetProductHoldInfo), 
                                                                                                        new PropertyMetadata(true));

        /// <summary>
        /// 输入序号找不到MB报错信息的ErroCode,不填则默认报错SFC002
        /// </summary>
        [DescriptionAttribute("NotExistException")]
        [CategoryAttribute("InArguments Of GetProduct")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NotExistException
        {
            get
            {
                return ((bool)(base.GetValue(GetProductHoldInfo.NotExistExceptionProperty)));
            }
            set
            {
                base.SetValue(GetProductHoldInfo.NotExistExceptionProperty, value);
            }
        }

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        public static DependencyProperty InputSessionKeyProperty = DependencyProperty.Register("InputSessionKey", 
                                                                                                                                        typeof(InputSessionKeyEnum), 
                                                                                                                                        typeof(GetProductHoldInfo), 
                                                                                                                                        new PropertyMetadata(InputSessionKeyEnum.Key));

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        [DescriptionAttribute("InputSessionKey")]
        [CategoryAttribute("InArugment Of GetProductHoldInfo")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public InputSessionKeyEnum InputSessionKey
        {
            get
            {
                return ((InputSessionKeyEnum)(base.GetValue(GetProductHoldInfo.InputSessionKeyProperty)));
            }
            set
            {
                base.SetValue(GetProductHoldInfo.InputSessionKeyProperty, value);
            }
        }

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ModelName
        /// </summary>
        public enum InputSessionKeyEnum
        {
            /// <summary>
            /// 输入的是get session key Name ProductID
            /// </summary>
            ProductID = 1,
            /// <summary>
            /// 输入的是get session key Name ModelName
            /// </summary>
            ModelName=2,
            /// <summary>
            /// Session Key Name CustSN
            /// </summary>
            CustSN=3,
            /// <summary>
            /// Session key
            /// </summary>
            Key=4
        }
    }
}

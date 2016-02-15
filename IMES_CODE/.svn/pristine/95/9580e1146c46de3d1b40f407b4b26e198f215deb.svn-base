// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 检查当前MB, Product是否应该进入当前站处理
//                      
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Process;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.Common;
using IMES.Resolve.Common;
namespace IMES.Activity
{
    /// <summary>
    /// Property: ProductId, Station
    /// </summary>
    /// <summary>
    /// 检查当前MB, Product是否应该进入当前站处理
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于以MB, Product为操作主线的workflow
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取当前处理对象的CurrentStation, Result;
    ///         2.根据当前处理对象的Model获取相应的Process对象;
    ///         3.检查当前处理对象的状态是否允许在当站进行处理;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         IProduct
    ///         Pallet
    ///         Process
    ///         IProcessRepository
    /// </para> 
    /// </remarks>
    public partial class BlockStationByProductList : BaseActivity
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public BlockStationByProductList()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(BlockStationByProductList), new PropertyMetadata(true));

        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("InArguments Of SFC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
       
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(IsStopWFProperty)));
            }
            set
            {
                base.SetValue(IsStopWFProperty, value);
            }
        }


        /// <summary>
        ///  ProductListSessionName
        /// </summary>
        public static DependencyProperty ProductListSessionNameProperty = DependencyProperty.Register("ProductListSessionName", typeof(string), typeof(BlockStationByProductList), new PropertyMetadata(Session.SessionKeys.ProdList));

        /// <summary>
        ///  ProductListSessionName
        /// </summary>
        [DescriptionAttribute("ProductList Session Name")]
        [CategoryAttribute("InArguments Of ProductList Session Name")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]

        public string ProductListSessionName
        {
            get
            {
                return ((string)(base.GetValue(ProductListSessionNameProperty)));
            }
            set
            {
                base.SetValue(ProductListSessionNameProperty, value);
            }
        }


        /// <summary>
        /// 根据流程类型执行卡站操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProcessRepository CurrentProcessRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
            string keyOfSFC = Key;
            string notEmpytLine = Line;
             Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //IList<IProduct> prodList =utl.IsNull<IList <IProduct>>( session,Session.SessionKeys.ProdList);
            IList<IProduct> prodList = utl.IsNull<IList<IProduct>>(session,this.ProductListSessionName);
            IProduct currentProduct = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);                
            IList<string> prodNoList = prodList.Select(x => x.ProId).ToList();               

			keyOfSFC = currentProduct.ProId;
			if (currentProduct.Status == null)
			{
				throw new FisException("SFC002", new string[] { keyOfSFC });
			}

			if (string.IsNullOrEmpty(notEmpytLine) && currentProduct.Status != null)
			{
				notEmpytLine = currentProduct.Status.Line;
			}

			if (currentProduct.Status != null && string.IsNullOrEmpty(currentProduct.Status.ReworkCode))
			{
               
                IMES.FisObject.Common.Line.Line line = ActivityCommonImpl.Instance.CheckAndSetLine(session, notEmpytLine, true);
                string aliasLine = line==null?"": line.LineEx.AliasLine;
                var needCheckMode = prodList.Select(x => x.Model).Distinct();
                foreach (var model in needCheckMode)
                {
                    IList<ModelProcess> currentModelProcess = CurrentProcessRepository.GetModelProcessByModelLine(model, aliasLine);
                    if (currentModelProcess == null || currentModelProcess.Count == 0)
                    {
                        //CurrentProcessRepository.CreateModelProcess(model, Editor, aliasLine);
                        ResolveProcess.CreateModelProcess(currentProduct.ModelObj, Editor, aliasLine);
                    }
                }
			}

            try
            {
                var needCheckStationList = prodList.GroupBy(x => new { Model = x.Model, StationId = x.Status.StationId, Status = x.Status.Status })
                                                                  .Select(y => y.First().ProId).Distinct();
                foreach (string prodId in needCheckStationList)
                {
                    CurrentProcessRepository.SFC(notEmpytLine, Customer, Station, prodId, "Product");
                }
                session.AddValue(Session.SessionKeys.ProdNoList, prodNoList);                
            }
            catch (FisException fe)
            {
                fe.stopWF = IsStopWF;
                throw fe;
            }
           

            return base.DoExecute(executionContext);
        }
    }    
}

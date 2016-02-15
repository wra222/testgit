// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 检查当前MB, Product是否应该进入当前站处理
//                      
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:
using System;
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
    public partial class BlockStationByCarton : BaseActivity
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public BlockStationByCarton()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(IsStopWFEnum), typeof(BlockStationByCarton));

        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("InArguments Of SFC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(IsStopWFEnum.Stop)]
        public IsStopWFEnum IsStopWF
        {
            get
            {
                return ((IsStopWFEnum)(base.GetValue(BlockStationByCarton.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(BlockStationByCarton.IsStopWFProperty, value);
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
            try
            {
                IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                IList<IProduct> prodList = null;
                string cartonSN = CurrentSession.GetValue("CartonSN") as string;
                if (!string.IsNullOrEmpty(cartonSN))
                {
                    prodList = prodRep.GetProductListByCartonNo(cartonSN);
                }
                if (string.IsNullOrEmpty(cartonSN) || prodList == null || prodList.Count == 0)
                {
                    throw new FisException("CHK109", new string[] { }); // 沒有對應的Carton!
                }

                IList<string> prodNoList = new List<string>();
				string prodStatusStationId = "";
                foreach (Product p in prodList)
                {
                    string nowStatusStationId = "";
                    if (p.Status != null)
                        nowStatusStationId = p.Status.StationId;
                    if ("" == prodStatusStationId)
                        prodStatusStationId = nowStatusStationId;

                    if ("" == prodStatusStationId || !prodStatusStationId.Equals(nowStatusStationId))
                    {
                        throw new FisException("CHK1034", new string[] { cartonSN }); // Carton %1 的 Products 狀態不一致
                    }
					
					prodNoList.Add(p.ProId);
                }

                Product currentProduct = ((Product)prodList[0]);
                //CurrentSession.AddValue(Session.SessionKeys.ProdList, prodList);
                CurrentSession.AddValue(Session.SessionKeys.ProdNoList, prodNoList);
                CurrentSession.AddValue(Session.SessionKeys.Product, currentProduct);

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
					string firstLine = "";
					if (!string.IsNullOrEmpty(notEmpytLine)){
						firstLine = notEmpytLine.Substring(0, 1);
					}
					IList<ModelProcess> currentModelProcess = CurrentProcessRepository.GetModelProcessByModelLine(currentProduct.Model, firstLine);
					if (currentModelProcess == null || currentModelProcess.Count == 0)
					{
						//CurrentProcessRepository.CreateModelProcess(currentProduct.Model, Editor, firstLine);
                        ResolveProcess.CreateModelProcess(currentProduct.ModelObj, Editor, firstLine);
					}
				}

                try
                {
                    CurrentProcessRepository.SFC(notEmpytLine, Customer, Station, keyOfSFC, "Product");
                }
                catch (FisException )
                {
                    throw new FisException("SFC024", new string[] { currentProduct.ProId });
                }
            }
            catch (FisException fe)
            {
                if (IsStopWF == IsStopWFEnum.NotStop)
                {
                    fe.stopWF = false;
                }
                throw fe;
            }

            return base.DoExecute(executionContext);
        }
    }

}

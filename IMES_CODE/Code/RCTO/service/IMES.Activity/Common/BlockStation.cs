// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 检查当前MB, Product是否应该进入当前站处理
//                      
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-18   Yuan XiaoWei                 create
// 2012-01-30   Yuan XiaoWei                 ITC-1360-0082
// 2012-03-01   Yuan XiaoWei                 ITC-1360-0859
// 2012-03-01   Yuan XiaoWei                 ITC-1360-0861
// 2012-03-01   Yuan XiaoWei                 ITC-1360-0863
// 2012-03-01   Yuan XiaoWei                 ITC-1360-1171
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
    public partial class BlockStation : BaseActivity
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public BlockStation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当前流程的类型，共有两种，MB,Product
        /// </summary>
        public static DependencyProperty ProcessTypeProperty = DependencyProperty.Register("ProcessType", typeof(ProcessTypeEnum), typeof(BlockStation));

        /// <summary>
        /// 当前流程的类型，共有两种，MB,Product
        /// </summary>
        [DescriptionAttribute("ProcessType")]
        [CategoryAttribute("InArguments Of SFC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public ProcessTypeEnum ProcessType
        {
            get
            {
                return ((ProcessTypeEnum)(base.GetValue(BlockStation.ProcessTypeProperty)));
            }
            set
            {
                base.SetValue(BlockStation.ProcessTypeProperty, value);
            }
        }

        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(IsStopWFEnum), typeof(BlockStation));

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
                return ((IsStopWFEnum)(base.GetValue(BlockStation.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(BlockStation.IsStopWFProperty, value);
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
                switch (ProcessType)
                {
                    case ProcessTypeEnum.MB:
                        keyOfSFC = Key;
                        if (string.IsNullOrEmpty(notEmpytLine))
                        {
                            var mb = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;
                            if (mb == null || mb.MBStatus == null)
                            {
                                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                                mb = mbRepository.Find(keyOfSFC);
                                if (mb == null || mb.MBStatus == null)
                                {
                                    throw new FisException("SFC001", new string[] { keyOfSFC });
                                }
                            }
                            notEmpytLine = mb.MBStatus.Line;
                        }
                        break;
                    case ProcessTypeEnum.Product:
                        Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
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

                        break;
                    default:
                        break;
                }

                CurrentProcessRepository.SFC(notEmpytLine, Customer, Station, keyOfSFC, Enum.GetName(typeof(ProcessTypeEnum), ProcessType));
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

    /// <summary>
    /// 当前流程的类型，共有两种，MB,Product
    /// </summary>
    public enum ProcessTypeEnum
    {
        /// <summary>
        /// MB
        /// </summary>
        MB = 1,
        /// <summary>
        /// Product
        /// </summary>
        Product = 2
    }

    /// <summary>
    /// 遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
    /// </summary>
    public enum IsStopWFEnum
    {
        /// <summary>
        /// 停止WorkFlow
        /// </summary>
        Stop = 1,

        /// <summary>
        /// 不停止WorkFlow
        /// </summary>
        NotStop = 2
    }

}

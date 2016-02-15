// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 针对每个Prodcut记录PalletLog表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-03-19   chen xu              create
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
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using System.Collections.Generic;

namespace IMES.Activity
{

    /// <summary>
    /// 用于记录Pallet的过站Log到PalletLog表
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有以Pallet为主线的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取Pallet，调用Pallet的AddLog方法
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Pallet
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
    ///         更新PalletLog  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IPalletRepository
    ///              Pallet
    ///              PalletLog
    /// </para> 
    /// </remarks>
    public partial class WritePalletListLog : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public WritePalletListLog()
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
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string> ScanedProductIDList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
            IList<string> palletList = new List<string>();

            Line = string.IsNullOrEmpty(this.Line) ? string.Empty : this.Line;

            foreach (string iprodId in ScanedProductIDList)
            {

                var currentProduct = productRepository.Find(iprodId);

                if (palletList != null && palletList.Count > 0)
                {
                    Boolean exitFlag = false;
                    foreach (string ipalletno in palletList)
                    {
                        if (ipalletno == currentProduct.PalletNo)
                        {
                            exitFlag = true;
                            break;
                        }

                    }

                    if (!exitFlag)
                    {
                        palletList.Add(currentProduct.PalletNo);
                    }
                }
                else palletList.Add(currentProduct.PalletNo);

            }

            foreach(string ipalletno in palletList)
            {
            
                PalletLog newPalletLog = new PalletLog();

                newPalletLog.PalletNo = ipalletno;
                newPalletLog.Editor = Editor;
                newPalletLog.Line = Line;
                newPalletLog.Station = Station;

                

                IPalletRepository PalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                Pallet CurrentPallet = PalletRepository.Find(ipalletno);

                CurrentPallet.Station = this.Station;
                CurrentPallet.Editor = this.Editor;
                CurrentPallet.Udt = DateTime.Now;

                CurrentPallet.AddLog(newPalletLog);

                PalletRepository.Update(CurrentPallet, CurrentSession.UnitOfWork);
            }
            return base.DoExecute(executionContext);
        }
    }
}

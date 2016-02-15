// INVENTEC corporation (c)2012 all rights reserved. 
// Description:
//      1.检查[Model1]是否存在未进包装（不存在69的成功过站Log）且已经产生CUSTSN（Product.CUSTSN<>''）的Product记录，若不存在记录，则报错：“Model：XXX不存在可以转换的Product
//      2.获取[Current Station]，Station倒序排列
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-06-18   Kerwin                       create
// 2012-06-27   Kerwin                       ITC-1413-0027
// Known issues:
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using System.Data;

namespace IMES.Activity
{

    /// <summary>
    /// 检查Model1是否满足ChangeModel条件
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC Change Model
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     1.检查[Model1]是否存在未进包装（不存在69的成功过站Log）且已经产生CUSTSN（Product.CUSTSN!=''）的Product记录，若不存在记录，则报错：“Model：XXX不存在可以转换的Product
    ///     2.获取[Current Station]，Station倒序排列
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     CHK155
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
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
    ///         MB
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckModel1 : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckModel1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查Model1是否满足ChangeModel条件
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            string model1 = CurrentSession.GetValue(Session.SessionKeys.Model1) as string;

            IModelRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
            DataTable StationTable = CurrentRepository.GetCurrentStationList(model1);
            if (StationTable == null || StationTable.Rows.Count == 0)
            {
                throw new FisException("CHM001", new string[] { model1 });
            }
            else
            {
                List<StationDescrQty> StationList = new List<StationDescrQty>();
                int stationCount = StationTable.Rows.Count;
                for (int i = 0; i < stationCount; i++)
                {
                    StationList.Add(new StationDescrQty(StationTable.Rows[i][0] as string, StationTable.Rows[i][1] as string, (int)StationTable.Rows[i][2]));
                }

                CurrentSession.AddValue(Session.SessionKeys.StationTable, StationList);
            }


            return base.DoExecute(executionContext);
        }

    }
}


// INVENTEC corporation (c)2012 all rights reserved. 
// Description:
//      1.根据Syssetting检查Model1和Model2的各个PN Qty是否一致
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-06-18   Kerwin                       create
// Known issues:
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.FisBOM;

namespace IMES.Activity
{

    /// <summary>
    /// 检查Model1,Model2是否满足ChangeModel条件
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
    ///     1.根据Syssetting检查Model1和Model2的各个PN Qty是否一致
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
    public partial class ChangeModelCheck : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ChangeModelCheck()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查Model1,Model2是否满足ChangeModel条件
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartRepository MyPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string ChangeModelCheckItem = "";
            IList<string> ChangeModelCheckItemList = MyPartRepository.GetValueFromSysSettingByName("ChangeModelCheckItem");
            if (ChangeModelCheckItemList != null && ChangeModelCheckItemList.Count > 0)
            {
                ChangeModelCheckItem = ChangeModelCheckItemList[0];
            }

            if (!string.IsNullOrEmpty(ChangeModelCheckItem))
            {

                string[] CheckItems = ChangeModelCheckItem.Split(new string[] { "~" }, System.StringSplitOptions.RemoveEmptyEntries);

                ModelBomMatch MatchManager = new ModelBomMatch();
                string model1 = CurrentSession.GetValue(Session.SessionKeys.Model1) as string;
                string model2 = CurrentSession.GetValue(Session.SessionKeys.Model2) as string;


                IBOMRepository MyBomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

                IHierarchicalBOM model1Bom = MyBomRepository.GetHierarchicalBOMByModel(model1);

                IHierarchicalBOM model2Bom = MyBomRepository.GetHierarchicalBOMByModel(model2);

                IList<PNQty> model1PNQtyList;
                IList<PNQty> model2PNQtyList;

                foreach (string CheckItem in CheckItems)
                {

                    model1PNQtyList = MatchManager.GetPNQty(CheckItem, model1Bom);
                    model2PNQtyList = MatchManager.GetPNQty(CheckItem, model2Bom);

                    //var query = from model1PnQty in model1PNQtyList
                    //            join model2PnQty in model2PNQtyList on model1PnQty.Pn equals model2PnQty.Pn into match
                    //            from result in match.DefaultIfEmpty()
                    //            where model1PnQty.Qty != result.Qty
                    //            select new PNQty(model1PnQty.Pn, result == null ? -1 : result.Qty);

                    foreach (PNQty tempModel1 in model1PNQtyList)
                    {
                        bool match = false;
                        foreach (PNQty tempModel2 in model2PNQtyList)
                        {
                            if (tempModel2.Pn == tempModel1.Pn)
                            {
                                if (tempModel2.Qty == tempModel1.Qty)
                                {
                                    match = true;
                                    break;
                                }
                                else
                                {
                                    throw new FisException("CHM003", new string[] { model1, model2 });
                                }
                            }
                        }
                        if (!match)
                        {
                            throw new FisException("CHM002", new string[] { model1, model2 });
                        }
                    }


                }
                CurrentSession.AddValue(Session.SessionKeys.ChangeModelCheckItem, CheckItems);
            }


            return base.DoExecute(executionContext);
        }

    }
}


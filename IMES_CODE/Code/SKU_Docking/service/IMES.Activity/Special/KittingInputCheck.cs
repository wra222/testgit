// INVENTEC corporation (c)2012 all rights reserved. 
// Description:
//      增加每颗料的Check
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-06-19   ITC000052                       create
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
    /// 检查是否满足条件
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC Kitting Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     增加每颗料的Check
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
    public partial class KittingInputCheck : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public KittingInputCheck()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查是否满足条件
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartRepository MyPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string KittingInputCheckItem = "";
            IList<string> KittingInputCheckItemList = MyPartRepository.GetValueFromSysSettingByName("FAKittingCheckItem");
            if (KittingInputCheckItemList != null && KittingInputCheckItemList.Count > 0)
            {
                KittingInputCheckItem = KittingInputCheckItemList[0];
            }
            //KittingInputCheckItem = "WWAN~WL~HDD~DDR~CPU~LCM~BAT";
            if (!string.IsNullOrEmpty(KittingInputCheckItem))
            {

                string[] CheckItems = KittingInputCheckItem.Split(new string[] { "~" }, System.StringSplitOptions.RemoveEmptyEntries);

                ModelBomMatch MatchManager = new ModelBomMatch();

                Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
                string model = currentProduct.Model;

                IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IHierarchicalBOM modelBom = bomRep.GetHierarchicalBOMByModel(model);

                IList<PNQty> modelPNQtyList;


                foreach (string CheckItem in CheckItems)
                {
                    modelPNQtyList = MatchManager.GetPNQty(CheckItem, modelBom);

                    IList<string> _PartNo_list = (IList<string>)CurrentSession.GetValue("_PartNo_list");
                    bool flag = false;
                    for (int ii = 0; ii < modelPNQtyList.Count; ii++)
                    {
                        flag =  false;
                        for (int i = 0; i < _PartNo_list.Count; i++)
                        {
                            if (modelPNQtyList[ii].Pn == _PartNo_list[i])
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag == false)
                        {
                            List<string> erpara = new List<string>();
                            erpara.Add(CheckItem);
                            FisException ex;
                            ex = new FisException("CHK285", erpara);
                            throw ex;
                           
                        }
                    }

                }

           }


            return base.DoExecute(executionContext);
        }

    }
}


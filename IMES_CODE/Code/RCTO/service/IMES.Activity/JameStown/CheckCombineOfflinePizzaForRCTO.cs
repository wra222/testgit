// INVENTEC corporation (c)201all rights reserved. 
// Description:  Check CombineOfflinePizzaForRCTO
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// Check CombineOfflinePizzaForRCTO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      mantis 1970
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.
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
    ///         Session.MB
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class CheckCombineOfflinePizzaForRCTO : BaseActivity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CheckCombineOfflinePizzaForRCTO()
        {
            InitializeComponent();
        }

        private int CheckPartMatch(ref IProductPart pp, ref IList<IBOMNode> lstBomsC2)
        {
            int pos = -1;
            for (int i = 0; i < lstBomsC2.Count; i++)
            {
                IBOMNode n = lstBomsC2[i];
                if (pp.PartID.Equals(n.Part.PN)) // case 1, 结合的是PartNo
                {
                    return i;
                }

                foreach (PartInfo pi in n.Part.Attributes)
                {
                    if ("VendorCode".Equals(pi.InfoType))
                    {
                        if (pp.PartSn.Length >= 5 && pp.PartSn.Substring(0, 5).Equals(pi.InfoValue)) // case 2, 结合的是CT
                        {
                            return i;
                        }
                    }
                    if ("SUB".Equals(pi.InfoType))
                    {
                        if (pp.PartID.Equals(pi.InfoValue)) // case 3, 替代料
                        {
                            return i;
                        }
                    }
                }
            }

            return pos;
        }

        /// <summary>
        /// Check CombinePizzaWithCarton
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var PizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            try
            {
                Pizza pizzaObj = null;
                string pizzaNo = CurrentSession.GetValue(Session.SessionKeys.PizzaID) as string;
                var pizzaNoList = CurrentSession.GetValue(Session.SessionKeys.PizzaNoList) as List<string>;
                if (null == pizzaNoList)
                    pizzaNoList = new List<string>();

                if (pizzaNoList.Contains(pizzaNo))
                {
                    throw new FisException("CHK1041", new string[] { }); // 此PizzaID 已經刷入
                }

                if (!string.IsNullOrEmpty(pizzaNo))
                {
                    pizzaObj = PizzaRepository.Find(pizzaNo);
                }
                if (null == pizzaObj)
                {
                    throw new FisException("CHK852", new string[] { }); // 請刷正確的PizzaID
                }

                if (pizzaObj.Status == null || !"PKJROK".Equals(pizzaObj.Status.StationID))
                {
                    throw new FisException("CHK1038", new string[] { }); // 此PizzaID状态错误，不能结合
                }

                if (!string.IsNullOrEmpty(pizzaObj.CartonSN))
                {
                    throw new FisException("CHK1039", new string[] { }); // 此PizzaID已经结合CartonSN
                }

                IProduct currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
                if (currentProduct.Model != pizzaObj.Model)
                {
                    throw new FisException("CHK1040", new string[] { }); // 此PizzaID与SN Model不匹配
                }

                //IList<IProduct> prodList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
                IList<string> prodNoList = (IList<string>) CurrentSession.GetValue(Session.SessionKeys.ProdNoList);
                if ((pizzaObj.PizzaParts == null) || (pizzaObj.PizzaParts.Count != prodNoList.Count))
                {
                    throw new FisException("CHK1042", new string[] { }); // 此PizzaID的料品数量与CartonSN的产品数量不匹配
                }

                string equalPartNo = "";

                int idxPizzaMatch = -1;
                IList<IBOMNode> lstBomsC2 = CurrentSession.GetValue("BomC2") as IList<IBOMNode>;
                IList<string> lstCheckedBomPartNo = CurrentSession.GetValue("CheckedBomPartNo") as IList<string>;
                for (int i = 0; i < pizzaObj.PizzaParts.Count; i++)
                {
                    IProductPart pp = pizzaObj.PizzaParts[i];
                    int idxPartMatch = CheckPartMatch(ref pp, ref lstBomsC2);

                    if (idxPartMatch < 0)
                    {
                        throw new FisException("CHK1044", new string[] { }); // PartNo不匹配
                    }
                    equalPartNo = lstBomsC2[idxPartMatch].Part.PN;

                    idxPizzaMatch = idxPartMatch;
                }

                if (lstCheckedBomPartNo.Contains(equalPartNo))
                {
                    throw new FisException("CHK1043", new string[] { }); // 相同料的Pizza 已經刷入
                }
                lstCheckedBomPartNo.Add(equalPartNo);

                pizzaNoList.Add(pizzaNo);
                CurrentSession.AddValue(Session.SessionKeys.PizzaNoList, pizzaNoList);

                if (pizzaNoList.Count == lstBomsC2.Count)
                {
                    CurrentSession.AddValue(Session.SessionKeys.IsComplete, true);
                }

                CurrentSession.AddValue("CheckedBomPartNo", lstCheckedBomPartNo);

                CurrentSession.AddValue("IdxPizzaMatch", idxPizzaMatch);
            }
            catch (FisException ex)
            {
                ex.stopWF = false;
                throw;
            }

            return base.DoExecute(executionContext);
        }

    }
}

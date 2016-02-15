// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 检查输入的Pizza是否可以解绑
//             CI-MES12-SPEC-PAK-UC Packing Pizza       
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21   zhu lei                      create
// Known issues:
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 检查输入的Pizza是否可以解绑
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于Packing Pizza Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：CHK100
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PizzaID
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Pizza
    /// </para> 
    /// </remarks>
    public partial class CheckUnpackPizza : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckUnpackPizza()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查输入的Pizza是否可以解绑
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var currentPizzaID = (string)CurrentSession.GetValue(Session.SessionKeys.PizzaNoList);

            //判断是否包含”-”字符
            if (currentPizzaID.IndexOf("-") > 0)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                ex = new FisException("CHK101", erpara);
                throw ex;
            }
            else
            {
                Pizza currentpizza = new Pizza();
                PizzaStatus currentPizzaStatus = new PizzaStatus();
                IPizzaRepository currentPizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                IList<PizzaPart> pizzaParts = (IList<PizzaPart>)currentPizzaRepository.GetPizzaPartsByValue(currentPizzaID);
                //按照Value找到一条记录
                if (pizzaParts.Count == 1)
                {
                    var pizzaID = pizzaParts[0].PizzaID.ToString();
                    currentpizza = currentPizzaRepository.Find(pizzaID);
                    currentPizzaStatus = currentpizza.Status;
                    IList<PizzaPart> currentPizzaPart = (IList<PizzaPart>)currentpizza.PizzaParts;
                }
                //按照Value不能找到记录
                else if (pizzaParts.Count == 0)
                {
                    currentpizza = currentPizzaRepository.Find(currentPizzaID);
                //    IList<PizzaPart> currentPizzaPart = (IList<PizzaPart>)currentpizza.PizzaParts;
                    //按照PizzaID能找到记录
               //     if (currentPizzaPart.Count > 0)
                    if (currentpizza!=null)
                    {
                        currentPizzaStatus = currentpizza.Status;
                    }
                    //按照PizzaID和Value都不能找到记录
                    else
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        ex = new FisException("PAK011", erpara);
                        throw ex;
                    }
                }
                //按照Value找到多条记录
                else if (pizzaParts.Count > 1)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(currentPizzaID);
                    ex = new FisException("CHK102", erpara);
                    throw ex;
                }

                IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProduct currentProduct = (IProduct)currentProductRepository.GetProductByPizzaID(currentPizzaID);
                //判断是否已与机器绑定
                if (currentProduct != null && currentProduct.ProId != "")
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("CHK103", erpara);
                    throw ex;
                }
                else
                {
                    var infoType = "KIT2";
                    int count = currentProductRepository.GetProductInfoCountByInfoValue(infoType, currentPizzaID);
                    if (count > 0)
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        ex = new FisException("CHK103", erpara);
                        throw ex;
                    }
                }

                CurrentSession.AddValue(Session.SessionKeys.Pizza, currentpizza);

            }

            return base.DoExecute(executionContext);
        }
    }
}

// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UC: mantis 2156
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 
// Known issues:
using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Station;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace IMES.Activity
{
    /// <summary>
    /// JameStown新机型; Conbime Offline Pizza
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class UnpackOfflinePizza : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public UnpackOfflinePizza()
		{
			InitializeComponent();
		}

        

        private void UpdatePizza(ref IList<string> updatePizzas, string station, string changeCartonSN)
        {
            IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            foreach (string PizzaID in updatePizzas)
            {
                Pizza pizza = pizzaRepository.Find(PizzaID);
                if (pizza == null)
                {
                    throw new NullReferenceException("Pizza in session is null");
                    //continue;
                }

                if (null != changeCartonSN)
                    pizza.CartonSN = changeCartonSN;

                if (null != pizza.Status)
                    pizza.Status.StationID = station;

                string line = string.IsNullOrEmpty(this.Line) ? pizza.Status.LineID : this.Line;
                var PizzaLog = new PizzaLog
                {
                    Model = pizza.Model,
                    Editor = Editor,
                    Line = line,
                    Station = Station,
                    Descr = "",
                    PizzaID = pizza.PizzaID,
                    Cdt = DateTime.Now
                };

                pizza.AddPizzaLog(PizzaLog);
                pizzaRepository.Update(pizza, CurrentSession.UnitOfWork);

            }
        }

        /// <summary>
        /// Unpack Offline Pizza
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            List<string> errpara = new List<string>();

            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            string pizzaId = this.Key;
            Pizza p = pizzaRepository.Find(pizzaId);

            if (null == p)
            {
                var ex = new FisException("CHK100", new string[] { pizzaId });
                throw ex;            }

            if ((null == p.Status) || !("PKJOK".Equals(p.Status.StationID) || "PKJROK".Equals(p.Status.StationID)))
            {
                var ex = new FisException("CHK1046", new string[] { });
                throw ex;
            }

            IProduct prod = prodRep.GetProductByPizzaID(pizzaId);
            if ((null != prod) || (!string.IsNullOrEmpty(p.CartonSN)))
            {
                var ex = new FisException("CHK1047", new string[] { });
                throw ex;
            }

            pizzaRepository.BackUpPizzaStatusDefered(CurrentSession.UnitOfWork, pizzaId, CurrentSession.Editor);
            pizzaRepository.BackUpPizzaPartDefered(CurrentSession.UnitOfWork, pizzaId, CurrentSession.Editor);

            PizzaPart delpizza = new PizzaPart();            delpizza.PizzaID = pizzaId;

            pizzaRepository.DeletePizzaPartDefered(CurrentSession.UnitOfWork, delpizza);


            p.Status.StationID = this.Station;

            string line = string.IsNullOrEmpty(this.Line) ? p.Status.LineID : this.Line;
            var PizzaLog = new PizzaLog
            {
                Model = p.Model,
                Editor = Editor,
                Line = line,
                Station = Station,
                Descr = "",
                PizzaID = p.PizzaID,
                Cdt = DateTime.Now
            };

            p.AddPizzaLog(PizzaLog);
            pizzaRepository.Update(p, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }

	}
}

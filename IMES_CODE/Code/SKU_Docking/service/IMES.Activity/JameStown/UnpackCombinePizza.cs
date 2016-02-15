// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UC: mantis 2086
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
using IMES.Common;

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
    public partial class UnpackCombinePizza : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public UnpackCombinePizza()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(UnpackCombinePizza));

        /// <summary>
        /// Status
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(UnpackCombinePizza.StatusProperty)));
            }
            set
            {
                base.SetValue(UnpackCombinePizza.StatusProperty, value);
            }
        }

        /// <summary>
        /// StationIdJCProperty
        /// </summary>
        public static DependencyProperty StationIdJCProperty = DependencyProperty.Register("StationIdJC", typeof(string), typeof(UnpackCombinePizza));

        /// <summary>
        /// StationId for model 
        /// </summary>
        [DescriptionAttribute("StationIdJC")]
        [CategoryAttribute("InArguments Of UnpackCombinePizza")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string StationIdJC
        {
            get
            {
                return ((string)(base.GetValue(UnpackCombinePizza.StationIdJCProperty)));
            }
            set
            {
                base.SetValue(UnpackCombinePizza.StationIdJCProperty, value);
            }
        }


        /// <summary>
        /// StationId173Property
        /// </summary>
        public static DependencyProperty StationId173Property = DependencyProperty.Register("StationId173", typeof(string), typeof(UnpackCombinePizza));

        /// <summary>
        /// StationId for model 
        /// </summary>
        [DescriptionAttribute("StationId173")]
        [CategoryAttribute("InArguments Of UnpackCombinePizza")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string StationId173
        {
            get
            {
                return ((string)(base.GetValue(UnpackCombinePizza.StationId173Property)));
            }
            set
            {
                base.SetValue(UnpackCombinePizza.StationId173Property, value);
            }
        }

        private void UpdatePizza(Pizza pizza, string station, string changeCartonSN)
        {
            IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

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

        private void UnpackPizzaPart(Pizza p, string unpackForNotStation)
        {
            IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            IList<PizzaPart> plistUnpack = new List<PizzaPart>();
            IList<PizzaPart> plist = pizzaRepository.GetPizzaPartsByValueLike(14, "", p.PizzaID);
            foreach (PizzaPart pp in plist)
            {
                if (unpackForNotStation.Equals(pp.Station))
                {
                    continue;
                }
                plistUnpack.Add(pp);
            }

            if (plistUnpack.Count > 0)
            {
                pizzaRepository.BackUpPizzaPartDefered(CurrentSession.UnitOfWork, p.PizzaID, CurrentSession.Editor);

                foreach (PizzaPart pp in plistUnpack)
                {
                    PizzaPart delpizza = new PizzaPart();
                    delpizza.PizzaID = p.PizzaID;
                    delpizza.PartNo = pp.PartNo;
                    delpizza.PartSn = pp.PartSn;
                    pizzaRepository.DeletePizzaPartDefered(CurrentSession.UnitOfWork, delpizza);
                }
            }
        }

        /// <summary>
        /// Unpack CombinePizza
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            List<string> errpara = new List<string>();

            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            IList<IProduct> prodList = null;
            string cartonSN = CurrentSession.GetValue("CartonSN") as string;

            if (!string.IsNullOrEmpty(cartonSN))
            {
                prodList = prodRep.GetProductListByCartonNo(cartonSN);
            }
            else
            {
                Product curProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as Product;
                prodList = new List<IProduct>();
                prodList.Add(curProduct);
            }

            IList<Pizza> updatePizzasJC = new List<Pizza>();
            IList<Pizza> updatePizzas173 = new List<Pizza>();
            
            bool modelIsJC = false;
            bool modelIs173 = false;

            foreach (IProduct p in prodList)
            {
                //modelIsJC = (p.Model.IndexOf("JC") == 0);
                modelIsJC = ActivityCommonImpl.Instance.CheckModelByProcReg(p.Model, "ThinClient");
                modelIs173 = (p.Model.IndexOf("173") == 0);
                if (!(modelIsJC || modelIs173))
                    continue;

                prodRep.BackUpProductDefered(CurrentSession.UnitOfWork, p.ProId, CurrentSession.Editor);
                prodRep.BackUpProductStatusDefered(CurrentSession.UnitOfWork, p.ProId, CurrentSession.Editor);
                prodRep.BackUpProductPartDefered(CurrentSession.UnitOfWork, p.ProId, CurrentSession.Editor);

                if (null != p.Status)
                    p.Status.StationId = this.Station;

                if (modelIsJC)
                {
                    Pizza pizza = pizzaRepository.Find(p.PizzaID);
                    if (pizza == null)
                    {
                        throw new NullReferenceException("Pizza in session is null");
                        //continue;
                    }

                    //if (!updatePizzasJC.Contains(p.PizzaID))
                    updatePizzasJC.Add(pizza);

                    p.PizzaID = "";
                }
                //else if (modelIs173)
                //{
                //    if (!updatePizzas173.Contains(p.PizzaID))
                //        updatePizzas173.Add(p.PizzaID);
                //}

                string line = string.IsNullOrEmpty(this.Line) ? p.Status.Line : this.Line;
                var productLog = new ProductLog
                {
                    Model = p.Model,
                    Status = Status,
                    Editor = Editor,
                    Line = line,
                    Station = Station,
                    Cdt = DateTime.Now
                };

                p.AddLog(productLog);
                prodRep.Update(p, CurrentSession.UnitOfWork);

            }

            if (modelIs173 && !string.IsNullOrEmpty(cartonSN))
            {
                IList<Pizza> pizzas = pizzaRepository.GetCombinePizzaByCartonSN(cartonSN);
                foreach (Pizza p in pizzas)
                {
                    //if (!updatePizzas173.Contains(p.PizzaID))
                    updatePizzas173.Add(p);
                }
            }

            foreach (Pizza p in updatePizzasJC)
            {
                UnpackPizzaPart(p, "PKJOK");
                UpdatePizza(p, this.StationIdJC, null);
            }

            foreach (Pizza p in updatePizzas173)
            {
                UnpackPizzaPart(p, "PKJROK");
                UpdatePizza(p, this.StationId173, "");
            }

            return base.DoExecute(executionContext);
        }

	}
}

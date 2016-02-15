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

using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.FisObject.PAK.Pizza;

namespace IMES.Activity
{
    /// <summary>
    /// UpdatePizzaStatus  and Write PizzaLog
    /// </summary>
    public partial class UpdateMultiPizzaStatusAndLog : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public UpdateMultiPizzaStatusAndLog()
		{
			InitializeComponent();
		}
         
        /// <summary>
        /// CartonSNName
        /// </summary>
        public static DependencyProperty CartonSNNameProperty = DependencyProperty.Register("CartonSNName", typeof(string), typeof(UpdateMultiPizzaStatusAndLog), new PropertyMetadata(""));

        /// <summary>
        ///  Session Name Of CartonSN
        /// </summary>
        [DescriptionAttribute("CartonSNName")]
        [CategoryAttribute("Session Name Of CartonSN")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string CartonSNName
        {
            get
            {
                return ((string)(base.GetValue(UpdateMultiPizzaStatusAndLog.CartonSNNameProperty)));
            }
            set
            {
                base.SetValue(UpdateMultiPizzaStatusAndLog.CartonSNNameProperty, value);
            }
        }



        /// <summary>
        /// Update Pizza Status and Log
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var PizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            var pizzaNoList = CurrentSession.GetValue(Session.SessionKeys.PizzaNoList) as List<string>;
            if (pizzaNoList == null)
            {
                throw new NullReferenceException("PizzaNoList in session is null");
            }
            foreach (var pizzaNo in pizzaNoList)
            {
                Pizza pizzaObj = PizzaRepository.Find(pizzaNo);

                if (!string.IsNullOrEmpty(CartonSNName))
                {
                    string cartonSN = CurrentSession.GetValue(CartonSNName) as string;
                    if (!string.IsNullOrEmpty(cartonSN))
                    {
                        pizzaObj.CartonSN = cartonSN;
                    }
                }
                
                pizzaObj.Status.StationID = Station;
                pizzaObj.Status.Editor = Editor;
                pizzaObj.Status.Udt = DateTime.Now;

                var PizzaLog = new PizzaLog
                {
                    Model = pizzaObj.Model,
                    Editor = Editor,
                    Line = Line,
                    Station = Station,
                    Descr = "",
                    PizzaID = pizzaObj.PizzaID,
                    Cdt = DateTime.Now
                };

                pizzaObj.AddPizzaLog(PizzaLog);
                PizzaRepository.Update(pizzaObj, CurrentSession.UnitOfWork);
            }

            return base.DoExecute(executionContext);
        }
	}
}

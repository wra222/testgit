using System;
using System.Collections.Generic;
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
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 保存ExplicitCheckItem
    /// </summary>
	public partial class SaveExplictCheckItem: BaseActivity
	{
		public SaveExplictCheckItem()
		{
			InitializeComponent();
		}

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var objectToCheck = GetObjectToCheck();
            var items =
                (IList<ICheckItem>) CurrentSession.GetValue(Session.SessionKeys.ExplicityCheckItemList);
            foreach (ICheckItem item in items)
            {
                if (item.Matched)
                {
                    //item.Save(objectToCheck);
                    if (item.ItemType.Equals(CheckItemType.PizzaProperty))
                    {
                        Pizza pizza = (Pizza)CurrentSession.GetValue(Session.SessionKeys.Pizza);
                        if (pizza == null)
                        {
                            pizza = ((IProduct)objectToCheck).PizzaObj;
                        }
                        IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                        repPizza.Update(pizza, CurrentSession.UnitOfWork);
                    }
                    else
                    {
                        UpdateMainObject(objectToCheck);
                    }
                }
            }
            return base.DoExecute(executionContext);
        }

        private ICheckObject GetObjectToCheck()
        {
            var objectToCheck = default(ICheckObject);
            switch (CurrentSession.Type)
            {
                case Session.SessionType.MB:
                    objectToCheck = (ICheckObject)CurrentSession.GetValue(Session.SessionKeys.MB);
                    break;
                case Session.SessionType.Product:
                    objectToCheck = (ICheckObject)CurrentSession.GetValue(Session.SessionKeys.Product);
                    break;
                default:
                    break;
            }
            return objectToCheck;
        }
	}
}

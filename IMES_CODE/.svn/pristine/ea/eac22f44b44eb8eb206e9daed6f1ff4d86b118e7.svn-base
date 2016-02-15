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
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.Common.Misc;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
	public partial class GetDefectComponent: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public GetDefectComponent()
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
            IMiscRepository miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            Session session = CurrentSession;
            string partSerialNo = (string)session.GetValue("PartSerialNo");
            string customer = this.Customer;
            IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            IList<string> customerList = iPartRepository.GetValueFromSysSettingByName("Customer");
            if (customerList != null && customerList.Count > 0)
            {
                customer = customerList[0];
            }

            IList<DefectComponentInfo> defectComponentInfoList = new List<DefectComponentInfo>();
            switch (TypeFrom)
            {
                case TypeFromEnum.Vendor:
                    defectComponentInfoList = miscRep.GetData<DefectComponent, DefectComponentInfo>(new DefectComponentInfo { Vendor = partSerialNo, Customer = customer });
                    break;
                case TypeFromEnum.PartSN:
                    defectComponentInfoList = miscRep.GetData<DefectComponent, DefectComponentInfo>(new DefectComponentInfo { PartSn = partSerialNo, Customer = customer });
                    break;
                case TypeFromEnum.ID:
                    string lastsn = partSerialNo.Substring(1, partSerialNo.Length - 1);
                    int ID = 0;
                    bool result = int.TryParse(lastsn, out ID);
                    if (!result)
                    {
                        throw new FisException("CQCHK50109", new string[] { partSerialNo});
                    }
                    defectComponentInfoList = miscRep.GetData<DefectComponent, DefectComponentInfo>(new DefectComponentInfo { ID = ID, Customer = customer });
                    
                    break;
            }
            if (defectComponentInfoList.Count == 0)
            {
                throw new FisException("CQCHK50109", new string[] { partSerialNo });
            }
            session.AddValue("DefectComponentInfo", defectComponentInfoList);
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// Type的来源，共有三种Vendor，PartSN，ID
        /// </summary>
        public static DependencyProperty TypeFromProperty = DependencyProperty.Register("TypeFrom", typeof(TypeFromEnum), typeof(GetDefectComponent));

        /// <summary>
        /// Type的来源，共有三种Vendor，PartSN，ID
        /// </summary>
        [DescriptionAttribute("TypeFromProperty")]
        [CategoryAttribute("InArguments Of GetDefectComponent")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(TypeFromEnum.Vendor)]
        public TypeFromEnum TypeFrom
        {
            get
            {
                return ((TypeFromEnum)(base.GetValue(GetDefectComponent.TypeFromProperty)));
            }
            set
            {
                base.SetValue(GetDefectComponent.TypeFromProperty, value);
            }
        }


        /// <summary>
        /// MO的来源，共有四种MONO，ProdMO，Product,MB
        /// </summary>
        public enum TypeFromEnum
        {
            /// <summary>
            /// 从Session.Vendor中获取
            /// </summary>
            Vendor = 1,

            /// <summary>
            /// 从Session.PartSN中获取
            /// </summary>
            PartSN = 2,

            /// <summary>
            /// 从Session.ID中获取
            /// </summary>
            ID = 3
        }
	}
}

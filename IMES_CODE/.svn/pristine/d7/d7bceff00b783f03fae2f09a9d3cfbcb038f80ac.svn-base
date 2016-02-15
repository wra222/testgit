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
using IMES.Common;

namespace IMES.Activity
{
	public partial class CheckDefectComponentStatus: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckDefectComponentStatus()
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
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            string partSerialNo = (string)session.GetValue("PartSerialNo");
            IList<DefectComponentInfo> defectComponentInfoList = (IList<DefectComponentInfo>)session.GetValue("DefectComponentInfo");
            
            string allowStatus = AllowStatus;
            //if (string.IsNullOrEmpty(allowStatus))
            //{
            //    throw new FisException("AllowStatus is null");
            //}
            var allowStatusList = allowStatus.Split('~');
            

            //string allowList = "";
            //foreach (string s in tempAllowList)
            //{
            //    allowList += "'" + s + "',";
            //}
            //allowList = allowList.Substring(0, allowList.Length - 1);

            //defectComponentInfoList = defectComponentInfoList.Where(x => tempAllowList.Contains(x.Status)).Select(x => x).ToList();

//            string strSQL = @"select a.PartSn, a.PartNo, a.PartType, a.[Status], b.Value as StatusDescr, a.DefectCode, a.DefectDescr
//                                            from DefectComponent a
//                                            inner join ConstValue b on b.[Type]='DCStatus'
//                                             and b.Name = a.[Status] 
//                                            where a.ID = @ID and a.Customer =@Customer and 
//                                            a.[Status]in (@status) ";

//            SqlParameter paraCustomer = new SqlParameter("@Customer", SqlDbType.VarChar, 20);
//            paraCustomer.Direction = ParameterDirection.Input;
//            paraCustomer.Value = this.Customer;
//            SqlParameter paraID = new SqlParameter("@ID", SqlDbType.Int, 20);
//            paraID.Direction = ParameterDirection.Input;
//            paraID.Value = ID;
//            SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.Int, 20);
//            paraStatus.Direction = ParameterDirection.Input;
//            paraStatus.Value = allowList;
//            DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL,
//                                                                                                                        paraCustomer,
//                                                                                                                        paraID,
//                                                                                                                        paraStatus);

            if (!defectComponentInfoList.All(x => allowStatusList.Contains(x.Status)))
            {
                throw new FisException("CQCHK50110", new string[] { partSerialNo });
            }
            IList<ConstValueInfo> constList =null;
            ConstValueInfo constDescr =null;
            string statusDesc = "";

            if (utl.TryConstValue("DCStatus", defectComponentInfoList[0].Status, out constList, out constDescr))
            {
                statusDesc = constDescr.value;
            }

            //IList<ConstValueInfo> constList = miscRep.GetData<ConstValue, ConstValueInfo>(new ConstValueInfo { type = "DCStatus", name = defectComponentInfoList[0].Status });
            //if (constList.Count == 0)
            //{
            //    throw new FisException("CQCHK50110", new string[] { partSerialNo });
            //}
            //string statusDesc = constList[0].value;

            session.AddValue("StatusDesc", statusDesc);
            return base.DoExecute(executionContext);
        }


        /// <summary>
        ///  
        /// </summary>
        public static DependencyProperty AllowStatusProperty = DependencyProperty.Register("AllowStatus", typeof(string), typeof(CheckDefectComponentStatus), new PropertyMetadata("00~20"));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("AllowStatusProperty")]
        [CategoryAttribute("InArguments Of CheckDefectComponentStatus")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string AllowStatus
        {
            get
            {
                return ((string)(base.GetValue(AllowStatusProperty)));
            }
            set
            {
                base.SetValue(AllowStatusProperty, value);
            }
        }
	}
}

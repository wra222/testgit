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
    /// <summary>
    /// 
    /// </summary>
	public partial class CheckDefectComponentInfo: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckDefectComponentInfo()
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
            ArrayList ret = new ArrayList();
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            string partSerialNo = this.Key;
            IList<DefectComponentInfo> defectComponentInfoList = (IList<DefectComponentInfo>)session.GetValue("DefectComponentInfo");
            string custcode = (string)session.GetValue("custcode");
            string comment = (string)session.GetValue("Comment")??"";
            if (defectComponentInfoList.Count == 0)
            {
                throw new FisException("CQCHK50109", new string[] { "" });
            }

            int maxRecycleCount = int.Parse(utl.GetSysSetting("DefectComponentRecycleCnt", "2"));

            IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            IList<DefectComponentInfo> defectComponentInfoSaveList = new List<DefectComponentInfo>();
            List<string> recycleList = new List<string>();
            foreach (DefectComponentInfo item in defectComponentInfoList)
            {
                string partNo = item.PartNo;
                if (string.IsNullOrEmpty(partNo))
                {
                    throw new FisException("CHK027", new string[] { partNo });
                }

                IPart part= iPartRepository.Find(partNo);
                if (part == null)
                {
                    throw new FisException("CHK027", new string[] { partNo });
                }
                #region 取得vendor
                string vendor =item.Vendor;
                if (string.IsNullOrEmpty(vendor))
                {
                    vendor = part.Attributes.Where(x => x.InfoType.ToUpper() == "VENDOR").Select(y => y.InfoValue).FirstOrDefault();
                }
                if (string.IsNullOrEmpty(vendor))
                {
                    throw new FisException("CQCHK50116", new string[] { item.PartNo });
                }
                #endregion
                #region 取得IECPn
                string iecPn = item.IECPn;
                if (string.IsNullOrEmpty(iecPn))
                {
                    iecPn = part.Attributes.Where(x => x.InfoType.ToUpper() == "RDESC").Select(y => y.InfoValue).FirstOrDefault();
                }
                if (string.IsNullOrEmpty(iecPn))
                {
                    throw new FisException("CQCHK50116", new string[] { item.PartNo });
                }
             
                #endregion
             
                #region 取得/寫入recycleCount
                string status =(string)session.GetValue("Status");
                int recycleCount= miscRep.GetDataCount<DefectComponentLog, DefectComponentLogInfo>(
                    new DefectComponentLogInfo {
                        PartSn = item.PartSn,
                        Status="00"
                    }, 
                  "ID");
                //Check Recycle Count
                if (item.Status == "20" && 
                    status == "10" && 
                    item.PartNo!=item.PartSn &&
                    recycleCount>maxRecycleCount)
                {
                    throw new FisException("CQCHK50115", new string[] { recycleCount.ToString(), maxRecycleCount.ToString() });
                }
               
                recycleList.Add(recycleCount.ToString());
                #endregion

                #region 寫入 DefectComponent.Vendor值及DefectComponent.IECPN 值
                item.Vendor = vendor;
                item.IECPn = iecPn;
                item.Comment = comment;
                defectComponentInfoSaveList.Add(item);
                #endregion
            }
            session.AddValue("DefectComponentInfo", defectComponentInfoSaveList);
            session.AddValue("RecycleList", recycleList);
            
            return base.DoExecute(executionContext);
        }
	}
}

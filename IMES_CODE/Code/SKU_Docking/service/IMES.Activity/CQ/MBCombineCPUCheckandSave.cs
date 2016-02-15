using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using IMES.FisObject.Common.Material;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MBCombineCPUCheckandSave : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public MBCombineCPUCheckandSave()
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
            MB currentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
            string cvsn = (string)CurrentSession.GetValue(Session.SessionKeys.CPUVendorSn);
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string valCheckMaterialStatus = "";
            IList<ConstValueInfo> valueList = partRep.GetConstValueListByType("CheckMaterialStatus");
            if (null != valueList)
            {
                var valueListCpu = (from p in valueList
                                    where p.name == "SACPU"
                                    select p).ToList();
                if (valueListCpu != null && valueListCpu.Count > 0)
                    valCheckMaterialStatus = valueListCpu[0].value;
            }
            if ("Y" != valCheckMaterialStatus && "N" != valCheckMaterialStatus)
            {
                // 请联系IE维护是否需要检查CPU状态
                throw new FisException("CQCHK0050", new string[] { });
            }

            bool needCheckMaterialStatus = false;
            if ("Y" == valCheckMaterialStatus)
            {
                needCheckMaterialStatus = true;

                IList<ConstValueTypeInfo> lstConstValueType = partRep.GetConstValueTypeList("NoCheckOnBoardCPUStatus_SA");
                if (null != lstConstValueType)
                {
                    foreach (ConstValueTypeInfo cvt in lstConstValueType)
                    {
                        if (cvt.value == currentMB.Family)
                        {
                            needCheckMaterialStatus = false;
                            break;
                        }
                    }
                }
            }
            if (needCheckMaterialStatus)
            {
                IMaterialRepository MaterialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();

                IList<string> lst = new List<string>();
                lst.Add(cvsn);
                IList<Material> lstMaterials = MaterialRepository.GetMaterialByMultiCT(lst);
                if (null == lstMaterials || lstMaterials.Count == 0)
                {
                    // 此CPU:@CPUCT未收集
                    FisException fex = new FisException("CQCHK0051", new string[] { cvsn });
                    fex.stopWF = false;
                    throw fex;
                }

                Material mat = lstMaterials[0];
                if ("Collect" != mat.Status && "Dismantle" != mat.Status)
                {
                    // 此CPU：@CPUCT为不可结合状态
                    FisException fex = new FisException("CQCHK0052", new string[] { cvsn });
                    fex.stopWF = false;
                    throw fex;
                }


                MaterialLog mlog = new MaterialLog();
                mlog.Action = "SA CombineCPU";
                mlog.Cdt = DateTime.Now;
                mlog.Comment = "";
                mlog.Editor = this.Editor;
                mlog.Line = this.Line;
                mlog.MaterialCT = cvsn;
                mlog.PreStatus = mat.Status;
                mlog.Stage = "SA";
                mlog.Status = "Assembly";

                mat.AddMaterialLog(mlog);

                mat.PreStatus = mat.Status;
                mat.Status = "Assembly";
                mat.Udt = DateTime.Now;
                MaterialRepository.Update(mat, CurrentSession.UnitOfWork);

            }
            

            return base.DoExecute(executionContext);
        }
	}
}

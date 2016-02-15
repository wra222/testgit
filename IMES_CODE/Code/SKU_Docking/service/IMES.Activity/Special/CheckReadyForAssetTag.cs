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

using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.MO;
using IMES.FisObject.FA.Product;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;

namespace IMES.Activity
{
    /// <summary>
    /// Check Asset tag partNo ready for print travel card
    /// </summary>
    public partial class CheckReadyForAssetTag : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
		public CheckReadyForAssetTag()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check Asset tag partNo ready for print travel card
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string> siteList = ipartRepository.GetValueFromSysSettingByName("Site");
            string site = (siteList != null && siteList.Count > 0 && !string.IsNullOrEmpty(siteList[0])) ? siteList[0].Trim() : "IPC";
            if (site != "IPC")
            {
                return base.DoExecute(executionContext);
            }


            string model = "";
            MO mo = (MO)CurrentSession.GetValue(Session.SessionKeys.ProdMO);

            if (mo == null)
            {
                IProduct prod = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                if (prod != null)
                {
                    model = prod.Model;
                }
            }
            else
            {
                model = mo.Model;
            }

            if (!model.StartsWith("PC"))
            {
                return base.DoExecute(executionContext);
            }

            IList<ConstValueTypeInfo>  astTypeInfos= ipartRepository.GetConstValueTypeList("ASTType");

            if (astTypeInfos == null || astTypeInfos.Count == 0)
            {
                throw new FisException("TRC002", new string[] { "ASTType" });
            }

            var astTypes = (from p in astTypeInfos
                            select p.value.Trim()).ToList();
            IList<string> astBomParts=  bomRep.GetAssetPartNo(model, astTypes);
            if (astBomParts.Count > 0)
            {
                IList<ConstValueTypeInfo> astPartInfos = ipartRepository.GetConstValueTypeList("ASTReadyPartNo");
                if (astPartInfos == null || astPartInfos.Count == 0)
                {
                    throw new FisException("TRC002", new string[] { "ASTReadyPartNo" });
                }

                var readyPartNos = (from p in astPartInfos
                                                  select p.value.Trim()).ToList();
                
                foreach (string partNo in astBomParts)
                {
                    if (!readyPartNos.Contains(partNo))
                    {
                        throw new FisException("TRC003", new string[] { partNo });
                    }
                }
            }
            return base.DoExecute(executionContext);
        }
	}
}

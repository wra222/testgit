using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.ComponentModel;

namespace IMES.Activity.Special
{
	/// <summary>
	/// 
	/// </summary>
    public partial class BindDummyCPU: BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public BindDummyCPU()
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
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

            IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();

            IFlatBOM cpuBom = null;
            IFlatBOM bom = null;
            IProduct defaultProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (defaultProduct.CVSN == "")
            {
                //当KP3 站号39 时,如果没有结合CPU,且BOM中有CPU，则插入CPU信息
                bom = (IFlatBOM)CurrentSession.GetValue(Session.SessionKeys.SessionBom);
                foreach (FlatBOMItem item in bom.BomItems){
                    if (item.CheckItemType != "CPU") { 
                        //bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                        cpuBom = ibomRepository.GetFlatBOMByPartTypeModel("CPU", defaultProduct.Model, Station, defaultProduct);
                    }
                }

            }
            string hCode = "";
            if (cpuBom != null) 
            {
                bom.Merge(cpuBom);

                //get hcode
                var hBom = ibomRepository.GetHierarchicalBOMByModel(defaultProduct.Model);
                foreach (BOMNode node in hBom.FirstLevelNodes) {
                    if (node.Part.BOMNodeType == "BM" && node.Part.Descr == "CPU") {
                        hCode = node.Part.GetProperty("HCode");
                    }
                }

                //	CPU的VENDOR CT@cpu通过下面方法获得
                //@cpu=@code+substring(@PodcutID,3,1)+substring(@PodcutID,1,6)+'A'+substring(@PodcutID,7,10)
                //是@code +ProductID第三码  +ProductID 第1至6码 +‘A’+ 第七码至第十码
                //其中@code为 此Model下阶的BomNodeType为’BM’,Descr为‘CPU’的Part的PartInfo里的InfoType=‘HCode’的InfoValue
                string CpuVendorCT = "";
                CpuVendorCT = hCode + currentProduct.ProId.Substring(2, 1) + currentProduct.ProId.Substring(0, 6) + "A" + currentProduct.ProId.Substring(6, 3);

                PartUnit matchedPart = bom.Match(CpuVendorCT, Station);

                if (matchedPart != null)
                {
                    try
                    {
                        matchedPart.ProductId = defaultProduct.ProId;
                        bom.Check(matchedPart, defaultProduct, Station);
                        bom.AddCheckedPart(matchedPart);
                    }
                    catch (FisException ex)
                    {
                        ex.stopWF = false;
                        throw;
                    }
                }


           }

            return base.DoExecute(executionContext);
        }
	}
}

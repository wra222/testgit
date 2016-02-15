
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Xml;
using IMES.FisObject.Common.FisBOM;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GetAstPart : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public GetAstPart()
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
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            var currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            if (currentProduct == null)
            {
                var ex1 = new FisException("SFC002", new string[] { "" });
                throw ex1;
            }
              CurrentSession.AddValue("Image1Src","");
              CurrentSession.AddValue("Image2Src", "");
              string iamge1BomDescr= (string)CurrentSession.GetValue("Image1ASTType");
              string iamge2BomDescr = (string)CurrentSession.GetValue("Image2ASTType");         
             IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
             IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currentProduct.Model);
             string strMN2 = currentProduct.GetModelProperty("MN2") as string;
             string iamge1 = strMN2.Trim()+GetImageSrc(bom, iamge1BomDescr.Split(new char[] { ',', '~' }));
             string iamge2 = strMN2.Trim()+GetImageSrc(bom, iamge2BomDescr.Split(new char[] { ',', '~' }));
             CurrentSession.AddValue("Image1Src", iamge1);
             CurrentSession.AddValue("Image2Src", iamge2);
             return base.DoExecute(executionContext);
        }


        private string GetImageSrc(IHierarchicalBOM bom,string[] ASTDescr)
        {
            string imageUrl = "";
            IList<string> PNList = new List<string>();
            for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
            {
                IPart part6 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;

                if ((part6.BOMNodeType == "PP") || ((part6.BOMNodeType == "AT") && (ASTDescr.Contains(part6.Descr))))
                {
                    PNList.Add(part6.PN);
                }
            }

            if ((PNList != null) || (PNList.Count == 0))
            {
                for (int i = 1; i < PNList.Count; i++)
                {
                    string t = PNList[i];
                    int j = i;
                    while ((j > 0) && string.Compare(PNList[j - 1], t, false) > 0)
                    {
                        PNList[j] = PNList[j - 1];
                        --j;
                    }

                    PNList[j] = t;
                }
            }
            foreach (string pn2 in PNList)
            {
                imageUrl += pn2.Trim();
            }

            return imageUrl;

        }
	}
}

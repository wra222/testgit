// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-05-21   200038                       Create
// Known issues:
//

using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.MAC.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.MAC.Filter.dll")]
    public class Filter: IFilterModule
    {
        private const string PartCheckType = "MAC";

        /// <summary>
        ///若Model符合以下条件中的任何一个，增加MAC的收集
        ///Model的前2码为’TC’
        ///Model的前3码为’156’，且Model的PN属性值(ModelInfo.Name=’PN’)的前2码为’PT’        /// </summary>
        /// <param name="hierarchicalBom"></param>
        /// <param name="station"></param>
        /// <param name="mainObject"></param>
        /// <returns></returns>
        public object FilterBOM(object hierarchicalBom, string station, object mainObject)
        {
            return CreateMACBom();
            #region disable code for mantis 0002706: Docking Board Input 收集MAC 邏輯修改
            //var repository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, IMES.FisObject.Common.Model.Model>();
            //string model = string.Empty;
            //Model modelObj = null;
            //if (mainObject == null)
            //{
            //    model = ((IHierarchicalBOM) hierarchicalBom).Model;
            //    modelObj = repository.Find(model);
            //}
            //else
            //{
            //    model = ((Product)mainObject).Model;
            //    modelObj = ((Product)mainObject).ModelObj;
            //}
            ////Model的前2码为’TC’
            //if (!string.IsNullOrEmpty(model) 
            //    && model.Length >= 2 
            //    && string.Compare(model.Substring(0,2), "TC") == 0)
            //{
            //    return CreateMACBom();
            //}

            ////Model的前3码为’156’，且Model的PN属性值(ModelInfo.Name=’PN’)的前2码为’PT’        /// </summary>
            //if (!string.IsNullOrEmpty(model)
            //    && model.Length >= 3
            //    && string.Compare(model.Substring(0, 3), "156") == 0)
            //{
            //    string modelPn = modelObj.GetAttribute("PN");
            //    if (!string.IsNullOrEmpty(modelPn)
            //        && modelPn.Length >= 2
            //        && string.Compare(modelPn.Substring(0, 2), "PT") == 0)
            //    {
            //        return CreateMACBom();
            //    }
            //}

            //return null;
            #endregion
        }

        /// <summary>
        ///PartType：’MAC’
        ///PartDescription：’MAC’
        ///PartNo/ItemName=’NIC Address’
        ///Qty：1
        /// </summary>
        /// <returns></returns>
        private IFlatBOM CreateMACBom()
        {
            var flatBOMItem = new FlatBOMItem(1, PartCheckType, null);
            flatBOMItem.Descr = "MAC";
            flatBOMItem.PartNoItem = "NIC Address";
            IList<IFlatBOMItem> flatBOMItems = new List<IFlatBOMItem>();
            flatBOMItems.Add(flatBOMItem);
            return new FlatBOM(flatBOMItems);
        }
    }
}

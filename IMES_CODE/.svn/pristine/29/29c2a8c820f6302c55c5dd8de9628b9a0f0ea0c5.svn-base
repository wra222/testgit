// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-05-21   200038                       Create
// Known issues:
//
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.CheckItemModule.Utility;

namespace IMES.CheckItemModule.DockingMB.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.DockingMB.Filter.dll")]
    public class Filter: IFilterModule
    {
        private const string PartCheckType = "DockingMB";
        private const string MBCodeAttributeName = "MB";
        private const string MBBomNodeType = "MB";

        /// <summary>
        /// Model：Product.Model
        ///Model直接下阶MB阶（ModelBOM.Material=[Model] and Component=Part.PartNo and Part.BomNodeType=’MB’），取得所有MB为共用料；获取MB阶MB属性@MBCode (PartInfo.InfoType=’MB’ and PartInfo.PartNo=[PartNo])
        ///若上述信息不存在，则取Model的MB属性@MBCode（ModelInfo.Model=[Model] and Name=’MB’）
        ///若以上信息都不存在，则报错：“Model：XXX组建错误，没有MBCode”
        /// </summary>
        /// <param name="hierarchicalBom"></param>
        /// <param name="station"></param>
        /// <param name="mainObject"></param>
        /// <returns></returns>
        public object FilterBOM(object hierarchicalBom, string station, object mainObject)
        {
            //根据Model展1阶，得到第一阶是MB的part [BomNodeType=MB]的MBCode[PartInfo.InfoValue(InfoType='MB')]，
            IFlatBOM ret = null;
            if (hierarchicalBom == null)
            {
                throw new ArgumentNullException();
            }
            Model modelObj = null;
            if (mainObject == null)
            {
                var repository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, IMES.FisObject.Common.Model.Model>();
                string model = ((IHierarchicalBOM)hierarchicalBom).Model;
                modelObj = repository.Find(model);
            }
            else
            {
                modelObj = ((Product) mainObject).ModelObj;
            }
            var parts = new List<IPart>();
            IList<KPVendorCode> kpVCList = new List<KPVendorCode>();

            String mbCodeString = string.Empty;
            var bom = (HierarchicalBOM)hierarchicalBom;
            IList<IBOMNode> mbNodes = bom.GetFirstLevelNodesByNodeType(MBBomNodeType);
            if (mbNodes != null && mbNodes.Count > 0)
            {
                foreach (IBOMNode bomNode in mbNodes)
                {
                    IList<string> mbCodeList = new List<string>();
                    string mbCode = this.GetMBCode(bomNode);
                    if (string.IsNullOrEmpty(mbCode))
                    {
                        string modelMbCodeString = string.Empty;
                        mbCodeList = this.GetModelMBCode(modelObj);
                        foreach (string code in mbCodeList)
                        {
                            if (string.IsNullOrEmpty(modelMbCodeString))
                            {
                                modelMbCodeString = code;
                            }
                            else
                            {
                                modelMbCodeString += "," + code;
                            }
                        }
                        if (!string.IsNullOrEmpty(modelMbCodeString))
                        {
                            //PartInfo mbPartInfo = new PartInfo(0, bomNode.Part.PN, MBCodeAttributeName, modelMbCodeString, "", DateTime.Now, DateTime.Now);
                            //bomNode.Part.AddAttribute(mbPartInfo);
                            kpVCList.Add(new KPVendorCode
                            {
                                PartNo = bomNode.Part.PN,
                                VendorCode = modelMbCodeString
                            });
                        }
                    }
                    else
                    {
                        mbCodeList.Add(mbCode);
                        kpVCList.Add(new KPVendorCode
                        {
                            PartNo = bomNode.Part.PN,
                            VendorCode = mbCode
                        });
                    }
                    
                    if (mbCodeList.Count == 0)
                    {
                        //若以上信息都不存在，则报错：“Model：XXX组建错误，没有MBCode”
                        throw new FisException("PAK081", new string[0]);
                    }

                    foreach (string code in mbCodeList)
                    {
                        if (string.IsNullOrEmpty(mbCodeString))
                        {
                            mbCodeString = code;
                        }
                        else
                        {
                            if (!mbCodeString.Contains(code))
                            {
                                mbCodeString += "," + code;
                            }
                        }
                    }
                    parts.Add(bomNode.Part);
                }
            }
            else //BOM中不存在MB类型节点
            {
                Model model = modelObj;
                string modelMbCodeString = string.Empty;
                IList<string> mbCodeList = this.GetModelMBCode(model);
                if (mbCodeList.Count == 0)
                {
                    return null;
                }
                foreach (string code in mbCodeList)
                {
                    if (string.IsNullOrEmpty(modelMbCodeString))
                    {
                        modelMbCodeString = code;
                    }
                    else
                    {
                        modelMbCodeString += "," + code;
                    }
                }
                IPart modelPart = new Part(model.ModelName, "MB", "MB", model.CustPN, model.ModelName, string.Empty, string.Empty, string.Empty, DateTime.Now, DateTime.Now, string.Empty);
                if (!string.IsNullOrEmpty(modelMbCodeString))
                {
                    //var mbPartInfo = new PartInfo(0, modelPart.PN, MBCodeAttributeName, modelMbCodeString, "", DateTime.Now, DateTime.Now);
                   // modelPart.AddAttribute(mbPartInfo);
                    kpVCList.Add(new KPVendorCode
                    {
                        PartNo = modelPart.PN,
                        VendorCode = modelMbCodeString
                    });
                }
                parts.Add(modelPart);
                mbCodeString = modelMbCodeString;
            }

            if (parts.Count > 0)
            {
                var flatBOMItem = new FlatBOMItem(1, PartCheckType, parts);
                flatBOMItem.Tag = kpVCList;
                flatBOMItem.Descr = parts.ElementAt(0).Descr;
                flatBOMItem.PartNoItem = mbCodeString;
                IList<IFlatBOMItem> flatBOMItems = new List<IFlatBOMItem>();
                flatBOMItems.Add(flatBOMItem);
                ret = new FlatBOM(flatBOMItems);
            }

            return ret;
        }

        //MBCode[PartInfo.InfoValue(InfoType='MB')] || MBCode[.InfoValue(InfoType='MB')]
        public string GetMBCode(object node)
        {
            string mbCode = null;
            if (((BOMNode)node).Part != null)
            {
                mbCode = ((BOMNode)node).Part.GetAttribute(MBCodeAttributeName);
            }
            return mbCode;
        }

        public IList<string> GetModelMBCode(Model model)
        {
            string mbCode = null;
            IList<string> ret = new List<string>();
            if (model != null)
            {
                mbCode = model.GetAttribute(MBCodeAttributeName);
            }
            if (!string.IsNullOrEmpty(mbCode))
            {
                ret.Add(mbCode);
            }
            for (int i = 1; i < 4; i++)
            {
                mbCode = model.GetAttribute(string.Format("{0}{1}", MBCodeAttributeName, i));
                if (!string.IsNullOrEmpty(mbCode))
                {
                    ret.Add(mbCode);
                }
            }
            return ret;
        }
    }
}

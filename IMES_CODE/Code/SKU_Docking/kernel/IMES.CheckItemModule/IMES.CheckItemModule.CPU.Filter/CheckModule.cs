// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 
// Known issues:
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.CheckItemModule.Utility;
using IMES.DataModel;
//using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Material;

namespace IMES.CheckItemModule.CPU.Filter
{
    [Export(typeof (ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CPU.Filter.dll")]
    internal class CheckModule : ICheckModule
    {
        /// <summary>
        /// </summary>
        /// <param name=""></param>
        public void Check(object partUnit, object bomItem, string station)
        {
            if (partUnit == null)
            {
                throw new FisException("CHK174", new[] { "IMES.CheckItemModule.CPU.Filter.CheckModule.Check" });
            }

            string partSn = ((PartUnit)partUnit).Sn.Trim();
            if (partSn.Length < 5)
                return;
            
			Session session = SessionManager.GetInstance.GetSession(((PartUnit)partUnit).ProductId, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            IProduct product = (IProduct)session.GetValue(Session.SessionKeys.Product);
            if (product == null)
            {
                throw new FisException("No product object in session");
            }
            
            String currentVendorCode = partSn.Substring(0, 5);

            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string valCheckMaterialStatus = "";
            IList<ConstValueInfo> valueList = partRep.GetConstValueListByType("CheckMaterialStatus");
            if (null != valueList)
            {
                var valueListCpu = (from p in valueList
                                    where p.name == "CPU"
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

                IList<ConstValueTypeInfo> lstConstValueType = partRep.GetConstValueTypeList("NoCheckCPUStatus");
                if (null != lstConstValueType)
                {
                    foreach (ConstValueTypeInfo cvt in lstConstValueType)
                    {
                        if (cvt.value == product.Family)
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
                lst.Add(partSn);
                IList<Material> lstMaterials = MaterialRepository.GetMaterialByMultiCT(lst);
                if (null == lstMaterials || lstMaterials.Count == 0)
                {
                    // 此CPU:@CPUCT未收集
                    FisException fex = new FisException("CQCHK0051", new string[] { partSn });
                    fex.stopWF = false;
                    throw fex;
                }
                
                Material mat = lstMaterials[0];
                if ("Collect" != mat.Status && "Dismantle" != mat.Status)
                {
                    // 此CPU：@CPUCT为不可结合状态
                    FisException fex = new FisException("CQCHK0052", new string[] { partSn });
                    fex.stopWF = false;
                    throw fex;
                }

                string spec = "";
                IMaterialBoxRepository materialBoxRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository, MaterialBox>();
                IList<MaterialBox> lstBox = materialBoxRepository.GetMaterialBoxByLot("CPU", mat.LotNo);
                if (null != lstBox && lstBox.Count > 0)
                    spec = lstBox[0].SpecNo;

                string infor = "";
                IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IHierarchicalBOM curBOM = bomRepository.GetHierarchicalBOMByModel(product.Model);
                if (curBOM == null)
                {
                    throw new FisException("CHK986", new string[] { "Model ", "BOM", product.Model });
                }
                IList<IBOMNode> bomNodeLst = curBOM.FirstLevelNodes;
                if (null != bomNodeLst)
                {
                    foreach (IBOMNode bn in bomNodeLst)
                    {
                        IPart p = bn.Part;
                        if (p.Descr != null && p.Descr.IndexOf("CPU") >= 0 && p.Attributes != null)
                        {
                            foreach (PartInfo pi in p.Attributes)
                            {
                                if ("Infor".Equals(pi.InfoType))
                                {
                                    infor = pi.InfoValue;
                                    break;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(infor))
                            break;
                    }
                }
                if (spec == "" || spec != infor)
                {
                    // 请核对CPU料号
                    FisException fex = new FisException("CQCHK0053", new string[] {  });
                    fex.stopWF = false;
                    throw fex;
                }

                session.AddValue("MaterialCpu", mat);

            } // needCheckMaterialStatus

            if (needCheckMaterialStatus)
                session.AddValue("CheckMaterialCpuStatus", "Y");
            else
                session.AddValue("CheckMaterialCpuStatus", "N");

        }

    }
}

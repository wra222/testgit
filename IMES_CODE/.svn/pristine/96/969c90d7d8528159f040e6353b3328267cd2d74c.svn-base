using System;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.Linq;
using IMES.FisObject.Common.Process;

namespace IMES.CheckItemModule.CQ.LCM.Filter
{using IMES.FisObject.Common.Model;
    using IMES.FisObject.Common.Line;

    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.LCM.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object part_unit, object bom_item, string station)
        {
            FisException fex = new FisException();
            fex.stopWF = false;

            IProcessRepository CurrentProcessRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
            if (part_unit != null)
            {
                PartUnit pu = (PartUnit)part_unit;
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                Session session = (Session)pu.CurrentSession;
                if (session == null)
                {
                    fex.mErrmsg = "Can not get Session instance from SessionManager!";
                    throw fex;
                    //throw new FisException("Can not get Session instance from SessionManager!");
                }

                Product ownerProduct = (Product)session.GetValue(Session.SessionKeys.Product);
                bool isCleanRoomModel = false;

                //IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                //IList<IMES.FisObject.Common.Model.ModelInfo> infos = modelRep.GetModelInfoByModelAndName(ownerProduct.Model, "MN1");
                //foreach (IMES.FisObject.Common.Model.ModelInfo info in infos)
                //{
                //    if (info.Value.ToUpper().IndexOf("TOUCHSMART") >= 0)
                //    {
                //        isCleanRoomModel = true;
                //        break;
                //    }
                //}
                //Mantis 0000449: 无尘室touchsmart机型判定逻辑变更。
                //                select *
                //from ModelBOM a,Part c, PartInfo b 
                //where a.Material='PCBP48AANACY' and
                //      a.Component = c.PartNo and 
                //      c.BomNodeType ='PL' and
                //     a.Component=b.PartNo and 
                //     b.InfoType='TYPE' and
                //     left(UPPER(InfoValue),5)='TOUCH' 

                isCleanRoomModel = checkCleanRoomModel(ownerProduct.Model);
                session.AddValue("IsCleanRoomModel", isCleanRoomModel);

                if (isCleanRoomModel)
                {
                    IProduct partProduct = productRepository.GetProductByIdOrSn(pu.Sn);
                    if (partProduct != null)
                    {

                        string firstLine = "";
                        if (!string.IsNullOrEmpty(partProduct.Status.Line))
                        {
                            firstLine = partProduct.Status.Line.Substring(0, 1);
                        }
                       
                        IList<ModelProcess> currentModelProcess = CurrentProcessRepository.GetModelProcessByModelLine(partProduct.Model, firstLine);
                        if (currentModelProcess == null || currentModelProcess.Count == 0)
                        {
                            CurrentProcessRepository.CreateModelProcess(partProduct.Model, session.Editor, firstLine);
                        }

                        CurrentProcessRepository.SFC(partProduct.Status.Line, session.Customer, "CR32", partProduct.ProId, "Product");

                    }
                    else
                    {
                        FisException ex = new FisException("CQCHK0042", new string[] { pu.Sn });
                        ex.stopWF = false;
                        throw ex;
                        //fex.mErrmsg = "IsCleanRoomModel, Can not get Product instance by Part.Sn when Check !";
                        //throw fex;
                        //throw new FisException(("IsCleanRoomModel, Can not get Product instance by Part.Sn when Check !");
                    }
                }
   
            }
        }


        private bool checkCleanRoomModel(string model)
        {
            bool ret = false;
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM curBOM = bomRep.GetHierarchicalBOMByModel(model);
            IList<IBOMNode> bomNodeLst = curBOM.FirstLevelNodes;

            if (bomNodeLst != null && bomNodeLst.Count > 0)
            {
                var PartPLList = (from p in bomNodeLst
                                  where p.Part.BOMNodeType.ToUpper() == "PL" &&
                                            p.Part.Attributes != null &&
                                            p.Part.Attributes.Count > 0
                                  select p.Part).ToList();
                if (PartPLList != null && PartPLList.Count > 0)
                {                  
                    foreach (IPart part in PartPLList)
                    {
                        var attrList = (from p in part.Attributes
                                        where !string.IsNullOrEmpty(p.InfoType) &&
                                                  p.InfoType == "TYPE" &&
                                                  !string.IsNullOrEmpty(p.InfoValue) &&
                                                  p.InfoValue.ToUpper().StartsWith("TOUCH")
                                        select p).ToList();
                        if (attrList != null && attrList.Count > 0)
                        {
                            ret = true;
                            break;
                        }
                    }                   
                }               
            }

            return ret;
        }
   }
}

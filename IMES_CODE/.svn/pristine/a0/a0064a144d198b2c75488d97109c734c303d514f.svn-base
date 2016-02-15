// INVENTEC corporation (c)2012 all rights reserved. 
// Description: PAK UnitWeight Interface
// UI:CI-MES12-SPEC-PAK-DATA MAINTAIN2.docx
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-04-18   Chen Xu itc208014            create
// Known issues:

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using System.Data;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure.UnitOfWork;
using System.Linq;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.TestLog;
using IMES.FisObject.Common.Station;
namespace IMES.Station.Implementation
{
    /// <summary>
    ///  UC 具体业务：   UC 具体业务： 根据刷入的Model号或者CustSN号，获得当前标准重量，并支持修改该重量
    /// </summary>
    public class ModelWeight : MarshalByRefObject, IModelWeight
    {
        /// <summary>
        /// 获取ModelWeight
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns></returns>
        public ArrayList GetModelWeightByModelorCustSN(string inputData)
        {
            //检查合法model
            //看取得的数据是否有数据
            String result = "";
            try
            {
                string model = string.Empty;
                IModelRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();


                //3)	如果刷入为10位，如果是CN打头的初步认定为Customer SN，如在Product．CustSN中不存在，则提示“非法的Customer SN”
                if (inputData.Length == 10 && (inputData.Substring(0, 2) == "CN"||inputData.Substring(0, 2) == "5C"))
                {
                    var currentProduct = CommonImpl.GetProductByInput(inputData, CommonImpl.InputTypeEnum.CustSN);
                    if (currentProduct == null)
                    {
                        FisException fe = new FisException("PAK042", new string[] { inputData });  //此Customer S/N %1 不存在！
                        throw fe;
                    }
                    else if (string.IsNullOrEmpty(currentProduct.Model))
                    {
                        FisException fe = new FisException("PAK028", new string[] { inputData });  //该Customer SN %1 还未与Model绑定！
                        throw fe;
                    }
                    //else if (!currentProduct.ProductLogs.Where(x => x.Station == "PKCK" && x.Status == StationStatus.Pass).Any())
                    //{
                    //    FisException fe = new FisException("PAK185");  //该Customer SN %1 还未与Model绑定！
                    //    throw fe;
                    //}
                    else model = currentProduct.Model;
                    IList<ProductLog> productLogList = currentProduct.ProductLogs.OrderBy(p => p.Cdt).ToList();
                    int count = productLogList.Count;
                    if (count > 0)
                    {
                        ProductLog lastProductLog = productLogList[count - 1];
                        if (!(lastProductLog.Station == "PKCK" && lastProductLog.Status == StationStatus.Pass))
                        {
                            FisException fe = new FisException("PAK185", new string[] { inputData });  //该Customer SN 为sorting！
                            throw fe;
                        }
                    }
                    
                }
                else
                {
                    Model modelItem = itemRepository.Find(inputData);
                    if (modelItem == null)
                    {
                        var currentProduct = CommonImpl.GetProductByInput(inputData, CommonImpl.InputTypeEnum.CustSN);
                        if (currentProduct == null || string.IsNullOrEmpty(currentProduct.Model))
                        {
                            FisException fe = new FisException("CHK079", new string[] { inputData });   //找不到与此序号 %1 匹配的Product! 
                            throw fe;
                        }
                        else model = currentProduct.Model;

                    }
                    else model = inputData;

                }

                IModelWeightRepository itemRepositoryModelWeight = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();

                DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(model);
                //if (modelWeight == null || modelWeight.Rows.Count == 0)
                //{
                //    //This Model, there is no standard weight, please go to the weighing.
                //    //该Model尚无标准重量，请先去称重。
                //    List<string> erpara = new List<string>();
                //    erpara.Add(model);
                //    FisException ex;
                //    ex = new FisException("PAK123", erpara);
                //    throw ex;
                //}

                if (modelWeight == null || modelWeight.Rows.Count == 0)
                {
                    result = "0.00";
                }
                else
                {
                    result = modelWeight.Rows[0][1].ToString();
                }
                //if (result == "")
                //{
                //    //该Model尚无标准重量，请先去称重。
                //    List<string> erpara = new List<string>();
                //    erpara.Add(model);
                //    FisException ex;
                //    ex = new FisException("PAK123", erpara);
                //    throw ex;
                //}

                ModelWeightDef item = new ModelWeightDef();
                item.Model = model;
                item.UnitWeight = result;

                //IList<string> ls2= GetPrIDListInHoldByModel(model);
                ArrayList arr = new ArrayList();
                arr.Add(item);
                //arr.Add(ls2);
                return arr;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }


        }

        /// <summary>
        /// 保存修改的ModelWeight
        /// </summary>
        /// <param name="item">item</param>
        /// <returns></returns>
        public void SaveModelWeightItem(ModelWeightDef item)
        {
            
            try
            {

                IModelWeightRepository itemRepositoryModelWeight = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();
                ////看取得的数据是否有, 防止update的是空记录，但[PAK_SkuMasterWeight_FIS]中加入了记录
                //DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(item.Model);
                ////if (modelWeight == null || modelWeight.Rows.Count == 0)
                ////{
                ////    //该Model尚无标准重量，请先去称重。
                ////    List<string> erpara = new List<string>();
                ////    erpara.Add(item.Model);
                ////    FisException ex;
                ////    ex = new FisException("PAK123", erpara);
                ////    throw ex;
                ////}
                //if (modelWeight == null || modelWeight.Rows.Count == 0 || item.UnitWeight == "0.00")
                //{
                //    item.UnitWeight = "0.00";
                //}
                //else if (item.UnitWeight != "0.00")
                //{ 
                    
                //}
                //else
                //{
                //    item.UnitWeight = modelWeight.Rows[0][1].ToString();
                //}
                if (string.IsNullOrEmpty(item.Model))
                {
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add("Model");
                    FisException ex;
                    ex = new FisException("CQCHK0006", erpara);
                    throw ex;
                }

                decimal  inputUnitweight =decimal.Parse(item.UnitWeight);
                if (inputUnitweight == 0)
                {
                    //该Model尚无标准重量，请先去称重。
                        List<string> erpara = new List<string>();
                        erpara.Add(item.Model);
                        FisException ex;
                        ex = new FisException("CQCHK0048", erpara);
                        throw ex;
                }

                IModelWeightRepository currentModelWeightRepository = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();
                IMES.FisObject.PAK.StandardWeight.ModelWeight currentModelWeight = currentModelWeightRepository.Find(item.Model);
                bool isAdd = false;
                if (currentModelWeight == null)
                {
                    currentModelWeight = new IMES.FisObject.PAK.StandardWeight.ModelWeight();
                    //每次手動修改標準重需將ModelWeight.Remark='New',在10分鐘後送給SAP Unitweight
                    currentModelWeight.Remark = "New";
                    isAdd = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(currentModelWeight.SendStatus))
                    {
                        currentModelWeight.Remark = currentModelWeight.UnitWeight.ToString();
                    }
                }

                

                currentModelWeight.Model = item.Model;
                //currentModelWeight.UnitWeight = Decimal.Parse(item.UnitWeight);
                currentModelWeight.UnitWeight = inputUnitweight;
                currentModelWeight.SendStatus = "";
                currentModelWeight.Editor = item.Editor;
                currentModelWeight.Udt = DateTime.Now;

                IPizzaRepository itemRepositoryPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository>();

                //ModelWeightInfo setValue =new ModelWeightInfo();
                //setValue.unitWeight = Decimal.Parse(item.UnitWeight);
                //setValue.editor = item.Editor;
                //setValue.udt = DateTime.Now;

                //ModelWeightInfo condition =new ModelWeightInfo();
                //condition.model = item.Model;

                PakSkuMasterWeightFisInfo pakSkuMasterWeight = new PakSkuMasterWeightFisInfo();
                pakSkuMasterWeight.model = item.Model;
                //pakSkuMasterWeight.weight = setValue.unitWeight;               
                //pakSkuMasterWeight.cdt = setValue.udt;
                pakSkuMasterWeight.weight = inputUnitweight;
                pakSkuMasterWeight.cdt = currentModelWeight.Udt; 

                UnitOfWork uow = new UnitOfWork();
                //itemRepositoryModelWeight.UpdateModelWeightDefered(uow,setValue,condition);
                itemRepositoryPizza.DeletetPakSkuMasterWeightFisByModelDefered(uow, item.Model);
                itemRepositoryPizza.InsertPakSkuMasterWeightFisDefered(uow, pakSkuMasterWeight);
                if (isAdd)
                {
                    currentModelWeightRepository.Add(currentModelWeight, uow);
                }
                else
                {
                    currentModelWeightRepository.Update(currentModelWeight, uow);
                }
                
                
                uow.Commit();

            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }
        public void SaveModelWeightItemAndHold(ModelWeightDef item, IList<string> lstPrdID, string holdStation, string defectCode)
        {
            try
            {

                IModelWeightRepository itemRepositoryModelWeight = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();
                //看取得的数据是否有, 防止update的是空记录，但[PAK_SkuMasterWeight_FIS]中加入了记录
                DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(item.Model);
                //if (modelWeight == null || modelWeight.Rows.Count == 0)
                //{
                //    //该Model尚无标准重量，请先去称重。
                //    List<string> erpara = new List<string>();
                //    erpara.Add(item.Model);
                //    FisException ex;
                //    ex = new FisException("PAK123", erpara);
                //    throw ex;
                //}
                if (modelWeight == null || modelWeight.Rows.Count == 0)
                {
                    item.UnitWeight = "0.00";
                }
                else
                {
                    item.UnitWeight = modelWeight.Rows[0][1].ToString();
                }

                IPizzaRepository itemRepositoryPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository>();

                ModelWeightInfo setValue = new ModelWeightInfo();
                setValue.unitWeight = Decimal.Parse(item.UnitWeight);
                setValue.sendStatus = "";
                setValue.remark = "";
                setValue.editor = item.Editor;

                setValue.udt = DateTime.Now;

                ModelWeightInfo condition = new ModelWeightInfo();
                condition.model = item.Model;

                PakSkuMasterWeightFisInfo pakSkuMasterWeight = new PakSkuMasterWeightFisInfo();
                pakSkuMasterWeight.model = item.Model;
                pakSkuMasterWeight.weight = setValue.unitWeight;                
                pakSkuMasterWeight.cdt = setValue.udt;

                UnitOfWork uow = new UnitOfWork();
                itemRepositoryModelWeight.UpdateModelWeightDefered(uow, setValue, condition);
                itemRepositoryPizza.DeletetPakSkuMasterWeightFisByModelDefered(uow, item.Model);
                itemRepositoryPizza.InsertPakSkuMasterWeightFisDefered(uow, pakSkuMasterWeight);
                HoldProductByProductList(lstPrdID, item, uow,holdStation);
             
             //void WriteHoldCodeDefered(IUnitOfWork uow, IList<string> productIDList, TestLog log, IList<string> defectList);
                TestLog testLog = new TestLog(0, "", "", "", "HOLD", TestLog.TestLogStatus.Fail, "", "HOLD", "", "", item.Editor, "HOLD", DateTime.Now);
                IProductRepository prodyctRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IList<string> defectLst = new List<string>();
                defectLst.Add(defectCode);
                prodyctRep.WriteHoldCodeDefered(uow, lstPrdID, testLog, defectLst);
                uow.Commit();

            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        
        }
        public void ReleaseHold(IList<string> lstPrdID,IList<int> lstHoldID, string defectCode,string editor)
        {
        
              IUnitOfWork uow=new UnitOfWork();
              IProductRepository prodyctRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
              IList<TbProductStatus> lstP=prodyctRep.GetProductStatus(lstPrdID);
              prodyctRep.UpdateStationToPreStationDefered(uow, lstPrdID, editor);
              prodyctRep.WriteProdLogByPreStationDefered(uow, lstPrdID, editor);
              prodyctRep.UpdateProductPreStationDefered(uow,lstP);

              IList<HoldInfo> lstHoldInfo = new List<HoldInfo>();
              foreach (int id in lstHoldID)
              {
                  HoldInfo h = new HoldInfo();
                  h.HoldID = id;
                  lstHoldInfo.Add(h);
              }

              prodyctRep.ReleaseHoldProductIDDefered(uow, lstHoldInfo, defectCode, editor);
              uow.Commit();
        
        }
        private void HoldProductByProductList(IList<string> lstPrdID, ModelWeightDef item, IUnitOfWork uow, string holdStation)
        {
            try
            {
                IProductRepository prodyctRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                prodyctRep.UpdateProductPreStationDefered(uow, lstPrdID);
                prodyctRep.UpdateProductStatusDefered(uow, lstPrdID, "", holdStation, 0, 0, item.Editor);
                prodyctRep.WriteProductLogDefered(uow, lstPrdID, "", holdStation, 0, item.Editor);
          
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        
         
        }


        public IList<string> GetPrIDListInHoldByModel(string model)
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> stationList = 
                partRep.GetConstValueTypeList("ChangeModelWeightStation").Where(x => x.value.Trim() != "").Select(x=>x.value).ToList();
            if (stationList.Count == 0)
            {
               throw new FisException("IDL002", new string[] { "ChangeModelWeightStation" });
            }
            //    IList<string> GetModelByStation(IList<string> stationList);
            IProductRepository prodyctRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string> lst = prodyctRep.GetProductIDByModelStation(model,stationList);
            return lst;

         
        }
        public IList<HoldInfo> GetHoldProductList(string model)
        {
            try
            {
                IProductRepository prodyctRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IList<string> stationLst=new List<string>();
                stationLst.Add("HOLD");
                IList<HoldInfo> lstHold = prodyctRep.GetHoldInfo(model, stationLst);
                return lstHold;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
           
        
        
        
        
        }
        public List<string> GetDefectCodeList(string type)
        {
            IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();
            IList<IMES.DataModel.DefectInfo> defList = defectRepository.GetDefectList(type);
            List<string> lst = new List<string>();
            foreach (IMES.DataModel.DefectInfo d in defList)
            {
                lst.Add(d.id + "-" + d.description);
            }
            return lst;
        }

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }


    }


}

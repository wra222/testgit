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
        public String GetModelWeightByModelorCustSN(string inputData)
        {
            //检查合法model
            //看取得的数据是否有数据
            String result = "";
            try
            {
                string model = string.Empty;
                IModelRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
                

                //3)	如果刷入为10位，如果是CN打头的初步认定为Customer SN，如在Product．CustSN中不存在，则提示“非法的Customer SN”
                if (inputData.Length == 10 && inputData.Substring(0, 2) == "CN")
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
                    else model = currentProduct.Model;
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
                if (modelWeight == null || modelWeight.Rows.Count == 0)
                {
                    //This Model, there is no standard weight, please go to the weighing.
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add(model);
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;
                }

                result = modelWeight.Rows[0][1].ToString();

                if (result == "")
                {
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add(model);
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;
                }
                
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }

            return result;

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
                //看取得的数据是否有, 防止update的是空记录，但[PAK_SkuMasterWeight_FIS]中加入了记录
                DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(item.Model);
                if (modelWeight == null || modelWeight.Rows.Count == 0)
                {
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add(item.Model);
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;
                }

                IPizzaRepository itemRepositoryPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository>();

                ModelWeightInfo setValue =new ModelWeightInfo();
                setValue.unitWeight = Decimal.Parse(item.UnitWeight);
                setValue.sendStatus = "";
                setValue.remark = "";
                setValue.editor = item.Editor;
                setValue.udt = DateTime.Now;

                ModelWeightInfo condition =new ModelWeightInfo();
                condition.model = item.Model;

                PakSkuMasterWeightFisInfo pakSkuMasterWeight = new PakSkuMasterWeightFisInfo();
                pakSkuMasterWeight.model = item.Model;
                pakSkuMasterWeight.weight = setValue.unitWeight;
                pakSkuMasterWeight.cdt = setValue.udt;

                UnitOfWork uow = new UnitOfWork();
                itemRepositoryModelWeight.UpdateModelWeightDefered(uow,setValue,condition);
                itemRepositoryPizza.DeletetPakSkuMasterWeightFisByModelDefered(uow, item.Model);
                itemRepositoryPizza.InsertPakSkuMasterWeightFisDefered(uow, pakSkuMasterWeight);
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

using System;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.FisObject.PAK.StandardWeight;
using System.Data;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure.UnitOfWork;


namespace IMES.Maintain.Implementation
{

    public class ModelWeightManager:MarshalByRefObject, IModelWeight
    {

        public String GetUnitWeightByModel(string model)
        {
            //检查合法model
            //看取得的数据是否有数据
            String result = "";
            try
            {

                IModelRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();

                Model modelItem=itemRepository.Find(model);

                if (modelItem == null)
                {
                    //This model does not exist
                    //不存在这个Model
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT148", erpara);
                    throw ex;

                }

                IModelWeightRepository itemRepositoryModelWeight = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();

                DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(model);
                if (modelWeight == null || modelWeight.Rows.Count == 0)
                {
                    //This Model, there is no standard weight, please go to the weighing.
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT149", erpara);
                    throw ex;
                }

                result = modelWeight.Rows[0][1].ToString();

                if (result == "")
                {
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT149", erpara);
                    throw ex;
                }
                
            }
            catch (Exception)
            {
                throw;
            }

            return result;

        }

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
                    FisException ex;
                    ex = new FisException("DMT149", erpara);
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
            catch (Exception)
            {
                throw;
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

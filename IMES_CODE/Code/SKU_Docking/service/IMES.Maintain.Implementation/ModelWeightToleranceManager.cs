using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.FisObject.PAK.StandardWeight;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Maintain.Interface;
using IMES.Maintain.Interface.MaintainIntf;

namespace IMES.Maintain.Implementation
{
    public class ModelWeightToleranceManager : MarshalByRefObject, IModelWeightTolerance 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IModelToleranceRepository modelToleranceRepository = RepositoryFactory.GetInstance().GetRepository<IModelToleranceRepository, ModelTolerance>();

        //取得ModelWeightTolerance列表，family为空时取得Model==customer的纪录
        public IList<ModelWeightTolerance> GetModelWeightToleranceList(string customer, string family)
        {
            IList<ModelWeightTolerance> modelWeightToleranceList = new List<ModelWeightTolerance>();
            if(family == null || family == "")
            {
                ModelTolerance modelTolerance = modelToleranceRepository.FindModelWeightToleranceByCustomer(customer);
                if (modelTolerance != null)
                {
                    ModelWeightTolerance modelWeightTolerance = new ModelWeightTolerance();
                    modelWeightTolerance.Model = modelTolerance.Model;
                    modelWeightTolerance.Udt = modelTolerance.Udt;
                    modelWeightTolerance.UnitTolerance = modelTolerance.UnitTolerance;
                    modelWeightTolerance.CartonTolerance = modelTolerance.CartonTolerance;
                    modelWeightTolerance.Cdt = modelTolerance.Cdt;
                    modelWeightTolerance.Editor = modelTolerance.Editor;
                    modelWeightToleranceList.Add(modelWeightTolerance);
                }
            }
            else
            {
                IList<ModelTolerance> modelToleraceList = modelToleranceRepository.FindModelWeightToleranceListByFamily(family);
                foreach (ModelTolerance modelTolerance in modelToleraceList)
                {
                    ModelWeightTolerance modelWeightTolerance = new ModelWeightTolerance();
                    modelWeightTolerance.Model = modelTolerance.Model;
                    modelWeightTolerance.Udt = modelTolerance.Udt;
                    modelWeightTolerance.UnitTolerance = modelTolerance.UnitTolerance;
                    modelWeightTolerance.CartonTolerance = modelTolerance.CartonTolerance;
                    modelWeightTolerance.Cdt = modelTolerance.Cdt;
                    modelWeightTolerance.Editor = modelTolerance.Editor;
                    modelWeightToleranceList.Add(modelWeightTolerance);
                }
            }
            return modelWeightToleranceList;
        }

        /// 添加新纪录
        public void AddModelWeightTolerance(ModelWeightTolerance modelWeightTolerance)
        {
            ModelTolerance modelTolerance = new ModelTolerance();
            modelTolerance.Model = modelWeightTolerance.Model;
            modelTolerance.Udt = modelWeightTolerance.Udt;
            modelTolerance.UnitTolerance = modelWeightTolerance.UnitTolerance;
            modelTolerance.CartonTolerance = modelWeightTolerance.CartonTolerance;
            modelTolerance.Cdt = modelWeightTolerance.Cdt;
            modelTolerance.Editor = modelWeightTolerance.Editor;
            modelToleranceRepository.AddModelWeightTolerance(modelTolerance);
        }

        /// 更新纪录
        public void UpdateModelWeightTolerance(ModelWeightTolerance modelWeightTolerance,string model)
        {
            ModelTolerance modelTolerance = new ModelTolerance();
            modelTolerance.Model = modelWeightTolerance.Model;
            modelTolerance.Udt = new DateTime();
            modelTolerance.UnitTolerance = modelWeightTolerance.UnitTolerance;
            modelTolerance.CartonTolerance = modelWeightTolerance.CartonTolerance;
            modelTolerance.Cdt = modelWeightTolerance.Cdt;
            modelTolerance.Editor = modelWeightTolerance.Editor;
            modelToleranceRepository.UpdateModelWeightTolerance(modelTolerance,model);
        }

        /// 删除纪录
        public void DeleteModelWeightTolerance(string model)
        {
            modelToleranceRepository.DeleteModelWeightToleranceByModel(model);
        }

        public bool IFModelIsExists(string model)
        {
            ModelTolerance modelTolerance = modelToleranceRepository.FindModelWeightToleranceByCustomer(model);
            if (modelTolerance == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

﻿/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  ICT input interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-1-20  liu xiaoling          Create 
 * Known issues:
 * qa bug no:ITC-1136-0120
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Maintain.Interface.MaintainIntf;
using log4net;
using IMES.FisObject.Common.Process;
using IMES.FisObject.Common.Part;
namespace IMES.Maintain.Implementation
{

    class ModelManager :MarshalByRefObject, IModelManager
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
        public IProcessRepository processRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
        public IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

        #region IModel 成员

        //model
        public IList<ModelMaintainInfo> GetModelList(string FamilyId)
        {
            IList<ModelMaintainInfo> modelList = new List<ModelMaintainInfo>();
            try
            {
                IList<Model> tmpModelList = modelRepository.GetModelObjList(FamilyId);

                foreach (Model temp in tmpModelList)
                {
                    ModelMaintainInfo model = new ModelMaintainInfo();

                    model = convertToModelMaintainInfoFromModelObj(temp);

                    modelList.Add(model);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

            return modelList;
        }

        //for liu jing-ke 2010/4/7 return null, i dont use it
        public ModelMaintainInfo GetModel(string strModelName)
        {
            ModelMaintainInfo model = new ModelMaintainInfo();

            try
            {
                Model modelObj = modelRepository.Find(strModelName);

                if (modelObj == null)
                {
                    return null;
                }
                else
                {
                    model = convertToModelMaintainInfoFromModelObj(modelObj);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

            return model;
        }

        public void AddModel(ModelMaintainInfo modelInfo)
        {
            try
            {
                Model modelObj = convertToModelObjFromModelMaintainInfo(modelInfo);

                IUnitOfWork work = new UnitOfWork();

                modelRepository.Add(modelObj, work);

                work.Commit();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;//new SystemException(e.Message);
            }

        }

        public void SaveModel(ModelMaintainInfo modelInfo, string oldModelName)
        {
            try
            {
                Model modelObj = convertToModelObjFromModelMaintainInfo(modelInfo);

                IUnitOfWork work = new UnitOfWork();

                modelRepository.ChangeModelDefered(work, modelObj, oldModelName);

                //删除model_process表
                processRepository.DeleteModelProcessByModelDefered(work, oldModelName);

                work.Commit();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

        }

        public void DeleteModel(string strModelName)
        {
            try
            {
                Model model = modelRepository.Find(strModelName);

                IUnitOfWork work = new UnitOfWork();

                modelRepository.Remove(model, work);

                //删除model_process表
                processRepository.DeleteModelProcessByModelDefered(work, strModelName);

                //删除processRule的信息
                IList<int> lstID = processRepository.GetProcessRuleIDByModel(strModelName);
                foreach (int temp in lstID)
                {
                    processRepository.DeleteProcessRuleByIDDefered(work, temp);
                }

                work.Commit();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        public void SaveModelPrice(ModelMaintainInfo modelInfo)
        {
            try
            {
                Model model = modelRepository.Find(modelInfo.Model);
                model.Price = modelInfo.Price;
                model.Editor = modelInfo.Editor;
                
                IUnitOfWork work = new UnitOfWork();

                modelRepository.ChangeModelDefered(work, model, modelInfo.Model);

                work.Commit();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

        }


        private ModelMaintainInfo convertToModelMaintainInfoFromModelObj(Model temp)
        {
            ModelMaintainInfo model = new ModelMaintainInfo();

            model.Model = temp.ModelName;
            model.CustPN = temp.CustPN;
            model.Region = temp.Region;
            model.ShipType = temp.ShipType;
            model.Status = temp.Status;
            model.OsCode = temp.OSCode;
            model.OSDesc = temp.OSDesc;
            model.Price = temp.Price;
            model.BomApproveDate = temp.BOMApproveDate;
            model.Editor = temp.Editor;
            model.Cdt = temp.Cdt;
            model.Udt = temp.Udt;
            model.Family = temp.FamilyName;

            return model;
        }

        private Model convertToModelObjFromModelMaintainInfo(ModelMaintainInfo temp)
        {
            Model model = new Model();

            model.ModelName = temp.Model;
            model.CustPN = temp.CustPN;
            model.Region = temp.Region;
            model.ShipType = temp.ShipType;
            model.Status = temp.Status;
            model.OSCode = temp.OsCode;
            model.OSDesc = temp.OSDesc;
            model.Price = temp.Price;
            model.BOMApproveDate = temp.BomApproveDate;
            model.Editor = temp.Editor;
            model.Cdt = temp.Cdt;
            model.Udt = temp.Udt;
            model.FamilyName = temp.Family;

            return model;
        }


        //modelinfo
        public IList<ModelInfoNameAndModelInfoValueMaintainInfo> GetModelInfoList(string strModelName)
        {
            IList<ModelInfoNameAndModelInfoValueMaintainInfo> modelInfoList = new List<ModelInfoNameAndModelInfoValueMaintainInfo>();

            try
            {
                IList<ModelInfoNameAndModelInfoValue> tmpModelObj = modelRepository.GetModelInfoNameAndModelInfoValueListByModel(strModelName);


                foreach (ModelInfoNameAndModelInfoValue temp in tmpModelObj)
                {
                    ModelInfoNameAndModelInfoValueMaintainInfo modelinfo = new ModelInfoNameAndModelInfoValueMaintainInfo();

                    modelinfo = convertToMaintainInfoFromObj(temp);

                    modelInfoList.Add(modelinfo);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

            return modelInfoList;
        }

        public ModelInfoMaintainInfo GetModelInfo(string strModelInfoId)
        {
            ModelInfoMaintainInfo modelInfo = new ModelInfoMaintainInfo();
            try
            {
                IMES.FisObject.Common.Model.ModelInfo modelInfoObj = modelRepository.GetModelInfoById(Int32.Parse(strModelInfoId));
                modelInfo = convertToModelInfoMaintainInfoFromModelInfoObj(modelInfoObj);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

            return modelInfo;
        }

        public long AddModelInfo(ModelInfoMaintainInfo Object)
        {
            SystemException ex;
            List<string> paraError = new List<string>();
            try 
            {

                IUnitOfWork work = new UnitOfWork();

                IMES.FisObject.Common.Model.ModelInfo objModelInfo = convertToModelInfoObjFromModelInfoMaintainInfo(Object);
                
                modelRepository.AddModelInfoDefered(work, objModelInfo);
                work.Commit();

                return objModelInfo.ID;

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }


        }

        public void SaveModelInfo(ModelInfoMaintainInfo Object)
        {
            SystemException ex;
            List<string> paraError = new List<string>();
            int modelInfoId;
            try
            {
                IUnitOfWork work = new UnitOfWork();

                IMES.FisObject.Common.Model.ModelInfo objModelInfo = convertToModelInfoObjFromModelInfoMaintainInfo(Object);
                
                modelRepository.SaveModelInfoDefered(work, objModelInfo);
                work.Commit();

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

        }

        public void DeleteModelInfo(ModelInfoMaintainInfo Object)
        {
            try 
            {
                IMES.FisObject.Common.Model.ModelInfo objModelInfo = convertToModelInfoObjFromModelInfoMaintainInfo(Object);
                IUnitOfWork work = new UnitOfWork();

                modelRepository.DeleteModelInfoDefered(work, objModelInfo);
                work.Commit();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        private ModelInfoNameAndModelInfoValueMaintainInfo convertToMaintainInfoFromObj(ModelInfoNameAndModelInfoValue temp)
        {
            ModelInfoNameAndModelInfoValueMaintainInfo modelInfo = new ModelInfoNameAndModelInfoValueMaintainInfo();

            modelInfo.Name = temp.Name;
            modelInfo.Value = temp.Value;
            modelInfo.Editor = temp.Editor;
            modelInfo.Cdt = temp.Cdt;
            modelInfo.Udt = temp.Udt;
            modelInfo.Id = temp.ID;
            modelInfo.Description = temp.Description;

            return modelInfo;
        }


        private ModelInfoMaintainInfo convertToModelInfoMaintainInfoFromModelInfoObj(IMES.FisObject.Common.Model.ModelInfo temp)
        {
            ModelInfoMaintainInfo modelInfo = new ModelInfoMaintainInfo();

            modelInfo.Name = temp.Name;
            modelInfo.Value = temp.Value;
            modelInfo.Editor = temp.Editor;
            modelInfo.Cdt = temp.Cdt;
            modelInfo.Udt = temp.Udt;
            modelInfo.Id = temp.ID;
            modelInfo.Model = temp.ModelName;

            return modelInfo;
        }


        private IMES.FisObject.Common.Model.ModelInfo convertToModelInfoObjFromModelInfoMaintainInfo(ModelInfoMaintainInfo temp)
        {
            IMES.FisObject.Common.Model.ModelInfo modelinfo = new IMES.FisObject.Common.Model.ModelInfo();

            modelinfo.Name = temp.Name;
            modelinfo.Value = temp.Value;
            modelinfo.ID = temp.Id;
            modelinfo.Editor = temp.Editor;
            modelinfo.Cdt = temp.Cdt;
            modelinfo.Udt = temp.Udt;
            modelinfo.ModelName = temp.Model;

            return modelinfo;
        }
        #endregion

        public IList<RegionInfo> GetRegionList()
        {
            IList<RegionInfo> regionList = new List<RegionInfo>();
            IList<Region> fisObjectList = partRepository.GetRegionList();
            if (fisObjectList != null)
            {
                foreach (Region temp in fisObjectList)
                {
                    RegionInfo region = new RegionInfo();
                    region.Region = temp.region;
                    region.Description = temp.Description;
                    region.Editor = temp.Editor;
                    region.Cdt = temp.Cdt;
                    region.Udt = temp.Udt;
                    regionList.Add(region);
                }
            }
            return regionList;
        }

        public IList<ConstValueInfo> GetConstValueListByType(String type)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                //2012-8-13
                //IList<ConstValueInfo> retLst = itemRepository.GetConstValueListByType(type);
                ConstValueInfo temp = new ConstValueInfo();
                temp.type = type;
                ConstValueInfo temp2 = new ConstValueInfo();
                temp2.name = "";
                IList<ConstValueInfo> retLst = itemRepository.GetConstValueListByType(temp, temp2);

                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }





        #region IModelManager 成员

        private ShipTypeMaintainInfo convertToMaintainInfoFromObj(ShipType temp)
        {
            ShipTypeMaintainInfo shipType = new ShipTypeMaintainInfo();

            shipType.ShipType = temp.shipType;
            shipType.Editor = temp.Editor;
            shipType.Cdt = temp.Cdt;
            shipType.Udt = temp.Udt;
            shipType.Description = temp.Description;

            return shipType;
        }

        private ModelInfoNameMaintainInfo convertToMaintainInfoFromObj(ModelInfoName temp)
        {
            ModelInfoNameMaintainInfo modelInfoName = new ModelInfoNameMaintainInfo();

            modelInfoName.Name = temp.Name;
            modelInfoName.Region = temp.Region;
            modelInfoName.Editor = temp.Editor;
            modelInfoName.Cdt = temp.Cdt;
            modelInfoName.Udt = temp.Udt;
            modelInfoName.Descr = temp.Description;
            modelInfoName.Id = temp.ID;

            return modelInfoName;
        }

        private ModelInfoName convertToObjFromMaintainInfo(ModelInfoNameMaintainInfo temp)
        {
            ModelInfoName modelinfo = new ModelInfoName();

            modelinfo.Name = temp.Name;
            modelinfo.Region = temp.Region;
            modelinfo.ID = temp.Id;
            modelinfo.Editor = temp.Editor;
            modelinfo.Cdt = temp.Cdt;
            modelinfo.Udt = temp.Udt;
            modelinfo.Description = temp.Descr;

            return modelinfo;
        }

        public string GetFamilyNameByModel(string strModelName)
        {

            FisException ex;
            List<string> paraError = new List<string>();

            try
            {
                Model tmpModel = modelRepository.Find(strModelName.ToUpper());

                if (tmpModel == null)
                {
                    ex = new FisException("DMT010", paraError);//new SystemException("There is the same Rule!");//
                    throw ex;
                }
                else
                {

                    return tmpModel.FamilyName;
                }

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        public IList<ModelMaintainInfo> GetModelListByModel(string strFamilyName, string strModelName)
        {
            FisException ex;
            List<string> paraError = new List<string>();

            IList<ModelMaintainInfo> modelList = new List<ModelMaintainInfo>();

            try
            {
                IList<Model> tmpModelList = modelRepository.GetModelListByModel(strFamilyName, strModelName.ToUpper());

                if (tmpModelList.Count() == 0) 
                {
                    ex = new FisException("DMT010", paraError);//new SystemException("There is the same Rule!");//
                    throw ex;
                }

                foreach (Model temp in tmpModelList)
                {
                    ModelMaintainInfo model = new ModelMaintainInfo();

                    model = convertToModelMaintainInfoFromModelObj(temp);

                    modelList.Add(model);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

            return modelList;
        }

        public IList<ShipTypeMaintainInfo> GetShipTypeList()
        {
            IList<ShipTypeMaintainInfo> shipTypeList = new List<ShipTypeMaintainInfo>();
            try
            {
                IList<ShipType> tmpShipTypeList = modelRepository.GetShipTypeList();

                foreach (ShipType temp in tmpShipTypeList)
                {
                    ShipTypeMaintainInfo shipType = new ShipTypeMaintainInfo();

                    shipType = convertToMaintainInfoFromObj(temp);

                    shipTypeList.Add(shipType);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

            return shipTypeList;
        }

        public IList<ModelInfoNameMaintainInfo> GetModelInfoNameList()
        {
            IList<ModelInfoNameMaintainInfo> modelInfoNameList = new List<ModelInfoNameMaintainInfo>();
            try
            {
                IList<ModelInfoName> tmpModelInfoNameList = modelRepository.GetModelInfoNameList();

                foreach (ModelInfoName temp in tmpModelInfoNameList)
                {
                    ModelInfoNameMaintainInfo modelInfoName = new ModelInfoNameMaintainInfo();

                    modelInfoName = convertToMaintainInfoFromObj(temp);

                    modelInfoNameList.Add(modelInfoName);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

            return modelInfoNameList;
        }

        public int AddModelInfoName(ModelInfoNameMaintainInfo Object)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                int count;
                //count = modelRepository.CheckExistedModelInfoName(Object.Region, Object.Name, "");
                count = modelRepository.CheckExistedModelInfoName(Object.Region, Object.Name, "");

                //取得ModelInfoName中等于region和modelInfoName的记录个数。
                if (count > 0)
                {
                    //paraError.Add(Object.PartNo);
                    //paraError.Add(Object.AssemblyCode);
                    //ex = new FisException("CHK020", paraError);
                    ex = new FisException("DMT011", paraError);//new SystemException("There is the same Rule!");//
                    throw ex;
                }
                else
                {
                    ModelInfoName objModelInfoName = convertToObjFromMaintainInfo(Object);

                    IUnitOfWork work = new UnitOfWork();

                    modelRepository.AddModelInfoNameDefered(work, objModelInfoName);

                    work.Commit();
                    return objModelInfoName.ID;
                }

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

        }

        public void SaveModelInfoName(ModelInfoNameMaintainInfo Object)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                int count;
                //count = modelRepository.CheckExistedModelInfoName(Object.Region, Object.Name, Object.Id.ToString());
                count = modelRepository.CheckExistedModelInfoName(Object.Region, Object.Name, Object.Id.ToString());

                //取得ModelInfoName中等于region和modelInfoName的记录个数。
                if (count > 0)
                {
                    //paraError.Add(Object.PartNo);
                    //paraError.Add(Object.AssemblyCode);
                    //ex = new FisException("CHK020", paraError);
                    ex = new FisException("DMT011", paraError);//new SystemException("There is the same Rule!");//
                    throw ex;
                }
                else
                {
                    ModelInfoName objModelInfoName = convertToObjFromMaintainInfo(Object);

                    IUnitOfWork work = new UnitOfWork();

                    modelRepository.SaveModelInfoNameDefered(work, objModelInfoName);

                    work.Commit();
                }

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        public void DeleteModelInfoName(ModelInfoNameMaintainInfo Object)
        {
            try
            {
                ModelInfoName modelInfoName = convertToObjFromMaintainInfo(Object);

                IUnitOfWork work = new UnitOfWork();
                modelRepository.DeleteModelInfoNameDefered(work, modelInfoName);
                work.Commit();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

        }

        #endregion
    }
}
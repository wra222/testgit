﻿/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  ICT input interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-1-20  liu xiaoling          Create 
 * Known issues:
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
using IMES.FisObject.Common.FisBOM;
using System.Data.SqlClient;
using System.Data;
namespace IMES.Maintain.Implementation
{

    class ModelManagerEx : MarshalByRefObject, IModelManagerEx
    {
        public IModelRepositoryEx modelRepositoryEx = RepositoryFactory.GetInstance().GetRepository<IModelRepositoryEx>();
        public IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<ModelMaintainInfo> GetModelListByPartialModelNo(string modelNo, int rowCount)
        {
            IList<ModelMaintainInfo> modelList = null;

            try
            {
                modelList = modelRepositoryEx.GetModelListByPartialModelNo(modelNo, rowCount);
                if (modelList.Count != 0)
                {
                    return modelList;
                }
                else
                {
                    return null;
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

        public IList<ModelInfoNameAndModelInfoValueMaintainInfo> GetModelInfoNameAndModelInfoValueListByModels(IList<string> models)
        {
            IList<ModelInfoNameAndModelInfoValueMaintainInfo> ret = null;

            try
            {
                ret = modelRepositoryEx.GetModelInfoNameAndModelInfoValueListByModels(models);
                return ret;
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

        public void DeleteModelsInfo(string infoName, IList<string> models)
        {
            try
            {
                modelRepositoryEx.DeleteModelsInfo(infoName, models);
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

        public void UpdateModelsInfo(string infoName, string infoValue, IList<string> models)
        {
            try
            {
                modelRepositoryEx.UpdateModelsInfo(infoName, infoValue, models);
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

        public IList<string> GetExistedModelsFromModelInfoByModels(string infoName, IList<string> models, ref IList<string> notExistedModels)
        {
            IList<string> ret = null;

            try
            {
                ret = modelRepositoryEx.GetExistedModelsFromModelInfoByModels(infoName, models);
                for (int i = 0; i < models.Count; i++)
                {
                    if (!ret.Contains(models[i]))
                    {
                        notExistedModels.Add(models[i]);
                    }
                }
                return ret;
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

        public bool ModelExist(string modelId)
        {
            if(null == modelRepository.FindFromDB(modelId))
                return false;
            return true;
        }

        public void DeleteModel(string model, string editor)
        {
            try
            {
                logger.Info("DeleteModel model=" + model + ", editor=" + editor);
                modelRepositoryEx.DeleteModelEx(model);
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

        public IList<string> GetModelsFromProduct(string modelNo, int rowCount)
        {
            IList<string> lst = null;

            try
            {
                lst = modelRepositoryEx.GetModelsFromProduct(modelNo, rowCount);
                return lst;
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

        public IList<string> GetCustomerByFamily(string family)
        {
            IList<string> lst = new List<string>();

            try
            {
                string sqlStr = @"select distinct CustomerID from Family where Family = @family ";

                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@family", SqlDbType.VarChar);
                paramsArray[0].Value = family;

                DataTable dt = IMES.Infrastructure.Repository._Schema.SqlHelper.ExecuteDataFill(IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString_BOM,
                                                                                    CommandType.Text,
                                                                                    sqlStr,
                                                                                    paramsArray);
                if (dt.Rows.Count == 0)
                {
                    return lst;
                }
                string familyValue = dt.Rows[0]["CustomerID"].ToString().Trim();
                lst.Add(familyValue);
                return lst;
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

    }

}

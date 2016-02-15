﻿/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:Service for RCTOWeight Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI RCTO Weight
 * UC:CI-MES12-SPEC-PAK-UC RCTO Weight
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-09-08  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using IMES.FisObject.Common.Part;
using System.Linq;

namespace IMES.Station.Implementation
{
    /// <summary>
    ///  
    /// </summary>
    public class _RCTOWeight : MarshalByRefObject, IRCTOWeight
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IModelWeightRepository mvRepository = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository, IMES.FisObject.PAK.StandardWeight.ModelWeight>();
        private IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();

        #region RCTOWeight

        /// <summary>
        /// 获取Model的标准重量
        /// </summary>
        public decimal GetModelWeight(string model)
        {
            logger.Debug("(RCTOWeight)GetModelWeight start, Model:" + model);

            try
            {
                if (modelRepository.Find(model) == null)
                {
                    throw new FisException("CHK804", new string[] { model });
                }

                IMES.FisObject.PAK.StandardWeight.ModelWeight currentModelWeight = mvRepository.Find(model);

                if (currentModelWeight == null) return -1;
                else return currentModelWeight.UnitWeight;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(RCTOWeight)GetModelWeight end, Model:" + model);
            }
        }

        /// <summary>
        /// 更新Model的标准重量
        /// </summary>
        public void SetModelWeight(string model, decimal weight, string editor)
        {
            logger.Debug("(RCTOWeight)SetModelWeight start, Model:" + model);

            try
            {
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                bool need1000 = partRep.GetConstValueTypeList("RCTOModelWeightUnit").Where(x => x.value == model.Trim()).Any();
                if (need1000)
                {
                    weight = Math.Round(weight * 1000, 2);
                }
                weight = Math.Round(weight, 2);
                IMES.FisObject.PAK.StandardWeight.ModelWeight mw=mvRepository.Find(model);
                //if (mvRepository.Find(model) != null)
                if (mw!=null)
                {
                    ModelWeightInfo cond = new ModelWeightInfo();
                    ModelWeightInfo item = new ModelWeightInfo();

                    cond.model = model;

                    item.model = model;
                    item.unitWeight = weight;
                    item.editor = editor;
                    item.remark = mw.UnitWeight.ToString();
                    item.sendStatus = "";

                    mvRepository.UpdateModelWeight(item, cond);
                }
                else
                {
                    IMES.FisObject.PAK.StandardWeight.ModelWeight item = new IMES.FisObject.PAK.StandardWeight.ModelWeight();
                    item.Model = model;
                    item.SendStatus = "";
                    item.Remark = "New";
                    item.UnitWeight = weight;
                    item.CartonWeight = 0;
                    item.Editor = editor;
                    UnitOfWork uow = new UnitOfWork();
                    mvRepository.Add(item, uow);
                    uow.Commit();
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(RCTOWeight)SetModelWeight end, Model:" + model);
            }
        }
        #endregion
    }
}

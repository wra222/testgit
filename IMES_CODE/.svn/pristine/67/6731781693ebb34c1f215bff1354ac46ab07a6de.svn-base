/*
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
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.PrintItem;
using IMES.FisObject.PAK.DN;
using System.Configuration;
using IMES.FisObject.Common.Misc;

namespace IMES.Maintain.Implementation
{

    class LabelSettingManager : MarshalByRefObject, ILabelSettingManager
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ILabelTypeRepository labelTypeRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository>();
        public IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
        public IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
        public IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        public IMiscRepository miscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();

        public static readonly string DB_GetData = ConfigurationManager.AppSettings["DB_GetData"].ToString();
        public static readonly string DB_PAK = ConfigurationManager.AppSettings["DB_PAK"].ToString();









        #region ILabelSettingManager 成员

        public IList<LabelTypeMaintainInfo> getLableTypeList()
        {
            IList<LabelTypeMaintainInfo> labelTypeList = new List<LabelTypeMaintainInfo>();
            try
            {
                IList<LabelType> tmpLabelTypeList = labelTypeRepository.GetLabelTypeList();

                foreach (LabelType temp in tmpLabelTypeList)
                {
                    LabelTypeMaintainInfo labelType = new LabelTypeMaintainInfo();

                    labelType = convertToMaintainInfoFromObj(temp);

                    labelTypeList.Add(labelType);
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

            return labelTypeList;
        }

        public void AddLabelType(LabelTypeMaintainInfo infoLabelType)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                //检查是否已存在相同的Process
                if (labelTypeRepository.CheckExistedLabelType(infoLabelType.LabelType) > 0)
                {
                    ex = new FisException("DMT132", paraError);
                    throw ex;

                }
                else
                {
                    LabelType labelTypeObj = new LabelType();
                    labelTypeObj = convertToObjFromMaintainInfo(labelTypeObj, infoLabelType);

                    IUnitOfWork work = new UnitOfWork();

                    labelTypeRepository.AddLabelTypeDefered(work, labelTypeObj);

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

        public void SaveLabelType(LabelTypeMaintainInfo infoLabelType)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                LabelType labelTypeObj = new LabelType();
                labelTypeObj = convertToObjFromMaintainInfo(labelTypeObj, infoLabelType);

                IUnitOfWork work = new UnitOfWork();

                labelTypeRepository.SaveLabelTypeDefered(work, labelTypeObj);

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

        public void DeleteLabelType(string strLabelType)
        {
            try
            {
                LabelType labelTypeObj = labelTypeRepository.Find(strLabelType);

                //panduanlabeltype数据是否与其他数据有关联
                IList<PCodeLabelTypeMaintainInfo> pCodeLabelTypeList = new List<PCodeLabelTypeMaintainInfo>();
                IList<PrintTemplate> tmpPrintTemplateList = labelTypeRepository.GetPrintTemplateListByLabelType(strLabelType);
                if ((pCodeLabelTypeList != null && pCodeLabelTypeList.Count > 0) || (tmpPrintTemplateList != null && tmpPrintTemplateList.Count > 0))
                {
                    //此labeltype数据与其他表有关联，不能删除
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT131", erpara);
                    throw ex;
                }

                #region 删除与labeltype有关联的数据
                ////删除对应的related function
                //IList<PCodeLabelTypeMaintainInfo> pCodeLabelTypeList = new List<PCodeLabelTypeMaintainInfo>();
                //if (pCodeLabelTypeList != null && pCodeLabelTypeList.Count > 0)
                //{
                //    foreach (PCodeLabelTypeMaintainInfo temp in pCodeLabelTypeList)
                //    {
                //        IUnitOfWork work1 = new UnitOfWork();

                //        labelTypeRepository.DeletePCodeByLabelTypeDefered(work1, strLabelType);

                //        work1.Commit();
                //    }
                //}

                ////删除labeltype对应的template
                //IList<PrintTemplate> tmpPrintTemplateList = labelTypeRepository.GetPrintTemplateListByLabelType(strLabelType);
                //if(tmpPrintTemplateList!=null&&tmpPrintTemplateList.Count>0)
                //{
                //    foreach (PrintTemplate temp in tmpPrintTemplateList)
                //    {
                //        IUnitOfWork work2 = new UnitOfWork();
                //        labelTypeRepository.DeletePrintTemplateDefered(work2, temp);
                //        //删除对应的labelrule
                //        IList<LabelRule> lstLabelRule = labelTypeRepository.GetLabelRuleByTemplateName(temp.TemplateName);
                //        if (lstLabelRule != null && lstLabelRule.Count > 0)
                //        {
                //            foreach (LabelRule tempLabelRule in lstLabelRule)
                //            {
                //                UnitOfWork work3 = new UnitOfWork();
                //                labelTypeRepository.DeleteLabelRuleDefered(work3, tempLabelRule);

                //                //删除对应的labelruleset数据
                //                IList<LabelRuleSet> lstLabelRuleSet = new List<LabelRuleSet>();
                //                lstLabelRuleSet = labelTypeRepository.GetLabelRuleSetByRuleID(tempLabelRule.RuleID);
                //                if (lstLabelRuleSet != null && lstLabelRuleSet.Count > 0)
                //                {
                //                    foreach (LabelRuleSet labelRuleSetTmp in lstLabelRuleSet)
                //                    {
                //                        UnitOfWork work4 = new UnitOfWork();
                //                        labelTypeRepository.DeleteLabelRuleSetDefered(work4, labelRuleSetTmp);
                //                        work4.Commit();
                //                    }
                //                }
                //                work3.Commit();
                //            }
                //        }
                //        work2.Commit();  
                //    }     
                //}
                #endregion

                IUnitOfWork work = new UnitOfWork();
                labelTypeRepository.DeleteLabelTypeDefered(work, labelTypeObj);
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

        public IList<PrintTemplateMaintainInfo> getPrintTemplateListByLableType(string strLableType)
        {
            IList<PrintTemplateMaintainInfo> printTemplateList = new List<PrintTemplateMaintainInfo>();
            try
            {
                IList<PrintTemplate> tmpPrintTemplateList = labelTypeRepository.GetPrintTemplateListByLabelType(strLableType);

                foreach (PrintTemplate temp in tmpPrintTemplateList)
                {
                    PrintTemplateMaintainInfo printTemplate = new PrintTemplateMaintainInfo();

                    printTemplate = convertToMaintainInfoFromObj(temp);

                    printTemplateList.Add(printTemplate);
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

            return printTemplateList;
        }

        public void AddPrintTemplate(PrintTemplateMaintainInfo infoPrintTemplate)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                //检查是否已存在相同的Process
                if (labelTypeRepository.CheckExistedTemplateName(infoPrintTemplate.TemplateName) > 0)
                {
                    ex = new FisException("DMT134", paraError);
                    throw ex;
                }
                else
                {
                    PrintTemplate printTemplateObj = new PrintTemplate();
                    printTemplateObj = convertToObjFromMaintainInfo(printTemplateObj, infoPrintTemplate);
                    IUnitOfWork work = new UnitOfWork();
                    labelTypeRepository.AddPrintTemplateDefered(work, printTemplateObj);
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

        public void SavePrintTemplate(PrintTemplateMaintainInfo infoPrintTemplate)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                PrintTemplate printTemplateObj = new PrintTemplate();
                printTemplateObj = convertToObjFromMaintainInfo(printTemplateObj, infoPrintTemplate);
                IUnitOfWork work = new UnitOfWork();
                labelTypeRepository.SavePrintTemplateDefered(work, printTemplateObj);
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

        public void DeletePrintTemplate(string strTemplateName)
        {
            try
            {
                PrintTemplate printTemplateObj = labelTypeRepository.GetPrintTemplate(strTemplateName);
                IUnitOfWork work = new UnitOfWork();
                IList<LabelRuleMaintainInfo> labelRuleList = new List<LabelRuleMaintainInfo>();
                IList<LabelRule> tmpLabelRuleList = labelTypeRepository.GetLabelRuleByTemplateName(strTemplateName);
                if (tmpLabelRuleList != null && tmpLabelRuleList.Count > 0)
                {
                    //此labeltypeTmpLate数据与rule有关联，不能删除
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT135", erpara);
                    throw ex;
                }
                labelTypeRepository.DeletePrintTemplateDefered(work, printTemplateObj);
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

        public IList<PCodeLabelTypeMaintainInfo> getPCodeByLabelType(string strLabelType)
        {
            IList<PCodeLabelTypeMaintainInfo> pCodeLabelTypeList = new List<PCodeLabelTypeMaintainInfo>();
            try
            {
                IList<PCodeLabelType> tmpPCodeLabelTypeList = labelTypeRepository.GetPCodeByLabelType(strLabelType);

                foreach (PCodeLabelType temp in tmpPCodeLabelTypeList)
                {
                    PCodeLabelTypeMaintainInfo pCodeLabelType = new PCodeLabelTypeMaintainInfo();

                    pCodeLabelType = convertToMaintainInfoFromObj(temp);

                    pCodeLabelTypeList.Add(pCodeLabelType);
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

            return pCodeLabelTypeList;
        }

        public void SavePCode(IList<string> arrCheckedPCode, PCodeLabelTypeMaintainInfo PCodeLabelTypeInfo)
        {
            try
            {

                IUnitOfWork work = new UnitOfWork();
                labelTypeRepository.DeletePCodeByLabelTypeDefered(work, PCodeLabelTypeInfo.LabelType);

                for (int i = 0; i < arrCheckedPCode.Count(); i++)
                {
                    PCodeLabelType pCodeLabelTypeObj = new PCodeLabelType();


                    pCodeLabelTypeObj = convertToObjFromMaintainInfo(pCodeLabelTypeObj, PCodeLabelTypeInfo);

                    pCodeLabelTypeObj.PCode = arrCheckedPCode[i];

                    labelTypeRepository.SavePCodeDefered(work, pCodeLabelTypeObj);

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

        public IList<LabelRuleMaintainInfo> getLabelRuleByTemplateName(string strTemplateName)
        {
            IList<LabelRuleMaintainInfo> labelRuleList = new List<LabelRuleMaintainInfo>();
            try
            {
                IList<LabelRule> tmpLabelRuleList = labelTypeRepository.GetLabelRuleByTemplateName(strTemplateName);

                foreach (LabelRule temp in tmpLabelRuleList)
                {
                    LabelRuleMaintainInfo labelRule = new LabelRuleMaintainInfo();

                    labelRule = convertToMaintainInfoFromObj(temp);

                    labelRuleList.Add(labelRule);
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

            return labelRuleList;
        }

        public int AddLabelRule(LabelRuleMaintainInfo infoLabelRule)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {

                LabelRule labelRuleObj = new LabelRule();
                labelRuleObj = convertToObjFromMaintainInfo(labelRuleObj, infoLabelRule);

                IUnitOfWork work = new UnitOfWork();

                labelTypeRepository.AddLabelRuleDefered(work, labelRuleObj);

                work.Commit();
                return labelRuleObj.RuleID;

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

        public void DeleteLabelRule(int RuleID)
        {
            try
            {
                LabelRule labelRuleObj = labelTypeRepository.GetLabelRuleByRuleID(RuleID);
                IUnitOfWork work = new UnitOfWork();
                IList<LabelRuleSet> tmpLabelRuleSetList = labelTypeRepository.GetLabelRuleSetByRuleID(RuleID);
                if(tmpLabelRuleSetList!=null&&tmpLabelRuleSetList.Count>0)
                {
                    foreach (LabelRuleSet tmp in tmpLabelRuleSetList)
                    {
                        labelTypeRepository.DeleteLabelRuleSetDefered(work,tmp);
                    }
                }
                labelTypeRepository.DeleteLabelRuleDefered(work, labelRuleObj);
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

        public IList<LabelRuleSetMaintainInfo> GetLabelRuleSetByRuleID(int RuleID)
        {
            IList<LabelRuleSetMaintainInfo> labelRuleSetList = new List<LabelRuleSetMaintainInfo>();
            try
            {
                IList<LabelRuleSet> tmpLabelRuleSetList = labelTypeRepository.GetLabelRuleSetByRuleID(RuleID);

                foreach (LabelRuleSet temp in tmpLabelRuleSetList)
                {
                    LabelRuleSetMaintainInfo labelRuleSet = new LabelRuleSetMaintainInfo();

                    labelRuleSet = convertToMaintainInfoFromObj(temp);

                    labelRuleSetList.Add(labelRuleSet);
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

            return labelRuleSetList;
        }

        public IList<string> GetModelList()
        {
            IList<string> modelList = new List<string>();
            try
            {
                IList<string> tmpModelList = miscRepository.getAllFieldNameInTable(DB_GetData, "Model");

                foreach (string temp in tmpModelList)
                {
                    modelList.Add(temp);
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

        public IList<string> GetModelInfoNameList()
        {
            IList<string> modelInfoNameList = new List<string>();
            try
            {
                //IList<string> tmpModelInfoNameList = miscRepository.getAllFieldNameInTable(DB_GetData, "ModelInfo");
                IList<string> tmpModelInfoNameList = modelRepository.GetAllModelInfoName();

                foreach (string temp in tmpModelInfoNameList)
                {
                    modelInfoNameList.Add(temp);
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

        public IList<string> GetDeliveryNoList()
        {
            IList<string> deliveryList = new List<string>();
            try
            {
                IList<string> tmpDeliveryList = miscRepository.getAllFieldNameInTable(DB_PAK, "Delivery");

                foreach (string temp in tmpDeliveryList)
                {
                    deliveryList.Add(temp);
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

            return deliveryList;
        }

        public IList<string> GetDeliveryInfoTypeList()
        {
            IList<string> deliveryInfoList = new List<string>();
            try
            {
                //IList<string> tmpDeliveryInfoList = miscRepository.getAllFieldNameInTable(DB_PAK, "DeliveryInfo");
                IList<string> tmpDeliveryInfoList = deliveryRepository.GetDeliveryInfoList();

                foreach (string temp in tmpDeliveryInfoList)
                {
                    deliveryInfoList.Add(temp);
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

            return deliveryInfoList;
        }

        public IList<string> GetPartList()
        {
            IList<string> partList = new List<string>();
            try
            {
                IList<string> tmpPartList = miscRepository.getAllFieldNameInTable(DB_GetData, "Part");

                foreach (string temp in tmpPartList)
                {
                    partList.Add(temp);
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

            return partList;
        }

        public IList<string> GetPartInfoTypeList()
        {
            IList<string> partInfoList = new List<string>();
            try
            {
                //IList<string> tmpPartInfoList = miscRepository.getAllFieldNameInTable(DB_GetData, "PartInfo");
                IList<string> tmpPartInfoList = partRepository.GetPartInfoList();

                foreach (string temp in tmpPartInfoList)
                {
                    partInfoList.Add(temp);
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

            return partInfoList;
        }

        public int SaveLabelRuleSet(LabelRuleSetMaintainInfo infoLabelRuleSet)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                LabelRuleSet labelRuleSetObj = null;
                if (infoLabelRuleSet.Id != 0)
                {
                    labelRuleSetObj = labelTypeRepository.GetLabelRuleSetByID(infoLabelRuleSet.Id);
                }
                if (labelRuleSetObj == null)
                {
                    //检查是否已存在相同的LabelRuleSet
                    int count = labelTypeRepository.CheckExistedAttributeName(infoLabelRuleSet.RuleID, infoLabelRuleSet.Mode, infoLabelRuleSet.AttributeName);
                    if (count > 0)
                    {
                        ex = new FisException("DMT136", paraError);
                        throw ex;
                    }

                    labelRuleSetObj = new LabelRuleSet();
                    labelRuleSetObj = convertToObjFromMaintainInfo(labelRuleSetObj, infoLabelRuleSet);

                    IUnitOfWork work = new UnitOfWork();
                    labelTypeRepository.AddLabelRuleSetDefered(work, labelRuleSetObj);
                    work.Commit();

                }
                else
                {

                    labelRuleSetObj = convertToObjFromMaintainInfo(labelRuleSetObj, infoLabelRuleSet);

                    IUnitOfWork work = new UnitOfWork();

                    labelTypeRepository.SaveLabelRuleSetDefered(work, labelRuleSetObj);

                    work.Commit();
                }

                return labelRuleSetObj.ID;

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

        public void DeleteLabelRuleSet(int iLabelRuleSetID)
        {
            try
            {
                LabelRuleSet objLabelRuleSet = null;
                objLabelRuleSet = labelTypeRepository.GetLabelRuleSetByID(iLabelRuleSetID);
                IUnitOfWork work = new UnitOfWork();
                labelTypeRepository.DeleteLabelRuleSetDefered(work, objLabelRuleSet);
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


        private LabelTypeMaintainInfo convertToMaintainInfoFromObj(LabelType temp)
        {
            LabelTypeMaintainInfo labelType = new LabelTypeMaintainInfo();

            labelType.LabelType = temp.LblType;
            labelType.PrintMode = temp.PrintMode;
            labelType.RuleMode = temp.RuleMode;
            labelType.Description = temp.Description;
            labelType.Editor = temp.Editor;
            labelType.Cdt = temp.Cdt;
            labelType.Udt = temp.Udt;

            return labelType;
        }

        private PrintTemplateMaintainInfo convertToMaintainInfoFromObj(PrintTemplate temp)
        {
            PrintTemplateMaintainInfo printTemplate = new PrintTemplateMaintainInfo();

            printTemplate.LabelType = temp.LblType;
            printTemplate.TemplateName = temp.TemplateName;
            printTemplate.SpName = temp.SpName;
            printTemplate.Piece = temp.Piece;
            printTemplate.Layout = temp.Layout;
            printTemplate.Description = temp.Description;
            printTemplate.Editor = temp.Editor;
            printTemplate.Cdt = temp.Cdt;
            printTemplate.Udt = temp.Udt;

            return printTemplate;
        }

        private PCodeLabelTypeMaintainInfo convertToMaintainInfoFromObj(PCodeLabelType temp)
        {
            PCodeLabelTypeMaintainInfo pCodeLabelType = new PCodeLabelTypeMaintainInfo();

            pCodeLabelType.LabelType = temp.LabelType;
            pCodeLabelType.PCode = temp.PCode;

            return pCodeLabelType;
        }

        private LabelRuleMaintainInfo convertToMaintainInfoFromObj(LabelRule temp)
        {
            LabelRuleMaintainInfo labelRule = new LabelRuleMaintainInfo();

            labelRule.RuleID = temp.RuleID;
            labelRule.TemplateName = temp.TemplateName;

            return labelRule;
        }

        private LabelRuleSetMaintainInfo convertToMaintainInfoFromObj(LabelRuleSet temp)
        {
            LabelRuleSetMaintainInfo labelRuleSet = new LabelRuleSetMaintainInfo();

            labelRuleSet.Id = temp.ID;
            labelRuleSet.RuleID = temp.RuleID;
            labelRuleSet.Mode = temp.Mode;
            labelRuleSet.Editor = temp.Editor;
            labelRuleSet.Cdt = temp.Cdt;
            labelRuleSet.AttributeName = temp.AttributeName;
            labelRuleSet.AttributeValue = temp.AttributeValue;
            labelRuleSet.Udt = temp.Udt;

            return labelRuleSet;
        }




        private LabelType convertToObjFromMaintainInfo(LabelType obj, LabelTypeMaintainInfo temp)
        {
            obj.LblType = temp.LabelType;
            obj.PrintMode = temp.PrintMode;
            obj.RuleMode = temp.RuleMode;
            obj.Description = temp.Description;
            obj.Editor = temp.Editor;

            return obj;
        }

        private PrintTemplate convertToObjFromMaintainInfo(PrintTemplate obj, PrintTemplateMaintainInfo temp)
        {
            obj.TemplateName = temp.TemplateName;
            obj.LblType = temp.LabelType;
            obj.Piece = temp.Piece;
            obj.SpName = temp.SpName;
            obj.Layout = temp.Layout;
            obj.Description = temp.Description;
            obj.Editor = temp.Editor;
            return obj;
        }

        private PCodeLabelType convertToObjFromMaintainInfo(PCodeLabelType obj, PCodeLabelTypeMaintainInfo temp)
        {
            obj.LabelType = temp.LabelType;
            obj.PCode = temp.PCode;

            return obj;
        }

        private LabelRule convertToObjFromMaintainInfo(LabelRule obj, LabelRuleMaintainInfo temp)
        {
            obj.TemplateName = temp.TemplateName;

            return obj;
        }

        private LabelRuleSet convertToObjFromMaintainInfo(LabelRuleSet obj, LabelRuleSetMaintainInfo temp)
        {
            obj.ID = temp.Id;
            obj.RuleID = temp.RuleID;
            obj.AttributeName = temp.AttributeName;
            obj.AttributeValue = temp.AttributeValue;
            obj.Editor = temp.Editor;
            obj.Mode = temp.Mode;

            return obj;
        }


        #endregion
    }
}
